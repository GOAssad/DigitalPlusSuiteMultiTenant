# ============================================================
# suspend-license.ps1
# Suspende o reactiva una licencia.
#
# Uso:
#   .\suspend-license.ps1 -LicenciaId 1                    (suspender)
#   .\suspend-license.ps1 -LicenciaId 1 -Reactivate        (reactivar)
#   .\suspend-license.ps1 -CompanyId "DP_ACME"             (suspender por empresa)
# ============================================================

param(
    [int]$LicenciaId = 0,
    [string]$CompanyId = "",
    [switch]$Reactivate,
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

$connStr = "Server=$Server;Database=$Database;User Id=$User;Password=$Password;Encrypt=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()

# Buscar
$where = if ($LicenciaId -gt 0) { "Id = $LicenciaId" } else { "CompanyId = @CompanyId" }
$findCmd = $conn.CreateCommand()
$findCmd.CommandText = "SELECT Id, CompanyId, LicenseType, SuspendedAt FROM Licencias WHERE $where"
if ($CompanyId) { $findCmd.Parameters.AddWithValue("@CompanyId", $CompanyId) | Out-Null }

$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($findCmd)
$dt = New-Object System.Data.DataTable
$adapter.Fill($dt) | Out-Null

if ($dt.Rows.Count -eq 0) {
    Write-Host "ERROR: Licencia no encontrada." -ForegroundColor Red
    $conn.Close()
    return
}

foreach ($row in $dt.Rows) {
    $id = $row["Id"]
    $company = $row["CompanyId"]

    if ($Reactivate) {
        $sql = "UPDATE Licencias SET LicenseType = 'active', SuspendedAt = NULL, GraceEndsAt = NULL, UpdatedAt = SYSUTCDATETIME() WHERE Id = $id"
        $action = "admin_reactivate"
        $label = "REACTIVADA"
        $color = "Green"
    } else {
        $now = "SYSUTCDATETIME()"
        $sql = "UPDATE Licencias SET LicenseType = 'suspended', SuspendedAt = $now, GraceEndsAt = DATEADD(DAY, 7, $now), UpdatedAt = $now WHERE Id = $id"
        $action = "admin_suspend"
        $label = "SUSPENDIDA"
        $color = "Red"
    }

    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $sql
    $cmd.ExecuteNonQuery() | Out-Null

    # Log
    $logCmd = $conn.CreateCommand()
    $logCmd.CommandText = "INSERT INTO LicenciasLog (LicenciaId, [Action], Details) VALUES ($id, '$action', 'Manual admin action')"
    $logCmd.ExecuteNonQuery() | Out-Null

    Write-Host "Licencia ID $id ($company): $label" -ForegroundColor $color
}

$conn.Close()
Write-Host ""
Write-Host "El cambio se aplicara en el proximo heartbeat del cliente (hasta 4 horas)." -ForegroundColor Cyan
if (-not $Reactivate) {
    Write-Host "El cliente tiene 7 dias de gracia antes del bloqueo total." -ForegroundColor Yellow
}
Write-Host ""
