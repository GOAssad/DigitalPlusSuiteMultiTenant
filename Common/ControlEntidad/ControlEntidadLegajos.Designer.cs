namespace Acceso.ControlEntidad
{
    partial class ControlEntidadLegajos
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
            this.picture.Image = global::Acceso.Properties.Resources.empleados050x050;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Text = "Legajo";
            // 
            // ControlEntidadLegajos
            // 
            this.ActualizaenFormulario = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ControlarVacio = true;
            this.DESCSQLFrom = "Apellido";
            this.GeneraNuevo = true;
            this.IDSQLWhere = "NumeroLegajo";
            this.IDSQLWherePK = "NumeroLegajo";
            this.Name = "ControlEntidadLegajos";
            this.SqlAyuda = "Select NumeroLegajo Legajo, Apellido, Nombre from Legajo";
            this.TablaSQL = "Legajo";
            this.TextoEtiqueta = "Legajo";
            this.TituloAyuda = "Legajos";
            this.VariableGlobalMascara = "VARAnchoMaxLegajo";
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
