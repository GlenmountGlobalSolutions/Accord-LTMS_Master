Imports Telerik.Web.UI

Friend Class Utility

    Public Shared Sub ExpandParentNode(ByRef childNode As TreeNode)

        If (childNode.Parent IsNot Nothing) Then
            childNode.Parent.Expand()
            ExpandParentNode(childNode.Parent)
        End If

    End Sub

    Public Shared Function GetMondayOfDate(strDate As String) As Date
        Dim m As String = ""
        Dim d As String = ""
        Dim y As String = ""
        Dim dMonday As Date

        Try
            FormatDate(strDate, m, d, y)

            ''If Sunday is the first day of week:
            'Dim today As Date = New Date(CInt(y), CInt(m), CInt(d))
            'Dim dayDiff As Integer = today.DayOfWeek - DayOfWeek.Monday
            'dMonday = today.AddDays(-dayDiff)

            'If Monday is the first day of week:
            Dim today As Date = New Date(CInt(y), CInt(m), CInt(d))
            Dim dayIndex As Integer = today.DayOfWeek
            If dayIndex < DayOfWeek.Monday Then
                dayIndex += 7 'Monday is first day of week, no day of week should have a smaller index
            End If
            Dim dayDiff As Integer = dayIndex - DayOfWeek.Monday
            dMonday = today.AddDays(-dayDiff)

        Catch ex As Exception
            'Master.eMsg(ex.ToString())
        End Try

        Return dMonday

    End Function

    ''' <summary>
    '''     'inputs: dt format - m/d/yyyy
    '''     'inputs: dt format - mm/md/yyyy    
    '''     'outputs: mm/dd/yyyy
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="month"></param>
    ''' <param name="day"></param>
    ''' <param name="year"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FormatDate(ByVal dt As String, ByRef month As String, ByRef day As String, ByRef year As String) As Boolean
        Dim bResult As Boolean = False
        Dim m, d, y As String
        Try

            'minimum of 8 chars
            If (dt.Length >= 8) Then

                m = dt.Substring(0, dt.IndexOf("/"))
                dt = dt.Substring(dt.IndexOf("/") + 1)
                d = dt.Substring(0, dt.IndexOf("/"))
                dt = dt.Substring(dt.IndexOf("/") + 1)
                y = dt

                If (m.Length < 2) Then
                    m = "0" + m
                End If
                If (d.Length < 2) Then
                    d = "0" + d
                End If

                month = m
                day = d
                year = y.Substring(0, 4)


                If ((month.Length <= 0) Or (day.Length <= 0) Or (year.Length <= 0)) Then
                    bResult = False
                ElseIf ((Convert.ToInt16(month) <= 0) Or (Convert.ToInt16(month) > 12)) Then
                    bResult = False
                ElseIf ((Convert.ToInt16(day) <= 0) Or (Convert.ToInt16(day) > 31)) Then
                    bResult = False
                ElseIf ((Convert.ToInt16(year) < 1900) Or (Convert.ToInt16(year) > 3000)) Then
                    bResult = False
                Else
                    bResult = True
                End If
            End If

        Catch ex As Exception
            'Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Public Shared Function FormattedDate(ByVal dt As String) As String
        Dim m As String = ""
        Dim d As String = ""
        Dim y As String = ""

        If (Not Utility.FormatDate(dt, m, d, y)) Then
            Return ""
        End If

        Return (m + "/" + d + "/" + y)
    End Function


    ''' <summary>
    ''' This method will expand the tree view
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="numLevels">Only supports to level 1 or 2</param>
    ''' <returns></returns>
    ''' <remarks>From original 1.1 code</remarks>
    Public Shared Function TreeExpand(ByRef tree As WebControls.TreeView, ByVal numLevels As Integer) As Boolean
        Dim bResult As Boolean = False

        Dim objNode As TreeNode
        Dim objNode2 As TreeNode

        Try
            If (tree IsNot Nothing And tree.Nodes.Count > 0) Then
                tree.CollapseAll()

                If (numLevels = 1) Or (numLevels = 2) Then

                    'expand all nodes (level 1)
                    For Each objNode In tree.Nodes
                        objNode.Expanded = True
                    Next

                    If (numLevels = 2) Then

                        'expand all nodes (level 2)
                        For Each objNode In tree.Nodes
                            For Each objNode2 In objNode.ChildNodes
                                objNode2.Expanded = True
                            Next
                        Next
                    End If

                    bResult = True
                End If
            End If

        Catch ex As Exception
            'Master.eMsg(ex.ToString())
        End Try

        Return bResult

    End Function

    Public Shared Sub TreeExpand(ByRef tree As RadTreeView, ByVal numLevels As Integer)
        Dim objNode As RadTreeNode
        Dim objNode2 As RadTreeNode

        If (tree IsNot Nothing And tree.Nodes.Count > 0) Then
            tree.CollapseAllNodes()

            If (numLevels = 1) Or (numLevels = 2) Then
                'expand all nodes (level 1)
                For Each objNode In tree.Nodes
                    objNode.Expanded = True
                Next

                If (numLevels = 2) Then
                    'expand all nodes (level 2)
                    For Each objNode In tree.Nodes
                        For Each objNode2 In objNode.Nodes
                            objNode2.Expanded = True
                        Next
                    Next
                End If
            End If
        End If
    End Sub

    Public Shared Function RemoveHtmlTags(ByVal strText As Object) As String
        Dim res As String = String.Empty
        If strText IsNot Nothing Then
            Dim regex As New System.Text.RegularExpressions.Regex("<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase Or System.Text.RegularExpressions.RegexOptions.Multiline)
            res = regex.Replace(strText.ToString(), String.Empty)
        End If
        Return res
    End Function

    Public Shared Function FindNodeByText(nodetext As String, parentCollection As TreeNodeCollection) As TreeNode
        Dim node As TreeNode = Nothing

        For Each childnode As TreeNode In parentCollection
            'iterate through the treeview nnode
            If childnode.Text = nodetext Then
                node = childnode
            ElseIf childnode.ChildNodes.Count > 0 Then
                ' check for child item(level 2)
                node = FindNodeByText(nodetext, childnode.ChildNodes)
            End If
            'if Match found return node
            If (node IsNot Nothing) Then
                Exit For
            End If
        Next
        Return node
    End Function

    Public Shared Function FindNodeByValue(nodeValue As String, parentCollection As TreeNodeCollection) As TreeNode
        Dim node As TreeNode = Nothing

        For Each childnode As TreeNode In parentCollection
            'iterate through the treeview nnode
            If childnode.Value = nodeValue Then
                node = childnode
            ElseIf childnode.ChildNodes.Count > 0 Then
                ' check for child item(level 2)
                node = FindNodeByValue(nodeValue, childnode.ChildNodes)
            End If
            'if Match found return node
            If (node IsNot Nothing) Then
                Exit For
            End If
        Next
        Return node
    End Function

    Public Shared Sub SelectAndExpandToNode(ByVal node As TreeNode)
        SelectAndExpandToNode(node, True)
    End Sub
    Public Shared Sub SelectAndExpandToNode(ByVal node As TreeNode, ByVal expandNode As Boolean)
        If node IsNot Nothing Then
            node.Selected = True
            For cnt As Integer = 0 To node.Depth + 1
                If node IsNot Nothing Then
                    If (cnt > 0 Or expandNode) Then
                        node.Expand()
                    End If
                    node = node.Parent
                Else
                    Exit For
                End If
            Next
        End If
    End Sub


    Public Shared Function IntegerWithinBounds(target As Integer, minBound As Integer, maxBound As Integer) As Integer
        Return Math.Min(maxBound, Math.Max(minBound, target))
    End Function

    Public Shared Function GetClientIPAddress() As String
        Dim IP4Address As String = String.Empty
        For Each IPA As System.Net.IPAddress In System.Net.Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress)
            If IPA.AddressFamily.ToString() = "InterNetwork" Then
                IP4Address = IPA.ToString()
                Exit For
            End If
        Next
        If IP4Address <> String.Empty Then
            Return IP4Address
        End If
        For Each IPA As System.Net.IPAddress In System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())
            If IPA.AddressFamily.ToString() = "InterNetwork" Then
                IP4Address = IPA.ToString()
                Exit For
            End If
        Next
        Return IP4Address
    End Function
End Class
