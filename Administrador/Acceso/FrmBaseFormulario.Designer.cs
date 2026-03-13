namespace Acceso
{
    partial class FrmBaseFormulario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBaseFormulario));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonEliminar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNuevo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGuardar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonActualizar = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelTitulo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonSalir = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonEliminar,
            this.toolStripButtonNuevo,
            this.toolStripButtonGuardar,
            this.toolStripButtonActualizar,
            this.toolStripLabelTitulo,
            this.toolStripButtonSalir});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.toolStrip.Size = new System.Drawing.Size(1117, 48);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.Renderer = new Acceso.DarkToolStripRenderer();
            // 
            // toolStripButtonEliminar
            // 
            this.toolStripButtonEliminar.AutoSize = false;
            this.toolStripButtonEliminar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonEliminar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(240)))));
            this.toolStripButtonEliminar.Image = global::Acceso.Properties.Resources.Eliminar_030x030_Color;
            this.toolStripButtonEliminar.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripButtonEliminar.Name = "toolStripButtonEliminar";
            this.toolStripButtonEliminar.Size = new System.Drawing.Size(110, 40);
            this.toolStripButtonEliminar.Text = "Eliminar";
            this.toolStripButtonEliminar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonEliminar.Click += new System.EventHandler(this.toolStripButtonEliminar_Click);
            // 
            // toolStripButtonNuevo
            // 
            this.toolStripButtonNuevo.AutoSize = false;
            this.toolStripButtonNuevo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonNuevo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(240)))));
            this.toolStripButtonNuevo.Image = global::Acceso.Properties.Resources.Nuevo_032x032_Color;
            this.toolStripButtonNuevo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNuevo.Name = "toolStripButtonNuevo";
            this.toolStripButtonNuevo.Size = new System.Drawing.Size(110, 40);
            this.toolStripButtonNuevo.Text = "Nuevo";
            this.toolStripButtonNuevo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonNuevo.Click += new System.EventHandler(this.toolStripButtonEditar_Click);
            // 
            // toolStripButtonGuardar
            // 
            this.toolStripButtonGuardar.AutoSize = false;
            this.toolStripButtonGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonGuardar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.toolStripButtonGuardar.Image = global::Acceso.Properties.Resources.Guardar_030x030_Color;
            this.toolStripButtonGuardar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGuardar.Name = "toolStripButtonGuardar";
            this.toolStripButtonGuardar.Size = new System.Drawing.Size(110, 40);
            this.toolStripButtonGuardar.Text = "Guardar";
            this.toolStripButtonGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonGuardar.Click += new System.EventHandler(this.toolStripButtonGuardar_Click);
            // 
            // toolStripButtonActualizar
            // 
            this.toolStripButtonActualizar.AutoSize = false;
            this.toolStripButtonActualizar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonActualizar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(240)))));
            this.toolStripButtonActualizar.Image = global::Acceso.Properties.Resources.Refrescar_025x032_Color;
            this.toolStripButtonActualizar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonActualizar.Name = "toolStripButtonActualizar";
            this.toolStripButtonActualizar.Size = new System.Drawing.Size(110, 40);
            this.toolStripButtonActualizar.Text = "Actualizar";
            this.toolStripButtonActualizar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonActualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonActualizar.Click += new System.EventHandler(this.toolStripButtonActualizar_Click);
            // 
            // toolStripLabelTitulo
            // 
            this.toolStripLabelTitulo.AutoSize = false;
            this.toolStripLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(168)))), ((int)(((byte)(76)))));
            this.toolStripLabelTitulo.Name = "toolStripLabelTitulo";
            this.toolStripLabelTitulo.Size = new System.Drawing.Size(500, 40);
            this.toolStripLabelTitulo.Text = "...";
            this.toolStripLabelTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripButtonSalir
            // 
            this.toolStripButtonSalir.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSalir.AutoSize = false;
            this.toolStripButtonSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(240)))));
            this.toolStripButtonSalir.Image = global::Acceso.Properties.Resources.Salir_030x030_Verde1;
            this.toolStripButtonSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSalir.Name = "toolStripButtonSalir";
            this.toolStripButtonSalir.Size = new System.Drawing.Size(110, 40);
            this.toolStripButtonSalir.Text = "Salir";
            this.toolStripButtonSalir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonSalir.Click += new System.EventHandler(this.toolStripButtonSalir_Click);
            // 
            // FrmBaseFormulario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1117, 701);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmBaseFormulario";
            this.Text = "FrmBaseFormulario";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        public System.Windows.Forms.ToolStripButton toolStripButtonEliminar;
        public System.Windows.Forms.ToolStripButton toolStripButtonNuevo;
        private System.Windows.Forms.ToolStripButton toolStripButtonGuardar;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTitulo;
        public System.Windows.Forms.ToolStripButton toolStripButtonSalir;
        private System.Windows.Forms.ToolStripButton toolStripButtonActualizar;
    }
}