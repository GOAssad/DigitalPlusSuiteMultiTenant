namespace Acceso.Generales
{
    partial class FrmRegistrar
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
            this.textoEmail = new Global.Controles.Text.TextoEtiqueta();
            this.controlEntidadUsuarios = new Global.Formularios.ControlEntidad.ControlEntidadSimple();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.BotonRegistrar = new Global.Controles.BotonBase();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textoEmail
            // 
            this.textoEmail.Location = new System.Drawing.Point(573, 2);
            this.textoEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textoEmail.Name = "textoEmail";
            this.textoEmail.Size = new System.Drawing.Size(375, 81);
            this.textoEmail.TabIndex = 1;
            this.textoEmail.Titulo = "eMail";
            this.textoEmail.Valor = null;
            // 
            // controlEntidadUsuarios
            // 
            this.controlEntidadUsuarios.ActualizaenFormulario = true;
            this.controlEntidadUsuarios.BusquedaAvanzada = false;
            this.controlEntidadUsuarios.ConTitulo = true;
            this.controlEntidadUsuarios.DESCSQLFrom = "Nombre";
            this.controlEntidadUsuarios.GeneraNuevo = false;
            this.controlEntidadUsuarios.IDSQLWhere = "sUsuarioID";
            this.controlEntidadUsuarios.IDSQLWherePK = "nUsuarioID";
            this.controlEntidadUsuarios.IDValor = 0;
            this.controlEntidadUsuarios.Location = new System.Drawing.Point(3, 3);
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
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.Controls.Add(this.textoEmail, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.controlEntidadUsuarios, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 113);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.95323F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(951, 99);
            this.tableLayoutPanel2.TabIndex = 5;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // BotonRegistrar
            // 
            this.BotonRegistrar.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonRegistrar.Location = new System.Drawing.Point(53, 319);
            this.BotonRegistrar.Name = "BotonRegistrar";
            this.BotonRegistrar.Size = new System.Drawing.Size(120, 40);
            this.BotonRegistrar.TabIndex = 6;
            this.BotonRegistrar.Text = "Registrar";
            this.BotonRegistrar.UseVisualStyleBackColor = true;
            this.BotonRegistrar.Click += new System.EventHandler(this.BotonRegistrar_Click);
            // 
            // FrmRegistrar
            // 
            this.ClientSize = new System.Drawing.Size(951, 768);
            this.Controls.Add(this.BotonRegistrar);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "FrmRegistrar";
            this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
            this.Controls.SetChildIndex(this.panelMenuIndividual, 0);
            this.Controls.SetChildIndex(this.BotonRegistrar, 0);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Global.Controles.Text.TextoEtiqueta textoEmail;
        private Global.Formularios.ControlEntidad.ControlEntidadSimple controlEntidadUsuarios;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Global.Controles.BotonBase BotonRegistrar;
    }
}
