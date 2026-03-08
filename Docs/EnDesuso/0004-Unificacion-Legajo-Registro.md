# TASK: Unificación de Solapas "Legajo" y "Registro"

## 🎯 Objetivo

Unificar la funcionalidad de las solapas **Legajo** y **Registro** dentro de una única solapa llamada **Legajo**, eliminando completamente la solapa **Registro** del formulario.
revisar el formato actual del formulario que se encuentra en 2 archivos que se encuentran en el folder \salidas donde Legajos-Legajo.png muestra la solapa Legajo actualmente y Legajos-Registro.png muestra la solapa Registro actualmente



La nueva solapa **Legajo** deberá contener:

- La estructura visual base actual de Legajo.
- La funcionalidad completa que actualmente está en la solapa Registro.
- Eliminación de los controles que quedarán obsoletos.

---

## 📌 Contexto Visual

- Imagen 1: `Legajos-Legajo.png` → Primera solapa actual (Legajo).
- Imagen 2: `Legajos-Registro.png` → Segunda solapa actual (Registro).

---

## 🔧 Cambios Requeridos

### 1️⃣ Eliminación de Controles en Solapa "Legajo"

Actualmente, en el panel derecho de la solapa **Legajo** existen los siguientes controles:

- Botón **Fichadas**
- Botón **Ausencias**
- Botón **Consolidado**
- Botón **Tarde**
- Control **Fecha Desde**

### Acción requerida:

- Eliminar completamente estos controles:
  - UI
  - Eventos asociados
  - Lógica vinculada
  - Referencias en código

No deben quedar handlers ni métodos huérfanos.

---

### 2️⃣ Migración de Funcionalidad de "Registro"

En el espacio donde estaban los controles eliminados se debe:

- Insertar toda la funcionalidad visual y lógica actualmente presente en la solapa **Registro**.
- Replicar exactamente:
  - Controles
  - Eventos
  - Lógica de negocio
  - Binding
  - Validaciones
  - Integración con datos
  - Dependencias

El comportamiento final debe ser idéntico al que hoy tiene la solapa Registro.

No duplicar lógica innecesariamente.

---

### 3️⃣ Eliminación Completa de la Solapa "Registro"

Una vez migrada la funcionalidad:

- Eliminar la pestaña "Registro" del TabControl.
- Eliminar su contenedor visual.
- Eliminar referencias en:
  - Eventos
  - Métodos
  - Inicializaciones
  - Cargas de datos
  - Designer

El proyecto debe compilar sin errores ni warnings nuevos.

---

## 🧠 Consideraciones Técnicas

- Analizar el ciclo de vida del formulario antes de modificar código.
- Revisar eventos como:
  - Load
  - SelectedIndexChanged
  - Inicialización de datos
- Si existen métodos compartidos, centralizar correctamente.
- Evitar duplicación de lógica.
- No modificar la lógica de negocio más allá de lo necesario.

---

## 🧪 Validaciones Obligatorias

1. Crear un legajo nuevo.
2. Editar un legajo existente.
3. Activar huella digital.
4. Cargar foto.
5. Guardar cambios.
6. Verificar que no existan:
   - NullReferenceException
   - Eventos huérfanos
   - Controles sin inicializar

---

## 📂 Entregables Esperados

1. Resumen técnico de cambios realizados.
2. Lista de archivos modificados.
3. Confirmación de compilación exitosa.
4. Indicar si fue necesario refactorizar estructura.

---

## 🚫 Restricciones

- No cambiar nombres de clases innecesariamente.
- No modificar estilos visuales salvo adaptación obligatoria al layout.
- No introducir mejoras no solicitadas.
- No alterar comportamiento funcional existente.

---

## 📌 Flujo de Trabajo Esperado

1. Analizar estructura actual del formulario.
2. Documentar estrategia antes de modificar código.
3. Aplicar cambios.
4. Verificar compilación.
5. Ejecutar validaciones.
6. Entregar informe técnico.