namespace Acceso.Generales
{
	partial class FrmLoginInicio01
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLoginInicio01));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// textoUsuario
			// 
			this.textoUsuario.Location = new System.Drawing.Point(130, 155);
			this.textoUsuario.Size = new System.Drawing.Size(180, 32);
			// 
			// textoPassword
			// 
			this.textoPassword.Location = new System.Drawing.Point(130, 260);
			this.textoPassword.Size = new System.Drawing.Size(180, 32);
			// 
			// botonAceptar
			// 
			this.botonAceptar.Location = new System.Drawing.Point(259, 7);
			// 
			// botonSalir
			// 
			this.botonSalir.Location = new System.Drawing.Point(59, 7);
			// 
			// etiquetaSimple1
			// 
			this.etiquetaSimple1.BackColor = System.Drawing.Color.SaddleBrown;
			this.etiquetaSimple1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetaSimple1.Location = new System.Drawing.Point(130, 127);
			this.etiquetaSimple1.Size = new System.Drawing.Size(79, 25);
			this.etiquetaSimple1.Visible = false;
			// 
			// etiquetaSimple2
			// 
			this.etiquetaSimple2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetaSimple2.Location = new System.Drawing.Point(130, 232);
			this.etiquetaSimple2.Size = new System.Drawing.Size(63, 25);
			this.etiquetaSimple2.Visible = false;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.SaddleBrown;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.panel2.Location = new System.Drawing.Point(0, 458);
			this.panel2.Size = new System.Drawing.Size(615, 59);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.SaddleBrown;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(1, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(609, 513);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// FrmLoginInicio01
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.SaddleBrown;
			this.ClientSize = new System.Drawing.Size(615, 517);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "FrmLoginInicio01";
			this.Text = "FrmLoginInicio01";
			this.TransparencyKey = System.Drawing.Color.SaddleBrown;
			this.Controls.SetChildIndex(this.pictureBox1, 0);
			this.Controls.SetChildIndex(this.panel2, 0);
			this.Controls.SetChildIndex(this.etiquetaSimple1, 0);
			this.Controls.SetChildIndex(this.etiquetaSimple2, 0);
			this.Controls.SetChildIndex(this.textoUsuario, 0);
			this.Controls.SetChildIndex(this.textoPassword, 0);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
	}
}