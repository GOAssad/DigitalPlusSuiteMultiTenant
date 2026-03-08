namespace Acceso.ControlEntidad
{
    partial class CargarImagenBasedeDatos
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
            this.tableLayoutBase = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutBotones = new System.Windows.Forms.TableLayoutPanel();
            this.btnBuscar = new Global.Controles.BotonBase();
            this.picFoto = new System.Windows.Forms.PictureBox();
            this.tableLayoutBase.SuspendLayout();
            this.tableLayoutBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutBase
            // 
            this.tableLayoutBase.ColumnCount = 1;
            this.tableLayoutBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutBase.Controls.Add(this.tableLayoutBotones, 0, 1);
            this.tableLayoutBase.Controls.Add(this.picFoto, 0, 0);
            this.tableLayoutBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutBase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutBase.Name = "tableLayoutBase";
            this.tableLayoutBase.RowCount = 2;
            this.tableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutBase.Size = new System.Drawing.Size(159, 210);
            this.tableLayoutBase.TabIndex = 0;
            // 
            // tableLayoutBotones
            // 
            this.tableLayoutBotones.ColumnCount = 3;
            this.tableLayoutBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutBotones.Controls.Add(this.btnBuscar, 1, 0);
            this.tableLayoutBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutBotones.Location = new System.Drawing.Point(3, 150);
            this.tableLayoutBotones.Name = "tableLayoutBotones";
            this.tableLayoutBotones.RowCount = 1;
            this.tableLayoutBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutBotones.Size = new System.Drawing.Size(153, 57);
            this.tableLayoutBotones.TabIndex = 0;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBuscar.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.Image = global::Acceso.Properties.Resources.Folder035x022;
            this.btnBuscar.Location = new System.Drawing.Point(53, 3);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(44, 51);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // picFoto
            // 
            this.picFoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picFoto.Location = new System.Drawing.Point(3, 3);
            this.picFoto.Name = "picFoto";
            this.picFoto.Size = new System.Drawing.Size(153, 141);
            this.picFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picFoto.TabIndex = 1;
            this.picFoto.TabStop = false;
            // 
            // CargarImagenBasedeDatos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutBase);
            this.Name = "CargarImagenBasedeDatos";
            this.Size = new System.Drawing.Size(159, 210);
            this.tableLayoutBase.ResumeLayout(false);
            this.tableLayoutBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutBase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutBotones;
        private Global.Controles.BotonBase btnBuscar;
        public System.Windows.Forms.PictureBox picFoto;
    }
}
