using Dapper;
using DigitalPlus.Data;
using DigitalPlus.DTOs;
using DigitalPlus.Entidades;
using DigitalPlus.Entidades.DigitalOnePlusEscritorio;
using DigitalPlus.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Drawing.Text;
using System.Security.Claims;

namespace DigitalPlus.Repositorios
{
    public class RepositorioUsuarios
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly string connectionString;

        public RepositorioUsuarios(ApplicationDbContext context,
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                SignInManager<IdentityUser> signInManager,
                IConfiguration configuration)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            //connectionString = configuration.GetConnectionString("DigitalOne");
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<UsuarioDTO> UsuarioLogeado(string usuario)
        {
            return await context.Users
                .Where(x => x.UserName == usuario)
                .Select(x => new UsuarioDTO { Email = x.Email, UserId = x.Id }).FirstOrDefaultAsync();


        }

        public async Task<RespuestaPaginada<UsuarioDTO>> Get(Paginacion paginacion)
        {
            var queryable = context.Users.AsQueryable();
            var respuesta = new RespuestaPaginada<UsuarioDTO>();
            respuesta.TotalPaginas = await queryable.CalcularTotalPaginas(paginacion.CantidadRegistros);
            respuesta.Registros = await queryable.Paginar(paginacion)
                 .Select(x => new UsuarioDTO { UserName = x.UserName,  Email = x.Email, UserId = x.Id }).ToListAsync();
            return respuesta;
        }

        public async Task<List<RolDTO>> GetRoles()
        {
            return await context.Roles
                .Select(x => new RolDTO { Nombre = x.Name, RoleId = x.Id }).ToListAsync();
        }

        //22/11/2022 los calaims estan relacionado con los usuarios, 
        // me parece que en este metodo y en todos los que tienen claims
        // hay que pasar el usuario
        public async Task<List<RolDTO>> GetClaims()
        {
            return await context.Roles
                .Select(x => new RolDTO { Nombre = x.Name, RoleId = x.Id }).ToListAsync();
        }

        public async Task PostRol(IdentityRole rol)
        {
            await roleManager.CreateAsync(rol);
        }

        //21/11/2022 los claims estan asociados a los usuarios, tengo que ver para que los puedo usar
        // asi que el alta de claims tiene que estar en el editar de usuario
        // Creo que tiene que ser la sucursal, por ejemplo
        //de esta manera puedo dar con los prmisos y los combos
        //segun el usuario logeado cuales son las sucursales que le muestro
        //tanto para consulta de fichadas como para asignarle a los legajos
        public async Task PostClaim(IdentityUser user, Claim claim)
        {
            await userManager.AddClaimAsync(user, claim);
        }
        public async Task Post(IdentityUser user, string pass)
        {
            await userManager.CreateAsync(user, pass);
        }


        public async Task<List<UsuarioSucursal>> GetUsuarioSucursal(string UsuarioId)
        {
            return await context.UsuariosSucursales
                .Include(x => x.Sucursal)
                .Where(x => x.UsuarioId == UsuarioId).ToListAsync();
        }

        public async Task<IdentityUser> Get(string id)
        {
            return await context.Users.Select(x => new IdentityUser
            { Email = x.Email, UserName = x.UserName, Id = x.Id })
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task AsignarRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserId);
            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleId));
            await userManager.AddToRoleAsync(usuario, editarRolDTO.RoleId);
        }

        public async Task RemoverUsuarioRol(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserId);
            await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleId));
            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.RoleId);
        }

        public async Task<List<UsuarioRolesDTO>> GetRolesPorUsuario(string UsuarioId)
        {

            using var connection = new SqlConnection(connectionString);
            var lista = await connection.QueryAsync<UsuarioRolesDTO>(@$"
                select b.Id UserId, b.Email NombreUsuario,
                    c.Id RolId, c.Name NombreRol
                    from AspnetUserRoles a
                    inner join AspNetUsers b on a.UserId = b.Id
                    inner join AspNetRoles c on a.RoleId = c.Id
                    where a.UserId = @UsuarioId", new { UsuarioId });


            return lista.ToList();
        }


        public async Task<bool> GetTieneEsteRol(string UsuarioId, string role)
        {

            using var connection = new SqlConnection(connectionString);
            var lista = await connection.QueryAsync<UsuarioRolesDTO>(@$"
                select b.Id UserId, b.Email NombreUsuario,
                    c.Id RolId, c.Name NombreRol
                    from AspnetUserRoles a
                    inner join AspNetUsers b on a.UserId = b.Id
                    inner join AspNetRoles c on a.RoleId = c.Id
                    where a.UserId = @UsuarioId and c.Name = @role", new { UsuarioId, role });


            return lista.Count() > 0;
        }

        public async Task GuardarUsuarioSucursales(string usuario, List<int> modelo)
        {
            using var connection = new SqlConnection(connectionString);
            // primero borro todas las sucursales del usuario
            await connection.ExecuteAsync($@"Delete UsuariosSucursales where UsuarioId = @usuario", new { @usuario });

            //ahora recorro la lista y guardo cada uno de las Sucursales
            int sucursal;
            foreach (var item in modelo)
            {
                sucursal = item;

                await connection.ExecuteAsync($@"insert Into UsuariosSucursales (UsuarioId, SucursalId) 
                        Values (@usuario, @sucursal)", new { @usuario, @sucursal });
            }

        }

        //buscar si el usuario del escritorio existe, si exste lo borro y lo inserto y sino lo inserto
        public async Task ActualizarUsuarioEscritorio(GRALUsuario gralUsuario)
        {
            string email = gralUsuario.Email;
            using var connection = new SqlConnection(connectionString);

            // lo borro
            await connection.ExecuteAsync($@"Delete GRALUsuarios where Email = @email", new { @email });

            //comprimo el password
            gralUsuario.Password = EncriptarString(gralUsuario.Password);

            //inserto
            string pass = gralUsuario.Password;
            await connection.ExecuteAsync($@"insert Into GRALUsuarios (eMail, Password) 
                        Values (@email, @pass)", new { @email, @pass });

        }
        private string EncriptarString(string _cadenaAencriptar)
        {
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            return Convert.ToBase64String(encryted);
            
        }

    }
}
