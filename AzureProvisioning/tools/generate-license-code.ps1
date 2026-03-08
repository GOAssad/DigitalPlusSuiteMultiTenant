# ============================================================
# generate-license-code.ps1
# Genera un codigo de activacion de LICENCIA para DigitalPlus.
# El codigo define plan, cantidad de legajos y duracion.
#
# Uso:
#   .\generate-license-code.ps1
#   .\generate-license-code.ps1 -Plan "premium" -MaxLegajos 50 -DurationDays 365 -Notes "Cliente ACME"
#   .\generate-license-code.ps1 -Plan "basic" -MaxLegajos 25 -CodeExpiryHours 72 -CreatedBy "gustavo"
# ============================================================

param(
    [string]$Plan = "basic",
    [int]$MaxLegajos = 25,
    [int]$DurationDays = 365,
    [int]$CodeExpiryHours = 168,       # 7 dias para que el cliente use el codigo
    [string]$CreatedBy = "admin",
    [string]$Notes = "",
    [string]$Server = "sd-1985882-l.ferozo.com,11434",
    [string]$Database = "DigitalPlusAdmin",
    [string]$User = "dp_admin_svc",
    [string]$Password = "u#v@rYKSLb#Eu7d6xeYp"
)

$ErrorActionPreference = 'Stop'

# Generar codigo random (XXXX-XXXX-XXXX-XXXX)
$bytes = New-Object byte[] 16
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rng.GetBytes($bytes)
$hex = [BitConverter]::ToString($bytes).Replace("-","").ToUpper()
$code = "$($hex.Substring(0,4))-$($hex.Substring(4,4))-$($hex.Substring(8,4))-$($hex.Substring(12,4))"

# Calcular SHA256
$sha256 = [System.Security.Cryptography.SHA256]::Create()
$hashBytes = $sha256.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($code))
$hashHex = [BitConverter]::ToString($hashBytes).Replace("-","").ToLower()

# Insertar en BD
$connStr = "Server=$Server;Database=$Database;User Id=$User;Password=$Password;Encrypt=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()

$expiresAt = (Get-Date).ToUniversalTime().AddHours($CodeExpiryHours)

$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
INSERT INTO LicenseCodes (CodeHash, [Plan], MaxLegajos, DurationDays, ExpiresAt, CreatedBy, Notes)
VALUES (@CodeHash, @Plan, @MaxLegajos, @DurationDays, @ExpiresAt, @CreatedBy, @Notes);
SELECT SCOPE_IDENTITY();
"@
$cmd.Parameters.AddWithValue("@CodeHash", $hashHex) | Out-Null
$cmd.Parameters.AddWithValue("@Plan", $Plan) | Out-Null
$cmd.Parameters.AddWithValue("@MaxLegajos", $MaxLegajos) | Out-Null
$cmd.Parameters.AddWithValue("@DurationDays", $DurationDays) | Out-Null
$cmd.Parameters.AddWithValue("@ExpiresAt", $expiresAt) | Out-Null
$cmd.Parameters.AddWithValue("@CreatedBy", $CreatedBy) | Out-Null
$cmd.Parameters.AddWithValue("@Notes", $Notes) | Out-Null

$newId = $cmd.ExecuteScalar()
$conn.Close()

Write-Host ""
Write-Host "=== CODIGO DE LICENCIA GENERADO ===" -ForegroundColor Green
Write-Host "Codigo:       $code" -ForegroundColor Yellow
Write-Host "Plan:         $Plan"
Write-Host "Max Legajos:  $MaxLegajos"
Write-Host "Duracion:     $DurationDays dias"
Write-Host "Codigo expira: $($expiresAt.ToString('yyyy-MM-dd HH:mm:ss')) UTC ($CodeExpiryHours hs para usarlo)"
Write-Host "ID en BD:     $newId"
Write-Host "Creado por:   $CreatedBy"
if ($Notes) { Write-Host "Notas:        $Notes" }
Write-Host ""
Write-Host "Entregue este codigo al cliente. Es de un solo uso." -ForegroundColor Cyan
Write-Host "Al activarlo, su licencia sera: $Plan con $MaxLegajos legajos por $DurationDays dias." -ForegroundColor Cyan
Write-Host ""
