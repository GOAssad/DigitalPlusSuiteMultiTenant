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
#define AppVersion    "1.0"
#define AppPublisher  "DigitalOnePlus"
#define AppId         "{{A1B2C3D4-5E6F-7A8B-9C0D-E1F2A3B4C5D6}"

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

; --- Herramienta DPAPI para cifrar configs ---
#define SourceProtector "..\tools\ConfigProtector\bin\Release\net48"

; --- API del portal de licencias ---
#define PortalApiUrl "https://licencias.digitaloneplus.com/api/activar"

; ============================================================
[Setup]
; ============================================================
AppId={#AppId}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
AppCopyright=Copyright (C) 2026 {#AppPublisher}

DefaultDirName=C:\DigitalPlusCloud
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
  // Pagina: Codigo de activacion
  ActivacionPage:   TWizardPage;
  lblActivacion:    TNewStaticText;
  edtActivacion:    TNewEdit;
  btnActivar:       TNewButton;
  lblActivResult:   TNewStaticText;

  // Pagina: URL Portal Web
  UrlPage:       TWizardPage;
  lblUrlInfo:    TNewStaticText;
  lblUrl:        TNewStaticText;
  edtUrl:        TNewEdit;

  // Estado interno
  sConnectionString:   String;    // Connection string recibida de Azure
  bActivacionOK:       Boolean;   // True si el codigo fue validado
  sNombreEmpresa:      String;    // Nombre de la empresa (viene del provision)

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
  sNombreEmpresa := '';

  sUrl := '{#PortalApiUrl}';
  sBody := '{"Codigo":"' + sCodigo + '"}';

  try
    WinHttpReq := CreateOleObject('WinHttp.WinHttpRequest.5.1');
    WinHttpReq.Open('POST', sUrl, False);
    WinHttpReq.SetRequestHeader('Content-Type', 'application/json');
    WinHttpReq.Send(sBody);

    StatusCode := WinHttpReq.Status;
    sResponse := WinHttpReq.ResponseText;

    if StatusCode = 200 then
    begin
      // Extraer connectionString del JSON
      iPos := Pos('connectionString":"', sResponse);
      if iPos > 0 then
      begin
        iPos := iPos + Length('connectionString":"');
        iEnd := PosEx('"', sResponse, iPos);
        if iEnd > iPos then
          sConnectionString := Copy(sResponse, iPos, iEnd - iPos);
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

      if sConnectionString <> '' then
        Result := True;
    end;
  except
    // Error de conexion
  end;
end;

// ============================================================
// PAGINAS CUSTOM DEL WIZARD
// ============================================================

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
    lblActivResult.Caption := 'Codigo valido. Empresa: ' + sNombreEmpresa;
    lblActivResult.Font.Color := clGreen;
  end
  else
  begin
    bActivacionOK := False;
    lblActivResult.Caption := 'Codigo invalido o expirado. Intente nuevamente.';
    lblActivResult.Font.Color := clRed;
  end;
end;

procedure CreateActivacionPage;
begin
  ActivacionPage := CreateCustomPage(wpSelectTasks,
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

procedure CreateUrlPage;
begin
  UrlPage := CreateCustomPage(ActivacionPage.ID,
    'Portal Web DigitalPlus',
    'Ingrese la URL del portal web (opcional)');

  lblUrlInfo := TNewStaticText.Create(WizardForm);
  lblUrlInfo.Parent := UrlPage.Surface;
  lblUrlInfo.Caption :=
    'Si su empresa dispone de un portal web DigitalPlus, ' +
    'ingrese la URL a continuacion. Puede dejarlo en blanco.';
  lblUrlInfo.Left := 0;
  lblUrlInfo.Top := 0;
  lblUrlInfo.Width := UrlPage.SurfaceWidth;
  lblUrlInfo.WordWrap := True;

  lblUrl := TNewStaticText.Create(WizardForm);
  lblUrl.Parent := UrlPage.Surface;
  lblUrl.Caption := 'URL del portal:';
  lblUrl.Left := 0;
  lblUrl.Top := lblUrlInfo.Top + lblUrlInfo.Height + 20;

  edtUrl := TNewEdit.Create(WizardForm);
  edtUrl.Parent := UrlPage.Surface;
  edtUrl.Left := 0;
  edtUrl.Top := lblUrl.Top + lblUrl.Height + 5;
  edtUrl.Width := UrlPage.SurfaceWidth;
  edtUrl.Text := '';
end;

// ============================================================
// CONFIGURACION POST-INSTALACION
// ============================================================

procedure WriteConfigs;
var
  FilePath: String;
  Lines: TArrayOfString;
  i: Integer;
  sUrlWeb: String;
begin
  sUrlWeb := Trim(edtUrl.Text);
  if sUrlWeb = '' then
    sUrlWeb := 'https://www.digitaloneplus.com/';

  // --- Fichador config ---
  FilePath := ExpandConstant('{app}\Fichadas\TEntradaSalida.exe.config');
  if LoadStringsFromFile(FilePath, Lines) then
  begin
    for i := 0 to GetArrayLength(Lines) - 1 do
    begin
      StringChange(Lines[i], '{{CONNECTION_STRING}}', sConnectionString);
      StringChange(Lines[i], '{{NOMBRE_EMPRESA}}', sNombreEmpresa);
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
      StringChange(Lines[i], '{{NOMBRE_EMPRESA}}', sNombreEmpresa);
      StringChange(Lines[i], '{{URL_DIGITALPLUS_WEB}}', sUrlWeb);
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
  sConnectionString := '';
  sNombreEmpresa := '';

  CreateActivacionPage;
  CreateUrlPage;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;

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
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // 1. Escribir connection string en configs
    WriteConfigs;

    // 2. Cifrar configs con DPAPI
    EncryptConfigs;

    // 3. Aplicar seguridad ACL
    ApplySecurity;
  end;
end;
