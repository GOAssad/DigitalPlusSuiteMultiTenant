# TASK: Etapa 3.1 — Integración Lemon Squeezy en Portal MT
## Reemplazar simulador de pago por Lemon Squeezy real

**Proyecto:** DigitalPlus Suite Multi-Tenant
**Componente principal:** Portal Multi-Tenant (Blazor Server .NET 10)
**Componentes secundarios:** Azure Functions (.NET 8 Isolated)
**Base de datos:** DigitalPlusAdmin (Ferozo)

---

## CONTEXTO DEL SISTEMA

El Portal MT (`digitalplusportalmt.azurewebsites.net`) es la interfaz web del cliente. Actualmente tiene una pantalla de gestión de planes en `/configuracion/planes` que permite al usuario ver su plan actual y simular un cambio de plan. El simulador funciona correctamente en términos de UX pero no procesa pagos reales.

Las Azure Functions ya existen y están deployadas en Azure. Manejan activación y heartbeat de licencias. Son el lugar correcto para agregar los nuevos endpoints.

Los planes ya están definidos en la tabla `PlanConfig` de DigitalPlusAdmin con sus precios en USD (mensual y anual).

**Por qué Lemon Squeezy y no Stripe:** Argentina no está soportada por Stripe para crear cuentas de merchant. Lemon Squeezy opera como Merchant of Record, soporta pagos desde 135+ países, maneja taxes automáticamente, y acepta cuentas de Argentina. La API es REST estándar, sin SDK oficial para .NET — se usa HttpClient directo.

---

## PASO 0 — CONFIGURACIÓN LEMON SQUEEZY (hacer esto primero, fuera de código)

Gustavo debe completar esto antes de que Claude Code pueda probar el flujo end-to-end:

1. Crear cuenta en https://lemonsqueezy.com con datos de ItengraIA
2. Crear una **Store** (tienda) para Digital One
3. En la store, crear los **Products** con sus variantes:

| Producto | Variantes |
|---|---|
| Digital One Basic | Mensual (USD) + Anual (USD) |
| Digital One Pro | Mensual (USD) + Anual (USD) |
| Digital One Enterprise | Mensual (USD) + Anual (USD) |

   Cada variante es una suscripción recurrente. Los precios en USD se toman de la tabla `PlanConfig` de la BD.

4. Por cada variante, anotar el **Variant ID** numérico (visible en la URL al editar la variante)

5. En Settings > Webhooks, crear un webhook apuntando a:
   ```
   https://<azure-functions-url>/api/lsq/webhook
   ```
   Eventos a suscribir:
   - `subscription_created`
   - `subscription_updated`
   - `subscription_payment_success`
   - `subscription_expired`
   - `subscription_cancelled`

6. Copiar el **Signing Secret** del webhook (se genera al crearlo)

7. En Settings > API, generar una **API Key**

Variables que necesita el sistema:
- `LemonSqueezy__ApiKey` → la API key generada
- `LemonSqueezy__WebhookSecret` → el signing secret del webhook
- `LemonSqueezy__StoreId` → ID numérico de la store

---

## PASO 1 — BASE DE DATOS

Agregar campos Lemon Squeezy a la tabla `Empresas` en DigitalPlusAdmin. Crear y aplicar la migración EF Core en el proyecto Portal Licencias (que tiene el DbContext de DigitalPlusAdmin).

```sql
ALTER TABLE Empresas ADD
    LsqCustomerId        NVARCHAR(50)  NULL,   -- customer ID en Lemon Squeezy
    LsqSubscriptionId    NVARCHAR(50)  NULL,   -- subscription ID en Lemon Squeezy
    LsqVariantId         NVARCHAR(50)  NULL,   -- variant ID del plan activo
    LsqUpdatePaymentUrl  NVARCHAR(500) NULL,   -- URL para actualizar tarjeta (viene en webhook)
    LsqCustomerPortalUrl NVARCHAR(500) NULL,   -- URL del customer portal (viene en webhook)
    PlanVencimiento      DATETIME      NULL,   -- próxima fecha de cobro/renovación
    PlanOrigen           NVARCHAR(20)  NULL DEFAULT 'manual';
    -- Valores de PlanOrigen: 'lsq' | 'manual'
```

