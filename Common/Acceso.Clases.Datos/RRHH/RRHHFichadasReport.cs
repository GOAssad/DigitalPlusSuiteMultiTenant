using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHFichadasReport
    {
        public DateTime reportDate { get; private set; }
        public DateTime startDate { get; private set; }
        public DateTime endDate { get; private set; }
        public List <RRHHFichadasListing> fichadalisting { get; set; }
        public List<RRHHHorasTrabajadasListing> horastrabajadaslisting { get; set; }

        //Metodos
        public void createFichadasReport(DateTime fromDate, DateTime toDate, string legajodesde, string legajohasta, string grupo)
        {
            reportDate = DateTime.Now;
            startDate = fromDate;
            endDate = toDate;

            var rrhhfichadasdao = new RRHHFichadasDao();
            var result = rrhhfichadasdao.getFichadas(fromDate, toDate, legajodesde, legajohasta, grupo);

            fichadalisting = new List<RRHHFichadasListing>();
            foreach (System.Data.DataRow rows in result.Rows)
            {
                var fichadasmodel = new RRHHFichadasListing()
                {
                    slegajoid = rows["sLegajoID"].ToString(),
                    sapellido = rows["sApellido"].ToString(),
                    snombre = rows["sNombre"].ToString(),
                    horariosector = rows["HorarioSector"].ToString(),
                    fecha = rows["Fecha"].ToString(),
                    hora = rows["Hora"].ToString(),
                    sentrasale = rows["sEntraSale"].ToString(),
                    sSucursalID = rows["sSucursalID"].ToString(),
                    sSucursalGrupoID = rows["sSucursalGrupoID"].ToString(),
                    sDescSucursal = rows["sDescSucursal"].ToString(),
                    horaturno = rows["HorarioEstab"].ToString(),
                    sDiaNombre = rows["DiaSemana"].ToString()
                };
                fichadalisting.Add(fichadasmodel);
            }
        }

        public void createFichadasReport(DateTime fromDate, DateTime toDate, string legajodesde, string legajohasta, string ES, string grupo)
        {
            reportDate = DateTime.Now;
            startDate = fromDate;
            endDate = toDate;

            var rrhhfichadasdao = new RRHHFichadasDao();
            var result = rrhhfichadasdao.getFichadas(fromDate, toDate, legajodesde, legajohasta, grupo);

            fichadalisting = new List<RRHHFichadasListing>();
            foreach (System.Data.DataRow rows in result.Rows)
            {
                if (rows["sEntraSale"].ToString() == ES)
                {
                    var fichadasmodel = new RRHHFichadasListing()
                    {
                        slegajoid = rows["sLegajoID"].ToString(),
                        sapellido = rows["sApellido"].ToString(),
                        snombre = rows["sNombre"].ToString(),
                        horariosector = rows["HorarioSector"].ToString(),
                        fecha = rows["Fecha"].ToString(),
                        hora = rows["Hora"].ToString(),
                        sentrasale = rows["sEntraSale"].ToString(),
                        sSucursalID = rows["sSucursalID"].ToString(),
                        sSucursalGrupoID = rows["sSucursalGrupoID"].ToString(),
                        horaturno = rows["HorarioEstab"].ToString(),
                        sDiaNombre = rows["DiaSemana"].ToString()
                    };
                    fichadalisting.Add(fichadasmodel);
                }
               
            }
        }
        public void createHorasTrabajadasReport(DateTime fromDate, DateTime toDate, string legajodesde, string legajohasta, string grupo)
        {
            reportDate = DateTime.Now;
            startDate = fromDate;
            endDate = toDate;

            var rrhhfichadasdao = new RRHHHorasTrabajadasDao();
            var result = rrhhfichadasdao.getHorasTrabajadas(fromDate, toDate, legajodesde, legajohasta, grupo);

            horastrabajadaslisting = new List<RRHHHorasTrabajadasListing>();
            foreach (System.Data.DataRow rows in result.Rows)
            {
                
                var fichadasmodel = new RRHHHorasTrabajadasListing()
                {
                    slegajoid = rows["sLegajoID"].ToString(),
                    sapellido = rows["sApellido"].ToString(),
                    snombre = rows["sNombre"].ToString(),
                    horariosector = rows["HorarioSector"].ToString(),
                    fecha = rows["Fecha"].ToString(),
                    MinHora = rows["MinHora"].ToString(),
                    MaxHora = rows["MaxHora"].ToString(),
                    sSucursalID = rows["sSucursalID"].ToString(),
                    sSucursalGrupoID = rows["sSucursalGrupoID"].ToString(),
                    DiaSemana = rows["DiaSemana"].ToString(),
                    NumeroDia = (int)rows["NumeroDia"],
                    sDescSucursal = rows["sDescSucursal"].ToString(),
                    HorarioEstab = rows["HorarioEstab"].ToString(),
                    MinutosTrabajados = (int)rows["MinutosTrabajados"],
                    Hs_minTrab = rows["Hs_minTrab"].ToString()
                };
                horastrabajadaslisting.Add(fichadasmodel);
                
            }
        }
    }
}
