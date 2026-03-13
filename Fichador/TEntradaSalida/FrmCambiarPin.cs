using Acceso.Clases.Datos.RRHH;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Acceso
{
    public class FrmCambiarPin : Form
    {
        private Label lblTitulo;
        private Label lblMensaje;
        private TextBox txtNuevoPin;
        private TextBox txtConfirmarPin;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblError;

        private readonly string _legajoId;
        private readonly string _legajoNombre;
        private readonly bool _esPrimerPin;

        public FrmCambiarPin(string legajoId, string legajoNombre, bool esPrimerPin)
        {
            _legajoId = legajoId;
            _legajoNombre = legajoNombre;
            _esPrimerPin = esPrimerPin;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Cambiar PIN";
            this.Size = new Size(350, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 247, 244);

            lblTitulo = new Label
            {
                Text = _legajoNombre,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(201, 168, 76),
                Location = new Point(20, 15),
                Size = new Size(300, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            string msg = _esPrimerPin
                ? "Debe crear un PIN para fichar.\nIngrese un PIN de 4 a 6 digitos."
                : "Su PIN ha expirado.\nIngrese un nuevo PIN de 4 a 6 digitos.";

            lblMensaje = new Label
            {
                Text = msg,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.DimGray,
                Location = new Point(20, 50),
                Size = new Size(300, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblNuevo = new Label
            {
                Text = "Nuevo PIN:",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(40, 100),
                Size = new Size(100, 25)
            };

            txtNuevoPin = new TextBox
            {
                Font = new Font("Segoe UI", 14F),
                Location = new Point(150, 95),
                Size = new Size(140, 35),
                MaxLength = 6,
                PasswordChar = '*',
                TextAlign = HorizontalAlignment.Center
            };
            txtNuevoPin.KeyPress += OnlyDigits_KeyPress;

            var lblConfirmar = new Label
            {
                Text = "Confirmar:",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(40, 140),
                Size = new Size(100, 25)
            };

            txtConfirmarPin = new TextBox
            {
                Font = new Font("Segoe UI", 14F),
                Location = new Point(150, 135),
                Size = new Size(140, 35),
                MaxLength = 6,
                PasswordChar = '*',
                TextAlign = HorizontalAlignment.Center
            };
            txtConfirmarPin.KeyPress += OnlyDigits_KeyPress;

            lblError = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Red,
                Location = new Point(20, 175),
                Size = new Size(300, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnAceptar = new Button
            {
                Text = "Guardar PIN",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(201, 168, 76),
                ForeColor = Color.FromArgb(11, 17, 32),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(40, 210),
                Size = new Size(120, 35)
            };
            btnAceptar.Click += BtnAceptar_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Font = new Font("Segoe UI", 10F),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(180, 210),
                Size = new Size(120, 35)
            };
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] {
                lblTitulo, lblMensaje, lblNuevo, txtNuevoPin,
                lblConfirmar, txtConfirmarPin, lblError,
                btnAceptar, btnCancelar
            });

            this.AcceptButton = btnAceptar;
            this.CancelButton = btnCancelar;
        }

        private void OnlyDigits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            string pin = txtNuevoPin.Text.Trim();
            string confirm = txtConfirmarPin.Text.Trim();

            if (pin.Length < 4)
            {
                lblError.Text = "El PIN debe tener al menos 4 digitos.";
                txtNuevoPin.Focus();
                return;
            }

            if (pin != confirm)
            {
                lblError.Text = "Los PINs no coinciden.";
                txtConfirmarPin.Focus();
                return;
            }

            var pinHelper = new RRHHLegajosPin();
            if (pinHelper.CambiarPin(_legajoId, pin))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblError.Text = "Error al guardar el PIN. Intente nuevamente.";
            }
        }
    }
}
