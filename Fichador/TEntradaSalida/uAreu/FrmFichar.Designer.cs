
namespace Acceso.uAreu
{
    partial class FrmFichar
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFichar));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerHora = new System.Windows.Forms.Timer(this.components);
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblHora = new Global.Controles.EtiquetaTitulo();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.etiquetaSucursal = new Global.Controles.EtiquetaTitulo();
            this.lblFecha = new Global.Controles.EtiquetaTitulo();
            this.lblEstado = new System.Windows.Forms.Label();
            this.etiquetaES = new System.Windows.Forms.Label();
            this.PictureHuella = new System.Windows.Forms.PictureBox();
            this.etiquetaNombre = new System.Windows.Forms.Label();
            this.lblEmpresa = new System.Windows.Forms.Label();
            // PIN controls
            this.panelPin = new System.Windows.Forms.Panel();
            this.lblPinTitulo = new System.Windows.Forms.Label();
            this.lblLegajoId = new System.Windows.Forms.Label();
            this.txtLegajoId = new System.Windows.Forms.TextBox();
            this.lblPin = new System.Windows.Forms.Label();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.btnFicharPin = new System.Windows.Forms.Button();
            this.lblPinError = new System.Windows.Forms.Label();
            // Demo controls
            this.panelDemo = new System.Windows.Forms.Panel();
            this.lblDemoTitulo = new System.Windows.Forms.Label();
            this.lblDemoBanner = new System.Windows.Forms.Label();
            this.cmbLegajosDemo = new System.Windows.Forms.ComboBox();
            this.btnFicharDemo = new System.Windows.Forms.Button();
            // Mode switch
            this.lnkCambiarModo = new System.Windows.Forms.LinkLabel();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureHuella)).BeginInit();
            this.panelPin.SuspendLayout();
            this.panelDemo.SuspendLayout();
            this.SuspendLayout();
            //
            // timer
            //
            this.timer.Interval = 3000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            //
            // timerHora
            //
            this.timerHora.Tick += new System.EventHandler(this.timerHora_Tick);
            //
            // panelHeader
            //
            this.panelHeader.BackColor = System.Drawing.Color.RoyalBlue;
            this.panelHeader.Controls.Add(this.lblHora);
            this.panelHeader.Controls.Add(this.pictureBox1);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(510, 90);
            this.panelHeader.TabIndex = 0;
            //
            // lblHora
            //
            this.lblHora.AutoEllipsis = true;
            this.lblHora.BackColor = System.Drawing.Color.Transparent;
            this.lblHora.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHora.ForeColor = System.Drawing.Color.White;
            this.lblHora.Location = new System.Drawing.Point(0, 0);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(510, 90);
            this.lblHora.TabIndex = 1;
            this.lblHora.Text = "00:00:00";
            this.lblHora.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pictureBox1
            //
            this.pictureBox1.Image = global::TEntradaSalida.Properties.Resources.Logo_Solo;
            this.pictureBox1.Location = new System.Drawing.Point(8, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(46, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            //
            // etiquetaSucursal
            //
            this.etiquetaSucursal.AutoEllipsis = true;
            this.etiquetaSucursal.BackColor = System.Drawing.Color.RoyalBlue;
            this.etiquetaSucursal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.etiquetaSucursal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSucursal.ForeColor = System.Drawing.Color.White;
            this.etiquetaSucursal.Location = new System.Drawing.Point(0, 515);
            this.etiquetaSucursal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaSucursal.Name = "etiquetaSucursal";
            this.etiquetaSucursal.Size = new System.Drawing.Size(510, 55);
            this.etiquetaSucursal.TabIndex = 7;
            this.etiquetaSucursal.Text = "0000";
            this.etiquetaSucursal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblEmpresa
            //
            this.lblEmpresa.AutoEllipsis = true;
            this.lblEmpresa.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpresa.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblEmpresa.Location = new System.Drawing.Point(5, 93);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(500, 25);
            this.lblEmpresa.TabIndex = 8;
            this.lblEmpresa.Text = "";
            this.lblEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblFecha
            //
            this.lblFecha.AutoEllipsis = true;
            this.lblFecha.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblFecha.Location = new System.Drawing.Point(5, 123);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(500, 38);
            this.lblFecha.TabIndex = 2;
            this.lblFecha.Text = "01/01/2026";
            this.lblFecha.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblEstado
            //
            this.lblEstado.AutoEllipsis = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.ForeColor = System.Drawing.Color.DimGray;
            this.lblEstado.Location = new System.Drawing.Point(5, 167);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(500, 30);
            this.lblEstado.TabIndex = 3;
            this.lblEstado.Text = "";
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // PictureHuella
            //
            this.PictureHuella.BackColor = System.Drawing.Color.White;
            this.PictureHuella.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PictureHuella.Image = global::TEntradaSalida.Properties.Resources.Vidrio;
            this.PictureHuella.Location = new System.Drawing.Point(180, 205);
            this.PictureHuella.Margin = new System.Windows.Forms.Padding(2);
            this.PictureHuella.Name = "PictureHuella";
            this.PictureHuella.Size = new System.Drawing.Size(150, 180);
            this.PictureHuella.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureHuella.TabIndex = 5;
            this.PictureHuella.TabStop = false;
            this.PictureHuella.Click += new System.EventHandler(this.PictureHuella_Click);
            //
            // panelPin
            //
            this.panelPin.Location = new System.Drawing.Point(55, 200);
            this.panelPin.Name = "panelPin";
            this.panelPin.Size = new System.Drawing.Size(400, 190);
            this.panelPin.TabIndex = 20;
            this.panelPin.Visible = false;
            this.panelPin.Controls.Add(this.lblPinTitulo);
            this.panelPin.Controls.Add(this.lblLegajoId);
            this.panelPin.Controls.Add(this.txtLegajoId);
            this.panelPin.Controls.Add(this.lblPin);
            this.panelPin.Controls.Add(this.txtPin);
            this.panelPin.Controls.Add(this.btnFicharPin);
            this.panelPin.Controls.Add(this.lblPinError);
            //
            // lblPinTitulo
            //
            this.lblPinTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPinTitulo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblPinTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblPinTitulo.Name = "lblPinTitulo";
            this.lblPinTitulo.Size = new System.Drawing.Size(400, 30);
            this.lblPinTitulo.Text = "Fichada por PIN";
            this.lblPinTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblLegajoId
            //
            this.lblLegajoId.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLegajoId.Location = new System.Drawing.Point(30, 40);
            this.lblLegajoId.Name = "lblLegajoId";
            this.lblLegajoId.Size = new System.Drawing.Size(120, 30);
            this.lblLegajoId.Text = "Nro. Legajo:";
            this.lblLegajoId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtLegajoId
            //
            this.txtLegajoId.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtLegajoId.Location = new System.Drawing.Point(155, 38);
            this.txtLegajoId.Name = "txtLegajoId";
            this.txtLegajoId.Size = new System.Drawing.Size(180, 32);
            this.txtLegajoId.TabIndex = 0;
            this.txtLegajoId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // lblPin
            //
            this.lblPin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPin.Location = new System.Drawing.Point(30, 80);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(120, 30);
            this.lblPin.Text = "PIN:";
            this.lblPin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtPin
            //
            this.txtPin.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtPin.Location = new System.Drawing.Point(155, 78);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(180, 32);
            this.txtPin.MaxLength = 6;
            this.txtPin.PasswordChar = '*';
            this.txtPin.TabIndex = 1;
            this.txtPin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // btnFicharPin
            //
            this.btnFicharPin.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnFicharPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFicharPin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFicharPin.ForeColor = System.Drawing.Color.White;
            this.btnFicharPin.Location = new System.Drawing.Point(115, 125);
            this.btnFicharPin.Name = "btnFicharPin";
            this.btnFicharPin.Size = new System.Drawing.Size(170, 40);
            this.btnFicharPin.TabIndex = 2;
            this.btnFicharPin.Text = "Fichar";
            this.btnFicharPin.UseVisualStyleBackColor = false;
            this.btnFicharPin.Click += new System.EventHandler(this.btnFicharPin_Click);
            //
            // lblPinError
            //
            this.lblPinError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPinError.ForeColor = System.Drawing.Color.Red;
            this.lblPinError.Location = new System.Drawing.Point(0, 170);
            this.lblPinError.Name = "lblPinError";
            this.lblPinError.Size = new System.Drawing.Size(400, 20);
            this.lblPinError.Text = "";
            this.lblPinError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // panelDemo
            //
            this.panelDemo.Location = new System.Drawing.Point(55, 200);
            this.panelDemo.Name = "panelDemo";
            this.panelDemo.Size = new System.Drawing.Size(400, 190);
            this.panelDemo.TabIndex = 21;
            this.panelDemo.Visible = false;
            this.panelDemo.Controls.Add(this.lblDemoTitulo);
            this.panelDemo.Controls.Add(this.lblDemoBanner);
            this.panelDemo.Controls.Add(this.cmbLegajosDemo);
            this.panelDemo.Controls.Add(this.btnFicharDemo);
            //
            // lblDemoTitulo
            //
            this.lblDemoTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDemoTitulo.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblDemoTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblDemoTitulo.Name = "lblDemoTitulo";
            this.lblDemoTitulo.Size = new System.Drawing.Size(400, 30);
            this.lblDemoTitulo.Text = "MODO DEMOSTRACION";
            this.lblDemoTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblDemoBanner
            //
            this.lblDemoBanner.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDemoBanner.ForeColor = System.Drawing.Color.DimGray;
            this.lblDemoBanner.Location = new System.Drawing.Point(0, 30);
            this.lblDemoBanner.Name = "lblDemoBanner";
            this.lblDemoBanner.Size = new System.Drawing.Size(400, 20);
            this.lblDemoBanner.Text = "Seleccione un legajo para simular la fichada";
            this.lblDemoBanner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // cmbLegajosDemo
            //
            this.cmbLegajosDemo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLegajosDemo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbLegajosDemo.Location = new System.Drawing.Point(50, 65);
            this.cmbLegajosDemo.Name = "cmbLegajosDemo";
            this.cmbLegajosDemo.Size = new System.Drawing.Size(300, 29);
            this.cmbLegajosDemo.TabIndex = 0;
            //
            // btnFicharDemo
            //
            this.btnFicharDemo.BackColor = System.Drawing.Color.DarkOrange;
            this.btnFicharDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFicharDemo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFicharDemo.ForeColor = System.Drawing.Color.White;
            this.btnFicharDemo.Location = new System.Drawing.Point(115, 115);
            this.btnFicharDemo.Name = "btnFicharDemo";
            this.btnFicharDemo.Size = new System.Drawing.Size(170, 40);
            this.btnFicharDemo.TabIndex = 1;
            this.btnFicharDemo.Text = "Simular Fichada";
            this.btnFicharDemo.UseVisualStyleBackColor = false;
            this.btnFicharDemo.Click += new System.EventHandler(this.btnFicharDemo_Click);
            //
            // lnkCambiarModo
            //
            this.lnkCambiarModo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkCambiarModo.Location = new System.Drawing.Point(5, 493);
            this.lnkCambiarModo.Name = "lnkCambiarModo";
            this.lnkCambiarModo.Size = new System.Drawing.Size(500, 20);
            this.lnkCambiarModo.TabIndex = 22;
            this.lnkCambiarModo.TabStop = true;
            this.lnkCambiarModo.Text = "";
            this.lnkCambiarModo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkCambiarModo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCambiarModo_LinkClicked);
            //
            // etiquetaES
            //
            this.etiquetaES.AutoEllipsis = true;
            this.etiquetaES.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaES.ForeColor = System.Drawing.Color.RoyalBlue;
            this.etiquetaES.Location = new System.Drawing.Point(5, 393);
            this.etiquetaES.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaES.Name = "etiquetaES";
            this.etiquetaES.Size = new System.Drawing.Size(500, 52);
            this.etiquetaES.TabIndex = 4;
            this.etiquetaES.Text = "";
            this.etiquetaES.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // etiquetaNombre
            //
            this.etiquetaNombre.AutoEllipsis = true;
            this.etiquetaNombre.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaNombre.ForeColor = System.Drawing.Color.DimGray;
            this.etiquetaNombre.Location = new System.Drawing.Point(5, 453);
            this.etiquetaNombre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaNombre.Name = "etiquetaNombre";
            this.etiquetaNombre.Size = new System.Drawing.Size(500, 42);
            this.etiquetaNombre.TabIndex = 6;
            this.etiquetaNombre.Text = "";
            this.etiquetaNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // FrmFichar
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(510, 595);
            this.Controls.Add(this.lnkCambiarModo);
            this.Controls.Add(this.panelDemo);
            this.Controls.Add(this.panelPin);
            this.Controls.Add(this.etiquetaNombre);
            this.Controls.Add(this.etiquetaES);
            this.Controls.Add(this.PictureHuella);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.lblEmpresa);
            this.Controls.Add(this.etiquetaSucursal);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmFichar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DigitalPlus Fichadas";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            this.Load += new System.EventHandler(this.CaptureForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureHuella)).EndInit();
            this.panelPin.ResumeLayout(false);
            this.panelPin.PerformLayout();
            this.panelDemo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timerHora;
        private System.Windows.Forms.Panel panelHeader;
        private Global.Controles.EtiquetaTitulo lblHora;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Global.Controles.EtiquetaTitulo etiquetaSucursal;
        private Global.Controles.EtiquetaTitulo lblFecha;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.PictureBox PictureHuella;
        private System.Windows.Forms.Label etiquetaES;
        private System.Windows.Forms.Label etiquetaNombre;
        private System.Windows.Forms.Label lblEmpresa;
        // PIN controls
        private System.Windows.Forms.Panel panelPin;
        private System.Windows.Forms.Label lblPinTitulo;
        private System.Windows.Forms.Label lblLegajoId;
        private System.Windows.Forms.TextBox txtLegajoId;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.Button btnFicharPin;
        private System.Windows.Forms.Label lblPinError;
        // Demo controls
        private System.Windows.Forms.Panel panelDemo;
        private System.Windows.Forms.Label lblDemoTitulo;
        private System.Windows.Forms.Label lblDemoBanner;
        private System.Windows.Forms.ComboBox cmbLegajosDemo;
        private System.Windows.Forms.Button btnFicharDemo;
        // Mode switch
        private System.Windows.Forms.LinkLabel lnkCambiarModo;
    }
}
