Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class ShippingSchedule
    Inherits System.Web.UI.Page


#Region "local vars and constants"

    Const LOT_NUM_DELIMITER As String = ","
    Const REVISION_TYPE_ID As String = "0002"
    Const ON_HOLD As String = "<span class='boldShipTime'>ON HOLD</span>"

    Enum procGetBroadcastPoints
        BroadcastPointID
        Description
        ImageName
        DefaultDailyBuildQuantityShip
        DefaultDailyBuildQuantityJob
        defaultSelection
    End Enum

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
        BroadcastPointID
        ImageName
    End Enum

    Enum procPSGetShipLotDetailColumnType
        LotNumber8
        LotNumber
        ShippingNotes
        ShipTime
        ArrivalTime
        ApproxSeqTime
        TrailerNumber
        Driver
        ActualDepartureTime
    End Enum

#End Region

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                Me.tbDay.Text = Now().ToString("MM/dd/yyyy")
                lblSelectBroadcastPointID.Text = My.Resources.txtBroadcastPointCaption + ":"
                ShowLotData(False)

                DataAccess.BroadcastPointID.Load_ddlBroadcastPointID(Me.ddlBroadcastPointID, Request, Server)

                Load_ddlDriver()
                Load_ddlTemplates()
                LoadDay()

            End If

            GetBroadcastPointKey()
            'Me.lblRevNo.Text = Me.hidRevNumber.Value
            'Me.lblRevComments.Text = Me.hidComments.Value
            'Me.lblRev.Text = Me.hidNextRevNumber.Value

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ShippingSchedule_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub



    Private Sub ddlBroadcastPointID_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlBroadcastPointID.SelectedIndexChanged
        Load_ddlTemplates()
        RefreshShippingSchedule()
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        RefreshShippingSchedule()
    End Sub

    Private Sub cbHold_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbHold.CheckedChanged
        Try
            LoadDay()
            SelectLastSelectedNodeInTree(treeShip)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            SaveLotData()
            LoadDay()
            SelectLastSelectedNodeInTree(treeShip)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdMove_Click(sender As Object, e As System.EventArgs) Handles cmdMove.Click
        Try
            MoveLot()
            LoadDay()
            'SelectLastSelectedNodeInTree(treeShip)
            'LoadDay()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdTemplate_Click(sender As Object, e As System.EventArgs) Handles cmdTemplate.Click
        Dim templateText As String
        Dim status As String = ""
        Dim message As String = ""

        Try
            templateText = ddlTemplates.SelectedItem.Text
            AssignTemplate(ddlTemplates.SelectedValue, status, message)

            If (status.ToUpper() <> "TRUE") Then
                Master.Msg = "Error: unable to Assign Template " + templateText + ".<br> S.P. Status: " + status + ".<br>S.P. Message: " + message
            Else
                Master.tMsg("Assign Template", "Template " + templateText + " has been assigned to lots from " + Me.tbDay.Text)
                LoadDay()
                SelectLastSelectedNodeInTree(treeShip)
                ScriptManager.RegisterStartupScript(cmdTemplate, cmdTemplate.GetType(), "cmdTemplateRefresh", "RefreshAfterTemplate();", True)
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdRevInc_Click(sender As Object, e As System.EventArgs) Handles cmdRevInc.Click
        Try
            If (txtComment.Text.Length > 0) Then
                InsertRevision()
            Else
                Master.eMsg("Please Enter a Comment.")
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibNext_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibNext.Click
        Try
            Dim dt As Date
            If Date.TryParse(tbDay.Text, dt) Then
                tbDay.Text = dt.AddDays(1).ToString("MM/dd/yyyy")
                LoadDay()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibPrev_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibPrev.Click
        Try
            Dim dt As Date
            If Date.TryParse(tbDay.Text, dt) Then
                tbDay.Text = dt.AddDays(-1).ToString("MM/dd/yyyy")
                LoadDay()
            End If
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

    Protected Sub RadTreeView_NodeClick(sender As Object, e As Telerik.Web.UI.RadTreeNodeEventArgs)
        Try
            If (treeShip.SelectedNode IsNot Nothing) Then
                hidNodeSeqDT.Value = treeShip.SelectedNode.Value
                hidLotNum.Value = Left(treeShip.SelectedNode.Text.Replace("<span class='allOrders'>", "").Replace("</span>", ""), 10)
                hidNodeLot.Value = hidLotNum.Value

                ShowLotData(LoadLotData())

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub RadTreeView_ContextMenuItemClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeViewContextMenuEventArgs)
        Try
            Select Case e.MenuItem.Text
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

