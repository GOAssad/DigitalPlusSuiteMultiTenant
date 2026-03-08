# Instrucciones del Project Leader – Generación Documento v2

He revisado tu propuesta para generar la versión 2 del documento de seguridad SQL y del portal de administración.

Podés proceder con la generación del documento **DigitalPlus_Seguridad_SQL_y_AdminPortal_v2.md**, pero incorporando las siguientes decisiones arquitectónicas que deben considerarse definitivas para esta etapa.

---

# 1. MODELO DE ROLES SQL

El sistema no utilizará un único rol `dp_role_app`.

Se deberán definir los siguientes roles:

dp_role_runtime  
dp_role_admin  
dp_role_web  

### Propósito de cada rol

**dp_role_runtime**

Usado por:

- Fichador
- procesos automáticos

Permisos:

- EXECUTE sobre stored procedures de fichadas
- SELECT sobre tablas necesarias para validación de huellas y legajos

Restricciones:

- No DELETE
- No ALTER


---

**dp_role_admin**

Usado por:

- aplicación Administrador

Permisos:

- SELECT
- INSERT
- UPDATE
- EXECUTE

DELETE solo cuando sea estrictamente necesario.


---

**dp_role_web**

Usado por:

- DigitalPlusWeb

Permisos:

- SELECT amplio
- EXECUTE de stored procedures
- INSERT y UPDATE según necesidades de EF Core y Dapper


---

# 2. USUARIOS SQL POR EMPRESA

Inicialmente el sistema utilizará:

**un único usuario SQL por empresa**

Este usuario pertenecerá a los roles necesarios.

La separación por usuario (runtime/admin/web) queda preparada arquitectónicamente pero **no será implementada en esta primera etapa**.

---

# 3. POLÍTICA DE PASSWORD

Los scripts deben evaluar utilizar:

CHECK_POLICY = ON

Solo se utilizará:

CHECK_POLICY = OFF

si existen limitaciones técnicas del entorno que lo requieran.

Este punto debe documentarse.

---

# 4. MODELO DE BOOTSTRAP DE INSTALACIÓN

El instalador necesitará credenciales privilegiadas únicamente durante la instalación.

Debe soportar dos modos:

### Windows Authentication

Si el usuario instalador tiene permisos suficientes.

Ejemplo de conexión:
Integrated Security=True

---

### SQL Authentication

Credenciales bootstrap como:
sa


o cualquier usuario con permisos sysadmin.

---

### Uso del bootstrap

Las credenciales privilegiadas se utilizarán únicamente para:

- crear base de datos
- ejecutar scripts de estructura
- crear login SQL dedicado
- crear usuario en la base de datos
- asignar roles

Una vez finalizada la instalación:

**las credenciales bootstrap se descartan y no se almacenan en ningún archivo.**

---

# 5. SEPARACIÓN DE RESPONSABILIDADES

Debe quedar explícitamente documentado qué componente es responsable de cada función.

### Azure Functions

Responsables de:

- activación de licencias
- heartbeat
- emisión de tickets
- validación de licencias

Aquí vive la lógica operativa de licencias.

---

### Portal Admin (DigitalPlusAdminWeb)

Responsable de:

- administración de empresas
- generación de licencias
- visualización de activaciones
- bloqueo manual de licencias
- auditoría

El portal es únicamente un **backoffice administrativo**.

No debe duplicar lógica de licencias existente en Azure Functions.

---

### Instalador

Responsable de:

- creación de base de datos
- creación de usuarios SQL dedicados
- asignación de roles
- configuración inicial del sistema

---

### Aplicaciones cliente

Administrador  
Fichador  
DigitalPlusWeb

Se conectarán únicamente utilizando el **usuario SQL dedicado generado durante la instalación**.

---

# 6. PORTAL ADMIN – MEJORAS FUNCIONALES

El portal administrativo debe contemplar funcionalidades operativas reales.

Además de CRUD básicos debe incluir:

### Dashboard

Licencias próximas a vencer  
Empresas en modo trial  
Instalaciones sin heartbeat reciente

---

### Gestión

Listado de activaciones por máquina  
Historial de activaciones  
Búsqueda por empresa o licencia

---

### Control manual

Bloqueo de licencia  
Reactivación manual  
Revocación de activaciones

---

### Auditoría

Registro completo de operaciones administrativas.

---

# 7. VALIDACIÓN DEL HOSTING (FEROZO)

El documento debe incluir un procedimiento técnico para validar si el hosting permite:

CREATE LOGIN  
CREATE USER  

Ejemplo de prueba:

```sql
CREATE LOGIN dp_test_login
WITH PASSWORD = 'Test123456!';

Si el comando falla, se deberán analizar alternativas:

SQL users sin login

instancia SQL dedicada

---
# 8. ORDEN DE IMPLEMENTACION


El orden de implementación debe ser el siguiente:

1 Definir modelo definitivo de seguridad SQL
2 Verificar capacidades del hosting (Ferozo)
3 Crear scripts de roles y logins
4 Probar creación manual de usuarios
5 Actualizar instalador
6 Desarrollar portal admin
7 Mejorar integración con Azure Function

---
# 9. DOCUMENTO A GENERAR

Debés generar:

DocumentacionClaude/DigitalPlus_Seguridad_SQL_y_AdminPortal_v2.md

Este documento será considerado la base arquitectónica final antes de comenzar la implementación.

---

Pequeña observación estratégica mientras tanto.

Si todo esto se implementa como lo estamos diseñando, DigitalPlus va a tener algo que la mayoría de los softwares de control horario **no tienen**:

- arquitectura instalable
- licenciamiento centralizado
- administración multiempresa
- seguridad razonable

En otras palabras, **un producto serio** en vez de un programa que “funciona mientras nadie lo mire demasiado”. Y eso, sorprendentemente, ya es media victoria en el mundo del software.