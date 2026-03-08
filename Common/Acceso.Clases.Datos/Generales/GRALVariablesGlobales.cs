using System;
using System.Collections.Generic;
using System.Data;

namespace Acceso.Clases.Datos.Generales
{

    public static class GRALVariablesGlobales
    {

        private static string _sUsuarioID;

        public static string sUsuarioID
        {
            get { return _sUsuarioID; }
            set { _sUsuarioID = value; }
        }

        private static DateTime _FechaInicio;


        public static DateTime FechaInicio
        {
            get { return _FechaInicio; }
            set { _FechaInicio = value; }
        }

        public static string TraerValorDataBase(string svariableglobalid)
        {
            // Multi-tenant: tabla VariableSistema, columnas Clave / Valor, filtro por EmpresaId
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            DataTable dt;
            string cadena = "SELECT Valor FROM VariableSistema WHERE Clave = '" + svariableglobalid + "' AND EmpresaId = " + empresaId;
            dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);
            if (dt.Rows.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return dt.Rows[0][0].ToString().Trim();
            }
        }


        public static string TraerValorArchivo(string path, string archivo, int linea)
        {
            return Global.Funciones.Cadenas.LeerLineaArchivoTexto(path, archivo, linea);
        }

        public static DataTable ListaVariables()
        {
            // Multi-tenant: tabla VariableSistema, filtro por EmpresaId
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena = "SELECT * FROM VariableSistema WHERE EmpresaId = " + empresaId;
            return Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);
        }

        public static bool GuardarVariable(string variable, string valor)
        {
            // Multi-tenant: tabla VariableSistema, columnas Clave / Valor
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string query = "UPDATE VariableSistema SET Valor = '" + valor + "' WHERE Clave = '" + variable + "' AND EmpresaId = " + empresaId;
            return Global.Datos.SQLServer.EjecutarSPsinRespuesta(query, false);
        }

        public static List<GRALVariablesGlobalesList> LVariablesGlobales = new List<GRALVariablesGlobalesList>();
        public static List<GRALVariablesGlobalesList> LLenarLista()
        {
            DataTable dt = new DataTable();
            dt = ListaVariables();
            foreach (DataRow row in dt.Rows)
            {
                // Multi-tenant columns: Clave, TipoValor, Valor
                LVariablesGlobales.Add(new GRALVariablesGlobalesList()
                {
                    sVariableGlobalID = row["Clave"]?.ToString() ?? string.Empty,
                    sDescripcion      = row["TipoValor"]?.ToString() ?? string.Empty,
                    sValor            = row["Valor"]?.ToString() ?? string.Empty,
                    bModificado       = false
                });
            }
            return LVariablesGlobales;
        }

    }
}
