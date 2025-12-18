using System.Threading;
using System.Threading.Tasks;

public static class CancellationDemo
{
    public static async Task LongRunningOperationAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Long-running operation started.");
        for (int i = 0; i < 10; i++)
        {
            // Check if cancellation has been requested
            cancellationToken.ThrowIfCancellationRequested();
            // Simulate a long-running operation
            await Task.Delay(1000, cancellationToken);
            Console.WriteLine($"Iteration {i} is running...");
        }
        Console.WriteLine("Long-running operation completed.");
    }

    public static async Task RunCancellationTestAsync()
    {
        var cts = new CancellationTokenSource();
        var task = LongRunningOperationAsync(cts.Token);
        await Task.Delay(4000); // Let it run for 4 seconds
        Console.WriteLine("Requesting cancellation...");
        cts.Cancel();

        try
        {
            await task;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was canceled.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Operation failed: {ex.Message}");
        }
    }
}