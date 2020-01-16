Attribute VB_Name = "ConsultarStatus"
Public Sub ConsultarStatus()
On Error GoTo erro
Dim consStatServ
Dim statusServico

frmMain.ClearLog

Set consStatServ = CreateObject("Unimake.Business.DFe.Xml.NFe.ConsStatServ")
consStatServ.Versao = "4.00"
consStatServ.CUF = 41
consStatServ.TpAmb = 1

Set statusServico = CreateObject("Unimake.Business.DFe.Servicos.NFe.StatusServico")
statusServico.SetXML consStatServ.GerarXML()
statusServico.Inicializar (Config.InicializarConfiguracao())
statusServico.Executar

frmMain.EscreveLog statusServico.RetornoWSString
frmMain.EscreveLog statusServico.Result.XMotivo

Exit Sub
erro:
Utility.TrapException
End Sub
