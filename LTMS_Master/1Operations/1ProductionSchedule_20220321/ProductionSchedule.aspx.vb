Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class ProductionSchedule
    Inherits System.Web.UI.Page

    Public Const NO_TOTAL As String = "?"
    Public Const SHIFT As String = "Shift "
    Public Const ON_HOLD As String = " <b>ON HOLD</b>"
    Public Const ON_HOLD_NO_MARKUP As String = " ON HOLD"

#Region "Enumerations"

    Enum Totals
        Day1Total
        Day2Total
        Day3Total
        Day4Total
        Day5Total
        Day6Total
        Day7Total
    End Enum

    Enum psGetSchedule
        ProductionDate
        DayHeading
        Shift
        ShiftSuffix
        FrameCode
        IC
        Model
        VehicleModel
        BC
        Qty
        LotNumber8
        LotNumber10
        SubLot
        OnHold
        MarkedAsBuilt
        JobsProduced
        SequenceDT
        LotNumber
        SubLotNumber
        vchProdSchedIndex
        SetexSchedule
        ProductionScheduleHighlightColor
        BroadcastPointID
        ImageName
    End Enum

    Enum procGetBroadcastPoints
        BroadcastPointID
        Description
        ImageName
        DefaultDailyBuildQuantityShip
        DefaultDailyBuildQuantityJob
        defaultSelection
    End Enum

