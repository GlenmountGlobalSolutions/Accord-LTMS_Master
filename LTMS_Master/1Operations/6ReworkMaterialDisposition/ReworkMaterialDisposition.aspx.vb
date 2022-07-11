Imports System.Data.SqlClient
Imports System.Globalization

Public Class ReworkMaterialDisposition
    Inherits System.Web.UI.Page

#Region "page constants"

    Private Const PARAM_LIST_ID_REWORK_AREA As String = "52"
    Private Const PARAM_LIST_ID_CHARGE_TO As String = "53"
    Private Const PARAM_LIST_ID_REWORK_ACTION As String = "54"

    Private Const CATEGORY_TYPE_ID_REWORK As String = "2"
    Private Const CATEGORY_TYPE_ID_MATERIAL As String = "1"
    Private Const MATERIAL_LIST_TYPE_ID_REWORK_REASONS As String = "3"
    Private Const MATERIAL_LIST_TYPE_ID_DISP_REASONS As String = "2"
    Private Const MATERIAL_LIST_TYPE_ID_PART_LIST As String = "1"

    Private Const COL_PROD_SCHED_LOT_NUM As String = "SubLotNumber"
    Private Const COL_PROD_SCHED_LOT_PROD_ID As String = "BC"
    Private Const COL_PROD_SCHED_LOT_MODEL As String = "Model"
    Private Const COL_PROD_SCHED_VEHICLE_LINE As String = "VehicleModel"

    Private Const COL_PROD_DATA_COLOR As String = "Color"
    Private Const COL_PROD_DATA_PROD_ID As String = "ShipCode"
    Private Const COL_PROD_DATA_VEH_MODEL As String = "VehicleModel"
    Private Const COL_PROD_DATA_SEAT_CODE As String = "LabelSeatCode"

#End Region


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                If (Not LoadDayShift()) Then
                    Master.Msg = "Error: Unable to get current shift information! Please check Shift Configuration."
                    Return
                End If
                LoadParamListValues(Me.ddlInitRework, PARAM_LIST_ID_REWORK_AREA)
                LoadLotNumDDL()
                LoadSeatCompDDL()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ReworkMaterialDisposition_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try

            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region


