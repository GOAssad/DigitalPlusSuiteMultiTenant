# Reporte de Arquitectura — DigitalPlus Web (Blazor Server)

**Fecha:** 2026-03-01
**Objetivo:** Analisis completo de la aplicacion web para evaluar la viabilidad de evolucionarla a un portal multi-tenant (una sola instancia, multiples empresas con bases de datos independientes).

---

## 1. Descripcion General

| Atributo | Valor |
|---|---|
| **Framework** | Blazor Server (.NET 7.0) |
| **Solucion** | Un solo proyecto (`DigitalPlus.sln`) |
| **ORM** | Entity Framework Core 7.0.4 + Dapper 2.1.35 (hibrido) |
| **Autenticacion** | ASP.NET Identity (IdentityUser estandar) |
| **Base de datos** | SQL Server (Ferozo produccion) |
| **Real-time** | Azure SignalR Service |
| **Exportacion** | EPPlus (Excel) |
| **UI** | Bootstrap + Blazored.Typeahead + QuickGrid (alpha) |

La aplicacion lee datos de la base DigitalPlus que es alimentada por las apps de escritorio (Fichador y Administrador). Actualmente conecta a una unica base de datos hardcodeada en `appsettings.json`.

---

## 2. Estructura del Proyecto

```
DigitalPlus/
├── Areas/Identity/Pages/Account/     Login, Register, Logout, ResetPassword
├── Componentes/                      14 componentes reutilizables (Dialogo, FechaDesdeHasta, etc.)
├── Data/                             ApplicationDbContext (EF Core)
├── DTOs/                             40+ Data Transfer Objects
├── Entidades/                        20 entidades de dominio
│   └── Configuraciones/              17 archivos IEntityTypeConfiguration (Fluent API)
├── Helpers/                          Utilidades (Excel, cadenas, JS interop, paginacion)
├── Migrations/                       2 migraciones EF Core
├── Pages/                            ~90 paginas Razor (CRUD completo)
│   ├── Legajos/                      ABM empleados
│   ├── Fichadas/                     Consolidado, Control, Extras, Tarde, Ausencias
│   ├── Horarios/                     Gestion de horarios y calendarios
│   ├── Categorias, Sectores, Sucursales, Terminales, Incidencias, Feriados, Vacaciones
│   ├── Usuarios/                     Gestion de usuarios y roles
│   ├── Noticias/                     Noticias internas
│   └── FichaTelas/                   Modulo de productos (Ficha de Telas)
├── Repositorios/                     14 repositorios (capa de datos)
├── Servicios/                        Servicios de calendario, modales, migracion
├── Shared/                           MainLayout, NavMenu, Paginacion, ListadoGenerico
└── wwwroot/                          Archivos estaticos
```

---

## 3. Capa de Autenticacion y Autorizacion

### Configuracion actual

- Usa `IdentityUser` estandar (sin propiedades custom)
- Lockout: 5 min tras 5 intentos fallidos
- Paginas scaffoldeadas: Login, Register, Logout, ResetPassword, Manage

### Roles definidos

| Rol | Acceso |
|---|---|
| `ADMINISTRADOR` | Acceso completo: Atributos, Usuarios, Noticias + todo lo demas |
| `Supervisor_Local` | Legajos, Fichadas |
| `Admin_Telas` | Modulo de Productos (Ficha de Telas) |

### Autorizacion en UI (NavMenu.razor)

```html
<AuthorizeView Roles="ADMINISTRADOR">          <!-- Solo admin -->
<AuthorizeView Roles="ADMINISTRADOR, Supervisor_Local">  <!-- Admin + supervisor -->
<AuthorizeView Roles="ADMINISTRADOR, Admin_Telas">       <!-- Admin + telas -->
```

### Integracion con apps de escritorio

- La tabla `GRALUsuarios` almacena credenciales del sistema de escritorio
- El login web tiene codigo comentado para sincronizar con escritorio (`ActualizarUsuarioEscritorio`)
- Las credenciales de escritorio se encriptan en Base64

---

## 4. Capa de Datos

### DbContext

