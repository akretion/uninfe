Attribute VB_Name = "ConsultarSituacao"
Public Sub ConsultarSituacao()
On Error GoTo erro
Dim consSitNFe
Dim consultaProtocolo

frmMain.ClearLog

Set consSitNFe = CreateObject("Unimake.Business.DFe.Xml.NFe.ConsSitNFe")
consSitNFe.Versao = "4.00"
consSitNFe.TpAmb = 1
consSitNFe.ChNFe = "41200106117473000150550010000606641403753210"

Set consultaProtocolo = CreateObject("Unimake.Business.DFe.Servicos.NFe.ConsultaProtocolo")
consultaProtocolo.SetXML consSitNFe.GerarXML()
consultaProtocolo.Inicializar (Config.InicializarConfiguracao())
consultaProtocolo.Executar

frmMain.EscreveLog consultaProtocolo.RetornoWSString
frmMain.EscreveLog consultaProtocolo.Result.XMotivo

Exit Sub
erro:
Utility.TrapException

End Sub