#Region "Initial selection area code"

    Private Sub ddlInitLot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlInitLot.SelectedIndexChanged
        Try
            Me.ddlInitComp.SelectedIndex = 0
            Me.ddlRewCategory.Items.Clear()
            Me.ddlMatCategory.Items.Clear()
            Me.ddlRewReason.Items.Clear()
            Me.ddlRewResponsibility.Items.Clear()
            Me.ddlRewAction.Items.Clear()
            Me.tbRewQuantity.Text = ""
            Me.ddlMatPart.Items.Clear()
            Me.tbMatPartNo.Text = ""
            Me.ddlMatReason.Items.Clear()
            Me.ddlMatResponsibility.Items.Clear()
            Me.tbMatQtyReturned.Text = ""
            Me.tbMatQtyScrapped.Text = ""
            Me.tbMatQtyTotal.Text = ""
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlInitComp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlInitComp.SelectedIndexChanged
        Try
            Me.ddlMatCategory.Items.Clear()
            Me.ddlMatPart.Items.Clear()
            Me.ddlMatReason.Items.Clear()
            Me.ddlMatResponsibility.Items.Clear()
            Me.tbMatPartNo.Text = ""
            Me.tbMatQtyReturned.Text = ""
            Me.tbMatQtyScrapped.Text = ""
            Me.tbMatQtyTotal.Text = ""

            Me.ddlRewCategory.Items.Clear()
            Me.ddlRewReason.Items.Clear()
            Me.ddlRewResponsibility.Items.Clear()
            Me.ddlRewAction.Items.Clear()
            Me.tbRewQuantity.Text = ""

            If (Me.ddlInitComp.SelectedIndex <= 0) Then
                Return
            End If

            LoadMatTypeDDL(Me.ddlRewCategory, Me.ddlInitComp.SelectedValue, CATEGORY_TYPE_ID_REWORK)
            LoadMatTypeDDL(Me.ddlMatCategory, Me.ddlInitComp.SelectedValue, CATEGORY_TYPE_ID_MATERIAL)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Rework selection area code"

    Private Sub ddlRewCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlRewCategory.SelectedIndexChanged
        Try
            Me.ddlRewReason.Items.Clear()
            If (Me.ddlInitComp.SelectedIndex <= 0) Then
                Return
            End If
            If (Me.ddlRewCategory.SelectedIndex <= 0) Then
                Return
            End If

            LoadListValuesDDL(Me.ddlRewReason, Me.ddlInitComp.SelectedValue, CATEGORY_TYPE_ID_REWORK, ddlRewCategory.SelectedValue, MATERIAL_LIST_TYPE_ID_REWORK_REASONS)
            LoadChargeToDDL(Me.ddlRewResponsibility)
            LoadParamListValues(Me.ddlRewAction, PARAM_LIST_ID_REWORK_ACTION)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlRewAction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlRewAction.SelectedIndexChanged
        Try
            If (Me.ddlRewAction.SelectedIndex <= 0) Then
                Me.tbRewQuantity.Text = ""
            End If
            Me.tbRewQuantity.Focus()
            Me.tbRewQuantity.Text = "1"
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRewCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRewCancel.Click
        Try
            ClearRew()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRewSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRewSave.Click
        Try
            If (CheckRewInputs()) Then
                SaveRew()
                ClearRew()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Material selection area code"

    Private Sub ddlMatCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMatCategory.SelectedIndexChanged
        Try
            Me.tbMatPartNo.Text = ""
            If (Me.ddlInitComp.SelectedIndex <= 0) Then
                Me.ddlMatPart.Items.Clear()
                Return
            End If
            If (Me.ddlMatCategory.SelectedIndex <= 0) Then
                Me.ddlMatPart.Items.Clear()
                Return
            End If

            LoadListValuesDDL(Me.ddlMatPart, Me.ddlInitComp.SelectedValue, CATEGORY_TYPE_ID_MATERIAL, Me.ddlMatCategory.SelectedValue, MATERIAL_LIST_TYPE_ID_PART_LIST)
            LoadListValuesDDL(Me.ddlMatReason, Me.ddlInitComp.SelectedValue, CATEGORY_TYPE_ID_MATERIAL, Me.ddlMatCategory.SelectedValue, MATERIAL_LIST_TYPE_ID_DISP_REASONS)
            LoadChargeToDDL(Me.ddlMatResponsibility)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlMatPart_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMatPart.SelectedIndexChanged
        Try
            Me.tbMatPartNo.Text = ""
            If (Me.ddlMatPart.SelectedIndex <= 0) Then
                Return
            End If
            If (Me.ddlInitLot.SelectedIndex <= 0) Then
                Return
            End If

            Dim vehModel As String = ""
            Dim seatStyle As String = ""
            Dim color As String = ""
            Dim prodID As String = ""
            SetLotData(Me.ddlInitLot.SelectedValue.Replace("-", ""), vehModel, seatStyle, color, prodID)

            Dim ds As DataSet
            Dim sql As String = "SELECT TOP 1 PartNumber FROM tblRMDProductParts WHERE "
            sql += "(ListValueID = " + Me.ddlMatPart.SelectedValue + ")"
            sql += " AND (ProductID = '" + prodID + "')"
            ds = DA.GetDataSet(sql)
            If (DA.IsDSEmpty(ds)) Then
                Return
            End If

            Me.tbMatPartNo.Text = ds.Tables(0).DefaultView.Table.Rows(0)(0).ToString()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub ddlMatResponsibility_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMatResponsibility.SelectedIndexChanged
        Try
            If (Me.ddlMatResponsibility.SelectedIndex > 0) Then
                Me.tbMatQtyReturned.Focus()
            End If

            Me.tbMatQtyReturned.Text = "0"
            Me.tbMatQtyScrapped.Text = "0"
            Me.tbMatQtyTotal.Text = "0"

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdMatCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMatCancel.Click
        Try
            ClearMat()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdMatSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMatSave.Click
        Try
            If (CheckMatInputs()) Then
                SaveMat()
                ClearMat()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region


