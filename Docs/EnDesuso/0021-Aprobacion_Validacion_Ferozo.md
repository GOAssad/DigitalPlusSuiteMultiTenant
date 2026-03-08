# Aprobación de validación Ferozo y siguiente paso

La validación de Ferozo queda aprobada.

Resultados confirmados:

- CREATE LOGIN: SI
- CHECK_POLICY = ON: SI
- CREATE USER: SI
- CREATE ROLE: SI
- GRANT ON SCHEMA: SI
- ALTER ROLE ADD MEMBER: SI
- Conexión con usuario de prueba: SI

Conclusión:

Ferozo soporta completamente el modelo definido en `0020-DigitalPlus_Seguridad_SQL_y_AdminPortal_v2.md`.

No se requieren alternativas como `CREATE USER WITHOUT LOGIN`, ni cambios de estrategia por limitaciones del hosting.

## Siguiente tarea

Procedé con el **Paso 3: creación de scripts definitivos de roles y logins**, pero estructurándolo de la siguiente manera:

### 1. Scripts definitivos para Ferozo

Generar scripts separados y listos para ejecución para:

- creación de roles en `DigitalPlusAdmin`
- creación de `dp_admin_svc`
- creación de roles necesarios en `DigitalPlus`
- creación de `dp_web_svc`
- asignación de permisos y membresías correspondientes

### 2. Script base para instalaciones cliente

Generar un script o plantilla reutilizable para:

- creación de roles `dp_role_runtime` y `dp_role_admin`
- creación del login dedicado por empresa
- creación del user en la base del cliente
- asignación de roles
- uso de `CHECK_POLICY = ON` por defecto

### 3. Plan de aplicación

Además de los scripts, generar un documento que indique:

- orden exacto de ejecución
- qué configuraciones deben reemplazarse
- qué archivos deben modificarse
- qué pruebas funcionales deben realizarse luego del cambio
- cómo verificar que `sa` ya no se use en runtime

## Entregables

Generar:

- scripts SQL definitivos
- documento de aplicación y despliegue
- checklist de validación post-cambio

No avanzar todavía con el instalador ni con el portal admin hasta cerrar y probar completamente esta fase central.