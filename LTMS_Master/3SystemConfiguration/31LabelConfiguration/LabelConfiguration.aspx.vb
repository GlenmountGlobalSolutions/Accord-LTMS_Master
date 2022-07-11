Public Class LabelConfiguration
    Inherits System.Web.UI.Page

    Dim fError As Boolean
    Public OldText As New ArrayList         'old value of ctrl
    Public NewText As New ArrayList         'new value of ctrl
    Public InputCaption As New ArrayList    'caption asscoiated with ctrl
    Public CtrlIndex As New ArrayList       'index of ctrl

#Region "Event Handlers"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                LoadPageData()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Public Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbSeatStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSeatStyle.SelectedIndexChanged
        Try
            LoadStyleData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Dim strInsert As String
        Dim m As String
        Dim newStyle As String = ddProductStyle.SelectedItem.Value.ToUpper
        Try
            strInsert = "Insert into tblPrintSerialNumberParameters (STYLE_CODE,MODIFIED_DT,RECORDED_BY)" _
                & "Values ('" & newStyle & "'," _
                & "getdate(),'" & Page.User.Identity.Name.ToString() & "')"

            DA.ExecSQL(strInsert)

            'reload all lists and tables
            LoadPageData()
            'preselect the new style and load the parameters.
            lbSeatStyle.SelectedIndex = lbSeatStyle.Items.IndexOf(lbSeatStyle.Items.FindByValue(newStyle))
            LoadStyleData()

            m = "Style code " & newStyle & " is added successfully!"
            Master.tMsg("New", m)

        Catch x As Exception When Err.Number = 5
            Master.Msg = "Error: Seat style. " & newStyle & " already exists! Failed to add new seat style."
        Catch x As Exception
            Master.eMsg(x.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try

            If tNextProdNumber_Validation() And tComment_Validation() And tSeatPartNumber_Validation() Then

                If tSeatPartNumber.OldText <> tSeatPartNumber.Text Or tNextProdNumber.OldText <> tNextProdNumber.Text Or tComment.OldText <> tComment.Text Then
                    If CheckPrintParameters() = True Then
                        ModifyPrintParameters() 'Update data record in database
                    Else
                        InsertPrintParameters() 'Insert new data record in database
                    End If

                    'Show check mark next to items that were updated.
                    Dim y As Integer
                    For y = 0 To CtrlIndex.Count - 1
                        Select Case CtrlIndex(y).ToString
                            Case "2"
                                Image2.Visible = True
                            Case "4"
                                Image4.Visible = True
                            Case "5"
                                Image5.Visible = True
                        End Select
                    Next

                    'log actions

                    Master.tMsg("Save", "SeatStyle Code: " & lbSeatStyle.SelectedItem.Value & " " & InputCaption(0).ToString() & " (" & OldText(0).ToString() & ")(" & NewText(0).ToString() & ")")
                    Master.tMsg("Save", "Parameters for style code " & lbSeatStyle.SelectedItem.Value.ToUpper & " are saved!")

                    'reload the webpage
                    LoadStyleData()
                    PrintSerialNumberParameters()
                Else
                    Master.Msg = "Save cancelled: changes have not been made!"
                End If

            Else
                Master.Msg = "Save cancelled: please fix the errors below!"
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Master.Msg = "Save Failed! "
            LoadStyleData()
        Finally
        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try

            DA.ExecSQL("Delete From tblPrintSerialNumberParameters Where STYLE_CODE = '" & lbSeatStyle.SelectedItem.Value & "'")
            Master.tMsg("Delete", "Style code " & lbSeatStyle.SelectedItem.Text.ToUpper & " is deleted.")

            LoadPageData()
            ResetInputs()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
#End Region

#Region "Methods"
    Private Function tNextProdNumber_Validation() As Boolean
        Dim bResult As Boolean = False

        Try
            If tNextProdNumber.Text.Length > 7 Then
                fError = True
                lblNextProdNumber.Text = "Error: Input must be less then 8 chars."
                lblNextProdNumber.Visible = True
                bResult = False
            Else

                OldText.Add(tNextProdNumber.OldText)
                NewText.Add(tNextProdNumber.Text)
                InputCaption.Add("Next Production Number")
                CtrlIndex.Add(2)

                fError = False
                lblNextProdNumber.Text = ""
                lblNextProdNumber.Visible = False
                bResult = True
            End If
        Catch ex As Exception
            fError = True
            lblNextProdNumber.Text = "Error: Value must be alphanumeric and less then 7 chars."
            lblNextProdNumber.Visible = True
            Master.eMsg(ex.ToString())
            bResult = False
        End Try
        Return bResult
    End Function

    Private Function tSeatPartNumber_Validation() As Boolean
        Try
            If tSeatPartNumber.Text.Length <= 14 Then
                CtrlIndex.Add(5)
                OldText.Add(tSeatPartNumber.OldText)
                NewText.Add(tSeatPartNumber.Text)
                InputCaption.Add("Seat Part Number")

                fError = False
                lblSeatPartNumber.Text = ""
                lblSeatPartNumber.Visible = False
                Return True
            Else
                fError = True
                lblSeatPartNumber.Text = "Error: Input must be less then 14 chars"
                lblSeatPartNumber.Visible = True
                Return False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Function tComment_Validation() As Boolean
        Try
            If tComment.Text.Length <= 30 Then
                OldText.Add(tComment.OldText)
                NewText.Add(tComment.Text)
                InputCaption.Add("Comment")
                CtrlIndex.Add(4)

                fError = False
                lblComment.Text = ""
                lblComment.Visible = False
                Return True
            Else
                fError = True
                lblComment.Text = "Error: Input must be less then 30 chars"
                lblComment.Visible = True
                Return False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Function CheckPrintParameters() As Boolean
        Dim str As String
        str = "Select STYLE_CODE From tblPrintSerialNumberParameters " & _
        "Where STYLE_CODE='" & lbSeatStyle.SelectedItem.Value & "'"

        Dim ds As DataSet

        ds = DA.GetDataSet(str)
        If (ds.Tables(0).Rows.Count = 0) Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub ModifyPrintParameters()
        Try
            Dim strUpdate As String

            strUpdate = "Update tblPrintSerialNumberParameters Set " & _
                "MODIFIED_DT=getdate()," & _
                "RECORDED_BY='" & Page.User.Identity.Name.ToString() & "'," & _
                "LABEL_SEAT_PART_NUMBER='" & tSeatPartNumber.Text.ToUpper & "'," & _
                "NEXT_PRODUCTION_NUMBER='" & tNextProdNumber.Text.ToUpper & "'," & _
                "LABEL_HUMAN_READABLE='" & tComment.Text.ToUpper & "' " & _
                "Where STYLE_CODE ='" & lbSeatStyle.SelectedItem.Value.ToUpper & "'"
            DA.ExecSQL(strUpdate)

            Master.tMsg("Save", strUpdate)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Throw ex
        End Try
    End Sub

    Private Sub InsertPrintParameters()
        Try
            Dim strInsert As String
            strInsert = "Insert into tblPrintSerialNumberParameters " & _
              "(STYLE_CODE,MODIFIED_DT,RECORDED_BY,NEXT_PRODUCTION_NUMBER,LABEL_SEAT_PART_NUMBER,LABEL_SEAT_CODE,LABEL_HUMAN_READABLE,LABEL_SEAT_PART_NUMBER) Values ('" & _
              UCase(Trim(lbSeatStyle.SelectedItem.Value)) & "'," & _
               "getdate(),'" & _
               Page.User.Identity.Name.ToString() & "','" & _
               UCase(Trim(Request("tNextProdNumber"))) & "','" & _
               UCase(Trim(Request("txtSeatCodeLabel"))) & "','" & _
               UCase(Trim(Request("tComment"))) & "',')" & _
               UCase(Trim(Request("tSeatPartNumber"))) & "')"

            DA.ExecSQL(strInsert)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Throw ex
        End Try
    End Sub

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdDelete)
            Master.Secure(Me.cmdNew)
            Master.Secure(Me.cmdSave)

            If lbSeatStyle.SelectedItem Is Nothing Then
                cmdSave.Enabled = False
                cmdDelete.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub LoadPageData()
        Try
            'populate teh list box
            LoadSeatStyles()
            'populate the datagrid
            PrintSerialNumberParameters()
            'populate the list box withing the add new dialog
            LoadProductStyles()

        Catch ex As Exception
            Master.eMsg("Failed to load page data.")
        End Try
    End Sub

    Private Sub LoadSeatStyles()
        Dim strsql As String
        strsql = "SELECT DISTINCT STYLE_CODE FROM tblPrintSerialNumberParameters"

        lbSeatStyle.DataSource = DA.GetDataSet(strsql)
        lbSeatStyle.DataTextField = "STYLE_CODE"
        lbSeatStyle.DataValueField = "STYLE_CODE"
        lbSeatStyle.DataBind()

    End Sub

    Private Sub LoadProductStyles()
        Try

            Dim str As String
            'ddProductStyle is located in the Add NEW DIALOG
            str = "SELECT DISTINCT LEFT(ProductID, '4') AS EXP1 FROM   tblProducts"
            ddProductStyle.DataSource = DA.GetDataSet(str)
            ddProductStyle.DataTextField = "EXP1"
            ddProductStyle.DataValueField = "EXP1"
            ddProductStyle.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadStyleData()

        Dim ds As DataSet
        Dim bChecked As Boolean = False

        Try

            Dim strsql As String
            strsql = "SELECT LABEL_SEAT_PART_NUMBER, MODIFIED_DT, RECORDED_BY" _
            & ", LABEL_HUMAN_READABLE, LABEL_SEAT_CODE, LABEL_SEAT_PART_NUMBER, NEXT_PRODUCTION_NUMBER" _
            & " FROM tblPrintSerialNumberParameters WHERE STYLE_CODE='" & lbSeatStyle.SelectedItem.Value & "'"

            ds = DA.GetDataSet(strsql)

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                With ds.Tables(0).Rows(0)

                    tNextProdNumber.Text = .Item("NEXT_PRODUCTION_NUMBER").ToString()
                    tNextProdNumber.OldText = .Item("NEXT_PRODUCTION_NUMBER").ToString()

                    tComment.Text = .Item("LABEL_HUMAN_READABLE").ToString()
                    tComment.OldText = .Item("LABEL_HUMAN_READABLE").ToString()

                    Me.tSeatPartNumber.Text = .Item("LABEL_SEAT_PART_NUMBER").ToString()
                    Me.tSeatPartNumber.OldText = .Item("LABEL_SEAT_PART_NUMBER").ToString()

                    lblRecordedBy.Text = .Item("RECORDED_BY").ToString()
                    lblModifyDT.Text = .Item("MODIFIED_DT").ToString()

                End With
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            ResetInputs()
        End Try

    End Sub

    Private Sub ResetInputs()
        tNextProdNumber.Text = ""
        tComment.Text = ""
        tSeatPartNumber.Text = ""
        lblRecordedBy.Text = ""
        lblModifyDT.Text = ""
    End Sub

    Private Sub PrintSerialNumberParameters()
        Try
            Dim strsql As String
            strsql = "Select LABEL_SEAT_PART_NUMBER, STYLE_CODE,MODIFIED_DT,RECORDED_BY,NEXT_PRODUCTION_NUMBER," & _
             "LABEL_SEAT_PART_NUMBER,LABEL_SEAT_CODE,LABEL_HUMAN_READABLE" & _
             " From tblPrintSerialNumberParameters Order By MODIFIED_DT desc"

            dgSeatStyle.DataSource = DA.GetDataSet(strsql)
            dgSeatStyle.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

#End Region

End Class