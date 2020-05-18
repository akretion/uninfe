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
;Registrar
Source: "Register_UnimakeDFe.bat"; DestDir: "{app}"; Flags: ignoreversion
;Arquivos da DLL principal
Source: "..\fontes\Unimake.DFe\Compilacao\netstandard2.0\Unimake.Business.DFe.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\fontes\Unimake.DFe\Compilacao\netstandard2.0\Unimake.Business.DFe.tlb"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\fontes\Unimake.DFe\Compilacao\netstandard2.0\Unimake.Security.Platform.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\fontes\Unimake.DFe\Compilacao\netstandard2.0\Unimake.Security.Platform.tlb"; DestDir: "{app}"; Flags: ignoreversion
;DLLs auxiliares
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\System.Security.AccessControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\System.Security.Cryptography.Xml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\System.Security.Permissions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\System.Security.Principal.Windows.dll"; DestDir: "{app}"; Flags: ignoreversion
;Arquivos auxiliares (Schemas e Configs)
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\Config.xml"; DestDir: "{app}\Servicos\Config"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\CTe\AN.xml"; DestDir: "{app}\Servicos\Config\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\CTe\Config.xml"; DestDir: "{app}\Servicos\Config\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\CTe\PR.xml"; DestDir: "{app}\Servicos\Config\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\AC.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\AL.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\AM.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\AP.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\BA.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\CE.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\Config.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\DF.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\ES.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\GO.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\MG.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\MS.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\MT.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\PB.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\PE.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\PI.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\PR.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\RJ.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\RN.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\RO.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\RR.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\RS.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\SC.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\SE.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\SP.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\SVRS.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\MDFe\TO.xml"; DestDir: "{app}\Servicos\Config\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\AC.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\AL.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\AM.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\AP.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\BA.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\CE.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\Config.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\DF.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\ES.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\GO.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\MG.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\MS.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\MT.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\PB.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\PE.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\PI.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\PR.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\RJ.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\RN.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\RO.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\RR.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\RS.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\SE.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\SP.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\SVRS.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFCe\TO.xml"; DestDir: "{app}\Servicos\Config\NFCe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\AC.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\AL.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\AM.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\AN.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\AP.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\BA.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\CE.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\Config.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\DF.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\ES.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\GO.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\MG.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\MS.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\MT.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\PB.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\PE.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\PI.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\PR.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\RJ.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\RN.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\RO.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\RR.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\RS.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\SC.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\SE.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\SP.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\SVAN.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\SVCAN.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\SVCRS.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\SVRS.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Servicos\Config\NFe\TO.xml"; DestDir: "{app}\Servicos\Config\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cancCTeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\consCad_v2.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\consReciCTeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\consReciCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\consSitCTeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\consSitCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\consStatServCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\consStatServTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteModalAereo_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteModalAquaviario_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteModalDutoviario_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteModalFerroviario_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteModalRodoviarioOS_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteModalRodoviario_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteMultiModal_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteOS_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cteTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\cte_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\distDFeInt_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\enviCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\evCancCECTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\evCancCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\evCCeCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\evCECTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\eventoCTeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\eventoCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\evEPECCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\evGTV_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\evPrestDesacordo_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\evRegMultimodal_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\inutCTeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\inutCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\leiauteConsultaCadastro_v2.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\procCTeOS_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\procCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\procEventoCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\procInutCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retConsCad_v2.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retConsReciCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retConsSitCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retConsStatServCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retCTeOS_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retDistDFeInt_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retEnviCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retEventoCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\retInutCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\tiposBasico_v1.03.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\tiposDistDFe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\tiposGeralCTe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\CTe\xmldsig-core-schema_v1.01.xsd"; DestDir: "{app}\Xml\Schemas\CTe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\consMDFeNaoEncTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\consMDFeNaoEnc_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\consReciMDFeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\consReciMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\consSitMDFeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\consSitMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\consStatServMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\consStatServTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\distMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\enviMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\evCancMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\evEncMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\eventoMDFeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\eventoMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\evIncCondutorMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\evInclusaoDFeMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\evPagtoOperMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\leiauteDistMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\mdfeConsultaDFeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\mdfeConsultaDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\mdfeModalAereo_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\mdfeModalAquaviario_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\mdfeModalFerroviario_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\mdfeModalRodoviario_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\mdfeTiposBasico_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\mdfe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\procEventoMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\procMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retConsMDFeNaoEnc_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retConsReciMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retConsSitMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retConsStatServMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retDistMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retEnviMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retEventoMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retMDFeConsultaDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\retMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\tiposGeralMDFe_v3.00.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\MDFe\xmldsig-core-schema_v1.01.xsd"; DestDir: "{app}\Xml\Schemas\MDFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\CCe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\confRecebto_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\consCad_v2.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\consReciNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\consSitNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\consStatServ_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\distDFeInt_v1.01.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e110110_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e110111_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e110112_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e111500_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e111501_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e111502_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e111503_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e210200_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e210210_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e210220_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e210240_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e411500_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e411501_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e411502_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\e411503_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\envCCe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\envConfRecebto_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\envEventoCancNFe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\envEventoCancSubst_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\enviNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\envRemIndus_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\eventoCancNFe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\eventoCancSubst_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\eventoRemIndus_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\inutNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteCCe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteConfRecebto_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteConsSitNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteConsStatServ_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteConsultaCadastro_v2.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteEventoCancNFe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteEventoCancSubst_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteInutNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\leiauteRemIndus_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\nfe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\procCCeNFe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\procConfRecebtoNFe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\procEventoCancNFe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\procEventoCancSubst_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\procEventoNFe_v99.99.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\procInutNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\procNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\procRemIndus_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\resEvento_v1.01.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\resNFe_v1.01.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retConsCad_v2.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retConsReciNFe_v2.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retConsReciNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retConsSitNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retConsStatServ_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retDistDFeInt_v1.01.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retEnvCCe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retEnvConfRecebto_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retEnvEventoCancNFe_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retEnvEventoCancSubst_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retEnviNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retEnvRemIndus_v1.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\retInutNFe_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\tiposBasico_v1.03.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\tiposBasico_v4.00.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\tiposDistDFe_v1.01.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion
Source: "..\..\testes\TesteDLL_Unimake.Business.DFe\bin\Release\Xml\Schemas\NFe\xmldsig-core-schema_v1.01.xsd"; DestDir: "{app}\Xml\Schemas\NFe"; Flags: ignoreversion

