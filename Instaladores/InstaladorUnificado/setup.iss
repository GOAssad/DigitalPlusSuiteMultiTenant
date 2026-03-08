; ============================================================
; DigitalPlus Suite - Instalador Unificado v1.3
; Herramienta: Inno Setup 6.x
; Generado:    2026-03-04
;
; Instala simultaneamente:
;   - DigitalPlus Fichadas   (TEntradaSalida.exe)
;   - DigitalPlus Administrador (Acceso.exe)
;
; Modos de instalacion:
;   LOCAL: Instala SQL Express 2019 si no hay, Windows Auth, BD automatica
;   NUBE:  Codigo de activacion -> provisioning Azure -> connection string remota
; ============================================================

; --- Identidad del instalador unificado ---
#define AppName       "DigitalPlus Suite"
#define AppVersion    "1.3"
#define AppPublisher  "DigitalOnePlus"
#define AppId         "{{D5E6F7A8-1B2C-3D4E-5F6A-7B8C9D0E1F2A}"

; --- Fichador ---
#define FichBin       "..\Fichador\TEntradaSalida\bin\Release"
#define FichExe       "TEntradaSalida.exe"
#define FichIcon      "..\Fichador\TEntradaSalida\Resources\Huellaksk.ico"

; --- Administrador ---
#define AdminBin      "..\Administrador\Acceso\bin\Release"
#define AdminExe      "Acceso.exe"
#define AdminIcon     "..\Administrador\Acceso\logo_web.ico"

; --- Recursos compartidos ---
#define SourceRTE_x86 "..\Digital-Persona-SDK-master\RTE\Install"
#define SourceRTE_x64 "..\Digital-Persona-SDK-master\RTE\Install\x64"
#define SourceSQL     "..\Datos\DigitalPlus_Script.sql"

; --- SQL Server Express 2019 ---
#define SourceSQLExpress "Prerequisites\SQLEXPR_x64_ENU.exe"

; --- Herramienta DPAPI para cifrar configs ---
#define SourceProtector "..\tools\ConfigProtector\bin\Release\net48"

; --- Provisioning Azure ---
#define ProvisionUrl "https://digitalplus-provision.azurewebsites.net/api/provision"
#define ProvisionApiKey "_coa6vaAjo3cCIio9j-VnrwPSb2qQuxBF8fa2E5XEUM"

