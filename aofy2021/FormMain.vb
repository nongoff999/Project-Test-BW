Imports System
Imports System.Windows.Forms
Imports FireSharp.Config
Imports FireSharp.Response
Imports FireSharp.Interfaces
Public Class FormMain

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
        .BasePath = "https://XXX-default-rtdb.asia-southeast1.firebasedatabase.app/"
    }
    Private client As IFirebaseClient
    Private Sub FormReg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'If strUser <> String.Empty Then
        '    Try
        '        client = New FireSharp.FirebaseClient(ifc)
        '        Dim res = client.Get("person/" + strUser)
        '        Dim Personal As New PersonalData()
        '        Personal = res.ResultAs(Of PersonalData)
        '        UserManagementToolStripMenuItem.Visible = False
        '        MainToolStripMenuItem.Visible = True
        '        Dim ctrl = New UCMain(strUser)
        '        Me.PanelUser.Controls.Add(ctrl)
        '    Catch ex As Exception
        '        Try
        '            Dim res = client.Get("admin/" + strUser)
        '            Dim Personal As New PersonalData()
        '            Personal = res.ResultAs(Of PersonalData)
        '            UserManagementToolStripMenuItem.Visible = True
        '            MainToolStripMenuItem.Visible = False
        '            Dim ctrl = New UCUserManagement(strUser)
        '            Me.PanelUser.Controls.Add(ctrl)

        '        Catch ex2 As Exception
        '            MessageBox.Show(ex.Message)
        '            Dim ctrl = New UCLogin()
        '            Me.PanelUser.Controls.Add(ctrl)
        '        End Try

        '    End Try
        'Else
        '    UserManagementToolStripMenuItem.Visible = False
        '    MainToolStripMenuItem.Visible = False
        '    Dim ctrl = New UCLogin()
        '    Me.PanelUser.Controls.Add(ctrl)
        'End If

        UserManagementToolStripMenuItem.Visible = False
        MainToolStripMenuItem.Visible = False
        Dim ctrl = New UCLogin()
        Me.PanelUser.Controls.Add(ctrl)

    End Sub
    Public Sub New(strUserLogin As String)
        InitializeComponent()
        strUser = strUserLogin
    End Sub

    Private Sub MainToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MainToolStripMenuItem.Click
        Me.PanelUser.Controls.Clear()
        Dim ctrl = New UCMain(strUser)
        Me.PanelUser.Controls.Add(ctrl)
    End Sub

    Private Sub UserManagementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserManagementToolStripMenuItem.Click
        Me.PanelUser.Controls.Clear()
        Dim ctrl = New UCUserManagement(strUser)
        Me.PanelUser.Controls.Add(ctrl)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        'Application.Exit()
        Environment.Exit(0)
    End Sub
End Class
