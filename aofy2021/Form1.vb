Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim OpenFormMT = New FormMain(String.Empty)
        OpenFormMT.Show()
        Me.Visible = False
    End Sub
End Class
