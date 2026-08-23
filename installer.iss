; =====================================================================
;  GlassHub EventHorizon - Script do Instalador Oficial (Inno Setup)
;  Identidade Visual Cosmic Glassmorphism, Suporte x64, Desktop & Explorer
; =====================================================================

#define MyAppName "GlassHub Event Horizon"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "GlassHub"
#define MyAppURL "https://github.com/matheustheus27/GlassHubEventHorizon"
#define MyAppExeName "GlassHub.EventHorizon.GUI.exe"
#define MyCliExeName "evh.exe"

[Setup]
; Identificador Único do Aplicativo (GUID)
AppId={{8F41A99F-4D2A-4D78-9B21-87C246E20B99}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}/issues
AppUpdatesURL={#MyAppURL}/releases

; Diretórios de Instalação
DefaultDirName={autopf}\GlassHub\EventHorizon
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=LICENSE
OutputDir=setup_output
OutputBaseFilename=GlassHubEventHorizon_Setup_v{#MyAppVersion}

; Ícone e Estilo Visual do Instalador
SetupIconFile=assets\app.ico
UninstallDisplayIcon={app}\app.ico
WizardStyle=modern
WizardSmallImageFile=assets\logo.png

; Arquitetura e Compressão
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
Compression=lzma2/ultra64
SolidCompression=yes
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "explorercontext"; Description: "Integrar ao menu de contexto do Windows Explorer (Clique com botão direito em arquivos/pastas)"; GroupDescription: "Integração do Sistema:"
Name: "addtopath"; Description: "Adicionar utilitário de linha de comando (evh) ao PATH do usuário"; GroupDescription: "Desenvolvedores / CLI:"; Flags: unchecked

[Files]
; Binários publicados pela pipeline publish.ps1
Source: "publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\app.ico"
Name: "{group}\{#MyAppName} CLI (Terminal)"; Filename: "cmd.exe"; Parameters: "/k ""{app}\{#MyCliExeName}"" --help"; IconFilename: "{app}\app.ico"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\app.ico"; Tasks: desktopicon

[Registry]
; Integração com o Menu de Contexto do Windows Explorer para Arquivos
Root: HKA; Subkey: "Software\Classes\*\shell\GlassHubEventHorizon"; ValueType: string; ValueName: ""; ValueData: "Abrir com GlassHub EventHorizon"; Flags: uninsdeletekey; Tasks: explorercontext
Root: HKA; Subkey: "Software\Classes\*\shell\GlassHubEventHorizon"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\app.ico"""; Flags: uninsdeletekey; Tasks: explorercontext
Root: HKA; Subkey: "Software\Classes\*\shell\GlassHubEventHorizon\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Flags: uninsdeletekey; Tasks: explorercontext

; Integração com o Menu de Contexto do Windows Explorer para Diretórios/Pastas
Root: HKA; Subkey: "Software\Classes\Directory\shell\GlassHubEventHorizon"; ValueType: string; ValueName: ""; ValueData: "Compactar com GlassHub EventHorizon"; Flags: uninsdeletekey; Tasks: explorercontext
Root: HKA; Subkey: "Software\Classes\Directory\shell\GlassHubEventHorizon"; ValueType: string; ValueName: "Icon"; ValueData: """{app}\app.ico"""; Flags: uninsdeletekey; Tasks: explorercontext
Root: HKA; Subkey: "Software\Classes\Directory\shell\GlassHubEventHorizon\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Flags: uninsdeletekey; Tasks: explorercontext

; Registro no PATH do usuário (opcional)
Root: HKCU; Subkey: "Environment"; ValueType: expandsz; ValueName: "Path"; ValueData: "{olddata};{app}"; Check: NeedsAddPath; Tasks: addtopath

[Code]
// Helper em Pascal Script para verificar e adicionar ao PATH sem duplicar entradas
function NeedsAddPath(): Boolean;
var
  OrigPath: string;
begin
  if not RegQueryStringValue(HKEY_CURRENT_USER, 'Environment', 'Path', OrigPath) then
  begin
    Result := True;
    exit;
  end;
  Result := Pos(';' + ExpandConstant('{app}') + ';', ';' + OrigPath + ';') = 0;
end;

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
