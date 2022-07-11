Imports System.Data.SqlClient

Public Class ShippingLabel
    Inherits System.Web.UI.Page

    Dim driverLotTraceMsg As New ArrayList
    Dim driverCheckShipMsg As New ArrayList
    Dim passLotTraceMsg As New ArrayList
    Dim passCheckShipMsg As New ArrayList
    Dim rearBackLotTraceMsg As New ArrayList
    Dim rearBackCheckShipMsg As New ArrayList
    Dim Comp4LotTraceMsg As New ArrayList
    Dim Comp4CheckShipMsg As New ArrayList
    Dim Comp5LotTraceMsg As New ArrayList
    Dim Comp5CheckShipMsg As New ArrayList
    Dim Comp6LotTraceMsg As New ArrayList
    Dim Comp6CheckShipMsg As New ArrayList
    Dim Comp7LotTraceMsg As New ArrayList
    Dim Comp7CheckShipMsg As New ArrayList
    Dim Comp8LotTraceMsg As New ArrayList
    Dim Comp8CheckShipMsg As New ArrayList
    Dim Comp9LotTraceMsg As New ArrayList
    Dim Comp9CheckShipMsg As New ArrayList
    Dim Comp10LotTraceMsg As New ArrayList
    Dim Comp10CheckShipMsg As New ArrayList
    Dim Comp11LotTraceMsg As New ArrayList
    Dim Comp11CheckShipMsg As New ArrayList
    Dim Comp12LotTraceMsg As New ArrayList
    Dim Comp12CheckShipMsg As New ArrayList
    Dim Comp13LotTraceMsg As New ArrayList
    Dim Comp13CheckShipMsg As New ArrayList

    Dim bShipStatus As Boolean = False

    Dim strShipStatus As String = ""
    Dim strTempShipStatus As String = ""

    Dim bDriverFound As Boolean
    Dim bPassFound As Boolean
    Dim bRearBackFound As Boolean
    Dim bComp4Found As Boolean
    Dim bComp5Found As Boolean
    Dim bComp6Found As Boolean
    Dim bComp7Found As Boolean
    Dim bComp8Found As Boolean
    Dim bComp9Found As Boolean
    Dim bComp10Found As Boolean
    Dim bComp11Found As Boolean
    Dim bComp12Found As Boolean
    Dim bComp13Found As Boolean
    'Dim hidDupSSN As String
    'Dim hidDupDT As String

    Private DupSsnPeriod As String = BizLayer.GetApplicationParameterValue("2233", "0001")

    'Enum SerialNumberType
    '    Driver
    '    Passenger
    '    RearBack
    '    RearCushion
    'End Enum

