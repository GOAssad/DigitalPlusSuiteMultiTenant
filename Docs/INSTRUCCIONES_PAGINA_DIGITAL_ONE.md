# Instrucciones para la Página de Producto "Digital One" en IntegraIA.tech

> **Contexto**: Este documento contiene las instrucciones para crear la página del producto "Digital One" dentro del sitio web de IntegraIA (www.integraia.tech). La página va en la sección Productos. Está orientada al **usuario final** (empresas que quieren controlar asistencia de empleados). NO incluir detalles técnicos internos.

---

## 1. ESTRUCTURA DE LA PÁGINA

La página debe tener las siguientes secciones en orden:

### Hero / Banner Principal
- Título: **"Digital One"**
- Subtítulo: **"Control de Asistencia Inteligente para tu Empresa"**
- Descripción breve (2-3 líneas): Sistema integral de control de asistencia con fichada por huella digital, PIN o selección visual. Incluye aplicaciones de escritorio para fichada y administración, más un portal web para gestión desde cualquier lugar.
- Botón CTA principal: **"Solicitar Demo"** (link a contacto o formulario)
- Botón secundario: **"Descargar"** (ancla a sección descargas)
- **Imagen**: `hero-digital-one.png` — Composición que muestre las 3 interfaces: Fichador, Administrador y Portal Web juntos. Idealmente un mockup en pantallas.

---

### Sección: Modalidades de Fichada
Título: **"Tres formas de registrar asistencia"**

Mostrar 3 cards lado a lado:

**Card 1 - Huella Digital**
- Ícono: huella digital
- Título: "Huella Digital"
- Texto: "Identificación biométrica instantánea con lector DigitalPersona. El empleado apoya su dedo y el sistema lo reconoce en menos de 1 segundo entre todos los empleados registrados."
- **Imagen**: `fichador-huella.png` — Captura del Fichador mostrando la huella capturada y el semáforo verde con "Bienvenido [nombre]"

**Card 2 - PIN**
- Ícono: teclado numérico
- Título: "PIN Personal"
- Texto: "Cada empleado tiene un PIN numérico personal. Ideal como alternativa cuando no hay lector biométrico disponible. El PIN se puede cambiar voluntariamente y tiene política de expiración configurable."
- **Imagen**: `fichador-pin.png` — Captura del Fichador en modo PIN mostrando los campos de legajo y PIN

**Card 3 - Modo Visual**
- Ícono: lista de personas
- Título: "Selección Visual"
- Texto: "El empleado se selecciona de una lista visual. Pensado para demostraciones y entornos donde la simplicidad es prioridad."
- **Imagen**: `fichador-demo.png` — Captura del Fichador en modo Demo mostrando la lista de empleados

---

### Sección: Semáforo Visual
Título: **"Feedback inmediato para el empleado"**

Texto: "Al fichar, el sistema muestra un semáforo de colores que confirma visualmente el resultado:"
- 🟢 **Verde**: Fichada registrada correctamente. Muestra nombre del empleado y si es ENTRADA o SALIDA.
- 🟡 **Amarillo**: Procesando captura de huella.
- 🔴 **Rojo**: Huella no reconocida. El empleado debe reintentar.

**Imagen**: `fichador-semaforo.png` — Captura del Fichador con semáforo verde mostrando "Bienvenido [nombre] - ENTRADA"

---

### Sección: Administrador
Título: **"Gestión completa desde el escritorio"**

Texto introductorio: "La aplicación Administrador permite gestionar todos los aspectos del control de asistencia: empleados, horarios, sucursales, reportes y más."

Mostrar features en grilla (2 o 3 columnas):

1. **Gestión de Empleados**
   - Alta, baja y modificación de legajos
   - Foto del empleado vía webcam
   - Registro de huellas digitales (asistente guiado)
   - Asignación de PIN

2. **Fichadas y Reportes**
   - Consulta de fichadas por período y empleado
   - Reporte de llegadas tarde
   - Reporte de horas trabajadas
   - Exportación a Excel y PDF

3. **Tablas Maestras**
   - Sucursales (ubicaciones físicas)
   - Categorías de empleados
   - Horarios y turnos
   - Sectores y departamentos
   - Incidencias (vacaciones, licencias, permisos)
   - Feriados

4. **Gestión de PINs**
   - Vista de todos los empleados con estado de PIN
   - Filtros: con PIN, sin PIN, expirados, pendientes de cambio
   - Forzar cambio de PIN
   - Resetear PIN

5. **Configuración de Fichada**
   - Habilitar/deshabilitar modo PIN
   - Configurar días de expiración de PIN
   - Habilitar/deshabilitar modo Demo

6. **Identidad de la Empresa**
   - Logo de la empresa visible en la aplicación
   - Links automáticos a redes sociales y web de la empresa

