using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace Global.Datos
{
    public static class SQLAccess
    {
        // Connection string se lee de ConnectionStrings["Local"] en el .exe.config

        private static string SQLServidor { get; set; }
        private static string SQLDataBase { get; set; }
        private static string SQLUsuario { get; set; }
        private static string SQLPassword { get; set; }
        private static string SQLString { get; set; }
 
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

                connection.Open();
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
        }
        

        /// <summary>
        /// Arma la cdena de Conexion
        /// </summary>
        private static void ActualizarProp()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Local"];
            if (cs == null || string.IsNullOrEmpty(cs.ConnectionString))
            {
                throw new InvalidOperationException(
                    "No se encontro la cadena de conexion 'Local' en el archivo de configuracion.");
            }
            SQLString = cs.ConnectionString;
            SQLDataBase = new System.Data.SqlClient.SqlConnectionStringBuilder(SQLString).InitialCatalog;
        }

        private static void ActualizarProp(string cadena)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings[cadena];
            if (cs == null || string.IsNullOrEmpty(cs.ConnectionString))
            {
                throw new InvalidOperationException(
                    "No se encontro la cadena de conexion '" + cadena + "' en el archivo de configuracion.");
            }
            SQLString = cs.ConnectionString;
            SQLDataBase = new System.Data.SqlClient.SqlConnectionStringBuilder(SQLString).InitialCatalog;
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
