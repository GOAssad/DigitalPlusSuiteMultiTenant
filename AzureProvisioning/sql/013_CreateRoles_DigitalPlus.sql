-- ============================================================
-- 013_CreateRoles_DigitalPlus.sql
-- Crear rol dp_role_web en DigitalPlus (Ferozo)
--
-- Ejecutar en: Ferozo (DigitalPlus)
-- Requisito: credenciales sysadmin (sa)
-- Paso: 3 de 5
-- ============================================================

USE [DigitalPlus];
GO

-- Rol web: usado por DigitalPlusWeb (Blazor Server)
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_web' AND type = 'R')
    CREATE ROLE dp_role_web;
GO

-- SELECT en todas las tablas de negocio
GRANT SELECT ON dbo.Categorias TO dp_role_web;
GRANT SELECT ON dbo.Dedos TO dp_role_web;
GRANT SELECT ON dbo.Dias TO dp_role_web;
GRANT SELECT ON dbo.Feriados TO dp_role_web;
GRANT SELECT ON dbo.Fichadas TO dp_role_web;
GRANT SELECT ON dbo.GRALUsuarios TO dp_role_web;
GRANT SELECT ON dbo.Horarios TO dp_role_web;
GRANT SELECT ON dbo.HorariosDias TO dp_role_web;
GRANT SELECT ON dbo.HorariosDiasEventos TO dp_role_web;
GRANT SELECT ON dbo.Incidencias TO dp_role_web;
GRANT SELECT ON dbo.IncidenciasLegajos TO dp_role_web;
GRANT SELECT ON dbo.Legajos TO dp_role_web;
GRANT SELECT ON dbo.LegajosHuellas TO dp_role_web;
GRANT SELECT ON dbo.LegajosSucursales TO dp_role_web;
GRANT SELECT ON dbo.Noticias TO dp_role_web;
GRANT SELECT ON dbo.Sectores TO dp_role_web;
GRANT SELECT ON dbo.Sucursales TO dp_role_web;
GRANT SELECT ON dbo.Terminales TO dp_role_web;
GRANT SELECT ON dbo.UsuariosSucursales TO dp_role_web;
GRANT SELECT ON dbo.Vacaciones TO dp_role_web;
GRANT SELECT ON dbo.VariablesGlobales TO dp_role_web;

-- SELECT en tablas de Identity (EF Core las necesita)
GRANT SELECT ON dbo.AspNetUsers TO dp_role_web;
GRANT SELECT ON dbo.AspNetRoles TO dp_role_web;
GRANT SELECT ON dbo.AspNetUserRoles TO dp_role_web;
GRANT SELECT ON dbo.AspNetUserClaims TO dp_role_web;
GRANT SELECT ON dbo.AspNetUserLogins TO dp_role_web;
GRANT SELECT ON dbo.AspNetUserTokens TO dp_role_web;
GRANT SELECT ON dbo.AspNetRoleClaims TO dp_role_web;
GRANT SELECT ON dbo.__EFMigrationsHistory TO dp_role_web;

-- INSERT en tablas de negocio que la web puede crear
GRANT INSERT ON dbo.Fichadas TO dp_role_web;
GRANT INSERT ON dbo.Legajos TO dp_role_web;
GRANT INSERT ON dbo.LegajosHuellas TO dp_role_web;
GRANT INSERT ON dbo.LegajosSucursales TO dp_role_web;
GRANT INSERT ON dbo.Horarios TO dp_role_web;
GRANT INSERT ON dbo.HorariosDias TO dp_role_web;
GRANT INSERT ON dbo.HorariosDiasEventos TO dp_role_web;
GRANT INSERT ON dbo.Incidencias TO dp_role_web;
GRANT INSERT ON dbo.IncidenciasLegajos TO dp_role_web;
GRANT INSERT ON dbo.Feriados TO dp_role_web;
GRANT INSERT ON dbo.Noticias TO dp_role_web;
GRANT INSERT ON dbo.Vacaciones TO dp_role_web;
GRANT INSERT ON dbo.Categorias TO dp_role_web;
GRANT INSERT ON dbo.Sectores TO dp_role_web;
GRANT INSERT ON dbo.Sucursales TO dp_role_web;
GRANT INSERT ON dbo.UsuariosSucursales TO dp_role_web;
GRANT INSERT ON dbo.GRALUsuarios TO dp_role_web;
GRANT INSERT ON dbo.Terminales TO dp_role_web;
GRANT INSERT ON dbo.VariablesGlobales TO dp_role_web;

-- INSERT en tablas de Identity
GRANT INSERT ON dbo.AspNetUsers TO dp_role_web;
GRANT INSERT ON dbo.AspNetRoles TO dp_role_web;
GRANT INSERT ON dbo.AspNetUserRoles TO dp_role_web;
GRANT INSERT ON dbo.AspNetUserClaims TO dp_role_web;
GRANT INSERT ON dbo.AspNetUserLogins TO dp_role_web;
GRANT INSERT ON dbo.AspNetUserTokens TO dp_role_web;
GRANT INSERT ON dbo.AspNetRoleClaims TO dp_role_web;
GRANT INSERT ON dbo.__EFMigrationsHistory TO dp_role_web;

