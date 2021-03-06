Public Class ProductCodeConfiguration
    Inherits System.Web.UI.Page

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                ddlLineNum_LoadList()

                ddlStyleGroups.Enabled = False
                ddlComponentCodes.Enabled = False
                lbProducts.Enabled = False
                lbComponentNames.Enabled = False

                ' Default the checkbox to unchecked.
                cbAllProducts.Checked = False

                hidDirtyBit.Value = "false"
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

    Private Sub ddlLineNum_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLineNum.SelectedIndexChanged
        Try
            LoadProductCodeConfigurationControls(True)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStyleGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStyleGroups.SelectedIndexChanged
        Try
            LoadProductCodeConfigurationIntoWorkingTable()
            LoadProductCodeConfigurationControls(False)
            lbAddProduct_LoadList()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlComponentCodes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlComponentCodes.SelectedIndexChanged
        Try
            UpdateComponentCodes()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbProducts_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbProducts.SelectedIndexChanged
        Try
            LoadMasksAndSelectComponentCode()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub lbComponentNames_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbComponentNames.SelectedIndexChanged
        Try
            LoadMasksAndSelectComponentCode()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdAddProduct_Click(sender As Object, e As System.EventArgs) Handles cmdAddProduct.Click
        Try
            AddProductToListBox()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRemoveProduct_Click(sender As Object, e As System.EventArgs) Handles cmdRemoveProduct.Click
        Try
            RemoveProduct()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            SaveConfiguration()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As System.EventArgs) Handles cmdCancel.Click
        Try
            LoadProductCodeConfigurationIntoWorkingTable()
            LoadProductCodeConfigurationControls(False)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cbAllProducts_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbAllProducts.CheckedChanged
        Try
            ValidateIfCheckBoxCanBeChecked()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        Try
            Master.Secure(cmdSave)
            Master.Secure(cmdCancel)
            Master.Secure(cmdAddProduct)
            Master.Secure(cmdRemoveProduct)

            If hidDirtyBit.Value = "false" Then
                cmdSave.Enabled = False
                cmdCancel.Enabled = False
            End If

            If (ddlLineNum.SelectedIndex <= 0) Or (ddlStyleGroups.SelectedIndex <= 0) Then
                cmdAddProduct.Enabled = False
                cmdRemoveProduct.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub lbProducts_LoadList()
        Try
            With lbProducts
                .Items.Clear()
                If (ddlStyleGroups.SelectedIndex > 0) Then
                    .DataSource = DA.GetDataSet("SELECT Distinct ProductID, ProductID AS ProductDescription " & _
                                                " FROM dbo.tblSGProductCodeConfigWorking WHERE [LogInName] = '" & Page.User.Identity.Name.ToString() & _
                                                "' AND [StyleGroupID] = " & ddlStyleGroups.SelectedValue)
                    .DataTextField = "ProductDescription"
                    .DataValueField = "ProductID"
                    .DataBind()
                    .Enabled = True
                End If
            End With
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbComponentNames_LoadList()
        Try
            With lbComponentNames
                .Items.Clear()
                If (ddlLineNum.SelectedIndex > 0) Then
                    .DataSource = DA.GetDataSet("SELECT PLCArrayOrder, ComponentName FROM dbo.tblSGComponentNames WHERE LineID = " & ddlLineNum.SelectedValue & " ORDER BY PLCArrayOrder")
                    .DataTextField = "ComponentName"
                    .DataValueField = "PLCArrayOrder"
                    .DataBind()
                    .Enabled = True
                End If
            End With
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbAddProduct_LoadList()
        Try
            If (ddlStyleGroups.SelectedIndex > 0) Then
                Dim sql As String = "SELECT [ProductID],[ProductID] AS ProductDescription " & _
                                    "FROM dbo.tblProducts " & _
                                    "WHERE [ProductID] NOT IN (SELECT DISTINCT [ProductID] " & _
                                                            "FROM dbo.tblSGProductCodeConfig) " & _
                                    "AND [ProductID] NOT IN (SELECT DISTINCT [ProductID] " & _
                                                            "FROM dbo.tblSGProductCodeConfigWorking " & _
                                                            "WHERE [LogInName] = '" & Page.User.Identity.Name.ToString() & "' " & _
                                                            "AND [StyleGroupID] = " & ddlStyleGroups.SelectedValue & ") " & _
                                    "ORDER BY [ProductID]"
                With lbAddProduct
                    .DataSource = DA.GetDataSet(sql)
                    .DataTextField = "ProductDescription"
                    .DataValueField = "ProductID"
                    .DataBind()
                End With
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLineNum_LoadList()
        Try
            With ddlLineNum
                .DataSource = DA.GetDataSet("SELECT LineID, LineName FROM dbo.tblSGLines")
                .DataTextField = "LineName"
                .DataValueField = "LineID"
                .DataBind()
                .Items.Insert(0, "Choose a Line")
                .SelectedIndex = 0
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub ddlStyleGroupsList_LoadList()
        Try
            With ddlStyleGroups
                .Items.Clear()

                If (ddlLineNum.SelectedIndex > 0) Then
                    .DataSource = DA.GetDataSet("SELECT StyleGroupName, StyleGroupID FROM dbo.tblSGStyleGroups WHERE LineID = " & ddlLineNum.SelectedValue & " ORDER BY PLCArrayOrder")
                    .DataTextField = "StyleGroupName"
                    .DataValueField = "StyleGroupID"
                    .DataBind()
                    .Items.Insert(0, "Choose a Style Group")
                    .SelectedIndex = 0
                    .Enabled = True
                End If
            End With
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlComponentCodes_LoadList()
        Try
            With ddlComponentCodes
                .DataSource = DA.GetDataSet("SELECT ComponentCodeID, ComponentCode FROM dbo.tblSGComponentCodes WHERE ComponentCode <> '' ORDER BY ComponentCode ASC")
                .DataTextField = "ComponentCode"
                .DataValueField = "ComponentCodeID"
                .DataBind()
                .Items.Insert(0, "NONE")
                .Enabled = True
            End With
            ddlComponentCodes_SetSelectedIndex()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlComponentCodes_SetSelectedIndex()
        Dim strComponentCode As String = ""
        Try
            If (ddlLineNum.SelectedIndex <= 0) Or (ddlStyleGroups.SelectedIndex <= 0) Or (lbComponentNames.SelectedIndex < 0) Or (lbProducts.SelectedIndex < 0) Then
                ddlComponentCodes.SelectedIndex = -1
            Else
                ' Set the Component Code drop down index to the value in the working table for the 
                '   currently selected StyleGroup/Product/Component Name. If there isn't one or it
                '   is empty, then set the index to zero.
                Dim SQL As String = "SELECT w.ComponentCode " & _
                                                    "FROM dbo.tblSGProductCodeConfigWorking w " & _
                                                    "WHERE w.LogInName = '" & Page.User.Identity.Name.ToString() & "' " & _
                                                    "AND w.StyleGroupID = " & ddlStyleGroups.SelectedValue & " " & _
                                                    "AND w.ProductID = '" & lbProducts.SelectedValue & "' " & _
                                                    "AND w.ComponentNameIDX = " & lbComponentNames.SelectedValue & " "

                Using ds As DataSet = DA.GetDataSet(SQL)
                    ddlComponentCodes.SelectedIndex = 0

                    If (ds.Tables.Count = 1) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                        strComponentCode = ds.Tables(0).Rows(0).Item("ComponentCode").ToString()

                        If (strComponentCode.Length > 0) Then
                            ddlComponentCodes.SelectedIndex = ddlComponentCodes.Items.IndexOf(ddlComponentCodes.Items.FindByText(strComponentCode))
                        End If
                    End If
                End Using

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadProductCodeConfigurationControls(ByRef loadStyleGroups As Boolean)
        Try
            If (loadStyleGroups) Then
                ddlStyleGroupsList_LoadList()
            End If

            ddlComponentCodes_LoadList()
            lbProducts_LoadList()
            lbComponentNames_LoadList()

            LoadMasksAndSelectComponentCode()

            hidDirtyBit.Value = "false"

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadProductCodeConfigurationIntoWorkingTable()
        Try
            If (ddlStyleGroups.SelectedIndex > 0) And (Page.User.Identity.IsAuthenticated) Then
                DA.ExecSQL("EXECUTE dbo.procSGProductCodeConfiguration_GetData @StyleGroupID = " & ddlStyleGroups.SelectedValue & ", @LogInName = '" & Page.User.Identity.Name.ToString() & "'")
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub LoadMasksAndSelectComponentCode()
        Dim strComponentCode As String = ""
        Dim SQL As String

        Try
            If (ddlLineNum.SelectedIndex <= 0) Or
                (ddlStyleGroups.SelectedIndex <= 0) Or
                (lbComponentNames.SelectedIndex < 0) Or
                (lbProducts.SelectedIndex < 0) Then

                txtMask1Start.Text = ""
                txtMask1Length.Text = ""
                txtMask2Start.Text = ""
                txtMask2Length.Text = ""
                txtFails.Text = ""

                ddlComponentCodes.SelectedIndex = -1
            Else
                SQL = "SELECT Mask1Start, Mask1Length, Mask2Start, Mask2Length, Fails " & _
                        "FROM dbo.tblSGComponentNames " & _
                        "WHERE PLCArrayOrder = " & lbComponentNames.SelectedItem.Value & " " & _
                        "AND LineID = " & ddlLineNum.SelectedValue

                Using ds As DataSet = DA.GetDataSet(SQL)
                    If (ds.Tables.Count = 1) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                        With ds.Tables(0).Rows(0)
                            txtMask1Start.Text = .Item("Mask1Start").ToString()
                            txtMask1Length.Text = .Item("Mask1Length").ToString()
                            txtMask2Start.Text = .Item("Mask2Start").ToString()
                            txtMask2Length.Text = .Item("Mask2Length").ToString()
                            txtFails.Text = .Item("Fails").ToString()
                        End With
                    End If
                End Using


                ' Set the Component Code drop down index to the value in the working table for the 
                '   currently selected StyleGroup/Product/Component Name. If there isn't one or it
                '   is empty, then set the index to zero.
                ddlComponentCodes_SetSelectedIndex()

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub UpdateComponentCodes()
        Try
            Dim SQL As String = ""
            Dim componentCode As String = ""

            If (ddlLineNum.SelectedIndex > 0) AndAlso (ddlStyleGroups.SelectedIndex > 0) AndAlso (lbComponentNames.SelectedIndex >= 0) AndAlso (lbProducts.SelectedIndex >= 0) Then

                'if ComponentCode is not selected set to empty string. else use selection
                If (ddlComponentCodes.SelectedIndex <= 0) Then
                    componentCode = ""
                Else
                    componentCode = ddlComponentCodes.SelectedItem.Text
                End If

                SQL = "UPDATE dbo.tblSGProductCodeConfigWorking " & _
                      "SET ComponentCode = '" & componentCode & "' " & _
                      "WHERE LogInName = '" & Page.User.Identity.Name.ToString() & "' " & _
                      "AND StyleGroupID = " & ddlStyleGroups.SelectedValue & " " & _
                      "AND ComponentNameIDX = " & lbComponentNames.SelectedValue

                'if checkbox is unchecked limit the update to only the selected product
                If (cbAllProducts.Checked = False) Then
                    SQL += "AND ProductID = '" & lbProducts.SelectedValue & "' "
                End If

                DA.ExecSQL(SQL)

                hidDirtyBit.Value = "true"

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ValidateIfCheckBoxCanBeChecked()
        Try
            If cbAllProducts.Checked Then
                If (ddlLineNum.SelectedIndex <= 0) Or (ddlStyleGroups.SelectedIndex <= 0) Or (lbProducts.SelectedIndex < 0) Then
                    ' A product has to be selected before the box can be checked.
                    cbAllProducts.Checked = False
                    Master.tMsg("", "Please select a product before checking the Apply Settings to all Products checkbox.", True, "Red")
                Else
                    If lbProducts.Items.Count > 1 Then
                        ' If the user checks the box and there are more than one product in the list, 
                        '   make all the other products the same as the one selected.
                        DA.ExecSQL("EXECUTE dbo.procSGProductCodeConfiguration_SetAllProducts @StyleGroupID = " & ddlStyleGroups.SelectedValue & _
                                   ", @LogInName = '" & Page.User.Identity.Name.ToString() & "', @ProductID = '" & lbProducts.SelectedValue & "'")

                        hidDirtyBit.Value = "true"
                    End If
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub AddProductToListBox()
        Dim strSQL As String
        Dim bHasExistingProducts = False
        Dim indices As Integer()
        Dim idx As Integer
        Dim strProduct As String = ""

        Try
            If (lbAddProduct.SelectedIndex >= 0) Then
                indices = lbAddProduct.GetSelectedIndices()
                For Each idx In indices
                    strProduct = lbAddProduct.Items(idx).Value


                    Using ds As DataSet = DA.GetDataSet("SELECT COUNT(*) AS [total] FROM dbo.tblSGProductCodeConfigWorking WHERE [LogInName] = '" & Page.User.Identity.Name.ToString() & _
                                                        "' AND [StyleGroupID] = " & ddlStyleGroups.SelectedValue)
                        If ((ds.Tables.Count > 0) AndAlso (ds.Tables(0).Rows.Count) > 0 AndAlso (CInt(ds.Tables(0).Rows(0).Item("total")) > 0)) Then
                            bHasExistingProducts = True
                        End If
                    End Using

                    If (cbAllProducts.Checked And bHasExistingProducts) Then
                        ' Add rows to the working table with Component Codes that match the existing Products in this Style Group.
                        strSQL = "INSERT INTO dbo.tblSGProductCodeConfigWorking " & _
                                 "SELECT DISTINCT LogInName, StyleGroupID, '" & strProduct & "',ComponentNameIDX, ComponentCode " & _
                                 "FROM dbo.tblSGProductCodeConfigWorking " & _
                                 "WHERE [LogInName] = '" & Page.User.Identity.Name.ToString() & "' " & _
                                 "AND [StyleGroupID] = " & ddlStyleGroups.SelectedValue

                    Else
                        ' Add rows to the working table for the Product without any Component Codes defined.
                        strSQL = "INSERT INTO dbo.tblSGProductCodeConfigWorking " & _
                                 "SELECT '" & Page.User.Identity.Name.ToString() & "', " & ddlStyleGroups.SelectedValue & ", '" & strProduct & "', PLCArrayOrder, '' " & _
                                 "FROM dbo.tblSGComponentNames " & _
                                 "WHERE LineID = " & ddlLineNum.SelectedValue
                    End If

                    DA.ExecSQL(strSQL)
                Next

                lbProducts_LoadList()
                lbProducts.SelectedIndex = lbProducts.Items.IndexOf(lbProducts.Items.FindByText(lbAddProduct.SelectedValue))

                lbComponentNames.SelectedIndex = -1
                ddlComponentCodes.SelectedIndex = -1

                LoadMasksAndSelectComponentCode()

                hidDirtyBit.Value = "true"
            Else
                Master.tMsg("", "Nothing was selected in the Add Product dialog, so no product was added to this Style Group!", True, "Red")
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub RemoveProduct()
        Try
            DA.ExecSQL("DELETE FROM dbo.tblSGProductCodeConfigWorking WHERE [ProductID] = '" & lbProducts.SelectedItem.Value & _
                       "' AND StyleGroupID = " & ddlStyleGroups.SelectedValue & " AND LogInName = '" & Page.User.Identity.Name.ToString() & "'")

            lbProducts_LoadList()

            lbComponentNames.SelectedIndex = -1
            ddlComponentCodes.SelectedIndex = -1

            LoadMasksAndSelectComponentCode()

            hidDirtyBit.Value = "true"

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SaveConfiguration()
        Dim applicationParameterTypeID As String = ""
        Dim strLineNumber As String = ""

        Try
            If (ddlStyleGroups.SelectedIndex > 0) And (Page.User.Identity.IsAuthenticated) Then
                DA.ExecSQL("EXECUTE dbo.procSGProductCodeConfiguration_SaveData @StyleGroupID = " & ddlStyleGroups.SelectedValue & ", @LogInName = '" & Page.User.Identity.Name.ToString() & "'")

                hidDirtyBit.Value = "false"
                strLineNumber = CStr(CInt(ddlLineNum.SelectedValue) * 100)
                BizLayer.SetRecipeSavedDT("15", "0", strLineNumber)
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region


End Class