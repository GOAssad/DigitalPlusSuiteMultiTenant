$conn = New-Object System.Data.SqlClient.SqlConnection('Server=sd-1985882-l.ferozo.com,11434;Database=DigitalPlusAdmin;User Id=sa;Password=Soporte1;TrustServerCertificate=True;')
$conn.Open()
Write-Host "Connected to Ferozo (DigitalPlusAdmin)"

foreach ($file in @('001_CreateActivationCodesTable.sql','002_SP_ValidateAndConsumeCode.sql','003_SP_GenerateActivationCode.sql','004_SP_ListActivationCodes.sql','005_CreateLicenciasTable.sql','006_CreateLicenciasLogTable.sql','007_SP_LicenseActivate.sql','008_SP_LicenseHeartbeat.sql','009_CreateLicenseCodesTable.sql','010_SP_ValidateLicenseCode.sql')) {
    Write-Host "Executing $file..."
    $sql = [System.IO.File]::ReadAllText((Join-Path $PSScriptRoot $file))
    foreach ($batch in ($sql -split '(?m)^GO\s*$')) {
        $batch = $batch.Trim()
        if ($batch.Length -gt 0) {
            $cmd = $conn.CreateCommand()
            $cmd.CommandText = $batch
            $cmd.CommandTimeout = 60
            $cmd.ExecuteNonQuery() | Out-Null
        }
    }
    Write-Host "  OK"
}
$conn.Close()
Write-Host "Done!"
