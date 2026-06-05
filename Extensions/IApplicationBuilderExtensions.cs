// ReSharper disable InconsistentNaming

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using DIAutowire.Attributes.Method;
using DIAutowire.Attributes.Implementation;

namespace DIAutowire.Extensions;

public static class IApplicationBuilderExtensions
{
    public static async Task LaunchAutorun(this IServiceProvider provider)
    {
        var diAssembly = typeof(AutorunAttribute).Assembly.GetName().Name;
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a == typeof(AutorunAttribute).Assembly || a.GetReferencedAssemblies().Any(r => r.Name == diAssembly))
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch
                {
                    return Type.EmptyTypes;
                }
            })
            .ToList();

        var autorunMethods = types
            .SelectMany(t =>
                t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                             BindingFlags.Static))
            .Where(m => m.GetCustomAttribute<AutorunAttribute>() != null)
            .Select(m => new
            {
                Method = m,
                m.GetCustomAttribute<AutorunAttribute>()!.Priority,
                ClassType = m.DeclaringType
            });

        var allAutoruns = autorunMethods
            .OrderByDescending(x => (int)x.Priority)
            .ToList();

        using var scope = provider.CreateScope();
        var scopedProvider = scope.ServiceProvider;

        foreach (var autorun in allAutoruns)
        {
            var method = autorun.Method;
            object? instance = null;

            if (!method.IsStatic)
            {
                var type = autorun.ClassType;

                // Determine the correct provider based on the service lifetime
                var autowireAttr = type?.GetCustomAttribute<AutowireAttribute>();
                var isSingleton = autowireAttr?.ImplementationLifetime == ServiceLifetime.Singleton;
                var actualProvider = isSingleton ? provider : scopedProvider;

                // Try resolving by interface first (e.g. IClassName)
                var interfaceType = type?.GetInterface($"I{type.Name}");
                if (interfaceType is not null)
                {
                    instance = actualProvider.GetService(interfaceType);
                }

                // Fallback to resolving by class type
                if (instance is null && type is not null)
                {
                    instance = actualProvider.GetService(type);
                }

                // If we still can't resolve it, skip
                if (instance is null)
                    continue;
            }

            var result = method.Invoke(instance, null);

            if (result is not Task && result is not ValueTask)
                continue;

            await (dynamic)result;
        }
    }
}