Imports System.Data.SqlClient
Imports System.Web.Security

Public Class Login
    Inherits System.Web.UI.Page

    Const AUTO_LOGOUT = "AutoLogout"
    Const AUTO_REDIRECT = "AutoRedirect"

    Enum tblUserAccounts
        UserID
        UserTypeID
        FullName
    End Enum


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Auto-populate for ReadOnly account
                txtUserName.Text = "readonly"
                '#If DEBUG Then
                '                'TODO:  remove before going to production release
                '                If (HttpContext.Current.IsDebuggingEnabled) Then
                '                    txtUserName.Text = "tegron"
                '                    txtPassword.Attributes.Add("value", "integrity")
                '                End If
                '#End If
                CheckAndSetReturnURLMessage()
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub CheckAndSetReturnURLMessage()
        Dim strUserName As String = ""
        Dim strReturnURL As String = ""
        Dim strReturnURLPageName As String = ""
        Dim bFormAuthenticationRedirect As Boolean = False

        Try
            strReturnURL = ""
            If (Request("ReturnURL") IsNot Nothing) Then
                ' The Forms Authentication redirected to login.  get the return URL from Request string
                strReturnURL = Request.QueryString("ReturnURL")
                bFormAuthenticationRedirect = True
            End If

            If (strReturnURL.Length > 0) Then

                strReturnURLPageName = Right(strReturnURL, strReturnURL.Length - InStrRev(strReturnURL, "/"))
                If (strReturnURLPageName.Length > 0) Then
                    strReturnURLPageName = GetPageDisplayNameFromURL(strReturnURLPageName)
                End If

                    If (bFormAuthenticationRedirect = True) Then
                        If (Session("UserFirstLastName") IsNot Nothing) Then
                            strUserName = Session("UserFirstLastName").ToString().Trim()
                        End If

                        lblLoginInfo.Text = "<span class='fontRed' >Note:</span> You are here because you attempted to access the secured page <span class='bold'>'" & strReturnURLPageName & "'</span>"

                        If strUserName.Length > 0 Then
                            lblLoginInfo.Text += " with the account <span class='bold'>'" & strUserName & "'</span>, which does not have access to that page."
                            lblLoginInfo.Text += "<br><br>Please perform one of the following step(s):" & _
                                                 "<br><span class='bold'>* Log in with an account that does have access" & _
                                                 "<br>* Navigate to a different page to which you do have access.</span>"
                        Else
                            lblLoginInfo.Text += " and you are either not logged in or your session has expired.<br><br>" & _
                                                 "Please perform one of the following step(s):<br><span class='bold' >* Login.</span>"
                        End If
                    End If
                End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function GetPageDisplayNameFromURL(strReturnURLPageName As String) As String
        Dim strDisplay As String = ""
        Dim strTitle As String
        Dim strChar As String
        Dim strPrevChar As String
        Dim strNextChar As String
        Try

            strTitle = Left(strReturnURLPageName, InStr(strReturnURLPageName, ".") - 1)

            For i = 1 To strTitle.Length()
                strChar = Mid(strTitle, i, 1)

                strPrevChar = ""
                If i > 1 Then
                    strPrevChar = Mid(strTitle, i - 1, 1)
                End If

                strNextChar = ""
                If i < Len(strTitle) Then
                    strNextChar = Mid(strTitle, i + 1, 1)
                End If

                If UCase(strChar) = strChar And (UCase(strPrevChar) <> strPrevChar Or UCase(strNextChar) <> strNextChar) And i <> 1 Then
                    strDisplay += " "
                End If

                strDisplay += strChar
            Next
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return strDisplay

    End Function
    Private Sub btnLogin_Click(sender As Object, e As System.EventArgs) Handles btnLogin.Click
        Try
            Dim strReturnURL As String = ""
            Dim objAuthenticationTicket As FormsAuthenticationTicket
            Dim objCookie As HttpCookie

            If (txtUserName.Text.Trim().Length = 0) Then
                Master.Msg = "You must enter your user name."
            Else
                If AuthenticateUser() Then
                    'Store application parameters in Session Variables
                    StoreApplicationParameters()

                    objAuthenticationTicket = New FormsAuthenticationTicket(1, txtUserName.Text, Now(), Now().AddMinutes(CDbl(Session("SessionTimeout"))), False, Session("UserTypeID").ToString())


                    'Set Session Timeout according to the value just read from the DB
                    Session.Timeout = CInt(Session("SessionTimeout"))

                    'Create the cookie with the name defined in the web.config file and a value of the authentication ticket (encrypted)
                    objCookie = New HttpCookie(FormsAuthentication.FormsCookieName)
                    objCookie.Value = FormsAuthentication.Encrypt(objAuthenticationTicket)

                    Response.Cookies.Add(objCookie)

                    strReturnURL = ""
                    If (Request("ReturnURL") IsNot Nothing) Then
                        ' The Forms Authentication redirected to login.  get the return URL from Request string
                        strReturnURL = Request.QueryString("ReturnURL")
                    End If

                    If (strReturnURL.Length > 0) Then
                        Response.Redirect(strReturnURL, False)
                    Else
                        Response.Redirect("Home.aspx", False)
                    End If

                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

    Private Function AuthenticateUser() As Boolean
        Dim blResult As Boolean = False
        Dim oCommand As New SqlCommand
        Dim strSQL As String
        Dim oDataTable As DataTable

        Try
            'This should defintely be converted to stored procedure
            strSQL = "SELECT UserID,UserTypeID, FirstName + ' ' + LastName FROM tblUserAccounts as UserFirstLastName "
            strSQL += "WHERE LogInName = '" & txtUserName.Text & "' "
            strSQL += "AND Password = '" & txtPassword.Text & "' "

            oDataTable = DA.GetDataSet(strSQL).Tables(0)

            If oDataTable.Rows.Count > 0 Then
                Session("UserName") = txtUserName.Text
                Session("Password") = txtPassword.Text

                With oDataTable.Rows(0)
                    Session("UserFirstLastName") = .Item(tblUserAccounts.FullName).ToString().Trim()
                    Session("UserID") = .Item(tblUserAccounts.UserID).ToString().Trim()
                    Session("UserTypeID") = .Item(tblUserAccounts.UserTypeID).ToString().Trim()
                End With

                blResult = True
            Else
                Master.Msg = "Invalid Username and/or password."
                Master.MsgBlink = True
                Master.MsgColor = "Red"
                blResult = False
            End If
        Catch ex As Exception
            Master.Msg += ex.ToString() & "<BR>"
            Master.MsgColor = "Red"
            Master.MsgBlink = True
        Finally
            oCommand.Dispose()
        End Try

        Return blResult

    End Function

    Private Sub StoreApplicationParameters()
        Session("SessionTimeout") = BizLayer.GetApplicationParameterValue("0103")
        Session("SessionJavascriptTimeout") = BizLayer.GetApplicationParameterValue("0105")
        Session("UserWorkingDirFileSystem") = BizLayer.GetApplicationParameterValue("0106") & "\" & Session.SessionID
    End Sub



End Class