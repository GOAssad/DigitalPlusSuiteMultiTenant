using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global.Funciones
{
	static public class Formatos
	{
		#region Grillas
		static public void Grid(DataGridView dgv)
		{
			dgv.RowsDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
			dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
			dgv.BorderStyle = BorderStyle.None;
			dgv.BackgroundColor =
				System.Drawing.Color.FromArgb((int)(byte)45, (int)(byte)66, (int)(byte)91);

			GridFormatoParaTodas(dgv);
		}
		/// <summary>
		/// Se adapta a los tonos del formulario 
		/// </summary>
		/// <param name="dgv">DataGridView que se quiere Modificar</param>
		/// <param name="formulario">Formulario en el que se encuentra el datagrid</param>
		static public void Grid(DataGridView dgv, Form formulario)
		{
			dgv.RowsDefaultCellStyle.BackColor = formulario.BackColor;
			dgv.BorderStyle = BorderStyle.None;
			dgv.BackgroundColor = formulario.BackColor;
			dgv.EnableHeadersVisualStyles = false;
			GridFormatoParaTodas(dgv);

		}

		private static void GridFormatoParaTodas(DataGridView dgv)
		{
			dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
			dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.EnableHeadersVisualStyles = false;
			dgv.RowHeadersVisible = false;
		}

		#endregion

		#region Formatos Varios String

		public static string FormatoMoneda(this decimal amount, string currencyCode)
		{
			var culture = (from c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
						   let r = new RegionInfo(c.LCID)
						   where r != null
						   && r.ISOCurrencySymbol.ToUpper() == currencyCode.ToUpper()
						   select c).FirstOrDefault();

			if (culture == null)
				return currencyCode + " "+ amount.ToString("###,###,##0.00");

			return string.Format(culture, "{0:C}", amount);
		}

		public static string FormatoDecimal(this decimal amount)
		{
			
			return string.Format("{0:###,###,###,##0.##}", amount);

			
		}

		public static string FormatoEntero(this decimal amount)
		{

			return string.Format("{0:###,###,###,##0}", amount);


		}

		public static string StringLeft(this string value, int largo)
		{
			if (string.IsNullOrEmpty(value)) return value;
			largo = Math.Abs(largo);

			return (value.Length <= largo
				   ? value
				   : value.Substring(0, largo)
				   );

		}

		public static string StringReplicate(char value, int cantidad)
		{
			return new String(value, cantidad);
		}

		/// <summary>
		/// Pasar los numeros hora y minuto y devuelve la cadena con el formato de Time para almacenar en SQL
		/// </summary>
		/// <param name="hora">Valor hora</param>
		/// <param name="min">Valor Minuto</param>
		/// <param name="incluirSegundos">true = :00, false = vacio</param>
		/// <returns>Devuelve String</returns>
		public static string StringNumeroHora(int hora, int min, bool incluirSegundos)
		{
			string sHoraFinal;
			sHoraFinal = StringReplicate('0', 2 - hora.ToString().Trim().Length)
					+ hora.ToString().Trim() + ":"
					+ StringReplicate('0', 2 - min.ToString().Trim().Length)
					+ min.ToString().Trim() + (incluirSegundos ? ":00" : string.Empty);

			return sHoraFinal;
		}
		/// <summary>
		/// Pasar los textos numericos hora y minuto y devuelve la cadena con el formato de Time para almacenar en SQL
		/// </summary>
		/// <param name="hora">Valor hora</param>
		/// <param name="min">Valor Minuto</param>
		/// <param name="incluirSegundos">true = :00, false = vacio</param>
		/// <returns>Devuelve String</returns>
		public static string StringTextoHora(string hora, string min, bool incluirSegundos)
		{
			//no tiene que tener ceros a la izquierda
			//lo convierto a entero y despues lo vuelvo a pasar como string y listo
			hora = int.Parse(hora).ToString().Trim();
			min = int.Parse(min).ToString().Trim();

			string sHoraFinal;
			sHoraFinal = StringReplicate('0', 2 - hora.Length)
					+ hora + ":"
					+ StringReplicate('0', 2 - min.Length)
					+ min + (incluirSegundos ? ":00" : string.Empty);

			return sHoraFinal;
		}
		#endregion

	}
}
