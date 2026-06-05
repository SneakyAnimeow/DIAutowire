// ReSharper disable InconsistentNaming

using JetBrains.Annotations;

namespace DIAutowire.Attributes.Interface;

/// <summary>
/// Marks a class or interface as a component that can be automatically registered in the dependency injection container.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false)]
public class DIComponentAttribute : Attribute;