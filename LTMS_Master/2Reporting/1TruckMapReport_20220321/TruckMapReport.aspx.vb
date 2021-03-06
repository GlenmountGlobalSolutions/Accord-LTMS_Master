Imports System.Data.SqlClient
Imports System.Drawing
Imports Telerik.Web.UI

Public Class TruckMapReport
    Inherits System.Web.UI.Page

    Public Const LOT_NUM_DELIMITER As String = ","

    Enum procPSGetShipSchedColumnType
        table_id
        LotNumber
        LotNumber8
        LotNumber8NoBlanks
        LotNumberNoBlanks
        LotNumber10
        SubLotNumber
        SequenceDT
        OnHold
        TruckMapCreated
        Load
        FrameCode
        Model
        BC
        Qty
        ShipTime
        ArrivalTime
        ApproxSeqTime
        Trailer
        DriverName
        ActualDepartTime
        SetexOrder
    End Enum

#Region "Event handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                hidLastTab.Value = "0"

                txtDay.Text = Now().ToString("MM/dd/yyyy")
                txtShipDate_Begin.Text = Date.Now().AddDays(-1).ToString("MM/dd/yyyy")
                txtShipDate_End.Text = Now().ToString("MM/dd/yyyy")


                lblSelectBroadcastPointID.Text = My.Resources.txtBroadcastPointCaption + ":"
                DataAccess.BroadcastPointID.Load_ddlBroadcastPointID(Me.ddlBroadcastPointID, Request, Server)

                RefreshNewReports()
            End If
            GetBroadcastPointKey()

            hidShowNewReport.Value = ""

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub TruckMapReport_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        Try
            RefreshNewReports()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCreateReport_Click(sender As Object, e As System.EventArgs) Handles cmdCreateReport.Click
        Try
            CreateNewReport()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdRevInc_Click(sender As Object, e As System.EventArgs) Handles cmdRevInc.Click
        Try
            InsertRevision()
            LoadLots()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibNext_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibNext.Click
        Dim dt As Date
        Try
            If (Date.TryParse(Me.txtDay.Text, dt)) Then
                txtDay.Text = dt.AddDays(1).ToString("MM/dd/yyyy")
                LoadLots()
            Else
                Master.Msg = "Invalid Date"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibPrev_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibPrev.Click
        Dim dt As Date
        Try
            If (Date.TryParse(Me.txtDay.Text, dt)) Then
                txtDay.Text = dt.AddDays(-1).ToString("MM/dd/yyyy")
                LoadLots()
            Else
                Master.Msg = "Invalid Date"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRefreshHistoryList_Click(sender As Object, e As System.EventArgs) Handles cmdRefreshHistoryList.Click
        Try
            LoadArchivedReportFiles()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cblLotNum_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cblLotNum.SelectedIndexChanged
        LoadTruckMapDefaults()
    End Sub

    Private Sub cmdReprint_Click(sender As Object, e As System.EventArgs) Handles cmdReprint.Click
        LogTrc("Reprint", Page, "Reprint Truck Map Report.  BLNumber :: " + Me.ddlOldRepFile.SelectedValue)
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

    Private Sub ddlBroadcastPointID_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlBroadcastPointID.SelectedIndexChanged
        RefreshNewReports()
    End Sub

#End Region

