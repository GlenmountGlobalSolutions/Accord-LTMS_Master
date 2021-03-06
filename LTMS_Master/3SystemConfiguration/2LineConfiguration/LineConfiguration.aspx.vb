Imports System.Data.SqlClient

Public Class LineConfiguration
    Inherits System.Web.UI.Page

    Enum SGLineColumn
        LineID = 0
        LineName
        LineNumber
        LineTypeID
        MultiPLC
        Upgrade2015
        EDSFilePath
    End Enum

#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                LoadLinesListBox()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        EnableControls()
    End Sub


    Protected Sub lbLines_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbLines.SelectedIndexChanged
        LoadLineDetails()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        SaveLine()
        LoadLineDetails()
    End Sub

    Private Sub cmdNew_Click(sender As Object, e As System.EventArgs) Handles cmdNew.Click
        InsertLine()
    End Sub

#End Region

#Region "Subs and Functions"


    Private Sub EnableControls()
        Try
            Master.Secure(cmdNew)
            Master.Secure(cmdSave)

            If (lbLines.SelectedIndex < 0) Then
                cmdSave.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub LoadLinesListBox()
        Try
            lbLines.DataSource = DA.GetDataSet("procSelectSGLines", Nothing, Nothing)
            lbLines.DataValueField = "LineID"
            lbLines.DataTextField = "LineName"
            lbLines.DataBind()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub LoadLineDetails()
        Dim parameters As New List(Of SqlParameter)
        Dim ds As DataSet = Nothing
        Dim li As ListItem = Nothing

        Try
            If (lbLines.SelectedValue.Length > 0) Then
                parameters.Add(New SqlParameter("@lineID", lbLines.SelectedValue))
                ds = DA.GetDataSet("procSelectSGLines", parameters, Nothing)

                If (DA.IsNotEmpty(ds)) Then
                    txtLineName.Text = ds.Tables(0).Rows(0).Item(SGLineColumn.LineName).ToString()
                    txtLineName.OldText = txtLineName.Text

                    txtEDSFilePath.Text = ds.Tables(0).Rows(0).Item(SGLineColumn.EDSFilePath).ToString()
                    txtEDSFilePath.OldText = txtEDSFilePath.Text

                    lblLineID.Text = ds.Tables(0).Rows(0).Item(SGLineColumn.LineID).ToString()
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub InsertLine()
        Dim oSqlParameter As SqlParameter
        Dim parameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim status As String = ""
        Dim message As String = ""
        Dim newLineID As Integer

        Try
            If (txtNewLineName.Text.Length = 0) Then
                Master.tMsg("Alert", "New Line [" + txtNewLineName.Text + "] has not been created.", True, "Red")
            Else
                parameters.Add(New SqlParameter("@lineName", txtNewLineName.Text))
                parameters.Add(New SqlParameter("@edsFilePath", txtNewEDSFilePath.Text))

                oSqlParameter = New SqlParameter("@newLineID", SqlDbType.Int)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("[dbo].[procInsertSGLines]", parameters)

                For Each oParameter In colOutput
                    With oParameter
                        If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                            status = oParameter.Value.ToString()
                        ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                            message = oParameter.Value.ToString()
                        ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@newLineID" Then
                            newLineID = Convert.ToInt32(oParameter.Value)
                        End If
                    End With
                Next

                LoadLinesListBox()

                If ((status.ToUpper() = "TRUE")) Then
                    Master.Msg = "New Line [" + txtNewLineName.Text + "] has been created."
                    lbLines.SelectedIndex = lbLines.Items.IndexOf(lbLines.Items.FindByValue(newLineID.ToString()))
                Else
                    Master.Msg = "New Line [" + txtNewLineName.Text + "] has not been created.  " + message
                End If

                LoadLineDetails()

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub SaveLine()
        Dim oSqlParameter As SqlParameter
        Dim parameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim status As String = ""
        Dim message As String = ""

        Try
            If (lbLines.SelectedIndex < 0) Then
                Master.tMsg("Alert", "There is nothing to save. No Line is selected!", True, "Red")

            ElseIf (txtLineName.OldText = txtLineName.Text) And (txtEDSFilePath.OldText = txtEDSFilePath.Text) Then

                Master.tMsg("Alert", "No changes were made; nothing to save.", True, "Red")
            Else
                parameters.Add(New SqlParameter("@lineID", lbLines.SelectedValue))
                parameters.Add(New SqlParameter("@lineName", txtLineName.Text))
                parameters.Add(New SqlParameter("@edsFilePath", txtEDSFilePath.Text))


                oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
                oSqlParameter.Direction = ParameterDirection.Output
                parameters.Add(oSqlParameter)

                colOutput = DA.ExecSP("[dbo].[procUpdateSGLines]", parameters)

                For Each oParameter In colOutput
                    With oParameter
                        If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                            status = oParameter.Value.ToString()
                        ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                            message = oParameter.Value.ToString()
                        End If
                    End With
                Next

                If ((status.ToUpper() = "TRUE")) Then
                    Master.Msg = "Line changes have been saved."
                    lbLines.SelectedIndex = lbLines.Items.IndexOf(lbLines.Items.FindByValue(lbLines.SelectedValue.ToString()))
                Else
                    Master.Msg = "Line changes have not been saved.  " + message
                End If

                LoadLinesListBox()

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

End Class