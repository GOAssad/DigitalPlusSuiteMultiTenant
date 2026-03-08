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
    public partial class TextoSimple : System.Windows.Forms.TextBox
    {
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

        public TextoSimple()
        {
            InitializeComponent();
        }
        private void AsignarValor()
        {
            this.Text = _Valor;
        }

        private void TextoSimple_TextChanged(object sender, EventArgs e)
        {
            _Valor = this.Text.Trim();
        }
    }
}
