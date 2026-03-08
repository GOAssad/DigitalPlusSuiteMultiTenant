namespace Acceso.ControlEntidad
{
    partial class ControlEntidadUbicaciones
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
            this.picture.Image = global::Acceso.Properties.Resources.Sector030x030;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Text = "Sector";
            // 
            // botonBusquedaAdicional
            // 
            this.botonBusquedaAdicional.Location = new System.Drawing.Point(425, 3);
            // 
            // btnBusqueda
            // 
            this.btnBusqueda.Location = new System.Drawing.Point(425, 37);
            // 
            // textoBusqueda
            // 
            this.textoBusqueda.Location = new System.Drawing.Point(423, 37);
            // 
            // etiquetaID
            // 
            this.etiquetaID.Location = new System.Drawing.Point(404, 9);
            // 
            // textoDescripcion
            // 
            this.textoDescripcion.Location = new System.Drawing.Point(69, 36);
            // 
            // ControlEntidadUbicaciones
            // 
            this.AutoGenerarCodigo = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.DESCSQLFrom = "Nombre";
            this.GeneraNuevo = true;
            this.IDSQLWhere = "sUbicacionID";
            this.IDSQLWherePK = "sUbicacionID";
            this.MostrarID = true;
            this.Name = "ControlEntidadUbicaciones";
            this.Size = new System.Drawing.Size(455, 70);
            this.SqlAyuda = "Select sUbicacionID Ubicacion, Nombre Descripcion from Sector";
            this.TablaSQL = "Sector";
            this.TextoEtiqueta = "Sector";
            this.TituloAyuda = "Sectores";
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
