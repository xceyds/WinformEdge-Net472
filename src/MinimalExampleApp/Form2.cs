using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinimalExampleApp;
public partial class Form2 : Form
{
    public Form2()
    {
        InitializeComponent();

        Shown += Form2_Shown;

        HandleCreated += Form2_HandleCreated;
    }

    private void Form2_HandleCreated(object? sender, EventArgs e)
    {
        var p = Parent;
        var o = Owner;
    }

    private void Form2_Shown(object? sender, EventArgs e)
    {
        var p = Parent;
        var o = Owner;
    }
}
