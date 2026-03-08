-- ============================================================
-- 011_CreateRoles_DigitalPlusAdmin.sql
-- Crear rol dp_role_admin en DigitalPlusAdmin (Ferozo)
--
-- Ejecutar en: Ferozo (DigitalPlusAdmin)
-- Requisito: credenciales sysadmin (sa)
-- Paso: 1 de 5
-- ============================================================

USE [DigitalPlusAdmin];
GO

-- Rol admin: usado por Azure Functions y scripts de gestion
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_admin' AND type = 'R')
    CREATE ROLE dp_role_admin;
GO

-- Permisos granulares por tabla
-- Tablas: ActivationCodes, Licencias, LicenciasLog, LicenseCodes

-- SELECT en todas las tablas
GRANT SELECT ON dbo.ActivationCodes TO dp_role_admin;
GRANT SELECT ON dbo.Licencias TO dp_role_admin;
GRANT SELECT ON dbo.LicenciasLog TO dp_role_admin;
GRANT SELECT ON dbo.LicenseCodes TO dp_role_admin;

-- INSERT en todas las tablas
GRANT INSERT ON dbo.ActivationCodes TO dp_role_admin;
GRANT INSERT ON dbo.Licencias TO dp_role_admin;
GRANT INSERT ON dbo.LicenciasLog TO dp_role_admin;
GRANT INSERT ON dbo.LicenseCodes TO dp_role_admin;

-- UPDATE en tablas que lo requieren
GRANT UPDATE ON dbo.ActivationCodes TO dp_role_admin;
GRANT UPDATE ON dbo.Licencias TO dp_role_admin;
GRANT UPDATE ON dbo.LicenseCodes TO dp_role_admin;

-- DELETE solo en LicenciasLog (limpieza de logs viejos)
GRANT DELETE ON dbo.LicenciasLog TO dp_role_admin;

-- EXECUTE en todos los SPs
GRANT EXECUTE ON dbo.License_Activate TO dp_role_admin;
GRANT EXECUTE ON dbo.License_Heartbeat TO dp_role_admin;
GRANT EXECUTE ON dbo.License_ValidateAndConsumeCode TO dp_role_admin;
GRANT EXECUTE ON dbo.Provisioning_InsertActivationCode TO dp_role_admin;
GRANT EXECUTE ON dbo.Provisioning_ListActivationCodes TO dp_role_admin;
GRANT EXECUTE ON dbo.Provisioning_ValidateAndConsumeCode TO dp_role_admin;
GO

PRINT 'Rol dp_role_admin creado en DigitalPlusAdmin con permisos granulares.';
GO
