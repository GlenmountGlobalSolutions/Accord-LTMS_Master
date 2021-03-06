<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ComponentCodeConfiguration.aspx.vb" Inherits="LTMS_Master.ComponentCodeConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            CreateDialog_New();
            CreateDialog_Delete();
        }

        function CreateDialog_Delete() {
            var $cmdButton = $('#MainContent_cmdDelete');
            var $dlgDiv = $('#divDialogDelete');

            // add dialog open event
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Copy Dialog
            $dlgDiv.modalDialog({
                control: $cmdButton,
                width: 500
            });
        }

        function CreateDialog_New() {
            var $cmdButton = $('#MainContent_cmdNew');
            var $dlgDiv = $('#divDialogNew');

            // add dialog open event
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Copy Dialog
            $dlgDiv.modalDialog({
                control: $cmdButton,
                width: 500
            });
        }

    </script>
    <style>
        #divMainContent span.ui-icon, #divDialogNew span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 170px;
        }
        #divMainLeftPanel select
        {
            width: 160px;
        }
        #cmdPanel
        {
            width: 160px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:Label ID="Label5" runat="server" CssClass="spanLabel h2">Component Codes:</asp:Label>
        <asp:ListBox ID="lbComponentcodes" runat="server" CssClass="selectListBox" AutoPostBack="True"></asp:ListBox>
        <div id="cmdPanel">
            <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" ToolTip="Add new Component" Text="New"></asp:Button>
            <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save changes" Text="Save"></asp:Button>
            <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" ToolTip="Delete Component" Text="Delete"></asp:Button>
        </div>
    </div>
    <div id="divMainCenterPanel">
        <table>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label2" runat="server" Width="110px" CssClass="spanLabel">Component Code:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="tComponentCode" runat="server" CssClass="textEntry" Width="184px"></GGS:WebInputBox>
                    <asp:Image ID="Image3" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                    <asp:Label ID="lblComponentCode" runat="server" CssClass="spanLabel" EnableViewState="False" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMainRightPanel">
    </div>
    <div id="divDialogNew" title="Add New Component Code">
        <table id="Table1">
            <tr>
                <td style="width: 200px">
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label1" runat="server" Width="150px" CssClass="spanLabel">Component Code:</asp:Label>
                </td>
                <td style="height: 15px">
                    <GGS:WebInputBox ID="tNewComponentCode" runat="server" CssClass="textEntry" Width="184px"></GGS:WebInputBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Component Code">
        <p class="pCenter">
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span><span class="spanLabel">You are about to Delete the selected component code. Do you wish to continue?</span>
        </p>
    </div>
</asp:Content>
