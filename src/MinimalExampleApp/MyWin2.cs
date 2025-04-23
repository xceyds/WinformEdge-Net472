using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WinFormedge;

namespace MinimalExampleApp;
public partial class MyWin2 : Formedge
{
    public MyWin2()
    {
        Url = "https://www.baidu.com";
        Icon = SystemIcons.WinLogo;
        Size = new Size(1920, 1080);
        StartPosition = FormStartPosition.CenterParent;

        Activated += MyWin2_Activated;
    }

    private void MyWin2_Activated(object? sender, EventArgs e)
    {
        Debug.WriteLine($"${nameof(MyWin2)} activated");
    }
}
