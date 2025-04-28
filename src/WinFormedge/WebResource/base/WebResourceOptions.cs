namespace WinFormedge.WebResource;

public delegate string WebResourceFallbackDelegate(string requestUrl);
public abstract class WebResourceOptions
{
    public required string Scheme { get; init; } = "http";
    public required string HostName { get; init; }
    public WebResourceFallbackDelegate? OnFallback { get; set; }
}