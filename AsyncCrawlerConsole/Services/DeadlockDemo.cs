using System.Threading;
using System.Threading.Tasks;

public static class DeadlockDemo
{
    public static async Task<string> AsyncOperationAsync()
    {
        await Task.Delay(1000);
        return "Async Operation Completed";
    }

    public static string SyncOverAsyncWithDeadlock()
    {
        return AsyncOperationAsync().Result;
    }

    public static string SyncOverAsyncSafe()
    {
        return Task.Run(() => AsyncOperationAsync()).Result;
    }

    public static async Task RunDeadlockTestAsync()
    {
        var originalContext = SynchronizationContext.Current;
        var uiContext = new SynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(uiContext);

        Console.WriteLine("=== Deadlock Test ===");
        Console.WriteLine("Without ConfigureAwait(false) – deadlock is possible");
        try
        {
            string result = SyncOverAsyncWithDeadlock();
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Deadlock occurred: {ex.Message}");
        }
        Console.WriteLine("With Task.Run - Safe – no deadlock");
        string safeResult = SyncOverAsyncSafe();
        Console.WriteLine(safeResult);

        SynchronizationContext.SetSynchronizationContext(originalContext);
    }
}