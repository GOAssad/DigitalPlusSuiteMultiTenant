using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Global.Datos
{
	static public class Funciones
	{
		public static void autocompletar(TextBox cuadrotexto, string query)
		{
			DataTable dt = new DataTable();
			try
			{
				dt =SQLServer.Ejecutar(query);
				cuadrotexto.AutoCompleteCustomSource.Clear();
				foreach (DataRow dr in dt.Rows)
				{
					cuadrotexto.AutoCompleteCustomSource.Add(dr[0].ToString());
				}
			}
			catch (Exception ex)
			{

				MessageBox.Show("Error llenando el cuadro de Autocompletar " + "\r" + ex.Message);
			}
		}

    }
    public static class DataTableExt
    {

        /// <summary>
        /// Exportar DataTable a Excel 
        /// </summary>
        /// <param name="DataTable">Data table de Origen</param>
        /// <param name="ExcelFilePath">Donde se va a crear el Excel</param>
        public static void ExportToExcel(this System.Data.DataTable DataTable, string ExcelFilePath = null)
        {
            try
            {
                int ColumnsCount;

                if (DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
                    throw new Exception("Altiora: Tabla Vacia!\n");

                // load excel, and create a new workbook
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();

                // single worksheet
                Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.ActiveSheet;

                object[] Header = new object[ColumnsCount];

                // column headings               
                for (int i = 0; i < ColumnsCount; i++)
                    Header[i] = DataTable.Columns[i].ColumnName;

                Microsoft.Office.Interop.Excel.Range HeaderRange = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, ColumnsCount]));
                HeaderRange.Value = Header;
                HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                HeaderRange.Font.Bold = true;

                // DataCells
                int RowsCount = DataTable.Rows.Count;
                object[,] Cells = new object[RowsCount, ColumnsCount];

                for (int j = 0; j < RowsCount; j++)
                    for (int i = 0; i < ColumnsCount; i++)
                        Cells[j, i] = DataTable.Rows[j][i];

                Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[RowsCount + 1, ColumnsCount])).Value = Cells;

                // check fielpath
                if (ExcelFilePath != null && ExcelFilePath != "")
                {
                    try
                    {
                        Worksheet.SaveAs(ExcelFilePath);
                        Excel.Quit();
                        System.Windows.Forms.MessageBox.Show("Archivo Excel Almacenado");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Altiora: El Archivo Excel no pudo almacenrse! Verifique la ruta.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    Excel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
