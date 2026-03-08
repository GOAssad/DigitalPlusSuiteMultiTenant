# ============================================================
# generate-code.ps1
# Genera un codigo de activacion para el provisioning de DigitalPlus.
# Inserta el hash SHA256 en la tabla ActivationCodes de Ferozo.
#
# Uso:
#   .\generate-code.ps1
#   .\generate-code.ps1 -ExpiryHours 48 -CreatedBy "admin" -Notes "Cliente X"
# ============================================================

param(
    [int]$ExpiryHours = 24,
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

$cmd = $conn.CreateCommand()
$cmd.CommandText = "Provisioning_InsertActivationCode"
$cmd.CommandType = [System.Data.CommandType]::StoredProcedure
$cmd.Parameters.AddWithValue("@CodeHash", $hashHex) | Out-Null
$cmd.Parameters.AddWithValue("@ExpiresAt", (Get-Date).ToUniversalTime().AddHours($ExpiryHours)) | Out-Null
$cmd.Parameters.AddWithValue("@CreatedBy", $CreatedBy) | Out-Null
$cmd.Parameters.AddWithValue("@Notes", $Notes) | Out-Null
$outParam = $cmd.Parameters.Add("@NewId", [System.Data.SqlDbType]::Int)
$outParam.Direction = [System.Data.ParameterDirection]::Output
$cmd.ExecuteNonQuery() | Out-Null

$conn.Close()

Write-Host ""
Write-Host "=== CODIGO DE ACTIVACION GENERADO ===" -ForegroundColor Green
Write-Host "Codigo:     $code" -ForegroundColor Yellow
Write-Host "Expira:     $((Get-Date).ToUniversalTime().AddHours($ExpiryHours).ToString('yyyy-MM-dd HH:mm:ss')) UTC"
Write-Host "ID en BD:   $($outParam.Value)"
Write-Host "Creado por: $CreatedBy"
if ($Notes) { Write-Host "Notas:      $Notes" }
Write-Host ""
Write-Host "Entregue este codigo al cliente. Es de un solo uso y expira en $ExpiryHours horas." -ForegroundColor Cyan
Write-Host ""
