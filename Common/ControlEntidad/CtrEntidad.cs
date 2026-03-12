using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Acceso.Clases.Datos.Generales;
using Global.Datos;

namespace Acceso.ControlEntidad
{
    public partial class CtrEntidad : UserControl
    {
        #region Declaracion de Propiedades


        /// <summary>
        /// Estos campos que siguen tiene que ser en un futuro un objeto de la entidad que tenga todas las propiedades llenas
        /// ahora le pongo dos textos adicionales para zafar y donde Campo es la columna y Valor es el Valor que tomo
        /// Lo uso en BuscarRegistroElegido()
        /// </summary>


        /// <summary>
        /// Nombre del campo auxiliar que se quiere tener cuando se ejecuta el metodo BuscarRegistroElegido
        /// </summary>
        [DescriptionAttribute("Nombre del campo auxiliar 1 que se quiere tener cuando se ejecuta el metodo BuscarRegistroElegido"), Category("DigitalOne")]
        public string AuxCampo1 { get; set; }

        /// <summary>
        /// Nombre del campo auxiliar que se quiere tener cuando se ejecuta el metodo BuscarRegistroElegido
        /// </summary>
        [DescriptionAttribute("Nombre del campo auxiliar 2 que se quiere tener cuando se ejecuta el metodo BuscarRegistroElegido"), Category("DigitalOne")]
        public string AuxCampo2 { get; set; }

        public string AuxValor1;
        public string AuxValor2;

        private string _VariableGlobalMascara;
        /// <summary>
        /// Nombre de la variable global que contiene la mascara del codigo
        /// </summary>
        [DescriptionAttribute("Nombre de la variable global que contiene la mascara del codigo"), Category("DigitalOne")]
        public string VariableGlobalMascara
        {
            get { return _VariableGlobalMascara; }
            set
            {
                _VariableGlobalMascara = value;
                //ConfigurarControlMascara();
            }
        }


        //Gus 14/04/2021 - Propiedad que indica si hay que controlar antes de guardar si 
        // esta lleno el control o no, la idea es que en el FormularioBase, recorra este tipo de controles
        // se fije si tiene esta propiedad activa y que controle si esta lleno, si no esta lleno, 
        // personalizar el mensaje de texto diciendo que el control.nombre o algo asi esta vaci
        private bool _ControlarVacio;
        /// <summary>
        /// Si estra en True el formularioBase va a controlar si el campo esta lleno antes de Guardar
        /// </summary>
        [DescriptionAttribute("Si esta en True el formulariobase va a controlar si el control esta lleno"), Category("DigitalOne")]
        public bool ControlarVacio
        {
            get { return _ControlarVacio; }
            set { _ControlarVacio = value; }
        }


        private bool _ActualizaenFormulario;
        /// <summary>
        /// Usarlo para cuando esta entidad no sea la primaria en el formulario y no querramos que cada vez
        /// que se cambie el valor se inicialicen todos los controles restantes que se 
        /// manejan desde el método AcutalizarTodo
        /// </summary>
        [DescriptionAttribute("Usarlo para cuando no se quiera correr el Metodo ActualizarTodo"), Category("DigitalOne")]
        public bool ActualizaenFormulario
        {
            get { return _ActualizaenFormulario; }
            set { _ActualizaenFormulario = value; }
        }


        private string _TextoEtiqueta = " ";

        /// <summary>
        /// Valor que toma la etiqueta
        /// </summary>
        [DescriptionAttribute("Etiqueta que se llena con Lineas max 20 caracteres"), Category("DigitalOne")]
        public string TextoEtiqueta
        {
            get { return _TextoEtiqueta; }
            set
            {
                _TextoEtiqueta = value;
                ConfigurarControl();
            }
        }


        private string _TituloAyuda;
        /// <summary>
        /// Titulo que tomara el formulario de Ayuda
        /// </summary>
        [DescriptionAttribute("Titulo que tomara el formulario de Ayuda"), Category("DigitalOne")]
        public string TituloAyuda
        {
            get { return _TituloAyuda; }
            set
            {
                _TituloAyuda = value;
                ConfigurarControl();
            }
        }