#Region "Methods"

    Private Sub EnableControls()
        Try
            Master.Secure(cmdRefresh)

            Master.Secure(cmdSave)
            Master.Secure(cmdMove)
            Master.Secure(cmdPrint)
            Master.Secure(cmdTemplate)
            Master.Secure(cmdRevInc)

            lblDate.Text = Me.tbDay.Text
            lblDeliveryDate.Text = Me.tbDay.Text

            If (Me.ddlBroadcastPointID.SelectedIndex <= 0) Then
                Me.cmdTemplate.Enabled = False
                Me.cmdRevInc.Enabled = False
            End If

            If (Me.treeShip.Nodes.Count <= 0) Then
                Me.cmdPrint.Enabled = False
            End If

            If (treeShip.SelectedNode Is Nothing) Then
                cmdSave.Enabled = False
                cmdMove.Enabled = False
                ShowLotData(False)
            Else

                Select Case treeShip.SelectedNode.Level
                    Case 0  ' top level -date
                        cmdMove.Enabled = False
                        cmdSave.Enabled = False

                        'Case 1  ' lot
                        'cmdMove.Enabled = False
                        'cmdSave.Enabled = False

                    Case 2  ' sublot
                        cmdMove.Enabled = False

                End Select
            End If

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

    Private Sub Load_ddlDriver()
        Try
            Dim sql As String = "SELECT ParameterListValue AS [TEXT], ParameterListValue AS [VALUE] FROM tblParameterListValues WHERE (ParameterListID = '51') ORDER BY DISPLAYID"
            Dim ds As DataSet

            'WebControlFunctions.DDL_DataBind(Me.ddlDriver, sql, "VALUE", "TEXT")

            Me.ddlDriver.Items.Clear()

            ds = DA.GetDataSet(sql)
            If (DA.IsDSEmpty(ds)) Then
                Me.ddlDriver.Items.Insert(0, "")
                Return
            End If

            Me.ddlDriver.DataSource = ds
            Me.ddlDriver.DataTextField = "TEXT"
            Me.ddlDriver.DataValueField = "VALUE"
            Me.ddlDriver.DataBind()
            Me.ddlDriver.Items.Insert(0, "")

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub Load_ddlTemplates()
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        Try
            Me.ddlTemplates.Items.Clear()

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
            colParameters.Add(oSqlParameter)

            ds = DA.GetDataSet("procGetDeliveryTemplates", colParameters)

            If (DA.IsDataSetNotEmpty(ds)) Then
                Me.ddlTemplates.DataSource = ds
                Me.ddlTemplates.DataTextField = "Description"
                Me.ddlTemplates.DataValueField = "DeliveryTemplateID"
                Me.ddlTemplates.DataBind()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Sub ShowLotData(ByVal show As Boolean)
        Me.pnlLotData.Visible = show
    End Sub

    Private Function GetDayID(formattedDate As String) As Integer
        Dim dayofweek As Integer = CDate(formattedDate).DayOfWeek()
        GetDayID = dayofweek - 1
    End Function

    Private Function LoadDay() As Boolean
        Dim bResult As Boolean = False
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim ds As DataSet
        Dim hold As Integer = 1
        Dim strDt As String = ""
        Dim numDays As Integer = 7
        Dim RevisedDt As Date
        Dim intDayID As Integer

        Try
            strDt = Utility.FormattedDate(Me.tbDay.Text)
            'intDayID = GetDayID(strDt)
            'RevisedDt = Utility.GetMondayOfDate(strDt).Date()
            'strDt = RevisedDt.ToString("MM/dd/yyyy")

            If (strDt.Length <= 0) Then
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

                oSqlParameter = New SqlParameter("@vchBegDT", SqlDbType.VarChar, 30)
                oSqlParameter.Value = strDt
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@DayofWeekID", SqlDbType.Int)       ''''
                oSqlParameter.Value = intDayID
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
                oSqlParameter.Value = hold
                colParameters.Add(oSqlParameter)

                'oSqlParameter = New SqlParameter("@SetexOrderFlag", SqlDbType.Bit)
                'oSqlParameter.Value = 1
                'colParameters.Add(oSqlParameter)


                oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
                oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
                colParameters.Add(oSqlParameter)

                ds = DA.GetDataSet("procPSGetShipSched", colParameters)

                'clear all trees and totals
                ClearDayTreesTotals()

                'display day
                LoadDayDateLabel()

                If (ds Is Nothing) Then
                    bResult = False
                ElseIf (ds.Tables.Count < 2) Then
                    bResult = False
                Else

                    bResult = PopulateTreeView(Me.treeShip, ds.Tables(0))

                    If (Not ds.Tables(1) Is Nothing) Then
                        If (Not ds.Tables(1).DefaultView.Table.Rows(0)(0) Is Nothing) Then
                            Me.lblTotalShip.Text = ds.Tables(1).DefaultView.Table.Rows(0)(0).ToString()
                        End If
                    End If

                    If (bResult) Then
                        Me.treeShip.Nodes(0).Expanded = True
                    End If
                End If

                LoadRevision()

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally
            Me.cmdPrint.Enabled = bResult
        End Try

        Return bResult

    End Function

    Private Sub ClearDayTreesTotals()
        Me.treeShip.Nodes.Clear()
        Me.lblTotalShip.Text = "?"
    End Sub

    Private Sub LoadDayDateLabel()
        Try
            Dim dt As Date

            If (Date.TryParse(Me.tbDay.Text, dt)) Then
                lblDayOfWeek.Text = dt.ToString("dddd, MM/dd/yyyy")
            Else
                Master.Msg = "Invalid Date"
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub AssignTemplate(ByVal templateID As String, ByRef status As String, ByRef message As String)

        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Try
            oSqlParameter = New SqlParameter("@TemplateID", SqlDbType.Decimal)
            oSqlParameter.Value = templateID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@LotNumber8s", SqlDbType.VarChar, 8000)
            oSqlParameter.Value = GetDelimitedLots()
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procPSAssignTemplate", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function GetDelimitedLots() As String
        Dim i As Int32 = 0
        Dim retval As String = ""
        Dim nodeText As String = ""
        Dim tree As RadTreeView = Me.treeShip
        Dim node As RadTreeNode

        Try

            If (tree.Nodes.Count <= 0) Then
                Return ""
            End If

            node = tree.Nodes(0)

            If (node.Nodes.Count <= 0) Then
                Return ""
            End If

            For i = 0 To (node.Nodes.Count - 1)
                'check if HTML in nodetext from the background formatting 
                'MAS 06-06-06

                nodeText = node.Nodes(i).Text.Replace("-", "")
                nodeText = Utility.RemoveHtmlTags(nodeText)
                'MAS 09-05-2006 check if lot is on hold and if so then do not add to lots 
                If Not InStr(nodeText, "on hold", CompareMethod.Text) > 0 Then
                    retval += (nodeText.Substring(0, 8) + LOT_NUM_DELIMITER)
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

    Private Function LoadRevision() As Boolean
        Dim bResult As Boolean = False
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        Dim trackID As String = Me.tbDay.Text
        Dim ds1 As DataSet

        Try
            Me.lblRevNo.Text = "?"
            Me.lblRevComments.Text = "?"

            oSqlParameter = New SqlParameter("@TrackingID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = tbDay.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@RevisionTypeID", SqlDbType.VarChar, 50)
            oSqlParameter.Value = REVISION_TYPE_ID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
            colParameters.Add(oSqlParameter)

            ds1 = DA.GetDataSet("procGetLastRevisionComment", colParameters)

            If (DA.IsDSEmpty(ds1)) Then
                Me.lblRev.Text = "1"
                bResult = False
            Else
                Me.lblRevNo.Visible = True
                Me.lblRevComments.Visible = True
                Me.lblRev.Visible = True
                Me.lblRevNo.Text = ds1.Tables(0).DefaultView.Table.Rows(0)("RevisionNumber").ToString()
                Me.lblRevComments.Text = ds1.Tables(0).DefaultView.Table.Rows(0)("Comment").ToString()
                Me.lblRev.Text = ds1.Tables(0).DefaultView.Table.Rows(0)("NextRevisionNumber").ToString()

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
            Dim intRevisonNumber As Integer

            If (Integer.TryParse(Me.txtComment.Text, intRevisonNumber)) Then
                Master.tMsg("Revision", "Error: unable to update Revision for " + Me.lblDayOfWeek.Text + ".<br> S.P. Status: " + status + ".<br>S.P. Message: " + message)
            Else
                'procPSInsertRevision @RevisionTypeID varchar(50), @RevisionNumber Int,
                '                     @Comment varchar(80), @TrackingID varchar(50),
                '                     @Status varchar(80) OUT, @ErrorMsg varchar(80) OUT 

                oSqlParameter = New SqlParameter("@RevisionTypeID", SqlDbType.VarChar, 50)
                oSqlParameter.Value = REVISION_TYPE_ID
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@RevisionNumber", SqlDbType.Int)
                oSqlParameter.Value = Me.lblRev.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Comment", SqlDbType.VarChar, 500)
                oSqlParameter.Value = txtComment.Text
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@TrackingID", SqlDbType.VarChar, 50)
                oSqlParameter.Value = Me.tbDay.Text
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
                    Master.tMsg("Revision", "Error: unable to update Revision for " + Me.lblDayOfWeek.Text + ".<br> S.P. Status: " + status + ".<br>S.P. Message: " + message)
                Else
                    Master.tMsg("Revision", "Revision for " + Me.lblDayOfWeek.Text + " has been updated.")
                End If
            End If

            LoadRevision()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Function SaveLotData() As Boolean
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim status As String = ""
        Dim message As String = ""
        Dim bResult As Boolean = False

        Try
            'procPSUpdateLotForShipping @LotNumber8 varchar(8),
            '					@ShipTime varchar(300),
            '					@ArrivalTime varchar(300),
            '					@ApproxSeqTime varchar(300),
            '					@Trailer varchar(300),
            '					@DriverName varchar(300),
            '					@ActualDepartTime varchar(300),
            '					@Status Varchar(80) OUT,
            '					@ErrorMsg Varchar(80) OUT  

            oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
            oSqlParameter.Value = Me.lblLdLot.Text.Replace("-", "")
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ShipTime", SqlDbType.VarChar, 300)
            oSqlParameter.Value = Me.tbLdShipTime.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ArrivalTime", SqlDbType.VarChar, 300)
            oSqlParameter.Value = Me.tbLdArrivalTime.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ApproxSeqTime", SqlDbType.VarChar, 300)
            oSqlParameter.Value = Me.tbLdSeqTime.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Trailer", SqlDbType.VarChar, 300)
            oSqlParameter.Value = Me.txtTrailer.Text 'Me.ddlTrailer.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@DriverName", SqlDbType.VarChar, 300)
            oSqlParameter.Value = Me.ddlDriver.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ActualDepartTime", SqlDbType.VarChar, 300)
            oSqlParameter.Value = Me.tbLdDepartureTime.Text
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

            colOutput = DA.ExecSP("procPSUpdateLotForShipping", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status.ToUpper() <> "TRUE") Then
                Master.tMsg("Save", "Error: Unable to save Lot Detail for Lot " + Me.lblLdLot.Text + ".<br>SP Status: " + status + ".<br>SP Message: " + message)
                bResult = False
            End If

            Master.tMsg("Save", "Updated Lot Detail for Lot " + Me.lblLdLot.Text)
            bResult = True

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function PopulateTreeView(ByRef tree As RadTreeView, ByRef dt As DataTable) As Boolean
        Dim dateNode As New RadTreeNode
        Dim lotNode As New RadTreeNode
        Dim subLotNode As New RadTreeNode

        Dim strDate As String = Me.tbDay.Text
        Dim strLot As String = ""
        Dim strSubLot As String = ""
        Dim strBC As String = ""
        Dim strQty As String = ""
        Dim strSeqDT As String = ""
        Dim SetexFlag As Boolean = False
        Dim dateChange As Boolean = False
        Dim holdFlag As Boolean = False
        Dim strOnHold As String = ""
        Dim strShipTime As String = ""
        Dim strImageName As String = ""

        Dim dateAddedFlag As Boolean = False
        Dim bResult As Boolean = False

        Dim qtyLot As Int32 = 0
        Dim i As Int32

        Try
            'clear out tree control
            tree.Nodes.Clear()

            If (dt Is Nothing) Then
                bResult = False
            ElseIf (dt.Rows.Count <= 0) Then
                bResult = False
            Else
                'add all nodes
                For i = 0 To dt.Rows.Count - 1
                    strBC = dt.Rows(i)(procPSGetShipSchedColumnType.BC).ToString()
                    strOnHold = dt.Rows(i)(procPSGetShipSchedColumnType.OnHold).ToString()
                    strSeqDT = dt.Rows(i)(procPSGetShipSchedColumnType.SequenceDT).ToString()
                    strQty = dt.Rows(i)(procPSGetShipSchedColumnType.Qty).ToString()
                    strImageName = dt.Rows(i)(procPSGetShipSchedColumnType.ImageName).ToString()

                    If (strQty = "") Then
                        strQty = "0"
                    End If

                    If (Not dateAddedFlag) Then
                        strDate = Me.tbDay.Text
                        dateNode = Nothing
                        dateNode = New RadTreeNode
                        dateNode.Text = strDate
                        dateNode.Value = strSeqDT
                        tree.Nodes.Add(dateNode)
                        dateChange = True
                        dateAddedFlag = True
                    End If

                    'add new lot if needed
                    If ((strLot <> dt.Rows(i)(procPSGetShipSchedColumnType.LotNumberNoBlanks).ToString()) Or (dateChange)) Then
                        'add quantity to previous lot and on hold text if needed
                        lotNode.Text += (" : " + qtyLot.ToString())
                        'MS 06-06-2006 check if the ship code is "" then we know this is a customer order and therefore we want the background to be gray
                        If SetexFlag = False Then
                            lotNode.Text = "<span class='allOrders'>" + lotNode.Text + "</span>"
                        End If
                        If (holdFlag) Then
                            lotNode.Text += ON_HOLD
                        Else
                            lotNode.Text += strShipTime
                        End If
                        qtyLot = 0
                        holdFlag = False
                        strShipTime = dt.Rows(i)(procPSGetShipSchedColumnType.ShipTime).ToString()

                        If (strShipTime.Length > 0) Then
                            strShipTime = "<span class=""boldShipTime"">" + strShipTime + "</span>"
                        End If

                        strLot = dt.Rows(i)(procPSGetShipSchedColumnType.LotNumberNoBlanks).ToString()
                        lotNode = Nothing
                        lotNode = New RadTreeNode
                        'MS 06-06-2006 check if a Setex Order if so set a value
                        SetexFlag = CBool(dt.Rows(i)(procPSGetShipSchedColumnType.SetexOrder))
                        lotNode.Text = strLot  'then a Setex Order
                        lotNode.Value = strSeqDT
                        lotNode.ImageUrl = String.Format("../../Images/Misc/{0}", strImageName)


                        If (strOnHold.ToUpper = "TRUE") Then
                            holdFlag = True
                        End If
                        dateNode.Nodes.Add(lotNode)
                    End If
                    'reset shift change flag
                    dateChange = False

                    'add sublot
                    strSubLot = dt.Rows(i)(procPSGetShipSchedColumnType.SubLotNumber).ToString()
                    subLotNode = Nothing
                    subLotNode = New RadTreeNode
                    'MS 06-06-2006 check if the ship code is "" then we know this is a customer order and therefore we want the background to be gray
                    If CBool(dt.Rows(i)(procPSGetShipSchedColumnType.SetexOrder)) = False Then
                        subLotNode.Text = "<span class='allOrders'>" + strSubLot + " : " + strQty + " " + strBC + "</span>"
                    Else
                        subLotNode.Text = strSubLot + " : " + strQty + " " + strBC
                    End If
                    subLotNode.Value = strSeqDT
                    subLotNode.ImageUrl = String.Format("../../Images/Misc/{0}", strImageName)

                    lotNode.Nodes.Add(subLotNode)
                    qtyLot += Convert.ToInt32(strQty)
                Next

                'add total to the very last lot
                lotNode.Text += (" : " + qtyLot.ToString())

                'add ship time to the very last lot
                lotNode.Text += strShipTime
                If SetexFlag = False Then
                    lotNode.Text = "<span class='allOrders'>" + lotNode.Text + "</span>"
                End If

                bResult = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Sub ClearLotData()
        Me.lblLdLoad.Text = ""
        Me.lblLdLot.Text = ""
        Me.tbLdShipTime.Text = ""
        Me.tbLdNotes.Text = ""
        Me.tbLdDepartureTime.Text = ""
        Me.tbLdArrivalTime.Text = ""
    End Sub

    Private Function LoadLotData() As Boolean
        Dim retVal As Boolean = False
        Dim sql As String = ""
        Dim strLot As String = ""
        Dim ds As New DataSet
        Dim strIndex As String = "0"
        Dim strDriver As String = ""
        Dim i As Integer = 0
        Dim idx As Integer = 0

        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        Try
            If (treeShip.SelectedNode IsNot Nothing) Then

                'strLot = treeShip.SelectedNode.Text.Trim().Replace("-", "").Replace("<span class='allOrders'>", "")
                strLot = treeShip.SelectedNode.Text.Trim().Replace("-", "").Replace("<span class='allOrders'>", "")
                strLot = strLot.Substring(0, 8)

                If (treeShip.SelectedNode.Level = 1) Then
                    idx = treeShip.SelectedNode.Index
                ElseIf (treeShip.SelectedNode.Level = 2) Then
                    idx = treeShip.SelectedNode.ParentNode.Index
                End If
                Me.lblLdLoad.Text = (idx + 1).ToString()

                oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
                oSqlParameter.Value = strLot
                colParameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
                oSqlParameter.Value = ddlBroadcastPointID.SelectedValue
                colParameters.Add(oSqlParameter)

                ds = DA.GetDataSet("procPSGetShipLotDetail", colParameters)

                If (DA.IsDSEmpty(ds)) Then
                    ClearLotData()
                    retVal = False
                Else
                    Me.lblLdLot.Text = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipLotDetailColumnType.LotNumber).ToString()
                    Me.tbLdShipTime.Text = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipLotDetailColumnType.ShipTime).ToString()
                    Me.tbLdArrivalTime.Text = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipLotDetailColumnType.ArrivalTime).ToString()
                    Me.tbLdSeqTime.Text = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipLotDetailColumnType.ApproxSeqTime).ToString()
                    Me.tbLdDepartureTime.Text = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipLotDetailColumnType.ActualDepartureTime).ToString()
                    Me.tbLdNotes.Text = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipLotDetailColumnType.ShippingNotes).ToString()
                    Me.txtTrailer.Text = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipLotDetailColumnType.TrailerNumber).ToString()
                    strDriver = ds.Tables(0).DefaultView.Table.Rows(0)(procPSGetShipLotDetailColumnType.Driver).ToString()

                    'Find the driver and set
                    i = ddlDriver.Items.IndexOf(ddlDriver.Items.FindByValue(strDriver))
                    If (i < 0) Then
                        Me.ddlDriver.Items.Insert(1, strDriver)
                        Me.ddlDriver.Items(1).Value = strDriver
                        i = 1
                    End If
                    Me.ddlDriver.SelectedIndex = i

                    retVal = True
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return retVal

    End Function

    Private Sub MoveLot()
        Dim lotNum As String = ""
        Dim status As String = ""
        Dim message As String = ""
        Dim retVal As String = ""
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Try
            lotNum = hidNodeLot.Value.Trim().Replace("-", "")

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
            oSqlParameter.Value = "SHIP"
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@CurrentBroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = Me.ddlBroadcastPointID.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@NewBroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = Me.hidMoveBroadcastPointID.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            'oSqlParameter = New SqlParameter("@ProdSchedDTIncrement", 0)
            'oSqlParameter.Direction = ParameterDirection.Output
            'colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procPSMoveLot", colParameters)

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
                Master.tMsg("Move", "Lot Number: " + lotNum + " has been moved.")
            Else
                Master.tMsg("Move", "Error - Lot Number: " + lotNum + " has NOT been moved. <BR>Error message: " + message + "<BR>Status message: " + status)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub RefreshShippingSchedule()
        Dim lot As String = ""
        Try
            If (treeShip.SelectedNode IsNot Nothing) Then
                lot = treeShip.SelectedNode.Value
            End If

            LoadDay()

            'select previously selected node
            'If (lot.Length > 0) Then
            '    SelectLastSelectedNodeInTree(treeShip)
            '    If (treeShip.SelectedNode IsNot Nothing) Then
            '        ShowLotData(LoadLotData())
            '    End If
            'End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region


End Class