namespace Acceso.ControlEntidad
{
    partial class ControlEntidadTerminales
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
            this.picture.Image = global::Acceso.Properties.Resources.Terminal035x035;
            // 
            // textoCodigo
            // 
            this.textoCodigo.Size = new System.Drawing.Size(151, 27);
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Text = "Terminal";
            // 
            // textoDescripcion
            // 
            this.textoDescripcion.Location = new System.Drawing.Point(226, 37);
            this.textoDescripcion.Size = new System.Drawing.Size(300, 28);
            // 
            // ControlEntidadTerminales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.DESCSQLFrom = "Nombre";
            this.GeneraNuevo = true;
            this.IDSQLWhere = "sTerminalID";
            this.IDSQLWherePK = "sTerminalID";
            this.Name = "ControlEntidadTerminales";
            this.SqlAyuda = "select sTerminalID Terminal, Nombre Descripcion, sSucursalID Sucural from Termina" +
    "l";
            this.TablaSQL = "Terminal";
            this.TextoEtiqueta = "Terminal";
            this.TituloAyuda = "Terminales";
            this.VariableGlobalMascara = "VarAnchoMaxTerminal";
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
