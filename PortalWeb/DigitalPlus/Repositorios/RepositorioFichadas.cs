using Dapper;
using DigitalPlus.Data;
using DigitalPlus.DTOs;
using DigitalPlus.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DigitalPlus.Repositorios
{
    public class RepositorioFichadas
    {
        private readonly ApplicationDbContext context;
        private readonly string connectionString;

        public RepositorioFichadas(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            //connectionString = configuration.GetConnectionString("DigitalOne");
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<FichadaAusenciasDTO>> GetAusencias(string usuario, DateTime fd, DateTime fh, int leg)
        {
            using var connection = new SqlConnection(connectionString);

            var Listado = await connection.QueryAsync<FichadaAusenciasDTO>($@"WebAusencias_Listado_General "
                , new { fd, fh,leg, usuario  }
                , commandType: System.Data.CommandType.StoredProcedure);

            return Listado.ToList();
        }
        public async Task<List<FichadaAusenciasDTO>> GetAusenciasSuc(DateTime fd, DateTime fh, int su, int le, string usuario)
        {
            using var connection = new SqlConnection(connectionString);

            var Listado = await connection.QueryAsync<FichadaAusenciasDTO>($@"WebAusencias_Listado_General_ConSucursales "
                , new { fd, fh, su, le, usuario }
                , commandType: System.Data.CommandType.StoredProcedure);

            return Listado.ToList();
        }

        public async Task<List<Fichada>> GetFichadas(DateTime fd, DateTime fh, int suc, int leg)
        {
            fh = fh.AddDays(1);


            if (suc == 0 && leg == 0)
            {
                return await context.Fichadas.AsNoTracking()
                .Where(f => f.Registro >= fd)
                .Where(f => f.Registro <= fh)
                .Include(s => s.Sucursal)
                .Include(l => l.Legajo)
                .OrderByDescending(o => o.Registro)
                .ToListAsync();
            }
            else if (suc > 0 && leg == 0)
            {
                return await context.Fichadas.AsNoTracking()
                .Where(f => f.Registro >= fd)
                .Where(f => f.Registro <= fh)
                .Where(s => s.SucursalId == suc)
                .Include(s => s.Sucursal)
                .Include(l => l.Legajo)
                .OrderByDescending(o => o.Registro)
                .ToListAsync();
            }
            else if (suc == 0 && leg > 0)
            {
                return await context.Fichadas.AsNoTracking()
                .Where(f => f.Registro >= fd)
                .Where(f => f.Registro <= fh)
                .Where(s => s.Legajoid == leg)
                .Include(s => s.Sucursal)
                .Include(l => l.Legajo)
                .OrderByDescending(o => o.Registro)
                .ToListAsync();
            }
            else
            {
                return await context.Fichadas.AsNoTracking()
                .Where(f => f.Registro >= fd)
                .Where(f => f.Registro <= fh)
                .Where(s => s.Legajoid == leg)
                .Where(s => s.SucursalId == suc)
                .Include(s => s.Sucursal)
                .Include(l => l.Legajo)
                .OrderByDescending(o => o.Registro)
                .ToListAsync();
            }

        }


        //traigo las fichadas de las sucursales asociadas al usuario logeado
        public async Task<List<FichadaListadoGeneralDTO>> GetFichadas(string usuario, DateTime fd, DateTime fh, int suc, int leg)
        {
            using var connection = new SqlConnection(connectionString);

            string whereSucursal = string.Empty;
            if (suc > 0)
                whereSucursal = " and a.SucursalId = @suc ";


            string whereLegajo = string.Empty;
            if (leg > 0)
                whereLegajo = " and a.LegajoId = @leg ";





            var Listado = await connection.QueryAsync<FichadaListadoGeneralDTO>($@"select d.*, 
	                c.Nombre as NombreSucursal, e.Nombre as NombreLegajo, c.CodigoSucursal 
                    from Fichadas d
	                inner join LegajosSucursales a on a.LegajoId = d.Legajoid
	                inner join UsuariosSucursales b on a.SucursalId = b.SucursalId
	                inner join sucursales c on a.SucursalId = c.Id
	                inner join legajos e on d.Legajoid = e.id
	                where b.UsuarioId  = @usuario and d.Registro between @fd and @fh " + whereSucursal + whereLegajo, new { usuario, fd, fh, suc, leg });

            return Listado.ToList();
        }

        /// <summary>
        /// Traer Todas las fichadas que llegaron Tarde o salieron temprano por rango de Fecha
        /// </summary>
        /// <param name="fd">Fecha Desde</param>
        /// <param name="fh">Fecha hasta</param>
        /// <returns>List<FichadaListadoGeneralDTO></FichadaListadoGeneralDTO></returns>
        public async Task<List<FichadaListadoGeneralDTO>> GetFichadasTardeGeneral(DateTime fed, DateTime feh, int suc, int leg, string usuarioId)
        {
            using var connection = new SqlConnection(connectionString);

            var listado = await connection.QueryAsync<FichadaListadoGeneralDTO>("WebLlegadaTarde_Listado_General", new { fed, feh, suc, leg, usuarioId }, commandType: System.Data.CommandType.StoredProcedure);
            return listado.ToList();
        }
        public async Task<List<Fichada>> Get(int id, DateTime fd, DateTime fh)
        {


            fh = fh.AddDays(1);
            return await context.Fichadas.AsNoTracking()
                .Where(f => f.Legajoid == id)
                .Where(f => f.Registro >= fd)
                .Where(f => f.Registro <= fh)
                .Include(s => s.Sucursal)
                .OrderByDescending(o => o.Registro)
                .ToListAsync();
        }
        public async Task<List<FichadaControlDTO>> GetControl(int id, DateTime fed, DateTime feh)
        {

            //string algo =  DateTime.Parse(fd, CultureInfo.CreateSpecificCulture("es-US"));
            //DateTime fed = DateTime.Parse(fd.ToString("d", CultureInfo.CreateSpecificCulture("es-US")));
            //DateTime feh = DateTime.Parse(fh.ToString("d", CultureInfo.CreateSpecificCulture("es-US")));

            using var connection = new SqlConnection(connectionString);
            //string sql = @$"WebControlAcceso {id},{fechad},{fechah}";

            var listado = await connection.QueryAsync<FichadaControlDTO>("WebControlAcceso_Listado", new {id, fed, feh }, commandType: System.Data.CommandType.StoredProcedure);

            return listado.OrderByDescending(x => x.Fecha).ToList();

        }
        public async Task<List<FichadaTardeDTO>> GetTarde(int id, DateTime fed, DateTime feh)
        {

            using var connection = new SqlConnection(connectionString);
            //string sql = @$"WebControlAcceso {id},{fechad},{fechah}";

            var listado = await connection.QueryAsync<FichadaTardeDTO>("WebLlegadaTarde_Listado", new { id, fed, feh }, commandType: System.Data.CommandType.StoredProcedure);

            return listado.OrderByDescending(x => x.Fecha).ToList();

        }
        public async Task<List<FichadaTardeDTO>> GetExtra(int id, DateTime fed, DateTime feh)
        {

            using var connection = new SqlConnection(connectionString);
            //string sql = @$"WebControlAcceso {id},{fechad},{fechah}";

            var listado = await connection.QueryAsync<FichadaTardeDTO>("WebLlegadaTarde_Listado", new { id, fed, feh }, commandType: System.Data.CommandType.StoredProcedure);

            return listado.OrderByDescending(x => x.Fecha).ToList();

        }
        public async Task<List<FichadaConsolidadoDTO>> GetConsolidado(int id, DateTime fd, DateTime fh)
        {

            //string algo =  DateTime.Parse(fd, CultureInfo.CreateSpecificCulture("es-US"));
            DateTime fed = DateTime.Parse(fd.ToString("d", CultureInfo.CreateSpecificCulture("es-US")));
            DateTime feh = DateTime.Parse(fh.ToString("d", CultureInfo.CreateSpecificCulture("es-US")));

            using var connection = new SqlConnection(connectionString);
            //string sql = @$"WebControlAcceso {id},{fechad},{fechah}";

            var listado = await connection.QueryAsync<FichadaConsolidadoDTO>("WebConsolidado_Listado", 
                    new { id, fed, feh }, commandType: System.Data.CommandType.StoredProcedure);

            return listado.ToList();

        }
        public async Task<Fichada> GetFichada(int id)
        {
            return await context.Fichadas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<int> Post(Fichada fichada)
        {
            fichada.Id = 0;
            context.Add(fichada);
            await context.SaveChangesAsync();
            return fichada.Id;
        }
        public async Task Put(Fichada fichada)
        {
            context.Attach(fichada).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var existe = await context.Fichadas.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Fichada {id} no encontrado"); }
            context.Remove(new Fichada { Id = id });
            await context.SaveChangesAsync();
        }

    }
}
