// ReSharper disable ConvertToPrimaryConstructor

using DIAutowire.Enums;
using JetBrains.Annotations;

namespace DIAutowire.Attributes.Method;

/// <summary>
/// Marks a method to be automatically executed at application startup.
/// </summary>
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers | ImplicitUseTargetFlags.WithInheritors)]
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AutorunAttribute : Attribute
{
    public AutorunPriority Priority { get; init; }

    public AutorunAttribute(AutorunPriority priority = AutorunPriority.Default)
    {
        Priority = priority;
    }
}