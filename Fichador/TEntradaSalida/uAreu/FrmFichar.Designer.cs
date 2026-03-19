
namespace Acceso.uAreu
{
    partial class FrmFichar
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFichar));

            // Colores del tema
            var cDarkBg = System.Drawing.Color.FromArgb(13, 17, 28);        // fondo principal
            var cCardBg = System.Drawing.Color.FromArgb(22, 28, 48);        // cards/paneles
            var cHeaderBg = System.Drawing.Color.FromArgb(8, 11, 20);       // header/footer
            var cGold = System.Drawing.Color.FromArgb(201, 168, 76);        // acento dorado
            var cGoldLight = System.Drawing.Color.FromArgb(232, 201, 122);  // dorado claro
            var cTextPrimary = System.Drawing.Color.White;
            var cTextSecondary = System.Drawing.Color.FromArgb(140, 140, 170);
            var cSemaforoOff = System.Drawing.Color.FromArgb(40, 44, 60);

            // --- Instanciar todos los controles ---
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerHora = new System.Windows.Forms.Timer(this.components);
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblHora = new Global.Controles.EtiquetaTitulo();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picLogoIntegraIA = new System.Windows.Forms.PictureBox();
            this.lblEmpresa = new System.Windows.Forms.Label();
            this.lblFecha = new Global.Controles.EtiquetaTitulo();
            this.panelSeparador = new System.Windows.Forms.Panel();
            this.panelSemaforo = new System.Windows.Forms.Panel();
            this.lblSemaforoRojo = new System.Windows.Forms.Label();
            this.lblSemaforoAmarillo = new System.Windows.Forms.Label();
            this.lblSemaforoVerde = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.panelLector = new System.Windows.Forms.Panel();
            this.lblLectorLed = new System.Windows.Forms.Label();
            this.PictureHuella = new System.Windows.Forms.PictureBox();
            this.lblLectorBrand = new System.Windows.Forms.Label();
            this.panelPin = new System.Windows.Forms.Panel();
            this.lblPinTitulo = new System.Windows.Forms.Label();
            this.lblLegajoId = new System.Windows.Forms.Label();
            this.txtLegajoId = new System.Windows.Forms.TextBox();
            this.lblPin = new System.Windows.Forms.Label();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.btnFicharPin = new System.Windows.Forms.Button();
            this.lblPinError = new System.Windows.Forms.Label();
            this.lnkCambiarPin = new System.Windows.Forms.LinkLabel();
            this.panelDemo = new System.Windows.Forms.Panel();
            this.lblDemoTitulo = new System.Windows.Forms.Label();
            this.lblDemoBanner = new System.Windows.Forms.Label();
            this.cmbLegajosDemo = new System.Windows.Forms.ComboBox();
            this.btnFicharDemo = new System.Windows.Forms.Button();
            this.panelQR = new System.Windows.Forms.Panel();
            this.lblQrTitulo = new System.Windows.Forms.Label();
            this.picCamara = new System.Windows.Forms.PictureBox();
            this.lblQrInstruccion = new System.Windows.Forms.Label();
            this.etiquetaES = new System.Windows.Forms.Label();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.lblIniciales = new System.Windows.Forms.Label();
            this.etiquetaNombre = new System.Windows.Forms.Label();
            // Barra de modos
            this.panelModos = new System.Windows.Forms.Panel();
            this.btnModoHuella = new System.Windows.Forms.Label();
            this.btnModoPin = new System.Windows.Forms.Label();
            this.btnModoQR = new System.Windows.Forms.Label();
            this.btnModoDemo = new System.Windows.Forms.Label();
            // Reemplaza lnkCambiarModo (se mantiene oculto por compatibilidad)
            this.lnkCambiarModo = new System.Windows.Forms.LinkLabel();
            this.etiquetaSucursal = new Global.Controles.EtiquetaTitulo();

            // --- SuspendLayout ---
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoIntegraIA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureHuella)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCamara)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.panelPin.SuspendLayout();
            this.panelDemo.SuspendLayout();
            this.panelQR.SuspendLayout();
            this.panelLector.SuspendLayout();
            this.panelSemaforo.SuspendLayout();
            this.panelModos.SuspendLayout();
            this.SuspendLayout();

            // ==============================================================
            // TIMERS
            // ==============================================================
            this.timer.Interval = 3000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            this.timerHora.Tick += new System.EventHandler(this.timerHora_Tick);

            // ==============================================================
            // HEADER - Barra superior
            // ==============================================================
            this.panelHeader.BackColor = cHeaderBg;
            this.panelHeader.Controls.Add(this.pictureBox1);
            this.panelHeader.Controls.Add(this.picLogoIntegraIA);
            this.panelHeader.Controls.Add(this.lblHora);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(620, 80);
            this.panelHeader.TabIndex = 0;
            //
            // lblHora
            //
            this.lblHora.AutoEllipsis = true;
            this.lblHora.BackColor = System.Drawing.Color.Transparent;
            this.lblHora.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lblHora.ForeColor = cTextPrimary;
            this.lblHora.Location = new System.Drawing.Point(0, 0);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(620, 80);
            this.lblHora.TabIndex = 1;
            this.lblHora.Text = "00:00:00";
            this.lblHora.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pictureBox1 - Logo empresa
            //
            this.pictureBox1.Image = global::TEntradaSalida.Properties.Resources.Logo_Solo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(3);
            //
            // picLogoIntegraIA
            //
            this.picLogoIntegraIA.Location = new System.Drawing.Point(556, 14);
            this.picLogoIntegraIA.Name = "picLogoIntegraIA";
            this.picLogoIntegraIA.Size = new System.Drawing.Size(52, 52);
            this.picLogoIntegraIA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogoIntegraIA.TabIndex = 2;
            this.picLogoIntegraIA.TabStop = false;
            this.picLogoIntegraIA.BackColor = System.Drawing.Color.White;
            this.picLogoIntegraIA.Padding = new System.Windows.Forms.Padding(3);

            // ==============================================================
            // EMPRESA + FECHA
            // ==============================================================
            this.lblEmpresa.AutoEllipsis = true;
            this.lblEmpresa.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblEmpresa.ForeColor = cGold;
            this.lblEmpresa.BackColor = cDarkBg;
            this.lblEmpresa.Location = new System.Drawing.Point(5, 84);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(610, 24);
            this.lblEmpresa.TabIndex = 8;
            this.lblEmpresa.Text = "";
            this.lblEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.lblFecha.AutoEllipsis = true;
            this.lblFecha.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblFecha.ForeColor = cTextSecondary;
            this.lblFecha.BackColor = cDarkBg;
            this.lblFecha.Location = new System.Drawing.Point(5, 108);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(610, 22);
            this.lblFecha.TabIndex = 2;
            this.lblFecha.Text = "";
            this.lblFecha.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // panelSeparador
            //
            this.panelSeparador.BackColor = System.Drawing.Color.FromArgb(40, 44, 64);
            this.panelSeparador.Location = new System.Drawing.Point(145, 133);
            this.panelSeparador.Name = "panelSeparador";
            this.panelSeparador.Size = new System.Drawing.Size(330, 1);
            this.panelSeparador.TabIndex = 30;

            // ==============================================================
            // BARRA DE MODOS - Botones pill horizontales
            // ==============================================================
            this.panelModos.BackColor = System.Drawing.Color.FromArgb(16, 20, 36);
            this.panelModos.Location = new System.Drawing.Point(75, 140);
            this.panelModos.Name = "panelModos";
            this.panelModos.Size = new System.Drawing.Size(470, 36);
            this.panelModos.TabIndex = 40;
            this.panelModos.Controls.Add(this.btnModoHuella);
            this.panelModos.Controls.Add(this.btnModoPin);
            this.panelModos.Controls.Add(this.btnModoQR);
            this.panelModos.Controls.Add(this.btnModoDemo);
            //
            int modoW = 112, modoH = 30, modoY = 3, modoGap = 6;
            int modoX = 5;
            // btnModoHuella
            this.btnModoHuella.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnModoHuella.ForeColor = cTextSecondary;
            this.btnModoHuella.BackColor = System.Drawing.Color.FromArgb(30, 34, 54);
            this.btnModoHuella.Location = new System.Drawing.Point(modoX, modoY);
            this.btnModoHuella.Size = new System.Drawing.Size(modoW, modoH);
            this.btnModoHuella.Name = "btnModoHuella";
            this.btnModoHuella.Text = "\uD83D\uDD90  Huella";
            this.btnModoHuella.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnModoHuella.Cursor = System.Windows.Forms.Cursors.Hand;
            modoX += modoW + modoGap;
            // btnModoPin
            this.btnModoPin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnModoPin.ForeColor = cTextSecondary;
            this.btnModoPin.BackColor = System.Drawing.Color.FromArgb(30, 34, 54);
            this.btnModoPin.Location = new System.Drawing.Point(modoX, modoY);
            this.btnModoPin.Size = new System.Drawing.Size(modoW, modoH);
            this.btnModoPin.Name = "btnModoPin";
            this.btnModoPin.Text = "\uD83D\uDD12  PIN";
            this.btnModoPin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnModoPin.Cursor = System.Windows.Forms.Cursors.Hand;
            modoX += modoW + modoGap;
            // btnModoQR
            this.btnModoQR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnModoQR.ForeColor = cTextSecondary;
            this.btnModoQR.BackColor = System.Drawing.Color.FromArgb(30, 34, 54);
            this.btnModoQR.Location = new System.Drawing.Point(modoX, modoY);
            this.btnModoQR.Size = new System.Drawing.Size(modoW, modoH);
            this.btnModoQR.Name = "btnModoQR";
            this.btnModoQR.Text = "\uD83D\uDCF7  QR";
            this.btnModoQR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnModoQR.Cursor = System.Windows.Forms.Cursors.Hand;
            modoX += modoW + modoGap;
            // btnModoDemo
            this.btnModoDemo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnModoDemo.ForeColor = cTextSecondary;
            this.btnModoDemo.BackColor = System.Drawing.Color.FromArgb(30, 34, 54);
            this.btnModoDemo.Location = new System.Drawing.Point(modoX, modoY);
            this.btnModoDemo.Size = new System.Drawing.Size(modoW, modoH);
            this.btnModoDemo.Name = "btnModoDemo";
            this.btnModoDemo.Text = "\u26A0  Demo";
            this.btnModoDemo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnModoDemo.Cursor = System.Windows.Forms.Cursors.Hand;

            // ==============================================================
            // SEMAFORO - 3 circulos pequenos
            // ==============================================================
            this.panelSemaforo.BackColor = cDarkBg;
            this.panelSemaforo.Location = new System.Drawing.Point(215, 182);
            this.panelSemaforo.Name = "panelSemaforo";
            this.panelSemaforo.Size = new System.Drawing.Size(190, 28);
            this.panelSemaforo.TabIndex = 31;
            this.panelSemaforo.Controls.Add(this.lblSemaforoRojo);
            this.panelSemaforo.Controls.Add(this.lblSemaforoAmarillo);
            this.panelSemaforo.Controls.Add(this.lblSemaforoVerde);
            //
            this.lblSemaforoRojo.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.lblSemaforoRojo.ForeColor = cSemaforoOff;
            this.lblSemaforoRojo.Location = new System.Drawing.Point(8, -4);
            this.lblSemaforoRojo.Name = "lblSemaforoRojo";
            this.lblSemaforoRojo.Size = new System.Drawing.Size(50, 36);
            this.lblSemaforoRojo.Text = "\u25CF";
            this.lblSemaforoRojo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.lblSemaforoAmarillo.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.lblSemaforoAmarillo.ForeColor = cSemaforoOff;
            this.lblSemaforoAmarillo.Location = new System.Drawing.Point(70, -4);
            this.lblSemaforoAmarillo.Name = "lblSemaforoAmarillo";
            this.lblSemaforoAmarillo.Size = new System.Drawing.Size(50, 36);
            this.lblSemaforoAmarillo.Text = "\u25CF";
            this.lblSemaforoAmarillo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.lblSemaforoVerde.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.lblSemaforoVerde.ForeColor = cSemaforoOff;
            this.lblSemaforoVerde.Location = new System.Drawing.Point(132, -4);
            this.lblSemaforoVerde.Name = "lblSemaforoVerde";
            this.lblSemaforoVerde.Size = new System.Drawing.Size(50, 36);
            this.lblSemaforoVerde.Text = "\u25CF";
            this.lblSemaforoVerde.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblEstado
            //
            this.lblEstado.AutoEllipsis = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic);
            this.lblEstado.ForeColor = cTextSecondary;
            this.lblEstado.BackColor = cDarkBg;
            this.lblEstado.Location = new System.Drawing.Point(5, 212);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(610, 20);
            this.lblEstado.TabIndex = 3;
            this.lblEstado.Text = "";
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ==============================================================
            // PANEL LECTOR - Carcasa visual del lector de huellas
            // ==============================================================
            this.panelLector.BackColor = System.Drawing.Color.FromArgb(22, 22, 40);
            this.panelLector.Location = new System.Drawing.Point(225, 238);
            this.panelLector.Name = "panelLector";
            this.panelLector.Size = new System.Drawing.Size(170, 200);
            this.panelLector.TabIndex = 32;
            //
            this.lblLectorLed.BackColor = cSemaforoOff;
            this.lblLectorLed.Location = new System.Drawing.Point(70, 10);
            this.lblLectorLed.Name = "lblLectorLed";
            this.lblLectorLed.Size = new System.Drawing.Size(30, 6);
            this.lblLectorLed.TabIndex = 0;
            //
            this.PictureHuella.BackColor = System.Drawing.Color.FromArgb(180, 178, 170);
            this.PictureHuella.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PictureHuella.Image = global::TEntradaSalida.Properties.Resources.Vidrio;
            this.PictureHuella.Location = new System.Drawing.Point(20, 24);
            this.PictureHuella.Margin = new System.Windows.Forms.Padding(0);
            this.PictureHuella.Name = "PictureHuella";
            this.PictureHuella.Size = new System.Drawing.Size(130, 150);
            this.PictureHuella.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureHuella.TabIndex = 1;
            this.PictureHuella.TabStop = false;
            this.PictureHuella.Click += new System.EventHandler(this.PictureHuella_Click);
            //
            this.lblLectorBrand.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Bold);
            this.lblLectorBrand.ForeColor = cGold;
            this.lblLectorBrand.Location = new System.Drawing.Point(0, 180);
            this.lblLectorBrand.Name = "lblLectorBrand";
            this.lblLectorBrand.Size = new System.Drawing.Size(170, 16);
            this.lblLectorBrand.Text = "D I G I T A L   O N E";
            this.lblLectorBrand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.panelLector.Controls.Add(this.lblLectorLed);
            this.panelLector.Controls.Add(this.PictureHuella);
            this.panelLector.Controls.Add(this.lblLectorBrand);

            // ==============================================================
            // PANEL PIN - Card oscura
            // ==============================================================
            this.panelPin.BackColor = cCardBg;
            this.panelPin.Location = new System.Drawing.Point(60, 238);
            this.panelPin.Name = "panelPin";
            this.panelPin.Size = new System.Drawing.Size(500, 210);
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
            this.lblPinTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPinTitulo.ForeColor = cGold;
            this.lblPinTitulo.Location = new System.Drawing.Point(0, 6);
            this.lblPinTitulo.Name = "lblPinTitulo";
            this.lblPinTitulo.Size = new System.Drawing.Size(500, 28);
            this.lblPinTitulo.Text = "\uD83D\uDD12  Fichada por PIN";
            this.lblPinTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.lblLegajoId.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLegajoId.ForeColor = cTextSecondary;
            this.lblLegajoId.Location = new System.Drawing.Point(80, 44);
            this.lblLegajoId.Name = "lblLegajoId";
            this.lblLegajoId.Size = new System.Drawing.Size(120, 28);
            this.lblLegajoId.Text = "Nro. Legajo:";
            this.lblLegajoId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.txtLegajoId.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtLegajoId.BackColor = System.Drawing.Color.FromArgb(35, 40, 65);
            this.txtLegajoId.ForeColor = cTextPrimary;
            this.txtLegajoId.Location = new System.Drawing.Point(205, 42);
            this.txtLegajoId.Name = "txtLegajoId";
            this.txtLegajoId.Size = new System.Drawing.Size(190, 31);
            this.txtLegajoId.TabIndex = 0;
            this.txtLegajoId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLegajoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //
            this.lblPin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPin.ForeColor = cTextSecondary;
            this.lblPin.Location = new System.Drawing.Point(80, 84);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(120, 28);
            this.lblPin.Text = "PIN:";
            this.lblPin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            this.txtPin.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtPin.BackColor = System.Drawing.Color.FromArgb(35, 40, 65);
            this.txtPin.ForeColor = cTextPrimary;
            this.txtPin.Location = new System.Drawing.Point(205, 82);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(190, 31);
            this.txtPin.MaxLength = 6;
            this.txtPin.PasswordChar = '*';
            this.txtPin.TabIndex = 1;
            this.txtPin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //
            this.btnFicharPin.BackColor = cGold;
            this.btnFicharPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFicharPin.FlatAppearance.BorderSize = 0;
            this.btnFicharPin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnFicharPin.ForeColor = cHeaderBg;
            this.btnFicharPin.Location = new System.Drawing.Point(150, 128);
            this.btnFicharPin.Name = "btnFicharPin";
            this.btnFicharPin.Size = new System.Drawing.Size(170, 38);
            this.btnFicharPin.TabIndex = 2;
            this.btnFicharPin.Text = "Fichar";
            this.btnFicharPin.UseVisualStyleBackColor = false;
            this.btnFicharPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFicharPin.Click += new System.EventHandler(this.btnFicharPin_Click);
            //
            this.lblPinError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPinError.ForeColor = System.Drawing.Color.FromArgb(220, 80, 80);
            this.lblPinError.Location = new System.Drawing.Point(0, 170);
            this.lblPinError.Name = "lblPinError";
            this.lblPinError.Size = new System.Drawing.Size(500, 20);
            this.lblPinError.Text = "";
            this.lblPinError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.lnkCambiarPin.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lnkCambiarPin.LinkColor = cGoldLight;
            this.lnkCambiarPin.ActiveLinkColor = cGold;
            this.lnkCambiarPin.Location = new System.Drawing.Point(305, 134);
            this.lnkCambiarPin.Name = "lnkCambiarPin";
            this.lnkCambiarPin.Size = new System.Drawing.Size(130, 22);
            this.lnkCambiarPin.TabIndex = 3;
            this.lnkCambiarPin.TabStop = true;
            this.lnkCambiarPin.Text = "Cambiar mi PIN";
            this.lnkCambiarPin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkCambiarPin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCambiarPin_LinkClicked);

            // ==============================================================
            // PANEL DEMO - Card oscura
            // ==============================================================
            this.panelDemo.BackColor = cCardBg;
            this.panelDemo.Location = new System.Drawing.Point(60, 238);
            this.panelDemo.Name = "panelDemo";
            this.panelDemo.Size = new System.Drawing.Size(500, 210);
            this.panelDemo.Padding = new System.Windows.Forms.Padding(10);
            this.panelDemo.TabIndex = 21;
            this.panelDemo.Visible = false;
            this.panelDemo.Controls.Add(this.lblDemoTitulo);
            this.panelDemo.Controls.Add(this.lblDemoBanner);
            this.panelDemo.Controls.Add(this.cmbLegajosDemo);
            this.panelDemo.Controls.Add(this.btnFicharDemo);
            //
            this.lblDemoTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDemoTitulo.ForeColor = cGoldLight;
            this.lblDemoTitulo.Location = new System.Drawing.Point(0, 6);
            this.lblDemoTitulo.Name = "lblDemoTitulo";
            this.lblDemoTitulo.Size = new System.Drawing.Size(500, 28);
            this.lblDemoTitulo.Text = "\u26A0  MODO DEMOSTRACION";
            this.lblDemoTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.lblDemoBanner.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDemoBanner.ForeColor = cTextSecondary;
            this.lblDemoBanner.Location = new System.Drawing.Point(0, 38);
            this.lblDemoBanner.Name = "lblDemoBanner";
            this.lblDemoBanner.Size = new System.Drawing.Size(500, 20);
            this.lblDemoBanner.Text = "Seleccione un legajo para simular la fichada";
            this.lblDemoBanner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.cmbLegajosDemo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLegajosDemo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbLegajosDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLegajosDemo.Location = new System.Drawing.Point(100, 68);
            this.cmbLegajosDemo.Name = "cmbLegajosDemo";
            this.cmbLegajosDemo.Size = new System.Drawing.Size(300, 29);
            this.cmbLegajosDemo.TabIndex = 0;
            //
            this.btnFicharDemo.BackColor = cGoldLight;
            this.btnFicharDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFicharDemo.FlatAppearance.BorderSize = 0;
            this.btnFicharDemo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnFicharDemo.ForeColor = cHeaderBg;
            this.btnFicharDemo.Location = new System.Drawing.Point(160, 115);
            this.btnFicharDemo.Name = "btnFicharDemo";
            this.btnFicharDemo.Size = new System.Drawing.Size(180, 38);
            this.btnFicharDemo.TabIndex = 1;
            this.btnFicharDemo.Text = "Simular Fichada";
            this.btnFicharDemo.UseVisualStyleBackColor = false;
            this.btnFicharDemo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFicharDemo.Click += new System.EventHandler(this.btnFicharDemo_Click);

            // ==============================================================
            // PANEL QR - Card oscura con camara
            // ==============================================================
            this.panelQR.BackColor = cCardBg;
            this.panelQR.Location = new System.Drawing.Point(60, 238);
            this.panelQR.Name = "panelQR";
            this.panelQR.Size = new System.Drawing.Size(500, 240);
            this.panelQR.Padding = new System.Windows.Forms.Padding(10);
            this.panelQR.TabIndex = 23;
            this.panelQR.Visible = false;
            this.panelQR.Controls.Add(this.lblQrTitulo);
            this.panelQR.Controls.Add(this.picCamara);
            this.panelQR.Controls.Add(this.lblQrInstruccion);
            //
            this.lblQrTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblQrTitulo.ForeColor = cGold;
            this.lblQrTitulo.Location = new System.Drawing.Point(0, 2);
            this.lblQrTitulo.Name = "lblQrTitulo";
            this.lblQrTitulo.Size = new System.Drawing.Size(500, 26);
            this.lblQrTitulo.Text = "\uD83D\uDCF7  Fichada por QR";
            this.lblQrTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            this.picCamara.BackColor = System.Drawing.Color.FromArgb(10, 12, 22);
            this.picCamara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCamara.Location = new System.Drawing.Point(130, 30);
            this.picCamara.Name = "picCamara";
            this.picCamara.Size = new System.Drawing.Size(240, 185);
            this.picCamara.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCamara.TabIndex = 0;
            this.picCamara.TabStop = false;
            //
            this.lblQrInstruccion.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblQrInstruccion.ForeColor = cTextSecondary;
            this.lblQrInstruccion.Location = new System.Drawing.Point(0, 220);
            this.lblQrInstruccion.Name = "lblQrInstruccion";
            this.lblQrInstruccion.Size = new System.Drawing.Size(500, 18);
            this.lblQrInstruccion.Text = "Presente su codigo QR frente a la camara";
            this.lblQrInstruccion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ==============================================================
            // ZONA RESULTADO - ENTRADA/SALIDA + Avatar + Nombre
            // ==============================================================
            //
            // etiquetaES
            //
            this.etiquetaES.AutoEllipsis = true;
            this.etiquetaES.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.etiquetaES.ForeColor = cGold;
            this.etiquetaES.BackColor = cDarkBg;
            this.etiquetaES.Location = new System.Drawing.Point(5, 485);
            this.etiquetaES.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaES.Name = "etiquetaES";
            this.etiquetaES.Size = new System.Drawing.Size(610, 48);
            this.etiquetaES.TabIndex = 4;
            this.etiquetaES.Text = "";
            this.etiquetaES.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // picAvatar
            //
            this.picAvatar.BackColor = System.Drawing.Color.FromArgb(22, 28, 48);
            this.picAvatar.Location = new System.Drawing.Point(80, 538);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(58, 58);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAvatar.TabIndex = 33;
            this.picAvatar.TabStop = false;
            this.picAvatar.Visible = false;
            //
            // lblIniciales
            //
            this.lblIniciales.BackColor = cGold;
            this.lblIniciales.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblIniciales.ForeColor = cHeaderBg;
            this.lblIniciales.Location = new System.Drawing.Point(80, 538);
            this.lblIniciales.Name = "lblIniciales";
            this.lblIniciales.Size = new System.Drawing.Size(58, 58);
            this.lblIniciales.Text = "";
            this.lblIniciales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblIniciales.Visible = false;
            //
            // etiquetaNombre
            //
            this.etiquetaNombre.AutoEllipsis = true;
            this.etiquetaNombre.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular);
            this.etiquetaNombre.ForeColor = cTextPrimary;
            this.etiquetaNombre.BackColor = cDarkBg;
            this.etiquetaNombre.Location = new System.Drawing.Point(5, 538);
            this.etiquetaNombre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaNombre.Name = "etiquetaNombre";
            this.etiquetaNombre.Size = new System.Drawing.Size(610, 36);
            this.etiquetaNombre.TabIndex = 6;
            this.etiquetaNombre.Text = "";
            this.etiquetaNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ==============================================================
            // lnkCambiarModo - OCULTO (reemplazado por panelModos)
            // ==============================================================
            this.lnkCambiarModo.Location = new System.Drawing.Point(0, 0);
            this.lnkCambiarModo.Name = "lnkCambiarModo";
            this.lnkCambiarModo.Size = new System.Drawing.Size(1, 1);
            this.lnkCambiarModo.Visible = false;
            this.lnkCambiarModo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCambiarModo_LinkClicked);

            // ==============================================================
            // FOOTER - Barra inferior
            // ==============================================================
            this.etiquetaSucursal.AutoEllipsis = true;
            this.etiquetaSucursal.BackColor = cHeaderBg;
            this.etiquetaSucursal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.etiquetaSucursal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.etiquetaSucursal.ForeColor = cGoldLight;
            this.etiquetaSucursal.Location = new System.Drawing.Point(0, 570);
            this.etiquetaSucursal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.etiquetaSucursal.Name = "etiquetaSucursal";
            this.etiquetaSucursal.Size = new System.Drawing.Size(620, 40);
            this.etiquetaSucursal.TabIndex = 7;
            this.etiquetaSucursal.Text = "";
            this.etiquetaSucursal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ==============================================================
            // FORM
            // ==============================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = cDarkBg;
            this.ClientSize = new System.Drawing.Size(620, 660);
            this.Controls.Add(this.picAvatar);
            this.Controls.Add(this.lblIniciales);
            this.Controls.Add(this.panelSemaforo);
            this.Controls.Add(this.panelSeparador);
            this.Controls.Add(this.panelModos);
            this.Controls.Add(this.lnkCambiarModo);
            this.Controls.Add(this.panelQR);
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
            ((System.ComponentModel.ISupportInitialize)(this.picCamara)).EndInit();
            this.panelPin.ResumeLayout(false);
            this.panelPin.PerformLayout();
            this.panelDemo.ResumeLayout(false);
            this.panelQR.ResumeLayout(false);
            this.panelLector.ResumeLayout(false);
            this.panelSemaforo.ResumeLayout(false);
            this.panelModos.ResumeLayout(false);
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
        // QR controls
        private System.Windows.Forms.Panel panelQR;
        private System.Windows.Forms.Label lblQrTitulo;
        private System.Windows.Forms.PictureBox picCamara;
        private System.Windows.Forms.Label lblQrInstruccion;
        // Mode selector
        private System.Windows.Forms.Panel panelModos;
        private System.Windows.Forms.Label btnModoHuella;
        private System.Windows.Forms.Label btnModoPin;
        private System.Windows.Forms.Label btnModoQR;
        private System.Windows.Forms.Label btnModoDemo;
        // Links (lnkCambiarModo oculto, reemplazado por panelModos)
        private System.Windows.Forms.LinkLabel lnkCambiarPin;
        private System.Windows.Forms.LinkLabel lnkCambiarModo;
        // Avatar
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Label lblIniciales;
    }
}
