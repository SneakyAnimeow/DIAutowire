using Microsoft.Extensions.DependencyInjection;

namespace DIAutowire.Helpers;

/// <summary>
/// Helper to resolve components from DI.
/// </summary>
public class ServiceResolver<TComponent>(IServiceProvider serviceProvider)
    where TComponent : notnull
{
    /// <summary>
    /// Resolves an implementation.
    /// </summary>
    public TComponent Resolve(string? name=null)
    {
        return name == null 
            ? serviceProvider.GetRequiredService<TComponent>()
            : serviceProvider.GetRequiredKeyedService<TComponent>(name);
    }

    /// <summary>
    /// Creates a new auto disposable scope and resolves a scoped implementation for singletons.
    /// </summary>
    public ScopedResult<TComponent> ResolveScoped(string? name=null)
    {
        var scope = serviceProvider.CreateScope();
        var component = name is null
            ? scope.ServiceProvider.GetRequiredService<TComponent>()
            : scope.ServiceProvider.GetRequiredKeyedService<TComponent>(name);
        return new ScopedResult<TComponent>(component, scope);
    }
}

/// <summary>
/// A struct that implements IDisposable to release a DI scope when disposed.
/// </summary>
public readonly struct ScopedResult<T>(T component, IServiceScope scope) : IDisposable where T : notnull
{
    public T Component { get; } = component;

    public void Dispose() => scope.Dispose();
}
