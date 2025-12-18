public class AsyncMethods
{
    public async void Run()
    {
        var downloader = new AsyncDownloader();
        Console.WriteLine("Downloading the page...");

        string content = await downloader.DownloadPageAsync("https://httpbin.org/html");

        Console.WriteLine($"Content lenght: {content.Length}");
        Console.WriteLine($"First 250 characters");
        Console.WriteLine(content.Substring(0, Math.Min(250, content.Length)));

        Console.WriteLine("=== 5 Async methods ===");

        string single = await AsyncExamples.DownloadSinglePageAsync();
        Console.WriteLine($"1.Content lenght single page: {single.Length}");

        var (seq1, seq2) = await AsyncExamples.DownloadTwoPagesSequentiallyAsync();
        Console.WriteLine($"2.Content-page1: {seq1.Length}, Content-page2: {seq2.Length}");

        string delayMsg = await AsyncExamples.DelayAndReturnAsync();
        Console.WriteLine($"3.{delayMsg}");

        string[] parallel = await AsyncExamples.DownloadTwoPagesParallelAsync();
        Console.WriteLine($"4.2 pages parallel Page1: {parallel[0].Length}, page2: {parallel[1].Length}");

        int cpuResult = await AsyncExamples.CpuBoundTaskAsync();
        Console.WriteLine($"5.Total CPU-bound: {cpuResult}");
    }
}