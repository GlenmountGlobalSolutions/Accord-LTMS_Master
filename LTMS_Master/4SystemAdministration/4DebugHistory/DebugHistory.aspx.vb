Imports System.IO

Public Class DebugHistory
    Inherits System.Web.UI.Page


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                LoadApplicationsList()

                'Default From/To Date textboxes
                txtFromDate.Text = Today.AddMonths(-1).ToString("MM/dd/yyyy")
                txtToDate.Text = Today.ToString("MM/dd/yyyy")

                RefreshDebugHistory()

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub DebugHistory_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub




    Private Sub cmdTogglePaging_Click(sender As Object, e As System.EventArgs) Handles cmdTogglePaging.Click
        Try
            TogglePaging()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        Try
            'go back to starting page index position
            dgDetailGrid.CurrentPageIndex = 0

            RefreshDebugHistory()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlApplications_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlApplications.SelectedIndexChanged
        Try
            RefreshDebugHistory()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlDisplayCount_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDisplayCount.SelectedIndexChanged
        Try
            RefreshDebugHistory()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub dgDetailGrid_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDetailGrid.PageIndexChanged
        Try
            SetDetailDataGridCurrentPageIndex(e.NewPageIndex)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dgDetailGrid_SortCommand(source As Object, e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgDetailGrid.SortCommand
        Try
            SortDetailData(e.SortExpression)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub



#End Region

#Region "Methods"

    Private Sub EnableControls()
        Try
            Master.Secure(cmdRefresh)
            Master.Secure(cmdTogglePaging)
            Master.Secure(cmdExport)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadApplicationsList()
        Try
            With ddlApplications
                .DataSource = DA.GetDataSet("SELECT Description, ApplicationID FROM tblApplications")
                .DataTextField = "Description"
                .DataValueField = "ApplicationID"
                .DataBind()
                .Items.Insert(0, "ALL")
                .SelectedIndex = 0
            End With

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub RefreshDebugHistory()
        Dim ds As DataSet
        Try
            ds = GetDebugHistoryDataSet()
            If (DA.IsDSEmpty(ds)) Then
                Master.Msg = "No data found"
                dgDetailGrid.DataSource = Nothing
                dgDetailGrid.DataBind()
            Else
                Master.Msg = ds.Tables(0).Rows.Count & " record(s) found." & "<BR>"

                dgDetailGrid.DataSource = ds
                dgDetailGrid.DataBind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function GetDebugHistoryDataSet() As DataSet
        Dim ds As DataSet = Nothing
        Dim sql As String = ""
        Try

            sql = "SELECT "

            'Update the SQL string according to selection of how many rows to display
            If ddlDisplayCount.SelectedIndex >= 0 Then
                sql += " TOP " & ddlDisplayCount.SelectedItem.Value
            End If
            'Build reminder of SQL 
            sql += " [DateTime] AS DT1, ApplicationID, CONVERT(varchar, [DateTime], 101) + ' ' + CONVERT(varchar, [DateTime], 8) AS [DateTime]," & _
                " UserName, ScreenName, ActionPerformed, UserHostAddress, REPLACE([Description], CHAR(13) + CHAR(10), '<BR>') [Description]" & _
                " FROM [dbo].[tblDebug]"

            sql += " WHERE ([DateTime] BETWEEN '" & Me.txtFromDate.Text & " 00:00:00' AND '" & Me.txtToDate.Text & " 23:59:59' ) "

            'Filter the data based on the application selected
            If Me.ddlApplications.SelectedItem.Value.CompareTo("ALL") <> 0 Then
                sql += " AND ApplicationID='" & ddlApplications.SelectedItem.Value & "'"
            End If

            If (hidSortField.Value.Length = 0) Then
                hidSortField.Value = "DT1 DESC"
            End If
            sql += " ORDER BY " & hidSortField.Value

            ds = DA.GetDataSet(sql)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return ds

    End Function

    Private Sub TogglePaging()
        Try
            If dgDetailGrid.AllowPaging Then
                cmdTogglePaging.Text = "Paging"
                dgDetailGrid.AllowPaging = False
            Else
                cmdTogglePaging.Text = "Show All"
                dgDetailGrid.AllowPaging = True
            End If

            RefreshDebugHistory()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SetDetailDataGridCurrentPageIndex(newPageIndex As Integer)
        Try
            dgDetailGrid.CurrentPageIndex = newPageIndex
            RefreshDebugHistory()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SortDetailData(sortExpression As String)
        Try
            Dim sortType As String

            'replace DateTime with DT1
            If (sortExpression.IndexOf("DateTime") >= 0) Then
                sortExpression = "DT1"
            End If

            If (hidSortField.Value.IndexOf("DESC") >= 0) Then
                sortType = ""
            Else
                sortType = " DESC"
            End If

            hidSortField.Value = sortExpression & sortType

            'go back to starting page index position
            dgDetailGrid.CurrentPageIndex = 0

            RefreshDebugHistory()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region


End Class