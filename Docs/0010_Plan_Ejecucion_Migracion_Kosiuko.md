# TASK 0010 - Plan de ejecución y migración inicial Kosiuko

## Objetivo
Definir el orden de ejecución recomendado para que la implementación de `DigitalPlusMultiTenant` no se convierta en una demolición improvisada con teclado.

---

## Etapa 1 - Análisis del sistema actual
Analizar en detalle:

`C:\Apps\Claude\Huellas\DigitalPlusWeb_Claude`

Salida esperada:

- documento técnico del estado actual
- inventario de módulos
- inventario de entidades
- inventario de páginas
- inventario de tablas consultadas
- inventario de usuarios/autenticación

---

## Etapa 2 - Diseño de arquitectura nueva
Definir:

- estructura de proyectos
- entidades base
- estrategia de tenancy
- estrategia de Identity
- criterios de UI compartida
- servicios y repositorios

Salida esperada:

- documento de arquitectura
- árbol propuesto de solución

---

## Etapa 3 - Diseño de base de datos nueva
Definir:

- entidades principales
- relaciones
- auditoría
- índices
- restricciones
- criterios de naming

Salida esperada:

- modelo de base nuevo
- primeras migraciones EF

---

## Etapa 4 - Creación de solución nueva
Crear la solución:

`DigitalPlusMultiTenant`

con .NET 10 y los proyectos necesarios.

Salida esperada:

- solución compilando
- dependencias instaladas
- DbContext inicial
- Identity integrado

---

## Etapa 5 - Infraestructura de tenancy
Implementar:

- `Empresa`
- contexto de empresa activa
- asociación usuario-empresa
- filtros por empresa
- bases para autorización

Salida esperada:

- login funcionando
- usuario restringido por empresa

---

## Etapa 6 - Migraciones EF
Crear migraciones iniciales de la nueva base.

Salida esperada:

- base `DigitalPlusMultiTenant` creada localmente
- esquema consistente

---

## Etapa 7 - Migración de datos local
Trabajar en local con:

- `DigitalPlus` copia de Ferozo
- `DigitalPlusAdmin` local
- `DigitalPlusMultiTenant`

Crear scripts, procesos o utilidades para migrar:

1. Kosiuko como empresa
2. datos maestros
3. legajos
4. fichadas
5. usuarios

Salida esperada:

- datos migrados localmente
- validaciones de conteo
- validaciones funcionales

---

## Etapa 8 - UI inicial profesional
Implementar una base visual homogénea:

- layout principal
- menú lateral o superior consistente
- dashboard
- tabla paginada reutilizable
- filtros reutilizables
- pantallas base de legajos y fichadas

Salida esperada:

- portal navegable con imagen profesional

---

## Etapa 9 - Validación funcional
Comparar el sistema nuevo contra el actual para Kosiuko.

Validar:

- login
- acceso por empresa
- listados
- cantidad de datos visibles
- consistencia de información
- navegación principal

---

## Etapa 10 - Documentación y próximos pasos
Documentar:

- qué quedó terminado
- qué falta
- qué se decidió
- qué problemas aparecieron
- siguientes tareas recomendadas

---

## Regla de trabajo
Durante toda esta etapa:

- no tocar producción innecesariamente
- no depender de Ferozo para debug diario
- primero resolver en local
- dejar trazabilidad de decisiones y scripts

---

## Resultado esperado final de esta fase
Al cierre debe existir una base sólida para continuar el desarrollo del portal multiempresa con Kosiuko ya migrada y con el nuevo sistema encaminado como producto SaaS real.
