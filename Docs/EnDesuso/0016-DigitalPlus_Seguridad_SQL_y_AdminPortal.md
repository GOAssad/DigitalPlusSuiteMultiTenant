# DIGITALPLUS — Seguridad SQL y Portal de Administracion

**Fecha:** 2026-03-05
**Generado por:** Claude (Opus 4.6)
**Documento origen:** DigitalPlus_Arquitectura_Actual_Rta_PL.md

---

## 1. ANALISIS DE IMPACTO: Eliminar uso de `sa`

### 1.1 Estado actual de credenciales en el proyecto

| Archivo | Ubicacion | Usa sa/Soporte1 | Severidad | Accion |
|---|---|---|---|---|
| `Common\Acceso.Clases.Datos\App.config` | Desktop | Si (2 connection strings legacy) | CRITICA | Limpiar — estas conexiones apuntan a 192.168.0.11/TocayAnda (BD antigua, no se usan) |
| `Common\Global.Datos\SQLServer.cs` | Desktop | No — lee de config en runtime | NINGUNA | Sin cambio |
| `Common\Global.Datos\SQLAccess.cs` | Desktop | No — lee de config en runtime | NINGUNA | Sin cambio |
| `InstaladorUnificado\setup.iss` | Instalador | No — usa Windows Auth | NINGUNA | Solo agregar creacion de usuario SQL |
| `fichador.app.config.template` | Instalador | No — usa placeholders | NINGUNA | Actualizar placeholder para nuevo usuario |
| `admin.app.config.template` | Instalador | No — usa placeholders | NINGUNA | Actualizar placeholder para nuevo usuario |
| `AzureProvisioning\local.settings.json` | Azure (dev) | Si — Ferozo sa/Soporte1 | ALTA | Reemplazar por usuario dedicado |
| `AzureProvisioning\docs\deployment-guide.md` | Documentacion | Si — ejemplos con sa | MEDIA | Actualizar ejemplos |
| `AzureProvisioning\tools\*.ps1` | Scripts admin | Si — todos usan sa/Soporte1 | ALTA | Parametrizar o usar usuario dedicado |
| `DigitalPlusWeb\appsettings.json` | Web (prod) | Si (2 de 4 strings) | CRITICA | Reemplazar por usuario dedicado |
| `DigitalPlusWeb\appsettings.Development.json` | Web (dev) | Si (4 strings) | MEDIA | Mover a User Secrets |

### 1.2 Que NO se rompe

- **SQLServer.cs y SQLAccess.cs**: Ya leen de `ConnectionStrings["Local"]` sin hardcodear. Si el config tiene otro usuario, funciona igual.
- **Instalador local**: Ya usa `Integrated Security=True` (Windows Auth). No pasa por `sa`.
- **Azure Functions codigo**: Lee de App Settings. Si cambiamos el App Setting, funciona.
- **Libreria de licencias**: Solo usa HTTP, no SQL.

### 1.3 Que SI hay que cambiar

| Componente | Cambio necesario |
|---|---|
| **Ferozo** | Crear usuario SQL dedicado para DigitalPlusAdmin y para DigitalPlus |
| **Azure App Settings** | Actualizar `ProvisioningDb`, `CloudSqlUser`, `CloudSqlPassword` |
| **Scripts PowerShell** | Cambiar credenciales por defecto o parametrizar |
| **Web appsettings** | Reemplazar sa por usuario dedicado |
| **Common App.config** | Limpiar connection strings legacy (TocayAnda) |
| **Instalador (modo local)** | Agregar paso de creacion de login/user/rol SQL |
| **deployment-guide.md** | Actualizar ejemplos |

---

## 2. NUEVO MODELO DE SEGURIDAD SQL

### 2.1 Usuarios propuestos

| Servidor | Usuario | BD | Proposito |
|---|---|---|---|
| Ferozo | `dp_admin_svc` | DigitalPlusAdmin | Azure Functions (licencias, provisioning) |
| Ferozo | `dp_web_svc` | DigitalPlus | Aplicacion web en produccion |
| Local (cada cliente) | `dp_<Empresa>_<GUID6>` | DP_NombreEmpresa | Fichador + Administrador |

### 2.2 Rol de aplicacion

```sql
-- Crear en cada BD cliente y en DigitalPlusAdmin
CREATE ROLE dp_role_app;

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO dp_role_app;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_app;

-- NO dar: db_owner, ALTER, CREATE TABLE, DROP
```

### 2.3 Script de creacion — Ferozo

```sql
-- Ejecutar con sa (unica vez)
USE [master];

-- Usuario para Azure Functions / Scripts admin
CREATE LOGIN [dp_admin_svc] WITH PASSWORD = '<password_seguro>';
GO

USE [DigitalPlusAdmin];
CREATE USER [dp_admin_svc] FOR LOGIN [dp_admin_svc];
CREATE ROLE dp_role_app;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO dp_role_app;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_app;
ALTER ROLE dp_role_app ADD MEMBER [dp_admin_svc];
GO

-- Usuario para la web
CREATE LOGIN [dp_web_svc] WITH PASSWORD = '<password_seguro>';
GO

USE [DigitalPlus];
CREATE USER [dp_web_svc] FOR LOGIN [dp_web_svc];
CREATE ROLE dp_role_app;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO dp_role_app;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_app;
ALTER ROLE dp_role_app ADD MEMBER [dp_web_svc];
GO
```

