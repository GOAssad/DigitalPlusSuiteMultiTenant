namespace Acceso.ControlEntidad
{
    partial class ControlEntidad
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.picture = new System.Windows.Forms.PictureBox();
            this.lblLinkEntidad = new Global.Controles.EtiquetaSimple();
            this.textoCodigo = new Global.Controles.TextoSimple();
            this.botonBusquedaAdicional = new Global.Controles.BotonAplicacion();
            this.btnBusqueda = new Global.Controles.BotonAplicacion();
            this.textoBusqueda = new Global.Controles.TextoSimple();
            this.etiquetaID = new Global.Controles.EtiquetaSimple();
            this.textoDescripcion = new Global.Controles.TextoSimple();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Image = global::Acceso.Properties.Resources.Logo_DO_025x025;
            this.picture.Location = new System.Drawing.Point(5, 10);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(55, 55);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLinkEntidad.Image = global::Acceso.Properties.Resources.buscar020x020;
            this.lblLinkEntidad.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLinkEntidad.Location = new System.Drawing.Point(72, 3);
            this.lblLinkEntidad.Name = "lblLinkEntidad";
            this.lblLinkEntidad.Size = new System.Drawing.Size(207, 30);
            this.lblLinkEntidad.TabIndex = 1;
            this.lblLinkEntidad.Text = ".";
            this.lblLinkEntidad.Click += new System.EventHandler(this.lblLinkEntidad_Click);
            this.lblLinkEntidad.MouseLeave += new System.EventHandler(this.lblLinkEntidad_MouseLeave);
            this.lblLinkEntidad.MouseHover += new System.EventHandler(this.lblLinkEntidad_MouseHover);
            this.lblLinkEntidad.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblLinkEntidad_MouseUp);
            // 
            // textoCodigo
            // 
            this.textoCodigo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoCodigo.Location = new System.Drawing.Point(69, 37);
            this.textoCodigo.Name = "textoCodigo";
            this.textoCodigo.Size = new System.Drawing.Size(100, 27);
            this.textoCodigo.TabIndex = 2;
            this.textoCodigo.Enter += new System.EventHandler(this.textoCodigo_Enter);
            this.textoCodigo.Leave += new System.EventHandler(this.textoCodigo_Leave);
            // 
            // botonBusquedaAdicional
            // 
            this.botonBusquedaAdicional.BackColor = System.Drawing.SystemColors.Info;
            this.botonBusquedaAdicional.Location = new System.Drawing.Point(531, 3);
            this.botonBusquedaAdicional.Name = "botonBusquedaAdicional";
            this.botonBusquedaAdicional.Size = new System.Drawing.Size(26, 28);
            this.botonBusquedaAdicional.TabIndex = 102;
            this.botonBusquedaAdicional.TabStop = false;
            this.botonBusquedaAdicional.UseVisualStyleBackColor = false;
            this.botonBusquedaAdicional.Visible = false;
            this.botonBusquedaAdicional.Click += new System.EventHandler(this.botonBusquedaAdicional_Click);
            // 
            // btnBusqueda
            // 
            this.btnBusqueda.BackColor = System.Drawing.SystemColors.Info;
            this.btnBusqueda.Location = new System.Drawing.Point(531, 37);
            this.btnBusqueda.Name = "btnBusqueda";
            this.btnBusqueda.Size = new System.Drawing.Size(26, 28);
            this.btnBusqueda.TabIndex = 100;
            this.btnBusqueda.TabStop = false;
            this.btnBusqueda.Text = "...";
            this.btnBusqueda.UseVisualStyleBackColor = false;
            this.btnBusqueda.Click += new System.EventHandler(this.btnBusqueda_Click);
            // 
            // textoBusqueda
            // 
            this.textoBusqueda.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoBusqueda.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoBusqueda.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.textoBusqueda.Location = new System.Drawing.Point(529, 37);
            this.textoBusqueda.Name = "textoBusqueda";
            this.textoBusqueda.Size = new System.Drawing.Size(1, 27);
            this.textoBusqueda.TabIndex = 103;
            this.textoBusqueda.TabStop = false;
            this.textoBusqueda.Enter += new System.EventHandler(this.textoBusqueda_Enter);
            this.textoBusqueda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textoBusqueda_KeyPress);
            this.textoBusqueda.Leave += new System.EventHandler(this.textoBusqueda_Leave);
            // 
            // etiquetaID
            // 
            this.etiquetaID.AutoSize = true;
            this.etiquetaID.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaID.ForeColor = System.Drawing.Color.Red;
            this.etiquetaID.Location = new System.Drawing.Point(510, 9);
            this.etiquetaID.Name = "etiquetaID";
            this.etiquetaID.Size = new System.Drawing.Size(17, 19);
            this.etiquetaID.TabIndex = 101;
            this.etiquetaID.Text = "0";
            this.etiquetaID.Visible = false;
            // 
            // textoDescripcion
            // 
            this.textoDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoDescripcion.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoDescripcion.Location = new System.Drawing.Point(174, 37);
            this.textoDescripcion.Name = "textoDescripcion";
            this.textoDescripcion.Size = new System.Drawing.Size(352, 28);
            this.textoDescripcion.TabIndex = 99;
            // 
            // ControlEntidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.textoBusqueda);
            this.Controls.Add(this.textoDescripcion);
            this.Controls.Add(this.botonBusquedaAdicional);
            this.Controls.Add(this.btnBusqueda);
            this.Controls.Add(this.etiquetaID);
            this.Controls.Add(this.textoCodigo);
            this.Controls.Add(this.lblLinkEntidad);
            this.Controls.Add(this.picture);
            this.Name = "ControlEntidad";
            this.Size = new System.Drawing.Size(565, 70);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected System.Windows.Forms.PictureBox picture;
        public Global.Controles.TextoSimple textoCodigo;
        protected Global.Controles.EtiquetaSimple lblLinkEntidad;
        protected Global.Controles.BotonAplicacion botonBusquedaAdicional;
        protected Global.Controles.BotonAplicacion btnBusqueda;
        protected Global.Controles.TextoSimple textoBusqueda;
        protected Global.Controles.EtiquetaSimple etiquetaID;
        public Global.Controles.TextoSimple textoDescripcion;
    }
}
