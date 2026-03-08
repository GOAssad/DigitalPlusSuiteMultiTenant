using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global.Controles.FechayHora
{
	public partial class FechaDesdeHasta : UserControl
	{

		/// <summary>
		/// Contiene el valor ingresado en la Fecha Desde
		/// </summary>
		[DescriptionAttribute("Contiene el valor ingresado en la Fecha Desde"), Category("Altiora")]
		public DateTime FechaDesde
		{
			get { return pickerDesde.Value.Date; }
			set { pickerDesde.Value = value; }
		}


		/// <summary>
		/// Contiene el valor ingresado en la Fecha Hasta
		/// </summary>
		[DescriptionAttribute("Contiene el valor ingresado en la Fecha Hasta"), Category("Altiora")]
		public DateTime FechaHasta
		{
			get { return pickerHasta.Value.Date; }
			set { pickerHasta.Value = value; }
			
		}

		public FechaDesdeHasta()
		{
			InitializeComponent();
			lblDiaNombreDesde.DataBindings.Add("Text", pickerDesde, "DiaNombre", true, DataSourceUpdateMode.OnPropertyChanged);
			lblDiaNombreHasta.DataBindings.Add("Text", pickerHasta, "DiaNombre", true, DataSourceUpdateMode.OnPropertyChanged);

            FechaDesde = DateTime.Now.Date;
            FechaHasta = DateTime.Now.Date;
        }

		private void pickerHasta_ValueChanged(object sender, EventArgs e)
		{
			if (pickerHasta.Value.Date < pickerDesde.Value.Date)
			{
				MessageBox.Show("La Fecha Hasta debe ser mayor o igual a la Fecha Desde");
				pickerHasta.Value = pickerDesde.Value;
			}
		}
	}
}
