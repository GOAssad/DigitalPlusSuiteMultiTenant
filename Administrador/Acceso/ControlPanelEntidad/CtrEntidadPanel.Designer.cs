
namespace Acceso.ControlPanelEntidad
{
    partial class CtrEntidadPanel
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
            this.panelLeft = new System.Windows.Forms.Panel();
            this.picture = new FontAwesome.Sharp.IconPictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textoCodigo = new System.Windows.Forms.MaskedTextBox();
            this.panel1 = new Global.Controles.Panel();
            this.textoBusqueda = new Global.Controles.TextoSimple();
            this.textoDescripcion = new Global.Controles.TextoSimple();
            this.btnBusqueda = new FontAwesome.Sharp.IconButton();
            this.panelTop = new Global.Controles.Panel();
            this.botonBusquedaAdicional = new Global.Controles.BotonAplicacion();
            this.etiquetaID = new Global.Controles.EtiquetaSimple();
            this.lblLinkEntidad = new FontAwesome.Sharp.IconButton();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.picture);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(84, 86);
            this.panelLeft.TabIndex = 0;
            // 
            // picture
            // 
            this.picture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picture.BackColor = System.Drawing.SystemColors.Control;
            this.picture.ForeColor = System.Drawing.Color.ForestGreen;
            this.picture.IconChar = FontAwesome.Sharp.IconChar.NetworkWired;
            this.picture.IconColor = System.Drawing.Color.ForestGreen;
            this.picture.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.picture.IconSize = 55;
            this.picture.Location = new System.Drawing.Point(14, 17);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(55, 55);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picture.TabIndex = 106;
            this.picture.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.70443F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.23481F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel1.Controls.Add(this.textoCodigo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBusqueda, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(84, 45);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(606, 40);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textoCodigo
            // 
            this.textoCodigo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoCodigo.Location = new System.Drawing.Point(3, 3);
            this.textoCodigo.Name = "textoCodigo";
            this.textoCodigo.Size = new System.Drawing.Size(113, 34);
            this.textoCodigo.TabIndex = 111;
            this.textoCodigo.Enter += new System.EventHandler(this.textoCodigo_Enter);
            this.textoCodigo.Leave += new System.EventHandler(this.textoCodigo_Leave);
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.textoBusqueda);
            this.panel1.Controls.Add(this.textoDescripcion);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(122, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(438, 34);
            this.panel1.TabIndex = 112;
            // 
            // textoBusqueda
            // 
            this.textoBusqueda.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoBusqueda.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoBusqueda.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.textoBusqueda.Location = new System.Drawing.Point(430, 0);
            this.textoBusqueda.Name = "textoBusqueda";
            this.textoBusqueda.Size = new System.Drawing.Size(10, 34);
            this.textoBusqueda.TabIndex = 104;
            this.textoBusqueda.TabStop = false;
            this.textoBusqueda.Valor = null;
            this.textoBusqueda.Leave += new System.EventHandler(this.textoBusqueda_Leave);
            // 
            // textoDescripcion
            // 
            this.textoDescripcion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textoDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textoDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoDescripcion.Location = new System.Drawing.Point(3, 0);
            this.textoDescripcion.Name = "textoDescripcion";
            this.textoDescripcion.Size = new System.Drawing.Size(424, 34);
            this.textoDescripcion.TabIndex = 100;
            this.textoDescripcion.Valor = null;
            // 
            // btnBusqueda
            // 
            this.btnBusqueda.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBusqueda.IconChar = FontAwesome.Sharp.IconChar.Search;
            this.btnBusqueda.IconColor = System.Drawing.Color.ForestGreen;
            this.btnBusqueda.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBusqueda.IconSize = 25;
            this.btnBusqueda.Location = new System.Drawing.Point(566, 3);
            this.btnBusqueda.Name = "btnBusqueda";
            this.btnBusqueda.Size = new System.Drawing.Size(30, 30);
            this.btnBusqueda.TabIndex = 109;
            this.btnBusqueda.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBusqueda.UseVisualStyleBackColor = true;
            this.btnBusqueda.Click += new System.EventHandler(this.btnBusqueda_Click);
            // 
            // panelTop
            // 
            this.panelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTop.Controls.Add(this.botonBusquedaAdicional);
            this.panelTop.Controls.Add(this.etiquetaID);
            this.panelTop.Controls.Add(this.lblLinkEntidad);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(84, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(606, 44);
            this.panelTop.TabIndex = 2;
            // 
            // botonBusquedaAdicional
            // 
            this.botonBusquedaAdicional.BackColor = System.Drawing.SystemColors.Info;
            this.botonBusquedaAdicional.Location = new System.Drawing.Point(568, 6);
            this.botonBusquedaAdicional.Name = "botonBusquedaAdicional";
            this.botonBusquedaAdicional.Size = new System.Drawing.Size(30, 30);
            this.botonBusquedaAdicional.TabIndex = 109;
            this.botonBusquedaAdicional.TabStop = false;
            this.botonBusquedaAdicional.UseVisualStyleBackColor = false;
            this.botonBusquedaAdicional.Visible = false;
            this.botonBusquedaAdicional.Click += new System.EventHandler(this.botonBusquedaAdicional_Click);
            // 
            // etiquetaID
            // 
            this.etiquetaID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.etiquetaID.AutoSize = true;
            this.etiquetaID.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaID.ForeColor = System.Drawing.Color.Red;
            this.etiquetaID.Location = new System.Drawing.Point(489, 13);
            this.etiquetaID.Name = "etiquetaID";
            this.etiquetaID.Size = new System.Drawing.Size(17, 19);
            this.etiquetaID.TabIndex = 108;
            this.etiquetaID.Text = "0";
            this.etiquetaID.Visible = false;
            // 
            // lblLinkEntidad
            // 
            this.lblLinkEntidad.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblLinkEntidad.FlatAppearance.BorderSize = 0;
            this.lblLinkEntidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLinkEntidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLinkEntidad.IconChar = FontAwesome.Sharp.IconChar.Search;
            this.lblLinkEntidad.IconColor = System.Drawing.Color.ForestGreen;
            this.lblLinkEntidad.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.lblLinkEntidad.IconSize = 35;
            this.lblLinkEntidad.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLinkEntidad.Location = new System.Drawing.Point(0, 0);
            this.lblLinkEntidad.Name = "lblLinkEntidad";
            this.lblLinkEntidad.Size = new System.Drawing.Size(259, 44);
            this.lblLinkEntidad.TabIndex = 107;
            this.lblLinkEntidad.Text = "Titulo";
            this.lblLinkEntidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLinkEntidad.UseVisualStyleBackColor = false;
            this.lblLinkEntidad.Click += new System.EventHandler(this.lblLinkEntidad_Click);
            this.lblLinkEntidad.MouseLeave += new System.EventHandler(this.lblLinkEntidad_MouseLeave);
            this.lblLinkEntidad.MouseHover += new System.EventHandler(this.lblLinkEntidad_MouseHover);
            // 
            // CtrEntidadPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelLeft);
            this.Name = "CtrEntidadPanel";
            this.Size = new System.Drawing.Size(690, 86);
            this.Load += new System.EventHandler(this.CtrEntidadPanel_Load);
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private Global.Controles.Panel panelTop;
        protected FontAwesome.Sharp.IconPictureBox picture;
        protected FontAwesome.Sharp.IconButton lblLinkEntidad;
        protected Global.Controles.EtiquetaSimple etiquetaID;
        protected Global.Controles.BotonAplicacion botonBusquedaAdicional;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected FontAwesome.Sharp.IconButton btnBusqueda;
        public System.Windows.Forms.MaskedTextBox textoCodigo;
        private Global.Controles.Panel panel1;
        public Global.Controles.TextoSimple textoDescripcion;
        protected Global.Controles.TextoSimple textoBusqueda;
    }
}
