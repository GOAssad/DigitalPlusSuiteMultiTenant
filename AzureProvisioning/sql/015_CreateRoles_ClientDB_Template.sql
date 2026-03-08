-- ============================================================
-- 015_CreateRoles_ClientDB_Template.sql
-- Template para crear roles en BD de cliente (DP_NombreEmpresa)
--
-- Ejecutar en: BD del cliente (local, SQL Express)
-- Requisito: credenciales sysadmin o db_owner (bootstrap)
-- Paso: 5 de 5
--
-- Este script es una PLANTILLA. El instalador lo ejecutara
-- automaticamente reemplazando las variables.
--
-- Variables a reemplazar:
--   {{DB_NAME}}     - nombre de la BD (ej: DP_Mi_Empresa)
--   {{LOGIN_NAME}}  - nombre del login (ej: dp_Mi_Empresa_a1b2c3)
--   {{PASSWORD}}    - password generado (20 chars aleatorios)
-- ============================================================

-- ============================================================
-- PARTE 1: Crear roles
-- ============================================================

USE [{{DB_NAME}}];
GO

-- Rol runtime: usado por Fichador
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_runtime' AND type = 'R')
    CREATE ROLE dp_role_runtime;
GO

-- SELECT solo sobre tablas necesarias para fichadas
GRANT SELECT ON dbo.Legajos TO dp_role_runtime;
GRANT SELECT ON dbo.LegajosHuellas TO dp_role_runtime;
GRANT SELECT ON dbo.Horarios TO dp_role_runtime;
GRANT SELECT ON dbo.Sucursales TO dp_role_runtime;
GRANT SELECT ON dbo.Terminales TO dp_role_runtime;
GRANT SELECT ON dbo.VariablesGlobales TO dp_role_runtime;
GRANT SELECT ON dbo.Fichadas TO dp_role_runtime;
GRANT SELECT ON dbo.LegajosSucursales TO dp_role_runtime;
GRANT SELECT ON dbo.Categorias TO dp_role_runtime;
GRANT SELECT ON dbo.Sectores TO dp_role_runtime;
GRANT SELECT ON dbo.GRALUsuarios TO dp_role_runtime;

-- INSERT solo en Fichadas
GRANT INSERT ON dbo.Fichadas TO dp_role_runtime;

-- EXECUTE sobre SP de fichadas
GRANT EXECUTE ON dbo.EscritorioFichadasSPSALIDA TO dp_role_runtime;
GO

-- Rol admin: usado por Administrador
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_admin' AND type = 'R')
    CREATE ROLE dp_role_admin;
GO

-- SELECT en todas las tablas
GRANT SELECT ON SCHEMA::dbo TO dp_role_admin;

-- INSERT en todas las tablas
GRANT INSERT ON SCHEMA::dbo TO dp_role_admin;

-- UPDATE en todas las tablas
GRANT UPDATE ON SCHEMA::dbo TO dp_role_admin;

-- EXECUTE en todos los SPs
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_admin;

-- DELETE solo en tablas donde el Administrador borra registros
GRANT DELETE ON dbo.LegajosHuellas TO dp_role_admin;
GRANT DELETE ON dbo.Fichadas TO dp_role_admin;
GRANT DELETE ON dbo.IncidenciasLegajos TO dp_role_admin;
GRANT DELETE ON dbo.Horarios TO dp_role_admin;
GRANT DELETE ON dbo.HorariosDias TO dp_role_admin;
GRANT DELETE ON dbo.HorariosDiasEventos TO dp_role_admin;
GRANT DELETE ON dbo.LegajosSucursales TO dp_role_admin;
GO

PRINT 'Roles dp_role_runtime y dp_role_admin creados en {{DB_NAME}}.';
GO

-- ============================================================
-- PARTE 2: Crear login y usuario dedicado
-- ============================================================

USE [master];
GO

IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = '{{LOGIN_NAME}}')
BEGIN
    CREATE LOGIN [{{LOGIN_NAME}}]
    WITH PASSWORD = '{{PASSWORD}}',
         DEFAULT_DATABASE = [{{DB_NAME}}],
         CHECK_POLICY = ON,
         CHECK_EXPIRATION = OFF;
    PRINT 'Login {{LOGIN_NAME}} creado.';
END
GO

USE [{{DB_NAME}}];
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = '{{LOGIN_NAME}}')
BEGIN
    CREATE USER [{{LOGIN_NAME}}] FOR LOGIN [{{LOGIN_NAME}}];
    PRINT 'User {{LOGIN_NAME}} creado en {{DB_NAME}}.';
END
GO

-- Asignar ambos roles (Fichador + Administrador comparten usuario en local)
ALTER ROLE dp_role_runtime ADD MEMBER [{{LOGIN_NAME}}];
ALTER ROLE dp_role_admin ADD MEMBER [{{LOGIN_NAME}}];
GO

PRINT 'Login {{LOGIN_NAME}} asignado a dp_role_runtime + dp_role_admin.';
GO
