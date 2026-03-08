# ETAPA 4 — Provisioning seguro (Azure) + Instalador sin credenciales (Cloud/Local)

## Objetivo
Implementar un mecanismo de provisioning para que el instalador NO pida servidor/usuario/password y pueda obtener la configuración de conexión de forma segura y administrable.

Usaremos:
- Azure Functions (HTTP endpoint)
- Azure Key Vault (secrets)
- Activation codes (1 uso / expiran)
- DPAPI (ya implementado) para proteger el .config en el cliente

Referencias de patrón:
- Managed Identity para acceder a Key Vault sin credenciales embebidas
- Key Vault references / uso de Key Vault para secretos

## Arquitectura (MVP)
1) Admin genera un activation code para una empresa (o lo asigna a un perfil/servidor).
2) Instalador pide:
   - Empresa
   - Código de activación
   - Tipo: Local o Nube
3) Instalador llama a `POST /provision` (Azure Function).
4) La Function valida el código, arma/retorna configuración.
5) Instalador crea config de apps y ejecuta DPAPI para proteger `connectionStrings`.

## Reglas de negocio críticas
A) Cloud:
- Si la DB generada YA existe => NO continuar.
- Mostrar: "La base ya existe. Cambie el nombre de empresa y reintente. No se modificó nada."
- Volver atrás.

B) Local:
- Detectar cualquier instancia SQL local.
- Si no hay => instalar SQL Express.
- Si DB ya existe => NO continuar, volver atrás, cambiar nombre, reintentar.

## Azure Function: Endpoint
### POST /provision
Request JSON:
{
  "activationCode": "XXXX-XXXX",
  "companyName": "Mi Empresa S.A.",
  "installType": "cloud" | "local",
  "machineId": "<hash>"
}

Response JSON (si OK):
{
  "companySanitized": "MiEmpresaSA",
  "dbName": "DP_MiEmpresaSA",
  "mode": "cloud" | "local",
  "server": "<cloud-server-or-local-template>",
  "connectionString": "<string o params>",
  "policy": {
    "cloudMustFailIfDbExists": true,
    "localMustFailIfDbExists": true
  }
}

Errores:
- 400 invalid payload
- 401/403 invalid or expired activationCode
- 409 db already exists (cloud) => instalador debe volver atrás

## Seguridad
- Guardar secretos (credenciales cloud, endpoints) en Azure Key Vault.
- La Function debe leer Key Vault usando Managed Identity (RBAC: Key Vault Secrets User).
- Activation codes:
  - almacenados como hash (no plaintext)
  - expiran (ej 24hs)
  - 1 uso

## Donweb (no obligatorio en MVP)
- Opcional: Nginx reverse proxy delante del endpoint + rate limit + logs.
- Opcional: portal admin para generar activation codes.

## Implementación: tareas para Claude
1) Crear proyecto Azure Functions (C#) HTTP Trigger:
   - /provision
   - Validación JSON
   - Sanitización companyName (reglas ya definidas)
2) Crear tabla ActivationCodes (Azure SQL o alternativa):
   - CodeHash, ExpiresAt, UsedAt, ProfileId
   - Stored procedures o queries para validar+marcar usado
3) Integrar Key Vault:
   - Secret: CloudSqlConnectionString_<ProfileId> (o similar)
   - Leer desde Function con Managed Identity
4) Cambiar instalador (Inno Setup):
   - Agregar campo "Activation Code"
   - Consumir /provision
   - Manejar 409 (db exists) => volver atrás
   - Generar config con "Local" connection string
   - Ejecutar DPAPI protector (ya implementado) para ambas apps
5) Logging:
   - Instalador log: empresa, dbName, resultado, error
   - Function log: request id, result, sin secretos

## Entregables
- Código Azure Function + instrucciones de deploy
- Script SQL tabla ActivationCodes + SPs
- Documentación: cómo generar activation codes (aunque sea manual al inicio)
- setup.iss actualizado (provisioning + manejo 409)
- Checklist de pruebas end-to-end