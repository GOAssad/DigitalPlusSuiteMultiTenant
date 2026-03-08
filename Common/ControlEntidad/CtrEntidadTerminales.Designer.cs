namespace Acceso.ControlEntidad
{
    partial class CtrEntidadTerminales
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
            this.picture.Image = global::Acceso.Properties.Resources.Terminal075x075;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Text = "Terminal";
            // 
            // CtrEntidadTerminales
            // 
            this.ActualizaenFormulario = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.DESCSQLFrom = "Nombre";
            this.IDSQLWhere = "sTerminalID";
            this.IDSQLWherePK = "sTerminalID";
            this.Name = "CtrEntidadTerminales";
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
