using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Acceso.Clases.Datos.RRHH;
using Acceso.Datos.Model;

namespace Acceso.uAreu
{
    public partial class EnrollmentForm : Form
    {
        private AppData Data;
        


        public int idLegajo;
        public int CantDedos;
        public string LegajoNombre;


        public EnrollmentForm(AppData data)
        {
            InitializeComponent();

            Data = data;                                        // Keep reference to the data
            ExchangeData(true);                                 // Init data with default control values;
            Data.OnChange += delegate { ExchangeData(false); }; // Track data changes to keep the form synchronized

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
                EnrollmentControl.EnrolledFingerMask = Data.EnrolledFingersMask;
                EnrollmentControl.MaxEnrollFingerCount = Data.MaxEnrollFingerCount;
                
            }
        }

        // event handling
        public void EnrollmentControl_OnEnroll(Object Control, int Finger, DPFP.Template Template, ref DPFP.Gui.EventHandlerStatus Status)
        {
            if (Data.IsEventHandlerSucceeds)
            {
                Data.Templates[Finger - 1] = Template;          // store a finger template
                ExchangeData(true);                             // update other data

                ListEvents.Items.Insert(0, String.Format("OnEnroll: finger {0}", Finger));
            }
            else
                Status = DPFP.Gui.EventHandlerStatus.Failure;   // force a "failure" status
        }

        public void EnrollmentControl_OnDelete(Object Control, int Finger, ref DPFP.Gui.EventHandlerStatus Status)
        {
            if (Data.IsEventHandlerSucceeds)
            {
                Data.Templates[Finger - 1] = null;              // clear the finger template
                ExchangeData(true);                             // update other data

                ListEvents.Items.Insert(0, String.Format("OnDelete: finger {0}", Finger));
            }
            else
                Status = DPFP.Gui.EventHandlerStatus.Failure;   // force a "failure" status

            // borro de la base de datos.
            Global.Datos.SQLServer.EjecutarSPsinRespuesta( "delete RRHHLegajosHuellas where " +
                "nLegajoID = '" + idLegajo.ToString().Trim() + "' and nDedo = '" + 
                (Finger - 1).ToString().Trim() + "'", false);
        }

        private void EnrollmentControl_OnCancelEnroll(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, String.Format("OnCancelEnroll: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnReaderConnect(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, String.Format("OnReaderConnect: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnReaderDisconnect(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, String.Format("OnReaderDisconnect: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnStartEnroll(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, String.Format("OnStartEnroll: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnFingerRemove(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, String.Format("OnFingerRemove: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnFingerTouch(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, String.Format("OnFingerTouch: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentControl_OnSampleQuality(object Control, string ReaderSerialNumber, int Finger, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            ListEvents.Items.Insert(0, String.Format("OnSampleQuality: {0}, finger {1}, {2}", ReaderSerialNumber, Finger, CaptureFeedback));
        }

        private void EnrollmentControl_OnComplete(object Control, string ReaderSerialNumber, int Finger)
        {
            ListEvents.Items.Insert(0, String.Format("OnComplete: {0}, finger {1}", ReaderSerialNumber, Finger));
        }

        private void EnrollmentForm_Load(object sender, EventArgs e)
        {
            this.ListEvents.Items.Clear();
            EnrollmentControl.EnrolledFingerMask = CantDedos; //viene de FrmRegistrar
            Array.Clear(Data.Templates, 0, Data.Templates.Length);
            
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {

        }


        //public override void ClickGuardar()
        //{
        //    base.ClickGuardar();
        //    olegajo.id = controlEntidadLegajos.IDValor;
        //    olegajo.sNombre = textoEtiquetaNombre.Valor;
        //    olegajo.sApellido = textoEtiquetaApellido.Valor;
        //    olegajo.nSector = comboDesplegable1.SelectedIndex;

        //    olegajohuella.nLegajoID = olegajo.id;
        //    olegajohuella.sLegajoNombre = olegajo.sApellido.Trim() + ", " + olegajo.sNombre.Trim();

        //    if (olegajo.Actualizar())
        //    {
        //        // aca tengo que actualiar las huellas
        //        // primero lleno las propiedades


        //        byte[] streamHuella;

        //        olegajohuella.nFingerMask = 0;

        //        for (int i = 0; i < Data.Templates.Count(); i++)
        //        {
        //            if (Data.Templates[i] != null)
        //            {
        //                switch (i)
        //                {
        //                    case 0: olegajohuella.nFingerMask = 32; break;
        //                    case 1: olegajohuella.nFingerMask = 64; break;
        //                    case 2: olegajohuella.nFingerMask = 128; break;
        //                    case 3: olegajohuella.nFingerMask = 256; break;
        //                    case 4: olegajohuella.nFingerMask = 512; break;
        //                    case 5: olegajohuella.nFingerMask = 1; break;
        //                    case 6: olegajohuella.nFingerMask = 2; break;
        //                    case 7: olegajohuella.nFingerMask = 4; break;
        //                    case 8: olegajohuella.nFingerMask = 8; break;
        //                    case 9: olegajohuella.nFingerMask = 16; break;
        //                    default: break;
        //                }


        //                //if (i != 0) //no tengo cargadas la huellas
        //                if (olegajohuella.nFingerMask != 0)

        //                {
        //                    streamHuella = Data.Templates[i].Bytes;
        //                    olegajohuella.nDedo = i;
        //                    olegajohuella.iHuella = streamHuella;
        //                    olegajohuella.Actualizar();
        //                }
        //            }
        //            olegajohuella.nFingerMask = 0;
        //        }
        //        MessageBox.Show("Registro Actualizado");
        //    }
        //    else
        //        MessageBox.Show("Hubo un error");
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            

            using (AccesosEntidades db = new AccesosEntidades())
            {

                try
                {
                    //MessageBox.Show(Data.Templates.Count().ToString());
                    //Uso el dedo indice de la mano derecha que se que es el indice 1
                    byte[] streamHuella;
                    //Acceso.Datos.Model.RRHHLegajosHuellas empleado = new Acceso.Datos.Model.RRHHLegajosHuellas();
                    Acceso.Clases.Datos.RRHH.RRHHLegajosHuellas empleado = new Clases.Datos.RRHH.RRHHLegajosHuellas();
                    

                    for (int i = 0; i < Data.Templates.Count(); i++)
                    {
                        if(Data.Templates[i] != null)
                        {
                            switch (i)
                            {
                                case 0: empleado.nFingerMask = 32; break;
                                case 1: empleado.nFingerMask = 64; break;
                                case 2: empleado.nFingerMask = 128;break;
                                case 3: empleado.nFingerMask = 256;break;
                                case 4: empleado.nFingerMask = 512;break;
                                case 5: empleado.nFingerMask = 16; break;
                                case 6: empleado.nFingerMask = 8; break;
                                case 7: empleado.nFingerMask = 4; break;
                                case 8: empleado.nFingerMask = 2; break;
                                case 9: empleado.nFingerMask = 1; break;





                                default:break;


                            }
                            streamHuella = Data.Templates[i].Bytes;

                            empleado.nDedo = i;
                            empleado.nLegajoID = idLegajo;
                            empleado.iHuella = streamHuella;
                            empleado.sLegajoNombre = LegajoNombre;

                            empleado.Actualizar();

                            //db.RRHHLegajosHuellas.Add(empleado);
                            //db.SaveChanges();

                            
                        }
                    }

                    
                    MessageBox.Show("Registro agregado a la BD correctamente");

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void EnrollmentControl_Load(object sender, EventArgs e)
        {

        }
    }
}
