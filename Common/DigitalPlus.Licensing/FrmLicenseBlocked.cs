using System;
using System.Windows.Forms;

namespace DigitalPlus.Licensing
{
    public partial class FrmLicenseBlocked : Form
    {
        private readonly LicenseManager _licenseManager;
        private readonly LicenseValidationResult _initialResult;

        public FrmLicenseBlocked(LicenseValidationResult result, LicenseManager licenseManager)
        {
            InitializeComponent();
            _licenseManager = licenseManager;
            _initialResult = result;
            lblMessage.Text = result.UserMessage;

            // Solo mostrar panel de activacion si es trial/no-license
            bool showActivation = result.State == LicenseState.TrialExpired
                || result.State == LicenseState.TrialLegajosExceeded
                || result.State == LicenseState.NoLicense;
            pnlActivation.Visible = showActivation;
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            var code = txtCodigo.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Ingrese un codigo de activacion.", "Activacion",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnActivar.Enabled = false;
            btnActivar.Text = "Validando...";
            Application.DoEvents();

            var result = _licenseManager.ActivateWithCode(code);

            if (!result.IsBlocked)
            {
                var info = _licenseManager.GetLicenseInfoText();
                MessageBox.Show("Licencia activada correctamente.\n\n" + info,
                    "Activacion exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            lblMessage.Text = result.UserMessage;
            btnActivar.Enabled = true;
            btnActivar.Text = "Activar";
        }

        private void btnReintentar_Click(object sender, EventArgs e)
        {
            btnReintentar.Enabled = false;
            btnReintentar.Text = "Verificando...";
            Application.DoEvents();

            var result = _licenseManager.ValidateAtStartup();

            if (!result.IsBlocked)
            {
                MessageBox.Show("Licencia verificada correctamente.", "Licencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            lblMessage.Text = result.UserMessage;
            btnReintentar.Enabled = true;
            btnReintentar.Text = "Reintentar";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
