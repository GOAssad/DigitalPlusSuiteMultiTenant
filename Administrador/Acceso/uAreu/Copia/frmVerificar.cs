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
    public partial class frmVerificar : FrmEntradaSalida
    {
        private DPFP.Template Template;
        private DPFP.Verification.Verification Verificator;
        private Acceso.Datos.Model.AccesosEntidades contexto;

        public void Verify(DPFP.Template template)
        {
            Template = template;
            ShowDialog();
        }

        protected override void Init()
        {
            base.Init();
            base.Text = "Verificación de Huella Digital";
            Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
            UpdateStatus(0);
        }

        private void UpdateStatus(int FAR)
        {
            // Show "False accept rate" value
            SetStatus(String.Format("RATIO FALSO ACEPTADO (FAR) = {0}", FAR));
        }

        protected override void Process(DPFP.Sample Sample)
        {
            base.Process(Sample);

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

                foreach (var emp in contexto.RRHHLegajosHuellas)
                {
                    stream = new MemoryStream(emp.iHuella);
                    template = new DPFP.Template(stream);

                    Verificator.Verify(features, template, ref result);
                    UpdateStatus(result.FARAchieved);
                    if (result.Verified)
                    {
                        DataTable tabla = new DataTable();
                        string Nombre;
                        Nombre = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                           "Select sNombre from RRHHLegajos where id = " + emp.nLegajoID.ToString().Trim()).Rows[0][0].ToString();
                    
                            MakeReport("BIENVENIDO: " + Nombre);
                        break;
                    }
                }
            }
        }

        public frmVerificar()
        {
            contexto = new Acceso.Datos.Model.AccesosEntidades();
            InitializeComponent();
        }
    }
}
