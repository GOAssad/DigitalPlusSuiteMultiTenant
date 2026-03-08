# Guia de Deployment - DigitalPlus Provisioning

## Prerequisitos

1. Cuenta Azure con suscripcion activa
2. Azure CLI instalado (`az`)
3. .NET 8 SDK
4. Acceso al servidor SQL de Ferozo

## Paso 1: Ejecutar SQL scripts en Ferozo

Conectar a `sd-1985882-l.ferozo.com,11434` / DB `DigitalPlus` y ejecutar en orden:

```
sql/001_CreateActivationCodesTable.sql
sql/002_SP_ValidateAndConsumeCode.sql
sql/003_SP_GenerateActivationCode.sql
sql/004_SP_ListActivationCodes.sql
```

## Paso 2: Crear recursos en Azure

```bash
# Login
az login

# Resource group
az group create --name rg-digitalplus --location brazilsouth

# Storage account (requerido por Functions)
az storage account create \
    --name stdigitalplusfunc \
    --resource-group rg-digitalplus \
    --location brazilsouth \
    --sku Standard_LRS

# Function App (Consumption Plan = gratis)
az functionapp create \
    --name digitalplus-provision \
    --resource-group rg-digitalplus \
    --storage-account stdigitalplusfunc \
    --consumption-plan-location brazilsouth \
    --runtime dotnet-isolated \
    --runtime-version 8 \
    --functions-version 4 \
    --os-type Windows
```

## Paso 3: Configurar App Settings

```bash
az functionapp config appsettings set \
    --name digitalplus-provision \
    --resource-group rg-digitalplus \
    --settings \
    "ProvisioningDb=Server=sd-1985882-l.ferozo.com,11434;Database=DigitalPlus;User Id=sa;Password=Soporte1;Encrypt=True;TrustServerCertificate=True;" \
    "CloudSqlServer=sd-1985882-l.ferozo.com,11434" \
    "CloudSqlUser=sa" \
    "CloudSqlPassword=Soporte1" \
    "ProvisioningApiKey=GENERAR_UNA_KEY_RANDOM_AQUI"
```

> **IMPORTANTE:** Reemplazar `GENERAR_UNA_KEY_RANDOM_AQUI` con una key random (ej: `openssl rand -hex 32`).

## Paso 4: Deploy

```bash
cd AzureProvisioning/src/DigitalPlus.Provisioning
dotnet publish -c Release -o ./publish
cd publish
func azure functionapp publish digitalplus-provision
```

O desde Visual Studio / VS Code con la extension Azure Functions.

## Paso 5: Verificar

```bash
# Health check
curl https://digitalplus-provision.azurewebsites.net/api/health

# Deberia responder: {"status":"healthy","timestamp":"..."}
```

## Paso 6: Generar codigos de activacion

```powershell
cd AzureProvisioning/tools
.\generate-code.ps1 -ExpiryHours 24 -CreatedBy "admin" -Notes "Cliente de prueba"
```

## Paso 7: Probar el endpoint

```bash
curl -X POST https://digitalplus-provision.azurewebsites.net/api/provision \
  -H "Content-Type: application/json" \
  -H "X-Api-Key: TU_API_KEY" \
  -d '{
    "activationCode": "XXXX-XXXX-XXXX-XXXX",
    "companyName": "Mi Empresa S.A.",
    "installType": "cloud",
    "machineId": "TEST-PC"
  }'
```

## Costos

| Recurso | Costo |
|---------|-------|
| Azure Functions Consumption Plan | Gratis (1M req/mes) |
| Storage Account | ~$0.02/mes |
| Application Insights (opcional) | Gratis 5GB/mes |
| Ferozo SQL Server | Ya pagado |
| **Total** | **~$0.02/mes** |

## Endpoints

| Metodo | Ruta | Descripcion |
|--------|------|-------------|
| GET | /api/health | Health check |
| POST | /api/provision | Provisioning (requiere X-Api-Key) |

## Codigos de respuesta del endpoint /provision

| Codigo | Significado |
|--------|-------------|
| 200 | OK - provisioning exitoso |
| 400 | Payload invalido |
| 401 | API key invalida o faltante |
| 403 | Codigo de activacion invalido/expirado/usado |
| 409 | BD ya existe en cloud |
| 500 | Error interno al crear BD |
