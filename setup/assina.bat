@Echo Off

   if "%1"=="setup"  goto setup
   if "%1"=="SETUP"  goto setup
   if "%1"=="uninfe" goto uninfe
   if "%1"=="UNINFE" goto uninfe

   goto nao_existe

:uninfe
   cd ..\fontes\uninfe\bin\release\
   "C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" sign /a /t http://timestamp.verisign.com/scripts/timstamp.dll MetroFramework.dll MetroFramework.Fonts.dll MetroFramework.Design.dll NFe.Certificado.dll NFe.Components.dll NFe.Components.Info.dll NFe.Components.Wsdl.dll NFe.ConvertTxt.dll NFe.Service.dll NFe.Settings.dll NFe.Threadings.dll NFe.UI.dll NFe.Validate.dll NFe.SAT.dll Unimake.SAT.dll Unimake.Business.DFe.dll uninfe.exe uninfeservico.exe
   cd ..\..\..\..\setup

   cd ..\fontes\uninfe\bin\Release35_AnyCPU\
   "C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" sign /a /t http://timestamp.verisign.com/scripts/timstamp.dll MetroFramework.dll MetroFramework.Fonts.dll MetroFramework.Design.dll NFe.Certificado.dll NFe.Components.dll NFe.Components.Info.dll NFe.Components.Wsdl.dll NFe.ConvertTxt.dll NFe.Service.dll NFe.Settings.dll NFe.Threadings.dll NFe.UI.dll NFe.Validate.dll uninfe.exe uninfeservico.exe
   cd ..\..\..\..\setup

   cd ..\fontes\uninfe\bin\x86\Release46_x86\
   "C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" sign /a /t http://timestamp.verisign.com/scripts/timstamp.dll MetroFramework.dll MetroFramework.Fonts.dll MetroFramework.Design.dll NFe.Certificado.dll NFe.Components.dll NFe.Components.Info.dll NFe.Components.Wsdl.dll NFe.ConvertTxt.dll NFe.Service.dll NFe.Settings.dll NFe.Threadings.dll NFe.UI.dll NFe.Validate.dll Unimake.SAT.dll uninfe.exe NFe.SAT.dll Unimake.Business.DFe.dll uninfeservico.exe
   cd ..\..\..\..\..\setup

   Goto Fim

:nao_existe
   echo %1.exe não foi localizado
   Goto Fim

:setup
   "C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" sign /a /t http://timestamp.verisign.com/scripts/timstamp.dll \projetos\instaladores\iuninfe5.exe \projetos\instaladores\iuninfe5_fw35.exe \projetos\instaladores\iuninfe5_fw46_x86.exe
   Goto Fim

:fim
