using DPFP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Altiora.DigitalPersona
{
	/* NOTE: This form is inherited from the CaptureForm,
		so the VisualStudio Form Designer may not load it properly
		(at least until you build the project).
		If you want to make changes in the form layout - do it in the base CaptureForm.
		All changes in the CaptureForm will be reflected in all derived forms 
		(i.e. in the EnrollmentForm and in the VerificationForm)
	*/
	public class EnrollmentForm : CaptureForm
	{
		public delegate void OnTemplateEventHandler(DPFP.Template template);

		public event OnTemplateEventHandler OnTemplate;

		protected override void Init()
		{
			base.Init();
			base.Text = "Registraci�n de Huella";
			Enroller = new DPFP.Processing.Enrollment();			// Create an enrollment.
			UpdateStatus();
		}
		

		protected override void Process(DPFP.Sample Sample)
		{
			base.Process(Sample);

			// Process the sample and create a feature set for the enrollment purpose.
			DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);
			DPFP.Capture.SampleConversion ToByte = new DPFP.Capture.SampleConversion();

			// Check quality of the sample and add to enroller if it's good
			if (features != null) try
			{
				MakeReport("Las propiedades de la Huella fueron creadas.");
				Enroller.AddFeatures(features);		// Add feature set to template.
			}
			finally {
				UpdateStatus();

				// Check if template has been created.
				switch(Enroller.TemplateStatus)
				{
					case DPFP.Processing.Enrollment.Status.Ready:   // report success and stop capturing
					{
						OnTemplate(Enroller.Template);
						SetPrompt("Click en cerrar y verifique la huella");
						Stop();


						//  byte[] btarr = null;
						// Enroller.Template.Serialize(ref btarr);
						MemoryStream fingerprintData = new MemoryStream();
						Enroller.Template.Serialize(fingerprintData);
						fingerprintData.Position = 0;
						BinaryReader br = new BinaryReader(fingerprintData);
						byte[] bytes = br.ReadBytes((Int32)fingerprintData.Length);
                        br.Close();
                        fingerprintData.Close();

						try
						{
							/*if (User.id == 0)
								User.id = dbinfo.compare(User.Login, User.Pass);
							dact_id = dbinfo.InsertFingSample(btarr);
							dbinfo.InsertFinger(User.id, GetFingNum(), dact_id);*/

							SqlParameter[] par = new SqlParameter[3];
							//@legajo varchar(30),  @dedo int, @Huella binary

							par[0] = new SqlParameter();
							par[0].ParameterName = "@legajo";
							par[0].Value = legajo;
							par[0].SqlDbType = SqlDbType.VarChar;

							par[1] = new SqlParameter();
							par[1].ParameterName = "@dedo";
                            par[1].Value = NumDedo.Value;
							par[1].SqlDbType = SqlDbType.Int;


							par[2] = new SqlParameter();
							par[2].ParameterName = "@Huella";
							par[2].Value = bytes;
							par[2].SqlDbType = SqlDbType.Image;

							string queryString = "RRHHLegajosHuellas_Insert";
							var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Local"];
							string SQLString = cs != null ? cs.ConnectionString : "";
							using (SqlConnection connection = new SqlConnection( SQLString))
							{
								SqlCommand command = new SqlCommand(queryString, connection);
								command.CommandText = queryString;
								command.CommandType = CommandType.StoredProcedure;
										command.Parameters.Add(par[0]);
										command.Parameters.Add(par[1]);
										command.Parameters.Add(par[2]);
								connection.Open();
								command.ExecuteNonQuery();
										command.Parameters.Clear();

							}
						}
						catch (Exception e)
						{
							MakeReport(e.Message);
						}
						break;
					}

					case DPFP.Processing.Enrollment.Status.Failed:  // report failure and restart capturing
					{
						Enroller.Clear();
						Stop();
						UpdateStatus();
						OnTemplate(null);
						Start();
						break;
					}
				}
			}
		}

		private void UpdateStatus()
		{
			// Show number of samples needed.
			SetStatus(String.Format("Veces necesarias de escaneo: {0}", Enroller.FeaturesNeeded));
		}

		private DPFP.Processing.Enrollment Enroller;
	}
}
