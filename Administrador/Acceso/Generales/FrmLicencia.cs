using System;
using System.Windows.Forms;
using DigitalPlus.Licensing;

namespace Acceso.Generales
{
    public partial class FrmLicencia : Form
    {
        public FrmLicencia()
        {
            InitializeComponent();
        }

        private void FrmLicencia_Load(object sender, EventArgs e)
        {
            MostrarInfoLicencia();
        }

        private void MostrarInfoLicencia()
        {
            var mgr = Program.LicMgr;
            if (mgr == null || mgr.CurrentTicket == null)
            {
                lblInfo.Text = "No hay informacion de licencia disponible.\n\nIngrese un codigo para activar.";
                lblEstadoValor.Text = "Sin licencia";
                lblEstadoValor.ForeColor = System.Drawing.Color.Red;
                return;
            }

            lblInfo.Text = mgr.GetLicenseInfoText();

            var dias = mgr.GetDaysRemaining();
            if (dias.HasValue && dias.Value <= 7)
            {
                lblEstadoValor.Text = string.Format("Vence en {0} dia(s)", dias.Value);
                lblEstadoValor.ForeColor = System.Drawing.Color.OrangeRed;
            }
            else if (mgr.CurrentTicket.LicenseType == "trial")
            {
                lblEstadoValor.Text = "Periodo de prueba";
                lblEstadoValor.ForeColor = System.Drawing.Color.DarkOrange;
            }
            else if (mgr.CurrentTicket.LicenseType == "active")
            {
                lblEstadoValor.Text = "Licencia activa";
                lblEstadoValor.ForeColor = System.Drawing.Color.Green;
            }
            else if (mgr.CurrentTicket.LicenseType == "suspended")
            {
                lblEstadoValor.Text = "Licencia suspendida";
                lblEstadoValor.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblEstadoValor.Text = mgr.CurrentTicket.LicenseType;
                lblEstadoValor.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            var code = txtCodigo.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Ingrese un codigo de activacion o renovacion.",
                    "Licencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnActivar.Enabled = false;
            btnActivar.Text = "Validando...";
            Application.DoEvents();

            var mgr = Program.LicMgr;
            if (mgr == null)
            {
                MessageBox.Show("El sistema de licencias no esta inicializado.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnActivar.Enabled = true;
                btnActivar.Text = "Activar / Renovar";
                return;
            }

            var result = mgr.ActivateWithCode(code);

            if (!result.IsBlocked)
            {
                var info = mgr.GetLicenseInfoText();
                MessageBox.Show("Licencia activada correctamente.\n\n" + info,
                    "Activacion exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCodigo.Text = "";
                MostrarInfoLicencia();
            }
            else
            {
                MessageBox.Show(result.UserMessage, "Error de activacion",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnActivar.Enabled = true;
            btnActivar.Text = "Activar / Renovar";
        }
    }
}
