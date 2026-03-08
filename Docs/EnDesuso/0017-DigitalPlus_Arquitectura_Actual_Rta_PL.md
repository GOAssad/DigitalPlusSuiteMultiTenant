# TASK: Refactor de Seguridad SQL + Portal de Administración de Licencias

## Contexto

Existe un documento generado previamente:

DocumentacionClaude/DigitalPlus_Arquitectura_Actual.md

Este documento describe la arquitectura actual del sistema DigitalPlus.

A partir de ese análisis se definieron **mejoras arquitectónicas obligatorias** que deben implementarse sin romper la compatibilidad con las aplicaciones existentes.

Los objetivos principales son:

1. Eliminar el uso del usuario **sa** para las conexiones del sistema
2. Crear **usuarios SQL dedicados por empresa** durante la instalación
3. Preparar una **administración visual de licencias**
4. Reemplazar gradualmente la administración basada en scripts


---

# 1. NUEVO MODELO DE CONEXIÓN A BASE DE DATOS

Actualmente el sistema puede utilizar credenciales privilegiadas.

Esto debe cambiar.

## Objetivo

Cada instalación del sistema debe tener:

- un **usuario SQL único**
- creado automáticamente durante la instalación
- con permisos limitados

El usuario `sa` solo podrá utilizarse durante la **instalación inicial**.

Después de la instalación:

Administrador  
Fichador  
DigitalPlusWeb  

deben conectarse **únicamente con el usuario dedicado**.


---

# 2. CREACIÓN AUTOMÁTICA DE USUARIO SQL

Durante la instalación el instalador deberá:

1. Solicitar credenciales de bootstrap (sa o usuario sysadmin)
2. Conectarse al servidor SQL
3. Crear la base de datos si no existe
4. Ejecutar los scripts de estructura de DigitalPlus
5. Crear un login SQL dedicado

Formato sugerido:

dp_<Empresa>_<GUIDCorto>

Ejemplo:

dp_Acme_4F7A9C

Luego deberá:

- crear el usuario en la base de datos
- asignar permisos necesarios


---

# 3. MODELO DE PERMISOS SQL

El sistema debe utilizar **roles SQL controlados**.

Crear roles como:

dp_role_app

Este rol deberá tener permisos mínimos necesarios:

SELECT  
INSERT  
UPDATE  
EXECUTE (stored procedures)

Evitar permisos globales como:

db_owner


---

# 4. MODIFICACIÓN DEL INSTALADOR

El instalador actual deberá actualizarse para:

### Solicitar

Servidor SQL  
Usuario bootstrap  
Password bootstrap  
Nombre de base de datos  
Nombre de empresa

### Proceso

1 Crear base de datos si no existe  
2 Ejecutar scripts de estructura  
3 Crear login dedicado  
4 Crear user en la DB  
5 Asignar roles  
6 Generar connectionstring con el usuario dedicado  

### Importante

Las credenciales bootstrap **no deben almacenarse** en ningún archivo.

Solo deben utilizarse durante el proceso de instalación.


---

# 5. ACTUALIZACIÓN DE ARCHIVOS DE CONFIGURACIÓN

Los archivos de configuración de:

Administrador  
Fichador  
DigitalPlusWeb  

deben contener únicamente:

connection string con el usuario dedicado

Estas configuraciones deben continuar cifrándose con **DPAPI**.


---

# 6. PORTAL DE ADMINISTRACIÓN DE LICENCIAS

Actualmente existen scripts PowerShell para administrar licencias.

Esto debe evolucionar a una **administración visual web**.

## Nueva aplicación

Crear un nuevo proyecto:

DigitalPlusAdminWeb

Tecnología:

Blazor Server

Debe reutilizar:

- Identity
- EF Core
- estructura similar a DigitalPlusWeb


---

# 7. FUNCIONALIDADES DEL PORTAL ADMIN

El portal deberá permitir:

### Gestión de empresas

Crear empresa  
Editar empresa  
Ver estado

### Gestión de licencias

Crear licencia  
Trial  
Activa  
Vencida

### Activaciones

Ver máquinas activadas  
Fecha de activación  
Último heartbeat

### Tickets de licencia

Emitir ticket  
Revocar ticket

### Auditoría

Registro de operaciones


---

# 8. RELACIÓN CON AZURE FUNCTIONS

Las Azure Functions actuales seguirán utilizándose para:

activación de licencia  
heartbeat  
provisioning

El portal web deberá interactuar con la base:

DigitalPlusAdmin


---

# 9. DOCUMENTACIÓN

Una vez realizado el análisis e implementación preliminar se deberá generar:

DocumentacionClaude/DigitalPlus_Seguridad_SQL_y_AdminPortal.md

El documento deberá explicar:

nuevo modelo de seguridad  
creación de usuarios SQL  
roles y permisos  
arquitectura del portal admin  


---

# 10. IMPORTANTE

Antes de realizar cambios en el código:

1. Analizar impacto en Administrador
2. Analizar impacto en Fichador
3. Analizar impacto en DigitalPlusWeb
4. Verificar compatibilidad con el instalador actual

No romper compatibilidad con instalaciones existentes.


---

# 11. ENTREGABLE ESPERADO

Claude deberá entregar:

1 Documento de análisis de implementación  
2 Lista de cambios necesarios  
3 Propuesta de estructura del portal admin  
4 Scripts SQL para creación de usuarios y roles