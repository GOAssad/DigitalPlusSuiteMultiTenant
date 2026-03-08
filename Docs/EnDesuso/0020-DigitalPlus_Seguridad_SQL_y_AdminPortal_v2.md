# DIGITALPLUS -- Seguridad SQL y Portal de Administracion (v2)

**Fecha:** 2026-03-05
**Generado por:** Claude (Opus 4.6)
**Version anterior:** DigitalPlus_Seguridad_SQL_y_AdminPortal.md (v1)
**Documentos origen:**
- 0018-DigitalPlus_Arquitectura.md (requisitos del Project Leader)
- 0019-DigitalPlus_Arquitectura_ProjectLeader.md (decisiones definitivas)

---

## RESUMEN DE CAMBIOS RESPECTO A v1

| Aspecto | v1 | v2 |
|---|---|---|
| Roles SQL | Un unico `dp_role_app` | Tres roles: `dp_role_runtime`, `dp_role_admin`, `dp_role_web` |
| Usuarios por empresa | Un usuario por empresa | Un usuario por empresa (separacion por rol preparada, no implementada) |
| CHECK_POLICY | OFF | ON por defecto, OFF solo con justificacion tecnica |
| Bootstrap | Mencionado pero no detallado | Modelo completo documentado |
| Compatibilidad | Implicita | Politica explicita con reglas claras |
| Responsabilidades | No definidas | Matriz completa por componente |
| Portal Admin | Estructura basica | Dashboard con alertas, control manual, auditoria |
| Validacion Ferozo | Mencionada como riesgo | Procedimiento tecnico completo |

---

## 1. MODELO DEFINITIVO DE SEGURIDAD SQL

### 1.1 Roles

El sistema define tres roles de base de datos con permisos diferenciados segun el principio de menor privilegio.

#### dp_role_runtime

**Usado por:** Fichador, procesos automaticos

```sql
CREATE ROLE dp_role_runtime;

-- SELECT solo sobre tablas necesarias para fichadas y validacion
GRANT SELECT ON dbo.Legajos TO dp_role_runtime;
GRANT SELECT ON dbo.LegajosHuellas TO dp_role_runtime;
GRANT SELECT ON dbo.Horarios TO dp_role_runtime;
GRANT SELECT ON dbo.Sucursales TO dp_role_runtime;
GRANT SELECT ON dbo.Terminales TO dp_role_runtime;
GRANT SELECT ON dbo.VariablesGlobales TO dp_role_runtime;
GRANT SELECT ON dbo.Fichadas TO dp_role_runtime;
GRANT SELECT ON dbo.LegajosSucursales TO dp_role_runtime;

-- INSERT solo en Fichadas (registrar fichadas)
GRANT INSERT ON dbo.Fichadas TO dp_role_runtime;

-- EXECUTE sobre SPs de fichadas
GRANT EXECUTE ON dbo.EscritorioFichadasSPSALIDA TO dp_role_runtime;

-- Restricciones explicitas:
-- NO DELETE
-- NO ALTER
-- NO CREATE TABLE
-- NO DROP
-- NO UPDATE (excepto via SP)
```

**Justificacion:** El Fichador es una aplicacion de kiosko que solo necesita leer legajos/huellas y registrar fichadas. Limitar sus permisos reduce el riesgo ante compromiso de la estacion.

---

#### dp_role_admin

**Usado por:** Administrador (aplicacion de escritorio)

```sql
CREATE ROLE dp_role_admin;

-- CRUD amplio (necesario para gestion de legajos, huellas, horarios, etc.)
GRANT SELECT ON SCHEMA::dbo TO dp_role_admin;
GRANT INSERT ON SCHEMA::dbo TO dp_role_admin;
GRANT UPDATE ON SCHEMA::dbo TO dp_role_admin;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_admin;

-- DELETE solo sobre tablas especificas donde es necesario
GRANT DELETE ON dbo.LegajosHuellas TO dp_role_admin;
GRANT DELETE ON dbo.Fichadas TO dp_role_admin;
GRANT DELETE ON dbo.IncidenciasLegajos TO dp_role_admin;
GRANT DELETE ON dbo.Horarios TO dp_role_admin;

-- Restricciones explicitas:
-- NO db_owner
-- NO ALTER
-- NO CREATE TABLE
-- NO DROP
```

