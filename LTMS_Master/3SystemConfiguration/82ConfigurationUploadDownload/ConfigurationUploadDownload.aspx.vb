Imports System.Data.SqlClient

Public Class ConfigurationUploadDownload
    Inherits System.Web.UI.Page

    Const DownloadStart As String = "0001"
    Const UploadStart As String = "0002"
    Const PLCLastModifiedDT As String = "0003"
    Const JITLastModifiedDT As String = "0004"
    Const DownloadPercentDone As String = "0007"
    Const UploadPercentDone As String = "0008"
    '' currently not used on page 
    'Const TransactionTriggerInteger As String = "0005"
    'Const LineDataID As String = "0006"
    'Const InboundHeatbeatValue As String = "0009"
    'Const InboundHeatbeatCount As String = "0010"
    'Const InboundHeartbeatDT As String = "0011"


    Const LineTypeID_Other = "0"
    Const LineTypeID_Front = "1"
    Const LineTypeID_Rear = "2"
    Const LineTypeID_Shipping = "3"

    Enum CmdButtonType
        Download
        Upload
    End Enum

    Enum TransactionID
        BuildAuxTask = 1
        BuildDataCollection = 2
        BuildComponentScan = 3
        UserLogin = 4
        BuildStation = 5
        BuildTool = 6
        BuildHeartbeat = 7
        QualityAuxTask = 8
        QualityDataCollection = 9
        QualityComponentScan = 10
        QualityStation = 11
        QualityTool = 12
        QualityHeartbeat = 13
        ComponentNameData = 14
        ProductComponentCode = 15
        StepDetail = 16
        PreShipVerify = 17
        Verify = 18
        ShippingPrintStation = 19
        ShippingHeartbeat = 20
        ShippingDataCollection = 21
    End Enum


