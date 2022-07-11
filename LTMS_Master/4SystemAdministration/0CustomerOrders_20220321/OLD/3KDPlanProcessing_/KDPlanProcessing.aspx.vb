Imports System
Imports System.IO
Imports System.Data.SqlClient
Imports Excel
Imports System.Globalization
Imports System.Linq


Public Class ImportFileFormatException
    Inherits Exception

    Public Sub New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
    End Sub
End Class

Public Class KDPlanProcessing
    Inherits System.Web.UI.Page

    Const QTY_PER_BATCH As Integer = 60
    Private BatchModuleApplicationID As String
    Private ImpersonDomain As String
    Private ImpersonUser As String
    Private ImpersonPass As String

    Private ActivateFrameTest As Boolean = False
    Private ActivateRecordLengthTest As Boolean = False
    Private ActivateQuantityTest As Boolean = False
    Private ActivateSequenceTest As Boolean = False
    Private SelectedMonth As Integer = 0
    Public Shared strEDICheck As String = ""
    Public Shared strProdLine As String = ""
    Public Shared ReferenceNumberID As String = ""
    Public Shared strKDEntireFileMonthArray(13) As String
    Public Shared strMsgArray(13) As String


#Region "Events"

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Try
            Master.SetScriptManagerTimeout(600)
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            BatchModuleApplicationID = ConfigurationManager.AppSettings("BatchModuleApplicationID")
            ActivateFrameTest = CBool(ConfigurationManager.AppSettings("BatchModuleActivateFrameTest"))
            ActivateRecordLengthTest = CBool(ConfigurationManager.AppSettings("BatchModuleActivateRecordLenghtTest"))
            ActivateQuantityTest = CBool(ConfigurationManager.AppSettings("BatchModuleActivateQuantityTest"))
            ActivateSequenceTest = CBool(ConfigurationManager.AppSettings("BatchModuleActivateSequenceTest"))

            ImpersonDomain = ReturnDomain()
            ImpersonUser = ReturnUser()
            ImpersonPass = ReturnPassword()

            If Not Page.IsPostBack Then
                If (ImpersonUser.ToString().Length > 0) Then
                    Using New Impersonation(ImpersonDomain, ImpersonUser, ImpersonPass)
                        'load files into directory tree listbox and clear file parameters
                        LoadFiles()
                        ResetControls()
                        Array.Clear(strKDEntireFileMonthArray, 0, strKDEntireFileMonthArray.Length)
                        Array.Clear(strMsgArray, 0, strMsgArray.Length)

                    End Using
                Else
                    LoadFiles()
                    ResetControls()
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Try
            Page.DataBind()
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub TreeView_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView.SelectedNodeChanged
        Dim node As TreeNode
        Dim strNvalues As New ArrayList
        Dim strDirIn As String = ReturnInbound()
        Dim strDirArc As String = ReturnOutbound()
        Dim x As Integer = 0
        Try
            'clear out all file information
            ResetControls()

            lblIssues.Text = "Batch Issues:"

            node = TreeView.SelectedNode
            If (node IsNot Nothing) AndAlso (node.Text <> strDirIn) AndAlso (node.Text <> strDirArc) Then
                lblFileName.Text = node.Text.ToString()

                If UCase(Right(lblFileName.Text, 3)) = "TXT" Then
                    cmdProcess.Enabled = True
                End If

                'Clear Status and dates
                tbIssues.Text = ""
                tbFileContents.Text = ""
                lbNFileDates.Items.Clear()

                If (ImpersonUser.ToString().Length > 0) Then
                    Using New Impersonation(ImpersonDomain, ImpersonUser, ImpersonPass)
                        strNvalues = ReturnNvalues(node.Parent.Text & node.Text)    'Call function to determine what N-values are in file
                    End Using
                Else
                    strNvalues = ReturnNvalues(node.Parent.Text & node.Text)    'Call function to determine what N-values are in file
                End If
            End If

            'if N-Values found in file
            If (strNvalues IsNot Nothing) AndAlso (strNvalues.Count > 0) Then
                For x = 0 To strNvalues.Count - 1
                    lbNfiles.Items.Add(strNvalues.Item(x).ToString()) 'populate the list box with N-values in the file
                Next
                If lbNfiles.Items.Count > 0 Then
                    lbNfiles.SelectedIndex = 0
                    ChangeSelectedKDPlan()
                End If
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    'call function to only display in text box the N-file for the current selection from the list box
    Private Sub lbNfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbNfiles.SelectedIndexChanged
        If lbNfiles.SelectedValue <> "" Then    'make sure an N-value is selected
            ChangeSelectedKDPlan()
        End If
    End Sub

    Private Sub cmdMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMove.Click
        'copy to outbound if file is located in the inbound directory or 
        'copy to inbound if the file is located in the outbound directory

        Try
            Dim strDirIn As String = ReturnInbound()
            Dim strDirArc As String = ReturnOutbound()
            Dim strFileDest As String
            Dim strFileSrc As String

            'get path of file selected
            Dim strfilepath As String = TreeView.SelectedNode.Parent.Text()


            strFileSrc = strfilepath + lblFileName.Text
            If strfilepath = strDirIn Then
                'from inbound so copy to outbound
                strFileDest = strDirArc + lblFileName.Text

            ElseIf strfilepath = strDirArc Then
                'from outbound so copy to inbound
                strFileDest = strDirIn + lblFileName.Text
            Else
                strFileDest = ""
            End If

            If (ImpersonUser.ToString().Length > 0) Then
                Using New Impersonation(ImpersonDomain, ImpersonUser, ImpersonPass)

                    'write to outbound, copy original file to new directory
                    File.Copy(strFileSrc, strFileDest)
                    'delete original file
                    File.Delete(strFileSrc)
                End Using
            Else
                'write to outbound, copy original file to new directory
                File.Copy(strFileSrc, strFileDest)
                'delete original file
                File.Delete(strFileSrc)
            End If


            'refresh the view after deleting file
            'load files into directory tree listbox and clear file parameters
            LoadFiles()
            ResetControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            'get path of current selection
            Dim strfilepath As String = TreeView.SelectedNode.Parent.Text()

            If (ImpersonUser.ToString().Length > 0) Then
                Using New Impersonation(ImpersonDomain, ImpersonUser, ImpersonPass)

                    File.Delete(strfilepath & lblFileName.Text)
                End Using
            Else
                File.Delete(strfilepath & lblFileName.Text)
            End If

            'refresh the view after deleting file
            'load files into directory tree listbox and clear file parameters
            LoadFiles()
            ResetControls()

        Catch ex As Exception
            Master.eMsg(ex.ToString())

        End Try
    End Sub

    Private Sub cmdProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdProcess.Click
        Dim strWarningMessage As String
        Dim strErrorRecords As String
        Dim strErrorFrame As String
        Dim NValue As String
        Dim objNode As New TreeNode
        Dim objNodeParent As New TreeNode
        Dim strArrayErrorRecords As New ArrayList
        Dim strArrayErrorFrame As New ArrayList
        Dim blnValidQuantity As Boolean
        Dim blnValidSequence As Boolean
        Dim strValidSequence As String
        Dim strValidQuantity As String
        Dim blnN0Indicator As Boolean
        Dim intMultiMonthFlag As Integer = 0
        Dim bContinue As Boolean = False
        Dim status As String
        Dim istxtFile As Boolean = False
        Dim processedRecords As Integer = 0

        ' Handle the Process dialog first:
        If dialogProcessAction.Value.ToUpper() = "PROCESS" Then
            'process button was clicked
            Me.divDialogProcessAccept_Click()

        ElseIf dialogProcessAction.Value.ToUpper() = "CANCEL" Then
            'cancel button was clicked
            divDialogProcessCancel_Click()
        ElseIf dialogProcessAction.Value.ToUpper() = "REPORT" Then
            'report button was clicked

            '(currently handled with the .click event.)
        End If
        'reset the process button idicator
        dialogProcessAction.Value = ""

        'determine if N-1 data already exists, if it does, prompt user whether this data is N-0 data
        'if labeled as n-1 file check if n-0
        If lbNfiles.SelectedValue <> "" Then
            If InStr(Trim(lbNfiles.SelectedValue.ToString), "N-1", CompareMethod.Text) > 0 Then 'N-1 Data Found
                If ReturnProcessed(NewKDPlanNValueMonth(lbNfiles.SelectedValue)) = 0 Then
                    blnN0Indicator = False
                    Session("N0Indicator") = "False"
                Else
                    'N-1 already processed, ask if this is N-0 data
                    blnN0Indicator = True
                    Session("N0Indicator") = "True"
                End If
            Else
                'Not N-1 Data
                blnN0Indicator = False
                Session("N0Indicator") = "False"
            End If
        End If

        Try

            objNode = TreeView.SelectedNode
            objNodeParent = objNode.Parent
            Dim strfilepath As String = objNodeParent.Text()
            istxtFile = strfilepath.Substring(strfilepath.Length - 3).Equals("txt")

            'set path to currently selected parent node
            If lbNfiles.SelectedValue.ToString <> "" Then    'check to make sure N-Value selected

                If InStr(NewKDPlanNValueMonth(lbNfiles.SelectedValue.ToString), "*", CompareMethod.Text) > 0 Then
                    ' Alert = "* found"
                    intMultiMonthFlag = 1
                End If

                NValue = lbNfiles.SelectedValue.Substring(0, 3)

                WriteNValue(NewKDPlanNValueMonth(lbNfiles.SelectedValue)) 'write N-value selected to temp table


                'create file then test file
                bContinue = False
                If (ImpersonUser.ToString().Length > 0) Then
                    Using New Impersonation(ImpersonDomain, ImpersonUser, ImpersonPass)
                        bContinue = CreateDtsFile(ReturnDTSFile(), strArrayErrorFrame, strArrayErrorRecords, blnValidQuantity, blnValidSequence)

                    End Using
                Else
                    'without impersonation
                    bContinue = CreateDtsFile(ReturnDTSFile(), strArrayErrorFrame, strArrayErrorRecords, blnValidQuantity, blnValidSequence)
                End If

                If bContinue = True Then

                    strErrorFrame = ""
                    If strArrayErrorFrame.Count <> 0 And istxtFile Then
                        'there were error in the frame codes
                        strErrorFrame = "The following Frame codes were found In the incoming " & lbNfiles.SelectedValue & " file, but are Not configured In JIT Manager.  " &
                        "This file can Not be processed until the following frame codes have been added To the system Or added To the No-Build Indicator." & Environment.NewLine

                        For i = 0 To strArrayErrorFrame.Count - 1
                            strErrorFrame = strErrorFrame & strArrayErrorFrame.Item(i).ToString() & Environment.NewLine
                        Next i
                    End If

                    'populate warning messsage if error in Record Length
                    strErrorRecords = ""
                    If strArrayErrorRecords.Count <> 0 And istxtFile Then
                        'add lines to the text message to display in the error popup.
                        For i = 0 To strArrayErrorRecords.Count - 1
                            strErrorRecords = strErrorRecords & strArrayErrorRecords.Item(i).ToString() & ", "
                        Next i
                        strErrorRecords = "On Line " & strErrorRecords
                        strErrorRecords = strErrorRecords & "Of the incoming file, the length Of characters  does Not meet the configured length range.  " &
                        "Please verify that this line contains valid data. "

                    End If

                    strValidQuantity = ""
                    If blnValidQuantity = False And istxtFile Then
                        'quantity validation failed so add this message to popup.
                        strValidQuantity = "The quantity found In the incoming N-1 file does Not match the quantity found In the current N-2 data."
                    End If

                    strValidSequence = ""
                    If blnValidSequence = False And istxtFile Then
                        'sequence validation failed, so add this message to popup.
                        strValidSequence = "Sequence validation failed, KD-plan selected will be processed out Of order." & Environment.NewLine
                    End If

                    strWarningMessage = ""
                    If strErrorFrame <> "" Then
                        strWarningMessage = strWarningMessage & strErrorFrame & Environment.NewLine
                    End If
                    If strValidSequence <> "" Then
                        strWarningMessage = strWarningMessage & strValidSequence & Environment.NewLine
                    End If
                    If strErrorRecords <> "" Then
                        strWarningMessage = strWarningMessage & strErrorRecords & Environment.NewLine
                    End If
                    If strValidQuantity <> "" Then
                        strWarningMessage = strWarningMessage & Environment.NewLine & strValidQuantity & Environment.NewLine
                    End If

                    If strArrayErrorRecords.Count = 0 And strArrayErrorFrame.Count = 0 And blnValidQuantity = True And blnValidSequence = True Then
                        'if validation true, then no errors
                        If captureValidFlag.Value.ToString.ToUpper() = "True" Then
                            'user clicked ok to validation and process anyway
                            'call function to write DTS parameters including file name and N-Value to process
                            WriteNValue(NewKDPlanNValueMonth(lbNfiles.SelectedValue))

                            'call final process file popup here if
                            If captureProcessFlag.Value.ToString.ToUpper() = "" Then
                                If intMultiMonthFlag = 1 Then
                                    strWarningMessage = "The File Contains Data Outside Of the Month Specified!  Are you sure you want To process " & NewKDPlanNValueMonth(lbNfiles.SelectedValue) & "data?"
                                Else
                                    strWarningMessage = "Are you sure you want to process " & NewKDPlanNValueMonth(lbNfiles.SelectedValue) & "data?"
                                End If
                                'Page.ClientScript.RegisterStartupScript(Me.GetType, "captureProcess", "<script>captureProcess('" & strWarningMessage & "')</script>")

                                divDialogProcessMessage.InnerHtml = strWarningMessage
                                dialogControl.Value = "captureProcess"
                            ElseIf captureProcessFlag.Value.ToString.ToUpper = "TRUE" Then
                                'On second time through if process popup true then
                                captureProcessFlag.Value = ""
                                'process file
                                If Session("KDProcess").ToString.ToUpper() = "TRUE" Then
                                    status = ""
                                    If NValue = "N-1" Or NValue = "830" Then
                                        If Session("N0Process").ToString.ToUpper() = "TRUE" Then       'check if user clicked ok to N-0 Processing
                                            'process as an N-0 file
                                            Session("N0Process") = "FALSE"
                                            status = ImportFile(lbNfiles.SelectedValue.ToString())
                                        Else
                                            'process as an N-1 file
                                            Session("KDProcess") = "UNKNOWN"
                                            status = ImportFile(lbNfiles.SelectedValue.ToString())

                                        End If
                                    ElseIf NValue = "N-2" Or NValue = "N-3" Then
                                        'process as an N-2 or N-3 file
                                        Session("KDProcess") = "UNKNOWN"
                                        status = ImportFile(lbNfiles.SelectedValue.ToString())
                                    End If
                                    captureProcessFlag.Value = "FALSE"

                                    processedRecords = ReturnProcessed(NewKDPlanNValueMonth(lbNfiles.SelectedValue))

                                    If status = "1" And NValue = "N-0" Then
                                        lblStatus.Text = "Processed"
                                        Master.tMsg("cmdProcess", lbNfiles.SelectedValue & " data has been successfully loaded from file " & lblFileName.Text)
                                    ElseIf status = "1" And processedRecords > 0 Then
                                        lblStatus.Text = "Processed"
                                        Master.tMsg("cmdProcess", lbNfiles.SelectedValue & " data has been successfully loaded from file " & lblFileName.Text)
                                    ElseIf status = "1" And processedRecords = 0 Then
                                        lblStatus.Text = "Processed"
                                        Master.tMsg("cmdProcess", lbNfiles.SelectedValue & " data was processed but no new lots were found in file " & lblFileName.Text)
                                    Else
                                        lblStatus.Text = "Error Processing"
                                        Master.tMsg("cmdProcess", "Warning! File " & lblFileName.Text & " encountered an error processing " & lbNfiles.SelectedValue & " data.")
                                    End If
                                End If
                            End If
                        End If
                        If captureProcessFlag.Value.ToString.ToUpper = "" Then
                            'first time through, so popup confirmation popup box
                            If intMultiMonthFlag = 1 Then
                                strWarningMessage = "The File Contains Data Outside of the Month Specified!  Are you sure you want to process: " & lbNfiles.SelectedValue & " data?"
                            Else
                                strWarningMessage = "Are you sure you want to process: " & NewKDPlanNValueMonth(lbNfiles.SelectedValue) & " data?"
                            End If
                            'Page.ClientScript.RegisterStartupScript(Me.GetType, "captureProcess", "<script>captureProcess('" & strWarningMessage & "')</script>")

                            divDialogProcessMessage.InnerHtml = strWarningMessage
                            dialogControl.Value = "captureProcess"
                        End If
                        If captureProcessFlag.Value.ToString.ToUpper = "TRUE" Then
                            'if process popup true then process the file.
                            captureValidFlag.Value = ""

                            'process file
                            If Session("KDProcess").ToString.ToUpper() = "TRUE" Then
                                status = ""
                                If NValue = "N-1" Or NValue = "830" Then
                                    If Session("N0Process").ToString.ToUpper() = "TRUE" Then       'check if user clicked ok to N-0 Processing
                                        'process as an N-0 file
                                        Session("N0Process") = "FALSE"
                                        status = ImportFile(lbNfiles.SelectedValue.ToString())
                                    Else
                                        'process as an N-1 file
                                        Session("KDProcess") = "UNKNOWN"
                                        status = ImportFile(lbNfiles.SelectedValue.ToString())

                                    End If
                                ElseIf NValue = "N-2" Or NValue = "N-3" Then
                                    'process as an N-2 or N-3 file
                                    Session("KDProcess") = "UNKNOWN"
                                    status = ImportFile(lbNfiles.SelectedValue.ToString())
                                End If
                                captureProcessFlag.Value = "FALSE"

                                processedRecords = ReturnProcessed(NewKDPlanNValueMonth(lbNfiles.SelectedValue))

                                If status = "1" And NValue = "N-0" Then
                                    lblStatus.Text = "Processed"
                                    Master.tMsg("cmdProcess", lbNfiles.SelectedValue & " data has been successfully loaded from file " & lblFileName.Text)
                                ElseIf status = "1" And processedRecords > 0 Then
                                    lblStatus.Text = "Processed"
                                    Master.tMsg("cmdProcess", lbNfiles.SelectedValue & " data has been successfully loaded from file " & lblFileName.Text)
                                ElseIf status = "1" And processedRecords = 0 Then
                                    lblStatus.Text = "Processed"
                                    Master.tMsg("cmdProcess", lbNfiles.SelectedValue & " data was processed but no new lots were found in file " & lblFileName.Text)
                                Else
                                    lblStatus.Text = "Error Processing"
                                    Master.tMsg("cmdProcess", "Warning! File " & lblFileName.Text & " encountered an error processing " & lbNfiles.SelectedValue & " data.")
                                End If

                            End If
                        End If
                    Else
                        If captureValidFlag.Value.ToString.ToUpper = "" Then
                            If strArrayErrorFrame.Count <> 0 Then
                                'frame errors, so setup popup dialog for critical validation alert
                                tbConfirmValidDisabledMessage.Text = Regex.Replace(strWarningMessage, "[|]", Environment.NewLine)
                                'dialogMessage01.Value = strWarningMessage
                                dialogControl.Value = "captureValidDisabled"
                            Else
                                'no frame errors, so non-critical validation
                                'Page.ClientScript.RegisterStartupScript(Me.GetType, "captureValid", "<script>captureValid('" & strWarningMessage & "')</script>")

                                tbConfirmValidMessage.Text = strWarningMessage
                                dialogControl.Value = "captureValid"
                            End If
                        End If
                        If captureValidFlag.Value.ToString.ToUpper = "TRUE" Then
                            'call function to write DTS parameters including file name and N-Value to process
                            WriteNValue(NewKDPlanNValueMonth(lbNfiles.SelectedValue))

                            'call process popup here if process null
                            If captureProcessFlag.Value.ToString.ToUpper = "" Then
                                If intMultiMonthFlag = 1 Then
                                    strWarningMessage = "The File Contains Data Outside of the Month Specified!  Are you sure you want to process: " & NewKDPlanNValueMonth(lbNfiles.SelectedValue) & " data?"
                                Else
                                    strWarningMessage = "Are you sure you want to process: " & NewKDPlanNValueMonth(lbNfiles.SelectedValue) & " data?"
                                End If
                                'Page.ClientScript.RegisterStartupScript(Me.GetType, "captureProcess", "<script>captureProcess('" & strWarningMessage & "')</script>")

                                divDialogProcessMessage.InnerHtml = strWarningMessage
                                dialogControl.Value = "captureProcess"
                            ElseIf captureProcessFlag.Value.ToString.ToUpper = "TRUE" Then
                                'if process popup true then process file
                                captureValidFlag.Value = ""
                                'make sure user clicked ok and verify this
                                If Session("KDProcess").ToString.ToUpper() = "TRUE" Then
                                    status = ""
                                    If NValue = "N-1" Or NValue = "830" Then
                                        If Session("N0Process").ToString.ToUpper() = "TRUE" Then       'check if user clicked ok to N-0 Processing
                                            'process as an N-0 file
                                            Session("N0Process") = "FALSE"
                                            status = ImportFile(NewKDPlanNValueMonth(lbNfiles.SelectedValue.ToString()))
                                        Else
                                            'process as an N-1 file
                                            Session("KDProcess") = "UNKNOWN"
                                            status = ImportFile(NewKDPlanNValueMonth(lbNfiles.SelectedValue.ToString()))

                                        End If
                                    ElseIf NValue = "N-2" Or NValue = "N-3" Then
                                        'process as an N-2 or N-3 file
                                        Session("KDProcess") = "UNKNOWN"
                                        status = ImportFile(lbNfiles.SelectedValue.ToString())
                                    End If
                                    captureProcessFlag.Value = "FALSE"

                                    processedRecords = ReturnProcessed(NewKDPlanNValueMonth(lbNfiles.SelectedValue))

                                    If status = "1" And NValue = "N-0" Or NValue = "830" Then
                                        lblStatus.Text = "Processed"
                                        Master.tMsg("cmdProcess", NewKDPlanNValueMonth(lbNfiles.SelectedValue) & " data has been successfully loaded from file " & lblFileName.Text)
                                    ElseIf status = "1" And processedRecords > 0 Then
                                        lblStatus.Text = "Processed"
                                        Master.tMsg("cmdProcess", NewKDPlanNValueMonth(lbNfiles.SelectedValue) & " data has been successfully loaded from file " & lblFileName.Text)
                                    ElseIf status = "1" And processedRecords = 0 Then
                                        lblStatus.Text = "Processed"
                                        Master.tMsg("cmdProcess", NewKDPlanNValueMonth(lbNfiles.SelectedValue) & " data was processed but no new lots were found in file " & lblFileName.Text)
                                    Else
                                        lblStatus.Text = "Error Processing"
                                        Master.tMsg("cmdProcess", "Warning! File " & lblFileName.Text & " encountered an error processing " & lbNfiles.SelectedValue & " data.")
                                    End If

                                End If
                            ElseIf captureProcessFlag.Value.ToString.ToUpper = "FALSE" Then
                                'user clicked cancel
                                captureValidFlag.Value = ""
                            End If
                        End If
                        If captureValidFlag.Value.ToString.ToUpper = "FALSE" Then
                            captureValidFlag.Value = ""
                        End If
                    End If
                    captureProcessFlag.Value = ""

                End If
            Else
                Master.tMsg("cmdProcess", "Please choose a KD-plan to process!")
            End If
        Catch ex As Exception
            lblStatus.Text = "Error Processing"
            Master.eMsg(ex.Source)
            Master.tMsg("cmdProcess", "Warning! File " & lblFileName.Text & " encountered an error processing " & lbNfiles.SelectedValue & " data. Error: " & ex.ToString())
        End Try
    End Sub

    Private Function NewKDPlanNValueMonth(sInput As String) As String

        Dim iMonthSelected As Integer = 0
        Dim iYearSelected As Integer = 0
        Dim sMonthSelected As String = ""
        Dim nvalue As String = ""
        Dim sYear As String = ""
        Dim sRest As String = ""



        On Error Resume Next
        iMonthSelected = CInt(lbNFileDates.SelectedValue.Substring(7, 2))
        iYearSelected = CInt(lbNFileDates.SelectedValue.Substring(0, 4))

        'First Get string month
        Select Case iMonthSelected
            Case 0
                sMonthSelected = sInput
            Case 1
                sMonthSelected = "January"
            Case 2
                sMonthSelected = "February"
            Case 3
                sMonthSelected = "March"
            Case 4
                sMonthSelected = "April"
            Case 5
                sMonthSelected = "May"
            Case 6
                sMonthSelected = "June"
            Case 7
                sMonthSelected = "July"
            Case 8
                sMonthSelected = "August"
            Case 9
                sMonthSelected = "September"
            Case 10
                sMonthSelected = "October"
            Case 11
                sMonthSelected = "November"
            Case 12
                sMonthSelected = "December"
        End Select

        If iMonthSelected <> 0 Then
            'Get the N value
            nvalue = sInput.Substring(0, 3).Trim

            'Get the rest
            sRest = sInput.Substring(sInput.LastIndexOf("-"))
            'Concatinate 
            NewKDPlanNValueMonth = nvalue & " " & sMonthSelected & " " & iYearSelected & " " & sRest
        Else
            NewKDPlanNValueMonth = sInput
        End If

    End Function
    Private Function ConvertToMonth(sInput As String) As String

        Dim sMonthSelected As String = ""

        Try
            Select Case CInt(sInput)
                Case 1
                    sMonthSelected = "January"
                Case 2
                    sMonthSelected = "February"
                Case 3
                    sMonthSelected = "March"
                Case 4
                    sMonthSelected = "April"
                Case 5
                    sMonthSelected = "May"
                Case 6
                    sMonthSelected = "June"
                Case 7
                    sMonthSelected = "July"
                Case 8
                    sMonthSelected = "August"
                Case 9
                    sMonthSelected = "September"
                Case 10
                    sMonthSelected = "October"
                Case 11
                    sMonthSelected = "November"
                Case 12
                    sMonthSelected = "December"
            End Select

            ConvertToMonth = sMonthSelected

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Function

