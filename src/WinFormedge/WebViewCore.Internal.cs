using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormedge;
partial class WebViewCore
{
    
    public WebViewCore(Control hostControl)
    {
        _hostControl = hostControl;
        Container.HandleCreated += HostHandleCreated;
        Container.HandleDestroyed += HostHandleDestroyed;
    }

    public void Close()
    {
        if (Initialized)
        {
            Controller.Close();
            _controller = null;
        }
    }
}