-- UPDATE en tablas de negocio
GRANT UPDATE ON dbo.Fichadas TO dp_role_web;
GRANT UPDATE ON dbo.Legajos TO dp_role_web;
GRANT UPDATE ON dbo.LegajosHuellas TO dp_role_web;
GRANT UPDATE ON dbo.LegajosSucursales TO dp_role_web;
GRANT UPDATE ON dbo.Horarios TO dp_role_web;
GRANT UPDATE ON dbo.HorariosDias TO dp_role_web;
GRANT UPDATE ON dbo.HorariosDiasEventos TO dp_role_web;
GRANT UPDATE ON dbo.Incidencias TO dp_role_web;
GRANT UPDATE ON dbo.IncidenciasLegajos TO dp_role_web;
GRANT UPDATE ON dbo.Feriados TO dp_role_web;
GRANT UPDATE ON dbo.Noticias TO dp_role_web;
GRANT UPDATE ON dbo.Vacaciones TO dp_role_web;
GRANT UPDATE ON dbo.Categorias TO dp_role_web;
GRANT UPDATE ON dbo.Sectores TO dp_role_web;
GRANT UPDATE ON dbo.Sucursales TO dp_role_web;
GRANT UPDATE ON dbo.UsuariosSucursales TO dp_role_web;
GRANT UPDATE ON dbo.GRALUsuarios TO dp_role_web;
GRANT UPDATE ON dbo.Terminales TO dp_role_web;
GRANT UPDATE ON dbo.VariablesGlobales TO dp_role_web;

-- UPDATE en tablas de Identity
GRANT UPDATE ON dbo.AspNetUsers TO dp_role_web;
GRANT UPDATE ON dbo.AspNetRoles TO dp_role_web;
GRANT UPDATE ON dbo.AspNetUserRoles TO dp_role_web;
GRANT UPDATE ON dbo.AspNetUserClaims TO dp_role_web;
GRANT UPDATE ON dbo.AspNetUserLogins TO dp_role_web;
GRANT UPDATE ON dbo.AspNetUserTokens TO dp_role_web;
GRANT UPDATE ON dbo.AspNetRoleClaims TO dp_role_web;

-- DELETE restringido (solo donde EF Core lo necesita)
GRANT DELETE ON dbo.LegajosHuellas TO dp_role_web;
GRANT DELETE ON dbo.Fichadas TO dp_role_web;
GRANT DELETE ON dbo.IncidenciasLegajos TO dp_role_web;
GRANT DELETE ON dbo.LegajosSucursales TO dp_role_web;
GRANT DELETE ON dbo.HorariosDias TO dp_role_web;
GRANT DELETE ON dbo.HorariosDiasEventos TO dp_role_web;
GRANT DELETE ON dbo.UsuariosSucursales TO dp_role_web;
GRANT DELETE ON dbo.Vacaciones TO dp_role_web;
GRANT DELETE ON dbo.Feriados TO dp_role_web;

-- DELETE en tablas de Identity (EF Core lo requiere)
GRANT DELETE ON dbo.AspNetUserRoles TO dp_role_web;
GRANT DELETE ON dbo.AspNetUserClaims TO dp_role_web;
GRANT DELETE ON dbo.AspNetUserLogins TO dp_role_web;
GRANT DELETE ON dbo.AspNetUserTokens TO dp_role_web;
GRANT DELETE ON dbo.AspNetRoleClaims TO dp_role_web;

-- EXECUTE en todos los SPs
GRANT EXECUTE ON dbo.EscritorioFichadasSPSALIDA TO dp_role_web;
GRANT EXECUTE ON dbo.EscritorioLegajoActualizar TO dp_role_web;
GRANT EXECUTE ON dbo.EscritorioLegajosHuellasActualizar TO dp_role_web;
GRANT EXECUTE ON dbo.RRHHLegajos_DeleteTodo TO dp_role_web;
GRANT EXECUTE ON dbo.WebAusencias_Listado_General TO dp_role_web;
GRANT EXECUTE ON dbo.WebAusencias_Listado_General_ConSucursales TO dp_role_web;
GRANT EXECUTE ON dbo.WebCalculoMinutosMensualesCalendarioPorLegajo TO dp_role_web;
GRANT EXECUTE ON dbo.WebConsolidado_Listado TO dp_role_web;
GRANT EXECUTE ON dbo.WebControlAcceso_Listado TO dp_role_web;
GRANT EXECUTE ON dbo.WebDashBoardMinutosTrabajadosMensualesPorLegajo TO dp_role_web;
GRANT EXECUTE ON dbo.WebDashBoardMinutosTrabajadosMensualesPorSucursal TO dp_role_web;
GRANT EXECUTE ON dbo.WebHorasExtras_Listado TO dp_role_web;
GRANT EXECUTE ON dbo.WebLlegadaTarde_Listado TO dp_role_web;
GRANT EXECUTE ON dbo.WebLlegadaTarde_Listado_General TO dp_role_web;
GO

PRINT 'Rol dp_role_web creado en DigitalPlus con permisos granulares.';
GO
