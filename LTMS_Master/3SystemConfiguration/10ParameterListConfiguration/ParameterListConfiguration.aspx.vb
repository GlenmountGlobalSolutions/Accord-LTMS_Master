Imports System.Data.SqlClient

Public Class ParameterListConfiguration
    Inherits System.Web.UI.Page

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not Page.IsPostBack) Then
                lbProdParamList_Load()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Sub lbProdParamList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbProdParamList.SelectedIndexChanged
        Try
            lbProdParamListSelected()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbParamListValues_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbParamListValues.SelectedIndexChanged
        Try
            txtProdParamListVal.Text = lbParamListValues.SelectedItem.Text
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        SaveParameterList()
    End Sub

    Private Sub cmdCopy_Click(sender As Object, e As System.EventArgs) Handles cmdCopy.Click
        CopyParameterList()
    End Sub

    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        NewParameterList()
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        DeleteParamterList()
    End Sub


    Private Sub cmdSaveProdParamListValue_Click(sender As Object, e As System.EventArgs) Handles cmdSaveProdParamListValue.Click
        SaveProdParameterListValue()
    End Sub

    Private Sub cmdNewProdParamListValue_Click(sender As Object, e As System.EventArgs) Handles cmdNewProdParamListValue.Click
        NewProdParameterListValue()
    End Sub

    Private Sub cmdDeleteProdParamListValue_Click(sender As Object, e As System.EventArgs) Handles cmdDeleteProdParamListValue.Click
        DeleteParameterListValue()
    End Sub



    Private Sub ibUPProdParamList_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibUPProdParamList.Click
        MoveParameterListUp()
    End Sub

    Private Sub ibDNProdParamList_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibDNProdParamList.Click
        MoveParameterListDown()
    End Sub


    Private Sub ibUpProdParamListValues_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibUpProdParamListValues.Click
        MoveParameterListValueUp()
    End Sub

    Private Sub ibDNProdParamListValues_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibDNProdParamListValues.Click
        MoveParameterListValueDown()
    End Sub


#End Region