#End Region

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                hidLastTab.Value = "0"
                Me.tbDay.Text = Now().ToString("MM/dd/yyyy")
                LoadProdData()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ProductionSchedule_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        Try
            LoadProdData()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibNext_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibNext.Click
        Try
            Dim dt As Date
            Dim interval As Integer = If(hidLastTab.Value = "1", 1, 7)

            If (Date.TryParse(Me.tbDay.Text, dt)) Then
                tbDay.Text = dt.AddDays(interval).ToString("MM/dd/yyyy")
                LoadProdData()
            Else
                Master.Msg = "Invalid Date"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibPrev_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibPrev.Click
        Try
            Dim dt As Date
            Dim interval As Integer = If(hidLastTab.Value = "1", 1, 7)
            interval = interval * -1

            If (Date.TryParse(Me.tbDay.Text, dt)) Then
                tbDay.Text = dt.AddDays(interval).ToString("MM/dd/yyyy")
                LoadProdData()
            Else
                Master.Msg = "Invalid Date"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cbHold_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbHold.CheckedChanged
        Try
            LoadProdData()
            SelectLastSelectedNode()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub rbOrders_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rbOrders.SelectedIndexChanged
        Try
            LoadProdData()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        Try
            DeleteLotOrSubLot()
            ClearHiddenNodeValues()
            LoadProdData()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdRevInc_Click(sender As Object, e As System.EventArgs) Handles cmdRevInc.Click
        Try
            InsertRevision()
            LoadProdData()
            SelectLastSelectedNode()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdResequence_Click(sender As Object, e As System.EventArgs) Handles cmdResequence.Click
        Try
            ResequenceLots()
            LoadProdData()
            SelectLastSelectedNode()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Try
            SaveNew()
            LoadProdData()
            SelectLastSelectedNode()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdEdit_Click(sender As Object, e As System.EventArgs) Handles cmdEdit.Click
        Try
            SaveEdits()
            LoadProdData()
            SelectLastSelectedNode()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdExpandAll_Click(sender As Object, e As System.EventArgs) Handles cmdExpandAll.Click
        treeMon.ExpandAllNodes()
        treeTue.ExpandAllNodes()
        treeWed.ExpandAllNodes()
        treeThu.ExpandAllNodes()
        treeFri.ExpandAllNodes()
        treeSat.ExpandAllNodes()
        treeSun.ExpandAllNodes()
        treeDay.ExpandAllNodes()
    End Sub

    Private Sub cmdCollapseAll_Click(sender As Object, e As System.EventArgs) Handles cmdCollapseAll.Click
        treeMon.CollapseAllNodes()
        treeTue.CollapseAllNodes()
        treeWed.CollapseAllNodes()
        treeThu.CollapseAllNodes()
        treeFri.CollapseAllNodes()
        treeSat.CollapseAllNodes()
        treeSun.CollapseAllNodes()
        treeDay.CollapseAllNodes()
    End Sub

    Protected Sub RadTreeView_ContextMenuItemClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeViewContextMenuEventArgs)
        Try
            Select Case e.MenuItem.Text
                Case "On Hold"
                    If e.Node.Level = 1 Then
                        PlaceSelectedLotsOnorOffHold(psHoldType.OnHold, CType(sender, RadTreeView))
                    End If
                    Exit Select
                Case "Off Hold"
                    If e.Node.Level = 1 Then
                        PlaceSelectedLotsOnorOffHold(psHoldType.OffHold, CType(sender, RadTreeView))
                    End If
                    Exit Select
                Case "Expand All"
                    e.Node.TreeView.ExpandAllNodes()
                    Exit Select
                Case "Collapse All"
                    e.Node.TreeView.CollapseAllNodes()
                    Exit Select
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
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

    Private Sub treeMove_NodeDataBound(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles treeMove.NodeDataBound
        Try
            Dim bSetexOrder As Boolean
            Dim dataSourceRow As DataRowView = CType(e.Node.DataItem, DataRowView)
            'e.Item.Attributes("customAttribute1") = dataSourceRow("CustomAttribute2").ToString()
            e.Node.CssClass = "comboBoxPrePopImage"
            e.Node.ImageUrl = String.Format("../../Images/Misc/{0}", dataSourceRow("ImageName").ToString())
            e.Node.Selected = CBool(dataSourceRow("IsSelected").ToString())
            e.Node.AllowDrop = False
            If e.Node.Level > 0 Then
                'e.Node.AllowDrag = False
            End If

            If (Boolean.TryParse(dataSourceRow("SetexOrder").ToString(), bSetexOrder)) Then
                If bSetexOrder = False Then
                    e.Node.Text = "<span class='allOrders'>" + e.Node.Text + "</span>"
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub treeMove_NodeDrop(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeDragDropEventArgs)
        Try

            ''181217 tdye replaced the following section
            'Dim lastLot As String = e.DraggedNodes(0).Value

            'For Each node As RadTreeNode In e.DraggedNodes
            '    If node.Level = 0 Then
            '        MoveLot(node.Value, e.DropPosition.ToString, e.DestDragNode.Value)
            '    Else
            '        MoveSubLot(node.Value, e.DropPosition.ToString, e.DestDragNode.Value)
            '    End If
            'Next

            Dim lastLot As String = e.DraggedNodes(0).Value
            Dim intNodeIndex As Integer = e.DestDragNode.Index
            Dim intNextNodeIndex As Integer = intNodeIndex + 1
            Dim strDestNode As String
            Dim strDropPosition As String
            Dim strLotNumbers As String = ""
            Dim bDone As Boolean = False

            If String.Equals(e.DropPosition.ToString, "Below", StringComparison.OrdinalIgnoreCase) Then
                strDestNode = e.DestDragNode.TreeView.Nodes(intNextNodeIndex).Value
                strDropPosition = "Above"
            Else
                strDestNode = e.DestDragNode.Value
                strDropPosition = "Above"
            End If


            '20191001 PAH Modified to allow up to 2 dragged lots
            For Each node As RadTreeNode In e.DraggedNodes
                If node.Level = 0 And e.DraggedNodes.Count > 2 Then
                    Master.eMsg("No more than 2 Lots may be selected.")
                    Exit For
                End If
                If node.Level = 0 And (rbOrders.SelectedValue = "1") Then  'this is a setex order
                    MoveLot(node.Value, strDropPosition, strDestNode)
                    bDone = True 'skip the upcoming MoveLot
                ElseIf node.Level = 0 And (rbOrders.SelectedValue <> "1") Then 'all orders
                    strLotNumbers = strLotNumbers & (node.Value.Substring(0, 8) & ",")
                Else
                    MoveSubLot(node.Value, strDropPosition, strDestNode)
                    bDone = True 'skip the upcoming MoveLot
                End If
            Next

            If Not bDone And strLotNumbers <> "" Then
                MoveLot(strLotNumbers, strDropPosition, strDestNode)
                bDone = True
            End If

            LoadProdData()
            hidNodeLot.Value = lastLot
            hidLotNum8.Value = lastLot.Substring(0, 8)
            treeMove.DataBind()
            SelectLastSelectedNode()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ddlMoveBP_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlMoveBP.SelectedIndexChanged
        ClearHiddenNodeValues()
        MoveLots_Rebind()
    End Sub

#End Region

#Region "private methods"

    Private Sub EnableControls()
        'Mark S. 10-17-2005
        'secure buttons.  code that enables and disables certain buttons depending on the operation is now in javascript

        Master.Secure(Me.cmdNew)
        Master.Secure(Me.cmdMove)
        Master.Secure(Me.cmdEdit)
        Master.Secure(Me.cmdDelete)
        Master.Secure(Me.cmdResequence)
        Master.Secure(Me.cmdPrint)

        Master.Secure(Me.cmdRevInc)
        Master.Secure(Me.cmdExport)
        Master.Secure(Me.cmdCollapseAll)
        Master.Secure(Me.cmdExpandAll)


        Master.Secure(Me.cmdRefresh)

        Master.Secure(Me.ibNext)
        Master.Secure(Me.ibPrev)

        'initially disable all buttons
        'Me.cmdNew.Enabled = False
        'Me.cmdMove.Enabled = False
        'Me.cmdEdit.Enabled = False
        'Me.cmdDelete.Enabled = False
    End Sub

    Private Sub ClearHiddenNodeValues()
        hidNodeSeqDT.Value = ""
        hidNodeLot.Value = ""
        hidNodeLevel.Value = ""

        hidNewProdID.Value = ""
        hidNewLotNew.Value = ""
        hidNewNValues.Value = ""

        hidTreeID.Value = ""
        hidEditProdID.Value = ""

        'hidNodeText.Value = ""
        'hidNodeID.Value = ""
        'hidNodeSubLot.Value = ""
    End Sub

    Private Sub ClearDayTreesTotals()
        Me.treeDay.Nodes.Clear()
        Me.lblTotalSingleDay.Text = NO_TOTAL
    End Sub

    Private Sub ClearWeekTreesTotals()
        Me.treeMon.Nodes.Clear()
        Me.treeTue.Nodes.Clear()
        Me.treeWed.Nodes.Clear()
        Me.treeThu.Nodes.Clear()
        Me.treeFri.Nodes.Clear()
        Me.treeSat.Nodes.Clear()
        Me.treeSun.Nodes.Clear()

        Me.lblTotalMon.Text = NO_TOTAL
        Me.lblTotalTue.Text = NO_TOTAL
        Me.lblTotalWed.Text = NO_TOTAL
        Me.lblTotalThu.Text = NO_TOTAL
        Me.lblTotalFri.Text = NO_TOTAL
        Me.lblTotalSat.Text = NO_TOTAL
        Me.lblTotalSun.Text = NO_TOTAL

        Me.lblTotal.Text = NO_TOTAL
    End Sub

    Private Function LoadProdData() As Boolean
        Dim bResult As Boolean = False
        Try
            ClearHiddenNodeValues()

            If (hidLastTab.Value = "1") Then
                lblWeekTotalCaption.Visible = False
                lblTotal.Visible = False

                bResult = LoadDay()
            Else
                lblWeekTotalCaption.Visible = True
                lblTotal.Visible = True

                bResult = LoadWeek()
            End If
            GetBroadcastPointKey()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return bResult
    End Function

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

    Private Function LoadDay() As Boolean
        Dim bResult As Boolean = False
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim ds As DataSet = Nothing
        Dim dt As Date

        Try
            If (Date.TryParse(Me.tbDay.Text, dt) = False) Then
                Master.Msg = "Error: Please enter a Valid Date."
                bResult = False
            Else
                lblSingleDayOfWeek.Text = dt.ToString("dddd, MM/dd/yyyy")

                oSqlParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
                oSqlParameter.Value = dt.ToString("MM/dd/yyyy")
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
                oSqlParameter.Value = If(cbHold.Checked, 0, 1)
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@NumDays", SqlDbType.Int)
                oSqlParameter.Value = 1
                colParameters.Add(oSqlParameter)

                ' SETEX Orders were selected to be returned
                If (rbOrders.SelectedValue = "1") Then
                    ds = DA.GetDataSet("procPSGetProdSched", colParameters)
                Else
                    ds = DA.GetDataSet("procPSGetProdSchedHonda", colParameters)
                End If

                ''error checking
                If (DA.IsDataSetNotEmpty(ds)) AndAlso (ds.Tables.Count >= 2) Then
                    If (ds.Tables(1) IsNot Nothing) AndAlso (ds.Tables(1).DefaultView.Table.Rows(0)(0) IsNot Nothing) Then
                        lblTotalSingleDay.Text = ds.Tables(1).DefaultView.Table.Rows(0)(0).ToString()
                    End If
                    If (PopulateTreeView(treeDay, ds.Tables(0))) Then
                        Utility.TreeExpand(treeDay, 1)
                    End If
                End If

                LoadRevision(Utility.GetMondayOfDate(Me.tbDay.Text).ToString("MM/dd/yyyy"))

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function LoadWeek() As Boolean
        Dim bResult As Boolean = True
        Dim ds As DataSet = Nothing

        Dim dMonday As Date = Utility.GetMondayOfDate(Me.tbDay.Text)
        Dim strMonday As String = dMonday.ToShortDateString()

        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        Try

            If (Me.tbDay.Text.Length > 0) Then

                LoadDayLabelsWeek()

                oSqlParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
                oSqlParameter.Value = strMonday
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
                oSqlParameter.Value = If(cbHold.Checked, 0, 1)
                colParameters.Add(oSqlParameter)


                ' SETEX Orders were selected to be returned
                If (rbOrders.SelectedValue = "1") Then
                    ds = DA.GetDataSet("procPSGetProdSched", colParameters)
                Else
                    ds = DA.GetDataSet("procPSGetProdSchedHonda", colParameters)
                End If

                If (ds IsNot Nothing) AndAlso (ds.Tables.Count >= 7) Then

                    bResult = bResult And PopulateTreeView(Me.treeMon, ds.Tables(0))
                    bResult = bResult And PopulateTreeView(Me.treeTue, ds.Tables(1))
                    bResult = bResult And PopulateTreeView(Me.treeWed, ds.Tables(2))
                    bResult = bResult And PopulateTreeView(Me.treeThu, ds.Tables(3))
                    bResult = bResult And PopulateTreeView(Me.treeFri, ds.Tables(4))
                    bResult = bResult And PopulateTreeView(Me.treeSat, ds.Tables(5))
                    bResult = bResult And PopulateTreeView(Me.treeSun, ds.Tables(6))

                    bResult = bResult And LoadWeekTotals(ds.Tables(7))

                    Utility.TreeExpand(Me.treeMon, 1)
                    Utility.TreeExpand(Me.treeTue, 1)
                    Utility.TreeExpand(Me.treeWed, 1)
                    Utility.TreeExpand(Me.treeThu, 1)
                    Utility.TreeExpand(Me.treeFri, 1)
                    Utility.TreeExpand(Me.treeSat, 1)
                    Utility.TreeExpand(Me.treeSun, 1)

                End If

                LoadRevision(dMonday.ToString("MM/dd/yyyy"))

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function PopulateTreeView(ByRef tree As RadTreeView, ByRef dt As DataTable) As Boolean
        Dim shiftNode As New RadTreeNode
        Dim lotNode As New RadTreeNode
        Dim subLotNode As New RadTreeNode

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

        Dim qtyLot As Integer
        Dim i As Integer

        Try
            'clear out tree control
            tree.Nodes.Clear()

            lotNode.CssClass = "treeViewNode"
            lotNode.SelectedCssClass = "treeViewNodeSelected"
            subLotNode.CssClass = "treeViewNode"
            subLotNode.SelectedCssClass = "treeViewNodeSelected"


            If (dt IsNot Nothing) Then
                If (dt.Rows.Count > 0) Then

                    'add all nodes
                    For i = 0 To dt.Rows.Count - 1

                        strBC = dt.Rows(i)(psGetSchedule.BC).ToString()
                        strOnHold = dt.Rows(i)(psGetSchedule.OnHold).ToString()
                        strSeqDT = dt.Rows(i)(psGetSchedule.SequenceDT).ToString()
                        strQty = dt.Rows(i)(psGetSchedule.Qty).ToString()

                        Boolean.TryParse(dt.Rows(i)(psGetSchedule.SetexSchedule).ToString(), SetexFlag)


                        If (strQty = "") Then
                            strQty = "0"
                        End If

                        'add new shift node if needed
                        If (strShift <> dt.Rows(i)(psGetSchedule.Shift).ToString()) Then
                            strShift = dt.Rows(i)(psGetSchedule.Shift).ToString()
                            shiftNode = Nothing
                            shiftNode = New RadTreeNode()
                            shiftNode.Text = SHIFT + strShift + dt.Rows(i)(psGetSchedule.ShiftSuffix).ToString()
                            shiftNode.Value = strSeqDT
                            tree.Nodes.Add(shiftNode)
                            shiftChange = True
                        End If

                        'add new lot if needed
                        If ((strLot <> dt.Rows(i)(psGetSchedule.LotNumber8).ToString()) Or (shiftChange)) Then
                            'add quantity to previous lot and on hold text if needed
                            lotNode.Text += (" : " + qtyLot.ToString())
                            If (holdFlag) Then
                                lotNode.Text += ON_HOLD
                            End If

                            'MS 06-06-2006 check if the ship code is "" then we know this is a customer order
                            'and therefore we want the background to be gray
                            If (rbOrders.SelectedValue = "0") Then
                                If (SetexFlag = False) Then
                                    lotNode.Text = "<span class='allOrders'>" + lotNode.Text + "</span>"
                                End If
                            End If

                            qtyLot = 0
                            holdFlag = False

                            strLot = dt.Rows(i)(psGetSchedule.LotNumber8).ToString()
                            lotNode = Nothing
                            lotNode = New RadTreeNode

                            If (rbOrders.SelectedValue = "0") Then
                                SetexSchedule = CBool(dt.Rows(i)(psGetSchedule.SetexSchedule))
                            End If

                            lotNode.Text = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3)
                            lotNode.Value = strSeqDT
                            lotNode.ImageUrl = String.Format("../../Images/Misc/{0}", dt.Rows(i)(psGetSchedule.ImageName))

                            If (strOnHold.ToUpper = "TRUE") Then
                                'lotNode.Text = lotNode.Text + ON_HOLD
                                holdFlag = True
                            End If

                            shiftNode.Nodes.Add(lotNode)
                        End If
                        'reset shift change flag
                        shiftChange = False

                        'add sublot
                        strSubLot = dt.Rows(i)(psGetSchedule.SubLot).ToString()
                        subLotNode = Nothing
                        subLotNode = New RadTreeNode
                        subLotNode.Text = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3) + "-" + strSubLot + " : " + strQty + " " + strBC
                        subLotNode.Value = strSeqDT
                        subLotNode.ImageUrl = String.Format("../../Images/Misc/{0}", dt.Rows(i)(psGetSchedule.ImageName))

                        If (rbOrders.SelectedValue = "0") Then
                            If (SetexFlag = False) Then
                                subLotNode.Text = "<span class='allOrders'>" + subLotNode.Text + "</span>"
                            End If
                        End If
                        lotNode.Nodes.Add(subLotNode)

                        qtyLot += Convert.ToInt32(strQty)

                    Next

                    'add total to the very last lot
                    lotNode.Text += (" : " + qtyLot.ToString())
                    If (rbOrders.SelectedValue = "0") Then
                        If (SetexFlag = False) Then
                            lotNode.Text = "<span class='allOrders'>" + lotNode.Text + "</span>"
                        End If
                    End If


                    bResult = True

                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Sub LoadDayLabelsWeek()
        Try
            Dim dt As Date = Utility.GetMondayOfDate(tbDay.Text())

            lblDateMon.Text = dt.ToString("dddd, MM/dd/yyyy")
            dt = dt.AddDays(1)
            lblDateTue.Text = dt.ToString("dddd, MM/dd/yyyy")
            dt = dt.AddDays(1)
            lblDateWed.Text = dt.ToString("dddd, MM/dd/yyyy")
            dt = dt.AddDays(1)
            lblDateThu.Text = dt.ToString("dddd, MM/dd/yyyy")
            dt = dt.AddDays(1)
            lblDateFri.Text = dt.ToString("dddd, MM/dd/yyyy")
            dt = dt.AddDays(1)
            lblDateSat.Text = dt.ToString("dddd, MM/dd/yyyy")
            dt = dt.AddDays(1)
            lblDateSun.Text = dt.ToString("dddd, MM/dd/yyyy")

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function LoadWeekTotals(ByRef dt As DataTable) As Boolean
        Dim bResult As Boolean = False

        Try
            If (dt IsNot Nothing) Then
                If (dt.Rows.Count > 0) Then
                    If (dt.Columns.Count >= 7) Then

                        Dim grandTotal As Integer = 0
                        Dim dayTotal As Integer = 0

                        dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.Day1Total).ToString())
                        grandTotal = grandTotal + dayTotal
                        Me.lblTotalMon.Text = dayTotal.ToString()

                        dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.Day2Total).ToString())
                        grandTotal = grandTotal + dayTotal
                        Me.lblTotalTue.Text = dayTotal.ToString()

                        dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.Day3Total).ToString())
                        grandTotal = grandTotal + dayTotal
                        Me.lblTotalWed.Text = dayTotal.ToString()

                        dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.Day4Total).ToString())
                        grandTotal = grandTotal + dayTotal
                        Me.lblTotalThu.Text = dayTotal.ToString()

                        dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.Day5Total).ToString())
                        grandTotal = grandTotal + dayTotal
                        Me.lblTotalFri.Text = dayTotal.ToString()

                        dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.Day6Total).ToString())
                        grandTotal = grandTotal + dayTotal
                        Me.lblTotalSat.Text = dayTotal.ToString()

                        dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.Day7Total).ToString())
                        grandTotal = grandTotal + dayTotal
                        Me.lblTotalSun.Text = dayTotal.ToString()

                        Me.lblTotal.Text = grandTotal.ToString()
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Sub SelectLastSelectedNode()
        Try
            If rbOrders.SelectedValue = "1" Then 'setex orders
                SelectLastSelectedNodeInTree(treeMon)
                SelectLastSelectedNodeInTree(treeTue)
                SelectLastSelectedNodeInTree(treeWed)
                SelectLastSelectedNodeInTree(treeThu)
                SelectLastSelectedNodeInTree(treeFri)
                SelectLastSelectedNodeInTree(treeSat)
                SelectLastSelectedNodeInTree(treeSun)
            Else
                SelectLastSelectedNodeInTree(treeDay)
            End If
            SelectLastSelectedNodeInTree(treeMove)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SelectLastSelectedNodeInTree(ByRef tree As RadTreeView)
        Try
            Dim node As RadTreeNode
            node = tree.Nodes.FindNodeByText(hidNodeLot.Value, True)
            If node IsNot Nothing Then
                node.Selected = True
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub PlaceSelectedLotsOnorOffHold(psHoldType As psHoldType, sender As RadTreeView)
        Try
            For Each node As RadTreeNode In sender.SelectedNodes
                PlaceLotOnOrOffHold(psHoldType, node.Text)
            Next
            LoadProdData()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub PlaceLotOnOrOffHold(typeOfHold As psHoldType, LotNumber As String)
        Dim LotNumber_Short As String = ""
        Dim message As String = ""
        Dim changeCopy As String = ""
        Try
            LotNumber_Short = LotNumber.Replace("-", "").Substring(0, 8)

            If (typeOfHold = psHoldType.OnHold) Then
                changeCopy = "ON"
            Else
                changeCopy = "OFF"
            End If

            If (ChangeHoldStatus(LotNumber_Short, changeCopy, message)) Then
                Master.tMsg(changeCopy + " HOLD", "Lot Number " + LotNumber_Short + " status changed to " + changeCopy + " HOLD")
            Else
                Master.tMsg(changeCopy + " HOLD", "Error - Lot Number: " + LotNumber_Short + " has NOT been placed " + changeCopy + " HOLD. <BR>Error message: " + message)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function ChangeHoldStatus(ByVal lotNum As String, ByVal OnOrOff As String, ByRef message As String) As Boolean
        Dim bResult As Boolean = False
        Dim status As String = ""
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Try
            oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 80)
            oSqlParameter.Value = lotNum
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@OnOrOff", SqlDbType.VarChar, 80)
            oSqlParameter.Value = OnOrOff
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 5)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procPSChangeHoldStatus", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status.ToUpper() = "TRUE") Then
                bResult = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult
    End Function

    Private Sub ResequenceLots()
        Dim status As String = ""
        Dim LotNumber As String
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Try 'resequence lots so they are flipped, can flip starting with lot or sublot
            'remove dashes
            If (hidNodeLot.Value.Length = 0) Then
                Master.Msg = "Please Select a Lot Number to Resequence."
            Else
                LotNumber = hidNodeLot.Value.Trim().Replace("-", "").Substring(0, 8)

                oSqlParameter = New SqlParameter("@StartLotNumber", SqlDbType.VarChar, 30)
                oSqlParameter.Value = LotNumber
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                colParameters.Add(oSqlParameter)

                'return data to populate the tree from stored procedure
                colOutput = DA.ExecSP("procPSResequenceLots", colParameters)

                For Each oParameter In colOutput
                    With oParameter
                        If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                            status = oParameter.Value.ToString()
                        End If
                    End With
                Next

                If status = "Success!" Then
                    Master.tMsg("cmdResequence", "Resequencing of Lots Starting with Lot Number: " & LotNumber & " is Complete.")
                Else
                    Master.tMsg("cmdResequence", "Resequencing of Lots with Lot Number: " & LotNumber & " Failed!. " & status)
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SaveNew()

        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Dim beforeAfter As String
        Dim status As String = ""
        Dim message As String = ""

        Try
            If (ValidateNewInput()) Then

                If (Me.cbNewNextPos.Checked) Then
                    beforeAfter = ""
                Else
                    beforeAfter = Me.rblNewLocation.SelectedValue
                End If

                oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 8)
                oSqlParameter.Value = Me.txtNewLotNum.Text.Replace("-", "")
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@BeforeAfter", SqlDbType.VarChar, 6)
                oSqlParameter.Value = beforeAfter
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@AdjacentLot", SqlDbType.VarChar, 8)
                oSqlParameter.Value = Me.hidNewLotNew.Value.Replace("-", "")
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@SubLotIndex", SqlDbType.VarChar, 2)
                oSqlParameter.Value = Me.txtNewSLIndex.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@JobQuantity", SqlDbType.VarChar, 80)
                oSqlParameter.Value = Me.txtNewQuantity.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ProductID", SqlDbType.VarChar, 80)
                oSqlParameter.Value = hidNewProdID.Value
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@RecordedBy", SqlDbType.VarChar, 80)
                'oSqlParameter.Value = Session("UserFirstLastName")
                oSqlParameter.Value = "ProductionSchedule: " & Session("UserFirstLastName").ToString()
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ProductionNotes", SqlDbType.VarChar, 500)
                oSqlParameter.Value = ""
                colParameters.Add(oSqlParameter)

                'MS 9-20-2005 allow for N-value parameter which was added for KD processing
                oSqlParameter = New SqlParameter("@PSIndex", SqlDbType.Decimal, 80)
                oSqlParameter.Value = DBNull.Value
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@SSIndex", SqlDbType.Decimal, 80)
                oSqlParameter.Value = DBNull.Value
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Nvalue", SqlDbType.VarChar, 80)
                oSqlParameter.Value = hidNewNValues.Value
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
                oSqlParameter.Value = Me.txtNewLotNum.Text.Substring(0, 1)
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                colParameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("procPSNewSubLot", colParameters)

                For Each oParameter In colOutput
                    With oParameter
                        If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                            status = oParameter.Value.ToString()
                        ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                            message = oParameter.Value.ToString()
                        End If
                    End With
                Next

                LogTrc("New Lot", Page, "ProductionSchedule.  @LotNumber :: " + Me.txtNewLotNum.Text.Replace("-", "") + " @BeforeAfter :: " + beforeAfter + " @AdjacentLot :: " + Me.hidNewLotNew.Value.Replace("-", "") +
                       " @SubLotIndex :: " + Me.txtNewSLIndex.Text + " @JobQuantity :: " + Me.txtNewQuantity.Text + " @ProductID :: " + hidNewProdID.Value + " @RecordedBy :: " + "ProductionSchedule: " & Session("UserFirstLastName").ToString() +
                      " @Nvalue :: " + hidNewNValues.Value + " @BroadcastPointID :: " + Me.txtNewLotNum.Text.Substring(0, 1) + " @Status :: " + status + " @ErrorMsg :: " + message)

                If (status.ToUpper() = "TRUE") Then
                    Master.tMsg("New", "New Sub-Lot for Lot Number: " + txtNewLotNum.Text + " has been created.")
                Else
                    Master.tMsg("New", "Error - Lot Number: " + txtNewLotNum.Text + "has NOT been created. <BR>Error message: " + message + "<BR>Status message: " + status)
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function ValidateNewInput() As Boolean
        Dim bResult As Boolean = False
        Try
            ''lot number: 0-1234-567
            If (Me.txtNewLotNum.Text.Replace("-", "").Length <> 8) Then
                Master.Msg = "Error - Invalid Lot Number.<BR>Valid Lot Number format: 0-1234-567"
            ElseIf (Me.txtNewSLIndex.Text.Length <= 0) Then
                Master.Msg = "Error - Invalid Sub Lot Number.<BR>Sub Lot Number Must Be > or = to 0"
            ElseIf (Convert.ToInt32(Me.txtNewSLIndex.Text) < 0) Then
                Master.Msg = "Error - Invalid Sub Lot Number.<BR>Sub Lot Number Must Be > or = to 0"
            ElseIf (Me.txtNewSLIndex.Text.Length > 2) Then
                Master.Msg = "Error - Invalid Sub Lot Number.<BR>Sub Lot Number Must Be 2 digits or less."
            ElseIf (Me.txtNewQuantity.Text.Length <= 0) Then
                Master.Msg = "Error - Invalid Job Quantity.<BR>Job Quantity Must Be > or = to 0"
            ElseIf (Convert.ToInt32(Me.txtNewQuantity.Text) < 0) Then
                Master.Msg = "Error - Invalid Job Quantity.<BR>Job Quantity Must Be > or = to 0"
            ElseIf (hidNewNValues.Value.Length = 0) Then
                Master.Msg = "Error - N-Value is not allowed to be blank."
            Else
                bResult = True
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return bResult
    End Function

    Private Sub DeleteLotOrSubLot()
        Try
            Dim selectedNodeLevel As Integer
            Dim lot As String = ""
            Dim status As String = ""
            Dim message As String = ""

            If (Integer.TryParse(hidNodeLevel.Value, selectedNodeLevel) = False) Then
                Master.eMsg("Error: Lot/Sub-Lot cannot been deleted. Please make a valid selection.")
            Else
                lot = hidNodeLot.Value.ToString().Trim().Substring(0, 10)

                If (selectedNodeLevel = 1) Then

                    DeleteLot(lot, status, message)

                    If (status.ToUpper() = "TRUE") Then
                        Master.tMsg("Delete", "Deleted Lot Number: " + lot)
                    Else
                        Master.tMsg("Delete", "Error: Lot Number: " + lot + " was not deleted. <br>Error message: " + message + "<BR>Status message: " + status)
                    End If

                ElseIf (selectedNodeLevel = psNodeType.SUBLOT) Then

                    DeleteSubLot(hidNodeSeqDT.Value, status, message)

                    If (status.ToUpper() = "TRUE") Then
                        Master.tMsg("Delete", "Deleted Sub-Lot Number: " + hidNodeLot.Value + "; Sequence DT: " + hidNodeSeqDT.Value.Substring(0, 13))
                    Else
                        Master.tMsg("Delete", "Error: Sub-Lot Number: " + lot + " was not deleted. <br>Error message: " + message + "<BR>Status message: " + status)
                    End If
                Else        'this is a Shift, we do not delete at this level (selectedNodeLevel = psNodeType.SHIFT ) 
                    Master.tMsg("Delete", "Error: Lot/Sub-Lot has not been deleted. Please make a valid selection.")
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub DeleteLot(ByVal lotNum As String, ByRef status As String, ByRef message As String)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 80)
        oSqlParameter.Value = lotNum.Replace("-", "")
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
        oSqlParameter.Value = lotNum.Substring(0, 1)
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 5)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        colOutput = DA.ExecSP("procPSDeleteLot", colParameters)


        For Each oParameter In colOutput
            With oParameter
                If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                    status = oParameter.Value.ToString()
                ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                    message = oParameter.Value.ToString()
                End If
            End With
        Next


        If (status Is Nothing) Then
            status = ""
        End If
        If (message Is Nothing) Then
            message = ""
        End If

    End Sub

    Private Sub DeleteSubLot(ByVal seqDT As String, ByRef status As String, ByRef message As String)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        oSqlParameter = New SqlParameter("@SequenceDT", SqlDbType.DateTime)
        oSqlParameter.Value = seqDT
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
        oSqlParameter.Value = hidNodeLot.Value.Substring(0, 1)
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 5)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        colOutput = DA.ExecSP("procPSDeleteSubLot", colParameters)

        For Each oParameter In colOutput
            With oParameter
                If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                    status = oParameter.Value.ToString()
                ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                    message = oParameter.Value.ToString()
                End If
            End With
        Next


        If (status Is Nothing) Then
            status = ""
        End If
        If (message Is Nothing) Then
            message = ""
        End If

    End Sub

    Private Sub SaveEdits()
        Try
            Dim status As String = ""
            Dim message As String = ""
            Dim seqDT As String = Me.hidNodeSeqDT.Value

            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As New List(Of SqlParameter)


            oSqlParameter = New SqlParameter("@SequenceDT", SqlDbType.DateTime)
            oSqlParameter.Value = seqDT
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@SubLotIndex", SqlDbType.VarChar, 2)
            oSqlParameter.Value = Me.txtEditSLIndex.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@JobQuantity", SqlDbType.VarChar, 80)
            oSqlParameter.Value = Me.txtEditQuantity.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ProductID", SqlDbType.VarChar, 80)
            oSqlParameter.Value = hidEditProdID.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ProductionNotes", SqlDbType.VarChar, 500)
            oSqlParameter.Value = Me.txtEditNotes.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = hidNodeLot.Value.Substring(0, 1)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procPSUpdateSubLot", colParameters)
            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status.ToUpper() = "TRUE") Then
                Master.tMsg("Edit", "Sub-Lot with Sequence DT of: " + seqDT + " has been updated.")
            Else
                Master.tMsg("Edit", "Error - Sub-Lot with Sequence DT of: " + seqDT + " has NOT been edited. <BR>Error message: " + message + "<BR>Status message: " + status)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Function MoveLot(srcLotNum As String, beforeAfter As String, destLotNum As String) As String
        Dim retVal As String = ""
        Dim status As String = ""
        Dim message As String = ""
        Dim lotNum As String = ""
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim strBroadcastPoint As String
        Dim strMovedInShipping As String = ""
        Dim strProcToBeRun As String = ""
        Dim strBeforeAfter As String = ""
        Dim strAdjacentLot As String = ""

        Try
            strAdjacentLot = destLotNum.Replace("-", "").Substring(0, 8)

            If String.Equals(beforeAfter, "Above", StringComparison.OrdinalIgnoreCase) Then
                strBeforeAfter = "BEFORE"
            ElseIf String.Equals(beforeAfter, "Below", StringComparison.OrdinalIgnoreCase) Then
                strBeforeAfter = "AFTER"
            End If

            If (rbOrders.SelectedValue = "0") Then
                lotNum = srcLotNum.Trim().Replace("-", "")
            Else
                lotNum = srcLotNum.Trim().Replace("-", "").Substring(0, 8)
            End If
            strBroadcastPoint = lotNum.Substring(0, 1)

            oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 80)
            oSqlParameter.Value = lotNum
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BeforeAfter", SqlDbType.VarChar, 6)
            oSqlParameter.Value = strBeforeAfter
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@AdjacentLot", SqlDbType.VarChar, 80)
            oSqlParameter.Value = strAdjacentLot
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ProdOrShipType", SqlDbType.VarChar, 80)
            oSqlParameter.Value = "PROD"
            colParameters.Add(oSqlParameter)

            If (rbOrders.SelectedValue = "1") Then
                'add parameter for whether to move in shipping index or not
                'oSqlParameter = New SqlParameter("@MoveShipIndex", SqlDbType.Bit)
                'oSqlParameter.Value = "True"
                'colParameters.Add(oSqlParameter)
                'strMovedInShipping = " Also Moved in Shipping Schedule"
            End If

            'If Me.cbMoveShipIndex.Checked Then

            'End If

            oSqlParameter = New SqlParameter("@CurrentBroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = strBroadcastPoint
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@NewBroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = strBroadcastPoint
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ProdSchedIndexIncrement", 0)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)


            If (rbOrders.SelectedValue = "1") Then '  this is a setex order
                'call the procedure to move in the production schedule and optionally shipping schedule
                strProcToBeRun = "Setex Only orders, procPSMoveLotProdSched::"  'run twice in case of paired moves
                colOutput = DA.ExecSP("procPSMoveLotProdSched", colParameters)
            Else
                'move the lot in the Honda Schedule, in other words move all 3 indexes, prod, honda, ship.
                strProcToBeRun = "ALL Orders,  procPSMoveLotMulti::"
                colOutput = DA.ExecSP("procPSMoveLotMulti", colParameters)
            End If

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status.ToUpper() = "TRUE") Then
                Master.tMsg("Move", strProcToBeRun + " Lot Number: " + lotNum + " has been moved " + beforeAfter + " " + strAdjacentLot + strMovedInShipping)
            Else
                Master.tMsg("Move", "Error - Lot Number: " + lotNum + " has NOT been moved. <BR>Error message: " + message + "<BR>Status message: " + status)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return retVal

    End Function
    Private Sub MoveSubLot(srcLotNum As String, beforeAfter As String, destLotNum As String)
        Try
            Dim status As String = ""
            Dim message As String = ""
            Dim seqDT As String = Me.hidNodeSeqDT.Value
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            Dim dt As DateTime
            Dim strBeforeAfter As String = ""
            Dim lotNum As String = ""
            Dim strBroadcastPoint As String
            lotNum = srcLotNum.Trim().Replace("-", "").Substring(0, 10)
            strBroadcastPoint = lotNum.Substring(0, 1)

            If String.Equals(beforeAfter, "Above", StringComparison.OrdinalIgnoreCase) Then
                strBeforeAfter = "BEFORE"
            ElseIf String.Equals(beforeAfter, "Below", StringComparison.OrdinalIgnoreCase) Then
                strBeforeAfter = "AFTER"
            End If
            'oSqlParameter = New SqlParameter("@SequenceDT", SqlDbType.DateTime)
            'oSqlParameter.Value = dt
            'colParameters.Add(oSqlParameter)
            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = strBroadcastPoint
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 80)
            oSqlParameter.Value = lotNum
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BeforeAfter", SqlDbType.VarChar, 6)
            oSqlParameter.Value = strBeforeAfter
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@AdjacentLot", SqlDbType.VarChar, 12)
            oSqlParameter.Value = destLotNum.Replace("-", "").Substring(0, 10)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            If (rbOrders.SelectedValue = "1") Then
                'call the procedure to move in the production schedule and optionally shipping schedule
                colOutput = DA.ExecSP("procPSMoveSubLotProdSched", colParameters)
            Else
                'move the lot in the Honda Schedule, in other words move indexes, prod, honda
                colOutput = DA.ExecSP("procPSMoveSubLot", colParameters)
            End If

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status.ToUpper() = "TRUE") Then
                Master.tMsg("Move", " Lot Number: " + lotNum + " has been moved " + beforeAfter + " " + destLotNum.Replace("-", "").Substring(0, 10))
            Else
                Master.tMsg("Move", "Error - Lot Number: " + lotNum + " has NOT been moved. <BR>Error message: " + message + "<BR>Status message: " + status)
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub UnCheckAllNodes()
        Try
            treeMon.UncheckAllNodes()
            treeTue.UncheckAllNodes()
            treeWed.UncheckAllNodes()
            treeThu.UncheckAllNodes()
            treeFri.UncheckAllNodes()
            treeSat.UncheckAllNodes()
            treeSun.UncheckAllNodes()
            treeDay.UncheckAllNodes()
            treeMove.UncheckAllNodes()
            ClearHiddenNodeValues()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub MoveLots_Rebind()
        Try
            UnCheckAllNodes()
            hidBroadcastPointID.Value = ddlMoveBP.SelectedValue
            treeMove.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region

