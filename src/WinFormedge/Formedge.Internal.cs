using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormedge;

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;

using WinFormedge.HostForms;
using WinFormedge.WebResource;

public abstract partial class Formedge
{

    private static readonly string FORMEDGE_MESSAGE_PASSCODE = Guid.NewGuid().ToString("N");

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

            var script = Properties.Resources.Formedge.Replace("{{FORMEDGE_MESSAGE_PASSCODE}}",FORMEDGE_MESSAGE_PASSCODE);

            if(OperatingSystem.IsWindowsVersionAtLeast(10,0,22000))
            {
                script = script.Replace("{{IS_SNAP_LAYOUTS_ENABLED}}", "true");
            }
            else
            {
                script = script.Replace("{{IS_SNAP_LAYOUTS_ENABLED}}", "false");
            }

            WebView.Browser.WebMessageReceived += CoreWebView2WebMessageReceived;
            WebView.Browser.AddScriptToExecuteOnDocumentCreatedAsync(script);

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

    private void CoreWebView2WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            var jsdoc = JsonDocument.Parse(e.WebMessageAsJson);
            if (jsdoc == null || jsdoc.RootElement.ValueKind != JsonValueKind.Object) return;

            if (jsdoc.RootElement.TryGetProperty("passcode", out var elPasscode) && jsdoc.RootElement.TryGetProperty("message", out var elMessage))
            {
                var passcode = elPasscode.GetString();
                var name = elMessage.GetString();

                if(passcode!=FORMEDGE_MESSAGE_PASSCODE) return;

                switch (name)
                {
                    case "FormedgeWindowCommand":
                        HandleWindowCommandJavaScript(jsdoc.RootElement);
                        break;
                    //case "FormedgeWindowSnapLayoutsRequired" when OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000):
                    //    HandleWindowSnapLayoutsRequired(jsdoc.RootElement);
                    //    break;
                }


            }
        }
        catch
        {

        }


    }

    //private bool _isSnapLayoutsRequired = false;

    //private void HandleWindowSnapLayoutsRequired(JsonElement jsonElement)
    //{
    //    if (!jsonElement.TryGetProperty("status", out var statusEl)) return;

    //    var status = statusEl.GetBoolean();

    //    _isSnapLayoutsRequired = status;

    //    if (status)
    //    {
    //        SendMessage((HWND)HostWindow.Handle, WM_NCMOUSEHOVER, (WPARAM)HTMAXBUTTON, MARCOS.FromPoint(Control.MousePosition));
    //    }
        

    //}

    private void HandleWindowCommandJavaScript(JsonElement jsonElement)
    {
        if (!jsonElement.TryGetProperty("command", out var elCommand)) return;

        var command = elCommand.GetString();


        switch (command)
        {
            case "minimize":
                WindowState = FormWindowState.Minimized;
                break;
            case "maximize":
                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    WindowState = FormWindowState.Maximized;
                }
                break;
            case "fullscreen":
                ToggleFullscreen();
                break;
            case "close":
                Close();
                break;

        }
    }

    public void ClearVirtualHostNameToEmbeddedResourcesMapping(EmbeddedFileResourceOptions options)
    {
        UnregisterWebResourceHandler(new EmbeddedFileResourceHandler(options));
    }

    public void ClearVirtualHostNameToFolderMapping(string hostName)
    {
        if (CoreWebView2 != null)
        {
            CoreWebView2.ClearVirtualHostNameToFolderMapping(hostName);
        }
    }

    public void RegisterWebResourceHander(WebResourceHandler resourceHandler)
    {
        WebView.RegisterWebResourceHander(resourceHandler);
    }

    public void SetVirtualHostNameToEmbeddedResourcesMapping(EmbeddedFileResourceOptions options)
    {
        RegisterWebResourceHander(new EmbeddedFileResourceHandler(options));
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

    public void UnregisterWebResourceHandler(WebResourceHandler resourceHandler)
    {
        WebView.UnregisterWebResourceHander(resourceHandler);
    }

    internal BrowserHostForm HostWindow { get; }
    internal WebViewCore WebView { get; }
    protected virtual void OnActivated(object? sender, EventArgs e)
    {
        Activated?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs e)
    {
    }

    protected virtual void OnDeactivate(object? sender, EventArgs e)
    {
        Deactivate?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnFormClosed(object? sender, FormClosedEventArgs e)
    {
        FormClosed?.Invoke(this, e);
    }

    protected virtual void OnFormClosing(object? sender, FormClosingEventArgs e)
    {
        FormClosing?.Invoke(this, e);
    }

    protected virtual void OnMove(object? sender, EventArgs e)
    {
        Move?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnResize(object? sender, EventArgs e)
    {
        Resize?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnResizeBegin(object? sender, EventArgs e)
    {
        ResizeBegin?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnResizeEnd(object? sender, EventArgs e)
    {
        ResizeEnd?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnShown(object? sender, EventArgs e)
    {
        Shown?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnVisibleChanged(object? sender, EventArgs e)
    {
        VisibleChanged?.Invoke(this, e);
    }

    private Action? _setVirtualHostNameToFolderMapping;

    private void OnActivatedCore(object? sender, EventArgs e)
    {
        if (WebView.Initialized)
        {
            WebView.Controller.MoveFocus(CoreWebView2MoveFocusReason.Programmatic);
        }

        OnActivated(sender, e);
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

    private void OnDocumentTitleChangedCore(object? sender, object e)
    {
        UpdateFormText();
    }

    private void OnStatusBarTextChangedCore(object? sender, object e)
    {
    }

    private class ContextMenuHelper
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
}