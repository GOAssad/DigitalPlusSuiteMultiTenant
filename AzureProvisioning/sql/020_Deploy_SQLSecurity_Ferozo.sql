-- ============================================================
-- 020_Deploy_SQLSecurity_Ferozo.sql
-- Script maestro: ejecuta todos los scripts de seguridad SQL
--
-- Ejecutar en: Ferozo con sqlcmd
-- Requisito: credenciales sysadmin (sa)
--
-- USO:
--   sqlcmd -S sd-1985882-l.ferozo.com,11434 -U sa -P Soporte1 -i 020_Deploy_SQLSecurity_Ferozo.sql
--
-- IMPORTANTE:
--   1. Reemplazar <PASSWORD_SEGURO_ADMIN> en 012 antes de ejecutar
--   2. Reemplazar <PASSWORD_SEGURO_WEB> en 014 antes de ejecutar
--   3. Si prefiere ejecucion manual, ejecutar cada script por separado
--
-- ORDEN:
--   017 -> Backup
--   011 -> Roles DigitalPlusAdmin
--   012 -> Login dp_admin_svc
--   013 -> Roles DigitalPlus
--   014 -> Login dp_web_svc
--   016 -> Verificacion
-- ============================================================

PRINT '============================================================';
PRINT 'DEPLOY SEGURIDAD SQL - FEROZO';
PRINT 'Inicio: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '============================================================';
PRINT '';

PRINT '>>> Paso 0: Backup...';
:r .\017_Backup_PreSecurity.sql

PRINT '';
PRINT '>>> Paso 1: Roles en DigitalPlusAdmin...';
:r .\011_CreateRoles_DigitalPlusAdmin.sql

PRINT '';
PRINT '>>> Paso 2: Login dp_admin_svc...';
:r .\012_CreateLogin_dp_admin_svc.sql

PRINT '';
PRINT '>>> Paso 3: Roles en DigitalPlus...';
:r .\013_CreateRoles_DigitalPlus.sql

PRINT '';
PRINT '>>> Paso 4: Login dp_web_svc...';
:r .\014_CreateLogin_dp_web_svc.sql

PRINT '';
PRINT '>>> Paso 5: Verificacion...';
:r .\016_VerifyPermissions.sql

PRINT '';
PRINT '============================================================';
PRINT 'DEPLOY COMPLETADO';
PRINT 'Fin: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '============================================================';
PRINT '';
PRINT 'SIGUIENTE PASO:';
PRINT '  Ejecutar 018_TestWritePermissions.sql conectado con cada usuario nuevo';
PRINT '  para verificar permisos de escritura.';
GO
