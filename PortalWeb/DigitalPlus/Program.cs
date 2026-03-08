global using DigitalPlus.Helpers;
global using DigitalPlus.Entidades;

using DigitalPlus.Areas.Identity;
using DigitalPlus.Data;
using DigitalPlus.Repositorios;
using DigitalPlus.Servicios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using DigitalPlus.Servicios.Modal;

var builder = WebApplication.CreateBuilder(args);

///////


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddLocalization();

//------------------------------------------------------------------------------
//Entity Framework - Sistema
string cadena = builder.Configuration.GetConnectionString("DefaultConnection");
// string cadena = builder.Configuration.GetConnectionString("DigitalOne");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options => 
    options.UseSqlServer(cadena), ServiceLifetime.Transient);



builder.Services.AddIdentity<IdentityUser, IdentityRole>(
   option =>
   {
       option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
       option.Lockout.MaxFailedAccessAttempts = 5;
       option.Lockout.AllowedForNewUsers = false;
   })
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders()
  .AddDefaultUI();


builder.Services.AddScoped<TokenProvider>();
//------------------------------------------------------------------------------
// Servicios
builder.Services.AddScoped<IMostrarMensajes, MostrarMensajes>();
builder.Services.AddScoped<ListadoVariablesGlobales>();
builder.Services.AddTransient<RepositorioSectores>();
builder.Services.AddTransient<RepositorioLegajos>();
builder.Services.AddTransient<RepositorioProductos>();
builder.Services.AddTransient<RepositorioSucursales>();
builder.Services.AddTransient<RepositorioHorarios>();
builder.Services.AddTransient<RepositorioTerminales>();
builder.Services.AddTransient<RepositorioIncidencias>();
builder.Services.AddTransient<RepositorioFeriados>();
builder.Services.AddTransient<RepositorioFichadas>();
builder.Services.AddTransient<RepositorioCategorias>();
builder.Services.AddTransient<RepositorioUsuarios>();
builder.Services.AddTransient<RepositorioVacaciones>();
builder.Services.AddTransient<RepositorioNoticias>();
builder.Services.AddTransient<RepositorioVariablesGlobales>();

builder.Services.AddTransient<MigrarDigitalOne>();

//--------------------------------------------------------------------
//Formulario Modal
builder.Services.AddScoped<IModalService, ModalService>();

//---------------------------------------------------------------------
//Calendario
builder.Services.AddScoped<IDayEventService, DayEventService>();
builder.Services.AddScoped<IHorarioDiaEventoServicio, HorarioDiaEventoServicio>();

//------------------------------------------------------------------------------
// SignalIR
builder.Services.AddSignalRCore().AddAzureSignalR(builder.Configuration.GetConnectionString("SignalR"));

//----------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Gustavo para ver si esto me permite usar el debug mejor
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
//------------------------------------------------------------------
//Autenticacion
app.UseAuthentication();
//Autorizacion
app.UseAuthorization();
//------------------------------------------------------------------

//gustavo 24/10/2022
app.MapRazorPages();
//---------------------------------------

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// -- Cultura para Argentina
app.UseRequestLocalization(new RequestLocalizationOptions()
    .AddSupportedCultures(new[] { "en-US", "es-AR" })
    .AddSupportedUICultures(new[] { "en-US", "es-AR" }));

app.Run();



