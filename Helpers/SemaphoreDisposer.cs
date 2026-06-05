namespace DIAutowire.Helpers;

/// <summary>
/// A struct that implements IDisposable to release a SemaphoreSlim when disposed.
/// </summary>
/// <param name="semaphore">The SemaphoreSlim to be released when the struct is disposed.</param>
public readonly struct SemaphoreDisposer(SemaphoreSlim semaphore) : IDisposable
{
    public void Dispose() => semaphore.Release();
}