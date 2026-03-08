# Plan de Aplicacion -- Seguridad SQL (Fase 1-2)

**Fecha:** 2026-03-05 (actualizado con ajustes de 0023)
**Basado en:** DigitalPlus_Seguridad_SQL_y_AdminPortal_v2.md
**Ajustes:** 0023-Ajustes_Plan_Aplicacion_Seguridad_SQL.md
**Alcance:** Creacion de roles, logins y usuarios en Ferozo + actualizacion de configs

**ALCANCE EXPLICITO DE ESTA FASE:**
- Seguridad SQL central (Ferozo)
- Actualizacion de credenciales en Azure Functions
- Actualizacion de credenciales en DigitalPlusWeb

**NO INCLUYE:** cambios en el instalador, portal de administracion, migracion de clientes existentes.

---

## 1. SCRIPTS GENERADOS

| # | Archivo | Ejecutar en | Descripcion |
|---|---|---|---|
| 017 | `017_Backup_PreSecurity.sql` | Ferozo / master | Backup de ambas BDs antes del cambio |
| 011 | `011_CreateRoles_DigitalPlusAdmin.sql` | Ferozo / DigitalPlusAdmin | Crea rol `dp_role_admin` con permisos granulares |
| 012 | `012_CreateLogin_dp_admin_svc.sql` | Ferozo / master + DigitalPlusAdmin | Crea login + user `dp_admin_svc`, asigna rol |
| 013 | `013_CreateRoles_DigitalPlus.sql` | Ferozo / DigitalPlus | Crea rol `dp_role_web` con permisos granulares |
| 014 | `014_CreateLogin_dp_web_svc.sql` | Ferozo / master + DigitalPlus | Crea login + user `dp_web_svc`, asigna rol |
| 015 | `015_CreateRoles_ClientDB_Template.sql` | BD cliente (local) | Template para el instalador: roles + login dedicado |
| 016 | `016_VerifyPermissions.sql` | Ferozo / ambas BDs | Verifica que todo esta correcto (solo lectura) |
| 018 | `018_TestWritePermissions.sql` | Ferozo / ambas BDs | Valida permisos de escritura con INSERT+ROLLBACK |
| 019 | `019_MonitorConnections.sql` | Ferozo | Monitoreo de conexiones activas post-cambio |
| 020 | `020_Deploy_SQLSecurity_Ferozo.sql` | Ferozo (sqlcmd) | Script maestro que ejecuta 017+011+012+013+014+016 |

Ubicacion: `AzureProvisioning\sql\`

---

## 2. ORDEN EXACTO DE EJECUCION

### Paso 0: Backup de seguridad

**OBLIGATORIO antes de cualquier cambio.**

Ejecutar `017_Backup_PreSecurity.sql` con sa en Ferozo.

Si Ferozo no permite `BACKUP DATABASE` por restricciones del hosting, usar el panel web de Ferozo para generar un backup manual de ambas BDs.

Objetivo: permitir rollback rapido si ocurre algun problema con permisos o scripts.

### Paso 1: Generar passwords seguros

Antes de ejecutar los scripts, generar dos passwords de al menos 20 caracteres:

- Password para `dp_admin_svc`: _________________________________
- Password para `dp_web_svc`: _________________________________

Requisitos: mayusculas + minusculas + numeros + al menos 2 caracteres especiales (!@#$%).

### Paso 2: Ejecutar scripts en Ferozo

Conectarse a Ferozo con `sa` y ejecutar en este orden:

**Opcion A (script maestro):**
```
sqlcmd -S sd-1985882-l.ferozo.com,11434 -U sa -P Soporte1 -i 020_Deploy_SQLSecurity_Ferozo.sql
```
(Requiere haber reemplazado los passwords en 012 y 014 previamente)

**Opcion B (manual, uno por uno):**
```
1. 017_Backup_PreSecurity.sql              (backup de ambas BDs)
2. 011_CreateRoles_DigitalPlusAdmin.sql    (crea rol en DigitalPlusAdmin)
3. 012_CreateLogin_dp_admin_svc.sql        (crea login + user, reemplazar <PASSWORD_SEGURO_ADMIN>)
4. 013_CreateRoles_DigitalPlus.sql         (crea rol en DigitalPlus)
5. 014_CreateLogin_dp_web_svc.sql          (crea login + user, reemplazar <PASSWORD_SEGURO_WEB>)
6. 016_VerifyPermissions.sql               (verifica todo)
```

### Paso 3: Probar conexion con nuevos usuarios

**3a. Probar SELECT (lectura):**

```
-- Probar dp_admin_svc contra DigitalPlusAdmin
sqlcmd -S sd-1985882-l.ferozo.com,11434 -U dp_admin_svc -P "<password>" -d DigitalPlusAdmin -Q "SELECT COUNT(*) FROM Licencias"

