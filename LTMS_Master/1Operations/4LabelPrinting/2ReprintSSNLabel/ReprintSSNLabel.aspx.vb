Public Class ReprintSSNLabel
    Inherits System.Web.UI.Page


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'tSeatSerialNum.Attributes.Add("onKeyUp", "SSN()")

            If Not IsPostBack() Then
                ddPrinters_DataBind()
                NCopies()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ReprintSSNLabel_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        EnableControls()
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        Try
            Dim strinsert As String
            ' HeadRest_Label_Print(tSeatSerialNum.Text, tComments.Text, tSeatCode.Text, tSeatPartNum.Text, ddPrinters.SelectedItem.Text, Page.User.Identity.Name.ToString, CStr(1))
            HeadRest_Label_Print(tSeatSerialNum.Text, tComments.Text, tSeatCode.Text, tSeatPartNum.Text, ddPrinters.SelectedItem.Text, Page.User.Identity.Name.ToString, tNCopies.Text)

            strinsert = "Insert into tblLabelPrintHistory " & _
               "(LABEL_SEAT_CODE,LABEL_SEAT_PART_NUMBER," & _
               "LABEL_HUMAN_READABLE,LOT_TRACE_DT," & _
               "SEAT_SERIAL_NUMBER,RECORDED_BY,PRINT_LOCATION,PRINTED_BY) Values " & _
               "('" & UCase(tSeatCode.Text) & "','" & UCase(tSeatPartNum.Text) & "','" & _
               UCase(tComments.Text) & "','" & Now & "','" & _
               UCase(tSeatSerialNum.Text) & "','" & Page.User.Identity.Name.ToString() & "','" & _
               ddPrinters.SelectedItem.Value & "','" & "')"
            DA.ExecSQL(strinsert)
            CheckSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            'Msg = "Error: Label is not sent to printer"
        End Try
    End Sub

    Private Sub tSeatSerialNum_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tSeatSerialNum.TextChanged
        Try
            If CheckSSN() = False Then
                cmdPrint.Enabled = False
                ddPrinters.SelectedIndex = 0
            Else
                ddPrinters.SelectedIndex = 0
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddPrinters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddPrinters.SelectedIndexChanged
        Try
            If ddPrinters.SelectedItem.Value = "" Then
                cmdPrint.Enabled = False
            Else
                If CheckSSN() = True Then
                    'scr.Secure = cmdPrint
                Else
                    cmdPrint.Enabled = False
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region

#Region "Methods"

    Private Sub EnableControls()
        Master.Secure(Me.cmdPrint)
        Master.Secure(Me.ddPrinters)

        If ddPrinters.SelectedItem.Value = "" Then
            cmdPrint.Enabled = False
        End If

    End Sub


    Private Sub ddPrinters_DataBind()
        Dim ds As New DataSet
        ds = DA.GetDataSet("Select Name, NetworkPath from tblLabelPrinters Where LabelPrinter = 1")
        ddPrinters.DataSource = ds
        ddPrinters.DataTextField = "Name"
        ddPrinters.DataValueField = "NetworkPath"
        ddPrinters.DataBind()

        Dim i As Integer
        Dim foundindex As Integer
        Dim strPrinterName As String
        'retrieve default printer name
        strPrinterName = BizLayer.GetApplicationParameterValue("2231", "0001").Trim()
        'find printer name by one configured in application parameter
        If Not ds Is Nothing Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                If Trim(CStr(ds.Tables(0).Rows(i).Item(0))) = strPrinterName Then
                    foundindex = i
                    Exit For
                End If
            Next
        End If
        'select the value if it exists.
        ddPrinters.SelectedIndex = foundindex

    End Sub

    Private Function HeadRest_Label_Print(ByVal SerialNumber As String, ByVal HumanReadableText As String, ByVal SeatCode As String, ByVal SeatPartNumber As String, ByVal QueueName As String, ByVal Requestor As String, ByVal LabelType As String) As Boolean
        Dim strDelim As String = "|"
        Dim strQuantity As String = "1"
        Dim str As String
        Dim LabelTypeCode As String = ""
        Dim ds As DataSet

        If IsNumeric(tNCopies.Text) Then
            strQuantity = CStr(CType(tNCopies.Text, Int32))
        Else
            strQuantity = "1"
        End If

        str = "SELECT TOP 1 LabelParameterValue FROM tblLabelParameters WHERE LabelID=1 AND [LabelParameterTypeID]='0007'"
        ds = DA.GetDataSet(str)
        If ds.Tables(0).Rows.Count = 0 Then
            'fix the data
        Else
            LabelTypeCode = ds.Tables(0).Rows(0).Item("LabelParameterValue").ToString()
        End If

        ds.Clear()

        hidPrintString.Value = SerialNumber & strDelim & HumanReadableText & strDelim & SeatCode & strDelim & SeatPartNumber & strDelim & QueueName & strDelim & Requestor & strDelim & LabelTypeCode & strDelim & strQuantity

        Return True
    End Function

    Private Sub NCopies()
        tNCopies.Text = BizLayer.GetApplicationParameterValue("0112", "0001")
    End Sub

    Private Function CheckSSN() As Boolean
        Dim str As String
        Dim ds As DataSet
        str = "SELECT SEAT_SERIAL_NUMBER, LABEL_HUMAN_READABLE, LABEL_SEAT_CODE, LABEL_SEAT_PART_NUMBER, LOT_TRACE_DT FROM tblLabelPrintHistory WHERE (SEAT_SERIAL_NUMBER = '" & (tSeatSerialNum.Text) & "')ORDER BY LOT_TRACE_DT DESC"
        ds = DA.GetDataSet(str)
        If ds.Tables(0).Rows.Count = 0 Then
            tComments.Text = ""
            tSeatCode.Text = ""
            tSeatPartNum.Text = ""
            lblInvalisSSN.Visible = True
            Return False    'false
        Else
            lblInvalisSSN.Visible = False
            tComments.Text = ds.Tables(0).Rows(0).Item("LABEL_HUMAN_READABLE").ToString() & ""
            tSeatCode.Text = ds.Tables(0).Rows(0).Item("LABEL_SEAT_CODE").ToString() & ""
            tSeatPartNum.Text = ds.Tables(0).Rows(0).Item("LABEL_SEAT_PART_NUMBER").ToString() & ""
            Return True    'ok
        End If

    End Function


#End Region



End Class