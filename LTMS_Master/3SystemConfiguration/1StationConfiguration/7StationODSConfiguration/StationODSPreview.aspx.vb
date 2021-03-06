Imports System.Data.SqlClient

Public Class StationODSPreview
    Inherits System.Web.UI.Page

#Region "Properties and Enums"


    ' See table tblConfigurationTypes
    Enum ConfigurationTypes
        InstructionSet = 1
        InstructionStepFilters = 2
        AuxiliaryListA = 3
        AuxiliaryListB = 4
    End Enum

    Enum GridInstructionsColumn
        StepID
        SeeEDS
        SpecificQuality
        SeqControlFlag
        ImgSeeEDS
        ImgSpecificQuality
        ImgSeqControlFlag
        Description
    End Enum

    Enum GridMCRInfo
        MCRName
        MCRValue
    End Enum

    Enum MCRDatasetColumns
        MasterChangeRequestID = 0
        MCRNumber
        StationMCRNumber
        MCRDate
        MCRStatus_ConfigurationID
        MCRStatus
        IsActiveOnClient
        MCRDescription
        MCRNumberTextValue
    End Enum

    Private ReadOnly Property LineID As Integer
        Get
            Dim id As Integer = -1
            Integer.TryParse(ddlLineNumbers.SelectedValue, id)
            Return id
        End Get
    End Property

    Private ReadOnly Property StationID As String
        Get
            Return ddlStations.SelectedValue
        End Get
    End Property

    Private ReadOnly Property VehicleModelID As Integer
        Get
            Dim id As Integer = -1
            Integer.TryParse(ddlVehicleModels.SelectedValue, id)
            Return id
        End Get
    End Property

#End Region


#Region "Events Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                LoadConfigurationTypeLabels()
                LoadFilterDropDownLists()
                divSimulateForm.Visible = False

                '#If DEBUG Then
                '                ddlLineNumbers.SelectedValue = "7"
                '                GenerateODSDisplay()
                '                ddlStations.SelectedValue = "0701"
                '                GenerateODSDisplay()
                '                ddlVehicleModels.SelectedValue = "190"
                '                GenerateODSDisplay()
                '                ddlMCRNumber.SelectedValue = "78"
                '                GenerateODSDisplay()
                '                ddlModels.SelectedValue = "A1"
                '                GenerateODSDisplay()
                '                ddlProductCode.SelectedValue = "E10U-B"
                '                GenerateODSDisplay()
                '#End If

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub StationODSPreview_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        EnableControls()
    End Sub


    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        GenerateODSDisplay()
    End Sub


    Private Sub ddlLineNumbers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlLineNumbers.SelectedIndexChanged
        GenerateODSDisplay()
    End Sub

    Private Sub ddlStations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStations.SelectedIndexChanged
        GenerateODSDisplay()
    End Sub

    Private Sub ddlVehicleModels_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlVehicleModels.SelectedIndexChanged
        GenerateODSDisplay()
    End Sub

    Private Sub ddlMCRNumber_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMCRNumber.SelectedIndexChanged
        GenerateODSDisplay()
    End Sub

    Private Sub ddlModels_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlModels.SelectedIndexChanged
        GenerateODSDisplay()
    End Sub

    Private Sub ddlProductCode_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlProductCode.SelectedIndexChanged
        GenerateODSDisplay()
    End Sub

    Private Sub ddlDisplay_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDisplay.SelectedIndexChanged
        If ddlDisplay.SelectedValue = "0" Then
            pnlODS.Visible = True
            pnlPPETools.Visible = True
            pnlAbCntrl.Visible = True
            pnlEDS.Visible = False
        Else
            pnlODS.Visible = False
            pnlPPETools.Visible = False
            pnlAbCntrl.Visible = False
            pnlEDS.Visible = True
        End If
        GenerateODSDisplay()
    End Sub



    Private Sub gvInstructions_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInstructions.RowDataBound
        Dim img As Image
        Dim boolFlag As Boolean
        Dim color As Drawing.Color

        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                boolFlag = (IsDBNull(DataBinder.Eval(e.Row.DataItem, "SeeEDSFlag")) = False AndAlso CBool(DataBinder.Eval(e.Row.DataItem, "SeeEDSFlag")))
                img = CType(e.Row.FindControl("imgSeeEDSFlag"), Image)
                img.ImageUrl = IIf(boolFlag, "~/Images/ODS/SeeEDS.png", "~/Images/Misc/trans.gif").ToString()

                boolFlag = (IsDBNull(DataBinder.Eval(e.Row.DataItem, "SpecificQualityFlag")) = False AndAlso CBool(DataBinder.Eval(e.Row.DataItem, "SpecificQualityFlag")))
                img = CType(e.Row.FindControl("imgSpecificQualityFlag"), Image)
                img.ImageUrl = IIf(boolFlag, "~/Images/ODS/SpecificQuality.png", "~/Images/Misc/trans.gif").ToString()

                boolFlag = (IsDBNull(DataBinder.Eval(e.Row.DataItem, "SeqControlFlag")) = False AndAlso CBool(DataBinder.Eval(e.Row.DataItem, "SeqControlFlag")))
                img = CType(e.Row.FindControl("imgSeqControlFlag"), Image)
                img.ImageUrl = IIf(boolFlag, "~/Images/ODS/SeqControl.png", "~/Images/Misc/trans.gif").ToString()

                'lookup the color, if not null then format the row
                If (IsDBNull(DataBinder.Eval(e.Row.DataItem, "Color")) = False) Then
                    color = Drawing.Color.FromName(DataBinder.Eval(e.Row.DataItem, "Color").ToString())
                    e.Row.BackColor = color
                End If

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvInstructions_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInstructions.RowCreated
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                e.Row.Cells(GridInstructionsColumn.SeeEDS).ColumnSpan = 3
                e.Row.Cells(GridInstructionsColumn.SpecificQuality).Visible = False
                e.Row.Cells(GridInstructionsColumn.SeqControlFlag).Visible = False
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvMCR_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMCR.RowCreated
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                e.Row.Cells(GridMCRInfo.MCRName).ColumnSpan = 2
                e.Row.Cells(GridMCRInfo.MCRValue).Visible = False
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region