        private string _SqlAyuda;
        /// <summary>
        /// Query SQL que va a llenar la informacion de la lupa
        /// </summary>
        [DescriptionAttribute("Query SQL que va a llenar la informacion de la lupa"), Category("DigitalOne")]
        public string SqlAyuda
        {
            get { return _SqlAyuda; }
            set
            {
                _SqlAyuda = value;
            }
        }

        private string _TablaSQL;
        /// <summary>
        /// Tabla donde se obtienen los datos una vez seleccionado el ID desde la lupita
        /// </summary>
        [DescriptionAttribute("Tabla donde se obtienen los datos una vez seleccionado el ID desde la lupita"), Category("DigitalOne")]
        public string TablaSQL
        {
            get { return _TablaSQL; }
            set { _TablaSQL = value; }
        }

        private string _IDSQLWhere;
        /// <summary>
        /// ID clave para ejecutar el Where, es el dato que se obtiene desde la primer columna de la grid de ayuda
        /// </summary>
        [DescriptionAttribute("ID clave para ejecutar el Where, es el dato que se obtiene desde la primer columna de la grid de ayuda"), Category("DigitalOne")]
        public string IDSQLWhere
        {
            get { return _IDSQLWhere; }
            set { _IDSQLWhere = value; }
        }

        private string _IDSQLWherePK;
        /// <summary>
        /// Campo Primary Key de la Tabla
        /// </summary>
        [DescriptionAttribute("Campo Pimary Key de la tabla "), Category("DigitalOne")]
        public string IDSQLWherePK
        {
            get { return _IDSQLWherePK; }
            set { _IDSQLWherePK = value; }
        }

        private string _DESCSQLFrom;
        /// <summary>
        /// Campo que se debe ir a buscar a la tabla para que se llene el texto de Descripcion
        /// </summary>
        [DescriptionAttribute("Campo que se debe ir a buscar a la tabla para que se llene el texto de Descripcion"), Category("DigitalOne")]
        public string DESCSQLFrom
        {
            get { return _DESCSQLFrom; }
            set { _DESCSQLFrom = value; }
        }


        private bool _ConTitulo = true;
        /// <summary>
        /// True: Muestra Titulo, False: Oculta titulo y achica relocaliza los controles
        /// </summary>
        [DescriptionAttribute("True: Muestra Titulo, False: Oculta titulo y achica relocaliza los controles"), Category("DigitalOne")]
        public bool ConTitulo
        {
            get { return _ConTitulo; }
            set { _ConTitulo = value; }
        }

        private bool _GeneraNuevo = false;
        /// <summary>
        /// True: Si es Verdadero, la descripcion esta habilitada para llenar un registro nuevo
        /// </summary>
        [DescriptionAttribute("Si es Verdadero, la descripcion esta habilitada para llenar un registro nuevo"), Category("DigitalOne")]
        public bool GeneraNuevo
        {
            get { return _GeneraNuevo; }
            set { _GeneraNuevo = value; }
        }

        private int _IDValor;
        /// <summary>
        /// Valor del Primary Key de la Entidad
        /// </summary>
        [DescriptionAttribute("Valor del Primary Key de la Entidad"), Category("DigitalOne")]
        public int IDValor
        {
            get { return _IDValor; }
            set { _IDValor = value; }
        }


        private string _EntidadCodigo;
        /// <summary>
        /// Valor de clabe alfanumerico que tiene el control
        /// </summary>
        public string EntidadCodigo
        {
            get { return _EntidadCodigo; }
            set { _EntidadCodigo = value; }
        }


        private string _EntidadDescripcion;
        /// <summary>
        /// Valor de la descripcion del control
        /// </summary>
        public string EntidadDecripcion
        {
            get { return _EntidadDescripcion; }
            set { _EntidadDescripcion = value; }
        }


