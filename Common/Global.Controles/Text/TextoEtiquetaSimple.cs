using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global.Controles.Text
{
    public partial class TextoEtiquetaSimple : UserControl
    {
		#region Declaracion de Propiedades

		private string _Titulo;


		[DescriptionAttribute("Titulo de la entidad"), Category("Tocayanda")]
		public string Titulo
		{
			get { return _Titulo; }
			set
			{
				_Titulo = value;
				ConfigurarControl();
			}
		}
		private string _Valor;
		[DescriptionAttribute("Valor de la Entidad"), Category("Tocayanda")]
		public string Valor
		{
			get { return _Valor; }
			set
			{
				_Valor = value;
				AsignarValor();
			}
		}


		#endregion
		public TextoEtiquetaSimple()
        {
            InitializeComponent();
        }
		private void ConfigurarControl()
		{
			etiquetaEntidad.Text = _Titulo;
		}

		private void AsignarValor()
		{
			textoEntidad.Text = _Valor;
		}

        private void textoEntidad_TextChanged(object sender, EventArgs e)
        {
			_Valor = textoEntidad.Text.Trim();
		}
    }
}