**Justificacion:** El Administrador gestiona legajos, huellas, horarios e incidencias. Necesita CRUD completo pero con DELETE restringido a las tablas donde realmente se borran registros. Nunca debe poder modificar la estructura de la BD.

---

#### dp_role_web

**Usado por:** DigitalPlusWeb (Blazor Server)

```sql
CREATE ROLE dp_role_web;

-- SELECT amplio (necesario para EF Core y Dapper queries)
GRANT SELECT ON SCHEMA::dbo TO dp_role_web;

-- INSERT y UPDATE para funcionalidades web (gestion de legajos, configuracion)
GRANT INSERT ON SCHEMA::dbo TO dp_role_web;
GRANT UPDATE ON SCHEMA::dbo TO dp_role_web;

-- EXECUTE para SPs de reportes y dashboards
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_web;

-- DELETE solo donde sea necesario
GRANT DELETE ON dbo.LegajosHuellas TO dp_role_web;
GRANT DELETE ON dbo.Fichadas TO dp_role_web;

-- Restricciones explicitas:
-- NO db_owner
-- NO ALTER
-- NO CREATE TABLE
-- NO DROP
```

**Justificacion:** La web usa EF Core (que genera INSERT/UPDATE/DELETE) y Dapper (queries directas). Necesita permisos amplios de lectura/escritura pero sin control estructural.

---

### 1.2 Usuarios SQL

#### Fase 1 (implementacion actual): Un usuario por empresa

Inicialmente cada empresa tendra un unico usuario SQL que pertenecera a los tres roles. Esto simplifica la gestion sin perder la separacion logica.

| Servidor | Usuario | BD | Roles asignados |
|---|---|---|---|
| Ferozo | `dp_admin_svc` | DigitalPlusAdmin | `dp_role_admin` (CRUD licencias) |
| Ferozo | `dp_web_svc` | DigitalPlus | `dp_role_web` |
| Local (cada cliente) | `dp_<Empresa>_<GUID6>` | DP_NombreEmpresa | `dp_role_runtime` + `dp_role_admin` |

**Nota:** En instalaciones locales, Fichador y Administrador comparten el mismo usuario SQL con ambos roles (`dp_role_runtime` + `dp_role_admin`). Esto es aceptable porque ambos corren en la misma maquina local bajo control del cliente.

#### Fase futura (preparada pero no implementada): Un usuario por rol

Cuando sea necesario, se podra crear un usuario separado para cada aplicacion:

| Aplicacion | Usuario | Rol |
|---|---|---|
| Fichador | `dp_<Empresa>_rt` | `dp_role_runtime` |
| Administrador | `dp_<Empresa>_adm` | `dp_role_admin` |

Esto no requiere cambios en la estructura de roles, solo crear los logins adicionales y actualizar los connection strings.

---

## 2. POLITICA DE PASSWORD

### 2.1 Recomendacion: CHECK_POLICY = ON

```sql
CREATE LOGIN [dp_example_login]
WITH PASSWORD = 'P@ssw0rd_Seguro_2026!',
     DEFAULT_DATABASE = [DP_MiEmpresa],
     CHECK_POLICY = ON,
     CHECK_EXPIRATION = OFF;
```

**CHECK_POLICY = ON** aplica las politicas de complejidad de Windows (minimo 8 caracteres, mezcla de mayusculas/minusculas/numeros/especiales). Esto es deseable porque:

- Fuerza passwords robustos en entornos donde el admin podria elegir uno debil
- Es el default de SQL Server
- Compatible con la mayoria de entornos

**CHECK_EXPIRATION = OFF** porque no queremos que los passwords de servicio expiren (causaria caida de las aplicaciones).

### 2.2 Cuando usar CHECK_POLICY = OFF

Solo se justifica en estos escenarios:

| Escenario | Motivo |
|---|---|
| SQL Express sin Windows Policy Service | El servicio `secpol.msc` puede no estar disponible en ediciones Express en algunos Windows |
| Entornos legacy sin politica de dominio | CHECK_POLICY falla si Windows no tiene politica definida |
| Hosting compartido (Ferozo) | Si el hosting no permite CHECK_POLICY = ON por restricciones del proveedor |

