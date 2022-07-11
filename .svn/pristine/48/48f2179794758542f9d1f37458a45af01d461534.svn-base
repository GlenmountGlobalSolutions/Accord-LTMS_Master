Imports System.Data.SqlClient

Public Class StationTaskConfiguration
    Inherits System.Web.UI.Page

#Region "Globals"

    Protected WithEvents WebInputBox As GGS.WebInputBox
    Protected WithEvents WebDropDownList As GGS.WebDropDownList 'holds reference to the parameter in each template
    Protected WithEvents WebCheckBox As GGS.WebCheckBox
    Protected WithEvents imgCheck As System.Web.UI.WebControls.Image

    Private sqlUpdateParams As New ArrayList    'putting my update commands here and when save is fired I execute them
    Private preSQL As New ArrayList
    Private InputCaption As New ArrayList    'caption asscoiated with control
    Private OldText As New ArrayList         'old value of ctrl
    Private NewText As New ArrayList         'new posted value of ctrl

#End Region

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not (Page.IsPostBack)) Then
                ddlLineNumber_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub StationTaskConfiguration_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLineNumber_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLineNumber.SelectedIndexChanged
        Try
            ddlStyleGroups_Bind()
            ddlStationsList_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStyleGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStyleGroups.SelectedIndexChanged
        Try
            dataListTaskConfig_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStations.SelectedIndexChanged
        Try
            dataListTaskConfig_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCopy_Click(sender As Object, e As System.EventArgs) Handles cmdCopy.Click
        Try
            CopyTaskConfiguration()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim s As String
            Dim StatusMessage As String = ""
            Dim x As Int32
            x = 0

            If ((ddlStations.SelectedIndex < 1) Or (ddlStyleGroups.SelectedIndex < 1)) Then
                Master.tMsg("Alert", "There is nothing to save. No Station or Style Group is selected", True, "Red")
            Else
                For Each s In sqlUpdateParams
                    DA.ExecSQL(s)

                    If StatusMessage <> "" Then
                        StatusMessage = StatusMessage & vbCrLf
                    End If
                    StatusMessage = StatusMessage & "   Station Parameter: " & InputCaption(x).ToString() & " " & _
                                                    "for StationID: " & ddlStations.SelectedItem.Text & " " & _
                                                    "was changed from : " & OldText(x).ToString() & " to: " & NewText(x).ToString
                    x = x + 1
                Next

                Select Case x
                    Case 0
                        Master.Msg = "No parameters were modified; thus, none were saved!"
                    Case 1
                        Master.Msg = "One parameter was saved!" & vbCrLf & StatusMessage
                    Case Is > 1
                        Master.Msg = x & " Parameters were saved!" & vbCrLf & StatusMessage
                End Select
            End If

            ' Update the Last Modified Date/Time in the Application Parameters table.
            If (x > 0) Then
                UpdateTransactionParameters()
            End If

            dataListTaskConfig_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        Try
            Dim bEnabled As Boolean = False

            Master.Secure(cmdCopy)
            Master.Secure(cmdSave)

            If (ddlStyleGroups.SelectedIndex < 1) Or (ddlStations.SelectedIndex < 1) Then
                cmdCopy.Enabled = False
                cmdSave.Enabled = False
            End If

            bEnabled = (ddlLineNumber.SelectedIndex > 0)
            ddlStyleGroups.Enabled = bEnabled
            ddlStations.Enabled = bEnabled

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLineNumber_Bind()
        Try
            ' There are no Aux Tasks allowed on the Shipping line, so make sure Line 300 does not show up in the list.
            Dim ds As DataSet = DA.GetDataSet("SELECT [LineID], [LineName] FROM [dbo].[tblSGLines] WHERE [LineName] NOT LIKE '%300%'")

            With ddlLineNumber
                .DataSource = ds
                .DataTextField = "LineName"
                .DataValueField = "LineID"
                .DataBind()
                .Items.Insert(0, New ListItem("Choose a Line", "0"))
            End With

            With ddlLineNumber_Copy
                .DataSource = ds.Copy()
                .DataTextField = "LineName"
                .DataValueField = "LineID"
                .DataBind()
                .Items.Insert(0, New ListItem("Choose a Line", "0"))
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStyleGroups_Bind()
        Try
            With ddlStyleGroups
                .Items.Clear()

                If (ddlLineNumber.SelectedIndex > 0) Then
                    .DataSource = DA.GetDataSet("SELECT [StyleGroupID], [StyleGroupName] FROM [dbo].[tblSGStyleGroups] WHERE [LineID] = " & ddlLineNumber.SelectedValue & " ORDER BY PLCArrayOrder")
                    .DataTextField = "StyleGroupName"
                    .DataValueField = "StyleGroupID"
                    .DataBind()
                    .Items.Insert(0, "Choose a Style Group")
                End If
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStationsList_Bind()
        Try
            With ddlStations
                .Items.Clear()

                If (ddlLineNumber.SelectedIndex > 0) Then
                    .DataSource = DA.GetDataSet("SELECT [StationID], [Description] FROM [dbo].[tblStations] WHERE [LineID] = " & ddlLineNumber.SelectedValue & " ORDER BY DisplayID")
                    .DataTextField = "Description"
                    .DataValueField = "StationID"
                    .DataBind()
                    .Items.Insert(0, "Choose a Station")
                End If
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dataListTaskConfig_Bind()
        Try
            With dataListTaskConfig
                .DataSource = Nothing

                If (ddlStyleGroups.SelectedIndex > 0) And (ddlStations.SelectedIndex > 0) Then
                    Dim TheSQL As String =
                                "SELECT  pt.TaskParamTypeID,  " &
                                        "pt.Description, " &
                                        "p.StyleGroupID, " &
                                        "p.StationID, " &
                                        "p.TaskParamValue, " &
                                        "pt.ParameterTypeID, " &
                                        "pt.ParameterListID, " &
                                        "p.LT_Req " &
                               "FROM tblSGTaskParameterTypes pt " &
                               "INNER JOIN tblSGTaskParameters p " &
                               "ON p.TaskParamTypeID = pt.TaskParamTypeID " &
                               "WHERE (p.StationID = '" & ddlStations.SelectedValue & "') " &
                               "AND (p.StyleGroupID = '" & ddlStyleGroups.SelectedValue & "') " &
                               "ORDER BY pt.DisplayID"

                    .DataSource = DA.GetDataSet(TheSQL)
                End If

                .DataBind()
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Function ObjectVisibilityInTemplate(ByVal e As Object) As Object
        Try
            Dim StationParameterValue As String
            Dim strInputCaption As String
            Dim SQLUpdateCommandText As String

            Dim dlItem As DataListItem = CType(e, DataListItem)
            Dim dlLblDesc As Label = CType(dlItem.FindControl("dllblDesc"), Label)                  'getting ref to description label
            Dim dbDataRVProdParam As System.Data.DataRowView = CType(dlItem.DataItem, DataRowView)  'getting ref to data row

            WebDropDownList = CType(dlItem.FindControl("dlwddlTaskParam"), GGS.WebDropDownList)     'getting ref to drop box
            WebInputBox = CType(dlItem.FindControl("dlwibTaskParam"), GGS.WebInputBox)              'getting ref to input box
            imgCheck = CType(dlItem.FindControl("dlImgCheck"), Web.UI.WebControls.Image)            'getting ref to Check image

            strInputCaption = dbDataRVProdParam.Item("Description").ToString()

            StationParameterValue = dbDataRVProdParam.Item("TaskParamValue").ToString()

            dlLblDesc.ToolTip = "ParameterListID = " & dbDataRVProdParam.Item("ParameterListID").ToString() & ", " & _
                                "TaskParamTypeID: " & dbDataRVProdParam.Item("TaskParamTypeID").ToString


            SQLUpdateCommandText = "UPDATE tblSGTaskParameters " &
                                   "SET TaskParamValue = '6933A6C7-80FC-4b37-907E-26FCE24DD7EE' " &
                                   "WHERE StationID = '" & dbDataRVProdParam.Item("StationID").ToString() & "' " &
                                   "AND StyleGroupID = " & dbDataRVProdParam.Item("StyleGroupID").ToString() & " " &
                                   "AND TaskParamTypeID = '" & dbDataRVProdParam.Item("TaskParamTypeID").ToString() & "' "

            'setting what to display box or dropdown
            If (dbDataRVProdParam.Item("ParameterListID").ToString() = "") Then
                'box visible=true, dd visible=false
                WebDropDownList.Visible = False
                WebInputBox.Visible = True
                WebInputBox.SQLText = SQLUpdateCommandText
                WebInputBox.InputCaption = strInputCaption
                WebInputBox.OldText = StationParameterValue
            Else
                WebDropDownList.Visible = True
                WebInputBox.Visible = False

                'binding databox to source
                TaskParameter_Bind(dbDataRVProdParam.Item("ParameterListID").ToString(), dbDataRVProdParam.Item("TaskParamValue").ToString().Trim)
                WebDropDownList.SQLText = SQLUpdateCommandText
                WebDropDownList.InputCaption = strInputCaption
                WebDropDownList.OldText = StationParameterValue
            End If

            ' Setting visibility of the Check image.
            Dim k As String
            imgCheck.Visible = False
            For Each k In preSQL
                If (String.CompareOrdinal(Trim(k), Trim(SQLUpdateCommandText)) = 0) Then
                    imgCheck.Visible = True
                End If
            Next

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return (Nothing)
    End Function

    Public Function ObjectVisibilityInTemplate2(ByVal e As Object) As Object
        Try
            Dim StationParameterValue As String
            Dim strInputCaption As String
            Dim SQLUpdateCommandText As String

            Dim dlItem As DataListItem = CType(e, DataListItem)
            Dim dlLblDesc As Label = CType(dlItem.FindControl("dllblDesc"), Label)                  'getting ref to description label
            Dim dbDataRVProdParam As System.Data.DataRowView = CType(dlItem.DataItem, DataRowView)  'getting ref to data row

            'imgCheck = CType(dlItem.FindControl("dlImgCheck"), Web.UI.WebControls.Image)            'getting ref to Check image

            WebCheckBox = CType(dlItem.FindControl("cbLTReq"), GGS.WebCheckBox)       '''          'getting ref to check box

            strInputCaption = dbDataRVProdParam.Item("Description").ToString()

            StationParameterValue = dbDataRVProdParam.Item("LT_Req").ToString()

            'dlLblDesc.ToolTip = "ParameterListID = " & dbDataRVProdParam.Item("ParameterListID").ToString() & ", " &
            '                    "TaskParamTypeID: " & dbDataRVProdParam.Item("TaskParamTypeID").ToString


            SQLUpdateCommandText = "UPDATE tblSGTaskParameters " &
                                   "SET LT_Req = '6933A6C7-80FC-4b37-907E-26FCE24DD7FG' " &
                                   "WHERE StationID = '" & dbDataRVProdParam.Item("StationID").ToString() & "' " &
                                   "AND StyleGroupID = " & dbDataRVProdParam.Item("StyleGroupID").ToString() & " " &
                                   "AND TaskParamTypeID = '" & dbDataRVProdParam.Item("TaskParamTypeID").ToString() & "' "

            'setting what to display box or dropdown
            If Not (dlLblDesc.Text.ToUpper.Contains("POKE")) Then
                WebCheckBox.Visible = False
            Else
                WebCheckBox.Visible = True

                'binding databox to source
                WebCheckBox.SQLText = SQLUpdateCommandText
                WebCheckBox.InputCaption = strInputCaption
                WebCheckBox.OldText = StationParameterValue
            End If

            ''''Setting visibility of the Check image.
            '''Dim k As String
            '''imgCheck.Visible = False
            '''For Each k In preSQL
            '''    If (String.CompareOrdinal(Trim(k), Trim(SQLUpdateCommandText)) = 0) Then
            '''        imgCheck.Visible = True
            '''    End If
            '''Next

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return (Nothing)
    End Function

    Private Sub TaskParameter_Bind(ByRef ParameterListID As String, ByRef ParamValue As String)
        Dim liZ As New ListItem
        Dim strSql As String

        strSql = "SELECT v.ParameterListValue " &
                 "FROM dbo.tblParameterLists l " &
                 "INNER JOIN dbo.tblParameterListValues v " &
                 "ON l.ParameterListID = v.ParameterListID " &
                 "WHERE l.ParameterListID = '" & ParameterListID & "'"

        WebDropDownList.DataSource = DA.GetDataSet(strSql)
        WebDropDownList.DataTextField = "ParameterListValue"
        WebDropDownList.DataValueField = "ParameterListValue"
        WebDropDownList.DataBind()
        liZ.Text = ParamValue
        WebDropDownList.Items.Insert(0, liZ)

        If Not ParamValue = "" Then           'if there is blank one already do not insert
            WebDropDownList.Items.Insert(1, New ListItem)
        End If

    End Sub

    Public Sub dlwddlTaskParam_IndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim x As String
            Dim WebDropDownList As GGS.WebDropDownList = CType(sender, GGS.WebDropDownList)    'getting ref to control


            If (WebDropDownList.SelectedItem.Value = "{DFFCEE3D-763E-4f8d-A22B-E3A074FF1541}") Then
                WebDropDownList.SelectedItem.Value = ""
            End If
            x = WebDropDownList.SQLText
            preSQL.Add(x)
            x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", WebDropDownList.SelectedItem.ToString())
            sqlUpdateParams.Add(x)

            InputCaption.Add(WebDropDownList.InputCaption)
            OldText.Add(WebDropDownList.OldText)
            NewText.Add(WebDropDownList.SelectedItem.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub dlwibTaskParam_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim x As String
            Dim WebInputBox As GGS.WebInputBox = CType(sender, GGS.WebInputBox)    'getting ref to control

            x = WebInputBox.SQLText
            preSQL.Add(x)
            If (Not x Is Nothing) Then
                If (x.IndexOf("D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD") > 0) Then
                    WebInputBox.Text = FormatForSaveOrDisplay(WebInputBox.Text, True)
                    x = x.Replace("D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD", WebInputBox.Text)
                Else
                    x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", WebInputBox.Text)
                End If
                sqlUpdateParams.Add(x)
            End If

            InputCaption.Add(WebInputBox.InputCaption)
            OldText.Add(WebInputBox.OldText)
            NewText.Add(WebInputBox.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub cbLTReq_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim x As String
            Dim WebCheckBox As GGS.WebCheckBox = CType(sender, GGS.WebCheckBox)    'getting ref to control

            x = WebCheckBox.SQLText
            preSQL.Add(x)
            x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7FG", WebCheckBox.Checked.ToString())
            sqlUpdateParams.Add(x)

            InputCaption.Add(WebCheckBox.InputCaption)
            OldText.Add(WebCheckBox.OldText)
            NewText.Add(WebCheckBox.Checked)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function FormatForSaveOrDisplay(ByRef str As String, ByRef bfrmt As Boolean) As String
        Dim dschar As DataSet
        Dim dr As DataRow

        dschar = DA.GetDataSet("SELECT * FROM tblCharacterSwap")
        If (bfrmt = False) Then       'display
            For Each dr In dschar.Tables(0).Rows
                str = str.Replace(dr(1).ToString, dr(0).ToString())
            Next
            Return str

        Else          'save
            For Each dr In dschar.Tables(0).Rows
                str = str.Replace(dr(0).ToString, dr(1).ToString())
            Next
            Return str
        End If
    End Function

    Private Sub CopyTaskConfiguration()
        Try
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As SqlParameter

            If ((ddlStations.SelectedIndex < 1) Or (ddlStyleGroups.SelectedIndex < 1)) Then
                Master.tMsg("Alert", "Please select the Station/Style Group you would like to copy!", True, "Red")
            Else
                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@FromStyleGroupID", SqlDbType.Int)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = CInt(ddlStyleGroups.SelectedValue)
                colParms.Add(prmNext)
                '------------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@FromStationID", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = ddlStations.SelectedValue
                colParms.Add(prmNext)
                '------------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ToStyleGroupID", SqlDbType.Int)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = CInt(hidDDLStyleGroup.Value)
                colParms.Add(prmNext)
                '------------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ToStationID", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = hidDDLStation.Value
                colParms.Add(prmNext)
                '------------------------------------------------------------
                DA.ExecSP("procSGCopyTask", colParms)

                Master.tMsg("Copy", "Tasks for Station: " & ddlStations.SelectedItem.ToString() & " and Style Group: " & ddlStyleGroups.SelectedItem.ToString() & _
                            " were copied to Station:" & hidDDLStation_Name.Value.Trim() & " and Style Group: " & hidDDLStyleGroup_Name.Value.Trim())

                ' Update the Last Modified Date/Time in the Application Parameters table.
                UpdateTransactionParameters()

                ddlStations.SelectedIndex = ddlStations.Items.IndexOf(ddlStations.Items.FindByValue(hidDDLStation.Value))
                ddlStyleGroups.SelectedIndex = ddlStyleGroups.Items.IndexOf(ddlStyleGroups.Items.FindByValue(hidDDLStyleGroup.Value))

                dataListTaskConfig_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub UpdateTransactionParameters()

        Try
            Dim strLineNumber As String = ""

            strLineNumber = CStr(CInt(ddlLineNumber.SelectedValue.ToString) * 100)

            BizLayer.SetRecipeSavedDT("1", "8", strLineNumber)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub StationTaskConfiguration_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

#End Region

End Class