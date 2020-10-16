ECHO Assinando arquivo %1
SET signtool="C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe"
SET signtoolParams=sign /n "UNIMAKE" /t http://timestamp.verisign.com/scripts/timstamp.dll
CALL %signtool% %signtoolParams% %1