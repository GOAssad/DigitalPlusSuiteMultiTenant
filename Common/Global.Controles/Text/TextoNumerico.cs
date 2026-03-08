using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global.Controles
{
	public partial class TextoNumerico : TextoSimple
	{
		private int _TipoNumero;
		private int _Maximo;
		private int _Minimo;
		private decimal _ValorNumerico;
		

		public TextoNumerico()
		{
			InitializeComponent();
			
		}

		/// <summary>
		/// 1: Entero, 2: Moneda, 3: Numero, 4: Porcentaje
		/// </summary>
		/// 
		[DescriptionAttribute("1:Entero, 2:Moneda, 3:Numero, 4:Porcentaje"),Category("Altiora")]
		public int TipoNumero {
			get { return _TipoNumero; }
			set { _TipoNumero = value; }
		}

		/// <summary>
		/// Es el valor que se debe tomar cada vez que se necesite tomar el valor de este campo
		/// </summary>
		[DescriptionAttribute("Contiene el valor ingresado en formato decimal"), Category("Altiora")]
		public decimal ValorNumerico
		{
			get { return _ValorNumerico; }
			set { _ValorNumerico = value; }
		}

		[DescriptionAttribute("Valor numerico maximo permitido"), Category("Altiora")]
		public int Maximo
		{
			get { return _Maximo; }
			set { _Maximo = value; }
		}

		[DescriptionAttribute("Valor numerico minimo permitido"), Category("Altiora")]
		public int Minimo
		{
			get { return _Minimo; }
			set { _Minimo = value; }
		}


		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		/// <summary>
		/// Metodo que se llama cuando toma el foco el control TextBox
		/// </summary>
		/// <param name="sender">Pasar el Sender desde el metodo que se lo llama</param>
		/// <param name="e">Pasar el e desde el metodo que se lo llama</param>
		private void TextBoxFormattedOnEnter(object sender, EventArgs e)
		{
			// Referencio el control TextBox que desencadenao el evento.
			//
			TextBox tb = (TextBox)sender;

			// Muestro el valor de Tag sin formatear.
			//
			tb.Text = Convert.ToString(tb.Tag);

		}

		/// <summary>
		/// Cuando dejas el textoBox
		/// </summary>
		/// <param name="sender">se le pasa el sender de donde estoy parado</param>
		/// <param name="e">se le pasa el e desde estoy parado</param>
		private void TextBoxFormattedOnLeave(object sender, EventArgs e)
		{
			
			// Referencio el TextBox que llamo al evento.
			//
			TextBox tb = (TextBox)sender;

			// Me fijo si el valor Aplica
			//
			decimal numero = default(decimal);
			bool bln = decimal.TryParse(tb.Text, out numero);

			if ((!(bln)))
			{
				// Si el valor no es valido, limpio el control
				tb.Clear();
				return;
			}
			// Formateo el valor segun  TipoNumero

			if (this.TipoNumero == 2)
				tb.Text = string.Format("{0:C2}", numero);

			if (this.TipoNumero == 4)
				tb.Text = string.Format("{0:P2}", numero);
			
			if (this.TipoNumero == 4)
				{
				if (numero > 1)
				{
					tb.Clear();
					return;
				}
			}
			else
			{ 
				if (numero > _Maximo)
				{
					tb.Clear();
					return;
				}

				if (numero < _Minimo)
				{
					tb.Clear();
					return;
				}
			}

			// En la propiedad Tag guardo el valor con todos los decimales.
			//
			tb.Tag = numero;
			this.ValorNumerico = numero;



		}

		private void TextoNumerico_KeyPress(object sender, KeyPressEventArgs e)
		{
			//if (TipoNumero == 1)  //numero entero no le pongo punto
			//{
			//	if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
			//		e.Handled = true;
			//	else
			//		e.Handled = false;
			//}
			//else
			//{
			//	if ((e.KeyChar >= '0' && e.KeyChar <= '9'))
			//		if (e.KeyChar == ',' || e.KeyChar =='.' || e.KeyChar == '\b')
			//		e.Handled = false;
			//	else
			//		e.Handled = true;
			//}

		}

		private void TextoNumerico_Leave(object sender, EventArgs e)
		{
			this.TextBoxFormattedOnLeave(sender, e);
		}

		private void TextoNumerico_Enter(object sender, EventArgs e)
		{
			this.TextBoxFormattedOnEnter(sender, e);
		}

	}
}
