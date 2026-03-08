using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Acceso.Clases.Datos.Generales;

namespace Acceso.Generales
{
	public partial class FrmUsuarios : Global.Formularios.FrmEntidadesBase
	{

		bool yapaso = false;
		bool tienegp = false;

		GRALUsuariosRep oUsuario = new GRALUsuariosRep();

		public FrmUsuarios()
		{
			InitializeComponent();
			textoEtiquetaEmail.DataBindings.Add("Valor", oUsuario, "sEmail", true, DataSourceUpdateMode.OnValidation);
		}

		private void TieneGp()
		{
			tienegp = bool.Parse(
				Acceso.Clases.Datos.Generales.GRALVariablesGlobalesServicios
				.TraerValor("GRALCompatibilidadDynamics"));

			if (!tienegp) return;

			// tiene activado GP. 
			comboUsuarioGP.Visible = true;
			LLenoComboUsuarios();


			yapaso = true;

		}

		private void LLenoComboUsuarios()
		{
			string dbGP =
			Acceso.Clases.Datos.Generales.GRALVariablesGlobalesServicios.TraerValor("DYGPBasedeDatos").Trim();

			string query = "SELECT name FROM " + dbGP +
				   "..SYSUSERS WHERE altuid IS NULL and sid is not null ORDER BY name";

			comboUsuarioGP.Visible = true;
			etiquetaGP.Visible = true;
			comboUsuarioGP.DataSource = Global.Datos.SQLServer.EjecutarParaSoloLectura(query);
			comboUsuarioGP.DisplayMember = "name";
			comboUsuarioGP.ValueMember = "name";

		}

		private void FrmUsuarios_Paint(object sender, PaintEventArgs e)
		{
			
			Pen pen = new Pen(Color.Snow);
			int fila;
			fila = controlEntidadUsuarios.Location.Y + controlEntidadUsuarios.Height + 5;
			e.Graphics.DrawLine(pen, 10, fila, this.Width - 10, fila);

			
			fila = textoEtiquetaEmail.Location.Y + textoEtiquetaEmail.Height + 5;
			e.Graphics.DrawLine(pen, 10, fila, this.Width - 10, fila);
		}

		private void botonMenuGuardar_Click(object sender, EventArgs e)
		{
			oUsuario.sUsuarioID = controlEntidadUsuarios.textoCodigo.Text.Trim();
			oUsuario.sApellido = controlEntidadUsuarios.textoDescripcion.Text.Trim();
			oUsuario.sEmail = textoEtiquetaEmail.Valor;
			oUsuario.sPassword = textoPassword1.Text.Trim();
			oUsuario.sPasswordCtr = textoPassword2.Text.Trim();
						

			if (oUsuario.Guardar())
			{
				Informar("Usuario Almacenado...");
				controlEntidadUsuarios.LimpiarCamposForzoso();
				controlEntidadUsuarioCategoria.LimpiarCamposForzoso();
				comboUsuarioGP.SelectedIndex = 0;
				ActualizarTodo();
				return;
			}
			
			MessageBox.Show("No se pudo almacenar el regitro");
		}

		public override void ActualizarTodo()
		{
			base.ActualizarTodo();
			oUsuario.InicializarporsID(controlEntidadUsuarios.textoCodigo.Text.Trim());
			//			textoemail.Text = oUsuario.sEmail;
			textoEtiquetaEmail.Valor = oUsuario.sEmail;
			textoPassword1.Text = oUsuario.sPassword;
			textoPassword2.Text = oUsuario.sPasswordCtr;
			
			if (yapaso)
				return;

			TieneGp();

		}
		private void etiquetaEmail_Click(object sender, EventArgs e)
		{

		}

		private void botonMenuBorrar_Click(object sender, EventArgs e)
		{
			if (oUsuario.eliminar())
			{
				controlEntidadUsuarios.LimpiarCamposForzoso();
				controlEntidadUsuarioCategoria.LimpiarCamposForzoso();
				comboUsuarioGP.SelectedIndex = 0;
				Informar("Registro Eliminado");
				ActualizarTodo();
				return;
			}
		}
	}
}
