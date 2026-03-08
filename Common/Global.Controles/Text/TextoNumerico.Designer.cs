namespace Global.Controles
{
	partial class TextoNumerico
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
			this.SuspendLayout();
			// 
			// TextoNumerico
			// 
			this.Enter += new System.EventHandler(this.TextoNumerico_Enter);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextoNumerico_KeyPress);
			this.Leave += new System.EventHandler(this.TextoNumerico_Leave);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
