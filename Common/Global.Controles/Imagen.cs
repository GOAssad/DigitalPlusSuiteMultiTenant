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
    public partial class Imagen : PictureBox
    {
        public Imagen()
        {
            InitializeComponent();
        }

        public override bool AllowDrop { get; set; } 

    }
}