**Imagen principal**: `administrador-principal.png` — Captura de la pantalla principal del Administrador mostrando el menú lateral y un formulario abierto (ej: lista de legajos)

**Imagen secundaria**: `administrador-huellas.png` — Captura del formulario de registro de huellas digitales

---

### Sección: Portal Web
Título: **"Tu empresa en la palma de tu mano"**

Texto introductorio: "Accedé a toda la información de asistencia desde cualquier navegador, en cualquier dispositivo. El portal web te da visibilidad completa del estado de tu equipo en tiempo real."

Features destacadas:

1. **Dashboard en Tiempo Real**
   - Empleados presentes vs ausentes hoy
   - Llegadas tarde del día
   - Total de fichadas (entradas/salidas)
   - Gráfico semanal de asistencia
   - Distribución horaria de fichadas
   - Últimas fichadas en tiempo real

2. **Gestión de Empleados**
   - Alta, baja y modificación desde el navegador
   - Búsqueda y filtros
   - Vista de detalle completa

3. **Consulta de Fichadas**
   - Filtro por empleado, fecha y tipo
   - Exportación de datos

4. **Administración de Entidades**
   - Horarios, Categorías, Sectores, Sucursales
   - Terminales registradas
   - Incidencias y vacaciones
   - Feriados

5. **Gestión de Usuarios**
   - Crear y administrar usuarios del portal
   - Roles y permisos
   - Cambio de contraseña obligatorio en primer ingreso

6. **Seguridad**
   - Aislamiento total de datos entre empresas
   - Cada empresa solo ve su propia información
   - Autenticación segura por email y contraseña

**Imagen principal**: `portal-dashboard.png` — Captura del Dashboard del portal mostrando los KPIs, gráficos y últimas fichadas

**Imagen secundaria**: `portal-legajos.png` — Captura de la lista de empleados en el portal web

**Imagen terciaria**: `portal-fichadas.png` — Captura de la consulta de fichadas con filtros

---

### Sección: Multi-Empresa (Cloud)
Título: **"Una plataforma, múltiples empresas"**

Texto: "Digital One funciona en modalidad multi-tenant: todas tus sucursales y empresas del grupo comparten la misma plataforma, con aislamiento total de datos. Cada empresa tiene su propia configuración, empleados, horarios y reportes completamente independientes."

Bullets:
- Base de datos compartida con aislamiento por empresa
- Cada empresa con su propio logo, branding y redes sociales
- Gestión centralizada desde el portal de licencias
- Alta de nuevas empresas en minutos con usuario admin auto-provisionado
- Suspensión/reactivación de empresas con efecto inmediato

**Imagen**: `multi-empresa.png` — Diagrama conceptual mostrando múltiples empresas conectadas a la misma plataforma (puede ser un diseño gráfico, no captura)

---

### Sección: Descargas
Título: **"Descargá Digital One"**

Mostrar 2 cards de descarga + 1 de documentación:

**Card 1 - Instalador Cloud (Liviano)**
- Título: "Instalador Cloud"
- Peso: ~25 MB
- Texto: "Para empresas que ya tienen su cuenta creada en la plataforma. Requiere un código de activación proporcionado por el administrador. No instala base de datos local; se conecta directamente a la nube."
- Requisitos: Windows 10 o superior, conexión a internet, código de activación
- Botón: **"Descargar Instalador Cloud"**
- **Archivo de descarga**: `DigitalPlus_Cloud_Setup_v1.0.exe`
- **Cuándo usar**: "Elegí esta opción si tu empresa ya está registrada en Digital One y te dieron un código de activación."

**Card 2 - Instalador Completo**
- Título: "Instalador Completo"
- Peso: ~180 MB
- Texto: "Instalación independiente con base de datos local incluida (SQL Server Express). Ideal para empresas que quieren operar de forma autónoma sin depender de conexión a internet."
- Requisitos: Windows 10 o superior, 4 GB RAM mínimo
- Botón: **"Descargar Instalador Completo"**
- **Archivo de descarga**: `DigitalPlus_Setup_v1.0.exe`
- **Cuándo usar**: "Elegí esta opción si querés instalar Digital One en tu red local sin necesidad de conexión permanente a internet."

**Card 3 - Manual de Usuario**
- Título: "Manual de Usuario"
- Texto: "Guía completa de uso del Fichador, Administrador y Portal Web."
- Botón: **"Descargar Manual (PDF)"**
- **Archivo de descarga**: `DigitalOne_Manual_Usuario.pdf`

---

### Sección: Planes y Precios (placeholder)
Título: **"Planes"**

> NOTA: Esta sección debe quedar con contenido genérico o placeholder porque los planes y precios no están definidos aún. Sugerir algo como:

