using Microsoft.Extensions.DependencyInjection;
using DIAutowire.Extensions;
using DIAutowire.Attributes.Implementation;
using DIAutowire.Attributes.Method;
using DIAutowire.Enums;
using DIAutowire.Attributes.Interface;

namespace DIAutowire.Tests;

public static class AutorunExecutionLog
{
    public static List<string> Log { get; } = new();
    public static void Clear() => Log.Clear();
}

[Service]
[Autowire(ServiceLifetime.Scoped)]
public partial class AutorunScopedService
{
    [Autorun(AutorunPriority.Low)]
    public void RunLowPriority()
    {
        AutorunExecutionLog.Log.Add("Scoped:Low");
    }

    [Autorun(AutorunPriority.High)]
    public Task RunHighPriorityAsync()
    {
        AutorunExecutionLog.Log.Add("Scoped:High");
        return Task.CompletedTask;
    }
}

[Autowire(ServiceLifetime.Singleton)]
public partial class AutorunSingletonService
{
    [Autorun(AutorunPriority.Critical)]
    public void RunCriticalPriority()
    {
        AutorunExecutionLog.Log.Add("Singleton:Critical");
    }
}

public class LaunchAutorunTests : IDisposable
{
    public LaunchAutorunTests()
    {
        AutorunExecutionLog.Clear();
    }

    public void Dispose()
    {
        AutorunExecutionLog.Clear();
    }

    [Fact]
    public async Task LaunchAutorun_ExecutesMethodsInPriorityOrder()
    {
        var services = new ServiceCollection();
        services.DIAutowire();
        var provider = services.BuildServiceProvider();

        await provider.LaunchAutorun();

        var expectedOrder = new List<string>
        {
            "Singleton:Critical",
            "Scoped:High",
            "Scoped:Low"
        };

        Assert.Equal(expectedOrder, AutorunExecutionLog.Log);
    }
}
