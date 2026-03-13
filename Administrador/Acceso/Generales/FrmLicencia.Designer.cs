namespace Acceso.Generales
{
    partial class FrmLicencia
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblEstadoValor;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox grpActivacion;
        private System.Windows.Forms.Label lblCodigoLabel;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Button btnActivar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblEstadoValor = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.grpActivacion = new System.Windows.Forms.GroupBox();
            this.lblCodigoLabel = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.btnActivar = new System.Windows.Forms.Button();
            this.grpActivacion.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.lblTitulo.Location = new System.Drawing.Point(30, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(500, 35);
            this.lblTitulo.Text = "Licencia Digital One";
            //
            // lblEstadoValor
            //
            this.lblEstadoValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblEstadoValor.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblEstadoValor.Location = new System.Drawing.Point(30, 58);
            this.lblEstadoValor.Name = "lblEstadoValor";
            this.lblEstadoValor.Size = new System.Drawing.Size(500, 25);
            this.lblEstadoValor.Text = "";
            //
            // lblInfo
            //
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(200, 200, 210);
            this.lblInfo.Location = new System.Drawing.Point(30, 95);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(500, 120);
            this.lblInfo.Text = "";
            //
            // grpActivacion
            //
            this.grpActivacion.Controls.Add(this.lblCodigoLabel);
            this.grpActivacion.Controls.Add(this.txtCodigo);
            this.grpActivacion.Controls.Add(this.btnActivar);
            this.grpActivacion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grpActivacion.ForeColor = System.Drawing.Color.FromArgb(200, 200, 210);
            this.grpActivacion.Location = new System.Drawing.Point(30, 230);
            this.grpActivacion.Name = "grpActivacion";
            this.grpActivacion.Size = new System.Drawing.Size(500, 110);
            this.grpActivacion.TabIndex = 0;
            this.grpActivacion.Text = "Activar o Renovar Licencia";
            //
            // lblCodigoLabel
            //
            this.lblCodigoLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCodigoLabel.ForeColor = System.Drawing.Color.FromArgb(200, 200, 210);
            this.lblCodigoLabel.Location = new System.Drawing.Point(15, 30);
            this.lblCodigoLabel.Name = "lblCodigoLabel";
            this.lblCodigoLabel.Size = new System.Drawing.Size(140, 22);
            this.lblCodigoLabel.Text = "Codigo de activacion:";
            //
            // txtCodigo
            //
            this.txtCodigo.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtCodigo.Location = new System.Drawing.Point(15, 55);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(310, 26);
            this.txtCodigo.TabIndex = 0;
            //
            // btnActivar
            //
            this.btnActivar.BackColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.btnActivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivar.FlatAppearance.BorderSize = 0;
            this.btnActivar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnActivar.ForeColor = System.Drawing.Color.FromArgb(11, 17, 32);
            this.btnActivar.Location = new System.Drawing.Point(340, 52);
            this.btnActivar.Name = "btnActivar";
            this.btnActivar.Size = new System.Drawing.Size(145, 32);
            this.btnActivar.TabIndex = 1;
            this.btnActivar.Text = "Activar / Renovar";
            this.btnActivar.UseVisualStyleBackColor = false;
            this.btnActivar.Click += new System.EventHandler(this.btnActivar_Click);
            //
            // FrmLicencia
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(11, 17, 32);
            this.ClientSize = new System.Drawing.Size(570, 370);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEstadoValor);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.grpActivacion);
            this.Name = "FrmLicencia";
            this.Text = "Licencias";
            this.Load += new System.EventHandler(this.FrmLicencia_Load);
            this.grpActivacion.ResumeLayout(false);
            this.grpActivacion.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
