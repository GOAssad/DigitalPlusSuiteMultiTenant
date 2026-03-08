using Dapper;
using DigitalPlus.Data;
using DigitalPlus.Entidades;
using DigitalPlus.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DigitalPlus.Repositorios
{
    public class RepositorioNoticias
    {
        private readonly ApplicationDbContext context;
        
        private readonly string connectionString;

        public RepositorioNoticias(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            connectionString = configuration.GetConnectionString("DefaultConnection");

            
        }
        /// <summary>
        /// Lista de Todas las Noticias
        /// </summary>
        /// <param name="texto">String para filtrar</param>
        /// <returns>Lista de noticias Filtradas por el texto</returns>
        public async Task<List<Noticia>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.Noticias.AsNoTracking().ToListAsync();

            else
                return await context.Noticias.Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<Noticia>> GetLegajosNoticiasSQL(int legajo)
        {

            using var connection = new SqlConnection(connectionString);

            IEnumerable<Noticia> lista = await connection.QueryAsync<Noticia>(@"Select a.* from noticias a
		                inner join LegajosNoticias b on a.id = b.NoticiaId
		                 where b.LegajoId = @legajo", new { legajo });

            return lista.ToList();
        }

        public List<Noticia> GetUsuarioNoticiasSQL(string UsuarioId)
        {

            using var connection = new SqlConnection(connectionString);
            IEnumerable<Noticia> lista = connection.Query<Noticia>($@"select a.* 
                    from Noticias a
                    inner join UsuariosNoticias b on a.Id = b.NoticiaId
                    where UsuarioId = @usuarioId
                    ", new { UsuarioId });

            return lista.ToList();
        }

        /// <summary>
        /// Todas Las Noticias
        /// </summary>
        /// <returns>Lista de todas las noticias</returns>
        public async Task<List<Noticia>> Get()
        {
            return await context.Noticias.AsNoTracking().ToListAsync();
        }

        public async Task<List<Noticia>> GetPublicos()
        {
            return await context.Noticias.Where(x => x.Privado == false).AsNoTracking().ToListAsync();
        }
        
        public async Task<Noticia> Get(int id)
        {
            return await context.Noticias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Noticia> GetLista()
        {
            return  context.Noticias.AsNoTracking().ToList();
        }

        public async Task<int> Post(Noticia noticia)
        {
            context.Add(noticia);
            await context.SaveChangesAsync();
            return noticia.Id;
        }

        public async Task Put(Noticia noticia)
        {
            context.Attach(noticia).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Noticias.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Noticia {id} no encontrada"); }
            context.Remove(new Noticia { Id = id });
            await context.SaveChangesAsync();
        }

    }
}
