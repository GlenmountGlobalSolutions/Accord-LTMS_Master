Imports System.Data.SqlClient

Public Class ShippingTemplateConfiguration
    Inherits System.Web.UI.Page

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                lblSelectBroadcastPointID.Text = My.Resources.txtBroadcastPointCaption + ":"
                DataAccess.BroadcastPointID.Load_ddlBroadcastPointID(Me.ddlBroadcastPointID, Request, Server)

                RefreshTemplateConfigurations()
            End If
            GetBroadcastPointKey()
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


    Private Sub ddlBroadcastPointID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlBroadcastPointID.SelectedIndexChanged
        DataAccess.BroadcastPointID.Set_ddlBroadcastPoint_Cookie(Me.ddlBroadcastPointID.SelectedValue, Response)
        RefreshTemplateConfigurations()
    End Sub

    Private Sub ddlTemplatesNew_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTemplatesNew.SelectedIndexChanged
        'synchronize text box
        tbTemplateNew.Text = ddlTemplatesNew.SelectedItem.Text
    End Sub

    Private Sub ddlTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTemplates.SelectedIndexChanged
        Try
            ClearTextboxes()
            LoadTimesListBox()
            ddlTemplatesNew.SelectedIndex = ddlTemplates.SelectedIndex
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub lbTTimes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbTTimes.SelectedIndexChanged
        Try
            ClearTextboxes()
            LoadTemplateData()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try

            Dim tID As String = ""
            Dim index As String = ""
            Dim status As String = ""
            Dim errorMsg As String = ""

            If (ValidateDialogInput()) Then
                'Save the data and return templateID, Shipping index, savestatus, and errormsg.
                InsertNewTemplate(tID, index, status, errorMsg)

                If Not (Convert.ToBoolean(status)) Then
                    Master.Msg = "Error: unable to create new Delivery Time.<br>S.P. Status: " + status + ".<br>S.P. Message: " + errorMsg
                Else
                    LoadTemplatesDDL()
                    Me.ddlTemplates.SelectedIndex = Me.ddlTemplates.Items.IndexOf(Me.ddlTemplates.Items.FindByValue(tID))

                    LoadTimesListBox()
                    Me.lbTTimes.SelectedIndex = Me.lbTTimes.Items.IndexOf(Me.lbTTimes.Items.FindByValue(index))

                    LoadTemplateData()
                End If
            Else
                Master.eMsg("Error: Unable to create new Delivery Time.")
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally

        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        UpdateTemplate()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            If (Me.ddlTemplates.SelectedIndex >= 0) And (Me.lbTTimes.SelectedIndex >= 0) Then

                'DELETE FROM tblPSDeliveryTemplateTimes WHERE (DeliveryTemplateID = 1) AND (DeliveryTimeIndex = 99.9)
                Dim sql As String = "DELETE FROM tblPSDeliveryTemplateTimes WHERE "
                sql += "(DeliveryTemplateID = " + Me.ddlTemplates.SelectedValue + ") "
                sql += "AND (DeliveryTimeIndex = " + Me.lbTTimes.SelectedValue + ")"

                DA.ExecSQL(sql)

                Master.tMsg("Delete", "Template: " + Me.lbTTimes.SelectedItem.Text + " was deleted.")

                'delete template if it contains only 1 item
                If (Me.lbTTimes.Items.Count <= 1) Then
                    sql = "DELETE FROM tblPSDeliveryTemplates WHERE "
                    sql += "(DeliveryTemplateID = " + Me.ddlTemplates.SelectedValue + ") "
                    DA.ExecSQL(sql)
                    LoadTemplatesDDL()
                End If

                LoadTimesListBox()
                ClearTextboxes()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub rblTemplateNew_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rblTemplateNew.SelectedIndexChanged
        Try
            If (Me.rblTemplateNew.SelectedValue = "0") Then
                Me.tbTemplateNew.Visible = False
                Me.ddlTemplatesNew.Visible = True

                Me.tbIndexNew.Text = Me.tbIndex.OldText
                Me.tbArrivalNew.Text = Me.tbArrival.OldText
                Me.tbSeqNew.Text = Me.tbSeq.OldText
                Me.tbShipNew.Text = Me.tbShip.OldText
                Me.ddlDriverNew.SelectedValue = Me.ddlDriver.OldText

            Else
                Me.tbTemplateNew.Visible = True
                Me.ddlTemplatesNew.Visible = False

                Me.tbIndexNew.Text = ""
                Me.tbArrivalNew.Text = ""
                Me.tbSeqNew.Text = ""
                Me.tbShipNew.Text = ""
                Me.ddlDriverNew.SelectedIndex = -1
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub



#End Region

#Region "Methods"

    Private Sub RefreshTemplateConfigurations()
        ClearTextboxes()
        LoadTemplatesDDL()
        LoadTimesListBox()
        LoadDriverDDL()
    End Sub

    Enum procGetBroadcastPoints
        BroadcastPointID
        Description
        ImageName
        DefaultDailyBuildQuantityShip
        DefaultDailyBuildQuantityJob
        defaultSelection
    End Enum


    Private Sub GetBroadcastPointKey()
        Dim dsBroadCastPoint As DataSet
        Dim cnt As Integer = 1
        Try
            dsBroadCastPoint = DA.GetDataSet("procGetBroadcastPoints")
            If dsBroadCastPoint.Tables.Count > 0 Then

                For Each row As DataRow In dsBroadCastPoint.Tables(0).Rows

                    'Select Case CInt(row.Item(procGetBroadcastPoints.BroadcastPointID))
                    Select Case cnt
                        Case 2
                            UpdatePanelBroadcastPoint2(row.Item(procGetBroadcastPoints.Description).ToString(), row.Item(procGetBroadcastPoints.ImageName).ToString())
                        Case 3
                            UpdatePanelBroadcastPoint3(row.Item(procGetBroadcastPoints.Description).ToString(), row.Item(procGetBroadcastPoints.ImageName).ToString())
                        Case 4
                            UpdatePanelBroadcastPoint4(row.Item(procGetBroadcastPoints.Description).ToString(), row.Item(procGetBroadcastPoints.ImageName).ToString())
                        Case Else
                            UpdatePanelBroadcastPoint1(row.Item(procGetBroadcastPoints.Description).ToString(), row.Item(procGetBroadcastPoints.ImageName).ToString())
                    End Select

                    cnt = cnt + 1
                Next
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub UpdatePanelBroadcastPoint1(description As String, imageName As String)
        lblBP1.Text = description
        Panel1.Visible = True
        image1.Attributes("src") = "../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint2(description As String, imageName As String)
        lblBP2.Text = description
        Panel2.Visible = True
        image2.Attributes("src") = "../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint3(description As String, imageName As String)
        lblBP3.Text = description
        Panel3.Visible = True
        image3.Attributes("src") = "../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint4(description As String, imageName As String)
        lblBP4.Text = description
        Panel4.Visible = True
        image4.Attributes("src") = "../../Images/Misc/" + imageName
    End Sub

    Private Function ValidateInput() As Boolean
        Dim bResult As Boolean = False
        Try
            'check for changes
            If Me.tbIndex.Text = Me.tbIndex.OldText AndAlso _
                Me.tbShip.Text = Me.tbShip.OldText AndAlso _
                Me.tbArrival.Text = Me.tbArrival.OldText AndAlso _
                Me.tbSeq.Text = Me.tbSeq.OldText AndAlso _
                Me.ddlDriver.SelectedItem.Text = Me.ddlDriver.OldText Then

                Master.Msg = "Save canceled.  Nothing was changed."
            Else
                If (Not ValidateTime(Me.tbSeq.Text)) Then
                    Master.Msg = "Error: Invalid Sequence Time. Valid format is - HH:MM xM"
                    Me.imgSeq.Visible = True
                Else
                    bResult = True
                    Me.imgSeq.Visible = False
                End If

                If (Not ValidateTime(Me.tbArrival.Text)) Then
                    Master.Msg = "Error: Invalid Arrival Time. Valid format is - HH:MM xM"
                    Me.imgArrival.Visible = True
                Else
                    bResult = True And bResult
                    Me.imgArrival.Visible = False
                End If

                If (Not ValidateTime(Me.tbShip.Text)) Then
                    Master.Msg = "Error: Invalid Ship Time. Valid format is - HH:MM xM"
                    Me.imgShip.Visible = True
                Else
                    bResult = True And bResult
                    Me.imgShip.Visible = False
                End If

                If (IsNumeric(Me.tbIndex.Text)) Then
                    Dim s As Single = Convert.ToSingle(Me.tbIndex.Text)
                    If (s < 0) Or (s >= 100) Then
                        Master.Msg = "Error: Invalid Index. Index must be a number > 0 and < 100"
                        Me.imgIndex.Visible = True
                    Else
                        bResult = True And bResult
                        Me.imgIndex.Visible = False
                    End If
                Else
                    Master.Msg = "Error: Invalid Index. Index must be a number > 0 and < 100"
                    Me.imgIndex.Visible = True
                End If
            End If


        Catch ex As Exception
            Master.Msg = "Please fix the listed errors."
            Me.imgIndex.Visible = True
        End Try

        Return bResult

    End Function

    Public Shared Function ValidateTime(ByVal time As String) As Boolean
        Try
            Dim status As Boolean = True

            '/*
            '*   Regex:              ^((([0]?[1-9]|1[0-2])(:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))$
            '*   Description:        Enforces HH:MM ?M format
            '*   Derived from:       http://regexlib.com/REDetails.aspx?regexp_id=144
            '*
            '*   HH validation:      ([0]?[1-9]|1[0-2]) = allows 1-9 or 10, 11, 12.
            '*   : validation:       (:)
            '*   MM validation:      [0-5][0-9] = allows 00-59.
            '*   " ?M" validation:   ( )?(AM|am|aM|Am|PM|pm|pM|Pm) = enforces one space and any possible way to write AM/PM.
            '*
            '*  NOTE:  This save regex is used in JAVASCRIPT to validate the NEW SHIPPING ENTRY dialog.
            '*/
            Dim pattern As String = "^((([0]?[1-9]|1[0-2])(:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))$"

            If Not Regex.IsMatch(time, pattern) Then
                status = False
            End If

            Return status
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ValidateDialogInput() As Boolean
        Try
            Dim status As Boolean = True

            If (Not ValidateTime(Me.tbSeqNew.Text)) Then
                status = False And status
            End If
            If (Not ValidateTime(Me.tbArrivalNew.Text)) Then
                status = False And status
            End If
            If (Not ValidateTime(Me.tbShipNew.Text)) Then
                status = False And status
            End If

            If (IsNumeric(Me.tbIndexNew.Text)) Then
                Dim s As Single = Convert.ToSingle(Me.tbIndexNew.Text)
                If (s < 0) Or (s >= 100) Then
                    status = False And status
                End If
            Else
                status = False And status
            End If

            Return status
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function


    Private Sub LoadDriverDDL()
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        Try
            Me.ddlDriver.Items.Clear()
            Me.ddlDriverNew.Items.Clear()

            oSqlParameter = New SqlParameter("@ParameterListID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = "51"
            colParameters.Add(oSqlParameter)

            ds = DA.GetDataSet("procGetParameterListValues", colParameters)

            If (DA.IsDataSetNotEmpty(ds)) Then
                Me.ddlDriver.DataSource = ds
                Me.ddlDriver.DataTextField = "ParameterListValue"
                Me.ddlDriver.DataValueField = "ParameterListValue"
                Me.ddlDriver.DataBind()
                Me.ddlDriver.Items.Insert(0, "")
                Me.ddlDriver.SelectedIndex = 0

                Me.ddlDriverNew.DataSource = ds.Copy
                Me.ddlDriverNew.DataTextField = "ParameterListValue"
                Me.ddlDriverNew.DataValueField = "ParameterListValue"
                Me.ddlDriverNew.DataBind()
                Me.ddlDriverNew.Items.Insert(0, "")
                Me.ddlDriverNew.SelectedIndex = 0
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadTemplatesDDL()
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        Try
            Me.ddlTemplates.Items.Clear()
            Me.ddlTemplatesNew.Items.Clear()

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
            colParameters.Add(oSqlParameter)

            ds = DA.GetDataSet("procGetDeliveryTemplates", colParameters)

            If (DA.IsDataSetNotEmpty(ds)) Then
                Me.ddlTemplates.DataSource = ds
                Me.ddlTemplates.DataTextField = "Description"
                Me.ddlTemplates.DataValueField = "DeliveryTemplateID"
                Me.ddlTemplates.DataBind()

                Me.ddlTemplatesNew.DataSource = ds.Copy
                Me.ddlTemplatesNew.DataTextField = "Description"
                Me.ddlTemplatesNew.DataValueField = "DeliveryTemplateID"
                Me.ddlTemplatesNew.DataBind()

                'synchronize textbox
                Me.tbTemplateNew.Text = ddlTemplatesNew.SelectedItem.Text
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Sub LoadTimesListBox()
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Try
            Me.lbTTimes.Items.Clear()

            If (Me.ddlTemplates.SelectedIndex >= 0) Then

                oSqlParameter = New SqlParameter("@DeliveryTemplateID", SqlDbType.Int)
                oSqlParameter.Value = Me.ddlTemplates.SelectedValue
                colParameters.Add(oSqlParameter)

                ds = DA.GetDataSet("procGetDeliveryTemplateTimes", colParameters)

                If (DA.IsDataSetNotEmpty(ds)) Then
                    Me.lbTTimes.DataSource = ds
                    Me.lbTTimes.DataTextField = "Description"
                    Me.lbTTimes.DataValueField = "DeliveryTimeIndex"
                    Me.lbTTimes.DataBind()
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())

        End Try
    End Sub

    Private Sub LoadTemplateData()
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim strDriver As String
        Try

            If (Me.ddlTemplates.SelectedIndex >= 0) And (Me.lbTTimes.SelectedIndex >= 0) Then

                oSqlParameter = New SqlParameter("@DeliveryTemplateID", SqlDbType.Int)
                oSqlParameter.Value = Me.ddlTemplates.SelectedValue
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@DeliveryTimeIndex", SqlDbType.Decimal)
                oSqlParameter.Value = Me.lbTTimes.SelectedValue
                colParameters.Add(oSqlParameter)

                ds = DA.GetDataSet("procGetDeliveryTemplateTimes", colParameters)

                If (DA.IsDataSetNotEmpty(ds)) Then
                    With ds.Tables(0).Rows(0)
                        Me.tbIndex.Text = .Item("DeliveryTimeIndex").ToString()
                        Me.tbArrival.Text = .Item("ArrivalTime").ToString()
                        Me.tbSeq.Text = .Item("ApproximateSequenceTime").ToString()
                        Me.tbShip.Text = .Item("ShipTime").ToString()

                        strDriver = .Item("Driver").ToString()

                        'select driver
                        Dim i As Integer = -1
                        i = Me.ddlDriver.Items.IndexOf(Me.ddlDriver.Items.FindByText(strDriver))
                        If (i < 0) Then
                            Me.ddlDriver.Items.Insert(1, strDriver)
                            Me.ddlDriver.Items(1).Value = strDriver
                            i = 0
                        End If
                        Me.ddlDriver.SelectedIndex = i


                        Me.tbIndex.OldText = .Item("DeliveryTimeIndex").ToString()
                        Me.tbArrival.OldText = .Item("ArrivalTime").ToString()
                        Me.tbSeq.OldText = .Item("ApproximateSequenceTime").ToString()
                        Me.tbShip.OldText = .Item("ShipTime").ToString()
                        Me.ddlDriver.OldText = .Item("Driver").ToString()

                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                        'XXXX   Dialog New
                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                        Me.tbIndexNew.Text = .Item("DeliveryTimeIndex").ToString()
                        Me.tbArrivalNew.Text = .Item("ArrivalTime").ToString()
                        Me.tbSeqNew.Text = .Item("ApproximateSequenceTime").ToString()
                        Me.tbShipNew.Text = .Item("ShipTime").ToString()

                        strDriver = .Item("Driver").ToString()

                        'select driver
                        i = -1
                        i = Me.ddlDriverNew.Items.IndexOf(Me.ddlDriverNew.Items.FindByText(strDriver))
                        If (i < 0) Then
                            Me.ddlDriverNew.Items.Insert(1, strDriver)
                            Me.ddlDriverNew.Items(1).Value = strDriver
                            i = 0
                        End If
                        Me.ddlDriverNew.SelectedIndex = i


                        Me.tbIndexNew.OldText = .Item("DeliveryTimeIndex").ToString()
                        Me.tbArrivalNew.OldText = .Item("ArrivalTime").ToString()
                        Me.tbSeqNew.OldText = .Item("ApproximateSequenceTime").ToString()
                        Me.tbShipNew.OldText = .Item("ShipTime").ToString()
                        Me.ddlDriverNew.OldText = .Item("Driver").ToString()

                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                        'XXXX   END of Dialog New
                        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    End With
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ClearTextboxes()
        Me.tbArrival.Text = ""
        Me.tbIndex.Text = ""
        Me.tbSeq.Text = ""
        Me.tbShip.Text = ""
        If (Me.ddlDriver.Items.Count > 0) Then
            Me.ddlDriver.SelectedIndex = 0
        Else
            Me.ddlDriver.SelectedIndex = -1
        End If


        Me.tbArrivalNew.Text = ""
        Me.tbIndexNew.Text = ""
        Me.tbSeqNew.Text = ""
        Me.tbShipNew.Text = ""
        If (Me.ddlDriverNew.Items.Count > 0) Then
            Me.ddlDriverNew.SelectedIndex = 0
        Else
            Me.ddlDriverNew.SelectedIndex = -1
        End If
    End Sub

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdDelete)
            Master.Secure(Me.cmdNew)
            Master.Secure(Me.cmdSave)

            If ddlBroadcastPointID.SelectedIndex <= 0 Then
                cmdNew.Enabled = False
            End If

            If lbTTimes.SelectedItem Is Nothing Then
                cmdSave.Enabled = False
                cmdDelete.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub UpdateTemplate()
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New Collections.Generic.List(Of System.Data.SqlClient.SqlParameter)
        Dim colOutput As Collections.Generic.List(Of System.Data.SqlClient.SqlParameter)

        Try
            If (ValidateInput()) Then

                If (Me.ddlDriver.SelectedIndex < 0) Then
                    Me.ddlDriver.SelectedIndex = 0
                End If

                oSqlParameter = New SqlParameter("@DeliveryTemplateID", SqlDbType.Int)
                oSqlParameter.Value = Me.ddlTemplates.SelectedValue
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@DeliveryTimeIndex", SqlDbType.Decimal)
                oSqlParameter.Value = Me.tbIndex.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ShipTime", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.tbShip.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ArrivalTime", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.tbArrival.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ApproximateSequenceTime", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.tbSeq.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Driver", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.ddlDriver.SelectedItem.Text
                colParameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("procUpdateDeliveryTemplateTime", colParameters)

                If Me.tbIndex.Text <> Me.tbIndex.OldText Then
                    Me.imgIndexCk.Visible = True
                End If
                If Me.tbShip.Text <> Me.tbShip.OldText Then
                    Me.imgShipCk.Visible = True
                End If
                If Me.tbArrival.Text <> Me.tbArrival.OldText Then
                    Me.imgArrivalCk.Visible = True
                End If
                If Me.tbSeq.Text <> Me.tbSeq.OldText Then
                    Me.imgSeqCk.Visible = True
                End If
                If Me.ddlDriver.SelectedItem.Text <> Me.ddlDriver.OldText Then
                    Me.imgDriverCk.Visible = True
                End If

                Master.tMsg("Save", "Information for Delivery Template: " + Me.lbTTimes.SelectedItem.Text + " has been updated.")

                'reload the data
                LoadTimesListBox()
                'research for entry incase the index id was changed.
                Me.lbTTimes.SelectedIndex = Me.lbTTimes.Items.IndexOf(Me.lbTTimes.Items.FindByValue(Me.tbIndex.Text))
                LoadTemplateData()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub InsertNewTemplate(ByRef templateId As String, ByRef indexId As String, ByRef spStatus As String, ByRef spErrorMsg As String)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New Collections.Generic.List(Of System.Data.SqlClient.SqlParameter)
        Dim colOutput As Collections.Generic.List(Of System.Data.SqlClient.SqlParameter)

        Try
            If ddlBroadcastPointID.SelectedIndex <= 0 Then
                Master.Msg = "Error: Please select a " + My.Resources.txtBroadcastPointCaption + "."
            Else
                oSqlParameter = New SqlParameter("@TemplateDesc", SqlDbType.VarChar, 80)
                If (Me.rblTemplateNew.SelectedValue = "0") Then
                    oSqlParameter.Value = Me.ddlTemplatesNew.SelectedItem.Text
                Else
                    oSqlParameter.Value = Me.tbTemplateNew.Text
                End If
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@DeliveryTimeIndex", SqlDbType.Decimal)
                oSqlParameter.Value = Me.tbIndexNew.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ShipTime", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.tbShipNew.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ArrivalTime", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.tbArrivalNew.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ApproximateSequenceTime", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.tbSeqNew.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Driver", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.ddlDriverNew.SelectedItem.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
                oSqlParameter.Value = Me.ddlBroadcastPointID.SelectedValue
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@DeliveryTemplateID", SqlDbType.Int)
                oSqlParameter.Direction = ParameterDirection.Output
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                colParameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("procInsertDeliveryTemplateTime", colParameters)

                indexId = Me.tbIndexNew.Text
                templateId = ""
                spErrorMsg = ""
                spStatus = ""
                For Each par As SqlParameter In colOutput
                    If par.ParameterName = "@DeliveryTemplateID" Then
                        templateId = par.Value.ToString()
                    End If
                    If par.ParameterName = "@Status" Then
                        spStatus = par.Value.ToString()
                    End If
                    If par.ParameterName = "@ErrorMsg" Then
                        spErrorMsg = par.Value.ToString()
                    End If
                Next


            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

End Class