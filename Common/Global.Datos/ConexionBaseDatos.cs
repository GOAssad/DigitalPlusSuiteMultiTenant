using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
//using log4net;
//using log4net.Config;

namespace Global.Datos
{

/// <summary>
/// Descripci�n breve de ConexionBaseDatos.
/// </summary>
public class ConexionBaseDatos
{
	private OleDbConnection connection = null;
	private OleDbDataAdapter adapter = null;
    //private ILog log = LogManager.GetLogger("Log");  

    private SqlConnection conexion_destino = null;

	public ConexionBaseDatos(string provider, string servidor, string basedatos, string usuario, string clave)
	{
        //XmlConfigurator.Configure();
		connection = new OleDbConnection("Provider=" + provider + "; Server=" + servidor + "; Database=" + basedatos + "; UID=" + usuario + "; PWD=" + clave);
		connection.Open();

		adapter = new OleDbDataAdapter();
	}

    /// <summary>ConexionBaseDatos: Utiliza SqlConnection</summary>
    /// <param name="ConexionString">Conecta mediante SqlConnection</param>
    public ConexionBaseDatos(string ConexionString)
    {
        // Connection string se recibe por parametro
        conexion_destino = new SqlConnection();
        conexion_destino.ConnectionString = ConexionString;
    }


        public bool BlulkCopy(DataTable oDT, string TablaDestino)
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

    public DataSet Seleccionar(string comando)
	{
        adapter.SelectCommand = new OleDbCommand(comando, connection);        

		DataSet ds = new DataSet();		
		adapter.Fill(ds, "Tabla");

		return ds;
	}

	public void Ejecutar(string comando)
	{	
		int lTemp;

			adapter.SelectCommand = new OleDbCommand(comando, connection);
            //log.Info(comando);
			lTemp= adapter.SelectCommand.ExecuteNonQuery();

	}

    //public DataSet EjecutarSPConNulos(string sp, DataTable parametros)
    //{

    //    //adapter.SelectCommand.Parameters = 
    //    adapter.SelectCommand = new OleDbCommand(sp, connection);
    //    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

    //    for (int i = 0; i < parametros.Rows.Count; i++)
    //        adapter.SelectCommand.Parameters.Add(parametros.Rows[i]["name"].ToString(), 1);

    //    adapter.SelectCommand.ExecuteReader();

    //    DataSet ds = new DataSet();

    //    connection.Close();

    //    adapter.Fill(ds);

    //    connection.Open();

    //    return (ds);
    //}


    public DataSet EjecutarSPDatos(string sp, DataTable parametros)
    {
        int j = 0;
        string lStringParametros, lValor;
        lStringParametros = "";
        adapter.SelectCommand = new OleDbCommand(sp, connection);
        connection.ResetState();
        while (connection.State != ConnectionState.Open && j<10)
        {
            //log.Error("Error de conexion... Reconectando "+j.ToString());
            connection.Open();
            connection.ResetState();
            j++;
        }
        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

        for (int i = 0; i < parametros.Rows.Count; i++)
        {
            lValor = parametros.Rows[i]["valor"].ToString();
            lStringParametros = lStringParametros + lValor;
            if (i<parametros.Rows.Count-1)
            {lStringParametros = lStringParametros+" , ";}
        }
        try
        {
            //log.Error("Exec " + sp.Trim() + " " + lStringParametros.Trim());
            return this.Seleccionar("Exec " + sp.Trim() + " " + lStringParametros.Trim());
        }
        catch (Exception ex)
        {
            //log.Error("Error Sql- " + ex.Message);
            throw ex;
        }

    }

    public void EjecutarSPSinDatos(string sp, DataTable parametros)
    {
        int j = 0;
        string lStringParametros, lValor;
        lStringParametros = "";
        adapter.SelectCommand = new OleDbCommand(sp, connection);
        while (connection.State != ConnectionState.Open && j < 10)
        {
            //log.Error("Error de conexion... Reconectando " + j.ToString());
            connection.Open();
            connection.ResetState();
            j++;
        }
        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

        for (int i = 0; i < parametros.Rows.Count; i++)
        {
            lValor = parametros.Rows[i]["valor"].ToString();
            lStringParametros = lStringParametros + lValor ;
            if (i < parametros.Rows.Count - 1)
            { lStringParametros = lStringParametros + " , "; }
        }
        try
        {
            //log.Error("Exec " + sp.Trim() + " " + lStringParametros.Trim());
            this.Ejecutar("Exec " + sp.Trim() + " " + lStringParametros.Trim());
            return;
        }
        catch (Exception ex)
        {
            //log.Error("Error Sql- " + ex.Message);
            throw ex;
        }
    }

    public int EjecutarSPArchivo(string sp, string pNombre, string pExtension, string pInfo, Byte[] pArchivo)
    {
        try
        {
            adapter.SelectCommand = new OleDbCommand(sp, connection);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            OleDbParameter lImagen = new OleDbParameter("@piContenido", SqlDbType.Image);
            OleDbParameter lNombre = new OleDbParameter("@psNombre", SqlDbType.Text);
            OleDbParameter lExtension = new OleDbParameter("@psExtension", SqlDbType.Text);
            OleDbParameter lInfo = new OleDbParameter("@psInfo", SqlDbType.Text);
            lImagen.Value = pArchivo;
            lNombre.Value = pNombre;
            lExtension.Value = pExtension;
            lInfo.Value = pInfo;

            adapter.SelectCommand.Parameters.Add(lNombre);
            adapter.SelectCommand.Parameters.Add(lExtension);
            adapter.SelectCommand.Parameters.Add(lImagen);
            adapter.SelectCommand.Parameters.Add(lInfo);

            DataSet ds = new DataSet();
            adapter.Fill(ds, "Tabla");

            return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
}

}
