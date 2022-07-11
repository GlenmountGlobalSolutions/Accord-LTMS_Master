
Imports System.Data.SqlClient
Public Class PrintNewSSNLabel
    Inherits System.Web.UI.Page

    Dim strHumanReadable As String = ""
    Dim strSeatCode As String = ""
    Dim strSeatPartNumber As String = ""

    Enum LabelType
        Reject = 0
        Foam = 1
        HeadRest = 2
    End Enum

#Region "Event Handlers"


    Private Sub PrintNewSSNLabel_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim strSelectedPrinter As String = ""

            If Not IsPostBack() Then
                ddPrinters_DataBind()
                SeatType_DataBind()
                NCopies()

                'grab the selected printer from the request object and run client-side script to select it from the printer drop down list
                'this is needed for each postback that occurs because the printer drop down won't keep its selection (not a dot net drop down)
                If (hidSelectedPrinterPrelim.Value.Length > 0) Then
                    strSelectedPrinter = hidSelectedPrinterPrelim.Value
                End If

                populateLabelTypeDropDown()
                populatePrinterDropDown(strSelectedPrinter)
                lblNextSSN.Text = GetNextSSN(ddSeatType.SelectedItem.Value)
                lblPrintSSN.Text = hidlblNextSSN.Value
            Else
                lblPrintSSN.Text = hidlblNextSSN.Value
            End If

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

    Private Sub cmdPrint_Click(sender As Object, e As System.EventArgs) Handles cmdPrint.Click
        Try
            lblDescNextSSN.Visible = True
            SendPrintJob()
            lblNextSSN.Text = GetNextSSN(ddSeatType.SelectedItem.Value)
            lblPrintSSN.Text = lblNextSSN.Text
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SendPrintJob()

        Dim strinsert As String = ""

        Try

            HeadRest_Label_Print(hidlblNextSSN.Value, strHumanReadable, strSeatCode, strSeatPartNumber, hidSelectedPrinterPrelim.Value, Page.User.Identity.Name.ToString, ddlLabelTypes.SelectedValue)

            strinsert = "Insert into tblLabelPrintHistory " &
               "(LABEL_SEAT_CODE,LABEL_SEAT_PART_NUMBER," &
               "LABEL_HUMAN_READABLE,LOT_TRACE_DT," &
               "SEAT_SERIAL_NUMBER,RECORDED_BY,PRINT_LOCATION,PRINTED_BY) Values " &
               "('" & UCase(strSeatCode) & "','" & UCase(strSeatPartNumber) & "','" &
               UCase(strHumanReadable) & "','" & Now & "','" &
               UCase(hidlblNextSSN.Value) & "','" & Page.User.Identity.Name.ToString() & "','" &
               hidSelectedPrinterPrelim.Value & "','" & "')"
            DA.ExecSQL(strinsert)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub



    Private Sub ddSeatType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddSeatType.SelectedIndexChanged
        Try
            lblNextSSN.Text = GetNextSSN(ddSeatType.SelectedItem.Value)
            lblPrintSSN.Text = lblNextSSN.Text
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

        If lblNextSSN.Text.Contains("Error") Then
            cmdPrint.Enabled = False
        End If

    End Sub


    Sub ddPrinters_DataBind()
        Dim ds As New DataSet

        ds = DA.GetDataSet("Select  Name, NetworkPath from tblLabelPrinters Where LabelPrinter = 1")
        ddPrinters.DataSource = ds
        ddPrinters.DataTextField = "Name"
        ddPrinters.DataValueField = "NetworkPath"
        ddPrinters.DataBind()
        Dim i As Integer
        Dim foundindex As Integer
        Dim strPrinterName As String
        'retrieve default printer name
        strPrinterName = Trim(BizLayer.GetApplicationParameterValue("2230", "0001"))
        'find printer name by one configured in application parameter
        If Not ds Is Nothing Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                If Trim(ds.Tables(0).Rows(i).Item(0).ToString()) = strPrinterName Then
                    foundindex = i
                    Exit For
                End If
            Next
        End If
        'select the value if it exists.
        ddPrinters.SelectedIndex = foundindex

    End Sub

    Sub SeatType_DataBind()
        ddSeatType.DataSource = DA.GetDataSet("SELECT DISTINCT ProductID FROM tblProducts WHERE (ComponentID <> '03') AND (ComponentID <> '04')")
        ddSeatType.DataTextField = "ProductID"
        ddSeatType.DataValueField = "ProductID"
        ddSeatType.DataBind()
    End Sub

    Sub NCopies()
        tNCopies.Text = BizLayer.GetApplicationParameterValue("0112", "0001")
    End Sub

    Function GetNextSSN(ByRef SeatNumber As String) As String
        Dim strResult As String = ""
        Dim strSerialNumber As String
        Dim strNextPN As String
        Dim strNextProductionNumber As String = ""
        Dim strSelect As String
        Dim p_strtemporary As String
        Dim p_strtemporaryColorCode As String
        Dim ds As DataSet
        Dim blnReCheckNumber As Boolean
        Dim intCounter As Integer = 0

        'modified by pb, 08/17/04
        p_strtemporary = Mid(SeatNumber, 1, 4)
        p_strtemporaryColorCode = Mid(SeatNumber, 5)

        strSelect = "Select NEXT_PRODUCTION_NUMBER " &
        "from tblPrintSerialNumberParameters " &
        "where STYLE_CODE = '" & p_strtemporary & "'"

        ds = DA.GetDataSet(strSelect)
        If (DA.IsDSEmpty(ds)) Then
            strResult = ""
        Else


            If (ds.Tables(0).Rows(0).Item("Next_Production_Number").ToString()) <> "" Then
                strNextProductionNumber = (ds.Tables(0).Rows(0).Item("Next_Production_Number").ToString())
            End If


            'let try to build make serial number and check if it ever used or not
            strSerialNumber = UCase((p_strtemporary)) & UCase((strNextProductionNumber)) & UCase((p_strtemporaryColorCode))

            'blnTestLike = 1 'if true keep checking to make sure that serial number was not used
            blnReCheckNumber = True

            Do While blnReCheckNumber = True

                strSelect = "Select SEAT_SERIAL_NUMBER " &
                "from tblLabelPrintHistory " &
                "where SEAT_SERIAL_NUMBER Like  '" & (strSerialNumber) & "%'"

                ds = DA.GetDataSet(strSelect)

                If (ds.Tables(0).Rows.Count = 0) Then
                    blnReCheckNumber = False
                    strResult = UCase((strSerialNumber))
                    hidlblNextSSN.Value = strResult
                Else
                    strNextPN = Serial_Counter(UCase((strNextProductionNumber)))
                    strSerialNumber = UCase((p_strtemporary)) & UCase((strNextPN)) & UCase((p_strtemporaryColorCode))
                    strNextProductionNumber = strNextPN
                    hidlblNextSSN.Value = strSerialNumber
                End If

                intCounter += 1

                If intCounter > 99999 Then
                    strResult = "Error. Invalid seat type due to: seat style format or label configuration!"
                    cmdPrint.Enabled = False
                    blnReCheckNumber = False
                End If
            Loop

        End If
        Return strResult

    End Function

    Private Function Serial_Counter(ByVal SNum As String) As String
        Dim strString As String
        Dim intLengthString As Integer
        Dim intLengthRight As Integer
        Dim strLeftstring As String
        Dim strRightString As String
        Dim strRightReturn As String
        Dim strLeftReturn As String = ""
        Dim intRightInteger As Integer
        Dim strReturnString As String

        strString = (SNum)
        intLengthString = Len(strString)

        If intLengthString > 5 Then
            Master.Msg = "Serial Number:  " & strString & " too Long - Serial Number Length Must Equal 5 Characters."
            Serial_Counter = "00000"
            Return ""
        End If
        If intLengthString < 5 Then
            Master.Msg = "Serial Number:  " & strString & " too Short - Serial Number Length Must Equal 5 Characters."
            Serial_Counter = "00000"
            Return ""
        End If

        strLeftstring = Mid(SNum, 1, 1)
        strRightString = Mid(SNum, 2)
        If IsNumeric(strRightString) Then
            intRightInteger = CInt(strRightString)
        Else
            Master.Msg = "Invalid Serial Number: " & strString & " - Right 4 Digits Must Be Numeric"
            Serial_Counter = "00000"
            Return ""
        End If

        If intRightInteger < 9999 Then
            intRightInteger = intRightInteger + 1
            strRightReturn = CStr(intRightInteger)
            intLengthRight = Len(strRightReturn)
            Do While intLengthRight <= 3
                strRightReturn = "0" & strRightReturn
                intLengthRight = intLengthRight + 1
            Loop
            strReturnString = strLeftstring + strRightReturn
        ElseIf intRightInteger = 9999 Then
            strLeftstring = UCase(strLeftstring)
            Select Case strLeftstring
                Case "0"
                    strLeftReturn = "1"
                Case "1"
                    strLeftReturn = "2"
                Case "2"
                    strLeftReturn = "3"
                Case "3"
                    strLeftReturn = "4"
                Case "4"
                    strLeftReturn = "5"
                Case "5"
                    strLeftReturn = "6"
                Case "6"
                    strLeftReturn = "7"
                Case "7"
                    strLeftReturn = "8"
                Case "8"
                    strLeftReturn = "9"
                Case "9"
                    strLeftReturn = "A"
                Case "A"
                    strLeftReturn = "B"
                Case "B"
                    strLeftReturn = "C"
                Case "C"
                    strLeftReturn = "D"
                Case "D"
                    strLeftReturn = "E"
                Case "E"
                    strLeftReturn = "F"
                Case "F"
                    strLeftReturn = "G"
                Case "G"
                    strLeftReturn = "H"
                Case "H"
                    strLeftReturn = "J"
                Case "J"
                    strLeftReturn = "K"
                Case "K"
                    strLeftReturn = "L"
                Case "L"
                    strLeftReturn = "M"
                Case "M"
                    strLeftReturn = "N"
                Case "N"
                    strLeftReturn = "P"
                Case "P"
                    strLeftReturn = "Q"
                Case "Q"
                    strLeftReturn = "R"
                Case "R"
                    strLeftReturn = "S"
                Case "S"
                    strLeftReturn = "T"
                Case "T"
                    strLeftReturn = "U"
                Case "U"
                    strLeftReturn = "V"
                Case "V"
                    strLeftReturn = "W"
                Case "W"
                    strLeftReturn = "X"
                Case "X"
                    strLeftReturn = "Y"
                Case "Y"
                    strLeftReturn = "Z"
                Case Else
                    Master.Msg = "Invalid Serial Number: " & strString & " - Out Of Range"
                    Serial_Counter = "00000"
            End Select
            strRightReturn = "0000"
            strReturnString = strLeftReturn + strRightReturn
        Else
            Master.Msg = "Invalid Serial Number: " & strString
            strReturnString = "00000"
        End If

        Return UCase(strReturnString)

    End Function


    Function Foam_Label_Print(ByVal SeatNumber As String, ByVal HondaLotNumber As String, ByVal QueueName As String, ByVal Requestor As String, ByVal LabelType As LabelType) As String
        Dim strResult As String = ""
        Dim strSelect As String
        Dim strinsert As String
        Dim strLSC As String = ""
        Dim strLPN As String = ""
        Dim strLHR As String = ""
        Dim strHLN As String
        Dim strSerialNumber As String = ""
        Dim strNextPN As String
        Dim strNextProductionNumber As String = ""
        Dim strUpdate As String
        Dim p_strtemporary As String
        Dim p_strtemporaryColorCode As String
        Dim ds As DataSet
        Dim ds1 As DataSet
        Dim blnReCheckNumber As Boolean
        Dim intCounter As Integer = 0

        If LabelType = PrintNewSSNLabel.LabelType.Reject Then    'Reject
            HeadRest_Label_Print("", "", "", "", QueueName, Requestor, "0")
            strResult = ""
        Else
            If LabelType = PrintNewSSNLabel.LabelType.HeadRest Then     ' HeadRest
                strResult = strSerialNumber.ToUpper()

            ElseIf LabelType = PrintNewSSNLabel.LabelType.Foam Then     ' Foam

                p_strtemporary = Mid(SeatNumber, 1, 4)
                p_strtemporaryColorCode = Mid(SeatNumber, 5)

                strSelect = "Select NEXT_PRODUCTION_NUMBER " &
                "from tblPrintSerialNumberParameters " &
                "where STYLE_CODE = '" & p_strtemporary & "'"

                ds = DA.GetDataSet(strSelect)
                If (DA.IsDSEmpty(ds)) Then
                    Return ""
                End If


                'If (Err.Number <> 0) Then
                '	Return 0
                '	Exit Function
                'End If

                If (ds.Tables(0).Rows(0).Item("Next_Production_Number").ToString() & "") <> "" Then
                    strNextProductionNumber = ds.Tables(0).Rows(0).Item("Next_Production_Number").ToString()
                End If


                'let try to build make serial number and check if it ever used or not
                strSerialNumber = (p_strtemporary & strNextProductionNumber & p_strtemporaryColorCode).ToUpper()

                'blnTestLike = 1 'if true keep checking to make sure that serial number was not used
                blnReCheckNumber = True

                Do While blnReCheckNumber = True

                    strSelect = "Select SEAT_SERIAL_NUMBER " &
                    "from tblLabelPrintHistory " &
                    "where SEAT_SERIAL_NUMBER Like  '" & (strSerialNumber) & "%'"

                    ds = DA.GetDataSet(strSelect)

                    If (ds.Tables(0).Rows.Count = 0) Then

                        blnReCheckNumber = False

                        strSelect = "Select LABEL_SEAT_CODE," &
                        "LABEL_SEAT_PART_NUMBER,LABEL_HUMAN_READABLE " &
                        "from tblPrintSerialNumberParameters " &
                        "where STYLE_CODE = '" & (p_strtemporary) & "'"


                        ds = DA.GetDataSet(strSelect)

                        If ds.Tables(0).Rows(0).Item("Label_Seat_Code").ToString() <> "" Then
                            strLSC = ds.Tables(0).Rows(0).Item("Label_Seat_Code").ToString()
                        End If
                        If ds.Tables(0).Rows(0).Item("Label_Seat_Part_Number").ToString() <> "" Then
                            strLPN = ds.Tables(0).Rows(0).Item("Label_Seat_Part_Number").ToString()
                        End If
                        If ds.Tables(0).Rows(0).Item("Label_Human_Readable").ToString() <> "" Then
                            strLHR = ds.Tables(0).Rows(0).Item("Label_Human_Readable").ToString()
                        End If

                        strSelect = "Select ProductParameterValue FROM tblProductParameters Where " &
                            "ProductParameterTypeId = " & "'0136'" &
                            "And ProductID = " &
                            "'" & SeatNumber & "'"
                        ds1 = DA.GetDataSet(strSelect)
                        If ds1.Tables(0).Rows.Count > 0 Then
                            strLSC = ds1.Tables(0).Rows(0).Item("ProductParameterValue").ToString()
                        End If
                        If strLSC = "" Then
                            strLSC = "Bad Data"
                        End If

                        strHLN = HondaLotNumber

                        strinsert = "Insert into tblLabelPrintHistory " &
                        "(LABEL_SEAT_CODE,LABEL_SEAT_PART_NUMBER," &
                        "HONDA_LOT_NUMBER,LABEL_HUMAN_READABLE,LOT_TRACE_DT," &
                        "SEAT_SERIAL_NUMBER,RECORDED_BY,PRINT_LOCATION) Values " &
                        "('" & strLSC & "','" & strLPN & "','" &
                        strHLN & "','" & strLHR & "','" & Now & "','" &
                        strSerialNumber & "','" & Requestor & "','" &
                        QueueName & "')".ToUpper()

                        DA.ExecSQL(strinsert)

                        HeadRest_Label_Print(strSerialNumber, strLHR, strLSC, strLPN, QueueName, Requestor, "1")


                        strNextPN = Serial_Counter(strNextProductionNumber.ToUpper())

                        strNextProductionNumber = strNextPN

                        strUpdate = "Update tblPrintSerialNumberParameters " &
                        "Set NEXT_PRODUCTION_NUMBER = '" & strNextProductionNumber &
                        "' Where STYLE_CODE = '" & Mid(SeatNumber, 1, 4) & "'".ToUpper()


                        DA.ExecSQL(strUpdate)
                        'here where loops will exit and if ok everything function will return next ssn (serial number)

                        strResult = strSerialNumber.ToUpper()

                    Else
                        strNextPN = Serial_Counter(strNextProductionNumber.ToUpper())
                        strSerialNumber = (p_strtemporary & strNextPN & p_strtemporaryColorCode).ToUpper()
                        strNextProductionNumber = strNextPN
                    End If
                Loop

                intCounter += 1

                If intCounter > 99999 Then
                    strResult = "Error. Invalid seat type due to: seat style format or label configuration!"
                    cmdPrint.Enabled = False
                    blnReCheckNumber = False
                End If
            End If
        End If

        Return strResult

    End Function



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

        str = "SELECT TOP 1 SEAT_SERIAL_NUMBER, LABEL_HUMAN_READABLE, LABEL_SEAT_CODE, LABEL_SEAT_PART_NUMBER, LOT_TRACE_DT FROM tblLabelPrintHistory WHERE (SEAT_SERIAL_NUMBER = '" & (hidlblNextSSN.Value) & "')ORDER BY LOT_TRACE_DT DESC"
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


#End Region





End Class