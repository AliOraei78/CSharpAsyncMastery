public static class UrlGenerator
{
    public static List<string> GetDelayedUrls(int count = 100)
    {
        var urls = new List<string>();
        for (int i = 0; i < count; i++)
            urls.Add($"https://httpbin.org/delay/1");
        return urls;
    }

    public static List<string> GetVariedDelayedUrls(int count = 100)
    {
        var urls = new List<string>();
        for (int i = 0; i < count; i++)
        {
            int delay = i % 5 + 1;
            urls.Add($"https://httpbin.org/delay/1");
        }
        return urls;
    }
}