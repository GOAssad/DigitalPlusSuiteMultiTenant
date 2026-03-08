# DIGITALPLUS — ARQUITECTURA ACTUAL DEL SISTEMA

**Fecha:** 2026-03-05
**Generado por:** Claude (Opus 4.6)
**Propósito:** Documentación completa para Project Leader

---

## 1. VISIÓN GENERAL DEL PROYECTO

### Qué es DigitalPlus

DigitalPlus es un **sistema de control de accesos y gestión de personal** basado en reconocimiento biométrico (huella digital). Permite registrar ingresos y egresos del personal, administrar legajos, gestionar horarios, y visualizar información de asistencia a través de una plataforma web.

### Qué problema resuelve

- Control de asistencia del personal mediante huella dactilar
- Registro automático de fichadas (entrada/salida)
- Gestión centralizada de legajos, sucursales, categorías y horarios
- Reportes de horas trabajadas, llegadas tarde, horas extras
- Administración remota vía web

### Componentes principales

El ecosistema DigitalPlus está compuesto por **3 sistemas + infraestructura cloud**:

```
┌──────────────┐   ┌──────────────┐   ┌──────────────────┐
│  Fichador    │   │Administrador │   │  DigitalPlusWeb  │
│  (WinForms)  │   │  (WinForms)  │   │  (Blazor Server) │
└──────┬───────┘   └──────┬───────┘   └────────┬─────────┘
       │                  │                     │
       └──────────┬───────┘                     │
                  │                             │
         ┌────────▼────────┐          ┌─────────▼──────────┐
         │  BD Local       │          │  BD Ferozo         │
         │  (SQL Express)  │          │  (SQL Server)      │
         │  DP_Empresa     │          │  DigitalPlus       │
         └─────────────────┘          └────────────────────┘

         ┌──────────────────────────────────────────────┐
         │        Azure Functions (Provisioning)        │
         │  - Activación de instalaciones               │
         │  - Sistema de licencias                      │
         │  BD: DigitalPlusAdmin (Ferozo)               │
         └──────────────────────────────────────────────┘
```

### Tecnologías utilizadas

| Componente | Tecnología |
|---|---|
| Fichador | C# WinForms (.NET Framework 4.8) |
| Administrador | C# WinForms (.NET Framework 4.8) |
| Web | Blazor Server (.NET Core) + EF Core + Dapper |
| Azure Functions | .NET 8 Isolated |
| Base de datos | SQL Server / SQL Express |
| Biometría | DigitalPersona SDK (DPFP) |
| Instalador | Inno Setup (Pascal) |
| Control de versiones | Git + GitHub (privado) |

---

## 2. ARQUITECTURA GENERAL DEL SISTEMA

### 2.1 Administrador

