Imports Telerik.Web.UI
Imports System.Data.SqlClient

Public Class StationSequenceConfiguration
    Inherits System.Web.UI.Page

    Enum UpdateType
        updateSeq
        prevSeq
        nextSeq
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
                'DataAccess.BroadcastPointID.Load_ddlBroadcastPointID(Me.ddlBroadcastPointID, Request, Server)

                Load_ddlTop_ShowTopXrecords()
                Load_lbStations()

            End If
            GetBroadcastPointKey()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub StationSequenceConfiguration_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbStations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbStations.SelectedIndexChanged
        lbStationsSelectedIndexChanged()
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        Try
            LoadStationData()
            LoadSequenceData()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdPrev_Click(sender As Object, e As System.EventArgs) Handles cmdPrev.Click
        ' The dialog posts back to the cmdUpdate button.
        ' The procedure UpdateSequenceData()  determines which event to continue with (Prev, Next or Update)
    End Sub

    Private Sub cmdNext_Click(sender As Object, e As System.EventArgs) Handles cmdNext.Click
        ' The dialog posts back to the cmdUpdate button.
        ' The procedure UpdateSequenceData()  determines which event to continue with (Prev, Next or Update)
    End Sub

    Private Sub cmdUpdate_Click(sender As Object, e As System.EventArgs) Handles cmdUpdate.Click
        ' The dialog posts back to the cmdUpdate button.
        ' The procedure UpdateSequenceData()  determines which event to continue with (Prev, Next or Update)
        Try
            UpdateSequenceData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ddlSeq_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSeq.SelectedIndexChanged
        Try
            LoadSequenceData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub ddlSeq_ItemDataBound(ByVal o As Object, ByVal e As RadComboBoxItemEventArgs)
        Dim dataSourceRow As DataRowView = CType(e.Item.DataItem, DataRowView)
        'e.Item.Attributes("customAttribute1") = dataSourceRow("CustomAttribute2").ToString()
        e.Item.CssClass = "comboBoxPrePopImage"
        e.Item.ImageUrl = String.Format("../../../Images/Misc/{0}", dataSourceRow("ImageName").ToString())
    End Sub


#End Region

