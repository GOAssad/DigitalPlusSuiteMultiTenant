# OBJETIVO

Rediseñar el instalador unificado de DigitalPlus para que pueda ser usado por usuarios no técnicos.

El instalador YA NO debe pedir:
- Servidor SQL
- Usuario SQL
- Password SQL

Los datos de conexión deben ser gestionados internamente por el sistema y no deben ser visibles para el usuario final.

El instalador debe pedir SOLO:

1) Nombre de Empresa (obligatorio)
2) Carpeta de instalación
3) Tipo de instalación:
   - Instalación Local (Base de datos en la PC)
   - Instalación en Nube
4) Otros atributos que ayuden a mejorar la experiencia de instalación (sugerir).

---

# NUEVO MODELO DE BASE DE DATOS

El nombre de la base de datos se genera automáticamente a partir del nombre de empresa.

Reglas de sanitización:

Input:
NombreEmpresa

Proceso:

1) Eliminar espacios
2) Eliminar caracteres especiales
3) Permitir solo letras y números
4) Reemplazar caracteres acentuados
5) Máximo 50 caracteres

Ejemplo:

"Mi Empresa S.A." → MiEmpresaSA

Nombre final de la base de datos:

DP_<EmpresaSanitizada>

Ejemplo:

DP_MiEmpresaSA

---

# COMPORTAMIENTO SEGÚN TIPO DE INSTALACIÓN

---

# CASO 1 — INSTALACIÓN EN NUBE

El instalador utilizará una conexión preconfigurada hacia el servidor cloud.

El usuario no debe conocer:

- servidor
- usuario
- password

Estos datos deben venir desde un mecanismo interno del instalador.

## Verificación de base de datos

Antes de crear la base:

1) El instalador verifica si la base ya existe en el servidor cloud.

### Si NO existe

- Crear la base de datos
- Ejecutar scripts SQL de DigitalPlus

### Si YA existe

NO continuar con la instalación.

Mostrar mensaje:

"La base de datos ya existe en el servidor.
Esto podría indicar que pertenece a otra empresa o a una instalación previa.

Debe cambiar el nombre de empresa e intentar nuevamente la instalación."

Opciones disponibles:

[ Volver atrás ]
[ Cancelar instalación ]

El instalador debe registrar en log:

- nombre empresa ingresado
- nombre base generado
- fecha y hora
- resultado de la verificación

La base existente **NO debe eliminarse ni modificarse bajo ninguna circunstancia.**

---

# CASO 2 — INSTALACIÓN LOCAL

La base de datos se instalará en la PC local.

El instalador debe detectar si existe alguna instancia de SQL Server instalada en la máquina.

No importa si es:

- SQL Server Express
- SQL Server Standard
- SQL Server Developer
- otra edición

## Paso 1 — Detectar instancia SQL

Si existe al menos una instancia SQL Server:

→ utilizar esa instancia.

Si NO existe ninguna instancia:

→ instalar automáticamente SQL Server Express.

Una vez que exista una instancia SQL disponible:

continuar con la instalación.

---

# CREACIÓN DE BASE DE DATOS LOCAL

Con una instancia SQL disponible:

1) verificar si existe la base de datos generada.

### Si NO existe

- Crear la base
- Ejecutar scripts SQL del modelo DigitalPlus.

### Si YA existe

NO continuar con la instalación.

Mostrar mensaje:

"La base de datos ya existe en la instancia SQL local.

Debe cambiar el nombre de empresa para generar una base diferente."

Opciones disponibles:

[ Volver atrás ]
[ Cancelar instalación ]

La base existente **NO debe eliminarse ni modificarse.**

---

# CONFIGURACIÓN DE LAS APLICACIONES

Los templates de configuración deben contener:

ConnectionStrings

name = Local

Server = <instancia_detectada>
Database = DP_<EmpresaSanitizada>
Integrated Security = True
TrustServerCertificate = True

Luego se debe ejecutar el proceso de protección de configuración utilizando **DPAPI ProtectedConfiguration** ya implementado.

---

# CAMBIOS EN EL INSTALADOR (INNO SETUP)

Modificar setup.iss.

Nueva pantalla con los campos:

- Nombre de Empresa
- Carpeta de instalación
- Tipo de instalación (Local / Nube)

Eliminar completamente los campos:

- Servidor SQL
- Usuario SQL
- Password SQL

Agregar lógica:

GenerateDatabaseName()

DetectSqlInstance()

InstallSqlExpressIfNeeded()

CheckDatabaseExists()

CreateDatabase()

---

# CREACIÓN DE BASE Y DATOS INICIALES

Si la base es creada:

Ejecutar scripts SQL del modelo DigitalPlus.

Debe dejar inicializados:

AspNetUsers

usuarios base:
- admin
- usuario normal

AspNetRoles

roles base del sistema.

Todas las tablas necesarias para que el sistema pueda iniciar correctamente.

---

# PRUEBAS OBLIGATORIAS

Caso 1  
Empresa nueva  
→ DB no existe  
→ se crea correctamente

Caso 2  
Empresa existente en nube  
→ instalación bloqueada  
→ usuario debe cambiar nombre

Caso 3  
Empresa existente en local  
→ instalación bloqueada  
→ usuario debe cambiar nombre

Caso 4  
Empresa con caracteres especiales

"Compañía Ñandú SRL"

→ CompaniaNanduSRL

Caso 5  
SQL Server no instalado

→ instalador instala SQL Express automáticamente

Caso 6  
Permisos insuficientes

→ error claro y rollback

---

# ARCHIVOS QUE DEBEN CAMBIAR

setup.iss

templates de configuración

scripts SQL

helper de creación de base (si se requiere)

---

# OUTPUT QUE NECESITO

1) setup.iss actualizado
2) función de sanitización de nombre de empresa
3) lógica de detección de instancias SQL
4) lógica de instalación automática de SQL Express
5) lógica de verificación de base existente
6) lógica de creación de base
7) checklist de pruebas
8) diagrama del flujo del instalador