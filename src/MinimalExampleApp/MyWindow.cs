using WinFormedge;
using WinFormedge.WebResource;

using Microsoft.Web.WebView2.Core;
using System.Reflection;

namespace MinimalExampleApp;
internal class MyWindow : Formedge
{
    public MyWindow()
    {
        ExtendsContentIntoTitleBar = true;

        //Url = "https://cn.bing.com";
        Size = new Size(1440, 900);

        MinimumSize = new Size(1440, 900);

        //WindowSystemBackdropType = SystemBackdropType.TransientWindow;
        //BackColor = Color.FromArgb(100, 0, 0, 0);
        //DefaultBackgroundColor = Color.Transparent;

        Load += MyWindow_Load;
        DOMContentLoaded += MyWindow_DOMContentLoaded;

        this.SetVirtualHostNameToEmbeddedResourcesMapping(new EmbeddedFileResourceOptions { 
            Scheme="https", 
            HostName="embedded.appresource.local", 
            ResourceAssembly=Assembly.GetExecutingAssembly(),
            DefaultFolderName="Resources\\wwwroot"
        });

        Url = "https://embedded.appresource.local";
    }

    private void MyWindow_Load(object? sender, EventArgs e)
    {
        
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
