Imports FireSharp.Config
Imports FireSharp.Response
Imports FireSharp.Interfaces
Public Class UCLogin
    Dim strUser As String
    Public Property _strUser() As String
        Get
            Return strUser
        End Get
        Set(ByVal value As String)
            strUser = value
        End Set
    End Property
    Private ifc As New FirebaseConfig With
    {
        .AuthSecret = "M4IRc1lMuL08Ug94iwBaIEf8xEHeo370sMe7u77A",
        .BasePath = "https://aofy2021-default-rtdb.asia-southeast1.firebasedatabase.app/"
    }

    Private client As IFirebaseClient
    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            client = New FireSharp.FirebaseClient(ifc)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            MessageBox.Show("there was some problem in your internet.")
        End Try
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        Try
            Dim res = client.Get("person/" + txtUser.Text.Trim)
            Dim Personal As New PersonalData()
            Personal = res.ResultAs(Of PersonalData)
            If txtUser.Text = Nothing Then
                MessageBox.Show("UserName field is empty." & vbCrLf & "Please fill in the UserName field to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If txtPass.Text = Nothing Then
                MessageBox.Show("Password field is empty." & vbCrLf & "Please fill in the name Password to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If Not Personal Is Nothing Then

                If txtUser.Text.Trim = Personal.UserName And txtPass.Text.Trim = Personal.Password Then
                    strUser = txtUser.Text
                    'Dim OpenFormMT = New FormMain(strUser)
                    'OpenFormMT.Show()
                    'Me.Visible = False
                    Controls.Clear()
                    Dim ctrl = New UCMain(strUser)
                    Controls.Add(ctrl)
                Else


                    strUser = String.Empty
                    MessageBox.Show("Username or password is incorrect")
                End If

            Else
                res = client.Get("admin/" + txtUser.Text.Trim)
                Personal = res.ResultAs(Of PersonalData)
                If Not Personal Is Nothing Then
                    If txtUser.Text.Trim = Personal.UserName And txtPass.Text.Trim = Personal.Password Then
                        strUser = txtUser.Text
                        'Dim OpenFormMT = New FormMain(strUser)
                        'OpenFormMT.Show()
                        'Me.Visible = False
                        Controls.Clear()
                        Dim ctrl = New UCUserManagement(strUser)
                        Controls.Add(ctrl)
                    Else
                        res = client.Get("admin/" + txtUser.Text.Trim)
                        MessageBox.Show("Username or password is incorrect")
                    End If
                Else
                    strUser = String.Empty
                    MessageBox.Show("Username or password is incorrect")
                End If
            End If
        Catch ex As Exception

        End Try



    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim oForm As New FormReg
        oForm.Show()
        'Me.Visible = False
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Application.Exit()
    End Sub

    Private Sub txtPass_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPass.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnLogin_Click(sender, e)
        End If
    End Sub
End Class
