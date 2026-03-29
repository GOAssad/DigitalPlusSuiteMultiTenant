using System;
using System.Windows.Forms;
using Acceso.Ventas;

namespace Acceso
{
	static class Program
	{
		public static string sysUsuario = string.Empty;
		public static string sysUsuarioEmail = string.Empty;
		public static string sysUsuarioRol = string.Empty;

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

			Application.Run(new FrmMainMenu());
		}
    }

	public static class ObjGlobal
    {
		//public static Acceso.Clases.Datos.RRHH.RRHHLegajos oLegajo = new Clases.Datos.RRHH.RRHHLegajos();
		public static Acceso.Clases.Datos.Generales.GRALUsuarios oUsuario = new Clases.Datos.Generales.GRALUsuarios();
	}
}
