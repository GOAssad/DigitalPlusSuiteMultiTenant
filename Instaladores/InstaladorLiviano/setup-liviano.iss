; ============================================================
; DigitalPlus Suite - Instalador Liviano v1.0
; Herramienta: Inno Setup 6.x
; Generado:    2026-03-07
;
; Instala simultaneamente:
;   - DigitalPlus Fichadas   (TEntradaSalida.exe)
;   - DigitalPlus Administrador (Acceso.exe)
;   - Driver DigitalPersona RTE
;
; NO instala SQL Server Express (la BD esta en la nube).
; Requiere codigo de activacion generado desde el portal.
; ============================================================

; --- Identidad del instalador liviano ---
#define AppName       "DigitalPlus Suite (Cloud)"
#define AppVersion    "1.0.0-202603202038"
#define AppPublisher  "DigitalOnePlus"
#define AppId         "{{A1B2C3D4-5E6F-7A8B-9C0D-E1F2A3B4C5D6}"

; --- Fichador ---
#define FichBin       "..\..\Fichador\TEntradaSalida\bin\Release"
#define FichExe       "TEntradaSalida.exe"
#define FichIcon      "..\..\Fichador\TEntradaSalida\Resources\Huellaksk.ico"

; --- Administrador ---
#define AdminBin      "..\..\Administrador\Acceso\bin\Release"
#define AdminExe      "Acceso.exe"
#define AdminIcon     "..\..\Administrador\Acceso\logo_web.ico"

; --- Recursos compartidos ---
#define SourceRTE_x86 "..\..\SDK\Digital-Persona-SDK-master\RTE\Install"
#define SourceRTE_x64 "..\..\SDK\Digital-Persona-SDK-master\RTE\Install\x64"

; --- Herramienta DPAPI para cifrar configs ---
#define SourceProtector "..\..\tools\ConfigProtector\bin\Release\net48"

; --- API del portal de licencias ---
#define PortalApiUrl "https://digitalpluslicencias.azurewebsites.net/api/activar"
#define PortalApiFreeUrl "https://digitalpluslicencias.azurewebsites.net/api/activar-free"
#define PortalApiPaisesUrl "https://digitalpluslicencias.azurewebsites.net/api/paises"
#define PortalApiValidarUrl "https://digitalpluslicencias.azurewebsites.net/api/validar-free"

