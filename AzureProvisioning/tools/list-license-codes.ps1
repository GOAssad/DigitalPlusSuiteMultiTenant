# ============================================================
# list-license-codes.ps1
# Lista los codigos de licencia generados.
#
# Uso:
#   .\list-license-codes.ps1              (solo disponibles)
#   .\list-license-codes.ps1 -ShowAll     (incluye usados y expirados)
# ============================================================

param(
    [switch]$ShowAll,
    [string]$Server = "sd-1985882-l.ferozo.com,11434",
    [string]$Database = "DigitalPlusAdmin",
    [string]$User = "dp_admin_svc",
    [string]$Password = "u#v@rYKSLb#Eu7d6xeYp"
)

$ErrorActionPreference = 'Stop'

$connStr = "Server=$Server;Database=$Database;User Id=$User;Password=$Password;Encrypt=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()

$sql = @"
SELECT Id, LEFT(CodeHash, 8) + '...' AS CodeHash,
       [Plan], MaxLegajos, DurationDays,
       ExpiresAt, UsedAt, UsedByCompany, UsedByMachine,
       CreatedAt, CreatedBy, Notes
FROM LicenseCodes
"@

if (-not $ShowAll) {
    $sql += " WHERE UsedAt IS NULL AND ExpiresAt > SYSUTCDATETIME()"
}
$sql += " ORDER BY CreatedAt DESC"

$cmd = $conn.CreateCommand()
$cmd.CommandText = $sql

$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$dt = New-Object System.Data.DataTable
$adapter.Fill($dt) | Out-Null
$conn.Close()

if ($dt.Rows.Count -eq 0) {
    Write-Host "No se encontraron codigos." -ForegroundColor Yellow
    return
}

$label = if ($ShowAll) { "TODOS" } else { "DISPONIBLES" }
Write-Host ""
Write-Host "=== CODIGOS DE LICENCIA - $label ($($dt.Rows.Count)) ===" -ForegroundColor Green
Write-Host ""

foreach ($row in $dt.Rows) {
    $used = $row["UsedAt"] -ne [DBNull]::Value
    $expired = ($row["ExpiresAt"] -lt (Get-Date).ToUniversalTime()) -and (-not $used)
    $color = if ($used) { "DarkGray" } elseif ($expired) { "Red" } else { "Green" }

    $status = if ($used) { "USADO" } elseif ($expired) { "EXPIRADO" } else { "DISPONIBLE" }

    Write-Host "ID: $($row["Id"])  |  Hash: $($row["CodeHash"])  |  [$status]" -ForegroundColor $color
    Write-Host "  Plan: $($row["Plan"])  |  Max Legajos: $($row["MaxLegajos"])  |  Duracion: $($row["DurationDays"]) dias"
    Write-Host "  Codigo expira: $($row["ExpiresAt"].ToString('yyyy-MM-dd HH:mm'))"

    if ($used) {
        Write-Host "  Usado por: $($row["UsedByCompany"])  |  Maquina: $($row["UsedByMachine"])"
        Write-Host "  Fecha uso: $($row["UsedAt"].ToString('yyyy-MM-dd HH:mm'))"
    }

    if ($row["CreatedBy"] -ne [DBNull]::Value) {
        Write-Host "  Creado por: $($row["CreatedBy"])  |  $($row["CreatedAt"].ToString('yyyy-MM-dd HH:mm'))"
    }
    if ($row["Notes"] -ne [DBNull]::Value -and $row["Notes"] -ne "") {
        Write-Host "  Notas: $($row["Notes"])"
    }
    Write-Host ""
}
