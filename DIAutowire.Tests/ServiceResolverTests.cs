using Microsoft.Extensions.DependencyInjection;
using DIAutowire.Extensions;
using DIAutowire.Helpers;
using DIAutowire.Attributes.Implementation;
using DIAutowire.Attributes.Interface;

namespace DIAutowire.Tests;

[Service]
[Autowire]
public partial class ResolverTargetService
{
    public Guid Id { get; } = Guid.NewGuid();
}

[DIComponent]
[Autowire("keyed-target")]
public partial class KeyedResolverTargetService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class ServiceResolverTests
{
    [Fact]
    public void Resolve_ReturnsUnkeyedService()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var resolver = provider.GetRequiredService<ServiceResolver<IResolverTargetService>>();
        var resolved = resolver.Resolve();

        Assert.NotNull(resolved);
        Assert.NotEqual(Guid.Empty, resolved.Id);
    }

    [Fact]
    public void Resolve_ReturnsKeyedService()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var resolver = provider.GetRequiredService<ServiceResolver<IKeyedResolverTargetService>>();
        var resolved = resolver.Resolve("keyed-target");

        Assert.NotNull(resolved);
        Assert.NotEqual(Guid.Empty, resolved.Id);
    }

    [Fact]
    public void ResolveScoped_ReturnsNewScopeForUnkeyedService()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var resolver = provider.GetRequiredService<ServiceResolver<IResolverTargetService>>();
        
        using var scopedResult1 = resolver.ResolveScoped();
        using var scopedResult2 = resolver.ResolveScoped();

        Assert.NotNull(scopedResult1.Component);
        Assert.NotNull(scopedResult2.Component);
        Assert.NotEqual(scopedResult1.Component.Id, scopedResult2.Component.Id);
    }

    [Fact]
    public void ResolveScoped_ReturnsNewScopeForKeyedService()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var resolver = provider.GetRequiredService<ServiceResolver<IKeyedResolverTargetService>>();
        
        using var scopedResult1 = resolver.ResolveScoped("keyed-target");
        using var scopedResult2 = resolver.ResolveScoped("keyed-target");

        Assert.NotNull(scopedResult1.Component);
        Assert.NotNull(scopedResult2.Component);
        Assert.NotEqual(scopedResult1.Component.Id, scopedResult2.Component.Id);
    }
}
