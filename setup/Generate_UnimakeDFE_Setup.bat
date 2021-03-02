ECHO OFF
CHCP 65001
::Variáveis
SET filesDir=Z:\uninfe\trunk\fontes\Unimake.DFe\Compilacao\INTEROP_Release\
SET istool="C:\Program Files (x86)\Inno Script Studio\ISStudio.exe"

::Prepara
DEL /S %cd%\err

CLS

@ECHO Compilando Unimake.DFe

CALL Z:\uninfe\trunk\fontes\Unimake.DFe\Unimake.DFe.sln


@ECHO Limpando diretório de release

::Apaga os arquivos desnecessários
DEL /S %filesDir%\*.xml
DEL /S %filesDir%\*.pdb
DEL /S %filesDir%\*.json
DEL /S %filesDir%\App.config

::Apaga os arquivos desnecessários
DEL /S %filesDir%\net462\*.xml
DEL /S %filesDir%\net462\*.pdb
DEL /S %filesDir%\net462\*.json
DEL /S %filesDir%\net462\App.config

::Apaga os arquivos desnecessários
DEL /S %filesDir%\net472\*.xml
DEL /S %filesDir%\net472\*.pdb
DEL /S %filesDir%\net472\*.json
DEL /S %filesDir%\net472\App.config

::Ações
@ECHO Assinando executáveis e dlls
FORFILES /p %filesDir% /s /m unimake.* /c "cmd /c %CD%\sign.bat @path %cd%"
IF EXIST err GOTO err


@ECHO Compilando script
:: Aqui abrimos o diretório apenas para atualizar o script, poderia ser automatizado, mas ... 
EXPLORER %filesDir%
CALL %istool% Unimake.DFe.iss
EXPLORER %cd%\output

@ECHO Assinando o instalador
CALL sign %cd%\output\Install_Unimake.DFe.exe %cd%
IF NOT EXIST err GOTO ok

:err
@ECHO Error: %ERRORLEVEL%
PAUSE

:ok
exit /B 0