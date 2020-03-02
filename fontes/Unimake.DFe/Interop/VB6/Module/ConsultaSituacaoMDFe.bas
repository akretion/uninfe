Attribute VB_Name = "ConsultaSituacaoMDFe"
Option Explicit

Public Sub ConsultarSituacaoMDFe()
On Error GoTo erro
Dim ConsSitMDFe
Dim consultaProtocolo

Log.ClearLog

Set ConsSitMDFe = CreateObject("Unimake.Business.DFe.Xml.MDFe.ConsSitMDFe")
ConsSitMDFe.Versao = "3.00"
ConsSitMDFe.TpAmb = TpAmb
ConsSitMDFe.ChMDFe = CUF & "170701761135000132570010000186931903758906"

Set consultaProtocolo = CreateObject("Unimake.Business.DFe.Servicos.MDFe.ConsultaProtocolo")
consultaProtocolo.Executar (ConsSitMDFe), (Config.InicializarConfiguracao(MDFe))

Log.EscreveLog consultaProtocolo.RetornoWSString, True
Log.EscreveLog consultaProtocolo.result.XMotivo, True

Exit Sub
erro:
Utility.TrapException

End Sub


