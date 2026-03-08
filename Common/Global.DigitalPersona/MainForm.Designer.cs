namespace Altiora.DigitalPersona
{
	partial class MainForm
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
			System.Windows.Forms.Label Bevel;
			System.Windows.Forms.Label label1;
			this.EnrollButton = new System.Windows.Forms.Button();
			this.VerifyButton = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.LoadButton = new System.Windows.Forms.Button();
			this.CloseButton = new System.Windows.Forms.Button();
			this.LoadButtonDB = new System.Windows.Forms.Button();
			this.VerifyButtonDB = new System.Windows.Forms.Button();
			this.controlEntidadLegajo = new Altiora.Formularios.ControlEntidad.ControlEntidadSimple();
			Bevel = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// botonMenuSalir
			// 
			this.botonMenuSalir.Location = new System.Drawing.Point(962, 4);
			// 
			// botonImprimir
			// 
			this.botonImprimir.Location = new System.Drawing.Point(821, 4);
			// 
			// panelMenuIndividual
			// 
			this.panelMenuIndividual.Size = new System.Drawing.Size(1100, 48);
			// 
			// Bevel
			// 
			Bevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			Bevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			Bevel.Location = new System.Drawing.Point(272, 251);
			Bevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			Bevel.Name = "Bevel";
			Bevel.Size = new System.Drawing.Size(497, 4);
			Bevel.TabIndex = 2;
			// 
			// label1
			// 
			label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			label1.Location = new System.Drawing.Point(272, 409);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(497, 4);
			label1.TabIndex = 5;
			// 
			// EnrollButton
			// 
			this.EnrollButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.EnrollButton.Location = new System.Drawing.Point(272, 98);
			this.EnrollButton.Margin = new System.Windows.Forms.Padding(4);
			this.EnrollButton.Name = "EnrollButton";
			this.EnrollButton.Size = new System.Drawing.Size(497, 37);
			this.EnrollButton.TabIndex = 0;
			this.EnrollButton.Text = "Registrar Huella";
			this.EnrollButton.UseVisualStyleBackColor = true;
			this.EnrollButton.Click += new System.EventHandler(this.EnrollButton_Click);
			// 
			// VerifyButton
			// 
			this.VerifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.VerifyButton.Enabled = false;
			this.VerifyButton.Location = new System.Drawing.Point(272, 142);
			this.VerifyButton.Margin = new System.Windows.Forms.Padding(4);
			this.VerifyButton.Name = "VerifyButton";
			this.VerifyButton.Size = new System.Drawing.Size(497, 37);
			this.VerifyButton.TabIndex = 1;
			this.VerifyButton.Text = "Verificar Huella";
			this.VerifyButton.UseVisualStyleBackColor = true;
			this.VerifyButton.Click += new System.EventHandler(this.VerifyButton_Click);
			// 
			// SaveButton
			// 
			this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SaveButton.Enabled = false;
			this.SaveButton.Location = new System.Drawing.Point(272, 271);
			this.SaveButton.Margin = new System.Windows.Forms.Padding(4);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(497, 37);
			this.SaveButton.TabIndex = 3;
			this.SaveButton.Text = "Guardar Plantilla de Huellas";
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// LoadButton
			// 
			this.LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LoadButton.Location = new System.Drawing.Point(272, 315);
			this.LoadButton.Margin = new System.Windows.Forms.Padding(4);
			this.LoadButton.Name = "LoadButton";
			this.LoadButton.Size = new System.Drawing.Size(497, 37);
			this.LoadButton.TabIndex = 4;
			this.LoadButton.Text = "Leer Plantilla de Huellas";
			this.LoadButton.UseVisualStyleBackColor = true;
			this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Location = new System.Drawing.Point(669, 423);
			this.CloseButton.Margin = new System.Windows.Forms.Padding(4);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(100, 28);
			this.CloseButton.TabIndex = 6;
			this.CloseButton.Text = "Close";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// LoadButtonDB
			// 
			this.LoadButtonDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LoadButtonDB.Location = new System.Drawing.Point(275, 360);
			this.LoadButtonDB.Margin = new System.Windows.Forms.Padding(4);
			this.LoadButtonDB.Name = "LoadButtonDB";
			this.LoadButtonDB.Size = new System.Drawing.Size(497, 37);
			this.LoadButtonDB.TabIndex = 4;
			this.LoadButtonDB.Text = "Leer de Base de Datos y Guardar en Archivo";
			this.LoadButtonDB.UseVisualStyleBackColor = true;
			this.LoadButtonDB.Click += new System.EventHandler(this.LoadButtonDB_Click);
			// 
			// VerifyButtonDB
			// 
			this.VerifyButtonDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.VerifyButtonDB.Enabled = false;
			this.VerifyButtonDB.Location = new System.Drawing.Point(272, 187);
			this.VerifyButtonDB.Margin = new System.Windows.Forms.Padding(4);
			this.VerifyButtonDB.Name = "VerifyButtonDB";
			this.VerifyButtonDB.Size = new System.Drawing.Size(497, 37);
			this.VerifyButtonDB.TabIndex = 1;
			this.VerifyButtonDB.Text = "Fingerprint Verification";
			this.VerifyButtonDB.UseVisualStyleBackColor = true;
			this.VerifyButtonDB.Click += new System.EventHandler(this.VerifyButtonDB_Click);
			// 
			// controlEntidadLegajo
			// 
			this.controlEntidadLegajo.ActualizaenFormulario = true;
			this.controlEntidadLegajo.BusquedaAvanzada = false;
			this.controlEntidadLegajo.ConTitulo = true;
			this.controlEntidadLegajo.DESCSQLFrom = "FullName";
			this.controlEntidadLegajo.GeneraNuevo = false;
			this.controlEntidadLegajo.IDSQLWhere = "Employid";
			this.controlEntidadLegajo.IDSQLWherePK = "Employid";
			this.controlEntidadLegajo.IDValor = 0;
			this.controlEntidadLegajo.Location = new System.Drawing.Point(275, 22);
			this.controlEntidadLegajo.MostrarID = false;
			this.controlEntidadLegajo.Name = "controlEntidadLegajo";
			this.controlEntidadLegajo.Size = new System.Drawing.Size(495, 60);
			this.controlEntidadLegajo.SqlAyuda = "Select EmployID, FullName from DYGPUPR00100";
			this.controlEntidadLegajo.TabIndex = 17;
			this.controlEntidadLegajo.TablaSQL = "DYGPUPR00100";
			this.controlEntidadLegajo.TextoEtiqueta = " Legajo";
			this.controlEntidadLegajo.TituloAyuda = "Legajos";
			this.controlEntidadLegajo.VariableGlobalMascara = null;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CloseButton;
			this.ClientSize = new System.Drawing.Size(1100, 645);
			this.Controls.Add(this.controlEntidadLegajo);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(label1);
			this.Controls.Add(this.LoadButtonDB);
			this.Controls.Add(this.LoadButton);
			this.Controls.Add(this.SaveButton);
			this.Controls.Add(Bevel);
			this.Controls.Add(this.VerifyButtonDB);
			this.Controls.Add(this.VerifyButton);
			this.Controls.Add(this.EnrollButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.OcultarPanelComandoso = true;
			this.OcultarPanelTitulo = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Fingerprint Enrollment and Verification Sample";
			this.Controls.SetChildIndex(this.EnrollButton, 0);
			this.Controls.SetChildIndex(this.VerifyButton, 0);
			this.Controls.SetChildIndex(this.VerifyButtonDB, 0);
			this.Controls.SetChildIndex(Bevel, 0);
			this.Controls.SetChildIndex(this.SaveButton, 0);
			this.Controls.SetChildIndex(this.LoadButton, 0);
			this.Controls.SetChildIndex(this.LoadButtonDB, 0);
			this.Controls.SetChildIndex(label1, 0);
			this.Controls.SetChildIndex(this.CloseButton, 0);
			this.Controls.SetChildIndex(this.controlEntidadLegajo, 0);
			this.Controls.SetChildIndex(this.panelMenuIndividual, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button EnrollButton;
		private System.Windows.Forms.Button VerifyButton;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Button LoadButton;
		private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button LoadButtonDB;
        private System.Windows.Forms.Button VerifyButtonDB;
		private Formularios.ControlEntidad.ControlEntidadSimple controlEntidadLegajo;
	}
}

