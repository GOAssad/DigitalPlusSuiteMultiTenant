using Dapper;
using DigitalPlus.Data;
using DigitalPlus.Entidades;
using DigitalPlus.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DigitalPlus.Repositorios
{
    public class RepositorioSucursales
    {
        private readonly ApplicationDbContext context;
        
        private readonly string connectionString;

        public RepositorioSucursales(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            connectionString = configuration.GetConnectionString("DefaultConnection");

            
        }
        /// <summary>
        /// Lista de Todas las Sucursales
        /// </summary>
        /// <param name="texto">String para filtrar</param>
        /// <returns>Lista de sucursales Filtradas por el texto</returns>
        public async Task<List<Sucursal>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.Sucursales.AsNoTracking().ToListAsync();

            else
                return await context.Sucursales.Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<Sucursal>> GetLegajosSucursalesSQL(int legajo)
        {

            using var connection = new SqlConnection(connectionString);

            IEnumerable<Sucursal> lista = await connection.QueryAsync<Sucursal>(@"Select a.* from sucursales a
		                inner join LegajosSucursales b on a.id = b.SucursalId
		                 where b.LegajoId = @legajo", new { legajo });

            return lista.ToList();
        }

        

        public List<Sucursal> GetUsuarioSucursalesSQL(string UsuarioId)
        {

            using var connection = new SqlConnection(connectionString);
            IEnumerable<Sucursal> lista = connection.Query<Sucursal>($@"select a.* 
                    from Sucursales a
                    inner join UsuariosSucursales b on a.Id = b.SucursalId
                    where UsuarioId = @usuarioId
                    ", new { UsuarioId });

            return lista.ToList();
        }

        /// <summary>
        /// Todas Las Sucursales
        /// </summary>
        /// <returns>Lista de todas las sucursales</returns>
        public async Task<List<Sucursal>> Get()
        {
            return await context.Sucursales.AsNoTracking().ToListAsync();

        }
        public async Task<List<Sucursal>> GetLegajosSucursales(int Legajoid)
        {
            return await context.Sucursales
                .Include(x => x.LegajoSucursal.Where(x => x.LegajoId == Legajoid))
                .ToListAsync();
        }

        public async Task<Sucursal> Get(int id)
        {
            return await context.Sucursales.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Sucursal> GetLista()
        {
            return  context.Sucursales.AsNoTracking().ToList();
        }

        public async Task<int> Post(Sucursal sucursal)
        {
            context.Add(sucursal);
            await context.SaveChangesAsync();
            return sucursal.Id;
        }

        public async Task Put(Sucursal sucursal)
        {
            context.Attach(sucursal).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Sucursales.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Sucursal {id} no encontrado"); }
            context.Remove(new Sucursal { Id = id });
            await context.SaveChangesAsync();
        }

    }
}
