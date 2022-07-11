Public Class FileSystemPurgeConfiguration
    Inherits System.Web.UI.Page

#Region "Event Handlers"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                GetListPurgeEntries()
                ResetPageData()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbList.SelectedIndexChanged
        Try
            LoadConfigData(Me.lbList.SelectedItem.Value)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim id As String
            id = Me.lbList.SelectedItem.Value

            If (Me.lbList.SelectedIndex >= 0) Then

                DA.ExecSQL("DELETE FROM tblFileSystemPurgeConfiguration WHERE FileSystemPurgeConfigurationID = " & id)
                'log event
                Master.tMsg("Delete", "File System Purge configuration entry with description: " & Me.lbList.SelectedItem.Text & " was deleted")

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally
            'reload page
            GetListPurgeEntries()
            ResetPageData()
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim errorMsg As String = ""
            If ValidateInputForSave(errorMsg) Then
                SaveConfig(Me.lbList.SelectedItem.Value)


                'reload listbox and set selected item
                Dim index As Integer = Me.lbList.SelectedIndex
                GetListPurgeEntries()
                Me.lbList.SelectedIndex = index
                LoadConfigData(Me.lbList.SelectedItem.Value)
            Else
                Master.eMsg(errorMsg)
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Dim strInput As String = tbDescriptionNew.Text.ToString()
        Try
            Dim errorMsg As String = ""
            If (ValidateDialogNew(errorMsg)) Then

                If (InsertArchivePurgeConfiguration()) Then
                    'log event
                    Master.tMsg("New", "New File System Purge configuration entry was created with description: " & strInput)

                    'reload listbox, set selected item, load data for that item
                    GetListPurgeEntries()
                    Dim index As Integer = Me.lbList.Items.IndexOf(Me.lbList.Items.FindByText(strInput))
                    Me.lbList.SelectedIndex = index
                    If (index >= 0) Then
                        LoadConfigData(Me.lbList.SelectedItem.Value)
                    End If
                Else
                    Master.tMsg("New", "New item was not created.")
                End If
            Else
                Master.eMsg("Error: " & errorMsg)
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"

    Private Function ValidateInputForSave(ByRef errorMsg As String) As Boolean
        Try
            Dim status As Boolean = True

            If (Me.tbDescription.Text.Length <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Description. Please enter a description. <br />"
                Me.imgQuestDesc.Visible = True
            Else : Me.imgQuestDesc.Visible = False
            End If

            If (Me.tbFolder.Text.Length <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Archive Folder Path. Please enter an archive folder path. <br />"
                Me.imgQuestPath.Visible = True
            Else : Me.imgQuestPath.Visible = False
            End If

            If (Me.tbSearchPattern.Text.Length <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Search Pattern. Please enter search pattern. <br />"
                Me.imgQuestSearch.Visible = True
            Else : Me.imgQuestSearch.Visible = False
            End If

            If (IsNumeric(Me.tbPurgeAge.Text)) Then
                Dim s As Single = Convert.ToSingle(Me.tbPurgeAge.Text)
                If (s < 0) Then
                    status = False And status
                    errorMsg = errorMsg & "Error: Invalid Age In Days For Purge. Please enter a purge age greater than 0. <br />"
                    Me.imgQuestAge.Visible = True
                Else : Me.imgQuestAge.Visible = False
                End If
            Else
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Age In Days For Purge. Please enter a purge age greater than 0. <br />"
                Me.imgQuestAge.Visible = True
            End If

            Return status
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Function ValidateDialogNew(ByRef errorMsg As String) As Boolean
        Try
            Dim status As Boolean = True

            If (Me.tbDescriptionNew.Text.Length <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Description. Please enter a description. <br />"
            End If

            If (Me.tbFolderNew.Text.Length <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Archive Folder Path. Please enter an archive folder path. <br />"
            End If

            If (Me.tbSearchPatternNew.Text.Length <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Search Pattern. Please enter search pattern. <br />"
            End If

            If (IsNumeric(Me.tbPurgeAgeNew.Text)) Then
                Dim s As Single = Convert.ToSingle(Me.tbPurgeAgeNew.Text)
                If (s < 0) Then
                    status = False And status
                    errorMsg = errorMsg & "Error: Invalid Age In Days For Purge. Please enter a purge age greater than 0. <br />"
                End If
            Else
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Age In Days For Purge. Please enter a purge age greater than 0. <br />"
            End If

            Return status
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            errorMsg = ex.ToString()
            Return False
        End Try
    End Function

    Private Function SaveConfig(ByVal id As String) As Boolean
        Try

            Dim ds As New DataSet
            Dim strsql As String

            strsql = "SELECT FileSystemPurgeConfigurationID FROM tblFileSystemPurgeConfiguration WHERE FileSystemPurgeConfigurationID = " & id
            ds.Reset()
            ds = DA.GetDataSet(strsql)

            'does the entry exist?
            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                Dim baseSQL As String = "UPDATE tblFileSystemPurgeConfiguration SET "
                Dim addSQL As String = ""

                Dim log As String = ""

                GetValuesToSave(addsql, log)

                If (addsql.Length <= 0) Then
                    Master.Msg = "Save cancelled: values have not changed."
                    Return True
                End If

                'remove ", " from last item
                addsql = addsql.TrimEnd(" "c)
                addSQL = addSQL.TrimEnd(","c)
                strsql = baseSQL & addSQL & " WHERE FileSystemPurgeConfigurationID = " & id
                DA.ExecSQL(strsql)
                Master.tMsg("save", "The following items have been saved <br> " & log)
            Else
                Master.eMsg("error: database archive purge entry does not exist! nothing was saved.")

            End If


            Return True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Sub GetValuesToSave(ByRef addSQL As String, ByRef log As String)

        'check for change in values
        If (Me.tbDescription.Text <> Me.tbDescription.OldText) Then
            addSQL += " Description = '" & Me.tbDescription.Text & "', "
            log += "Description: Old value - " & Me.tbDescription.OldText & ". New value - " & Me.tbDescription.Text & ".<br>"
            Me.imgDesc.Visible = True
        End If
        If (Me.cbAutoPurge.Checked <> Convert.ToBoolean(Me.cbAutoPurge.OldText)) Then
            addSQL += " AutoPurgeEnabled = " & Convert.ToInt32(Me.cbAutoPurge.Checked) & ", "
            log += "AutoPurgeEnabled: Old value - " & Me.cbAutoPurge.OldText & ". New value - " & Me.cbAutoPurge.Checked & ".<br>"
            Me.imgAutoPurge.Visible = True
        End If
        If (Me.tbFolder.Text <> Me.tbFolder.OldText) Then
            addSQL += " FolderPath = '" & Me.tbFolder.Text & "', "
            log += "FolderPath: Old value - " & Me.tbFolder.OldText & ". New value - " & Me.tbFolder.Text & ".<br>"
            Me.imgPath.Visible = True
        End If
        If (Me.cbSubfolders.Checked <> Convert.ToBoolean(Me.cbSubfolders.OldText)) Then
            addSQL += " IncludeSubfolders = " & Convert.ToInt32(Me.cbSubfolders.Checked) & ", "
            log += "IncludeSubfolders: Old value - " & Me.cbSubfolders.OldText & ". New value - " & Me.cbSubfolders.Checked & ".<br>"
            Me.imgSubF.Visible = True
        End If
        If (Me.tbSearchPattern.Text <> Me.tbSearchPattern.OldText) Then
            addSQL += " SearchPattern = '" & Me.tbSearchPattern.Text & "', "
            log += "SearchPattern: Old value - " & Me.tbSearchPattern.OldText & ". New value - " & Me.tbSearchPattern.Text & ".<br>"
            Me.imgSearch.Visible = True
        End If
        If (Me.tbPurgeAge.Text <> Me.tbPurgeAge.OldText) Then
            addSQL += " RequiredAgeInDaysForPurge = " & Convert.ToInt32(Me.tbPurgeAge.Text) & ", "
            log += "RequiredAgeInDaysForPurge: Old value - " & Me.tbPurgeAge.OldText & ". New value - " & Me.tbPurgeAge.Text & ".<br>"
            Me.imgAge.Visible = True
        End If

    End Sub

    Private Function LoadConfigData(ByVal id As String) As Boolean
        Dim ds As DataSet
        Dim status As Boolean = False
        Try
            ResetPageData()

            'make sure an item has been selected
            If (Me.lbList.SelectedIndex >= 0) Then

                Dim strSQL As String
                strSQL = "SELECT * FROM tblFileSystemPurgeConfiguration WHERE FileSystemPurgeConfigurationID = " & id

                ds = DA.GetDataSet(strSQL)

                If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                    With ds.Tables(0).Rows(0)
                        Me.tbDescription.Text = .Item("Description").ToString()
                        Me.cbAutoPurge.Checked = Convert.ToBoolean(.Item("AutoPurgeEnabled"))
                        Me.tbFolder.Text = .Item("FolderPath").ToString()
                        Me.cbSubfolders.Checked = Convert.ToBoolean(.Item("IncludeSubfolders"))
                        Me.tbSearchPattern.Text = .Item("SearchPattern").ToString()
                        Me.tbPurgeAge.Text = .Item("RequiredAgeInDaysForPurge").ToString()

                        'store values for validation/change comparison
                        Me.tbDescription.OldText = .Item("Description").ToString()
                        Me.cbAutoPurge.OldText = .Item("AutoPurgeEnabled").ToString()
                        Me.tbFolder.OldText = .Item("FolderPath").ToString()
                        Me.cbSubfolders.OldText = .Item("IncludeSubfolders").ToString()
                        Me.tbSearchPattern.OldText = .Item("SearchPattern").ToString()
                        Me.tbPurgeAge.OldText = .Item("RequiredAgeInDaysForPurge").ToString()

                        status = True


                    End With
                Else
                    Master.eMsg("ERROR:  Miconfigured purge configuration entry.  Please report to System Administrator!")
                    status = False
                End If

            End If

            Return status

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Sub GetListPurgeEntries()
        Dim ds As New DataSet
        Dim strSQL As String

        strSQL = "SELECT Description, FileSystemPurgeConfigurationID FROM tblFileSystemPurgeConfiguration"

        Try

            ds = DA.GetDataSet(strSQL)

            'make sure that there is data in the dataset
            If (Not ds Is Nothing) Then
                Me.lbList.DataSource = ds
                Me.lbList.DataTextField = "Description"
                Me.lbList.DataValueField = "FileSystemPurgeConfigurationID"
                Me.lbList.DataBind()
            Else
                Me.lbList.Items.Clear()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ResetPageData()

        Me.tbDescription.Text = ""
        Me.cbAutoPurge.Checked = False
        Me.tbFolder.Text = ""
        Me.cbSubfolders.Checked = False
        Me.tbSearchPattern.Text = ""
        Me.tbPurgeAge.Text = ""

        ResetDialogNew()
    End Sub

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdDelete)
            Master.Secure(Me.cmdNew)
            Master.Secure(Me.cmdSave)

            If lbList.SelectedItem Is Nothing Then
                cmdSave.Enabled = False
                cmdDelete.Enabled = False

                tbDescription.Enabled = False
                cbAutoPurge.Enabled = False
                tbFolder.Enabled = False
                cbSubfolders.Enabled = False
                tbSearchPattern.Enabled = False
                tbPurgeAge.Enabled = False
            Else

                tbDescription.Enabled = True
                cbAutoPurge.Enabled = True
                tbFolder.Enabled = True
                cbSubfolders.Enabled = True
                tbSearchPattern.Enabled = True
                tbPurgeAge.Enabled = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region

#Region "New Dialog"
    Private Sub ResetDialogNew()

        Me.tbDescriptionNew.Text = ""
        Me.cbAutoPurgeNew.Checked = False
        Me.tbFolderNew.Text = ""
        Me.cbSubfoldersNew.Checked = False
        Me.tbSearchPatternNew.Text = ""
        Me.tbPurgeAgeNew.Text = ""

    End Sub

    Public Function InsertArchivePurgeConfiguration() As Boolean

        Dim colParameters As New Collections.Generic.List(Of System.Data.SqlClient.SqlParameter)
        Dim colOutput As Collections.Generic.List(Of System.Data.SqlClient.SqlParameter)

        Try

            colParameters.Add(New SqlClient.SqlParameter("@Description", Me.tbDescriptionNew.Text.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@AutoPurgeEnabled", Me.cbAutoPurgeNew.Checked.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@FolderPath", Me.tbFolderNew.Text.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@IncludeSubfolders", Me.cbSubfoldersNew.Checked.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@SearchPattern", Me.tbSearchPatternNew.Text.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@RequiredAgeInDaysForPurge", Me.tbPurgeAgeNew.Text.ToString()))

            colOutput = DA.ExecSP("procInsertFileSystemPurgeConfiguration", colParameters)


            Return True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function
#End Region

End Class