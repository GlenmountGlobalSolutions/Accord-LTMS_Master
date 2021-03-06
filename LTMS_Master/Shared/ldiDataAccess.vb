Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices

Module DA
    'added this overloaded function to allow for database connection to be set to archive or production database

    Friend Function GetRemoteDataSet(ByVal p_strSQL As String, _
                        Optional ByVal p_strDataTableName As String = "", Optional ByVal p_strRemoteDBConnectionString As Boolean = False) As DataSet
        Dim strCon As String

        If p_strRemoteDBConnectionString = True Then
            strCon = ConfigurationManager.ConnectionStrings("RemoteConnStringSql").ConnectionString
        Else
            strCon = ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString
        End If

        Dim oConn As New SqlConnection(strCon)
        Dim oCommand As New SqlCommand
        Dim oDataAdapter As New SqlDataAdapter
        Dim oDataSet As New DataSet

        Try
            oConn.Open()

            oCommand.CommandText = p_strSQL
            oCommand.Connection = oConn

            oDataAdapter.SelectCommand = oCommand

            If Len(p_strDataTableName) <> 0 Then
                oDataAdapter.Fill(oDataSet, p_strDataTableName)
            Else
                oDataAdapter.Fill(oDataSet)
            End If

            Return oDataSet

        Finally
            oConn.Close()

            oCommand.Dispose()
            oDataAdapter.Dispose()
            oConn.Dispose()
            oDataSet.Dispose()
        End Try
    End Function

    Friend Function ExecSPRemote(ByVal p_strStoredProcedureName As String, _
                   ByVal p_colParameters As List(Of SqlParameter), _
                   ByVal intCommandTimeout As Integer, _
                   Optional ByVal p_strRemoteDBConnectionString As Boolean = False) As List(Of SqlParameter)
        'Returns the output parameters of the SP

        Dim strCon As String

        If p_strRemoteDBConnectionString = True Then
            strCon = ConfigurationManager.ConnectionStrings("RemoteConnStringSql").ConnectionString
        Else
            strCon = ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString
        End If

        Dim oConn As New SqlConnection(strCon)
        Dim oCommand As New SqlCommand
        Dim oParameter As SqlParameter
        Dim colOutputParameters As New List(Of SqlParameter)
        Dim retval As Integer

        Try
            oConn.Open()
            oCommand.CommandTimeout = intCommandTimeout
            oCommand.CommandText = p_strStoredProcedureName
            oCommand.CommandType = CommandType.StoredProcedure
            oCommand.Connection = oConn

            'Add the parameters as passed in 
            For Each oParameter In p_colParameters
                oCommand.Parameters.Add(oParameter)
            Next

            retval = oCommand.ExecuteNonQuery()

            'Return the output parameters of this SP
            For Each oParameter In p_colParameters
                With oParameter
                    If .Direction = ParameterDirection.Output Then
                        colOutputParameters.Add(oCommand.Parameters(.ParameterName))
                    End If
                End With
            Next

            Return colOutputParameters

        Finally
            oConn.Close()
            oCommand.Dispose()
            oConn.Dispose()
        End Try
    End Function


    Friend Function GetDataSet(ByVal p_strSQL As String, _
               Optional ByVal p_strDataTableName As String = "") As DataSet

        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString)
        Dim oCommand As New SqlCommand
        Dim oDataAdapter As New SqlDataAdapter
        Dim oDataSet As New DataSet

        Try
            oConn.Open()
            oCommand.CommandTimeout = 300
            oCommand.CommandText = p_strSQL
            oCommand.Connection = oConn

            oDataAdapter.SelectCommand = oCommand

            If Len(p_strDataTableName) <> 0 Then
                oDataAdapter.Fill(oDataSet, p_strDataTableName)
            Else
                oDataAdapter.Fill(oDataSet)
            End If

            Return oDataSet

        Finally
            oConn.Close()

            oCommand.Dispose()
            oDataAdapter.Dispose()
            oConn.Dispose()
            oDataSet.Dispose()
        End Try
    End Function

    Friend Function GetDataSet(ByVal p_strStoredProcedureName As String, _
               ByVal p_colParameters As List(Of SqlParameter), _
               Optional ByVal p_strDataTableName As String = "") As DataSet

        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString)
        Dim oCommand As New SqlCommand
        Dim oDataAdapter As New SqlDataAdapter
        Dim oDataSet As New DataSet
        Dim oParameter As SqlParameter
        Try
            oConn.Open()

            oCommand.CommandText = p_strStoredProcedureName
            oCommand.CommandType = CommandType.StoredProcedure
            oCommand.Connection = oConn
            oCommand.CommandTimeout = 300
            'Add the parameters as passed in 

            If (p_colParameters IsNot Nothing) Then
                For Each oParameter In p_colParameters
                    oCommand.Parameters.Add(oParameter)
                Next
            End If

            oDataAdapter.SelectCommand = oCommand

            If Len(p_strDataTableName) <> 0 Then
                oDataAdapter.Fill(oDataSet, p_strDataTableName)
            Else
                oDataAdapter.Fill(oDataSet)
            End If


            If (p_colParameters IsNot Nothing) Then
                For Each oParameter In p_colParameters
                    With oParameter
                        If .ParameterName = "@intRowCount" Then
                            oDataSet.ExtendedProperties.Add("RowCount", oDataAdapter.SelectCommand.Parameters("@intRowCount").Value)
                            Exit For
                        End If
                    End With
                Next
            End If

            Return oDataSet

        Finally
            oConn.Close()
            oCommand.Dispose()
            oDataAdapter.Dispose()
            oConn.Dispose()
            oDataSet.Dispose()
        End Try
    End Function

    Friend Function GetLargeDataSet(ByVal p_strStoredProcedureName As String, _
               ByVal p_colParameters As Collections.Specialized.HybridDictionary, _
               Optional ByVal p_strDataTableName As String = "", Optional ByVal int_Command_Timeout As Integer = 300) As DataSet

        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString)
        Dim oCommand As New SqlCommand
        oCommand.CommandTimeout = int_Command_Timeout
        Dim oDataAdapter As New SqlDataAdapter
        Dim oDataSet As New DataSet
        Dim oParameter As SqlParameter

        Try
            oConn.Open()

            oCommand.CommandText = p_strStoredProcedureName
            oCommand.CommandType = CommandType.StoredProcedure
            oCommand.Connection = oConn

            'Add the parameters as passed in 
            For Each oParameter In p_colParameters
                oCommand.Parameters.Add(oParameter.Value)
            Next

            oDataAdapter.SelectCommand = oCommand

            If Len(p_strDataTableName) <> 0 Then
                oDataAdapter.Fill(oDataSet, p_strDataTableName)
            Else
                oDataAdapter.Fill(oDataSet)
            End If

            If p_colParameters.Contains("@intRowCount") Then
                oDataSet.ExtendedProperties.Add("RowCount", oDataAdapter.SelectCommand.Parameters("@intRowCount").Value)
            End If

            Return oDataSet

        Finally
            oConn.Close()
            oCommand.Dispose()
            oDataAdapter.Dispose()
            oConn.Dispose()
            oDataSet.Dispose()
        End Try
    End Function


    Friend Function ExecSP(ByVal p_strStoredProcedureName As String, _
               ByVal p_colParameters As List(Of SqlParameter)) As List(Of SqlParameter)
        'Returns the output parameters of the SP

        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString)
        Dim oCommand As New SqlCommand
        Dim oParameter As SqlParameter
        Dim colOutputParameters As New List(Of SqlParameter)
        Dim retval As Integer
        Dim timeout As Integer

        Try
            If (Integer.TryParse(ConfigurationManager.AppSettings.Get("SQLConnectionTimeout"), timeout) = False) Then
                timeout = 300
            End If

            oConn.Open()

            oCommand.CommandText = p_strStoredProcedureName
            oCommand.CommandType = CommandType.StoredProcedure
            oCommand.Connection = oConn
            oCommand.CommandTimeout = timeout   '300
            'Add the parameters as passed in 
            For Each oParameter In p_colParameters
                oCommand.Parameters.Add(oParameter)
            Next

            retval = oCommand.ExecuteNonQuery()

            'Return the output parameters of this SP
            For Each oParameter In p_colParameters
                With oParameter
                    If .Direction = ParameterDirection.Output Then
                        colOutputParameters.Add(oCommand.Parameters(.ParameterName))
                    End If
                End With
            Next

            Return colOutputParameters

        Finally
            oConn.Close()
            oCommand.Dispose()
            oConn.Dispose()
        End Try
    End Function

    Friend Function ExecSQL(ByVal p_strSQL As String) As Long
        'Returns the # of rows affected

        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString)
        Dim oCommand As New SqlCommand
        Dim retval As Long

        Try
            oConn.Open()

            oCommand.CommandType = CommandType.Text
            oCommand.Connection = oConn
            oCommand.CommandText = p_strSQL
            oCommand.CommandTimeout = 300

            retval = oCommand.ExecuteNonQuery()

            Return retval

        Finally
            oConn.Close()
            oCommand.Dispose()
            oConn.Dispose()
        End Try
    End Function

    'pb, 03/19/04
    'function checks for data in the first table in the dataset
    'returns false if dataset contains data, true otherwise
    Friend Function IsDSEmpty(ByRef ds As DataSet) As Boolean
        Dim bResult As Boolean = True

        If (ds IsNot Nothing) AndAlso (ds.Tables.Count > 0) AndAlso (ds.Tables(0).Rows.Count > 0) Then
            bResult = False
        End If

        Return bResult

    End Function

    Friend Function IsDataSetNotEmpty(ByRef ds As DataSet) As Boolean
        Dim bResult As Boolean = False

        If (ds IsNot Nothing) AndAlso (ds.Tables.Count > 0) AndAlso (ds.Tables(0).Rows.Count > 0) Then
            bResult = True
        End If

        Return bResult

    End Function

    <Extension()> _
    Public Function IsEmpty(ByVal input As DataSet) As Boolean
        Dim bResult As Boolean = True

        If (input IsNot Nothing) AndAlso (input.Tables.Count > 0) AndAlso (input.Tables(0).Rows.Count > 0) Then
            bResult = False
        End If

        Return bResult
    End Function

    <Extension()> _
    Public Function IsNotEmpty(ByVal input As DataSet) As Boolean
        Dim bResult As Boolean = False

        If (input IsNot Nothing) AndAlso (input.Tables.Count > 0) AndAlso (input.Tables(0).Rows.Count > 0) Then
            bResult = True
        End If

        Return bResult
    End Function


    Function GetDataSet_tblUserTypes() As DataSet
        Return GetDataSet("SELECT UserTypeid, Description FROM tblUserTypes ORDER BY UserTypeID")
    End Function

    Function GetDataSet_tblUserAccounts(ByVal UserID As String) As DataSet
        Return GetDataSet("SELECT UserID, FirstName, LastName, LogInName, Password, UserTypeID, " & _
                                    "ModifiedDT, ChangedByUser, StationBypassEnabled, " & _
                                    "BarcodeIdentifier, BadgeID, LoginLevel, PLCUser " & _
                            "FROM tblUserAccounts " & _
                            "WHERE userid = " & UserID)
    End Function
End Module
