using WinFormedge;
using Microsoft.Web.WebView2.Core;

namespace MinimalExampleApp;
internal class MyWindow : Formedge
{
    public MyWindow()
    {
        ExtendsContentIntoTitleBar = true;

        Url = "https://cn.bing.com";
        Size = new Size(1440, 900);

        MinimumSize = new Size(1440, 900);

        //WindowSystemBackdropType = SystemBackdropType.TransientWindow;
        //BackColor = Color.FromArgb(100, 0, 0, 0);
        //DefaultBackgroundColor = Color.Transparent;

        Load += MyWindow_Load;
        DOMContentLoaded += MyWindow_DOMContentLoaded;
    }

    private void MyWindow_Load(object? sender, EventArgs e)
    {
        CoreWebView2.AddWebResourceRequestedFilter("", CoreWebView2WebResourceContext.All);

        CoreWebView2.RemoveWebResourceRequestedFilter("", CoreWebView2WebResourceContext.All);
    }

    private void MyWindow_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
    {
        ExecuteScriptAsync(""""
(()=>{
const headerEl = document.querySelector("#hdr");
//headerEl.querySelectorAll(":scope>div").forEach(c=>c.style.appRegion="no-drag");
headerEl.style.appRegion="drag";
})();
"""");
    }
}
