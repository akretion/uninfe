@ECHO OFF
:: ignorar se é windows 32 ou 64
:: IF DEFINED ProgramFiles(x86) GOTO 64Bits

ECHO 32 Bits Windows
C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe Unimake.Business.DFe.dll /codebase /tlb
C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe Unimake.Security.Platform.dll /codebase /tlb
::GOTO exitScript

:64Bits
ECHO 64 Bits Windows
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe Unimake.Business.DFe.dll /codebase /tlb
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe Unimake.Security.Platform.dll /codebase /tlb

:exitScript
EXIT 0