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
        await ParallelDownloader.DownloadLimitedParallelAsync( urls, maxConcurren: 10 );

        Console.WriteLine("=== ConfigureAwait Test in Console App ===");
        await ConfigureAwaitDemo.RunWithConfigureAwaitTrueAsync();
        Console.WriteLine("----------");
        await ConfigureAwaitDemo.RunWithConfigureAwaitFalseAsync();
        Console.WriteLine("----------");
        Console.WriteLine("It doesn’t matter in a Console App because it doesn’t have a SynchronizationContext!");
        Console.WriteLine("----------");
        await ConfigureAwaitContextDemo.RunWithContextSimulationAsync();
        await DeadlockDemo.RunDeadlockTestAsync();
        Console.WriteLine("=== CancellationToken Test ===");
        await CancellationDemo.RunCancellationTestAsync();
        var downloader = new AsyncDownloader();
        var cts = new CancellationTokenSource();

        var task = downloader.DownloadPageWithCancellationAsync("https://httpbin.org/delay/10", cts.Token);
        await Task.Delay(3000);
        Console.WriteLine("Request for cancellation after 3 seconds...");
        cts.Cancel();

        try
        {
            string result = await task;
            Console.WriteLine(result);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled because of exception");
        }
        var urls = UrlGenerator.GetDelayedUrls(50);
        var cts = new CancellationTokenSource();
        var task = ParallelCancellationDownloader.DownloadParallelWithCancellationAsync(urls, cts.Token);
        await Task.Delay(5000);
        Console.WriteLine("\nWill cancell after 5 seconds...");
        cts.Cancel();
        await task;
        var downloader = new AsyncDownloader();
        string result = await downloader.DownloadPageWithTimeoutAsync("https://httpbin.org/delay/10", TimeSpan.FromSeconds(3));
        Console.WriteLine(result);
        string result = await RetryDownloader.DownloadWithRetryAsync("https://httpbin.org/delay/10", maxRetries: 3, timeout: TimeSpan.FromSeconds(2));
        Console.WriteLine(result);
        string result = await ResilientDownloader.DownloadWithTimeoutAndRetryAsync(
        "https://httpbin.org/delay/10",
        maxRetries: 4,
        timeout: TimeSpan.FromSeconds(2));

        Console.WriteLine(result);
                */
        Console.WriteLine("=== Testing the Resilient Downloader with a problematic page ===\n");

        // Page with a long delay (8 seconds) + 2-second timeout + 4 retries
        string result1 = await ResilientDownloader.DownloadWithTimeoutAndRetryAsync(
            "https://httpbin.org/delay/8",
            maxRetries: 4,
            timeout: TimeSpan.FromSeconds(2));

        Console.WriteLine($"\nResult of the long page: {result1}\n");

        // Page with a 500 error (the server returns an error)
        string result2 = await ResilientDownloader.DownloadWithTimeoutAndRetryAsync(
            "https://httpbin.org/status/500",
            maxRetries: 3,
            timeout: TimeSpan.FromSeconds(5));

        Console.WriteLine($"\nResult of the error page: {result2}\n");

        // Good page
        string result3 = await ResilientDownloader.DownloadWithTimeoutAndRetryAsync(
            "https://httpbin.org/html",
            maxRetries: 1,
            timeout: TimeSpan.FromSeconds(10));

        Console.WriteLine($"\nResult of the good page: content length {result3.Length} characters");

        Console.ReadKey();
    }
}