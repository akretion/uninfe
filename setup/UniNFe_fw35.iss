#include ReadReg(HKEY_LOCAL_MACHINE,'Software\Sherlock Software\InnoTools\Downloader','ScriptPath','')

[Setup]
AppName=UniNFe - Monitor de Documentos Fiscais Eletrônicos
AppVerName=UniNFe 5.0
DefaultDirName={sd}\Unimake\UniNFe
DefaultGroupName=Unimake Softwares
SetupIconFile=C:\clipart\unimake\ICONES\Install.ico
;SetupIconFile=C:\Documents and Settings\Alcala\Desktop\Instalador\install.ico
;UninstallDisplayIcon={app}\MyProg.exe
;OutputDir=userdocs:Inno Setup Examples Output
AppCopyright=Unimake Softwares
InfoBeforeFile=..\doc\usuario\readme_fw35.txt
LicenseFile=..\doc\usuario\licenca.txt
AppPublisherURL=www.uninfe.com.br
AppSupportURL=www.uninfe.com.br
AppUpdatesURL=www.uninfe.com.br
AppVersion=5.0
AppSupportPhone=(044) 3141-4900
UninstallDisplayIcon={app}\uninfe.exe
UninstallDisplayName=UniNFe - Monitor DF-e
AppPublisher=Unimake Softwares
DisableProgramGroupPage=true
DisableReadyPage=false
DisableFinishedPage=true
WizardImageFile=C:\Program Files (x86)\Inno Setup 5\WizModernImage-IS.bmp
WizardSmallImageFile=C:\Program Files (x86)\Inno Setup 5\WizModernSmallImage-IS.bmp
OutputBaseFilename=iuninfe5_fw35
VersionInfoVersion=5.0
VersionInfoCompany=Unimake Softwares
VersionInfoDescription=UniNFe - Monitor DF-e
VersionInfoCopyright=Unimake Softwares
VersionInfoProductName=UniNFe
VersionInfoProductVersion=5.0
OutputDir=\projetos\instaladores
DisableDirPage=false

[Languages]
Name: brazilianportuguese; MessagesFile: compiler:Languages\BrazilianPortuguese.isl

[Files]
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\MetroFramework.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.Certificado.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.Components.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.Components.Info.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.Components.Wsdl.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.ConvertTxt.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.Service.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.Settings.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.Threadings.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.UI.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\NFe.Validate.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\uninfe.exe; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\Release35_AnyCPU\uninfeservico.exe; DestDir: {app}; Flags: ignoreversion
Source: \projetos\dv\trunk\fontes\includes\sefaz.inc; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\UniNfeSobre.xml; DestDir: {app}; Flags: ignoreversion
Source: ..\doc\usuario\uninfe.url; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\NFe.Components.Wsdl\NFe\WSDL\*.*; DestDir: {app}\nfe\wsdl; Flags: ignoreversion recursesubdirs
Source: ..\fontes\NFe.Components.Wsdl\NFe\schemas\*.*; DestDir: {app}\nfe\schemas; Flags: ignoreversion recursesubdirs
Source: ..\fontes\NFe.Components.Wsdl\NFse\WSDL\*.*; DestDir: {app}\nfse\wsdl; Flags: ignoreversion recursesubdirs
Source: ..\fontes\NFe.Components.Wsdl\NFse\schemas\*.*; DestDir: {app}\nfse\schemas; Flags: ignoreversion recursesubdirs

[Icons]
Name: {group}\UniNFe - Monitor DF-e; Filename: {app}\uninfe.exe; WorkingDir: {app}; IconFilename: {app}\uninfe.exe; IconIndex: 0; Languages: ; Comment: Aplicativo responsável por monitorar os arquivos de documentos fiscais eletrônicos (NF-e, NFC-e, CT-e, MDF-e, NFS-e, etc.) para assinar, validar e enviar ao SEFAZ.
Name: {group}\Links\www.uninfe.com.br; Filename: {app}\uninfe.url; IconFilename: {app}\uninfe.url; Flags: runmaximized

[UninstallDelete]
Type: files; Name: {app}\uninfe.url

[Run]
Filename: {app}\uninfe.exe; WorkingDir: {app}; Flags: postinstall shellexec; Parameters: /updatewsdl

[LangOptions]
LanguageName=Portuguese
LanguageID=$0416

[Code]
//incialização do setup. É sempre chamada pelo Inno ao iniciar o setup
procedure InitializeWizard();
var
    filename  : string;
    regresult : cardinal;
begin
    // verifica se o framework 3.5 sp1 está instalado.
    // mais detalhes para outros frameworks: http://msdn.microsoft.com/en-us/kb/kbarticle.aspx?id=318785
    RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5', 'SP', regresult);

    //se o resultado for um. Então o SP1 está instalado
    //Este resultado é o valor da chave
    if regresult <> 1 then begin
      // definir o caminho do arquivo
      filename := expandconstant('{tmp}\fx3.5sp1.exe');

      // não está instalado. Exibir a mensagem para o usuário se deseja instalar o fw
      if MsgBox('Para continuar a instalação é necessário fazer o download do Framework 3.5 SP1. Deseja continuar?', mbInformation, mb_YesNo) = idYes then begin
          //iniciar o itd
          itd_init;

          //adiciona um arquivo na fila de downloads. (pode se adicionar quantos forem necessários)
          itd_addfile('http://download.microsoft.com/download/2/0/e/20e90413-712f-438c-988e-fdaa79a8ac3d/dotnetfx35.exe', filename);

          //aqui dizemos ao itd que é para fazer o download após o inno exibir a tela de preparação do setup
          itd_downloadafter(2);
        end else begin
          // o usuário optou por não fazer o download do fw, então avisamos de onde ele pode baixar
          MsgBox('O link para download manual do framework é http://download.microsoft.com/download/2/0/e/20e90413-712f-438c-988e-fdaa79a8ac3d/dotnetfx35.exe', mbInformation, mb_Ok);
      end
    end
end;

//Este método é chamado pelo Inno ao clicar em próximo. Neste momento a interface já está criada
procedure CurStepChanged(CurStep: TSetupStep);
var
    filename  : string;
    ErrorCode: Integer;
begin

filename := expandconstant('{tmp}\fx3.5sp1.exe');

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
