using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormedge;
internal partial class WebViewCore
{
    public string Url
    {
        get => Browser?.Source ?? ABOUT_BLANK;
        set
        {
            if (Browser != null)
            {
                Browser.Navigate(value);
            }
            else
            {
                _defferedUrl = value;
            }
        }
    }




}
