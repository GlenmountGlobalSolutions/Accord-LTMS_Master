Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports System.Data.SqlClient
Imports System.Runtime.Serialization.Json

<ServiceContract(Namespace:="")>
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class ConfigurationServices

    ' To use HTTP GET, add <WebGet()> attribute. (Default ResponseFormat is WebMessageFormat.Json)
    ' To create an operation that returns XML,
    '     add <WebGet(ResponseFormat:=WebMessageFormat.Xml)>,
    '     and include the following line in the operation body:
    '         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml"
    <OperationContract()>
    Public Sub DoWork()
        ' Add your operation implementation here
    End Sub

    ' Add more operations here and mark them with <OperationContract()>

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function GetStyleGroupsByLineNumber(lineNumber As String) As List(Of NameValuePair)
        Dim nvList As New List(Of NameValuePair)
        Dim sql As String = ""
        Dim ds As DataSet

        sql = "SELECT -1 as [StyleGroupID], 'Choose a Style Group' AS [StyleGroupName], -1 as [PLCArrayOrder] UNION " & _
              "SELECT [StyleGroupID], [StyleGroupName], [PLCArrayOrder] FROM [dbo].[tblSGStyleGroups] " & _
              "WHERE [LineID] = " & lineNumber & " ORDER BY PLCArrayOrder"

        ds = DA.GetDataSet(sql)

        If (ds IsNot Nothing) Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    nvList.Add(New NameValuePair(dr(StationStyleGroupType.StyleGroupName).ToString(), dr(StationStyleGroupType.StyleGroupID).ToString()))
                Next
                End If
            End If

        Return nvList

    End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function GetStationsByLineNumber(lineNumber As String) As List(Of NameValuePair)
        Dim nvList As New List(Of NameValuePair)
        Dim sql As String = ""
        Dim ds As DataSet

        sql = "SELECT '-1' AS [StationID], 'Choose a Station' AS [Description], -1 AS [DisplayID] FROM [dbo].[tblStations] UNION " & _
              "SELECT [StationID], [Description], [DisplayID] FROM [dbo].[tblStations] " & _
              "WHERE [LineID] = " & lineNumber & " ORDER BY [DisplayID]"

        ds = DA.GetDataSet(sql)

        If (ds IsNot Nothing) Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    nvList.Add(New NameValuePair(dr(StationsType.Description).ToString(), dr(StationsType.StationID).ToString()))
                Next
                End If
            End If

        Return nvList

    End Function
End Class
