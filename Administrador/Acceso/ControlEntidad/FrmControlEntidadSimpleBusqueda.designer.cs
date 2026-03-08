namespace Acceso.ControlEntidad
{
	partial class FrmControlEntidadSimpleBusqueda
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.botonBase1 = new Global.Controles.BotonBase();
            this.controlEntidadBusqueda_01 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            //
            // botonBase1
            //
            this.botonBase1.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonBase1.Location = new System.Drawing.Point(393, 98);
            this.botonBase1.Name = "botonBase1";
            this.botonBase1.Size = new System.Drawing.Size(120, 40);
            this.botonBase1.TabIndex = 1;
            this.botonBase1.Text = "Buscar";
            this.botonBase1.UseVisualStyleBackColor = true;
            this.botonBase1.Click += new System.EventHandler(this.botonBase1_Click);
            //
            // controlEntidadBusqueda_01
            //
            this.controlEntidadBusqueda_01.Location = new System.Drawing.Point(18, 12);
            this.controlEntidadBusqueda_01.Name = "controlEntidadBusqueda_01";
            this.controlEntidadBusqueda_01.Size = new System.Drawing.Size(495, 80);
            this.controlEntidadBusqueda_01.TabIndex = 0;
            //
            // FrmControlEntidadSimpleBusqueda
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(529, 151);
            this.Controls.Add(this.botonBase1);
            this.Controls.Add(this.controlEntidadBusqueda_01);
            this.Name = "FrmControlEntidadSimpleBusqueda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmControlEntidadSimpleBusqueda";
            this.ResumeLayout(false);

		}

		#endregion
		private Global.Controles.BotonBase botonBase1;
        public System.Windows.Forms.Panel controlEntidadBusqueda_01;
	}
}