Aplicar la migración en Ferozo.

---

## PASO 2 — AZURE FUNCTIONS

Agregar nuevas funciones al proyecto Azure Functions existente. No hay NuGet oficial de Lemon Squeezy para .NET — usar `HttpClient` estándar para llamadas a la API REST.

### Variables de entorno

En `local.settings.json`:
```json
{
  "Values": {
    "LemonSqueezy__ApiKey": "...",
    "LemonSqueezy__WebhookSecret": "...",
    "LemonSqueezy__StoreId": "..."
  }
}
```

En Azure Portal > Function App > Configuration > Application Settings (producción): agregar las mismas tres variables.

### Función 1: CreateCheckout

**Trigger:** `POST /api/lsq/create-checkout`

Crea una URL de checkout de Lemon Squeezy para que el usuario pague.

**Request body:**
```json
{
  "empresaId": 5,
  "variantId": "12345",
  "successUrl": "https://digitalplusportalmt.azurewebsites.net/configuracion/planes?success=true"
}
```

**Lógica:**
1. Buscar la empresa en DigitalPlusAdmin por `empresaId` (necesita email de la empresa)
2. Llamar a la API de Lemon Squeezy para crear un checkout:

```http
POST https://api.lemonsqueezy.com/v1/checkouts
Authorization: Bearer {ApiKey}
Content-Type: application/vnd.api+json
Accept: application/vnd.api+json

{
  "data": {
    "type": "checkouts",
    "attributes": {
      "checkout_options": {
        "embed": false,
        "media": false,
        "button_color": "#c9a84c"
      },
      "checkout_data": {
        "email": "email@empresa.com",
        "custom": {
          "empresaId": "5",
          "plan": "basic"
        }
      },
      "product_options": {
        "redirect_url": "https://digitalplusportalmt.azurewebsites.net/configuracion/planes?success=true"
      }
    },
    "relationships": {
      "store": {
        "data": { "type": "stores", "id": "{StoreId}" }
      },
      "variant": {
        "data": { "type": "variants", "id": "{variantId}" }
      }
    }
  }
}
```

3. Retornar la `checkout_url` del response

**Response:**
```json
{
  "checkoutUrl": "https://digital-one.lemonsqueezy.com/checkout/..."
}
```

### Función 2: Webhook

**Trigger:** `POST /api/lsq/webhook`

Recibe todos los eventos de Lemon Squeezy.

**Verificación de firma (obligatoria):**
```csharp
var signature = request.Headers["X-Signature"];
var body = await request.ReadAsStringAsync();
var hash = HMACSHA256(key: WebhookSecret, data: body);
var expectedSignature = hash.ToHexString();
if (signature != expectedSignature) return Unauthorized();
```

**Siempre retornar HTTP 200** incluso ante errores internos. Lemon Squeezy reintenta 3 veces con backoff exponencial si no recibe 200.

El evento se identifica por el header `X-Event-Name` o por `meta.event_name` en el body JSON.

Los datos custom del checkout llegan en `meta.custom_data` → `{ "empresaId": "5", "plan": "basic" }`.

**Eventos a manejar:**

#### `subscription_created`
```json
meta.event_name = "subscription_created"
meta.custom_data.empresaId = "5"
meta.custom_data.plan = "basic"
data.id = "sub_xxx"                           ← LsqSubscriptionId
data.attributes.customer_id = "cus_xxx"      ← LsqCustomerId
data.attributes.variant_id = "12345"          ← LsqVariantId
data.attributes.renews_at = "2026-04-23..."   ← PlanVencimiento
data.attributes.urls.update_payment_method    ← LsqUpdatePaymentUrl
data.attributes.urls.customer_portal          ← LsqCustomerPortalUrl
```
- Leer `empresaId` y `plan` de `meta.custom_data`
- Guardar en Empresa: `LsqSubscriptionId`, `LsqCustomerId`, `LsqVariantId`, `LsqUpdatePaymentUrl`, `LsqCustomerPortalUrl`, `PlanOrigen = "lsq"`, `PlanVencimiento`
- Llamar a `ActualizarLicenciaAsync(empresaId, plan)` — método existente

