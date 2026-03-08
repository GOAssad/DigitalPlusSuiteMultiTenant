using DigitalPlus.Data;
using DigitalPlus.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Repositorios
{
    public class RepositorioIncidencias
    {
        private readonly ApplicationDbContext context;

        public RepositorioIncidencias(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Incidencia>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.Incidencias.AsNoTracking().ToListAsync();

            else
                return await context.Incidencias.Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }


        public async Task<IncidenciaLegajo> GetIncidenciaLegajo(int id)
        {
            return await context.IncidenciasLegajos
                .Where(x => x.Id == id)
                .Include(x => x.Legajo)
                .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Incidencia> Get(int id)
        {
            return await context.Incidencias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Post(Incidencia Incidencia)
        {
            context.Add(Incidencia);
            await context.SaveChangesAsync();
            return Incidencia.Id;
        }
        public async Task<int> Post(IncidenciaLegajo Incidencia)
        {
            context.Add(Incidencia);
            await context.SaveChangesAsync();
            return Incidencia.Id;
        }

        public async Task Put(IncidenciaLegajo Incidencia)
        {
            context.Attach(Incidencia).State = EntityState.Modified;
            await context.SaveChangesAsync();
            
        }

        public async Task Put(Incidencia Incidencia)
        {
            context.Attach(Incidencia).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Incidencias.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Incidencia {id} no encontrado"); }
            context.Remove(new Incidencia { Id = id });
            await context.SaveChangesAsync();
        }

        public async Task DeleteIncidencialegajo(int id)
        {
            var existe = await context.IncidenciasLegajos.AnyAsync(x=> x.Id == id);
            if (!existe) { throw new ApplicationException($"IncidenciaLegajo {id} no encontrado"); }
            context.Remove(new IncidenciaLegajo { Id = id });
            await context.SaveChangesAsync();
        }

    }
}




