using System.Reflection;

using WinFormedge;
using WinFormedge.WebResource;

namespace MinimalExampleApp;
internal class ExcludedEdgesWindow : Formedge
{
    public ExcludedEdgesWindow()
    {
        Load += ExcludedEdgesWindow_Load;

        SetVirtualHostNameToEmbeddedResourcesMapping(new EmbeddedFileResourceOptions
        {
            Scheme = "https",
            HostName = "embedded.appresource.local",
            ResourceAssembly = Assembly.GetExecutingAssembly(),
            DefaultFolderName = "Resources\\wwwroot"
        });

        Url = "https://embedded.appresource.local/excluded_border_test.html";

        WebMessageReceived += ExcludedEdgesWindow_WebMessageReceived;

        AllowDeveloperTools = false;
    }

    private void ExcludedEdgesWindow_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
    {
        if(e.WebMessageAsJson == "\"ExperienceClicked\"")
        {
            var ps = new System.Diagnostics.ProcessStartInfo("https://github.com/XuanchenLin/WinFormedge")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            System.Diagnostics.Process.Start(ps);

            
        }
    }

    protected override WindowSettings ConfigureHostWindowSettings(HostWindowBuilder opts)
    {
        var win = opts.UseDefaultWindow();

        win.ExtendsContentIntoTitleBar = true;
        win.SystemBackdropType = SystemBackdropType.Manual;
        win.ShowWindowDecorators = false;
        win.MinimumSize = new Size(800, 480);
        win.Size = new Size(960, 640);
        win.WindowEdgeOffsets = new Padding(13, 8, 21, 25);
        win.StartPosition = FormStartPosition.CenterScreen;

        return win;
    }

    private void ExcludedEdgesWindow_Load(object? sender, EventArgs e)
    {
        
    }


}
