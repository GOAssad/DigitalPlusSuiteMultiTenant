# CLAUDE INIT – Unificación de Instaladores DigitalPlus

## 1. Estado Actual del Proyecto

Actualmente el proyecto se encuentra en el siguiente estado:

- Existen dos aplicaciones de escritorio totalmente funcionales y compatibles con la base de datos **DigitalPlus**.
- Ambas aplicaciones comparten el mismo modelo de datos.
- Se desarrollaron instaladores independientes para cada una.

Ubicación de instaladores actuales:

- Fichadas:  
  `\fichadas\installer`

- Administrador:  
  `\Administrador\instalador`

Ambos instaladores funcionan correctamente de manera independiente.

---

## 2. Nuevo Objetivo

Se requiere unificar ambos instaladores en un único instalador maestro.

Motivo:

En muchos escenarios se deberán instalar ambas aplicaciones juntas en la misma PC.  
Ambas comparten:

- Nombre de empresa
- Parámetros de conexión a base de datos
- Lógica de creación/verificación de base
- Estructura del modelo de datos
- Usuarios iniciales del sistema
- Archivos de configuración similares

No tiene sentido mantener dos instaladores que solicitan la misma información.

---

## 3. Objetivo Técnico

Desarrollar un único instalador que:

1. Instale simultáneamente:
   - Aplicación Fichadas
   - Aplicación Administrador

2. Solicite una sola vez:
   - Servidor SQL
   - Nombre de base de datos
   - Usuario SQL
   - Password SQL
   - Nombre de empresa
   - Ruta de instalación

3. Configure automáticamente:
   - ConnectionStrings correctos para ambas aplicaciones
   - Archivos de configuración
   - Nombre de empresa visible en ambas apps
   - Accesos directos
   - Todo lo necesario para que ambas aplicaciones funcionen sin pasos manuales

---

## 4. Reglas de Funcionamiento del Nuevo Instalador

### 4.1 Configuración de Base de Datos

El instalador debe:

- Verificar si la base de datos existe.
- Si NO existe:
  - Crear la base DigitalPlus.
  - Ejecutar el modelo de datos completo.
  - Crear:
    - Usuario Administrador
    - Usuario estándar
    - Registros necesarios en tablas Identity (AspNetUsers, etc.)
- Si existe:
  - Validar que la estructura mínima requerida esté presente.
  - No duplicar información.
  - No sobrescribir datos existentes.

---

### 4.2 Configuración Compartida

El instalador debe generar dinámicamente:

- ConnectionString única basada en los datos ingresados.
- Archivos de configuración para ambas aplicaciones.
- Parámetro "NombreEmpresa" accesible desde ambas apps.
- Persistencia correcta en App.config o archivo correspondiente.

---

### 4.3 Instalación Física de Aplicaciones

Estructura sugerida:

C:\DigitalPlus\
│
├── Administrador\
└── Fichadas\

Debe:

- Copiar los binarios correspondientes.
- Generar accesos directos independientes:
  - DigitalPlus Administrador
  - DigitalPlus Fichadas
- Permitir selección de ruta personalizada si el usuario lo desea.

---

## 5. Requisitos Técnicos

Claude debe:

1. Analizar ambos instaladores actuales.
2. Identificar lógica repetida.
3. Diseñar un instalador unificado reutilizando componentes comunes.
4. Evitar duplicación de código.
5. Mantener compatibilidad total con:
   - .NET Framework 4.8
   - Modelo de datos actual
   - Estructura de soluciones existente

---

## 6. Entregables Esperados

Se debe generar:

1. Diseño técnico del instalador unificado.
2. Estructura del nuevo proyecto de instalación.
3. Lista de componentes necesarios.
4. Flujo completo de ejecución.
5. Código para:
   - Generación dinámica de ConnectionStrings.
   - Creación y validación de base de datos.
   - Escritura de archivos de configuración.
6. Instrucciones claras para compilar y generar el instalador final.

---

## 7. Restricciones

- No modificar la lógica interna de las aplicaciones.
- No alterar el modelo de datos existente.
- No romper compatibilidad actual.
- No eliminar instaladores anteriores hasta validar el nuevo instalador unificado.

---

## 8. Principio del Proyecto

El sistema ya funciona correctamente.

El objetivo no es corregir errores.
El objetivo es profesionalizar la instalación.

Resultado esperado:

✔ Un solo instalador  
✔ Una sola configuración  
✔ Cero pasos manuales  
✔ Instalación limpia y replicable  
✔ Ambas aplicaciones funcionando correctamente desde el primer inicio