- `ApplicationDbContext` hereda de `IdentityDbContext`
- 23 DbSets (Legajos, Fichadas, Horarios, Sucursales, Sectores, etc.)
- Configuracion via Fluent API en 17 archivos `IEntityTypeConfiguration`
- Registrado como **DbContextFactory Transient** en DI

### Connection Strings (appsettings.json)

| Nombre | Destino | Usado por |
|---|---|---|
| `DefaultConnection` | Ferozo / DigitalPlus (produccion) | **Program.cs** (principal) |
| `DefaultConnectionAdmin` | Ferozo / DigitalPlus (user GASSAD) | No usado activamente |
| `DefaultConnectionFichas` | 192.168.0.11 / PRD01 | RepositorioProductos |
| `DefaultConnectionAcceso` | 192.168.0.11 / Accesos | RepositorioProductos |
| `SignalR` | Azure SignalR | builder.Services.AddAzureSignalR() |
| `DigitalOne` | Azure / DigitalOne (legacy) | MigrarDigitalOne.cs |

**Problema critico:** La connection string principal esta hardcodeada en `Program.cs` linea 30:
```csharp
string cadena = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(cadena), ServiceLifetime.Transient);
```

### Repositorios (14 en total)

| Repositorio | EF Core | Dapper | Stored Procedures |
|---|---|---|---|
| RepositorioLegajos | Si | Si | WebCalculoMinutosMensualesCalendarioPorLegajo |
| RepositorioFichadas | Si | Si | WebAusencias*, WebControlAcceso*, WebLlegadaTarde*, WebConsolidado* |
| RepositorioProductos | Limitado | Si | - (queries cross-DB a PRD01/Accesos) |
| RepositorioUsuarios | Si | Si | - |
| RepositorioSucursales | Si | Si | - |
| RepositorioHorarios | Si | No | - |
| RepositorioSectores | Si | No | - |
| RepositorioCategorias | Si | No | - |
| RepositorioTerminales | Si | No | - |
| RepositorioIncidencias | Si | No | - |
| RepositorioFeriados | Si | No | - |
| RepositorioVacaciones | Si | No | - |
| RepositorioNoticias | Si | No | - |
| RepositorioVariablesGlobales | Si | No | - |

Todos registrados como **Transient** en DI. Cada repositorio lee la connection string directamente de `IConfiguration` en su constructor.

---

## 5. Modelo de Datos (Entidades)

### Entidades principales

```
Legajo (1) ──── (N) LegajoSucursal ──── (1) Sucursal (1) ←── (N) UsuarioSucursal → IdentityUser
   │                                          │
   ├── Sector                                 └── Terminal
   ├── Categoria
   ├── Horario → HorarioDia → Dia, HorarioDiaEvento
   ├── (N) LegajoHuella → Dedo
   ├── (N) Fichada → Sucursal
   └── (N) IncidenciaLegajo → Incidencia

Independientes: Feriado, Vacacion, Noticia, VariableGlobal, GRALUsuario
```

### Mapeo con DigitalPlus (schema compartido con escritorio)

| Entidad Web | Tabla SQL | Entidad Escritorio (DigitalOne) |
|---|---|---|
| Legajo | Legajos | RRHHLegajos |
| Fichada | Fichadas | RRHHFichadas |
| LegajoHuella | LegajosHuellas | RRHHLegajosHuellas |
| Horario | Horarios | RRHHHorarios |
| Categoria | Categorias | RRHHCategorias |
| Sector | Sectores | RRHHUbicaciones |
| Sucursal | Sucursales | GralSucursales |
| Terminal | Terminales | GRALTerminales |
| Incidencia | Incidencias | RRHHIncidencias |
| IncidenciaLegajo | IncidenciasLegajos | RRHHIncidenciasLegajos |
| VariableGlobal | VariablesGlobales | GRALVariablesGlobales |
| GRALUsuario | GRALUsuarios | GRALUsuarios |

---

## 6. Que esta bien

