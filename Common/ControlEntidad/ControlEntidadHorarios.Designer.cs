namespace Acceso.ControlEntidad
{
    partial class ControlEntidadHorarios
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
            this.picture.Image = global::Acceso.Properties.Resources.Calendario_30x30_blanco;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Text = "Horario";
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
            // ControlEntidadHorarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ControlarVacio = true;
            this.DESCSQLFrom = "Nombre";
            this.IDSQLWhere = "sHorarioID";
            this.IDSQLWherePK = "sHorarioID";
            this.Name = "ControlEntidadHorarios";
            this.Size = new System.Drawing.Size(490, 70);
            this.SqlAyuda = "Select sHorarioID Codigo, Nombre Descripcion from Horario";
            this.TablaSQL = "Horario";
            this.TextoEtiqueta = "Horario";
            this.TituloAyuda = "Horarios";
            this.VariableGlobalMascara = "VARAnchoMaxHorrio";
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
