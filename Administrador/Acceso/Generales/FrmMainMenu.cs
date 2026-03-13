using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FontAwesome.Sharp;
using Acceso.Clases.Datos.Metadatos;
using Acceso.Clases.Datos.Generales;
using System.Data;
using System.Linq;
using Acceso.Generales;
using Acceso.RRHH;
using System.Data.SqlClient;
using DigitalPlus.Licensing;

namespace Acceso.Ventas
{
    public partial class FrmMainMenu : Form
    {
        //Fields
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;
        private Panel panelWelcome;

        public FrmMainMenu()
        {
            InitializeComponent();
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);

            //remuevo la cabecera del form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            //saco los botones que no van
            ActivarBotonesMenu();


        }


        // Colores integraia.tech
        private static readonly Color ColorGold = Color.FromArgb(201, 168, 76);
        private static readonly Color ColorGoldLight = Color.FromArgb(232, 201, 122);
        private static readonly Color ColorDark = Color.FromArgb(11, 17, 32);
        private static readonly Color ColorDarkest = Color.FromArgb(5, 8, 16);
        private static readonly Color ColorBg = Color.FromArgb(248, 247, 244);

        private struct RGBColors
        {
            public static Color colorDash = Color.FromArgb(201, 168, 76);
            public static Color colorClie = Color.FromArgb(201, 168, 76);
            public static Color colorMovi = Color.FromArgb(201, 168, 76);
            public static Color colorCons = Color.FromArgb(201, 168, 76);
            public static Color colorConf = Color.FromArgb(201, 168, 76);
            public static Color colorLogO = Color.FromArgb(201, 168, 76);
            public static Color colorLega = Color.FromArgb(232, 201, 122);
        }

        private void ActivarBotonesMenu() { }

