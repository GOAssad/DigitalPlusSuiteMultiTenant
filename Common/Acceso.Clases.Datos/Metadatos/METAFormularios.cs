using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acceso.Clases.Datos.Metadatos;

namespace Acceso.Clases.Datos.Metadatos
{
    public static class METAFormularios
    {
        
        public static List<METAFormulariosList> metaFormularios = new List<METAFormulariosList>();

        
        public static void ObtenerListaFormularios()
        {
            DataTable dt;
            try
            {
                string codigo;
                codigo = "Select sFormularioID, sDescripcion, nNivel from METAFormularios";
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(codigo);

                foreach (DataRow item in dt.Rows)
                {
                    metaFormularios.Add(new METAFormulariosList()
                    {
                        sFormularioID = item[0].ToString(),
                        sDescripcion = item[1].ToString(),
                        nNivel = (int)item[2]
                    });
                }

            }
            catch (Exception)
            {

                return;
            }
        }
    }
}
