Imports FireSharp.Config
Imports FireSharp.Response
Imports FireSharp.Interfaces
Public Class FormReg
    Private ifc As New FirebaseConfig With
    {
        .AuthSecret = "M4IRc1lMuL08Ug94iwBaIEf8xEHeo370sMe7u77A",
        .BasePath = "https://XXX-default-rtdb.asia-southeast1.firebasedatabase.app/"
    }

    Private client As IFirebaseClient
    Private Sub FormReg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            client = New FireSharp.FirebaseClient(ifc)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            MessageBox.Show("there was some problem in your internet.")
        End Try
        txtFname.Focus()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtFname.Text = Nothing Then
            MessageBox.Show("UserName field is empty." & vbCrLf & "Please fill in the First name field to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If txtLname.Text = Nothing Then
            MessageBox.Show("UserName field is empty." & vbCrLf & "Please fill in the Last name field to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If txtUser.Text = Nothing Then
            MessageBox.Show("Password field is empty." & vbCrLf & "Please fill in the UserName  to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If txtPass.Text = Nothing Then
            MessageBox.Show("Password field is empty." & vbCrLf & "Please fill in the  Password to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If txtCFPass.Text = Nothing Then
            MessageBox.Show("Password field is empty." & vbCrLf & "Please fill in the  Password to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If txtPass.Text <> txtCFPass.Text Then
            MessageBox.Show("Passwords do not match." & vbCrLf & "Please fill in the  Password to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim std As New PersonalData() With
            {
                .FirstName = txtFname.Text,
                .LastName = txtLname.Text,
                .Password = txtPass.Text,
                .UserName = txtUser.Text,
                .Status = "user"
            }
        Dim setter = client.Set("person/" + txtUser.Text, std)
        MessageBox.Show("data inserted successfully.")
        'Dim OpenFormLogin = New FormLogin
        'OpenFormLogin.Show()
        Me.Visible = False
        Controls.Clear()
        Dim ctrl = New UCLogin
        Controls.Add(ctrl)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        'Dim OpenFormLogin = New FormLogin
        'OpenFormLogin.Show()
        'Controls.Clear()
        'Dim ctrl = New UCLogin
        'Controls.Add(ctrl)
        Me.Visible = False
    End Sub

    Private Sub txtLname_TextChanged(sender As Object, e As EventArgs) Handles txtLname.TextChanged

    End Sub

    Private Sub txtFname_TextChanged(sender As Object, e As EventArgs) Handles txtFname.TextChanged

    End Sub

    Private Sub txtUser_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtPass_TextChanged(sender As Object, e As EventArgs) Handles txtPass.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub lbUser_Click(sender As Object, e As EventArgs) Handles lbUser.Click

    End Sub

    Private Sub lbPassword_Click(sender As Object, e As EventArgs) Handles lbPassword.Click

    End Sub

End Class