#Region "Revision information"

    Private Function LoadRevision(trackID As String) As Boolean
        Dim bResult As Boolean = False
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim strRevisionType As String
        Dim AllOrdersFlag As Boolean = False

        Try
            Me.lblDate.Text = trackID
            Me.lblRevNo.Text = "?"
            Me.lblRevComments.Text = ""

            'check whether Setex Orders or All Orders Selected and assign Revision Type accordingly
            If (rbOrders.SelectedValue = "1") Then
                strRevisionType = "0001" 'for Setex Orders
            Else
                strRevisionType = "0004" 'for all orders

            End If

            oSqlParameter = New SqlParameter("@TrackingID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = trackID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@RevisionTypeID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = strRevisionType
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

            'check whether Setex Orders or All Orders Selected and assign Revision Type accordingly
            If (rbOrders.SelectedValue = "1") Then
                strRevisionType = "0001" 'for Setex Orders
            Else
                strRevisionType = "0004" 'for all orders

            End If
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
            oSqlParameter.Value = Me.lblDate.Text
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
                Master.tMsg("Revision", "Error: unable to update Revision for week starting on " + lblDateMon.Text + ".<br> S.P. Status: " + status + ".<br>S.P. Message: " + message)
            Else
                Master.tMsg("Revision", "Revision for week starting on " + lblDateMon.Text + " has been updated.")
            End If

            LoadRevision(Me.lblDate.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

#End Region

    Private Sub cmdOnHold_Click(sender As Object, e As System.EventArgs) Handles cmdOnHold.Click
        Dim lot As String = ""
        lot = hidNodeLot.Value.ToString().Trim().Substring(0, 10)
        PlaceLotOnOrOffHold(psHoldType.OnHold, lot)
        LoadProdData()
    End Sub

    Private Sub cmdOffHold_Click(sender As Object, e As System.EventArgs) Handles cmdOffHold.Click
        Dim lot As String = ""
        lot = hidNodeLot.Value.ToString().Trim().Substring(0, 10)
        PlaceLotOnOrOffHold(psHoldType.OffHold, lot)
        LoadProdData()
    End Sub


End Class