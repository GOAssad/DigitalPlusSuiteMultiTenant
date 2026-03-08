-- ============================================================
-- 019_MonitorConnections.sql
-- Monitoreo de conexiones activas post-implementacion
--
-- Ejecutar en: Ferozo (cualquier BD)
-- Requisito: credenciales sysadmin (sa)
-- Uso: ejecutar periodicamente durante los primeros 7 dias
--
-- Resultado esperado: solo dp_admin_svc y dp_web_svc
-- Si aparece sa, identificar que componente lo usa
-- ============================================================

PRINT '=== Conexiones activas ===';
PRINT 'Fecha: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '';

SELECT
    s.login_name AS Login,
    s.host_name AS Host,
    s.program_name AS Programa,
    DB_NAME(s.database_id) AS [Database],
    s.login_time AS Conexion,
    s.last_request_start_time AS UltimaActividad,
    s.status AS Estado
FROM sys.dm_exec_sessions s
WHERE s.is_user_process = 1
ORDER BY s.login_time DESC;

PRINT '';

-- Resumen por login
PRINT '=== Resumen por login ===';
SELECT
    s.login_name AS Login,
    COUNT(*) AS Conexiones,
    MAX(s.last_request_start_time) AS UltimaActividad
FROM sys.dm_exec_sessions s
WHERE s.is_user_process = 1
GROUP BY s.login_name
ORDER BY s.login_name;

-- Alerta si sa sigue activo
IF EXISTS (
    SELECT 1 FROM sys.dm_exec_sessions
    WHERE is_user_process = 1 AND login_name = 'sa'
    AND last_request_start_time > DATEADD(MINUTE, -30, GETDATE())
)
BEGIN
    PRINT '';
    PRINT '*** ALERTA: El login sa tiene actividad en los ultimos 30 minutos ***';
    PRINT '*** Verificar que componente lo esta usando ***';
END
ELSE
BEGIN
    PRINT '';
    PRINT '[OK] No hay actividad reciente del login sa.';
END
GO
