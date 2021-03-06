Imports System.Data.SqlClient

Public Class StationConfiguration
    Inherits System.Web.UI.Page

#Region "Globals"

    Private bCapChanged As Boolean   'flag that monitors if caption was changeded and if so display check box

    Private preSQL As New ArrayList
    Private sqlUpdateParams As New ArrayList  'putting my update commands here and when save is fired I execute them
    Private InputCaption As New ArrayList        'caption asscoiated with control
    Private OldText As New ArrayList             'old value of ctrl
    Private NewText As New ArrayList             'new posted value of ctrl

    Protected WithEvents wddlWebDropDownList As GGS.WebDropDownList 'holds reference to the parameter in each template
    Protected WithEvents xtxtWebInputBox As GGS.WebInputBox
    Protected WithEvents imgCheck As System.Web.UI.WebControls.Image

#End Region

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                BindLines()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdMoveUp_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdMoveUp.Click
        Try
            Dim dsDisplayID As New DataSet

            If (Me.lbStations.SelectedIndex < 0) Then
                Master.Msg = "Cannot move item up: Station has not been selected"
            Else

                If (Me.lbStations.SelectedIndex > 0) Then
                    Dim selectedStationID As Integer = CInt(Me.lbStations.SelectedItem.Value)
                    Dim selectedIndex As Integer = Me.lbStations.SelectedIndex

                    'get displayID of currently selected item
                    Dim dispID As Integer = 0
                    dsDisplayID.Reset()
                    dsDisplayID = DA.GetDataSet("SELECT TOP 1 DisplayID FROM tblStations WHERE StationID=" & selectedStationID)
                    dispID = Convert.ToInt32(dsDisplayID.Tables(0).DefaultView.Table.Rows(0)(0))

                    'assing new displayID to item directly above currently selected item
                    DA.ExecSQL("UPDATE tblStations SET DisplayID = " & dispID & " WHERE StationID=" & Me.lbStations.Items(selectedIndex - 1).Value)

                    'assign new displayID to currently selected item
                    DA.ExecSQL("UPDATE tblStations SET DisplayID = " & (dispID - 1) & " WHERE StationID=" & selectedStationID)

                    'reload listbox
                    lbStationsList_Bind()

                    'select item
                    Me.lbStations.SelectedIndex = (selectedIndex - 1)

                Else             'item selected is already at the top of the list
                    Master.Msg = "Cannot move item up."
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdMoveDown_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdMoveDown.Click
        Try
            Dim dsDisplayID As New DataSet

            If (Me.lbStations.SelectedIndex < 0) Then
                Master.Msg = "Cannot move item down: Station has not been selected"
            Else

                If (Me.lbStations.SelectedIndex < (Me.lbStations.Items.Count - 1)) Then
                    Dim selectedStationID As Integer = CInt(Me.lbStations.SelectedItem.Value)
                    Dim selectedIndex As Integer = Me.lbStations.SelectedIndex

                    'get displayID of currently selected item
                    Dim dispID As Integer = 0
                    dsDisplayID = DA.GetDataSet("SELECT TOP 1 DisplayID FROM tblStations WHERE StationID=" & selectedStationID)
                    dispID = Convert.ToInt32(dsDisplayID.Tables(0).DefaultView.Table.Rows(0)(0))

                    'assing new displayID to item directly below currently selected item
                    DA.ExecSQL("UPDATE tblStations SET DisplayID = " & dispID & " WHERE StationID=" & Me.lbStations.Items(selectedIndex + 1).Value)

                    'assign new displayID to currently selected item
                    DA.ExecSQL("UPDATE tblStations SET DisplayID = " & (dispID + 1) & " WHERE StationID=" & selectedStationID)

                    'reload listbox
                    lbStationsList_Bind()

                    'select item
                    Me.lbStations.SelectedIndex = (selectedIndex + 1)

                Else             'item selected is already at the bottom of the list
                    Master.Msg = "Cannot move item down."
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ddlLineNumber_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlLineNumber.SelectedIndexChanged
        Try
            lbStationsList_Bind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub lbStations_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbStations.SelectedIndexChanged
        Try
            dlStationConfig_Bind()
            Master.Msg = "Parameters for station: " & lbStations.SelectedItem.ToString()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub wibDesc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim x As String
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            bCapChanged = True

            x = "UPDATE tblStations SET [Description] = '" & wtb.Text & "' WHERE stationID = '" & lbStations.SelectedItem.Value & "'"

            preSQL.Add(x)
            sqlUpdateParams.Add(x)

            InputCaption.Add("Station Description")
            OldText.Add(wtb.OldText)
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub wibStations_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control
            Dim x As String

            x = wtb.SQLText
            preSQL.Add(x)
            If (Not x Is Nothing) Then
                If (x.IndexOf("D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD") > 0) Then
                    wtb.Text = FormatForSaveOrDisplay(wtb.Text, True)
                    x = x.Replace("D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD", wtb.Text)
                Else
                    x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", wtb.Text)
                End If
                sqlUpdateParams.Add(x)
            End If

            InputCaption.Add(wtb.InputCaption)
            OldText.Add(wtb.OldText)
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub dbStations_IndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim x As String
            Dim wddlStationIndex As GGS.WebDropDownList = CType(sender, GGS.WebDropDownList)    'getting ref to control

            If (wddlStationIndex.SelectedItem.Value = "{DFFCEE3D-763E-4f8d-A22B-E3A074FF1541}") Then
                wddlStationIndex.SelectedItem.Value = ""
            End If
            x = wddlStationIndex.SQLText
            preSQL.Add(x)
            x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", wddlStationIndex.SelectedItem.ToString())
            sqlUpdateParams.Add(x)

            InputCaption.Add(wddlStationIndex.InputCaption)
            OldText.Add(wddlStationIndex.OldText)
            NewText.Add(wddlStationIndex.SelectedItem.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim s As String
        Dim strMsg As String = ""
        Dim x As Int32
        x = 0
        Try

            If (lbStations.SelectedIndex > 0) Then   'list box is slelected then do save
                For Each s In sqlUpdateParams
                    DA.ExecSQL(s)

                    If strMsg <> "" Then
                        strMsg = strMsg & vbCrLf
                    End If
                    strMsg = strMsg & "   Station Parameter: " & InputCaption(x).ToString() & " for StationID: " & lbStations.SelectedItem.Text & " was changed from : " & OldText(x).ToString() & " to: " & NewText(x).ToString

                    x = x + 1
                Next
                'reset sql variable
                sqlUpdateParams.Clear()

                If (bCapChanged = True) Then
                    'description changed
                    Dim selecteditm As ListItem
                    selecteditm = lbStations.SelectedItem()
                    lbStationsList_Bind()
                    lbStations.SelectedIndex = lbStations.Items.IndexOf(lbStations.Items.FindByValue(selecteditm.Value))

                    bCapChanged = False
                End If

                Select Case x
                    Case 0
                        Master.tMsg("Alert", "No parameters were modified; thus, none were saved!", True, "Red")
                    Case 1
                        Master.tMsg("Save", x & " Parameter for station: " & lbStations.SelectedItem.ToString() & " is saved!" & vbCrLf & strMsg, False, "Red")
                    Case Is > 1
                        Master.tMsg("Save", x & " Parameters for station: " & lbStations.SelectedItem.ToString() & " were saved!" & vbCrLf & strMsg, False, "Red")
                End Select

                ' Update the Last Modified Date/Time in the Application Parameters table.
                If (x > 0) Then
                    UpdateTransactionParameters()
                End If
            Else
                Master.tMsg("Alert", "There is nothing to save. No Station is selected!", True, "Red")
            End If

            dlStationConfig_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub cmdCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopy.Click
        Try
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As SqlParameter

            If (lbStations.SelectedIndex < 1) Then
                Master.tMsg("Alert", "Please, select the Station you would like to copy!", True, "Red")
            Else
                'IN----------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@FromStationID", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = lbStations.SelectedItem.Value
                colParms.Add(prmNext)
                '------------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ToStationID", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = hidDDLStation.Value
                colParms.Add(prmNext)
                '------------------------------------------------------------
                DA.ExecSP("procCopyStation", colParms)

                Master.tMsg("Copy", "Station: '" & lbStations.SelectedItem.Text & "' was copied to:'" & hidDDLStation_Name.Value.Trim() & "'")

                ' Update the Last Modified Date/Time in the Application Parameters table.
                UpdateTransactionParameters()

                lbStationsList_Bind()
                lbStations.SelectedIndex = lbStations.Items.IndexOf(lbStations.Items.FindByValue(hidDDLStation.Value))
                dlStationConfig_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Dim oSqlParameter As SqlParameter
        Dim parameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim status As String = ""
        Dim message As String = ""
        Dim errMessage As String = ""
        Dim newStationID As Integer

        Try
            message = "New Station [" + txtNewStationName.Text + "] on Line [" + ddlLineNumber_New.SelectedItem.Text + "] has "

            If ((txtNewStationName.Text.Length = 0) Or (ddlLineNumber_New.SelectedIndex <= 0)) Then
                Master.tMsg("Alert", message + "not been created. ", True, "Red")
            Else
                parameters.Add(New SqlParameter("@StationDesc", txtNewStationName.Text))
                parameters.Add(New SqlParameter("@StationType", "9"))  ' just hardcoding this until we figure how we want the 'Add New'  to actually work. ie. can we add any type or just ODS?
                parameters.Add(New SqlParameter("@lineID", ddlLineNumber_New.SelectedValue))

                oSqlParameter = New SqlParameter("@nStationID", SqlDbType.Int)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                ' this procedure does not have a status or errmsg
                'oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
                'oSqlParameter.Direction = ParameterDirection.Output
                'parameters.Add(oSqlParameter)

                'oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
                'oSqlParameter.Direction = ParameterDirection.Output
                'parameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("[dbo].[procInsertNewStation]", parameters)

                For Each oParameter In colOutput
                    With oParameter
                        ' this procedure does not have a status or errmsg
                        'If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        '    status = oParameter.Value.ToString()
                        'ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        '    errMessage = oParameter.Value.ToString()
                        'Else
                        If .Direction = ParameterDirection.Output And .ParameterName = "@nStationID" Then
                            newStationID = Convert.ToInt32(oParameter.Value)
                        End If
                    End With
                Next

                lbStationsList_Bind()

                ' this procedure does not have a status or errmsg
                'If ((status.ToUpper() = "TRUE")) Then
                Master.Msg = message + "been created."
                lbStations.SelectedIndex = lbStations.Items.IndexOf(lbStations.Items.FindByText(txtNewStationName.Text))
                '    Else
                '    Master.Msg = message + "not been created." + errMessage
                'End If

                dlStationConfig_Bind()

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Subs and Functions"

    Private Sub lbStationsList_Bind()
        Try
            With lbStations
                .Items.Clear()

                If (ddlLineNumber.SelectedIndex > 0) Then
                    .DataSource = DA.GetDataSet("SELECT StationID, [Description] FROM dbo.tblStations WHERE LineID = " & ddlLineNumber.SelectedValue & " ORDER BY DisplayID")
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

    Private Sub dlStationConfig_Bind()  'data list bind
        With dlStationsConfig
            .DataSource = Nothing

            If (lbStations.SelectedIndex > 0) Then
                Dim TheSQL As String = _
                    "SELECT  t.CharacterSwapEnabled, " & _
                            "t.StationParameterTypeID,  " & _
                            "t.Description, " & _
                            "p.StationID, " & _
                            "p.StationParameterValue, " & _
                            "t.ParameterTypeID, " & _
                            "t.StationParameterListID, " & _
                            "t.StationTypeID, " & _
                            "t.ParameterListID " & _
                   "FROM dbo.tblStationParameterTypes t " & _
                   "INNER JOIN dbo.tblStationParameters p " & _
                   "ON p.StationParameterTypeID = t.StationParameterTypeID " & _
                   "INNER JOIN dbo.tblStations s " & _
                   "ON p.StationID = s.StationID " & _
                   "AND t.StationTypeID = s.StationTypeID " & _
                   "WHERE p.StationID = '" & lbStations.SelectedItem.Value & "' " & _
                   "ORDER BY t.DisplayID"

                .DataSource = DA.GetDataSet(TheSQL)
            End If

            .DataBind()
        End With
    End Sub

    Private Sub EnableControls()
        Try
            Master.Secure(cmdSave)
            Master.Secure(cmdCopy)
            Master.Secure(cmdMoveUp)
            Master.Secure(cmdMoveDown)

            If (lbStations.SelectedIndex < 1) Then
                cmdSave.Enabled = False
                cmdCopy.Enabled = False
                cmdMoveUp.Enabled = False
                cmdMoveDown.Enabled = False

            ElseIf (lbStations.SelectedIndex = 1) Then
                cmdMoveUp.Enabled = False
                cmdMoveDown.Enabled = cmdMoveDown.Enabled And True

            ElseIf (lbStations.SelectedIndex = lbStations.Items.Count - 1) Then
                cmdMoveUp.Enabled = cmdMoveUp.Enabled And True
                cmdMoveDown.Enabled = False

            ElseIf (lbStations.SelectedIndex > 0) Then
                cmdMoveUp.Enabled = cmdMoveUp.Enabled And True
                cmdMoveDown.Enabled = cmdMoveDown.Enabled And True

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function FormatForSaveOrDisplay(ByRef str As String, ByRef bfrmt As Boolean) As String
        Dim dschar As DataSet
        Dim dr As DataRow

        dschar = DA.GetDataSet("SELECT * FROM dbo.tblCharacterSwap")
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

    Public Function ObjectVisibilityInTemplate(ByVal e As Object) As String
        Try
            Dim StationParameterValue As String
            Dim strInputCaption As String
            Dim SQLUpdateCommandText As String    'update string which is stored in controll and send to the user
            Dim CharSwap As Boolean = False

            Dim dlItem As DataListItem = CType(e, DataListItem)
            Dim dlLblDesc As Label = CType(dlItem.FindControl("dlLblDesc"), Label)
            Dim dbDataRVProdParam As DataRowView = CType(dlItem.DataItem, DataRowView) 'holds a reference to DataItem in datalist object

            dbDataRVProdParam = CType(dlItem.DataItem, DataRowView)
            wddlWebDropDownList = CType(dlItem.FindControl("wddlStationIndex"), GGS.WebDropDownList)    'getting ref to drop box
            xtxtWebInputBox = CType(dlItem.FindControl("wibStations"), GGS.WebInputBox)                 'getting ref to input box
            imgCheck = CType(dlItem.FindControl("dlImgCheck"), Web.UI.WebControls.Image)

            strInputCaption = dbDataRVProdParam.Item("Description").ToString()

            StationParameterValue = dbDataRVProdParam.Item("StationParameterValue").ToString()

            dlLblDesc.ToolTip = "ParameterListID = " & dbDataRVProdParam.Item("ParameterListID").ToString() & _
                                ", StationTypeID:" & dbDataRVProdParam.Item("StationTypeID").ToString() & _
                                ", StationParameterListID:" & dbDataRVProdParam.Item("StationParameterListID").ToString() & _
                                ", StationParameterTypeID:" & dbDataRVProdParam.Item("StationParameterTypeID").ToString()

            'setting what to display text box or drop-down listbox
            If (dbDataRVProdParam.Item("ParameterListID").ToString().Trim = "") Then
                'box visible=true, dd visible=false
                wddlWebDropDownList.Visible = False
                xtxtWebInputBox.Visible = True
            Else
                wddlWebDropDownList.Visible = True
                xtxtWebInputBox.Visible = False

                'binding drop-down listbox to source
                ddStationParameter_Bind(dbDataRVProdParam.Item("ParameterListID").ToString(), _
                                        dbDataRVProdParam.Item("StationParameterValue").ToString().Trim)
            End If

            If (dbDataRVProdParam("CharacterSwapEnabled").ToString() = "") Then
                CharSwap = False
            Else
                CharSwap = CBool(dbDataRVProdParam("CharacterSwapEnabled"))
            End If

            If CharSwap Then
                SQLUpdateCommandText = "UPDATE tblStationParameters Set StationParameterValue = 'D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD' WHERE " _
               & "(StationID = '" & dbDataRVProdParam.Item("StationID").ToString() & "') AND (StationParameterTypeID = '" & dbDataRVProdParam.Item("StationParameterTypeID").ToString() & "') "
            Else
                SQLUpdateCommandText = "UPDATE tblStationParameters Set StationParameterValue = '6933A6C7-80FC-4b37-907E-26FCE24DD7EE' WHERE " _
               & "(StationID = '" & dbDataRVProdParam.Item("StationID").ToString() & "') AND (StationParameterTypeID = '" & dbDataRVProdParam.Item("StationParameterTypeID").ToString() & "') "
            End If

            xtxtWebInputBox.SQLText = SQLUpdateCommandText
            xtxtWebInputBox.InputCaption = strInputCaption
            xtxtWebInputBox.OldText = StationParameterValue

            If (CharSwap) Then
                xtxtWebInputBox.Text = FormatForSaveOrDisplay(xtxtWebInputBox.Text, False)
            End If

            wddlWebDropDownList.SQLText = SQLUpdateCommandText
            wddlWebDropDownList.InputCaption = strInputCaption
            wddlWebDropDownList.OldText = StationParameterValue

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

        ObjectVisibilityInTemplate = ""
    End Function

    Private Sub ddStationParameter_Bind(ByRef ParameterListID As String, ByRef ParamValue As String)
        Dim liZ As New ListItem
        Dim strSql As String

        strSql = "SELECT v.ParameterListValue " & _
                 "FROM dbo.tblParameterLists l " & _
                 "INNER JOIN dbo.tblParameterListValues v " & _
                 "ON l.ParameterListID = v.ParameterListID " & _
                 "WHERE l.ParameterListID = '" & ParameterListID & "'"

        wddlWebDropDownList.DataSource = DA.GetDataSet(strSql)
        wddlWebDropDownList.DataTextField = "ParameterListValue"
        wddlWebDropDownList.DataValueField = "ParameterListValue"
        wddlWebDropDownList.DataBind()
        liZ.Text = ParamValue
        wddlWebDropDownList.Items.Insert(0, liZ)

        If Not ParamValue = "" Then           'if there is blank one already do not insert
            wddlWebDropDownList.Items.Insert(1, New ListItem)
        End If

    End Sub

    Public Function StationDesc(ByRef x As Object) As String
        Try
            Dim dlItem As DataListItem = CType(x, DataListItem)
            Dim imgCaption As Web.UI.WebControls.Image = CType(dlItem.FindControl("imgCaption"), Web.UI.WebControls.Image)
            Dim wibDesc As GGS.WebInputBox = CType(dlItem.FindControl("wibDesc"), GGS.WebInputBox)

            If bCapChanged = True Then
                imgCaption.Visible = True
                'check box vsbl if control changed
            End If

            If lbStations.SelectedItem IsNot Nothing Then
                wibDesc.OldText = lbStations.SelectedItem.Text
                StationDesc = lbStations.SelectedItem.Text
            Else
                StationDesc = "Error: Station Description missing!"
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            StationDesc = "Error getting Station Description!"
        End Try
    End Function

    Private Sub BindLines()
        Dim ds As DataSet
        Try
            ds = DA.GetDataSet("SELECT LineID, LineName FROM dbo.tblSGLines")

            ' Lines for the drop-down list on the main web page
            ddlLineNumber.DataSource = ds
            ddlLineNumber.DataTextField = "LineName"
            ddlLineNumber.DataValueField = "LineID"
            ddlLineNumber.DataBind()
            ddlLineNumber.Items.Insert(0, "Choose a Line")

            ddlLineNumber_Copy.DataSource = ds
            ddlLineNumber_Copy.DataTextField = "LineName"
            ddlLineNumber_Copy.DataValueField = "LineID"
            ddlLineNumber_Copy.DataBind()
            ddlLineNumber_Copy.Items.Insert(0, "Choose a Line")

            ddlLineNumber_New.DataSource = ds
            ddlLineNumber_New.DataTextField = "LineName"
            ddlLineNumber_New.DataValueField = "LineID"
            ddlLineNumber_New.DataBind()
            ddlLineNumber_New.Items.Insert(0, "Choose a Line")


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub UpdateTransactionParameters()

        Try
            Dim strLineNumber As String = ""
            strLineNumber = ddlLineNumber.SelectedValue

            BizLayer.SetRecipeSavedDT("5", "11", strLineNumber)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

#End Region

End Class