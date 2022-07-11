<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="Reports.aspx.vb" Inherits="LTMS_Master.Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {

            var $ctl04 = $('#ctl00_MainContent_ReportViewer1_ctl04');
            var $ctl05 = $('#ctl00_MainContent_ReportViewer1_ctl05');

            //Check if there is a parameter list section and apply styles to the parameter list and the navigationBar
            if (($get('ctl00_MainContent_ReportViewer1_ctl04') != null) &&
                ($get('ctl00_MainContent_ReportViewer1_ctl04').control != null)) {
                $ctl04.addClass('reportViewer_parameterList');
                $ctl05.addClass('reportViewer_navigationBar');

            } else {
                // Ther are no parameters so apply style to the navigation bar only    
                $ctl05.addClass('reportViewer_navigationBar_noParameters');
            }

            $(':text').addClass('textEntry');
            $('#ctl00_MainContent_ReportViewer1_ctl04_ctl05').addClass('ReportViewer1_radioButton');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="reportViewer">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Remote" Height="100%"
            Width="100%" Font-Names="Tahoma" ShowCredentialPrompts="True" BackColor="#DDDDDD"
            SplitterBackColor="#CCCCCC">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
