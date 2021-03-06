Imports System.Data.SqlClient

Public Class StationComponentScanAssignment
    Inherits System.Web.UI.Page

#Region "Globals"

    Private InputCaption As New ArrayList    'caption asscoiated with control
    Private OldText As New ArrayList         'old value of ctrl
    Private NewText As New ArrayList         'new posted value of ctrl
    Private preSQL As New ArrayList
    Private sqlUpdateParams As New ArrayList    'putting my update commands here and when save is fired I execute them

    Protected WithEvents WebDropDownList As GGS.WebDropDownList 'holds reference to the parameter in each tamplate
    Protected WithEvents WebInputBox As GGS.WebInputBox
    Protected WithEvents imgCheck As System.Web.UI.WebControls.Image

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not (Page.IsPostBack)) Then
                ddlLineNumber_Bind()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLineNumber_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlLineNumber.SelectedIndexChanged
        Try
            If (ddlLineNumber.SelectedIndex < 1) Then
                ddlStyleGroups.Items.Clear()
                ddlStations.Items.Clear()
            Else
                ddlStyleGroupsList_Bind()
                ddlStationsList_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub ddlStyleGroups_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStyleGroups.SelectedIndexChanged
        Try
            dlStationConfig_Bind()
            Master.Msg = "Parameters: " & ddlStations.SelectedItem.ToString() & ", " & ddlStyleGroups.SelectedItem.ToString()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub ddlStations_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStations.SelectedIndexChanged
        Try
            dlStationConfig_Bind()
            Master.Msg = "Parameters: " & ddlStations.SelectedItem.ToString() & ", " & ddlStyleGroups.SelectedItem.ToString()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub cmdCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopy.Click
        Try
            Dim str As String

            If ((ddlStations.SelectedIndex < 1) Or (ddlStyleGroups.SelectedIndex < 1)) Then
                ' Should never get here!  Button should be disabled until these are selected.
                Master.tMsg("Alert", "Please select the Station/Style Group you would like to copy!", True, "Red")
            Else
                str = ddlStations.SelectedItem.Text
                Dim colParms As New List(Of SqlParameter)
                Dim prmNext As SqlParameter

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
                DA.ExecSP("procCopyComponentScan", colParms)

                Master.tMsg("Copy", "Component Scan Assignment for Station: " & ddlStations.SelectedItem.ToString() & _
                                    " and Style Group: " & ddlStyleGroups.SelectedItem.ToString() & _
                                    " was copied to Station: " & hidDDLStation_Name.Value.Trim() & _
                                    " and Style Group: " & hidDDLStyleGroup_Name.Value.Trim())

                ' Update the Last Modified Date/Time in the Application Parameters table.
                UpdateTransactionParameters()

                ddlStations.SelectedIndex = ddlStations.Items.IndexOf(ddlStations.Items.FindByValue(hidDDLStation.Value))
                ddlStyleGroups.SelectedIndex = ddlStyleGroups.Items.IndexOf(ddlStyleGroups.Items.FindByValue(hidDDLStyleGroup.Value))

                dlStationConfig_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim s As String
            Dim x As Int32 = 0

            If ((ddlStations.SelectedItem Is Nothing) Or (ddlStations.SelectedIndex < 1) Or _
                    (ddlStyleGroups.SelectedItem Is Nothing) Or (ddlStyleGroups.SelectedIndex < 1)) Then
                Master.tMsg("Alert", "There is nothing to save. No Station or Style Group is selected", True, "Red")
            Else
                For Each s In sqlUpdateParams
                    DA.ExecSQL(s)
                    Master.tMsg("Save", "Station Parameter: " & InputCaption(x).ToString() & " for StationID: " & ddlStations.SelectedItem.Text & " was changed from : " & OldText(x).ToString() & " to: " & NewText(x).ToString())
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

                If (x > 0) Then
                    ' Update the Last Modified Date/Time in the Application Parameters table.
                    UpdateTransactionParameters()

                    dlStationConfig_Bind()
                End If

                'reset sqlUpdateParams
                sqlUpdateParams.Clear()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Sub dlwibParam_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
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

    Public Sub dlwddlParam_IndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim wddl As GGS.WebDropDownList = CType(sender, GGS.WebDropDownList)    'getting ref to control
            Dim x As String

            If (wddl.SelectedItem.Value = "{DFFCEE3D-763E-4f8d-A22B-E3A074FF1541}") Then
                wddl.SelectedItem.Value = ""
            End If
            x = wddl.SQLText
            preSQL.Add(x)
            x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", wddl.SelectedItem.ToString())
            sqlUpdateParams.Add(x)

            InputCaption.Add(wddl.InputCaption)
            OldText.Add(wddl.OldText)
            NewText.Add(wddl.SelectedItem.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLineNumber_Bind()
        Try

            ddlLineNumber.Items.Clear()
            ddlLineNumber_Copy.Items.Clear()

            Dim ds As DataSet = DA.GetDataSet("SELECT LineID, LineName FROM dbo.tblSGLines ORDER BY LineName")

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then

                ddlLineNumber.DataSource = ds
                ddlLineNumber.DataTextField = "LineName"
                ddlLineNumber.DataValueField = "LineID"
                ddlLineNumber.DataBind()
                ddlLineNumber.Items.Insert(0, "Choose a Line")

                ' Initialize the Line dropdown list in the Copy dialog.
                ddlLineNumber_Copy.DataSource = ds.Copy()
                ddlLineNumber_Copy.DataTextField = "LineName"
                ddlLineNumber_Copy.DataValueField = "LineID"
                ddlLineNumber_Copy.DataBind()
                ddlLineNumber_Copy.Items.Insert(0, "Choose a Line")
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())

        End Try
    End Sub

    Private Sub ddlStyleGroupsList_Bind()   'dd box bind
        If (ddlLineNumber.SelectedIndex < 1) Then
            ddlStyleGroups.DataSource = Nothing
            ddlStyleGroups.DataBind()
        Else
            ddlStyleGroups.DataSource = DA.GetDataSet("SELECT StyleGroupID, StyleGroupName FROM dbo.tblSGStyleGroups WHERE LineID = " & ddlLineNumber.SelectedValue & " ORDER BY PLCArrayOrder")
            ddlStyleGroups.DataTextField = "StyleGroupName"
            ddlStyleGroups.DataValueField = "StyleGroupID"
            ddlStyleGroups.DataBind()
            ddlStyleGroups.Items.Insert(0, "Choose a Style Group")
        End If
    End Sub

    Private Sub ddlStationsList_Bind()   'dd box bind
        If (ddlLineNumber.SelectedIndex < 1) Then
            ddlStations.DataSource = Nothing
            ddlStations.DataBind()
        Else
            ddlStations.DataSource = DA.GetDataSet("SELECT StationID, [Description] FROM dbo.tblStations WHERE LineID = " & ddlLineNumber.SelectedValue & " ORDER BY DisplayID")
            ddlStations.DataTextField = "Description"
            ddlStations.DataValueField = "StationID"
            ddlStations.DataBind()
            ddlStations.Items.Insert(0, "Choose a Station")
        End If
    End Sub

    Private Sub dlStationConfig_Bind()  'data list bind
        If ((ddlStations.SelectedIndex < 1) Or (ddlStyleGroups.SelectedIndex < 1)) Then
            dlStationsConfig.DataSource = Nothing
            dlStationsConfig.DataBind()
        Else
            Dim TheSQL As String = _
                "SELECT  t.CompScanParamTypeID,  " & _
                        "t.[Description], " & _
                        "p.StyleGroupID, " & _
                        "p.StationID, " & _
                        "p.CompScanParamValue, " & _
                        "t.ParameterTypeID, " & _
                        "t.ParameterListID " & _
               "FROM tblSGComponentScanParameterTypes t " & _
               "INNER JOIN tblSGComponentScanParameters p " & _
               "ON p.CompScanParamTypeID = t.CompScanParamTypeID " & _
               "WHERE p.StationID = '" & ddlStations.SelectedValue & "' " & _
               "AND p.StyleGroupID = '" & ddlStyleGroups.SelectedValue & "' " & _
               "ORDER BY t.DisplayID"

            dlStationsConfig.DataSource = DA.GetDataSet(TheSQL)
            dlStationsConfig.DataBind()
        End If
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

    Public Function ObjectVisibilityInTemplate(ByVal e As Object) As String
        Try
            Dim SQLUpdateCommandText As String    'update string which is stored in controll and send to the user
            Dim StationParameterValue As String
            Dim strInputCaption As String

            Dim dlItem As DataListItem = CType(e, DataListItem)
            Dim dlLblDesc As Label = CType(dlItem.FindControl("dllblDesc"), Label)
            Dim dbDataRVProdParam As DataRowView = CType(dlItem.DataItem, DataRowView)  'holds a reference to DataItem in datalist object

            WebDropDownList = CType(dlItem.FindControl("dlwddlParam"), GGS.WebDropDownList)    'getting ref to drop box
            WebInputBox = CType(dlItem.FindControl("dlwibParam"), GGS.WebInputBox)                'getting ref to input box
            imgCheck = CType(dlItem.FindControl("dlImgCheck"), Web.UI.WebControls.Image)

            strInputCaption = dbDataRVProdParam.Item("Description").ToString()

            StationParameterValue = dbDataRVProdParam.Item("CompScanParamValue").ToString()

            dlLblDesc.ToolTip = "ParameterListID = " & dbDataRVProdParam.Item("ParameterListID").ToString() & "," & _
                                "CompScanParamTypeID:" & dbDataRVProdParam.Item("CompScanParamTypeID").ToString()

            SQLUpdateCommandText = "UPDATE tblSGComponentScanParameters " & _
                                   "SET CompScanParamValue = '6933A6C7-80FC-4b37-907E-26FCE24DD7EE' " & _
                                   "WHERE (StationID = '" & dbDataRVProdParam.Item("StationID").ToString() & "') " & _
                                   "AND (StyleGroupID = " & dbDataRVProdParam.Item("StyleGroupID").ToString() & ") " & _
                                   "AND (CompScanParamTypeID = '" & dbDataRVProdParam.Item("CompScanParamTypeID").ToString() & "') "

            'setting what to display text box or drop-down list box
            If (dbDataRVProdParam.Item("ParameterListID").ToString() = "") Then
                'text visible=true, dd visible=false
                WebDropDownList.Visible = False
                WebInputBox.Visible = True

                WebInputBox.SQLText = SQLUpdateCommandText
                WebInputBox.InputCaption = strInputCaption
                WebInputBox.OldText = StationParameterValue
            Else
                'text visible=false, dd visible=true
                WebDropDownList.Visible = True
                WebInputBox.Visible = False

                'binding drop-down list to source
                ddParameter_Bind(dbDataRVProdParam.Item("ParameterListID").ToString(), _
                                 dbDataRVProdParam.Item("CompScanParamValue").ToString().Trim)

                WebDropDownList.SQLText = SQLUpdateCommandText
                WebDropDownList.InputCaption = strInputCaption
                WebDropDownList.OldText = StationParameterValue
            End If

            ' Set visibility on the check image.
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

    Private Sub ddParameter_Bind(ByRef ParameterListID As String, ByRef ParamValue As String)
        Dim liZ As New ListItem
        Dim strSql As String

        strSql = "SELECT v.ParameterListValue " & _
                 "FROM dbo.tblParameterLists l " & _
                 "INNER JOIN dbo.tblParameterListValues v " & _
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

    Private Sub EnableControls()
        Try
            Dim bEnabled As Boolean = False

            Master.Secure(cmdSave)
            Master.Secure(cmdCopy)

            If ((ddlStations.SelectedIndex < 1) Or (ddlStyleGroups.SelectedIndex < 1)) Then
                cmdSave.Enabled = False
                cmdCopy.Enabled = False
            End If

            bEnabled = (ddlLineNumber.SelectedIndex > 0)
            ddlStyleGroups.Enabled = bEnabled
            ddlStations.Enabled = bEnabled

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub UpdateTransactionParameters()

        Try
            Dim strLineNumber As String = ""

            strLineNumber = CStr(CInt(ddlLineNumber.SelectedValue.ToString) * 100)

            BizLayer.SetRecipeSavedDT("3", "10", strLineNumber)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

#End Region

End Class