using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Altiora.DigitalPersona
{
	/* NOTE: This form is a base for the EnrollmentForm and the VerificationForm,
		All changes in the CaptureForm will be reflected in all its derived forms.
	*/
	public partial class CaptureForm : Form, DPFP.Capture.EventHandler
	{

        public string legajo;

		public CaptureForm()
		{
			InitializeComponent();
            
		}

		protected virtual void Init()
		{
            try
            {
                Capturer = new DPFP.Capture.Capture();				// Create a capture operation.

                if ( null != Capturer )
                    Capturer.EventHandler = this;					// Subscribe for capturing events.
                else
                    SetPrompt("No se puede inicializar la operación de Captura!");
            }
            catch
            {
                MessageBox.Show("No se puede inicializar la operación de Captura!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
            }
		}

		protected virtual void Process(DPFP.Sample Sample)
		{
			// Draw fingerprint sample image.
			DrawPicture(ConvertSampleToBitmap(Sample));
		}

		protected void Start()
		{
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                    SetPrompt("Usar el lector para escanear la  Huella");
                }
                catch
                {
                    SetPrompt("No se pudo iniciar de Captura!");
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
                    SetPrompt("No se pudo terminar la captura!");
                }
            }
		}
		
	#region Form Event Handlers:

		private void CaptureForm_Load(object sender, EventArgs e)
		{
            labellegajo.Text = legajo;
			Init();
			Start();                                                // Start capture operation.

			TraerHuellasAlmacenadas();

		}
		private void TraerHuellasAlmacenadas()
		{
			DataTable dt = new DataTable();
			dt = Altiora.Datos.SQLServer.EjecutarParaSoloLectura("Select * from RRHHLegajosHuellas where sLegajoID = '" + legajo + "'", "AccesoXuXoHuella");
			foreach (DataRow item in dt.Rows)
			{
				switch (item["nDedo"])
				{
					case 1:
						label01.ForeColor = Color.DarkGreen;
						label01.Visible = true;
						break;
					case 2:
						label02.ForeColor = Color.DarkGreen;
						label02.Visible = true;
						break;
					case 3:
						label03.ForeColor = Color.DarkGreen;
						label03.Visible = true;
						break;
					case 4:
						label04.ForeColor = Color.DarkGreen;
						label04.Visible = true;
						break;
					case 5:
						label05.ForeColor = Color.DarkGreen;
						label05.Visible = true;
						break;
					default:
						break;
				}

			}
		}

		private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Stop();
		}
	#endregion

	#region EventHandler Members:

		public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
		{
			MakeReport("La huella fue capturada.");
			SetPrompt("Escanee el mismo dedo nuevamente.");
			Process(Sample);
		}

		public void OnFingerGone(object Capture, string ReaderSerialNumber)
		{
			MakeReport("El dedo fue retirado del lector.");
		}

		public void OnFingerTouch(object Capture, string ReaderSerialNumber)
		{
			MakeReport("El lector fue tocado");
		}

		public void OnReaderConnect(object Capture, string ReaderSerialNumber)
		{
			MakeReport("El lector de huellas fue conectado");
		}

		public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
		{
			MakeReport("El lector de huellas fue desconectado.");
		}

		public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
		{
			if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
				MakeReport("La calidad del escaneo fue buena");
			else
				MakeReport("Calidad de escaneo Pobre.");
		}
	#endregion

		protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
		{
			DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();	// Create a sample convertor.
			Bitmap bitmap = null;												            // TODO: the size doesn't matter
			Convertor.ConvertToPicture(Sample, ref bitmap);									// TODO: return bitmap as a result
			return bitmap;
		}

		protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
		{
			DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();	// Create a feature extractor
			DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
			DPFP.FeatureSet features = new DPFP.FeatureSet();
			Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);			// TODO: return features as a result?
			if (feedback == DPFP.Capture.CaptureFeedback.Good)
				return features;
			else
				return null;
		}

		protected void SetStatus(string status)
		{
			this.Invoke(new Function(delegate() {
				StatusLine.Text = status;
			}));
		}

		protected void SetPrompt(string prompt)
		{
			this.Invoke(new Function(delegate() {
				Prompt.Text = prompt;
			}));
		}
		protected void MakeReport(string message)
		{
			this.Invoke(new Function(delegate() {
				StatusText.AppendText(message + "\r\n");
			}));
		}

		private void DrawPicture(Bitmap bitmap)
		{
			this.Invoke(new Function(delegate() {
				Picture.Image = new Bitmap(bitmap, Picture.Size);	// fit the image into the picture box
			}));
		}

		private DPFP.Capture.Capture Capturer;

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

	}
}