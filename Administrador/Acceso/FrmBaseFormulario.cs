using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global.Funciones;
using Acceso.ControlPanelEntidad;

namespace Acceso
{
    public partial class FrmBaseFormulario : Form
    {
        public FrmBaseFormulario()
        {
            InitializeComponent();
            
        }
                public ControlPanelEntidad.CtrEntidadPanel ocPEnt = new ControlPanelEntidad.CtrEntidadPanel();

       

        #region Propiedades Formulario
        private bool _AcitvarToolBar = true;
        [DescriptionAttribute("TRUE la barra esta visible"), Category("DigitalOne")]
        public bool ActivarToolBar
        {
            get { return _AcitvarToolBar; }
            set
            {
                _AcitvarToolBar = value;
                ActualizarToolbar();
            }
        }

        private bool _BotonEliminarVisible;
        [DescriptionAttribute("TRUE boton Eliminar de barra de navegacion Activado"), Category("DigitalOne")]
        public bool BotonEliminarVisible
        {
            get { return _BotonEliminarVisible; }
            set
            {
                _BotonEliminarVisible = value;
                ActualizarToolbar();
            }
        }

        private bool _BotonGuardarVisible;
        [DescriptionAttribute("TRUE boton Guardar de barra de navegacion: Activado"), Category("DigitalOne")]
        public bool BotonGuardarVisible
        {
            get { return _BotonGuardarVisible; }
            set
            {
                _BotonGuardarVisible = value;
                ActualizarToolbar();
            }
        }

        private bool _BotonGuardarEnable;
        [DescriptionAttribute("TRUE boton Guardar de barra de navegacion: Enable"), Category("DigitalOne")]
        public bool BotonGuardarEnable
        {
            get { return _BotonGuardarEnable; }
            set
            {
                _BotonGuardarEnable = value;
                ActualizarToolbar();
            }
        }

        private bool _BotonEditarVisible;
        [DescriptionAttribute("TRUE boton Editar de barra de navegacion: Activado"), Category("DigitalOne")]
        public bool BotonEditarVisible
        {
            get { return _BotonEditarVisible; }
            set
            {
                _BotonEditarVisible = value;
                ActualizarToolbar();
            }
        }

        private bool _BotonActualizarVisible;
        [DescriptionAttribute("TRUE boton Actualizar de barra de navegacion: Activado"), Category("DigitalOne")]
        public bool BotonActualizarVisible
        {
            get { return _BotonActualizarVisible; }
            set
            {
                _BotonActualizarVisible = value;
                ActualizarToolbar();
            }
        }

        private string _Titulo;
        [DescriptionAttribute("Titulo del formulario"), Category("DigitalOne")]
        public string Titulo
        {
            get { return _Titulo; }
            set
            {
                _Titulo = value;
                ActualizarToolbar();
            }
        }

        private string _NombreSistema;
        [DescriptionAttribute("Contiene el valor de la variable global GRALNombreSistema"), Category("DigitalOne")]
        public string NombreSistema
        {
            get { return _NombreSistema; }
            set { _NombreSistema = value; }
        }

        private DialogResult _drRespuesta;
        [DescriptionAttribute("Respuesta a Cualquier Resultado de Dialogo de un messagebox"), Category("TocayanaComportamiento")]
        public DialogResult drRespuesta
        {
            get { return _drRespuesta; }
            set { _drRespuesta = value; }
        }


        #endregion

        
        public virtual void ActualizarToolbar()
        {
            toolStripButtonEliminar.Visible = _BotonEliminarVisible;
            toolStripButtonGuardar.Visible = _BotonGuardarVisible;
            toolStripButtonGuardar.Enabled = _BotonGuardarEnable;

            //// este boton es el boton nuevo, el de editar fue reemplazado por este 13/04/2021
            toolStripButtonNuevo.Visible = _BotonEditarVisible;
            ///////////////////////////////////////////////////////////

            toolStripButtonActualizar.Visible = _BotonActualizarVisible;

            toolStripLabelTitulo.Text = _Titulo;

            this.toolStrip.Visible = _AcitvarToolBar;
        }


        public virtual void ClickSalir()
        {

        }