#Region "Private Methods"

    Private Sub EnableControls()
        Try
            Master.Secure(cmdRefresh)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub LoadFilterDropDownLists()
        LoadLines()
        LoadStations()
        LoadVehicleModels()
        LoadMCRs()
        LoadModels()
        LoadProductCodes()
    End Sub


    Private Sub LoadLines()
        Try
            If (ddlLineNumbers.Items.Count = 0) Then
                ' Lines for the drop-down list on the main web page
                ddlLineNumbers.Items.Clear()
                ddlLineNumbers.DataSource = DA.GetDataSet("SELECT LineID, LineName FROM dbo.tblSGLines")
                ddlLineNumbers.DataTextField = "LineName"
                ddlLineNumbers.DataValueField = "LineID"
                ddlLineNumbers.DataBind()
                ddlLineNumbers.Items.Insert(0, New ListItem("Choose a Line", ""))
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadStations()
        Try
            Dim ds As DataSet = Nothing
            Dim selectedValue As String = ddlStations.SelectedValue
            ddlStations.Items.Clear()

            If (ddlLineNumbers.SelectedIndex > 0) Then
                ds = GetStations(LineID)

                ddlStations.DataSource = ds
                ddlStations.DataTextField = "Description"
                ddlStations.DataValueField = "StationID"
                ddlStations.DataBind()
            End If

            ddlStations.Items.Insert(0, New ListItem("Choose a Station", ""))

            If (selectedValue.Length > 0) Then
                Dim li As ListItem = ddlStations.Items.FindByValue(selectedValue)
                If (li IsNot Nothing) Then
                    li.Selected = True
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadVehicleModels()
        Try
            Dim ds As DataSet = Nothing
            Dim selectedValue As String = ddlVehicleModels.SelectedValue

            ddlVehicleModels.Items.Clear()

            If (ddlStations.SelectedIndex > 0) Then
                ds = GetInstructionSets(ddlStations.SelectedValue)
            Else
                ds = GetInstructionSets("-1")
            End If

            ddlVehicleModels.DataSource = ds
            ddlVehicleModels.DataTextField = "Description"
            ddlVehicleModels.DataValueField = "ConfigurationID"
            ddlVehicleModels.DataBind()

            ddlVehicleModels.Items.Insert(0, New ListItem("Choose a Vehicle Model", ""))

            If (selectedValue.Length > 0) Then
                Dim li As ListItem = ddlVehicleModels.Items.FindByValue(selectedValue)
                If (li IsNot Nothing) Then
                    li.Selected = True
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadMCRs()
        Dim ds As DataSet = Nothing
        Dim param As New SqlParameter()
        Dim parameters As New List(Of SqlParameter)
        Try
            Dim selectedValue As String = ddlMCRNumber.SelectedValue
            ddlMCRNumber.Items.Clear()
            If (ddlLineNumbers.SelectedIndex > 0 And _
                ddlVehicleModels.SelectedIndex > 0) Then

                parameters.Add(New SqlParameter("@LineID", LineID.ToString()))
                parameters.Add(New SqlParameter("@ConfigurationID", VehicleModelID.ToString()))
                ds = DA.GetDataSet("[ods].[procSelectMasterChangeRequests]", parameters, "")

                If (ds.IsNotEmpty()) Then
                    For Each record As DataRow In ds.Tables(0).Rows()
                        Dim li As New ListItem(record(MCRDatasetColumns.MCRNumberTextValue).ToString(), record(MCRDatasetColumns.MasterChangeRequestID).ToString())
                        li.Attributes.Add("MCRDate", record(MCRDatasetColumns.MCRDate).ToString())
                        If (CBool(record(MCRDatasetColumns.IsActiveOnClient))) Then
                            li.Attributes.Add("class", "IsActiveOnClient")
                            If (selectedValue = "0") Then
                                li.Selected = True
                            End If
                        End If
                        ddlMCRNumber.Items.Add(li)
                    Next
                    If (selectedValue.Length > 0) Then
                        Dim li As ListItem = ddlMCRNumber.Items.FindByValue(selectedValue)
                        If (li IsNot Nothing) Then
                            li.Selected = True
                        End If
                    End If
                End If
            End If

            ddlMCRNumber.Items.Insert(0, New ListItem("Choose an MCR Number", "0"))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadModels()
        Dim ds As DataSet
        Dim param As New SqlParameter()
        Dim parameters As New List(Of SqlParameter)
        Try
            Dim selectedValue As String = ddlModels.SelectedValue
            ddlModels.Items.Clear()
            If (ddlStations.SelectedIndex > 0) Then

                If LineID = 0 Then
                    parameters.Add(New SqlParameter("@LineID", DBNull.Value))
                Else
                    parameters.Add(New SqlParameter("@LineID", LineID.ToString()))
                End If

                If StationID Is Nothing Then
                    parameters.Add(New SqlParameter("@StationID", DBNull.Value))
                Else
                    parameters.Add(New SqlParameter("@StationID", StationID.ToString()))
                End If

                If VehicleModelID = 0 Then
                    parameters.Add(New SqlParameter("@ConfigurationID", DBNull.Value))
                Else
                    parameters.Add(New SqlParameter("@ConfigurationID", VehicleModelID.ToString()))
                End If

                ds = DA.GetDataSet("[ods].[procSelectInstructionSetVehicleModels]", parameters, "")


                ddlModels.DataSource = ds
                ddlModels.DataTextField = "ModelNameFull"
                ddlModels.DataValueField = "ModelID"
                ddlModels.DataBind()
            End If

            ddlModels.Items.Insert(0, New ListItem("Choose a Model", ""))

            If (selectedValue.Length > 0) Then
                Dim li As ListItem = ddlModels.Items.FindByValue(selectedValue)
                If (li IsNot Nothing) Then
                    li.Selected = True
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadProductCodes()
        Dim ds As DataSet
        Dim param As New SqlParameter()
        Dim parameters As New List(Of SqlParameter)
        Try
            Dim selectedValue As String = ddlProductCode.SelectedValue
            ddlProductCode.Items.Clear()
            If (ddlModels.SelectedIndex > 0) Then
                parameters.Add(New SqlParameter("@LineID", LineID.ToString()))
                parameters.Add(New SqlParameter("@ModelCode", ddlModels.SelectedValue.ToString()))

                ds = DA.GetDataSet("[ods].[procGetInstructionProductCodes]", parameters, "")

                ddlProductCode.DataSource = ds
                ddlProductCode.DataTextField = "ProductID"
                ddlProductCode.DataValueField = "ProductID"
                ddlProductCode.DataBind()

            End If

            ddlProductCode.Items.Insert(0, New ListItem("Choose a Product", ""))

            If (selectedValue.Length > 0) Then
                Dim li As ListItem = ddlProductCode.Items.FindByValue(selectedValue)
                If (li IsNot Nothing) Then
                    li.Selected = True
                Else
                    If (ddlProductCode.Items.Count >= 2) Then
                        ddlProductCode.SelectedIndex = 1
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Function GetStations(ByVal lineID As Integer) As DataSet
        Dim ds As DataSet = Nothing
        Try
            Dim parameters As New List(Of SqlParameter)
            parameters.Add(New SqlParameter("@LineID", lineID))
            parameters.Add(New SqlParameter("@hasInstructions", DBNull.Value))

            ds = DA.GetDataSet("[ods].[procSelectStations]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function

    Private Function GetInstructionSets(ByVal stationID As String) As DataSet
        Dim ds As DataSet = Nothing
        Dim parameters As New List(Of SqlParameter)
        Try
            parameters.Add(New SqlParameter("@StationID", stationID))
            parameters.Add(New SqlParameter("@hasInstructions", DBNull.Value))

            ds = DA.GetDataSet("[ods].[procSelectInstructionSets]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function


    Private Sub LoadConfigurationTypeLabels()
        Dim ds As DataSet = Nothing
        Dim parameters As New List(Of SqlParameter)
        Try
            parameters.Add(New SqlParameter("@ConfigurationTypeID", DBNull.Value))
            ds = DA.GetDataSet("[ods].[procSelectConfigurationTypes]", parameters, "")

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                For Each row As DataRow In ds.Tables(0).Rows
                    If CInt(row.Item("ConfigurationTypeID")) = ConfigurationTypes.InstructionSet Then
                        'sets the label over the third drop down which defaults to "Instruction Sets"
                        lblInstructionSet.Text = row.Item("Description").ToString() & ":"

                    ElseIf CInt(row.Item("ConfigurationTypeID")) = ConfigurationTypes.AuxiliaryListA Then
                        'sets the label over the first auxiliary list which defaults as "Axiliary List A"
                        gvTools.Columns(0).HeaderText = row.Item("Description").ToString()

                    ElseIf CInt(row.Item("ConfigurationTypeID")) = ConfigurationTypes.AuxiliaryListB Then
                        'sets the label over the second auxiliary list which defaults as "Axiliary List B"
                        gvPPE.Columns(0).HeaderText = row.Item("Description").ToString()

                    End If
                Next

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub GenerateODSDisplay()
        Dim dr As DataRow = Nothing
        Dim ds As DataSet = Nothing
        Dim qryString As String
        Dim parameters As New List(Of SqlParameter)
        Try
            LoadFilterDropDownLists()

            gvInstructions.DataBind()
            gvMCR.DataBind()
            gvPPE.DataBind()
            gvTools.DataBind()

            If ((ddlLineNumbers.SelectedIndex = 0) OrElse _
                (ddlStations.SelectedIndex = 0) OrElse _
                (ddlVehicleModels.SelectedIndex = 0) OrElse _
                (ddlMCRNumber.SelectedIndex = 0) OrElse _
                (ddlModels.SelectedIndex = 0) OrElse _
                (ddlProductCode.SelectedIndex = 0)) Then
                '(ddlProductCode.SelectedIndex = 0 And ddlProductCode.Items.Count > 1)) Then

                divSimulateForm.Visible = False
            Else
                parameters.Add(New SqlParameter("@LineID", LineID))
                parameters.Add(New SqlParameter("@StationID", StationID))
                parameters.Add(New SqlParameter("@ModelCode", ddlModels.SelectedValue))

                If (ddlProductCode.SelectedIndex > 0) Then
                    parameters.Add(New SqlParameter("@ProductCode", ddlProductCode.SelectedValue))
                Else
                    parameters.Add(New SqlParameter("@ProductCode", DBNull.Value))
                End If


                ds = DA.GetDataSet("[ods].[procGetInstructionDetails]", parameters, "")

                If (DA.IsDataSetNotEmpty(ds)) Then
                    dr = ds.Tables(0).Rows(0)

                    'product header
                    lblStationName.Text = dr.Item("StationDescription").ToString()
                    hidHeaderColor.Value = dr.Item("HeaderBGColor").ToString()

                    'product information
                    lblHondaCode.Text = dr.Item("ModelCode").ToString()
                    lblModel.Text = dr.Item("Model").ToString()

                    If (ddlProductCode.SelectedIndex > 0) Then
                        lblProductCode.Text = dr.Item("ProductID").ToString()
                    Else
                        lblProductCode.Text = "&nbsp;"
                    End If

                    If pnlEDS.Visible Then
                        qryString = "lineID=" & LineID & "&stationID=" & StationID & "&cfgID=" & ddlVehicleModels.SelectedValue & "&mcrID=" & ddlMCRNumber.SelectedValue & "&modelCode=" & ddlModels.SelectedValue
                        imgWireHarness.ImageUrl = "imageFetch.ashx?" & qryString
                        imgWireHarness.AlternateText = imgWireHarness.ImageUrl
                    Else
                        imgWireHarness.AlternateText = "No Image Found"
                    End If
                End If

                divSimulateForm.Visible = True

            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


#End Region



End Class