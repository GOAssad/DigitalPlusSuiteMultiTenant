namespace Acceso.ControlEntidad
{
    partial class CtrEntidad
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
            this.botonBusquedaAdicional = new Global.Controles.BotonAplicacion();
            this.btnBusqueda = new Global.Controles.BotonAplicacion();
            this.textoBusqueda = new Global.Controles.TextoSimple();
            this.etiquetaID = new Global.Controles.EtiquetaSimple();
            this.textoDescripcion = new Global.Controles.TextoSimple();
            this.textoCodigo = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Image = global::Acceso.Properties.Resources.Logo_DO_025x025;
            this.picture.Location = new System.Drawing.Point(3, 2);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(90, 90);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLinkEntidad.Image = global::Acceso.Properties.Resources.buscar020x020;
            this.lblLinkEntidad.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLinkEntidad.Location = new System.Drawing.Point(121, 2);
            this.lblLinkEntidad.Name = "lblLinkEntidad";
            this.lblLinkEntidad.Size = new System.Drawing.Size(207, 46);
            this.lblLinkEntidad.TabIndex = 1;
            this.lblLinkEntidad.Text = "Texto";
            this.lblLinkEntidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLinkEntidad.Click += new System.EventHandler(this.lblLinkEntidad_Click);
            this.lblLinkEntidad.MouseLeave += new System.EventHandler(this.lblLinkEntidad_MouseLeave);
            this.lblLinkEntidad.MouseHover += new System.EventHandler(this.lblLinkEntidad_MouseHover);
            this.lblLinkEntidad.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblLinkEntidad_MouseUp);
            // 
            // botonBusquedaAdicional
            // 
            this.botonBusquedaAdicional.BackColor = System.Drawing.SystemColors.Info;
            this.botonBusquedaAdicional.Location = new System.Drawing.Point(804, 26);
            this.botonBusquedaAdicional.Name = "botonBusquedaAdicional";
            this.botonBusquedaAdicional.Size = new System.Drawing.Size(35, 35);
            this.botonBusquedaAdicional.TabIndex = 102;
            this.botonBusquedaAdicional.TabStop = false;
            this.botonBusquedaAdicional.UseVisualStyleBackColor = false;
            this.botonBusquedaAdicional.Visible = false;
            this.botonBusquedaAdicional.Click += new System.EventHandler(this.botonBusquedaAdicional_Click);
            // 
            // btnBusqueda
            // 
            this.btnBusqueda.BackColor = System.Drawing.SystemColors.Info;
            this.btnBusqueda.Location = new System.Drawing.Point(804, 60);
            this.btnBusqueda.Name = "btnBusqueda";
            this.btnBusqueda.Size = new System.Drawing.Size(35, 35);
            this.btnBusqueda.TabIndex = 100;
            this.btnBusqueda.TabStop = false;
            this.btnBusqueda.Text = "...";
            this.btnBusqueda.UseVisualStyleBackColor = false;
            this.btnBusqueda.Click += new System.EventHandler(this.btnBusqueda_Click);
            // 
            // textoBusqueda
            // 
            this.textoBusqueda.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoBusqueda.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoBusqueda.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.textoBusqueda.Location = new System.Drawing.Point(797, 51);
            this.textoBusqueda.Name = "textoBusqueda";
            this.textoBusqueda.Size = new System.Drawing.Size(1, 41);
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
            this.etiquetaID.Location = new System.Drawing.Point(755, 2);
            this.etiquetaID.Name = "etiquetaID";
            this.etiquetaID.Size = new System.Drawing.Size(17, 19);
            this.etiquetaID.TabIndex = 101;
            this.etiquetaID.Text = "0";
            this.etiquetaID.Visible = false;
            // 
            // textoDescripcion
            // 
            this.textoDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoDescripcion.Location = new System.Drawing.Point(271, 51);
            this.textoDescripcion.Name = "textoDescripcion";
            this.textoDescripcion.Size = new System.Drawing.Size(520, 41);
            this.textoDescripcion.TabIndex = 99;
            // 
            // textoCodigo
            // 
            this.textoCodigo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoCodigo.Location = new System.Drawing.Point(126, 51);
            this.textoCodigo.Name = "textoCodigo";
            this.textoCodigo.Size = new System.Drawing.Size(139, 41);
            this.textoCodigo.TabIndex = 104;
            this.textoCodigo.Enter += new System.EventHandler(this.textoCodigo_Enter);
            this.textoCodigo.Leave += new System.EventHandler(this.textoCodigo_Leave);
            // 
            // CtrEntidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textoCodigo);
            this.Controls.Add(this.textoBusqueda);
            this.Controls.Add(this.textoDescripcion);
            this.Controls.Add(this.botonBusquedaAdicional);
            this.Controls.Add(this.btnBusqueda);
            this.Controls.Add(this.etiquetaID);
            this.Controls.Add(this.lblLinkEntidad);
            this.Controls.Add(this.picture);
            this.Name = "CtrEntidad";
            this.Size = new System.Drawing.Size(845, 98);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected System.Windows.Forms.PictureBox picture;
        protected Global.Controles.EtiquetaSimple lblLinkEntidad;
        protected Global.Controles.BotonAplicacion botonBusquedaAdicional;
        protected Global.Controles.BotonAplicacion btnBusqueda;
        protected Global.Controles.TextoSimple textoBusqueda;
        protected Global.Controles.EtiquetaSimple etiquetaID;
        public Global.Controles.TextoSimple textoDescripcion;
        public System.Windows.Forms.MaskedTextBox textoCodigo;
    }
}
