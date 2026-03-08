using System.Data.SqlClient;

namespace Acceso.Clases.Datos

{
    public abstract class ConnectionSql
    {
        protected SqlConnection getConnection()
        {
            // Usa la connection string "Local" del .exe.config (misma que SQLServer.cs)
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Local"];
            if (cs != null && !string.IsNullOrEmpty(cs.ConnectionString))
            {
                return new SqlConnection(cs.ConnectionString);
            }

            // Fallback legacy (solo si no hay "Local" configurado)
            string SQLPassword = Global.Funciones.Configuracion.RecuperaValor("SQLClave", "Soporte1");
            string SQLDataBase = Global.Funciones.Configuracion.RecuperaValor("SQLBaseDatos", "DigitalPlusMultiTenant");
            string SQLServidor = Global.Funciones.Configuracion.RecuperaValor("SQLServidor", ".");
            string SQLUsuario = Global.Funciones.Configuracion.RecuperaValor("SQLUsuario", "sa");

            string SQLCadena = @"Data Source=" + SQLServidor + ";Initial Catalog=" +
            SQLDataBase + ";User Id=" + SQLUsuario + ";Password=" + SQLPassword + ";";

            return new SqlConnection(SQLCadena);
        }


    }
}
