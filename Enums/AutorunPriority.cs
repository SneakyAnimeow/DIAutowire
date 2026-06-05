using JetBrains.Annotations;

namespace DIAutowire.Enums;

/// <summary>
/// Enum representing the priority of autorun methods. Methods with higher priority will be executed before those with lower priority.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public enum AutorunPriority
{
    Low = 0,
    Default = 1,
    High = 2,
    Critical = 3
}