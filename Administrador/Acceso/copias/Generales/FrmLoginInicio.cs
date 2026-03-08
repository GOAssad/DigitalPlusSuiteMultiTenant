using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Acceso.Clases.Datos.Generales;


namespace Acceso.Generales
{
	public partial class FrmLoginInicio : Global.Formularios.FrmLogin
	{
		GRALUsuariosRep UsuariosRep = new GRALUsuariosRep();

		public FrmLoginInicio()
		{
			InitializeComponent();
			//CargarVariablesDinamicas();

			textoUsuario.DataBindings.Add("Text", UsuariosRep, "sUsuarioID", true, DataSourceUpdateMode.OnPropertyChanged);
			textoPassword.DataBindings.Add("Text", UsuariosRep, "sPassword", true, DataSourceUpdateMode.OnPropertyChanged);
			//lblTituloLogin.Text = GRALVariablesGlobalesServicios.TraerValor("GRALEmpresaRazonSocial");
		}

		private void CargarVariablesDinamicas()
		{
			lblMensaje.Text = "Inicializando Variables Dinamicas...";
			GRALLoginDTO.CargarVariablesDinamicasLogin();
		}

		private void CargarVariablesGlobalesEstaticas(Datos.Model.GRALUsuarios oUsuario)
		{
			lblMensaje.Text = "Inicializando Variables Globales esenciales...";
			GRALLoginDTO.CargarVariablesGlobalesLogin(textoUsuario.Text.Trim(), oUsuario);
		}

		private void botonAceptar_Click(object sender, EventArgs e)
		{
			Acceso.Datos.Model.GRALUsuarios oUsuario = new Datos.Model.GRALUsuarios();
			oUsuario = UsuariosRep.logearse();
			if (UsuariosRep.Existe)
			{
				lblMensaje.Text = "Acceso Correcto...";
				CargarVariablesGlobalesEstaticas(oUsuario);
				this.Text = this.Text + " - " + sysGlobales.UsuarioLogeado;
				Entro = true;
				this.Close();
			}
			else
			{
				//MessageBox.Show("Intente Nuevamente!!!");
				Entro = false;
				lblMensaje.Text = "Usuario o Clave Incorrecta...";
			}
		}

		private void botonSalir_Click(object sender, EventArgs e)
		{
			
		}
	}
}
