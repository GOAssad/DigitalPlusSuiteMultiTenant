# Autorización para Fase 2 – Producción

La prueba local quedó aprobada.

Podés proceder con la actualización de configuraciones de producción en el siguiente orden:

## 1. Azure App Settings

Actualizar en la Function App:

- ProvisioningDb
- CloudSqlUser
- CloudSqlPassword

Reemplazar credenciales `sa` por `dp_admin_svc`.

Luego reiniciar la Function App.

### Verificación inmediata
Probar en producción:

- /api/health
- /api/license/activate

Si ambos endpoints responden correctamente, continuar.

---

## 2. Scripts PowerShell

Actualizar los 6 archivos en:

AzureProvisioning/tools/

Cambiar defaults de usuario/password para usar:

- dp_admin_svc

No dejar credenciales viejas por defecto.

---

## 3. Web appsettings.json (producción)

Actualizar los connection strings de producción para que usen:

- dp_web_svc

Luego reiniciar el sitio web.

### Verificación inmediata
Confirmar en producción:

- login
- dashboard
- listado de legajos
- listado de fichadas

Si todo responde bien, continuar.

---

## 4. Monitoreo de conexiones

Ejecutar:

019_MonitorConnections.sql

Resultado esperado:

- dp_admin_svc
- dp_web_svc

Si aparece `sa`, identificar inmediatamente qué componente sigue usándolo.

---

## Regla de ejecución

No hacer todos los cambios y verificar al final.

Se debe avanzar por bloques:

1. Azure Functions
2. Verificar
3. Scripts PowerShell
4. Web producción
5. Verificar
6. Monitoreo

---

## Importante

- No tocar todavía el instalador
- No tocar todavía el portal admin
- No deshabilitar todavía `sa`
- No postergar verificaciones para después

Objetivo de esta fase:

- Azure Functions operando con `dp_admin_svc`
- Scripts administrativos actualizados
- DigitalPlusWeb operando con `dp_web_svc`
- `sa` fuera del runtime normal