# Plan de Reducción de Costos Azure — Grupo Harley
**Ahorro estimado: ~AR$207,000/mes**
Factura actual: AR$233,306 → Objetivo: ~AR$26,000

---

## PASO 1 — Bajar App Service Plan Premium P0v3 → B1
**Impacto: -AR$102,000/mes** (de AR$115,656 a ~AR$13,000)

Este es el `ghapp20230522...` en el grupo de recursos `grgharley`, región US Central.

1. Ir a **Azure Portal** → buscar **App Service Plans**
2. Seleccionar el plan `ghapp20230522...`
3. En el menú izquierdo → **Scale Up (App Service Plan)**
4. En la pestaña **Production**, seleccionar **B1**
   - 1 core, 1.75 GB RAM — suficiente para uso bajo
5. Click en **Apply**
6. Esperar confirmación (1-2 minutos, sin downtime)

> ⚠️ Las apps dentro del plan NO se reinician. El cambio es transparente.

---

## PASO 2 — Bajar App Service Plan Estándar S1 → B1
**Impacto: -AR$57,000/mes** (de AR$70,094 a ~AR$13,000)

Este es el `digitalplus20230714...` en el grupo de recursos `grdigitalplus`, región US Central.

1. Ir a **Azure Portal** → **App Service Plans**
2. Seleccionar el plan `digitalplus20230714...`
3. En el menú izquierdo → **Scale Up (App Service Plan)**
4. Seleccionar **B1** en la pestaña **Production**
5. Click en **Apply**

---

## PASO 3 — Eliminar Azure SignalR Service dedicado
**Impacto: -AR$47,000/mes**

> 💡 Blazor Server incluye SignalR internamente. El servicio dedicado de Azure SignalR solo es necesario para escalar a miles de conexiones simultáneas. Con 2-3 consultas/día no lo necesitás.

**Antes de eliminar — verificar que las apps no lo referencien:**

1. Ir a **Azure Portal** → buscar **SignalR Service**
2. Seleccionar `signalrgrupcharley`
3. En el menú izquierdo → **Connection strings**
4. Copiar el nombre del connection string (ej: `AzureSignalRConnectionString`)
5. Ir a cada App Service del grupo `grgharley`
6. En cada app → **Configuration** → **Application settings**
7. Verificar si existe ese connection string
8. Si existe → **eliminarlo** de la configuración de la app
9. Hacer **Save** y **restart** la app
10. Probar que Blazor funciona correctamente (2-3 minutos de prueba)
11. Una vez confirmado → volver al SignalR Service → **Delete**

> ⚠️ Si al eliminar el connection string la app falla, significa que está configurada para usar el servicio externo. En ese caso, simplemente no agregar el connection string y Blazor usará su SignalR interno automáticamente.

---

## PASO 4 — Eliminar apps innecesarias
**Impacto: libera slots, simplifica administración**

### Eliminar `digitalplusnewfamily`
1. Ir a **App Services** → seleccionar `digitalplusnewfamily`
2. Click en **Delete**
3. Confirmar escribiendo el nombre del recurso
4. Click en **Delete**

### Eliminar `digitalplusdashboard`
1. Ir a **App Services** → seleccionar `digitalplusdashboard`
2. Click en **Delete**
3. Confirmar y eliminar

> 💡 Cuando retomes el desarrollo del dashboard, lo creás nuevo dentro del plan `digitalplus` existente (ya pagado por el B1).

---

## Resumen de ahorro

| Acción | Costo actual | Costo nuevo | Ahorro |
|--------|-------------|-------------|--------|
| Plan Premium P0v3 → B1 | AR$115,656 | ~AR$13,000 | **~AR$102,000** |
| Plan Estándar S1 → B1 | AR$70,094 | ~AR$13,000 | **~AR$57,000** |
| Eliminar SignalR dedicado | AR$47,085 | AR$0 | **AR$47,000** |
| DNS + Storage | AR$488 | AR$488 | — |
| **TOTAL** | **AR$233,306** | **~AR$26,500** | **~AR$207,000/mes** |

---

## Verificación post-cambio

Después de aplicar todos los cambios, verificar:

- [ ] Las apps de gharley.com.ar responden correctamente
- [ ] Blazor Server carga y las conexiones en tiempo real funcionan
- [ ] digitalplusapp opera con normalidad
- [ ] Revisar la próxima factura (ciclo siguiente) para confirmar reducción

---

*Generado: Marzo 2026 — basado en factura E0600YU3CZ (ciclo 11/01/2026 - 10/02/2026)*
