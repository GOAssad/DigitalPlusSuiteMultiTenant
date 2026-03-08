using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acceso
{
    public partial class FrmBaseReportes : Form
    {
        public ControlPanelEntidad.CtrEntidadPanel ocPEnt = new ControlPanelEntidad.CtrEntidadPanel();

        public FrmBaseReportes()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Aca tenes que ingresar el codigo que toma el valor del texto de la lupita para ir a buscar los datos.
        /// </summary>
        public virtual void ActualizarTodo()
        {


        }

        /// <summary>
        /// Aca tenes que ingresar el codigo que necesitas para cuando ingresas un valor erroneo el la lupita o para cuando queres blanquear todo el formulario
        /// </summary>
        public virtual void BlanquearTodo()
        {

        }
    }
}

