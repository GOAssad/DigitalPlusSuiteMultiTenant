using Acceso.Clases.Datos.Generales;
using Acceso.Clases.Datos.RRHH;
using AForge.Video.DirectShow;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;




namespace Acceso.RRHH
{
    public partial class FrmRRHHLegajos : Acceso.FrmBaseFormulario
    {
        private readonly RRHHLegajos olegajo = new Acceso.Clases.Datos.RRHH.RRHHLegajos();
        // oTurno y oListaTurnos removed (turnos tab removed, read-only mode)
        private readonly RRHHFichadasDao oAusencias = new RRHHFichadasDao();

        private AppData Data;                   // keeps application-wide data


        //Usado para los turnos
        private int dragRow = -1;
        private Label dragLabel = null;

        // ═══════════════════════════════════════════════════════
        // PANEL FOTO — campos (UI construida en InicializarPanelFoto)
        // ═══════════════════════════════════════════════════════
        private FilterInfoCollection _camaras;
        private VideoCaptureDevice _camDevice;
        private readonly object _frameLock = new object();
        private volatile bool _camCerrando;
        private volatile bool _camActiva;
        private volatile bool _primerFrame;

        private enum EstadoFoto { SinFoto, Preview, ConFoto }
        private EstadoFoto _estadoFoto = EstadoFoto.SinFoto;

        // Controles creados programáticamente
        private PictureBox _picFoto;
        private ComboBox _cboCamara;
        private FlowLayoutPanel _toolbar;
        private Button _btnEncender, _btnCapturar, _btnSubir;
        private Button _btnAceptar, _btnRechazar;
        private Button _btnEliminar, _btnDescargar, _btnRotar, _btnEspejo;

        private readonly SaveFileDialog dialogxls = new SaveFileDialog
        {
            Filter = "XLSX Excel|*.xlsx",
            DefaultExt = "xlsx",
            InitialDirectory = @"c:\",
            Title = "Guardar Archivo Excel",
            CheckFileExists = false,
            CheckPathExists = true
        };

        private readonly SaveFileDialog dialogpdf = new SaveFileDialog
        {
            Filter = "PDF Adobe|*.pdf",
            DefaultExt = "pdf",
            InitialDirectory = @"c:\",
            Title = "Guardar Archivo Excel",
            CheckFileExists = false,
            CheckPathExists = true
        };


        public FrmRRHHLegajos()
        {
            InitializeComponent();
            configuraciongeneral();

        }

        public FrmRRHHLegajos(string legajo)
        {
            InitializeComponent();
            configuraciongeneral();

            controlEntidadLegajos.BuscarRegistroElegido(legajo);

            ActualizarTodo();

        }

        private Button btnPin;

        // Controles de la pestaña Móvil (v2)
        private TabPage PageMovil;
        private Label lblMovilEstado;
        private Label lblMovilDispositivo;
        private Label lblMovilPlataforma;
        private Label lblMovilRegistro;
        private Label lblMovilUltimoUso;
        private Button btnGenerarCodigo;
        private Button btnDesactivarDispositivo;
        private TextBox txtCodigoActivacion;
        private int _terminalMovilId;

        private void configuraciongeneral()
        {
            ConfiguracionGeneraldelFormulario();
            monthCalendar1.MinDate = DateTime.Today;
            AgregarBotonPin();
            AgregarPestanaMovil();

            // Layout dinámico 80/20 para controles de datos
            panelDatosLegajos.Resize += (s, e) => AjustarLayoutDatos();
            AjustarLayoutDatos();
        }

        private void AjustarLayoutDatos()
        {
            int panelW = panelDatosLegajos.ClientSize.Width;
            if (panelW < 100) return;

            int margen = 10;
            int gap = 10;
            int zonaEntidades = (int)(panelW * 0.80);
            int anchoControl = (zonaEntidades - margen * 2 - gap) / 2;
            int altoControl = 86;

            // Fila 1: Horario | Sucursal
            controlEntidadHorario.Location = new Point(margen, 5);
            controlEntidadHorario.Size = new Size(anchoControl, altoControl);

            controlEntidadSucursal.Location = new Point(margen + anchoControl + gap, 5);
            controlEntidadSucursal.Size = new Size(anchoControl, altoControl);

            // Fila 2: Sector | Categoria
            controlEntidadSimpleUbicaciones.Location = new Point(margen, 95);
            controlEntidadSimpleUbicaciones.Size = new Size(anchoControl, altoControl);

            controlEntidadSimpleCategorias.Location = new Point(margen + anchoControl + gap, 95);
            controlEntidadSimpleCategorias.Size = new Size(anchoControl, altoControl);

            // Zona 20% restante: PIN arriba, Activo abajo
            int zona20 = panelW - zonaEntidades;
            int xCentro20 = zonaEntidades + (zona20 - chkActivo.Width) / 2;

            // PIN: primera fila del 20%
            if (btnPin != null)
            {
                int xPin = zonaEntidades + (zona20 - btnPin.Width) / 2;
                btnPin.Location = new Point(xPin, 30);
            }

            // Activo: segunda fila del 20%
            chkActivo.Location = new Point(xCentro20, 120);

            // lblInactivo debajo del chkActivo
            lblInactivo.Location = new Point(xCentro20, chkActivo.Bottom + 4);
        }

        private void AgregarBotonPin()
        {
            btnPin = new Button
            {
                Text = "PIN",
                Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 35),
                Enabled = false
            };
            btnPin.FlatAppearance.BorderSize = 0;
            btnPin.Click += BtnPin_Click;

