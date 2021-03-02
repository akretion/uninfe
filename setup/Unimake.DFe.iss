#include ReadReg(HKEY_LOCAL_MACHINE,'Software\Sherlock Software\InnoTools\Downloader','ScriptPath','')

#define MyAppName "Unimake.DFe"
#define MyAppVersion "1.0"
#define MyAppPublisher "Unimake Software"
#define MyAppURL "http://www.uninfe.com.br"
#define MyAppExeName "Unimake.Business.DFe.dll"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{0B4E1A1E-0B3D-4794-88A8-643F6F23D607}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
VersionInfoCompany=Unimake Software
DefaultDirName={pf}\Unimake\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
; LicenseFile="..\..\fontes\Package\licenças\licenca.txt"
OutputDir=output
OutputBaseFilename=Install_Unimake.DFe
Compression=lzma
SolidCompression=yes
AppCopyright=Unimake Software
AppMutex=Unimake.DFe_MUTEX
AllowUNCPath=False
VersionInfoVersion=1.0
VersionInfoCopyright=2020 - Todos os direitos reservados
VersionInfoProductName=Unimake.DFe
VersionInfoProductVersion=1.0
VersionInfoProductTextVersion=1.0 Beta
UsePreviousAppDir=False
PrivilegesRequired=admin

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Icons]

[ThirdParty]
UseRelativePaths=True

[LangOptions]

[Files]
;Exemplo em VB6
Source: "..\fontes\Unimake.DFe\Interop\VB6\VB6.rar"; DestDir: "{app}\Exemplo_VB6"; Flags: ignoreversion; Tasks: exemplo_vb6
;Registrar
Source: "Register_UnimakeDFe.bat"; DestDir: "{app}"; Flags: ignoreversion
;.netstandard
Source: "..\fontes\Unimake.DFe\Compilacao\INTEROP_Release\netstandard2.0\Unimake.Business.DFe.dll"; DestDir: "{app}"; Flags: ignoreversion
;DLLs net472
Source: "..\fontes\Unimake.DFe\Compilacao\INTEROP_Release\net472\System.Security.Permissions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\fontes\Unimake.DFe\Compilacao\INTEROP_Release\net472\System.Security.Principal.Windows.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\fontes\Unimake.DFe\Compilacao\INTEROP_Release\net472\Unimake.Business.DFe.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\fontes\Unimake.DFe\Compilacao\INTEROP_Release\net472\System.Security.AccessControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\fontes\Unimake.DFe\Compilacao\INTEROP_Release\net472\System.Security.Cryptography.Xml.dll"; DestDir: "{app}"; Flags: ignoreversion

[Run]
Filename: "{app}\Register_UnimakeDFe.bat"; Flags: runhidden; StatusMsg: "Registrando dll"

[UninstallDelete]
Type: filesandordirs; Name: "{app}\Unimake.DFe"

[Tasks]
Name: "exemplo_vb6"; Description: "Instalar exemplo em VB6"

[Code]
//incialização do setup. É sempre chamada pelo Inno ao iniciar o setup
procedure InitializeWizard();
var
    filename  : string;
    regresult : cardinal;
begin
    // verifica se o framework 4.7.2 sp1 está instalado.
    // mais detalhes para outros frameworks: https://msdn.microsoft.com/pt-br/library/Hh925568(v=VS.110).aspx
    RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\full', 'Release', regresult);
    //< 461808 
    if regresult < 461808 then begin
      // definir o caminho do arquivo
      filename := expandconstant('{tmp}\fx472.exe');

      // não está instalado. Exibir a mensagem para o usuário se deseja instalar o fw
      if MsgBox('Para continuar a instalação é necessário fazer o download do Framework 4.7.2. Deseja continuar?', mbInformation, mb_YesNo) = idYes then begin
          //iniciar o itd
          itd_init;

          //adiciona um arquivo na fila de downloads. (pode se adicionar quantos forem necessários)
          itd_addfile('http://unimake2.com.br/NDP472-KB4054530-x86-x64-AllOS-ENU.exe', filename);
          itd_addmirror('http://74.222.1.252/download/NDP472-KB4054530-x86-x64-AllOS-ENU.exe', filename)

          //aqui dizemos ao itd que é para fazer o download após o inno exibir a tela de preparação do setup
          itd_downloadafter(wpReady);
        end else begin
          // o usuário optou por não fazer o download do fw, então avisamos de onde ele pode baixar
          MsgBox('O link para download manual do framework é http://go.microsoft.com/fwlink/?linkid=863265', mbInformation, mb_Ok);
      end
    end
end;

//Este método é chamado pelo Inno ao clicar em próximo. Neste momento a interface já está criada
procedure CurStepChanged(CurStep: TSetupStep);
var
    filename  : string;
    ErrorCode: Integer;
begin

filename := expandconstant('{tmp}\fx472.exe');

if CurStep = ssInstall then begin
    
    // este passo só irá acontecer após o download do arquivo.
    // para evitar erros, validamos se o arquivo foi baixado. Se não foi, continua com o setup.
    if fileExists(filename) then begin
       // foi baixado. Executar o instalador do fw.
       if not ShellExec('', filename,'', '', SW_SHOW, ewWaitUntilTerminated, ErrorCode) then begin
         // Xi! Deu erro.
         if ErrorCode <> 0 then begin
              MsgBox('Erro ao executar o arquivo ' + filename + chr(13) + SysErrorMessage(ErrorCode), mbError, mb_Ok);
         end;
       end
    end;
end;
end;
