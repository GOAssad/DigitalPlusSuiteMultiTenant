# PORTAL DE LICENCIAS DIGITALPLUS - Manual para Integra IA

**Version:** 14.0
**Fecha:** 2026-03-25
**Audiencia:** Equipo interno de Integra IA (administradores del sistema)

---

## INDICE

1. [Introduccion](#1-introduccion)
2. [Acceso al Portal](#2-acceso-al-portal)
3. [Dashboard](#3-dashboard)
4. [Gestion de Empresas](#4-gestion-de-empresas)
5. [Alta de Nueva Empresa (Wizard)](#5-alta-de-nueva-empresa-wizard)
6. [Detalle de Empresa](#6-detalle-de-empresa)
7. [Desactivacion de Empresas](#7-desactivacion-de-empresas)
8. [Gestion de Licencias](#8-gestion-de-licencias)
9. [Codigos de Licencia](#9-codigos-de-licencia)
10. [Log de Auditoria](#10-log-de-auditoria)
11. [Atributos del Sistema](#11-atributos-del-sistema)
12. [Gestion de Usuarios](#12-gestion-de-usuarios)
13. [Flujo Operativo Completo](#13-flujo-operativo-completo)
14. [APIs del Portal](#14-apis-del-portal)
15. [Referencia Tecnica](#15-referencia-tecnica)

---

## 1. INTRODUCCION

### Que es el Portal de Licencias

El Portal de Licencias es la aplicacion de **backoffice** utilizada internamente por Integra IA para:

- **Crear y gestionar empresas clientes**
- **Provisionarles una base de datos** en el servidor de Ferozo
- **Generar codigos de activacion** para que los clientes instalen el software
- **Administrar licencias** (planes, vencimientos, bloqueos)
- **Auditar** todas las operaciones del sistema de licenciamiento

### Quien usa este portal

Exclusivamente el equipo de Integra IA. Los clientes finales **nunca** acceden a este portal. Ellos interactuan con el sistema a traves de las aplicaciones de escritorio (Fichador y Administrador) y el portal web de su empresa.

### Stack tecnico

| Componente | Tecnologia |
|---|---|
| Frontend + Backend | Blazor Server (.NET 10) |
| Base de datos | SQL Server en Ferozo (DigitalPlusAdmin) |
| Autenticacion | ASP.NET Identity |
| ORM | Entity Framework Core |

---

## 2. ACCESO AL PORTAL

### URL

| Entorno | URL |
|---|---|
| Local (desarrollo) | `https://localhost:7200` |
| Produccion | `https://digitalpluslicencias.azurewebsites.net` |

### Credenciales por defecto

| Campo | Valor |
|---|---|
| Email | `admin@digitalplus.com` |
| Password | `Admin123` |

> **Importante:** Cambie la contrasena por defecto despues del primer acceso en produccion.

### Pantalla de Login

1. Ingrese su email y contrasena
2. Haga clic en **Iniciar Sesion**

> **Nota:** El boton de login se deshabilita automaticamente al hacer clic para evitar errores por doble-submit (proteccion contra tokens antiforgery duplicados).

> [CAPTURA: Pantalla de login del Portal de Licencias]

---

## 3. DASHBOARD

La pagina principal muestra un resumen ejecutivo del estado del sistema:

### Estadisticas visibles

- **Total de empresas** registradas
- **Licencias activas** vs trial vs vencidas
- **Empresas en modo trial** (pendientes de conversion)
- **Instalaciones sin heartbeat reciente** (posibles problemas)

> [CAPTURA: Dashboard del portal mostrando tarjetas con estadisticas]

---

## 4. GESTION DE EMPRESAS

### Listado de Empresas

Desde el menu lateral, acceda a **Empresas**. Vera un listado con todas las empresas registradas.

**Funcionalidades del listado:**
- Busqueda por nombre o Company ID
- Filtro por estado (activa, trial, suspendida)
- Boton **Editar** para acceder al detalle de cada empresa
- Boton **Eliminar** para eliminar una empresa directamente desde el listado (con doble confirmacion)

> [CAPTURA: Listado de empresas con filtro y columnas (Nombre, CUIT, Estado, Fecha alta)]

### Columnas del listado

| Columna | Descripcion |
|---|---|
| Nombre | Razon social de la empresa |
| Identificacion fiscal | CUIT/CUIL/RUC segun el pais |
| Pais | Pais de la empresa |
| Estado | Activa / Trial / Suspendida |
| Codigo Activacion | Codigo para el instalador liviano |
| Fecha de alta | Cuando fue creada |

---

## 5. ALTA DE NUEVA EMPRESA (Wizard)

### Cuando usar esta funcion

Cada vez que un nuevo cliente contrata DigitalPlus, debe crearse su empresa en el portal. El wizard automatiza todo el proceso en **6 pasos**.

### Acceso

Desde el listado de Empresas, haga clic en **Nueva Empresa**.

### Paso 1 - Datos de la empresa

Complete los datos basicos:

| Campo | Requerido | Descripcion |
|---|---|---|
| Nombre / Razon Social | Si | Nombre comercial o razon social |
| Pais | Si | Seleccionar de la lista (10 paises LATAM) |
| Tipo de ID Fiscal | Si | Se actualiza segun el pais (CUIT, RUC, RFC...) |
| Numero de ID Fiscal | Si | Numero de identificacion fiscal |
| Email de contacto | No | Email del contacto principal |
| Telefono | No | Telefono de contacto |
| Direccion | No | Direccion fisica |

> [CAPTURA: Formulario de datos de empresa con campos y selects cascading (Pais -> Tipo ID)]

### Paso 2 - Provisionamiento en base de datos multi-tenant

El sistema provisiona la nueva empresa dentro de la base de datos compartida `DigitalPlusMultiTenant` en Ferozo:
- **No se crea una BD separada por empresa.** Todas las empresas comparten la misma base de datos.
- Se asigna un `EmpresaId` unico a la nueva empresa dentro de la BD compartida.
- El esquema tiene **29 tablas con nombres en singular** (Legajo, Fichada, Sucursal, Sector, Horario, Categoria, Incidencia, Feriado, etc.).
- Todos los datos se filtran por `EmpresaId`, garantizando el aislamiento entre empresas.
- Se crean los datos iniciales (incidencias predeterminadas, etc.) asociados al nuevo `EmpresaId`.

> [CAPTURA: Barra de progreso de provisionamiento de empresa]

### Paso 3 - Registro de empresa

Se registra la empresa en la tabla `Empresas` de DigitalPlusAdmin con un **Codigo de Activacion** generado automaticamente.

### Paso 4 - Creacion de licencia

Se crea una licencia inicial para la empresa:
- Tipo: Trial (o segun lo que seleccione)
- MachineId: 'pending' (se actualizara cuando el cliente instale)

### Paso 5 - Confirmacion

Se muestra un resumen con:
- Datos de la empresa creada
- Codigo de activacion (para enviar al cliente)
- Estado de la base de datos

> [CAPTURA: Pantalla de confirmacion mostrando resumen y codigo de activacion]

### Paso 6 - Auto-provisioning de usuario admin

Al completar el wizard, el sistema automaticamente:
1. Crea un registro `Empresa` en la BD `DigitalPlusMultiTenant` con el codigo de la empresa
2. Crea un usuario admin en `AspNetUsers` de DigitalPlusMultiTenant con email `admin@{companyId}.com`
3. Genera una **contraseña temporal** (10 caracteres aleatorios) que se muestra en pantalla
4. Asigna el rol `AdminEmpresa` al usuario
5. Marca `MustChangePassword = 1` para forzar cambio en el primer login del Portal MT

> **Importante:** La contraseña temporal se muestra **una sola vez** en la pantalla de confirmacion. Debe copiarla y enviarsela al cliente junto con el codigo de activacion.

### Que hacer despues del alta

1. **Copiar el codigo de activacion** y la **contraseña temporal del portal**
2. **Enviarselo al cliente** junto con el archivo del instalador (`DigitalPlus_Cloud_Setup_v1.0.exe`)
3. El cliente ejecuta el **Instalador Liviano**, ingresa el codigo de activacion, y el sistema se configura automaticamente
4. Al abrir el **Fichador** por primera vez, la computadora se registra automaticamente como terminal asociada a la sucursal principal — **no es necesario dar de alta terminales manualmente**
5. El cliente usara el email y contraseña temporal para acceder al **Portal Web** (se le pedira cambiar la contraseña en el primer acceso)

### Que enviar al cliente

Envie al cliente los siguientes elementos (por email, WhatsApp, etc.):

| Elemento | Ejemplo | Para que lo necesita |
|---|---|---|
| Instalador | `DigitalPlus_Cloud_Setup_v1.0.exe` | Instalar Fichador y Administrador |
| Codigo de activacion | `3CCA-6A4C-C675-A967-21A2-F95D` | Activar la instalacion (lo pide el instalador) |
| Email del portal | `admin@{companyId}.com` | Acceder al Portal Web |
| Contraseña temporal | (la que se mostro en el wizard) | Primer login en el Portal Web |
| URL del portal | `https://digitalplusportalmt.azurewebsites.net` | Direccion del portal web |

---

## 6. DETALLE DE EMPRESA

Al hacer clic en una empresa del listado, se abre su ficha completa.

### Navegacion por tabs

El detalle de empresa organiza la columna principal en **dos tabs**:

| Tab | Contenido |
|---|---|
| **Datos** | Formulario de edicion de la empresa (datos generales, fiscales, contacto, identidad, notas) + boton Guardar |
| **Legajos** | Listado de legajos de la empresa consultados cross-database desde DigitalPlusMultiTenant |

### Tab Datos

Todos los datos ingresados en el alta son editables:
- Nombre, datos fiscales, contacto, direccion
- Los selects de Pais y Tipo de ID Fiscal funcionan en cascada
- **Base de datos:** Campo editable que indica a que BD se conecta la empresa (por defecto `DigitalPlusMultiTenant`). Todas las empresas comparten la misma BD en el modelo multi-tenant actual, pero el campo queda como registro y preparacion para eventual sharding.

### Modulos opcionales

En la seccion "Datos generales" hay un toggle para activar/desactivar modulos opcionales:

- **Fichado Movil habilitado:** Permite a los empleados de la empresa fichar ingreso/egreso desde el celular. Si esta desactivado:
  - El login mobile (`/api/mobile/login`) rechaza el acceso con mensaje "La empresa no tiene habilitado el modulo movil"
  - En el Portal MT, la opcion "Fichado Movil" se oculta del menu lateral
  - El checkbox "Acceso movil" por legajo no aparece en el formulario de Legajos del Portal MT

- **Modo Kiosco habilitado:** Permite fichar mediante QR en terminales compartidas (tablets/PCs en sucursales). Si esta desactivado:
  - No se pueden registrar terminales en modo kiosko
  - La pagina `/kiosko/` no funciona para esa empresa

- **Nota:** "Terminales Moviles" aparece en el menu del Portal MT si Mobile **o** Kiosco esta habilitado. El usuario debe cerrar sesion y volver a iniciarla para ver los cambios (los flags se cargan como claims al momento del login).

### Dashboard "Uso del sistema"

En la barra lateral derecha del detalle de empresa, la card **"Uso del sistema"** muestra estadisticas en tiempo real consultadas a DigitalPlusMultiTenant:

- **Indicadores principales:** Legajos activos, fichadas totales, usuarios del portal
- **Infraestructura:** Sucursales, terminales desktop, terminales moviles
- **Ultima fichada:** Fecha/hora, cantidad de fichadas y legajos que ficharon ese dia
- **Origen fichadas (ultimo dia):** Badges con iconos (huella, PIN, movil, manual, web, demo)
- **Dias con actividad (ult. 30d):** Badge verde (>=20), amarillo (>=10), rojo (<10)
- **Fichadas por dispositivo (ult. 15 dias):** Huella, PIN, Celular con iconos y contadores

### Tab Legajos

El tab **Legajos** muestra informacion cross-database consultada en tiempo real desde DigitalPlusMultiTenant:

- **Badge en el tab** con cantidad total de legajos de la empresa
- **Buscador** por nombre, apellido o numero de legajo (filtro en tiempo real con `oninput`)
- **Tabla scrollable** (max 500px) con columnas: Nro, Apellido, Nombre, Categoria, Sucursales, Fichadas (badge), Ultima fichada, Estado (Activo/Inactivo)
- Legajos inactivos se muestran con fila gris (`table-secondary`)
- Carga asincronica en background (no bloquea la UI)
- **Estados de carga:** spinner mientras carga, mensaje "sin legajos" si la empresa no tiene, error con detalle y boton "Reintentar" si falla la conexion

> Esta informacion es de **solo lectura**. Para gestionar legajos (alta, baja, modificacion), usar el Portal Multi-Tenant de la empresa.

> [CAPTURA: Tab Legajos mostrando listado con buscador y tabla de legajos]

### Zona peligrosa

Al final de la barra lateral hay una card **"Zona peligrosa"** con dos operaciones destructivas:

**Limpiar Empresa:**
- Elimina datos transaccionales: fichadas, vacaciones, incidencias asignadas, eventos calendario, terminales moviles, codigos de activacion movil
- Mantiene: empresa, usuarios, sucursales, sectores, categorias, horarios, terminales, legajos (con huellas, PINs), incidencias, feriados, noticias, codigo de activacion
- La empresa queda en estado "recien creada" pero con toda su estructura intacta
- **Doble confirmacion:** primero aceptar la advertencia, luego escribir el nombre exacto de la empresa

**Eliminar Empresa:**
- Elimina absolutamente todo de DigitalPlusMultiTenant (datos, entidades, usuarios, empresa)
- Elimina licencias y registro de la empresa en DigitalPlusAdmin
- **Irreversible.** No queda nada
- **Doble confirmacion** igual que Limpiar
- Redirige a la lista de empresas despues de eliminar

### Identidad de la Empresa

La seccion **"Identidad de la Empresa"** permite gestionar la imagen corporativa del cliente:

**Logo:**
- Subir logo en formato PNG, JPG, SVG o WebP (max 500 KB)
- Vista previa del logo actual
- Boton para quitar el logo

**Pagina web y redes sociales:**
- **Pagina web** (icono de globo) - URL del sitio corporativo
- **Facebook** - URL del perfil de Facebook
- **Instagram** - URL del perfil de Instagram
- **LinkedIn** - URL del perfil de LinkedIn
- **X (Twitter)** - URL del perfil de X/Twitter
- **YouTube** - URL del canal de YouTube
- **TikTok** - URL del perfil de TikTok

Cada campo tiene su icono oficial de la red social para facil identificacion. Todos los campos son opcionales.

> **Importante:** Los datos de identidad se usan en las apps de escritorio del cliente. El menu del Administrador desktop muestra automaticamente links a cada red social que tenga URL cargada. Si un campo esta vacio, no aparece en el menu.

> [CAPTURA: Seccion Identidad de la Empresa con logo y campos de redes sociales]

### Codigo de Activacion

En la seccion de codigo de activacion puede:

- **Ver** el codigo actual
- **Copiar** al portapapeles (boton de copiar)
- **Regenerar** un nuevo codigo (invalida el anterior)

> [CAPTURA: Seccion de codigo de activacion con botones Copiar y Regenerar]

> **Atencion:** Al regenerar un codigo, el anterior deja de funcionar. Si el cliente ya instalo con el codigo anterior, no se ve afectado (la conexion ya fue configurada). El nuevo codigo es necesario solo para nuevas instalaciones.

### Licencias asociadas

Se muestra el listado de licencias vinculadas a la empresa con:
- Tipo (trial/active/suspended)
- MachineId
- Plan y cantidad de legajos
- Fecha de vencimiento
- Ultimo heartbeat

> [CAPTURA: Lista de licencias de la empresa con estado y detalles]

### Seccion Suscripcion Lemon Squeezy

Visible solo cuando la empresa tiene `PlanOrigen = "lsq"`. Muestra:
- Customer ID (con link al dashboard de Lemon Squeezy)
- Subscription ID
- Origen del plan (badge)
- Proxima renovacion
- Estado (Activa/Sin suscripcion)
- Boton **Cancelar suscripcion** (con modal de confirmacion)

---

## 7. DESACTIVACION DE EMPRESAS

### Para que sirve

Permite **suspender el acceso** de una empresa cliente a todo el sistema DigitalPlus. Util en casos de impago, fin de contrato, o cualquier situacion que requiera cortar el servicio.

### Como desactivar una empresa

1. Ir a **Empresas** en el menu lateral
2. Hacer clic en **Editar** en la empresa a desactivar
3. En el campo **Estado**, cambiar de "Activa" a:
   - **Suspendida**: Bloqueo temporal (reactivable)
   - **Baja**: Bloqueo definitivo
4. Hacer clic en **Guardar**

### Efecto inmediato

El cambio de estado tiene efecto inmediato en todos los componentes:

| Componente | Comportamiento cuando empresa NO esta "activa" |
|---|---|
| **Portal Multi-Tenant** | El login es rechazado con mensaje "El acceso a su empresa ha sido suspendido" |
| **Fichador** | Al iniciar muestra "Acceso Suspendido" y se cierra |
| **Administrador** | Al iniciar muestra "Acceso Suspendido" y se cierra |

> **Nota:** Si las apps desktop ya estan abiertas al momento de la desactivacion, seguiran funcionando hasta que se reinicien. La verificacion se realiza al iniciar la aplicacion.

### Como reactivar una empresa

Mismo procedimiento: editar la empresa y cambiar el Estado a **Activa**. El acceso se restaura inmediatamente.

### Patron fail-open

Si la verificacion de estado falla (por ejemplo, si no hay conexion a DigitalPlusAdmin), el sistema **permite el acceso** por defecto. Esto evita que una falla de red bloquee a todos los clientes.

---

## 8. GESTION DE LICENCIAS

### Listado de Licencias

Desde el menu lateral, acceda a **Licencias**. Vera todas las licencias del sistema.

> [CAPTURA: Listado de licencias con filtros por estado y empresa]

### Detalle de Licencia

Al hacer clic en una licencia, puede ver y modificar:

| Campo | Descripcion |
|---|---|
| Empresa | Empresa a la que pertenece |
| MachineId | Identificador unico del equipo |
| Tipo | trial / active / suspended |
| Plan | basic / professional / enterprise |
| MaxLegajos | Cantidad maxima de empleados permitidos |
| TrialEndsAt | Fecha de fin del trial |
| ExpiresAt | Fecha de vencimiento de la licencia |
| LastHeartbeat | Ultimo heartbeat recibido |

> [CAPTURA: Formulario de detalle de licencia con todos los campos]

### Acciones sobre licencias

| Accion | Descripcion |
|---|---|
| **Modificar plan** | Cambiar de basic a professional, etc. |
| **Extender dias** | Agregar dias de vigencia |
| **Suspender** | Bloquear temporalmente (7 dias de gracia) |
| **Reactivar** | Desbloquear una licencia suspendida |

---

## 9. CODIGOS DE LICENCIA

### Que son

Los codigos de licencia son **codigos de uso unico** con formato `XXXX-XXXX-XXXX-XXXX` que el cliente ingresa para activar su licencia (pasar de Trial a activa).

> **No confundir con el Codigo de Activacion de la empresa** (usado en el instalador). Los codigos de licencia se usan dentro de las apps para activar/upgrade la licencia.

### Generar codigos

Desde **Codigos** puede:

1. Seleccionar el **Plan** (basic, professional, enterprise)
2. Definir **MaxLegajos** (cantidad de empleados permitidos)
3. Definir **DurationDays** (duracion en dias de la licencia)
4. Hacer clic en **Generar**

> [CAPTURA: Formulario de generacion de codigos de licencia]

### Listado de codigos

El listado muestra todos los codigos generados con:
- Codigo (parcialmente oculto)
- Plan y MaxLegajos
- Estado (disponible / usado)
- Fecha de uso y MachineId (si fue usado)

> [CAPTURA: Listado de codigos de licencia con estado]

---

## 10. LOG DE ACTIVIDAD

### Para que sirve

Registra **todas las operaciones** del sistema de licenciamiento para uso interno de IntegraIA:
- Activaciones (trial y con codigo)
- Heartbeats de terminales desktop
- Upgrades desde trial
- Suspensiones y reactivaciones
- Expiraciones de licencia

Este log es **cross-tenant**: muestra actividad de TODAS las empresas en un solo lugar.

### Acceso

Desde el menu lateral, acceda a **Log**.

### Filtros disponibles

| Filtro | Descripcion |
|---|---|
| Desde/Hasta | Rango de fechas (default: ultimos 30 dias) |
| Empresa | Filtrar por CompanyId especifico (combo dinamico) |
| Accion | Tipo: activacion, heartbeat, trial_start, upgrade, suspend, unsuspend, expire |
| Buscar | Texto libre en IP, app o detalles |

### Columnas del log

| Columna | Descripcion |
|---|---|
| Fecha/Hora | Timestamp de la operacion (hora local) |
| Accion | Badge de color con icono segun tipo de operacion |
| Empresa | CompanyId de la empresa involucrada |
| App | Aplicacion que genero el evento (Fichador, Administrador) |
| IP | Direccion IP del cliente |
| Detalles | Informacion adicional (plan, legajos, etc.) |

### Paginacion y Export

- Los resultados se paginan de a 50 registros con navegacion anterior/siguiente
- Boton **CSV** para exportar los registros visibles

### Depuracion

En la zona inferior (marcada en rojo), se pueden eliminar logs antiguos seleccionando una antiguedad minima (3, 6, 12 o 24 meses). La operacion se ejecuta en batches para no saturar el transaction log de Ferozo.

### Diferencia con Auditoria del Portal MT

Este log registra **actividad de licenciamiento** (heartbeats, activaciones, etc.) y es para uso de IntegraIA. La **Auditoria del Portal MT** registra operaciones CRUD de cada empresa (crear legajos, modificar horarios, etc.) y es para el administrador de la empresa.

---

## 11. ATRIBUTOS DEL SISTEMA

### Para que sirve

Desde **Atributos** se gestionan las tablas de referencia utilizadas en todo el sistema.

### Paises

ABM de paises disponibles para seleccionar al crear empresas.
- Precargados: Argentina, Chile, Uruguay, Paraguay, Brasil, Bolivia, Peru, Colombia, Mexico, Ecuador

> [CAPTURA: ABM de Paises]

### Tipos de Identificacion Fiscal

ABM de tipos de documento fiscal, vinculados a paises.
- Precargados: CUIT, CUIL (Argentina), RUC (Peru, Ecuador), RFC (Mexico), NIT (Colombia, Bolivia), RUT (Chile, Uruguay), CNPJ (Brasil)

> [CAPTURA: ABM de Tipos de Identificacion Fiscal]

> **Nota:** No se pueden eliminar tipos de identificacion que estan en uso por alguna empresa (integridad referencial).

---

## 12. GESTION DE USUARIOS

### Para que sirve

Permite gestionar los usuarios administradores que tienen acceso al Portal de Licencias. El registro publico esta **deshabilitado** - solo un administrador existente puede crear nuevos usuarios.

### Acceso

Desde el menu lateral, acceda a **Usuarios**.

### Crear un usuario

1. Haga clic en **Nuevo Usuario**
2. Complete los campos:
   - **Email**: sera el nombre de usuario para el login
   - **Contraseña**: minimo 6 caracteres
3. Haga clic en **Crear**

El nuevo usuario se crea con el rol "Administrador" y acceso completo al portal.

### Resetear contraseña

1. En el listado de usuarios, haga clic en **Editar** junto al usuario
2. Ingrese la nueva contraseña
3. Haga clic en **Guardar**

### Eliminar un usuario

1. Haga clic en **Eliminar** junto al usuario
2. Confirme la eliminacion

> **Nota:** El usuario `admin@digitalplus.com` no puede ser eliminado (proteccion contra quedarse sin acceso).

### Usuarios por defecto

| Email | Password | Notas |
|---|---|---|
| `admin@digitalplus.com` | `Admin123` | Usuario principal del Portal de Licencias. No eliminable. Cambiar contraseña en produccion. |

### Credenciales SuperAdmin - Portal Multi-Tenant y Apps Desktop

El SuperAdmin es el usuario de IntegraIA con acceso cross-tenant (puede acceder a cualquier empresa).

| Email | Password | Rol | EmpresaId | Notas |
|---|---|---|---|---|
| `admin@integraia.tech` | `Admin123` | SuperAdmin | 1 (IntegraIA) | Acceso total a todas las empresas. Cambiar password en produccion. |
| `admin@kosiuko.com` | `Admin123` | AdminEmpresa | 2 (Kosiuko) | Admin de la empresa Kosiuko, solo accede a su empresa. |

> **Importante:**
> - El SuperAdmin (`admin@integraia.tech`) puede iniciar sesion en **cualquier instalacion desktop** (Fichador o Administrador) sin importar el EmpresaId configurado. Util para soporte tecnico remoto.
> - Los usuarios con rol AdminEmpresa solo pueden acceder a la empresa que tienen asignada (validado contra el EmpresaId del app.config de la instalacion).
> - **Cambiar la contraseña por defecto** de ambos usuarios en produccion.

### Jerarquia de roles (Portal Multi-Tenant)

| Rol | Acceso | Puede crear/editar |
|---|---|---|
| `SuperAdmin` | Acceso global cross-tenant. Solo para IntegraIA. No aparece en combo de creacion de usuarios. | Todo |
| `AdminEmpresa` | Acceso total a la empresa. Gestiona usuarios, configuracion, estructura. | Todo dentro de su empresa |
| `Operador` | Gestiona legajos, fichadas y vacaciones. No puede modificar estructura (sucursales, horarios, etc). | Legajos, vacaciones |
| `Consulta` | Solo lectura, reportes y exportacion. No ve botones de crear/editar/eliminar. | Nada |

---

## 13. CONFIGURACION DE PLANES

### Acceso

Menu lateral > **Planes**

### Que es

La pagina de Configuracion de Planes permite definir los parametros y limites de cada plan de licencia. Los valores se almacenan en la tabla `PlanConfig` de DigitalPlusAdmin y se aplican al crear nuevas licencias.

### Planes disponibles

| Plan | Descripcion |
|---|---|
| **Free** | Plan gratuito sin limite de tiempo. Funcionalidad limitada. |
| **Basic** | Plan de pago basico. 1 ano de duracion. |
| **Pro** | Plan profesional con mas capacidad. 1 ano de duracion. |
| **Enterprise** | Plan empresarial sin limites funcionales. 1 ano de duracion. |

### Parametros configurables

| Parametro | Descripcion | Valores ejemplo |
|---|---|---|
| **MaxLegajos** | Cantidad maxima de legajos que la empresa puede crear | Free=5, Basic=25, Pro=100, Enterprise=0 (ilimitado) |
| **MaxSucursales** | Cantidad maxima de sucursales | Free=1, Basic=3, Pro=10, Enterprise=0 (ilimitado) |
| **MaxFichadasRolling30d** | Cantidad maxima de fichadas permitidas en los ultimos 30 dias (rolling) | Free=200, resto=0 (ilimitado) |
| **MaxTerminalesMoviles** | Cantidad maxima de terminales moviles permitidas (0=ilimitado) | Free=1, Basic=3, Pro=10, Enterprise=0 (ilimitado) |
| **MobileHabilitado** | Si permite fichado desde dispositivo movil (1=si, 0=no) | Todos=1 |
| **DuracionDias** | Duracion de la licencia en dias desde la activacion | Free=0 (sin vencimiento), resto=365 |
| **GraciaDias** | Dias de gracia despues del vencimiento antes de bloquear | Free=0, resto=7 |

**Convencion:** El valor `0` en parametros de cantidad (legajos, sucursales, fichadas) significa **ilimitado**. En duracion, `0` significa **sin vencimiento**.

### Como editar un parametro

1. En la card del plan, haga clic en el icono de lapiz junto al valor
2. Modifique el valor (solo acepta numeros >= 0)
3. Haga clic en el check verde para guardar

### Como agregar un nuevo parametro

En la seccion inferior "Agregar parametro":
1. Seleccione el plan
2. Escriba el nombre del parametro (ej: `MaxTerminales`)
3. Ingrese el valor
4. Opcionalmente agregue una descripcion
5. Click en "Agregar"

> **Nota:** Al agregar un parametro nuevo, debe repetir la operacion para cada plan si desea que todos lo tengan.

### Comportamiento al vencer un plan pago

Cuando un plan Basic/Pro/Enterprise vence:
1. Se activa un periodo de gracia (por defecto 7 dias)
2. Durante la gracia, el sistema funciona normalmente pero muestra advertencias
3. Despues de la gracia, el sistema se bloquea hasta renovar

### Relacion con la tabla Licencias

Cuando se crea una licencia para una empresa, los valores de `MaxLegajos`, `MaxSucursales` y `MaxFichadasMes` se copian desde PlanConfig. Esto permite hacer overrides por cliente si es necesario (editando la licencia individual sin cambiar el plan global).

### 13.9 Integracion con Lemon Squeezy

Lemon Squeezy es la pasarela de pago para suscripciones online. Cuando un cliente contrata un plan desde el Portal MT:

1. El Portal MT crea un checkout via Azure Function (`POST /api/lsq/create-checkout`)
2. El cliente completa el pago en Lemon Squeezy
3. LSQ envia un webhook (`POST /api/lsq/webhook`) a las Azure Functions
4. El webhook actualiza la tabla `Empresas` con los datos de la suscripcion y aplica los limites del plan desde `PlanConfig`

**Campos LSQ en tabla Empresas:**

| Campo | Descripcion |
|---|---|
| LsqCustomerId | ID del customer en Lemon Squeezy |
| LsqSubscriptionId | ID de la suscripcion activa |
| LsqVariantId | Variant ID del plan/periodo contratado |
| LsqStatus | Estado de la suscripcion (active, cancelled) |
| LsqUpdatePaymentUrl | URL para actualizar medio de pago |
| LsqCustomerPortalUrl | URL del portal de facturacion del cliente |
| PlanVencimiento | Fecha de proxima renovacion/cobro |
| PlanOrigen | Origen del plan: "lsq" (Lemon Squeezy) o "manual" (asignado por soporte) |

**Cancelacion desde Portal Licencias:**

En el detalle de empresa, seccion "Suscripcion Lemon Squeezy", el boton **Cancelar suscripcion** envia un PATCH a la API de Lemon Squeezy con `cancelled: true`. Esto cancela la suscripcion al final del periodo actual (no inmediatamente). El webhook `subscription_expired` degradara automaticamente a Free cuando venza.

**Variant IDs configurados:**

| Variant ID | Plan | Periodo |
|---|---|---|
| 1439760 | Basic | Mensual |
| 1439796 | Basic | Anual |
| 1439799 | Pro | Mensual |
| 1439800 | Pro | Anual |

El mapeo variant → plan se configura en las Azure Functions via variables de entorno `LemonSqueezy__VariantMap__{id}`.

### 13.10 Integracion con MercadoPago

MercadoPago es la pasarela de pago para clientes de Argentina. Usa **Checkout Pro** (pago unico) en vez de Preapproval (suscripciones recurrentes), porque Preapproval requiere que el email del pagador coincida con el email de la suscripcion.

**Flujo de pago:**

1. El Portal MT detecta que la empresa es de Argentina (PaisId=1) y muestra MercadoPago como opcion principal
2. Crea un Checkout Pro via Azure Function (`POST /api/mp/create-subscription`)
3. El precio en USD se convierte a ARS usando el tipo de cambio vigente (tabla TiposCambio)
4. El usuario paga en MercadoPago (ARS)
5. MP envia webhook (`POST /api/mp/webhook`) con `payment.approved`
6. El webhook parsea el ExternalReference (`emp_{id}_plan_{plan}_{monthly|annual}`) y actualiza licencia + vencimiento

**Campos MP en tabla Empresas:**

| Campo | Descripcion |
|---|---|
| MpSubscriptionId | ID de la preference de Checkout Pro |
| MpCustomerId | ID del pagador en MP |
| MpStatus | Estado: pending, active, cancelled |
| PlanOrigen | 'mp' cuando el pago fue via MercadoPago |

**Vencimiento automatico:**
- Mensual: 30 dias desde el pago
- Anual: 365 dias desde el pago
- Enterprise manual: sin vencimiento automatico

**Cancelacion:** Con Checkout Pro no hay suscripcion recurrente en MP. La cancelacion solo marca MpStatus='cancelled' localmente. La licencia expira naturalmente en PlanVencimiento.

**Monedas y Tipo de Cambio:**
- Tabla `Monedas`: USD (base), ARS
- Tabla `TiposCambio`: historial de conversion con VigenteDesde
- Gestion: Portal Licencias > Atributos > Monedas (CRUD + modal tipo de cambio con historial)
- Los precios se cargan una sola vez en USD (PlanConfig) y se convierten al cobrar en ARS

### 13.11 Auditoria de licencias (LicenciasLog)

Todas las operaciones sobre licencias se registran en la tabla `LicenciasLog` con el campo `App` indicando el origen:

| Origen (App) | Acciones registradas |
|---|---|
| License_Activate (SP) | trial_start, activate, upgrade_from_trial |
| License_Heartbeat (SP) | heartbeat |
| MercadoPago | mp_checkout_created, mp_payment_approved, mp_cancelled |
| LemonSqueezy | lsq_subscription_created, lsq_payment_success, lsq_plan_changed, lsq_cancelled, lsq_expired |
| AzureFunctions | payment_approved (compartido MP+LSQ al actualizar licencia) |
| PortalLicencias | create, update, plan_change, extend, suspend, unsuspend, upgrade |

La pagina **/log** del Portal Licencias muestra todos estos eventos con filtros por fecha, empresa, accion y busqueda.

---

## 14. FLUJO OPERATIVO COMPLETO

### Caso: Nuevo cliente contrata DigitalPlus

```
1. INTEGRA IA: Crear empresa en el Portal de Licencias
   Portal > Empresas > Nueva Empresa > Completar wizard
   |
   v
2. INTEGRA IA: Obtener el Codigo de Activacion
   Portal > Empresa > Codigo de Activacion > Copiar
   |
   v
3. INTEGRA IA: Enviar al cliente
   - Archivo: DigitalPlus_Cloud_Setup_v1.0.exe
   - Codigo de activacion
   - Credenciales del portal web (email + password temporal)
   - URL del portal: https://digitalplusportalmt.azurewebsites.net
   |
   v
4. CLIENTE: Ejecutar Instalador Liviano
   - Ingresa el codigo de activacion
   - El instalador llama a /api/activar del portal
   - Obtiene connection string y configura las apps
   - Se instala Fichador + Administrador + driver de huella
   |
   v
5. CLIENTE: Abrir Fichador por primera vez
   - La computadora se auto-registra como Terminal
   - Se asocia a la sucursal principal de la empresa
   - NO es necesario registrar terminales manualmente
   |
   v
6. CLIENTE: Usar el sistema
   - Apps conectan a la BD multi-tenant en Ferozo
   - En modo multi-tenant, la validacion de licencia esta deshabilitada
   - Flujo simplificado: Instalar con codigo -> Abrir -> Listo para usar
   |
   v
7. MONITOREO CONTINUO
   - Heartbeats cada 4 horas (automatico)
   - Revisar Dashboard para detectar problemas
   - Log de auditoria para diagnostico
```

### Caso: Cliente con problemas de conexion

1. El cliente reporta que el sistema esta bloqueado
2. Ir a **Log** y buscar por empresa
3. Verificar el ultimo heartbeat en **Licencias**
4. Si el heartbeat es mayor a 72 horas, indicar al cliente que revise su conexion a internet
5. Una vez restaurada la conexion, la app se desbloquea sola al reiniciar

### Caso: Cliente quiere mas legajos

> **Nota:** En modo multi-tenant (cloud), la validacion de licencia esta deshabilitada y no hay limite de legajos impuesto por licencia. La gestion de limites se realiza administrativamente desde el portal.

1. Ajustar los parametros de la empresa desde el portal de licencias
2. No se requiere accion del lado del cliente

### Caso: Suspender un cliente (impago, etc.)

**Opcion 1 - Desactivar empresa (recomendado para multi-tenant):**
1. Ir a **Empresas** > seleccionar la empresa
2. Cambiar Estado a **Suspendida**
3. Efecto inmediato: login rechazado en Portal MT, apps desktop se cierran al iniciar
4. Para reactivar: cambiar Estado a **Activa**

**Opcion 2 - Suspender licencia (para instalaciones locales):**
1. Ir a **Licencias** > seleccionar la licencia del cliente
2. Hacer clic en **Suspender**
3. El cliente tiene **7 dias de gracia** antes del bloqueo total
4. Para reactivar: hacer clic en **Reactivar**

---

## 15. APIs DEL PORTAL

### API de Activacion

```
POST /api/activar
```

Invocado por el **Instalador Liviano** durante la instalacion para obtener los datos de conexion de la empresa.

**Request:**

```json
{
  "Codigo": "EE509930E07E"
}
```

**Response (exito - 200):**

```json
{
  "connectionString": "Server=sd-1985882-l.ferozo.com,11434;Database=DigitalPlusMultiTenant;User Id=dp_app_svc;...",
  "adminConnectionString": "Server=sd-1985882-l.ferozo.com,11434;Database=DigitalPlusAdmin;...",
  "empresaId": 2,
  "adminEmpresaId": 5,
  "companyId": "kosiuko-sa",
  "nombreEmpresa": "Kosiuko S.A.",
  "databaseName": "DigitalPlusMultiTenant"
}
```

**Response (error - 404):**

```json
{
  "error": "Codigo invalido o empresa inactiva"
}
```

**Campos importantes:**

| Campo | Descripcion | Se escribe en |
|---|---|---|
| `connectionString` | Connection string a DigitalPlusMultiTenant (usa dp_app_svc) | app.config `Local` |
| `adminConnectionString` | Connection string a DigitalPlusAdmin | app.config `Admin` |
| `empresaId` | ID de la empresa en DigitalPlusMultiTenant | app.config `EmpresaId` |
| `adminEmpresaId` | ID de la empresa en DigitalPlusAdmin | app.config `AdminEmpresaId` |
| `nombreEmpresa` | Nombre comercial | app.config `NombreEmpresa` |

> **Nota:** `empresaId` y `adminEmpresaId` son valores DIFERENTES. El instalador los mapea a las appSettings correctas.

### API de Verificacion de Estado

```
POST /api/verificar-estado
```

Permite a las apps desktop verificar si la empresa esta activa. Pensado para verificaciones periodicas o al inicio.

**Request:**

```json
{
  "CompanyId": "kosiuko-sa"
}
```

**Response (exito - 200):**

```json
{
  "activa": true,
  "estado": "activa",
  "nombre": "Kosiuko S.A."
}
```

**Response empresa suspendida (200):**

```json
{
  "activa": false,
  "estado": "suspendida",
  "nombre": "Kosiuko S.A."
}
```

### API de Validacion Free

```
POST /api/validar-free
```

Validacion previa antes de registrar una empresa Free. Verifica que el email no este ya registrado en otra empresa.

**Request:**

```json
{
  "NombreEmpresa": "Mi Empresa S.A.",
  "Email": "admin@miempresa.com",
  "Pais": "Argentina"
}
```

**Response (exito - 200):**

```json
{
  "valid": true
}
```

**Response (email duplicado - 400):**

```json
{
  "error": "Ya existe una empresa registrada con ese email"
}
```

### API de Activacion Free

```
POST /api/activar-free
```

Registra una empresa nueva con plan Free sin necesidad de codigo de activacion. El endpoint es **idempotente**: si se llama dos veces con el mismo email, la segunda vez retorna los datos de la empresa existente sin crear una nueva.

**Request:**

```json
{
  "NombreEmpresa": "Mi Empresa S.A.",
  "Email": "admin@miempresa.com",
  "Pais": "Argentina"
}
```

**Response (exito - 200):**

```json
{
  "connectionString": "Server=sd-1985882-l.ferozo.com,11434;Database=DigitalPlusMultiTenant;User Id=dp_app_svc;...",
  "adminConnectionString": "Server=sd-1985882-l.ferozo.com,11434;Database=DigitalPlusAdmin;...",
  "empresaId": 5,
  "adminEmpresaId": 10,
  "companyId": "mi-empresa-sa",
  "nombreEmpresa": "Mi Empresa S.A.",
  "databaseName": "DigitalPlusMultiTenant"
}
```

**Rollback automatico:** Si el provisioning multi-tenant falla (por ejemplo, error de conexion a Ferozo), el sistema realiza rollback manual eliminando los datos parciales creados en DigitalPlusAdmin. No queda basura en la base de datos.

**Email de bienvenida:** Al completarse exitosamente el registro, se envia automaticamente un email de bienvenida al email proporcionado con las credenciales de acceso al Portal Web (email + password temporal). El envio es fire-and-forget (no bloquea la respuesta).

### API de Paises

```
GET /api/paises
```

Retorna la lista de paises disponibles para el registro Free. Usado por el instalador liviano para popular el combo de seleccion de pais.

**Response (200):**

```json
[
  { "nombre": "Argentina" },
  { "nombre": "Chile" },
  { "nombre": "Uruguay" },
  ...
]
```

### MaxTerminalesMoviles

El parametro `MaxTerminalesMoviles` en PlanConfig controla la cantidad maxima de dispositivos moviles que una empresa puede registrar:

- **0** = ilimitado (sin restriccion)
- **Entero positivo** = cantidad maxima de terminales moviles permitidas

La validacion se aplica en el endpoint `RegistrarDispositivo` del `MobileController`. Si la empresa alcanza el limite, el registro de nuevos dispositivos es rechazado.

El checkbox **MobileHabilitado** en el detalle de empresa del Portal de Licencias habilita o deshabilita el modulo movil completo. MaxTerminalesMoviles solo aplica cuando MobileHabilitado esta activo.

### Notas generales de las APIs

- No requieren autenticacion (el codigo/companyId es la credencial)
- El connection string de `/api/activar` usa el usuario SQL `dp_app_svc` (no `sa`)
- Actualmente las apps desktop verifican estado via consulta directa a DigitalPlusAdmin (connection string `Admin` en app.config), no via esta API

---

## 16. REFERENCIA TECNICA

### Estructura de bases de datos

**DigitalPlusAdmin** - Base de datos administrativa (portal de licencias):

| Tabla | Proposito |
|---|---|
| Empresas | Registro de empresas clientes (logo, identidad, redes sociales, codigo activacion) |
| Licencias | Estado de licencias por empresa/maquina |
| LicenciasLog | Auditoria de operaciones |
| LicenseCodes | Codigos de uso unico para activar licencias |
| ActivationCodes | Codigos de activacion del instalador (legacy) |
| Paises | Tabla de paises |
| TiposIdentificacionFiscal | Tipos de documento fiscal por pais |
| AspNet* | Tablas de Identity para autenticacion del portal |

**DigitalPlusMultiTenant** - Base de datos operativa compartida (multi-tenant):

- Es la **unica BD operativa** para todas las empresas (no se crea una BD por empresa).
- Contiene 32 tablas con **nombres en singular** (29 originales + 3 de Terminal Movil v2): Legajo, Fichada, Sucursal, Sector, Horario, Categoria, Incidencia, Feriado, LegajoSucursal, LegajoHuella, LegajoDomicilio, HorarioDetalle, etc.
- Todas las tablas principales incluyen la columna `EmpresaId` para aislar los datos por empresa.
- Las tablas hijas (LegajoSucursal, LegajoHuella, LegajoPin, LegajoDomicilio, HorarioDetalle) no tienen `EmpresaId` directamente; se filtran por JOIN con su tabla padre.

### Patron TenantContext

Las aplicaciones de escritorio (Fichador, Administrador) usan el patron `TenantContext` para el aislamiento multi-tenant:

- El instalador escribe el `EmpresaId` en el `app.config` de cada aplicacion.
- En runtime, `Global.Datos.TenantContext.EmpresaId` lee ese valor (fallback a 1).
- Todas las queries de acceso a datos agregan automaticamente `WHERE EmpresaId = @empresaId`.

### Stored Procedures de licenciamiento (en Azure Functions)

| SP | Funcion |
|---|---|
| `License_Activate` | Crear trial o activar con codigo |
| `License_Heartbeat` | Actualizar heartbeat y retornar estado |
| `License_ValidateAndConsumeCode` | Validar y marcar codigo como usado (atomico) |

### Scripts PowerShell de gestion (AzureProvisioning/tools/)

Estos scripts permiten gestionar licencias directamente desde la linea de comandos:

| Script | Funcion |
|---|---|
| `generate-license-code.ps1` | Genera codigo XXXX-XXXX-XXXX-XXXX |
| `list-licenses.ps1` | Lista licencias activas |
| `list-license-codes.ps1` | Lista codigos disponibles |
| `modify-license.ps1` | Modifica licencia (plan, maxLegajos, dias) |
| `suspend-license.ps1` | Suspende o reactiva licencia |

### Configuracion del portal

| Parametro | Ubicacion | Descripcion |
|---|---|---|
| DefaultConnection | appsettings.json (ConnectionStrings) | Conexion a DigitalPlusAdmin (Identity + datos admin) |
| CloudSql | appsettings.json | Conexion a Ferozo con `sa` para provisioning (CREATE DATABASE, schema) |
| ClientSql | appsettings.json | Conexion a Ferozo con `dp_app_svc` para generar connection strings de clientes |
| MultiTenant:DatabaseName | appsettings.json | Nombre de la BD multi-tenant (DigitalPlusMultiTenant) |
| Identity config | Program.cs | Configuracion de autenticacion (password min 6 chars, sin requisitos complejos) |

### Seguridad SQL

El portal maneja dos usuarios SQL diferenciados:

| Usuario | Usado para | Configurado en |
|---|---|---|
| `sa` | Provisioning (crear empresas, tablas, datos iniciales) | CloudSql |
| `dp_app_svc` | Connection strings que reciben las apps desktop | ClientSql |

> **Importante:** Las apps desktop nunca reciben `sa`. El connection string generado por `/api/activar` usa `dp_app_svc` con permisos granulares (SELECT/INSERT/UPDATE/DELETE en tablas, EXECUTE en SPs).

---

*Fin del Manual del Portal de Licencias para Integra IA*
