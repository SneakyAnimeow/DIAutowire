// ReSharper disable ConvertToPrimaryConstructor

using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DIAutowire.Attributes.Implementation;

/// <summary>
/// Marks a class as an implementation of an interface for DI registration.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AutowireAttribute : Attribute
{
    public string ImplementationName { get; init; }

    public ServiceLifetime ImplementationLifetime { get; init; }

    public AutowireAttribute(string implementationName="", ServiceLifetime implementationLifetime=ServiceLifetime.Scoped)
    {
        ImplementationName = implementationName;
        ImplementationLifetime = implementationLifetime;
    }

    public AutowireAttribute(ServiceLifetime implementationLifetime) : this("", implementationLifetime){}

    public AutowireAttribute() : this(ServiceLifetime.Scoped){}
}