-- ============================================================
-- 016_VerifyPermissions.sql
-- Script de verificacion post-cambio
--
-- Ejecutar en: Ferozo (DigitalPlusAdmin y DigitalPlus)
-- Requisito: credenciales sysadmin (sa) para verificar
--
-- Este script NO modifica nada. Solo verifica que los
-- permisos estan correctamente configurados.
-- ============================================================

-- ============================================================
-- VERIFICACION 1: dp_admin_svc en DigitalPlusAdmin
-- ============================================================
USE [DigitalPlusAdmin];
GO

PRINT '=== Verificacion: dp_admin_svc en DigitalPlusAdmin ===';

-- Verificar que el usuario existe
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_admin_svc')
    PRINT '[OK] User dp_admin_svc existe'
ELSE
    PRINT '[FALLO] User dp_admin_svc NO existe'

-- Verificar que el rol existe
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_admin' AND type = 'R')
    PRINT '[OK] Rol dp_role_admin existe'
ELSE
    PRINT '[FALLO] Rol dp_role_admin NO existe'

-- Verificar membresia
IF IS_ROLEMEMBER('dp_role_admin', 'dp_admin_svc') = 1
    PRINT '[OK] dp_admin_svc es miembro de dp_role_admin'
ELSE
    PRINT '[FALLO] dp_admin_svc NO es miembro de dp_role_admin'

-- Verificar permisos efectivos
PRINT '';
PRINT 'Permisos efectivos de dp_admin_svc:';
SELECT
    dp.class_desc AS Tipo,
    OBJECT_NAME(dp.major_id) AS Objeto,
    dp.permission_name AS Permiso,
    dp.state_desc AS Estado
FROM sys.database_permissions dp
JOIN sys.database_principals pr ON dp.grantee_principal_id = pr.principal_id
WHERE pr.name = 'dp_role_admin'
ORDER BY Objeto, Permiso;
GO

-- ============================================================
-- VERIFICACION 2: dp_web_svc en DigitalPlus
-- ============================================================
USE [DigitalPlus];
GO

PRINT '';
PRINT '=== Verificacion: dp_web_svc en DigitalPlus ===';

-- Verificar que el usuario existe
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_web_svc')
    PRINT '[OK] User dp_web_svc existe'
ELSE
    PRINT '[FALLO] User dp_web_svc NO existe'

-- Verificar que el rol existe
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_web' AND type = 'R')
    PRINT '[OK] Rol dp_role_web existe'
ELSE
    PRINT '[FALLO] Rol dp_role_web NO existe'

-- Verificar membresia
IF IS_ROLEMEMBER('dp_role_web', 'dp_web_svc') = 1
    PRINT '[OK] dp_web_svc es miembro de dp_role_web'
ELSE
    PRINT '[FALLO] dp_web_svc NO es miembro de dp_role_web'

-- Verificar permisos efectivos
PRINT '';
PRINT 'Permisos efectivos de dp_web_svc:';
SELECT
    dp.class_desc AS Tipo,
    OBJECT_NAME(dp.major_id) AS Objeto,
    dp.permission_name AS Permiso,
    dp.state_desc AS Estado
FROM sys.database_permissions dp
JOIN sys.database_principals pr ON dp.grantee_principal_id = pr.principal_id
WHERE pr.name = 'dp_role_web'
ORDER BY Objeto, Permiso;
GO

-- ============================================================
-- VERIFICACION 3: Logins de servidor
-- ============================================================
USE [master];
GO

PRINT '';
PRINT '=== Verificacion: Logins de servidor ===';

SELECT
    name AS Login,
    type_desc AS Tipo,
    is_disabled AS Deshabilitado,
    default_database_name AS BD_Default,
    CASE WHEN is_policy_checked = 1 THEN 'ON' ELSE 'OFF' END AS CheckPolicy,
    CASE WHEN is_expiration_checked = 1 THEN 'ON' ELSE 'OFF' END AS CheckExpiration
FROM sys.sql_logins
WHERE name IN ('dp_admin_svc', 'dp_web_svc')
ORDER BY name;
GO

PRINT '';
PRINT '=== Verificacion completada ===';
GO
