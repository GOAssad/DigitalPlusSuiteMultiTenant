using System;
using System.Windows.Forms;

namespace Acceso
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Acceso.uAreu.FrmFichar());
        }
    }
}
