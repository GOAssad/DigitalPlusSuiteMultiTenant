using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.Funciones;

namespace Global.Datos
{
    public static class SQLServer
    {
		// Connection string se lee de ConnectionStrings["Local"] en el .exe.config

		private static string SQLServidor { get; set; }
		private static string SQLDataBase { get; set; }
		private static string SQLUsuario { get; set; }
		private static string SQLPassword { get; set; }

		private static string SQLString { get; set; }

		/// <summary>
		/// Pasar un Query como parametro y te devuelve un DataTable con los datos
		/// </summary>
		/// <param name="queryString">Instruccion SQL completa</param>
		/// <returns></returns>
		public static DataTable Ejecutar(string queryString)
        {
			//Traigo del configurador los datos por default
			ActualizarProp();

					
            using (SqlConnection connection = new SqlConnection(
					   SQLString))
            {
				DataTable dt = new DataTable();

				try
				{
					SqlCommand command = new SqlCommand(queryString, connection);

					command.Connection.Open();
					dt.Load(command.ExecuteReader());
					return dt;
				}
				catch (Exception ex)
				{

					System.Windows.Forms.MessageBox.Show(ex.Message);
					return dt;
				}
                
            }
        }


        /// <summary>
        /// Para subir info a un servidor de forma masiva
        /// </summary>
        /// <param name="oDT">DataTable a transferir</param>
        /// <param name="TablaDestino">Tabla destino que toma los datos de oDT</param>
        /// <param name="cadenadestino">Nombre de la cadena de conexion que esta en el config</param>
        /// <returns></returns>
        public static bool BlulkCopy(DataTable oDT, string TablaDestino, string cadenadestino)
        {
            //Traigo del configurador los datos por default
            ActualizarProp(cadenadestino);

            using (SqlConnection conexion_destino = new SqlConnection(
                       SQLString))
            {

                try
                {
                    // Verificar estructura de datos y crear la tabla en la base de datos
                    SqlBulkCopy importar = default(SqlBulkCopy);
                    importar = new SqlBulkCopy(conexion_destino);
                    importar.DestinationTableName = TablaDestino;
                    importar.WriteToServer(oDT);
                }
                catch (Exception)
                {

                    return false;
                }

                return true;
            }            
        }

        /// <summary>
        /// Pasar un Query como parametro y te devuelve un DataTable con los datos
        /// </summary>
        /// <param name="queryString">Instruccion SQL completa</param>
        /// /// <param name="cadena">Cadena de conexion que est en APP.config</param>
        /// <returns></returns>
        public static DataTable Ejecutar(string queryString, string cadena)
		{
			//Traigo del configurador los datos por default
			ActualizarProp(cadena);


			using (SqlConnection connection = new SqlConnection(
					   SQLString))
			{
				DataTable dt = new DataTable();

				try
				{
					SqlCommand command = new SqlCommand(queryString, connection);

					command.Connection.Open();
					dt.Load(command.ExecuteReader());
					return dt;
				}
				catch (Exception ex)
				{

					System.Windows.Forms.MessageBox.Show(ex.Message);
					return dt;
				}

			}
		}
		/// <summary>
		/// Devuelve un DataTable llenado por un DataAdapter
		/// </summary>
		/// <param name="queryString">Query que se quiere ejecutar para devolver los datos</param>
		/// <returns></returns>
		public static DataTable EjecutarParaSoloLectura(string queryString)
		{
			//Traigo del configurador los datos por default
			ActualizarProp();


			using (SqlConnection connection = new SqlConnection(
					   SQLString))
			{
				DataTable dt = new DataTable();
				try
				{
					connection.Open();
					SqlCommand command = new SqlCommand(queryString, connection);
					SqlDataAdapter sd = new SqlDataAdapter(command);
					sd.Fill(dt);

					return dt;
				}
				catch (Exception ex)
				{

					System.Windows.Forms.MessageBox.Show(ex.Message);
					return dt;
				}

			}
		}

       
        public static DataTable EjecutarParaSoloLectura(string queryString, string cadena)
		{
			//Traigo del configurador los datos por default
			ActualizarProp(cadena);


			using (SqlConnection connection = new SqlConnection(
					   SQLString))
			{
				DataTable dt = new DataTable();
				try
				{
					connection.Open();
					SqlCommand command = new SqlCommand(queryString, connection);
					SqlDataAdapter sd = new SqlDataAdapter(command);
					sd.Fill(dt);

					return dt;
				}
				catch (Exception ex)
				{

					System.Windows.Forms.MessageBox.Show(ex.Message);
					return dt;
				}

			}
		}

