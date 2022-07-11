Imports System.IO
Partial Class ReworkMaterialDispositionReports
    Inherits System.Web.UI.Page

    Private Const PARAM_LIST_ID_REWORK_AREA As String = "52"


#Region "Event Handlers"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                LoadAreas()
                LoadSortDDLs(Me.rblType.SelectedValue)
                SetDefaults()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCreateFile_Click(sender As Object, e As System.EventArgs) Handles cmdCreateFile.Click
        Try
            CreateSpreadsheet()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub ibNextBeg_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibNextBeg.Click
        Try
            Dim dt As Date
            Dim interval As Integer = 1

            If (Date.TryParse(Me.tbDayBeg.Text, dt)) Then
                tbDayBeg.Text = dt.AddDays(interval).ToString("MM/dd/yyyy")
            Else
                Master.Msg = "Invalid Date"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibPrevBeg_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibPrevBeg.Click
        Try
            Dim dt As Date
            Dim interval As Integer = 1
            interval = interval * -1


            If (Date.TryParse(Me.tbDayBeg.Text, dt)) Then
                tbDayBeg.Text = dt.AddDays(interval).ToString("MM/dd/yyyy")
            Else
                Master.Msg = "Invalid Date"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibNextEnd_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibNextEnd.Click
        Try
            Dim dt As Date
            Dim interval As Integer = 1

            If (Date.TryParse(Me.tbDayEnd.Text, dt)) Then
                tbDayEnd.Text = dt.AddDays(interval).ToString("MM/dd/yyyy")
            Else
                Master.Msg = "Invalid Date"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibPrevEnd_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibPrevEnd.Click
        Try
            Dim dt As Date
            Dim interval As Integer = 1
            interval = interval * -1


            If (Date.TryParse(Me.tbDayEnd.Text, dt)) Then
                tbDayEnd.Text = dt.AddDays(interval).ToString("MM/dd/yyyy")
            Else
                Master.Msg = "Invalid Date"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region

