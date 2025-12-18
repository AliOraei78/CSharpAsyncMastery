using System.Diagnostics;

public static class SequentialDownloader
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task DownloadSequentiallyAsync(List<string> urls)
    {
        var stopwatch = Stopwatch.StartNew();
        var results = new List<string>();
        foreach (var url in urls)
        {
            try
            {
                string content = await _httpClient.GetStringAsync(url);
                results.Add($"Url downloaded: {url}, Charcters: {content.Length}");
            }
            catch (Exception ex)
            {
                results.Add($"Error in: {url}, Message: {ex.Message}");
            }
        }
        stopwatch.Stop();

        Console.WriteLine("=== Sequential download ===");
        Console.WriteLine($"Total time {stopwatch.ElapsedMilliseconds} ms, {stopwatch.Elapsed.TotalSeconds:F2} seconds");
        Console.WriteLine($"Total page: {results.Count}");

        Console.WriteLine("First 5 results");
        foreach( var result in results.Take(5) )
            Console.WriteLine(result);
        Console.WriteLine("----------\n");
    }
}