using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Global.Funciones
{/// <summary>
/// Clase que contiene funciones para trabajar e interactuar con datis
/// </summary>
	public static class BasedeDatos
	{
		/// <summary>
		/// Metodo de extension que convierte cualquier objeto que implemente IEnumerable<T> interface (como List<T>, Stack<T>,
		/// LinkedList<T>, Queue<T>, Dictionary<TKey, Value> y Hashset<T>) a un DataTable. 
		/// </summary>
		/// <typeparam name="T">Clase del tipo del IEnumerable</typeparam>
		/// <param name="list">Nombre del IEnumerable</param>
		/// <returns>Devuelve un DataTable</returns>
		public static DataTable PasaraDataTable<T>(this IEnumerable<T> list)
		{

			DataTable dtOutput = new DataTable("tblOutput");
			//usa reflection para saber los tipos de datos de cada columna
			PropertyInfo[] properties = list.FirstOrDefault().GetType().
				GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo propertyInfo in properties)
				dtOutput.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);

			DataRow dr;
			foreach (T t in list)
			{
				dr = dtOutput.NewRow();
				//iterate through all the properties of the current object
				//and set their values to data row
				foreach (PropertyInfo propertyInfo in properties)
				{
					dr[propertyInfo.Name] = propertyInfo.GetValue(t, null);
				}
				dtOutput.Rows.Add(dr);
			}

			return dtOutput;
		}

        /// <summary>
        /// Devuelve una Lista desde el datatable que pasas como parametro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">Datatable que queres convertir en lista</param>
        /// <returns></returns>
        public static List<T> PasaraLista<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }


        /// <summary>
        /// Metodo estatico que le mandas un Sqlexcepcion de sql para que te devuelva un mensaje claro al usuario
        /// </summary>
        /// <typeparam name="T">Clase del tipo del IEnumerable</typeparam>
        /// <param name="list">Nombre del IEnumerable</param>
        /// <returns>Devuelve un DataTable</returns>
        public static string ErorrSql(SqlError error)
		{

			switch (error.Number)
			{
				case 109:
					return "Problemas con insert";
				case 110:
					return "Más problemas con insert";
				case 113:
					return "Problemas con comentarios";
				case 156:
					return "Error de sintaxis";

				default:
					return error.Message;
			}
		}

        /// <summary>
        /// Busca Instalaciones de SQL dentro de la Registry
        /// </summary>
        /// <returns>Devuelve una lista con los nombres de las instancias</returns>
        public static List<string> SQLInstancias()
        {
            List<string> s = new List<string>();
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        s.Add(Environment.MachineName + @"\" + instanceName);
                    }
                }

                instanceKey = hklm.OpenSubKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        s.Add(Environment.MachineName + @"\" + instanceName);
                    }
                }

            }
            return s;
        }

        public static DataTable SQLInstanciasEnum()
        {
            System.Data.Sql.SqlDataSourceEnumerator instance =
                System.Data.Sql.SqlDataSourceEnumerator.Instance;
            System.Data.DataTable dataTable = instance.GetDataSources();
            return dataTable;

            
        }
	}
}
