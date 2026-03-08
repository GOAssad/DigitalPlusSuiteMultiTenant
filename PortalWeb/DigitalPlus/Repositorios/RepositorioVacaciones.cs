using Dapper;
using DigitalPlus.Data;
using DigitalPlus.DTOs.Vacaciones;
using DigitalPlus.Entidades;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DigitalPlus.Repositorios
{
    public class RepositorioVacaciones
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public RepositorioVacaciones(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Vacacion>> Get(int id)
        {

            return await this.context.Vacaciones.AsNoTracking()
                .Where(x => x.LegajoId == id).ToListAsync();

        }

        public async Task Post(Vacacion vacacion)
        {
            int LegajoId;
            string Nota;
            DateTime @FechaDesde, @FechaHasta;

            LegajoId = vacacion.LegajoId;
            Nota = vacacion.Nota;
            FechaDesde = vacacion.FechaDesde;
            FechaHasta = vacacion.FechaHasta;


            using var connection = new SqlConnection(connectionString);
            await connection.QueryAsync($@"insert into Vacaciones (LegajoId, FechaDesde, FechaHasta, Nota)
                        values (@LegajoId, @FechaDesde, @FechaHasta, @Nota)", new { @LegajoId, @FechaDesde, @FechaHasta, @Nota });

        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.QueryAsync($@"delete Vacaciones where id = @id", new { @id });
        }

        public async Task<List<ListadoVacacionesPorRangodeFechasDTO>> ListadoVacaciones(DateTime fd, DateTime fh)
        {
            using var connection = new SqlConnection(connectionString);
            var listado = await connection.QueryAsync<ListadoVacacionesPorRangodeFechasDTO>($@"set language spanish
                    select b.Id LegajoId,
                    b.Nombre, c.id SectorId, c.Nombre NombreSector, a.FechaDesde, a.FechaHasta, a.Nota
                    from vacaciones a
                    inner join legajos b on a.LegajoId = b.Id
                    inner join Sectores c on b.SectorId = c.Id
                    where 
                    a.FechaDesde >= @fd and a.FechaDesde <= @fh and
                    a.FechaHasta <= @fh and a.FechaHasta >= @fd",new { @fd, @fh});

            return listado.ToList();
        }

    }
}