#End Region

#Region "Methods"
    Private Sub ChangeSelectedKDPlan()
        Try
            'find N-value selected.
            Dim strNValue As String = lbNfiles.SelectedValue.ToString

            'find currently selected file
            Dim strfilepath As String = TreeView.SelectedNode.Parent.Text.ToString()

            If (ImpersonUser.ToString().Length > 0) Then
                Using New Impersonation(ImpersonDomain, ImpersonUser, ImpersonPass)
                    'call function to write only the selected plan to the textbox.
                    WriteKDPlanSelected(strfilepath & lblFileName.Text, strNValue)

                End Using
            Else
                'call function to write only the selected plan to the textbox without impersonation.
                WriteKDPlanSelected(strfilepath & lblFileName.Text, strNValue)
            End If


            'query database to determine if selected N-value is in table and therefore KD plan has been processed
            'if the data is N-0 there will never be an N-0 Value in the Orders Table so do not have to worry about this condition
            Dim intCompare As Integer = ReturnProcessed(strNValue)  'returns count of records with N-value
            If intCompare > 0 Then
                'set to processed
                lblStatus.Text = "Processed"

            Else
                'set to not processed
                lblStatus.Text = "Not Processed"

            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub LoadFiles()
        Try
            Dim sfiles() As String
            Dim sfile As String
            Dim strDirIn As String = ReturnInbound()
            Dim strDirArc As String = ReturnOutbound()

            Dim objChildNode As TreeNode
            Dim objParentNode As TreeNode

            Dim x As Integer = 0

            'clear out tree control
            TreeView.Nodes.Clear()

            '********  INCOMMING  ***************
            If Len(strDirIn) > 0 Then
                objParentNode = New TreeNode
                objParentNode.Text = strDirIn
                objParentNode.ImageUrl = "../../../Images/TreeView/dir_open.gif"

                'add parent node to root level
                TreeView.Nodes.Add(objParentNode)

                If (ImpersonUser.ToString().Length > 0) Then
                    Using New Impersonation(ImpersonDomain, ImpersonUser, ImpersonPass)
                        sfiles = GetListOfFilesWithoutPath(strDirIn)
                    End Using
                Else
                    sfiles = GetListOfFilesWithoutPath(strDirIn)
                End If

                'now add child node
                For Each sfile In sfiles
                    objChildNode = New TreeNode
                    objChildNode.Text = sfile
                    If (sfile.Substring(sfile.Length - 3) = "txt") Then
                        objChildNode.ImageUrl = "../../../Images/TreeView/aspxdoc.gif"
                    Else
                        objChildNode.ImageUrl = "../../../Images/TreeView/excel.png"
                    End If
                    objParentNode.ChildNodes.Add(objChildNode)
                Next
            Else
                Master.eMsg("Error: Inbound directory isn't configured.")
            End If


            '***********  OUTGOING  ************
            sfiles = Nothing
            If Len(strDirArc) > 0 Then

                objParentNode = New TreeNode
                objParentNode.Text = strDirArc
                objParentNode.ImageUrl = "../../../Images/TreeView/dir_open.gif"

                'add parent node to root level
                TreeView.Nodes.Add(objParentNode)

                If (ImpersonUser.ToString().Length > 0) Then
                    Using New Impersonation(ImpersonDomain, ImpersonUser, ImpersonPass)

                        sfiles = GetListOfFilesWithoutPath(strDirArc)
                    End Using
                Else
                    'without impersonation
                    sfiles = GetListOfFilesWithoutPath(strDirArc)
                End If

                'now add child nodes
                For Each sfile In sfiles
                    objChildNode = New TreeNode
                    objChildNode.Text = sfile
                    If (sfile.Substring(sfile.Length - 3) = "txt") Then
                        objChildNode.ImageUrl = "../../../Images/TreeView/aspxdoc.gif"
                    Else
                        objChildNode.ImageUrl = "../../../Images/TreeView/excel.png"
                    End If
                    objParentNode.ChildNodes.Add(objChildNode)
                Next
            Else
                Master.eMsg("Error: Outbound directory isn't configured.")
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Function ReturnNvalues(ByVal path As String) As ArrayList
        'this function will return the n values in current file.  These are used for populating list box, the file status, and file date
        'update 1-20-2006 modify so for N-0 files, it will count the number of records of each and 
        'then populate based on that.

        Dim strReturnNvalues As New ArrayList
        Dim strLine As String
        Dim strModstrJobs As String
        Dim strModKDPlan As String = ""
        Dim strDetailRecord As String
        Dim strFrameCodeFromFile As String = ""
        Dim lstNoBuildIdentifierArray As New ArrayList
        Dim strKDEntireFile As String = ""
        Dim mnth As String
        Dim yr As String
        Dim day As String
        Dim key As String
        Dim keyCount As Integer
        Dim dateCounts As New Hashtable
        Dim de As DictionaryEntry
        Dim maxDate As DictionaryEntry
        Dim dt As DateTime
        Dim ext As String
        Dim noBuildStartPosition As Integer
        Dim noBuildStringLength As Integer
        Dim lineLength As Integer
        Dim excelReader As IExcelDataReader
        Dim dsResult As DataSet
        Dim marker As String = ""
        Dim qty As Integer
        Dim batchno As Integer
        Dim lstbatchno As Integer
        Dim strPartNumberFromFile As String = ""
        Dim strLotNumber As String = ""
        Dim multiListRefID As New List(Of String) '2 Char Reference ID related to broadcast point

        lblAsterick.Text = ""   'reset the flag
        lblAsterick.Visible = False

        noBuildStartPosition = ReturnNoBuildStartPosition()
        noBuildStringLength = ReturnNoBuildStringLength()
        lineLength = ReturnStringLength() 'return valid record length for file, this is configured in the database.

        Try
            'build array of identifiers for "no build frame codes"
            For Each Str As String In ReturnNoBuildStringIdentifiers().Split(","c)
                lstNoBuildIdentifierArray.Add(Str)
            Next

            ext = path.Substring(path.Length - 3)

            If (ext.Equals("txt")) Then

                'open a stream to path passed in
                Using sReader = New StreamReader(path)

                    ' ********'READ'*********
                    strLine = Trim(sReader.ReadLine)  'read in very first line which should be header record
                    strLine = strLine.Replace(Chr(9), "")
                    Do While strLine = ""   'skip blank lines
                        strLine = Trim(sReader.ReadLine)
                    Loop

                    strEDICheck = strLine.Substring(0, 4)


                    If strEDICheck = "ISA*" Then
                        MonthSelection.Visible = False
                        'have EDI 830 file - 20190318

                        '' get 832 date from first record in file and format it
                        ''read in the file send date
                        mnth = strLine.Substring(70, 6).Substring(2, 2)    'parse out month
                        day = strLine.Substring(70, 6).Substring(4, 2) 'parse out day
                        yr = strLine.Substring(70, 6).Substring(0, 2) 'parse out year

                        strModKDPlan = mnth & "/" & day & "/" & yr

                        'Get the Production Line
                        Do Until sReader.Peek = -1
                            Do Until strLine.Contains("ST*830*") Or sReader.Peek = -1
                                strLine = Trim(sReader.ReadLine())
                            Loop

                            If sReader.Peek = -1 Then
                                Exit Do
                            End If

                            'Get the 2 character Reference ID
                            multiListRefID.Add(strLine.Substring(GetNthIndex(strLine, "*", 2) + 1, 2))
                            strLine = Trim(sReader.ReadLine())

                        Loop
                        If multiListRefID.Count <> 0 Then
                            For i = 0 To multiListRefID.Count
                                strProdLine = BizLayer.GetBroadcastPoint("0034", multiListRefID(i))
                                If strProdLine <> "" Then
                                    ReferenceNumberID = multiListRefID(i).Trim
                                    Exit For
                                End If
                            Next
                        Else
                            tbIssues.Text = "File Does Not Include An 830 Section."
                            tbFileContents.Text = ""
                            Exit Function
                        End If

                        'Add the day so separate file same month won't be marked as Processed.
                        strReturnNvalues.Add("830 " & CDate(strModKDPlan).ToString("MMMM dd yyyy") & " - Line " & strProdLine & marker)

                    Else
                        MonthSelection.Visible = False
                        ' get KD plan date from first record in file and format it
                        'read in the file send date
                        mnth = strLine.Substring(51, 6).Substring(2, 2)    'parse out month
                        day = strLine.Substring(51, 6).Substring(4, 2) 'parse out day
                        yr = strLine.Substring(51, 6).Substring(0, 2) 'parse out year

                        lblSendDateTitle.Text = "KD Plan Send Date:"
                        lblNFiles.Text = "KD Plans Found in File:"

                        strModstrJobs = mnth & "/" & "01" & "/" & yr
                        lblSendDate.Text = mnth & "/" & day & "/" & yr 'set the file send date
                        strDetailRecord = ""

                        If strLine = "" Then        'check for invalid records, extra spaces in file, or end of file
                            strLine = Trim(sReader.ReadLine)
                        Else
                            'find the month and year of the lot to be created
                            strDetailRecord = Trim(strLine).Substring(0, 7)
                        End If

                        Do While (sReader.Peek <> -1)  '1 record has been read at this point
                            'keep processing rows until the trailer record is reached
                            Do Until strDetailRecord = "**TRAIL"
                                If strDetailRecord = "**HEADE" Then
                                    Exit Do 'means found 2nd header record, done with detail records
                                Else 'start processing detail records
                                    '////////detail records
                                    strFrameCodeFromFile = strLine.Substring(11, 7) & strLine.Substring(31, 1)
                                    'check for production line and exit do if not configured line
                                    strProdLine = strLine.Substring(2, 1)

                                    If lstNoBuildIdentifierArray.Contains(UCase(strFrameCodeFromFile.Substring(noBuildStartPosition - 1, noBuildStringLength))) Then
                                        'skip the line because no build frame code found
                                        'JMH(framecount isn't used anywhere)       intframecount = intframecount + 1
                                    Else
                                        'count up the dates
                                        key = strLine.Substring(3, 4)
                                        'check that key does not already exist and also that it is not a no-build
                                        If Not dateCounts.ContainsKey(key) Then
                                            dateCounts.Add(key, 1)
                                        Else
                                            keyCount = CInt(dateCounts.Item(key))
                                            keyCount = keyCount + 1
                                            dateCounts.Item(key) = keyCount
                                        End If

                                    End If

                                    '/////////detail records
                                End If
                                'end processing detail records

                                'after read in header record
                                strLine = Trim(sReader.ReadLine) ' ******'READ'******
                                strLine = strLine.Replace(Chr(9), "")
                                If strLine = "" Then        'check for invalid records, extra spaces in file, or end of file
                                    strLine = Trim(sReader.ReadLine)
                                Else
                                    'find the month and year of the lot to be created
                                    strDetailRecord = Trim(strLine).Substring(0, 7)
                                End If
                            Loop

                            'read records after the header record  ' ******'READ'******
                            strLine = Trim(sReader.ReadLine)
                            strLine = strLine.Replace(Chr(9), "") ' remove tabs
                            If strLine.Length < lineLength - 20 Then
                                sReader.ReadLine()
                            End If
                            If strLine = "" Then        'check for invalid records, extra spaces in file, or end of file
                                strLine = Trim(sReader.ReadLine)
                            Else
                                'find the month and year of the lot to be created
                                strDetailRecord = Trim(strLine).Substring(0, 7)
                            End If

                            'determine max of what was passed in and set this to be returned
                            If Not dateCounts.Count = 0 Then 'takes care of first pass
                                For Each de In dateCounts
                                    If IsNothing(maxDate) OrElse CInt(de.Value) > CInt(maxDate.Value) Then
                                        maxDate = de
                                    End If
                                Next

                                dt = DateTime.ParseExact(maxDate.Key.ToString(), "yyMM", Nothing)
                                mnth = dt.Month.ToString()
                                yr = dt.Year.ToString()
                                strModKDPlan = mnth & "/" & "01" & "/" & yr

                                'determine if N-1, N-2 or N-3, or N-0, or N-0* with data from two months
                                Select Case CInt((DateDiff(DateInterval.Month, CDate(strModstrJobs), CDate(strModKDPlan))))
                                    Case 0 'found N-0 data
                                        If dateCounts.Keys.Count > 1 Then 'found two dates in the file, so set asterick data
                                            lblAsterick.Text = "* File Contains Data Outside of Month Specified"
                                            marker = "*"
                                        Else
                                            lblAsterick.Text = "* Flagged as N-0 File, Double Check"
                                            marker = "*"
                                        End If
                                        strReturnNvalues.Add("N-1 " & Format(CDate(strModKDPlan), "MMMM yyyy") & " - Line " & strProdLine & marker)

                                    Case 1 'found N-1 data 
                                        If dateCounts.Keys.Count > 1 Then 'found two dates in the file, so set asterick data
                                            lblAsterick.Text = "* File Contains Data Outside of Month Specified"
                                            marker = "*"
                                        Else : marker = ""
                                        End If
                                        strReturnNvalues.Add("N-1 " & Format(CDate(DateAdd(DateInterval.Month, 1, CDate(strModstrJobs))), "MMMM yyyy") & " - Line " & strProdLine & marker)

                                    Case 2 'found N-2 data
                                        strReturnNvalues.Add("N-2 " & Format(CDate(DateAdd(DateInterval.Month, 2, CDate(strModstrJobs))), "MMMM yyyy") & " - Line " & strProdLine)

                                    Case 3 'found N-3 data
                                        strReturnNvalues.Add("N-3 " & Format(CDate(DateAdd(DateInterval.Month, 3, CDate(strModstrJobs))), "MMMM yyyy") & " - Line " & strProdLine)

                                End Select

                                dateCounts.Clear()
                                keyCount = 0
                                maxDate.Value = Nothing
                            End If
                        Loop
                    End If
                End Using

            ElseIf (ext.Equals("xls") Or ext.Equals("lsx")) Then

                MonthSelection.Visible = True
                lblSendDateTitle.Text = "POP File Send Date:"
                lblNFiles.Text = "POP File: "

                Using stream As FileStream = File.Open(path, FileMode.Open, FileAccess.Read)

                    If (ext.Equals("xls")) Then
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream) 'Reading from a binary Excel file ('97-2003 format; *.xls)
                    Else
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream) 'Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    End If

                    Using (excelReader)
                        excelReader.IsFirstRowAsColumnNames = True  'DataSet - Create column names from first row

                        dsResult = excelReader.AsDataSet() 'DataSet - The result of each spreadsheet will be created in the result.Tables
                        dsResult.Tables(0).Rows.RemoveAt(0) ' this will remove 2nd row;  IsFirstRowAsColumnNames = True removes 1st row

                        'find first patch of 60; use that as the K-1 plan date
                        For Each row As DataRow In dsResult.Tables(0).Rows  '.Select("[Batch No] IS NOT NULL")
                            If ((Not row("Prod Dt").ToString.ToUpper().Contains("Daily Total:")) AndAlso (row("Batch No").ToString().Trim().Length <> 0) AndAlso (row("KD Lot No").ToString().Trim().Length <> 0)) Then
                                batchno = CInt(row("Batch No").ToString())
                                If lstbatchno <> batchno Then
                                    lstbatchno = batchno
                                    qty = 0
                                End If

                                qty = qty + CInt(row("Qty").ToString())
                                ''''''    'If qty = QTY_PER_BATCH Then
                                strLine = dsResult.Tables(0).Rows(0).Item("Prod Dt").ToString()
                                mnth = strLine.Substring(0, 2)  'parse out day
                                day = strLine.Substring(3, 2) 'parse out month
                                yr = strLine.Substring(6, 2)   'parse out year

                                strModKDPlan = mnth & "/" & day & "/" & yr
                                lblSendDate.Text = strModKDPlan 'set the file send date
                                strProdLine = row("Batch No").ToString().Substring(0, 1)
                                Exit For
                                ''''''''End If
                            End If
                        Next

                        excelReader.Close() 'Free resources (IExcelDataReader is IDisposable)
                    End Using
                End Using

                strReturnNvalues.Add("N-1 " & Format(CDate(strModKDPlan), "MMMM dd yyyy") & " - Line " & strProdLine & marker)

            End If

            If (lblAsterick.Text.Length > 0) Then
                lblAsterick.Visible = True
            Else
                lblAsterick.Visible = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally

        End Try

        Return strReturnNvalues
    End Function

    Function ReturnFrameCode(ByVal PartR As String, ByVal PartL As String, ByVal PartMidR As String, ByVal PartMidL As String, ByVal PartRR As String, ByVal ColorCode As String) As String
        Try
            Dim colParms As New List(Of SqlParameter)
            Dim prmNext As Data.SqlClient.SqlParameter
            Dim ds As DataSet
            Dim Part3 As String = ""

            tbIssues.Text = ""
            ReturnFrameCode = ""

            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@RightPart", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = PartR
            colParms.Add(prmNext)
            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@LeftPart", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = PartL
            colParms.Add(prmNext)
            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@RightMIDPart", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = PartMidR
            colParms.Add(prmNext)
            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@LeftMIDPart", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = PartMidL
            colParms.Add(prmNext)
            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@RearPart", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            If PartRR <> "" Then 'If there is a rear part to supply
                prmNext.Value = PartRR
                Part3 = ", " + PartRR
            Else
                prmNext.Value = ""
            End If
            colParms.Add(prmNext)
            'IN--------------------------------------------------------
            prmNext = New Data.SqlClient.SqlParameter("@ColorCode", SqlDbType.VarChar, 48)
            prmNext.Direction = ParameterDirection.Input
            prmNext.Value = ColorCode
            colParms.Add(prmNext)

            ds = DA.GetDataSet("procGetFrameCode", colParms)
            colParms.Clear()

            If (ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0) Then
                tbIssues.Text = "No framecode defined in database for part set: " + PartR + ", " + PartL + Part3
                tbFileContents.Text = ""
            Else
                ReturnFrameCode = ds.Tables(0).Rows(0).Item(0).ToString
            End If

            If ReturnFrameCode = "99" Then
                tbIssues.Text = "No framecode defined in database for part set: " + PartR + ", " + PartL + Part3
                tbFileContents.Text = ""
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            ReturnFrameCode = "No framecode defined For Part Set!"
            tbFileContents.Text = ""
        End Try

    End Function
    Public Function GetNthIndex(searchString As String, charToFind As String, n As Integer) As Integer
        Dim charIndexPair = searchString.Select(Function(c, i) New With {.Character = c, .Index = i}) _
                                    .Where(Function(x) x.Character = charToFind) _
                                    .ElementAtOrDefault(n - 1)
        Return If(charIndexPair IsNot Nothing, charIndexPair.Index, -1)
    End Function

    Private Sub WriteKDPlanSelected(ByVal path As String, ByVal strNvalue As String)

        Dim dateCounts As New Hashtable
        Dim de As DictionaryEntry
        Dim dt As DateTime
        Dim maxDate As DictionaryEntry
        Dim key As String
        Dim keyCount As Integer
        Dim mnth As String
        Dim yr As String
        Dim day As String
        Dim strLine As String = ""
        Dim strItem As String = ""
        Dim strProdDate As String = ""
        Dim KDMonth As Integer = 0
        Dim strKDDate As String = ""
        Dim strNValueFoundinFile As String = ""
        Dim strKDEntireFile As String = ""

        Dim strModstrJobs As String
        Dim strModKDPlan As String
        Dim strDetailRecord As String
        Dim strFrameCodeFromFile As String
        Dim strFrameCodeFromFileCheck As String = ""
        Dim strFrameCodeFromFileEDI(13) As String
        Dim lstNoBuildIdentifierArray As New ArrayList
        Dim noBuildStartPosition As Integer
        Dim noBuildStringLength As Integer
        Dim lineLength As Integer
        Dim ext As String
        Dim ord As Integer
        Dim excelReader As IExcelDataReader
        Dim dsResult As DataSet
        Dim marker As String = ""
        Dim qty As Integer
        Dim TotalQtyLeft As Integer = 0
        Dim TotalQtyRight As Integer = 0
        Dim TotalQtyLeftMid As Integer = 0
        Dim TotalQtyRightMid As Integer = 0
        Dim TotalQtyRear As Integer = 0
        Dim TotalWeekLeft As Integer = 0
        Dim TotalWeekRight As Integer = 0
        Dim TotalWeekMidLeft As Integer = 0
        Dim TotalWeekMidRight As Integer = 0
        Dim TotalWeekRear As Integer = 0
        Dim DoingRightParts As Boolean = False
        Dim DoingLeftParts As Boolean = False
        Dim DoingRightMidParts As Boolean = False
        Dim DoingLeftMidParts As Boolean = False
        Dim DoingRearParts As Boolean = False
        Dim batchQty As Integer
        Dim batchno As Integer
        Dim lstbatchno As Integer
        Dim msg As String = "" '"File may be corrupt.  Batch sizes are too small. Refer To:  "
        Dim strData01 As String = ""
        Dim strData02 As String = ""
        Dim strData03 As String = ""
        Dim strData04 As String = ""
        Dim strData05 As String = ""
        Dim strData06 As String = ""
        Dim strData07 As String = ""
        Dim strData08 As String = ""
        Dim strData09 As String = ""
        Dim strData10 As String = ""
        Dim strData11 As String = ""
        Dim strData12 As String = ""
        Dim strPartNumberFromFile As String = ""
        Dim strLotNumber As String = ""
        Dim strLotNumber2 As String = ""
        Dim pos1 As Integer = 0
        Dim pos2 As Integer = 0
        Dim pos4 As Integer = 0
        Dim pos7 As Integer = 0
        Dim pos8 As Integer = 0
        Dim strLineMod As String = ""
        Dim weekcount As Integer = 0
        Dim j As Integer = 0

        Dim rt As Integer = 1
        Dim lt As Integer = 1
        Dim mrt As Integer = 1
        Dim mlt As Integer = 1
        Dim rr As Integer = 1

        Dim intJuianWeek As Integer = 0
        Dim ht As Hashtable = New Hashtable()
        Dim iSortCount As Integer = 100
        Dim iPartCount As Integer = 1
        Dim iPartX As Integer = 0


        Dim multiListR As New List(Of List(Of String)) 'Rights
        Dim multiListL As New List(Of List(Of String)) 'Lefts
        Dim multiListMidR As New List(Of List(Of String)) 'Mid Rights
        Dim multiListMidL As New List(Of List(Of String)) 'Mid Lefts
        Dim multiListRR As New List(Of List(Of String)) 'Rears
        Dim listRecordR As New List(Of String)
        Dim listRecordL As New List(Of String)
        Dim listRecordMidR As New List(Of String)
        Dim listRecordMidL As New List(Of String)
        Dim listRecordRR As New List(Of String)

        Dim SeatFrontRight As String = ""
        Dim SeatFrontLeft As String = ""
        Dim SeatMidRight As String = ""
        Dim SeatMidLeft As String = ""
        Dim SeatRear As String = ""
        Dim ColorFrontRight As String = ""
        Dim ColorFrontLeft As String = ""
        Dim ColorMidRight As String = ""
        Dim ColorMidLeft As String = ""
        Dim ColorRear As String = ""

        Dim CurrentPartColor As String = ""
        Dim WeekCounter As Integer = 0
        Dim strLineName As String = ""

        lblAsterick.Text = ""   'reset the flag
        lblAsterick.Visible = False

        Dim FrontRightsProcessed As Boolean = False
        Dim FrontLeftsProcessed As Boolean = False
        Dim MidRightsProcessed As Boolean = False
        Dim MidLeftsProcessed As Boolean = False
        Dim RearsProcessed As Boolean = False
        Dim CurrentPart As String = ""
        Dim lstModelExceptionIdentifierArray As New ArrayList

        noBuildStartPosition = ReturnNoBuildStartPosition()
        noBuildStringLength = ReturnNoBuildStringLength()
        lineLength = ReturnStringLength() 'return valid record length for file, this is configured in the database.

        '''Initialize
        listRecordR.Add("")
        listRecordL.Add("")
        listRecordMidR.Add("")
        listRecordMidL.Add("")
        listRecordRR.Add("")
        multiListR.Add(listRecordR)
        multiListL.Add(listRecordL)
        multiListMidR.Add(listRecordMidR)
        multiListMidL.Add(listRecordMidL)
        multiListRR.Add(listRecordRR)

        Try
            ext = path.Substring(path.Length - 3)

            If (ext.Equals("txt")) Then

                'open a stream to path passed in
                Using sReader = New StreamReader(path)

                    'build array of identifiers for "no build frame codes"
                    For Each Str As String In ReturnNoBuildStringIdentifiers().Split(","c)
                        lstNoBuildIdentifierArray.Add(Str)
                    Next

                    ' ******'READ'******
                    strLine = Trim(sReader.ReadLine)  'read in very first line which should be header record
                    strLine = strLine.Replace(Chr(9), "")
                    Do While strLine = ""
                        strLine = Trim(sReader.ReadLine)
                    Loop


                    'Is it an EDI file
                    strEDICheck = strLine.Substring(0, 4)

                    If strEDICheck = "ISA*" Then
                        'have EDI 830 file - 20190318 get 830 date from first record in file and format it as the file send date
                        mnth = strLine.Substring(70, 6).Substring(2, 2) 'parse out month
                        day = strLine.Substring(70, 6).Substring(4, 2) 'parse out day
                        yr = strLine.Substring(70, 6).Substring(0, 2) 'parse out year
                        strModKDPlan = mnth & "/" & "01" & "/" & yr

                        lblSendDateTitle.Text = "EDI 830 Send Date:"
                        lblNFiles.Text = "830 Plans Found in File:"

                        strModstrJobs = mnth & "/" & "01" & "/" & yr
                        lblSendDate.Text = mnth & "/" & day & "/" & yr 'set the file send date
                        strDetailRecord = ""

                        Do Until sReader.Peek = -1

                            Do Until strLine.Contains("ST*830*" & ReferenceNumberID) Or sReader.Peek = -1
                                strLine = Trim(sReader.ReadLine())
                            Loop

                            strLine = Trim(sReader.ReadLine())

                            Do Until strLine.Contains("LIN*") Or sReader.Peek = -1
                                strLine = Trim(sReader.ReadLine())
                                If strLine.Contains("LIN*") Then
                                    If (strLine.Contains("BP*81100")) Then
                                        'get data from the line
                                        pos4 = GetNthIndex(strLine, "*", 4) + 1
                                        multiListR.Add(New List(Of String))
                                        multiListR(iPartX).Add(strLine.Substring(8, 11)) 'Part
                                        multiListR(iPartX).Add(strLine.Substring(pos4 - 7, 6)) 'Color Code
                                    ElseIf (strLine.Contains("BP*81500")) Then
                                        'get data from the line
                                        pos4 = GetNthIndex(strLine, "*", 4) + 1
                                        multiListL.Add(New List(Of String))
                                        multiListL(iPartX).Add(strLine.Substring(8, 11)) 'Part
                                        multiListL(iPartX).Add(strLine.Substring(pos4 - 7, 6)) 'Color Code
                                    ElseIf (strLine.Contains("BP*81300")) Then
                                        'get data from the line
                                        pos4 = GetNthIndex(strLine, "*", 4) + 1
                                        multiListMidR.Add(New List(Of String))
                                        multiListMidR(iPartX).Add(strLine.Substring(8, 11)) 'Part
                                        multiListMidR(iPartX).Add(strLine.Substring(pos4 - 7, 6)) 'Color Code
                                    ElseIf (strLine.Contains("BP*81700")) Then
                                        'get data from the line
                                        pos4 = GetNthIndex(strLine, "*", 4) + 1
                                        multiListMidL.Add(New List(Of String))
                                        multiListMidL(iPartX).Add(strLine.Substring(8, 11)) 'Part
                                        multiListMidL(iPartX).Add(strLine.Substring(pos4 - 7, 6)) 'Color Code
                                    ElseIf (strLine.Contains("BP*82100")) Then
                                        'get data from the line
                                        pos4 = GetNthIndex(strLine, "*", 4) + 1
                                        multiListRR.Add(New List(Of String))
                                        multiListRR(iPartX).Add(strLine.Substring(8, 11)) 'Part
                                        multiListRR(iPartX).Add(strLine.Substring(pos4 - 7, 6)) 'Color Code
                                    End If

                                    strLine = Trim(sReader.ReadLine())

                                ElseIf strLine.Contains("ST*830") Or strLine.Contains("IEA*") Or sReader.Peek = -1 Then
                                    Exit Do
                                End If
                            Loop

                        Loop

                        '*Get the details*
                        'We now know the parts contained in the file so go back to beginning of file and process.
                        sReader.DiscardBufferedData()
                        sReader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin)

                        strLine = Trim(sReader.ReadLine)  'read in very first line which should be header record
                        strLine = strLine.Replace(Chr(9), "")
                        Do While strLine = ""
                            strLine = Trim(sReader.ReadLine)
                        Loop

                        strDetailRecord = ""

                        'Set expectations noting element 0 Is always "".
                        If multiListMidR.Count = 1 Then
                            MidRightsProcessed = True
                        End If
                        If multiListMidL.Count = 1 Then
                            MidLeftsProcessed = True
                        End If
                        If multiListRR.Count = 1 Then
                            RearsProcessed = True
                        End If


                        Do Until strLine.Contains("ST*830*" & ReferenceNumberID) Or sReader.Peek = -1
                            strLine = Trim(sReader.ReadLine())
                        Loop

                        While Not FrontRightsProcessed Or Not FrontLeftsProcessed Or Not MidRightsProcessed Or Not MidLeftsProcessed Or Not RearsProcessed

                            If multiListR(0).Count = 0 Or multiListL(0).Count = 0 Then 'No fronts then don't process
                                Exit While
                            End If

                            CurrentPart = ""

                            If Not FrontRightsProcessed Then

                                'get right(s)
                                Do Until strLine.Contains("LIN*") And strLine.Contains(multiListR(0).Item(rt))
                                    strLine = Trim(sReader.ReadLine())
                                Loop

                                CurrentPart = multiListR(0).Item(rt)
                                CurrentPartColor = multiListR(0).Item(rt + 1)

                                If multiListMidR(0).Count = 1 Then 'No mids
                                    SeatMidRight = ""
                                Else
                                    SeatMidRight = multiListMidR(0).Item(rt)
                                End If
                                If multiListMidL(0).Count = 1 Then 'No Mids
                                    SeatMidLeft = ""
                                Else
                                    SeatMidLeft = multiListMidL(0).Item(rt)
                                End If
                                If multiListRR(0).Count = 1 Then 'No Rears
                                    SeatRear = ""
                                ElseIf multiListRR(0).Count = 3 Then
                                    SeatRear = multiListRR(0).Item(1)
                                ElseIf multiListRR(0).Count = 5 Then
                                    SeatRear = multiListRR(0).Item(4)
                                End If

                                'get the right framecode
                                strFrameCodeFromFileCheck = ReturnFrameCode(multiListR(0).Item(rt), multiListL(0).Item(rt), SeatMidRight, SeatMidLeft, SeatRear, multiListR(0).Item(2).Trim)

                                If rt = multiListR(0).Count - 2 Then
                                    FrontRightsProcessed = True
                                End If

                                rt += 2 'move on to the next part whch is the second element in the list

                                DoingRightParts = True
                                DoingLeftParts = False
                                DoingRightMidParts = False
                                DoingLeftMidParts = False
                                DoingRearParts = False

                            ElseIf Not FrontLeftsProcessed Then
                                'reset
                                sReader.DiscardBufferedData()
                                sReader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin)
                                Do Until strLine.Contains("ST*830*" & ReferenceNumberID) Or sReader.Peek = -1
                                    strLine = Trim(sReader.ReadLine())
                                Loop

                                'get left(s)
                                Do Until strLine.Contains("LIN*") And strLine.Contains(multiListL(0).Item(lt))
                                    strLine = Trim(sReader.ReadLine())
                                Loop

                                CurrentPart = multiListL(0).Item(lt)
                                CurrentPartColor = multiListL(0).Item(lt + 1)

                                If multiListMidR(0).Count = 1 Then
                                    SeatMidRight = ""
                                Else
                                    SeatMidRight = multiListMidR(0).Item(lt)
                                End If
                                If multiListMidL(0).Count = 1 Then
                                    SeatMidLeft = ""
                                Else
                                    SeatMidLeft = multiListMidL(0).Item(lt)
                                End If
                                If multiListRR(0).Count = 1 Then 'No Rears
                                    SeatRear = ""
                                ElseIf multiListRR(0).Count = 3 Then
                                    SeatRear = multiListRR(0).Item(1)
                                ElseIf multiListRR(0).Count = 5 Then
                                    SeatRear = multiListRR(0).Item(4)
                                End If

                                'get the left framecode
                                strFrameCodeFromFileCheck = ReturnFrameCode(multiListR(0).Item(lt), multiListL(0).Item(lt), SeatMidRight, SeatMidLeft, SeatRear, multiListR(0).Item(2).Trim)

                                If lt = multiListL(0).Count - 2 Then
                                    FrontLeftsProcessed = True
                                End If

                                lt += 2 'move on to the next part

                                DoingRightParts = False
                                DoingLeftParts = True
                                DoingRightMidParts = False
                                DoingLeftMidParts = False
                                DoingRearParts = False

                            ElseIf Not MidRightsProcessed Then
                                'reset
                                sReader.DiscardBufferedData()
                                sReader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin)
                                Do Until strLine.Contains("ST*830*" & ReferenceNumberID) Or sReader.Peek = -1
                                    strLine = Trim(sReader.ReadLine())
                                Loop

                                'get Mid right(s)
                                Do Until strLine.Contains("LIN*") And strLine.Contains(multiListMidR(0).Item(mrt)) Or sReader.Peek = -1
                                    strLine = Trim(sReader.ReadLine())
                                Loop

                                CurrentPart = multiListMidR(0).Item(mrt)
                                CurrentPartColor = multiListMidR(0).Item(mrt + 1)

                                If multiListMidR(0).Count > multiListR(0).Count Then
                                    SeatFrontRight = multiListR(0).Item(1)
                                Else
                                    SeatFrontRight = multiListR(0).Item(mrt)
                                End If
                                If multiListMidL(0).Count > multiListL(0).Count Then
                                    SeatFrontLeft = multiListL(0).Item(1)
                                Else
                                    SeatFrontLeft = multiListL(0).Item(mlt)
                                End If
                                If multiListRR(0).Count = 1 Then 'No Rears
                                    SeatRear = ""
                                ElseIf multiListRR(0).Count = 3 Then
                                    SeatRear = multiListRR(0).Item(1)
                                ElseIf multiListRR(0).Count = 5 Then
                                    SeatRear = multiListRR(0).Item(4)
                                End If

                                'get the mid right framecode
                                strFrameCodeFromFileCheck = ReturnFrameCode(SeatFrontRight, SeatFrontLeft, SeatMidRight, SeatMidLeft, SeatRear, multiListR(0).Item(2).Trim)

                                If mrt = multiListMidR(0).Count - 2 Then
                                    MidRightsProcessed = True
                                End If

                                mrt += 2

                                DoingRightParts = False
                                DoingLeftParts = False
                                DoingRightMidParts = True
                                DoingLeftMidParts = False
                                DoingRearParts = False

                            ElseIf Not MidLeftsProcessed Then
                                'reset
                                sReader.DiscardBufferedData()
                                sReader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin)
                                Do Until strLine.Contains("ST*830*" & ReferenceNumberID) Or sReader.Peek = -1
                                    strLine = Trim(sReader.ReadLine())
                                Loop

                                'get mid left(s)
                                Do Until strLine.Contains("LIN*") And strLine.Contains(multiListMidL(0).Item(mlt)) Or sReader.Peek = -1
                                    strLine = Trim(sReader.ReadLine())
                                Loop

                                CurrentPart = multiListMidL(0).Item(mlt)
                                CurrentPartColor = multiListMidL(0).Item(mlt + 1)

                                If multiListMidR(0).Count > multiListR(0).Count Then  'when there are multiple mids to one front.
                                    SeatFrontRight = multiListR(0).Item(1)
                                Else
                                    SeatFrontRight = multiListR(0).Item(mrt)
                                End If
                                If multiListMidL(0).Count > multiListL(0).Count Then
                                    SeatFrontLeft = multiListL(0).Item(1)
                                Else
                                    SeatFrontLeft = multiListL(0).Item(mlt)
                                End If
                                If multiListRR(0).Count = 1 Then 'No Rears
                                    SeatRear = ""
                                ElseIf multiListRR(0).Count = 3 Then
                                    SeatRear = multiListRR(0).Item(1)
                                ElseIf multiListRR(0).Count = 5 Then
                                    SeatRear = multiListRR(0).Item(4)
                                End If

                                'get the mid left framecode
                                strFrameCodeFromFileCheck = ReturnFrameCode(SeatFrontRight, SeatFrontLeft, SeatMidRight, SeatMidLeft, SeatRear, multiListR(0).Item(2).Trim)

                                If mlt = multiListMidL(0).Count - 2 Then
                                    MidLeftsProcessed = True
                                End If

                                mlt += 2

                                DoingRightParts = False
                                DoingLeftParts = False
                                DoingRightMidParts = False
                                DoingLeftMidParts = True
                                DoingRearParts = False

                            ElseIf Not RearsProcessed Then
                                'reset
                                sReader.DiscardBufferedData()
                                sReader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin)
                                Do Until strLine.Contains("ST*830*" & ReferenceNumberID) Or sReader.Peek = -1
                                    strLine = Trim(sReader.ReadLine())
                                Loop

                                'get rear(s)
                                If multiListRR.Count <> 1 Then
                                    Do Until strLine.Contains("LIN*") And strLine.Contains(multiListRR(0).Item(rr)) Or sReader.Peek = -1
                                        strLine = Trim(sReader.ReadLine())
                                    Loop

                                    CurrentPart = multiListRR(0).Item(rr)
                                    CurrentPartColor = multiListRR(0).Item(rr + 1)

                                    If multiListMidR(0).Count = 1 Then
                                        SeatMidRight = ""
                                    Else
                                        SeatMidRight = multiListMidR(0).Item(rr)
                                    End If
                                    If multiListMidL(0).Count = 1 Then
                                        SeatMidLeft = ""
                                    Else
                                        SeatMidLeft = multiListMidL(0).Item(rr)
                                    End If
                                    If multiListRR(0).Count = 1 Then
                                        SeatRear = ""
                                    Else
                                        SeatRear = multiListRR(0).Item(rr)
                                    End If

                                    'get the rear framecode
                                    strFrameCodeFromFileCheck = ReturnFrameCode(multiListR(0).Item(rr), multiListL(0).Item(rr), SeatMidRight, SeatMidLeft, SeatRear, multiListR(0).Item(2).Trim)

                                    If rr = multiListRR(0).Count - 2 Then
                                        RearsProcessed = True
                                    End If

                                    rr += 2

                                    DoingRightParts = False
                                    DoingLeftParts = False
                                    DoingRightMidParts = False
                                    DoingLeftMidParts = False
                                    DoingRearParts = True
                                End If

                            End If


                            'Lets find some associated lots 
                            Do Until strLine.Contains("FST*") Or sReader.Peek = -1
                                strLine = Trim(sReader.ReadLine())
                            Loop

                            'Get Line Name 20190808
                            strLineName = BizLayer.GetBroadcastParamValue("0035", strProdLine)

                            'Lets process some lots
                            Do While strLine.Contains("FST*") And sReader.Peek <> -1

                                weekcount += 1
                                WeekCounter += 1

                                If DoingRightParts Then
                                    TotalWeekRight += WeekCounter
                                    WeekCounter = 0
                                ElseIf DoingLeftParts Then
                                    TotalWeekLeft += WeekCounter
                                    WeekCounter = 0
                                ElseIf DoingRightMidParts Then
                                    TotalWeekMidRight += WeekCounter
                                    WeekCounter = 0
                                ElseIf DoingLeftMidParts Then
                                    TotalWeekMidLeft += WeekCounter
                                    WeekCounter = 0
                                ElseIf DoingRearParts Then
                                    TotalWeekRear += WeekCounter
                                    WeekCounter = 0
                                End If

                                'find the position of the date after 4th *
                                pos4 = GetNthIndex(strLine, "*", 4) + 1

                                'Get the Julian week number
                                intJuianWeek = GetWeekOfYear(CDate(strLine.Substring(GetNthIndex(strLine, "*", 4) + 1, 8).Insert(4, "-").Insert(7, "-")))
                                intJuianWeek = intJuianWeek + (100 * iPartCount)

                                'find the month and year of the lot to be created, count up the dates
                                key = strLine.Substring(GetNthIndex(strLine, "*", 4) + 3, 4)

                                'check that key does not already exist and also that it is not a no-build
                                If Not dateCounts.ContainsKey(key) Then
                                    dateCounts.Add(key, 1)
                                Else
                                    keyCount = CInt(dateCounts.Item(key))
                                    keyCount = keyCount + 1
                                    dateCounts.Item(key) = keyCount
                                End If

                                'Frame and color code dervived above.. Get lot numbers from string
                                strLotNumber = strProdLine & "0000" & strLine.Substring(pos4, 9)  'will be displayed on right side of
                                strLotNumber2 = strProdLine & Trim(strLine.Substring(pos4 + 2, 2)) & intJuianWeek.ToString.PadLeft(5, System.Convert.ToChar("0")) & "1" 'will be displayed on left side of

                                'Get the qty
                                pos1 = GetNthIndex(strLine, "*", 1) + 1 'first * position
                                pos2 = GetNthIndex(strLine, "*", 2) + 1 'second * position
                                qty = CInt(Trim(strLine).Substring(pos1, pos2 - pos1 - 1))

                                If DoingRightParts Then
                                    TotalQtyRight += qty
                                ElseIf DoingLeftParts Then
                                    TotalQtyLeft += qty
                                ElseIf DoingRightMidParts Then
                                    TotalQtyRightMid += qty
                                ElseIf DoingLeftMidParts Then
                                    TotalQtyLeftMid += qty
                                ElseIf DoingRearParts Then
                                    TotalQtyRear += qty
                                End If

                                'Build the line entry IF the frames were defined
                                If strFrameCodeFromFileCheck <> "99" And strFrameCodeFromFileCheck.Length > 0 Then
                                    strLineMod = "  " & strLotNumber2 & strFrameCodeFromFileCheck & "0" & " " & CurrentPartColor.Trim.PadRight(10, System.Convert.ToChar(" ")) & strFrameCodeFromFileCheck(strFrameCodeFromFileCheck.Length - 1) & " xxxx" & " " & qty.ToString.PadLeft(5, System.Convert.ToChar("0")) & "   " & strLineName & " 0" & strLotNumber & "   " & CurrentPart

                                    'Add to hash table to allow for asc sorting based on the intJuianWeek
                                    ht.Add(iSortCount + intJuianWeek, strLineMod)
                                    iSortCount = iSortCount + 100
                                End If

                                'goto next line
                                strLine = Trim(sReader.ReadLine())

                                If strLine.Contains("ISA*") Or sReader.Peek = -1 Then
                                    Exit Do 'means found end header record or next section so done with detail records
                                End If
                            Loop
                            iPartCount += 1
                        End While

                        'Sort the data from the Hashtable
                        Dim keys = ht.Keys.Cast(Of Integer)().ToArray()
                        Dim values = ht.Values.Cast(Of String)().ToArray()
                        Array.Sort(keys, values)

                        'Confirm Qty
                        If multiListRR.Count = 1 Then 'No rears to confirm qty - Thailand seats
                            If (TotalQtyRight = TotalQtyLeft) And (TotalWeekRight = TotalWeekLeft) Then
                                For iRow As Integer = 0 To ht.Count - 1
                                    strKDEntireFile = strKDEntireFile & values(iRow) & vbCrLf
                                Next
                            Else
                                If (TotalQtyRight <> TotalQtyLeft) Then
                                    tbIssues.Text = "Total Quanties of Parts in File Not Equal. Total Righthand: " + CStr(TotalQtyRight) + ",  Total Lefthand: " + CStr(TotalQtyLeft)
                                    tbFileContents.Text = ""
                                ElseIf TotalWeekRight <> TotalWeekLeft Then
                                    tbIssues.Text = "Total Weeks of Parts in File Not Equal. Total Righthand: " + CStr(TotalWeekRight) + ",  Total Lefthand: " + CStr(TotalWeekLeft)
                                    tbFileContents.Text = ""
                                End If
                            End If
                        Else
                            If (TotalQtyRight = TotalQtyLeft And TotalQtyRightMid = TotalQtyLeftMid) And (TotalWeekRight = TotalWeekLeft) And (TotalWeekMidRight = TotalWeekMidLeft) Then
                                'If (TotalQtyRight = TotalQtyLeft And TotalQtyRightMid = TotalQtyLeftMid And TotalQtyRight = TotalQtyRear) And (TotalWeekRight = TotalWeekLeft) And (TotalWeekMidRight = TotalWeekMidLeft) Then
                                'If (TotalQtyRight = TotalQtyLeft And TotalQtyRight = TotalQtyRear) And (TotalWeekRight = TotalWeekLeft) Then
                                For iRow As Integer = 0 To ht.Count - 1
                                    strKDEntireFile = strKDEntireFile & values(iRow) & vbCrLf
                                Next
                            Else
                                If (TotalQtyRight <> TotalQtyLeft Or TotalQtyRight <> TotalQtyRear) Then
                                    tbIssues.Text = "Total Quanties of Parts in File Not Equal. Total Righthand: " + CStr(TotalQtyRight) + ",  Total Lefthand: " + CStr(TotalQtyLeft)
                                    tbFileContents.Text = ""
                                ElseIf TotalWeekRight <> TotalWeekLeft Then
                                    tbIssues.Text = "Total Weeks of Parts in File Not Equal. Total Righthand: " + CStr(TotalWeekRight) + ",  Total Lefthand: " + CStr(TotalWeekLeft)
                                    tbFileContents.Text = ""
                                End If
                            End If
                        End If

                        ht.Clear()

                    Else

                        'Start original txt KD file version
                        ' get KD plan date from first record in file and format it
                        mnth = strLine.Substring(51, 6).Substring(2, 2)    'parse out month
                        day = strLine.Substring(51, 6).Substring(4, 2) 'parse out day
                        yr = strLine.Substring(51, 6).Substring(0, 2) 'parse out year

                        lblSendDateTitle.Text = "KD Plan Send Date:"
                        lblNFiles.Text = "KD Plans Found in File:"

                        strModstrJobs = mnth & "/" & "01" & "/" & yr
                        lblSendDate.Text = mnth & "/" & day & "/" & yr 'set the file send date
                        strDetailRecord = ""
                        If strLine = "" Then        'check for invalid records, extra spaces in file, or end of file
                            strLine = Trim(sReader.ReadLine)
                        Else
                            'find the month and year of the lot to be created
                            strDetailRecord = Trim(strLine).Substring(0, 7)
                        End If
                        Do While (sReader.Peek <> -1)  '1 record has been read at this point
                            'keep processing rows until the trailer record is reached
                            Do Until strDetailRecord = "**TRAIL"
                                If strDetailRecord = "**HEADE" Then
                                    Exit Do 'means found 2nd header record, done with detail records
                                Else 'start processing detail records
                                    '////////detail records
                                    strFrameCodeFromFile = strLine.Substring(11, 7) & strLine.Substring(31, 1)
                                    'check for production line and exit do if not configured line
                                    strProdLine = strLine.Substring(2, 1)

                                    If lstNoBuildIdentifierArray.Contains(UCase(strFrameCodeFromFile.Substring(noBuildStartPosition - 1, noBuildStringLength))) Then
                                        'skip the line because no build frame code foudn
                                        '#JMH(framecount isn't used anywhere)       intframecount = intframecount + 1

                                    Else
                                        'count up the dates
                                        key = strLine.Substring(3, 4)
                                        'check that key does not already exist and also that it is not a no-build
                                        If Not dateCounts.ContainsKey(key) Then
                                            dateCounts.Add(key, 1)
                                        Else
                                            keyCount = CInt(dateCounts.Item(key))
                                            keyCount = keyCount + 1
                                            dateCounts.Item(key) = keyCount
                                        End If

                                    End If

                                    '/////////detail records
                                End If
                                'end processing detail records

                                strKDEntireFile = strKDEntireFile & strLine & vbCrLf
                                'after read in header record
                                strLine = Trim(sReader.ReadLine) ' ******READ'******
                                strLine = strLine.Replace(Chr(9), "")
                                If strLine = "" Then        'check for invalid records, extra spaces in file, or end of file
                                    strLine = Trim(sReader.ReadLine)
                                Else
                                    'find the month and year of the lot to be created
                                    strDetailRecord = Trim(strLine).Substring(0, 7)
                                End If

                            Loop

                            'read records after the header record  ' *******'READ'*******
                            strLine = Trim(sReader.ReadLine)
                            strLine = strLine.Replace(Chr(9), "")
                            If strLine.Length < lineLength - 20 Then
                                sReader.ReadLine()

                            End If
                            If strLine = "" Then        'check for invalid records, extra spaces in file, or end of file
                                strLine = Trim(sReader.ReadLine)
                            Else
                                'find the month and year of the lot to be created
                                strDetailRecord = Trim(strLine).Substring(0, 7)
                            End If

                            'determine max of what was passed in and set this to be returned
                            If Not dateCounts.Count = 0 Then 'takes care of first pass
                                For Each de In dateCounts
                                    If IsNothing(maxDate) OrElse CInt(de.Value) > CInt(maxDate.Value) Then
                                        maxDate = de
                                    End If
                                Next

                                dt = DateTime.ParseExact(maxDate.Key.ToString(), "yyMM", Nothing)
                                mnth = dt.Month.ToString()
                                yr = dt.Year.ToString()
                                strModKDPlan = mnth & "/" & "01" & "/" & yr

                                'determine if N-1, N-2 or N-3, or N-0, or N-0* with data from two months
                                Select Case CInt((DateDiff(DateInterval.Month, CDate(strModstrJobs), CDate(strModKDPlan))))
                                    Case 0 'found N-0 data
                                        If dateCounts.Keys.Count > 1 Then 'found two dates in the file, so set asterick data
                                            lblAsterick.Text = "* File Contains Data Outside of Month Specified"
                                            marker = "*"
                                        Else
                                            lblAsterick.Text = "* Flagged as N-0 File, Double Check"
                                            marker = "*"
                                        End If
                                        strNValueFoundinFile = ("N-1 " & Format(CDate(strModKDPlan), "MMMM yyyy") & " - Line " & strProdLine & marker)

                                    Case 1 'found N-1 data 
                                        If dateCounts.Keys.Count > 1 Then 'found two dates in the file, so set asterick data
                                            lblAsterick.Text = "* File Contains Data Outside of Month Specified"
                                            marker = "*"
                                        Else : marker = ""
                                        End If
                                        strNValueFoundinFile = ("N-1 " & Format(CDate(DateAdd(DateInterval.Month, 1, CDate(strModstrJobs))), "MMMM yyyy") & " - Line " & strProdLine & marker)

                                    Case 2 'found N-2 data
                                        strNValueFoundinFile = ("N-2 " & Format(CDate(DateAdd(DateInterval.Month, 2, CDate(strModstrJobs))), "MMMM yyyy") & " - Line " & strProdLine)

                                    Case 3 'found N-3 data
                                        strNValueFoundinFile = ("N-3 " & Format(CDate(DateAdd(DateInterval.Month, 3, CDate(strModstrJobs))), "MMMM yyyy") & " - Line " & strProdLine)
                                    Case Else

                                End Select

                                dateCounts.Clear()
                                keyCount = 0
                                maxDate.Value = Nothing
                            End If

                            'compare NValue found in file to one passed in
                            If strNValueFoundinFile = strNvalue Then
                                'write out file to textbox
                                Exit Try
                            Else
                                'otherwise, clear out previous lines
                                strKDEntireFile = ""
                            End If
                        Loop
                    End If
                End Using      'streampath

            ElseIf (ext.Equals("xls") Or ext.Equals("lsx")) Then

                Using stream As FileStream = File.Open(path, FileMode.Open, FileAccess.Read)

                    If (ext.Equals("xls")) Then
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream) 'Reading from a binary Excel file ('97-2003 format; *.xls)
                    Else
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream) 'Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    End If

                    Using (excelReader)
                        excelReader.IsFirstRowAsColumnNames = True  'DataSet - Create column names from first row

                        dsResult = excelReader.AsDataSet() 'DataSet - The result of each spreadsheet will be created in the result.Tables
                        dsResult.Tables(0).Rows.RemoveAt(0) ' this will remove 2nd row;  IsFirstRowAsColumnNames = True removes 1st row

                        lbNFileDates.Items.Clear()
                        tbIssues.Text = ""
                        strMsgArray.Clear(strMsgArray, 0, strMsgArray.Length)

                        For Each Str As String In ReturnModelExceptionStringIdentifiers().Split(","c)
                            lstModelExceptionIdentifierArray.Add(Str)
                        Next


                        'find first batch of 60; use that as the K-1 plan date
                        For Each row As DataRow In dsResult.Tables(0).Rows  '.Select("[Batch No] IS NOT NULL")
                            strItem = ""

                            If ((Not row("Prod Dt").ToString.ToUpper().Contains("DAILY TOTAL:")) AndAlso (row("KD Lot No").ToString().Trim().Length <> 0)) Then
                                If Integer.TryParse(row("Batch No").ToString(), batchno) Then
                                    batchno = CInt(row("Batch No").ToString())
                                Else
                                    batchno = 0
                                End If

                                strProdDate = row("Prod Seq").ToString().Substring(0, 6)
                                strKDDate = row("KD Lot No").ToString().Substring(6, 6)

                                '''''If strProdDate = strKDDate Then   'filter out mismatched months

                                If batchno = 0 And KDMonth = 12 Then
                                    strProdDate = row("Prod Seq").ToString().Substring(0, 6)
                                End If

                                If (lstbatchno <> batchno) Then
                                    If ((batchQty < QTY_PER_BATCH) And (strKDEntireFile.Length > 0)) Or ((lstModelExceptionIdentifierArray.Contains(row("MTOC").ToString().Trim())) And (strKDEntireFile.Length > 0)) Then
                                        msg += lstbatchno.ToString() + ", "
                                        'Throw New ImportFileFormatException(msg)
                                        strMsgArray(KDMonth) = strMsgArray(KDMonth) & msg
                                        msg = ""
                                    End If

                                    lstbatchno = batchno
                                    strLine = ""
                                    qty = 0
                                    batchQty = 0
                                Else 'reset when processing without batch numbers now
                                    If ((batchQty = QTY_PER_BATCH) And (strKDEntireFile.Length > 0)) Or ((lstModelExceptionIdentifierArray.Contains(row("MTOC").ToString().Trim())) And (strKDEntireFile.Length > 0)) Then
                                        strLine = ""
                                        qty = 0
                                        batchQty = 0
                                    End If
                                End If


                                Dim liTest As New ListItem
                                liTest.Value = strKDDate.Substring(0, 4) + " / " + strKDDate.Substring(4, 2)
                                If lbNFileDates.Items.Contains(liTest) = False Then
                                    lbNFileDates.Items.Add(liTest)
                                End If

                                Dim li As ListItem
                                Dim sl As SortedList = New SortedList

                                'Loop through each Item in the List And move them over to the SortedList
                                For Each li In lbNFileDates.Items
                                    sl.Add(Trim(li.Text), Trim(li.Value))
                                Next

                                ' Move sorted items back to List again
                                lbNFileDates.DataSource = sl
                                lbNFileDates.DataValueField = "Value"
                                lbNFileDates.DataTextField = "Key"
                                lbNFileDates.DataBind()

                                KDMonth = CInt(strKDDate.Substring(4, 2))

                                strItem = row("KD Lot No").ToString()
                                strProdLine = strItem.Substring(5, 1)
                                strLine += String.Format("  {0}{1}{2}{3}{4}", strProdLine, strItem.Substring(8, 2), strItem.Substring(10, 2), strItem.Substring(13, 3), strItem.Substring(17, 1))

                                strItem = row("MTOC").ToString().Trim()
                                If strItem.Length = 4 Then
                                    strItem = strItem + "    "
                                    strLine += String.Format("{0}", strItem.Substring(0, 4))

                                    ord = row.Table.Columns("MTOC").Ordinal
                                    If (ord + 1 < row.Table.Columns.Count) Then
                                        strItem = row(ord + 1).ToString() + "          "
                                        strLine += String.Format("{0}", strItem.Substring(0, 6))
                                    Else
                                        Throw New ImportFileFormatException("2nd MTOC column is missing")
                                    End If

                                    If (ord + 2 < row.Table.Columns.Count) Then
                                        strItem = row(ord + 2).ToString() + "          "
                                        strLine += String.Format("{0}", strItem.Substring(0, 10))
                                    Else
                                        Throw New ImportFileFormatException("Frame Code column is missing")
                                    End If

                                    If (ord + 3 < row.Table.Columns.Count) Then
                                        strItem = row(ord + 3).ToString() + "  "
                                        strLine += String.Format("{0}", strItem.Substring(0, 2))
                                    Else
                                        Throw New ImportFileFormatException("Interior Color column is missing")
                                    End If
                                ElseIf strItem.Length = 21 Then
                                    strItem = strItem + "  "
                                    strLine += String.Format("{0}", strItem.Substring(0, 22))
                                Else
                                    Throw New ImportFileFormatException("MTOC column format is not correct")
                                End If

                                strLine += "xxxx "

                                qty = CInt(row("Qty").ToString())
                                batchQty += qty
                                strItem = "00000" + qty.ToString()
                                strLine += String.Format("{0}   {1}", strItem.Substring(strItem.Length - 5), row("KD Lot No").ToString())
                                strLine += vbCrLf

                                If (batchQty = QTY_PER_BATCH) Or (batchQty = 1 And lstModelExceptionIdentifierArray.Contains(row("MTOC").ToString().Trim())) Then

                                    strKDEntireFile += strLine

                                    Select Case KDMonth
                                        Case 1
                                            strData01 = strData01 & strLine
                                        Case 2
                                            strData02 = strData02 & strLine
                                        Case 3
                                            strData03 = strData03 & strLine
                                        Case 4
                                            strData04 = strData04 & strLine
                                        Case 5
                                            strData05 = strData05 & strLine
                                        Case 6
                                            strData06 = strData06 & strLine
                                        Case 7
                                            strData07 = strData07 & strLine
                                        Case 8
                                            strData08 = strData08 & strLine
                                        Case 9
                                            strData09 = strData09 & strLine
                                        Case 10
                                            strData10 = strData10 & strLine
                                        Case 11
                                            strData11 = strData11 & strLine
                                        Case 12
                                            strData12 = strData12 & strLine

                                    End Select
                                    strLine = ""
                                    qty = 0
                                ElseIf (qty > QTY_PER_BATCH) Then
                                    msg = "File may be corrupt.  Batch size is too large.  Refer to: " + batchno.ToString()
                                    Throw New ImportFileFormatException(msg)
                                End If

                            End If

                        Next
                        excelReader.Close() 'Free resources (IExcelDataReader is IDisposable)
                    End Using
                End Using



                'Stuff data into array
                strKDEntireFileMonthArray(1) = strData01
                strKDEntireFileMonthArray(2) = strData02
                strKDEntireFileMonthArray(3) = strData03
                strKDEntireFileMonthArray(4) = strData04
                strKDEntireFileMonthArray(5) = strData05
                strKDEntireFileMonthArray(6) = strData06
                strKDEntireFileMonthArray(7) = strData07
                strKDEntireFileMonthArray(8) = strData08
                strKDEntireFileMonthArray(9) = strData09
                strKDEntireFileMonthArray(10) = strData10
                strKDEntireFileMonthArray(11) = strData11
                strKDEntireFileMonthArray(12) = strData12

                If (Right(msg, 2).Equals(", ")) Then
                    Master.tMsg("Read File", Left(msg, msg.Length - 2))
                End If

            End If

            If (lblAsterick.Text.Length > 0) Then
                lblAsterick.Visible = True
            Else
                lblAsterick.Visible = False
            End If

        Catch ex As ImportFileFormatException
            Master.tMsg("Read File", ex.Message)
            lbNfiles.SelectedIndex = -1
            lbNfiles.Items.Clear()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        Finally
            If strKDEntireFile.Length <> 0 Then
                tbFileContents.Text = ""
                If Right(lblFileName.Text, 3) = "txt" Then
                    tbFileContents.Text = strKDEntireFile
                End If
            End If
        End Try
    End Sub

    Public Overridable Function GetWeekOfYear(dt As DateTime) As Integer
        ' Gets the Calendar instance associated with a CultureInfo.
        Dim myCI As New CultureInfo("en-US")
        Dim myCal As Calendar = myCI.Calendar

        ' Gets the DTFI properties required by GetWeekOfYear.
        Dim myCWR As CalendarWeekRule = myCI.DateTimeFormat.CalendarWeekRule
        Dim myFirstDOW As DayOfWeek = myCI.DateTimeFormat.FirstDayOfWeek

        GetWeekOfYear = myCal.GetWeekOfYear(dt, myCWR, myFirstDOW)

    End Function
    Private Function GetListOfFilesWithoutPath(ByVal strDir As String) As String()
        Dim sfiles() As String

        Try
            'save to array list
            sfiles = Directory.GetFiles(strDir)

            'strip out the path information before the filename
            For x = 0 To sfiles.GetLength(0) - 1
                sfiles(x) = Path.GetFileName(sfiles(x))
            Next

            Return sfiles
        Catch ex As Exception
            Master.eMsg(ex.ToString())

            'return empty string array
            Return New String() {}
        End Try
    End Function

    Function ReturnProcessed(ByVal strNValue As String) As Integer
        Try
            'remove asterick if present
            strNValue = strNValue.Replace("*", "")
            Return CInt(DA.GetDataSet("SELECT count(*) from tblorderdata where OrderParameterID='0154' and OrderParameterValue='" & strNValue & "'").Tables(0).Rows(0).Item(0))

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return 0
        End Try
    End Function

    Private Sub ResetControls()
        'clear out all file information
        lblSendDateTitle.Text = "Plan Send Date:"
        lblNFiles.Text = "Plans Found in File:"
        lbNfiles.Items.Clear()
        tbFileContents.Text = ""
        tbIssues.Text = ""
        lblFileName.Text = ""
        lblStatus.Text = ""
        lblSendDate.Text = ""
        lblAsterick.Text = ""
        lblAsterick.Visible = False
        lbNFileDates.Items.Clear()
    End Sub

    Private Function ImportFile(ByVal strNValue As String) As String

        Dim dtStart As Date = Now
        Dim elapsed_time As TimeSpan
        Dim fileType As String

        'remove asterick if present
        strNValue = strNValue.Replace("*", "")

        fileType = Right(lblFileName.Text, 3)

        Dim parameters As New List(Of SqlParameter)
        parameters.Add(New SqlParameter("@NValue", strNValue))
        parameters.Add(New SqlParameter("@FileType", fileType))

        'set SQL parameters for status output: used to report back  if DTS package successed
        Dim oSqlParameter As New SqlParameter("@Status", SqlDbType.VarChar, 5)
        oSqlParameter.Direction = ParameterDirection.Output
        parameters.Add(oSqlParameter)

        Dim colOutput As List(Of SqlParameter) = DA.ExecSP("[dbo].[procPSOPMLiteImportFile]", parameters)
        Dim status As String = ""
        For Each par As SqlParameter In colOutput
            With par
                If .Direction = ParameterDirection.Output And .ParameterName = "@Status" Then
                    status = par.Value.ToString()
                End If
            End With
        Next

        Dim dtEnd As Date = Now

