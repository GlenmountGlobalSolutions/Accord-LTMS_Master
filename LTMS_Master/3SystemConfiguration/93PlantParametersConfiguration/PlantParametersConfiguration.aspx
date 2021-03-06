<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PlantParametersConfiguration.aspx.vb" Inherits="LTMS_Master.PlantParametersConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            CreateDialog_NewParameter();
            CreateDialog_DeleteParameter();
        }

        function CreateDialog_NewParameter() {
            var $cmdNew = $('#MainContent_cmdNew');
            var $dlgNew = $('#divDialogNew');

            $cmdNew.click(function () { $dlgNew.dialog('open'); return false; });           // add event to button

            $dlgNew.modalDialog({                                                           // Add New Dialog
                control: $cmdNew,                                                           //$cmdNew is the button that has code in the aspx.vb and will perform the postback
                width: 360,                                                                 // default width is 300  see jQuery UI documentation for more dialog options
                validationFunction: function () { return ValidateDialog_NewParameter(); }
            });
        }

        function ValidateDialog_NewParameter() {
            var bValid = false;
            try {
                bValid = checkText($('#MainContent_txtNewValue'), 'Enter a Value for the new Parameter.', $('#helpValue'));
                bValid = checkText($('#MainContent_txtNewDescription'), 'Enter a Description for the new Parameter.', $('#helpDescription')) && bValid;

            } catch (err) {
                alert(err);
            }
            return bValid;
        }

        function CreateDialog_DeleteParameter() {
            var $cmdDelete = $('#MainContent_cmdDelete');
            var $dlgDelete = $('#divDialogDelete');
            $cmdDelete.click(function () { $dlgDelete.dialog('open'); return false; });
            $dlgDelete.deleteDialog({ control: $cmdDelete, width: 500 });
        }

    </script>
    <style type="text/css">
        input.textEntry
        {
            width: 200px;
        }
        
        #divMainLeftPanel, #divMainLeftPanel select
        {
            width: 270px;
        }
        
        #divMainCenterPanel span.ui-icon-triangle-1-e, #divDialogNew span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
        #cmdPanel
        {
            margin-left: auto;
            margin-right: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <span class="spanLabel h2">Plant Parameters</span>
                <asp:ListBox ID="lbParams" runat="server" AutoPostBack="True" CssClass='selectListBox'></asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" ToolTip="Add Parameter" Text="New" CommandArgument="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save Parameter" Text="Save" CommandArgument="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" ToolTip="Delete Parameter" Text="Delete" CommandArgument="Delete"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" DefaultButton="cmdSave">
                    <table id="Table2">
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="lblDesc" runat="server" CssClass="spanLabel">Description</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="txtDEscription" runat="server" CssClass="textEntry"></GGS:WebInputBox>
                            </td>
                            <td>
                                <asp:Image ID="imgDesc" runat="server" Width="18px" Height="18px" ImageUrl="~/images/check.gif" Visible="false"></asp:Image>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="lblValue" runat="server" CssClass="spanLabel">Value</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="txtValue" runat="server" CssClass="textEntry"></GGS:WebInputBox>
                            </td>
                            <td>
                                <asp:Image ID="imgValue" runat="server" Width="18px" Height="18px" ImageUrl="~/images/check.gif" Visible="false"></asp:Image>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="Add New Plant Parameter">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table1" class="tableCenter">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="lblDescription" runat="server" CssClass="spanLabel">Description</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewDescription" TabIndex="2" runat="server" CssClass="textEntry"></asp:TextBox>
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
                    <asp:Label ID="lblLabelText" runat="server" CssClass="spanLabel">Value</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewValue" TabIndex="3" runat="server" CssClass="textEntry"></asp:TextBox>
                </td>
                <td>
                    <div id="helpValue" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Plant Parameter">
        <div class="ui-corner-all" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter h3" style="margin-left: 30px;">
            You are about to Delete the selected Plant Parameter.
            <br />
            <br />
            Do you wish to continue?
        </p>
    </div>
</asp:Content>
