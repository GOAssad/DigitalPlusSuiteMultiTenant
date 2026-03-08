# TAREA: Unificar persistencia contra base DigitalPlus (GUS-IDEAPAD)

## Contexto general

Existen dos aplicaciones de escritorio dentro del proyecto DigitalPlusDesk_Claude:

1) Administrador
2) Fichador

Ambas utilizan un detector de huella digital (SDK incluido en Complementos y Digital-Persona-SDK-master).

Actualmente:
- La base de datos correcta y vigente es: 
  Servidor: GUS-IDEAPAD
  Base: DigitalPlus
- Esta base es copia exacta de producción.
- Fichador YA está guardando en DigitalPlus (requiere revisión pero funciona).
- Administrador fue desarrollado originalmente para guardar en otra estructura/base distinta y debe adaptarse completamente.

Objetivo:
Unificar completamente la persistencia de:
- Usuarios
- Huellas digitales
- Registros de acceso (ingreso/egreso)

para que TODO quede almacenado en la base DigitalPlus.

---

# ESTRUCTURA DEL PROYECTO

DigitalPlusDesk_Claude
 ├── Complementos (SDK y dependencias)
 └── DigitalOnePlus
      ├── Common
      ├── Datos
      ├── Administrador
      ├── Fichador
      └── Digital-Persona-SDK-master

Common:
Contiene librerías compartidas por Administrador y Fichador.

Administrador:
- Registra usuarios
- Captura huellas
- Guarda usuarios + huellas en la base

Fichador:
- Lee huella
- Identifica persona
- Registra ingreso/egreso en base

---

# OBJETIVO TÉCNICO

1) Estudiar la base DigitalPlus en GUS-IDEAPAD.
2) Identificar:
   - Tablas actuales de usuarios
   - Tablas actuales de huellas
   - Tablas actuales de registros (ingresos/egresos)
   - Relaciones y claves primarias
3) Detectar dónde Administrador está apuntando todavía a:
   - Tablas antiguas
   - Otra base
   - Estructuras obsoletas
4) Adaptar TODO el flujo de persistencia del Administrador para que:
   - Inserte usuarios en las tablas correctas de DigitalPlus
   - Inserte huellas en las tablas correctas
   - Respete las claves y relaciones actuales
5) Validar que Fichador esté usando exactamente la misma estructura.

---

# PLAN DE TRABAJO OBLIGATORIO

## ETAPA 1 – Análisis Base DigitalPlus

Generar reporte:
- Lista de tablas relevantes
- PK / FK
- Campos críticos
- Tipos de datos
- Restricciones
- Si existen SP o vistas para guardar huellas o fichadas

Entregable:
DocumentacionClaude/Reporte_Modelo_DB_Actual.md

---

## ETAPA 2 – Auditoría del código

Relevar en Administrador:

- ConnectionStrings
- Queries inline
- Stored Procedures utilizados
- Repositorios / clases DAL
- Mapeos a entidades
- Nombres de tablas hardcodeadas
- Uso de transacciones

Generar tabla:

| Archivo | Método | Tabla referenciada | Acción (Insert/Update/Select) | Riesgo |

Entregable:
DocumentacionClaude/Reporte_Impacto_Codigo.md

---

## ETAPA 3 – Mapeo de equivalencias

Crear tabla:

| Tabla antigua | Tabla nueva DigitalPlus | Campo antiguo | Campo nuevo | Transformación necesaria | Confianza |

Si algo no tiene equivalente directo:
- Proponer vista de compatibilidad
o
- Proponer refactor de código

NO decidir sin documentar.

---

## ETAPA 4 – Adaptación controlada

Adaptar por bloques:

1) Persistencia de Usuario
2) Persistencia de Huella
3) Validación de duplicados
4) Relaciones usuario-huella
5) Transacciones

Cada bloque debe:
- Compilar
- Insertar datos correctamente
- Poder validarse con SELECT directo en DB

---

## RESTRICCIONES

- NO modificar lógica del SDK.
- NO cambiar estructura de DigitalPlus.
- NO romper Fichador.
- Mantener consistencia transaccional.
- Manejar correctamente errores de conexión.

---

## VALIDACIÓN FINAL

Debe verificarse:

1) Alta de usuario desde Administrador
2) Registro de huella
3) Verificación en base DigitalPlus
4) Fichador reconoce esa huella
5) Registro de ingreso/egreso se guarda correctamente

---

## SI HAY DUDA

Antes de implementar:
Documentar la duda.
Proponer 2 alternativas.
Explicar riesgos.
Esperar confirmación.

---

# RESULTADO ESPERADO

- Administrador y Fichador persisten contra DigitalPlus.
- No existen referencias a bases antiguas.
- Código documentado.
- Migración trazable.
- Sistema estable.