1. **Separacion de capas clara:** Entidades, DTOs, Repositorios, Componentes, Pages
2. **Patron repositorio consistente:** 14 repositorios con responsabilidades bien definidas
3. **Configuracion EF Core via Fluent API:** 17 archivos de configuracion en lugar de atributos
4. **DbContextFactory Transient:** Correcto para Blazor Server (evita problemas de concurrencia)
5. **Roles y autorizacion funcional:** 3 roles con AuthorizeView en la UI
6. **Stored Procedures para reportes pesados:** Queries complejas delegadas a SQL Server
7. **Componentes reutilizables:** Dialogo, FechaDesdeHasta, SelectorMultipleTypeahead, etc.
8. **Patrón hibrido EF+Dapper:** EF para CRUD simple, Dapper para queries complejas

---

## 7. Que esta mal / Riesgos

### CRITICOS

| # | Problema | Ubicacion | Impacto |
|---|---|---|---|
| 1 | **Connection string hardcodeada** | Program.cs:30 | Imposible multi-tenant |
| 2 | **Sin TenantId en Identity** | IdentityUser estandar | No hay forma de asociar usuario a empresa |
| 3 | **Repositorios leen connection string en constructor** | Todos los repos | Todos apuntan a la misma BD siempre |
| 4 | **Sin tests automatizados** | Todo el proyecto | Refactoring peligroso sin red de seguridad |
| 5 | **SQL injection en RepositorioProductos** | Linea ~97-101 | `" and c.SGMCOD = '" + idgrupo + "'"` — concatenacion directa |

### IMPORTANTES

| # | Problema | Ubicacion | Impacto |
|---|---|---|---|
| 6 | Credenciales en appsettings.json | appsettings.json | Passwords visibles en codigo |
| 7 | 6 connection strings sin documentar | appsettings.json | Confusion sobre cual se usa |
| 8 | No hay Unit of Work pattern | Repositorios | Sin coordinacion de transacciones |
| 9 | No hay error boundaries en Razor | Components | Errores no controlados crashean la pagina |
| 10 | Datos biometricos sin encriptar | LegajosHuellas | Riesgo legal si hay breach |
| 11 | Links externos hardcodeados (Kosiuko, etc.) | NavMenu.razor | Especificos de un cliente |
| 12 | .NET 7.0 (fin de soporte Nov 2024) | DigitalPlus.csproj | Vulnerabilidades sin parche |
| 13 | QuickGrid v0.1.0-alpha | DigitalPlus.csproj | Paquete en alpha, inestable |
| 14 | Codigo de migracion DigitalOne activo | MigrarDigitalOne.cs | Logica legacy que deberia eliminarse |

---

## 8. Evaluacion Multi-Tenant

### Estrategia recomendada: Database-per-Tenant (Base de datos por empresa)

Dado que el modelo actual ya funciona con una BD por empresa (las apps de escritorio instalan una BD por cliente), la estrategia mas natural es:

```
                    ┌─────────────────────────────────┐
                    │     DigitalPlus Web (unico)      │
                    │     Portal Multi-Tenant           │
                    └──────────┬──────────┬────────────┘
                               │          │
                    ┌──────────▼──┐  ┌────▼───────────┐
                    │  BD Maestra  │  │  BD Maestra     │
                    │  (Identity + │  │  (Tenants +     │
                    │   Tenants)   │  │   Conn Strings) │
                    └──────────┬──┘  └────┬────────────┘
                               │          │
              ┌────────────────┼──────────┼────────────────┐
              │                │          │                 │
     ┌────────▼────┐  ┌───────▼────┐  ┌──▼──────────┐  ┌──▼──────────┐
     │ Empresa_A   │  │ Empresa_B  │  │ Empresa_C   │  │ Empresa_N   │
     │ DigitalPlus │  │ DigitalPlus│  │ DigitalPlus │  │ DigitalPlus │
     │ (Ferozo)    │  │ (Azure)    │  │ (Local)     │  │ (...)       │
     └─────────────┘  └────────────┘  └─────────────┘  └─────────────┘
```

**Ventajas de esta estrategia:**
- Maximo aislamiento de datos entre empresas
- Compatible con el modelo actual (cada instalacion de escritorio ya tiene su propia BD)
- No requiere agregar TenantId a las 20 entidades existentes
- No requiere modificar los stored procedures
- No requiere reescribir queries Dapper con filtros de tenant

