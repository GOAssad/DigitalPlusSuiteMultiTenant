# v1.3 — Sistema de Licencias: Implementacion Completa

**Fecha:** 2026-03-04
**Estado:** Implementado, pendiente pruebas exhaustivas

---

## Resumen

Sistema de licencias freemium con ticket firmado RSA + cache DPAPI. Azure Functions es la autoridad central, emite tickets firmados. El cliente valida con clave publica embebida y cachea localmente.

**3 escenarios de bloqueo:**
1. Sin licencia / trial expirado
2. Licencia caducada (fecha de vencimiento superada)
3. Falta de pago (suspendida, gracia de 7 dias)

**Trial:** 14 dias, maximo 5 legajos. Configurable desde el servidor.

---

## Arquitectura

```
[App WinForms] → DigitalPlus.Licensing.dll → HTTP POST → [Azure Functions]
                         ↓                                      ↓
                   license.dat (DPAPI)              Tabla Licencias (Ferozo)
                   clock.dat (DPAPI)                Tabla LicenciasLog
                   machine.id
```

---

## Lo que se hizo (por fase)

### Fase A: Azure (servidor)

#### A1. SQL en Ferozo (ejecutados)
- `AzureProvisioning/sql/005_CreateLicenciasTable.sql` — Tabla `Licencias` (CompanyId, MachineId, LicenseType, Plan, MaxLegajos, fechas de trial/expiracion/suspension/gracia)
- `AzureProvisioning/sql/006_CreateLicenciasLogTable.sql` — Tabla `LicenciasLog` (auditoria)
- `AzureProvisioning/sql/007_SP_LicenseActivate.sql` — SP `License_Activate` (UPSERT con UPDLOCK)
- `AzureProvisioning/sql/008_SP_LicenseHeartbeat.sql` — SP `License_Heartbeat` (actualiza heartbeat, retorna estado)

#### A2. Par de claves RSA
- Script: `AzureProvisioning/tools/generate-license-keys.ps1`
- Clave privada (PFX base64): `AzureProvisioning/tools/license-private-key.txt` → configurada como App Setting `LicenseSigningKey` en Azure
- Clave publica (DER base64): `AzureProvisioning/tools/license-public-cert.txt` → embebida en `LicenseValidator.cs`
- Password PFX: `temp1234!` → App Setting `LicenseSigningKeyPassword` en Azure

#### A3. Modelos y servicio
- `src/DigitalPlus.Provisioning/Models/LicenseModels.cs` — Request/Response/Ticket
- `src/DigitalPlus.Provisioning/Services/LicenseService.cs` — ActivateAsync, HeartbeatAsync, SignTicket, firma RSA-SHA256

#### A4. Endpoints Azure Functions (deployed)
- `POST /api/license/activate` — `LicenseActivateFunction.cs`
- `POST /api/license/heartbeat` — `LicenseHeartbeatFunction.cs`
- Registrado en `Program.cs` como Singleton

Las 4 funciones estan habilitadas en Azure: Health, LicenseActivate, LicenseHeartbeat, Provision.

### Fase B: Libreria cliente — DigitalPlus.Licensing

