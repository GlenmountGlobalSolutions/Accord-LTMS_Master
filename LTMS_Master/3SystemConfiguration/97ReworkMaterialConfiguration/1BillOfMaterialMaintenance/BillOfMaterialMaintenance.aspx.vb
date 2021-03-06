Imports System.Data.SqlClient

Public Class BillOfMaterialMaintenance
    Inherits System.Web.UI.Page

#Region "constants"

    Public Const COMPONENT_ID As String = "04"
    Private Const CAT_TYPE_ID As String = "1"
    Private Const COL_PN As String = "PartNumber"
    Private Const COL_PRODUCT_ID As String = "ProductID"
    Private Const COL_LIST_VAL_ID As String = "ListValueID"
    Private Const COL_LIST_VALUE As String = "ListValue"
    Private Const COL_PROD_PARAM_VAL As String = "ProductParameterValue"
    Private Const PROD_PARAM_TYPE_ID_COLOR As String = "0215"
    Private Const PROD_PARAM_TYPE_ID_LINE As String = "0216"
    Private Const PROD_PARAM_TYPE_ID_SEAT_CODE As String = "0213"

    Private Const KEY_CHECK_IMG_ID As String = "CHECK_IMG_ID"

#End Region

#Region "global vars"

    'data stored after change events
    Private sql As New ArrayList
    Private log As New ArrayList
    Private checkPics As New ArrayList

#End Region

