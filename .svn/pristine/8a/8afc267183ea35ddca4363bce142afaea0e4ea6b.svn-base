Imports Microsoft.ReportingServices
Imports Microsoft.Reporting.WebForms

Public Class Reports
    Inherits System.Web.UI.Page

    Private reportServerURI As Uri = New Uri(System.Configuration.ConfigurationManager.AppSettings.Get("reportServerURI"))
    Private reportServerFolder As String = System.Configuration.ConfigurationManager.AppSettings.Get("reportServerFolder").ToString()

    Const REPORT_NAME As String = "report"
    Const REPORT_TITLE As String = "webpageTitle"


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not (Page.IsPostBack) Then

                Me.displayReport(Request.QueryString)

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Reports_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            Dim strArrayTitle() As String
            strArrayTitle = Request.QueryString().GetValues(REPORT_TITLE)

            If (strArrayTitle IsNot Nothing) Then
                Master.PageTitle = strArrayTitle(0).Trim()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

#End Region

#Region "Methods"

    'this method displays a report in the reportViewer control
    Private Sub displayReport(ByVal params As NameValueCollection)
        Try
            Dim strReport As String = ""
            Dim strUserName As String = ""
            Dim strPassword As String = ""
            Dim strArrayReport() As String
            Dim paramList As New Generic.List(Of ReportParameter)
            Dim bSendParameters As Boolean = False

            strArrayReport = params.GetValues(REPORT_NAME)

            If (strArrayReport Is Nothing) Then
                Master.eMsg("Report Name is Missing.")
            Else
                strReport = strArrayReport(0).Trim()

                ' get any parameters that may be passed in
                If (params.Count > 0) Then
                    For i = 0 To params.Count - 1
                        If (params.GetKey(i).ToString() <> REPORT_NAME) And (params.GetKey(i).ToString() <> REPORT_TITLE) Then
                            If (params.Get(i).ToLower().Equals("null")) Then
                                paramList.Add(New ReportParameter(params.GetKey(i)))
                            Else
                                paramList.Add(New ReportParameter(params.GetKey(i), params.Get(i), False))
                            End If
                        End If
                    Next
                    bSendParameters = True
                End If


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
                ReportViewer1.DataBind()

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region




End Class