        private void OcultarSubMenus() { }
        private void ActivateButton(object senderBtn, Color color)
        {
            DisableButton();
            if (senderBtn != null)
            {
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(18, 25, 45);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;

                //left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();

                //Icon Current Child Form
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = color;
                 
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = ColorDark;
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void  OpenChildForm(Form childForm)
        {
            if(currentChildForm != null)
            {
                currentChildForm.Close();
            }
            if (panelWelcome != null)
                panelWelcome.Visible = false;
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.FormClosed += (s, ev) =>
            {
                if (currentChildForm == childForm)
                {
                    currentChildForm = null;
                    Reset();
                    MostrarPanelBienvenida();
                }
            };
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = childForm.Text;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
         
            if (currentChildForm != null)
                    currentChildForm.Close();

            Reset();
            MostrarPanelBienvenida();
        }

        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            iconCurrentChildForm.IconChar = IconChar.Home;
            iconCurrentChildForm.IconColor = ColorGold;
            lblTitleChildForm.Text = "Inicio";
        }

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void iconMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panelDesktop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmMainMenu_Load(object sender, EventArgs e)
        {
            //Ventas.FrmLogin oLogin = new Ventas.FrmLogin();
            //oLogin.ShowDialog();
            //if (!oLogin.entro)
            //{
            //    Application.Exit();
            //    return;

            //}
            //_nNivel = oLogin.oUsuario.nNivel;

            ////Guardo el usuario en una variableGlobal para usarla en todo el sistema
            ////TODO: 18/05/2022 - Tiene que ser reemnplazado por ObjGlobal.oUsuario
            //Acceso.Clases.Datos.Generales.GRALVariablesGlobales.sUsuarioID = oLogin.oUsuario.sUsuarioID;


            //Ahora la fecha de inicio
           Acceso.Clases.Datos.Generales.GRALVariablesGlobales.FechaInicio = new DateTime(DateTime.Now.Year, 1, 1);


            //Gustavo 18/05/2022 -- muestro el nombre del usuario con la nueva variable global
            //lblUsuario.Text = ObjGlobal.oUsuario.sApellido + ", " + ObjGlobal.oUsuario.sNombre ;
            string empresa = ConfigurationManager.AppSettings["NombreEmpresa"];
            lblUsuario.Text = !string.IsNullOrEmpty(empresa) ? empresa : "Bienvenido...";

            // Mostrar info de licencia en barra inferior con nombre real de empresa
            if (Program.LicMgr != null && Program.LicMgr.CurrentTicket != null)
            {
                var t = Program.LicMgr.CurrentTicket;
                var plan = string.IsNullOrEmpty(t.Plan) ? "" : t.Plan.Substring(0, 1).ToUpper() + t.Plan.Substring(1);
                DateTime? vence = t.LicenseType == "trial" ? t.TrialEndsAt : t.ExpiresAt;
                if (vence.HasValue)
                {
                    var dias = (int)(vence.Value - DateTime.UtcNow).TotalDays;
                    if (dias < 0) dias = 0;
                    lblLicenciaStatus.Text = string.Format("{0} | {1} | Vence: {2} ({3}d)",
                        empresa, plan, vence.Value.ToLocalTime().ToString("dd/MM/yyyy"), dias);
                }
                else
                {
                    lblLicenciaStatus.Text = string.Format("{0} | {1}", empresa, plan);
                }
            }

            CargarLogos();

            // Verificar que la empresa esta activa en DigitalPlusAdmin
            var empresaInfo = EmpresaInfoService.ObtenerEmpresa();
            if (empresaInfo != null && !string.IsNullOrEmpty(empresaInfo.Estado) && empresaInfo.Estado != "activa")
            {
                MessageBox.Show(
                    "El acceso a su empresa ha sido suspendido.\nContacte al administrador del sistema.",
                    "Acceso Suspendido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s2, e2) => this.Close();
                return;
            }

            CargarLinksEmpresa();
            CrearPanelBienvenida();
        }

        private void CargarLogos()
        {
            try
            {
                // Logo de empresa desde DigitalPlusAdmin
                var empresa = EmpresaInfoService.ObtenerEmpresa();
                if (empresa != null && empresa.Logo != null && empresa.Logo.Length > 0)
                {
                    var ms = new MemoryStream(empresa.Logo);
                    btnHome.Image = Image.FromStream(ms);

                    if (!string.IsNullOrEmpty(empresa.Nombre))
                        lblUsuario.Text = empresa.Nombre;
                }
            }
            catch { }

            try
            {
                // Logo IntegraIA (recurso embebido)
                byte[] integraBytes = EmpresaInfoService.ObtenerLogoIntegraIA();
                if (integraBytes != null && integraBytes.Length > 0)
                {
                    var ms = new MemoryStream(integraBytes);
                    picLogoIntegraIA.Image = Image.FromStream(ms);
                }
            }
            catch { }
        }

        private void CrearPanelBienvenida()
        {
            panelWelcome = new Panel();
            panelWelcome.Dock = DockStyle.Fill;
            panelWelcome.BackColor = ColorBg;

            // Panel central oscuro con contenido (escalado 1.3x)
            int pw = 650; // 500 * 1.3
            var panelCentro = new Panel();
            panelCentro.Size = new Size(pw, 680);
            panelCentro.BackColor = ColorDark;
            panelCentro.Anchor = AnchorStyles.None;

            // Titulo DIGITAL ONE
            var lblTitulo = new Label();
            lblTitulo.Text = "DIGITAL ONE";
            lblTitulo.Font = new Font("Segoe UI", 42F, FontStyle.Bold);
            lblTitulo.ForeColor = ColorGold;
            lblTitulo.Size = new Size(pw, 90);
            lblTitulo.Location = new Point(0, 30);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            // Subtitulo
            var lblSub = new Label();
            lblSub.Text = "Administrador";
            lblSub.Font = new Font("Segoe UI", 21F, FontStyle.Regular);
            lblSub.ForeColor = Color.FromArgb(200, 200, 210);
            lblSub.Size = new Size(pw, 40);
            lblSub.Location = new Point(0, 150);
            lblSub.TextAlign = ContentAlignment.MiddleCenter;

            // Linea dorada separadora
            var linea = new Panel();
            linea.BackColor = ColorGold;
            linea.Size = new Size(156, 3);
            linea.Location = new Point((pw - 156) / 2, 210);

            // Logo IntegraIA con fondo claro para visibilidad
            int logoW = 330, logoH = 178;
            var pnlLogoBack = new Panel();
            pnlLogoBack.Size = new Size(logoW, logoH);
            pnlLogoBack.Location = new Point((pw - logoW) / 2, 235);
            pnlLogoBack.BackColor = Color.Transparent;
            pnlLogoBack.Paint += (s2, ev2) =>
            {
                var rect = new Rectangle(0, 0, pnlLogoBack.Width, pnlLogoBack.Height);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int r = 18;
                    path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                    path.AddArc(rect.Right - r - 1, rect.Y, r, r, 270, 90);
                    path.AddArc(rect.Right - r - 1, rect.Bottom - r - 1, r, r, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - r - 1, r, r, 90, 90);
                    path.CloseFigure();
                    ev2.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var brush = new SolidBrush(Color.FromArgb(240, 240, 245)))
                        ev2.Graphics.FillPath(brush, path);
                }
            };

            var picLogo = new PictureBox();
            picLogo.Size = new Size(304, 152);
            picLogo.Location = new Point(13, 13);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.BackColor = Color.Transparent;
            try
            {
                byte[] logoBytes = EmpresaInfoService.ObtenerLogoIntegraIA();
                if (logoBytes != null && logoBytes.Length > 0)
                    picLogo.Image = Image.FromStream(new MemoryStream(logoBytes));
            }
            catch { }
            pnlLogoBack.Controls.Add(picLogo);

            // Info empresa y usuario
            string empresa = ConfigurationManager.AppSettings["NombreEmpresa"] ?? "";
            try
            {
                var empresaInfo = EmpresaInfoService.ObtenerEmpresa();
                if (empresaInfo != null && !string.IsNullOrEmpty(empresaInfo.Nombre))
                    empresa = empresaInfo.Nombre;
            }
            catch { }

            var lblEmpresa = new Label();
            lblEmpresa.Text = empresa;
            lblEmpresa.Font = new Font("Segoe UI", 18F, FontStyle.Regular);
            lblEmpresa.ForeColor = Color.FromArgb(232, 234, 240);
            lblEmpresa.Size = new Size(pw, 38);
            lblEmpresa.Location = new Point(0, 440);
            lblEmpresa.TextAlign = ContentAlignment.MiddleCenter;

            var lblUser = new Label();
            lblUser.Text = Program.sysUsuario;
            lblUser.Font = new Font("Segoe UI", 14F, FontStyle.Italic);
            lblUser.ForeColor = Color.FromArgb(160, 160, 180);
            lblUser.Size = new Size(pw, 32);
            lblUser.Location = new Point(0, 485);
            lblUser.TextAlign = ContentAlignment.MiddleCenter;

            // Version
            var lblVer = new Label();
            lblVer.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            lblVer.Font = new Font("Segoe UI", 10F);
            lblVer.ForeColor = Color.FromArgb(100, 100, 120);
            lblVer.Size = new Size(pw, 24);
            lblVer.Location = new Point(0, 560);
            lblVer.TextAlign = ContentAlignment.MiddleCenter;

            // Powered by
            var lblPowered = new Label();
            lblPowered.Text = "powered by integraia.tech";
            lblPowered.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblPowered.ForeColor = Color.FromArgb(120, 120, 140);
            lblPowered.Size = new Size(pw, 24);
            lblPowered.Location = new Point(0, 588);
            lblPowered.TextAlign = ContentAlignment.MiddleCenter;

            panelCentro.Controls.AddRange(new Control[] {
                lblTitulo, lblSub, linea, pnlLogoBack, lblEmpresa, lblUser, lblVer, lblPowered
            });

            panelWelcome.Controls.Add(panelCentro);

            // Centrar el panel cuando cambie el tamaño
            panelWelcome.Resize += (s, ev) =>
            {
                panelCentro.Location = new Point(
                    (panelWelcome.Width - panelCentro.Width) / 2,
                    (panelWelcome.Height - panelCentro.Height) / 2);
            };

            panelDesktop.Controls.Add(panelWelcome);
            panelWelcome.BringToFront();

            // Posicionar inicialmente
            panelCentro.Location = new Point(
                (panelDesktop.Width - panelCentro.Width) / 2,
                (panelDesktop.Height - panelCentro.Height) / 2);
        }

