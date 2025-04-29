using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WinFormedge;
partial class WebViewCore
{
    internal Control _hostControl;

    public Control Container
    {
        get
        {
            return _hostControl.TopLevelControl ?? _hostControl;
        }
    }

    private Control? _temporaryContainerControl;
    private void HostHandleCreated(object? sender, EventArgs e)
    {
        if (Container.RecreatingHandle)
        {
            if (_temporaryContainerControl == null) throw new NullReferenceException("Temporary container control is null.");

            Controller.ParentWindow = Container.Handle;

            _temporaryContainerControl.Dispose();
            _temporaryContainerControl = null;
        }
        else
        {
            CreateWebView2();
        }

        HandleSystemColorMode();
    }

    private void HandleSystemColorMode()
    {
        BOOL mode = FormedgeApp.Current.GetSystemColorMode() == SystemColorMode.Dark ? true : false;

        if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
        {
            unsafe
            {
                DwmSetWindowAttribute((HWND)Container.Handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, &mode, (uint)sizeof(BOOL)).ThrowOnFailure();
            }
        }
    }

    private void HostHandleDestroyed(object? sender, EventArgs e)
    {
        if (Container.RecreatingHandle)
        {
            _temporaryContainerControl = new Control();
            _temporaryContainerControl.CreateControl();
            Controller.ParentWindow = _temporaryContainerControl.Handle;
        }
    }

    internal bool HostWndProc(ref Message m)
    {
        var msg = (uint)m.Msg;
        switch (msg)
        {
            case WM_SETTINGCHANGE when m.LParam != 0:
                {
                    OnWmSettingChangeWithImmersiveColorSet(m.LParam);
                }
                break;
            case WM_ERASEBKGND:
                {
                    return Initialized;
                }
        }


        return false;
    }

    private void OnWmSettingChangeWithImmersiveColorSet(nint lParam)
    {
        const string IMMERSIVE_COLOR_SET = "ImmersiveColorSet";

        const int strlen = 255;

        var buffer = new byte[strlen];

        Marshal.Copy(lParam, buffer, 0, buffer.Length);


        var setting = Encoding.Unicode.GetString(buffer);

        setting = setting.Substring(0, setting.IndexOf('\0'));

        if (setting == IMMERSIVE_COLOR_SET)
        {
            HandleSystemColorMode();
        }

    }
}
