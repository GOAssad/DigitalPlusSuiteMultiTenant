# DIGITALPLUS - Reporte de Arquitectura para Project Leader

**Version:** 3.0
**Fecha:** 2026-03-07
**Generado por:** Claude Opus 4.6

---

## 1. VISION GENERAL

### Que es DigitalPlus

DigitalPlus es un **sistema de control de accesos y gestion de personal** basado en reconocimiento biometrico (huella digital), fichada por PIN y modo demo. Permite registrar ingresos y egresos del personal, administrar legajos, gestionar horarios, y visualizar informacion de asistencia a traves de multiples plataformas.

### Que problema resuelve

- Control de asistencia del personal mediante huella dactilar, PIN o modo demostracion
- Registro automatico de fichadas (entrada/salida)
- Gestion centralizada de legajos, sucursales, categorias y horarios
- Reportes de horas trabajadas, llegadas tarde, horas extras
- Administracion remota via web
- Licenciamiento y distribucion comercial del software

### Empresa desarrolladora

**Integra IA** - La solucion se comercializa bajo el nombre DigitalPlus / DigitalOnePlus.

---

## 2. ARQUITECTURA GENERAL

### Diagrama de componentes

```
APLICACIONES DE ESCRITORIO (Windows)
+---------------------+    +---------------------+
|     FICHADOR        |    |   ADMINISTRADOR     |
|  (TEntradaSalida)   |    |     (Acceso)        |
|  WinForms .NET 4.8  |    |  WinForms .NET 4.8  |
|                     |    |                     |
| - Fichada x huella  |    | - ABM Legajos       |
| - Fichada x PIN     |    | - Enrolamiento      |
| - Modo Demo         |    | - Config. sistema   |
| - Semaforo visual   |    | - Reportes          |
+----------+----------+    +----------+----------+
           |                          |
           +------------+-------------+
                        |
              +---------v---------+
              |   BASE DE DATOS   |
              |   SQL Server      |
              | (Local o Ferozo)  |
              +-------------------+

APLICACION WEB
+---------------------+
|  DIGITALPLUSWEB     |
|  Blazor Server      |
|  .NET 7             |
|  (Ferozo hosting)   |
+----------+----------+
           |
           v
  BD DigitalPlus (Ferozo)


PORTAL DE LICENCIAS (Backoffice Integra IA)
+---------------------+
| PORTAL LICENCIAS    |
| Blazor Server       |
| .NET 10             |
+----------+----------+
           |
           v
  BD DigitalPlusAdmin (Ferozo)


INFRAESTRUCTURA CLOUD
+---------------------+
| AZURE FUNCTIONS     |
| .NET 8 Isolated     |
| - License/activate  |
| - License/heartbeat |
+----------+----------+
           |
           v
  BD DigitalPlusAdmin (Ferozo)
```

---

## 3. COMPONENTES DEL SISTEMA

### 3.1 Fichador (TEntradaSalida)

| Atributo | Detalle |
|---|---|
| **Tipo** | Aplicacion de escritorio Windows |
| **Stack** | C# / .NET Framework 4.8 / WinForms |
| **Solucion** | `TEntradaSalida.sln` |
| **Ejecutable** | `TEntradaSalida.exe` |
| **Proposito** | Terminal de fichaje biometrico en cada sucursal |

**Funcionalidades:**
- Captura de huella digital mediante lector USB DigitalPersona uAreU 4500
- Comparacion 1:N contra todas las huellas registradas en la BD
- Fichada por PIN (4-6 digitos) como alternativa a la huella
- Modo Demo para demostraciones comerciales sin hardware
- Deteccion automatica de lector con fallback a PIN o Demo
- Semaforo visual (verde/amarillo/rojo) como feedback
- Registro automatico de entrada o salida
- Identificacion de terminal por nombre de maquina
- Sistema de licencias integrado (trial 14 dias, activacion por codigo)

