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
        HostControl = hostControl;
        Container.HandleCreated += HostHandleCreated;
        Container.HandleDestroyed += HostHandleDestroyed;
    }
}
