using System;
using System.Configuration;
using System.Windows.Forms;
using Acceso.Clases.Datos.Generales;
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

        private string ObtenerNombreEmpresa()
        {
            try
            {
                var empresa = EmpresaInfoService.ObtenerEmpresa();
                if (empresa != null && !string.IsNullOrEmpty(empresa.Nombre))
                    return empresa.Nombre;
            }
            catch { }

            return ConfigurationManager.AppSettings["NombreEmpresa"] ?? "Sin configurar";
        }

        private void MostrarInfoLicencia()
        {
            var mgr = Program.LicMgr;
            string nombreEmpresa = ObtenerNombreEmpresa();
            int legajosActuales = Program.ContarLegajos();

            // Mostrar codigo de activacion actual si existe
            string codigoActual = "";
            try
            {
                var empresa = EmpresaInfoService.ObtenerEmpresa();
                if (empresa != null && !string.IsNullOrEmpty(empresa.CodigoActivacion))
                    codigoActual = empresa.CodigoActivacion;
            }
            catch { }

            if (!string.IsNullOrEmpty(codigoActual))
            {
                lblCodigoActual.Text = "Codigo actual: " + codigoActual;
                lblCodigoActual.Visible = true;
            }
            else
            {
                lblCodigoActual.Visible = false;
            }

            if (mgr == null || mgr.CurrentTicket == null)
            {
                lblInfo.Text = string.Format("Empresa: {0}\nLegajos actuales: {1}\n\nNo hay informacion de licencia disponible.\nIngrese un codigo para activar.", nombreEmpresa, legajosActuales);
                lblEstadoValor.Text = "Sin licencia";
                lblEstadoValor.ForeColor = System.Drawing.Color.Red;
                return;
            }

            var t = mgr.CurrentTicket;
            var tipo = t.LicenseType == "trial" ? "Prueba" :
                       t.LicenseType == "active" ? "Activa" :
                       t.LicenseType == "suspended" ? "Suspendida" : t.LicenseType;
            var plan = string.IsNullOrEmpty(t.Plan) ? "" : t.Plan.Substring(0, 1).ToUpper() + t.Plan.Substring(1);

            string vencimiento;
            if (t.LicenseType == "trial" && t.TrialEndsAt.HasValue)
            {
                var dias = (int)(t.TrialEndsAt.Value - DateTime.UtcNow).TotalDays;
                if (dias < 0) dias = 0;
                vencimiento = t.TrialEndsAt.Value.ToLocalTime().ToString("dd/MM/yyyy") +
                    " (" + dias + " dia" + (dias != 1 ? "s" : "") + ")";
            }
            else if (t.ExpiresAt.HasValue)
            {
                var dias = (int)(t.ExpiresAt.Value - DateTime.UtcNow).TotalDays;
                if (dias < 0) dias = 0;
                vencimiento = t.ExpiresAt.Value.ToLocalTime().ToString("dd/MM/yyyy") +
                    " (" + dias + " dia" + (dias != 1 ? "s" : "") + ")";
            }
            else
            {
                vencimiento = "Sin vencimiento";
            }

            lblInfo.Text = string.Format(
                "Empresa: {0}\nLicencia: {1} - Plan {2}\nMax legajos: {3}\nLegajos actuales: {4}\nVence: {5}",
                nombreEmpresa, tipo, plan, t.MaxLegajos, legajosActuales, vencimiento);

            var dias2 = mgr.GetDaysRemaining();
            if (dias2.HasValue && dias2.Value <= 7)
            {
                lblEstadoValor.Text = string.Format("Vence en {0} dia(s)", dias2.Value);
                lblEstadoValor.ForeColor = System.Drawing.Color.OrangeRed;
            }
            else if (t.LicenseType == "trial")
            {
                lblEstadoValor.Text = "Periodo de prueba";
                lblEstadoValor.ForeColor = System.Drawing.Color.DarkOrange;
            }
            else if (t.LicenseType == "active")
            {
                lblEstadoValor.Text = "Licencia activa";
                lblEstadoValor.ForeColor = System.Drawing.Color.Green;
            }
            else if (t.LicenseType == "suspended")
            {
                lblEstadoValor.Text = "Licencia suspendida";
                lblEstadoValor.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblEstadoValor.Text = t.LicenseType;
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
