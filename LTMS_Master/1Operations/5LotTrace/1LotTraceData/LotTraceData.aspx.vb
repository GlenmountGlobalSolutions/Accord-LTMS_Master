Imports System.Data.SqlClient

Public Class LotTraceData
    Inherits System.Web.UI.Page

    Protected datatable As New DataTable()
    Dim strHumanReadable As String = ""
    Dim strSeatCode As String = ""
    Dim strSeatPartNumber As String = ""

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strPrinter As String = ""
        Dim strSelectedPrinter As String = ""
        Dim strJSForSetSelectedPrinter As String = ""

        Try
            If (Not IsPostBack) Then
                SetSSSNDetailPanelsVisibility(False)
                GetAvailableTaskStatusValues()

                'grab the selected printer from the request object and run client-side script to select it from the printer drop down list
                'this is needed for each postback that occurs because the printer drop down won't keep its selection (not a dot net drop down)
                If (hidSelectedPrinterPrelim.Value.Length > 0) Then
                    strSelectedPrinter = hidSelectedPrinterPrelim.Value
                End If

                populateLabelTypeDropDown()
                populatePrinterDropDown(strSelectedPrinter)
                lblPrintSSN.Visible = False

            Else
                lblSSN.Text = hidlblSSN.Value
                lblSeatStyle.Text = hidlblSeatStyle.Value
                lblProductID.Text = hidlblProductID.Value
                lblSeatType.Text = hidlblSeatType.Value
                lblSeatDesc.Text = hidlblSeatDesc.Value
                lblColorDesc.Text = hidlblColorDesc.Value
                lblPrintSSN.Text = hidlblSSN.Value
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub LotTraceData_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        EnableControls()
    End Sub

    Private Sub cmdView_Click(sender As Object, e As System.EventArgs) Handles cmdView.Click
        Try
            SetSSSNDetailPanelsVisibility(False)
            DetermineSSN()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub DetermineSSN()
        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim ds As DataSet
        Dim bResult As Boolean = False
        Dim strOperationID As String = ""
        Dim strDescription As String = ""
        Dim pintReturn As Integer = 0


        Try
            If txtSSN.Text <> "" Then
                lblMessage.Text = ""
                lblddlSSN.Visible = False
                ddlSSN.Visible = False
                pnlShipStatus.Visible = False

                prmNext = New Data.SqlClient.SqlParameter("@SerialNumber", SqlDbType.VarChar, 32)
                prmNext.Value = txtSSN.Text.Trim.ToUpper
                colParms.Add(prmNext)
                ds = DA.GetDataSet("procSGGetSSN", colParms)
                colParms.Clear()

                If (ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0) Then
                    lblMessage.Text = "Error: Please enter valid serial number!"
                    lblShipStatus.Text = ""
                    cmdPrint.Visible = False
                ElseIf (ds.Tables(0).Rows.Count > 10) Then
                    lblMessage.Text = "Notice: Too many records retrieved!  Try a different part or contact IT Operations."
                    lblShipStatus.Text = ""
                    cmdPrint.Visible = False
                Else
                    With ddlSSN
                        .Items.Clear()
                        .DataSource = ds
                        .DataTextField = "SSN"
                        .DataValueField = "SSN"
                        .DataBind()
                        If ds.Tables(0).Rows.Count > 1 Then
                            lblddlSSN.Visible = True
                            ddlSSN.Visible = True
                            .Items.Insert(0, "Choose an SSN")
                        Else
                            LoadShipStatusAndDetails(ds.Tables(0).Rows(0).Item(0).ToString)
                        End If
                    End With
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdModifyHistory_Click(sender As Object, e As System.EventArgs) Handles cmdModifyHistory.Click
        Try
            SaveModifiedProductHistory()
            LoadShipStatusAndDetails(ddlSSN.SelectedItem.ToString())

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dgHistory_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHistory.ItemDataBound
        Dim i As Integer = 0
        Dim SSN As String = ""
        Dim StyleGroupID As String = ""
        Dim Product As String = ""
        Dim OperationType As String = ""
        Dim OperationName As String = ""
        Dim RequirementData As String = ""
        Dim OperationResult As String = ""
        Dim StationID As String = ""
        Dim AnchorText As String = ""

        Dim TorgueValueResult_0 As String = "0"
        Dim TorgueValueResult_1 As String = "0"
        Dim TorgueValueResult_2 As String = "0"
        Dim TorgueValueResult_3 As String = "0"
        Dim TorgueValueResult_4 As String = "0"
        Dim TorgueValueResult_5 As String = "0"
        Dim TorgueValueResult_6 As String = "0"
        Dim TorgueValueResult_7 As String = "0"
        Dim TorgueValueResult_8 As String = "0"
        Dim TorgueValueResult_9 As String = "0"

        Dim TorgueAngleResult_0 As String = "0"
        Dim TorgueAngleResult_1 As String = "0"
        Dim TorgueAngleResult_2 As String = "0"
        Dim TorgueAngleResult_3 As String = "0"
        Dim TorgueAngleResult_4 As String = "0"
        Dim TorgueAngleResult_5 As String = "0"
        Dim TorgueAngleResult_6 As String = "0"
        Dim TorgueAngleResult_7 As String = "0"
        Dim TorgueAngleResult_8 As String = "0"
        Dim TorgueAngleResult_9 As String = "0"


        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                'SSN = txtSSN.Text
                If e.Item.ItemIndex <> -1 Then

                    SSN = CType(e.Item.DataItem, DataRowView).Row("SSN").ToString()
                    StyleGroupID = CType(e.Item.DataItem, DataRowView).Row("StyleGroupID").ToString()
                    Product = CType(e.Item.DataItem, DataRowView).Row("Product").ToString()
                    OperationType = CType(e.Item.DataItem, DataRowView).Row("OperationType").ToString()
                    OperationName = CType(e.Item.DataItem, DataRowView).Row("OperationName").ToString()
                    RequirementData = CType(e.Item.DataItem, DataRowView).Row("RequirementData").ToString()
                    OperationResult = CType(e.Item.DataItem, DataRowView).Row("OperationResult").ToString()
                    StationID = CType(e.Item.DataItem, DataRowView).Row("StationID").ToString()

                    'Populate Torque variables based on required data
                    If RequirementData = "1" Then
                        TorgueValueResult_0 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR0").ToString()
                        TorgueValueResult_1 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR1").ToString()
                        TorgueValueResult_2 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR2").ToString()
                        TorgueValueResult_3 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR3").ToString()
                        TorgueValueResult_4 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR4").ToString()
                        TorgueValueResult_5 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR5").ToString()
                        TorgueValueResult_6 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR6").ToString()
                        TorgueValueResult_7 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR7").ToString()
                        TorgueValueResult_8 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR8").ToString()
                        TorgueValueResult_9 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTVR9").ToString()

                        TorgueAngleResult_0 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR0").ToString()
                        TorgueAngleResult_1 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR1").ToString()
                        TorgueAngleResult_2 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR2").ToString()
                        TorgueAngleResult_3 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR3").ToString()
                        TorgueAngleResult_4 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR4").ToString()
                        TorgueAngleResult_5 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR5").ToString()
                        TorgueAngleResult_6 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR6").ToString()
                        TorgueAngleResult_7 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR7").ToString()
                        TorgueAngleResult_8 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR8").ToString()
                        TorgueAngleResult_9 = CType(e.Item.DataItem, DataRowView).Row("T1C1GTAR9").ToString()

                    ElseIf RequirementData = "2" Then
                        TorgueValueResult_0 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR0").ToString()
                        TorgueValueResult_1 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR1").ToString()
                        TorgueValueResult_2 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR2").ToString()
                        TorgueValueResult_3 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR3").ToString()
                        TorgueValueResult_4 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR4").ToString()
                        TorgueValueResult_5 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR5").ToString()
                        TorgueValueResult_6 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR6").ToString()
                        TorgueValueResult_7 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR7").ToString()
                        TorgueValueResult_8 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR8").ToString()
                        TorgueValueResult_9 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTVR9").ToString()

                        TorgueAngleResult_0 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR0").ToString()
                        TorgueAngleResult_1 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR1").ToString()
                        TorgueAngleResult_2 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR2").ToString()
                        TorgueAngleResult_3 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR3").ToString()
                        TorgueAngleResult_4 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR4").ToString()
                        TorgueAngleResult_5 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR5").ToString()
                        TorgueAngleResult_6 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR6").ToString()
                        TorgueAngleResult_7 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR7").ToString()
                        TorgueAngleResult_8 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR8").ToString()
                        TorgueAngleResult_9 = CType(e.Item.DataItem, DataRowView).Row("T1C2GTAR9").ToString()
                    ElseIf RequirementData = "3" Then
                        TorgueValueResult_0 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR0").ToString()
                        TorgueValueResult_1 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR1").ToString()
                        TorgueValueResult_2 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR2").ToString()
                        TorgueValueResult_3 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR3").ToString()
                        TorgueValueResult_4 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR4").ToString()
                        TorgueValueResult_5 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR5").ToString()
                        TorgueValueResult_6 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR6").ToString()
                        TorgueValueResult_7 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR7").ToString()
                        TorgueValueResult_8 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR8").ToString()
                        TorgueValueResult_9 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTVR9").ToString()

                        TorgueAngleResult_0 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR0").ToString()
                        TorgueAngleResult_1 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR1").ToString()
                        TorgueAngleResult_2 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR2").ToString()
                        TorgueAngleResult_3 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR3").ToString()
                        TorgueAngleResult_4 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR4").ToString()
                        TorgueAngleResult_5 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR5").ToString()
                        TorgueAngleResult_6 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR6").ToString()
                        TorgueAngleResult_7 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR7").ToString()
                        TorgueAngleResult_8 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR8").ToString()
                        TorgueAngleResult_9 = CType(e.Item.DataItem, DataRowView).Row("T1C3GTAR9").ToString()
                    ElseIf RequirementData = "4" Then
                        TorgueValueResult_0 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR0").ToString()
                        TorgueValueResult_1 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR1").ToString()
                        TorgueValueResult_2 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR2").ToString()
                        TorgueValueResult_3 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR3").ToString()
                        TorgueValueResult_4 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR4").ToString()
                        TorgueValueResult_5 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR5").ToString()
                        TorgueValueResult_6 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR6").ToString()
                        TorgueValueResult_7 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR7").ToString()
                        TorgueValueResult_8 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR8").ToString()
                        TorgueValueResult_9 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTVR9").ToString()

                        TorgueAngleResult_0 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR0").ToString()
                        TorgueAngleResult_1 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR1").ToString()
                        TorgueAngleResult_2 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR2").ToString()
                        TorgueAngleResult_3 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR3").ToString()
                        TorgueAngleResult_4 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR4").ToString()
                        TorgueAngleResult_5 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR5").ToString()
                        TorgueAngleResult_6 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR6").ToString()
                        TorgueAngleResult_7 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR7").ToString()
                        TorgueAngleResult_8 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR8").ToString()
                        TorgueAngleResult_9 = CType(e.Item.DataItem, DataRowView).Row("T1C4GTAR9").ToString()

                    ElseIf RequirementData = "13" Then
                        TorgueValueResult_0 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR0").ToString()
                        TorgueValueResult_1 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR1").ToString()
                        TorgueValueResult_2 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR2").ToString()
                        TorgueValueResult_3 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR3").ToString()
                        TorgueValueResult_4 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR4").ToString()
                        TorgueValueResult_5 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR5").ToString()
                        TorgueValueResult_6 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR6").ToString()
                        TorgueValueResult_7 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR7").ToString()
                        TorgueValueResult_8 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR8").ToString()
                        TorgueValueResult_9 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTVR9").ToString()

                        TorgueAngleResult_0 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR0").ToString()
                        TorgueAngleResult_1 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR1").ToString()
                        TorgueAngleResult_2 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR2").ToString()
                        TorgueAngleResult_3 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR3").ToString()
                        TorgueAngleResult_4 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR4").ToString()
                        TorgueAngleResult_5 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR5").ToString()
                        TorgueAngleResult_6 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR6").ToString()
                        TorgueAngleResult_7 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR7").ToString()
                        TorgueAngleResult_8 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR8").ToString()
                        TorgueAngleResult_9 = CType(e.Item.DataItem, DataRowView).Row("T2C1GTAR9").ToString()

                    ElseIf RequirementData = "14" Then
                        TorgueValueResult_0 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR0").ToString()
                        TorgueValueResult_1 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR1").ToString()
                        TorgueValueResult_2 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR2").ToString()
                        TorgueValueResult_3 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR3").ToString()
                        TorgueValueResult_4 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR4").ToString()
                        TorgueValueResult_5 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR5").ToString()
                        TorgueValueResult_6 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR6").ToString()
                        TorgueValueResult_7 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR7").ToString()
                        TorgueValueResult_8 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR8").ToString()
                        TorgueValueResult_9 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTVR9").ToString()

                        TorgueAngleResult_0 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR0").ToString()
                        TorgueAngleResult_1 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR1").ToString()
                        TorgueAngleResult_2 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR2").ToString()
                        TorgueAngleResult_3 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR3").ToString()
                        TorgueAngleResult_4 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR4").ToString()
                        TorgueAngleResult_5 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR5").ToString()
                        TorgueAngleResult_6 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR6").ToString()
                        TorgueAngleResult_7 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR7").ToString()
                        TorgueAngleResult_8 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR8").ToString()
                        TorgueAngleResult_9 = CType(e.Item.DataItem, DataRowView).Row("T2C2GTAR9").ToString()
                    ElseIf RequirementData = "15" Then
                        TorgueValueResult_0 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR0").ToString()
                        TorgueValueResult_1 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR1").ToString()
                        TorgueValueResult_2 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR2").ToString()
                        TorgueValueResult_3 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR3").ToString()
                        TorgueValueResult_4 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR4").ToString()
                        TorgueValueResult_5 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR5").ToString()
                        TorgueValueResult_6 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR6").ToString()
                        TorgueValueResult_7 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR7").ToString()
                        TorgueValueResult_8 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR8").ToString()
                        TorgueValueResult_9 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTVR9").ToString()

                        TorgueAngleResult_0 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR0").ToString()
                        TorgueAngleResult_1 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR1").ToString()
                        TorgueAngleResult_2 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR2").ToString()
                        TorgueAngleResult_3 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR3").ToString()
                        TorgueAngleResult_4 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR4").ToString()
                        TorgueAngleResult_5 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR5").ToString()
                        TorgueAngleResult_6 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR6").ToString()
                        TorgueAngleResult_7 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR7").ToString()
                        TorgueAngleResult_8 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR8").ToString()
                        TorgueAngleResult_9 = CType(e.Item.DataItem, DataRowView).Row("T2C3GTAR9").ToString()
                    ElseIf RequirementData = "16" Then
                        TorgueValueResult_0 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR0").ToString()
                        TorgueValueResult_1 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR1").ToString()
                        TorgueValueResult_2 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR2").ToString()
                        TorgueValueResult_3 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR3").ToString()
                        TorgueValueResult_4 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR4").ToString()
                        TorgueValueResult_5 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR5").ToString()
                        TorgueValueResult_6 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR6").ToString()
                        TorgueValueResult_7 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR7").ToString()
                        TorgueValueResult_8 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR8").ToString()
                        TorgueValueResult_9 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTVR9").ToString()

                        TorgueAngleResult_0 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR0").ToString()
                        TorgueAngleResult_1 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR1").ToString()
                        TorgueAngleResult_2 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR2").ToString()
                        TorgueAngleResult_3 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR3").ToString()
                        TorgueAngleResult_4 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR4").ToString()
                        TorgueAngleResult_5 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR5").ToString()
                        TorgueAngleResult_6 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR6").ToString()
                        TorgueAngleResult_7 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR7").ToString()
                        TorgueAngleResult_8 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR8").ToString()
                        TorgueAngleResult_9 = CType(e.Item.DataItem, DataRowView).Row("T2C4GTAR9").ToString()
                    End If


                    'Create lotAnchor string
                    AnchorText = "<a id='lotAnchor_" & i & "' href='#' data-SSN='" & SSN & _
                        "' data-StyleGroupID='" & StyleGroupID & _
                        "' data-Product='" & Product & _
                        "' data-OperationType='" & OperationType & _
                        "' data-OperationName='" & OperationName & _
                        "' data-RequirementData='" & RequirementData & _
                        "' data-OperationResult='" & OperationResult & _
                        "' data-StationID='" & StationID & _
                        "' data-Torque1='" & TorgueValueResult_0 & _
                        "' data-Torque2='" & TorgueValueResult_1 & _
                        "' data-Torque3='" & TorgueValueResult_2 & _
                        "' data-Torque4='" & TorgueValueResult_3 & _
                        "' data-Torque5='" & TorgueValueResult_4 & _
                        "' data-Torque6='" & TorgueValueResult_5 & _
                        "' data-Torque7='" & TorgueValueResult_6 & _
                        "' data-Torque8='" & TorgueValueResult_7 & _
                        "' data-Torque9='" & TorgueValueResult_8 & _
                        "' data-Torque10='" & TorgueValueResult_9 & _
                        "' data-Angle1='" & TorgueAngleResult_0 & _
                        "' data-Angle2='" & TorgueAngleResult_1 & _
                        "' data-Angle3='" & TorgueAngleResult_2 & _
                        "' data-Angle4='" & TorgueAngleResult_3 & _
                        "' data-Angle5='" & TorgueAngleResult_4 & _
                        "' data-Angle6='" & TorgueAngleResult_5 & _
                        "' data-Angle7='" & TorgueAngleResult_6 & _
                        "' data-Angle8='" & TorgueAngleResult_7 & _
                        "' data-Angle9='" & TorgueAngleResult_8 & _
                        "' data-Angle10='" & TorgueAngleResult_9 & _
                        "'>" & OperationType & "</a>"

                    'Cell(3) determines the link will be on the fourth column of the datagrid 
                    e.Item.Cells(3).Text = AnchorText

                    i = i + 1

                End If

                If (CType(e.Item.DataItem, DataRowView).Row("OperationResult").ToString().ToUpper = "FAIL") Or _
                   (CType(e.Item.DataItem, DataRowView).Row("OperationResult").ToString().ToUpper = "NOT FOUND") Or _
                   (CType(e.Item.DataItem, DataRowView).Row("OperationStatus").ToString().ToUpper = "FAIL") Or _
                   (CType(e.Item.DataItem, DataRowView).Row("OperationStatus").ToString().ToUpper = "NOT FOUND") Then

                    e.Item.Cells(6).CssClass = "ui-state-error-text"
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        Master.Secure(cmdView)
        Master.Secure(cmdPrint)
    End Sub

    Private Sub LoadShipStatusAndDetails(SSN As String)
        Dim strError As String = ""
        Try

            If (LoadDataGrid_ProductRequirements(SSN, strError) = True) Then
                GetSSNDetails(SSN)
                SetSSSNDetailPanelsVisibility(True)
                lblMessage.Text = ""
            Else
                SetSSSNDetailPanelsVisibility(False)
                lblMessage.Text = strError
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Function LoadDataGrid_ProductRequirements(ByRef strSSN As String, ByRef strErr As String) As Boolean
        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim ds As DataSet
        Dim bResult As Boolean = False
        Dim strOperationID As String = ""
        Dim strDescription As String = ""
        Dim i As Integer
        Dim colOutParms As List(Of SqlParameter)
        Dim pintReturn As Integer = 0

        Try
            dgHistory.DataSource = Nothing
            dgHistory.DataBind()

            prmNext = New Data.SqlClient.SqlParameter("@SerialNumber", SqlDbType.VarChar, 32)
            prmNext.Value = strSSN.ToUpper
            colParms.Add(prmNext)
            ds = DA.GetDataSet("procSGGetProductRequirements", colParms)
            colParms.Clear()

            If (ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0) Then
                bResult = False
                strErr = "Error: Please enter valid serial number!"
            Else
                dgHistory.DataSource = ds
                dgHistory.DataBind()
                bResult = True
            End If


            'Get Status
            prmNext = New Data.SqlClient.SqlParameter("@SSN", SqlDbType.VarChar, 16)
            prmNext.Value = strSSN
            colParms.Add(prmNext)
            prmNext = New Data.SqlClient.SqlParameter("@ReturnCode", SqlDbType.Int)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            colOutParms = DA.ExecSP("procSGVerifySSN_Single", colParms)
            colParms.Clear()

            For Each oParameter In colOutParms
                With oParameter
                    If .Direction = ParameterDirection.Output Then
                        Select Case .ParameterName
                            Case "@ReturnCode"
                                pintReturn = CType(oParameter.Value.ToString(), Integer)
                        End Select
                    End If
                End With
            Next

            colOutParms.Clear()

            If pintReturn = 1 Then
                lblShipStatus.Text = "<span class='fontGreen' >OK</span>"
            Else
                lblShipStatus.Text = "<span class='fontRed' >NOT OK</span>"

                For i = 0 To dgHistory.Items.Count - 1
                    If dgHistory.Items(i).Cells(6).Text.ToUpper = "NOT FOUND" Then
                        lblShipStatus.Text = "<span class='fontRed' >Lot Trace Data Missing! </span>"
                        Exit For
                    ElseIf dgHistory.Items(i).Cells(6).Text.ToUpper = "FAIL" Then
                        lblShipStatus.Text = "<span class='fontRed' >NOT OK</span>"
                    End If
                Next
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Sub GetSSNDetails(strSSN As String)
        Dim colParms As New List(Of SqlParameter)
        Dim colOutParms As List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter

        Try

            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@Data", SqlDbType.VarChar, 48)
            prmNext.Value = strSSN
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@SSN", SqlDbType.VarChar, 80)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@IBC", SqlDbType.VarChar, 80)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@ProdIDStatus", SqlDbType.VarChar, 80)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@SeatType", SqlDbType.VarChar, 80)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@SeatDesc", SqlDbType.VarChar, 80)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@SeatStyle", SqlDbType.VarChar, 80)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@ColorDesc", SqlDbType.VarChar, 80)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)

            'OUT--------------------------------------------------------
            colOutParms = DA.ExecSP("procGetProductDetails", colParms)

            For Each oParameter In colOutParms
                With oParameter
                    If .Direction = ParameterDirection.Output Then

                        Select Case .ParameterName

                            Case "@SSN"
                                lblSSN.Text = oParameter.Value.ToString()
                                lblPrintSSN.Text = lblSSN.Text
                                hidlblSSN.Value = lblSSN.Text
                            Case "@IBC"
                                lblProductID.Text = oParameter.Value.ToString()
                                hidlblProductID.Value = lblProductID.Text
                            Case "@SeatType"
                                lblSeatType.Text = oParameter.Value.ToString()
                                hidlblSeatType.Value = lblSeatType.Text
                            Case "@SeatDesc"
                                lblSeatDesc.Text = oParameter.Value.ToString()
                                hidlblSeatDesc.Value = lblSeatDesc.Text
                            Case "@SeatStyle"
                                lblSeatStyle.Text = oParameter.Value.ToString()
                                hidlblSeatStyle.Value = lblSeatStyle.Text
                            Case "@ColorDesc"
                                lblColorDesc.Text = oParameter.Value.ToString()
                                hidlblColorDesc.Value = lblColorDesc.Text
                        End Select

                    End If
                End With
            Next

            'lblSSN.Text = strSSN

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SaveModifiedProductHistory()
        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter

        Try
            'if the Torque and Angle values are empty, make them zeros
            txtTorque1.Text = If(txtTorque1.Text = "", "0", txtTorque1.Text)
            txtTorque2.Text = If(txtTorque2.Text = "", "0", txtTorque2.Text)
            txtTorque3.Text = If(txtTorque3.Text = "", "0", txtTorque3.Text)
            txtTorque4.Text = If(txtTorque4.Text = "", "0", txtTorque4.Text)
            txtTorque5.Text = If(txtTorque5.Text = "", "0", txtTorque5.Text)
            txtTorque6.Text = If(txtTorque6.Text = "", "0", txtTorque6.Text)
            txtTorque7.Text = If(txtTorque7.Text = "", "0", txtTorque7.Text)
            txtTorque8.Text = If(txtTorque8.Text = "", "0", txtTorque8.Text)
            txtTorque9.Text = If(txtTorque9.Text = "", "0", txtTorque9.Text)
            txtTorque10.Text = If(txtTorque10.Text = "", "0", txtTorque10.Text)

            txtAngle1.Text = If(txtAngle1.Text = "", "0", txtAngle1.Text)
            txtAngle2.Text = If(txtAngle2.Text = "", "0", txtAngle2.Text)
            txtAngle3.Text = If(txtAngle3.Text = "", "0", txtAngle3.Text)
            txtAngle4.Text = If(txtAngle4.Text = "", "0", txtAngle4.Text)
            txtAngle5.Text = If(txtAngle5.Text = "", "0", txtAngle5.Text)
            txtAngle6.Text = If(txtAngle6.Text = "", "0", txtAngle6.Text)
            txtAngle7.Text = If(txtAngle7.Text = "", "0", txtAngle7.Text)
            txtAngle8.Text = If(txtAngle8.Text = "", "0", txtAngle8.Text)
            txtAngle9.Text = If(txtAngle9.Text = "", "0", txtAngle9.Text)
            txtAngle10.Text = If(txtAngle10.Text = "", "0", txtAngle10.Text)

            'Ensure user did not clear initialized fields
            If hidOperationType.Value.ToUpper = "TORQUE" And txtTorque1.Text = "0" Then
                Master.eMsg("At Least 1 Torque Must Be Entered.")
            Else
                prmNext = New Data.SqlClient.SqlParameter("@SerialNumber", SqlDbType.VarChar, 32)

                prmNext.Value = ddlSSN.SelectedItem.ToString()
                colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@StationID", SqlDbType.VarChar, 4)
                    prmNext.Value = hidStationID.Value
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@StyleGroupID", SqlDbType.Int)
                    prmNext.Value = hidStyleGroupID.Value
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@OperationType", SqlDbType.VarChar, 50)
                    prmNext.Value = hidOperationType.Value
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@OperationName", SqlDbType.VarChar, 80)
                    prmNext.Value = hidOperationName.Value
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@RequirementData", SqlDbType.VarChar, 50)
                    prmNext.Value = hidRequirementData.Value
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@sData1", SqlDbType.VarChar, 82)
                    prmNext.Value = txtComponentScan.Text.ToUpper
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData1", SqlDbType.Real)
                    prmNext.Value = txtTorque1.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData2", SqlDbType.Real)
                    prmNext.Value = txtTorque2.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData3", SqlDbType.Real)
                    prmNext.Value = txtTorque3.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData4", SqlDbType.Real)
                    prmNext.Value = txtTorque4.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData5", SqlDbType.Real)
                    prmNext.Value = txtTorque5.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData6", SqlDbType.Real)
                    prmNext.Value = txtTorque6.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData7", SqlDbType.Real)
                    prmNext.Value = txtTorque7.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData8", SqlDbType.Real)
                    prmNext.Value = txtTorque8.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData9", SqlDbType.Real)
                    prmNext.Value = txtTorque9.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData10", SqlDbType.Real)
                    prmNext.Value = txtTorque10.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData11", SqlDbType.Real)
                    prmNext.Value = txtAngle1.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData12", SqlDbType.Real)
                    prmNext.Value = txtAngle2.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData13", SqlDbType.Real)
                    prmNext.Value = txtAngle3.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData14", SqlDbType.Real)
                    prmNext.Value = txtAngle4.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData15", SqlDbType.Real)
                    prmNext.Value = txtAngle5.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData16", SqlDbType.Real)
                    prmNext.Value = txtAngle6.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData17", SqlDbType.Real)
                    prmNext.Value = txtAngle7.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData18", SqlDbType.Real)
                    prmNext.Value = txtAngle8.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData19", SqlDbType.Real)
                    prmNext.Value = txtAngle9.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@rData20", SqlDbType.Real)
                    prmNext.Value = txtAngle10.Text
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@EditedBy", SqlDbType.VarChar, 82)
                    prmNext.Value = Session("UserName")
                    colParms.Add(prmNext)
                    '---------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@ResultValid", SqlDbType.Bit)
                    prmNext.Value = If(hidResultValid.Value.ToUpper() = "PASS", True, False)
                    colParms.Add(prmNext)

                    DA.ExecSP("procSGManualInsertLineData", colParms)


                    'CLEAR ENTRIES
                    txtComponentScan.Text = ""

                    txtTorque1.Text = "0"
                    txtTorque2.Text = "0"
                    txtTorque3.Text = "0"
                    txtTorque4.Text = "0"
                    txtTorque5.Text = "0"
                    txtTorque6.Text = "0"
                    txtTorque7.Text = "0"
                    txtTorque8.Text = "0"
                    txtTorque9.Text = "0"
                    txtTorque10.Text = "0"

                    txtAngle1.Text = "0"
                    txtAngle2.Text = "0"
                    txtAngle3.Text = "0"
                    txtAngle4.Text = "0"
                    txtAngle5.Text = "0"
                    txtAngle6.Text = "0"
                    txtAngle7.Text = "0"
                    txtAngle8.Text = "0"
                    txtAngle9.Text = "0"
                    txtAngle10.Text = "0"
                End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SetSSSNDetailPanelsVisibility(bVisible As Boolean)
        Try
            pnlShipStatus.Visible = bVisible
            pnlSSNDetails.Visible = bVisible
            pnlSSNDataGrid.Visible = bVisible
            cmdPrint.Visible = bVisible
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub GetAvailableTaskStatusValues()
        Dim colParms As New List(Of SqlParameter)
        Dim param As Data.SqlClient.SqlParameter
        Dim ds As DataSet

        Try
            Me.rblTaskValuesAuxT.Items.Clear()
            Me.rblTaskValuesComp.Items.Clear()
            Me.rblTaskValuesTorq.Items.Clear()

            param = New Data.SqlClient.SqlParameter("@ParameterListID ", SqlDbType.VarChar, 4)
            param.Value = "2"   ' from [tblParameterLists]  the Pass/Fail list
            colParms.Add(param)
            ds = DA.GetDataSet("procGetParameterListValues", colParms)

            Me.rblTaskValuesAuxT.DataSource = ds
            Me.rblTaskValuesAuxT.DataTextField = "ParameterListValue"
            Me.rblTaskValuesAuxT.DataValueField = "ParameterListValue"
            Me.rblTaskValuesAuxT.DataBind()

            Me.rblTaskValuesComp.DataSource = ds
            Me.rblTaskValuesComp.DataTextField = "ParameterListValue"
            Me.rblTaskValuesComp.DataValueField = "ParameterListValue"
            Me.rblTaskValuesComp.DataBind()

            Me.rblTaskValuesTorq.DataSource = ds
            Me.rblTaskValuesTorq.DataTextField = "ParameterListValue"
            Me.rblTaskValuesTorq.DataValueField = "ParameterListValue"
            Me.rblTaskValuesTorq.DataBind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlSSN_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSSN.SelectedIndexChanged
        Try
            SetSSSNDetailPanelsVisibility(False)
            LoadShipStatusAndDetails(ddlSSN.SelectedItem.ToString())
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
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

    Private Sub cmdPrint_Click(sender As Object, e As EventArgs) Handles cmdPrint.Click
        SendPrintJob()
    End Sub

    Private Sub SendPrintJob()

        Dim strinsert As String = ""

        Try

            HeadRest_Label_Print(hidlblSSN.Value, strHumanReadable, strSeatCode, strSeatPartNumber, hidSelectedPrinterPrelim.Value, Page.User.Identity.Name.ToString, ddlLabelTypes.SelectedValue)

            strinsert = "Insert into tblLabelPrintHistory " &
               "(LABEL_SEAT_CODE,LABEL_SEAT_PART_NUMBER," &
               "LABEL_HUMAN_READABLE,LOT_TRACE_DT," &
               "SEAT_SERIAL_NUMBER,RECORDED_BY,PRINT_LOCATION,PRINTED_BY) Values " &
               "('" & UCase(strSeatCode) & "','" & UCase(strSeatPartNumber) & "','" &
               UCase(strHumanReadable) & "','" & Now & "','" &
               UCase(hidlblSSN.Value) & "','" & Page.User.Identity.Name.ToString() & "','" &
               hidSelectedPrinterPrelim.Value & "','" & "')"
            DA.ExecSQL(strinsert)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Function HeadRest_Label_Print(ByVal SerialNumber As String, ByVal HumanReadableText As String, ByVal SeatCode As String, ByVal SeatPartNumber As String, ByVal QueueName As String, ByVal Requestor As String, ByVal LabelType As String) As Boolean

        Dim strDelim As String = "|"
        Dim strQuantity As String = "1"
        Dim str As String
        Dim LabelTypeCode As String = ""
        Dim ds As DataSet
        'Dim psi As New ProcessStartInfo

        str = "SELECT TOP 1 LabelParameterValue FROM tblLabelParameters WHERE LabelID=1 AND [LabelParameterTypeID]='0007'"
        ds = DA.GetDataSet(str)
        If ds.Tables(0).Rows.Count = 0 Then
            'Pre-Vetted
        Else
            LabelTypeCode = ds.Tables(0).Rows(0).Item("LabelParameterValue").ToString()
        End If

        ds.Clear()

        str = "SELECT TOP 1 SEAT_SERIAL_NUMBER, LABEL_HUMAN_READABLE, LABEL_SEAT_CODE, LABEL_SEAT_PART_NUMBER, LOT_TRACE_DT FROM tblLabelPrintHistory WHERE (SEAT_SERIAL_NUMBER = '" & (hidlblSSN.Value) & "')ORDER BY LOT_TRACE_DT DESC"
        ds = DA.GetDataSet(str)

        If ds.Tables(0).Rows.Count = 0 Then
            'Pre-Vetted
        Else
            strHumanReadable = ds.Tables(0).Rows(0).Item("LABEL_HUMAN_READABLE").ToString() & ""
            strSeatCode = ds.Tables(0).Rows(0).Item("LABEL_SEAT_CODE").ToString() & ""
            strSeatPartNumber = ds.Tables(0).Rows(0).Item("LABEL_SEAT_PART_NUMBER").ToString() & ""
        End If

        hidPrintString.Value = SerialNumber & strDelim & strHumanReadable & strDelim & strSeatCode & strDelim & strSeatPartNumber & strDelim & QueueName & strDelim & Requestor & strDelim & LabelTypeCode & strDelim & strQuantity

        'Master.Msg = "1 Label was sent to printer"

        Return True
    End Function
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

                    Dim paramProductId As New SqlParameter("@ProductID", SqlDbType.VarChar)
                    paramProductId.Size = 100
                    paramProductId.Value = hidlblProductID.Value 'ddlSeatStyle.SelectedValue

                    Dim paramProdQty As New SqlParameter("@ProductID_Quantity", SqlDbType.Int)
                    paramProdQty.Value = CType(1, Integer)

                    objCmd.Parameters.Add(param2)
                    objCmd.Parameters.Add(param3)
                    objCmd.Parameters.Add(par4)
                    objCmd.Parameters.Add(paramProductId)
                    objCmd.Parameters.Add(paramProdQty)


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


    Private Sub populateLabelTypeDropDown()
        Dim colParameters As New List(Of SqlParameter)

        Try

            ddlLabelTypes.DataTextField = "description"
            ddlLabelTypes.DataValueField = "labelID"
            ddlLabelTypes.DataSource = DA.GetDataSet("procGetLabels", colParameters)
            ddlLabelTypes.DataBind()
            ddlLabelTypes.Items.Insert(0, New ListItem("Select Label Type", ""))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region

End Class