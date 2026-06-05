using DIAutowire.Helpers;
using JetBrains.Annotations;

namespace DIAutowire.Extensions;

/// <summary>
/// Provides extension methods for SemaphoreSlim to allow for easier asynchronous locking using the 'using' statement.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class SemaphoreSlimExtensions
{
    /// <summary>
    /// Asynchronously waits to enter the SemaphoreSlim and returns an IDisposable that will release the semaphore when disposed.
    /// </summary>
    /// <param name="semaphore">The SemaphoreSlim to wait on.</param>
    /// <returns>An IDisposable that will release the semaphore when disposed.</returns>
    public static async Task<IDisposable> WaitAsync(this SemaphoreSlim semaphore)
    {
        await semaphore.WaitAsync();
        return new SemaphoreDisposer(semaphore);
    }
}