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




namespace Acceso.RRHH
{
    public partial class FrmRRHHLegajos : Acceso.FrmBaseFormulario
    {
        private readonly RRHHLegajos olegajo = new Acceso.Clases.Datos.RRHH.RRHHLegajos();
        private readonly RRHHLegajosTurnos oTurno = new RRHHLegajosTurnos();
        private readonly List<RRHHLegajosTurnos> oListaTurnos = new List<RRHHLegajosTurnos>();
        private readonly RRHHFichadasDao oAusencias = new RRHHFichadasDao();

        private AppData Data;                   // keeps application-wide data


        //Usado para los turnos
        private int dragRow = -1;
        private Label dragLabel = null;

        /// <summary>
        /// Objetos de Aforge para captura de Video y Fotos desde WebCam
        /// </summary>
        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice videoCaptureDevice;

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

        private void configuraciongeneral()
        {
            ConfiguracionGeneraldelFormulario();
            monthCalendar1.MinDate = DateTime.Today;
            AgregarBotonPin();

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
                ? "El legajo " + legajoId + " tiene PIN asignado.\n¿Que desea hacer?"
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
                        "EXEC EscritorioLegajoPIN_ForzarCambio '" + legajoId + "'", false);
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

            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                cboCamera.Items.Add(filterInfo.Name);
            }

            if (cboCamera.Items.Count > 0)
            {
                cboCamera.SelectedIndex = 0;
                videoCaptureDevice = new VideoCaptureDevice();
                cboCamera.Enabled = true;
                btnInicioCamara.Enabled = true;
                videoSourcePlayer1.Visible = true;
            }
            else
            {
                cboCamera.Enabled = false;
                btnInicioCamara.Enabled = false;
                btnTomarFoto.Enabled = false;
                videoSourcePlayer1.Visible = false;
            }

            //Saco el control de Reportes 
            tabControl1.TabPages.Remove(tabControl1.TabPages["PageReportes"]);

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

            ApagoCamara();

            btnEliminarFoto.Enabled = false;

            olegajo.sLegajoID = controlEntidadLegajos.textoCodigo.Text.Trim();
            olegajo.Inicializar();

            if (!olegajo.Existe)
            {
                //si no existe limpio los campos de tipo controlentidad, el resto se blanquea desde las clases
                controlEntidadHorario.LimpiarCamposForzoso();
                ctrTurnosLegajos.LimpiarCamposForzoso();

                controlEntidadSucursal.LimpiarCamposForzoso();
                controlEntidadSimpleUbicaciones.LimpiarCamposForzoso();
                controlEntidadSimpleCategorias.LimpiarCamposForzoso();
                ctrEntidadPaises1.LimpiarCamposForzoso();
            }


            EstatusBotonReportes();
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
            ctrTurnosLegajos.BuscarRegistroElegido(olegajo.sHorarioID);

            controlEntidadSucursal.textoCodigo.Text = olegajo.sSucursalID;
            controlEntidadSucursal.BuscarRegistroElegido(olegajo.sSucursalID);


            actualizarDedos();
            //textoPassword1.Text = oUsuario.sPassword;
            //textoPassword2.Text = oUsuario.sPasswordCtr;


            //check de seguimiento
            chkSeguimiento.Checked = olegajo.lSeguimiento;



            picFotoCamara.Image = null;
            picFotoCamara.BackgroundImage = null;

