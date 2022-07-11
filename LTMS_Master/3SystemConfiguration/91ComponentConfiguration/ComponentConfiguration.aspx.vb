Public Class ComponentConfiguration
    Inherits System.Web.UI.Page

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

    Private Sub lbComponents_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbComponents.SelectedIndexChanged
        Try
            LoadComponentProperties()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim log As String = ""
            Dim setSql As String = ""
            Dim sql As String = ""
            Dim componentID As String = tCompID.Text

            If ValidateComponentProperties() Then
                If tCompID.Text <> tCompID.OldText Then
                    setSql = setSql & ", ComponentID='" & tCompID.Text + "'"
                    log = log & "<em>Component ID</em> is changed from <b>" & tCompID.OldText.ToString() & "</b> to <b>" & tCompID.Text.ToString() & "</b>.<br />"
                    imgCheckCompID.Visible = True
                Else
                    imgCheckCompID.Visible = False
                End If

                If tDescription.Text <> tDescription.OldText Then
                    setSql = setSql & ", Description='" & tDescription.Text & "'"
                    log = log & "<em>Description</em> is changed from <b>" & tDescription.OldText.ToString() & "</b> to <b>" & tDescription.Text.ToString() & "</b>.<br />"
                    imgCheckDescription.Visible = True
                Else
                    imgCheckDescription.Visible = False
                End If

                If tMask.Text <> tMask.OldText Then
                    setSql = setSql & ", SerialNumberMask='" & FormatForSaveOrDisplay(tMask.Text, True) & "'"
                    log = log & "<em>Mask</em> is changed from <b>" & tMask.OldText.ToString() & "</b> to <b>" & tMask.Text.ToString() & "</b>.<br />"
                    imgCheckMask.Visible = True
                Else
                    imgCheckMask.Visible = False
                End If

                If ddOrderParam.SelectedIndex > 0 And ddOrderParam.SelectedItem.Text <> ddOrderParam.OldText Then
                    setSql = setSql & ", OrderParameterID='" & ddOrderParam.SelectedItem.Value & "'"
                    log = log & "<em>Order Parameter</em> is changed from <b>" & ddOrderParam.OldText.ToString() & "</b> to <b>" & ddOrderParam.SelectedItem.Text.ToString() & "</b>.<br />"
                    imgCheckOrderParam.Visible = True
                Else
                    imgCheckOrderParam.Visible = False
                End If

                If Len(setSql) > 1 Then

                    sql = "UPDATE dbo.tblComponentID SET" & setSql.Remove(0, 1) & " WHERE ComponentID = '" & lbComponents.SelectedItem.Value & "'"

                    DA.ExecSQL(sql)
                    Master.tMsg("Save", log)

                    LoadComponentList()
                    Me.lbComponents.SelectedIndex = lbComponents.Items.IndexOf(lbComponents.Items.FindByValue(componentID))
                    LoadComponentProperties()
                Else
                    Master.Msg = "Not saved: Values have not changed."
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            DA.ExecSQL("DELETE FROM dbo.tblComponentID where ComponentID= '" & lbComponents.SelectedItem.Value & "'")
            Master.tMsg("Delete", "Component <em>" & lbComponents.SelectedItem.Text.ToUpper() & "</em> was deleted.")

            LoadPageData()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Try
            Dim ds As DataSet
            Dim sql As String = "Insert into tblComponentID (ComponentID, Description, SerialNumberMask, OrderParameterID) " _
                                & "values ('" & wibNewComponentID.Text & "','" & wibNewDescription.Text & "','" & FormatForSaveOrDisplay(wibNewMask.Text, True) & "','" & wddNewOrderParam.SelectedItem.Value & "')"
            ds = DA.GetDataSet(sql)
            Master.tMsg("Add", "Component <em>" & wibNewDescription.Text.ToUpper() & "</em> was added.")

            LoadPageData()
            lbComponents.SelectedIndex = lbComponents.Items.IndexOf(lbComponents.Items.FindByValue(wibNewComponentID.Text))
            LoadComponentProperties()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally

        End Try
    End Sub
#End Region

