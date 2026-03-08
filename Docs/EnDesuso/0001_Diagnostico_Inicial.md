# Diagnóstico Inicial del Sistema – Proyecto C# .NET Framework 4.8

## Contexto

Se requiere realizar un estudio completo del sistema de escritorio desarrollado en C# con .NET Framework 4.8.

El objetivo es comprender integralmente el sistema, documentar su arquitectura real, detectar riesgos y deuda técnica, y generar documentación accionable para programador y project leader.

---

# Entregables Obligatorios

Crear dentro de la carpeta `DocumentacionClaude/` los siguientes archivos:

1. `Reporte_Programador.md`
2. `Reporte_ProjectLeader.md`

Ambos deben estar escritos en Markdown, ser concretos, estructurados y contener referencias explícitas a rutas, archivos, clases y métodos cuando corresponda.

---

# Alcance del Análisis

## 1. Inventario y Estructura

- Listar todas las soluciones (`.sln`) y proyectos (`.csproj`)
- Indicar tipo de proyecto (WinForms, WPF, Console, Class Library, Service, etc.)
- Identificar el proyecto de inicio (Startup Project)
- Identificar formularios/pantallas principales
- Determinar estructura de capas (UI, Dominio, Datos, Servicios), incluso si están mezcladas

---

## 2. Dependencias y Entorno

- Enumerar todos los paquetes NuGet con versión
- Identificar dependencias externas (DLL manuales, COM, SDKs)
- Identificar frameworks o librerías clave:
  - ORM (Entity Framework, Dapper, ADO.NET)
  - Logging
  - DI
  - Reporting
  - Controles UI
- Configuración de compilación (Debug/Release, AnyCPU/x86/x64)
- Requisitos para ejecución (connection strings, archivos externos, permisos, variables)

---

## 3. Configuración y Seguridad

- Revisar `App.config` y cualquier archivo de configuración adicional
- Identificar:
  - connectionStrings
  - endpoints
  - credenciales hardcodeadas
  - rutas absolutas
- Señalar qué configuraciones deberían externalizarse o protegerse

---

## 4. Persistencia y Base de Datos

- Determinar mecanismo de acceso a datos:
  - ADO.NET
  - Entity Framework
  - Dapper
  - Stored Procedures
- Identificar:
  - tablas/vistas/SP más utilizados
  - patrón de acceso (repositorios, helpers SQL, acceso directo)
  - manejo de transacciones
  - manejo de errores
- Detectar SQL inseguro (concatenación de strings sin parámetros)

---

## 5. Arquitectura Real y Flujos

- Describir el flujo de arranque de la aplicación
- Describir flujo de login/autenticación (si existe)
- Documentar 3 casos de uso principales
- Identificar módulos funcionales
- Detectar:
  - clases excesivamente grandes
  - alto acoplamiento
  - duplicación de código

---

## 6. Calidad y Riesgos Técnicos

Identificar:

- Manejo de excepciones (try/catch vacíos o silenciosos)
- Uso de threads, tasks o background workers
- Riesgo de bloqueo de UI por llamadas a DB
- Recursos no liberados (Dispose faltante)
- Rutas hardcodeadas
- Credenciales embebidas
- Falta de cifrado
- Ausencia de tests
- Baja testeabilidad

---

## 7. Compilación y Ejecución

Documentar:

- Cómo abrir la solución
- Restaurar paquetes
- Compilar
- Ejecutar
- Errores encontrados
- Troubleshooting
- Checklist para levantar entorno limpio

---

# Estructura del Reporte_Programador.md

Debe contener:

1. Resumen técnico del sistema
2. Cómo correr el proyecto (paso a paso)
3. Mapa del repositorio (tabla: proyecto | tipo | responsabilidad)
4. Dependencias
5. Configuración relevante
6. Acceso a datos
7. Top 10 puntos críticos (con referencia a archivo/clase/método)
8. Backlog técnico sugerido:
   - Quick wins (1–3 días)
   - Mejoras medianas (1–3 semanas)

---

# Estructura del Reporte_ProjectLeader.md

Debe contener:

1. Visión general del sistema
2. Estado de salud técnica
3. Tabla de riesgos priorizados:
   - Riesgo
   - Impacto
   - Probabilidad
   - Evidencia
   - Mitigación sugerida
4. Oportunidades rápidas de mejora
5. Plan por fases:
   - Fase 0: Estabilización
   - Fase 1: Refactor guiado
   - Fase 2: Mejora estructural
6. Preguntas abiertas que deben responderse con el dueño del sistema

---

# Reglas

- No inventar información.
- Si algo no está claro, marcar como "NO CONFIRMADO".
- Cada hallazgo importante debe incluir referencia concreta a archivo o clase.
- Escribir en español técnico claro.
- No incluir relleno.
- Crear rama sugerida: `docs/diagnostico-inicial`
- Commits descriptivos.

---

# Opcional

- Incluir diagramas Mermaid simples (flujo de arranque o dependencias entre proyectos).
- Identificar hotspots por tamaño o complejidad.

---

Fin del documento.