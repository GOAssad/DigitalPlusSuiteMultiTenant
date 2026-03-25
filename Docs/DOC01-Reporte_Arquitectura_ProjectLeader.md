# DIGITALPLUS - Reporte de Arquitectura para Project Leader

**Version:** 17.0
**Fecha:** 2026-03-24
**Generado por:** Claude Opus 4.6

---

## 1. VISION GENERAL

### Que es DigitalPlus

DigitalPlus es un **sistema de control de accesos y gestion de personal** basado en reconocimiento biometrico (huella digital), fichada por PIN y modo demo. Permite registrar ingresos y egresos del personal, administrar legajos, gestionar horarios, y visualizar informacion de asistencia a traves de multiples plataformas.

### Que problema resuelve

- Control de asistencia del personal mediante huella dactilar, PIN, dispositivo movil o modo demostracion
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
| - Fichada x huella  |    | - Huellas + Foto    |
| - Fichada x PIN     |    | - Datos read-only   |
| - Modo Demo         |    | - Tab Movil         |
| - Semaforo visual   |    |                     |
+----------+----------+    +----------+----------+
           |                          |
           +------------+-------------+
                        |
              +---------v---------+
              |   BASE DE DATOS   |
              |   SQL Server      |
              | (Local o Ferozo)  |
              +-------------------+

PORTAL MULTI-TENANT (Web)
+---------------------+
| PORTAL MULTI-TENANT |
| Blazor Server       |
| .NET 10 / EF Core   |
| (Ferozo hosting)    |
+----------+----------+
           |
           v
  BD DigitalPlusMultiTenant (Ferozo / Local)

DIGITALPLUSWEB - LEGACY (obsoleto)
+---------------------+
|  DIGITALPLUSWEB     |
|  Blazor Server      |
|  .NET 7             |
|  (Ferozo hosting)   |
+----------+----------+
           |
           v
  BD DigitalPlus (Ferozo)


