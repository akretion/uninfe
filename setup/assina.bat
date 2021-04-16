@Echo Off

   if "%1"=="setup"  goto setup
   if "%1"=="SETUP"  goto setup
   if "%1"=="uninfe" goto uninfe
   if "%1"=="UNINFE" goto uninfe

   goto nao_existe

:uninfe
   cd ..\fontes\uninfe\bin\release\
   "C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" sign /f "d:\projetos\unimake-codesign-EXE-DLL.pfx" /p uni-123456! /t http://timestamp.comodoca.com/authenticode MetroFramework.dll MetroFramework.Design.dll MetroFramework.Fonts.dll NFe.Certificado.dll NFe.Components.dll NFe.Components.Info.dll NFe.Components.Wsdl.dll NFe.ConvertTxt.dll NFe.Service.dll NFe.Settings.dll NFe.Threadings.dll NFe.UI.dll NFe.Validate.dll NFe.SAT.dll itextsharp.dll Unimake.SAT.dll Newtonsoft.Json.dll Unimake.Business.DFe.dll System.Security.Cryptography.Xml.dll BouncyCastle.Crypto.dll uninfe.exe uninfeservico.exe
   cd ..\..\..\..\setup

   cd ..\fontes\uninfe\bin\x86\Release46_x86\
   "C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" sign /f "d:\projetos\unimake-codesign-EXE-DLL.pfx" /p uni-123456! /t http://timestamp.comodoca.com/authenticode MetroFramework.dll MetroFramework.Design.dll MetroFramework.Fonts.dll NFe.Certificado.dll NFe.Components.dll NFe.Components.Info.dll NFe.Components.Wsdl.dll NFe.ConvertTxt.dll NFe.Service.dll NFe.Settings.dll NFe.Threadings.dll NFe.UI.dll NFe.Validate.dll NFe.SAT.dll itextsharp.dll Unimake.SAT.dll Newtonsoft.Json.dll Unimake.Business.DFe.dll System.Security.Cryptography.Xml.dll BouncyCastle.Crypto.dll uninfe.exe uninfeservico.exe
   cd ..\..\..\..\..\setup

   Goto Fim

:nao_existe
   echo %1.exe não foi localizado
   Goto Fim

:setup
   "C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" sign /f "d:\projetos\unimake-codesign-EXE-DLL.pfx" /p uni-123456! /t http://timestamp.comodoca.com/authenticode \projetos\instaladores\iuninfe5.exe \projetos\instaladores\iuninfe5_fw46_x86.exe
   Goto Fim

:fim
