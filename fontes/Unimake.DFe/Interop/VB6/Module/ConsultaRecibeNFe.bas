Attribute VB_Name = "ConsultaReciboNFe"
Option Explicit
Public Sub ConsultarReciboNFe()
On Error GoTo erro
Dim ConsReciNFe, RetAutorizacao

Log.ClearLog

Set ConsReciNFe = CreateObject("Unimake.Business.DFe.Xml.NFe.ConsReciNFe")
Set RetAutorizacao = CreateObject("Unimake.Business.DFe.Servicos.NFe.RetAutorizacao")

With ConsReciNFe
    .Versao = "4.00"
    .TpAmb = TpAmb
    .NRec = "310000069231900"
End With

RetAutorizacao.Executar (ConsReciNFe), (Config.InicializarConfiguracao(NFe))

Log.EscreveLog RetAutorizacao.RetornoWSString, True
Log.EscreveLog RetAutorizacao.result.XMotivo, False

Exit Sub
erro:
Utility.TrapException

End Sub