**Si CHECK_POLICY = ON falla durante la instalacion**, el instalador debe:
1. Registrar el error en el log
2. Reintentar con CHECK_POLICY = OFF
3. Informar al usuario que se uso un password sin validacion de politica
4. Documentar la excepcion en el archivo de configuracion

### 2.3 Generacion de passwords

Los passwords se generan automaticamente durante la instalacion con estas caracteristicas:

- Longitud: 20 caracteres
- Composicion: mayusculas + minusculas + numeros + especiales (`!@#$%`)
- Generados con `System.Security.Cryptography.RandomNumberGenerator`
- Nunca se muestran al usuario
- Se almacenan unicamente en el connection string cifrado con DPAPI

---

## 3. MODELO DE BOOTSTRAP DE INSTALACION

### 3.1 Principio

Las credenciales privilegiadas (sysadmin o equivalente) se necesitan **unicamente durante la instalacion** para:

1. Crear la base de datos
2. Ejecutar scripts de estructura (CREATE TABLE, etc.)
3. Crear login SQL dedicado
4. Crear usuario en la base de datos
5. Crear roles y asignar permisos
6. Cargar datos iniciales

Una vez completada la instalacion, **las credenciales bootstrap se descartan y no se almacenan en ningun archivo.**

### 3.2 Modos de autenticacion bootstrap

#### Modo 1: Windows Authentication (recomendado)

```
Integrated Security=True
```

Requisitos:
- El usuario de Windows que ejecuta el instalador debe tener permisos `sysadmin` o `dbcreator` en la instancia SQL
- SQL Express instalado localmente: el usuario que instalo SQL Express tiene `sysadmin` automaticamente
- Este es el modo predeterminado del instalador actual (v1.2+)

Ventaja: No se solicitan credenciales al usuario.

#### Modo 2: SQL Authentication

```
User Id=sa;Password=<ingresada_por_usuario>
```

Requisitos:
- El usuario debe conocer las credenciales de un login con permisos `sysadmin`
- El modo mixto (SQL + Windows Auth) debe estar habilitado en la instancia

Caso de uso: Instalacion en un servidor SQL existente donde el usuario instalador no tiene permisos Windows sobre la instancia.

### 3.3 Flujo de bootstrap en el instalador

```
[Inicio instalacion]
        |
        v
[Seleccionar modo: Local / Nube]
        |
   [Local] -----> [Detectar instancia SQL Express]
        |                    |
        v                    v
[Usar Windows Auth]  [Si no detecta: ofrecer instalar SQL Express]
        |
        v
[Probar conexion bootstrap]
        |
   [Exito] -----> [Crear BD DP_<Empresa>]
        |                    |
        v                    v
[Si falla: pedir credenciales SQL Auth]
        |                    |
        v                    v
                    [Ejecutar scripts de estructura]
                             |
                             v
                    [Crear login/user/roles SQL dedicados]
                             |
                             v
                    [Generar password aleatorio]
                             |
                             v
                    [Armar connection string con usuario dedicado]
                             |
                             v
                    [Cifrar config con DPAPI (ConfigProtector.exe)]
                             |
                             v
                    [DESCARTAR credenciales bootstrap de memoria]
                             |
                             v
                    [Instalacion completa]
```

### 3.4 Lo que NO se almacena

| Dato | Se almacena? | Donde |
|---|---|---|
| Password del usuario dedicado (dp_xxx) | Si | Connection string cifrado con DPAPI |
| Credenciales bootstrap (sa o Windows) | NO | Se descartan al finalizar la instalacion |
| Nombre del login creado | Si | Connection string cifrado con DPAPI |
| API Key de provisioning | Si | Config cifrado con DPAPI |

---

## 4. COMPATIBILIDAD CON INSTALACIONES EXISTENTES

### 4.1 Politica de compatibilidad

Las siguientes reglas son **definitivas** para esta etapa:

**Regla 1:** Las instalaciones existentes NO se rompen.
- Instalaciones locales con `Integrated Security=True` siguen funcionando sin cambio.
- La web en Ferozo con `sa` sigue funcionando hasta que se migre manualmente.

**Regla 2:** NO se realiza migracion automatica.
- No existe script que convierta una instalacion vieja al nuevo modelo.
- La migracion sera manual, asistida por documentacion.

**Regla 3:** El nuevo modelo aplica unicamente a nuevas instalaciones.
- Instalador v1.4+ creara usuarios SQL dedicados.
- Instalador v1.3 y anteriores no se modifican.

