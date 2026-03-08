# EJECUCIÓN DEL PLAN – SEGURIDAD SQL (PRODUCCIÓN)

Documento base:

0022-Plan_Aplicacion_Seguridad_SQL.md

Ajustes incorporados:

0023-Ajustes_Plan_Aplicacion_Seguridad_SQL.md

La arquitectura ya fue validada y el hosting Ferozo confirmó soporte completo para:

- CREATE LOGIN
- CREATE USER
- CREATE ROLE
- GRANT
- ALTER ROLE
- CHECK_POLICY = ON

Por lo tanto **no se requieren más validaciones ni pruebas adicionales**.

El objetivo ahora es **ejecutar el plan y dejar el sistema funcionando con usuarios SQL dedicados en producción**.

---

# OBJETIVO

Eliminar el uso de `sa` en runtime y reemplazarlo por:

- `dp_admin_svc` → Azure Functions / administración
- `dp_web_svc` → DigitalPlusWeb

El login `sa` permanecerá habilitado temporalmente como fallback.

---

# FASE 1 – EJECUCIÓN SQL EN FEROZO

Conectarse a Ferozo con usuario `sa`.

Ejecutar los scripts en este orden.

## Paso 1 – Backup de seguridad

```sql
017_Backup_PreSecurity.sql

Debe crear backup de:

DigitalPlus

DigitalPlusAdmin

Si el backup falla detener ejecución.

Paso 2 – Deploy de seguridad SQL

Ejecutar el script maestro:

020_Deploy_SQLSecurity_Ferozo.sql

Este script ejecuta internamente:

011_CreateRoles_DigitalPlusAdmin.sql
012_CreateLogin_dp_admin_svc.sql
013_CreateRoles_DigitalPlus.sql
014_CreateLogin_dp_web_svc.sql
016_VerifyPermissions.sql

Resultado esperado:

roles creados

logins creados

usuarios asociados a cada base

permisos asignados

Paso 3 – Validar conexión con nuevos usuarios

Probar conexión directa.

sqlcmd -S sd-1985882-l.ferozo.com,11434 -U dp_admin_svc -P "<password_admin>" -d DigitalPlusAdmin -Q "SELECT COUNT(*) FROM Licencias"
sqlcmd -S sd-1985882-l.ferozo.com,11434 -U dp_web_svc -P "<password_web>" -d DigitalPlus -Q "SELECT COUNT(*) FROM Legajos"

Ambos comandos deben retornar resultados sin error.

Paso 4 – Verificar permisos de escritura

Ejecutar:

018_TestWritePermissions.sql

Este script ejecuta:

BEGIN TRAN
INSERT INTO LicenciasLog (...)
ROLLBACK

Debe ejecutarse sin error.

FASE 2 – ACTUALIZAR CONFIGURACIONES

Una vez confirmados los nuevos usuarios.

Azure Functions

Actualizar App Settings en:

Function App: digitalplus-provision

Cambiar:

ProvisioningDb
CloudSqlUser
CloudSqlPassword

Reemplazar credenciales sa por dp_admin_svc.

Reiniciar Azure Functions.

DigitalPlusWeb

Actualizar connection strings en:

DigitalPlusWeb/appsettings.json

Reemplazar:

User Id=sa

por

User Id=dp_web_svc

Reiniciar sitio web.

Scripts administrativos

Actualizar defaults en:

AzureProvisioning/tools/*.ps1

Cambiar credenciales a:

dp_admin_svc
FASE 3 – VERIFICACIÓN FUNCIONAL

Confirmar que el sistema funciona correctamente.

Azure Functions

Probar:

/api/health
/api/license/activate
/api/license/heartbeat
/api/provision

Todos deben responder correctamente.

DigitalPlusWeb

Verificar:

login

dashboard

listado de legajos

listado de fichadas

reportes

creación de legajo

FASE 4 – MONITOREO DE CONEXIONES

Ejecutar:

019_MonitorConnections.sql

Resultado esperado:

Solo deben aparecer logins:

dp_admin_svc
dp_web_svc

Si aparece sa, identificar qué componente lo utiliza.

FASE 5 – ESTABILIZACIÓN

Durante los próximos 7 días:

monitorear conexiones SQL

verificar licencias

verificar provisioning

Si no aparecen problemas:

cambiar password de sa

o deshabilitar login sa

ENTREGABLE ESPERADO

Al finalizar la ejecución:

Sistema funcionando con usuarios SQL dedicados

Azure Functions operando con dp_admin_svc

DigitalPlusWeb operando con dp_web_svc

sa fuera del runtime normal

Monitoreo activo de conexiones

ALCANCE DE ESTA ENTREGA

Esta fase no incluye:

modificaciones al instalador

portal de administración

migración de clientes existentes

Esos puntos se abordarán en fases posteriores.

RESULTADO FINAL

DigitalPlus quedará operando con:

seguridad SQL basada en roles

usuarios dedicados

credenciales separadas por servicio

eliminación del uso de sa en runtime