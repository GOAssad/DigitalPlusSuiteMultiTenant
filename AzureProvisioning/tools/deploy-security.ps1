$ErrorActionPreference = 'Stop'
$server = "sd-1985882-l.ferozo.com,11434"
$user = "sa"
$pass = "Soporte1"

# Generar passwords seguros (20 chars)
function New-SecurePassword {
    $chars = 'abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789'
    $specials = '!@#$%'
    $pwd = ''
    $rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider
    $bytes = New-Object byte[] 1
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
    return $pwd
}

$passAdmin = New-SecurePassword
$passWeb = New-SecurePassword

Write-Host "=== PASSWORDS GENERADOS ===" -ForegroundColor Cyan
Write-Host "  dp_admin_svc: $passAdmin"
Write-Host "  dp_web_svc:   $passWeb"
Write-Host ""

# ============================================================
# 011: Crear roles en DigitalPlusAdmin
# ============================================================
Write-Host "=== 011: Roles en DigitalPlusAdmin ===" -ForegroundColor Cyan
$connAdmin = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlusAdmin;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$connAdmin.Open()

$cmd = $connAdmin.CreateCommand()
$cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_admin' AND type = 'R')
    CREATE ROLE dp_role_admin;

GRANT SELECT ON dbo.ActivationCodes TO dp_role_admin;
GRANT SELECT ON dbo.Licencias TO dp_role_admin;
GRANT SELECT ON dbo.LicenciasLog TO dp_role_admin;
GRANT SELECT ON dbo.LicenseCodes TO dp_role_admin;
GRANT INSERT ON dbo.ActivationCodes TO dp_role_admin;
GRANT INSERT ON dbo.Licencias TO dp_role_admin;
GRANT INSERT ON dbo.LicenciasLog TO dp_role_admin;
GRANT INSERT ON dbo.LicenseCodes TO dp_role_admin;
GRANT UPDATE ON dbo.ActivationCodes TO dp_role_admin;
GRANT UPDATE ON dbo.Licencias TO dp_role_admin;
GRANT UPDATE ON dbo.LicenseCodes TO dp_role_admin;
GRANT DELETE ON dbo.LicenciasLog TO dp_role_admin;
GRANT EXECUTE ON dbo.License_Activate TO dp_role_admin;
GRANT EXECUTE ON dbo.License_Heartbeat TO dp_role_admin;
GRANT EXECUTE ON dbo.License_ValidateAndConsumeCode TO dp_role_admin;
GRANT EXECUTE ON dbo.Provisioning_InsertActivationCode TO dp_role_admin;
GRANT EXECUTE ON dbo.Provisioning_ListActivationCodes TO dp_role_admin;
GRANT EXECUTE ON dbo.Provisioning_ValidateAndConsumeCode TO dp_role_admin;
"@
$cmd.ExecuteNonQuery() | Out-Null
Write-Host "[OK] Rol dp_role_admin creado" -ForegroundColor Green
$connAdmin.Close()

# ============================================================
# 012: Crear login dp_admin_svc
# ============================================================
Write-Host ""
Write-Host "=== 012: Login dp_admin_svc ===" -ForegroundColor Cyan

$connMaster = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=master;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$connMaster.Open()
$cmd = $connMaster.CreateCommand()
$cmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'dp_admin_svc') CREATE LOGIN [dp_admin_svc] WITH PASSWORD = '$passAdmin', DEFAULT_DATABASE = [DigitalPlusAdmin], CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;"
$cmd.ExecuteNonQuery() | Out-Null
Write-Host "[OK] Login dp_admin_svc creado" -ForegroundColor Green
$connMaster.Close()

$connAdmin2 = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlusAdmin;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$connAdmin2.Open()
$cmd = $connAdmin2.CreateCommand()
$cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_admin_svc')
    CREATE USER [dp_admin_svc] FOR LOGIN [dp_admin_svc];
ALTER ROLE dp_role_admin ADD MEMBER [dp_admin_svc];
"@
$cmd.ExecuteNonQuery() | Out-Null
Write-Host "[OK] User dp_admin_svc asignado a dp_role_admin" -ForegroundColor Green
$connAdmin2.Close()

# ============================================================
# 013: Crear roles en DigitalPlus
# ============================================================
Write-Host ""
Write-Host "=== 013: Roles en DigitalPlus ===" -ForegroundColor Cyan

$connDP = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlus;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$connDP.Open()

