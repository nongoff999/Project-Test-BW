Imports System
Imports System.Windows.Forms
Imports FireSharp.Config
Imports FireSharp.Response
Imports FireSharp.Interfaces
Public Class UCMain
    Dim strUser, strStatus, strStatusChangePass As String
    Public Property _strUser() As String
        Get
            Return strUser
        End Get
        Set(ByVal value As String)
            strUser = value
        End Set
    End Property
    Public Property _strStatus() As String
        Get
            Return strStatus
        End Get
        Set(ByVal value As String)
            strStatus = value
        End Set
    End Property

    Public Property _strStatusChangePass() As String
        Get
            Return strStatusChangePass
        End Get
        Set(ByVal value As String)
            strStatusChangePass = value
        End Set
    End Property
    Private ifc As New FirebaseConfig With
    {
        .AuthSecret = "M4IRc1lMuL08Ug94iwBaIEf8xEHeo370sMe7u77A",
        .BasePath = "https://aofy2021-default-rtdb.asia-southeast1.firebasedatabase.app/"
    }
    Private client As IFirebaseClient
    Private Sub FormReg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            client = New FireSharp.FirebaseClient(ifc)
            Dim res = client.Get("person/" + strUser)
            Dim Personal As New PersonalData()
            Personal = res.ResultAs(Of PersonalData)
            txtFname.Text = Personal.FirstName
            txtLname.Text = Personal.LastName
            txtUser.Text = Personal.UserName
            txtPass.Text = Personal.Password
            strStatus = Personal.Status

        Catch ex As Exception
            Try
                Dim res = client.Get("admin/" + strUser)
                Dim Personal As New PersonalData()
                Personal = res.ResultAs(Of PersonalData)
                txtFname.Text = Personal.FirstName
                txtLname.Text = Personal.LastName
                txtUser.Text = Personal.UserName
                txtPass.Text = Personal.Password
                strStatus = Personal.Status
            Catch ex2 As Exception
                MessageBox.Show(ex.Message)
            End Try

        End Try
        txtFname.Enabled = False
        txtLname.Enabled = False
        txtPass.Enabled = False
        btnSave.Enabled = False
    End Sub
    Public Sub New(strUserLogin As String)
        InitializeComponent()
        strUser = strUserLogin
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
        If strStatusChangePass = "Y" Then
            If txtCFPass.Text = Nothing Then
                MessageBox.Show("Password field is empty." & vbCrLf & "Please fill in the  Password to continue.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If txtNewPass.Text <> txtCFPass.Text Then
                MessageBox.Show("Passwords do not match.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtNewPass.Text = String.Empty
                txtCFPass.Text = String.Empty
                txtNewPass.Focus()
                Return
            End If
            If txtNewPass.Text = txtPass.Text Then
                MessageBox.Show("password duplicate  " & vbCrLf & " pls. input password again.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtNewPass.Text = String.Empty
                txtCFPass.Text = String.Empty
                txtNewPass.Focus()
                Return
            End If


        End If

        Try
            Dim Personal As New PersonalData()
            Personal.FirstName = txtFname.Text
            Personal.LastName = txtLname.Text
            Personal.UserName = txtUser.Text
            Personal.Password = IIf(strStatusChangePass = "Y", txtNewPass.Text, txtPass.Text)
            Personal.Status = strStatus
            If strStatus = "admin" Then
                Dim Res = client.Update("admin/" + strUser, Personal)
            Else
                Dim Res = client.Update("person/" + strUser, Personal)
            End If
            If strStatusChangePass = "Y" Then
                'Dim OpenFormMT = New FormMT(strUser)
                'OpenFormMT.Close()
                'Dim OpenFormLogin = New FormLogin
                'OpenFormLogin.Show()


                'Dim OpenFormLogin = New FormLogin
                'OpenFormLogin.Show()
                'Me.Visible = False
                Controls.Clear()
                Dim ctrl = New UCLogin
                Me.Controls.Add(ctrl)
            End If
            MessageBox.Show("Update success.")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Label4.Visible = False
        Label1.Visible = False
        txtFname.Enabled = False
        txtLname.Enabled = False
        txtNewPass.Visible = False
        txtCFPass.Visible = False
        btnEdit.Visible = False
        strStatusChangePass = "N"
        btnNoChangePass.Visible = False
        btnChangePass.Visible = False
        btnEdit.Visible = True
        btnSave.Enabled = False
    End Sub

    Private Sub btnChangePass_Click(sender As Object, e As EventArgs) Handles btnChangePass.Click

        Label4.Visible = True
        Label1.Visible = True
        txtFname.Enabled = False
        txtLname.Enabled = False
        txtNewPass.Visible = True
        txtCFPass.Visible = True
        btnEdit.Visible = False
        strStatusChangePass = "Y"
        btnNoChangePass.Visible = True
        btnChangePass.Visible = False
    End Sub

    Private Sub btnNoChangePass_Click(sender As Object, e As EventArgs) Handles btnNoChangePass.Click
        Label4.Visible = False
        Label1.Visible = False
        txtFname.Enabled = True
        txtLname.Enabled = True
        txtNewPass.Visible = False
        txtCFPass.Visible = False
        btnEdit.Visible = False
        strStatusChangePass = "N"
        btnNoChangePass.Visible = False
        btnChangePass.Visible = True
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        'Dim OpenFormLogin = New FormLogin
        'OpenFormLogin.Show()
        'Me.Visible = False
        Controls.Clear()
        Dim ctrl = New UCLogin
        Controls.Add(ctrl)



    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        btnEdit.Visible = False
        btnChangePass.Visible = True
        strStatusChangePass = "N"
        txtFname.Enabled = True
        txtLname.Enabled = True
        btnSave.Enabled = True
    End Sub
End Class