-- Probar dp_web_svc contra DigitalPlus
sqlcmd -S sd-1985882-l.ferozo.com,11434 -U dp_web_svc -P "<password>" -d DigitalPlus -Q "SELECT COUNT(*) FROM Legajos"
```

**3b. Probar escritura (INSERT + ROLLBACK):**

Ejecutar `018_TestWritePermissions.sql` conectado con cada usuario nuevo. Este script hace INSERT dentro de una transaccion que se revierte, verificando permisos de escritura sin alterar datos.

Si ambos retornan resultados sin error, los usuarios funcionan correctamente.

### Paso 3.5: Probar desde aplicaciones locales (ANTES de produccion)

**OBLIGATORIO antes de cambiar configs de produccion.**

1. Ejecutar Azure Functions localmente con las nuevas credenciales en `local.settings.json`
2. Ejecutar DigitalPlusWeb en entorno de desarrollo con el nuevo usuario SQL
3. Confirmar que:
   - Consultas funcionan
   - Escritura funciona
   - Licencias se generan correctamente
   - Provisioning responde correctamente

Solo despues de confirmar estos puntos se debe aplicar el cambio en Azure Portal y en el deployment web.

### Paso 4: Actualizar configuraciones

Una vez confirmado que los usuarios funcionan:

#### 4a. Azure App Settings (Azure Functions)

Actualizar en Azure Portal (Function App `digitalplus-provision`):

| Setting | Valor actual | Nuevo valor |
|---|---|---|
| `ProvisioningDb` | `...User Id=sa;Password=Soporte1...` | `...User Id=dp_admin_svc;Password=<password_admin>...` |
| `CloudSqlUser` | `sa` | `dp_admin_svc` |
| `CloudSqlPassword` | `Soporte1` | `<password_admin>` |

Comando:
```bash
az functionapp config appsettings set \
  --name digitalplus-provision \
  --resource-group rg-digitalplus-provision \
  --settings \
  "ProvisioningDb=Server=sd-1985882-l.ferozo.com,11434;Database=DigitalPlusAdmin;User Id=dp_admin_svc;Password=<PASSWORD>;Encrypt=True;TrustServerCertificate=True;" \
  "CloudSqlUser=dp_admin_svc" \
  "CloudSqlPassword=<PASSWORD>"
