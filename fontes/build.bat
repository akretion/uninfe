rem @ECHO OFF
 SET solutionPath=%1
 SET msbuild="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
 SET logPath=%CD%\Log
 %msbuild% %solutionPath% /p:PostBuildEvent= /p:Configuration=Release35_AnyCPU /p:Platform="Any CPU" /p:TargetFrameworkVersion=v3.5 >%logPath%_FW35_AnyCPU.txt
 %msbuild% %solutionPath% /p:PostBuildEvent= /p:Configuration=Release46_x86    /p:Platform="x86" /p:TargetFrameworkVersion=v4.6.2 >%logPath%_FW46_x86.txt
 %msbuild% %solutionPath% /p:PostBuildEvent= /p:Configuration=Release46_x64    /p:Platform="x64" /p:TargetFrameworkVersion=v4.6.2 >%logPath%_FW46_x64.txt
 EXIT