### 4.2 Proceso de migracion manual (futura, opcional)

Para cuando un cliente existente quiera migrar al nuevo modelo:

1. Crear login SQL dedicado en la instancia del cliente
2. Crear usuario + asignar roles en la BD del cliente
3. Actualizar connection string en los .exe.config
4. Cifrar con DPAPI
5. Verificar que todo funciona
6. (Opcional) Revocar el acceso por Windows Auth si no se usa mas

Este proceso se documentara como guia tecnica pero NO se automatizara en esta etapa.

---

## 5. SEPARACION DE RESPONSABILIDADES

### 5.1 Matriz de responsabilidades

| Funcion | Azure Functions | Portal Admin | Instalador | Fichador | Administrador | Web |
|---|---|---|---|---|---|---|
| Activacion de licencia | X | | | | | |
| Heartbeat | X | | | | | |
| Emision de tickets | X | | | | | |
| Validacion de codigos de licencia | X | | | | | |
| Provisioning (creacion BD cloud) | X | | | | | |
| Administracion de empresas | | X | | | | |
| Generacion de codigos de licencia | | X | | | | |
| Visualizacion de activaciones | | X | | | | |
| Bloqueo/reactivacion de licencias | | X | | | | |
| Auditoria administrativa | | X | | | | |
| Creacion de base de datos | | | X | | | |
| Creacion de usuarios SQL | | | X | | | |
| Asignacion de roles | | | X | | | |
| Configuracion inicial | | | X | | | |
| Registro de fichadas | | | | X | | |
| Validacion de huellas | | | | X | | |
| Heartbeat periodico (timer 4h) | | | | X | | |
| Gestion de legajos | | | | | X | |
| Gestion de huellas | | | | | X | |
| Gestion de horarios | | | | | X | |
| Configuracion del sistema | | | | | X | |
| Validacion de licencia (startup) | | | | X | X | |
| Reportes y dashboards | | | | | | X |
| Consultas de fichadas | | | | | | X |
| Gestion web de legajos | | | | | | X |

### 5.2 Regla de no duplicacion

**La logica de licencias (activacion, validacion, heartbeat, emision de tickets) vive UNICAMENTE en Azure Functions.**

El Portal Admin es un backoffice que:
- Lee datos de licencias (SELECT sobre Licencias, LicenciasLog, LicenseCodes)
- Modifica datos administrativos (UPDATE/INSERT directo a la BD)
- NO llama a Azure Functions para activar/validar licencias
- NO duplica logica de negocio de licencias

Las aplicaciones cliente (Fichador, Administrador):
- Llaman a Azure Functions via HTTP para activacion y heartbeat
- Validan tickets localmente (firma RSA)
- NO acceden directamente a la BD de licencias (DigitalPlusAdmin)

---

## 6. PORTAL ADMIN (DigitalPlusAdminWeb)

### 6.1 Tecnologia

- **Blazor Server** (.NET 8)
- **EF Core** para acceso a DigitalPlusAdmin
- **ASP.NET Identity** para autenticacion
- **Bootstrap 5** para UI (consistente con DigitalPlusWeb)

### 6.2 Base de datos

Usa la BD **DigitalPlusAdmin** existente. Tablas adicionales solo para Identity:

```
DigitalPlusAdmin
├── Licencias              (existente)
├── LicenciasLog           (existente)
├── LicenseCodes           (existente)
├── ActivationCodes        (existente)
└── AspNet*                (nuevas - Identity: Users, Roles, Claims, etc.)
```

### 6.3 Pantallas

#### Dashboard (pagina principal)

Contadores:
- Licencias activas
- Licencias trial
- Licencias suspendidas
- Licencias vencidas

Alertas:
- Licencias que vencen en los proximos 30 dias
- Empresas en modo trial que expiran pronto
- Instalaciones sin heartbeat en las ultimas 48 horas (posible cliente desconectado)

Actividad reciente:
- Ultimos 10 heartbeats recibidos
- Ultimas 5 activaciones

---

#### Licencias

**Listado** con filtros por:
- Empresa (CompanyId)
- Tipo (active, trial, suspended, expired)
- Plan (basic, standard, premium)
- Fecha de vencimiento

