Imports System.IO

Public Class DebugHistoryExport
    Inherits System.Web.UI.Page


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ds As DataSet
        Dim strCount As String
        Dim strFromDate As String
        Dim strToDate As String
        Dim strApplicationID As String
        Dim strSortField As String

        Try
            strCount = Request.QueryString("strCount")
            strFromDate = Request.QueryString("strFromDate")
            strToDate = Request.QueryString("strToDate")
            strApplicationID = Request.QueryString("strApplicationID")
            strSortField = Request.QueryString("strSortField")

            ds = GetDebugHistoryDataSet(strCount, strFromDate, strToDate, strApplicationID, strSortField)

            CreateSpreadsheet(ds, "DebugHistory.csv")

        Catch ex As Exception
            Response.ContentType = "text"
            Response.Write(ex.ToString())
        End Try

    End Sub

#End Region

#Region "Methods"

    Private Sub CreateSpreadsheet(ByRef ds As DataSet, filename As String)

        Dim oStreamWriter As System.IO.StreamWriter
        Dim oStringWriter As System.IO.StringWriter
        Dim myFileStream As System.IO.FileStream
        Dim workingDir As String
        Dim str As String

        Dim oXR As System.Xml.XmlTextReader
        Dim oXSLT As System.Xml.Xsl.XslCompiledTransform
        Dim i As String
        Dim a() As String
        Dim j As Integer
        Dim oXpath As System.Xml.XPath.XPathDocument
        Dim strPath As String = ""

        Dim removeTempFolder As Boolean = False

        Try
            If DA.IsDataSetNotEmpty(ds) Then

                workingDir = Session("UserWorkingDirFileSystem").ToString()

                If Not Directory.Exists(workingDir) Then
                    removeTempFolder = True
                    System.IO.Directory.CreateDirectory(workingDir.ToString())
                End If

                myFileStream = New  _
                   System.IO.FileStream(workingDir & "\DebugHistory.xml", _
                   System.IO.FileMode.OpenOrCreate, _
                   System.IO.FileAccess.ReadWrite, _
                   System.IO.FileShare.ReadWrite)

                oStreamWriter = New System.IO.StreamWriter(myFileStream)

                ds.WriteXml(oStreamWriter)

                myFileStream.Close()

                oXR = New System.Xml.XmlTextReader(workingDir & "\DebugHistory.xml")
                oXSLT = New System.Xml.Xsl.XslCompiledTransform()

                i = Request.Url.AbsoluteUri
                a = i.Split(CChar("/"))
                For j = 0 To a.GetLength(0) - 2
                    If strPath = "" Then
                        strPath = a(j)
                    Else
                        strPath = strPath & "/" & a(j)
                    End If
                Next

                oXSLT.Load(strPath & "/DebugCSV.xsl")

                oXpath = New System.Xml.XPath.XPathDocument(oXR)
                oStringWriter = New System.IO.StringWriter()

                oXSLT.Transform(oXpath, Nothing, oStringWriter)

                str = oStringWriter.ToString
                str = str.Replace("&nbsp;", " ")             'replacing the ' with empty space

                oStreamWriter = File.CreateText(workingDir & "\" & filename)
                oStreamWriter.Write(str)
                oXR.Close()
                oStreamWriter.Close()

                With Response
                    .Clear()
                    .Charset = ""
                    .Buffer = True
                    .ContentType = "application/excel"
                    .AddHeader("Content-Disposition", "attachment; filename=" & filename)
                    .WriteFile(workingDir & "\" & filename)
                    .Flush()
                    .Close()
                End With

                'HttpContext.Current.ApplicationInstance.CompleteRequest()

                File.Delete(workingDir & "\DebugHistory.xml")
                File.Delete(workingDir & "\" & filename)

                If removeTempFolder Then
                    System.IO.Directory.Delete(workingDir.ToString())
                End If
            End If

        Catch ex As Exception
            Response.ContentType = "text"
            Response.Write(ex.ToString())
        End Try
    End Sub

    Private Function GetDebugHistoryDataSet(strCount As String, strFromDate As String, strToDate As String, strApplicationID As String, strSortField As String) As DataSet
        Dim ds As DataSet = Nothing
        Dim sql As String = ""
        Try

            sql = "SELECT TOP " & strCount

            'Build reminder of SQL 
            sql += " [DateTime] AS DT1, ApplicationID, CONVERT(varchar, [DateTime], 101) + ' ' + CONVERT(varchar, [DateTime], 8) AS [DateTime]," & _
                " UserName, ScreenName, ActionPerformed, UserHostAddress, REPLACE([Description], CHAR(13) + CHAR(10), '<BR>') [Description]" & _
                " FROM [dbo].[tblDebug]"

            sql += " WHERE ([DateTime] BETWEEN '" & strFromDate & " 00:00:00' AND '" & strToDate & " 23:59:59' ) "

            'Filter the data based on the application selected
            If strApplicationID.Length > 0 AndAlso strApplicationID.CompareTo("ALL") <> 0 Then
                sql += " AND ApplicationID='" & strApplicationID & "'"
            End If

            If (strSortField.Length = 0) Then
                strSortField = "DT1 DESC"
            End If
            sql += " ORDER BY " & strSortField

            ds = DA.GetDataSet(sql)

            ds.Tables(0).Columns.Remove("DT1")
            ds.Tables(0).Columns.Remove("ApplicationID")

        Catch ex As Exception
            Response.ContentType = "text"
            Response.Write(ex.ToString())
        End Try

        Return ds

    End Function
#End Region



End Class