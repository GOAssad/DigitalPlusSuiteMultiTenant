using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Acceso.Clases.Datos.Generales;
using Global.Datos;

namespace Acceso.Generales
{
    public class FrmLoginAdmin : Form
    {
        private Panel panelHeader;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblError;
        private Label lblVersion;
        private CheckBox chkRecordar;

        public string UsuarioNombre { get; private set; }
        public string UsuarioEmail { get; private set; }
        public string UsuarioRol { get; private set; }

        public FrmLoginAdmin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Digital One - Iniciar Sesion";
            this.ClientSize = new Size(420, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 247, 244);

            // Header oscuro
            panelHeader = new Panel();
            panelHeader.BackColor = Color.FromArgb(11, 17, 32);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Size = new Size(420, 110);

            lblTitulo = new Label();
            lblTitulo.Text = "DIGITAL ONE";
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(201, 168, 76);
            lblTitulo.Location = new Point(0, 18);
            lblTitulo.Size = new Size(420, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            lblSubtitulo = new Label();
            lblSubtitulo.Text = "Administrador";
            lblSubtitulo.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblSubtitulo.ForeColor = Color.FromArgb(200, 200, 210);
            lblSubtitulo.Location = new Point(0, 58);
            lblSubtitulo.Size = new Size(420, 25);
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;

            lblVersion = new Label();
            lblVersion.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.FromArgb(120, 120, 140);
            lblVersion.Location = new Point(0, 85);
            lblVersion.Size = new Size(420, 18);
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;

            panelHeader.Controls.AddRange(new Control[] { lblTitulo, lblSubtitulo, lblVersion });

            // Campos de login
            int labelX = 60;
            int fieldX = 60;
            int fieldW = 300;

            lblEmail = new Label();
            lblEmail.Text = "Email";
            lblEmail.Font = new Font("Segoe UI", 10F);
            lblEmail.ForeColor = Color.FromArgb(80, 80, 100);
            lblEmail.Location = new Point(labelX, 130);
            lblEmail.Size = new Size(fieldW, 22);

            txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 13F);
            txtEmail.Location = new Point(fieldX, 154);
            txtEmail.Size = new Size(fieldW, 32);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.TabIndex = 0;

            lblPassword = new Label();
            lblPassword.Text = "Contrase\u00F1a";
            lblPassword.Font = new Font("Segoe UI", 10F);
            lblPassword.ForeColor = Color.FromArgb(80, 80, 100);
            lblPassword.Location = new Point(labelX, 200);
            lblPassword.Size = new Size(fieldW, 22);

            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 13F);
            txtPassword.Location = new Point(fieldX, 224);
            txtPassword.Size = new Size(fieldW, 32);
            txtPassword.PasswordChar = '\u2022';
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.TabIndex = 1;

            chkRecordar = new CheckBox();
            chkRecordar.Text = "Recordar credenciales";
            chkRecordar.Font = new Font("Segoe UI", 9F);
            chkRecordar.ForeColor = Color.FromArgb(80, 80, 100);
            chkRecordar.Location = new Point(fieldX, 270);
            chkRecordar.Size = new Size(fieldW, 24);
            chkRecordar.TabIndex = 2;

            btnLogin = new Button();
            btnLogin.Text = "Iniciar Sesi\u00F3n";
            btnLogin.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(201, 168, 76);
            btnLogin.ForeColor = Color.FromArgb(11, 17, 32);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Location = new Point(fieldX, 305);
            btnLogin.Size = new Size(fieldW, 48);
            btnLogin.TabIndex = 3;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += BtnLogin_Click;

            lblError = new Label();
            lblError.Text = "";
            lblError.Font = new Font("Segoe UI", 9F);
            lblError.ForeColor = Color.FromArgb(220, 50, 50);
            lblError.Location = new Point(fieldX, 365);
            lblError.Size = new Size(fieldW, 40);
            lblError.TextAlign = ContentAlignment.TopCenter;

            // Texto powered by
            var lblPowered = new Label();
            lblPowered.Text = "powered by integraia.tech";
            lblPowered.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblPowered.ForeColor = Color.FromArgb(160, 160, 170);
            lblPowered.Location = new Point(0, 455);
            lblPowered.Size = new Size(420, 20);
            lblPowered.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.AddRange(new Control[]
            {
                panelHeader, lblEmail, txtEmail, lblPassword, txtPassword,
                chkRecordar, btnLogin, lblError, lblPowered
            });

            this.AcceptButton = btnLogin;

            // Cargar credenciales guardadas
            this.Shown += (s, e) =>
            {
                CargarCredenciales();
                if (!string.IsNullOrEmpty(txtEmail.Text))
                    txtPassword.Focus();
                else
                    txtEmail.Focus();
            };
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Ingrese email y contrase\u00F1a.";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Verificando...";