#Region "Methods"

    Private Sub LoadPageData()
        Try
            ClearComponentProperties()
            LoadComponentList()
            LoadOrderParameters()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadComponentList()
        Try
            lbComponents.DataSource = DA.GetDataSet("Select ComponentID, Description from [dbo].[tblComponentID]")
            lbComponents.DataTextField = "Description"
            lbComponents.DataValueField = "ComponentID"
            lbComponents.DataBind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadComponentProperties()

        ClearComponentProperties()

        If lbComponents.SelectedIndex >= 0 Then
            Dim ds As DataSet
            ds = DA.GetDataSet("select Description, SerialNumberMask, ComponentID, OrderParameterID from [dbo].[tblComponentID] where ComponentID='" & lbComponents.SelectedItem.Value & "'")

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                With ds.Tables(0).Rows(0)

                    tCompID.Text = .Item("ComponentID").ToString()
                    tCompID.OldText = .Item("ComponentID").ToString()
                    tCompID.Enabled = True

                    tDescription.Text = .Item("Description").ToString()
                    tDescription.OldText = .Item("Description").ToString()
                    tDescription.Enabled = True

                    tMask.Text = Me.FormatForSaveOrDisplay(.Item("SerialNumberMask").ToString(), False)
                    tMask.OldText = tMask.Text
                    tMask.Enabled = True

                    ddOrderParam.Enabled = True
                    If ddOrderParam.Items.FindByValue(.Item("OrderParameterID").ToString()) Is Nothing Then
                        'If we cannot find the item
                        ddOrderParam.OldText = ""
                        ddOrderParam.SelectedIndex = 0
                    Else
                        'select the item if we can find the item.
                        ddOrderParam.SelectedIndex = ddOrderParam.Items.IndexOf(ddOrderParam.Items.FindByValue(.Item("OrderParameterID").ToString()))
                        ddOrderParam.OldText = ddOrderParam.SelectedItem.Text
                    End If

                End With
            End If
        End If
    End Sub

    Private Sub LoadOrderParameters()
        Try
            Dim ds As DataSet
            ds = DA.GetDataSet("SELECT OrderParameterID, Description FROM dbo.tblOrderParameters")

            'main page order param dropdown
            ddOrderParam.Items.Clear()
            ddOrderParam.DataSource = ds
            ddOrderParam.DataTextField = "Description"
            ddOrderParam.DataValueField = "OrderParameterID"
            ddOrderParam.DataBind()
            ddOrderParam.Items.Insert(0, New ListItem("Please Select"))

            'New dialog order param dropdown
            wddNewOrderParam.Items.Clear()
            wddNewOrderParam.DataSource = ds.Copy()
            wddNewOrderParam.DataTextField = "Description"
            wddNewOrderParam.DataValueField = "OrderParameterID"
            wddNewOrderParam.DataBind()
            wddNewOrderParam.Items.Insert(0, New ListItem("Please Select"))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ClearComponentProperties()
        tCompID.Text = ""
        tCompID.OldText = ""
        tCompID.Enabled = False

        tDescription.Text = ""
        tDescription.OldText = ""
        tDescription.Enabled = False

        tMask.Text = ""
        tMask.OldText = ""
        tMask.Enabled = False

        ddOrderParam.SelectedIndex = -1
        ddOrderParam.Enabled = False
    End Sub

    Private Function ValidateComponentProperties() As Boolean
        Try
            Dim status As Boolean = True

            If Len(tDescription.Text) < 1 Then
                Master.Msg = "Error: Invalid Description. Please enter a description."
                status = False And status
                Me.imgQuestDescription.Visible = True
            Else : imgQuestDescription.Visible = False
            End If

            If (IsNumeric(Me.tCompID.Text)) Then
                Dim s As Single = Convert.ToSingle(Me.tCompID.Text)
                If (s < 0) Or (s >= 10000) Then
                    Master.Msg = "Error: Invalid Component ID. Please enter a component ID greater than 0."
                    status = False And status
                    Me.imgQuestCompID.Visible = True
                Else : imgQuestCompID.Visible = False
                End If
            Else
                Master.Msg = "Error: Invalid Component ID. Please enter a component ID greater than 0."
                status = False And status
                imgQuestCompID.Visible = True
            End If

            Return status

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Private Function FormatForSaveOrDisplay(ByRef str As String, ByRef bfrmt As Boolean) As String
        Dim dschar As DataSet
        Dim dr As DataRow

        dschar = DA.GetDataSet("SELECT * FROM tblCharacterSwap")
        If (bfrmt = False) Then       'display
            For Each dr In dschar.Tables(0).Rows
                str = str.Replace(dr(1).ToString, dr(0).ToString())
            Next
        Else          'save
            For Each dr In dschar.Tables(0).Rows
                str = str.Replace(dr(0).ToString, dr(1).ToString())
            Next
        End If

        Return str
    End Function

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdDelete)
            Master.Secure(Me.cmdNew)
            Master.Secure(Me.cmdSave)

            If (Me.lbComponents.SelectedIndex >= 0) Then
                cmdDelete.Enabled = True
                cmdSave.Enabled = True
            Else
                cmdSave.Enabled = False
                cmdDelete.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

End Class