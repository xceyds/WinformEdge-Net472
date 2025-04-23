namespace WinFormedge;

internal class FormedgeApplicationContext : ApplicationContext
{

    internal enum RunningType
    {
        None,
        Form,
        Formium
    }

    Formedge? _formium;

    Form? _form;


    internal RunningType RunningOn => _formium == null && _form == null ? RunningType.None : _formium != null ? RunningType.Formium : RunningType.Form;


    public void UseStartupForm(Formedge formium)
    {

        _form = null;
        _formium = formium;
        MainForm = formium.HostWindow;

    }



    public void UseStartupForm(Form form)
    {
        _formium = null;
        _form = form;
        MainForm = form;

    }

    public IWin32Window? MainWindowHandle => RunningOn switch
    {
        RunningType.Form => _form,
        RunningType.Formium => _formium,
        _ => null
    };


}