            try
            {
                int empresaId = TenantContext.EmpresaId;

                // Buscar usuario en AspNetUsers
                // Sanitizar email: solo permitir caracteres validos de email
                string safeEmail = email.ToUpperInvariant().Replace("'", "''");
                string sql = "SELECT u.Id, u.PasswordHash, u.NombreCompleto, u.IsActive, " +
                    "u.AccesoAdminDesktop, u.EmpresaId, u.LockoutEnd, u.AccessFailedCount, " +
                    "(SELECT TOP 1 r.Name FROM AspNetUserRoles ur " +
                    "INNER JOIN AspNetRoles r ON ur.RoleId = r.Id " +
                    "WHERE ur.UserId = u.Id) AS Rol " +
                    "FROM AspNetUsers u WHERE u.NormalizedEmail = '" + safeEmail + "'";

                var dt = SQLServer.EjecutarParaSoloLectura(sql);

                if (dt.Rows.Count == 0)
                {
                    lblError.Text = "Email o contrase\u00F1a incorrectos.";
                    return;
                }

                DataRow row = dt.Rows[0];

                // Verificar que esta activo
                bool isActive = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
                if (!isActive)
                {
                    lblError.Text = "La cuenta est\u00E1 desactivada.";
                    return;
                }

                // Verificar EmpresaId
                int userEmpresaId = Convert.ToInt32(row["EmpresaId"]);
                string rol = row["Rol"] != DBNull.Value ? row["Rol"].ToString() : "";

                // SuperAdmin puede acceder a cualquier empresa
                if (rol != "SuperAdmin" && userEmpresaId != empresaId)
                {
                    lblError.Text = "Este usuario no pertenece a esta empresa.";
                    return;
                }

                // Verificar AccesoAdminDesktop
                bool accesoAdmin = row["AccesoAdminDesktop"] != DBNull.Value
                    && Convert.ToBoolean(row["AccesoAdminDesktop"]);

                if (!accesoAdmin)
                {
                    lblError.Text = "No tiene permiso para acceder al Administrador.";
                    return;
                }

                // Verificar lockout
                if (row["LockoutEnd"] != DBNull.Value)
                {
                    DateTimeOffset lockoutEnd = (DateTimeOffset)row["LockoutEnd"];
                    if (lockoutEnd > DateTimeOffset.UtcNow)
                    {
                        lblError.Text = "Cuenta bloqueada temporalmente. Intente m\u00E1s tarde.";
                        return;
                    }
                }

                // Verificar password
                string storedHash = row["PasswordHash"]?.ToString();
                if (!IdentityPasswordVerifier.VerifyPassword(storedHash, password))
                {
                    lblError.Text = "Email o contrase\u00F1a incorrectos.";
                    return;
                }

                // Login exitoso
                UsuarioNombre = row["NombreCompleto"] != DBNull.Value
                    ? row["NombreCompleto"].ToString() : email;
                UsuarioEmail = email;
                UsuarioRol = rol;

                // Guardar o borrar credenciales segun checkbox
                if (chkRecordar.Checked)
                    GuardarCredenciales(email, password);
                else
                    BorrarCredenciales();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error de conexi\u00F3n: " + ex.Message;
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Iniciar Sesi\u00F3n";
            }
        }
        #region Credenciales guardadas (DPAPI)

        private static string ObtenerRutaCredenciales()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DigitalOne", "Administrador");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return Path.Combine(dir, ".cred");
        }

        private void GuardarCredenciales(string email, string password)
        {
            try
            {
                string data = email + "|" + password;
                byte[] plain = Encoding.UTF8.GetBytes(data);
                byte[] encrypted = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(ObtenerRutaCredenciales(), encrypted);
            }
            catch { }
        }

        private void CargarCredenciales()
        {
            try
            {
                string path = ObtenerRutaCredenciales();
                if (!File.Exists(path)) return;

                byte[] encrypted = File.ReadAllBytes(path);
                byte[] plain = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                string data = Encoding.UTF8.GetString(plain);

                int sep = data.IndexOf('|');
                if (sep > 0)
                {
                    txtEmail.Text = data.Substring(0, sep);
                    txtPassword.Text = data.Substring(sep + 1);
                    chkRecordar.Checked = true;
                }
            }
            catch
            {
                // Si el archivo esta corrupto, lo borramos
                BorrarCredenciales();
            }
        }

        private static void BorrarCredenciales()
        {
            try
            {
                string path = ObtenerRutaCredenciales();
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch { }
        }

        #endregion
    }
}