#Region "Methods"

    Private Sub EnableControls()
        Try
            With Master
                .Secure(cmdNew)
                .Secure(cmdSave)
                .Secure(cmdDelete)
                .Secure(cmdCopy)

                .Secure(ibUPProdParamList)
                .Secure(ibDNProdParamList)
                .Secure(ibUpProdParamListValues)
                .Secure(ibDNProdParamListValues)

                .Secure(cmdSaveProdParamListValue)
                .Secure(cmdDeleteProdParamListValue)
                .Secure(cmdNewProdParamListValue)
            End With

            If (lbProdParamList.SelectedItem Is Nothing) Then
                cmdSave.Enabled = False
                cmdDelete.Enabled = False
                cmdCopy.Enabled = False

                ibUPProdParamList.Enabled = False
                ibDNProdParamList.Enabled = False
                ibUpProdParamListValues.Enabled = False
                ibDNProdParamListValues.Enabled = False

                cmdSaveProdParamListValue.Enabled = False
                cmdDeleteProdParamListValue.Enabled = False
                cmdNewProdParamListValue.Enabled = False
            Else

                If (lbParamListValues.SelectedItem Is Nothing) Then
                    ibUpProdParamListValues.Enabled = False
                    ibDNProdParamListValues.Enabled = False
                    cmdSaveProdParamListValue.Enabled = False
                    cmdDeleteProdParamListValue.Enabled = False
                    Master.Secure(cmdNewProdParamListValue)
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub lbProdParamList_Load()
        Try

            Dim SelItem As ListItem = Nothing

            If (lbProdParamList.SelectedItem IsNot Nothing) Then
                SelItem = lbProdParamList.SelectedItem
            End If

            lbProdParamList.DataSource = DA.GetDataSet("Select ParameterListID, Description FROM tblParameterLists ORDER BY DisplayID")
            lbProdParamList.DataTextField = "Description"
            lbProdParamList.DataValueField = "ParameterListID"
            lbProdParamList.DataBind()

            If (SelItem IsNot Nothing) Then
                lbProdParamList.SelectedIndex = lbProdParamList.Items.IndexOf(lbProdParamList.Items.FindByValue(SelItem.Value))
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub lbParamListValues_Load()
        Try
            Dim SelItem As ListItem = Nothing
            If (lbParamListValues.SelectedItem IsNot Nothing) Then
                SelItem = lbParamListValues.SelectedItem
            End If

            With lbParamListValues
                .DataSource = DA.GetDataSet("SELECT ParameterListValue, DisplayID FROM tblParameterListValues " _
                                        & " WHERE (ParameterListID = '" & lbProdParamList.SelectedItem.Value & "') ORDER BY DisplayID")
                .DataTextField = "ParameterListValue"
                .DataValueField = "DisplayID"
                .DataBind()
            End With

            If (Not SelItem Is Nothing) Then
                lbParamListValues.SelectedIndex = lbParamListValues.Items.IndexOf(lbParamListValues.Items.FindByValue(SelItem.Value))
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbProdParamListSelected()
        Try
            txtProdParamListVal.Text = ""
            txtProdParamListDesc.Text = lbProdParamList.SelectedItem.Text
            lbParamListValues.Items.Clear()
            lbParamListValues_Load()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Move List Item Up / Down Methods"

    Private Sub MoveParameterListUp()
        Try
            If (lbProdParamList.SelectedIndex = 0) Then
                Master.eMsg("You can not move up.")
            Else

                'moving up by swapping diplayID

                DA.ExecSQL("Update  tblParameterLists Set DisplayID='" & lbProdParamList.SelectedIndex & "' Where ParameterListID = '" & lbProdParamList.SelectedItem.Value & "'")

                DA.ExecSQL("Update  tblParameterLists Set DisplayID='" & lbProdParamList.SelectedIndex + 1 & "' Where ParameterListID = '" & lbProdParamList.Items(lbProdParamList.SelectedIndex - 1).Value & "'")

                lbProdParamList_Load()
                lbParamListValues_Load()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub MoveParameterListDown()
        Try
            If (lbProdParamList.SelectedIndex = lbProdParamList.Items.Count - 1) Then
                Master.Alert = "You can not move down."
            Else

                'moving down by swapping diplayID
                DA.ExecSQL("Update  tblParameterLists Set DisplayID='" & lbProdParamList.SelectedIndex + 2 & "' Where ParameterListID = '" & lbProdParamList.SelectedItem.Value & "'")

                'moving up by swapping
                DA.ExecSQL("Update  tblParameterLists Set DisplayID='" & lbProdParamList.SelectedIndex + 1 & "' Where ParameterListID = '" & lbProdParamList.Items(lbProdParamList.SelectedIndex + 1).Value & "'")

                lbProdParamList_Load()
                lbParamListValues_Load()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub MoveParameterListValueUp()
        Dim SelItem As ListItem = Nothing
        Try
            'moving up with respect to grown but not number
            If (lbParamListValues.SelectedItem IsNot Nothing) Then
                If (lbParamListValues.SelectedIndex = 0) Then
                    Master.Alert = "You can not move up."
                Else
                    'moving down by swapping diplayID
                    DA.ExecSQL("Update  tblParameterListValues Set DisplayID='" & lbParamListValues.SelectedIndex & "' Where  (DisplayID = '" & lbParamListValues.SelectedItem.Value & "') AND (ParameterListValue = '" & lbParamListValues.SelectedItem.Text & "')")

                    'moving up by swapping
                    DA.ExecSQL("Update  tblParameterListValues Set DisplayID='" & lbParamListValues.SelectedIndex + 1 & "' Where  (DisplayID = '" & lbParamListValues.Items(lbParamListValues.SelectedIndex - 1).Value & "') AND (ParameterListValue = '" & lbParamListValues.Items(lbParamListValues.SelectedIndex - 1).Text & "')")

                    If (lbParamListValues.SelectedItem IsNot Nothing) Then
                        SelItem = lbParamListValues.SelectedItem
                    End If

                    lbParamListValues_Load()

                    If (SelItem IsNot Nothing) Then
                        lbParamListValues.SelectedIndex = lbParamListValues.Items.IndexOf(lbParamListValues.Items.FindByValue(CStr(CDbl(SelItem.Value) - 1)))
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub MoveParameterListValueDown()
        Dim SelItem As ListItem = Nothing
        Try
            'moving up with respect to grown but not number
            With lbParamListValues

                If (.SelectedItem IsNot Nothing) Then
                    If (.SelectedIndex = .Items.Count - 1) Then
                        Master.Alert = "You can not move down."
                    Else
                        'moving down by swapping diplayID
                        DA.ExecSQL("Update  tblParameterListValues Set DisplayID='" & .SelectedIndex + 2 & "' Where  (DisplayID = '" & .SelectedItem.Value & "') AND (ParameterListValue = '" & .SelectedItem.Text & "')")
                        'moving up by swapping
                        DA.ExecSQL("Update  tblParameterListValues Set DisplayID='" & .SelectedIndex + 1 & "' Where  (DisplayID = '" & .Items(.SelectedIndex + 1).Value & "') AND (ParameterListValue = '" & .Items(.SelectedIndex + 1).Text & "')")

                        If (Not .SelectedItem Is Nothing) Then
                            SelItem = .SelectedItem
                        End If

                        lbParamListValues_Load()

                        If (Not SelItem Is Nothing) Then
                            .SelectedIndex = .Items.IndexOf(.Items.FindByValue(CStr(CDbl(SelItem.Value) + 1)))
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Parameter List Methods"

    Private Sub SaveParameterList()
        Dim sqlstrUpdate As String
        Try
            If (lbProdParamList.SelectedItem Is Nothing) Then
                Master.Msg = "Please, select a Parameter list "
            Else
                If (txtProdParamListDesc.Text.Length = 0) Then
                    Master.Msg = "Please enter description"
                ElseIf (lbProdParamList.SelectedItem.Text = txtProdParamListDesc.Text) Then
                    Master.Msg = "Save Cancelled: changes have not been made."
                Else
                    sqlstrUpdate = "Update  tblParameterLists Set Description ='" & Me.txtProdParamListDesc.Text & "' where  ParameterListID = '" & lbProdParamList.SelectedItem.Value & "'"
                    DA.ExecSQL(sqlstrUpdate)
                    Master.tMsg("Save-Product Parameter", "Product parameter: " & lbProdParamList.SelectedItem.Text & " was renamed to: " & txtProdParamListDesc.Text)
                    lbProdParamList_Load()
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub CopyParameterList()
        Try
            Dim curParameterList As String = lbProdParamList.SelectedItem.Text
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As Data.SqlClient.SqlParameter
            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@ToBeCopiedParameterListID", SqlDbType.VarChar, 4)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = lbProdParamList.SelectedItem.Value
            colParms.Add(prmNext)
            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@Desc", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = txtList.Text
            colParms.Add(prmNext)
            '------------------------------------------------------------
            DA.ExecSP("procCopyWithNewParameterListID", colParms)
            lbProdParamList_Load()
            lbProdParamList.SelectedIndex = lbProdParamList.Items.IndexOf(lbProdParamList.Items.FindByText(txtList.Text))
            lbParamListValues.Items.Clear()
            lbParamListValues_Load()
            txtProdParamListDesc.Text = lbProdParamList.SelectedItem.Text

            Master.tMsg("Copy-Product Parameter", "Product parameter: " & curParameterList & " was copied with new ID: " & lbProdParamList.SelectedItem.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub NewParameterList()
        Try
            Dim strInput As String = txtDesc.Text

            If lbProdParamList.Items.FindByText(strInput) IsNot Nothing Then
                Master.Msg = "Error: Unable to create new Parameter. There is already a Plant Parameter List with the name " & strInput & "."
            Else
                If (strInput.Length > 0) Then
                    If Not (txtDesc.Text = "") Then
                        Dim colParms As New List(Of SqlParameter)
                        Dim prmNext As Data.SqlClient.SqlParameter
                        prmNext = New Data.SqlClient.SqlParameter("@NewProdParamDesc", SqlDbType.VarChar, 48)
                        prmNext.Direction = ParameterDirection.Input
                        prmNext.Value = txtDesc.Text
                        colParms.Add(prmNext)
                        DA.ExecSP("procAddNewProductPrameterListID", colParms)
                    End If

                    lbProdParamList_Load()

                End If
            End If

            lbProdParamList.SelectedIndex = lbProdParamList.Items.IndexOf(lbProdParamList.Items.FindByText(strInput))
            lbProdParamListSelected()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub DeleteParamterList()
        Try
            Dim z As Integer
            Dim h As Integer
            Dim str As String = ""

            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As Data.SqlClient.SqlParameter

            If lbProdParamList.SelectedItem Is Nothing Then
                Master.Alert = "Please, select product parameters list you would like to delete."
            Else
                For Each xListITem In lbProdParamList.Items
                    If (lbProdParamList.Items.Item(z).Selected = True) Then

                        prmNext = New Data.SqlClient.SqlParameter("@ParameterListID", SqlDbType.VarChar, 4)
                        prmNext.Direction = ParameterDirection.Input
                        prmNext.Value = lbProdParamList.Items.Item(z).Value
                        colParms.Add(prmNext)
                        DA.ExecSP("procDeleteProdParamList", colParms)
                        colParms.Clear()

                        Master.tMsg("Delete-Product Parameter", "Product parameter: " & lbProdParamList.Items.Item(z).Text & " is deleted")
                        If (str = "") Then
                            str = lbProdParamList.Items.Item(z).Text
                        Else
                            str = str & ", " & lbProdParamList.Items.Item(z).Text
                        End If
                        h = h + 1
                    End If
                    z = z + 1
                Next
                If (h = 1) Then
                    Master.Msg = "Product parameter list value " & str & " was deleted."
                Else
                    Master.Msg = "Product parameters list values: " & str & " were deleted."
                End If
                lbParamListValues.Items.Clear()
                lbProdParamList_Load()
                txtProdParamListDesc.Text = ""
                txtProdParamListVal.Text = ""
                If (h = 1) Then
                    Master.Msg = "Product parameter list value " & str & " was deleted."
                Else
                    Master.Msg = "Product parameters list values: " & str & " were deleted."
                End If
                lbParamListValues.Items.Clear()
                lbProdParamList_Load()
                txtProdParamListDesc.Text = ""
                txtProdParamListVal.Text = ""
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region

#Region "Parameter List Value Methods"

    Private Sub SaveProdParameterListValue()
        Try
            Dim sqlstrUpdate As String

            If (lbParamListValues.SelectedItem Is Nothing) Then
                Master.Msg = "Please, select a parameter list value"
            Else
                If (txtProdParamListVal.Text = "") Then
                    Master.Msg = "Please enter new value"
                ElseIf (lbParamListValues.SelectedItem.Text = txtProdParamListVal.Text) Then
                    Master.Msg = "Save Cancelled: changes have not been made."
                Else
                    sqlstrUpdate = "Update   tblParameterListValues Set  ParameterListValue ='" & Me.txtProdParamListVal.Text & "' where   ParameterListValue = '" & lbParamListValues.SelectedItem.Text & "' and DisplayID = '" & lbParamListValues.SelectedItem.Value & "'" & " and ParameterListID = '" & lbProdParamList.SelectedItem.Value & "'"
                    DA.ExecSQL(sqlstrUpdate)
                    Master.tMsg("Save-Product Parameter Value", "Product Parameter Value: " & lbParamListValues.SelectedItem.Text & " in product parameter: " & lbProdParamList.SelectedItem.Text & "was renamed to: " & txtProdParamListVal.Text)
                    lbParamListValues_Load()
                    Master.Msg = "Parameter type " & txtProdParamListVal.Text & " was saved."
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub NewProdParameterListValue()
        Try
            Dim strInput As String = txtVal.Text

            Try
                Dim colParms As New List(Of SqlParameter)
                Dim prmNext As Data.SqlClient.SqlParameter
                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ParameterListValue", SqlDbType.VarChar, 80)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = strInput
                colParms.Add(prmNext)
                '------------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ParameterListID", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = lbProdParamList.SelectedItem.Value
                colParms.Add(prmNext)
                '------------------------------------------------------------
                DA.ExecSP("procAddNewProductPrameterListValue", colParms)
            Catch ex As Exception

                If Err.Number = 5 Then
                    Master.Alert = "Error: Product parameter list value already exists."
                Else
                    Master.eMsg(ex.ToString())
                End If
            End Try

            Master.tMsg("cmdNewProdParamListValue_Click", "Parameter type " & strInput & "  was added.")
            lbParamListValues_Load()
            lbParamListValues.SelectedIndex = lbParamListValues.Items.IndexOf(lbParamListValues.Items.FindByText(strInput))
            txtProdParamListVal.Text = lbParamListValues.SelectedItem.Text


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub DeleteParameterListValue()
        Try
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As Data.SqlClient.SqlParameter

            Dim z As Integer
            Dim h As Integer
            Dim str As String = ""

            If lbParamListValues.SelectedItem Is Nothing Then
                Master.Alert = "Please, select product parameters list value you would like to delete."
            Else
                For Each xListITem In lbParamListValues.Items
                    If (lbParamListValues.Items.Item(z).Selected = True) Then

                        prmNext = New Data.SqlClient.SqlParameter("@ParameterListID", SqlDbType.VarChar, 4)
                        prmNext.Direction = ParameterDirection.Input
                        prmNext.Value = lbParamListValues.Items.Item(z).Text
                        colParms.Add(prmNext)
                        'IN--------------------------------------------------------
                        prmNext = New Data.SqlClient.SqlParameter("@ParamTypeListValue", SqlDbType.VarChar, 48)
                        prmNext.Direction = ParameterDirection.Input
                        prmNext.Value = lbParamListValues.Items.Item(z).Text
                        colParms.Add(prmNext)
                        '------------------------------------------------------------
                        DA.ExecSP("procDeleteProdParamListValue", colParms)
                        colParms.Clear()
                        If (str = "") Then
                            str = lbParamListValues.Items.Item(z).Text
                        Else
                            str = str & ", " & lbParamListValues.Items.Item(z).Text
                        End If
                        h = h + 1
                    End If
                    z = z + 1
                Next
                If (h = 1) Then
                    Master.Msg = "Product parameter list value " & str & " was deleted."
                Else
                    Master.Msg = "Product parameters list values: " & str & " were deleted."
                End If
                Master.tMsg("cmdDeleteProdParamListValue_Click", "Parameter list value " & str & "  was deleted.")

                lbParamListValues.ClearSelection()
                lbParamListValues.Items.Clear()
                lbParamListValues_Load()
                txtProdParamListVal.Text = ""
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region


End Class