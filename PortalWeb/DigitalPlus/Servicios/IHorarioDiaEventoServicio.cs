using Dapper;
using DigitalPlus.Data;
using DigitalPlus.Entidades;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Servicios
{
    public interface IHorarioDiaEventoServicio
    {
        HorarioDiaEvento SaveOrUpdate(HorarioDiaEvento oDayEvent);
        HorarioDiaEvento GetEvent(DateTime eventDate);
        List<HorarioDiaEvento> GetEvents(DateTime fromDate, DateTime toDate);
        string Delete(int id);
        Task<HorarioDiaEvento> Post(HorarioDiaEvento oDayEvent);
        List<HorarioDiaEvento> GetEventos(DateTime fromDate, DateTime toDate, int legajo);
        Task<HorarioDiaEvento> GetEvento(DateTime fechaEvento, int legajo);
        Task Borrar(HorarioDiaEvento modelo);
        Task BorrarYAgregar(List<HorarioDiaEvento> aInsertar, bool insertar);
        Task<bool> QueCalendario(int id);
    }

    public class HorarioDiaEventoServicio : IHorarioDiaEventoServicio
    {
        private readonly IConfiguration Configuration;
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;
        HorarioDiaEvento _oDayEvent = new HorarioDiaEvento();
        List<HorarioDiaEvento> _oDayEvents = new List<HorarioDiaEvento>();
        public HorarioDiaEventoServicio(IConfiguration configuration, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            Configuration = configuration;
            this.contextFactory = contextFactory;
        }
        public string Delete(int id)
        {
            string message = string.Empty;
            try
            {
                _oDayEvent = new HorarioDiaEvento()
                {
                    Id = id
                };
                using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var oDayEvents = con.Query<HorarioDiaEvento>("SP_DayEvent",
                        this.SetParameters(_oDayEvent, (int)OperationType.Delete),
                        commandType: CommandType.StoredProcedure);

                    message = "Deleted";
                }
            }
            catch (Exception ex)
            {

                message = ex.Message;
            }
            return message;
        }

        public async Task<HorarioDiaEvento> GetEvento(DateTime fechaEvento, int legajo)
        {
            _oDayEvent = new HorarioDiaEvento();
            using var context = contextFactory.CreateDbContext();
            var oDayEvents = await context.HorariosDiasEventos
               .Where(f => f.LegajoId == legajo)
               .Where(f => f.FechaDesde.Year == fechaEvento.Year
                                && f.FechaDesde.Month == fechaEvento.Month
                                && f.FechaDesde.Day == fechaEvento.Day)
               .Include(s => s.Legajo)
               .AsNoTracking()
               .FirstOrDefaultAsync();

            if (oDayEvents != null)
            {
                _oDayEvent = oDayEvents;
            }
            else
            {
                _oDayEvent.FechaDesde = fechaEvento;
                _oDayEvent.Nota = string.Empty;
                _oDayEvent.LegajoId = legajo;
            }

            return _oDayEvent;


        }

        public HorarioDiaEvento GetEvent(DateTime eventDate)
        {

            using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                _oDayEvent = new HorarioDiaEvento();
                if (con.State == ConnectionState.Closed) con.Open();

                string sql = string.Format(@"set language spanish;
                            SELECT * FROM DayEvent WHERE EventDate = '{0}' ",
                                eventDate.ToShortTimeString());

                var oDayEvents = con.Query<HorarioDiaEvento>(sql).ToList();

                if (oDayEvents != null && oDayEvents.Count() > 0)
                {
                    _oDayEvent = oDayEvents.SingleOrDefault();
                }
                else
                {
                    _oDayEvent.FechaDesde = eventDate;
                    _oDayEvent.Nota = String.Empty;
                }
            }
            return _oDayEvent;
        }
        public List<HorarioDiaEvento> GetEvents(DateTime fromDate, DateTime toDate)
        {
            _oDayEvents = new List<HorarioDiaEvento>();
            using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                string sql = string.Format(@"set language spanish;
                        SELECT * FROM DayEvent WHERE EventDate BETWEEN '{0}' AND '{1}'",
                    fromDate.ToShortDateString(), toDate.ToShortDateString());

                var oDayEvents = con.Query<HorarioDiaEvento>(sql).ToList();

                if (oDayEvents != null && oDayEvents.Count() > 0)
                {
                    _oDayEvents = oDayEvents;
                }
            }
            return _oDayEvents;
        }
        public List<HorarioDiaEvento> GetEventos(DateTime fromDate, DateTime toDate, int legajo)
        {
            using var context = contextFactory.CreateDbContext();
            return context.HorariosDiasEventos.AsNoTracking()
                .Where(x => x.LegajoId == legajo).ToList();

        }
        public async Task<HorarioDiaEvento> Post(HorarioDiaEvento oDayEvent)
        {
            using var context = contextFactory.CreateDbContext();
            //primero borro lo que tenga este legajo el dia en que estoy parado
            var objeto = await context.HorariosDiasEventos.AsNoTracking()
                .Where(x => x.LegajoId == oDayEvent.LegajoId)
                .Where(x => x.FechaDesde.Year == oDayEvent.FechaDesde.Year)
                .Where(x => x.FechaDesde.Month == oDayEvent.FechaDesde.Month)
                .Where(x => x.FechaDesde.Day == oDayEvent.FechaDesde.Day)
                .FirstOrDefaultAsync();

            if (objeto is not null)
            {
                context.Remove(new HorarioDiaEvento { Id = objeto.Id });
                await context.SaveChangesAsync();
            }

            context.Add(oDayEvent);
            await context.SaveChangesAsync();
            return oDayEvent;
        }
        public HorarioDiaEvento SaveOrUpdate(HorarioDiaEvento oDayEvent)
        {
            _oDayEvent = new HorarioDiaEvento();
            try
            {
                int operationType = Convert.ToInt32(oDayEvent.Id == 0 ? OperationType.Insert : OperationType.Update);

                using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var oDayEvents = con.Query<HorarioDiaEvento>("SP_DayEvent",
                        this.SetParameters(oDayEvent, operationType),
                        commandType: CommandType.StoredProcedure);

                    if (oDayEvents != null && oDayEvents.Count() > 0)
                    {
                        _oDayEvent = oDayEvents.FirstOrDefault();
                    }
                }

            }
            catch (Exception ex)
            {

                return null;
            }
            return _oDayEvent;
        }
        public async Task<bool> QueCalendario(int id)
        {
            using var context = contextFactory.CreateDbContext();
            return await context.HorariosDiasEventos.AsNoTracking().AnyAsync(x => x.LegajoId == id);
            
        }
        private DynamicParameters SetParameters(HorarioDiaEvento oDayEvent, int operationType)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@DayEventId", oDayEvent.Id);
            parameters.Add("@Note", oDayEvent.Nota);
            parameters.Add("@EventDate", oDayEvent.FechaDesde);
            parameters.Add("@OperationType", operationType);

            return parameters;

        }
        public async Task Borrar(HorarioDiaEvento modelo)
        {
            using var context = contextFactory.CreateDbContext();
            var existe = await context.HorariosDiasEventos.AsNoTracking().AnyAsync(x => x.Id == modelo.Id);
            if (!existe) { throw new ApplicationException($"Evento {modelo.Id} no encontrado"); }

        
            context.Remove(modelo);
            await context.SaveChangesAsync();
        }
        
        public async Task BorrarYAgregar(List<HorarioDiaEvento> aInsertar, bool insertar)
        {
            using var context = contextFactory.CreateDbContext();
            foreach (var item in aInsertar)
            {
                await BorrarPorFechaYLegajo(item.LegajoId, item.FechaDesde.Year, item.FechaDesde.Month, item.FechaDesde.Day);

            }

            if (insertar)
            {

                context.HorariosDiasEventos.AddRange(aInsertar);
                await context.SaveChangesAsync();
            }

        }

        public async Task BorrarPorFechaYLegajo(int legajoid, int ano, int mes, int dia)
        {
            using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                await con.QueryAsync(@"Delete HorariosDiasEventos 
                        where legajoId = @legajoid and 
                        Year(FechaDesde) = @ano and Month(FechaDesde) = @mes and 
                        Day(FechaDesde) = @dia", new { legajoid, ano, mes, dia });

            }
        }

    }
}
