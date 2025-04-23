using WinFormedge;

namespace MinimalExampleApp;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();


        var app = FormedgeApp.CreateBuilder()
            .UseCulture(Application.CurrentCulture.Name)
            .UseDevTools()
            .UseModernScrollbarStyle()
            .UseWinFormedgeApp<MyFormedgeApp>().Build();

        app.Run();
            

    }
}
