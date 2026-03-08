using Dapper;
using DigitalPlus.Data;
using DigitalPlus.Entidades;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DigitalPlus.Servicios
{
    public interface IDayEventService
    {
        DayEvent SaveOrUpdate(DayEvent oDayEvent);
        DayEvent GetEvent(DateTime eventDate);
        List<DayEvent> GetEvents(DateTime fromDate, DateTime toDate);
        string Delete(int id);
    }

    public class DayEventService : IDayEventService
    {
        private readonly IConfiguration Configuration;
        DayEvent _oDayEvent = new DayEvent();
        List<DayEvent> _oDayEvents = new List<DayEvent>();

        public DayEventService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Delete(int id)
        {
            string message = string.Empty;
            try
            {
                _oDayEvent = new DayEvent()
                {
                    DayEventId = id
                };
                using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var oDayEvents = con.Query<DayEvent>("SP_DayEvent",
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

        public DayEvent GetEvent(DateTime eventDate)
        {

            using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                _oDayEvent = new DayEvent();
                if (con.State == ConnectionState.Closed) con.Open();

                string sql = string.Format(@"set language spanish;
                            SELECT * FROM DayEvent WHERE EventDate = '{0}' ",
                                eventDate.ToShortTimeString());

                var oDayEvents = con.Query<DayEvent>(sql).ToList();

                if (oDayEvents != null && oDayEvents.Count() > 0)
                {
                    _oDayEvent = oDayEvents.SingleOrDefault();
                }
                else
                {
                    _oDayEvent.EventDate = eventDate;
                    _oDayEvent.FromDate = eventDate;
                    _oDayEvent.ToDate = eventDate;
                }
            }
            return _oDayEvent;
        }

        public List<DayEvent> GetEvents(DateTime fromDate, DateTime toDate)
        {
            _oDayEvents = new List<DayEvent>();
            using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                string sql = string.Format(@"set language spanish;
                        SELECT * FROM DayEvent WHERE EventDate BETWEEN '{0}' AND '{1}'",
                    fromDate.ToShortDateString(), toDate.ToShortDateString());

                var oDayEvents = con.Query<DayEvent>(sql).ToList();

                if (oDayEvents != null && oDayEvents.Count() > 0)
                {
                    _oDayEvents = oDayEvents;
                }
            }
            return _oDayEvents;
        }

        public DayEvent SaveOrUpdate(DayEvent oDayEvent)
        {
            _oDayEvent = new DayEvent();
            try
            {
                int operationType = Convert.ToInt32(oDayEvent.DayEventId == 0 ? OperationType.Insert : OperationType.Update);

                using (IDbConnection con = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var oDayEvents = con.Query<DayEvent>("SP_DayEvent",
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

                _oDayEvent.Message = ex.Message;
            }
            return _oDayEvent;
        }

        private DynamicParameters SetParameters(DayEvent oDayEvent, int operationType)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@DayEventId", oDayEvent.DayEventId);
            parameters.Add("@Note", oDayEvent.Note);
            parameters.Add("@EventDate", oDayEvent.EventDate);
            parameters.Add("@OperationType", operationType);

            return parameters;

        }
    }
}
