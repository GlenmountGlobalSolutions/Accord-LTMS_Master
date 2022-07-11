Public Class TeardownAndRelease
    Inherits System.Web.UI.Page



#Region "Event Handlers"

    Private Sub TeardownAndRelease_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Try
            EnableControls()

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub

    Private Sub cmdTeardown_Click(sender As Object, e As System.EventArgs) Handles cmdTeardown.Click
        Try
            ' only do anything if they clicked 'YES' on the javascript dialog...
            Dim ShouldContinue As Boolean = (HiddenFlag.Value.ToString().ToUpper() = "TRUE")

            ' grab text from screen...
            Dim TheSSN As String = tbSSN.Text().Trim()

            ' create a SQL string to call our SP with the correct parameters...
            Dim TheStr As String = String.Format("procDoTearDownOrRelease 1, '{0}', ''", TheSSN)
            Dim NumRecords As Long

            If (Not ShouldContinue) Then
                Master.Msg = "Operation Canceled"
            End If

            If (ShouldContinue) Then
                ShouldContinue = ShouldContinue And (TheSSN <> "")

                If (Not ShouldContinue) Then
                    Master.Msg = "Please enter a value in the 'SSN' field."
                End If
            End If

            ' execute the SP...
            If (ShouldContinue) Then
                NumRecords = DA.ExecSQL(TheStr)

                If (NumRecords > 0) Then
                    Master.Msg = "Teardown successful for SSN: " & TheSSN
                Else
                    Master.Msg = "No matching records found for SSN: " & TheSSN
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub

    Private Sub cmdRelease_Click(sender As Object, e As System.EventArgs) Handles cmdRelease.Click
        Try
            ' only do anything if they clicked 'YES' on the javascript dialog...
            Dim ShouldContinue As Boolean = (HiddenFlag.Value.ToString().ToUpper() = "TRUE")

            ' grab text from screen...
            Dim TheComponentSN As String = tbComponentSN.Text().Trim()

            ' create a SQL string to call our SP with the correct parameters...
            Dim TheStr As String = String.Format("procDoTearDownOrRelease 0, '', '{0}'", TheComponentSN)
            Dim NumRecords As Long

            If (Not ShouldContinue) Then
                Master.Msg = "Operation Canceled"
            End If

            If (ShouldContinue) Then
                ShouldContinue = ShouldContinue And (TheComponentSN <> "")

                If (Not ShouldContinue) Then
                    Master.Msg = "Please enter a value in the 'Component SN' field."
                End If
            End If

            ' execute the SP...
            If (ShouldContinue) Then
                NumRecords = DA.ExecSQL(TheStr)

                If (NumRecords > 0) Then
                    Master.Msg = "Release successful for Component SN: " & TheComponentSN
                Else
                    Master.Msg = "No matching records found for Component SN: " & TheComponentSN
                End If
            End If
        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try

    End Sub
#End Region

#Region "Methods"

#End Region

    Private Sub EnableControls()
        Try
            cmdTeardown.Enabled = True
            cmdRelease.Enabled = True

        Catch ex As Exception
            Master.eMsg(ex.ToString())
        End Try
    End Sub



End Class