; ============================================================
[Setup]
; ============================================================
AppId={#AppId}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
AppCopyright=Copyright (C) 2026 {#AppPublisher}

DefaultDirName=C:\DigitalPlusCloud
DisableDirPage=no
DefaultGroupName=DigitalPlus
AllowNoIcons=yes

OutputDir=.\Output
OutputBaseFilename=DigitalPlus_Cloud_Setup_v{#AppVersion}
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
Source: "{#FichBin}\AForge.dll";                     DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\AForge.Video.dll";               DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\AForge.Video.DirectShow.dll";    DestDir: "{app}\Fichadas"; Flags: ignoreversion
Source: "{#FichBin}\zxing.dll";                      DestDir: "{app}\Fichadas"; Flags: ignoreversion

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
Source: "{#AdminBin}\DPFPShrXTypeLibNET.dll";               DestDir: "{app}\Administrador"; Flags: ignoreversion skipifsourcedoesntexist
Source: "{#AdminBin}\DPFPVerNET.dll";                       DestDir: "{app}\Administrador"; Flags: ignoreversion
Source: "{#AdminBin}\DPFPCtlXTypeLibNET.dll";               DestDir: "{app}\Administrador"; Flags: ignoreversion skipifsourcedoesntexist
Source: "{#AdminBin}\AxInterop.DPFPCtlXLib.dll";            DestDir: "{app}\Administrador"; Flags: ignoreversion skipifsourcedoesntexist
Source: "{#AdminBin}\Interop.DPFPCtlXLib.dll";              DestDir: "{app}\Administrador"; Flags: ignoreversion skipifsourcedoesntexist

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

; --- Driver DigitalPersona RTE ---
Source: "{#SourceRTE_x86}\Setup.msi"; DestDir: "{tmp}"; DestName: "DP_RTE_x86.msi"; \
    Flags: deleteafterinstall
Source: "{#SourceRTE_x64}\Setup.msi"; DestDir: "{tmp}"; DestName: "DP_RTE_x64.msi"; \
    Flags: deleteafterinstall

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

; Opciones post-instalacion
Filename: "{app}\Fichadas\{#FichExe}"; \
    Description: "Ejecutar DigitalPlus Fichadas ahora"; \
    Flags: nowait postinstall skipifsilent unchecked

Filename: "{app}\Administrador\{#AdminExe}"; \
    Description: "Ejecutar DigitalPlus Administrador ahora"; \
    Flags: nowait postinstall skipifsilent unchecked

// ============================================================
[Code]
// ============================================================

// ------------------------------------------------------------
// Variables globales
// ------------------------------------------------------------
var
  // Pagina: Seleccion de modo
  ModoPage:         TWizardPage;
  rbCodigo:         TNewRadioButton;
  rbFree:           TNewRadioButton;
  lblModoDesc:      TNewStaticText;

  // Pagina: Codigo de activacion
  ActivacionPage:   TWizardPage;
  lblActivacion:    TNewStaticText;
  edtActivacion:    TNewEdit;
  btnActivar:       TNewButton;
  lblActivResult:   TNewStaticText;

  // Pagina: Plan Free
  FreePage:         TWizardPage;
  lblFreeDesc:      TNewStaticText;
  lblFreeNombre:    TNewStaticText;
  edtFreeNombre:    TNewEdit;
  lblFreeEmail:     TNewStaticText;
  edtFreeEmail:     TNewEdit;
  lblFreePais:      TNewStaticText;
  cmbFreePais:      TNewComboBox;
  lblFreeResult:    TNewStaticText;
  paisIds:          array of Integer;

  // Estado interno
  sConnectionString:      String;    // Connection string a DigitalPlusMultiTenant
  sAdminConnectionString: String;    // Connection string a DigitalPlusAdmin
  bActivacionOK:          Boolean;   // True si el codigo fue validado
  sNombreEmpresa:         String;    // Nombre de la empresa (viene del portal)
  sEmpresaId:             String;    // EmpresaId del tenant en DigitalPlusMultiTenant
  sAdminEmpresaId:        String;    // Id de la empresa en DigitalPlusAdmin
  sUrlPortal:             String;    // URL del portal web de la empresa
  sAdminEmail:            String;    // Email del admin (viene del portal)
  sAdminPassword:         String;    // Password del admin (viene del portal)
  bModoFree:              Boolean;   // True si eligio plan Free
  nFreePaisId:            Integer;   // PaisId seleccionado en combo Free
  sFreeValidacion:        String;    // Resultado de validacion previa

// ============================================================
// UTILIDADES
// ============================================================

// Posicion de caracter en string desde un offset (PosEx no existe en Inno)
function PosEx(const SubStr, S: String; Offset: Integer): Integer;
var
  sSub: String;
  i: Integer;
begin
  Result := 0;
  if Offset > Length(S) then Exit;
  sSub := Copy(S, Offset, Length(S) - Offset + 1);
  i := Pos(SubStr, sSub);
  if i > 0 then
    Result := Offset + i - 1;
end;

// ============================================================
// ACTIVACION VIA PORTAL DE LICENCIAS
// ============================================================

// Llama al portal DigitalPlus Licencias para validar el codigo
// y obtener la connection string y datos de empresa
function ActivarCodigo(const sCodigo: String): Boolean;
var
  WinHttpReq: Variant;
  sUrl, sBody, sResponse: String;
  StatusCode: Integer;
  iPos, iEnd: Integer;
begin
  Result := False;
  sConnectionString := '';
  sAdminConnectionString := '';
  sNombreEmpresa := '';
  sEmpresaId := '';
  sAdminEmpresaId := '';
  sUrlPortal := '';
  sAdminEmail := '';
  sAdminPassword := '';

  sUrl := '{#PortalApiUrl}';
  sBody := '{"Codigo":"' + sCodigo + '"}';

  try
    WinHttpReq := CreateOleObject('WinHttp.WinHttpRequest.5.1');
    WinHttpReq.Option(9) := $0A80;
    WinHttpReq.SetTimeouts(10000, 15000, 30000, 120000);
    WinHttpReq.Open('POST', sUrl, False);
    WinHttpReq.SetRequestHeader('Content-Type', 'application/json');
    WinHttpReq.Send(sBody);

    StatusCode := WinHttpReq.Status;
    sResponse := WinHttpReq.ResponseText;

    if StatusCode = 200 then
    begin
      // Extraer connectionString del JSON (BD DigitalPlusMultiTenant)
      iPos := Pos('connectionString":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('connectionString":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sConnectionString := Copy(sResponse, iPos, iEnd - iPos);
      end;

      // Extraer adminConnectionString del JSON (BD DigitalPlusAdmin)
      iPos := Pos('adminConnectionString":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('adminConnectionString":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sAdminConnectionString := Copy(sResponse, iPos, iEnd - iPos);
      end;

      // Extraer empresaId del JSON (EmpresaId en DigitalPlusMultiTenant)
      iPos := Pos('empresaId":', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('empresaId":');
        iEnd := PosEx(',', sResponse, iPos);
        if iEnd = 0 then
          iEnd := PosEx('}', sResponse, iPos);
        if iEnd > iPos then
          sEmpresaId := Trim(Copy(sResponse, iPos, iEnd - iPos));
      end;

      // Extraer adminEmpresaId del JSON (Id en DigitalPlusAdmin)
      iPos := Pos('adminEmpresaId":', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('adminEmpresaId":');
        iEnd := PosEx(',', sResponse, iPos);
        if iEnd = 0 then
          iEnd := PosEx('}', sResponse, iPos);
        if iEnd > iPos then
          sAdminEmpresaId := Trim(Copy(sResponse, iPos, iEnd - iPos));
      end;

      // Extraer nombreEmpresa del JSON
      iPos := Pos('nombreEmpresa":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('nombreEmpresa":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sNombreEmpresa := Copy(sResponse, iPos, iEnd - iPos);
      end;

      // Extraer urlPortal del JSON
      iPos := Pos('urlPortal":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('urlPortal":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sUrlPortal := Copy(sResponse, iPos, iEnd - iPos);
      end;

      // Extraer email del admin
      iPos := Pos('"email":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('"email":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sAdminEmail := Copy(sResponse, iPos, iEnd - iPos);
      end;

      // Extraer password del admin
      iPos := Pos('"password":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('"password":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sAdminPassword := Copy(sResponse, iPos, iEnd - iPos);
      end;

      if (sConnectionString <> '') and (sEmpresaId <> '') then
        Result := True;
    end;
  except
    // Error de conexion
  end;
end;

// Carga la lista de paises desde la API
procedure CargarPaises;
var
  WinHttpReq: Variant;
  sResponse: String;
  iPos, iEnd, iIdPos, iIdEnd: Integer;
  sNombre: String;
  nId: Integer;
  n: Integer;
begin
  n := 0;
  try
    WinHttpReq := CreateOleObject('WinHttp.WinHttpRequest.5.1');
    WinHttpReq.Option(9) := $0A80;
    WinHttpReq.Open('GET', '{#PortalApiPaisesUrl}', False);
    WinHttpReq.Send('');

    if WinHttpReq.Status = 200 then
    begin
      sResponse := WinHttpReq.ResponseText;
      // Parse JSON array: [{"id":1,"nombre":"Argentina"},{"id":2,"nombre":"Chile"}]
      iPos := 1;
      while iPos < Length(sResponse) do
      begin
        // Buscar "id":
        iIdPos := PosEx('"id":', sResponse, iPos);
        if iIdPos = 0 then Break;
        iIdPos := iIdPos + Length('"id":');
        iIdEnd := PosEx(',', sResponse, iIdPos);
        if iIdEnd = 0 then iIdEnd := PosEx('}', sResponse, iIdPos);
        nId := StrToIntDef(Trim(Copy(sResponse, iIdPos, iIdEnd - iIdPos)), 0);

        // Buscar "nombre":"
        iPos := PosEx('"nombre":"', sResponse, iIdEnd);
        if iPos = 0 then Break;
        iPos := iPos + Length('"nombre":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd = 0 then Break;
        sNombre := Copy(sResponse, iPos, iEnd - iPos);

        // Agregar al combo
        cmbFreePais.Items.Add(sNombre);
        SetArrayLength(paisIds, n + 1);
        paisIds[n] := nId;
        n := n + 1;

        iPos := iEnd + 1;
      end;

      // Seleccionar el primero por defecto
      if cmbFreePais.Items.Count > 0 then
        cmbFreePais.ItemIndex := 0;
    end;
  except
    // Si falla, dejamos el combo vacio
    cmbFreePais.Items.Add('(No se pudieron cargar los paises)');
    cmbFreePais.ItemIndex := 0;
  end;
end;

// Valida datos antes de instalar (email duplicado, nombre duplicado)
function ValidarDatosFree(const sNombre, sEmail: String): String;
var
  WinHttpReq: Variant;
  sUrl, sBody, sResponse: String;
  StatusCode: Integer;
  iPos, iEnd: Integer;
begin
  Result := ''; // vacio = OK

  sUrl := '{#PortalApiValidarUrl}';
  sBody := '{"Nombre":"' + sNombre + '","Email":"' + sEmail + '","PaisId":0}';

  try
    WinHttpReq := CreateOleObject('WinHttp.WinHttpRequest.5.1');
    WinHttpReq.Option(9) := $0A80;
    WinHttpReq.SetTimeouts(10000, 15000, 30000, 30000);
    WinHttpReq.Open('POST', sUrl, False);
    WinHttpReq.SetRequestHeader('Content-Type', 'application/json');
    WinHttpReq.Send(sBody);

    StatusCode := WinHttpReq.Status;
    sResponse := WinHttpReq.ResponseText;

    if StatusCode = 400 then
    begin
      // Error de validacion - extraer mensaje
      iPos := Pos('error":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('error":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          Result := Copy(sResponse, iPos, iEnd - iPos);
      end;
      if Result = '' then
        Result := 'Error de validacion';
    end
    else if StatusCode <> 200 then
    begin
      Result := 'Error de conexion (HTTP ' + IntToStr(StatusCode) + ')';
    end;
  except
    Result := 'No se pudo conectar al servidor. Verifique su conexion a internet.';
  end;
end;

// Llama al portal para registrar empresa con plan Free
function ActivarFree(const sNombre, sEmail: String; nPaisId: Integer): Boolean;
var
  WinHttpReq: Variant;
  sUrl, sBody, sResponse: String;
  StatusCode: Integer;
  iPos, iEnd: Integer;
begin
  Result := False;
  sConnectionString := '';
  sAdminConnectionString := '';
  sNombreEmpresa := '';
  sEmpresaId := '';
  sAdminEmpresaId := '';
  sUrlPortal := '';

  sUrl := '{#PortalApiFreeUrl}';
  sBody := '{"Nombre":"' + sNombre + '","Email":"' + sEmail + '","PaisId":' + IntToStr(nPaisId) + '}';

  try
    WinHttpReq := CreateOleObject('WinHttp.WinHttpRequest.5.1');
    // Habilitar TLS 1.2 (0x0800) + TLS 1.1 (0x0200) + TLS 1.0 (0x0080)
    WinHttpReq.Option(9) := $0A80;
    // Timeout: resolver=10s, conectar=15s, enviar=30s, recibir=120s (provisioning puede tardar)
    WinHttpReq.SetTimeouts(10000, 15000, 30000, 120000);
    WinHttpReq.Open('POST', sUrl, False);
    WinHttpReq.SetRequestHeader('Content-Type', 'application/json');
    WinHttpReq.Send(sBody);

    StatusCode := WinHttpReq.Status;
    sResponse := WinHttpReq.ResponseText;

    if StatusCode = 200 then
    begin
      // Extraer connectionString
      iPos := Pos('connectionString":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('connectionString":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sConnectionString := Copy(sResponse, iPos, iEnd - iPos);
      end;

      // Extraer adminConnectionString
      iPos := Pos('adminConnectionString":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('adminConnectionString":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sAdminConnectionString := Copy(sResponse, iPos, iEnd - iPos);
      end;

      // Extraer empresaId
      iPos := Pos('empresaId":', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('empresaId":');
        iEnd := PosEx(',', sResponse, iPos);
        if iEnd = 0 then
          iEnd := PosEx('}', sResponse, iPos);
        if iEnd > iPos then
          sEmpresaId := Trim(Copy(sResponse, iPos, iEnd - iPos));
      end;

      // Extraer adminEmpresaId
      iPos := Pos('adminEmpresaId":', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('adminEmpresaId":');
        iEnd := PosEx(',', sResponse, iPos);
        if iEnd = 0 then
          iEnd := PosEx('}', sResponse, iPos);
        if iEnd > iPos then
          sAdminEmpresaId := Trim(Copy(sResponse, iPos, iEnd - iPos));
      end;

      // Extraer nombreEmpresa
      iPos := Pos('nombreEmpresa":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('nombreEmpresa":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sNombreEmpresa := Copy(sResponse, iPos, iEnd - iPos);
      end;

      // Extraer urlPortal
      iPos := Pos('urlPortal":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('urlPortal":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sUrlPortal := Copy(sResponse, iPos, iEnd - iPos);
      end;

      if (sConnectionString <> '') and (sEmpresaId <> '') and (sEmpresaId <> '0') then
        Result := True
      else
        sNombreEmpresa := 'La empresa se registro pero no se pudo completar el aprovisionamiento. EmpresaId=' + sEmpresaId;
    end
    else if StatusCode = 409 then
    begin
      // Empresa ya existe
      iPos := Pos('error":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('error":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sNombreEmpresa := Copy(sResponse, iPos, iEnd - iPos);
      end;
      if sNombreEmpresa = '' then
        sNombreEmpresa := 'Ya existe una empresa con ese nombre';
    end;
  except
    // Error de conexion
  end;
end;

// ============================================================
// PAGINAS CUSTOM DEL WIZARD
// ============================================================

// --- Handler boton Activar Codigo ---
procedure OnActivarClick(Sender: TObject);
var
  sCodigo: String;
begin
  sCodigo := Trim(edtActivacion.Text);
  if sCodigo = '' then
  begin
    lblActivResult.Caption := 'Ingrese un codigo de activacion.';
    lblActivResult.Font.Color := clRed;
    Exit;
  end;

  lblActivResult.Caption := 'Validando codigo...';
  lblActivResult.Font.Color := clWindowText;
  WizardForm.Update;

  if ActivarCodigo(sCodigo) then
  begin
    bActivacionOK := True;
    lblActivResult.Caption := 'Codigo valido. Empresa: ' + sNombreEmpresa + ' (ID: ' + sEmpresaId + ')';
    lblActivResult.Font.Color := clGreen;
  end
  else
  begin
    bActivacionOK := False;
    lblActivResult.Caption := 'Codigo invalido o expirado. Intente nuevamente.';
    lblActivResult.Font.Color := clRed;
  end;
end;

// (La logica de registro Free se ejecuta en NextButtonClick)

// --- Pagina: Seleccion de modo ---
procedure CreateModoPage;
begin
  ModoPage := CreateCustomPage(wpSelectTasks,
    'Tipo de Instalacion',
    'Seleccione como desea activar DigitalPlus');

  lblModoDesc := TNewStaticText.Create(WizardForm);
  lblModoDesc.Parent := ModoPage.Surface;
  lblModoDesc.Caption :=
    'Seleccione una opcion:';
  lblModoDesc.Left := 0;
  lblModoDesc.Top := 0;
  lblModoDesc.Width := ModoPage.SurfaceWidth;
  lblModoDesc.WordWrap := True;

  rbCodigo := TNewRadioButton.Create(WizardForm);
  rbCodigo.Parent := ModoPage.Surface;
  rbCodigo.Caption := 'Tengo un codigo de activacion';
  rbCodigo.Left := 10;
  rbCodigo.Top := lblModoDesc.Top + lblModoDesc.Height + 20;
  rbCodigo.Width := ModoPage.SurfaceWidth - 20;
  rbCodigo.Font.Style := [fsBold];
  rbCodigo.Checked := True;

  rbFree := TNewRadioButton.Create(WizardForm);
  rbFree.Parent := ModoPage.Surface;
  rbFree.Caption := 'Instalar version gratuita (hasta 5 legajos)';
  rbFree.Left := 10;
  rbFree.Top := rbCodigo.Top + rbCodigo.Height + 15;
  rbFree.Width := ModoPage.SurfaceWidth - 20;
  rbFree.Font.Style := [fsBold];
end;

// --- Pagina: Codigo de activacion ---
procedure CreateActivacionPage;
begin
  ActivacionPage := CreateCustomPage(ModoPage.ID,
    'Codigo de Activacion',
    'Ingrese el codigo proporcionado por su proveedor');

  lblActivacion := TNewStaticText.Create(WizardForm);
  lblActivacion.Parent := ActivacionPage.Surface;
  lblActivacion.Caption :=
    'Este instalador requiere un codigo de activacion para conectar ' +
    'con la base de datos en la nube.' + #13#10 + #13#10 +
    'El codigo le fue enviado por el administrador del sistema. ' +
    'Si no lo tiene, contacte a soporte.';
  lblActivacion.Left := 0;
  lblActivacion.Top := 0;
  lblActivacion.Width := ActivacionPage.SurfaceWidth;
  lblActivacion.WordWrap := True;

  edtActivacion := TNewEdit.Create(WizardForm);
  edtActivacion.Parent := ActivacionPage.Surface;
  edtActivacion.Left := 0;
  edtActivacion.Top := lblActivacion.Top + lblActivacion.Height + 20;
  edtActivacion.Width := 300;
  edtActivacion.Font.Size := 12;

  btnActivar := TNewButton.Create(WizardForm);
  btnActivar.Parent := ActivacionPage.Surface;
  btnActivar.Caption := 'Validar Codigo';
  btnActivar.Left := edtActivacion.Left + edtActivacion.Width + 10;
  btnActivar.Top := edtActivacion.Top;
  btnActivar.Width := 120;
  btnActivar.Height := edtActivacion.Height;
  btnActivar.OnClick := @OnActivarClick;

  lblActivResult := TNewStaticText.Create(WizardForm);
  lblActivResult.Parent := ActivacionPage.Surface;
  lblActivResult.Left := 0;
  lblActivResult.Top := edtActivacion.Top + edtActivacion.Height + 15;
  lblActivResult.Width := ActivacionPage.SurfaceWidth;
  lblActivResult.WordWrap := True;
  lblActivResult.Caption := '';
end;

// --- Pagina: Plan Free ---
procedure CreateFreePage;
begin
  FreePage := CreateCustomPage(ModoPage.ID,
    'Version Gratuita',
    'Complete los datos de su empresa para activar el plan Free');

  lblFreeDesc := TNewStaticText.Create(WizardForm);
  lblFreeDesc.Parent := FreePage.Surface;
  lblFreeDesc.Caption :=
    'El plan gratuito incluye:' + #13#10 +
    '  - Hasta 5 legajos' + #13#10 +
    '  - 1 sucursal' + #13#10 +
    '  - 200 fichadas por mes' + #13#10 +
    '  - Sin vencimiento' + #13#10 + #13#10 +
    'Complete los datos para registrar su empresa:';
  lblFreeDesc.Left := 0;
  lblFreeDesc.Top := 0;
  lblFreeDesc.Width := FreePage.SurfaceWidth;
  lblFreeDesc.WordWrap := True;

  lblFreeNombre := TNewStaticText.Create(WizardForm);
  lblFreeNombre.Parent := FreePage.Surface;
  lblFreeNombre.Caption := 'Nombre de la empresa:';
  lblFreeNombre.Left := 0;
  lblFreeNombre.Top := lblFreeDesc.Top + lblFreeDesc.Height + 15;

  edtFreeNombre := TNewEdit.Create(WizardForm);
  edtFreeNombre.Parent := FreePage.Surface;
  edtFreeNombre.Left := 0;
  edtFreeNombre.Top := lblFreeNombre.Top + lblFreeNombre.Height + 5;
  edtFreeNombre.Width := 350;
  edtFreeNombre.Font.Size := 11;

  lblFreeEmail := TNewStaticText.Create(WizardForm);
  lblFreeEmail.Parent := FreePage.Surface;
  lblFreeEmail.Caption := 'Email del administrador:';
  lblFreeEmail.Left := 0;
  lblFreeEmail.Top := edtFreeNombre.Top + edtFreeNombre.Height + 15;

  edtFreeEmail := TNewEdit.Create(WizardForm);
  edtFreeEmail.Parent := FreePage.Surface;
  edtFreeEmail.Left := 0;
  edtFreeEmail.Top := lblFreeEmail.Top + lblFreeEmail.Height + 5;
  edtFreeEmail.Width := 350;
  edtFreeEmail.Font.Size := 11;

  lblFreePais := TNewStaticText.Create(WizardForm);
  lblFreePais.Parent := FreePage.Surface;
  lblFreePais.Caption := 'Pais:';
  lblFreePais.Left := 0;
  lblFreePais.Top := edtFreeEmail.Top + edtFreeEmail.Height + 15;

  cmbFreePais := TNewComboBox.Create(WizardForm);
  cmbFreePais.Parent := FreePage.Surface;
  cmbFreePais.Left := 0;
  cmbFreePais.Top := lblFreePais.Top + lblFreePais.Height + 5;
  cmbFreePais.Width := 350;
  cmbFreePais.Style := csDropDownList;

  lblFreeResult := TNewStaticText.Create(WizardForm);
  lblFreeResult.Parent := FreePage.Surface;
  lblFreeResult.Left := 0;
  lblFreeResult.Top := cmbFreePais.Top + cmbFreePais.Height + 20;
  lblFreeResult.Width := FreePage.SurfaceWidth;
  lblFreeResult.WordWrap := True;
  lblFreeResult.Caption := '';
end;

// ============================================================
// CONFIGURACION POST-INSTALACION
// ============================================================

procedure WriteConfigs;
var
  FilePath: String;
  Lines: TArrayOfString;
  i: Integer;
begin
  // --- Fichador config ---
  FilePath := ExpandConstant('{app}\Fichadas\TEntradaSalida.exe.config');
  if LoadStringsFromFile(FilePath, Lines) then
  begin
    for i := 0 to GetArrayLength(Lines) - 1 do
    begin
      StringChange(Lines[i], '{{CONNECTION_STRING}}', sConnectionString);
      StringChange(Lines[i], '{{ADMIN_CONNECTION_STRING}}', sAdminConnectionString);
      StringChange(Lines[i], '{{EMPRESA_ID}}', sEmpresaId);
      StringChange(Lines[i], '{{ADMIN_EMPRESA_ID}}', sAdminEmpresaId);
      StringChange(Lines[i], '{{NOMBRE_EMPRESA}}', sNombreEmpresa);
      StringChange(Lines[i], '{{URL_PORTAL}}', sUrlPortal);
    end;
    SaveStringsToFile(FilePath, Lines, False);
  end;

  // --- Administrador config ---
  FilePath := ExpandConstant('{app}\Administrador\Acceso.exe.config');
  if LoadStringsFromFile(FilePath, Lines) then
  begin
    for i := 0 to GetArrayLength(Lines) - 1 do
    begin
      StringChange(Lines[i], '{{CONNECTION_STRING}}', sConnectionString);
      StringChange(Lines[i], '{{ADMIN_CONNECTION_STRING}}', sAdminConnectionString);
      StringChange(Lines[i], '{{EMPRESA_ID}}', sEmpresaId);
      StringChange(Lines[i], '{{ADMIN_EMPRESA_ID}}', sAdminEmpresaId);
      StringChange(Lines[i], '{{NOMBRE_EMPRESA}}', sNombreEmpresa);
      StringChange(Lines[i], '{{URL_PORTAL}}', sUrlPortal);
    end;
    SaveStringsToFile(FilePath, Lines, False);
  end;
end;

procedure EncryptConfigs;
var
  sProtector: String;
  ResultCode: Integer;
begin
  sProtector := ExpandConstant('{app}\tools\ConfigProtector.exe');
  if FileExists(sProtector) then
  begin
    Exec(sProtector,
      '"' + ExpandConstant('{app}\Fichadas\TEntradaSalida.exe.config') + '"',
      '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
    Exec(sProtector,
      '"' + ExpandConstant('{app}\Administrador\Acceso.exe.config') + '"',
      '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  end;
end;

procedure ApplySecurity;
var
  ResultCode: Integer;
  sDir: String;
begin
  sDir := ExpandConstant('{app}');
  Exec('icacls.exe',
    '"' + sDir + '" /inheritance:r ' +
    '/grant *S-1-5-32-544:(OI)(CI)F ' +
    '/grant *S-1-5-18:(OI)(CI)F ' +
    '/grant *S-1-5-32-545:(OI)(CI)RX',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;

// ============================================================
// EVENTOS DEL WIZARD
// ============================================================

function InitializeSetup: Boolean;
var
  Version: TWindowsVersion;
begin
  Result := True;

  // Verificar 64 bits
  if not IsWin64 then
  begin
    // Permitir pero advertir (los drivers x86 se instalaran)
  end;

  // Verificar .NET Framework 4.8
  GetWindowsVersionEx(Version);
  // La verificacion real se hace por registro
  // pero para simplificar, confiamos en que si el SO soporta
  // InnoSetup moderno, tiene .NET 4.8
end;

procedure InitializeWizard;
begin
  bActivacionOK := False;
  bModoFree := False;
  sConnectionString := '';
  sAdminConnectionString := '';
  sNombreEmpresa := '';
  sEmpresaId := '';
  sAdminEmpresaId := '';
  sUrlPortal := '';

  CreateModoPage;
  CreateActivacionPage;
  CreateFreePage;
  CargarPaises;
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := False;
  // Saltar pagina de codigo si eligio Free
  if PageID = ActivacionPage.ID then
    Result := bModoFree;
  // Saltar pagina Free si eligio codigo
  if PageID = FreePage.ID then
    Result := not bModoFree;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;

  // Al salir de la pagina de modo, guardar seleccion
  if CurPageID = ModoPage.ID then
  begin
    bModoFree := rbFree.Checked;
    // Resetear activacion al cambiar de modo
    bActivacionOK := False;
  end;

  // Validar que el codigo fue activado antes de avanzar
  if CurPageID = ActivacionPage.ID then
  begin
    if not bActivacionOK then
    begin
      MsgBox('Debe validar un codigo de activacion antes de continuar.',
        mbError, MB_OK);
      Result := False;
    end;
  end;

  // Pagina Free: solo validar campos obligatorios (el registro se hace post-install)
  if CurPageID = FreePage.ID then
  begin
    if Trim(edtFreeNombre.Text) = '' then
    begin
      lblFreeResult.Caption := 'Ingrese el nombre de su empresa.';
      lblFreeResult.Font.Color := clRed;
      Result := False;
      Exit;
    end;
    if Trim(edtFreeEmail.Text) = '' then
    begin
      lblFreeResult.Caption := 'Ingrese un email de contacto.';
      lblFreeResult.Font.Color := clRed;
      Result := False;
      Exit;
    end;
    if cmbFreePais.ItemIndex < 0 then
    begin
      lblFreeResult.Caption := 'Seleccione un pais.';
      lblFreeResult.Font.Color := clRed;
      Result := False;
      Exit;
    end;

    // Guardar PaisId para usarlo en post-install
    nFreePaisId := 0;
    if (cmbFreePais.ItemIndex >= 0) and (cmbFreePais.ItemIndex < GetArrayLength(paisIds)) then
      nFreePaisId := paisIds[cmbFreePais.ItemIndex];

    // Validar contra el servidor (email duplicado, etc.)
    lblFreeResult.Caption := 'Validando datos...';
    lblFreeResult.Font.Color := clWindowText;
    WizardForm.Update;

    sFreeValidacion := ValidarDatosFree(Trim(edtFreeNombre.Text), Trim(edtFreeEmail.Text));
    if sFreeValidacion <> '' then
    begin
      lblFreeResult.Caption := sFreeValidacion;
      lblFreeResult.Font.Color := clRed;
      Result := False;
      Exit;
    end;

    lblFreeResult.Caption := '';
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // 0. Si es modo Free, registrar la empresa ahora (post-install, archivos ya copiados)
    if bModoFree and (not bActivacionOK) then
    begin
      WizardForm.StatusLabel.Caption := 'Registrando empresa en la nube...';
      WizardForm.Update;

      if ActivarFree(Trim(edtFreeNombre.Text), Trim(edtFreeEmail.Text), nFreePaisId) then
      begin
        bActivacionOK := True;
        MsgBox('Empresa registrada exitosamente.' + #13#10 + #13#10 +
          'Se enviaron las credenciales de acceso a:' + #13#10 +
          Trim(edtFreeEmail.Text) + #13#10 + #13#10 +
          'Utilice esas credenciales para ingresar al portal web' + #13#10 +
          'y a la aplicacion Administrador.',
          mbInformation, MB_OK);
      end
      else
      begin
        // Si falla, avisar pero no abortar (los archivos ya estan instalados)
        if sNombreEmpresa <> '' then
          MsgBox('Advertencia: ' + sNombreEmpresa + #13#10 +
            'Los archivos se instalaron pero no se pudo registrar la empresa.' + #13#10 +
            'Contacte a soporte para completar la activacion.',
            mbError, MB_OK)
        else
          MsgBox('No se pudo registrar la empresa en la nube.' + #13#10 +
            'Verifique su conexion a internet y contacte a soporte.',
            mbError, MB_OK);
      end;
    end;

    // 1. Escribir connection string en configs
    if bActivacionOK then
    begin
      WizardForm.StatusLabel.Caption := 'Configurando aplicaciones...';
      WizardForm.Update;
      WriteConfigs;

      // 2. Cifrar configs con DPAPI
      WizardForm.StatusLabel.Caption := 'Protegiendo configuracion...';
      WizardForm.Update;
      EncryptConfigs;

      // 3. Mostrar credenciales de acceso (si vienen del servidor)
      if (not bModoFree) and (sAdminEmail <> '') then
      begin
        MsgBox('Instalacion completada exitosamente.' + #13#10 + #13#10 +
          'Credenciales de acceso:' + #13#10 +
          'Usuario: ' + sAdminEmail + #13#10 +
          'Contrasena: ' + sAdminPassword + #13#10 + #13#10 +
          'Utilice estas credenciales para ingresar al portal web' + #13#10 +
          'y a la aplicacion Administrador.' + #13#10 + #13#10 +
          'IMPORTANTE: Debera cambiar la contrasena en el primer inicio de sesion.',
          mbInformation, MB_OK);
      end;
    end;

    // 3. Aplicar seguridad ACL
    WizardForm.StatusLabel.Caption := 'Aplicando permisos de seguridad...';
    WizardForm.Update;
    ApplySecurity;
  end;
end;
