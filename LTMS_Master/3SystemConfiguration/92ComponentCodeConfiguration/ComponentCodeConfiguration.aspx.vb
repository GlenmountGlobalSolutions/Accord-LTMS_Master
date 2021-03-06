Public Class ComponentCodeConfiguration
    Inherits System.Web.UI.Page

#Region "global vars"

    Public NewText As New ArrayList         'new posted value of ctrl
    Public sqlText As New ArrayList         'sql to make update
    Public InputCaption As New ArrayList    'caption asscoiated with control
    Public OldText As New ArrayList         'old value of ctrl

#End Region

#Region "Page and form items events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                GetComponentCodes()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbComponentcodes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbComponentcodes.SelectedIndexChanged
        Try
            GetDataIntoCTRLs()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tComponentCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tComponentCode.TextChanged
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            sqlText.Add("Update tblSGComponentCodes Set ComponentCode='" & wtb.Text & "' where ComponentCodeID=" & lbComponentcodes.SelectedItem.Value)
            OldText.Add(wtb.OldText)
            InputCaption.Add("Component Code")
            NewText.Add(wtb.Text)

            sqlText.Add("UPDATE tblSGProductCodeConfig SET ComponentCode = '" & wtb.Text & "' WHERE ComponentCode = '" & wtb.OldText & "'")
            OldText.Add(wtb.OldText)
            InputCaption.Add("Description")
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim s As String
            Dim x As Integer
            Dim i As Int32

            For Each s In sqlText
                DA.ExecSQL(s)

                Select Case InputCaption(x).ToString
                    Case "Component Code"
                        Master.tMsg("Save", InputCaption(x).ToString() & " is changed from: " & OldText(x).ToString() & " To: " & NewText(x).ToString())
                        Image3.Visible = True
                End Select
                x = x + 1
            Next

            'pb, 01/21/05
            i = Me.lbComponentcodes.SelectedIndex
            GetComponentCodes()
            Me.lbComponentcodes.SelectedIndex = i

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            DA.ExecSQL("DELETE FROM tblSGComponentCodes where ComponentCodeID=" & lbComponentcodes.SelectedItem.Value)
            DA.ExecSQL("UPDATE tblSGProductCodeConfig SET ComponentCode = '' WHERE ComponentCode = '" & lbComponentcodes.SelectedItem.ToString() & "'")

            Master.tMsg("Delete", "Component Code " & lbComponentcodes.SelectedItem.ToString.ToUpper & " is deleted.")

            tComponentCode.Text = ""

            GetComponentCodes()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim sql As String
            Dim ds As DataSet
            Dim sCompCode As String = tNewComponentCode.Text

            sql = "SELECT ComponentCode FROM tblSGComponentCodes WHERE ComponentCode = '" & sCompCode & "' "
            ds = DA.GetDataSet(sql)

            If ds.Tables(0).Rows.Count > 0 Then
                Master.Msg = "Component Code already exists in the database, please enter a new code."
            Else
                sql = "INSERT INTO tblSGComponentCodes (ComponentCode) VALUES ('" & sCompCode & "')"
                DA.ExecSQL(sql)

                Master.tMsg("Add", "Component " & sCompCode.ToUpper() & " is added.")

                GetComponentCodes()
                lbComponentcodes.SelectedIndex = lbComponentcodes.Items.IndexOf(lbComponentcodes.Items.FindByText(sCompCode))
                GetDataIntoCTRLs()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Other Controls"

    Private Sub EnableControls()
        Master.Secure(Me.cmdNew)
        Master.Secure(Me.cmdSave)
        Master.Secure(Me.cmdDelete)

        If (lbComponentcodes.SelectedItem Is Nothing) Then
            Me.cmdSave.Enabled = False
            Me.cmdDelete.Enabled = False
        End If
    End Sub

    Private Sub GetComponentCodes()
        Try
            Dim sSQL As String

            sSQL = "SELECT cc.ComponentCodeID, cc.ComponentCode " & _
                    "FROM tblSGComponentCodes cc Order By cc.ComponentCode ASC"

            lbComponentcodes.DataSource = DA.GetDataSet(sSQL)
            lbComponentcodes.DataTextField = "ComponentCode"
            lbComponentcodes.DataValueField = "ComponentCodeID"
            lbComponentcodes.DataBind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub GetDataIntoCTRLs()
        Try
            If lbComponentcodes.SelectedItem Is Nothing Or lbComponentcodes.SelectedIndex < 0 Then
                tComponentCode.Text = ""
            Else
                tComponentCode.Text = lbComponentcodes.SelectedItem.ToString
            End If
            tComponentCode.OldText = tComponentCode.Text

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

End Class