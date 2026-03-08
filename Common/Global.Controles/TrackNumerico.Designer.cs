
namespace Global.Controles
{
    partial class TrackNumerico
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
            this.trackbar = new System.Windows.Forms.TrackBar();
            this.panelDetalle = new System.Windows.Forms.Panel();
            this.lblValor = new System.Windows.Forms.Label();
            this.etiquetaEntidad = new Global.Controles.EtiquetaSimple();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbar)).BeginInit();
            this.panelDetalle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.picture);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(84, 80);
            this.panelLeft.TabIndex = 4;
            // 
            // picture
            // 
            this.picture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picture.BackColor = System.Drawing.SystemColors.Control;
            this.picture.ForeColor = System.Drawing.Color.ForestGreen;
            this.picture.IconChar = FontAwesome.Sharp.IconChar.SortAmountDownAlt;
            this.picture.IconColor = System.Drawing.Color.ForestGreen;
            this.picture.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.picture.IconSize = 55;
            this.picture.Location = new System.Drawing.Point(14, 13);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(55, 55);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picture.TabIndex = 107;
            this.picture.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.trackbar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelDetalle, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(84, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(335, 80);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // trackbar
            // 
            this.trackbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackbar.Location = new System.Drawing.Point(3, 43);
            this.trackbar.Name = "trackbar";
            this.trackbar.Size = new System.Drawing.Size(329, 34);
            this.trackbar.TabIndex = 2;
            this.trackbar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackbar.Scroll += new System.EventHandler(this.trackbar_Scroll);
            // 
            // panelDetalle
            // 
            this.panelDetalle.Controls.Add(this.lblValor);
            this.panelDetalle.Controls.Add(this.etiquetaEntidad);
            this.panelDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetalle.Location = new System.Drawing.Point(3, 3);
            this.panelDetalle.Name = "panelDetalle";
            this.panelDetalle.Size = new System.Drawing.Size(329, 34);
            this.panelDetalle.TabIndex = 3;
            // 
            // lblValor
            // 
            this.lblValor.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblValor.Location = new System.Drawing.Point(272, 0);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(57, 34);
            this.lblValor.TabIndex = 3;
            this.lblValor.Text = "0";
            this.lblValor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // etiquetaEntidad
            // 
            this.etiquetaEntidad.Dock = System.Windows.Forms.DockStyle.Left;
            this.etiquetaEntidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaEntidad.Location = new System.Drawing.Point(0, 0);
            this.etiquetaEntidad.Name = "etiquetaEntidad";
            this.etiquetaEntidad.Size = new System.Drawing.Size(266, 34);
            this.etiquetaEntidad.TabIndex = 2;
            this.etiquetaEntidad.Text = "Descripcion";
            this.etiquetaEntidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TrackNumerico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panelLeft);
            this.Name = "TrackNumerico";
            this.Size = new System.Drawing.Size(419, 80);
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbar)).EndInit();
            this.panelDetalle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        protected FontAwesome.Sharp.IconPictureBox picture;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TrackBar trackbar;
        private System.Windows.Forms.Panel panelDetalle;
        private System.Windows.Forms.Label lblValor;
        private EtiquetaSimple etiquetaEntidad;
    }
}
