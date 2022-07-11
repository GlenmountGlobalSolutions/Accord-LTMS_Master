Public Class ShiftConfiguration
    Inherits System.Web.UI.Page

#Region "Globals"

    Private dsTable As New DataSet
    Private sql As New ArrayList
    Private log As New ArrayList
    Private selectedIDs As New ArrayList
    Private searchTextChanged As Boolean
    Private Const TABLE As String = "tblPTShifts"
    Private Const ORDERBYCOL As String = "ShiftNumber"
    Private Const NEWWINDOW As String = "NewShift_.aspx"
    Private Const ID_KEY As String = "ShiftID"
    Private Const COLUMN_KEY As String = "Column"
    Private Const DATATYPE_KEY As String = "DataType"
    Private Const ITEM As String = "Production Tracking Shift"

#End Region

#Region "Web Events"

    Private Sub ShiftConfiguration_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            searchTextChanged = False
            If (Not IsPostBack) Then
                LoadDG(False)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ShiftConfiguration_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
            'set selected page
            '            Me.tbPageNum.Text = (Me.dgMain.CurrentPageIndex + 1).ToString()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Function BindTB(ByRef sender As Object, ByVal colName As String, ByVal colNum As Int32, ByVal IDcolName As String, Optional ByVal dataType As String = "nvarchar") As String
        Try
            Dim txt As String = ""
            Dim value As String = ""

            Dim dgItem As DataGridItem = CType(sender, DataGridItem)                                     'getting ref to datagrid item
            Dim wtb As GGS.WebInputBox = CType(dgItem.Cells(colNum).Controls(1), GGS.WebInputBox)        'getting ref to control
            Dim drv As System.Data.DataRowView = CType(dgItem.DataItem, System.Data.DataRowView)

            'get control data value
            If (drv.Item(colName) Is System.DBNull.Value) Then
                txt = ""
            Else
                txt = drv.Item(colName).ToString
            End If

            'set textbox fields
            wtb.Text = txt
            wtb.OldText = txt

            value = BizLayer.SetKeyValue(value, ID_KEY, drv.Item(IDcolName).ToString())
            value = BizLayer.SetKeyValue(value, COLUMN_KEY, colName)
            value = BizLayer.SetKeyValue(value, DATATYPE_KEY, dataType)

            wtb.SQLText = value

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return ""
    End Function

#End Region

