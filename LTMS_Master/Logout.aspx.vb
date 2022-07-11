Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FormsAuthentication.SignOut()
        Session("UserFirstLastName") = ""
        Session.Clear()
        Session.Abandon()
        Response.Redirect("~/Login.aspx", True)

    End Sub

End Class