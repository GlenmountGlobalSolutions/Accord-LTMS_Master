Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Web.Security
Imports System.Xml
Imports System.Data.SqlClient


Public Class SiteSecurity
    Inherits System.Web.UI.Page

    Private xconfig As New XmlDocument
    Private selectedPage As String
    Private fullPath As String
    Private strNewControlName As String
    Private delFlag As Boolean

#Region "EVent Handlers"

    Private Sub SiteSecurity_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Try
            LoadCheckBoxList()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim strCurrentPage As String = ""
            Dim tNode As TreeNode

            strNewControlName = ""
            delFlag = False

            'load web.config
            xconfig.Load(Request.PhysicalApplicationPath() & "Web.config")

            If Not IsPostBack Then
                Dim RootDir As New System.IO.DirectoryInfo(Request.PhysicalApplicationPath())

                ' output the directory into a node
                Dim RootNode As TreeNode = GetDirectoryContents(RootDir, Nothing, "*.aspx")
                RootNode.Text = ""

                ' add the output to the tree
                treeDirectory.Nodes.Add(RootNode)
                If Request("CurrentPage") IsNot Nothing Then
                    strCurrentPage = Request("CurrentPage")

                    tNode = (treeDirectory.FindNode(strCurrentPage))
                    If tNode IsNot Nothing Then
                        tNode.Selected = True
                        Utility.ExpandParentNode(tNode)
                    End If
                End If
            End If

            If treeDirectory.SelectedNode IsNot Nothing Then
                selectedPage = treeDirectory.SelectedValue  ''set page variable (xxx.aspx)
                fullPath = treeDirectory.SelectedNode.ValuePath ''set full path variable (folder/folder/page)
            Else
                selectedPage = ""
                fullPath = ""
            End If

            If (fullPath.StartsWith("/")) Then
                fullPath = fullPath.Substring(1)
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub SiteSecurity_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try

            'set up security for selected item
            SetupSecurity()

            'hide button checkbox if nothing is selected
            If (Me.lbButtons.SelectedIndex < 0) Then
                Me.cblistButtonAccess.Enabled = False
            Else
                Me.cblistButtonAccess.Enabled = True
            End If

            'disable security for root folder
            If (fullPath.Length <= 0) Then
                Me.cblistSiteAccess.Enabled = False
            Else
                Me.cblistSiteAccess.Enabled = True
            End If

            'create link to the currently selected page
            Me.hlPage.Text = selectedPage
            If (IsAspx(selectedPage)) Then
                Me.hlPage.NavigateUrl = Me.Request.ApplicationPath & "/" & fullPath
                Me.hlPage.Visible = True
                Me.lblCurrent.Visible = True
            Else
                Me.hlPage.Visible = False
                Me.lblCurrent.Visible = False
            End If

            EnableControls()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSecure_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSecure.Click
        SecurePage()
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim path As String = SelectedScreenName()
            Dim dsScreenID As New DataSet
            Dim dsControl As New DataSet
            Dim pathFound As Boolean = False
            Dim screenID As Integer = 0
            Dim userTypeID As Integer = 0
            Dim controlName As String = txtCtrlName.Text

            If (path.Length <= 0) Then
                Master.Msg = "Error: Unable to create control for selected path. <br>Please make sure that an .aspx page has been selected."
            Else

                'get screen ID
                dsScreenID = DA.GetDataSet("SELECT TOP 1 ScreenID FROM tblScreensSecurity WHERE (ScreenPath = '" & path & "')")
                If (dsScreenID.Tables.Count > 0) Then
                    If (dsScreenID.Tables(0).DefaultView.Table.Rows.Count > 0) Then
                        pathFound = True
                    End If
                End If

                If (pathFound = False) Then
                    dsScreenID = DA.GetDataSet("INSERT INTO tblScreensSecurity (ScreenPath) VALUES ('" & path & "') SELECT IDENT_CURRENT('tblScreensSecurity') AS ScreenID")
                End If

                'error checking
                If (dsScreenID.Tables.Count <= 0) Then
                    Master.Msg = "Error: Unable to create an entry for the selected path. <br>Please make sure that an .aspx page has been selected."
                    Exit Sub
                End If
                If (dsScreenID.Tables(0).DefaultView.Table.Rows.Count <= 0) Then
                    Master.Msg = "Error: Unable to create an entry for the selected path. <br>Please make sure that an .aspx page has been selected."
                    Exit Sub
                End If

                screenID = Convert.ToInt32(dsScreenID.Tables(0).DefaultView.Table.Rows(0)(0))

                'make sure that the control does not already exist
                dsControl = DA.GetDataSet("SELECT ScreenID FROM tblScreensControlsSecurity WHERE (ScreenID = " & screenID & ") AND (ControlName = '" & controlName & "')")
                If (dsControl.Tables.Count > 0) Then
                    If (dsControl.Tables(0).DefaultView.Table.Rows.Count > 0) Then
                        Master.Msg = "Error: control with that name already exists. <br>Please type in a different control name."
                        Exit Sub
                    End If
                End If


                For i = 0 To (Me.cblistUserTypes.Items.Count - 1)
                    'If (Me.cblistUserTypes.Items(i).Selected = True) Then
                    If (Me.cblistUserTypes.Items(i).Selected = True) Then
                        userTypeID = CInt(Me.cblistUserTypes.Items(i).Value)
                        'insert new record into the controls security table
                        DA.ExecSQL("INSERT INTO tblScreensControlsSecurity (ScreenID, ControlName, UserTypeID) VALUES  (" & screenID & ", '" & controlName & "', " & userTypeID & ")")
                    End If
                Next

                Master.tMsg("New", "Success! Control " & controlName & " was created for " & path)

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim selectedUserTypes As New ArrayList
            Dim message As String = ""

            For i = 0 To (Me.cblistButtonAccess.Items.Count - 1)
                If (Me.cblistButtonAccess.Items(i).Selected = True) Then
                    selectedUserTypes.Add(Me.cblistButtonAccess.Items(i).Value)
                End If
            Next


            Security.SaveControlSecurity(selectedPage, Me.lbButtons.SelectedItem.Text, selectedUserTypes, message)

            Master.eMsg(message)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim screenID As Integer = -1
        Dim dsScreenID As New DataSet

        Try
            'get screen id
            dsScreenID = DA.GetDataSet("SELECT TOP 1 ScreenID FROM tblScreensSecurity WHERE (ScreenPath = '" & selectedPage & "')")
            If (dsScreenID.Tables.Count > 0) Then
                If (dsScreenID.Tables(0).DefaultView.Table.Rows.Count > 0) Then
                    screenID = CInt(dsScreenID.Tables(0).DefaultView.Table.Rows(0)(0))
                End If
            End If

            If (screenID <= 0) Then
                Master.tMsg("Delete", "Error: unable to locate ScreenID record in tblScreensSecurity for path " & selectedPage)
                Exit Sub
            End If

            'execute sql statement
            DA.ExecSQL("DELETE FROM tblScreensControlsSecurity WHERE (ControlName = '" & Me.lbButtons.SelectedItem.Text & "') AND (ScreenID = " & screenID & ")")

            'log event
            Master.tMsg("Delete", "Control " & Me.lbButtons.SelectedItem.Text & " for screen name " & selectedPage & " has been deleted.")

            delFlag = True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region