#If DEBUG Then
        pnlProcessTime.Visible = True
        elapsed_time = dtEnd.Subtract(dtStart)
        lblProcTime.Text = elapsed_time.TotalSeconds.ToString("0.000")
#End If

        Return status
    End Function

    Private Sub EnableControls()
        Try
            Master.Secure(Me.cmdDelete)
            Master.Secure(Me.cmdMove)
            Master.Secure(Me.cmdProcess)

            Dim node As TreeNode = TreeView.SelectedNode
            Dim strDirIn As String = ReturnInbound()
            Dim strDirArc As String = ReturnOutbound()
            If (node IsNot Nothing) AndAlso (node.Text <> strDirIn) AndAlso (node.Text <> strDirArc) Then
                'only enable the controls if the selected node is a file.
                tbFileContents.Enabled = True
                lbNfiles.Enabled = True
                lbNFileDates.Enabled = True
                cmdDelete.Enabled = True
                cmdMove.Enabled = True
                cmdProcess.Enabled = True

            Else
                'disable all controls
                cmdDelete.Enabled = False
                cmdMove.Enabled = False
                tbFileContents.Enabled = False
                lbNfiles.Enabled = False
                lbNFileDates.Enabled = False
                cmdProcess.Enabled = False
            End If

            If lbNFileDates.SelectedIndex < 0 And UCase(Right(lblFileName.Text, 3)) = "TXT" Then
                'Only process when a specific plan is selected
                If tbIssues.Text = "" Then
                    cmdProcess.Enabled = True
                Else
                    cmdProcess.Enabled = False
                End If
            ElseIf lbNFileDates.SelectedIndex > -1 Then
                cmdProcess.Enabled = True
            Else
                cmdProcess.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub lbNFileDates_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbNFileDates.SelectedIndexChanged

        Dim curItem As String = lbNFileDates.SelectedItem.ToString()
        'SelectedMonth = CInt(curItem.Substring(0, 2))
        SelectedMonth = CInt(curItem.Substring(7, 2))
        '
        If strMsgArray(SelectedMonth) = "" Then
            lblIssues.Text = "Batch Issues:"
        Else
            lblIssues.Text = "File may be corrupt...  The following batch sizes are too small:"
        End If

        tbFileContents.Text = ""
        tbIssues.Text = ""

        tbFileContents.Text = strKDEntireFileMonthArray(SelectedMonth)
        tbIssues.Text = strMsgArray(SelectedMonth)

        If lbNFileDates.SelectedIndex < 0 Then
            'Only process when a specific plan is selected
            cmdProcess.Enabled = False
        Else
            cmdProcess.Enabled = True
        End If
        If ReturnProcessed(NewKDPlanNValueMonth(lbNfiles.SelectedValue)) > 0 Then
            lblStatus.Text = "Processed"
        Else
            lblStatus.Text = "Not Processed"
        End If

    End Sub
