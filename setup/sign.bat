ECHO Assinando arquivo %1
SET signtool="C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe"
SET signtoolParams=sign /n "UNIMAKE"
CALL %signtool% %signtoolParams% %1