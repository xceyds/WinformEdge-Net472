using WinFormedge.HostForms;

namespace WinFormedge;

public sealed class DefaultWindowSettings : WindowSettings
{
    internal protected override bool HasSystemTitlebar => !ExtendsContentIntoTitleBar;
    internal protected override WindowProc? WndProc { get; set; }
    internal protected override WindowProc? DefWndProc { get; set; }

    public SystemBackdropType SystemBackdropType
    {
        get; set;
    } = SystemBackdropType.Auto;

    public bool SystemMenu
    {
        get; set;
    } = true;



    public bool ShowWindowDecorators { get; set; } = true;

    public bool ExtendsContentIntoTitleBar { get; set; } = false;

    public Padding WindowEdgeOffsets { get; set; } = Padding.Empty;
    public override bool Fullscreen
    {
        get => _form?.Fullscreen ?? false;
        set
        {
            if (_form is not null)
            {
                _form.Fullscreen = value;
            }
        }
    }


    private DefaultWindow? _form = null;

    internal protected override Form CreateHostWindow()
    {
        var form = _form = new DefaultWindow(this)
        {

        };

        return form;
    }


}
