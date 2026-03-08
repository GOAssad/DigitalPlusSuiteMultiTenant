namespace Acceso.uAreu
{
    partial class FrmRegistrar
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox gbVerification;
            System.Windows.Forms.GroupBox gbReturnValues;
            System.Windows.Forms.Label lblFalseAcceptRate;
            System.Windows.Forms.GroupBox gbEnrollment;
            System.Windows.Forms.GroupBox gbEventHandlerStatus;
            System.Windows.Forms.Label lblMaxFingers;
            System.Windows.Forms.Label lblMask;
            this.VerifyButton = new System.Windows.Forms.Button();
            this.FalseAcceptRate = new System.Windows.Forms.TextBox();
            this.IsFeatureSetMatched = new System.Windows.Forms.CheckBox();
            this.MaxFingers = new System.Windows.Forms.NumericUpDown();
            this.Mask = new System.Windows.Forms.NumericUpDown();
            this.EnrollButton = new System.Windows.Forms.Button();
            this.IsFailure = new System.Windows.Forms.RadioButton();
            this.IsSuccess = new System.Windows.Forms.RadioButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.controlEntidadLegajos = new Acceso.ControlEntidad.ControlEntidadSimple();
            this.textoEtiquetaApellido = new Global.Controles.Text.TextoEtiqueta();
            this.textoEtiquetaNombre = new Global.Controles.Text.TextoEtiqueta();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboCategoria = new Global.Controles.ComboDesplegable();
            this.comboSector = new Global.Controles.ComboDesplegable();
            this.etiquetaDedos = new Global.Controles.EtiquetaLink();
            this.pictureHuella = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelContenedor = new System.Windows.Forms.Panel();
            this.EnrollmentControl = new DPFP.Gui.Enrollment.EnrollmentControl();
            gbVerification = new System.Windows.Forms.GroupBox();
            gbReturnValues = new System.Windows.Forms.GroupBox();
            lblFalseAcceptRate = new System.Windows.Forms.Label();
            gbEnrollment = new System.Windows.Forms.GroupBox();
            gbEventHandlerStatus = new System.Windows.Forms.GroupBox();
            lblMaxFingers = new System.Windows.Forms.Label();
            lblMask = new System.Windows.Forms.Label();
            this.toolStripContainerMenu.ContentPanel.SuspendLayout();
            this.toolStripContainerMenu.SuspendLayout();
            gbVerification.SuspendLayout();
            gbReturnValues.SuspendLayout();
            gbEnrollment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxFingers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mask)).BeginInit();
            gbEventHandlerStatus.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureHuella)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelContenedor.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainerMenu
            // 
            // 
            // toolStripContainerMenu.ContentPanel
            // 
            this.toolStripContainerMenu.ContentPanel.Controls.Add(this.tableLayoutPanel1);
            this.toolStripContainerMenu.ContentPanel.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.toolStripContainerMenu.ContentPanel.Size = new System.Drawing.Size(1052, 768);
            this.toolStripContainerMenu.ContentPanel.Load += new System.EventHandler(this.toolStripContainerMenu_ContentPanel_Load);
            this.toolStripContainerMenu.Size = new System.Drawing.Size(1052, 768);
            // 
            // gbVerification
            // 
            gbVerification.Controls.Add(this.VerifyButton);
            gbVerification.Controls.Add(gbReturnValues);
            gbVerification.Enabled = false;
            gbVerification.Location = new System.Drawing.Point(691, 292);
            gbVerification.Margin = new System.Windows.Forms.Padding(4);
            gbVerification.Name = "gbVerification";
            gbVerification.Padding = new System.Windows.Forms.Padding(4);
            gbVerification.Size = new System.Drawing.Size(287, 247);
            gbVerification.TabIndex = 7;
            gbVerification.TabStop = false;
            gbVerification.Text = "Verification";
            gbVerification.Visible = false;
            // 
            // VerifyButton
            // 
            this.VerifyButton.Location = new System.Drawing.Point(97, 198);
            this.VerifyButton.Margin = new System.Windows.Forms.Padding(4);
            this.VerifyButton.Name = "VerifyButton";
            this.VerifyButton.Size = new System.Drawing.Size(143, 28);
            this.VerifyButton.TabIndex = 1;
            this.VerifyButton.Text = "Verify Fingerprints";
            this.toolTip.SetToolTip(this.VerifyButton, "Start fingerprint verification");
            this.VerifyButton.UseVisualStyleBackColor = true;
            this.VerifyButton.Click += new System.EventHandler(this.VerifyButton_Click);
            // 
            // gbReturnValues
            // 
            gbReturnValues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gbReturnValues.Controls.Add(this.FalseAcceptRate);
            gbReturnValues.Controls.Add(lblFalseAcceptRate);
            gbReturnValues.Controls.Add(this.IsFeatureSetMatched);
            gbReturnValues.Location = new System.Drawing.Point(12, 49);
            gbReturnValues.Margin = new System.Windows.Forms.Padding(4);
            gbReturnValues.Name = "gbReturnValues";
            gbReturnValues.Padding = new System.Windows.Forms.Padding(4);
            gbReturnValues.Size = new System.Drawing.Size(267, 123);
            gbReturnValues.TabIndex = 0;
            gbReturnValues.TabStop = false;
            gbReturnValues.Text = "Return values";
            // 
            // FalseAcceptRate
            // 
            this.FalseAcceptRate.Location = new System.Drawing.Point(176, 76);
            this.FalseAcceptRate.Margin = new System.Windows.Forms.Padding(4);
            this.FalseAcceptRate.Name = "FalseAcceptRate";
            this.FalseAcceptRate.ReadOnly = true;
            this.FalseAcceptRate.Size = new System.Drawing.Size(149, 22);
            this.FalseAcceptRate.TabIndex = 2;
            this.toolTip.SetToolTip(this.FalseAcceptRate, "Displays the false accept rate (FAR)");
            // 
            // lblFalseAcceptRate
            // 
            lblFalseAcceptRate.Location = new System.Drawing.Point(8, 76);
            lblFalseAcceptRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblFalseAcceptRate.Name = "lblFalseAcceptRate";
            lblFalseAcceptRate.Size = new System.Drawing.Size(160, 25);
            lblFalseAcceptRate.TabIndex = 1;
            lblFalseAcceptRate.Text = "False accept rate";
            lblFalseAcceptRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IsFeatureSetMatched
            // 
            this.IsFeatureSetMatched.AutoCheck = false;
            this.IsFeatureSetMatched.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IsFeatureSetMatched.Location = new System.Drawing.Point(8, 39);
            this.IsFeatureSetMatched.Margin = new System.Windows.Forms.Padding(4);
            this.IsFeatureSetMatched.Name = "IsFeatureSetMatched";
            this.IsFeatureSetMatched.Size = new System.Drawing.Size(185, 30);
            this.IsFeatureSetMatched.TabIndex = 0;
            this.IsFeatureSetMatched.Text = "Feature set matched?";
            this.toolTip.SetToolTip(this.IsFeatureSetMatched, "Displays a match result");
            this.IsFeatureSetMatched.UseVisualStyleBackColor = true;
            // 
            // gbEnrollment
            // 
            gbEnrollment.Controls.Add(this.MaxFingers);
            gbEnrollment.Controls.Add(this.Mask);
            gbEnrollment.Controls.Add(this.EnrollButton);
            gbEnrollment.Controls.Add(gbEventHandlerStatus);
            gbEnrollment.Controls.Add(lblMaxFingers);
            gbEnrollment.Controls.Add(lblMask);
            gbEnrollment.Enabled = false;
            gbEnrollment.Location = new System.Drawing.Point(691, 4);
            gbEnrollment.Margin = new System.Windows.Forms.Padding(4);
            gbEnrollment.Name = "gbEnrollment";
            gbEnrollment.Padding = new System.Windows.Forms.Padding(4);
            gbEnrollment.Size = new System.Drawing.Size(287, 231);
            gbEnrollment.TabIndex = 8;
            gbEnrollment.TabStop = false;
            gbEnrollment.Text = "Registrar Huellas";
            gbEnrollment.Visible = false;
            // 
            // MaxFingers
            // 
            this.MaxFingers.Enabled = false;
            this.MaxFingers.Location = new System.Drawing.Point(213, 64);
            this.MaxFingers.Margin = new System.Windows.Forms.Padding(4);
            this.MaxFingers.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MaxFingers.Name = "MaxFingers";
            this.MaxFingers.Size = new System.Drawing.Size(125, 22);
            this.MaxFingers.TabIndex = 3;
            this.toolTip.SetToolTip(this.MaxFingers, "Enter a number from 1 to 10");
            this.MaxFingers.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // Mask
            // 
            this.Mask.Enabled = false;
            this.Mask.Location = new System.Drawing.Point(213, 33);
            this.Mask.Margin = new System.Windows.Forms.Padding(4);
            this.Mask.Maximum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.Mask.Name = "Mask";
            this.Mask.Size = new System.Drawing.Size(125, 22);
            this.Mask.TabIndex = 1;
            this.toolTip.SetToolTip(this.Mask, "Enter a number from 0 to 1023");
            // 
            // EnrollButton
            // 
            this.EnrollButton.Location = new System.Drawing.Point(99, 184);
            this.EnrollButton.Margin = new System.Windows.Forms.Padding(4);
            this.EnrollButton.Name = "EnrollButton";
            this.EnrollButton.Size = new System.Drawing.Size(143, 28);
            this.EnrollButton.TabIndex = 5;
            this.EnrollButton.Text = "Registrar Huellas";
            this.toolTip.SetToolTip(this.EnrollButton, "Start fingerprint enrollment");
            this.EnrollButton.UseVisualStyleBackColor = true;
            this.EnrollButton.Click += new System.EventHandler(this.EnrollButton_Click);
            // 
            // gbEventHandlerStatus
            // 
            gbEventHandlerStatus.Controls.Add(this.IsFailure);
            gbEventHandlerStatus.Controls.Add(this.IsSuccess);
            gbEventHandlerStatus.Enabled = false;
            gbEventHandlerStatus.Location = new System.Drawing.Point(11, 94);
            gbEventHandlerStatus.Margin = new System.Windows.Forms.Padding(4);
            gbEventHandlerStatus.Name = "gbEventHandlerStatus";
            gbEventHandlerStatus.Padding = new System.Windows.Forms.Padding(4);
            gbEventHandlerStatus.Size = new System.Drawing.Size(335, 74);
            gbEventHandlerStatus.TabIndex = 4;
            gbEventHandlerStatus.TabStop = false;
            gbEventHandlerStatus.Text = "Metodo de Evento";
            // 
            // IsFailure
            // 
            this.IsFailure.AutoSize = true;
            this.IsFailure.Location = new System.Drawing.Point(201, 36);
            this.IsFailure.Margin = new System.Windows.Forms.Padding(4);
            this.IsFailure.Name = "IsFailure";
            this.IsFailure.Size = new System.Drawing.Size(70, 21);
            this.IsFailure.TabIndex = 1;
            this.IsFailure.TabStop = true;
            this.IsFailure.Text = "Fallido";
            this.toolTip.SetToolTip(this.IsFailure, "Force an enrollment failure");
            this.IsFailure.UseVisualStyleBackColor = true;
            // 
            // IsSuccess
            // 
            this.IsSuccess.AutoSize = true;
            this.IsSuccess.Location = new System.Drawing.Point(35, 36);
            this.IsSuccess.Margin = new System.Windows.Forms.Padding(4);
            this.IsSuccess.Name = "IsSuccess";
            this.IsSuccess.Size = new System.Drawing.Size(74, 21);
            this.IsSuccess.TabIndex = 0;
            this.IsSuccess.TabStop = true;
            this.IsSuccess.Text = "Exitoso";
            this.toolTip.SetToolTip(this.IsSuccess, "Allow a successful enrollment");
            this.IsSuccess.UseVisualStyleBackColor = true;
            // 
            // lblMaxFingers
            // 
            lblMaxFingers.Location = new System.Drawing.Point(8, 65);
            lblMaxFingers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMaxFingers.Name = "lblMaxFingers";
            lblMaxFingers.Size = new System.Drawing.Size(197, 25);
            lblMaxFingers.TabIndex = 2;
            lblMaxFingers.Text = "Max Huellas permitidos";
            lblMaxFingers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMask
            // 
            lblMask.Location = new System.Drawing.Point(8, 33);
            lblMask.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMask.Name = "lblMask";
            lblMask.Size = new System.Drawing.Size(197, 25);
            lblMask.TabIndex = 0;
            lblMask.Text = "Mascara de Huella";
            lblMask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // controlEntidadLegajos
            // 
            this.controlEntidadLegajos.ActualizaenFormulario = true;
            this.controlEntidadLegajos.BusquedaAvanzada = false;
            this.controlEntidadLegajos.ConTitulo = true;
            this.controlEntidadLegajos.DESCSQLFrom = "Nombre";
            this.controlEntidadLegajos.GeneraNuevo = false;
            this.controlEntidadLegajos.IDSQLWhere = "LegajoId";
            this.controlEntidadLegajos.IDSQLWherePK = "id";
            this.controlEntidadLegajos.IDValor = 0;
            this.controlEntidadLegajos.Location = new System.Drawing.Point(6, 9);
            this.controlEntidadLegajos.MostrarID = false;
            this.controlEntidadLegajos.Name = "controlEntidadLegajos";
            this.controlEntidadLegajos.Size = new System.Drawing.Size(495, 70);
            this.controlEntidadLegajos.SqlAyuda = "select sLegajoID, sApellido from RRHHLegajos";
            this.controlEntidadLegajos.TabIndex = 10;
            this.controlEntidadLegajos.TablaSQL = "RRHHlegajos";
            this.controlEntidadLegajos.TextoEtiqueta = " Legajo";
            this.controlEntidadLegajos.TituloAyuda = "Legajos";
            this.controlEntidadLegajos.VariableGlobalMascara = null;
            // 
            // textoEtiquetaApellido
            // 
            this.textoEtiquetaApellido.Location = new System.Drawing.Point(6, 84);
            this.textoEtiquetaApellido.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textoEtiquetaApellido.Name = "textoEtiquetaApellido";
            this.textoEtiquetaApellido.Size = new System.Drawing.Size(395, 62);
            this.textoEtiquetaApellido.TabIndex = 12;
            this.textoEtiquetaApellido.Titulo = "Apellido";
            this.textoEtiquetaApellido.Valor = null;
            // 
            // textoEtiquetaNombre
            // 
            this.textoEtiquetaNombre.Location = new System.Drawing.Point(6, 150);
            this.textoEtiquetaNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textoEtiquetaNombre.Name = "textoEtiquetaNombre";
            this.textoEtiquetaNombre.Size = new System.Drawing.Size(395, 62);
            this.textoEtiquetaNombre.TabIndex = 12;
            this.textoEtiquetaNombre.Titulo = "Nombre";
            this.textoEtiquetaNombre.Valor = null;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboCategoria);
            this.panel1.Controls.Add(this.comboSector);
            this.panel1.Controls.Add(this.etiquetaDedos);
            this.panel1.Controls.Add(this.pictureHuella);
            this.panel1.Controls.Add(this.controlEntidadLegajos);
            this.panel1.Controls.Add(this.textoEtiquetaApellido);
            this.panel1.Controls.Add(this.textoEtiquetaNombre);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 282);
            this.panel1.TabIndex = 15;
            // 
            // comboCategoria
            // 
            this.comboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCategoria.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboCategoria.ForeColor = System.Drawing.Color.Black;
            this.comboCategoria.FormattingEnabled = true;
            this.comboCategoria.Items.AddRange(new object[] {
            "Sin Asignar",
            "Jefe",
            "Gerente",
            "Supervisor",
            "Encargado de local",
            "Vendedor",
            "Administrativo"});
            this.comboCategoria.Location = new System.Drawing.Point(273, 239);
            this.comboCategoria.Name = "comboCategoria";
            this.comboCategoria.Size = new System.Drawing.Size(259, 29);
            this.comboCategoria.TabIndex = 18;
            // 
            // comboSector
            // 
            this.comboSector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSector.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboSector.ForeColor = System.Drawing.Color.Black;
            this.comboSector.FormattingEnabled = true;
            this.comboSector.Items.AddRange(new object[] {
            "Sin Asignar",
            "Administracion Dardo Rocha",
            "Unicenter",
            "Fabrica - Cuyo",
            "Alto Palermo",
            "Avellaneda",
            "Local Rosario",
            "Administracion Dardo Rocha - IT",
            "Fabrica - Cuyo - Corte",
            "Fabrica - Cuyo - Diseño",
            "Abasto",
            "Alcorta",
            "Solar",
            "Santa Fe",
            "Patio Bulrich",
            "Galerias Pacifico",
            "Sucre",
            "Administracion Dardo Rocha - Control",
            "DOT",
            "Fabrica - Cuyo",
            "Nordelta",
            "Pilar"});
            this.comboSector.Location = new System.Drawing.Point(6, 239);
            this.comboSector.Name = "comboSector";
            this.comboSector.Size = new System.Drawing.Size(259, 29);
            this.comboSector.TabIndex = 17;
            // 
            // etiquetaDedos
            // 
            this.etiquetaDedos.AutoSize = true;
            this.etiquetaDedos.Font = new System.Drawing.Font("Segoe UI Semilight", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaDedos.ForeColor = System.Drawing.Color.White;
            this.etiquetaDedos.LinkColor = System.Drawing.Color.SlateGray;
            this.etiquetaDedos.Location = new System.Drawing.Point(507, 161);
            this.etiquetaDedos.Name = "etiquetaDedos";
            this.etiquetaDedos.Size = new System.Drawing.Size(42, 51);
            this.etiquetaDedos.TabIndex = 16;
            this.etiquetaDedos.TabStop = true;
            this.etiquetaDedos.Text = "0";
            this.etiquetaDedos.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.etiquetaDedos_LinkClicked);
            // 
            // pictureHuella
            // 
            this.pictureHuella.Enabled = false;
            this.pictureHuella.Image = global::Acceso.Properties.Resources.HuellaDactilar;
            this.pictureHuella.Location = new System.Drawing.Point(414, 118);
            this.pictureHuella.Name = "pictureHuella";
            this.pictureHuella.Size = new System.Drawing.Size(87, 94);
            this.pictureHuella.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureHuella.TabIndex = 16;
            this.pictureHuella.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(gbEnrollment, 1, 0);
            this.tableLayoutPanel1.Controls.Add(gbVerification, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelContenedor, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 47);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(982, 721);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // panelContenedor
            // 
            this.panelContenedor.Controls.Add(this.EnrollmentControl);
            this.panelContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenedor.Location = new System.Drawing.Point(3, 291);
            this.panelContenedor.Name = "panelContenedor";
            this.panelContenedor.Size = new System.Drawing.Size(681, 427);
            this.panelContenedor.TabIndex = 16;
            // 
            // EnrollmentControl
            // 
            this.EnrollmentControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EnrollmentControl.EnrolledFingerMask = 0;
            this.EnrollmentControl.Location = new System.Drawing.Point(1, 4);
            this.EnrollmentControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EnrollmentControl.MaxEnrollFingerCount = 10;
            this.EnrollmentControl.Name = "EnrollmentControl";
            this.EnrollmentControl.ReaderSerialNumber = "00000000-0000-0000-0000-000000000000";
            this.EnrollmentControl.Size = new System.Drawing.Size(660, 386);
            this.EnrollmentControl.TabIndex = 1;
            // 
            // FrmRegistrar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.BotonEliminarVisible = true;
            this.BotonGuardarVisible = true;
            this.BotonNuevoVisible = true;
            this.ClientSize = new System.Drawing.Size(1052, 768);
            this.Name = "FrmRegistrar";
            this.Titulo = "Registro de Legajos";
            this.Load += new System.EventHandler(this.FrmRegistrar_Load);
            this.toolStripContainerMenu.ContentPanel.ResumeLayout(false);
            this.toolStripContainerMenu.ContentPanel.PerformLayout();
            this.toolStripContainerMenu.ResumeLayout(false);
            this.toolStripContainerMenu.PerformLayout();
            gbVerification.ResumeLayout(false);
            gbReturnValues.ResumeLayout(false);
            gbReturnValues.PerformLayout();
            gbEnrollment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MaxFingers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mask)).EndInit();
            gbEventHandlerStatus.ResumeLayout(false);
            gbEventHandlerStatus.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureHuella)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelContenedor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button VerifyButton;
        private System.Windows.Forms.TextBox FalseAcceptRate;
        private System.Windows.Forms.CheckBox IsFeatureSetMatched;
        private System.Windows.Forms.NumericUpDown MaxFingers;
        private System.Windows.Forms.NumericUpDown Mask;
        private System.Windows.Forms.Button EnrollButton;
        private System.Windows.Forms.RadioButton IsFailure;
        private System.Windows.Forms.RadioButton IsSuccess;
        private ControlEntidad.ControlEntidadSimple controlEntidadLegajos;
        private Global.Controles.Text.TextoEtiqueta textoEtiquetaNombre;
        private Global.Controles.Text.TextoEtiqueta textoEtiquetaApellido;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureHuella;
        private Global.Controles.EtiquetaLink etiquetaDedos;
        private Global.Controles.ComboDesplegable comboSector;
        private Global.Controles.ComboDesplegable comboCategoria;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelContenedor;
        private DPFP.Gui.Enrollment.EnrollmentControl EnrollmentControl;
    }
}
