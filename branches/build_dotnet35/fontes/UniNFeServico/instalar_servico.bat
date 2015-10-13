c:\windows\microsoft.net\framework\v2.0.50727\installutil /i UniNFeServico.exe
pause
net start UniNFeServico
pause

net stop UniNFeServico
pause
c:\windows\microsoft.net\framework\v2.0.50727\installutil /u UniNFeServico.exe
pause
