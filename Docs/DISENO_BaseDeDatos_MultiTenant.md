# Diseno de Base de Datos - DigitalPlusMultiTenant

## 1. Principios de diseno

- **Una sola base**: `DigitalPlusMultiTenant` - no se crean bases por cliente
- **Tenant por columna**: toda tabla funcional lleva `EmpresaId` (int, FK a Empresa)
- **EF Core Migrations**: la base se genera y evoluciona con migraciones
- **Naming profesional**: PascalCase, singular, sin prefijos legacy (RRHH, GRAL)
- **Auditoria**: CreatedAt, CreatedBy, UpdatedAt, UpdatedBy en entidades importantes
- **Soft delete**: IsActive (bool) en entidades que lo requieran
- **Identity integrado**: ASP.NET Identity con EmpresaId en el usuario

---

## 2. Entidad raiz: Empresa

```
Empresa
  Id                  int PK IDENTITY
  Codigo              nvarchar(20) UNIQUE       -- codigo corto (ej: "KSK")
  Nombre              nvarchar(200) NOT NULL
  NombreFantasia      nvarchar(200) NULL
  Cuit                nvarchar(13) NULL
  IsActive            bit NOT NULL DEFAULT 1
  CreatedAt           datetime2 NOT NULL
  CreatedBy           nvarchar(100) NULL
  UpdatedAt           datetime2 NULL
  UpdatedBy           nvarchar(100) NULL
```

Todo lo demas cuelga directa o indirectamente de Empresa.

---

## 3. Mapeo completo: tabla actual -> entidad nueva

### 3.1 Estructura organizativa

#### Sucursal (antes: Sucursales)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| Nombre | Nombre | nvarchar(100) NOT NULL | |
| CodigoSucursal | Codigo | nvarchar(10) NOT NULL | renombrado |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | IsActive | bit DEFAULT 1 | |
| *(nuevo)* | CreatedAt | datetime2 | auditoria |
| *(nuevo)* | CreatedBy | nvarchar(100) | |
| *(nuevo)* | UpdatedAt | datetime2 | |
| *(nuevo)* | UpdatedBy | nvarchar(100) | |

**Indice unico**: (EmpresaId, Codigo)
**Indice unico**: (EmpresaId, Nombre)

---

#### Sector (antes: Sectores)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| Nombre | Nombre | nvarchar(100) NOT NULL | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | IsActive | bit DEFAULT 1 | |
| *(nuevo)* | CreatedAt/CreatedBy/UpdatedAt/UpdatedBy | | auditoria |

**Indice unico**: (EmpresaId, Nombre)

---

#### Categoria (antes: Categorias)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| Nombre | Nombre | nvarchar(100) NOT NULL | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | IsActive | bit DEFAULT 1 | |
| *(nuevo)* | CreatedAt/CreatedBy/UpdatedAt/UpdatedBy | | auditoria |

**Indice unico**: (EmpresaId, Nombre)

---

#### Horario (antes: Horarios)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| Nombre | Nombre | nvarchar(100) NOT NULL | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | IsActive | bit DEFAULT 1 | |
| *(nuevo)* | CreatedAt/CreatedBy/UpdatedAt/UpdatedBy | | auditoria |

**Indice unico**: (EmpresaId, Nombre)

---

#### HorarioDetalle (antes: HorariosDias)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| HorarioId | HorarioId | int FK NOT NULL | |
| DiaId | DiaSemana | int NOT NULL | 1-7, ya no FK a tabla Dias |
| HoraDesde | HoraDesde | time NOT NULL | cambio de int a time |
| HoraHasta | HoraHasta | time NOT NULL | cambio de int a time |
| MinutoDesde | *(absorbido en HoraDesde)* | | |
| MinutoHasta | *(absorbido en HoraHasta)* | | |
| Cerrado | IsCerrado | bit NOT NULL | renombrado |

**Nota**: HoraDesde/HoraHasta cambian de (int hora + int minuto) a `time(0)`. Mas limpio.
**No lleva EmpresaId propio** - hereda tenant via Horario.

---

#### Terminal (antes: Terminales)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| Nombre | Nombre | nvarchar(100) NOT NULL | |
| Descripcion | Descripcion | nvarchar(500) NULL | |
| SucursalId | SucursalId | int FK NOT NULL | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant (redundante pero necesario para filtros eficientes) |
| *(nuevo)* | IsActive | bit DEFAULT 1 | |
| *(nuevo)* | CreatedAt/CreatedBy/UpdatedAt/UpdatedBy | | auditoria |

**Indice unico**: (EmpresaId, Nombre)

---