            // Agregar al panel de datos, arriba del botón Activo
            panelDatosLegajos.Controls.Add(btnPin);
            btnPin.BringToFront();
        }

        private void BtnPin_Click(object sender, EventArgs e)
        {
            if (!olegajo.Existe) return;

            string legajoId = olegajo.sLegajoID;
            string nombre = (olegajo.sApellido + ", " + olegajo.sNombre).Trim();

            var pinHelper = new RRHHLegajosPin();
            pinHelper.CargarLegajo(legajoId);

            string[] opciones;
            if (pinHelper.HasPin)
                opciones = new[] { "Resetear PIN (forzar cambio)", "Eliminar PIN", "Cancelar" };
            else
                opciones = new[] { "Asignar PIN", "Cancelar" };

            string msg = pinHelper.HasPin
                ? "El legajo " + legajoId + " tiene PIN asignado.\n\n" +
                  "[Sí] = Resetear PIN (forzar cambio)\n" +
                  "[No] = Eliminar PIN\n" +
                  "[Cancelar] = No hacer nada"
                : "El legajo " + legajoId + " no tiene PIN.\n¿Desea asignar uno?";

            if (!pinHelper.HasPin)
            {
                var result = MessageBox.Show(msg, "PIN - " + nombre,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Generar PIN temporal y forzar cambio
                    string pinTemp = new Random().Next(1000, 9999).ToString();
                    if (pinHelper.CambiarPin(legajoId, pinTemp))
                    {
                        // Forzar cambio en proximo uso
                        Global.Datos.SQLServer.EjecutarSPsinRespuesta(
                            "UPDATE lp SET lp.PinMustChange = 1 FROM LegajoPin lp INNER JOIN Legajo l ON lp.LegajoId = l.Id WHERE l.NumeroLegajo = '" + legajoId.Replace("'", "''") + "' AND l.EmpresaId = " + Global.Datos.TenantContext.EmpresaId, false);
                        MessageBox.Show("PIN temporal asignado: " + pinTemp +
                            "\n\nEl empleado debera cambiarlo en su primer uso.",
                            "PIN asignado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                var result = MessageBox.Show(msg, "PIN - " + nombre,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Forzar cambio
                    Global.Datos.SQLServer.EjecutarSPsinRespuesta(
                        "UPDATE lp SET lp.PinMustChange = 1 FROM LegajoPin lp INNER JOIN Legajo l ON lp.LegajoId = l.Id WHERE l.NumeroLegajo = '" + legajoId.Replace("'", "''") + "' AND l.EmpresaId = " + Global.Datos.TenantContext.EmpresaId, false);
                    MessageBox.Show("Se marco cambio obligatorio de PIN para este legajo.",
                        "PIN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == DialogResult.No)
                {
                    // Eliminar PIN
                    if (MessageBox.Show("¿Eliminar el PIN del legajo " + legajoId + "?",
                        "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        Global.Datos.SQLServer.EjecutarSPsinRespuesta(
                            "DELETE p FROM LegajoPin p INNER JOIN Legajo l ON p.LegajoId = l.Id WHERE l.NumeroLegajo = '" + legajoId + "' AND l.EmpresaId = " + Global.Datos.TenantContext.EmpresaId, false);
                        MessageBox.Show("PIN eliminado.", "PIN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        private void ConfiguracionGeneraldelFormulario()
        {
            Data = new AppData();                               // Create the application data object
            Data.OnChange += delegate { ExchangeData(false); }; // Track data changes to keep the form synchronized
            //Enroller = new EnrollmentForm(Data);
            ExchangeData(false);

            controlEntidadLegajos.textoCodigo.Focus();


        }
        private void FrmRRHHLegajosUareU_Load(object sender, EventArgs e)
        {

            InicializarPanelFoto();

            // Tabs de Reportes, Domicilios y Turnos removidos en Designer (modo solo huellas+foto)

            //deshabilito todos los controles hasta que se cargue un usuario
            HabilitarDeshabilitarControles();

        }

        private void HabilitarDeshabilitarControles()
        {
            bool activar = olegajo.Existe;
            timer1.Enabled = olegajo.Existe;
            controlEntidadSucursal.Enabled = true;
            if (btnPin != null) btnPin.Enabled = activar;
        }


        // Simple dialog data exchange (DDX) implementation.
        public void ExchangeData(bool read)
        {
            if (read)
            {   // read values from the form's controls to the data object
                Data.EnrolledFingersMask = EnrollmentControl.EnrolledFingerMask;
                Data.MaxEnrollFingerCount = EnrollmentControl.MaxEnrollFingerCount;
                Data.Update();
            }
            else
            {   // read values from the data object to the form's controls
                try
                {
                    EnrollmentControl.EnrolledFingerMask = Data.EnrolledFingersMask;
                    EnrollmentControl.MaxEnrollFingerCount = Data.MaxEnrollFingerCount;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    // throw;
                }
            }
        }


        public override void ActualizarTodo()
        {
            base.ActualizarTodo();


            lblBuscando.Visible = true;
            lblBuscando.Text = "Buscando...";
            lblBuscando.Refresh();

            // oparent.lblMensaje.Text = string.Empty;

            DetenerCamara();

            olegajo.sLegajoID = controlEntidadLegajos.textoCodigo.Text.Trim();
            olegajo.Inicializar();

            if (!olegajo.Existe)
            {
                //si no existe limpio los campos de tipo controlentidad, el resto se blanquea desde las clases
                controlEntidadHorario.LimpiarCamposForzoso();

                controlEntidadSucursal.LimpiarCamposForzoso();
                controlEntidadSimpleUbicaciones.LimpiarCamposForzoso();
                controlEntidadSimpleCategorias.LimpiarCamposForzoso();
            }


            HabilitarDeshabilitarControles();


            textoEtiquetaApellido.Valor = olegajo.sApellido;
            textoEtiquetaNombre.Valor = olegajo.sNombre;

            controlEntidadSimpleUbicaciones.BuscarRegistroElegido(olegajo.nSector);
            controlEntidadSimpleCategorias.BuscarRegistroElegido(olegajo.nCategoria);



            chkSeguimiento.CheckState = CheckState.Unchecked;
            chkActivo.CheckState = CheckState.Unchecked;

            if (olegajo.lSeguimiento)
            {
                chkSeguimiento.CheckState = CheckState.Checked;
            }

            chkSeguimiento.Refresh();


            if (olegajo.lActivo)
            {
                chkActivo.CheckState = CheckState.Checked;
            }

            controlEntidadHorario.BuscarRegistroElegido(olegajo.sHorarioID);

            controlEntidadSucursal.textoCodigo.Text = olegajo.sSucursalID;
            controlEntidadSucursal.BuscarRegistroElegido(olegajo.sSucursalID);


            actualizarDedos();
            //textoPassword1.Text = oUsuario.sPassword;
            //textoPassword2.Text = oUsuario.sPasswordCtr;


            //check de seguimiento
            chkSeguimiento.Checked = olegajo.lSeguimiento;



            DetenerCamara();
            _picFoto.Image = null;

            // Foto
            if (olegajo.iFoto != null)
            {
                _picFoto.Image = Image.FromStream(new System.IO.MemoryStream(olegajo.iFoto));
                SetEstado(EstadoFoto.ConFoto);
            }
            else
            {
                SetEstado(EstadoFoto.SinFoto);
            }


            // Domicilios y Turnos removidos (modo solo huellas+foto)


            // Sucursal es read-only, no se controla permisos de edicion

                lblBuscando.Visible = false;

            // Terminal Móvil (v2)
            CargarDatosMovil();

        }

        public override bool ClickGuardar()
        {


            if (controlEntidadLegajos.textoCodigo.Text == string.Empty)
            {
                // oparent.lblMensaje.Text = "El codigo de legajo esta vacio";
                return false;
            }
            if (!controlEntidadLegajos.Controlarsihayvalores())
            {
                // oparent.lblMensaje.Text = "Hay datos que no pudieron ser validados " + "\n\r" + "Por favor revise los controles y vuelva a intentarlo";

                return false;
            }

            if (!olegajo.Existe)
            {
                // Alta nueva: verificar limite de legajos de la licencia
                var ticket = Program.LicMgr?.CurrentTicket;
                if (ticket != null)
                {
                    int legajosActuales = Program.ContarLegajos();
                    if (legajosActuales >= ticket.MaxLegajos)
                    {
                        MessageBox.Show(
                            "No se pueden agregar mas legajos.\n" +
                            "Su licencia permite hasta " + ticket.MaxLegajos + " legajos y actualmente tiene " + legajosActuales + ".\n\n" +
                            "Contacte a su proveedor para ampliar su plan.",
                            "Limite de licencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }


            if (!base.ClickGuardar())
            {
                return false;
            }

            lblGuardando.Visible = true;
            lblGuardando.Refresh();

            olegajo.sNombre = textoEtiquetaNombre.Valor;
            olegajo.sApellido = textoEtiquetaApellido.Valor;
            olegajo.nSector = int.Parse(controlEntidadSimpleUbicaciones.textoCodigo.Text.ToString().Trim());
            olegajo.nCategoria = int.Parse(controlEntidadSimpleCategorias.textoCodigo.Text.ToString().Trim());
            olegajo.sHorarioID = controlEntidadHorario.textoCodigo.Text.Trim();
            olegajo.sSucursalID = controlEntidadSucursal.textoCodigo.Text.Trim();
            olegajo.lActivo = chkActivo.Checked;
            olegajo.lSeguimiento = chkSeguimiento.Checked;
            olegajo.iFoto = null;

            if (_picFoto.Image != null)
            {
                try
                {
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                    {
                        _picFoto.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        olegajo.iFoto = stream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Informar("No se pudo cargar la imagen, guarde el Legajo e intentelo nuevamente " + "\n\r" +
                        ex.Message);

                    /// si da error sigo de largo, 
                    /// es posible que sea porque guarda estando la camara activa sin haber tomado la foto
                }
            }


            if (olegajo.Actualizar())
            {
                // Domicilios y Turnos removidos (modo solo huellas+foto)

                bool lhuellas;
                lhuellas = GuardarHuellas();
                if (lhuellas)
                {
                    Informar("Registro agregado a la BD correctamente");
                    limpiarcampos();

                    lblGuardando.Visible = false;
                    return true;
                }
                else
                {
                    Informar("Algo no salio bien almacenando las huellas");

                    lblGuardando.Visible = false;
                    return false;
                }



            } // fin ----- if  oLegajo.Actualizar()



            else
            {
                MessageBox.Show(olegajo.sMensaje);
                lblGuardando.Visible = false;
                return false;
            }
        }

        private bool GuardarHuellas()
        {
            lblGuardando.Visible = true;
            try
            {

                byte[] streamHuella;
                Acceso.Clases.Datos.RRHH.RRHHLegajosHuellas empleado = new Clases.Datos.RRHH.RRHHLegajosHuellas();


                for (int i = 0; i < Data.Templates.Length; i++)
                {
                    if (Data.Templates[i] != null)
                    {
                        switch (i)
                        {
                            case 0: empleado.nFingerMask = 32; break;
                            case 1: empleado.nFingerMask = 64; break;
                            case 2: empleado.nFingerMask = 128; break;
                            case 3: empleado.nFingerMask = 256; break;
                            case 4: empleado.nFingerMask = 512; break;
                            case 5: empleado.nFingerMask = 16; break;
                            case 6: empleado.nFingerMask = 8; break;
                            case 7: empleado.nFingerMask = 4; break;
                            case 8: empleado.nFingerMask = 2; break;
                            case 9: empleado.nFingerMask = 1; break;


                            default: break;

                        }
                        streamHuella = Data.Templates[i].Bytes;

                        empleado.nLegajoID = olegajo.nLegajoID;
                        empleado.sLegajoID = olegajo.sLegajoID;
                        empleado.nDedo = i;
                        empleado.iHuella = streamHuella;
                        empleado.sLegajoNombre = olegajo.sApellido + ", " + olegajo.sNombre;

                        if (!empleado.Actualizar())
                        {
                            lblGuardando.Visible = false;
                            return false;

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

                lblGuardando.Visible = false;
                return false;
            }

            lblGuardando.Visible = false;
            return true;
        }

        public override void ClickEliminar()
        {
            base.ClickEliminar();
            if (drRespuesta == DialogResult.Yes)
            {
                if (!olegajo.eliminar())
                {
                    MessageBox.Show(olegajo.sMensaje);
                }
                else
                {
                    MessageBox.Show("Legajo Eliminado");
                    limpiarcampos();
                }
            }
        }

        private void limpiarcampos()
        {

            base.ClickNuevo();


            chkActivo.Checked = false;
            lblInactivo.Visible = false;


            DetenerCamara();
            if (_picFoto != null) _picFoto.Image = null;
            SetEstado(EstadoFoto.SinFoto);

            textoEtiquetaApellido.Text = string.Empty;
            textoEtiquetaNombre.Text = string.Empty;

            olegajo.QueDedos = 0;
            olegajo.Existe = false;
            actualizarDedos();

            DetenerCamara();
            olegajo.Existe = false;
            HabilitarDeshabilitarControles();

        }

        private void EnrollmentControl_OnCancelEnroll(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, string.Format("OnCancelEnroll: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnComplete(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, string.Format("OnComplete: {0}, finger {1}", ReaderSerialNumber, Finger));

            //Informar("Guardar aca");
        }

        private void EnrollmentControl_OnDelete(object Control, int Finger, ref DPFP.Gui.EventHandlerStatus Status)
        {
            if (Data.IsEventHandlerSucceeds)
            {

                Data.Templates[Finger - 1] = null;              // clear the finger template
                ExchangeData(true);                             // update other data
                ListEvents.Items.Insert(0, string.Format("OnDelete: finger {0}", Finger));
            }
            else
            {
                Status = DPFP.Gui.EventHandlerStatus.Failure;   // force a "failure" status
            }


            // borro de la base de datos (DigitalPlus: LegajosHuellas)
            Global.Datos.SQLServer.EjecutarSPsinRespuesta(
                "DELETE lh FROM LegajoHuella lh INNER JOIN Legajo l ON lh.LegajoId = l.Id WHERE l.NumeroLegajo = '" + olegajo.sLegajoID.Trim() +
                "' AND l.EmpresaId = " + Global.Datos.TenantContext.EmpresaId + " AND lh.DedoId = " + (Finger - 1).ToString().Trim(), false);
        }

        private void EnrollmentControl_OnEnroll(object Control, int Finger, DPFP.Template Template, ref DPFP.Gui.EventHandlerStatus Status)
        {
            if (Data.IsEventHandlerSucceeds)
            {
                Data.Templates[Finger - 1] = Template;          // store a finger template
                ExchangeData(true);                             // update other data


                ListEvents.Items.Insert(0, string.Format("OnEnroll: finger {0}", Finger));
            }
            else
            {
                Status = DPFP.Gui.EventHandlerStatus.Failure;   // force a "failure" status
            }
        }

        private void EnrollmentControl_OnFingerRemove(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, string.Format("OnFingerRemove: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnFingerTouch(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, string.Format("OnFingerTouch: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnReaderConnect(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, string.Format("OnReaderConnect: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnReaderDisconnect(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, string.Format("OnReaderDisconnect: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnSampleQuality(object Control, string ReaderSerialNumber, int Finger, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            ListEvents.Items.Insert(0, string.Format("OnSampleQuality: {0}, finger {1}, {2}", ReaderSerialNumber, Finger, CaptureFeedback));
        }

        private void EnrollmentControl_OnStartEnroll(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, string.Format("OnStartEnroll: {0}, finger {1}", ReaderSerialNumber, Finger));
        }
        private void actualizarDedos()
        {
            ListEvents.Items.Clear();
            Data.EnrolledFingersMask = olegajo.QueDedos;  //viene de FrmRegistrar
            Array.Clear(Data.Templates, 0, Data.Templates.Length);
            ExchangeData(false);
        }

        private void controlEntidadHorario_Load(object sender, EventArgs e)
        {

        }

        private void chkActivo_CheckedChanged(object sender, EventArgs e)
        {
            controlarActivo();
        }

        private void controlarActivo()
        {
            if (chkActivo.Checked)
            {
                lblInactivo.Visible = false;
            }
            else
            {
                lblInactivo.Visible = true;
            }
        }



        private void EnrollmentControl_Load(object sender, EventArgs e)
        {

        }


        // ═══════════════════════════════════════════════════════
        // PANEL FOTO — Construido programáticamente, patrón Fichador
        // ═══════════════════════════════════════════════════════

        private void InicializarPanelFoto()
        {
            var darkBg = Color.FromArgb(15, 20, 35);
            var goldColor = Color.FromArgb(201, 168, 76);
            var btnFont = new Font("Segoe UI", 8.5f, FontStyle.Regular);

            // Título
            var lblTitulo = new Label
            {
                Text = "Foto",
                Font = new Font("Segoe UI", 13f),
                ForeColor = goldColor,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Combo cámaras
            _cboCamara = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9f),
                Height = 28
            };

            // Detectar cámaras
            _camaras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in _camaras)
                _cboCamara.Items.Add(fi.Name);
            if (_cboCamara.Items.Count > 0)
                _cboCamara.SelectedIndex = 0;

            // Toolbar con FlowLayout
            _toolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 38,
                AutoSize = false,
                WrapContents = false,
                Padding = new Padding(2, 4, 2, 2),
                BackColor = Color.FromArgb(20, 26, 46)
            };

            Button MakeBtn(string text, string tooltip, Color fg, EventHandler click)
            {
                var b = new Button
                {
                    Text = text,
                    Font = btnFont,
                    ForeColor = fg,
                    BackColor = Color.FromArgb(30, 38, 60),
                    FlatStyle = FlatStyle.Flat,
                    Height = 30,
                    AutoSize = true,
                    Padding = new Padding(6, 0, 6, 0),
                    Margin = new Padding(2),
                    Cursor = Cursors.Hand,
                    Visible = false
                };
                b.FlatAppearance.BorderColor = Color.FromArgb(60, 70, 100);
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 55, 80);
                if (click != null) b.Click += click;
                toolTip1.SetToolTip(b, tooltip);
                return b;
            }

            // Estado SinFoto
            _btnEncender = MakeBtn("📷 Cámara", "Encender cámara", goldColor, OnEncenderCamara);
            _btnCapturar = MakeBtn("📸 Capturar", "Tomar foto", Color.White, OnCapturar);
            _btnSubir = MakeBtn("📁 Subir", "Subir imagen desde archivo", Color.LightGray, OnSubirImagen);

            // Estado Preview
            _btnAceptar = MakeBtn("✔ Aceptar", "Confirmar foto", Color.FromArgb(80, 200, 80), OnAceptar);
            _btnRechazar = MakeBtn("✖ Reintentar", "Descartar y volver", Color.FromArgb(230, 100, 100), OnRechazar);

            // Estado ConFoto
            _btnEliminar = MakeBtn("🗑 Eliminar", "Eliminar imagen", Color.FromArgb(230, 100, 100), OnEliminar);
            _btnDescargar = MakeBtn("💾 Descargar", "Guardar como archivo", Color.LightGray, OnDescargar);
            _btnRotar = MakeBtn("↻ Rotar", "Rotar 90°", Color.LightGray, OnRotar);
            _btnEspejo = MakeBtn("⇔ Espejo", "Voltear horizontal", Color.LightGray, OnEspejo);

            _toolbar.Controls.AddRange(new System.Windows.Forms.Control[] {
                _btnEncender, _btnCapturar, _btnSubir,
                _btnAceptar, _btnRechazar,
                _btnEliminar, _btnDescargar, _btnRotar, _btnEspejo
            });

            // PictureBox — exactamente como el Fichador
            _picFoto = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = darkBg,
                BorderStyle = BorderStyle.None
            };

            // Armar panelCamara (Dock order matters: último = Fill)
            panelCamara.Controls.Clear();
            panelCamara.BackColor = darkBg;
            panelCamara.Controls.Add(_picFoto);       // Fill
            panelCamara.Controls.Add(_toolbar);        // Top (3)
            panelCamara.Controls.Add(_cboCamara);      // Top (2)
            panelCamara.Controls.Add(lblTitulo);       // Top (1)

            SetEstado(EstadoFoto.SinFoto);
        }

        private void SetEstado(EstadoFoto estado)
        {
            _estadoFoto = estado;
            bool hayCam = _cboCamara != null && _cboCamara.Items.Count > 0;

            if (_btnEncender == null) return; // guard durante init

            _btnEncender.Visible = estado == EstadoFoto.SinFoto && hayCam;
            _btnCapturar.Visible = estado == EstadoFoto.SinFoto && _camActiva;
            _btnSubir.Visible = estado == EstadoFoto.SinFoto;

            _btnAceptar.Visible = estado == EstadoFoto.Preview;
            _btnRechazar.Visible = estado == EstadoFoto.Preview;

            _btnEliminar.Visible = estado == EstadoFoto.ConFoto;
            _btnDescargar.Visible = estado == EstadoFoto.ConFoto;
            _btnRotar.Visible = estado == EstadoFoto.ConFoto;
            _btnEspejo.Visible = estado == EstadoFoto.ConFoto;
        }

        // ── Cámara: Start / Stop / Frame ──

        private void DetenerCamara()
        {
            _camCerrando = true;
            _camActiva = false;
            if (_camDevice != null)
            {
                try { _camDevice.NewFrame -= OnNewFrame; } catch { }
                try
                {
                    if (_camDevice.IsRunning)
                    {
                        _camDevice.SignalToStop();
                        _camDevice.WaitForStop();
                    }
                }
                catch { }
                _camDevice = null;
            }
            _camCerrando = false;
        }

        private void OnNewFrame(object sender, AForge.Video.NewFrameEventArgs e)
        {
            if (_camCerrando) return;
            _primerFrame = true;
            try
            {
                var frame = (Bitmap)e.Frame.Clone();
                if (!_camCerrando && !IsDisposed && IsHandleCreated)
                {
                    BeginInvoke((Action)(() =>
                    {
                        if (_camCerrando || !_camActiva) { frame.Dispose(); return; }
                        var old = _picFoto.Image;
                        _picFoto.Image = frame;
                        old?.Dispose();
                    }));
                }
                else frame.Dispose();
            }
            catch { }
        }

        private void MostrarCamaraOcupada()
        {
            DetenerCamara();
            string proceso = DetectarProcesoConCamara();
            string msg = "Cámara no disponible\nEstá siendo utilizada por otra aplicación";
            if (!string.IsNullOrEmpty(proceso)) msg += ":\n" + proceso;

            int w = _picFoto.Width > 0 ? _picFoto.Width : 400;
            int h = _picFoto.Height > 0 ? _picFoto.Height : 300;
            var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(20, 25, 45));
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using (var f1 = new Font("Segoe UI", 10))
                    g.DrawString(msg, f1, new SolidBrush(Color.FromArgb(201, 168, 76)),
                        new RectangleF(10, 10, w - 20, h - 20), sf);
            }
            _picFoto.Image = bmp;
            SetEstado(EstadoFoto.SinFoto);
        }

        private static string DetectarProcesoConCamara()
        {
            try
            {
                foreach (var p in System.Diagnostics.Process.GetProcesses())
                {
                    try
                    {
                        string n = p.ProcessName.ToLowerInvariant();
                        if (n == "tentradaysalida" || n.Contains("fichador")) return "DigitalOne Fichador";
                    }
                    catch { }
                }
            }
            catch { }
            return null;
        }

        // ── Handlers de botones ──

        private void OnEncenderCamara(object sender, EventArgs e)
        {
            try
            {
                DetenerCamara();
                _picFoto.Image = null;
                _primerFrame = false;
                _camCerrando = false;

                _camDevice = new VideoCaptureDevice(_camaras[_cboCamara.SelectedIndex].MonikerString);
                _camDevice.NewFrame += OnNewFrame;
                _camDevice.VideoSourceError += (s, ev) =>
                {
                    if (!IsDisposed && IsHandleCreated)
                        BeginInvoke((Action)(() => MostrarCamaraOcupada()));
                };
                _camDevice.Start();

                // Timer: si en 1.5s no llega un frame, la cámara está ocupada
                var timer = new Timer { Interval = 1500 };
                timer.Tick += (s2, e2) =>
                {
                    timer.Stop(); timer.Dispose();
                    if (!_primerFrame && !_camCerrando)
                        MostrarCamaraOcupada();
                };
                timer.Start();

                _camActiva = true;
                SetEstado(EstadoFoto.SinFoto);
            }
            catch (Exception ex)
            {
                InformarError("Error al iniciar cámara: " + ex.Message);
            }
        }

        private void OnCapturar(object sender, EventArgs e)
        {
            if (_picFoto.Image == null) return;
            var img = new Bitmap(_picFoto.Image);
            DetenerCamara();
            _picFoto.Image = img;
            SetEstado(EstadoFoto.Preview);
        }

        private void OnSubirImagen(object sender, EventArgs e)
        {
            try
            {
                DetenerCamara();
                var dlg = new OpenFileDialog
                {
                    Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.webp;*.tiff|Todos|*.*",
                    Title = "Seleccionar foto"
                };
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _picFoto.Image = Image.FromFile(dlg.FileName);
                    SetEstado(EstadoFoto.Preview);
                }
            }
            catch (Exception ex) { InformarError(ex.Message); }
        }

        private void OnAceptar(object sender, EventArgs e)
        {
            SetEstado(EstadoFoto.ConFoto);
        }

        private void OnRechazar(object sender, EventArgs e)
        {
            _picFoto.Image = null;
            SetEstado(EstadoFoto.SinFoto);
            if (_cboCamara.Items.Count > 0)
                OnEncenderCamara(sender, e);
        }

        private void OnEliminar(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Eliminar la foto?", "DigitalOne", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _picFoto.Image = null;
                SetEstado(EstadoFoto.SinFoto);
            }
        }

        private void OnDescargar(object sender, EventArgs e)
        {
            if (_picFoto.Image == null) return;
            var sfd = new SaveFileDialog
            {
                Filter = "JPEG|*.jpg|PNG|*.png",
                DefaultExt = "jpg",
                FileName = $"Foto_{olegajo.sApellido}_{olegajo.sNombre}".Replace(" ", "_"),
                Title = "Guardar foto"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var fmt = sfd.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                    ? System.Drawing.Imaging.ImageFormat.Png
                    : System.Drawing.Imaging.ImageFormat.Jpeg;
                _picFoto.Image.Save(sfd.FileName, fmt);
            }
        }

        private void OnRotar(object sender, EventArgs e)
        {
            if (_picFoto.Image == null) return;
            _picFoto.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            _picFoto.Invalidate();
        }

        private void OnEspejo(object sender, EventArgs e)
        {
            if (_picFoto.Image == null) return;
            _picFoto.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            _picFoto.Invalidate();
        }

        private void FrmRRHHLegajosUareU_FormClosing(object sender, FormClosingEventArgs e)
        {
            DetenerCamara();
        }

        private void ApellidoyNombre(object sender, EventArgs e)
        {
            controlEntidadLegajos.textoDescripcion.Text =
               textoEtiquetaApellido.Valor.Trim() + ", " + textoEtiquetaNombre.Valor.Trim();
        }

        private void panelDatosLegajos_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            //paso como parametros el legajo, apellido y nombre para las etiquetas

            Reportes.frmFichadasHistorialLegajo oFrmRep = new
                Reportes.frmFichadasHistorialLegajo(olegajo.sLegajoID, olegajo.sApellido, olegajo.sNombre);
            oFrmRep.ShowDialog();
        }

        private void btnFaltas_Click(object sender, EventArgs e)
        {
            Reportes.frmAusenciasHistorialLegajos oFrmRep = new
               Reportes.frmAusenciasHistorialLegajos(olegajo.sLegajoID, olegajo.sApellido, olegajo.sNombre);
            oFrmRep.ShowDialog();
        }

        // EstatusBotonReportes y ActualizarSeguimiento removidos (tab reportes removido)

        private void timer1_Tick(object sender, EventArgs e)
        {
            // ActualizarSeguimiento removido (tab reportes removido)
        }

        private void PanelIndicadores_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTituloEficiencia_Click(object sender, EventArgs e)
        {

        }

        private void etiquetaTitulo2_Click(object sender, EventArgs e)
        {

        }

        private void lblEtiquetaTitulo_Click(object sender, EventArgs e)
        {

        }

        private void etiquetaTitulo4_Click(object sender, EventArgs e)
        {

        }

        private void lblEficiencia_Click(object sender, EventArgs e)
        {

        }

        private void btnRefrescarTurno_Click(object sender, EventArgs e)
        {


        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }

        private void fTurnoDesde_Validated(object sender, EventArgs e)
        {

        }

        private void fTurnoDesde_ValueChanged(object sender, EventArgs e)
        {
        }

        private void fTurnoDesdeDes_ValueChanged(object sender, EventArgs e)
        {

        }

        private void fTurnoHasta_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnBorrarDia_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {


        }

        public override void ClickNuevo()
        {
            base.ClickNuevo();
            limpiarcampos();

            controlEntidadLegajos.textoCodigo.Focus();
        }

        private void armodecripcion()
        {
            string separador;
            separador = (textoEtiquetaNombre.Text.Length > 0 && textoEtiquetaApellido.Text.Length > 0) ? ", " : "";



            controlEntidadLegajos.textoDescripcion.Text = textoEtiquetaApellido.Text.Trim() +
               separador + textoEtiquetaNombre.Text.Trim();

        }

        private void textoEtiquetaNombre_Leave(object sender, EventArgs e)
        {

            armodecripcion();

            if (tabControl1.SelectedTab == PageLegajo)
            {
                controlEntidadHorario.textoCodigo.Focus();
            }
        }

        private void textoEtiquetaApellido_Leave(object sender, EventArgs e)
        {
            armodecripcion();
        }



        // llenarCalenarioTurnos removido (tab turnos removido)
        // btnArrow_Click removido (tab turnos removido)

        // dgFechasTurno events, PageTurnos_Click, btnBorrarTurnos_Click removidos (tab turnos removido)

        // trackNumerico1_ScrollChanged, FiltroGridFechasTurnos, dgFechasTurno_CellContentClick removidos



        private bool SucursalPermitida()
        {
            return true;
        }

        #region Pestaña Móvil (v2)

        private void AgregarPestanaMovil()
        {
            PageMovil = new TabPage
            {
                Name = "PageMovil",
                Text = "Móvil",
                Padding = new Padding(15),
                UseVisualStyleBackColor = true
            };

            var panelMovil = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            var font = new Font("Segoe UI", 11F);
            var fontBold = new Font("Segoe UI", 11F, FontStyle.Bold);
            var fontTitle = new Font("Segoe UI", 14F, FontStyle.Bold);
            int y = 10;

            var lblTitulo = new Label
            {
                Text = "Terminal Móvil",
                Font = fontTitle,
                ForeColor = Color.SteelBlue,
                Location = new Point(10, y),
                AutoSize = true
            };
            panelMovil.Controls.Add(lblTitulo);
            y += 40;

            lblMovilEstado = new Label { Font = fontBold, Location = new Point(10, y), AutoSize = true };
            panelMovil.Controls.Add(lblMovilEstado);
            y += 30;

            panelMovil.Controls.Add(new Label { Text = "Dispositivo:", Font = fontBold, Location = new Point(10, y), AutoSize = true });
            lblMovilDispositivo = new Label { Font = font, Location = new Point(160, y), AutoSize = true };
            panelMovil.Controls.Add(lblMovilDispositivo);
            y += 28;

            panelMovil.Controls.Add(new Label { Text = "Plataforma:", Font = fontBold, Location = new Point(10, y), AutoSize = true });
            lblMovilPlataforma = new Label { Font = font, Location = new Point(160, y), AutoSize = true };
            panelMovil.Controls.Add(lblMovilPlataforma);
            y += 28;

            panelMovil.Controls.Add(new Label { Text = "Registrado:", Font = fontBold, Location = new Point(10, y), AutoSize = true });
            lblMovilRegistro = new Label { Font = font, Location = new Point(160, y), AutoSize = true };
            panelMovil.Controls.Add(lblMovilRegistro);
            y += 28;

            panelMovil.Controls.Add(new Label { Text = "Último uso:", Font = fontBold, Location = new Point(10, y), AutoSize = true });
            lblMovilUltimoUso = new Label { Font = font, Location = new Point(160, y), AutoSize = true };
            panelMovil.Controls.Add(lblMovilUltimoUso);
            y += 45;

            btnDesactivarDispositivo = new Button
            {
                Text = "Desactivar dispositivo",
                Font = font,
                Size = new Size(250, 38),
                Location = new Point(10, y),
                Enabled = false,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDesactivarDispositivo.Click += BtnDesactivarDispositivo_Click;
            panelMovil.Controls.Add(btnDesactivarDispositivo);
            y += 55;

            // Separador
            panelMovil.Controls.Add(new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Location = new Point(10, y),
                Size = new Size(500, 2)
            });
            y += 15;

            panelMovil.Controls.Add(new Label
            {
                Text = "Generar código de activación",
                Font = fontBold,
                Location = new Point(10, y),
                AutoSize = true
            });
            y += 30;

            btnGenerarCodigo = new Button
            {
                Text = "Generar código",
                Font = font,
                Size = new Size(200, 38),
                Location = new Point(10, y),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerarCodigo.Click += BtnGenerarCodigo_Click;
            panelMovil.Controls.Add(btnGenerarCodigo);

            txtCodigoActivacion = new TextBox
            {
                Font = new Font("Consolas", 16F, FontStyle.Bold),
                Location = new Point(230, y),
                Size = new Size(220, 38),
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Center,
                BackColor = Color.FromArgb(255, 243, 205)
            };
            panelMovil.Controls.Add(txtCodigoActivacion);

            PageMovil.Controls.Add(panelMovil);
            tabControl1.Controls.Add(PageMovil);
        }

        private void CargarDatosMovil()
        {
            _terminalMovilId = 0;
            lblMovilDispositivo.Text = "-";
            lblMovilPlataforma.Text = "-";
            lblMovilRegistro.Text = "-";
            lblMovilUltimoUso.Text = "-";
            txtCodigoActivacion.Text = "";
            btnDesactivarDispositivo.Enabled = false;

            if (!olegajo.Existe || string.IsNullOrEmpty(olegajo.sLegajoID))
            {
                lblMovilEstado.Text = "Sin legajo seleccionado";
                lblMovilEstado.ForeColor = Color.Gray;
                btnGenerarCodigo.Enabled = false;
                return;
            }

            btnGenerarCodigo.Enabled = true;

            try
            {
                int legajoId = Convert.ToInt32(olegajo.nLegajoID);
                int empresaId = Global.Datos.TenantContext.EmpresaId;
                DataTable dt = TerminalMovilDAL.ObtenerPorLegajo(legajoId, empresaId);

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    _terminalMovilId = Convert.ToInt32(row["Id"]);
                    lblMovilEstado.Text = "Dispositivo activo";
                    lblMovilEstado.ForeColor = Color.FromArgb(40, 167, 69);
                    lblMovilDispositivo.Text = row["Nombre"]?.ToString() ?? "(sin nombre)";
                    lblMovilPlataforma.Text = row["Plataforma"]?.ToString() ?? "-";
                    lblMovilRegistro.Text = row["FechaRegistro"] != DBNull.Value
                        ? Convert.ToDateTime(row["FechaRegistro"]).ToString("dd/MM/yyyy HH:mm") : "-";
                    lblMovilUltimoUso.Text = row["UltimoUso"] != DBNull.Value
                        ? Convert.ToDateTime(row["UltimoUso"]).ToString("dd/MM/yyyy HH:mm") : "Nunca";
                    btnDesactivarDispositivo.Enabled = true;
                }
                else
                {
                    lblMovilEstado.Text = "Sin dispositivo registrado";
                    lblMovilEstado.ForeColor = Color.FromArgb(108, 117, 125);
                }
            }
            catch (Exception ex)
            {
                lblMovilEstado.Text = "Error al consultar: " + ex.Message;
                lblMovilEstado.ForeColor = Color.Red;
            }
        }

        private void BtnGenerarCodigo_Click(object sender, EventArgs e)
        {
            if (!olegajo.Existe) return;

            try
            {
                int legajoId = Convert.ToInt32(olegajo.nLegajoID);
                int empresaId = Global.Datos.TenantContext.EmpresaId;
                string codigo = TerminalMovilDAL.GenerarCodigo(legajoId, empresaId);
                txtCodigoActivacion.Text = codigo;
                MessageBox.Show(
                    $"Código generado: {codigo}\n\nVálido por 24 horas.\nEl empleado debe ingresarlo en la app Digital One Mobile.",
                    "Código de Activación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar código: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDesactivarDispositivo_Click(object sender, EventArgs e)
        {
            if (_terminalMovilId == 0) return;

            var result = MessageBox.Show(
                "¿Desactivar el dispositivo móvil de este empleado?\n\nEl empleado deberá registrar un nuevo dispositivo con un código de activación.",
                "Confirmar desactivación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (TerminalMovilDAL.Desactivar(_terminalMovilId))
                {
                    CargarDatosMovil();
                    MessageBox.Show("Dispositivo desactivado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al desactivar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}