#End Region

#Region "DTS File Validation "
    Private Function CreateDtsFile(ByVal path As String, ByRef strArrayErrorFrame As ArrayList, ByRef strArrayErrorRecords As ArrayList, ByRef blnValidQuantity As Boolean, ByRef blnValidSequence As Boolean) As Boolean
        Dim result As Boolean = True

        Try
            'write contents of textbox to temp file, this will have only the KD plan data selected.
            If (result = True) Then result = WriteFile(path) And result

            '            If (path.Substring(path.Length - 3).Equals("txt")) Then
            'do validation on frame codes
            If (result = True) AndAlso ActivateFrameTest Then result = ValidFrameCode(path, strArrayErrorFrame) And result

            'do validation on record length
            If (result = True) AndAlso ActivateRecordLengthTest Then result = ValidRecordLength(path, strArrayErrorRecords) And result

            'do validation on quantity in N-1 matching quantity in N-2
            If (result = True) AndAlso ActivateQuantityTest Then result = ValidQuantity(path, blnValidQuantity) And result

            'do validation on sequence to make sure N-1 is replacing N-2 data and N-2 is replacing N-3 data
            'NOTE: production line validation is done by simply not displaying those KD plans that are a different production line
            If (result = True) AndAlso ActivateSequenceTest Then result = ValidSequence(blnValidSequence) And result
            'End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            result = False
        End Try

        Return result
    End Function

    Private Function WriteFile(ByVal path As String) As Boolean
        Dim result As Boolean = False
        Try
            'delete file if currently present
            If File.Exists(path) Then
                File.Delete(path)

            End If

            'open a new file for writing
            Using sWriter As New StreamWriter(path, False)
                'write file out
                sWriter.Write(tbFileContents.Text)

            End Using

            result = True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            result = False
        End Try

        Return result
    End Function

    Private Function ValidFrameCode(ByVal path As String, ByRef errorList As ArrayList) As Boolean
        'pull out all frame codes and store in an array and compare to data in SQL Server
        Dim result As Boolean = False

        Dim strLine As String
        Dim strValidFrameCodeList As ArrayList
        Dim strNoBuildIdentifierArray() As String
        Dim noBuildStartPosition As Integer
        Dim noBuildStringLength As Integer
        Dim strFrameCodeFromFile As String

        Dim bfound As Boolean

        Try
            strNoBuildIdentifierArray = ReturnNoBuildStringIdentifiers.Split(","c)
            noBuildStartPosition = ReturnNoBuildStartPosition()
            noBuildStringLength = ReturnNoBuildStringLength()
            strValidFrameCodeList = ReturnValidFrameCodes()    'load all the valid frame codes from the database

            'open a stream to path passed in
            Using sReader = New StreamReader(path)

                Do While sReader.Peek <> -1
                    bfound = False
                    strLine = sReader.ReadLine
                    strFrameCodeFromFile = strLine.Substring(11, 7) & strLine.Substring(31, 1)

                    'check NoBuild frame codes
                    For Each frame As String In strNoBuildIdentifierArray
                        If frame.ToUpper() = strFrameCodeFromFile.Substring(noBuildStartPosition - 1, noBuildStringLength).ToUpper() Then
                            bfound = True
                        End If
                    Next

                    'check enabled frame codes
                    For Each frameCode As String In strValidFrameCodeList
                        If frameCode.ToUpper() = strFrameCodeFromFile.ToUpper() Then
                            'found a valid frame code
                            bfound = True
                        End If
                    Next

                    If Not bfound AndAlso Not errorList.Contains(strFrameCodeFromFile) Then
                        'never found the frame code, add the error to the arraylist
                        errorList.Add(strFrameCodeFromFile)
                    End If
                Loop
            End Using

            result = True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            result = False
        End Try

        Return result
    End Function

    Private Function ValidRecordLength(ByVal path As String, ByRef errorList As ArrayList) As Boolean
        Dim result As Boolean = False

        Dim intLine As Integer = 0
        Dim strLine As String
        Dim lineLength As Integer = ReturnStringLength() 'return valid record length for file, this is configured in the database.

        'open file for reading
        Try
            'open a stream to path passed in
            Using sReader = New StreamReader(path)
                Do While sReader.Peek <> -1
                    strLine = sReader.ReadLine

                    'check length of each strLine and keep count of line
                    intLine = intLine + 1
                    If strLine.Length() <> lineLength Then
                        'if length is not the configured length, add line number to error arraylist 
                        errorList.Add(intLine)
                    End If
                Loop
            End Using

            result = True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            result = False
        End Try

        Return result
    End Function

    Private Function ValidQuantity(ByVal path As String, ByRef blnValid As Boolean) As Boolean
        Dim result As Boolean = False

        Dim strLine As String
        Dim strNfileDate As String
        Dim intDBOrderQty As Integer
        Dim intQty As Integer

        Try
            strNfileDate = lbNfiles.SelectedValue.Remove(0, 4)  'parse down to set just to month and date

            'determine if N-1 data, if N-1 data then process validation, if N-2 or N-3 return blnValid=true since no validation required for these
            If lbNfiles.SelectedValue.Substring(0, 3) = "N-1" Then
                'get order qty from database
                intDBOrderQty = ReturnDBOrderQty("N-2 " & strNfileDate)

                'open file for reading and get order quantity from file
                intQty = 0
                Using sReader = New StreamReader(path)
                    Do While sReader.Peek <> -1
                        strLine = sReader.ReadLine
                        'read each line and add quantity of each order
                        intQty = intQty + CInt(strLine.Substring(38, 5))
                    Loop
                End Using

                'check if quantites match then return true else false
                If intDBOrderQty = intQty Then
                    blnValid = True
                Else
                    blnValid = False
                End If

            Else
                'not N-1 data
                blnValid = True
            End If

            result = True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            result = False
        End Try

        Return result
    End Function

    Private Function ValidSequence(ByVal sequenceIsValid As Boolean) As Boolean
        Dim result As Boolean = False

        Dim mnth As String
        Dim yr As String
        Dim strfulldate As String
        Dim strNfileDate As String
        Dim OrderParameter As String

        Try
            sequenceIsValid = False
            strNfileDate = lbNfiles.SelectedValue.Remove(0, 4)
            If (TreeView.SelectedNode.Text.Substring(TreeView.SelectedNode.Text.Length - 3).Equals("txt")) And strEDICheck <> "ISA*" Then
                strNfileDate = strNfileDate.Substring(0, strNfileDate.LastIndexOf("-") - 1)

                'convert to mm\dd\yyyy format
                mnth = strNfileDate.Remove(strNfileDate.Length - 5, 5) 'parse out month
                yr = strNfileDate.Substring(strNfileDate.Length - 4, 4) 'parse out year
                strfulldate = mnth & "/" & "01/" & yr
            Else
                strNfileDate = strNfileDate.Substring(0, strNfileDate.LastIndexOf("-") - 1)
                strfulldate = CStr(CDate(strNfileDate))
            End If

            'subtract a month and format the date.
            strfulldate = CStr(CDate(strfulldate).AddMonths(-1))
            strfulldate = Format(CDate(strfulldate), "MMMM yyyy")

            'set default value to 1, so it assumes sequence is valid since it found 1 item with previous N-value
            Dim intDBSequence As Integer = 1


            If lbNfiles.SelectedValue.Substring(0, 3) = "N-2" Then          'if N-2 then check db
                'select if exists orderparameter value with current date
                OrderParameter = "N-1 " & strfulldate & "%"
                intDBSequence = ReturnDBOrderRecordCount(OrderParameter)

            ElseIf lbNfiles.SelectedValue.Substring(0, 3) = "N-3" Then      'if N-3 then check db
                'if N-3 then call
                'select if exists for current date
                OrderParameter = "N-2 " & strfulldate & "%"
                intDBSequence = ReturnDBOrderRecordCount(OrderParameter)

            Else
                'N-0 and N-1 and 830 do not require db check
                intDBSequence = 1
            End If

            If intDBSequence = 0 Then
                'no N-value found for previous month
                sequenceIsValid = False
            Else
                'either the data is N-1, or there were records found for previous N-value so can proceed with processing
                sequenceIsValid = True
            End If

            result = True           '<--- is this correct?  do all the above validation, throw it out and return true no matter what? 

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            result = False
        End Try
        Return result
    End Function

    Private Function ReturnValidFrameCodes() As ArrayList
        'return SQL for returning the list of Valid Frame codes
        Dim dt As DataTable = New DataTable
        Dim results As ArrayList = New ArrayList
        Try
            dt = DA.GetDataSet("SELECT productid from tblProducts where componentID='03'").Tables(0)
            For Each row As DataRow In dt.Rows()
                results.Add(row.Item(0).ToString())
            Next
            Return results
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return New ArrayList
        End Try
    End Function

    Private Function ReturnDBOrderQty(ByVal OrderParameterValue As String) As Integer
        Dim ds As DataSet
        Dim result As Integer
        Try
            ds = DA.GetDataSet("select sum(cast(orderparameterValue as integer)) from tblOrderData    where orderparameterid='0008' and sequenceNumber in (select sequencenumber from tblOrderData where OrderParameterValue='" & OrderParameterValue & "')")

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) AndAlso Not IsDBNull(ds.Tables(0).Rows(0).Item(0)) Then
                result = CInt(ds.Tables(0).Rows(0).Item(0))
            Else
                result = 0
            End If

            Return result
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return -1
        End Try
    End Function

    Private Function ReturnDBOrderRecordCount(ByVal OrderParameterValue As String) As Integer
        Dim ds As DataSet
        Dim result As Integer
        Try
            ds = DA.GetDataSet("SELECT count(*) from tblorderdata where OrderParameterID='0154' and OrderParameterValue Like '" & OrderParameterValue & "'")

            If ((ds IsNot Nothing) AndAlso (ds.Tables(0).Rows.Count > 0)) Then
                result = CInt(ds.Tables(0).Rows(0).Item(0))
            Else
                result = 0
            End If

            Return result
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return -1
        End Try
    End Function

