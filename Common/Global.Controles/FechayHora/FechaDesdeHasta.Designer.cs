namespace Global.Controles.FechayHora
{
	partial class FechaDesdeHasta
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

		#region Código generado por el Diseñador de componentes

		/// <summary> 
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblDiaNombreHasta = new Global.Controles.EtiquetaSimple();
            this.etiquetaSimple2 = new Global.Controles.EtiquetaSimple();
            this.lblDiaNombreDesde = new Global.Controles.EtiquetaSimple();
            this.etiquetaSimple1 = new Global.Controles.EtiquetaSimple();
            this.pickerHasta = new Global.Controles.PickerFechaHora();
            this.pickerDesde = new Global.Controles.PickerFechaHora();
            this.SuspendLayout();
            // 
            // lblDiaNombreHasta
            // 
            this.lblDiaNombreHasta.AutoSize = true;
            this.lblDiaNombreHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiaNombreHasta.Location = new System.Drawing.Point(130, 51);
            this.lblDiaNombreHasta.Name = "lblDiaNombreHasta";
            this.lblDiaNombreHasta.Size = new System.Drawing.Size(13, 20);
            this.lblDiaNombreHasta.TabIndex = 1;
            this.lblDiaNombreHasta.Text = ".";
            // 
            // etiquetaSimple2
            // 
            this.etiquetaSimple2.AutoSize = true;
            this.etiquetaSimple2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple2.Location = new System.Drawing.Point(130, 0);
            this.etiquetaSimple2.Name = "etiquetaSimple2";
            this.etiquetaSimple2.Size = new System.Drawing.Size(54, 20);
            this.etiquetaSimple2.TabIndex = 1;
            this.etiquetaSimple2.Text = "Hasta";
            // 
            // lblDiaNombreDesde
            // 
            this.lblDiaNombreDesde.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDiaNombreDesde.AutoSize = true;
            this.lblDiaNombreDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiaNombreDesde.Location = new System.Drawing.Point(0, 51);
            this.lblDiaNombreDesde.Name = "lblDiaNombreDesde";
            this.lblDiaNombreDesde.Size = new System.Drawing.Size(13, 20);
            this.lblDiaNombreDesde.TabIndex = 1;
            this.lblDiaNombreDesde.Text = ".";
            // 
            // etiquetaSimple1
            // 
            this.etiquetaSimple1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.etiquetaSimple1.AutoSize = true;
            this.etiquetaSimple1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple1.Location = new System.Drawing.Point(0, 0);
            this.etiquetaSimple1.Name = "etiquetaSimple1";
            this.etiquetaSimple1.Size = new System.Drawing.Size(58, 20);
            this.etiquetaSimple1.TabIndex = 1;
            this.etiquetaSimple1.Text = "Desde";
            // 
            // pickerHasta
            // 
            this.pickerHasta.DiaNombre = "Domingo";
            this.pickerHasta.DiaResumido = "Dom";
            this.pickerHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.pickerHasta.Location = new System.Drawing.Point(123, 24);
            this.pickerHasta.Name = "pickerHasta";
            this.pickerHasta.Size = new System.Drawing.Size(115, 22);
            this.pickerHasta.TabIndex = 0;
            this.pickerHasta.ValueChanged += new System.EventHandler(this.pickerHasta_ValueChanged);
            // 
            // pickerDesde
            // 
            this.pickerDesde.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pickerDesde.DiaNombre = "Domingo";
            this.pickerDesde.DiaResumido = "Dom";
            this.pickerDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.pickerDesde.Location = new System.Drawing.Point(0, 24);
            this.pickerDesde.Name = "pickerDesde";
            this.pickerDesde.Size = new System.Drawing.Size(115, 22);
            this.pickerDesde.TabIndex = 0;
            // 
            // FechaDesdeHasta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.lblDiaNombreHasta);
            this.Controls.Add(this.etiquetaSimple2);
            this.Controls.Add(this.lblDiaNombreDesde);
            this.Controls.Add(this.etiquetaSimple1);
            this.Controls.Add(this.pickerHasta);
            this.Controls.Add(this.pickerDesde);
            this.Name = "FechaDesdeHasta";
            this.Size = new System.Drawing.Size(245, 88);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private PickerFechaHora pickerDesde;
		private PickerFechaHora pickerHasta;
		private EtiquetaSimple etiquetaSimple1;
		private EtiquetaSimple etiquetaSimple2;
		private EtiquetaSimple lblDiaNombreDesde;
		private EtiquetaSimple lblDiaNombreHasta;
	}
}