#### `subscription_payment_success`
- Recuperar empresa por `LsqSubscriptionId`
- Actualizar `PlanVencimiento` con `data.attributes.renews_at`
- Actualizar URLs de portal/pago (pueden cambiar)
- Si empresa estaba suspendida, reactivarla

#### `subscription_updated`
- Recuperar empresa por `LsqSubscriptionId`
- Actualizar `LsqVariantId`, `PlanVencimiento`, URLs
- Si cambió la variante (upgrade/downgrade), determinar el plan y llamar `ActualizarLicenciaAsync`

#### `subscription_expired` / `subscription_cancelled`
- Recuperar empresa por `LsqSubscriptionId`
- Degradar a Free: llamar `ActualizarLicenciaAsync(empresaId, "free")`
- Limpiar `LsqSubscriptionId`, cambiar `PlanOrigen = "manual"`

---

## PASO 3 — PORTAL MT (Blazor)

### 3.1 Configuración

En `appsettings.json` del Portal MT:
```json
{
  "LemonSqueezy": {
    "Variants": {
      "BasicMensual": "1439760",
      "BasicAnual":   "1439796",
      "ProMensual":   "1439799",
      "ProAnual":     "1439800"
    }
  },
  "AzureFunctions": {
    "BaseUrl": "https://<function-app>.azurewebsites.net/api/"
  }
}
```

Los valores numéricos de los Variant IDs los provee Gustavo al crear los productos en Lemon Squeezy.

### 3.2 LemonSqueezyService

Crear `Services/LemonSqueezyService.cs`:

```csharp
public class LemonSqueezyService
{
    // Llama a Azure Function CreateCheckout y retorna la checkoutUrl
    Task<string> CreateCheckoutAsync(int empresaId, string variantId, string successUrl);
}
```

Registrar en `Program.cs` como servicio scoped con HttpClient apuntando a `AzureFunctions:BaseUrl`.

### 3.3 Modificar página /configuracion/planes

**Al cargar la página:**
- Si `?success=true` → mostrar alert verde: "¡Tu plan fue actualizado! Los nuevos límites están activos."
- Si `?canceled=true` → mostrar alert amarillo: "El proceso de pago fue cancelado."

**Botón de upgrade a plan pago:**
1. Click → mostrar spinner
2. Obtener el `variantId` correspondiente al plan elegido y período (mensual/anual) desde config
3. Llamar a `LemonSqueezyService.CreateCheckoutAsync(empresaId, variantId, successUrl)`
4. Recibir `checkoutUrl`
5. `NavigationManager.NavigateTo(checkoutUrl, forceLoad: true)`

**Selector mensual/anual:**
Agregar un toggle o tabs "Mensual / Anual" visible en la página de planes. Al seleccionar Anual, los precios muestran el valor anual con el descuento correspondiente (ya calculado en la BD, leer de `PlanConfig`). El `variantId` usado en el checkout cambia según la selección.

**Sección "Gestionar suscripción"** (visible solo si `PlanOrigen == "lsq"`):
- Texto: plan actual + fecha próximo cobro (`PlanVencimiento`)
- Botón "Actualizar tarjeta" → redirige a `LsqUpdatePaymentUrl` (guardada en BD, viene del webhook)
- Botón "Portal de facturación" → redirige a `LsqCustomerPortalUrl` (igual)
- Texto aclaratorio al pie de los planes: **"Precios expresados en USD"**

**Plan Free:** sin botón de pago, mostrar planes disponibles como opciones de upgrade.

### 3.4 Lemon.js (opcional pero recomendado)

Agregar en el layout principal para habilitar el checkout como overlay en lugar de nueva pestaña:

```html
<script src="https://app.lemonsqueezy.com/js/lemon.js" defer></script>
```

Si se usa overlay, el botón de checkout debe tener la clase `lemonsqueezy-button` y el link con `?embed=1`. Sin esto el checkout abre en una nueva pestaña (también funciona).

---

## PASO 4 — PORTAL LICENCIAS