### 3.2 Personas / RRHH

#### Legajo (antes: Legajos)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| LegajoId (nvarchar) | NumeroLegajo | nvarchar(25) NOT NULL | renombrado, mas claro |
| Nombre | Apellido | nvarchar(100) NOT NULL | separado (hoy es "Apellido, Nombre") |
| *(extraido)* | Nombre | nvarchar(100) NOT NULL | separado |
| SectorId | SectorId | int FK NOT NULL | |
| CategoriaId | CategoriaId | int FK NOT NULL | |
| HorarioId | HorarioId | int FK NULL | |
| Activo | IsActive | bit NOT NULL DEFAULT 1 | renombrado |
| CalendarioPersonalizado | HasCalendarioPersonalizado | bit NOT NULL DEFAULT 0 | renombrado |
| Foto | Foto | varbinary(max) NULL | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | Email | nvarchar(200) NULL | para futuro autoservicio |
| *(nuevo)* | Telefono | nvarchar(50) NULL | |
| *(nuevo)* | FechaIngreso | date NULL | |
| *(nuevo)* | FechaEgreso | date NULL | |
| *(nuevo)* | CreatedAt/CreatedBy/UpdatedAt/UpdatedBy | | auditoria |

**Indice unico**: (EmpresaId, NumeroLegajo)
**Indices**: EmpresaId+IsActive, EmpresaId+SectorId, EmpresaId+CategoriaId

---

#### LegajoSucursal (antes: LegajosSucursales)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| LegajoId | LegajoId | int FK NOT NULL | |
| SucursalId | SucursalId | int FK NOT NULL | |

**PK compuesta**: (LegajoId, SucursalId)
**No lleva EmpresaId propio** - hereda de Legajo y Sucursal.

---

#### LegajoHuella (antes: LegajosHuellas)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| LegajoId | LegajoId | int FK NOT NULL | ahora es FK a Legajo.Id |
| DedoId | DedoId | int NOT NULL | 1-10, ya no FK a tabla Dedos |
| sLegajoId | *(eliminado)* | | redundante, se usa LegajoId |
| huella | Huella | varbinary(max) NOT NULL | |
| nFingerMask | FingerMask | int NOT NULL | renombrado |
| sLegajoNombre | *(eliminado)* | | redundante, se navega a Legajo |

**PK compuesta**: (LegajoId, DedoId)

---

#### LegajoPin (antes: campos en LegajosPin via SPs)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| *(nuevo)* | Id | int PK IDENTITY | |
| *(de SP)* | LegajoId | int FK UNIQUE NOT NULL | 1:1 con Legajo |
| *(de SP)* | PinHash | nvarchar(100) NOT NULL | |
| *(de SP)* | PinSalt | nvarchar(100) NOT NULL | |
| *(de SP)* | PinMustChange | bit DEFAULT 0 | |
| *(de SP)* | PinChangedAt | datetime2 NULL | |
| *(nuevo)* | CreatedAt | datetime2 | |

---

#### LegajoDomicilio (antes: RRHHLegajosDomicilios - no existe en DB actual)
| Campo nuevo | Tipo | Notas |
|---|---|---|
| Id | int PK IDENTITY | |
| LegajoId | int FK UNIQUE NOT NULL | 1:1 con Legajo |
| Calle | nvarchar(200) NULL | |
| Altura | nvarchar(20) NULL | |
| Piso | nvarchar(20) NULL | |
| Barrio | nvarchar(100) NULL | |
| Localidad | nvarchar(100) NULL | |
| Provincia | nvarchar(100) NULL | |
| CodigoPostal | nvarchar(10) NULL | nuevo |
| PaisId | int NULL | |

**Nota**: esta tabla no existe en la BD actual (la clase devuelve vacio). Se crea nueva.

---

### 3.3 Operacion

#### Fichada (antes: Fichadas)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| SucursalId | SucursalId | int FK NOT NULL | |
| Legajoid | LegajoId | int FK NOT NULL | corregido naming |
| Registro | FechaHora | datetime2 NOT NULL | renombrado |
| EntraSale | Tipo | nvarchar(1) NOT NULL | 'E'/'S', renombrado |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | TerminalId | int FK NULL | ahora se sabe de que terminal vino |
| *(nuevo)* | Origen | nvarchar(20) NULL | 'Huella','PIN','Demo','Manual','Web' |
| *(nuevo)* | CreatedAt | datetime2 | |

**Indices**: (EmpresaId, FechaHora), (EmpresaId, LegajoId, FechaHora), SucursalId

---

