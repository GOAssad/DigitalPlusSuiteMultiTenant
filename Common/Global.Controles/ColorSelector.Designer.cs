namespace Global.Controles
{
    partial class ColorSelector
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
            this.etiquetaSimple1 = new Global.Controles.EtiquetaSimple();
            this.tbRed = new System.Windows.Forms.TrackBar();
            this.etiquetaSimple2 = new Global.Controles.EtiquetaSimple();
            this.TbGreen = new System.Windows.Forms.TrackBar();
            this.etiquetaSimple3 = new Global.Controles.EtiquetaSimple();
            this.TbBlue = new System.Windows.Forms.TrackBar();
            this.pnColor = new System.Windows.Forms.Panel();
            this.etiquetaTitulo = new Global.Controles.EtiquetaTitulo();
            this.lblRed = new Global.Controles.EtiquetaSimple();
            this.lblGreen = new Global.Controles.EtiquetaSimple();
            this.lblBlue = new Global.Controles.EtiquetaSimple();
            this.txtColor = new Global.Controles.TextoSimple();
            ((System.ComponentModel.ISupportInitialize)(this.tbRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbBlue)).BeginInit();
            this.SuspendLayout();
            // 
            // etiquetaSimple1
            // 
            this.etiquetaSimple1.AutoSize = true;
            this.etiquetaSimple1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple1.Location = new System.Drawing.Point(4, 43);
            this.etiquetaSimple1.Name = "etiquetaSimple1";
            this.etiquetaSimple1.Size = new System.Drawing.Size(43, 20);
            this.etiquetaSimple1.TabIndex = 0;
            this.etiquetaSimple1.Text = "Rojo";
            // 
            // tbRed
            // 
            this.tbRed.Location = new System.Drawing.Point(3, 66);
            this.tbRed.Maximum = 255;
            this.tbRed.Name = "tbRed";
            this.tbRed.Size = new System.Drawing.Size(266, 56);
            this.tbRed.TabIndex = 1;
            this.tbRed.TickFrequency = 5;
            this.tbRed.Scroll += new System.EventHandler(this.tb_Scroll);
            // 
            // etiquetaSimple2
            // 
            this.etiquetaSimple2.AutoSize = true;
            this.etiquetaSimple2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple2.Location = new System.Drawing.Point(5, 125);
            this.etiquetaSimple2.Name = "etiquetaSimple2";
            this.etiquetaSimple2.Size = new System.Drawing.Size(53, 20);
            this.etiquetaSimple2.TabIndex = 0;
            this.etiquetaSimple2.Text = "Verde";
            // 
            // TbGreen
            // 
            this.TbGreen.Location = new System.Drawing.Point(3, 153);
            this.TbGreen.Maximum = 255;
            this.TbGreen.Name = "TbGreen";
            this.TbGreen.Size = new System.Drawing.Size(263, 56);
            this.TbGreen.TabIndex = 1;
            this.TbGreen.TickFrequency = 5;
            this.TbGreen.Scroll += new System.EventHandler(this.tb_Scroll);
            // 
            // etiquetaSimple3
            // 
            this.etiquetaSimple3.AutoSize = true;
            this.etiquetaSimple3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaSimple3.Location = new System.Drawing.Point(5, 212);
            this.etiquetaSimple3.Name = "etiquetaSimple3";
            this.etiquetaSimple3.Size = new System.Drawing.Size(42, 20);
            this.etiquetaSimple3.TabIndex = 0;
            this.etiquetaSimple3.Text = "Azul";
            // 
            // TbBlue
            // 
            this.TbBlue.Location = new System.Drawing.Point(3, 236);
            this.TbBlue.Maximum = 255;
            this.TbBlue.Name = "TbBlue";
            this.TbBlue.Size = new System.Drawing.Size(263, 56);
            this.TbBlue.TabIndex = 1;
            this.TbBlue.TickFrequency = 5;
            this.TbBlue.Scroll += new System.EventHandler(this.tb_Scroll);
            // 
            // pnColor
            // 
            this.pnColor.Location = new System.Drawing.Point(289, 66);
            this.pnColor.Name = "pnColor";
            this.pnColor.Size = new System.Drawing.Size(105, 99);
            this.pnColor.TabIndex = 2;
            // 
            // etiquetaTitulo
            // 
            this.etiquetaTitulo.AutoEllipsis = true;
            this.etiquetaTitulo.AutoSize = true;
            this.etiquetaTitulo.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.etiquetaTitulo.ForeColor = System.Drawing.Color.SteelBlue;
            this.etiquetaTitulo.Location = new System.Drawing.Point(3, 0);
            this.etiquetaTitulo.Name = "etiquetaTitulo";
            this.etiquetaTitulo.Size = new System.Drawing.Size(23, 38);
            this.etiquetaTitulo.TabIndex = 3;
            this.etiquetaTitulo.Text = ".";
            // 
            // lblRed
            // 
            this.lblRed.AutoSize = true;
            this.lblRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRed.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblRed.Location = new System.Drawing.Point(211, 43);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(18, 20);
            this.lblRed.TabIndex = 4;
            this.lblRed.Text = "0";
            // 
            // lblGreen
            // 
            this.lblGreen.AutoSize = true;
            this.lblGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreen.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblGreen.Location = new System.Drawing.Point(211, 130);
            this.lblGreen.Name = "lblGreen";
            this.lblGreen.Size = new System.Drawing.Size(18, 20);
            this.lblGreen.TabIndex = 4;
            this.lblGreen.Text = "0";
            // 
            // lblBlue
            // 
            this.lblBlue.AutoSize = true;
            this.lblBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlue.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblBlue.Location = new System.Drawing.Point(211, 213);
            this.lblBlue.Name = "lblBlue";
            this.lblBlue.Size = new System.Drawing.Size(18, 20);
            this.lblBlue.TabIndex = 4;
            this.lblBlue.Text = "0";
            // 
            // txtColor
            // 
            this.txtColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtColor.Location = new System.Drawing.Point(289, 236);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(100, 27);
            this.txtColor.TabIndex = 5;
            // 
            // ColorSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.lblBlue);
            this.Controls.Add(this.lblGreen);
            this.Controls.Add(this.lblRed);
            this.Controls.Add(this.etiquetaTitulo);
            this.Controls.Add(this.etiquetaSimple3);
            this.Controls.Add(this.etiquetaSimple2);
            this.Controls.Add(this.etiquetaSimple1);
            this.Controls.Add(this.pnColor);
            this.Controls.Add(this.TbBlue);
            this.Controls.Add(this.TbGreen);
            this.Controls.Add(this.tbRed);
            this.Name = "ColorSelector";
            this.Size = new System.Drawing.Size(408, 295);
            ((System.ComponentModel.ISupportInitialize)(this.tbRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbBlue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EtiquetaSimple etiquetaSimple1;
        private System.Windows.Forms.TrackBar tbRed;
        private EtiquetaSimple etiquetaSimple2;
        private System.Windows.Forms.TrackBar TbGreen;
        private EtiquetaSimple etiquetaSimple3;
        private System.Windows.Forms.TrackBar TbBlue;
        private System.Windows.Forms.Panel pnColor;
        private EtiquetaTitulo etiquetaTitulo;
        private EtiquetaSimple lblRed;
        private EtiquetaSimple lblGreen;
        private EtiquetaSimple lblBlue;
        private TextoSimple txtColor;
    }
}
