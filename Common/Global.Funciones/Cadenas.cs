using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Global.Funciones
{
    static public class Cadenas
	{
		/// <summary>
		/// Busca con metodo Normal usando foreach:
		/// Busca en la grid que se pasa como parameto un string en la columna indicada, 
		/// si la columna no se especifica busca en toda la grid
		/// </summary>
		/// <param name="TextoABuscar">texto que se pretende encontrar</param>
		/// <param name="Columna">Nombre de la columna donde se quiere buscar (empty = todas)</param>
		/// <param name="grid">Grilla en donde se quiere buscar</param>
		/// <returns>devuelve true o false</returns>
		static public bool BuscarenGridModoNormal(string TextoABuscar, string Columna, DataGridView grid)
		{
			bool encontrado = false;
			if (TextoABuscar == string.Empty) return false;
			if (grid.RowCount == 0) return false;
			grid.ClearSelection();
			if (Columna == string.Empty)
			{
				foreach (DataGridViewRow row in grid.Rows)
				{
					foreach (DataGridViewCell cell in row.Cells)
						if (cell.Value.ToString() == TextoABuscar)
						{
							row.Selected = true;
							return true;
						}
				}
			}
			else
			{
				foreach (DataGridViewRow row in grid.Rows)
				{

					if (row.Cells[Columna].Value.ToString() == TextoABuscar)
					{
						row.Selected = true;
						return true;
					}
				}
			}
			return encontrado;
		}
		/// <summary>
		/// Busca con metodo LINQ:
		/// Busca en la grid que se pasa como parameto un string en la columna indicada, 
		/// si la columna no se especifica busca en toda la grid 
		/// </summary>
		/// <param name="TextoABuscar">texto que se pretende encontrar</param>
		/// <param name="Columna">Nombre de la columna donde se quiere buscar (empty = todas)</param>
		/// <param name="grid">Grilla en donde se quiere buscar</param>
		/// <returns>devuelve true o false y se para sobre el registro si lo encuentra</returns>
		static public bool BuscarenGridModoLINQ(string TextoABuscar, string Columna, DataGridView grid)
		{
			bool encontrado = false;
			if (TextoABuscar == string.Empty) return false;
			if (grid.RowCount == 0) return false;
			grid.ClearSelection();
			if (Columna == string.Empty)
			{
				IEnumerable<DataGridViewRow> obj = (from DataGridViewRow row in grid.Rows.Cast<DataGridViewRow>()
													from DataGridViewCell cells in row.Cells
													where cells.OwningRow.Equals(row) && cells.Value.ToString() == TextoABuscar
													select row);


				if (obj.Any())
				{
					int i = obj.FirstOrDefault().Index;
					grid.Rows[i].Selected = true;
					
					return true;
				}

			}
			else
			{
				IEnumerable<DataGridViewRow> obj = (from DataGridViewRow row in grid.Rows.Cast<DataGridViewRow>()
													where row.Cells[Columna].Value.ToString() == TextoABuscar
													select row);
				if (obj.Any())
				{
					grid.Rows[obj.FirstOrDefault().Index].Selected = true;
					return true;
				}

			}
			return encontrado;

		}

		/// <summary>
		/// Busca con metodo LINQ:
		/// Busca en la grid que se pasa como parameto un string en la columna indicada, 
		/// si la columna no se especifica busca en toda la grid 
		/// </summary>
		/// <param name="TextoABuscar">texto que se pretende encontrar</param>
		/// <param name="Columna">Nombre de la columna donde se quiere buscar (empty = todas)</param>
		/// <param name="grid">Grilla en donde se quiere buscar</param>
		/// <returns>devuelve true o false y se para sobre el registro si lo encuentra</returns>
		static public bool BuscarenGridModoLINQLike(string TextoABuscar, string Columna, DataGridView grid)
		{
			bool encontrado = false;
			if (TextoABuscar == string.Empty) return false;
			if (grid.RowCount == 0) return false;
			grid.ClearSelection();
			if (Columna == string.Empty)
			{
				IEnumerable<DataGridViewRow> obj = (from DataGridViewRow row in grid.Rows.Cast<DataGridViewRow>()
													from DataGridViewCell cells in row.Cells
													where cells.OwningRow.Equals(row) && cells.Value.ToString().Contains(TextoABuscar)
													select row);


				if (obj.Any())
				{
					grid.Rows[obj.FirstOrDefault().Index].Selected = true;
					return true;
				}

			}
			else
			{
				IEnumerable<DataGridViewRow> obj = (from DataGridViewRow row in grid.Rows.Cast<DataGridViewRow>()
													where row.Cells[Columna].Value.ToString().Contains(TextoABuscar)
													select row);
				if (obj.Any())
				{
					grid.Rows[obj.FirstOrDefault().Index].Selected = true;
					return true;
				}

			}
			return encontrado;

		}

		public static string EncriptarString(this string _cadenaAencriptar)
		{
			string result = string.Empty;
			byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
			result = Convert.ToBase64String(encryted);
			return result;
		}
		public static string DesencriptarString(this string _cadenaAdesencriptar)
		{
			string result = string.Empty;
			byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
			//result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
			result = System.Text.Encoding.Unicode.GetString(decryted);
			return result;
		}
        
        /// <summary>
        /// Leer una linea determinada de un archivo de texto
        /// </summary>
        /// <param name="path">Camino completo sin barra invertida final</param>
        /// <param name="archivo">Nombre del archivo que se va a leer hubicado en path</param>
        /// <param name="linea">Numero de linea que se va a leer</param>
        /// <returns></returns>
        public static string LeerLineaArchivoTexto(string path, string archivo, int linea)
        {
            string line;
            string pathcompleto = path + "\\" + archivo;

            try
            {
                StreamReader sr = new StreamReader(pathcompleto);
                int numLinea = 1;

                line = sr.ReadLine();
                while (line != null)
                {
                    if (numLinea == linea)
                        return line;

                    line = sr.ReadLine();
                    numLinea++;
                }

            }
            catch (Exception)
            {
                return string.Empty;
            }
            

            return string.Empty;
        }

        public static System.Net.IPAddress ValidarIP(string directionIP)
        {
            System.Net.IPAddress ip = new System.Net.IPAddress(new byte[] { 0, 0, 0, 0 });
            System.Net.IPAddress.TryParse(directionIP, out ip);
            return ip;
        }
	}
}
