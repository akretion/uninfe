Attribute VB_Name = "Log"
Private fMain As frmMain

Public Sub SetFrmMain(frm As frmMain)
Set fMain = frmMain
End Sub

Public Sub EscreveLog(ByVal Log As String, ByVal isXML As Boolean)
Dim XmlDoc As New MSXML2.DOMDocument60

If isXML Then
On Error GoTo erroXML
    XmlDoc.loadXML (Log)
    PrettyPrint XmlDoc
    Log = XmlDoc.xml
End If

continue:
fMain.EscreveLog Log
Exit Sub
erroXML:
Resume continue

End Sub

Public Sub ClearLog()
fMain.ClearLog
End Sub


