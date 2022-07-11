Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use

        'Below is used to extract the roles from the custom-built forms authentication cookie (with roles assigned from DB)
        'and assign them to the the HTTPContext user and thus enable role based forms authentication outside
        'in a custom role based environment (i.e. when not using Windows NT Groups/Roles)
        If HttpContext.Current.User IsNot Nothing Then
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                If HttpContext.Current.User.Identity.AuthenticationType = "Forms" Then
                    Dim id As FormsIdentity = CType(HttpContext.Current.User.Identity, FormsIdentity)
                    Dim ticket As FormsAuthenticationTicket = id.Ticket
                    Dim userData As String = ticket.UserData
                    Dim roles() As String = userData.Split(CChar(","))

                    HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(id, roles)

                End If
            End If
        End If
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends

    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class