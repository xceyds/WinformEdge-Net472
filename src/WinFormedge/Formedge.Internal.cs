using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormedge;

using System.Windows.Forms;

using WinFormedge.HostForms;
using WinFormedge.WebResource;

public abstract partial class Formedge
{

    class ContextMenuHelper
    {
        public static bool IsRequiredItem(int commandId)
        {
            if (FormedgeApp.Current.EnableDevTools && commandId == 50162)
            {
                return true;
            }

            return commandId >= 50150 && commandId <= 50157 && commandId != 50154 && commandId != 50155;
        }
    }

    internal BrowserHostForm HostWindow { get; }

    internal WebViewCore WebView { get; }

    private Action? _setVirtualHostNameToFolderMapping;


    public Formedge()
    {
        HostWindow = new BrowserHostForm();

        WebView = new WebViewCore(HostWindow);

        WebView.WebViewCreated += (_, _) =>
        {
            if (WebView.Browser == null) throw new InvalidOperationException();

            WebView.Controller.DefaultBackgroundColor = DefaultBackgroundColor;

            WebView.Browser.ContentLoading += OnContentLoading;
            WebView.Browser.NavigationStarting += OnNavigationStarting;
            WebView.Browser.NavigationCompleted += OnNavigationCompleted;
            WebView.Browser.DOMContentLoaded += OnDOMContentLoaded;
            WebView.Browser.WebMessageReceived += OnWebMessageReceived;

            WebView.Controller.GotFocus += OnGotFocus;
            WebView.Controller.LostFocus += OnLostFocus;

            WebView.ConfigureSettings += ConfigureWebView2Settings;

            WebView.Browser.ContextMenuRequested += OnContextMenuRequestedCore;
            WebView.Browser.DocumentTitleChanged += OnDocumentTitleChangedCore;
            WebView.Browser.StatusBarTextChanged += OnStatusBarTextChangedCore;

            WebView.Browser.Settings.IsNonClientRegionSupportEnabled = HostWindow.ExtendsContentIntoTitleBar || HostWindow.Popup;

            _setVirtualHostNameToFolderMapping?.Invoke();

            OnLoad();
        };

        HostWindow.Activated += OnActivatedCore;
        HostWindow.Deactivate += OnDeactivate;
        HostWindow.ResizeBegin += OnResizeBegin;
        HostWindow.Resize += OnResize;
        HostWindow.ResizeEnd += OnResizeEnd;
        HostWindow.Move += OnMove;
        HostWindow.Shown += OnShown;
        HostWindow.VisibleChanged += OnVisibleChanged;
        HostWindow.FormClosing += OnFormClosing;
        HostWindow.FormClosed += OnFormClosed;

        HostWindow.OnWindowProc += WebView.HostWndProc;
        HostWindow.OnWindowProc += WndProc;

        HostWindow.OnDefWindowProc += DefWndProc;
    }

    private void OnActivatedCore(object? sender, EventArgs e)
    {
        if (WebView.Initialized)
        {
            WebView.Controller.MoveFocus(CoreWebView2MoveFocusReason.Programmatic);
        }

        OnActivated(sender, e);
    }

    protected virtual void OnFormClosed(object? sender, FormClosedEventArgs e)
    {
        FormClosed?.Invoke(this, e);
    }

    protected virtual void OnFormClosing(object? sender, FormClosingEventArgs e)
    {
        FormClosing?.Invoke(this, e);
    }

    protected virtual void OnVisibleChanged(object? sender, EventArgs e)
    {
        VisibleChanged?.Invoke(this, e);
    }

    protected virtual void OnShown(object? sender, EventArgs e)
    {
        Shown?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnMove(object? sender, EventArgs e)
    {
        Move?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnResizeEnd(object? sender, EventArgs e)
    {
        ResizeEnd?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnResize(object? sender, EventArgs e)
    {
        Resize?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnResizeBegin(object? sender, EventArgs e)
    {
        ResizeBegin?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnDeactivate(object? sender, EventArgs e)
    {
        Deactivate?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnActivated(object? sender, EventArgs e)
    {
        Activated?.Invoke(this, EventArgs.Empty);
    }

    private void OnContextMenuRequestedCore(object? sender, CoreWebView2ContextMenuRequestedEventArgs e)
    {
        var editingItems = new List<CoreWebView2ContextMenuItem>();


        for (int i = 0; i < e.MenuItems.Count; i++)
        {
            var item = e.MenuItems[i];

            if (ContextMenuHelper.IsRequiredItem(item.CommandId) || item.Kind == CoreWebView2ContextMenuItemKind.Separator)
            {
                // 避免连续添加多个分隔符
                if (item.Kind == CoreWebView2ContextMenuItemKind.Separator)
                {
                    if (editingItems.Count == 0 || editingItems[^1].Kind != CoreWebView2ContextMenuItemKind.Separator)
                    {
                        editingItems.Add(item);
                    }
                }
                else
                {
                    editingItems.Add(item);
                }
            }
        }

        if (editingItems.Count > 0 && editingItems[0].Kind == CoreWebView2ContextMenuItemKind.Separator)
        {
            editingItems.RemoveAt(0);
        }

        if (editingItems.Count > 0 && editingItems.LastOrDefault()?.Kind == CoreWebView2ContextMenuItemKind.Separator)
        {
            editingItems.RemoveAt(editingItems.Count - 1);
        }



        e.MenuItems.Clear();

        foreach (var item in editingItems)
        {
            e.MenuItems.Add(item);
        }

        OnContextMenuRequested(sender, e);
    }

    protected virtual void OnContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs e)
    {
        
    }

    private void OnStatusBarTextChangedCore(object? sender, object e)
    {
    }

    private void OnDocumentTitleChangedCore(object? sender, object e)
    {
        UpdateFormText();
    }

    public void RegisterWebResourceHander(WebResourceHandler resourceHandler)
    {
        WebView.RegisterWebResourceHander(resourceHandler);
    }

    public void UnregisterWebResourceHandler(WebResourceHandler resourceHandler)
    {
        WebView.UnregisterWebResourceHander(resourceHandler);
    }

    public void SetVirtualHostNameToFolderMapping(string hostName, string folderPath, CoreWebView2HostResourceAccessKind accessKind)
    {

        if (CoreWebView2 != null)
        {
            CoreWebView2.SetVirtualHostNameToFolderMapping(hostName, folderPath, accessKind);
        }
        else
        {
            _setVirtualHostNameToFolderMapping += () => CoreWebView2!.SetVirtualHostNameToFolderMapping(hostName, folderPath, accessKind);
        }
        
    }

}


