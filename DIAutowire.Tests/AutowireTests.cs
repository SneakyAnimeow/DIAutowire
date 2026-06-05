using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using DIAutowire.Extensions;
using DIAutowire.Attributes.Implementation;
using DIAutowire.Attributes.Interface;

[assembly: DeclareDIComponentType("Adapter")]

namespace DIAutowire.Tests;

// A custom component to test assembly-level attribute generation
[Adapter]
[Autowire(ServiceLifetime.Transient)]
public partial class CustomAdapter
{
    public string Name => "TestAdapter";
}

// Classes with [DIComponent] + [Autowire] get interface generation AND DI registration
[DIComponent]
[Autowire(ServiceLifetime.Singleton)]
public partial class SingletonService
{
    public string Data { get; set; } = "SingletonData";
}

[DIComponent]
[Autowire(ServiceLifetime.Scoped)]
public partial class ScopedService
{
}

[DIComponent]
[Autowire(ServiceLifetime.Transient)]
public partial class TransientService
{
}

[DIComponent]
[Autowire("keyed-service", ServiceLifetime.Transient)]
public partial class KeyedTransientService
{
}

// Classes with ONLY [Autowire] get self-registration, no interface generation
[Autowire(ServiceLifetime.Scoped)]
public class SelfRegisteredService
{
    public string Value => "SelfRegistered";
}

[Autowire("keyed-self", ServiceLifetime.Transient)]
public class KeyedSelfRegisteredService
{
    public string Value => "KeyedSelf";
}

public class AutowireTests
{
    [Fact]
    public void DIAutowire_RegistersSingletonService()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var singleton1 = provider.GetService<ISingletonService>();
        var singleton2 = provider.GetService<ISingletonService>();

        Assert.NotNull(singleton1);
        Assert.NotNull(singleton2);
        Assert.Same(singleton1, singleton2);
        Assert.Equal("SingletonData", singleton1.Data);
    }

    [Fact]
    public void DIAutowire_RegistersScopedService()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();

        var scoped1a = scope1.ServiceProvider.GetService<IScopedService>();
        var scoped1b = scope1.ServiceProvider.GetService<IScopedService>();
        var scoped2 = scope2.ServiceProvider.GetService<IScopedService>();

        Assert.NotNull(scoped1a);
        Assert.Same(scoped1a, scoped1b);
        Assert.NotSame(scoped1a, scoped2);
    }

    [Fact]
    public void DIAutowire_RegistersTransientService()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var transient1 = provider.GetService<ITransientService>();
        var transient2 = provider.GetService<ITransientService>();

        Assert.NotNull(transient1);
        Assert.NotSame(transient1, transient2);
    }

    [Fact]
    public void DIAutowire_RegistersKeyedService()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var keyedService = provider.GetKeyedService<IKeyedTransientService>("keyed-service");
        
        Assert.NotNull(keyedService);
        Assert.Null(provider.GetService<IKeyedTransientService>());
    }

    [Fact]
    public void DIAutowire_RegistersCustomComponent()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var adapter = provider.GetService<ICustomAdapter>();
        
        Assert.NotNull(adapter);
        Assert.Equal("TestAdapter", adapter.Name);
    }

    [Fact]
    public void DIAutowire_RegistersSelfService_WhenNoComponent()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var service = provider.GetService<SelfRegisteredService>();

        Assert.NotNull(service);
        Assert.Equal("SelfRegistered", service.Value);
    }

    [Fact]
    public void DIAutowire_RegistersKeyedSelfService_WhenNoComponent()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        var keyedService = provider.GetKeyedService<KeyedSelfRegisteredService>("keyed-self");

        Assert.NotNull(keyedService);
        Assert.Equal("KeyedSelf", keyedService.Value);
        Assert.Null(provider.GetService<KeyedSelfRegisteredService>());
    }
}
