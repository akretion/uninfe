@ECHO OFF
 SET solutionPath=%1
 SET msbuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
 SET logPath=%CD%\Log
 %msbuild% %solutionPath% /p:PostBuildEvent= /p:Configuration=Release35_AnyCPU /p:Platform="Any CPU" /p:TargetFrameworkVersion=v3.5 >%logPath%AnyCPU.txt
 CLS
 %msbuild% %solutionPath% /p:PostBuildEvent= /p:Configuration=Release45_x64  /p:Platform="x64" /p:TargetFrameworkVersion=v4.5 >%logPath%x64.txt
 EXIT