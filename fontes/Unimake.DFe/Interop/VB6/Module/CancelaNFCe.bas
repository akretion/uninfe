Attribute VB_Name = "CancelaNFCe"
Option Explicit

Public Sub CancelarNFCe()
On Error GoTo erro
Dim EnvEvento, RecepcaoEvento, Evento, InfEvento, DetEventoCanc, CStat

Log.ClearLog

Set RecepcaoEvento = CreateObject("Unimake.Business.DFe.Servicos.NFCe.RecepcaoEvento")
Set EnvEvento = CreateObject("Unimake.Business.DFe.Xml.NFe.EnvEvento")
Set Evento = CreateObject("Unimake.Business.DFe.Xml.NFe.Evento")
Set InfEvento = CreateObject("Unimake.Business.DFe.Xml.NFe.InfEvento")
Set DetEventoCanc = CreateObject("Unimake.Business.DFe.Xml.NFe.DetEventoCanc")

With DetEventoCanc
    .NProt = "141190000660363"
    .Versao = "1.00"
    .XJust = "Justificativa para cancelamento da NFe de teste"
End With
              
With InfEvento
    Set .DetEvento = DetEventoCanc
    .COrgao = CUF
    .ChNFe = "41190806117473000150550010000579131943463890"
    .CNPJ = "06117473000150"
    .DhEvento = DateTime.Now
    .TpEvento = 110111
    .NSeqEvento = 1
    .VerEvento = "1.00"
    .TpAmb = TpAmb
End With
    
Evento.Versao = "1.00"
Set Evento.InfEvento = InfEvento

EnvEvento.AddEvento (Evento)
EnvEvento.Versao = "1.00"
EnvEvento.IdLote = "000000000000001"

RecepcaoEvento.Executar (EnvEvento), (Config.InicializarConfiguracao(NFCe))

''Gravar o XML de distribui��o se a inutiliza��o foi homologada
If (RecepcaoEvento.result.CStat = 128) Then ''128 = Lote de evento processado com sucesso
    CStat = RecepcaoEvento.result.GetEvento(0).InfEvento.CStat
    
    '' 135: Evento homologado com vincula��o da respectiva NFe
    '' 136: Evento homologado sem vincula��o com a respectiva NFe (SEFAZ n�o encontrou a NFe na base dela)
    '' 155: Evento de Cancelamento homologado fora do prazo permitido para cancelamento
                        
    Select Case CStat
        Case 135, 136, 155
            RecepcaoEvento.GravarXmlDistribuicao "D:\temp\"
        Case Else ''Evento rejeitado
            Log.EscreveLog "Evento rejeitado", False
    End Select
End If

Log.EscreveLog RecepcaoEvento.RetornoWSString, True

Exit Sub
erro:
Utility.TrapException

End Sub
