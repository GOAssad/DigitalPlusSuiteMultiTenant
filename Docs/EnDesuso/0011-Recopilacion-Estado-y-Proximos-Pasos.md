# Recopilacion de Estado del Proyecto — 2026-03-04

## Indice

1. [Estado general de cada sistema](#1-estado-general-de-cada-sistema)
2. [Lo que se hizo (resumen cronologico)](#2-lo-que-se-hizo)
3. [Instalador v1.2 — detalle tecnico](#3-instalador-v12)
4. [Azure Provisioning — estado](#4-azure-provisioning)
5. [Lo que falta por hacer](#5-lo-que-falta-por-hacer)
6. [v1.3 — Sistema de Licencias (proximo paso principal)](#6-v13--sistema-de-licencias)

---

## 1. Estado general de cada sistema

| Sistema | Stack | Estado | Ruta |
|---|---|---|---|
| **Fichador** | WinForms .NET Framework | ✅ Funcionando, compilado, instalador OK | `DigitalOnePlus\Fichador\` |
| **Administrador** | WinForms .NET Framework | ⚠️ Codigo adaptado a DigitalPlus, pendiente compilar | `DigitalOnePlus\Administrador\Acceso\` |
| **DigitalPlusWeb** | Blazor Server .NET 7 | ✅ En produccion (Ferozo) | `DigitalPlusWeb_Claude\DigitalPlus\` |
| **Instalador Unificado** | Inno Setup 6.x | ✅ v1.2 funcional (Local + Nube) | `DigitalOnePlus\InstaladorUnificado\setup.iss` |
| **Azure Provisioning** | Azure Functions .NET 8 Isolated | ✅ Desplegado y testeado | `DigitalOnePlus\AzureProvisioning\` |

---

## 2. Lo que se hizo

### Sesion 2026-02-28
- Diagnostico inicial del Fichador
- Fix de archivos obj/ que no se regeneraban

### Sesion 2026-03-01
- Script SQL completo `DigitalPlus_Script.sql` (24 tablas, SPs, Identity, datos default)
- Adaptacion completa del Administrador a schema DigitalPlus (41 archivos Designer + ediciones manuales)
- Fixes criticos: alta de legajos con huellas, stubbing de tablas inexistentes
- Nombre de empresa en FrmFichar, fix de connection string "Local"
- Instalador v1.1

### Sesion 2026-03-03
- **Hardening DPAPI**: eliminacion de credenciales hardcodeadas, ConfigProtector.exe
- ACL NTFS restrictiva (Admins=Full, SYSTEM=Full, Users=RX)
- Limpieza de connection strings en todo el codigo

### Sesion 2026-03-04 (hoy)
- **Azure Provisioning**: tabla ActivationCodes en Ferozo, 3 SPs, Azure Functions desplegado
- **Instalador v1.2**: reescritura completa con:
  - SQL Express 2019 incluido (~249MB) para instalacion offline
  - Windows Authentication (sin sa/password)
  - Deteccion dinamica de instancias SQL (SQLEXPRESS, MSSQLSERVER, otras)
  - Modo Local vs Nube con pagina de seleccion
  - Modo Nube: codigo de activacion + llamada HTTP a Azure provisioning
  - Deteccion de instalacion existente (3 AppIds + archivos en disco)
  - Nombre de BD sanitizado segun empresa (ej: "Mi Empresa" → `DP_Mi_Empresa`)
  - No borra BD existente (exit 0 si ya existe)
  - Forward declarations corregidas para Pascal/Inno Setup

---

## 3. Instalador v1.2

### Archivo: `InstaladorUnificado\setup.iss`

### Paginas del wizard
1. Bienvenida (si detecta instalacion previa, pregunta si continuar)
2. Nombre de empresa
3. URL del portal web
4. **Modo de instalacion** (Local / Nube) — NUEVO en v1.2
5. **Codigo de activacion** (solo modo Nube) — NUEVO en v1.2
6. Directorio de instalacion
7. Opciones (accesos directos, auto-inicio)

### Flujo post-install MODO LOCAL
1. Sanitizar nombre empresa → `sNombreBD` (ej: `DP_Mi_Empresa`)
2. Detectar SQL Server (registry) → setea `sDataSource` dinamicamente
3. Si no hay SQL → instalar SQL Express 2019 silenciosamente (3-10 min)
4. Crear BD con nombre sanitizado (si no existe)
5. Escribir configs (Windows Auth, `Integrated Security=True`)
6. Registrar terminal en GRALTerminales
7. Bootstrap Identity (usuarios admin/user)
8. Cifrar connectionStrings con DPAPI
9. Aplicar ACL NTFS

### Flujo post-install MODO NUBE
1. Connection string ya obtenida del Azure provisioning
2. Escribir configs con connection string remota
3. Cifrar connectionStrings con DPAPI
4. Aplicar ACL NTFS

### Variables clave
- `sNombreBD`: nombre de BD sanitizado (ej: `DP_Mi_Empresa`)
- `sDataSource`: fuente SQL detectada (`.` para default, `.\SQLEXPRESS` para named)
- `sNombreInstancia`: nombre de instancia SQL detectada
- `bModoLocal`: true=local, false=nube
- `sCloudConnStr`: connection string de Azure (modo nube)
- `bActivacionOK`: codigo validado contra Azure

### Prerequisitos incluidos
- `Prerequisites\SQLEXPR_x64_ENU.exe` (~249MB, SQL Express 2019)
- `tools\ConfigProtector\ConfigProtector.exe` (cifrado DPAPI)

### AppIds registrados (para deteccion de instalacion previa)
- Unificado: `{D5E6F7A8-1B2C-3D4E-5F6A-7B8C9D0E1F2A}`
- Fichador individual: `{B3F2A1C0-4D5E-6F7A-8B9C-0D1E2F3A4B5C}`
- Administrador individual: `{A4C1B2D3-E5F6-7A8B-9C0D-1E2F3A4B5C6D}`

---

## 4. Azure Provisioning

### Estado: ✅ Desplegado y funcional

| Recurso | Valor |
|---|---|
| URL | `https://digitalplus-provision.azurewebsites.net/api/` |
| Resource Group | `rg-digitalplus-provision` (brazilsouth) |
| Storage Account | `stdigitalplusprv` |
| Function App | `digitalplus-provision` |
| API Key | `_coa6vaAjo3cCIio9j-VnrwPSb2qQuxBF8fa2E5XEUM` |
| Cuenta Azure | `gassad@live.com.ar` (MFA, usar `--use-device-code`) |

### Endpoints
- `POST /api/provision` — valida activation code, retorna connection string
- `GET /api/health` — health check contra Ferozo

### Tabla ActivationCodes en Ferozo
- Campos: `Code`, `CompanyName`, `IsUsed`, `UsedAt`, `MachineId`, `InstallType`, `CreatedAt`, `ExpiresAt`
- SPs: `ActivationCodes_Validate`, `ActivationCodes_MarkUsed`, `ActivationCodes_Insert`
- Generador: `generate-code.ps1`

---

## 5. Lo que falta por hacer

### Prioridad ALTA — Proximo paso

#### 5.1 Sistema de Licencias (v1.3) — VER SECCION 6

#### 5.2 Compilar Administrador
- Todo el codigo fue adaptado a DigitalPlus pero NO fue compilado
- Bloqueante: MSComCtl2.OCX (COM reference) — registrar con `regsvr32` o eliminar
- SPs faltantes en DigitalPlus para funcionalidad completa:
  - `RRHHFichadas_SP_LISTADO` (reportes)
  - `RRHHFichadas_SP_MANUAL`, `RRHHFichadas_SP_MANUAL_SELECT_GRUPO` (fichadas manuales)
  - `RRHHIncidenciasLegajos*` (incidencias)

### Prioridad MEDIA

#### 5.3 Ampliar DigitalPlusWeb con reportes (Etapa 4)
- La web esta en produccion pero faltan reportes avanzados
- Pendiente definir alcance

#### 5.4 Actualizar documentacion del instalador
- `0009-Instalador-v1.2-SQLExpress-WindowsAuth.md` necesita actualizarse con:
  - Modo Local/Nube
  - Codigo de activacion
  - BD con nombre sanitizado
  - Deteccion de instalacion existente

### Prioridad BAJA

#### 5.5 Limpieza de dead code en Administrador
- `RRHHFichadasDao.obtenerFichadas()` tiene SQL viejo (no se llama)
- `GRALPaises` no existe — soft-fail actual es aceptable

---

## 6. v1.3 — Sistema de Licencias (PROXIMO PASO PRINCIPAL)

### Objetivo
Controlar el uso del software con un sistema de licencias que pueda suspender la operacion del sistema en tres escenarios:

### Escenarios de suspension

| # | Escenario | Descripcion | Ejemplo |
|---|---|---|---|
| 1 | **Sin licencia** | El cliente nunca activo una licencia | Trial expirado o nunca ingreso codigo |
| 2 | **Licencia caducada** | La fecha de validez de la licencia expiro | Licencia valida hasta 2026-06-30, hoy es 2026-07-01 |
| 3 | **Falta de pago** | Licencia activa pero abono mensual no pagado | Licencia OK hasta 2026-12-31 pero ultimo pago fue febrero |

### Comportamiento esperado
- Cuando se cumple CUALQUIERA de los 3 escenarios → **el sistema se suspende**
- "Suspender" = las apps (Fichador y Administrador) NO permiten operar
- Debe mostrar un mensaje claro indicando la razon y como resolverlo
- Posiblemente permitir un "modo gracia" de X dias antes del corte total

### Limite de transacciones (freemium)
- Si NO tiene licencia, puede operar hasta cierta cantidad de transacciones (fichadas)
- Superado el limite → se suspende y pide activar licencia
- Esto permite que el cliente pruebe el sistema antes de comprar

### Consideraciones tecnicas a resolver

#### 6.1 Donde almacenar la licencia
- **Opcion A**: Solo local (archivo cifrado + registro) — mas simple, offline
- **Opcion B**: Validacion online contra Azure — mas seguro, requiere internet
- **Opcion C**: Hibrido — licencia local con heartbeat periodico a Azure

#### 6.2 Que contiene una licencia
- Codigo de activacion / serial
- Nombre de empresa
- Fecha de emision
- Fecha de vencimiento
- Tipo de plan (trial, mensual, anual)
- Limite de transacciones (si aplica)
- MachineId (para atar al equipo)
- Estado de pago (ultimo pago confirmado)

#### 6.3 Como verificar el pago
- El Azure Function actual (`/api/provision`) solo valida activation codes
- Se necesita un nuevo endpoint o extension para verificar estado de pago
- Posibles fuentes de pago: Mercado Pago, transferencia bancaria, manual
- Opcion simple: el admin marca "pagado" desde un panel y el sistema consulta

#### 6.4 Donde verificar en las apps
- **Fichador**: al iniciar la app y periodicamente durante el uso
- **Administrador**: al iniciar la app
- **Web**: al iniciar sesion (si aplica licencia al modulo web)

#### 6.5 Tabla(s) nuevas en BD
```
Licencias
  - Id (int, PK)
  - CodigoActivacion (nvarchar)
  - Empresa (nvarchar)
  - FechaEmision (datetime)
  - FechaVencimiento (datetime)
  - TipoPlan (nvarchar: trial/mensual/anual)
  - LimiteTransacciones (int, null = ilimitado)
  - MachineId (nvarchar)
  - Estado (nvarchar: activa/suspendida/caducada)
  - UltimoPagoConfirmado (datetime, null)
  - ProximoPagoEsperado (datetime, null)

LicenciasLog
  - Id (int, PK)
  - LicenciaId (FK)
  - Evento (nvarchar: activada/renovada/suspendida/pagada)
  - Fecha (datetime)
  - Detalle (nvarchar)
```

#### 6.6 Interaccion con sistemas existentes
- El instalador v1.2 ya tiene modo Local/Nube y activation codes
- Azure Provisioning ya valida codes → se puede extender para licencias
- Las apps ya leen connection string de .config → agregar lectura de licencia
- DigitalPlusWeb podria tener un panel de administracion de licencias

### Flujo propuesto (alto nivel)

```
INSTALACION:
  1. Cliente instala con v1.2 (Local o Nube)
  2. Si no tiene codigo → modo trial (N fichadas gratis)
  3. Si tiene codigo → activa licencia completa

OPERACION DIARIA:
  1. App inicia → lee licencia local
  2. Verifica: ¿licencia valida? ¿no caducada? ¿pago al dia?
  3. Si todo OK → opera normalmente
  4. Si algo falla → muestra mensaje y bloquea operacion
  5. (Opcional) heartbeat periodico a Azure para sincronizar estado

RENOVACION/PAGO:
  1. Admin marca pago en panel web (o se confirma automaticamente)
  2. Azure actualiza estado de licencia
  3. Proximo heartbeat de la app detecta el pago y desbloquea
```

### Decisiones pendientes para la proxima sesion
1. ¿Hibrido (local + Azure heartbeat) o solo local?
2. ¿Cuantas fichadas gratis en modo trial?
3. ¿Cuantos dias de gracia antes del corte por falta de pago?
4. ¿El modulo web tambien tiene licencia o solo escritorio?
5. ¿Como se confirma el pago? (automatico via Mercado Pago, manual, etc.)
6. ¿La licencia se ata a un equipo (MachineId) o a una empresa?

---

## Archivos de documentacion existentes

| # | Archivo | Contenido |
|---|---|---|
| 0001 | `0001_Diagnostico_Inicial.md` | Diagnostico inicial del Fichador |
| 0002 | `0002_Objetivo_Instalador.md` | Objetivo del instalador unificado |
| 0003 | `0003_Adaptacion_a_Base_de_Datos.md` | Adaptacion del Administrador a DigitalPlus |
| 0004 | `0004-Unificacion-Legajo-Registro.md` | Unificacion de legajo y registro |
| 0005 | `0005-Instaladores.md` | Instaladores individuales y unificado |
| 0006 | `0006-ClaudeInit.md` | Inicializacion de Claude |
| 0007 | `0007-Manual-Instalacion-Usuario.md` | Manual de instalacion para usuario |
| 0008 | `0008-DPAPI-Hardening-Implementacion.md` | Implementacion DPAPI + ACL |
| 0009 | `0009-Instalador-v1.2-SQLExpress-WindowsAuth.md` | Instalador v1.2 (necesita actualizacion) |
| 0010 | `0010-ImplementarEndPont_Azure_Instalacion.md` | Azure provisioning endpoint |
| 0011 | `0011-Recopilacion-Estado-y-Proximos-Pasos.md` | **ESTE ARCHIVO** |
