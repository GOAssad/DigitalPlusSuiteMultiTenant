# DigitalPlus MultiTenant - Orden de lectura para Claude

## Objetivo
Estos documentos deben leerse y ejecutarse en orden. El objetivo es diseñar y comenzar la migración del portal actual `DigitalPlusWeb_Claude` hacia una nueva solución `DigitalPlusMultiTenant` con arquitectura SaaS multiempresa.

## Orden obligatorio

### 1. `0007_TASK_DigitalPlus_Web_MultiTenant.md`
Documento principal de alcance funcional y técnico.

### 2. `0008_Arquitectura_SaaS_MultiTenant.md`
Define las decisiones arquitectónicas que Claude debe respetar.

### 3. `0009_Modelo_BaseDeDatos_MultiTenant.md`
Define criterios de modelado, naming, integridad referencial, auditoría, tenancy y migración.

### 4. `0010_Plan_Ejecucion_Migracion_Kosiuko.md`
Define el orden de implementación y la migración de datos desde Ferozo hacia la nueva base.

## Instrucciones para Claude
- No improvisar una arquitectura distinta a la definida en estos documentos.
- Antes de programar, analizar el proyecto actual en:
  - `C:\Apps\Claude\Huellas\DigitalPlusWeb_Claude`
- Mantener la lógica funcional útil del portal actual.
- Modernizar stack, estructura, base de datos y experiencia visual.
- Trabajar localmente en modo debug.
- Producción actual en Ferozo no debe romperse ni alterarse durante la etapa inicial.

## Contexto importante
El portal actual usa Blazor Server .NET 7, EF Core + Dapper e Identity, según el reporte de arquitectura entregado previamente. La nueva solución debe conservar lo que sirva, pero reestructurada profesionalmente para multiempresa y migrada a .NET 10. fileciteturn0file0
