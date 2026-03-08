using DigitalPlus.Data;
using DigitalPlus.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Repositorios
{
    public class RepositorioVariablesGlobales
    {
        private readonly ApplicationDbContext context;

        public RepositorioVariablesGlobales(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<VariableGlobal>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.VariablesGlobales.AsNoTracking().ToListAsync();

            else
                return await context.VariablesGlobales.Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }
        public async Task<List<VariableGlobal>> Get()
        {

            return await context.VariablesGlobales.AsNoTracking().ToListAsync();

        }
        public List<VariableGlobal> GetLista()
        {

            return context.VariablesGlobales.AsNoTracking().ToList();

        }

        public async Task<VariableGlobal> Get(int id)
        {
            return await context.VariablesGlobales.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<VariableGlobal> GetPorsId(string sid)
        {
            return await context.VariablesGlobales.AsNoTracking().FirstOrDefaultAsync(x => x.sId == sid);
        }

        public async Task<int> Post(VariableGlobal variable)
        {
            context.Add(variable);
            await context.SaveChangesAsync();
            return variable.Id;
        }

        public async Task Put(VariableGlobal variable)
        {
            context.Attach(variable).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.VariablesGlobales.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Variable Global {id} no encontrada"); }
            context.Remove(new VariableGlobal{ Id = id });
            await context.SaveChangesAsync();
        }

    }
}
