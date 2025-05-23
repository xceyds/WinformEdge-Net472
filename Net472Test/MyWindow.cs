using WinFormedge;
using Microsoft.Web.WebView2.Core;

namespace Net472Test;

internal class MyWindow : Formedge
{
    public MyWindow()
    {
        Load += MyWindow_Load;
        DOMContentLoaded += MyWindow_DOMContentLoaded;

        Url = "https://cn.bing.com";
    }

    private void MyWindow_Load(object? sender, EventArgs e)
    {
        // Window and WebView2 are ready to use here.
    }

    private void MyWindow_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
    {
        // The DOM content is loaded and ready to use here.
        ExecuteScriptAsync(""""
                   (()=>{
                        const headerEl = document.querySelector("#hdr");
                        headerEl.style.appRegion="drag";
                        })();
        """");
    }


    protected override WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
    {
        var win = opts.UseDefaultWindow();
        win.ExtendsContentIntoTitleBar = true;
        return win;
    }

}