-- ============================================================
-- 014_CreateLogin_dp_web_svc.sql
-- Crear login y usuario dp_web_svc para DigitalPlusWeb
--
-- Ejecutar en: Ferozo (master + DigitalPlus)
-- Requisito: credenciales sysadmin (sa)
-- Prerequisito: 013_CreateRoles_DigitalPlus.sql
-- Paso: 4 de 5
--
-- IMPORTANTE: Reemplazar <PASSWORD_SEGURO_WEB> por un password real
-- antes de ejecutar. Usar minimo 20 caracteres con mayusculas,
-- minusculas, numeros y especiales.
-- ============================================================

USE [master];
GO

-- Crear login de servidor
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'dp_web_svc')
BEGIN
    CREATE LOGIN [dp_web_svc]
    WITH PASSWORD = '<PASSWORD_SEGURO_WEB>',
         DEFAULT_DATABASE = [DigitalPlus],
         CHECK_POLICY = ON,
         CHECK_EXPIRATION = OFF;
    PRINT 'Login dp_web_svc creado.';
END
ELSE
    PRINT 'Login dp_web_svc ya existe.';
GO

-- Crear usuario en DigitalPlus y asignar rol
USE [DigitalPlus];
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_web_svc')
BEGIN
    CREATE USER [dp_web_svc] FOR LOGIN [dp_web_svc];
    PRINT 'User dp_web_svc creado en DigitalPlus.';
END
ELSE
    PRINT 'User dp_web_svc ya existe en DigitalPlus.';
GO

ALTER ROLE dp_role_web ADD MEMBER [dp_web_svc];
GO

PRINT 'dp_web_svc asignado a dp_role_web en DigitalPlus.';
GO
