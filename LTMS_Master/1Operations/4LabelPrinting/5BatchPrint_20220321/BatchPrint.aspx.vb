Imports System.Data.SqlClient

Public Class BatchPrint
    Inherits System.Web.UI.Page

    Public Const NO_TOTAL As String = "?"

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strPrinter As String = ""
        Dim strSelectedPrinter As String = ""
        Dim strJSForSetSelectedPrinter As String = ""

        Try
            If (Not IsPostBack) Then
                lblSelectBroadcastPointID.Text = My.Resources.txtBroadcastPointCaption + ":"

                'grab the selected printer from the request object and run client-side script to select it from the printer drop down list
                'this is needed for each postback that occurs because the printer drop down won't keep its selection (not a dot net drop down)
                If (hidSelectedPrinterPrelim.Value.Length > 0) Then
                    strSelectedPrinter = hidSelectedPrinterPrelim.Value
                End If

                'lets populate the drop down list of printers on the client
                populatePrinterDropDown(strSelectedPrinter)

                ' populate the Broadcast points
                DataAccess.BroadcastPointID.Load_ddlBroadcastPointID(Me.ddlBroadcastPointID, Request, Server)

                tbDay.Text = Now().ToString("MM/dd/yyyy")
                hidLastTab.Value = "0"

                'let's populate the dropdown list of label types (driver, passenger, weld, etc.)
                populateLabelTypeDropDown()
                LoadDay()
            End If

            populatePrinterDropDown(strSelectedPrinter)


            treeSingleDay.Attributes.Add("onClick", "return GetSelectedNode(event, 'MainContent_treeSingleDay_Data');")
            hidPrintString.Value = ""

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub BatchPrint_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cbHold_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbHold.CheckedChanged
        Try
            LoadDay()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        Try
            LoadDay()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlBroadcastPointID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlBroadcastPointID.SelectedIndexChanged
        Try
            DataAccess.BroadcastPointID.Set_ddlBroadcastPoint_Cookie(Me.ddlBroadcastPointID.SelectedValue, Response)
            LoadDay()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLabelTypes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLabelTypes.SelectedIndexChanged
        Try
            populateSeatStyleDropDown()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub btnPrintJob_Click(sender As Object, e As System.EventArgs) Handles btnPrintJob.Click
        SendPrintJob()
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        Master.Secure(Me.btnPrintJob)
        Master.Secure(Me.cmdRefresh)

        'Master.Secure Me.cmdColSingleDay)
        'Master.Secure = Me.cmdExpSingleDay
        'Master.Secure = Me.ibNext
        'Master.Secure = Me.ibPrev

    End Sub

    'this sub populates the dropdown list for the client's printers
    Private Sub populatePrinterDropDown(ByVal theSelectedPrinter As String)
        Dim sb As New StringBuilder

        Try
            sb.Append("<script language=""VBScript"">PopulatePrinters(")
            sb.Append(ControlChars.Quote)
            sb.Append(theSelectedPrinter)
            sb.Append(ControlChars.Quote)
            sb.Append(")</script>")

            Page.ClientScript.RegisterStartupScript(Me.GetType, "populatePrinter", sb.ToString())

        Catch ex As Exception

        End Try
    End Sub

    Private Sub populateLabelTypeDropDown()
        Dim colParameters As New List(Of SqlParameter)

        Try

            ddlLabelTypes.DataTextField = "description"
            ddlLabelTypes.DataValueField = "labelID"
            ddlLabelTypes.DataSource = DA.GetDataSet("procGetLabels", colParameters)
            ddlLabelTypes.DataBind()

            ddlLabelTypes.Items.Insert(0, "Select Label Type")
            ddlLabelTypes.Items(0).Value = "0"

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub populateSeatStyleDropDown()
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim ds As DataSet = Nothing

        Try

            oSqlParameter = New SqlParameter("@labelID", SqlDbType.VarChar, 30)
            oSqlParameter.Value = ddlLabelTypes.SelectedValue
            colParameters.Add(oSqlParameter)

            ddlSeatStyle.ClearSelection()
            ddlSeatStyle.DataTextField = "productid"
            ddlSeatStyle.DataValueField = "productid"
            ddlSeatStyle.DataSource = DA.GetDataSet("procGetSeatStyle", colParameters)
            ddlSeatStyle.DataBind()

            'add default list item
            ddlSeatStyle.Items.Insert(0, "Select Seat Style")
            ddlSeatStyle.Items(0).Value = "0"

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ClearDayTreesTotals()
        Me.treeSingleDay.Nodes.Clear()
        Me.lblTotalSingleDay.Text = NO_TOTAL
    End Sub

    Private Sub LoadDayDateLabel()
        Try
            Dim dt As Date

            If (Date.TryParse(Me.tbDay.Text, dt)) Then
                lblSingleDayOfWeek.Text = dt.ToString("dddd, MM/dd/yyyy")
            Else
                Master.Msg = "Invalid Date"
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function LoadDay() As Boolean
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim ds As DataSet = Nothing
        Dim bResult As Boolean
        Dim hold As Integer = 1

        Dim strDate As String = Utility.FormattedDate(Me.tbDay.Text)
        LoadDayDateLabel()

        Try

            If (strDate.Length <= 0) Then
                Master.Msg = "Error: Please select a valid date."
                bResult = False
            ElseIf (ddlBroadcastPointID.SelectedIndex = 0) Then
                If (IsPostBack) Then
                    Master.Msg = "Error: Please select a " + My.Resources.txtBroadcastPointCaption + "."
                End If
                bResult = False
            Else

                If (Me.cbHold.Checked) Then
                    hold = 0
                End If

                'call sp with selected week's monday's date
                'call stored procedure and get 9 tables in a dataset (last table not used here)

                oSqlParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
                oSqlParameter.Value = strDate
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
                oSqlParameter.Value = hold
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@NumDays", SqlDbType.Int)
                oSqlParameter.Value = 1
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
                oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
                colParameters.Add(oSqlParameter)

                ' SETEX Orders were selected to be returned
                ds = DA.GetDataSet("procPSGetProdSched", colParameters)

                ''error checking
                If (DA.IsDSEmpty(ds)) Then
                    Me.treeSingleDay.Nodes.Clear()
                Else
                    'clear all trees and totals
                    ClearDayTreesTotals()

                    If (ds IsNot Nothing) Then
                        If (ds.Tables.Count >= 2) Then
                            bResult = bResult Or PopulateTreeView(Me.treeSingleDay, ds.Tables(0))
                            TreeExpand(Me.treeSingleDay, 1)
                            Me.lblTotalSingleDay.Text = ds.Tables(1).DefaultView.Table.Rows(0)(0).ToString()
                        End If
                    End If
                End If

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    ''' <summary>
    ''' This method will expand the tree view
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="numLevels">Only supports to level 1 or 2</param>
    ''' <returns></returns>
    ''' <remarks>From original 1.1 code</remarks>
    Private Function TreeExpand(ByRef tree As TreeView, ByVal numLevels As Integer) As Boolean
        Dim bResult As Boolean = False

        Dim objNode As TreeNode
        Dim objNode2 As TreeNode

        Try
            If (tree IsNot Nothing And tree.Nodes.Count > 0) Then
                If (numLevels = 1) Or (numLevels = 2) Then

                    'expand all nodes (level 1)
                    For Each objNode In tree.Nodes
                        objNode.Expanded = True
                    Next

                    If (numLevels = 2) Then

                        'expand all nodes (level 2)
                        For Each objNode In tree.Nodes
                            For Each objNode2 In objNode.ChildNodes
                                objNode2.Expanded = True
                            Next
                        Next
                    End If

                    'tree.Nodes(0).Selected = True

                    bResult = True
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function PopulateTreeView(ByRef tree As TreeView, ByRef dt As DataTable) As Boolean
        Dim shiftNode As New TreeNode
        Dim lotNode As New TreeNode
        Dim subLotNode As New TreeNode

        Dim bResult As Boolean = False
        Dim strShift As String = ""
        Dim strLot As String = ""
        Dim strSubLot As String = ""
        Dim strBC As String = ""
        Dim strQty As String = ""
        Dim strNode As String = ""
        Dim strSeqDT As String = ""
        Dim strOnHold As String = ""
        Dim shiftChange As Boolean = False
        Dim holdFlag As Boolean = False

        Dim SetexFlag As Boolean = False
        Dim SetexSchedule As Boolean = False

        Const SHIFT As String = "Shift "
        Const ON_HOLD As String = " <b>ON HOLD</b>"

        Dim qtyLot As Int32 = 0
        Dim i As Int32

        Try
            'clear out tree control
            tree.Nodes.Clear()

            If (dt IsNot Nothing) Then
                If (dt.Rows.Count > 0) Then

                    'add all nodes
                    For i = 0 To dt.Rows.Count - 1

                        strBC = dt.Rows(i)(ProductionSchedule.psGetSchedule.BC).ToString()
                        strOnHold = dt.Rows(i)(ProductionSchedule.psGetSchedule.OnHold).ToString()
                        strSeqDT = dt.Rows(i)(ProductionSchedule.psGetSchedule.SequenceDT).ToString()
                        strQty = dt.Rows(i)(ProductionSchedule.psGetSchedule.Qty).ToString()

                        If (strQty = "") Then
                            strQty = "0"
                        End If

                        'add new shift node if needed
                        If (strShift <> dt.Rows(i)(ProductionSchedule.psGetSchedule.Shift).ToString()) Then
                            strShift = dt.Rows(i)(ProductionSchedule.psGetSchedule.Shift).ToString()
                            shiftNode = Nothing
                            shiftNode = New TreeNode
                            shiftNode.Text = SHIFT + strShift + dt.Rows(i)(ProductionSchedule.psGetSchedule.ShiftSuffix).ToString()
                            shiftNode.Value = strSeqDT
                            tree.Nodes.Add(shiftNode)
                            shiftChange = True
                        End If

                        'add new lot if needed
                        If ((strLot <> dt.Rows(i)(ProductionSchedule.psGetSchedule.LotNumber8).ToString()) Or (shiftChange)) Then
                            'add quantity to previous lot and on hold text if needed
                            lotNode.Text += (" : " + qtyLot.ToString())
                            If (holdFlag) Then
                                lotNode.Text += ON_HOLD
                            End If

                            qtyLot = 0
                            holdFlag = False

                            strLot = dt.Rows(i)(ProductionSchedule.psGetSchedule.LotNumber8).ToString()
                            lotNode = Nothing
                            lotNode = New TreeNode

                            lotNode.Text = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3)

                            If (strOnHold.ToUpper = "TRUE") Then
                                'lotNode.Text = lotNode.Text + ON_HOLD
                                holdFlag = True
                            End If

                            shiftNode.ChildNodes.Add(lotNode)
                        End If
                        'reset shift change flag
                        shiftChange = False

                        'add sublot
                        strSubLot = dt.Rows(i)(ProductionSchedule.psGetSchedule.SubLot).ToString()
                        subLotNode = Nothing
                        subLotNode = New TreeNode
                        subLotNode.Text = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3) + "-" + strSubLot + " : " + strQty + " " + strBC
                        ''subLotNode.Value = strSeqDT

                        lotNode.ChildNodes.Add(subLotNode)

                        qtyLot += Convert.ToInt32(strQty)

                    Next

                    'add total to the very last lot
                    lotNode.Text += (" : " + qtyLot.ToString())

                    bResult = True

                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    '*********************************************************************************
    'Name:  John Rose
    'Date:  10/9/2007
    'Description:  This function calls a stored procedure that returns a command string.  
    'The command string will be passed to an executable on the client.
    'Input parameter:  strPrinter - this is the printer that the user selected. It's a parameter passed to the sp.
    'Output:  strPrintData - this is the command line passed to the executable that actually does the printing of labels
    '****************************************************************************************
    Private Function createPrintCommandLine(ByVal strPrinter As String) As String

        Dim strPrintData As String = ""
        Dim strSelectedTreeNode As String = ""

        Try
            'create parameter objects
            'these parameters are used for both lot and style print
            Dim par4 As New SqlParameter("@BatchPrintData", SqlDbType.VarChar)
            par4.Size = 1000
            par4.Direction = ParameterDirection.Output

            Dim param2 As New SqlParameter("@LabelType", SqlDbType.VarChar)
            param2.Size = 100
            param2.Value = ddlLabelTypes.SelectedValue

            Dim param3 As New SqlParameter("@PrintQueue", SqlDbType.VarChar)
            param3.Size = 100
            param3.Value = strPrinter


            Using myConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString)
                myConnection.Open()

                Using objCmd As New SqlCommand
                    objCmd.Connection = myConnection
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.CommandText = "procGetBatchPrintData"

                    If (hidLastTab.Value = "0") Then    ' "Print From Lot" Tab is selected 
                        'get the lot number from the selected tree node
                        strSelectedTreeNode = parseLotNumber(hidNodeValue.Value.ToString())

                        'create parameter objects and add to command arobject
                        Dim param1 As New SqlParameter("@SelectedLot", SqlDbType.VarChar)
                        param1.Size = 20
                        param1.Value = strSelectedTreeNode

                        objCmd.Parameters.Add(param1)
                        objCmd.Parameters.Add(param2)
                        objCmd.Parameters.Add(param3)
                        objCmd.Parameters.Add(par4)
                    Else    'If (hidLastTab.Value = "1") Then   ' "Print From Style" Tab is selected

                        Dim paramProductId As New SqlParameter("@ProductID", SqlDbType.VarChar)
                        paramProductId.Size = 100
                        paramProductId.Value = ddlSeatStyle.SelectedValue

                        Dim paramProdQty As New SqlParameter("@ProductID_Quantity", SqlDbType.Int)
                        paramProdQty.Value = CType(txtSeatStyleQty.Text, Integer)

                        objCmd.Parameters.Add(param2)
                        objCmd.Parameters.Add(param3)
                        objCmd.Parameters.Add(par4)
                        objCmd.Parameters.Add(paramProductId)
                        objCmd.Parameters.Add(paramProdQty)
                    End If

                    'execute sp
                    objCmd.ExecuteNonQuery()
                    'the sp returns its value in the form of an output parameter
                    'get the value from the output parameter

                    strPrintData = handleDBValue(par4.Value)
                End Using
            End Using

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return strPrintData

    End Function

    '******************************************************************************
    'Name:  John Rose
    'Date:  10/9/2007
    'Description:  In this function, the lot number is being parsed.  It is removing the
    'sub lot (: 12) portion of the number.
    'Input parameter:  the full lot number
    'returned value:  Full lot number without sublot attached.
    '*********************************************************************************
    Private Function parseLotNumber(ByVal strFullLotNumber As String) As String
        Dim intColonLoc As Integer
        Dim strParsedLotNum As String
        Dim strResult As String = ""

        Try
            intColonLoc = strFullLotNumber.IndexOf(":")
            strParsedLotNum = strFullLotNumber.Substring(0, intColonLoc)
            strResult = Trim(strParsedLotNum)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return strResult

    End Function

    Private Sub parseLotNumAndSublot(ByVal strFullLotNumber As String, ByRef strLotNumber As String, ByRef strQuantity As String)
        Dim intColonLoc As Integer

        Try
            intColonLoc = strFullLotNumber.IndexOf(":")
            strLotNumber = strFullLotNumber.Substring(0, intColonLoc)
            strQuantity = strFullLotNumber.Substring(intColonLoc + 1)
            strQuantity = Trim(strQuantity)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Public Function handleDBValue(ByVal theinput As System.Object) As String
        'this function gets called when the data is not null
        Dim returnMe As String
        If IsDBNull(theinput) Then
            returnMe = ""
        Else
            returnMe = CType(theinput, String)
        End If
        Return returnMe
    End Function

    Private Sub SendPrintJob()
        Dim strPrintCommand As String
        Dim strLotNumber As String = ""
        Dim strQuantity As String = ""
        Dim sbjs As New StringBuilder

        Try
            'get the selected printer from request object
            If (hidSelectedPrinterPrelim.Value.Length > 0) Then
                'grab the selected printer value
                strPrintCommand = createPrintCommandLine(hidSelectedPrinterPrelim.Value)
                '       strPrintCommand = HttpUtility.UrlEncode(strPrintCommand)

                If strPrintCommand.Length = 0 Then
                    Master.eMsg("The database returned an empty string.  The system cannot complete the print request.")
                End If

                hidPrintString.Value = strPrintCommand
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region


End Class