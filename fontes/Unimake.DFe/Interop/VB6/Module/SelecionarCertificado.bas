Attribute VB_Name = "SelecionarCertificado"
Public Function SelecionarCertificado()
Dim selCertificado
Set selCertificado = CreateObject("Unimake.Security.Platform.CertificadoDigital")
Set SelecionarCertificado = selCertificado.Selecionar()
End Function


