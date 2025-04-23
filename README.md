<p align="center">
    <img src="./artworks/WinFormedgeLogo.png" width="144" />
</p>
<h1 align="center">The WinFormedge Project</h1>
<p align="center"><strong>A library based on Microsoft WebView2 that can buid powerful WinForms applications with HTML, CSS and JavaScript easily.</strong></p>

# WinFormedge

## ✨ About

**WinFormege** is an opensource .NET library based on Microsoft WebView2. It allows developers to create modern and visually WinForms applications using HTML, CSS, and JavaScript. With WinFormedge, you can easily integrate web technologies into your WinForms projects, enabling you to build rich and interactive user interfaces.

The motivation for developing the WinFormedge project comes from another project I maintain called **WinFormium** (AKA **NanUI**). It also allows developers to create WinForms applications using Web technologies, but the difference is that WinFormium is based on the Chromium Embedded Framework (CEF). If you are more interested in the CEF-based WinFormium project, please go to the WinFormium project repository for more information.

## 🖥️ Requirements

**Development Environment:**
- Visual Studio 2022
- .NET 8.0 or higher

**Deployment Environment:**
- Windows 10 1903 or higher

This is a **Windows ONLY** library. It is not compatible with other operating systems.

The minimum supported operating system is Windows 10 version 1903 (May 2019 Update). And some fetures such as SystemBackdrop and SystemColorMode are only available on Windows 11.

## 📋 Milestones

The project is currently in the early stages of development. The following milestones are planned for the project:

- [ ] Implement features of the Form
    - [x] Default Form
    - [x] Borderless Form
    - [x] Fullscreen mode
    - [x] System color mode
    - [ ] SystemBackdrop
    - [ ] DirectCompostion Visual Tree (for CoreWebView2CompositionController)
- [ ] Implement features of the WebView
    - [x] WebView2Environment
    - [x] WebView2Controller
    - [x] Interfaces of WebView2 like WebView2 Control for WinForm
    - [x] Custom Context Menu
    - [x] AppRegion support
    - [x] Resizing and Moving actions on Borderless Form
    - [ ] Embedded File Resources for WebResourceRequest & WebResourceResponse
    - [ ] Proxy File Resources for WebResourceRequest & WebResourceResponse

## 🔧 Installation

The release of WinFormedge library is not available yet and it will be on NuGet soon after the first release. if you want to try it, you can build the library by using the source code in this moment.

When the project is released, it will be available on NuGet. You can install it by using the NuGet Package Manager in Visual Studio.

    
## 🪄 Getting Started

### Create a WinForm Application by using default project template

**1. Replace initialization code by using WinFormedge application initialization procedure.**

You should use `FormedgeApp` instead of `Application` class to initialize your WinForm application in the `program.cs` file. The `FormedgeApp` class is a builder for creating a WinFormedge application. It provides methods for configuring the application and running it.

```csharp
using WinFormedge;

namespace MinimalExampleApp;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var app = FormedgeApp.CreateBuilder()
            .UseDevTools()
            .UseWinFormedgeApp<MyFormedgeApp>().Build();

        app.Run();
    }
}
```

When the `FormedgeApp` class is created, it will automatically initialize the WebView2 environment and run the message loop.


**2. Create a AppStartup class.**

The `AppStartup` class is the entry point of your WinFormedge application. It provides methods for configuring the application. You can override the `OnApplicationLaunched` method to perform any initialization tasks before the application starts.

And you must override the `OnApplicationStartup` method to create the main window of your application. If the `OnApplicationStartup` method returns values created by `StartupOptions` class, the `FormedgeApp` class will create the main window of your application. Otherwise if the `OnApplicationStartup` method returns `null` the application will be closed.

```csharp
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
```

You can do some staffs like User Login, User Settings, etc. in the `OnApplicationStartup` method to determine using which window to start the application. Andalso you can end the application by returning `null` if conditions are not met.

**3. Create a MainWindow class.**

The `MainWindow` class is the main window of your application. It inherits from the `Formedge` class. You can use the `Formedge` class to create a window with a WebView2 control.
```csharp
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

        Load += MyWindow_Load;
        DOMContentLoaded += MyWindow_DOMContentLoaded;
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
}
```

Codes above creates a `Formedge` window. By using `Url` property, you can set the initial URL of the window. Sets the `ExtendsContentIntoTitleBar` property to `true` to extend the WebView into the non-client area of the window to create a borderless window. You can also set the `Size` property to set the initial size of the window.

The `Load` event is raised when the window and WebView2 are ready to use. You can use this event to perform any initialization tasks that require the WebView2 control to be ready.

The `DOMContentLoaded` event is raised when the DOM content is loaded and ready to use. You can use this event to perform any tasks that require the DOM content to be loaded. As you can see in the example, you can use the `ExecuteScriptAsync` method to execute JavaScript code in the WebView2 control. The JavaScript code in the example sets the `appRegion` property of the header element to `drag`, which allows the user to drag the window by clicking and dragging the element on these rectangles.

**4. Run the application.**

You can run the application by pressing `F5` in Visual Studio. The application will create a window with a WebView2 control that displays the Bing homepage. 

![PREVIEW](./docs/preview.png)

You can drag the window by clicking and dragging the header element of the page and resize the window by dragging the edges of the window. 

## 📚 Documentation

The documentation for the WinFormedge project is not available yet. However, you can find the source code and examples in the `src` and `examples` folders. The source code is well-documented and provides examples of how to use the WinFormedge library right now.


## 🛠️ Contributing

Contributions are welcome! If you have any ideas, suggestions, or bug reports, please feel free to open an issue or submit a pull request.

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

## Acknowledgements

- [Microsoft WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/)
- [Chromium Embedded Framework](https://bitbucket.org/chromiumembedded/cef)
- [NanUI](https://github.com/XuanchenLin/NanUI)
- [WinFormium](https://github.com/XuanchenLin/WinFormium)