#Region "Methods"
    Private Sub EnableControls()
        Try
            Master.Secure(cmdCreateFile)


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub LoadAreas()
        Dim ds As DataSet
        Dim sql As String = "SELECT ParameterListValue AS TEXT, ParameterListValue AS VALUE FROM tblParameterListValues WHERE (ParameterListID = '" + ReworkMaterialDispositionReports.PARAM_LIST_ID_REWORK_AREA.ToString + "') ORDER BY ParameterListValue"

        Me.cblAreas.Items.Clear()

        ds = DA.GetDataSet(sql)
        If (DA.IsDSEmpty(ds)) Then
            Return
        End If

        cblAreas.DataSource = ds
        'cblAreas.DataValueField = "VALUE"
        cblAreas.DataValueField = "TEXT"
        cblAreas.DataTextField = "TEXT"
        cblAreas.DataBind()
    End Sub
    Private Sub LoadSortDDLs(ByVal tblName As String)
        Dim strSQL As String = "SELECT c.name FROM syscolumns c " & _
          "INNER JOIN sysobjects o ON c.id = o.id " & _
          "WHERE o.name = '" & tblName & "' " & _
          "ORDER BY c.colid"
        Dim oDataTable As DataTable

        oDataTable = DA.GetDataSet(strSQL).Tables(0)

        With Me.ddlSortParam1
            .DataSource = oDataTable
            .DataTextField = "name"
            .DataValueField = "name"
            .DataBind()
        End With

        With Me.ddlSortParam2
            .DataSource = oDataTable
            .DataTextField = "name"
            .DataValueField = "name"
            .DataBind()
        End With

        With Me.ddlSortParam3
            .DataSource = oDataTable
            .DataTextField = "name"
            .DataValueField = "name"
            .DataBind()
        End With


    End Sub

    Private Sub SetDefaults()
        Dim objListItem As ListItem

        Me.tbDayBeg.Text = Now().ToString("MM/dd/yyyy")
        Me.tbDayEnd.Text = Now().ToString("MM/dd/yyyy")


        For Each objListItem In cblAreas.Items
            objListItem.Selected = True
        Next

    End Sub

    Private Sub CreateSpreadsheet() 'ByVal begDT As String)
        Dim strExcelFilePrefix As String = ""
        Select Case Me.rblType.SelectedIndex
            Case 0
                strExcelFilePrefix = "Rework.Report"
            Case 1
                strExcelFilePrefix = "Material.Disposition.Report"
        End Select
        Dim strTemplateFilePath As String = Server.MapPath(Request.ApplicationPath) & "\2Reporting\" & strExcelFilePrefix & ".Template.xls"
        Dim strExportFilePath As String = Session("UserWorkingDirFileSystem").ToString & "\" & strExcelFilePrefix & "." & Format(Now(), "MMddyy_HHmmss") & ".xls"
        Dim strConnectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;" & _
                 "Data Source=" & strExportFilePath & ";" & _
                 "Extended Properties=""Excel 8.0;HDR=NO"""                                ';MAXSCANROWS=1
        Dim oDataset As DataSet
        Dim oDataTable As DataTable
        Dim oDataRow As DataRow
        Dim strBaseSQL As String
        Dim strSQL As String
        Dim objStringBuilder As System.Text.StringBuilder = New System.Text.StringBuilder("")
        Dim i As Integer
        Dim oConn As New OleDb.OleDbConnection(strConnectionString)
        Dim oCommand As New OleDb.OleDbCommand
        Dim retval As Long
        Dim objListItem As ListItem
        Dim strAreas As String = ""
        Dim sort1, sort2, sort3 As String      'pb, 03/09/05

        Try
            'pb, 03/09/05
            sort1 = Me.ddlSortParam1.SelectedValue
            sort2 = Me.ddlSortParam2.SelectedValue
            sort3 = Me.ddlSortParam3.SelectedValue

            If (sort1 = sort2) Then
                sort2 = ""
            End If
            If (sort1 = sort3) Then
                sort3 = ""
            End If
            If (sort2 = sort3) Then
                sort3 = ""
            End If


            'make sure Session directory exists
            If Not Directory.Exists(Session("UserWorkingDirFileSystem").ToString) Then
                System.IO.Directory.CreateDirectory(Session("UserWorkingDirFileSystem").ToString)
            End If

            'first make copy of template XLS file
            File.Copy(strTemplateFilePath, strExportFilePath, True)

            'strSQL = "SELECT ReworkDate,Shift,ReworkArea,TeamMember,LotNumber,VehicleLine,SeatStyle,Color,Component,SeatSerialNumber,Category,ReworkReason,ReworkAction,ChargeTo,ReturnedQuantity,Location FROM tblRMDReworkHistory"

            'pb, 03/09/05
            'strSQL = "SELECT * FROM " & Me.rblType.SelectedValue
            If (Me.rblType.SelectedValue = "tblRMDReworkHistory") Then
                'strSQL = "SELECT ReworkDate, Shift, ReworkArea, TeamMember, LotNumber, VehicleLine, SeatStyle, Color, Component, SeatSerialNumber, Category, ReworkReason, ReworkAction, ChargeTo, ReturnedQuantity FROM tblRMDReworkHistory "
                strSQL = "SELECT ShiftStartDate, Shift, ReworkArea, TeamMember, LotNumber, VehicleLine, SeatStyle, Color, Component, SeatSerialNumber, Category, ReworkReason, ReworkAction, ChargeTo, ReturnedQuantity FROM tblRMDReworkHistory "
            ElseIf (Me.rblType.SelectedValue = "tblRMDMaterialDispositionHistory") Then
                'strSQL = "SELECT DispositionDate, Shift, ReworkArea, TeamMember, LotNumber, VehicleLine, SeatStyle, Color, Component, SeatSerialNumber, Category, PartDescription, PartNumber, DispositionReason, ChargeTo, ReturnQuantity, ScrapQuantity, TotalQuantity FROM tblRMDMaterialDispositionHistory "
                strSQL = "SELECT ShiftStartDate, Shift, ReworkArea, TeamMember, LotNumber, VehicleLine, SeatStyle, Color, Component, SeatSerialNumber, Category, PartDescription, PartNumber, DispositionReason, ChargeTo, ReturnQuantity, ScrapQuantity, TotalQuantity FROM tblRMDMaterialDispositionHistory "
            Else
                Master.Msg = "Error: Unable to determine report type selection."
                Return
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'add Shift filtering to WHERE Clause
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If Me.rblShift.SelectedValue <> "ALL" Then
                strSQL += " WHERE Shift = " & Me.rblShift.SelectedValue
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'add ReworkArea filtering to WHERE Clause
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If strSQL.IndexOf("WHERE") = -1 Then
                strSQL += " WHERE ReworkArea IN ("
            Else
                strSQL += " AND ReworkArea IN ("
            End If

            For Each objListItem In Me.cblAreas.Items
                If objListItem.Selected Then
                    strSQL += "'" & objListItem.Value & "',"
                    strAreas += objListItem.Value & ","
                End If
            Next

            'trim off trailing comma (if exists)
            'If strSQL.Substring(strSQL.Length - 1) = "," Then
            strSQL = strSQL.Substring(0, strSQL.Length - 1) & ")"
            strAreas = strAreas.Substring(0, strAreas.Length - 1)
            'End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'add Date filtering to WHERE Clause
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If strSQL.IndexOf("WHERE") = -1 Then
                If Me.rblType.SelectedIndex = 1 Then
                    strSQL += " WHERE ReworkDate"
                Else
                    strSQL += " WHERE DispositionDate"
                End If
            Else
                If Me.rblType.SelectedIndex = 0 Then
                    strSQL += " AND ReworkDate"
                Else
                    strSQL += " AND DispositionDate"
                End If
            End If
            'strSQL += " BETWEEN '" & Me.tbDayBeg.Text & "' AND '" & Me.tbDayEnd.Text & "'"
            'pb, 03/08/05. changed to go to next day to simulate going to the very end of the currently selected day
            strSQL += " BETWEEN '" & Me.tbDayBeg.Text & "' AND '" & DateAdd(DateInterval.Day, 1, Convert.ToDateTime(Me.tbDayEnd.Text.ToString)) & "'"
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'add Sort Field #1,2,3
            'modified by pb, 03/09/05
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If sort1.Length > 0 Then
                If strSQL.IndexOf("ORDER BY") = -1 Then
                    strSQL += " ORDER BY " & sort1
                Else
                    strSQL += "," & sort1
                End If
                strSQL += " " & Me.rblSort1.SelectedValue
            End If

            If sort2.Length > 0 Then
                If strSQL.IndexOf("ORDER BY") = -1 Then
                    strSQL += " ORDER BY " & sort2
                Else
                    strSQL += "," & sort2
                End If
                strSQL += " " & Me.rblSort2.SelectedValue
            End If

            If sort3.Length > 0 Then
                If strSQL.IndexOf("ORDER BY") = -1 Then
                    strSQL += " ORDER BY " & sort3
                Else
                    strSQL += "," & sort3
                End If
                strSQL += " " & Me.rblSort3.SelectedValue
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'MsgBox(strSQL.ToString)
            oDataset = DA.GetDataSet(strSQL)

            If Not DA.IsDSEmpty(oDataset) Then
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Open and write to Excel using ADO.NET (Reference this article: How To Use ADO.NET to Retrieve and Modify Records in an Excel Workbook With Visual Basic .NET: http://support.microsoft.com/kb/316934/EN-US/)
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                oConn.Open()
                oCommand.CommandType = CommandType.Text
                oCommand.Connection = oConn

                'reset SQL string (now will be used to connect to Excel spreadsheet)
                strSQL = ""

                For Each oDataTable In oDataset.Tables
                    'strBaseSQL = "INSERT INTO [" & oDataTable.Namespace & "$] ("
                    objStringBuilder = New System.Text.StringBuilder("")
                    'objStringBuilder.Append("INSERT INTO [" & oDataTable.Namespace & "$] (")

                    'objStringBuilder.Append("INSERT INTO [Sheet1$] (ReworkDate,Shift,Area,TeamMember,LotNumber,VehicleLine,SeatStyle,Color,Component,SSN,Category,Reason,ReworkAction,ChargeTo,Quantity,Location) ")

                    'objStringBuilder.Append("INSERT INTO [Sheet1$A:P] ") 'this works except that number columns column up with that funny error indicator
                    'objStringBuilder.Append("INSERT INTO [Sheet1$] (F1,F2,F3,F4,F5,F6,F7,F8,F9,F10,F11,F12,F13,F14,F15,F16) ") 'F# is a generic keyword indicating field/column # (this too works except that number columns column up with that funny error indicator)
                    'objStringBuilder.Append("INSERT INTO [Sheet1$] (A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P) ")

                    'This supports up to 26 (A-Z) columns in the Excel spreadsheet
                    objStringBuilder.Append("INSERT INTO [Sheet1$A:" & Chr(oDataTable.Columns.Count + 64) & "] ")

                    'For i = 1 To oDataTable.Columns.Count
                    '    objStringBuilder.Append("F")
                    '    objStringBuilder.Append(Chr(i + 64))

                    '    If i <> oDataTable.Columns.Count Then
                    '        objStringBuilder.Append(",")
                    '    End If
                    'Next
                    'objStringBuilder.Append(") ")

                    'objStringBuilder.Append("INSERT INTO [Sheet1$] (F1,F2,F3,F4,F5,F6,F7,F8,F9,F10,F11,F12,F13,F14,F15,F16) ") 'F# is a generic keyword indicating field/column # (this too works except that number columns column up with that funny error indicator)

                    'For Each oDataColumn In oDataTable.Columns
                    '    objStringBuilder.Append(oDataColumn.ColumnName & ",")
                    'Next

                    strBaseSQL = objStringBuilder.ToString

                    'trim off trailing comma
                    'strBaseSQL = strBaseSQL.Substring(0, strBaseSQL.Length - 1)

                    'finish off Base SQL
                    'strBaseSQL += ") VALUES ("
                    strBaseSQL += "VALUES ("

                    For Each oDataRow In oDataTable.Rows
                        'strSQL = strBaseSQL
                        objStringBuilder = New System.Text.StringBuilder("")
                        objStringBuilder.Append(strBaseSQL)

                        For i = 0 To oDataRow.ItemArray.GetUpperBound(0)
                            'strSQL += "'" & oDataRow.ItemArray(i) & "',"

                            'If i = 1 Or i = 14 Then (Shift or Quantity column)
                            If IsNumeric(oDataRow.ItemArray(i)) Then
                                objStringBuilder.Append("" & oDataRow.ItemArray(i).ToString & ",")
                            Else
                                If oDataRow.ItemArray(i).ToString.Length > 0 Then
                                    objStringBuilder.Append("'" & oDataRow.ItemArray(i).ToString & "',")
                                Else
                                    objStringBuilder.Append("NULL,")
                                End If
                            End If
                        Next

                        strSQL = objStringBuilder.ToString

                        'trim off trailing comma
                        strSQL = strSQL.Substring(0, strSQL.Length - 1)

                        'add ending parenthesis
                        strSQL += ")"

                        'now insert into Excel spreadsheet
                        'DA.OleDBExecSQL(strSQL, strConnectionString)
                        'MsgBox(strSQL.ToString)

                        oCommand.CommandText = strSQL

                        retval = oCommand.ExecuteNonQuery()
                    Next
                Next
            Else
                Master.tMsg("", "No data found for selected criteria.")
                Exit Sub
            End If

            'close connection
            oConn.Close()
            oCommand.Dispose()
            oConn.Dispose()


            'Re-open connection to do cell updates (in header section of report)
            oConn.ConnectionString = strConnectionString.Replace("HDR=YES", "HDR=NO")
            oConn.Open()
            oCommand.CommandType = CommandType.Text
            oCommand.Connection = oConn

            'Update Date & Shift selections
            oCommand.CommandText = "UPDATE [Sheet1$A3:A3] SET F1 = 'Date Range: " & Me.tbDayBeg.Text & " - " & Me.tbDayEnd.Text & "'"
            retval = oCommand.ExecuteNonQuery()

            'Update Areas selected
            oCommand.CommandText = "UPDATE [Sheet1$E3:E3] SET F1 = 'Areas Included: " & strAreas & "'"
            retval = oCommand.ExecuteNonQuery()

            'Update Date of This Report
            oCommand.CommandText = "UPDATE [Sheet1$A4:A4] SET F1 = 'Report Date: " & (Now().Month.ToString() + "/" + Now().Day.ToString() + "/" + Now().Year.ToString()) & "'"
            retval = oCommand.ExecuteNonQuery()

            'close connection
            oConn.Close()
            oCommand.Dispose()
            oConn.Dispose()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Download file in "chunks" (REJ - needed to avoid "Server Application Error" on some systems: http://support.microsoft.com/default.aspx?scid=kb;en-us;812406&Product=aspnet)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim iStream As System.IO.Stream
            Dim buffer(10000) As Byte            ' Buffer to read 10K bytes in chunk:
            Dim length As Integer            ' Length of the file:
            Dim dataToRead As Long           ' Total bytes to read:
            Dim filepath As String = strExportFilePath           ' Identify the file to download including its path.
            Dim filename As String = System.IO.Path.GetFileName(filepath)            ' Identify the file name.

            ' Open the file.
            iStream = New System.IO.FileStream(filepath, System.IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)

            ' Total bytes to read:
            dataToRead = iStream.Length

            Response.ClearContent()
            Response.ClearHeaders()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & filename)

            ' Read the bytes.
            While dataToRead > 0
                ' Verify that the client is connected.
                If Response.IsClientConnected Then
                    ' Read the data in buffer
                    length = iStream.Read(buffer, 0, 10000)

                    ' Write the data to the current output stream.
                    Response.OutputStream.Write(buffer, 0, length)

                    ' Flush the data to the HTML output.
                    Response.Flush()

                    ReDim buffer(10000)                   ' Clear the buffer
                    dataToRead = dataToRead - length
                Else
                    'prevent infinite loop if user disconnects
                    dataToRead = -1
                End If
            End While

            Response.Close()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Finally
            If Not oConn Is Nothing Then
                oConn.Close()
            End If

            oCommand.Dispose()
            oConn.Dispose()
        End Try
    End Sub

#End Region

 
End Class