- **Trial**: 14 días gratis, hasta 5 empleados
- **Básico**: Hasta X empleados (precio a definir)
- **Profesional**: Hasta X empleados (precio a definir)
- **Enterprise**: Sin límite de empleados (precio a definir)

Botón: **"Contactanos para más información"**

---

### Sección: CTA Final
Título: **"¿Listo para modernizar el control de asistencia de tu empresa?"**
- Botón principal: **"Solicitar Demo Gratuita"**
- Botón secundario: **"Contactar a Ventas"**

---

## 2. LISTA DE IMÁGENES NECESARIAS

El usuario debe proporcionar estas imágenes con los nombres exactos indicados. Si se colocan en la carpeta correcta del proyecto web con estos nombres, la página debería funcionar directamente.

| # | Archivo | Descripción | Dónde obtenerla |
|---|---------|-------------|-----------------|
| 1 | `hero-digital-one.png` | Composición hero: Fichador + Administrador + Portal juntos | Diseñar o capturar las 3 apps y componer |
| 2 | `fichador-huella.png` | Fichador con huella capturada y semáforo verde | Captura de pantalla del Fichador |
| 3 | `fichador-pin.png` | Fichador en modo PIN (campos legajo y PIN visibles) | Captura del Fichador en modo PIN |
| 4 | `fichador-demo.png` | Fichador en modo Demo (lista de empleados) | Captura del Fichador en modo Demo |
| 5 | `fichador-semaforo.png` | Fichador con semáforo verde y "Bienvenido - ENTRADA" | Captura del Fichador tras fichada exitosa |
| 6 | `administrador-principal.png` | Pantalla principal del Administrador con menú | Captura del Administrador |
| 7 | `administrador-huellas.png` | Formulario de registro de huellas | Captura del wizard de huellas |
| 8 | `portal-dashboard.png` | Dashboard del portal con KPIs y gráficos | Captura del portal web en /dashboard |
| 9 | `portal-legajos.png` | Lista de empleados en el portal | Captura del portal en /legajos |
| 10 | `portal-fichadas.png` | Consulta de fichadas con filtros | Captura del portal en /fichadas |
| 11 | `multi-empresa.png` | Diagrama multi-empresa conceptual | Diseño gráfico (no captura) |
| 12 | `logo-digital-one.png` | Logo del producto Digital One | Diseñar o usar existente |

**Formato recomendado**: PNG, resolución mínima 1200px de ancho, fondo limpio.

---

## 3. ARCHIVOS DE DESCARGA

Estos archivos deben estar disponibles para descarga directa:

| Archivo | Ubicación actual | Notas |
|---------|-----------------|-------|
| `DigitalPlus_Cloud_Setup_v1.0.exe` | `Instaladores/InstaladorLiviano/Output/` | ~25 MB |
| `DigitalPlus_Setup_v1.0.exe` | `Instaladores/InstaladorUnificado/Output/` | ~180 MB (pendiente fix compilación) |
| `DigitalOne_Manual_Usuario.pdf` | Generar desde `Docs/DOC02` | Convertir a PDF |

---

## 4. ESTILO Y TONO

- **Tono**: Profesional pero accesible. No usar jerga técnica.
- **Audiencia**: Dueños de empresas, gerentes de RRHH, responsables de operaciones.
- **Palabras clave**: control de asistencia, fichada, huella digital, PIN, portal web, multi-empresa, cloud, reportes, empleados.
- **NO mencionar**: SQL Server, .NET, Blazor, DPAPI, connection strings, EmpresaId, multi-tenant (usar "multi-empresa" en su lugar).
- **Idioma**: Español rioplatense (vos, usá, elegí, descargá).

---

## 5. SEO

- **Title tag**: "Digital One - Control de Asistencia por Huella Digital | IntegraIA"
- **Meta description**: "Sistema de control de asistencia con fichada por huella digital, PIN y portal web. Gestión de empleados, reportes y multi-empresa en la nube."
- **H1**: "Digital One"
- **URL sugerida**: `/productos/digital-one`

---

## 6. NOTAS PARA EL DESARROLLADOR

- Las capturas de pantalla del Fichador están en `DigitalPlusSuiteMultiTenant/Docs/Salidas/` (algunas se pueden usar como referencia pero necesitan capturas limpias).
- El portal web está en `https://digitalplusportalmt.azurewebsites.net` (login: admin@kosiuko.com / Admin123) para capturar screenshots.
- El portal de licencias NO se muestra en la página del producto (es backoffice interno de IntegraIA).
- El InstaladorUnificado tiene un bug de compilación pendiente (ruta de ícono). Mientras tanto, el botón de descarga puede estar deshabilitado o linkeado a "Próximamente".