            // Foto
            if (olegajo.iFoto != null)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(olegajo.iFoto);
                picFotoCamara.Image = Image.FromStream(ms);
                btnEliminarFoto.Enabled = true;
                ms.Close();
                ms.Dispose();
            }


            ///Gustavo 13/05/2021 - Domicilios
            ///Solapa de Domicilios



            textoCalle.Valor = olegajo.oDomicilio.sCalle;
            textoAltura.Valor = olegajo.oDomicilio.sAltura;
            textoBarrio.Valor = olegajo.oDomicilio.sBarrio;
            textoPiso.Valor = olegajo.oDomicilio.sPiso;
            textoLocalidad.Valor = olegajo.oDomicilio.sLocalidad;
            textoProvincia.Valor = olegajo.oDomicilio.sProvincia;
            // GRALPaises no existe en DigitalPlus — domicilios se ignoran


            ///--Fin Domicilios

            //Ahora los turnos cargados
            lblBuscando.Text = "Buscando turnos...";
            oTurno.sLegajoID = olegajo.sLegajoID;
            oTurno.Inicializar();
            llenarCalenarioTurnos();


            //18/05/2022
            // Me fijo si habilito los controles o no segun si es administrador o si no es, si tiene permiso para ver las fichadas del legajo
            if (ObjGlobal.oUsuario.nNivel == 1)
                controlEntidadSucursal.Enabled = true;
            else
            {
                if (ObjGlobal.oUsuario.PerteneceASucursal(controlEntidadSucursal.ValorCodigo, ObjGlobal.oUsuario.sUsuarioID))
                    controlEntidadSucursal.Enabled = true;
                else
                {
                    controlEntidadSucursal.Enabled = false;
                }
            }

                lblBuscando.Visible = false;

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

            if (picFotoCamara.Image != null)
            {
                try
                {
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                    {
                        picFotoCamara.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
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
                ///Gustavo 13/05/2021
                ///Domicilios

                olegajo.oDomicilio.sLegajoID = olegajo.sLegajoID;
                olegajo.oDomicilio.sAltura = textoAltura.Valor;
                olegajo.oDomicilio.sCalle = textoCalle.Valor;
                olegajo.oDomicilio.sLocalidad = textoLocalidad.Valor;
                olegajo.oDomicilio.sPiso = textoPiso.Valor;
                olegajo.oDomicilio.sProvincia = textoProvincia.Valor;
                olegajo.oDomicilio.sBarrio = textoBarrio.Valor;

                if (ctrEntidadPaises1.textoCodigo.Text == string.Empty)
                {
                    ctrEntidadPaises1.textoCodigo.Text = "0";
                }

                olegajo.oDomicilio.nPaisID = Convert.ToInt16(ctrEntidadPaises1.textoCodigo.Text);

                if (olegajo.oDomicilio.sCalle.Length + olegajo.oDomicilio.sAltura.Length > 0)
                {
                    if (!olegajo.oDomicilio.Actualizar())
                    {
                        InformarError("No se pudo almacenar el domicilio del Legajo " + "\n\r" +
                            "De todas formas se guardo el resto de la informacion " + "\n\r" +
                            olegajo.oDomicilio.sMensaje);

                        lblGuardando.Visible = false;

                        return false;
                    }

                    ///--Fin Domicilios
                }
                //Guardo los turnos
                oTurno.sLegajoID = olegajo.sLegajoID;
                oTurno.sHorarioID = olegajo.sHorarioID;

                //borro todo
                oTurno.BorrarTodos();

                foreach (DataGridViewRow item in dgFechasTurno.Rows)
                {
                    oTurno.dEntrada = Convert.ToDateTime(item.Cells[0].Value);
                    oTurno.dSalida = Convert.ToDateTime(item.Cells[0].Value);
                    oTurno.Actualizar();

                }


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


            picFotoCamara.Image = null;
            picFotoCamara.BackgroundImage = null;

            btnTomarFoto.Enabled = false;
            btnEliminarFoto.Enabled = false;

            textoEtiquetaApellido.Text = string.Empty;
            textoEtiquetaNombre.Text = string.Empty;

            olegajo.QueDedos = 0;
            olegajo.Existe = false;  //esto lo pongo para deshabilitar los botones de los reportes
            EstatusBotonReportes();
            actualizarDedos();

            ///Gustavo 13/05/2021
            ///
            textoAltura.Valor = string.Empty;
            textoCalle.Valor = string.Empty;
            textoBarrio.Valor = string.Empty;
            textoLocalidad.Valor = string.Empty;
            textoPiso.Valor = string.Empty;
            textoProvincia.Valor = string.Empty;
            txtCodPost.Valor = string.Empty;
            /// Fin 13/05/2021
            /// 

            listSeguimiento.DataSource = null;
            listSeguimiento.Items.Clear();

            ApagoCamara();
            olegajo.Existe = false;
            EstatusBotonReportes();
            HabilitarDeshabilitarControles();

            btnBorrarTurnos.PerformClick();

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


        private void ApagoCamara()
        {
            if (cboCamera.Items.Count > 0)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.Stop();
                }

                if (videoSourcePlayer1.IsRunning)
                {
                    videoSourcePlayer1.SignalToStop();
                }
            }
            picFotoCamara.Image = null;
            picFotoCamara.BringToFront();

        }


        private void btnImagen_Click(object sender, EventArgs e)
        {
            btnEliminarFoto.Enabled = false;
            try
            {

                ApagoCamara();

                videoSourcePlayer1.Visible = false;
                picFotoCamara.Visible = true;

                OpenFileDialog fo = new OpenFileDialog
                {
                    Filter = "jpg files (*.jpg) | *.jpg| png files (*.png) | *.png"
                };
                DialogResult rs = fo.ShowDialog();
                if (rs == DialogResult.OK)
                {
                    picFotoCamara.Image = Image.FromFile(fo.FileName);
                    btnEliminarFoto.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                InformarError(ex.Message);

            }
        }

        private void ApellidoyNombre(object sender, EventArgs e)
        {
            controlEntidadLegajos.textoDescripcion.Text =
               textoEtiquetaApellido.Valor.Trim() + ", " + textoEtiquetaNombre.Valor.Trim();
        }

        private void panelDatosLegajos_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnInicioCamara_Click(object sender, EventArgs e)
        {
            picFotoCamara.Visible = false;
            videoSourcePlayer1.Visible = true;
            btnTomarFoto.Enabled = true;

            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
            videoSourcePlayer1.VideoSource = videoCaptureDevice;
            videoCaptureDevice.Start();
        }

        private void btnTomarFoto_Click(object sender, EventArgs e)
        {
            Bitmap img = videoSourcePlayer1.GetCurrentVideoFrame();
            picFotoCamara.Image = img;
            picFotoCamara.Visible = true;

            DialogResult dialogResult = MessageBox.Show("Me Gusta como Salio la Foto", "DigitalOne", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                picFotoCamara.Image = null;
                picFotoCamara.Visible = false;
                return;
            }

            // Detener la cámara y hacer una copia independiente del bitmap
            ApagoCamara();
            picFotoCamara.Image = new Bitmap(img);
            btnEliminarFoto.Enabled = true;
            btnTomarFoto.Enabled = false;
        }

        private void FrmRRHHLegajosUareU_FormClosing(object sender, FormClosingEventArgs e)
        {

            ApagoCamara();

        }

        private void btnEliminarFoto_Click(object sender, EventArgs e)

        {
            DialogResult dialogResult = MessageBox.Show("Elimina la Foto?", "DigitalOne", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                picFotoCamara.Image = null;
                btnEliminarFoto.Enabled = false;
                btnTomarFoto.Enabled = false;
            }

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

        private void EstatusBotonReportes()
        {

            //ACA TIENE QUE IR EL MAXIMO ENTRE PRINCIPIO DE AÑO Y FECHA DE INGRESO DEL LEGAJO Y FECHA DE INICIO DEL SISTEMA
            //****************FALTA LA FECHA DE INGRESO EN EL LEGAJO ************************************************
            DateTime fromDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime fechaVar = Acceso.Clases.Datos.Generales.GRALVariablesGlobales.FechaInicio;

            if (fechaVar > DateTime.Now)
            {
                //InformarError("La Fecha de inicio no puede ser mayor a la fecha actual " + "\n\r" +
                //    "La fecha de inicio pasa a ser el primer dia del año");

                fechaVar = DateTime.Today.AddDays(-30);
            }


            fromDate = fromDate.CompareTo(fechaVar) > 0
                    ? fromDate : fechaVar;

            lblFecha.Text = "Desde Fecha: " + fromDate.ToShortDateString();

            DateTime ToDate = DateTime.Now;


            if (olegajo.Existe)
            {
                olegajo.CantidadAusencias(fromDate, ToDate);

                lblAusencias.Text = olegajo.Ausencias.ToString();
                lblIncidencias.Text = olegajo.Incidencias.ToString();
                lblPresencias.Text = olegajo.TotalFichadas.ToString();
                lblEficiencia.Text = olegajo.Eficiencia.ToString("#.##");

                ActualizarSeguimiento();

            }
            else
            {
                lblAusencias.Text = "0";
                lblIncidencias.Text = "0";
                lblPresencias.Text = "0";
                lblEficiencia.Text = "0.00";
            }
        }

        private void ActualizarSeguimiento()
        {
            listSeguimiento.DataSource = null;
            listSeguimiento.Items.Clear();

            if (olegajo.Existe)
            {
                //Ahora busco la actividad diaria
                listSeguimiento.DataSource = olegajo.ActividadDiaria();
                listSeguimiento.DisplayMember = "Accion";
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ActualizarSeguimiento();
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



        private void llenarCalenarioTurnos()
        {
            DateTime FechaAux;
            DataGridViewRow row;

            //Vaciar el dataGrid
            dgFechasTurno.Rows.Clear();
            //vaciar el calendario
            monthCalendar2.RemoveAllBoldedDates();

            string dia;

            foreach (DataRow item in oTurno.dtLegajosTurnos.Rows)
            {
                FechaAux = Convert.ToDateTime(item["dEntrada"]);
                monthCalendar2.AddBoldedDate(FechaAux);

                dia = FechaAux.ToString("dddd");

                row = (DataGridViewRow)dgFechasTurno.Rows[0].Clone();
                row.Cells[0].Value = FechaAux;
                row.Cells[1].Value = dia;
                dgFechasTurno.Rows.Add(row);
            }

            monthCalendar2.UpdateBoldedDates();


        }
        private void btnArrow_Click(object sender, EventArgs e)
        {
            DateTime inicio = monthCalendar1.SelectionStart;
            DateTime final = monthCalendar1.SelectionEnd;
            monthCalendar1.UpdateBoldedDates();

            DateTime FechaAux;
            DataGridViewRow row;

            string dia;
            //Selecciono en el Calendario Final las Fechas
            TimeSpan dias = final - inicio;
            for (int i = 0; i <= dias.Days; i++)
            {
                FechaAux = inicio.AddDays(i);

                //dia de la semana
                dia = FechaAux.ToString("dddd");


                bool exist
                    = dgFechasTurno.Rows.Cast<DataGridViewRow>().Any(r => Convert.ToDateTime(r.Cells[0].Value).ToShortDateString() == FechaAux.ToShortDateString());

                if (!exist)
                {
                    monthCalendar2.AddBoldedDate(FechaAux);
                    row = (DataGridViewRow)dgFechasTurno.Rows[0].Clone();
                    row.Cells[0].Value = FechaAux;
                    row.Cells[1].Value = dia;
                    dgFechasTurno.Rows.Add(row);
                }

            }
            monthCalendar2.UpdateBoldedDates();

        }

        private void dgFechasTurno_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            dragRow = e.RowIndex;

            if (dragLabel == null)
            {
                dragLabel = new Label();
            }

            {
                //MessageBox.Show("Borrar " + dgFechasTurno[e.ColumnIndex, dragRow].Value.ToString());
                dgFechasTurno.Rows.RemoveAt(dragRow);


                //Ahora paso lo que queda en la grid en el calendario
                DateTime FechaAux = new DateTime();
                monthCalendar2.RemoveAllBoldedDates();
                monthCalendar2.UpdateBoldedDates();

                foreach (DataGridViewRow row in dgFechasTurno.Rows)
                {

                    FechaAux = Convert.ToDateTime(row.Cells[0].Value);

                    monthCalendar2.AddBoldedDate(FechaAux);
                }

                monthCalendar2.UpdateBoldedDates();

            }
        }

        private void PageTurnos_Click(object sender, EventArgs e)
        {

        }

        private void btnBorrarTurnos_Click(object sender, EventArgs e)
        {
            monthCalendar1.RemoveAllBoldedDates();
            monthCalendar2.RemoveAllBoldedDates();

            monthCalendar2.UpdateBoldedDates();
            monthCalendar1.UpdateBoldedDates();

            dgFechasTurno.Rows.Clear();
        }

        private void trackNumerico1_ScrollChanged(object sender, EventArgs e)
        {
            // cuando muevo esto tengo que filtrar en el datagridview segun las fechas
            //FiltroGridFechasTurnos();
        }

        private void FiltroGridFechasTurnos()
        {

            DataView dv = oTurno.dtLegajosTurnos.DefaultView;

            //string cadenaFiltro;
            //string fromDate = DateTime.Today.AddDays(trackNumerico1.Valor).ToShortDateString();

            //cadenaFiltro = "dEntrada > #" + fromDate + "#";
            //dv.RowFilter = cadenaFiltro;

            dv.RowFilter = string.Format(CultureInfo.InvariantCulture.DateTimeFormat,
                     "dEntrada > #{0}#", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));


            dgFechasTurno.DataSource = dv.ToTable();


            //    if (txtFiltro.TextLength <= 1)
            //    {
            //        GetdataFromDatabase();
            //        CantidadRegistros();
            //        return;
            //    }

            //    lblBuscando.Visible = true;

            //    DataView dv = dt.DefaultView;
            //    dv.RowFilter = string.Format("sApellido like '%{0}%' or sNombre like '%{0}%'", txtFiltro.Text.Trim());
            //    dataGridView1.DataSource = dv.ToTable();
            //    CantidadRegistros();

            //    lblBuscando.Visible = false;


        }

        private void dgFechasTurno_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private bool SucursalPermitida()
        {
            return true;
        }
    }
}

