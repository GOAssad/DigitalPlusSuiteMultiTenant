# TASK: Reemplazar control de cámara en Administrador y rediseñar panel de foto

**Componente:** App de escritorio `Administrador`
**Área afectada:** Panel de captura de foto en el formulario de legajo

---

## 1. CONTEXTO

El Administrador actualmente tiene un control de cámara para capturar la foto del empleado que presenta un problema de **zoom no deseado**: aunque la foto se toma correctamente, el control aplica un zoom que distorsiona la visualización.

El componente `Fichador` tiene implementado un control de cámara distinto (el mismo utilizado para leer QR) que **no tiene este problema**. La solución es reutilizar ese control en el Administrador y, a la vez, **rediseñar el panel de control de foto** para agregar funcionalidades completas de gestión de imagen.

---

## 2. CAMBIO 1 — Reemplazar el control de cámara

Identificar qué control/librería/componente se usa en `Fichador` para acceder a la cámara en el modo de lectura QR y reemplazar el control de cámara actual del `Administrador` por ese mismo control.

**Objetivo:** eliminar el zoom no deseado que ocurre hoy en el Administrador al capturar la foto, manteniendo la calidad de imagen.

---

## 3. CAMBIO 2 — Rediseñar el panel de control de foto

El panel de control de la cámara debe ser **rediseñado completamente** con las siguientes funcionalidades, agrupadas por estado:

### 3.1 Estado: Sin foto

Cuando no hay foto cargada en el marco:

| Botón / Acción | Comportamiento |
|---|---|
| **Encender cámara** | Activa el feed de la cámara en el marco |
| **Tomar foto** | Captura el frame actual y lo muestra en el marco (estado de preview) |
| **Subir foto** | Abre selector de archivos. Formatos compatibles: todos los soportados nativamente por el componente de imagen en uso (JPG, PNG, BMP, GIF, WEBP, etc.). La imagen seleccionada se muestra en el marco como si hubiese sido tomada con la cámara |

### 3.2 Estado: Preview (foto capturada o subida, aún no aceptada)

| Botón / Acción | Comportamiento |
|---|---|
| **Aceptar foto** | La foto queda confirmada en el marco. Se habilitan los controles del estado "Con foto" |
| **Rechazar / Reintentar** | Descarta el preview y vuelve al estado "Sin foto" (cámara encendida si estaba activa) |

### 3.3 Estado: Con foto (aceptada)

| Botón / Acción | Comportamiento |
|---|---|
| **Guardar** | Guarda la imagen en binario en la base de datos, igual que hoy |
| **Eliminar imagen** | Borra la imagen del marco sin importar su origen (capturada, subida o cargada desde BD). Vuelve al estado "Sin foto" |
| **Descargar imagen** | Descarga la imagen actualmente mostrada en el marco como archivo de imagen. Este botón también debe estar disponible cuando se consulta un legajo existente que ya tiene foto |

### 3.4 Herramientas opcionales de edición de imagen

Si el stack tecnológico lo permite sin fricción excesiva, agregar las siguientes herramientas que actúan sobre la imagen aceptada antes de guardar:

| Herramienta | Descripción |
|---|---|
| **Recortar (crop)** | Permite seleccionar un área rectangular de la imagen y descartar el resto |
| **Eliminar fondo** | Detecta y elimina el fondo de la foto (útil para fotos de perfil). Usar librería disponible en el ecosistema del proyecto |
| **Rotar** | Rotación en pasos de 90° |
| **Espejo horizontal** | Voltear la imagen horizontalmente (útil si la cámara invierte la imagen) |

> Si alguna de estas herramientas genera complejidad desproporcionada, incluir solo las que se puedan implementar limpiamente. Prioridad: crop > rotar > espejo > eliminar fondo.

---

## 4. COMPORTAMIENTO AL CONSULTAR UN LEGAJO EXISTENTE

Cuando se abre el formulario de un legajo que **ya tiene foto** registrada en la BD:

- La foto se carga automáticamente en el marco al abrir el formulario.
- El panel de control debe mostrar directamente los botones del estado **"Con foto"**: Guardar, Eliminar imagen, Descargar imagen y herramientas de edición si las hay.
- El botón **Descargar** debe estar siempre visible cuando hay foto en el marco, independientemente del origen.

---

## 5. PERSISTENCIA — Sin cambios

El mecanismo de guardado en base de datos **no cambia**: la imagen se guarda en binario exactamente como se hace hoy. Solo cambia la UI y el control de cámara.

---

## 6. NOTAS DE IMPLEMENTACIÓN

- Respetar el estilo visual existente del Administrador.
- El panel de control puede organizarse como una barra de iconos/botones debajo o al costado del marco de cámara, según lo que quede más limpio con el layout actual.
- Los botones deben habilitarse/deshabilitarse según el estado actual (sin foto / preview / con foto) para evitar acciones inválidas.
- Si usás alguna librería nueva para edición de imagen, documentar cuál es y cómo se instaló.
