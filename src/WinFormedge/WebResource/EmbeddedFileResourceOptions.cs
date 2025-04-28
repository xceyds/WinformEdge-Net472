using System.Reflection;


namespace WinFormedge.WebResource;

public sealed class EmbeddedFileResourceOptions : WebResourceOptions
{
    public string? DefaultFolderName { get; init; }
    public string? DefaultNamespace { get; init; }

    public CoreWebView2WebResourceContext WebResourceContext { get; init; } = CoreWebView2WebResourceContext.All;
    public required Assembly ResourceAssembly { get; init; }

}
