namespace WinFormedge;
using System.Windows.Forms;

public abstract partial class Formedge
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class FormedgeHostObject
    {

        private readonly Formedge _formedge;

        public bool Activated { get; set; }
        public string WindowState => _formedge.Fullscreen ? "fullscreen" : _formedge.WindowState.ToString().ToLower();
        public bool HasTitleBar => _formedge.HasSystemTitlebar;

        public string FormedgeVersion => typeof(Formedge).Assembly.GetName().Version?.ToString() ?? string.Empty;

        public string ChromiumVersion => _formedge.WebView.Browser?.Environment.BrowserVersionString ?? string.Empty;

        public void Minimize()
        {

            _formedge.WindowState = FormWindowState.Minimized;
        }

        public void Maximize()
        {

            _formedge.WindowState = FormWindowState.Maximized;
        }

        public void Restore()
        {
            if (_formedge.Fullscreen)
            {
                _formedge.Fullscreen = false;
            }
            else
            {
                _formedge.WindowState = FormWindowState.Normal;
            }
        }

        public void Fullscreen()
        {
            _formedge.Fullscreen = true;
        }

        public void ToggleFullscreen()
        {
            _formedge.Fullscreen = !_formedge.Fullscreen;
        }

        public void Close()
        {
            _formedge.Close();
        }

        public void Activate()
        {
            if (_formedge.Fullscreen) return;

            _formedge.Activate();
        }

        internal FormedgeHostObject(Formedge formedge)
        {
            _formedge = formedge;

            Activated = _formedge.HostWindow.Focused;

            _formedge.Activated += (_, _) => Activated = true;
            _formedge.Deactivate += (_, _) => Activated = false;
        }
    }
}