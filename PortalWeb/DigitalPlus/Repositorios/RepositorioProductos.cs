using Dapper;
using DigitalPlus.Data;
using DigitalPlus.DTOs;
using DigitalPlus.Entidades.DigitalOnePlusEscritorio;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace DigitalPlus.Repositorios
{
    public class RepositorioProductos
    {
        private readonly ApplicationDbContext context;
        private readonly RepositorioSectores repositorioSectores;
        private readonly RepositorioCategorias repositorioCategorias;
        private readonly string connectionString;
        private readonly string connectionStringAcceso;
        public string mensaje = string.Empty;



        public RepositorioProductos(ApplicationDbContext context, IConfiguration configuration,
                            RepositorioSectores repositorioSectores,
                            RepositorioCategorias repositorioCategorias)
        {
            this.context = context;
            this.repositorioSectores = repositorioSectores;
            this.repositorioCategorias = repositorioCategorias;
            connectionString = configuration.GetConnectionString("DefaultConnectionFichas");
            connectionStringAcceso = configuration.GetConnectionString("DefaultConnectionAcceso");
        }


        public async Task<List<ProveedorDTO>> TraerProveedores()
        {
            using var connection = new SqlConnection(connectionString);
            mensaje = string.Empty;

            try
            {
                IEnumerable<ProveedorDTO> Lista = await connection
                        .QueryAsync<ProveedorDTO>($@"
                            select distinct b.VENDORID, c.VENDNAME
                             from PRD01..iv00101 A 
                            inner Join PRD01..iv00103 B ON A.ITEMNMBR = b.ITEMNMBR 
                            inner join PRD01..PM00200 c on b.VENDORID = c.VENDORID
                             where a.ITMCLSCD = 'MP-TEL' order by 2 ");

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }
        public async Task<List<GrupoDTO>> TraerGrupos(string id)
        {
            using var connection = new SqlConnection(connectionString);
            mensaje = string.Empty;
            string innerProveedor = string.Empty;
            if (id != "0")
                innerProveedor = " inner join PRD01..IV00103 A on B.ITEMNMBR = A.ITEMNMBR AND a.VENDORID = " + id;

            try
            {
                string sQuery = $@"  
                         Select DISTINCT C.DSCRIPTN GRUPO, c.SGMCOD GrupoID FROM PRD01..iv00101 B 
                         INNER JOIN    PRD01..SOL_GCA00100 C ON LEFT(b.ITEMNMBR, 2) = C.SGMCOD AND 
                         c.ITMCLSCD = 'mp-tel' AND C.SGMTNUMB = 1 {innerProveedor}  
                         WHERE B.ITMCLSCD = 'mp-tel' ORDER BY 1";

                IEnumerable<GrupoDTO> Lista = await connection
                        .QueryAsync<GrupoDTO>(sQuery);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }
        public async Task<List<ColorDTO>> TraerColores(string idproveedor, string idgrupo)
        {
            using var connection = new SqlConnection(connectionString);
            mensaje = string.Empty;

            string innerGrupos = string.Empty;
            if (idgrupo != "0")
                innerGrupos = " and c.SGMCOD = '" + idgrupo.Trim() + "' ";

            string innerProveedor = string.Empty;
            if (idproveedor != "0")
                innerProveedor = " inner join PRD01..IV00103 A on B.ITEMNMBR = A.ITEMNMBR AND a.VENDORID = " + idproveedor.Trim();

            try
            {
                IEnumerable<ColorDTO> Lista = await connection
                        .QueryAsync<ColorDTO>($@"SELECT DISTINCT 
                     h.DSCRIPTN Color, h.SGMCOD ColorID 
                     FROM PRD01..iv00101 B  
                     INNER JOIN    PRD01..SOL_GCA00100 C ON LEFT(b.ITEMNMBR, 2) = C.SGMCOD AND  
                     c.ITMCLSCD = 'mp-tel' AND C.SGMTNUMB = 1  
                     INNER JOIN    PRD01..SOL_GCA00100 h ON SUBSTRING(b.ITEMNMBR, 13, 3) = h.SGMCOD AND 
                     h.ITMCLSCD = 'mp-tel' AND H.SGMTNUMB = 6 {innerProveedor} 
                      WHERE B.ITMCLSCD = 'mp-tel' {innerGrupos}  
                     ORDER BY 1, 2 ", new { innerProveedor, innerGrupos });

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }
        public async Task<List<ColorDTO>> TraerColores(string query)
        {
            using var connection = new SqlConnection(connectionString);
            try
            {
                IEnumerable<ColorDTO> Lista = await connection
                        .QueryAsync<ColorDTO>(query);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }
        public async Task<List<ProductoDTO>> TraerProductos(string sQuery)
        {
            using var connection = new SqlConnection(connectionString);


            try
            {
                IEnumerable<ProductoDTO> Lista = await connection
                        .QueryAsync<ProductoDTO>(sQuery);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }

        public async Task<List<ProductoDTO>> TraerProductos(string grupo, string color, string codigo, string descripcion = "", string proveedor = "", string sJoin = "left")
        {


            //alguno de estos campos tiene que estar lleno si o si
            if (codigo == string.Empty && descripcion == string.Empty && proveedor == string.Empty)
                return null;



            mensaje = string.Empty;

            string whereGrupo = string.Empty;
            if (grupo != "0")
                whereGrupo = $" and C.SGMCOD = '{grupo}' ";

            string whereColor = string.Empty;
            if (color != "0")
                whereColor = $" and H.SGMCOD = '{color}' ";

            string whereId = string.Empty;
            if (codigo != string.Empty)
                whereId = $" A.ITEMNMBR LIKE  '{codigo}%' ";

            string whereDesc = string.Empty;
            if (descripcion != string.Empty)
                if (whereId != string.Empty)
                    whereDesc = $"  and a.ITEMDESC like '%{descripcion}%' ";
                else
                    whereDesc = $"   a.ITEMDESC like '%{descripcion}%' ";

            string whereProv = string.Empty;
            if (proveedor != string.Empty)
                if (codigo != string.Empty || descripcion != string.Empty)
                    whereProv = $"  and c.VENDNAME like '%{proveedor}%' ";
                else
                    whereProv = $"  c.VENDNAME like '%{proveedor}%' ";

            using var connection = new SqlConnection(connectionString);
            //armo el query
            string sQuery = string.Empty;

            sQuery = $@"Select distinct a.itemnmbr, a.itemdesc, isnull(d.QTYONHND,0) Cantidad, c.VendName  
                    from PRD01..iv00101 a  left join PRD01..IV00102 d on a.itemnmbr = d.itemnmbr   
                    INNER  join PRD01..iv00103 b on a.ITEMNMBR = b.ITEMNMBR  
                    INNER  join PRD01..PM00200 c ON B.VENDORID = C.VENDORID  
                    WHERE  {whereId}  {whereDesc} {whereProv}
                    and d.locncode = 'DPT-CUY' ";


            try
            {
                IEnumerable<ProductoDTO> Lista = await connection
                        .QueryAsync<ProductoDTO>(sQuery);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }

        public async Task<List<DYGPIV00102DTO>> TraerStock(string codigo)
        {
            using var connection = new SqlConnection(connectionStringAcceso);
            string sQuery = $@"select * from DYGPIV00102 where Deposito = 'DPT-CUY' and item = '{codigo.Trim()}'";

            try
            {
                IEnumerable<DYGPIV00102DTO> Lista = await connection
                        .QueryAsync<DYGPIV00102DTO>(sQuery);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }

        public async Task<List<DYGPPOP10110_ULTIMAS>> TraerComprasUltimas(string codigo)
        {
            using var connection = new SqlConnection(connectionStringAcceso);
            string sQuery = $@"select * from DYGPPOP10110_ULTIMAS where 
                    Deposito = 'DPT-CUY' and codigo = '{codigo.Trim()}' order by FechaOC desc";

            try
            {
                IEnumerable<DYGPPOP10110_ULTIMAS> Lista = await connection
                        .QueryAsync<DYGPPOP10110_ULTIMAS>(sQuery);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }
        public async Task<List<DYGPPOP10110DTO>> TraerCompras(string codigo)
        {
            using var connection = new SqlConnection(connectionStringAcceso);
            string sQuery = $@"SELECT * FROM DYGPPOP10110 where 
                    codigo = '{codigo.Trim()}' order by FechaOC desc";

            try
            {
                IEnumerable<DYGPPOP10110DTO> Lista = await connection
                        .QueryAsync<DYGPPOP10110DTO>(sQuery);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }
        public async Task<List<DYGPINVEReservasArticulosDTO>> TraerHistorialReservas(string codigo)
        {
            using var connection = new SqlConnection(connectionStringAcceso);
            string sQuery = $@"SELECT * FROM DYGPINVEReservasArticulos where 
                    ITEMNMBR = '{codigo.Trim()}' order by dFechaReserva desc";

            try
            {
                IEnumerable<DYGPINVEReservasArticulosDTO> Lista = await connection
                        .QueryAsync<DYGPINVEReservasArticulosDTO>(sQuery);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }

        public async Task<List<DYGPPK01033DTO>> TraerOrdenesProduccion(string codigo)
        {
            using var connection = new SqlConnection(connectionStringAcceso);
            string sQuery = $@"SELECT * FROM DYGPPK01033 where 
                    ITEMNMBR = '{codigo.Trim()}' order by 1,3 desc";

            try
            {
                IEnumerable<DYGPPK01033DTO> Lista = await connection
                        .QueryAsync<DYGPPK01033DTO>(sQuery);

                return Lista.ToList();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }
        public async Task Reservar(string item, decimal cantidad, int signo, DateTime fechaVencimiento, string usuario, string descripcion = "")
        {

            using var connection = new SqlConnection(connectionStringAcceso);
            string comando = $@"UPDATE PRD01.DBO.iv00102 SET QTYONHND = QTYONHND - {cantidad * signo}
                ,QTYINSVC = QTYINSVC +  {cantidad * signo}
            Where itemnmbr = '{item}' and LOCNCODE in('DPT-CUY','')";

            int control = 0;
            control = await connection.ExecuteAsync(comando);

    
            DateTime Fecha = DateTime.Now;

            if(control > 0)
            {
                        await connection.ExecuteAsync($@"insert DYGPINVEReservasArticulos 
                        (ITEMNMBR, sUsuarioID, sDescripcion, dFechaReserva, dFechaVencimiento, nCantidad)
                        Values (@item, @usuario, @descripcion, @Fecha, @fechaVencimiento, @cantidad * @signo)", 
                        new { @item, @usuario, @descripcion, @Fecha, fechaVencimiento, @cantidad, @signo  });

            }

        }

        public async Task<DYGPIV00101DTO> InfoProducto(string codigo)
        {
            using var connection = new SqlConnection(connectionStringAcceso);
            string sQuery = $@"SELECT * FROM DYGPIV00101 where 
                    sArticuloID = '{codigo.Trim()}'";

            try
            {
                IEnumerable<DYGPIV00101DTO> objeto = await connection
                        .QueryAsync<DYGPIV00101DTO>(sQuery);

                return objeto.FirstOrDefault();
            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return null;
            }
        }

    }


}

