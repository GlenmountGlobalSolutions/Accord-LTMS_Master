Imports System.Data.SqlClient
Imports System.Xml

Public Class OperatorDisplayConfiguration
    Inherits System.Web.UI.Page

    Const cUpOne As Integer = -1
    Const cDownOne As Integer = 1

#Region "Event Handlers"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                ResetPage()
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

    Private Sub ddConfigurationTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddConfigurationTypes.SelectedIndexChanged
        Try
            ResetConfigurations()
            ResetSavedValues()

            If ddConfigurationTypes.SelectedIndex > 0 Then
                LoadConfigurations(CInt(ddConfigurationTypes.SelectedValue))
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbConfigurations_PreRender(sender As Object, e As System.EventArgs) Handles lbConfigurations.PreRender
        For Each item As ListItem In lbConfigurations.Items
            item.Attributes.Add("title", "ConfigurationID: " + item.Value)
        Next
    End Sub

    Private Sub lbConfigurations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbConfigurations.SelectedIndexChanged
        Try
            If lbConfigurations.SelectedIndex > -1 Then

                ResetParameters()
                ResetSavedValues()

                LoadConfigurationParameters(CInt(ddConfigurationTypes.SelectedValue), CInt(lbConfigurations.SelectedValue))
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibDown_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibDown.Click
        Try

            Dim configurationTypeID As Integer = CInt(ddConfigurationTypes.SelectedValue)
            Dim configurationID As Integer = CInt(lbConfigurations.SelectedValue)
            Dim configurationText As String = lbConfigurations.SelectedItem.Text

            If (Me.lbConfigurations.SelectedIndex > (Me.lbConfigurations.Items.Count - 1)) Then
                Master.Alert = "Cannot move item down."
            Else
                ResetParameters()
                ResetSavedValues()

                ChangeConfigurationDisplayID(configurationID, cDownOne)

                'reload the page
                ResetPage()
                ddConfigurationTypes.SelectedIndex = ddConfigurationTypes.Items.IndexOf(ddConfigurationTypes.Items.FindByValue(configurationTypeID.ToString()))
                LoadConfigurations(configurationTypeID)
                lbConfigurations.SelectedIndex = lbConfigurations.Items.IndexOf(lbConfigurations.Items.FindByValue(configurationID.ToString()))
                LoadConfigurationParameters(configurationTypeID, configurationID)
                Master.tMsg("SAVE", "Configuration [" & configurationText & "] was moved down.")

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibUp_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibUp.Click
        Try
            Dim configurationTypeID As Integer = CInt(ddConfigurationTypes.SelectedValue)
            Dim configurationID As Integer = CInt(lbConfigurations.SelectedValue)
            Dim configurationText As String = lbConfigurations.SelectedItem.Text

            If (lbConfigurations.SelectedIndex < 1) Then
                Master.Alert = "Cannot move item up."
            Else
                ResetParameters()
                ResetSavedValues()

                ChangeConfigurationDisplayID(configurationID, cUpOne)

                'reload the page
                ResetPage()
                ddConfigurationTypes.SelectedIndex = ddConfigurationTypes.Items.IndexOf(ddConfigurationTypes.Items.FindByValue(configurationTypeID.ToString()))
                LoadConfigurations(configurationTypeID)
                lbConfigurations.SelectedIndex = lbConfigurations.Items.IndexOf(lbConfigurations.Items.FindByValue(configurationID.ToString()))
                LoadConfigurationParameters(configurationTypeID, configurationID)

                Master.tMsg("SAVE", "Configuration [" & configurationText & "] was moved up.")

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Dim configurationID As Integer
        Dim configurationTypeID As Integer
        Dim newName As String
        Try
            If ValidateParameters("NEW") Then
                configurationTypeID = CInt(ddNewType.SelectedValue)
                newName = tbNewName.Text

                configurationID = AddConfiguration(newName, configurationTypeID)
                Master.tMsg("SAVE", "Configuration [" & newName & "] was added.")

                'reload the page
                ResetPage()
                ResetSavedValues()
                ddConfigurationTypes.SelectedIndex = ddConfigurationTypes.Items.IndexOf(ddConfigurationTypes.Items.FindByValue(configurationTypeID.ToString()))
                LoadConfigurations(configurationTypeID)
                lbConfigurations.SelectedIndex = lbConfigurations.Items.IndexOf(lbConfigurations.Items.FindByValue(CStr(configurationID)))
                If lbConfigurations.SelectedIndex >= 0 Then
                    LoadConfigurationParameters(configurationTypeID, CInt(lbConfigurations.SelectedValue))
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            If lbConfigurations.SelectedIndex >= 0 Then
                Dim msg As String = ""
                Dim failMsg As String = ""
                Dim xmlDoc As New XmlDocument()
                Dim nodeList As XmlNodeList
                Dim img As Web.UI.WebControls.Image = Nothing

                Dim configurationTypeID As Integer = CInt(ddConfigurationTypes.SelectedValue)
                Dim configurationID As Integer = CInt(lbConfigurations.SelectedValue)
                xmlDoc.InnerXml = hidXmlParameters.Value

                If xmlDoc.FirstChild.HasChildNodes Then

                    'build message text to display changed values.
                    nodeList = xmlDoc.GetElementsByTagName("Parameter")
                    For Each element As XmlElement In nodeList
                        If element.Attributes("newValue").Value <> element.Attributes("oldValue").Value Then
                            msg = msg & element.Attributes("Description").Value & " was changed from [" & element.Attributes("oldValue").Value & "] to [" & element.Attributes("newValue").Value & "]. <br />"
                            failMsg = failMsg & element.Attributes("Description").Value & " [" & element.Attributes("newValue").Value & "] <br />"
                        End If
                    Next

                    If (SaveConfigurationParameters(configurationID, xmlDoc)) Then
                        hidXmlParametersSaved.Value = hidXmlParameters.Value
                        Master.Msg = msg
                    Else
                        hidXmlParametersSaved.Value = ""
                        Master.Msg = "Save Failed: There is already a configuration with the values:<br />" + failMsg
                    End If


                    'reload the page
                    ResetPage()
                    ddConfigurationTypes.SelectedIndex = ddConfigurationTypes.Items.IndexOf(ddConfigurationTypes.Items.FindByValue(configurationTypeID.ToString()))
                    LoadConfigurations(configurationTypeID)
                    lbConfigurations.SelectedIndex = lbConfigurations.Items.IndexOf(lbConfigurations.Items.FindByValue(configurationID.ToString()))
                    LoadConfigurationParameters(configurationTypeID, configurationID)


                End If
            Else
                Master.Msg = "Please select a configuration."
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim configurationTypeID As Integer = CInt(ddConfigurationTypes.SelectedValue)
            Dim configurationID As Integer = CInt(lbConfigurations.SelectedValue)

            If lbConfigurations.SelectedIndex >= 0 Then
                DeleteConfiguration(configurationID)

                'reload the page
                ResetPage()
                ResetSavedValues()
                ddConfigurationTypes.SelectedIndex = ddConfigurationTypes.Items.IndexOf(ddConfigurationTypes.Items.FindByValue(configurationTypeID.ToString()))
                LoadConfigurations(configurationTypeID)
            Else
                Master.Msg = "Please select a configuration."
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"
    Private Function ValidateParameters(ByVal action As String) As Boolean
        Dim passed As Boolean = True

        If action = "NEW" Then
            If Len(tbNewName.Text) < 1 Then
                Master.Msg = "Save canceled. Please enter a new name when creating a new configuration."
                passed = False And passed
            End If
            If ddNewType.SelectedIndex <= 0 Then
                Master.Msg = "Save canceled. Please select a new type when creating a new configuration."
                passed = False And passed
            End If
        ElseIf action = "SAVE" Then

        End If

        Return passed
    End Function

    Private Sub LoadConfigurationTypes()

        Dim ds As DataSet = GetConfigurationTypes()

        With ddConfigurationTypes
            .DataSource = ds
            .DataTextField = "Description"
            .DataValueField = "ConfigurationTypeID"
            .DataBind()
            .Items.Insert(0, "Choose a Type")
        End With

        With ddNewType
            .DataSource = ds.Copy()
            .DataTextField = "Description"
            .DataValueField = "ConfigurationTypeID"
            .DataBind()
            .Items.Insert(0, "Choose a Type")
        End With

    End Sub

    Private Sub LoadConfigurations(ByVal configurationTypeID As Integer)

        Dim ds As DataSet = GetConfigurations(configurationTypeID)
        With lbConfigurations
            .DataSource = ds
            .DataTextField = "Description"
            .DataValueField = "ConfigurationID"
            .DataBind()
        End With

    End Sub

    Private Sub LoadConfigurationParameters(ByVal configurationTypeID As Integer, ByVal configurationID As Integer)

        ResetParameters()

        With dlParameters
            .DataSource = GetConfigurationParameters(configurationTypeID, configurationID)
            .DataBind()
        End With

    End Sub

    Private Sub ResetPage()

        ResetTypes()

        LoadConfigurationTypes()
    End Sub

    Private Sub ResetParameters()

        dlParameters.Controls.Clear()
        hidXmlParameters.Value = ""

        Dim xmlDoc As New XmlDocument()
        xmlDoc.AppendChild(xmlDoc.CreateElement("Parameters"))
        hidXmlParameters.Value = xmlDoc.InnerXml
    End Sub

    Private Sub ResetTypes()

        ddConfigurationTypes.Items.Clear()
        ResetConfigurations()
    End Sub

    Private Sub ResetConfigurations()

        lbConfigurations.Items.Clear()
        ResetParameters()
    End Sub

    Private Sub ResetSavedValues()
        hidXmlParametersSaved.Value = ""

    End Sub

    Private Sub EnableControls()
        Dim buttonsVisible As Boolean = True
        Try
            Master.Secure(Me.cmdNew)
            Master.Secure(Me.cmdSave)
            Master.Secure(Me.cmdDelete)

            If lbConfigurations.SelectedItem Is Nothing Then
                cmdSave.Enabled = False
                cmdDelete.Enabled = False
            End If

            If (lbConfigurations.SelectedIndex < 1) Or lbConfigurations.SelectedItem Is Nothing Then
                ibUp.Enabled = False
            Else
                ibUp.Enabled = True
            End If

            If (Me.lbConfigurations.SelectedIndex > (Me.lbConfigurations.Items.Count - 2)) Or lbConfigurations.SelectedItem Is Nothing Then
                ibDown.Enabled = False
            Else
                ibDown.Enabled = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub Parameter_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim wtb As GGS.WebInputBox
        Dim xmlDoc As New XmlDocument()
        Dim recConfigurationID As Integer
        Dim recValue As String
        Dim recOldValue As String
        Dim recParameterTypeID As Integer
        Dim nodeList As XmlNodeList

        Try
            'get reference to control
            wtb = CType(sender, GGS.WebInputBox)
            xmlDoc.InnerXml = hidXmlParameters.Value

            recConfigurationID = CInt(lbConfigurations.SelectedValue)
            recValue = wtb.Text
            recOldValue = wtb.OldText
            recParameterTypeID = CInt(wtb.InputCaption)

            If recOldValue <> recValue Then
                nodeList = xmlDoc.GetElementsByTagName("Parameter")
                For Each element As XmlElement In nodeList
                    If CInt(element.Attributes("ConfigurationParameterTypeID").Value) = recParameterTypeID Then
                        element.Attributes("newValue").Value = recValue
                    End If
                Next

                hidXmlParameters.Value = xmlDoc.InnerXml
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub Parameter_IndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim wddl As GGS.WebDropDownList
        Dim xmlDoc As New XmlDocument()
        Dim recConfigurationID As Integer
        Dim recValue As String
        Dim recOldValue As String
        Dim recParameterTypeID As Integer
        Dim nodeList As XmlNodeList

        Try
            'get reference to control
            wddl = CType(sender, GGS.WebDropDownList)
            xmlDoc.InnerXml = hidXmlParameters.Value

            recConfigurationID = CInt(lbConfigurations.SelectedValue)
            recValue = wddl.SelectedItem.Text
            recOldValue = wddl.OldText
            recParameterTypeID = CInt(wddl.InputCaption)

            If recOldValue <> recValue Then
                nodeList = xmlDoc.GetElementsByTagName("Parameter")
                For Each element As XmlElement In nodeList
                    If CInt(element.Attributes("ConfigurationParameterTypeID").Value) = recParameterTypeID Then
                        element.Attributes("newValue").Value = recValue
                    End If
                Next

                hidXmlParameters.Value = xmlDoc.InnerXml
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Function DLControlBind(ByVal e As Object) As Object
        Dim inputbox As GGS.WebInputBox = Nothing
        Dim ddList As GGS.WebDropDownList = Nothing
        Dim imgCheck As New System.Web.UI.WebControls.Image
        Dim xmldoc As New XmlDocument()
        Dim oldXmlDox As New XmlDocument()
        Dim nodeList As XmlNodeList
        Dim child As XmlElement
        Dim dlItem As DataListItem
        Dim drv As DataRowView
        Dim parameterLabel As String
        Dim parameterValue As String
        Dim ParameterListID As String
        Dim parameterTypeID As Integer
        Dim parameterConfigurationID As Integer

        Try
            dlItem = CType(e, DataListItem)
            drv = CType(dlItem.DataItem, DataRowView)

            'get columns supplied for the record
            parameterLabel = drv("Description").ToString()
            parameterValue = drv("Value").ToString()
            ParameterListID = drv("ParameterListID").ToString()
            If drv("ConfigurationID") IsNot DBNull.Value Then
                parameterConfigurationID = CInt(drv("ConfigurationID"))
            End If
            If drv("ConfigurationParameterTypeID") IsNot DBNull.Value Then
                parameterTypeID = CInt(drv("ConfigurationParameterTypeID"))
            End If

            'get references to each control
            For Each ctrl As Control In dlItem.Controls
                Select Case ctrl.ToString()
                    Case "GGS.WebDropDownList"
                        ddList = CType(ctrl, GGS.WebDropDownList)

                    Case "GGS.WebInputBox"
                        inputbox = CType(ctrl, GGS.WebInputBox)

                    Case "System.Web.UI.WebControls.Image"
                        imgCheck = CType(ctrl, Image)
                End Select
            Next

            'if ParameterListID is empty display textbox else dropbox
            If Len(ParameterListID) > 0 Then
                'displaying dropbox
                inputbox.Visible = False
                ddList.Visible = True

                'binding databox to source
                dbParameterListID_Bind(ddList, Trim(ParameterListID), Trim(parameterValue), False)
            Else
                'displaying textbox
                ddList.Visible = False
                inputbox.Visible = True
            End If

            inputbox.OldText = parameterValue
            inputbox.Text = parameterValue
            inputbox.ToolTip = "Configuration Parameter Type ID = " & parameterTypeID.ToString() & ""
            inputbox.InputCaption = parameterTypeID.ToString()

            ddList.OldText = parameterValue
            ddList.ToolTip = "Configuration Parameter Type ID = " & parameterTypeID.ToString() & ""
            ddList.InputCaption = parameterTypeID.ToString()

            xmldoc.InnerXml = hidXmlParameters.Value

            'create new node
            child = xmldoc.CreateElement("Parameter")
            child.SetAttribute("ConfigurationID", parameterConfigurationID.ToString())
            child.SetAttribute("ConfigurationParameterTypeID", parameterTypeID.ToString())
            child.SetAttribute("newValue", parameterValue)
            child.SetAttribute("oldValue", parameterValue)
            child.SetAttribute("Description", parameterLabel)

            'append new node to the document
            xmldoc.FirstChild.AppendChild(child)

            'save the document
            hidXmlParameters.Value = xmldoc.InnerXml

            'dispaly check mark if it's a recently saved item.
            If Len(hidXmlParametersSaved.Value.ToString()) > 0 Then
                oldXmlDox.InnerXml = hidXmlParametersSaved.Value
                nodeList = oldXmlDox.GetElementsByTagName("Parameter")
                For Each element As XmlElement In nodeList
                    If element.Attributes("newValue").Value <> element.Attributes("oldValue").Value AndAlso CInt(element.Attributes("ConfigurationParameterTypeID").Value) = parameterTypeID Then
                        imgCheck.Visible = True
                    End If
                Next
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return Nothing
    End Function

    Private Sub dbParameterListID_Bind(ByRef ddList As GGS.WebDropDownList, ByRef paramID As String, ByRef Param As String, ByVal bSwapChr As Boolean)
        Dim strSQL As String

        strSQL = "SELECT tblParameterListValues.ParameterListValue " & _
                 "FROM tblParameterLists " & _
                 "INNER JOIN tblParameterListValues " & _
                 "ON tblParameterLists.ParameterListID = tblParameterListValues.ParameterListID " & _
                 "WHERE (tblParameterLists.ParameterListID = '" & paramID & "')"

        ddList.DataSource = DA.GetDataSet(strSQL)
        ddList.DataTextField = "ParameterListValue"
        ddList.DataValueField = "ParameterListValue"
        ddList.DataBind()
        ddList.TabIndex.ToString()

        If bSwapChr Then
            ddList.Items.Insert(0, FormatForSaveOrDisplay(Param, False))
            ddList.OldText = Param
            If Not Param = "" Then        'if there is blank one already do not insert
                ddList.Items.Insert(1, New ListItem)
            End If
        Else
            ddList.Items.Insert(0, Param)
            ddList.OldText = Param
            If Not Param = "" Then        'if there is blank one already do not insert
                ddList.Items.Insert(1, New ListItem)
            End If
        End If
    End Sub

    Private Function FormatForSaveOrDisplay(ByRef str As String, ByRef bfrmt As Boolean) As String
        Dim dschar As DataSet
        Dim dr As DataRow

        dschar = DA.GetDataSet("SELECT *  FROM tblCharacterSwap")
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
#End Region

