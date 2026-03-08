namespace Acceso.uAreu
{
    partial class EnrollmentForm
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
            this.EnrollmentControl = new DPFP.Gui.Enrollment.EnrollmentControl();
            this.GroupEvents = new System.Windows.Forms.GroupBox();
            this.ListEvents = new System.Windows.Forms.ListBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.GroupEvents.SuspendLayout();
            this.SuspendLayout();
            // 
            // EnrollmentControl
            // 
            this.EnrollmentControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EnrollmentControl.EnrolledFingerMask = 0;
            this.EnrollmentControl.Location = new System.Drawing.Point(13, 3);
            this.EnrollmentControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EnrollmentControl.MaxEnrollFingerCount = 10;
            this.EnrollmentControl.Name = "EnrollmentControl";
            this.EnrollmentControl.ReaderSerialNumber = "00000000-0000-0000-0000-000000000000";
            this.EnrollmentControl.Size = new System.Drawing.Size(492, 314);
            this.EnrollmentControl.TabIndex = 0;
            this.EnrollmentControl.OnDelete += new DPFP.Gui.Enrollment.EnrollmentControl._OnDelete(this.EnrollmentControl_OnDelete);
            this.EnrollmentControl.OnEnroll += new DPFP.Gui.Enrollment.EnrollmentControl._OnEnroll(this.EnrollmentControl_OnEnroll);
            this.EnrollmentControl.OnFingerTouch += new DPFP.Gui.Enrollment.EnrollmentControl._OnFingerTouch(this.EnrollmentControl_OnFingerTouch);
            this.EnrollmentControl.OnFingerRemove += new DPFP.Gui.Enrollment.EnrollmentControl._OnFingerRemove(this.EnrollmentControl_OnFingerRemove);
            this.EnrollmentControl.OnComplete += new DPFP.Gui.Enrollment.EnrollmentControl._OnComplete(this.EnrollmentControl_OnComplete);
            this.EnrollmentControl.OnReaderConnect += new DPFP.Gui.Enrollment.EnrollmentControl._OnReaderConnect(this.EnrollmentControl_OnReaderConnect);
            this.EnrollmentControl.OnReaderDisconnect += new DPFP.Gui.Enrollment.EnrollmentControl._OnReaderDisconnect(this.EnrollmentControl_OnReaderDisconnect);
            this.EnrollmentControl.OnSampleQuality += new DPFP.Gui.Enrollment.EnrollmentControl._OnSampleQuality(this.EnrollmentControl_OnSampleQuality);
            this.EnrollmentControl.OnStartEnroll += new DPFP.Gui.Enrollment.EnrollmentControl._OnStartEnroll(this.EnrollmentControl_OnStartEnroll);
            this.EnrollmentControl.OnCancelEnroll += new DPFP.Gui.Enrollment.EnrollmentControl._OnCancelEnroll(this.EnrollmentControl_OnCancelEnroll);
            // 
            // GroupEvents
            // 
            this.GroupEvents.Controls.Add(this.ListEvents);
            this.GroupEvents.Location = new System.Drawing.Point(12, 396);
            this.GroupEvents.Name = "GroupEvents";
            this.GroupEvents.Size = new System.Drawing.Size(661, 152);
            this.GroupEvents.TabIndex = 1;
            this.GroupEvents.TabStop = false;
            this.GroupEvents.Text = "Eventos";
            // 
            // ListEvents
            // 
            this.ListEvents.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ListEvents.FormattingEnabled = true;
            this.ListEvents.ItemHeight = 16;
            this.ListEvents.Location = new System.Drawing.Point(6, 22);
            this.ListEvents.Name = "ListEvents";
            this.ListEvents.Size = new System.Drawing.Size(649, 116);
            this.ListEvents.TabIndex = 0;
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CloseButton.Location = new System.Drawing.Point(554, 554);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(113, 43);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "Cerrar";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 554);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 43);
            this.button1.TabIndex = 3;
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EnrollmentForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 625);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.GroupEvents);
            this.Controls.Add(this.EnrollmentControl);
            this.Name = "EnrollmentForm";
            this.Text = "EnrollmentForm";
            this.Load += new System.EventHandler(this.EnrollmentForm_Load);
            this.GroupEvents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DPFP.Gui.Enrollment.EnrollmentControl EnrollmentControl;
        private System.Windows.Forms.GroupBox GroupEvents;
        private System.Windows.Forms.ListBox ListEvents;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button button1;
    }
}