Imports System.Data.SqlClient

Public Class LotTraceDataTransfer
    Inherits System.Web.UI.Page

    Dim ds As DataSet
    Dim dsFrom As DataSet
    Dim dsTo As DataSet
    Dim bPreVetted As Boolean


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                cmdTransfer.Enabled = False
                txtSSNFrom.Focus()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LotTraceDataTransfer_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtSSNFrom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSSNFrom.TextChanged
        Try
            EvaluatetxtSSNFromText()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub txtSSNTo_TextChanged(sender As Object, e As System.EventArgs) Handles txtSSNTo.TextChanged
        Try
            EvaluatetxtSSNToText()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdTransfer_Click(sender As Object, e As System.EventArgs) Handles cmdTransfer.Click
        Try
            bPreVetted = True
            DoTransfer()
            bPreVetted = False
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dgProdOperHistFrom_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgProdOperHistFrom.ItemDataBound

        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                If e.Item.ItemIndex <> -1 Then

                    hidStyleGroupIDFrom.Value = CType(e.Item.DataItem, DataRowView).Row("StyleGroupID").ToString()

                End If

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dgProdOperHistTo_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgProdOperHistTo.ItemDataBound

        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                If e.Item.ItemIndex <> -1 Then

                    hidStyleGroupIDTo.Value = CType(e.Item.DataItem, DataRowView).Row("StyleGroupID").ToString()

                End If

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Sub DoTransfer()
        Dim colParms As New List(Of SqlParameter)
        Dim colOutParms As List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim bResult As Boolean = False

        Try

            If hidStyleGroupIDFrom.Value = hidStyleGroupIDTo.Value Then

                prmNext = New Data.SqlClient.SqlParameter("@OldProductID", SqlDbType.VarChar, 48)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = txtSSNFrom.Text.ToUpper
                colParms.Add(prmNext)

                prmNext = New Data.SqlClient.SqlParameter("@NewProductID", SqlDbType.VarChar, 48)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = txtSSNTo.Text.ToUpper
                colParms.Add(prmNext)
                '------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@TransferStatus", SqlDbType.VarChar, 48)
                prmNext.Direction = ParameterDirection.Output
                colParms.Add(prmNext)
                '------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ErrorMsg", SqlDbType.VarChar, 48)
                prmNext.Direction = ParameterDirection.Output
                colParms.Add(prmNext)
                '------------------------------------------------------

                colOutParms = DA.ExecSP("procSGLotTraceDataTransfer", colParms)

                For Each oParameter In colOutParms
                    With oParameter
                        If .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                            Master.Msg = oParameter.Value.ToString()
                        ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@TransferStatus" Then
                            Master.Msg = "Transfer Successful."
                            EvaluatetxtSSNToText()
                            Exit For
                        End If
                    End With
                Next

            Else
                Master.Msg = "The SSN's must be from the same Style Group!"
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub EnableControls()
        Try
            Dim bFrom As Boolean
            Dim bTo As Boolean

            Boolean.TryParse(hidFrom.Value, bFrom)
            Boolean.TryParse(hidTo.Value, bTo)

            Master.Secure(cmdTransfer)

            If (bFrom And bTo) Then
                'only set to true if Master.Secure already set it to true
                cmdTransfer.Enabled = cmdTransfer.Enabled And True
            Else
                cmdTransfer.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub EvaluatetxtSSNFromText()
        Try
            Dim bFrom As Boolean = False
            Dim strError As String = ""
            Dim lblMessage As String = ""
            Dim bUsedgProdOperHist As Boolean = False

            hidFrom.Value = bFrom.ToString()

            If (GetLotData(txtSSNFrom.Text.ToUpper, strError, bUsedgProdOperHist) = False Or (txtSSNFrom.Text.Count <> 11)) Then
                bFrom = False
                hidFrom.Value = bFrom.ToString()
                dgProdOperHistFrom.Controls.Clear()
                dgProdOperHistTo.Controls.Clear()
                pnlProdOperHistFrom.Visible = False
                pnlProdOperHistTo.Visible = False
                pErrFrom.Visible = True
                lblMsgFrom.Visible = True
                txtSSNTo.Text = ""
            Else
                bFrom = True
                hidFrom.Value = bFrom.ToString()
                pErrFrom.Visible = False
                lblMsgFrom.Visible = False
                dgProdOperHistTo.Controls.Clear()
                pnlProdOperHistTo.Visible = False
                pnlProdOperHistFrom.Visible = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub EvaluatetxtSSNToText()
        Try
            Dim bTo As Boolean
            Dim strError As String = ""
            Dim bUsedgProdOperHist As Boolean = True


            If txtSSNFrom.Text = "" Then
                txtSSNTo.Text = ""
                Master.Msg = "Please, enter FROM first."
                lblPeaseEnterSSN.Visible = True
                dgProdOperHistTo.Controls.Clear()
                pnlProdOperHistTo.Visible = False
                hidTo.Value = "False"
            Else
                If (GetLotData(txtSSNTo.Text.ToUpper, strError, bUsedgProdOperHist) = False Or (txtSSNTo.Text.Count <> 11)) Then
                    bTo = False
                    hidTo.Value = bTo.ToString()
                    dgProdOperHistTo.Controls.Clear()
                    pnlProdOperHistTo.Visible = False
                    lblMsgTo.Visible = True
                    pErrTo.Visible = True
                    lblPeaseEnterSSN.Visible = True
                Else
                    bTo = True
                    hidTo.Value = bTo.ToString()
                    lblMsgTo.Visible = False
                    pErrTo.Visible = False
                    pnlProdOperHistTo.Visible = True

                    If hidStyleGroupIDFrom.Value <> hidStyleGroupIDTo.Value Then
                        Master.Msg = "The SSN's must be from the same Style Group!"
                        cmdTransfer.Enabled = False
                        bTo = False
                        hidTo.Value = bTo.ToString()
                    Else
                        cmdTransfer.Enabled = True
                        bTo = True
                        hidTo.Value = bTo.ToString()
                    End If

                End If

            End If

            If txtSSNFrom.Text.Trim <> "" And txtSSNTo.Text.Trim <> "" Then
                lblPeaseEnterSSN.Visible = False
            Else
                lblPeaseEnterSSN.Visible = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Function GetLotData(ByRef strSSN As String, ByRef strErr As String, ByRef bWhich As Boolean) As Boolean
        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim ds As DataSet
        Dim bResult As Boolean = False

        Try

            If bWhich = True Then
                dgProdOperHistTo.DataSource = Nothing
                dgProdOperHistTo.DataBind()
            Else
                dgProdOperHistFrom.DataSource = Nothing
                dgProdOperHistFrom.DataBind()
            End If

            prmNext = New Data.SqlClient.SqlParameter("@SerialNumber", SqlDbType.VarChar, 32)
            prmNext.Value = strSSN
            colParms.Add(prmNext)
            ds = DA.GetDataSet("procSGGetProductRequirements", colParms)
            colParms.Clear()

            If (ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0) Then
                bResult = False
                strErr = "Error: Please enter valid serial number!"
                Master.eMsg(strErr)
            Else
                If bWhich = True Then
                    dgProdOperHistTo.DataSource = ds
                    dgProdOperHistTo.DataBind()
                    If ds.Tables(0).Rows(0).ItemArray(7).ToString = "Not Found" Or bPreVetted Then
                        bResult = True
                    Else
                        bResult = False
                        strErr = "Error: Please enter a serial number that doesn't already exist!"
                        Master.eMsg(strErr)
                    End If
                Else
                    dgProdOperHistFrom.DataSource = ds
                    dgProdOperHistFrom.DataBind()
                    bResult = True
                End If
            End If

            If txtSSNFrom.Text.Trim <> "" And txtSSNTo.Text.Trim <> "" Then
                lblPeaseEnterSSN.Visible = False
            Else
                lblPeaseEnterSSN.Visible = True
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

#End Region

End Class