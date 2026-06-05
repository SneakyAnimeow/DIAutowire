// ReSharper disable InconsistentNaming

using DIAutowire.Attributes.Interface;
using JetBrains.Annotations;

namespace DIAutowire.Core;

[DeclareDIComponentType("Service")]
[DeclareDIComponentType("Component")]
[DeclareDIComponentType("Repository")]
[UsedImplicitly]
public static class BuiltInDIComponentTypes;