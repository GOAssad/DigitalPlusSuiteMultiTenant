namespace Acceso.Generales
{
	partial class FrmConfigurador
	{
		/// <summary>
		/// Variable del diseñador necesaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpiar los recursos que se estén usando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new Global.Controles.TabPagina();
            this.trackControlEntidadBusqueda = new Global.Controles.TrackNumericoCombo();
            this.trackSucursal = new Global.Controles.TrackNumericoCombo();
            this.TextoEmpresaRazonSocial = new Global.Controles.TextoSimple();
            this.etiquetaSimple1 = new Global.Controles.EtiquetaSimple();
            this.tabPageCompatibilidad = new Global.Controles.TabPagina();
            this.etiquetaSimple6 = new Global.Controles.EtiquetaSimple();
            this.numericoGPOpenYear = new Global.Controles.Numerico();
            this.etiquetaSimple4 = new Global.Controles.EtiquetaSimple();
            this.comboBasesDeDatos = new Global.Controles.ComboDesplegable();
            this.checkBoxManufacturingGP = new Global.Controles.CheckBox();
            this.panel2 = new Global.Controles.Panel();
            this.checkDynamics = new Global.Controles.CheckBox();
            this.checkSumma = new Global.Controles.CheckBox();
            this.tabPageInventario = new Global.Controles.TabPagina();
            this.panel3 = new Global.Controles.Panel();
            this.comboSeparadorSucursal = new Global.Controles.ComboDesplegable();
            this.comboSeparadorTipo = new Global.Controles.ComboDesplegable();
            this.textoMascaraDocumento = new Global.Controles.TextoSimple();
            this.checkTipoDocumento = new Global.Controles.CheckBox();
            this.checkDocumentoSucursal = new Global.Controles.CheckBox();
            this.trackNumericoDigitosDocumento = new Global.Controles.TrackNumericoCombo();
            this.etiquetaSimple5 = new Global.Controles.EtiquetaSimple();
            this.trackDeposito = new Global.Controles.TrackNumericoCombo();
            this.trackDigitosTipoMovimientoInventario = new Global.Controles.TrackNumericoCombo();
            this.trackDigitosCodigoClaseArticulo = new Global.Controles.TrackNumericoCombo();
            this.trackDigitosCodigoArticulo = new Global.Controles.TrackNumericoCombo();
            this.tabPageRRHH = new Global.Controles.TabPagina();
            this.etiquetaSimple3 = new Global.Controles.EtiquetaSimple();
            this.fechaDesdeHastaFeriados = new Global.Controles.FechayHora.FechaDesdeHasta();
            this.trackDigitosCodigoIncidencias = new Global.Controles.TrackNumericoCombo();
            this.trackDigitosCodigoTurnos = new Global.Controles.TrackNumericoCombo();
            this.tabPageControles = new Global.Controles.TabPagina();
            this.etiquetaSimple2 = new Global.Controles.EtiquetaSimple();
            this.numericoMaxRegLupita = new Global.Controles.Numerico();
            this.tabPageContable = new Global.Controles.TabPagina();
            this.trackDigitoCodigoTipo = new Global.Controles.TrackNumericoCombo();
            this.tabControl.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageCompatibilidad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericoGPOpenYear)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabPageInventario.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPageRRHH.SuspendLayout();
            this.tabPageControles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericoMaxRegLupita)).BeginInit();
            this.SuspendLayout();
            // 
            // botonMenuGuardar
            // 
            this.botonMenuGuardar.Click += new System.EventHandler(this.botonMenuGuardar_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl.Controls.Add(this.tabPageGeneral);
            this.tabControl.Controls.Add(this.tabPageCompatibilidad);
            this.tabControl.Controls.Add(this.tabPageInventario);
            this.tabControl.Controls.Add(this.tabPageRRHH);
            this.tabControl.Controls.Add(this.tabPageControles);
            this.tabControl.Controls.Add(this.tabPageContable);
            this.tabControl.Location = new System.Drawing.Point(0, 59);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1118, 605);
            this.tabControl.TabIndex = 4;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPageGeneral.Controls.Add(this.trackControlEntidadBusqueda);
            this.tabPageGeneral.Controls.Add(this.trackSucursal);
            this.tabPageGeneral.Controls.Add(this.TextoEmpresaRazonSocial);
            this.tabPageGeneral.Controls.Add(this.etiquetaSimple1);
            this.tabPageGeneral.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 28);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(1110, 573);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            // 
            // trackControlEntidadBusqueda
            // 
            this.trackControlEntidadBusqueda.Location = new System.Drawing.Point(253, 99);
            this.trackControlEntidadBusqueda.Maximo = 10;
            this.trackControlEntidadBusqueda.Minimo = 0;
            this.trackControlEntidadBusqueda.Name = "trackControlEntidadBusqueda";
            this.trackControlEntidadBusqueda.Size = new System.Drawing.Size(215, 100);
            this.trackControlEntidadBusqueda.TabIndex = 3;
            this.trackControlEntidadBusqueda.Titulo = "Min. Ctrl. Digitos Texto";
            this.trackControlEntidadBusqueda.Valor = 0;
            this.trackControlEntidadBusqueda.Load += new System.EventHandler(this.trackNumericoCombo1_Load);
            // 
            // trackSucursal
            // 
            this.trackSucursal.Location = new System.Drawing.Point(32, 99);
            this.trackSucursal.Maximo = 32;
            this.trackSucursal.Minimo = 0;
            this.trackSucursal.Name = "trackSucursal";
            this.trackSucursal.Size = new System.Drawing.Size(215, 100);
            this.trackSucursal.TabIndex = 3;
            this.trackSucursal.Titulo = "Ancho Max. Sucursales";
            this.trackSucursal.Valor = 0;
            this.trackSucursal.Load += new System.EventHandler(this.trackSucursal_Load);
            // 
            // TextoEmpresaRazonSocial
            // 
            this.TextoEmpresaRazonSocial.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextoEmpresaRazonSocial.Location = new System.Drawing.Point(225, 35);
            this.TextoEmpresaRazonSocial.Name = "TextoEmpresaRazonSocial";
            this.TextoEmpresaRazonSocial.Size = new System.Drawing.Size(693, 32);
            this.TextoEmpresaRazonSocial.TabIndex = 1;
            // 
            // etiquetaSimple1
            // 
            this.etiquetaSimple1.AutoSize = true;
            this.etiquetaSimple1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple1.Location = new System.Drawing.Point(27, 38);
            this.etiquetaSimple1.Name = "etiquetaSimple1";
            this.etiquetaSimple1.Size = new System.Drawing.Size(123, 29);
            this.etiquetaSimple1.TabIndex = 0;
            this.etiquetaSimple1.Text = "Compañia";
            // 
            // tabPageCompatibilidad
            // 
            this.tabPageCompatibilidad.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPageCompatibilidad.Controls.Add(this.etiquetaSimple6);
            this.tabPageCompatibilidad.Controls.Add(this.numericoGPOpenYear);
            this.tabPageCompatibilidad.Controls.Add(this.etiquetaSimple4);
            this.tabPageCompatibilidad.Controls.Add(this.comboBasesDeDatos);
            this.tabPageCompatibilidad.Controls.Add(this.checkBoxManufacturingGP);
            this.tabPageCompatibilidad.Controls.Add(this.panel2);
            this.tabPageCompatibilidad.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageCompatibilidad.Location = new System.Drawing.Point(4, 28);
            this.tabPageCompatibilidad.Name = "tabPageCompatibilidad";
            this.tabPageCompatibilidad.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCompatibilidad.Size = new System.Drawing.Size(192, 68);
            this.tabPageCompatibilidad.TabIndex = 1;
            this.tabPageCompatibilidad.Text = "Compatibilidad";
            this.tabPageCompatibilidad.UseVisualStyleBackColor = true;
            // 
            // etiquetaSimple6
            // 
            this.etiquetaSimple6.AutoSize = true;
            this.etiquetaSimple6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple6.Location = new System.Drawing.Point(505, 136);
            this.etiquetaSimple6.Name = "etiquetaSimple6";
            this.etiquetaSimple6.Size = new System.Drawing.Size(126, 20);
            this.etiquetaSimple6.TabIndex = 6;
            this.etiquetaSimple6.Text = "Ejercicio Actual";
            // 
            // numericoGPOpenYear
            // 
            this.numericoGPOpenYear.Location = new System.Drawing.Point(657, 128);
            this.numericoGPOpenYear.Maximum = new decimal(new int[] {
            2200,
            0,
            0,
            0});
            this.numericoGPOpenYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericoGPOpenYear.Name = "numericoGPOpenYear";
            this.numericoGPOpenYear.Size = new System.Drawing.Size(120, 28);
            this.numericoGPOpenYear.TabIndex = 5;
            this.numericoGPOpenYear.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // etiquetaSimple4
            // 
            this.etiquetaSimple4.AutoSize = true;
            this.etiquetaSimple4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple4.Location = new System.Drawing.Point(501, 94);
            this.etiquetaSimple4.Name = "etiquetaSimple4";
            this.etiquetaSimple4.Size = new System.Drawing.Size(150, 20);
            this.etiquetaSimple4.TabIndex = 4;
            this.etiquetaSimple4.Text = "Base de Datos GP";
            // 
            // comboBasesDeDatos
            // 
            this.comboBasesDeDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBasesDeDatos.Enabled = false;
            this.comboBasesDeDatos.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBasesDeDatos.ForeColor = System.Drawing.Color.Black;
            this.comboBasesDeDatos.FormattingEnabled = true;
            this.comboBasesDeDatos.Location = new System.Drawing.Point(657, 85);
            this.comboBasesDeDatos.Name = "comboBasesDeDatos";
            this.comboBasesDeDatos.Size = new System.Drawing.Size(178, 29);
            this.comboBasesDeDatos.TabIndex = 3;
            // 
            // checkBoxManufacturingGP
            // 
            this.checkBoxManufacturingGP.AutoSize = true;
            this.checkBoxManufacturingGP.Enabled = false;
            this.checkBoxManufacturingGP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxManufacturingGP.Location = new System.Drawing.Point(493, 55);
            this.checkBoxManufacturingGP.Name = "checkBoxManufacturingGP";
            this.checkBoxManufacturingGP.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxManufacturingGP.Size = new System.Drawing.Size(342, 24);
            this.checkBoxManufacturingGP.TabIndex = 2;
            this.checkBoxManufacturingGP.Text = "Tiene el Modulo de Manufactura Instalado";
            this.checkBoxManufacturingGP.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.checkDynamics);
            this.panel2.Controls.Add(this.checkSumma);
            this.panel2.Location = new System.Drawing.Point(36, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(405, 100);
            this.panel2.TabIndex = 1;
            // 
            // checkDynamics
            // 
            this.checkDynamics.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkDynamics.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkDynamics.Location = new System.Drawing.Point(14, 15);
            this.checkDynamics.Name = "checkDynamics";
            this.checkDynamics.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkDynamics.Size = new System.Drawing.Size(376, 24);
            this.checkDynamics.TabIndex = 0;
            this.checkDynamics.Text = "Activar Compatibiliad con Microsoft Dynamics";
            this.checkDynamics.UseVisualStyleBackColor = true;
            this.checkDynamics.CheckedChanged += new System.EventHandler(this.checkDynamics_CheckedChanged);
            // 
            // checkSumma
            // 
            this.checkSumma.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkSumma.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkSumma.Location = new System.Drawing.Point(107, 45);
            this.checkSumma.Name = "checkSumma";
            this.checkSumma.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkSumma.Size = new System.Drawing.Size(283, 24);
            this.checkSumma.TabIndex = 0;
            this.checkSumma.Text = "Activar Compatibiliad con Summa\r\n";
            this.checkSumma.UseVisualStyleBackColor = true;
            // 
            // tabPageInventario
            // 
            this.tabPageInventario.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPageInventario.Controls.Add(this.panel3);
            this.tabPageInventario.Controls.Add(this.etiquetaSimple5);
            this.tabPageInventario.Controls.Add(this.trackDeposito);
            this.tabPageInventario.Controls.Add(this.trackDigitosTipoMovimientoInventario);
            this.tabPageInventario.Controls.Add(this.trackDigitosCodigoClaseArticulo);
            this.tabPageInventario.Controls.Add(this.trackDigitosCodigoArticulo);
            this.tabPageInventario.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageInventario.Location = new System.Drawing.Point(4, 28);
            this.tabPageInventario.Name = "tabPageInventario";
            this.tabPageInventario.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInventario.Size = new System.Drawing.Size(192, 68);
            this.tabPageInventario.TabIndex = 2;
            this.tabPageInventario.Text = "Inventario";
            this.tabPageInventario.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.comboSeparadorSucursal);
            this.panel3.Controls.Add(this.comboSeparadorTipo);
            this.panel3.Controls.Add(this.textoMascaraDocumento);
            this.panel3.Controls.Add(this.checkTipoDocumento);
            this.panel3.Controls.Add(this.checkDocumentoSucursal);
            this.panel3.Controls.Add(this.trackNumericoDigitosDocumento);
            this.panel3.Location = new System.Drawing.Point(74, 198);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(228, 252);
            this.panel3.TabIndex = 5;
            // 
            // comboSeparadorSucursal
            // 
            this.comboSeparadorSucursal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSeparadorSucursal.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboSeparadorSucursal.ForeColor = System.Drawing.Color.Black;
            this.comboSeparadorSucursal.FormattingEnabled = true;
            this.comboSeparadorSucursal.Items.AddRange(new object[] {
            "-",
            "_",
            ".",
            " "});
            this.comboSeparadorSucursal.Location = new System.Drawing.Point(169, 48);
            this.comboSeparadorSucursal.Name = "comboSeparadorSucursal";
            this.comboSeparadorSucursal.Size = new System.Drawing.Size(52, 29);
            this.comboSeparadorSucursal.TabIndex = 6;
            this.comboSeparadorSucursal.SelectedIndexChanged += new System.EventHandler(this.comboDesplegable2_SelectedIndexChanged);
            // 
            // comboSeparadorTipo
            // 
            this.comboSeparadorTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSeparadorTipo.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboSeparadorTipo.ForeColor = System.Drawing.Color.Black;
            this.comboSeparadorTipo.FormattingEnabled = true;
            this.comboSeparadorTipo.Items.AddRange(new object[] {
            "-",
            "_",
            ".",
            " "});
            this.comboSeparadorTipo.Location = new System.Drawing.Point(169, 13);
            this.comboSeparadorTipo.Name = "comboSeparadorTipo";
            this.comboSeparadorTipo.Size = new System.Drawing.Size(52, 29);
            this.comboSeparadorTipo.TabIndex = 6;
            this.comboSeparadorTipo.SelectedIndexChanged += new System.EventHandler(this.comboDesplegable1_SelectedIndexChanged);
            // 
            // textoMascaraDocumento
            // 
            this.textoMascaraDocumento.Enabled = false;
            this.textoMascaraDocumento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoMascaraDocumento.Location = new System.Drawing.Point(3, 209);
            this.textoMascaraDocumento.Name = "textoMascaraDocumento";
            this.textoMascaraDocumento.Size = new System.Drawing.Size(218, 27);
            this.textoMascaraDocumento.TabIndex = 8;
            this.textoMascaraDocumento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkTipoDocumento
            // 
            this.checkTipoDocumento.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkTipoDocumento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkTipoDocumento.Location = new System.Drawing.Point(40, 16);
            this.checkTipoDocumento.Name = "checkTipoDocumento";
            this.checkTipoDocumento.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkTipoDocumento.Size = new System.Drawing.Size(113, 24);
            this.checkTipoDocumento.TabIndex = 7;
            this.checkTipoDocumento.Text = "Incluir Tipo";
            this.checkTipoDocumento.UseVisualStyleBackColor = true;
            this.checkTipoDocumento.CheckedChanged += new System.EventHandler(this.checkTipoDocumento_CheckedChanged);
            // 
            // checkDocumentoSucursal
            // 
            this.checkDocumentoSucursal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkDocumentoSucursal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkDocumentoSucursal.Location = new System.Drawing.Point(6, 53);
            this.checkDocumentoSucursal.Name = "checkDocumentoSucursal";
            this.checkDocumentoSucursal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkDocumentoSucursal.Size = new System.Drawing.Size(147, 24);
            this.checkDocumentoSucursal.TabIndex = 6;
            this.checkDocumentoSucursal.Text = "Incluir Sucursal";
            this.checkDocumentoSucursal.UseVisualStyleBackColor = true;
            this.checkDocumentoSucursal.CheckedChanged += new System.EventHandler(this.checkDocumentoSucursal_CheckedChanged);
            // 
            // trackNumericoDigitosDocumento
            // 
            this.trackNumericoDigitosDocumento.Location = new System.Drawing.Point(6, 90);
            this.trackNumericoDigitosDocumento.Maximo = 15;
            this.trackNumericoDigitosDocumento.Minimo = 0;
            this.trackNumericoDigitosDocumento.Name = "trackNumericoDigitosDocumento";
            this.trackNumericoDigitosDocumento.Size = new System.Drawing.Size(215, 100);
            this.trackNumericoDigitosDocumento.TabIndex = 5;
            this.trackNumericoDigitosDocumento.Titulo = "Cantidad de Digitos";
            this.trackNumericoDigitosDocumento.Valor = 0;
            // 
            // etiquetaSimple5
            // 
            this.etiquetaSimple5.AutoSize = true;
            this.etiquetaSimple5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple5.Location = new System.Drawing.Point(70, 175);
            this.etiquetaSimple5.Name = "etiquetaSimple5";
            this.etiquetaSimple5.Size = new System.Drawing.Size(242, 20);
            this.etiquetaSimple5.TabIndex = 3;
            this.etiquetaSimple5.Text = "Mascara Documento Inventario";
            // 
            // trackDeposito
            // 
            this.trackDeposito.Location = new System.Drawing.Point(756, 33);
            this.trackDeposito.Maximo = 32;
            this.trackDeposito.Minimo = 0;
            this.trackDeposito.Name = "trackDeposito";
            this.trackDeposito.Size = new System.Drawing.Size(215, 100);
            this.trackDeposito.TabIndex = 2;
            this.trackDeposito.Titulo = "Ancho Max. Deposito";
            this.trackDeposito.Valor = 0;
            // 
            // trackDigitosTipoMovimientoInventario
            // 
            this.trackDigitosTipoMovimientoInventario.Location = new System.Drawing.Point(514, 33);
            this.trackDigitosTipoMovimientoInventario.Maximo = 32;
            this.trackDigitosTipoMovimientoInventario.Minimo = 0;
            this.trackDigitosTipoMovimientoInventario.Name = "trackDigitosTipoMovimientoInventario";
            this.trackDigitosTipoMovimientoInventario.Size = new System.Drawing.Size(215, 100);
            this.trackDigitosTipoMovimientoInventario.TabIndex = 2;
            this.trackDigitosTipoMovimientoInventario.Titulo = "Ancho Max. Tipo Mov";
            this.trackDigitosTipoMovimientoInventario.Valor = 0;
            // 
            // trackDigitosCodigoClaseArticulo
            // 
            this.trackDigitosCodigoClaseArticulo.Location = new System.Drawing.Point(283, 33);
            this.trackDigitosCodigoClaseArticulo.Maximo = 32;
            this.trackDigitosCodigoClaseArticulo.Minimo = 0;
            this.trackDigitosCodigoClaseArticulo.Name = "trackDigitosCodigoClaseArticulo";
            this.trackDigitosCodigoClaseArticulo.Size = new System.Drawing.Size(215, 100);
            this.trackDigitosCodigoClaseArticulo.TabIndex = 1;
            this.trackDigitosCodigoClaseArticulo.Titulo = "Ancho Max. Cod. Clase";
            this.trackDigitosCodigoClaseArticulo.Valor = 0;
            // 
            // trackDigitosCodigoArticulo
            // 
            this.trackDigitosCodigoArticulo.Location = new System.Drawing.Point(56, 33);
            this.trackDigitosCodigoArticulo.Maximo = 32;
            this.trackDigitosCodigoArticulo.Minimo = 0;
            this.trackDigitosCodigoArticulo.Name = "trackDigitosCodigoArticulo";
            this.trackDigitosCodigoArticulo.Size = new System.Drawing.Size(215, 100);
            this.trackDigitosCodigoArticulo.TabIndex = 0;
            this.trackDigitosCodigoArticulo.Titulo = "Ancho Max. Cod. Articulo";
            this.trackDigitosCodigoArticulo.Valor = 0;
            // 
            // tabPageRRHH
            // 
            this.tabPageRRHH.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPageRRHH.Controls.Add(this.trackDigitoCodigoTipo);
            this.tabPageRRHH.Controls.Add(this.etiquetaSimple3);
            this.tabPageRRHH.Controls.Add(this.fechaDesdeHastaFeriados);
            this.tabPageRRHH.Controls.Add(this.trackDigitosCodigoIncidencias);
            this.tabPageRRHH.Controls.Add(this.trackDigitosCodigoTurnos);
            this.tabPageRRHH.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageRRHH.Location = new System.Drawing.Point(4, 28);
            this.tabPageRRHH.Name = "tabPageRRHH";
            this.tabPageRRHH.Size = new System.Drawing.Size(1110, 573);
            this.tabPageRRHH.TabIndex = 3;
            this.tabPageRRHH.Text = "RRHH";
            this.tabPageRRHH.UseVisualStyleBackColor = true;
            // 
            // etiquetaSimple3
            // 
            this.etiquetaSimple3.AutoSize = true;
            this.etiquetaSimple3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple3.ForeColor = System.Drawing.Color.Blue;
            this.etiquetaSimple3.Location = new System.Drawing.Point(79, 144);
            this.etiquetaSimple3.Name = "etiquetaSimple3";
            this.etiquetaSimple3.Size = new System.Drawing.Size(228, 20);
            this.etiquetaSimple3.TabIndex = 4;
            this.etiquetaSimple3.Text = "Rango para Mostrar Feriados";
            // 
            // fechaDesdeHastaFeriados
            // 
            this.fechaDesdeHastaFeriados.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fechaDesdeHastaFeriados.FechaDesde = new System.DateTime(2018, 7, 16, 0, 0, 0, 0);
            this.fechaDesdeHastaFeriados.FechaHasta = new System.DateTime(2018, 7, 16, 0, 0, 0, 0);
            this.fechaDesdeHastaFeriados.Location = new System.Drawing.Point(66, 167);
            this.fechaDesdeHastaFeriados.MaximumSize = new System.Drawing.Size(266, 76);
            this.fechaDesdeHastaFeriados.MinimumSize = new System.Drawing.Size(266, 76);
            this.fechaDesdeHastaFeriados.Name = "fechaDesdeHastaFeriados";
            this.fechaDesdeHastaFeriados.Size = new System.Drawing.Size(266, 76);
            this.fechaDesdeHastaFeriados.TabIndex = 3;
            // 
            // trackDigitosCodigoIncidencias
            // 
            this.trackDigitosCodigoIncidencias.Location = new System.Drawing.Point(303, 34);
            this.trackDigitosCodigoIncidencias.Maximo = 5;
            this.trackDigitosCodigoIncidencias.Minimo = 0;
            this.trackDigitosCodigoIncidencias.Name = "trackDigitosCodigoIncidencias";
            this.trackDigitosCodigoIncidencias.Size = new System.Drawing.Size(215, 100);
            this.trackDigitosCodigoIncidencias.TabIndex = 2;
            this.trackDigitosCodigoIncidencias.Titulo = "Ancho Max. Cod. Incid.";
            this.trackDigitosCodigoIncidencias.Valor = 0;
            // 
            // trackDigitosCodigoTurnos
            // 
            this.trackDigitosCodigoTurnos.Location = new System.Drawing.Point(62, 34);
            this.trackDigitosCodigoTurnos.Maximo = 5;
            this.trackDigitosCodigoTurnos.Minimo = 0;
            this.trackDigitosCodigoTurnos.Name = "trackDigitosCodigoTurnos";
            this.trackDigitosCodigoTurnos.Size = new System.Drawing.Size(215, 100);
            this.trackDigitosCodigoTurnos.TabIndex = 1;
            this.trackDigitosCodigoTurnos.Titulo = "Ancho Max. Cod. Turnos";
            this.trackDigitosCodigoTurnos.Valor = 0;
            // 
            // tabPageControles
            // 
            this.tabPageControles.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPageControles.Controls.Add(this.etiquetaSimple2);
            this.tabPageControles.Controls.Add(this.numericoMaxRegLupita);
            this.tabPageControles.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageControles.Location = new System.Drawing.Point(4, 28);
            this.tabPageControles.Name = "tabPageControles";
            this.tabPageControles.Size = new System.Drawing.Size(192, 68);
            this.tabPageControles.TabIndex = 4;
            this.tabPageControles.Text = "Controles";
            this.tabPageControles.UseVisualStyleBackColor = true;
            // 
            // etiquetaSimple2
            // 
            this.etiquetaSimple2.AutoSize = true;
            this.etiquetaSimple2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple2.Location = new System.Drawing.Point(20, 26);
            this.etiquetaSimple2.Name = "etiquetaSimple2";
            this.etiquetaSimple2.Size = new System.Drawing.Size(275, 20);
            this.etiquetaSimple2.TabIndex = 2;
            this.etiquetaSimple2.Text = "Cant. Max. de Registros para ayuda";
            // 
            // numericoMaxRegLupita
            // 
            this.numericoMaxRegLupita.Location = new System.Drawing.Point(306, 20);
            this.numericoMaxRegLupita.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericoMaxRegLupita.Name = "numericoMaxRegLupita";
            this.numericoMaxRegLupita.Size = new System.Drawing.Size(112, 28);
            this.numericoMaxRegLupita.TabIndex = 1;
            // 
            // tabPageContable
            // 
            this.tabPageContable.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPageContable.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageContable.Location = new System.Drawing.Point(4, 28);
            this.tabPageContable.Name = "tabPageContable";
            this.tabPageContable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageContable.Size = new System.Drawing.Size(192, 68);
            this.tabPageContable.TabIndex = 5;
            this.tabPageContable.Text = "Contable";
            this.tabPageContable.UseVisualStyleBackColor = true;
            // 
            // trackDigitoCodigoTipo
            // 
            this.trackDigitoCodigoTipo.Location = new System.Drawing.Point(536, 34);
            this.trackDigitoCodigoTipo.Maximo = 5;
            this.trackDigitoCodigoTipo.Minimo = 0;
            this.trackDigitoCodigoTipo.Name = "trackDigitoCodigoTipo";
            this.trackDigitoCodigoTipo.Size = new System.Drawing.Size(215, 100);
            this.trackDigitoCodigoTipo.TabIndex = 5;
            this.trackDigitoCodigoTipo.Titulo = "Ancho Max. Cod. Tipo";
            this.trackDigitoCodigoTipo.Valor = 0;
            // 
            // FrmConfigurador
            // 
            this.ClientSize = new System.Drawing.Size(1118, 692);
            this.Controls.Add(this.tabControl);
            this.Name = "FrmConfigurador";
            this.Controls.SetChildIndex(this.panelMenuIndividual, 0);
            this.Controls.SetChildIndex(this.tabControl, 0);
            this.tabControl.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageCompatibilidad.ResumeLayout(false);
            this.tabPageCompatibilidad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericoGPOpenYear)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tabPageInventario.ResumeLayout(false);
            this.tabPageInventario.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabPageRRHH.ResumeLayout(false);
            this.tabPageRRHH.PerformLayout();
            this.tabPageControles.ResumeLayout(false);
            this.tabPageControles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericoMaxRegLupita)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private Global.Controles.TabPagina tabPageGeneral;
		private Global.Controles.TabPagina tabPageRRHH;
		private Global.Controles.TabPagina tabPageInventario;
		private Global.Controles.TabPagina tabPageCompatibilidad;
		private Global.Controles.TabPagina tabPageControles;
		private Global.Controles.TabPagina tabPageContable;
		private Global.Controles.EtiquetaSimple etiquetaSimple1;
		private Global.Controles.TextoSimple TextoEmpresaRazonSocial;
		private Global.Controles.CheckBox checkSumma;
		private Global.Controles.CheckBox checkDynamics;
		private Global.Controles.Panel panel2;
		private Global.Controles.TrackNumericoCombo trackDigitosCodigoArticulo;
		private Global.Controles.TrackNumericoCombo trackDigitosCodigoTurnos;
		private Global.Controles.EtiquetaSimple etiquetaSimple2;
		private Global.Controles.Numerico numericoMaxRegLupita;
		private Global.Controles.TrackNumericoCombo trackDigitosCodigoIncidencias;
		private Global.Controles.TrackNumericoCombo trackDigitosCodigoClaseArticulo;
		private Global.Controles.TrackNumericoCombo trackDigitosTipoMovimientoInventario;
		private Global.Controles.CheckBox checkBoxManufacturingGP;
		private Global.Controles.EtiquetaSimple etiquetaSimple3;
		private Global.Controles.FechayHora.FechaDesdeHasta fechaDesdeHastaFeriados;
		private Global.Controles.EtiquetaSimple etiquetaSimple4;
		private Global.Controles.ComboDesplegable comboBasesDeDatos;
		private Global.Controles.Panel panel3;
		private Global.Controles.TextoSimple textoMascaraDocumento;
		private Global.Controles.CheckBox checkTipoDocumento;
		private Global.Controles.CheckBox checkDocumentoSucursal;
		private Global.Controles.TrackNumericoCombo trackNumericoDigitosDocumento;
		private Global.Controles.EtiquetaSimple etiquetaSimple5;
		private Global.Controles.ComboDesplegable comboSeparadorSucursal;
		private Global.Controles.ComboDesplegable comboSeparadorTipo;
		private Global.Controles.TrackNumericoCombo trackDeposito;
		private Global.Controles.TrackNumericoCombo trackSucursal;
		private Global.Controles.TrackNumericoCombo trackControlEntidadBusqueda;
		private Global.Controles.EtiquetaSimple etiquetaSimple6;
		private Global.Controles.Numerico numericoGPOpenYear;
        private Global.Controles.TrackNumericoCombo trackDigitoCodigoTipo;
	}
}
