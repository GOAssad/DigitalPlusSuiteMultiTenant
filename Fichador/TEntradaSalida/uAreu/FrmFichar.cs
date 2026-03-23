using Acceso.Clases.Datos.Generales;
using Acceso.Clases.Datos.RRHH;
using AForge.Video;
using AForge.Video.DirectShow;
using DigitalPlus.Licensing;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZXing;

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

        private enum ModoFichada { Huella, Pin, Demo, QR }
        private ModoFichada _modoActual;
        private bool _lectorDisponible;
        private bool _lectorFisico; // true solo cuando OnReaderConnect se dispara
        private bool _pinHabilitado;
        private bool _demoHabilitado;
        private bool _modoCambiadoPorUsuario;
        private System.Windows.Forms.Timer _timerDetectarLector;

        // QR - camara y decodificacion
        private VideoCaptureDevice _videoDevice;
        private FilterInfoCollection _videoDevices;
        private BarcodeReader _qrReader;
        private bool _qrHabilitado;
        private DateTime _ultimoQrLeido = DateTime.MinValue;
        private string _ultimoQrToken = "";
        private System.Windows.Forms.Timer _timerQrDecode;
        private Bitmap _frameActual;
        private readonly object _frameLock = new object();
        private volatile bool _cerrando;
        private volatile bool _primerFrameCamara;

        public FrmFichar()
        {
            InitializeComponent();

            // Version label - discreto, esquina inferior derecha
            var lblBuild = new System.Windows.Forms.Label();
            lblBuild.Text = "v" + Global.Datos.BuildInfo.FullVersion;
            lblBuild.Font = new System.Drawing.Font("Segoe UI", 7F);
            lblBuild.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            lblBuild.AutoSize = true;
            lblBuild.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblBuild.Location = new System.Drawing.Point(this.ClientSize.Width - 175, this.ClientSize.Height - 16);
            this.Controls.Add(lblBuild);
            lblBuild.BringToFront();
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
            HuellaLog.Write("Init() inicio");
            try
            {
                Capturer = new DPFP.Capture.Capture();
                HuellaLog.Write("Init() Capturer creado OK");
                if (null != Capturer)
                {
                    Capturer.EventHandler = this;
                    _lectorDisponible = true;
                    HuellaLog.Write("Init() EventHandler asignado, _lectorDisponible=true");
                }
            }
            catch (Exception ex)
            {
                HuellaLog.Write("Init() EXCEPCION: " + ex.Message);
                Capturer = null;
                _lectorDisponible = false;
            }
            Text = "Digital One";
            if (_lectorDisponible)
                Verificator = new DPFP.Verification.Verification();
            HuellaLog.Write("Init() fin, _lectorDisponible=" + _lectorDisponible);
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

            // Detectar camaras USB para modo QR
            try
            {
                HuellaLog.Write("DetectarModoInicial() Buscando camaras...");
                DetectarCamarasQR();
            }
            catch (Exception ex)
            {
                _qrHabilitado = false;
                HuellaLog.Write("DetectarModoInicial() EXCEPCION camaras: " + ex.GetType().Name + " - " + ex.Message);
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
                if (!_modoCambiadoPorUsuario)
                    CambiarModoSinLector();
                else
                    ActualizarLinkModo(); // Solo actualizar botones, no cambiar modo
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
            else if (_qrHabilitado)
            {
                CambiarModo(ModoFichada.QR);
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
            // Detener camara si salimos de modo QR
            if (_modoActual == ModoFichada.QR && modo != ModoFichada.QR)
                DetenerCamara();

            _modoActual = modo;

            panelLector.Visible = (modo == ModoFichada.Huella);
            panelPin.Visible = (modo == ModoFichada.Pin);
            panelDemo.Visible = (modo == ModoFichada.Demo);
            panelQR.Visible = (modo == ModoFichada.QR);

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

            // Iniciar camara si entramos a modo QR
            if (modo == ModoFichada.QR)
            {
                IniciarCamara();
                lblEstado.Text = "Presente su QR frente a la camara";
                lblEstado.ForeColor = Color.DimGray;
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

        private static readonly Color ModoActivoBg = Color.FromArgb(201, 168, 76);
        private static readonly Color ModoActivoFg = Color.FromArgb(13, 17, 28);
        private static readonly Color ModoInactivoBg = Color.FromArgb(30, 34, 54);
        private static readonly Color ModoInactivoFg = Color.FromArgb(140, 140, 170);
        private static readonly Color ModoOcultoBg = Color.FromArgb(16, 20, 36);

        private void ActualizarLinkModo()
        {
            try
            {
                // Actualizar botones de modo visual
                ActualizarBotonModo(btnModoHuella, ModoFichada.Huella, _lectorDisponible);
                ActualizarBotonModo(btnModoPin, ModoFichada.Pin, true);
                ActualizarBotonModo(btnModoQR, ModoFichada.QR, _qrHabilitado);
                ActualizarBotonModo(btnModoDemo, ModoFichada.Demo, _demoHabilitado);
            }
            catch { }
        }

        private void ActualizarBotonModo(System.Windows.Forms.Label btn, ModoFichada modo, bool disponible)
        {
            if (!disponible)
            {
                btn.Visible = false;
                return;
            }
            btn.Visible = true;
            if (_modoActual == modo)
            {
                btn.BackColor = ModoActivoBg;
                btn.ForeColor = ModoActivoFg;
            }
            else
            {
                btn.BackColor = ModoInactivoBg;
                btn.ForeColor = ModoInactivoFg;
            }
        }

        private void lnkCambiarModo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Rotar: Huella -> PIN -> Demo -> QR -> Huella...
            ModoFichada? siguiente = null;

            // Orden de rotacion
            var orden = new[] { ModoFichada.Huella, ModoFichada.Pin, ModoFichada.Demo, ModoFichada.QR };
            int idxActual = Array.IndexOf(orden, _modoActual);

            for (int i = 1; i < orden.Length; i++)
            {
                var candidato = orden[(idxActual + i) % orden.Length];
                if (ModoDisponible(candidato))
                {
                    siguiente = candidato;
                    break;
                }
            }

            if (siguiente.HasValue)
                CambiarModo(siguiente.Value);
        }

        private bool ModoDisponible(ModoFichada modo)
        {
            switch (modo)
            {
                case ModoFichada.Huella: return _lectorDisponible;
                case ModoFichada.Pin: return true; // PIN siempre disponible
                case ModoFichada.Demo: return _demoHabilitado;
                case ModoFichada.QR: return _qrHabilitado;
                default: return false;
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
                if (pinHelper.PinMustChange)
                {
                    // Cambio obligatorio: solo OK, abre directo el formulario de cambio
                    MessageBox.Show("El administrador requiere que cambie su PIN.",
                        "Cambio de PIN requerido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    using (var frm = new FrmCambiarPin(legajoId, pinHelper.sLegajoNombre, false))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                            lblPinError.Text = "PIN cambiado. Ingrese su nuevo PIN para fichar.";
                    }
                }
                else
                {
                    // No tiene PIN: preguntar si quiere crear
                    var result = MessageBox.Show("Este legajo no tiene PIN asignado.\n¿Desea crear uno ahora?",
                        "PIN no configurado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        using (var frm = new FrmCambiarPin(legajoId, pinHelper.sLegajoNombre, true))
                        {
                            if (frm.ShowDialog() == DialogResult.OK)
                                lblPinError.Text = "PIN creado. Ingrese su PIN para fichar.";
                        }
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

        #region Fichada por QR (camara USB)

        private void DetectarCamarasQR()
        {
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            int count = _videoDevices != null ? _videoDevices.Count : 0;
            HuellaLog.Write("DetectarCamarasQR() count=" + count);
            _qrHabilitado = (count > 0);
            if (_qrHabilitado)
            {
                for (int i = 0; i < _videoDevices.Count; i++)
                    HuellaLog.Write("  Camara[" + i + "]: " + _videoDevices[i].Name);
                _qrReader = new BarcodeReader();
                _qrReader.Options.PossibleFormats = new[] { BarcodeFormat.QR_CODE };
                HuellaLog.Write("DetectarCamarasQR() QR habilitado OK");
            }
            else
            {
                HuellaLog.Write("DetectarCamarasQR() No se encontraron camaras");
            }
        }

        private void IniciarCamara()
        {
            try
            {
                if (_videoDevices == null || _videoDevices.Count == 0) return;

                _primerFrameCamara = false;
                _videoDevice = new VideoCaptureDevice(_videoDevices[0].MonikerString);
                _videoDevice.NewFrame += VideoDevice_NewFrame;
                _videoDevice.VideoSourceError += (s, e) =>
                {
                    if (!_cerrando && !IsDisposed)
                        BeginInvoke((Action)(() => MostrarCamaraOcupada()));
                };
                _videoDevice.Start();

                _timerQrDecode = new System.Windows.Forms.Timer();
                _timerQrDecode.Interval = 250;
                _timerQrDecode.Tick += TimerQrDecode_Tick;
                _timerQrDecode.Start();

                // Verificar que llegan frames (si la cámara está ocupada, no llegan)
                var timerCheck = new System.Windows.Forms.Timer { Interval = 1500 };
                timerCheck.Tick += (s, e) =>
                {
                    timerCheck.Stop();
                    timerCheck.Dispose();
                    if (!_primerFrameCamara && !_cerrando)
                        MostrarCamaraOcupada();
                };
                timerCheck.Start();

                HuellaLog.Write("IniciarCamara() OK - " + _videoDevices[0].Name);
            }
            catch (Exception ex)
            {
                HuellaLog.Write("IniciarCamara() ERROR: " + ex.Message);
                MostrarCamaraOcupada();
            }
        }

        private void MostrarCamaraOcupada()
        {
            DetenerCamara();

            string proceso = null;
            try
            {
                foreach (var p in System.Diagnostics.Process.GetProcesses())
                {
                    try
                    {
                        string name = p.ProcessName.ToLowerInvariant();
                        if (name == "acceso" || name.Contains("administrador"))
                        { proceso = "DigitalOne Administrador"; break; }
                    }
                    catch { }
                }
            }
            catch { }

            string msg = "Cámara no disponible\nEstá siendo utilizada por otra aplicación";
            if (!string.IsNullOrEmpty(proceso))
                msg += $":\n{proceso}";

            // Dibujar mensaje en el PictureBox de la cámara
            int w = picCamara.Width > 0 ? picCamara.Width : 240;
            int h = picCamara.Height > 0 ? picCamara.Height : 185;
            var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(20, 25, 45));
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using (var font = new Font("Segoe UI", 9))
                    g.DrawString(msg, font, Brushes.DarkGoldenrod, new RectangleF(5, 5, w - 10, h - 10), sf);
            }
            picCamara.Image = bmp;
            HuellaLog.Write("Camara ocupada" + (proceso != null ? " por " + proceso : ""));
        }

        private void DetenerCamara()
        {
            if (_timerQrDecode != null)
            {
                _timerQrDecode.Stop();
                _timerQrDecode.Dispose();
                _timerQrDecode = null;
            }

            if (_videoDevice != null)
            {
                try { _videoDevice.NewFrame -= VideoDevice_NewFrame; } catch { }
                try
                {
                    if (_videoDevice.IsRunning)
                    {
                        _videoDevice.SignalToStop();
                        _videoDevice.WaitForStop();
                    }
                }
                catch { }
                _videoDevice = null;
            }

            lock (_frameLock)
            {
                if (_frameActual != null)
                {
                    _frameActual.Dispose();
                    _frameActual = null;
                }
            }

            try { if (!_cerrando && picCamara != null) picCamara.Image = null; } catch { }
        }

        private void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (_cerrando || _videoDevice == null) return;
            _primerFrameCamara = true;

            try
            {
                lock (_frameLock)
                {
                    if (_frameActual != null)
                        _frameActual.Dispose();
                    _frameActual = (Bitmap)eventArgs.Frame.Clone();
                }

                if (_cerrando || this.IsDisposed || !this.IsHandleCreated) return;
                var display = (Bitmap)eventArgs.Frame.Clone();
                this.BeginInvoke(new Function(delegate ()
                {
                    try
                    {
                        if (_cerrando || picCamara == null) { display.Dispose(); return; }
                        var old = picCamara.Image;
                        picCamara.Image = display;
                        if (old != null) old.Dispose();
                    }
                    catch { display.Dispose(); }
                }));
            }
            catch { }
        }

        private void TimerQrDecode_Tick(object sender, EventArgs e)
        {
            Bitmap frame = null;
            lock (_frameLock)
            {
                if (_frameActual != null)
                    frame = (Bitmap)_frameActual.Clone();
            }

            if (frame == null) return;

            try
            {
                var result = _qrReader.Decode(frame);
                if (result != null && !string.IsNullOrEmpty(result.Text))
                {
                    string token = result.Text.Trim();

                    // Cooldown: mismo QR no repite en 5 segundos
                    if (token == _ultimoQrToken && (DateTime.Now - _ultimoQrLeido).TotalSeconds < 5)
                        return;

                    _ultimoQrToken = token;
                    _ultimoQrLeido = DateTime.Now;

                    HuellaLog.Write("QR detectado: " + token);

                    var pinHelper = new RRHHLegajosPin();
                    if (pinHelper.BuscarPorQrToken(token))
                    {
                        HuellaLog.Write("QR legajo encontrado: " + pinHelper.sLegajoNombre + " (ID=" + pinHelper.nLegajoID + ")");
                        RegistrarFichada(pinHelper.nLegajoID, pinHelper.sLegajoID, pinHelper.sLegajoNombre);
                    }
                    else
                    {
                        HuellaLog.Write("QR no encontrado en EmpresaId=" + Global.Datos.TenantContext.EmpresaId);
                        ActivarSemaforo(1);
                        etiquetaNombre.Text = "QR leido pero no corresponde a esta empresa";
                        etiquetaNombre.ForeColor = Color.FromArgb(220, 50, 50);
                        lblEstado.Text = "QR no reconocido";
                        lblEstado.ForeColor = Color.FromArgb(220, 50, 50);
                        timer.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                HuellaLog.Write("TimerQrDecode ERROR: " + ex.Message);
            }
            finally
            {
                frame.Dispose();
            }
        }

        #endregion

        #region Registrar Fichada (comun a todos los modos)

        private void RegistrarFichada(int nLegajoID, string sLegajoID, string nombre)
        {
            HuellaLog.Write("RegistrarFichada() inicio legajo=" + sLegajoID + " nombre=" + nombre + " nLegajoID=" + nLegajoID);
            try
            {
                sNombre = nombre;
                oFichada.sLegajoID = sLegajoID;
                oFichada.dRegistro = DateTime.Now;
                int sucursalId = 0;
                try
                {
                    if (oTerminal.sSucursalID != null && !string.IsNullOrEmpty(oTerminal.sSucursalID.sSucursalID))
                        sucursalId = Convert.ToInt32(oTerminal.sSucursalID.sSucursalID);
                }
                catch { }
                oFichada.nSucursalID = sucursalId;
                oFichada.nLegajoID = nLegajoID;
                oFichada.nTerminalId = oTerminal.nId;
                // Mapear al valor del enum OrigenFichada del portal: Huella=0, PIN=1, Demo=2, QR=0
                oFichada.sOrigen = _modoActual == ModoFichada.Pin ? "PIN" : _modoActual.ToString();
                HuellaLog.Write("RegistrarFichada() nLegajoID=" + nLegajoID + " sucursalId=" + sucursalId + " origen=" + oFichada.sOrigen + " empresaId=" + Global.Datos.TenantContext.EmpresaId);

                if (oFichada.Actualizar())
                {
                    // Verificar si el SP denegó por permisos de sucursal
                    if (oFichada.sEntraSale == "DENEGADO")
                    {
                        HuellaLog.Write("RegistrarFichada() DENEGADO por permisos de sucursal");
                        sBienVenidaAux = "Fichada no permitida en esta sucursal";
                        sCadenaEntraSale = string.Empty;
                        MakeReport();
                        ActivarSemaforo(1);
                    }
                    else
                    {
                        HuellaLog.Write("RegistrarFichada() OK - EntraSale=" + oFichada.sEntraSale);
                        sCadenaEntraSale = oFichada.sEntraSale == "E" ? "ENTRADA" : "SALIDA";
                        sBienVenidaAux = oFichada.sEntraSale == "E"
                            ? "Bienvenido " + sNombre
                            : "Hasta Luego " + sNombre;
                        MakeReport();
                        MostrarAvatar(nLegajoID, nombre);
                        ActivarSemaforo(3);
                        controlcolor = true;
                    }
                }
                else
                {
                    HuellaLog.Write("RegistrarFichada() FALLO");
                    sBienVenidaAux = string.Empty;
                    sCadenaEntraSale = string.Empty;
                    MakeReport();
                    ActivarSemaforo(1);
                }
            }
            catch (Exception ex)
            {
                HuellaLog.Write("RegistrarFichada() EXCEPTION: " + ex.ToString());
                ActivarSemaforo(1);
            }
            timer.Enabled = true;
        }

        #endregion

        #region Huella digital (logica original)

        protected virtual void Process(DPFP.Sample Sample)
        {
            HuellaLog.Write("Process() inicio, _modoActual=" + _modoActual);
            // Ignorar huellas si no estamos en modo Huella
            if (_modoActual != ModoFichada.Huella)
            {
                HuellaLog.Write("Process() IGNORADO - no estamos en modo Huella");
                return;
            }

            ActivarSemaforo(4);
            DrawPicture(ConvertSampleToBitmap(Sample));

            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);
            HuellaLog.Write("Process() features=" + (features != null ? "OK" : "NULL (mala calidad)"));
            if (features != null)
            {
                DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                DPFP.Template template = new DPFP.Template();
                sNombre = string.Empty;
                sBienVenidaAux = string.Empty;
                sCadenaEntraSale = string.Empty;

                try
                {
                    oLHuellas.TodasLasHuellas();
                    HuellaLog.Write("TodasLasHuellas() OK, error='" + oLHuellas.sMensajeError + "'");
                }
                catch (Exception ex)
                {
                    HuellaLog.Write("TodasLasHuellas() EXCEPTION: " + ex.Message);
                    MessageBox.Show(ex.Message);
                    return;
                }

                int totalHuellas = oLHuellas.dtLegajosHuellas != null ? oLHuellas.dtLegajosHuellas.Rows.Count : 0;
                HuellaLog.Write("Total huellas cargadas: " + totalHuellas);

                ActivarSemaforo(2);

                for (int i = 0; i < oLHuellas.dtLegajosHuellas.Rows.Count; i++)
                {
                    // Usar DeSerialize para reconstruir desde Bytes (mismo formato que Administrador al grabar)
                    byte[] huellaBytes = (byte[])oLHuellas.dtLegajosHuellas.Rows[i]["iHuella"];
                    template = new DPFP.Template();
                    template.DeSerialize(huellaBytes);
                    Verificator.Verify(features, template, ref result);
                    HuellaLog.Write("Huella[" + i + "] legajo=" + oLHuellas.dtLegajosHuellas.Rows[i]["sLegajoID"] +
                        " dedo=" + oLHuellas.dtLegajosHuellas.Rows[i]["nDedo"] +
                        " FAR=" + result.FARAchieved + " verified=" + result.Verified);
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
            HuellaLog.Write("Start() llamado, Capturer=" + (Capturer != null ? "OK" : "NULL"));
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                    HuellaLog.Write("Start() StartCapture() OK");
                }
                catch (Exception ex)
                {
                    HuellaLog.Write("Start() EXCEPCION: " + ex.GetType().Name + " - " + ex.Message);
                }
            }
        }

        protected void Stop()
        {
            HuellaLog.Write("Stop() llamado");
            if (null != Capturer)
            {
                try { Capturer.StopCapture(); }
                catch { }
            }
        }

        #region Form Event Handlers

        private void CaptureForm_Load(object sender, EventArgs e)
        {
            // Formas visuales: ovalo lector + circulo avatar
            AplicarFormaLector();
            AplicarFormaAvatar();

            string nombreEmpresa = ConfigurationManager.AppSettings["NombreEmpresa"];
            if (!string.IsNullOrEmpty(nombreEmpresa))
            {
                lblEmpresa.Text = nombreEmpresa;
                this.Text = "Digital One v" + Global.Datos.BuildInfo.FullVersion + " - " + nombreEmpresa;
            }

            CargarLogos();

            // Verificar que la empresa esta activa en DigitalPlusAdmin
            var empresaInfo = Acceso.Clases.Datos.Generales.EmpresaInfoService.ObtenerEmpresa();
            if (empresaInfo != null && !string.IsNullOrEmpty(empresaInfo.Estado) && empresaInfo.Estado != "activa")
            {
                MessageBox.Show(
                    "El acceso a su empresa ha sido suspendido.\nContacte al administrador del sistema.",
                    "Acceso Suspendido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s2, e2) => this.Close();
                return;
            }

            // Diagnostico de BD al arrancar
            DiagnosticoBD();

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

            // Botones de modo
            btnModoHuella.Click += (s, ev) => { if (_lectorDisponible) { _modoCambiadoPorUsuario = true; CambiarModo(ModoFichada.Huella); } };
            btnModoPin.Click += (s, ev) => { _modoCambiadoPorUsuario = true; CambiarModo(ModoFichada.Pin); };
            btnModoQR.Click += (s, ev) => { if (_qrHabilitado) { _modoCambiadoPorUsuario = true; CambiarModo(ModoFichada.QR); } };
            btnModoDemo.Click += (s, ev) => { if (_demoHabilitado) { _modoCambiadoPorUsuario = true; CambiarModo(ModoFichada.Demo); } };
        }

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _cerrando = true;

            // Detener timer QR primero
            if (_timerQrDecode != null)
            {
                _timerQrDecode.Stop();
                _timerQrDecode.Dispose();
                _timerQrDecode = null;
            }

            // Desuscribir y detener la camara completamente
            if (_videoDevice != null)
            {
                try { _videoDevice.NewFrame -= VideoDevice_NewFrame; } catch { }
                try
                {
                    if (_videoDevice.IsRunning)
                    {
                        _videoDevice.SignalToStop();
                        _videoDevice.WaitForStop();
                    }
                }
                catch { }
                _videoDevice = null;
            }

            lock (_frameLock)
            {
                if (_frameActual != null)
                {
                    _frameActual.Dispose();
                    _frameActual = null;
                }
            }

            Stop();
            Verificator = null;
        }

        #endregion

        #region EventHandler Members

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            HuellaLog.Write("OnComplete() - Sample recibido, ReaderSN=" + ReaderSerialNumber);
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            HuellaLog.Write("OnFingerGone()");
            ActivarSemaforo(controlcolor ? 3 : 1);
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            HuellaLog.Write("OnFingerTouch() - ReaderSN=" + ReaderSerialNumber);
            ActivarSemaforo(2);
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            HuellaLog.Write("OnReaderConnect() - ReaderSN=" + ReaderSerialNumber);
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
            HuellaLog.Write("OnReaderDisconnect() - ReaderSN=" + ReaderSerialNumber);
            _lectorFisico = false;
            _lectorDisponible = false;

            this.Invoke(new Function(delegate ()
            {
                CambiarModoSinLector();
            }));
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            HuellaLog.Write("OnSampleQuality() - Feedback=" + CaptureFeedback);
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
                        : Color.FromArgb(201, 168, 76);
                }));
            }
            else
            {
                etiquetaNombre.Text = sBienVenidaAux;
                etiquetaES.Text = sCadenaEntraSale;
                etiquetaES.ForeColor = (sCadenaEntraSale != null && sCadenaEntraSale.Contains("SALIDA"))
                    ? Color.OrangeRed
                    : Color.FromArgb(201, 168, 76);
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

        // Cache de avatares en memoria para no re-consultar la BD
        private System.Collections.Generic.Dictionary<int, Image> _cacheAvatares
            = new System.Collections.Generic.Dictionary<int, Image>();
        // null = sin foto, se usa para saber si ya consultamos
        private System.Collections.Generic.HashSet<int> _sinFoto
            = new System.Collections.Generic.HashSet<int>();

        private void AplicarFormaAvatar()
        {
            // Forma circular para el avatar foto
            var pathAvatar = new System.Drawing.Drawing2D.GraphicsPath();
            pathAvatar.AddEllipse(0, 0, picAvatar.Width, picAvatar.Height);
            picAvatar.Region = new Region(pathAvatar);

            // Forma circular para las iniciales
            var pathIniciales = new System.Drawing.Drawing2D.GraphicsPath();
            pathIniciales.AddEllipse(0, 0, lblIniciales.Width, lblIniciales.Height);
            lblIniciales.Region = new Region(pathIniciales);
        }

        private string ObtenerIniciales(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return "?";
            // Remover "Bienvenido " / "Hasta Luego " si viene con prefijo
            nombre = nombre.Replace("Bienvenido ", "").Replace("Hasta Luego ", "").Trim();
            var partes = nombre.Split(new[] { ' ', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length >= 2)
                return (partes[0][0].ToString() + partes[1][0].ToString()).ToUpper();
            if (partes.Length == 1 && partes[0].Length >= 2)
                return partes[0].Substring(0, 2).ToUpper();
            return nombre.Length > 0 ? nombre[0].ToString().ToUpper() : "?";
        }

        private void MostrarAvatar(int nLegajoID, string nombre)
        {
            // Mostrar iniciales inmediatamente como placeholder
            lblIniciales.Text = ObtenerIniciales(nombre);
            lblIniciales.Visible = true;
            picAvatar.Visible = false;
            PosicionarConAvatar();

            // Si ya tenemos la foto en cache, mostrar directo
            if (_cacheAvatares.ContainsKey(nLegajoID))
            {
                picAvatar.Image = _cacheAvatares[nLegajoID];
                picAvatar.Visible = true;
                lblIniciales.Visible = false;
                return;
            }

            // Si ya sabemos que no tiene foto, quedarse con iniciales
            if (_sinFoto.Contains(nLegajoID))
                return;

            // Cargar foto async desde la BD
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                        "SELECT Foto FROM Legajo WHERE Id = " + nLegajoID);
                    if (dt.Rows.Count > 0 && dt.Rows[0]["Foto"] != System.DBNull.Value)
                    {
                        byte[] fotoBytes = (byte[])dt.Rows[0]["Foto"];
                        if (fotoBytes != null && fotoBytes.Length > 0)
                        {
                            var ms = new MemoryStream(fotoBytes);
                            var img = Image.FromStream(ms);
                            _cacheAvatares[nLegajoID] = img;

                            if (!this.IsDisposed)
                            {
                                this.Invoke(new Function(delegate ()
                                {
                                    picAvatar.Image = img;
                                    picAvatar.Visible = true;
                                    lblIniciales.Visible = false;
                                }));
                            }
                            return;
                        }
                    }
                    // No tiene foto
                    _sinFoto.Add(nLegajoID);
                }
                catch
                {
                    // Error de BD — quedarse con iniciales, no bloquear
                    _sinFoto.Add(nLegajoID);
                }
            });
        }

        private void OcultarAvatar()
        {
            picAvatar.Visible = false;
            lblIniciales.Visible = false;
            etiquetaNombre.Location = new Point(5, 538);
            etiquetaNombre.Size = new Size(610, 36);
            etiquetaNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        }

        private void PosicionarConAvatar()
        {
            etiquetaNombre.Location = new Point(148, 538);
            etiquetaNombre.Size = new Size(460, 58);
            etiquetaNombre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }

        private void AplicarFormaLector()
        {
            // Ovalo para el PictureHuella (simula sensor del lector)
            var pathHuella = new System.Drawing.Drawing2D.GraphicsPath();
            pathHuella.AddEllipse(0, 0, PictureHuella.Width, PictureHuella.Height);
            PictureHuella.Region = new Region(pathHuella);

            // Forma redondeada para la carcasa del lector
            var pathLector = new System.Drawing.Drawing2D.GraphicsPath();
            int r = 20; // radio de esquinas
            var rect = new Rectangle(0, 0, panelLector.Width, panelLector.Height);
            pathLector.AddArc(rect.X, rect.Y, r * 2, r * 2, 180, 90);
            pathLector.AddArc(rect.Right - r * 2, rect.Y, r * 2, r * 2, 270, 90);
            pathLector.AddArc(rect.Right - r * 2, rect.Bottom - r * 2, r * 2, r * 2, 0, 90);
            pathLector.AddArc(rect.X, rect.Bottom - r * 2, r * 2, r * 2, 90, 90);
            pathLector.CloseFigure();
            panelLector.Region = new Region(pathLector);
        }

        private static readonly Color SemaforoApagado = Color.FromArgb(40, 44, 60);
        private static readonly Color SemaforoRojo = Color.FromArgb(220, 50, 50);
        private static readonly Color SemaforoAmarillo = Color.FromArgb(232, 201, 122);
        private static readonly Color SemaforoVerde = Color.FromArgb(50, 180, 80);

        private void ActivarSemaforoInternal(int pos)
        {
            // Resetear todos los circulos a apagado
            lblSemaforoRojo.ForeColor = SemaforoApagado;
            lblSemaforoAmarillo.ForeColor = SemaforoApagado;
            lblSemaforoVerde.ForeColor = SemaforoApagado;
            lblLectorLed.BackColor = SemaforoApagado;

            switch (pos)
            {
                case 1: // Error - rojo
                    lblSemaforoRojo.ForeColor = SemaforoRojo;
                    lblLectorLed.BackColor = SemaforoRojo;
                    lblEstado.ForeColor = SemaforoRojo;
                    lblEstado.Text = _modoActual == ModoFichada.Huella
                        ? "Huella no reconocida"
                        : "Error en fichada";
                    break;
                case 2: // Verificando - amarillo
                    lblSemaforoAmarillo.ForeColor = SemaforoAmarillo;
                    lblLectorLed.BackColor = SemaforoAmarillo;
                    lblEstado.ForeColor = Color.FromArgb(201, 168, 76);
                    lblEstado.Text = "Verificando...";
                    break;
                case 3: // Exito - verde
                    lblSemaforoVerde.ForeColor = SemaforoVerde;
                    lblLectorLed.BackColor = SemaforoVerde;
                    lblEstado.ForeColor = SemaforoVerde;
                    lblEstado.Text = "\u00A1Registro exitoso!";
                    break;
                default: // Inactivo - todo apagado
                    lblEstado.ForeColor = Color.FromArgb(100, 100, 110);
                    lblEstado.Text = string.Empty;
                    break;
            }
        }

        private void DiagnosticoBD()
        {
            try
            {
                int empresaId = Global.Datos.TenantContext.EmpresaId;
                string connName = System.Configuration.ConfigurationManager.ConnectionStrings["Local"]?.ConnectionString ?? "NULL";
                // Extraer solo server y database del connection string para no mostrar password
                string serverInfo = "?";
                string dbInfo = "?";
                try
                {
                    var csb = new System.Data.SqlClient.SqlConnectionStringBuilder(connName);
                    serverInfo = csb.DataSource;
                    dbInfo = csb.InitialCatalog;
                }
                catch { }

                var dtLegajos = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                    "SELECT COUNT(*) AS cnt FROM Legajo WHERE EmpresaId = " + empresaId + " AND IsActive = 1");
                int legajos = dtLegajos.Rows.Count > 0 ? Convert.ToInt32(dtLegajos.Rows[0][0]) : 0;

                var dtHuellas = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                    "SELECT COUNT(*) AS cnt FROM LegajoHuella lh INNER JOIN Legajo l ON lh.LegajoId = l.Id WHERE l.EmpresaId = " + empresaId);
                int huellas = dtHuellas.Rows.Count > 0 ? Convert.ToInt32(dtHuellas.Rows[0][0]) : 0;

                string diag = string.Format("BD: {0}/{1} | EmpresaId={2} | Legajos={3} | Huellas={4}",
                    serverInfo, dbInfo, empresaId, legajos, huellas);
                HuellaLog.Write("DIAG: " + diag);

                // Mostrar diagnostico temporal en titulo
                this.Text += " [" + diag + "]";
            }
            catch (Exception ex)
            {
                HuellaLog.Write("DIAG ERROR: " + ex.Message);
                this.Text += " [BD ERROR: " + ex.Message + "]";
            }
        }

        private void ConfiguracionLocal()
        {
            oTerminal.sTerminalID = Environment.MachineName.ToString();
            oTerminal.Inicializar();

            if (!oTerminal.Existe)
            {
                // Auto-registrar terminal con la primera sucursal de la empresa
                try
                {
                    int empresaId = Global.Datos.TenantContext.EmpresaId;
                    var dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                        "SELECT TOP 1 Id FROM Sucursal WHERE EmpresaId = " + empresaId + " ORDER BY Id");
                    if (dt.Rows.Count > 0)
                    {
                        string sucursalId = dt.Rows[0]["Id"].ToString();
                        oTerminal.sDescripcion = Environment.MachineName;
                        oTerminal.sSucursalID.sSucursalID = sucursalId;
                        oTerminal.Actualizar();
                        oTerminal.Inicializar(); // recargar con datos completos
                    }
                }
                catch { /* si falla el auto-registro, sigue sin terminal */ }
            }

            if (oTerminal.Existe)
            {
                etiquetaSucursal.Text = oTerminal.sSucursalID.sSucursalID + "  -  " +
                    oTerminal.sSucursalID.sDescripcion.Trim() + "\n" +
                    "Terminal: " + oTerminal.sTerminalID;
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
            OcultarAvatar();
            ActivarSemaforo(4);
            PictureHuella.Image = null;
            controlcolor = false;
        }

        private void timerHora_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss tt");
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void PictureHuella_Click(object sender, EventArgs e)
        {
        }

        private void CargarLogos()
        {
            try
            {
                // Logo de empresa desde DigitalPlusAdmin
                var empresa = Acceso.Clases.Datos.Generales.EmpresaInfoService.ObtenerEmpresa();
                if (empresa != null && empresa.Logo != null && empresa.Logo.Length > 0)
                {
                    var ms = new MemoryStream(empresa.Logo);
                    pictureBox1.Image = Image.FromStream(ms);

                    if (!string.IsNullOrEmpty(empresa.Nombre))
                    {
                        lblEmpresa.Text = empresa.Nombre;
                        this.Text = "Digital One v" + Global.Datos.BuildInfo.FullVersion + " - " + empresa.Nombre;
                    }
                }
            }
            catch { }

            try
            {
                // Logo IntegraIA (recurso embebido)
                byte[] integraBytes = Acceso.Clases.Datos.Generales.EmpresaInfoService.ObtenerLogoIntegraIA();
                if (integraBytes != null && integraBytes.Length > 0)
                {
                    var ms = new MemoryStream(integraBytes);
                    picLogoIntegraIA.Image = Image.FromStream(ms);
                }
            }
            catch { }
        }
    }
}
