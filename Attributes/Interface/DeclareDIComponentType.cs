// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToPrimaryConstructor

using JetBrains.Annotations;

namespace DIAutowire.Attributes.Interface;

/// <summary>
/// Generates DIComponentType Attribute with specified name.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class DeclareDIComponentTypeAttribute : Attribute
{
    public string DIComponentName { get; init; }

    public DeclareDIComponentTypeAttribute(string diComponentName)
    {
        DIComponentName = diComponentName;
    }
}