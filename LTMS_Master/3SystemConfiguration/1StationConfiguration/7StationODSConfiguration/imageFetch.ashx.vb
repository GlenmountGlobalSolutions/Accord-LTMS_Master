Imports System.Web
Imports System.Drawing.Imaging
Imports System.Data.SqlClient

Public Class imageFetch
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim dr As DataRow = Nothing
        Dim ds As DataSet = Nothing
        Dim file As String
        Dim impersonateDomain As String
        Dim impersonateUser As String
        Dim impersonatePass As String
        Dim parameters As New List(Of SqlParameter)

        Dim lineID As String = context.Request.QueryString("lineID")
        Dim stationID As String = context.Request.QueryString("stationID")
        Dim cfgID As String = context.Request.QueryString("cfgID")
        Dim mcrID As String = context.Request.QueryString("mcrID")
        Dim modelCode As String = context.Request.QueryString("modelCode")

        parameters.Add(New SqlParameter("@LineID", lineID))
        parameters.Add(New SqlParameter("@StationID", stationID))
        parameters.Add(New SqlParameter("@ConfigurationID", cfgID))
        parameters.Add(New SqlParameter("@MasterChangeRequestID", mcrID))
        parameters.Add(New SqlParameter("@ModelCode", modelCode))

        ds = DA.GetDataSet("[ods].[procGetInstructionImages]", parameters, "")

        If (DA.IsDataSetNotEmpty(ds)) Then
            dr = ds.Tables(0).Rows(0)
            file = dr.Item("ImagePath").ToString()
            context.Response.ContentType = "image/jpg"

            impersonateUser = BizLayer.GetApplicationParameterValue("2213", "0050")

            If (impersonateUser.ToString().Length > 0) Then
                impersonateDomain = BizLayer.GetApplicationParameterValue("2215", "0050")
                impersonatePass = BizLayer.GetApplicationParameterValue("2214", "0050")
                Using New Impersonation(impersonateDomain, impersonateUser, impersonatePass)
                    'context.Response.WriteFile(file)
                    context.Response.BinaryWrite(System.IO.File.ReadAllBytes(file))
                End Using
            Else
                context.Response.WriteFile(file)
            End If
        End If
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class