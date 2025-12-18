using System.Threading;

public static class ResilientDownloader
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<string> DownloadWithTimeoutAndRetryAsync(string url, int maxRetries = 3, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(5);

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            using var cts = new CancellationTokenSource(timeout.Value);

            try
            {
                Console.WriteLine($"Attempt {attempt} to download {url} with timeout {timeout.Value.TotalSeconds} seconds.");
                HttpResponseMessage response = await _httpClient.GetAsync(url, cts.Token);

                string content = await response.Content.ReadAsStringAsync(cts.Token);
                Console.WriteLine($"Success! {url}, {content.Length} characters");
                return content;
            }
            catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
            {
                Console.WriteLine($"Attempt {attempt} timed out after {timeout.Value.TotalSeconds} seconds.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
            }

            if (attempt < maxRetries)
            {
                int delayMs = 1000 * attempt;
                Console.WriteLine($"Wait {delayMs} ms");
                await Task.Delay(delayMs);
            }
        }
        return $"Failed to download {url} after {maxRetries} attempts.";
    }
}