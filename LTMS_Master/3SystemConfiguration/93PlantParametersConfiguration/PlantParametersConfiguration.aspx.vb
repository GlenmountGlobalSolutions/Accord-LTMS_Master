Public Class PlantParametersConfiguration
    Inherits System.Web.UI.Page


#Region "Event Handlers"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                LoadListBox()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub PlantParametersConfiguration_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        Dim ID As Integer = -1
        Dim strID As String = ""
        Dim description As String = ""
        Dim value As String = ""

        Try
            ID = GetNewID()

            If (ID <= 0) Then
                Master.Msg = "Error: Unable to create new ID. Please make sure that all of the current ID's are less than the 4 digit upper limit - 9999"
            Else
                strID = ID.ToString("D4")
                'make sure that input from dialog box is not empty
                If ((txtNewDescription.Text.Length > 0) And (txtNewValue.Text.Length > 0)) Then
                    description = txtNewDescription.Text
                    value = txtNewValue.Text

                    If (Me.lbParams.Items.FindByText(description) IsNot Nothing) Then
                        Master.Msg = "Error: Unable to create new Parameter.  There is already a Plant Parameter with the name <span class='failureNotification'>" + description + "</span>."
                    Else
                        DA.ExecSQL("INSERT INTO tblPlantParametersTypes (PlantParameterTypeID, Description, DisplayID) VALUES ('" & ID & "', '" & description & "', " + strID + ")")
                        DA.ExecSQL("INSERT INTO tblPlantParameters (PlantParameterTypeID, PlantParameterValue) VALUES ('" & ID & "', '" & value & "')")

                        Master.tMsg("New", "New Plant Parameter with description " & description & " and value " & value & " was created.")

                        're-load listbox
                        LoadListBox()

                        HidePicsAndLoadValues()

                    End If

                    If (Me.lbParams.Items.FindByText(description) IsNot Nothing) Then
                        Me.lbParams.SelectedIndex = Me.lbParams.Items.IndexOf(Me.lbParams.Items.FindByText(description))
                        LoadSelectedValues()
                    Else
                        Me.lbParams.SelectedIndex = -1
                    End If

                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Try
            'these values are used to build the sql statement
            Dim csvSqlValues As String = ""
            Dim sql As String = ""
            Dim log As String = ""
            Dim mask As String = ""
            Dim selectedDesc As String = txtDEscription.Text

            'look at data in each textbox and check if it has changed
            'note: sqltext value of input box contains column name
            Me.imgDesc.Visible = False
            Me.imgValue.Visible = False

            If (Me.txtDEscription.Text <> Me.txtDEscription.OldText) Then
                DA.ExecSQL("UPDATE tblPlantParametersTypes SET Description = '" & Me.txtDEscription.Text & "' WHERE (PlantParameterTypeID = '" & Me.lbParams.SelectedValue & "')")
                Me.imgDesc.Visible = True
                log = "Plant Parameter Description changed from " & Me.txtDEscription.OldText & " to " & Me.txtDEscription.Text & ".<br>"
                Master.tMsg("Save", log)
            End If

            If (Me.txtValue.Text <> Me.txtValue.OldText) Then
                DA.ExecSQL("UPDATE tblPlantParameters SET PlantParameterValue = '" & Me.txtValue.Text & "' WHERE (PlantParameterTypeID = '" & Me.lbParams.SelectedValue & "')")
                Me.imgValue.Visible = True
                log = "Plant Parameter Value changed from " & Me.txtValue.OldText & " to " & Me.txtValue.Text & ".<br>"
                Master.tMsg("Save", log)
            End If

            If (log.Length = 0) Then
                Master.Msg = "Not saved: values have not changed"
            End If

            LoadListBox()

            If (Me.lbParams.Items.FindByText(selectedDesc) IsNot Nothing) Then
                Me.lbParams.SelectedIndex = Me.lbParams.Items.IndexOf(Me.lbParams.Items.FindByText(selectedDesc))
                LoadSelectedValues()
            Else
                Me.lbParams.SelectedIndex = -1
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        Try
            DA.ExecSQL("DELETE FROM tblPlantParameters WHERE (PlantParameterTypeID = '" & Me.lbParams.SelectedValue & "')")
            DA.ExecSQL("DELETE FROM tblPlantParametersTypes WHERE (PlantParameterTypeID = '" & Me.lbParams.SelectedValue & "')")

            Me.lbParams.SelectedIndex = -1

            Master.tMsg("Delete", "Plant Parameter with description " & Me.txtDEscription.OldText & " and value " & Me.txtValue.OldText & " was deleted.")

            're-load listbox
            LoadListBox()
            Me.lbParams.SelectedIndex = -1
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Protected Sub lbParams_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbParams.SelectedIndexChanged
        Try
            HidePicsAndLoadValues()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        Master.Secure(cmdNew)
        Master.Secure(cmdSave)
        Master.Secure(cmdDelete)

        If (Me.lbParams.SelectedIndex < 0) Then
            cmdSave.Enabled = False
            cmdDelete.Enabled = False
        End If


    End Sub

    Private Sub HidePics()
        Me.imgDesc.Visible = False
        Me.imgValue.Visible = False
    End Sub

    Private Sub HidePicsAndLoadValues()
        Try
            HidePics()
            LoadSelectedValues()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub
    Private Sub LoadListBox()
        Try
            Dim ds As New DataSet
            ds.Reset()
            ds = DA.GetDataSet("SELECT [ppt].PlantParameterTypeID AS ID, [ppt].Description AS Description FROM [dbo].[tblPlantParametersTypes]  ppt INNER JOIN [dbo].[tblPlantParameters] pp ON [ppt].PlantParameterTypeID = [pp].PlantParameterTypeID ORDER BY [ppt].DisplayID")

            Me.lbParams.DataSource = ds
            Me.lbParams.DataTextField = "Description"
            Me.lbParams.DataValueField = "ID"
            Me.lbParams.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadSelectedValues()
        Dim i As Integer
        Dim dsValues As New DataSet

        Try
            'make sure that a value has been selected
            If (Me.lbParams.SelectedIndex >= 0) Then
                dsValues = DA.GetDataSet("SELECT tblPlantParametersTypes.Description AS Description, tblPlantParameters.PlantParameterValue AS PlantParameterValue " + _
                                         "FROM  [dbo].tblPlantParametersTypes INNER JOIN tblPlantParameters ON [dbo].tblPlantParametersTypes.PlantParameterTypeID " + _
                                         "= tblPlantParameters.PlantParameterTypeID WHERE (tblPlantParametersTypes.PlantParameterTypeID = '" & Me.lbParams.SelectedValue & "')")

                'error checking
                If (Not DA.IsDSEmpty(dsValues)) Then
                    Me.txtDEscription.Text = CStr(dsValues.Tables(0).DefaultView.Table.Rows(i)("Description"))
                    Me.txtDEscription.OldText = CStr(dsValues.Tables(0).DefaultView.Table.Rows(i)("Description"))

                    Me.txtValue.Text = CStr(dsValues.Tables(0).DefaultView.Table.Rows(i)("PlantParameterValue"))
                    Me.txtValue.OldText = CStr(dsValues.Tables(0).DefaultView.Table.Rows(i)("PlantParameterValue"))
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function GetNewID() As Integer
        Dim id As Integer
        Dim count As Integer
        Dim strResult As String

        Try
            count = Me.lbParams.Items.Count

            'nothing in listbox, start with new id
            If (count <= 0) Then
                id = 1
            Else
                id = Convert.ToInt32(Me.lbParams.Items(count - 1).Value.ToString())
                id += 1
                strResult = id.ToString()

                'make sure that id did not go over 9999 (4 digits) to 10000 (5 digits)
                If (strResult.Length >= 5) Then
                    id = -1
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return id

    End Function


#End Region

End Class