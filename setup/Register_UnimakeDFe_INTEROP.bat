@ECHO OFF
:: ignorar se é windows 32 ou 64
:: IF DEFINED ProgramFiles(x86) GOTO 64Bits
SET dllPath="D:\Projetos\uninfe\trunk\fontes\Unimake.DFe\Compilacao\INTEROP_Release\netstandard2.0\Unimake.Business.DFe.dll"
SET dllSecurityPath="D:\Projetos\uninfe\testes\TesteDLL_Unimake.Business.DFe\bin\INTEROP_Release\Unimake.Security.Platform.dll"
ECHO 32 Bits Windows
C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe %dllPath% /codebase /tlb
C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe %dllSecurityPath% /codebase /tlb
::GOTO exitScript

:64Bits
ECHO 64 Bits Windows
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe %dllPath% /codebase /tlb
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe %dllSecurityPath% /codebase /tlb

:exitScript
PAUSE
EXIT 0