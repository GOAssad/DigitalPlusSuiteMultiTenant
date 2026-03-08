# ============================================================
# Genera par de claves RSA 2048-bit para firma de tickets de licencia
# Compatible con PowerShell 5.1 (Windows)
# Ejecutar UNA SOLA VEZ.
# ============================================================

param(
    [string]$OutputDir = ".",
    [string]$Subject = "CN=DigitalPlus License Signing"
)

Write-Host "`n=== Generando par de claves RSA 2048-bit ===" -ForegroundColor Cyan

# Crear certificado auto-firmado con clave RSA 2048
$cert = New-SelfSignedCertificate `
    -Subject $Subject `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -KeyUsage DigitalSignature `
    -NotAfter (Get-Date).AddYears(20) `
    -CertStoreLocation "Cert:\CurrentUser\My" `
    -HashAlgorithm SHA256 `
    -KeyExportPolicy Exportable

$thumbprint = $cert.Thumbprint
Write-Host "Certificado creado: $thumbprint" -ForegroundColor Green

# Exportar PFX (contiene private key)
$pfxPassword = ConvertTo-SecureString -String "temp1234!" -Force -AsPlainText
$pfxPath = Join-Path $OutputDir "license-temp.pfx"
Export-PfxCertificate -Cert "Cert:\CurrentUser\My\$thumbprint" -FilePath $pfxPath -Password $pfxPassword | Out-Null

# Exportar PFX como base64 - el servidor lo carga con X509Certificate2
$pfxBytes = [System.IO.File]::ReadAllBytes($pfxPath)
$pfxBase64 = [Convert]::ToBase64String($pfxBytes)

$privatePath = Join-Path $OutputDir "license-private-key.txt"
$pfxBase64 | Set-Content $privatePath -Encoding ASCII
Write-Host "Clave privada (PFX base64) guardada en: $privatePath" -ForegroundColor Yellow
Write-Host "  Tamano: $($pfxBase64.Length) caracteres" -ForegroundColor Gray

# Exportar certificado publico como DER (sin private key)
$certPath = Join-Path $OutputDir "license-public-cert.txt"
Export-Certificate -Cert "Cert:\CurrentUser\My\$thumbprint" -FilePath (Join-Path $OutputDir "license-temp.cer") -Type CERT | Out-Null
$certBytes = [System.IO.File]::ReadAllBytes((Join-Path $OutputDir "license-temp.cer"))
$certBase64 = [Convert]::ToBase64String($certBytes)
$certBase64 | Set-Content $certPath -Encoding ASCII
Write-Host "Certificado publico (DER base64) guardado en: $certPath" -ForegroundColor Yellow
Write-Host "  Tamano: $($certBase64.Length) caracteres" -ForegroundColor Gray

# Limpiar archivos temporales y cert del store
Remove-Item $pfxPath -ErrorAction SilentlyContinue
Remove-Item (Join-Path $OutputDir "license-temp.cer") -ErrorAction SilentlyContinue
Remove-Item "Cert:\CurrentUser\My\$thumbprint" -ErrorAction SilentlyContinue

Write-Host "`n=== INSTRUCCIONES ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. CLAVE PRIVADA (Azure App Setting):" -ForegroundColor White
Write-Host "   Nombre:   LicenseSigningKey"
Write-Host "   Valor:    contenido de $privatePath"
Write-Host "   Formato:  PFX base64 (cargable con new X509Certificate2(bytes, 'temp1234!'))"
Write-Host "   Password: temp1234!"
Write-Host ""
Write-Host "2. PASSWORD DEL PFX (Azure App Setting):" -ForegroundColor White
Write-Host "   Nombre:   LicenseSigningKeyPassword"
Write-Host "   Valor:    temp1234!"
Write-Host ""
Write-Host "3. CERTIFICADO PUBLICO (Cliente .NET 4.8):" -ForegroundColor White
Write-Host "   Copiar contenido de $certPath"
Write-Host "   Pegar en LicenseValidator.cs como const string PublicCertBase64"
Write-Host ""
Write-Host "4. SEGURIDAD:" -ForegroundColor Red
Write-Host "   NO subir $privatePath a repositorios Git"
Write-Host ""
Write-Host "Thumbprint: $thumbprint" -ForegroundColor Gray
