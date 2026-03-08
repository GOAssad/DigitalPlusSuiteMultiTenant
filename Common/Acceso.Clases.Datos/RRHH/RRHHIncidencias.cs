using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHIncidencias
    {
        protected string _sIncidenciaID;
        protected string _sDescripcion;
        protected int _nForeColor;
        protected int _nBackColor;

        public string sMensaje;
        public bool existe;

        private readonly SqlParameter[] par = new SqlParameter[4];
        private DataTable dt;

        public string sIncidenciaID
        {
            set => _sIncidenciaID = value;
            get => _sIncidenciaID;
        }

        public string sDescripcion
        {
            set => _sDescripcion = value;
            get => _sDescripcion;
        }

        public int nForeColor
        {
            get => _nForeColor;
            set => _nForeColor = value;
        }

        public int nBackColor
        {
            get => _nBackColor;
            set => _nBackColor = value;
        }

        public void Inicializar()
        {
            // Multi-tenant: tabla Incidencia, columnas Id, Nombre, Color, EmpresaId
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena = "SELECT * FROM Incidencia WHERE Id = " + _sIncidenciaID + " AND EmpresaId = " + empresaId;
            sMensaje = string.Empty;
            existe = false;
            sDescripcion = string.Empty;

            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                sDescripcion = dt.Rows[0]["Nombre"].ToString();

                // Multi-tenant tiene Color como string hex (#RRGGBB)
                // Convertir a int para compatibilidad con el código existente
                string colorStr = dt.Rows[0]["Color"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(colorStr))
                {
                    // Usar el mismo color para ForeColor y BackColor
                    try
                    {
                        colorStr = colorStr.TrimStart('#');
                        int colorInt = int.Parse(colorStr, System.Globalization.NumberStyles.HexNumber);
                        nBackColor = colorInt;
                        // ForeColor: calcular contraste (blanco o negro)
                        int r = (colorInt >> 16) & 0xFF;
                        int g = (colorInt >> 8) & 0xFF;
                        int b = colorInt & 0xFF;
                        nForeColor = (r * 299 + g * 587 + b * 114) / 1000 > 128 ? 0 : 0xFFFFFF;
                    }
                    catch
                    {
                        nForeColor = 0;
                        nBackColor = 0xFFFFFF;
                    }
                }
                else
                {
                    nForeColor = 0;
                    nBackColor = 0xFFFFFF;
                }

                existe = true;
            }
            catch (Exception e)
            {
                sMensaje = e.Message;
            }
        }
        public bool Actualizar()
        {
            string cadena = "Incidencia_SP";
            try
            {
                llenarParametros();
                Global.Datos.SQLServer.EjecutarSPInsertUpdate(cadena, par);
                return true;
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                return false;
            }
        }

        public bool Eliminar()
        {
            string cadena = "Incidencia_Delete";
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter
            {
                ParameterName = "@Id",
                Value = _sIncidenciaID,
                SqlDbType = SqlDbType.Int
            };
            par[1] = new SqlParameter
            {
                ParameterName = "@EmpresaId",
                Value = Global.Datos.TenantContext.EmpresaId,
                SqlDbType = SqlDbType.Int
            };

            try
            {
                Global.Datos.SQLServer.EjecutarSPInsertUpdate(cadena, par);
                return true;
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                return false;
            }
        }

        private void llenarParametros()
        {
            par[0] = new SqlParameter
            {
                ParameterName = "@Id",
                Value = _sIncidenciaID,
                SqlDbType = SqlDbType.Int
            };

            par[1] = new SqlParameter
            {
                ParameterName = "@Nombre",
                Value = _sDescripcion,
                SqlDbType = SqlDbType.NVarChar
            };

            // Convertir BackColor int a hex string para la BD
            par[2] = new SqlParameter
            {
                ParameterName = "@Color",
                Value = "#" + _nBackColor.ToString("X6"),
                SqlDbType = SqlDbType.NVarChar
            };

            par[3] = new SqlParameter
            {
                ParameterName = "@EmpresaId",
                Value = Global.Datos.TenantContext.EmpresaId,
                SqlDbType = SqlDbType.Int
            };
        }

    }
}
