using System;
using System.Data;
using Acceso.Clases.Datos.RRHH;

namespace Acceso.Reportes
{
    public partial class frmFichadasHistorialLegajo : Acceso.FrmBaseReportes
    {
        private string sLegajo;
        private string sApellido;
        private string sNombre;

        public frmFichadasHistorialLegajo()
        {
            InitializeComponent();
        }
        

        public frmFichadasHistorialLegajo(string legajo, string apellido, string nombre)
        {
            InitializeComponent();

            //para los querys
            sLegajo = legajo;
            sApellido = apellido;
            sNombre = nombre;

            

        }

        private void frmFichadasHistorialLegajo_Load(object sender, EventArgs e)
        {

            this.reportViewer1.LocalReport.EnableHyperlinks = true;
            this.reportViewer1.Visible = false;


            //para las etiqueta    s
            etiquetaLegajo.Text = sLegajo;
            etiquetaApellido.Text = sApellido;
            etiquetaNombre.Text = sNombre;

            dateTimePickerFrom.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dateTimePickerTo.Value= DateTime.Now;


            btnPersonalizado.PerformClick();
            
        }

       
        private void etiquetaSimple5_Click(object sender, EventArgs e)
        {

        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            var fromDate = DateTime.Today;
            var toDate = DateTime.Now;
            var legDesde = sLegajo;
            var legHasta = sLegajo;

            getFichadasReport(fromDate, toDate, legDesde, legHasta);
        }

        private void getFichadasReport(DateTime startDate, DateTime endDate, string legajodesde, string legajohasta)
        {
            RRHHFichadasReport fichadasModel = new RRHHFichadasReport();
            fichadasModel.createFichadasReport(startDate, endDate, legajodesde, legajohasta, string.Empty);

            RRHHFichadasReportBindingSource.DataSource = fichadasModel;
            RRHHFichadasListingBindingSource.DataSource = fichadasModel.fichadalisting;

            this.reportViewer1.Visible = true;
            this.reportViewer1.RefreshReport();

        }

        private void btn7dias_Click(object sender, EventArgs e)
        {
            var fromDate = DateTime.Today.AddDays(-7);
            var toDate = DateTime.Now;
            var legDesde = sLegajo;
            var legHasta = sLegajo;

            getFichadasReport(fromDate, toDate, legDesde, legHasta);
        }

        private void btnEsteMes_Click(object sender, EventArgs e)
        {
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toDate = DateTime.Now;
            var legDesde = sLegajo;
            var legHasta = sLegajo;

            getFichadasReport(fromDate, toDate, legDesde, legHasta);
        }

        private void btn30Dias_Click(object sender, EventArgs e)
        {
            var fromDate = DateTime.Today.AddDays(-30);
            var toDate = DateTime.Now;
            var legDesde = sLegajo;
            var legHasta = sLegajo;

            //actualizo los picker
            dateTimePickerFrom.Value = fromDate;
            dateTimePickerTo.Value = toDate;

            getFichadasReport(fromDate, toDate, legDesde, legHasta);
        }

        private void btnEsteAno_Click(object sender, EventArgs e)
        {
            var fromDate = new DateTime(DateTime.Now.Year, 1, 1);
            var toDate = DateTime.Now;
            var legDesde = sLegajo;
            var legHasta = sLegajo;

            getFichadasReport(fromDate, toDate, legDesde, legHasta);
        }

        private void btnPersonalizado_Click(object sender, EventArgs e)
        {

            var startDate = dateTimePickerFrom.Value;
            var endDate = dateTimePickerTo.Value;
            var legajodesde = sLegajo;
            var legajohasta = sLegajo;


            RRHHFichadasReport fichadasModel = new RRHHFichadasReport();


            switch (comboES.SelectedIndex)
            {
                case 0:
                    fichadasModel.createFichadasReport(startDate, endDate, legajodesde, legajohasta, string.Empty);
                    break;
                case 1:
                    fichadasModel.createFichadasReport(startDate, endDate, legajodesde, legajohasta, "E", string.Empty);
                    break;
                case 2:
                    fichadasModel.createFichadasReport(startDate, endDate, legajodesde, legajohasta, "S", string.Empty);
                    break;
                default:
                    fichadasModel.createFichadasReport(startDate, endDate, legajodesde, legajohasta, string.Empty);
                    break;
            }

            RRHHFichadasReportBindingSource.DataSource = fichadasModel;
            RRHHFichadasListingBindingSource.DataSource = fichadasModel.fichadalisting;

            this.reportViewer1.Visible = true;
            this.reportViewer1.RefreshReport();


        }
    }
}
