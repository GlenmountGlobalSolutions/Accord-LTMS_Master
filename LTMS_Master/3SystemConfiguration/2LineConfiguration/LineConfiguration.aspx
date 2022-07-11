<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LineConfiguration.aspx.vb" Inherits="LTMS_Master.LineConfiguration" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemConfiguration.js") %>"></script>
    <script type="text/javascript">
            var $cmdButton_New;
            var $dlgDiv_New;
            var $txtNewLineName;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            CacheControls();
            CreateDialog_NewLine();
        }

        function CacheControls() {
            $cmdButton_New = $('#MainContent_cmdNew');
            $dlgDiv_New = $('#divDialogNew');
            $txtNewLineName = $('#MainContent_txtNewLineName');
        }

        function CreateDialog_NewLine() {

            $cmdButton_New.click(function () { $dlgDiv_New.dialog('open'); return false; });

            $dlgDiv_New.modalDialog({
                control: $cmdButton_New,
                width:360,
                validationFunction: function () { return ValidateDialog_NewLine(); }
            });
        }
        function ValidateDialog_NewLine() {
            var bValid = true;
            bValid = checkText($txtNewLineName, "Please Enter a Line Name. This is a required field.", $("#helpLineName")) && bValid;
            return bValid;
        }
    </script>
    <style type="text/css">
        #divDialogNew span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 236px;
        }
        #divMainLeftPanel select
        {
            width: 220px;
        }
        .selectListBox
        {
            height: 300px;
            width: 220px;
            float: left;
        }
        #cmdPanel
        {
            width: 220px;
        }
        #divMainContent span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label3" runat="server" CssClass="h2 spanLabel">Lines:</asp:Label>
                <asp:ListBox ID="lbLines" runat="server" AutoPostBack="true" CssClass="selectListBox"></asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="Add New Line" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="Save Line changes" Text="Save"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="lineProperties">
                    <table>
                        <tr>
                            <td style="width: 150px;">
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label2" runat="server" Width="80px" CssClass="spanLabel">Line Name:</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="txtLineName" runat="server" Width="220px" CssClass="textEntry"></GGS:WebInputBox>
                                <asp:Image ID="Image2" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                                <asp:Label ID="lblLineNameHelp" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160px;">
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label1" runat="server" Width="130px" CssClass="spanLabel">Wire Harness File Path:</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="txtEDSFilePath" runat="server" Width="220px" CssClass="textEntry"></GGS:WebInputBox>
                                <asp:Image ID="Image1" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                                <asp:Label ID="lblHelpEDSFilePath" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label9" runat="server" Width="80px" CssClass="spanLabel">Line ID:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLineID" runat="server" Width="30px" CssClass="spanReadOnly"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="New Line">
        <p class="validationHints h4">All form fields are required.</p>
        <table class="tableCenter">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" CssClass="spanLabel">Line Name:</asp:Label>
                    <asp:TextBox ID="txtNewLineName" runat="server" Width="134px" CssClass="textEntry NoColorOnChange" MaxLength="50" ></asp:TextBox>
                </td>
                <td>
                    <div id="helpLineName" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" CssClass="spanLabel">EDS File Path:</asp:Label>
                    <asp:TextBox ID="txtNewEDSFilePath" runat="server" Width="134px" CssClass="textEntry NoColorOnChange" MaxLength="50" ></asp:TextBox>
                </td>
                <td>
                    <div id="Div1" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
