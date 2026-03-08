using DigitalPlus.Data;
using DigitalPlus.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Repositorios
{
    public class RepositorioSectores
    {
        private readonly ApplicationDbContext context;

        public RepositorioSectores(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Sector>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.Sectores.AsNoTracking().ToListAsync();

            else
                return await context.Sectores.Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<Sector>> Get()
        {
            
                return await context.Sectores.AsNoTracking().ToListAsync();

        }
        public List<Sector> GetLista()
        {

            return context.Sectores.AsNoTracking().ToList();

        }

        public async Task<Sector> Get(int id)
        {
            return await context.Sectores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Post(Sector sector)
        {
            context.Add(sector);
            await context.SaveChangesAsync();
            return sector.Id;
        }

        public async Task Put(Sector sector)
        {
            context.Attach(sector).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Sectores.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Sector {id} no encontrado"); }
            context.Remove(new Sector { Id = id });
            await context.SaveChangesAsync();
        }

    }
}
