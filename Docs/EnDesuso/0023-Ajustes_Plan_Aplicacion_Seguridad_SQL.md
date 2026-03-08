# Ajustes al Plan de Aplicación – Seguridad SQL (Fase 1-2)

Documento base revisado:

0022-Plan_Aplicacion_Seguridad_SQL.md

El plan está aprobado en términos generales, pero se requieren algunos **ajustes menores para mejorar seguridad, verificaciones y control de despliegue** antes de ejecutarlo en producción.

Estos cambios **no modifican la arquitectura definida**, solo fortalecen el procedimiento de implementación.

---

# 1. NUEVO PASO 0 – BACKUP PREVIO

Antes de ejecutar cualquier script SQL en Ferozo se debe realizar un backup de las bases involucradas.

Agregar al inicio del plan:

## Paso 0 – Backup de seguridad

Ejecutar:

```sql
BACKUP DATABASE DigitalPlus
TO DISK = 'DigitalPlus_pre_security_change.bak';

BACKUP DATABASE DigitalPlusAdmin
TO DISK = 'DigitalPlusAdmin_pre_security_change.bak';

Objetivo:

permitir rollback rápido si ocurre algún problema con permisos o scripts

preservar estado anterior antes de modificar seguridad

2. VALIDACIÓN DE PERMISOS DE ESCRITURA

Actualmente el plan prueba conexión usando SELECT.

Esto confirma lectura pero no confirma permisos de escritura.

Se debe agregar una prueba adicional utilizando una transacción reversible.

Agregar en la sección de pruebas de conexión:

BEGIN TRAN

INSERT INTO LicenciasLog (LicenciaId, Accion, Fecha)
VALUES (1, 'test-permissions', GETDATE());

ROLLBACK

Resultado esperado:

el INSERT debe ejecutarse sin error

el ROLLBACK asegura que no se alteren datos reales

3. VALIDACIÓN DESDE APLICACIONES ANTES DE CAMBIAR PRODUCCIÓN

Antes de actualizar Azure App Settings y configuración de la Web en producción se debe realizar una prueba previa desde entorno controlado.

Agregar el siguiente paso antes del cambio de configuraciones:

Paso intermedio – prueba desde aplicación

Ejecutar localmente Azure Functions con las nuevas credenciales

Ejecutar DigitalPlusWeb en entorno de desarrollo usando el nuevo usuario SQL

Confirmar que:

consultas funcionan

escritura funciona

licencias se generan correctamente

provisioning responde correctamente

Solo después de confirmar estos puntos se debe aplicar el cambio en Azure Portal y en el deployment web.

Objetivo:

evitar reinicios innecesarios de Azure Functions

detectar problemas de permisos antes de tocar producción

4. SCRIPT MAESTRO OPCIONAL

Para facilitar ejecución controlada se debe generar un script maestro opcional que ejecute todos los scripts SQL en el orden correcto.

Nuevo archivo sugerido:

020_Deploy_SQLSecurity_Ferozo.sql

Contenido:

:r .\011_CreateRoles_DigitalPlusAdmin.sql
:r .\012_CreateLogin_dp_admin_svc.sql
:r .\013_CreateRoles_DigitalPlus.sql
:r .\014_CreateLogin_dp_web_svc.sql
:r .\016_VerifyPermissions.sql

Este script permite:

ejecutar todo en un solo paso

o ejecutar scripts individuales manualmente

5. MONITOREO POST-IMPLEMENTACIÓN

Agregar una sección de monitoreo durante los primeros días posteriores al cambio.

Monitoreo de conexiones

Durante los primeros días ejecutar periódicamente:

SELECT
    s.login_name,
    s.host_name,
    s.program_name,
    DB_NAME(s.database_id) AS database_name,
    s.login_time
FROM sys.dm_exec_sessions s
WHERE s.is_user_process = 1
ORDER BY s.login_time DESC;

Resultado esperado:

solo deben aparecer los usuarios:

dp_admin_svc
dp_web_svc

Si aparece sa, identificar inmediatamente qué componente lo está utilizando.

6. POLÍTICA PARA EL LOGIN SA

No modificar el login sa inmediatamente después del cambio.

Regla operativa:

mantener sa activo durante al menos 7 días

confirmar que todas las funciones operan correctamente con los nuevos usuarios

solo después considerar:

cambiar password de sa a uno complejo

o deshabilitar el login si el hosting lo permite

7. ALCANCE DE ESTA FASE

Confirmar explícitamente que esta fase solo cubre:

seguridad SQL central (Ferozo)

actualización de credenciales en Azure Functions

actualización de credenciales en DigitalPlusWeb

No incluye:

cambios en el instalador

portal de administración

migración de clientes existentes

Estas tareas corresponden a fases posteriores.

RESULTADO ESPERADO

Luego de aplicar estas modificaciones el plan de aplicación tendrá:

mayor seguridad operativa

verificación de permisos completa

capacidad de rollback segura

control de despliegue más robusto

Una vez incorporados estos ajustes el plan quedará listo para ejecución en producción.


---

Pequeño comentario final, Gustavo.

Con esto ya pasamos por todas las fases serias de un cambio de seguridad:

1. **diseño**
2. **validación del hosting**
3. **scripts**
4. **plan de despliegue**
5. **verificación y rollback**

Eso es exactamente cómo se hace en sistemas que no quieren romper producción a las 3 AM.

Así que sí… sorprendentemente estamos haciendo ingeniería de software como adultos. Qué experiencia tan inusual.