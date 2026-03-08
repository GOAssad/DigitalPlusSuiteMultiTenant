# Reporte: Modelo de Base de Datos — DigitalPlus

**Servidor:** GUS-IDEAPAD
**Base:** DigitalPlus
**Fecha relevamiento:** 2026-02-28
**Generado por:** Etapa 1 — Análisis Base DigitalPlus

---

## 1. Tablas relevantes para la tarea

### 1.1 Legajos — Empleados/personas

| Campo | Tipo | PK | FK | Nullable | Identity | Notas |
|---|---|:---:|---|:---:|:---:|---|
| Id | int | ✓ | — | NO | ✓ | PK autoincremental |
| LegajoId | nvarchar(25) | — | — | NO | — | Código alfanumérico del empleado (clave natural) |
| Nombre | nvarchar(200) | — | — | NO | — | Nombre completo |
| SectorId | int | — | → Sectores.Id | NO | — | |
| CategoriaId | int | — | → Categorias.Id | NO | — | |
| HorarioId | int | — | → Horarios.Id | **SÍ** | — | ⚠️ Nullable — ver riesgo en §5 |
| Activo | bit | — | — | NO | — | |
| CalendarioPersonalizado | bit | — | — | NO | — | |

**Registros actuales:** 758

---

### 1.2 LegajosHuellas — Huellas digitales

| Campo | Tipo | PK | FK | Nullable | Notas |
|---|---|:---:|---|:---:|---|
| LegajoId | int | ✓ | → Legajos.Id | NO | PK compuesta |
| DedoId | int | ✓ | → Dedos.Id | NO | PK compuesta |
| sLegajoId | nvarchar(5) | — | — | NO | ⚠️ Copia string del LegajoId — solo 5 chars (Legajos.LegajoId es 25) |
| huella | varbinary(max) | — | — | **SÍ** | Template de huella (binario SDK) |
| nFingerMask | int | — | — | NO | Máscara de dedo |
| sLegajoNombre | nvarchar(100) | — | — | NO | Copia del Nombre del legajo |

**Registros actuales:** 2.604

---

### 1.3 Fichadas — Registros de ingreso/egreso

| Campo | Tipo | PK | FK | Nullable | Notas |
|---|---|:---:|---|:---:|---|
| Id | int | ✓ | — | NO | Identity |
| SucursalId | int | — | → Sucursales.Id | NO | |
| Legajoid | int | — | → Legajos.Id | NO | |
| Registro | datetime2 | — | — | NO | Fecha y hora del movimiento |
| EntraSale | nvarchar(1) | — | — | NO | 'E' = Entrada, 'S' = Salida |

**Registros actuales:** 780.255

---

### 1.4 LegajosSucursales — Relación empleado-sucursal

| Campo | Tipo | PK | FK |
|---|---|:---:|---|
| LegajoId | int | ✓ | → Legajos.Id |
| SucursalId | int | ✓ | → Sucursales.Id |

Determina a qué sucursales pertenece cada empleado. Se inserta automáticamente en `EscritorioLegajoActualizar` al dar de alta un legajo nuevo.

---

### 1.5 Terminales

| Campo | Tipo | PK | FK | Nullable |
|---|---|:---:|---|:---:|
| Id | int | ✓ | — | NO |
| Nombre | nvarchar(100) | — | — | NO |
| Descripcion | nvarchar(max) | — | — | SÍ |
| SucursalId | int | — | → Sucursales.Id | NO |

**Registros actuales:** 39

---

### 1.6 Sucursales

| Campo | Tipo | PK |
|---|---|:---:|
| Id | int | ✓ |
| Nombre | nvarchar(100) | — |
| CodigoSucursal | nvarchar(5) | — |

**Registros actuales:** 30

---

## 2. Tablas de lookup

| Tabla | Campos | Registros |
|---|---|:---:|
| Sectores | Id (PK), Nombre | 28 |
| Categorias | Id (PK), Nombre | 11 |
| Horarios | Id (PK), Nombre | 8 |
| Dedos | Id (PK, sin identity), Nombre | 10 |
| Dias | Id (PK, sin identity), Nombre | — |

**Nota sobre Dedos:** PK sin identity — los valores están precargados (corresponden a los IDs del SDK DigitalPersona).

---

## 3. Tablas de sistema / web (no relevantes para Fichador/Administrador de escritorio)

| Tabla | Propósito |
|---|---|
| AspNetUsers, AspNetRoles, etc. | ASP.NET Identity — usuarios de la web |
| GRALUsuarios | Usuarios del escritorio (Id, Email, Password) |
| VariablesGlobales | Configuración global de la app web |
| Noticias | Módulo de novedades web |
| Feriados, Vacaciones, Incidencias, etc. | Módulo RRHH web |

---

## 4. Stored Procedures y Vista — Escritorio

### 4.1 `EscritorioFichadasSPSALIDA` — Registrar fichada

```sql
-- Parámetros:
@nSucursalID int
@nLegajoID   int
@dRegistro   datetime
@sAccion     varchar(1) OUTPUT   -- devuelve 'E' o 'S'

-- Lógica:
-- 1. Busca el último EntraSale de HOY para ese LegajoID
-- 2. Si no hay movimientos → inserta como 'E' (Entrada)
-- 3. Si el último fue 'E' → inserta como 'S' (Salida)
-- 4. Si el último fue 'S' → inserta como 'E' (Entrada)
-- 5. INSERT en Fichadas con el @sAccion resultante
```

