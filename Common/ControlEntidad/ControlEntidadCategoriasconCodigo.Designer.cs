namespace Acceso.ControlEntidad
{
    partial class ControlEntidadCategoriasconCodigo
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
            this.picture.Image = global::Acceso.Properties.Resources.Categorias035x036;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Text = "Categoria";
            // 
            // botonBusquedaAdicional
            // 
            this.botonBusquedaAdicional.Location = new System.Drawing.Point(459, 3);
            // 
            // btnBusqueda
            // 
            this.btnBusqueda.Location = new System.Drawing.Point(459, 37);
            // 
            // textoBusqueda
            // 
            this.textoBusqueda.Location = new System.Drawing.Point(457, 37);
            // 
            // etiquetaID
            // 
            this.etiquetaID.Location = new System.Drawing.Point(438, 9);
            // 
            // textoDescripcion
            // 
            this.textoDescripcion.Size = new System.Drawing.Size(280, 28);
            // 
            // ControlEntidadCategoriasconCodigo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ControlarVacio = true;
            this.DESCSQLFrom = "Nombre";
            this.IDSQLWhere = "sCategoriaID";
            this.IDSQLWherePK = "sCategoriaID";
            this.Name = "ControlEntidadCategoriasconCodigo";
            this.Size = new System.Drawing.Size(490, 70);
            this.SqlAyuda = "Select sCategoriaID Categoria, Nombre Descripcion from Categoria";
            this.TablaSQL = "Categoria";
            this.TextoEtiqueta = "Categoria";
            this.TituloAyuda = "Categorias";
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
