using System;
using System.Configuration;
using System.Windows.Forms;
using Acceso.Ventas;
using DigitalPlus.Licensing;
using Global.Datos;

namespace Acceso
{
	static class Program
	{
		public static string sysUsuario = string.Empty;
		public static LicenseManager LicMgr { get; private set; }

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
			// En modo debug, saltear validacion de licencia
			Application.Run(new FrmMainMenu());
#else
			var apiKey = ConfigurationManager.AppSettings["ProvisioningApiKey"] ?? "";
			LicMgr = new LicenseManager(apiKey, "Administrador");
			int legajos = ContarLegajos();
			var result = LicMgr.ValidateAtStartup(legajos);

			if (result.IsBlocked)
			{
				using (var frm = new FrmLicenseBlocked(result, LicMgr))
				{
					if (frm.ShowDialog() != DialogResult.OK)
						return;
				}
			}

			// Alerta de vencimiento proximo
			var diasRestantes = LicMgr.GetDaysRemaining();
			if (diasRestantes.HasValue && diasRestantes.Value <= 7 && diasRestantes.Value >= 0)
			{
				MessageBox.Show(
					string.Format("Su licencia vence en {0} dia(s).\nContacte a soporte para renovar.", diasRestantes.Value),
					"Aviso de vencimiento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			Application.Run(new FrmMainMenu());
#endif
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

	public static class ObjGlobal
    {
		//public static Acceso.Clases.Datos.RRHH.RRHHLegajos oLegajo = new Clases.Datos.RRHH.RRHHLegajos();
		public static Acceso.Clases.Datos.Generales.GRALUsuarios oUsuario = new Clases.Datos.Generales.GRALUsuarios();
	}
}
