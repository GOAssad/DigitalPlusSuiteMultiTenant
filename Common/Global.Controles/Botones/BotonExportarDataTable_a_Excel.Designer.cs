namespace Global.Controles.Botones
{
    partial class BotonExportarDataTable_a_Excel
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
            this.botonExcel = new Global.Controles.Botones.BotonBaseXuXo();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // botonExcel
            // 
            this.botonExcel.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.botonExcel.Dock = System.Windows.Forms.DockStyle.Top;
            this.botonExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botonExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.botonExcel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.botonExcel.Location = new System.Drawing.Point(0, 0);
            this.botonExcel.Name = "botonExcel";
            this.botonExcel.Size = new System.Drawing.Size(145, 38);
            this.botonExcel.TabIndex = 0;
            this.botonExcel.Text = "EXCEL";
            this.botonExcel.UseVisualStyleBackColor = false;
            this.botonExcel.Click += new System.EventHandler(this.botonExcel_Click);
            // 
            // BotonExportarDataTable_a_Excel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.botonExcel);
            this.Name = "BotonExportarDataTable_a_Excel";
            this.Size = new System.Drawing.Size(145, 38);
            this.Load += new System.EventHandler(this.BotonExportarDataTable_a_Excel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private BotonBaseXuXo botonExcel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
