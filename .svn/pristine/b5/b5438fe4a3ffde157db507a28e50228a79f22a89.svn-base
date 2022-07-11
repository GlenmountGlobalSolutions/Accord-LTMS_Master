Imports System.Data.SqlClient

Public Class ArchiveLotTraceDataTransfer
    Inherits System.Web.UI.Page

#Region "Event Handlers"

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            Master.Secure(cmdTransfer)
            Master.Secure(cmdViewData)
            cmdViewData.Enabled = True
            cmdTransfer.Enabled = True

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdViewData_Click(sender As Object, e As System.EventArgs) Handles cmdViewData.Click
        Try
            If ValidSSN(txtSSN.Text) Then
                If GetProductOperationRequirements() AndAlso GetProdOperHistoryFrom() Then
                    cmdTransfer.Visible = True
                Else
                    cmdTransfer.Visible = False
                    Master.tMsg("Invalid SSN", "Invalid SSN")

                    dgProductOperationResults.DataSource = Nothing
                    dgProductOperationResults.DataBind()

                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdTransfer_Click(sender As Object, e As System.EventArgs) Handles cmdTransfer.Click
        Try
            If ValidSSN(txtSSN.Text) AndAlso ValidateSSN(txtSSN.Text) Then

                'pass in the SSN, pass in the Remote Database name

                Dim dbName As String = ""
                Dim password As String = ""
                Dim serverName As String = ""
                Dim userID As String = ""
                Dim oSqlParameter As SqlParameter
                Dim colParameters As New List(Of SqlParameter)
                Dim colOutput As List(Of SqlParameter)
                Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("RemoteConnStringSql").ConnectionString
                ParseConnectionString(ConnectionString, dbName, password, serverName, userID)

                If ConnectionString IsNot Nothing AndAlso ConnectionString.Length > 0 Then
                    If ValidSSN(serverName) And ValidSSN(dbName) Then
                        oSqlParameter = New SqlParameter("@ServerName", SqlDbType.VarChar, 80)
                        oSqlParameter.Value = serverName
                        colParameters.Add(oSqlParameter)

                        oSqlParameter = New SqlParameter("@DatabaseName", SqlDbType.VarChar, 80)
                        oSqlParameter.Value = dbName
                        colParameters.Add(oSqlParameter)

                        oSqlParameter = New SqlParameter("@ProductID", SqlDbType.VarChar, 80)
                        oSqlParameter.Value = txtSSN.Text
                        colParameters.Add(oSqlParameter)


                        colOutput = DA.ExecSP("procArchiveLotTransfer", colParameters)

                        Master.tMsg("Transfer Successful.", "Transfer from Archive to Production Succeeded for Lot:" + txtSSN.Text)
                    Else
                        Master.tMsg("Error in Transfer", "Remote SQL Connection String Invalid, check web.config. Transfer failed for " + txtSSN.Text)
                    End If
                Else
                    Master.tMsg("Error in Transfer", "Remote SQL Connection String Invalid, check web.config. Transfer failed for " + txtSSN.Text)
                End If

            Else
                Master.tMsg("Error in Transfer", "Invalid SSN, cannot Transfer from Archive")
                dgProductOperationResults.Controls.Clear()
                cmdTransfer.Visible = False

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region