### 2.4 Script de creacion — Instalador local

El instalador debera agregar un paso que ejecute (con Windows Auth durante instalacion):

```sql
-- Variables: @LoginName, @Password, @DatabaseName

-- Crear login
CREATE LOGIN [@LoginName] WITH PASSWORD = '@Password',
    DEFAULT_DATABASE = [@DatabaseName],
    CHECK_POLICY = OFF;

-- Crear user en la BD
USE [@DatabaseName];
CREATE USER [@LoginName] FOR LOGIN [@LoginName];

-- Crear rol y asignar
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_app')
    CREATE ROLE dp_role_app;

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO dp_role_app;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_app;
ALTER ROLE dp_role_app ADD MEMBER [@LoginName];
```

El nombre del login sera: `dp_<EmpresaSanitizada>_<6 chars random>`
El password se genera aleatoriamente (16 chars, alfanumerico + especiales).
Ambos se guardan en el connection string del .exe.config y se cifran con DPAPI.

### 2.5 Modificaciones al instalador

Cambios en `setup.iss`:

1. **Nuevo paso**: Despues de crear la BD y ejecutar el script de estructura, crear el login/user/rol
2. **Generar password**: Funcion Pascal que genera password aleatorio
3. **Armar connection string**: Usar `User Id=dp_xxx;Password=xxx` en vez de `Integrated Security=True`
4. **Cifrar con DPAPI**: Ya existe este paso (ConfigProtector.exe)

**Importante**: Las credenciales bootstrap (Windows Auth) solo se usan durante la instalacion. El connection string final del .exe.config usa el usuario dedicado.

### 2.6 Compatibilidad con instalaciones existentes

- Instalaciones locales existentes usan `Integrated Security=True` → **siguen funcionando** sin cambio
- El nuevo modelo aplica solo a nuevas instalaciones
- Para migrar existentes: script de actualizacion que crea el usuario y cambia el config (futuro)

---

## 3. LISTA DE CAMBIOS NECESARIOS

### Fase 1 — Ferozo (sin tocar apps)

| # | Cambio | Impacto |
|---|---|---|
| 1.1 | Crear login `dp_admin_svc` en Ferozo | Ninguno |
| 1.2 | Crear user + rol en DigitalPlusAdmin | Ninguno |
| 1.3 | Crear login `dp_web_svc` en Ferozo | Ninguno |
| 1.4 | Crear user + rol en DigitalPlus | Ninguno |
| 1.5 | Probar conexion con nuevos usuarios | Ninguno |

### Fase 2 — Actualizar configs (cambio transparente)

| # | Cambio | Impacto |
|---|---|---|
| 2.1 | Azure App Setting `ProvisioningDb` → `dp_admin_svc` | Azure Functions usan nuevo user |
| 2.2 | Azure App Settings `CloudSqlUser`/`CloudSqlPassword` → `dp_admin_svc` | Provisioning usa nuevo user |
| 2.3 | Web `appsettings.json` → `dp_web_svc` | Web usa nuevo user |
| 2.4 | Scripts PowerShell → parametro `-User dp_admin_svc` | Scripts usan nuevo user |
| 2.5 | `deployment-guide.md` → actualizar ejemplos | Solo documentacion |
| 2.6 | `Common\Acceso.Clases.Datos\App.config` → limpiar strings legacy | Elimina referencia a TocayAnda |

### Fase 3 — Actualizar instalador

| # | Cambio | Impacto |
|---|---|---|
| 3.1 | Agregar generacion de password en setup.iss | Nuevas instalaciones |
| 3.2 | Agregar creacion de login/user/rol SQL | Nuevas instalaciones |
| 3.3 | Connection string con usuario dedicado en vez de Windows Auth | Nuevas instalaciones |
| 3.4 | Bumpar version a 1.4 | Solo version |

### Fase 4 — Portal Admin (proyecto nuevo)

| # | Cambio | Impacto |
|---|---|---|
| 4.1 | Crear proyecto DigitalPlusAdminWeb (Blazor Server) | Nuevo |
| 4.2 | Implementar autenticacion (Identity) | Nuevo |
| 4.3 | CRUD empresas + licencias | Nuevo |
| 4.4 | Vista de activaciones y heartbeats | Nuevo |
| 4.5 | Emision/revocacion de codigos | Nuevo |
| 4.6 | Log de auditoria | Nuevo |
| 4.7 | Deploy en Ferozo o Azure | Nuevo |

---

## 4. PROPUESTA DE ESTRUCTURA DEL PORTAL ADMIN

### 4.1 Proyecto