#Region "Methods"


    Private Sub EnableControls()
        Dim toDlg As String = ""

        Master.Secure(cmdNext)
        Master.Secure(cmdPrev)
        Master.Secure(cmdUpdate)

        If (Me.lbStations.SelectedIndex < 0) Then
            Me.cmdUpdate.Enabled = False
            Me.cmdNext.Enabled = False
            Me.cmdPrev.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' Loads the ddlTop that displays the 'Show Top Selected'
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Load_ddlTop_ShowTopXrecords()
        Try
            Dim sql As String = "SELECT [ParameterListValue] FROM [tblParameterListValues] WHERE(ParameterListID = 42) ORDER BY DisplayID "

            With ddlTop
                .DataSource = DA.GetDataSet(sql)
                .DataTextField = "ParameterListValue"
                .DataValueField = "ParameterListValue"
                .DataBind()
                .Items.Insert(0, "CLOSEST")
                .SelectedIndex = 0
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Load_lbStations()
        Dim colParameters As New List(Of SqlParameter)
        Dim ds As DataSet

        Try
            lbStations.Items.Clear()

            ds = DA.GetDataSet("procGetStationSequencePointers")

            If (DA.IsDataSetNotEmpty(ds)) Then
                lbStations.DataSource = ds
                lbStations.DataTextField = "Description"
                lbStations.DataValueField = "StationID"
                lbStations.DataBind()
                lbStations.SelectedIndex = 0
                lbStationsSelectedIndexChanged()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub LoadStationData()
        Try
            Dim seq As String = ""
            Dim compl As String = ""
            Dim total As String = ""

            If (Me.lbStations.SelectedIndex < 0) Then   'PAH
                Me.lblStation.Text = ""
                Me.txtCurrSeq.Text = ""
                Me.txtCurrJobsCompleted.Text = ""
                Me.txtCurrTotalJobs.Text = ""
            Else
                GetSeqInfoData(compl, total, seq, "")

                Me.lblStation.Text = Me.lbStations.SelectedItem.Text
                Me.txtCurrSeq.Text = seq
                Me.txtCurrJobsCompleted.Text = compl
                Me.txtCurrTotalJobs.Text = total

                LoadSequenceDDL()

                'pb, 1/17/05
                'note: had to hard code a length of 10 for seq number (13 with dashes)
                Dim i As Integer
                If ddlSeq.Items.Count > 0 Then  'PAH
                    For i = 0 To ddlSeq.Items.Count - 1
                        If ((ddlSeq.Items(i).Text.Length >= 13) And (Me.txtCurrSeq.Text.Length >= 13)) Then
                            If ddlSeq.Items(i).Text.Substring(0, 13) = txtCurrSeq.Text.Substring(0, 13) Then
                                ddlSeq.SelectedIndex = i
                                Exit For
                            End If
                        End If
                    Next
                End If

                If (ddlSeq.SelectedIndex >= 0) Then
                        hidOriginalSequence.Value = ddlSeq.SelectedItem.Text
                    End If
                    hidOriginalTotalJobs.Value = total
                    hidOriginalJobsCompleted.Value = compl

                End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadSequenceData()
        Try
            'pb, 1/20/05
            'note: had to hard code a length of 10 for seq number (13 with dashes)

            Dim seqNum As String = ""
            Dim seq As String = ""
            Dim compl As String = ""
            Dim total As String = ""

            Me.txtUpdateJobsCompleted.Text = ""
            Me.txtUpdateTotalJobs.Text = ""

            If (Me.ddlSeq.SelectedIndex >= 0) Then
                If Me.ddlSeq.SelectedItem.Text.Length >= 13 Then
                    seqNum = Me.ddlSeq.SelectedItem.Text.Substring(0, 13).Replace("-", "")

                    GetSeqInfoData(compl, total, seq, seqNum)

                    txtUpdateJobsCompleted.Text = compl
                    txtUpdateTotalJobs.Text = total
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub LoadSequenceDDL()
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)

            Me.ddlSeq.Items.Clear()

            oSqlParameter = New SqlParameter("@Top", SqlDbType.VarChar, 8)
            oSqlParameter.Value = ddlTop.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
            oSqlParameter.Value = Left(Replace(Me.txtCurrSeq.Text, "-", ""), 8)
            colParameters.Add(oSqlParameter)

            ddlSeq.DataSource = DA.GetDataSet("procPSGetStationSequenceLots", colParameters)
            ddlSeq.DataTextField = "SeqInfoText"    '"SeqInfo"
            ddlSeq.DataValueField = "SeqInfoValue"    '"SequenceNumber"
            ddlSeq.DataBind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbStationsSelectedIndexChanged()
        Try
            LoadStationData()
            LoadSequenceData()
            cmdRefresh.Enabled = True

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub GetSeqInfoData(ByRef CompletedQuantity As String, ByRef TotalQuantity As String, ByRef SequenceNumber As String, ByVal seqNum As String)
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            Dim qtyNeeded As Integer = 0
            Dim qtyRow1 As Integer = 0
            Dim qtyRow2 As Integer = 0
            Dim qtyRow3 As Integer = 0


            'CREATE PROCEDURE procGetCurrentSequenceInfo @StationID varchar(4), @InputSequenceNumber varchar(50) = '', @CompletedQuantity varchar(4) Out,@TotalQuantity varchar(4) Out,
            '			@SequenceNumber varchar(50) Out, @Status varchar(80) OUT, @ErrorMsg varchar(80) OUT 

            oSqlParameter = New SqlParameter("@StationID", SqlDbType.VarChar, 8)
            oSqlParameter.Value = Me.lbStations.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@InputSequenceNumber", SqlDbType.VarChar, 50)
            oSqlParameter.Value = seqNum
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@JobsCompleted", SqlDbType.VarChar, 4)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@TotalQuantity", SqlDbType.VarChar, 4)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@SequenceNumber", SqlDbType.VarChar, 50)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@SeatRowsPerOrder", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@QtyRow1", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@QtyRow2", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@QtyRow3", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)


            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procGetCurrentSequenceInfo", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    Select Case (.ParameterName)
                        Case "@BroadcastPointID"
                            hidBroadcastPointID.Value = oParameter.Value.ToString()
                        Case "@SequenceNumber"
                            SequenceNumber = oParameter.Value.ToString()
                        Case "@JobsCompleted"
                            CompletedQuantity = oParameter.Value.ToString()
                        Case "@TotalQuantity"
                            TotalQuantity = oParameter.Value.ToString()
                        Case "@SeatRowsPerOrder"
                            qtyNeeded = CInt(If(IsDBNull(oParameter.Value), 0, oParameter.Value))
                        Case "@QtyRow1"
                            qtyRow1 = CInt(If(IsDBNull(oParameter.Value), 0, oParameter.Value))
                        Case "@QtyRow2"
                            qtyRow2 = CInt(If(IsDBNull(oParameter.Value), 0, oParameter.Value))
                        Case "@QtyRow3"
                            qtyRow3 = CInt(If(IsDBNull(oParameter.Value), 0, oParameter.Value))
                    End Select
                End With
            Next

            txtPalletRow1Total.Text = TotalQuantity
            If (qtyNeeded > 1) Then
                txtPalletRow2Total.Text = TotalQuantity
                txtPalletRow3Total.Text = TotalQuantity

                lblRow2.Enabled = True
                lblRow3.Enabled = True
                txtPalletRow2Total.Enabled = True
                txtPalletRow3Total.Enabled = True
                txtPalletRow2Complete.Enabled = True
                txtPalletRow3Complete.Enabled = True
            Else
                lblRow2.Enabled = False
                lblRow3.Enabled = False
                txtPalletRow2Total.Enabled = False
                txtPalletRow3Total.Enabled = False
                txtPalletRow2Complete.Enabled = False
                txtPalletRow3Complete.Enabled = False
            End If

            txtPalletRow1Complete.Text = qtyRow1.ToString()
            If (qtyNeeded > 1) Then
                txtPalletRow2Complete.Text = qtyRow2.ToString()
                txtPalletRow3Complete.Text = qtyRow3.ToString()
            Else
                txtPalletRow2Total.Text = ""
                txtPalletRow3Total.Text = ""
                txtPalletRow2Complete.Text = ""
                txtPalletRow3Complete.Text = ""
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub UpdateSequenceData()
        Try
            Dim str As String = ""
            Dim bResult As Boolean = False

            Select Case (hidUpdateType.Value())
                Case "update"
                    bResult = UpdateStationSequence()

                Case "prev"
                    str = "previous "
                    bResult = UpdateStationSequence_NextOrPrev(UpdateType.prevSeq)

                Case "next"
                    str = "next "
                    bResult = UpdateStationSequence_NextOrPrev(UpdateType.nextSeq)

                Case Else
                    Master.Msg = "Error performing action."

            End Select

            LoadStationData()
            LoadSequenceData()

            If (bResult) Then
                DisplayUpdateMessage(str)
            Else
                Master.tMsg("Update", "Station Sequence Configuration Failed.")
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function UpdateStationSequence() As Boolean
        Dim bResult As Boolean = False
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            Dim status As String = ""
            Dim message As String = ""

            Dim JobComplete As Integer = 0
            Dim R1Complete As Integer = 0
            Dim R2Complete As Integer = 0
            Dim R3Complete As Integer = 0

            Integer.TryParse(Me.txtUpdateJobsCompleted.Text, JobComplete)
            Integer.TryParse(Me.txtPalletRow1Complete.Text, R1Complete)
            Integer.TryParse(Me.txtPalletRow2Complete.Text, R2Complete)
            Integer.TryParse(Me.txtPalletRow3Complete.Text, R3Complete)


            'CREATE PROCEDURE procUpdateSequence 
            '@StationID		VARCHAR(4)
            ', @SequenceNumber	VARCHAR(50)
            ', @CompletedJobs	INT
            ', @CompletedRow1	INT
            ', @CompletedRow2	INT
            ', @CompletedRow3	INT
            ', @Status			VARCHAR(80) OUT
            ', @ErrorMsg			VARCHAR(80) OUT 

            oSqlParameter = New SqlParameter("@StationID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = Me.lbStations.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@SequenceNumber", SqlDbType.VarChar, 50)
            oSqlParameter.Value = Me.ddlSeq.SelectedValue.Substring(0, 10)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@CompletedJobs", SqlDbType.Int)
            oSqlParameter.Value = JobComplete
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@CompletedRow1", SqlDbType.Int)
            oSqlParameter.Value = R1Complete
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@CompletedRow2", SqlDbType.Int)
            oSqlParameter.Value = R2Complete
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@CompletedRow3", SqlDbType.Int)
            oSqlParameter.Value = R3Complete
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@UserName", SqlDbType.VarChar, 80)
            oSqlParameter.Value = Session("UserFirstLastName")
            colParameters.Add(oSqlParameter)


            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)


            colOutput = DA.ExecSP("procUpdateSequence", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status <> "True") Then
                Master.Msg = "Error: Unable to update Station Sequence Configuration.<br>S.P. Status: " + status + ".<br>S.P. Error Message: " + message
            Else
                bResult = True
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return bResult
    End Function

    Private Function UpdateStationSequence_NextOrPrev(type As UpdateType) As Boolean

        Dim bResult As Boolean = False
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter) = Nothing

            Dim status As String = ""
            Dim message As String = ""
            Dim bContinue As Boolean = False

            'procDecrementSequence @StationID varchar(4),
            '			@SequenceNumber varchar(50) OUT, @ProductID varchar(80) OUT,
            '           @Status varchar(80) OUT, @ErrorMsg varchar(80) OUT 


            oSqlParameter = New SqlParameter("@StationID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = Me.lbStations.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@UserName", SqlDbType.VarChar, 80)
            oSqlParameter.Value = Session("UserFirstLastName")
            colParameters.Add(oSqlParameter)


            oSqlParameter = New SqlParameter("@SequenceNumber", SqlDbType.VarChar, 50)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ProductID", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            If (type = UpdateType.prevSeq) Then
                colOutput = DA.ExecSP("procDecrementSequence", colParameters)
                bContinue = True
            ElseIf (type = UpdateType.nextSeq) Then
                colOutput = DA.ExecSP("procIncrementSequence", colParameters)
                bContinue = True
            End If

            If (bContinue) Then
                For Each oParameter In colOutput
                    With oParameter
                        If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                            status = oParameter.Value.ToString()
                        ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                            message = oParameter.Value.ToString()
                        End If
                    End With
                Next
            End If

            If (status <> "True") Then
                Master.Msg = "Error: Unable to update Station Sequence Configuration.<br>S.P. Status: " + status + ".<br>S.P. Error Message: " + message
            Else
                bResult = True
            End If


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
        image1.Attributes("src") = "../../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint2(description As String, imageName As String)
        lblBP2.Text = description
        Panel2.Visible = True
        image2.Attributes("src") = "../../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint3(description As String, imageName As String)
        lblBP3.Text = description
        Panel3.Visible = True
        image3.Attributes("src") = "../../../Images/Misc/" + imageName
    End Sub

    Private Sub UpdatePanelBroadcastPoint4(description As String, imageName As String)
        lblBP4.Text = description
        Panel4.Visible = True
        image4.Attributes("src") = "../../../Images/Misc/" + imageName
    End Sub


    Private Sub DisplayUpdateMessage(ByVal strUpdateType As String)
        Try
            Dim strStation = ""
            Dim strSequence = ""

            If (lbStations.SelectedIndex >= 0) Then
                strStation = lbStations.SelectedItem.Text
            End If
            If (ddlSeq.SelectedIndex >= 0) Then
                strSequence = ddlSeq.SelectedItem.Text
            End If

            Dim strMessage As String = "Station Sequence Configuration updated "
            strMessage += strUpdateType
            strMessage += "sequence for station: " + strStation +
                        ", Original Sequence: " + hidOriginalSequence.Value.Replace("-", "") +
                        ", Total Jobs: " + hidOriginalTotalJobs.Value +
                        ", Jobs Completed: " + hidOriginalJobsCompleted.Value +
                        ", New Sequence: " + strSequence +
                        ", Total Jobs: " + txtUpdateTotalJobs.Text +
                        ", Jobs Completed: " + txtUpdateJobsCompleted.Text

            Master.tMsg("Update", strMessage)

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

#End Region

End Class