**Modos de fichada:**
1. **Huella:** Requiere lector DigitalPersona conectado
2. **PIN:** Empleado ingresa su numero de legajo + PIN de 4-6 digitos
3. **Demo:** Seleccion visual del empleado, sin hardware requerido

### 3.2 Administrador (Acceso)

| Atributo | Detalle |
|---|---|
| **Tipo** | Aplicacion de escritorio Windows |
| **Stack** | C# / .NET Framework 4.8 / WinForms |
| **Solucion** | `DigitalOneAdministrador.sln` |
| **Ejecutable** | `Acceso.exe` |
| **Proposito** | Gestion completa del sistema |

**Funcionalidades:**
- ABM de Legajos (empleados) con foto por camara web
- Enrolamiento de huellas digitales
- Gestion de PIN por empleado (asignar, resetear, forzar cambio)
- Gestion de Sucursales, Categorias, Horarios, Sectores
- Gestion de Incidencias (permisos, ausencias, vacaciones)
- Configuracion del sistema (modo PIN, modo Demo, expiracion de PIN)
- PINs vencidos: listado y forzar cambio masivo
- Reportes con Microsoft ReportViewer
- Exportacion a Excel (SpreadsheetLight / DocumentFormat.OpenXml)
- Generacion de PDF (iText 7)
- Sistema de licencias integrado
- Informacion de licencia en barra de estado y menu

### 3.3 DigitalPlusWeb

| Atributo | Detalle |
|---|---|
| **Tipo** | Aplicacion web |
| **Stack** | Blazor Server / .NET 7 / EF Core + Dapper |
| **Solucion** | `DigitalPlus.sln` |
| **Hosting** | Ferozo (produccion) |
| **Proposito** | Gestion administrativa via web |

**Funcionalidades:**
- Gestion de Legajos, Fichadas, Horarios, Categorias
- Gestion de Sectores, Sucursales, Terminales
- Feriados, Incidencias, Vacaciones
- Noticias y Variables del sistema
- Gestion de Usuarios (Identity)
- Publicacion automatizada via GitHub Actions

### 3.4 Portal de Licencias (DigitalPlus.Licencias)

