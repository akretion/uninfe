Attribute VB_Name = "Config"

Public Function InicializarConfiguracao()
Static certificado
Static flagCertificado As Boolean

If flagCertificado = False Then
    Set certificado = SelecionarCertificado.SelecionarCertificado()
    flagCertificado = True
End If

Set InicializarConfiguracao = CreateObject("Unimake.Business.DFe.Servicos.Configuracao")
InicializarConfiguracao.TipoDFe = 0
Set InicializarConfiguracao.CertificadoDigital = certificado
End Function
