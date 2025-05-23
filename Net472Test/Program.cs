using WinFormedge;

namespace Net472Test;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        var app = FormedgeApp.CreateAppBuilder()
            .UseDevTools()
            .UseWinFormedgeApp<MyFormedgeApp>().Build();

        app.Run();
    }
}