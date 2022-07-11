Imports System.Data.SqlClient

Public Class ODSRevisionLog
    Inherits System.Web.UI.Page

    ' this is the special case keyword used to dentoe which status is the 'Active' one.
    Private Const KEYWORD_ACTIVE = "Active"


    Enum gvMCRGridColumns
        printerColumn
        rbSelector
        IsActiveOnClient
        MCRNumber
        MCRDate
        MCRStatus
        MCRDescription
        StationID
        ModifiedDT
        ModifiedBy
    End Enum

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                txtMCRDateFrom.Text = "01/01/2010"
                txtMCRDateTo.Text = Date.Today().ToString("MM/dd/yyyy")
                InitializePageDropDownLists()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        EnableControls()
    End Sub


    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        gvMCRGrid.DataBind()
    End Sub

    Private Sub cmdSetActiveMCR_Click(sender As Object, e As System.EventArgs) Handles cmdSetActiveMCR.Click
        Try
            SetMCRToActiveOnClient(hidMcrID.Value, Page.User.Identity.Name)
            gvMCRGrid.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdEditMCR_Click(sender As Object, e As System.EventArgs) Handles cmdEditMCR.Click
        Try
            SaveMCREdit()
            gvMCRGrid.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdEditStationMCR_Click(sender As Object, e As System.EventArgs) Handles cmdEditStationMCR.Click
        Try
            SaveStationMCREdit()
            gvMCRGrid.DataBind()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub gvMCRGrid_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMCRGrid.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim mcrId As String = gvMCRGrid.DataKeys(e.Row.RowIndex).Value.ToString()

                ' get a reference to controls
                Dim mcrNumber As Label = CType(e.Row.FindControl("lblMCRNumber"), Label)
                mcrNumber.Text = CStr(DataBinder.Eval(e.Row.DataItem, "MCRNumber"))
                mcrNumber.ToolTip = "MCRID [" & mcrId.ToString() & "] "


                Dim radioButtonOutput As Literal = CType(e.Row.FindControl("rbSelector"), Literal)
                Dim isActiveOnClient As Boolean = CType(DataBinder.Eval(e.Row.DataItem, "IsActiveOnClient"), Boolean)
                SetActiveRadioButton(e, radioButtonOutput, isActiveOnClient, mcrId)

                Dim gvDetails As GridView = TryCast(e.Row.FindControl("gvDetails"), GridView)
                gvDetails.DataSource = GetMCRDetail(mcrId)
                gvDetails.DataBind()

                Dim printerImage As ImageButton = CType(e.Row.FindControl("imgPrint"), ImageButton)
                printerImage.OnClientClick = "SetMCRIDandOpenPrintDialog(" + mcrId + ", '" + mcrNumber.Text + "'); return false;"
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region