#Region "Methods"

    Private Sub EnableControls()
        Master.Secure(Me.cmdMatCancel)
        Master.Secure(Me.cmdMatSave)
        Master.Secure(Me.cmdRewCancel)
        Master.Secure(Me.cmdRewSave)
    End Sub


    Private Function LoadDayShift() As Boolean
        Dim shiftNum As String = "0"
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        oSqlParameter = New SqlParameter("@intCurrentShiftID", SqlDbType.Int)
        oSqlParameter.Value = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@vchCurrentShiftStartDT", SqlDbType.VarChar, 30)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@vchCurrentShiftEndDT", SqlDbType.VarChar, 30)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@intCurrentShiftNumber", SqlDbType.Int)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        colOutput = DA.ExecSP("procPTGetCurrentShiftInfo", colParameters)


        For Each oParameter In colOutput
            With oParameter
                If .Direction = ParameterDirection.Output And .ParameterName = "@intCurrentShiftNumber" Then
                    shiftNum = oParameter.Value.ToString()
                ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@vchCurrentShiftStartDT" Then

                    Me.tbDay.Text = DateTime.Parse(oParameter.Value.ToString()).ToString("MM/dd/yyyy")
                End If
            End With
        Next

        If (shiftNum = "1") Then
            Me.rblShift.SelectedIndex = 0
        ElseIf (shiftNum = "2") Then
            Me.rblShift.SelectedIndex = 1
        ElseIf (shiftNum = "3") Then
            Me.rblShift.SelectedIndex = 2
        Else
            Me.rblShift.SelectedIndex = -1
            Return False
        End If


        Return True
    End Function


    Private Sub LoadLotNumDDL()
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim ds As DataSet
        Dim hold As Int32 = 1
        Dim strDt As String = ""
        Dim numDays As Integer = 1

        strDt = Utility.FormattedDate(Me.tbDay.Text)

        Me.ddlInitLot.Items.Clear()

        'CREATE PROCEDURE procPSGetProdSched @BegDT varchar(30),
        '							@Report bit = 0,
        '							@NumDays int = 7				    
        '--                         @Status Varchar(80) OUT, 
        '--					        @ErrorMaster.Msg Varchar(80) OUT 

        'call sp with selected week's monday's date
        'sql = "exec procPSGetProdSched '" + Me.ddlWeek.SelectedValue + "', NULL, NULL"
        'call stored procedure and get 9 tables in a dataset (last table not used here)
        'ds = DA.GetDataSet(sql)

        oSqlParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
        oSqlParameter.Value = strDt
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
        oSqlParameter.Value = hold
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@NumDays", SqlDbType.Int)
        oSqlParameter.Value = numDays
        colParameters.Add(oSqlParameter)

        ds = DA.GetDataSet("procPSGetProdSched", colParameters)

        If (DA.IsDSEmpty(ds)) Then
            Return
        End If

        LotNumDDLBind(ds.Tables(0))

    End Sub

    Private Sub LotNumDDLBind(ByRef dt As DataTable)
        Me.ddlInitLot.Items.Clear()
        If (dt Is Nothing) Then
            Return
        End If
        If (dt.Rows.Count <= 0) Then
            Return
        End If

        Dim i As Integer
        For i = 0 To (dt.Rows.Count - 1)
            Me.ddlInitLot.Items.Insert(i, "")
            Me.ddlInitLot.Items(i).Value = dt.Rows(i)(COL_PROD_SCHED_LOT_NUM).ToString()
            Me.ddlInitLot.Items(i).Text = dt.Rows(i)(COL_PROD_SCHED_LOT_NUM).ToString()
            Me.ddlInitLot.Items(i).Text += (", " + dt.Rows(i)(COL_PROD_SCHED_VEHICLE_LINE).ToString())
            Me.ddlInitLot.Items(i).Text += (", " + dt.Rows(i)(COL_PROD_SCHED_LOT_MODEL).ToString())
            Me.ddlInitLot.Items(i).Text += (", " + dt.Rows(i)(COL_PROD_SCHED_LOT_PROD_ID).ToString())
        Next

        Me.ddlInitLot.Items.Insert(0, "")
        Me.ddlInitLot.SelectedIndex = 0

    End Sub

    Private Sub LoadSeatCompDDL()
        Dim ds As DataSet
        Dim sql As String = "SELECT ComponentID AS VALUE, Description AS TEXT FROM tblRMDComponents ORDER BY ComponentID"

        Me.ddlInitComp.Items.Clear()

        ds = DA.GetDataSet(sql)
        If (DA.IsDSEmpty(ds)) Then
            Return
        End If

        Me.ddlInitComp.DataSource = ds
        Me.ddlInitComp.DataValueField = "VALUE"
        Me.ddlInitComp.DataTextField = "TEXT"
        Me.ddlInitComp.DataBind()
        Me.ddlInitComp.Items.Insert(0, "")
        Me.ddlInitComp.SelectedIndex = 0
    End Sub

#End Region

