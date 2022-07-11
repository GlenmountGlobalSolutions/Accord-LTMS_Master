Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class ProductionSchedule30
    Inherits System.Web.UI.Page
    Public Const NO_TOTAL As String = "?"
    Public Const SHIFT As String = "Shift "
    Public Const ON_HOLD As String = " <b>*</b>"
    Public Const ON_HOLD_NO_MARKUP As String = " ON HOLD"

#Region "Enumerations"

    Enum Totals
        W1Day1Total
        W1Day2Total
        W1Day3Total
        W1Day4Total
        W1Day5Total
        W1Day6Total
        W1Day7Total
        W2Day1Total
        W2Day2Total
        W2Day3Total
        W2Day4Total
        W2Day5Total
        W2Day6Total
        W2Day7Total
        W3Day1Total
        W3Day2Total
        W3Day3Total
        W3Day4Total
        W3Day5Total
        W3Day6Total
        W3Day7Total
        W4Day1Total
        W4Day2Total
        W4Day3Total
        W4Day4Total
        W4Day5Total
        W4Day6Total
        W4Day7Total
        W5Day1Total
        W5Day2Total
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
        ProductionSchedule30HighlightColor
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
                hidLastTabH.Value = "0"
                Me.tbDay.Text = Now().ToString("MM/dd/yyyy")
                LoadProdData()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ProductionSchedule30_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
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
            'interval = interval * -1
            interval *= -1

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
            Me.rblWhichWeek.ClearSelection()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdResequence_Click(sender As Object, e As System.EventArgs) Handles cmdResequence.Click
        Try
            ResequenceLots()
            LoadProdData()
            SelectLastSelectedNode()
            RefreshScreen()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdMoveDlg_Click(sender As Object, e As System.EventArgs) Handles cmdMoveDlg.Click
        Try
            MoveLot()
            RefreshScreen()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Try
            SaveNew()
            LoadProdData()
            SelectLastSelectedNode()
            RefreshScreen()
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
        treeW1Mon.ExpandAllNodes()
        treeW1Tue.ExpandAllNodes()
        treeW1Wed.ExpandAllNodes()
        treeW1Thu.ExpandAllNodes()
        treeW1Fri.ExpandAllNodes()
        treeW1Sat.ExpandAllNodes()
        treeW1Sun.ExpandAllNodes()
        treeW2Mon.ExpandAllNodes()
        treeW2Tue.ExpandAllNodes()
        treeW2Wed.ExpandAllNodes()
        treeW2Thu.ExpandAllNodes()
        treeW2Fri.ExpandAllNodes()
        treeW2Sat.ExpandAllNodes()
        treeW2Sun.ExpandAllNodes()
        treeW3Mon.ExpandAllNodes()
        treeW3Tue.ExpandAllNodes()
        treeW3Wed.ExpandAllNodes()
        treeW3Thu.ExpandAllNodes()
        treeW3Fri.ExpandAllNodes()
        treeW3Sat.ExpandAllNodes()
        treeW3Sun.ExpandAllNodes()
        treeW4Mon.ExpandAllNodes()
        treeW4Tue.ExpandAllNodes()
        treeW4Wed.ExpandAllNodes()
        treeW4Thu.ExpandAllNodes()
        treeW4Fri.ExpandAllNodes()
        treeW4Sat.ExpandAllNodes()
        treeW4Sun.ExpandAllNodes()
        treeW5Mon.ExpandAllNodes()
        treeW5Tue.ExpandAllNodes()
        treeHondaW1Mon.ExpandAllNodes()
        treeHondaW1Tue.ExpandAllNodes()
        treeHondaW1Wed.ExpandAllNodes()
        treeHondaW1Thu.ExpandAllNodes()
        treeHondaW1Fri.ExpandAllNodes()
        treeHondaW1Sat.ExpandAllNodes()
        treeHondaW1Sun.ExpandAllNodes()
        treeHondaW2Mon.ExpandAllNodes()
        treeHondaW2Tue.ExpandAllNodes()
        treeHondaW2Wed.ExpandAllNodes()
        treeHondaW2Thu.ExpandAllNodes()
        treeHondaW2Fri.ExpandAllNodes()
        treeHondaW2Sat.ExpandAllNodes()
        treeHondaW2Sun.ExpandAllNodes()
        treeHondaW3Mon.ExpandAllNodes()
        treeHondaW3Tue.ExpandAllNodes()
        treeHondaW3Wed.ExpandAllNodes()
        treeHondaW3Thu.ExpandAllNodes()
        treeHondaW3Fri.ExpandAllNodes()
        treeHondaW3Sat.ExpandAllNodes()
        treeHondaW3Sun.ExpandAllNodes()
        treeHondaW4Mon.ExpandAllNodes()
        treeHondaW4Tue.ExpandAllNodes()
        treeHondaW4Wed.ExpandAllNodes()
        treeHondaW4Thu.ExpandAllNodes()
        treeHondaW4Fri.ExpandAllNodes()
        treeHondaW4Sat.ExpandAllNodes()
        treeHondaW4Sun.ExpandAllNodes()
        treeHondaW5Mon.ExpandAllNodes()
        treehondaW5Tue.ExpandAllNodes()
        treeDay.ExpandAllNodes()
        treeHondaDay.ExpandAllNodes()
    End Sub

    Private Sub cmdCollapseAll_Click(sender As Object, e As System.EventArgs) Handles cmdCollapseAll.Click
        treeW1Mon.CollapseAllNodes()
        treeW1Tue.CollapseAllNodes()
        treeW1Wed.CollapseAllNodes()
        treeW1Thu.CollapseAllNodes()
        treeW1Fri.CollapseAllNodes()
        treeW1Sat.CollapseAllNodes()
        treeW1Sun.CollapseAllNodes()
        treeW2Mon.CollapseAllNodes()
        treeW2Tue.CollapseAllNodes()
        treeW2Wed.CollapseAllNodes()
        treeW2Thu.CollapseAllNodes()
        treeW2Fri.CollapseAllNodes()
        treeW2Sat.CollapseAllNodes()
        treeW2Sun.CollapseAllNodes()
        treeW3Mon.CollapseAllNodes()
        treeW3Tue.CollapseAllNodes()
        treeW3Wed.CollapseAllNodes()
        treeW3Thu.CollapseAllNodes()
        treeW3Fri.CollapseAllNodes()
        treeW3Sat.CollapseAllNodes()
        treeW3Sun.CollapseAllNodes()
        treeW4Mon.CollapseAllNodes()
        treeW4Tue.CollapseAllNodes()
        treeW4Wed.CollapseAllNodes()
        treeW4Thu.CollapseAllNodes()
        treeW4Fri.CollapseAllNodes()
        treeW4Sat.CollapseAllNodes()
        treeW4Sun.CollapseAllNodes()
        treeW5Mon.CollapseAllNodes()
        treeW5Tue.CollapseAllNodes()
        treeHondaW1Mon.CollapseAllNodes()
        treeHondaW1Tue.CollapseAllNodes()
        treeHondaW1Wed.CollapseAllNodes()
        treeHondaW1Thu.CollapseAllNodes()
        treeHondaW1Fri.CollapseAllNodes()
        treeHondaW1Sat.CollapseAllNodes()
        treeHondaW1Sun.CollapseAllNodes()
        treeHondaW2Mon.CollapseAllNodes()
        treeHondaW2Tue.CollapseAllNodes()
        treeHondaW2Wed.CollapseAllNodes()
        treeHondaW2Thu.CollapseAllNodes()
        treeHondaW2Fri.CollapseAllNodes()
        treeHondaW2Sat.CollapseAllNodes()
        treeHondaW2Sun.CollapseAllNodes()
        treeHondaW3Mon.CollapseAllNodes()
        treeHondaW3Tue.CollapseAllNodes()
        treeHondaW3Wed.CollapseAllNodes()
        treeHondaW3Thu.CollapseAllNodes()
        treeHondaW3Fri.CollapseAllNodes()
        treeHondaW3Sat.CollapseAllNodes()
        treeHondaW3Sun.CollapseAllNodes()
        treeHondaW4Mon.CollapseAllNodes()
        treeHondaW4Tue.CollapseAllNodes()
        treeHondaW4Wed.CollapseAllNodes()
        treeHondaW4Thu.CollapseAllNodes()
        treeHondaW4Fri.CollapseAllNodes()
        treeHondaW4Sat.CollapseAllNodes()
        treeHondaW4Sun.CollapseAllNodes()
        treeHondaW5Mon.CollapseAllNodes()
        treehondaW5Tue.CollapseAllNodes()
        treeDay.CollapseAllNodes()
        treeHondaDay.CollapseAllNodes()
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
                Case "New"
                    ScriptManager.RegisterStartupScript(cmdNew, cmdNew.GetType(), "ShowNewDlg", "OpenNewDialog();", True)
                Case "Move"
                    ScriptManager.RegisterStartupScript(cmdMoveDlg, cmdMoveDlg.GetType(), "ShowMoveDlg", "OpenMoveDialog();", True)
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

    'Private Sub treeMove_NodeDataBound(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs) Handles treeMove.NodeDataBound
    '    Try
    '        Dim bSetexOrder As Boolean
    '        Dim dataSourceRow As DataRowView = CType(e.Node.DataItem, DataRowView)
    '        'e.Item.Attributes("customAttribute1") = dataSourceRow("CustomAttribute2").ToString()
    '        e.Node.CssClass = "comboBoxPrePopImage"
    '        e.Node.ImageUrl = String.Format("../../Images/Misc/{0}", dataSourceRow("ImageName").ToString())
    '        e.Node.Selected = CBool(dataSourceRow("IsSelected").ToString())
    '        e.Node.AllowDrop = False
    '        If e.Node.Level > 0 Then
    '            'e.Node.AllowDrag = False
    '        End If

    '        If (Boolean.TryParse(dataSourceRow("SetexOrder").ToString(), bSetexOrder)) Then
    '            If bSetexOrder = False Then
    '                e.Node.Text = "<span Class='allOrders'>" + e.Node.Text + "</span>"
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Master.eMsg(ex.ToString())
    '    End Try
    'End Sub

    'Protected Sub treeMove_NodeDrop(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeDragDropEventArgs)
    '    Try

    '        ''181217 tdye replaced the following section
    '        'Dim lastLot As String = e.DraggedNodes(0).Value

    '        'For Each node As RadTreeNode In e.DraggedNodes
    '        '    If node.Level = 0 Then
    '        '        MoveLot(node.Value, e.DropPosition.ToString, e.DestDragNode.Value)
    '        '    Else
    '        '        MoveSubLot(node.Value, e.DropPosition.ToString, e.DestDragNode.Value)
    '        '    End If
    '        'Next

    '        Dim lastLot As String = e.DraggedNodes(0).Value
    '        Dim intNodeIndex As Integer = e.DestDragNode.Index
    '        Dim intNextNodeIndex As Integer = intNodeIndex + 1
    '        Dim strDestNode As String
    '        Dim strDropPosition As String
    '        Dim strLotNumbers As String = ""
    '        Dim bDone As Boolean = False

    '        If String.Equals(e.DropPosition.ToString, "Below", StringComparison.OrdinalIgnoreCase) Then
    '            strDestNode = e.DestDragNode.TreeView.Nodes(intNextNodeIndex).Value
    '            strDropPosition = "Above"
    '        Else
    '            strDestNode = e.DestDragNode.Value
    '            strDropPosition = "Above"
    '        End If


    '        '20191001 PAH Modified to allow up to 2 dragged lots
    '        For Each node As RadTreeNode In e.DraggedNodes
    '            If node.Level = 0 And e.DraggedNodes.Count > 2 Then
    '                Master.eMsg("No more than 2 Lots may be selected.")
    '                Exit For
    '            End If
    '            If node.Level = 0 And (hidIsSetexOrder.Value = "1") Then  'this is a setex order
    '                'pah MoveLot(node.Value, strDropPosition, strDestNode)  this has been commented out due to the 30 day view not using drag and drop
    '                bDone = True 'skip the upcoming MoveLot
    '            ElseIf node.Level = 0 And (hidIsSetexOrder.Value <> "1") Then 'all orders
    '                strLotNumbers = strLotNumbers & (node.Value.Substring(0, 8) & ",")
    '            Else
    '                MoveSubLot(node.Value, strDropPosition, strDestNode)
    '                bDone = True 'skip the upcoming MoveLot
    '            End If
    '        Next

    '        If Not bDone And strLotNumbers <> "" Then
    '            'pah MoveLot(strLotNumbers, strDropPosition, strDestNode)   this has been commented out due to the 30 day view not using drag and drop
    '            bDone = True
    '        End If

    '        LoadProdData()
    '        hidNodeLot.Value = lastLot
    '        hidLotNum8.Value = lastLot.Substring(0, 8)
    '        'pah     treeMove.DataBind()
    '        SelectLastSelectedNode()

    '    Catch ex As Exception
    '        Master.eMsg(ex.ToString())
    '    End Try

    'End Sub

    'pah Private Sub ddlMoveBP_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles ddlMoveBP.SelectedIndexChanged
    '    ClearHiddenNodeValues()
    '    MoveLots_Rebind()
    'End Sub

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
        hidNodeSeq.Value = ""
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
        treeHondaDay.Nodes.Clear()
        Me.lblTotalSingleDay.Text = NO_TOTAL
        Me.lblTotalSingleDayH.Text = NO_TOTAL
    End Sub

    Private Sub ClearWeekTreesTotals()
        Me.treeW1Mon.Nodes.Clear()
        Me.treeW1Tue.Nodes.Clear()
        Me.treeW1Wed.Nodes.Clear()
        Me.treeW1Thu.Nodes.Clear()
        Me.treeW1Fri.Nodes.Clear()
        Me.treeW1Sat.Nodes.Clear()
        Me.treeW1Sun.Nodes.Clear()
        Me.lblW1TotalMon.Text = NO_TOTAL
        Me.lblW1TotalTue.Text = NO_TOTAL
        Me.lblW1TotalWed.Text = NO_TOTAL
        Me.lblW1TotalThu.Text = NO_TOTAL
        Me.lblW1TotalFri.Text = NO_TOTAL
        Me.lblW1TotalSat.Text = NO_TOTAL
        Me.lblW1TotalSun.Text = NO_TOTAL

        treeHondaW1Mon.Nodes.Clear()
        treeHondaW1Tue.Nodes.Clear()
        treeHondaW1Wed.Nodes.Clear()
        treeHondaW1Thu.Nodes.Clear()
        treeHondaW1Fri.Nodes.Clear()
        treeHondaW1Sat.Nodes.Clear()
        treeHondaW1Sun.Nodes.Clear()
        Me.lblHondaW1TotalMon.Text = NO_TOTAL
        Me.lblHondaW1TotalTue.Text = NO_TOTAL
        Me.lblHondaW1TotalWed.Text = NO_TOTAL
        Me.lblHondaW1TotalThu.Text = NO_TOTAL
        Me.lblHondaW1TotalFri.Text = NO_TOTAL
        Me.lblHondaW1TotalSat.Text = NO_TOTAL
        Me.lblHondaW1TotalSun.Text = NO_TOTAL

        Me.treeW2Mon.Nodes.Clear()
        Me.treeW2Tue.Nodes.Clear()
        Me.treeW2Wed.Nodes.Clear()
        Me.treeW2Thu.Nodes.Clear()
        Me.treeW2Fri.Nodes.Clear()
        Me.treeW2Sat.Nodes.Clear()
        Me.treeW2Sun.Nodes.Clear()
        Me.lblW2TotalMon.Text = NO_TOTAL
        Me.lblW2TotalTue.Text = NO_TOTAL
        Me.lblW2TotalWed.Text = NO_TOTAL
        Me.lblW2TotalThu.Text = NO_TOTAL
        Me.lblW2TotalFri.Text = NO_TOTAL
        Me.lblW2TotalSat.Text = NO_TOTAL
        Me.lblW2TotalSun.Text = NO_TOTAL
        treeHondaW2Mon.Nodes.Clear()
        treeHondaW2Tue.Nodes.Clear()
        treeHondaW2Wed.Nodes.Clear()
        treeHondaW2Thu.Nodes.Clear()
        treeHondaW2Fri.Nodes.Clear()
        treeHondaW2Sat.Nodes.Clear()
        treeHondaW2Sun.Nodes.Clear()
        Me.lblHondaW2TotalMon.Text = NO_TOTAL
        Me.lblHondaW2TotalTue.Text = NO_TOTAL
        Me.lblHondaW2TotalWed.Text = NO_TOTAL
        Me.lblHondaW2TotalThu.Text = NO_TOTAL
        Me.lblHondaW2TotalFri.Text = NO_TOTAL
        Me.lblHondaW2TotalSat.Text = NO_TOTAL
        Me.lblHondaW2TotalSun.Text = NO_TOTAL

        Me.treeW3Mon.Nodes.Clear()
        Me.treeW3Tue.Nodes.Clear()
        Me.treeW3Wed.Nodes.Clear()
        Me.treeW3Thu.Nodes.Clear()
        Me.treeW3Fri.Nodes.Clear()
        Me.treeW3Sat.Nodes.Clear()
        Me.treeW3Sun.Nodes.Clear()
        Me.lblW3TotalMon.Text = NO_TOTAL
        Me.lblW3TotalTue.Text = NO_TOTAL
        Me.lblW3TotalWed.Text = NO_TOTAL
        Me.lblW3TotalThu.Text = NO_TOTAL
        Me.lblW3TotalFri.Text = NO_TOTAL
        Me.lblW3TotalSat.Text = NO_TOTAL
        Me.lblW3TotalSun.Text = NO_TOTAL
        treeHondaW3Mon.Nodes.Clear()
        treeHondaW3Tue.Nodes.Clear()
        treeHondaW3Wed.Nodes.Clear()
        treeHondaW3Thu.Nodes.Clear()
        treeHondaW3Fri.Nodes.Clear()
        treeHondaW3Sat.Nodes.Clear()
        treeHondaW3Sun.Nodes.Clear()
        Me.lblHondaW3TotalMon.Text = NO_TOTAL
        Me.lblHondaW3TotalTue.Text = NO_TOTAL
        Me.lblHondaW3TotalWed.Text = NO_TOTAL
        Me.lblHondaW3TotalThu.Text = NO_TOTAL
        Me.lblHondaW3TotalFri.Text = NO_TOTAL
        Me.lblHondaW3TotalSat.Text = NO_TOTAL
        Me.lblHondaW3TotalSun.Text = NO_TOTAL

        Me.treeW4Mon.Nodes.Clear()
        Me.treeW4Tue.Nodes.Clear()
        Me.treeW4Wed.Nodes.Clear()
        Me.treeW4Thu.Nodes.Clear()
        Me.treeW4Fri.Nodes.Clear()
        Me.treeW4Sat.Nodes.Clear()
        Me.treeW4Sun.Nodes.Clear()
        Me.lblW4TotalMon.Text = NO_TOTAL
        Me.lblW4TotalTue.Text = NO_TOTAL
        Me.lblW4TotalWed.Text = NO_TOTAL
        Me.lblW4TotalThu.Text = NO_TOTAL
        Me.lblW4TotalFri.Text = NO_TOTAL
        Me.lblW4TotalSat.Text = NO_TOTAL
        Me.lblW4TotalSun.Text = NO_TOTAL
        treeHondaW4Mon.Nodes.Clear()
        treeHondaW4Tue.Nodes.Clear()
        treeHondaW4Wed.Nodes.Clear()
        treeHondaW4Thu.Nodes.Clear()
        treeHondaW4Fri.Nodes.Clear()
        treeHondaW4Sat.Nodes.Clear()
        treeHondaW4Sun.Nodes.Clear()
        Me.lblHondaW4TotalMon.Text = NO_TOTAL
        Me.lblHondaW4TotalTue.Text = NO_TOTAL
        Me.lblHondaW4TotalWed.Text = NO_TOTAL
        Me.lblHondaW4TotalThu.Text = NO_TOTAL
        Me.lblHondaW4TotalFri.Text = NO_TOTAL
        Me.lblHondaW4TotalSat.Text = NO_TOTAL
        Me.lblHondaW4TotalSun.Text = NO_TOTAL

        Me.treeW5Mon.Nodes.Clear()
        Me.treeW5Tue.Nodes.Clear()
        Me.lblW5TotalMon.Text = NO_TOTAL
        Me.lblW5TotalTue.Text = NO_TOTAL
        treeHondaW5Mon.Nodes.Clear()
        treeHondaW5Tue.Nodes.Clear()
        Me.lblHondaW5TotalMon.Text = NO_TOTAL
        Me.lblHondaW5TotalTue.Text = NO_TOTAL

        Me.lblW1Total.Text = NO_TOTAL
        Me.lblW2Total.Text = NO_TOTAL
        Me.lblW3Total.Text = NO_TOTAL
        Me.lblW4Total.Text = NO_TOTAL
        Me.lblW5Total.Text = NO_TOTAL


    End Sub

    Private Function LoadProdData() As Boolean
        Dim bResult As Boolean = False
        Try
            ClearHiddenNodeValues()

            If (hidLastTab.Value = "1") Or (hidLastTabH.Value = "1") Then
                bResult = LoadDay()
            End If

            If (hidLastTabH.Value = "0") And (hidLastTab.Value = "0") Then
                Me.lblWeekTotalCaption.Visible = True
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

                    'cnt = cnt + 1
                    cnt += 1
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

    Private Function GetDayID(formattedDate As String) As Integer
        Dim dayofweek As Integer = CDate(formattedDate).DayOfWeek()
        GetDayID = dayofweek - 1
    End Function

    Private Function LoadDay() As Boolean
        Dim bResult As Boolean = False
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim oSqlHParameter As SqlParameter
        Dim colHParameters As New List(Of SqlParameter)
        Dim ds As DataSet = Nothing
        Dim dsHonda As DataSet = Nothing
        Dim dt As Date
        Dim RevisedDt As Date
        Dim intDayID As Integer
        Dim strDT As String = ""

        Try

            If (Date.TryParse(Me.tbDay.Text, dt) = False) Then
                Master.Msg = "Error: Please enter a Valid Date."
                bResult = False
            Else
                strDT = Utility.FormattedDate(Me.tbDay.Text)
                intDayID = GetDayID(strDT)
                RevisedDt = Utility.GetMondayOfDate(strDT).Date()
                strDT = RevisedDt.ToString("MM/dd/yyyy")

                oSqlParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
                oSqlParameter.Value = strDT 'dt.ToString("MM/dd/yyyy")
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@DayofWeekID", SqlDbType.Int)
                oSqlParameter.Value = intDayID
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
                oSqlParameter.Value = If(cbHold.Checked, 0, 1)
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@NumDays", SqlDbType.Int)
                oSqlParameter.Value = 7
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@SSRSReport", SqlDbType.Bit)
                oSqlParameter.Value = 0
                colParameters.Add(oSqlParameter)


                oSqlHParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
                oSqlHParameter.Value = strDT 'dt.ToString("MM/dd/yyyy")
                colHParameters.Add(oSqlHParameter)

                oSqlHParameter = New SqlParameter("@DayofWeekID", SqlDbType.Int)
                oSqlHParameter.Value = intDayID
                colHParameters.Add(oSqlHParameter)

                oSqlHParameter = New SqlParameter("@Report", SqlDbType.Bit)
                oSqlHParameter.Value = If(cbHold.Checked, 0, 1)
                colHParameters.Add(oSqlHParameter)

                oSqlHParameter = New SqlParameter("@NumDays", SqlDbType.Int)
                oSqlHParameter.Value = 7
                colHParameters.Add(oSqlHParameter)

                oSqlHParameter = New SqlParameter("@SSRSReport", SqlDbType.Bit)
                oSqlHParameter.Value = 0
                colHParameters.Add(oSqlHParameter)

                'Upper Honda
                If hidLastTabH.Value = "1" Then
                    lblSingleDayOfWeekH.Text = dt.ToString("MM/dd/yyyy")
                    dsHonda = DA.GetDataSet("procPSGetProdSchedHonda", colHParameters)
                    If (DA.IsDataSetNotEmpty(dsHonda)) AndAlso (dsHonda.Tables.Count >= 2) Then
                        If (dsHonda.Tables(1) IsNot Nothing) AndAlso (dsHonda.Tables(1).DefaultView.Table.Rows(0)(0) IsNot Nothing) Then
                            lblTotalSingleDayH.Text = dsHonda.Tables(dsHonda.Tables.Count - 1).DefaultView.Table.Rows(0)(0).ToString()
                            'lblW1Total.Text = dsHonda.Tables(dsHonda.Tables.Count - 1).DefaultView.Table.Rows(0)(0).ToString()
                            'Me.lblW1Total.Text = "Honda Day Total: " & lblTotalSingleDayH.Text

                        End If
                        If (PopulateTreeView(treeHondaDay, dsHonda.Tables(0))) Then
                            Utility.TreeExpand(treeHondaDay, 1)
                        End If
                    End If

                End If

                'Lower Setex
                If hidLastTab.Value = "1" Then
                    'Me.lblW1Total.Text = ""
                    Me.lblWeekTotalCaption.Visible = False
                    'Me.lblW1Total.Visible = True
                    Me.lblW2Total.Visible = False
                    Me.lblW3Total.Visible = False
                    Me.lblW4Total.Visible = False
                    Me.lblW5Total.Visible = False
                    Me.lblW1.Visible = False
                    Me.lblW2.Visible = False
                    Me.lblW3.Visible = False
                    Me.lblw4.Visible = False
                    Me.lblW5.Visible = False
                    lblSingleDayOfWeek.Text = dt.ToString("MM/dd/yyyy")
                    ds = DA.GetDataSet("procPSGetProdSched", colParameters)
                    ''error checking
                    If (DA.IsDataSetNotEmpty(ds)) AndAlso (ds.Tables.Count >= 2) Then
                        If (ds.Tables(1) IsNot Nothing) AndAlso (ds.Tables(1).DefaultView.Table.Rows(0)(0) IsNot Nothing) Then
                            lblTotalSingleDay.Text = ds.Tables(ds.Tables.Count - 1).DefaultView.Table.Rows(0)(0).ToString()
                            Me.lblW1Total.Text = "Setex Day Total: " & lblTotalSingleDay.Text
                        End If
                        If (PopulateTreeView(treeDay, ds.Tables(0))) Then
                            Utility.TreeExpand(treeDay, 1)
                        End If
                    End If

                End If

                LoadRevision(1, Utility.GetMondayOfDate(Me.tbDay.Text).ToString("MM/dd/yyyy"))

                End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function LoadWeek() As Boolean
        Dim bResult As Boolean = True
        Dim ds As DataSet = Nothing
        Dim dsHonda As DataSet = Nothing
        Dim NumberOfLastTable As Integer = 0
        Dim NumberOfLastHondaTable As Integer = 0
        Dim dMonday As Date = Utility.GetMondayOfDate(Me.tbDay.Text)
        Dim strMonday As String = dMonday.ToShortDateString()
        Dim CurrentTableIndex As Integer = 0
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        Try



            If hidLastTab.Value = "0" And hidLastTabH.Value = "0" Then
                ResetProdSched30Labels()
                If (Me.tbDay.Text.Length > 0) Then

                    oSqlParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
                    oSqlParameter.Value = strMonday
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
                    oSqlParameter.Value = If(cbHold.Checked, 0, 1)
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@NumDays", SqlDbType.Int)
                    oSqlParameter.Value = 50
                    colParameters.Add(oSqlParameter)

                    ds = DA.GetDataSet("procPSGetProdSched", colParameters)

                    oSqlParameter = Nothing
                    colParameters.Clear()

                    oSqlParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
                    oSqlParameter.Value = strMonday
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
                    oSqlParameter.Value = If(cbHold.Checked, 0, 1)
                    colParameters.Add(oSqlParameter)

                    oSqlParameter = New SqlParameter("@NumDays", SqlDbType.Int)
                    oSqlParameter.Value = 50
                    colParameters.Add(oSqlParameter)

                    dsHonda = DA.GetDataSet("procPSGetProdSchedHonda", colParameters)

                    If (ds IsNot Nothing) AndAlso (ds.Tables.Count > 1) Then

                        NumberOfLastTable = ds.Tables.Count - 1

                        bResult = bResult And PopulateTreeView(treeW1Mon, ds.Tables(0))
                        lblW1DateMon.Text = ds.Tables(0).Rows(0).ItemArray(1).ToString
                        'CurrentTableIndex = CurrentTableIndex + 1
                        CurrentTableIndex += 1

                        If (ds.Tables.Count >= 1 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW1Tue, ds.Tables(1))
                            lblW1DateTue.Text = ds.Tables(1).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeW1Tue.Nodes.Clear()
                            lblW1DateTue.Visible = False
                            lblW1TotalTue.Visible = False
                            Label2.Visible = False
                        End If
                        If (ds.Tables.Count >= 2 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW1Wed, ds.Tables(2))
                            lblW1DateWed.Text = ds.Tables(2).Rows(0).ItemArray(1).ToString
                            lblW1DateWed.Visible = True
                            lblW1TotalWed.Visible = True
                            Label3.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW1Wed.Nodes.Clear()
                            lblW1DateWed.Visible = False
                            lblW1TotalWed.Visible = False
                            Label3.Visible = False
                        End If
                        If (ds.Tables.Count >= 3 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW1Thu, ds.Tables(3))
                            lblW1DateThu.Text = ds.Tables(3).Rows(0).ItemArray(1).ToString
                            lblW1DateThu.Visible = True
                            lblW1TotalThu.Visible = True
                            Label4.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW1Thu.Nodes.Clear()
                            lblW1DateThu.Visible = False
                            lblW1TotalThu.Visible = False
                            Label4.Visible = False
                        End If
                        If (ds.Tables.Count >= 4 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW1Fri, ds.Tables(4))
                            lblW1DateFri.Text = ds.Tables(4).Rows(0).ItemArray(1).ToString
                            lblW1DateFri.Visible = True
                            lblW1TotalFri.Visible = True
                            Label5.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW1Fri.Nodes.Clear()
                            lblW1DateFri.Visible = False
                            lblW1TotalFri.Visible = False
                            Label5.Visible = False
                        End If
                        If (ds.Tables.Count >= 5 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW1Sat, ds.Tables(5))
                            lblW1DateSat.Text = ds.Tables(5).Rows(0).ItemArray(1).ToString
                            lblW1DateSat.Visible = True
                            lblW1TotalSat.Visible = True
                            Label6.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW1Sat.Nodes.Clear()
                            lblW1DateSat.Visible = False
                            lblW1TotalSat.Visible = False
                            Label6.Visible = False
                        End If
                        If (ds.Tables.Count >= 6 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW1Sun, ds.Tables(6))
                            lblW1DateSun.Text = ds.Tables(6).Rows(0).ItemArray(1).ToString
                            lblW1DateSun.Visible = True
                            lblW1TotalSun.Visible = True
                            Label20.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW1Sun.Nodes.Clear()
                            lblW1DateSun.Visible = False
                            lblW1TotalSun.Visible = False
                            Label20.Visible = False
                        End If
                        If (ds.Tables.Count >= 7 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW2Mon, ds.Tables(7))
                            lblW2DateMon.Text = ds.Tables(7).Rows(0).ItemArray(1).ToString
                            lblW2DateMon.Visible = True
                            lblW2TotalMon.Visible = True
                            Label22.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW2Mon.Nodes.Clear()
                            lblW2DateMon.Visible = False
                            lblW2TotalMon.Visible = False
                            Label22.Visible = False
                        End If
                        If (ds.Tables.Count >= 8 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW2Tue, ds.Tables(8))
                            lblW2DateTue.Text = ds.Tables(8).Rows(0).ItemArray(1).ToString
                            lblW2DateTue.Visible = True
                            lblW2TotalTue.Visible = True
                            Label27.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW2Tue.Nodes.Clear()
                            lblW2DateTue.Visible = False
                            lblW2TotalTue.Visible = False
                            Label27.Visible = False
                        End If
                        If (ds.Tables.Count >= 9 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW2Wed, ds.Tables(9))
                            lblW2DateWed.Text = ds.Tables(9).Rows(0).ItemArray(1).ToString
                            lblW2DateWed.Visible = True
                            lblW2TotalWed.Visible = True
                            Label34.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW2Wed.Nodes.Clear()
                            lblW2DateWed.Visible = False
                            lblW2TotalWed.Visible = False
                            Label34.Visible = False
                        End If
                        If (ds.Tables.Count >= 10 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW2Thu, ds.Tables(10))
                            lblW2DateThu.Text = ds.Tables(10).Rows(0).ItemArray(1).ToString
                            lblW2DateThu.Visible = True
                            lblW2TotalThu.Visible = True
                            Label35.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW2Thu.Nodes.Clear()
                            lblW2DateThu.Visible = False
                            lblW2TotalThu.Visible = False
                            Label35.Visible = False
                        End If
                        If (ds.Tables.Count >= 11 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW2Fri, ds.Tables(11))
                            lblW2DateFri.Text = ds.Tables(11).Rows(0).ItemArray(1).ToString
                            lblW2DateFri.Visible = True
                            lblW2TotalFri.Visible = True
                            Label36.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW2Fri.Nodes.Clear()
                            lblW2DateFri.Visible = False
                            lblW2TotalFri.Visible = False
                            Label36.Visible = False
                        End If
                        If (ds.Tables.Count >= 12 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW2Sat, ds.Tables(12))
                            lblW2DateSat.Text = ds.Tables(12).Rows(0).ItemArray(1).ToString
                            lblW2DateSat.Visible = True
                            lblW2TotalSat.Visible = True
                            Label37.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW2Sat.Nodes.Clear()
                            lblW2DateSat.Visible = False
                            lblW2TotalSat.Visible = False
                            Label37.Visible = False
                        End If
                        If (ds.Tables.Count >= 13 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW2Sun, ds.Tables(13))
                            lblW2DateSun.Text = ds.Tables(13).Rows(0).ItemArray(1).ToString
                            lblW2DateSun.Visible = True
                            lblW2TotalSun.Visible = True
                            Label38.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW2Sun.Nodes.Clear()
                            lblW2DateSun.Visible = False
                            lblW2TotalSun.Visible = False
                            Label38.Visible = False
                        End If
                        If (ds.Tables.Count >= 14 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW3Mon, ds.Tables(14))
                            lblW3DateMon.Text = ds.Tables(14).Rows(0).ItemArray(1).ToString
                            lblW3DateMon.Visible = True
                            lblW3TotalMon.Visible = True
                            Label39.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW3Mon.Nodes.Clear()
                            lblW3DateMon.Visible = False
                            lblW3TotalMon.Visible = False
                            Label39.Visible = False
                        End If
                        If (ds.Tables.Count >= 15 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW3Tue, ds.Tables(15))
                            lblW3DateTue.Text = ds.Tables(15).Rows(0).ItemArray(1).ToString
                            lblW3DateTue.Visible = True
                            lblW3TotalTue.Visible = True
                            Label40.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW3Tue.Nodes.Clear()
                            lblW3DateTue.Visible = False
                            lblW3TotalTue.Visible = False
                            Label40.Visible = False
                        End If
                        If (ds.Tables.Count >= 16 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW3Wed, ds.Tables(16))
                            lblW3DateWed.Text = ds.Tables(16).Rows(1).ItemArray(1).ToString
                            lblW3DateWed.Visible = True
                            lblW3TotalWed.Visible = True
                            Label41.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW3Wed.Nodes.Clear()
                            lblW3DateWed.Visible = False
                            lblW3TotalWed.Visible = False
                            Label41.Visible = False
                        End If
                        If (ds.Tables.Count >= 17 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW3Thu, ds.Tables(17))
                            lblW3DateThu.Text = ds.Tables(17).Rows(0).ItemArray(1).ToString
                            lblW3DateThu.Visible = True
                            lblW3TotalThu.Visible = True
                            Label42.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW3Thu.Nodes.Clear()
                            lblW3DateThu.Visible = False
                            lblW3TotalThu.Visible = False
                            Label42.Visible = False
                        End If
                        If (ds.Tables.Count >= 18 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW3Fri, ds.Tables(18))
                            lblW3DateFri.Text = ds.Tables(18).Rows(0).ItemArray(1).ToString
                            lblW3DateFri.Visible = True
                            lblW3TotalFri.Visible = True
                            Label43.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW3Fri.Nodes.Clear()
                            lblW3DateFri.Visible = False
                            lblW3TotalFri.Visible = False
                            Label43.Visible = False
                        End If
                        If (ds.Tables.Count >= 19 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW3Sat, ds.Tables(19))
                            lblW3DateSat.Text = ds.Tables(19).Rows(0).ItemArray(1).ToString
                            lblW3DateSat.Visible = True
                            lblW3TotalSat.Visible = True
                            Label44.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW3Sat.Nodes.Clear()
                            lblW3DateSat.Visible = False
                            lblW3TotalSat.Visible = False
                            Label44.Visible = False
                        End If
                        If (ds.Tables.Count >= 20 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW3Sun, ds.Tables(20))
                            lblW3DateSun.Text = ds.Tables(20).Rows(0).ItemArray(1).ToString
                            lblW3DateSun.Visible = True
                            lblW3TotalSun.Visible = True
                            Label45.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW3Sun.Nodes.Clear()
                            lblW3DateSun.Visible = False
                            lblW3TotalSun.Visible = False
                            Label45.Visible = False
                        End If
                        If (ds.Tables.Count >= 21 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW4Mon, ds.Tables(21))
                            lblW4DateMon.Text = ds.Tables(21).Rows(0).ItemArray(1).ToString
                            lblW4DateMon.Visible = True
                            lblW4TotalMon.Visible = True
                            Label46.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW4Mon.Nodes.Clear()
                            lblW4DateMon.Visible = False
                            lblW4TotalMon.Visible = False
                            Label46.Visible = False
                        End If
                        If (ds.Tables.Count >= 22 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW4Tue, ds.Tables(22))
                            lblW4DateTue.Text = ds.Tables(22).Rows(0).ItemArray(1).ToString
                            lblW4DateTue.Visible = True
                            lblW4TotalTue.Visible = True
                            Label47.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW4Tue.Nodes.Clear()
                            lblW4DateTue.Visible = False
                            lblW4TotalTue.Visible = False
                            Label47.Visible = False
                        End If
                        If (ds.Tables.Count >= 23 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW4Wed, ds.Tables(23))
                            lblW4DateWed.Text = ds.Tables(23).Rows(0).ItemArray(1).ToString
                            lblW4DateWed.Visible = True
                            lblW4TotalWed.Visible = True
                            Label48.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW4Wed.Nodes.Clear()
                            lblW4DateWed.Visible = False
                            lblW4TotalWed.Visible = False
                            Label48.Visible = False
                        End If
                        If (ds.Tables.Count >= 24 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW4Thu, ds.Tables(24))
                            lblW4DateThu.Text = ds.Tables(24).Rows(0).ItemArray(1).ToString
                            lblW4DateThu.Visible = True
                            lblW4TotalThu.Visible = True
                            Label49.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW4Thu.Nodes.Clear()
                            lblW4DateThu.Visible = False
                            lblW4TotalThu.Visible = False
                            Label49.Visible = False
                        End If
                        If (ds.Tables.Count >= 25) And CurrentTableIndex < NumberOfLastTable Then
                            bResult = bResult And PopulateTreeView(treeW4Fri, ds.Tables(25))
                            lblW4DateFri.Text = ds.Tables(25).Rows(0).ItemArray(1).ToString
                            lblW4DateFri.Visible = True
                            lblW4TotalFri.Visible = True
                            Label50.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW4Fri.Nodes.Clear()
                            lblW4DateFri.Visible = False
                            lblW4TotalFri.Visible = False
                            Label50.Visible = False
                        End If
                        If (ds.Tables.Count >= 26 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW4Sat, ds.Tables(26))
                            lblW4DateSat.Text = ds.Tables(26).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeW4Sat.Nodes.Clear()
                            lblW4DateSat.Visible = False
                            lblW4TotalSat.Visible = False
                            Label51.Visible = False
                        End If
                        If (ds.Tables.Count >= 27 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW4Sun, ds.Tables(27))
                            lblW4DateSun.Text = ds.Tables(27).Rows(0).ItemArray(1).ToString
                            lblW4DateSun.Visible = True
                            lblW4TotalSun.Visible = True
                            Label52.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW4Sun.Nodes.Clear()
                            lblW4DateSun.Visible = False
                            lblW4TotalSun.Visible = False
                            Label52.Visible = False
                        End If
                        If (ds.Tables.Count >= 28 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW5Mon, ds.Tables(28))
                            lblW5DateMon.Text = ds.Tables(28).Rows(0).ItemArray(1).ToString
                            lblW5DateMon.Visible = True
                            lblW5TotalMon.Visible = True
                            Label83.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW5Mon.Nodes.Clear()
                            lblW5DateMon.Visible = False
                            lblW5TotalMon.Visible = False
                            Label83.Visible = False
                        End If
                        If (ds.Tables.Count >= 29 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeW5Tue, ds.Tables(29))
                            lblW5DateTue.Text = ds.Tables(29).Rows(0).ItemArray(1).ToString
                            lblW5DateTue.Visible = True
                            lblW5TotalTue.Visible = True
                            Label85.Visible = True
                            CurrentTableIndex += 1
                        Else
                            treeW5Tue.Nodes.Clear()
                            lblW5DateTue.Visible = False
                            lblW5TotalTue.Visible = False
                            Label85.Visible = False
                        End If

                        bResult = bResult And LoadWeekTotals(ds.Tables(ds.Tables.Count - 1))

                    End If


                    If (dsHonda IsNot Nothing) AndAlso (dsHonda.Tables.Count >= 1) Then 'pah

                        CurrentTableIndex = 0
                        NumberOfLastTable = dsHonda.Tables.Count - 1
                        ' If (dsHonda.Tables.Count >= 1 And CurrentTableIndex < NumberOfLastTable) Then
                        bResult = bResult And PopulateTreeView(treeHondaW1Mon, dsHonda.Tables(0))
                        lblHondaW1DateMon.Text = dsHonda.Tables(0).Rows(0).ItemArray(1).ToString
                        CurrentTableIndex += 1
                        'Else
                        '    treeHondaW1Tue.Nodes.Clear()
                        '    lblHondaW1DateTue.Visible = False
                        '    lblHondaW1TotalTue.Visible = False
                        '    Label13.Visible = False
                        'End If
                        If (dsHonda.Tables.Count >= 1 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW1Tue, dsHonda.Tables(1))
                            lblHondaW1DateTue.Text = dsHonda.Tables(1).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW1Tue.Nodes.Clear()
                            lblHondaW1DateTue.Visible = False
                            lblHondaW1TotalTue.Visible = False
                            Label13.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 2 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW1Wed, dsHonda.Tables(2))
                            lblHondaW1DateWed.Text = dsHonda.Tables(2).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW1Wed.Nodes.Clear()
                            lblHondaW1DateWed.Visible = False
                            lblHondaW1TotalWed.Visible = False
                            Label15.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 3 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW1Thu, dsHonda.Tables(3))
                            lblHondaW1DateThu.Text = dsHonda.Tables(3).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW1Thu.Nodes.Clear()
                            lblHondaW1DateThu.Visible = False
                            lblHondaW1TotalThu.Visible = False
                            Label17.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 4 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW1Fri, dsHonda.Tables(4))
                            lblHondaW1DateFri.Text = dsHonda.Tables(4).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW1Fri.Nodes.Clear()
                            lblHondaW1DateFri.Visible = False
                            lblHondaW1TotalFri.Visible = False
                            Label19.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 5 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW1Sat, dsHonda.Tables(5))
                            lblHondaW1DateSat.Text = dsHonda.Tables(5).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW1Sat.Nodes.Clear()
                            lblHondaW1DateSat.Visible = False
                            lblHondaW1TotalSat.Visible = False
                            Label59.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 6 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW1Sun, dsHonda.Tables(6))
                            lblHondaW1DateSun.Text = dsHonda.Tables(6).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW1Sun.Nodes.Clear()
                            lblHondaW1DateSun.Visible = False
                            lblHondaW1TotalSun.Visible = False
                            Label60.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 7 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW2Mon, dsHonda.Tables(7))
                            lblHondaW2DateMon.Text = dsHonda.Tables(7).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW2Mon.Nodes.Clear()
                            lblHondaW2DateMon.Visible = False
                            lblHondaW2TotalMon.Visible = False
                            Label61.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 8 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW2Tue, dsHonda.Tables(8))
                            lblHondaW2DateTue.Text = dsHonda.Tables(8).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW2Tue.Nodes.Clear()
                            lblHondaW2DateTue.Visible = False
                            lblHondaW2TotalTue.Visible = False
                            Label62.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 9 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW2Wed, dsHonda.Tables(9))
                            lblHondaW2DateWed.Text = dsHonda.Tables(9).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW2Wed.Nodes.Clear()
                            lblHondaW2DateWed.Visible = False
                            lblHondaW2TotalWed.Visible = False
                            Label63.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 10 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW2Thu, dsHonda.Tables(10))
                            lblHondaW2DateThu.Text = dsHonda.Tables(10).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW2Thu.Nodes.Clear()
                            lblHondaW2DateThu.Visible = False
                            lblHondaW2TotalThu.Visible = False
                            Label64.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 11 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW2Fri, dsHonda.Tables(11))
                            lblHondaW2DateFri.Text = dsHonda.Tables(11).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW2Fri.Nodes.Clear()
                            lblHondaW2DateFri.Visible = False
                            lblHondaW2TotalFri.Visible = False
                            Label65.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 12 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW2Sat, dsHonda.Tables(12))
                            lblHondaW2DateSat.Text = dsHonda.Tables(12).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW2Sat.Nodes.Clear()
                            lblHondaW2DateSat.Visible = False
                            lblHondaW2TotalSat.Visible = False
                            Label66.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 13 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW2Sun, dsHonda.Tables(13))
                            lblHondaW2DateSun.Text = dsHonda.Tables(13).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW2Sun.Nodes.Clear()
                            lblHondaW2DateSun.Visible = False
                            lblHondaW2TotalSun.Visible = False
                            Label67.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 14 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW3Mon, dsHonda.Tables(14))
                            lblHondaW3DateMon.Text = dsHonda.Tables(14).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW3Mon.Nodes.Clear()
                            lblHondaW3DateMon.Visible = False
                            lblHondaW3TotalMon.Visible = False
                            Label68.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 15 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW3Tue, dsHonda.Tables(15))
                            lblHondaW3DateTue.Text = dsHonda.Tables(15).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW3Tue.Nodes.Clear()
                            lblHondaW3DateTue.Visible = False
                            lblHondaW3TotalTue.Visible = False
                            Label69.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 16 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW3Wed, dsHonda.Tables(16))
                            lblHondaW3DateWed.Text = dsHonda.Tables(16).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW3Wed.Nodes.Clear()
                            lblHondaW3DateWed.Visible = False
                            lblHondaW3TotalWed.Visible = False
                            Label70.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 17 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW3Thu, dsHonda.Tables(17))
                            lblHondaW3DateThu.Text = dsHonda.Tables(17).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW3Thu.Nodes.Clear()
                            lblHondaW3DateThu.Visible = False
                            lblHondaW3TotalThu.Visible = False
                            Label71.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 18 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW3Fri, dsHonda.Tables(18))
                            lblHondaW3DateFri.Text = dsHonda.Tables(18).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW3Fri.Nodes.Clear()
                            lblHondaW3DateFri.Visible = False
                            lblHondaW3TotalFri.Visible = False
                            Label72.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 19 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW3Sat, dsHonda.Tables(19))
                            lblHondaW3DateSat.Text = dsHonda.Tables(19).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW3Sat.Nodes.Clear()
                            lblHondaW3DateSat.Visible = False
                            lblHondaW3TotalSat.Visible = False
                            Label73.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 20 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW3Sun, dsHonda.Tables(20))
                            lblHondaW3DateSun.Text = dsHonda.Tables(20).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW3Sun.Nodes.Clear()
                            lblHondaW3DateSun.Visible = False
                            lblHondaW3TotalSun.Visible = False
                            Label74.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 21 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW4Mon, dsHonda.Tables(21))
                            lblHondaW4DateMon.Text = dsHonda.Tables(21).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW4Mon.Nodes.Clear()
                            lblHondaW4DateMon.Visible = False
                            lblHondaW4TotalMon.Visible = False
                            Label75.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 22 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW4Tue, dsHonda.Tables(22))
                            lblHondaW4DateTue.Text = dsHonda.Tables(22).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW4Tue.Nodes.Clear()
                            lblHondaW4DateTue.Visible = False
                            lblHondaW4TotalTue.Visible = False
                            Label76.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 23 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW4Wed, dsHonda.Tables(23))
                            lblHondaW4DateWed.Text = dsHonda.Tables(23).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW4Wed.Nodes.Clear()
                            lblHondaW4DateWed.Visible = False
                            lblHondaW4TotalWed.Visible = False
                            Label77.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 24 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW4Thu, dsHonda.Tables(24))
                            lblHondaW4DateThu.Text = dsHonda.Tables(24).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW4Thu.Nodes.Clear()
                            lblHondaW4DateThu.Visible = False
                            lblHondaW4TotalThu.Visible = False
                            Label78.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 25 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW4Fri, dsHonda.Tables(25))
                            lblHondaW4DateFri.Text = dsHonda.Tables(25).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW4Fri.Nodes.Clear()
                            lblHondaW4DateFri.Visible = False
                            lblHondaW4TotalFri.Visible = False
                            Label79.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 26 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW4Sat, dsHonda.Tables(26))
                            lblHondaW4DateSat.Text = dsHonda.Tables(26).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW4Sat.Nodes.Clear()
                            lblHondaW4DateSat.Visible = False
                            lblHondaW4TotalSat.Visible = False
                            Label80.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 27 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW4Sun, dsHonda.Tables(27))
                            lblHondaW4DateSun.Text = dsHonda.Tables(27).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW4Sun.Nodes.Clear()
                            lblHondaW4DateSun.Visible = False
                            lblHondaW4TotalSun.Visible = False
                            Label81.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 28 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treeHondaW5Mon, dsHonda.Tables(28))
                            lblHondaW5DateMon.Text = dsHonda.Tables(28).Rows(0).ItemArray(1).ToString
                            CurrentTableIndex += 1
                        Else
                            treeHondaW5Mon.Nodes.Clear()
                            lblHondaW5DateMon.Visible = False
                            lblHondaW5TotalMon.Visible = False
                            Label84.Visible = False
                        End If
                        If (dsHonda.Tables.Count >= 29 And CurrentTableIndex < NumberOfLastTable) Then
                            bResult = bResult And PopulateTreeView(treehondaW5Tue, dsHonda.Tables(29))
                            lblHondaW5DateTue.Text = dsHonda.Tables(29).Rows(0).ItemArray(1).ToString
                        Else
                            treehondaW5Tue.Nodes.Clear()
                            lblHondaW5DateTue.Visible = False
                            lblHondaW5TotalTue.Visible = False
                            Label87.Visible = False
                        End If

                        bResult = bResult And LoadHondaWeekTotals(dsHonda.Tables(dsHonda.Tables.Count - 1))

                    End If


                    Utility.TreeExpand(treeW1Mon, 1)
                    Utility.TreeExpand(treeW1Tue, 1)
                    Utility.TreeExpand(treeW1Wed, 1)
                    Utility.TreeExpand(treeW1Thu, 1)
                    Utility.TreeExpand(treeW1Fri, 1)
                    Utility.TreeExpand(treeW1Sat, 1)
                    Utility.TreeExpand(treeW1Sun, 1)

                    Utility.TreeExpand(treeW2Mon, 1)
                    Utility.TreeExpand(treeW2Tue, 1)
                    Utility.TreeExpand(treeW2Wed, 1)
                    Utility.TreeExpand(treeW2Thu, 1)
                    Utility.TreeExpand(treeW2Fri, 1)
                    Utility.TreeExpand(treeW2Sat, 1)
                    Utility.TreeExpand(treeW2Sun, 1)

                    Utility.TreeExpand(treeW3Mon, 1)
                    Utility.TreeExpand(treeW3Tue, 1)
                    Utility.TreeExpand(treeW3Wed, 1)
                    Utility.TreeExpand(treeW3Thu, 1)
                    Utility.TreeExpand(treeW3Fri, 1)
                    Utility.TreeExpand(treeW3Sat, 1)
                    Utility.TreeExpand(treeW3Sun, 1)

                    Utility.TreeExpand(treeW4Mon, 1)
                    Utility.TreeExpand(treeW4Tue, 1)
                    Utility.TreeExpand(treeW4Wed, 1)
                    Utility.TreeExpand(treeW4Thu, 1)
                    Utility.TreeExpand(treeW4Fri, 1)
                    Utility.TreeExpand(treeW4Sat, 1)
                    Utility.TreeExpand(treeW4Sun, 1)

                    Utility.TreeExpand(treeW5Mon, 1)
                    Utility.TreeExpand(treeW5Tue, 1)

                    Utility.TreeExpand(treeHondaW1Mon, 1)
                    Utility.TreeExpand(treeHondaW1Tue, 1)
                    Utility.TreeExpand(treeHondaW1Wed, 1)
                    Utility.TreeExpand(treeHondaW1Thu, 1)
                    Utility.TreeExpand(treeHondaW1Fri, 1)
                    Utility.TreeExpand(treeHondaW1Sat, 1)
                    Utility.TreeExpand(treeHondaW1Sun, 1)

                    Utility.TreeExpand(treeHondaW2Mon, 1)
                    Utility.TreeExpand(treeHondaW2Tue, 1)
                    Utility.TreeExpand(treeHondaW2Wed, 1)
                    Utility.TreeExpand(treeHondaW2Thu, 1)
                    Utility.TreeExpand(treeHondaW2Fri, 1)
                    Utility.TreeExpand(treeHondaW2Sat, 1)
                    Utility.TreeExpand(treeHondaW2Sun, 1)

                    Utility.TreeExpand(treeHondaW3Mon, 1)
                    Utility.TreeExpand(treeHondaW3Tue, 1)
                    Utility.TreeExpand(treeHondaW3Wed, 1)
                    Utility.TreeExpand(treeHondaW3Thu, 1)
                    Utility.TreeExpand(treeHondaW3Fri, 1)
                    Utility.TreeExpand(treeHondaW3Sat, 1)
                    Utility.TreeExpand(treeHondaW3Sun, 1)

                    Utility.TreeExpand(treeHondaW4Mon, 1)
                    Utility.TreeExpand(treeHondaW4Tue, 1)
                    Utility.TreeExpand(treeHondaW4Wed, 1)
                    Utility.TreeExpand(treeHondaW4Thu, 1)
                    Utility.TreeExpand(treeHondaW4Fri, 1)
                    Utility.TreeExpand(treeHondaW4Sat, 1)
                    Utility.TreeExpand(treeHondaW4Sun, 1)

                    Utility.TreeExpand(treeHondaW5Mon, 1)
                    Utility.TreeExpand(treehondaW5Tue, 1)
                End If


                Dim adjustedTrackingDate As DateTime

                adjustedTrackingDate = CDate(dMonday.ToString("MM/dd/yyyy"))
                LoadRevision(1, adjustedTrackingDate.ToString("MM/dd/yyyy"))

                adjustedTrackingDate = adjustedTrackingDate.AddDays(7)
                LoadRevision(2, adjustedTrackingDate.ToString("MM/dd/yyyy"))

                adjustedTrackingDate = adjustedTrackingDate.AddDays(7)
                LoadRevision(3, adjustedTrackingDate.ToString("MM/dd/yyyy"))

                adjustedTrackingDate = adjustedTrackingDate.AddDays(7)
                LoadRevision(4, adjustedTrackingDate.ToString("MM/dd/yyyy"))

                hidMondaysDate.Value = Utility.GetMondayOfDate(Me.tbDay.Text).ToString("MM/dd/yyyy")

                Me.Label12.Text = "Comment For: " + CDate(hidMondaysDate.Value).AddDays(0).ToString("MM/dd/yyyy") + "   "
                Me.Label54.Text = "Comment For: " + CDate(hidMondaysDate.Value).AddDays(7).ToString("MM/dd/yyyy") + "   "
                Me.Label56.Text = "Comment For: " + CDate(hidMondaysDate.Value).AddDays(14).ToString("MM/dd/yyyy") + "   "
                Me.Label58.Text = "Comment For: " + CDate(hidMondaysDate.Value).AddDays(21).ToString("MM/dd/yyyy") + "   "
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
        Dim strIsSetexSchedule As String = "0"

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
                        strIsSetexSchedule = dt.Rows(i)(psGetSchedule.SetexSchedule).ToString()

                        Boolean.TryParse(dt.Rows(i)(psGetSchedule.SetexSchedule).ToString(), SetexFlag)

                        If (strQty = "") Then
                            strQty = "0"
                        End If

                        'add new shift node if needed
                        If (strShift <> dt.Rows(i)(psGetSchedule.Shift).ToString()) Then
                            strShift = dt.Rows(i)(psGetSchedule.Shift).ToString()
                            shiftNode = Nothing
                            shiftNode = New RadTreeNode()
                            'shiftNode.Text = SHIFT + strShift + dt.Rows(i)(psGetSchedule.ShiftSuffix).ToString()
                            shiftNode.Text = dt.Rows(i)(psGetSchedule.ShiftSuffix).ToString()
                            shiftNode.Value = strSeqDT
                            tree.Nodes.Add(shiftNode)
                            shiftChange = True
                        End If

                        'add new lot if needed
                        If ((strLot <> dt.Rows(i)(psGetSchedule.LotNumber8).ToString()) Or (shiftChange)) Then
                            'add quantity to previous lot and on hold text if needed
                            'lotNode.Text += (" : " + qtyLot.ToString())
                            'If (holdFlag) Then
                            '    lotNode.Text += ON_HOLD
                            'End If

                            ''''MS 06-06-2006 check if the ship code is "" then we know this is a customer order
                            ''''and therefore we want the background to be gray
                            '''If (hidIsSetexOrder.Value = "0") Then
                            '''    If (SetexFlag = False) Then
                            '''        lotNode.Text = "<span class='allOrders'>" + lotNode.Text + "</span>"
                            '''    End If
                            '''End If

                            qtyLot = 0
                            holdFlag = False

                            strLot = dt.Rows(i)(psGetSchedule.LotNumber8).ToString()
                            lotNode = Nothing
                            lotNode = New RadTreeNode

                            If (hidIsSetexOrder.Value = "0") Then
                                SetexSchedule = CBool(dt.Rows(i)(psGetSchedule.SetexSchedule))
                            End If


                            'lotNode.Text = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3)
                            'lotNode.Value = strSeqDT

                            lotNode.Text = strLot.Substring(3, 2) + "-" + strLot.Substring(5, 3) '+ "-" + strLot.Substring(5, 3)
                            lotNode.Value = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3)
                            lotNode.LongDesc = strSeqDT
                            lotNode.ImageUrl = String.Format("../../Images/Misc/{0}", dt.Rows(i)(psGetSchedule.ImageName))

                            If strIsSetexSchedule = "0" And tree.ID.Contains("Honda") Then
                                lotNode.BackColor = Drawing.Color.LightGray
                            End If

                            If (strOnHold.ToUpper = "TRUE") Then
                                'lotNode.Text = lotNode.Text + ON_HOLD
                                lotNode.BackColor = Drawing.Color.Pink
                                lotNode.Text += ON_HOLD
                                holdFlag = True
                            End If

                            shiftNode.Nodes.Add(lotNode)
                        End If
                        'reset shift change flag
                        shiftChange = False

                        'add sublot
                        'strSubLot = dt.Rows(i)(psGetSchedule.SubLot).ToString()
                        'subLotNode = Nothing
                        'subLotNode = New RadTreeNode

                        'subLotNode.Text = strLot.Substring(4, 5) '+ "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3) + "-" + strSubLot + " : " + strQty + " " + strBC
                        'subLotNode.Text = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3) + "-" + strSubLot + " : " + strQty + " " + strBC
                        'subLotNode.Value = strSeqDT

                        'subLotNode.ImageUrl = String.Format("../../Images/Misc/{0}", dt.Rows(i)(psGetSchedule.ImageName))

                        If (hidIsSetexOrder.Value = "0") Then
                            If (SetexFlag = False) Then
                                'subLotNode.Text = "<span class='allOrders'>" + subLotNode.Text + "</span>"
                            End If
                        End If
                        'lotNode.Nodes.Add(subLotNode)

                        qtyLot += Convert.ToInt32(strQty)

                    Next

                    ''add total to the very last lot
                    'lotNode.Text += (" : " + qtyLot.ToString())
                    'If (rbOrders.SelectedValue = "0") Then
                    '    If (SetexFlag = False) Then
                    '        lotNode.Text = "<span class='allOrders'>" + lotNode.Text + "</span>"
                    '    End If
                    'End If


                    bResult = True

                End If
            Else
                tree.Visible = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function ResetProdSched30Labels() As Boolean

        lblW1DateTue.Visible = True
        lblW1TotalTue.Visible = True
        Label2.Visible = True
        lblW1DateWed.Visible = True
        lblW1TotalWed.Visible = True
        Label3.Visible = True
        lblW1DateThu.Visible = True
        lblW1TotalThu.Visible = True
        Label4.Visible = True
        lblW1DateFri.Visible = True
        lblW1TotalFri.Visible = True
        Label5.Visible = True
        lblW1DateSat.Visible = True
        lblW1TotalSat.Visible = True
        Label6.Visible = True
        lblW1DateSun.Visible = True
        lblW1TotalSun.Visible = True
        Label20.Visible = True
        lblW2DateMon.Visible = True
        lblW2TotalMon.Visible = True
        Label22.Visible = True
        lblW2DateTue.Visible = True
        lblW2TotalTue.Visible = True
        Label27.Visible = True
        lblW2DateWed.Visible = True
        lblW2TotalWed.Visible = True
        Label34.Visible = True
        lblW2DateThu.Visible = True
        lblW2TotalThu.Visible = True
        Label35.Visible = True
        lblW2DateFri.Visible = True
        lblW2TotalFri.Visible = True
        Label36.Visible = True
        lblW2DateSat.Visible = True
        lblW2TotalSat.Visible = True
        Label37.Visible = True
        lblW2DateSun.Visible = True
        lblW2TotalSun.Visible = True
        Label38.Visible = True
        lblW3DateMon.Visible = True
        lblW3TotalMon.Visible = True
        Label39.Visible = True
        lblW3DateTue.Visible = True
        lblW3TotalTue.Visible = True
        Label40.Visible = True
        lblW3DateWed.Visible = True
        lblW3TotalWed.Visible = True
        Label41.Visible = True
        lblW3DateThu.Visible = True
        lblW3TotalThu.Visible = True
        Label42.Visible = True
        lblW3DateFri.Visible = True
        lblW3TotalFri.Visible = True
        Label43.Visible = True
        lblW3DateSat.Visible = True
        lblW3TotalSat.Visible = True
        Label44.Visible = True
        lblW3DateSun.Visible = True
        lblW3TotalSun.Visible = True
        Label45.Visible = True
        lblW4DateMon.Visible = True
        lblW4TotalMon.Visible = True
        Label46.Visible = True
        lblW4DateTue.Visible = True
        lblW4TotalTue.Visible = True
        Label47.Visible = True
        lblW4DateWed.Visible = True
        lblW4TotalWed.Visible = True
        Label48.Visible = True
        lblW4DateThu.Visible = True
        lblW4TotalThu.Visible = True
        Label49.Visible = True
        lblW4DateFri.Visible = True
        lblW4TotalFri.Visible = True
        Label50.Visible = True
        lblW4DateSat.Visible = True
        lblW4TotalSat.Visible = True
        Label51.Visible = True
        lblW4DateSun.Visible = True
        lblW4TotalSun.Visible = True
        Label52.Visible = True
        lblW5DateMon.Visible = True
        lblW5TotalMon.Visible = True
        Label83.Visible = True
        lblW5DateTue.Visible = True
        lblW5TotalTue.Visible = True
        Label85.Visible = True

        lblHondaW1DateTue.Visible = True
        lblHondaW1TotalTue.Visible = True
        Label13.Visible = True
        lblHondaW1DateWed.Visible = True
        lblHondaW1TotalWed.Visible = True
        Label15.Visible = True
        lblHondaW1DateThu.Visible = True
        lblHondaW1TotalThu.Visible = True
        Label17.Visible = True
        lblHondaW1DateFri.Visible = True
        lblHondaW1TotalFri.Visible = True
        Label19.Visible = True
        lblHondaW1DateSat.Visible = True
        lblHondaW1TotalSat.Visible = True
        Label59.Visible = True
        lblHondaW1DateSun.Visible = True
        lblHondaW1TotalSun.Visible = True
        Label60.Visible = True
        lblHondaW2DateMon.Visible = True
        lblHondaW2TotalMon.Visible = True
        Label61.Visible = True
        lblHondaW2DateTue.Visible = True
        lblHondaW2TotalTue.Visible = True
        Label62.Visible = True
        lblHondaW2DateWed.Visible = True
        lblHondaW2TotalWed.Visible = True
        Label63.Visible = True
        lblHondaW2DateThu.Visible = True
        lblHondaW2TotalThu.Visible = True
        Label64.Visible = True
        lblHondaW2DateFri.Visible = True
        lblHondaW2TotalFri.Visible = True
        Label65.Visible = True
        lblHondaW2DateSat.Visible = True
        lblHondaW2TotalSat.Visible = True
        Label66.Visible = True
        lblHondaW2DateSun.Visible = True
        lblHondaW2TotalSun.Visible = True
        Label67.Visible = True
        lblHondaW3DateMon.Visible = True
        lblHondaW3TotalMon.Visible = True
        Label68.Visible = True
        lblHondaW3DateTue.Visible = True
        lblHondaW3TotalTue.Visible = True
        Label69.Visible = True
        lblHondaW3DateWed.Visible = True
        lblHondaW3TotalWed.Visible = True
        Label70.Visible = True
        lblHondaW3DateThu.Visible = True
        lblHondaW3TotalThu.Visible = True
        Label71.Visible = True
        lblHondaW3DateFri.Visible = True
        lblHondaW3TotalFri.Visible = True
        Label72.Visible = True
        lblHondaW3DateSat.Visible = True
        lblHondaW3TotalSat.Visible = True
        Label73.Visible = True
        lblHondaW3DateSun.Visible = True
        lblHondaW3TotalSun.Visible = True
        Label74.Visible = True
        lblHondaW4DateMon.Visible = True
        lblHondaW4TotalMon.Visible = True
        Label75.Visible = True
        lblHondaW4DateTue.Visible = True
        lblHondaW4TotalTue.Visible = True
        Label76.Visible = True
        lblHondaW4DateWed.Visible = True
        lblHondaW4TotalWed.Visible = True
        Label77.Visible = True
        lblHondaW4DateThu.Visible = True
        lblHondaW4TotalThu.Visible = True
        Label78.Visible = True
        lblHondaW4DateFri.Visible = True
        lblHondaW4TotalFri.Visible = True
        Label79.Visible = True
        lblHondaW4DateSat.Visible = True
        lblHondaW4TotalSat.Visible = True
        Label80.Visible = True
        lblHondaW4DateSun.Visible = True
        lblHondaW4TotalSun.Visible = True
        Label81.Visible = True
        lblHondaW5DateMon.Visible = True
        lblHondaW5TotalMon.Visible = True
        Label84.Visible = True
        lblHondaW5DateTue.Visible = True
        lblHondaW5TotalTue.Visible = True
        Label87.Visible = True

        Me.lblW1Total.Visible = True
        Me.lblW2Total.Visible = True
        Me.lblW3Total.Visible = True
        Me.lblW4Total.Visible = True
        Me.lblW5Total.Visible = True
        Me.lblW1.Visible = True
        Me.lblW2.Visible = True
        Me.lblW3.Visible = True
        Me.lblw4.Visible = True
        Me.lblW5.Visible = True

        Me.lblW1Total.Text = ""
        Me.lblW2Total.Text = ""
        Me.lblW3Total.Text = ""
        Me.lblW4Total.Text = ""
        Me.lblW5Total.Text = ""


    End Function

    Private Function LoadWeekTotals(ByRef dt As DataTable) As Boolean
        Dim bResult As Boolean = False

        Try
            If (dt IsNot Nothing) Then
                If (dt.Rows.Count > 0) Then
                    If (dt.Columns.Count >= 27) Then

                        Dim grandTotal As Integer = 0
                        Dim dayTotal As Integer = 0

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day1Total).ToString())
                        'grandTotal = grandTotal + dayTotal
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW1TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW1TotalTue.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day3Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day3Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day3Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW1TotalWed.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day4Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day4Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day4Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW1TotalThu.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day5Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day5Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day5Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW1TotalFri.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day6Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day6Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day6Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW1TotalSat.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day7Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day7Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day7Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW1TotalSun.Text = dayTotal.ToString()

                        Me.lblW1Total.Text = grandTotal.ToString()
                        grandTotal = 0

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW2TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW2TotalTue.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day3Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day3Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day3Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW2TotalWed.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day4Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day4Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day4Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW2TotalThu.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day5Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day5Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day5Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW2TotalFri.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day6Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day6Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day6Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW2TotalSat.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day7Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day7Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day7Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW2TotalSun.Text = dayTotal.ToString()

                        Me.lblW2Total.Text = grandTotal.ToString()
                        grandTotal = 0

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW3TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW3TotalTue.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day3Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day3Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day3Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW3TotalWed.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day4Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day4Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day4Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW3TotalThu.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day5Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day5Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day5Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW3TotalFri.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day6Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day6Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day6Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW3TotalSat.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day7Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day7Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day7Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW3TotalSun.Text = dayTotal.ToString()

                        Me.lblW3Total.Text = grandTotal.ToString()
                        grandTotal = 0

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW4TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW4TotalTue.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day3Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day3Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day3Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW4TotalWed.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day4Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day4Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day4Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW4TotalThu.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day5Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day5Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day5Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW4TotalFri.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day6Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day6Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day6Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW4TotalSat.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day7Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day7Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day7Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW4TotalSun.Text = dayTotal.ToString()

                        Me.lblW4Total.Text = grandTotal.ToString()
                        grandTotal = 0

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W5Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W5Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W5Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW5TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W5Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W5Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W5Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblW5TotalTue.Text = dayTotal.ToString()

                        Me.lblW5Total.Text = grandTotal.ToString()


                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function


    Private Function LoadHondaWeekTotals(ByRef dt As DataTable) As Boolean
        Dim bResult As Boolean = False

        Try
            If (dt IsNot Nothing) Then
                If (dt.Rows.Count > 0) Then
                    If (dt.Columns.Count >= 14) Then

                        Dim grandTotal As Integer = 0
                        Dim dayTotal As Integer = 0

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW1TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW1TotalTue.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day3Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day3Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day3Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW1TotalWed.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day4Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day4Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day4Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW1TotalThu.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day5Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day5Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day5Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW1TotalFri.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day6Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day6Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day6Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW1TotalSat.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W1Day7Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W1Day7Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W1Day7Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW1TotalSun.Text = dayTotal.ToString()
                        dayTotal = 0
                        'Me.lblHondaW1Total.Text = grandTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW2TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW2TotalTue.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day3Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day3Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day3Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW2TotalWed.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day4Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day4Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day4Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW2TotalThu.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day5Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day5Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day5Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW2TotalFri.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day6Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day6Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day6Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW2TotalSat.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W2Day7Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W2Day7Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W2Day7Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW2TotalSun.Text = dayTotal.ToString()
                        dayTotal = 0
                        'Me.lblHondaW2Total.Text = grandTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW3TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW3TotalTue.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day3Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day3Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day3Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW3TotalWed.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day4Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day4Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day4Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW3TotalThu.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day5Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day5Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day5Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW3TotalFri.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day6Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day6Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day6Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW3TotalSat.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W3Day7Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W3Day7Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W3Day7Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW3TotalSun.Text = dayTotal.ToString()
                        dayTotal = 0
                        'Me.lblHondaW3Total.Text = grandTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW4TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW4TotalTue.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day3Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day3Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day3Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW4TotalWed.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day4Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day4Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day4Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW4TotalThu.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day5Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day5Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day5Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW4TotalFri.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day6Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day6Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day6Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW4TotalSat.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W4Day7Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W4Day7Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W4Day7Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW4TotalSun.Text = dayTotal.ToString()
                        dayTotal = 0
                        'Me.lblHondaW4Total.Text = grandTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W5Day1Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W5Day1Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W5Day1Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW5TotalMon.Text = dayTotal.ToString()

                        'dayTotal = Convert.ToInt32(dt.Rows(0)(Totals.W5Day2Total).ToString())
                        dayTotal = If(Len(dt.Rows(0)(Totals.W5Day2Total).ToString()) > 0, Convert.ToInt32(dt.Rows(0)(Totals.W5Day2Total).ToString()), 0)
                        grandTotal += dayTotal
                        Me.lblHondaW5TotalTue.Text = dayTotal.ToString()

                        'Me.lblHondaW5Total.Text = grandTotal.ToString()


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
            If hidIsSetexOrder.Value = "1" Then 'setex orders
                SelectLastSelectedNodeInTree(treeW1Mon)
                SelectLastSelectedNodeInTree(treeW1Tue)
                SelectLastSelectedNodeInTree(treeW1Wed)
                SelectLastSelectedNodeInTree(treeW1Thu)
                SelectLastSelectedNodeInTree(treeW1Fri)
                SelectLastSelectedNodeInTree(treeW1Sat)
                SelectLastSelectedNodeInTree(treeW1Sun)

                SelectLastSelectedNodeInTree(treeW2Mon)
                SelectLastSelectedNodeInTree(treeW2Tue)
                SelectLastSelectedNodeInTree(treeW2Wed)
                SelectLastSelectedNodeInTree(treeW2Thu)
                SelectLastSelectedNodeInTree(treeW2Fri)
                SelectLastSelectedNodeInTree(treeW2Sat)
                SelectLastSelectedNodeInTree(treeW2Sun)

                SelectLastSelectedNodeInTree(treeW3Mon)
                SelectLastSelectedNodeInTree(treeW3Tue)
                SelectLastSelectedNodeInTree(treeW3Wed)
                SelectLastSelectedNodeInTree(treeW3Thu)
                SelectLastSelectedNodeInTree(treeW3Fri)
                SelectLastSelectedNodeInTree(treeW3Sat)
                SelectLastSelectedNodeInTree(treeW3Sun)

                SelectLastSelectedNodeInTree(treeW4Mon)
                SelectLastSelectedNodeInTree(treeW4Tue)
                SelectLastSelectedNodeInTree(treeW4Wed)
                SelectLastSelectedNodeInTree(treeW4Thu)
                SelectLastSelectedNodeInTree(treeW4Fri)
                SelectLastSelectedNodeInTree(treeW4Sat)
                SelectLastSelectedNodeInTree(treeW4Sun)

                SelectLastSelectedNodeInTree(treeW5Mon)
                SelectLastSelectedNodeInTree(treeW5Tue)
            Else
                SelectLastSelectedNodeInTree(treeDay)
            End If
            'pah      SelectLastSelectedNodeInTree(treeMove)

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
                'PlaceLotOnOrOffHold(psHoldType, node.Text)  
                PlaceLotOnOrOffHold(psHoldType, hidNodeSDT.Value)
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
            If (hidLTP.Value.Length = 0) Then
                Master.Msg = "Please Select a Lot Number to Resequence."
            Else
                LotNumber = hidLTP.Value.Trim().Replace("-", "").Substring(0, 8)

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
                oSqlParameter.Value = "ProductionSchedule30: " & Session("UserFirstLastName").ToString()
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

                LogTrc("New Lot", Page, "ProductionSchedule30.  @LotNumber :: " + Me.txtNewLotNum.Text.Replace("-", "") + " @BeforeAfter :: " + beforeAfter + " @AdjacentLot :: " + Me.hidNewLotNew.Value.Replace("-", "") +
                       " @SubLotIndex :: " + Me.txtNewSLIndex.Text + " @JobQuantity :: " + Me.txtNewQuantity.Text + " @ProductID :: " + hidNewProdID.Value + " @RecordedBy :: " + "ProductionSchedule30: " & Session("UserFirstLastName").ToString() +
                      " @Nvalue :: " + hidNewNValues.Value + " @BroadcastPointID :: " + Me.txtNewLotNum.Text.Substring(0, 1) + " @Status :: " + status + " @ErrorMsg :: " + message)

                If (status.ToUpper() = "TRUE") Then
                    Master.tMsg("New", "New Sub-Lot for Lot Number: " + txtNewLotNum.Text + " has been created.")
                    ScriptManager.RegisterStartupScript(cmdNew, cmdNew.GetType(), "cmdNew", "DoRefreshPostBack();", True)
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
                lot = hidNodeSDT.Value.ToString().Trim().Substring(0, 10)

                If (selectedNodeLevel = 1) Then

                    DeleteLot(lot, status, message)

                    If (status.ToUpper() = "TRUE") Then
                        Master.tMsg("Delete", "Deleted Lot Number: " + lot)
                    Else
                        Master.tMsg("Delete", "Error: Lot Number: " + lot + " was not deleted. <br>Error message: " + message + "<BR>Status message: " + status)
                    End If

                ElseIf (selectedNodeLevel = psNodeType.SUBLOT) Then

                    DeleteSubLot(hidNodeSDT.Value, status, message)

                    If (status.ToUpper() = "TRUE") Then
                        Master.tMsg("Delete", "Deleted Sub-Lot Number: " + hidNodeLot.Value + "; Sequence DT: " + hidNodeSeq.Value.Substring(0, 13))
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
            Dim seqDT As String = "" 'Me.hidNodeSeqDT.Value
            Dim seqNum As String = Me.hidNodeLot.Value

            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As New List(Of SqlParameter)

            seqNum = seqNum.Replace("-"c, "").Substring(0, 10)

            'oSqlParameter = New SqlParameter("@SequenceDT", SqlDbType.DateTime)
            oSqlParameter = New SqlParameter("@SequenceNum", SqlDbType.VarChar, 80)
            'oSqlParameter.Value = seqDT
            oSqlParameter.Value = seqNum
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



    Private Sub MoveLot()

        Dim lotNum As String
        Dim location As Integer
        Dim multilot As String()
        Dim x As Integer

        Try
            lotNum = hidLTP.Value.Trim().ToString().Replace("-", "")
            location = lotNum.LastIndexOf(":")
            If location <> -1 Then
                lotNum = lotNum.Substring(0, 8)
            End If

            multilot = lotNum.Split(" "c)

            If multilot.Length() = 1 Then
                DoLotMove(multilot(0))
            ElseIf multilot.Length() = 2 Then
                If hidSetexOrder.Value = "1" Then 'SETEX
                    If Me.rblBeforeAfter.SelectedValue = "AFTER" Then
                        For x = 0 To 1
                            DoLotMove(multilot(x))
                        Next
                    ElseIf Me.rblBeforeAfter.SelectedValue = "BEFORE" Then
                        For x = 1 To 0 Step -1
                            DoLotMove(multilot(x))
                        Next
                    Else
                        'Do nothing
                    End If
                ElseIf hidSetexOrder.Value = "0" Then 'Honda
                    lotNum = lotNum.Replace(" ", "|")
                    DoLotMove(lotNum)
                Else
                    'Do nothing
                End If

            Else
                'Do nothing
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try



        'Dim lotNum As String
        ''Dim IsASetexOrder As String = ""
        'Dim location As Integer
        'Dim multilot As String()
        'Dim x As Integer

        'Try

        '    'IsASetexOrder = hidSetexOrder.Value
        '    lotNum = hidLTP.Value.Trim().ToString().Replace("-", "")

        '    location = lotNum.LastIndexOf(" ")
        '    If location <> -1 Then
        '        lotNum = lotNum.Remove(location, 1)
        '    End If

        '    location = lotNum.LastIndexOf("|")
        '    If location <> -1 Then
        '        lotNum = lotNum.Remove(location, 1)
        '    End If

        '    'If hidSetexOrder.Value = "0" And lotNum.Length > 0 And lotNum.Length < 9 Then
        '    multilot = lotNum.Split("|"c)
        '    'Else
        '    '    ReDim multilot(0)
        '    '    multilot(0) = lotNum
        '    'End If

        '    If multilot.Length() = 1 Then
        '        DoLotMove(multilot(0))
        '    ElseIf multilot.Length() = 2 Then
        '        If hidSetexOrder.Value = "1" Then
        '            If Me.rblBeforeAfter.SelectedValue = "AFTER" Then
        '                For x = 0 To 1
        '                    DoLotMove(multilot(x))
        '                Next
        '            ElseIf Me.rblBeforeAfter.SelectedValue = "BEFORE" Then
        '                For x = 1 To 0 Step -1
        '                    DoLotMove(multilot(x))
        '                Next
        '            Else
        '                'Do nothing
        '            End If
        '        ElseIf hidSetexOrder.Value = "0" Then 'Honda
        '            If Me.rblBeforeAfter.SelectedValue = "AFTER" Then
        '                For x = 0 To 1
        '                    DoLotMove(multilot(x))
        '                Next
        '            ElseIf Me.rblBeforeAfter.SelectedValue = "BEFORE" Then
        '                For x = 0 To 1
        '                    DoLotMove(multilot(x))
        '                Next
        '            Else
        '                'Do nothing
        '            End If
        '        Else
        '            'Do nothing
        '        End If

        '    Else
        '        'Do nothing
        '    End If



        'Catch ex As Exception
        '    Master.eMsg(ex.ToString())
        'End Try
    End Sub

    Private Sub DoLotMove(lotnum As String)

        Dim status As String = ""
        Dim message As String = ""
        Dim retVal As String = ""
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim strProcToBeRun As String = ""

        Try

            oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 80)
            oSqlParameter.Value = lotNum
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BeforeAfter", SqlDbType.VarChar, 6)
            oSqlParameter.Value = Me.rblBeforeAfter.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@AdjacentLot", SqlDbType.VarChar, 80)
            oSqlParameter.Value = Me.hidLotNum.Value.Replace("-", "")
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ProdOrShipType", SqlDbType.VarChar, 80)
            oSqlParameter.Value = "PROD"
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@CurrentBroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = lotNum.Substring(0, 1)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@NewBroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = lotNum.Substring(0, 1)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)


            If (hidSetexOrder.Value = "1") Then '  this is a setex order
                'call the procedure to move in the production schedule and optionally shipping schedule
                strProcToBeRun = "Setex Only orders, procPSMoveLotProdSched::"  'run twice in case of paired moves
                colOutput = DA.ExecSP("procPSMoveLotProdSched", colParameters)
            Else
                'move the lot in the Honda Schedule, in other words move all 3 indexes, prod, honda, ship.
                ' If lotnum.Length < 9 Then
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
                Master.tMsg("Move", strProcToBeRun + " Lot Number: " + lotnum + " has been moved " + Me.rblBeforeAfter.SelectedValue + " " + Me.hidLotNum.Value.Replace("-", ""))
            Else
                Master.tMsg("Move", "Error - Lot Number: " + lotnum + " has NOT been moved. <BR>Error message: " + message + "<BR>Status message: " + status)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub



    'Private Function MoveLot(srcLotNum As String, beforeAfter As String, destLotNum As String) As String
    '    Dim retVal As String = ""
    '    Dim status As String = ""
    '    Dim message As String = ""
    '    Dim lotNum As String = ""
    '    Dim oSqlParameter As SqlParameter
    '    Dim colParameters As New List(Of SqlParameter)
    '    Dim colOutput As List(Of SqlParameter)
    '    Dim strBroadcastPoint As String
    '    Dim strMovedInShipping As String = ""
    '    Dim strProcToBeRun As String = ""
    '    Dim strBeforeAfter As String = ""
    '    Dim strAdjacentLot As String = ""

    '    Try
    '        strAdjacentLot = destLotNum.Replace("-", "").Substring(0, 8)

    '        If String.Equals(beforeAfter, "Above", StringComparison.OrdinalIgnoreCase) Then
    '            strBeforeAfter = "BEFORE"
    '        ElseIf String.Equals(beforeAfter, "Below", StringComparison.OrdinalIgnoreCase) Then
    '            strBeforeAfter = "AFTER"
    '        End If

    '        If (rbOrders.SelectedValue = "0") Then
    '            lotNum = srcLotNum.Trim().Replace("-", "")
    '        Else
    '            lotNum = srcLotNum.Trim().Replace("-", "").Substring(0, 8)
    '        End If
    '        strBroadcastPoint = lotNum.Substring(0, 1)

    '        oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 80)
    '        oSqlParameter.Value = lotNum
    '        colParameters.Add(oSqlParameter)

    '        oSqlParameter = New SqlParameter("@BeforeAfter", SqlDbType.VarChar, 6)
    '        oSqlParameter.Value = strBeforeAfter
    '        colParameters.Add(oSqlParameter)

    '        oSqlParameter = New SqlParameter("@AdjacentLot", SqlDbType.VarChar, 80)
    '        oSqlParameter.Value = strAdjacentLot
    '        colParameters.Add(oSqlParameter)

    '        oSqlParameter = New SqlParameter("@ProdOrShipType", SqlDbType.VarChar, 80)
    '        oSqlParameter.Value = "PROD"
    '        colParameters.Add(oSqlParameter)

    '        If (rbOrders.SelectedValue = "1") Then
    '            'add parameter for whether to move in shipping index or not
    '            'oSqlParameter = New SqlParameter("@MoveShipIndex", SqlDbType.Bit)
    '            'oSqlParameter.Value = "True"
    '            'colParameters.Add(oSqlParameter)
    '            'strMovedInShipping = " Also Moved in Shipping Schedule"
    '        End If

    '        'If Me.cbMoveShipIndex.Checked Then

    '        'End If

    '        oSqlParameter = New SqlParameter("@CurrentBroadcastPointID", SqlDbType.VarChar, 4)
    '        oSqlParameter.Value = strBroadcastPoint
    '        colParameters.Add(oSqlParameter)

    '        oSqlParameter = New SqlParameter("@NewBroadcastPointID", SqlDbType.VarChar, 4)
    '        oSqlParameter.Value = strBroadcastPoint
    '        colParameters.Add(oSqlParameter)

    '        oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
    '        oSqlParameter.Direction = ParameterDirection.Output
    '        colParameters.Add(oSqlParameter)

    '        oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
    '        oSqlParameter.Direction = ParameterDirection.Output
    '        colParameters.Add(oSqlParameter)

    '        oSqlParameter = New SqlParameter("@ProdSchedIndexIncrement", 0)
    '        oSqlParameter.Direction = ParameterDirection.Output
    '        colParameters.Add(oSqlParameter)


    '        If (rbOrders.SelectedValue = "1") Then '  this is a setex order
    '            'call the procedure to move in the production schedule and optionally shipping schedule
    '            strProcToBeRun = "Setex Only orders, procPSMoveLotProdSched::"  'run twice in case of paired moves
    '            colOutput = DA.ExecSP("procPSMoveLotProdSched", colParameters)
    '        Else
    '            'move the lot in the Honda Schedule, in other words move all 3 indexes, prod, honda, ship.
    '            strProcToBeRun = "ALL Orders,  procPSMoveLotMulti::"
    '            colOutput = DA.ExecSP("procPSMoveLotMulti", colParameters)
    '        End If

    '        For Each oParameter In colOutput
    '            With oParameter
    '                If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
    '                    status = oParameter.Value.ToString()
    '                ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
    '                    message = oParameter.Value.ToString()
    '                End If
    '            End With
    '        Next

    '        If (status.ToUpper() = "TRUE") Then
    '            Master.tMsg("Move", strProcToBeRun + " Lot Number: " + lotNum + " has been moved " + beforeAfter + " " + strAdjacentLot + strMovedInShipping)
    '        Else
    '            Master.tMsg("Move", "Error - Lot Number: " + lotNum + " has NOT been moved. <BR>Error message: " + message + "<BR>Status message: " + status)
    '        End If

    '    Catch ex As Exception
    '        Master.eMsg(ex.ToString())
    '    End Try

    '    Return retVal

    'End Function
    Private Sub MoveSubLot(srcLotNum As String, beforeAfter As String, destLotNum As String)
        Try
            Dim status As String = ""
            Dim message As String = ""
            Dim seqDT As String = Me.hidNodeSeq.Value
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            'Dim dt As DateTime
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

            If (hidIsSetexOrder.Value = "1") Then
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
            treeW1Mon.UncheckAllNodes()
            treeW1Tue.UncheckAllNodes()
            treeW1Wed.UncheckAllNodes()
            treeW1Thu.UncheckAllNodes()
            treeW1Fri.UncheckAllNodes()
            treeW1Sat.UncheckAllNodes()
            treeW1Sun.UncheckAllNodes()

            treeW2Mon.UncheckAllNodes()
            treeW2Tue.UncheckAllNodes()
            treeW2Wed.UncheckAllNodes()
            treeW2Thu.UncheckAllNodes()
            treeW2Fri.UncheckAllNodes()
            treeW2Sat.UncheckAllNodes()
            treeW2Sun.UncheckAllNodes()

            treeW3Mon.UncheckAllNodes()
            treeW3Tue.UncheckAllNodes()
            treeW3Wed.UncheckAllNodes()
            treeW3Thu.UncheckAllNodes()
            treeW3Fri.UncheckAllNodes()
            treeW3Sat.UncheckAllNodes()
            treeW3Sun.UncheckAllNodes()

            treeW4Mon.UncheckAllNodes()
            treeW4Tue.UncheckAllNodes()
            treeW4Wed.UncheckAllNodes()
            treeW4Thu.UncheckAllNodes()
            treeW4Fri.UncheckAllNodes()
            treeW4Sat.UncheckAllNodes()
            treeW4Sun.UncheckAllNodes()

            treeW5Mon.UncheckAllNodes()
            treeW5Tue.UncheckAllNodes()

            treeDay.UncheckAllNodes()
            'pah         treeMove.UncheckAllNodes()
            ClearHiddenNodeValues()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub MoveLots_Rebind()
        Try
            UnCheckAllNodes()
            ddlLot.DataBind()
            ' LoadWeek()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region

#Region "Revision information"

    Private Function LoadRevision(week As Integer, trackID As String) As Boolean
        Dim bResult As Boolean = False
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim strRevisionType As String
        Dim AllOrdersFlag As Boolean = False

        Try
            Dim dMonday As Date = Utility.GetMondayOfDate(Me.tbDay.Text)
            Dim strMonday As String = dMonday.ToString("MM/dd/yyyy")

            'Me.lblDate.Text = strMonday

            'check whether Setex Orders or All Orders Selected and assign Revision Type accordingly
            If (hidIsSetexOrder.Value = "1") Then
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
                If week = 1 Then
                    Me.lblRevNoW1.Text = "1"
                    hidrbNextRevW1.Value = "2"
                ElseIf week = 2 Then
                    Me.lblRevNoW2.Text = "1"
                    hidrbNextRevW2.Value = "2"
                ElseIf week = 3 Then
                    Me.lblRevNoW3.Text = "1"
                    hidrbNextRevW3.Value = "2"
                ElseIf week = 4 Then
                    Me.lblRevNoW4.Text = "1"
                    hidrbNextRevW4.Value = "2"
                End If
                bResult = False
            Else
                If week = 1 Then
                    Me.lblRevNoW1.Text = ds.Tables(0).DefaultView.Table.Rows(0)("RevisionNumber").ToString()
                    Me.lblRevCommentsW1.Text = ds.Tables(0).DefaultView.Table.Rows(0)("Comment").ToString()
                    hidrbNextRevW1.Value = ds.Tables(0).DefaultView.Table.Rows(0)("NextRevisionNumber").ToString()
                    bResult = True
                ElseIf week = 2 Then
                    Me.lblRevNoW2.Text = ds.Tables(0).DefaultView.Table.Rows(0)("RevisionNumber").ToString()
                    Me.lblRevCommentsW2.Text = ds.Tables(0).DefaultView.Table.Rows(0)("Comment").ToString()
                    hidrbNextRevW2.Value = ds.Tables(0).DefaultView.Table.Rows(0)("NextRevisionNumber").ToString()
                    bResult = True
                ElseIf week = 3 Then
                    Me.lblRevNoW3.Text = ds.Tables(0).DefaultView.Table.Rows(0)("RevisionNumber").ToString()
                    Me.lblRevCommentsW3.Text = ds.Tables(0).DefaultView.Table.Rows(0)("Comment").ToString()
                    hidrbNextRevW3.Value = ds.Tables(0).DefaultView.Table.Rows(0)("NextRevisionNumber").ToString()
                    bResult = True
                ElseIf week = 4 Then
                    Me.lblRevNoW4.Text = ds.Tables(0).DefaultView.Table.Rows(0)("RevisionNumber").ToString()
                    Me.lblRevCommentsW4.Text = ds.Tables(0).DefaultView.Table.Rows(0)("Comment").ToString()
                    hidrbNextRevW4.Value = ds.Tables(0).DefaultView.Table.Rows(0)("NextRevisionNumber").ToString()
                    bResult = True
                End If
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
            Dim adjustedTrackingDate As DateTime
            Dim selweek As Integer = 0

            selweek = CInt(Me.rblWhichWeek.SelectedValue)


            Dim dMonday As Date = Utility.GetMondayOfDate(Me.tbDay.Text)
            adjustedTrackingDate = dMonday

            Select Case selweek
                Case 1
                    'adjustedTrackingDate
                Case 2
                    adjustedTrackingDate = adjustedTrackingDate.AddDays(7)
                Case 3
                    adjustedTrackingDate = adjustedTrackingDate.AddDays(14)
                Case 4
                    adjustedTrackingDate = adjustedTrackingDate.AddDays(21)
            End Select

            'check whether Setex Orders or All Orders Selected and assign Revision Type accordingly
            If (hidIsSetexOrder.Value = "1") Then
                strRevisionType = "0001" 'for Setex Orders
            Else
                strRevisionType = "0004" 'for all orders
            End If

            oSqlParameter = New SqlParameter("@RevisionTypeID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = strRevisionType
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@RevisionNumber", SqlDbType.Int)
            oSqlParameter.Value = Me.hidNextRev.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Comment", SqlDbType.VarChar, 500)
            oSqlParameter.Value = (Me.txtComment.Text)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@TrackingID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = adjustedTrackingDate.ToString("MM/dd/yyyy")
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
                Master.tMsg("Revision", "Error: unable to update Revision for week starting on " + lblW1DateMon.Text + ".<br> S.P. Status: " + status + ".<br>S.P. Message: " + message)
            Else
                Master.tMsg("Revision", "Revision for week starting on " + lblW1DateMon.Text + " has been updated.")
            End If

            'LoadRevision(Me.lblDate.Text)

            adjustedTrackingDate = dMonday
            'adjustedTrackingDate = CDate(Me.lblDate.Text)
            LoadRevision(1, adjustedTrackingDate.ToString("MM/dd/yyyy"))

            adjustedTrackingDate = adjustedTrackingDate.AddDays(7)
            LoadRevision(2, adjustedTrackingDate.ToString("MM/dd/yyyy"))

            adjustedTrackingDate = adjustedTrackingDate.AddDays(7)
            LoadRevision(3, adjustedTrackingDate.ToString("MM/dd/yyyy"))

            adjustedTrackingDate = adjustedTrackingDate.AddDays(7)
            LoadRevision(4, adjustedTrackingDate.ToString("MM/dd/yyyy"))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

#End Region

    Private Sub cmdOnHold_Click(sender As Object, e As System.EventArgs) Handles cmdOnHold.Click
        Dim lot As String = ""
        lot = hidNodeSDT.Value.ToString().Trim().Substring(0, 10)
        PlaceLotOnOrOffHold(psHoldType.OnHold, lot)
        LoadProdData()

    End Sub

    Private Sub cmdOffHold_Click(sender As Object, e As System.EventArgs) Handles cmdOffHold.Click
        Dim lot As String = ""
        lot = hidNodeSDT.Value.ToString().Trim().Substring(0, 10)
        PlaceLotOnOrOffHold(psHoldType.OffHold, lot)
        LoadProdData()

    End Sub


    Private Sub RefreshScreen()
        ScriptManager.RegisterStartupScript(cmdRefresh, cmdRefresh.GetType(), "cmdRefresh", "RefreshAfterMove();", True)
    End Sub

End Class