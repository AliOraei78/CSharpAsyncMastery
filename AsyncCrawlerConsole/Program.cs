public class Program
{
    static async Task Main(string[] args)
    {
        /*
        var asyncMethods = new AsyncMethods();
        asyncMethods.Run();

        var urls = UrlGenerator.GetDelayedUrls(100);
        Console.WriteLine($"URL count: {urls.Count}");
        Console.WriteLine($"First 5 urls: ");
        foreach ( var url in urls ) 
            Console.WriteLine( url );
        
        var urls = UrlGenerator.GetDelayedUrls(100);

        await SequentialDownloader.DownloadSequentiallyAsync( urls );
        await ParallelDownloader.DownloadParallelAsync( urls );
        await ParallelDownloader.DownloadLimitedParallelAdync( urls, maxConcurren: 10 );

        Console.WriteLine("=== ConfigureAwait Test in Console App ===");
        await ConfigureAwaitDemo.RunWithConfigureAwaitTrueAsync();
        Console.WriteLine("----------");
        await ConfigureAwaitDemo.RunWithConfigureAwaitFalseAsync();
        Console.WriteLine("----------");
        Console.WriteLine("It doesn’t matter in a Console App because it doesn’t have a SynchronizationContext!");
        Console.WriteLine("----------");
        await ConfigureAwaitContextDemo.RunWithContextSimulationAsync();
                */

        await DeadlockDemo.RunDeadlockTestAsync();

        Console.ReadKey();
    }
}