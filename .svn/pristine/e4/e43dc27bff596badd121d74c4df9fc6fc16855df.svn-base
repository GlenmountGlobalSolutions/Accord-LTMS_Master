Imports Telerik.Web.UI
Imports System.Data.SqlClient

Public Class DailyBuildQuantities
    Inherits System.Web.UI.Page

    Dim dsMonthDataA As DataSet
    Dim dsMonthDataB As DataSet
    Dim dsMonthDataC As DataSet
    Dim dsMonthDataD As DataSet

    Const qtyDefault As String = "30"       'BizLayer.GetApplicationParameterValue("0024", "0001")
    Dim intBroadcastPointCount As Integer = 1

    Enum BuildQtyType
        ProductionDate
        ShiftQuantity1
        ShiftQuantity2
        ShiftQuantity3
        LotProd
        LotShip
        PlannedShipQuantity
        EndLotProducedJobQuantity
    End Enum

    Enum procGetBroadcastPoints
        BroadcastPointID
        Description
        ImageName
        DefaultDailyBuildQuantityShip
        DefaultDailyBuildQuantityJob
        defaultSelection
    End Enum

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                hidRad1LastTab.Value = "0"
                hidRad2LastTab.Value = "0"
                BuildMonthsList()
            End If
            hidRad1Shift.Value = (CInt(hidRad1LastTab.Value) + 1).ToString()
            hidRad2Shift.Value = (CInt(hidRad2LastTab.Value) + 1).ToString()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub DailyBuildQuantities_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim startDate As Date
        Try

            EnableControls()

            'set calendar visible month to selected date
            If (Date.TryParse(ddlMonth.SelectedValue, startDate)) Then
                calDailyBuild.VisibleDate = startDate
            End If

            GetCalendarData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub calDailyBuild_DayRender(sender As Object, e As DayRenderEventArgs) Handles calDailyBuild.DayRender
        Dim dtoBuildQtyA As New DTO_DailyBuildQuantity
        Dim dtoBuildQtyB As New DTO_DailyBuildQuantity
        Dim dtoBuildQtyC As New DTO_DailyBuildQuantity
        Dim dtoBuildQtyD As New DTO_DailyBuildQuantity
        Dim strCell As String = ""
        Dim startDate As Date
        Dim intSelectedMonth As Integer = 0

        Try
            If (Date.TryParse(ddlMonth.SelectedValue, startDate)) Then
                intSelectedMonth = startDate.Month
            End If

            If e.Day.Date.Month = intSelectedMonth Then        ' Only fill days that are within the selected month.
                dtoBuildQtyA = OnlyFillDaysThatAreWithinSelectedMonth(e.Day.Date.ToShortDateString(), dsMonthDataA)
                dtoBuildQtyB = OnlyFillDaysThatAreWithinSelectedMonth(e.Day.Date.ToShortDateString(), dsMonthDataB)
                dtoBuildQtyC = OnlyFillDaysThatAreWithinSelectedMonth(e.Day.Date.ToShortDateString(), dsMonthDataC)
                dtoBuildQtyD = OnlyFillDaysThatAreWithinSelectedMonth(e.Day.Date.ToShortDateString(), dsMonthDataD)
                'strCell = BuildCalendarDayAsCols(dtoBuildQtyA, dtoBuildQtyB, dtoBuildQtyC, dtoBuildQtyD)
                strCell = BuildCalendarDayAsRows(dtoBuildQtyA, dtoBuildQtyB, dtoBuildQtyC, dtoBuildQtyD)
                e.Cell.Controls.Add(New LiteralControl(strCell))
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ibPrevMo_Click(sender As Object, e As ImageClickEventArgs) Handles ibPrevMo.Click
        Try
            If (ddlMonth.SelectedIndex > 0) Then
                ddlMonth.SelectedIndex -= 1
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibNextMo_Click(sender As Object, e As ImageClickEventArgs) Handles ibNextMo.Click
        Try
            If (ddlMonth.SelectedIndex < (ddlMonth.Items.Count - 1)) Then
                ddlMonth.SelectedIndex += 1
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdPrePop_Click(sender As Object, e As EventArgs) Handles cmdPrePop.Click
        Try
            BuildQuantityPrePopulate(lblBPID1.Text, CInt("0" & txtPrePopShipQty1.Text), CInt("0" & txtPrePopProdQty1.Text))
            BuildQuantityPrePopulate(lblBPID2.Text, CInt("0" & txtPrePopShipQty2.Text), CInt("0" & txtPrePopProdQty2.Text))
            BuildQuantityPrePopulate(lblBPID3.Text, CInt("0" & txtPrePopShipQty3.Text), CInt("0" & txtPrePopProdQty3.Text))
            BuildQuantityPrePopulate(lblBPID4.Text, CInt("0" & txtPrePopShipQty4.Text), CInt("0" & txtPrePopProdQty4.Text))
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Protected Sub rgPrePop_RowDrop(ByVal sender As Object, ByVal e As GridDragDropEventArgs)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        If String.IsNullOrEmpty(e.HtmlElement) Then
            If e.DraggedItems(0).OwnerGridID = rgPrePop.ClientID Then
                If e.DestDataItem IsNot Nothing AndAlso e.DestDataItem.OwnerGridID = rgPrePop.ClientID Then

                    Dim destinationIndex As Integer = e.DestDataItem.ItemIndex + 1
                    Dim prePopId As Integer = DirectCast(e.DraggedItems(0).GetDataKeyValue("PrePopulateID"), Integer)

                    If ((e.DropPosition = GridItemDropPosition.Above) AndAlso (e.DestDataItem.ItemIndex > e.DraggedItems(0).ItemIndex)) Then
                        destinationIndex = (destinationIndex - 1)
                    End If
                    If ((e.DropPosition = GridItemDropPosition.Below) AndAlso (e.DestDataItem.ItemIndex < e.DraggedItems(0).ItemIndex)) Then
                        destinationIndex = (destinationIndex + 1)
                    End If

                    oSqlParameter = New SqlParameter("@PrePopulateID", SqlDbType.VarChar, 4)
                    oSqlParameter.Value = prePopId
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@DisplayID", SqlDbType.VarChar, 4)
                    oSqlParameter.Value = destinationIndex
                    colParameters.Add(oSqlParameter)

                    DA.ExecSP("procPSDailyBuildQuantityPrePopMove", colParameters)

                    rgPrePop.Rebind()
                End If
            End If
        End If
    End Sub

    Protected Sub rgPrePop_ItemDeleted(ByVal source As Object, ByVal e As GridDeletedEventArgs)
        'Try
        '    If Not e.Exception Is Nothing Then
        '        e.ExceptionHandled = True
        '        Master.eMsg(Server.HtmlEncode("Unable to Delete. Reason: " + e.Exception.Message).Replace("'", "'").Replace(vbCrLf, "<br />"))
        '    Else
        '        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        '        LoadPrePopBroadcastPointSums(CInt(dataItem.GetDataKeyValue("BroadcastPointID").ToString()), dataItem.GetDataKeyValue("BroadcastPointID").ToString())
        '    End If
        'Catch ex As Exception
        '    Master.eMsg(ex.ToString())
        'End Try
    End Sub

    Protected Sub rgPrePop_ItemUpdated(ByVal source As Object, ByVal e As GridUpdatedEventArgs)
        'Try
        '    If Not e.Exception Is Nothing Then
        '        e.KeepInEditMode = True
        '        e.ExceptionHandled = True
        '        Master.eMsg(Server.HtmlEncode("Unable to Update. Reason: " + e.Exception.Message).Replace("'", "'").Replace(vbCrLf, "<br />"))
        '    Else
        '        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        '        LoadPrePopBroadcastPointSums(CInt(dataItem.GetDataKeyValue("BroadcastPointID").ToString()), dataItem.GetDataKeyValue("BroadcastPointID").ToString())
        '    End If
        'Catch ex As Exception
        '    Master.eMsg(ex.ToString())
        'End Try
    End Sub

    Protected Sub RadComboBox1_ItemDataBound(ByVal o As Object, ByVal e As RadComboBoxItemEventArgs)
        Try
            Dim dataSourceRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            'e.Item.Attributes("customAttribute1") = dataSourceRow("CustomAttribute2").ToString()
            e.Item.CssClass = "comboBoxPrePopImage"
            e.Item.ImageUrl = String.Format("../../Images/Misc/{0}", dataSourceRow("ImageName").ToString())
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub rgEditBreakdown_RowDrop(ByVal sender As Object, ByVal e As GridDragDropEventArgs)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        If String.IsNullOrEmpty(e.HtmlElement) Then
            If e.DraggedItems(0).OwnerGridID = rgEditBreakdown.ClientID Then
                If e.DestDataItem IsNot Nothing AndAlso e.DestDataItem.OwnerGridID = rgEditBreakdown.ClientID Then

                    Dim destinationIndex As Integer = e.DestDataItem.ItemIndex + 1
                    Dim prePopId As Integer = DirectCast(e.DraggedItems(0).GetDataKeyValue("BreakdownID"), Integer)

                    If ((e.DropPosition = GridItemDropPosition.Above) AndAlso (e.DestDataItem.ItemIndex > e.DraggedItems(0).ItemIndex)) Then
                        destinationIndex = (destinationIndex - 1)
                    End If
                    If ((e.DropPosition = GridItemDropPosition.Below) AndAlso (e.DestDataItem.ItemIndex < e.DraggedItems(0).ItemIndex)) Then
                        destinationIndex = (destinationIndex + 1)
                    End If

                    oSqlParameter = New SqlParameter("@BreakdownID", SqlDbType.VarChar, 4)
                    oSqlParameter.Value = prePopId
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@DisplayID", SqlDbType.VarChar, 4)
                    oSqlParameter.Value = destinationIndex
                    colParameters.Add(oSqlParameter)

                    DA.ExecSP("procPSDailyBuildQuantityBreakdownMove", colParameters)

                    rgEditBreakdown.Rebind()
                End If
            End If
        End If
    End Sub

    Protected Sub rgEditBreakdown_ItemDeleted(ByVal source As Object, ByVal e As GridDeletedEventArgs)
        Try
            If Not e.Exception Is Nothing Then
                e.ExceptionHandled = True
                Master.eMsg(Server.HtmlEncode("Unable to Delete. Reason: " + e.Exception.Message).Replace("'", "'").Replace(vbCrLf, "<br />"))
            Else
                rgEditTotals.Rebind()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub rgEditBreakdown_ItemUpdated(ByVal source As Object, ByVal e As GridUpdatedEventArgs)
        Try
            If Not e.Exception Is Nothing Then
                e.KeepInEditMode = True
                e.ExceptionHandled = True
                Master.eMsg(Server.HtmlEncode("Unable to Update. Reason: " + e.Exception.Message).Replace("'", "'").Replace(vbCrLf, "<br />"))
            Else
                rgEditTotals.Rebind()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub rgEditBreakdown_ItemInserted(ByVal source As Object, ByVal e As GridInsertedEventArgs)
        Try
            If Not e.Exception Is Nothing Then
                e.ExceptionHandled = True
                Master.eMsg(Server.HtmlEncode("Unable to Update. Reason: " + e.Exception.Message).Replace("'", "'").Replace(vbCrLf, "<br />"))
            Else
                rgEditTotals.Rebind()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Protected Sub rgEditTotals_OnItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs)
        Try
            If e.Item.IsInEditMode Then
                If Not (TypeOf e.Item Is IGridInsertItem) Then
                    Dim editItem As GridEditableItem = DirectCast(e.Item, GridEditableItem)
                    Dim rowView As DataRowView = DirectCast(editItem.DataItem, DataRowView)

                    Dim comboProduced As RadComboBox = DirectCast(editItem.FindControl("RadComboBoxEndLotProduced"), RadComboBox)
                    comboProduced.SelectedValue = rowView("EndLotProduced").ToString()
                    comboProduced.Text = rowView("EndLotProducedText").ToString()

                    Dim comboShipped As RadComboBox = DirectCast(editItem.FindControl("RadComboBoxEndLotShipped"), RadComboBox)
                    comboShipped.SelectedValue = rowView("EndLotShipped").ToString()
                    comboShipped.Text = rowView("EndLotShippedText").ToString()
                End If
            ElseIf (TypeOf e.Item Is GridDataItem) Then
                Dim item As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim strLineTotal As String = item("QuantityLineTotal").Text
                'Dim img As ElasticButton = DirectCast(item("EditCommandColumn").Controls(0), ElasticButton) 'Accessing EditCommandColumn
                'Dim img As ElasticButton = DirectCast(item.FindControl("EditCommandColumn"), ElasticButton)
                If (strLineTotal = "0") Then
                    'img.Enabled = False
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Protected Sub rgEditTotals_OnUpdateCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs)
        Try
            Dim editedItem As GridEditableItem = TryCast(e.Item, GridEditableItem)

            Dim comboBoxProduced As RadComboBox = DirectCast(editedItem.FindControl("RadComboBoxEndLotProduced"), RadComboBox)
            Dim comboBoxShipped As RadComboBox = DirectCast(editedItem.FindControl("RadComboBoxEndLotShipped"), RadComboBox)

            SqlDataSourceEditDay.UpdateParameters("EndLotProduced").DefaultValue = comboBoxProduced.SelectedValue
            SqlDataSourceEditDay.UpdateParameters("EndLotShipped").DefaultValue = comboBoxShipped.SelectedValue

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Protected Sub RadComboBoxEndLotProduced_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        Try
            Dim cbx As RadComboBox = DirectCast(sender, RadComboBox)
            Dim dataItem As GridDataItem = DirectCast(cbx.Parent.Parent, GridDataItem)

            SqlDataSourceEndLotProduced.SelectParameters("BroadcastPointID").DefaultValue = CStr(dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("BroadcastPointID"))
            cbx.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub RadComboBoxEndLotShipped_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        Try
            Dim cbx As RadComboBox = DirectCast(sender, RadComboBox)
            Dim dataItem As GridDataItem = DirectCast(cbx.Parent.Parent, GridDataItem)

            SqlDataSourceEndLotShipped.SelectParameters("BroadcastPointID").DefaultValue = CStr(dataItem.OwnerTableView.DataKeyValues(dataItem.ItemIndex)("BroadcastPointID"))
            cbx.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub SqlDataSourceEditDay_Selecting(sender As Object, e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SqlDataSourceEditDay.Selecting
        Try
            SqlDataSourceEditDay.SelectParameters("ProductionDate").DefaultValue = hidDailyBuildDate.Value
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        Try

            Master.Secure(cmdEdit)
            Master.Secure(cmdPrePop)
            'Master.Secure(Me.cmdRefresh)
            Master.Secure(ibNextMo)
            Master.Secure(ibPrevMo)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub BuildMonthsList()
        Dim strText As String
        Dim strValue As String
        Dim startDate As Date = Now()
        Dim endDate As Date = Now()
        Try

            ''Making a list of 19 months
            ''past 12 months + current + 6 months

            startDate = startDate.AddYears(-1)  'roll back 1 year for start date
            endDate = endDate.AddMonths(6)  'add 6 months to current month 1 year for end date

            ddlMonth.Items.Clear()

            Do While startDate.Date <= endDate.Date
                strText = String.Format("{0:MMMM} {1}", startDate, startDate.Year)
                strValue = String.Format("{0:MM}/01/{1}", startDate, startDate.Year)

                Dim itm As New ListItem(strText, strValue)
                ddlMonth.Items.Add(itm)

                startDate = startDate.AddMonths(1)
            Loop


            ddlMonth.SelectedIndex = 12  'sets to current month

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub BuildQuantityPrePopulate(id As String, ShipQty As Integer, JobQty As Integer)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Try
            If (id.Length > 0) Then
                oSqlParameter = New SqlParameter("@DateStart", SqlDbType.VarChar, 30)
                oSqlParameter.Value = txtBegin.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@DateEnd", SqlDbType.VarChar, 30)
                oSqlParameter.Value = txtEnd.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Mon", SqlDbType.Bit)
                oSqlParameter.Value = Convert.ToBoolean(cblDays.Items(0).Selected).ToString()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Tue", SqlDbType.Bit)
                oSqlParameter.Value = Convert.ToBoolean(cblDays.Items(1).Selected).ToString()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Wed", SqlDbType.Bit)
                oSqlParameter.Value = Convert.ToBoolean(cblDays.Items(2).Selected).ToString()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Thu", SqlDbType.Bit)
                oSqlParameter.Value = Convert.ToBoolean(cblDays.Items(3).Selected).ToString()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Fri", SqlDbType.Bit)
                oSqlParameter.Value = Convert.ToBoolean(cblDays.Items(4).Selected).ToString()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Sat", SqlDbType.Bit)
                oSqlParameter.Value = Convert.ToBoolean(cblDays.Items(5).Selected).ToString()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Sun", SqlDbType.Bit)
                oSqlParameter.Value = Convert.ToBoolean(cblDays.Items(6).Selected).ToString()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@PlannedShipQuantity", SqlDbType.SmallInt)
                oSqlParameter.Value = ShipQty
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@EndLotProducedJobQuantity", SqlDbType.SmallInt)
                oSqlParameter.Value = JobQty
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@SetexSchedule", SqlDbType.Int)
                oSqlParameter.Value = rbOrders.SelectedValue
                colParameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("procPSDailyBuildQuantityPopulate", colParameters)

                Master.tMsg("Save", String.Format("Daily Build Quantity Pre-Populated for dates: {0} to {1}", txtBegin.Text, txtEnd.Text))
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Sub LoadPrePopBroadcastPointSums(bpid As Integer, BroadcastPointID As String)
        Try
            Select Case bpid
                Case 2
                    LoadPrePopBroadcastPointSums(BroadcastPointID, txtPrePopBuildQty2, txtPrePopShift1BP2, txtPrePopShift2BP2, txtPrePopShift3BP2)
                Case 3
                    LoadPrePopBroadcastPointSums(BroadcastPointID, txtPrePopBuildQty3, txtPrePopShift1BP3, txtPrePopShift2BP3, txtPrePopShift3BP3)
                Case 4
                    LoadPrePopBroadcastPointSums(BroadcastPointID, txtPrePopBuildQty4, txtPrePopShift1BP4, txtPrePopShift2BP4, txtPrePopShift3BP4)
                Case Else
                    LoadPrePopBroadcastPointSums(BroadcastPointID, txtPrePopBuildQty1, txtPrePopShift1BP1, txtPrePopShift2BP1, txtPrePopShift3BP1)
            End Select

            Dim shift1Total As Integer = CInt("0" & txtPrePopShift1BP1.Text) + CInt("0" & txtPrePopShift1BP2.Text) + CInt("0" & txtPrePopShift1BP3.Text) + CInt("0" & txtPrePopShift1BP4.Text)
            Dim shift2Total As Integer = CInt("0" & txtPrePopShift2BP1.Text) + CInt("0" & txtPrePopShift2BP2.Text) + CInt("0" & txtPrePopShift2BP3.Text) + CInt("0" & txtPrePopShift2BP4.Text)
            Dim shift3Total As Integer = CInt("0" & txtPrePopShift3BP1.Text) + CInt("0" & txtPrePopShift3BP2.Text) + CInt("0" & txtPrePopShift3BP3.Text) + CInt("0" & txtPrePopShift3BP4.Text)
            txtPrePopDailyShift1.Text = shift1Total.ToString()
            txtPrePopDailyShift2.Text = shift2Total.ToString()
            txtPrePopDailyShift3.Text = shift3Total.ToString()

            Dim total As Integer = CInt("0" & txtPrePopBuildQty1.Text) + CInt("0" & txtPrePopBuildQty2.Text) + CInt("0" & txtPrePopBuildQty3.Text) + CInt("0" & txtPrePopBuildQty4.Text)
            txtPrePopDailyTotal.Text = total.ToString()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub LoadPrePopBroadcastPointSums(bpid As String, txtBoxSum As TextBox, txtShift1Sum As TextBox, txtShift2Sum As TextBox, txtShift3Sum As TextBox)
        Try
            Dim dssa As DataSourceSelectArguments = New DataSourceSelectArguments()
            dssa.MaximumRows = 1
            dssa.AddSupportedCapabilities(DataSourceCapabilities.Page)

            SqlDataSourcePrePopBPSum.SelectParameters("BroadcastPointID").DefaultValue = CStr(bpid)
            Dim dvSql As DataView = DirectCast(SqlDataSourcePrePopBPSum.Select(dssa), DataView)
            For Each drvSql As DataRowView In dvSql
                txtBoxSum.Text = drvSql("SUMBP").ToString()
                txtShift1Sum.Text = drvSql("Shift1").ToString()
                txtShift2Sum.Text = drvSql("Shift2").ToString()
                txtShift3Sum.Text = drvSql("Shift3").ToString()
            Next
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "calendar functions"

    Private Function OnlyFillDaysThatAreWithinSelectedMonth(dte As String, dsMonthData As DataSet) As DTO_DailyBuildQuantity
        Dim view As DataView = New DataView
        Dim dtoBuildQty = New DTO_DailyBuildQuantity() With {.EndLotProducedJobQuantity = qtyDefault, .ProductionDate = dte}


        If (dsMonthData IsNot Nothing) AndAlso (dsMonthData.Tables(0).Rows.Count > 0) Then      ' If there is data from database, use that to fill structure.
            view.Table = dsMonthData.Tables(0)
            view.RowFilter = String.Format("ProductionDate = #{0}#", dte)

            If (view.Count > 0) Then
                dtoBuildQty.ShiftQuantity1 = view(0).Row(BuildQtyType.ShiftQuantity1).ToString()
                dtoBuildQty.ShiftQuantity2 = view(0).Row(BuildQtyType.ShiftQuantity2).ToString()
                dtoBuildQty.ShiftQuantity3 = view(0).Row(BuildQtyType.ShiftQuantity3).ToString()

                dtoBuildQty.LotProd = view(0).Row(BuildQtyType.LotProd).ToString()
                dtoBuildQty.LotShip = view(0).Row(BuildQtyType.LotShip).ToString()

                dtoBuildQty.PlannedShipQuantity = view(0).Row(BuildQtyType.PlannedShipQuantity).ToString()
                dtoBuildQty.EndLotProducedJobQuantity = view(0).Row(BuildQtyType.EndLotProducedJobQuantity).ToString()

                If (dtoBuildQty.EndLotProducedJobQuantity = "") Then
                    dtoBuildQty.EndLotProducedJobQuantity = qtyDefault
                End If
            End If
        End If

        Return dtoBuildQty

    End Function

    Private Sub GetCalendarData()
        Dim dsBroadCastPoint As DataSet
        Dim oSqlParameter As SqlParameter
        Dim startDate As Date
        Dim cnt As Integer = 1
        Try
            If (Date.TryParse(ddlMonth.SelectedValue, startDate)) Then

                dsBroadCastPoint = DA.GetDataSet("procGetBroadcastPoints")
                If dsBroadCastPoint.Tables.Count > 0 Then
                    intBroadcastPointCount = CInt(dsBroadCastPoint.Tables(0).Rows.Count)

                    For Each row As DataRow In dsBroadCastPoint.Tables(0).Rows
                        Dim colParameters As New List(Of SqlParameter)

                        oSqlParameter = New SqlParameter("@ProductionDate", SqlDbType.DateTime)
                        oSqlParameter.Value = startDate.ToString("MM/dd/yyyy")
                        colParameters.Add(oSqlParameter)

                        oSqlParameter = New SqlParameter("@SetexSchedule", SqlDbType.SmallInt)
                        oSqlParameter.Value = rbOrders.SelectedValue
                        colParameters.Add(oSqlParameter)

                        oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
                        oSqlParameter.Value = row.Item(procGetBroadcastPoints.BroadcastPointID)
                        colParameters.Add(oSqlParameter)


                        'Select Case CInt(row.Item(procGetBroadcastPoints.BroadcastPointID))
                        Select Case cnt
                            Case 2
                                dsMonthDataB = DA.GetDataSet("procPSDailyBuildQuantityCalendarSelect", colParameters)
                                UpdatePanelBroadcastPoint2(row.Item(procGetBroadcastPoints.BroadcastPointID).ToString(), row.Item(procGetBroadcastPoints.Description).ToString(), row.Item(procGetBroadcastPoints.DefaultDailyBuildQuantityShip).ToString(), row.Item(procGetBroadcastPoints.DefaultDailyBuildQuantityJob).ToString(), row.Item(procGetBroadcastPoints.ImageName).ToString())
                            Case 3
                                dsMonthDataC = DA.GetDataSet("procPSDailyBuildQuantityCalendarSelect", colParameters)
                                UpdatePanelBroadcastPoint3(row.Item(procGetBroadcastPoints.BroadcastPointID).ToString(), row.Item(procGetBroadcastPoints.Description).ToString(), row.Item(procGetBroadcastPoints.DefaultDailyBuildQuantityShip).ToString(), row.Item(procGetBroadcastPoints.DefaultDailyBuildQuantityJob).ToString(), row.Item(procGetBroadcastPoints.ImageName).ToString())
                            Case 4
                                dsMonthDataD = DA.GetDataSet("procPSDailyBuildQuantityCalendarSelect", colParameters)
                                UpdatePanelBroadcastPoint4(row.Item(procGetBroadcastPoints.BroadcastPointID).ToString(), row.Item(procGetBroadcastPoints.Description).ToString(), row.Item(procGetBroadcastPoints.DefaultDailyBuildQuantityShip).ToString(), row.Item(procGetBroadcastPoints.DefaultDailyBuildQuantityJob).ToString(), row.Item(procGetBroadcastPoints.ImageName).ToString())
                            Case Else
                                dsMonthDataA = DA.GetDataSet("procPSDailyBuildQuantityCalendarSelect", colParameters)
                                UpdatePanelBroadcastPoint1(row.Item(procGetBroadcastPoints.BroadcastPointID).ToString(), row.Item(procGetBroadcastPoints.Description).ToString(), row.Item(procGetBroadcastPoints.DefaultDailyBuildQuantityShip).ToString(), row.Item(procGetBroadcastPoints.DefaultDailyBuildQuantityJob).ToString(), row.Item(procGetBroadcastPoints.ImageName).ToString())
                        End Select

                        cnt = cnt + 1
                    Next
                End If

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub UpdatePanelBroadcastPoint1(broadcastPointID As String, description As String, qtyShip As String, qtyJob As String, imageName As String)
        LoadPrePopBroadcastPointSums(1, broadcastPointID)
        lblBPID1.Text = broadcastPointID
        lblBP1.Text = description
        Panel1.Visible = True
        lblPrePopTop1.Text = description
        prepopTopPanel1.Visible = True
        txtPrePopShipQty1.Text = qtyShip
        txtPrePopProdQty1.Text = qtyJob
        image1.Attributes("src") = "../../Images/Misc/" + imageName
        prePopImage1.Attributes("src") = "../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint2(broadcastPointID As String, description As String, qtyShip As String, qtyJob As String, imageName As String)
        LoadPrePopBroadcastPointSums(2, broadcastPointID)
        lblBPID2.Text = broadcastPointID
        lblBP2.Text = description
        Panel2.Visible = True
        lblPrePopTop2.Text = description
        prepopTopPanel2.Visible = True
        txtPrePopShipQty2.Text = qtyShip
        txtPrePopProdQty2.Text = qtyJob
        image2.Attributes("src") = "../../Images/Misc/" + imageName
        prePopImage2.Attributes("src") = "../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint3(broadcastPointID As String, description As String, qtyShip As String, qtyJob As String, imageName As String)
        LoadPrePopBroadcastPointSums(3, broadcastPointID)
        lblBPID3.Text = broadcastPointID
        lblBP3.Text = description
        Panel3.Visible = True
        lblPrePopTop3.Text = description
        prepopTopPanel3.Visible = True
        txtPrePopShipQty3.Text = qtyShip
        txtPrePopProdQty3.Text = qtyJob
        image3.Attributes("src") = "../../Images/Misc/" + imageName
        prePopImage3.Attributes("src") = "../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint4(broadcastPointID As String, description As String, qtyShip As String, qtyJob As String, imageName As String)
        LoadPrePopBroadcastPointSums(4, broadcastPointID)
        lblBPID4.Text = broadcastPointID
        lblBP4.Text = description
        Panel4.Visible = True
        lblPrePopTop4.Text = description
        prepopTopPanel4.Visible = True
        txtPrePopShipQty4.Text = qtyShip
        txtPrePopProdQty4.Text = qtyJob
        image4.Attributes("src") = "../../Images/Misc/" + imageName
        prePopImage4.Attributes("src") = "../../Images/Misc/" + imageName
    End Sub

    Private Function BuildCalendarDayAsCols(dtoA As DTO_DailyBuildQuantity, dtoB As DTO_DailyBuildQuantity, dtoC As DTO_DailyBuildQuantity, dtoD As DTO_DailyBuildQuantity) As String
        Const strDiv_start As String = "<div class='calendarCellDiv'>"
        Const strRow_header As String = "<tr><td class='calendarCellHeader'>"
        Const strTable_start As String = "<table class='calenderCellTable'>"

        Const strRow_header_col As String = "</td><td class='calendarCellHeader'>"
        Const strRow_header_val As String = "{0}"

        Const strRow_start As String = "<tr><td class='calendarCellLabel'>"

        Const strRow_1_Col_1 As String = "</td><td  class='calendarCellQty' id='tdA_Shift1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_1 As String = "</td><td  class='calendarCellQty' id='tdA_Shift2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_1 As String = "</td><td  class='calendarCellQty' id='tdA_Shift3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_1 As String = "</td><td  class='calendarCellQty' id='tdA_PlannedShipQty'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_1 As String = "</td><td  class='calendarCellQty' id='tdA_LotProdShip'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"

        Const strRow_1_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdB_Shift1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdB_Shift2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdB_Shift3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdB_PlannedShipQty'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdB_LotProdShip'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"

        Const strRow_1_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdC_Shift1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdC_Shift2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdC_Shift3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdC_PlannedShipQty'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdC_LotProdShip'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"

        Const strRow_1_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdD_Shift1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdD_Shift2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdD_Shift3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdD_PlannedShipQty'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdD_LotProdShip'><span class='calendarCellSpan readonly ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"

        Const strRow_1_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdTotal_Shift1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdTotal_Shift2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdTotal_Shift3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdTotal_PlannedShipQty'><span>"
        Const strRow_5_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdTotal_LotProdShip'><span>"

        Const strLabel_1 As String = "<label style='display:none' id='lblDate'>"
        Const strLabel_2 As String = "<label style='display:none' id='lblJobQty'>"

        Const strLabel_end As String = "</label>"

        Const strRow_end As String = "</span></td></tr>"
        Const strTable_end As String = "</table>"
        Const strDiv_end As String = "</div>"

        Const strDesc_Shift As String = "" '"Shift"
        Const strDesc_Shift1 As String = "1st:"
        Const strDesc_Shift2 As String = "2nd:"
        Const strDesc_Shift3 As String = "3rd:"
        Const strDesc_ShipQt As String = "End Lot:"
        Const strDesc_EndLot As String = "Of:"

        Dim strCell As New StringBuilder()
        Dim total As Integer

        Try
            strCell.Append(strDiv_start + strTable_start)

            strCell.Append(strRow_header)
            strCell.Append(strDesc_Shift)

            strCell.Append(strRow_header_col)
            strCell.AppendFormat(strRow_header_val, String.Format("<img src='{0}' class='calendarImgBP1' />", image1.Attributes("src")))
            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_header_col)
                strCell.AppendFormat(strRow_header_val, String.Format("<img src='{0}' class='calendarImgBP2' />", image2.Attributes("src")))
            End If
            If (intBroadcastPointCount > 2) Then
                strCell.Append(strRow_header_col)
                strCell.AppendFormat(strRow_header_val, String.Format("<img src='{0}' class='calendarImgBP3' />", image3.Attributes("src")))
            End If
            If (intBroadcastPointCount > 3) Then
                strCell.Append(strRow_header_col)
                strCell.AppendFormat(strRow_header_val, String.Format("<img src='{0}' class='calendarImgBP4' />", image4.Attributes("src")))
            End If
            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_header_col)
                strCell.Append("Total")
            End If
            strCell.Append(strRow_end)

            strCell.Append(strRow_start)
            strCell.Append(strDesc_Shift1)
            strCell.Append(strRow_1_Col_1)
            strCell.Append(dtoA.ShiftQuantity1)
            total = CInt(dtoA.ShiftQuantity1)

            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_1_Col_2)
                strCell.Append(dtoB.ShiftQuantity1)
                total = total + CInt(dtoB.ShiftQuantity1)
            End If
            If (intBroadcastPointCount > 2) Then
                strCell.Append(strRow_1_Col_3)
                strCell.Append(dtoC.ShiftQuantity1)
                total = total + CInt(dtoC.ShiftQuantity1)
            End If
            If (intBroadcastPointCount > 3) Then
                strCell.Append(strRow_1_Col_4)
                strCell.Append(dtoD.ShiftQuantity1)
                total = total + CInt(dtoD.ShiftQuantity1)
            End If
            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_1_Col_5)
                strCell.Append(total)
            End If
            strCell.Append(strRow_end)

            strCell.Append(strRow_start)
            strCell.Append(strDesc_Shift2)
            strCell.Append(strRow_2_Col_1)
            strCell.Append(dtoA.ShiftQuantity2)
            total = CInt(dtoA.ShiftQuantity2)

            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_2_Col_2)
                strCell.Append(dtoB.ShiftQuantity2)
                total = total + CInt(dtoB.ShiftQuantity2)
            End If
            If (intBroadcastPointCount > 2) Then
                strCell.Append(strRow_2_Col_3)
                strCell.Append(dtoC.ShiftQuantity2)
                total = total + CInt(dtoC.ShiftQuantity2)
            End If
            If (intBroadcastPointCount > 3) Then
                strCell.Append(strRow_2_Col_4)
                strCell.Append(dtoD.ShiftQuantity2)
                total = total + CInt(dtoD.ShiftQuantity2)
            End If
            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_2_Col_5)
                strCell.Append(total)
            End If
            strCell.Append(strRow_end)

            strCell.Append(strRow_start)
            strCell.Append(strDesc_Shift3)
            strCell.Append(strRow_3_Col_1)
            strCell.Append(dtoA.ShiftQuantity3)
            total = CInt(dtoA.ShiftQuantity3)

            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_3_Col_2)
                strCell.Append(dtoB.ShiftQuantity3)
                total = total + CInt(dtoB.ShiftQuantity3)
            End If
            If (intBroadcastPointCount > 2) Then
                strCell.Append(strRow_3_Col_3)
                strCell.Append(dtoC.ShiftQuantity3)
                total = total + CInt(dtoC.ShiftQuantity3)
            End If
            If (intBroadcastPointCount > 3) Then
                strCell.Append(strRow_3_Col_4)
                strCell.Append(dtoD.ShiftQuantity3)
                total = total + CInt(dtoD.ShiftQuantity3)
            End If
            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_3_Col_5)
                strCell.Append(total)
            End If
            strCell.Append(strRow_end)

            strCell.Append(strRow_start)
            strCell.Append(strDesc_ShipQt)
            strCell.Append(strRow_4_Col_1)
            strCell.Append(dtoA.PlannedShipQuantity)

            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_4_Col_2)
                strCell.Append(dtoB.PlannedShipQuantity)
            End If
            If (intBroadcastPointCount > 2) Then
                strCell.Append(strRow_4_Col_3)
                strCell.Append(dtoC.PlannedShipQuantity)
            End If
            If (intBroadcastPointCount > 3) Then
                strCell.Append(strRow_4_Col_4)
                strCell.Append(dtoD.PlannedShipQuantity)
            End If
            strCell.Append(strRow_4_Col_5)
            strCell.Append(strRow_end)

            strCell.Append(strRow_start)
            strCell.Append(strDesc_EndLot)
            strCell.Append(strRow_5_Col_1)
            strCell.Append(dtoA.LotProd)

            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_5_Col_2)
                strCell.Append(dtoB.LotProd)
            End If
            If (intBroadcastPointCount > 2) Then
                strCell.Append(strRow_5_Col_3)
                strCell.Append(dtoC.LotProd)
            End If
            If (intBroadcastPointCount > 3) Then
                strCell.Append(strRow_5_Col_4)
                strCell.Append(dtoD.LotProd)
            End If
            strCell.Append(strRow_5_Col_5)
            strCell.Append(strRow_end)

            strCell.Append(strTable_end)

            strCell.Append(strLabel_1 + dtoA.ProductionDate + strLabel_end)
            strCell.Append(strLabel_2 + dtoA.EndLotProducedJobQuantity + strLabel_end)

            strCell.Append(strDiv_end)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return strCell.ToString()


    End Function

    Private Function BuildCalendarDayAsRows(dtoA As DTO_DailyBuildQuantity, dtoB As DTO_DailyBuildQuantity, dtoC As DTO_DailyBuildQuantity, dtoD As DTO_DailyBuildQuantity) As String
        Const strDiv_start As String = "<div class='calendarCellDiv'>"
        Const strRow_header As String = "<tr><td class='calendarCellHeader'>"
        Const strTable_start As String = "<table class='calenderCellTable'>"

        Const strRow_header_col As String = "</td><td class='calendarCellHeader'>"
        Const strRow_header_val As String = "{0}"

        Const strRow_start As String = "<tr><td class='calendarCellLabel'>"

        Const strRow_1_Col_1 As String = "</td><td  class='calendarCellQty' id='tdRow_1_Col_1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_1 As String = "</td><td  class='calendarCellQty' id='tdRow_2_Col_1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_1 As String = "</td><td  class='calendarCellQty' id='tdRow_3_Col_1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_1 As String = "</td><td  class='calendarCellQty' id='tdRow_4_Col_1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_1 As String = "</td><td  class='calendarCellQty' id='tdRow_5_Col_1'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"

        Const strRow_1_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdRow_1_Col_2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdRow_2_Col_2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdRow_3_Col_2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdRow_4_Col_2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_2 As String = "</span></td><td  class='calendarCellQty' id='tdRow_5_Col_2'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"

        Const strRow_1_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdRow_1_Col_3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdRow_2_Col_3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdRow_3_Col_3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdRow_4_Col_3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_3 As String = "</span></td><td  class='calendarCellQty' id='tdRow_5_Col_3'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"

        Const strRow_1_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdRow_1_Col_4'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdRow_2_Col_4'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdRow_3_Col_4'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdRow_4_Col_4'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_4 As String = "</span></td><td  class='calendarCellQty' id='tdRow_5_Col_4'><span>"

        Const strRow_1_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdRow_1_Col_5'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_2_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdRow_2_Col_5'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_3_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdRow_3_Col_5'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_4_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdRow_4_Col_5'><span class='calendarCellSpan ui-state-active ui-button ui-widget ui-state-default ui-button-text-only ui-corner-left ui-corner-right'>"
        Const strRow_5_Col_5 As String = "</span></td><td  class='calendarCellQty' id='tdRow_5_Col_5'><span>"

        Const strLabel_1 As String = "<label style='display:none' id='lblDate'>"
        Const strLabel_2 As String = "<label style='display:none' id='lblJobQty'>"

        Const strLabel_end As String = "</label>"

        Const strRow_end As String = "</span></td></tr>"
        Const strTable_end As String = "</table>"
        Const strDiv_end As String = "</div>"

        Const strDesc_Shift As String = "" '"Shift"
        Const strDesc_Shift1 As String = "1st:"
        Const strDesc_Shift2 As String = "2nd:"
        Const strDesc_Shift3 As String = "3rd:"
        Const strDesc_ShipQt As String = "End Lot:"
        Const strDesc_EndLot As String = "Of:"

        Dim strCell As New StringBuilder()
        Dim totalShift1 As Integer
        Dim totalShift2 As Integer
        Dim totalShift3 As Integer

        Try
            strCell.Append(strDiv_start + strTable_start)

            strCell.Append(strRow_header)
            strCell.Append(strDesc_Shift)
            strCell.Append(strRow_header_col)
            strCell.Append(strDesc_Shift1)
            strCell.Append(strRow_header_col)
            strCell.Append(strDesc_Shift2)
            strCell.Append(strRow_header_col)
            strCell.Append(strDesc_Shift3)
            strCell.Append(strRow_header_col)
            strCell.Append(strDesc_ShipQt)
            strCell.Append(strRow_header_col)
            strCell.Append(strDesc_EndLot)
            strCell.Append(strRow_header_col)
            strCell.Append(strRow_end)


            strCell.Append(strRow_start)
            strCell.AppendFormat(strRow_header_val, String.Format("<img src='{0}' class='calendarImgBP1' />", image1.Attributes("src")))
            strCell.Append(strRow_1_Col_1)
            strCell.Append(dtoA.ShiftQuantity1)
            strCell.Append(strRow_1_Col_2)
            strCell.Append(dtoA.ShiftQuantity2)
            strCell.Append(strRow_1_Col_3)
            strCell.Append(dtoA.ShiftQuantity3)
            strCell.Append(strRow_1_Col_4)
            strCell.Append(dtoA.LotProd)
            strCell.Append(strRow_1_Col_5)
            strCell.Append(dtoA.EndLotProducedJobQuantity)
            strCell.Append(strRow_end)

            totalShift1 = CInt(dtoA.ShiftQuantity1)
            totalShift2 = CInt(dtoA.ShiftQuantity2)
            totalShift3 = CInt(dtoA.ShiftQuantity3)

            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_start)
                strCell.AppendFormat(strRow_header_val, String.Format("<img src='{0}' class='calendarImgBP2' />", image2.Attributes("src")))
                strCell.Append(strRow_2_Col_1)
                strCell.Append(dtoB.ShiftQuantity1)
                strCell.Append(strRow_2_Col_2)
                strCell.Append(dtoB.ShiftQuantity2)
                strCell.Append(strRow_2_Col_3)
                strCell.Append(dtoB.ShiftQuantity3)
                strCell.Append(strRow_2_Col_4)
                strCell.Append(dtoB.LotProd)
                strCell.Append(strRow_2_Col_5)
                strCell.Append(dtoB.EndLotProducedJobQuantity)
                strCell.Append(strRow_end)

                totalShift1 = totalShift1 + CInt(dtoB.ShiftQuantity1)
                totalShift2 = totalShift2 + CInt(dtoB.ShiftQuantity2)
                totalShift3 = totalShift3 + CInt(dtoB.ShiftQuantity3)
            End If

            If (intBroadcastPointCount > 2) Then
                strCell.Append(strRow_start)
                strCell.AppendFormat(strRow_header_val, String.Format("<img src='{0}' class='calendarImgBP3' />", image3.Attributes("src")))
                strCell.Append(strRow_3_Col_1)
                strCell.Append(dtoC.ShiftQuantity1)
                strCell.Append(strRow_3_Col_2)
                strCell.Append(dtoC.ShiftQuantity2)
                strCell.Append(strRow_3_Col_3)
                strCell.Append(dtoC.ShiftQuantity3)
                strCell.Append(strRow_3_Col_4)
                strCell.Append(dtoC.LotProd)
                strCell.Append(strRow_3_Col_5)
                strCell.Append(dtoC.EndLotProducedJobQuantity)
                strCell.Append(strRow_end)

                totalShift1 = totalShift1 + CInt(dtoC.ShiftQuantity1)
                totalShift2 = totalShift2 + CInt(dtoC.ShiftQuantity2)
                totalShift3 = totalShift3 + CInt(dtoC.ShiftQuantity3)
            End If

            If (intBroadcastPointCount > 3) Then
                strCell.Append(strRow_start)
                strCell.AppendFormat(strRow_header_val, String.Format("<img src='{0}' class='calendarImgBP4' />", image4.Attributes("src")))
                strCell.Append(strRow_4_Col_1)
                strCell.Append(dtoD.ShiftQuantity1)
                strCell.Append(strRow_4_Col_2)
                strCell.Append(dtoD.ShiftQuantity2)
                strCell.Append(strRow_4_Col_3)
                strCell.Append(dtoD.ShiftQuantity3)
                strCell.Append(strRow_4_Col_4)
                strCell.Append(dtoD.LotProd)
                strCell.Append(strRow_4_Col_5)
                strCell.Append(dtoD.EndLotProducedJobQuantity)
                strCell.Append(strRow_end)

                totalShift1 = totalShift1 + CInt(dtoD.ShiftQuantity1)
                totalShift2 = totalShift2 + CInt(dtoD.ShiftQuantity2)
                totalShift3 = totalShift3 + CInt(dtoD.ShiftQuantity3)
            End If

            If (intBroadcastPointCount > 1) Then
                strCell.Append(strRow_start)
                'strCell.AppendFormat(strRow_header_val, "Total:")
                strCell.Append(strRow_5_Col_1)
                strCell.Append(totalShift1)
                strCell.Append(strRow_5_Col_2)
                strCell.Append(totalShift2)
                strCell.Append(strRow_5_Col_3)
                strCell.Append(totalShift3)
                strCell.Append(strRow_5_Col_4)
                strCell.Append(strRow_5_Col_5)
                strCell.Append(strRow_end)
            End If


            strCell.Append(strTable_end)

            strCell.Append(strLabel_1 + dtoA.ProductionDate + strLabel_end)
            strCell.Append(strLabel_2 + dtoA.EndLotProducedJobQuantity + strLabel_end)

            strCell.Append(strDiv_end)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return strCell.ToString()


    End Function

#End Region

End Class