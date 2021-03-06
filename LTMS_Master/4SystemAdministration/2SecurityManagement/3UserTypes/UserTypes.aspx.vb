Imports System.Xml

Public Class UserTypes
    Inherits System.Web.UI.Page

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                LoadListboxes()
            End If

            EnableControls()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub



    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try

            Dim strNewType As String
            Dim strFromID As String
            Dim dsNewID As New DataSet

            strNewType = txtUserType.Text
            strFromID = ddlUserTypes.SelectedValue

            'create new user type
            If (strNewType.Length() > 0) Then
                dsNewID = DA.GetDataSet("INSERT INTO tblUserTypes (Description) VALUES ('" & strNewType & "') SELECT IDENT_CURRENT('tblUserTypes')")
                'log event
                Master.tMsg("New", "New User Type Created: " & strNewType)
            End If

            'perform error checking
            If (strFromID.Length > 0) Then
                If (dsNewID.Tables.Count > 0) Then
                    If (dsNewID.Tables(0).DefaultView.Table.Rows.Count > 0) Then
                        Dim newID As String
                        newID = (dsNewID.Tables(0).DefaultView.Table.Rows(0)(0)).ToString()
                        If (Convert.ToInt32(strFromID) > 0) Then                             'perform inheritance only if user did not select "None"
                            'TODO:  Evaluate after all pages are complete if this method is still required
                            ExecWebConfigInheritance(strFromID, newID)
                            ExecSQLInheritance(strFromID, newID)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally
            LoadListboxes()
        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            DA.ExecSQL("DELETE FROM tblUserTypes WHERE (UserTypeID = " & Me.lbUserTypes.SelectedItem.Value & ")")

            'delete all references to this user type from the controls security table
            DA.ExecSQL("DELETE FROM tblScreensControlsSecurity WHERE (UserTypeID = " & Me.lbUserTypes.SelectedItem.Value & ")")

            'TODO:  Evaluate after all pages are complete if this method is still required
            DeleteFromWebConfig(Me.lbUserTypes.SelectedItem.Value.ToString())

            'log event
            Master.tMsg("Delete", "User Type Deleted: " & Me.lbUserTypes.SelectedItem.Text)


        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Master.Msg = "Error: Unable to delete User Type. Please make sure that there are no users of this type."
        Finally
            LoadListboxes()
        End Try

    End Sub

    Private Sub cmdRename_Click(sender As Object, e As System.EventArgs) Handles cmdRename.Click
        Try
            Dim strRenameFrom As String = Me.lbUserTypes.SelectedItem.Text
            Dim strInput As String = txtRenameTo.Text

            'create new user type
            If (strInput.Length() > 0) Then
                DA.ExecSQL("UPDATE tblUserTypes SET Description = '" & strInput & "' WHERE (UserTypeID = " & Me.lbUserTypes.SelectedItem.Value & ")")
                'log event
                Master.tMsg("Rename", "User Type Renamed from: " & strRenameFrom & " to: " & strInput)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally
            LoadListboxes()
        End Try
    End Sub

