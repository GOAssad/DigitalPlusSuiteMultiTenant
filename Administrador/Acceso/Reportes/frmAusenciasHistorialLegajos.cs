using Acceso.Clases.Datos.RRHH;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Acceso.Reportes
{
    public partial class frmAusenciasHistorialLegajos : Acceso.FrmBaseReportes
    {
        private string sLegajo;
        private string sApellido;
        private string sNombre;

        public frmAusenciasHistorialLegajos()
        {
            InitializeComponent();
        }

        public frmAusenciasHistorialLegajos(string legajo, string apellido, string nombre)
        {
            InitializeComponent();

            //para los querys
            sLegajo = legajo;
            sApellido = apellido;
            sNombre = nombre;

        }

        private void frmAusenciasHistorialLegajos_Load(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.EnableHyperlinks = true;
            this.reportViewer1.Visible = false;


            //para las etiqueta    s
            etiquetaLegajo.Text = sLegajo;
            etiquetaApellido.Text = sApellido;
            etiquetaNombre.Text = sNombre;
            
            //Cambio los valores del datetimepicker
            dateTimePickerFrom.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dateTimePickerTo.Value = DateTime.Now;

            btnPersonalizado.PerformClick();
            this.reportViewer1.RefreshReport();


        }

        private void btnPersonalizado_Click(object sender, EventArgs e)
        {
            
            RRHHAusenciasLegajoReport fichadasModelAusencias = new RRHHAusenciasLegajoReport();
            

            var fromDate = new DateTime(dateTimePickerFrom.Value.Year, dateTimePickerFrom.Value.Month, dateTimePickerFrom.Value.Day);
            var ToDate = new DateTime(dateTimePickerTo.Value.Year, dateTimePickerTo.Value.Month, dateTimePickerTo.Value.Day, 23, 59, 59);

            var legDesde = sLegajo;

            var incidencia = chkIncidencia.Checked;

            fichadasModelAusencias.CreateAusenciasReport(legDesde, fromDate, ToDate, incidencia);

            RRHHAusenciasLegajoListingBindingSource.DataSource = fichadasModelAusencias.fichadalisting;

            this.reportViewer1.Visible = true;
            this.reportViewer1.RefreshReport();
            labelAusencias.Text = fichadasModelAusencias.fichadalisting.Count.ToString();
        }
    }
}
