using DigitalPlus.Data;
using DigitalPlus.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Repositorios
{
    public class RepositorioTerminales
    {
        private readonly ApplicationDbContext context;

        public RepositorioTerminales(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Terminal>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.Terminales.Include(p => p.Sucursal).AsNoTracking().ToListAsync();

            else
                return await context.Terminales.Where(x => x.Nombre.Contains(texto) || x.Descripcion.Contains(texto))
                .Include(p => p.Sucursal).AsNoTracking().ToListAsync();


        }
        public async Task<List<Terminal>> Get()
        {
            return await context.Terminales
                .Include(p => p.Sucursal)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Terminal> Get(int id)
        {
            return await context.Terminales.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Post(Terminal terminal)
        {
            context.Add(terminal);
            await context.SaveChangesAsync();
            return terminal.Id;
        }

        public async Task Put(Terminal terminal)
        {
            context.Attach(terminal).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Terminales.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Terminal {id} no encontrado"); }
            context.Remove(new Terminal { Id = id });
            await context.SaveChangesAsync();
        }


    }
}

   


        