$blocks = @(
    "IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_web' AND type = 'R') CREATE ROLE dp_role_web;"

    # SELECT
    "GRANT SELECT ON dbo.Categorias TO dp_role_web; GRANT SELECT ON dbo.Dedos TO dp_role_web; GRANT SELECT ON dbo.Dias TO dp_role_web; GRANT SELECT ON dbo.Feriados TO dp_role_web; GRANT SELECT ON dbo.Fichadas TO dp_role_web; GRANT SELECT ON dbo.GRALUsuarios TO dp_role_web; GRANT SELECT ON dbo.Horarios TO dp_role_web; GRANT SELECT ON dbo.HorariosDias TO dp_role_web; GRANT SELECT ON dbo.HorariosDiasEventos TO dp_role_web; GRANT SELECT ON dbo.Incidencias TO dp_role_web; GRANT SELECT ON dbo.IncidenciasLegajos TO dp_role_web; GRANT SELECT ON dbo.Legajos TO dp_role_web; GRANT SELECT ON dbo.LegajosHuellas TO dp_role_web; GRANT SELECT ON dbo.LegajosSucursales TO dp_role_web; GRANT SELECT ON dbo.Noticias TO dp_role_web; GRANT SELECT ON dbo.Sectores TO dp_role_web; GRANT SELECT ON dbo.Sucursales TO dp_role_web; GRANT SELECT ON dbo.Terminales TO dp_role_web; GRANT SELECT ON dbo.UsuariosSucursales TO dp_role_web; GRANT SELECT ON dbo.Vacaciones TO dp_role_web; GRANT SELECT ON dbo.VariablesGlobales TO dp_role_web; GRANT SELECT ON dbo.AspNetUsers TO dp_role_web; GRANT SELECT ON dbo.AspNetRoles TO dp_role_web; GRANT SELECT ON dbo.AspNetUserRoles TO dp_role_web; GRANT SELECT ON dbo.AspNetUserClaims TO dp_role_web; GRANT SELECT ON dbo.AspNetUserLogins TO dp_role_web; GRANT SELECT ON dbo.AspNetUserTokens TO dp_role_web; GRANT SELECT ON dbo.AspNetRoleClaims TO dp_role_web; GRANT SELECT ON dbo.__EFMigrationsHistory TO dp_role_web;"

    # INSERT
    "GRANT INSERT ON dbo.Fichadas TO dp_role_web; GRANT INSERT ON dbo.Legajos TO dp_role_web; GRANT INSERT ON dbo.LegajosHuellas TO dp_role_web; GRANT INSERT ON dbo.LegajosSucursales TO dp_role_web; GRANT INSERT ON dbo.Horarios TO dp_role_web; GRANT INSERT ON dbo.HorariosDias TO dp_role_web; GRANT INSERT ON dbo.HorariosDiasEventos TO dp_role_web; GRANT INSERT ON dbo.Incidencias TO dp_role_web; GRANT INSERT ON dbo.IncidenciasLegajos TO dp_role_web; GRANT INSERT ON dbo.Feriados TO dp_role_web; GRANT INSERT ON dbo.Noticias TO dp_role_web; GRANT INSERT ON dbo.Vacaciones TO dp_role_web; GRANT INSERT ON dbo.Categorias TO dp_role_web; GRANT INSERT ON dbo.Sectores TO dp_role_web; GRANT INSERT ON dbo.Sucursales TO dp_role_web; GRANT INSERT ON dbo.UsuariosSucursales TO dp_role_web; GRANT INSERT ON dbo.GRALUsuarios TO dp_role_web; GRANT INSERT ON dbo.Terminales TO dp_role_web; GRANT INSERT ON dbo.VariablesGlobales TO dp_role_web; GRANT INSERT ON dbo.AspNetUsers TO dp_role_web; GRANT INSERT ON dbo.AspNetRoles TO dp_role_web; GRANT INSERT ON dbo.AspNetUserRoles TO dp_role_web; GRANT INSERT ON dbo.AspNetUserClaims TO dp_role_web; GRANT INSERT ON dbo.AspNetUserLogins TO dp_role_web; GRANT INSERT ON dbo.AspNetUserTokens TO dp_role_web; GRANT INSERT ON dbo.AspNetRoleClaims TO dp_role_web; GRANT INSERT ON dbo.__EFMigrationsHistory TO dp_role_web;"

    # UPDATE
    "GRANT UPDATE ON dbo.Fichadas TO dp_role_web; GRANT UPDATE ON dbo.Legajos TO dp_role_web; GRANT UPDATE ON dbo.LegajosHuellas TO dp_role_web; GRANT UPDATE ON dbo.LegajosSucursales TO dp_role_web; GRANT UPDATE ON dbo.Horarios TO dp_role_web; GRANT UPDATE ON dbo.HorariosDias TO dp_role_web; GRANT UPDATE ON dbo.HorariosDiasEventos TO dp_role_web; GRANT UPDATE ON dbo.Incidencias TO dp_role_web; GRANT UPDATE ON dbo.IncidenciasLegajos TO dp_role_web; GRANT UPDATE ON dbo.Feriados TO dp_role_web; GRANT UPDATE ON dbo.Noticias TO dp_role_web; GRANT UPDATE ON dbo.Vacaciones TO dp_role_web; GRANT UPDATE ON dbo.Categorias TO dp_role_web; GRANT UPDATE ON dbo.Sectores TO dp_role_web; GRANT UPDATE ON dbo.Sucursales TO dp_role_web; GRANT UPDATE ON dbo.UsuariosSucursales TO dp_role_web; GRANT UPDATE ON dbo.GRALUsuarios TO dp_role_web; GRANT UPDATE ON dbo.Terminales TO dp_role_web; GRANT UPDATE ON dbo.VariablesGlobales TO dp_role_web; GRANT UPDATE ON dbo.AspNetUsers TO dp_role_web; GRANT UPDATE ON dbo.AspNetRoles TO dp_role_web; GRANT UPDATE ON dbo.AspNetUserRoles TO dp_role_web; GRANT UPDATE ON dbo.AspNetUserClaims TO dp_role_web; GRANT UPDATE ON dbo.AspNetUserLogins TO dp_role_web; GRANT UPDATE ON dbo.AspNetUserTokens TO dp_role_web; GRANT UPDATE ON dbo.AspNetRoleClaims TO dp_role_web;"

    # DELETE restringido
    "GRANT DELETE ON dbo.LegajosHuellas TO dp_role_web; GRANT DELETE ON dbo.Fichadas TO dp_role_web; GRANT DELETE ON dbo.IncidenciasLegajos TO dp_role_web; GRANT DELETE ON dbo.LegajosSucursales TO dp_role_web; GRANT DELETE ON dbo.HorariosDias TO dp_role_web; GRANT DELETE ON dbo.HorariosDiasEventos TO dp_role_web; GRANT DELETE ON dbo.UsuariosSucursales TO dp_role_web; GRANT DELETE ON dbo.Vacaciones TO dp_role_web; GRANT DELETE ON dbo.Feriados TO dp_role_web; GRANT DELETE ON dbo.AspNetUserRoles TO dp_role_web; GRANT DELETE ON dbo.AspNetUserClaims TO dp_role_web; GRANT DELETE ON dbo.AspNetUserLogins TO dp_role_web; GRANT DELETE ON dbo.AspNetUserTokens TO dp_role_web; GRANT DELETE ON dbo.AspNetRoleClaims TO dp_role_web;"

    # EXECUTE SPs
    "GRANT EXECUTE ON dbo.EscritorioFichadasSPSALIDA TO dp_role_web; GRANT EXECUTE ON dbo.EscritorioLegajoActualizar TO dp_role_web; GRANT EXECUTE ON dbo.EscritorioLegajosHuellasActualizar TO dp_role_web; GRANT EXECUTE ON dbo.RRHHLegajos_DeleteTodo TO dp_role_web; GRANT EXECUTE ON dbo.WebAusencias_Listado_General TO dp_role_web; GRANT EXECUTE ON dbo.WebAusencias_Listado_General_ConSucursales TO dp_role_web; GRANT EXECUTE ON dbo.WebCalculoMinutosMensualesCalendarioPorLegajo TO dp_role_web; GRANT EXECUTE ON dbo.WebConsolidado_Listado TO dp_role_web; GRANT EXECUTE ON dbo.WebControlAcceso_Listado TO dp_role_web; GRANT EXECUTE ON dbo.WebDashBoardMinutosTrabajadosMensualesPorLegajo TO dp_role_web; GRANT EXECUTE ON dbo.WebDashBoardMinutosTrabajadosMensualesPorSucursal TO dp_role_web; GRANT EXECUTE ON dbo.WebHorasExtras_Listado TO dp_role_web; GRANT EXECUTE ON dbo.WebLlegadaTarde_Listado TO dp_role_web; GRANT EXECUTE ON dbo.WebLlegadaTarde_Listado_General TO dp_role_web;"
)

