namespace Acceso.ControlEntidad
{
	partial class FrmAyudaEntidadSimple
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAyudaEntidadSimple));
			this.advancedDataGridView = new ADGV.AdvancedDataGridView();
			this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblTotal = new Global.Controles.EtiquetaSimple();
			this.botonAceptar = new Global.Controles.BotonBase();
			((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// advancedDataGridView
			// 
			this.advancedDataGridView.AllowUserToAddRows = false;
			this.advancedDataGridView.AllowUserToDeleteRows = false;
			this.advancedDataGridView.AllowUserToOrderColumns = true;
			this.advancedDataGridView.AutoGenerateColumns = false;
			this.advancedDataGridView.AutoGenerateContextFilters = true;
			this.advancedDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.advancedDataGridView.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.advancedDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.advancedDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
			this.advancedDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.HotTrack;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.advancedDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.advancedDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.advancedDataGridView.DataSource = this.bindingSource;
			this.advancedDataGridView.DateWithTime = false;
			this.advancedDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
			this.advancedDataGridView.EnableHeadersVisualStyles = false;
			this.advancedDataGridView.Location = new System.Drawing.Point(0, 0);
			this.advancedDataGridView.MultiSelect = false;
			this.advancedDataGridView.Name = "advancedDataGridView";
			this.advancedDataGridView.ReadOnly = true;
			this.advancedDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.advancedDataGridView.RowHeadersVisible = false;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
			this.advancedDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle2;
			this.advancedDataGridView.RowTemplate.Height = 24;
			this.advancedDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.advancedDataGridView.Size = new System.Drawing.Size(643, 329);
			this.advancedDataGridView.TabIndex = 0;
			this.advancedDataGridView.TimeFilter = false;
			this.advancedDataGridView.SortStringChanged += new System.EventHandler(this.advancedDataGridView_SortStringChanged);
			this.advancedDataGridView.FilterStringChanged += new System.EventHandler(this.advancedDataGridView_FilterStringChanged);
			this.advancedDataGridView.DoubleClick += new System.EventHandler(this.advancedDataGridView_DoubleClick);
			// 
			// bindingSource
			// 
			this.bindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.bindingSource_ListChanged);
			// 
			// lblTotal
			// 
			this.lblTotal.AutoSize = true;
			this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTotal.Location = new System.Drawing.Point(12, 345);
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.Size = new System.Drawing.Size(18, 20);
			this.lblTotal.TabIndex = 1;
			this.lblTotal.Text = "0";
			// 
			// botonAceptar
			// 
			this.botonAceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.botonAceptar.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.botonAceptar.Location = new System.Drawing.Point(511, 335);
			this.botonAceptar.Name = "botonAceptar";
			this.botonAceptar.Size = new System.Drawing.Size(120, 40);
			this.botonAceptar.TabIndex = 2;
			this.botonAceptar.Text = "Aceptar";
			this.botonAceptar.UseVisualStyleBackColor = true;
			this.botonAceptar.Click += new System.EventHandler(this.botonAceptar_Click);
			// 
			// FrmAyudaEntidadSimple
			// 
			this.AcceptButton = this.botonAceptar;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(643, 386);
			this.Controls.Add(this.botonAceptar);
			this.Controls.Add(this.lblTotal);
			this.Controls.Add(this.advancedDataGridView);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(661, 424);
			this.Name = "FrmAyudaEntidadSimple";
			this.Opacity = 0.95D;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Ayuda:";
			this.Load += new System.EventHandler(this.FrmAyudaEntidadSimple_Load);
			((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private Global.Controles.EtiquetaSimple lblTotal;
		public System.Windows.Forms.BindingSource bindingSource;
		private Global.Controles.BotonBase botonAceptar;
		public ADGV.AdvancedDataGridView advancedDataGridView;
	}
}