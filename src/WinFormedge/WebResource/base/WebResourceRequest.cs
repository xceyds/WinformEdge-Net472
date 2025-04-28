namespace WinFormedge.WebResource;

public sealed class WebResourceRequest
{
    public Uri Uri { get; }
    public CoreWebView2HttpRequestHeaders Headers { get; }

    public string RequestUrl
    {
        get
        {
            var original = Uri.OriginalString;
            if (original.Contains('?'))
            {
                return original.Substring(0, original.IndexOf("?"));
            }

            return original;
        }
    }

    public string RelativePath => $"{Uri?.LocalPath ?? string.Empty}".TrimStart('/');
    public string FileName => Path.GetFileName(RelativePath);
    public string FileExtension => Path.GetExtension(FileName).TrimStart('.');
    public bool HasFileName => !string.IsNullOrEmpty(FileName);


    public CoreWebView2WebResourceRequest Request { get; }

    internal WebResourceRequest(CoreWebView2WebResourceRequest request, CoreWebView2WebResourceRequestSourceKinds requestSourceKinds, CoreWebView2WebResourceContext webResourceContext)
    {
        Request = request;
        Uri = new Uri(request.Uri);
        Headers = request.Headers;
    }
}
