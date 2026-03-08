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
	public partial class HoraMinutos : UserControl
	{
		private int _Hora;
		[DescriptionAttribute("Hora: toma el primer control de la izquiera"), Category("Altiora")]
		public int Hora
		{
			get { return _Hora; }
			set {
                _Hora = value;
                actualizar();
              }
		}

		private int _Minutos;
		[DescriptionAttribute("Minutos: toma el segundo control de la izquiera"), Category("Altiora")]
		public int Minutos
		{
			get { return _Minutos; }
			set {
                _Minutos = value;
                actualizar();
            }
		}


		public HoraMinutos()
		{
			InitializeComponent();
		}

		private void HoraMinutos_Load(object sender, EventArgs e)
		{

		}

        private void actualizar()
        {
            numHora.Value = _Hora;
            numMinuto.Value = _Minutos;
        }

		private void numHora_ValueChanged(object sender, EventArgs e)
		{
		    _Hora = int.Parse(((Global.Controles.Numerico)(sender)).Value.ToString());
            CambioValores();

		}

		private void numMinuto_ValueChanged(object sender, EventArgs e)
		{
			_Minutos = int.Parse(((Global.Controles.Numerico)(sender)).Value.ToString());
            CambioValores();
		}
        /// <summary>
        /// Cuando Cambias la hora o los minutos corre este evento
        /// </summary>
        public virtual void CambioValores()
        {

        }
        private void numHora_Enter(object sender, EventArgs e)
		{
		}
	}
}