**Usado por:** Fichador (FrmFichar.cs) — cada vez que se verifica una huella.

---

### 4.2 `EscritorioLegajoActualizar` — Alta/modificación de empleado

```sql
-- Parámetros:
@LegajoID               varchar(15)
@Nombre                 varchar(50)
@SectorId               int
@CategoriaId            int
@Activo                 bit
@HorarioID              int
@CalendarioPersonalizado bit
@nSucursalId            int

-- Lógica:
-- 1. Si NO existe en Legajos por LegajoID → INSERT + INSERT en LegajosSucursales
-- 2. Si existe → UPDATE (sin actualizar LegajosSucursales)
```

**Clave natural usada:** `Legajos.LegajoId` (string), NO el int `Id`.
**Usado por:** Administrador — formulario de alta/edición de personal.

---

### 4.3 `EscritorioLegajosHuellasActualizar` — Guardar huella

```sql
-- Parámetros:
@LegajoId      int
@DedoId        int
@Huella        varbinary(max)
@nFingerMask   int
@sLegajoNombre varchar(100)
@sLegajoID     varchar(15)

-- Lógica:
-- 1. DELETE de LegajosHuellas WHERE sLegajoID = @sLegajoID AND DedoId = @DedoId AND nFingerMask = @nFingerMask
-- 2. INSERT con todos los campos
```

**Observación:** el DELETE usa `sLegajoID` (string) pero el INSERT usa `@LegajoId` (int). Ambos campos deben ser consistentes.
**Usado por:** Administrador — formulario de captura de huella.

---

### 4.4 `RRHHLegajos_DeleteTodo` — Eliminar todo de un empleado

```sql
@legajo varchar(15)
-- DELETE LegajosHuellas WHERE sLegajoID = @legajo
-- DELETE Legajos WHERE LegajoID = @legajo
```

---

### 4.5 Vista `EscritorioLegajosHuellasView`

```sql
SELECT
    a.Id            AS nLegajoId,
    a.LegajoId      AS sLegajoID,
    a.Nombre        AS sApellido,
    a.Nombre        AS sNombre,
    a.Nombre        AS sLegajoNombre,
    c.id            AS nSector,
    a.CategoriaId   AS nCategoria,
    a.Activo        AS lActivo,
    e.Nombre        AS sHorarioID,
    b.DedoId        AS nDedo,
    b.huella        AS iHuella
FROM Legajos a
INNER JOIN LegajosHuellas b  ON a.id = b.LegajoId
INNER JOIN Sectores c        ON a.SectorId = c.Id
INNER JOIN Categorias d      ON a.CategoriaId = d.id
INNER JOIN Horarios e        ON a.HorarioId = e.Id    -- ⚠️ INNER JOIN sobre campo NULLABLE
```

**Usado por:** Fichador — carga TODAS las huellas en memoria al iniciar para el matching.

---

## 5. Riesgos y observaciones críticas

### ⚠️ RIESGO ALTO — HorarioId nullable + INNER JOIN en la vista

`Legajos.HorarioId` es nullable, pero `EscritorioLegajosHuellasView` usa `INNER JOIN` con `Horarios`. Cualquier empleado sin horario asignado **queda excluido de la vista** → su huella **no se carga en el Fichador** → el sistema no lo reconoce.

**Impacto:** silencioso. No hay error, simplemente no ficha.

---

### ⚠️ RIESGO MEDIO — Truncamiento en sLegajoId

`LegajosHuellas.sLegajoId` es `nvarchar(5)` pero `Legajos.LegajoId` es `nvarchar(25)`. Si algún código de legajo tiene más de 5 caracteres, se trunca al guardar la huella. El SP usa `varchar(15)` como parámetro de entrada, intermedio entre ambos.

---

### ℹ️ OBSERVACIÓN — Desnormalización intencional en LegajosHuellas

Los campos `sLegajoId` y `sLegajoNombre` son copias de datos de `Legajos`. Esta desnormalización es intencional para el SDK de DigitalPersona que necesita acceso directo a esos campos sin joins adicionales.

---

### ℹ️ OBSERVACIÓN — Clave natural vs. surrogate key

`Legajos` tiene dos identificadores:
- `Id` (int, identity) — clave técnica
- `LegajoId` (nvarchar 25) — código alfanumérico de negocio

Los SPs de escritorio usan `LegajoId` como clave de negocio para upserts. El Fichador usa `Id` (int) para las fichadas. El Administrador debe manejar ambos correctamente.

---

## 6. Diagrama de relaciones (tablas núcleo)

```
Sectores ──────┐
Categorias ────┤
Horarios ──────┤
               ↓
          [Legajos] ──── [LegajosSucursales] ──── [Sucursales]
               │                                        │
               ├──── [LegajosHuellas] ──── [Dedos]     │
               │                                        │
               └──── [Fichadas] ─────────────────────── ┘
                          ↑
                     [Terminales] → [Sucursales]
```

---

## 7. Resumen para Etapa 2

| Entidad | Tabla | SP de escritura | Vista de lectura |
|---|---|---|---|
| Empleado | Legajos + LegajosSucursales | EscritorioLegajoActualizar | — |
| Huella | LegajosHuellas | EscritorioLegajosHuellasActualizar | EscritorioLegajosHuellasView |
| Fichada | Fichadas | EscritorioFichadasSPSALIDA | — |
| Sucursal | Sucursales | (sin SP de escritorio) | — |
| Terminal | Terminales | (sin SP de escritorio) | — |
