namespace WinFormedge;

public sealed class AppCreationAction
{
    internal Form? Form { get; set; }

    internal Formedge? EdgeForm { get; set; }


    public AppCreationAction()
    {
    }

    internal void ConfigureAppContext(FormedgeApplicationContext appContext)
    {
        if (EdgeForm != null)
        {
            appContext.UseStartupForm(EdgeForm);
        }
        else if (Form != null)
        {
            appContext.UseStartupForm(Form);
        }
    }
}