**Ruta:** `DigitalOnePlus\Administrador\Acceso\`
**Ejecutable:** `Acceso.exe`
**Estado:** Compila OK, adaptado a schema DigitalPlus

**Funciones:**
- Gestión completa de legajos (alta, baja, modificación)
- Registro y administración de huellas digitales (DigitalPersona SDK)
- Gestión de sucursales, categorías, horarios, terminales
- Administración de usuarios del sistema
- Fichadas manuales
- Reportes de fichadas y horas trabajadas
- Gestión de incidencias

**Estructura del proyecto:**
```
Administrador\Acceso\
├── Program.cs              → Punto de entrada + validación de licencia
├── Generales\              → FrmMainMenu, FrmGRALTerminales, etc.
├── RRHH\                   → FrmRRHHLegajos (ABM legajos principal)
├── Reportes\               → FrmRptFichadas, FrmRptHorasTrabajadas
├── uAreu\                  → Integración DigitalPersona (enrollment, verificación)
├── ControlEntidad\         → Controles reutilizables de entidades
├── Properties\             → AssemblyInfo, Resources, Settings
└── copias\, Copia\         → Código legacy (no activo)
```

**Dependencias:**
- `Common\Acceso.Clases.Datos\` — Clases de acceso a datos (RRHHLegajos, RRHHFichadas, etc.)
- `Common\Global.Datos\` — SQLServer.cs (helper estático de ejecución SQL)
- `Common\Global.Funciones\` — Utilidades generales
- `Common\DigitalPlus.Licensing\` — Librería de licencias
- DigitalPersona SDK (DPFP.dll)
- SpreadsheetLight, DocumentFormat.OpenXml (exportación Excel)

**Interacción con BD:** Usa `SQLServer.Ejecutar()` (ADO.NET directo) y SPs. Connection string se lee de `ConnectionStrings["Local"]` en el .exe.config, cifrado con DPAPI.

**Licencias:** Valida al startup (`Program.LicMgr.ValidateAtStartup(legajos)`). Bloquea alta de legajos si se supera el límite de la licencia.

---

### 2.2 Fichador

**Ruta:** `DigitalOnePlus\Fichador\TEntradaSalida\`
**Ejecutable:** `TEntradaSalida.exe`
**Estado:** Funcionando en producción

**Funciones:**
- Interfaz de fichado para el personal
- Lectura de huella digital y verificación contra BD
- Registro de fichada (entrada/salida) con timestamp
- Pantalla de bienvenida con nombre del empleado

**Estructura del proyecto:**
```
Fichador\TEntradaSalida\
├── Program.cs              → Punto de entrada + validación de licencia
├── uAreu\
│   ├── FrmFichar.cs        → Pantalla principal de fichado + timer licencia (4h)
│   └── EnrollmentForm.cs   → Registro de huellas (compartido con Administrador)
├── documentacionClaude\    → Documentación técnica generada
└── Installer\              → Instalador individual (legacy, reemplazado por unificado)
```

**Flujo de fichado:**
1. El lector biométrico captura la huella
2. Se compara contra todas las huellas almacenadas en `LegajosHuellas`
3. Si hay match, se identifica el legajo
4. Se llama al SP `EscritorioFichadasSPSALIDA` que determina si es entrada o salida
5. Se registra la fichada en la tabla `Fichadas`
6. Se muestra mensaje de bienvenida/despedida

**Licencias:** Valida al startup y cada 4 horas via timer. Si la licencia se bloquea, cierra la aplicación.

---

### 2.3 DigitalPlusWeb

**Ruta:** `C:\Apps\Claude\Huellas\DigitalPlusWeb_Claude\DigitalPlus\`
**Estado:** En producción (Ferozo)

**Funciones:**
- Dashboard con minutos trabajados por legajo y sucursal
- Control de acceso (listado de fichadas)
- Reportes: consolidado, horas extras, llegadas tarde
- Gestión de legajos, sucursales, horarios, categorías, sectores
- Autenticación con ASP.NET Identity

**Arquitectura:**
- Blazor Server (.NET Core)
- Entity Framework Core + Dapper (híbrido)
- `ApplicationDbContext` hereda de `IdentityDbContext`
- 23 DbSets, 92 componentes Razor
- Patrón repositorio: `RepositorioXxx` inyectado como Transient
- 9 repositorios EF puro + 5 híbridos (EF+Dapper para SPs complejos)

**SPs que consume:**
- `WebConsolidado_Listado`
- `WebControlAcceso_Listado`
- `WebHorasExtras_Listado`
- `WebLlegadaTarde_Listado`
- `WebDashBoardMinutosTrabajadosMensualesPorLegajo`
- `WebDashBoardMinutosTrabajadosMensualesPorSucursal`
- `WebCalculoMinutosMensualesCalendarioPorLegajo`

**Nota:** La web es la **referencia canónica** del schema DigitalPlus. No se puede cambiar la estructura de la BD sin coordinar con este sistema.

---

## 3. BASE DE DATOS

### 3.1 Bases de datos del sistema

| BD | Ubicación | Propósito |
|---|---|---|
| `DigitalPlus` | Ferozo (producción) | BD del primer cliente + app web |
| `DP_NombreEmpresa` | Local (SQL Express) | BD de cada cliente desktop |
| `DigitalPlusAdmin` | Ferozo | BD administrativa multi-tenant (licencias, activation codes) |

**Servidor Ferozo:** `sd-1985882-l.ferozo.com, 11434`
**Servidor local:** SQL Express (instancia SQLEXPRESS)

### 3.2 Schema DigitalPlus — Tablas principales

| Tabla | Descripción |
|---|---|
| `Legajos` | Datos del personal (Id, LegajoId, Nombre, Sector, Categoria, Activo, Foto) |
| `LegajosHuellas` | Huellas digitales vinculadas a cada legajo (LegajoId, DedoId, Huella) |
| `Fichadas` | Registros de entrada/salida (Registro, LegajoId, Tipo, Terminal) |
| `Horarios` | Definición de horarios laborales |
| `Categorias` | Categorías de personal |
| `Sectores` | Sectores/ubicaciones (antes RRHHUbicaciones) |
| `Sucursales` | Sucursales de la empresa (CodigoSucursal) |
| `LegajosSucursales` | Relación N:N legajo-sucursal |
| `Terminales` | Terminales/dispositivos de fichado |
| `Incidencias` | Tipos de incidencia |
| `IncidenciasLegajos` | Incidencias asignadas a legajos |
| `VariablesGlobales` | Configuración del sistema (sId, Valor) |
| `GRALUsuarios` | Usuarios del sistema administrador |

### 3.3 Schema DigitalPlusAdmin — Tablas administrativas

| Tabla | Descripción |
|---|---|
| `ActivationCodes` | Códigos de activación del instalador (CodeHash, ExpiresAt, UsedAt) |
| `Licencias` | Estado de licencia por empresa+máquina (Plan, MaxLegajos, ExpiresAt) |
| `LicenciasLog` | Auditoría de eventos de licencia (activate, heartbeat, suspend) |
| `LicenseCodes` | Códigos de activación de licencia (Plan, MaxLegajos, DurationDays) |

### 3.4 Stored Procedures principales

**Escritorio (en cada BD cliente):**
- `EscritorioFichadasSPSALIDA` — Registra fichada y determina entrada/salida
- `EscritorioLegajoActualizar` — Inserta/actualiza legajo
- `EscritorioLegajosHuellasActualizar` — Inserta/actualiza huella

**Web (en BD Ferozo/DigitalPlus):**
- `WebConsolidado_Listado`, `WebControlAcceso_Listado`, `WebHorasExtras_Listado`, `WebLlegadaTarde_Listado`
- `WebDashBoardMinutosTrabajadosMensualesPorLegajo`, `WebDashBoardMinutosTrabajadosMensualesPorSucursal`

**Licencias (en DigitalPlusAdmin):**
- `License_Activate` — Crea o upgradea licencia
- `License_Heartbeat` — Renueva heartbeat, retorna estado
- `License_ValidateAndConsumeCode` — Valida y consume código de licencia
- `Provisioning_ValidateAndConsumeCode` — Valida código del instalador
- `Provisioning_InsertActivationCode` — Inserta código de instalador
- `Provisioning_ListActivationCodes` — Lista códigos de instalador

### 3.5 Mapping DigitalOne → DigitalPlus

El Administrador y Fichador fueron originalmente construidos para **DigitalOne** (sistema anterior). Se adaptaron al schema DigitalPlus:

| Tabla DigitalOne | Tabla DigitalPlus |
|---|---|
| `RRHHLegajos` | `Legajos` |
| `RRHHLegajosHuellas` | `LegajosHuellas` |
| `RRHHFichadas` | `Fichadas` |
| `RRHHHorarios` | `Horarios` |
| `RRHHCategorias` | `Categorias` |
| `RRHHUbicaciones` | `Sectores` |
| `GralSucursales` | `Sucursales` |
| `GralTerminales` | `Terminales` |
| `GRALVariablesGlobales` | `VariablesGlobales` |

---

## 4. SISTEMA DE LICENCIAS

### 4.1 Objetivo

Controlar el uso del software por cliente: limitar legajos, definir planes, manejar períodos de prueba, y poder suspender/reactivar remotamente.

### 4.2 Arquitectura

```
Cliente (Fichador/Administrador)
    │
    │  HTTP POST (TLS 1.2)
    │  Header: X-Api-Key
    │
    ▼
