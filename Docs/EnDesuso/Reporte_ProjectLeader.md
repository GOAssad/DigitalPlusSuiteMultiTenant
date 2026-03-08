# Reporte para Project Leader — TEntradaSalida (Fichador de Huellas)

**Fecha:** 2026-02-28
**Generado por:** Claude Sonnet 4.6 — Diagnóstico inicial
**Sistema:** Terminal de control de asistencia por huella digital

---

## 1. Visión General del Sistema

**¿Qué hace?**
Es una aplicación de escritorio Windows que funciona como **terminal de fichaje biométrico**. El empleado apoya el dedo en un lector USB (DigitalPersona uAreU 4500), el sistema compara la huella contra todos los empleados registrados en la base de datos, y registra automáticamente su entrada o salida. Un semáforo visual (verde/rojo/amarillo) indica el resultado al instante.

**¿Para quién?**
Uso en las terminales físicas de cada sucursal. No es una app de gestión ni de reportes — es una terminal de captura dedicada.

**Tecnología:**
- Lenguaje: C# / .NET Framework 4.8 / WinForms
- Base de datos: SQL Server (ADO.NET + Stored Procedures)
- SDK biométrico: DigitalPersona One Touch SDK 1.6.1
- Visual Studio 2019+

**Contexto de la solución:**
Este proyecto es parte de un ecosistema más amplio (DigitalOnePlus) que comparte 4 librerías comunes (`Global.Datos`, `Global.Funciones`, `Global.Controles`, `Acceso.Clases.Datos`). El módulo analizado es exclusivamente la terminal de fichaje.

---

## 2. Estado de Salud Técnica

| Dimensión | Estado | Comentario |
|---|---|---|
| Funcionalidad core | ✅ Operativa | El flujo de fichaje funciona correctamente |
| Seguridad | 🔴 Crítico | Credenciales en texto plano, SQL injection |
| Rendimiento | 🟡 Riesgo | Carga completa de huellas en cada scan |
| Robustez | 🟡 Riesgo | Sin manejo async, sin logging, catches vacíos |
| Mantenibilidad | 🟡 Regular | Sin tests, acoplamiento directo, sin DI |
| Portabilidad del build | 🟡 Riesgo | Rutas absolutas al SDK externo |
| Documentación técnica | 🔴 Ausente | Ninguna documentación previa |
| Tests automatizados | 🔴 Ausente | Cero tests en toda la solución |

**Diagnóstico global: el sistema cumple su función pero tiene deuda técnica significativa, especialmente en seguridad. No es apto para escalar o distribuir sin intervención.**

---

## 3. Tabla de Riesgos Priorizados

