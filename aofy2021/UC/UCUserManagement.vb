Imports System
Imports System.Windows.Forms
Imports FireSharp.Config
Imports FireSharp.Response
Imports FireSharp.Interfaces
Imports System.Web.Script.Serialization
Imports System.ComponentModel

Public Class UCUserManagement
    Dim strUser, strStatusUser As String
    Public Property _strUser() As String
        Get
            Return strUser
        End Get
        Set(ByVal value As String)
            strUser = value
        End Set
    End Property
    Public Property _strStatusUser() As String
        Get
            Return strStatusUser
        End Get
        Set(ByVal value As String)
            strStatusUser = value
        End Set
    End Property
    Private dtTableGrd As DataTable

    Private ifc As New FirebaseConfig With
    {
        .AuthSecret = "M4IRc1lMuL08Ug94iwBaIEf8xEHeo370sMe7u77A",
        .BasePath = "https://aofy2021-default-rtdb.asia-southeast1.firebasedatabase.app/"
    }
    Private client As IFirebaseClient
    Public Sub New(strUserLogin As String)
        InitializeComponent()
        strUser = strUserLogin
    End Sub
    Private Sub FormReg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            client = New FireSharp.FirebaseClient(ifc)

            ShowRecord()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            MessageBox.Show("there was some problem in your internet.")
        End Try
        txtStatus.Text = "user"
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
                .Status = "user",
                .UserName = txtUser.Text
            }

        Dim setter = client.Set("person/" + txtUser.Text, std)
        MessageBox.Show("data inserted successfully.")
        btnRefresh_Click(sender, e)
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtFname.Text = String.Empty
        txtLname.Text = String.Empty
        txtUser.Text = String.Empty
        txtPass.Text = String.Empty
        txtCFPass.Text = String.Empty
        txtStatus.Text = "user"
        btnSave.Text = "Save"
        txtPass.Enabled = True
        txtCFPass.Enabled = True
        txtUser.Enabled = True
        btnClear.Visible = False
        btnDel.Visible = False
    End Sub
    Private Sub ShowRecord()
        Try
            Dim dtTable As New DataTable
            dtTable.Columns.Add("FirstName")
            dtTable.Columns.Add("LastName")
            dtTable.Columns.Add("UserName")
            dtTable.Columns.Add("Password")
            dtTable.Columns.Add("Status")

            '-------------------------------------------Conditions for deleting columns. This was executed only once.
            'If clearDGVCol = True Then
            GVUserData.Columns.Clear()
            '    clearDGVCol = False
            'End If
            '-------------------------------------------

            Dim SRRecord = client.Get("person/") 'To load and hold Database in JSON file format.

            '-------------------------------------------To converts the specified JSON string to an object of type T.
            '-------------------------------------------For more information see here : 
            '-------------------------------------------https://docs.microsoft.com/en-us/dotnet/api/system.web.script.serialization.javascriptserializer.deserialize?view=netframework-4.8
            Dim myJsonTool As New JavaScriptSerializer
            Dim myDeserializedItems = myJsonTool.Deserialize(Of Dictionary(Of String, PersonalData))(SRRecord.Body)
            '-------------------------------------------

            '-------------------------------------------To enter a Database (in JSON file format that has been previously converted into an object form) into a Table.
            For Each dictItem As KeyValuePair(Of String, PersonalData) In myDeserializedItems
                dtTable.Rows.Add(dictItem.Value.FirstName, dictItem.Value.LastName, dictItem.Value.UserName, dictItem.Value.Password, dictItem.Value.Status)
            Next
            '-------------------------------------------

            GVUserData.DataSource = dtTable 'Gets or sets the data source that the DataGridView is displaying data for.
            dtTableGrd = dtTable 'Entering data from dtTable into dtTableGrd, dtTableGrd is used to find data and display it on DataGridView.

            GVUserData.Sort(GVUserData.Columns(0), ListSortDirection.Ascending)

            lbTotalUser.Text = "Total Users : " & GVUserData.RowCount

            GVUserData.ClearSelection()
        Catch ex As Exception
            If ex.Message = "One or more errors occurred." Then
                MessageBox.Show("Cannot connect to firebase, check your network !", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf ex.Message = "Object reference not set to an instance of an object." Then
                Dim dtTable As New DataTable
                dtTable.Columns.Add("FirstName")
                dtTable.Columns.Add("LastName")
                dtTable.Columns.Add("UserName")
                dtTable.Columns.Add("Password")
                dtTable.Columns.Add("Status")
                GVUserData.DataSource = dtTable
                MessageBox.Show("Database not found or Database is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
        lbRecView.Text = "Record View"

    End Sub


    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Me.Text = "VB Net Firebase RealTime Database (Loading. Please wait...)"
        lbRecView.Text = "Record View (Loading. Please wait...)"

        ShowRecord()
    End Sub

    Private Sub btnDel_Click(sender As Object, e As EventArgs) Handles btnDel.Click
        'If AllCellsSelected(GVUserData) = True Then
        '    Try
        '        If MsgBox("Are you sure you want to delete all data ?", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Confirmation") = MsgBoxResult.Cancel Then Return

        '        Me.Text = "VB Net Firebase RealTime Database (Deleting. Please wait...)"
        '        lbRecView.Text = "Record View (Deleting. Please wait...)"


        '        Dim deleteAll = client.Delete("person") 'To delete data in the Firebase Database

        '        MessageBox.Show("Data successfully deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)


        '        lbRecView.Text = "Record View"


        '        btnRefresh_Click(sender, e)
        '        Return
        '    Catch ex As Exception
        '        If ex.Message = "One or more errors occurred." Then
        '            MessageBox.Show("Cannot connect to firebase, check your network !", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Else
        '            MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        End If


        '        lbRecView.Text = "Record View"
        '        btnRefresh_Click(sender, e)
        '    End Try
        'End If
        '-------------------------------------------

        '-------------------------------------------Conditions for deleting one data or several data in the Database.
        Try
            'If GVUserData.SelectedRows.Count = 0 Then
            '    MessageBox.Show("Please select one row or several rows to be deleted.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Return
            'End If

            If MsgBox("Are you sure you want to delete this data ?", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Confirmation") = MsgBoxResult.Cancel Then Return

            Me.Text = "VB Net Firebase RealTime Database (Deleting. Please wait...)"
            lbRecView.Text = "Record View (Deleting. Please wait...)"


            '-------------------------------------------

            'For Each row As DataGridViewRow In GVUserData.SelectedRows
            '    If row.Selected = True Then
            '        Dim delete = client.Delete("person/" & row.DataBoundItem(1).ToString)
            '    End If
            'Next
            If txtUser.Text = Nothing Then
                MessageBox.Show("Please select one row or several rows to be deleted.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim delete = client.Delete("person/" & txtUser.Text)
            MessageBox.Show("Data successfully deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

            lbRecView.Text = "Record View"


            btnRefresh_Click(sender, e)
        Catch ex As Exception
            If ex.Message = "One or more errors occurred." Then
                MessageBox.Show("Cannot connect to firebase, check your network !", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            lbRecView.Text = "Record View"

        End Try
        '-------------------------------------------
    End Sub
    Private Function AllCellsSelected(dgv As DataGridView) As Boolean
        If dgv.RowCount = 0 Then
            AllCellsSelected = False
            Return AllCellsSelected
        End If
        AllCellsSelected = (dgv.SelectedCells.Count = (dgv.RowCount * dgv.Columns.GetColumnCount(DataGridViewElementStates.Visible)))
        If dgv.RowCount = 1 Then
            AllCellsSelected = False
        End If
    End Function

    'Private Sub btnClearSelect_Click(sender As Object, e As EventArgs)
    '    GVUserData.ClearSelection()
    'End Sub

    Private Sub txtFname_TextChanged(sender As Object, e As EventArgs) Handles txtFname.TextChanged
        btnClear.Visible = True

    End Sub

    Private Sub txtLname_TextChanged(sender As Object, e As EventArgs) Handles txtLname.TextChanged
        btnClear.Visible = True
    End Sub

    Private Sub txtUser_TextChanged(sender As Object, e As EventArgs) Handles txtUser.TextChanged
        btnClear.Visible = True
    End Sub

    Private Sub txtPass_TextChanged(sender As Object, e As EventArgs) Handles txtPass.TextChanged
        btnClear.Visible = True
    End Sub

    Private Sub txtCFPass_TextChanged(sender As Object, e As EventArgs) Handles txtCFPass.TextChanged
        btnClear.Visible = True
    End Sub

    Private Sub GVUserData_MouseUp(sender As Object, e As MouseEventArgs) Handles GVUserData.MouseUp
        btnEdit_Click(sender, e)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        '-------------------------------------------The condition to check whether the data to be edited has been selected in DataGridView
        'If GVUserData.SelectedRows.Count = 0 Then
        '    MessageBox.Show("Please select one row in the table to edit.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Return
        'End If
        '-------------------------------------------

        '-------------------------------------------Conditions to check whether there is more than one data selected on DataGridView.
        If GVUserData.SelectedRows.Count > 1 Then
            MessageBox.Show("You select " & GVUserData.SelectedRows.Count & " rows in the Table." & vbCrLf & "The Edit feature can only edit one row in a table.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        '-------------------------------------------

        If GVUserData.SelectedRows.Count = 1 Then
            txtFname.Text = GVUserData.SelectedRows(0).Cells("FirstName").Value
            txtLname.Text = GVUserData.SelectedRows(0).Cells("LastName").Value
            txtUser.Text = GVUserData.SelectedRows(0).Cells("Username").Value
            txtPass.Text = GVUserData.SelectedRows(0).Cells("Password").Value
            txtStatus.Text = GVUserData.SelectedRows(0).Cells("Status").Value
            txtCFPass.Text = GVUserData.SelectedRows(0).Cells("Password").Value
            btnSave.Text = "Update"
            txtPass.Enabled = False
            txtCFPass.Enabled = False
            txtUser.Enabled = False
            btnDel.Visible = True
            btnClear.Visible = True
        End If
    End Sub
End Class
