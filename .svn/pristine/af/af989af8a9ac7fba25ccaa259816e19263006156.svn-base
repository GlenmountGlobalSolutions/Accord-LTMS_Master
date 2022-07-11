Imports System.Data.SqlClient

Public Class StationToolConfiguration
    Inherits System.Web.UI.Page

#Region "Globals"

    Protected WithEvents WebInputBox As GGS.WebInputBox
    Protected WithEvents WebDropDownList As GGS.WebDropDownList 'holds reference to the parameter in each template
    Protected WithEvents imgCheck As System.Web.UI.WebControls.Image

    Private sqlUpdateParams As New ArrayList    'putting my update commands here and when save is fired I execute them
    Private preSQL As New ArrayList
    Private InputCaption As New ArrayList    'caption associated with control
    Private OldText As New ArrayList         'old value of ctrl
    Private NewText As New ArrayList         'new posted value of ctrl

#End Region

#Region "Event Handlers"

    Private Sub StationToolConfiguration_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If (Not (Page.IsPostBack)) Then
                ddlLineNumber_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub StationToolConfiguration_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLineNumber_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLineNumber.SelectedIndexChanged
        Try
            ddlStyleGroupsList_Bind()
            ddlStationsList_Bind()
            dataListToolConfig_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStations.SelectedIndexChanged
        Try
            dataListToolConfig_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStyleGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStyleGroups.SelectedIndexChanged
        Try
            dataListToolConfig_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCopy_Click(sender As Object, e As System.EventArgs) Handles cmdCopy.Click
        Try
            CopyToolConfiguration()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCopyTool1Names_Click(sender As Object, e As System.EventArgs) Handles cmdCopyTool1Names.Click
        Try
            Dim str As String
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As SqlParameter

            If ((ddlStyleGroups.SelectedIndex < 1) Or (ddlStations.SelectedIndex < 1)) Then
                Master.tMsg("Alert", "Please select the Station/Style Group you would like to copy!", True, "Red")
            Else

                str = ddlStations.SelectedItem.Text

                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@StyleGroupID", SqlDbType.Int)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = ddlStyleGroups.SelectedValue
                colParms.Add(prmNext)
                '------------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@StationID", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = ddlStations.SelectedValue
                colParms.Add(prmNext)
                '------------------------------------------------------------
                DA.ExecSP("procSGCopyTool1Names", colParms)

                Master.tMsg("Copy", "Torque Tool 1 Names for Station: " & ddlStations.SelectedItem.ToString() & " and Style Group: " & ddlStyleGroups.SelectedItem.ToString() & " were copied to all Stations on the current line.")

                ' Update the Last Modified Date/Time in the Application Parameters table.
                UpdateTransactionParameters()

                dataListToolConfig_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCopyTool2Names_Click(sender As Object, e As System.EventArgs) Handles cmdCopyTool2Names.Click
        Try
            Dim str As String
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As SqlParameter

            If ((ddlStyleGroups.SelectedIndex < 1) Or (ddlStations.SelectedIndex < 1)) Then
                Master.tMsg("Alert", "Please select the Station/Style Group you would like to copy!", True, "Red")
            Else

                str = ddlStations.SelectedItem.Text

                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@StyleGroupID", SqlDbType.Int)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = ddlStyleGroups.SelectedValue
                colParms.Add(prmNext)
                '------------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@StationID", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = ddlStations.SelectedValue
                colParms.Add(prmNext)
                '------------------------------------------------------------
                DA.ExecSP("procSGCopyTool2Names", colParms)

                Master.tMsg("Copy", "Torque Tool 2 Names for Station: " & ddlStations.SelectedItem.ToString() & " and Style Group: " & ddlStyleGroups.SelectedItem.ToString() & " were copied to all Stations on the current line.")

                ' Update the Last Modified Date/Time in the Application Parameters table.
                UpdateTransactionParameters()

                dataListToolConfig_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim s As String
            Dim x As Int32
            x = 0

            If ((ddlStations.SelectedIndex < 1) Or (ddlStyleGroups.SelectedIndex < 1)) Then
                Master.tMsg("Alert", "There is nothing to save. No Station or Style Group is selected", True, "Red")
            Else
                For Each s In sqlUpdateParams
                    DA.ExecSQL(s)
                    Master.tMsg("Save", "Station Parameter: " & InputCaption(x).ToString() & _
                                        " for StationID: " & ddlStations.SelectedItem.Text & _
                                        " was changed from : " & OldText(x).ToString() & " to: " & NewText(x).ToString())
                    x = x + 1
                Next

                Select Case x
                    Case 0
                        Master.Msg = "No parameters were modified; thus, none were saved!"
                    Case 1
                        Master.Msg = x & " Parameter is saved!"
                    Case Is > 1
                        Master.Msg = x & " Parameters were saved!"
                End Select

                ' Update the Last Modified Date/Time in the Application Parameters table.
                If (x > 0) Then
                    UpdateTransactionParameters()
                End If

                'reset sqlUpdateParams
                sqlUpdateParams.Clear()
                dataListToolConfig_Bind()
            End If

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
            Master.Secure(cmdCopyTool1Names)
            Master.Secure(cmdCopyTool2Names)

            If (ddlStyleGroups.SelectedIndex < 1) Or (ddlStations.SelectedIndex < 1) Then
                cmdCopy.Enabled = False
                cmdSave.Enabled = False
                cmdCopyTool1Names.Enabled = False
                cmdCopyTool2Names.Enabled = False
            End If

            If (ddlLineNumber.SelectedIndex > 0) Then
                bEnabled = True
            End If

            ddlStyleGroups.Enabled = bEnabled
            ddlStations.Enabled = bEnabled

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLineNumber_Bind()
        Try
            ' There are no Tools allowed on the Shipping line, so make sure Line 300 does not show up in the list.
            Dim ds As DataSet = DA.GetDataSet("SELECT [LineID], [LineName] FROM [dbo].[tblSGLines] WHERE [LineName] NOT LIKE '%300%'")

            With ddlLineNumber
                .DataSource = ds
                .DataTextField = "LineName"
                .DataValueField = "LineID"
                .DataBind()
                .Items.Insert(0, "Choose a Line")
            End With

            With ddlLineNumber_Copy
                .DataSource = ds.Copy()
                .DataTextField = "LineName"
                .DataValueField = "LineID"
                .DataBind()
                .Items.Insert(0, "Choose a Line")
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStyleGroupsList_Bind()
        Try
            With ddlStyleGroups
                .Items.Clear()

                If (ddlLineNumber.SelectedIndex > 0) Then
                    .DataSource = DA.GetDataSet("SELECT StyleGroupID, StyleGroupName FROM dbo.tblSGStyleGroups WHERE LineID = " & ddlLineNumber.SelectedValue & " ORDER BY PLCArrayOrder")
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

    Private Sub dataListToolConfig_Bind()
        Try
            With dlStationToolConfig
                .DataSource = Nothing

                If (ddlStyleGroups.SelectedIndex > 0) And (ddlStations.SelectedIndex > 0) Then
                    Dim TheSQL As String = _
                            "SELECT  t.ToolParamTypeID,  " & _
                                    "t.Description, " & _
                                    "p.StyleGroupID, " & _
                                    "p.StationID, " & _
                                    "p.ToolParamValue, " & _
                                    "t.ParameterTypeID, " & _
                                    "t.ParameterListID " & _
                            "FROM tblSGToolParameterTypes t " & _
                            "INNER JOIN tblSGToolParameters p " & _
                            "ON p.ToolParamTypeID = t.ToolParamTypeID " & _
                            "WHERE p.StationID = '" & ddlStations.SelectedValue & "' " & _
                            "AND p.StyleGroupID = '" & ddlStyleGroups.SelectedValue & "' " & _
                            "ORDER BY t.DisplayID"

                    .DataSource = DA.GetDataSet(TheSQL)
                End If

                .DataBind()
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub dlwddlToolParam_IndexChanged(sender As Object, e As System.EventArgs)
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

    Public Sub dlwibToolParam_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
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

    Public Function ObjectVisibilityInTemplate(ByVal e As Object) As Object
        Try
            Dim StationParameterValue As String
            Dim strInputCaption As String
            Dim SQLUpdateCommandText As String

            Dim dlItem As DataListItem = CType(e, DataListItem)                                     'getting ref to datalist item
            Dim dataRowViewProdParam As System.Data.DataRowView = CType(dlItem.DataItem, DataRowView)  'getting ref to data row

            Dim dlLblDesc As Label = CType(dlItem.FindControl("dllblDesc"), Label)                  'getting ref to Description label
            WebDropDownList = CType(dlItem.FindControl("dlwddlToolParam"), GGS.WebDropDownList)     'getting ref to drop box
            WebInputBox = CType(dlItem.FindControl("dlwibToolParam"), GGS.WebInputBox)              'getting ref to input box
            imgCheck = CType(dlItem.FindControl("dlImgCheck"), Web.UI.WebControls.Image)            'getting ref to Check image

            strInputCaption = dataRowViewProdParam.Item("Description").ToString()
            StationParameterValue = dataRowViewProdParam.Item("ToolParamValue").ToString()

            dlLblDesc.ToolTip = "ParameterListID = " & dataRowViewProdParam.Item("ParameterListID").ToString() & ", " & _
                                "ParameterTypeID: " & dataRowViewProdParam.Item("ParameterTypeID").ToString() & ", " & _
                                "ToolParamTypeID: " & dataRowViewProdParam.Item("ToolParamTypeID").ToString

            SQLUpdateCommandText = "UPDATE tblSGToolParameters " & _
                                   "SET ToolParamValue = '6933A6C7-80FC-4b37-907E-26FCE24DD7EE' " & _
                                   "WHERE StationID = '" & dataRowViewProdParam.Item("StationID").ToString() & "' " & _
                                   "AND StyleGroupID = " & dataRowViewProdParam.Item("StyleGroupID").ToString() & " " & _
                                   "AND ToolParamTypeID = '" & dataRowViewProdParam.Item("ToolParamTypeID").ToString() & "' "

            'setting what to display box or dropdown
            If (dataRowViewProdParam.Item("ParameterListID").ToString() = "") Then
                'box visible=true, dd visible=false
                WebDropDownList.Visible = False
                WebInputBox.Visible = True

                WebInputBox.SQLText = SQLUpdateCommandText
                WebInputBox.InputCaption = strInputCaption
                WebInputBox.OldText = StationParameterValue
            Else
                WebDropDownList.Visible = True
                WebInputBox.Visible = False

                ToolParameter_Bind(dataRowViewProdParam.Item("ParameterListID").ToString(), _
                                   dataRowViewProdParam.Item("ToolParamValue").ToString())            'binding databox to source

                WebDropDownList.SQLText = SQLUpdateCommandText
                WebDropDownList.InputCaption = strInputCaption
                WebDropDownList.OldText = StationParameterValue
            End If

            ' Set visibility of Check image.
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

    Private Sub ToolParameter_Bind(ByRef ParameterListID As String, ByRef ParamValue As String)
        Dim liZ As New ListItem
        Dim strSql As String

        strSql = "SELECT v.ParameterListValue " & _
                     "FROM tblParameterLists l " & _
                     "INNER JOIN tblParameterListValues v " & _
                     "ON l.ParameterListID = v.ParameterListID " & _
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

    Private Sub CopyToolConfiguration()
        Try
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As SqlParameter

            If ((ddlStations.SelectedIndex <= 0) Or (ddlStyleGroups.SelectedIndex <= 0)) Then
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
                DA.ExecSP("procSGCopyTool", colParms)

                Master.tMsg("Copy", "Tool configuration for Station: " & ddlStations.SelectedItem.ToString() & " and Style Group: " & ddlStyleGroups.SelectedItem.ToString() & _
                            " were copied to Station:" & hidDDLStation_Name.Value.Trim() & " and Style Group: " & hidDDLStyleGroup_Name.Value.Trim())

                ' Update the Last Modified Date/Time in the Application Parameters table.
                UpdateTransactionParameters()

                ddlStations.SelectedIndex = ddlStations.Items.IndexOf(ddlStations.Items.FindByValue(hidDDLStation.Value))
                ddlStyleGroups.SelectedIndex = ddlStyleGroups.Items.IndexOf(ddlStyleGroups.Items.FindByValue(hidDDLStyleGroup.Value))

                dataListToolConfig_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub UpdateTransactionParameters()

        Try
            Dim strLineNumber As String = ""

            strLineNumber = CStr(CInt(ddlLineNumber.SelectedValue.ToString) * 100)

            BizLayer.SetRecipeSavedDT("6", "12", strLineNumber)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

#End Region

End Class