#Region "Private Methods"

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdReprint)
            Master.Secure(Me.cmdRefresh)
            Master.Secure(Me.cmdCreateReport)
            Master.Secure(Me.cmdRevInc)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub RefreshNewReports()
        Try
            LoadDriverDDL()
            LoadTruckMapDefaults()
            LoadLots()
            LoadBLNum()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub CreateNewReport()
        Dim blnExistingBLNumber As Boolean
        Try
            Dim err As String = ValidateLotSelection()

            'check input lots
            If (err.Length > 0) Then
                Master.Msg = err
            Else
                blnExistingBLNumber = ExistingBLNumber()

                SaveReportData()


                If Not blnExistingBLNumber Then
                    LogTrc("Print", Page, "Printing New Truck Map Report.  BLNumber :: " + Me.hidBLNumber.Value + ", Lots : " + GetSelectedLots())
                    'UpdateBLNumber(Me.hidBLNumber.Value)
                End If

                hidShowNewReport.Value = "true"

                RefreshNewReports()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    ' adds the data fro the report to the appropriate tables.
    Private Function SaveReportData() As Boolean
        Dim bResult As Boolean = False
        Dim colParameters As New List(Of SqlParameter)
        Dim oSqlParameter As SqlParameter
        Dim colOutput As List(Of SqlParameter)
        Dim status As String = ""
        Dim errMessage As String = ""

        Try
            oSqlParameter = New SqlParameter("@ShippingDate", SqlDbType.VarChar, 50)
            oSqlParameter.Value = Me.txtDay.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New Data.SqlClient.SqlParameter("@LotNumber8s", SqlDbType.VarChar, 50)
            oSqlParameter.Value = GetSelectedLots()
            colParameters.Add(oSqlParameter)

            oSqlParameter = New Data.SqlClient.SqlParameter("@TrailerID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = Me.txtTrailer.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New Data.SqlClient.SqlParameter("@Driver", SqlDbType.VarChar, 50)
            oSqlParameter.Value = ddlDriver.SelectedItem.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New Data.SqlClient.SqlParameter("@BLNumber", SqlDbType.VarChar, 50)
            oSqlParameter.Value = (Me.lblBLNumberCombined.Text)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 500)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New Data.SqlClient.SqlParameter("@BP", SqlDbType.Int)
            oSqlParameter.Value = ddlBroadcastPointID.SelectedItem.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BLNumbersOutput", SqlDbType.VarChar, 500)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)



            colOutput = DA.ExecSP("procPSGetTruckMapReport", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        errMessage = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@BLNumbersOutput" Then
                        Me.lblBLNumberCombined11.Value = oParameter.Value.ToString()
                        'MsgBox(oParameter.Value.ToString())
                    End If
                End With
            Next

            If (status <> "TRUE") Then
                bResult = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return bResult
    End Function

    Private Function ValidateLotSelection() As String
        Dim status As String = ""
        Dim errMessage As String = ""
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Try
            If (Me.hidBLNumber.Value.Length = 0) Then
                status = "FALSE"
                errMessage = "BLNumber was not supplied."
            Else
                oSqlParameter = New SqlParameter("@LotNumber8s", SqlDbType.VarChar, 8000)
                oSqlParameter.Value = GetSelectedLots()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 500)
                oSqlParameter.Direction = ParameterDirection.Output
                colParameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("procPSGetTruckMapTotalJobs", colParameters)

                For Each oParameter In colOutput
                    With oParameter
                        If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                            status = oParameter.Value.ToString()
                        ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                            errMessage = oParameter.Value.ToString()
                        End If
                    End With
                Next
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        If (status <> "TRUE") Then
            Return errMessage
        Else
            Return ""    'implies OK
        End If

    End Function

    Private Function ExistingBLNumber() As Boolean
        Dim sql As String = ""
        Dim ds As DataSet
        Dim text As String = ""
        Try
            sql = "exec procPSGetTruckMapScreenDefaults '" + Me.GetSelectedLots + "'"

            ds = DA.GetDataSet(sql)

            'check table
            If (ds Is Nothing) Then
                Return False
            End If
            If (ds.Tables.Count < 1) Then
                Return False
            End If
            If (ds.Tables(0).DefaultView.Table.Rows.Count <= 0) Then
                Return False
            End If

            text = ds.Tables(0).DefaultView.Table.Rows(0)("BLNumber").ToString()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return text.Length > 0

    End Function

    Private Function LoadArchivedReportFiles() As Boolean
        Dim bResult As Boolean = False
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim dt As Date
        Try
            If (hidLastTab.Value = "1") Then
                If (Date.TryParse(txtShipDate_Begin.Text, dt) = False) Then
                    Master.Msg = "Invalid Beginning Date"
                ElseIf (Date.TryParse(txtShipDate_End.Text, dt) = False) Then
                    Master.Msg = "Invalid End Date"
                Else

                    oSqlParameter = New SqlParameter("@ShipDate_Begin", SqlDbType.Date)
                    oSqlParameter.Value = txtShipDate_Begin.Text
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@ShipDate_End", SqlDbType.Date)
                    oSqlParameter.Value = txtShipDate_End.Text()
                    colParameters.Add(oSqlParameter)

                    ds = DA.GetDataSet("procGetTruckMap_History", colParameters)

                    Me.ddlOldRepFile.Items.Clear()

                    If (DA.IsDSEmpty(ds)) Then
                        Master.Msg = "No Reports Available for Selected Date Range."
                        bResult = False
                    Else
                        Me.ddlOldRepFile.DataSource = ds
                        Me.ddlOldRepFile.DataTextField = "BLNumber"
                        Me.ddlOldRepFile.DataValueField = "BLNumber"
                        Me.ddlOldRepFile.DataBind()

                        bResult = True
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult
    End Function

    Private Function UpdateBLNumber(ByVal newBLNum As String) As Boolean
        Dim sql As String = "UPDATE tblApplicationParameters SET ApplicationParameterValue = '" + newBLNum + "' WHERE (ApplicationID = '0001') AND (ApplicationParameterTypeID = '0121')"
        DA.ExecSQL(sql)
        Return True
    End Function


    Private Function GetSelectedLots() As String
        Dim i As Int32
        Dim retval As String = ""
        Try
            For i = 0 To (Me.cblLotNum.Items.Count - 1)
                If (Me.cblLotNum.Items(i).Selected = True) Then
                    retval += (Me.cblLotNum.Items(i).Value + LOT_NUM_DELIMITER)
                End If
            Next
            'remove last delimiter
            If (retval.Length > 2) Then
                retval = retval.Substring(0, (retval.Length() - LOT_NUM_DELIMITER.Length))
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return retval
    End Function


    Private Sub LoadBLNum()
        Dim ds As DataSet
        Dim sql As String = "SELECT (SELECT ApplicationParameterValue FROM tblApplicationParameters WHERE (ApplicationID = '0001') AND (ApplicationParameterTypeID = '0120')) AS [BLPrefix], (SELECT ApplicationParameterValue FROM tblApplicationParameters WHERE (ApplicationID = '0001') AND (ApplicationParameterTypeID = '0121')) AS [BLNum]"
        Dim blNum As String = ""
        Dim blPrefix As String = ""

        Try
            ds = DA.GetDataSet(sql)

            Me.hidBLNumber.Value = ""
            Me.lblBLNumberCombined.Text = ""

            If (DA.IsDSEmpty(ds)) Then
                Return
            End If
            If (ds.Tables(0).DefaultView.Table.Rows.Count <= 0) Then
                Return
            End If

            blPrefix = ds.Tables(0).DefaultView.Table.Rows(0)("BLPrefix").ToString()
            blNum = ds.Tables(0).DefaultView.Table.Rows(0)("BLNum").ToString()
            blNum = (Convert.ToInt32(blNum) + 1).ToString()
            blNum = blNum.PadLeft(7, Convert.ToChar("0"))
            Me.hidBLNumber.Value = blNum
            Me.lblBLNumberCombined.Text = blPrefix + blNum

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadDriverDDL()
        Try
            Dim ds As DataSet
            Dim sql As String = "SELECT ParameterListValue AS [TEXT], ParameterListValue AS [VALUE] FROM tblParameterListValues WHERE (ParameterListID = '51') ORDER BY DISPLAYID"

            ds = DA.GetDataSet(sql)

            Me.ddlDriver.Items.Clear()
            If (DA.IsDSEmpty(ds) = False) Then

                Me.ddlDriver.DataSource = ds
                Me.ddlDriver.DataTextField = "TEXT"
                Me.ddlDriver.DataValueField = "VALUE"
                Me.ddlDriver.DataBind()

                Me.ddlDriver.Items.Insert(0, "")
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadTruckMapDefaults()
        Dim sql As String = ""
        Dim ds As DataSet
        Dim text As String = ""

        Try
            sql = "exec procPSGetTruckMapScreenDefaults '" + Me.GetSelectedLots() + "'"

            ds = DA.GetDataSet(sql)

            'check table
            If (ds Is Nothing) Then
                Return
            End If
            If (ds.Tables.Count < 1) Then
                Return
            End If
            If (ds.Tables(0).DefaultView.Table.Rows.Count <= 0) Then
                Return
            End If

            text = ds.Tables(0).DefaultView.Table.Rows(0)("BLNumber").ToString()

            If text.Length > 0 Then
                Me.lblBLNumberCombined.Text = text
                Me.lblBLNumberCombined.ForeColor = Color.Red
            Else
                LoadBLNum()
                Me.lblBLNumberCombined.ForeColor = Color.Empty
            End If

            Me.txtTrailer.Text = ds.Tables(0).DefaultView.Table.Rows(0)("TrailerID").ToString()

            text = ds.Tables(0).DefaultView.Table.Rows(0)("Driver").ToString()
            If IsNothing(text.Length) Or text = "" Then
                ddlDriver.SelectedValue = "None"
            Else

                ddlDriver.SelectedValue = ddlDriver.Items.FindByText(text).Value
            End If


            LoadRevision()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function LoadLots() As Boolean
        Dim bResult As Boolean = False
        Dim strDt As String = ""
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim ds As DataSet
        Dim text As String = ""
        Dim val As String = ""
        Dim i, j As Integer
        Dim status As String = ""

        Try
            If (hidLastTab.Value = "0") Then

                strDt = Utility.FormattedDate(Me.txtDay.Text)

                If (strDt.Length <= 0) Then
                    Master.Msg = "Error: Please select a valid date."
                    bResult = False
                ElseIf (ddlBroadcastPointID.SelectedIndex = 0) Then
                    If (IsPostBack) Then
                        Master.Msg = "Error: Please select a " + My.Resources.txtBroadcastPointCaption + "."
                    End If
                    bResult = False
                Else

                    oSqlParameter = New SqlParameter("@vchBegDT", SqlDbType.VarChar, 30)
                    oSqlParameter.Value = strDt
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
                    oSqlParameter.Value = 1
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@SetexOrderFlag", SqlDbType.Bit)
                    oSqlParameter.Value = 1
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
                    oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
                    colParameters.Add(oSqlParameter)

                    ds = DA.GetDataSet("procPSGetShipSched", colParameters)

                    Me.cblLotNum.Items.Clear()

                    If (ds Is Nothing) Then
                        bResult = False
                    ElseIf (ds.Tables.Count < 1) Then
                        bResult = False
                    ElseIf (ds.Tables(0).Rows.Count > 0) Then
                        'insert first item
                        text = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipSchedColumnType.LotNumberNoBlanks).ToString()
                        val = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipSchedColumnType.LotNumber8NoBlanks).ToString()

                        'If ds.Tables(0).DefaultView.Table.Rows(0)("TruckMapCreated") Then
                        '    text = "<SPAN style=""text-decoration:line-through;"">" & text & "</SPAN>"
                        'End If
                        If Me.cmdReprint.Enabled = False Then
                            If CBool(ds.Tables(0).DefaultView.Table.Rows(0)("TruckMapCreated")) Then
                                'Do Nothing
                                j = 0
                            Else
                                status = CheckCompleteJobTotal(val)
                                If status = "TRUE" Then
                                    Me.cblLotNum.Items.Insert(0, text)
                                    Me.cblLotNum.Items(0).Value = val
                                    j = 1
                                Else
                                    'Do Nothing
                                    j = 0
                                End If
                            End If

                        Else
                            If CBool(ds.Tables(0).DefaultView.Table.Rows(0)("TruckMapCreated")) Then
                                text = "<SPAN style=""text-decoration:line-through;"">" & text & "</SPAN>"
                            End If
                            Me.cblLotNum.Items.Insert(0, text)
                            Me.cblLotNum.Items(0).Value = val
                            j = 1
                        End If

                        For i = 1 To (ds.Tables(0).DefaultView.Table.Rows.Count - 1)
                            text = ds.Tables(0).DefaultView.Table.Rows(i)(procPSGetShipSchedColumnType.LotNumberNoBlanks).ToString()
                            val = ds.Tables(0).DefaultView.Table.Rows(i)(procPSGetShipSchedColumnType.LotNumber8NoBlanks).ToString()
                            If j = 0 Then
                                'If (val <> Me.cblLotNum.Items(j - 1).Value) Then
                                'If ds.Tables(0).DefaultView.Table.Rows(i)("TruckMapCreated") Then
                                '    text = "<SPAN style=""text-decoration:line-through;"">" & text & "</SPAN>"
                                'End If
                                If Me.cmdReprint.Enabled = False Then
                                    If CBool(ds.Tables(0).DefaultView.Table.Rows(i)("TruckMapCreated")) Then
                                        'Do Nothing
                                    Else
                                        status = CheckCompleteJobTotal(val)
                                        If status = "TRUE" Then
                                            Me.cblLotNum.Items.Insert(j, text)
                                            Me.cblLotNum.Items(j).Value = val
                                            j = (j + 1)
                                        End If
                                    End If

                                    'End If
                                Else
                                    If CBool(ds.Tables(0).DefaultView.Table.Rows(i)("TruckMapCreated")) Then
                                        text = "<SPAN style=""text-decoration:line-through;"">" & text & "</SPAN>"
                                    End If
                                    Me.cblLotNum.Items.Insert(j, text)
                                    Me.cblLotNum.Items(j).Value = val
                                    j = (j + 1)
                                End If
                            Else
                                If (val <> Me.cblLotNum.Items(j - 1).Value) Then
                                    'If ds.Tables(0).DefaultView.Table.Rows(i)("TruckMapCreated") Then
                                    '    text = "<SPAN style=""text-decoration:line-through;"">" & text & "</SPAN>"
                                    'End If
                                    If Me.cmdReprint.Enabled = False Then
                                        If CBool(ds.Tables(0).DefaultView.Table.Rows(i)("TruckMapCreated")) Then
                                            'Do Nothing
                                        Else
                                            status = CheckCompleteJobTotal(val)
                                            If status = "TRUE" Then
                                                Me.cblLotNum.Items.Insert(j, text)
                                                Me.cblLotNum.Items(j).Value = val
                                                j = (j + 1)
                                            End If
                                        End If

                                    Else
                                        If CBool(ds.Tables(0).DefaultView.Table.Rows(i)("TruckMapCreated")) Then
                                            text = "<SPAN style=""text-decoration:line-through;"">" & text & "</SPAN>"
                                        End If
                                        Me.cblLotNum.Items.Insert(j, text)
                                        Me.cblLotNum.Items(j).Value = val
                                        j = (j + 1)
                                    End If
                                End If
                            End If
                        Next

                        bResult = True
                    End If

                    bResult = LoadRevision()

                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function CheckCompleteJobTotal(ByVal LotNumber As String) As String
        Dim status As String = ""
        Dim message As String = ""

        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As New List(Of SqlParameter)

        'CREATE PROCEDURE procPSGetTruckMapTotalJobs @LotNumber8s varchar(8000),
        '@Status Varchar(80) OUT, 
        '@ErrorMsg Varchar(500) OUT
        oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 80)
        oSqlParameter.Value = LotNumber
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 500)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        colOutput = DA.ExecSP("procCheckJobsCompleted", colParameters)

        For Each oParameter In colOutput
            With oParameter
                If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                    status = oParameter.Value.ToString()
                ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                    message = oParameter.Value.ToString()
                End If
            End With
        Next

        If (status <> "TRUE") Then
            Return "FALSE"
        Else
            Return "TRUE"
        End If

    End Function

    Private Function LoadRevision() As Boolean
        Dim bResult As Boolean = False
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim strRevisionType As String
        Dim AllOrdersFlag As Boolean = False

        Try
            Me.lblDate.Text = Me.txtDay.Text
            Me.lblRevNo.Text = "?"
            Me.lblRevComments.Text = ""

            strRevisionType = "0003" 'truck map  - see table [tblRevisionTypes]

            oSqlParameter = New SqlParameter("@TrackingID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = Me.lblBLNumberCombined.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@RevisionTypeID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = strRevisionType
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
            colParameters.Add(oSqlParameter)

            ds = DA.GetDataSet("procGetLastRevisionComment", colParameters)

            If (DA.IsDSEmpty(ds)) Then
                Me.lblRev.Text = "1"
                bResult = False
            Else
                Me.lblRevNo.Text = ds.Tables(0).DefaultView.Table.Rows(0)("RevisionNumber").ToString()
                Me.lblRevComments.Text = ds.Tables(0).DefaultView.Table.Rows(0)("Comment").ToString()
                Me.lblRev.Text = ds.Tables(0).DefaultView.Table.Rows(0)("NextRevisionNumber").ToString()
                bResult = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult
    End Function

    Private Sub InsertRevision()
        Try
            Dim status As String = ""
            Dim message As String = ""
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            Dim strRevisionType As String

            strRevisionType = "0003" 'truck map  - see table [tblRevisionTypes]

            oSqlParameter = New SqlParameter("@RevisionTypeID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = strRevisionType
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@RevisionNumber", SqlDbType.Int)
            oSqlParameter.Value = Me.lblRev.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Comment", SqlDbType.VarChar, 500)
            oSqlParameter.Value = Me.txtComment.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@TrackingID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = Me.lblBLNumberCombined.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procPSInsertRevision", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next


            If (status <> "TRUE") Then
                Master.tMsg("Revision", "Error: unable to update Truck Map Revision for " + txtDay.Text + ".<br> S.P. Status: " + status + ".<br>S.P. Message: " + message)
            Else
                Master.tMsg("Revision", "Truck Map Revision for " + txtDay.Text + " has been updated.")
            End If

            LoadRevision()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

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


#End Region



End Class