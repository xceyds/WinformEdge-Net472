using WinFormedge;

namespace FantasticCalculatorDemo;
internal class CalculatorApp : AppStartup
{
    protected override bool OnApplicationLaunched(string[] args)
    {
        ApplicationConfiguration.Initialize();

        return true;
    }
    protected override AppCreationAction? OnApplicationStartup(StartupSettings options)
    {
        return options.UseMainWindow(new MainCalculatorWindow());
    }
}