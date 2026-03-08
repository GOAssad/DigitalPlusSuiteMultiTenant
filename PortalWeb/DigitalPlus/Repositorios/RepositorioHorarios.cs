using Dapper;
using DigitalPlus.Data;
using DigitalPlus.DTOs;
using DigitalPlus.Entidades;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace DigitalPlus.Repositorios
{
    public class RepositorioHorarios
    {
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;
        private readonly string connectionString;

        public RepositorioHorarios(IDbContextFactory<ApplicationDbContext> contextFactory, 
            IConfiguration configuration)
        {
            this.contextFactory = contextFactory;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Horario>> Get(string texto)
        {
            using var context = this.contextFactory.CreateDbContext();
            if (texto is null || texto == string.Empty)
                return await context.Horarios
                    .Include(x => x.HorariosDias)
                    .AsNoTracking().ToListAsync();

            else
                return await context.Horarios.Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }

        
        public async Task<Horario> Get(int id)
        {
            using var context = this.contextFactory.CreateDbContext();
            return await context.Horarios
                .Include(x => x.HorariosDias).ThenInclude(d => d.Dia)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Post(Horario modelo)
        {

            using var context = this.contextFactory.CreateDbContext();
            context.Add(modelo);
            await context.SaveChangesAsync();
            return modelo.Id;
        }

        public List<Horario> GetLista()
        {
            using var context = this.contextFactory.CreateDbContext();
            return context.Horarios.AsNoTracking().ToList();

        }

        public async Task Put(Horario modelo)
        {
            using var connection = new SqlConnection(connectionString);

            string nombre = modelo.Nombre;
            int Id = modelo.Id;

            await connection.QueryAsync($@"update horarios set Nombre = @Nombre where Id = @Id", new { @nombre, @Id });

            int id = 0;
            int HorarioId = 0;
            int DiaId = 0;
            int HoraDesde = 0;
            int HoraHasta  = 0;
            int MinDesde  = 0;
            int MinHasta = 0;
            int Cerrado = 0;

            // borro los dias de este horario.
            await connection.QueryAsync($@"delete horariosDias where HorarioId = @Id", new { @Id });

            // Inserto la lista de los horarios
            foreach (var item in modelo.HorariosDias)
            {
                id= item.Id;
                HorarioId= item.HorarioId;
                DiaId= item.DiaId;
                HoraDesde= item.HoraDesde;
                HoraHasta= item.HoraHasta;
                MinDesde = item.MinutoDesde;
                MinHasta = item.MinutoHasta;
                Cerrado = item.Cerrado;

                await connection.QueryAsync($@"insert into HorariosDias (HorarioId, DiaId, HoraDesde, HoraHasta, MinutoDesde, MinutoHasta, Cerrado)
                    Values (@HorarioId, @DiaId, @HoraDesde, @HoraHasta, @MinDesde, @MinHasta, @Cerrado)", 
                    new { @Id, @HorarioId, @DiaId, @HoraDesde, @HoraHasta, @MinDesde, @MinHasta, @Cerrado });
            }

            //using var context = this.contextFactory.CreateDbContext();
            //try
            //{
            //    context.Attach(modelo).State = EntityState.Modified;
            //    await context.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{

            //    return ex.Message;
            //}
            //return string.Empty;
        }


        public async Task Delete(int id)
        {
            using var context = this.contextFactory.CreateDbContext();
            var existe = await context.Horarios.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Horario {id} no encontrado"); }
            context.Remove(new Horario { Id = id });
            await context.SaveChangesAsync();
        }

        public async Task<List<Dia>> GetDias()
        {
            using var context = this.contextFactory.CreateDbContext();
            return await context.Dias.AsNoTracking()
                .OrderBy(o => o.Id)
                .ToListAsync();
        }
    }
}
