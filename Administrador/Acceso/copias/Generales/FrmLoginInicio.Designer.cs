namespace Acceso.Generales
{
	partial class FrmLoginInicio
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
			this.SuspendLayout();
			// 
			// botonAceptar
			// 
			this.botonAceptar.Click += new System.EventHandler(this.botonAceptar_Click);
			// 
			// botonSalir
			// 
			this.botonSalir.Click += new System.EventHandler(this.botonSalir_Click);
			// 
			// FrmLoginInicio
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.ClientSize = new System.Drawing.Size(850, 411);
			this.Name = "FrmLoginInicio";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
	}
}
