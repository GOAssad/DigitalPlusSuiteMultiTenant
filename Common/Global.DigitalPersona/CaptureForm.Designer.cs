namespace Altiora.DigitalPersona
{
	partial class CaptureForm
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
			System.Windows.Forms.Label PromptLabel;
			System.Windows.Forms.Label StatusLabel;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label1;
			this.Prompt = new System.Windows.Forms.TextBox();
			this.StatusText = new System.Windows.Forms.TextBox();
			this.StatusLine = new System.Windows.Forms.Label();
			this.CloseButton = new System.Windows.Forms.Button();
			this.labellegajo = new System.Windows.Forms.Label();
			this.NumDedo = new System.Windows.Forms.NumericUpDown();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.Picture = new System.Windows.Forms.PictureBox();
			this.label01 = new System.Windows.Forms.Label();
			this.label02 = new System.Windows.Forms.Label();
			this.label03 = new System.Windows.Forms.Label();
			this.label04 = new System.Windows.Forms.Label();
			this.label05 = new System.Windows.Forms.Label();
			PromptLabel = new System.Windows.Forms.Label();
			StatusLabel = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.NumDedo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
			this.SuspendLayout();
			// 
			// PromptLabel
			// 
			PromptLabel.AutoSize = true;
			PromptLabel.Location = new System.Drawing.Point(13, 63);
			PromptLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			PromptLabel.Name = "PromptLabel";
			PromptLabel.Size = new System.Drawing.Size(57, 17);
			PromptLabel.TabIndex = 1;
			PromptLabel.Text = "Prompt:";
			// 
			// StatusLabel
			// 
			StatusLabel.AutoSize = true;
			StatusLabel.Location = new System.Drawing.Point(221, 110);
			StatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			StatusLabel.Name = "StatusLabel";
			StatusLabel.Size = new System.Drawing.Size(52, 17);
			StatusLabel.TabIndex = 3;
			StatusLabel.Text = "Status:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(13, 110);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(63, 17);
			label2.TabIndex = 3;
			label2.Text = "Escaneo";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(605, 378);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(42, 17);
			label1.TabIndex = 1;
			label1.Text = "Dedo";
			// 
			// Prompt
			// 
			this.Prompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Prompt.Location = new System.Drawing.Point(16, 84);
			this.Prompt.Margin = new System.Windows.Forms.Padding(4);
			this.Prompt.Name = "Prompt";
			this.Prompt.ReadOnly = true;
			this.Prompt.Size = new System.Drawing.Size(476, 22);
			this.Prompt.TabIndex = 2;
			// 
			// StatusText
			// 
			this.StatusText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.StatusText.BackColor = System.Drawing.SystemColors.Window;
			this.StatusText.Location = new System.Drawing.Point(224, 131);
			this.StatusText.Margin = new System.Windows.Forms.Padding(4);
			this.StatusText.Multiline = true;
			this.StatusText.Name = "StatusText";
			this.StatusText.ReadOnly = true;
			this.StatusText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.StatusText.Size = new System.Drawing.Size(268, 238);
			this.StatusText.TabIndex = 4;
			// 
			// StatusLine
			// 
			this.StatusLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.StatusLine.Location = new System.Drawing.Point(13, 375);
			this.StatusLine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.StatusLine.Name = "StatusLine";
			this.StatusLine.Size = new System.Drawing.Size(479, 48);
			this.StatusLine.TabIndex = 5;
			this.StatusLine.Text = "observaciones";
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Location = new System.Drawing.Point(615, 52);
			this.CloseButton.Margin = new System.Windows.Forms.Padding(4);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(100, 54);
			this.CloseButton.TabIndex = 6;
			this.CloseButton.Text = "Cerrar";
			this.CloseButton.UseVisualStyleBackColor = true;
			// 
			// labellegajo
			// 
			this.labellegajo.AutoSize = true;
			this.labellegajo.Location = new System.Drawing.Point(13, 9);
			this.labellegajo.Name = "labellegajo";
			this.labellegajo.Size = new System.Drawing.Size(12, 17);
			this.labellegajo.TabIndex = 7;
			this.labellegajo.Text = ".";
			// 
			// NumDedo
			// 
			this.NumDedo.Location = new System.Drawing.Point(654, 373);
			this.NumDedo.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.NumDedo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NumDedo.Name = "NumDedo";
			this.NumDedo.Size = new System.Drawing.Size(63, 22);
			this.NumDedo.TabIndex = 8;
			this.NumDedo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NumDedo.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Altiora.DigitalPersona.Properties.Resources.Mano_Derecha_;
			this.pictureBox1.Location = new System.Drawing.Point(504, 131);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(213, 238);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 9;
			this.pictureBox1.TabStop = false;
			// 
			// Picture
			// 
			this.Picture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.Picture.BackColor = System.Drawing.SystemColors.Window;
			this.Picture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Picture.Location = new System.Drawing.Point(16, 131);
			this.Picture.Margin = new System.Windows.Forms.Padding(4);
			this.Picture.Name = "Picture";
			this.Picture.Size = new System.Drawing.Size(200, 238);
			this.Picture.TabIndex = 0;
			this.Picture.TabStop = false;
			// 
			// label01
			// 
			this.label01.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label01.ForeColor = System.Drawing.Color.Red;
			this.label01.Location = new System.Drawing.Point(685, 187);
			this.label01.Name = "label01";
			this.label01.Size = new System.Drawing.Size(23, 25);
			this.label01.TabIndex = 10;
			this.label01.Text = "1";
			// 
			// label02
			// 
			this.label02.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label02.ForeColor = System.Drawing.Color.Red;
			this.label02.Location = new System.Drawing.Point(617, 116);
			this.label02.Name = "label02";
			this.label02.Size = new System.Drawing.Size(23, 25);
			this.label02.TabIndex = 10;
			this.label02.Text = "2";
			// 
			// label03
			// 
			this.label03.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label03.ForeColor = System.Drawing.Color.Red;
			this.label03.Location = new System.Drawing.Point(569, 110);
			this.label03.Name = "label03";
			this.label03.Size = new System.Drawing.Size(23, 25);
			this.label03.TabIndex = 10;
			this.label03.Text = "3";
			// 
			// label04
			// 
			this.label04.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label04.ForeColor = System.Drawing.Color.Red;
			this.label04.Location = new System.Drawing.Point(532, 134);
			this.label04.Name = "label04";
			this.label04.Size = new System.Drawing.Size(23, 25);
			this.label04.TabIndex = 10;
			this.label04.Text = "4";
			// 
			// label05
			// 
			this.label05.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label05.ForeColor = System.Drawing.Color.Red;
			this.label05.Location = new System.Drawing.Point(503, 174);
			this.label05.Name = "label05";
			this.label05.Size = new System.Drawing.Size(23, 25);
			this.label05.TabIndex = 10;
			this.label05.Text = "5";
			// 
			// CaptureForm
			// 
			this.AcceptButton = this.CloseButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CloseButton;
			this.ClientSize = new System.Drawing.Size(728, 436);
			this.Controls.Add(this.label05);
			this.Controls.Add(this.label04);
			this.Controls.Add(this.label03);
			this.Controls.Add(this.label02);
			this.Controls.Add(this.label01);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.NumDedo);
			this.Controls.Add(this.labellegajo);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.StatusLine);
			this.Controls.Add(this.StatusText);
			this.Controls.Add(label2);
			this.Controls.Add(StatusLabel);
			this.Controls.Add(this.Prompt);
			this.Controls.Add(label1);
			this.Controls.Add(PromptLabel);
			this.Controls.Add(this.Picture);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(527, 358);
			this.Name = "CaptureForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "label";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
			this.Load += new System.EventHandler(this.CaptureForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.NumDedo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox Picture;
		private System.Windows.Forms.TextBox Prompt;
		private System.Windows.Forms.TextBox StatusText;
		private System.Windows.Forms.Label StatusLine;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label labellegajo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label label03;
        private System.Windows.Forms.Label label04;
        private System.Windows.Forms.Label label05;
        public System.Windows.Forms.NumericUpDown NumDedo;
	}
}