foreach ($block in $blocks) {
    $cmd = $connDP.CreateCommand()
    $cmd.CommandText = $block
    $cmd.ExecuteNonQuery() | Out-Null
}
Write-Host "[OK] Rol dp_role_web creado con permisos granulares" -ForegroundColor Green
$connDP.Close()

# ============================================================
# 014: Crear login dp_web_svc
# ============================================================
Write-Host ""
Write-Host "=== 014: Login dp_web_svc ===" -ForegroundColor Cyan

$connMaster2 = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=master;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$connMaster2.Open()
$cmd = $connMaster2.CreateCommand()
$cmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'dp_web_svc') CREATE LOGIN [dp_web_svc] WITH PASSWORD = '$passWeb', DEFAULT_DATABASE = [DigitalPlus], CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;"
$cmd.ExecuteNonQuery() | Out-Null
Write-Host "[OK] Login dp_web_svc creado" -ForegroundColor Green
$connMaster2.Close()

$connDP2 = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlus;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$connDP2.Open()
$cmd = $connDP2.CreateCommand()
$cmd.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dp_web_svc')
    CREATE USER [dp_web_svc] FOR LOGIN [dp_web_svc];
ALTER ROLE dp_role_web ADD MEMBER [dp_web_svc];
"@
$cmd.ExecuteNonQuery() | Out-Null
Write-Host "[OK] User dp_web_svc asignado a dp_role_web" -ForegroundColor Green
$connDP2.Close()