#Region "Event Handlers"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If (Not IsPostBack) Then
                LoadPageData()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Try
            EnableControls()

            'hide second display box because the frame shows as a tiny circle when nothing is selected.
            If ddlMatType.SelectedIndex > 0 Then
                Me.hideMe.Visible = True
            Else
                Me.hideMe.Visible = False
            End If

            'enable/disable material types drop down
            If ddlComp.SelectedIndex > 0 Then
                ddlMatType.Enabled = True
            Else : ddlMatType.Enabled = False
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbProducts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbProducts.SelectedIndexChanged
        Try
            'clear/reset all controls on the page.
            Me.ddlComp.Items.Clear()   'drop down components
            Me.ddlMatType.Items.Clear()   'drop down material category
            Me.dlValues.Controls.Clear()   'datalist of parts

            Dim id As String
            If (Me.lbProducts.SelectedIndex >= 0) Then
                id = Me.lbProducts.SelectedValue
                LoadProductLabel()  'displayed description at top of center table.
                LoadSeatComponentDDL()   'load the Seat Component dropdown.

                'deal with copy dialog
                Me.lblPrompt.Text = ("Copy from product " + Me.lbProducts.SelectedValue + " to: ")
                LoadCopyDialogProductDDL()

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlComp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlComp.SelectedIndexChanged
        Try
            Me.ddlMatType.Items.Clear()   'drop down material category
            Me.dlValues.Controls.Clear()   'datalist of parts
            If (Me.ddlComp.SelectedIndex > 0) Then
                LoadMaterialTypesDDL()  ' load teh material category drop down

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlMatType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMatType.SelectedIndexChanged
        Try
            Me.dlValues.Controls.Clear()

            Dim pId As String = ""
            Dim compId As String = ""
            Dim catId As String = ""
            If (Me.lbProducts.SelectedIndex >= 0) Then
                pId = Me.lbProducts.SelectedValue

                If (Me.ddlComp.SelectedIndex > 0) Then
                    compId = Me.ddlComp.SelectedValue

                    If (Me.ddlMatType.SelectedIndex > 0) Then
                        catId = Me.ddlMatType.SelectedValue
                        PartDataList(pId, compId, catId)   'load and populate the datalist  (third table on page)

                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' called by the GGS:WebInputBox control in each row of the datalist on textChanged.

        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            'Dim partNum As String = ""
            Dim prodID, comp, listType, listValID, oldPN, newPN, desc, imgID As String

            prodID = Me.lbProducts.SelectedItem.Text
            comp = Me.ddlComp.SelectedItem.Text
            listType = Me.ddlMatType.SelectedItem.Text

            oldPN = wtb.OldText
            newPN = wtb.Text

            'prodID = BizLayer.GetKeyValue(wtb.SQLText, COL_PRODUCT_ID)
            listValID = BizLayer.GetKeyValue(wtb.SQLText, COL_LIST_VAL_ID)
            desc = BizLayer.GetKeyValue(wtb.SQLText, COL_LIST_VALUE)

            'log event and create update sql statement
            log.Add(desc + " for " + prodID + ", " + comp + ", " + listType + " was saved. Old part number: " + oldPN + ", new part number: " + newPN + ".<br>")

            'CREATE PROCEDURE procRMDUpdateProductPart	@ProductID varchar(32),
            '				@ListValueID int,
            '				@PartNumber varchar(80)
            sql.Add("EXEC procRMDUpdateProductPart '" + prodID + "', '" + listValID + "', '" + newPN + "'")

            'save check mark image id
            imgID = BizLayer.GetKeyValue(wtb.SQLText, KEY_CHECK_IMG_ID)
            checkPics.Add(imgID)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim i As Integer
            Dim execSql As String = ""
            Dim finalLog As String = ""
            Dim logStr As String
            Dim img As Web.UI.WebControls.Image = Nothing
            Dim s As String = ""

            'error checking
            If (log.Count <= 0) Then
                Master.Msg = "Not saved: Values have not changed"
            Else
                If (Me.lbProducts.SelectedIndex < 0) Then
                    Master.Msg = "Not Saved: Please select a product"
                Else
                    'execute all save sql statements
                    For Each execSql In sql
                        DA.ExecSQL(execSql)
                        i = 0
                    Next

                    'get one long log string
                    For Each logStr In log
                        finalLog += logStr
                    Next

                    i = 0
                    Master.tMsg("Save", finalLog)

                    ''display check mark pics for every saved item

                    For Each s In checkPics
                        For i = 0 To dlValues.Controls.Count - 1
                            img = CType(Page.FindControl(s), Image)
                            If (Not img Is Nothing) Then
                                img.Visible = True
                                Exit For
                            End If
                        Next
                    Next
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Try
            'clear/reset product info and part detail controls
            Me.ddlComp.SelectedIndex = -1
            Me.ddlMatType.Items.Clear()
            Me.dlValues.Controls.Clear()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopy.Click
        Try
            'execute s.p.
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)

            Dim status As String = ""
            Dim message As String = ""

            'CREATE PROCEDURE procRMDCopyProduct		@ProductToCopy varchar(80), 
            '							@ProductCopiedTo varchar(80), 
            '							@Status varchar(80) out, 
            '							@ErrorMsg varchar(80) out  AS

            oSqlParameter = New Data.SqlClient.SqlParameter("@ProductToCopy", SqlDbType.VarChar, 80)
            oSqlParameter.Value = Me.lbProducts.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New Data.SqlClient.SqlParameter("@ProductCopiedTo", SqlDbType.VarChar, 80)
            oSqlParameter.Value = ddlProd.SelectedItem.Value
            colParameters.Add(oSqlParameter)


            oSqlParameter = New Data.SqlClient.SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New Data.SqlClient.SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procRMDCopyProduct", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status.ToUpper <> "TRUE") Then
                Master.tMsg("Error", "Error: Unable to copy Bill of Material Maintenance configuration.<br>S.P. Status: " + status + ".<br>S.P. Error Message: " + message)
            Else
                Master.tMsg("Copy", "Bill of Material Maintenance configuration was copied from " + Me.lbProducts.SelectedValue + " to " + ddlProd.SelectedItem.Value)
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Function DLControlBind(ByVal sender As Object) As String
        'function is called by the datalist dlvalues for each row of data.

        Dim dlItem As DataListItem = CType(sender, DataListItem)
        Dim drv As System.Data.DataRowView = CType(dlItem.DataItem, DataRowView)
        'get ref to textbox
        Dim wtb As GGS.WebInputBox = CType(dlItem.FindControl("dlWibPN"), GGS.WebInputBox)

        'get check image
        Dim img As Web.UI.WebControls.Image = CType(dlItem.FindControl("dlImgCheck"), Web.UI.WebControls.Image)

        Dim text As String = ""
        Dim prodID As String = ""
        Dim partNum As String = ""
        Dim listValID As String = ""
        Dim desc As String = ""

        If (Not drv.Item(COL_PRODUCT_ID) Is System.DBNull.Value) Then
            prodID = drv.Item(COL_PRODUCT_ID).ToString()
        End If
        If (Not drv.Item(COL_PN) Is System.DBNull.Value) Then
            partNum = drv.Item(COL_PN).ToString()
        End If
        If (Not drv.Item(COL_LIST_VAL_ID) Is System.DBNull.Value) Then
            listValID = drv.Item(COL_LIST_VAL_ID).ToString()
        End If
        If (Not drv.Item(COL_LIST_VALUE) Is System.DBNull.Value) Then
            desc = drv.Item(COL_LIST_VALUE).ToString()
        End If

        Dim value As String = ""
        'value = BizLayer.SetKeyValue(value, COL_PRODUCT_ID, prodID)
        value = BizLayer.SetKeyValue(value, COL_PN, partNum)
        value = BizLayer.SetKeyValue(value, COL_LIST_VAL_ID, listValID)
        value = BizLayer.SetKeyValue(value, COL_LIST_VALUE, desc)
        value = BizLayer.SetKeyValue(value, KEY_CHECK_IMG_ID, img.UniqueID)
        wtb.SQLText = value

        'compare ProductID column to current product selection
        'if they are equal then add PartNumber column value to text property of textbox
        If (prodID = Me.lbProducts.SelectedValue) Then
            wtb.Text = partNum
            wtb.OldText = partNum
        End If
        Return ""

    End Function

    Private Sub PartDataList(ByVal ProdID As String, ByVal CompID As String, ByVal matTypeID As String)
        Try
            Dim dlSql As String = ""
            Dim ds As DataSet

            Me.dlValues.Controls.Clear()

            'CREATE PROCEDURE procRMDGetPartNumbers @prodID varchar(50), @compID int, @matTypeID int AS
            dlSql = "EXEC procRMDGetPartNumbers '" + ProdID + "', " + CompID + ", " + matTypeID

            ds = DA.GetDataSet(dlSql)
            If (ds IsNot Nothing) Then
                Me.dlValues.DataSource = ds
                Me.dlValues.DataBind()

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadPageData()
        LoadProductList()
        LoadProductLabel()
        LoadSeatComponentDDL()
        LoadCopyDialogProductDDL()

    End Sub

    Private Sub LoadProductList()
        Try
            Dim ds As DataSet
            Dim sql As String = "SELECT ProductID AS VALUE, ProductID AS TEXT FROM tblProducts WHERE (ComponentID = '" + COMPONENT_ID + "') ORDER BY ProductID"

            Me.lbProducts.Items.Clear()

            ds = DA.GetDataSet(sql)
            If Not (DA.IsDSEmpty(ds)) Then
                Me.lbProducts.DataSource = ds
                Me.lbProducts.DataTextField = "TEXT"
                Me.lbProducts.DataValueField = "VALUE"
                Me.lbProducts.DataBind()
                Me.lbProducts.SelectedIndex = 0
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadProductLabel()
        Try
            Dim ProdID As String = Me.lbProducts.SelectedValue
            Dim ds As DataSet
            Dim sql As String = ""

            sql += " SELECT ProductParameterTypeID, ProductID, ProductParameterValue, ComponentID"
            sql += " FROM (SELECT Prod.ComponentID, Params.ProductParameterValue, Prod.ProductID, Params.ProductParameterTypeID"
            sql += " FROM tblProducts Prod INNER JOIN"
            sql += " tblProductParameters Params ON Prod.ProductID = Params.ProductID"
            sql += " WHERE (Prod.ComponentID = '" + COMPONENT_ID + "') AND (Prod.ProductID = '" + ProdID + "')) DERIVEDTBL"
            sql += " WHERE (ProductParameterTypeID = '" + PROD_PARAM_TYPE_ID_COLOR + "') OR"
            sql += " (ProductParameterTypeID = '" + PROD_PARAM_TYPE_ID_LINE + "') OR"
            sql += " (ProductParameterTypeID = '" + PROD_PARAM_TYPE_ID_SEAT_CODE + "')"
            sql += " ORDER BY ProductParameterTypeID"

            ds = DA.GetDataSet(sql)
            Me.lblPartInfo.Text = ""

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count = 3)) Then
                Me.lblPartInfo.Text += (ds.Tables(0).DefaultView.Table.Rows(2)(COL_PROD_PARAM_VAL).ToString() + ", ")
                Me.lblPartInfo.Text += (ds.Tables(0).DefaultView.Table.Rows(0)(COL_PROD_PARAM_VAL).ToString() + ", ")
                Me.lblPartInfo.Text += ds.Tables(0).DefaultView.Table.Rows(1)(COL_PROD_PARAM_VAL).ToString()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadSeatComponentDDL()
        Try
            Dim ds As DataSet
            Dim sql As String = "SELECT ComponentID AS VALUE, Description AS TEXT FROM tblRMDComponents"

            Me.ddlComp.Items.Clear()

            ds = DA.GetDataSet(sql)
            If (ds IsNot Nothing) Then
                Me.ddlComp.DataSource = ds
                Me.ddlComp.DataTextField = "TEXT"
                Me.ddlComp.DataValueField = "VALUE"
                Me.ddlComp.DataBind()

                Me.ddlComp.Items.Insert(0, "Please Select")
                Me.ddlComp.SelectedIndex = 0
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadMaterialTypesDDL()
        Try
            Dim CompID As String = Me.ddlComp.SelectedValue
            Me.ddlMatType.Items.Clear()

            Dim ds As DataSet
            Dim sql As String = ""

            sql += " SELECT MatTypes.MaterialTypeID AS VALUE, MatTypes.Description AS TEXT"
            sql += " FROM tblRMDCategories Cat INNER JOIN"
            sql += " tblRMDCategoryTypes CatTypes ON Cat.CategoryTypeID = CatTypes.CategoryTypeID INNER JOIN"
            sql += " tblRMDMaterialTypes MatTypes ON Cat.CategoryID = MatTypes.CategoryID"
            sql += " WHERE (Cat.ComponentID = " + CompID + ") AND (CatTypes.CategoryTypeID = " + CAT_TYPE_ID + ")"

            ds = DA.GetDataSet(sql)

            If (ds IsNot Nothing) Then
                Me.ddlMatType.DataSource = ds
                Me.ddlMatType.DataTextField = "TEXT"
                Me.ddlMatType.DataValueField = "VALUE"
                Me.ddlMatType.DataBind()

                Me.ddlMatType.Items.Insert(0, "Please Select")
                Me.ddlMatType.SelectedIndex = 0
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdCancel)
            Master.Secure(Me.cmdSave)
            Master.Secure(Me.cmdCopy)

            If (Me.lbProducts.SelectedIndex < 0) Then
                Me.cmdCopy.Enabled = False
            End If

            If (Me.ddlMatType.SelectedIndex <= 0) Then
                Me.cmdSave.Enabled = False
                Me.cmdCancel.Enabled = False
            Else
                Me.cmdSave.Enabled = True
                Me.cmdCancel.Enabled = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadCopyDialogProductDDL()
        Dim ds As DataSet
        Dim sql As String = "SELECT ProductID AS VALUE, ProductID AS TEXT FROM tblProducts WHERE (ComponentID = '" + COMPONENT_ID + "') "
        sql += " AND (ProductID <> '" + Me.lbProducts.SelectedValue
        sql += "') ORDER BY ProductID"

        Me.ddlProd.Items.Clear()

        ds = DA.GetDataSet(sql)
        If (ds IsNot Nothing) Then
            Me.ddlProd.DataSource = ds
            Me.ddlProd.DataTextField = "TEXT"
            Me.ddlProd.DataValueField = "VALUE"
            Me.ddlProd.DataBind()
            Me.ddlProd.SelectedIndex = 0
        End If
    End Sub

#End Region
End Class