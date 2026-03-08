# TASK: Revisión Arquitectónica – Seguridad SQL y Portal de Administración

## Contexto

Existe un documento previamente generado:

DocumentacionClaude/DigitalPlus_Seguridad_SQL_y_AdminPortal.md

Este documento describe el nuevo modelo de seguridad SQL y la futura implementación del portal de administración de licencias.

Antes de comenzar cualquier implementación real, es necesario **revisar y ajustar el diseño arquitectónico** para asegurar:

- compatibilidad con instalaciones existentes
- correcta separación de responsabilidades
- modelo de seguridad adecuado
- viabilidad técnica en entornos reales (hosting, SQL Server, etc.)

Esta tarea consiste en **realizar una segunda versión del documento**, mejorando la definición técnica del modelo.

---

# 1. MODELO DEFINITIVO DE SEGURIDAD SQL

El documento actual propone un rol único `dp_role_app` con permisos amplios.

Esto debe revisarse.

## Objetivo

Definir un modelo de permisos que permita:

- principio de menor privilegio
- escalabilidad futura
- separación de responsabilidades

### Propuesta inicial a evaluar

Definir roles como:

dp_role_runtime  
dp_role_admin  
dp_role_web  

Cada rol debe tener únicamente los permisos necesarios.

Evitar permisos globales como:

GRANT SELECT ON SCHEMA::dbo  
GRANT DELETE ON SCHEMA::dbo

salvo que sea absolutamente necesario.

---

# 2. POLÍTICA DE PASSWORD Y SEGURIDAD

En los scripts actuales aparece:

CHECK_POLICY = OFF

Esto debe revisarse.

## Objetivo

Determinar si es posible utilizar:

CHECK_POLICY = ON

Si existen razones técnicas para mantenerlo desactivado, documentar claramente el motivo.

---

# 3. MODELO DE BOOTSTRAP DE BASE DE DATOS

El sistema necesita credenciales privilegiadas únicamente durante la instalación inicial.

Se debe definir claramente el modelo de bootstrap.

### El instalador debe soportar

1. Windows Authentication (si el usuario tiene permisos)
2. SQL Authentication (usuario sysadmin temporal)

El documento debe describir claramente:

- cómo se obtienen estas credenciales
- cómo se utilizan durante la instalación
- confirmación de que **no se almacenan permanentemente**

---

# 4. COMPATIBILIDAD CON INSTALACIONES EXISTENTES

El sistema ya tiene instalaciones funcionando con modelos de conexión actuales.

Debe definirse explícitamente la estrategia de compatibilidad.

## Reglas

1. Las instalaciones existentes no deben romperse
2. No se realizará migración automática
3. El nuevo modelo se aplicará únicamente a nuevas instalaciones

Documentar claramente esta política.

---

# 5. SEPARACIÓN DE RESPONSABILIDADES ENTRE COMPONENTES

El sistema incluye varios componentes:

DigitalPlusWeb  
DigitalPlusAdminWeb  
Azure Functions  
Instalador  
Aplicaciones de escritorio

El documento debe definir claramente **qué responsabilidad tiene cada uno**.

### Azure Functions

Responsables de:

- activación de licencia
- heartbeat
- provisioning
- emisión de tickets

### Portal Admin (DigitalPlusAdminWeb)

Responsable de:

- administración de empresas
- administración de licencias
- visualización de activaciones
- auditoría

### Instalador

Responsable de:

- creación de base de datos
- creación de usuarios SQL dedicados
- configuración inicial del sistema

Evitar duplicar lógica de negocio entre portal y Azure Functions.

---

# 6. MEJORAS FUNCIONALES DEL PORTAL ADMIN

El portal propuesto debe incluir funcionalidades operativas reales.

Además de CRUD básicos debe considerar:

### Dashboard

Licencias próximas a vencer  
Empresas en modo trial  
Instalaciones sin heartbeat reciente  

### Gestión

Activaciones por máquina  
Historial de activaciones  
Búsqueda por empresa o licencia  

### Control manual

Bloqueo de licencia  
Reactivación manual  
Revocación de activaciones  

### Auditoría

Registro completo de operaciones administrativas.

---

# 7. VALIDACIÓN DE HOSTING (FEROZO)

Antes de implementar el nuevo modelo se debe verificar si el entorno actual permite:

CREATE LOGIN  
CREATE USER  

El documento debe incluir un procedimiento de verificación para determinar si el hosting soporta:

- creación de logins SQL
- asignación de roles
- administración de usuarios

---

# 8. ORDEN DE IMPLEMENTACIÓN RECOMENDADO

El documento debe proponer un orden técnico correcto para implementar los cambios.

Orden sugerido:

1. Definir modelo definitivo de seguridad SQL
2. Verificar capacidades del hosting
3. Crear scripts de roles y usuarios
4. Probar creación manual de usuarios
5. Actualizar Azure Functions y Web
6. Actualizar instalador
7. Desarrollar portal admin

---

# 9. NUEVO DOCUMENTO A GENERAR

Luego de realizar esta revisión se debe generar una nueva versión del documento:

DocumentacionClaude/DigitalPlus_Seguridad_SQL_y_AdminPortal_v2.md

Este documento será considerado la **versión definitiva del diseño arquitectónico** antes de comenzar la implementación.