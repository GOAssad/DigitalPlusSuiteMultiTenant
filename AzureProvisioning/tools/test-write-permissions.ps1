$ErrorActionPreference = 'Stop'
$server = "sd-1985882-l.ferozo.com,11434"

$passAdmin = $args[0]
$passWeb = $args[1]

# ============================================================
# Test dp_admin_svc en DigitalPlusAdmin
# ============================================================
Write-Host "=== Test dp_admin_svc en DigitalPlusAdmin ===" -ForegroundColor Cyan

$conn1 = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlusAdmin;User Id=dp_admin_svc;Password=$passAdmin;Encrypt=True;TrustServerCertificate=True;")
$conn1.Open()

# SELECT
$cmd = $conn1.CreateCommand()
$cmd.CommandText = "SELECT COUNT(*) FROM Licencias"
$val = $cmd.ExecuteScalar()
Write-Host "  [OK] SELECT Licencias: $val" -ForegroundColor Green

# INSERT + ROLLBACK
$cmd2 = $conn1.CreateCommand()
$cmd2.CommandText = @"
BEGIN TRAN;
INSERT INTO LicenciasLog (LicenciaId, [Action], Details) VALUES (1, 'test_permissions', 'Verificacion de permisos');
ROLLBACK;
"@
$cmd2.ExecuteNonQuery() | Out-Null
Write-Host "  [OK] INSERT LicenciasLog (revertido)" -ForegroundColor Green

# EXECUTE SP
$cmd3 = $conn1.CreateCommand()
$cmd3.CommandText = "SELECT COUNT(*) FROM sys.procedures"
$spCount = $cmd3.ExecuteScalar()
Write-Host "  [OK] Acceso a SPs: $spCount encontrados" -ForegroundColor Green

$conn1.Close()

# ============================================================
# Test dp_web_svc en DigitalPlus
# ============================================================
Write-Host ""
Write-Host "=== Test dp_web_svc en DigitalPlus ===" -ForegroundColor Cyan

$conn2 = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlus;User Id=dp_web_svc;Password=$passWeb;Encrypt=True;TrustServerCertificate=True;")
$conn2.Open()

# SELECT Legajos
$cmd = $conn2.CreateCommand()
$cmd.CommandText = "SELECT COUNT(*) FROM Legajos"
$val = $cmd.ExecuteScalar()
Write-Host "  [OK] SELECT Legajos: $val" -ForegroundColor Green

# SELECT Identity
$cmd2 = $conn2.CreateCommand()
$cmd2.CommandText = "SELECT COUNT(*) FROM AspNetUsers"
$val2 = $cmd2.ExecuteScalar()
Write-Host "  [OK] SELECT AspNetUsers: $val2" -ForegroundColor Green

# UPDATE + ROLLBACK
$cmd3 = $conn2.CreateCommand()
$cmd3.CommandText = "BEGIN TRAN; UPDATE VariablesGlobales SET Valor = Valor WHERE sId = 'NombreEmpresa'; ROLLBACK;"
$cmd3.ExecuteNonQuery() | Out-Null
Write-Host "  [OK] UPDATE VariablesGlobales (revertido)" -ForegroundColor Green

# DELETE WHERE 1=0
$cmd4 = $conn2.CreateCommand()
$cmd4.CommandText = "DELETE FROM Fichadas WHERE 1=0"
$cmd4.ExecuteNonQuery() | Out-Null
Write-Host "  [OK] DELETE Fichadas (WHERE 1=0)" -ForegroundColor Green

# EXECUTE SP count
$cmd5 = $conn2.CreateCommand()
$cmd5.CommandText = "SELECT COUNT(*) FROM sys.procedures WHERE name LIKE 'Web%'"
$spCount = $cmd5.ExecuteScalar()
Write-Host "  [OK] SPs Web accesibles: $spCount" -ForegroundColor Green

$conn2.Close()

Write-Host ""
Write-Host "=== TODOS LOS TESTS PASARON ===" -ForegroundColor Green