| Atributo | Detalle |
|---|---|
| **Tipo** | Aplicacion web (backoffice) |
| **Stack** | Blazor Server / .NET 10 / EF Core |
| **Solucion** | `DigitalPlus.Licencias.sln` |
| **Repositorio** | Independiente (`C:\apps\claude\huellas\DigitalPlusLicencias\`) |
| **Proposito** | Administracion de empresas, licencias y provisionamiento |

**Funcionalidades:**
- Dashboard con estadisticas
- ABM de Empresas (con wizard de alta en 5 pasos)
- Gestion de Licencias y codigos de activacion
- Log de auditoria
- Atributos del sistema (Paises, Tipos de identificacion fiscal)
- API REST `/api/activar` para el instalador liviano
- Provisionamiento de bases de datos en Ferozo

### 3.5 Azure Functions (Provisioning)

| Atributo | Detalle |
|---|---|
| **Tipo** | Serverless Functions |
| **Stack** | .NET 8 Isolated Worker |
| **Hosting** | Azure (brazilsouth) |
| **Proposito** | Licenciamiento y provisioning |

**Endpoints:**
- `POST /api/license/activate` - Crear trial o activar con codigo, retorna ticket firmado RSA
- `POST /api/license/heartbeat` - Actualizar heartbeat, retornar ticket actualizado
- `POST /api/provision` - Provisioning de instalaciones (modo nube del instalador unificado)

### 3.6 Instaladores

#### Instalador Unificado (v1.3)
| Atributo | Detalle |
|---|---|
| **Herramienta** | Inno Setup 6.x |
| **Script** | `InstaladorUnificado\setup.iss` |
| **Salida** | `~180 MB` (incluye SQL Express) |
| **Proposito** | Instalacion completa con 2 modos: LOCAL y NUBE |

- **Modo LOCAL:** Instala SQL Express 2019 si no existe, crea BD automaticamente, Windows Auth
- **Modo NUBE:** Pide codigo de activacion, llama a Azure Functions para provisioning
- Instala: Fichador + Administrador + Driver DigitalPersona RTE
- Cifra configuracion con DPAPI via ConfigProtector
- Aplica seguridad ACL sobre la carpeta de instalacion

#### Instalador Liviano (v1.0)
| Atributo | Detalle |
|---|---|
| **Herramienta** | Inno Setup 6.x |
| **Script** | `InstaladorLiviano\setup-liviano.iss` |
| **Salida** | `~25 MB` (sin SQL Express) |
| **Proposito** | Instalacion cloud-only para clientes con BD en Ferozo |

- NO instala SQL Server Express
- Pide codigo de activacion (generado desde Portal de Licencias)
- Llama a API del portal (`/api/activar`) para obtener connection string
- Instala: Fichador + Administrador + Driver DigitalPersona RTE
- Cifra configuracion con DPAPI

---

## 4. LIBRERIAS COMPARTIDAS (Common)

Todas las aplicaciones de escritorio comparten estas librerias:

| Libreria | Responsabilidad |
|---|---|
| **Acceso.Clases.Datos** | Entidades de dominio RRHH, acceso a datos especifico del negocio, clases PIN |
| **Global.Datos** | Capa de infraestructura: clase estatica SQLServer con metodos ADO.NET |
| **Global.Funciones** | Utilidades transversales: configuracion, cadenas, formatos, mail |
| **Global.Controles** | Controles WinForms personalizados (botones, grillas, etiquetas) |
| **DigitalPlus.Licensing** | Sistema de licencias: LicenseManager, LicenseValidator, LicenseClient, LicenseCache, ClockGuard, MachineIdProvider, FrmLicenseBlocked |
| **Global.DigitalPersona** | Interfaz con SDK DigitalPersona para lectores de huella |

---

## 5. BASE DE DATOS

### Estructura de bases de datos

| Base de datos | Ubicacion | Proposito |
|---|---|---|
| `DP_{companyId}` | Ferozo / Local | BD operativa de cada empresa (ej: `DP_integra_ia_srl`) |
| `DigitalPlus` | Ferozo | BD de DigitalPlusWeb (produccion, 758 legajos) |
| `DigitalPlusAdmin` | Ferozo | BD administrativa: licencias, empresas, codigos, auditoria |

### Tablas principales (BD empresa)

El esquema incluye **29 tablas** creadas automaticamente por `SchemaScript.sql` v3.0:

**RRHH:**
- `RRHHLegajos` - Empleados (con PIN, PinExpiraEn, PinRequiereCambio)
- `RRHHLegajosHuellas` - Huellas digitales binarias
- `RRHHFichadas` - Registro de fichadas (entrada/salida)
- `RRHHHorarios`, `RRHHCategorias`, `RRHHIncidencias`
- `RRHHUbicaciones` (Sectores), `RRHHIncidenciasLegajos`

**Generales:**
- `GRALSucursales`, `GRALTerminales`, `GRALVariablesGlobales`
- `GRALFeriados`, `GRALNoticias`

**Variables globales relevantes:**
- `FichadaModoPIN` - Habilita fichada por PIN
- `PinExpiraDias` - Dias de expiracion del PIN (0=no expira)
- `FichadaModoDemo` - Habilita modo demostracion

### Tablas administrativas (DigitalPlusAdmin)

- `Empresas` - Registro de empresas clientes (con CodigoActivacion)
- `Licencias` - Estado de licencias por empresa/maquina
- `LicenciasLog` - Auditoria de operaciones de licencia
- `LicenseCodes` - Codigos de activacion de uso unico
- `ActivationCodes` - Codigos de activacion del instalador
- `Paises` - 10 paises LATAM
- `TiposIdentificacionFiscal` - 11 tipos (CUIT, CUIL, RUC, RFC, NIT, RUT, CNPJ)

### Acceso a datos

| Componente | Patron |
|---|---|
| Fichador/Administrador | ADO.NET puro + Stored Procedures via `Global.Datos.SQLServer` |
| DigitalPlusWeb | EF Core + Dapper |
| Portal Licencias | EF Core |
| Azure Functions | ADO.NET + Stored Procedures |

---

## 6. SISTEMA DE LICENCIAS

### Constantes

| Parametro | Valor |
|---|---|
| Duracion trial | 14 dias |
| Max legajos en trial | 5 |
| Grace period offline | 72 horas |
| Grace period suspension | 7 dias |
| Clock Guard tolerancia | 1 hora |
| Check periodico | 4 horas |

### Flujo de activacion

```
App inicia
    |
    v
ValidateAtStartup(legajos)
    |
    v
Sin cache? --> POST /api/license/activate --> TRIAL (14 dias, 5 legajos)
    |
    v
Trial expira o legajos > 5? --> FrmLicenseBlocked
    |
    v
Usuario ingresa codigo --> ActivateWithCode()
    |
    v
POST /api/license/activate (con codigo) --> Azure valida y consume
    |
    v
Retorna ticket firmado RSA + signature --> Cache local (DPAPI)
```

### Seguridad del licenciamiento

- Firma RSA-SHA256 (clave privada en Azure, publica embebida en cliente)
- DPAPI Machine + entropy para cache local (`license.dat`)
- ClockGuard detecta manipulacion de reloj del sistema
- MachineId = SHA256(MachineGuid + BiosSerial + ProcessorId)
- Codigos de uso unico con flag `UsedAt` en BD
- UPDLOCK + HOLDLOCK en stored procedures para evitar race conditions

### Reglas de bloqueo

| Razon | Causa | Puede activar? |
|---|---|---|
| TrialExpired | Trial vencido | Si, con codigo |
| TrialLegajosExceeded | Legajos > MaxLegajos | Si, con codigo |
| Expired | Licencia vencida | No |
| SuspendedBlocked | Suspension + grace vencido | No |
| OfflineBlocked | Sin heartbeat > 72h | Reconectar |
| InvalidSignature | Ticket modificado | No |
| ClockTampered | Reloj retrocede > 1h | No |

---

## 7. SEGURIDAD SQL

### Estado actual

Se implemento un modelo de seguridad con usuarios SQL dedicados:

| Usuario | Rol | Usado por |
|---|---|---|
| `dp_admin_svc` | Administracion | Azure Functions, Portal Licencias |
| `dp_web_svc` | Web | DigitalPlusWeb (deploy pausado) |
| `sa` | Superadmin | Aun en uso (pendiente deshabilitar) |

### Fases del plan de migracion

| Fase | Estado |
|---|---|
| 1. Scripts SQL en Ferozo | COMPLETADA |
| 2. Actualizar configuraciones | EN PROGRESO (web pausado) |
| 3. Verificacion funcional | PENDIENTE |
| 4. Monitoreo conexiones | PENDIENTE |
| 5. Deshabilitar sa | PENDIENTE |

### Pendiente critico

`BuildClientConnectionString` en el portal de licencias usa `sa` para armar el connection string de las empresas. **Debe usar el usuario SQL dedicado de la empresa.**

---

## 8. ESTRUCTURA DE REPOSITORIOS

### Repositorio principal: DigitalPlusDesk_Claude (Git)

```
C:\Apps\Claude\Huellas\DigitalPlusDesk_Claude\
|
+-- Common\                          Librerias compartidas (nivel raiz)
|   +-- Acceso.Clases.Datos\         Capa de datos RRHH
|   +-- DigitalPlus.Licensing\       Sistema de licencias
|   +-- Global.Calendario\           Control calendario
|   +-- Global.Controles\            Controles WinForms custom
|   +-- Global.Datos\                Infraestructura ADO.NET
|   +-- Global.DigitalPersona\       SDK huella digital
|   +-- Global.Funciones\            Utilidades
|   +-- Imagenes\                    Recursos graficos
|
+-- Complementos\                    SDKs externos
|   +-- Digital-Persona-SDK-master\  SDK DigitalPersona
|   +-- Excel\                       Complementos Excel
|
+-- DigitalOnePlus\                  Aplicaciones
    +-- Administrador\               App Administrador
    |   +-- Acceso\                  Proyecto principal (.csproj)
    |   |   +-- Generales\           Forms generales (MainMenu, Config)
    |   |   +-- RRHH\                Forms RRHH (Legajos, Fichadas)
    |   |   +-- Reportes\            Reportes RDLC
    |   |   +-- SQL\                 Scripts SQL
    |   +-- Instalador\              Instalador legacy (individual)
    |   +-- Documentacion\           Docs del Administrador
    |
    +-- Fichador\                    App Fichador
    |   +-- TEntradaSalida\          Proyecto principal (.csproj)
    |   |   +-- uAreu\               Form de fichaje (FrmFichar)
    |   +-- Installer\               Instalador legacy (individual)
    |   +-- DocumentacionClaude\     Documentacion del proyecto
    |
    +-- InstaladorUnificado\         Instalador completo (Local + Nube)
    |   +-- setup.iss                Script InnoSetup
    |   +-- Prerequisites\           SQL Express 2019 EXE
    |   +-- Output\                  EXE generado
    |
    +-- InstaladorLiviano\           Instalador cloud-only
    |   +-- setup-liviano.iss        Script InnoSetup
    |   +-- Output\                  EXE generado (~25MB)
    |
    +-- AzureProvisioning\           Azure Functions
    |   +-- src\DigitalPlus.Provisioning\  Codigo fuente
    |   +-- sql\                     Scripts SQL (011-020)
    |   +-- tools\                   Scripts PowerShell de gestion
    |   +-- docs\                    Documentacion de provisioning
    |
    +-- Common\                      Copia de librerias (nivel DigitalOnePlus)
    +-- Datos\                       Scripts SQL, queries
    +-- tools\                       Herramientas (ConfigProtector)
```

### Repositorio DigitalPlusWeb_Claude (Git separado)

```
C:\Apps\Claude\Huellas\DigitalPlusWeb_Claude\
+-- DigitalPlus\                     Proyecto Blazor Server .NET 7
    +-- Pages\                       Paginas Razor (Legajos, Fichadas, etc.)
    +-- Entidades\                   Modelos de dominio
    +-- Repositorios\                Acceso a datos
    +-- Servicios\                   Logica de negocio
    +-- Data\                        DbContext EF Core
    +-- Areas\Identity\              Autenticacion
```

### Repositorio DigitalPlusLicencias (Git separado)

```
C:\apps\claude\huellas\DigitalPlusLicencias\
+-- DigitalPlus.Licencias\          Proyecto Blazor Server .NET 10
    +-- Components\Pages\            Paginas del portal
    +-- Models\                      Modelos de datos
    +-- Services\                    Servicios (DB provisioning, etc.)
    +-- Data\                        DbContext EF Core
```

---

## 9. HERRAMIENTAS NECESARIAS

### Para desarrollo

| Herramienta | Version | Proposito |
|---|---|---|
| Visual Studio | 2019 o superior | IDE principal para Fichador/Administrador |
| .NET Framework 4.8 SDK | - | Compilacion apps desktop |
| .NET 7 SDK | - | DigitalPlusWeb |
| .NET 10 SDK | - | Portal Licencias |
| .NET 8 SDK | - | Azure Functions |
| SQL Server Management Studio | 18+ | Gestion de BD |
| Inno Setup 6.x | 6.7.1 | Compilar instaladores |
| DigitalPersona One Touch SDK | 1.6.1 | Lectura de huellas digitales |
| Git | - | Control de versiones |
| Azure Functions Core Tools | v4 | Desarrollo local de Azure Functions |
| Azure CLI | - | Deploy a Azure |

### Para compilar instaladores

| Recurso | Ruta |
|---|---|
| InnoSetup ISCC.exe | `C:\Users\Gustavo\AppData\Local\Programs\Inno Setup 6\ISCC.exe` |
| SQL Express 2019 (unificado) | `InstaladorUnificado\Prerequisites\SQLEXPR_x64_ENU.exe` |
| Binarios Release Fichador | `Fichador\TEntradaSalida\bin\Release\` |
| Binarios Release Administrador | `Administrador\Acceso\bin\Release\` |
| SDK DigitalPersona RTE | `Digital-Persona-SDK-master\RTE\Install\` |

### Servicios externos

| Servicio | Cuenta | Proposito |
|---|---|---|
| Azure | gassad@live.com.ar | Azure Functions (licencias) |
| Ferozo | - | Hosting SQL Server y DigitalPlusWeb |
| GitHub | - | Repositorios y CI/CD |

---

## 10. ESTADO ACTUAL DEL PROYECTO (Marzo 2026)

### Completado

- Fichador con 3 modos de fichada (Huella, PIN, Demo)
- Administrador con gestion de PIN y configuracion
- Sistema de licencias end-to-end (trial -> bloqueo -> activacion)
- Azure Functions para licenciamiento desplegadas
- Portal de Licencias funcional (local)
- Instalador Unificado v1.3 (Local + Nube)
- Instalador Liviano v1.0 compilado OK (25MB)
- Seguridad SQL: fase 1 completada (scripts ejecutados)
- API `/api/activar` en portal para instalador liviano
- Alta de empresa desde portal (crea BD + esquema 29 tablas)

### En progreso

- Migracion de seguridad SQL (fases 2-5)
- Portal Licencias: pendiente deploy a produccion

### Pendiente

- **CRITICO:** `BuildClientConnectionString` usa `sa` (debe usar usuario dedicado)
- Probar flujo completo: crear empresa -> instalar liviano -> apps conectan
- Deploy portal licencias a `licencias.digitaloneplus.com`
- Deploy `dp_web_svc` en DigitalPlusWeb (pausado)
- Deshabilitar usuario `sa`

### Prioridad baja

- Auto-fetch datos fiscales (AFIP, etc.)
- DigitalPlusWeb multi-tenant
- WhatsApp integration
- Reportes ampliados en web
- Limpieza dead code Administrador

---

## 11. OBSERVACIONES ARQUITECTONICAS

### Fortalezas

1. **Arquitectura instalable** con licenciamiento centralizado - ventaja competitiva real
2. **Dos modos de despliegue** (local con SQL Express, nube con Ferozo) cubren distintos segmentos
3. **Licenciamiento robusto** con firma RSA, anti-tampering, y gestion centralizada
4. **Portal de administracion** permite operar el negocio de licencias de forma profesional
5. **Separation of concerns** entre apps desktop, web, y Azure Functions

### Puntos de atencion

1. Las apps desktop usan .NET Framework 4.8 (legacy, sin soporte LTS). Evaluar migracion a .NET 8+ en el futuro.
2. No hay tests automatizados en ningun componente
3. Sin sistema de logging estructurado en las apps desktop
4. El acceso a datos en desktop es via clase estatica `SQLServer` (acoplamiento fuerte)
5. Tres repositorios Git separados dificultan la gestion unificada de versiones

### Riesgos

1. **Dependencia de Ferozo:** Si Ferozo cae, todas las instalaciones nube quedan sin servicio
2. **sa aun activo:** Riesgo de seguridad hasta completar la migracion
3. **Sin backup automatizado** documentado para las BD de empresas en Ferozo
4. **SDK DigitalPersona** es un componente legacy sin actualizaciones recientes

---

*Fin del Reporte de Arquitectura para Project Leader*