#Region "re-usable load ddl functions"

    Private Function LoadMatTypeDDL(ByRef ddl As DropDownList, ByVal compID As String, ByVal catTypeID As String) As Boolean

        Dim ds As DataSet
        Dim sql As String

        ddl.Items.Clear()
        If (compID Is Nothing) Then
            Return False
        ElseIf (compID.Length <= 0) Then
            Return False
        End If

        sql = ""
        sql += " SELECT MatTypes.Description AS [TEXT], MatTypes.MaterialTypeID AS [VALUE]"
        sql += " FROM tblRMDMaterialTypes MatTypes INNER JOIN"
        sql += " tblRMDCategories Cat ON MatTypes.CategoryID = Cat.CategoryID"
        sql += " WHERE (Cat.CategoryTypeID = " + catTypeID + ") AND (Cat.ComponentID = " + compID + ")"

        ds = DA.GetDataSet(sql)
        If (DA.IsDSEmpty(ds)) Then
            Return False
        End If

        ddl.DataSource = ds
        ddl.DataValueField = "VALUE"
        ddl.DataTextField = "TEXT"
        ddl.DataBind()

        ddl.Items.Insert(0, "")
        ddl.SelectedIndex = 0

        Return True
    End Function

    Private Function LoadListValuesDDL(ByRef ddl As DropDownList, ByVal compID As String, ByVal catTypeID As String, ByVal matTypeID As String, ByVal matListTypeID As String) As Boolean

        Dim ds As DataSet
        Dim sql As String

        ddl.Items.Clear()
        If (compID Is Nothing) Then
            Return False
        ElseIf (compID.Length <= 0) Then
            Return False
        End If

        sql = ""
        sql += " SELECT ListVals.ListValueID AS [VALUE], ListVals.ListValue AS [TEXT]"
        sql += " FROM tblRMDMaterialTypes MatTypes INNER JOIN"
        sql += " tblRMDCategories Cat ON MatTypes.CategoryID = Cat.CategoryID INNER JOIN"
        sql += " tblRMDMaterialLists MatLists ON MatTypes.MaterialTypeID = MatLists.MaterialTypeID INNER JOIN"
        sql += " tblRMDListValues ListVals ON MatLists.MaterialListID = ListVals.MaterialListID"
        sql += " WHERE (Cat.ComponentID = " + compID + ") "
        sql += " AND (Cat.CategoryTypeID = " + catTypeID + ") "
        sql += " AND (MatLists.MaterialListTypeID = " + matListTypeID + ")"
        sql += " AND (MatTypes.MaterialTypeID = " + matTypeID + ")"

        ds = DA.GetDataSet(sql)
        If (DA.IsDSEmpty(ds)) Then
            Return False
        End If

        ddl.DataSource = ds
        ddl.DataValueField = "VALUE"
        ddl.DataTextField = "TEXT"
        ddl.DataBind()

        ddl.Items.Insert(0, "")
        ddl.SelectedIndex = 0

        Return True
    End Function

    Private Function LoadChargeToDDL(ByRef ddl As DropDownList) As Boolean
        Dim ds As DataSet
        Dim sql As String = "SELECT ParameterListValue AS TEXT, ParameterListValue AS VALUE FROM tblParameterListValues WHERE (ParameterListID = '" + PARAM_LIST_ID_CHARGE_TO + "') ORDER BY ParameterListValue"

        ddl.Items.Clear()

        ds = DA.GetDataSet(sql)
        If (DA.IsDSEmpty(ds)) Then
            Return False
        End If

        ddl.DataSource = ds
        ddl.DataValueField = "VALUE"
        ddl.DataTextField = "TEXT"
        ddl.DataBind()
        ddl.Items.Insert(0, "")
        ddl.SelectedIndex = 0

        Return True
    End Function

    Private Function LoadParamListValues(ByRef ddl As DropDownList, ByVal id As String) As Boolean
        Dim ds As DataSet
        Dim sql As String = "SELECT ParameterListValue AS TEXT, ParameterListValue AS VALUE FROM tblParameterListValues WHERE (ParameterListID = '" + id + "') ORDER BY DisplayID"

        ddl.Items.Clear()

        ds = DA.GetDataSet(sql)
        If (DA.IsDSEmpty(ds)) Then
            Return False
        End If

        ddl.DataSource = ds
        ddl.DataValueField = "VALUE"
        ddl.DataTextField = "TEXT"
        ddl.DataBind()
        ddl.Items.Insert(0, "")
        ddl.SelectedIndex = 0

        Return True
    End Function

#End Region


