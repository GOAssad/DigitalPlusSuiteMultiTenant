using Acceso.ControlEntidad;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Global.Datos;

namespace Acceso.ControlPanelEntidad

{
    public partial class CtrEntidadPanel : UserControl
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
            get => _ControlarVacio;
            set => _ControlarVacio = value;
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
            get => _ActualizaenFormulario;
            set => _ActualizaenFormulario = value;
        }


        private string _TextoEtiqueta = string.Empty;

        /// <summary>
        /// Valor que toma la etiqueta
        /// </summary>
        [DescriptionAttribute("Etiqueta que se llena con Lineas max 20 caracteres"), Category("DigitalOne")]
        public string TextoEtiqueta
        {
            get => _TextoEtiqueta;
            set
            {
                _TextoEtiqueta = value;
                ConfigurarControl();
            }
        }

        private string _Mascara = string.Empty;

        /// <summary>
        /// Valor que toma la etiqueta
        /// </summary>
        [DescriptionAttribute("Mascara para el cuadro de codigo"), Category("DigitalOne")]
        public string Mascara
        {
            get => _Mascara;
            set
            {
                _Mascara = value;
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
            get => _TituloAyuda;
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
            get => _SqlAyuda;
            set => _SqlAyuda = value;
        }

        private string _TablaSQL;
        /// <summary>
        /// Tabla donde se obtienen los datos una vez seleccionado el ID desde la lupita
        /// </summary>
        [DescriptionAttribute("Tabla donde se obtienen los datos una vez seleccionado el ID desde la lupita"), Category("DigitalOne")]
        public string TablaSQL
        {
            get => _TablaSQL;
            set => _TablaSQL = value;
        }

        private string _IDSQLWhere;
        /// <summary>
        /// ID clave para ejecutar el Where, es el dato que se obtiene desde la primer columna de la grid de ayuda
        /// </summary>
        [DescriptionAttribute("ID clave para ejecutar el Where, es el dato que se obtiene desde la primer columna de la grid de ayuda"), Category("DigitalOne")]
        public string IDSQLWhere
        {
            get => _IDSQLWhere;
            set => _IDSQLWhere = value;
        }

        private string _IDSQLWherePK;
        /// <summary>
        /// Campo Primary Key de la Tabla
        /// </summary>
        [DescriptionAttribute("Campo Pimary Key de la tabla "), Category("DigitalOne")]
        public string IDSQLWherePK
        {
            get => _IDSQLWherePK;
            set => _IDSQLWherePK = value;
        }

        private string _DESCSQLFrom;
        /// <summary>
        /// Campo que se debe ir a buscar a la tabla para que se llene el texto de Descripcion
        /// </summary>
        [DescriptionAttribute("Campo que se debe ir a buscar a la tabla para que se llene el texto de Descripcion"), Category("DigitalOne")]
        public string DESCSQLFrom
        {
            get => _DESCSQLFrom;
            set => _DESCSQLFrom = value;
        }



        private bool _GeneraNuevo = false;
        /// <summary>
        /// True: Si es Verdadero, la descripcion esta habilitada para llenar un registro nuevo
        /// </summary>
        [DescriptionAttribute("Si es Verdadero, la descripcion esta habilitada para llenar un registro nuevo"), Category("DigitalOne")]
        public bool GeneraNuevo
        {
            get => _GeneraNuevo;
            set => _GeneraNuevo = value;
        }

        private int _IDValor;
        /// <summary>
        /// Valor del Primary Key de la Entidad
        /// </summary>
        [DescriptionAttribute("Valor del Primary Key de la Entidad"), Category("DigitalOne")]
        public int IDValor
        {
            get => _IDValor;
            set => _IDValor = value;
        }


        private string _ValorCodigo;
        /// <summary>
        /// Valor de clabe alfanumerico que tiene el control
        /// </summary>
        [DescriptionAttribute("Valor del Campo Codigo"), Category("DigitalOneValores")]
        public string ValorCodigo
        {
            get => _ValorCodigo;
            set
            {
                _ValorCodigo = value;
                AsignarValor();
            }
        }


        private string _ValorDecripcion;
        /// <summary>
        /// Valor de la descripcion del control
        /// </summary>
        [DescriptionAttribute("Valor del Campo Decripcion"), Category("DigitalOneValores")]
        public string EntidadDecripcion
        {
            get => _ValorDecripcion;
            set => _ValorDecripcion = value;
        }


        private bool _MostrarID;
        /// <summary>
        /// Activando esta propiedad se va a mostrar en chiquito el id del codigo para la base de datos
        /// </summary>
        [DescriptionAttribute("Activando esta propiedad se va a mostrar en chiquito el id del codigo para la base de datos"), Category("DigitalOne")]
        public bool MostrarID
        {
            get => _MostrarID;
            set
            {
                _MostrarID = value;
                etiquetaID.Visible = _MostrarID;
            }
        }

        private FontAwesome.Sharp.IconChar _iconoFontAwesome;
        [DescriptionAttribute("Icono"), Category("DigitalOne")]

        public FontAwesome.Sharp.IconChar iconoFontAwesome
        {
            get => _iconoFontAwesome;
            set
            {
                _iconoFontAwesome = value;
                ConfigurarControl();
            }
        }

        private Color _iconoFontAwesomeColor;
        [DescriptionAttribute("Color del Icono"), Category("DigitalOne")]

        public Color iconoFontAwesomeColor
        {
            get => _iconoFontAwesomeColor;
            set
            {
                _iconoFontAwesomeColor = value;
                ConfigurarControl();
            }
        }


        //Gustavo 6/6/2021 - Para Forms modale, lo hago readonly y escondo la lupita
        private bool _SoloLectura;
        [DescriptionAttribute("En true pone los 2 controles visuales de solo lectura y oculta la lupita y los buscadores"), Category("DigitalOne")]

        public bool SoloLectura
        {
            get => _SoloLectura;
            set
            {
                _SoloLectura = value;
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
            get => _AutoGenerarCodigo;
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
            get => _SinImagen;
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
            get => _esNumerico;
            set => _esNumerico = value;
        }

        #endregion

        public event EventHandler BotonBusquedaEspecial;
        public FrmControlEntidadSimpleBusqueda oFormBusqueda;
        private System.Drawing.Point point = new Point();
        private System.Drawing.Size size = new Size();


        private bool _BusquedaAvanzada;
        /// <summary>
        /// Es verdadero si el control es heredado, sino esta en falso. Usado para el resize
        /// </summary>
        [DescriptionAttribute("Habilita el boton de busqueda avanzada"), Category("DigitalOne")]
        public bool BusquedaAvanzada
        {
            get => _BusquedaAvanzada;
            set
            {
                _BusquedaAvanzada = value;
                botonBusquedaAdicional.Visible = value;
            }
        }

        private bool BusquedaEspecial;

        public CtrEntidadPanel()
        {
            InitializeComponent();
            ConfigurarControl();
        }
        private void ConfigurarControl()
        {
            lblLinkEntidad.Text = _TextoEtiqueta;

            /// Gustavo 6/6/2021, lo pongo arriba porque si esta propiedad esta en verdadero, la prorp GeneraNuevo
            /// tambien tiene que ser falso, ya que esta propiedad desabilita el campo textDescripcion
            if (_SoloLectura)
            {
                textoCodigo.Enabled = false;
                _GeneraNuevo = false; //fuezo porque tiene que estar deseable.
                lblLinkEntidad.Visible = false;
                textoBusqueda.Visible = false;
                btnBusqueda.Visible = false;
                botonBusquedaAdicional.Visible = false;

            }
            else
            {
                //si no es read only solo activo el textoCodigo y las busquedas, el resto tienen prop individual
                //por eso es que no igualo las propiedades con el valor de _SoloLectura
                textoCodigo.Enabled = true;
                lblLinkEntidad.Visible = true;
                textoBusqueda.Visible = true;
                btnBusqueda.Visible = true;
            }

            if (_GeneraNuevo)
            {
                textoDescripcion.Enabled = true;
            }
            else
            {
                textoDescripcion.Enabled = false;
            }

            if (_SinImagen)
            {
                picture.Visible = false;
            }
            else
            {
                picture.Visible = true;
            }

            if (AutoGenerarCodigo)
            {
                textoCodigo.Visible = false;
            }
            else
            {
                textoCodigo.Visible = true;
            }

            if (_iconoFontAwesome != FontAwesome.Sharp.IconChar.None)
            {
                picture.IconChar = _iconoFontAwesome;
            }

            if (_iconoFontAwesomeColor != Color.Empty)
            {
                picture.IconColor = _iconoFontAwesomeColor;
            }

            textoCodigo.Mask = _Mascara;



        }


        // Gustavo 13/04/2021 - para usar en el autogeneracodigo en el guardar del control heredado
        public int VariableGlobalLen;

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
            textoCodigo.Text = string.Empty;
            textoDescripcion.Text = string.Empty;
            etiquetaID.Text = "0";
            _IDValor = 0;
            _ValorCodigo = string.Empty;
            _ValorDecripcion = string.Empty;
        }
        private void PasarDatos()
        {
            if (null != Parent)
            {
                try
                {
                    Form oPar = FindForm();

                    if (oPar.GetType().BaseType.Name == "FrmBaseFormulario")
                    {
                        FrmBaseFormulario oParent = (FrmBaseFormulario)FindForm();
                        if (null != oParent)
                        {

                            oParent.ocPEnt = this;

                            if (_ActualizaenFormulario)
                            {

                                oParent.ActualizarTodo();
                            }
                        }
                    }
                    if (oPar.GetType().BaseType.Name == "FrmBaseReportes")
                    {
                        FrmBaseReportes oParentReport = (FrmBaseReportes)FindForm();
                        if (null != oParentReport)
                        {

                            oParentReport.ocPEnt = this;

                            if (_ActualizaenFormulario)
                            {

                                oParentReport.ActualizarTodo();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    FrmBaseFormulario oParent = (FrmBaseFormulario)FindForm();
                    if (null != oParent)
                    {
                        if (_ActualizaenFormulario)
                        {
                            oParent.ActualizarTodo();
                        }
                    }
                }
            }
        }

        private void textoCodigo_Leave(object sender, EventArgs e)
        {
            textoDescripcion.Text = string.Empty;
            etiquetaID.Text = "0";
            _IDValor = 0;
            _ValorCodigo = string.Empty;
            _ValorDecripcion = string.Empty;


            BuscarRegistroElegido(textoCodigo.Text.Trim());
            PasarDatos();
        }
        public void BuscarRegistroElegido(string codigo)
        {
            DataTable dt = new DataTable();

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
                _ValorCodigo = codigo;
                //////////////////////////////////////////////////////////////////////////////

                textoDescripcion.Text = dt.Rows[0][_DESCSQLFrom].ToString().Trim();
                _ValorDecripcion = textoDescripcion.Text.Trim();

                etiquetaID.Text = dt.Rows[0][0].ToString().Trim();

                /// Para el caso que el pk sea un strin (Ej las vistas de gp)
                bool bln = int.TryParse(dt.Rows[0][0].ToString(), out int numero);

                if (bln)
                {
                    _IDValor = int.Parse(dt.Rows[0][0].ToString());
                }

                //Gustavo 14/03/2021
                // LLeno los campos Auxiliares si se necesitan
                if (AuxCampo1 != null)
                {
                    if (AuxCampo1 != string.Empty)
                    {
                        AuxValor1 = dt.Rows[0][AuxCampo1].ToString();
                    }
                }
                if (AuxCampo2 != null)
                {
                    if (AuxCampo2 != string.Empty)
                    {
                        AuxValor2 = dt.Rows[0][AuxCampo2].ToString();
                    }
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

            if (null != Parent)
            {
                try
                {
                    Form oPar = FindForm();

                    if (oPar.GetType().BaseType.Name == "FrmBaseFormulario")
                    {
                        FrmBaseFormulario oParent = (FrmBaseFormulario)FindForm();
                        if (null != oParent)
                        {

                            oParent.ocPEnt = this;

                            if (_ActualizaenFormulario)
                            {

                                oParent.BlanquearTodo();
                            }
                        }
                    }
                    if (oPar.GetType().BaseType.Name == "FrmBaseReportes")
                    {
                        FrmBaseReportes oParentReport = (FrmBaseReportes)FindForm();
                        if (null != oParentReport)
                        {

                            oParentReport.ocPEnt = this;

                            if (_ActualizaenFormulario)
                            {
                                oParentReport.BlanquearTodo();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    FrmBaseFormulario oParent = (FrmBaseFormulario)FindForm();
                    if (null != oParent)
                    {
                        if (_ActualizaenFormulario)
                        {
                            oParent.ActualizarTodo();
                        }
                    }
                }
            }


            //if (null != this.Parent)
            //{

            //    try
            //    {
            //        FrmBaseFormulario oParent = (FrmBaseFormulario)this.FindForm();

            //        if (null != oParent)
            //        {
            //            oParent.ocPEnt = this;

            //            if (_ActualizaenFormulario)
            //                oParent.BlanquearTodo();
            //        }
            //    }
            //    catch (Exception)
            //    {

            //        FrmBaseFormulario oParent = (FrmBaseFormulario)this.FindForm();


            //        if (null != oParent)
            //        {

            //            if (_ActualizaenFormulario)
            //                oParent.BlanquearTodo();
            //        }
            //    }
            //}
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

                if (dt.Rows.Count == 0)
                {
                    return;
                }

                textoCodigo.Text = dt.Rows[0][_IDSQLWhere].ToString();
                textoDescripcion.Text = dt.Rows[0][_DESCSQLFrom].ToString();
                etiquetaID.Text = dt.Rows[0][0].ToString();
                _IDValor = int.Parse(dt.Rows[0][0].ToString());

                _ValorCodigo = textoCodigo.Text.Trim();
                _ValorDecripcion = textoDescripcion.Text.Trim();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textoCodigo_Enter(object sender, EventArgs e)
        {

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


            FrmAyudaEntidadSimple oAyuda = new FrmAyudaEntidadSimple
            {
                Titulo = _TituloAyuda
            };


            DataTable dt = new DataTable();
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
            textoBusqueda.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            textoBusqueda.ForeColor = System.Drawing.SystemColors.ScrollBar;
        }

        private void botonBusquedaAdicional_Click(object sender, EventArgs e)
        {
            //controlador de eventos
            if (BotonBusquedaEspecial != null)
            {
                oFormBusqueda = new FrmControlEntidadSimpleBusqueda();
                BotonBusquedaEspecial(this, e);
                oFormBusqueda.ShowDialog();
            }
        }

        /// <summary>
        /// Esta funcion devuelve true si los campos codio y descripcion estan llenos o no son string.empty
        /// </summary>
        /// <returns></returns>
        public bool Controlarsihayvalores()
        {
            bool lControl = textoCodigo.Text != string.Empty & textoDescripcion.Text != string.Empty;
            if (lControl)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            AbrirVentanaAyuda();
        }

        private void iconButton1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void iconButton1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void iconButton1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            if (textoBusqueda.Size.Width > 10)
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

        /// <summary>
        /// Gustvo 6/6/2021 para dejar de usar los textbox y tomar los valores de las propiedades
        /// </summary>
        private void AsignarValor()
        {
            textoCodigo.Text = _ValorCodigo;
            textoDescripcion.Text = _ValorDecripcion;
        }

        private void CtrEntidadPanel_Load(object sender, EventArgs e)
        {
            lblLinkEntidad.TabStop = false;
            btnBusqueda.TabStop = false;
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
