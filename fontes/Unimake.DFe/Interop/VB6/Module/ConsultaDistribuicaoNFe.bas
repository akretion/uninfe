Attribute VB_Name = "ConsultaDistribuicaoNFe"
Option Explicit

Public Sub ConsultarDistribuicaoNFe()
On Error GoTo erro
Dim DistDFeInt, DistNSU, DistribuicaoDFe
Dim nsu     As String: nsu = "000000000000000"
Dim folder  As String: folder = "D:\temp\uninfe" ''<<<altere para um paste existente em sua m�quina

Log.ClearLog

Set DistDFeInt = CreateObject("Unimake.Business.DFe.xml.NFe.DistDFeInt")
Set DistNSU = CreateObject("Unimake.Business.DFe.xml.NFe.DistNSU")
Set DistribuicaoDFe = CreateObject("Unimake.Business.DFe.Servicos.NFe.DistribuicaoDFe")


Do While True
    Log.EscreveLog "Aguarde, consultando NSU n�mero " & nsu, False
    DistNSU.UltNSU = nsu
    
    With DistDFeInt
        .Versao = "1.01"
        .TpAmb = TpAmb
        .CNPJ = "06117473000150"
        .CUFAutor = CUF
        Set .DistNSU = DistNSU
    End With

    DistribuicaoDFe.Executar (DistDFeInt), (Config.InicializarConfiguracao(NFe))
    
    If (DistribuicaoDFe.result.CStat = 138) Then ''Documentos localizados
    
        If Not DirExists(folder) Then MkDir folder
        DistribuicaoDFe.GravarXMLDocZIP folder, True
        nsu = DistribuicaoDFe.result.UltNSU

        If CInt(DistribuicaoDFe.result.UltNSU) >= CInt(DistribuicaoDFe.result.MaxNSU) Then Exit Do
    Else
        Log.EscreveLog DistribuicaoDFe.result.XMotivo, False
        Exit Do
    End If
Loop

Log.EscreveLog "Consulta de distribui��o conclu�da com sucesso", False

Exit Sub
erro:
Utility.TrapException

End Sub