# ============================================================
# 016: Verificacion
# ============================================================
Write-Host ""
Write-Host "=== 016: Verificacion ===" -ForegroundColor Cyan

# DigitalPlusAdmin
$cv1 = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlusAdmin;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$cv1.Open()
foreach ($check in @(
    @("SELECT 1 FROM sys.database_principals WHERE name = 'dp_admin_svc'", "User dp_admin_svc en DigitalPlusAdmin"),
    @("SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_admin' AND type = 'R'", "Rol dp_role_admin en DigitalPlusAdmin"),
    @("SELECT IS_ROLEMEMBER('dp_role_admin', 'dp_admin_svc')", "dp_admin_svc miembro de dp_role_admin")
)) {
    $cmd = $cv1.CreateCommand(); $cmd.CommandText = $check[0]
    $val = $cmd.ExecuteScalar()
    if ($val -eq 1) { Write-Host "  [OK] $($check[1])" -ForegroundColor Green }
    else { Write-Host "  [FALLO] $($check[1])" -ForegroundColor Red }
}
$cv1.Close()

# DigitalPlus
$cv2 = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=DigitalPlus;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$cv2.Open()
foreach ($check in @(
    @("SELECT 1 FROM sys.database_principals WHERE name = 'dp_web_svc'", "User dp_web_svc en DigitalPlus"),
    @("SELECT 1 FROM sys.database_principals WHERE name = 'dp_role_web' AND type = 'R'", "Rol dp_role_web en DigitalPlus"),
    @("SELECT IS_ROLEMEMBER('dp_role_web', 'dp_web_svc')", "dp_web_svc miembro de dp_role_web")
)) {
    $cmd = $cv2.CreateCommand(); $cmd.CommandText = $check[0]
    $val = $cmd.ExecuteScalar()
    if ($val -eq 1) { Write-Host "  [OK] $($check[1])" -ForegroundColor Green }
    else { Write-Host "  [FALLO] $($check[1])" -ForegroundColor Red }
}
$cv2.Close()

# Logins en master
$cv3 = New-Object System.Data.SqlClient.SqlConnection("Server=$server;Database=master;User Id=$user;Password=$pass;Encrypt=True;TrustServerCertificate=True;")
$cv3.Open()
$cmd = $cv3.CreateCommand()
$cmd.CommandText = "SELECT name, is_disabled, default_database_name, CASE WHEN is_policy_checked=1 THEN 'ON' ELSE 'OFF' END AS CheckPolicy FROM sys.sql_logins WHERE name IN ('dp_admin_svc','dp_web_svc')"
$reader = $cmd.ExecuteReader()
Write-Host ""
Write-Host "  Logins:" -ForegroundColor Cyan
while ($reader.Read()) {
    Write-Host "    $($reader['name']) | Disabled=$($reader['is_disabled']) | DB=$($reader['default_database_name']) | CheckPolicy=$($reader['CheckPolicy'])"
}
$reader.Close()
$cv3.Close()

Write-Host ""
Write-Host "============================================================" -ForegroundColor Green
Write-Host "FASE 1 COMPLETADA" -ForegroundColor Green
Write-Host "============================================================" -ForegroundColor Green
Write-Host ""
Write-Host "PASSWORDS:" -ForegroundColor Yellow
Write-Host "  dp_admin_svc: $passAdmin" -ForegroundColor Yellow
Write-Host "  dp_web_svc:   $passWeb" -ForegroundColor Yellow
Write-Host ""
Write-Host "Siguiente: ejecutar test de escritura (018)" -ForegroundColor Cyan
