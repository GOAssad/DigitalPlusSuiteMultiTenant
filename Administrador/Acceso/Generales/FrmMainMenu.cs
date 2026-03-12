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


        //Struct
        private struct RGBColors
        {
            public static Color colorDash =Color.FromArgb(172,126,241);
            public static Color colorClie = Color.ForestGreen;
            public static Color colorMovi =Color.FromArgb(253,138,114);
            public static Color colorCons =Color.FromArgb(95,77,221);
            public static Color colorConf =Color.FromArgb(249,88,155);
            public static Color colorLogO = Color.Red;
            public static Color colorLega = Color.FromArgb(238, 243, 106);
        }

        private void ActivarBotonesMenu() { }

        private void OcultarSubMenus() { }
        private void ActivateButton(object senderBtn, Color color)
        {
            DisableButton();
            if (senderBtn != null)
            {
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(37, 36, 81);
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
                currentBtn.BackColor = Color.FromArgb(31,30,68);
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
                //abre solo un formulario
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm; //asociar el formulario al panel
            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = childForm.Text;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
         
            if (currentChildForm != null)
                    currentChildForm.Close();
        
            Reset();
        }

        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            iconCurrentChildForm.IconChar = IconChar.Home;
            iconCurrentChildForm.IconColor = Color.MediumPurple;
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

            // Mostrar info de licencia en barra inferior
            if (Program.LicMgr != null && Program.LicMgr.CurrentTicket != null)
                lblLicenciaStatus.Text = Program.LicMgr.GetStatusBarText();

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
            string urlPortal = ConfigurationManager.AppSettings["UrlDigitalPlusWeb"];
            if (!string.IsNullOrEmpty(urlPortal))
                AgregarBotonLink("Portal Web", IconChar.Globe, urlPortal);

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

    }
}