Azure Functions (digitalplus-provision.azurewebsites.net)
    │
    │  SqlConnection
    │
    ▼
Ferozo / DigitalPlusAdmin
    (Licencias, LicenciasLog, LicenseCodes)
```

Los clientes **nunca acceden directamente** a las tablas de licencias. Solo llaman endpoints HTTP. Las respuestas (tickets) están **firmadas con RSA** para evitar manipulación.

### 4.3 Endpoints

| Endpoint | Método | Función |
|---|---|---|
| `POST /api/license/activate` | Crear trial o activar con código | Retorna ticket firmado |
| `POST /api/license/heartbeat` | Renovar ticket, reportar legajos | Retorna ticket actualizado |
| `GET /api/health` | Health check | Status + timestamp |

### 4.4 Flujo de validación

**Al iniciar la app:**
1. Lee cache local (`license.dat`) cifrado con DPAPI
2. Verifica firma RSA del ticket
3. Verifica integridad del reloj (anti-tampering)
4. Intenta heartbeat al servidor para renovar ticket
5. Evalúa reglas: tipo, expiración, legajos, suspensión

**Cada 4 horas (Fichador):**
- Timer ejecuta heartbeat + revalidación
- Si la licencia se bloquea, cierra la app

**Al dar de alta un legajo (Administrador):**
- Cuenta legajos actuales (`SELECT COUNT(*) FROM Legajos`)
- Compara contra `MaxLegajos` del ticket
- Si supera el límite, bloquea el guardado con mensaje

### 4.5 Estados de licencia

#### Trial (modo prueba)
- Se crea automáticamente en la primera ejecución
- Plan: `free`, MaxLegajos: **5**, Duración: **14 días**
- Sin código de activación necesario

#### Licencia activa
- Se activa con un código generado por el administrador
- Plan, MaxLegajos y duración configurables por código
- Planes disponibles: `basic`, `premium` (extensible)

#### Licencia vencida
- Trial: pasados los 14 días, la app muestra pantalla de bloqueo
- Activa: pasada la fecha de expiración, se bloquea
- El usuario puede ingresar un nuevo código de activación para desbloquear

#### Licencia suspendida
- Suspendida remotamente por el administrador
- Período de gracia: **7 días** antes del bloqueo total
- Puede ser reactivada remotamente

#### Exceso de legajos
- Si `currentLegajos > MaxLegajos`, bloquea alta de nuevos legajos
- No bloquea el uso normal de la app (fichado sigue funcionando)

### 4.6 Códigos de licencia

Se generan con `generate-license-code.ps1`:
- Formato: `XXXX-XXXX-XXXX-XXXX`
- Se almacena solo el hash SHA256 en `LicenseCodes`
- Un solo uso
- Tienen expiración configurable (por defecto 7 días para usar el código)
- Definen: Plan, MaxLegajos, DurationDays

### 4.7 Scripts de administración

| Script | Función |
|---|---|
| `generate-license-code.ps1` | Genera código con plan/legajos/duración |
| `list-licenses.ps1` | Lista licencias (filtro por empresa, estado) |
| `list-license-codes.ps1` | Lista códigos generados (usados/disponibles) |
| `modify-license.ps1` | Cambia plan, legajos o extiende expiración |
| `suspend-license.ps1` | Suspende o reactiva una licencia |
| `generate-code.ps1` | Genera código de activación del instalador |

---

## 5. SISTEMA DE INSTALACIÓN

### 5.1 Decisión de unificación

Se unificaron los instaladores de Fichador y Administrador en un solo instalador (`setup.iss` con Inno Setup) porque ambos comparten dependencias (Common, DigitalPersona SDK) y la misma BD.

### 5.2 Versión actual: v1.3

**Ruta:** `DigitalOnePlus\InstaladorUnificado\setup.iss`

### 5.3 Qué instala

- **DigitalPlus Fichadas** (`TEntradaSalida.exe`)
- **DigitalPlus Administrador** (`Acceso.exe`)
- **SQL Express 2019** (si no está instalado, ~249MB, instalación silenciosa)
- **ConfigProtector.exe** (cifrado DPAPI de connection strings)
- **DigitalPlus.Licensing.dll** (librería de licencias)

### 5.4 Flujo del instalador

1. **Detección de instalación previa** — Si ya está instalado, ofrece reinstalar
2. **Selección de modo:**
   - **Local:** Instala SQL Express + crea BD local
   - **Nube:** Pide código de activación → provisioning Azure → connection string remota
3. **Datos solicitados al usuario:**
   - Nombre de empresa
   - URL del portal web DigitalPlus (opcional)
   - Código de activación (modo nube)
   - Carpeta de instalación (default: `C:\DigitalPlus`)
4. **Modo Local:**
   - Detecta instancias SQL (SQLEXPRESS > MSSQLSERVER > primera encontrada)
   - Crea BD con nombre sanitizado: "Mi Empresa" → `DP_Mi_Empresa`
   - Si la BD ya existe, no la borra
   - Ejecuta script SQL para crear tablas
   - Usa Windows Auth (`Integrated Security=True`) — sin usuario/password SA
5. **Modo Nube:**
   - Valida código de activación contra Azure Functions (`POST /api/provision`)
   - Recibe connection string de la BD cloud
6. **Post-instalación:**
   - Genera connection strings en los .exe.config
   - Ejecuta ConfigProtector.exe para cifrar con DPAPI
   - Aplica ACL NTFS (Admins=Full, SYSTEM=Full, Users=RX)
   - Crea accesos directos (escritorio + menú inicio)

### 5.5 Seguridad del instalador

- **DPAPI Machine-level:** Connection strings cifradas en el .exe.config
- **ACL NTFS:** Los .config solo son legibles por el usuario (no modificables)
- **No hay credenciales hardcodeadas:** Se eliminó `CadenaporConfiguracion()` con sa/Soporte1
- **Códigos de activación:** Hash SHA256, un solo uso, con expiración

---

## 6. CONFIGURACIÓN DEL SISTEMA

### 6.1 Archivos de configuración

| Archivo | Ubicación | Contenido |
|---|---|---|
| `TEntradaSalida.exe.config` | `{app}\Fichadas\` | ConnectionString "Local", ProvisioningApiKey, NombreEmpresa |
| `Acceso.exe.config` | `{app}\Administrador\` | ConnectionString "Local", ProvisioningApiKey, NombreEmpresa |

### 6.2 Connection string

```xml
<connectionStrings>
  <add name="Local"
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=DP_MiEmpresa;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