        private void MostrarPanelBienvenida()
        {
            if (panelWelcome != null)
            {
                panelWelcome.Visible = true;
                panelWelcome.BringToFront();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro que quiere salir?", "Atención!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
        }

        private void btnLegajos_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            ActivateButton(sender, RGBColors.colorLega);
            OpenChildForm(new RRHH.FrmRRHHLegajos());
        }

        private void CargarLinksEmpresa()
        {
            panelSocialLinks.Controls.Clear();

            var empresa = EmpresaInfoService.ObtenerEmpresa();
            if (empresa == null) return;

            // Portal DigitalPlus Web (siempre visible si hay URL configurada)
            string urlPortalLink = ConfigurationManager.AppSettings["UrlPortal"];
            if (!string.IsNullOrEmpty(urlPortalLink))
                AgregarBotonLink("Portal Web", IconChar.Globe, urlPortalLink);

            // Links dinamicos desde la ficha de empresa en DigitalPlusAdmin
            if (!string.IsNullOrEmpty(empresa.PaginaWeb))
                AgregarBotonLink("Web", IconChar.Globe, empresa.PaginaWeb);

            if (!string.IsNullOrEmpty(empresa.Facebook))
                AgregarBotonLink("Facebook", IconChar.FacebookF, empresa.Facebook);

            if (!string.IsNullOrEmpty(empresa.Instagram))
                AgregarBotonLink("Instagram", IconChar.Instagram, empresa.Instagram);

            if (!string.IsNullOrEmpty(empresa.LinkedIn))
                AgregarBotonLink("LinkedIn", IconChar.LinkedinIn, empresa.LinkedIn);

            if (!string.IsNullOrEmpty(empresa.Twitter))
                AgregarBotonLink("X (Twitter)", IconChar.Twitter, empresa.Twitter);

            if (!string.IsNullOrEmpty(empresa.YouTube))
                AgregarBotonLink("YouTube", IconChar.Youtube, empresa.YouTube);

            if (!string.IsNullOrEmpty(empresa.TikTok))
                AgregarBotonLink("TikTok", IconChar.Tiktok, empresa.TikTok);
        }

