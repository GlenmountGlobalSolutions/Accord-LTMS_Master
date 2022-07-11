<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ComponentConfiguration.aspx.vb" Inherits="LTMS_Master.ComponentConfiguration" %>

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
            $dlgDiv.deleteDialog({ control: $cmdButton });
        }

        function CreateDialog_New() {
            var $cmdButton = $('#MainContent_cmdNew');
            var $dlgDiv = $('#divDialogNew');

            // add dialog open event
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Copy Dialog
            $dlgDiv.modalDialog({
                control: $cmdButton,
                width: 450,
                validationFunction: function () { return ValidateDialog_New(); }
            });
        }

        function ValidateDialog_New() {
            var bValid = true;
            try {
                bValid = checkDropDownList($('#MainContent_wddNewOrderParam'), 'Select an Order Parameter.', $('#helpNewOrderParameter')) && bValid;
                bValid = checkText($('#MainContent_wibNewDescription'), 'Enter a Description for the new Component.', $('#helpNewDescription')) && bValid;
                bValid = checkNumericRange($('#MainContent_wibNewComponentID'), 0, 1000, 'Enter a new Component ID.', $('#helpNewComponentID')) && bValid;
            } catch (Err) {
                alert(Err);
            }
            return bValid;
        }

    </script>
    <style>
        #divMainContent span.ui-icon-triangle-1-e, #divDialogNew span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 190px;
        }
        #divMainLeftPanel select
        {
            width: 180px;
        }
        #cmdPanel
        {
            width: 180px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label8" runat="server" CssClass="spanLabel h2">Components:</asp:Label>
                <asp:ListBox ID="lbComponents" runat="server" AutoPostBack="True" CssClass="selectListBox"></asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" ToolTip="New item" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save changes" Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" ToolTip="Delete selected item" Text="Delete"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label2" runat="server" Width="110px" CssClass="spanLabel">Component ID:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tCompID" runat="server" Width="183px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgCheckCompID" runat="server" ImageUrl="~/Images/check.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestCompID" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label1" runat="server" Width="110px" CssClass="spanLabel">Description:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tDescription" runat="server" Width="183px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgCheckDescription" runat="server" ImageUrl="~/Images/check.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestDescription" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label3" runat="server" Width="110px" CssClass="spanLabel">Mask:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tMask" runat="server" Width="183px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgCheckMask" runat="server" ImageUrl="~/Images/check.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestMask" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label4" runat="server" Width="110px" CssClass="spanLabel">Order Parameter:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebDropDownList ID="ddOrderParam" runat="server" Width="190px" CssClass="selectDropDown">
                            </GGS:WebDropDownList>
                            <asp:Image ID="imgCheckOrderParam" runat="server" ImageUrl="~/Images/check.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestOrderParam" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainRightPanel">
    </div>
    <div id="divDialogNew" title="New Component Configuration">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table class="tableCenter">
            <tr>
                <td style="width: 200px;">
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label9" runat="server" Width="150px" CssClass="spanLabel">Component ID:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewComponentID" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
                <td>
                    <div id="helpNewComponentID" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label10" runat="server" Width="150px" CssClass="spanLabel">Description:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewDescription" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
                <td>
                    <div id="helpNewDescription" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label11" runat="server" Width="150px" CssClass="spanLabel">Mask:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewMask" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label12" runat="server" Width="150px" CssClass="spanLabel">Order Parameter:</asp:Label>
                </td>
                <td>
                    <GGS:WebDropDownList ID="wddNewOrderParam" runat="server" Width="190px" CssClass="selectDropDown">
                    </GGS:WebDropDownList>
                </td>
                <td>
                    <div id="helpNewOrderParameter" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Component Configuration">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to delete the selected component. Do you wish to continue?
        </p>
    </div>
</asp:Content>
