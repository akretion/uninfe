#include ReadReg(HKEY_LOCAL_MACHINE,'Software\Sherlock Software\InnoTools\Downloader','ScriptPath','')

[Setup]
AppName=UniNFe - Monitor de Documentos Fiscais Eletrônicos
AppVerName=UniNFe 5.1
DefaultDirName={sd}\Unimake\UniNFe
DefaultGroupName=Unimake Softwares
SetupIconFile=d:\clipart\unimake\ICONES\Install.ico
;SetupIconFile=C:\Documents and Settings\Alcala\Desktop\Instalador\install.ico
;UninstallDisplayIcon={app}\MyProg.exe
;OutputDir=userdocs:Inno Setup Examples Output
AppCopyright=Unimake Softwares
InfoBeforeFile=..\doc\usuario\readme.txt
LicenseFile=..\doc\usuario\licenca.txt
AppPublisherURL=www.uninfe.com.br
AppSupportURL=www.uninfe.com.br
AppUpdatesURL=www.uninfe.com.br
AppVersion=5.1
AppSupportPhone=(044) 3141-4900
UninstallDisplayIcon={app}\uninfe.exe
UninstallDisplayName=UniNFe - Monitor DF-e
AppPublisher=Unimake Softwares
DisableProgramGroupPage=true
DisableReadyPage=false
DisableFinishedPage=true
WizardImageFile=d:\Program Files (x86)\Inno Setup 5\WizModernImage-IS.bmp
WizardSmallImageFile=d:\Program Files (x86)\Inno Setup 5\WizModernSmallImage-IS.bmp
OutputBaseFilename=iuninfe5_fw46_x86
VersionInfoVersion=5.1
VersionInfoCompany=Unimake Softwares
VersionInfoDescription=UniNFe - Monitor DF-e
VersionInfoCopyright=Unimake Softwares
VersionInfoProductName=UniNFe
VersionInfoProductVersion=5.1
OutputDir=\projetos\instaladores
DisableDirPage=false

[Languages]
Name: brazilianportuguese; MessagesFile: compiler:Languages\BrazilianPortuguese.isl

[Files]
Source: ..\fontes\uninfe\bin\x86\Release46_x86\BouncyCastle.Crypto.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\itextsharp.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\MetroFramework.Design.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\MetroFramework.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\MetroFramework.Fonts.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\Microsoft.Win32.Primitives.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\netstandard.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\Newtonsoft.Json.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Certificado.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Components.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Components.Info.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Components.Wsdl.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Components.XmlSerializers.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.ConvertTxt.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.SAT.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Service.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Settings.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Threadings.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.UI.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\NFe.Validate.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.AppContext.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Collections.Concurrent.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Collections.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Collections.NonGeneric.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Collections.Specialized.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.ComponentModel.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.ComponentModel.EventBasedAsync.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.ComponentModel.Primitives.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.ComponentModel.TypeConverter.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Console.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Data.Common.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.Contracts.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.Debug.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.FileVersionInfo.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.Process.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.StackTrace.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.TextWriterTraceListener.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.Tools.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.TraceSource.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Diagnostics.Tracing.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Drawing.Primitives.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Dynamic.Runtime.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Globalization.Calendars.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Globalization.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Globalization.Extensions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.Compression.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.Compression.ZipFile.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.FileSystem.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.FileSystem.DriveInfo.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.FileSystem.Primitives.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.FileSystem.Watcher.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.IsolatedStorage.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.MemoryMappedFiles.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.Pipes.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.IO.UnmanagedMemoryStream.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Linq.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Linq.Expressions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Linq.Parallel.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Linq.Queryable.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.Http.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.NameResolution.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.NetworkInformation.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.Ping.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.Primitives.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.Requests.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.Security.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.Sockets.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.WebHeaderCollection.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.WebSockets.Client.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Net.WebSockets.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.ObjectModel.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Reflection.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Reflection.Extensions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Reflection.Primitives.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Resources.Reader.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Resources.ResourceManager.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Resources.Writer.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.CompilerServices.VisualC.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.Extensions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.Handles.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.InteropServices.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.InteropServices.RuntimeInformation.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.Numerics.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.Serialization.Formatters.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.Serialization.Json.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.Serialization.Primitives.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Runtime.Serialization.Xml.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.AccessControl.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Claims.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Cryptography.Algorithms.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Cryptography.Csp.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Cryptography.Encoding.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Cryptography.Primitives.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Cryptography.X509Certificates.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Cryptography.Xml.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Permissions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Principal.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.Principal.Windows.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Security.SecureString.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Text.Encoding.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Text.Encoding.Extensions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Text.RegularExpressions.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Threading.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Threading.Overlapped.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Threading.Tasks.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Threading.Tasks.Parallel.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Threading.Thread.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Threading.ThreadPool.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Threading.Timer.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.ValueTuple.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Xml.ReaderWriter.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Xml.XDocument.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Xml.XmlDocument.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Xml.XmlSerializer.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Xml.XPath.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\System.Xml.XPath.XDocument.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\Unimake.Business.DFe.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\Unimake.SAT.dll; DestDir: {app}; Flags: ignoreversion

Source: ..\fontes\uninfe\bin\x86\Release46_x86\uninfe.exe; DestDir: {app}; Flags: ignoreversion
Source: ..\fontes\uninfe\bin\x86\Release46_x86\uninfeservico.exe; DestDir: {app}; Flags: ignoreversion

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
    // verifica se o framework 4.8 está instalado.
    //
    // mais detalhes para outros frameworks: https://msdn.microsoft.com/pt-br/library/Hh925568(v=VS.110).aspx
    //
    // 528040 = No Windows 10 maio 2019 atualização e Windows 10 de novembro de 2019 atualização. 
    // 528049 = Em todos os outros sistemas operacionais Windows (incluindo outros sistemas operacionais Windows 10)
    // 528372 = No Windows 10 pode 2020 atualização e atualização de 10 de outubro de 2020 do Windows
    RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\full', 'Release', regresult);

    if regresult < 528040 then begin
      // definir o caminho do arquivo
      filename := expandconstant('{tmp}\fw48.exe');

      // não está instalado. Exibir a mensagem para o usuário se deseja instalar o fw
      if MsgBox('Para continuar a instalação é necessário fazer o download do Framework 4.8. Deseja continuar?', mbInformation, mb_YesNo) = idYes then begin
          //iniciar o itd
          itd_init;

          //adiciona um arquivo na fila de downloads. (pode se adicionar quantos forem necessários)
          itd_addfile('http://download.visualstudio.microsoft.com/download/pr/9acd2157-dc1e-41fc-9f4d-35d56fc49f6b/406745de80fb60de18220db262021b92/ndp48-x86-x64-allos-enu.exe', filename);

          //aqui dizemos ao itd que é para fazer o download após o inno exibir a tela de preparação do setup
          itd_downloadafter(2);
        end else begin
          // o usuário optou por não fazer o download do fw, então avisamos de onde ele pode baixar
          MsgBox('O link para download manual do framework é https://download.visualstudio.microsoft.com/download/pr/9acd2157-dc1e-41fc-9f4d-35d56fc49f6b/406745de80fb60de18220db262021b92/ndp48-x86-x64-allos-enu.exe', mbInformation, mb_Ok);
      end
    end
end;

//Este método é chamado pelo Inno ao clicar em próximo. Neste momento a interface já está criada
procedure CurStepChanged(CurStep: TSetupStep);
var
    filename  : string;
    ErrorCode: Integer;
begin

filename := expandconstant('{tmp}\fw48.exe');

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