#Region "User Events"

    Private Sub cmdCopy_Click(sender As Object, e As System.EventArgs) Handles cmdCopy.Click
        Try
            Dim id As String = ""
            Dim x As Integer = 0

            If (selectedIDs.Count <= 0) Then
                Master.Msg = "Error: please select at least one (1)  " & ITEM & " to copy."
            Else
                For x = 0 To (selectedIDs.Count - 1)
                    id = selectedIDs(x).ToString()

                    'build insert select statement
                    Dim sql As String = BuildCopySqlString(id, "int")

                    'execute sql
                    DA.ExecSQL(sql)

                    'log event
                    Master.tMsg("Copy", "A copy of a  " & ITEM & " with ID " & id & " was created.")
                Next

                'Me.dgMain.CurrentPageIndex = dgMain.PageCount() - 1
                LoadDG()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        Try
            Dim id As String = ""
            Dim x As Integer = 0

            If (selectedIDs.Count <= 0) Then
                Master.Msg = "Error: please select at least one (1)  " & ITEM & " to delete."
            Else
                For x = 0 To (selectedIDs.Count - 1)
                    id = selectedIDs(x).ToString()

                    Dim sql As String = ""
                    sql = "DELETE FROM " & TABLE & " WHERE (" & ID_KEY & " = " & id & ")"

                    'execute sql
                    DA.ExecSQL(sql)

                    'log event
                    Master.tMsg("Delete", " " & ITEM & " with ID " & id & " was deleted.")
                Next

                LoadDG()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim str = ""
            Dim sql As String = ""
            Dim description As String

            'store description
            description = Me.tbDesc.Text.ToString()

            'build sql string
            sql = ""
            sql += "INSERT INTO tblPTShifts (ShiftNumber, Description, ShiftStartTime, ShiftEndTime) "
            sql += "VALUES ("            '(1, '1', '1', '1')
            sql += Me.tbShiftNumber.Text
            sql += ", '"
            sql += Me.tbDesc.Text
            sql += "', '"
            sql += Me.tbStart.Text
            sql += "', '"
            sql += Me.tbEnd.Text
            sql += "')"

            DA.ExecSQL(sql)

            Master.tMsg("New", "New  " & ITEM & " with Description " & description & " was created.")

            're-load grid
            LoadDG()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim s As String = ""
            Dim i As Integer = 0

            If (sql.Count <= 0) Then
                Master.Msg = "Not Saved: values have not changed."
            Else
                For i = 0 To (sql.Count - 1)
                    s = sql(i).ToString()
                    'execute sql
                    DA.ExecSQL(s)
                Next

                'log messages
                For i = 0 To (log.Count - 1)
                    s = log(i).ToString()
                    Master.tMsg("Save", s)
                Next

                LoadDG()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub CB_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim cb As CheckBox = CType(sender, CheckBox)             'getting ref to control

            If cb.Checked Then
                selectedIDs.Add(cb.Text)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub Textbox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            Dim id, oldVal, newVal, colName, dataType As String

            'extract values
            id = BizLayer.GetKeyValue(wtb.SQLText, ID_KEY)
            colName = BizLayer.GetKeyValue(wtb.SQLText, COLUMN_KEY)
            dataType = BizLayer.GetKeyValue(wtb.SQLText, DATATYPE_KEY)

            oldVal = wtb.OldText
            newVal = wtb.Text

            'save and log event
            sql.Add(BuildSqlString(colName, newVal, id, dataType))
            log.Add(ITEM & " with ID " & id & " saved. " & colName & " old value: " & oldVal & ", new value: " & newVal & ".<br>")

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Subs and Functions"

    Private Sub LoadDG(Optional ByVal limitSearch As Boolean = True)
        dsTable.Reset()
        Dim likeStr As String = ""
        dsTable = DA.GetDataSet("SELECT * FROM " & TABLE & " ORDER BY " & ORDERBYCOL)
        Me.dgMain.DataSource = dsTable
        Me.dgMain.DataBind()
    End Sub

    Private Sub EnableControls()
        Master.Secure(cmdNew)
        Master.Secure(cmdSave)
        Master.Secure(cmdDelete)
        Master.Secure(cmdCopy)
    End Sub

    Private Function BuildSqlString(ByVal column As String, ByVal val As String, ByVal ID As String, ByVal dataType As String) As String
        Dim sql As String = ""

        Select Case dataType.ToLower()
            Case "nvarchar"
                'this will result in "... = N'value' " string (N + quotes around value)
                sql = "UPDATE " & TABLE & " SET " & column & " = N'" & val & "' WHERE (" & ID_KEY & " = " & ID & ")"

            Case "int"
                'this will result in "... = value " string (no quotes around value)
                sql = "UPDATE " & TABLE & " SET " & column & " = " & val & " WHERE (" & ID_KEY & " = " & ID & ")"

            Case "bit"
                'convert true / false strings to 1, 0, or null
                If (val.ToUpper() = "TRUE") Then
                    val = "1"

                ElseIf (val.ToUpper() = "FALSE") Then
                    val = "0"

                ElseIf (val = "1") Then
                    'already formatted so do nothing

                ElseIf (val = "0") Then
                    'already formatted so do nothing

                Else
                    'insert null
                    val = "NULL"
                End If

                sql = "UPDATE " & TABLE & " SET " & column & " = " & val & " WHERE (" & ID_KEY & " = " & ID & ")"

            Case Else
                'default case - assume "varchar" type. 'this will result in "... = 'value' " string (quotes around value)
                sql = "UPDATE " & TABLE & " SET " & column & " = '" & val & "' WHERE (" & ID_KEY & " = " & ID & ")"

        End Select

        Return sql
    End Function

    Private Function BuildCopySqlString(ByVal id As String, Optional ByVal dataType As String = "nvarchar") As String
        'Dim sql As String = ""
        Dim sql As String = ""

        'insert part
        sql += "INSERT INTO tblPTShifts"
        sql += "(ShiftNumber, Description, ShiftStartTime, ShiftEndTime, Hour1StartTime, Hour1EndTime, Hour1PlannedProduction, Hour2StartTime, Hour2EndTime, "
        sql += "Hour2PlannedProduction, Hour3StartTime, Hour3EndTime, Hour3PlannedProduction, Hour4StartTime, Hour4EndTime, Hour4PlannedProduction, "
        sql += "Hour5StartTime, Hour5EndTime, Hour5PlannedProduction, Hour6StartTime, Hour6EndTime, Hour6PlannedProduction, Hour7StartTime, Hour7EndTime, "
        sql += "Hour7PlannedProduction, Hour8StartTime, Hour8EndTime, Hour8PlannedProduction, Hour9StartTime, Hour9EndTime, Hour9PlannedProduction) "

        'selece part
        sql += "SELECT 1+ShiftNumber, Description, ShiftStartTime, ShiftEndTime, Hour1StartTime, Hour1EndTime, Hour1PlannedProduction, Hour2StartTime, Hour2EndTime, "
        sql += "Hour2PlannedProduction, Hour3StartTime, Hour3EndTime, Hour3PlannedProduction, Hour4StartTime, Hour4EndTime, Hour4PlannedProduction, "
        sql += "Hour5StartTime, Hour5EndTime, Hour5PlannedProduction, Hour6StartTime, Hour6EndTime, Hour6PlannedProduction, Hour7StartTime, Hour7EndTime, "
        sql += "Hour7PlannedProduction, Hour8StartTime, Hour8EndTime, Hour8PlannedProduction, Hour9StartTime, Hour9EndTime, Hour9PlannedProduction "
        sql += "FROM tblPTShifts "
        sql += "WHERE (ShiftID = "
        If (dataType.ToLower() = "int") Then
            sql += id & ")"
        ElseIf (dataType.ToLower() = "nvarchar") Then
            sql += "'" & id & "')"
        Else          'if data type is unknows use "nvarchar" type with quotes around id.
            sql += "'" & id & "')"
        End If

        Return sql
    End Function

#End Region

End Class