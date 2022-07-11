Public Class ErrorHistory
    Inherits System.Web.UI.Page

    Private Const m_strTopPlaceholder As String = "{2CC63627-62F6-4070-97C4-DC45E5C3E347}"
    Private m_strSQLBase As String = "SELECT TOP " & m_strTopPlaceholder & " [RecordedDT] AS DT1, ApplicationID, CustomError, AppTitle, CONVERT(varchar, [RecordedDT], 101) + ' ' + CONVERT(varchar, [RecordedDT], 8) [RecordedDT], REPLACE(ErrDescription, CHAR(13) + CHAR(10), '<BR>') [ErrDescription], REPLACE(REPLACE(ErrorMessage, CHAR(13) + CHAR(10), '\n'),'''','\''') ErrorMessage, [AdditionalInfo],'More Info' as 'More Info' FROM tblErrorHistory"

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                FillApplicationsDDL()

                'Default From/To Date textboxes
                txtFromDate.Text = Today.AddMonths(-1).ToString("MM/dd/yyyy")
                txtToDate.Text = Today.ToString("MM/dd/yyyy")

                'Let's Refresh the data on the first load of the page
                RefreshData()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            Master.Secure(cmdRefresh)
            Master.Secure(cmdTogglePaging)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Events"

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        Try
            RefreshData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdTogglePaging_Click(sender As Object, e As System.EventArgs) Handles cmdTogglePaging.Click
        Try
            'toggle paging 
            dgDetailGrid.AllowPaging = Not dgDetailGrid.AllowPaging

            BindData()

            If dgDetailGrid.AllowPaging Then
                cmdTogglePaging.Text = "Show All"
            Else
                cmdTogglePaging.Text = "Paging"
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dgDetailGrid_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDetailGrid.ItemDataBound
        Dim i As Integer = 0
        Dim peMSG As String = ""
        Dim paInfo As String = ""
        Dim pAnchorText As String = ""


        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                If e.Item.ItemIndex <> -1 Then

                    pEMSG = CType(e.Item.DataItem, DataRowView).Row("ErrorMessage").ToString()
                    paInfo = CType(e.Item.DataItem, DataRowView).Row("AdditionalInfo").ToString()


                    ''Create lotAnchor string
                    'pAnchorText = "<a id='lotAnchor_" & i & "' href='#' data-eMSG='" & peMSG & _
                    '    "' data-aInfo='" & paInfo & _
                    '    "'>" & paInfo & "</a>"


                    'Create lotAnchor string
                    pAnchorText = "<a id='lotAnchor_" & i & "' href='#' data-eMSG='" & peMSG & _
                        "' data-aInfo='" & paInfo & _
                        "'>" & "More Info" & "</a>"

                    'Cell(8) determines the link will be on the eigth column of the datagrid 
                    e.Item.Cells(3).Text = pAnchorText

                    i = i + 1

                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try





    End Sub

    Private Sub dgDetailGrid_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDetailGrid.PageIndexChanged
        Try
            ' Handles dgDetailGrid.PageIndexChanged
            dgDetailGrid.CurrentPageIndex = e.NewPageIndex

            BindData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dgDetailGrid_SortCommand(source As Object, e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgDetailGrid.SortCommand
        Try
            'changed because sorting of the date time column needs to be done on a column that has not been formatted
            'that is, RecordedDT column has a different format than original DT1 column. sorting on formatted column does not work right. (PB)
            Dim sortExpr As String = e.SortExpression
            'replace DateTime with DT1
            If (sortExpr.IndexOf("RecordedDT") >= 0 Or sortExpr.IndexOf("[RecordedDT]") >= 0) Then
                sortExpr = "DT1"
            End If

            Dim sortType As String
            If (txtSortField.Value.IndexOf("DESC") >= 0) Then
                sortType = ""
            Else : sortType = " DESC"
            End If

            txtSortField.Value = sortExpr & sortType

            'go back to starting page index position
            dgDetailGrid.CurrentPageIndex = 0

            're-bind data
            BindData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlApplications_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlApplications.SelectedIndexChanged
        Try
            RefreshData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlDisplayCount_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDisplayCount.SelectedIndexChanged
        Try
            RefreshData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtFromDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtFromDate.TextChanged
        Try
            RefreshData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtToDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtToDate.TextChanged
        Try
            RefreshData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub FillApplicationsDDL()
        Dim oListItem As ListItem

        ddlApplications.DataSource = DA.GetDataSet("SELECT ApplicationID, [Description] FROM dbo.tblApplications")

        ddlApplications.DataTextField = "Description"
        ddlApplications.DataValueField = "ApplicationID"
        ddlApplications.DataBind()

        ddlApplications.Items.Add("ALL")
        oListItem = ddlApplications.Items.FindByText("ALL")

        If Not oListItem Is Nothing Then
            oListItem.Selected = True
        End If
    End Sub

    Private Sub RefreshData()
        If CheckDateFormats() Then
            Session("dtErrorHistory") = Nothing

            BindData()
        End If
    End Sub

    Private Function CheckDateFormats() As Boolean
        Dim blnResult As Boolean = True

        If Not IsDate(Me.txtFromDate.Text) Then
            Master.Msg += "Invalid From Date format."
            blnResult = False
        End If

        If Not IsDate(Me.txtToDate.Text) Then
            Master.Msg += "Invalid To Date format."
            blnResult = False
        End If

        Return blnResult
    End Function

    Private Sub BindData()
        Dim oDataView As DataView
        Dim oDataTable As DataTable
        Try
            ' If the DataView can be found in the cache, use it.
            If Session("dtErrorHistory") IsNot Nothing Then
                ' All items in the cache are of type Object, so you have to explicitly cast it before working with it further.
                oDataTable = CType(Session("dtErrorHistory"), DataTable)
                oDataView = oDataTable.DefaultView
            Else ' DataView was not found in the cache.
                'Update the SQL string according to selection of how many rows to display
                m_strSQLBase = m_strSQLBase.Replace(m_strTopPlaceholder, ddlDisplayCount.SelectedItem.Value)

                'Filter the data based on the application selected
                If Me.ddlApplications.SelectedItem.Value.CompareTo("ALL") <> 0 Then
                    m_strSQLBase += " WHERE ApplicationID='" & ddlApplications.SelectedItem.Value & "'"
                End If

                'Additionally filter the data based on the from and to dates
                If m_strSQLBase.IndexOf("WHERE") = -1 Then
                    m_strSQLBase += " WHERE "
                Else
                    m_strSQLBase += " AND "
                End If
                m_strSQLBase += " [RecordedDT] BETWEEN '" & Me.txtFromDate.Text & " 00:00:00' AND '" & Me.txtToDate.Text & " 23:59:59'"

                'now construct the order by/sort so that the right TOP XXXX records are returned
                'm_strSQLBase += " ORDER BY " & txtSortField.Value
                If (txtSortField.Value.IndexOf("RecordedDT") >= 0) Then
                    m_strSQLBase += " ORDER BY CONVERT(datetime, " & txtSortField.Value.Split(CChar(" "))(0) & ") " & txtSortField.Value.Split(CChar(" "))(1)
                Else
                    m_strSQLBase += " ORDER BY " & txtSortField.Value
                End If

                ' Create a new DataView.
                'oDataView = DA.GetDataSet(m_strSQLBase).Tables(0).DefaultView
                oDataTable = DA.GetDataSet(m_strSQLBase).Tables(0)
                oDataView = oDataTable.DefaultView

                ' Put the new DataView into the Cache
                'Cache("dtErrorHistory") = oDataView
                'Cache.Insert("dtErrorHistory", oDataView, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(Session.Timeout))
                'Session("dtErrorHistory") = oDataView
                Session("dtErrorHistory") = oDataTable
            End If

            'Sort data (this might be redundant if the previous logic retrieved the data from the DB.  (Don't worry about redundancy - OK)
            'Sorting is done here, not in DB query (PB)
            oDataView.Sort = txtSortField.Value

            With dgDetailGrid
                'If .Items.Count <= 0 Then
                If oDataView.Table.Rows.Count <= 0 Then
                    .VirtualItemCount = CInt(False)
                    .Visible = False

                    Master.Msg = "No data found"
                Else
                    .DataSource = oDataView 'oDataset.Tables(0).DefaultView
                    .DataBind()

                    Trace.Write("dgDetailGrid.Columns.Count=" & .Columns.Count)
                    Trace.Write("dgDetailGrid.Items.Count=" & .Items.Count)

                    .Visible = True

                    Master.Msg = oDataView.Table.Rows.Count & " record(s) found."
                End If
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

End Class