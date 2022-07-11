Imports System.Data.SqlClient

Public Class ReworkMaterialDispositionListConfiguration
    Inherits System.Web.UI.Page

    Enum TreeLevel
        Component
        Category
        MaterialType
        MaterialList
        MaterialListValue
    End Enum

    Enum ComponentColumn
        ComponentID
        CompDesc
        CategoryID
        MaterialTypeID
        MatTypeDesc
        MaterialListID
        ListValue
        CatTypeDesc
        MatListDesc
        ListValueID
    End Enum


#Region "Event Handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then
                PopulateTreeView()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        EnableControls()
    End Sub


    Private Sub cmdAction_Click(sender As Object, e As System.EventArgs) Handles cmdAction.Click
        ModalDialogEventHandler()
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As System.EventArgs) Handles cmdDelete.Click
        DeleteSelectedItem()
    End Sub

#End Region

#Region "Methods"

    Private Sub EnableControls()
        Try
            Master.Secure(cmdAction)
            Master.Secure(cmdCopy)
            Master.Secure(cmdDelete)
            Master.Secure(cmdRename)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


#End Region

    Private Sub PopulateTreeView()
        Try
            Dim ds As DataSet
            Dim bContinue As Boolean = False

            Dim compNode As TreeNode = Nothing
            Dim catNode As TreeNode = Nothing
            Dim matTypeNode As TreeNode = Nothing
            Dim matListNode As TreeNode = Nothing
            Dim listValNode As TreeNode = Nothing

            Dim strComp As String = ""
            Dim strCat As String = ""
            Dim strMatType As String = ""
            Dim strMatList As String = ""
            Dim strListVal As String = ""


            ds = DA.GetDataSet("EXEC procRMDGetListValues")

            If (DA.IsDataSetNotEmpty(ds)) Then

                treeComp.Nodes.Clear()

                For Each dr As DataRow In ds.Tables(0).Rows
                    ' check if this is a toplevel node - component
                    If (strComp <> dr(ComponentColumn.ComponentID).ToString()) Then

                        strComp = dr(ComponentColumn.ComponentID).ToString()
                        compNode = New TreeNode(dr(ComponentColumn.CompDesc).ToString(), dr(ComponentColumn.ComponentID).ToString())

                        treeComp.Nodes.Add(compNode)
                        'reset other vars
                        strCat = ""
                        strMatType = ""
                        strMatList = ""
                    End If

                    ' check if this is a second level node - material
                    If (strCat <> dr(ComponentColumn.CategoryID).ToString()) Then
                        strMatType = dr(ComponentColumn.MaterialTypeID).ToString()
                        strCat = dr(ComponentColumn.CategoryID).ToString()
                        catNode = New TreeNode(dr(ComponentColumn.CatTypeDesc).ToString(), dr(ComponentColumn.CategoryID).ToString())

                        If compNode IsNot Nothing Then
                            compNode.ChildNodes.Add(catNode)
                        End If

                        'reset other vars
                        strMatType = ""
                        strMatList = ""
                    End If

                    ' check if this is a third level node - parts
                    If (strMatType <> dr(ComponentColumn.MaterialTypeID).ToString()) Then
                        strMatType = dr(ComponentColumn.MaterialTypeID).ToString()
                        matTypeNode = New TreeNode(dr(ComponentColumn.MatTypeDesc).ToString(), strMatType)

                        If catNode IsNot Nothing Then
                            catNode.ChildNodes.Add(matTypeNode)
                        End If

                        'reset other vars
                        strMatList = ""
                    End If


                    'add new part list if needed
                    If (strMatList <> dr(ComponentColumn.MaterialListID).ToString()) Then
                        strMatList = dr(ComponentColumn.MaterialListID).ToString()
                        matListNode = New TreeNode(dr(ComponentColumn.MatListDesc).ToString(), strMatList)

                        If matTypeNode IsNot Nothing Then
                            matTypeNode.ChildNodes.Add(matListNode)
                        End If
                    End If

                    If dr(ComponentColumn.ListValue).ToString().Length > 0 Then
                        strListVal = dr(ComponentColumn.ListValue).ToString()
                        listValNode = New TreeNode(strListVal, dr(ComponentColumn.ListValueID).ToString())

                        If matListNode IsNot Nothing Then
                            matListNode.ChildNodes.Add(listValNode)
                        End If
                    End If

                Next

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub ModalDialogEventHandler()
        Try
            Select Case (hidAction.Value())
                Case "New"
                    AddNewDescription()
                Case "Copy"
                    CopyDescription()
                Case "Rename"
                    RenameDescription()
                Case Else
                    Master.Msg = "No action taken."
            End Select

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub DeleteSelectedItem()
        Try
            Dim node As TreeNode = Nothing
            Dim sql As String = ""
            Dim log As String = ""
            Dim parentDepth As Integer = -1
            Dim parentValue As String = ""

            If (treeComp.SelectedNode IsNot Nothing) Then
                If (treeComp.SelectedNode.Parent IsNot Nothing) Then
                    parentDepth = treeComp.SelectedNode.Parent.Depth
                    parentValue = treeComp.SelectedNode.Parent.Value
                End If

                Select Case (treeComp.SelectedNode.Depth)
                    Case TreeLevel.Component
                        sql = "DELETE FROM tblRMDComponents WHERE (ComponentID = '" + treeComp.SelectedNode.Value + "')"
                        log = "Component " + treeComp.SelectedNode.Text + " was deleted"

                    Case TreeLevel.MaterialType
                        sql = "DELETE FROM tblRMDMaterialTypes WHERE (MaterialTypeID = '" + treeComp.SelectedNode.Value + "')"
                        log = "Material Type " + treeComp.SelectedNode.Text + " was deleted"

                    Case TreeLevel.MaterialListValue
                        sql = "DELETE FROM tblRMDListValues WHERE (ListValueID = '" + treeComp.SelectedNode.Value + "')"
                        log = "List Value " + treeComp.SelectedNode.Text + " was deleted"

                    Case Else
                        Master.Msg = "Error: unable to determine selected item."
                End Select

                DA.ExecSQL(sql)
                Master.tMsg("Delete", log)

                PopulateTreeView()

                If (parentValue.Length > 0) Then
                    node = Utility.FindNodeByValue(parentValue, treeComp.Nodes)
                    If (node IsNot Nothing) AndAlso (parentDepth >= 0) Then
                        Utility.SelectAndExpandToNode(node)
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub RenameDescription()
        Dim node As TreeNode
        Dim depth As Integer
        Dim parentValue As String = ""
        Dim newDesc As String
        Dim sql As String = ""
        Dim log As String = ""

        Try
            newDesc = txtDlgDescription.Text()

            If (treeComp.SelectedNode IsNot Nothing) Then
                depth = treeComp.SelectedNode.Depth
                parentValue = treeComp.SelectedNode.Value

                Select Case (depth)
                    Case TreeLevel.Component
                        sql = "UPDATE tblRMDComponents SET [Description] = '" + newDesc + "' WHERE (ComponentID = '" + treeComp.SelectedNode.Value + "')"
                        log = "Component " + treeComp.SelectedNode.Value + " was renamed to " + newDesc

                    Case TreeLevel.MaterialType
                        sql = "UPDATE tblRMDMaterialTypes SET [Description] = '" + newDesc + "'  WHERE (MaterialTypeID = '" + treeComp.SelectedNode.Value + "')"
                        log = "Material Type " + treeComp.SelectedNode.Value + " was renamed to " + newDesc

                    Case TreeLevel.MaterialListValue
                        sql = "UPDATE tblRMDListValues SET ListValue = '" + newDesc + "'  WHERE (ListValueID = '" + treeComp.SelectedNode.Value + "')"
                        log = "List Value " + treeComp.SelectedNode.Value + " was renamed to " + newDesc
                End Select

                DA.ExecSQL(Sql)
                Master.tMsg("Rename", log)

                PopulateTreeView()


                If (parentValue.Length > 0) Then
                    'expand and select the parent
                    node = Utility.FindNodeByValue(parentValue, treeComp.Nodes)
                Else
                    node = Utility.FindNodeByText(newDesc, treeComp.Nodes)
                End If

                If (node IsNot Nothing) Then
                    Utility.SelectAndExpandToNode(node)
                End If

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#Region "Add New"

    Private Sub AddNewDescription()
        Try
            Dim node As TreeNode
            Dim depth As Integer = 0
            Dim parentValue As String = ""
            Dim newDesc As String = txtDlgDescription.Text()

            If (treeComp.SelectedNode IsNot Nothing) Then
                depth = treeComp.SelectedNode.Depth
                parentValue = treeComp.SelectedNode.Value
            End If

            Select Case (depth)
                Case TreeLevel.Component
                    NewComponent(newDesc)

                Case TreeLevel.Category
                    NewMaterialType(newDesc, treeComp.SelectedNode.Value)

                Case TreeLevel.MaterialList
                    NewListValues(newDesc, treeComp.SelectedNode.Value)
            End Select

            PopulateTreeView()

            If (parentValue.Length > 0) Then
                'expand and select the parent
                node = Utility.FindNodeByValue(parentValue, treeComp.Nodes)
            Else
                node = Utility.FindNodeByText(newDesc, treeComp.Nodes)
            End If

            If (node IsNot Nothing) Then
                Utility.SelectAndExpandToNode(node)
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub NewComponent(desc As String)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Dim newID As String = ""
        Dim status As String = ""
        Dim message As String = ""

        Try

            'CREATE PROCEDURE procRMDNewComponent		@ComponentDesc varchar(80), 
            '							@ComponentID int out, 
            '							@Status varchar(80) out, 
            '							@ErrorMsg varchar(80) out  AS

            oSqlParameter = New SqlParameter("@ComponentDesc", SqlDbType.VarChar, 80)
            oSqlParameter.Value = desc
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ComponentID", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procRMDNewComponent", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@ComponentID" Then
                        newID = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status <> "True") Then
                Master.Msg = "Error: Unable to create new component.<br>S.P. Status: " + status + ".<br>S.P. Error Message: " + message
            Else
                Master.tMsg("New", "Component with ID: " + newID + ", description: " + desc + " was created.")
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub NewMaterialType(desc As String, ByVal catID As String)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Dim newID As String = ""
        Dim status As String = ""
        Dim message As String = ""

        Try
            'CREATE PROCEDURE procRMDNewMaterialType	@MaterialDesc varchar(80),
            '							@CategoryID int,
            '							@MaterialTypeID int out, 
            '							@Status varchar(80) out, 
            '							@ErrorMsg varchar(80) out  AS

            oSqlParameter = New SqlParameter("@MaterialDesc", SqlDbType.VarChar, 80)
            oSqlParameter.Value = desc
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@CategoryID", SqlDbType.Int)
            oSqlParameter.Value = catID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@MaterialTypeID", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procRMDNewMaterialType", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@MaterialTypeID" Then
                        newID = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status <> "True") Then
                Master.Msg = "Error: Unable to create new material type.<br>S.P. Status: " + status + ".<br>S.P. Error Message: " + message
            Else
                Master.tMsg("New", "Material Type with ID: " + newID + ", description: " + desc + " was created.")
            End If



        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub NewListValues(desc As String, ByVal matListID As String)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Dim newID As String = ""
        Dim status As String = ""
        Dim message As String = ""

        Try
            'CREATE PROCEDURE procRMDNewListValue	@ListValue varchar(80),
            '						@MaterialListID int,
            '						@ListValueID int out, 
            '						@Status varchar(80) out, 
            '						@ErrorMsg varchar(80) out  AS

            oSqlParameter = New SqlParameter("@ListValue", SqlDbType.VarChar, 80)
            oSqlParameter.Value = desc
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@MaterialListID", SqlDbType.Int)
            oSqlParameter.Value = matListID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ListValueID", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procRMDNewListValue", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@ListValueID" Then
                        newID = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status <> "True") Then
                Master.Msg = "Error: Unable to create new list value.<br>S.P. Status: " + status + ".<br>S.P. Error Message: " + message
            Else
                Master.tMsg("New", "List Value with ID: " + newID + ", description: " + desc + " was created.")
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Copy"

    Private Sub CopyDescription()
        Try
            Dim node As TreeNode
            Dim depth As Integer
            Dim parentValue As String = ""
            Dim newDesc As String = txtDlgDescription.Text()

            If (treeComp.SelectedNode IsNot Nothing) Then
                depth = treeComp.SelectedNode.Depth
                parentValue = treeComp.SelectedNode.Value

                Select Case (depth)
                    Case TreeLevel.Component
                        CopyComponent(newDesc, treeComp.SelectedNode.Value)
                    Case TreeLevel.MaterialType
                        CopyMaterialType(newDesc, treeComp.SelectedNode.Value, treeComp.SelectedNode.Parent.Value)
                    Case Else
                        Master.Msg = "Error: unable to determine selected item."
                End Select

                PopulateTreeView()

                If (parentValue.Length > 0) Then
                    'expand and select the parent
                    node = Utility.FindNodeByValue(parentValue, treeComp.Nodes)
                Else
                    node = Utility.FindNodeByText(newDesc, treeComp.Nodes)
                End If

                If (node IsNot Nothing) Then
                    Utility.SelectAndExpandToNode(node)
                End If

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub CopyComponent(newDesc As String, ID As String)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Dim newID As String = ""
        Dim status As String = ""
        Dim message As String = ""

        Try


            'CREATE PROCEDURE procRMDCopyComponent @oldCompID int, 
            '						@newCompDesc varchar(50), 
            '						@newCompID int out, 
            '						@Status varchar(80) out, @ErrorMsg varchar(80) out  AS

            oSqlParameter = New SqlParameter("@oldCompID", SqlDbType.Int)
            oSqlParameter.Value = ID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@newCompDesc", SqlDbType.VarChar, 50)
            oSqlParameter.Value = newDesc
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@newCompID", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procRMDCopyComponent", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@newCompID" Then
                        newID = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status <> "True") Then
                Master.Msg = "Error: Unable to copy component.<br>S.P. Status: " + status + ".<br>S.P. Error Message: " + message
            Else
                Master.tMsg("Copy", "Component with ID: " + newID + ", was copied to: " + newDesc)
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub CopyMaterialType(newDesc As String, ID As String, ByVal CatID As String)
        Dim oSqlParameter As SqlParameter
        Dim colParameters As New List(Of SqlParameter)
        Dim colOutput As List(Of SqlParameter)

        Dim newID As String = ""
        Dim status As String = ""
        Dim message As String = ""

        Try
            'CREATE PROCEDURE procRMDCopyMaterialType @oldTypeID int, 
            '						@catID int,
            '						@newTypeDesc varchar(50), 
            '						@newTypeID int out, 
            '						@Status varchar(80) out, @ErrorMsg varchar(80) out  AS


            oSqlParameter = New SqlParameter("@oldTypeID", SqlDbType.Int)
            oSqlParameter.Value = ID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@catID", SqlDbType.Int)
            oSqlParameter.Value = CatID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@newTypeDesc", SqlDbType.VarChar, 50)
            oSqlParameter.Value = newDesc
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@newTypeID", SqlDbType.Int)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@Status", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@ErrorMsg", SqlDbType.VarChar, 80)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procRMDCopyMaterialType", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output And .ParameterName = "@newTypeID" Then
                        newID = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                        status = oParameter.Value.ToString()
                    ElseIf .Direction = ParameterDirection.Output And .ParameterName = "@ErrorMsg" Then
                        message = oParameter.Value.ToString()
                    End If
                End With
            Next

            If (status <> "True") Then
                Master.Msg = "Error: Unable to copy component.<br>S.P. Status: " + status + ".<br>S.P. Error Message: " + message
            Else
                Master.tMsg("Copy", "Component with ID: " + newID + ", was copied to: " + newDesc)
            End If


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region




End Class