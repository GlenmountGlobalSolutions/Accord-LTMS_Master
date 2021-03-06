Imports System.Data.SqlClient
Imports System.Xml
Imports System.IO
Imports System.Runtime.CompilerServices

Public Class ODSStationConfiguration
    Inherits System.Web.UI.Page

#Region "Properties and Enums"

    ' this is the special case keyword used to dentoe which status is the 'Active' one.
    Private Const KEYWORD_ACTIVE = "Active"

    ' See table tblConfigurationTypes
    Enum ConfigurationTypes
        InstructionSet = 1
        InstructionStepFilters = 2
        AuxiliaryListA = 3
        AuxiliaryListB = 4
    End Enum

    Enum MCRDatasetColumns
        MasterChangeRequestID = 0
        MCRNumber
        StationMCRNumber
        MCRDate
        MCRStatus_ConfigurationID
        MCRStatus
        IsActiveOnClient
        MCRDescription
        MCRNumberTextValue
    End Enum


    'gridview gvSteps column configuration
    Enum gvStepsColumnConfig
        rbSelectorSteps
        InstructionID
        StepID
        DisplayID
        Color
        Description
    End Enum

    'gridview gvImages column configuration
    Enum gvImagesColumnConfig
        rbSelectorImages
        InstructionID
        ImageID
        DisplayID
        Description
        ImagePath
    End Enum


    'First six columns of gridview gvChecks
    Enum gvChecksColumnConfig
        InstructionID
        StepID
        DisplayID
        SeeEDSFlag
        SpecificQualityFlag
        SeqControlFlag
    End Enum

    Enum gvImageChecksColumnConfig
        InstructionID
        ImageID
        DisplayID
    End Enum

    Enum DialogType
        CopyDst
        CopySrc
        Edit
        NewDlg
    End Enum

    Private _impersonateDomain As String
    Private _impersonateUser As String
    Private _impersonatePass As String
    Private _selectedRowIndex_Steps As Integer = -1
    Private _selectedRowIndex_Images As Integer = -1

    Private Property SelectedRowIndex_Steps() As Integer
        Get
            If (String.IsNullOrWhiteSpace(Request.Form("rbgSelectorGroupSteps"))) OrElse (Integer.TryParse(Request.Form("rbgSelectorGroupSteps"), _selectedRowIndex_Steps) = False) Then
                _selectedRowIndex_Steps = -1
            End If

            Return _selectedRowIndex_Steps
        End Get
        Set(value As Integer)
            _selectedRowIndex_Steps = value
        End Set
    End Property

    Private Property SelectedRowIndex_Images() As Integer
        Get
            If (String.IsNullOrWhiteSpace(Request.Form("rbgSelectorGroupImages"))) OrElse (Integer.TryParse(Request.Form("rbgSelectorGroupImages"), _selectedRowIndex_Images) = False) Then
                _selectedRowIndex_Images = -1
            End If

            Return _selectedRowIndex_Images
        End Get
        Set(value As Integer)
            _selectedRowIndex_Images = value
        End Set
    End Property

    Private ReadOnly Property LineID As Integer
        Get
            Dim id As Integer = -1
            Integer.TryParse(ddlLineNumbers.SelectedValue, id)
            Return id
        End Get
    End Property

    Private ReadOnly Property StationID As String
        Get
            Return ddlStations.SelectedValue
        End Get
    End Property

    Private ReadOnly Property ConfigurationID As Integer
        Get
            Dim id As Integer = -1
            Integer.TryParse(ddlInstructionSets.SelectedValue, id)
            Return id
        End Get
    End Property

    Private ReadOnly Property MasterChangeRequestID As Integer
        Get
            Dim id As Integer = -1
            Integer.TryParse(ddlMCRNumber.SelectedValue, id)
            Return id
        End Get
    End Property

    Private ReadOnly Property LineEDSFilePath As String
        Get
            Dim path As String = ""
            If (ddlLineNumbers.SelectedItem.Attributes.Count > 0) Then
                path = ddlLineNumbers.SelectedItem.Attributes("ImagePath")
            End If
            Return path
        End Get
    End Property

