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

            // Licencia: validar al inicio (carga cache, intenta heartbeat)
            var licMgr = new LicenseManager("", "Fichador");
            var licResult = licMgr.ValidateAtStartup(ContarLegajos());

            if (licResult.IsBlocked)
            {
                // Mostrar formulario de activacion (permite ingresar codigo)
                using (var frm = new FrmLicenseBlocked(licResult, licMgr))
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                        return; // Usuario cancelo -> salir
                }
            }

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
