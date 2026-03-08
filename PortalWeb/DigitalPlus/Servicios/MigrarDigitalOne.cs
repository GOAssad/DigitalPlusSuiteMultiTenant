using Dapper;
using DigitalPlus.Data;
using DigitalPlus.DTOs;
using DigitalPlus.Entidades;
using DigitalPlus.Repositorios;
using Microsoft.Data.SqlClient;
using System.Data;


namespace DigitalPlus.Servicios
{
    public class MigrarDigitalOne

    {
        private readonly ApplicationDbContext context;
        private readonly RepositorioCategorias repositorioCategorias;
        private readonly RepositorioSectores repositorioSectores;
        private readonly RepositorioSucursales repositorioSucursales;
        private readonly RepositorioLegajos repositorioLegajos;
        private readonly RepositorioHorarios repositorioHorarios;
        private readonly string connectionString;
        public string mensaje { get; set; }

        public MigrarDigitalOne(ApplicationDbContext context, IConfiguration configuration,
                RepositorioCategorias repositorioCategorias,
                RepositorioSectores repositorioSectores,
                RepositorioSucursales repositorioSucursales,
                RepositorioLegajos repositorioLegajos,
                RepositorioHorarios repositorioHorarios)
        {
            this.context = context;
            this.repositorioCategorias = repositorioCategorias;
            this.repositorioSectores = repositorioSectores;
            this.repositorioSucursales = repositorioSucursales;
            this.repositorioLegajos = repositorioLegajos;
            this.repositorioHorarios = repositorioHorarios;
            connectionString = configuration.GetConnectionString("DigitalOne"); //Seria Azure
        }
        public void Dias()
        {


            List<Dia> modelo = new List<Dia>();

            modelo.Add(new Dia { Id = 1, Nombre = "Lunes" });
            modelo.Add(new Dia { Id = 2, Nombre = "Martes" });
            modelo.Add(new Dia { Id = 3, Nombre = "Miercoles" });
            modelo.Add(new Dia { Id = 4, Nombre = "Jueves" });
            modelo.Add(new Dia { Id = 5, Nombre = "Viernes" });
            modelo.Add(new Dia { Id = 6, Nombre = "Sabado" });
            modelo.Add(new Dia { Id = 7, Nombre = "Domingo" });

            context.AddRange(modelo);
            context.SaveChanges();
        }
        public void Dedos()
        {


            List<Dedo> modelo = new List<Dedo>();

            modelo.Add(new Dedo { Id = 0, Nombre = "D - Pulgar" });
            modelo.Add(new Dedo { Id = 1, Nombre = "D - Indice" });
            modelo.Add(new Dedo { Id = 2, Nombre = "D - Mayor" });
            modelo.Add(new Dedo { Id = 3, Nombre = "D - Anular" });
            modelo.Add(new Dedo { Id = 4, Nombre = "D - Meñique" });
            modelo.Add(new Dedo { Id = 5, Nombre = "I - Pulgar" });
            modelo.Add(new Dedo { Id = 6, Nombre = "I - Indice" });
            modelo.Add(new Dedo { Id = 7, Nombre = "I - mayor" });
            modelo.Add(new Dedo { Id = 8, Nombre = "I - Anular" });
            modelo.Add(new Dedo { Id = 9, Nombre = "I - Meñique" });


            context.AddRange(modelo);
            context.SaveChanges();
        }
        public void Categorias()
        {

            using var connection = new SqlConnection(connectionString);

            var reader = connection.ExecuteReader(@"Select * from RRHHCategorias");


            List<Categoria> categoria = new List<Categoria>();

            while (reader.Read())
            {
                categoria.Add(new Categoria { Nombre = reader[1].ToString() });
            }

            context.AddRange(categoria);
            context.SaveChanges();
        }
        public void Sectores()
        {

            using var connection = new SqlConnection(connectionString);

            var reader = connection.ExecuteReader(@"Select * from RRHHUbicaciones");


            List<Sector> modelo = new List<Sector>();

            while (reader.Read())
            {
                modelo.Add(new Sector { Nombre = reader[1].ToString() });
            }

            context.AddRange(modelo);
            context.SaveChanges();
        }
        public void Sucursales()
        {
            using var connection = new SqlConnection(connectionString);

            var reader = connection.ExecuteReader(@"Select * from GRALSucursales");

            List<Sucursal> modelo = new List<Sucursal>();

            while (reader.Read())
            {
                modelo.Add(new Sucursal { Nombre = reader[1].ToString(), CodigoSucursal = reader["sSucursalId"].ToString() });
            }

            context.AddRange(modelo);
            context.SaveChanges();
        }
        public async Task Terminales()
        {
            using var connection = new SqlConnection(connectionString);

            var reader = connection.ExecuteReader(@"Select * from GRALTerminales");

            List<Terminal> modelo = new List<Terminal>();
            List<Sucursal> sucursales = await repositorioSucursales.Get();

            while (reader.Read())
            {
                Terminal term = new Terminal();
                term.Nombre = reader["sTerminalID"].ToString();
                term.Descripcion = reader["sDescripcion"].ToString();

                term.SucursalId = sucursales.Where(x => x.CodigoSucursal == reader["sSucursalID"].ToString()).Select(x => x.Id).FirstOrDefault();

                modelo.Add(term);
            }

            context.AddRange(modelo);
            await context.SaveChangesAsync();
        }
        public async Task Legajos()
        {
            using var connection = new SqlConnection(connectionString);

            //IEnumerable<doRRHHLegajoDTO> reader;





            var reader = connection.ExecuteReader(@"Select sLegajoId,  rtrim(ltrim(sApellido)) + ', ' + ltrim(rtrim(sNombre)) as Nombre,  
                            c.sDescripcion sSector , b.sDescripcion sCategoria , lActivo ,
                            a.sHorarioID , d.sDescripcion sSucursal , e.sDescripcion sHorario,  lSeguimiento 
                            from RRHHLegajos a 
	                            inner join RRHHCategorias b on a.nCategoria = b.sCategoriaID
	                            inner join RRHHUbicaciones c on a.nSector = c.sUbicacionID
	                            inner join GRALSucursales d on a.sSucursalID = d.sSucursalID
								left join RRHHHorarios e on a.sHorarioID = e.sHorarioID");


            List<Legajo> modelo = new List<Legajo>();
            List<Categoria> categoria = await repositorioCategorias.Get();
            List<Sector> sectores = await repositorioSectores.Get();
            List<Sucursal> sucursal = await repositorioSucursales.Get();
            List<Horario> horarios = repositorioHorarios.GetLista();


            while (reader.Read())
            {
                Legajo legajo = new Legajo();
                LegajoSucursal legsuc = new LegajoSucursal();

                legajo.Nombre = reader["Nombre"].ToString();
                legajo.LegajoId = reader["sLegajoId"].ToString();
                legajo.Activo = (bool)reader["lActivo"];

                legajo.CategoriaId = categoria.Where(x => x.Nombre == reader["sCategoria"].ToString()).Select(x => x.Id).FirstOrDefault();
                legajo.SectorId = sectores.Where(x => x.Nombre == reader["sSector"].ToString()).Select(x => x.Id).FirstOrDefault();

                if (reader["sHorario"].ToString() is null)
                {
                    legajo.HorarioId = null;
                }
                else
                {
                    legajo.HorarioId = horarios.Where(x => x.Nombre == reader["sHorario"].ToString()).Select(x => x.Id).FirstOrDefault();
                }

                legsuc.SucursalId = sucursal.Where(x => x.Nombre == reader["sSucursal"].ToString()).Select(x => x.Id).FirstOrDefault();
                legsuc.LegajoId = 0;

                legajo.LegajoSucursal.Add(legsuc);

                modelo.Add(legajo);

            }
            context.AddRange(modelo);
            await context.SaveChangesAsync();


            /*
             * Usar esto con Dapper si no podes hacerlo con Entity Framework y listo
             update legajos set HorarioId = (
                select d.Id
                from RRHHLegajos a 
                inner join RRHHHorarios b on a.sHorarioID = b.sHorarioID
                inner join legajos c on a.sLegajoID = c.LegajoId
                inner join horarios d on b.sDescripcion = d.Nombre 
                where Legajos.id = c.Id)
                where Exists (
                select d.Id
                from RRHHLegajos a 
                inner join RRHHHorarios b on a.sHorarioID = b.sHorarioID
                inner join legajos c on a.sLegajoID = c.LegajoId
                inner join horarios d on b.sDescripcion = d.Nombre 
                where Legajos.id = c.Id)

            
             */



        }

        public void Incidencias()
        {

            using var connection = new SqlConnection(connectionString);

            var reader = connection.ExecuteReader(@"Select * from RRHHIncidencias");


            List<Incidencia> modelo = new List<Incidencia>();

            while (reader.Read())
            {
                modelo.Add(new Incidencia { Nombre = reader[1].ToString(), Color = "Black" });
            }

            context.AddRange(modelo);
            context.SaveChanges();
        }

        public async Task Horarios()
        {
            using var connection = new SqlConnection(connectionString);
            var reader = connection.ExecuteReader(@"Select * from RRHHHorarios");

            List<Horario> modelo = new List<Horario>();

            while (reader.Read())
            {
                modelo.Add(new Horario { Nombre = reader["sDescripcion"].ToString() });

            }
            context.AddRange(modelo);
            await context.SaveChangesAsync();
        }

        public async Task Horariosdias()
        {
            using var connection = new SqlConnection(connectionString);
            var reader = connection.ExecuteReader(@"select a.*, c.Id, b.sDescripcion from RRHHHorariosDias a
                    	inner join rrhhhorarios b on a.sHorarioID = b.sHorarioID 
	                    inner join Horarios c on b.sDescripcion = c.Nombre
");

            List<HorarioDia> modelo = new List<HorarioDia>();
            List<Horario> horarios = new List<Horario>();
            horarios = repositorioHorarios.GetLista();


            int abiertocerrado;

            int horarioid = 0;


            while (reader.Read())
            {
                abiertocerrado = 0;

                //busco el horarioid por nombre
                horarioid = horarios.Where(x => x.Nombre == reader["sDescripcion"].ToString()).Select(x => x.Id).FirstOrDefault();

                if ((int)reader["hd"] + (int)reader["hh"] + (int)reader["md"] + (int)reader["mh"] == 0)
                {
                    abiertocerrado = 1;
                }

                if (horarioid > 0)
                {
                    modelo.Add(new HorarioDia
                    {
                        DiaId = (int)reader["nDia"],
                        HoraDesde = (int)reader["hd"],
                        HoraHasta = (int)reader["hh"],
                        MinutoDesde = (int)reader["md"],
                        MinutoHasta = (int)reader["mh"],
                        // HorarioId = int.Parse(reader["sHorarioID"].ToString()),
                        HorarioId =horarioid,
                        Cerrado = abiertocerrado

                    });
                }
                
            }

            context.AddRange(modelo);
            await context.SaveChangesAsync();

        }

        public async Task Huellas()
        {
            using var connection = new SqlConnection(connectionString);
            var reader = connection.ExecuteReader(@"Select * from RRHHLegajosHuellas");

            List<LegajoHuella> modelo = new List<LegajoHuella>();
            List<Legajo> legajos = await repositorioLegajos.Get();

            int legajoId = 0;

            while (reader.Read())
            {

                legajoId = legajos.Where(x => x.LegajoId == reader["sLegajoID"].ToString()).Select(x => x.Id).FirstOrDefault();

                if (legajoId > 0)
                {
                    modelo.Add(new LegajoHuella
                    {
                        DedoId = (int)reader["nDedo"],
                        huella = (byte[])reader["iHuella"],
                        LegajoId = legajoId,
                        nFingerMask = (int)reader["nFingerMask"],
                        sLegajoId = reader["sLegajoid"].ToString(),
                        sLegajoNombre = reader["sLegajoNombre"].ToString()
                    });
                }
            }
            context.AddRange(modelo);
            await context.SaveChangesAsync();
            

        }
        public async Task Fichadas()
        {
            using var connection = new SqlConnection(connectionString);
            var reader = connection.ExecuteReader(@"Select * from RRHHFichadas  where sEntraSale is not null");

            List<Fichada> modelo = new List<Fichada>();
            List<Legajo> legajos = await repositorioLegajos.Get();
            List<Sucursal> sucursales = await repositorioSucursales.Get();
            int legajoId = 0;
            int sucursalId = 0;

            while (reader.Read())
            {
                legajoId = legajos.Where(x => x.LegajoId == reader["sLegajoID"].ToString()).Select(x => x.Id).FirstOrDefault();
                sucursalId = sucursales.Where(x => x.CodigoSucursal == reader["sSucursalID"].ToString()).Select(x => x.Id).FirstOrDefault();

                if (legajoId > 0 && sucursalId > 0)
                {
                    modelo.Add(new Fichada
                    {
                        EntraSale = reader["sEntraSale"].ToString(),
                        Legajoid = legajoId,
                        Registro = (DateTime)reader["dregistro"],
                        SucursalId = sucursalId
                    });
                }
            }
            context.AddRange(modelo);
            await context.SaveChangesAsync();


            /*
             * Tambien podes hacer esto por Dapper
             * 
             * insert into Fichadas (SucursalId, LegajoId, Registro, EntraSale)
                select b.id, c.id, a.dRegistro, isnull(a.sEntraSale,'E') from RRHHFichadas a 
                    inner join sucursales b on a.sSucursalID = b.CodigoSucursal
                    inner join legajos c on a.sLegajoID = c.LegajoId



             */


        }
    }
}