**Detalle** de cada licencia:
- Datos completos (empresa, maquina, plan, legajos, expiracion)
- Ultimo heartbeat recibido
- Historial de eventos (LicenciasLog)

**Acciones:**
- Modificar plan / maxLegajos / expiracion
- Suspender licencia (con periodo de gracia de 7 dias)
- Reactivar licencia suspendida
- Revocar activacion (fuerza re-activacion)

---

#### Codigos de licencia

**Listado** con estados:
- Disponibles (no usados, no expirados)
- Usados (con detalle de empresa/maquina/fecha)
- Expirados (sin usar y fecha vencida)

**Generar nuevo codigo:**
- Formulario: plan, maxLegajos, duracionDias, diasParaUsar
- Al generar, muestra el codigo XXXX-XXXX-XXXX-XXXX en pantalla (unica vez, no se puede recuperar)

---

#### Empresas

**Listado** agrupado por CompanyId unico de la tabla Licencias.

**Detalle** por empresa:
- Todas las licencias de la empresa
- Todas las activaciones
- Todos los heartbeats
- Historial completo

**Busqueda** por nombre de empresa o ID de licencia.

---

#### Auditoria

**Tabla LicenciasLog** con filtros:
- Rango de fechas
- Tipo de accion (activate, heartbeat, admin_modify, admin_suspend, admin_reactivate)
- Empresa

Ordenado por fecha descendente. Paginado.

---

### 6.4 Hosting

**Opcion recomendada:** Azure App Service (free tier F1)
- Ventaja: ya tenemos infraestructura Azure con `rg-digitalplus-provision`
- Mismo resource group y suscripcion
- Acceso restringido por Identity (usuario/password)

**Alternativa:** Ferozo como sitio web separado (si el plan lo permite).

---

## 7. VALIDACION DEL HOSTING (FEROZO)

### 7.1 Problema

Los hosting compartidos de SQL Server suelen tener restricciones. Es necesario verificar si Ferozo permite:

- `CREATE LOGIN` (nivel servidor)
- `CREATE USER` (nivel base de datos)
- `CREATE ROLE` (nivel base de datos)
- `GRANT` / `REVOKE` / `ALTER ROLE` (nivel base de datos)

### 7.2 Procedimiento de verificacion

Ejecutar los siguientes comandos de prueba en Ferozo (con las credenciales `sa` actuales):

#### Paso 1: Verificar permisos de servidor

```sql
-- Verificar si el login actual tiene permiso para crear logins
SELECT HAS_PERMS_BY_NAME(NULL, NULL, 'CREATE LOGIN') AS CanCreateLogin;

-- Si retorna 0, no se pueden crear logins
```

#### Paso 2: Intentar crear un login de prueba

```sql
CREATE LOGIN dp_test_probe
WITH PASSWORD = 'Test_Pr0be_2026!',
     CHECK_POLICY = ON;

-- Si falla: anotar el mensaje de error exacto
-- Si funciona: continuar con paso 3
```

#### Paso 3: Crear usuario y rol de prueba

```sql
USE [DigitalPlusAdmin];

CREATE USER dp_test_probe FOR LOGIN dp_test_probe;

CREATE ROLE dp_test_role;
GRANT SELECT ON SCHEMA::dbo TO dp_test_role;
ALTER ROLE dp_test_role ADD MEMBER dp_test_probe;
```

#### Paso 4: Verificar CHECK_POLICY

```sql
-- Si el paso 2 funciono con CHECK_POLICY = ON, perfecto
-- Si fallo, intentar:
CREATE LOGIN dp_test_probe2
WITH PASSWORD = 'Test_Pr0be_2026!',
     CHECK_POLICY = OFF;

-- Documentar cual funciono
```

#### Paso 5: Limpiar

```sql
USE [DigitalPlusAdmin];
DROP USER IF EXISTS dp_test_probe;
DROP ROLE IF EXISTS dp_test_role;

USE [master];
DROP LOGIN dp_test_probe;
DROP LOGIN dp_test_probe2;  -- si se creo
```

### 7.3 Alternativas si Ferozo no permite CREATE LOGIN

