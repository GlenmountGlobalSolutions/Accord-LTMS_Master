Public Class StyleGroupConfiguration
    Inherits System.Web.UI.Page

    Public NewText As New ArrayList         'new posted value of ctrl
    Public sqlText As New ArrayList         'sql to make update
    Public InputCaption As New ArrayList    'caption associated with control
    Public OldText As New ArrayList         'old value of ctrl

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                ddlLineNum_LoadAndBind()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub StyleGroupConfiguration_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Try
            If (txtNewStyleGroupID.Text.Length > 0) Then
                Dim sql As String = "Insert into tblSGStyleGroups (PLCArrayOrder, StyleGroupName, LineID) values (" & txtNewStyleGroupID.Text & ",'" & txtNewStyleGroupName.Text & "'," & ddlLineNum.SelectedItem.Value & ")"
                DA.ExecSQL(sql)

                LoadStyleGroups()
                lbStyleGroups.SelectedIndex = lbStyleGroups.Items.IndexOf(lbStyleGroups.Items.FindByText(txtNewStyleGroupName.Text))
                LoadDetails()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        Dim sqlStatement As String
        Dim counter As Integer
        Dim strNewText = txtStyleGroupName.Text()

        Try
            counter = 0
            imgIndex.Visible = False
            imgName.Visible = False

            For Each sqlStatement In sqlText
                DA.ExecSQL(sqlStatement)
                Master.tMsg("Save", InputCaption(counter).ToString() & " is changed from: " & OldText(counter).ToString() & " To: " & NewText(counter).ToString())

                Select Case InputCaption(counter).ToString()
                    Case "Style Group Index"
                        imgIndex.Visible = True
                    Case "Style Group Name"
                        imgName.Visible = True
                End Select
                counter = counter + 1
            Next

            LoadStyleGroups()
            lbStyleGroups.SelectedIndex = lbStyleGroups.Items.IndexOf(lbStyleGroups.Items.FindByText(strNewText))
            LoadDetails()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        Try
            DA.GetDataSet("DELETE FROM tblSGStyleGroups where StyleGroupID=" & lbStyleGroups.SelectedItem.Value & " AND [LineID] = " & ddlLineNum.SelectedItem.Value)
            LoadStyleGroups()
            LoadDetails()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub ddlLineNum_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLineNum.SelectedIndexChanged
        Try
            txtStyleGroupName.Text = ""
            txtStyleGroupName.OldText = ""
            txtStyleGroupID.Text = ""
            txtStyleGroupID.OldText = ""

            If (ddlLineNum.SelectedIndex < 0) Then
                lbStyleGroups.Items.Clear()
            Else
                LoadStyleGroups()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub lbStyleGroups_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbStyleGroups.SelectedIndexChanged
        Try
            LoadDetails()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub txtStyleGroupID_TextChanged(sender As Object, e As System.EventArgs) Handles txtStyleGroupID.TextChanged
        Try
            sqlText.Add("Update tblSGStyleGroups Set PLCArrayOrder=" & txtStyleGroupID.Text & " where StyleGroupID=" & lbStyleGroups.SelectedItem.Value & " AND [LineID] = " & ddlLineNum.SelectedItem.Value)
            OldText.Add(txtStyleGroupID.OldText)
            InputCaption.Add("Style Group Index")
            NewText.Add(txtStyleGroupID.Text)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub txtStyleGroupName_TextChanged(sender As Object, e As System.EventArgs) Handles txtStyleGroupName.TextChanged
        Try
            sqlText.Add("Update tblSGStyleGroups Set StyleGroupName='" & txtStyleGroupName.Text & "' where StyleGroupID=" & lbStyleGroups.SelectedItem.Value & " AND [LineID] = " & ddlLineNum.SelectedItem.Value)
            OldText.Add(txtStyleGroupName.OldText)
            InputCaption.Add("Style Group Name")
            NewText.Add(txtStyleGroupName.Text)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub



#End Region

#Region "Methods"

    Private Sub EnableControls()
        Try
            Master.Secure(cmdNew)
            Master.Secure(cmdSave)
            Master.Secure(cmdDelete)

            If (ddlLineNum.SelectedIndex <= 0) Then
                cmdNew.Enabled = False
            End If

            If (lbStyleGroups.SelectedIndex < 0) Then
                cmdSave.Enabled = False
                cmdDelete.Enabled = False
                txtStyleGroupID.Enabled = False
                txtStyleGroupName.Enabled = False
            Else
                txtStyleGroupID.Enabled = True
                txtStyleGroupName.Enabled = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlLineNum_LoadAndBind()
        Try
            ddlLineNum.DataSource = DA.GetDataSet("SELECT [LineID], [LineName] FROM [dbo].[tblSGLines]")
            ddlLineNum.DataTextField = "LineName"
            ddlLineNum.DataValueField = "LineID"
            ddlLineNum.DataBind()
            ddlLineNum.Items.Insert(0, "Choose a Line")

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Sub LoadStyleGroups()
        Try

            If ddlLineNum.SelectedIndex > 0 Then
                lbStyleGroups.DataSource = Nothing
                lbStyleGroups.DataSource = DA.GetDataSet("SELECT [StyleGroupID], [StyleGroupName] FROM tblSGStyleGroups WHERE [LineID] = " & ddlLineNum.SelectedItem.Value & " ORDER BY [PLCArrayOrder]")
                lbStyleGroups.DataTextField = "StyleGroupName"
                lbStyleGroups.DataValueField = "StyleGroupID"
                lbStyleGroups.DataBind()
            Else
                lbStyleGroups.Items.Clear()

                txtStyleGroupName.Text = ""
                txtStyleGroupName.OldText = ""

                txtStyleGroupID.Text = ""
                txtStyleGroupID.OldText = ""
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub LoadDetails()
        Try
            If lbStyleGroups.SelectedItem Is Nothing Then
                txtStyleGroupName.Text = ""
                txtStyleGroupName.OldText = ""

                txtStyleGroupID.Text = ""
                txtStyleGroupID.OldText = ""

            Else
                Dim ds As DataSet
                ds = DA.GetDataSet("select [StyleGroupName], [PLCArrayOrder] from dbo.tblSGStyleGroups where StyleGroupID=" & lbStyleGroups.SelectedItem.Value & " AND [LineID] = " & ddlLineNum.SelectedItem.Value)
                txtStyleGroupName.Text = ds.Tables(0).Rows(0).Item("StyleGroupName").ToString()
                txtStyleGroupName.OldText = txtStyleGroupName.Text

                txtStyleGroupID.Text = ds.Tables(0).Rows(0).Item("PLCArrayOrder").ToString()
                txtStyleGroupID.OldText = txtStyleGroupID.Text

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

#End Region


End Class