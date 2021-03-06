Imports System.ComponentModel
Imports System.IO
Imports System.Xml


Public Class Site
    Inherits System.Web.UI.MasterPage

    Private xconfig As New XmlDocument
    Dim strTitle As String = ""
    Dim strUserTypeID As String = ""
    Dim bMsgBlink As Boolean
    Dim strAlert As String

    Private bRebindSecurityControlDataSet As Boolean


#Region "Properties"


    Public Property PageName() As String
        Get
            Return ViewState("PageName").ToString()
        End Get
        Set(ByVal Value As String)
            ViewState("PageName") = Value
        End Set
    End Property

    Public Property PageTitle() As String

        Get
            Return HyperLinkRefreshPage.Text
        End Get
        Set(value As String)
            HyperLinkRefreshPage.Text = value
        End Set
    End Property

    Public Property HeaderDescription() As String
        'writes message at top of window
        Get
            Return lblHeaderDescription.Text
        End Get
        Set(ByVal Value As String)
            lblHeaderDescription.Text = Value
        End Set
    End Property

    Public Property Msg() As String
        'writes message at top of window
        Get
            Return lblMessage.Text
        End Get
        Set(ByVal Value As String)
            lblMessage.Text = Value
            lblMessage.ToolTip = Value
        End Set
    End Property

    Public Property MsgBlink() As Boolean
        'writes message at top of window
        Get
            Return bMsgBlink
        End Get
        Set(ByVal Value As Boolean)
            bMsgBlink = Value
            ApplyFormatingToMsg()
        End Set
    End Property

    Public Property MsgColor() As String
        'writes message at top of window
        Get
            Return CStr(ViewState("MsgColor"))
        End Get
        Set(ByVal Value As String)
            ViewState("MsgColor") = Value
            ApplyFormatingToMsg()
        End Set
    End Property

    Public Property Alert() As String
        Get
            Return strAlert
        End Get
        Set(value As String)
            Try
                Dim i As Random
                i = New System.Random(5)
                i.Next(1, 10).ToString()

                strAlert = value.Replace(Chr(39), "")                     'replacing the ' with empty space
                strAlert = strAlert.Replace(Chr(10), "\n")                    'replacing the ' with empty space
                strAlert = strAlert.Replace("\x", "\X")                   'replacing the ' with empty space
                Msg = strAlert
                strAlert = strAlert.Replace("\", "\\")                    'replacing the ' with empty space
                Page.ClientScript.RegisterStartupScript(Me.GetType, "AlertFromServer" & i.Next(1, 10).ToString(), "<script language=""javascript""> alert('" & strAlert & "'); </script>")

                MsgColor = "Red"
                MsgBlink = True
            Catch

            End Try
        End Set
    End Property


    Public Sub eMsg(Optional ByVal strCustomAddInfo As String = "")
        'this method is only called by RunTimeError_.aspx to log unheandled error
        LogErr("Runtime Error", Page, strCustomAddInfo)
        Msg = strCustomAddInfo
        MsgBlink = True

    End Sub


    Friend Sub Secure(ByVal oControl As Button)
        Secure(oControl, oControl.Text)
    End Sub

    Friend Sub Secure(ByVal oControl As LinkButton)
        Secure(oControl, oControl.Text)
    End Sub

    Friend Sub Secure(ByVal oControl As DropDownList)
        Secure(oControl, oControl.Text)
    End Sub

    Friend Sub Secure(ByVal oControl As ImageButton)
        Secure(oControl, oControl.ID)
    End Sub

    Friend Sub Secure(ByVal oControl As CheckBox)
        Secure(oControl, oControl.Text)
    End Sub

    Friend Sub Secure(ByVal oControl As WebControl, ByVal controlText As String)
        Dim sR As StringReader
        Dim ds As DataSet = Nothing
        Dim DR() As DataRow = Nothing
        Dim XMLDS As Object
        Dim sql As String
        Dim bEnableControl As Boolean = False

        Try
            If oControl.ID = "Login" Then
                oControl.Enabled = True
            Else
                If Session("UserTypeID") IsNot Nothing Then

                    sql = "SELECT ss.ScreenID, ss.ScreenPath, scs.ControlName, scs.UserTypeID  " & _
                        "  FROM tblScreensSecurity ss INNER JOIN  tblScreensControlsSecurity scs ON ss.ScreenID = scs.ScreenID " & _
                        "  WHERE ss.ScreenPath = '" & PageName & "' AND scs.UserTypeID = " & Session("UserTypeID").ToString()

                    XMLDS = ViewState("XMLDS")
                    'OrElse XMLDS.ToString() <> "<NewDataSet />"

                    ' determine if we get the dataset from the database or from viewstate
                    If (XMLDS Is Nothing OrElse XMLDS.ToString() = "") Then
                        bRebindSecurityControlDataSet = True
                    End If

                    If bRebindSecurityControlDataSet = True Then
                        ds = DA.GetDataSet(sql)
                        ViewState("XMLDS") = ds.GetXml()
                        bRebindSecurityControlDataSet = False                         'resetting flag 
                    Else
                        If XMLDS IsNot Nothing Then
                            sR = New StringReader(XMLDS.ToString())
                            ds = New DataSet
                            ds.ReadXml(sR)
                        End If
                    End If

                    ' we should have a dataset at this point, double check
                    If ds IsNot Nothing Then

                        If ds.Tables.Count > 0 Then     'if Count = 0 this is when no data is in db to secure controls, so disable the conrol
                            DR = ds.Tables(0).Select("ControlName ='" & controlText & "'")

                            If DR.Length = 0 Then       ' look for control by id if text didn't work
                                DR = ds.Tables(0).Select("ControlName ='" & oControl.ID.ToString() & "' AND UserTypeID=" & Session("UserTypeID").ToString())
                            End If

                            If DR.Length > 0 Then   'control is secured
                                'if usertype is >= controltype then enable ctrl
                                If (Session("UserTypeID").ToString() = DR(0)("UserTypeID").ToString()) Then
                                    bEnableControl = True
                                End If
                            End If
                        Else
                            'when control is not secured in the db then disable it by default
                            bEnableControl = False
                        End If
                    End If

                End If
            End If

        Catch ex As Exception
            eMsg(ex.ToString())
        End Try

        oControl.Enabled = bEnableControl

    End Sub


#End Region

#Region "Event Handlers"

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Try
            PageTitle = RequestPage()
        Catch ex As Exception
            Msg = ex.ToString()
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If HttpContext.Current.User IsNot Nothing Then
                If HttpContext.Current.User.Identity.IsAuthenticated Then
                    If HttpContext.Current.User.Identity.Name.Length > 0 Then
                        lblUser.Text = Page.User.Identity.Name.ToString()
                    End If
                Else
                    mnMainMenu.Visible = False
                End If
            End If

            If Session("UserTypeID") Is Nothing OrElse Session("UserTypeID").ToString() = "" Then
                'if the user is still authenticated, but the session has timed out, kill the authentication
                If HttpContext.Current.User IsNot Nothing Then
                    If HttpContext.Current.User.Identity.IsAuthenticated Then
                        FormsAuthentication.SignOut()
                        Session("UserFirstLastName") = ""
                        Session.Clear()
                        Session.Abandon()
                    End If
                End If

                RemoveMenuAndRedirectToLogin()

            End If

            Page.Header.DataBind()

            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetExpires(Now())
            Response.AddHeader("Pragma", "no-cache")


            If Not IsPostBack Then
                LoadUserTypesforSeucrityPopUpForControls()
            End If

        Catch ex As Exception
            Msg = ex.ToString()
        End Try

    End Sub

    Private Sub cmdSecure_Click(sender As Object, e As System.EventArgs) Handles cmdSecure.Click
        Try
            SecureControl()
        Catch ex As Exception
            eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub HyperLinkRefreshPage_PreRender(sender As Object, e As System.EventArgs) Handles HyperLinkRefreshPage.PreRender
        HyperLinkRefreshPage.NavigateUrl = Request.Path.ToString()
    End Sub

    Private Sub linkBtnLogout_Click(sender As Object, e As System.EventArgs) Handles linkBtnLogout.Click
        Try
            LogoutUser()
        Catch ex As Exception
            eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub linkBtnSecurity_Click(sender As Object, e As System.EventArgs) Handles linkBtnSecurity.Click
        Try
            Dim strURL As String = ResolveUrl("~/4SystemAdministration/2SecurityManagement/2SiteSecurity/SiteSecurity.aspx")
            Dim strRequestPath = Page.Request.FilePath()
            Dim strAppPath As String = Request.ApplicationPath()

            If (strAppPath.Length > 1) Then
                strRequestPath = strRequestPath.Replace(strAppPath, "")
            End If

            strURL += "?CurrentPage=" & strRequestPath
            Response.Redirect(strURL, False)
        Catch ex As Exception
            eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SecureControl()
        Try
            Dim selectedUserTypes As New ArrayList(hidCheckedUserTypes.Value.Split(","c))
            Dim message As String = ""
            Dim pageName = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            Security.SaveControlSecurity(pageName, hidClickedControlName.Value, selectedUserTypes, message)
            Msg = message

        Catch ex As Exception
            eMsg(ex.ToString())
        End Try
    End Sub


#End Region


#Region "Private Methods"

    Private Sub RemoveMenuAndRedirectToLogin()
        Try
            mnMainMenu.Visible = False

            If PageName.ToUpper() <> "Login.aspx".ToUpper() Then
                If PageName.ToUpper() = "Logout.aspx".ToUpper() Then
                    Response.Redirect("~/Login.aspx", False)
                Else
                    Response.Redirect("~/Login.aspx?ReturnURL=" & Request.RawUrl, False)
                End If

            End If

        Catch ex As Exception
            Msg = ex.ToString()
        End Try
    End Sub

    Private Function RequestPage() As String
        Dim i As Integer
        Dim strPrevChar As String = ""
        Dim strChar As String = ""
        Dim strNextChar As String = ""
        Dim strDisplay As String = ""
        Dim strPageName As String

        Try
            strPageName = Request.ServerVariables("PATH_INFO")
            strPageName = Right(strPageName, Len(strPageName) - InStrRev(strPageName, "/"))

            If (strPageName.Length > 0) Then

                PageName = strPageName
                strTitle = Left(strPageName, InStr(strPageName, ".") - 1)
                strTitle = strTitle.Replace("_", "")

                For i = 1 To Len(strTitle)
                    strChar = Mid(strTitle, i, 1)

                    If i > 1 Then
                        strPrevChar = Mid(strTitle, i - 1, 1)
                    Else
                        strPrevChar = ""
                    End If

                    If i < Len(strTitle) Then
                        strNextChar = Mid(strTitle, i + 1, 1)
                    Else
                        strNextChar = ""
                    End If

                    'prepend non-breaking space if this character is uppercase and either the last character isn't uppercase or the next next character isn't uppercase
                    If UCase(strChar) = strChar And (UCase(strPrevChar) <> strPrevChar Or (UCase(strNextChar) <> strNextChar And strNextChar <> "s")) Then
                        strDisplay += " "
                    End If

                    strDisplay += strChar
                Next
            End If

            strDisplay = strDisplay.Trim()

        Catch ex As Exception
            eMsg(ex.ToString())
        End Try

        Return strDisplay
    End Function

    Private Sub ApplyFormatingToMsg()
        If Not MsgColor = "" Then
            lblMessage.Style.Remove("Color")
            lblMessage.Style.Add("Color", MsgColor)
        Else
            lblMessage.Style.Remove("Color")
        End If
        If MsgBlink = True Then
            Me.Page.Page.ClientScript.RegisterStartupScript(Me.GetType, "blink1", "<script type=""text/javascript"">BlinkMessage();</script>")
        End If
    End Sub


#End Region


#Region "Public Methods"

    Public Sub tMsg(ByRef Action As String, ByRef strMsg As String, Optional ByRef bBlink As Boolean = False, Optional ByRef Color As String = "")
        LogTrc(Action, Page, strMsg)
        Msg = strMsg
        MsgBlink = bBlink
        MsgColor = Color
    End Sub

    Public Sub LoadUserTypesforSeucrityPopUpForControls()
        Try
            With cblistButtonAccess
                .DataSource = DA.GetDataSet_tblUserTypes()
                .DataTextField = "Description"
                .DataValueField = "UserTypeID"
                .DataBind()
            End With
        Catch ex As Exception
            eMsg(ex.ToString())
        End Try
    End Sub

    Public Function InsertRightClickContextMenu() As String
        Dim strResult As String = ""
        Dim intUserType As Integer
        Try
            If Session("UserTypeID") IsNot Nothing Then
                If Integer.TryParse(Session("UserTypeID").ToString(), intUserType) Then
                    If intUserType = modEnum.UserType.Administrator Then
                        ' this will make the call to the javascript function 
                        strResult = "AddRightClickContextMenu();"
                    End If
                End If
            End If
        Catch ex As Exception
            eMsg(ex.ToString())
        End Try

        Return strResult
    End Function

    Public Sub LogoutUser()
        Try
            Dim strURL As String = ""

            FormsAuthentication.SignOut()
            Session("UserFirstLastName") = ""
            Session.Clear()
            Session.Abandon()

            Response.Redirect("~/Login.aspx", False)
            
        Catch ex As Exception
            eMsg(ex.ToString())
        End Try
    End Sub

    'called via an eval() statement to set the value in javascript
    Public Function GetSessionTimeoutFromApplicationParameters() As String
        Dim strResult As String = "600000" ' 10 minutes
        Try
            ''Session("SessionJavascriptTimeout") is set after login from tblApplicationParameters on page Login.aspx
            If (Session("SessionJavascriptTimeout") IsNot Nothing) Then
                strResult = Session("SessionJavascriptTimeout").ToString()
            End If
        Catch ex As Exception
            eMsg(ex.ToString())
        End Try

        Return strResult
    End Function

    Public Function SetScriptManagerTimeout(seconds As Integer) As Integer
        Dim oldValue As Integer = ScriptManager1.AsyncPostBackTimeout
        ScriptManager1.AsyncPostBackTimeout = seconds
        Return oldValue
    End Function

#End Region


End Class