Public Class DatabaseArchivePurgeConfiguration
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

    Private Sub ddlTableName_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTableName.SelectedIndexChanged
        Try
            LoadDTFieldNames(Me.ddlTableName.SelectedItem.Value)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
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

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim id As String
            id = Me.lbList.SelectedItem.Value

            If (Me.lbList.SelectedIndex >= 0) Then

                DA.ExecSQL("DELETE FROM tblArchivePurgeConfiguration WHERE ArchivePurgeConfigurationID = " & id)
                'log event
                Master.tMsg("Delete", "Database Archive Purge configuration entry with description: " & Me.lbList.SelectedItem.Text & " was deleted")

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally
            'reload page
            GetListPurgeEntries()
            ResetPageData()
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Dim strInput As String = tbDescriptionNew.Text.ToString()
        Try
            Dim errorMsg As String = ""
            If (ValidateDialogNew(errorMsg)) Then

                If (InsertArchivePurgeConfiguration()) Then
                    'log event
                    Master.tMsg("New", "New Database Archive Purge configuration entry was created with description: " & strInput)

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
                Master.eMsg(errorMsg)
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

            If (Me.tbArchiveFolder.Text.Length <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Archive Folder Path. Please enter an archive folder path. <br />"
                Me.imgQuestPath.Visible = True
            Else : Me.imgQuestPath.Visible = False
            End If

            If (IsNumeric(Me.tbArchiveAge.Text)) Then
                Dim s As Single = Convert.ToSingle(Me.tbArchiveAge.Text)
                If (s < 0) Then
                    status = False And status
                    errorMsg = errorMsg & "Error: Invalid Age In Days For Archive. Please enter an archive age greater than 0. <br />"
                    Me.imgQuestArchAge.Visible = True
                Else : Me.imgQuestArchAge.Visible = False
                End If
            Else
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Age In Days For Archive. Please enter an archive age greater than 0. <br />"
                Me.imgQuestArchAge.Visible = True
            End If

            If (IsNumeric(Me.tbPurgeAge.Text)) Then
                Dim s As Single = Convert.ToSingle(Me.tbPurgeAge.Text)
                If (s < 0) Then
                    status = False And status
                    errorMsg = errorMsg & "Error: Invalid Age In Days For Purge. Please enter a purge age greater than 0. <br />"
                    Me.imgQuestPurgeAge.Visible = True
                Else : Me.imgQuestPurgeAge.Visible = False
                End If
            Else
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Age In Days For Purge. Please enter a purge age greater than 0. <br />"
                Me.imgQuestPurgeAge.Visible = True
            End If

            If (Me.ddlTableName.SelectedIndex <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Table Name. Please select a table. <br />"
                Me.imgQuestTable.Visible = True
            Else : Me.imgQuestTable.Visible = False
            End If

            If (Me.ddlDTFieldName.SelectedIndex <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid D/T Field Name. Please select a field. <br />"
                Me.imgQuestDT.Visible = True
            Else : Me.imgQuestDT.Visible = False
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

            If (Me.tbDescriptionNew.Text.Length > 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid description. Please enter a description. <br />"
            End If

            If (IsNumeric(Me.tbArchiveAgeNew.Text)) Then
                Dim s As Single = Convert.ToSingle(Me.tbArchiveAgeNew.Text)
                If (s < 0) Then
                    status = False And status
                    errorMsg = errorMsg & "Error: Invalid Archive Age. Please enter an archive age greater than 0. <br />"
                End If
            Else
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Archive Age. Please enter an archive age greater than 0. <br />"
            End If

            If (Me.tbArchiveFolderNew.Text.Length > 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Archive Folder Path. Please enter an archive folder path. <br />"

            End If


            If (IsNumeric(Me.tbPurgeAgeNew.Text)) Then
                Dim s As Single = Convert.ToSingle(Me.tbPurgeAgeNew.Text)
                If (s < 0) Then
                    status = False And status
                    errorMsg = errorMsg & "Error: Invalid Purge Age. Please enter a purge age greater than 0. <br />"
                End If
            Else
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Purge Age. Please enter a purge age greater than 0. <br />"
            End If

            If (Me.ddlTableNameNew.SelectedIndex <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid Table Name. Please select a table. <br />"

            End If
            If (Me.ddlDTFieldNameNew.SelectedIndex <= 0) Then
                status = False And status
                errorMsg = errorMsg & "Error: Invalid D/T Field Name. Please select a field. <br />"

            End If


            Return status
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Function SaveConfig(ByVal id As String) As Boolean
        Try

            Dim ds As New DataSet
            Dim strsql As String

            strsql = "select *, convert(varchar, lastarchivedrecorddt, 121) 'lastarchivedrecorddt_121' from tblarchivepurgeconfiguration  where archivepurgeconfigurationid = " & id
            ds.Reset()
            ds = DA.GetDataSet(strsql)

            'does the entry exist?
            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                Dim basesql As String = "update tblarchivepurgeconfiguration set "
                Dim addsql As String = ""

                Dim log As String = ""

                GetValuesToSave(addsql, log)

                If (addsql.Length <= 0) Then
                    Master.Msg = "not saved: values have not changed."
                    Return True
                End If

                'remove ", " from last item
                addsql = addsql.TrimEnd(" "c)
                addsql = addsql.TrimEnd(","c)
                strsql = basesql & addsql & " WHERE archivepurgeconfigurationid = " & id
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
        If (Me.cbAutoArchive.Checked <> Convert.ToBoolean(Me.cbAutoArchive.OldText)) Then
            addSQL += " AutoArchiveEnabled = " & Convert.ToInt32(Me.cbAutoArchive.Checked) & ", "
            log += "AutoArchiveEnabled: Old value - " & Me.cbAutoArchive.OldText & ". New value - " & Me.cbAutoArchive.Checked & ".<br>"
            Me.imgAutoArch.Visible = True
        End If
        If (Me.cbAutoPurge.Checked <> Convert.ToBoolean(Me.cbAutoPurge.OldText)) Then
            addSQL += " AutoPurgeEnabled = " & Convert.ToInt32(Me.cbAutoPurge.Checked) & ", "
            log += "AutoPurgeEnabled: Old value - " & Me.cbAutoPurge.OldText & ". New value - " & Me.cbAutoPurge.Checked & ".<br>"
            Me.imgAutoPurge.Visible = True
        End If
        If (Me.cbAutoArchiveToDB.Checked <> Convert.ToBoolean(Me.cbAutoArchiveToDB.OldText)) Then
            addSQL += " AutoArchivetoDBEnabled = " & Convert.ToInt32(Me.cbAutoArchiveToDB.Checked) & ", "
            log += "AutoArchivetoDBEnabled: Old value - " & Me.cbAutoArchiveToDB.OldText & ". New value - " & Me.cbAutoArchiveToDB.Checked & ".<br>"
            Me.imgAutoArchiveToDB.Visible = True
        End If
        If (Me.tbArchiveFolder.Text <> Me.tbArchiveFolder.OldText) Then
            addSQL += " ArchiveFolderPath = '" & Me.tbArchiveFolder.Text & "', "
            log += "ArchiveFolderPath: Old value - " & Me.tbArchiveFolder.OldText & ". New value - " & Me.tbArchiveFolder.Text & ".<br>"
            Me.imgPath.Visible = True
        End If
        If (Me.cbRequiredForPurge.Checked <> Convert.ToBoolean(Me.cbRequiredForPurge.OldText)) Then
            addSQL += " ArchiveRequiredForPurge = " & Convert.ToInt32(Me.cbRequiredForPurge.Checked) & ", "
            log += "ArchiveRequiredForPurge: Old value - " & Me.cbRequiredForPurge.OldText & ". New value - " & Me.cbRequiredForPurge.Checked & ".<br>"
            Me.imgArchReq.Visible = True
        End If
        If (Me.tbArchiveAge.Text <> Me.tbArchiveAge.OldText) Then
            addSQL += " RequiredAgeInDaysForArchive = " & Convert.ToInt32(Me.tbArchiveAge.Text) & ", "
            log += "RequiredAgeInDaysForArchive: Old value - " & Me.tbArchiveAge.OldText & ". New value - " & Me.tbArchiveAge.Text & ".<br>"
            Me.imgArchAge.Visible = True
        End If
        If (Me.tbPurgeAge.Text <> Me.tbPurgeAge.OldText) Then
            addSQL += " RequiredAgeInDaysForPurge = " & Convert.ToInt32(Me.tbPurgeAge.Text) & ", "
            log += "RequiredAgeInDaysForPurge: Old value - " & Me.tbPurgeAge.OldText & ". New value - " & Me.tbPurgeAge.Text & ".<br>"
            Me.imgPurgeAge.Visible = True
        End If

        If (Me.ddlTableName.SelectedItem.Value <> Me.ddlTableName.OldText) Then
            addSQL += " TableName = '" & Me.ddlTableName.SelectedItem.Value & "', "
            log += "TableName: Old value - " & Me.ddlTableName.OldText & ". New value - " & Me.ddlTableName.SelectedItem.Value & ".<br>"
            Me.imgTable.Visible = True
        End If
        If (Me.ddlDTFieldName.SelectedItem.Value <> Me.ddlDTFieldName.OldText) Then
            addSQL += " DateTimeFieldName = '" & Me.ddlDTFieldName.SelectedItem.Value & "', "
            log += "DateTimeFieldName: Old value - " & Me.ddlDTFieldName.OldText & ". New value - " & Me.ddlDTFieldName.SelectedItem.Value & ".<br>"
            Me.imgDT.Visible = True
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
                strSQL = "SELECT TableName,DateTimeFieldName, Description,AutoArchivetoDBEnabled, AutoArchiveEnabled, AutoPurgeEnabled, ArchiveFolderPath, ArchiveRequiredForPurge, RequiredAgeInDaysForArchive, RequiredAgeInDaysForPurge, CONVERT(varchar, LastArchivedRecordDT, 121) 'LastArchivedRecordDT_121' FROM dbo.tblArchivePurgeConfiguration  WHERE ArchivePurgeConfigurationID = " & id

                ds = DA.GetDataSet(strSQL)

                If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                    With ds.Tables(0).Rows(0)

                        Me.tbDescription.Text = .Item("Description").ToString()
                        Me.cbAutoArchive.Checked = Convert.ToBoolean(.Item("AutoArchiveEnabled"))
                        Me.cbAutoPurge.Checked = Convert.ToBoolean(.Item("AutoPurgeEnabled"))
                        Me.tbArchiveFolder.Text = .Item("ArchiveFolderPath").ToString()
                        Me.cbRequiredForPurge.Checked = Convert.ToBoolean(.Item("ArchiveRequiredForPurge"))
                        Me.tbArchiveAge.Text = .Item("RequiredAgeInDaysForArchive").ToString()
                        Me.tbPurgeAge.Text = .Item("RequiredAgeInDaysForPurge").ToString()

                        'store values for validation/change comparison
                        Me.tbDescription.OldText = .Item("Description").ToString()
                        Me.cbAutoArchive.OldText = .Item("AutoArchiveEnabled").ToString()
                        Me.cbAutoPurge.OldText = .Item("AutoPurgeEnabled").ToString()
                        Me.cbAutoArchiveToDB.OldText = .Item("AutoArchivetoDBEnabled").ToString()
                        Me.tbArchiveFolder.OldText = .Item("ArchiveFolderPath").ToString()
                        Me.cbRequiredForPurge.OldText = .Item("ArchiveRequiredForPurge").ToString()
                        Me.tbArchiveAge.OldText = .Item("RequiredAgeInDaysForArchive").ToString()
                        Me.tbPurgeAge.OldText = .Item("RequiredAgeInDaysForPurge").ToString()
                        Me.ddlTableName.OldText = .Item("TableName").ToString()
                        Me.ddlDTFieldName.OldText = .Item("DateTimeFieldName").ToString()

                        'Load table dropdown and DTField dropdown
                        LoadTableNames()
                        LoadDTFieldNames(.Item("TableName").ToString())

                        'select table
                        Dim i As Integer = -1
                        i = Me.ddlTableName.Items.IndexOf(Me.ddlTableName.Items.FindByText(.Item("TableName").ToString()))
                        Me.ddlTableName.SelectedIndex = i

                        If (Me.ddlTableName.SelectedIndex >= 0) Then
                            'select DT Field
                            i = -1
                            i = Me.ddlDTFieldName.Items.IndexOf(Me.ddlDTFieldName.Items.FindByText(.Item("DateTimeFieldName").ToString()))
                            Me.ddlDTFieldName.SelectedIndex = i

                            If (Me.ddlDTFieldName.SelectedIndex >= 0) Then
                                status = True
                            Else
                                Master.eMsg("ERROR:  Miconfigured purge configuration entry.  DT field selection is required!")
                                status = False
                            End If
                        Else
                            Master.eMsg("ERROR:  Miconfigured purge configuration entry.  Table selection is required!")
                            status = False

                        End If


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

    Private Function LoadTableNames() As Boolean 'ByVal p_strTableName As String) As Boolean
        Dim ds As DataSet = Nothing
        Dim strSQL As String
        Dim status As Boolean = False

        strSQL = "SELECT DISTINCT o.name FROM sysobjects o "
        strSQL += "INNER JOIN syscolumns c ON c.id = o.id "
        strSQL += "WHERE LEFT(o.name,3) = 'tbl' AND o.type = 'u' AND c.xtype = 61 AND o.name <> 'tblArchivePurgeConfiguration'"

        Try
            ds = DA.GetDataSet(strSQL)

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                Me.ddlTableName.DataSource = ds
                Me.ddlTableName.DataTextField = "name"
                Me.ddlTableName.DataValueField = "name"
                Me.ddlTableName.DataBind()
                Me.ddlTableName.Items.Insert(0, "Please select")

                status = True
            Else
                Me.ddlTableName.Items.Clear()
                status = False

            End If

            Return status

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Function GetDTFieldNames(ByVal p_strTableName As String) As DataSet
        Try
            Dim ds As DataSet
            Dim strSQL As String

            strSQL = "SELECT c.name FROM syscolumns c "
            strSQL += "INNER JOIN sysobjects o ON c.id = o.id "
            strSQL += "WHERE o.name = '" & p_strTableName & "' AND c.xtype = 61 "
            strSQL += "ORDER BY c.colid"


            ds = DA.GetDataSet(strSQL)
            Return ds

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function LoadDTFieldNames(ByVal p_strTableName As String) As Boolean
        Dim ds As DataSet = Nothing
        Dim status As Boolean = False

        Try

            ds = GetDTFieldNames(p_strTableName)

            Me.ddlDTFieldName.DataSource = ds
            Me.ddlDTFieldName.DataTextField = "name"
            Me.ddlDTFieldName.DataValueField = "name"
            Me.ddlDTFieldName.DataBind()
            Me.ddlDTFieldName.Items.Insert(0, "Please select")

            Return True

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Sub GetListPurgeEntries()
        Dim ds As New DataSet
        Dim strSQL As String

        strSQL = "SELECT Description, ArchivePurgeConfigurationID FROM tblArchivePurgeConfiguration ORDER BY DisplayID"
        Try

            ds = DA.GetDataSet(strSQL)

            'make sure that there is data in the dataset
            If (Not ds Is Nothing) Then
                Me.lbList.DataSource = ds
                Me.lbList.DataTextField = "Description"
                Me.lbList.DataValueField = "ArchivePurgeConfigurationID"
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
        Me.cbAutoArchive.Checked = False
        Me.cbAutoPurge.Checked = False
        Me.tbArchiveFolder.Text = ""
        Me.cbRequiredForPurge.Checked = False
        Me.tbArchiveAge.Text = ""
        Me.tbPurgeAge.Text = ""
        Me.ddlDTFieldName.Items.Clear()
        Me.ddlTableName.Items.Clear()

        ResetDialogNew()
    End Sub

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdDelete)
            Master.Secure(Me.cmdNew)
            Master.Secure(Me.cmdSave)

            If lbList.SelectedIndex < 0 Then
                cmdSave.Enabled = False
                cmdDelete.Enabled = False

                tbDescription.Enabled = False
                cbAutoArchive.Enabled = False
                cbAutoPurge.Enabled = False
                cbAutoArchiveToDB.Enabled = False
                tbArchiveFolder.Enabled = False
                cbRequiredForPurge.Enabled = False
                tbArchiveAge.Enabled = False
                tbPurgeAge.Enabled = False
                ddlTableName.Enabled = False
            Else
                tbDescription.Enabled = True
                cbAutoArchive.Enabled = True
                cbAutoPurge.Enabled = True
                cbAutoArchiveToDB.Enabled = True
                tbArchiveFolder.Enabled = True
                cbRequiredForPurge.Enabled = True
                tbArchiveAge.Enabled = True
                tbPurgeAge.Enabled = True
                ddlTableName.Enabled = True

            End If

            If ddlTableName.SelectedIndex <= 0 Then
                ddlDTFieldName.Enabled = False
            Else
                ddlDTFieldName.Enabled = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "New Dialog"
    Private Sub ResetDialogNew()
        'Reset New Entry dialog
        LoadTableNamesNew()
        If (Me.ddlTableNameNew.Items.Count > 0) Then
            Me.ddlTableNameNew.SelectedIndex = 0
            LoadDTFieldNamesNew(Me.ddlTableNameNew.Items(0).Value)
        End If

        Me.tbDescriptionNew.Text = ""
        Me.cbAutoArchiveNew.Checked = False
        Me.cbAutoPurgeNew.Checked = False
        Me.tbArchiveFolderNew.Text = ""
        Me.cbRequiredForPurgeNew.Checked = False
        Me.tbArchiveAgeNew.Text = ""
        Me.tbPurgeAgeNew.Text = ""

    End Sub

    Private Sub ddlTableNameNew_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTableNameNew.SelectedIndexChanged
        LoadDTFieldNamesNew(Me.ddlTableNameNew.SelectedItem.Value)
    End Sub

    Private Function LoadTableNamesNew() As Boolean
        Dim ds As DataSet = Nothing
        Dim strSQL As String

        strSQL = "SELECT DISTINCT o.name FROM sysobjects o "
        strSQL += "INNER JOIN syscolumns c ON c.id = o.id "
        strSQL += "WHERE LEFT(o.name,3) = 'tbl' AND o.type = 'u' AND c.xtype = 61 AND o.name <> 'tblArchivePurgeConfiguration'"
        'strSQL += "AND o.name NOT IN (SELECT DISTINCT Description FROM tblArchivePurgeConfiguration)"

        Try
            ds = DA.GetDataSet(strSQL)

            Me.ddlTableNameNew.DataSource = ds
            Me.ddlTableNameNew.DataTextField = "name"
            Me.ddlTableNameNew.DataValueField = "name"
            Me.ddlTableNameNew.DataBind()
            Me.ddlTableNameNew.Items.Insert(0, "Please select")

            Return True

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Function LoadDTFieldNamesNew(ByVal p_strTableName As String) As Boolean
        Try
            Dim ds As DataSet = Nothing

            ds = GetDTFieldNames(p_strTableName)

            Me.ddlDTFieldNameNew.DataSource = ds
            Me.ddlDTFieldNameNew.DataTextField = "name"
            Me.ddlDTFieldNameNew.DataValueField = "name"
            Me.ddlDTFieldNameNew.DataBind()

            Me.ddlDTFieldNameNew.Items.Insert(0, "Please select")
            Return True

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Public Function InsertArchivePurgeConfiguration() As Boolean

        Dim colParameters As New Collections.Generic.List(Of System.Data.SqlClient.SqlParameter)
        Dim colOutput As Collections.Generic.List(Of System.Data.SqlClient.SqlParameter)

        Try

            '@Description varchar(80),
            '@AutoArchiveEnabled bit,
            '@AutoPurgeEnabled bit,
            '@AutoArchivetoDBEnabled bit,
            '@ArchiveFolderPath varchar(500),
            '@ArchiveRequiredForPurge bit,
            '@RequiredAgeInDaysForArchive int,
            '@RequiredAgeInDaysForPurge int,
            '@TableName varchar(500),
            '@DateTimeFieldName varchar(500),
            '@Status  varchar(80)='' OUT,
            '@ErrorMsg varchar(80) ='' OUT

            colParameters.Add(New SqlClient.SqlParameter("@Description", Me.tbDescriptionNew.Text.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@AutoArchiveEnabled", Me.cbAutoArchiveNew.Checked.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@AutoPurgeEnabled", Me.cbAutoPurgeNew.Checked.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@AutoArchivetoDBEnabled", Me.cbAutoArchiveDBNew.Checked.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@ArchiveFolderPath", Me.tbArchiveFolderNew.Text.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@ArchiveRequiredForPurge", Me.cbRequiredForPurgeNew.Checked.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@RequiredAgeInDaysForArchive", Me.tbArchiveAgeNew.Text.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@RequiredAgeInDaysForPurge", Me.tbPurgeAgeNew.Text.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@TableName", Me.ddlTableNameNew.Text.ToString()))
            colParameters.Add(New SqlClient.SqlParameter("@DateTimeFieldName", Me.ddlDTFieldNameNew.Text.ToString()))

            colOutput = DA.ExecSP("procInsertArchivePurgeConfiguration", colParameters)


            Return True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function
#End Region
End Class