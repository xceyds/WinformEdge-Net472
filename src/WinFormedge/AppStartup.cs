using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormedge;
public abstract class AppStartup
{
    internal protected virtual bool OnApplicationLaunched(string[] args)
    {
        return true;
    }

    internal abstract protected AppCreationAction? OnApplicationStartup(StartupSettings options);

    internal protected virtual void OnApplicationException(Exception? exception = null)
    {
    }


    internal protected virtual void OnApplicationTerminated()
    {
    }

    internal protected virtual void ConfigureSchemeRegistrations(List<CoreWebView2CustomSchemeRegistration> customSchemeRegistrations)
    {
    }

    internal protected virtual void ConfigureAdditionalBrowserArgs(NameValueCollection additionalBrowserArgs)
    {
    }

}
