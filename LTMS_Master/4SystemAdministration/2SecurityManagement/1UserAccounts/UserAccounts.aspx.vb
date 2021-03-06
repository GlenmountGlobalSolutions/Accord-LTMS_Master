Public Class UserAccounts
    Inherits System.Web.UI.Page

    Private ctrlEffected As New ArrayList

#Region "Web Events"

    Protected Sub UserAccounts_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not Page.IsPostBack) Then
                lbUsers_DataBind()
                ddlNewUserType_DataBind()
                ddlNewPLCUser_DataBind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub UserAccounts_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "User Events"

    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim strSQL As String
            Dim row As DataRow
            Dim intIdentity As Integer
            Dim bPLCUser As Boolean
            Dim strPLCUserBit As String

            ' convert boolean into a '1' or a '0' for the SQL statement...
            If (ddlNewPLCUser.SelectedItem.Value.ToUpper() = "TRUE") Then
                bPLCUser = True
            End If
            strPLCUserBit = CStr(IIf(bPLCUser, "1", "0"))

            strSQL = "BEGIN INSERT INTO tblUserAccounts (ModifiedDT, FirstName, LastName, LogInName, Password, BadgeID, LoginLevel, PLCUser, ChangedByUser, UserTypeID) " _
                        & "VALUES(GETDATE(),'" _
                                & txtNewFirstName.Text.Trim & "','" _
                                & txtNewLastName.Text.Trim & "','" _
                                & txtNewLogInName.Text & "','" _
                                & txtNewPassword.Text & "','" _
                                & txtNewBadgeID.Text.Trim & "','" _
                                & txtNewLoginLevel.Text.Trim & "'," _
                                & strPLCUserBit & ",'" _
                                & Session("UserFirstLastName").ToString() & "'," _
                                & ddlNewUserType.SelectedItem.Value & ") " _
                        & "SELECT @@IDENTITY END"

            row = DA.GetDataSet(strSQL).Tables(0).Rows(0)

            intIdentity = CInt(row(0))

            lbUsers_DataBind()
            lbUsers.SelectedIndex = lbUsers.Items.IndexOf(lbUsers.Items.FindByValue(intIdentity.ToString()))

            ' Update the Date/Time stamps in the Application Parameters table.
            UpdateTransactionParameters()

            LoadDataIntoCTRLS(False)
            ctrlEffected.Clear()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        Try
            imgLoginNameQuestion.Visible = False

            DA.ExecSQL("Delete from tblUserAccounts where UserID = " & lbUsers.SelectedItem.Value)
            Master.tMsg("Delete", "Userid: " & lbUsers.SelectedItem.Value & " UserName: " & lbUsers.SelectedItem.Text & " was deleted by: " & Session("UserFirstLastName").ToString())

            ' Update the Date/Time stamps in the Application Parameters table.
            UpdateTransactionParameters()

            lbUsers_DataBind()
            ctrlEffected.Clear()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally
            ClearControls()
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim strSQL As String
            Dim bPLCUser As Boolean
            Dim strPLCUserBit As String
            Dim bStationBypass As Boolean
            Dim strStationBypassBit As String

            If ValidateForSave() Then
                ' convert PLC User boolean into a '1' or a '0' for the SQL statement...
                If (ddlPLCUser.SelectedItem.Value.ToUpper() = "TRUE") Then
                    bPLCUser = True
                End If
                strPLCUserBit = CStr(IIf(bPLCUser, "1", "0"))

                ' convert Station Bypass boolean into a '1' or a '0' for the SQL statement...
                If (ddlStationByPassEnabled.SelectedItem.Value.ToUpper() = "TRUE") Then
                    bStationBypass = True
                End If
                strStationBypassBit = CStr(IIf(bStationBypass, "1", "0"))

                strSQL = "UPDATE tblUserAccounts " & _
                         "SET FirstName = '" & txtFName.Text.Trim & "', " & _
                             "LastName = '" & txtLName.Text.Trim & "', " & _
                             "LogInName = '" & txtLogInName.Text & "', " & _
                             "Password = '" & txtPassword.Text & "', " & _
                             "UserTypeID = " & ddlUserType.SelectedItem.Value & ", " & _
                             "StationBypassEnabled = " & strStationBypassBit & ", " & _
                             "BarcodeIdentifier = '" & txtBarCodeID.Text & "', " & _
                             "BadgeID = '" & txtBadgeID.Text.Trim & "', " & _
                             "LoginLevel = '" & txtLoginLevel.Text.Trim & "', " & _
                             "PLCUser = " & strPLCUserBit & ", " & _
                             "ModifiedDT = GETDATE(), " & _
                             "ChangedByUser = '" & Session("UserFirstLastName").ToString() & "' " & _
                         "WHERE UserID=" & lbUsers.SelectedItem.Value

                DA.ExecSQL(strSQL)

                ' Update the Date/Time stamps in the Application Parameters table.
                UpdateTransactionParameters()

                LoadDataIntoCTRLS(True)

                Dim li As New ListItem()
                li = lbUsers.SelectedItem

                lbUsers_DataBind()
                lbUsers.SelectedIndex = lbUsers.Items.IndexOf(lbUsers.Items.FindByValue(li.Value))

                Master.Msg = "User data is saved!"
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbUsers_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbUsers.SelectedIndexChanged
        Try
            LoadDataIntoCTRLS(False)

            imgLoginNameQuestion.Visible = False
            imgBadgeIDQuestion.Visible = False
            imgLoginLevelQuestion.Visible = False
            imgPasswordQuestion.Visible = False

            ctrlEffected.Clear()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtFName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFName.TextChanged
        Try
            ctrlEffected.Add(UserAccountType.FirstName)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtLName_TextChanged(sender As Object, e As System.EventArgs) Handles txtLName.TextChanged
        Try
            ctrlEffected.Add(UserAccountType.LastName)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtLogInName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLogInName.TextChanged
        Try
            ctrlEffected.Add(UserAccountType.LogInName)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtLoginLevel_TextChanged(sender As Object, e As System.EventArgs) Handles txtLoginLevel.TextChanged
        Try
            ctrlEffected.Add(UserAccountType.LoginLevel)
            
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtBadgeID_TextChanged(sender As Object, e As System.EventArgs) Handles txtBadgeID.TextChanged
        Try
            ctrlEffected.Add(UserAccountType.BadgeID)
            
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtBarCodeID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarCodeID.TextChanged
        Try
            ' remember this control so that we can put a checkmark above it...
            ctrlEffected.Add(UserAccountType.BarcodeIdentifier)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ddlUserType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlUserType.SelectedIndexChanged
        Try
            ctrlEffected.Add(UserAccountType.UserTypeID)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStationByPassEnabled_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlStationByPassEnabled.SelectedIndexChanged
        Try
            ' remember this control so that we can put a checkmark above it...
            ctrlEffected.Add(UserAccountType.StationBypassEnabled)
            
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlPLCUser_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlPLCUser.SelectedIndexChanged
        Try
            ' remember this control so that we can put a checkmark above it...
            ctrlEffected.Add(UserAccountType.PLCUser)
            
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Subs and Functions"

    Private Sub ddlUserType_DataBind(ByRef usertypeID As String)
        Try
            Dim li As New ListItem()

            ddlUserType.DataSource = DA.GetDataSet_tblUserTypes()
            ddlUserType.DataTextField = "Description"
            ddlUserType.DataValueField = "UserTypeID"
            ddlUserType.DataBind()
            If (usertypeID = "") Then
                li.Value = "-1"
                li.Text = "Please, select the privilage"
                ddlUserType.Items.Insert(0, li)
            Else
                ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue(usertypeID))
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlNewUserType_DataBind()
        Dim li As New ListItem()

        ddlNewUserType.DataSource = DA.GetDataSet_tblUserTypes
        ddlNewUserType.DataTextField = "Description"
        ddlNewUserType.DataValueField = "UserTypeID"
        ddlNewUserType.DataBind()
        li.Value = "-1"
        li.Text = "Please select user type"
        ddlNewUserType.Items.Insert(0, li)
    End Sub

    Private Sub ddlNewPLCUser_DataBind()
        Dim li As New ListItem()

        ddlNewPLCUser.Items.Add("True")
        ddlNewPLCUser.Items.Add("False")

        li.Value = "-1"
        li.Text = "Please select a value"
        ddlNewPLCUser.Items.Insert(0, li)
    End Sub

    Private Sub ClearControls()
        txtFName.Text = ""
        txtLName.Text = ""
        ddlUserType.Items.Clear()
        txtLogInName.Text = ""
        ModifiedDT.Text = ""
        txtPassword.Text = ""
        ChagedByUser.Text = ""
        ddlStationByPassEnabled.Items.Clear()
        ddlPLCUser.Items.Clear()
    End Sub

    Private Sub EnableControls()

        Master.Secure(cmdNew)
        Master.Secure(cmdSave)
        Master.Secure(cmdDelete)

        If (lbUsers.SelectedIndex = -1) Then
            cmdSave.Enabled = False
            cmdDelete.Enabled = False

            txtFName.Enabled = False
            txtLName.Enabled = False
            txtLogInName.Enabled = False
            txtLoginLevel.Enabled = False
            txtBadgeID.Enabled = False
            txtPassword.Enabled = False
            ddlUserType.Enabled = False
            ddlStationByPassEnabled.Enabled = False
            txtBarCodeID.Enabled = False
            ddlPLCUser.Enabled = False
        Else
            cmdSave.Enabled = cmdSave.Enabled And True
            cmdDelete.Enabled = cmdDelete.Enabled And False

            cmdSave.Enabled = True
            cmdDelete.Enabled = True
            txtFName.Enabled = True
            txtLName.Enabled = True
            txtLogInName.Enabled = True
            txtLoginLevel.Enabled = True
            txtBadgeID.Enabled = True
            txtPassword.Enabled = True
            ddlUserType.Enabled = True
            ddlStationByPassEnabled.Enabled = True
            txtBarCodeID.Enabled = True
            ddlPLCUser.Enabled = True
        End If

    End Sub

    Private Sub lbUsers_DataBind()
        lbUsers.DataSource = DA.GetDataSet("SELECT UserID,  LastName + ', ' + FirstName AS LFName FROM  tblUserAccounts ORDER BY LastName")
        lbUsers.DataTextField = "LFName"
        lbUsers.DataValueField = "UserID"
        lbUsers.DataBind()
    End Sub

    Private Sub LoadDataIntoCTRLS(ByRef x As Boolean)
        Try
            Dim row As DataRow
            Dim z As String

            If (lbUsers.SelectedItem IsNot Nothing) Then
                row = DA.GetDataSet_tblUserAccounts(lbUsers.SelectedItem.Value).Tables(0).Rows(0)

                txtFName.Text = row(UserAccountType.FirstName).ToString()
                txtLName.Text = row(UserAccountType.LastName).ToString()

                ' This code is different from the other boxes because of the wierd way that HTML handles
                '   a textbox with the hidden "Password" text in it.
                txtPassword.Text = row(UserAccountType.Password).ToString()
                txtPassword.Attributes.Add("value", txtPassword.Text)
                txtPassword.OldText = txtPassword.Text

                ddlUserType_DataBind(row(UserAccountType.UserTypeID).ToString())
                ddlNewUserType_DataBind()
                txtLogInName.Text = row(UserAccountType.LogInName).ToString()
                txtLoginLevel.Text = row(UserAccountType.LoginLevel).ToString()
                txtBadgeID.Text = row(UserAccountType.BadgeID).ToString()
                txtBarCodeID.Text = Trim(row(UserAccountType.BarcodeIdentifier).ToString())
                ModifiedDT.Text = row(UserAccountType.ModifiedDT).ToString()
                ChagedByUser.Text = row(UserAccountType.ChangedByUser).ToString()

                ' Store the database value in the .OldText field to be used later for validation.
                txtLogInName.OldText = txtLogInName.Text

                Try
                    ddlStationByPassEnabled.Items.Clear()
                    ddlStationByPassEnabled.Items.Add("True")
                    ddlStationByPassEnabled.Items.Add("False")

                    If (Convert.ToBoolean(row(UserAccountType.StationBypassEnabled))) Then
                        ddlStationByPassEnabled.SelectedIndex = 0
                    Else
                        ddlStationByPassEnabled.SelectedIndex = 1
                    End If
                Catch e As InvalidCastException
                    ' just consider a NULL to be false...
                    ddlStationByPassEnabled.SelectedIndex = 1
                End Try

                Try
                    ddlPLCUser.Items.Clear()
                    ddlPLCUser.Items.Add("True")
                    ddlPLCUser.Items.Add("False")

                    If (Convert.ToBoolean(row(UserAccountType.PLCUser))) Then
                        ddlPLCUser.SelectedIndex = 0
                    Else
                        ddlPLCUser.SelectedIndex = 1
                    End If
                Catch e As InvalidCastException
                    ' just consider a NULL to be false...
                    ddlPLCUser.SelectedIndex = 1
                End Try

                If (x = True) Then
                    For Each z In ctrlEffected
                        Select Case z
                            Case CStr(UserAccountType.FirstName)
                                imgFirstNameCheck.Visible = True

                            Case CStr(UserAccountType.LastName)
                                imgLastNameCheck.Visible = True

                            Case CStr(UserAccountType.LogInName)
                                imgLoginNameCheck.Visible = True

                            Case CStr(UserAccountType.LoginLevel)
                                imgLoginLevelCheck.Visible = True

                            Case CStr(UserAccountType.BadgeID)
                                imgBadgeIDCheck.Visible = True

                            Case CStr(UserAccountType.Password)
                                imgPasswordCheck.Visible = True

                            Case CStr(UserAccountType.UserTypeID)
                                imgUserTypeCheck.Visible = True

                            Case CStr(UserAccountType.StationBypassEnabled)
                                imgStationBypassCheck.Visible = True

                            Case CStr(UserAccountType.BarcodeIdentifier)
                                imgBarcodeIDCheck.Visible = True

                            Case CStr(UserAccountType.PLCUser)
                                imgPLCUserCheck.Visible = True
                        End Select
                    Next
                    ctrlEffected.Clear()
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function ValidateLogInName() As Boolean 'checking for duplicate
        Dim bResult As Boolean = False
        Dim table As DataTable
        Dim sql As String

        sql = "Select LogInName from tblUserAccounts where LogInName='" & txtLogInName.Text & "'"
        table = DA.GetDataSet(sql).Tables(0)
        If (table.Rows.Count > 0) Then
            bResult = False
        Else
            bResult = True
        End If

        Return bResult
    End Function

    Private Function ValidateForSave() As Boolean
        Dim bResult As Boolean = True

        If (txtBadgeID.Text.Trim() = "") Then
            imgBadgeIDQuestion.Visible = True
            Master.Msg = "Error: Login Level is a required field!"
            bResult = False
        Else
            imgBadgeIDQuestion.Visible = False
        End If

        If (txtLoginLevel.Text.Trim() = "") Then
            imgLoginLevelQuestion.Visible = True
            Master.Msg = "Error: Login Level is a required field!"
            bResult = False
        Else
            imgLoginLevelQuestion.Visible = False
        End If

        ' This is being done here instead of in the TextChanged event because of the wierd way that
        '   HTML handles the textbox with the hidden "Password" text in it.
        If txtPassword.OldText <> txtPassword.Text Then
            ctrlEffected.Add(UserAccountType.Password)
        End If

        If (txtPassword.Text.Length < 3) Then
            imgPasswordQuestion.Visible = True
            Master.Msg = "Error: Password must be at least 3 characters long!"
            bResult = False
        Else
            imgPasswordQuestion.Visible = False
        End If

        If (txtLogInName.Text.Trim() = "") Then
            imgLoginNameQuestion.Visible = True
            Master.Msg = "Error: Login Name is required field!"
            bResult = False
        Else
            If (txtLogInName.OldText <> txtLogInName.Text) And (ValidateLogInName() = False) Then
                Master.Msg = "Error: Login name: " & txtLogInName.Text & " already exists!"
                imgLoginNameQuestion.Visible = True
                bResult = False
            Else
                imgLoginNameQuestion.Visible = False
            End If
        End If

        If (txtFName.Text.Trim() & txtLName.Text.Trim() = "") Then
            Master.Msg = "Error:  Either First or Last Name is required!"
            bResult = False
        End If

        Return bResult
    End Function

    Private Sub UpdateTransactionParameters()
        Try
            BizLayer.SetRecipeSavedDT("4", "0", "%")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region

End Class