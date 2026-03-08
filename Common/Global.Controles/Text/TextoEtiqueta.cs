using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Global.Controles.Text
{
	public partial class TextoEtiqueta : UserControl
	{
		#region Declaracion de Propiedades

		private string _Titulo;
		

		[DescriptionAttribute("Titulo de la entidad"), Category("DigitalOne")]
		public string Titulo
		{
			get { return _Titulo; }
			set { _Titulo = value;
				ConfigurarControl();
			}
		}
		private string _Valor;
		[DescriptionAttribute("Valor de la Entidad"), Category("DigitalOne")]
		public string Valor
		{
			get { return _Valor; }
			set
			{
				_Valor = value;
				AsignarValor();
			}
		}


		private FontAwesome.Sharp.IconChar _iconoFontAwesome;
		[DescriptionAttribute("Icono"), Category("DigitalOne")]

		public FontAwesome.Sharp.IconChar iconoFontAwesome
		{
			get { return _iconoFontAwesome; }
			set
			{
				_iconoFontAwesome = value;
				ConfigurarControl();
			}
		}

		

		private string _Mascara = string.Empty;

		/// <summary>
		/// Valor que toma la etiqueta
		/// </summary>
		[DescriptionAttribute("Mascara para el cuadro de codigo"), Category("DigitalOne")]
		public string Mascara
		{
			get { return _Mascara; }
			set
			{
				_Mascara = value;
				ConfigurarControl();
			}
		}
	
		private bool _Numerico;

		/// <summary>
		/// Valor que toma la etiqueta
		/// </summary>
		[DescriptionAttribute("Activa el KeyPress Event para controlar los digitos ingresados"), Category("DigitalOne")]
		public bool Numerico
		{
			get { return _Numerico; }
			set
			{
				_Numerico = value;
			}
		}

		private HorizontalAlignment _Alinear = HorizontalAlignment.Left;
		[DescriptionAttribute("Alineacion del Texto"), Category("DigitalOne")]
		public HorizontalAlignment Alinear
        {
            get { return _Alinear; }
			set { _Alinear = value;
				ConfigurarControl();
			}
        }
		#endregion

		public TextoEtiqueta()
		{
			InitializeComponent();
		}


		private void ConfigurarControl()
		{
			etiquetaEntidad.Text = _Titulo;

			if (_iconoFontAwesome != FontAwesome.Sharp.IconChar.None)
				picture.IconChar = _iconoFontAwesome;

			textoEntidad.Mask = _Mascara;

			textoEntidad.TextAlign = _Alinear;
		}

		private void AsignarValor()
		{
			textoEntidad.Text = _Valor;
		}

		private void TextoEtiqueta_Resize(object sender, EventArgs e)
		{
			//int ancho = this.Size.Width;
			//this.Size = new Size(ancho , Altura);
			
		}

		private void textoEntidad_TextChanged(object sender, EventArgs e)
		{
			_Valor = textoEntidad.Text.Trim();
		}


        private void textoEntidad_KeyPress(object sender, KeyPressEventArgs e)
        {
			if (!_Numerico)
				return;

			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
				e.Handled = true;

			if ((e.KeyChar == '.') && ((sender as MaskedTextBox).Text.IndexOf('.') > -1))
				e.Handled = true;
        }
    }
}
