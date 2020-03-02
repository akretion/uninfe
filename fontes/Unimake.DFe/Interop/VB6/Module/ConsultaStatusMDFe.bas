Attribute VB_Name = "ConsultaStatusMDFe"
Option Explicit

Public Sub ConsultarStatusMDFe()
On Error GoTo erro
Dim ConsStatServMDFe
Dim statusServico

Log.ClearLog

Set ConsStatServMDFe = CreateObject("Unimake.Business.DFe.Xml.MDFe.ConsStatServMDFe")
Set statusServico = CreateObject("Unimake.Business.DFe.Servicos.MDFe.StatusServico")

ConsStatServMDFe.Versao = "3.00"
ConsStatServMDFe.TpAmb = TpAmb
statusServico.Executar (ConsStatServMDFe), (Config.InicializarConfiguracao(MDFe, CUF))

Log.EscreveLog statusServico.RetornoWSString, True
Log.EscreveLog statusServico.result.XMotivo, False

Exit Sub
erro:
Utility.TrapException
End Sub


