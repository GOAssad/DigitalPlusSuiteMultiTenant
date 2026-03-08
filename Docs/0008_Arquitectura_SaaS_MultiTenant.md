# TASK 0008 - Arquitectura SaaS MultiTenant

## Objetivo
Definir la arquitectura objetivo que la nueva solución `DigitalPlusMultiTenant` debe respetar.

Este documento no es opcional. Sirve para evitar decisiones improvisadas durante la implementación.

---

## 1. Estilo arquitectónico
La solución debe seguir una arquitectura limpia, pragmática y mantenible.

No se busca sobreingeniería. Tampoco se acepta una estructura desordenada.

Se propone una solución con capas claras:

- **UI / Blazor**
- **Application / Services**
- **Domain**
- **Infrastructure**
- **Persistence**

### Estructura sugerida

- `DigitalPlusMultiTenant.Web`
- `DigitalPlusMultiTenant.Application`
- `DigitalPlusMultiTenant.Domain`
- `DigitalPlusMultiTenant.Infrastructure`
- `DigitalPlusMultiTenant.Persistence`

Si se considera mejor una variante levemente distinta, justificarla. Pero debe conservar separación real de responsabilidades.

---

## 2. Patrón de tenancy
Se define tenancy por columna:

`EmpresaId`

No se debe crear una base por cliente.
No se debe crear un deploy por cliente.
No se debe usar schema por cliente.

La solución debe ser una sola aplicación y una sola base multiempresa.

---

## 3. Tenant resolution
Debe definirse desde el inicio cómo se identifica la empresa activa.

Se permite resolver el tenant por alguno de estos medios:

1. por usuario autenticado y su empresa asociada
2. por subdominio en una etapa futura
3. por claim o contexto de sesión

Para esta primera etapa, la resolución mínima obligatoria es:

- usuario autenticado
- usuario asociado a una empresa
- filtrado de datos por empresa en toda la aplicación

---

## 4. Reglas de aislamiento
Toda operación funcional debe ejecutarse dentro del contexto de una empresa.

Esto implica:

- queries filtradas por `EmpresaId`
- altas vinculadas a la empresa activa
- edición solo sobre datos de la empresa activa
- eliminación restringida a datos de la empresa activa

Debe evitarse por diseño cualquier fuga de datos entre empresas.

Si se usa EF Core, se recomienda evaluar:

- global query filters para entidades tenant-aware
- servicios que inyecten el contexto de empresa activa

---

## 5. Identity
ASP.NET Identity es obligatorio.

Se recomienda:

- extender `IdentityUser` o usar entidad propia derivada
- agregar `EmpresaId` al usuario
- contemplar roles globales y roles por empresa si hiciera falta

Modelo mínimo sugerido:

- `SuperAdmin` → acceso global
- `AdminEmpresa` → administración de su empresa
- `UsuarioEmpresa` → acceso operativo limitado

Si el sistema actual ya tiene usuarios y roles, se deben mapear hacia esta estructura.

---

## 6. Persistencia
### EF Core
Debe ser el mecanismo principal para:

- modelo de datos
- migraciones
- relaciones
- persistencia estándar

### Dapper
Debe usarse solo en casos donde claramente aporte valor:

- dashboards con agregaciones
- listados complejos
- reportes de lectura intensiva
- consultas que con EF resulten excesivamente pesadas o poco controlables

No usar Dapper como excusa para romper consistencia arquitectónica.

---

## 7. Auditoría
Todas las entidades importantes deben contemplar auditoría.

Campos sugeridos:

- `CreatedAt`
- `CreatedBy`
- `UpdatedAt`
- `UpdatedBy`
- `IsActive` cuando aplique

En entidades sensibles también puede evaluarse:

- `DeletedAt`
- `DeletedBy`
- soft delete

---

## 8. Naming profesional
Se deben evitar nombres ambiguos, crípticos o heredados de baja calidad.

Reglas:

- nombres de tablas en singular o plural, pero consistentes
- nombres de columnas claros
- PK estándar `Id`
- FK estándar `EntidadId`
- fechas con nombres explícitos
- booleanos con prefijos comprensibles (`Is`, `Can`, `Has`, `Requires`)

No trasladar ciegamente nombres legacy si están mal diseñados.

---

## 9. Integridad referencial
La base nueva debe tener integridad referencial real.

Esto implica:

- claves foráneas explícitas
- restricciones consistentes
- índices en claves foráneas relevantes
- relaciones bien definidas en EF
- políticas de delete coherentes

Evitar cascadas destructivas donde comprometan historial.

---

## 10. UI y consistencia visual
La nueva aplicación debe tener una línea visual única.

Debe definirse una base de componentes reutilizables para:

- tablas paginadas
- filtros
- formularios
- tarjetas KPI
- alertas
- diálogos
- encabezados de módulo

No más pantallas huérfanas con estilos distintos. Bastante tuvimos ya con ese deporte corporativo.

---

## 11. Dashboard
Debe existir un dashboard inicial atractivo, útil y comercializable.

Debe mostrar, al menos:

- total de legajos activos
- fichadas del día
- llegadas tarde
- ausencias si la lógica ya existe
- sucursales activas
- terminales activas

Si el origen de datos para ciertos KPI no está limpio aún, dejar la estructura preparada y documentar qué falta.

---

## 12. Logging y trazabilidad
La nueva solución debería contemplar logging estructurado desde el comienzo.

Sugerencia:

- logging de errores
- logging de autenticación
- logging de acciones administrativas relevantes
- logging de procesos de migración

---

## 13. Seguridad
La solución debe contemplar desde el inicio:

- autenticación robusta con Identity
- autorización por roles
- aislamiento por tenant
- protección contra acceso cruzado entre empresas
- no exponer datos de una empresa en consultas o rutas de otra

---

## 14. Decisiones no permitidas
No implementar:

- un deploy separado por empresa
- una base separada por empresa para esta etapa
- lógica multi-tenant improvisada en cada pantalla
- acceso a datos sin contemplar tenant
- naming heredado sin criterio

---

## 15. Resultado esperado
La arquitectura final debe dejar preparado el sistema para crecer como producto SaaS real y no como una colección de parches con login bonito.
