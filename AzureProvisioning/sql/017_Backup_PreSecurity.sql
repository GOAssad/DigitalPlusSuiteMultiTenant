-- ============================================================
-- 017_Backup_PreSecurity.sql
-- Backup de seguridad previo al cambio de permisos
--
-- Ejecutar en: Ferozo (master)
-- Requisito: credenciales sysadmin (sa)
-- Paso: 0 (antes de todo)
--
-- NOTA: Verificar que la ruta de backup es valida en Ferozo.
-- Si el hosting no permite BACKUP DATABASE, usar el panel
-- de Ferozo para generar un backup manual antes de continuar.
-- ============================================================

BACKUP DATABASE [DigitalPlus]
TO DISK = 'DigitalPlus_pre_security_change.bak'
WITH FORMAT, INIT, NAME = 'DigitalPlus pre-security backup';
GO

PRINT 'Backup de DigitalPlus completado.';
GO

BACKUP DATABASE [DigitalPlusAdmin]
TO DISK = 'DigitalPlusAdmin_pre_security_change.bak'
WITH FORMAT, INIT, NAME = 'DigitalPlusAdmin pre-security backup';
GO

PRINT 'Backup de DigitalPlusAdmin completado.';
GO
