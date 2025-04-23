using WinFormedge;

namespace MinimalExampleApp;

internal class MyFormedgeApp : AppStartup
{
    protected override bool OnApplicationLaunched(string[] args)
    {

        return true;
    }
    protected override AppCreationAction? OnApplicationStartup(StartupOptions options)
    {
        return options.UseMainWindow(new MyWindow());
    }
}