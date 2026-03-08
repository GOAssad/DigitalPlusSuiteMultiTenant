using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global.Controles
{
	public partial class Reloj : UserControl
	{
		public Reloj()
		{
			InitializeComponent();
			etiquetaReloj.Text = DateTime.Now.ToLongTimeString();
			etiquetafecha.Text = DateTime.Now.ToShortDateString();
			etiquetadia.Text = DateTime.Now.ToString("dddd");
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			etiquetaReloj.Text = DateTime.Now.ToLongTimeString();
			etiquetafecha.Text = DateTime.Now.ToShortDateString();
			etiquetadia.Text = DateTime.Now.ToString("dddd");
		}
	}
}