#End Region

    Private Sub EnableControls()
        Try
            Master.Secure(cmdNew)
            Master.Secure(cmdDelete)
            Master.Secure(cmdRename)

            If (Me.lbUserTypes.SelectedIndex <= 0) Then
                cmdDelete.Enabled = False
                cmdRename.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub LoadListboxes()
        Try
            Dim dsUserTypes As DataSet = DA.GetDataSet_tblUserTypes()

            Me.lbUserTypes.DataSource = dsUserTypes
            Me.lbUserTypes.DataTextField = "Description"
            Me.lbUserTypes.DataValueField = "UserTypeID"
            Me.lbUserTypes.DataBind()

            Me.ddlUserTypes.DataSource = dsUserTypes
            Me.ddlUserTypes.DataTextField = "Description"
            Me.ddlUserTypes.DataValueField = "UserTypeID"
            Me.ddlUserTypes.DataBind()

            ' Add an option for 'None'
            Me.ddlUserTypes.Items.Insert(0, "None")
            Me.ddlUserTypes.Items(0).Value = ""

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ExecSQLInheritance(ByVal fromID As String, ByVal newID As String)
        DA.ExecSQL("INSERT INTO tblScreensControlsSecurity (ScreenID, ControlName, UserTypeID) SELECT ScreenID, ControlName, " & newID & " FROM tblScreensControlsSecurity WHERE UserTypeID = " & fromID)
    End Sub


    'TODO:  Evaluate after all pages are complete if this method is still required
    Private Sub DeleteFromWebConfig(ByVal id As String)
        'Dim xconfig As New XmlDocument

        'xconfig.Load(Request.PhysicalApplicationPath() & "Web.config")

        ''create backup
        'xconfig.Save(Request.PhysicalApplicationPath() & "_WebConfigBackup/Web.config.backup." & Date.Now.Month.ToString() & "_" & Date.Now.Day.ToString() & "_" & Date.Now.Year.ToString() & "_" & Date.Now.Hour.ToString() & "_" & Date.Now.Minute.ToString() & "_" & Date.Now.Second.ToString() & "_" & New System.Random().Next().ToString())

        ''Dim dsWebConfig As DataSet
        ''dsWebConfig

        'Dim allowUserList As String = ""
        ''Dim denyUserList As String = ""
        'Dim i As Integer = 0
        'Dim index As Integer = 0
        'Dim ixml As Integer = 0

        'For i = 0 To (xconfig.SelectNodes("configuration/location").Count - 1)
        '    'find node that contains 'fromID'
        '    allowUserList = xconfig.SelectNodes("configuration/location").Item(ixml).ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes(0).Value.ToString()
        '    index = allowUserList.IndexOf(id)
        '    If (index >= 0) Then
        '        'check position of id in the list
        '        If (allowUserList.IndexOf("," & id) >= 0) Then              'id is not at the beginning
        '            allowUserList = allowUserList.Replace("," & id, "")
        '            'save list
        '            xconfig.SelectNodes("configuration/location").Item(i).ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes(0).Value = allowUserList
        '        ElseIf (allowUserList.IndexOf((id & ",")) >= 0) Then                  'id is at the beginning
        '            allowUserList = allowUserList.Replace(id & ",", "")
        '            'save list
        '            xconfig.SelectNodes("configuration/location").Item(i).ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes(0).Value = allowUserList
        '        Else                  'id is the only entry in the list
        '            'allowUserList = allowUserList.Replace(id, "")					  
        '            xconfig.DocumentElement.RemoveChild(xconfig.SelectNodes("configuration/location").Item(i))
        '            ixml -= 1
        '        End If

        '    End If
        '    ixml += 1
        'Next

        ''save file
        'xconfig.Save(Request.PhysicalApplicationPath() & "Web.config")

    End Sub

    'TODO:  Evaluate after all pages are complete if this method is still required
    Private Sub ExecWebConfigInheritance(ByVal fromID As String, ByVal newID As String)
        'Dim xconfig As New XmlDocument

        'xconfig.Load(Request.PhysicalApplicationPath() & "Web.config")

        ''create backup
        'xconfig.Save(Request.PhysicalApplicationPath() & "_WebConfigBackup/Web.config.backup." & Date.Now.Month.ToString() & "_" & Date.Now.Day.ToString() & "_" & Date.Now.Year.ToString() & "_" & Date.Now.Hour.ToString() & "_" & Date.Now.Minute.ToString() & "_" & Date.Now.Second.ToString() & "_" & New System.Random().Next().ToString())

        ''Dim dsWebConfig As DataSet
        ''dsWebConfig

        'Dim allowUserList As String
        'Dim i As Integer

        'For i = 0 To (xconfig.SelectNodes("configuration/location").Count - 1)
        '    'find node that contains 'fromID'
        '    allowUserList = xconfig.SelectNodes("configuration/location").Item(i).ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes(0).Value.ToString()
        '    If (allowUserList.IndexOf(fromID) >= 0) Then
        '        allowUserList += ("," & newID)
        '        'modify list
        '        xconfig.SelectNodes("configuration/location").Item(i).ChildNodes(0).ChildNodes(0).ChildNodes(0).Attributes(0).Value = allowUserList
        '    End If
        'Next

        ''save file
        'xconfig.Save(Request.PhysicalApplicationPath() & "Web.config")
        ''log event
        ''tMsg("Apply Inheritance", "New User Type ID: " & newID & " was inherited from User Type ID: " & fromID)

    End Sub


End Class