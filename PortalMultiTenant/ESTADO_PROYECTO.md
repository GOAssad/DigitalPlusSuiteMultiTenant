# Portal Multi-Tenant DigitalPlus - Estado del Proyecto

## Ultimo commit: ffc811a (2026-03-08)

## Stack
- .NET 10 / Blazor Server (Interactive Server) / EF Core 10 / SQL Server
- ASP.NET Identity con roles (SuperAdmin, AdminEmpresa, Operador, Consulta)
- Bootstrap 5 + Bootstrap Icons / Clean Architecture (5 proyectos)

## Como ejecutar
1. Abrir `DigitalPlusMultiTenant.slnx` en Visual Studio 2025
2. F5 o `dotnet run` desde `src\DigitalPlusMultiTenant.Web`
3. Login: admin@kosiuko.com / Admin123

## Funcionalidad Completa
- Dashboard con cards de resumen
- Login en español con branding DigitalPlus
- Register deshabilitado (solo admin crea usuarios)
- 13 paginas CRUD completas:
  - Sucursales, Sectores, Categorias, Horarios, Terminales
  - Legajos (paginacion server-side, filtros, ordenamiento)
  - Fichadas (paginacion, filtro por sucursal/tipo/legajo/fecha)
  - Incidencias, Vacaciones, Feriados, Noticias
  - Usuarios (ABM con roles, cambio contraseña) - solo admin
  - Configuracion (edicion inline VariablesSistema) - solo admin

## Arquitectura Tenant
- Cada pagina inyecta IApplicationDbContext
- Query filters automaticos por EmpresaId en todas las entidades
- TenantService resuelve EmpresaId desde claim (HttpContext o AuthenticationStateProvider)
- ApplicationUser NO tiene query filter (necesario para login)

## Base de Datos
- BD: DigitalPlusMultiTenant (localhost, Trusted_Connection)
- Datos Kosiuko migrados: 758 legajos, 780K fichadas, 30 sucursales
- Script: scripts\migrate_kosiuko.sql

## Proximos Pasos
1. Probar las 6 paginas nuevas en browser
2. Reportes de asistencia (llegadas tarde, ausencias, horas)
3. Exportar a Excel/CSV
4. Dashboard con graficos (Chart.js)
5. Adaptar apps desktop al schema multi-tenant
6. Deploy a produccion
