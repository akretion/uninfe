VERSION 5.00
Begin VB.Form frmMain 
   Caption         =   "Unimake.DFe Interop Tests"
   ClientHeight    =   11850
   ClientLeft      =   60
   ClientTop       =   405
   ClientWidth     =   11580
   LinkTopic       =   "Form1"
   ScaleHeight     =   11850
   ScaleWidth      =   11580
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdAutorizarNFe 
      Caption         =   "Autorizar NF-e"
      Height          =   375
      Left            =   240
      TabIndex        =   5
      Top             =   1320
      Width           =   1575
   End
   Begin VB.CommandButton cmdConsultarSituacaoNFe 
      Caption         =   "Consultar Situacao"
      Height          =   375
      Left            =   240
      TabIndex        =   4
      Top             =   840
      Width           =   1575
   End
   Begin VB.TextBox txtLog 
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   4215
      Left            =   600
      MultiLine       =   -1  'True
      TabIndex        =   2
      Top             =   5160
      Width           =   8175
   End
   Begin VB.CommandButton cmdConsultarStatusNFe 
      Caption         =   "Consultar Status"
      Height          =   375
      Left            =   240
      TabIndex        =   0
      Top             =   360
      Width           =   1575
   End
   Begin VB.Frame Frame1 
      Caption         =   "NF-e"
      Height          =   2895
      Left            =   120
      TabIndex        =   1
      Top             =   0
      Width           =   1815
   End
   Begin VB.Label lblLog 
      Alignment       =   2  'Center
      Caption         =   "LOG"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   2040
      TabIndex        =   3
      Top             =   3120
      Width           =   4335
   End
   Begin VB.Line ln 
      BorderWidth     =   2
      X1              =   0
      X2              =   11535
      Y1              =   3480
      Y2              =   3495
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub cmdAutorizarNFe_Click()
AutorizarNFe.AutorizarNFe
End Sub

Private Sub cmdConsultarSituacaoNFe_Click()
ConsultarSituacao.ConsultarSituacao
End Sub

Private Sub cmdConsultarStatusNFe_Click()
ConsultarStatus.ConsultarStatus
End Sub

Private Sub Form_Resize()
lblLog.Left = 1
txtLog.Left = 1
lblLog.Width = Width
ln.X2 = Width
txtLog.Width = Width - 1
txtLog.Top = ln.Y1 + 50
txtLog.Height = Height - txtLog.Top
End Sub

Public Static Sub EscreveLog(ByVal log As String)
txtLog.Text = txtLog.Text & vbCrLf & " <<< " & Now & " >>>" & vbCrLf & log
End Sub

Public Static Sub ClearLog()
txtLog.Text = ""
End Sub