**Ubicacion:** `Common\DigitalPlus.Licensing\` (.NET Framework 4.8, sin NuGet)

| Archivo | Responsabilidad |
|---|---|
| `LicenseTicket.cs` | Modelo del ticket (DataContract) |
| `LicenseStatus.cs` | Enum `LicenseState` + `LicenseValidationResult` con `IsBlocked` |
| `LicenseValidator.cs` | Verifica firma RSA con clave publica X509 embebida. Evalua reglas de bloqueo |
| `LicenseCache.cs` | Save/Load de `license.dat` cifrado con DPAPI Machine + entropy |
| `LicenseClient.cs` | HTTP POST a Azure (activate + heartbeat). TLS 1.2, timeout 15s |
| `MachineIdProvider.cs` | SHA256(MachineGuid + BiosSerial + ProcessorId), persiste en `machine.id` |
| `ClockGuard.cs` | Detecta reloj manipulado comparando con ultimo serverTimeUtc (DPAPI en `clock.dat`) |
| `LicenseManager.cs` | Facade: ValidateAtStartup(), PeriodicCheck(), ActivateWithCode() |
| `FrmLicenseBlocked.cs` | Form modal: mensaje + campo codigo activacion + Activar + Reintentar + Salir |
| `JsonHelper.cs` | Serialization con DataContractJsonSerializer (sin dependencia NuGet) |

#### Reglas de evaluacion (LicenseValidator.Evaluate):
| Estado | Condicion | Bloqueado? |
|---|---|---|
| TrialActive | Trial vigente | No |
| TrialExpired | TrialEndsAt pasado | SI |
| TrialLegajosExceeded | Legajos activos > MaxLegajos | SI (toda la app) |
| Valid | Licencia activa OK | No |
| Expired | ExpiresAt pasado | SI |
| Suspended | Dentro de gracia (7 dias) | No (warning) |
| SuspendedBlocked | Gracia vencida | SI |
| OfflineGrace | Sin heartbeat < 72h | No |
| OfflineBlocked | Sin heartbeat > 72h | SI |
| InvalidSignature | Ticket modificado | SI |
| ClockTampered | Reloj retrocede >1h | SI |

### Fase C: Integracion en apps WinForms

#### Fichador
- `TEntradaSalida\Program.cs` — Valida licencia antes de Application.Run. Si bloqueado muestra FrmLicenseBlocked.
- `TEntradaSalida\uAreu\FrmFichar.cs` — Acepta LicenseManager, timer cada 4 horas llama PeriodicCheck()
- `TEntradaSalida.csproj` — ProjectReference a DigitalPlus.Licensing

#### Administrador
- `Acceso\Program.cs` — Valida licencia antes de Application.Run. Si bloqueado muestra FrmLicenseBlocked.
- `Administrador.csproj` — ProjectReference a DigitalPlus.Licensing

#### Config templates actualizados
- `fichador.app.config.template` — agregado `ProvisioningApiKey`
- `administrador.app.config.template` — agregado `ProvisioningApiKey`

### Fase D: Instalador v1.3

- `setup.iss` — Version bump a 1.3
- Agregado `DigitalPlus.Licensing.dll` en [Files] para Fichadas y Administrador
- Agregado reemplazo `{{PROVISIONING_API_KEY}}` en EscribirConfigFichador y EscribirConfigAdministrador

### Compilacion y deploy
- Fichador: compila OK (0 errores, 0 warnings)
- Administrador: compila OK (0 errores, 0 warnings)
- MSBuild usado: `C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe`
- Azure Functions: deployed y 4 funciones habilitadas
- SQL: ejecutado en Ferozo con `run-all.ps1`
- App Settings en Azure: `LicenseSigningKey` y `LicenseSigningKeyPassword` configurados
- Instalacion limpia local: probada y funciona

---

## Prueba pendiente: limite de 5 legajos

Verificar que al llegar a 5 legajos en trial, la app se bloquea y no permite mas operaciones. El bloqueo es de TODA la app, no solo del alta de legajos.

**Como probar:**
1. Instalar limpio (trial se activa automaticamente)
2. Dar de alta 5 legajos desde el Administrador
3. Intentar dar de alta el 6to legajo
4. Verificar que la app se bloquea al siguiente PeriodicCheck o ValidateAtStartup

**IMPORTANTE:** Actualmente el conteo de legajos se pasa como parametro `currentLegajos` en ValidateAtStartup() y PeriodicCheck(). En Program.cs de ambas apps se llama con 0 (default). Para que funcione el bloqueo por legajos, hay que pasar el conteo real de legajos activos. Esto requiere:
- Contar legajos activos desde la BD antes de llamar ValidateAtStartup
- O hacer el conteo dentro del timer de 4 horas en FrmFichar

---

## Flujo de activacion de licencia (para el usuario final)

### Escenario: usuario adquiere licencia

1. El proveedor genera un **codigo de activacion** en Ferozo (tabla `ActivationCodes` via `generate-code.ps1`)
2. Se lo envia al cliente (email, WhatsApp, etc.)
3. El usuario ejecuta la app → si esta bloqueado, aparece `FrmLicenseBlocked`
4. Ingresa el codigo en el campo "Codigo de activacion" y presiona "Activar"
5. La app llama `POST /api/license/activate` con el codigo
6. Azure valida el codigo, crea/actualiza la licencia como "active"
7. El ticket firmado se guarda en cache local
8. La app se desbloquea

### PENDIENTE: Crear una forma de generar codigos de activacion de LICENCIA

Actualmente `generate-code.ps1` genera codigos para **activacion del instalador** (tabla `ActivationCodes`).
Se necesita un mecanismo similar para **licencias**:
- Un codigo que al usarse en `License_Activate` cambie la licencia de trial a active
- El SP `License_Activate` ya acepta `@ActivationCode` — si no es NULL, crea licencia activa
- Falta definir: como se validan esos codigos? Tabla aparte? El mismo `ActivationCodes`?

---

## Que sigue (lista de pendientes)

### Inmediato (para que funcione el bloqueo por legajos)
1. **Pasar conteo real de legajos** en ValidateAtStartup y PeriodicCheck — actualmente se pasa 0
2. **Probar limite de 5 legajos** — verificar bloqueo real

### Corto plazo
3. **Definir mecanismo de codigos de activacion de licencia** — como genera el proveedor un codigo que convierta trial en activo
4. **Panel de administracion de licencias** — donde el proveedor puede ver/modificar licencias, cambiar MaxLegajos, suspender por falta de pago
5. **Probar todos los escenarios** de la tabla de testing (14 escenarios)

### Mediano plazo
6. **Mostrar dias restantes de trial** — actualmente no se muestra en la UI (solo se bloquea cuando expira)
7. **Warning de suspension** — mostrar aviso cuando esta en periodo de gracia
8. **Probar el instalador compilado** con Inno Setup en entorno limpio
