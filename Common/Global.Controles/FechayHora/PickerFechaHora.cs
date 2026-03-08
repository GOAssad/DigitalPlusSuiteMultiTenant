using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global.Controles
{
	public partial class PickerFechaHora : DateTimePicker
	{
		private string _DiaNombre = " ";
		/// <summary>
		/// Nombre del dia, se actualiza en el ValueChanged
		/// </summary>
		[DescriptionAttribute("Nombre del dia, se actualiza en el ValueChanged"), Category("Altiora")]
		public string DiaNombre
		{
			get { return _DiaNombre; }
			set {
				_DiaNombre = value;
				ActualizarDia();
			}
		}

		private string _DiaResumido = " ";
		/// <summary>
		/// Nombre del dia, se actualiza en el ValueChanged
		/// </summary>
		[DescriptionAttribute("Nombre del dia Resumido en las primeras 3 letras, se actualiza en el ValueChanged"), Category("Altiora")]
		public string DiaResumido
		{
			get { return _DiaResumido; }
			set {
				_DiaResumido = value;
				ActualizarDia();
			}
		}

		public PickerFechaHora()
		{
			InitializeComponent();
			
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}



		private void NombreDia(DayOfWeek dia)
		{

			switch (dia)
			{
				case DayOfWeek.Sunday:
					_DiaNombre = "Domingo";
					_DiaResumido = "Dom";					
					break;
				case DayOfWeek.Monday:
					_DiaNombre = "Lunes";
					_DiaResumido = "Lun";
					break;
				case DayOfWeek.Tuesday:
					_DiaNombre = "Martes";
					_DiaResumido = "Mar";
					break;
				case DayOfWeek.Wednesday:
					_DiaNombre = "Miercoles";
					_DiaResumido = "Mie";
					break;
				case DayOfWeek.Thursday:
					_DiaNombre = "Jueves";
					_DiaResumido = "Jue";
					break;
				case DayOfWeek.Friday:
					_DiaNombre = "Viernes";
					_DiaResumido = "Vie";
					break;
				case DayOfWeek.Saturday:
					_DiaNombre = "Sabado";
					_DiaResumido = "Sab";
					break;
				default:
					break;
			}

		}

		private void ActualizarDia()
		{
			NombreDia(this.Value.DayOfWeek);
		}

		private void PickerFechaHora_Leave(object sender, EventArgs e)
		{
			ActualizarDia();
		}
	}
}
