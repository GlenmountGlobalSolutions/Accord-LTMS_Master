Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web

<ServiceContract(Namespace:="")>
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class AdministrationServices

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
<WebInvoke(Method:="POST", ResponseFormat:=WebMessageFormat.Json)>
    Public Function DoesLogInNameExist(ByVal loginName As String) As Boolean
        Dim bResult As Boolean = False
        Dim ds As DataSet

        ds = DA.GetDataSet("Select LogInName from tblUserAccounts where LogInName='" & loginName & "'")

        If (ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0) Then
            bResult = True
        End If

        Return bResult

    End Function

End Class
