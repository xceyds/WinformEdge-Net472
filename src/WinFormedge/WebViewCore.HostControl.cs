using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormedge;
partial class WebViewCore
{
    internal Control HostControl { get; }


    //private bool _patchResizingIfNeeded = false;

    internal Control Container
    {
        get
        {
            return HostControl.TopLevelControl ?? HostControl;
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
        }


        return false;
    }


    //private void OnWmSizing(ref Message m)
    //{
    //    var direction = (uint)m.WParam;

    //    GetWindowRect((HWND)m.HWnd, out var rect);

    //    var windowRect = System.Drawing.Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);

    //    var proposedRect = Marshal.PtrToStructure<Rectangle>(m.LParam);

    //    if ((direction == WMSZ_LEFT || direction == WMSZ_TOP || direction == WMSZ_TOPLEFT) && (proposedRect.Top < windowRect.Top || proposedRect.Left < windowRect.Left))
    //    {
    //        if (Initialized)
    //        {
    //            var dx = Math.Abs(proposedRect.X - windowRect.X);
    //            var dy = Math.Abs(proposedRect.Y - windowRect.Y);
    //            Debug.WriteLine($"px: {proposedRect.X}, py: {proposedRect.Y} ox:{windowRect.X} oy:{windowRect.Y}");

    //            Controller.Bounds = new Rectangle(dx, dy, Controller.Bounds.Width, Controller.Bounds.Height);
    //        }

    //    }
    //}



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