```

#### 4b. Scripts PowerShell (tools/)

Actualizar parametros por defecto en todos los scripts de `AzureProvisioning\tools\`:

| Archivo | Cambiar |
|---|---|
| `generate-code.ps1` | `-User "dp_admin_svc"` `-Password "<password>"` |
| `generate-license-code.ps1` | `-User "dp_admin_svc"` `-Password "<password>"` |
| `list-licenses.ps1` | `-User "dp_admin_svc"` `-Password "<password>"` |
| `list-license-codes.ps1` | `-User "dp_admin_svc"` `-Password "<password>"` |
| `modify-license.ps1` | `-User "dp_admin_svc"` `-Password "<password>"` |
| `suspend-license.ps1` | `-User "dp_admin_svc"` `-Password "<password>"` |

#### 4c. DigitalPlusWeb (Ferozo)

Actualizar `appsettings.json` en el deployment de la web:

| Connection String | Valor actual | Nuevo valor |
|---|---|---|
| `DefaultConnection` | `...User Id=sa;Password=Soporte1...` | `...User Id=dp_web_svc;Password=<password_web>...` |
| Otras strings que usen sa | idem | idem |

#### 4d. Common\Acceso.Clases.Datos\App.config

Limpiar connection strings legacy que apuntan a `192.168.0.11/TocayAnda`. Estas no se usan pero contienen credenciales.

#### 4e. local.settings.json (desarrollo)

Actualizar `AzureProvisioning\src\DigitalPlus.Provisioning\local.settings.json`:

```json
"ProvisioningDb": "Server=sd-1985882-l.ferozo.com,11434;Database=DigitalPlusAdmin;User Id=dp_admin_svc;Password=<PASSWORD>;Encrypt=True;TrustServerCertificate=True;"
```

### Paso 5: Verificar funcionamiento completo

Probar TODAS las funciones con los nuevos usuarios antes de quitar sa.

### Paso 6: (FUTURO) Securizar sa

Solo despues de confirmar que todo funciona durante al menos una semana:
- Cambiar password de sa a uno muy complejo
- O deshabilitar el login sa si es posible

---

## 3. ARCHIVOS QUE DEBEN MODIFICARSE

| Archivo | Tipo de cambio | Componente |
|---|---|---|
| Azure App Settings (3 settings) | Reemplazar credenciales | Azure Functions |
| `tools/generate-code.ps1` | Cambiar defaults User/Password | Scripts admin |
| `tools/generate-license-code.ps1` | Cambiar defaults User/Password | Scripts admin |
| `tools/list-licenses.ps1` | Cambiar defaults User/Password | Scripts admin |
| `tools/list-license-codes.ps1` | Cambiar defaults User/Password | Scripts admin |
| `tools/modify-license.ps1` | Cambiar defaults User/Password | Scripts admin |
| `tools/suspend-license.ps1` | Cambiar defaults User/Password | Scripts admin |
| `local.settings.json` | Cambiar ProvisioningDb | Azure Functions (dev) |
| `DigitalPlusWeb/appsettings.json` | Cambiar connection strings | Web |
| `DigitalPlusWeb/appsettings.Development.json` | Cambiar connection strings | Web (dev) |
| `Common/Acceso.Clases.Datos/App.config` | Limpiar strings legacy | Desktop (dev) |
| `deployment-guide.md` | Actualizar ejemplos | Documentacion |

---

## 4. PRUEBAS FUNCIONALES POST-CAMBIO

### 4.1 Azure Functions (dp_admin_svc)

| # | Prueba | Comando / Accion | Resultado esperado |
|---|---|---|---|
| 1 | Health check | `GET /api/health` | 200 OK con status "healthy" |
| 2 | Activar licencia trial | `POST /api/license/activate` con activationCode vacio | 200 OK con ticket trial |
| 3 | Heartbeat | `POST /api/license/heartbeat` con ticket valido | 200 OK con ticket renovado |
| 4 | Provisioning | `POST /api/provision` con activation code valido | 200 OK con dbName |
| 5 | Listar codigos | `.\generate-code.ps1` + `.\list-license-codes.ps1` | Codigo generado y visible |
| 6 | Generar license code | `.\generate-license-code.ps1` | Codigo generado OK |
| 7 | Listar licencias | `.\list-licenses.ps1` | Lista correcta |
| 8 | Modificar licencia | `.\modify-license.ps1 -LicenciaId 1 -MaxLegajos 50` | Licencia modificada |
| 9 | Suspender licencia | `.\suspend-license.ps1 -LicenciaId 1` | Licencia suspendida |
| 10 | Reactivar licencia | `.\suspend-license.ps1 -LicenciaId 1 -Reactivate` | Licencia reactivada |

### 4.2 DigitalPlusWeb (dp_web_svc)

| # | Prueba | Accion | Resultado esperado |
|---|---|---|---|
| 1 | Login | Iniciar sesion en la web | Acceso correcto |
| 2 | Dashboard | Ver dashboard | Datos cargados |
| 3 | Listado legajos | Navegar a legajos | Lista completa |
| 4 | Listado fichadas | Navegar a fichadas | Lista completa |
| 5 | Reportes | Ejecutar consolidado/control acceso | Datos correctos |
| 6 | Crear legajo | Agregar un legajo de prueba | Se guarda OK |
| 7 | Editar legajo | Modificar un legajo | Se guarda OK |

### 4.3 Verificacion de que sa ya no se usa en runtime

```sql
-- Ejecutar en Ferozo para ver conexiones activas
SELECT
    s.login_name,
    s.host_name,
    s.program_name,
    DB_NAME(s.database_id) AS [database],
    s.login_time
