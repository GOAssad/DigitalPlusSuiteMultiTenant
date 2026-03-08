# ============================================================
# modify-license.ps1
# Modifica plan, legajos o expiracion de una licencia existente.
#
# Uso:
#   .\modify-license.ps1 -LicenciaId 1 -MaxLegajos 50
#   .\modify-license.ps1 -LicenciaId 1 -Plan "premium" -MaxLegajos 100
#   .\modify-license.ps1 -LicenciaId 1 -ExtendDays 365
#   .\modify-license.ps1 -CompanyId "DP_ACME" -MaxLegajos 200
# ============================================================

param(
    [int]$LicenciaId = 0,
    [string]$CompanyId = "",
    [string]$Plan = "",
    [int]$MaxLegajos = 0,
    [int]$ExtendDays = 0,
    [string]$Server = "sd-1985882-l.ferozo.com,11434",
    [string]$Database = "DigitalPlusAdmin",
    [string]$User = "dp_admin_svc",
    [string]$Password = "u#v@rYKSLb#Eu7d6xeYp"
)

$ErrorActionPreference = 'Stop'

if ($LicenciaId -eq 0 -and $CompanyId -eq "") {
    Write-Host "ERROR: Debe especificar -LicenciaId o -CompanyId" -ForegroundColor Red
    return
}

if ($Plan -eq "" -and $MaxLegajos -eq 0 -and $ExtendDays -eq 0) {
    Write-Host "ERROR: Debe especificar al menos uno: -Plan, -MaxLegajos o -ExtendDays" -ForegroundColor Red
    return
}

$connStr = "Server=$Server;Database=$Database;User Id=$User;Password=$Password;Encrypt=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()

# Buscar la licencia
$findSql = "SELECT Id, CompanyId, [Plan], MaxLegajos, ExpiresAt, LicenseType FROM Licencias WHERE "
if ($LicenciaId -gt 0) {
    $findSql += "Id = $LicenciaId"
} else {
    $findSql += "CompanyId = @CompanyId"
}

$findCmd = $conn.CreateCommand()
$findCmd.CommandText = $findSql
if ($CompanyId) { $findCmd.Parameters.AddWithValue("@CompanyId", $CompanyId) | Out-Null }

$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($findCmd)
$dt = New-Object System.Data.DataTable
$adapter.Fill($dt) | Out-Null

if ($dt.Rows.Count -eq 0) {
    Write-Host "ERROR: Licencia no encontrada." -ForegroundColor Red
    $conn.Close()
    return
}

if ($dt.Rows.Count -gt 1) {
    Write-Host "Se encontraron $($dt.Rows.Count) licencias para '$CompanyId'. Use -LicenciaId para especificar:" -ForegroundColor Yellow
    foreach ($r in $dt.Rows) {
        Write-Host "  ID: $($r["Id"])  |  Maquina: (use list-licenses.ps1 para ver)"
    }
    $conn.Close()
    return
}

$row = $dt.Rows[0]
$id = $row["Id"]
Write-Host ""
Write-Host "Licencia encontrada:" -ForegroundColor Cyan
Write-Host "  ID: $id  |  Empresa: $($row["CompanyId"])  |  Tipo: $($row["LicenseType"])"
Write-Host "  Plan actual: $($row["Plan"])  |  Max Legajos: $($row["MaxLegajos"])"
if ($row["ExpiresAt"] -ne [DBNull]::Value) {
    Write-Host "  Expira: $($row["ExpiresAt"].ToString('yyyy-MM-dd HH:mm'))"
}

# Armar UPDATE
$sets = @("UpdatedAt = SYSUTCDATETIME()")
$logDetails = @()

if ($Plan -ne "") {
    $sets += "[Plan] = '$Plan'"
    $logDetails += "Plan: $Plan"
}
if ($MaxLegajos -gt 0) {
    $sets += "MaxLegajos = $MaxLegajos"
    $logDetails += "MaxLegajos: $MaxLegajos"
}
if ($ExtendDays -gt 0) {
    if ($row["ExpiresAt"] -ne [DBNull]::Value) {
        $sets += "ExpiresAt = DATEADD(DAY, $ExtendDays, ExpiresAt)"
    } else {
        $sets += "ExpiresAt = DATEADD(DAY, $ExtendDays, SYSUTCDATETIME())"
    }
    $logDetails += "ExtendDays: $ExtendDays"
}

$updateSql = "UPDATE Licencias SET $($sets -join ', ') WHERE Id = $id"
$cmd = $conn.CreateCommand()
$cmd.CommandText = $updateSql
$cmd.ExecuteNonQuery() | Out-Null

# Log
$logSql = "INSERT INTO LicenciasLog (LicenciaId, [Action], Details) VALUES ($id, 'admin_modify', @Details)"
$logCmd = $conn.CreateCommand()
$logCmd.CommandText = $logSql
$logCmd.Parameters.AddWithValue("@Details", ($logDetails -join ", ")) | Out-Null
$logCmd.ExecuteNonQuery() | Out-Null

$conn.Close()

Write-Host ""
Write-Host "=== LICENCIA MODIFICADA ===" -ForegroundColor Green
if ($Plan -ne "") { Write-Host "  Plan: $Plan" }
if ($MaxLegajos -gt 0) { Write-Host "  Max Legajos: $MaxLegajos" }
if ($ExtendDays -gt 0) { Write-Host "  Extendida: +$ExtendDays dias" }
Write-Host ""
Write-Host "El cambio se aplicara en el proximo heartbeat del cliente (hasta 4 horas)." -ForegroundColor Cyan
Write-Host ""
