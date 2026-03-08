using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace Altiora.DigitalPersona
{
	delegate void Function();	// a simple delegate for marshalling calls from event handlers to the GUI thread

	public partial class MainForm : Altiora.Formularios.FrmEntidadesBase
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void EnrollButton_Click(object sender, EventArgs e)
		{
			EnrollmentForm Enroller = new EnrollmentForm();
			Enroller.OnTemplate += this.OnTemplate;
			Enroller.legajo = controlEntidadLegajo.textoCodigo.Text.Trim();
			Enroller.ShowDialog();
		}

		private void VerifyButton_Click(object sender, EventArgs e)
		{
			VerificationForm Verifier = new VerificationForm();
			Verifier.Verify(Template);
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog save = new SaveFileDialog();
			save.Filter = "Fingerprint Template File (*.fpt)|*.fpt";
			if (save.ShowDialog() == DialogResult.OK) {
				using (FileStream fs = File.Open(save.FileName, FileMode.Create, FileAccess.Write)) {
					Template.Serialize(fs);
				}
			}
		}

		private void LoadButton_Click(object sender, EventArgs e)
		{

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Fingerprint Template File (*.fpt)|*.fpt";
            if (open.ShowDialog() == DialogResult.OK) {
                using (FileStream fs = File.OpenRead(open.FileName)) {
                    DPFP.Template template = new DPFP.Template(fs);
                    OnTemplate(template);
                }
            }

		}
          private void LoadButtonDB_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            // Connection string se lee de ConnectionStrings["Local"]
			//using (SqlConnection connection = new SqlConnection(connectionString))
			//{

				//SqlDataAdapter adapter = new SqlDataAdapter();
				//adapter.SelectCommand = new SqlCommand("Select * from RRHHLegajosHuellas where sLegajoID = '" + controlEntidadLegajo.textoCodigo.Text.Trim() + "'", connection);
				//adapter.Fill(dt);

				string query = "Select * from RRHHLegajosHuellas where sLegajoID = '" + controlEntidadLegajo.textoCodigo.Text.Trim() + "'";
				dt = Altiora.Datos.SQLServer.EjecutarParaSoloLectura(query, "AccesoXuXoHuella");
				if (dt.Rows.Count == 0)
				{
					Informar("No se encontraron registros de Huellas con el legajo Ingresado");
					return;
				}

				Byte[] data = new Byte[0];
				data = (Byte[])(dt.Rows[0]["bHuella"]);
				using (MemoryStream fs = new MemoryStream(data))
				{
					//Template.Serialize(fs);
					DPFP.Template template = new DPFP.Template(fs);
				
					OnTemplate(template);
				}
			//}
           
        }
		private void OnTemplate(DPFP.Template template)
		{
			this.Invoke(new Function(delegate()
			{
				Template = template;
				VerifyButton.Enabled = SaveButton.Enabled = (Template != null);
				if (Template != null)
					Informar("El sistema se encuentra listo para la verificaci�n");
				else
					Informar("No se reconoce un huella en la informacion cargada. Por favor repita la operaci�n");
			}));
		}

		private DPFP.Template Template;

        private void VerifyButtonDB_Click(object sender, EventArgs e)
        {
            VerificationForm Verifier = new VerificationForm();
            Verifier.Verify(Template);
        }

		public override void ActualizarTodo()
		{
			base.ActualizarTodo();
			Informar("Buscando Huellas almacenadas del legajo " + controlEntidadLegajo.textoCodigo.Text.Trim());
			DataTable dt = new DataTable();

			string query = "Select * from RRHHLegajosHuellas where sLegajoID = '" + controlEntidadLegajo.textoCodigo.Text.Trim() + "'";
			dt = Altiora.Datos.SQLServer.EjecutarParaSoloLectura(query, "AccesoXuXoHuella");
			if (dt.Rows.Count == 0)
			{
				Informar("No se encontraron registros de Huellas con el legajo Ingresado");
				return;
			}

			int dedo;
			foreach (DataRow dr in dt.Rows)
			{
				Byte[] data = new Byte[0];
				data = (Byte[])(dr["bHuella"]);

				dedo = (int)(dr["nDedo"]);

				using (MemoryStream fs = new MemoryStream(data))
				{
					//Template.Serialize(fs);
					DPFP.Template template = new DPFP.Template(fs);
					OnTemplate(template);
				}
			}
			//Byte[] data = new Byte[0];
			//data = (Byte[])(dt.Rows[0]["bHuella"]);
			
		}

		private void actualizarDedo(int dedo)
		{
			
		}

	}
}