        public virtual void ClickEliminar()
        {
            //drRespuesta = new DialogResult();
            drRespuesta =
            MessageBox.Show("Esta seguro que quiere eliminar el registro?", "Eliminar Registro", MessageBoxButtons.YesNo);

        }

        private bool retornoGuardar;
        private string MensajeGuardar;
        public virtual bool ClickGuardar()
        {
            MensajeGuardar = string.Empty;
            retornoGuardar = true;

            ///Busco si hay ControlEntidad en el formulario, si hay me fijo si tiene que estar llenos
            ///si encuentro alguno que tenga que estar lleno y esta vacio personalizo el mensaje de error
            ///y retorno. Uso la libreria de Funciones donde hay un metodo extensor
            ///
            //this.GetControls<ControlEntidad.ControlEntidadSimple>().ForEach(p => p.AutoGenerarCodigo = true);
            //this.GetControls<ControlEntidad.CtrEntidad>().ForEach(p => ControlarVaciosEntidad(p));
            this.GetControls<ControlPanelEntidad.CtrEntidadPanel>().ForEach(p => ControlarVaciosEntidad(p));
            if (!retornoGuardar)
            {
                MensajeGuardar = "Se detectaron campos vacios obligatorios: " + "\n" + MensajeGuardar;
                InformarError(MensajeGuardar);
            }
            
            return retornoGuardar;
        }
        private void ControlarVaciosEntidad(ControlPanelEntidad.CtrEntidadPanel c)
        {
            if (c.ControlarVacio)
            {
                if (!c.Controlarsihayvalores())
                {
                    retornoGuardar = false;
                    MensajeGuardar = MensajeGuardar + "\n" + c.TextoEtiqueta;
                }
            }
        }
       
        private void BlanquearEntidad(ControlPanelEntidad.CtrEntidadPanel c)
        {
            c.LimpiarCamposForzoso();
        }


        public virtual void ClickNuevo()
        {
            this.GetControls<ControlPanelEntidad.CtrEntidadPanel>().ForEach(p => BlanquearEntidad(p));
        }

        public virtual void ClickActualizar()
        {

        }

        /// <summary>
        /// Aca tenes que ingresar el codigo que toma el valor del texto de la lupita para ir a buscar los datos.
        /// </summary>
        public virtual void ActualizarTodo()
        {


        }
        public virtual void Informar(string texto)
        {

            //toolStripStatuslblBuscar.Text = texto;
            //MessageBox.Show(texto, NombreSistema,MessageBoxButtons.OK,MessageBoxIcon.Information);
            MessageBox.Show(texto, "DigitalOne", MessageBoxButtons.OK, MessageBoxIcon.Information);

            

        }

        public virtual void InformarError(string texto)
        {
            //toolStripStatuslblBuscar.Text = texto;
            //MessageBox.Show(texto, NombreSistema, MessageBoxButtons.OK, MessageBoxIcon.Error);
            MessageBox.Show(texto, "DigitalOne", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public virtual void Informar(string texto, string titulo)
        {
            //toolStripStatuslblBuscar.Text = texto;
             MessageBox.Show(texto, titulo);

        }
        public virtual void Informar(string texto, string titulo, MessageBoxButtons messageBoxButtons)
        {
            //toolStripStatuslblBuscar.Text = texto;
            drRespuesta =
            MessageBox.Show(texto, titulo, messageBoxButtons);
        }


        /// <summary>
        /// Aca tenes que ingresar el codigo que necesitas para cuando ingresas un valor erroneo el la lupita o para cuando queres blanquear todo el formulario
        /// </summary>
        public virtual void BlanquearTodo()
        {

        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Estas seguro que quiere Salir", "Mensaje al Usuario",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                ClickSalir();
                this.Close();
            }
        }

            private void toolStripButtonEliminar_Click(object sender, EventArgs e)
            {
                ClickEliminar();
            }

        private void toolStripButtonEditar_Click(object sender, EventArgs e)
        {
            ClickNuevo();
        }

        private void toolStripButtonGuardar_Click(object sender, EventArgs e)
        {
            ClickGuardar();
        }

        private void toolStripButtonActualizar_Click(object sender, EventArgs e)
        {
            ClickActualizar();
        }

        private void timerPanel_Tick(object sender, EventArgs e)
        {

        }            
    }
}

