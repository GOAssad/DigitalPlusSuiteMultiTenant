namespace Global.Controles.FechayHora
{
	partial class HoraMinutos
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
			this.numHora = new Global.Controles.Numerico();
			this.numMinuto = new Global.Controles.Numerico();
			this.etiquetaSimple1 = new Global.Controles.EtiquetaSimple();
			this.etiquetaSimple2 = new Global.Controles.EtiquetaSimple();
			((System.ComponentModel.ISupportInitialize)(this.numHora)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMinuto)).BeginInit();
			this.SuspendLayout();
			// 
			// numHora
			// 
			this.numHora.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.numHora.Location = new System.Drawing.Point(40, 0);
			this.numHora.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
			this.numHora.Name = "numHora";
			this.numHora.Size = new System.Drawing.Size(46, 22);
			this.numHora.TabIndex = 0;
			this.numHora.ValueChanged += new System.EventHandler(this.numHora_ValueChanged);
			this.numHora.Enter += new System.EventHandler(this.numHora_Enter);
			// 
			// numMinuto
			// 
			this.numMinuto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.numMinuto.Location = new System.Drawing.Point(110, 0);
			this.numMinuto.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
			this.numMinuto.Name = "numMinuto";
			this.numMinuto.Size = new System.Drawing.Size(46, 22);
			this.numMinuto.TabIndex = 1;
			this.numMinuto.ValueChanged += new System.EventHandler(this.numMinuto_ValueChanged);
			// 
			// etiquetaSimple1
			// 
			this.etiquetaSimple1.AutoSize = true;
			this.etiquetaSimple1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetaSimple1.Location = new System.Drawing.Point(0, 0);
			this.etiquetaSimple1.Name = "etiquetaSimple1";
			this.etiquetaSimple1.Size = new System.Drawing.Size(37, 20);
			this.etiquetaSimple1.TabIndex = 2;
			this.etiquetaSimple1.Text = "h/m";
			// 
			// etiquetaSimple2
			// 
			this.etiquetaSimple2.AutoSize = true;
			this.etiquetaSimple2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetaSimple2.Location = new System.Drawing.Point(90, 0);
			this.etiquetaSimple2.Name = "etiquetaSimple2";
			this.etiquetaSimple2.Size = new System.Drawing.Size(15, 20);
			this.etiquetaSimple2.TabIndex = 3;
			this.etiquetaSimple2.Text = ":";
			// 
			// HoraMinutos
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.etiquetaSimple2);
			this.Controls.Add(this.etiquetaSimple1);
			this.Controls.Add(this.numMinuto);
			this.Controls.Add(this.numHora);
			this.Name = "HoraMinutos";
			this.Size = new System.Drawing.Size(160, 27);
			this.Load += new System.EventHandler(this.HoraMinutos_Load);
			((System.ComponentModel.ISupportInitialize)(this.numHora)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMinuto)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		public Numerico numHora;
		public Numerico numMinuto;
		private EtiquetaSimple etiquetaSimple1;
		private EtiquetaSimple etiquetaSimple2;
	}
}