#Region "Private Methods"

    Private Sub SetupSecurity()
        Try
            LoadSiteCheckBoxList()
            LoadButtonAccessListBox()
            LoadButtonAccessCheckBoxList()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub CreateWebConfigNode(ByVal path As String, ByVal allowUserList As String, Optional ByVal denyUserList As String = "*")
        Dim locationNode As XmlNode
        Dim pathAttribute As XmlAttribute
        Dim systemWebNode As XmlNode
        Dim authorizationNode As XmlNode
        Dim allowNode As XmlNode
        Dim allowAttribute As XmlAttribute
        Dim denyNode As XmlNode
        Dim denyAttribute As XmlAttribute

        'create nodes
        locationNode = xconfig.CreateElement("location")
        systemWebNode = xconfig.CreateElement("system.web")
        authorizationNode = xconfig.CreateElement("authorization")
        allowNode = xconfig.CreateElement("allow")
        denyNode = xconfig.CreateElement("deny")

        'create attributes
        pathAttribute = xconfig.CreateAttribute("path")
        pathAttribute.Value = path
        allowAttribute = xconfig.CreateAttribute("roles")
        allowAttribute.Value = allowUserList
        denyAttribute = xconfig.CreateAttribute("users")
        denyAttribute.Value = denyUserList

        'assign attributes 
        locationNode.Attributes.SetNamedItem(pathAttribute)
        allowNode.Attributes.SetNamedItem(allowAttribute)
        denyNode.Attributes.SetNamedItem(denyAttribute)

        'append nodes
        authorizationNode.AppendChild(allowNode)
        authorizationNode.AppendChild(denyNode)
        systemWebNode.AppendChild(authorizationNode)
        locationNode.AppendChild(systemWebNode)
        xconfig.DocumentElement.AppendChild(locationNode)

    End Sub

    Private Sub LoadCheckBoxList()
        'bind checkbox list
        Me.cblistUserTypes.DataSource = DA.GetDataSet_tblUserTypes()
        Me.cblistUserTypes.DataTextField = "Description"
        Me.cblistUserTypes.DataValueField = "UserTypeID"
        Me.cblistUserTypes.DataBind()
    End Sub

