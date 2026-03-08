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
	public partial class Numerico : System.Windows.Forms.NumericUpDown
	{
		public Numerico()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		private void Numerico_Enter(object sender, EventArgs e)
		{
		
		}
	}
}
