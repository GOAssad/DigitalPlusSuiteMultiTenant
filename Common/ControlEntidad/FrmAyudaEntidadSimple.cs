using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global.Controles;

namespace Acceso.ControlEntidad
{
	public partial class FrmAyudaEntidadSimple : Form
	{

		#region Propiedades
		private string _Titulo;
		/// <summary>
		/// Titulo que toma del formulario, la propiedad se llena desde el control que lo llama
		/// </summary>
		[DescriptionAttribute("Titulo que toma del formulario, la propiedad se llena desde el control que lo llama"), Category("Altiora")]
		public string Titulo
		{
			get { return _Titulo; }
			set
			{
				_Titulo = value;
				CambiarPropiedades();
			}
		}

		private string _IDElegido;

		public string IDElegido
		{
			get { return _IDElegido; }
			set { _IDElegido = value; }
		} 
		#endregion

		private void CambiarPropiedades()
		{
			Text = _Titulo;
		}

		public FrmAyudaEntidadSimple()
		{
			InitializeComponent();
			advancedDataGridView.AutoGenerateColumns = true;
			//Global.Funciones.Formatos.Grid(advancedDataGridView, this);
			//advancedDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			
			
		}
		private void advancedDataGridView_SortStringChanged(object sender, EventArgs e)
		{
			this.bindingSource.Sort = this.advancedDataGridView.SortString;
		}

		private void advancedDataGridView_FilterStringChanged(object sender, EventArgs e)
		{
			this.bindingSource.Filter = this.advancedDataGridView.FilterString;
		}

		private void bindingSource_ListChanged(object sender, ListChangedEventArgs e)
		{
			lblTotal.Text = string.Format("Total Registros: {0}", this.bindingSource.List.Count);
		}

		private void FrmAyudaEntidadSimple_Load(object sender, EventArgs e)
		{
			
		}

		private void botonAceptar_Click(object sender, EventArgs e)
		{
			_IDElegido = string.Empty;
			if (advancedDataGridView.Rows.Count == 0) return;
			if (advancedDataGridView.CurrentRow.Index >= 0)
				_IDElegido = advancedDataGridView.CurrentRow.Cells[0].Value.ToString();

		}

		private void advancedDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			botonAceptar_Click(sender, e);
		}

		private void advancedDataGridView_DoubleClick(object sender, EventArgs e)
		{
			if (advancedDataGridView.CurrentRow != null)
			{
				if (advancedDataGridView.CurrentRow.Index >= 0)
				{
					botonAceptar_Click(sender, e);
					DialogResult = DialogResult.OK;
				}
			}
		}
	}
}
