using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

public class AsyncDownloader
{
    private readonly HttpClient _httpClient;
    public AsyncDownloader()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharpAsyncMastery/1.0");
    }

    public async Task<string> DownloadPageAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
        catch(HttpRequestException ex)
        {
            return $"Error: {url}: {ex}";
        }
    }

    public async Task<string> DownloadPageWithCancellationAsync(string url, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"Starting download: {url}");
            HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            return content;
        }
        catch(OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return $"Download cancelled: {url}";
        }
        catch(HttpRequestException ex)
        {
            return $"Error: {url}: {ex.Message}";
        }
    }

    public async Task<string> DownloadPageWithTimeoutAsync(string url, TimeSpan timeout)
    {
        using (var cts = new CancellationTokenSource(timeout))
        {
            try
            {
                Console.WriteLine($"Starting download with timeout: {timeout.TotalSeconds}, url: {url}");
                HttpResponseMessage response = await _httpClient.GetAsync(url, cts.Token);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync(cts.Token);
                Console.WriteLine($"Completed download: {url}, {content.Length} characters");
                return content;
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested)
            {
                Console.WriteLine($"Download timed out after: {timeout.TotalSeconds}ms");
                return $"Download timed out, url: {url}";
            }
            catch (Exception ex)
            {
                return $"Error: {url}: {ex.Message}";
            }
        }
    }
}