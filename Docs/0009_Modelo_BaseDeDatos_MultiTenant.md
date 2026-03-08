# TASK 0009 - Modelo de Base de Datos MultiTenant

## Objetivo
Definir criterios de modelado para la nueva base `DigitalPlusMultiTenant`, que será construida con EF Core Migrations y deberá permitir migrar funcionalidad y datos desde la base actual `DigitalPlus`.

---

## 1. Principios obligatorios
La nueva base debe:

- ser nueva
- generarse con EF Core Migrations
- soportar multiempresa real
- respetar integridad referencial
- tener naming profesional
- contemplar auditoría
- facilitar migración desde la base actual

---

## 2. Entidad raíz
Debe existir una tabla principal:

## Empresa
Campos mínimos sugeridos:

- `Id`
- `Nombre`
- `NombreFantasia` si aplica
- `Codigo`
- `Cuit` o identificación fiscal si aplica
- `IsActive`
- `CreatedAt`
- `CreatedBy`
- `UpdatedAt`
- `UpdatedBy`

Todo lo demás debe colgar directa o indirectamente de Empresa.

---

## 3. Entidades funcionales mínimas sugeridas
Estas entidades deberán evaluarse y ajustarse según el portal actual, pero en principio la nueva base debe contemplar equivalentes claras para:

### Estructura organizativa
- `Sucursal`
- `Sector`
- `Categoria`
- `Horario`
- `Terminal`

### Personas / RRHH
- `Legajo`
- `LegajoHuella` si sigue siendo necesario en web o para referencia
- `Incidencia`
- `IncidenciaLegajo`
- `Vacacion` si hoy existe lógica equivalente

### Operación
- `Fichada`
- `Feriado`
- `Noticia` si sigue teniendo valor funcional
- `VariableSistema` si alguna configuración debe seguir existiendo

### Seguridad
- `ApplicationUser` con Identity
- tablas propias de Identity

---

## 4. Regla de tenancy
Toda tabla funcional que pertenezca a una empresa debe tener:

`EmpresaId`

Excepciones posibles:

- tablas puramente globales del sistema
- catálogos globales reales si los hubiera

Pero la regla general es que la información de negocio sea tenant-aware.

---

## 5. Reglas de relaciones
Ejemplos mínimos esperados:

- `Sucursal` → pertenece a `Empresa`
- `Sector` → pertenece a `Empresa`
- `Categoria` → pertenece a `Empresa`
- `Horario` → pertenece a `Empresa`
- `Terminal` → pertenece a `Empresa`
- `Legajo` → pertenece a `Empresa`
- `Legajo` → puede pertenecer a `Sucursal`, `Sector`, `Categoria`, `Horario`
- `Fichada` → pertenece a `Empresa`
- `Fichada` → pertenece a `Legajo`
- `Fichada` → puede referenciar `Terminal`
- `IncidenciaLegajo` → pertenece a `Legajo` e `Incidencia`
- `ApplicationUser` → pertenece a `Empresa`

---

## 6. Claves e índices
### Clave primaria
Usar:

`Id`

### Clave foránea
Usar:

`EmpresaId`
`LegajoId`
`SucursalId`
`SectorId`
`CategoriaId`
`HorarioId`
`TerminalId`

### Índices mínimos sugeridos
- índices por `EmpresaId`
- índices compuestos donde tenga sentido, por ejemplo:
  - `EmpresaId + NumeroLegajo`
  - `EmpresaId + FechaHora`
  - `EmpresaId + IsActive`

---

## 7. Auditoría
Agregar auditoría a tablas importantes.

Campos sugeridos:

- `CreatedAt`
- `CreatedBy`
- `UpdatedAt`
- `UpdatedBy`
- `IsActive`

No hace falta meter auditoría barroca en cada baldosa del sistema, pero sí en todo lo importante.

---

## 8. Convenciones de nombres
Definir y aplicar una convención única.

Sugerencia:

- entidades en singular dentro del modelo
- tablas en singular o plural, pero consistente
- nombres en PascalCase si EF lo resuelve bien
- evitar prefijos legacy tipo `RRHH`, `GRAL` salvo que exista una razón concreta para sostenerlos

Ejemplo de preferencia:

- `Empresa`
- `Sucursal`
- `Legajo`
- `Fichada`
- `Horario`

En vez de:

- `RRHHLegajos`
- `GRALSucursales`
- etc.

La base nueva no tiene por qué seguir arrastrando nombres viejos si se puede dejar profesional.

---

## 9. Identity
La nueva base debe integrar Identity correctamente.

La entidad de usuario debe contemplar, como mínimo:

- `EmpresaId`
- `NombreCompleto` si aplica
- `IsActive`

Debe analizarse cómo mapear los usuarios actuales del portal actual a esta nueva estructura.

---

## 10. Migración de datos desde DigitalPlus
Debe diseñarse un proceso de migración controlado.

Fases sugeridas:

### Fase A
Relevar tablas origen en `DigitalPlus`.

### Fase B
Mapear equivalencias entre tabla origen y tabla destino.

### Fase C
Definir transformaciones necesarias:

- renombre de campos
- normalización
- limpieza de datos
- conversión de tipos
- relaciones nuevas

### Fase D
Insertar primero datos maestros:

- empresa
- sucursales
- sectores
- categorías
- horarios
- terminales
- incidencias

### Fase E
Insertar datos operativos:

- legajos
- fichadas
- relaciones auxiliares

### Fase F
Migrar usuarios

### Fase G
Validar resultados funcionales

---

## 11. Empresa inicial: Kosiuko
La primera empresa de la nueva base debe ser:

`Kosiuko`

Todos los datos migrados desde la base actual productiva deben quedar correctamente asociados a esa empresa mediante `EmpresaId`.

---

## 12. Validaciones obligatorias post-migración
Se deben comparar, como mínimo:

- cantidad de legajos
- cantidad de fichadas
- sucursales
- sectores
- categorías
- horarios
- usuarios migrados

Además se debe validar navegación funcional en el portal nuevo.

---

## 13. Entregables de esta tarea
Claude debe entregar:

1. diagrama de entidades
2. listado de tablas destino
3. relaciones
4. índices sugeridos
5. estrategia de migración tabla a tabla
6. primer set de migraciones EF

---

## 14. Criterio final
La nueva base no debe ser una copia fea de la vieja. Debe ser una evolución profesional que permita mantener, escalar y vender el producto sin seguir apilando deuda técnica como si fuera un hobby destructivo.
