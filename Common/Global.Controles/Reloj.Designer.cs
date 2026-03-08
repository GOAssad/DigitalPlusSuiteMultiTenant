namespace Global.Controles
{
	partial class Reloj
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
			this.components = new System.ComponentModel.Container();
			this.etiquetaReloj = new Global.Controles.EtiquetaTitulo();
			this.etiquetadia = new Global.Controles.EtiquetaSimple();
			this.etiquetafecha = new Global.Controles.EtiquetaSimple();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// etiquetaReloj
			// 
			this.etiquetaReloj.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetaReloj.ForeColor = System.Drawing.Color.SteelBlue;
			this.etiquetaReloj.Location = new System.Drawing.Point(0, 14);
			this.etiquetaReloj.Name = "etiquetaReloj";
			this.etiquetaReloj.Size = new System.Drawing.Size(294, 75);
			this.etiquetaReloj.TabIndex = 0;
			this.etiquetaReloj.Text = "etiquetaTitulo1";
			this.etiquetaReloj.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// etiquetadia
			// 
			this.etiquetadia.AutoSize = true;
			this.etiquetadia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetadia.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.etiquetadia.Location = new System.Drawing.Point(13, 103);
			this.etiquetadia.Name = "etiquetadia";
			this.etiquetadia.Size = new System.Drawing.Size(128, 20);
			this.etiquetadia.TabIndex = 1;
			this.etiquetadia.Text = "etiquetaSimple1";
			// 
			// etiquetafecha
			// 
			this.etiquetafecha.AutoSize = true;
			this.etiquetafecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.etiquetafecha.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.etiquetafecha.Location = new System.Drawing.Point(147, 103);
			this.etiquetafecha.Name = "etiquetafecha";
			this.etiquetafecha.Size = new System.Drawing.Size(128, 20);
			this.etiquetafecha.TabIndex = 1;
			this.etiquetafecha.Text = "etiquetaSimple1";
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// Reloj
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.etiquetafecha);
			this.Controls.Add(this.etiquetadia);
			this.Controls.Add(this.etiquetaReloj);
			this.Name = "Reloj";
			this.Size = new System.Drawing.Size(297, 150);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Timer timer1;
		public EtiquetaTitulo etiquetaReloj;
		public EtiquetaSimple etiquetadia;
		public EtiquetaSimple etiquetafecha;
	}
}
