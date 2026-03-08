namespace Global.Controles.Listas
{
	partial class ListViewPositivoNegativo
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Positivo",
            "Positivo",
            "Negativo"}, 0);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Negativo", 1);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListViewPositivoNegativo));
			this.listView = new Global.Controles.ListView();
			this.Signo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
			this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Signo});
			this.listView.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listView.FullRowSelect = true;
			this.listView.HideSelection = false;
			listViewItem1.StateImageIndex = 0;
			listViewItem2.StateImageIndex = 0;
			this.listView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
			this.listView.LargeImageList = this.imageList;
			this.listView.Location = new System.Drawing.Point(3, 3);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.ShowGroups = false;
			this.listView.ShowItemToolTips = true;
			this.listView.Size = new System.Drawing.Size(159, 79);
			this.listView.SmallImageList = this.imageList;
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.SmallIcon;
			// 
			// Signo
			// 
			this.Signo.Text = "Signo";
			this.Signo.Width = 206;
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "Positivo-60x60-Color.png");
			this.imageList.Images.SetKeyName(1, "Negativo-60x60-Color.png");
			// 
			// ListViewPositivoNegativo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.listView);
			this.Name = "ListViewPositivoNegativo";
			this.Size = new System.Drawing.Size(164, 85);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ColumnHeader Signo;
		private System.Windows.Forms.ImageList imageList;
		public ListView listView;
	}
}
