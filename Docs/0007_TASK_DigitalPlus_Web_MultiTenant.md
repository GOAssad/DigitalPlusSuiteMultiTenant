# TASK 0007 - DigitalPlus Web MultiTenant

## Objetivo general
Crear una nueva versión del portal web **DigitalPlus** que soporte **multiempresa (multi-tenant)** y que permita reemplazar progresivamente el portal actual por una única solución web más moderna, más mantenible y más profesional.

El objetivo de negocio es evitar tener múltiples deploys separados por cliente y pasar a una arquitectura centralizada de tipo SaaS, con una sola aplicación capaz de administrar múltiples empresas.

---

## 1. Punto de partida obligatorio
Debes comenzar realizando un análisis técnico completo del portal web actual ubicado en:

`C:\Apps\Claude\Huellas\DigitalPlusWeb_Claude`

Debes relevar como mínimo:

- arquitectura actual
- estructura de la solución
- uso de EF Core
- uso de Dapper
- uso de Identity
- entidades de dominio
- repositorios
- servicios
- layout y componentes visuales
- páginas existentes
- módulos funcionales
- dependencias técnicas
- forma actual de conexión a base de datos
- naming actual de tablas y columnas

**No debes comenzar la nueva implementación sin antes entender completamente el sistema actual.**

---

## 2. Tecnología objetivo
La nueva solución debe construirse con:

- **Blazor Server**
- **.NET 10**
- **Entity Framework Core**
- **ASP.NET Identity**

Dapper podrá usarse solamente cuando realmente aporte valor para:

- consultas complejas
- listados pesados
- dashboards con agregaciones
- queries optimizadas de lectura

La regla general debe ser:

- **EF Core para modelo y persistencia principal**
- **Dapper solo donde sea claramente conveniente**

---

## 3. Estrategia Multi-Tenant obligatoria
La solución debe implementarse como:

**Single Database Multi-Tenant**

Es decir:

- una única base de datos
- múltiples empresas dentro de la misma base
- aislamiento lógico por empresa

Debe existir una entidad principal:

`Empresa`

Todas las entidades funcionales que correspondan deben incluir:

`EmpresaId`

Esto incluye, entre otras:

- Sucursales
- Sectores
- Categorías
- Horarios
- Legajos
- Fichadas
- Incidencias
- Terminales
- Feriados por empresa si aplica
- Noticias por empresa si aplica
- Usuarios y relaciones de acceso

Debe existir integridad referencial real entre tablas.

---

## 4. Alcance funcional mínimo a migrar
La nueva solución debe contemplar, al menos, el equivalente funcional del portal actual.

Se debe analizar la versión productiva actual y reproducir como mínimo las áreas principales que ya existen hoy.

Tomar como referencia el comportamiento actual del portal para conservar:

- legajos
- fichadas
- horarios
- categorías
- sectores
- sucursales
- terminales
- incidencias
- vacaciones si ya existen
- gestión de usuarios
- variables necesarias del sistema

No se trata de copiar el sistema viejo tal cual. Se trata de **conservar la funcionalidad útil y reconstruirla profesionalmente**.

---

## 5. Base de datos nueva
La nueva base debe ser una base completamente nueva, creada por **Entity Framework Migrations**.

Debe cumplir con estas reglas:

- nombres profesionales y consistentes
- integridad referencial
- claves foráneas explícitas
- índices razonables
- auditoría
- soporte real para multiempresa
- eliminación de estructuras heredadas pobres o ambiguas

La base actual que sirve como origen funcional es:

`DigitalPlus` en Ferozo

Debes estudiar esa estructura y diseñar sus equivalentes en la nueva base.

---

## 6. Empresa inicial a crear y migrar
La primera empresa a cargar en la nueva base será:

`Kosiuko`

Esa empresa es la que hoy está funcionando en producción con el portal actual.

Se debe:

1. crear la empresa Kosiuko en la nueva base
2. migrar los datos actuales desde la base actual a las nuevas tablas equivalentes
3. validar que el resultado funcional obtenido sea como mínimo equivalente al portal actual

La meta no es una migración cosmética. La meta es que la nueva solución reproduzca el resultado del sistema actual, pero con mejor arquitectura.

---

## 7. Usuarios, autenticación y autorización
La nueva solución debe utilizar **ASP.NET Identity**.

También se deben contemplar los usuarios ya existentes en el portal actual.

Se debe:

- relevar cómo están almacenados actualmente
- definir estrategia de migración
- llevarlos a Identity en la nueva solución
- vincular cada usuario a su empresa
- garantizar que un usuario solo vea información de su empresa, salvo perfiles administrativos globales si se definen

Debe quedar resuelto el modelo de:

- autenticación
- autorización
- aislamiento por tenant
- administración de usuarios por empresa

---

## 8. Mejora visual y de UX
El portal nuevo no debe limitarse a “funcionar”. Debe verse claramente más profesional que el actual.

Se requiere:

- dashboard atractivo
- KPIs útiles
- listados paginados
- filtros consistentes
- diseño homogéneo
- navegación clara
- mismo estilo visual en todas las pantallas
- componentes reutilizables

El sistema debe dejar de parecer una suma de pantallas aisladas.

Debe sentirse como un producto único, prolijo y comercializable.

---

## 9. Entorno de trabajo durante desarrollo
Mientras esta migración esté en desarrollo:

- la producción actual seguirá funcionando en Ferozo
- el desarrollo se hará en entorno local
- en modo debug se trabajará contra bases locales

Deben existir localmente estas bases:

- `DigitalPlus` → copia local de la base productiva de Ferozo
- `DigitalPlusAdmin` → copia o base local administrativa si hace falta referencia
- `DigitalPlusMultiTenant` → nueva base con estructura nueva

La migración de datos debe probarse primero localmente.

---

## 10. Resultado esperado de esta etapa
Al finalizar esta etapa debe existir como mínimo:

1. nueva solución `DigitalPlusMultiTenant`
2. stack actualizado a .NET 10
3. modelo multi-tenant definido y funcionando
4. base creada por EF Migrations
5. Identity funcionando
6. empresa Kosiuko creada
7. proceso inicial de migración de datos implementado
8. dashboard inicial moderno
9. listados paginados en módulos principales
10. arquitectura documentada

---

## 11. Entregables obligatorios
Debes generar como salida de trabajo:

1. documento de análisis del portal actual
2. documento de arquitectura de la nueva solución
3. modelo de entidades
4. estrategia de tenancy
5. modelo de Identity y autorización
6. esquema de base de datos
7. migraciones iniciales de EF
8. proceso o scripts de migración de datos
9. primer prototipo funcional del nuevo portal
10. log de decisiones técnicas y trade-offs

---

## 12. Regla importante
No rehacer por rehacer. No arrastrar deuda técnica innecesaria. No inventar una arquitectura caprichosa.

Debes tomar lo útil del portal actual, mejorarlo, modernizarlo y dejar una base sólida para escalar a múltiples empresas sin múltiples deploys.