; ============================================================
[Setup]
; ============================================================
AppId={#AppId}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
AppCopyright=Copyright (C) 2026 {#AppPublisher}

DefaultDirName=C:\DigitalPlus
DefaultGroupName=DigitalPlus
AllowNoIcons=yes

OutputDir=.\Output
OutputBaseFilename=DigitalPlus_Suite_Setup_v{#AppVersion}
SetupIconFile={#FichIcon}

Compression=lzma2
SolidCompression=yes
WizardStyle=modern

PrivilegesRequired=admin
MinVersion=6.1sp1
ArchitecturesAllowed=x64compatible

UninstallDisplayIcon={app}\Fichadas\{#FichExe}
UninstallDisplayName={#AppName}
CloseApplications=yes

; ============================================================
[Languages]
; ============================================================
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

; ============================================================
[Tasks]
; ============================================================

; --- Fichadas ---
Name: "fichadas_desktop"; Description: "Crear acceso directo en el Escritorio para Fichadas"; \
    GroupDescription: "Accesos directos - Fichadas:"; Flags: checkedonce
Name: "fichadas_startup"; Description: "Iniciar Fichadas automaticamente con Windows"; \
    GroupDescription: "Opciones adicionales - Fichadas:"; Flags: unchecked

; --- Administrador ---
Name: "admin_desktop"; Description: "Crear acceso directo en el Escritorio para Administrador"; \
    GroupDescription: "Accesos directos - Administrador:"; Flags: checkedonce
Name: "admin_startup"; Description: "Iniciar Administrador automaticamente con Windows"; \
    GroupDescription: "Opciones adicionales - Administrador:"; Flags: unchecked

; ============================================================
[Files]
; ============================================================

; ----------------------------------------------------------------
; FICHADAS  ->  {app}\Fichadas\
; ----------------------------------------------------------------

; --- Ejecutable principal ---
Source: "{#FichBin}\{#FichExe}";                   DestDir: "{app}\Fichadas"; Flags: ignoreversion

; --- Librerias del dominio y comunes ---
Source: "{#FichBin}\Acceso.Clases.Datos.dll";        DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\Acceso.Clases.Datos.dll.config"; DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\Global.Controles.dll";           DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\Global.Controles.dll.config";    DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\Global.Datos.dll";               DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\Global.Datos.dll.config";        DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\Global.Funciones.dll";           DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\Global.Funciones.dll.config";    DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\DigitalPlus.Licensing.dll";      DestDir: "{app}\Fichadas"; Flags: ignoreversion

; --- NuGet ---
Source: "{#FichBin}\FontAwesome.Sharp.dll";          DestDir: "{app}\Fichadas"; Flags: ignoreversion

; --- DLLs del SDK DigitalPersona ---
Source: "{#FichBin}\DPFPDevNET.dll";                 DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\DPFPEngNET.dll";                 DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\DPFPShrNET.dll";                 DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\DPFPVerNET.dll";                 DestDir: "{app}\Fichadas"; Flags: ignoreversion

; --- Localizacion ---
Source: "{#FichBin}\fr\*"; DestDir: "{app}\Fichadas\fr"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#FichBin}\ru\*"; DestDir: "{app}\Fichadas\ru"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist

; --- Config desde template ---
Source: "fichador.app.config.template"; DestDir: "{app}\Fichadas"; DestName: "TEntradaSalida.exe.config"; \
    Flags: ignoreversion

; ----------------------------------------------------------------
; HERRAMIENTAS  ->  {app}\tools\
; ----------------------------------------------------------------
Source: "{#SourceProtector}\ConfigProtector.exe"; DestDir: "{app}\tools"; Flags: ignoreversion

; ----------------------------------------------------------------
; ADMINISTRADOR  ->  {app}\Administrador\
; ----------------------------------------------------------------

; --- Ejecutable principal ---
Source: "{#AdminBin}\{#AdminExe}";                        DestDir: "{app}\Administrador"; Flags: ignoreversion

; --- Librerias del dominio y comunes ---
Source: "{#AdminBin}\Acceso.Clases.Datos.dll";              DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Acceso.Clases.Datos.dll.config";       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Global.Controles.dll";                 DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Global.Controles.dll.config";          DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Global.Datos.dll";                     DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Global.Datos.dll.config";              DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Global.Funciones.dll";                 DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Global.Funciones.dll.config";          DestDir: "{app}\Administrador"; Flags: ignoreversion

; --- AForge (vision por computadora / camara) ---
Source: "{#AdminBin}\AForge.dll";                           DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\AForge.Controls.dll";                  DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\AForge.Imaging.dll";                   DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\AForge.Math.dll";                      DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\AForge.Video.dll";                     DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\AForge.Video.DirectShow.dll";          DestDir: "{app}\Administrador"; Flags: ignoreversion

; --- DLLs del SDK DigitalPersona ---
Source: "{#AdminBin}\DPFPDevNET.dll";                       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DPFPEngNET.dll";                       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DPFPGuiNET.dll";                       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DPFPShrNET.dll";                       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DPFPShrXTypeLibNET.dll";               DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DPFPVerNET.dll";                       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DPFPCtlXTypeLibNET.dll";               DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\AxInterop.DPFPCtlXLib.dll";            DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Interop.DPFPCtlXLib.dll";              DestDir: "{app}\Administrador"; Flags: ignoreversion

; --- Microsoft Report Viewer ---
Source: "{#AdminBin}\Microsoft.ReportViewer.Common.dll";             DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Microsoft.ReportViewer.DataVisualization.dll";  DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Microsoft.ReportViewer.ProcessingObjectModel.dll"; DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Microsoft.ReportViewer.WinForms.dll";           DestDir: "{app}\Administrador"; Flags: ignoreversion

; --- SQL Server Types (soporte espacial) ---
Source: "{#AdminBin}\Microsoft.SqlServer.Types.dll";         DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\SqlServerTypes\x86\SqlServerSpatial140.dll"; DestDir: "{app}\Administrador\SqlServerTypes\x86"; Flags: ignoreversion
Source: "{#AdminBin}\SqlServerTypes\x86\msvcr120.dll";       DestDir: "{app}\Administrador\SqlServerTypes\x86"; Flags: ignoreversion
Source: "{#AdminBin}\SqlServerTypes\x64\SqlServerSpatial140.dll"; DestDir: "{app}\Administrador\SqlServerTypes\x64"; Flags: ignoreversion
Source: "{#AdminBin}\SqlServerTypes\x64\msvcr120.dll";       DestDir: "{app}\Administrador\SqlServerTypes\x64"; Flags: ignoreversion

; --- iText 7 (generacion de PDF) ---
Source: "{#AdminBin}\itext.barcodes.dll";                   DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\itext.forms.dll";                      DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\itext.io.dll";                         DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\itext.kernel.dll";                     DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\itext.layout.dll";                     DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\itext.pdfa.dll";                       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\itext.sign.dll";                       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\itext.styledxmlparser.dll";            DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\itext.svg.dll";                        DestDir: "{app}\Administrador"; Flags: ignoreversion

; --- Otras librerias NuGet ---
Source: "{#AdminBin}\AdvancedDataGridView.dll";             DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\BouncyCastle.Crypto.dll";              DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Common.Logging.dll";                   DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\Common.Logging.Core.dll";              DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DocumentFormat.OpenXml.dll";           DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\FontAwesome.Sharp.dll";                DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\SpreadsheetLight.dll";                 DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DigitalPlus.Licensing.dll";            DestDir: "{app}\Administrador"; Flags: ignoreversion

; --- Recursos de idiomas ---
Source: "{#AdminBin}\de\*";     DestDir: "{app}\Administrador\de";     Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\es\*";     DestDir: "{app}\Administrador\es";     Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\fr\*";     DestDir: "{app}\Administrador\fr";     Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\it\*";     DestDir: "{app}\Administrador\it";     Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\ja\*";     DestDir: "{app}\Administrador\ja";     Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\ko\*";     DestDir: "{app}\Administrador\ko";     Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\pt\*";     DestDir: "{app}\Administrador\pt";     Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\ru\*";     DestDir: "{app}\Administrador\ru";     Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\zh-CHS\*"; DestDir: "{app}\Administrador\zh-CHS"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "{#AdminBin}\zh-CHT\*"; DestDir: "{app}\Administrador\zh-CHT"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist

; --- Config desde template ---
Source: "administrador.app.config.template"; DestDir: "{app}\Administrador"; DestName: "Acceso.exe.config"; \
    Flags: ignoreversion

; ----------------------------------------------------------------
; RECURSOS COMPARTIDOS (temporales, se borran al terminar)
; ----------------------------------------------------------------

; --- Script SQL de creacion de BD ---
Source: "{#SourceSQL}"; DestDir: "{tmp}"; DestName: "crear_bd.sql"; \
    Flags: deleteafterinstall

; --- Driver DigitalPersona RTE (se instala UNA vez para ambas apps) ---
Source: "{#SourceRTE_x86}\Setup.msi"; DestDir: "{tmp}"; DestName: "DP_RTE_x86.msi"; \
    Flags: deleteafterinstall
Source: "{#SourceRTE_x64}\Setup.msi"; DestDir: "{tmp}"; DestName: "DP_RTE_x64.msi"; \
    Flags: deleteafterinstall

; --- SQL Server Express 2019 (solo se extrae si modo local y no hay SQL) ---
Source: "{#SourceSQLExpress}"; DestDir: "{tmp}"; DestName: "SQLEXPR_x64_ENU.exe"; \
    Flags: deleteafterinstall; Check: NecesitaExtraerSQLExpress

; ============================================================
[Icons]
; ============================================================

; --- Menu Inicio ---
Name: "{group}\DigitalPlus Fichadas";           Filename: "{app}\Fichadas\{#FichExe}"
Name: "{group}\DigitalPlus Administrador";      Filename: "{app}\Administrador\{#AdminExe}"
Name: "{group}\Desinstalar DigitalPlus Suite";  Filename: "{uninstallexe}"

; --- Escritorio (condicional por task) ---
Name: "{autodesktop}\DigitalPlus Fichadas";      Filename: "{app}\Fichadas\{#FichExe}"; \
    Tasks: fichadas_desktop
Name: "{autodesktop}\DigitalPlus Administrador"; Filename: "{app}\Administrador\{#AdminExe}"; \
    Tasks: admin_desktop

; ============================================================
[Registry]
; ============================================================

; Auto-inicio Fichadas
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; \
    ValueType: string; ValueName: "DigitalPlus Fichadas"; \
    ValueData: """{app}\Fichadas\{#FichExe}"""; \
    Flags: uninsdeletevalue; Tasks: fichadas_startup

; Auto-inicio Administrador
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; \
    ValueType: string; ValueName: "DigitalPlus Administrador"; \
    ValueData: """{app}\Administrador\{#AdminExe}"""; \
    Flags: uninsdeletevalue; Tasks: admin_startup

; ============================================================
[Run]
; ============================================================

; Instalar RTE DigitalPersona UNA sola vez (compartido)
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\DP_RTE_x86.msi"" /quiet /norestart"; \
    StatusMsg: "Instalando driver DigitalPersona..."; \
    Flags: waituntilterminated; Check: not IsWin64

Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\DP_RTE_x64.msi"" /quiet /norestart"; \
    StatusMsg: "Instalando driver DigitalPersona (64-bit)..."; \
    Flags: waituntilterminated; Check: IsWin64

; Opciones post-instalacion: ejecutar cualquiera de las dos apps
Filename: "{app}\Fichadas\{#FichExe}"; \
    Description: "Ejecutar DigitalPlus Fichadas ahora"; \
    Flags: nowait postinstall skipifsilent unchecked

Filename: "{app}\Administrador\{#AdminExe}"; \
    Description: "Ejecutar DigitalPlus Administrador ahora"; \
    Flags: nowait postinstall skipifsilent unchecked

; ============================================================
[Code]
// ============================================================

// ------------------------------------------------------------
// Variables globales
// ------------------------------------------------------------
var
  // Pagina: Empresa
  EmpresaPage:   TWizardPage;
  lblEmpresa:    TNewStaticText;
  edtEmpresa:    TNewEdit;

  // Pagina: URL Portal Web
  UrlPage:       TWizardPage;
  lblUrlInfo:    TNewStaticText;
  lblUrl:        TNewStaticText;
  edtUrl:        TNewEdit;

  // Pagina: Modo de instalacion (Local vs Nube)
  ModoPage:      TWizardPage;
  lblModoInfo:   TNewStaticText;
  lblModoNube:   TNewStaticText;
  rbLocal:       TNewRadioButton;
  rbNube:        TNewRadioButton;

  // Pagina: Codigo de activacion (solo modo Nube)
  ActivacionPage:   TWizardPage;
  lblActivacion:    TNewStaticText;
  edtActivacion:    TNewEdit;
  btnActivar:       TNewButton;
  lblActivResult:   TNewStaticText;

  // Estado interno
  bModoLocal:          Boolean;   // True = local, False = nube
  bSQLInstalado:       Boolean;   // True si ya habia una instancia SQL
  bSQLRecienInstalado: Boolean;   // True si se instalo SQL Express en esta sesion
  bBDDisponible:       Boolean;   // True si la BD esta lista
  sNombreInstancia:    String;    // Nombre de instancia detectado
  sDataSource:         String;    // Data Source para connection strings
  sConnectionString:   String;    // Connection string final (local o nube)
  bActivacionOK:       Boolean;   // True si el codigo de activacion fue validado
  sCloudConnStr:       String;    // Connection string recibida de Azure (modo nube)
  sNombreBD:           String;    // Nombre de la BD (DP_NombreEmpresa sanitizado)


// ============================================================
// DETECCION DE SQL SERVER
// ============================================================

// ------------------------------------------------------------
// Detecta instancias de SQL Server instaladas.
// Prioridad: SQLEXPRESS > MSSQLSERVER > primera disponible
// ------------------------------------------------------------
function DetectarSQLServer: Boolean;
var
  Names: TArrayOfString;
  i: Integer;
begin
  Result := False;
  sNombreInstancia := '';
  sDataSource := '';

  if not RegGetValueNames(HKLM64, 'SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL', Names) then
    Exit;

  if GetArrayLength(Names) = 0 then
    Exit;

  Result := True;

  // Buscar SQLEXPRESS primero (preferido)
  for i := 0 to GetArrayLength(Names) - 1 do
  begin
    if CompareText(Names[i], 'SQLEXPRESS') = 0 then
    begin
      sNombreInstancia := 'SQLEXPRESS';
      sDataSource := '.\SQLEXPRESS';
      Exit;
    end;
  end;

  // Buscar MSSQLSERVER (instancia por defecto)
  for i := 0 to GetArrayLength(Names) - 1 do
  begin
    if CompareText(Names[i], 'MSSQLSERVER') = 0 then
    begin
      sNombreInstancia := 'MSSQLSERVER';
      sDataSource := '.';
      Exit;
    end;
  end;

  // Usar la primera que haya
  sNombreInstancia := Names[0];
  sDataSource := '.\' + Names[0];
end;


// ------------------------------------------------------------
// Check function para [Files]: solo extrae SQL Express si
// es modo local y no hay SQL Server instalado
// ------------------------------------------------------------
function NecesitaExtraerSQLExpress: Boolean;
begin
  Result := bModoLocal and (not DetectarSQLServer);
end;


// ============================================================
// CONNECTION STRINGS
// ============================================================

// ------------------------------------------------------------
// Connection string para .NET SqlClient (local)
// ------------------------------------------------------------
function GetLocalConnectionString: String;
begin
  Result :=
    'Data Source=' + sDataSource + ';' +
    'Initial Catalog=' + sNombreBD + ';' +
    'Integrated Security=True;';
end;

// ------------------------------------------------------------
// Connection string ADODB (para registro de terminal, local)
// ------------------------------------------------------------
function GetLocalOleDbConnectionString: String;
begin
  Result :=
    'Provider=SQLOLEDB;' +
    'Data Source=' + sDataSource + ';' +
    'Initial Catalog=' + sNombreBD + ';' +
    'Integrated Security=SSPI;';
end;


// ============================================================
// FUNCIONES AUXILIARES
// ============================================================

// ------------------------------------------------------------
// Verifica .NET Framework 4.8 (Release >= 528040)
// ------------------------------------------------------------
function IsDotNet48Installed: Boolean;
var
  Release: Cardinal;
begin
  Result := RegQueryDWordValue(HKLM,
    'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full',
    'Release', Release) and (Release >= 528040);
end;

// ------------------------------------------------------------
// Sanitiza el nombre de empresa para usar como nombre de BD.
// Quita espacios, acentos y caracteres especiales.
// Resultado: DP_NombreEmpresa (solo letras, numeros, guion bajo)
// ------------------------------------------------------------
function SanitizarNombreBD(empresa: String): String;
var
  i: Integer;
  c: Char;
  s: String;
begin
  s := Trim(empresa);
  Result := '';
  for i := 1 to Length(s) do
  begin
    c := s[i];
    if ((c >= 'A') and (c <= 'Z')) or
       ((c >= 'a') and (c <= 'z')) or
       ((c >= '0') and (c <= '9')) then
      Result := Result + c
    else if (c = ' ') or (c = '-') or (c = '_') then
      Result := Result + '_';
    // Otros caracteres se descartan
  end;

  // Quitar guiones bajos duplicados
  while Pos('__', Result) > 0 do
    StringChange(Result, '__', '_');

  // Quitar guion bajo al inicio/final
  while (Length(Result) > 0) and (Result[1] = '_') do
    Delete(Result, 1, 1);
  while (Length(Result) > 0) and (Result[Length(Result)] = '_') do
    Delete(Result, Length(Result), 1);

  if Result = '' then
    Result := 'DigitalPlus';

  Result := 'DP_' + Result;
end;


// ------------------------------------------------------------
// Escapa comillas simples para PowerShell
// ------------------------------------------------------------
function EscapePS(s: String): String;
begin
  Result := s;
  StringChange(Result, '''', '''''');
end;

// ------------------------------------------------------------
// Escapa comillas simples para SQL
// ------------------------------------------------------------
function EscapeSQL(s: String): String;
begin
  Result := s;
  StringChange(Result, '''', '''''');
end;

// ------------------------------------------------------------
// Verifica si la aplicacion ya esta instalada.
// Busca por registry (instalador unificado) o por archivos en
// disco (instalacion individual o manual).
// ------------------------------------------------------------
function IsAppAlreadyInstalled: Boolean;
var
  Path: String;
  DefaultDir: String;
begin
  Result := False;

  // Opcion 1: Instalador unificado previo (registry)
  if RegQueryStringValue(HKLM,
    'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{#AppId}_is1',
    'InstallLocation', Path) and (Path <> '') then
  begin
    Result := True;
    Exit;
  end;

  // Opcion 2: Fichador individual (registry)
  if RegQueryStringValue(HKLM,
    'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{B3F2A1C0-5E4D-4F8B-9A7C-1D2E3F4A5B6C}_is1',
    'InstallLocation', Path) and (Path <> '') then
  begin
    Result := True;
    Exit;
  end;

  // Opcion 3: Administrador individual (registry)
  if RegQueryStringValue(HKLM,
    'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A4C1B2D3-6F5E-4A9C-8B7D-2E3F4A5B6C7D}_is1',
    'InstallLocation', Path) and (Path <> '') then
  begin
    Result := True;
    Exit;
  end;

  // Opcion 4: Archivos en disco (directorio por defecto)
  DefaultDir := ExpandConstant('{param:DIR|C:\DigitalPlus}');
  if FileExists(DefaultDir + '\Fichadas\TEntradaSalida.exe') or
     FileExists(DefaultDir + '\Administrador\Acceso.exe') then
  begin
    Result := True;
  end;
end;


// ============================================================
// DETECCION DE INSTALACION EXISTENTE
// ============================================================

// ------------------------------------------------------------
// Bloquea la instalacion si faltan requisitos.
// Si ya esta instalado, pregunta si desea reinstalar.
// ------------------------------------------------------------
function InitializeSetup(): Boolean;
begin
  Result := True;

  if not IsWin64 then
  begin
    MsgBox(
      'Este instalador requiere un sistema operativo de 64 bits.' + #13#10 + #13#10 +
      'SQL Server Express 2019 solo esta disponible para 64 bits.',
      mbCriticalError, MB_OK);
    Result := False;
    Exit;
  end;

  if not IsDotNet48Installed then
  begin
    MsgBox(
      'Este instalador requiere Microsoft .NET Framework 4.8 o superior.' + #13#10 + #13#10 +
      'Por favor instale .NET Framework 4.8 y vuelva a ejecutar el instalador.' + #13#10 +
      'Descarga: https://dotnet.microsoft.com/download/dotnet-framework/net48',
      mbCriticalError, MB_OK);
    Result := False;
    Exit;
  end;

  if IsAppAlreadyInstalled then
  begin
    if MsgBox(
      'DigitalPlus Suite ya esta instalado en este equipo.' + #13#10 + #13#10 +
      'Si continua, se actualizaran los archivos de la aplicacion.' + #13#10 +
      'La base de datos y la configuracion existente se mantendran.' + #13#10 + #13#10 +
      'Desea continuar con la reinstalacion?',
      mbConfirmation, MB_YESNO) = IDNO then
    begin
      Result := False;
    end;
  end;
end;


// ============================================================
// PAGINAS DEL WIZARD
// ============================================================

// ------------------------------------------------------------
// Pagina 1: Nombre de empresa
// ------------------------------------------------------------
procedure CreateEmpresaPage;
begin
  EmpresaPage := CreateCustomPage(
    wpWelcome,
    'Configuracion de empresa',
    'Ingrese el nombre de su empresa tal como aparecera en el sistema.');

  lblEmpresa := TNewStaticText.Create(EmpresaPage);
  lblEmpresa.Parent  := EmpresaPage.Surface;
  lblEmpresa.Left    := 0;
  lblEmpresa.Top     := 8;
  lblEmpresa.Caption := 'Nombre de la empresa:';

  edtEmpresa := TNewEdit.Create(EmpresaPage);
  edtEmpresa.Parent  := EmpresaPage.Surface;
  edtEmpresa.Left    := 0;
  edtEmpresa.Top     := 28;
  edtEmpresa.Width   := EmpresaPage.Surface.Width;
  edtEmpresa.Text    := 'Mi Empresa';
end;


// ------------------------------------------------------------
// Pagina 2: URL del portal DigitalPlusWeb
// ------------------------------------------------------------
procedure CreateUrlPage;
begin
  UrlPage := CreateCustomPage(
    EmpresaPage.ID,
    'Portal web DigitalPlus',
    'Ingrese la URL del portal web de DigitalPlus (modulo de administracion web).');

  lblUrlInfo := TNewStaticText.Create(UrlPage);
  lblUrlInfo.Parent     := UrlPage.Surface;
  lblUrlInfo.Left       := 0;
  lblUrlInfo.Top        := 8;
  lblUrlInfo.Width      := UrlPage.Surface.Width;
  lblUrlInfo.WordWrap   := True;
  lblUrlInfo.Caption    :=
    'Esta URL se usara cuando presione el boton "DigitalPlusWeb" en el menu principal del Administrador.' + #13#10 +
    'Puede ser una IP local (http://192.168.0.x/), un dominio, o una URL en la nube.';

  lblUrl := TNewStaticText.Create(UrlPage);
  lblUrl.Parent  := UrlPage.Surface;
  lblUrl.Left    := 0;
  lblUrl.Top     := 52;
  lblUrl.Caption := 'URL del portal web:';

  edtUrl := TNewEdit.Create(UrlPage);
  edtUrl.Parent  := UrlPage.Surface;
  edtUrl.Left    := 0;
  edtUrl.Top     := 72;
  edtUrl.Width   := UrlPage.Surface.Width;
  edtUrl.Text    := 'https://digitalplusapp.azurewebsites.net/';
end;


// ------------------------------------------------------------
// Pagina 3: Modo de instalacion (Local vs Nube)
// ------------------------------------------------------------
procedure CreateModoPage;
begin
  bModoLocal := True;  // default: local

  ModoPage := CreateCustomPage(
    UrlPage.ID,
    'Tipo de instalacion',
    'Seleccione como desea configurar la base de datos del sistema.');

  rbLocal := TNewRadioButton.Create(ModoPage);
  rbLocal.Parent  := ModoPage.Surface;
  rbLocal.Left    := 0;
  rbLocal.Top     := 16;
  rbLocal.Width   := ModoPage.Surface.Width;
  rbLocal.Height  := 20;
  rbLocal.Caption := 'Instalacion LOCAL (base de datos en este equipo)';
  rbLocal.Checked := True;

  lblModoInfo := TNewStaticText.Create(ModoPage);
  lblModoInfo.Parent   := ModoPage.Surface;
  lblModoInfo.Left     := 20;
  lblModoInfo.Top      := 40;
  lblModoInfo.Width    := ModoPage.Surface.Width - 20;
  lblModoInfo.WordWrap := True;
  lblModoInfo.Caption  :=
    'La base de datos se crea en este equipo.' + #13#10 +
    'No requiere conexion a internet.' + #13#10 +
    'Si no tiene SQL Server instalado, se instalara automaticamente.';

  rbNube := TNewRadioButton.Create(ModoPage);
  rbNube.Parent  := ModoPage.Surface;
  rbNube.Left    := 0;
  rbNube.Top     := 110;
  rbNube.Width   := ModoPage.Surface.Width;
  rbNube.Height  := 20;
  rbNube.Caption := 'Instalacion en la NUBE (base de datos remota)';

  lblModoNube := TNewStaticText.Create(ModoPage);
  lblModoNube.Parent   := ModoPage.Surface;
  lblModoNube.Left     := 20;
  lblModoNube.Top      := 134;
  lblModoNube.Width    := ModoPage.Surface.Width - 20;
  lblModoNube.WordWrap := True;
  lblModoNube.Caption  :=
    'La base de datos se aloja en un servidor remoto.' + #13#10 +
    'Requiere un codigo de activacion y conexion a internet.';
end;


// ============================================================
// VALIDACION DE CODIGO DE ACTIVACION (NUBE)
// Debe estar ANTES de CreateActivacionPage para que el
// identificador BtnActivarClick sea conocido.
// ============================================================

// ------------------------------------------------------------
// Llama al endpoint Azure para validar el codigo de activacion.
// Usa MSXML2.XMLHTTP para hacer POST desde Inno Setup Pascal.
// ------------------------------------------------------------
procedure BtnActivarClick(Sender: TObject);
var
  Http:       Variant;
  JsonBody:   String;
  StatusCode: Integer;
  Response:   String;
  ConnStrPos: Integer;
  ConnStrEnd: Integer;
  MachineId:  String;
begin
  bActivacionOK := False;
  sCloudConnStr := '';

  if Trim(edtActivacion.Text) = '' then
  begin
    lblActivResult.Caption := 'Ingrese un codigo de activacion.';
    Exit;
  end;

  lblActivResult.Caption := 'Validando codigo... (requiere conexion a internet)';
  WizardForm.Repaint;

  MachineId := GetEnv('COMPUTERNAME');

  JsonBody :=
    '{' +
    '"activationCode":"' + Trim(edtActivacion.Text) + '",' +
    '"companyName":"' + Trim(edtEmpresa.Text) + '",' +
    '"installType":"cloud",' +
    '"machineId":"' + MachineId + '"' +
    '}';

  try
    Http := CreateOleObject('MSXML2.XMLHTTP');
    Http.Open('POST', '{#ProvisionUrl}', False);
    Http.SetRequestHeader('Content-Type', 'application/json');
    Http.SetRequestHeader('X-Api-Key', '{#ProvisionApiKey}');
    Http.Send(JsonBody);

    StatusCode := Http.Status;
    Response   := Http.ResponseText;

  except
    lblActivResult.Caption :=
      'Error de conexion. Verifique que tiene acceso a internet.' + #13#10 +
      'Detalle: ' + GetExceptionMessage;
    Exit;
  end;

  if StatusCode = 200 then
  begin
    ConnStrPos := Pos('"connectionString":"', Response);
    if ConnStrPos > 0 then
    begin
      ConnStrPos := ConnStrPos + 20;
      ConnStrEnd := ConnStrPos;
      while (ConnStrEnd <= Length(Response)) and (Response[ConnStrEnd] <> '"') do
        ConnStrEnd := ConnStrEnd + 1;
      sCloudConnStr := Copy(Response, ConnStrPos, ConnStrEnd - ConnStrPos);
    end;

    if sCloudConnStr <> '' then
    begin
      bActivacionOK := True;
      lblActivResult.Caption :=
        'Codigo validado correctamente.' + #13#10 +
        'La base de datos en la nube esta lista.';
    end
    else
    begin
      lblActivResult.Caption :=
        'Codigo validado, pero no se recibio una cadena de conexion.' + #13#10 +
        'Contacte soporte tecnico.';
    end;
  end
  else if StatusCode = 403 then
  begin
    lblActivResult.Caption :=
      'Codigo rechazado: ' + Response;
  end
  else if StatusCode = 409 then
  begin
    lblActivResult.Caption :=
      'La base de datos ya existe en la nube. Contacte soporte.';
  end
  else
  begin
    lblActivResult.Caption :=
      'Error del servidor (codigo HTTP ' + IntToStr(StatusCode) + ').' + #13#10 +
      Response;
  end;
end;


// ------------------------------------------------------------
// Pagina 4: Codigo de activacion (solo visible en modo Nube)
// ------------------------------------------------------------
procedure CreateActivacionPage;
begin
  bActivacionOK := False;

  ActivacionPage := CreateCustomPage(
    ModoPage.ID,
    'Codigo de activacion',
    'Ingrese el codigo de activacion proporcionado por DigitalPlus.');

  lblActivacion := TNewStaticText.Create(ActivacionPage);
  lblActivacion.Parent   := ActivacionPage.Surface;
  lblActivacion.Left     := 0;
  lblActivacion.Top      := 8;
  lblActivacion.Width    := ActivacionPage.Surface.Width;
  lblActivacion.WordWrap := True;
  lblActivacion.Caption  :=
    'Para la instalacion en la nube necesita un codigo de activacion valido.' + #13#10 +
    'Formato: XXXX-XXXX-XXXX-XXXX' + #13#10 + #13#10 +
    'Si no tiene un codigo, contacte a su proveedor de DigitalPlus.';

  edtActivacion := TNewEdit.Create(ActivacionPage);
  edtActivacion.Parent := ActivacionPage.Surface;
  edtActivacion.Left   := 0;
  edtActivacion.Top    := 72;
  edtActivacion.Width  := 250;
  edtActivacion.Text   := '';

  btnActivar := TNewButton.Create(ActivacionPage);
  btnActivar.Parent  := ActivacionPage.Surface;
  btnActivar.Left    := 260;
  btnActivar.Top     := 70;
  btnActivar.Width   := 120;
  btnActivar.Height  := 26;
  btnActivar.Caption := 'Validar codigo';
  btnActivar.OnClick := @BtnActivarClick;

  lblActivResult := TNewStaticText.Create(ActivacionPage);
  lblActivResult.Parent   := ActivacionPage.Surface;
  lblActivResult.Left     := 0;
  lblActivResult.Top      := 108;
  lblActivResult.Width    := ActivacionPage.Surface.Width;
  lblActivResult.WordWrap := True;
  lblActivResult.Caption  := '';
end;


// ============================================================
// INICIALIZACION DEL WIZARD
// ============================================================

procedure InitializeWizard;
begin
  CreateEmpresaPage;
  CreateUrlPage;
  CreateModoPage;
  CreateActivacionPage;
end;


// ============================================================
// NAVEGACION DEL WIZARD
// ============================================================

// ------------------------------------------------------------
// Controla la visibilidad de paginas segun el modo
// y valida antes de avanzar
// ------------------------------------------------------------
function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := False;

  // Saltar la pagina de activacion si es modo local
  if PageID = ActivacionPage.ID then
    Result := bModoLocal;
end;


function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;

  // Al salir de la pagina de URL, validar
  if CurPageID = UrlPage.ID then
  begin
    if Trim(edtUrl.Text) = '' then
    begin
      MsgBox('Debe ingresar la URL del portal web antes de continuar.', mbError, MB_OK);
      Result := False;
    end;
  end;

  // Al salir de la pagina de modo, guardar eleccion
  if CurPageID = ModoPage.ID then
  begin
    bModoLocal := rbLocal.Checked;
  end;

  // Al salir de la pagina de activacion, verificar que se valido
  if CurPageID = ActivacionPage.ID then
  begin
    if not bActivacionOK then
    begin
      MsgBox(
        'Debe validar el codigo de activacion antes de continuar.' + #13#10 +
        'Presione "Validar codigo" y espere el resultado.',
        mbError, MB_OK);
      Result := False;
    end;
  end;
end;


// ============================================================
// INSTALACION DE SQL EXPRESS (MODO LOCAL)
// ============================================================

procedure InstalarSQLExpress;
var
  ResultCode: Integer;
  ExePath: String;
  Params: String;
begin
  ExePath := ExpandConstant('{tmp}\SQLEXPR_x64_ENU.exe');

  Params :=
    '/Q ' +
    '/IACCEPTSQLSERVERLICENSETERMS ' +
    '/ACTION="Install" ' +
    '/FEATURES=SQLENGINE ' +
    '/INSTANCENAME="SQLEXPRESS" ' +
    '/SQLSVCACCOUNT="NT AUTHORITY\SYSTEM" ' +
    '/SQLSYSADMINACCOUNTS="BUILTIN\Administrators" ' +
    '/TCPENABLED=1 ' +
    '/NPENABLED=1 ' +
    '/SECURITYMODE=SQL ' +
    '/UPDATEENABLED=0';

  if not Exec(ExePath, Params, '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    MsgBox(
      'No se pudo ejecutar el instalador de SQL Server Express.' + #13#10 +
      'Instale SQL Server Express manualmente y vuelva a ejecutar este instalador.',
      mbCriticalError, MB_OK);
    Exit;
  end;

  if (ResultCode = 0) or (ResultCode = 3010) then
  begin
    bSQLRecienInstalado := True;
  end
  else
  begin
    MsgBox(
      'SQL Server Express se instalo con advertencias o errores (codigo: ' + IntToStr(ResultCode) + ').' + #13#10 + #13#10 +
      'Si las aplicaciones no pueden conectarse a la base de datos, ' + #13#10 +
      'instale SQL Server Express manualmente.',
      mbError, MB_OK);
    bSQLRecienInstalado := True;
  end;
end;


// ============================================================
// ESCRITURA DE CONFIGS
// ============================================================

// ------------------------------------------------------------
// Escribe la configuracion de Fichadas
// ------------------------------------------------------------
procedure EscribirConfigFichador;
var
  FilePath: String;
  Lines:    TArrayOfString;
  i:        Integer;
begin
  FilePath := ExpandConstant('{app}\Fichadas\TEntradaSalida.exe.config');

  if LoadStringsFromFile(FilePath, Lines) then
  begin
    for i := 0 to GetArrayLength(Lines) - 1 do
    begin
      StringChange(Lines[i], '{{CONNECTION_STRING}}', sConnectionString);
      StringChange(Lines[i], '{{NOMBRE_EMPRESA}}', Trim(edtEmpresa.Text));
      StringChange(Lines[i], '{{PROVISIONING_API_KEY}}', '{#ProvisionApiKey}');
    end;
    SaveStringsToFile(FilePath, Lines, False);
  end;
end;


// ------------------------------------------------------------
// Escribe la configuracion de Administrador
// ------------------------------------------------------------
procedure EscribirConfigAdministrador;
var
  FilePath: String;
  Lines:    TArrayOfString;
  i:        Integer;
begin
  FilePath := ExpandConstant('{app}\Administrador\Acceso.exe.config');

  if LoadStringsFromFile(FilePath, Lines) then
  begin
    for i := 0 to GetArrayLength(Lines) - 1 do
    begin
      StringChange(Lines[i], '{{CONNECTION_STRING}}',    sConnectionString);
      StringChange(Lines[i], '{{NOMBRE_EMPRESA}}',       Trim(edtEmpresa.Text));
      StringChange(Lines[i], '{{URL_DIGITALPLUS_WEB}}',  Trim(edtUrl.Text));
      StringChange(Lines[i], '{{PROVISIONING_API_KEY}}', '{#ProvisionApiKey}');
    end;
    SaveStringsToFile(FilePath, Lines, False);
  end;
end;


// ============================================================
// CREACION DE BD (MODO LOCAL)
// ============================================================

function BuildCrearBDScript: String;
var
  SqlFile: String;
  PS: String;
begin
  SqlFile := EscapePS(ExpandConstant('{tmp}\crear_bd.sql'));

  PS := '$ErrorActionPreference = ''Stop''' + #13#10;
  PS := PS + '$server   = ''' + EscapePS(sDataSource) + '''' + #13#10;
  PS := PS + '$database = ''' + EscapePS(sNombreBD) + '''' + #13#10;
  PS := PS + '$sqlFile  = ''' + SqlFile + '''' + #13#10;
  PS := PS + '' + #13#10;
  PS := PS + 'try {' + #13#10;

  // Conectar a master con Windows Auth
  PS := PS + '    $connStr = "Server=$server;Database=master;Integrated Security=True;Connect Timeout=30;"' + #13#10;
  PS := PS + '    $conn = New-Object System.Data.SqlClient.SqlConnection($connStr)' + #13#10;
  PS := PS + '    $conn.Open()' + #13#10;
  PS := PS + '' + #13#10;

  // Verificar si la BD ya existe
  PS := PS + '    $checkCmd = $conn.CreateCommand()' + #13#10;
  PS := PS + '    $checkCmd.CommandText = "SELECT COUNT(*) FROM sys.databases WHERE name=N''$database''"' + #13#10;
  PS := PS + '    $dbExists = [int]$checkCmd.ExecuteScalar()' + #13#10;
  PS := PS + '    if ($dbExists -gt 0) {' + #13#10;
  PS := PS + '        $conn.Close()' + #13#10;
  PS := PS + '        exit 0' + #13#10;
  PS := PS + '    }' + #13#10;
  PS := PS + '' + #13#10;

  // Crear la BD
  PS := PS + '    $createCmd = $conn.CreateCommand()' + #13#10;
  PS := PS + '    $createCmd.CommandText = "CREATE DATABASE [$database]"' + #13#10;
  PS := PS + '    $createCmd.CommandTimeout = 300' + #13#10;
  PS := PS + '    $createCmd.ExecuteNonQuery() | Out-Null' + #13#10;
  PS := PS + '' + #13#10;

  // Verificar que se creo
  PS := PS + '    $checkCmd = $conn.CreateCommand()' + #13#10;
  PS := PS + '    $checkCmd.CommandText = "SELECT COUNT(*) FROM sys.databases WHERE name=N''$database''"' + #13#10;
  PS := PS + '    $dbExists = [int]$checkCmd.ExecuteScalar()' + #13#10;
  PS := PS + '    if ($dbExists -eq 0) {' + #13#10;
  PS := PS + '        "CREATE DATABASE ejecutado pero la BD no aparece en sys.databases" | Out-File "$env:TEMP\dp_crear_bd_error.txt" -Encoding UTF8' + #13#10;
  PS := PS + '        $conn.Close()' + #13#10;
  PS := PS + '        exit 2' + #13#10;
  PS := PS + '    }' + #13#10;
  PS := PS + '' + #13#10;

  // Leer el script SQL
  PS := PS + '    $content = [System.IO.File]::ReadAllText($sqlFile, [System.Text.Encoding]::UTF8)' + #13#10;
  PS := PS + '    $content = $content -replace ''\[DigitalPlus\]'', "[$database]"' + #13#10;
  PS := PS + '    $content = $content -replace "N''DigitalPlus''", "N''$database''"' + #13#10;
  PS := PS + '' + #13#10;

  // Cambiar conexion a la BD creada
  PS := PS + '    $conn.Close()' + #13#10;
  PS := PS + '    $dbConnStr = "Server=$server;Database=$database;Integrated Security=True;Connect Timeout=30;"' + #13#10;
  PS := PS + '    $conn = New-Object System.Data.SqlClient.SqlConnection($dbConnStr)' + #13#10;
  PS := PS + '    $conn.Open()' + #13#10;
  PS := PS + '' + #13#10;

  // Dividir por GO y ejecutar cada batch
  PS := PS + '    $batches = $content -split "(?mi)^\s*GO\s*$"' + #13#10;
  PS := PS + '    $errCount = 0' + #13#10;
  PS := PS + '    $errMsgs  = @()' + #13#10;
  PS := PS + '    foreach ($batch in $batches) {' + #13#10;
  PS := PS + '        $t = $batch.Trim()' + #13#10;
  PS := PS + '        if ($t -eq '''' -or $t -eq ''GO'') { continue }' + #13#10;
  PS := PS + '        if ($t -match ''(?i)^\s*CREATE\s+DATABASE\b'') { continue }' + #13#10;
  PS := PS + '        if ($t -match ''(?i)^\s*ALTER\s+DATABASE\b'') { continue }' + #13#10;
  PS := PS + '        if ($t -match ''(?i)^\s*USE\s+'') { continue }' + #13#10;
  PS := PS + '        if ($t -match ''(?i)^\s*CREATE\s+USER\b'') { continue }' + #13#10;
  PS := PS + '        if ($t -match ''(?i)^\s*ALTER\s+ROLE\b'') { continue }' + #13#10;
  PS := PS + '        if ($t -match ''(?i)^\s*IF\s+.*FULLTEXTSERVICEPROPERTY'') { continue }' + #13#10;
  PS := PS + '        $cmd = $conn.CreateCommand()' + #13#10;
  PS := PS + '        $cmd.CommandText    = $t' + #13#10;
  PS := PS + '        $cmd.CommandTimeout = 300' + #13#10;
  PS := PS + '        try { $cmd.ExecuteNonQuery() | Out-Null } catch {' + #13#10;
  PS := PS + '            $errCount++' + #13#10;
  PS := PS + '            if ($errCount -le 10) { $errMsgs += $_.Exception.Message }' + #13#10;
  PS := PS + '        }' + #13#10;
  PS := PS + '    }' + #13#10;
  PS := PS + '' + #13#10;

  PS := PS + '    $conn.Close()' + #13#10;
  PS := PS + '    if ($errCount -gt 0) {' + #13#10;
  PS := PS + '        "BD creada OK. Errores menores en $errCount batches (tablas/indices ya existentes):`r`n" + ($errMsgs | Out-String) | Out-File "$env:TEMP\dp_crear_bd_warnings.txt" -Encoding UTF8' + #13#10;
  PS := PS + '    }' + #13#10;
  PS := PS + '    exit 0' + #13#10;
  PS := PS + '} catch {' + #13#10;
  PS := PS + '    $_.Exception.Message | Out-File -FilePath "$env:TEMP\dp_crear_bd_error.txt" -Encoding UTF8' + #13#10;
  PS := PS + '    exit 1' + #13#10;
  PS := PS + '}' + #13#10;

  Result := PS;
end;


procedure CrearBaseDeDatos;
var
  PSPath:     String;
  ResultCode: Integer;
  PSContent:  String;
begin
  bBDDisponible := False;
  PSPath    := ExpandConstant('{tmp}\crear_bd.ps1');
  PSContent := BuildCrearBDScript;

  if not SaveStringToFile(PSPath, PSContent, False) then
  begin
    MsgBox(
      'No se pudo escribir el script de creacion de base de datos.' + #13#10 +
      'La instalacion continuara pero debera crear la BD manualmente.',
      mbError, MB_OK);
    Exit;
  end;

  if not Exec(
    ExpandConstant('{sys}\WindowsPowerShell\v1.0\powershell.exe'),
    '-ExecutionPolicy Bypass -NonInteractive -WindowStyle Hidden -File "' + PSPath + '"',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    MsgBox(
      'No se pudo ejecutar PowerShell.' + #13#10 +
      'Cree la base de datos manualmente usando el script SQL incluido.',
      mbError, MB_OK);
    DeleteFile(PSPath);
    Exit;
  end;

  DeleteFile(PSPath);

  if ResultCode = 0 then
  begin
    bBDDisponible := True;
  end
  else if ResultCode = 2 then
  begin
    MsgBox(
      'La base de datos "' + sNombreBD + '" no pudo ser creada.' + #13#10 +
      'Revise %TEMP%\dp_crear_bd_error.txt para detalles.' + #13#10 + #13#10 +
      'Las aplicaciones fueron instaladas correctamente.' + #13#10 +
      'Solo falta la base de datos para que funcionen.',
      mbError, MB_OK);
  end
  else
  begin
    MsgBox(
      'Hubo un error al crear la base de datos (codigo: ' + IntToStr(ResultCode) + ').' + #13#10 +
      'Revise el archivo %TEMP%\dp_crear_bd_error.txt para detalles.' + #13#10 + #13#10 +
      'Las aplicaciones fueron instaladas correctamente. Puede crear la BD manualmente.',
      mbError, MB_OK);
  end;
end;


// ============================================================
// REGISTRO DE TERMINAL (MODO LOCAL)
// ============================================================

procedure RegistrarTerminal;
var
  Connection:  Variant;
  ConnStr:     String;
  TerminalID:  String;
  sSQL:        String;
  dmy:         Variant;
begin
  TerminalID := GetEnv('COMPUTERNAME');
  ConnStr    := GetLocalOleDbConnectionString;

  try
    Connection := CreateOleObject('ADODB.Connection');
    Connection.Open(ConnStr);

    sSQL :=
      'EXEC GRALTerminales_SP'                              +
      ' @sTerminalID         = N''' + TerminalID  + ''','  +
      ' @sDescripcion        = N''Terminal ' + TerminalID + ''','  +
      ' @sSucursalID         = N'''','                      +
      ' @sMensajeBienVenida  = N''Bienvenido, Nmu()!'',' +
      ' @sIPV4               = N''''';

    dmy := Null;
    Connection.Execute(sSQL, dmy, 1);
    Connection.Close;

  except
    // El registro de terminal es opcional; no interrumpir la instalacion
  end;
end;


// ============================================================
// BOOTSTRAP IDENTITY (MODO LOCAL)
// ============================================================

function BuildBootstrapScript: String;
var
  PS: String;
begin
  PS := '$ErrorActionPreference = ''Stop''' + #13#10;
  PS := PS + '$server   = ''' + EscapePS(sDataSource) + '''' + #13#10;
  PS := PS + '$database = ''' + EscapePS(sNombreBD) + '''' + #13#10;
  PS := PS + '' + #13#10;

  PS := PS + 'function New-IdentityHash([string]$p) {' + #13#10;
  PS := PS + '    $s = New-Object byte[] 16' + #13#10;
  PS := PS + '    $rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider' + #13#10;
  PS := PS + '    $rng.GetBytes($s)' + #13#10;
  PS := PS + '    $dk = (New-Object System.Security.Cryptography.Rfc2898DeriveBytes -ArgumentList $p,$s,1000).GetBytes(32)' + #13#10;
  PS := PS + '    $h = New-Object byte[] 49; $h[0] = 0' + #13#10;
  PS := PS + '    [Array]::Copy($s,  0, $h,  1, 16)' + #13#10;
  PS := PS + '    [Array]::Copy($dk, 0, $h, 17, 32)' + #13#10;
  PS := PS + '    return [Convert]::ToBase64String($h)' + #13#10;
  PS := PS + '}' + #13#10;
  PS := PS + '' + #13#10;

  PS := PS + 'try {' + #13#10;
  PS := PS + '    $adminHash = New-IdentityHash ''Admin@1234''' + #13#10;
  PS := PS + '    $userHash  = New-IdentityHash ''User@1234''' + #13#10;
  PS := PS + '    $cs = "Server=$server;Database=$database;Integrated Security=True;Connect Timeout=30;"' + #13#10;
  PS := PS + '    $conn = New-Object System.Data.SqlClient.SqlConnection($cs)' + #13#10;
  PS := PS + '    $conn.Open()' + #13#10;
  PS := PS + '' + #13#10;

  PS := PS + '    foreach ($rol in @(''ADMINISTRADOR'', ''Registrado'')) {' + #13#10;
  PS := PS + '        $cmd = $conn.CreateCommand()' + #13#10;
  PS := PS + '        $cmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name=@n) " +' + #13#10;
  PS := PS + '            "INSERT INTO AspNetRoles(Id,Name) VALUES(CAST(NEWID() AS nvarchar(128)),@n)"' + #13#10;
  PS := PS + '        $cmd.Parameters.AddWithValue("@n", $rol) | Out-Null' + #13#10;
  PS := PS + '        $cmd.ExecuteNonQuery() | Out-Null' + #13#10;
  PS := PS + '    }' + #13#10;
  PS := PS + '' + #13#10;

  PS := PS + '    $cmd = $conn.CreateCommand()' + #13#10;
  PS := PS + '    $cmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE UserName=@un) BEGIN " +' + #13#10;
  PS := PS + '        "DECLARE @id NVARCHAR(128)=CAST(NEWID() AS NVARCHAR(128)); " +' + #13#10;
  PS := PS + '        "INSERT INTO AspNetUsers(Id,Email,EmailConfirmed,PasswordHash,SecurityStamp," +' + #13#10;
  PS := PS + '        "PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,UserName) " +' + #13#10;
  PS := PS + '        "VALUES(@id,@em,1,@ph,CAST(NEWID() AS NVARCHAR(128)),0,0,0,0,@un); " +' + #13#10;
  PS := PS + '        "INSERT INTO AspNetUserRoles(UserId,RoleId) SELECT @id,Id FROM AspNetRoles WHERE Name=@rol " +' + #13#10;
  PS := PS + '        "END"' + #13#10;
  PS := PS + '    $cmd.Parameters.AddWithValue("@un",  ''admin'') | Out-Null' + #13#10;
  PS := PS + '    $cmd.Parameters.AddWithValue("@em",  ''admin@digitalplus.com'') | Out-Null' + #13#10;
  PS := PS + '    $cmd.Parameters.AddWithValue("@ph",  $adminHash) | Out-Null' + #13#10;
  PS := PS + '    $cmd.Parameters.AddWithValue("@rol", ''ADMINISTRADOR'') | Out-Null' + #13#10;
  PS := PS + '    $cmd.ExecuteNonQuery() | Out-Null' + #13#10;
  PS := PS + '' + #13#10;

  PS := PS + '    $cmd = $conn.CreateCommand()' + #13#10;
  PS := PS + '    $cmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE UserName=@un) BEGIN " +' + #13#10;
  PS := PS + '        "DECLARE @id NVARCHAR(128)=CAST(NEWID() AS NVARCHAR(128)); " +' + #13#10;
  PS := PS + '        "INSERT INTO AspNetUsers(Id,Email,EmailConfirmed,PasswordHash,SecurityStamp," +' + #13#10;
  PS := PS + '        "PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,UserName) " +' + #13#10;
  PS := PS + '        "VALUES(@id,@em,1,@ph,CAST(NEWID() AS NVARCHAR(128)),0,0,0,0,@un); " +' + #13#10;
  PS := PS + '        "INSERT INTO AspNetUserRoles(UserId,RoleId) SELECT @id,Id FROM AspNetRoles WHERE Name=@rol " +' + #13#10;
  PS := PS + '        "END"' + #13#10;
  PS := PS + '    $cmd.Parameters.AddWithValue("@un",  ''user'') | Out-Null' + #13#10;
  PS := PS + '    $cmd.Parameters.AddWithValue("@em",  ''user@digitalplus.com'') | Out-Null' + #13#10;
  PS := PS + '    $cmd.Parameters.AddWithValue("@ph",  $userHash) | Out-Null' + #13#10;
  PS := PS + '    $cmd.Parameters.AddWithValue("@rol", ''Registrado'') | Out-Null' + #13#10;
  PS := PS + '    $cmd.ExecuteNonQuery() | Out-Null' + #13#10;
  PS := PS + '' + #13#10;

  PS := PS + '    $conn.Close()' + #13#10;
  PS := PS + '    exit 0' + #13#10;
  PS := PS + '} catch {' + #13#10;
  PS := PS + '    $_.Exception.Message | Out-File "$env:TEMP\dp_bootstrap_error.txt" -Encoding UTF8' + #13#10;
  PS := PS + '    exit 1' + #13#10;
  PS := PS + '}' + #13#10;

  Result := PS;
end;


procedure BootstrapIdentity;
var
  PSPath:     String;
  ResultCode: Integer;
begin
  PSPath := ExpandConstant('{tmp}\bootstrap_identity.ps1');

  if not SaveStringToFile(PSPath, BuildBootstrapScript, False) then
  begin
    MsgBox(
      'No se pudo escribir el script de inicializacion de usuarios.' + #13#10 +
      'La instalacion continuara pero debera crear los usuarios manualmente.',
      mbError, MB_OK);
    Exit;
  end;

  if not Exec(
    ExpandConstant('{sys}\WindowsPowerShell\v1.0\powershell.exe'),
    '-ExecutionPolicy Bypass -NonInteractive -WindowStyle Hidden -File "' + PSPath + '"',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    MsgBox(
      'No se pudo ejecutar PowerShell para inicializar usuarios.' + #13#10 +
      'Cree los usuarios manualmente.' + #13#10 + #13#10 +
      'Credenciales por defecto:' + #13#10 +
      '  Administrador: admin / Admin@1234' + #13#10 +
      '  Normal:        user  / User@1234',
      mbInformation, MB_OK);
    Exit;
  end;

  if ResultCode <> 0 then
    MsgBox(
      'Advertencia: no se pudieron crear los usuarios iniciales (codigo: ' + IntToStr(ResultCode) + ').' + #13#10 +
      'Revise %TEMP%\dp_bootstrap_error.txt para detalles.' + #13#10 + #13#10 +
      'Credenciales a crear manualmente:' + #13#10 +
      '  Administrador: admin / Admin@1234' + #13#10 +
      '  Normal:        user  / User@1234',
      mbInformation, MB_OK);

  DeleteFile(PSPath);
end;


// ============================================================
// SEGURIDAD: DPAPI + ACL
// ============================================================

procedure ProtegerConfigs;
var
  ResultCode: Integer;
  ToolPath, ConfigPath: String;
begin
  ToolPath := ExpandConstant('{app}\tools\ConfigProtector.exe');

  ConfigPath := ExpandConstant('{app}\Fichadas\TEntradaSalida.exe.config');
  if not Exec(ToolPath, '"' + ConfigPath + '"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    MsgBox('No se pudo ejecutar ConfigProtector para Fichadas.' + #13#10 +
           'La configuracion quedara sin cifrar.', mbError, MB_OK);
    Exit;
  end;
  if ResultCode <> 0 then
    MsgBox('Error al cifrar configuracion de Fichadas (codigo: ' + IntToStr(ResultCode) + ').' + #13#10 +
           'La configuracion quedara sin cifrar.', mbError, MB_OK);

  ConfigPath := ExpandConstant('{app}\Administrador\Acceso.exe.config');
  if not Exec(ToolPath, '"' + ConfigPath + '"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    MsgBox('No se pudo ejecutar ConfigProtector para Administrador.' + #13#10 +
           'La configuracion quedara sin cifrar.', mbError, MB_OK);
    Exit;
  end;
  if ResultCode <> 0 then
    MsgBox('Error al cifrar configuracion de Administrador (codigo: ' + IntToStr(ResultCode) + ').' + #13#10 +
           'La configuracion quedara sin cifrar.', mbError, MB_OK);
end;


procedure AplicarACL;
var
  ResultCode: Integer;
  AppDir: String;
begin
  AppDir := ExpandConstant('{app}');
  Exec('icacls.exe',
    '"' + AppDir + '" /inheritance:r ' +
    '/grant *S-1-5-32-544:(OI)(CI)F ' +
    '/grant *S-1-5-18:(OI)(CI)F ' +
    '/grant *S-1-5-32-545:(OI)(CI)RX',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;


// ============================================================
// POST-INSTALACION
// ============================================================

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin

    // ========== MODO LOCAL ==========
    if bModoLocal then
    begin
      // Paso 0: Sanitizar nombre de empresa para nombre de BD
      sNombreBD := SanitizarNombreBD(Trim(edtEmpresa.Text));

      // Paso 1: Detectar SQL Server (setea sNombreInstancia y sDataSource)
      bSQLInstalado := DetectarSQLServer;

      // Paso 2: Instalar SQL Express si no hay SQL
      if not bSQLInstalado then
      begin
        WizardForm.StatusLabel.Caption := 'Instalando SQL Server Express 2019 (esto puede tardar varios minutos)...';
        WizardForm.Repaint;
        InstalarSQLExpress;
        sNombreInstancia := 'SQLEXPRESS';
        sDataSource := '.\SQLEXPRESS';
      end;

      // Paso 3: Establecer connection string local
      sConnectionString := GetLocalConnectionString;

      // Paso 4: Crear BD
      WizardForm.StatusLabel.Caption := 'Creando base de datos ' + sNombreBD + '...';
      WizardForm.Repaint;
      CrearBaseDeDatos;

      // Paso 5: Escribir configs
      EscribirConfigFichador;
      EscribirConfigAdministrador;

      // Paso 6: Registrar terminal (solo si BD disponible)
      if bBDDisponible then
        RegistrarTerminal;

      // Paso 7: Bootstrap Identity (solo si BD disponible)
      if bBDDisponible then
      begin
        WizardForm.StatusLabel.Caption := 'Inicializando usuarios del sistema...';
        WizardForm.Repaint;
        BootstrapIdentity;
      end;
    end

    // ========== MODO NUBE ==========
    else
    begin
      // La connection string ya fue obtenida durante la validacion del codigo
      sConnectionString := sCloudConnStr;
      bBDDisponible := True;  // la BD fue creada por el provisioning en Azure

      // Escribir configs con la connection string de la nube
      EscribirConfigFichador;
      EscribirConfigAdministrador;

      // No se registra terminal ni bootstrap en modo nube
      // (la BD en la nube ya tiene su propia configuracion)
    end;

    // ========== COMUN A AMBOS MODOS ==========

    // Cifrar connectionStrings con DPAPI
    WizardForm.StatusLabel.Caption := 'Cifrando configuracion...';
    WizardForm.Repaint;
    ProtegerConfigs;

    // Aplicar ACL restrictiva
    AplicarACL;
  end;
end;