### Componentes necesarios

| Componente | Descripcion | Estado actual |
|---|---|---|
| **Tabla Tenants** | Empresa, connection string, estado, fecha alta | No existe |
| **ApplicationUser** | Extiende IdentityUser con TenantId | No existe (usa IdentityUser estandar) |
| **BD Maestra** | Almacena Identity + Tenants (separada de datos) | No existe |
| **ITenantService** | Resuelve tenant del usuario logueado | No existe |
| **TenantAwareDbContextFactory** | Crea DbContext con connection string dinamica | No existe |
| **Login con tenant** | El login resuelve a que empresa pertenece el usuario | No existe |
| **Middleware de tenant** | Setea el tenant en cada request | No existe |

### Lo que hay que cambiar (Database-per-Tenant)

| Capa | Cambio | Esfuerzo |
|---|---|---|
| **Identity** | Crear ApplicationUser con TenantId, migrar BD maestra | 1 semana |
| **Program.cs** | Reemplazar DbContextFactory estatico por dinamico | 2-3 dias |
| **Repositorios** | Inyectar ITenantService en vez de leer IConfiguration | 1 semana |
| **Dapper queries** | Usar connection string del tenant (no de appsettings) | 3-4 dias |
| **Login** | Resolver tenant al autenticar | 2-3 dias |
| **NavMenu** | Eliminar links hardcodeados, hacerlos configurables | 1 dia |
| **Tests** | Crear suite basica de tests | 2 semanas |

### Lo que NO hay que cambiar

- Entidades (no necesitan TenantId)
- Stored procedures (cada BD tiene los suyos)
- Componentes UI (siguen igual)
- Logica de autorizacion por sucursal (sigue igual dentro de cada BD)

---

## 9. Resumen de Recomendaciones

### Prioridad ALTA (hacer antes de multi-tenant)

1. **Actualizar a .NET 8.0 LTS** — .NET 7 ya no tiene soporte
2. **Corregir SQL injection** en RepositorioProductos (linea ~97)
3. **Sacar credenciales de appsettings.json** — usar User Secrets o Key Vault
4. **Eliminar codigo legacy** — MigrarDigitalOne.cs, WeatherForecast*, Counter.razor, FetchData.razor
5. **Eliminar links hardcodeados** de NavMenu.razor (Kosiuko, Herencia, Casa Chic)

### Prioridad MEDIA (hacer durante multi-tenant)

6. **Crear suite de tests** antes de refactorizar
7. **Implementar error boundaries** en componentes Blazor
8. **Documentar connection strings** — cual se usa donde y para que
9. **Reemplazar QuickGrid alpha** por version estable o alternativa
10. **Implementar logging centralizado** (Serilog o similar)

### Prioridad BAJA (mejoras futuras)

11. Implementar Unit of Work pattern para transacciones coordinadas
12. Agregar cache de connection strings de tenant (Redis o MemoryCache)
13. Encriptar datos biometricos en reposo
14. Agregar auditoría de acciones (quien hizo que, cuando)

---

## 10. Estimacion de Esfuerzo Total

| Fase | Duracion | Equipo |
|---|---|---|
| **Fase 0:** Limpieza y actualizacion .NET 8 | 1-2 semanas | 1 dev |
| **Fase 1:** Infraestructura multi-tenant (BD maestra, ITenantService, login) | 2-3 semanas | 1-2 devs |
| **Fase 2:** Refactor repositorios y DbContext dinamico | 2 semanas | 1-2 devs |
| **Fase 3:** Testing y QA | 2 semanas | 1-2 devs |
| **Fase 4:** Deploy y onboarding primer tenant | 1 semana | 1 dev |
| **Total** | **8-10 semanas** | **1-2 desarrolladores** |

Esta estimacion asume la estrategia **Database-per-Tenant** (la mas limpia para este caso). Si se optara por row-level security (TenantId en cada tabla), el esfuerzo subiria a 12-16 semanas por la cantidad de queries Dapper a modificar.

---

*Reporte generado por analisis automatizado del codigo fuente completo.*