#### Feriado (antes: Feriados)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| Nombre | Nombre | nvarchar(100) NOT NULL | |
| Fecha | Fecha | date NOT NULL | cambia de datetime2 a date |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |

**Indice unico**: (EmpresaId, Fecha)

---

#### Incidencia (antes: Incidencias)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| Nombre | Nombre | nvarchar(100) NOT NULL | |
| Color | Color | nvarchar(15) NULL | |
| Abreviatura | Abreviatura | nvarchar(4) NULL | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | IsActive | bit DEFAULT 1 | |

**Indice unico**: (EmpresaId, Nombre)

---

#### IncidenciaLegajo (antes: IncidenciasLegajos)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| IncidenciaId | IncidenciaId | int FK NOT NULL | |
| LegajoId | LegajoId | int FK NOT NULL | |
| Fecha | Fecha | date NOT NULL | cambia a date |
| Detalle | Detalle | nvarchar(500) NULL | limita largo |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | CreatedAt/CreatedBy | | auditoria |

---

#### Vacacion (antes: Vacaciones)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| LegajoId | LegajoId | int FK NOT NULL | |
| FechaDesde | FechaDesde | date NOT NULL | cambia a date |
| FechaHasta | FechaHasta | date NOT NULL | cambia a date |
| Nota | Nota | nvarchar(500) NULL | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | CreatedAt/CreatedBy | | auditoria |

---

#### EventoCalendario (antes: HorariosDiasEventos)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| FechaDesde | FechaDesde | date NOT NULL | |
| FechaHasta | FechaHasta | date NOT NULL | |
| LegajoId | LegajoId | int FK NOT NULL | |
| Nota | Nota | nvarchar(500) NULL | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |

---

#### Noticia (antes: Noticias)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| Nombre | Titulo | nvarchar(200) NOT NULL | renombrado |
| Detalle | Contenido | nvarchar(max) NULL | renombrado |
| FechaDesde | FechaDesde | date NOT NULL | |
| FechaHasta | FechaHasta | date NOT NULL | |
| Privado | IsPrivada | bit DEFAULT 0 | renombrado |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | CreatedAt/CreatedBy | | auditoria |

---

#### VariableSistema (antes: VariablesGlobales)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| Id | Id | int PK IDENTITY | |
| sId | *(eliminado)* | | no se usa |
| Nombre | Clave | nvarchar(100) NOT NULL | renombrado |
| Detalle | Descripcion | nvarchar(500) NULL | renombrado |
| TipoValor | TipoValor | nvarchar(50) NULL | |
| Valor | Valor | nvarchar(500) NULL | |
| Reiniciar | RequiereReinicio | bit DEFAULT 0 | renombrado |
| *(nuevo)* | EmpresaId | int FK NULL | NULL = global del sistema |

**Indice unico**: (EmpresaId, Clave) -- permite config global (EmpresaId=NULL) y por empresa

---

### 3.4 Seguridad / Identity

#### ApplicationUser (extiende IdentityUser)
| Campo actual (AspNetUsers) | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| *(Identity estandar)* | *(todos los campos Identity)* | | |
| *(nuevo)* | EmpresaId | int FK NOT NULL | tenant |
| *(nuevo)* | NombreCompleto | nvarchar(200) NULL | |
| *(nuevo)* | IsActive | bit DEFAULT 1 | |
| *(nuevo)* | CreatedAt | datetime2 | |

#### ApplicationRole (extiende IdentityRole)
Roles predefinidos:
- `SuperAdmin` - acceso total cross-tenant
- `AdminEmpresa` - administra su empresa
- `Operador` - operacion diaria (fichadas, reportes)
- `Consulta` - solo lectura

#### UsuarioSucursal (antes: UsuariosSucursales)
| Campo actual | Campo nuevo | Tipo | Notas |
|---|---|---|---|
| UsuarioId | UserId | nvarchar(450) FK | FK a ApplicationUser |
| SucursalId | SucursalId | int FK | |

**PK compuesta**: (UserId, SucursalId)

---

### 3.5 Tablas eliminadas

| Tabla actual | Motivo |
|---|---|
| Dedos | Catalogo fijo de 10 valores -> enum en codigo |
| Dias | Catalogo fijo de 7 valores -> enum en codigo |
| GRALUsuarios | Legacy, reemplazado por Identity |

---

## 4. Diagrama de relaciones

