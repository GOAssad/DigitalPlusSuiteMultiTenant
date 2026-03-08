using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DPFP;
using DPFP.Capture;

namespace Altiora.DigitalPersona
{
	public partial class UareUAcceso : Altiora.Formularios.FrmEntidadesBase, DPFP.Capture.EventHandler
	{
		private DPFP.Capture.Capture Capturer;

		//Verificacion
		private DPFP.Template Template;
		private DPFP.Verification.Verification Verificator;

		public UareUAcceso()
		{
			InitializeComponent();
		}

		#region Verificacion
			public void Verify(DPFP.Template template)
			{
				Template = template;
				ShowDialog();
			}
			private void UpdateStatus(int FAR)
			{
				// Show "False accept rate" value
				SetStatus(String.Format("False Accept Rate (FAR) = {0}", FAR));
			}

		#endregion


		public virtual void Init()
		{
			try
			{
				Capturer = new DPFP.Capture.Capture();              // Create a capture operation.

				if (null != Capturer)
					Capturer.EventHandler = this;                   // Subscribe for capturing events.
				else
					Informar("No se puede inicializar la operación de Captura!");
			}
			catch
			{
				MessageBox.Show("No se puede inicializar la operación de Captura!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			//Verificar
			Informar("Verificacion de Huella");
			Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
			UpdateStatus(0);

		}
		protected virtual void Process(DPFP.Sample Sample)
		{
			// Draw fingerprint sample image.
			try
			{
				DrawPicture(ConvertSampleToBitmap(Sample));

				//Verificar
				// Process the sample and create a feature set for the enrollment purpose.
				DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

				// Check quality of the sample and start verification if it's good
				// TODO: move to a separate task
				if (features != null)
				{
					// Compare the feature set with our template
					DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
					Verificator.Verify(features, Template, ref result);
					UpdateStatus(result.FARAchieved);
					if (result.Verified)
						MakeReport("VERIFICADA.");
					else
						MakeReport("NOT RECONOCIDA.");


				}
			}
			catch (Exception ex)
			{

				System.Windows.Forms.MessageBox.Show(ex.Message);
			}
			
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

		#region Metodos de Captura de Huella
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
				Informar(status);
			}));
		}
		protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
		{
			DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();  // Create a sample convertor.
			Bitmap bitmap = null;                                                           // TODO: the size doesn't matter
			Convertor.ConvertToPicture(Sample, ref bitmap);                                 // TODO: return bitmap as a result
			return bitmap;
		}
		protected void SetPrompt(string prompt)
		{
			this.Invoke(new Function(delegate () {
				etiquetaPrompt.Text = prompt;
			}));
		}
		protected void MakeReport(string message)
		{
			this.Invoke(new Function(delegate () {
				//StatusText.AppendText(message + "\r\n");
				Informar(message + "\r\n");
			}));
		}
		private void DrawPicture(Bitmap bitmap)
		{
			this.Invoke(new Function(delegate () {
				Picture.Image = new Bitmap(bitmap, Picture.Size);   // Llenar la imagen en el Picture Box
			}));
		}

		#endregion

		#region Manejadores de Eventos del Formulario
		private void UareUAcceso_Load(object sender, EventArgs e)
		{
			Init();
			Start();
		}

		private void UareUAcceso_FormClosed(object sender, FormClosedEventArgs e)
		{
			Stop();
		}
		#endregion

		#region Manejadores de Eventos de UareU
			public void OnComplete(object Capture, string ReaderSerialNumber, Sample Sample)
			{
				MakeReport("La huella fue capturada.");
				SetPrompt("Escanee el mismo dedo nuevamente.");
				Process(Sample);
			}

			public void OnFingerGone(object Capture, string ReaderSerialNumber)
			{
				MakeReport("Dedo retirado del lector");
				//throw new NotImplementedException();
			}

			public void OnFingerTouch(object Capture, string ReaderSerialNumber)
			{
				MakeReport("Contacto con el lector");
				//throw new NotImplementedException();
			}

			public void OnReaderConnect(object Capture, string ReaderSerialNumber)
			{
				MakeReport("Sensor de Huellas conectado");
				//throw new NotImplementedException();
			}

			public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
			{
				MakeReport("Sensor de Huellas Desconectado");
				//throw new NotImplementedException();
			}

			public void OnSampleQuality(object Capture, string ReaderSerialNumber, CaptureFeedback CaptureFeedback)
		{
			if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
				MakeReport("La calidad del escaneo fue buena");
			else
				MakeReport("Calidad de escaneo Pobre.");
			//throw new NotImplementedException();
		}

		#endregion


	}
}