#Region "Event Handlers"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack() Then
                ddPrinters_DataBind()
                HidePrintDupSSNwaring()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ShippingLabel_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    'Private Sub HeadRest_Label_Print() 'ByVal SerialNumber As String, ByVal HumanReadableText As String, ByVal SeatCode As String, ByVal SeatPartNumber As String, ByVal QueueName As String, ByVal Requestor As String, ByVal LabelType As String) As Boolean
    '    Dim strDelim As String = "|"
    '    Dim strQuantity As String = "1"
    '    Dim str As String
    '    Dim LabelTypeCode As String = ""
    '    Dim ds As DataSet

    '    strQuantity = "1"

    '    str = "SELECT TOP 1 LabelParameterValue FROM tblLabelParameters WHERE LabelID=1 AND [LabelParameterTypeID]='0007'"
    '    ds = DA.GetDataSet(str)
    '    If ds.Tables(0).Rows.Count = 0 Then
    '        'fix the data
    '    Else
    '        LabelTypeCode = ds.Tables(0).Rows(0).Item("LabelParameterValue").ToString()
    '    End If

    '    ds.Clear()

    '    hidPrintString.Value = SerialNumber & strDelim & HumanReadableText & strDelim & SeatCode & strDelim & SeatPartNumber & strDelim & QueueName & strDelim & Requestor & strDelim & LabelTypeCode & strDelim & strQuantity

    '    Return True
    'End Sub

    Private Sub txtDriverSeatSerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtDriverSeatSerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtPassSeatSerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtPassSeatSerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtRearBackSerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtRearBackSerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Sub txtComp4SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp4SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp5SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp5SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp6SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp6SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp7SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp7SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp8SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp8SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp9SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp9SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp10SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp10SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp11SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp11SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp12SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp12SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtComp13SerialNum_TextChanged(sender As Object, e As System.EventArgs) Handles txtComp13SerialNum.TextChanged
        Try
            HidePrintDupSSNwaring()
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddPrinters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddPrinters.SelectedIndexChanged
        Try
            CheckAllSeatSSN()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub



    Private Sub cmdPrint_Click(sender As Object, e As System.EventArgs) Handles cmdPrint.Click
        Try
            'pb - 06/21/04
            'added check for duplicate ssn's
            Dim dupDriverSSNdt As String = ""
            Dim dupPassSSNdt As String = ""
            Dim dupRearBackSSNdt As String = ""
            Dim dupComponent4SSNdt As String = ""
            Dim dupComponent5SSNdt As String = ""
            Dim dupComponent6SSNdt As String = ""
            Dim dupComponent7SSNdt As String = ""
            Dim dupComponent8SSNdt As String = ""
            Dim dupComponent9SSNdt As String = ""
            Dim dupComponent10SSNdt As String = ""
            Dim dupComponent11SSNdt As String = ""
            Dim dupComponent12SSNdt As String = ""
            Dim dupComponent13SSNdt As String = ""

            hidDupSSN.Value = ""
            hidDupDT.Value = ""

            dupDriverSSNdt = CheckForDuplicateSSNs(Me.txtDriverSeatSerialNum.Text)
            dupPassSSNdt = CheckForDuplicateSSNs(Me.txtPassSeatSerialNum.Text)
            dupRearBackSSNdt = CheckForDuplicateSSNs(Me.txtRearBackSerialNum.Text)
            dupComponent4SSNdt = CheckForDuplicateSSNs(Me.txtComp4SerialNum.Text)
            dupComponent5SSNdt = CheckForDuplicateSSNs(Me.txtComp5SerialNum.Text)
            dupComponent6SSNdt = CheckForDuplicateSSNs(Me.txtComp6SerialNum.Text)
            dupComponent7SSNdt = CheckForDuplicateSSNs(Me.txtComp7SerialNum.Text)
            dupComponent8SSNdt = CheckForDuplicateSSNs(Me.txtComp8SerialNum.Text)
            dupComponent9SSNdt = CheckForDuplicateSSNs(Me.txtComp9SerialNum.Text)
            dupComponent10SSNdt = CheckForDuplicateSSNs(Me.txtComp10SerialNum.Text)
            dupComponent11SSNdt = CheckForDuplicateSSNs(Me.txtComp11SerialNum.Text)
            dupComponent12SSNdt = CheckForDuplicateSSNs(Me.txtComp12SerialNum.Text)
            dupComponent13SSNdt = CheckForDuplicateSSNs(Me.txtComp13SerialNum.Text)


            If (dupDriverSSNdt.Length > 0 And dupDriverSSNdt <> " ") Then
                Me.lblPrintDupSSN.Text = "WARNING: Driver SSN has already been shipped!<br>"
            End If
            If (dupPassSSNdt.Length > 0 And dupPassSSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Passenger SSN has already been shipped!<br>"
            End If
            If (dupRearBackSSNdt.Length > 0 And dupRearBackSSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Rear Back SSN has already been shipped!<br>"
            End If
            If (dupComponent4SSNdt.Length > 0 And dupComponent4SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Rear Cushion SSN has already been shipped!<br>"
            End If
            If (dupComponent5SSNdt.Length > 0 And dupComponent5SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Rear Back 40% SSN has already been shipped!<br>"
            End If
            If (dupComponent6SSNdt.Length > 0 And dupComponent6SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Rear Back 60% SSN has already been shipped!<br>"
            End If
            If (dupComponent7SSNdt.Length > 0 And dupComponent7SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Rear Back 100% SSN has already been shipped!<br>"
            End If
            If (dupComponent8SSNdt.Length > 0 And dupComponent8SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Armrest RFID SSN has already been shipped!<br>"
            End If
            If (dupComponent9SSNdt.Length > 0 And dupComponent9SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Component 9 SSN has already been shipped!<br>"
            End If
            If (dupComponent10SSNdt.Length > 0 And dupComponent10SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Component 10 SSN has already been shipped!<br>"
            End If
            If (dupComponent11SSNdt.Length > 0 And dupComponent11SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Component 11 SSN has already been shipped!<br>"
            End If
            If (dupComponent12SSNdt.Length > 0 And dupComponent12SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Component 12 SSN has already been shipped!<br>"
            End If
            If (dupComponent13SSNdt.Length > 0 And dupComponent13SSNdt <> " ") Then
                Me.lblPrintDupSSN.Text += "WARNING: Component 13 SSN has already been shipped!<br>"
            End If

            If (dupDriverSSNdt.Length > 0 Or dupPassSSNdt.Length > 0 Or dupRearBackSSNdt.Length > 0 Or dupComponent4SSNdt.Length > 0 _
                    Or dupComponent5SSNdt.Length > 0 Or dupComponent6SSNdt.Length > 0 Or dupComponent7SSNdt.Length > 0 _
                    Or dupComponent8SSNdt.Length > 0 Or dupComponent9SSNdt.Length > 0 Or dupComponent10SSNdt.Length > 0 _
                    Or dupComponent11SSNdt.Length > 0 Or dupComponent12SSNdt.Length > 0 Or dupComponent13SSNdt.Length > 0) Then
                If (dupDriverSSNdt <> " " Or dupPassSSNdt <> " " Or dupRearBackSSNdt <> " " Or dupComponent4SSNdt <> " " _
                    Or dupComponent5SSNdt <> " " Or dupComponent6SSNdt <> " " Or dupComponent7SSNdt <> " " _
                    Or dupComponent8SSNdt <> " " Or dupComponent9SSNdt <> " " Or dupComponent10SSNdt <> " " _
                    Or dupComponent11SSNdt <> " " Or dupComponent12SSNdt <> " " Or dupComponent13SSNdt <> " ") Then

                    'display label, buttons, and exit (do not print yet)
                    Me.lblPrintDupSSN.Text += "ARE YOU SURE YOU WANT TO PRINT LABEL WITH DUPLICATE SSN(s)?"
                    Me.lblPrintDupSSN.Visible = True
                    Me.cmdPrintYes.Visible = True
                    Me.cmdPrintNo.Visible = True
                    Exit Sub
                End If
            End If

            'implied else
            PrintShippingLabel()
            InsertShippingLabel()
            HidePrintDupSSNwaring()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    'print confirmation (in case ssn's are duplicate). pb, 06/21/04
    Private Sub cmdPrintYes_Click(sender As Object, e As System.EventArgs) Handles cmdPrintYes.Click
        Try
            PrintShippingLabel()
            InsertShippingLabel()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    'print confirmation (in case ssn's are duplicate). pb, 06/21/04
    Private Sub cmdPrintNo_Click(sender As Object, e As System.EventArgs) Handles cmdPrintNo.Click
        Try
            'clear warning
            HidePrintDupSSNwaring()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region


#Region "Methods"

    Private Sub EnableControls()
        Dim bPrintEnabled = cmdPrint.Enabled

        Master.Secure(Me.cmdPrint)
        Master.Secure(Me.cmdPrintNo)
        Master.Secure(Me.cmdPrintYes)
        Master.Secure(Me.ddPrinters)

        If ddPrinters.SelectedItem.Value = "" Then
            cmdPrint.Enabled = False
            cmdPrintYes.Enabled = False
        End If

        'check if btn was previously disabled and keep that state.
        cmdPrint.Enabled = cmdPrint.Enabled And bPrintEnabled

    End Sub

    Private Sub HidePrintDupSSNwaring()
        'clear warning
        Me.lblPrintDupSSN.Text = ""
        Me.lblPrintDupSSN.Visible = False
        Me.cmdPrintNo.Visible = False
        Me.cmdPrintYes.Visible = False
    End Sub

    Sub ddPrinters_DataBind()
        Dim ds As New DataSet
        ds = DA.GetDataSet("Select [Name], [NetworkPath] from tblLabelPrinters Where LabelPrinter = 1")
        ddPrinters.DataSource = ds
        ddPrinters.DataTextField = "Name"
        ddPrinters.DataValueField = "NetworkPath"
        ddPrinters.DataBind()
        Dim i As Integer
        Dim foundindex As Integer
        Dim strPrinterName As String
        'retrieve default printer name
        strPrinterName = BizLayer.GetApplicationParameterValue("2232", "0001").Trim
        'find printer name by one configured in application parameter
        If Not ds Is Nothing Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i).Item(0).ToString().Trim() = strPrinterName Then
                    foundindex = i
                    Exit For
                End If
            Next
        End If
        'select the value if it exists.
        ddPrinters.SelectedIndex = foundindex

        'ddPrinters.Items.Insert(0, New ListItem("Select Printer", ""))
    End Sub

    Private Sub WriteMsg()
        Dim i As Integer
        tMessage.Text = ""
        lblShipStatus.Text = strShipStatus

        '  First print all Driver Serial number messages
        '   followed by all Passenger SSN Messages
        '   followed by all Rear Back SSN Messages
        '   followed by all REar Cushion SSN Messages

        For i = 0 To driverLotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & driverLotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To passLotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & passLotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To rearBackLotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & rearBackLotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp4LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp4LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp5LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp5LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp6LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp6LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp7LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp7LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp8LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp8LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp9LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp9LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp10LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp10LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp11LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp11LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp12LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp12LotTraceMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp13LotTraceMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp13LotTraceMsg(i).ToString() & vbCrLf
        Next

        '-------CheckShipMsg
        For i = 0 To driverCheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & driverCheckShipMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To passCheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & passCheckShipMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To rearBackCheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & rearBackCheckShipMsg(i).ToString() & vbCrLf
        Next

        For i = 0 To Comp4CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp4CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp5CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp5CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp6CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp6CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp7CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp7CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp8CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp8CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp9CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp9CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp10CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp10CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp11CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp11CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp12CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp12CheckShipMsg(i).ToString() & vbCrLf
        Next
        For i = 0 To Comp13CheckShipMsg.Count - 1
            tMessage.Text = tMessage.Text & Comp13CheckShipMsg(i).ToString() & vbCrLf
        Next


        If (driverLotTraceMsg.Count > 0) And (passLotTraceMsg.Count > 0) Then
            If (ddPrinters.SelectedIndex < 0) Then
                cmdPrint.Enabled = False
            ElseIf bShipStatus = False Then
                cmdPrint.Enabled = False
            Else
                cmdPrint.Enabled = True
            End If
        Else
            cmdPrint.Enabled = False
        End If


    End Sub


    Function ValidateSSN(ByVal SSN As String, ByRef strResult As String) As Boolean
        'Procedure(procCheckSerialNumber)
        '@Data varchar(48),
        '@CheckType VarChar(48),
        '@Status varchar(48) out, 
        '@Product_ID VarChar(48) Out, 
        '@SerialNumber VarChar(48) Out , 
        '@PLCProductCode  VarChar(8) Out as 

        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim colOutParms As List(Of SqlParameter)

        Dim bResult As Boolean = False

        'IN--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Data", SqlDbType.VarChar, 48)
        prmNext.Direction = ParameterDirection.Input
        prmNext.Value = SSN
        colParms.Add(prmNext)
        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@CheckType", SqlDbType.VarChar, 48)
        prmNext.Direction = ParameterDirection.Input
        prmNext.Value = 1
        colParms.Add(prmNext)
        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Status", SqlDbType.VarChar, 48)
        prmNext.Direction = ParameterDirection.Output
        colParms.Add(prmNext)
        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Product_ID", SqlDbType.VarChar, 48)
        prmNext.Direction = ParameterDirection.Output
        colParms.Add(prmNext)
        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@SerialNumber", SqlDbType.VarChar, 48)
        prmNext.Direction = ParameterDirection.Output
        colParms.Add(prmNext)
        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@PLCProductCode", SqlDbType.VarChar, 8)
        prmNext.Direction = ParameterDirection.Output
        colParms.Add(prmNext)
        '------------------------------------------------------------
        colOutParms = DA.ExecSP("procCheckSerialNumber", colParms)

        strResult = "Valid"
        bResult = True

        For Each oParameter In colOutParms
            With oParameter
                If .Direction = ParameterDirection.Output And .ParameterName = "@Status" And .Value.ToString() = "Invalid" Then

                    strResult = "Invalid"
                    bResult = False
                    Exit For
                End If
            End With
        Next

        'Add length check
        If txtDriverSeatSerialNum.Text = SSN Or txtPassSeatSerialNum.Text = SSN Or txtComp4SerialNum.Text = SSN Or txtComp6SerialNum.Text = SSN Or txtComp7SerialNum.Text = SSN Then
            If SSN.Length <> 11 Then
                strResult = "Invalid"
                bResult = False
            End If
        End If
        Return bResult

    End Function

    Private Sub CheckAllSeatSSN()
        Try
            Dim bLotTraceStatusDriverSSN As Boolean
            Dim bLotTraceStatusPassSSN As Boolean
            Dim bLotTraceStatusRearBackSSN As Boolean
            Dim bLotTraceStatusComp4SSN As Boolean
            Dim bLotTraceStatusComp5SSN As Boolean
            Dim bLotTraceStatusComp6SSN As Boolean
            Dim bLotTraceStatusComp7SSN As Boolean
            Dim bLotTraceStatusComp8SSN As Boolean
            Dim bLotTraceStatusComp9SSN As Boolean
            Dim bLotTraceStatusComp10SSN As Boolean
            Dim bLotTraceStatusComp11SSN As Boolean
            Dim bLotTraceStatusComp12SSN As Boolean
            Dim bLotTraceStatusComp13SSN As Boolean

            Dim Driver_ProductID As String = ""
            Dim Pass_ProductID As String = ""
            Dim RearBack_ProductID As String = ""
            Dim RearCushion_ProductID As String = ""

            Dim driverSSN As String = txtDriverSeatSerialNum.Text
            Dim passengerSSN As String = txtPassSeatSerialNum.Text
            Dim rearBackSSN As String = txtRearBackSerialNum.Text
            Dim Comp4SSN As String = txtComp4SerialNum.Text
            Dim Comp5SSN As String = txtComp5SerialNum.Text
            Dim Comp6SSN As String = txtComp6SerialNum.Text
            Dim Comp7SSN As String = txtComp7SerialNum.Text
            Dim Comp8SSN As String = txtComp8SerialNum.Text
            Dim Comp9SSN As String = txtComp9SerialNum.Text
            Dim Comp10SSN As String = txtComp10SerialNum.Text
            Dim Comp11SSN As String = txtComp11SerialNum.Text
            Dim Comp12SSN As String = txtComp12SerialNum.Text
            Dim Comp13SSN As String = txtComp13SerialNum.Text

            ' Clear Labels
            lblDriverSeatSerialNumStatus.Text = ""
            lblPassSeatSerialNumStatus.Text = ""
            lblRearBackSerialNumStatus.Text = ""
            lblComp4SerialNumStatus.Text = ""
            lblComp5SerialNumStatus.Text = ""
            lblComp6SerialNumStatus.Text = ""
            lblComp7SerialNumStatus.Text = ""
            lblComp8SerialNumStatus.Text = ""
            lblComp9SerialNumStatus.Text = ""
            lblComp10SerialNumStatus.Text = ""
            lblComp11SerialNumStatus.Text = ""
            lblComp12SerialNumStatus.Text = ""
            lblComp13SerialNumStatus.Text = ""

            If driverSSN.Trim.Length() > 0 Then
                ValidateSSN(driverSSN, lblDriverSeatSerialNumStatus.Text)
                bLotTraceStatusDriverSSN = CheckLotTraceData("Driver", driverSSN, driverLotTraceMsg)
            End If

            If passengerSSN.Trim.Length() > 0 Then
                ValidateSSN(passengerSSN, lblPassSeatSerialNumStatus.Text)
                bLotTraceStatusPassSSN = CheckLotTraceData("Passenger", passengerSSN, passLotTraceMsg)
            End If

            If rearBackSSN.Trim.Length() > 0 Then
                ValidateSSN(rearBackSSN, lblRearBackSerialNumStatus.Text)
                bLotTraceStatusRearBackSSN = CheckLotTraceData("RearBack", rearBackSSN, rearBackLotTraceMsg)
            End If

            If Comp4SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp4SSN, lblComp4SerialNumStatus.Text)
                bLotTraceStatusComp4SSN = CheckLotTraceData("RearCushion", Comp4SSN, Comp4LotTraceMsg)
            End If

            If Comp5SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp5SSN, lblComp5SerialNumStatus.Text)
                bLotTraceStatusComp5SSN = CheckLotTraceData("RB40", Comp5SSN, Comp5LotTraceMsg)
            End If
            If Comp6SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp6SSN, lblComp6SerialNumStatus.Text)
                bLotTraceStatusComp6SSN = CheckLotTraceData("RB60", Comp6SSN, Comp6LotTraceMsg)
            End If
            If Comp7SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp7SSN, lblComp7SerialNumStatus.Text)
                bLotTraceStatusComp7SSN = CheckLotTraceData("RB100", Comp7SSN, Comp7LotTraceMsg)
            End If
            If Comp8SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp8SSN, lblComp8SerialNumStatus.Text)
                bLotTraceStatusComp8SSN = CheckLotTraceData("ArmrestRFID", Comp8SSN, Comp8LotTraceMsg)
            End If

            If Comp9SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp9SSN, lblComp9SerialNumStatus.Text)
                bLotTraceStatusComp9SSN = CheckLotTraceData("Comp9", Comp9SSN, Comp9LotTraceMsg)
            End If
            If Comp10SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp10SSN, lblComp10SerialNumStatus.Text)
                bLotTraceStatusComp10SSN = CheckLotTraceData("Comp10", Comp10SSN, Comp10LotTraceMsg)
            End If
            If Comp11SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp11SSN, lblComp11SerialNumStatus.Text)
                bLotTraceStatusComp11SSN = CheckLotTraceData("Comp11", Comp11SSN, Comp11LotTraceMsg)
            End If
            If Comp12SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp12SSN, lblComp12SerialNumStatus.Text)
                bLotTraceStatusComp12SSN = CheckLotTraceData("Comp12", Comp12SSN, Comp12LotTraceMsg)
            End If
            If Comp13SSN.Trim.Length() > 0 Then
                ValidateSSN(Comp13SSN, lblComp13SerialNumStatus.Text)
                bLotTraceStatusComp13SSN = CheckLotTraceData("Comp13", Comp13SSN, Comp13LotTraceMsg)
            End If

            'Required are driver passenger rear cushion and (60%Back or 100%Back)
            If (Me.txtDriverSeatSerialNum.Text <> Me.txtPassSeatSerialNum.Text) And (Me.txtPassSeatSerialNum.Text <> Me.txtComp4SerialNum.Text) And (Me.txtComp4SerialNum.Text <> Me.txtComp6SerialNum.Text) And (Me.txtComp6SerialNum.Text <> Me.txtComp7SerialNum.Text) Then
                If Me.txtDriverSeatSerialNum.Text <> "" And Me.txtPassSeatSerialNum.Text <> "" And Me.txtComp4SerialNum.Text <> "" And (Me.txtComp6SerialNum.Text <> "" Or Me.txtComp7SerialNum.Text <> "") Then
                    If (bLotTraceStatusDriverSSN) And (bLotTraceStatusPassSSN) And bLotTraceStatusComp4SSN And bLotTraceStatusComp4SSN And (bLotTraceStatusComp6SSN Or bLotTraceStatusComp7SSN) Then
                        bShipStatus = CheckShippingParameters(driverSSN, passengerSSN)
                    End If
                End If
            End If

            WriteMsg()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Function CheckLotTraceData(ByVal ssnType As String, ByRef strSSN As String, ByRef Message As ArrayList) As Boolean
        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim bResult As Boolean = False
        Dim strOperationID As String = ""
        Dim strDescription As String = ""
        Dim colOutParms As List(Of SqlParameter)
        Dim pintReturn As Integer = 0

        Dim strErr As String = ""

        Try

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

            If pintReturn <> 1 Then
                strShipStatus = "Teardown"
                Message.Add(SerialNumberTypeName(ssnType) & " Lot Trace Data Missing")
            Else
                Message.Add(SerialNumberTypeName(ssnType) & " Lot Trace Data Found")
                bResult = True
            End If


            Return bResult

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    'Function CheckShippingParameters(ByRef DriverSeatStlyeID As String,
    '                                 ByRef PassSeatStlyeID As String,
    '                                 ByRef RearBackStlyeID As String,
    '                                 ByRef Comp4StlyeID As String, ByRef Comp5StlyeID As String, ByRef Comp6StlyeID As String,
    '                                 ByRef Comp7StlyeID As String, ByRef Comp8StlyeID As String, ByRef Comp9StlyeID As String,
    '                                 ByRef Comp10StlyeID As String, ByRef Comp11StlyeID As String, ByRef Comp12StlyeID As String,
    '                                 ByRef Comp13StlyeID As String) As Boolean

    '    Dim Model_Code As String = ""
    '    Dim Label_Seat_Code As String = ""
    '    Dim Color_Description As String = ""
    '    Dim Shipping_Code As String = ""
    '    Dim Product_Id As String = ""

    '    Dim colParms As New List(Of SqlParameter)
    '    Dim prmNext As Data.SqlClient.SqlParameter
    '    Dim colOutParms As List(Of SqlParameter)

    '    Dim bResult As Boolean = False
    '    If DriverSeatStlyeID <> "" Then
    '        DriverSeatStlyeID = DriverSeatStlyeID.Substring(0, 4) & DriverSeatStlyeID.Substring(DriverSeatStlyeID.Length - 2, 2)
    '    End If
    '    If PassSeatStlyeID <> "" Then
    '        PassSeatStlyeID = PassSeatStlyeID.Substring(0, 4) & PassSeatStlyeID.Substring(PassSeatStlyeID.Length - 2, 2)
    '    End If
    '    If RearBackStlyeID <> "" Then
    '        RearBackStlyeID = RearBackStlyeID.Substring(0, 4) & RearBackStlyeID.Substring(RearBackStlyeID.Length - 2, 2)
    '    End If
    '    If Comp4StlyeID <> "" Then
    '        Comp4StlyeID = Comp4StlyeID.Substring(0, 4) & Comp4StlyeID.Substring(Comp4StlyeID.Length - 2, 2)
    '    End If
    '    If Comp5StlyeID <> "" Then
    '        Comp5StlyeID = Comp5StlyeID.Substring(0, 4) & Comp5StlyeID.Substring(Comp5StlyeID.Length - 2, 2)
    '    End If
    '    If Comp6StlyeID <> "" Then
    '        Comp6StlyeID = Comp6StlyeID.Substring(0, 4) & Comp6StlyeID.Substring(Comp6StlyeID.Length - 2, 2)
    '    End If
    '    If Comp7StlyeID <> "" Then
    '        Comp7StlyeID = Comp7StlyeID.Substring(0, 4) & Comp7StlyeID.Substring(Comp7StlyeID.Length - 2, 2)
    '    End If
    '    If Comp8StlyeID <> "" Then
    '        Comp8StlyeID = Comp8StlyeID.Substring(0, 4) & Comp8StlyeID.Substring(Comp8StlyeID.Length - 2, 2)
    '    End If
    '    If Comp9StlyeID <> "" Then
    '        Comp9StlyeID = Comp9StlyeID.Substring(0, 4) & Comp9StlyeID.Substring(Comp9StlyeID.Length - 2, 2)
    '    End If
    '    If Comp10StlyeID <> "" Then
    '        Comp10StlyeID = Comp10StlyeID.Substring(0, 4) & Comp10StlyeID.Substring(Comp10StlyeID.Length - 2, 2)
    '    End If
    '    If Comp11StlyeID <> "" Then
    '        Comp11StlyeID = Comp11StlyeID.Substring(0, 4) & Comp11StlyeID.Substring(Comp11StlyeID.Length - 2, 2)
    '    End If
    '    If Comp12StlyeID <> "" Then
    '        Comp12StlyeID = Comp12StlyeID.Substring(0, 4) & Comp12StlyeID.Substring(Comp12StlyeID.Length - 2, 2)
    '    End If
    '    If Comp13StlyeID <> "" Then
    '        Comp13StlyeID = Comp13StlyeID.Substring(0, 4) & Comp13StlyeID.Substring(Comp13StlyeID.Length - 2, 2)
    '    End If


    '    'bDriverFound = LookupObjectInShippingValidationTable("0211", DriverSeatStlyeID, "Driver", driverCheckShipMsg)
    '    'bPassFound = LookupObjectInShippingValidationTable("0212", PassSeatStlyeID, "Passenger", passCheckShipMsg)
    '    'bRearBackFound = LookupObjectInShippingValidationTable("0210", RearBackStlyeID, "RearBack", rearBackCheckShipMsg)   '100% Rear Back same as Rear Back
    '    'bComp4Found = LookupObjectInShippingValidationTable("0477", Comp4StlyeID, "Comp4", Comp4CheckShipMsg)
    '    'bComp5Found = LookupObjectInShippingValidationTable("0479", Comp5StlyeID, "Comp5", Comp5CheckShipMsg)
    '    'bComp6Found = LookupObjectInShippingValidationTable("0480", Comp6StlyeID, "Comp6", Comp6CheckShipMsg)
    '    'bComp7Found = LookupObjectInShippingValidationTable("0210", Comp7StlyeID, "Comp7", Comp7CheckShipMsg) '100% Rear Back same as Rear Back
    '    'bComp8Found = LookupObjectInShippingValidationTable("NEED TO LOOK THIS UP", Comp8StlyeID, "Comp8", Comp8CheckShipMsg)   'Armrest RFID IS NOT VALIDATED
    '    'bComp9Found = LookupObjectInShippingValidationTable("NEED TO LOOK THIS UP", Comp9StlyeID, "Comp9", Comp9CheckShipMsg)
    '    'bComp10Found = LookupObjectInShippingValidationTable("NEED TO LOOK THIS UP", Comp10StlyeID, "Comp10", Comp10CheckShipMsg)
    '    'bComp11Found = LookupObjectInShippingValidationTable("NEED TO LOOK THIS UP", Comp11StlyeID, "Comp11", Comp11CheckShipMsg)
    '    'bComp12Found = LookupObjectInShippingValidationTable("NEED TO LOOK THIS UP", Comp12StlyeID, "Comp12", Comp12CheckShipMsg)
    '    'bComp13Found = LookupObjectInShippingValidationTable("NEED TO LOOK THIS UP", Comp13StlyeID, "Comp13", Comp13CheckShipMsg)

    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data1", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = DriverSeatStlyeID
    '    colParms.Add(prmNext)

    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data2", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = PassSeatStlyeID
    '    colParms.Add(prmNext)

    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data3", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = RearBackStlyeID
    '    colParms.Add(prmNext)

    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data4", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp4StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data5", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp5StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data6", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp6StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data7", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp7StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data8", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp8StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data9", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp9StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data10", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp10StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data11", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp11StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data12", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp12StlyeID
    '    colParms.Add(prmNext)
    '    'IN--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Data13", SqlDbType.VarChar, 48)
    '    prmNext.Direction = ParameterDirection.Input
    '    prmNext.Value = Comp13StlyeID
    '    colParms.Add(prmNext)

    '    'OUT--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Model_Code", SqlDbType.VarChar, 50)
    '    prmNext.Direction = ParameterDirection.Output
    '    'prmNext.Value = 1
    '    colParms.Add(prmNext)

    '    'OUT--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Label_Seat_Code", SqlDbType.VarChar, 50)
    '    prmNext.Direction = ParameterDirection.Output
    '    'prmNext.Value = 1
    '    colParms.Add(prmNext)

    '    'OUT--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Color_Description", SqlDbType.VarChar, 50)
    '    prmNext.Direction = ParameterDirection.Output
    '    'prmNext.Value = 1
    '    colParms.Add(prmNext)

    '    'OUT--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Shipping_Code", SqlDbType.VarChar, 50)
    '    prmNext.Direction = ParameterDirection.Output
    '    'prmNext.Value = 1
    '    colParms.Add(prmNext)

    '    'OUT--------------------------------------------------------
    '    prmNext = New Data.SqlClient.SqlParameter("@Product_Id", SqlDbType.VarChar, 50)
    '    prmNext.Direction = ParameterDirection.Output
    '    'prmNext.Value = 1
    '    colParms.Add(prmNext)


    '    colOutParms = DA.ExecSP("procGetMap4PrintValues", colParms)


    '    For Each oParameter In colOutParms
    '        With oParameter

    '            Select Case .ParameterName
    '                Case "@Model_Code"
    '                    Model_Code = .Value.ToString()
    '                Case "@Label_Seat_Code"
    '                    Label_Seat_Code = .Value.ToString()
    '                Case "@Color_Description"
    '                    Color_Description = .Value.ToString()
    '                Case "@Shipping_Code"
    '                    Shipping_Code = .Value.ToString()
    '                Case "@Product_Id"
    '                    Product_Id = .Value.ToString()
    '            End Select
    '        End With
    '    Next

    '    If Len(Product_Id) > 0 Then
    '        tModelCode.Text = Model_Code
    '        tLabelSeatCode.Text = Label_Seat_Code
    '        tColorDesc.Text = Color_Description
    '        'tShippingCode.Text = Shipping_Code
    '        tShippingCode4.Text = Product_Id
    '        strShipStatus = "<span class='fontGreen' >OK To Ship</span>"
    '        bResult = True

    '        'ElseIf bDriverFound = True Or bPassFound = True Or bRearBackFound = True Or bComp4Found = True Or bComp5Found = True Or bComp6Found = True Or bComp7Found = True _
    '        '     Or bComp8Found = True Or bComp9Found = True Or bComp10Found = True Or bComp11Found = True Or bComp12Found = True Or bComp13Found = True Then
    '        '    ' Product was not found, but we have valid SSN's so there must be a mismatch
    '        '    strShipStatus = "Mismatch"
    '        '    bResult = False
    '    Else
    '        ' otherwise... this is the case when Shipping Configuration has no conf entry
    '        strShipStatus = "Not Found"
    '        bResult = False
    '    End If

    '    Return bResult

    'End Function
    Function CheckShippingParameters(ByRef DriverSeatStlyeID As String,
                                     ByRef PassSeatStlyeID As String) As Boolean
        Dim Model_Code As String = ""
        Dim Label_Seat_Code As String = ""
        Dim Color_Description As String = ""
        Dim Shipping_Code As String = ""
        Dim Product_Id As String = ""
        Dim bResult As Boolean = False

        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim colOutParms As List(Of SqlParameter)


        If DriverSeatStlyeID <> "" Then
            DriverSeatStlyeID = DriverSeatStlyeID.Substring(0, 4) & DriverSeatStlyeID.Substring(DriverSeatStlyeID.Length - 2, 2)
        End If
        If PassSeatStlyeID <> "" Then
            PassSeatStlyeID = PassSeatStlyeID.Substring(0, 4) & PassSeatStlyeID.Substring(PassSeatStlyeID.Length - 2, 2)
        End If

        bDriverFound = LookupObjectInShippingValidationTable("0211", DriverSeatStlyeID, "Driver", driverCheckShipMsg)
        bPassFound = LookupObjectInShippingValidationTable("0212", PassSeatStlyeID, "Passenger", passCheckShipMsg)


        'IN--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Data1", SqlDbType.VarChar, 48)
        prmNext.Direction = ParameterDirection.Input
        prmNext.Value = DriverSeatStlyeID
        colParms.Add(prmNext)

        'IN--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Data2", SqlDbType.VarChar, 48)
        prmNext.Direction = ParameterDirection.Input
        prmNext.Value = PassSeatStlyeID
        colParms.Add(prmNext)
        'IN--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Data3", SqlDbType.VarChar, 48)
        prmNext.Direction = ParameterDirection.Input
        prmNext.Value = ""
        colParms.Add(prmNext)

        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Model_Code", SqlDbType.VarChar, 50)
        prmNext.Direction = ParameterDirection.Output
        'prmNext.Value = 1
        colParms.Add(prmNext)

        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Label_Seat_Code", SqlDbType.VarChar, 50)
        prmNext.Direction = ParameterDirection.Output
        'prmNext.Value = 1
        colParms.Add(prmNext)

        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Color_Description", SqlDbType.VarChar, 50)
        prmNext.Direction = ParameterDirection.Output
        'prmNext.Value = 1
        colParms.Add(prmNext)

        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Shipping_Code", SqlDbType.VarChar, 50)
        prmNext.Direction = ParameterDirection.Output
        'prmNext.Value = 1
        colParms.Add(prmNext)

        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@Product_Id", SqlDbType.VarChar, 50)
        prmNext.Direction = ParameterDirection.Output
        'prmNext.Value = 1
        colParms.Add(prmNext)


        colOutParms = DA.ExecSP("procGetMap4PrintValues", colParms)


        For Each oParameter In colOutParms
            With oParameter

                Select Case .ParameterName
                    Case "@Model_Code"
                        Model_Code = .Value.ToString()
                    Case "@Label_Seat_Code"
                        Label_Seat_Code = .Value.ToString()
                    Case "@Color_Description"
                        Color_Description = .Value.ToString()
                    Case "@Shipping_Code"
                        Shipping_Code = .Value.ToString()
                    Case "@Product_Id"
                        Product_Id = .Value.ToString()
                End Select
            End With
        Next

        If Len(Product_Id) > 0 Then
            tModelCode.Text = Model_Code
            tLabelSeatCode.Text = Label_Seat_Code
            tColorDesc.Text = Color_Description
            'tShippingCode.Text = Shipping_Code
            tShippingCode4.Text = Product_Id
            strShipStatus = "<span class='fontGreen' >OK To Ship</span>"
            bResult = True

        ElseIf bDriverFound = True Or bPassFound = True Or bRearBackFound = True Then
            ' Product was not found, but we have valid SSN's so there must be a mismatch
            strShipStatus = "Mismatch"
            bResult = False
        Else
            ' otherwise... this is the case when Shipping Configuration has no conf entry
            strShipStatus = "Not Found"
            bResult = False
        End If

        Return bResult

    End Function

    Function LookupObjectInShippingValidationTable(ByVal ProductParameterTypeId As String, ByVal ProductParameterValue As String, ssnType As String, ByRef arrayMessage As ArrayList) As Boolean
        Dim strselect As String = ""
        Dim ds As DataSet
        Dim bResult As Boolean = False

        Try
            strselect = "Select ProductParameterTypeId FROM tblProductParameters Where " &
                           "ProductParameterTypeId = '" & ProductParameterTypeId & "' And ProductParameterValue = " & "'" & ProductParameterValue & "'"


            ds = DA.GetDataSet(strselect)

            If ds.Tables(0).Rows.Count > 0 Then
                bResult = True
                'arrayMessage.Add(SerialNumberTypeName(ssnType) & " Found In Shipping Validation Table")
            Else
                arrayMessage.Add(SerialNumberTypeName(ssnType) & " Not Found In Shipping Validation Table")
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function


    'pb - 06/21/04, check for duplicate ssn's
    Private Function CheckForDuplicateSSNs(ByVal ssn As String, Optional ByVal period As String = "") As String

        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim colOutParms As List(Of SqlParameter)
        Dim strResult As String = ""

        If period Is Nothing OrElse period = "" Then
            period = DupSsnPeriod
        End If


        Dim ds As New DataSet
        Try

            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@Serial_Number", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = ssn
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@Period", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = period
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@Status", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            'OUT--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@DupDate", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Output
            colParms.Add(prmNext)
            '------------------------------------------------------------
            colOutParms = DA.ExecSP("procCheckDuplicateShip", colParms)

            For Each oParameter In colOutParms
                With oParameter
                    If .ParameterName = "@DupDate" Then
                        strResult = .Value.ToString()
                        If hidDupSSN.Value = "" Then
                            hidDupSSN.Value = ssn
                            hidDupDT.Value = strResult
                        End If
                    End If
                End With
            Next


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return strResult

    End Function

    Sub InsertShippingLabel()
        Try
            Dim strInsert As String

            strInsert = "insert into tblShippingLabelPrintHistory " &
                        "(LOT_TRACE_DT," &
                        "LABEL_SEAT_CODE," &
                        "MODEL_CODE," &
                        "SHIPPING_BARCODE," &
                        "COLOR_DESCRIPTION," &
                        "PRINTED_BY," &
                        "PRINT_LOCATION," &
                        "RECORDED_BY," &
                        "LABEL_STATUS," &
                        "COMPONENT1," &
                        "COMPONENT2," &
                        "COMPONENT3," &
                        "COMPONENT4," &
                        "COMPONENT5," &
                        "COMPONENT6," &
                        "COMPONENT7," &
                        "COMPONENT8," &
                        "COMPONENT9," &
                        "COMPONENT10," &
                        "COMPONENT11," &
                        "COMPONENT12," &
                        "COMPONENT13) " &
                        "values(" &
                        "getdate()" &
                        ",'" & tLabelSeatCode.Text &
                        "','" & tModelCode.Text &
                        "','" & tShippingCode4.Text &
                        "','" & tColorDesc.Text &
                        "','" & "Ship. Label Web Page" &
                        "','" & ddPrinters.SelectedItem.Value &
                        "','" & Me.Page.User.Identity.Name.ToString() &
                        "','" & "OK" &
                        "','" & txtPassSeatSerialNum.Text &
                        "','" & txtDriverSeatSerialNum.Text &
                        "','" & txtRearBackSerialNum.Text &
                        "','" & txtComp4SerialNum.Text &
                        "','" & txtComp5SerialNum.Text &
                        "','" & txtComp6SerialNum.Text &
                        "','" & txtComp7SerialNum.Text &
                        "','" & txtComp8SerialNum.Text &
                        "','" & txtComp9SerialNum.Text &
                        "','" & txtComp10SerialNum.Text &
                        "','" & txtComp11SerialNum.Text &
                        "','" & txtComp12SerialNum.Text &
                        "','" & txtComp13SerialNum.Text & "') "

            DA.ExecSQL(strInsert)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Sub PrintShippingLabel()
        Try
            'Dim strArg As String
            Dim strDelim As String
            Dim strQuantity As String
            strDelim = "|"

            strQuantity = "1"

            'HeadRest_Label_Print() 'tSeatSerialNum.Text, tComments.Text, tSeatCode.Text, tSeatPartNum.Text, ddPrinters.SelectedItem.Text, Page.User.Identity.Name.ToString, 1)
            'hidPrintString.Value = BizLayer.GetApplicationParameterValue("0113", "0001") & " " & tModelCode.Text & strDelim & tLabelSeatCode.Text & strDelim & tColorDesc.Text & strDelim & "" & strDelim & ddPrinters.SelectedItem.Value & strDelim & tShippingCode4.Text & strDelim & "7" & strDelim & "1" & strDelim & tShippingCode4.Text
            'hidPrintString.Value = tModelCode.Text & strDelim & tLabelSeatCode.Text & strDelim & tColorDesc.Text & strDelim & "" & strDelim & ddPrinters.SelectedItem.Value & strDelim & tShippingCode4.Text & strDelim & "7" & strDelim & "1" & strDelim & tShippingCode4.Text
            'hidPrintString.Value = tModelCode.Text & strDelim & tLabelSeatCode.Text & strDelim & tColorDesc.Text & strDelim & "" & strDelim & ddPrinters.SelectedItem.Value & strDelim & tShippingCode4.Text & strDelim & "7" & strDelim & "1" & strDelim & tShippingCode4.Text

            'Duplicate Ship Label Print
            'hidPrintString.Value = hidDupSSN.Value & "||" & "" & "|" & "" & "|" & ddPrinters.SelectedItem.Text & "|" & hidDupDT.Value & "|7|1"

            'Ship Label Print
            hidPrintString.Value = tModelCode.Text & "|" & tLabelSeatCode.Text & "|" & tColorDesc.Text & "||" & ddPrinters.SelectedItem.Text & "|" & tShippingCode4.Text & "|5|1"
            'strArg = BizLayer.GetApplicationParameterValue("0113", "0001") & " " & tModelCode.Text & strDelim & tLabelSeatCode.Text & strDelim & tColorDesc.Text & strDelim & "" & strDelim & ddPrinters.SelectedItem.Value & strDelim & tShippingCode4.Text & strDelim & "7" & strDelim & "1" & strDelim & tShippingCode4.Text
            'Process.Start(strArg)
            Master.Msg = "1 Label was sent to printer"

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Function SerialNumberTypeName(ssnType As String) As String
        Dim bResult As String = ""

        Select Case ssnType
            Case "Driver"
                bResult = "Driver Seat:    "
            Case "Passenger"
                bResult = "Passenger Seat: "
            Case "RearBack"
                bResult = "Rear Back:      "
            Case "RearCushion"
                bResult = "Rear Cushion:   "
            Case "RB40"
                bResult = "Rear Back 40%:   "
            Case "RB60"
                bResult = "Rear Back 60%:   "
            Case "RB100"
                bResult = "Rear Back 100%:   "
            Case "ArmrestRFID"
                bResult = "Armrest RFID:   "
            Case "Comp9"
                bResult = "Undefined"
            Case "Comp10"
                bResult = "Undefined"
            Case "Comp11"
                bResult = "Undefined"
            Case "Comp12"
                bResult = "Undefined"
            Case "Comp13"
                bResult = "Undefined"
            Case Else
                bResult = "Undefined"
        End Select

        Return bResult
    End Function

#End Region

End Class