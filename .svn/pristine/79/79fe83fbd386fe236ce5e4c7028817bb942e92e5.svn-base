Imports System.IO
Imports System.Data.SqlClient
Imports Microsoft.ReportingServices
Imports Microsoft.Reporting.WebForms

Public Class WorkInstructionExport
    Inherits System.Web.UI.Page

    Private reportServerURI As Uri = New Uri(System.Configuration.ConfigurationManager.AppSettings.Get("reportServerURI"))
    Private reportServerFolder As String = System.Configuration.ConfigurationManager.AppSettings.Get("reportServerFolder").ToString()

    Private Enum StationReportType
        All = 0
        One = 1
    End Enum

    Private Enum StationType
        ODS = 0
        EDS = 1
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim reportType As Integer
        Dim stationType As Integer
        Dim stationID As String
        Dim masterChangeRequestID As Integer
        Dim includeWatermark As Boolean
        Dim errorMessage As String = ""

        Dim lineName As String
        Dim modelName As String
        Dim mcrNumber As String

        Try
            stationID = Request("StationID")

            If (Integer.TryParse(Request("ReportType"), reportType) = False) Then
                errorMessage = "Report Type (All [0] or Single Station [1]) missing"
            End If

            If (Integer.TryParse(Request("StationType"), stationType) = False) Then
                errorMessage += "Station Type (ODS [0] or Wire Harness [1]) missing"
            End If

            If (errorMessage.Length = 0) Then

                If (Integer.TryParse(Request("MasterChangeRequestID"), masterChangeRequestID) = False) Then
                    errorMessage += " MasterChangeRequestID Missing"
                ElseIf (Boolean.TryParse(Request("IncludeWatermark"), includeWatermark) = False) Then
                    errorMessage += " Include Watermark Value Missing"
                Else

                    Select Case reportType
                        Case StationReportType.One
                            If (stationID.Length = 0) Then
                                errorMessage += " Station Missing"
                            Else
                                mcrNumber = Request("MCRNumber")
                                CreatePDF_OneStation(stationID, masterChangeRequestID, mcrNumber, includeWatermark, stationType)
                            End If
                        Case StationReportType.All
                            lineName = Request("Line")
                            modelName = Request("Model")
                            mcrNumber = Request("MCRNumber")

                            CreatePDF_AllStations(masterChangeRequestID, includeWatermark, lineName, modelName, mcrNumber, stationType)
                    End Select
                End If
            End If

            If (errorMessage.Length > 0) Then
                Response.Write("Invalid Data was sent for the export. </BR></BR>")
                Response.Write(errorMessage + "</BR>")
            End If

        Catch ex As Exception
            Response.ContentType = "text"
            Response.Write(ex.ToString())
        End Try

    End Sub

    Private Sub CreatePDF_OneStation(stationID As String, masterChangeRequestID As Integer, mcrNumber As String, includeWatermark As Boolean, stationType As Integer)
        Try

            Dim strReport As String = ""
            Dim strUserName As String = ""
            Dim strPassword As String = ""
            Dim paramList As New Generic.List(Of ReportParameter)
            Dim bSendParameters As Boolean = False
            Dim pdfFilename = "ODS_Station_" + stationID + "_mcr" + mcrNumber

            strReport = "ODSInstructionSet"

            If stationType = WorkInstructionExport.StationType.EDS Then
                strReport = "EDSInstructionSet"
                pdfFilename = "WireHarness_Station_" + stationID
            End If

            paramList.Add(New ReportParameter("MasterChangeRequestID", masterChangeRequestID.ToString(), False))
            paramList.Add(New ReportParameter("IncludeWatermark", includeWatermark.ToString(), False))
            paramList.Add(New ReportParameter("StationID", stationID, False))
            bSendParameters = True


            'get username and password from web.config
            strUserName = System.Configuration.ConfigurationManager.AppSettings.Get("reportUserName")
            strPassword = Security.ConvertPassword(System.Configuration.ConfigurationManager.AppSettings("reportPassword"))

            'configure the reportviewer control
            ReportViewer1.Reset()
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote
            ReportViewer1.ServerReport.ReportServerCredentials = New ReportServerCredentials(strUserName, strPassword, "")
            ReportViewer1.ServerReport.ReportServerUrl = reportServerURI
            ReportViewer1.ServerReport.ReportPath = reportServerFolder + strReport
            If (bSendParameters) Then
                ReportViewer1.ServerReport.SetParameters(paramList)
            End If
            'ReportViewer1.DataBind()

            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim bytes As Byte()

            bytes = ReportViewer1.ServerReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
            Response.ContentType = "Application/pdf"
            Response.AddHeader("content-disposition", "attachment; filename=" + pdfFilename + ".pdf")
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.Flush()
            Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try

        Response.Close()

        If Directory.Exists(Session("UserWorkingDirFileSystem").ToString()) Then
            System.IO.Directory.Delete(Session("UserWorkingDirFileSystem").ToString())
        End If

    End Sub

    Private Sub CreatePDF_AllStations(masterChangeRequestID As Integer, includeWatermark As Boolean, lineName As String, modelName As String, mcrNumber As String, stationType As Integer)
        Try

            Dim strReport As String = ""
            Dim strUserName As String = ""
            Dim strPassword As String = ""
            Dim paramList As New Generic.List(Of ReportParameter)
            Dim bSendParameters As Boolean = False
            Dim pdfFilename = "ODS_"

            lineName = lineName.Replace(" ", "")
            modelName = modelName.Replace(" ", "")
            mcrNumber = mcrNumber.Replace(" ", "")
            pdfFilename = pdfFilename + lineName + modelName + "_mcr" + mcrNumber

            strReport = "ODSInstructionSet"

            If stationType = WorkInstructionExport.StationType.EDS Then
                strReport = "EDSInstructionSet"
                pdfFilename = "WireHarness_"
            End If

            paramList.Add(New ReportParameter("MasterChangeRequestID", masterChangeRequestID.ToString(), False))
            paramList.Add(New ReportParameter("IncludeWatermark", includeWatermark.ToString(), False))
            bSendParameters = True


            'get username and password from web.config
            strUserName = System.Configuration.ConfigurationManager.AppSettings.Get("reportUserName")
            strPassword = Security.ConvertPassword(System.Configuration.ConfigurationManager.AppSettings("reportPassword"))

            'configure the reportviewer control
            ReportViewer1.Reset()
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote
            ReportViewer1.ServerReport.ReportServerCredentials = New ReportServerCredentials(strUserName, strPassword, "")
            ReportViewer1.ServerReport.ReportServerUrl = reportServerURI
            ReportViewer1.ServerReport.ReportPath = reportServerFolder + strReport
            If (bSendParameters) Then
                ReportViewer1.ServerReport.SetParameters(paramList)
            End If
            'ReportViewer1.DataBind()

            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim bytes As Byte()

            bytes = ReportViewer1.ServerReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
            Response.ContentType = "Application/pdf"
            Response.AddHeader("content-disposition", "attachment; filename=" + pdfFilename + ".pdf")
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.Flush()
            Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try

        Response.Close()

        If Directory.Exists(Session("UserWorkingDirFileSystem").ToString()) Then
            System.IO.Directory.Delete(Session("UserWorkingDirFileSystem").ToString())
        End If

    End Sub
End Class