#Region "Methods"

    Private Sub EnableControls()
        Try
            Master.Secure(cmdPrint)
            Master.Secure(cmdRefresh)
            Master.Secure(cmdEditMCR)
            Master.Secure(cmdSetActiveMCR)
            Master.Secure(chkIncludeWatermark)

            If (ddlLineNumbers.SelectedIndex > 0) Then
                ddlVehicleModel.Enabled = True
            Else
                ddlVehicleModel.Enabled = False
            End If

            If (ddlVehicleModel.SelectedIndex > 0) Then
                ddlMCRStatus.Enabled = True
            Else
                ddlMCRStatus.Enabled = False
            End If


            If (ddlLineNumbers.SelectedIndex > 0) And (ddlVehicleModel.SelectedIndex > 0) Then
                gvMCRGrid.Enabled = True
                cmdRefresh.Enabled = cmdRefresh.Enabled And True
                cmdPrint.Enabled = cmdPrint.Enabled And True
            Else
                gvMCRGrid.Enabled = False
                cmdRefresh.Enabled = False
                cmdPrint.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub InitializePageDropDownLists()
        Try
            LoadLines()
            LoadVehicleModels()
            LoadMCRStatuses()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadLines()
        Try
            Dim ds As DataSet = DA.GetDataSet("SELECT LineID, LineName FROM dbo.tblSGLines")

            ' Lines for the drop-down list on the main web page
            ddlLineNumbers.Items.Clear()
            ddlLineNumbers.DataSource = ds
            ddlLineNumbers.DataTextField = "LineName"
            ddlLineNumbers.DataValueField = "LineID"
            ddlLineNumbers.DataBind()
            ddlLineNumbers.Items.Insert(0, New ListItem("Choose a Line", ""))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadVehicleModels()
        Try
            Dim parameters As New List(Of SqlParameter)
            Dim ds As DataSet = Nothing

            ddlVehicleModel.Items.Clear()

            parameters.Add(New SqlParameter("@StationID", -1))
            parameters.Add(New SqlParameter("@hasInstructions", DBNull.Value))

            ds = DA.GetDataSet("[ods].[procSelectInstructionSets]", parameters, "")

            ddlVehicleModel.DataSource = ds
            ddlVehicleModel.DataTextField = "Description"
            ddlVehicleModel.DataValueField = "ConfigurationID"
            ddlVehicleModel.DataBind()
            ddlVehicleModel.Items.Insert(0, New ListItem("Choose a Model", ""))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadMCRStatuses()
        Try
            Dim parameters As New List(Of SqlParameter)
            Dim ds As DataSet = Nothing

            ddlMCRStatus.Items.Clear()

            parameters.Add(New SqlParameter("@ConfigurationTypeID", 5))

            ds = DA.GetDataSet("[ods].[procSelectConfigurations]", parameters, "")

            ddlMCRStatus.DataSource = ds
            ddlMCRStatus.DataTextField = "Description"
            ddlMCRStatus.DataValueField = "ConfigurationID"
            ddlMCRStatus.DataBind()
            'ddlMCRStatus.Items.Insert(0, New ListItem("Choose a Status", "0"))
            ddlMCRStatus.Items.Insert(0, New ListItem("-- All --", "-1"))

            ddlNewMCRStatus.Items.Clear()
            ddlNewMCRStatus.DataSource = ds
            ddlNewMCRStatus.DataTextField = "Description"
            ddlNewMCRStatus.DataValueField = "ConfigurationID"
            ddlNewMCRStatus.DataBind()


            ddlNewActiveMCRStatus.Items.Clear()
            ddlNewActiveMCRStatus.DataSource = ds
            ddlNewActiveMCRStatus.DataTextField = "Description"
            ddlNewActiveMCRStatus.DataValueField = "ConfigurationID"
            ddlNewActiveMCRStatus.DataBind()
            ddlNewActiveMCRStatus.Items.Remove(ddlNewActiveMCRStatus.Items.FindByText(KEYWORD_ACTIVE))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SetActiveRadioButton(ByRef e As System.Web.UI.WebControls.GridViewRowEventArgs, ByRef radioButtonOutput As Literal, ByRef isActiveOnClient As Boolean, ByRef mcrId As String)
        Try
            If (radioButtonOutput IsNot Nothing) Then

                ' Output the radiobutton markup except for the "checked" attribute
                radioButtonOutput.Text = String.Format("<input id='RowIndex{0}' type='radio' name='rbgSelectorGroup' value='{1}' ", e.Row.RowIndex, mcrId)

                If (isActiveOnClient) Then
                    radioButtonOutput.Text += " checked='checked'"
                    e.Row.CssClass = "IsActiveOnClient"
                End If
                If ddlMCRStatus.SelectedIndex <= 0 Then
                    radioButtonOutput.Text += " DISABLED='DISABLED'"
                End If
                radioButtonOutput.Text += " />"

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function GetMCRDetail(mcrID As String) As DataSet
        Dim ds As DataSet = Nothing
        Try
            Dim parameters As New List(Of SqlParameter)
            parameters.Add(New SqlParameter("@MasterChangeRequestID", mcrID))
            parameters.Add(New SqlParameter("@DateFrom", txtMCRDateFrom.Text))
            parameters.Add(New SqlParameter("@DateTo", txtMCRDateTo.Text))
            ds = DA.GetDataSet("[ods].[procGetInstructionRevisionLog_Detail]", parameters, "")
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return ds
    End Function

    Private Sub SetMCRToActiveOnClient(MCRID As String, modifiedBy As String)
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)

            oSqlParameter = New SqlParameter("@MasterChangeRequestID", SqlDbType.Int)
            oSqlParameter.Value = CInt(MCRID)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ModifiedBy", SqlDbType.VarChar, 50)
            oSqlParameter.Value = modifiedBy
            colParameters.Add(oSqlParameter)

            DA.ExecSP("[ods].[procUpdateMasterChangeRequest_SetIsActiveOnClient]", colParameters)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SaveMCREdit()
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            Dim status As String = ""
            Dim message As String = ""

            oSqlParameter = New SqlParameter("@masterChangeRequestID", SqlDbType.Int)
            oSqlParameter.Value = CInt(hidMcrID.Value)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@lineID", SqlDbType.Int)
            oSqlParameter.Value = ddlLineNumbers.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@configurationID", SqlDbType.Int)
            oSqlParameter.Value = ddlVehicleModel.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@mcrNumber", SqlDbType.VarChar, 10)
            oSqlParameter.Value = txtNewMCRNumber.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@mcrDate", SqlDbType.DateTime)
            oSqlParameter.Value = txtNewMCRDate.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@MCRDescription", SqlDbType.NVarChar, 100)
            oSqlParameter.Value = txtNewMCRDescription.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationMCRNumber", SqlDbType.NVarChar, 10)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationChangeDescription", SqlDbType.NVarChar, 400)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@mcrStatus_ConfigurationID", SqlDbType.Int)
            oSqlParameter.Value = ddlNewMCRStatus.SelectedValue
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@isActiveOnClient", SqlDbType.Bit)
            oSqlParameter.Value = hidActiveOnClient.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 50)
            oSqlParameter.Value = Page.User.Identity.Name
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@copyFromMCRID", SqlDbType.Int)
            oSqlParameter.Value = DBNull.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@changeCurrentActiveStatusTo", SqlDbType.Int)
            If (ddlNewMCRStatus.SelectedItem.Text = KEYWORD_ACTIVE) Then
                oSqlParameter.Value = ddlNewActiveMCRStatus.SelectedValue
            Else
                oSqlParameter.Value = DBNull.Value
            End If
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
                    End If
                End With
            Next

            If (status.ToUpper() = "TRUE") Then
                Master.tMsg("Update", "Master Change Request Has Been Updated Successfully.")
            Else
                Master.tMsg("Update", "Error - Master Change Request Update Failed. <BR>Error message: " + message + "<BR>Status message: " + status)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SaveStationMCREdit()
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            Dim status As String = ""
            Dim message As String = ""

            oSqlParameter = New SqlParameter("@masterChangeRequestID", SqlDbType.Int)
            oSqlParameter.Value = CInt(hidMcrID.Value)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = hidStationID.Value
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationMcrNumber", SqlDbType.VarChar, 10)
            oSqlParameter.Value = txtEditStationMCRNumber.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@stationChangeDescription", SqlDbType.NVarChar, 400)
            oSqlParameter.Value = txtEditDescriptionOfChange.Text
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ModifiedBy", SqlDbType.NVarChar, 50)
            oSqlParameter.Value = Page.User.Identity.Name
            colParameters.Add(oSqlParameter)


            ' output params
            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)


            colOutput = DA.ExecSP("[ods].[procUpdateStationMCRNumber]", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status.ToUpper() = "TRUE") Then
                Master.tMsg("Update", "Station MCR Number Has Been Updated Successfully.")
            Else
                Master.tMsg("Update", "Error - Station MCR Number Update Failed. <BR>Error message: " + message + "<BR>Status message: " + status)
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

End Class