namespace Global.Controles.Text
{
	partial class TextoEtiqueta
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.etiquetaEntidad = new Global.Controles.EtiquetaSimple();
            this.textoEntidad = new System.Windows.Forms.MaskedTextBox();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.picture = new FontAwesome.Sharp.IconPictureBox();
            this.panel1 = new Global.Controles.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.etiquetaEntidad, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textoEntidad, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(429, 85);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // etiquetaEntidad
            // 
            this.etiquetaEntidad.AutoSize = true;
            this.etiquetaEntidad.Dock = System.Windows.Forms.DockStyle.Left;
            this.etiquetaEntidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaEntidad.Location = new System.Drawing.Point(3, 0);
            this.etiquetaEntidad.Name = "etiquetaEntidad";
            this.etiquetaEntidad.Size = new System.Drawing.Size(114, 42);
            this.etiquetaEntidad.TabIndex = 1;
            this.etiquetaEntidad.Text = "Descripcion";
            this.etiquetaEntidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textoEntidad
            // 
            this.textoEntidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textoEntidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoEntidad.Location = new System.Drawing.Point(3, 45);
            this.textoEntidad.Name = "textoEntidad";
            this.textoEntidad.Size = new System.Drawing.Size(423, 34);
            this.textoEntidad.TabIndex = 2;
            this.textoEntidad.TextChanged += new System.EventHandler(this.textoEntidad_TextChanged);
            this.textoEntidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textoEntidad_KeyPress);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.picture);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(84, 85);
            this.panelLeft.TabIndex = 3;
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
            this.picture.Location = new System.Drawing.Point(14, 15);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(55, 55);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picture.TabIndex = 107;
            this.picture.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(84, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 85);
            this.panel1.TabIndex = 4;
            // 
            // TextoEtiqueta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelLeft);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TextoEtiqueta";
            this.Size = new System.Drawing.Size(513, 85);
            this.Resize += new System.EventHandler(this.TextoEtiqueta_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
		private EtiquetaSimple etiquetaEntidad;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelLeft;
        protected FontAwesome.Sharp.IconPictureBox picture;
        private Panel panel1;
        private System.Windows.Forms.MaskedTextBox textoEntidad;
    }
}
