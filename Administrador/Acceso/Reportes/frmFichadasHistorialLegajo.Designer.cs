namespace Acceso.Reportes
{
    partial class frmFichadasHistorialLegajo
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.RRHHFichadasListingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.RRHHFichadasReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboES = new Global.Controles.ComboDesplegable();
            this.etiquetaNombre = new Global.Controles.EtiquetaSimple();
            this.etiquetaApellido = new Global.Controles.EtiquetaSimple();
            this.etiquetaLegajo = new Global.Controles.EtiquetaTitulo();
            this.etiquetaSimple3 = new Global.Controles.EtiquetaSimple();
            this.etiquetaSimple7 = new Global.Controles.EtiquetaSimple();
            this.etiquetaSimple1 = new Global.Controles.EtiquetaSimple();
            this.etiquetaSimple8 = new Global.Controles.EtiquetaSimple();
            this.etiquetaSimpleFrom = new Global.Controles.EtiquetaSimple();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.btnPersonalizado = new Global.Controles.BotonMenuVertical();
            this.btnEsteAno = new Global.Controles.BotonMenuVertical();
            this.btn30Dias = new Global.Controles.BotonMenuVertical();
            this.btnEsteMes = new Global.Controles.BotonMenuVertical();
            this.btn7dias = new Global.Controles.BotonMenuVertical();
            this.btnToday = new Global.Controles.BotonMenuVertical();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.RRHHFichadasListingBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RRHHFichadasReportBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // RRHHFichadasListingBindingSource
            // 
            this.RRHHFichadasListingBindingSource.DataSource = typeof(Acceso.Clases.Datos.RRHH.RRHHFichadasListing);
            // 
            // RRHHFichadasReportBindingSource
            // 
            this.RRHHFichadasReportBindingSource.DataSource = typeof(Acceso.Clases.Datos.RRHH.RRHHFichadasReport);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.comboES);
            this.panel1.Controls.Add(this.etiquetaNombre);
            this.panel1.Controls.Add(this.etiquetaApellido);
            this.panel1.Controls.Add(this.etiquetaLegajo);
            this.panel1.Controls.Add(this.etiquetaSimple3);
            this.panel1.Controls.Add(this.etiquetaSimple7);
            this.panel1.Controls.Add(this.etiquetaSimple1);
            this.panel1.Controls.Add(this.etiquetaSimple8);
            this.panel1.Controls.Add(this.etiquetaSimpleFrom);
            this.panel1.Controls.Add(this.dateTimePickerTo);
            this.panel1.Controls.Add(this.dateTimePickerFrom);
            this.panel1.Controls.Add(this.btnPersonalizado);
            this.panel1.Controls.Add(this.btnEsteAno);
            this.panel1.Controls.Add(this.btn30Dias);
            this.panel1.Controls.Add(this.btnEsteMes);
            this.panel1.Controls.Add(this.btn7dias);
            this.panel1.Controls.Add(this.btnToday);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(183, 768);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Acceso.Properties.Resources.Logo;
            this.pictureBox1.Location = new System.Drawing.Point(41, 639);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 117);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // comboES
            // 
            this.comboES.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboES.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboES.ForeColor = System.Drawing.Color.Black;
            this.comboES.FormattingEnabled = true;
            this.comboES.Items.AddRange(new object[] {
            "Entradas y Salidas",
            "Solo Entrada",
            "Solo Salida"});
            this.comboES.Location = new System.Drawing.Point(6, 389);
            this.comboES.Name = "comboES";
            this.comboES.Size = new System.Drawing.Size(172, 25);
            this.comboES.TabIndex = 33;
            // 
            // etiquetaNombre
            // 
            this.etiquetaNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaNombre.ForeColor = System.Drawing.Color.SteelBlue;
            this.etiquetaNombre.Location = new System.Drawing.Point(2, 609);
            this.etiquetaNombre.Name = "etiquetaNombre";
            this.etiquetaNombre.Size = new System.Drawing.Size(176, 27);
            this.etiquetaNombre.TabIndex = 6;
            this.etiquetaNombre.Text = "Nombre";
            this.etiquetaNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // etiquetaApellido
            // 
            this.etiquetaApellido.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaApellido.ForeColor = System.Drawing.Color.SteelBlue;
            this.etiquetaApellido.Location = new System.Drawing.Point(3, 565);
            this.etiquetaApellido.Name = "etiquetaApellido";
            this.etiquetaApellido.Size = new System.Drawing.Size(175, 32);
            this.etiquetaApellido.TabIndex = 6;
            this.etiquetaApellido.Text = "Apellido";
            this.etiquetaApellido.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // etiquetaLegajo
            // 
            this.etiquetaLegajo.AutoEllipsis = true;
            this.etiquetaLegajo.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaLegajo.ForeColor = System.Drawing.Color.SteelBlue;
            this.etiquetaLegajo.Location = new System.Drawing.Point(3, 491);
            this.etiquetaLegajo.Name = "etiquetaLegajo";
            this.etiquetaLegajo.Size = new System.Drawing.Size(175, 61);
            this.etiquetaLegajo.TabIndex = 5;
            this.etiquetaLegajo.Text = "Legajo #";
            this.etiquetaLegajo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // etiquetaSimple3
            // 
            this.etiquetaSimple3.AutoSize = true;
            this.etiquetaSimple3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.etiquetaSimple3.Location = new System.Drawing.Point(52, 286);
            this.etiquetaSimple3.Name = "etiquetaSimple3";
            this.etiquetaSimple3.Size = new System.Drawing.Size(64, 20);
            this.etiquetaSimple3.TabIndex = 2;
            this.etiquetaSimple3.Text = "Fechas";
            // 
            // etiquetaSimple7
            // 
            this.etiquetaSimple7.AutoSize = true;
            this.etiquetaSimple7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple7.Location = new System.Drawing.Point(3, 354);
            this.etiquetaSimple7.Name = "etiquetaSimple7";
            this.etiquetaSimple7.Size = new System.Drawing.Size(54, 20);
            this.etiquetaSimple7.TabIndex = 2;
            this.etiquetaSimple7.Text = "Hasta";
            // 
            // etiquetaSimple1
            // 
            this.etiquetaSimple1.AutoSize = true;
            this.etiquetaSimple1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple1.Location = new System.Drawing.Point(3, 352);
            this.etiquetaSimple1.Name = "etiquetaSimple1";
            this.etiquetaSimple1.Size = new System.Drawing.Size(54, 20);
            this.etiquetaSimple1.TabIndex = 2;
            this.etiquetaSimple1.Text = "Hasta";
            // 
            // etiquetaSimple8
            // 
            this.etiquetaSimple8.AutoSize = true;
            this.etiquetaSimple8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple8.Location = new System.Drawing.Point(2, 326);
            this.etiquetaSimple8.Name = "etiquetaSimple8";
            this.etiquetaSimple8.Size = new System.Drawing.Size(58, 20);
            this.etiquetaSimple8.TabIndex = 2;
            this.etiquetaSimple8.Text = "Desde";
            // 
            // etiquetaSimpleFrom
            // 
            this.etiquetaSimpleFrom.AutoSize = true;
            this.etiquetaSimpleFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimpleFrom.Location = new System.Drawing.Point(3, 326);
            this.etiquetaSimpleFrom.Name = "etiquetaSimpleFrom";
            this.etiquetaSimpleFrom.Size = new System.Drawing.Size(58, 20);
            this.etiquetaSimpleFrom.TabIndex = 2;
            this.etiquetaSimpleFrom.Text = "Desde";
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerTo.Location = new System.Drawing.Point(85, 354);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(93, 22);
            this.dateTimePickerTo.TabIndex = 1;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(85, 326);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(93, 22);
            this.dateTimePickerFrom.TabIndex = 1;
            // 
            // btnPersonalizado
            // 
            this.btnPersonalizado.AutoEllipsis = true;
            this.btnPersonalizado.BackColor = System.Drawing.Color.SlateBlue;
            this.btnPersonalizado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPersonalizado.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnPersonalizado.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.btnPersonalizado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPersonalizado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPersonalizado.ForeColor = System.Drawing.Color.White;
            this.btnPersonalizado.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPersonalizado.Location = new System.Drawing.Point(3, 430);
            this.btnPersonalizado.Name = "btnPersonalizado";
            this.btnPersonalizado.Size = new System.Drawing.Size(175, 40);
            this.btnPersonalizado.TabIndex = 0;
            this.btnPersonalizado.Text = "Aplicar";
            this.btnPersonalizado.UseVisualStyleBackColor = false;
            this.btnPersonalizado.Click += new System.EventHandler(this.btnPersonalizado_Click);
            // 
            // btnEsteAno
            // 
            this.btnEsteAno.AutoEllipsis = true;
            this.btnEsteAno.BackColor = System.Drawing.Color.SlateBlue;
            this.btnEsteAno.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEsteAno.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnEsteAno.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.btnEsteAno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEsteAno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEsteAno.ForeColor = System.Drawing.Color.White;
            this.btnEsteAno.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEsteAno.Location = new System.Drawing.Point(3, 222);
            this.btnEsteAno.Name = "btnEsteAno";
            this.btnEsteAno.Size = new System.Drawing.Size(175, 40);
            this.btnEsteAno.TabIndex = 0;
            this.btnEsteAno.Text = "Este año";
            this.btnEsteAno.UseVisualStyleBackColor = false;
            this.btnEsteAno.Click += new System.EventHandler(this.btnEsteAno_Click);
            // 
            // btn30Dias
            // 
            this.btn30Dias.AutoEllipsis = true;
            this.btn30Dias.BackColor = System.Drawing.Color.SlateBlue;
            this.btn30Dias.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn30Dias.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btn30Dias.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.btn30Dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn30Dias.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn30Dias.ForeColor = System.Drawing.Color.White;
            this.btn30Dias.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn30Dias.Location = new System.Drawing.Point(3, 177);
            this.btn30Dias.Name = "btn30Dias";
            this.btn30Dias.Size = new System.Drawing.Size(175, 40);
            this.btn30Dias.TabIndex = 0;
            this.btn30Dias.Text = "Ultimos 30 dias";
            this.btn30Dias.UseVisualStyleBackColor = false;
            this.btn30Dias.Click += new System.EventHandler(this.btn30Dias_Click);
            // 
            // btnEsteMes
            // 
            this.btnEsteMes.AutoEllipsis = true;
            this.btnEsteMes.BackColor = System.Drawing.Color.SlateBlue;
            this.btnEsteMes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEsteMes.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnEsteMes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.btnEsteMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEsteMes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEsteMes.ForeColor = System.Drawing.Color.White;
            this.btnEsteMes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEsteMes.Location = new System.Drawing.Point(3, 131);
            this.btnEsteMes.Name = "btnEsteMes";
            this.btnEsteMes.Size = new System.Drawing.Size(175, 40);
            this.btnEsteMes.TabIndex = 0;
            this.btnEsteMes.Text = "Este mes";
            this.btnEsteMes.UseVisualStyleBackColor = false;
            this.btnEsteMes.Click += new System.EventHandler(this.btnEsteMes_Click);
            // 
            // btn7dias
            // 
            this.btn7dias.AutoEllipsis = true;
            this.btn7dias.BackColor = System.Drawing.Color.SlateBlue;
            this.btn7dias.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn7dias.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btn7dias.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.btn7dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn7dias.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7dias.ForeColor = System.Drawing.Color.White;
            this.btn7dias.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn7dias.Location = new System.Drawing.Point(3, 85);
            this.btn7dias.Name = "btn7dias";
            this.btn7dias.Size = new System.Drawing.Size(175, 40);
            this.btn7dias.TabIndex = 0;
            this.btn7dias.Text = "Ultimos 7 dias";
            this.btn7dias.UseVisualStyleBackColor = false;
            this.btn7dias.Click += new System.EventHandler(this.btn7dias_Click);
            // 
            // btnToday
            // 
            this.btnToday.AutoEllipsis = true;
            this.btnToday.BackColor = System.Drawing.Color.SlateBlue;
            this.btnToday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToday.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnToday.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToday.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToday.ForeColor = System.Drawing.Color.White;
            this.btnToday.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnToday.Location = new System.Drawing.Point(3, 39);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(175, 40);
            this.btnToday.TabIndex = 0;
            this.btnToday.Text = "Hoy";
            this.btnToday.UseVisualStyleBackColor = false;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "HistorialLegajo";
            reportDataSource1.Value = this.RRHHFichadasListingBindingSource;
            reportDataSource2.Name = "fichadasReport";
            reportDataSource2.Value = this.RRHHFichadasReportBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Acceso.Reportes.HistorialLegajo.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(183, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(933, 768);
            this.reportViewer1.TabIndex = 3;
            // 
            // frmFichadasHistorialLegajo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1116, 768);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmFichadasHistorialLegajo";
            this.Load += new System.EventHandler(this.frmFichadasHistorialLegajo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RRHHFichadasListingBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RRHHFichadasReportBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Global.Controles.EtiquetaSimple etiquetaSimple3;
        private Global.Controles.EtiquetaSimple etiquetaSimple7;
        private Global.Controles.EtiquetaSimple etiquetaSimple1;
        private Global.Controles.EtiquetaSimple etiquetaSimple8;
        private Global.Controles.EtiquetaSimple etiquetaSimpleFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private Global.Controles.BotonMenuVertical btnPersonalizado;
        private Global.Controles.BotonMenuVertical btnEsteAno;
        private Global.Controles.BotonMenuVertical btn30Dias;
        private Global.Controles.BotonMenuVertical btnEsteMes;
        private Global.Controles.BotonMenuVertical btn7dias;
        private Global.Controles.BotonMenuVertical btnToday;
        private Global.Controles.EtiquetaSimple etiquetaNombre;
        private Global.Controles.EtiquetaSimple etiquetaApellido;
        private Global.Controles.EtiquetaTitulo etiquetaLegajo;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource RRHHFichadasListingBindingSource;
        private System.Windows.Forms.BindingSource RRHHFichadasReportBindingSource;
        private Global.Controles.ComboDesplegable comboES;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
