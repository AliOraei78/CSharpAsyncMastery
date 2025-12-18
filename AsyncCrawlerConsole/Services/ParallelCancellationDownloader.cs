using System.Diagnostics;

public static class ParallelCancellationDownloader
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task DownloadParallelWithCancellationAsync(List<string> urls, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var tasks = urls.Select(url => DownloadSinglePageWithCancellationAsync(url, cancellationToken)).ToList();

        try
        {
            string[] results = await Task.WhenAll(tasks);

            stopwatch.Stop();

            Console.WriteLine("=== WhenAll with Cancellation ===");
            Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}ms, {stopwatch.Elapsed.Seconds:F2} seconds");
            Console.WriteLine($"Total completed downloaded: {results.Count(r => r.StartsWith("Downloaded"))}");
            Console.WriteLine("First 5");
            foreach (var result in results)
                Console.WriteLine(result);
        }
        catch(OperationCanceledException)
        {
            stopwatch.Stop();
            Console.WriteLine($"All downloads cancelled, time: {stopwatch.ElapsedMilliseconds} ms");
        }
        Console.WriteLine("----------\n");
    }

    private static async Task<string> DownloadSinglePageWithCancellationAsync(string url, CancellationToken cancellationToken)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            return $"Download completed: {url}, {content.Length} characters";
        }
        catch(OperationCanceledException)
        {
            return $"Cancelled: {url}";
        }
        catch(Exception ex)
        {
            return $"Error in {url}: {ex.Message}";
        }
    }
}