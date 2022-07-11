Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports System.Data.SqlClient


<ServiceContract(Namespace:="")>
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class SecurityService

    ''' <summary>
    ''' Method to retrieve a comma seperated list of numbers to test the connection to the service.
    ''' </summary>
    ''' <returns>comma seperated list of numbers</returns>
    ''' <remarks>Used to test the connection to the service</remarks>
    <OperationContract()>
    <WebInvoke(Method:="POST", ResponseFormat:=WebMessageFormat.Json)>
    Public Function TestService() As String
        Dim strResults As String = "1,44,45"
        Return strResults
    End Function

    ''' <summary>
    ''' Retrieves CSV list of Security Types that are secured for the page given
    ''' </summary>
    ''' <param name="pageName">Name of Page</param>
    ''' <param name="cmdName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    <WebInvoke(Method:="POST", ResponseFormat:=WebMessageFormat.Json)>
    Public Function GetControlSecurityOptions(ByVal pageName As String, ByVal cmdName As String) As String
        Dim strResult As String = ""
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnStringSql").ConnectionString)
        Dim oCommand As New SqlCommand
        Dim oDataAdapter As New SqlDataAdapter
        Dim oDataSet As New DataSet
        Dim oSQLParameter As SqlParameter

        Try
            oConn.Open()

            oCommand.CommandText = "procGetScreenControlSecurity"
            oCommand.CommandType = CommandType.StoredProcedure
            oCommand.Connection = oConn
            'oCommand.CommandTimeout = 300

            oSQLParameter = New SqlParameter("@pageName", SqlDbType.VarChar, 40)
            oSQLParameter.Value = pageName
            oCommand.Parameters.Add(oSQLParameter)

            oSQLParameter = New SqlParameter("@cmdName", SqlDbType.VarChar, 40)
            oSQLParameter.Value = cmdName
            oCommand.Parameters.Add(oSQLParameter)

            oDataAdapter.SelectCommand = oCommand

            oDataAdapter.Fill(oDataSet)

            If (oDataSet.Tables.Count > 0) Then
                If (oDataSet.Tables(0).DefaultView.Table.Rows.Count > 0) Then
                    strResult = CStr(oDataSet.Tables(0).DefaultView.Table.Rows(0)(0))
                End If
            End If

        Finally
            oConn.Close()
            oCommand.Dispose()
            oDataAdapter.Dispose()
            oConn.Dispose()
            oDataSet.Dispose()
        End Try

        Return strResult

    End Function

End Class
