using System;
using System.Collections.Generic;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHFichadasEntradaEstatusReport
    {
        public string FechaReporte { get; private set; }
        public DateTime InicioDate { get; private set; }
        public DateTime FinDate { get; private set; }
        public List<RRHHFichadasEntradaEstatusListing> fichadaEstatusListing { get; set; }

        //Metodos
        public void CreateFichadasEntradaEstatus( DateTime fdesde, DateTime fhasta, bool solotarde, string grupo)
        {

            InicioDate = fdesde;
            FinDate = fhasta;
            FechaReporte = DateTime.Now.ToShortDateString();


            var rrhhfichadasdao = new RRHHFichadasDao();
            var result = rrhhfichadasdao.getFichadasEntradaEstatus(fdesde, fhasta, solotarde, grupo);

            fichadaEstatusListing = new List<RRHHFichadasEntradaEstatusListing>();
            foreach (System.Data.DataRow rows in result.Rows)
            {

                var fichadaestatus = new RRHHFichadasEntradaEstatusListing()
                {
                    sLegajoID = rows["sLegajoID"].ToString(),
                    sApellido = rows["sApellido"].ToString(),
                    sNombre = rows["sNombre"].ToString(),
                    sFecha = rows["Fecha"].ToString(),
                    sHorarioID = rows["sHorarioID"].ToString(),
                    sDescripcion = rows["sDescripcion"].ToString(),
                    sHoraEntrada = rows["HoraEntrada"].ToString(),
                    sHoraEntradaOficial = rows["HoraEntradaOficial"].ToString(),
                    Estatus = rows["ESTATUS"].ToString(),
                    sUbicacion = rows["Sector"].ToString(),
                    sDescSucursal = rows["sDescSucursal"].ToString()

                };

                fichadaEstatusListing.Add(fichadaestatus);
            }
        }
    }
}
