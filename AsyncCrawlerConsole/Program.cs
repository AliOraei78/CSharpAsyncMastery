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
        */
        /*
        var urls = UrlGenerator.GetDelayedUrls(100);

        await SequentialDownloader.DownloadSequentiallyAsync( urls );
        await ParallelDownloader.DownloadParallelAsync( urls );
        await ParallelDownloader.DownloadLimitedParallelAsync( urls, maxConcurren: 10 );
        */
        /*
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
        */
        /*
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
        result = await RetryDownloader.DownloadWithRetryAsync("https://httpbin.org/delay/10", maxRetries: 3, timeout: TimeSpan.FromSeconds(2));
        Console.WriteLine(result);
        result = await ResilientDownloader.DownloadWithTimeoutAndRetryAsync(
        "https://httpbin.org/delay/10",
        maxRetries: 4,
        timeout: TimeSpan.FromSeconds(2));

        Console.WriteLine(result);
        */
        /*
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
        Console.WriteLine("=== Simple Async Stream ===");

        await foreach (var number in AsyncStreamDemo.GenerateNumebrsAsync(20, 300))
        {
            Console.WriteLine($"Received: {number}");
        } 
        */
        /*
        Console.WriteLine("=== Async Stream for extracting links ===");

        await foreach (var link in AsyncLinkExtractor.ExtractLinksAsync(
            "https://en.wikipedia.org/wiki/.NET",
            delayMs: 200))
        {
            Console.WriteLine($"Link received: {link}");
        } 
            */
        /*
        Console.WriteLine("=== Consuming Async Stream with CancellationToken ===");

        var cts = new CancellationTokenSource();

        // Start the operation
        var streamTask = ConsumeLinksAsync("https://en.wikipedia.org/wiki/.NET", cts.Token);

        // Cancel after 5 seconds
        await Task.Delay(5000);
        Console.WriteLine("\nCancellation requested after 5 seconds...");
        cts.Cancel();

        try
        {
            await streamTask;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Stream was cancelled!");
        }
        */
        /*
        Console.WriteLine("=== Consuming Async Stream with CancellationToken ===");

        var cts = new CancellationTokenSource();

        // Start the operation
        var streamTask = ConsumeLinksWithCancellationAsync("https://en.wikipedia.org/wiki/.NET", cts.Token);

        // Cancel after 5 seconds
        await Task.Delay(5000);
        Console.WriteLine("\nCancellation requested after 5 seconds...");
        cts.Cancel();

        try
        {
            await streamTask;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Stream was cancelled!");
        }
         */

       var crawler = new WebCrawler(maxConcurrent: 15, timeoutPerPage: TimeSpan.FromSeconds(10));

        var cts = new CancellationTokenSource();

        Console.WriteLine("Crawler started from https://en.wikipedia.org/wiki/.NET (max 200 pages)");

        var crawlTask = crawler.StartAsync(
            "https://en.wikipedia.org/wiki/.NET",
            maxPages: 200,
            cts.Token);

        // Cancel after 30 seconds (for testing)
        await Task.Delay(30000);
        Console.WriteLine("\nCancelling the crawler after 30 seconds...");
        cts.Cancel();

        try
        {
            await crawlTask;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Crawler was successfully cancelled.");
        }
       
        Console.ReadKey();
    }

    static async Task ConsumeLinksWithCancellationAsync(string url, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Starting consumption of links from {url}");

        await foreach (var link in AsyncLinkExtractor.ExtractLinksAsync(
            url,
            delayMs: 300,
            cancellationToken))
        {
            Console.WriteLine($"Consumed: {link}");
        }

        Console.WriteLine("All links have been consumed.");
    }
}
