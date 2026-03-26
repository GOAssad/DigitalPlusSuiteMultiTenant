# Tarea: Migración DigitalPlus → DigitalPlusMultiTenant + Alta Kosiuko

## Contexto general

Estamos pasando al cliente **Kosiuko** de la versión legacy del sistema (`DigitalPlus`, single-tenant, en producción en Ferozo) a la nueva versión multi-tenant (`DigitalPlusMultiTenant`). Ambas bases de datos residen en el **mismo servidor en Ferozo** y Claude Code tiene acceso directo a ellas.

El objetivo es que Kosiuko siga utilizando el sistema **exactamente igual que hoy** (registro de asistencia por huella dactilar), sin interrupciones, y que toda su información histórica quede disponible en la nueva base. Las funcionalidades nuevas (Mobile, QR, PIN, Kiosco) se habilitarán en una etapa posterior.

Los dispositivos Fichador y la aplicación Administrador **ya apuntan a la nueva base** mediante el instalador publicado en la web; no requieren ninguna acción de reconfiguración.

---

## Paso 1 — Alta de Kosiuko en DigitalPlusAdmin

En la base `DigitalPlusAdmin` (base de licencias/portal), crear el registro de la empresa Kosiuko con las siguientes características:

- **Nombre:** Kosiuko
- **Plan:** Enterprise Manual
  - Esto significa que **no se integra con Lemon** ni con ningún sistema de cobro automático.
  - La licencia se gestiona manualmente desde el portal de administración.
- Completar todos los campos requeridos por el modelo de empresa existente (RazónSocial, CUIT, etc.) con los valores que correspondan o placeholders si no se tienen aún — lo importante es que el registro sea válido y funcional.
- **Capturar el TenantId** (GUID o entero, según el esquema) generado para Kosiuko. Este valor se utilizará en todo el Paso 2 para asociar los datos migrados.

> ⚠️ Antes de insertar, verificar si ya existe algún registro de "Kosiuko" en `DigitalPlusAdmin` para evitar duplicados.

---

## Paso 2 — Migración de datos: DigitalPlus → DigitalPlusMultiTenant

### Instrucción general

Antes de escribir cualquier script de migración, **explorar el esquema completo** de ambas bases de datos:
- Listar todas las tablas de `DigitalPlus`
- Listar todas las tablas de `DigitalPlusMultiTenant`
- Mapear la correspondencia entre tablas origen → destino
- Identificar columnas que existen en el destino pero no en el origen (columnas nuevas de MT), y aplicarles sus valores por defecto

Toda la información migrada debe quedar asociada al **TenantId de Kosiuko** obtenido en el Paso 1.

---

### Tablas / entidades a migrar

#### 2.1 Sucursales (Branches / Offices)
- Migrar todos los registros de sucursales existentes en `DigitalPlus`.
- **No se migran datos de geolocalización** (latitud, longitud, radio de GPS). Si existen esas columnas en el origen, ignorarlas.
- Si `DigitalPlusMultiTenant` tiene columnas de geolocalización, dejarlas en `NULL` o en el valor por defecto.
- Asociar cada sucursal con el TenantId de Kosiuko.

#### 2.2 Legajos (Empleados)
- Migrar todos los empleados/legajos activos e inactivos.
- Preservar todos los campos del modelo de empleado: nombre, apellido, número de legajo, área, categoría, y cualquier otro atributo que exista en el origen.
- Asociar cada legajo con el TenantId de Kosiuko.
- Mantener los mismos IDs de empleado si el esquema de destino lo permite, o generar un mapeo ID-origen → ID-destino para usarlo en la migración de huellas.

#### 2.3 Huellas Dactilares (Fingerprints)
- **Esta es la migración más crítica.** Si las huellas no migran correctamente, los dispositivos físicos dejan de reconocer a los empleados.
- Las huellas están almacenadas como **blobs binarios** en `DigitalPlus`.
- Migrarlas **tal cual**, sin ninguna transformación del blob.
- Asegurarse de que la asociación legajo → huellas se mantiene correcta en el destino (usando el mapeo de IDs del punto anterior si fue necesario).
- Verificar que se migran **todas** las huellas de cada empleado (puede haber más de una por persona).

#### 2.4 Fichadas / Registros de Asistencia (Attendance Records)
- Migrar el historial completo de fichadas.
- Asociar con el TenantId de Kosiuko y con los IDs de legajo correctos en el destino.

#### 2.5 Horarios, Turnos y Configuración de Jornada
- Migrar todos los esquemas de horarios y turnos definidos.
- Asociar con el TenantId de Kosiuko.

#### 2.6 Cualquier otra tabla con datos de negocio
- Revisar si en `DigitalPlus` existen otras tablas con datos persistidos (configuraciones de empresa, feriados, categorías, áreas, etc.) y migrarlas también, asociando con el TenantId de Kosiuko.

---

### Columnas nuevas en DigitalPlusMultiTenant (versión MT)

La nueva versión incorpora funcionalidades que no existían: Mobile, QR, PIN y Kiosco. Al migrar, aplicar los siguientes valores por defecto para que Kosiuko opere **solo con huella dactilar**, tal como lo hace hoy:

| Funcionalidad | Valor por defecto al migrar |
|---|---|
| Registro por Mobile | Deshabilitado / `false` |
| Registro por QR | Deshabilitado / `false` |
| Registro por PIN | Deshabilitado / `false` |
| Modo Kiosco | Deshabilitado / `false` |
| Registro por Huella | Habilitado / `true` |

Adaptar según los nombres de columnas reales que encuentres en el esquema.

---

## Paso 3 — Verificación post-migración

Una vez ejecutada la migración, realizar las siguientes comprobaciones:

1. **Conteo de registros:** Verificar que el número de empleados, huellas y fichadas migradas en `DigitalPlusMultiTenant` coincide con el origen en `DigitalPlus`.
2. **Integridad referencial:** Confirmar que todas las huellas tienen su legajo correspondiente en el destino y que no hay huérfanos.
3. **Empresa en DigitalPlusAdmin:** Confirmar que Kosiuko existe con plan Enterprise Manual y el TenantId correcto.
4. **Asociación TenantId:** Confirmar que todos los registros migrados en `DigitalPlusMultiTenant` tienen el TenantId de Kosiuko.

---

## Notas importantes

- **No modificar ni eliminar nada en `DigitalPlus`.** La base de producción actual debe quedar intacta durante todo el proceso. La migración es aditiva: solo inserta en el destino.
- Si se detectan conflictos de clave primaria en el destino, resolverlos con un offset de IDs o con el mecanismo que el esquema de destino ya prevea para multi-tenant.
- Si alguna tabla del origen no tiene correspondencia clara en el destino, **no asumir**: reportar la tabla y esperar instrucción antes de proceder.
- Toda la operación debe ejecutarse dentro de una transacción (o por bloques con rollback disponible) para poder revertir si algo falla.