#Region "Database Methods"

    Private Function GetConfigurationTypes() As DataSet
        Dim parameters As New List(Of SqlParameter)
        Dim ds As DataSet = Nothing
        Try
            parameters.Add(New SqlParameter("@ConfigurationTypeID", DBNull.Value))
            ds = DA.GetDataSet("[ods].[procSelectConfigurationTypes]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function

    Private Function GetConfigurations(ByVal configurationTypeID As Integer) As DataSet
        Dim parameters As New List(Of SqlParameter)
        Dim ds As DataSet = Nothing
        Try
            parameters.Add(New SqlParameter("@ConfigurationTypeID", configurationTypeID))
            ds = DA.GetDataSet("[ods].[procSelectConfigurations]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function

    Private Function GetConfigurationParameters(ByVal configurationTypeID As Integer, ByVal configurationID As Integer) As DataSet
        Dim parameters As New List(Of SqlParameter)
        Dim ds As DataSet = Nothing
        Try
            parameters.Add(New SqlParameter("@ConfigurationTypeID", configurationTypeID))
            parameters.Add(New SqlParameter("@ConfigurationID", configurationID))
            ds = DA.GetDataSet("[ods].[procSelectConfigurationParameters]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function

    Private Sub DeleteConfiguration(ByVal configurationID As Integer)
        Dim bResult As Boolean = False
        Dim parameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim status As String = ""
        Dim message As String = ""

        Try
            parameters.Add(New SqlParameter("@ConfigurationID", configurationID))
            colOutput = DA.ExecSP("[ods].[procDeleteConfiguration]", parameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            Boolean.TryParse(status, bResult)

            If (bResult = False) Then
                Master.tMsg("ODS - Delete Configuration Parameters", message)
            Else
                Master.tMsg("ODS - Delete Configuration Parameters", "Configuration [" & lbConfigurations.SelectedItem.Text & "] was deleted.")
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ChangeConfigurationDisplayID(ByVal configurationID As Integer, ByVal delta As Integer)
        Dim parameters As New List(Of SqlParameter)
        Try
            parameters.Add(New SqlParameter("@ConfigurationID", configurationID))
            parameters.Add(New SqlParameter("@delta", delta))
            parameters.Add(New SqlParameter("@ModifiedBy", Page.User.Identity.Name.ToString()))
            DA.ExecSP("[ods].[procUpdateConfigurationDisplayID]", parameters)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function AddConfiguration(ByVal description As String, ByVal configurationTypeID As Integer) As Integer
        Dim configurationID As Integer

        Dim parameters As New List(Of SqlParameter)
        Dim oParameter As SqlParameter
        Dim colOutput As List(Of SqlParameter)
        Try
            parameters.Add(New SqlParameter("@Description", description))
            parameters.Add(New SqlParameter("@ConfigurationTypeID", configurationTypeID))
            parameters.Add(New SqlParameter("@ModifiedBy", Page.User.Identity.Name.ToString()))

            oParameter = New SqlParameter("@ConfigurationID", SqlDbType.Int)
            oParameter.Direction = ParameterDirection.Output
            parameters.Add(oParameter)

            colOutput = DA.ExecSP("[ods].[procInsertConfiguration]", parameters)

            For Each oParameter In colOutput
                If oParameter.Direction = ParameterDirection.Output And oParameter.ParameterName = "@ConfigurationID" Then
                    configurationID = CInt(oParameter.Value)
                End If
            Next
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return configurationID

    End Function

    Private Function SaveConfigurationParameters(ByVal configurationID As Integer, ByVal xmlParameters As XmlDocument) As Boolean
        Dim bResult As Boolean = False
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim status As String = ""
        Dim message As String = ""

        Try
            colParameters.Add(New SqlParameter("@ConfigurationID", configurationID))
            colParameters.Add(New SqlParameter("@xmlChangedParameters", xmlParameters.InnerXml.ToString()))
            colParameters.Add(New SqlParameter("@ModifiedBy", Page.User.Identity.Name.ToString()))

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("[ods].[procSaveConfigurationParameters]", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            Boolean.TryParse(status, bResult)

            If (bResult = False) Then
                Master.tMsg("ODS - Save Configuration Parameters", message)
            Else
                Master.tMsg("ODS - Save Configuration Parameters", "Save Configuration Parameters" + xmlParameters.InnerXml.ToString() + " have been updated.")
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult
    End Function

#End Region

End Class