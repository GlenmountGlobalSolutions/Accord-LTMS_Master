Imports System.Data.SqlClient

Public Class ApplicationConfiguration
    Inherits System.Web.UI.Page

#Region "Globals"
    Protected WithEvents WebInputBox As GGS.WebInputBox
    Protected WithEvents WebDropDownList As GGS.WebDropDownList 'holds reference to the parameter in each template
    Protected WithEvents imgCheck As System.Web.UI.WebControls.Image


    Private sqlUpdateParams As New ArrayList    'putting my update commands here and when save is fired I execute them
    Private InputCaption As New ArrayList     'caption asscoiated with control
    Private OldText As New ArrayList    'old value of ctrl
    Private NewText As New ArrayList    'new posted value of ctrl
    Private preSQL As New ArrayList

    Private bCapChanged As Boolean

    Public Const STR_NOT_FOUND As String = " * Value not found in list!"

#End Region

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                lbApps_Bind()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ApplicationConfiguration_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbApps_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbApps.SelectedIndexChanged
        Try
            dataListApps_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim x As Int32
            Dim s As String
            x = 0

            If (Not lbApps.SelectedItem Is Nothing) Then          'list box is slelected then do save
                Try
                    For Each s In sqlUpdateParams
                        If (InputCaption(x).ToString = "6933A6C7-80FC-4b37-907E-26FCE24DD7EE") Then
                            'description changed
                            DA.ExecSQL(s)
                            Master.tMsg("Save", "Description for Application: " & lbApps.SelectedItem.Text & " ID:" & lbApps.SelectedItem.Value & " was changed from : " & OldText(x).ToString & " to: " & NewText(x).ToString)
                            bCapChanged = True
                        Else
                            DA.ExecSQL(s)
                            Master.tMsg("Save", "Application parameter: " & InputCaption(x).ToString & " for Application: " & lbApps.SelectedItem.Text & " ID:" & lbApps.SelectedItem.Value & " was changed from : " & OldText(x).ToString & " to: " & NewText(x).ToString)
                        End If
                        x = x + 1
                    Next

                Catch q As Exception
                    Master.eMsg(q.ToString())
                End Try

                If (bCapChanged = True) Then
                    'decription is changed must refresh Description on the left side in lbBradcastPoint
                    Dim selecteditm As ListItem
                    selecteditm = lbApps.SelectedItem()
                    lbApps_Bind()
                    lbApps.SelectedIndex = lbApps.Items.IndexOf(lbApps.Items.FindByValue(selecteditm.Value))
                End If

                Select Case x
                    Case 0
                        Master.Msg = "No parameters were modified; thus, none were saved!"
                    Case 1
                        Master.Msg = x & " Parameter for Application: " & lbApps.SelectedItem.ToString & " is saved!"
                    Case Is > 1
                        Master.Msg = x & " Parameters for Application: " & lbApps.SelectedItem.ToString & " were saved!"
                End Select
            Else
                Master.tMsg("Alert", "There is nothing to save. No Station or Style Group is selected", True, "Red")
            End If

            dataListApps_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub Description_OnTextChaged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim x As String
            WebInputBox = CType(sender, GGS.WebInputBox)    'getting ref to control

            x = WebInputBox.SQLText

            sqlUpdateParams.Add("Update tblApplications Set Description = '" & WebInputBox.Text & "' Where ApplicationID = " & lbApps.SelectedItem.Value)
            InputCaption.Add("6933A6C7-80FC-4b37-907E-26FCE24DD7EE")
            OldText.Add(WebInputBox.OldText)
            NewText.Add(WebInputBox.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub dbAppParam_IndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Performing Save. If data in dropdown list changed then run appropriate sql statements
        'event/method:dbApp_IndexChanged is raised by FRAMEWORK if data is changed
        Try
            Dim x, TextToSave As String
            WebDropDownList = CType(sender, GGS.WebDropDownList)    'getting ref to control

            If (WebDropDownList.SelectedIndex > 0) Then      'do not save the top most value (inserted text...)
                x = WebDropDownList.SQLText
                preSQL.Add(x)
                If (x.IndexOf("{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD}") > 0) Then
                    TextToSave = FormatForSaveOrDisplay(WebDropDownList.SelectedValue.ToString, True)
                    x = x.Replace("{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD}", TextToSave)
                    sqlUpdateParams.Add(x)
                    InputCaption.Add(WebDropDownList.InputCaption)
                    OldText.Add(WebDropDownList.OldText)
                    NewText.Add(WebDropDownList.SelectedItem.Text)
                Else
                    x = WebDropDownList.SQLText
                    preSQL.Add(x)
                    x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", WebDropDownList.SelectedValue.ToString)
                    sqlUpdateParams.Add(x)
                    InputCaption.Add(WebDropDownList.InputCaption)
                    OldText.Add(WebDropDownList.OldText)
                    NewText.Add(WebDropDownList.SelectedItem.Text)
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub ParameterValueTextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim x As String
            WebInputBox = CType(sender, GGS.WebInputBox)    'getting ref to control

            x = WebInputBox.SQLText
            preSQL.Add(x)

            If (Not x Is Nothing) Then
                x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", WebInputBox.Text)
                sqlUpdateParams.Add(x)
            End If
            InputCaption.Add(WebInputBox.InputCaption)
            OldText.Add(WebInputBox.OldText)
            NewText.Add(WebInputBox.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        Master.Secure(cmdSave)

        If (lbApps.SelectedItem Is Nothing) Then
            cmdSave.Enabled = False
        End If
    End Sub

    Private Sub lbApps_Bind()
        Try
            Dim ds As DataSet = DA.GetDataSet("SELECT [ApplicationID], [Description] FROM [dbo].[tblApplications]")

            With lbApps
                .DataSource = ds
                .DataTextField = "Description"
                .DataValueField = "ApplicationID"
                .DataBind()
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dataListApps_Bind()
        Try
            With dlApps
                .DataSource = Nothing

                If Not (lbApps.SelectedItem Is Nothing) Then
                    Dim TheSQL As String = _
                            "SELECT tblApplicationParameterTypes.DropdownListSQL, " & _
                                   "tblApplicationParameters.ApplicationParameterTypeID, " & _
                                   "tblApplicationParameters.ApplicationID, " & _
                                   "tblApplicationParameters.ApplicationParameterValue, " & _
                                   "tblApplicationParameterTypes.Description, " & _
                                   "tblApplicationParameterTypes.ParameterListID " & _
                            "FROM tblApplicationParameters " & _
                            "INNER JOIN tblApplicationParameterTypes " & _
                            "ON tblApplicationParameters.ApplicationParameterTypeID = tblApplicationParameterTypes.ApplicationParameterTypeID " & _
                            "WHERE tblApplicationParameters.ApplicationID = '" & lbApps.SelectedItem.Value & "' " & _
                            "ORDER BY tblApplicationParameterTypes.DisplayID"
                    .DataSource = DA.GetDataSet(TheSQL)
                End If

                .DataBind()
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Function Description() As String
        Description = lbApps.SelectedItem.Text
    End Function

    Public Function Description(ByRef e As Object) As String
        'This method handles the descrption in the input box (in the header template, not item template)
        Try
            Dim dlItem As DataListItem = CType(e, DataListItem)
            WebInputBox = CType(dlItem.FindControl("txtDescription"), GGS.WebInputBox)              'getting ref to input box
            imgCheck = CType(dlItem.FindControl("Image2"), Web.UI.WebControls.Image)            'getting ref to Check image

            If bCapChanged = True Then           'flag whether I should display check mark or not 
                imgCheck.Visible = True
            Else
                imgCheck.Visible = False              'image is invisible
            End If

            WebInputBox.OldText = lbApps.SelectedItem.Text               'textbox
            Description = lbApps.SelectedItem.Text

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Description = "ERROR"
        End Try
    End Function

    Private Function FormatForSaveOrDisplay(ByRef str As String, ByRef bfrmt As Boolean) As String
        Dim dschar As DataSet
        Dim dr As DataRow

        dschar = DA.GetDataSet("SELECT * FROM tblCharacterSwap")
        If (bfrmt = False) Then       'display
            For Each dr In dschar.Tables(0).Rows
                str = str.Replace(dr(1).ToString, dr(0).ToString)
            Next
            Return str

        Else          'save
            For Each dr In dschar.Tables(0).Rows
                str = str.Replace(dr(0).ToString, dr(1).ToString)
            Next
            Return str
        End If
    End Function

    Public Function ObjectVisibilityInTemplate(ByVal e As Object) As Object
        Try
            Dim SQLUpdateCommandText As String
            Dim ParameterListID As String
            Dim ApplicationParameterValue As String
            Dim DropdownListSQL As String
            Dim strInputCaption As String         'caption which is next of the input or drop box

            Dim dlItem As DataListItem = CType(e, DataListItem)
            Dim dbDataRVProdParam As System.Data.DataRowView = CType(dlItem.DataItem, DataRowView)  'getting ref to data row

            Dim dlLblDesc As Label = CType(dlItem.FindControl("Label1"), Label)                  'getting ref to description label
            WebDropDownList = CType(dlItem.FindControl("WebDropDownList"), GGS.WebDropDownList)     'getting ref to drop box
            WebInputBox = CType(dlItem.FindControl("Webinputbox2"), GGS.WebInputBox)              'getting ref to input box
            imgCheck = CType(dlItem.FindControl("Image1"), Web.UI.WebControls.Image)            'getting ref to Check image

            dlLblDesc.ToolTip = "Application Parameter Type ID = " & dbDataRVProdParam.Item("ApplicationParameterTypeID").ToString()
            WebInputBox.ToolTip = "Current Value:" & dbDataRVProdParam.Item("ApplicationParameterValue").ToString() & ", ApplicationParameterTypeID:" & dbDataRVProdParam.Item("ApplicationParameterTypeID").ToString() & ", ApplicationID:" & dbDataRVProdParam.Item("ApplicationID").ToString()

            If (dbDataRVProdParam.Item("Description") Is System.DBNull.Value) Then
                strInputCaption = ""
            Else
                strInputCaption = dbDataRVProdParam.Item("Description").ToString()
            End If

            If (dbDataRVProdParam.Item("ParameterListID") Is System.DBNull.Value) Then
                ParameterListID = ""
            Else
                ParameterListID = dbDataRVProdParam.Item("ParameterListID").ToString()
            End If

            If (dbDataRVProdParam.Item("ApplicationParameterValue") Is System.DBNull.Value) Then
                ApplicationParameterValue = ""
            Else
                ApplicationParameterValue = dbDataRVProdParam.Item("ApplicationParameterValue").ToString()
            End If

            If (dbDataRVProdParam.Item("DropdownListSQL") Is System.DBNull.Value) Then
                DropdownListSQL = ""
            Else
                DropdownListSQL = dbDataRVProdParam.Item("DropdownListSQL").ToString()
            End If

            'check for sql statement for ddl. if no sql check for param list
            If (Not (Trim(DropdownListSQL) = "")) Then
                Dim dsDDLsqlBind As New DataSet
                dsDDLsqlBind = DA.GetDataSet(dbDataRVProdParam.Item("DropdownListSQL").ToString())
                WebDropDownList.DataSource = dsDDLsqlBind
                WebDropDownList.DataTextField = "Text"
                WebDropDownList.DataValueField = "Value"
                WebDropDownList.DataBind()
                WebDropDownList.Items.Insert(0, STR_NOT_FOUND)          'used if there is a value but it cannot be found.
                WebDropDownList.Items.Insert(1, "")          'empty space
                WebDropDownList.Items(1).Value = ""

                'select value
                If (Not WebDropDownList.Items.FindByValue(ApplicationParameterValue) Is Nothing) Then
                    WebDropDownList.SelectedIndex = WebDropDownList.Items.IndexOf(WebDropDownList.Items.FindByValue(ApplicationParameterValue))
                    WebDropDownList.OldText = ApplicationParameterValue
                Else
                    WebDropDownList.Items(0).Text = ApplicationParameterValue.ToString() + STR_NOT_FOUND
                    WebDropDownList.SelectedIndex = 0
                End If

                WebDropDownList.Visible = True          'displaying dropbox
                WebInputBox.Visible = False
            Else
                If (Not (Trim(ParameterListID) = "")) Then
                    WebDropDownList.Visible = True                   'displaying dropbox
                    'charswap is always false because value is not in the database - pb
                    dbParameterListID_Bind(Trim(ParameterListID), Trim(ApplicationParameterValue), False)   'binding databox to source
                    WebInputBox.Visible = False
                Else        'if ParameterListID is empty display textbox
                    WebDropDownList.Visible = False
                    WebInputBox.Visible = True
                End If
            End If

            SQLUpdateCommandText = "UPDATE tblApplicationParameters " & _
                                   "SET ApplicationParameterValue = '6933A6C7-80FC-4b37-907E-26FCE24DD7EE' " & _
                                   "WHERE ApplicationParameterTypeID = '" & dbDataRVProdParam.Item("ApplicationParameterTypeID").ToString() & "' " & _
                                   "AND ApplicationID = '" & dbDataRVProdParam.Item("ApplicationID").ToString() & "' "

            WebInputBox.SQLText = SQLUpdateCommandText
            WebInputBox.InputCaption = strInputCaption
            WebInputBox.OldText = ApplicationParameterValue

            WebDropDownList.SQLText = SQLUpdateCommandText
            WebDropDownList.InputCaption = strInputCaption

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

    Private Sub dbParameterListID_Bind(ByRef paramID As String, ByRef Param As String, ByVal bSwapChr As Boolean)
        Dim strSQL As String

        strSQL = "SELECT tblParameterListValues.ParameterListValue " & _
                 "FROM tblParameterLists " & _
                 "INNER JOIN tblParameterListValues " & _
                 "ON tblParameterLists.ParameterListID = tblParameterListValues.ParameterListID " & _
                 "WHERE tblParameterLists.ParameterListID = '" & paramID & "' "

        WebDropDownList.DataSource = DA.GetDataSet(strSQL)
        WebDropDownList.DataTextField = "ParameterListValue"
        WebDropDownList.DataValueField = "ParameterListValue"

        WebDropDownList.DataBind()
        WebDropDownList.TabIndex.ToString()
        If bSwapChr Then
            WebDropDownList.Items.Insert(0, FormatForSaveOrDisplay(Param, False))
            WebDropDownList.OldText = Param
            If Not Param = "" Then           'if there is blank one already do not insert
                WebDropDownList.Items.Insert(1, New ListItem)
            End If
        Else
            WebDropDownList.Items.Insert(0, Param)
            If Not Param = "" Then           'if there is blank one already do not insert
                WebDropDownList.Items.Insert(1, New ListItem)
            End If
            WebDropDownList.OldText = Param
        End If
    End Sub

#End Region

End Class