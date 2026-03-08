# ============================================================
# list-licenses.ps1
# Lista todas las licencias activas en DigitalPlusAdmin.
#
# Uso:
#   .\list-licenses.ps1
#   .\list-licenses.ps1 -ShowAll       (incluye suspendidas y expiradas)
#   .\list-licenses.ps1 -CompanyId "DP_ACME"
# ============================================================

param(
    [switch]$ShowAll,
    [string]$CompanyId = "",
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
SELECT Id, CompanyId, MachineId, LicenseType, [Plan], MaxLegajos,
       TrialEndsAt, ExpiresAt, SuspendedAt, LastHeartbeat, CreatedAt
FROM Licencias
WHERE 1=1
"@

if (-not $ShowAll) {
    $sql += " AND LicenseType != 'suspended'"
}
if ($CompanyId) {
    $sql += " AND CompanyId = @CompanyId"
}
$sql += " ORDER BY CreatedAt DESC"

$cmd = $conn.CreateCommand()
$cmd.CommandText = $sql
if ($CompanyId) {
    $cmd.Parameters.AddWithValue("@CompanyId", $CompanyId) | Out-Null
}

$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$dt = New-Object System.Data.DataTable
$adapter.Fill($dt) | Out-Null
$conn.Close()

if ($dt.Rows.Count -eq 0) {
    Write-Host "No se encontraron licencias." -ForegroundColor Yellow
    return
}

Write-Host ""
Write-Host "=== LICENCIAS ($($dt.Rows.Count)) ===" -ForegroundColor Green
Write-Host ""

foreach ($row in $dt.Rows) {
    $type = $row["LicenseType"]
    $color = switch ($type) {
        "active"    { "Green" }
        "trial"     { "Yellow" }
        "suspended" { "Red" }
        default     { "White" }
    }

    Write-Host "ID: $($row["Id"])  |  $($row["CompanyId"])  |  $($row["MachineId"])" -ForegroundColor $color
    Write-Host "  Tipo: $type  |  Plan: $($row["Plan"])  |  Max Legajos: $($row["MaxLegajos"])"

    if ($type -eq "trial" -and $row["TrialEndsAt"] -ne [DBNull]::Value) {
        Write-Host "  Trial expira: $($row["TrialEndsAt"].ToString('yyyy-MM-dd HH:mm'))"
    }
    if ($row["ExpiresAt"] -ne [DBNull]::Value) {
        Write-Host "  Licencia expira: $($row["ExpiresAt"].ToString('yyyy-MM-dd HH:mm'))"
    }
    if ($row["SuspendedAt"] -ne [DBNull]::Value) {
        Write-Host "  Suspendida: $($row["SuspendedAt"].ToString('yyyy-MM-dd HH:mm'))" -ForegroundColor Red
    }
    if ($row["LastHeartbeat"] -ne [DBNull]::Value) {
        Write-Host "  Ultimo heartbeat: $($row["LastHeartbeat"].ToString('yyyy-MM-dd HH:mm'))"
    }
    Write-Host "  Creada: $($row["CreatedAt"].ToString('yyyy-MM-dd HH:mm'))"
    Write-Host ""
}
