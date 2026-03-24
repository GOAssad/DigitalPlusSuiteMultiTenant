# TASK: Plan Enterprise — Tratamiento especial en Portal MT y Portal Licencias

**Proyecto:** DigitalPlus Suite Multi-Tenant
**Componentes:** Portal MT (Blazor), Portal Licencias (Blazor), Azure Functions
**Base de datos:** DigitalPlusAdmin (Ferozo)

---

## CONTEXTO

Lemon Squeezy ya está integrado y funcionando (checkout, webhooks, cancelación, alertas).

El plan Enterprise es un acuerdo comercial negociado directamente entre Integra IA y el cliente — precio variable por empresa, sin self-service. No aparece en la pantalla de planes del Portal MT como opción de compra directa.

**El problema actual:** cuando una empresa tiene plan Enterprise, la pantalla `/configuracion/planes` del Portal MT sigue mostrando Basic y Pro como opciones de upgrade, lo cual no tiene sentido y puede generar confusión.

---

## DISEÑO DE LA SOLUCIÓN

### Modelo de datos

Enterprise en Lemon Squeezy se maneja con **un producto por cliente**: Integra IA crea un producto específico en el dashboard de Lemon Squeezy con el precio acordado, y carga el Variant ID en el Portal Licencias.

Agregar campo a la tabla `Empresas` en DigitalPlusAdmin:

```sql
ALTER TABLE Empresas ADD
    LsqVariantIdCustom NVARCHAR(50) NULL;
    -- Variant ID personalizado para planes Enterprise negociados
    -- Cuando está cargado, tiene prioridad sobre los variants del config
```

Aplicar migración EF Core y en Ferozo.

### Regla de negocio

Una empresa es Enterprise cuando `Plan == "enterprise"` en su licencia. Este valor puede llegar por dos vías:
1. Webhook de Lemon Squeezy (si pagó con el variant custom)
2. Asignación manual desde el Portal Licencias (Integra IA lo activa directamente)

---

## CAMBIO 1 — Portal Licencias

### 1.1 Campo LsqVariantIdCustom en detalle de empresa

En la sección "Suscripción Lemon Squeezy" del detalle de empresa, agregar campo editable:

| Campo | Descripción |
|---|---|
| **Variant ID personalizado** | Variant ID del producto Enterprise creado en LSQ para este cliente |

Con botón **"Generar link de pago"** al lado: llama a `POST /api/lsq/create-checkout` con el `LsqVariantIdCustom` y genera una URL de checkout que Integra IA le envía al cliente por email.

### 1.2 Activación manual de plan Enterprise

En el detalle de empresa, agregar botón **"Activar plan Enterprise manualmente"** para casos donde el pago se gestiona fuera de Lemon Squeezy (transferencia, factura, etc.):

- Al hacer click → modal de confirmación con campo "Fecha de vencimiento"
- Confirmar → llama a `ActualizarLicenciaAsync(empresaId, "enterprise")` directamente
- Setea `PlanOrigen = "manual"`, `PlanVencimiento` con la fecha ingresada
- No requiere Lemon Squeezy

---

## CAMBIO 2 — Azure Functions

### 2.1 create-checkout soporta LsqVariantIdCustom

La función `POST /api/lsq/create-checkout` ya existe. Modificar para que:
1. Si la empresa tiene `LsqVariantIdCustom` cargado → usar ese variant
2. Si no → usar el variant del config según el plan seleccionado (comportamiento actual)

No hay breaking changes — el Portal MT sigue enviando el `variantId` explícitamente. El `LsqVariantIdCustom` es para el flujo iniciado desde el Portal Licencias.

### 2.2 VariantMap — agregar soporte Enterprise

El mapeo `LemonSqueezy:VariantMap:{variantId}` en Azure Functions debe poder mapear cualquier variant ID a `"enterprise"`. Como los variants Enterprise son por cliente (no fijos), el webhook debe detectar el plan por el nombre del producto o por metadata, no por variant ID fijo.

**Solución:** en el checkout generado para Enterprise, incluir en `custom_data` el campo `"plan": "enterprise"`. El webhook ya lee `meta.custom_data.plan` — no hay cambios en el handler.

---

## CAMBIO 3 — Portal MT: pantalla /configuracion/planes

### 3.1 Vista Enterprise

Cuando `Plan == "enterprise"`, reemplazar completamente la grilla de planes por una vista dedicada:

```
┌─────────────────────────────────────────────────────┐
│  ⭐ Plan Enterprise                                  │
│                                                     │
│  Tenés acceso completo a Digital One sin límites.   │
│                                                     │
│  Legajos:        Ilimitados                         │
│  Sucursales:     Ilimitadas                         │
│  Terminales:     Ilimitadas                         │
│  Fichadas:       Ilimitadas                         │
│                                                     │
│  Próxima renovación: [PlanVencimiento]              │
│                                                     │
│  [Gestionar facturación]  [Cancelar suscripción]    │
│                                                     │
│  ¿Necesitás ayuda? Contactá a soporte@integraia.tech│
└─────────────────────────────────────────────────────┘
```

- El botón "Gestionar facturación" solo aparece si `PlanOrigen == "lsq"` (tiene suscripción LSQ activa)
- El botón "Cancelar suscripción" solo aparece si `PlanOrigen == "lsq"`
- Si `PlanOrigen == "manual"` → solo mostrar la info del plan y el contacto de soporte, sin botones de pago

### 3.2 Ocultar planes Basic y Pro

Cuando `Plan == "enterprise"`, **no mostrar** la grilla de planes Basic/Pro ni ningún botón de upgrade. La empresa Enterprise no tiene opciones de self-service.

### 3.3 Middleware de jaula

Verificar que el middleware existente que redirige por suscripción expirada **no aplique** cuando `PlanOrigen == "manual"` y el plan es Enterprise — Integra IA renueva manualmente en esos casos.

---

## FLUJO COMPLETO ENTERPRISE CON PAGO LSQ

```
1. Integra IA cierra trato con cliente Enterprise
2. Integra IA crea producto en LSQ dashboard con precio acordado
3. Integra IA carga el Variant ID en Portal Licencias > detalle empresa > LsqVariantIdCustom
4. Integra IA hace click en "Generar link de pago"
   └── Azure Function crea checkout con custom_data.plan = "enterprise"
   └── Integra IA envía la URL al cliente por email
5. Cliente paga → webhook subscription_created
   └── Lee custom_data.plan = "enterprise"
   └── Llama ActualizarLicenciaAsync(empresaId, "enterprise")
   └── PlanOrigen = "lsq"
6. Cliente entra al Portal MT → ve vista Enterprise dedicada
```

## FLUJO COMPLETO ENTERPRISE MANUAL (sin LSQ)

```
1. Integra IA cierra trato, cobra por transferencia/factura
2. En Portal Licencias > detalle empresa > "Activar plan Enterprise manualmente"
   └── Ingresa fecha de vencimiento
   └── ActualizarLicenciaAsync(empresaId, "enterprise")
   └── PlanOrigen = "manual", PlanVencimiento = fecha ingresada
3. Cliente entra al Portal MT → ve vista Enterprise sin botones de pago
```

---

## RESTRICCIONES

- No modificar `ActualizarLicenciaAsync` — solo llamarla
- No agregar Enterprise como opción self-service en el Portal MT
- El middleware de jaula no debe bloquear empresas Enterprise con `PlanOrigen == "manual"`
- Los planes Basic y Pro no se ven cuando el plan activo es Enterprise

---

*Digital One — ItengraIA — 2026-03-24*