| Alternativa | Descripcion | Viabilidad |
|---|---|---|
| **SQL Users sin login** (`CREATE USER ... WITHOUT LOGIN`) | Usuarios contenidos en la BD, no requieren login de servidor | Alta — soportado en SQL Server 2012+, requiere `CONTAINMENT = PARTIAL` en la BD |
| **Usuarios adicionales via panel Ferozo** | Algunos hosting permiten crear usuarios SQL desde su panel web | Media — depende del proveedor |
| **Instancia SQL dedicada** | Contratar servidor SQL dedicado con control total | Baja (costo) — ultima opcion |
| **Mantener sa con password fuerte** | Cambiar password de sa a uno seguro y documentarlo | Aceptable como plan B temporal |

### 7.4 Resultado esperado

Antes de iniciar la implementacion, se debe tener documentado:

| Verificacion | Resultado |
|---|---|
| Ferozo permite CREATE LOGIN | **SI** |
| Ferozo permite CHECK_POLICY = ON | **SI** |
| Ferozo permite CREATE USER | **SI** |
| Ferozo permite CREATE ROLE | **SI** |
| Ferozo permite GRANT ON SCHEMA | **SI** |
| Ferozo permite ALTER ROLE ADD MEMBER | **SI** |
| Usuario de prueba puede conectarse y hacer SELECT | **SI** (SELECT COUNT(*) FROM Licencias = 2) |
| Login sa es sysadmin | **SI** (IsSysAdmin=1, IsSecurityAdmin=1, IsDbCreator=1) |
| Alternativa necesaria | **NO** -- todas las operaciones funcionan |
| Logins de prueba eliminados | **SI** (dp_test_probe + dp_test_role eliminados) |

**Conclusion:** Ferozo soporta completamente el modelo de seguridad propuesto. No se requieren alternativas. Se puede proceder con la implementacion usando CHECK_POLICY = ON.

---

## 8. SCRIPTS DE IMPLEMENTACION

### 8.1 Script: Crear roles en BD cliente (DP_*)

```sql
-- 011_CreateRoles_Client.sql
-- Ejecutar en: cada BD cliente (DP_NombreEmpresa)
-- Requisito: credenciales sysadmin o db_owner

-- Rol runtime (Fichador)
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_runtime' AND type = 'R')
    CREATE ROLE dp_role_runtime;

GRANT SELECT ON dbo.Legajos TO dp_role_runtime;
GRANT SELECT ON dbo.LegajosHuellas TO dp_role_runtime;
GRANT SELECT ON dbo.Horarios TO dp_role_runtime;
GRANT SELECT ON dbo.Sucursales TO dp_role_runtime;
GRANT SELECT ON dbo.Terminales TO dp_role_runtime;
GRANT SELECT ON dbo.VariablesGlobales TO dp_role_runtime;
GRANT SELECT ON dbo.Fichadas TO dp_role_runtime;
GRANT SELECT ON dbo.LegajosSucursales TO dp_role_runtime;
GRANT INSERT ON dbo.Fichadas TO dp_role_runtime;
GRANT EXECUTE ON dbo.EscritorioFichadasSPSALIDA TO dp_role_runtime;
GO

-- Rol admin (Administrador)
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_admin' AND type = 'R')
    CREATE ROLE dp_role_admin;

GRANT SELECT ON SCHEMA::dbo TO dp_role_admin;
GRANT INSERT ON SCHEMA::dbo TO dp_role_admin;
GRANT UPDATE ON SCHEMA::dbo TO dp_role_admin;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_admin;
GRANT DELETE ON dbo.LegajosHuellas TO dp_role_admin;
GRANT DELETE ON dbo.Fichadas TO dp_role_admin;
GRANT DELETE ON dbo.IncidenciasLegajos TO dp_role_admin;
GRANT DELETE ON dbo.Horarios TO dp_role_admin;
GO

-- Rol web (DigitalPlusWeb)
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_web' AND type = 'R')
    CREATE ROLE dp_role_web;

GRANT SELECT ON SCHEMA::dbo TO dp_role_web;
GRANT INSERT ON SCHEMA::dbo TO dp_role_web;
GRANT UPDATE ON SCHEMA::dbo TO dp_role_web;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_web;
GRANT DELETE ON dbo.LegajosHuellas TO dp_role_web;
GRANT DELETE ON dbo.Fichadas TO dp_role_web;
GO
```

### 8.2 Script: Crear roles en DigitalPlusAdmin (Ferozo)

