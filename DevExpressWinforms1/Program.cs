using DevExpress.XtraEditors;

namespace DevExpressWinforms1;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Views.MainForm());
    }
}
