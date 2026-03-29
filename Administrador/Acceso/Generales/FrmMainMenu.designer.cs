
namespace Acceso.Ventas
{
    partial class FrmMainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainMenu));
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnLogout = new FontAwesome.Sharp.IconButton();
            this.btnLegajos = new FontAwesome.Sharp.IconButton();
            this.panelSocialLinks = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnHome = new System.Windows.Forms.PictureBox();
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.panelBotonesWindos = new System.Windows.Forms.Panel();
            this.iconMinimize = new FontAwesome.Sharp.IconPictureBox();
            this.iconClose = new FontAwesome.Sharp.IconPictureBox();
            this.iconMaximize = new FontAwesome.Sharp.IconPictureBox();
            this.lblTitleChildForm = new System.Windows.Forms.Label();
            this.iconCurrentChildForm = new FontAwesome.Sharp.IconPictureBox();
            this.panelShadow = new System.Windows.Forms.Panel();
            this.panelDesktop = new System.Windows.Forms.Panel();
            this.reloj1 = new Global.Controles.Reloj();
            this.panelMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnHome)).BeginInit();
            this.panelTitleBar.SuspendLayout();
            this.panelBotonesWindos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconMaximize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconCurrentChildForm)).BeginInit();
            this.panelDesktop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.AutoScroll = true;
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.btnConfiguracion = new FontAwesome.Sharp.IconButton();
            this.btnCambiarClave = new FontAwesome.Sharp.IconButton();
            this.panelBranding = new System.Windows.Forms.Panel();
            this.picLogoIntegraIA = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoIntegraIA)).BeginInit();
            this.panelBranding.SuspendLayout();
            this.panelMenu.Controls.Add(this.panelBranding);
            this.panelMenu.Controls.Add(this.btnLogout);
            this.panelMenu.Controls.Add(this.panelSocialLinks);
            this.panelMenu.Controls.Add(this.btnCambiarClave);
            this.panelMenu.Controls.Add(this.btnConfiguracion);
            this.panelMenu.Controls.Add(this.btnLegajos);
            this.panelMenu.Controls.Add(this.panel1);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(190, 800);
            this.panelMenu.TabIndex = 0;
            //
            // btnLogout
            // 
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnLogout.IconChar = FontAwesome.Sharp.IconChar.SignOutAlt;
            this.btnLogout.IconColor = System.Drawing.Color.Gainsboro;
            this.btnLogout.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnLogout.IconSize = 32;
            this.btnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Location = new System.Drawing.Point(0, 821);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.btnLogout.Size = new System.Drawing.Size(173, 60);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "Cerrar Sesion";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnLegajos
            // 
            this.btnLegajos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLegajos.FlatAppearance.BorderSize = 0;
            this.btnLegajos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLegajos.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnLegajos.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.btnLegajos.IconColor = System.Drawing.Color.Gainsboro;
            this.btnLegajos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnLegajos.IconSize = 32;
            this.btnLegajos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLegajos.Location = new System.Drawing.Point(0, 200);
            this.btnLegajos.Name = "btnLegajos";
            this.btnLegajos.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.btnLegajos.Size = new System.Drawing.Size(173, 60);
            this.btnLegajos.TabIndex = 3;
            this.btnLegajos.Text = "Legajos";
            this.btnLegajos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLegajos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLegajos.UseVisualStyleBackColor = true;
            this.btnLegajos.Click += new System.EventHandler(this.btnLegajos_Click);
            //
            // panelSocialLinks
            //
            this.panelSocialLinks.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSocialLinks.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelSocialLinks.WrapContents = false;
            this.panelSocialLinks.AutoSize = true;
            this.panelSocialLinks.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelSocialLinks.Name = "panelSocialLinks";
            this.panelSocialLinks.Size = new System.Drawing.Size(173, 0);
            this.panelSocialLinks.TabIndex = 7;
            this.panelSocialLinks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.panelSocialLinks.Padding = System.Windows.Forms.Padding.Empty;
            this.panelSocialLinks.Margin = System.Windows.Forms.Padding.Empty;
            //
            // btnConfiguracion
            //
            this.btnConfiguracion.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConfiguracion.FlatAppearance.BorderSize = 0;
            this.btnConfiguracion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracion.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnConfiguracion.IconChar = FontAwesome.Sharp.IconChar.Cog;
            this.btnConfiguracion.IconColor = System.Drawing.Color.Gainsboro;
            this.btnConfiguracion.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnConfiguracion.IconSize = 32;
            this.btnConfiguracion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracion.Name = "btnConfiguracion";
            this.btnConfiguracion.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.btnConfiguracion.Size = new System.Drawing.Size(173, 60);
            this.btnConfiguracion.TabIndex = 9;
            this.btnConfiguracion.Text = "Configuracion";
            this.btnConfiguracion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConfiguracion.UseVisualStyleBackColor = true;
            this.btnConfiguracion.Click += new System.EventHandler(this.btnConfiguracion_Click);
            //
            // btnCambiarClave
            //
            this.btnCambiarClave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCambiarClave.FlatAppearance.BorderSize = 0;
            this.btnCambiarClave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarClave.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnCambiarClave.IconChar = FontAwesome.Sharp.IconChar.Lock;
            this.btnCambiarClave.IconColor = System.Drawing.Color.Gainsboro;
            this.btnCambiarClave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCambiarClave.IconSize = 32;
            this.btnCambiarClave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCambiarClave.Name = "btnCambiarClave";
            this.btnCambiarClave.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.btnCambiarClave.Size = new System.Drawing.Size(173, 60);
            this.btnCambiarClave.TabIndex = 11;
            this.btnCambiarClave.Text = "Cambiar Clave";
            this.btnCambiarClave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCambiarClave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCambiarClave.UseVisualStyleBackColor = true;
            this.btnCambiarClave.Click += new System.EventHandler(this.btnCambiarClave_Click);
            //
            // panelBranding
            //
            this.panelBranding.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(8)))), ((int)(((byte)(16)))));
            this.panelBranding.Controls.Add(this.picLogoIntegraIA);
            this.panelBranding.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBranding.Location = new System.Drawing.Point(0, 740);
            this.panelBranding.Name = "panelBranding";
            this.panelBranding.Size = new System.Drawing.Size(173, 55);
            this.panelBranding.TabIndex = 12;
            //
            // picLogoIntegraIA
            //
            this.picLogoIntegraIA.Location = new System.Drawing.Point(22, 3);
            this.picLogoIntegraIA.Name = "picLogoIntegraIA";
            this.picLogoIntegraIA.Size = new System.Drawing.Size(130, 48);
            this.picLogoIntegraIA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogoIntegraIA.TabIndex = 0;
            this.picLogoIntegraIA.TabStop = false;
            this.picLogoIntegraIA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(235)))));
            this.picLogoIntegraIA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLogoIntegraIA.Click += new System.EventHandler(this.picLogoIntegraIA_Click);
            //
            // panel1
            // 
            this.panel1.Controls.Add(this.btnHome);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(173, 105);
            this.panel1.TabIndex = 0;
            //
            // btnHome
            //
            this.btnHome.Image = global::Acceso.Properties.Resources.Logo;
            this.btnHome.Location = new System.Drawing.Point(39, 15);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(95, 75);
            this.btnHome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnHome.TabIndex = 0;
            this.btnHome.TabStop = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // panelTitleBar
            // 
            this.panelTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.panelTitleBar.Controls.Add(this.lblUsuario);
            this.panelTitleBar.Controls.Add(this.panelBotonesWindos);
            this.panelTitleBar.Controls.Add(this.lblTitleChildForm);
            this.panelTitleBar.Controls.Add(this.iconCurrentChildForm);
            this.panelTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitleBar.Location = new System.Drawing.Point(159, 0);
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.Size = new System.Drawing.Size(991, 75);
            this.panelTitleBar.TabIndex = 1;
            this.panelTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseDown);
            // 
            // lblUsuario
            // 
            this.lblUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.lblUsuario.Location = new System.Drawing.Point(675, 38);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(12, 17);
            this.lblUsuario.TabIndex = 4;
            this.lblUsuario.Text = ".";
            // 
            // panelBotonesWindos
            // 
            this.panelBotonesWindos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBotonesWindos.Controls.Add(this.iconMinimize);
            this.panelBotonesWindos.Controls.Add(this.iconClose);
            this.panelBotonesWindos.Controls.Add(this.iconMaximize);
            this.panelBotonesWindos.Location = new System.Drawing.Point(897, 12);
            this.panelBotonesWindos.Name = "panelBotonesWindos";
            this.panelBotonesWindos.Size = new System.Drawing.Size(82, 30);
            this.panelBotonesWindos.TabIndex = 3;
            // 
            // iconMinimize
            // 
            this.iconMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.iconMinimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.iconMinimize.IconChar = FontAwesome.Sharp.IconChar.WindowMinimize;
            this.iconMinimize.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.iconMinimize.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconMinimize.IconSize = 20;
            this.iconMinimize.Location = new System.Drawing.Point(3, 11);
            this.iconMinimize.Name = "iconMinimize";
            this.iconMinimize.Size = new System.Drawing.Size(20, 20);
            this.iconMinimize.TabIndex = 2;
            this.iconMinimize.TabStop = false;
            this.iconMinimize.Click += new System.EventHandler(this.iconMinimize_Click);
            // 
            // iconClose
            // 
            this.iconClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.iconClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.iconClose.IconChar = FontAwesome.Sharp.IconChar.WindowClose;
            this.iconClose.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.iconClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconClose.IconSize = 20;
            this.iconClose.Location = new System.Drawing.Point(55, 11);
            this.iconClose.Name = "iconClose";
            this.iconClose.Size = new System.Drawing.Size(20, 20);
            this.iconClose.TabIndex = 2;
            this.iconClose.TabStop = false;
            this.iconClose.Click += new System.EventHandler(this.iconClose_Click);
            // 
            // iconMaximize
            // 
            this.iconMaximize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.iconMaximize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.iconMaximize.IconChar = FontAwesome.Sharp.IconChar.WindowMaximize;
            this.iconMaximize.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.iconMaximize.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconMaximize.IconSize = 20;
            this.iconMaximize.Location = new System.Drawing.Point(29, 11);
            this.iconMaximize.Name = "iconMaximize";
            this.iconMaximize.Size = new System.Drawing.Size(20, 20);
            this.iconMaximize.TabIndex = 2;
            this.iconMaximize.TabStop = false;
            this.iconMaximize.Click += new System.EventHandler(this.iconMaximize_Click);
            // 
            // lblTitleChildForm
            // 
            this.lblTitleChildForm.AutoSize = true;
            this.lblTitleChildForm.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblTitleChildForm.Location = new System.Drawing.Point(56, 41);
            this.lblTitleChildForm.Name = "lblTitleChildForm";
            this.lblTitleChildForm.Size = new System.Drawing.Size(32, 13);
            this.lblTitleChildForm.TabIndex = 1;
            this.lblTitleChildForm.Text = "Inicio";
            // 
            // iconCurrentChildForm
            // 
            this.iconCurrentChildForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.iconCurrentChildForm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.iconCurrentChildForm.IconChar = FontAwesome.Sharp.IconChar.Home;
            this.iconCurrentChildForm.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.iconCurrentChildForm.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconCurrentChildForm.Location = new System.Drawing.Point(17, 23);
            this.iconCurrentChildForm.Name = "iconCurrentChildForm";
            this.iconCurrentChildForm.Size = new System.Drawing.Size(32, 32);
            this.iconCurrentChildForm.TabIndex = 0;
            this.iconCurrentChildForm.TabStop = false;
            // 
            // panelShadow
            // 
            this.panelShadow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(8)))), ((int)(((byte)(16)))));
            this.panelShadow.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelShadow.Location = new System.Drawing.Point(159, 75);
            this.panelShadow.Name = "panelShadow";
            this.panelShadow.Size = new System.Drawing.Size(991, 8);
            this.panelShadow.TabIndex = 2;
            // 
            // panelDesktop
            // 
            this.panelDesktop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(244)))));
            this.panelDesktop.Controls.Add(this.reloj1);
            this.panelDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDesktop.Location = new System.Drawing.Point(159, 83);
            this.panelDesktop.Name = "panelDesktop";
            this.panelDesktop.Size = new System.Drawing.Size(991, 717);
            this.panelDesktop.TabIndex = 3;
            this.panelDesktop.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDesktop_Paint);
            // 
            // reloj1
            // 
            this.reloj1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reloj1.Location = new System.Drawing.Point(756, 591);
            this.reloj1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.reloj1.Name = "reloj1";
            this.reloj1.Size = new System.Drawing.Size(223, 108);
            this.reloj1.TabIndex = 0;
            // 
            // FrmMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 800);
            this.Controls.Add(this.panelDesktop);
            this.Controls.Add(this.panelShadow);
            this.Controls.Add(this.panelTitleBar);
            this.Controls.Add(this.panelMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1166, 698);
            this.Name = "FrmMainMenu";
            this.Text = " ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMainMenu_Load);
            this.panelMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnHome)).EndInit();
            this.panelTitleBar.ResumeLayout(false);
            this.panelTitleBar.PerformLayout();
            this.panelBotonesWindos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconMaximize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconCurrentChildForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoIntegraIA)).EndInit();
            this.panelBranding.ResumeLayout(false);
            this.panelDesktop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconButton btnLegajos;
        private System.Windows.Forms.PictureBox btnHome;
        private System.Windows.Forms.Panel panelTitleBar;
        private FontAwesome.Sharp.IconPictureBox iconCurrentChildForm;
        private System.Windows.Forms.Label lblTitleChildForm;
        private System.Windows.Forms.Panel panelShadow;
        private System.Windows.Forms.Panel panelDesktop;
        private FontAwesome.Sharp.IconPictureBox iconClose;
        private FontAwesome.Sharp.IconPictureBox iconMinimize;
        private FontAwesome.Sharp.IconPictureBox iconMaximize;
        private System.Windows.Forms.Panel panelBotonesWindos;
        private FontAwesome.Sharp.IconButton btnLogout;
        private System.Windows.Forms.FlowLayoutPanel panelSocialLinks;
        private FontAwesome.Sharp.IconButton btnConfiguracion;
        private FontAwesome.Sharp.IconButton btnCambiarClave;
        private Global.Controles.Reloj reloj1;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Panel panelBranding;
        private System.Windows.Forms.PictureBox picLogoIntegraIA;
    }
}

