Imports System.Data.SqlClient
Imports System.IO
'Imports Microsoft.Office.Interop

Public Class ProductionScheduleExport
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            Dim dt As Date
            If (Date.TryParse(Request("exportDate"), dt)) Then
                CreateSpreadsheet(dt.ToString("MM/dd/yyyy"), Request("BroadcastPointID"))
            Else
                Response.Write("Invalid Data was sent for the export. </BR></BR>")
                Response.Write("Export Date: " + Request("exportDate") + "</BR>")
                Response.Write("Broadcast Point ID: " + Request("BroadcastPointID") + "</BR>")
            End If

        Catch ex As Exception
            'Response.ContentType = "text"
            Response.Write(ex.ToString())
        End Try
    End Sub

    Private Sub CreateSpreadsheet(ByVal begDT As String, ByVal BroadcastPointID As String)

        Dim oDataset As DataSet
        Dim oDataTable As DataTable
        Dim oDataRow As DataRow
        Dim oDataColumn As DataColumn
        Dim strBaseSQL As String
        Dim strSQL As String
        Dim objStringBuilder As System.Text.StringBuilder = New System.Text.StringBuilder("")
        Dim strTemplateFilePath As String = ""
        Dim strExportFilePath As String = ""
        Dim strConnectionString As String = ""
        Dim i As Integer
        Dim retval As Long

        Try
            If (Session("UserWorkingDirFileSystem") Is Nothing) Then
                Response.Write("Working Directory Not Found")
            Else

                strTemplateFilePath = Server.MapPath(Request.ApplicationPath) & "\2Reporting\ProductionScheduleExport.Excel2000.Template.xls"
                strExportFilePath = Session("UserWorkingDirFileSystem").ToString() & "\ProductionScheduleExport." & Format(Now(), "MMddyy_HHmmss") & ".xls"
                strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" & _
                                    "Data Source=" & strExportFilePath & ";" & _
                                    "Extended Properties=""Excel 12.0 Xml;HDR=YES"""

                'make sure Session directory exists
                If Not Directory.Exists(Session("UserWorkingDirFileSystem").ToString()) Then
                    System.IO.Directory.CreateDirectory(Session("UserWorkingDirFileSystem").ToString())
                End If

                'first make copy of template XLS file
                File.Copy(strTemplateFilePath, strExportFilePath, True)

                oDataset = GetExportedDataset(begDT, BroadcastPointID)

                If DA.IsDataSetNotEmpty(oDataset) Then
                    Using oConn As New OleDb.OleDbConnection(strConnectionString)
                        Using oCommand As New OleDb.OleDbCommand
                            oConn.Open()
                            oCommand.CommandType = CommandType.Text
                            oCommand.Connection = oConn
                            oCommand.CommandTimeout = 300

                            For Each oDataTable In oDataset.Tables
                                objStringBuilder = New System.Text.StringBuilder("")
                                objStringBuilder.Append("INSERT INTO [" & oDataTable.Namespace & "$] (")

                                For Each oDataColumn In oDataTable.Columns
                                    objStringBuilder.Append(oDataColumn.ColumnName & ",")
                                Next

                                strBaseSQL = objStringBuilder.ToString

                                'trim off trailing comma
                                strBaseSQL = strBaseSQL.Substring(0, strBaseSQL.Length - 1)

                                'finish off Base SQL
                                strBaseSQL += ") VALUES ("

                                For Each oDataRow In oDataTable.Rows
                                    'strSQL = strBaseSQL
                                    objStringBuilder = New System.Text.StringBuilder("")
                                    objStringBuilder.Append(strBaseSQL)

                                    For i = 0 To oDataRow.ItemArray.GetUpperBound(0)
                                        If IsNumeric(oDataRow.ItemArray(i)) Then
                                            objStringBuilder.Append("" & oDataRow.ItemArray(i).ToString() & ",")
                                        Else
                                            If oDataRow.ItemArray(i).ToString.Length > 0 Then
                                                objStringBuilder.Append("'" & oDataRow.ItemArray(i).ToString() & "',")
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
                                    oCommand.CommandText = strSQL
                                    '191028 tdye troubleshooting
                                    'Response.Write(strSQL)
                                    'Response.Close()

                                    retval = oCommand.ExecuteNonQuery()
                                Next
                            Next

                        End Using
                    End Using
                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Download file in "chunks" (REJ - needed to avoid "Server Application Error" on some systems: 
                '   http://support.microsoft.com/default.aspx?scid=kb;en-us;812406&Product=aspnet)
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim iStream As System.IO.Stream
                Dim buffer(10000) As Byte ' Buffer to read 10K bytes in chunk:
                Dim length As Integer ' Length of the file:
                Dim dataToRead As Long ' Total bytes to read:
                Dim filepath As String = strExportFilePath ' Identify the file to download including its path.
                Dim filename As String = System.IO.Path.GetFileName(filepath) ' Identify the file name.

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

                        ReDim buffer(10000) ' Clear the buffer
                        dataToRead = dataToRead - length
                    Else
                        'prevent infinite loop if user disconnects
                        dataToRead = -1
                    End If
                End While
            End If

        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try

        Response.Close()

        If Directory.Exists(Session("UserWorkingDirFileSystem").ToString()) Then
            System.IO.Directory.Delete(Session("UserWorkingDirFileSystem").ToString())
        End If

    End Sub

    Private Function GetExportedDataset(ByVal begdt As String, ByVal BroadcastPointID As String) As DataSet
        Dim ds As DataSet
        Dim ds2 As New DataSet
        Dim retval As Boolean = True
        Dim hold As Int32 = 0
        Dim dt As String = ""
        Dim NumDays As Integer = 7
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim SetexFlag As String

        Try

            'SetexFlag = Session("SetexOrders").ToString ' test'

            NumDays = CInt(BizLayer.GetApplicationParameterValue("2250", "0001"))


            SetexFlag = Request("SetexFlag")

            oSqlParameter = New SqlParameter("@BegDT", SqlDbType.VarChar, 30)
            oSqlParameter.Value = begdt
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Report", SqlDbType.Bit)
            oSqlParameter.Value = hold
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@NumDays", SqlDbType.Int)
            oSqlParameter.Value = NumDays
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Export", SqlDbType.Bit)
            oSqlParameter.Value = 1
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = BroadcastPointID
            colParameters.Add(oSqlParameter)

            If SetexFlag <> "0" Then
                ds = DA.GetDataSet("procPSGetProdSched", colParameters)
            Else
                ds = DA.GetDataSet("procPSGetProdSchedHonda", colParameters)
            End If
            ds2.Tables.Add(ds.Tables(ds.Tables.Count - 3).Copy())
            ds2.Tables(0).Namespace = "Production Schedule"
            ds2.Tables.Add(ds.Tables(ds.Tables.Count - 2).Copy())
            ds2.Tables(1).Namespace = "Daily Requirements"
            ds2.Tables.Add(ds.Tables(ds.Tables.Count - 1).Copy())
            ds2.Tables(2).Namespace = "Weekly Requirements"

        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try

        Return ds2
    End Function


End Class
