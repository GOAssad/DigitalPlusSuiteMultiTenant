using Acceso.Clases.Datos.RRHH;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Acceso
{
    public class FrmCambiarPinVoluntario : Form
    {
        private Label lblTitulo;
        private Label lblLegajo;
        private TextBox txtLegajo;
        private Label lblPinActual;
        private TextBox txtPinActual;
        private Label lblNuevoPin;
        private TextBox txtNuevoPin;
        private Label lblConfirmar;
        private TextBox txtConfirmarPin;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblError;
        private Label lblExito;

        public FrmCambiarPinVoluntario()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Cambiar PIN";
            this.ClientSize = new Size(420, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 247, 244);

            lblTitulo = new Label();
            lblTitulo.Text = "Cambio de PIN";
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(201, 168, 76);
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Size = new Size(420, 35);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            int labelX = 30;
            int fieldX = 180;
            int fieldW = 190;

            lblLegajo = new Label();
            lblLegajo.Text = "Nro. Legajo:";
            lblLegajo.Font = new Font("Segoe UI", 11F);
            lblLegajo.Location = new Point(labelX, 68);
            lblLegajo.Size = new Size(140, 30);
            lblLegajo.TextAlign = ContentAlignment.MiddleLeft;

            txtLegajo = new TextBox();
            txtLegajo.Font = new Font("Segoe UI", 14F);
            txtLegajo.Location = new Point(fieldX, 65);
            txtLegajo.Size = new Size(fieldW, 35);
            txtLegajo.TextAlign = HorizontalAlignment.Center;

            lblPinActual = new Label();
            lblPinActual.Text = "PIN actual:";
            lblPinActual.Font = new Font("Segoe UI", 11F);
            lblPinActual.Location = new Point(labelX, 113);
            lblPinActual.Size = new Size(140, 30);
            lblPinActual.TextAlign = ContentAlignment.MiddleLeft;

            txtPinActual = new TextBox();
            txtPinActual.Font = new Font("Segoe UI", 14F);
            txtPinActual.Location = new Point(fieldX, 110);
            txtPinActual.Size = new Size(fieldW, 35);
            txtPinActual.MaxLength = 6;
            txtPinActual.PasswordChar = '*';
            txtPinActual.TextAlign = HorizontalAlignment.Center;
            txtPinActual.KeyPress += OnlyDigits_KeyPress;

            lblNuevoPin = new Label();
            lblNuevoPin.Text = "Nuevo PIN:";
            lblNuevoPin.Font = new Font("Segoe UI", 11F);
            lblNuevoPin.Location = new Point(labelX, 158);
            lblNuevoPin.Size = new Size(140, 30);
            lblNuevoPin.TextAlign = ContentAlignment.MiddleLeft;

            txtNuevoPin = new TextBox();
            txtNuevoPin.Font = new Font("Segoe UI", 14F);
            txtNuevoPin.Location = new Point(fieldX, 155);
            txtNuevoPin.Size = new Size(fieldW, 35);
            txtNuevoPin.MaxLength = 6;
            txtNuevoPin.PasswordChar = '*';
            txtNuevoPin.TextAlign = HorizontalAlignment.Center;
            txtNuevoPin.KeyPress += OnlyDigits_KeyPress;

            lblConfirmar = new Label();
            lblConfirmar.Text = "Confirmar PIN:";
            lblConfirmar.Font = new Font("Segoe UI", 11F);
            lblConfirmar.Location = new Point(labelX, 203);
            lblConfirmar.Size = new Size(140, 30);
            lblConfirmar.TextAlign = ContentAlignment.MiddleLeft;

            txtConfirmarPin = new TextBox();
            txtConfirmarPin.Font = new Font("Segoe UI", 14F);
            txtConfirmarPin.Location = new Point(fieldX, 200);
            txtConfirmarPin.Size = new Size(fieldW, 35);
            txtConfirmarPin.MaxLength = 6;
            txtConfirmarPin.PasswordChar = '*';
            txtConfirmarPin.TextAlign = HorizontalAlignment.Center;
            txtConfirmarPin.KeyPress += OnlyDigits_KeyPress;

            lblError = new Label();
            lblError.Text = "";
            lblError.Font = new Font("Segoe UI", 9F);
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(10, 250);
            lblError.Size = new Size(400, 25);
            lblError.TextAlign = ContentAlignment.MiddleCenter;

            lblExito = new Label();
            lblExito.Text = "";
            lblExito.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblExito.ForeColor = Color.Green;
            lblExito.Location = new Point(10, 250);
            lblExito.Size = new Size(400, 25);
            lblExito.TextAlign = ContentAlignment.MiddleCenter;
            lblExito.Visible = false;

            btnAceptar = new Button();
            btnAceptar.Text = "Cambiar PIN";
            btnAceptar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnAceptar.BackColor = Color.FromArgb(201, 168, 76);
            btnAceptar.ForeColor = Color.FromArgb(11, 17, 32);
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Location = new Point(40, 290);
            btnAceptar.Size = new Size(160, 42);
            btnAceptar.Click += BtnAceptar_Click;

            btnCancelar = new Button();
            btnCancelar.Text = "Cerrar";
            btnCancelar.Font = new Font("Segoe UI", 11F);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Location = new Point(220, 290);
            btnCancelar.Size = new Size(160, 42);
            btnCancelar.Click += delegate { this.Close(); };

            this.Controls.AddRange(new Control[] {
                lblTitulo, lblLegajo, txtLegajo,
                lblPinActual, txtPinActual,
                lblNuevoPin, txtNuevoPin,
                lblConfirmar, txtConfirmarPin,
                lblError, lblExito,
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
            lblError.Text = "";
            lblExito.Visible = false;
            lblError.Visible = true;

            string legajo = txtLegajo.Text.Trim();
            string pinActual = txtPinActual.Text.Trim();
            string pinNuevo = txtNuevoPin.Text.Trim();
            string pinConfirmar = txtConfirmarPin.Text.Trim();

            if (string.IsNullOrEmpty(legajo))
            {
                lblError.Text = "Ingrese el numero de legajo.";
                txtLegajo.Focus();
                return;
            }

            if (string.IsNullOrEmpty(pinActual))
            {
                lblError.Text = "Ingrese su PIN actual.";
                txtPinActual.Focus();
                return;
            }

            if (pinNuevo.Length < 4)
            {
                lblError.Text = "El nuevo PIN debe tener al menos 4 digitos.";
                txtNuevoPin.Focus();
                return;
            }

            if (pinNuevo != pinConfirmar)
            {
                lblError.Text = "Los PINs nuevos no coinciden.";
                txtConfirmarPin.Text = "";
                txtConfirmarPin.Focus();
                return;
            }

            if (pinNuevo == pinActual)
            {
                lblError.Text = "El nuevo PIN debe ser diferente al actual.";
                txtNuevoPin.Text = "";
                txtConfirmarPin.Text = "";
                txtNuevoPin.Focus();
                return;
            }

            // Verificar legajo y PIN actual
            var pinHelper = new RRHHLegajosPin();

            if (!pinHelper.CargarLegajo(legajo))
            {
                lblError.Text = "Legajo no encontrado.";
                txtLegajo.Focus();
                return;
            }

            if (!pinHelper.HasPin)
            {
                lblError.Text = "Este legajo no tiene PIN asignado.";
                return;
            }

            if (!pinHelper.VerificarPin(legajo, pinActual))
            {
                lblError.Text = "El PIN actual es incorrecto.";
                txtPinActual.Text = "";
                txtPinActual.Focus();
                return;
            }

            // Cambiar PIN
            if (pinHelper.CambiarPin(legajo, pinNuevo))
            {
                lblError.Visible = false;
                lblExito.Text = "PIN cambiado exitosamente.";
                lblExito.Visible = true;

                // Limpiar campos
                txtLegajo.Text = "";
                txtPinActual.Text = "";
                txtNuevoPin.Text = "";
                txtConfirmarPin.Text = "";
                txtLegajo.Focus();
            }
            else
            {
                lblError.Text = "Error al guardar el PIN. Intente nuevamente.";
            }
        }
    }
}