```sql
-- 012_CreateRoles_Admin.sql
-- Ejecutar en: Ferozo (DigitalPlusAdmin)

-- Rol admin para Azure Functions y scripts de gestion
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_admin' AND type = 'R')
    CREATE ROLE dp_role_admin;

GRANT SELECT ON SCHEMA::dbo TO dp_role_admin;
GRANT INSERT ON SCHEMA::dbo TO dp_role_admin;
GRANT UPDATE ON SCHEMA::dbo TO dp_role_admin;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_admin;
GRANT DELETE ON dbo.LicenciasLog TO dp_role_admin;  -- limpieza de logs viejos
GO
```

### 8.3 Script: Crear login y usuario en Ferozo

```sql
-- 013_CreateUsers_Ferozo.sql
-- Ejecutar en: Ferozo (master) con credenciales sysadmin
-- PREREQUISITO: haber ejecutado la validacion del punto 7

USE [master];

-- Login para Azure Functions / Scripts admin
CREATE LOGIN [dp_admin_svc]
WITH PASSWORD = '<GENERAR_PASSWORD_SEGURO>',
     DEFAULT_DATABASE = [DigitalPlusAdmin],
     CHECK_POLICY = ON;   -- cambiar a OFF si la validacion determino que no es soportado
GO

USE [DigitalPlusAdmin];
CREATE USER [dp_admin_svc] FOR LOGIN [dp_admin_svc];
ALTER ROLE dp_role_admin ADD MEMBER [dp_admin_svc];
GO

-- Login para la web
USE [master];
CREATE LOGIN [dp_web_svc]
WITH PASSWORD = '<GENERAR_PASSWORD_SEGURO>',
     DEFAULT_DATABASE = [DigitalPlus],
     CHECK_POLICY = ON;   -- cambiar a OFF si la validacion determino que no es soportado
GO

USE [DigitalPlus];
-- Primero crear roles si no existen
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_web' AND type = 'R')
    CREATE ROLE dp_role_web;

GRANT SELECT ON SCHEMA::dbo TO dp_role_web;
GRANT INSERT ON SCHEMA::dbo TO dp_role_web;
GRANT UPDATE ON SCHEMA::dbo TO dp_role_web;
GRANT EXECUTE ON SCHEMA::dbo TO dp_role_web;
GRANT DELETE ON dbo.LegajosHuellas TO dp_role_web;
GRANT DELETE ON dbo.Fichadas TO dp_role_web;

CREATE USER [dp_web_svc] FOR LOGIN [dp_web_svc];
ALTER ROLE dp_role_web ADD MEMBER [dp_web_svc];
GO
```

### 8.4 Logica del instalador para creacion de usuarios locales

```
Pseudocodigo para setup.iss (Inno Setup / Pascal):

1. Generar loginName = "dp_" + SanitizedCompany + "_" + Random6Chars()
2. Generar password = RandomPassword(20)  -- mayus + minus + numeros + especiales
3. Ejecutar con credenciales bootstrap (Windows Auth):
   a. CREATE LOGIN [loginName] WITH PASSWORD = 'password', CHECK_POLICY = ON
      - Si falla: reintentar con CHECK_POLICY = OFF, registrar en log
   b. USE [dbName]; CREATE USER [loginName] FOR LOGIN [loginName]
   c. Crear roles si no existen (dp_role_runtime, dp_role_admin)
   d. ALTER ROLE dp_role_runtime ADD MEMBER [loginName]
   e. ALTER ROLE dp_role_admin ADD MEMBER [loginName]
4. Armar connection string: "Server=instancia;Database=dbName;User Id=loginName;Password=password;..."
5. Escribir en fichador.exe.config y admin.exe.config
6. Cifrar con ConfigProtector.exe (DPAPI)
7. Descartar loginName y password de memoria
```

---

## 9. ESTADO ACTUAL DE CREDENCIALES Y CAMBIOS NECESARIOS

### 9.1 Inventario de archivos con sa/Soporte1

