namespace Acceso.uAreu
{
    partial class FrmRegistrarSimple
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
            this.textoEtiquetaEmail = new Global.Controles.Text.TextoEtiqueta();
            this.btnRegistrarHuella = new Global.Controles.BotonBase();
            this.btnAgregar = new Global.Controles.BotonBase();
            this.txtHuella = new Global.Controles.TextoSimple();
            this.toolStripContainerMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlEntidadUsuarios
            // 
            this.controlEntidadUsuarios.ActualizaenFormulario = true;
            this.controlEntidadUsuarios.BusquedaAvanzada = false;
            this.controlEntidadUsuarios.ConTitulo = true;
            this.controlEntidadUsuarios.DESCSQLFrom = "Nombre";
            this.controlEntidadUsuarios.GeneraNuevo = false;
            this.controlEntidadUsuarios.IDSQLWhere = "sUsuarioID";
            this.controlEntidadUsuarios.IDSQLWherePK = "sUsuarioID";
            this.controlEntidadUsuarios.IDValor = 0;
            this.controlEntidadUsuarios.Location = new System.Drawing.Point(12, 135);
            this.controlEntidadUsuarios.MostrarID = false;
            this.controlEntidadUsuarios.Name = "controlEntidadUsuarios";
            this.controlEntidadUsuarios.Size = new System.Drawing.Size(495, 70);
            this.controlEntidadUsuarios.SqlAyuda = "Select sUsuarioID, sDescripcion from GRALUsuarios";
            this.controlEntidadUsuarios.TabIndex = 2;
            this.controlEntidadUsuarios.TablaSQL = "GRALUsuarios";
            this.controlEntidadUsuarios.TextoEtiqueta = "Legajo";
            this.controlEntidadUsuarios.TituloAyuda = "Legajo";
            this.controlEntidadUsuarios.VariableGlobalMascara = null;
            // 
            // textoEtiquetaEmail
            // 
            this.textoEtiquetaEmail.Location = new System.Drawing.Point(12, 210);
            this.textoEtiquetaEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textoEtiquetaEmail.Name = "textoEtiquetaEmail";
            this.textoEtiquetaEmail.Size = new System.Drawing.Size(395, 81);
            this.textoEtiquetaEmail.TabIndex = 3;
            this.textoEtiquetaEmail.Titulo = "email";
            this.textoEtiquetaEmail.Valor = null;
            // 
            // btnRegistrarHuella
            // 
            this.btnRegistrarHuella.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrarHuella.Location = new System.Drawing.Point(13, 324);
            this.btnRegistrarHuella.Name = "btnRegistrarHuella";
            this.btnRegistrarHuella.Size = new System.Drawing.Size(120, 40);
            this.btnRegistrarHuella.TabIndex = 4;
            this.btnRegistrarHuella.Text = "Registrar";
            this.btnRegistrarHuella.UseVisualStyleBackColor = true;
            this.btnRegistrarHuella.Click += new System.EventHandler(this.btnRegistrarHuella_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Enabled = false;
            this.btnAgregar.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.Location = new System.Drawing.Point(13, 370);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(120, 40);
            this.btnAgregar.TabIndex = 4;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnRegistrarHuella_Click);
            // 
            // txtHuella
            // 
            this.txtHuella.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHuella.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHuella.Location = new System.Drawing.Point(164, 382);
            this.txtHuella.Name = "txtHuella";
            this.txtHuella.Size = new System.Drawing.Size(233, 27);
            this.txtHuella.TabIndex = 5;
            // 
            // FrmRegistrar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.BotonGuardarVisible = true;
            this.ClientSize = new System.Drawing.Size(951, 768);
            this.Controls.Add(this.txtHuella);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnRegistrarHuella);
            this.Controls.Add(this.textoEtiquetaEmail);
            this.Controls.Add(this.controlEntidadUsuarios);
            this.Name = "FrmRegistrar";
            this.Controls.SetChildIndex(this.toolStripContainerMenu, 0);
            this.Controls.SetChildIndex(this.controlEntidadUsuarios, 0);
            this.Controls.SetChildIndex(this.textoEtiquetaEmail, 0);
            this.Controls.SetChildIndex(this.btnRegistrarHuella, 0);
            this.Controls.SetChildIndex(this.btnAgregar, 0);
            this.Controls.SetChildIndex(this.txtHuella, 0);
            this.toolStripContainerMenu.ResumeLayout(false);
            this.toolStripContainerMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Global.Formularios.ControlEntidad.ControlEntidadSimple controlEntidadUsuarios;
        private Global.Controles.Text.TextoEtiqueta textoEtiquetaEmail;
        private Global.Controles.BotonBase btnRegistrarHuella;
        private Global.Controles.BotonBase btnAgregar;
        private Global.Controles.TextoSimple txtHuella;
    }
}