#Region "TreeView"

    Private Function GetDirectoryContents(ByVal directory As System.IO.DirectoryInfo, ByVal parentNode As TreeNode, ByVal fileSearchPattern As String) As TreeNode
        Dim returnNode As TreeNode = Nothing

        Try
            ' validate param
            If directory Is Nothing Then
                Return Nothing
            End If

            ' create a node for this directory
            Dim DirNode As New TreeNode(directory.Name)



            ' get subdirectories of the current directory
            Dim SubDirectories As System.IO.DirectoryInfo() = directory.GetDirectories()

            ' output each subdirectory
            For DirectoryCount As Integer = 0 To SubDirectories.Length - 1
                'Only Display Folders that start with a number
                If Regex.IsMatch(SubDirectories(DirectoryCount).Name, "^\d") Then
                    GetDirectoryContents(SubDirectories(DirectoryCount), DirNode, fileSearchPattern)
                End If
            Next DirectoryCount

            ' output the current directories files
            Dim Files As System.IO.FileInfo() = directory.GetFiles(fileSearchPattern)

            For FileCount As Integer = 0 To Files.Length - 1
                If (Files(FileCount).Name.ToUpper() <> "Logout.aspx".ToUpper() And _
                    Files(FileCount).Name.ToUpper() <> "Login.aspx".ToUpper()) Then

                    Dim tNode As New TreeNode(Files(FileCount).Name)
                    DirNode.ChildNodes.Add(tNode)

                End If
            Next FileCount

            ' if the parent node is null, return this node
            ' otherwise add this node to the parent and return the parent
            If parentNode Is Nothing Then
                returnNode = DirNode
            Else
                parentNode.ChildNodes.Add(DirNode)
                returnNode = parentNode
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return returnNode

    End Function






    Private Function IsAspx(ByVal str As String) As Boolean
        Dim bResult As Boolean = False

        If (str.IndexOf(".aspx") >= 0) Then
            bResult = True
        End If

        Return bResult
    End Function

    Private Sub EnableControls()
        cmdSecure.Enabled = True
        cmdSave.Enabled = True
        cmdNew.Enabled = True
        cmdDelete.Enabled = True

        If (IsAspx(selectedPage) = False) Then
            cmdSave.Enabled = False
            cmdNew.Enabled = False
            cmdDelete.Enabled = False
        End If

        If (Me.lbButtons.SelectedIndex < 0) Then
            cmdSave.Enabled = False
            cmdDelete.Enabled = False
        End If

        If (fullPath.Length <= 0) Then
            Me.cmdSecure.Enabled = False
        End If

    End Sub

    Private Function xmlSelectedIndex(ByVal path As String) As Integer
        Dim i As Integer
        Dim intResult As Integer = -1

        If (path.Length > 0) Then
            For i = 0 To (xconfig.SelectNodes("configuration/location").Count - 1)
                If (path = xconfig.SelectNodes("configuration/location").Item(i).Attributes(0).Value().ToString()) Then
                    'mark node as found
                    intResult = i
                    Exit For
                End If
            Next
        End If

        Return intResult
    End Function

    Private Function SelectedScreenName() As String
        Dim strPageName As String = ""

        If treeDirectory.SelectedNode IsNot Nothing Then
            strPageName = treeDirectory.SelectedNode.ValuePath
        End If

        strPageName = Right(strPageName, Len(strPageName) - InStrRev(strPageName, "/"))
        Return strPageName
    End Function

#End Region

