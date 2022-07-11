Imports System.Data.SqlClient
Imports Telerik.Web.UI

Namespace DataAccess

    Public Class BroadcastPointID

        Friend Shared Function GetBroadcastPointIDs() As DataSet
            Return GetBroadcastPointIDs("", Utility.GetClientIPAddress)
        End Function

        Friend Shared Function GetBroadcastPointIDs(ByVal strBroadcastPointID As String, ByVal ipAddress As String) As DataSet
            Dim ds As New DataSet()
            Try
                Dim cmd As SqlCommand = PrepareCommand_GetBroadcastPointIDs(strBroadcastPointID, ipAddress)

                Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString)
                    cmd.Connection = conn
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(ds)
                        cmd.Dispose()
                    End Using
                End Using
            Catch e1 As Exception
                Throw
            End Try
            Return ds
        End Function

        Friend Shared Function PrepareCommand_GetBroadcastPointIDs(ByVal strBroadcastPointID As String, ipAddress As String) As SqlCommand
            Dim cmd As New SqlCommand()
            Dim prm As SqlParameter

            Try

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "procGetBroadcastPoints"

                prm = New SqlParameter()
                prm.ParameterName = "@RETURN_VALUE"
                prm.SqlDbType = SqlDbType.Int
                prm.Precision = CByte(Integer.Parse("0"))
                prm.Size = Integer.Parse("0")
                prm.Scale = CByte(Integer.Parse("0"))
                prm.SourceColumn = Nothing
                prm.Direction = ParameterDirection.ReturnValue
                cmd.Parameters.Add(prm)

                prm = New SqlParameter()
                prm.ParameterName = "@BroadcastPointID"
                prm.SqlDbType = SqlDbType.VarChar
                prm.Precision = CByte(Integer.Parse("0"))
                prm.Size = Integer.Parse("4")
                prm.Scale = CByte(Integer.Parse("0"))
                prm.SourceColumn = Nothing
                prm.Direction = ParameterDirection.Input

                If (strBroadcastPointID.Trim().Length = 0) Then
                    prm.Value = DBNull.Value
                Else
                    prm.Value = strBroadcastPointID.Trim()
                End If
                cmd.Parameters.Add(prm)

                prm = New SqlParameter()
                prm.ParameterName = "@IPAddress"
                prm.SqlDbType = SqlDbType.VarChar
                prm.Precision = CByte(Integer.Parse("0"))
                prm.Size = Integer.Parse("80")
                prm.Scale = CByte(Integer.Parse("0"))
                prm.SourceColumn = Nothing
                prm.Direction = ParameterDirection.Input

                If (ipAddress.Trim().Length = 0) Then
                    prm.Value = DBNull.Value
                Else
                    prm.Value = ipAddress.Trim()
                End If
                cmd.Parameters.Add(prm)

            Catch e1 As Exception
                Throw
            End Try
            Return cmd
        End Function

        Friend Shared Sub Set_ddlBroadcastPoint_Cookie(ByVal ddlBroadcastPointID As String, ByRef theResponse As HttpResponse)
            theResponse.Cookies("defaultBroadcastPointID").Value = ddlBroadcastPointID
            theResponse.Cookies("defaultBroadcastPointID").Expires = DateTime.Now.AddYears(1)
        End Sub

        Friend Shared Sub Load_ddlBroadcastPointID(ByRef ddlBroadcastPointID As DropDownList, ByRef rqst As HttpRequest, ByRef srvr As HttpServerUtility)
            Dim ds As DataSet
            Dim defaultValue As String = ""

            Try
                ddlBroadcastPointID.Items.Clear()

                ds = DataAccess.BroadcastPointID.GetBroadcastPointIDs()
                If (DA.IsDSEmpty(ds)) Then
                    ddlBroadcastPointID.Items.Insert(0, "")
                Else
                    ddlBroadcastPointID.DataSource = ds
                    ddlBroadcastPointID.DataTextField = "Description"
                    ddlBroadcastPointID.DataValueField = "BroadcastPointID"
                    ddlBroadcastPointID.DataBind()
                    ddlBroadcastPointID.Items.Insert(0, My.Resources.txtBroadcastPointSelect)

                    'Check for a cookie to determine the last broadcastPoint selected.
                    ' if last broadcast point was not found in the cookie, look for it the result set.
                    ' default from result set is based on the ip address stored in [tblStationParameters]

                    If rqst.Cookies("defaultBroadcastPointID") IsNot Nothing Then
                        defaultValue = srvr.HtmlEncode(rqst.Cookies("defaultBroadcastPointID").Value)
                    Else
                        For Each r As DataRow In ds.Tables(0).Rows()
                            If (r("defaultSelection").ToString = "1") Then
                                defaultValue = r("BroadcastPointID").ToString
                            End If
                        Next
                    End If

                    If defaultValue.Length > 0 Then
                        ddlBroadcastPointID.SelectedValue = defaultValue
                    End If
                End If

            Catch ex As Exception
                Throw
            End Try
        End Sub

        Friend Shared Sub Load_ddlBroadcastPointID(ByRef ddlBroadcastPointID As RadComboBox, ByRef rqst As HttpRequest, ByRef srvr As HttpServerUtility)
            Dim ds As DataSet
            Dim defaultValue As String = ""

            Try
                ddlBroadcastPointID.Items.Clear()

                ds = DataAccess.BroadcastPointID.GetBroadcastPointIDs()
                If (DA.IsDSEmpty(ds)) Then
                    ddlBroadcastPointID.Items.Insert(0, "")
                Else
                    ddlBroadcastPointID.DataSource = ds
                    ddlBroadcastPointID.DataTextField = "Description"
                    ddlBroadcastPointID.DataValueField = "BroadcastPointID"
                    ddlBroadcastPointID.DataBind()
                    ddlBroadcastPointID.Items.Insert(0, My.Resources.txtBroadcastPointSelect)

                    'Check for a cookie to determine the last broadcastPoint selected.
                    ' if last broadcast point was not found in the cookie, look for it the result set.
                    ' default from result set is based on the ip address stored in [tblStationParameters]

                    If rqst.Cookies("defaultBroadcastPointID") IsNot Nothing Then
                        defaultValue = srvr.HtmlEncode(rqst.Cookies("defaultBroadcastPointID").Value)
                    Else
                        For Each r As DataRow In ds.Tables(0).Rows()
                            If (r("defaultSelection").ToString = "1") Then
                                defaultValue = r("BroadcastPointID").ToString
                            End If
                        Next
                    End If

                    If defaultValue.Length > 0 Then
                        ddlBroadcastPointID.SelectedValue = defaultValue
                    End If
                End If

            Catch ex As Exception
                Throw
            End Try
        End Sub
    End Class

End Namespace
