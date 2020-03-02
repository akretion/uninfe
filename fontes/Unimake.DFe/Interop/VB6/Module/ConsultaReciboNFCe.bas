Attribute VB_Name = "ConsultaReciboNFCe"
Option Explicit
Public Sub ConsultarReciboNFCe()
On Error GoTo erro
Dim ConsReciNFe, RetAutorizacao

Log.ClearLog

Set ConsReciNFe = CreateObject("Unimake.Business.DFe.Xml.NFe.ConsReciNFe")
Set RetAutorizacao = CreateObject("Unimake.Business.DFe.Servicos.NFCe.RetAutorizacao")

With ConsReciNFe
    .Versao = "4.00"
    .TpAmb = TpAmb
    .NRec = CUF & "3456789012345"
End With

RetAutorizacao.Executar (ConsReciNFe), (Config.InicializarConfiguracao(NFCe))

Log.EscreveLog RetAutorizacao.RetornoWSString, True
Log.EscreveLog RetAutorizacao.result.XMotivo, False

Exit Sub
erro:
Utility.TrapException

End Sub