#Region "Methods"

    Public Function ValidSSN(ByVal SSN As String) As Boolean
        Try
            Select Case SSN
                Case Nothing
                    Return False
                Case ""
                    Return False
                Case Else
                    Return True
            End Select
        Catch
            Return False
        End Try
    End Function

    Private Function ValidateSSN(SSN As String) As Boolean
        Dim lds As DataSet
        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim bResult As Boolean = False

        Try
            'Procedure procGetOperationRequirements  @Data varchar(48) AS
            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@Data", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = SSN
            colParms.Add(prmNext)
            lds = DA.GetDataSet("procGetOperationRequirements", colParms)
            If lds.Tables.Count = 0 Then
                cmdTransfer.Enabled = False
                bResult = False
            Else
                bResult = True
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function GetProductOperationRequirements() As Boolean
        Dim colParms As New List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim bResult As Boolean = False
        Dim ds As DataSet
        Try
            'Procedure procGetOperationRequirements  @Data varchar(48) AS

            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@Data", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = txtSSN.Text
            colParms.Add(prmNext)
            ds = DA.GetDataSet("procGetOperationRequirements", colParms)
            ds.Tables(0).Columns.Add("Results")
            dgProductOperationsRequired.DataSource = ds
            dgProductOperationsRequired.DataBind()
            bResult = True

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Private Function GetProdOperHistoryFrom() As Boolean
        Dim bResult As Boolean = False

        Dim colParms As New List(Of SqlParameter)
        Dim colOutParms As List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim strOutputResult As String = "Record Not Found"
        Dim ds As New DataSet
        Dim i As Integer

        Try
            ds = CType(dgProductOperationsRequired.DataSource, DataSet)

            If ds.Tables IsNot Nothing AndAlso ds.Tables(0).Rows IsNot Nothing Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    'Procedure procGetOperationHistory  
                    '@Data varchar(48) , 'serial number from txtbox
                    '@OperationID Varchar(80),
                    '@PerformedDT DateTime Out, 
                    '@Operation Varchar(80) Out , 
                    '@OperationResults Varchar(80) Out , 
                    '@VerificationCriteria Varchar(80) Out , 
                    '@VerificationResults Varchar(80) Out,  
                    '@Source Varchar(80) Out ,'
                    '@Status Varchar(80) Out,
                    '@OPProductIdentifier Varchar(80) Out AS

                    prmNext = New Data.SqlClient.SqlParameter("@Data", SqlDbType.VarChar, 48)
                    prmNext.Direction = ParameterDirection.Input
                    prmNext.Value = txtSSN.Text.ToUpper
                    colParms.Add(prmNext)

                    prmNext = New Data.SqlClient.SqlParameter("@OperationID", SqlDbType.VarChar, 80)
                    prmNext.Direction = ParameterDirection.Input
                    prmNext.Value = dr.Item(0).ToString()
                    colParms.Add(prmNext)
                    '------------------------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@PerformedDT", SqlDbType.DateTime)
                    prmNext.Direction = ParameterDirection.Output
                    colParms.Add(prmNext)
                    '------------------------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@Operation", SqlDbType.VarChar, 80)
                    prmNext.Direction = ParameterDirection.Output
                    colParms.Add(prmNext)
                    '------------------------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@OperationResults", SqlDbType.VarChar, 80)
                    prmNext.Direction = ParameterDirection.Output
                    colParms.Add(prmNext)
                    '------------------------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@VerificationCriteria", SqlDbType.VarChar, 80)
                    prmNext.Direction = ParameterDirection.Output
                    colParms.Add(prmNext)
                    '------------------------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@VerificationResults", SqlDbType.VarChar, 80)
                    prmNext.Direction = ParameterDirection.Output
                    colParms.Add(prmNext)
                    '------------------------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@Source", SqlDbType.VarChar, 80)
                    prmNext.Direction = ParameterDirection.Output
                    colParms.Add(prmNext)
                    '------------------------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@OPProductIdentifier", SqlDbType.VarChar, 80)
                    prmNext.Direction = ParameterDirection.Output
                    colParms.Add(prmNext)
                    '------------------------------------------------------
                    prmNext = New Data.SqlClient.SqlParameter("@Status", SqlDbType.VarChar, 80)
                    prmNext.Direction = ParameterDirection.Output
                    colParms.Add(prmNext)
                    '-----------------------------------------------------
                    colOutParms = DA.ExecSPRemote("procGetOperationHistory", colParms, 30, True)

                    For Each oParameter In colOutParms
                        With oParameter
                            If .Direction = ParameterDirection.Output And .ParameterName = "@OperationResults" Then
                                strOutputResult = oParameter.Value.ToString()
                                Exit For
                            End If
                        End With
                    Next

                    ds.Tables(0).Rows(i)(2) = strOutputResult
                    strOutputResult = "Record Not Found"   'reset to default

                    i = i + 1
                    colParms.Clear()
                    colOutParms.Clear()
                Next
                'ds.Tables(0).Columns(0).Dispose()
                dgProductOperationResults.DataSource = ds
                dgProductOperationResults.DataBind()
                bResult = True
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Public Sub ParseConnectionString(ByVal s As String, ByRef dbName As String, ByRef password As String, ByRef servername As String, ByRef userId As String)
        Try
            'retrieve and parse the database login parameters into separate variables and apply login to Report
            Dim sp() As String
            sp = s.Split(CChar(";"))

            For Each v As String In sp
                Dim sv As String = v.Substring(0, v.IndexOf("="))
                Select Case sv
                    Case "data source"
                        servername = v.Substring(v.IndexOf("=") + 1, v.Length - v.IndexOf("=") - 1)
                    Case "initial catalog"
                        dbName = v.Substring(v.IndexOf("=") + 1, v.Length - v.IndexOf("=") - 1)
                    Case "password"
                        password = v.Substring(v.IndexOf("=") + 1, v.Length - v.IndexOf("=") - 1)
                    Case "user id"
                        userId = v.Substring(v.IndexOf("=") + 1, v.Length - v.IndexOf("=") - 1)
                End Select
            Next
        Catch
        End Try
    End Sub

#End Region



End Class