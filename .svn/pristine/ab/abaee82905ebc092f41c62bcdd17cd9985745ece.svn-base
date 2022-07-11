Public Class Security

    Public Shared Function GetDescriptionList(ByVal csvList As String) As String
        If (csvList.Length <= 0) Then
            Return ""
        End If

        Dim dsDescriptions As New DataSet
        dsDescriptions = DA.GetDataSet("SELECT Description FROM tblUserTypes WHERE (UserTypeID IN (" & csvList & "))")
        If (dsDescriptions.Tables.Count <= 0) Then
            Return ""
            If (dsDescriptions.Tables(0).DefaultView.Table.Rows.Count <= 0) Then
                Return ""
            End If
        End If

        Dim descList As String = ""
        Dim i As Integer

        While (i < dsDescriptions.Tables(0).DefaultView.Table.Rows.Count)
            descList += dsDescriptions.Tables(0).DefaultView.Table.Rows(i)(0).ToString() & ","
            i += 1
        End While
        descList = descList.TrimEnd(CChar(","))

        Return descList
    End Function

    Public Shared Function SaveControlSecurity(ByVal selectedPage As String, ByVal controlName As String, ByRef selectedUserTypes As ArrayList, ByRef message As String) As Boolean
        Dim bResult As Boolean = False
        Dim screenID As Integer = -1
        Dim dsScreenID As New DataSet
        Dim dsButtonSecurity As New DataSet

        Dim i As Integer = 0
        Dim userTypeID As String = ""
        Dim oldList As String = ""
        Dim newList As String = ""

        'check to see if all checkboxes have been unchecked
        If (selectedUserTypes.Count = 0) Then
            message = "Please select at least one user type."
        Else

            'get all permissions associated with button from db
            dsButtonSecurity = DA.GetDataSet("SELECT DISTINCT tblScreensControlsSecurity.UserTypeID FROM tblScreensSecurity " & _
                                             " INNER JOIN tblScreensControlsSecurity ON tblScreensSecurity.ScreenID = tblScreensControlsSecurity.ScreenID " & _
                                             " WHERE (tblScreensSecurity.ScreenPath = '" & selectedPage & _
                                             "') AND (tblScreensControlsSecurity.ControlName = '" & controlName & "')")

            'select permissions contained in db
            For i = 0 To (dsButtonSecurity.Tables(0).DefaultView.Table.Rows.Count - 1)
                oldList += dsButtonSecurity.Tables(0).DefaultView.Table.Rows(i)(0).ToString() & ","
            Next
            oldList = oldList.TrimEnd(CChar(","))
            oldList = GetDescriptionList(oldList)

            '*** delete records for selected control
            'get screen id
            dsScreenID = DA.GetDataSet("SELECT TOP 1 ScreenID FROM tblScreensSecurity WHERE (ScreenPath = '" & selectedPage & "')")
            If (dsScreenID.Tables.Count > 0) Then
                If (dsScreenID.Tables(0).DefaultView.Table.Rows.Count > 0) Then
                    screenID = CInt(dsScreenID.Tables(0).DefaultView.Table.Rows(0)(0))
                End If
            End If

            If (screenID <= 0) Then
                message = "Error: unable to locate ScreenID record in tblScreensSecurity for path " & selectedPage
            Else

                'execute sql statement
                DA.ExecSQL("DELETE FROM tblScreensControlsSecurity WHERE (ControlName = '" & controlName & "') AND (ScreenID = " & screenID & ")")


                '*** insert records for selected control
                For i = 0 To (selectedUserTypes.Count - 1)
                    userTypeID = selectedUserTypes(i).ToString()
                    newList += userTypeID & ","
                    'insert new record into the controls security table
                    DA.ExecSQL("INSERT INTO tblScreensControlsSecurity (ScreenID, ControlName, UserTypeID) VALUES  (" & screenID & ", '" & controlName & "', " & userTypeID & ")")
                Next

                newList = newList.TrimEnd(CChar(","))
                newList = GetDescriptionList(newList)

                'log event
                message = "Allow user types access list for: " & controlName & " has changed from: " & oldList & " to: " & newList
                bResult = True
            End If
        End If

        Return bResult
    End Function

    'this function converts the password using the Rot13 (http://www.rot13.com/) method
    Public Shared Function ConvertPassword(ByVal thePassword As String) As String
        Dim offset As Int32
        Dim c As Char
        Dim retval As String = ""
        Dim arr() As Char = thePassword.ToCharArray()

        ' loop through passed in string...
        For Each c In arr

            If (Char.ToUpper(c) >= "A") And (Char.ToUpper(c) <= "M") Then
                ' if char is between "A" and "Z" then add 13 to its ASCII value                
                offset = 13
            ElseIf (Char.ToUpper(c) >= "N") And (Char.ToUpper(c) <= "Z") Then
                ' if char is between "N" and "Z" then subtract 13 from its ASCII value                
                offset = -13
            Else
                ' don't change it...
                offset = 0
            End If

            retval = retval & Chr(Asc(c) + offset)
        Next c

        Return retval

    End Function
End Class
