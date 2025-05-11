<p align="center">
    <img src="./artworks/WinFormedgeLogo.png" width="144" />
</p>
<h1 align="center">WinFormedge 项目</h1>
<p align="center">这是一个基于 Microsoft WebView2 的 .NET 库，可轻松使用 HTML、CSS 和 JavaScript 构建强大的 WinForm 应用程序。</p>

## WinFormedge

![GitHub](https://img.shields.io/github/license/XuanchenLin/WinFormedge)
![Nuget](https://img.shields.io/nuget/v/WinFormedge)
![Nuget](https://img.shields.io/nuget/dt/WinFormedge)
[![Build](https://github.com/XuanchenLin/WinFormedge/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/XuanchenLin/WinFormedge/actions/workflows/dotnet-desktop.yml)

For `English`, please click [[HERE](README.md)] to switch.

## ✨ 简介

**WinFormedge** 是一个基于 **Microsoft WebView2** 的开源 .NET 库，使开发者能够使用 **HTML、CSS 和 JavaScript** 创建现代化、视觉吸引人的 WinForms 应用程序。通过 WinFormedge，您可以无缝地将 Web 技术集成到 WinForm 项目中，实现丰富且交互性强的用户界面。

WinFormedge 的灵感来源于我维护的另一个项目：**[WinFormium](https://github.com/xuanchenlin/NanUI)**（也称为 **NanUI**）。与 WinFormedge 类似，WinFormium 允许开发者使用 Web 技术构建 WinForms 应用程序，但不同于 WinFormedge，它依赖于 **Chromium Embedded Framework (CEF)**。

![预览](./docs/preview1.png)

## 🖥️ 系统要求

**开发环境：**

- Visual Studio 2022
- .NET 8.0 或更高版本

**部署环境：**

- Windows 10 1903 或更高版本

这是一个 **仅支持 Windows** 的库，不兼容其他操作系统。

最低支持的操作系统是 Windows 10 版本 1903（2019 年 5 月更新）。某些功能（如 SystemBackdrop 和 SystemColorMode）仅在 Windows 11 上可用。

## 🧩 更新日志

WinFormedge 项目的更新日志可在 [CHANGELOG.md](./CHANGELOG.md) 文件中查看。其中包含项目的详细变更记录、错误修复和新功能。更新日志会定期更新以反映项目的最新动态。

## 🔧 安装

WinFormedge 库尚现在已在 github 设置了自动化 nuget 发布，所以您能够从 nuget 找到最新版的 WinFormedge 发行版。使用 NuGet 包管理器或者任意 NuGet 管理工具搜索并安装 `WinFormedge` 即可。

## 🪄 快速入门

首先，您需要使用默认项目模板创建一个 WinForm 应用程序。

### 1. 使用 WinFormedge 应用程序初始化流程替换默认初始化代码

在 `program.cs` 文件中，您应使用 `FormedgeApp` 替代 `Application` 类来初始化 WinForm 应用程序。`FormedgeApp` 类是用于创建 WinFormedge 应用程序的构建器，提供了配置和运行应用程序的方法。

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

当 `FormedgeApp` 类创建后，它将自动初始化 WebView2 环境并运行消息循环。

### 2. 创建 AppStartup 类

`AppStartup` 类是 `WinFormedge` 应用程序的入口点，提供了配置应用程序的方法。您可以重写 OnApplicationLaunched 方法以在应用程序启动前执行初始化任务。

您必须重写 `OnApplicationStartup` 方法来创建应用程序的主窗口。当该方法返回由 `StartupOptions` 类生成的值时，`FormedgeApp` 类将创建应用程序主窗口；若方法返回 `null`，则应用程序将直接关闭。

```C#
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

您可以在 `OnApplicationStartup` 方法中执行一些操作，例如用户登录、用户设置等，以决定使用哪个窗口启动应用程序。此外，如果条件不满足，您还可以通过返回 `null` 来终止应用程序。

### 3. 创建 MainWindow 类

`MyWindow` 类是应用程序的主窗口，继承自 `Formedge` 类。您可以使用 `Formedge` 类创建一个带有 WebView2 的窗口。

```C#
using WinFormedge;
using Microsoft.Web.WebView2.Core;

namespace MinimalExampleApp;
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
        // 窗口和 WebView2 已准备就绪
    }

    private void MyWindow_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
    {
        // DOM 内容已加载完成
        ExecuteScriptAsync(""""
(()=>{
const headerEl = document.querySelector("#hdr");
headerEl.style.appRegion="drag";
})();
        """");
    }

    protected override WindowSettings ConfigureHostWindowSettings(HostWindowBuilder opts)
    {
        // 配置主窗口设置
        var win = opts.UseDefaultWindow();

        win.ExtendsContentIntoTitleBar = true;
        win.MinimumSize = new Size(960, 480);
        win.Size = new Size(1280, 800);
        win.SystemBackdropType = SystemBackdropType.MicaAlt;
        win.AllowFullScreen = true;

        return win;
    }
}
```

上述代码创建了一个 `Formedge` 窗口。通过使用 `Url` 属性，您可以设置窗口的初始 URL。

默认窗口属性可以在构造函数中设置。对于窗口的特殊样式属性，您需要重写 `Formedge` 类的 `ConfigureHostWindowSettings` 方法，并使用其 `HostWindowBuilder` 参数来确定将采用哪种窗口样式，以及配置该窗口具有的特殊样式。例如，在示例代码中，通过使用 `HostWindowBuilder` 参数的 `UseDefaultWindow` 方法，您可以指示 Formedge 创建一个默认窗口，并设置其 `ExtendsContentIntoTitleBar` 属性以实现无边框效果。

当窗口和 WebView2 准备就绪时，会触发 `Load` 事件。您可以使用此事件执行任何需要 WebView2 控件准备就绪的初始化任务。

当 DOM 内容加载完成并准备就绪时，会触发 `DOMContentLoaded` 事件。您可以使用此事件执行任何需要 DOM 内容加载完成的任务。如示例所示，您可以使用 `ExecuteScriptAsync` 方法在 WebView2 控件中执行 JavaScript 代码。示例中的 JavaScript 代码将 header 元素的 `appRegion` 属性设置为 `drag`，这允许用户通过点击并拖动这些矩形上的元素来移动窗口。

### 4. 运行应用程序

您可以在 Visual Studio 中按 F5 运行应用程序。应用程序将创建一个带有 WebView2 控件的窗口，并显示必应首页。

![PREVIEW](./docs/preview.png)

## 📚 文档

WinFormedge 项目的文档目前尚未提供。不过您可以在 `src` 和 `examples` 文件夹中找到源代码和示例。当前源代码已包含完善的注释说明，并提供了使用 WinFormedge 库的示例。

## 🛠️ 贡献指南

我们欢迎各种贡献！如果您有任何想法、建议或错误报告，请随时提交 issue 或 pull request。

## 📄 许可证

本项目采用 MIT 许可证，详情请参阅 [LICENSE](./LICENSE) 文件。

## 🧭 致谢

- [Microsoft WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/)
- [Chromium Embedded Framework](https://bitbucket.org/chromiumembedded/cef)
- [NanUI](https://github.com/XuanchenLin/NanUI)
- [WinFormium](https://github.com/XuanchenLin/WinFormium)
