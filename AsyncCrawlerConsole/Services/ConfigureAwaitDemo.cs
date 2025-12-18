public static class ConfigureAwaitDemo
{
    public static async Task RunWithConfigureAwaitTrueAsync() 
    { 
        Console.WriteLine($"Thread before await(true): {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(100).ConfigureAwait(true);
        Console.WriteLine($"Thread after await(true): {Thread.CurrentThread.ManagedThreadId}");
    }

    public static async Task RunWithConfigureAwaitFalseAsync() 
    { 
        Console.WriteLine($"Thread before await(false): {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(100).ConfigureAwait(false);
        Console.WriteLine($"Thread after await(false): {Thread.CurrentThread.ManagedThreadId}");
    }
}