        private bool _MostrarID;
        /// <summary>
        /// Activando esta propiedad se va a mostrar en chiquito el id del codigo para la base de datos
        /// </summary>
        [DescriptionAttribute("Activando esta propiedad se va a mostrar en chiquito el id del codigo para la base de datos"), Category("DigitalOne")]
        public bool MostrarID
        {
            get { return _MostrarID; }
            set
            {
                _MostrarID = value;
                etiquetaID.Visible = _MostrarID;
            }
        }

        private bool _SinDescripcion;
        /// <summary>
        /// Activando esta propiedad el control se reduce al codigo sin la descripcion
        /// </summary>
        [DescriptionAttribute("Activando esta propiedad se reduce el control hata ver solo el codigo"), Category("DigitalOne")]
        public bool SinDescripcion
        {
            get { return _SinDescripcion; }
            set
            {
                _SinDescripcion = value;
                ConfigurarControl();
            }
        }

        private bool _AutoGenerarCodigo;
        /// <summary>
        /// Activando esta propiedad el control se reduce al codigo sin la descripcion
        /// </summary>
        [DescriptionAttribute("Activando esta propiedad se desabilita la edicion del codigo y se genera solo"), Category("DigitalOne")]
        public bool AutoGenerarCodigo
        {
            get { return _AutoGenerarCodigo; }
            set
            {
                _AutoGenerarCodigo = value;
                ConfigurarControl();
            }

        }

        private bool _SinImagen;
        /// <summary>
        /// Activando esta propiedad se incluye una imagen al control
        /// </summary>
        [DescriptionAttribute("Activando esta propiedad se se puede incluir el logo"), Category("DigitalOne")]
        public bool SinImagen
        {
            get { return _SinImagen; }
            set
            {
                _SinImagen = value;
                ConfigurarControl();
            }
        }

        private bool _esNumerico;
        /// <summary>
        /// Acitvarlos si queres que el control controle si el valor ingresado es numerico en el leave
        /// </summary>
        public bool esNumerico
        {
            get { return _esNumerico; }
            set { _esNumerico = value; }
        }

        #endregion

        public event EventHandler BotonBusquedaEspecial;
        public FrmControlEntidadSimpleBusqueda oFormBusqueda;
        System.Drawing.Point point = new Point();
        System.Drawing.Size size = new Size();




        private bool _BusquedaAvanzada;
        /// <summary>
        /// Es verdadero si el control es heredado, sino esta en falso. Usado para el resize
        /// </summary>
        [DescriptionAttribute("Habilita el boton de busqueda avanzada"), Category("DigitalOne")]
        public bool BusquedaAvanzada
        {
            get { return _BusquedaAvanzada; }
            set
            {
                _BusquedaAvanzada = value;
                botonBusquedaAdicional.Visible = value;
            }
        }

        bool BusquedaEspecial;


        public CtrEntidad()
        {

            InitializeComponent();
            ConfigurarControl();
        }

        private void ConfigurarControl()
        {
            lblLinkEntidad.Text = _TextoEtiqueta;

            //Con o sin etiqueta
            if (_ConTitulo) lblLinkEntidad.Visible = true;
            else lblLinkEntidad.Visible = false;

            if (_GeneraNuevo) textoDescripcion.Enabled = true;
            else textoDescripcion.Enabled = false;


            if (_SinImagen) this.picture.Visible = false;
            else this.picture.Visible = true;

            if (AutoGenerarCodigo)
                textoCodigo.Visible = false;
            else
            {
                textoCodigo.Visible = true;
            }


        }
        // Gustavo 13/04/2021 - para usar en el autogeneracodigo en el guardar del control heredado
        public int VariableGlobalLen;
        protected virtual void ConfigurarControlMascara()
        {
            ////Cantidad de Digitos 
            ////-------------------------------------------------------------------------------
            if (_VariableGlobalMascara == string.Empty) return;

            string variable = GRALVariablesGlobales.TraerValorDataBase(_VariableGlobalMascara);
            if (variable == string.Empty) return;

            VariableGlobalLen = int.Parse(variable);
            textoCodigo.MaxLength = VariableGlobalLen;
            /////////////////////////////////////////////////////////////////////////////////////////////


            ////int RegistrosMax  = int.Parse(Acceso.Clases.Datos.Generales.GRALVariablesGlobalesServicios.TraerValor("CTRLRegistrosMaximosLupita").ToString());
            ////int RegistrosReal = Acceso.Clases.Datos.Generales.GRALVariablesDinamicasDTO.RegistrosIV00101;

        }