#Region "Page Events"

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                ddLineNumber_DataBind()
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Try
            SecureControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "User Events"

    Private Sub ddLineNumber_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddLineNumber.SelectedIndexChanged
        Try
            ClearAll_LastModified_Labels()
            ClearAll_ProgressBar_Values()
            ClearAll_HiddenInit()

            If (ddLineNumber.SelectedItem IsNot Nothing) And (ddLineNumber.SelectedIndex > 0) Then
                hidLineTypeID.Value = CStr(BizLayer.GetLineTypeID(CInt(ddLineNumber.SelectedValue)))
                hidMultiPLC.Value = CStr(BizLayer.GetMultiPLC(CInt(ddLineNumber.SelectedValue)))
                GetDataIntoCTRLs()
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdUploadDownloadHelper(message As String, cmdType As CmdButtonType, pbUp As HiddenField, pbDn As HiddenField, cmdButton As Button, transID As TransactionID, cmdTypePercentDone As String, cmdTypeStart As String)
        Try
            If cmdButton.Text = "Reset" Then
                pbUp.Value = "0"
                pbDn.Value = "0"
                Master.Msg = message + " Reset"
                UpdateTransactionParameters(transID, cmdTypeStart, "0")
                UpdateTransactionParameters(transID, cmdTypePercentDone, "100")
            Else
                UpdateTransactionParameters(transID, cmdTypeStart, "1")
                UpdateTransactionParameters(transID, cmdTypePercentDone, "0")
                Master.Msg = message + " Initiated!"
                If (cmdType = CmdButtonType.Download) Then
                    pbUp.Value = "1"
                    pbDn.Value = "0"
                Else
                    pbUp.Value = "0"
                    pbDn.Value = "1"
                End If
            End If
            GetDataIntoCTRLs()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDnldAux_Click(sender As Object, e As System.EventArgs) Handles cmdDnldAux.Click
        Try
            Dim strMsg As String = "Aux Task Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldAux, hidInitpbUpldAux, cmdDnldAux, TransactionID.BuildAuxTask, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldAux_Click(sender As Object, e As System.EventArgs) Handles cmdUpldAux.Click
        Try
            Dim strMsg As String = "Aux Task Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldAux, hidInitpbUpldAux, cmdUpldAux, TransactionID.BuildAuxTask, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdDnldQualAux_Click(sender As Object, e As System.EventArgs) Handles cmdDnldQualAux.Click
        Try
            Dim strMsg As String = "Aux Task Quality Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldQualAux, hidInitpbUpldQualAux, cmdDnldQualAux, TransactionID.QualityAuxTask, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldQualAux_Click(sender As Object, e As System.EventArgs) Handles cmdUpldQualAux.Click
        Try
            Dim strMsg As String = "Aux Task Quality Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldQualAux, hidInitpbUpldQualAux, cmdUpldQualAux, TransactionID.QualityAuxTask, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdDnldCompScan_Click(sender As Object, e As System.EventArgs) Handles cmdDnldCompScan.Click
        Try
            Dim strMsg As String = "Component Scan Enable Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldCompScan, hidInitpbUpldCompScan, cmdDnldCompScan, TransactionID.BuildComponentScan, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldCompScan_Click(sender As Object, e As System.EventArgs) Handles cmdUpldCompScan.Click
        Try
            Dim strMsg As String = "Component Scan Enable Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldCompScan, hidInitpbUpldCompScan, cmdUpldCompScan, TransactionID.BuildComponentScan, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdDnldQualCompScan_Click(sender As Object, e As System.EventArgs) Handles cmdDnldQualCompScan.Click
        Try
            Dim strMsg As String = "Component Scan Enable Quality Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldQualCompScan, hidInitpbUpldQualCompScan, cmdDnldQualCompScan, TransactionID.QualityComponentScan, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldQualCompScan_Click(sender As Object, e As System.EventArgs) Handles cmdUpldQualCompScan.Click
        Try
            Dim strMsg As String = ""
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldQualCompScan, hidInitpbUpldQualCompScan, cmdUpldQualCompScan, TransactionID.QualityComponentScan, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdDnldCompName_Click(sender As Object, e As System.EventArgs) Handles cmdDnldCompName.Click
        Try
            Dim strMsg As String = "Component Name Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldCompName, hidInitpbUpldCompName, cmdDnldCompName, TransactionID.ComponentNameData, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldCompName_Click(sender As Object, e As System.EventArgs) Handles cmdUpldCompName.Click
        Try
            Dim strMsg As String = "Component Name Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldCompName, hidInitpbUpldCompName, cmdUpldCompName, TransactionID.ComponentNameData, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdDnldTool_Click(sender As Object, e As System.EventArgs) Handles cmdDnldTool.Click
        Try
            Dim strMsg As String = "Tool Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldTool, hidInitpbUpldTool, cmdDnldTool, TransactionID.BuildTool, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldTool_Click(sender As Object, e As System.EventArgs) Handles cmdUpldTool.Click
        Try
            Dim strMsg As String = "Tool Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldTool, hidInitpbUpldTool, cmdUpldTool, TransactionID.BuildTool, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdDnldQualTool_Click(sender As Object, e As System.EventArgs) Handles cmdDnldQualTool.Click
        Try
            Dim strMsg As String = "Tool Quality Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldQualTool, hidInitpbUpldQualTool, cmdDnldQualTool, TransactionID.QualityTool, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldQualTool_Click(sender As Object, e As System.EventArgs) Handles cmdUpldQualTool.Click
        Try
            Dim strMsg As String = "Tool Quality upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldQualTool, hidInitpbUpldQualTool, cmdUpldQualTool, TransactionID.QualityTool, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDnldStation_Click(sender As Object, e As System.EventArgs) Handles cmdDnldStation.Click
        Try
            Dim strMsg As String = "Station Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldStation, hidInitpbUpldStation, cmdDnldStation, TransactionID.BuildStation, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldStation_Click(sender As Object, e As System.EventArgs) Handles cmdUpldStation.Click
        Try
            Dim strMsg As String = "Station Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldStation, hidInitpbUpldStation, cmdUpldStation, TransactionID.BuildStation, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdDnldQualStation_Click(sender As Object, e As System.EventArgs) Handles cmdDnldQualStation.Click
        Try
            Dim strMsg As String = "Station Quality Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldQualStation, hidInitpbUpldQualStation, cmdDnldQualStation, TransactionID.QualityStation, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldQualStation_Click(sender As Object, e As System.EventArgs) Handles cmdUpldQualStation.Click
        Try
            Dim strMsg As String = "Station Quality Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldQualStation, hidInitpbUpldQualStation, cmdUpldQualStation, TransactionID.QualityStation, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdDnldProdCode_Click(sender As Object, e As System.EventArgs) Handles cmdDnldProdCode.Click
        Try
            Dim strMsg As String = "Product Code Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldProdCode, hidInitpbUpldProdCode, cmdDnldProdCode, TransactionID.ProductComponentCode, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldProdCode_Click(sender As Object, e As System.EventArgs) Handles cmdUpldProdCode.Click
        Try
            Dim strMsg As String = "Product Code Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldProdCode, hidInitpbUpldProdCode, cmdUpldProdCode, TransactionID.ProductComponentCode, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdDnldUser_Click(sender As Object, e As System.EventArgs) Handles cmdDnldUser.Click
        Try
            Dim strMsg As String = "User Download"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Download, hidInitpbDnldUser, hidInitpbUpldUser, cmdDnldUser, TransactionID.UserLogin, DownloadPercentDone, DownloadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub cmdUpldUser_Click(sender As Object, e As System.EventArgs) Handles cmdUpldUser.Click
        Try
            Dim strMsg As String = "User Upload"
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear, LineTypeID_Shipping
                    cmdUploadDownloadHelper(strMsg, CmdButtonType.Upload, hidInitpbDnldUser, hidInitpbUpldUser, cmdUpldUser, TransactionID.UserLogin, UploadPercentDone, UploadStart)
                Case Else
                    Master.Msg = "There is no " + strMsg + " for this line!"
            End Select
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Sub cmdRefreshAndClearComplete_Click(sender As Object, e As System.EventArgs) Handles cmdRefreshAndClearComplete.Click
        Try
            ClearAll_HiddenInit()
            GetDataIntoCTRLs()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        Try
            GetDataIntoCTRLs()
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub ddLineNumber_DataBind()
        ddLineNumber.DataSource = DA.GetDataSet("SELECT LineNumber, LineName FROM dbo.tblSGLines ORDER BY LineName")
        ddLineNumber.DataTextField = "LineName"
        ddLineNumber.DataValueField = "LineNumber"
        ddLineNumber.DataBind()
        ddLineNumber.Items.Insert(0, New ListItem("Choose a Line", "0"))
    End Sub

    Private Sub ClearAll_LastModified_Labels()
        Dim tmpStr As String = "--"

        lblPLCAux.Text = tmpStr
        lblJITAux.Text = tmpStr
        lblPLCQualAux.Text = tmpStr

        lblPLCCompScan.Text = tmpStr
        lblJITCompScan.Text = tmpStr
        lblPLCQualCompScan.Text = tmpStr

        lblPLCCompName.Text = tmpStr
        lblJITCompName.Text = tmpStr
        lblPLCQualCompName.Text = tmpStr

        lblPLCTool.Text = tmpStr
        lblJITTool.Text = tmpStr
        lblPLCQualTool.Text = tmpStr

        lblPLCStation.Text = tmpStr
        lblJITStation.Text = tmpStr
        lblPLCQualStation.Text = tmpStr

        lblPLCProdCode.Text = tmpStr
        lblJITProdCode.Text = tmpStr
        lblPLCQualProdCode.Text = tmpStr

        lblPLCUser.Text = tmpStr
        lblJITUser.Text = tmpStr
        lblPLCQualUser.Text = tmpStr
    End Sub

    Private Sub ClearAll_ProgressBar_Values()
        Dim tmpStr As String = "0"

        hidpbDnldAux.Value = tmpStr
        hidpbUpldAux.Value = tmpStr

        hidpbDnldCompScan.Value = tmpStr
        hidpbUpldCompScan.Value = tmpStr

        hidpbDnldCompName.Value = tmpStr
        hidpbUpldCompName.Value = tmpStr

        hidpbDnldTool.Value = tmpStr
        hidpbUpldTool.Value = tmpStr

        hidpbDnldStation.Value = tmpStr
        hidpbUpldStation.Value = tmpStr

        hidpbDnldProdCode.Value = tmpStr
        hidpbUpldProdCode.Value = tmpStr

        hidpbDnldUser.Value = tmpStr
        hidpbUpldUser.Value = tmpStr

        'Quality Progress
        hidpbDnldQualAux.Value = tmpStr
        hidpbUpldQualAux.Value = tmpStr

        hidpbDnldQualCompScan.Value = tmpStr
        hidpbUpldQualCompScan.Value = tmpStr

        hidpbDnldQualCompName.Value = tmpStr
        hidpbUpldQualCompName.Value = tmpStr

        hidpbDnldQualTool.Value = tmpStr
        hidpbUpldQualTool.Value = tmpStr

        hidpbDnldQualStation.Value = tmpStr
        hidpbUpldQualStation.Value = tmpStr

        hidpbDnldQualProdCode.Value = tmpStr
        hidpbUpldQualProdCode.Value = tmpStr

        hidpbDnldQualUser.Value = tmpStr
        hidpbUpldQualUser.Value = tmpStr

    End Sub

    Private Sub ClearAll_HiddenInit()
        hidInitpbDnldAux.Value = "0"
        hidInitpbUpldAux.Value = "0"
        hidInitpbDnldCompScan.Value = "0"
        hidInitpbUpldCompScan.Value = "0"
        hidInitpbDnldCompName.Value = "0"
        hidInitpbUpldCompName.Value = "0"
        hidInitpbDnldTool.Value = "0"
        hidInitpbUpldTool.Value = "0"
        hidInitpbDnldStation.Value = "0"
        hidInitpbUpldStation.Value = "0"
        hidInitpbDnldProdCode.Value = "0"
        hidInitpbUpldProdCode.Value = "0"
        hidInitpbDnldUser.Value = "0"
        hidInitpbUpldUser.Value = "0"

        hidInitpbDnldQualAux.Value = "0"
        hidInitpbUpldQualAux.Value = "0"
        hidInitpbDnldQualCompScan.Value = "0"
        hidInitpbUpldQualCompScan.Value = "0"
        hidInitpbDnldQualCompName.Value = "0"
        hidInitpbUpldQualCompName.Value = "0"
        hidInitpbDnldQualTool.Value = "0"
        hidInitpbUpldQualTool.Value = "0"
        hidInitpbDnldQualStation.Value = "0"
        hidInitpbUpldQualStation.Value = "0"
        hidInitpbDnldQualProdCode.Value = "0"
        hidInitpbUpldQualProdCode.Value = "0"
        hidInitpbDnldQualUser.Value = "0"
        hidInitpbUpldQualUser.Value = "0"

    End Sub

    Private Sub SecureControls()
        Master.Secure(cmdRefresh)

        Master.Secure(cmdDnldAux)
        Master.Secure(cmdUpldAux)
        Master.Secure(cmdDnldCompScan)
        Master.Secure(cmdUpldCompScan)
        Master.Secure(cmdDnldCompName)
        Master.Secure(cmdUpldCompName)
        Master.Secure(cmdDnldTool)
        Master.Secure(cmdUpldTool)
        Master.Secure(cmdDnldStation)
        Master.Secure(cmdUpldStation)
        Master.Secure(cmdDnldProdCode)
        Master.Secure(cmdUpldProdCode)
        Master.Secure(cmdDnldUser)
        Master.Secure(cmdUpldUser)

        Master.Secure(cmdDnldQualAux)
        Master.Secure(cmdUpldQualAux)
        Master.Secure(cmdDnldQualCompScan)
        Master.Secure(cmdUpldQualCompScan)
        Master.Secure(cmdDnldQualCompName)
        Master.Secure(cmdUpldQualCompName)
        Master.Secure(cmdDnldQualTool)
        Master.Secure(cmdUpldQualTool)
        Master.Secure(cmdDnldQualStation)
        Master.Secure(cmdUpldQualStation)
        Master.Secure(cmdDnldQualProdCode)
        Master.Secure(cmdUpldQualProdCode)
        Master.Secure(cmdDnldQualUser)
        Master.Secure(cmdUpldQualUser)

    End Sub

    Private Sub EnableControls()
        Try
            DisableIfInProgress()
            EnableQualityButtons()
            EnableAuxAndTool()
            EnableIfTransactionFailed()

            If (ddLineNumber.SelectedItem Is Nothing) Or (ddLineNumber.SelectedIndex < 1) Then
                cmdRefresh.Enabled = False

                cmdDnldAux.Enabled = False
                cmdUpldAux.Enabled = False
                cmdDnldCompScan.Enabled = False
                cmdUpldCompScan.Enabled = False
                cmdDnldCompName.Enabled = False
                cmdUpldCompName.Enabled = False
                cmdDnldTool.Enabled = False
                cmdUpldTool.Enabled = False
                cmdDnldStation.Enabled = False
                cmdUpldStation.Enabled = False
                cmdDnldProdCode.Enabled = False
                cmdUpldProdCode.Enabled = False
                cmdDnldUser.Enabled = False
                cmdUpldUser.Enabled = False

                cmdDnldQualAux.Enabled = False
                cmdUpldQualAux.Enabled = False
                cmdDnldQualCompScan.Enabled = False
                cmdUpldQualCompScan.Enabled = False
                cmdDnldQualCompName.Enabled = False
                cmdUpldQualCompName.Enabled = False
                cmdDnldQualTool.Enabled = False
                cmdUpldQualTool.Enabled = False
                cmdDnldQualStation.Enabled = False
                cmdUpldQualStation.Enabled = False
                cmdDnldQualProdCode.Enabled = False
                cmdUpldQualProdCode.Enabled = False
                cmdDnldQualUser.Enabled = False
                cmdUpldQualUser.Enabled = False
            End If

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Sub EnableQualityButtons()
        Dim bResult As Boolean = False
        Try
            Boolean.TryParse(hidMultiPLC.Value, bResult)

            ''If try parse of hidMultiPLC.Value is succesful then use bResult else bResult is false.
            cmdDnldQualAux.Enabled = cmdDnldQualAux.Enabled And bResult
            cmdUpldQualAux.Enabled = cmdUpldQualAux.Enabled And bResult
            cmdDnldQualCompScan.Enabled = cmdDnldQualCompScan.Enabled And bResult
            cmdUpldQualCompScan.Enabled = cmdUpldQualCompScan.Enabled And bResult
            cmdDnldQualCompName.Enabled = cmdDnldQualCompName.Enabled And bResult
            cmdUpldQualCompName.Enabled = cmdUpldQualCompName.Enabled And bResult
            cmdDnldQualTool.Enabled = cmdDnldQualTool.Enabled And bResult
            cmdUpldQualTool.Enabled = cmdUpldQualTool.Enabled And bResult
            cmdDnldQualStation.Enabled = cmdDnldQualStation.Enabled And bResult
            cmdUpldQualStation.Enabled = cmdUpldQualStation.Enabled And bResult
            cmdDnldQualProdCode.Enabled = cmdDnldQualProdCode.Enabled And bResult
            cmdUpldQualProdCode.Enabled = cmdUpldQualProdCode.Enabled And bResult
            cmdDnldQualUser.Enabled = cmdDnldQualUser.Enabled And bResult
            cmdUpldQualUser.Enabled = cmdUpldQualUser.Enabled And bResult


            cmdDnldQualAux.Visible = bResult
            cmdUpldQualAux.Visible = bResult
            cmdDnldQualCompScan.Visible = bResult
            cmdUpldQualCompScan.Visible = bResult
            cmdDnldQualCompName.Visible = False 'Not used at this time
            cmdUpldQualCompName.Visible = False 'Not used at this time
            cmdDnldQualTool.Visible = bResult
            cmdUpldQualTool.Visible = bResult
            cmdDnldQualStation.Visible = bResult
            cmdUpldQualStation.Visible = bResult
            cmdDnldQualProdCode.Visible = False 'Not used at this time
            cmdUpldQualProdCode.Visible = False 'Not used at this time
            cmdDnldQualUser.Visible = False 'Not used at this time
            cmdUpldQualUser.Visible = False 'Not used at this time

            If ((bResult = False) And (ddLineNumber.SelectedIndex > 0)) Then
                lblPLCQualAux.Text = ""
                lblPLCQualCompScan.Text = ""
                lblPLCQualCompName.Text = ""
                lblPLCQualTool.Text = ""
                lblPLCQualStation.Text = ""
                lblPLCQualProdCode.Text = ""
                lblPLCQualUser.Text = ""

            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub EnableAuxAndTool()
        Dim bValue As Boolean = False
        Try
            Select Case (hidLineTypeID.Value)
                Case LineTypeID_Front, LineTypeID_Rear
                    bValue = True
                Case Else
                    bValue = False
            End Select

            cmdDnldAux.Enabled = cmdDnldAux.Enabled And bValue
            cmdUpldAux.Enabled = cmdUpldAux.Enabled And bValue
            cmdDnldQualAux.Enabled = cmdDnldQualAux.Enabled And bValue
            cmdUpldQualAux.Enabled = cmdUpldQualAux.Enabled And bValue

            cmdDnldTool.Enabled = cmdDnldTool.Enabled And bValue
            cmdUpldTool.Enabled = cmdUpldTool.Enabled And bValue
            cmdDnldQualTool.Enabled = cmdDnldQualTool.Enabled And bValue
            cmdUpldQualTool.Enabled = cmdUpldQualTool.Enabled And bValue
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub DisableIfInProgress()
        Try

            cmdDnldAux.Enabled = DisableInProgressHelper(hidpbDnldAux.Value, hidInitpbDnldAux.Value, hidpbUpldAux.Value, hidInitpbUpldAux.Value)
            cmdUpldAux.Enabled = cmdDnldAux.Enabled

            cmdDnldCompScan.Enabled = DisableInProgressHelper(hidpbDnldCompScan.Value, hidInitpbDnldCompScan.Value, hidpbUpldCompScan.Value, hidInitpbUpldCompScan.Value)
            cmdUpldCompScan.Enabled = cmdDnldCompScan.Enabled

            cmdDnldCompName.Enabled = DisableInProgressHelper(hidpbDnldCompName.Value, hidInitpbDnldCompName.Value, hidpbUpldCompName.Value, hidInitpbUpldCompName.Value)
            cmdUpldCompName.Enabled = cmdDnldCompName.Enabled

            cmdDnldTool.Enabled = DisableInProgressHelper(hidpbDnldTool.Value, hidInitpbDnldTool.Value, hidpbUpldTool.Value, hidInitpbUpldTool.Value)
            cmdUpldTool.Enabled = cmdDnldTool.Enabled

            cmdDnldStation.Enabled = DisableInProgressHelper(hidpbDnldStation.Value, hidInitpbDnldStation.Value, hidpbUpldStation.Value, hidInitpbUpldStation.Value)
            cmdUpldStation.Enabled = cmdDnldStation.Enabled

            cmdDnldProdCode.Enabled = DisableInProgressHelper(hidpbDnldProdCode.Value, hidInitpbDnldProdCode.Value, hidpbUpldProdCode.Value, hidInitpbUpldProdCode.Value)
            cmdUpldProdCode.Enabled = cmdDnldProdCode.Enabled

            cmdDnldUser.Enabled = DisableInProgressHelper(hidpbDnldUser.Value, hidInitpbDnldUser.Value, hidpbUpldUser.Value, hidInitpbUpldUser.Value)
            cmdUpldUser.Enabled = cmdDnldUser.Enabled


            cmdDnldQualAux.Enabled = DisableInProgressHelper(hidpbDnldQualAux.Value, hidInitpbDnldQualAux.Value, hidpbUpldQualAux.Value, hidInitpbUpldQualAux.Value)
            cmdUpldQualAux.Enabled = cmdDnldQualAux.Enabled

            cmdDnldQualCompScan.Enabled = DisableInProgressHelper(hidpbDnldQualCompScan.Value, hidInitpbDnldQualCompScan.Value, hidpbUpldQualCompScan.Value, hidInitpbUpldQualCompScan.Value)
            cmdUpldQualCompScan.Enabled = cmdDnldQualCompScan.Enabled

            cmdDnldQualCompName.Enabled = DisableInProgressHelper(hidpbDnldQualCompName.Value, hidInitpbDnldQualCompName.Value, hidpbUpldQualCompName.Value, hidInitpbUpldQualCompName.Value)
            cmdUpldQualCompName.Enabled = cmdDnldQualCompName.Enabled

            cmdDnldQualTool.Enabled = DisableInProgressHelper(hidpbDnldQualTool.Value, hidInitpbDnldQualTool.Value, hidpbUpldQualTool.Value, hidInitpbUpldQualTool.Value)
            cmdUpldQualTool.Enabled = cmdDnldQualTool.Enabled

            cmdDnldQualStation.Enabled = DisableInProgressHelper(hidpbDnldQualStation.Value, hidInitpbDnldQualStation.Value, hidpbUpldQualStation.Value, hidInitpbUpldQualStation.Value)
            cmdUpldQualStation.Enabled = cmdDnldQualStation.Enabled

            cmdDnldQualProdCode.Enabled = DisableInProgressHelper(hidpbDnldQualProdCode.Value, hidInitpbDnldQualProdCode.Value, hidpbUpldQualProdCode.Value, hidInitpbUpldQualProdCode.Value)
            cmdUpldQualProdCode.Enabled = cmdDnldQualProdCode.Enabled

            cmdDnldQualUser.Enabled = DisableInProgressHelper(hidpbDnldQualUser.Value, hidInitpbDnldQualUser.Value, hidpbUpldQualUser.Value, hidInitpbUpldQualUser.Value)
            cmdUpldQualUser.Enabled = cmdDnldQualUser.Enabled


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub
    Private Function DisableInProgressHelper(pbDnldValue As String, ByRef initDnldValue As String, pbUpldValue As String, ByRef initUpldValue As String) As Boolean
        Dim bResult As Boolean = False
        Dim maxValue As Double = 99
        Try
            Dim dnldValue As Double = CDbl(pbDnldValue)
            If (dnldValue > 0 And dnldValue < maxValue) Then
                initDnldValue = "1"
            End If

            Dim upldValue As Double = CDbl(pbUpldValue)
            If (upldValue > 0 And upldValue < maxValue) Then
                initUpldValue = "1"
            End If
            bResult = Not ((initDnldValue = "1" And dnldValue < maxValue) Or (initUpldValue = "1" And upldValue < maxValue))
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return bResult
    End Function


    Private Sub EnableIfTransactionFailed()
        EnableIfTransactionFailedHelper(cmdDnldAux)
        EnableIfTransactionFailedHelper(cmdUpldAux)
        EnableIfTransactionFailedHelper(cmdDnldCompScan)
        EnableIfTransactionFailedHelper(cmdUpldCompScan)
        EnableIfTransactionFailedHelper(cmdDnldCompName)
        EnableIfTransactionFailedHelper(cmdUpldCompName)
        EnableIfTransactionFailedHelper(cmdDnldTool)
        EnableIfTransactionFailedHelper(cmdUpldTool)
        EnableIfTransactionFailedHelper(cmdDnldStation)
        EnableIfTransactionFailedHelper(cmdUpldStation)
        EnableIfTransactionFailedHelper(cmdDnldProdCode)
        EnableIfTransactionFailedHelper(cmdUpldProdCode)
        EnableIfTransactionFailedHelper(cmdDnldUser)
        EnableIfTransactionFailedHelper(cmdUpldUser)

        EnableIfTransactionFailedHelper(cmdDnldQualAux)
        EnableIfTransactionFailedHelper(cmdUpldQualAux)
        EnableIfTransactionFailedHelper(cmdDnldQualCompScan)
        EnableIfTransactionFailedHelper(cmdUpldQualCompScan)
        EnableIfTransactionFailedHelper(cmdDnldQualCompName)
        EnableIfTransactionFailedHelper(cmdUpldQualCompName)
        EnableIfTransactionFailedHelper(cmdDnldQualTool)
        EnableIfTransactionFailedHelper(cmdUpldQualTool)
        EnableIfTransactionFailedHelper(cmdDnldQualStation)
        EnableIfTransactionFailedHelper(cmdUpldQualStation)
        EnableIfTransactionFailedHelper(cmdDnldQualProdCode)
        EnableIfTransactionFailedHelper(cmdUpldQualProdCode)
        EnableIfTransactionFailedHelper(cmdDnldQualUser)
        EnableIfTransactionFailedHelper(cmdUpldQualUser)
    End Sub
    Private Sub EnableIfTransactionFailedHelper(ctrl As Button)
        If (ctrl.Text = "Reset") Then ctrl.Enabled = True
    End Sub

    Private Sub SetProgressBarValue(hidnPBField As HiddenField, hidnInitField As HiddenField, btn As Button, value As String)
        Try
            Dim oldValue As Double
            Dim newValue As Double
            Dim secondsElapsed As Double = 0
            Dim failAfterSeconds As Double
            Dim updateTime As DateTime
            Dim strClass As String
            Dim btnName As String = "Upload"

            failAfterSeconds = CDbl(hidFailAfterSeconds.Value)
            oldValue = CDbl(hidnPBField.Value)
            newValue = CDbl(value)

            hidnPBField.Value = value

            strClass = btn.CssClass.Replace(" failed", "")
            If strClass.Contains("download") Then
                btnName = "Download"
            End If

            If (DateTime.TryParse(btn.Attributes("updateTime"), updateTime)) Then
                secondsElapsed = (DateTime.Now() - updateTime).TotalSeconds()
            End If

            If ((oldValue <> newValue) Or newValue > 99 Or secondsElapsed = 0) Then
                btn.Attributes.Add("updateTime", DateTime.Now().ToString())
            ElseIf (secondsElapsed > failAfterSeconds) Then
                btnName = "Reset"
                strClass += " failed"
            End If

            If (oldValue = 0 And newValue = 100) Then
                hidnInitField.Value = "0"
            End If

            btn.Text = btnName
            btn.CssClass = strClass


        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub GetDataIntoCTRLs()
        Try

            If (ddLineNumber.SelectedItem IsNot Nothing) And (ddLineNumber.SelectedIndex > 0) Then

                lblPLCAux.Text = SelectTransactionParameter(TransactionID.BuildAuxTask, PLCLastModifiedDT)
                lblPLCQualAux.Text = SelectTransactionParameter(TransactionID.QualityAuxTask, PLCLastModifiedDT)
                lblJITAux.Text = SelectTransactionParameter(TransactionID.BuildAuxTask, JITLastModifiedDT)

                'CompScan
                lblPLCCompScan.Text = SelectTransactionParameter(TransactionID.BuildComponentScan, PLCLastModifiedDT)
                lblPLCQualCompScan.Text = SelectTransactionParameter(TransactionID.QualityComponentScan, PLCLastModifiedDT)
                lblJITCompScan.Text = SelectTransactionParameter(TransactionID.BuildComponentScan, JITLastModifiedDT)

                lblPLCCompName.Text = SelectTransactionParameter(TransactionID.ComponentNameData, PLCLastModifiedDT)
                'not used'lblPLCQualCompName.Text = SelectTransactionParameter(TransactionID.ComponentNameData, PLCLastModifiedDT) 
                lblJITCompName.Text = SelectTransactionParameter(TransactionID.ComponentNameData, JITLastModifiedDT)

                lblPLCTool.Text = SelectTransactionParameter(TransactionID.BuildTool, PLCLastModifiedDT)
                lblPLCQualTool.Text = SelectTransactionParameter(TransactionID.QualityTool, PLCLastModifiedDT)
                lblJITTool.Text = SelectTransactionParameter(TransactionID.BuildTool, JITLastModifiedDT)

                lblPLCStation.Text = SelectTransactionParameter(TransactionID.BuildStation, PLCLastModifiedDT)
                lblPLCQualStation.Text = SelectTransactionParameter(TransactionID.QualityStation, PLCLastModifiedDT)
                lblJITStation.Text = SelectTransactionParameter(TransactionID.BuildStation, JITLastModifiedDT)

                lblPLCProdCode.Text = SelectTransactionParameter(TransactionID.ProductComponentCode, PLCLastModifiedDT)
                'not used'lblPLCQualProdCode.Text = SelectTransactionParameter(TransactionID.ProductComponentCode, PLCLastModifiedDT) 
                lblJITProdCode.Text = SelectTransactionParameter(TransactionID.ProductComponentCode, JITLastModifiedDT)

                lblPLCUser.Text = SelectTransactionParameter(TransactionID.UserLogin, PLCLastModifiedDT)
                'not used'lblPLCQualUser.Text = SelectTransactionParameter(TransactionID.UserLogin, PLCLastModifiedDT)
                lblJITUser.Text = SelectTransactionParameter(TransactionID.UserLogin, JITLastModifiedDT)


                'Build Progress
                SetProgressBarValue(hidpbDnldAux, hidInitpbDnldAux, cmdDnldAux, SelectTransactionParameter(TransactionID.BuildAuxTask, DownloadPercentDone))
                SetProgressBarValue(hidpbUpldAux, hidInitpbUpldAux, cmdUpldAux, SelectTransactionParameter(TransactionID.BuildAuxTask, UploadPercentDone))

                SetProgressBarValue(hidpbDnldCompScan, hidInitpbDnldCompScan, cmdDnldCompScan, SelectTransactionParameter(TransactionID.BuildComponentScan, DownloadPercentDone))
                SetProgressBarValue(hidpbUpldCompScan, hidInitpbUpldCompScan, cmdUpldCompScan, SelectTransactionParameter(TransactionID.BuildComponentScan, UploadPercentDone))

                SetProgressBarValue(hidpbDnldCompName, hidInitpbDnldCompName, cmdDnldCompName, SelectTransactionParameter(TransactionID.ComponentNameData, DownloadPercentDone))
                SetProgressBarValue(hidpbUpldCompName, hidInitpbUpldCompName, cmdUpldCompName, SelectTransactionParameter(TransactionID.ComponentNameData, UploadPercentDone))

                SetProgressBarValue(hidpbDnldTool, hidInitpbDnldTool, cmdDnldTool, SelectTransactionParameter(TransactionID.BuildTool, DownloadPercentDone))
                SetProgressBarValue(hidpbUpldTool, hidInitpbUpldTool, cmdUpldTool, SelectTransactionParameter(TransactionID.BuildTool, UploadPercentDone))

                SetProgressBarValue(hidpbDnldStation, hidInitpbDnldStation, cmdDnldStation, SelectTransactionParameter(TransactionID.BuildStation, DownloadPercentDone))
                SetProgressBarValue(hidpbUpldStation, hidInitpbUpldStation, cmdUpldStation, SelectTransactionParameter(TransactionID.BuildStation, UploadPercentDone))

                SetProgressBarValue(hidpbDnldProdCode, hidInitpbDnldProdCode, cmdDnldProdCode, SelectTransactionParameter(TransactionID.ProductComponentCode, DownloadPercentDone))
                SetProgressBarValue(hidpbUpldProdCode, hidInitpbUpldProdCode, cmdUpldProdCode, SelectTransactionParameter(TransactionID.ProductComponentCode, UploadPercentDone))

                SetProgressBarValue(hidpbDnldUser, hidInitpbDnldUser, cmdDnldUser, SelectTransactionParameter(TransactionID.UserLogin, DownloadPercentDone))
                SetProgressBarValue(hidpbUpldUser, hidInitpbUpldUser, cmdUpldUser, SelectTransactionParameter(TransactionID.UserLogin, UploadPercentDone))

                If CBool(hidMultiPLC.Value) = True Then
                    'Quality Progress
                    SetProgressBarValue(hidpbDnldQualAux, hidInitpbDnldQualAux, cmdDnldQualAux, SelectTransactionParameter(TransactionID.QualityAuxTask, DownloadPercentDone))
                    SetProgressBarValue(hidpbUpldQualAux, hidInitpbUpldQualAux, cmdUpldQualAux, SelectTransactionParameter(TransactionID.QualityAuxTask, UploadPercentDone))

                    SetProgressBarValue(hidpbDnldQualCompScan, hidInitpbDnldQualCompScan, cmdDnldQualCompScan, SelectTransactionParameter(TransactionID.QualityComponentScan, DownloadPercentDone))
                    SetProgressBarValue(hidpbUpldQualCompScan, hidInitpbUpldQualCompScan, cmdUpldQualCompScan, SelectTransactionParameter(TransactionID.QualityComponentScan, UploadPercentDone))

                    'SetProgressBarValue(hidpbDnldQualCompName, hidInitpDnldQualCompNameb, cmdDnldQualCompName, SelectTransactionParameter(TransactionID., DownloadPercentDone))
                    'SetProgressBarValue(hidpbUpldQualCompName, hidInitpbUpldQualCompName, cmdUpldQualCompName, SelectTransactionParameter(TransactionID., UploadPercentDone))

                    SetProgressBarValue(hidpbDnldQualTool, hidInitpbDnldQualTool, cmdDnldQualTool, SelectTransactionParameter(TransactionID.QualityTool, DownloadPercentDone))
                    SetProgressBarValue(hidpbUpldQualTool, hidInitpbUpldQualTool, cmdUpldQualTool, SelectTransactionParameter(TransactionID.QualityTool, UploadPercentDone))

                    SetProgressBarValue(hidpbDnldQualStation, hidInitpbDnldQualStation, cmdDnldQualStation, SelectTransactionParameter(TransactionID.QualityStation, DownloadPercentDone))
                    SetProgressBarValue(hidpbUpldQualStation, hidInitpbUpldQualStation, cmdUpldQualStation, SelectTransactionParameter(TransactionID.QualityStation, UploadPercentDone))

                    'SetProgressBarValue(hidpbDnldQualProdCode, hidInitpbDnldQualProdCode, cmdDnldQualProdCode, SelectTransactionParameter(TransactionID., DownloadPercentDone))
                    'SetProgressBarValue(hidpbUpldQualProdCode, hidInitpbUpldQualProdCode, cmdUpldQualProdCode, SelectTransactionParameter(TransactionID., UploadPercentDone))

                    'SetProgressBarValue(hidpbDnldQualUser, hidInitpbDnldQualUser, cmdDnldQualUser, SelectTransactionParameter(TransactionID., DownloadPercentDone))
                    'SetProgressBarValue(hidpbUpldQualUser, hidInitpbUPldQualUser, cmdUpldQualUser, SelectTransactionParameter(TransactionID., UploadPercentDone))
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Function RoundDecimalString(strInput As String) As String
        Dim decResult As Decimal
        Try
            If (Decimal.TryParse(strInput, decResult) = False) Then
                decResult = 0
            Else
                decResult = Math.Round(decResult, 2)
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
        Return decResult.ToString()
    End Function

    Private Sub UpdateTransactionParameters(transactionID As TransactionID, TransactionParameterTypeID As String, TransactionParameterValue As String)

        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)

            Dim status As String = ""
            Dim message As String = ""

            oSqlParameter = New SqlParameter("@TransactionID", SqlDbType.Int)
            oSqlParameter.Value = transactionID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@TransactionParameterTypeID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = TransactionParameterTypeID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@LineNumber", SqlDbType.Int)
            oSqlParameter.Value = CInt(ddLineNumber.SelectedValue)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@TransactionParameterValue", SqlDbType.VarChar, 200)
            oSqlParameter.Value = TransactionParameterValue
            colParameters.Add(oSqlParameter)

            DA.ExecSP("procSG2015UpdateTransParams", colParameters)

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub


    Private Function SelectTransactionParameter(transactionID As TransactionID, TransactionParameterTypeID As String) As String
        Dim strResult As String = ""
        Try
            Dim oSqlParameter As SqlParameter
            Dim colParameters As New List(Of SqlParameter)
            Dim colOutput As List(Of SqlParameter)
            Dim status As String = ""
            Dim message As String = ""

            oSqlParameter = New SqlParameter("@TransactionID", SqlDbType.Int)
            oSqlParameter.Value = transactionID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@TransactionParameterTypeID", SqlDbType.VarChar, 4)
            oSqlParameter.Value = TransactionParameterTypeID
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@LineNumber", SqlDbType.Int)
            oSqlParameter.Value = CInt(ddLineNumber.SelectedValue)
            colParameters.Add(oSqlParameter)

            oSqlParameter = New SqlParameter("@TransactionParameterValue", SqlDbType.VarChar, 200)
            oSqlParameter.Direction = ParameterDirection.Output
            colParameters.Add(oSqlParameter)

            colOutput = DA.ExecSP("procSG2015SelectTransParams", colParameters)

            For Each oParameter In colOutput
                With oParameter
                    If .Direction = ParameterDirection.Output Then
                        strResult = oParameter.Value.ToString()
                        Exit For
                    End If
                End With
            Next

            Select Case (TransactionParameterTypeID)
                Case PLCLastModifiedDT, JITLastModifiedDT
                    If strResult.Trim().Length = 0 Then
                        strResult = "--"
                    End If
                Case DownloadPercentDone, UploadPercentDone
                    If strResult.Trim().Length = 0 Then
                        strResult = "0"
                    End If
                    strResult = RoundDecimalString(strResult)
            End Select

        Catch ex As Exception
            Master.eMsg(ex.ToString())
            strResult = ""
        End Try
        Return strResult
    End Function

    'Private Sub ResetTransactionProgress()
    '    Try
    '        ' why would you want to set all the values to 100?  something must have failed.
    '        '   DA.ExecSQL("Update tblSGTransactionParameters Set TransactionParameterValue = '100' WHERE (TransactionParameterTypeID = '0007' or TransactionParameterTypeID = '0008') AND LineNumber = " & CInt(ddLineNumber.SelectedValue))
    '    Catch ex As Exception
    '        Master.eMsg(ex.ToString())
    '    End Try
    'End Sub

#End Region

End Class