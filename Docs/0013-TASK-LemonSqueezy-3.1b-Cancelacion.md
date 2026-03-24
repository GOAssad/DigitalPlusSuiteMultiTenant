# TASK: Cancelación de suscripción Lemon Squeezy desde Portal MT

**Contexto:** La integración con Lemon Squeezy ya está funcionando (checkout, webhooks, activación de plan). 
Falta permitir que el usuario cancele su suscripción activa desde el Portal MT.

---

## COMPORTAMIENTO ESPERADO

- El botón de cancelación es visible **únicamente** cuando `PlanOrigen == "lsq"` en la tabla `Empresas` de DigitalPlusAdmin
- Al cancelar, el plan **no se corta inmediatamente** — sigue activo hasta `PlanVencimiento`
- Cuando vence, el webhook `subscription_expired` ya existente degrada automáticamente a Free
- Después de cancelar, el usuario puede volver a contratar cualquier plan normalmente

---

## CAMBIOS REQUERIDOS

### 1. Nueva Azure Function

**Trigger:** `POST /api/lsq/cancel-subscription`

**Request body:**
```json
{ "empresaId": 5 }
```

**Lógica:**
1. Buscar `LsqSubscriptionId` de la empresa en DigitalPlusAdmin
2. Si no tiene suscripción activa → retornar error 400
3. Llamar a la API de Lemon Squeezy:
```http
PATCH https://api.lemonsqueezy.com/v1/subscriptions/{LsqSubscriptionId}
Authorization: Bearer {ApiKey}
Content-Type: application/vnd.api+json

{
  "data": {
    "type": "subscriptions",
    "id": "{LsqSubscriptionId}",
    "attributes": {
      "cancelled": true
    }
  }
}
```
4. Si la respuesta es exitosa → retornar 200 con `{ "ok": true, "vencimiento": "2026-04-23" }`
5. Si falla → retornar 400 con el error

> Nota: `cancelled: true` en Lemon Squeezy cancela al final del período actual, no inmediatamente.

---

### 2. LemonSqueezyService en Portal MT

Agregar método al servicio existente:

```csharp
Task<CancelResult> CancelSubscriptionAsync(int empresaId);
```

---

### 3. Página /configuracion/planes — Agregar botón de cancelación

**Condición de visibilidad:** solo mostrar si `PlanOrigen == "lsq"` Y la suscripción no está ya cancelada (verificar que `LsqSubscriptionId` no sea null).

**UX del botón:**

1. Botón "Cancelar suscripción" — estilo discreto (no destructivo visualmente, link o botón secundario, no dorado)
2. Click → mostrar modal de confirmación con este texto:
   > "¿Confirmás la cancelación? Tu plan **[Basic/Pro]** seguirá activo hasta el **[PlanVencimiento formateado]**. 
   > Al vencer pasará automáticamente al plan Free."
3. Botones del modal: "Sí, cancelar" y "Volver"
4. Si confirma → llamar a `LemonSqueezyService.CancelSubscriptionAsync(empresaId)`
5. Mostrar alert verde: "Tu suscripción fue cancelada. El plan sigue activo hasta [fecha]."
6. El botón "Cancelar suscripción" desaparece o se reemplaza por texto informativo: "Suscripción cancelada — activa hasta [fecha]"

---

### 4. Portal Licencias — Botón de cancelación para soporte

En la sección "Suscripción Lemon Squeezy" del detalle de empresa, agregar botón "Cancelar suscripción" que llame a la misma Azure Function. Requiere confirmación antes de ejecutar.

---

## LO QUE NO HAY QUE TOCAR

- El webhook `subscription_expired` ya maneja la degradación a Free automáticamente — no modificar
- No implementar downgrade directo entre planes — el flujo es cancelar + recontratar
- No modificar `ActualizarLicenciaAsync`

---

*Digital One — ItengraIA — 2026-03-24*
