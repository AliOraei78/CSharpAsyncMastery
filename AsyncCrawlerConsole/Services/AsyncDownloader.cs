using System.Threading.Tasks;
using System.Net.Http;

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
}