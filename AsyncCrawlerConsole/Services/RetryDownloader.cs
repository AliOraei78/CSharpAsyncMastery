using System.Threading;

public static class RetryDownloader
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<string> DownloadWithRetryAsync(
        string url,
        int maxRetries = 3,
        TimeSpan? timeout = null)
    {
        var cts = timeout.HasValue
            ? new CancellationTokenSource(timeout.Value)
            : new CancellationTokenSource();

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                Console.WriteLine($"Attempt {attempt} to download: {url}");
                HttpResponseMessage response = await _httpClient.GetAsync(url, cts.Token);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync(cts.Token);
                Console.WriteLine($"Successfully downloaded: {url}, {content.Length} characters");
                return content;
            }
            catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
            {
                Console.WriteLine($"Download timed out: {attempt}");
                if (attempt == maxRetries) return $"Final timeout: {url}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on attempt {attempt} for {url}: {ex.Message}");
                if (attempt == maxRetries) return $"Final attempt failed: {url}";
            }
            await Task.Delay(1000 * attempt, CancellationToken.None);
        }

        return "Failed";
    }

}