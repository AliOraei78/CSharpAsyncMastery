using System.Threading;
using System.Threading.Tasks;

public static class ConfigureAwaitContextDemo
{
    public static async Task RunWithContextSimulationAsync()
    {
        var originalContext = SynchronizationContext.Current;
        var uiContext = new SynchronizationContext();
        
        SynchronizationContext.SetSynchronizationContext(uiContext);
        Console.WriteLine("===Simulation of UI Context ===");

        Console.WriteLine($"Thread before await: {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(100);
        Console.WriteLine($"Thread after await(without ConfigureAwait): {Thread.CurrentThread.ManagedThreadId}");

        Console.WriteLine("Back to the UI thread (the context was captured)\n");

        await Task.Delay(100).ConfigureAwait(false);

        Console.WriteLine($"Thread after await (with ConfigureAwait(false)): {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine("Stayed on the ThreadPool, did not return to the UI thread");

        SynchronizationContext.SetSynchronizationContext(originalContext);
    }
}