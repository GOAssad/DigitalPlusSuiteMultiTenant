namespace Acceso.Generales
{
	partial class FrmUsuarios
	{
		/// <summary>
		/// Variable del diseñador necesaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpiar los recursos que se estén usando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.controlEntidadUsuarios = new Global.Formularios.ControlEntidad.ControlEntidadSimple();
			this.textoPassword2 = new Global.Controles.TextoSimple();
			this.comboUsuarioGP = new Global.Controles.ComboDesplegable();
			this.etiquetaGP = new Global.Controles.EtiquetaSimple();
			this.textoEtiquetaEmail = new Global.Controles.Text.TextoEtiqueta();
			this.etiquetaSimple1 = new Global.Controles.EtiquetaSimple();
			this.textoPassword1 = new Global.Controles.TextoSimple();
			this.controlEntidadUsuarioCategoria = new Global.Formularios.ControlEntidad.ControlEntidadSimple();
			this.SuspendLayout();
			// 
			// botonMenuEditar
			// 
			this.botonMenuEditar.Visible = false;
			// 
			// botonMenuBorrar
			// 
			this.botonMenuBorrar.Click += new System.EventHandler(this.botonMenuBorrar_Click);
			// 
			// botonMenuGuardar
			// 
			this.botonMenuGuardar.TabIndex = 0;
			this.botonMenuGuardar.Click += new System.EventHandler(this.botonMenuGuardar_Click);
			// 
			// controlEntidadUsuarios
			// 
			this.controlEntidadUsuarios.ActualizaenFormulario = true;
			this.controlEntidadUsuarios.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.controlEntidadUsuarios.BusquedaAvanzada = false;
			this.controlEntidadUsuarios.ConTitulo = true;
			this.controlEntidadUsuarios.DESCSQLFrom = "Nombre";
			this.controlEntidadUsuarios.GeneraNuevo = true;
			this.controlEntidadUsuarios.IDSQLWhere = "sUsuarioID";
			this.controlEntidadUsuarios.IDSQLWherePK = "nUsuarioID";
			this.controlEntidadUsuarios.IDValor = 0;
			this.controlEntidadUsuarios.Location = new System.Drawing.Point(108, 122);
			this.controlEntidadUsuarios.MostrarID = false;
			this.controlEntidadUsuarios.Name = "controlEntidadUsuarios";
			this.controlEntidadUsuarios.Size = new System.Drawing.Size(495, 60);
			this.controlEntidadUsuarios.SqlAyuda = "SELECT sUsuarioID, sDescripcion, sEmail FROM GralUsuarios order by sUsuarioID";
			this.controlEntidadUsuarios.TabIndex = 0;
			this.controlEntidadUsuarios.TablaSQL = "GRALUsuarios";
			this.controlEntidadUsuarios.TextoEtiqueta = "Usuario";
			this.controlEntidadUsuarios.TituloAyuda = "Usuarios";
			this.controlEntidadUsuarios.VariableGlobalMascara = null;
			// 
			// textoPassword2
			// 
			this.textoPassword2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textoPassword2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textoPassword2.Location = new System.Drawing.Point(115, 264);
			this.textoPassword2.Name = "textoPassword2";
			this.textoPassword2.PasswordChar = '*';
			this.textoPassword2.Size = new System.Drawing.Size(456, 32);
			this.textoPassword2.TabIndex = 2;
			// 
			// comboUsuarioGP
			// 
			this.comboUsuarioGP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUsuarioGP.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboUsuarioGP.ForeColor = System.Drawing.Color.Black;
			this.comboUsuarioGP.FormattingEnabled = true;
			this.comboUsuarioGP.Location = new System.Drawing.Point(118, 512);
			this.comboUsuarioGP.Name = "comboUsuarioGP";
			this.comboUsuarioGP.Size = new System.Drawing.Size(178, 29);
			this.comboUsuarioGP.TabIndex = 7;
			this.comboUsuarioGP.Visible = false;
			// 
			// etiquetaGP
			// 
			this.etiquetaGP.AutoSize = true;
			this.etiquetaGP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetaGP.Location = new System.Drawing.Point(16, 521);
			this.etiquetaGP.Name = "etiquetaGP";
			this.etiquetaGP.Size = new System.Drawing.Size(96, 20);
			this.etiquetaGP.TabIndex = 8;
			this.etiquetaGP.Text = "Usuario GP";
			this.etiquetaGP.Visible = false;
			// 
			// textoEtiquetaEmail
			// 
			this.textoEtiquetaEmail.Location = new System.Drawing.Point(115, 314);
			this.textoEtiquetaEmail.Name = "textoEtiquetaEmail";
			this.textoEtiquetaEmail.Size = new System.Drawing.Size(495, 60);
			this.textoEtiquetaEmail.TabIndex = 9;
			this.textoEtiquetaEmail.Titulo = "eMail";
			this.textoEtiquetaEmail.Valor = null;
			// 
			// etiquetaSimple1
			// 
			this.etiquetaSimple1.AutoSize = true;
			this.etiquetaSimple1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetaSimple1.Location = new System.Drawing.Point(111, 197);
			this.etiquetaSimple1.Name = "etiquetaSimple1";
			this.etiquetaSimple1.Size = new System.Drawing.Size(83, 20);
			this.etiquetaSimple1.TabIndex = 6;
			this.etiquetaSimple1.Text = "Password";
			this.etiquetaSimple1.Click += new System.EventHandler(this.etiquetaEmail_Click);
			// 
			// textoPassword1
			// 
			this.textoPassword1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textoPassword1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textoPassword1.Location = new System.Drawing.Point(115, 226);
			this.textoPassword1.Name = "textoPassword1";
			this.textoPassword1.PasswordChar = '*';
			this.textoPassword1.Size = new System.Drawing.Size(456, 32);
			this.textoPassword1.TabIndex = 1;
			// 
			// controlEntidadUsuarioCategoria
			// 
			this.controlEntidadUsuarioCategoria.ActualizaenFormulario = false;
			this.controlEntidadUsuarioCategoria.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.controlEntidadUsuarioCategoria.BusquedaAvanzada = false;
			this.controlEntidadUsuarioCategoria.ConTitulo = true;
			this.controlEntidadUsuarioCategoria.DESCSQLFrom = "Nombre";
			this.controlEntidadUsuarioCategoria.GeneraNuevo = false;
			this.controlEntidadUsuarioCategoria.IDSQLWhere = "nUsuarioCategoriaID";
			this.controlEntidadUsuarioCategoria.IDSQLWherePK = "nUsuarioCategoriaID";
			this.controlEntidadUsuarioCategoria.IDValor = 0;
			this.controlEntidadUsuarioCategoria.Location = new System.Drawing.Point(108, 398);
			this.controlEntidadUsuarioCategoria.MostrarID = false;
			this.controlEntidadUsuarioCategoria.Name = "controlEntidadUsuarioCategoria";
			this.controlEntidadUsuarioCategoria.Size = new System.Drawing.Size(495, 60);
			this.controlEntidadUsuarioCategoria.SqlAyuda = "SELECT nUsuarioCategoriaID, sDescripcion FROM GralUsuariosCategorias";
			this.controlEntidadUsuarioCategoria.TabIndex = 0;
			this.controlEntidadUsuarioCategoria.TablaSQL = "GRALUsuariosCategorias";
			this.controlEntidadUsuarioCategoria.TextoEtiqueta = "Categoria";
			this.controlEntidadUsuarioCategoria.TituloAyuda = "Categorias";
			this.controlEntidadUsuarioCategoria.VariableGlobalMascara = null;
			// 
			// FrmUsuarios
			// 
			this.ClientSize = new System.Drawing.Size(1118, 692);
			this.Controls.Add(this.textoEtiquetaEmail);
			this.Controls.Add(this.etiquetaGP);
			this.Controls.Add(this.comboUsuarioGP);
			this.Controls.Add(this.etiquetaSimple1);
			this.Controls.Add(this.textoPassword2);
			this.Controls.Add(this.textoPassword1);
			this.Controls.Add(this.controlEntidadUsuarioCategoria);
			this.Controls.Add(this.controlEntidadUsuarios);
			this.Name = "FrmUsuarios";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmUsuarios_Paint);
			this.Controls.SetChildIndex(this.controlEntidadUsuarios, 0);
			this.Controls.SetChildIndex(this.controlEntidadUsuarioCategoria, 0);
			this.Controls.SetChildIndex(this.textoPassword1, 0);
			this.Controls.SetChildIndex(this.textoPassword2, 0);
			this.Controls.SetChildIndex(this.etiquetaSimple1, 0);
			this.Controls.SetChildIndex(this.comboUsuarioGP, 0);
			this.Controls.SetChildIndex(this.etiquetaGP, 0);
			this.Controls.SetChildIndex(this.textoEtiquetaEmail, 0);
			this.Controls.SetChildIndex(this.panelMenuIndividual, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Global.Formularios.ControlEntidad.ControlEntidadSimple controlEntidadUsuarios;
		private Global.Controles.TextoSimple textoPassword2;
		private Global.Controles.ComboDesplegable comboUsuarioGP;
		private Global.Controles.EtiquetaSimple etiquetaGP;
		private Global.Controles.Text.TextoEtiqueta textoEtiquetaEmail;
		private Global.Controles.EtiquetaSimple etiquetaSimple1;
		private Global.Controles.TextoSimple textoPassword1;
		private Global.Formularios.ControlEntidad.ControlEntidadSimple controlEntidadUsuarioCategoria;
	}
}
