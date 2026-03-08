namespace Acceso.uAreu
{
    partial class VerificationForm
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
            this.VerificationControl = new DPFP.Gui.Verification.VerificationControl();
            this.lblPromopt = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // VerificationControl
            // 
            this.VerificationControl.Active = true;
            this.VerificationControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.VerificationControl.Location = new System.Drawing.Point(9, 18);
            this.VerificationControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.VerificationControl.Name = "VerificationControl";
            this.VerificationControl.ReaderSerialNumber = "00000000-0000-0000-0000-000000000000";
            this.VerificationControl.Size = new System.Drawing.Size(48, 47);
            this.VerificationControl.TabIndex = 0;
            this.VerificationControl.OnComplete += new DPFP.Gui.Verification.VerificationControl._OnComplete(this.OnComplete);
            // 
            // lblPromopt
            // 
            this.lblPromopt.Location = new System.Drawing.Point(74, 18);
            this.lblPromopt.Name = "lblPromopt";
            this.lblPromopt.Size = new System.Drawing.Size(456, 57);
            this.lblPromopt.TabIndex = 1;
            this.lblPromopt.Text = "para verificar su identidad toque el  lector con cualquiera de sus dedos registra" +
    "dos";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(455, 78);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "Cerrar";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // VerificationForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 125);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.lblPromopt);
            this.Controls.Add(this.VerificationControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VerificationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Verificación de huella";
            this.ResumeLayout(false);

        }

        #endregion

        private DPFP.Gui.Verification.VerificationControl VerificationControl;
        private System.Windows.Forms.Label lblPromopt;
        private System.Windows.Forms.Button CloseButton;
    }
}