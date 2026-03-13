
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
            this.picLogoIntegraIA = new System.Windows.Forms.PictureBox();
            this.etiquetaSucursal = new Global.Controles.EtiquetaTitulo();
            this.lblFecha = new Global.Controles.EtiquetaTitulo();
            this.lblEstado = new System.Windows.Forms.Label();
            this.etiquetaES = new System.Windows.Forms.Label();
            this.PictureHuella = new System.Windows.Forms.PictureBox();
            this.etiquetaNombre = new System.Windows.Forms.Label();
            this.lblEmpresa = new System.Windows.Forms.Label();
            // Semaforo
            this.panelSemaforo = new System.Windows.Forms.Panel();
            this.lblSemaforoRojo = new System.Windows.Forms.Label();
            this.lblSemaforoAmarillo = new System.Windows.Forms.Label();
            this.lblSemaforoVerde = new System.Windows.Forms.Label();
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
            // Links
            this.lnkCambiarPin = new System.Windows.Forms.LinkLabel();
            this.lnkCambiarModo = new System.Windows.Forms.LinkLabel();
            // Separador
            this.panelSeparador = new System.Windows.Forms.Panel();

            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoIntegraIA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureHuella)).BeginInit();
            this.panelPin.SuspendLayout();
            this.panelDemo.SuspendLayout();
            this.panelSemaforo.SuspendLayout();
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
            // ============================================================
            // panelHeader - Barra superior oscura
            // ============================================================
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(11, 17, 32);
            this.panelHeader.Controls.Add(this.pictureBox1);
            this.panelHeader.Controls.Add(this.picLogoIntegraIA);
            this.panelHeader.Controls.Add(this.lblHora);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(530, 90);
            this.panelHeader.TabIndex = 0;
            //
            // lblHora
            //
            this.lblHora.AutoEllipsis = true;
            this.lblHora.BackColor = System.Drawing.Color.Transparent;
            this.lblHora.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            this.lblHora.ForeColor = System.Drawing.Color.White;
            this.lblHora.Location = new System.Drawing.Point(0, 0);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(530, 90);
            this.lblHora.TabIndex = 1;
            this.lblHora.Text = "00:00:00";
            this.lblHora.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pictureBox1 - Logo empresa (izquierda)
            //
            this.pictureBox1.Image = global::TEntradaSalida.Properties.Resources.Logo_Solo;
            this.pictureBox1.Location = new System.Drawing.Point(10, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(3);
            //
            // picLogoIntegraIA - Logo IntegraIA (derecha)
            //
            this.picLogoIntegraIA.Location = new System.Drawing.Point(468, 19);
            this.picLogoIntegraIA.Name = "picLogoIntegraIA";
            this.picLogoIntegraIA.Size = new System.Drawing.Size(52, 52);
            this.picLogoIntegraIA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogoIntegraIA.TabIndex = 2;
            this.picLogoIntegraIA.TabStop = false;
            this.picLogoIntegraIA.BackColor = System.Drawing.Color.White;
            this.picLogoIntegraIA.Padding = new System.Windows.Forms.Padding(3);
            // ============================================================
            // Zona empresa + fecha
            // ============================================================
            //
            // lblEmpresa
            //
            this.lblEmpresa.AutoEllipsis = true;
            this.lblEmpresa.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblEmpresa.ForeColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.lblEmpresa.Location = new System.Drawing.Point(5, 96);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(520, 28);
            this.lblEmpresa.TabIndex = 8;
            this.lblEmpresa.Text = "";
            this.lblEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblFecha
            //
            this.lblFecha.AutoEllipsis = true;
            this.lblFecha.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.lblFecha.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblFecha.Location = new System.Drawing.Point(5, 124);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(520, 30);
            this.lblFecha.TabIndex = 2;
            this.lblFecha.Text = "01/01/2026";
            this.lblFecha.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // panelSeparador - linea sutil debajo de fecha
            //
            this.panelSeparador.BackColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.panelSeparador.Location = new System.Drawing.Point(165, 157);
            this.panelSeparador.Name = "panelSeparador";
            this.panelSeparador.Size = new System.Drawing.Size(200, 2);
            this.panelSeparador.TabIndex = 30;
            // ============================================================
            // Semaforo visual - 3 circulos
            // ============================================================
            //
            // panelSemaforo
            //
            this.panelSemaforo.BackColor = System.Drawing.Color.FromArgb(248, 247, 244);
            this.panelSemaforo.Location = new System.Drawing.Point(133, 160);
            this.panelSemaforo.Name = "panelSemaforo";
            this.panelSemaforo.Size = new System.Drawing.Size(264, 42);
            this.panelSemaforo.TabIndex = 31;
            this.panelSemaforo.Controls.Add(this.lblSemaforoRojo);
            this.panelSemaforo.Controls.Add(this.lblSemaforoAmarillo);
            this.panelSemaforo.Controls.Add(this.lblSemaforoVerde);
            //
            // lblSemaforoRojo
            //
            this.lblSemaforoRojo.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.lblSemaforoRojo.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblSemaforoRojo.Location = new System.Drawing.Point(8, -8);
            this.lblSemaforoRojo.Name = "lblSemaforoRojo";
            this.lblSemaforoRojo.Size = new System.Drawing.Size(72, 56);
            this.lblSemaforoRojo.Text = "\u25CF";
            this.lblSemaforoRojo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblSemaforoAmarillo
            //
            this.lblSemaforoAmarillo.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.lblSemaforoAmarillo.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblSemaforoAmarillo.Location = new System.Drawing.Point(96, -8);
            this.lblSemaforoAmarillo.Name = "lblSemaforoAmarillo";
            this.lblSemaforoAmarillo.Size = new System.Drawing.Size(72, 56);
            this.lblSemaforoAmarillo.Text = "\u25CF";
            this.lblSemaforoAmarillo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblSemaforoVerde
            //
            this.lblSemaforoVerde.Font = new System.Drawing.Font("Segoe UI", 36F);
            this.lblSemaforoVerde.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblSemaforoVerde.Location = new System.Drawing.Point(184, -8);
            this.lblSemaforoVerde.Name = "lblSemaforoVerde";
            this.lblSemaforoVerde.Size = new System.Drawing.Size(72, 56);
            this.lblSemaforoVerde.Text = "\u25CF";
            this.lblSemaforoVerde.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblEstado
            //
            this.lblEstado.AutoEllipsis = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(100, 100, 110);
            this.lblEstado.Location = new System.Drawing.Point(5, 200);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(520, 25);
            this.lblEstado.TabIndex = 3;
            this.lblEstado.Text = "";
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // ============================================================
            // panelLector - Carcasa del lector de huellas
            // ============================================================
            this.panelLector = new System.Windows.Forms.Panel();
            this.lblLectorLed = new System.Windows.Forms.Label();
            this.lblLectorBrand = new System.Windows.Forms.Label();
            this.panelLector.SuspendLayout();
            this.panelLector.BackColor = System.Drawing.Color.FromArgb(26, 26, 46);
            this.panelLector.Location = new System.Drawing.Point(180, 228);
            this.panelLector.Name = "panelLector";
            this.panelLector.Size = new System.Drawing.Size(170, 210);
            this.panelLector.TabIndex = 32;
            //
            // lblLectorLed - LED indicador en la parte superior del lector
            //
            this.lblLectorLed.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblLectorLed.Location = new System.Drawing.Point(70, 10);
            this.lblLectorLed.Name = "lblLectorLed";
            this.lblLectorLed.Size = new System.Drawing.Size(30, 6);
            this.lblLectorLed.TabIndex = 0;
            //
            // PictureHuella - Area de captura ovalada dentro del lector
            //
            this.PictureHuella.BackColor = System.Drawing.Color.FromArgb(220, 218, 210);
            this.PictureHuella.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PictureHuella.Image = global::TEntradaSalida.Properties.Resources.Vidrio;
            this.PictureHuella.Location = new System.Drawing.Point(15, 24);
            this.PictureHuella.Margin = new System.Windows.Forms.Padding(0);
            this.PictureHuella.Name = "PictureHuella";
            this.PictureHuella.Size = new System.Drawing.Size(140, 160);
            this.PictureHuella.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureHuella.TabIndex = 1;
            this.PictureHuella.TabStop = false;
            this.PictureHuella.Click += new System.EventHandler(this.PictureHuella_Click);
            //
            // lblLectorBrand - Texto marca en la base del lector
            //
            this.lblLectorBrand.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Bold);
            this.lblLectorBrand.ForeColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.lblLectorBrand.Location = new System.Drawing.Point(0, 190);
            this.lblLectorBrand.Name = "lblLectorBrand";
            this.lblLectorBrand.Size = new System.Drawing.Size(170, 16);
            this.lblLectorBrand.Text = "D I G I T A L   O N E";
            this.lblLectorBrand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // panelLector - agregar controles hijos
            //
            this.panelLector.Controls.Add(this.lblLectorLed);
            this.panelLector.Controls.Add(this.PictureHuella);
            this.panelLector.Controls.Add(this.lblLectorBrand);
            // ============================================================
            // panelPin - Card con fondo blanco y padding
            // ============================================================
            this.panelPin.BackColor = System.Drawing.Color.White;
            this.panelPin.Location = new System.Drawing.Point(55, 232);
            this.panelPin.Name = "panelPin";
            this.panelPin.Size = new System.Drawing.Size(420, 200);
            this.panelPin.Padding = new System.Windows.Forms.Padding(10);
            this.panelPin.TabIndex = 20;
            this.panelPin.Visible = false;
            this.panelPin.Controls.Add(this.lblPinTitulo);
            this.panelPin.Controls.Add(this.lblLegajoId);
            this.panelPin.Controls.Add(this.txtLegajoId);
            this.panelPin.Controls.Add(this.lblPin);
            this.panelPin.Controls.Add(this.txtPin);
            this.panelPin.Controls.Add(this.btnFicharPin);
            this.panelPin.Controls.Add(this.lblPinError);
            this.panelPin.Controls.Add(this.lnkCambiarPin);
            //
            // lblPinTitulo
            //
            this.lblPinTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblPinTitulo.ForeColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.lblPinTitulo.Location = new System.Drawing.Point(0, 8);
            this.lblPinTitulo.Name = "lblPinTitulo";
            this.lblPinTitulo.Size = new System.Drawing.Size(420, 30);
            this.lblPinTitulo.Text = "\u00A0\u00A0\uD83D\uDD12  Fichada por PIN";
            this.lblPinTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblLegajoId
            //
            this.lblLegajoId.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLegajoId.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblLegajoId.Location = new System.Drawing.Point(40, 48);
            this.lblLegajoId.Name = "lblLegajoId";
            this.lblLegajoId.Size = new System.Drawing.Size(120, 30);
            this.lblLegajoId.Text = "Nro. Legajo:";
            this.lblLegajoId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtLegajoId
            //
            this.txtLegajoId.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtLegajoId.Location = new System.Drawing.Point(165, 46);
            this.txtLegajoId.Name = "txtLegajoId";
            this.txtLegajoId.Size = new System.Drawing.Size(190, 31);
            this.txtLegajoId.TabIndex = 0;
            this.txtLegajoId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLegajoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //
            // lblPin
            //
            this.lblPin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPin.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblPin.Location = new System.Drawing.Point(40, 88);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(120, 30);
            this.lblPin.Text = "PIN:";
            this.lblPin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtPin
            //
            this.txtPin.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtPin.Location = new System.Drawing.Point(165, 86);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(190, 31);
            this.txtPin.MaxLength = 6;
            this.txtPin.PasswordChar = '*';
            this.txtPin.TabIndex = 1;
            this.txtPin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //
            // btnFicharPin
            //
            this.btnFicharPin.BackColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.btnFicharPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFicharPin.FlatAppearance.BorderSize = 0;
            this.btnFicharPin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFicharPin.ForeColor = System.Drawing.Color.FromArgb(11, 17, 32);
            this.btnFicharPin.Location = new System.Drawing.Point(110, 132);
            this.btnFicharPin.Name = "btnFicharPin";
            this.btnFicharPin.Size = new System.Drawing.Size(180, 42);
            this.btnFicharPin.TabIndex = 2;
            this.btnFicharPin.Text = "Fichar";
            this.btnFicharPin.UseVisualStyleBackColor = false;
            this.btnFicharPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFicharPin.Click += new System.EventHandler(this.btnFicharPin_Click);
            //
            // lblPinError
            //
            this.lblPinError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPinError.ForeColor = System.Drawing.Color.FromArgb(220, 50, 50);
            this.lblPinError.Location = new System.Drawing.Point(0, 178);
            this.lblPinError.Name = "lblPinError";
            this.lblPinError.Size = new System.Drawing.Size(420, 20);
            this.lblPinError.Text = "";
            this.lblPinError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lnkCambiarPin
            //
            this.lnkCambiarPin.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lnkCambiarPin.LinkColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.lnkCambiarPin.ActiveLinkColor = System.Drawing.Color.FromArgb(232, 201, 122);
            this.lnkCambiarPin.Location = new System.Drawing.Point(270, 140);
            this.lnkCambiarPin.Name = "lnkCambiarPin";
            this.lnkCambiarPin.Size = new System.Drawing.Size(130, 22);
            this.lnkCambiarPin.TabIndex = 3;
            this.lnkCambiarPin.TabStop = true;
            this.lnkCambiarPin.Text = "Cambiar mi PIN";
            this.lnkCambiarPin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkCambiarPin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCambiarPin_LinkClicked);
            // ============================================================
            // panelDemo - Card con fondo blanco
            // ============================================================
            this.panelDemo.BackColor = System.Drawing.Color.White;
            this.panelDemo.Location = new System.Drawing.Point(55, 232);
            this.panelDemo.Name = "panelDemo";
            this.panelDemo.Size = new System.Drawing.Size(420, 200);
            this.panelDemo.Padding = new System.Windows.Forms.Padding(10);
            this.panelDemo.TabIndex = 21;
            this.panelDemo.Visible = false;
            this.panelDemo.Controls.Add(this.lblDemoTitulo);
            this.panelDemo.Controls.Add(this.lblDemoBanner);
            this.panelDemo.Controls.Add(this.cmbLegajosDemo);
            this.panelDemo.Controls.Add(this.btnFicharDemo);
            //
            // lblDemoTitulo
            //
            this.lblDemoTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblDemoTitulo.ForeColor = System.Drawing.Color.FromArgb(232, 201, 122);
            this.lblDemoTitulo.Location = new System.Drawing.Point(0, 8);
            this.lblDemoTitulo.Name = "lblDemoTitulo";
            this.lblDemoTitulo.Size = new System.Drawing.Size(420, 30);
            this.lblDemoTitulo.Text = "\u26A0  MODO DEMOSTRACION";
            this.lblDemoTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblDemoBanner
            //
            this.lblDemoBanner.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDemoBanner.ForeColor = System.Drawing.Color.FromArgb(100, 100, 110);
            this.lblDemoBanner.Location = new System.Drawing.Point(0, 40);
            this.lblDemoBanner.Name = "lblDemoBanner";
            this.lblDemoBanner.Size = new System.Drawing.Size(420, 20);
            this.lblDemoBanner.Text = "Seleccione un legajo para simular la fichada";
            this.lblDemoBanner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // cmbLegajosDemo
            //
            this.cmbLegajosDemo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLegajosDemo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbLegajosDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLegajosDemo.Location = new System.Drawing.Point(60, 72);
            this.cmbLegajosDemo.Name = "cmbLegajosDemo";
            this.cmbLegajosDemo.Size = new System.Drawing.Size(300, 29);
            this.cmbLegajosDemo.TabIndex = 0;
            //
            // btnFicharDemo
            //
            this.btnFicharDemo.BackColor = System.Drawing.Color.FromArgb(232, 201, 122);
            this.btnFicharDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFicharDemo.FlatAppearance.BorderSize = 0;
            this.btnFicharDemo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFicharDemo.ForeColor = System.Drawing.Color.FromArgb(11, 17, 32);
            this.btnFicharDemo.Location = new System.Drawing.Point(120, 120);
            this.btnFicharDemo.Name = "btnFicharDemo";
            this.btnFicharDemo.Size = new System.Drawing.Size(180, 42);
            this.btnFicharDemo.TabIndex = 1;
            this.btnFicharDemo.Text = "Simular Fichada";
            this.btnFicharDemo.UseVisualStyleBackColor = false;
            this.btnFicharDemo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFicharDemo.Click += new System.EventHandler(this.btnFicharDemo_Click);
            // ============================================================
            // Zona resultado fichada
            // ============================================================
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.lblIniciales = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            //
            // etiquetaES - ENTRADA / SALIDA
            //
            this.etiquetaES.AutoEllipsis = true;
            this.etiquetaES.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.etiquetaES.ForeColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.etiquetaES.Location = new System.Drawing.Point(5, 445);
            this.etiquetaES.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaES.Name = "etiquetaES";
            this.etiquetaES.Size = new System.Drawing.Size(520, 45);
            this.etiquetaES.TabIndex = 4;
            this.etiquetaES.Text = "";
            this.etiquetaES.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // picAvatar - Foto circular del legajo
            //
            this.picAvatar.BackColor = System.Drawing.Color.FromArgb(26, 26, 46);
            this.picAvatar.Location = new System.Drawing.Point(55, 496);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(64, 64);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAvatar.TabIndex = 33;
            this.picAvatar.TabStop = false;
            this.picAvatar.Visible = false;
            //
            // lblIniciales - Iniciales cuando no hay foto
            //
            this.lblIniciales.BackColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.lblIniciales.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblIniciales.ForeColor = System.Drawing.Color.FromArgb(11, 17, 32);
            this.lblIniciales.Location = new System.Drawing.Point(55, 496);
            this.lblIniciales.Name = "lblIniciales";
            this.lblIniciales.Size = new System.Drawing.Size(64, 64);
            this.lblIniciales.Text = "";
            this.lblIniciales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblIniciales.Visible = false;
            //
            // etiquetaNombre
            //
            this.etiquetaNombre.AutoEllipsis = true;
            this.etiquetaNombre.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular);
            this.etiquetaNombre.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.etiquetaNombre.Location = new System.Drawing.Point(5, 496);
            this.etiquetaNombre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaNombre.Name = "etiquetaNombre";
            this.etiquetaNombre.Size = new System.Drawing.Size(520, 38);
            this.etiquetaNombre.TabIndex = 6;
            this.etiquetaNombre.Text = "";
            this.etiquetaNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lnkCambiarModo
            //
            this.lnkCambiarModo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkCambiarModo.LinkColor = System.Drawing.Color.FromArgb(201, 168, 76);
            this.lnkCambiarModo.ActiveLinkColor = System.Drawing.Color.FromArgb(232, 201, 122);
            this.lnkCambiarModo.Location = new System.Drawing.Point(5, 566);
            this.lnkCambiarModo.Name = "lnkCambiarModo";
            this.lnkCambiarModo.Size = new System.Drawing.Size(520, 20);
            this.lnkCambiarModo.TabIndex = 22;
            this.lnkCambiarModo.TabStop = true;
            this.lnkCambiarModo.Text = "";
            this.lnkCambiarModo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkCambiarModo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCambiarModo_LinkClicked);
            // ============================================================
            // etiquetaSucursal - Barra inferior oscura
            // ============================================================
            this.etiquetaSucursal.AutoEllipsis = true;
            this.etiquetaSucursal.BackColor = System.Drawing.Color.FromArgb(11, 17, 32);
            this.etiquetaSucursal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.etiquetaSucursal.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular);
            this.etiquetaSucursal.ForeColor = System.Drawing.Color.FromArgb(232, 201, 122);
            this.etiquetaSucursal.Location = new System.Drawing.Point(0, 558);
            this.etiquetaSucursal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaSucursal.Name = "etiquetaSucursal";
            this.etiquetaSucursal.Size = new System.Drawing.Size(530, 50);
            this.etiquetaSucursal.TabIndex = 7;
            this.etiquetaSucursal.Text = "0000";
            this.etiquetaSucursal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // ============================================================
            // FrmFichar
            // ============================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 247, 244);
            this.ClientSize = new System.Drawing.Size(530, 640);
            this.Controls.Add(this.picAvatar);
            this.Controls.Add(this.lblIniciales);
            this.Controls.Add(this.panelSemaforo);
            this.Controls.Add(this.panelSeparador);
            this.Controls.Add(this.lnkCambiarModo);
            this.Controls.Add(this.panelDemo);
            this.Controls.Add(this.panelPin);
            this.Controls.Add(this.etiquetaNombre);
            this.Controls.Add(this.etiquetaES);
            this.Controls.Add(this.panelLector);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.lblEmpresa);
            this.Controls.Add(this.etiquetaSucursal);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmFichar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Digital One";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            this.Load += new System.EventHandler(this.CaptureForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoIntegraIA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureHuella)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.panelPin.ResumeLayout(false);
            this.panelPin.PerformLayout();
            this.panelDemo.ResumeLayout(false);
            this.panelSemaforo.ResumeLayout(false);
            this.panelLector.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timerHora;
        private System.Windows.Forms.Panel panelHeader;
        private Global.Controles.EtiquetaTitulo lblHora;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox picLogoIntegraIA;
        private Global.Controles.EtiquetaTitulo etiquetaSucursal;
        private Global.Controles.EtiquetaTitulo lblFecha;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.PictureBox PictureHuella;
        private System.Windows.Forms.Label etiquetaES;
        private System.Windows.Forms.Label etiquetaNombre;
        private System.Windows.Forms.Label lblEmpresa;
        // Semaforo
        private System.Windows.Forms.Panel panelSemaforo;
        private System.Windows.Forms.Label lblSemaforoRojo;
        private System.Windows.Forms.Label lblSemaforoAmarillo;
        private System.Windows.Forms.Label lblSemaforoVerde;
        // Separador
        private System.Windows.Forms.Panel panelSeparador;
        // Lector de huellas visual
        private System.Windows.Forms.Panel panelLector;
        private System.Windows.Forms.Label lblLectorLed;
        private System.Windows.Forms.Label lblLectorBrand;
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
        // Links
        private System.Windows.Forms.LinkLabel lnkCambiarPin;
        private System.Windows.Forms.LinkLabel lnkCambiarModo;
        // Avatar
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Label lblIniciales;
    }
}
