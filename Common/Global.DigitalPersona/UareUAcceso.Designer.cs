namespace Altiora.DigitalPersona
{
	partial class UareUAcceso
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
			this.reloj1 = new Altiora.Controles.Reloj();
			this.Picture = new System.Windows.Forms.PictureBox();
			this.etiquetaPrompt = new Altiora.Controles.EtiquetaSimple();
			this.StatusText = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
			this.SuspendLayout();
			// 
			// reloj1
			// 
			this.reloj1.Location = new System.Drawing.Point(798, 25);
			this.reloj1.Name = "reloj1";
			this.reloj1.Size = new System.Drawing.Size(288, 132);
			this.reloj1.TabIndex = 5;
			// 
			// Picture
			// 
			this.Picture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.Picture.BackColor = System.Drawing.SystemColors.Window;
			this.Picture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Picture.Location = new System.Drawing.Point(33, 209);
			this.Picture.Margin = new System.Windows.Forms.Padding(4);
			this.Picture.Name = "Picture";
			this.Picture.Size = new System.Drawing.Size(200, 238);
			this.Picture.TabIndex = 6;
			this.Picture.TabStop = false;
			// 
			// etiquetaPrompt
			// 
			this.etiquetaPrompt.AutoSize = true;
			this.etiquetaPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetaPrompt.Location = new System.Drawing.Point(29, 461);
			this.etiquetaPrompt.Name = "etiquetaPrompt";
			this.etiquetaPrompt.Size = new System.Drawing.Size(13, 20);
			this.etiquetaPrompt.TabIndex = 7;
			this.etiquetaPrompt.Text = ".";
			// 
			// StatusText
			// 
			this.StatusText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.StatusText.BackColor = System.Drawing.SystemColors.Window;
			this.StatusText.Location = new System.Drawing.Point(241, 209);
			this.StatusText.Margin = new System.Windows.Forms.Padding(4);
			this.StatusText.Multiline = true;
			this.StatusText.Name = "StatusText";
			this.StatusText.ReadOnly = true;
			this.StatusText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.StatusText.Size = new System.Drawing.Size(438, 238);
			this.StatusText.TabIndex = 8;
			// 
			// UareUAcceso
			// 
			this.ClientSize = new System.Drawing.Size(1118, 692);
			this.Controls.Add(this.StatusText);
			this.Controls.Add(this.etiquetaPrompt);
			this.Controls.Add(this.Picture);
			this.Controls.Add(this.reloj1);
			this.Name = "UareUAcceso";
			this.OcultarPanelComandoso = true;
			this.OcultarPanelTitulo = true;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UareUAcceso_FormClosed);
			this.Load += new System.EventHandler(this.UareUAcceso_Load);
			this.Controls.SetChildIndex(this.panelMenuIndividual, 0);
			this.Controls.SetChildIndex(this.reloj1, 0);
			this.Controls.SetChildIndex(this.Picture, 0);
			this.Controls.SetChildIndex(this.etiquetaPrompt, 0);
			this.Controls.SetChildIndex(this.StatusText, 0);
			((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Altiora.Controles.Reloj reloj1;
		private System.Windows.Forms.PictureBox Picture;
		private Controles.EtiquetaSimple etiquetaPrompt;
		private System.Windows.Forms.TextBox StatusText;
	}
}
