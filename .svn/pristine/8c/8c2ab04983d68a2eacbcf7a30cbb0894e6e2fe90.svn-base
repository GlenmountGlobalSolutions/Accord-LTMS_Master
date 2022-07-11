Public Class PrinterConfiguration
    Inherits System.Web.UI.Page

    Enum tblLabelPrinters
        Name
        NetworkPath
        tblLabelPrintersID
        LabelPrinter
    End Enum

    Dim errflag As New ArrayList
    Dim SQL As New ArrayList
    Dim ctrlDes As New ArrayList
    Dim ctrlID As New ArrayList
    Dim NewText As New ArrayList
    Dim OldText As New ArrayList


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                GetPrinterData()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub PrinterConfiguration_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub lbPrinters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbPrinters.SelectedIndexChanged
        Try
            LoadData()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tPrinterName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tPrinterName.TextChanged
        Try
            ctrlDes.Add("Printer Name")          'this is id that uniquly identifies what contorols has fired
            SQL.Add("Update tblLabelPrinters set Name='" & tPrinterName.Text & "' where   tblLabelPrintersID =" & lbPrinters.SelectedItem.Value)
            errflag.Add(0)
            ctrlID.Add("1")
            NewText.Add(tPrinterName.Text)
            OldText.Add(tPrinterName.OldText)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tPrinterPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tPrinterPath.TextChanged
        Try
            ctrlDes.Add("Printer Path")          'this is id that uniquly identifies what contorols has fired
            SQL.Add("Update tblLabelPrinters set NetworkPath='" & tPrinterPath.Text & "' where   tblLabelPrintersID =" & lbPrinters.SelectedItem.Value)
            errflag.Add(0)
            ctrlID.Add("2")
            NewText.Add(tPrinterPath.Text)
            OldText.Add(tPrinterPath.OldText)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub xLabelPrinter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles xLabelPrinter.CheckedChanged
        Try
            Dim strql As String
            If (xLabelPrinter.Checked = True) Then
                strql = "1"
            Else
                strql = "0"
            End If

            ctrlDes.Add("Label Printer")             'this is id that uniquly identifies what contorols has fired

            SQL.Add("Update tblLabelPrinters set LabelPrinter='" & strql & "' where   tblLabelPrintersID =" & lbPrinters.SelectedItem.Value)
            errflag.Add(0)
            ctrlID.Add("3")
            NewText.Add(xLabelPrinter.Checked.ToString())
            OldText.Add((Not xLabelPrinter.Checked).ToString())

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim ds As DataSet
            Dim retval As String = ""
            Dim strIdentity As String = ""

            ds = DA.GetDataSet("Begin Insert into tblLabelPrinters (NetworkPath,Name) values ('" & txtPrinterPath.Text & "','" & txtPrinterName.Text & "');Select @@IDENTITY END")
            If (DA.IsDSEmpty(ds)) Then  'action failed
                Master.Msg = "Error: Unable to create printer.<br>Please make sure that printer with specified name does not already exist."
            Else
                strIdentity = ds.Tables(0).Rows(0).Item(0).ToString()
                GetPrinterData()
                lbPrinters.SelectedIndex = lbPrinters.Items.IndexOf(lbPrinters.Items.FindByValue(strIdentity))
                LoadData()

                Master.tMsg("New", "New printer: " & txtPrinterName.Text & " is added!")

            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim x As Int32
        Dim strIndex As String
        Try
            If (SQL.Count = 0) Then
                Master.Msg = "Not saved: values have not changed"
            Else

                For x = 0 To SQL.Count - 1
                    DA.ExecSQL(SQL(x).ToString())
                    Master.tMsg("Save", ctrlDes(x).ToString() & " is changed from: " & OldText(x).ToString() & " to: " & NewText(x).ToString())
                    Select Case ctrlID(x).ToString()
                        Case "1"
                            Image1.Visible = True
                        Case "2"
                            Image2.Visible = True
                        Case "3"
                            Image3.Visible = True
                    End Select
                Next

                strIndex = lbPrinters.SelectedItem.Value
                lbPrinters.SelectedIndex = lbPrinters.Items.IndexOf(lbPrinters.Items.FindByValue(strIndex))

                GetPrinterData()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            DA.ExecSQL("Delete from   tblLabelPrinters where   tblLabelPrintersID=" & lbPrinters.SelectedItem.Value)
            Master.tMsg("Delete", "Printer " & lbPrinters.SelectedItem.Text & " is deleted")
            GetPrinterData()
            ResetInputs()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"""

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdDelete)
            Master.Secure(Me.cmdNew)
            Master.Secure(Me.cmdSave)

            If lbPrinters.SelectedItem Is Nothing Then
                cmdSave.Enabled = False
                cmdDelete.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub GetPrinterData()
        Dim ds As DataSet
        Try
            ds = DA.GetDataSet("SELECT [Name],[NetworkPath],[tblLabelPrintersID],[LabelPrinter] FROM tblLabelPrinters")

            dgLabelPrinters.DataSource = ds
            dgLabelPrinters.DataBind()

            lbPrinters.DataSource = ds
            lbPrinters.DataBind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadData()
        Dim ds As DataSet
        Dim bChecked As Boolean = False

        Try
            ds = DA.GetDataSet("SELECT [Name],[NetworkPath],[tblLabelPrintersID],[LabelPrinter] FROM tblLabelPrinters WHERE tblLabelPrintersID=" & lbPrinters.SelectedItem.Value)

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                With ds.Tables(0).Rows(0)

                    tPrinterName.Text = .Item(tblLabelPrinters.Name).ToString()
                    tPrinterName.OldText = tPrinterName.Text

                    tPrinterPath.Text = .Item(tblLabelPrinters.NetworkPath).ToString()
                    tPrinterPath.OldText = tPrinterPath.Text
                    If (.Item("LabelPrinter") Is System.DBNull.Value) Then
                        xLabelPrinter.Checked = False
                        xLabelPrinter.OldText = "0"
                    Else
                        Boolean.TryParse(.Item(tblLabelPrinters.LabelPrinter).ToString(), bChecked)
                        xLabelPrinter.Checked = bChecked
                        xLabelPrinter.OldText = bChecked.ToString()
                    End If

                End With
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ResetInputs()
        tPrinterName.Text = ""
        tPrinterPath.Text = ""
    End Sub


#End Region


End Class