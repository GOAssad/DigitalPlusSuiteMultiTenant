namespace DigitalPlus.Licensing
{
    partial class FrmLicenseBlocked
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Panel pnlActivation;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Button btnActivar;
        private System.Windows.Forms.Button btnReintentar;
        private System.Windows.Forms.Button btnSalir;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pnlActivation = new System.Windows.Forms.Panel();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.btnActivar = new System.Windows.Forms.Button();
            this.btnReintentar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.pnlActivation.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(200, 50, 50);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(440, 30);
            this.lblTitle.Text = "DigitalPlus - Licencia";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblMessage
            //
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMessage.Location = new System.Drawing.Point(20, 60);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(440, 80);
            this.lblMessage.Text = "";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlActivation
            //
            this.pnlActivation.Controls.Add(this.lblCodigo);
            this.pnlActivation.Controls.Add(this.txtCodigo);
            this.pnlActivation.Controls.Add(this.btnActivar);
            this.pnlActivation.Location = new System.Drawing.Point(20, 150);
            this.pnlActivation.Name = "pnlActivation";
            this.pnlActivation.Size = new System.Drawing.Size(440, 70);
            //
            // lblCodigo
            //
            this.lblCodigo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCodigo.Location = new System.Drawing.Point(5, 5);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(140, 22);
            this.lblCodigo.Text = "Codigo de activacion:";
            //
            // txtCodigo
            //
            this.txtCodigo.Font = new System.Drawing.Font("Consolas", 11F);
            this.txtCodigo.Location = new System.Drawing.Point(5, 30);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(320, 25);
            //
            // btnActivar
            //
            this.btnActivar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnActivar.Location = new System.Drawing.Point(335, 28);
            this.btnActivar.Name = "btnActivar";
            this.btnActivar.Size = new System.Drawing.Size(95, 30);
            this.btnActivar.Text = "Activar";
            this.btnActivar.UseVisualStyleBackColor = true;
            this.btnActivar.Click += new System.EventHandler(this.btnActivar_Click);
            //
            // btnReintentar
            //
            this.btnReintentar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReintentar.Location = new System.Drawing.Point(140, 240);
            this.btnReintentar.Name = "btnReintentar";
            this.btnReintentar.Size = new System.Drawing.Size(110, 35);
            this.btnReintentar.Text = "Reintentar";
            this.btnReintentar.UseVisualStyleBackColor = true;
            this.btnReintentar.Click += new System.EventHandler(this.btnReintentar_Click);
            //
            // btnSalir
            //
            this.btnSalir.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSalir.Location = new System.Drawing.Point(260, 240);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(110, 35);
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            //
            // FrmLicenseBlocked
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 295);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.pnlActivation);
            this.Controls.Add(this.btnReintentar);
            this.Controls.Add(this.btnSalir);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLicenseBlocked";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DigitalPlus - Licencia";
            this.pnlActivation.ResumeLayout(false);
            this.pnlActivation.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