FROM sys.dm_exec_sessions s
WHERE s.is_user_process = 1
ORDER BY s.login_time DESC;
```

Resultado esperado: solo deben aparecer `dp_admin_svc` y `dp_web_svc` como logins activos. Si `sa` sigue apareciendo, identificar que componente lo usa y corregir.

---

## 5. CHECKLIST DE VALIDACION POST-CAMBIO

### Fase 0 (Backup)

- [ ] Backup de DigitalPlus realizado (017 o panel Ferozo)
- [ ] Backup de DigitalPlusAdmin realizado (017 o panel Ferozo)

### Fase 1 (Scripts SQL)

- [ ] Script 011 ejecutado sin errores en DigitalPlusAdmin
- [ ] Script 012 ejecutado sin errores (password reemplazado)
- [ ] Script 013 ejecutado sin errores en DigitalPlus
- [ ] Script 014 ejecutado sin errores (password reemplazado)
- [ ] Script 016 ejecutado -- todos los checks [OK]
- [ ] dp_admin_svc puede conectarse a DigitalPlusAdmin y hacer SELECT
- [ ] dp_web_svc puede conectarse a DigitalPlus y hacer SELECT
- [ ] Script 018 ejecutado con dp_admin_svc -- INSERT+ROLLBACK OK
- [ ] Script 018 ejecutado con dp_web_svc -- INSERT+ROLLBACK OK

### Fase 1.5 (Prueba local antes de produccion)

- [ ] Azure Functions ejecutadas localmente con dp_admin_svc -- OK
- [ ] DigitalPlusWeb ejecutada localmente con dp_web_svc -- OK
- [ ] Consultas, escritura, licencias y provisioning verificados

### Fase 2 (Actualizacion de configs)

- [ ] Azure App Settings actualizados (ProvisioningDb, CloudSqlUser, CloudSqlPassword)
- [ ] Azure Functions reiniciadas
- [ ] Health check retorna OK
- [ ] Activacion de licencia funciona
- [ ] Heartbeat funciona
- [ ] Scripts PowerShell actualizados (6 archivos)
- [ ] Scripts PowerShell probados (list-licenses, modify-license, suspend-license)
- [ ] Web appsettings.json actualizado
- [ ] Web funciona correctamente (login, dashboard, listados, reportes)
- [ ] Common App.config limpiado de strings legacy
- [ ] local.settings.json actualizado
- [ ] deployment-guide.md actualizado
- [ ] Verificado que sa NO aparece en conexiones activas de runtime

### Passwords guardados

- [ ] Password de dp_admin_svc guardado en lugar seguro (Azure App Settings)
- [ ] Password de dp_web_svc guardado en lugar seguro (web config en Ferozo)
- [ ] Passwords NO almacenados en archivos de texto plano en el repositorio

---

## 6. ROLLBACK

Si algo falla despues de actualizar las configuraciones:

1. **Revertir Azure App Settings** a sa/Soporte1
2. **Revertir web appsettings.json** a sa/Soporte1
3. **Revertir scripts PowerShell** a sa/Soporte1

Los logins y roles creados no afectan nada si no se usan. No es necesario eliminarlos para hacer rollback.

Si el problema es mas grave (datos corruptos), restaurar desde el backup del Paso 0.

---

## 7. MONITOREO POST-IMPLEMENTACION

Durante los primeros **7 dias** despues del cambio, ejecutar periodicamente `019_MonitorConnections.sql` para verificar que:

- Solo `dp_admin_svc` y `dp_web_svc` aparecen como logins activos
- `sa` NO tiene actividad reciente
- Si `sa` aparece, identificar inmediatamente que componente lo esta usando

---

## 8. POLITICA PARA EL LOGIN SA

- **Mantener sa activo** durante al menos 7 dias despues del cambio
- Confirmar que todas las funciones operan correctamente con los nuevos usuarios
- Solo despues de 7 dias sin incidentes considerar:
  - Cambiar password de sa a uno complejo (40+ caracteres)
  - O deshabilitar el login si el hosting lo permite
- **No apresurarse** a eliminar sa: es la red de seguridad durante la transicion

---

## 9. LO QUE NO SE HACE EN ESTA FASE

- NO se modifica el instalador (sera Fase 3, v1.4)
- NO se crea el portal admin (sera Fase 4)
- NO se migran instalaciones existentes
- NO se deshabilita sa (se hara despues de confirmar estabilidad durante 7+ dias)
