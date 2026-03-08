using Dapper;
using AutoMapper;
using DigitalPlus.Data;
using DigitalPlus.DTOs;
using DigitalPlus.Entidades;
using DigitalPlus.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DigitalPlus.Repositorios
{
    public class RepositorioLegajos
    {
        private readonly ApplicationDbContext context;
        private readonly RepositorioSectores repositorioSectores;
        private readonly RepositorioCategorias repositorioCategorias;
        private readonly string connectionString;

        public RepositorioLegajos(ApplicationDbContext context, IConfiguration configuration, 
                            RepositorioSectores repositorioSectores, 
                            RepositorioCategorias repositorioCategorias)
        {
            this.context = context;
            this.repositorioSectores = repositorioSectores;
            this.repositorioCategorias = repositorioCategorias;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<RespuestaPaginada<Legajo>> Get(Paginacion paginacion)
        {
            var queryable = context.Legajos
                .Include(s => s.Sector)
                .Include(s => s.Categoria)
                .AsQueryable();
            var respuestaPaginada = new RespuestaPaginada<Legajo>();
            respuestaPaginada.TotalPaginas = await queryable.CalcularTotalPaginas(paginacion.CantidadRegistros);
            respuestaPaginada.Registros = await queryable.AsNoTracking().Paginar(paginacion).ToListAsync();
            return respuestaPaginada;
        }

               
        
        public async Task<List<Legajo>> Get()
        {

            return await context.Legajos
                .Include(s => s.Sector)
                .Include(s => s.Categoria)
                .AsNoTracking().ToListAsync();
        }
        public async Task<Legajo> Get(int id)
        {


            return await context.Legajos
               .Where(x => x.Id == id)
            .Include(x => x.LegajoSucursal).ThenInclude(x => x.Sucursal)
            .Include(x => x.Sector)
            .Include(x => x.Categoria)
            .AsNoTracking().FirstOrDefaultAsync();
        }

        public Legajo GetLegajo(int id)
        {

            return context.Legajos
            .Where(x => x.Id == id)
            .Include(x => x.LegajoSucursal).ThenInclude(x => x.Sucursal)
            .Include(x => x.LegajoHuella).ThenInclude(x => x.Dedo)
            .Include(x => x.Sector)
            .Include(x => x.Categoria)
            .Include(x => x.Horario).ThenInclude(x => x.HorariosDias)
            .AsNoTracking().FirstOrDefault();
        }

        public LegajoActualiacionDTO PutGet(int id)
        {
            var legajo =  GetLegajo(id);

            

            var model = new LegajoVisualizarDTO();
            model.Legajo = legajo;
            model.Sucursales = legajo.LegajoSucursal.Select(x => x.Sucursal).ToList();
            if (model.Sucursales is null)
                model.Sucursales = new List<Sucursal>();

            model.Dedos = legajo.LegajoHuella.Select(x => x.Dedo).ToList();
            

            var legajoVisualizarDTO = model;
            var sucursalesSeleccionadasIds = legajoVisualizarDTO.Sucursales.Select(x => x.Id).ToList();
            var sucursalesNoSeleccionadas = context.Sucursales
                .Where(x => !sucursalesSeleccionadasIds.Contains(x.Id))
                .ToList();

            var dedosSeleccionadosIds = legajoVisualizarDTO.Dedos.Select(x => x.Id).ToList();
            var dedosNoSeleccionados = context.Dedos.Where(x => !dedosSeleccionadosIds.Contains(x.Id)).ToList();
                

            var reultado = new LegajoActualiacionDTO();
            reultado.Legajo = legajoVisualizarDTO.Legajo;
            reultado.SucursalesNoSeleccionadas = sucursalesNoSeleccionadas;
            reultado.SucursalesSeleccionadas = legajoVisualizarDTO.Sucursales;
            reultado.Sucursales = legajoVisualizarDTO.Sucursales;

            reultado.DedosNoSeleccionados = dedosNoSeleccionados;
            reultado.DedosSeleccionados = legajoVisualizarDTO.Dedos;
            reultado.Dedos = legajoVisualizarDTO.Dedos; 
            
            return reultado;

        }

        public async Task<List<Legajo>> Get(string texto)
        {
            if (texto is null || texto == string.Empty)
                return await context.Legajos
                .Include(s => s.Sector)
                .Include(c => c.Categoria)
                .AsNoTracking().ToListAsync();

            else
                return await context.Legajos
                .Include(s => s.Sector)
                .Include(c => c.Categoria)
                .Where(x => x.Nombre.Contains(texto))
                .AsNoTracking().ToListAsync();
        }

        //public async Task<List<Legajo>> Get(string textoBusqueda)
        //{
        //    if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<Legajo>(); }
        //    textoBusqueda = textoBusqueda.ToLower();
        //    return await context.Legajos
        //        .Where(x => x.Nombre.ToLower().Contains(textoBusqueda)).AsNoTracking().ToListAsync();
        //}

        public async Task<int> Post(Legajo persona)
        {
            context.Add(persona);
            await context.SaveChangesAsync();
            return persona.Id;
        }

        public async Task Actualizar(Legajo legajo)
        {

            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@"UPDATE Legajos 
                    SET Nombre = @Nombre, SucursalId = @SucursalId,
                    SectorId = @SectorId, CategoriaId = @CategoriaId, 
                    Activo = @Activo WHERE Id = @Id", legajo);
        }

        public async Task Put(Legajo persona)
        {
            

            context.Attach(persona).State = EntityState.Modified;
            await context.SaveChangesAsync();

            // la mierda de arriba solo actualiza el legajo asi que voy a hacer un update a mano para actualizar las tablas relacionadas
            // legajoSucursal
            // primero las borro, despues recorro la lista e inserto

            int legajo = persona.Id;

            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"delete LegajosSucursales
                     WHERE LegajoId = @legajo", new { legajo });


            int sucursal;
            // ahora recorro la lista de las sucursales seleccionadas
            foreach (var item in persona.LegajoSucursal)
            {
                sucursal = item.SucursalId;

                await connection.ExecuteAsync(@"insert into LegajosSucursales (SucursalId, LegajoId) Values (@sucursal, @legajo)", new { sucursal, legajo });

            }
        }

        public async Task Delete(int id)
        {
            var existe = await context.Legajos.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Legajo {id} no encontrada"); }
            context.Remove(new Legajo { Id = id });
            await context.SaveChangesAsync();
        }

        public async Task<RespuestaPaginada<Legajo>> Get(ParametrosBusquedaLegajos parametrosBusqueda)
        {
            var legajosQueryable = context.Legajos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parametrosBusqueda.Nombre))
            {
                legajosQueryable = legajosQueryable
                    .Where(x => x.Nombre.ToLower().Contains(parametrosBusqueda.Nombre.ToLower()))
                    .Where(x => x.Activo);
            }

            if (parametrosBusqueda.SectorId != 0)
            {
                legajosQueryable = legajosQueryable
                    .Where(x => x.SectorId == parametrosBusqueda.SectorId);

            }

            var respuesta = new RespuestaPaginada<Legajo>();
            respuesta.TotalPaginas = await legajosQueryable.CalcularTotalPaginas(parametrosBusqueda.CantidadRegistros);
            respuesta.Registros = await legajosQueryable.AsNoTracking().Paginar(parametrosBusqueda.Paginacion).ToListAsync();
            return respuesta;
        }
        public async Task BorrarLegajoSucursal(int id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@$"Delete LegajosSucursales
                    WHERE LegajoId = {id}", new {id});
        }

        public List<Dedo> GetDedos()
        {
            return context.Dedos.AsNoTracking().ToList();

        }

        public async Task<RespuestaPaginada<Legajo>> GetLegajoSegunUsuario(ParametrosBusquedaLegajos parametrosBusqueda)
        {
            using var connection = new SqlConnection(connectionString);

            /////////////////////////////////////////////////////////////////
            string usuario = parametrosBusqueda.UsuarioId;
            string texto = parametrosBusqueda.Nombre;
            int activo = Convert.ToInt16(parametrosBusqueda.Activo);

            IEnumerable<Legajo> Listado = await connection.QueryAsync<Legajo>($@"select distinct c.* 
                from UsuariosSucursales a
                -- traigo los legajos de las sucursales que tiene acceso el usuario
                inner join LegajosSucursales b on a.SucursalId = b.SucursalId
                inner join Legajos c on b.LegajoId = c.Id
                inner join Sucursales d on d.Id = b.SucursalId
                left join Sectores e on c.SectorId = e.Id
                left join Categorias f on c.CategoriaId = f.Id
                where a.UsuarioId = @usuario and 
                    ((c.Nombre like '%' + @texto + '%')	
				                or (e.Nombre like '%' + @texto + '%') 
				                or (f.Nombre like '%' + @texto + '%')
				                or (c.LegajoId like '%' + @texto + '%'))
                --and c.Activo = @activo
                order by 2", new { usuario, texto, activo});

            var legajosQueryable = Listado.AsQueryable();
            //var legajosQueryable = context.Legajos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parametrosBusqueda.Nombre))
            {
                legajosQueryable = legajosQueryable
                    .Where(x => x.Nombre.ToLower().Contains(parametrosBusqueda.Nombre.ToLower()))
                    .Where(x => x.Activo);
            }

            if (parametrosBusqueda.SectorId != 0)
            {
                legajosQueryable = legajosQueryable
                    .Where(x => x.SectorId == parametrosBusqueda.SectorId);

            }

            var respuesta = new RespuestaPaginada<Legajo>();
            respuesta.TotalPaginas = await legajosQueryable.CalcularTotalPaginas(parametrosBusqueda.CantidadRegistros);
            respuesta.Registros =  legajosQueryable.AsNoTracking().Paginar(parametrosBusqueda.Paginacion).ToList();
            return respuesta;


        }

        public async Task<List<Legajo>> GetLegajoSegunUsuario(string usuario, string texto)
        {
            using var connection = new SqlConnection(connectionString);


            //Gustavo 23/03/2023

            //---Estos son los legajos de las sucursales a las que tiene acceso el usuario
            //-- Traigo las sucursales a las que tiene acceso el usuario
            //--el distinct es para que filtre el legajo que tiene mas de una sucursal asignada
            IEnumerable<Legajo> Listado = await connection.QueryAsync<Legajo>($@"select distinct c.* 
                from UsuariosSucursales a
                -- traigo los legajos de las sucursales que tiene acceso el usuario
                inner join LegajosSucursales b on a.SucursalId = b.SucursalId
                inner join Legajos c on b.LegajoId = c.Id
                inner join Sucursales d on d.Id = b.SucursalId
                left join Sectores e on c.SectorId = e.Id
                left join Categorias f on c.CategoriaId = f.Id
                where a.UsuarioId = @usuario and 
                    ((c.Nombre like '%' + @texto + '%')	
				                or (e.Nombre like '%' + @texto + '%') 
				                or (f.Nombre like '%' + @texto + '%')
				                or (c.LegajoId like '%' + @texto + '%'))
                and c.Activo = 1
                order by 2", new { usuario, texto });


            //antes de devolver el listado tengo que llenar el Sector
            List<Sector> sectores = this.repositorioSectores.GetLista();
            List<Categoria> categorias = this.repositorioCategorias.GetLista();
            foreach (var item in Listado)
            {
                item.Sector = sectores.Where(x => x.Id == item.SectorId).FirstOrDefault();
                item.Categoria = categorias.Where(x => x.Id == item.CategoriaId).FirstOrDefault();
            }

            return Listado.ToList();
        }

        public async Task<List<DashBoardMinutosTrabajadosPorDiaPorLegajo>> MinutosMensualesTrabajadosPorLegajo(int Id)
        {
            
            using var connection = new SqlConnection(connectionString);
            var Listado = await connection.QueryAsync<DashBoardMinutosTrabajadosPorDiaPorLegajo>
                (@$"Select a.LegajoId, a.Nombre, b.FechaDesde, c.FechaHasta, b.Nombre NombreSucursal,
					DATEDIFF(MINUTE, b.FechaDesde, c.FechaHasta) AS Minutos
					from Legajos a
					inner join 
					(select f1.LegajoId, Format(f1.Registro,'d') Registro , min(f1.Registro) fechaDesde, s1.Nombre 
						from fichadas f1
							inner join sucursales s1 on f1.SucursalId = s1.Id
						where month(f1.Registro) = month(getdate()) and year(f1.registro) = year(getdate()) 
						and f1.LegajoId = @Id
						group by f1.LegajoId, Format(Registro,'d'), s1.Nombre )  b on a.id = b.LegajoId
					inner join 
					(select f.LegajoId, Format(f.Registro,'d') Registro , max(f.Registro) fechaHasta, s1.Nombre 
						from fichadas f
							inner join sucursales s1 on f.SucursalId = s1.Id
						where month(f.Registro) = month(getdate()) and year(f.registro) = year(getdate()) 
						and f.LegajoId = @Id 
						group by f.LegajoId, Format(f.Registro,'d'), s1.Nombre ) c on a.id = c.LegajoId and Format(b.fechaDesde,'d') = Format(c.fechaHasta,'d')
					where a.Id = @Id", new { @Id });

            return Listado.ToList();
        }

        public async Task<List<DashBoardMinutosMensualesCalendarioPorLegajo>> MinutosMensualesdelCalendarioDelLegajo(int LegajoId)
        {
            using var connection = new SqlConnection(connectionString);
            var Listado = await connection.QueryAsync<DashBoardMinutosMensualesCalendarioPorLegajo>
                ($@"exec WebCalculoMinutosMensualesCalendarioPorLegajo {@LegajoId}",new { @LegajoId});

            return Listado.ToList();
        }
        public async Task<List<DashBoardBusquedaDTO>> BusquedaGeneral(string texto)
        {
            using var connection = new SqlConnection(connectionString);

            IEnumerable<DashBoardBusquedaDTO> Lista = await connection
                    .QueryAsync<DashBoardBusquedaDTO>(@$"select a.Id,  a.Nombre, a.LegajoId, 
                                b.Nombre sector, c.Nombre Categoria
                            from Legajos a 
	                            inner join Sectores b on a.SectorId = b.Id
	                            inner join Categorias c on a.CategoriaId = c.Id
	                            inner join Horarios d on a.HorarioId = d.Id
                            where  a.Nombre like '%' + @texto + '%' ", new { texto });

            return Lista.ToList();
        }
    }
}
