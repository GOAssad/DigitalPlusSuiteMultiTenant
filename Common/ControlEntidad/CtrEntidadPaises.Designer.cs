namespace Acceso.ControlEntidad
{
    partial class CtrEntidadPaises
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
            this.picture.Image = global::Acceso.Properties.Resources.Paises075x075;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Text = "Pais";
            // 
            // textoCodigo
            // 
            this.textoCodigo.Mask = "000";
            this.textoCodigo.Text = "0";
            // 
            // CtrEntidadPaises
            // 
            this.ActualizaenFormulario = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ControlarVacio = true;
            this.DESCSQLFrom = "Nombre";
            this.IDSQLWhere = "nPaisID";
            this.IDSQLWherePK = "nPaisID";
            this.Name = "CtrEntidadPaises";
            this.SqlAyuda = "SELECT 1 AS nPaisID, \'Argentina\' AS Nombre";
            this.TablaSQL = "(SELECT 1 AS nPaisID, \'Argentina\' AS Nombre) AS GRALPaises";
            this.TextoEtiqueta = "Pais";
            this.TituloAyuda = "Paises";
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
