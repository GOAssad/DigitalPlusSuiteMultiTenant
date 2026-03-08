using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acceso.ControlEntidad
{
    public partial class CargarImagenBasedeDatos : UserControl
    {
        public CargarImagenBasedeDatos()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                picFoto.Image = Image.FromFile(dialog.FileName);
            }
        }
    }
}
