Attribute VB_Name = "AutorizarNFe"
Option Explicit
Public Sub AutorizarNFe()
On Error GoTo erro
Dim enviNFe
Dim autorizacao

frmMain.ClearLog

Set enviNFe = CreateObject("Unimake.Business.DFe.Xml.NFe.EnviNFe")
enviNFe.Versao = "4.00"
enviNFe.IdLote = "000000000000001"
enviNFe.IndSinc = 1
enviNFe.SetNFe GetNFe()

Set autorizacao = CreateObject("Unimake.Business.DFe.Servicos.NFe.Autorizacao")
autorizacao.SetXML enviNFe.GerarXML()
autorizacao.Inicializar (Config.InicializarConfiguracao())
autorizacao.Executar

frmMain.EscreveLog autorizacao.RetornoWSString
frmMain.EscreveLog autorizacao.result.XMotivo

Exit Sub
erro:
Utility.TrapException

End Sub

Function GetNFe()
Dim nfe
Set nfe = CreateObject("Unimake.Business.DFe.Xml.NFe.NFe")
nfe.SetInfNFe GetInfNFe()
Set GetNFe = nfe
End Function

                        
Function GetInfNFe()
Dim infNFe
Set infNFe = CreateObject("Unimake.Business.DFe.Xml.NFe.InfNFe")
infNFe.Versao = "4.00"
Set infNFe.Ide = GetIde()
Set infNFe.Emit = GetEmit()
Set infNFe.Dest = GetDest()
Set infNFe.Total = GetTotal()
Set infNFe.Transp = GetTransp()
Set infNFe.Cobr = GetCobr()
Set infNFe.Pag = GetPag()
Set infNFe.InfAdic = GetInfAdic()
Set infNFe.InfRespTec = GetInfRespTec()
infNFe.AddDet GetDet()
Set GetInfNFe = infNFe
End Function

Function GetIde()
Dim result
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.Ide")
With result
    .CUF = 41
    .NatOp = "VENDA PRODUC.DO ESTABELEC"
    .Mod = 55
    .Serie = 1
    .NNF = 57929
    .DhEmi = Now
    .DhSaiEnt = Now
    .TpNF = 1
    .IdDest = 2
    .CMunFG = 4118402
    .TpImp = 1
    .TpEmis = 1
    .TpAmb = 2
    .FinNFe = 1
    .IndFinal = 1
    .IndPres = 9
    .ProcEmi = 0
    .VerProc = "TESTE 1.00"
End With
Set GetIde = result
End Function

Function GetEmit()
Dim result
Dim ender
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.Emit")
With result
    .CNPJ = "06117473000150"
    .XNome = "UNIMAKE SOLUCOES CORPORATIVAS LTDA"
    .XFant = "UNIMAKE - PARANAVAI"
    .IE = "9032000301"
    .IM = "14018"
    .CNAE = "6202300"
    .CRT = 1
End With

Set ender = CreateObject("Unimake.Business.DFe.Xml.NFe.EnderEmit")
With ender
    .XLgr = "RUA ANTONIO FELIPE"
    .Nro = "1500"
    .XBairro = "CENTRO"
    .CMun = 4118402
    .XMun = "PARANAVAI"
    .UF = 41
    .CEP = "87704030"
    .Fone = "04431414900"
End With

Set result.EnderEmit = ender

Set GetEmit = result
End Function

Function GetDest()
Dim result
Dim ender
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.Dest")

With result
    .CNPJ = "04218457000128"
    .XNome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
    .IndIEDest = 1
    .IE = "582614838110"
    .Email = "janelaorp@janelaorp.com.br"
End With

Set ender = CreateObject("Unimake.Business.DFe.Xml.NFe.EnderDest")
With ender
    .XLgr = "AVENIDA DA SAUDADE"
    .Nro = "1555"
    .XBairro = "CAMPOS ELISEOS"
    .CMun = 3543402
    .XMun = "RIBEIRAO PRETO"
    .UF = 35
    .CEP = "14080000"
    .Fone = "01639611500"
End With

Set result.EnderDest = ender

Set GetDest = result
End Function

Function GetTotal()
Dim result
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.Total")
Set GetTotal = result
End Function

Function GetTransp()
Dim result
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.Transp")
Set GetTransp = result
End Function

Function GetCobr()
Dim result
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.Cobr")
Set GetCobr = result
End Function

Function GetPag()
Dim result
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.Pag")
Set GetPag = result
End Function

Function GetInfAdic()
Dim result
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.InfAdic")
Set GetInfAdic = result
End Function

Function GetInfRespTec()
Dim result
Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.InfRespTec")
Set GetInfRespTec = result
End Function

Function GetDet()
Dim result
Dim Prod
Dim Imposto
Dim ICMS, ICMSSN101
Dim PIS, PISOutr
Dim COFINS, COFINSOutr

Set result = CreateObject("Unimake.Business.DFe.Xml.NFe.Det")
result.NItem = 1

Set Prod = CreateObject("Unimake.Business.DFe.Xml.NFe.Prod")
With Prod
    .CProd = "01042"
    .CEAN = "SEM GTIN"
    .XProd = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
    .NCM = "84714900"
    .CFOP = "6101"
    .UCom = "LU"
    .QCom = 1#
    .VUnCom = 84.9
    .VProd = 84.9
    .CEANTrib = "SEM GTIN"
    .UTrib = "LU"
    .QTrib = 1#
    .VUnTrib = 84.9
    .IndTot = 1
    .XPed = "300474"
    .NItemPed = 1
End With

Set Imposto = CreateObject("Unimake.Business.DFe.Xml.NFe.Imposto")
Imposto.VTotTrib = 12.63

Set ICMS = CreateObject("Unimake.Business.DFe.Xml.NFe.ICMS")
Set ICMSSN101 = CreateObject("Unimake.Business.DFe.Xml.NFe.ICMSSN101")

With ICMSSN101
    .Orig = 0
    .PCredSN = 2.8255
    .VCredICMSSN = 2.4
End With

Set ICMS.ICMSSN101 = ICMSSN101
Imposto.AddICMS (ICMS)
                                                    
Set PIS = CreateObject("Unimake.Business.DFe.Xml.NFe.PIS")
Set PISOutr = CreateObject("Unimake.Business.DFe.Xml.NFe.PISOutr")

With PISOutr
    .CST = "99"
    .VBC = 0#
    .PPIS = 0#
    .VPIS = 0#
End With

Set PIS.PISOutr = PISOutr
Set Imposto.PIS = PIS

Set COFINS = CreateObject("Unimake.Business.DFe.Xml.NFe.COFINS")
Set COFINSOutr = CreateObject("Unimake.Business.DFe.Xml.NFe.COFINSOutr")

With COFINSOutr
    .CST = "99"
    .VBC = 0#
    .PCOFINS = 0#
    .VCOFINS = 0#
End With

Set COFINS.COFINSOutr = COFINSOutr
Set Imposto.COFINS = COFINS

Set GetDet = result
End Function
