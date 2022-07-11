<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StyleGroupConfiguration.aspx.vb" Inherits="LTMS_Master.StyleGroupConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.alphanum.js") %>"></script>
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            MakeIndexNumericOnly();

            CreateDialog_New();
            CreateDialog_Delete();
        }

        function MakeIndexNumericOnly() {
            // make the textboxes accept numeric only
            $('[id*="StyleGroupID"]').numeric({
                allowMinus: false,
                allowThouSep: false,
                allowDecSep: false
            });
        }


        function CreateDialog_New() {
            var $cmdNew = $('#MainContent_cmdNew');
            var $dlgNew = $('#divDialogNew');

            $cmdNew.click(function () { $dlgNew.dialog('open'); return false; });           // add event to button

            $dlgNew.modalDialog({                                                           // Add New Dialog
                control: $cmdNew,                                                           //$cmdNew is the button that has code in the aspx.vb and will perform the postback
                width: 370,                                                                 // default width is 300  see jQuery UI documentation for more dialog options
                validationFunction: function () { return ValidateNewStyleGroup(); }
            });
        }

        function ValidateNewStyleGroup() {
            var bValid = false;
            try {
                bValid = checkText($('#MainContent_txtNewStyleGroupName'), 'Enter an Index for the new Style Group.', $('#helpValue'));
                bValid = checkText($('#MainContent_txtNewStyleGroupID'), 'Enter a Name for the new Style Group.', $('#helpDescription')) && bValid;

            } catch (err) {
                alert(err);
            }
            return bValid;
        }

        function CreateDialog_Delete() {
            var $cmdDelete = $('#MainContent_cmdDelete');
            var $dlgDelete = $('#divDialogDelete');

            $cmdDelete.click(function () { $dlgDelete.dialog('open'); return false; }); // add event to button

            $dlgDelete.deleteDialog({ control: $cmdDelete });                           // Add Delete Dialog
        }
    

    </script>
    <style type="text/css">
        #divMainCenterPanel span.ui-icon-triangle-1-e, #divDialogNew span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 180px;
        }
        #divMainLeftPanel select
        {
            width: 170px;
        }
        #cmdPanel
        {
            width: 170px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel2" runat="server" DefaultButton="cmdSave">
                    <asp:Label ID="Label11" runat="server" CssClass="spanLabel h2">Line Number:</asp:Label>
                    <asp:DropDownList ID="ddlLineNum" runat="server" AutoPostBack="True" CssClass="selectDropDown">
                    </asp:DropDownList>
                    <asp:Label ID="Label2" runat="server" CssClass="spanLabel h2">Style Groups:</asp:Label>
                    <asp:ListBox ID="lbStyleGroups" runat="server" AutoPostBack="True" CssClass='selectListBox'></asp:ListBox>
                    <div id="cmdPanel">
                        <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" ToolTip="Add Parameter" Text="New" CommandArgument="New"></asp:Button>
                        <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save Parameter" Text="Save" CommandArgument="Save"></asp:Button>
                        <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" ToolTip="Delete Parameter" Text="Delete" CommandArgument="Delete"></asp:Button>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" DefaultButton="cmdSave">
                    <table id="Table3">
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="lblDesc" runat="server" CssClass="spanLabel">Style Group Index:</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="txtStyleGroupID" runat="server" Width="260px" CssClass="textEntry"></GGS:WebInputBox>
                            </td>
                            <td>
                                <asp:Image ID="imgIndex" runat="server" Width="18px" Height="18px" ImageUrl="~/images/check.gif" Visible="false"></asp:Image>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="lblName" runat="server" CssClass="spanLabel">Style Group Name:</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="txtStyleGroupName" runat="server" Width="260px" CssClass="textEntry"></GGS:WebInputBox>
                            </td>
                            <td>
                                <asp:Image ID="imgName" runat="server" Width="18px" Height="18px" ImageUrl="~/images/check.gif" Visible="false"></asp:Image>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="Add New Style Group">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table1" class="tableCenter">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="lblDescription" runat="server" CssClass="spanLabel">Style Group Index:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewStyleGroupID" TabIndex="2" runat="server" CssClass="textEntry" Width="180px"></asp:TextBox>
                </td>
                <td>
                    <div id="helpDescription" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="lblLabelText" runat="server" CssClass="spanLabel">Style Group Name:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewStyleGroupName" TabIndex="3" runat="server" CssClass="textEntry" Width="180px"></asp:TextBox>
                </td>
                <td>
                    <div id="helpValue" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Style Group">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to Delete the selected Style Group. Do you wish to continue?
        </p>
    </div>
</asp:Content>