En producción esta sección está **cifrada con DPAPI** (no legible en texto plano).

### 6.3 App Settings

```xml
<appSettings>
  <add key="NombreEmpresa" value="Mi Empresa" />
  <add key="ProvisioningApiKey" value="..." />
</appSettings>
```

### 6.4 Cache de licencia

- `license.dat` — Ticket + firma, cifrado con DPAPI
- `clock.dat` — Último timestamp del servidor (anti-tampering)
- Ubicación: directorio del ejecutable

---

## 7. CONTROL DE VERSIONES

### 7.1 Repositorio

- **Plataforma:** GitHub (repositorio privado)
- **URL:** `https://github.com/GOAssad/DigitalPlusDesk`
- **Rama principal:** `master`

### 7.2 Estructura del repositorio

```
DigitalPlusDesk/
├── Common/                          ← Librerías compartidas (OLD Common)
│   ├── Acceso.Clases.Datos/         ← Clases de datos (RRHHLegajos, etc.)
│   ├── Global.Datos/                ← SQLServer.cs (helper SQL)
│   ├── Global.Funciones/            ← Utilidades
│   └── DigitalPlus.Licensing/       ← Librería de licencias (.NET 4.8)
├── Complementos/                    ← SDK DigitalPersona (excluido de git)
└── DigitalOnePlus/
    ├── Administrador/
    │   ├── Acceso/                  ← Proyecto principal del Administrador
    │   └── Instalador/              ← Instalador individual (legacy)
    ├── Fichador/
    │   ├── TEntradaSalida/          ← Proyecto principal del Fichador
    │   ├── Installer/               ← Instalador individual (legacy)
    │   └── documentacionClaude/     ← Documentación técnica
    ├── AzureProvisioning/
    │   ├── src/DigitalPlus.Provisioning/ ← Azure Functions (.NET 8)
    │   ├── sql/                     ← Scripts SQL (tablas, SPs, migración)
    │   └── tools/                   ← Scripts PowerShell de administración
    ├── InstaladorUnificado/
    │   └── setup.iss                ← Instalador Inno Setup v1.3
    ├── Common/                      ← Common duplicado (NO referenciado)
    ├── Datos/                       ← Scripts SQL de creación de BD
    └── tools/
        └── ConfigProtector/         ← Herramienta DPAPI
```

