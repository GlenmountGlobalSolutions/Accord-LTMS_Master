Public Class ComponentNameConfiguration
    Inherits System.Web.UI.Page

#Region "Globals"

    Public NewText As New ArrayList         'new posted value of ctrl
    Public sqlText As New ArrayList         'sql to make update
    Public InputCaption As New ArrayList    'caption asscoiated with control
    Public OldText As New ArrayList         'old value of ctrl

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                GetLineNumbers()
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

    Private Sub ddLineNum_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddLineNum.SelectedIndexChanged
        Try
            If (ddLineNum.SelectedIndex < 1) Then
                lbComponentNames.Items.Clear()
            Else
                GetComponentNames()
            End If

            GetDataIntoCTRLs()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbComponentNames_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbComponentNames.SelectedIndexChanged
        Try
            GetDataIntoCTRLs()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tIndex_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tIndex.TextChanged
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            sqlText.Add("UPDATE dbo.tblSGComponentNames SET PLCArrayOrder = " & wtb.Text & " WHERE ComponentNameID = " & lbComponentNames.SelectedItem.Value)
            OldText.Add(wtb.OldText)
            InputCaption.Add("Component Index")
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tName.TextChanged
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            sqlText.Add("UPDATE dbo.tblSGComponentNames SET ComponentName='" & wtb.Text & "' WHERE ComponentNameID=" & lbComponentNames.SelectedItem.Value)
            OldText.Add(wtb.OldText)
            InputCaption.Add("Component Name")
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tMask1Start_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tMask1Start.TextChanged
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            sqlText.Add("UPDATE dbo.tblSGComponentNames SET Mask1Start=" & wtb.Text & " WHERE ComponentNameID=" & lbComponentNames.SelectedItem.Value)
            OldText.Add(wtb.OldText)
            InputCaption.Add("Mask 1 Start")
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tMask1Length_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tMask1Length.TextChanged
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            sqlText.Add("UPDATE dbo.tblSGComponentNames SET Mask1Length=" & wtb.Text & " WHERE ComponentNameID=" & lbComponentNames.SelectedItem.Value)
            OldText.Add(wtb.OldText)
            InputCaption.Add("Mask 1 Length")
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tMask2Start_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tMask2Start.TextChanged
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            sqlText.Add("UPDATE dbo.tblSGComponentNames SET Mask2Start=" & wtb.Text & " WHERE ComponentNameID=" & lbComponentNames.SelectedItem.Value)
            OldText.Add(wtb.OldText)
            InputCaption.Add("Mask 2 Start")
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub tMask2Length_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tMask2Length.TextChanged
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            sqlText.Add("UPDATE dbo.tblSGComponentNames SET Mask2Length=" & wtb.Text & " WHERE ComponentNameID=" & lbComponentNames.SelectedItem.Value)
            OldText.Add(wtb.OldText)
            InputCaption.Add("Mask 2 Length")
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtFails_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFails.TextChanged
        Try
            Dim wtb As GGS.WebInputBox = CType(sender, GGS.WebInputBox)            'getting ref to control

            sqlText.Add("UPDATE dbo.tblSGComponentNames SET Fails=" & wtb.Text & " WHERE ComponentNameID=" & lbComponentNames.SelectedItem.Value)
            OldText.Add(wtb.OldText)
            InputCaption.Add("Fails")
            NewText.Add(wtb.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim sql As String

            sql = "INSERT INTO dbo.tblSGComponentNames (ComponentName, Mask1Start, Mask1Length, Mask2Start, Mask2Length, PLCArrayOrder, LineID) " & _
                    "VALUES ('" & wibNewName.Text & "'," & wibNewMask1Start.Text & "," & wibNewMask1Length.Text & "," & _
                                  wibNewMask2Start.Text & "," & wibNewMask2Length.Text & "," & wibNewIndex.Text & "," & _
                                  ddLineNum.SelectedItem.Value.ToString() & ")"

            DA.ExecSQL(sql)

            ' Update the Last Modified Date/Time in the Application Parameters table.
            UpdateTransactionParameters()

            GetComponentNames()
            lbComponentNames.SelectedIndex = lbComponentNames.Items.IndexOf(lbComponentNames.Items.FindByText(wibNewName.Text))
            GetDataIntoCTRLs()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim s As String
            Dim x As Integer = 0
            Dim i As Int32

            For Each s In sqlText
                DA.ExecSQL(s)
                Master.tMsg("Save", InputCaption(x).ToString() & " is changed from: " & OldText(x).ToString() & " To: " & NewText(x).ToString())

                Select Case InputCaption(x).ToString
                    Case "Component Index"
                        imgIndex.Visible = True
                    Case "Component Name"
                        imgName.Visible = True
                    Case "Mask 1 Start"
                        imgMask1Start.Visible = True
                    Case "Mask 1 Length"
                        imgMask1Length.Visible = True
                    Case "Mask 2 Start"
                        imgMask2Start.Visible = True
                    Case "Mask 2 Length"
                        imgMask2Length.Visible = True
                End Select

                x = x + 1
            Next s

            ' Update the Last Modified Date/Time in the Application Parameters table.
            If (x > 0) Then
                UpdateTransactionParameters()
            End If

            'pb, 01/21/05
            i = Me.lbComponentNames.SelectedIndex
            GetComponentNames()
            Me.lbComponentNames.SelectedIndex = i

            GetDataIntoCTRLs()

            'reset sql
            sqlText.Clear()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            DA.ExecSQL("DELETE FROM dbo.tblSGComponentNames WHERE ComponentNameID = " & lbComponentNames.SelectedItem.Value)

            ' Update the Last Modified Date/Time in the Application Parameters table.
            UpdateTransactionParameters()

            GetComponentNames()
            GetDataIntoCTRLs()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub UpdateTransactionParameters()

        Try
            Dim strLineNumber As String = ""

            strLineNumber = CStr(CInt(ddLineNum.SelectedValue.ToString) * 100)
            BizLayer.SetRecipeSavedDT("14", "0", strLineNumber)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub GetDataIntoCTRLs()
        If lbComponentNames.SelectedItem Is Nothing Then
            tIndex.Text = ""
            tIndex.OldText = tIndex.Text
            tName.Text = ""
            tName.OldText = tName.Text
            tMask1Start.Text = ""
            tMask1Start.OldText = tMask1Start.Text
            tMask1Length.Text = ""
            tMask1Length.OldText = tMask1Length.Text
            tMask2Start.Text = ""
            tMask2Start.OldText = tMask2Start.Text
            tMask2Length.Text = ""
            tMask2Length.OldText = tMask2Length.Text
            txtFails.Text = ""
            txtFails.OldText = txtFails.Text
        Else
            Dim ds As DataSet
            ds = DA.GetDataSet("SELECT * FROM tblSGComponentNames WHERE ComponentNameID='" & lbComponentNames.SelectedItem.Value & "'")

            tIndex.Text = ds.Tables(0).Rows(0).Item("PLCArrayOrder").ToString() & ""
            tIndex.OldText = tIndex.Text
            tName.Text = ds.Tables(0).Rows(0).Item("ComponentName").ToString() & ""
            tName.OldText = tName.Text
            tMask1Start.Text = ds.Tables(0).Rows(0).Item("Mask1Start").ToString() & ""
            tMask1Start.OldText = tMask1Start.Text
            tMask1Length.Text = ds.Tables(0).Rows(0).Item("Mask1Length").ToString() & ""
            tMask1Length.OldText = tMask1Length.Text
            tMask2Start.Text = ds.Tables(0).Rows(0).Item("Mask2Start").ToString() & ""
            tMask2Start.OldText = tMask2Start.Text
            tMask2Length.Text = ds.Tables(0).Rows(0).Item("Mask2Length").ToString() & ""
            tMask2Length.OldText = tMask2Length.Text
            txtFails.Text = ds.Tables(0).Rows(0).Item("Fails").ToString() & ""
            txtFails.OldText = txtFails.Text

            ds.Clear()
            ds.Dispose()
        End If
    End Sub

    Private Sub GetComponentNames()
        lbComponentNames.DataSource = DA.GetDataSet("SELECT ComponentNameID, ComponentName FROM tblSGComponentNames WHERE LineID = " & ddLineNum.SelectedValue)
        lbComponentNames.DataTextField = "ComponentName"
        lbComponentNames.DataValueField = "ComponentNameID"
        lbComponentNames.DataBind()
    End Sub

    Private Sub GetLineNumbers()
        ddLineNum.DataSource = DA.GetDataSet("SELECT LineID, LineName FROM dbo.tblSGLines")
        ddLineNum.DataTextField = "LineName"
        ddLineNum.DataValueField = "LineID"
        ddLineNum.DataBind()
        ddLineNum.Items.Insert(0, "Choose a Line")
    End Sub

    Private Sub EnableControls()
        Master.Secure(Me.cmdNew)
        Master.Secure(Me.cmdSave)
        Master.Secure(Me.cmdDelete)

        If (lbComponentNames.SelectedItem Is Nothing) Then
            Me.cmdSave.Enabled = False
            Me.cmdDelete.Enabled = False
        End If
    End Sub

#End Region

End Class