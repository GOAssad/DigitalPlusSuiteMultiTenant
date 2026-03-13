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
		public static string sysUsuarioEmail = string.Empty;
		public static string sysUsuarioRol = string.Empty;
		public static LicenseManager LicMgr { get; private set; }

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Login contra Identity del Portal MT
			using (var frmLogin = new Generales.FrmLoginAdmin())
			{
				if (frmLogin.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				{
					return; // Cerro el login sin autenticarse
				}
				sysUsuario = frmLogin.UsuarioNombre;
				sysUsuarioEmail = frmLogin.UsuarioEmail;
				sysUsuarioRol = frmLogin.UsuarioRol;
			}

			// Inicializar licencia
			LicMgr = new LicenseManager("", "Administrador");
			var licResult = LicMgr.ValidateAtStartup(ContarLegajos());
			// No bloquear acceso por licencia: el admin siempre debe poder entrar
			// para gestionar/activar la licencia desde el menu

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
