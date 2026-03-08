using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global.Datos;

namespace Global.Controles.Botones
{
    public partial class BotonExportarDataTable_a_Excel : UserControl
    {
        private DataTable _Tabla;

        [DescriptionAttribute("Data Table a Migrar"), Category("Altiora")]
        public DataTable Tabla
        {
            get { return _Tabla; }
            set
            {
                _Tabla = value;
            }
        }
        public BotonExportarDataTable_a_Excel()
        {
            InitializeComponent();
        }

        private void botonExcel_Click(object sender, EventArgs e)
        {
            string archivo;
            saveFileDialog.Filter = "Archivo Excel|*.xlsx";
            saveFileDialog.Title = "Guardar en Archivo Excel";
            saveFileDialog.ShowDialog();
            archivo = saveFileDialog.FileName;
            if (archivo == string.Empty)           
                return;
            
            _Tabla.ExportToExcel(archivo);
        }

        private void BotonExportarDataTable_a_Excel_Load(object sender, EventArgs e)
        {

        }
    }
}
