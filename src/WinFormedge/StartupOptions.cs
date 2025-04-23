namespace WinFormedge;

public sealed class StartupOptions
{


    public StartupOptions()
    {


    }

    public AppCreationAction UseMainWindow(Formedge formium)
    {
        return new AppCreationAction()
        {
            EdgeForm = formium
        };
    }

    public AppCreationAction UseMainWindow(Form form)
    {
        return new AppCreationAction()
        {
            Form = form
        };
    }

    public AppCreationAction StartWitoutWindow()
    {
        return new AppCreationAction() { };
    }



}
