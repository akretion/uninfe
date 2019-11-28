Dim statusService
Dim request
dim configuracao
dim statusServico

'Cria o serviço de acesso ao ecoomerce
set statusService = CreateObject("Unimake.Business.DFe.Xml.NFe.ConsStatServ")
statusService.Versao = "4.00"
statusService.CUF = 2
statusService.TpAmb = 1

set configuracao = CreateObject("Unimake.Business.DFe.Servicos.Configuracao")
configuracao.TipoDFe = 0


set statusServico = CreateObject("Unimake.Business.DFe.Servicos.NFe.StatusServico")
statusServico.Teste
MsgBox statusServico.RetornoWSString
MsgBox statusServico.Result.XMotivo