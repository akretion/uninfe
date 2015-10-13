@Echo Off   

   if "%1"=="setup"  goto setup
   if "%1"=="SETUP"  goto setup
   if "%1"=="uninfe" goto uninfe
   if "%1"=="UNINFE" goto uninfe

   goto nao_existe

:uninfe
   call assina_udf.bat ..\fontes\uninfe\bin\release\MetroFramework.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Certificado.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Components.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Components.Info.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Components.Wsdl.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.ConvertTxt.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Interface.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Service.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Settings.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Threadings.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.UI.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\NFe.Validate.dll
   call assina_udf.bat ..\fontes\uninfe\bin\release\uninfe.exe
  
   Goto Fim

:nao_existe
   echo %1.exe não foi localizado
   Goto Fim

:setup
   call assina_udf.bat \projetos\instaladores\iuninfe.exe
   
   Goto Fim

:fim
   