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
    public partial class ColorSelector : UserControl
    {
        private string _Titulo;

        public string Titulo
        {
            get { return _Titulo; }
            set { _Titulo = value;
                etiquetaTitulo.Text = _Titulo;
            }
        }

        private int _ValorRojo;

        [DescriptionAttribute("Valor que toma el color Rojo"), Category("DigitalOne")]
        public int ValorRojo
        {
            get { return _ValorRojo; }
            set { _ValorRojo = value; }
        }

        private int _ValorVerde;

        [DescriptionAttribute("Valor que toma el color Verde"), Category("DigitalOne")]
        public int ValorVerde
        {
            get { return _ValorVerde; }
            set { _ValorVerde = value; }
        }

        private int _ValorAzul;

        [DescriptionAttribute("Valor que toma el color Azul"), Category("DigitalOne")]
        public int ValorAzul
        {
            get { return _ValorAzul; }
            set { _ValorAzul = value; }
        }



        [DescriptionAttribute("Evento que toma el control al modificar los colores"), Category("DigitalOne")]
        public event EventHandler ColorChanged;


        private Color _controlColor;

        [DescriptionAttribute("Valor que toma el Control al modificar los colores"), Category("DigitalOne")]
        public Color ControlColor
        {
            get { return _controlColor; }
            set { _controlColor = value; }
        }


        public ColorSelector()
        {
            InitializeComponent();
        }

        private void tb_Scroll(object sender, EventArgs e)
        {
            _ValorRojo = tbRed.Value;
            _ValorAzul = TbBlue.Value;
            _ValorVerde = TbGreen.Value;
            Color controlColor = Color.FromArgb(_ValorRojo, _ValorVerde, _ValorAzul);

            KnownColor kn = controlColor.ToKnownColor();
            txtColor.Text = kn.ToString();

            lblBlue.Text = _ValorAzul.ToString();
            lblGreen.Text = _ValorVerde.ToString();
            lblRed.Text = _ValorRojo.ToString();

            try
            {
                pnColor.BackColor = controlColor;
                _controlColor = controlColor;
                OnColorChanged(EventArgs.Empty);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        protected virtual void OnColorChanged(EventArgs e)
        {
            ColorChanged?.Invoke(this, e);
        }
    }
}
