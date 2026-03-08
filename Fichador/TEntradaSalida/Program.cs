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

            LicenseManager licMgr = null;

#if DEBUG
            // En modo Debug, saltar validacion de licencia para desarrollo
            licMgr = new LicenseManager("", "Fichador");
#else
            var apiKey = ConfigurationManager.AppSettings["ProvisioningApiKey"] ?? "";
            licMgr = new LicenseManager(apiKey, "Fichador");
            int legajos = ContarLegajos();
            var result = licMgr.ValidateAtStartup(legajos);

            if (result.IsBlocked)
            {
                using (var frm = new FrmLicenseBlocked(result, licMgr))
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                        return;
                }
            }

            // Alerta de vencimiento proximo
            var diasRestantes = licMgr.GetDaysRemaining();
            if (diasRestantes.HasValue && diasRestantes.Value <= 7 && diasRestantes.Value >= 0)
            {
                MessageBox.Show(
                    string.Format("Su licencia vence en {0} dia(s).\nContacte a soporte para renovar.", diasRestantes.Value),
                    "Aviso de vencimiento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
#endif

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
