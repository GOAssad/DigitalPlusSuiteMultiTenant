namespace Acceso.uAreu
{
    partial class FrmIngresoEgreso
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
            this.StatusLine = new Global.Controles.EtiquetaTitulo();
            this.Prompt = new Global.Controles.TextoSimple();
            this.StatusText = new Global.Controles.TextoSimple();
            this.Picture = new System.Windows.Forms.PictureBox();
            this.etiquetaBienVenida = new System.Windows.Forms.Label();
            this.etiquetaSucursal = new Global.Controles.EtiquetaTitulo();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusLine
            // 
            this.StatusLine.AutoEllipsis = true;
            this.StatusLine.AutoSize = true;
            this.StatusLine.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLine.ForeColor = System.Drawing.Color.SteelBlue;
            this.StatusLine.Location = new System.Drawing.Point(12, 403);
            this.StatusLine.Name = "StatusLine";
            this.StatusLine.Size = new System.Drawing.Size(203, 38);
            this.StatusLine.TabIndex = 0;
            this.StatusLine.Text = "etiquetaTitulo1";
            this.StatusLine.Visible = false;
            // 
            // Prompt
            // 
            this.Prompt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Prompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Prompt.Location = new System.Drawing.Point(12, 12);
            this.Prompt.Name = "Prompt";
            this.Prompt.ReadOnly = true;
            this.Prompt.Size = new System.Drawing.Size(595, 27);
            this.Prompt.TabIndex = 1;
            // 
            // StatusText
            // 
            this.StatusText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusText.Location = new System.Drawing.Point(12, 238);
            this.StatusText.Multiline = true;
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(776, 162);
            this.StatusText.TabIndex = 1;
            // 
            // Picture
            // 
            this.Picture.Location = new System.Drawing.Point(12, 45);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(220, 187);
            this.Picture.TabIndex = 3;
            this.Picture.TabStop = false;
            // 
            // etiquetaBienVenida
            // 
            this.etiquetaBienVenida.AutoSize = true;
            this.etiquetaBienVenida.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaBienVenida.Location = new System.Drawing.Point(238, 111);
            this.etiquetaBienVenida.Name = "etiquetaBienVenida";
            this.etiquetaBienVenida.Size = new System.Drawing.Size(93, 32);
            this.etiquetaBienVenida.TabIndex = 4;
            this.etiquetaBienVenida.Text = "label1";
            // 
            // etiquetaSucursal
            // 
            this.etiquetaSucursal.AutoEllipsis = true;
            this.etiquetaSucursal.AutoSize = true;
            this.etiquetaSucursal.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSucursal.ForeColor = System.Drawing.Color.SteelBlue;
            this.etiquetaSucursal.Location = new System.Drawing.Point(695, 9);
            this.etiquetaSucursal.Name = "etiquetaSucursal";
            this.etiquetaSucursal.Size = new System.Drawing.Size(77, 38);
            this.etiquetaSucursal.TabIndex = 0;
            this.etiquetaSucursal.Text = "0000";
            // 
            // FrmIngresoEgreso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.etiquetaBienVenida);
            this.Controls.Add(this.Picture);
            this.Controls.Add(this.StatusText);
            this.Controls.Add(this.Prompt);
            this.Controls.Add(this.etiquetaSucursal);
            this.Controls.Add(this.StatusLine);
            this.Name = "FrmIngresoEgreso";
            this.Text = "FrmIngresoEgreso";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            this.Load += new System.EventHandler(this.CaptureForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Global.Controles.EtiquetaTitulo StatusLine;
        private Global.Controles.TextoSimple Prompt;
        private Global.Controles.TextoSimple StatusText;
        private System.Windows.Forms.PictureBox Picture;
        private System.Windows.Forms.Label etiquetaBienVenida;
        private Global.Controles.EtiquetaTitulo etiquetaSucursal;
    }
}