$ErrorActionPreference = 'Stop'
$server = "sd-1985882-l.ferozo.com,11434"
$sa = "sa"
$saPass = "Soporte1"

# Generar password seguro (20 chars)
$chars = 'abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789'
$specials = '!@#$%'
$rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider
$bytes = New-Object byte[] 1
$pwd = ''
for ($i = 0; $i -lt 17; $i++) {
    $rng.GetBytes($bytes)
    $pwd += $chars[$bytes[0] % $chars.Length]
}
for ($i = 0; $i -lt 3; $i++) {
    $rng.GetBytes($bytes)
    $pos = $bytes[0] % $pwd.Length
    $rng.GetBytes($bytes)
    $sp = $specials[$bytes[0] % $specials.Length]
    $pwd = $pwd.Insert($pos, [string]$sp)
}

Write-Host "=== PASSWORD GENERADO ===" -ForegroundColor Cyan
Write-Host "  dp_app_svc: $pwd" -ForegroundColor Yellow
Write-Host ""

# 1. Crear login en master
Write-Host "=== Crear login dp_app_svc en master ===" -ForegroundColor Cyan
$connMaster = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=master;User Id=$sa;Password=$saPass;Encrypt=True;TrustServerCertificate=True;")
$connMaster.Open()
$cmd = $connMaster.CreateCommand()
$cmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'dp_app_svc') CREATE LOGIN [dp_app_svc] WITH PASSWORD = '$pwd', DEFAULT_DATABASE = [DigitalPlusMultiTenant], CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;"
$cmd.ExecuteNonQuery() | Out-Null
Write-Host "[OK] Login dp_app_svc creado" -ForegroundColor Green
$connMaster.Close()

# 2. Crear rol y user en DigitalPlusMultiTenant
Write-Host ""
Write-Host "=== Crear rol dp_role_app y user dp_app_svc en DigitalPlusMultiTenant ===" -ForegroundColor Cyan
$connMT = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlusMultiTenant;User Id=$sa;Password=$saPass;Encrypt=True;TrustServerCertificate=True;")
$connMT.Open()

# Crear rol
$cmd = $connMT.CreateCommand()
$cmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_app' AND type = 'R') CREATE ROLE dp_role_app;"
$cmd.ExecuteNonQuery() | Out-Null
Write-Host "[OK] Rol dp_role_app creado" -ForegroundColor Green

# SELECT en TODAS las tablas
$allTables = @('Categoria','Empresa','EventoCalendario','Feriado','Fichada','Horario','HorarioDetalle','Incidencia','IncidenciaLegajo','Legajo','LegajoDomicilio','LegajoHuella','LegajoPin','LegajoSucursal','Noticia','Sector','Sucursal','Terminal','UsuarioSucursal','Vacacion','VariableSistema','AspNetUsers','AspNetRoles','AspNetUserRoles','AspNetUserClaims','AspNetUserLogins','AspNetUserTokens','AspNetRoleClaims','__EFMigrationsHistory')
foreach ($t in $allTables) {
    $cmd = $connMT.CreateCommand()
    $cmd.CommandText = "GRANT SELECT ON dbo.[$t] TO dp_role_app;"
    $cmd.ExecuteNonQuery() | Out-Null
}
Write-Host "[OK] SELECT en todas las tablas" -ForegroundColor Green

# INSERT en tablas operativas
$insertTables = @('Fichada','Legajo','LegajoHuella','LegajoPin','LegajoSucursal','LegajoDomicilio','Horario','HorarioDetalle','Incidencia','IncidenciaLegajo','Feriado','Noticia','Vacacion','Categoria','Sector','Sucursal','UsuarioSucursal','Terminal','VariableSistema','EventoCalendario')
foreach ($t in $insertTables) {
    $cmd = $connMT.CreateCommand()
    $cmd.CommandText = "GRANT INSERT ON dbo.[$t] TO dp_role_app;"
    $cmd.ExecuteNonQuery() | Out-Null
}
Write-Host "[OK] INSERT en tablas operativas" -ForegroundColor Green

# UPDATE en tablas operativas
foreach ($t in $insertTables) {
    $cmd = $connMT.CreateCommand()
    $cmd.CommandText = "GRANT UPDATE ON dbo.[$t] TO dp_role_app;"
    $cmd.ExecuteNonQuery() | Out-Null
}
Write-Host "[OK] UPDATE en tablas operativas" -ForegroundColor Green