```
DigitalPlusAdminWeb/
├── Program.cs
├── appsettings.json                 ← Connection string a DigitalPlusAdmin
├── Data/
│   ├── AdminDbContext.cs            ← DbContext con tablas de licencias
│   └── ApplicationUser.cs           ← Usuario Identity
├── Models/
│   ├── Licencia.cs
│   ├── LicenciaLog.cs
│   ├── LicenseCode.cs
│   └── ActivationCode.cs
├── Services/
│   ├── LicenciaService.cs          ← CRUD licencias
│   ├── LicenseCodeService.cs       ← Generar/listar codigos
│   └── AuditService.cs             ← Log de auditoria
├── Pages/
│   ├── Index.razor                  ← Dashboard (total licencias, activas, trial, vencidas)
│   ├── Licencias/
│   │   ├── Index.razor              ← Lista de licencias con filtros
│   │   ├── Detail.razor             ← Detalle de licencia + historial
│   │   └── Edit.razor               ← Modificar plan/legajos/expiracion
│   ├── Codigos/
│   │   ├── Index.razor              ← Lista de codigos (usados/disponibles)
│   │   └── Generate.razor           ← Generar nuevo codigo
│   ├── Empresas/
│   │   ├── Index.razor              ← Lista de empresas (agrupadas de Licencias.CompanyId)
│   │   └── Detail.razor             ← Detalle empresa + sus licencias
│   ├── Auditoria/
│   │   └── Index.razor              ← Log de eventos con filtros
│   └── Account/
│       └── Login.razor
└── Shared/
    ├── MainLayout.razor
    └── NavMenu.razor
```

### 4.2 Tecnologia

- **Blazor Server** (.NET 8)
- **EF Core** para acceso a DigitalPlusAdmin
- **ASP.NET Identity** para autenticacion (usuario admin unico por ahora)
- **Bootstrap 5** para UI (consistente con DigitalPlusWeb)

### 4.3 Pantallas principales

#### Dashboard
- Licencias activas / trial / suspendidas / vencidas (contadores)
- Ultimos heartbeats (tabla con actividad reciente)
- Alertas: licencias por vencer en 30 dias, trials por expirar

#### Licencias
- Tabla con filtros: empresa, tipo, plan, estado
- Acciones: ver detalle, modificar, suspender, reactivar
- Detalle: historial de eventos (LicenciasLog)

#### Codigos de licencia
- Tabla: disponibles, usados, expirados
- Generar nuevo: formulario con plan, maxLegajos, duracion
- Al generar, muestra codigo en pantalla (unica vez)

#### Auditoria
- Tabla LicenciasLog con filtros por fecha, accion, empresa

### 4.4 Base de datos

El portal usa **la misma BD DigitalPlusAdmin** que ya existe. No requiere tablas nuevas excepto las de Identity:

```
DigitalPlusAdmin (existente)
├── Licencias              ← ya existe
├── LicenciasLog           ← ya existe
├── LicenseCodes           ← ya existe
├── ActivationCodes        ← ya existe
└── AspNet* (nuevas)       ← Identity tables (Users, Roles, etc.)
```

### 4.5 Hosting

**Opcion recomendada**: Deploy en Ferozo como sitio web separado (si el hosting lo permite) o en Azure App Service (free tier).

---

## 5. CRONOGRAMA SUGERIDO

| Fase | Descripcion | Estimacion | Dependencia |
|---|---|---|---|
| **1** | Crear usuarios SQL en Ferozo | Rapido | Ninguna |
| **2** | Actualizar configs y scripts | Rapido | Fase 1 |
| **3** | Actualizar instalador (v1.4) | Medio | Fase 2 |
| **4** | Portal Admin (DigitalPlusAdminWeb) | Grande | Fase 1 |

Las fases 1-3 (seguridad SQL) se pueden hacer primero.
La fase 4 (portal) se puede hacer en paralelo o despues.

---

## 6. RIESGOS Y MITIGACIONES

| Riesgo | Mitigacion |
|---|---|
| Nuevo usuario SQL sin permisos suficientes | Probar TODAS las funciones antes de quitar sa |
| Ferozo no permite crear logins SQL | Verificar antes de empezar (panel de Ferozo) |
| Romper instalaciones existentes | El cambio solo aplica a nuevas instalaciones |
| Portal admin expuesto publicamente | Proteger con Identity + IP whitelist si es posible |
| Password del usuario SQL se pierde | Queda en Azure App Settings y en DPAPI del config |

---

## 7. ORDEN DE IMPLEMENTACION RECOMENDADO

1. **Verificar** que Ferozo permite crear logins SQL (restriccion del hosting)
2. **Crear** usuarios en Ferozo (Fase 1)
3. **Probar** conexion con los nuevos usuarios
4. **Actualizar** Azure App Settings y web config (Fase 2)
5. **Verificar** que todo funciona con los nuevos usuarios
6. **Recien entonces** eliminar acceso de sa a las BDs (o cambiar password de sa)
7. **Actualizar** instalador (Fase 3)
8. **Crear** portal admin (Fase 4)