#End Region

#Region "Get or Set ApplicationParameterValues"

    'return directory for archived files
    Function ReturnOutbound() As String
        Dim strOutbound As String = CStr(ViewState("Outbound"))
        If (strOutbound Is Nothing OrElse strOutbound.Trim().Length = 0) Then
            strOutbound = BizLayer.GetApplicationParameterValue("2200", BatchModuleApplicationID)
            ViewState("Outbound") = strOutbound
        End If
        Return strOutbound
    End Function

    'return directory for inbound files
    Function ReturnInbound() As String
        Dim strInbound As String = CStr(ViewState("Inbound"))
        If (strInbound Is Nothing OrElse strInbound Is Nothing OrElse strInbound.Trim().Length = 0) Then
            strInbound = BizLayer.GetApplicationParameterValue("2201", BatchModuleApplicationID)
            ViewState("Inbound") = strInbound
        End If
        Return strInbound
    End Function

    'Return directory for DTS job to look for data to process
    Function ReturnDTSFile() As String
        Dim strDTSFile As String = CStr(ViewState("DTSFile"))
        If (strDTSFile Is Nothing OrElse strDTSFile.Trim().Length = 0) Then
            strDTSFile = BizLayer.GetApplicationParameterValue("2202", BatchModuleApplicationID)
            ViewState("DTSFile") = strDTSFile
        End If
        Return strDTSFile
    End Function

    Function WriteNValue(ByVal strNvalue As String) As Boolean
        Try
            'remove asterick if present
            strNvalue = "'" & strNvalue.Replace("*", "") & "'"

            'write n-value to database location for use with DTS file and package
            BizLayer.SetApplicationParameterValue("2203", strNvalue, BatchModuleApplicationID)

            Return True
        Catch ex As Exception
            Master.eMsg(ex.ToString())
            Return False
        End Try
    End Function

    Function ReturnStringLength() As Integer
        Dim len As Integer = 0
        Dim strStringLength As String = CStr(ViewState("StringLength"))
        If (strStringLength Is Nothing OrElse strStringLength.Trim().Length = 0) Then
            strStringLength = BizLayer.GetApplicationParameterValue("2205", BatchModuleApplicationID)
            ViewState("StringLength") = strStringLength
        End If
        Integer.TryParse(strStringLength, len)
        Return len
    End Function

    'return directory for valid record length of file
    Function ReturnNoBuildStartPosition() As Integer
        Dim pos As Integer = 0
        Dim strNoBuildStartPosition As String = CStr(ViewState("NoBuildStartPosition"))
        If (strNoBuildStartPosition Is Nothing OrElse strNoBuildStartPosition.Trim().Length = 0) Then
            strNoBuildStartPosition = BizLayer.GetApplicationParameterValue("2210", BatchModuleApplicationID)
            ViewState("NoBuildStartPosition") = strNoBuildStartPosition
        End If
        Integer.TryParse(strNoBuildStartPosition, pos)
        Return pos
    End Function

    'return directory for valid record length of file
    Function ReturnNoBuildStringLength() As Integer
        Dim len As Integer = 0
        Dim strNoBuildStringLen As String = CStr(ViewState("NoBuildStringLen"))
        If (strNoBuildStringLen Is Nothing OrElse strNoBuildStringLen.Trim().Length = 0) Then
            strNoBuildStringLen = BizLayer.GetApplicationParameterValue("2211", BatchModuleApplicationID)
            ViewState("NoBuildStringLen") = strNoBuildStringLen
        End If
        Integer.TryParse(strNoBuildStringLen, len)
        Return len
    End Function

    'return directory for valid record length of file
    Function ReturnNoBuildStringIdentifiers() As String
        Dim strNBIdentifier As String = CStr(ViewState("NoBuildStringIdentifiers"))
        If (strNBIdentifier Is Nothing OrElse strNBIdentifier.Trim().Length = 0) Then
            strNBIdentifier = BizLayer.GetApplicationParameterValue("2212", BatchModuleApplicationID)
            ViewState("NoBuildStringIdentifiers") = strNBIdentifier
        End If
        Return strNBIdentifier
    End Function

    Function ReturnModelExceptionStringIdentifiers() As String
        Dim strMEIdentifier As String = CStr(ViewState("ModelExceptionStringIdentifiers"))
        If (strMEIdentifier Is Nothing OrElse strMEIdentifier.Trim().Length = 0) Then
            strMEIdentifier = BizLayer.GetApplicationParameterValue("2240", BatchModuleApplicationID)
            ViewState("ModelExceptionStringIdentifiers") = strMEIdentifier
        End If
        Return strMEIdentifier
    End Function

    'get username
    Function ReturnUser() As String
        Dim strUser As String = CStr(ViewState("User"))
        If (strUser Is Nothing OrElse strUser.Trim().Length = 0) Then
            strUser = BizLayer.GetApplicationParameterValue("2213", BatchModuleApplicationID)
            ViewState("User") = strUser
        End If
        Return strUser
    End Function

    'get password
    Function ReturnPassword() As String
        Dim strPassword As String = CStr(ViewState("Password"))
        If (strPassword Is Nothing OrElse strPassword.Trim().Length = 0) Then
            strPassword = BizLayer.GetApplicationParameterValue("2214", BatchModuleApplicationID)
            ViewState("Password") = strPassword
        End If
        Return strPassword
    End Function

    'get domain
    Function ReturnDomain() As String
        Dim strDomain As String = CStr(ViewState("Domain"))
        If (strDomain Is Nothing OrElse strDomain.Trim().Length = 0) Then
            strDomain = BizLayer.GetApplicationParameterValue("2215", BatchModuleApplicationID)
            ViewState("Domain") = strDomain
        End If
        Return strDomain
    End Function

    Function ReturnNZeroDate() As String
        Dim strNZeroDate As String = CStr(ViewState("NZeroDate"))
        If (strNZeroDate Is Nothing OrElse strNZeroDate.Trim().Length = 0) Then
            strNZeroDate = BizLayer.GetApplicationParameterValue("2236", BatchModuleApplicationID)
            ViewState("NZeroDate") = strNZeroDate
        End If
        Return strNZeroDate
    End Function

#End Region

#Region "Dialog modalProcess_.aspx"

    Private Sub divDialogProcessAccept_Click()
        Session("KDProcess") = "TRUE"
        If CBool(Session("N0Indicator")) = True Then
            Session("N0Process") = "TRUE"
        Else
            Session("N0Process") = "False"
        End If
    End Sub

    Private Sub divDialogProcessCancel_Click()
        Session("KDProcess") = "FALSE"
        Session("KDReport") = "FALSE"
        If CBool(Session("N0Indicator")) = True Then
            Session("N0Process") = "FALSE"
            Session("N0Report") = "FALSE"
        Else
            Session("N0Process") = "TRUE"
        End If
    End Sub

#End Region


End Class

