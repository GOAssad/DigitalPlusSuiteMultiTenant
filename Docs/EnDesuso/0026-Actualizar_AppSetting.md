# Autorización – Actualización de appsettings de producción para DigitalPlusWeb

Podés proceder con la actualización del archivo de configuración de producción de DigitalPlusWeb.

## Objetivo

Dejar preparado el archivo `appsettings.json` de producción para que la web utilice el nuevo usuario SQL dedicado:

- `dp_web_svc`

## Cambios a realizar

Actualizar las conexiones de producción que hoy apuntan a Ferozo:

### Reemplazar

- `DefaultConnection`
- `DefaultConnectionCompleto`
- `DefaultConnectionAdmin`

### Nuevo criterio

Todas esas conexiones deben usar:

- `User Id=dp_web_svc`
- `Password=<password_web correspondiente>`

## Importante

- Las connection strings legacy que apuntan a `192.168.0.11` no forman parte de esta fase crítica de producción sobre Ferozo.
- No eliminar todavía esas strings legacy salvo que ya esté confirmado que no se usan.
- Solo dejar documentado cuáles siguen siendo legacy y cuáles son las activas de producción.

## Entregable esperado

1. Archivo `appsettings.json` de producción actualizado
2. Resumen exacto de qué connection strings fueron modificadas
3. Confirmación de que el cambio queda listo para deploy manual en Ferozo
4. Nota aclaratoria indicando que el deploy al hosting web deberá hacerse manualmente

## Aclaración operativa

Claude debe dejar el archivo listo y consistente, pero no asumir que el deploy ya ocurrió.

El estado esperado al finalizar este bloque es:

- configuración de producción actualizada en código
- lista para publicar manualmente en Ferozo
- sin depender más de usuarios previos como `DigitalPlus` o `GASSAD` para las conexiones activas a Ferozo