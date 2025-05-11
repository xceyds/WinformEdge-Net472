# WinFormedge 计算器示例

## 关于 WinFormedge

感谢关注 WinFormedge 项目。WinFormedge 是一款基于 Microsoft WebView2 构建的开源 .NET 库，它使开发者能够利用 HTML、CSS 和 JavaScript 创建现代化、视觉吸引力强的 WinForm 应用程序。通过 WinFormedge，您可以轻松地将 Web 技术集成到 WinForm 项目中，实现丰富且交互性强的用户界面。

你可以导航到 [WinFormedge](https://github.com/XuanchenLin/WinFormedge) 的仓库了解其原理。

## 关于本示例

本仓库代码演示了使用 WinFormedge 创建一个简单的 WinForm 应用程序，主要展现 WinFormedge 的基本用法和功能。示例中包含了一个简单的计算器界面，用户可以进行基本的加减乘除运算。

项目使用 WinFormedge 创建一个无边框窗体，窗体的外观和行为都通过 HTML、CSS 和 JavaScript 来实现。Web 前端代码在项目文件夹的 `wwwroot` 目录中。通过 WinFormedge 的扩展，这个文件夹作为应用程序的资源嵌入到程序集中。

本示例使用 Visual Studio 2022 基于 .NET 8.0 开发，所以你需要安装 .NET 8.0 SDK 来编译和运行这个示例。

![PREVIEW](./preview.png)

## 代码说明

**1. 初始化**

因为 WinFormedge 对 WinForm 和 WebView2 的初始化进行了封装，因此在 `Program.cs` 文件中，您需要使用 WinFormedge 的构造器来替代原有 `Main` 方法中的初始化逻辑。

```C#
using WinFormedge;

namespace FormedgeCalculator;

internal static class Program
{

    [STAThread]
    static void Main()
    {
        var app = FormedgeApp.CreateAppBuilder()
            .UseModernStyleScrollbar()
            // UseWinFormedgeApp 使用 AppStartup 的衍生类来管理应用程序的生命周期
            .UseWinFormedgeApp<CalculatorApp>()
#if DEBUG
            // 调试模式下启用 DevTools，您可以在浏览器界面点击右键选择“检查”来打开 DevTools
            .UseDevTools()
#endif
            // 创建 FormedgeApp 实例
            .Build();

        // 运行应用程序消息循环
        app.Run();
        
    }
}
```

在以上代码中，我们使用 `FormedgeApp.CreateAppBuilder()` 创建一个应用程序构建器，并通过 `UseWinFormedgeApp<CalculatorApp>()` 方法指定了应用程序的启动配置类为 `CalculatorApp`。

**2. 启动配置**

在 `CalculatorApp.cs` 文件中，我们定义了 `CalculatorApp` 类，继承自 `AppStartup`。在这个类中，我们可以配置应用程序的启动行为和资源，其中您需要在抽象方法 `OnApplicationStartup` 的实现中指定启动窗体。

```C#
using WinFormedge;

namespace FormedgeCalculator;

internal class CalculatorApp : AppStartup
{
    protected override bool OnApplicationLaunched(string[] args)
    {
        ApplicationConfiguration.Initialize();

        return true;
    }
    protected override AppCreationAction? OnApplicationStartup(StartupOptions options)
    {
        return options.UseMainWindow(new MainCalculatorWindow());
    }
}
```

在 `OnApplicationLaunched` 方法中我们可以进行一些应用程序的初始化操作，原本应在 `Main` 入口方法中执行的各种初始化代码需要移动到这个方法中执行。在 `OnApplicationStartup` 方法中，我们使用 `options.UseMainWindow` 来指定窗体 `MainCalculatorWindow` 的作为启动窗体实例。

**3. 窗体**

在 `MainCalculatorWindow.cs` 文件中，我们定义了 `MainCalculatorWindow` 类，继承自 `Formedge`。在这个类中，我们可以配置窗体的外观并绑定嵌入到程序集中的 Web 资源，并为这些资源分配合适的网址。

```C#
using WinFormedge;

namespace FormedgeCalculator;

internal class MainCalculatorWindow : Formedge
{
    public MainCalculatorWindow()
    {
        Resizable = false;
        Maximizable = false;
        WindowText = "计算器";
         // 扩展内容到标题栏，实现无边框效果
        ExtendsContentIntoTitleBar = true;
        Icon = new Icon(new MemoryStream(Properties.Resources.AppIcon));
        Size = new Size(340, 480);
        // 指定窗体背景模糊效果，这个效果需要 Windows 10 1903 及以上版本，您可以根据需要修改来体验不同的效果
        WindowSystemBackdropType = SystemBackdropType.BlurBehind;
        // 设置网页背景色，这里默认为白色。使用默认值时，您将无法看到背景模糊效果
        DefaultBackgroundColor = Color.Transparent;

        // 从程序集的嵌入资源中注册 Web 资源，此方法只是创建了 Web 中 Url 到程序集资源的一个资源映射关系，它并不是一个真实的 Web 服务器，所以你不能直接在当前进程外中访问这个 Url
        SetVirtualHostNameToEmbeddedResourcesMapping(new WinFormedge.WebResource.EmbeddedFileResourceOptions
        {
            Scheme = "https", // 指定协议
            HostName = "embeddedassets.local", // 设置一个虚假的主机名或者也可以是一个 Url 目录
            ResourceAssembly = typeof(MainCalculatorWindow).Assembly, // 指定资源所处的程序集
            WebResourceContext = Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.All,
            DefaultFolderName = "wwwroot" // 指定资源的根目录，因为我们在项目中使用了 wwwroot 作为 Web 资源的根目录，所以这里需要指定，否则将会从程序集的根目录中查找资源
        });

        // 通过 SetVirtualHostNameToEmbeddedResourcesMapping 方法创建的 Url 映射关系，您可以在 Web 中使用这个 Url 来访问嵌入到程序集中的资源
        Url = "https://embeddedassets.local"; 
    }
}

```

在 `MainCalculatorWindow` 类中，您可以根据需求设置了一些窗体的属性，例如示例中的 `Resizable`、`Maximizable`、`WindowText`、`Icon` 等。需要特别提醒的是 `WindowSystemBackdropType` 属性并是所有版本的 Windows 都支持，在不支持的 Windows 系统中将会使用默认的外观来呈现窗体。

另外我们使用 `SetVirtualHostNameToEmbeddedResourcesMapping` 方法来注册程序集中的文件作为 Web 资源，本示例的所使用的 Web 资源位于 `wwwroot` 文件夹中，并且该目录中的所有文件都设置为嵌入资源，如果忘记设置为嵌入资源，WebView2 将无法访问这些文件并返回 404 错误。

**4. Web 资源**

在 `wwwroot` 文件夹中，我们使用 HTML、CSS 和 JavaScript 来实现计算器的界面和功能。其中特别指出的是，在示例页面中右上角有两个按钮，分别是“最小化”和“关闭”按钮，如果您使用浏览器查看前端页面，那么点击这两个按钮将不会有任何效果，因为它们是通过 WinFormedge 特有的属性 `app-command` 属性来实现窗体的最小化和关闭的动作。

```html
<div id="command-buttons">
    <div id="miniBtn" app-command="minimize"></div>
    <div id="closeBtn" app-command="close"></div>
</div>
```

如上面的代码所示，`app-command` 属性的值可以是 `minimize`、`maximize`、`restore` 和 `close`，分别对应窗体的最小化、最大化、还原和关闭操作。这个属性只能在 WinFormedge 中使用，在浏览器中不会有任何效果。


**5. 运行**

在确保以上要点都已完成后，您可以在 Visual Studio 中运行项目。运行后，您将看到一个无边框的计算器窗体，您可以使用鼠标拖动窗体的计算结果区域来移动窗体，或者使用右上角的按钮来最小化和关闭窗体。

## WinFormedge vs WinFormium

WinFormium 是我维护的另一个项目，相同的是它们都是通过嵌入浏览器界面来实现 WinForm 应用程序的界面简单化工作，而不同的是 WinFormium 基于 Chromium Embedded Framework (CEF) 实现，WinFormedge 则是基于 Microsoft WebView2 实现。因此 WinFormium 发布时需要连同 CEF 的运行时一起分发从而增大了应用程序的体积，而 WinFormedge 则是直接使用 Windows 系统自带的 WebView2 运行时，所以它的体积更小，且不需要额外安装运行时。另外 WinFormium 的 API 设计是基于 CEF 的设计理念，而 WinFormedge 则是基于 WebView2 的设计理念，因此两者的 API 设计和使用方式有很大的不同。

## 感谢

目前 WinFormedge 仍处于开发阶段，这个示例只是展示了早期阶段 WinFormedge 的基本功能，并不代表最终的 API 设计和功能实现。我们会在后续版本中不断改进和完善 WinFormedge 的功能和性能。因此 API 可能在后期版本中会有所更改，您可以在 GitHub 上查看最新的 API 文档和示例代码。

如果您在使用过程中遇到问题，请随时在 GitHub 上提交问题或建议。您也可以通过提交 Pull Request 来贡献代码，我们欢迎任何形式的贡献和反馈。另外您还可以加入 NanUI 的 QQ 群（目前 WinFormedge 还没有单独的 QQ 群）来与我们交流和讨论， 群号：**521854872**，进群密码 `nanui`。

感谢您的支持和关注，我们期待与您一起探索 WinFormedge 的更多可能性！