        /// <summary>
        /// Limpiar los campos del control, se llama del formulario funciona si la propidad ActualizarFormulario esta en true
        /// </summary>
        public void LimpiarCampos()
        {
            if (!_ActualizaenFormulario)
            {
                limpiar();
            }
        }
        /// <summary>
        /// Limpiar los campos del control, se llama del formulario limpia siempre
        /// </summary>
        public void LimpiarCamposForzoso()
        {
            limpiar();
        }

        private void limpiar()
        {
            this.textoCodigo.Text = string.Empty;
            this.textoDescripcion.Text = string.Empty;
            this.etiquetaID.Text = "0";
            _IDValor = 0;
            _EntidadCodigo = string.Empty;
            _EntidadDescripcion = string.Empty;
        }
        private void PasarDatos()
        {
            if (null != this.Parent)
            {
                try
                {
                    Form oPar = this.FindForm();

                    if (oPar.GetType().BaseType.Name == "FrmBaseFormulario")
                    {
                        FrmBaseFormulario oParent = (FrmBaseFormulario)this.FindForm();
                        if (null != oParent)
                        {

                            oParent.ocEnt = this;

                            if (_ActualizaenFormulario)
                            {

                                oParent.ActualizarTodo();
                            }
                        }
                    }
                    if (oPar.GetType().BaseType.Name == "FrmBaseReportes")
                    {
                        FrmBaseReportes oParentReport = (FrmBaseReportes)this.FindForm();
                        if (null != oParentReport)
                        {

                            oParentReport.ocEnt = this;

                            if (_ActualizaenFormulario)
                            {

                                oParentReport.ActualizarTodo();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    FrmBaseFormulario oParent = (FrmBaseFormulario)this.FindForm();
                    if (null != oParent)
                    {
                        if (_ActualizaenFormulario) oParent.ActualizarTodo();
                    }
                }
            }
        }

        private void textoCodigo_Leave(object sender, EventArgs e)
        {
            textoDescripcion.Text = string.Empty;
            etiquetaID.Text = "0";
            _IDValor = 0;
            _EntidadCodigo = string.Empty;
            _EntidadDescripcion = string.Empty;


            BuscarRegistroElegido(textoCodigo.Text.Trim());
            this.PasarDatos();
        }
        public void BuscarRegistroElegido(string codigo)
        {
            var dt = new DataTable();

            //gustvo 6/3/2021
            textoDescripcion.Text = string.Empty;

            if (string.IsNullOrEmpty(_TablaSQL) || string.IsNullOrEmpty(_IDSQLWhere)) return;

            string consulta = "Select * from " + _TablaSQL + " WHERE " + _IDSQLWhere + " = '" + codigo + "'"
                + AppendEmpresaIdFilter(_TablaSQL);
            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(consulta);
                if (dt.Rows.Count == 0)
                {
                    BlanquearDatos();
                    return;
                }
                /// Gustavo 23/03/2021 agregando el parametro no hace falta llenar el codigo
                textoCodigo.Text = codigo;
                _EntidadCodigo = codigo;
                //////////////////////////////////////////////////////////////////////////////

                textoDescripcion.Text = dt.Rows[0][_DESCSQLFrom].ToString().Trim();
                _EntidadDescripcion = textoDescripcion.Text.Trim();

                etiquetaID.Text = dt.Rows[0][0].ToString().Trim();

                /// Para el caso que el pk sea un strin (Ej las vistas de gp)
                int numero = default(int);
                bool bln = int.TryParse(dt.Rows[0][0].ToString(), out numero);

                if (bln)
                    _IDValor = int.Parse(dt.Rows[0][0].ToString());

                //Gustavo 14/03/2021
                // LLeno los campos Auxiliares si se necesitan
                if (AuxCampo1 != null)
                {
                    if (AuxCampo1 != string.Empty)
                        AuxValor1 = dt.Rows[0][AuxCampo1].ToString();
                }
                if (AuxCampo2 != null)
                {
                    if (AuxCampo2 != string.Empty)
                        AuxValor2 = dt.Rows[0][AuxCampo2].ToString();
                }
                // Fin Gustavo 14/03/2021


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BlanquearDatos()
        {

            if (null != this.Parent)
            {
                try
                {
                    FrmBaseFormulario oParent = (FrmBaseFormulario)this.FindForm();

                    if (null != oParent)
                    {
                        oParent.ocEnt = this;

                        if (_ActualizaenFormulario)
                            oParent.BlanquearTodo();
                    }
                }
                catch (Exception)
                {

                    FrmBaseFormulario oParent = (FrmBaseFormulario)this.FindForm();


                    if (null != oParent)
                    {

                        if (_ActualizaenFormulario)
                            oParent.BlanquearTodo();
                    }
                }
            }
        }
        /// <summary>
        /// Llamar a este metodo para cuando se tenga el key en el formulario y se necesiten los datos del control
        /// </summary>
        /// <param name="codigo">Primary key de la tabla</param>
        public void BuscarRegistroElegido(int codigo)
        {
            DataTable dt = new DataTable();

            try
            {

                string comando = "Select * from " + _TablaSQL + " WHERE " + IDSQLWherePK + " = " + codigo
                    + AppendEmpresaIdFilter(_TablaSQL);
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(comando);

                if (dt.Rows.Count == 0) return;

                textoCodigo.Text = dt.Rows[0][_IDSQLWhere].ToString();
                textoDescripcion.Text = dt.Rows[0][_DESCSQLFrom].ToString();
                etiquetaID.Text = dt.Rows[0][0].ToString();
                _IDValor = int.Parse(dt.Rows[0][0].ToString());

                _EntidadCodigo = textoCodigo.Text.Trim();
                _EntidadDescripcion = textoDescripcion.Text.Trim();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textoCodigo_Enter(object sender, EventArgs e)
        {
            ConfigurarControlMascara();
        }
        private void AbrirVentanaAyuda()
        {
            //Gustavo 2/2/2021 --- Volver a incluir sysGlobales desde una copia sin EF
            //         if (BusquedaEspecial &&
            //                      textoBusqueda.Text.Trim().Length < sysGlobales.DigitosBusquedaControlEntidadTexto)
            //{
            //  SalirdeFoco();
            //  return;
            //}


            FrmAyudaEntidadSimple oAyuda = new FrmAyudaEntidadSimple();
            oAyuda.Titulo = _TituloAyuda;


            var dt = new DataTable();
            string consulta;

            try
            {
                int empresaId = TenantContext.EmpresaId;

                if (BusquedaEspecial)
                {
                    consulta = "Select " + IDSQLWhere + ", " + DESCSQLFrom + " from " + _TablaSQL + " WHERE (" + IDSQLWhere + " like '%" +
                                textoBusqueda.Text.Trim() + "%' or " + DESCSQLFrom + " like '%" + textoBusqueda.Text.Trim() + "%')"
                                + AppendEmpresaIdFilter(_TablaSQL);

                    dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(consulta);

                    BusquedaEspecial = false;
                }
                else
                {
                    string sqlAyuda = _SqlAyuda;
                    if (!string.IsNullOrEmpty(sqlAyuda) && sqlAyuda.TrimEnd().EndsWith("="))
                        sqlAyuda += " " + empresaId;
                    dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(sqlAyuda);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            oAyuda.bindingSource.DataSource = dt;



            DialogResult res = oAyuda.ShowDialog();
            if (res == DialogResult.OK)
            {
                textoCodigo.Text = oAyuda.IDElegido;
                BuscarRegistroElegido(textoCodigo.Text);

                PasarDatos();
            }
            oAyuda.advancedDataGridView.ClearFilter();
        }

        private void lblLinkEntidad_Click(object sender, EventArgs e)
        {
            AbrirVentanaAyuda();
        }

        private void lblLinkEntidad_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void lblLinkEntidad_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void lblLinkEntidad_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (this.textoBusqueda.Size.Width > 10)
            {
                //Lo vuelvo a dejar como estaba
                InicializarTextoBusqueda();
                return;
            }
            else
            {
                textoBusqueda.Enabled = true;

                point.X = textoBusqueda.Location.X;
                point.Y = textoBusqueda.Location.Y;

                size.Height = textoBusqueda.Height;
                size.Width = textoBusqueda.Width;

                textoBusqueda.Location = textoDescripcion.Location;
                textoBusqueda.Size = textoDescripcion.Size;
                textoBusqueda.Focus();
                textoBusqueda.Text = "ingrese texto a buscar";


            }
        }



        private void textoBusqueda_Enter(object sender, EventArgs e)
        {

            this.textoBusqueda.Text = string.Empty;
            this.textoDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        private void InicializarTextoBusqueda()
        {
            //Lo vuelvo a dejar en el tamaño original
            textoBusqueda.Size = size;
            textoBusqueda.Location = point;
        }

        private void textoBusqueda_Leave(object sender, EventArgs e)
        {
            SalirdeFoco();

            //Lo vuelvo a dejar en el tamaño original
            InicializarTextoBusqueda();

            //Llamo a la lupa para que me traiga los resultados
            BusquedaEspecial = true;
            AbrirVentanaAyuda();
        }

        private void SalirdeFoco()
        {
            //Lo deshabilito
            textoBusqueda.Enabled = false;

            // vuelvo a dejar la letra en gris e italic
            this.textoBusqueda.Font =  new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoBusqueda.ForeColor = System.Drawing.SystemColors.ScrollBar;
        }

        private void botonBusquedaAdicional_Click(object sender, EventArgs e)
        {
            //controlador de eventos
            if (this.BotonBusquedaEspecial != null)
            {
                oFormBusqueda = new FrmControlEntidadSimpleBusqueda();
                this.BotonBusquedaEspecial(this, e);
                oFormBusqueda.ShowDialog();
            }
        }

        /// <summary>
        /// Esta funcion devuelve true si los campos codio y descripcion estan llenos o no son string.empty
        /// </summary>
        /// <returns></returns>
        public bool Controlarsihayvalores()
        {
            bool lControl = this.textoCodigo.Text != string.Empty & this.textoDescripcion.Text != string.Empty;
            if (lControl)
                return true;
            else
            {
                return false;
            }

        }

        private void textoBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textoBusqueda.Text.Trim() == "ingrese texto a buscar")
                textoBusqueda.Text = string.Empty;
        }

        private static string AppendEmpresaIdFilter(string tablaSQL)
        {
            if (string.IsNullOrEmpty(tablaSQL)) return "";
            if (tablaSQL.Contains("(")) return "";
            string tablaUpper = tablaSQL.Trim().ToUpperInvariant();
            if (tablaUpper == "LEGAJO" || tablaUpper == "SUCURSAL" || tablaUpper == "CATEGORIA" ||
                tablaUpper == "HORARIO" || tablaUpper == "SECTOR" || tablaUpper == "INCIDENCIA" ||
                tablaUpper == "FERIADO" || tablaUpper == "EMPRESA")
            {
                return " AND EmpresaId = " + TenantContext.EmpresaId;
            }
            return "";
        }
    }
}