### 7.3 Flujo de trabajo

- Los cambios se commitean con mensajes descriptivos
- Se hace push a GitHub después de cada cambio importante
- No se usan ramas de feature (proyecto individual por ahora)
- Git es gestionado por Claude durante las sesiones de desarrollo

### 7.4 Nota importante sobre Common

**Ambos proyectos** (Fichador y Administrador) referencian el Common en:
`DigitalPlusDesk\Common\Acceso.Clases.Datos\`

**NO** usan el Common en `DigitalOnePlus\Common\` — ese directorio existe pero no está referenciado en los .csproj.

---

## 8. ESTRUCTURA DE CARPETAS DEL PROYECTO

```
C:\Apps\Claude\Huellas\
├── DigitalPlusDesk_Claude\          ← REPOSITORIO GIT
│   ├── .git\
│   ├── .gitignore
│   ├── Common\                      ← Librerías compartidas (ACTIVO)
│   ├── Complementos\                ← SDKs externos
│   └── DigitalOnePlus\
│       ├── Administrador\           ← App de administración
│       ├── Fichador\                ← App de fichado
│       ├── AzureProvisioning\       ← Cloud functions
│       ├── InstaladorUnificado\     ← Instalador
│       ├── Datos\                   ← Scripts SQL
│       └── tools\                   ← Herramientas
│
└── DigitalPlusWeb_Claude\           ← PROYECTO WEB (repo separado)
    ├── DigitalPlus\                 ← App Blazor Server
    └── Documentacion\               ← Docs SQL y técnica
