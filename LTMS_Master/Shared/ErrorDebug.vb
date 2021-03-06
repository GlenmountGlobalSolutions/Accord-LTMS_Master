Imports System.Data.SqlClient

Module ErrorDebug

    Public Function LogErr(ByVal strAction As String, ByVal objPage As Page, Optional ByVal p_strAdditionalInfo As String = "") As String
        Dim strPageName As String = ""
        Dim strErrorDesc As String = ""
        Dim parameters As New List(Of SqlParameter)
        Dim strUserFirstLastName As String = "No User Logged In"

        Try
            strErrorDesc = Err.Number.ToString() & " - " & Err.Description & " Additional Info: " & p_strAdditionalInfo
            strErrorDesc = strErrorDesc.Replace(vbCrLf, "")
            strPageName = objPage.Page.Request.Url.ToString
            strPageName = Right(strPageName, Len(strPageName) - InStrRev(strPageName, "/"))

            If (objPage.Session("UserFirstLastName") IsNot Nothing) Then
                strUserFirstLastName = objPage.Session("UserFirstLastName").ToString()
            End If

            With parameters
                .Add(New SqlParameter("@UserName", strUserFirstLastName))
                .Add(New SqlParameter("@ApplicationID", ConfigurationManager.AppSettings("ApplicationID")))
                .Add(New SqlParameter("@ErrorID", Err.Number.ToString()))
                .Add(New SqlParameter("@ErrorMsg", strPageName & ": " & Err.Description))
                .Add(New SqlParameter("@ProcName", System.Reflection.Assembly.GetExecutingAssembly.GetName.Name & "-" & strPageName))   'This is AppTitle
                .Add(New SqlParameter("@ErrSource", Err.Source))
                .Add(New SqlParameter("@ErrDescription", "Runtime Error in " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Name & " on page " & strPageName & " LINE::" & Err.Erl().ToString()))
                .Add(New SqlParameter("@CustomError", DBNull.Value))
                .Add(New SqlParameter("@AdditionalInfo", strErrorDesc))
                .Add(New SqlParameter("@UserHostAddress", objPage.Request.UserHostAddress.ToString()))
                .Add(New SqlParameter("@ActionPerformed", strAction))
            End With
            DA.ExecSP("[dbo].[procInsertErrorHistory]", parameters)

        Finally
            LogErr = strErrorDesc
        End Try
    End Function

    Public Sub LogTrc(ByVal action As String, ByRef objPage As Page, Optional ByVal desc As String = "")
        Dim strPageName As String
        'Dim strLogInfo As String
        Dim blnDebugToFile, blnDebugToDB As Boolean
        Dim parameters As New List(Of SqlParameter)
        Dim strUserFirstLastName As String = "No User Logged In"

        Try

            strPageName = objPage.Request.Url.ToString
            strPageName = Right(strPageName, Len(strPageName) - InStrRev(strPageName, "/"))

            blnDebugToFile = CBool(BizLayer.GetApplicationParameterValue("0041"))
            blnDebugToDB = CBool(BizLayer.GetApplicationParameterValue("0042"))

            If (objPage.Session("UserFirstLastName") IsNot Nothing) Then
                strUserFirstLastName = objPage.Session("UserFirstLastName").ToString()
            End If

            If blnDebugToDB Then

                With parameters
                    .Add(New SqlParameter("@UserName", strUserFirstLastName))
                    .Add(New SqlParameter("@ApplicationID", "0001"))
                    .Add(New SqlParameter("@UserHostAddress", Utility.GetClientIPAddress()))
                    .Add(New SqlParameter("@ScreenName", strPageName.ToString()))
                    .Add(New SqlParameter("@ActionPerformed", action.ToString()))
                    .Add(New SqlParameter("@Description", desc.ToString()))
                End With

                DA.ExecSP("[dbo].[procInsertDebug]", parameters)
            End If

            'If blnDebugToFile Then
            '    strLogInfo = "(" & Format$(Now(), "MM/DD/YYYY hh:mm:ss") & ") " & action & ": " & desc

            '    .LogFilePath = BizLayer.GetApplicationParameterValue("0046")
            '    .LogFilePrefix = BizLayer.GetApplicationParameterValue("0045")
            '    .LogFileSaveInterval = BizLayer.GetApplicationParameterValue("0047")

            '    .SaveToFile(strLogInfo)
            'End If

        Finally

        End Try

    End Sub
End Module
