Public Class ReworkHistory
    Inherits System.Web.UI.Page


    Private sql As New List(Of String)
    Private log As New List(Of String)
    Private selectedIDs As New List(Of String)
    Private Const ID_KEY As String = "ReworkDate"
    Private Const COLUMN_KEY As String = "Column"
    Private Const DATATYPE_KEY As String = "DataType"

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not IsPostBack) Then
            LoadDG()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'Put user code to initialize the page here
        Try
            EnableControls()
            'set selected page
            Me.tbPageNum.Text = (Me.dgMain.CurrentPageIndex + 1).ToString()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub imbLastPage_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbLastPage.Click
        Try
            If (dgMain.PageCount <= 0) Then
                Me.tbPageNum.Text = "1"
            Else

                Me.dgMain.CurrentPageIndex = (dgMain.PageCount - 1)
                Me.tbPageNum.Text = (Me.dgMain.CurrentPageIndex + 1).ToString()

                LoadDG()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub imbFirstPage_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbFirstPage.Click
        Try
            Me.dgMain.CurrentPageIndex = 0
            Me.tbPageNum.Text = "1"
            LoadDG()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub imgGoToPage_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGoToPage.Click
        Try
            If (dgMain.PageCount <= 0) Then
                Me.tbPageNum.Text = "1"
            ElseIf (Not IsNumeric(Me.tbPageNum.Text)) Then
                Master.Msg = "Please enter a Number in page number field."
            Else

                Dim i As Int32 = Convert.ToInt32(Me.tbPageNum.Text)
                If (i <= 0) Then
                    Me.dgMain.CurrentPageIndex = 0
                ElseIf (i > Me.dgMain.PageCount) Then
                    Me.dgMain.CurrentPageIndex = (dgMain.PageCount - 1)
                Else
                    Me.dgMain.CurrentPageIndex = (i - 1)
                End If

                Me.tbPageNum.Text = (Me.dgMain.CurrentPageIndex + 1).ToString()
                LoadDG()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub imbPrevPage_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbPrevPage.Click
        Try
            If (dgMain.PageCount <= 0) Then
                Me.tbPageNum.Text = "1"
            Else

                Dim i As Int32 = Me.dgMain.CurrentPageIndex
                If (i <= 0) Then
                    Me.dgMain.CurrentPageIndex = 0
                Else : Me.dgMain.CurrentPageIndex = (i - 1)
                End If

                Me.tbPageNum.Text = (Me.dgMain.CurrentPageIndex + 1).ToString()
                LoadDG()

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub imbNextPage_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbNextPage.Click
        Try
            If (dgMain.PageCount <= 0) Then
                Me.tbPageNum.Text = "1"
            Else

                Dim i As Int32 = Me.dgMain.CurrentPageIndex
                If (i >= (Me.dgMain.PageCount - 1)) Then
                    Me.dgMain.CurrentPageIndex = (dgMain.PageCount - 1)
                Else : Me.dgMain.CurrentPageIndex = (i + 1)
                End If

                Me.tbPageNum.Text = (Me.dgMain.CurrentPageIndex + 1).ToString()
                LoadDG()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub dgMain_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgMain.PageIndexChanged
        Try
            ' Handles dgDetailGrid.PageIndexChanged
            Me.dgMain.CurrentPageIndex = e.NewPageIndex

            LoadDG()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally

        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim s As String = ""
            Dim i As Integer = 0

            If (sql.Count <= 0) Then
                Master.Msg = "Not Saved: values have not changed."
            Else

                For Each stmt As String In sql
                    DA.ExecSQL(stmt)
                Next

                'log messages
                s = String.Join("<br />", log)
                Master.tMsg("Save", s.ToString())

                sql.Clear()
                log.Clear()

                LoadDG()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim id As String = ""
            Dim x As Integer = 0
            Dim IDstartDT As String = ""
            Dim IDendDT As String = ""
            Dim deleteLog As New List(Of String)

            If (selectedIDs.Count <= 0) Then
                Master.Msg = "Error: please select at least one (1) record to delete."
            Else

                For x = 0 To (selectedIDs.Count - 1)
                    id = selectedIDs(x).ToString()
                    IDstartDT = (id.Substring(0, id.Length - 3) + ".000" + id.Substring(id.Length - 3, 3))
                    IDendDT = (id.Substring(0, id.Length - 3) + ".999" + id.Substring(id.Length - 3, 3))

                    'NOTE: pb, 03/04/05. had to change WHERE statement from "=" to "LIKE" 
                    'because i could not get the EXACT datetime. so right now the resolution is 1 second...
                    Dim sql As String = "DELETE FROM tblRMDReworkHistory WHERE (ReworkDate >= CONVERT(DATETIME, '" + IDstartDT + "', 102))"
                    sql += " AND (ReworkDate < CONVERT(DATETIME, '" + IDendDT + "', 102))"

                    'execute sql
                    DA.ExecSQL(sql)

                    'log event
                    deleteLog.Add("Rework History with Datetime " & id & " was deleted.")
                Next

                selectedIDs.Clear()
                Master.tMsg("Save", String.Join("<br />", deleteLog))
                LoadDG()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"
    Public Function BindTB(ByRef sender As Object, ByVal colName As String, ByVal colNum As Int32, ByVal IDcolName As String, Optional ByVal dataType As String = "nvarchar") As String
        Try

            Dim dgItem As DataGridItem = CType(sender, DataGridItem)
            Dim drv As System.Data.DataRowView = CType(dgItem.DataItem, DataRowView)

            'getting ref to control
            Dim wtb As GGS.WebInputBox = CType(dgItem.Cells(colNum).Controls(1), GGS.WebInputBox)

            Dim txt As String = ""
            Dim value As String = ""


            'get control data value
            If (drv.Item(colName) Is System.DBNull.Value) Then
                txt = ""
            Else
                txt = drv.Item(colName).ToString()
            End If

            'set textbox fields
            wtb.Text = txt
            wtb.OldText = txt

            value = BizLayer.SetKeyValue(value, ID_KEY, drv.Item(IDcolName).ToString())
            value = BizLayer.SetKeyValue(value, COLUMN_KEY, colName)
            value = BizLayer.SetKeyValue(value, DATATYPE_KEY, dataType)

            wtb.SQLText = value

            Return ""

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return ""
        End Try
    End Function

    Public Sub Textbox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim id, oldVal, newVal, colName, dataType As String

            'getting ref to control
            Dim wtb As GGS.WebInputBox
            wtb = CType(sender, GGS.WebInputBox)

            'extract values
            id = BizLayer.GetKeyValue(wtb.SQLText, ID_KEY)
            colName = BizLayer.GetKeyValue(wtb.SQLText, COLUMN_KEY)
            dataType = BizLayer.GetKeyValue(wtb.SQLText, DATATYPE_KEY)

            oldVal = wtb.OldText
            newVal = wtb.Text

            'save and log event
            sql.Add(BuildSqlString(colName, newVal, id, dataType))
            log.Add("Rework History Record with Datetime " & id & " saved. " & colName & " old value: " & oldVal & ", new value: " & newVal & ".")

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function BuildSqlString(ByVal column As String, ByVal val As String, ByVal ID As String, ByVal dataType As String) As String
        Dim sql As String = ""
        Dim IDstartDT As String = (ID.Substring(0, ID.Length - 3) + ".000" + ID.Substring(ID.Length - 3, 3))
        Dim IDendDT As String = (ID.Substring(0, ID.Length - 3) + ".999" + ID.Substring(ID.Length - 3, 3))
        Select Case dataType.ToLower()
            'NOTE: pb, 03/04/05. had to change WHERE statement from "=" to "LIKE" 
            'because i could not get the EXACT datetime. so right now the resolution is 1 second...

            Case "nvarchar"        'this will result in "... = N'value' " string (N + quotes around value)
                sql = "UPDATE tblRMDReworkHistory SET " & column & " = N'" & val & "' WHERE (ReworkDate >= CONVERT(DATETIME, '" + IDstartDT + "', 102))"
                sql += " AND (ReworkDate < CONVERT(DATETIME, '" + IDendDT + "', 102))"

            Case "int"            'this will result in "... = value " string (no quotes around value)
                sql = "UPDATE tblRMDReworkHistory SET " & column & " = " & val & " WHERE (ReworkDate >= CONVERT(DATETIME, '" + IDstartDT + "', 102))"
                sql += " AND (ReworkDate < CONVERT(DATETIME, '" + IDendDT + "', 102))"

            Case "bit"
                'convert true / false strings to 1, 0, or null
                If (val.ToUpper() = "TRUE") Then
                    val = "1"
                ElseIf (val.ToUpper() = "FALSE") Then
                    val = "0"
                ElseIf (val = "1") Then               'already formatted so do nothing
                ElseIf (val = "0") Then             'already formatted so do nothing
                Else                'insert null
                    val = "NULL"
                End If

                sql = "UPDATE tblRMDReworkHistory SET " & column & " = " & val & " WHERE (ReworkDate >= CONVERT(DATETIME, '" + IDstartDT + "', 102))"
                sql += " AND (ReworkDate < CONVERT(DATETIME, '" + IDendDT + "', 102))"

            Case Else            'default case - assume "varchar" type. 'this will result in "... = 'value' " string (quotes around value)
                sql = "UPDATE tblRMDReworkHistory SET " & column & " = '" & val & "' WHERE (ReworkDate >= CONVERT(DATETIME, '" + IDstartDT + "', 102))"
                sql += " AND (ReworkDate < CONVERT(DATETIME, '" + IDendDT + "', 102))"

        End Select


        Return sql
    End Function

    Public Sub CB_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim cb As CheckBox
            cb = CType(sender, CheckBox)             'getting ref to control

            selectedIDs.Add(cb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub LoadDG()
        Try
            Dim ds As DataSet
            Dim sql As String = ""

            Me.dgMain.Controls.Clear()

            sql += " SELECT ReworkDate, ShiftStartDate, Shift, ReworkArea, TeamMember, LotNumber, "
            sql += " VehicleLine, SeatStyle, Color, Component, SeatSerialNumber, Category, ReworkReason, "
            sql += " ChargeTo, ReworkAction, ReturnedQuantity, Location"
            sql += " FROM tblRMDReworkHistory"
            sql += " ORDER BY ReworkDate DESC"

            ds = DA.GetDataSet(sql)
            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                Me.dgMain.DataSource = ds
                Me.dgMain.DataBind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdDelete)
            Master.Secure(Me.cmdSave)

            If (Me.dgMain.Items.Count <= 0) Then
                Me.cmdDelete.Enabled = False
                Me.cmdSave.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region

End Class