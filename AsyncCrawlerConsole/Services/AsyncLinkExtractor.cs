using HtmlAgilityPack;
using System.Runtime.CompilerServices;
using System.Threading;

public static class AsyncLinkExtractor
{
    private static readonly HttpClient _httpClient = new HttpClient();
    static AsyncLinkExtractor()
    {
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
    }

    public static async IAsyncEnumerable<string> ExtractLinksAsync(
        string url,
        int delayMs = 100,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string html;
        try
        {
            Console.WriteLine($"Starting page download: {url}");
            html = await _httpClient.GetStringAsync(url, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during download: {ex.Message}");
            yield break;
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

        if (linkNodes == null)
            yield break;

        foreach (var node in linkNodes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(delayMs, cancellationToken);

            string href = node.GetAttributeValue("href", string.Empty);
            if (string.IsNullOrEmpty(href))
                continue;

            string fullUrl = href.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? href
                : new Uri(new Uri(url), href).ToString();

            Console.WriteLine($"Link extracted: {fullUrl}");
            yield return fullUrl;
        }
    }

}