| # | Riesgo | Impacto | Probabilidad | Evidencia | Mitigación sugerida |
|---|---|---|---|---|---|
| R-1 | **Credenciales de BD (`sa/Soporte1`) en texto plano en app.config y código fuente** | Crítico | Alta | `app.config:9-12`, `ConnectionSql.cs:10-12`, `SQLServer.cs:468-471` | Cifrar con Windows DPAPI o externalizar a variables de entorno. Rotar contraseñas. No usar `sa`. |
| R-2 | **IP pública del servidor de producción expuesta en app.config** | Alto | Alta | `app.config:12` — `sd-1985882-l.ferozo.com:11434` | Mover configuración sensible fuera del código fuente. Usar nombres de perfil en lugar de IPs directas. |
| R-3 | **SQL Injection en consultas que concatenan strings** | Alto | Media | `GRALTerminales.cs:58`, `SQLServer.cs:225` | Reemplazar con parámetros `SqlParameter`. |
| R-4 | **Carga completa de huellas por cada scan (SELECT * en cada fichaje)** | Medio | Alta (si hay 50+ empleados) | `RRHHLegajosHuellas.cs:71` | Cachear la lista de huellas al arrancar la app y refrescar con un timer o evento. |
| R-5 | **Sin logging: los errores de producción son invisibles** | Medio | Alta | En toda la solución no existe sistema de log. Los errores se muestran como MessageBox o se descartan. | Incorporar NLog o Serilog con log en archivo local. |
| R-6 | **SDK biométrico hardcodeado en ruta absoluta del sistema** | Medio | Alta en nuevos equipos | `TEntradaSalida.csproj:62-76` | Incluir DLL en carpeta `lib\` del proyecto o usar variable de entorno. |
| R-7 | **Bug confirmado en RRHHFichadas.TraerFichadas()** | Bajo-Medio | Alta (siempre que se invoque) | `RRHHFichadas.cs:87-107` — usa `par` en lugar de `param` local recién declarado | Corregir para usar el array local `param`. |
| R-8 | **Sin autenticación: cualquiera puede ejecutar el fichador** | Bajo | Media | `Program.cs:19` — arranca directo sin login | Evaluar si la terminal física necesita protección de acceso. Si corre en kiosk, puede no ser necesario. |
| R-9 | **Proyectos "fantasma" en disco no incluidos en la solución** | Bajo | Baja | `Global.Controles - copia`, `ControlEntidad`, `Global.Calendario` sin .sln de referencia | Eliminar o documentar. Generan confusión al onboardear. |
| R-10 | **Sin tests automatizados** | Bajo a largo plazo | N/A | Cero archivos de test en toda la solución | Incorporar tests unitarios en lógica de negocio clave al refactorizar. |

---

## 4. Oportunidades Rápidas de Mejora

Las siguientes acciones tienen bajo costo de implementación y alto impacto inmediato:

1. **Cachear huellas al iniciar** (`FrmFichar.CaptureForm_Load`): cargar una vez el DataTable de huellas y reutilizarlo. Puede reducir el tiempo de respuesta del fichaje en un factor de 5-10x dependiendo del tamaño del plantel.

2. **Parametrizar los 2 SQL concatenados** (`GRALTerminales.cs`, `SQLServer.cs`): 30 minutos de trabajo, elimina riesgo de inyección.

3. **Agregar log a archivo** en los catch que hoy están vacíos: permite diagnosticar problemas en producción sin necesidad de conectarse al equipo.

4. **Eliminar `Global.Controles - copia`** del disco: evita confusión al desarrollo.

5. **Rotación de contraseña de BD**: cambiar `Soporte1` y usar un usuario no-`sa` con permisos mínimos (solo los SP y vistas necesarios).

---

## 5. Plan por Fases

### Fase 0: Estabilización (1–2 semanas)
*Objetivo: reducir riesgos activos sin romper nada.*

- [ ] Corregir bug `param` vs `par` en `RRHHFichadas.TraerFichadas()`
- [ ] Parametrizar SQL concatenados (GRALTerminales, ChequearConexion)
- [ ] Agregar logging básico a archivo (NLog, 1 día de trabajo)
- [ ] Rotar contraseña `Soporte1` y crear usuario de BD con permisos mínimos
- [ ] Limpiar artefactos: eliminar `Global.Controles - copia`
- [ ] Documentar las terminales registradas en `GRALTerminales` para onboarding

### Fase 1: Refactor Guiado (2–4 semanas)
*Objetivo: mejorar rendimiento y portabilidad sin cambiar arquitectura.*

- [ ] Implementar caché de huellas en memoria (carga al iniciar, timer de refresco cada N minutos)
- [ ] Mover DLL del SDK a carpeta `lib\` del proyecto (eliminar dependencia de ruta absoluta)
- [ ] Externalizar configuración sensible (usar Windows DPAPI o equivalente)
- [ ] Revisar y documentar todos los Stored Procedures usados por el módulo
- [ ] Agregar timeout explícito en las llamadas a BD durante el flujo de fichaje

### Fase 2: Mejora Estructural (1–3 meses)
*Objetivo: pagar deuda técnica, mejorar testabilidad.*

- [ ] Extraer la lógica de comparación de huellas a una clase separada (desacoplar del Form)
- [ ] Introducir inyección de dependencias básica (al menos para el acceso a datos)
- [ ] Agregar tests unitarios para la lógica de negocio (verificación, registro de fichada)
- [ ] Evaluar migración a .NET 6/8 LTS para soporte extendido
- [ ] Revisar si Entity Framework (configurado en app.config pero sin uso real) puede aprovecharse o debe eliminarse de la config

---

## 6. Preguntas Abiertas para el Dueño del Sistema

Estas preguntas deben responderse antes de tomar decisiones de arquitectura o seguridad:

1. **¿Qué connection string se usa en producción real?** El app.config tiene 4 cadenas (`Local`, `ksk`, `ConTocayAnda`, `ConTocayAnda_real`). El código usa `ConTocayAnda` por defecto. ¿La de `ferozo.com` es el servidor cloud de producción?

2. **¿Cuántos empleados hay registrados con huellas?** Esto determina la urgencia del caché de huellas (riesgo R-4). Con menos de 30 empleados el impacto es bajo; con 100+ es crítico.

3. **¿El comentario en `Program.cs` (`//Application.Run(new FrmIngresoEgreso())`) refiere a un formulario anterior?** ¿Existe `FrmIngresoEgreso` en otro lugar del ecosistema?

4. **¿Los proyectos `ControlEntidad`, `Global.Calendario`, `Global.DigitalPersona` presentes en disco son parte activa del ecosistema?** ¿O son proyectos abandonados?

5. **¿La app se distribuye como instalable o se ejecuta directamente desde red?** Esto determina el enfoque de manejo de la configuración segura y los DLL externos.

6. **¿Existe un sistema de backup de la BD y monitoreo del servidor `ferozo.com`?** La exposición de credenciales en texto plano lo hace más urgente.

7. **¿Se planea agregar más terminales?** Si es así, el modelo de configuración actual (un app.config por instalación con IP hardcodeada) no escala bien.

---

*Fin del Reporte Project Leader*
