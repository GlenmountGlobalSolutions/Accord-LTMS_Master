Imports System.Data.SqlClient

Public Class ProductConfiguration
    Inherits System.Web.UI.Page

#Region "Globals"

    Protected dbDataRVProdParam As System.Data.DataRowView 'holds a reference to DataItem in datalist object
    Public sqlText As New ArrayList           'sql to make update
    Public preSQL As New ArrayList
    Public InputCaption As New ArrayList        'caption asscoiated with control
    Public OldText As New ArrayList       'old value of ctrl
    Public NewText As New ArrayList       'new posted value of ctrl
    'Protected WithEvents ddComponetType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents imgCheck As System.Web.UI.WebControls.Image
    Dim inputbox As GGS.WebInputBox
    Dim ddList As GGS.WebDropDownList

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                ddProductType_DataBind()
                lbProductList_Bind()
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

    Private Sub ddProductType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddProductType.SelectedIndexChanged
        Try
            dlProductConfig.Controls.Clear()
            lbProductList_Bind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbProductList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbProductList.SelectedIndexChanged
        Try
            dlProductConfig_Bind()
            Master.Msg = "Product Parameters for: " & lbProductList.SelectedItem.Value

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCopy_Click(sender As Object, e As System.EventArgs) Handles cmdCopy.Click
        Try
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As Data.SqlClient.SqlParameter

            If (txtCopyProduct.Text.Length = 0) Then
                Master.Alert = "A Product name must be entered in the dialog to do a copy!"
            Else
                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@CopyFrom", SqlDbType.VarChar, 48)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = lbProductList.SelectedItem.ToString()
                colParms.Add(prmNext)
                'IN--------------------------------------------------------
                prmNext = New Data.SqlClient.SqlParameter("@CopyTo", SqlDbType.VarChar, 48)
                prmNext.Direction = ParameterDirection.Input
                prmNext.Value = txtCopyProduct.Text
                colParms.Add(prmNext)
                '------------------------------------------------------------
                DA.ExecSP("procASPCopyProduct", colParms)

                Master.tMsg("Copy", "Product: " & lbProductList.SelectedItem.ToString() & " was copied with new ID: " & txtCopyProduct.Text)
                lbProductList_Bind()
                lbProductList.SelectedIndex = lbProductList.Items.IndexOf(lbProductList.Items.FindByText(txtCopyProduct.Text))

                dlProductConfig_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        Try
            If (Not lbProductList.SelectedItem Is Nothing) Then

                Dim strListItemThatWasDeleted As String = lbProductList.SelectedItem.ToString

                Try
                    DA.GetDataSet("DELETE FROM dbo.tblSGProductCodeConfigWorking WHERE ProductID = '" & lbProductList.SelectedItem.ToString & "'")
                    DA.GetDataSet("DELETE FROM dbo.tblSGProductCodeConfig WHERE ProductID = '" & lbProductList.SelectedItem.ToString & "'")
                    DA.GetDataSet("Delete from tblProducts where ProductID ='" & lbProductList.SelectedItem.ToString & "'")
                    Master.tMsg("Delete", "Product ID: " & strListItemThatWasDeleted & " was deleted")
                Catch E1 As Exception
                    Master.eMsg(E1.ToString())
                Finally
                    lbProductList_Bind()
                    dlProductConfig.Controls.Clear()
                End Try
            Else
                Master.Alert = "Please, select the product you want to delete!"
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim strInsert As String
            Dim strNewProduct As String
            strNewProduct = txtNewProduct.Text

            If txtNewProduct.Text.Length > 0 Then
                Try
                    strInsert = "INSERT INTO tblProducts (ProductID,ComponentID) VALUES ('" & txtNewProduct.Text & "','" & ddProductType.SelectedValue & "')"
                    DA.ExecSQL(strInsert)
                    Master.tMsg("New", "New product with ID: " & txtNewProduct.Text & " was added")

                    ddProductType.SelectedIndex = ddProductType.Items.IndexOf(ddProductType.Items.FindByValue(ddProductType.SelectedValue))
                    lbProductList_Bind()
                    lbProductList.SelectedIndex = lbProductList.Items.IndexOf(lbProductList.Items.FindByValue(txtNewProduct.Text))
                    dlProductConfig_Bind()

                Catch ex As Exception When Err.Number = 5
                    Master.Alert = "Error! Product with ID: " & txtNewProduct.Text & " already exists!"

                Catch ex As Exception
                    Master.eMsg(ex.ToString())
                End Try
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRename_Click(sender As Object, e As System.EventArgs) Handles cmdRename.Click
        Try
            Dim ds As DataSet

            If (txtRenameProduct.Text.Length > 0) Then
                ds = DA.GetDataSet("Update tblProducts SET ProductID = '" & txtRenameProduct.Text & "' WHERE ProductID = '" & lbProductList.SelectedItem.Value & "'")
                Master.tMsg("Rename", "Product: " & lbProductList.SelectedItem.Text & " was renamed to: " & txtRenameProduct.Text)
                lbProductList_Bind()
                lbProductList.SelectedIndex = lbProductList.Items.IndexOf(lbProductList.Items.FindByValue(txtRenameProduct.Text))
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim i As Int32
            Dim _sqlText As String

            If (Not lbProductList.SelectedItem Is Nothing) Then    'list box is slelected then do save
                For Each _sqlText In sqlText
                    DA.ExecSQL(_sqlText)
                    Master.tMsg("Save", "Parameter: " & InputCaption(i).ToString() & " for ProductID: " & lbProductList.SelectedItem.Text & " was changed from : " & OldText(i).ToString() & " to: " & NewText(i).ToString())
                    i = i + 1
                Next
                dlProductConfig_Bind()    'rebinding datalist with updated values

            Else
                Master.Alert = "There is nothing to save. No product is selected"
                dlProductConfig_Bind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub dlProductConfig_Bind()  'data list bind
        With dlProductConfig
            .DataSource = Nothing

            If (lbProductList.SelectedIndex >= 0) Then
                Dim TheSQL As String = _
                    "SELECT p.ProductParameterTypeID, t.[Description], p.ProductParameterValue, " & _
                            "t.EnableCharacterSwap, t.ParameterListID, p.ProductID " & _
                    "FROM tblProductParameters p " & _
                    "LEFT OUTER JOIN tblProductParameterTypes t " & _
                    "ON p.ProductParameterTypeID = t.ProductParameterTypeID " & _
                    "WHERE p.ProductID = '" & lbProductList.SelectedItem.Value & "' " & _
                    "AND t.ComponentID = '" & ddProductType.SelectedItem.Value & "' " & _
                    "ORDER BY t.DisplayID"

                .DataSource = DA.GetDataSet(TheSQL)
            End If

            .DataBind()
        End With
    End Sub

    Public Sub dbProducts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Performing Save. If datata in textbox changed then run appropriate sql statements
        'event/method is raised by FRAMEWORK
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            Dim x As String
            x = wtb.SQLText

            preSQL.Add(x)
            If (x.IndexOf("{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD}") > 0) Then

                wtb.Text = FormatForSaveOrDisplay(wtb.Text, True)
                x = x.Replace("{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD}", wtb.Text)
                'x = x.Replace("{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD}", "")

                sqlText.Add(x)
                InputCaption.Add(wtb.InputCaption)
                OldText.Add(wtb.OldText)
                NewText.Add(wtb.Text)
            Else
                x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", wtb.Text)
                sqlText.Add(x)
                InputCaption.Add(wtb.InputCaption)
                OldText.Add(wtb.OldText)
                NewText.Add(wtb.Text)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub dbProducts_IndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Performing Save. If datata in dropwon list changed then run appropriate sql statements
        'event/method:dbProducts_IndexChanged is raised by FRAMEWORK if data is changed
        Try
            Dim wddl As GGS.WebDropDownList = CType(sender, GGS.WebDropDownList)    'getting ref to control
            Dim x, TextToSave As String

            x = wddl.SQLText
            preSQL.Add(x)

            If (x.IndexOf("{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD}") > 0) Then
                TextToSave = FormatForSaveOrDisplay(wddl.SelectedItem.ToString, True)
                x = x.Replace("{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD}", TextToSave)
                sqlText.Add(x)
                InputCaption.Add(wddl.InputCaption)
                OldText.Add(wddl.OldText)
                NewText.Add(wddl.SelectedItem.Text)
            Else
                x = wddl.SQLText
                preSQL.Add(x)
                x = x.Replace("6933A6C7-80FC-4b37-907E-26FCE24DD7EE", wddl.SelectedItem.ToString)
                sqlText.Add(x)
                InputCaption.Add(wddl.InputCaption)
                OldText.Add(wddl.OldText)
                NewText.Add(wddl.SelectedItem.Text)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Function ObjectVisibilityInTemplate(ByVal e As Object) As Object
        Try
            Dim SQLUpdateCommandText As String
            Dim ParameterListID As String
            Dim ProductParameterValue As String
            Dim strInputCaption As String    'caption wich is next of the input or drop box
            Dim imgCheck As New System.Web.UI.WebControls.Image

            Dim dlItem As DataListItem = CType(e, DataListItem)
            Dim dbDataRVProdParam As DataRowView = CType(dlItem.DataItem, DataRowView) 'holds a reference to DataItem in datalist object

            Dim ctrl As Control
            Dim colofCtrl As ControlCollection

            colofCtrl = dlItem.Controls 'geting ref to collection of controls in the template per record
            For Each ctrl In colofCtrl
                Select Case ctrl.ToString()
                    Case "GGS.WebDropDownList"
                        ddList = CType(ctrl, GGS.WebDropDownList)
                        ddList.ToolTip = "Product Parameter Type ID = " & dbDataRVProdParam("ProductParameterTypeID").ToString & ""

                    Case "GGS.WebInputBox"
                        inputbox = CType(ctrl, GGS.WebInputBox)
                        inputbox.ToolTip = "Product Parameter Type ID = " & dbDataRVProdParam("ProductParameterTypeID").ToString & ""

                    Case "System.Web.UI.WebControls.Image"
                        imgCheck = CType(ctrl, Image)
                End Select
            Next

            strInputCaption = dbDataRVProdParam("Description").ToString
            ParameterListID = dbDataRVProdParam("ParameterListID").ToString
            ProductParameterValue = dbDataRVProdParam("ProductParameterValue").ToString

            If (Not (dbDataRVProdParam("ParameterListID") Is System.DBNull.Value)) Then    'if ParameterListID is empty display textbox else dropbox
                inputbox.Visible = False
                ddList.Visible = True    'displaying dropbox
                Dim charswap As Boolean

                If (dbDataRVProdParam("EnableCharacterSwap").ToString = "") Then
                    charswap = False
                Else
                    charswap = CBool(dbDataRVProdParam("EnableCharacterSwap"))
                End If
                dbParameterListID_Bind(Trim(ParameterListID), Trim(ProductParameterValue), charswap)     'binding databox to source
            Else
                ddList.Visible = False
                inputbox.Visible = True
            End If

            If (dbDataRVProdParam("EnableCharacterSwap").ToString = "True") Then

                inputbox.Text = FormatForSaveOrDisplay(inputbox.Text, False)
                'i DO NOT DO FormatForSaveOrDisplay/CHARACTER SWAP FOR DROPDOWN BECAUSE LIST COME FROM tblParameterLists
                '{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD} INDCATES THAT CHAR SWAP IS REQUIRED
                SQLUpdateCommandText = "UPDATE tblProductParameters Set ProductParameterValue = '{D3E737F9-DB6B-4c5d-A8FD-14DDA9E637AD}' WHERE " _
                  & "(ProductID = '" & dbDataRVProdParam("ProductID").ToString & "') AND (ProductParameterTypeID = '" & dbDataRVProdParam("ProductParameterTypeID").ToString & "') "
            Else
                SQLUpdateCommandText = "UPDATE tblProductParameters Set ProductParameterValue = '6933A6C7-80FC-4b37-907E-26FCE24DD7EE' WHERE " _
                  & "(ProductID = '" & dbDataRVProdParam("ProductID").ToString & "') AND (ProductParameterTypeID = '" & dbDataRVProdParam("ProductParameterTypeID").ToString & "') "
            End If


            inputbox.SQLText = SQLUpdateCommandText
            inputbox.InputCaption = strInputCaption
            inputbox.OldText = ProductParameterValue

            ddList.SQLText = SQLUpdateCommandText
            ddList.InputCaption = strInputCaption

            Dim k As String
            imgCheck.Visible = False
            For Each k In preSQL
                If (String.CompareOrdinal(Trim(k), Trim(SQLUpdateCommandText)) = 0) Then
                    imgCheck.Visible = True
                End If
            Next

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return Nothing
    End Function

    Private Sub dbParameterListID_Bind(ByRef paramID As String, ByRef Param As String, ByVal bSwapChr As Boolean)
        Dim strSQL As String

        strSQL = "SELECT tblParameterListValues.ParameterListValue " & _
                 "FROM tblParameterLists " & _
                 "INNER JOIN tblParameterListValues " & _
                 "ON tblParameterLists.ParameterListID = tblParameterListValues.ParameterListID " & _
                 "WHERE (tblParameterLists.ParameterListID = '" & paramID & "')"

        ddList.DataSource = DA.GetDataSet(strSQL)
        ddList.DataTextField = "ParameterListValue"
        ddList.DataValueField = "ParameterListValue"
        ddList.DataBind()
        ddList.TabIndex.ToString()

        If bSwapChr Then
            ddList.Items.Insert(0, FormatForSaveOrDisplay(Param, False))
            ddList.OldText = Param
            If Not Param = "" Then        'if there is blank one already do not insert
                ddList.Items.Insert(1, New ListItem)
            End If
        Else
            ddList.Items.Insert(0, Param)
            If Not Param = "" Then        'if there is blank one already do not insert
                ddList.Items.Insert(1, New ListItem)
            End If
            ddList.OldText = Param
        End If
    End Sub

    Private Function FormatForSaveOrDisplay(ByRef str As String, ByRef bfrmt As Boolean) As String
        Dim dschar As DataSet
        Dim dr As DataRow

        dschar = DA.GetDataSet("SELECT *  FROM tblCharacterSwap")
        If (bfrmt = False) Then       'display
            For Each dr In dschar.Tables(0).Rows
                str = str.Replace(dr(1).ToString, dr(0).ToString)
            Next
            Return str

        Else          'save
            For Each dr In dschar.Tables(0).Rows
                str = str.Replace(dr(0).ToString, dr(1).ToString)
            Next
            Return str
        End If
    End Function

    Private Sub lbProductList_Bind()    'list box bind
        If (ddProductType.SelectedIndex <= 0) Then
            lbProductList.Items.Clear()
        Else
            lbProductList.DataSource = DA.GetDataSet("SELECT ProductID, [Description] FROM dbo.tblProducts WHERE ComponentID = '" & ddProductType.SelectedItem.Value & "'")
            lbProductList.DataTextField = "ProductID"
            lbProductList.DataValueField = "ProductID"
            lbProductList.DataBind()
        End If
    End Sub

    Private Sub ddProductType_DataBind()
        Dim strsql As String
        strsql = "SELECT ComponentID, Description FROM dbo.tblComponentID"
        ddProductType.DataSource = DA.GetDataSet(strsql)
        ddProductType.DataTextField = "Description"
        ddProductType.DataValueField = "ComponentID"
        ddProductType.DataBind()
        ddProductType.Items.Insert(0, "Choose a Type")
    End Sub

    Private Sub EnableControls()
        Master.Secure(Me.cmdNew)
        Master.Secure(Me.cmdSave)
        Master.Secure(Me.cmdRename)
        Master.Secure(Me.cmdDelete)
        Master.Secure(Me.cmdCopy)

        If (lbProductList.SelectedItem Is Nothing) Then
            cmdSave.Enabled = False
            cmdDelete.Enabled = False
            cmdCopy.Enabled = False
            cmdRename.Enabled = False
        End If

        If (Me.ddProductType.SelectedIndex < 0) Then
            Me.cmdNew.Enabled = False
        End If
    End Sub

#End Region

End Class