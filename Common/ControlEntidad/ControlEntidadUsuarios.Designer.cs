namespace Acceso.ControlEntidad
{
    partial class ControlEntidadUsuarios
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
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Image = global::Acceso.Properties.Resources.Usuarios035x035;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Text = "Usuario";
            // 
            // ControlEntidadUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ControlarVacio = true;
            this.DESCSQLFrom = "sApellido";
            this.IDSQLWhere = "sUsuarioID";
            this.IDSQLWherePK = "sUsuarioID";
            this.Name = "ControlEntidadUsuarios";
            this.SqlAyuda = "Select sUsuarioID Usuario, sApellido as Apellido, sNombre as Nombre from GRALUsua" +
    "rios";
            this.TablaSQL = "GRALUsuarios";
            this.TextoEtiqueta = "Usuario";
            this.TituloAyuda = "Usuarios";
            this.VariableGlobalMascara = "VARAnchoMaxUsuario";
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