```
Empresa (raiz)
  |
  +-- Sucursal          (EmpresaId)
  |     +-- Terminal     (SucursalId, EmpresaId)
  |
  +-- Sector             (EmpresaId)
  +-- Categoria           (EmpresaId)
  +-- Horario             (EmpresaId)
  |     +-- HorarioDetalle (HorarioId) -- hereda tenant
  |
  +-- Legajo              (EmpresaId, SectorId, CategoriaId, HorarioId?)
  |     +-- LegajoSucursal  (LegajoId, SucursalId) -- N:N
  |     +-- LegajoHuella    (LegajoId, DedoId)
  |     +-- LegajoPin       (LegajoId) -- 1:1
  |     +-- LegajoDomicilio (LegajoId) -- 1:1
  |     +-- Vacacion         (LegajoId, EmpresaId)
  |     +-- EventoCalendario (LegajoId, EmpresaId)
  |     +-- IncidenciaLegajo (LegajoId, IncidenciaId, EmpresaId)
  |
  +-- Fichada             (EmpresaId, LegajoId, SucursalId, TerminalId?)
  +-- Incidencia          (EmpresaId)
  +-- Feriado             (EmpresaId)
  +-- Noticia             (EmpresaId)
  +-- VariableSistema     (EmpresaId nullable)
  |
  +-- ApplicationUser     (EmpresaId)
        +-- UsuarioSucursal (UserId, SucursalId)
```

---

## 5. Estrategia de tenant isolation en EF Core

### 5.1 Interface base
```csharp
public interface ITenantEntity
{
    int EmpresaId { get; set; }
}
```

### 5.2 Clase base con auditoria
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