| Archivo | Ubicacion | Usa sa/Soporte1 | Accion |
|---|---|---|---|
| `Common\Acceso.Clases.Datos\App.config` | Desktop | Si (legacy TocayAnda) | Limpiar strings legacy |
| `Common\Global.Datos\SQLServer.cs` | Desktop | No (lee de config) | Sin cambio |
| `AzureProvisioning\local.settings.json` | Azure (dev) | Si (Ferozo) | Reemplazar por dp_admin_svc |
| `AzureProvisioning\tools\*.ps1` | Scripts admin | Si (parametros por defecto) | Cambiar defaults a dp_admin_svc |
| `AzureProvisioning\docs\deployment-guide.md` | Documentacion | Si (ejemplos) | Actualizar ejemplos |
| `DigitalPlusWeb\appsettings.json` | Web (prod) | Si (2 de 4 strings) | Reemplazar por dp_web_svc |
| `DigitalPlusWeb\appsettings.Development.json` | Web (dev) | Si (4 strings) | Mover a User Secrets |

### 9.2 Lo que NO se rompe al cambiar

- **SQLServer.cs / SQLAccess.cs**: Leen de `ConnectionStrings["Local"]`, funcionan con cualquier usuario
- **Instalador local**: Ya usa `Integrated Security=True` para bootstrap
- **Azure Functions codigo**: Lee de App Settings, cambiar el setting es suficiente
- **Libreria de licencias**: Solo usa HTTP, no SQL

---

## 10. ORDEN DE IMPLEMENTACION

| Paso | Descripcion | Prerequisito | Riesgo |
|---|---|---|---|
| **1** | Definir modelo definitivo de seguridad SQL | Ninguno | Ninguno (es este documento) |
| **2** | Verificar capacidades de Ferozo (seccion 7) | Paso 1 | Si no permite CREATE LOGIN, usar alternativa |
| **3** | Crear scripts de roles y logins (seccion 8) | Paso 2 | Ninguno |
| **4** | Probar creacion manual de usuarios en Ferozo | Paso 3 | Verificar que las apps funcionan con los nuevos usuarios |
| **5** | Actualizar configs y App Settings | Paso 4 | Probar TODAS las funciones antes de quitar sa |
| **6** | Actualizar instalador a v1.4 | Paso 5 | Solo afecta nuevas instalaciones |
| **7** | Desarrollar portal admin (DigitalPlusAdminWeb) | Paso 3 (solo necesita BD lista) | Proyecto independiente |
| **8** | Mejorar integracion con Azure Functions | Paso 7 | Opcional — el portal ya funciona con acceso directo a BD |

**Nota importante:** Los pasos 1-6 son secuenciales. El paso 7 (portal) puede hacerse en paralelo a partir del paso 3.

---

## 11. RIESGOS Y MITIGACIONES

| Riesgo | Probabilidad | Impacto | Mitigacion |
|---|---|---|---|
| Ferozo no permite CREATE LOGIN | Media | Alto | Validar antes de empezar (seccion 7). Alternativas documentadas |
| Nuevo usuario SQL sin permisos suficientes | Baja | Alto | Probar TODAS las funciones con el nuevo usuario antes de quitar sa |
| CHECK_POLICY = ON no funciona en SQL Express | Baja | Bajo | Fallback a OFF con documentacion |
| Romper instalaciones existentes | Ninguna | Critico | Politica explicita: solo nuevas instalaciones |
| Portal admin expuesto publicamente | Media | Medio | Identity + password fuerte. IP whitelist si es posible |
| Password de usuario SQL se pierde | Baja | Medio | Queda en Azure App Settings + DPAPI del config. Se puede regenerar |
| Migracion de clientes existentes falla | Baja | Medio | Proceso manual documentado, no automatizado |

---

## 12. CONCLUSIONES

Este documento define el modelo arquitectonico **definitivo** para la seguridad SQL y el portal de administracion de DigitalPlus.

### Decisiones clave

1. **Tres roles diferenciados** con permisos minimos necesarios para cada tipo de aplicacion
2. **Un usuario por empresa** inicialmente, con arquitectura preparada para separacion futura
3. **CHECK_POLICY = ON** como default, con fallback documentado
4. **Credenciales bootstrap descartadas** despues de la instalacion
5. **Compatibilidad total** con instalaciones existentes
6. **Separacion clara de responsabilidades** entre Azure Functions (logica), Portal (backoffice) e Instalador (setup)
7. **Validacion obligatoria de Ferozo** antes de implementar

### Proxima accion

**Paso 2: Ejecutar el procedimiento de validacion de Ferozo (seccion 7).**

Todo lo demas depende del resultado de esta verificacion.
