Imports Microsoft.Reporting.WebForms
Imports System.Security.Principal
Imports System.Net

Public Class ReportServerCredentials
    Implements IReportServerCredentials

    Private _userName As String
    Private _password As String
    Private _domain As String


    Public Sub New(ByVal userName As String, ByVal password As String, ByVal domain As String)
        _userName = userName
        _password = password
        _domain = domain
    End Sub

    Public ReadOnly Property ImpersonationUser() As WindowsIdentity Implements IReportServerCredentials.ImpersonationUser
        Get
            ' Use default identity.
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property NetworkCredentials() As ICredentials Implements IReportServerCredentials.NetworkCredentials
        Get
            ' Use default identity.
            Return New NetworkCredential(_userName, _password, _domain)
        End Get
    End Property

    Public Function GetFormsCredentials(ByRef authCookie As Cookie, ByRef user As String, ByRef password As String, ByRef authority As String) As Boolean Implements IReportServerCredentials.GetFormsCredentials
        ' Do not use forms credentials to authenticate.
        authCookie = Nothing
        authority = Nothing
        password = authority
        user = password
        Return False
    End Function

End Class