public abstract class TenantEntity : AuditableEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; } = null!;
}
```

### 5.3 Global query filters
```csharp
// En el DbContext:
protected override void OnModelCreating(ModelBuilder builder)
{
    // Para cada entidad que implemente ITenantEntity:
    builder.Entity<Sucursal>().HasQueryFilter(e => e.EmpresaId == _tenantId);
    builder.Entity<Legajo>().HasQueryFilter(e => e.EmpresaId == _tenantId);
    builder.Entity<Fichada>().HasQueryFilter(e => e.EmpresaId == _tenantId);
    // ... etc para todas las entidades tenant-aware
}
```

### 5.4 Auto-set EmpresaId en SaveChanges
```csharp
public override Task<int> SaveChangesAsync(...)
{
    foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
    {
        if (entry.State == EntityState.Added)
            entry.Entity.EmpresaId = _tenantId;
    }
    // + auditoria (CreatedAt, UpdatedAt, etc.)
    return base.SaveChangesAsync(...);
}
```

---

## 6. Indices recomendados (resumen)

| Entidad | Indice | Tipo |
|---|---|---|
| Empresa | Codigo | UNIQUE |
| Sucursal | (EmpresaId, Codigo) | UNIQUE |
| Sucursal | (EmpresaId, Nombre) | UNIQUE |
| Sector | (EmpresaId, Nombre) | UNIQUE |
| Categoria | (EmpresaId, Nombre) | UNIQUE |
| Horario | (EmpresaId, Nombre) | UNIQUE |
| Terminal | (EmpresaId, Nombre) | UNIQUE |
| Legajo | (EmpresaId, NumeroLegajo) | UNIQUE |
| Legajo | (EmpresaId, IsActive) | NONCLUSTERED |
| Fichada | (EmpresaId, FechaHora) | NONCLUSTERED |
| Fichada | (EmpresaId, LegajoId, FechaHora) | NONCLUSTERED |
| Feriado | (EmpresaId, Fecha) | UNIQUE |
| Incidencia | (EmpresaId, Nombre) | UNIQUE |
| VariableSistema | (EmpresaId, Clave) | UNIQUE |
| ApplicationUser | (EmpresaId) | NONCLUSTERED |

---

## 7. Estrategia de migracion de datos (Kosiuko)

### Orden de insercion (respetar dependencias FK)

```
1. Empresa         -> INSERT Kosiuko (Id=1)
2. Sucursal        -> FROM Sucursales + EmpresaId=1
3. Sector          -> FROM Sectores + EmpresaId=1
4. Categoria       -> FROM Categorias + EmpresaId=1
5. Horario         -> FROM Horarios + EmpresaId=1
6. HorarioDetalle  -> FROM HorariosDias (convertir hora/minuto a time)
7. Terminal        -> FROM Terminales + EmpresaId=1
8. Incidencia      -> FROM Incidencias + EmpresaId=1
9. Feriado         -> FROM Feriados + EmpresaId=1
10. Legajo         -> FROM Legajos + EmpresaId=1 (separar Nombre en Apellido+Nombre)
11. LegajoSucursal -> FROM LegajosSucursales
12. LegajoHuella   -> FROM LegajosHuellas (eliminar campos redundantes)
13. LegajoPin      -> FROM LegajosPin (si existe tabla fisica)
14. Fichada        -> FROM Fichadas + EmpresaId=1, Origen='Huella'
15. IncidenciaLeg. -> FROM IncidenciasLegajos + EmpresaId=1
16. Vacacion       -> FROM Vacaciones + EmpresaId=1
17. EventoCalend.  -> FROM HorariosDiasEventos + EmpresaId=1
18. Noticia        -> FROM Noticias + EmpresaId=1
19. VariableSist.  -> FROM VariablesGlobales + EmpresaId=1
20. ApplicationUser -> FROM AspNetUsers + EmpresaId=1
21. UsuarioSucurs. -> FROM UsuariosSucursales
```

### Transformaciones clave
- **Legajo.Nombre** -> separar por ", " en Apellido y Nombre
- **HorariosDias** -> (HoraDesde int, MinutoDesde int) -> time(0) `HH:mm`
- **Fichadas.EntraSale** -> se mantiene como Tipo ('E'/'S')
- **Fichadas** -> agregar Origen='Huella' (historico), las nuevas podran ser 'PIN','Demo','Web','Manual'
- **Dedos/Dias** -> no se migran, se usan enums en codigo

### Validaciones post-migracion
```sql
-- Comparar conteos:
SELECT 'Legajos' AS Tabla, COUNT(*) FROM Legajos             -- vieja
SELECT 'Legajo'  AS Tabla, COUNT(*) FROM Legajo WHERE EmpresaId=1  -- nueva
-- Repetir para: Fichada, Sucursal, Sector, Categoria, Horario, Terminal, etc.
```

---

## 8. Stored Procedures

**Criterio**: en la nueva solucion NO se usan stored procedures. Toda la logica se mueve a servicios de Application layer con EF Core.

Los SPs actuales y su destino:

| SP actual | Reemplazo |
|---|---|
| EscritorioFichadasSPSALIDA | FichadaService.RegistrarFichada() - logica E/S en C# |
| EscritorioLegajoActualizar | LegajoService.CrearOActualizar() |
| EscritorioLegajosHuellasActualizar | LegajoHuellaService.Guardar() |
| RRHHLegajos_DeleteTodo | LegajoService.Eliminar() - cascade en EF |
| EscritorioLegajoPIN_Verificar | PinService.Verificar() |
| EscritorioLegajoPIN_Cambiar | PinService.Cambiar() |
| EscritorioLegajosActivos_Lista | LegajoService.ListarActivos() - query EF |
| RRHHFichadas_SP_ELIMINAR | FichadaService.Eliminar() |
| RRHHFichadas_SP_MANUAL | FichadaService.RegistrarManual() |
| RRHHFichadas_SP_MANUAL_SELECT | FichadaService.Listar() con filtros |

Las funciones SQL (ConvertiraFecha, DiaDeSemana, PrimeraEntrada, UltimaSalida) se reemplazan por logica C#/LINQ.

---

## 9. Vista actual

| Vista actual | Reemplazo |
|---|---|
| EscritorioLegajosHuellasView | Query EF con Include/Join en LegajoService |

---

## 10. Resumen de entidades nuevas (20 tablas)

| # | Entidad | Tiene EmpresaId | Auditoria | Notas |
|---|---|---|---|---|
| 1 | Empresa | - (es la raiz) | Si | |
| 2 | Sucursal | Si | Si | |
| 3 | Sector | Si | Si | |
| 4 | Categoria | Si | Si | |
| 5 | Horario | Si | Si | |
| 6 | HorarioDetalle | No (hereda) | No | hijo de Horario |
| 7 | Terminal | Si | Si | |
| 8 | Legajo | Si | Si | Apellido+Nombre separados |
| 9 | LegajoSucursal | No (N:N) | No | join table |
| 10 | LegajoHuella | No (hereda) | No | datos biometricos |
| 11 | LegajoPin | No (hereda) | Parcial | hash+salt |
| 12 | LegajoDomicilio | No (hereda) | No | nueva, no existia en DB |
| 13 | Fichada | Si | Parcial | tabla mas grande |
| 14 | Incidencia | Si | No | catalogo |
| 15 | IncidenciaLegajo | Si | Parcial | |
| 16 | Vacacion | Si | Parcial | |
| 17 | EventoCalendario | Si | No | |
| 18 | Feriado | Si | No | |
| 19 | Noticia | Si | Parcial | |
| 20 | VariableSistema | Si (nullable) | No | NULL=global |
| + | ApplicationUser | Si | Parcial | extiende IdentityUser |
| + | UsuarioSucursal | No (N:N) | No | join table |
| + | Tablas Identity | No | No | gestionadas por Identity |
