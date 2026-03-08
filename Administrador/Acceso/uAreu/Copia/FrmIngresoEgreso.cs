using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Acceso.uAreu
{

    delegate void FunctionIngrEgr();

    public partial class FrmIngresoEgreso : Form, DPFP.Capture.EventHandler
    {

        private DPFP.Template Template;
        private DPFP.Verification.Verification Verificator;
        private Acceso.Datos.Model.AccesosEntidades contexto;
        string Nombre;

        public void Verify(DPFP.Template template)
        {
            Template = template;
            ShowDialog();
        }


        //------------------------------------------------------
        public FrmIngresoEgreso()
        {
            contexto = new Datos.Model.AccesosEntidades();
            InitializeComponent();


            // traigo la sucursal de la terminal
            etiquetaSucursal.Text = 
            Acceso.Clases.Datos.Generales.GRALVariablesGlobales.TraerValor("GRALSucursal");


        }

        protected virtual void Init()
        {
            try
            {
                Capturer = new DPFP.Capture.Capture();

                if (null != Capturer)
                    Capturer.EventHandler = this;
                else
                    SetPrompt("Operacion de captura fallida");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Operación de captura fallida " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Text = "Verificación de Huella Digital";
            Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
            UpdateStatus(0);
        }

        private void UpdateStatus(int FAR)
        {
            // Show "False accept rate" value
            SetStatus(String.Format("RATIO FALSO ACEPTADO (FAR) = {0}", FAR));
        }

        protected virtual void Process(DPFP.Sample Sample)
        {
            // Draw fingerprint sample image.
            DrawPicture(ConvertSampleToBitmap(Sample));

            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            // TODO: move to a separate task
            if (features != null)
            {
                // Compare the feature set with our template
                DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();

                DPFP.Template template = new DPFP.Template();
                Stream stream;
                Nombre = string.Empty;

                foreach (var emp in contexto.RRHHLegajosHuellas)
                {
                    stream = new MemoryStream(emp.iHuella);
                    template = new DPFP.Template(stream);

                    try
                    {
                        Verificator.Verify(features, template, ref result);
                        UpdateStatus(result.FARAchieved);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                        break;
                    }
                    if (result.Verified)
                    {
                        Nombre = emp.sLegajoNombre;
                        MakeReport("Fichaje Registrado de: " + Nombre);
                        /*
                        DataTable tabla = new DataTable();
                        
                        Nombre = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                           "Select sNombre from RRHHLegajos where id = " + emp.nLegajoID.ToString().Trim()).Rows[0][0].ToString();

                        */

                        break;
                    }
                }
                if (Nombre == string.Empty)
                   MakeReport("No se Reconocio la Huella" );

            }
        }

        protected void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                    SetPrompt("Escanea tu huella usando el lector");
                }
                catch
                {
                    SetPrompt("No se puede iniciar la captura");
                }
            }
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch
                {
                    SetPrompt("No se puede terminar la captura");
                }
            }
        }

        #region Form Event Handlers:

        private void CaptureForm_Load(object sender, EventArgs e)
        {
            Init();
            Start();                                                // Start capture operation.
        }

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
            Template = null;
            Verificator = null;

    }
        #endregion

        #region EventHandler Members:

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            MakeReport("La muestra ha sido capturada");
            SetPrompt("Escanea tu misma huella otra vez");
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            MakeReport("La huella fue removida del lector");
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            MakeReport("El lector fue tocado");
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("El Lector de huellas ha sido conectado");
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("El Lector de huellas ha sido desconectado");
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
                MakeReport("La calidad de la muestra es BUENA");
            else
                MakeReport("La calidad de la muestra es MALA");
        }
        #endregion

        protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();  // Create a sample convertor.
            Bitmap bitmap = null;                                                           // TODO: the size doesn't matter
            Convertor.ConvertToPicture(Sample, ref bitmap);                                 // TODO: return bitmap as a result
            return bitmap;
        }
        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();  // Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            // TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }

        protected void SetStatus(string status)
        {
            this.Invoke(new Function(delegate () {
                StatusLine.Text = status;
            }));
        }

        protected void SetPrompt(string prompt)
        {
            this.Invoke(new Function(delegate () {
                Prompt.Text = prompt;
            }));
        }
        protected void MakeReport(string message)
        {
            this.Invoke(new Function(delegate () {
                StatusText.AppendText(message + "\r\n");
                etiquetaBienVenida.Text = Nombre;
            }));
        }

        private void DrawPicture(Bitmap bitmap)
        {
            this.Invoke(new Function(delegate () {
                Picture.Image = new Bitmap(bitmap, Picture.Size);   // fit the image into the picture box
            }));
        }
        private DPFP.Capture.Capture Capturer;
    }
}
