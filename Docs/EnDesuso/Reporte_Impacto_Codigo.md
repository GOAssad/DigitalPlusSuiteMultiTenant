# Reporte: Auditoría de Código — Administrador
## Etapa 2 — Impacto sobre base DigitalPlus

**Proyecto auditado:** `Administrador\Acceso\` (solución DigitalOneAdministrador.sln)
**Fecha relevamiento:** 2026-03-01
**Generado por:** Etapa 2 — Auditoría código

---

## CONTEXTO CLAVE

El Administrador fue construido para **DigitalOne** (sistema anterior).
La base **DigitalPlus** es el resultado de una migración ya ejecutada sobre la que trabaja el proyecto web en producción.
La estructura de DigitalPlus **no puede modificarse**.
El objetivo es adaptar el Administrador para que apunte a DigitalPlus sin tocar el schema.

---

## 1. Mapping completo DigitalOne → DigitalPlus

| Tabla DigitalOne (Administrador) | Tabla DigitalPlus | Campo renombrado clave | Estado |
|---|---|---|:---:|
| `RRHHLegajos` | `Legajos` | `sApellido`+`sNombre` → `Nombre` | ⚠️ Adaptar |
| `RRHHLegajosHuellas` | `LegajosHuellas` | — | ⚠️ Adaptar |
| `RRHHFichadas` | `Fichadas` | `dRegistro`→`Registro`, `sEntraSale`→`EntraSale` | ⚠️ Adaptar |
| `RRHHHorarios` | `Horarios` | `sDescripcion`→`Nombre` | ⚠️ Adaptar |
| `RRHHCategorias` | `Categorias` | `sDescripcion`→`Nombre` | ⚠️ Adaptar |
| `RRHHUbicaciones` | `Sectores` | `sDescripcion`→`Nombre` (renombre completo) | ⚠️ Adaptar |
| `RRHHIncidencias` | `Incidencias` | `sDescripcion`→`Nombre` | ⚠️ Adaptar |
| `RRHHIncidenciasLegajos` | `IncidenciasLegajos` | estructura distinta | ⚠️ Adaptar |
| `GralSucursales` | `Sucursales` | `sSucursalId`→`CodigoSucursal`, `sDescripcion`→`Nombre` | ⚠️ Adaptar |
| `GralTerminales` | `Terminales` | `sDescripcion`→`Nombre` | ⚠️ Adaptar |
| `GRALVariablesGlobales` | `VariablesGlobales` | estructura distinta | ⚠️ Adaptar |
| `GRALUsuarios` | `GRALUsuarios` | mismo nombre | ✅ OK |
| `GRALSucursalesGrupos` | **(sin equivalente)** | no migrada | ❌ No existe |
| `METAMenu` | **(sin equivalente)** | no migrada | ❌ No existe |

**Tablas nuevas en DigitalPlus** (no existían en DigitalOne, el Administrador no las usa aún):

| Tabla | Propósito |
|---|---|
| `HorariosDias` | Detalle de horario por día de semana |
| `HorariosDiasEventos` | Calendario personalizado por legajo |
| `LegajosSucursales` | Relación empleado-sucursal (many-to-many) |
| `UsuariosSucursales` | Relación usuario web-sucursal |
| `Vacaciones` | Vacaciones por legajo |
| `Dias` | Catálogo días de semana |
| `Dedos` | Catálogo dedos (1-10, por SDK) |

---

## 2. Connection Strings — estado actual

**Archivo:** `Acceso\App.config`

Ninguna connection string apunta a `GUS-IDEAPAD` / base `DigitalPlus`.

| Nombre | Servidor actual | Base | Acción |
|---|---|---|:---:|
| `local` | `NOTE\SQL2014` | `DigitalOne` | 🔴 Cambiar a GUS-IDEAPAD / DigitalPlus |
| `ksk` | `192.168.0.11` | `Tocayanda` | 🔴 Cambiar |
| `Remoto` | `SQL5092.site4now.net` | `db_a750c0_digitaloneplus` | 🔴 Cambiar |
| `ConTocayAnda` | Azure SQL | `DigitalOne` | 🔴 Cambiar |
| `PRD01ConnectionString` | `192.168.0.11` | `PRD01` | 🔴 Cambiar |
| `DigitalOneConnectionString` | Azure SQL | `DigitalOne` | 🔴 Cambiar |

**Default del control `ctrParametrosConexion`** (hardcodeado en Designer.cs):
- Servidor: `NOTE\SQL2014` → cambiar a `GUS-IDEAPAD`
- BD: `GPDIN` → cambiar a `DigitalPlus`

---

## 3. Stored Procedures — análisis completo

### 3.1 SPs que SÍ existen en DigitalPlus (a usar directamente)

| SP en DigitalPlus | Función | Llamado actualmente por el Administrador |
|---|---|:---:|
| `EscritorioFichadasSPSALIDA` | Registrar fichada | ❌ No (solo lo usa el Fichador) |
| `EscritorioLegajoActualizar` | Alta/modificación empleado | ❌ No (debería usarlo) |
| `EscritorioLegajosHuellasActualizar` | Guardar huella | ❌ No (usa DELETE directo con nombre incorrecto) |
| `RRHHLegajos_DeleteTodo` | Eliminar empleado completo | ❌ No |

### 3.2 SPs del Administrador que NO existen en DigitalPlus (a crear)

| SP llamado por Administrador | Equivalente funcional en DigitalPlus | Prioridad |
|---|---|:---:|
| `RRHHFichadas_SP_MANUAL_SELECT_GRUPO` | Consulta de fichadas por grupo/rango | 🔴 Alta |
| `RRHHFichadas_SP_MANUAL_SELECT_GRUPO_SUCURSAL` | Consulta de fichadas por grupo+sucursal | 🔴 Alta |
| `RRHHFichadas_SP_MANUAL_SELECT_GRUPO_FERIADOS` | Consolidado con feriados | 🔴 Alta |
| `RRHHFichadas_SP_ELIMINAR` | DELETE de Fichadas por @id | 🔴 Alta (simple) |
| `RRHHFichadas_Consolidado` | Consolidado por legajo | 🔴 Alta |
| `_RRHHFichadasAusencias_SP_SELECT` | Ausencias por grupo | 🟠 Media |
| `RRHHFichadasAusenciasSucursal_SP_SELECT` | Ausencias por sucursal | 🟠 Media |
| `RRHHFichadasEntradaEstatus_SP_SELECT` | Llegadas tarde | 🟠 Media |
| `RRHHIncidencias_SP_MANUAL` | Gestión manual incidencias | 🟡 Baja |
| `RRHHIncidenciasLegajos_SP_SELECT` | Consulta incidencias por legajo | 🟡 Baja |
| `RRHHIncidenciasLegajosEliminar_SP_SELECT` | Consulta para eliminar incidencias | 🟡 Baja |
| `RRHHIncidenciasLegajos_Delete` | Eliminar incidencia de legajo | 🟡 Baja |
| `GRALVariablesGlobales_SP` | Guardar variable de configuración | 🟡 Baja |

**Nota:** Los SPs de tipo `Web*` en DigitalPlus (`WebConsolidado_Listado`, `WebControlAcceso_Listado`, etc.) implementan la misma lógica que los SPs del Administrador, pero con firma diferente. Pueden servir como base para crear las versiones de escritorio.

---

## 4. Tabla de impacto por módulo

### MÓDULO CRÍTICO: Enrolamiento de huellas (uAreu)

| Archivo | Problema | Solución |
|---|---|---|
| `uAreu\FrmRRHHLegajosUareU.cs` | Queries a `RRHHLegajos`, `RRHHHorarios`, `RRHHCategorias`, `RRHHUbicaciones` | Cambiar a `Legajos`, `Horarios`, `Categorias`, `Sectores` + renombrar campos |
| `uAreu\FrmRRHHLegajosUareU.cs` | Usa `sNombre` (campo inexistente) | Cambiar a `Nombre` |
| `uAreu\FrmRegistrar.cs` | `DELETE RRHHLegajosHuellas WHERE ...` | Cambiar a `DELETE LegajosHuellas WHERE ...` |
| `uAreu\EnrollmentForm.cs` | `DELETE RRHHLegajosHuellas WHERE ...` | Cambiar a `DELETE LegajosHuellas WHERE ...` |
| `uAreu\frmVerificar.cs` | `SELECT sNombre from RRHHLegajos` | Cambiar a `SELECT Nombre from Legajos` |
| `uAreu\FrmIngresoEgreso.cs` | `SELECT sNombre from RRHHLegajos` | Cambiar a `SELECT Nombre from Legajos` |

**Impacto:** CRÍTICO — este módulo es el núcleo funcional del Administrador.

### MÓDULO CRÍTICO: Legajos y huellas (RRHH)

| Archivo | Problema | Solución |
|---|---|---|
| `RRHH\FrmRRHHLegajos.cs` | `DELETE RRHHLegajosHuellas` | Cambiar a `LegajosHuellas` |
| `RRHH\FrmRRHHLegajos.cs` | SP `RRHHFichadas_Consolidado` | Crear SP en DigitalPlus |
| `RRHH\FrmRRHHLegajos.cs` | SP `RRHHFichadas_SP_MANUAL_SELECT_GRUPO_FERIADOS` | Crear SP en DigitalPlus |
| `RRHH\FrmRRHHLegajos.cs` | SP `RRHHFichadas_SP_ELIMINAR` | Crear SP simple: `DELETE Fichadas WHERE Id=@id` |
| `RRHH\FrmRRHHLegajos.cs` | SP `RRHHIncidenciasLegajos_Delete` | Crear SP en DigitalPlus |
| `RRHH\FrmRRHHMasterFichadasAgregar.cs` | `RRHHLegajos`, `GRALSucursales`, `GRALSucursalesGrupos` | Cambiar nombres + eliminar GruposSucursales |
| Todos los `FrmRRHHMasterFichadas*.cs` | SPs inexistentes en DigitalPlus | Crear SPs o reemplazar por SPs Web existentes |

### MÓDULO IMPORTANTE: Catálogos GRAL

| Archivo | Problema | Solución |
|---|---|---|
| `Generales\FrmGRALSucursales.Designer.cs` | `SELECT from GRALSucursales` | Cambiar a `Sucursales` con alias |
| `Generales\FrmGRALTerminales.Designer.cs` | `SELECT from GRALTerminales`, `GRALSucursales` | Cambiar a `Terminales`, `Sucursales` |
| `Generales\FrmGRALUsuarios.Designer.cs` | `SELECT from GRALUsuarios` | ✅ Sin cambios (tabla existe igual) |
| `Generales\FrmGRALSucursalesGrupos.cs` | `GRALSucursalesGrupos` no existe | Evaluar si se elimina el módulo |
| `Generales\FrmGRALConfiguracion.cs` | SP `GRALVariablesGlobales_SP` | Crear SP sobre tabla `VariablesGlobales` |
| `Generales\FrmMainMenu.cs` | `SELECT from METAMenu` | Tabla no existe — desactivar o reimplementar |

### MÓDULO SECUNDARIO: Reportes

| Archivo | Problema | Solución |
|---|---|---|
| `Reportes\frmFichadasReport.cs` | `SELECT from RRHHUbicaciones` | Cambiar a `Sectores` |
| `Reportes\FrmRptFichadas.cs` | `SELECT from RRHHUbicaciones` | Cambiar a `Sectores` |
| `Reportes\FrmRptHorasTrabajadas.cs` | `SELECT from RRHHUbicaciones` | Cambiar a `Sectores` |
| `Reportes\FrmRptHorasTrabajadas.Designer.cs` | `SELECT from RRHHLegajos` | Cambiar a `Legajos` con alias |
| Todos los `*.Designer.cs` de reportes | `SELECT from GRALSucursalesGrupos` | Eliminar filtro o reemplazar con `Sucursales` |

### MÓDULO SIN EQUIVALENTE: Sin acción posible directa

| Módulo | Estado | Decisión pendiente |
|---|---|---|
| `GRALSucursalesGrupos` | No migrado a DigitalPlus | ¿Eliminar del menú? |
| `METAMenu` | No migrado a DigitalPlus | ¿Hardcodear el menú? |
| `Ventas / Clientes` | Fuera de scope de fichadas | ¿Mantener desactivado? |

---

## 5. Impacto de los cambios de nombre de campo

El código del Administrador usa notación húngara (`s`=string, `n`=int, `l`=bool, `d`=date).
DigitalPlus eliminó esa notación. Cada query del Administrador necesita renombrar los campos con aliases o cambiar el código C# que los lee.

| Patrón viejo | Patrón nuevo | Impacto |
|---|---|---|
| `sDescripcion` | `Nombre` | Todos los grids y bindings |
| `sNombre`, `sApellido` | `Nombre` | FrmRRHHLegajosUareU y otros |
| `sLegajoID` | `LegajoId` | Queries y SP params |
| `lActivo` | `Activo` | Filtros |
| `dRegistro` | `Registro` | Fichadas |
| `sEntraSale` | `EntraSale` | Fichadas |
| `sSucursalID` (string) | `SucursalId` (int) | **Tipo cambia** — más complejo |
| `sLegajoID` (string como FK) | `LegajoId` (int como FK) | **Tipo cambia** — más complejo |

---

## 6. Resumen ejecutivo de riesgos

| Nivel | Cantidad | Descripción |
|---|:---:|---|
| 🔴 CRÍTICO | 1 | Connection strings incorrectas — ninguna apunta a DigitalPlus |
| 🔴 ALTO | ~30 | Nombres de tablas incorrectos (prefijo RRHH/GRAL) |
| 🔴 ALTO | 13 | SPs inexistentes en DigitalPlus |
| 🟠 MEDIO | 8 | Cambio de tipo en FK (string → int) en Sucursal/Legajo |
| 🟠 MEDIO | ~20 | Nombres de campos (notación húngara vs limpia) |
| ❌ Sin solución directa | 3 | `GRALSucursalesGrupos`, `METAMenu`, Ventas |
| ✅ OK | 1 | `GRALUsuarios` — igual en ambos sistemas |

---

## 7. Estrategia de adaptación recomendada (para Etapa 3)

### Restricciones confirmadas:
- NO cambiar estructura de DigitalPlus
- NO romper el Fichador (que ya funciona)
- NO romper el proyecto web

### Enfoque recomendado: Adaptar el código C# + Crear SPs faltantes

**Fase 3A — Quick wins (sin dependencias):**
1. Corregir connection strings en `App.config` y default de `ctrParametrosConexion`
2. Crear SP simple `RRHHFichadas_SP_ELIMINAR` (`DELETE Fichadas WHERE Id=@id`)
3. Corregir el módulo uAreu (enrolamiento) — cambiar nombres de tablas y campos (es el núcleo)

**Fase 3B — Módulo legajos (core):**
4. Adaptar `FrmRRHHLegajos.cs` y módulo RRHH: renombrar tablas, cambiar campo types
5. Crear SPs de consulta de fichadas (basarse en los `Web*` SPs que ya existen)

**Fase 3C — Catálogos:**
6. Corregir formularios GRAL (Sucursales, Terminales, Categorías, Horarios)
7. Decidir qué hacer con `GRALSucursalesGrupos` y `METAMenu`

**Fase 3D — Reportes:**
8. Adaptar reportes: cambiar `RRHHUbicaciones` → `Sectores`, `RRHHLegajos` → `Legajos`

### Lo que el proyecto Web ya tiene (no duplicar):
Los SPs `WebConsolidado_Listado`, `WebControlAcceso_Listado`, `WebLlegadaTarde_Listado`, `WebHorasExtras_Listado` ya implementan la lógica de reporting en DigitalPlus.
El Administrador puede crear wrappers que llamen estos mismos SPs, o crear versiones de escritorio con la misma lógica.

---

## 8. Archivos clave del plan

| Archivo | Descripción |
|---|---|
| `documentacionClaude/0003_Adaptacion_a_Base_de_Datos.md` | Plan completo (4 etapas) |
| `documentacionClaude/Reporte_Modelo_DB_Actual.md` | ✅ Etapa 1 — Modelo DB DigitalPlus |
| `documentacionClaude/Reporte_Impacto_Codigo.md` | ✅ Etapa 2 — Este reporte |
| `documentacionClaude/Plan_Adaptacion_Administrador.md` | ⏳ Etapa 3 — Plan detallado (pendiente) |
