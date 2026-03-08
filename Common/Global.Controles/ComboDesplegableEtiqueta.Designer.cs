namespace Global.Controles
{
    partial class ComboDesplegableEtiqueta
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
            this.comboDesplegable = new Global.Controles.ComboDesplegable();
            this.etiquetaEntidad = new Global.Controles.EtiquetaSimple();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboDesplegable
            // 
            this.comboDesplegable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboDesplegable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDesplegable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboDesplegable.ForeColor = System.Drawing.Color.Black;
            this.comboDesplegable.FormattingEnabled = true;
            this.comboDesplegable.Location = new System.Drawing.Point(3, 27);
            this.comboDesplegable.Name = "comboDesplegable";
            this.comboDesplegable.Size = new System.Drawing.Size(389, 28);
            this.comboDesplegable.TabIndex = 0;
            this.comboDesplegable.SelectedIndexChanged += new System.EventHandler(this.comboDesplegable_SelectedIndexChanged);
            // 
            // etiquetaEntidad
            // 
            this.etiquetaEntidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.etiquetaEntidad.AutoSize = true;
            this.etiquetaEntidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaEntidad.Location = new System.Drawing.Point(3, 0);
            this.etiquetaEntidad.Name = "etiquetaEntidad";
            this.etiquetaEntidad.Size = new System.Drawing.Size(389, 20);
            this.etiquetaEntidad.TabIndex = 1;
            this.etiquetaEntidad.Text = "Descripcion";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.comboDesplegable, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.etiquetaEntidad, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.92857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.07143F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(395, 62);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // ComboDesplegableEtiqueta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ComboDesplegableEtiqueta";
            this.Size = new System.Drawing.Size(395, 62);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private EtiquetaSimple etiquetaEntidad;
        private ComboDesplegable comboDesplegable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
