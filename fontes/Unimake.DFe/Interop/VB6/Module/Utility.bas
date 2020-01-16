Attribute VB_Name = "Utility"
Public Sub TrapException()
    MsgBox Err.Description, vbCritical, "Erro!!!"
End Sub
