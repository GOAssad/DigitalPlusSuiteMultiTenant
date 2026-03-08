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
    public partial class TrackNumerico : UserControl
    {
		[DescriptionAttribute("Evento Para calcular el Scroll"), Category("DigitalOne")]
		public event EventHandler ScrollChanged;

		#region Declaracion de Propiedades

		private string _Titulo;
	
		[DescriptionAttribute("Titulo de la entidad"), Category("DigitalOne")]
		public string Titulo
		{
			get { return _Titulo; }
			set
			{
				_Titulo = value;
				ConfigurarControl();
			}
		}


		private int _Valor;
		[DescriptionAttribute("Valor devuelto"), Category("DigitalOne")]
		public int Valor
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

		private Color _iconoFontAwesomeColor;
		[DescriptionAttribute("Color del Icono"), Category("DigitalOne")]

		public Color iconoFontAwesomeColor
		{
			get { return _iconoFontAwesomeColor; }
			set
			{
				_iconoFontAwesomeColor = value;
				ConfigurarControl();
			}
		}

		private Int32 _Maximo;
		/// <summary>
		/// Valor Maximo permitido
		/// </summary>
		[DescriptionAttribute("Valor Maximo Permitido"), Category("DigitalOne")]
		public Int32 Maximo
		{
			get { return _Maximo; }
			set
			{
				_Maximo = value;
				trackbar.Maximum = _Maximo;
			}
		}

		private Int32 _Minimo;
		/// <summary>
		/// Valor Minimo permitido
		/// </summary>
		[DescriptionAttribute("Valor Minimo Permitido"), Category("DigitalOne")]
		public Int32 Minimo
		{
			get { return _Minimo; }
			set
			{
				_Minimo = value;
				trackbar.Minimum = _Minimo;
			}
		}

	#endregion

		protected virtual void OnScrollChanged(EventArgs e)
        {
			ScrollChanged?.Invoke(this, e);
        }
		public TrackNumerico()
        {
            InitializeComponent();			
        }

		
		private void ConfigurarControl()
		{
			etiquetaEntidad.Text = _Titulo;

			if (_iconoFontAwesome != FontAwesome.Sharp.IconChar.None)
				picture.IconChar = _iconoFontAwesome;

			if (_iconoFontAwesomeColor != Color.Empty)
            {
				picture.IconColor = _iconoFontAwesomeColor;
				lblValor.ForeColor = _iconoFontAwesomeColor;
			}

		}
		private void AsignarValor()
		{
			if (_Valor < _Minimo) _Valor = _Minimo;

			trackbar.Value = _Valor;
		}

        private void trackbar_Scroll(object sender, EventArgs e)
        {
			_Valor = trackbar.Value;
			lblValor.Text = _Valor.ToString();
			OnScrollChanged(EventArgs.Empty); 

        }
    }
}
