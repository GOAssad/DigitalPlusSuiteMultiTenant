-- ============================================================
-- 012_CreateLogin_dp_admin_svc.sql
-- Crear login y usuario dp_admin_svc para Azure Functions y scripts
--
-- Ejecutar en: Ferozo (master + DigitalPlusAdmin)
-- Requisito: credenciales sysadmin (sa)
-- Prerequisito: 011_CreateRoles_DigitalPlusAdmin.sql
-- Paso: 2 de 5
--
-- IMPORTANTE: Reemplazar <PASSWORD_SEGURO_ADMIN> por un password real
-- antes de ejecutar. Usar minimo 20 caracteres con mayusculas,
-- minusculas, numeros y especiales.
-- ============================================================

USE [master];
GO

-- Crear login de servidor
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'dp_admin_svc')
BEGIN
    CREATE LOGIN [dp_admin_svc]
    WITH PASSWORD = '<PASSWORD_SEGURO_ADMIN>',
         DEFAULT_DATABASE = [DigitalPlusAdmin],
         CHECK_POLICY = ON,
         CHECK_EXPIRATION = OFF;
    PRINT 'Login dp_admin_svc creado.';
END
ELSE
    PRINT 'Login dp_admin_svc ya existe.';
GO

-- Crear usuario en DigitalPlusAdmin y asignar rol
USE [DigitalPlusAdmin];
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_admin_svc')
BEGIN
    CREATE USER [dp_admin_svc] FOR LOGIN [dp_admin_svc];
    PRINT 'User dp_admin_svc creado en DigitalPlusAdmin.';
END
ELSE
    PRINT 'User dp_admin_svc ya existe en DigitalPlusAdmin.';
GO

ALTER ROLE dp_role_admin ADD MEMBER [dp_admin_svc];
GO

PRINT 'dp_admin_svc asignado a dp_role_admin en DigitalPlusAdmin.';
GO
