Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports System.Data.SqlClient
Imports System.Runtime.Serialization.Json



<ServiceContract(Namespace:="")>
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class OperationServices

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

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function TestService_List(ByVal seqDT As String) As List(Of NameValuePair)
        Dim nvList As New List(Of NameValuePair)

        nvList.Add(New NameValuePair("test1", "one"))
        nvList.Add(New NameValuePair("test2", "two"))
        nvList.Add(New NameValuePair("test3", "three"))

        Return nvList

    End Function


    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function ProductionScheduleGetMoveList(ByVal nodeLevel As String, ByVal seqDT As String, ByVal lot As String, ByVal boolSetexOrder As String, ByVal broadcastPointID As String) As List(Of NameValuePair)
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim fulllot As String
        Dim nvList As New List(Of NameValuePair)
        Dim lot10 As String




        If (CInt(nodeLevel) = 2) Then
            lot10 = lot.Replace("-", "").Substring(0, 10)
            lot = lot.Replace("-", "").Substring(0, 8)
            oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
            oSqlParameter.Value = lot
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 1)
            oSqlParameter.Value = broadcastPointID
            colParameters.Add(oSqlParameter)

            If CBool(boolSetexOrder) = True Then 'return the orders closes ranked by production schedule dt
                ds = DA.GetDataSet("procPSGetClosestLots", colParameters)
            Else 'return the orders closest ranked by honda index
                ds = DA.GetDataSet("procPSGetClosestLotsHonda", colParameters)
            End If

            If (ds IsNot Nothing) Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If dr("ParentID").ToString().Length > 0 Then
                            If (dr("ID").ToString() <> lot10) Then 'And (dr("ID").ToString() <> HttpUtility.UrlDecode(seqDT).Replace("-", "").Substring(0, 10)) Then
                                fulllot = dr("ID").ToString().Substring(0, 1) + "-" + dr("ID").ToString().Substring(1, 4) + "-" + dr("ID").ToString().Substring(5, 3) + "-" + dr("ID").ToString().Substring(8, 2)
                                'nvList.Add(New NameValuePair(dr("ID").ToString(), fulllot))
                                nvList.Add(New NameValuePair(fulllot, fulllot))
                            Else
                                fulllot = dr("ID").ToString().Substring(0, 1) + "-" + dr("ID").ToString().Substring(1, 4) + "-" + dr("ID").ToString().Substring(5, 3) + "-" + dr("ID").ToString().Substring(8, 2)
                                'nvList.Add(New NameValuePair(dr("ID").ToString(), fulllot, True))  'set as selected
                                nvList.Add(New NameValuePair(fulllot, fulllot, True))  'set as selected
                            End If
                        End If
                    Next
                End If
            End If


        Else
            lot = lot.Replace("-", "").Substring(0, 8)
            oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
            oSqlParameter.Value = lot
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 1)
            oSqlParameter.Value = broadcastPointID
            colParameters.Add(oSqlParameter)

            If CBool(boolSetexOrder) = True Then 'return the orders closes ranked by production schedule index
                ds = DA.GetDataSet("procPSGetClosestLotsForProduction", colParameters)
            Else 'return the orders closest ranked by honda index
                ds = DA.GetDataSet("procPSGetClosestLotsForProductionHonda", colParameters)
            End If

            If (ds IsNot Nothing) Then
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If dr("TEXT").ToString().Length > 0 Then
                            If (dr("VALUE").ToString() <> lot) And (dr("VALUE").ToString() <> HttpUtility.UrlDecode(seqDT)) Then
                                nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString()))
                            Else
                                nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString(), True))  'set as selected
                            End If
                        End If
                    Next
                End If
            End If
        End If

        Return nvList

    End Function
    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function ShippingScheduleGetMoveList(ByVal nodeLevel As String, ByVal seqDT As String, ByVal lot As String, ByVal boolSetexOrder As String, ByVal broadcastPointID As String) As List(Of NameValuePair)
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)

        Dim nvList As New List(Of NameValuePair)


        'CREATE PROCEDURE procPSGetSubLots @SequenceDT datetime
        'if a sublot then return all the sublot nodes
        'If (CInt(nodeLevel) = psNodeType.SUBLOT) Then

        '    oSqlParameter = New SqlParameter("@SequenceDT", SqlDbType.DateTime)
        '    oSqlParameter.Value = HttpUtility.UrlDecode(seqDT)
        '    colParameters.Add(oSqlParameter)

        '    oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
        '    oSqlParameter.Value = broadcastPointID
        '    colParameters.Add(oSqlParameter)

        '    ds = DA.GetDataSet("procPSGetSubLots", colParameters)

        '    'CREATE PROCEDURE procPSGetClosestLotsForProduction @LotNumber8 varchar(8) 
        '    'if a lot then return all lot nodes
        'ElseIf (CInt(nodeLevel) = psNodeType.LOT) Then
        '    If (lot.Length > 0) Then
        '        lot = lot.Replace("-", "").Substring(0, 8)
        '    End If
        '    oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
        '    oSqlParameter.Value = lot
        '    colParameters.Add(oSqlParameter)

        '    If CBool(boolSetexOrder) = True Then 'return the orders closes ranked by production schedule index
        '        ds = DA.GetDataSet("procPSGetClosestLotsForProduction", colParameters)
        '    Else 'return the orders closest ranked by honda index
        '        ds = DA.GetDataSet("procPSGetClosestLotsForProductionHonda", colParameters)
        '    End If
        'Else
        '    Return Nothing
        'End If

        lot = lot.Replace("-", "").Substring(0, 8)
        oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
        oSqlParameter.Value = lot
        colParameters.Add(oSqlParameter)
        ds = DA.GetDataSet("procPSGetClosestLotsForShipping", colParameters)



        If (ds IsNot Nothing) Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If (dr("VALUE").ToString() <> lot) And (dr("VALUE").ToString() <> HttpUtility.UrlDecode(seqDT)) Then
                        nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString()))
                    Else
                        nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString(), True))  'set as selected
                    End If
                Next
            End If
        End If

        Return nvList

    End Function

    '<OperationContract()>
    '<WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    'Public Function ProductionSchedule_Dialog_PopulateDetails(ByVal seqDT As String, ByVal broadcastPointID As String) As ProductionScheduleAddNew_Detail
    '    Dim sql As String = ""
    '    Dim ds As DataSet
    '    Dim newDetail As New ProductionScheduleAddNew_Detail()
    '    Dim strLot As String = ""

    '    sql = sql + "SELECT OD.SequenceNumber,"
    '    sql = sql + " ISNULL((SELECT OD1.OrderParameterValue FROM tblOrderData OD1 WHERE OD1.SequenceDT = OD.SequenceDT AND OD1.BroadcastPointID = OD.BroadcastPointID AND OD1.OrderParameterID = '0008'), '???') AS 'JobQuantity',"
    '    sql = sql + " ISNULL((SELECT OD2.OrderParameterValue FROM tblOrderData OD2 WHERE OD2.SequenceDT = OD.SequenceDT AND OD2.BroadcastPointID = OD.BroadcastPointID AND OD2.OrderParameterID = '0004'), '???') AS 'ProductID',"
    '    sql = sql + " ISNULL((SELECT OD3.OrderParameterValue FROM tblOrderData OD3 WHERE OD3.SequenceDT = OD.SequenceDT AND OD3.BroadcastPointID = OD.BroadcastPointID AND OD3.OrderParameterID = '0041'), '???') AS 'ProductionNotes'"
    '    sql = sql + " FROM tblOrderData OD"
    '    sql = sql + " WHERE BroadcastPointID = '" + broadcastPointID + "'"
    '    sql = sql + " AND SequenceDT = '" + HttpUtility.UrlDecode(seqDT) + "'"
    '    sql = sql + " AND OrderParameterID = '0001'"
    '    ds = DA.GetDataSet(sql)

    '    If (Not DA.IsDSEmpty(ds)) Then
    '        strLot = ds.Tables(0).DefaultView.Table.Rows(0)("SequenceNumber").ToString().Substring(0, 8)
    '        newDetail.LotNumber = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3)
    '        newDetail.SubLotIndex = ds.Tables(0).DefaultView.Table.Rows(0)("SequenceNumber").ToString().Substring(8, 2)
    '        newDetail.Quantity = ds.Tables(0).DefaultView.Table.Rows(0)("JobQuantity").ToString()
    '        newDetail.ProductID = ds.Tables(0).DefaultView.Table.Rows(0)("ProductID").ToString()
    '        newDetail.ProductionNotes = ds.Tables(0).DefaultView.Table.Rows(0)("ProductionNotes").ToString()
    '    End If

    '    If (newDetail.ProductID.Length > 0) Then
    '        sql = "SELECT "
    '        sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0202' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [202],"
    '        sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "'), '???') AS [204],"
    '        sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0211' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [211],"
    '        sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0212' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [212],"
    '        sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0214' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [214],"
    '        sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0215' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [215]"
    '        ds = DA.GetDataSet(sql)
    '        If (Not DA.IsDSEmpty(ds)) Then
    '            newDetail.ShipCode4 = ds.Tables(0).DefaultView.Table.Rows(0)("204").ToString()
    '            newDetail.DriverSeatStyle = ds.Tables(0).DefaultView.Table.Rows(0)("211").ToString()
    '            newDetail.PassengerSeatStyle = ds.Tables(0).DefaultView.Table.Rows(0)("212").ToString()
    '            newDetail.ModelDescription = ds.Tables(0).DefaultView.Table.Rows(0)("214").ToString() + " " + ds.Tables(0).DefaultView.Table.Rows(0)("215").ToString()
    '        End If
    '    End If

    '    Return newDetail

    'End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function ProductionSchedule_Dialog_PopulateDetails(ByVal seqNum As String, ByVal broadcastPointID As String) As ProductionScheduleAddNew_Detail
        Dim sql As String = ""
        Dim ds As DataSet
        Dim newDetail As New ProductionScheduleAddNew_Detail()
        Dim strLot As String = ""

        seqNum = seqNum.Replace(":"c, "01").Replace(" "c, "")
        'seqNum = seqNum.Replace(" "c, "")

        sql = sql + "SELECT OD.SequenceNumber,"
        sql = sql + " ISNULL((SELECT OD1.OrderParameterValue FROM tblOrderData OD1 WHERE OD1.SequenceNumber = OD.SequenceNumber And OD1.BroadcastPointID = OD.BroadcastPointID And OD1.OrderParameterID = '0008'), '???') AS 'JobQuantity',"
        sql = sql + " ISNULL((SELECT OD2.OrderParameterValue FROM tblOrderData OD2 WHERE OD2.SequenceNumber = OD.SequenceNumber AND OD2.BroadcastPointID = OD.BroadcastPointID AND OD2.OrderParameterID = '0004'), '???') AS 'ProductID',"
        sql = sql + " ISNULL((SELECT OD3.OrderParameterValue FROM tblOrderData OD3 WHERE OD3.SequenceNumber = OD.SequenceNumber AND OD3.BroadcastPointID = OD.BroadcastPointID AND OD3.OrderParameterID = '0041'), '???') AS 'ProductionNotes'"
        sql = sql + " FROM tblOrderData OD"
        sql = sql + " WHERE BroadcastPointID = '" + broadcastPointID + "'"
        'sql = sql + " AND SequenceDT = '" + HttpUtility.UrlDecode(seqDT) + "'"
        sql = sql + " AND SequenceNumber = '" + HttpUtility.UrlDecode(seqNum) + "'" '+ "01'"
        sql = sql + " AND OrderParameterID = '0001'"
        ds = DA.GetDataSet(sql)

        If (Not DA.IsDSEmpty(ds)) Then
            strLot = ds.Tables(0).DefaultView.Table.Rows(0)("SequenceNumber").ToString().Substring(0, 8)
            newDetail.LotNumber = strLot.Substring(0, 1) + "-" + strLot.Substring(1, 4) + "-" + strLot.Substring(5, 3)
            newDetail.SubLotIndex = ds.Tables(0).DefaultView.Table.Rows(0)("SequenceNumber").ToString().Substring(8, 2)
            newDetail.Quantity = ds.Tables(0).DefaultView.Table.Rows(0)("JobQuantity").ToString()
            newDetail.ProductID = ds.Tables(0).DefaultView.Table.Rows(0)("ProductID").ToString()
            newDetail.ProductionNotes = ds.Tables(0).DefaultView.Table.Rows(0)("ProductionNotes").ToString()
        End If

        If (newDetail.ProductID.Length > 0) Then
            sql = "SELECT "
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0202' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [202],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "'), '???') AS [204],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0211' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [211],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0212' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [212],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0214' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [214],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0215' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + newDetail.ProductID + "')), '???') AS [215]"
            ds = DA.GetDataSet(sql)
            If (Not DA.IsDSEmpty(ds)) Then
                newDetail.ShipCode4 = ds.Tables(0).DefaultView.Table.Rows(0)("204").ToString()
                newDetail.DriverSeatStyle = ds.Tables(0).DefaultView.Table.Rows(0)("211").ToString()
                newDetail.PassengerSeatStyle = ds.Tables(0).DefaultView.Table.Rows(0)("212").ToString()
                newDetail.ModelDescription = ds.Tables(0).DefaultView.Table.Rows(0)("214").ToString() + " " + ds.Tables(0).DefaultView.Table.Rows(0)("215").ToString()
            End If
        End If

        Return newDetail

    End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function ProductionSchedule_Dialog_PopulateProductDetail(ByVal ProductID As String) As ProductionScheduleAddNew_Detail
        Dim sql As String = ""
        Dim ds As DataSet
        Dim newDetail As New ProductionScheduleAddNew_Detail()
        Dim strLot As String = ""

        If (ProductID.Length > 0) Then
            sql = "SELECT "
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0202' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + ProductID + "')), '???') AS [202],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + ProductID + "'), '???') AS [204],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0211' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + ProductID + "')), '???') AS [211],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0212' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + ProductID + "')), '???') AS [212],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0214' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + ProductID + "')), '???') AS [214],"
            sql = sql + " ISNULL((SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0215' AND ProductID = (SELECT ProductParameterValue FROM tblProductParameters WHERE ProductParameterTypeID = '0204' AND ProductID = '" + ProductID + "')), '???') AS [215]"
            ds = DA.GetDataSet(sql)
            If (Not DA.IsDSEmpty(ds)) Then
                newDetail.ShipCode4 = ds.Tables(0).DefaultView.Table.Rows(0)("204").ToString()
                newDetail.DriverSeatStyle = ds.Tables(0).DefaultView.Table.Rows(0)("211").ToString()
                newDetail.PassengerSeatStyle = ds.Tables(0).DefaultView.Table.Rows(0)("212").ToString()
                newDetail.ModelDescription = ds.Tables(0).DefaultView.Table.Rows(0)("214").ToString() + " " + ds.Tables(0).DefaultView.Table.Rows(0)("215").ToString()
            End If
        End If

        Return newDetail

    End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function ProductionSchedule_GetProductIDList(productID As String) As List(Of NameValuePair)
        Dim nvList As New List(Of NameValuePair)
        Dim sql As String = ""
        Dim ds As DataSet

        sql = "SELECT '' AS [VALUE] UNION ALL SELECT DISTINCT PP.ProductID AS [VALUE] FROM tblProductParameters PP INNER JOIN tblProductParameterTypes PPT ON PP.ProductParameterTypeID = PPT.ProductParameterTypeID WHERE (PPT.ComponentID = '03')"
        'sql = "SELECT DISTINCT PP.ProductID AS [VALUE] FROM tblProductParameters PP INNER JOIN tblProductParameterTypes PPT ON PP.ProductParameterTypeID = PPT.ProductParameterTypeID WHERE (PPT.ComponentID = '03')"
        ds = DA.GetDataSet(sql)

        If (ds IsNot Nothing) Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If (dr("VALUE").ToString() <> productID) Then
                        nvList.Add(New NameValuePair(dr("VALUE").ToString(), dr("VALUE").ToString()))
                    Else
                        nvList.Add(New NameValuePair(dr("VALUE").ToString(), dr("VALUE").ToString(), True))  'set as selected
                    End If
                Next
            End If
        End If

        Return nvList

    End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function ProductionSchedule_DialogNew_GetNValueList(ByVal broadcastPointID As String) As List(Of NameValuePair)
        Dim nvList As New List(Of NameValuePair)
        Dim ds As DataSet

        ds = DA.GetDataSet("SELECT DISTINCT OrderParameterValue FROM tblOrderData WHERE(OrderParameterID ='0154') AND BroadcastPointID = '" + broadcastPointID + "'")

        If (ds IsNot Nothing) Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    nvList.Add(New NameValuePair(dr("OrderParameterValue").ToString(), dr("OrderParameterValue").ToString()))
                Next
            End If
        End If

        Return nvList

    End Function

    ''<OperationContract()>
    ''<WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    ''Public Function ProductionSchedule_DialogNew_LotList(ByVal lotNumber As String, ByVal boolSetexOrder As String, ByVal broadcastPointID As String) As List(Of NameValuePair)
    ''    Dim nvList As New List(Of NameValuePair)
    ''    Dim oSqlParameter As SqlParameter
    ''    Dim colParameters As New List(Of SqlParameter)()
    ''    Dim ds As DataSet

    ''    oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
    ''    oSqlParameter.Value = lotNumber.Replace("-", "")
    ''    colParameters.Add(oSqlParameter)

    ''    oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
    ''    oSqlParameter.Value = broadcastPointID
    ''    colParameters.Add(oSqlParameter)

    ''    'MAS 06-06-06 check here if All Orders or just Setex Orders selected
    ''    'if SetexOrders was chosen then return Setex Orders
    ''    If CBool(boolSetexOrder) = True Then
    ''        ds = DA.GetDataSet("procPSGetClosestLotsForProduction", colParameters)
    ''    Else
    ''        'return all Orders
    ''        ds = DA.GetDataSet("procPSGetClosestLotsForProductionHonda", colParameters)
    ''    End If

    ''    If (ds IsNot Nothing) Then
    ''        If ds.Tables(0).Rows.Count > 0 Then
    ''            For Each dr As DataRow In ds.Tables(0).Rows
    ''                If (dr("TEXT").ToString() <> lotNumber) Then
    ''                    nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString()))
    ''                Else
    ''                    nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString(), True))
    ''                End If
    ''            Next
    ''        End If
    ''    End If


    ''    Return nvList

    ''End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function ProductionSchedule_DialogNew_LotList(ByVal lotNumber As String, ByVal boolSetexOrder As String, ByVal broadcastPointID As String) As List(Of NameValuePair)
        Dim nvList As New List(Of NameValuePair)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)()
        Dim ds As DataSet
        Dim location As Integer

        location = lotNumber.LastIndexOf(":")
        If location <> -1 Then
            lotNumber = lotNumber.Substring(0, 10).Replace("-", "")
        End If

        oSqlParameter = New SqlParameter("@LotNumber8", SqlDbType.VarChar, 8)
        oSqlParameter.Value = lotNumber.Replace("-", "")
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
        oSqlParameter.Value = broadcastPointID
        colParameters.Add(oSqlParameter)

        'MAS 06-06-06 check here if All Orders or just Setex Orders selected
        'if SetexOrders was chosen then return Setex Orders
        If CBool(boolSetexOrder) = True Then
            ds = DA.GetDataSet("procPSGetClosestLotsForProduction", colParameters)
        Else
            'return all Orders
            ds = DA.GetDataSet("procPSGetClosestLotsForProductionHonda", colParameters)
        End If

        If (ds IsNot Nothing) Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If (dr("TEXT").ToString().Replace("-", "") <> lotNumber) Then
                        nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString()))
                    Else
                        nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString(), True))
                    End If
                Next
            End If
        End If


        Return nvList

    End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function DailyBuildGetEndLots(ByVal productionDate As String, ByVal orderType As String, ByVal prodOrShipType As String, ByVal broadcastPointID As String) As List(Of NameValuePair)
        Dim ds As DataSet
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim nvList As New List(Of NameValuePair)
        Dim strEndLotProduced As String = ""
        Dim strEndLotShipped As String = ""

        oSqlParameter = New SqlParameter("@ProductionDate", SqlDbType.DateTime)
        oSqlParameter.Value = productionDate
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@ProdOrShipType", SqlDbType.VarChar, 4)
        oSqlParameter.Value = prodOrShipType
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
        oSqlParameter.Value = broadcastPointID
        colParameters.Add(oSqlParameter)

        If (orderType.ToUpper() = "ALL") Then
            ds = DA.GetDataSet("procPSGetEndLotsHonda", colParameters)
        Else
            ds = DA.GetDataSet("procPSGetEndLots", colParameters)
        End If

        If (ds IsNot Nothing) Then
            If ds.Tables(1).Rows.Count > 0 Then
                strEndLotProduced = ds.Tables(1).Rows(0)(0).ToString()
                strEndLotShipped = ds.Tables(1).Rows(0)(1).ToString()
            End If

            If ds.Tables(0).Rows.Count > 0 Then
                nvList.Add(New NameValuePair("", "-1"))  'add a blank row

                For Each dr As DataRow In ds.Tables(0).Rows
                    If ((dr("VALUE").ToString() = strEndLotProduced) Or (dr("VALUE").ToString() = strEndLotShipped)) Then
                        nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString(), True))
                    Else
                        nvList.Add(New NameValuePair(dr("TEXT").ToString(), dr("VALUE").ToString()))
                    End If
                Next
            End If
        End If

        Return nvList

    End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function DailyBuildGetLotSize(ByVal lotNumber As String, ByVal broadcastPointID As String) As String
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim strLotSize As String = ""

        oSqlParameter = New SqlParameter("@LotNumber", SqlDbType.VarChar, 80)
        oSqlParameter.Value = lotNumber
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@BroadcastPointID", SqlDbType.VarChar, 4)
        oSqlParameter.Value = broadcastPointID
        colParameters.Add(oSqlParameter)

        oSqlParameter = New SqlParameter("@LotSize", SqlDbType.Int)
        oSqlParameter.Direction = ParameterDirection.Output
        colParameters.Add(oSqlParameter)

        colOutput = DA.ExecSP("procPSGetLotSize", colParameters)

        For Each oParameter In colOutput
            With oParameter
                If .Direction = ParameterDirection.Output And .ParameterName = "@LotSize" Then
                    strLotSize = oParameter.Value.ToString()
                    Exit For
                End If
            End With
        Next
        If strLotSize.Trim.Length = 0 Then
            strLotSize = "???"
        End If
        Return strLotSize

    End Function


    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function ProductionSchedule_DialogMove_BroadcastPointIDs(ByVal selectedBroadcastPointID As String) As List(Of NameValuePair)
        Dim nvList As New List(Of NameValuePair)
        Dim colParameters As New List(Of SqlParameter)()
        Dim ds As DataSet

        ds = DA.GetDataSet("procGetBroadcastPoints")

        If (ds IsNot Nothing) Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If (dr("BroadcastPointID").ToString() <> selectedBroadcastPointID) Then
                        nvList.Add(New NameValuePair(dr("Description").ToString(), dr("BroadcastPointID").ToString()))
                    Else
                        nvList.Add(New NameValuePair(dr("Description").ToString(), dr("BroadcastPointID").ToString(), True))
                    End If
                Next
            End If
        End If

        Return nvList

    End Function

    <OperationContract()>
    <WebInvoke(Method:="POST", RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json)>
    Public Function LotTraceData_DialogModifyComponentHistory_VerifyComponentScan(ByVal StationID As String, ComponentScan As String, ComponentName As String, _
                                                                                  StyleGroupID As String, ProductID As String, ComponentNameIDX As String) As VerifyComponentScan
        Dim colParms As New List(Of SqlParameter)
        Dim colOutParms As List(Of SqlParameter)
        Dim prmNext As Data.SqlClient.SqlParameter
        Dim vcs As New VerifyComponentScan

        'IN--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@StationID", SqlDbType.VarChar, 4)
        prmNext.Value = StationID
        colParms.Add(prmNext)

        prmNext = New Data.SqlClient.SqlParameter("@ComponentScan", SqlDbType.VarChar, 82)
        prmNext.Value = ComponentScan
        colParms.Add(prmNext)

        prmNext = New Data.SqlClient.SqlParameter("@ComponentName", SqlDbType.VarChar, 50)
        prmNext.Value = ComponentName
        colParms.Add(prmNext)

        prmNext = New Data.SqlClient.SqlParameter("@StyleGroupID", SqlDbType.Int)
        prmNext.Value = StyleGroupID
        colParms.Add(prmNext)

        prmNext = New Data.SqlClient.SqlParameter("@ProductID", SqlDbType.VarChar, 32)
        prmNext.Value = ProductID
        colParms.Add(prmNext)

        prmNext = New Data.SqlClient.SqlParameter("@ComponentNameIDX", SqlDbType.Int)
        prmNext.Value = ComponentNameIDX
        colParms.Add(prmNext)

        'OUT--------------------------------------------------------
        prmNext = New Data.SqlClient.SqlParameter("@IsScanValid", SqlDbType.Bit)
        prmNext.Direction = ParameterDirection.Output
        colParms.Add(prmNext)

        prmNext = New Data.SqlClient.SqlParameter("@Status", SqlDbType.VarChar, 80)
        prmNext.Direction = ParameterDirection.Output
        colParms.Add(prmNext)

        prmNext = New Data.SqlClient.SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
        prmNext.Direction = ParameterDirection.Output
        colParms.Add(prmNext)

        colOutParms = DA.ExecSP("procSGVerifyComponentScan", colParms)

        For Each oParameter In colOutParms
            With oParameter
                If .Direction = ParameterDirection.Output Then
                    Select Case .ParameterName
                        Case "@IsScanValid"
                            vcs.IsScanValid = oParameter.Value.ToString()

                        Case "@Status"
                            vcs.Status = oParameter.Value.ToString()

                        Case "@ErrorMsg"
                            vcs.ErrorMsg = oParameter.Value.ToString()
                    End Select
                End If
            End With
        Next

        Return vcs

    End Function

End Class
