using WinFormedge.WebResource;

namespace WinFormedge;

public static class  RegisterEmbeddedFileResourceExtension
{
    public static void SetVirtualHostNameToEmbeddedResourcesMapping(this Formedge formedge,  EmbeddedFileResourceOptions options )
    {
        formedge.RegisterWebResourceHander(new EmbeddedFileResourceHandler(options));
    }
}