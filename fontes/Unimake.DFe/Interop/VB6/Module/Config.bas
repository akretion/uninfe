Attribute VB_Name = "Config"
Public Enum TipoDFe
    NFe
    NFCe
    CTe
    MDFe
    NFSe
    SAT
End Enum

Public Function InicializarConfiguracao(ByVal pTipoDFe As TipoDFe, Optional ByVal pCUF = 0)
Static certificado
Static flagCertificado As Boolean

If flagCertificado = False Then
    Set certificado = SelecionarCertificado.SelecionarCertificado()
    flagCertificado = True
End If

Set InicializarConfiguracao = CreateObject("Unimake.Business.DFe.Servicos.Configuracao")
InicializarConfiguracao.TipoDFe = CInt(pTipoDFe)

If pCUF > 0 Then InicializarConfiguracao.CodigoUF = pCUF

Set InicializarConfiguracao.CertificadoDigital = certificado
End Function
