using DigitalPlus.Data;
using DigitalPlus.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Repositorios
{
    public class RepositorioCategorias
    {
        private readonly ApplicationDbContext context;

        public RepositorioCategorias(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Categoria>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.Categorias.AsNoTracking().ToListAsync();

            else
                return await context.Categorias.Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }
        public async Task<List<Categoria>> Get()
        {

            return await context.Categorias.AsNoTracking().ToListAsync();

        }
        public List<Categoria> GetLista()
        {

            return context.Categorias.AsNoTracking().ToList();

        }

        public async Task<Categoria> Get(int id)
        {
            return await context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Post(Categoria categoria)
        {
            context.Add(categoria);
            await context.SaveChangesAsync();
            return categoria.Id;
        }

        public async Task Put(Categoria categoria)
        {
            context.Attach(categoria).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Categorias.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Categoria {id} no encontrado"); }
            context.Remove(new Categoria { Id = id });
            await context.SaveChangesAsync();
        }

    }
}
