Imports System.Data.SqlClient

Public Class ParameterConfiguration
    Inherits System.Web.UI.Page


#Region "Globals"

    Protected strParamDes As String 'after data bind data is loaded into variable and bound to the control
    Protected strDefVal As String

    Protected sqlUpdateParams As New ArrayList    'holds the update commands for each control that had data changed inside
    Protected Description As New ArrayList
    Protected NewValue As New ArrayList
    Protected OriginalValue As New ArrayList

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not Page.IsPostBack) Then
                ClearEditableControls()
                ddlProductType_DataBind()
                lbProdParams_Bind()

                ddCharSwap.Items.Add("True")
                ddCharSwap.Items.Add("False")
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

    Private Sub ddlProductType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlProductType.SelectedIndexChanged
        Try
            ClearEditableControls()
            lbProdParams_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbProdParams_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbProdParams.SelectedIndexChanged
        Try
            BindEditableControls(lbProdParams.SelectedItem.Value)

            tbParamDesc.Text = strParamDes
            tbParamDesc.OldText = strParamDes
            ParameterDescription(tbParamDesc)

            tbDefValue.Text = strDefVal
            tbDefValue.OldText = strDefVal
            DefaultValue(tbDefValue)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tbDefValue_TextChanged(sender As Object, e As System.EventArgs) Handles tbDefValue.TextChanged
        Try
            Dim x As String
            Dim wibSender As GGS.WebInputBox = CType(sender, GGS.WebInputBox)

            x = wibSender.SQLText
            If (Not x Is Nothing) Then
                x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", wibSender.Text)
                sqlUpdateParams.Add(x)
            End If

            Description.Add("Default Value")

            NewValue.Add(wibSender.Text)
            OriginalValue.Add(wibSender.OldText)
            wibSender.OldText = wibSender.Text

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tbParamDesc_TextChanged(sender As Object, e As System.EventArgs) Handles tbParamDesc.TextChanged
        Try
            Dim x As String
            Dim wibSender As GGS.WebInputBox = CType(sender, GGS.WebInputBox)

            x = wibSender.SQLText
            If (Not x Is Nothing) Then
                x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", wibSender.Text)
                sqlUpdateParams.Add(x)
            End If

            Description.Add("Description")
            NewValue.Add(wibSender.Text)
            OriginalValue.Add(wibSender.OldText)
            wibSender.OldText = wibSender.Text

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dbSelectionList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dbSelectionList.SelectedIndexChanged
        Try
            Dim SQLUpdateCommandText As String
            Dim wddl As GGS.WebDropDownList = CType(sender, GGS.WebDropDownList)

            If (wddl.SelectedItem.Value = "None") Then
                'if usere selected none---none comes not from db but programmibly
                SQLUpdateCommandText = "Update tblProductParameterTypes Set ParameterListID = Null Where ( ProductParameterTypeID = '" & lbProdParams.SelectedItem.Value & "' ) AND ComponentID = '" & ddlProductType.SelectedItem.Value & "'"

            Else
                SQLUpdateCommandText = "Update tblProductParameterTypes Set ParameterListID = '" & wddl.SelectedItem.Value & "' Where ( ProductParameterTypeID = '" & lbProdParams.SelectedItem.Value & "' ) AND ComponentID = '" & ddlProductType.SelectedItem.Value & "'"
            End If
            sqlUpdateParams.Add(SQLUpdateCommandText)
            Description.Add("Selection List")
            NewValue.Add(wddl.SelectedItem.Text)

            OriginalValue.Add(dbSelectionList.Items(0).Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddCharSwap_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddCharSwap.SelectedIndexChanged
        Try
            Dim TheNewValue As Boolean = (ddCharSwap.SelectedIndex() = 0)
            Dim TheProductParamID As String = lbProdParams.SelectedItem.Value()

            Dim SQLUpdateCommandText As String = _
                String.Format("UPDATE tblProductParameterTypes Set EnableCharacterSwap = {0} Where (ProductParameterTypeID = '{1}' AND ComponentID = '" & ddlProductType.SelectedItem.Value & "' )", _
                IIf(TheNewValue, "1", "0"), _
                TheProductParamID)

            ' remember to issue the above UPDATE statement when they click save...
            sqlUpdateParams.Add(SQLUpdateCommandText)

            ' log some values for debugging...    
            Description.Add("CharSwap")
            NewValue.Add(ddCharSwap.SelectedItem.Text())
            OriginalValue.Add(ddCharSwap.OldValue())

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As SqlParameter

            If (lbProdParams.SelectedItem Is Nothing) Then
                Master.Msg = "Please, select the Parameter you want to delete!"
            Else
                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ProdParamTypeID", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = lbProdParams.SelectedItem.Value
                colParms.Add(prmNext)
                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ProductType", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = ddlProductType.SelectedValue
                colParms.Add(prmNext)
                '------------------------------------------------------------
                DA.ExecSP("procDeleteProdParamType", colParms)

                Master.tMsg("Delete", "Product parameter: " & lbProdParams.SelectedItem.Text & " was deleted")

                ClearEditableControls()
                lbProdParams.ClearSelection()
                lbProdParams_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As SqlParameter

            If (Trim(txtNewParamDesc.Text) = "") Then
                Master.Alert = "Please enter parameter!"
            Else
                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ProdParamDesc", SqlDbType.VarChar, 48)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = txtNewParamDesc.Text.Trim()
                colParms.Add(prmNext)
                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@ProductType", SqlDbType.VarChar, 4)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = ddlProductType.SelectedValue
                colParms.Add(prmNext)
                '------------------------------------------------------------
                DA.ExecSP("procInsertNewProdParamType", colParms)

                lbProdParams.ClearSelection()
                lbProdParams_Bind()
                lbProdParams.SelectedIndex = lbProdParams.Items.IndexOf(lbProdParams.Items.FindByText(txtNewParamDesc.Text))
                BindEditableControls(lbProdParams.SelectedItem.Value)

                Master.tMsg("New", "Product parameter: " & lbProdParams.SelectedItem.Text & " was added to the list")

                tbParamDesc.Text = strParamDes
                ParameterDescription(tbParamDesc)
                tbDefValue.Text = strDefVal
                DefaultValue(tbDefValue)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim i As Int32
            Dim s As String

            If (Me.lbProdParams.SelectedIndex < 0) Then
                Master.Alert = "Please select a parameter type!"
            Else
                If sqlUpdateParams.Count = 0 Then
                    Master.Alert = "No parameter type was modified, save aborted!"
                Else
                    For Each s In sqlUpdateParams
                        DA.ExecSQL(s)
                        Master.tMsg("Save", Description(i).ToString() & " for product parameter: " & lbProdParams.SelectedItem.Text & " is chaged from: " & OriginalValue(i).ToString() & " to: " & NewValue(i).ToString())

                        i = i + 1
                    Next

                    lbProdParams_Bind()
                    BindEditableControls(lbProdParams.SelectedItem.Value)
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibDown_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibDown.Click
        Try
            If (Me.lbProdParams.SelectedIndex < 0) Then
                Master.Alert = "Cannot move item down: Parameter has not been selected"
            Else
                If (Me.lbProdParams.SelectedIndex < (Me.lbProdParams.Items.Count - 1)) Then
                    Dim dsDisplayID As DataSet
                    Dim selectedID As String = Me.lbProdParams.SelectedItem.Value
                    Dim selectedIndex As Integer = Me.lbProdParams.SelectedIndex

                    'get displayID of currently selected item
                    Dim dispID As Integer = 0
                    dsDisplayID = DA.GetDataSet("SELECT TOP 1 DisplayID FROM tblProductParameterTypes WHERE ProductParameterTypeID='" & selectedID & "'")
                    dispID = CInt(dsDisplayID.Tables(0).DefaultView.Table.Rows(0)(0))

                    'assign new displayID to item directly below currently selected item
                    DA.ExecSQL("UPDATE tblProductParameterTypes SET DisplayID = " & dispID & " WHERE ProductParameterTypeID='" & Me.lbProdParams.Items(selectedIndex + 1).Value & "' AND ComponentID = '" & ddlProductType.SelectedItem.Value & "'")

                    'assign new displayID to currently selected item
                    DA.ExecSQL("UPDATE tblProductParameterTypes SET DisplayID = " & (dispID + 1) & " WHERE ProductParameterTypeID='" & selectedID & "' AND ComponentID = '" & ddlProductType.SelectedItem.Value & "'")

                    'reload listbox
                    lbProdParams_Bind()

                    'select item
                    Me.lbProdParams.SelectedIndex = (selectedIndex + 1)

                Else 'item selected is already at the bottom of the list
                    Master.Alert = "Cannot move item down, it is already at the bottom of the list!"
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ibUP_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibUP.Click
        Try
            If (Me.lbProdParams.SelectedIndex < 0) Then
                Master.Alert = "Cannot move item up: Station has not been selected"
            Else
                If (Me.lbProdParams.SelectedIndex > 0) Then
                    Dim dsDisplayID As DataSet
                    Dim selectedID As String = Me.lbProdParams.SelectedItem.Value
                    Dim selectedIndex As Integer = Me.lbProdParams.SelectedIndex

                    'get displayID of currently selected item
                    Dim dispID As Integer = 0
                    dsDisplayID = DA.GetDataSet("SELECT TOP 1 DisplayID FROM tblProductParameterTypes WHERE ProductParameterTypeID='" & selectedID & "'")
                    dispID = Convert.ToInt32(dsDisplayID.Tables(0).DefaultView.Table.Rows(0)(0))

                    'assign new displayID to item directly above currently selected item
                    DA.ExecSQL("UPDATE tblProductParameterTypes SET DisplayID = " & dispID & " WHERE ProductParameterTypeID='" & Me.lbProdParams.Items(selectedIndex - 1).Value & "'")

                    'assign new displayID to currently selected item
                    DA.ExecSQL("UPDATE tblProductParameterTypes SET DisplayID = " & (dispID - 1) & " WHERE ProductParameterTypeID='" & selectedID & "'")

                    'reload listbox
                    lbProdParams_Bind()

                    'select item
                    Me.lbProdParams.SelectedIndex = (selectedIndex - 1)

                Else             'item selected is already at the top of the list
                    Master.Alert = "Cannot move item up, it is already at the top of the list!"
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        'pb, 1/25/04
        Master.Secure(cmdNew)
        Master.Secure(cmdSave)
        Master.Secure(cmdDelete)
        Master.Secure(ibDown)
        Master.Secure(ibUP)

        If (lbProdParams.SelectedItem Is Nothing) Then
            cmdSave.Enabled = False
            cmdDelete.Enabled = False
            cmdSave.Enabled = False
            ibUP.Enabled = False
            ibDown.Enabled = False
        End If

        If (Me.ddlProductType.SelectedIndex < 1) Then
            Me.cmdNew.Enabled = False
        End If
    End Sub

    Private Sub ddlProductType_DataBind()
        Dim strsql As String
        strsql = "SELECT ComponentID, Description FROM dbo.tblComponentID"
        ddlProductType.DataSource = DA.GetDataSet(strsql)
        ddlProductType.DataTextField = "Description"
        ddlProductType.DataValueField = "ComponentID"
        ddlProductType.DataBind()
        ddlProductType.Items.Insert(0, "Choose a Type")
    End Sub

    Private Sub lbProdParams_Bind()
        Dim SelItem As ListItem = Nothing

        If ddlProductType.SelectedIndex < 1 Then
            lbProdParams.Items.Clear()
        Else
            If (Not lbProdParams.SelectedItem Is Nothing) Then
                SelItem = lbProdParams.SelectedItem
            End If

            Dim sql As String = "SELECT ProductParameterTypeID, [Description] " & _
                                "FROM dbo.tblProductParameterTypes " & _
                                "WHERE ComponentID = '" & ddlProductType.SelectedItem.Value & "' " & _
                                "ORDER BY DisplayID"

            lbProdParams.DataSource = DA.GetDataSet(sql)
            lbProdParams.DataTextField = "Description"
            lbProdParams.DataValueField = "ProductParameterTypeID"
            lbProdParams.DataBind()

            If (Not SelItem Is Nothing) Then
                lbProdParams.SelectedIndex = lbProdParams.Items.IndexOf(lbProdParams.Items.FindByValue(SelItem.Value))
            End If
        End If
    End Sub

    Public Function ParameterDescription(ByVal x As Object) As String
        Dim SQLUpdateCommandText As String
        Dim tib As GGS.WebInputBox = CType(x, GGS.WebInputBox)

        SQLUpdateCommandText = "Update tblProductParameterTypes Set Description = '6933A6C7-80FC-4b37-907E-26FCE24DD7EE' Where ( ProductParameterTypeID = '" & lbProdParams.SelectedItem.Value & "'  AND ComponentID = '" & ddlProductType.SelectedItem.Value & "')"

        tib.SQLText = SQLUpdateCommandText

        ParameterDescription = strParamDes
    End Function

    Public Function DefaultValue(ByVal x As Object) As String
        Dim SQLUpdateCommandText As String
        Dim tib As GGS.WebInputBox = CType(x, GGS.WebInputBox)

        SQLUpdateCommandText = "Update tblProductParameterTypes Set  DefaultValue = '6933A6C7-80FC-4b37-907E-26FCE24DD7EE' Where ( ProductParameterTypeID = '" & lbProdParams.SelectedItem.Value & "'  AND ComponentID = '" & ddlProductType.SelectedItem.Value & "')"

        tib.SQLText = SQLUpdateCommandText

        DefaultValue = strDefVal
    End Function

    Private Sub ClearEditableControls()
        ' Clear the Array Lists of any old data.
        sqlUpdateParams.Clear()
        Description.Clear()
        NewValue.Clear()
        OriginalValue.Clear()

        ' Clear the items on the screen.
        tbParamDesc.Text = ""
        tbParamDesc.OldText = ""
        tbParamDesc.Enabled = False

        tbDefValue.Text = ""
        tbDefValue.OldText = ""
        tbDefValue.Enabled = False

        dbSelectionList.Items.Clear()
        dbSelectionList.SelectedIndex = -1
        dbSelectionList.Enabled = False

        ddProductTYpe.Items.Clear()

        ddCharSwap.SelectedIndex = -1
        ddCharSwap.Enabled = False
    End Sub

    Private Sub BindEditableControls(ByVal paramID As String)
        Dim strProdParamListID As String
        Dim dsetProdParamType As New DataSet
        Dim sql As String = "SELECT TOP 1 *  FROM tblProductParameterTypes WHERE (ProductParameterTypeID = '" & paramID & "') AND (ComponentID = '" & Me.ddlProductType.SelectedValue & "')"

        ClearEditableControls()

        tbParamDesc.Enabled = True
        tbDefValue.Enabled = True
        dbSelectionList.Enabled = True
        ddCharSwap.Enabled = True

        dsetProdParamType = DA.GetDataSet(sql)
        If (DA.IsDSEmpty(dsetProdParamType)) Then
            strParamDes = ""
            strDefVal = ""
            strProdParamListID = ""
        Else
            strParamDes = dsetProdParamType.Tables(0).DefaultView.Table.Rows(0)("Description").ToString()
            tbParamDesc.Text = strParamDes
            tbParamDesc.OldText = strParamDes

            strDefVal = dsetProdParamType.Tables(0).DefaultView.Table.Rows(0)("DefaultValue").ToString()
            tbDefValue.Text = strDefVal
            tbDefValue.OldText = strDefVal

            strProdParamListID = dsetProdParamType.Tables(0).DefaultView.Table.Rows(0)("ParameterListID").ToString()

            ddProductType_DataBind(dsetProdParamType.Tables(0).DefaultView.Table.Rows(0)("ComponentID").ToString())
        End If

        If (strProdParamListID Is Nothing Or strProdParamListID = "") Then
            sql = "SELECT DISTINCT ParameterListID, Description, DisplayID FROM tblParameterLists ORDER BY DisplayID"
            dbSelectionList.DataSource = DA.GetDataSet(sql)
            dbSelectionList.DataTextField = "Description"
            dbSelectionList.DataValueField = "ParameterListID"
            dbSelectionList.DataBind()
            dbSelectionList.Items.Insert(0, "None")
        Else
            sql = "SELECT '1', ParameterListID, Description " & _
                  "FROM dbo.tblParameterLists " & _
                  "WHERE ParameterListID = " & strProdParamListID & " " & _
                  "UNION " & _
                  "SELECT '2' , ParameterListID, Description " & _
                  "FROM dbo.tblParameterLists " & _
                  "WHERE (ParameterListID !=" & strProdParamListID & ")"

            dbSelectionList.DataSource = DA.GetDataSet(sql)
            dbSelectionList.DataTextField = "Description"
            dbSelectionList.DataValueField = "ParameterListID"
            dbSelectionList.DataBind()
            dbSelectionList.Items.Insert(1, "None")
        End If

        ' grab the value for EnableCharacterSwap for this record...

        Dim TheSQLStr As String = "SELECT EnableCharacterSwap " & _
                                  "FROM dbo.tblProductParameterTypes " & _
                                  "WHERE ProductParameterTypeID = " & paramID
        Dim TheDataSet As DataSet = DA.GetDataSet(TheSQLStr)
        Dim TheDBValue As Object = TheDataSet.Tables(0).Rows(0)(0)
        Dim ShouldSwap As Boolean = False

        If (Not IsDBNull(TheDBValue)) Then
            ShouldSwap = CType(TheDBValue, Boolean)
        End If

        ' set the listbox to either the first choice ("TRUE") or the second ("FALSE") based on the value from the table...
        ddCharSwap.SelectedIndex = CInt(IIf(ShouldSwap, 0, 1))
        ddCharSwap.OldValue = CStr(IIf(ShouldSwap, "True", "False"))
    End Sub

    Private Sub ddProductType_DataBind(ByRef cmpID As String)
        Dim strsql As String
        strsql = "SELECT ComponentID, Description FROM tblComponentID"
        ddProductTYpe.DataSource = DA.GetDataSet(strsql)
        ddProductTYpe.DataTextField = "Description"
        ddProductTYpe.DataValueField = "ComponentID"
        ddProductTYpe.DataBind()
        Dim ListItem As New ListItem
        ListItem.Text = ddProductTYpe.Items.FindByValue(cmpID).Text
        ddProductTYpe.Items.Insert(0, ListItem)
    End Sub

#End Region

End Class