using System.Diagnostics;

public static class ParallelDownloader
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task DownloadParallelAsync(List<string> urls)
    {
        var stopwatch = Stopwatch.StartNew();
        var tasks = urls.Select(url => DownloadSinglePageAsync(url)).ToList();
        string[] results = await Task.WhenAll(tasks);
        stopwatch.Stop();
        Console.WriteLine($"Parallel download completed in {stopwatch.ElapsedMilliseconds} ms, {stopwatch.Elapsed.TotalSeconds:F2 seconds}");
        Console.WriteLine($"Total pages: {results.Length}");
        Console.WriteLine($"First 5: ");
        foreach(var result in results.Take(5))
            Console.WriteLine(result);
        Console.WriteLine("----------\n");
    }

    public static async Task<string> DownloadSinglePageAsync(string url)
    {
        try 
        {
            var content = await _httpClient.GetByteArrayAsync(url);
            return $"Downloaded {url} with {content.Length} characters.";
        }
        catch (Exception ex)
        {
            return $"Error downloading {url}: {ex.Message}";
        }
    }

    public static async Task DownloadLimitedParallelAdync(List<string> urls, int maxConcurren  = 10)
    {
        var stopwatch = Stopwatch.StartNew();
        var semaphore = new SemaphoreSlim(maxConcurren);

        var tasks = new List<Task<string>>();
        foreach(var url in urls)
        {
            tasks.Add(Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    string content = await _httpClient.GetStringAsync(url);
                    return $"Downloaded {url} with {content.Length} characters.";
                }
                catch (Exception ex)
                {
                    return $"Error downloading {url}: {ex.Message}";
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        string[] results = await Task.WhenAll(tasks);
        stopwatch.Stop();
        Console.WriteLine($"Limited parallel download completed in {stopwatch.ElapsedMilliseconds} ms, {stopwatch.Elapsed.TotalSeconds:F2 seconds}");
        Console.WriteLine($"Total pages: {results.Length}");
        Console.WriteLine($"First 5: ");
        foreach (var result in results.Take(5))
            Console.WriteLine(result);
        Console.WriteLine("----------\n");
    }
}