#Region "Save and Cancel"

    Private Function CheckRewInputs() As Boolean
        If (Me.tbDay.Text.Length <= 0) Then
            Master.Msg = "Error: unable to determine the date!"
            Return False
        End If
        If (Me.rblShift.SelectedItem Is Nothing) Then
            Master.Msg = "Error: unable to determine the shift!"
            Return False
        End If
        If (Me.ddlInitRework.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Rework Area."
            Return False
        End If
        If (Me.ddlInitLot.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Lot Number."
            Return False
        End If
        ''needed???
        'If (Me.tbInitSerial.Text.Length <= 0) Then
        '	Master.Msg = "Error: Please enter a Serial Number."
        '	Return False
        'End If
        If (Me.ddlInitComp.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Seat Component."
            Return False
        End If
        If (Me.ddlRewCategory.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Rework Category."
            Return False
        End If
        If (Me.ddlRewReason.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Rework Reason."
            Return False
        End If
        If (Me.ddlRewResponsibility.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Rework Charge To."
            Return False
        End If
        If (Me.ddlRewAction.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Rework Action."
            Return False
        End If
        If (Me.tbRewQuantity.Text.Length <= 0) Then
            Master.Msg = "Error: Please enter a Quantity Reworked."
            Return False
        End If

        Return True
    End Function

    Private Sub SaveRew()
        Dim vehModel As String = ""
        Dim seatStyle As String = ""
        Dim color As String = ""
        Dim prodID As String = ""
        SetLotData(Me.ddlInitLot.SelectedValue.Replace("-", ""), vehModel, seatStyle, color, prodID)

        Dim sql As String = "INSERT INTO tblRMDReworkHistory "
        sql += " (ReworkDate, Shift, ReworkArea, TeamMember, LotNumber, VehicleLine, SeatStyle, Color, Component, "
        sql += " SeatSerialNumber, Category, ReworkReason, ChargeTo, ReworkAction, ReturnedQuantity, Location, ShiftStartDate) "
        sql += " VALUES ({ fn NOW() }"
        sql += ("," + Me.rblShift.SelectedValue)
        sql += (",'" + Me.ddlInitRework.SelectedItem.Text)
        sql += ("','" + Session("UserFirstLastName").ToString())
        sql += ("','" + Me.ddlInitLot.SelectedValue)
        sql += ("','" + vehModel)
        sql += ("','" + seatStyle)
        sql += ("','" + color)
        sql += ("','" + Me.ddlInitComp.SelectedItem.Text)
        sql += ("','" + Me.tbInitSerial.Text)
        sql += ("','" + Me.ddlRewCategory.SelectedItem.Text)
        sql += ("','" + Me.ddlRewReason.SelectedItem.Text)
        sql += ("','" + Me.ddlRewResponsibility.SelectedItem.Text)
        sql += ("','" + Me.ddlRewAction.SelectedItem.Text)
        sql += ("'," + Me.tbRewQuantity.Text)
        sql += (",'" + Request.UserHostAddress)
        sql += ("','" + Me.tbDay.Text)
        sql += ("')")

        DA.ExecSQL(sql)

        Master.tMsg("Save", "Rework history saved.")

    End Sub

    Private Sub ClearRew()
        'clear selections (?)
        Me.ddlRewCategory.SelectedIndex = -1
        Me.ddlRewReason.Items.Clear()
        Me.ddlRewResponsibility.SelectedIndex = -1
        Me.ddlRewAction.SelectedIndex = -1
        Me.tbRewQuantity.Text = ""
    End Sub

    Private Function CheckMatInputs() As Boolean
        If (Me.tbDay.Text.Length <= 0) Then
            Master.Msg = "Error: unable to determine the date!"
            Return False
        End If
        If (Me.rblShift.SelectedItem Is Nothing) Then
            Master.Msg = "Error: unable to determine the shift!"
            Return False
        End If
        If (Me.ddlInitRework.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Rework Area."
            Return False
        End If
        If (Me.ddlInitLot.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Lot Number."
            Return False
        End If
        ''needed???
        'If (Me.tbInitSerial.Text.Length <= 0) Then
        '	Master.Msg = "Error: Please enter a Serial Number."
        '	Return False
        'End If
        If (Me.ddlInitComp.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Seat Component."
            Return False
        End If
        If (Me.ddlMatCategory.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Material Category."
            Return False
        End If
        If (Me.ddlMatPart.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Part."
            Return False
        End If
        If (Me.ddlMatReason.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Disposition Reason."
            Return False
        End If
        If (Me.ddlMatResponsibility.SelectedIndex <= 0) Then
            Master.Msg = "Error: Please select a Material Charge To."
            Return False
        End If
        If (Me.tbMatQtyReturned.Text.Length <= 0) Then
            Master.Msg = "Error: Please enter a Quantity Returned."
            Return False
        End If
        If (Me.tbMatQtyScrapped.Text.Length <= 0) Then
            Master.Msg = "Error: Please enter a Quantity Scrapped."
            Return False
        End If
        If (Me.tbMatQtyTotal.Text.Length <= 0) Then
            Master.Msg = "Error: Please enter a Quantity of Parts Total."
            Return False
        End If

        Return True
    End Function

    Private Sub SaveMat()
        Dim vehModel As String = ""
        Dim seatStyle As String = ""
        Dim color As String = ""
        Dim prodID As String = ""
        SetLotData(Me.ddlInitLot.SelectedValue.Replace("-", ""), vehModel, seatStyle, color, prodID)

        Dim sql As String = "INSERT INTO tblRMDMaterialDispositionHistory "
        sql += " (DispositionDate, Shift, ReworkArea, TeamMember, LotNumber, VehicleLine, SeatStyle, Color, Component, "
        sql += " SeatSerialNumber, Category, PartNumber, DispositionReason, ChargeTo, ReturnQuantity, "
        sql += " ScrapQuantity, TotalQuantity, Location, PartDescription, ShiftStartDate) "
        sql += " VALUES ({ fn NOW() }"
        sql += ("," + Me.rblShift.SelectedValue)
        sql += (",'" + Me.ddlInitRework.SelectedItem.Text)
        sql += ("','" + Session("UserFirstLastName").ToString())
        sql += ("','" + Me.ddlInitLot.SelectedValue)
        sql += ("','" + vehModel)
        sql += ("','" + seatStyle)
        sql += ("','" + color)
        sql += ("','" + Me.ddlInitComp.SelectedItem.Text)
        sql += ("','" + Me.tbInitSerial.Text)
        sql += ("','" + Me.ddlMatCategory.SelectedItem.Text)
        sql += ("','" + Me.tbMatPartNo.Text)
        sql += ("','" + Me.ddlMatReason.SelectedItem.Text)
        sql += ("','" + Me.ddlMatResponsibility.SelectedItem.Text)
        sql += ("'," + Me.tbMatQtyReturned.Text)
        sql += ("," + Me.tbMatQtyScrapped.Text)
        sql += ("," + Me.tbMatQtyTotal.Text)
        sql += (",'" + Request.UserHostAddress)
        sql += ("','" + Me.ddlMatPart.SelectedItem.Text)
        sql += ("','" + Me.tbDay.Text)
        sql += ("')")

        DA.ExecSQL(sql)

        Master.tMsg("Save", "Material Disposition history saved.")
    End Sub

    Private Sub ClearMat()
        'clear selections (?)
        Me.ddlMatCategory.SelectedIndex = -1
        Me.ddlMatPart.Items.Clear()
        Me.tbMatPartNo.Text = ""
        Me.ddlMatReason.Items.Clear()
        Me.ddlMatResponsibility.SelectedIndex = -1
        Me.tbMatQtyScrapped.Text = ""
        Me.tbMatQtyTotal.Text = ""
        Me.tbMatQtyReturned.Text = ""
        Me.tbMatPartNo.Text = ""
    End Sub

#End Region

#Region "other functions"

    Private Sub SetLotData(ByVal lotNum10 As String, ByRef vehModel As String, ByRef seatStyle As String, ByRef color As String, ByRef prodID As String)
        Dim ds As DataSet
        Dim sql = "EXEC procRMDGetProductDataByLotNum '" + lotNum10 + "'"
        ds = DA.GetDataSet(sql)
        If (DA.IsDSEmpty(ds)) Then
            Return
        End If
        vehModel = ds.Tables(0).DefaultView.Table.Rows(0)(COL_PROD_DATA_VEH_MODEL).ToString()
        seatStyle = ds.Tables(0).DefaultView.Table.Rows(0)(COL_PROD_DATA_SEAT_CODE).ToString()
        color = ds.Tables(0).DefaultView.Table.Rows(0)(COL_PROD_DATA_COLOR).ToString()
        prodID = ds.Tables(0).DefaultView.Table.Rows(0)(COL_PROD_DATA_PROD_ID).ToString()
    End Sub

#End Region


End Class