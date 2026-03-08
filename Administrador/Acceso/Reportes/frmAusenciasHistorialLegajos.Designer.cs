namespace Acceso.Reportes
{
    partial class frmAusenciasHistorialLegajos
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
            this.RRHHAusenciasLegajoListingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labelAusencias = new System.Windows.Forms.Label();
            this.chkIncidencia = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.RRHHAusenciasLegajoListingBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // RRHHAusenciasLegajoListingBindingSource
            // 
            this.RRHHAusenciasLegajoListingBindingSource.DataSource = typeof(Acceso.Clases.Datos.RRHH.RRHHAusenciasLegajoListing);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.labelAusencias);
            this.panel1.Controls.Add(this.chkIncidencia);
            this.panel1.Controls.Add(this.pictureBox1);
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 768);
            this.panel1.TabIndex = 3;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Acceso.Properties.Resources.Logo;
            this.pictureBox2.Location = new System.Drawing.Point(36, 600);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 117);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 38;
            this.pictureBox2.TabStop = false;
            // 
            // labelAusencias
            // 
            this.labelAusencias.AutoSize = true;
            this.labelAusencias.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAusencias.ForeColor = System.Drawing.Color.Crimson;
            this.labelAusencias.Location = new System.Drawing.Point(85, 180);
            this.labelAusencias.Name = "labelAusencias";
            this.labelAusencias.Size = new System.Drawing.Size(35, 38);
            this.labelAusencias.TabIndex = 37;
            this.labelAusencias.Text = "0";
            this.labelAusencias.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkIncidencia
            // 
            this.chkIncidencia.AutoSize = true;
            this.chkIncidencia.Location = new System.Drawing.Point(48, 281);
            this.chkIncidencia.Name = "chkIncidencia";
            this.chkIncidencia.Size = new System.Drawing.Size(141, 21);
            this.chkIncidencia.TabIndex = 36;
            this.chkIncidencia.Text = "Incluir Incidencias";
            this.chkIncidencia.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Acceso.Properties.Resources.Incidencia030x030;
            this.pictureBox1.Location = new System.Drawing.Point(12, 272);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 35;
            this.pictureBox1.TabStop = false;
            // 
            // etiquetaNombre
            // 
            this.etiquetaNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaNombre.ForeColor = System.Drawing.Color.SteelBlue;
            this.etiquetaNombre.Location = new System.Drawing.Point(2, 127);
            this.etiquetaNombre.Name = "etiquetaNombre";
            this.etiquetaNombre.Size = new System.Drawing.Size(175, 27);
            this.etiquetaNombre.TabIndex = 6;
            this.etiquetaNombre.Text = "Nombre";
            this.etiquetaNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // etiquetaApellido
            // 
            this.etiquetaApellido.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaApellido.ForeColor = System.Drawing.Color.SteelBlue;
            this.etiquetaApellido.Location = new System.Drawing.Point(3, 83);
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
            this.etiquetaLegajo.Location = new System.Drawing.Point(3, 9);
            this.etiquetaLegajo.Name = "etiquetaLegajo";
            this.etiquetaLegajo.Size = new System.Drawing.Size(218, 61);
            this.etiquetaLegajo.TabIndex = 5;
            this.etiquetaLegajo.Text = "Legjao #";
            this.etiquetaLegajo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // etiquetaSimple3
            // 
            this.etiquetaSimple3.AutoSize = true;
            this.etiquetaSimple3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.etiquetaSimple3.Location = new System.Drawing.Point(72, 330);
            this.etiquetaSimple3.Name = "etiquetaSimple3";
            this.etiquetaSimple3.Size = new System.Drawing.Size(64, 20);
            this.etiquetaSimple3.TabIndex = 2;
            this.etiquetaSimple3.Text = "Fechas";
            // 
            // etiquetaSimple7
            // 
            this.etiquetaSimple7.AutoSize = true;
            this.etiquetaSimple7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple7.Location = new System.Drawing.Point(3, 383);
            this.etiquetaSimple7.Name = "etiquetaSimple7";
            this.etiquetaSimple7.Size = new System.Drawing.Size(54, 20);
            this.etiquetaSimple7.TabIndex = 2;
            this.etiquetaSimple7.Text = "Hasta";
            // 
            // etiquetaSimple1
            // 
            this.etiquetaSimple1.AutoSize = true;
            this.etiquetaSimple1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple1.Location = new System.Drawing.Point(3, 381);
            this.etiquetaSimple1.Name = "etiquetaSimple1";
            this.etiquetaSimple1.Size = new System.Drawing.Size(54, 20);
            this.etiquetaSimple1.TabIndex = 2;
            this.etiquetaSimple1.Text = "Hasta";
            // 
            // etiquetaSimple8
            // 
            this.etiquetaSimple8.AutoSize = true;
            this.etiquetaSimple8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple8.Location = new System.Drawing.Point(2, 355);
            this.etiquetaSimple8.Name = "etiquetaSimple8";
            this.etiquetaSimple8.Size = new System.Drawing.Size(58, 20);
            this.etiquetaSimple8.TabIndex = 2;
            this.etiquetaSimple8.Text = "Desde";
            // 
            // etiquetaSimpleFrom
            // 
            this.etiquetaSimpleFrom.AutoSize = true;
            this.etiquetaSimpleFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimpleFrom.Location = new System.Drawing.Point(3, 355);
            this.etiquetaSimpleFrom.Name = "etiquetaSimpleFrom";
            this.etiquetaSimpleFrom.Size = new System.Drawing.Size(58, 20);
            this.etiquetaSimpleFrom.TabIndex = 2;
            this.etiquetaSimpleFrom.Text = "Desde";
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerTo.Location = new System.Drawing.Point(74, 381);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(104, 22);
            this.dateTimePickerTo.TabIndex = 1;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(74, 353);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(104, 22);
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
            this.btnPersonalizado.Location = new System.Drawing.Point(3, 459);
            this.btnPersonalizado.Name = "btnPersonalizado";
            this.btnPersonalizado.Size = new System.Drawing.Size(175, 40);
            this.btnPersonalizado.TabIndex = 0;
            this.btnPersonalizado.Text = "Aplicar";
            this.btnPersonalizado.UseVisualStyleBackColor = false;
            this.btnPersonalizado.Click += new System.EventHandler(this.btnPersonalizado_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "RRHHAusenciasLegajoListing";
            reportDataSource1.Value = this.RRHHAusenciasLegajoListingBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Acceso.Reportes.AusenciaLegajoReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(188, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(889, 768);
            this.reportViewer1.TabIndex = 4;
            // 
            // frmAusenciasHistorialLegajos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1077, 768);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmAusenciasHistorialLegajos";
            this.Load += new System.EventHandler(this.frmAusenciasHistorialLegajos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RRHHAusenciasLegajoListingBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Global.Controles.EtiquetaSimple etiquetaNombre;
        private Global.Controles.EtiquetaSimple etiquetaApellido;
        private Global.Controles.EtiquetaTitulo etiquetaLegajo;
        private Global.Controles.EtiquetaSimple etiquetaSimple3;
        private Global.Controles.EtiquetaSimple etiquetaSimple7;
        private Global.Controles.EtiquetaSimple etiquetaSimple1;
        private Global.Controles.EtiquetaSimple etiquetaSimple8;
        private Global.Controles.EtiquetaSimple etiquetaSimpleFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private Global.Controles.BotonMenuVertical btnPersonalizado;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource RRHHAusenciasLegajoListingBindingSource;
        private System.Windows.Forms.CheckBox chkIncidencia;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelAusencias;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}
