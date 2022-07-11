Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions


Public Class BizLayer

    Friend Shared Function GetApplicationParameterValue(ByVal strAppParameterTypeID As String, Optional ByVal strApplicationID As String = "") As String
        Dim strSQL As String
        Dim strResult As String = ""

        If Len(strApplicationID) = 0 Then
            strApplicationID = ConfigurationManager.AppSettings("ApplicationID")
        End If

        strSQL = "SELECT ApplicationParameterValue FROM tblApplicationParameters " & _
                "WHERE ApplicationID = '" & strApplicationID & "' " & _
                "AND ApplicationParameterTypeID = '" & strAppParameterTypeID & "'"

        Using ds As DataSet = DA.GetDataSet(strSQL)
            If (ds IsNot Nothing) AndAlso (ds.Tables.Count > 0) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                strResult = ds.Tables(0).Rows(0).Item(0).ToString()
            End If
        End Using

        Return strResult

    End Function

    Friend Shared Function GetBroadcastPoint(ByVal strBPParameterTypeID As String, Optional ByVal strRefNumberID As String = "") As String
        Dim strSQL As String
        Dim strResult As String = ""

        strSQL = "SELECT BroadcastPointID FROM tblBroadcastParameters " &
                "WHERE BroadcastParameterValue = '" & strRefNumberID & "' " &
                "AND BroadcastParameterTypeID = '" & strBPParameterTypeID & "'"

        Using ds As DataSet = DA.GetDataSet(strSQL)
            If (ds IsNot Nothing) AndAlso (ds.Tables.Count > 0) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                strResult = ds.Tables(0).Rows(0).Item(0).ToString()
            End If
        End Using

        Return strResult

    End Function
    Friend Shared Sub SetApplicationParameterValue(ByVal strAppParameterTypeID As String, Optional ByVal strApplicationParameterValue As String = "", Optional ByVal strApplicationID As String = "")
        Dim Sql As String

        If (strApplicationID.Length = 0) Then
            strApplicationID = ConfigurationManager.AppSettings("ApplicationID")
        End If

        If (strApplicationParameterValue.Length = 0) Then
            strApplicationParameterValue = "GETDATE()"
        End If

        Sql = "UPDATE dbo.tblApplicationParameters SET ApplicationParameterValue = " & strApplicationParameterValue & " WHERE " & _
                " ApplicationID = '" & strApplicationID & _
                "' AND ApplicationParameterTypeID = '" & strAppParameterTypeID & "'"

        DA.ExecSQL(Sql)

    End Sub

    Friend Shared Function GetEmailList(ByVal p_strEmailListID As String) As String
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)
        Dim strResult As String = ""
        Try

            oSqlParameter = New SqlParameter("@EmailListID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = p_strEmailListID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@EmailAddressList", SqlDbType.VarChar, 4000)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procGetEmailList", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@EmailAddressList" Then
                        strResult = oParameter.Value.ToString()
                        Exit For
                    End If
                End With
            Next


        Catch ex As Exception
            'LogErr("", Me)
        End Try

        Return strResult

    End Function

    'pb, 03/24/04
    'the following key / value functions take a string "str" which has the format:
    'key1{guid_key_end}value1{guid_value_end}key1{guid_key_end}value2{guid_value_end}...
    Friend Shared Function GetKeyValue(ByVal str As String, ByVal key As String) As String
        'check for empty key
        If (key.Length <= 0) Then
            Return ""
        End If

        'find key
        Dim index As Int32 = str.IndexOf(key & GlobalConstants.GUID_after_key)
        Dim iStartVal As Int32 = -1
        Dim iEndVal As Int32 = -1
        Dim count As Int32 = 0
        Dim value As String = ""

        If (index < 0) Then      'key not found - return empty string
            Return ""

        Else         'set value
            'find starting and ending character positions
            iStartVal = str.IndexOf(GlobalConstants.GUID_after_key, index) + GlobalConstants.GUID_after_key.Length
            iEndVal = str.IndexOf(GlobalConstants.GUID_after_value, index)
            count = iEndVal - iStartVal
            'remove old value and insert new value
            value = str.Substring(iStartVal, count)
        End If

        Return value
    End Function

    Friend Shared Function SetKeyValue(ByVal str As String, ByVal key As String, ByVal value As String) As String
        'check for empty key
        If (key.Length <= 0) Then
            Return str
        End If

        'find key
        Dim index As Int32 = str.IndexOf(key & GlobalConstants.GUID_after_key)
        Dim iStartVal As Int32 = -1
        Dim iEndVal As Int32 = -1
        Dim count As Int32 = 0

        If (index < 0) Then   'append key and set value
            str = String.Concat(str, key & GlobalConstants.GUID_after_key & value & GlobalConstants.GUID_after_value)

        Else   'set value
            'find starting and ending character positions
            iStartVal = str.IndexOf(GlobalConstants.GUID_after_key, index) + GlobalConstants.GUID_after_key.Length
            iEndVal = str.IndexOf(GlobalConstants.GUID_after_value, index)
            count = iEndVal - iStartVal
            'remove old value and insert new value
            str = str.Remove(iStartVal, count)
            str = str.Insert(iStartVal, value)
        End If

        Return str  'return modified string
    End Function

    Friend Shared Sub SetRecipeSavedDT(ByVal Transaction1 As String, Optional ByVal Transaction2 As String = "0", Optional ByVal strLineID As String = "%")

        DA.ExecSQL("UPDATE tblSGTransactionParameters SET TransactionParameterValue = CONVERT(Varchar, GETDATE(), 121) " & _
                   "WHERE (TransactionID = " & Transaction1 & " OR TransactionID = " & Transaction2 & ") AND TransactionParameterTypeID = '0004' AND LineNumber LIKE '" & strLineID & "'")

    End Sub

    Friend Shared Function GetLineTypeID(ByVal intLineNumber As Integer) As Integer
        Dim intResult As Integer = 0
        Try
            Dim strSQL As String

            strSQL = "SELECT TOP 1 [LineTypeID] FROM [dbo].[tblSGLines] WHERE [LineNumber] = " & intLineNumber

            Using ds As DataSet = DA.GetDataSet(strSQL)
                If (ds IsNot Nothing) AndAlso (ds.Tables.Count > 0) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                    intResult = CInt(ds.Tables(0).Rows(0).Item(0))
                End If
            End Using

            Return intResult
        Catch ex As Exception
            Return intResult
        End Try
    End Function

    Friend Shared Function GetMultiPLC(ByVal intLineNumber As Integer) As Boolean
        Dim boolResult As Boolean = False
        Try
            Dim strSQL As String

            strSQL = "SELECT TOP 1 [MultiPLC] FROM [dbo].[tblSGLines] WHERE [LineNumber] = " & intLineNumber

            Using ds As DataSet = DA.GetDataSet(strSQL)
                If (ds IsNot Nothing) AndAlso (ds.Tables.Count > 0) AndAlso (ds.Tables(0).Rows.Count > 0) Then
                    boolResult = CBool(ds.Tables(0).Rows(0).Item(0))
                End If
            End Using

            Return boolResult
        Catch ex As Exception
            Return boolResult
        End Try
    End Function

End Class

Public Class GlobalConstants
    Public Const GUID_after_key As String = "{D156-4589-A992}"
    Public Const GUID_after_value As String = "{97E2-4b1f-8476}"
End Class