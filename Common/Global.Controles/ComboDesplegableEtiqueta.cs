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
    public partial class ComboDesplegableEtiqueta : UserControl
    {
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
        [DescriptionAttribute("Valor de la Entidad"), Category("DigitalOne")]
        public int Valor
        {
            get { return _Valor; }
            set
            {
                _Valor = value;
                AsignarValor();
            }
        }
        #endregion

        public ComboDesplegableEtiqueta()
        {
            InitializeComponent();
        }

        private void ConfigurarControl()
        {
            etiquetaEntidad.Text = _Titulo;
        }

        private void AsignarValor()
        {
            this.comboDesplegable.SelectedIndex = _Valor;
        }

        private void comboDesplegable_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Valor = comboDesplegable.SelectedIndex;
        }
    }
}
