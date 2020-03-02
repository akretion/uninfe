Attribute VB_Name = "Utility"
Public Const CUF = 41
Public Const AN = 91
Public Const TpAmb = 2

Public Sub TrapException()
    MsgBox Err.Description, vbCritical, "Erro!!!"
    Log.EscreveLog "========== ERRO ==========", False
    Log.EscreveLog Err.Description, False
    Log.EscreveLog "==========================", False
End Sub

Public Function DirExists()
On Error GoTo erro
    If GetAttr(Path) And vbDirectory Then
        DirExists = True
    Else
        DirExists = False
    End If
    
Exit Function

erro:
DirExists = False
End Function
