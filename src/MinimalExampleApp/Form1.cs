using WinFormedge.HostForms;

namespace MinimalExampleApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        StartPosition = FormStartPosition.CenterScreen;
        //ExtendsContentIntoTitleBar = true;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);


        var second = new Form2();


        second.Show(this);


        //TestFullScreen();
    }

    //private async void TestFullScreen()
    //{
    //    var rnd = new Random();
    //    while (true)
    //    {
    //        await Task.Delay(rnd.Next(500,5000));

    //        Fullscreen = !Fullscreen;
    //    }
    //}

    //private void button1_Click(object sender, EventArgs e)
    //{
    //    Fullscreen = true;

    //}

    //private void button2_Click(object sender, EventArgs e)
    //{
    //    Fullscreen = false;

    //}
}
