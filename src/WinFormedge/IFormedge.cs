
using WinFormedge.WebResource;

namespace WinFormedge;
public interface IFormedge
{
    bool AllowDeveloperTools { get; set; }
    bool AllowExternalDrop { get; set; }
    bool CanGoBack { get; }
    bool CanGoForward { get; }
    string DocumentTitle { get; }
    bool Enabled { get; set; }
    bool Fullscreen { get; set; }
    nint Handle { get; }
    int Height { get; set; }
    Icon? Icon { get; set; }
    int Left { get; set; }
    Point Location { get; set; }
    bool Maximizable { get; set; }
    Size MaximumSize { get; set; }
    bool Minimizable { get; set; }
    Size MinimumSize { get; set; }
    bool ShowDocumentTitle { get; set; }
    bool ShowInTaskbar { get; set; }
    Size Size { get; set; }
    FormStartPosition StartPosition { get; set; }
    int Top { get; set; }
    bool TopMost { get; set; }
    string Url { get; set; }
    bool Visible { get; set; }
    int Width { get; set; }
    string WindowCaption { get; set; }
    FormWindowState WindowState { get; set; }

    event EventHandler? Activated;
    event EventHandler<CoreWebView2ContentLoadingEventArgs>? ContentLoading;
    event EventHandler? Deactivate;
    event EventHandler<CoreWebView2DOMContentLoadedEventArgs>? DOMContentLoaded;
    event FormClosedEventHandler? FormClosed;
    event FormClosingEventHandler? FormClosing;
    event EventHandler? GotFocus;
    event EventHandler? Load;
    event EventHandler? LostFocus;
    event EventHandler? Move;
    event EventHandler<CoreWebView2NavigationCompletedEventArgs>? NavigationCompleted;
    event EventHandler<CoreWebView2NavigationStartingEventArgs>? NavigationStarting;
    event EventHandler? Resize;
    event EventHandler? ResizeBegin;
    event EventHandler? ResizeEnd;
    event EventHandler? Shown;
    event EventHandler? VisibleChanged;
    event EventHandler<CoreWebView2WebMessageReceivedEventArgs>? WebMessageReceived;

    void Activate();
    void ClearVirtualHostNameToEmbeddedResourcesMapping(EmbeddedFileResourceOptions options);
    void ClearVirtualHostNameToFolderMapping(string hostName);
    void Close();
    Task<string> ExecuteScriptAsync(string script);
    void GoBack();
    void GoForward();
    void NavigateToString(string htmlContent);
    void RegisterWebResourceHander(WebResourceHandler resourceHandler);
    void Reload();
    void SetVirtualHostNameToEmbeddedResourcesMapping(EmbeddedFileResourceOptions options);
    void SetVirtualHostNameToFolderMapping(string hostName, string folderPath, CoreWebView2HostResourceAccessKind accessKind);
    void Show();
    void Show(IWin32Window? owner);
    DialogResult ShowDialog();
    DialogResult ShowDialog(IWin32Window? owner);
    void Stop();
    void ToggleFullscreen();
    void UnregisterWebResourceHandler(WebResourceHandler resourceHandler);
}