using HtmlAgilityPack;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

public class WebCrawler
{
    private readonly HttpClient _httpClient;
    private readonly int _maxConcurrent;
    private readonly TimeSpan _timeoutPerPage;

    public WebCrawler(int maxConcurrent = 20, TimeSpan? timeoutPerPage = null)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CSharpCrawler/1.0");
        _maxConcurrent = maxConcurrent;
        _timeoutPerPage = timeoutPerPage ?? TimeSpan.FromSeconds(10);
    }

    public async Task StartAsync(
        string startUrl,
        int maxPages = 1000,
        CancellationToken cancellationToken = default)
    {
        var visited = new ConcurrentBag<string>();
        var queue = new ConcurrentQueue<string>();
        queue.Enqueue(startUrl);
        visited.Add(startUrl);

        var semaphore = new SemaphoreSlim(_maxConcurrent);

        var tasks = new List<Task>();

        int crawledCount = 0;

        while (queue.TryDequeue(out string? currentUrl)
               && crawledCount < maxPages
               && !cancellationToken.IsCancellationRequested)
        {
            await semaphore.WaitAsync(cancellationToken);

            var task = Task.Run(async () =>
            {
                try
                {
                    Interlocked.Increment(ref crawledCount);
                    Console.WriteLine($"[{crawledCount}/{maxPages}] Crawling: {currentUrl}");

                    using var cts = new CancellationTokenSource(_timeoutPerPage);
                    using var linkedCts =
                        CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken);

                    string html = await _httpClient.GetStringAsync(currentUrl, linkedCts.Token);

                    // Async Stream for extracting links
                    await foreach (var link in ExtractLinksAsync(html, currentUrl, linkedCts.Token))
                    {
                        if (!visited.Contains(link) && crawledCount < maxPages)
                        {
                            visited.Add(link);
                            queue.Enqueue(link);
                            Console.WriteLine($"   New link added: {link}");
                        }
                    }
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Global cancellation: {currentUrl}");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"Timeout for {currentUrl}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error on {currentUrl}: {ex.Message}");
                }
                finally
                {
                    semaphore.Release();
                }
            }, cancellationToken);

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        Console.WriteLine($"Crawler finished. Number of pages crawled: {crawledCount}");
    }

    // Async Stream for extracting links
    private static async IAsyncEnumerable<string> ExtractLinksAsync(
        string html,
        string baseUrl,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");

        if (linkNodes == null)
            yield break;

        foreach (var node in linkNodes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(10, cancellationToken); // Small delay for simulation

            string href = node.GetAttributeValue("href", string.Empty);
            if (string.IsNullOrEmpty(href))
                continue;

            Uri? fullUri = null;
            try
            {
                fullUri = new Uri(new Uri(baseUrl), href);
            }
            catch
            {
                continue;
            }

            if (fullUri != null &&
                (fullUri.Scheme == Uri.UriSchemeHttp || fullUri.Scheme == Uri.UriSchemeHttps))
            {
                yield return fullUri.ToString();
            }
        }
    }
}