TERMINAL MOVIL (PWA)
+-------------------------------+
|  Digital One Mobile (PWA)     |
|  HTML + CSS + JS estatico     |
|  Servida desde wwwroot/mobile |
|                               |
|  - Fichada x GPS              |
|  - JWT auth                   |
|  - Instalable como PWA        |
+---------------+---------------+
                |
                | HTTPS (POST /api/mobile/*)
                v
  Portal Multi-Tenant (MobileController)
                |
                v
  BD DigitalPlusMultiTenant
  (TerminalesMoviles, SucursalGeoconfigs, CodigosActivacionMovil)


PORTAL DE LICENCIAS (Backoffice Integra IA)
+---------------------+
| PORTAL LICENCIAS    |
| Blazor Server       |
| .NET 10             |
| Azure App Service   |
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
- **Auto-registro de terminal:** Al iniciar por primera vez, si la maquina no esta registrada como Terminal en la BD, se registra automaticamente asociandola a la sucursal por defecto de la empresa
- Cambio voluntario de PIN (link "Cambiar mi PIN" en pantalla de fichada, formulario FrmCambiarPinVoluntario)
- Si el admin fuerza cambio de PIN (PinMustChange), al ingresar legajo muestra mensaje obligatorio (solo boton OK) y va directo a pedir nuevo PIN sin requerir el actual
- Si el admin resetea el PIN (lo elimina), al ingresar legajo ofrece crear uno nuevo (dialogo Si/No)
- Verificacion de estado de empresa al iniciar: si la empresa esta suspendida en DigitalPlusAdmin, muestra mensaje y cierra la app
- Sistema de licencias integrado (trial 14 dias, activacion por codigo). En modo multi-tenant la validacion de licencia esta DESHABILITADA (la activacion se realiza desde el instalador)

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
| **Proposito** | Enrolamiento de huellas digitales y captura de foto. Datos de legajo en modo solo lectura. |

**Funcionalidades (simplificado desde v12.0):**
- **Solo 2 pestanas:** "Legajo" (huellas + foto) y "Movil" (gestion de terminal movil)
- Enrolamiento de huellas digitales (requiere lector DigitalPersona conectado)
- Captura de foto del empleado via camara web (se persiste en BD como VARBINARY)
- **Datos del legajo en modo solo lectura:** nombre, apellido, sector, categoria, sucursal, horario, estado se muestran pero no se pueden editar
- Boton Guardar solo persiste huellas y foto (no datos del legajo)
- Boton Eliminar oculto: los legajos no se pueden eliminar desde la app desktop
- **Alta y edicion de legajos se realiza exclusivamente desde el Portal MT web** (incluyendo domicilio, sucursales, PIN, etc.)
- Pestanas removidas: Reportes, Domicilios, Turnos
- Tab "Movil": generar codigo de activacion, desactivar dispositivo movil
- Verificacion de estado de empresa al iniciar: si la empresa esta suspendida en DigitalPlusAdmin, muestra mensaje y cierra la app
- Sistema de licencias integrado. En modo multi-tenant la validacion de licencia esta DESHABILITADA (la activacion se realiza desde el instalador)
- Informacion de licencia en barra de estado y menu
- Logo de empresa cliente y logo IntegraIA en la interfaz (desde DigitalPlusAdmin via EmpresaInfoService)
- Menu lateral dinamico con links a redes sociales de la empresa (web, Facebook, Instagram, LinkedIn, X/Twitter, YouTube, TikTok) cargados desde DigitalPlusAdmin

### 3.3 Portal Multi-Tenant (PortalMultiTenant)

| Atributo | Detalle |
|---|---|
| **Tipo** | Aplicacion web multi-tenant |
| **Stack** | Blazor Server / .NET 10 / EF Core |
| **Proyecto** | `PortalMultiTenant\` |
| **BD** | `DigitalPlusMultiTenant` (localhost / Ferozo) |
| **Proposito** | Gestion administrativa multi-tenant via web |

**Funcionalidades:**
- Arquitectura multi-tenant con filtro por EmpresaId en todas las consultas, protegida por FK compuesto en tabla Fichada
- Dashboard con estadisticas, noticias y logo de empresa
- **CRUD completo de Legajos** (alta, edicion, domicilio, sucursales, PIN). Es el punto unico de gestion de datos de legajo (el Administrador desktop es solo lectura)
- Foto del legajo: muestra la foto capturada desde Administrador desktop (180x180 redondeada en formulario, 28px avatar en lista)
- Domicilio del legajo: 7 campos (Calle, Altura, Piso, Barrio, Localidad, Provincia, CodigoPostal) en tab Datos
- Sucursal obligatoria: al crear un nuevo legajo, el tab Sucursales es visible y la validacion requiere al menos 1 sucursal asignada
- Gestion de Fichadas, Horarios, Categorias
- Gestion de Sectores, Sucursales, Terminales
- Feriados, Incidencias, Vacaciones
- Noticias y Variables del sistema
- Gestion de Usuarios (Identity)
- Tablas con nombres singulares (Legajo, Fichada, Sucursal, etc.)
- Header personalizado con logo y nombre de empresa (endpoint `/api/empresa-logo` con cache 1 hora)
- Proteccion contra double-submit en login (deshabilita boton mientras procesa)
- Verificacion de estado de empresa en login: si la empresa esta suspendida en DigitalPlusAdmin, rechaza el acceso (fail-open)
- Forzar cambio de contraseña en primer login (MustChangePassword con middleware + pagina ForceChangePassword)
- Auto-provisioning de usuario admin al crear empresa desde Portal Licencias (credenciales temporales)
- Hosting: Azure App Service (digitalplusportalmt.azurewebsites.net)
- **Identidad visual:** Theme oscuro con paleta integraia.tech (fondos #050810/#0B1120, acentos dorados #C9A84C, texto claro #E8EAF0, contenido #F8F7F4)
- Branding: "DIGITAL ONE" en sidebar

### 3.3a Terminal Movil - Digital One Mobile (PWA)

| Atributo | Detalle |
|---|---|
| **Tipo** | PWA (Progressive Web App) servida desde el portal |
| **Stack** | HTML + CSS + JavaScript estatico |
| **Carpeta** | `PortalMultiTenant/src/DigitalPlusMultiTenant.Web/wwwroot/mobile/` |
| **URL** | `https://digitalplusportalmt.azurewebsites.net/mobile/` |
| **Proposito** | Fichada desde smartphone con GPS + validacion de ubicacion |

**Estado:** Funcional y probado end-to-end (login, activacion, fichada GPS, historial).

**UI:** Tema oscuro navy (#0A1628), reloj en vivo, mapa GPS con anillos animados, boton teal rectangular, badges de precision GPS.

**Funcionalidades:**
- API REST en Portal MT: `POST /api/mobile/login`, `POST /api/mobile/registrar-dispositivo`, `POST /api/mobile/fichada`, `GET /api/mobile/estado`
- JWT Bearer auth (convive con cookie auth del portal, no interfiere)
- Login por PIN del legajo (SHA256), no usa Identity del portal
- Registro de dispositivo con codigo de activacion de uso unico (24hs)
- Validacion de ubicacion por GPS con Haversine (configurable por sucursal)
- Fichada restringida a sucursales asignadas al legajo (tabla LegajoSucursal)
- Verificacion de firma RSA del dispositivo en cada fichada
- Fichadas se insertan en tabla `Fichada` existente con `Origen = Movil`
- Un empleado = un dispositivo activo
- PWA instalable (manifest.json, service worker, iconos)
- Compatibilidad iOS Safari y Android Chrome

**Control de acceso (MobileHabilitado):**
- **Nivel empresa:** Flag `MobileHabilitado` en tabla `Empresa` (DigitalPlusMultiTenant) y `Empresas` (DigitalPlusAdmin). Se gestiona desde Portal Licencias. Si esta desactivado, el menu del Portal MT oculta las opciones de fichado movil.
- **Nivel legajo:** Flag `MobileHabilitado` en tabla `Legajo`. Se gestiona desde el formulario de Legajos en Portal MT. Controla si el empleado individual puede usar fichado movil.
- Ambos flags se validan en `MobileController.Login()`.

**Administracion:**
- Tab "Movil" en formulario Legajos del Administrador desktop (generar codigo, desactivar dispositivo)
- Pagina `/terminales-moviles` en Portal MT (listado de dispositivos, generar codigos de activacion)
- Pagina `/fichado-movil` en Portal MT (geoconfig por sucursal)
- Config GPS integrada en formulario de Sucursal con mapa Leaflet/OpenStreetMap (buscador de direccion, geocoding, marker arrastrable, circulo de radio)
- Gestion de PIN desde formulario de Legajos en Portal MT (asignar, cambiar, resetear)
- Menu condicional: links de fichado movil solo visibles cuando empresa tiene `MobileHabilitado = true`

**Tablas nuevas en DigitalPlusMultiTenant:**
- `TerminalesMoviles` - Smartphones registrados por empleado
- `SucursalGeoconfigs` - Config WiFi/GPS por sucursal
- `CodigosActivacionMovil` - Codigos de vinculacion dispositivo-empleado

### 3.3b DigitalPlusWeb - LEGACY

| Atributo | Detalle |
|---|---|
| **Tipo** | Aplicacion web (LEGACY - obsoleta) |
| **Stack** | Blazor Server / .NET 7 / EF Core + Dapper |
| **Proyecto** | `PortalWeb\` |
| **Hosting** | Ferozo (produccion) |
| **Proposito** | Gestion administrativa via web (reemplazada por Portal Multi-Tenant) |

**Nota:** Este componente esta siendo reemplazado por el Portal Multi-Tenant. No se realizan nuevos desarrollos sobre esta version.

### 3.4 Portal de Licencias (DigitalPlus.Licencias)

| Atributo | Detalle |
|---|---|
| **Tipo** | Aplicacion web (backoffice) |
| **Stack** | Blazor Server / .NET 10 / EF Core |
| **Solucion** | `DigitalPlus.Licencias.sln` |
| **Repositorio** | Copia sincronizada en `PortalLicencias\` dentro del repo principal |
| **Proposito** | Administracion de empresas, licencias y provisionamiento |

**Funcionalidades:**
- Dashboard con estadisticas
- ABM de Empresas (con wizard de alta en 6 pasos, incluyendo auto-provisioning de usuario admin en Portal MT)
- Seccion "Identidad de la Empresa": logo, pagina web, redes sociales (Facebook, Instagram, LinkedIn, X/Twitter, YouTube, TikTok)
- Gestion de Licencias y codigos de activacion
- Gestion de Usuarios administradores (CRUD con roles, registro publico deshabilitado)
- Desactivacion de empresas: cambiar Estado a "suspendida" o "baja" bloquea acceso en Portal MT y apps desktop
- Log de auditoria
- Seccion "Suscripcion Lemon Squeezy" en detalle de empresa: Customer ID, Subscription ID, estado, vencimiento
- Cancelacion de suscripcion via API de Lemon Squeezy (PATCH cancelled=true)
- Atributos del sistema (Paises, Tipos de identificacion fiscal)
- API REST `/api/activar` para el instalador liviano (retorna connectionString, adminConnectionString, empresaId, adminEmpresaId)
- API REST `/api/verificar-estado` para que apps desktop verifiquen si la empresa esta activa
- Provisionamiento de bases de datos en Ferozo
- Proteccion contra double-submit en login
- Hosting: Azure App Service (digitalpluslicencias.azurewebsites.net)
- **Identidad visual:** Misma paleta integraia.tech que Portal MT (oscuro + dorado)
- Branding: "DIGITAL ONE Licencias" en sidebar

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
- `POST /api/lsq/create-checkout` - Crear checkout de Lemon Squeezy para upgrade de plan
- `POST /api/lsq/webhook` - Recibir webhooks de Lemon Squeezy (subscription_created/updated/payment_success/expired/cancelled)
- `POST /api/lsq/cancel-subscription` - Cancelar suscripcion activa (PATCH cancelled=true al final del periodo)

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
- Llama a API del Portal de Licencias en Azure (`/api/activar`) para obtener connection strings
- Configura dos connection strings: `Local` (DigitalPlusMultiTenant con dp_app_svc) y `Admin` (DigitalPlusAdmin)
- Configura appSettings: `EmpresaId`, `AdminEmpresaId`, `NombreEmpresa`
- Instala: Fichador + Administrador + Driver DigitalPersona RTE
- Cifra configuracion con DPAPI via ConfigProtector
- Al ejecutar el Fichador por primera vez, la terminal se auto-registra en la BD asociandose a la sucursal por defecto

---

## 4. LIBRERIAS COMPARTIDAS (Common)

Todas las aplicaciones de escritorio comparten estas librerias:

| Libreria | Responsabilidad |
|---|---|
| **Acceso.Clases.Datos** | Entidades de dominio RRHH, acceso a datos especifico del negocio, clases PIN, EmpresaInfoService (consulta info de empresa + redes sociales desde DigitalPlusAdmin) |
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
| `DigitalPlusMultiTenant` | localhost / Ferozo | BD operativa multi-tenant (todas las empresas en una sola BD, filtradas por EmpresaId) |
| `DigitalPlus` | Ferozo | BD de DigitalPlusWeb LEGACY (produccion) |
| `DigitalPlusAdmin` | Ferozo | BD administrativa: licencias, empresas, codigos, auditoria |

### Datos migrados

- **Kosiuko:** EmpresaId=2, 758 legajos, 784K fichadas (SuperAdmin EmpresaId=1)

### Tablas principales (BD DigitalPlusMultiTenant)

Las tablas usan **nombres singulares** y todas las tablas principales incluyen `EmpresaId` para el filtrado multi-tenant:

**Tablas principales (con EmpresaId):**
- `Legajo` - Empleados (NumeroLegajo, Apellido, Nombre, IsActive, MobileHabilitado, PIN, PinExpiraEn, PinMustChange)
- `Fichada` - Registro de fichadas (FechaHora en lugar de Registro). FK compuesto `(LegajoId, EmpresaId) → Legajo(Id, EmpresaId)` impide mezcla cross-tenant a nivel BD
- `Sucursal` - Sucursales (Direccion, Localidad, Provincia, Telefono, Email + geoconfig GPS)
- `Horario` - Horarios
- `Categoria` - Categorias
- `Sector` - Sectores
- `Incidencia` - Incidencias (Color como string hex en lugar de ForeColor/BackColor int)
- `Feriado` - Feriados (Fecha unica en lugar de Desde/Hasta)
- `Terminal` - Terminales

**Tablas Terminal Movil v2 (con EmpresaId):**
- `TerminalesMoviles` - Smartphones registrados (DeviceId, PublicKey RSA, Plataforma)
- `SucursalGeoconfigs` - Config geolocalizacion por sucursal (WiFi BSSID, GPS lat/lon, radio, metodo)
- `CodigosActivacionMovil` - Codigos de vinculacion dispositivo-empleado (uso unico, expira 24hs)

**Tablas child (sin EmpresaId, filtradas por JOIN con tabla padre):**
- `LegajoHuella` - Huellas digitales binarias (FingerMask en lugar de nFingerMask)
- `LegajoPin` - PINs de legajos
- `LegajoSucursal` - Relacion legajo-sucursal
- `LegajoDomicilio` - Domicilios de legajos
- `HorarioDetalle` - Detalle de horarios

### Mapeo de columnas clave (legacy -> multi-tenant)

| Legacy | Multi-Tenant | Tabla |
|---|---|---|
| LegajoId | NumeroLegajo | Legajo |
| Activo | IsActive | Legajo |
| Nombre (completo) | Apellido + Nombre (separados) | Legajo |
| Registro | FechaHora | Fichada |
| nFingerMask | FingerMask | LegajoHuella |
| ForeColor/BackColor (int) | Color (string hex) | Incidencia |
| Desde/Hasta | Fecha (unica) | Feriado |

### Patron TenantContext

```csharp
// Global.Datos.TenantContext - lee EmpresaId de app.config, fallback a 1
int empresaId = Global.Datos.TenantContext.EmpresaId;
// Todas las queries agregan: WHERE EmpresaId = @empresaId
```

### Variables globales relevantes

- `FichadaModoPIN` - Habilita fichada por PIN
- `PinExpiraDias` - Dias de expiracion del PIN (0=no expira)
- `FichadaModoDemo` - Habilita modo demostracion

### Tablas administrativas (DigitalPlusAdmin)

- `Empresas` - Registro de empresas clientes (CodigoActivacion, Logo, PaginaWeb, redes sociales, LsqCustomerId, LsqSubscriptionId, LsqVariantId, LsqStatus, PlanVencimiento, PlanOrigen)
- `SolicitudUpgrade` - Solicitudes de upgrade de plan (legacy, reemplazado por Lemon Squeezy)
- `Licencias` - Estado de licencias por empresa/maquina
- `LicenciasLog` - Auditoria de operaciones de licencia
- `LicenseCodes` - Codigos de activacion de uso unico
- `ActivationCodes` - Codigos de activacion del instalador
- `Paises` - 10 paises LATAM
- `TiposIdentificacionFiscal` - 11 tipos (CUIT, CUIL, RUC, RFC, NIT, RUT, CNPJ)

### Acceso a datos

| Componente | Patron |
|---|---|
| Fichador/Administrador | ADO.NET puro + SQL directo multi-tenant via `Global.Datos.SQLServer` (SPs legacy reemplazados) |
| Portal Multi-Tenant | EF Core / .NET 10 |
| DigitalPlusWeb (legacy) | EF Core + Dapper |
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

### Modo multi-tenant

En modo multi-tenant, la validacion de licencia esta **deshabilitada** en las apps de escritorio (Fichador y Administrador). La activacion se realiza durante la instalacion, a traves del endpoint `/api/activar` del Portal de Licencias, invocado por el instalador liviano.

### Seguridad del licenciamiento

- Firma RSA-SHA256 (clave privada en Azure, publica embebida en cliente)
- DPAPI Machine + entropy para cache local (`license.dat`)
- ClockGuard detecta manipulacion de reloj del sistema
- MachineId = SHA256(MachineGuid + BiosSerial + ProcessorId)
- Codigos de uso unico con flag `UsedAt` en BD
- UPDLOCK + HOLDLOCK en stored procedures para evitar race conditions

### Pasarela de pago: Lemon Squeezy

Lemon Squeezy opera como Merchant of Record para pagos internacionales (Argentina no esta soportada por Stripe). La integracion incluye:

- **Checkout:** Portal MT llama a Azure Function que crea checkout en LSQ y redirige al usuario
- **Webhooks:** Azure Function recibe eventos de LSQ y actualiza Empresa + Licencia en DigitalPlusAdmin
- **Cancelacion:** PATCH con `cancelled=true` cancela al final del periodo actual (no inmediato)
- **Mapeo de planes:** Configurado via variables de entorno `LemonSqueezy:VariantMap:{variantId}` en Azure Functions
- **Alertas:** LicenciaAlerts muestra warning/danger cuando la suscripcion esta cancelada y proxima a vencer
- **Suscripcion expirada (jaula):** Middleware en Portal MT redirige a `/configuracion/planes` cuando la suscripcion expira. El usuario puede recontratar desde ahi
- **Confirmacion inline:** Todas las acciones destructivas en 9 listados del Portal MT requieren Confirmar/Cancelar antes de ejecutar

| Evento LSQ | Accion del sistema |
|---|---|
| subscription_created | Guarda IDs LSQ, actualiza plan y limites desde PlanConfig. Lee `custom_data.plan` para Enterprise |
| subscription_payment_success | Renueva PlanVencimiento, reactiva si estaba suspendida |
| subscription_updated | Actualiza LsqStatus/VariantId, cambia plan si cambio variante |
| subscription_cancelled | Marca LsqStatus='cancelled', plan sigue activo hasta PlanVencimiento |
| subscription_expired | Degrada a Free, limpia datos LSQ |

### Plan Enterprise

El plan Enterprise soporta dos modalidades:

**Enterprise Manual (PlanOrigen="manual"):**
- IntegraIA activa el plan directamente desde Portal Licencias (boton "Activar plan Enterprise manualmente")
- El pago se gestiona fuera de Lemon Squeezy (transferencia, factura, acuerdo directo)
- Se establece fecha de vencimiento manual. El middleware de jaula NO aplica para Enterprise manual
- IntegraIA renueva manualmente cuando corresponda

**Enterprise LSQ (PlanOrigen="lsq"):**
- IntegraIA crea un producto con precio personalizado en el dashboard de Lemon Squeezy
- Carga el Variant ID en Portal Licencias (campo `LsqVariantIdCustom` en tabla Empresas)
- Genera link de pago desde Portal Licencias (checkout directo con `custom_data.plan="enterprise"`)
- El cliente paga y LSQ cobra automaticamente mes a mes o anualmente
- El webhook lee `custom_data.plan` y activa el plan Enterprise con cobro recurrente

**Flujo de solicitud Enterprise (Portal MT):**
1. Cliente ve card Enterprise en `/configuracion/planes` y hace click en "Solicitar Enterprise"
2. Se crea SolicitudSoporte con Tipo="Enterprise" + Noticia en dashboard + email a notify@integraia.tech
3. IntegraIA recibe la solicitud, negocia y activa el plan (manual o LSQ segun acuerdo)
4. La solicitud se marca como Completada automaticamente al activar o revertir el plan
5. Portal Licencias permite revertir de Enterprise a otro plan (Free/Basic/Pro)

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
| SuscripcionCancelada | Suscripcion LSQ cancelada, vence pronto | Si, recontratar plan |

---

## 7. SEGURIDAD SQL

### Estado actual

Se implemento un modelo de seguridad con usuarios SQL dedicados y separacion de responsabilidades:

| Usuario | Rol | Usado por | Proposito |
|---|---|---|---|
| `sa` | Superadmin | Portal Licencias (CloudSql) | Provisioning: CREATE DATABASE, schema |
| `dp_app_svc` | `dp_role_app` | Apps desktop (via instalador) | Operaciones CRUD en DigitalPlusMultiTenant |
| `dp_admin_svc` | Administracion | Azure Functions | Licenciamiento |
| `dp_web_svc` | Web | DigitalPlusWeb (deploy pausado) | Portal legacy |

### Separacion CloudSql vs ClientSql

El Portal de Licencias maneja dos conexiones SQL diferenciadas:

- **CloudSql** (`sa`): Usado internamente para provisioning (crear empresas, esquemas, INSERT iniciales). Solo ejecuta en el servidor del portal.
- **ClientSql** (`dp_app_svc`): Usado para generar el connection string que reciben las apps desktop via `/api/activar`. El usuario `dp_app_svc` tiene permisos granulares (SELECT/INSERT/UPDATE/DELETE en 29 tablas + EXECUTE en 35 SPs) definidos por el rol `dp_role_app`.

### Script de creacion

`AzureProvisioning/tools/create-dp-app-svc.ps1` - Crea el login `dp_app_svc`, el rol `dp_role_app` con permisos granulares en DigitalPlusMultiTenant.

### Fases del plan de migracion

| Fase | Estado |
|---|---|
| 1. Scripts SQL en Ferozo | COMPLETADA |
| 2. Crear dp_app_svc con permisos granulares | COMPLETADA |
| 3. Separar CloudSql/ClientSql en Portal Licencias | COMPLETADA |
| 4. Deploy appsettings.json DigitalPlusWeb con dp_web_svc | PAUSADO |
| 5. Monitoreo conexiones | PENDIENTE |
| 6. Deshabilitar sa | PENDIENTE |

---

## 8. ESTRUCTURA DE REPOSITORIOS

### Repositorio principal: DigitalPlusSuiteMultiTenant

```
C:\Apps\Claude\Huellas\DigitalPlusSuiteMultiTenant\
|
+-- Fichador\                        App Fichador
|   +-- TEntradaSalida\              Proyecto principal (.csproj, WinForms .NET 4.8)
|
+-- Administrador\                   App Administrador
|   +-- Acceso\                      Proyecto principal (.csproj, WinForms .NET 4.8)
|
+-- Common\                          Librerias compartidas
|   +-- Acceso.Clases.Datos\         Capa de datos RRHH
|   +-- Global.Datos\                Infraestructura ADO.NET + TenantContext
|   +-- Global.Funciones\            Utilidades
|   +-- Global.Controles\            Controles WinForms custom
|   +-- DigitalPlus.Licensing\       Sistema de licencias
|   +-- Global.DigitalPersona\       SDK huella digital
|
+-- PortalMultiTenant\               Portal Multi-Tenant (Blazor Server .NET 10)
|
+-- PortalLicencias\                 Portal de Licencias (copia sincronizada, Blazor .NET 10)
|
+-- PortalWeb\                       DigitalPlusWeb LEGACY (.NET 7, no modificar)
|
+-- Instaladores\
|   +-- InstaladorUnificado\         Instalador completo (Local + Nube)
|   +-- InstaladorLiviano\           Instalador cloud-only (~25MB)
|
+-- Database\                        Scripts SQL, migraciones
|
+-- SDK\                             DigitalPersona SDK
|
+-- Tools\                           Herramientas (ConfigProtector)
|
+-- Docs\                            Documentacion
|
+-- SitioWeb\                        Sitio web integraia.tech (HTML, deploy Hostinger)
|   +-- images\                     Imagenes del sitio
|   +-- instalador\                 Instalador descargable (nombre fijo)
|
+-- AzureProvisioning\               Azure Functions
```

### Control de versiones con Git y GitHub

**Repositorio remoto:** `GOAssad/DigitalPlusSuiteMultiTenant` en GitHub.

**Estrategia de trabajo:** Se trabaja directamente sobre el branch `master`. No se usan feature branches ya que el desarrollo es centralizado (un solo desarrollador + Claude Code). Los cambios se commitean y pushean a GitHub de forma incremental.

**Tags de respaldo:** Antes de iniciar etapas de desarrollo que incorporan funcionalidad nueva significativa, se crea un **tag anotado** en Git para marcar un punto de restauracion estable. Esto permite volver al estado exacto del proyecto en caso de que el nuevo desarrollo introduzca problemas irrecuperables.

| Tag | Commit | Fecha | Descripcion |
|---|---|---|---|
| `v1.0-pre-mobile` | `730589f` | 2026-03-12 | **Version 1.0** — Snapshot estable antes de desarrollo Terminal Movil. Circuito completo probado en produccion: Fichador, Administrador, Portal MT, Portal Licencias. |

**Versionado del proyecto:**
- **v1.0** — Suite completa funcional: Fichador (Huella/PIN/Demo), Administrador, Portal MT, Portal Licencias, Instaladores, Azure Functions. Probado en produccion con Kosiuko y New Family.
- **v2.0** — Incorpora Terminal Movil como PWA (fichado desde smartphone con GPS + validacion de ubicacion). Backend, PWA y admin completos. Control MobileHabilitado a nivel empresa y legajo.

**Versionado automatico de builds:**
- Todas las aplicaciones (Fichador, Administrador, Portal MT, Portal Licencias) incluyen numero de version automatico con formato `1.0.0-YYYYMMDDHHMI`
- El script `Tools/update-build-number.ps1` genera/actualiza `BuildInfo.cs` en cada proyecto con la fecha y hora de compilacion
- La clase `BuildInfo` expone `BuildInfo.Version` para mostrar en UI (About, title bar, footer)

**Como restaurar desde un tag:**
```bash
# Ver el estado del proyecto en ese punto
git checkout v1.0-pre-mobile

# Volver al desarrollo actual
git checkout master
```

**Convenciones de commits:**
- Mensajes descriptivos en español
- Co-autoría con Claude Code cuando corresponde
- Sin feature branches: todo va a `master`
- Push manual a GitHub (no automatico)

### Repositorios LEGACY (ARCHIVADOS - NO MODIFICAR)

- `C:\Apps\Claude\Huellas\DigitalPlusDesk_Claude\` - Apps desktop legacy (archivado)
- `C:\Apps\Claude\Huellas\DigitalPlusWeb_Claude\` - Portal web legacy (archivado)

### Portal de Licencias (dentro del repo principal)

El Portal de Licencias esta sincronizado dentro del repo principal en `PortalLicencias\`, asegurando que todo el codigo sea recuperable desde GitHub con un solo `git clone`.

---

## 9. HERRAMIENTAS NECESARIAS

### Para desarrollo

| Herramienta | Version | Proposito |
|---|---|---|
| Visual Studio | 2019 o superior | IDE principal para Fichador/Administrador |
| .NET Framework 4.8 SDK | - | Compilacion apps desktop |
| .NET 7 SDK | - | DigitalPlusWeb (legacy) |
| .NET 10 SDK | - | Portal Multi-Tenant, Portal Licencias |
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

## 10. ESTADO ACTUAL DEL PROYECTO (Marzo 2026 - Actualizado 2026-03-24)

### Completado

- Fichador con 3 modos de fichada (Huella, PIN, Demo)
- Administrador con gestion de PIN y configuracion
- **Arquitectura multi-tenant implementada** (TenantContext, EmpresaId, tablas singulares)
- **BD DigitalPlusMultiTenant** creada en Ferozo con datos Kosiuko migrados (EmpresaId=2, 758 legajos, 784K fichadas)
- **Kosiuko registrada en DigitalPlusAdmin** (CodigoActivacion=EE509930E07E)
- **Cambio voluntario de PIN en Fichador** (FrmCambiarPinVoluntario)
- **Mejoras en gestion de PINs en Administrador** (filtro, resetear, forzar cambio, muestra todos los legajos)
- **Validacion de licencia deshabilitada en apps desktop** para modo multi-tenant
- **InstaladorLiviano listo para produccion**: API URL apunta a Azure, connection strings con dp_app_svc, AdminConnection y AdminEmpresaId
- **Llamadas a SP legacy reemplazadas** con SQL directo multi-tenant
- Sistema de licencias end-to-end (trial -> bloqueo -> activacion)
- Azure Functions para licenciamiento desplegadas
- Instalador Unificado v1.3 (Local + Nube)
- Instalador Liviano v1.0 compilado OK (25MB)
- **Seguridad SQL: dp_app_svc creado** con permisos granulares (dp_role_app), separacion CloudSql/ClientSql en Portal Licencias
- API `/api/activar` en portal: retorna connectionString (dp_app_svc), adminConnectionString, empresaId (MT), adminEmpresaId
- API `/api/verificar-estado` en portal: permite a apps desktop verificar si empresa esta activa
- Alta de empresa desde portal (crea BD + esquema + usuario admin en Portal MT con password temporal)
- Logo empresa + IntegraIA en Fichador y Administrador (EmpresaInfoService con conexion a DigitalPlusAdmin)
- Portal Multi-Tenant: logo y nombre de empresa en header, endpoint `/api/empresa-logo`, fix double-submit login
- Portal Multi-Tenant: verificacion de estado de empresa en login, forzar cambio de contraseña en primer acceso
- Portal Multi-Tenant: **deployado en Azure** (digitalplusportalmt.azurewebsites.net)
- Portal Licencias: seccion "Identidad de la Empresa" con web y 6 redes sociales
- Portal Licencias: gestion de usuarios administradores (CRUD), registro publico deshabilitado
- Portal Licencias: **deployado en Azure** (digitalpluslicencias.azurewebsites.net)
- Administrador: menu dinamico con links a redes sociales (reemplaza botones fijos hardcodeados)
- **Desactivacion de empresas**: desde Portal Licencias cambiar Estado bloquea acceso en Portal MT (login) y apps desktop (Form_Load)
- Portal Licencias sincronizado en repo principal (todo respaldado en GitHub)
- **Homologacion visual Phase 2:** Layout 80/20 en formulario Legajos, panel de camara ensanchado, boton PIN reubicado
- **Homologacion visual Phase 3-4:** Portales MT y Licencias con tema oscuro integraia.tech (dorado/oscuro)
- **Fix foto legajo:** Parametro @Foto en SP y capa de datos para persistir foto en BD
- **Fix PIN forzado:** Dialogo obligatorio (solo OK) sin opcion de escape
- **Auto-registro de terminal:** Fichador registra automaticamente la maquina en la BD al primer inicio
- **Compilacion Release:** Fichador y Administrador compilados en Release, InstaladorLiviano generado
- **Terminal Movil v2 - Backend:** MobileController (4 endpoints JWT), entidades EF Core, migracion aplicada en Ferozo, UbicacionService (WiFi/GPS/Haversine)
- **Terminal Movil v2 - Admin:** Tab "Movil" en FrmRRHHLegajos (generar codigo, desactivar dispositivo), DALs desktop (TerminalMovilDAL, SucursalGeoconfigDAL)
- **Terminal Movil v2 - Portal MT:** Paginas `/terminales-moviles` y `/fichado-movil`, NavMenu actualizado
- **Tag v1.0-pre-mobile** creado como punto de restauracion (commit 730589f)
- **Terminal Movil PWA:** App PWA completa en wwwroot/mobile/ (login, activacion, fichada GPS, historial). Probada end-to-end en iPhone y Android
- **MobileHabilitado (empresa):** Flag en tabla Empresa (DigitalPlusMultiTenant) y Empresas (DigitalPlusAdmin). Checkbox en Portal Licencias (EmpresaDetalle). Claim en login del Portal MT. Menu condicional en NavMenu
- **MobileHabilitado (legajo):** Flag en tabla Legajo. Checkbox en LegajoForm del Portal MT (visible solo si empresa tiene mobile habilitado)
- **Gestion de PIN desde Portal MT:** Asignar, cambiar y resetear PIN desde LegajoForm (tab Datos, seccion PIN Movil)
- **Iconos de origen en fichadas:** Columna Origen muestra iconos visuales (huella, PIN, movil, manual, web, demo) en vez de badges de texto
- **Campo DatabaseName editable:** En Portal Licencias, el campo "Base de datos" de la empresa ahora es editable. Constraint UNIQUE eliminado (multi-tenant comparte BD)
- **Rediseno PWA Mobile:** Tema oscuro, mapa GPS con anillos animados, reloj en vivo, boton teal, GPS watch continuo con precision
- **CRUD Sucursales mejorado:** Nuevos campos (Direccion, Localidad, Provincia, Telefono, Email), mapa Leaflet/OpenStreetMap integrado con buscador Nominatim, geocoding/reverse geocoding, circulo de radio
- **Validacion GPS por sucursal asignada:** Fichada movil ahora valida que el legajo tenga la sucursal asignada (LegajoSucursal) antes de resolver GPS
- **Roles en Portal MT:** SuperAdmin, AdminEmpresa, Operador, Consulta con permisos por pagina (Form pages restringidos, botones ocultos con AuthorizeView)
- **Portal Licencias - Limpiar/Eliminar empresa:** Limpiar elimina datos transaccionales (fichadas, vacaciones, etc) manteniendo entidades; Eliminar borra todo de MT y Admin. Doble confirmacion con escritura del nombre
- **Portal Licencias - Dashboard "Uso del sistema":** Mini dashboard en detalle de empresa (legajos, fichadas, usuarios, terminales, ultima fichada, fichadas por origen, dias activos 30d, fichadas 15d por dispositivo)
- **Email de activacion movil:** Servicio SMTP via MailKit (smtp.hostinger.com:465 SSL, notify@integraia.tech). Envio automatico al generar codigo de activacion desde LegajoForm
- **Pestaña Movil en LegajoForm:** Estado del dispositivo, generar codigo de activacion, envio automatico por email con deep link, boton reintentar/reenviar
- **Deep link PWA:** URL con ?code=XXX pre-carga el codigo de activacion en la app movil
- **Fix re-activacion DeviceId:** Si legajo ya tiene terminal activa y cambia DeviceId (cache borrada), el login actualiza automaticamente sin pedir codigo nuevo
- **Contraste visual mejorado:** Labels uppercase bold, inputs con borde/sombra, card headers oscuros con linea dorada en ambos portales
- **Icono PWA actualizado:** D1 dorado sobre fondo oscuro (192px/512px)
- **Portal Licencias: Tab Legajos en detalle empresa:** Lista cross-DB con NumeroLegajo, Apellido, Nombre, Categoria, Sucursales, Fichadas, Ultima fichada, Estado. Buscador en tiempo real. Carga async
- **Fix critico cross-tenant login movil:** MobileController.Login ahora filtra por empresa del dispositivo registrado. Si dispositivo no registrado y legajo ambiguo, rechaza
- **Fix cross-tenant PIN desktop:** RRHHLegajosPin ahora pasa @EmpresaId a todos los stored procedures (VerificarPin, CambiarPin, CargarLegajo, ListaLegajosActivos)
- **FK compuesto cross-tenant en Fichada:** `Fichada(LegajoId, EmpresaId) → Legajo(Id, EmpresaId)` impide insertar fichadas con legajo de otra empresa a nivel BD
- **Modo Kiosko + Fichada QR:** Terminal kiosko (ModoKiosko=1, SucursalId fija) para fichada por QR en dispositivos compartidos (tablets). QrToken por legajo (GUID, unique index). Endpoint `POST /api/mobile/fichar-qr` con validacion empresa+sucursal+cooldown. UI kiosko web en `/kiosko/` con html5-qrcode para escaneo de camara
- **PWA "Mi QR":** Tercera tab en PWA mobile donde el empleado ve su QR para mostrar en el kiosko
- **KioskoHabilitado:** Flag por empresa (Empresa.KioskoHabilitado) con switch en Portal Licencias, claim en login, menu condicional
- **QR en Portal MT:** Boton QR por legajo en lista (modal con QR visual), impresion masiva de credenciales QR, auto-generacion de QrToken al crear legajo
- **Fix cross-tenant PIN SPs:** `EscritorioLegajoPIN_Verificar` y `EscritorioLegajoPIN_Cambiar` ahora filtran por @EmpresaId
- **Fix cambio de plan:** Al cambiar plan en Portal Licencias, ahora aplica automaticamente limites de PlanConfig (MaxLegajos, MaxSucursales, MaxFichadasMes)
- **Origen QR en fichadas:** Icono bi-qr-code en LegajoForm, FichadasList y AsistenciaDiaria
- **Hora fichada QR:** Usa Clock.Now (Argentina) en vez de DateTime.UtcNow. Cooldown compara contra CreatedAt (UTC)
- **Fichador desktop QR (Fase 6):** Modo QR con camara USB en Fichador WinForms. AForge.Video.DirectShow + ZXing.Net. Timer 250ms decode, cooldown 5seg, BuscarPorQrToken() con GUID validation
- **Rediseno Fichador dark theme:** Full dark theme (#0D111C), botones de modo pill (Huella/PIN/QR/Demo), form 620x660, cards azul profundo, acentos dorados, TextBoxes oscuros
- **Fix cierre Fichador con camara:** Flag volatile `_cerrando` + `BeginInvoke` (no `Invoke`) elimina deadlock entre thread AForge y UI thread al cerrar form
- **Fichada.Origen cambiado a string:** Propiedad `string?` en vez de `OrigenFichada?` (enum). BD es nvarchar, desktop escribe "PIN"/"QR"/"Huella" via SP. Razor compara strings directos
- **Fix icono PIN:** `bi-dialpad` no existe en Bootstrap Icons, cambiado a `bi-keyboard`
- **Fix Asistencia Diaria:** Columnas Horas/Origen estaban invertidas en el body. Origen ahora muestra todos los origenes distintos del dia (multiples badges)
- **Fix concurrencia FichadasList:** Guard `_cargando` evita doble click en Buscar que lanzaba 2 queries concurrentes al DbContext
- **Instalador Liviano:** Incluye DLLs AForge.dll, AForge.Video.dll, AForge.Video.DirectShow.dll, zxing.dll
- **Sistema de versionado automatico:** `BuildInfo.cs` generado por `Tools/update-build-number.ps1` con formato `1.0.0-YYYYMMDDHHMI` en todas las apps (Fichador, Administrador, Portal MT, Portal Licencias)
- **Fix credenciales en endpoint /api/activar:** Ahora devuelve email+password del usuario admin creado, para que el instalador pueda mostrarlos al usuario
- **Boton "Activar Dispositivo" en Terminales Moviles:** Permite generar codigo de activacion sin necesidad de tener una terminal previa registrada
- **Fix cross-tenant login movil con codigo de activacion:** Validacion corregida para que el codigo de activacion solo funcione dentro de la empresa correcta
- **Fix timezone fichada movil:** Usa `Clock.Now` (hora Argentina) en vez de `DateTime.UtcNow` para registrar la hora correcta
- **Anti-duplicado en SP EscritorioFichadasSPSALIDA:** Hint `UPDLOCK` + cooldown de 30 segundos para evitar fichadas duplicadas por doble-click o lecturas rapidas
- **Importador Excel de legajos:** Carga masiva desde .xlsx con plantilla descargable, preview con validacion, matcheo por codigo o nombre
- **Calendario visual:** Tipo Google Calendar en tab Legajo (grilla mensual, EventoCalendario con HoraDesde/HoraHasta)
- **EventoCalendario integrado en 4 reportes:** Ausencias (motivo), LlegadasTarde/AsistenciaDiaria/HorasTrabajadas (excluye)
- **Suspender/reactivar empresa:** Portal Licencias zona peligrosa, sincroniza Estado (admin) + IsActive (MT), revalidacion auth cada 1 min
- **Validacion GPS mejorada:** Toggle deshabilitado sin coordenadas, auto-activa al poner ubicacion, default geoActivo=false
- **Reescritura panel camara Administrador:** Sin AForge.Controls, PictureBox estandar, patron lock+BeginInvoke, deteccion camara ocupada
- **SitioWeb integrado al repo:** integraia.tech con pagina producto, help online con 30+ anclas, ayuda contextual en Portal MT
- **Lemon Squeezy como pasarela de pago:** Checkout, webhooks (5 eventos), cancelacion, alertas, recontratar. Store ID 324804, Azure Functions
- **Suscripcion expirada jaula:** Middleware redirige a /planes, LicenciaAlerts con warning/danger/expirado, boton recontratar
- **Confirmacion inline:** Patron Confirmar/Cancelar en 9 listados del Portal MT
- **PermiteMovil condicionado:** Requiere legajo.MobileHabilitado=true, checkbox disabled si no, validacion end-to-end
- **Fichador: refresh sucursal + instalador selector sucursal:** oTerminal.Inicializar() antes de cada fichada, /api/activar devuelve sucursales
- **Plan Enterprise completo:** Vista dedicada en Portal MT (ilimitado), solicitud con registro+email, activacion manual o LSQ, reversion de plan
- **Administrador simplificado:** Solo 2 pestanas (Legajo: huellas+foto, Movil), datos solo lectura, alta/edicion exclusiva desde Portal MT
- **Permisos fichada por sucursal:** PermiteHuella/Pin/Qr/Movil/Kiosko verificados end-to-end (SP desktop + MobileController + Kiosko)
- **PlanConfig.Valor decimal:** Soporta precios con centavos (14.50, 29.99), precios en USD, anual con descuento dinamico

### En progreso

- Migracion de seguridad SQL: deshabilitar sa (pendiente estabilizacion)

### Pendiente
- **Enterprise LSQ end-to-end:** Probar flujo completo con variant custom en Lemon Squeezy (producto por cliente, cobro automatico)
- **SuperAdmin cross-tenant:** admin@integraia.tech debe poder seleccionar empresa y ver datos de cualquier tenant
- **TimeZone por Sucursal:** Campo TimeZone en tabla Sucursal para soporte multi-pais (reemplazar Clock.Now hardcodeado)
- **Probar circuito completo con usuario externo:** Enviar instalador a un usuario real
- Deploy `dp_web_svc` en DigitalPlusWeb (pausado)
- Deshabilitar usuario `sa`
- Paginacion en Asistencia Diaria del portal MT

### Prioridad baja

- Auto-fetch datos fiscales (AFIP, etc.)
- WhatsApp integration
- Reportes ampliados en web
- Limpieza dead code Administrador

---

## 11. OBSERVACIONES ARQUITECTONICAS

### Fortalezas

1. **Arquitectura multi-tenant** con BD unica y filtrado por EmpresaId - escalable y mantenible
2. **Arquitectura instalable** con licenciamiento centralizado - ventaja competitiva real
3. **Dos modos de despliegue** (local con SQL Express, nube con Ferozo) cubren distintos segmentos
4. **Licenciamiento robusto** con firma RSA, anti-tampering, y gestion centralizada
5. **Portal de administracion** permite operar el negocio de licencias de forma profesional
6. **Separation of concerns** entre apps desktop, web, y Azure Functions

### Puntos de atencion

1. Las apps desktop usan .NET Framework 4.8 (legacy, sin soporte LTS). Evaluar migracion a .NET 8+ en el futuro.
2. No hay tests automatizados en ningun componente
3. Sin sistema de logging estructurado en las apps desktop
4. El acceso a datos en desktop es via clase estatica `SQLServer` (acoplamiento fuerte)
5. Todo el codigo unificado en un solo repositorio GitHub (DigitalPlusSuiteMultiTenant), incluyendo Portal Licencias sincronizado

### Riesgos

1. **Dependencia de Ferozo:** Si Ferozo cae, todas las instalaciones nube quedan sin servicio
2. **sa aun activo en CloudSql:** Usado solo para provisioning, pero pendiente plan de reemplazo
3. **Sin backup automatizado** documentado para las BD de empresas en Ferozo
4. **SDK DigitalPersona** es un componente legacy sin actualizaciones recientes

---

*Fin del Reporte de Arquitectura para Project Leader*
