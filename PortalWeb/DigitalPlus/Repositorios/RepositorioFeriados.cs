using DigitalPlus.Data;
using DigitalPlus.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Repositorios
{
    public class RepositorioFeriados
    {
        private readonly ApplicationDbContext context;

        public RepositorioFeriados(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Feriado>> Get()
        {
            return await context.Feriados.AsNoTracking()
                .OrderBy(o => o.Fecha)
                .ToListAsync();
        }
        public async Task<List<Feriado>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.Feriados.AsNoTracking().ToListAsync();

            else
                return await context.Feriados.Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }

        public async Task<Feriado> Get(int id)
        {
            return await context.Feriados.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Post(Feriado feriado)
        {
            context.Add(feriado);
            await context.SaveChangesAsync();
            return feriado.Id;
        }

        public async Task Put(Feriado feriado)
        {
            context.Attach(feriado).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Feriados.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Feriado {id} no encontrado"); }
            context.Remove(new Feriado { Id = id });
            await context.SaveChangesAsync();
        }

    }
}