#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                hidLastTab.Value = "0"

                'Expire the current cache associated with the sqlDataSource_Colors which will force new data to be pulled.
                'If this is not done, subsequent new page trips will pull data from cache to populate the GridView
                Cache.Remove(sqlDataSource_Colors.CacheKeyDependency)
                Cache(sqlDataSource_Colors.CacheKeyDependency) = "CacheCleared"

                LoadConfigurationTypeLabels()
                BuildGrid_Checks(gvChecks, GetInstructionStepConfigurations(LineID, StationID, ConfigurationID, MasterChangeRequestID), [Enum].GetValues(GetType(gvChecksColumnConfig)).Length)
                BuildGrid_Checks(gvImageChecks, GetInstructionImageConfigurations(LineID, StationID, ConfigurationID, MasterChangeRequestID), [Enum].GetValues(GetType(gvImageChecksColumnConfig)).Length)
                InitializPageDropDownLists()

                txtMCRDateNew.Text = Date.Today().ToString("MM/dd/yyyy")
            End If
            hidModfiedBy.Value = Page.User.Identity.Name
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        EnableControls()
    End Sub

    Private Sub gvSteps_Load(sender As Object, e As System.EventArgs) Handles gvSteps.Load
        Try
            gvSteps.SelectedIndex = SelectedRowIndex_Steps
            gvChecks.SelectedIndex = SelectedRowIndex_Steps

            gvImages.SelectedIndex = SelectedRowIndex_Images
            gvImageChecks.SelectedIndex = SelectedRowIndex_Images
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvSteps_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSteps.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' get a reference to the Literal control
                Dim radioButtonOutput As Literal = CType(e.Row.FindControl("rbSelectorSteps"), Literal)

                If (radioButtonOutput IsNot Nothing) Then

                    ' Output the radiobutton markup except for the "checked" attribute
                    radioButtonOutput.Text = String.Format("<input id='RowIndex{0}' type='radio' name='rbgSelectorGroupSteps' value='{0}' ", e.Row.RowIndex)

                    If (SelectedRowIndex_Steps = e.Row.RowIndex) Then
                        radioButtonOutput.Text += " checked='checked'"
                    End If
                    If ddlInstructionSets.SelectedIndex <= 0 Then
                        radioButtonOutput.Text += " DISABLED='DISABLED'"
                    End If
                    radioButtonOutput.Text += " />"
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvImages_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvImages.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' get a reference to the Literal control
                Dim radioButtonOutput As Literal = CType(e.Row.FindControl("rbSelectorImages"), Literal)

                If (radioButtonOutput IsNot Nothing) Then

                    ' Output the radiobutton markup except for the "checked" attribute
                    radioButtonOutput.Text = String.Format("<input id='RowIndex{0}' type='radio' name='rbgSelectorGroupImages' value='{0}' ", e.Row.RowIndex)

                    If (SelectedRowIndex_Images = e.Row.RowIndex) Then
                        radioButtonOutput.Text += " checked='checked'"
                    End If
                    If ddlInstructionSets.SelectedIndex <= 0 Then
                        radioButtonOutput.Text += " DISABLED='DISABLED'"
                    End If
                    radioButtonOutput.Text += " />"
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub gvSteps_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSteps.RowDataBound
        Try
            '  do not use the visible property of the BulkEditBoundField on the ASPX page
            '  ---------------------------------------------------------------------
            e.Row.Cells(gvStepsColumnConfig.StepID).Visible = False  'hide column StepID
            e.Row.Cells(gvStepsColumnConfig.InstructionID).Visible = False  'hide column InstructionID

            Dim ddlColorNames As DropDownList
            Dim colorName As String

            If e.Row.RowType = DataControlRowType.DataRow Then

                colorName = DataBinder.Eval(e.Row.DataItem, "Color").ToString()
                ' if there is a value for colorname, find the dropdownlist and see if contains the value, if so, then set the selcted value
                If (colorName.Trim.Length > 0) Then
                    ddlColorNames = CType(e.Row.FindControl("ddlColor"), DropDownList)
                    If (ddlColorNames IsNot Nothing) Then
                        If Not IsNothing(ddlColorNames.Items.FindByValue(colorName)) Then
                            ddlColorNames.SelectedValue = colorName
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvImages_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvImages.RowDataBound
        Try
            '  do not use the visible property of the BulkEditBoundField on the ASPX page
            '  ---------------------------------------------------------------------
            e.Row.Cells(gvImagesColumnConfig.InstructionID).Visible = False
            e.Row.Cells(gvImagesColumnConfig.ImageID).Visible = False
            e.Row.Cells(gvImagesColumnConfig.DisplayID).Visible = False

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvChecks_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvChecks.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                Dim i As Integer = 0
                For Each cell As DataControlFieldHeaderCell In e.Row.Cells
                    cell.Attributes.Add("title", cell.Text)
                    Select Case i
                        '  do not use the visible property of the BulkEditBoundField on the ASPX page
                        '  ---------------------------------------------------------------------
                        Case gvChecksColumnConfig.InstructionID, gvChecksColumnConfig.StepID, gvChecksColumnConfig.DisplayID
                            'hide columns InstructionID, StepID & DisplayID
                            cell.Visible = False
                            '  ---------------------------------------------------------------------


                        Case gvChecksColumnConfig.SeeEDSFlag
                            cell.Text = String.Format("<img id='imgSeeEDS' src='../../../Images/ODS/SeeEDS.png' /><span class='rotateHeader45 odsHeaderSpecial'>{0}</span><div class='coverClip' />", cell.Text)

                        Case gvChecksColumnConfig.SpecificQualityFlag
                            cell.Text = String.Format("<img id='imgSpecificQuality' src='../../../Images/ODS/SpecificQuality.png' /><span class='rotateHeader45 odsHeaderSpecial'>{0}</span>", cell.Text)

                        Case gvChecksColumnConfig.SeqControlFlag
                            cell.Text = String.Format("<img id='imgSeqControl' src='../../../Images/ODS/SeqControl.png' /><span class='rotateHeader45 odsHeaderSpecial'>{0}</span>", cell.Text)

                        Case Else
                            If i Mod 2 = 0 Then
                                cell.Text = String.Format("<span class='rotateHeader45 odsHeader'>{0}</span>", cell.Text)
                                cell.CssClass = "odsColumn"
                            Else
                                cell.Text = String.Format("<span class='rotateHeader45 odsHeaderAlt'>{0}</span>", cell.Text)
                                cell.CssClass = "odsColumnAlt"
                            End If

                    End Select

                    i = i + 1
                Next
            ElseIf (e.Row.RowType = DataControlRowType.DataRow) Or (e.Row.RowType = DataControlRowType.EmptyDataRow) Then
                Dim i As Integer = 0
                For Each cell As DataControlFieldCell In e.Row.Cells
                    Select Case i
                        '  do not use the visible property of the BulkEditBoundField on the ASPX page
                        '  ---------------------------------------------------------------------
                        Case gvChecksColumnConfig.InstructionID, _
                            gvChecksColumnConfig.StepID, _
                            gvChecksColumnConfig.DisplayID

                            'hide columns InstructionID, StepID & DisplayID
                            cell.Visible = False
                            '  ---------------------------------------------------------------------

                        Case gvChecksColumnConfig.SeeEDSFlag, _
                            gvChecksColumnConfig.SpecificQualityFlag, _
                            gvChecksColumnConfig.SeqControlFlag

                            cell.CssClass = "odsColumnSpecial css-checkbox-td"

                        Case Else
                            If i Mod 2 = 0 Then
                                cell.CssClass = "odsColumn css-checkbox-td"
                            Else
                                cell.CssClass = "odsColumnAlt css-checkbox-td"
                            End If

                    End Select

                    i = i + 1
                Next
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub gvImageChecks_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvImageChecks.RowDataBound
        Try
            Dim strAddCoverClip = "<div class='coverClipForImages' />"  'only add to first one

            If (e.Row.RowType = DataControlRowType.Header) Then
                Dim i As Integer = 0
                For Each cell As DataControlFieldHeaderCell In e.Row.Cells
                    cell.Attributes.Add("title", cell.Text)
                    Select Case i
                        '  do not use the visible property of the BulkEditBoundField on the ASPX page
                        '  ---------------------------------------------------------------------
                        Case gvImageChecksColumnConfig.InstructionID, gvImageChecksColumnConfig.ImageID, gvImageChecksColumnConfig.DisplayID
                            'hide columns InstructionID, ImageID & DisplayID
                            cell.Visible = False
                            '  ---------------------------------------------------------------------
                        Case Else

                            If i Mod 2 = 0 Then
                                cell.Text = String.Format("<span class='rotateHeader45 odsHeaderAlt'>{0}</span>", cell.Text)
                                cell.CssClass = "odsColumnAlt"
                            Else
                                cell.Text = String.Format("<span class='rotateHeader45 odsHeader'>{0}</span>" + strAddCoverClip, cell.Text)
                                cell.CssClass = "odsColumn"
                                strAddCoverClip = ""    'only add to first one
                            End If
                    End Select
                    i = i + 1
                Next
            ElseIf (e.Row.RowType = DataControlRowType.DataRow) Or (e.Row.RowType = DataControlRowType.EmptyDataRow) Then
                Dim i As Integer = 0
                For Each cell As DataControlFieldCell In e.Row.Cells
                    Select Case i
                        '  do not use the visible property of the BulkEditBoundField on the ASPX page
                        '  ---------------------------------------------------------------------
                        Case gvImageChecksColumnConfig.InstructionID, _
                            gvImageChecksColumnConfig.ImageID, _
                            gvImageChecksColumnConfig.DisplayID
                            'hide columns InstructionID, ImageID & DisplayID
                            cell.Visible = False
                            '  ---------------------------------------------------------------------

                        Case Else
                            If i Mod 2 = 0 Then
                                cell.CssClass = "odsColumnAlt css-checkbox-td"
                            Else
                                cell.CssClass = "odsColumn css-checkbox-td"
                            End If

                    End Select

                    i = i + 1
                Next
                hidNumberOfModelCols.Value = i.ToString()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub




    Private Sub gvSteps_RowUpdating(sender As Object, e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvSteps.RowUpdating
        'If the row had an update, then pass the selected value.  if this is not done, then the edit will effectively save an empty value.
        e.NewValues("Color") = CType(gvSteps.Rows(e.RowIndex).FindControl("ddlColor"), DropDownList).SelectedValue
    End Sub


    Private Sub gvSteps_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvSteps.RowEditing
        Try
            gvChecks.EditIndex = e.NewEditIndex
            gvImages.EditIndex = e.NewEditIndex
            gvImageChecks.EditIndex = e.NewEditIndex
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvImages_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvImages.RowEditing
        Try
            gvSteps.EditIndex = e.NewEditIndex
            gvChecks.EditIndex = e.NewEditIndex
            gvImageChecks.EditIndex = e.NewEditIndex
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub gvSteps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvSteps.SelectedIndexChanged
        Try
            gvChecks.SelectedIndex = gvSteps.SelectedIndex
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvImages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvImages.SelectedIndexChanged
        Try
            gvImageChecks.SelectedIndex = gvImages.SelectedIndex
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub gvSteps_RowCancelingEdit(sender As Object, e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvSteps.RowCancelingEdit
        Try
            gvChecks.EditIndex = -1
            gvImages.EditIndex = -1
            gvImageChecks.EditIndex = -1
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub gvImages_RowCancelingEdit(sender As Object, e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvImages.RowCancelingEdit
        Try
            gvSteps.EditIndex = -1
            gvChecks.EditIndex = -1
            gvImageChecks.EditIndex = -1
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub



    Private Sub ddlLineNumbers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlLineNumbers.SelectedIndexChanged
        Try
            If (ddlLineNumbersCopySrc.Items.Count >= ddlLineNumbers.SelectedIndex - 1) Then
                ddlLineNumbersCopySrc.SelectedIndex = ddlLineNumbers.SelectedIndex
            End If
            If (ddlLineNumbersNew.Items.Count >= ddlLineNumbers.SelectedIndex - 1) Then
                ddlLineNumbersNew.SelectedIndex = ddlLineNumbers.SelectedIndex
            End If
            LoadStations()
            LoadMCRNumbers()
            LoadInstructions()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlStations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStations.SelectedIndexChanged
        Try
            If (ddlStationsCopySrc.Items.Count >= ddlStations.SelectedIndex - 1) Then
                ddlStationsCopySrc.SelectedIndex = ddlStations.SelectedIndex
                LoadInstructionsCopy(DialogType.CopySrc)
            End If
            LoadInstructionSets()
            LoadMCRNumbers()
            LoadInstructions()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlInstructionSets_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlInstructionSets.SelectedIndexChanged
        Try
            If (ddlInstructionSetsCopySrc.Items.Count >= ddlInstructionSets.SelectedIndex - 1) Then
                ddlInstructionSetsCopySrc.SelectedIndex = ddlInstructionSets.SelectedIndex
                LoadInstructionsCopy(DialogType.CopySrc)
            End If
            If (ddlInstructionSetsNew.Items.Count >= ddlInstructionSets.SelectedIndex - 1) Then
                ddlInstructionSetsNew.SelectedIndex = ddlInstructionSets.SelectedIndex
            End If

            LoadMCRNumbers()
            LoadInstructions()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlMCRNumber_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMCRNumber.SelectedIndexChanged
        Try
            If (ddlMCRNumberCopySrc.Items.Count = ddlMCRNumber.Items.Count) Then
                ddlMCRNumberCopySrc.SelectedIndex = ddlMCRNumber.SelectedIndex
                LoadInstructionsCopy(DialogType.CopySrc)
            End If
            If (ddlMCRNumber.SelectedIndex >= 0) Then
                LoadSaveDialogControls(ddlMCRNumber.SelectedItem)
            End If
            LoadInstructions()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub


    Private Sub ddlLineNumbersNew_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLineNumbersNew.SelectedIndexChanged
        Try
            GetMCRNumber_ForDialog(DialogType.NewDlg)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ddlInstructionSetsNew_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlInstructionSetsNew.SelectedIndexChanged
        Try
            GetMCRNumber_ForDialog(DialogType.NewDlg)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdMoveUp_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdMoveUp.Click
        If (hidLastTab.Value = "0") Then
            MoveRowUp(gvSteps, gvChecks, rowSelectorDelta_steps)
        Else
            MoveRowUp(gvImages, gvImageChecks, rowSelectorDelta_images)
        End If
    End Sub

    Private Sub cmdMoveDown_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdMoveDown.Click
        If (hidLastTab.Value = "0") Then
            MoveRowDown(gvSteps, gvChecks, rowSelectorDelta_steps)
        Else
            MoveRowDown(gvImages, gvImageChecks, rowSelectorDelta_images)
        End If
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click
        SaveInstructionSteps()
    End Sub

    Private Sub cmdCopy_Click(sender As Object, e As System.EventArgs) Handles cmdCopy.Click
        CopyStepsToInstruction()
    End Sub

    Private Sub cmdClearRow_Click(sender As Object, e As System.EventArgs) Handles cmdClearRow.Click
        Try
            If (hidLastTab.Value = "0") Then
                gvSteps.ClearSelectedRowData()
                gvChecks.ClearSelectedRowData()
            Else
                gvImages.ClearSelectedRowData()
                gvImageChecks.ClearSelectedRowData()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        Try
            If (ddlInstructionSetsCopySrc.Items.Count >= ddlInstructionSets.SelectedIndex - 1) Then
                ddlInstructionSetsCopySrc.SelectedIndex = ddlInstructionSets.SelectedIndex
                LoadInstructionsCopy(DialogType.CopySrc)
            End If
            LoadInstructions()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdCreateNew_Click(sender As Object, e As System.EventArgs) Handles cmdCreateNew.Click
        Try
            Dim li As ListItem
            Dim mcrID As Integer

            mcrID = SaveNewMCR()

            If (ddlLineNumbers.Items.Count >= ddlLineNumbersNew.SelectedIndex - 1) Then
                ddlLineNumbers.SelectedIndex = ddlLineNumbersNew.SelectedIndex
                LoadStations()
                LoadInstructions()
            End If
            If (ddlStations.Items.Count = 1) Then
                LoadStations()
                ddlStations.SelectedIndex = 1
                LoadInstructions()
            End If
            If (ddlInstructionSets.Items.Count >= ddlInstructionSetsNew.SelectedIndex - 1) Then
                ddlInstructionSets.SelectedIndex = ddlInstructionSetsNew.SelectedIndex
            End If

            LoadMCRNumbers()

            li = ddlMCRNumber.Items.FindByValue(mcrID.ToString())
            ddlMCRNumber.SelectedIndex = ddlMCRNumber.Items.IndexOf(li)

            LoadSaveDialogControls(li)
            LoadInstructions()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdRefreshImages_Click(sender As Object, e As System.EventArgs) Handles cmdRefreshImages.Click
        LoadImageFiles()
    End Sub


#End Region

#Region "Methods"
    Private Sub EnableControls()
        Try
            Master.Secure(cmdMoveUp)
            Master.Secure(cmdMoveDown)
            Master.Secure(cmdSave)
            Master.Secure(cmdCopy)
            Master.Secure(cmdPrint)
            Master.Secure(cmdCreateNew)
            Master.Secure(cmdClearRow)
            Master.Secure(cmdRefresh)
            Master.Secure(chkIncludeWatermark)


            If ddlLineNumbers.SelectedIndex > 0 Then
                ddlStations.Enabled = True
            Else
                ddlStations.Enabled = False
                cmdRefresh.Enabled = False
            End If

            If ddlStations.SelectedIndex > 0 Then
                ddlInstructionSets.Enabled = True
            Else
                ddlInstructionSets.Enabled = False
                ddlInstructionSets.SelectedIndex = -1
                cmdRefresh.Enabled = False
            End If

            If (ddlInstructionSets.SelectedIndex > 0) Then
                ddlMCRNumber.Enabled = True
            Else
                ddlMCRNumber.Enabled = False
                ddlMCRNumber.SelectedIndex = -1
                cmdRefresh.Enabled = False
            End If

            If (ddlMCRNumber.SelectedIndex > 0) Then
                cblAuxListA.Enabled = True
                cblAuxListB.Enabled = True
                gvSteps.Enabled = True
                gvChecks.Enabled = True
                cmdMoveUp.Enabled = cmdMoveUp.Enabled And True 'And SelectedRowIndex > 0
                cmdMoveDown.Enabled = cmdMoveDown.Enabled And True 'And SelectedRowIndex > 0 And SelectedRowIndex < gvSteps.Rows.Count - 1
                cmdClearRow.Enabled = cmdClearRow.Enabled And True
                cmdRefresh.Enabled = cmdRefresh.Enabled And True
                cmdPrint.Enabled = cmdPrint.Enabled And True
            Else
                cblAuxListA.Enabled = False
                cblAuxListB.Enabled = False
                gvSteps.Enabled = False
                gvChecks.Enabled = False
                cmdMoveUp.Enabled = False
                cmdMoveDown.Enabled = False
                cmdClearRow.Enabled = False
                cmdRefresh.Enabled = False
                cmdPrint.Enabled = False

                cmdSave.Enabled = False
            End If

            'Copy Dialog:source
            If ddlLineNumbersCopySrc.SelectedIndex > 0 Then
                ddlStationsCopySrc.Enabled = True
            Else
                ddlStationsCopySrc.Enabled = False
            End If

            If ddlStationsCopySrc.SelectedIndex > 0 Then
                ddlInstructionSetsCopySrc.Enabled = True
            Else
                ddlInstructionSetsCopySrc.Enabled = False
            End If

            If ddlInstructionSetsCopySrc.SelectedIndex > 0 Then
                ddlMCRNumberCopySrc.Enabled = True
            Else
                ddlMCRNumberCopySrc.Enabled = False
            End If

            If ddlMCRNumberCopySrc.SelectedIndex > 0 And ddlMCRNumberCopySrc.Enabled Then
                lbxInstructionsCopySrc.Enabled = True
            Else
                lbxInstructionsCopySrc.Enabled = False
            End If

            'Copy Dialog: destination
            If ddlLineNumbersCopyDst.SelectedIndex > 0 Then
                ddlStationsCopyDst.Enabled = True
            Else
                ddlStationsCopyDst.Enabled = False
            End If

            If ddlStationsCopyDst.SelectedIndex > 0 Then
                ddlInstructionSetsCopyDst.Enabled = True
            Else
                ddlInstructionSetsCopyDst.Enabled = False
            End If

            If ddlInstructionSetsCopyDst.SelectedIndex > 0 Then
                ddlMCRNumberCopyDst.Enabled = True
            Else
                ddlMCRNumberCopyDst.Enabled = False
            End If

            If ddlMCRNumberCopyDst.SelectedIndex > 0 And ddlMCRNumberCopyDst.Enabled Then
                lbxInstructionsCopyDst.Enabled = True
            Else
                lbxInstructionsCopyDst.Enabled = False
            End If


            'Copy Dialog: Down arrows
            If ddlInstructionSetsCopyDst.SelectedIndex > 0 AndAlso rblCopyMode.SelectedIndex > -1 Then
                If rblCopyMode.SelectedValue = "0" Then
                    'moving all items
                    cmdMoveDownCopy.Enabled = True
                ElseIf rblCopyMode.SelectedValue = "1" AndAlso lbxInstructionsCopySrc.SelectedIndex > -1 Then
                    If lbxInstructionsCopyDst.Items.FindByText(lbxInstructionsCopySrc.SelectedItem.Text) Is Nothing Then
                        'only copy items that do not exist in the destination box.
                        cmdMoveDownCopy.Enabled = True
                    Else
                        cmdMoveDownCopy.Enabled = False
                    End If
                Else
                    cmdMoveDownCopy.Enabled = False
                End If
            Else
                cmdMoveDownCopy.Enabled = False
            End If

            'Copy Dialog: up arrows
            If lbxInstructionsCopyDst.SelectedIndex > -1 Then
                cmdMoveUpCopy.Enabled = True
            Else
                cmdMoveUpCopy.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub InitializPageDropDownLists()
        ResetPage(Nothing, Nothing, Nothing, Nothing)
    End Sub

    Private Sub ResetPage(ByVal lineID As String, ByVal stationID As String, ByVal instructionConfigurationID As String, ByVal mcrNumber As String)
        Try
            '#If DEBUG Then
            '            LoadLines()
            '            ddlLineNumbers.SelectedIndex = 2
            '            LoadStations()
            '            ddlStations.SelectedIndex = 57
            '            LoadInstructionSets()
            '            ddlInstructionSets.SelectedIndex = 1
            '#Else
            LoadLines()
            If lineID IsNot Nothing Then                'select line
                ddlLineNumbers.SelectedIndex = ddlLineNumbers.Items.IndexOf(ddlLineNumbers.Items.FindByValue(lineID.ToString()))
                ddlLineNumbersCopySrc.SelectedIndex = ddlLineNumbers.SelectedIndex
            End If

            LoadStations()
            If stationID IsNot Nothing Then             'select station
                ddlStations.SelectedIndex = ddlStations.Items.IndexOf(ddlStations.Items.FindByValue(stationID))
                ddlStationsCopySrc.SelectedIndex = ddlStations.SelectedIndex
            End If

            LoadInstructionSets()
            If instructionConfigurationID IsNot Nothing Then    'select instruction set
                ddlInstructionSets.SelectedIndex = ddlInstructionSets.Items.IndexOf(ddlInstructionSets.Items.FindByValue(instructionConfigurationID.ToString()))
                ddlInstructionSetsCopySrc.SelectedIndex = ddlInstructionSets.SelectedIndex
            End If

            '#End If

            LoadMCRNumbers()
            If mcrNumber IsNot Nothing Then             'select MCR
                ddlMCRNumber.SelectedIndex = ddlMCRNumber.Items.IndexOf(ddlMCRNumber.Items.FindByValue(mcrNumber.ToString()))
                LoadSaveDialogControls(ddlMCRNumber.SelectedItem)
            End If

            LoadMCRStatusDescriptionsIntoDialogs()

            lbxInstructionsCopySrc.Items.Clear()
            lbxInstructionsCopyDst.Items.Clear()

            LoadAuxiliaryList(cblAuxListA, ConfigurationTypes.AuxiliaryListA)
            LoadAuxiliaryList(cblAuxListB, ConfigurationTypes.AuxiliaryListB)

            LoadInstructionsCopy(DialogType.CopySrc)
            LoadInstructions()

            copyStepCount.Value = "0"

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub BuildGrid_Checks(theGridView As GGS.BulkEditGridView, ds As DataSet, countOfgvChecksType As Integer)
        Dim i As Integer
        Try
            For i = countOfgvChecksType To theGridView.Columns.Count - 1
                theGridView.Columns.RemoveAt(countOfgvChecksType)
            Next
            If (ds.IsNotEmpty()) Then
                For i = countOfgvChecksType To ds.Tables(0).Columns.Count - 1
                    Dim checkCol As New CheckBoxField()
                    checkCol.HeaderText = ds.Tables(0).Columns(i).ColumnName
                    checkCol.DataField = checkCol.HeaderText
                    checkCol.Text = " "
                    theGridView.Columns.Insert(i, checkCol)
                Next
            End If
            theGridView.DataSource = ds
            theGridView.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadConfigurationTypeLabels()
        Dim ds As DataSet = GetConfigurationTypes()

        If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
            For Each row As DataRow In ds.Tables(0).Rows
                If CInt(row.Item("ConfigurationTypeID")) = ConfigurationTypes.InstructionSet Then
                    'sets the label over the third drop down which defaults to "Instruction Sets"
                    lblInstructionSet.Text = row.Item("Description").ToString() & ":"
                    lblInstructionSetCopySrc.Text = row.Item("Description").ToString() & ":"

                ElseIf CInt(row.Item("ConfigurationTypeID")) = ConfigurationTypes.AuxiliaryListA Then
                    'sets the label over the first auxiliary list which defaults as "Axiliary List A"
                    lblAuxListA.Text = row.Item("Description").ToString()

                ElseIf CInt(row.Item("ConfigurationTypeID")) = ConfigurationTypes.AuxiliaryListB Then
                    'sets the label over the second auxiliary list which defaults as "Axiliary List B"
                    lblAuxListB.Text = row.Item("Description").ToString()

                End If
            Next

        End If

    End Sub

    Private Sub LoadLines()
        Try
            Dim ds As DataSet = DA.GetDataSet("SELECT LineID, LineName, EDSFilePath FROM dbo.tblSGLines")

            ' Lines for the drop-down list on the main web page
            ddlLineNumbers.Items.Clear()
            ddlLineNumbers.DataSource = ds
            ddlLineNumbers.DataTextField = "LineName"
            ddlLineNumbers.DataValueField = "LineID"
            ddlLineNumbers.DataBind()

            For Each dr As DataRow In ds.Tables(0).Rows
                Dim li As New ListItem
                li = ddlLineNumbers.Items.FindByText(dr("LineName").ToString())
                If li IsNot Nothing Then
                    li.Attributes.Add("ImagePath", dr("EDSFilePath").ToString())
                End If
            Next dr
            ddlLineNumbers.Items.Insert(0, New ListItem("Choose a Line", ""))


            'Lines for the copy dialog src
            ddlLineNumbersCopySrc.Items.Clear()
            ddlLineNumbersCopySrc.DataSource = ds.Copy
            ddlLineNumbersCopySrc.DataTextField = "LineName"
            ddlLineNumbersCopySrc.DataValueField = "LineID"
            ddlLineNumbersCopySrc.DataBind()
            ddlLineNumbersCopySrc.Items.Insert(0, New ListItem("Choose a Line", ""))

            'Lines for the copy dialog destination
            ddlLineNumbersCopyDst.Items.Clear()
            ddlLineNumbersCopyDst.DataSource = ds.Copy
            ddlLineNumbersCopyDst.DataTextField = "LineName"
            ddlLineNumbersCopyDst.DataValueField = "LineID"
            ddlLineNumbersCopyDst.DataBind()
            ddlLineNumbersCopyDst.Items.Insert(0, New ListItem("Choose a Line", ""))

            'Lines for the New dialog 
            ddlLineNumbersNew.Items.Clear()
            ddlLineNumbersNew.DataSource = ds.Copy
            ddlLineNumbersNew.DataTextField = "LineName"
            ddlLineNumbersNew.DataValueField = "LineID"
            ddlLineNumbersNew.DataBind()
            ddlLineNumbersNew.Items.Insert(0, New ListItem("Choose a Line", ""))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadStations()
        Try
            Dim ds As DataSet = Nothing
            ddlStations.Items.Clear()
            If (ddlLineNumbers.SelectedIndex > 0) Then
                ds = GetStations(LineID)

                ddlStations.DataSource = ds
                ddlStations.DataTextField = "Description"
                ddlStations.DataValueField = "StationID"
                ddlStations.DataBind()

                ddlStationsCopySrc.DataSource = ds.Copy
                ddlStationsCopySrc.DataTextField = "Description"
                ddlStationsCopySrc.DataValueField = "StationID"
                ddlStationsCopySrc.DataBind()
            End If

            ddlStations.Items.Insert(0, New ListItem("Choose a Station", ""))
            ddlStationsCopySrc.Items.Insert(0, New ListItem("Choose a Station", ""))
            ddlStationsCopyDst.Items.Insert(0, New ListItem("Choose a Station", ""))
            ddlStationsCopyDst.SelectedIndex = 0

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadInstructionSets()
        Try
            Dim ds As DataSet = Nothing
            Dim instructionConfigurationID As Integer = ConfigurationID

            ddlInstructionSets.Items.Clear()

            If (ddlStations.SelectedIndex > 0) Then
                ds = GetInstructionSets(ddlStations.SelectedValue)
            Else
                ds = GetInstructionSets("-1")
            End If

            ddlInstructionSets.DataSource = ds
            ddlInstructionSets.DataTextField = "Description"
            ddlInstructionSets.DataValueField = "ConfigurationID"
            ddlInstructionSets.DataBind()
            ddlInstructionSets.Items.Insert(0, New ListItem("Choose a Model", ""))

            ddlInstructionSetsCopySrc.DataSource = ds.Copy()
            ddlInstructionSetsCopySrc.DataTextField = "Description"
            ddlInstructionSetsCopySrc.DataValueField = "ConfigurationID"
            ddlInstructionSetsCopySrc.DataBind()
            ddlInstructionSetsCopySrc.Items.Insert(0, New ListItem("Choose a Model", ""))

            ddlInstructionSetsCopyDst.Items.Insert(0, New ListItem("Choose a Model", ""))

            ddlInstructionSetsNew.DataSource = ds.Copy()
            ddlInstructionSetsNew.DataTextField = "Description"
            ddlInstructionSetsNew.DataValueField = "ConfigurationID"
            ddlInstructionSetsNew.DataBind()
            ddlInstructionSetsNew.Items.Insert(0, New ListItem("Choose a Model", ""))


            If instructionConfigurationID <> 0 Then    'select instruction set
                ddlInstructionSets.SelectedIndex = ddlInstructionSets.Items.IndexOf(ddlInstructionSets.Items.FindByValue(instructionConfigurationID.ToString()))
                ddlInstructionSetsCopySrc.SelectedIndex = ddlInstructionSets.SelectedIndex
                ddlInstructionSetsNew.SelectedIndex = ddlInstructionSets.SelectedIndex
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadMCRNumbers()
        Dim ds As DataSet = Nothing

        Try
            ddlMCRNumber.Items.Clear()
            txtStationMCR.Text = ""
            lblStationMCRNumber.Text = ""
            txtMCRDateEdit.Text = ""
            lblMCRDate.Text = ""

            ds = GetMCRNumbers()

            If (ds.IsNotEmpty()) Then
                For Each record As DataRow In ds.Tables(0).Rows()
                    Dim li As New ListItem(record(MCRDatasetColumns.MCRNumberTextValue).ToString(), record(MCRDatasetColumns.MasterChangeRequestID).ToString())
                    li.Attributes.Add("MCRDate", record(MCRDatasetColumns.MCRDate).ToString())
                    li.Attributes.Add("MCRNumber", record(MCRDatasetColumns.MCRNumber).ToString())
                    li.Attributes.Add("StationMCRNumber", record(MCRDatasetColumns.StationMCRNumber).ToString())
                    li.Attributes.Add("MCRStatusID", record(MCRDatasetColumns.MCRStatus_ConfigurationID).ToString())
                    li.Attributes.Add("MCRDescription", record(MCRDatasetColumns.MCRDescription).ToString())
                    li.Attributes.Add("MCRIsActiveOnClient", record(MCRDatasetColumns.IsActiveOnClient).ToString())
                    If (CBool(record(MCRDatasetColumns.IsActiveOnClient))) Then
                        li.Attributes.Add("class", "IsActiveOnClient")
                        li.Selected = True
                        LoadSaveDialogControls(li)
                    End If
                    ddlMCRNumber.Items.Add(li)
                Next

                GetMCRNumber_ForDialog(DialogType.CopySrc)
                GetMCRNumber_ForDialog(DialogType.NewDlg)
                LoadInstructionsCopy(DialogType.CopySrc)
            Else
                ClearDialogMCRDropDowns()
            End If
            ddlMCRNumber.Items.Insert(0, New ListItem("Choose an MCR Number", ""))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadMCRStatusDescriptionsIntoDialogs()
        Try
            Dim parameters As New List(Of SqlParameter)
            Dim ds As DataSet = Nothing

            parameters.Add(New SqlParameter("@ConfigurationTypeID", 5))

            ds = DA.GetDataSet("[ods].[procSelectConfigurations]", parameters, "")

            ddlMCRStatusEdit.Items.Clear()
            ddlMCRStatusEdit.DataSource = ds
            ddlMCRStatusEdit.DataTextField = "Description"
            ddlMCRStatusEdit.DataValueField = "ConfigurationID"
            ddlMCRStatusEdit.DataBind()

            ddlMCRStatusNew.Items.Clear()
            ddlMCRStatusNew.DataSource = ds.Copy
            ddlMCRStatusNew.DataTextField = "Description"
            ddlMCRStatusNew.DataValueField = "ConfigurationID"
            ddlMCRStatusNew.DataBind()

            '20140527   Active cannot show up in the list
            ddlMCRStatusNew.Items.Remove(ddlMCRStatusNew.Items.FindByText(KEYWORD_ACTIVE))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadInstructions()
        Try
            'clear the controls
            UncheckListA()
            UncheckListB()
            SelectedRowIndex_Steps = -1

            BuildGrid_Checks(gvChecks, GetInstructionStepConfigurations(LineID, StationID, ConfigurationID, MasterChangeRequestID), [Enum].GetValues(GetType(gvChecksColumnConfig)).Length)
            BuildGrid_Checks(gvImageChecks, GetInstructionImageConfigurations(LineID, StationID, ConfigurationID, MasterChangeRequestID), [Enum].GetValues(GetType(gvImageChecksColumnConfig)).Length)


            If ddlMCRNumber.SelectedIndex > 0 Then
                CheckOffItemsInAuxList(cblAuxListA, ConfigurationTypes.AuxiliaryListA)
                CheckOffItemsInAuxList(cblAuxListB, ConfigurationTypes.AuxiliaryListB)
                cblAuxListA.Enabled = True
                cblAuxListB.Enabled = True
            End If
            gvSteps.DataBind()
            gvImages.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadAuxiliaryList(ByRef cblAuxList As CheckBoxList, ByVal listConfigurationTypeID As ConfigurationTypes)
        Try
            cblAuxList.Items.Clear()
            cblAuxList.DataSource = GetConfigurations(listConfigurationTypeID)
            cblAuxList.DataTextField = "Description"
            cblAuxList.DataValueField = "ConfigurationID"
            cblAuxList.DataBind()
            cblAuxList.Enabled = False
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub CheckOffItemsInAuxList(ByRef cblAuxList As CheckBoxList, ByVal listConfigurationTypeID As ConfigurationTypes)
        Dim ds As DataSet
        Dim item As ListItem
        Try
            'check off any items in the Auxiliary list.
            ds = GetAuxiliaryLists(listConfigurationTypeID)
            If (DA.IsDataSetNotEmpty(ds)) Then
                For Each row As DataRow In ds.Tables(0).Rows
                    item = cblAuxList.Items.FindByValue(row.Item("ConfigurationID").ToString())
                    If item IsNot Nothing Then
                        item.Selected = True
                    End If
                Next
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function GetStations(ByVal lineID As Integer) As DataSet
        Dim ds As DataSet = Nothing
        Try
            Dim parameters As New List(Of SqlParameter)
            parameters.Add(New SqlParameter("@LineID", lineID))
            parameters.Add(New SqlParameter("@hasInstructions", DBNull.Value))

            ds = DA.GetDataSet("[ods].[procSelectStations]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function

    Private Function GetConfigurationTypes() As DataSet

        Dim parameters As New List(Of SqlParameter)
        parameters.Add(New SqlParameter("@ConfigurationTypeID", DBNull.Value))
        Dim ds As DataSet = DA.GetDataSet("[ods].[procSelectConfigurationTypes]", parameters, "")

        Return ds
    End Function

    Private Function GetConfigurations(ByVal configurationTypeID As Integer) As DataSet

        Dim parameters As New List(Of SqlParameter)
        parameters.Add(New SqlParameter("@ConfigurationTypeID", configurationTypeID))
        Dim ds As DataSet = DA.GetDataSet("[ods].[procSelectConfigurations]", parameters, "")

        Return ds

    End Function

    Private Function GetInstructionSets(ByVal stationID As String) As DataSet
        Dim ds As DataSet
        Dim parameters As New List(Of SqlParameter)
        parameters.Add(New SqlParameter("@StationID", stationID))
        parameters.Add(New SqlParameter("@hasInstructions", DBNull.Value))

        ds = DA.GetDataSet("[ods].[procSelectInstructionSets]", parameters, "")

        Return ds
    End Function

    Private Function GetInstructions(ByVal lineID As String, ByVal stationID As String, ByVal instructionConfigurationID As String, masterChangeRequestID As String) As DataSet

        Dim parameters As New List(Of SqlParameter)
        parameters.Add(New SqlParameter("@LineID", lineID))
        parameters.Add(New SqlParameter("@StationID", stationID))
        parameters.Add(New SqlParameter("@ConfigurationID", instructionConfigurationID))
        parameters.Add(New SqlParameter("@MasterChangeRequestID", masterChangeRequestID))
        Dim ds As DataSet = DA.GetDataSet("[ods].[procSelectInstructions]", parameters, "")

        Return ds
    End Function

    Private Function GetInstructionStepConfigurations(ByVal lineID As Integer, ByVal stationID As String, ByVal instructionConfigurationID As Integer, ByVal masterChangeRequestID As Integer) As DataSet

        Dim ds As DataSet
        Dim parameters As New List(Of SqlParameter)

        If lineID = 0 Then
            parameters.Add(New SqlParameter("@LineID", DBNull.Value))
        Else
            parameters.Add(New SqlParameter("@LineID", lineID.ToString()))
        End If

        If stationID Is Nothing Then
            parameters.Add(New SqlParameter("@StationID", DBNull.Value))
        Else
            parameters.Add(New SqlParameter("@StationID", stationID.ToString()))
        End If

        If instructionConfigurationID = 0 Then
            parameters.Add(New SqlParameter("@ConfigurationID", DBNull.Value))
        Else
            parameters.Add(New SqlParameter("@ConfigurationID", instructionConfigurationID.ToString()))
        End If

        If masterChangeRequestID = 0 Then
            parameters.Add(New SqlParameter("@MasterChangeRequestID", DBNull.Value))
        Else
            parameters.Add(New SqlParameter("@MasterChangeRequestID", masterChangeRequestID.ToString()))
        End If

        ds = DA.GetDataSet("[ods].[procSelectInstructionStepConfigurations]", parameters, "")

        Return ds

    End Function

    Private Function GetInstructionImageConfigurations(ByVal lineID As Integer, ByVal stationID As String, ByVal instructionConfigurationID As Integer, ByVal masterChangeRequestID As Integer) As DataSet

        Dim ds As DataSet
        Dim parameters As New List(Of SqlParameter)

        If lineID = 0 Then
            parameters.Add(New SqlParameter("@LineID", DBNull.Value))
        Else
            parameters.Add(New SqlParameter("@LineID", lineID.ToString()))
        End If

        If stationID Is Nothing Then
            parameters.Add(New SqlParameter("@StationID", DBNull.Value))
        Else
            parameters.Add(New SqlParameter("@StationID", stationID.ToString()))
        End If

        If instructionConfigurationID = 0 Then
            parameters.Add(New SqlParameter("@ConfigurationID", DBNull.Value))
        Else
            parameters.Add(New SqlParameter("@ConfigurationID", instructionConfigurationID.ToString()))
        End If

        If masterChangeRequestID = 0 Then
            parameters.Add(New SqlParameter("@MasterChangeRequestID", DBNull.Value))
        Else
            parameters.Add(New SqlParameter("@MasterChangeRequestID", masterChangeRequestID.ToString()))
        End If

        ds = DA.GetDataSet("[ods].[procSelectInstructionImageConfigurations]", parameters, "")

        Return ds

    End Function

    Private Function GetAuxiliaryLists(ByVal listConfigurationTypeID As Integer) As DataSet
        Dim ds As DataSet = Nothing
        Dim parameters As New List(Of SqlParameter)

        Try
            parameters.Add(New SqlParameter("@LineID", LineID))
            parameters.Add(New SqlParameter("@StationID", StationID))
            parameters.Add(New SqlParameter("@ConfigurationID", ConfigurationID))
            parameters.Add(New SqlParameter("@ConfigurationTypeID", listConfigurationTypeID))
            parameters.Add(New SqlParameter("@MasterChangeRequestID", MasterChangeRequestID))

            ds = DA.GetDataSet("[ods].[procSelectInstructionAuxiliaryLists]", parameters, "")

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return ds

    End Function

    Private Function GetMCRNumbers() As DataSet
        Dim ds As DataSet = Nothing
        Try
            Dim parameters As New List(Of SqlParameter)

            parameters.Add(New SqlParameter("@lineID", LineID.ToString()))
            parameters.Add(New SqlParameter("@stationID", StationID))
            parameters.Add(New SqlParameter("@configurationID", ConfigurationID.ToString()))

            ds = DA.GetDataSet("[ods].[procSelectMasterChangeRequests]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function

    Private Function GetMCRNumbers(lineID As String, stationID As String, configID As String) As DataSet
        Dim ds As DataSet = Nothing
        Try
            Dim parameters As New List(Of SqlParameter)

            parameters.Add(New SqlParameter("@lineID", lineID))
            parameters.Add(New SqlParameter("@configurationID", configID))

            ds = DA.GetDataSet("[ods].[procSelectMasterChangeRequests]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function

    Private Sub ClearDialogMCRDropDowns()
        Try
            ddlMCRNumberCopySrc.Items.Clear()
            ddlMCRNumberCopySrc.Items.Insert(0, New ListItem("Choose an MCR Number", ""))

            ddlMCRNumberNewCopyFrom.Items.Clear()
            ddlMCRNumberNewCopyFrom.Items.Insert(0, New ListItem("- Do Not Copy -", "-1"))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub GetMCRNumber_ForDialog(dlgType As DialogType)
        Dim ds As DataSet = Nothing
        Dim parameters As New List(Of SqlParameter)
        Dim ddlMCR As DropDownList = Nothing
        Dim ddlLine As DropDownList = Nothing
        Dim ddlConfig As DropDownList = Nothing
        Try

            Select Case dlgType
                Case DialogType.CopySrc
                    ddlMCR = ddlMCRNumberCopySrc
                    ddlLine = ddlLineNumbersCopySrc
                    ddlConfig = ddlInstructionSetsCopySrc

                Case DialogType.CopyDst
                    ddlMCR = ddlMCRNumberCopyDst
                    ddlLine = ddlLineNumbersCopyDst
                    ddlConfig = ddlInstructionSetsCopyDst

                Case DialogType.Edit
                    ' not used

                Case DialogType.NewDlg
                    ddlMCR = ddlMCRNumberNewCopyFrom
                    ddlLine = ddlLineNumbersNew
                    ddlConfig = ddlInstructionSetsNew
            End Select

            If (ddlMCR IsNot Nothing) Then
                ddlMCR.Items.Clear()

                ds = GetMCRNumbers(ddlLine.SelectedValue, "", ddlConfig.SelectedValue)

                If (ds.IsNotEmpty()) Then
                    For Each record As DataRow In ds.Tables(0).Rows()
                        Dim li As New ListItem(record(MCRDatasetColumns.MCRNumberTextValue).ToString(), record(MCRDatasetColumns.MasterChangeRequestID).ToString())
                        li.Attributes.Add("MCRDate", record(MCRDatasetColumns.MCRDate).ToString())
                        li.Attributes.Add("MCRNumber", record(MCRDatasetColumns.MCRNumber).ToString())
                        li.Attributes.Add("StationMCRNumber", record(MCRDatasetColumns.StationMCRNumber).ToString())
                        li.Attributes.Add("MCRStatusID", record(MCRDatasetColumns.MCRStatus_ConfigurationID).ToString())
                        li.Attributes.Add("MCRDescription", record(MCRDatasetColumns.MCRDescription).ToString())
                        li.Attributes.Add("MCRIsActiveOnClient", record(MCRDatasetColumns.IsActiveOnClient).ToString())
                        If (CBool(record(MCRDatasetColumns.IsActiveOnClient))) Then
                            li.Attributes.Add("class", "IsActiveOnClient")
                            li.Selected = True
                        End If
                        ddlMCR.Items.Add(li)
                    Next
                End If

                If (dlgType <> DialogType.NewDlg) Then
                    ddlMCR.Items.Insert(0, New ListItem("Choose an MCR Number", "-1"))
                Else
                    ddlMCR.Items.Insert(0, New ListItem("- Do Not Copy -", "-1"))
                End If

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadSaveDialogControls(li As ListItem)
        Try
            lblStationMCRNumber.Text = ""
            lblMCRDate.Text = ""

            If (li IsNot Nothing) Then
                lblStationMCRNumber.Text = li.Attributes("StationMCRNumber")

                lblMCRDate.Text = Utility.FormattedDate(li.Attributes("MCRDate"))

                txtMCRNumberEdit.Text = li.Attributes("MCRNumber")
                ddlMCRStatusEdit.SelectedIndex = ddlMCRStatusEdit.Items.IndexOf(ddlMCRStatusEdit.Items.FindByValue(li.Attributes("MCRStatusID")))
                txtMCRDescriptionEdit.Text = li.Attributes("MCRDescription")
            End If

            txtStationMCR.Text = lblStationMCRNumber.Text
            txtMCRDateEdit.Text = lblMCRDate.Text

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub SaveAuxiliaryList(ByRef cblAuxiliaryList As CheckBoxList, ByVal listConfigurationTypeID As ConfigurationTypes)
        Dim csvAuxiliaryList As New StringBuilder
        Try
            'get and save Auxiliary list
            For Each li As ListItem In cblAuxiliaryList.Items
                If li.Selected = True Then
                    csvAuxiliaryList.Append(li.Value).Append(",")
                End If
            Next
            If csvAuxiliaryList.Length > 1 Then
                csvAuxiliaryList.Length = csvAuxiliaryList.Length - 1   'trim the last comma
            End If
            UpdateAuxiliaryLists(listConfigurationTypeID, csvAuxiliaryList.ToString())
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function UpdateAuxiliaryLists(ByVal listConfigurationTypeID As Integer, ByVal csvItems As String) As DataSet
        Dim ds As DataSet = Nothing
        Dim parameters As New List(Of SqlParameter)
        Try
            parameters.Add(New SqlParameter("@LineID", LineID))
            parameters.Add(New SqlParameter("@StationID", StationID))
            parameters.Add(New SqlParameter("@ConfigurationID", ConfigurationID))
            parameters.Add(New SqlParameter("@MasterChangeRequestID", MasterChangeRequestID))
            parameters.Add(New SqlParameter("@List_ConfigurationTypeID", listConfigurationTypeID))
            parameters.Add(New SqlParameter("@ListItemIDs_csv", csvItems))
            parameters.Add(New SqlParameter("@ModifiedBy", Page.User.Identity.Name))
            ds = DA.GetDataSet("[ods].[procUpdateInstructionLists]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return ds
    End Function

    Private Sub CopyInstructionsToDB(ByVal xmlParameters As XmlDocument)
        Dim parameters As New List(Of SqlParameter)
        Try
            parameters.Add(New SqlParameter("@xmlParameters", xmlParameters.InnerXml.ToString()))
            parameters.Add(New SqlParameter("@ModifiedBy", Page.User.Identity.Name.ToString()))
            parameters.Add(New SqlParameter("@copyCheckMarks", chkCopyChecks.Checked))
            DA.ExecSP("[ods].[procCopyInstructions]", parameters)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SaveGridViewChecks()
        Try
            Dim values As String = GetXMLDirtyRows(gvChecks)

            If Len(values) > 1 Then
                Dim parameters As New List(Of SqlParameter)
                parameters.Add(New SqlParameter("@LineID", LineID))
                parameters.Add(New SqlParameter("@StationID", StationID))
                parameters.Add(New SqlParameter("@instruction_ConfigurationID", ConfigurationID))
                parameters.Add(New SqlParameter("@MasterChangeRequestID", MasterChangeRequestID))
                parameters.Add(New SqlParameter("@xmlParameters", values))
                parameters.Add(New SqlParameter("@ModifiedBy", Page.User.Identity.Name))
                DA.ExecSP("[ods].[procUpdateInstructionConfigurations]", parameters)
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SaveGridViewImageChecks()
        Try
            Dim values As String = GetXMLDirtyRows(gvImageChecks)

            If Len(values) > 1 Then
                Dim parameters As New List(Of SqlParameter)
                parameters.Add(New SqlParameter("@LineID", LineID))
                parameters.Add(New SqlParameter("@StationID", StationID))
                parameters.Add(New SqlParameter("@instruction_ConfigurationID", ConfigurationID))
                parameters.Add(New SqlParameter("@MasterChangeRequestID", MasterChangeRequestID))
                parameters.Add(New SqlParameter("@xmlParameters", values))
                parameters.Add(New SqlParameter("@ModifiedBy", Page.User.Identity.Name))
                DA.ExecSP("[ods].[procUpdateInstructionImageConfigurations]", parameters)
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function GetXMLDirtyRows(ByRef grid As GGS.BulkEditGridView) As String
        Dim xmlDoc As New XmlDocument()

        Try
            Dim dirtyRows As List(Of GridViewRow) = grid.DirtyRows

            If dirtyRows.Count > 0 Then
                Dim xmlGrid As XmlElement = xmlDoc.CreateElement("grid")
                For Each row As GridViewRow In dirtyRows
                    'extract the cell values
                    Dim values As New OrderedDictionary()
                    Dim headerValues As New List(Of String)
                    For Each cell As DataControlFieldCell In row.Cells
                        cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, True)
                        If values.Count = headerValues.Count + 1 Then
                            'only add the headertext if there was a cell value
                            headerValues.Add(cell.ContainingField.HeaderText)
                        End If
                    Next (cell)

                    'build the xml row
                    Dim xmlRow As XmlElement = xmlDoc.CreateElement("row")
                    For index As Integer = 0 To values.Count - 1
                        If values(index) IsNot Nothing AndAlso headerValues(index) IsNot Nothing Then
                            Dim xmlCol As XmlElement = xmlDoc.CreateElement("column")
                            xmlCol.SetAttribute("name", headerValues(index).ToString())
                            xmlCol.SetAttribute("value", values(index).ToString())
                            xmlRow.AppendChild(xmlCol)
                        End If
                    Next index

                    'get row data keys   (works except if the datakeyname has a space or special character not supported by xml)
                    For Each key As DictionaryEntry In grid.DataKeys(row.RowIndex).Values
                        xmlRow.SetAttribute(key.Key.ToString(), key.Value.ToString())
                    Next
                    xmlGrid.AppendChild(xmlRow)
                Next row

                xmlDoc.AppendChild(xmlGrid)
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return xmlDoc.InnerXml
    End Function

    Private Function SaveMCRNumber() As Boolean
        Dim result As Boolean = False
        Dim oSqlParameter As SqlParameter
        Dim parameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim mcrDate As Date
        Dim status As String = ""
        Dim message As String = ""
        Try
            If (Date.TryParse(txtMCRDateEdit.Text, mcrDate) = False) Then
                Master.eMsg("MCR Date is not a valid date.")
            Else

                parameters.Add(New SqlParameter("@masterChangeRequestID", ddlMCRNumber.SelectedValue))
                parameters.Add(New SqlParameter("@lineID", LineID))
                parameters.Add(New SqlParameter("@stationID", StationID))
                parameters.Add(New SqlParameter("@configurationID", ConfigurationID))
                parameters.Add(New SqlParameter("@mcrNumber", txtMCRNumberEdit.Text.Trim()))
                parameters.Add(New SqlParameter("@mcrDate", mcrDate.ToString()))
                parameters.Add(New SqlParameter("@mcrDescription", txtMCRDescriptionEdit.Text()))
                parameters.Add(New SqlParameter("@stationMCRNumber", txtStationMCR.Text.Trim()))
                parameters.Add(New SqlParameter("@stationChangeDescription", txtStationChangeDescriptionEdit.Text()))
                parameters.Add(New SqlParameter("@mcrStatus_ConfigurationID", ddlMCRStatusEdit.SelectedValue))
                parameters.Add(New SqlParameter("@isActiveOnClient", ddlMCRNumber.SelectedItem.Attributes("MCRIsActiveOnClient")))
                parameters.Add(New SqlParameter("@modifiedBy", Page.User.Identity.Name))

                ' output params
                oSqlParameter = New SqlParameter("@newMasterChangeRequestID", SqlDbType.Int)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("[ods].[procUpdateMasterChangeRequest]", parameters)

                For Each oParameter In colOutput
                    With oParameter
                        If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                            status = oParameter.Value.ToString()
                        ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                            message = oParameter.Value.ToString()
                        End If
                    End With
                Next

                result = (status.ToUpper() = "TRUE")

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return result
    End Function


    Private Sub SaveInstructionSteps()
        Try
            If (SaveMCRNumber()) Then
                gvSteps.Save()
                SaveGridViewChecks()
                gvImages.Save()
                SaveGridViewImageChecks()
                SaveAuxiliaryList(cblAuxListA, ConfigurationTypes.AuxiliaryListA)
                SaveAuxiliaryList(cblAuxListB, ConfigurationTypes.AuxiliaryListB)
                Master.Msg = "Configuration has been saved."
            Else
                Master.Msg = "Configuration has not been saved."
            End If

            'reload all information
            ResetPage(LineID.ToString(), StationID, ConfigurationID.ToString(), MasterChangeRequestID.ToString())

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub UncheckListA()
        For Each item As ListItem In cblAuxListA.Items
            item.Selected = False
        Next
    End Sub

    Private Sub UncheckListB()
        For Each item As ListItem In cblAuxListB.Items
            item.Selected = False
        Next
    End Sub

    Private Function SaveNewMCR() As Integer
        Dim mcrID As Integer
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            Dim status As String = ""
            Dim message As String = ""

            oSqlParameter = New SqlParameter("@masterChangeRequestID", SqlDbType.Int)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@lineID", SqlDbType.Int)
            oSqlParameter.Value = ddlLineNumbersNew.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@configurationID", SqlDbType.Int)
            oSqlParameter.Value = ddlInstructionSetsNew.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@mcrNumber", SqlDbType.VarChar, 10)
            oSqlParameter.Value = txtMCRNumberNew.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@mcrDate", SqlDbType.DateTime)
            oSqlParameter.Value = txtMCRDateNew.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@MCRDescription", SqlDbType.NVarChar, 100)
            oSqlParameter.Value = txtMCRDescriptionNew.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationMCRNumber", SqlDbType.NVarChar, 10)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationChangeDescription", SqlDbType.NVarChar, 400)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@mcrStatus_ConfigurationID", SqlDbType.Int)
            oSqlParameter.Value = ddlMCRStatusNew.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@isActiveOnClient", SqlDbType.Bit)
            oSqlParameter.Value = False
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 50)
            oSqlParameter.Value = Page.User.Identity.Name
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@copyFromMCRID", SqlDbType.Int)
            oSqlParameter.Value = ddlMCRNumberNewCopyFrom.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@changeCurrentActiveStatusTo", SqlDbType.Int)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)


            ' output params
            oSqlParameter = New SqlParameter("@newMasterChangeRequestID", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)


            colOutput = DA.ExecSP("[ods].[procUpdateMasterChangeRequest]", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@newMasterChangeRequestID" Then
                        mcrID = CInt(oParameter.Value.ToString())
                    End If
                End With
            Next

            If (status.ToUpper() = "TRUE") Then
                Master.tMsg("Insert", "Master Change Request [" + txtMCRNumberNew.Text + "] Has Been Added Successfully.")
            Else
                Master.tMsg("Insert", "Error - Master Change Request Insert Failed for [" + txtMCRNumberNew.Text + "]. <BR>Error message: " + message + "<BR>Status message: " + status)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return mcrID
    End Function

    Private Sub LoadImageFiles()
        Try
            Dim strDirIn As String = LineEDSFilePath.TrimEnd({"\"c})
            Dim rootNode As TreeNode
            Dim x As Integer = 0

            treeViewFolder.Nodes.Clear()

            If ((LineEDSFilePath.Length <= 0) AndAlso (Not Directory.Exists(LineEDSFilePath))) Then
                Master.eMsg("Error: Image directory is not configured.")
                treeViewFolder.Nodes.Add(New TreeNode("Error: Image directory is not configured.", "", Nothing))
            Else

                _impersonateUser = BizLayer.GetApplicationParameterValue("2213", "0050")

                If (_impersonateUser.ToString().Length > 0) Then
                    _impersonateDomain = BizLayer.GetApplicationParameterValue("2215", "0050")
                    _impersonatePass = BizLayer.GetApplicationParameterValue("2214", "0050")
                    Using New Impersonation(_impersonateDomain, _impersonateUser, _impersonatePass)
                        rootNode = Me.PopulateTreeView(strDirIn)
                    End Using
                Else
                    rootNode = Me.PopulateTreeView(strDirIn)
                End If
                ' Add this Node hierarchy to the TreeNode control. 
                treeViewFolder.Nodes.Add(rootNode)
                Utility.TreeExpand(treeViewFolder, 1)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    <MethodImplAttribute(MethodImplOptions.NoInlining)>
    Private Function PopulateTreeView(thisDir As String) As TreeNode
        Dim thisDirNode As New TreeNode(thisDir.Substring(thisDir.LastIndexOf("\") + 1), Nothing)


        Dim subDirs = From folder In Directory.EnumerateDirectories(thisDir)
        For Each subDir In subDirs
            thisDirNode.ChildNodes.Add(PopulateTreeView(subDir))
        Next

        Dim files = From file In Directory.EnumerateFiles(thisDir)
        For Each file In files
            thisDirNode.ChildNodes.Add(New TreeNode(file.Substring(file.LastIndexOf("\") + 1)))
        Next

        GC.Collect()
        GC.WaitForPendingFinalizers()

        Return thisDirNode
    End Function

    Private Sub MoveRowUp(gvLeft As GGS.BulkEditGridView, gvRight As GGS.BulkEditGridView, delta As HtmlInputHidden)
        Try
            If (gvLeft.SelectedIndex = 0) Then
                Master.Msg = "ERROR:  cannot move row up..."
            ElseIf (gvLeft.SelectedIndex < 0) Then
                Master.Msg = "Please Select a Row"
            Else
                delta.Value = "-1"
                gvLeft.MoveSelectedRowUp()
                gvRight.MoveSelectedRowUp()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub MoveRowDown(gvLeft As GGS.BulkEditGridView, gvRight As GGS.BulkEditGridView, delta As HtmlInputHidden)
        Try
            If (gvLeft.SelectedIndex = gvLeft.Rows.Count - 1) Then
                Master.Msg = "ERROR:  cannot move row down..."
            ElseIf (gvLeft.SelectedIndex < 0) Then
                Master.Msg = "Please Select a Row"
            Else
                delta.Value = "1"
                gvLeft.MoveSelectedRowDown()
                gvRight.MoveSelectedRowDown()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Copy Dialog Events"

    ' -===============  COPY SRC ==============
    Private Sub ddlLineNumbersCopySrc_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLineNumbersCopySrc.SelectedIndexChanged
        Try
            LoadStationsCopy(DialogType.CopySrc)
            LoadInstructionSetsCopy(DialogType.CopySrc)
            LoadInstructionsCopy(DialogType.CopySrc)
            GetMCRNumber_ForDialog(DialogType.CopySrc)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub

    Private Sub ddlStationsCopySrc_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStationsCopySrc.SelectedIndexChanged
        Try
            LoadInstructionSetsCopy(DialogType.CopySrc)
            LoadInstructionsCopy(DialogType.CopySrc)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub

    Private Sub ddlInstructionSetsCopySrc_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlInstructionSetsCopySrc.SelectedIndexChanged
        GetMCRNumber_ForDialog(DialogType.CopySrc)
        LoadInstructionsCopy(DialogType.CopySrc)
    End Sub

    Private Sub ddlMCRNumberCopySrc_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMCRNumberCopySrc.SelectedIndexChanged
        LoadInstructionsCopy(DialogType.CopySrc)
    End Sub

    ' -===============  COPY DST ==============
    Private Sub ddlLineNumbersCopyDst_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlLineNumbersCopyDst.SelectedIndexChanged
        Try
            LoadStationsCopy(DialogType.CopyDst)
            LoadInstructionSetsCopy(DialogType.CopyDst)
            LoadInstructionsCopy(DialogType.CopyDst)
            GetMCRNumber_ForDialog(DialogType.CopyDst)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub

    Private Sub ddlStationsCopyDst_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlStationsCopyDst.SelectedIndexChanged
        Try
            LoadInstructionSetsCopy(DialogType.CopyDst)
            LoadInstructionsCopy(DialogType.CopyDst)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub

    Private Sub ddlInstructionSetsCopyDst_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlInstructionSetsCopyDst.SelectedIndexChanged
        GetMCRNumber_ForDialog(DialogType.CopyDst)
        ddlMCRNumberCopyDst.SelectedIndex = 0
        LoadInstructionsCopy(DialogType.CopyDst)
    End Sub

    Private Sub ddlMCRNumberCopyDst_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMCRNumberCopyDst.SelectedIndexChanged
        LoadInstructionsCopy(DialogType.CopyDst)
    End Sub

    ' -===============  COPY UP / DOWN =============
    Private Sub cmdMoveUpCopy_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdMoveUpCopy.Click
        Try
            copyMsg.InnerText = ""
            If lbxInstructionsCopyDst.SelectedIndex >= 0 Then
                lbxInstructionsCopyDst.Items.RemoveAt(lbxInstructionsCopyDst.SelectedIndex)
                copyStepCount.Value = CStr(CInt(copyStepCount.Value) - 1)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub

    Private Sub cmdMoveDownCopy_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdMoveDownCopy.Click
        Try
            Dim cnt As Integer = CInt(copyStepCount.Value)
            copyMsg.InnerText = ""
            If rblCopyMode.SelectedIndex <> -1 Then
                Select Case CInt(rblCopyMode.SelectedItem.Value)
                    Case 0
                        For Each item As ListItem In lbxInstructionsCopySrc.Items
                            'If lbxInstructionsCopyDst.Items.FindByText(item.Text) Is Nothing Then
                                lbxInstructionsCopyDst.Items.Add(item)
                                cnt = cnt + 1
                            'End If
                        Next
                    Case 1
                        Dim item As ListItem = lbxInstructionsCopySrc.SelectedItem
                        item.Selected = False
                        lbxInstructionsCopyDst.Items.Add(item)
                        cnt = cnt + 1
                    Case Else
                        copyMsg.InnerText = "Please select a Copy Mode."
                End Select

                copyStepCount.Value = cnt.ToString()
            Else
                copyMsg.InnerText = "Please select a Copy Mode."
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub

#End Region

#Region "Copy Dialog Methods"

    Private Sub CopyStepsToInstruction()
        Try
            'create the xml document
            Dim xmlDoc As New XmlDocument()
            xmlDoc.AppendChild(xmlDoc.CreateElement("root"))

            'setup the xml element for communicating the source instruction set.
            Dim xmlSource As XmlElement = xmlDoc.CreateElement("source")
            xmlSource.SetAttribute("LineID", ddlLineNumbersCopySrc.SelectedValue)
            xmlSource.SetAttribute("StationID", ddlStationsCopySrc.SelectedValue)
            xmlSource.SetAttribute("ConfigurationID", ddlInstructionSetsCopySrc.SelectedValue)
            xmlSource.SetAttribute("MasterChangeRequestID", ddlMCRNumberCopySrc.SelectedValue)
            ' add to the xml document
            xmlDoc.FirstChild.AppendChild(xmlSource)

            'setup the xml element for communicating the destination instruction set.
            Dim xmlDestination As XmlElement = xmlDoc.CreateElement("destination")
            xmlDestination.SetAttribute("LineID", ddlLineNumbersCopyDst.SelectedValue)
            xmlDestination.SetAttribute("StationID", ddlStationsCopyDst.SelectedValue)
            xmlDestination.SetAttribute("ConfigurationID", ddlInstructionSetsCopyDst.SelectedValue)
            xmlDestination.SetAttribute("MasterChangeRequestID", ddlMCRNumberCopyDst.SelectedValue)
            ' add to the xml document
            xmlDoc.FirstChild.AppendChild(xmlDestination)

            'Get all instruction steps to copy
            Dim xmlSteps As XmlElement = xmlDoc.CreateElement("steps")
            For Each item As ListItem In lbxInstructionsCopyDst.Items
                'create new node
                Dim child As XmlElement = xmlDoc.CreateElement("step")
                child.SetAttribute("Text", item.Text)
                child.SetAttribute("Value", item.Value.ToString())

                'append new node to the steps
                xmlSteps.AppendChild(child)
            Next
            ' add to the xml document
            xmlDoc.FirstChild.AppendChild(xmlSteps)

            'call the database
            CopyInstructionsToDB(xmlDoc)

            'reload all information
            ResetPage(ddlLineNumbersCopyDst.SelectedValue, ddlStationsCopyDst.SelectedValue, ddlInstructionSetsCopyDst.SelectedValue, ddlMCRNumberCopyDst.SelectedValue)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub LoadStationsCopy(type As DialogType)
        Dim lastSelected As Integer
        Dim ddlTempLine As DropDownList = Nothing
        Dim ddlTempStation As DropDownList = Nothing

        Try
            Select Case type
                Case DialogType.CopySrc
                    ddlTempLine = ddlLineNumbersCopySrc
                    ddlTempStation = ddlStationsCopySrc

                Case DialogType.CopyDst
                    ddlTempLine = ddlLineNumbersCopyDst
                    ddlTempStation = ddlStationsCopyDst
            End Select
            lastSelected = ddlTempStation.SelectedIndex
            ddlTempStation.Items.Clear()

            If (ddlLineNumbersCopySrc IsNot Nothing) Then
                If (ddlLineNumbersCopySrc.SelectedIndex > 0) Then
                    ddlTempStation.DataSource = GetStations(CInt(ddlTempLine.SelectedValue))
                    ddlTempStation.DataTextField = "Description"
                    ddlTempStation.DataValueField = "StationID"
                    ddlTempStation.DataBind()
                End If
            End If
            ddlTempStation.Items.Insert(0, New ListItem("Choose a Station", ""))
            If (ddlTempStation.Items.Count >= lastSelected - 1) Then
                ddlTempStation.SelectedIndex = lastSelected
            End If
            copyMsg.InnerText = ""

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub

    Private Sub LoadInstructionSetsCopy(type As DialogType)
        Dim lastSelected As Integer
        Dim ddlTempStation As DropDownList = Nothing
        Dim ddlTempInstructions As DropDownList = Nothing

        Try
            Select Case type
                Case DialogType.CopySrc
                    ddlTempStation = ddlStationsCopySrc
                    ddlTempInstructions = ddlInstructionSetsCopySrc

                Case DialogType.CopyDst
                    ddlTempStation = ddlStationsCopyDst
                    ddlTempInstructions = ddlInstructionSetsCopyDst
            End Select

            If (ddlTempInstructions IsNot Nothing) Then
                lastSelected = ddlTempInstructions.SelectedIndex
                ddlTempInstructions.Items.Clear()

                If (ddlTempStation.SelectedIndex > 0) Then
                    ddlTempInstructions.DataSource = GetInstructionSets(ddlTempStation.SelectedValue)
                    ddlTempInstructions.DataTextField = "Description"
                    ddlTempInstructions.DataValueField = "ConfigurationID"
                    ddlTempInstructions.DataBind()
                End If
                ddlTempInstructions.Items.Insert(0, New ListItem("Choose a Model", ""))
                If (ddlTempInstructions.Items.Count >= lastSelected - 1) Then
                    ddlTempInstructions.SelectedIndex = lastSelected
                End If
            End If

            copyMsg.InnerText = ""

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub

    Private Sub LoadInstructionsCopy(type As DialogType)
        Dim lbxTemp As ListBox = Nothing
        Dim ddlTempLine As DropDownList = Nothing
        Dim ddlTempStation As DropDownList = Nothing
        Dim ddlTempInstruction As DropDownList = Nothing
        Dim ddlTempMCR As DropDownList = Nothing
        Dim strWarning As String = ""

        Try
            Select Case type
                Case DialogType.CopySrc
                    lbxTemp = lbxInstructionsCopySrc
                    ddlTempLine = ddlLineNumbersCopySrc
                    ddlTempStation = ddlStationsCopySrc
                    ddlTempInstruction = ddlInstructionSetsCopySrc
                    ddlTempMCR = ddlMCRNumberCopySrc

                Case DialogType.CopyDst
                    lbxTemp = lbxInstructionsCopyDst
                    ddlTempLine = ddlLineNumbersCopyDst
                    ddlTempStation = ddlStationsCopyDst
                    ddlTempInstruction = ddlInstructionSetsCopyDst
                    ddlTempMCR = ddlMCRNumberCopyDst
            End Select

            If (lbxTemp IsNot Nothing) Then
                lbxTemp.Items.Clear()

                If ddlTempInstruction.SelectedIndex > 0 And (ddlTempMCR.SelectedIndex > 0 Or ddlTempMCR.Enabled) Then
                    lbxTemp.DataSource = GetInstructions(ddlTempLine.SelectedValue, ddlTempStation.SelectedValue, ddlTempInstruction.SelectedValue, ddlTempMCR.SelectedValue)
                    lbxTemp.DataTextField = "Description"
                    lbxTemp.DataValueField = "StepID"
                    lbxTemp.DataBind()

                    If (lbxTemp.Items.Count = 0) Then
                        strWarning = "No Steps found for "
                        If ddlTempLine.SelectedItem IsNot Nothing Then
                            strWarning = strWarning + ddlTempLine.SelectedItem.Text + ", "
                        End If
                        If ddlTempStation.SelectedItem IsNot Nothing Then
                            strWarning = strWarning + ddlTempStation.SelectedItem.Text + ", "
                        End If
                        If ddlTempInstruction.SelectedItem IsNot Nothing Then
                            strWarning = strWarning + ddlTempInstruction.SelectedItem.Text + ", "
                        End If
                        If ddlTempMCR.SelectedItem IsNot Nothing Then
                            strWarning = strWarning + ddlTempMCR.SelectedItem.Text
                        End If
                        copyMsg.InnerText = strWarning
                    Else
                        copyMsg.InnerText = ""
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            copyMsg.InnerText = ex.ToString()
        End Try
    End Sub


#End Region

End Class