```

---

## 9. AVANCES REALIZADOS

### Etapa 1 — Análisis de BD DigitalPlus
- Reporte completo del modelo de datos actual
- Identificación de todas las tablas, columnas y relaciones

### Etapa 2 — Auditoría de código del Administrador
- Identificación de todas las referencias a tablas DigitalOne
- Reporte de impacto de código

### Etapa 3 — Adaptación de Administrador y Fichador a DigitalPlus
- 41 archivos Designer.cs actualizados (script PowerShell)
- Ediciones manuales en 20+ archivos
- Clases Common adaptadas (RRHHLegajos, RRHHLegajosHuellas, GRALSucursalesGrupos, etc.)
- Tablas inexistentes en DigitalPlus stubbeadas (RRHHLegajosDomicilios, RRHHLegajosTurnos)

### Seguridad — DPAPI Hardening
- Credenciales hardcodeadas eliminadas de todo el código
- ConfigProtector.exe para cifrado DPAPI de connection strings
- ACL NTFS en archivos .config
- Templates de config limpios

### Instalador unificado
- v1.0: Instalador básico
- v1.1: DPAPI hardening + ACL
- v1.2: SQL Express incluido + Windows Auth + Local/Nube + BD sanitizada
- v1.3: Sistema de licencias integrado

### Azure Provisioning
- Function App desplegada en Azure (brazilsouth)
- Endpoints: provision, health, license/activate, license/heartbeat
- Tabla ActivationCodes + 3 SPs para códigos del instalador
- Script generate-code.ps1

### Sistema de licencias v1.3
- Tablas: Licencias, LicenciasLog, LicenseCodes en DigitalPlusAdmin
- SPs: License_Activate, License_Heartbeat, License_ValidateAndConsumeCode
- Librería cliente: DigitalPlus.Licensing (.NET 4.8, sin NuGet)
- Integrado en Fichador (startup + timer 4h) y Administrador (startup + ClickGuardar)
- Firma RSA de tickets + cache DPAPI + clock guard
- Scripts de administración: generate, list, modify, suspend
- 14 escenarios probados exitosamente

### Migración a DigitalPlusAdmin
- BD administrativa separada de la BD del cliente
- Tablas de licencias y activation codes migradas
- Azure Functions actualizadas para apuntar a DigitalPlusAdmin

### Control de versiones
- Git inicializado con commit v1.3
- Repositorio privado en GitHub (GOAssad/DigitalPlusDesk)

---

## 10. ESTADO ACTUAL DEL PROYECTO

### Terminado
- ✅ Fichador: funcional en producción
- ✅ Administrador: compilado y adaptado a DigitalPlus
- ✅ Web: en producción (Ferozo)
- ✅ Instalador v1.3 con SQL Express y licencias
- ✅ Azure Provisioning desplegado
- ✅ Sistema de licencias completo (4/4 pendientes resueltos)
- ✅ DPAPI hardening
- ✅ Git + GitHub configurado

### En desarrollo / Pendiente
- ⏳ Etapa 4: Ampliar proyecto web con reportes
- ⏳ Panel de admin web (actualmente son scripts PowerShell)
- ⏳ SPs faltantes en BD clientes: `RRHHFichadas_SP_LISTADO`, `RRHHFichadas_SP_MANUAL`, `RRHHFichadas_SP_MANUAL_SELECT_GRUPO`, `RRHHIncidenciasLegajos*`
- ⏳ Tabla `GRALPaises` no existe en DigitalPlus (soft-fail en Administrador)
- ⏳ MSComCtl2/AxMSComCtl2: COM reference en .csproj del Administrador (requiere `regsvr32` o eliminar)

---

## 11. RECOMENDACIONES TÉCNICAS

### Observaciones

1. **Common duplicado:** Existe un `DigitalOnePlus\Common\` que NO se usa. Podría eliminarse para evitar confusión.

2. **Dead code:** `RRHHFichadasDao.obtenerFichadas()` tiene SQL de DigitalOne. No se llama pero confunde.

3. **SQLServer.cs es estático:** Cada llamada a `Ejecutar()` llama a `ActualizarProp()` que parsea el connection string. Funciona pero es ineficiente. No es urgente cambiarlo.

### Posibles mejoras

1. **Panel de admin web:** Migrar los scripts PowerShell a una sección protegida en DigitalPlusWeb (ya tiene autenticación por roles).

2. **Monitoreo de heartbeats:** Alertas cuando un cliente deja de enviar heartbeats (posible desconexión o manipulación).

3. **Renovación de licencias:** Actualmente para ampliar legajos hay que usar `modify-license.ps1`. Podría automatizarse con un nuevo código de activación que el cliente ingrese.

### Puntos críticos

1. **Clave RSA:** La clave privada está en Azure App Settings. Si se pierde, no se pueden firmar nuevos tickets. Respaldar.

2. **Ferozo como único servidor:** Tanto la BD del cliente web como DigitalPlusAdmin están en Ferozo. Si Ferozo cae, los nuevos clientes no pueden activar y los heartbeats fallan (pero las apps siguen funcionando offline con el ticket cacheado, hasta 72 horas).

3. **Password de BD:** Todas las conexiones a Ferozo usan `sa/Soporte1`. Debería crearse un usuario con permisos limitados para DigitalPlusAdmin.

### Riesgos arquitectónicos

1. **Single point of failure:** Ferozo es el único servidor SQL para producción web + licencias.

2. **Sin CI/CD:** Los deploys de Azure Functions son manuales (`az functionapp deployment`). Un pipeline automatizado reduciría errores.

3. **Sin tests automatizados:** Las pruebas se hacen manualmente. Para un sistema de licencias, tests unitarios serían valiosos.
