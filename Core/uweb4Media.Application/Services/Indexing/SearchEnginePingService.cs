namespace uweb4Media.Application.Services.Indexing;

public class SearchEnginePingService
{
    private readonly HttpClient _httpClient;

    public SearchEnginePingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task PingAllAsync(string sitemapUrl)
    {
        try
        {
            await _httpClient.GetAsync(
                $"https://www.google.com/ping?sitemap={System.Uri.EscapeDataString(sitemapUrl)}");
            await _httpClient.GetAsync($"https://www.bing.com/ping?sitemap={System.Uri.EscapeDataString(sitemapUrl)}");
            // OpenAI Index API veya diğerleri eklenebilir.
        }
        catch
        {
            // Loglanabilir, kritik hata değildir.
        }
    }
}