# DELETE restringido
$deleteTables = @('LegajoHuella','LegajoPin','Fichada','IncidenciaLegajo','LegajoSucursal','LegajoDomicilio','HorarioDetalle','UsuarioSucursal','Vacacion','Feriado','EventoCalendario')
foreach ($t in $deleteTables) {
    $cmd = $connMT.CreateCommand()
    $cmd.CommandText = "GRANT DELETE ON dbo.[$t] TO dp_role_app;"
    $cmd.ExecuteNonQuery() | Out-Null
}
Write-Host "[OK] DELETE en tablas seleccionadas" -ForegroundColor Green

# EXECUTE en todos los SPs
$sps = @('Categoria_Delete','Categoria_SP','ConvertiraFecha','DiaDeSemana','EscritorioFichadasSPSALIDA','EscritorioLegajoActualizar','EscritorioLegajoPIN_Cambiar','EscritorioLegajoPIN_Verificar','EscritorioLegajosActivos_Lista','EscritorioLegajosHuellasActualizar','Feriado_Delete','Feriado_SP','Horario_SP','HorarioDetalle_SP','Incidencia_Delete','Incidencia_SP','PrimeraEntrada','RRHHFichadas_SP_ELIMINAR','RRHHFichadas_SP_LISTADO','RRHHFichadas_SP_MANUAL','RRHHFichadas_SP_MANUAL_SELECT','RRHHFichadas_SP_MANUAL_SELECT_GRUPO','RRHHFichadasAusencias_SP_SELECT','RRHHFichadasAusencias_x_Legajo_SP_SELECT','RRHHFichadasEntradaEstatus_SP_SELECT','RRHHFichadasEntradaEstatusLegajo_SP_SELECT','RRHHIncidenciasLegajos_Delete','RRHHLegajos_DeleteTodo','RRHHVacaciones_SP_DELETE','RRHHVacaciones_SP_INSERT_LEGAJO','RRHHVacaciones_SP_SELECT_LEGAJO','Sucursal_Delete','Sucursal_SP','Terminal_Delete','Terminal_SP','UltimaSalida')
foreach ($sp in $sps) {
    $cmd = $connMT.CreateCommand()
    $cmd.CommandText = "GRANT EXECUTE ON dbo.[$sp] TO dp_role_app;"
    $cmd.ExecuteNonQuery() | Out-Null
}
Write-Host "[OK] EXECUTE en todos los SPs" -ForegroundColor Green

# Crear user y asignar rol
$cmd = $connMT.CreateCommand()
$cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_app_svc')
    CREATE USER [dp_app_svc] FOR LOGIN [dp_app_svc];
ALTER ROLE dp_role_app ADD MEMBER [dp_app_svc];
"@
$cmd.ExecuteNonQuery() | Out-Null
Write-Host "[OK] User dp_app_svc asignado a dp_role_app" -ForegroundColor Green
$connMT.Close()

# 3. Verificacion: conectar con dp_app_svc y leer datos
Write-Host ""
Write-Host "=== Verificacion ===" -ForegroundColor Cyan
$connTest = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlusMultiTenant;User Id=dp_app_svc;Password=$pwd;Encrypt=True;TrustServerCertificate=True;")
$connTest.Open()
$cmd = $connTest.CreateCommand()
$cmd.CommandText = "SELECT COUNT(*) FROM Legajo"
$count = $cmd.ExecuteScalar()
Write-Host "[OK] dp_app_svc puede leer Legajo: $count registros" -ForegroundColor Green

$cmd2 = $connTest.CreateCommand()
$cmd2.CommandText = "SELECT COUNT(*) FROM Fichada"
$count2 = $cmd2.ExecuteScalar()
Write-Host "[OK] dp_app_svc puede leer Fichada: $count2 registros" -ForegroundColor Green
$connTest.Close()

Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "dp_app_svc CREADO EXITOSAMENTE" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host "  Password: $pwd" -ForegroundColor Yellow
Write-Host ""
Write-Host "Connection string para CloudSql:" -ForegroundColor Cyan
Write-Host "  Server=$server;Database=DigitalPlusMultiTenant;User Id=dp_app_svc;Password=$pwd;Encrypt=True;TrustServerCertificate=True;" -ForegroundColor White