        public static DataTable EjecutarParaSoloLecturaSiempreTrue(string queryString)
        {
            //Traigo del configurador los datos por default
            ActualizarProp();


            using (SqlConnection connection = new SqlConnection(
                       SQLString))
            {
                DataTable dt = new DataTable();
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataAdapter sd = new SqlDataAdapter(command);
                    sd.Fill(dt);

                    return dt;
                }
                catch (Exception ex)
                {

                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    return dt;
                }

            }
        }
        //public static bool ChequearConexion(string server, string db, string usuario, string clave)
        public static bool ChequearConexion()
		{
			ActualizarProp();


			string queryString = "Select name from master.dbo.sysdatabases where name = '" + SQLDataBase + "' ";
			using (SqlConnection connection = new SqlConnection(
					   SQLString))
			{
				try
				{
					SqlCommand command = new SqlCommand(queryString, connection);

					DataTable dt = new DataTable();

					command.Connection.Open();
					dt.Load(command.ExecuteReader());
					if (dt.Rows.Count > 0) { return true; }
					else { return false; }
				}
				catch (Exception)
				{

					return false;
				}
			}
		}
        //public static bool ChequearConexion(string server, string db, string usuario, string clave)
        public static bool ChequearConexion(string cadena)
        {
            ActualizarProp(cadena);


            string queryString = "Select name from master.dbo.sysdatabases where name = '" + SQLDataBase + "' ";
            using (SqlConnection connection = new SqlConnection(
                       SQLString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);

                    DataTable dt = new DataTable();

                    command.Connection.Open();
                    dt.Load(command.ExecuteReader());
                    if (dt.Rows.Count > 0) { return true; }
                    else { return false; }
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
		/// <summary>
		/// Ejecuta un Store Procedure con una list de parametros, ideal para SP de Insert y Update
		/// <param name="queryString">Store Procedure</param>
		/// <param name="parametros">Matriz de Parametros de tipo SqlParameters</param>
		/// </summary>
		public static void EjecutarSPInsertUpdate(string queryString, SqlParameter[] parametros)
		{
			ActualizarProp();

			using (SqlConnection connection = new SqlConnection(
					   SQLString))
			{
				SqlCommand command = new SqlCommand(queryString, connection)
				{
					CommandText = queryString,
					CommandType = CommandType.StoredProcedure
				};

				for (int i = 0; i < parametros.Length; i++)
				{
					command.Parameters.Add(parametros[i]);
				}
                try
                {
					connection.Open();
					command.ExecuteNonQuery();
					command.Parameters.Clear();
				}
                catch (Exception )
                {

                    throw;
                }
				
			}
		}
        public static string EjecutarSPInsertUpdateSalida(string queryString, SqlParameter[] parametros)
        {
            ActualizarProp();
            int NroParamSalida;


            using (SqlConnection connection = new SqlConnection(
                       SQLString))
            {
                SqlCommand command = new SqlCommand(queryString, connection)
                {
                    CommandText = queryString,
                    CommandType = CommandType.StoredProcedure
                };

                for (int i = 0; i < parametros.Length; i++)
                {
                    command.Parameters.Add(parametros[i]);
                    if (parametros[i].Direction == ParameterDirection.Output)
                        NroParamSalida = i;
                     
                }

                connection.Open();
                command.ExecuteNonQuery();
                // Parametro de salida
                string sSalida = command.Parameters["@sAccion"].Value.ToString();
                command.Parameters.Clear();
                return sSalida;
            }
        }
        /// <summary>
        /// Ejecuta un Store Procedure con una list de parametros, ideal para SP de Insert y Update
        /// <param name="queryString">Store Procedure</param>
        /// <param name="parametros">Matriz de Parametros de tipo SqlParameters</param>
        /// <param name="cadena">Fuerza usar una cadena de conexion por si queres ejecutar el comando en otro lado</param>
        /// </summary>
        public static void EjecutarSPInsertUpdate(string queryString, SqlParameter[] parametros, string cadena)
		{
			ActualizarProp(cadena);

			using (SqlConnection connection = new SqlConnection(
					   SQLString))
			{
				SqlCommand command = new SqlCommand(queryString, connection)
				{
					CommandText = queryString,
					CommandType = CommandType.StoredProcedure
				};

				for (int i = 0; i < parametros.Length; i++)
				{
					command.Parameters.Add(parametros[i]);
				}
				connection.Open();
				command.ExecuteNonQuery();
				command.Parameters.Clear();
			}
		}


		/// <summary>
		/// Ejecuta un Store Procedure con una list de parametros, ideal para SP de Insert y Update devolviendo true o false Este metodo reemplaza el EjecutarSPInsertUpdate
		/// <param name="queryString">Store Procedure</param>
		/// <param name="parametros">Matriz de Parametros de tipo SqlParameters</param>
		/// </summary>
		public static bool EjecutarSPInsertUpdateBool(string queryString, SqlParameter[] parametros)
		{
			ActualizarProp();

			try
			{
				using (SqlConnection connection = new SqlConnection(
					   SQLString))
				{
					SqlCommand command = new SqlCommand(queryString, connection)
					{
						CommandText = queryString,
						CommandType = CommandType.StoredProcedure
					};

					for (int i = 0; i < parametros.Length; i++)
					{
						command.Parameters.Add(parametros[i]);
					}

					connection.Open();
					command.ExecuteNonQuery();
					command.Parameters.Clear();
				}
			}
			catch (Exception ex)
			{

				System.Windows.Forms.MessageBox.Show(ex.Message);
				return false;
			}
			

			return true;
			
		}
		/// <summary>
		/// Ejecutar una transaccion SQL sin devolucion de DataTable ni nada
		/// </summary>
		/// <param name="queryString">Comando SQL</param>
		/// <param name="commandtype">Tipo de Commando '0' = Query, '1' = Tabla Directa, '2' = Store Procedure</param>
		public static bool EjecutarSPsinRespuesta(string queryString, bool EsStore)
		{
			// este metodo tiene que ser transaccional

			//Traigo del configurador los datos por default
			ActualizarProp();

			try
			{
				using (SqlConnection connection = new SqlConnection(
					   SQLString))
				{
					SqlCommand command = new SqlCommand(queryString, connection);
					command.CommandText = queryString;
					if (EsStore) command.CommandType = CommandType.StoredProcedure;

					connection.Open();
					command.ExecuteNonQuery();

				}
			}
			catch (Exception ex)
			{

				System.Windows.Forms.MessageBox.Show(ex.Message);
				return false;
			}

			return true;
		}

       

        /// <summary>
        /// Arma la cadena de conexion leyendo de ConnectionStrings["Local"].
        /// Si no existe, lanza excepcion (NO hay fallback hardcodeado).
        /// </summary>
        private static void ActualizarProp()
		{
			var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Local"];
			if (cs == null || string.IsNullOrEmpty(cs.ConnectionString))
			{
				throw new InvalidOperationException(
					"No se encontro la cadena de conexion 'Local' en el archivo de configuracion. " +
					"Verifique que el archivo .exe.config existe y contiene una connectionString con name=\"Local\".");
			}
			SQLString = cs.ConnectionString;
			SQLDataBase = new SqlConnectionStringBuilder(SQLString).InitialCatalog;
		}

		/// <summary>
		/// Arma la cadena de conexion usando el nombre indicado.
		/// Si no existe, lanza excepcion (NO hay fallback hardcodeado).
		/// </summary>
		/// <param name="cadena">Nombre de la connection string en el .exe.config</param>
		private static void ActualizarProp(string cadena)
		{
			var cs = System.Configuration.ConfigurationManager.ConnectionStrings[cadena];
			if (cs == null || string.IsNullOrEmpty(cs.ConnectionString))
			{
				throw new InvalidOperationException(
					"No se encontro la cadena de conexion '" + cadena + "' en el archivo de configuracion.");
			}
			SQLString = cs.ConnectionString;
			SQLDataBase = new SqlConnectionStringBuilder(SQLString).InitialCatalog;
		}


		/// <summary>
		/// Ejecuta un SP con parametros, ideal para un Select con un Where e inicializar una clase
		/// <param name="queryString">Instruccion SQL</param>
		/// <param name="parametros">Matriz de Parametros de tipo SqlParameters</param>
		/// </summary>
		public static DataTable EjecutarSPSelect(string queryString, SqlParameter[] parametros, bool esSP = true)
		{
			ActualizarProp();

			DataTable dt = new DataTable();
			try
			{
				using (SqlConnection connection = new SqlConnection(
					   SQLString))
				{
					SqlCommand command = new SqlCommand(queryString, connection)
					{
                        
                    	CommandText = queryString,
						CommandType = esSP ? CommandType.StoredProcedure : CommandType.Text
					};

					for (int i = 0; i < parametros.Length; i++)
					{
						command.Parameters.Add(parametros[i]);
					}

					connection.Open();
					dt.Load(command.ExecuteReader());

					// 17/09/2018 Para que las columnas no sean read only
					foreach (DataColumn col in dt.Columns)
					{
						col.ReadOnly = false;
					}

					command.Parameters.Clear();
				}
			}
			catch (Exception ex)
			{

				System.Windows.Forms.MessageBox.Show(ex.Message);				
			}

			return dt;
		}

        /// <summary>
		/// Ejecuta un SP con parametros, ideal para un Select con un Where e inicializar una clase
		/// <param name="queryString">Instruccion SQL</param>
		/// <param name="parametros">Matriz de Parametros de tipo SqlParameters</param>
		/// </summary>
		public static DataTable EjecutarSPSelect(string queryString, SqlParameter[] parametros, string cadena)
        {
            ActualizarProp(cadena);

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(SQLString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection)
                    {
                        CommandText = queryString,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 0
                        
                    };

                    for (int i = 0; i < parametros.Length; i++)
                    {
                        command.Parameters.Add(parametros[i]);
                    }

                    connection.Open();
                    dt.Load(command.ExecuteReader());

                    // 17/09/2018 Para que las columnas no sean read only
                    foreach (DataColumn col in dt.Columns)
                    {
                        col.ReadOnly = false;
                    }

                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return dt;
        }
        
		public static DataTable EjecutarSPSelect(string queryString)
        {
			DataTable dt = new DataTable();
			try
			{
				using (SqlConnection connection = new SqlConnection(SQLString))
				{
					SqlCommand command = new SqlCommand(queryString, connection)
					{
						CommandText = queryString,
						CommandType = CommandType.StoredProcedure,
						CommandTimeout = 0

					};
					connection.Open();
					dt.Load(command.ExecuteReader());

					// 17/09/2018 Para que las columnas no sean read only
					foreach (DataColumn col in dt.Columns)
					{
						col.ReadOnly = false;
					}

				}
			}
			catch (Exception ex)
			{

				System.Windows.Forms.MessageBox.Show(ex.Message);
			}

			return dt;
		}
		public static SqlParameter LlenarTipoSql(int tipo)
		{
			SqlParameter p = new SqlParameter
			{
				ParameterName = "@TipoSQL",
				Value = tipo,
				SqlDbType = SqlDbType.SmallInt
			};

			return p;
		}

	}
}
