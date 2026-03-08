# PORTAL DE LICENCIAS DIGITALPLUS - Manual para Integra IA

**Version:** 1.0
**Fecha:** 2026-03-07
**Audiencia:** Equipo interno de Integra IA (administradores del sistema)

---

## INDICE

1. [Introduccion](#1-introduccion)
2. [Acceso al Portal](#2-acceso-al-portal)
3. [Dashboard](#3-dashboard)
4. [Gestion de Empresas](#4-gestion-de-empresas)
5. [Alta de Nueva Empresa (Wizard)](#5-alta-de-nueva-empresa-wizard)
6. [Detalle de Empresa](#6-detalle-de-empresa)
7. [Gestion de Licencias](#7-gestion-de-licencias)
8. [Codigos de Licencia](#8-codigos-de-licencia)
9. [Log de Auditoria](#9-log-de-auditoria)
10. [Atributos del Sistema](#10-atributos-del-sistema)
11. [Flujo Operativo Completo](#11-flujo-operativo-completo)
12. [API de Activacion](#12-api-de-activacion)
13. [Referencia Tecnica](#13-referencia-tecnica)

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
| Produccion | `https://licencias.digitaloneplus.com` (pendiente deploy) |

### Credenciales por defecto

| Campo | Valor |
|---|---|
| Email | `admin@digitalplus.com` |
| Password | `Admin123` |

> **Importante:** Cambie la contrasena por defecto despues del primer acceso en produccion.

### Pantalla de Login

1. Ingrese su email y contrasena
2. Haga clic en **Iniciar Sesion**

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
- Busqueda por nombre
- Filtro por estado (activa, trial, suspendida)
- Acceso al detalle de cada empresa

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

Cada vez que un nuevo cliente contrata DigitalPlus, debe crearse su empresa en el portal. El wizard automatiza todo el proceso en **5 pasos**.

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

### Paso 2 - Creacion de base de datos

El sistema crea automaticamente una base de datos en Ferozo para la nueva empresa:
- Nombre de la BD: `DP_{nombre_empresa_normalizado}` (ej: `DP_integra_ia_srl`)
- Se ejecuta el SchemaScript.sql completo (29 tablas, stored procedures, vistas, datos iniciales)
- Este proceso toma entre 30 segundos y 2 minutos

> [CAPTURA: Barra de progreso de creacion de base de datos]

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

### Que hacer despues del alta

1. **Copiar el codigo de activacion**
2. **Enviarselo al cliente** (por email, WhatsApp, etc.)
3. El cliente lo usara en el **Instalador Liviano** para conectarse a su base de datos

---

## 6. DETALLE DE EMPRESA

Al hacer clic en una empresa del listado, se abre su ficha completa.

### Datos editables

Todos los datos ingresados en el alta son editables:
- Nombre, datos fiscales, contacto, direccion
- Los selects de Pais y Tipo de ID Fiscal funcionan en cascada

> [CAPTURA: Formulario de edicion de empresa con todos los campos]

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

---

## 7. GESTION DE LICENCIAS

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

## 8. CODIGOS DE LICENCIA

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

## 9. LOG DE AUDITORIA

### Para que sirve

Registra **todas las operaciones** del sistema de licenciamiento:
- Activaciones (trial y con codigo)
- Heartbeats
- Suspensiones y reactivaciones
- Errores de validacion

### Acceso

Desde el menu lateral, acceda a **Log**.

### Columnas del log

| Columna | Descripcion |
|---|---|
| Fecha | Timestamp de la operacion |
| Accion | Tipo de operacion (activate, heartbeat, suspend, etc.) |
| Empresa | Empresa involucrada |
| App | Aplicacion que genero el evento (Fichador, Administrador) |
| IP | Direccion IP del cliente |
| Detalles | Informacion adicional |

> [CAPTURA: Pantalla de log de auditoria con filtros y grilla]

---

## 10. ATRIBUTOS DEL SISTEMA

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

## 11. FLUJO OPERATIVO COMPLETO

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
   - Codigo de activacion (para instalador)
   - Link de descarga del Instalador Liviano
   - Instrucciones de instalacion
   |
   v
4. CLIENTE: Ejecutar Instalador Liviano
   - Ingresa el codigo de activacion
   - El instalador llama a /api/activar del portal
   - Obtiene connection string y configura las apps
   |
   v
5. CLIENTE: Usar Fichador y Administrador
   - Apps conectan a la BD en Ferozo
   - Sistema arranca en modo Trial (14 dias, 5 legajos)
   |
   v
6. INTEGRA IA: Generar codigo de licencia (cuando el cliente paga)
   Portal > Codigos > Generar codigo (plan, legajos, duracion)
   |
   v
7. INTEGRA IA: Enviar codigo de licencia al cliente
   |
   v
8. CLIENTE: Activar licencia
   Administrador > Menu Licencias > Ingresar codigo
   El trial se convierte en licencia activa
   |
   v
9. MONITOREO CONTINUO
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

1. Generar un nuevo **codigo de licencia** con el plan y legajos deseados
2. Enviar el codigo al cliente
3. El cliente lo ingresa desde Administrador > Licencias
4. La licencia se actualiza automaticamente

### Caso: Suspender un cliente (impago, etc.)

1. Ir a **Licencias** > seleccionar la licencia del cliente
2. Hacer clic en **Suspender**
3. El cliente tiene **7 dias de gracia** antes del bloqueo total
4. Para reactivar: hacer clic en **Reactivar**

---

## 12. API DE ACTIVACION

### Endpoint

```
POST /api/activar
```

### Proposito

Este endpoint es invocado por el **Instalador Liviano** durante la instalacion para obtener los datos de conexion de la empresa.

### Request

```json
{
  "Codigo": "ABC123XYZ"
}
```

### Response (exito - 200)

```json
{
  "connectionString": "Server=sd-1985882-l.ferozo.com,11434;Database=DP_mi_empresa;...",
  "companyId": "mi_empresa",
  "nombreEmpresa": "Mi Empresa SRL",
  "databaseName": "DP_mi_empresa"
}
```

### Response (error - 404)

```json
{
  "error": "Codigo invalido o empresa no encontrada"
}
```

### Notas

- El codigo es el **CodigoActivacion** de la empresa (no el codigo de licencia)
- No requiere autenticacion (el codigo es la credencial)
- El connection string se arma dinamicamente con `BuildClientConnectionString()`

---

## 13. REFERENCIA TECNICA

### Estructura de la base de datos administrativa (DigitalPlusAdmin)

| Tabla | Proposito |
|---|---|
| Empresas | Registro de empresas clientes |
| Licencias | Estado de licencias por empresa/maquina |
| LicenciasLog | Auditoria de operaciones |
| LicenseCodes | Codigos de uso unico para activar licencias |
| ActivationCodes | Codigos de activacion del instalador (legacy) |
| Paises | Tabla de paises |
| TiposIdentificacionFiscal | Tipos de documento fiscal por pais |
| AspNet* | Tablas de Identity para autenticacion del portal |

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
| Connection string admin | appsettings.json | Conexion a DigitalPlusAdmin |
| Connection string cloud | appsettings.json (CloudSql) | Conexion para crear BDs de empresas |
| Identity config | Program.cs | Configuracion de autenticacion |

---

*Fin del Manual del Portal de Licencias para Integra IA*
