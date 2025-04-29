using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WinFormedge;
public partial class Formedge
{
    protected virtual void OnLoad()
    {
        Load?.Invoke(this, EventArgs.Empty);


    }



    public event EventHandler? Load;
}

