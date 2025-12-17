using System.Net.Http;
using System.Threading.Tasks;

public static class AsyncExamples
{
    public static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<string> DownloadSinglePageAsync() => await _httpClient.GetStringAsync("https://httpbin.org/html");

    public static async Task<(string, string)> DownloadTwoPagesSequentiallyAsync()
    {
        string page1 = await _httpClient.GetStringAsync("https://httpbin.org/html");
        string page2 = await _httpClient.GetStringAsync("https://httpbin.org/json");

        return (page1, page2);
    }

    public static async Task<string> DelayAndReturnAsync()
    {
        await Task.Delay(2000);
        return "Delayed for 2 secs!";
    }

    public static Task FireAndForgetDownloadAsync() => 
        _httpClient.GetStringAsync("https://httpbin.org/delay/1");

    public static ValueTask<string> GetCachedOrDownloadAsync() => 
        new ValueTask<string>("From chache");

    public static async Task<string> DownloadWithTimeoutAsync(string url, int timeoutMs = 5000)
    {
        using var cts = new CancellationTokenSource(timeoutMs);
        var response = await _httpClient.GetAsync(url, cts.Token);
        return await response.Content.ReadAsStringAsync(); 
    }

    public static async Task<string[]> DownloadTwoPagesParallelAsync()
    {
        Task<string> task1 = _httpClient.GetStringAsync("https://httpbin.org/html");
        Task<string> task2 = _httpClient.GetStringAsync("https://httpbin.org/json");

        return await Task.WhenAll(task1, task2);
    }

    public static async void AsyncVoidExample()
    {
        await Task.Delay(1000);
        Console.WriteLine("Async void is done!");
    }

    public static Task<int> CpuBoundTaskAsync()
    {
        return Task.Run(()
            =>
        {
            int sum = 0;
            for(int i = 0; i < 10_000_000; i++) sum += 1;
            return sum;
        });
    }

    public static async Task<string> DownloadAfterDelayAsync()
    {
        await Task.Delay(1000);
        return await _httpClient.GetStringAsync("https://httpbin.org/html");
    }
}