#Region "Button Access Security"

    Private Sub LoadSiteCheckBoxList()
        Dim i As Integer
        Dim j As Integer
        Try
            Me.cblistSiteAccess.DataSource = DA.GetDataSet_tblUserTypes
            Me.cblistSiteAccess.DataTextField = "Description"
            Me.cblistSiteAccess.DataValueField = "UserTypeID"
            cblistSiteAccess.DataBind()

            'get index of node (in web.config) that contains security settings for currently selected item
            Dim xmlIndex As Integer = xmlSelectedIndex(fullPath)

            'no entry in web.config - select all checkboxes and display unlocked pic
            If (xmlIndex < 0) Then
                For i = 0 To (Me.cblistSiteAccess.Items.Count - 1)
                    Me.cblistSiteAccess.Items(i).Selected = True
                Next
                imLocked.Visible = True
                imLocked.ImageUrl = "~/Images/UnLocking.gif"
                Exit Sub
            End If

            Dim allowList As String
            allowList = xconfig.SelectNodes("configuration/location").Item(xmlIndex).ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes(0).Value

            Dim AllowNums As String()
            AllowNums = allowList.ToString().Split(CChar(","))
            'loop through checkbox list and select items that appear in web.config allow list
            For i = 0 To (AllowNums.Length - 1)
                j = Me.cblistSiteAccess.Items.IndexOf(cblistSiteAccess.Items.FindByValue(AllowNums(i)))
                If (j >= 0) Then
                    Me.cblistSiteAccess.Items(j).Selected = True
                End If
            Next

            imLocked.Visible = True
            imLocked.ImageUrl = "~/Images/Locked.gif"
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadButtonAccessListBox()
        Dim sql As String
        Dim ds As DataSet
        Try

            'exit if an item is selected
            Dim index As Integer = Me.lbButtons.SelectedIndex

            sql = "SELECT DISTINCT tblScreensSecurity.ScreenPath, tblScreensControlsSecurity.ControlName FROM tblScreensSecurity INNER JOIN tblScreensControlsSecurity ON tblScreensSecurity.ScreenID = tblScreensControlsSecurity.ScreenID  WHERE  tblScreensSecurity.ScreenPath = '" & selectedPage & "'"

            ds = DA.GetDataSet(sql)

            Me.lbButtons.DataSource = ds
            Me.lbButtons.DataTextField = "ControlName"
            Me.lbButtons.DataValueField = "ControlName"
            Me.lbButtons.DataBind()

            'select newly created control
            If (strNewControlName.Length > 0) Then
                If (Me.lbButtons.Items.IndexOf(Me.lbButtons.Items.FindByText(strNewControlName)) >= 0) Then
                    Me.lbButtons.SelectedIndex = Me.lbButtons.Items.IndexOf(Me.lbButtons.Items.FindByText(strNewControlName))
                    Exit Sub
                End If
            End If

            If (delFlag = False) And (Me.lbButtons.Items.Count >= index) Then
                Me.lbButtons.SelectedIndex = index
            Else
                Me.lbButtons.SelectedIndex = -1
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadButtonAccessCheckBoxList()
        Dim i As Integer
        Dim j As Integer
        Dim dsButtonSecurity As New DataSet

        Me.cblistButtonAccess.DataSource = DA.GetDataSet_tblUserTypes
        Me.cblistButtonAccess.DataTextField = "Description"
        Me.cblistButtonAccess.DataValueField = "UserTypeID"
        Me.cblistButtonAccess.DataBind()

        If (Me.lbButtons.SelectedIndex >= 0) Then

            'get all permissions associated with button from db
            dsButtonSecurity = DA.GetDataSet("SELECT DISTINCT tblScreensControlsSecurity.UserTypeID FROM tblScreensSecurity INNER JOIN tblScreensControlsSecurity ON tblScreensSecurity.ScreenID = tblScreensControlsSecurity.ScreenID WHERE (tblScreensSecurity.ScreenPath = '" & selectedPage & "') AND (tblScreensControlsSecurity.ControlName = '" & Me.lbButtons.SelectedItem.Value & "')")

            'Error checking
            If (dsButtonSecurity.Tables.Count > 0) Then
                If (dsButtonSecurity.Tables(0).DefaultView.Table.Rows.Count > 0) Then

                    Dim tmp As String

                    'select permissions contained in db
                    For i = 0 To (dsButtonSecurity.Tables(0).DefaultView.Table.Rows.Count - 1)
                        tmp = dsButtonSecurity.Tables(0).DefaultView.Table.Rows(i)(0).ToString()
                        j = Me.cblistButtonAccess.Items.IndexOf(cblistButtonAccess.Items.FindByValue(tmp))
                        If (j >= 0) Then
                            Me.cblistButtonAccess.Items(j).Selected = True
                        End If
                    Next
                End If
            End If
        End If

    End Sub


#End Region

