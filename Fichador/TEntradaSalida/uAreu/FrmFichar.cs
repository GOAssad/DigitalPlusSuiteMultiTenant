using Acceso.Clases.Datos.Generales;
using Acceso.Clases.Datos.RRHH;
using DigitalPlus.Licensing;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Acceso.uAreu
{
    delegate void Function();

    public partial class FrmFichar : Form, DPFP.Capture.EventHandler
    {
        private DPFP.Verification.Verification Verificator;
        private DPFP.Capture.Capture Capturer;

        private bool controlcolor;

        private RRHHLegajosHuellas oLHuellas = new RRHHLegajosHuellas();
        private RRHHFichadas oFichada = new RRHHFichadas();
        private GRALTerminales oTerminal = new GRALTerminales();

        private string sNombre;
        private string sBienVenidaAux;
        private string sCadenaEntraSale;

        private LicenseManager _licenseManager;
        private System.Windows.Forms.Timer _licenseTimer;

        private enum ModoFichada { Huella, Pin, Demo }
        private ModoFichada _modoActual;
        private bool _lectorDisponible;
        private bool _lectorFisico; // true solo cuando OnReaderConnect se dispara
        private bool _pinHabilitado;
        private bool _demoHabilitado;
        private System.Windows.Forms.Timer _timerDetectarLector;

        public FrmFichar()
        {
            InitializeComponent();
        }

        public FrmFichar(LicenseManager licenseManager) : this()
        {
            _licenseManager = licenseManager;
            _licenseTimer = new System.Windows.Forms.Timer();
            _licenseTimer.Interval = 4 * 60 * 60 * 1000; // 4 horas
            _licenseTimer.Tick += LicenseTimer_Tick;
            _licenseTimer.Start();
        }

        private void LicenseTimer_Tick(object sender, EventArgs e)
        {
            if (_licenseManager == null) return;
            int legajos = Program.ContarLegajos();
            var result = _licenseManager.PeriodicCheck(legajos);
            if (result.IsBlocked)
            {
                _licenseTimer.Stop();
                MessageBox.Show(result.UserMessage, "Licencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }

        protected virtual void Init()
        {
            _lectorDisponible = false;
            try
            {
                Capturer = new DPFP.Capture.Capture();
                if (null != Capturer)
                {
                    Capturer.EventHandler = this;
                    _lectorDisponible = true;
                }
            }
            catch
            {
                Capturer = null;
                _lectorDisponible = false;
            }
            Text = "DigitalPlus Fichadas";
            if (_lectorDisponible)
                Verificator = new DPFP.Verification.Verification();
        }

        private void DetectarModoInicial()
        {
            // Leer configuracion de modos
            try
            {
                _pinHabilitado = RRHHLegajosPin.ModoPinHabilitado();
                _demoHabilitado = RRHHLegajosPin.ModoDemoHabilitado();
            }
            catch
            {
                // Si las variables no existen en BD, habilitar PIN y Demo por defecto
                _pinHabilitado = true;
                _demoHabilitado = true;
            }

            if (_lectorDisponible)
            {
                // El SDK se inicializo OK, pero eso no significa que haya lector fisico.
                // Arrancamos en modo Huella y esperamos 3 segundos a ver si OnReaderConnect se dispara.
                CambiarModo(ModoFichada.Huella);
                lblEstado.Text = "Buscando lector de huellas...";
                lblEstado.ForeColor = Color.DimGray;

                _timerDetectarLector = new System.Windows.Forms.Timer();
                _timerDetectarLector.Interval = 3000;
                _timerDetectarLector.Tick += TimerDetectarLector_Tick;
                _timerDetectarLector.Start();
            }
            else
            {
                // SDK no se pudo inicializar: no hay driver instalado
                CambiarModoSinLector();
            }
        }

        private void TimerDetectarLector_Tick(object sender, EventArgs e)
        {
            _timerDetectarLector.Stop();
            _timerDetectarLector.Dispose();
            _timerDetectarLector = null;

            if (!_lectorFisico)
            {
                // Pasaron 3 segundos y no se detecto lector fisico
                _lectorDisponible = false;
                CambiarModoSinLector();
            }
            else
            {
                // Lector confirmado, actualizar el link por si hay otros modos
                ActualizarLinkModo();
            }
        }

        private void CambiarModoSinLector()
        {
            // Sin lector fisico: elegir el mejor modo alternativo
            if (_pinHabilitado)
            {
                CambiarModo(ModoFichada.Pin);
                lblEstado.Text = "No se detecto lector de huellas.";
                lblEstado.ForeColor = Color.DarkOrange;
            }
            else if (_demoHabilitado)
            {
                CambiarModo(ModoFichada.Demo);
            }
            else
            {
                // Fallback: habilitar PIN y Demo automaticamente
                _pinHabilitado = true;
                _demoHabilitado = true;
                CambiarModo(ModoFichada.Pin);
                lblEstado.Text = "No se detecto lector de huellas.";
                lblEstado.ForeColor = Color.DarkOrange;
            }
        }

        private bool EsTrialActivo()
        {
            return _licenseManager?.CurrentTicket != null
                && _licenseManager.CurrentTicket.LicenseType == "trial";
        }

        private void CambiarModo(ModoFichada modo)
        {
            _modoActual = modo;

            PictureHuella.Visible = (modo == ModoFichada.Huella);
            panelPin.Visible = (modo == ModoFichada.Pin);
            panelDemo.Visible = (modo == ModoFichada.Demo);

            // Limpiar estado anterior
            sBienVenidaAux = string.Empty;
            sCadenaEntraSale = string.Empty;
            etiquetaNombre.Text = "";
            etiquetaES.Text = "";

            // Controlar captura de huella
            if (modo == ModoFichada.Huella && _lectorDisponible)
            {
                Start();
                lblEstado.Text = "Coloque su dedo en el lector";
                lblEstado.ForeColor = Color.DimGray;
            }
            else
            {
                // No llamar Stop() para mantener el monitoreo de conexion/desconexion del lector.
                // Solo limpiar el estado visual.
                if (modo != ModoFichada.Huella)
                {
                    lblEstado.Text = "";
                }
            }

            // Link para cambiar modo
            ActualizarLinkModo();

            // Cargar combo demo si corresponde
            if (modo == ModoFichada.Demo)
                CargarLegajosDemo();

            // Focus
            if (modo == ModoFichada.Pin)
            {
                txtLegajoId.Text = "";
                txtPin.Text = "";
                lblPinError.Text = "";
                txtLegajoId.Focus();
            }
        }

        private void ActualizarLinkModo()
        {
            // Construir opciones de cambio de modo
            var modos = new System.Collections.Generic.List<string>();

            if (_modoActual != ModoFichada.Huella && _lectorDisponible)
                modos.Add("Huella");
            if (_modoActual != ModoFichada.Pin && _pinHabilitado)
                modos.Add("PIN");
            if (_modoActual != ModoFichada.Demo && _demoHabilitado)
                modos.Add("Demo");

            if (modos.Count > 0)
                lnkCambiarModo.Text = "Cambiar a modo: " + string.Join(" | ", modos);
            else
                lnkCambiarModo.Text = "";
        }

        private void lnkCambiarModo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Rotar entre modos disponibles
            if (_modoActual == ModoFichada.Huella)
            {
                if (_pinHabilitado) CambiarModo(ModoFichada.Pin);
                else if (_demoHabilitado) CambiarModo(ModoFichada.Demo);
            }
            else if (_modoActual == ModoFichada.Pin)
            {
                if (_demoHabilitado) CambiarModo(ModoFichada.Demo);
                else if (_lectorDisponible) CambiarModo(ModoFichada.Huella);
            }
            else // Demo
            {
                if (_lectorDisponible) CambiarModo(ModoFichada.Huella);
                else if (_pinHabilitado) CambiarModo(ModoFichada.Pin);
            }
        }

        #region Fichada por PIN

        private void btnFicharPin_Click(object sender, EventArgs e)
        {
            FicharConPin();
        }

        private void FicharConPin()
        {
            string legajoId = txtLegajoId.Text.Trim();
            string pin = txtPin.Text.Trim();

            if (string.IsNullOrEmpty(legajoId))
            {
                lblPinError.Text = "Ingrese el numero de legajo.";
                txtLegajoId.Focus();
                return;
            }

            if (string.IsNullOrEmpty(pin))
            {
                lblPinError.Text = "Ingrese el PIN.";
                txtPin.Focus();
                return;
            }

            lblPinError.Text = "";
            var pinHelper = new RRHHLegajosPin();

            // Verificar si el legajo existe y tiene PIN
            if (!pinHelper.CargarLegajo(legajoId))
            {
                lblPinError.Text = "Legajo no encontrado.";
                txtLegajoId.Focus();
                return;
            }

            // Si no tiene PIN asignado o el admin reseteo el PIN, pedir crear uno nuevo
            if (!pinHelper.HasPin || pinHelper.PinMustChange)
            {
                string msg = !pinHelper.HasPin
                    ? "Este legajo no tiene PIN asignado.\n¿Desea crear uno ahora?"
                    : "El administrador requiere que cambie su PIN.\n¿Desea cambiarlo ahora?";
                string title = !pinHelper.HasPin ? "PIN no configurado" : "Cambio de PIN requerido";

                var result = MessageBox.Show(msg, title,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (var frm = new FrmCambiarPin(legajoId, pinHelper.sLegajoNombre, !pinHelper.HasPin))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                            lblPinError.Text = "PIN " + (!pinHelper.HasPin ? "creado" : "cambiado") + ". Ingrese su PIN para fichar.";
                    }
                }
                txtPin.Text = "";
                txtPin.Focus();
                return;
            }

            // Verificar PIN
            if (!pinHelper.VerificarPin(legajoId, pin))
            {
                lblPinError.Text = "PIN incorrecto.";
                txtPin.Text = "";
                txtPin.Focus();
                return;
            }

            // PIN expirado
            if (pinHelper.PinExpired)
            {
                MessageBox.Show("Su PIN ha expirado. Debe cambiarlo para continuar.",
                    "Cambio de PIN requerido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                using (var frm = new FrmCambiarPin(legajoId, pinHelper.sLegajoNombre, false))
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        txtPin.Text = "";
                        return;
                    }
                }
            }

            // Registrar fichada
            RegistrarFichada(pinHelper.nLegajoID, pinHelper.sLegajoID, pinHelper.sLegajoNombre);

            // Limpiar campos
            txtLegajoId.Text = "";
            txtPin.Text = "";
            txtLegajoId.Focus();
        }

        private void lnkCambiarPin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new FrmCambiarPinVoluntario())
            {
                frm.ShowDialog(this);
            }
        }

        #endregion

        #region Modo Demo

        private void CargarLegajosDemo()
        {
            try
            {
                DataTable dt = RRHHLegajosPin.ListaLegajosActivos();
                cmbLegajosDemo.DataSource = dt;
                cmbLegajosDemo.DisplayMember = "sLegajoNombre";
                cmbLegajosDemo.ValueMember = "nLegajoID";
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error cargando legajos: " + ex.Message;
                lblEstado.ForeColor = Color.Red;
            }
        }

        private void btnFicharDemo_Click(object sender, EventArgs e)
        {
            if (cmbLegajosDemo.SelectedItem == null)
            {
                lblEstado.Text = "Seleccione un legajo.";
                lblEstado.ForeColor = Color.DarkOrange;
                return;
            }

            DataRowView row = cmbLegajosDemo.SelectedItem as DataRowView;
            if (row == null) return;

            int nLegajoID = Convert.ToInt32(row["nLegajoID"]);
            string sLegajoID = row.Row.Table.Columns.Contains("sLegajoID")
                ? row["sLegajoID"].ToString() : "";
            string nombre = row["sLegajoNombre"].ToString();

            RegistrarFichada(nLegajoID, sLegajoID, nombre);
        }

        #endregion

        #region Registrar Fichada (comun a todos los modos)

        private void RegistrarFichada(int nLegajoID, string sLegajoID, string nombre)
        {
            sNombre = nombre;
            oFichada.sLegajoID = sLegajoID;
            oFichada.dRegistro = DateTime.Now;
            oFichada.nSucursalID = oTerminal.sSucursalID.sSucursalID.ToString() != string.Empty
                ? Convert.ToInt32(oTerminal.sSucursalID.sSucursalID.ToString()) : 0;
            oFichada.nLegajoID = nLegajoID;
            // Mapear al valor del enum OrigenFichada del portal: Huella=0, PIN=1, Demo=2
            oFichada.sOrigen = _modoActual == ModoFichada.Pin ? "PIN" : _modoActual.ToString();

            if (oFichada.Actualizar())
            {
                sCadenaEntraSale = oFichada.sEntraSale == "E" ? "ENTRADA" : "SALIDA";
                sBienVenidaAux = oFichada.sEntraSale == "E"
                    ? "Bienvenido " + sNombre
                    : "Hasta Luego " + sNombre;
                MakeReport();
                ActivarSemaforo(3);
                controlcolor = true;
            }
            else
            {
                sBienVenidaAux = string.Empty;
                sCadenaEntraSale = string.Empty;
                MakeReport();
                ActivarSemaforo(1);
            }
            timer.Enabled = true;
        }

        #endregion

        #region Huella digital (logica original)

        protected virtual void Process(DPFP.Sample Sample)
        {
            // Ignorar huellas si no estamos en modo Huella
            if (_modoActual != ModoFichada.Huella)
                return;

            ActivarSemaforo(4);
            DrawPicture(ConvertSampleToBitmap(Sample));

            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);
            if (features != null)
            {
                DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                DPFP.Template template = new DPFP.Template();
                Stream stream;
                sNombre = string.Empty;
                sBienVenidaAux = string.Empty;
                sCadenaEntraSale = string.Empty;

                try
                {
                    oLHuellas.TodasLasHuellas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                ActivarSemaforo(2);

                for (int i = 0; i < oLHuellas.dtLegajosHuellas.Rows.Count; i++)
                {
                    stream = new MemoryStream((byte[])oLHuellas.dtLegajosHuellas.Rows[i]["iHuella"]);
                    template = new DPFP.Template(stream);
                    Verificator.Verify(features, template, ref result);
                    controlcolor = false;

                    if (result.Verified)
                    {
                        int nLegajoID = oLHuellas.dtLegajosHuellas.Rows[i]["nLegajoID"].ToString() != string.Empty
                            ? Convert.ToInt32(oLHuellas.dtLegajosHuellas.Rows[i]["nLegajoID"].ToString()) : 0;
                        string sLegajoID = oLHuellas.dtLegajosHuellas.Rows[i]["sLegajoID"].ToString();
                        string nombre = oLHuellas.dtLegajosHuellas.Rows[i]["sLegajoNombre"].ToString();

                        this.Invoke(new Function(delegate ()
                        {
                            RegistrarFichada(nLegajoID, sLegajoID, nombre);
                        }));
                        break;
                    }
                    template = null;
                }

                if (sNombre == string.Empty)
                {
                    MakeReport();
                    ActivarSemaforo(1);
                }
            }
        }

        #endregion

        protected void Start()
        {
            if (null != Capturer)
            {
                try { Capturer.StartCapture(); }
                catch { }
            }
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try { Capturer.StopCapture(); }
                catch { }
            }
        }

        #region Form Event Handlers

        private void CaptureForm_Load(object sender, EventArgs e)
        {
            string nombreEmpresa = ConfigurationManager.AppSettings["NombreEmpresa"];
            if (!string.IsNullOrEmpty(nombreEmpresa))
            {
                lblEmpresa.Text = nombreEmpresa;
                this.Text = "DigitalPlus Fichadas - " + nombreEmpresa;
            }

            ConfiguracionLocal();
            Init();
            DetectarModoInicial();
            timerHora.Enabled = true;

            // Mostrar info de licencia en barra inferior
            if (_licenseManager != null && _licenseManager.CurrentTicket != null)
            {
                etiquetaSucursal.Text = _licenseManager.GetStatusBarText();
                etiquetaSucursal.Font = new Font("Segoe UI", 9F);
            }

            // Permitir Enter en campos PIN
            txtPin.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Enter) { FicharConPin(); ev.SuppressKeyPress = true; } };
            txtLegajoId.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Enter) { txtPin.Focus(); ev.SuppressKeyPress = true; } };
            // Solo digitos en PIN
            txtPin.KeyPress += (s, ev) => { if (!char.IsDigit(ev.KeyChar) && !char.IsControl(ev.KeyChar)) ev.Handled = true; };
        }

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
            Verificator = null;
        }

        #endregion

        #region EventHandler Members

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            ActivarSemaforo(controlcolor ? 3 : 1);
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            ActivarSemaforo(2);
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            _lectorFisico = true;
            _lectorDisponible = true;

            this.Invoke(new Function(delegate ()
            {
                // Cancelar timer de deteccion inicial si todavia esta corriendo
                if (_timerDetectarLector != null)
                {
                    _timerDetectarLector.Stop();
                    _timerDetectarLector.Dispose();
                    _timerDetectarLector = null;
                }

                // Siempre cambiar a modo huella cuando se conecta el lector
                if (_modoActual != ModoFichada.Huella)
                {
                    CambiarModo(ModoFichada.Huella);
                    lblEstado.Text = "Lector conectado - Coloque su dedo";
                    lblEstado.ForeColor = Color.Green;
                }
            }));
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            _lectorFisico = false;
            _lectorDisponible = false;

            this.Invoke(new Function(delegate ()
            {
                CambiarModoSinLector();
            }));
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
        }

        #endregion

        protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();
            Bitmap bitmap = null;
            Convertor.ConvertToPicture(Sample, ref bitmap);
            return bitmap;
        }

        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);
            return feedback == DPFP.Capture.CaptureFeedback.Good ? features : null;
        }

        protected void MakeReport()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Function(delegate ()
                {
                    etiquetaNombre.Text = sBienVenidaAux;
                    etiquetaES.Text = sCadenaEntraSale;
                    etiquetaES.ForeColor = (sCadenaEntraSale != null && sCadenaEntraSale.Contains("SALIDA"))
                        ? Color.OrangeRed
                        : Color.RoyalBlue;
                }));
            }
            else
            {
                etiquetaNombre.Text = sBienVenidaAux;
                etiquetaES.Text = sCadenaEntraSale;
                etiquetaES.ForeColor = (sCadenaEntraSale != null && sCadenaEntraSale.Contains("SALIDA"))
                    ? Color.OrangeRed
                    : Color.RoyalBlue;
            }
        }

        private void DrawPicture(Bitmap bitmap)
        {
            this.Invoke(new Function(delegate ()
            {
                PictureHuella.Image = new Bitmap(bitmap, PictureHuella.Size);
            }));
        }

        private void ActivarSemaforo(int pos)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Function(delegate () { ActivarSemaforoInternal(pos); }));
            }
            else
            {
                ActivarSemaforoInternal(pos);
            }
        }

        private void ActivarSemaforoInternal(int pos)
        {
            switch (pos)
            {
                case 1:
                    lblEstado.ForeColor = Color.Red;
                    lblEstado.Text = _modoActual == ModoFichada.Huella
                        ? "Huella no reconocida"
                        : "Error en fichada";
                    break;
                case 2:
                    lblEstado.ForeColor = Color.DarkOrange;
                    lblEstado.Text = "Verificando...";
                    break;
                case 3:
                    lblEstado.ForeColor = Color.Green;
                    lblEstado.Text = "¡Registro exitoso!";
                    break;
                default:
                    lblEstado.ForeColor = Color.DimGray;
                    lblEstado.Text = string.Empty;
                    break;
            }
        }

        private void ConfiguracionLocal()
        {
            oTerminal.sTerminalID = Environment.MachineName.ToString();
            oTerminal.Inicializar();
            if (oTerminal.Existe)
            {
                etiquetaSucursal.Text = oTerminal.sSucursalID.sSucursalID + "  -  " +
                    oTerminal.sSucursalID.sDescripcion.Trim();
                oFichada.sSucursalID = oTerminal.sSucursalID.sSucursalID;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            sBienVenidaAux = string.Empty;
            sNombre = string.Empty;
            sCadenaEntraSale = string.Empty;
            timer.Enabled = false;
            MakeReport();
            ActivarSemaforo(4);
            PictureHuella.Image = null;
            controlcolor = false;
        }

        private void timerHora_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void PictureHuella_Click(object sender, EventArgs e)
        {
        }
    }
}
