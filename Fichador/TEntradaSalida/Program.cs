using System;
using System.Configuration;
using System.Windows.Forms;
using DigitalPlus.Licensing;
using Global.Datos;

namespace Acceso
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Licencia: en multi-tenant la activacion se hace desde el instalador
            var licMgr = new LicenseManager("", "Fichador");

            Application.Run(new Acceso.uAreu.FrmFichar(licMgr));
        }

        internal static int ContarLegajos()
        {
            try
            {
                var dt = SQLServer.Ejecutar("SELECT COUNT(*) FROM Legajo WHERE EmpresaId = " + TenantContext.EmpresaId);
                if (dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch { }
            return 0;
        }
    }
}