        private void AgregarBotonLink(string texto, IconChar icono, string url)
        {
            var btn = new IconButton
            {
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Gainsboro,
                IconChar = icono,
                IconColor = Color.Gainsboro,
                IconFont = IconFont.Auto,
                IconSize = 28,
                ImageAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 20, 0),
                Size = new Size(173, 50),
                Text = texto,
                TextAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                UseVisualStyleBackColor = true,
                Tag = url,
                Margin = Padding.Empty
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) =>
            {
                var link = ((Control)s).Tag?.ToString();
                if (!string.IsNullOrEmpty(link))
                    System.Diagnostics.Process.Start(link);
            };
            panelSocialLinks.Controls.Add(btn);
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.colorConf);
            OpenChildForm(new FrmConfiguracion());
        }

        private void btnLicencias_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.colorConf);
            OpenChildForm(new FrmLicencia());
        }

        private void btnCambiarClave_Click(object sender, EventArgs e)
        {
            // Prioridad: BD admin (UrlPortal) > app.config
            string urlPortal = null;
            try
            {
                var empresaInfo = EmpresaInfoService.ObtenerEmpresa();
                if (empresaInfo != null && !string.IsNullOrEmpty(empresaInfo.UrlPortal))
                    urlPortal = empresaInfo.UrlPortal;
            }
            catch { }

            if (string.IsNullOrEmpty(urlPortal))
                urlPortal = ConfigurationManager.AppSettings["UrlPortal"];

            if (string.IsNullOrEmpty(urlPortal))
            {
                MessageBox.Show("No hay URL del portal configurada.\nContacte al administrador del sistema.",
                    "Cambiar Clave", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                System.Diagnostics.Process.Start(urlPortal);
                MessageBox.Show(
                    "Se abrio el portal en su navegador.\n\nInicie sesion y vaya a su perfil para cambiar la contrasena.",
                    "Cambiar Clave", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el portal: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picLogoIntegraIA_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.integraia.tech");
            }
            catch { }
        }
    }
}