#End Region

    Private Sub SecurePage()
        Try
            Dim allUnchecked As Boolean = True
            Dim allChecked As Boolean = True
            Dim allowUserList As String = ""
            Dim oldAllowList As String = ""
            Dim oldAllowNums As String()
            Dim oldDescList As String
            Dim newDescList As String
            Dim xmlIndex As Integer
            Dim found As Boolean = False
            Dim j As String
            Dim i As Integer = 0

            'create backup
            xconfig.Save(Request.PhysicalApplicationPath() & "_WebConfigBackup/Web.config.backup." & Date.Now.Month.ToString() & "_" & Date.Now.Day.ToString() & "_" & Date.Now.Year.ToString() & "_" & Date.Now.Hour.ToString() & "_" & Date.Now.Minute.ToString() & "_" & Date.Now.Second.ToString() & "_" & New System.Random().Next().ToString())


            '***************************************
            ' All boxes are Unchecked block
            'if all boxes are Unchecked - display message that this is not valid, do not save and exit 
            While (i < Me.cblistSiteAccess.Items.Count)
                If (Me.cblistSiteAccess.Items(i).Selected = True) Then
                    allUnchecked = False
                    Exit While
                End If
                i += 1
            End While

            If (allUnchecked) Then
                i = 0

                'check to see if old list is empty (every one has access)
                If (oldAllowList <> "") Then
                    'select previously selected items
                    oldAllowNums = oldAllowList.ToString().Split(CChar(","))
                    While (i < oldAllowNums.Length)
                        j = oldAllowNums(i)
                        Me.cblistSiteAccess.Items(cblistSiteAccess.Items.IndexOf(cblistSiteAccess.Items.FindByValue(j))).Selected = True
                        i += 1
                    End While

                Else    'all users have access so select all checkboxes
                    While (i < Me.cblistSiteAccess.Items.Count)
                        Me.cblistSiteAccess.Items(i).Selected = True
                        i += 1
                    End While
                End If

                Master.Msg = "Error: at least one user type must have access to the page!"

                Exit Sub
            End If
            'End of All boxes are Unchecked block
            '**************************************


            'create new strings based on selected checkboxes

            i = 0
            While (i < Me.cblistSiteAccess.Items.Count)
                If (Me.cblistSiteAccess.Items(i).Selected = True) Then
                    allowUserList += Me.cblistSiteAccess.Items(i).Value.ToString() & ","
                End If
                i += 1
            End While
            'remove last ","
            allowUserList = allowUserList.TrimEnd(CChar(","))

            xmlIndex = xmlSelectedIndex(fullPath)

            'find node in web.config
            If (xmlIndex >= 0) Then
                found = True
            End If

            If (found) Then
                'get old list
                oldAllowList = xconfig.SelectNodes("configuration/location").Item(xmlIndex).ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes(0).Value
                'remove item (it will be added again)
                xconfig.DocumentElement.RemoveChild(xconfig.SelectNodes("configuration/location").Item(xmlIndex))
            End If

            oldDescList = Security.GetDescriptionList(oldAllowList)
            newDescList = Security.GetDescriptionList(allowUserList)
            If (oldDescList = "") Then
                oldDescList = "Everyone"
            End If
            If (newDescList = "") Then
                newDescList = "Everyone"
            End If




            '***************************************
            ' All boxes are Checked block
            'if all boxes are Checked - remove entry from web.config, set unlocked image, save web.config and exit

            i = 0
            While (i < Me.cblistSiteAccess.Items.Count)
                If (Me.cblistSiteAccess.Items(i).Selected = False) Then
                    allChecked = False
                    Exit While
                End If
                i += 1
            End While

            If (allChecked) Then
                imLocked.ImageUrl = "~/Images/UnLocking.gif"
                imLocked.Visible = True
                'save web.config
                xconfig.Save(Request.PhysicalApplicationPath() & "Web.config")
                'log event
                Master.tMsg("Save", "Allow user types access list for: " & SelectedScreenName() & " has changed from: " & oldDescList & " to: Everyone")

                Exit Sub
            End If
            'End of All boxes are Unchecked block
            '**************************************

            'create node
            CreateWebConfigNode(fullPath, allowUserList)

            'save web.config
            xconfig.Save(Request.PhysicalApplicationPath() & "Web.config")

            'record event
            Master.tMsg("Save", "Allow user types access list for: " & SelectedScreenName() & " has changed from: " & oldDescList & " to: " & newDescList)

            imLocked.ImageUrl = "~/Images/Locked.gif"
            imLocked.Visible = True


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

End Class