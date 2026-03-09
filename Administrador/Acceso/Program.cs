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

			// Licencia: en multi-tenant la activacion se hace desde el instalador,
			// no se valida por app. Se mantiene LicMgr para info en UI.
			Application.Run(new FrmMainMenu());
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