[Run]
Filename: "{app}\Register_UnimakeDFe.bat"; Flags: runhidden; StatusMsg: "Registrando dll"

[Dirs]
Name: "{app}\Xml\NFe"
Name: "{app}\Xml\NFe\Schemas"
Name: "{app}\Xml\Schemas"
Name: "{app}\Xml\Schemas\NFe"
Name: "{app}\Servicos"
Name: "{app}\Servicos\Config"
Name: "{app}\Servicos\Config\NFCe"
Name: "{app}\Servicos\Config\NFe"
Name: "{app}\Servicos\NFe"
Name: "{app}\Servicos\NFe\Config"
Name: "{app}\Servicos\Config"
Name: "{app}\Servicos\Config\CTe"
Name: "{app}\Servicos\Config\MDFe"
Name: "{app}\Servicos\Config\NFCe"
Name: "{app}\Servicos\Config\NFe"
Name: "{app}\Servicos\NFe"
Name: "{app}\Servicos\NFe\Config"
Name: "{app}\Xml"
Name: "{app}\Xml\NFe"
Name: "{app}\Xml\NFe\Schemas"
Name: "{app}\Xml\Schemas"
Name: "{app}\Xml\Schemas\CTe"
Name: "{app}\Xml\Schemas\MDFe"
Name: "{app}\Xml\Schemas\NFe"

[UninstallDelete]
Type: filesandordirs; Name: "{app}\Unimake.DFe"

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
