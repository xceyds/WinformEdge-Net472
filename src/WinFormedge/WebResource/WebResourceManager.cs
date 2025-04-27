namespace WinFormedge.WebResource;

sealed class WebResourceManager
{

    public List<WebResourceHandler> Handlers { get; } = new();


    private CoreWebView2? _webView2 = null;

    private bool _initialized = false;

    internal WebResourceManager()
    {
    }

    public void Initialize(CoreWebView2 coreWebView2)
    {
        _webView2 = coreWebView2;

        foreach (var handler in Handlers)
        {
            AddResourceRequestedFilter(coreWebView2, handler);
        }

        _initialized = true;

        coreWebView2.WebResourceRequested += CoreWebView2WebResourceRequested;

    }

    private void CoreWebView2WebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
    {


        var uri = new Uri(e.Request.Uri);
        var webview = _webView2;

        CoreWebView2WebResourceResponse GetNotFoundResponse() => webview.Environment.CreateWebResourceResponse(null, StatusCodes.Status404NotFound, "Not Found", "");

        if (webview == null)
        {
            return;
        }

        if (uri == null)
        {
            e.Response = GetNotFoundResponse();

            return;
        }


        var matchedHandlers = Handlers.Where(x => x.WebResourceContext == e.ResourceContext);

        matchedHandlers = matchedHandlers.Where(x => x.Uri.Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase));

        matchedHandlers = matchedHandlers.Where(x => x.Uri.Host.Equals(uri.Host, StringComparison.InvariantCultureIgnoreCase));

        matchedHandlers = matchedHandlers.Where(x => uri.AbsolutePath.StartsWith(x.Uri.AbsolutePath));

        var targetHandler = matchedHandlers.OrderBy(x => x.Uri.AbsolutePath.Length).FirstOrDefault();

        if (targetHandler == null)
        {
            e.Response = GetNotFoundResponse();

            return;
        }

        targetHandler.HandleRequest(webview, e);



    }

    private static void AddResourceRequestedFilter(CoreWebView2 coreWebView2, WebResourceHandler handler)
    {
        var scheme = handler.Scheme.ToLower();
        var hostName = handler.HostName.ToLower();

        var url = GetFilterUrl(scheme, hostName);

        coreWebView2.AddWebResourceRequestedFilter(url + "*", handler.WebResourceContext);
    }

    private static string GetFilterUrl(string scheme, string hostName)
    {
        var url = $"{scheme}://{hostName}";

        if (url.Last() != '/') url += '/';

        url = url.ToLower();
        return url;
    }

    public void RegisterResourceHander(WebResourceHandler handler)
    {
        var scheme = handler.Scheme.ToLower();
        var hostName = handler.HostName.ToLower();
        var context = handler.WebResourceContext;

        if (Handlers.Contains(handler))
        {
            throw new InvalidOperationException("Handler is existed");
        }

        Handlers.Add(handler);

        if (_initialized)
        {
            AddResourceRequestedFilter(_webView2!, handler);
        }
    }

    public void UnregisterResourceHander(WebResourceHandler handler)
    {
        var scheme = handler.Scheme.ToLower();
        var hostName = handler.HostName.ToLower();
        var context = handler.WebResourceContext;

        if (Handlers.Contains(handler))
        {
            Handlers.Remove(handler);
        }

        if (_initialized)
        {
            var url = GetFilterUrl(scheme, hostName);


            _webView2!.RemoveWebResourceRequestedFilter(url + "*", handler.WebResourceContext);
        }

    }
}