Agregar en la vista de detalle de empresa una sección "Suscripción Lemon Squeezy" (solo lectura):

| Campo | Valor |
|---|---|
| LsqCustomerId | ID del customer (con link al dashboard de Lemon Squeezy) |
| LsqSubscriptionId | ID de la suscripción activa |
| PlanOrigen | lsq / manual |
| Próxima renovación | PlanVencimiento formateado |
| Estado | Activa / Cancelada / Vencida |

Agregar botón **"Cancelar suscripción"** (para soporte por parte de Integra IA):
- Llama directamente a la API de Lemon Squeezy:
  ```
  DELETE https://api.lemonsqueezy.com/v1/subscriptions/{LsqSubscriptionId}
  ```
  Esto cancela al final del período actual.
- Requiere confirmación antes de ejecutar.
- Este botón puede llamar a la API directamente desde el Portal Licencias (que ya tiene acceso a DigitalPlusAdmin) sin pasar por las Azure Functions.

---

## PASO 5 — VARIABLES DE ENTORNO Y DEPLOY

### Azure Functions — Application Settings en Azure Portal
```
LemonSqueezy__ApiKey         = ...
LemonSqueezy__WebhookSecret  = ...
LemonSqueezy__StoreId        = ...
```

### Portal MT — Application Settings en Azure Portal
```
LemonSqueezy__Variants__BasicMensual = 1439760
LemonSqueezy__Variants__BasicAnual   = 1439796
LemonSqueezy__Variants__ProMensual   = 1439799
LemonSqueezy__Variants__ProAnual     = 1439800
AzureFunctions__BaseUrl                   = https://<function-app>.azurewebsites.net/api/
```

### Portal Licencias — Application Settings en Azure Portal
```
LemonSqueezy__ApiKey = ...   (para cancelar suscripciones desde el portal de admin)
```

---

## FLUJO COMPLETO ESPERADO (verificar end-to-end)

```
Cliente en Portal MT > /configuracion/planes
  └── Ve plan actual: Free
  └── Selecciona período: Mensual
  └── Click en "Contratar Basic"
      └── LemonSqueezyService llama a Azure Function CreateCheckout
          └── Azure Function llama a API de Lemon Squeezy
          └── Lemon Squeezy devuelve checkoutUrl
      └── Portal MT redirige al navegador a Lemon Squeezy Checkout
          └── Cliente ingresa tarjeta (modo test)
          └── Lemon Squeezy procesa el pago
          └── Redirige a /configuracion/planes?success=true
              └── Portal MT muestra confirmación
          └── En paralelo: Lemon Squeezy envía webhook subscription_created
              └── Azure Function lee empresaId de meta.custom_data
              └── Actualiza Empresa en BD (plan, IDs, URLs, vencimiento)
              └── Llama ActualizarLicenciaAsync → PlanConfig aplicado
              └── Plan activo inmediatamente
```

---

## MODO TEST EN LEMON SQUEEZY

Lemon Squeezy tiene modo test nativo. En el dashboard hay un toggle "Test mode". En test mode:
- Los pagos no son reales
- Se puede simular eventos de webhook desde el dashboard (Settings > Webhooks > "Simulate event")
- Las URLs de checkout son de prueba

Tarjeta de prueba: `4242 4242 4242 4242`, fecha futura, cualquier CVC.

Para probar webhooks localmente, usar un túnel como **ngrok**:
```bash
ngrok http 7071
# Usar la URL pública de ngrok como webhook URL en Lemon Squeezy dashboard
```

---

## RESTRICCIONES

- **No modificar** `ActualizarLicenciaAsync` — solo llamarla desde el webhook handler
- **No modificar** el flujo de activación por código del instalador — Lemon Squeezy es un canal adicional
- **No tocar** la lógica de heartbeat existente
- El webhook debe retornar **HTTP 200** siempre, incluso ante errores internos
- Las URLs `LsqUpdatePaymentUrl` y `LsqCustomerPortalUrl` tienen expiración — actualizar siempre desde el webhook, nunca cachear en cliente

---

*Etapa 3.1 — Digital One Comercialización Internacional*
*ItengraIA — 2026-03-23*
