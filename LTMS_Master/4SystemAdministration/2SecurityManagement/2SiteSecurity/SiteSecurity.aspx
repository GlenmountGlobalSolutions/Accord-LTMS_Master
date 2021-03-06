<%@ Page Title="Site Security" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SiteSecurity.aspx.vb" Inherits="LTMS_Master.SiteSecurity" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            RemoveTreeViewSkipLinks();

            AddDialog_New();

            AddDilaog_Delete();
        }

        function AddDialog_New() {
            var $cmdNew = $('#MainContent_cmdNew');
            var $dlgNew = $('#divDlgNewControl');

            $cmdNew.click(function () { $dlgNew.dialog('open'); return false; });

            $dlgNew.modalDialog({
                control: $cmdNew,
                width: 360,
                validationFunction: function () { return ValidateDialog_NewControl(); }
            });
        }

        function ValidateDialog_NewControl() {
            var bValid = checkLength($("#MainContent_txtCtrlName"), "Control Name", 1, 80, $("#helpCtrlName"));
            bValid = bValid && checkCheckBoxListCount($("#testChecked :checked"), 1, "Please select at least 1 checkbox.", $("#helpUserType"));
            return bValid;
        }

        function AddDilaog_Delete() {
            var $cmdDelete = $('#MainContent_cmdDelete');
            var $dlgDelete = $('#divDialogDelete');

            $cmdDelete.click(function () { $dlgDelete.dialog('open'); return false; });
            $dlgDelete.deleteDialog({ control: $cmdDelete, width: 360 });

        }

    </script>
    <style type="text/css">
        #divMainLeftPanel
        {
            margin-right: 12px;
            width: 350px;
        }
        #divMainCenterPanel
        {
            width: 300px;
        }
        
        #divMainTopPanel
        {
            margin-bottom:30px;
        }
        
        #divMainCenterPanel span.ui-icon-triangle-1-e, #divDialogNew span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
        #MainContent_treeDirectory a
        {
            margin-left: 5px;
        }
        .selectListBox
        {
            height: 232px;
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divMainLeftPanel" class="divBorder">
                <asp:Label ID="Label1" runat="server" CssClass="h2">Site Directory:</asp:Label>
                <asp:Panel ID="pnlTreeview" runat="server" ScrollBars="Auto" BorderWidth="0">
                    <asp:TreeView ID="treeDirectory" runat="server" BorderColor="GhostWhite" ShowLines="False" ShowExpandCollapse="True" RootNodeStyle-ImageUrl="~/Images/TreeView/dir.gif" ParentNodeStyle-ImageUrl="~/Images/TreeView/dir.gif" LeafNodeStyle-ImageUrl="~/Images/TreeView/aspxdoc.gif" RootNodeStyle-HorizontalPadding="0" RootNodeStyle-VerticalPadding="0" ParentNodeStyle-HorizontalPadding="0" ParentNodeStyle-VerticalPadding="0" LeafNodeStyle-HorizontalPadding="0" LeafNodeStyle-VerticalPadding="0" ExpandDepth="1" Height="400px" ShowCheckBoxes="None">
                        <LeafNodeStyle HorizontalPadding="0px" ImageUrl="~/Images/TreeView/aspxdoc.gif" VerticalPadding="0px"></LeafNodeStyle>
                        <ParentNodeStyle HorizontalPadding="0px" ImageUrl="~/Images/TreeView/dir.gif" VerticalPadding="0px"></ParentNodeStyle>
                        <RootNodeStyle HorizontalPadding="0px" ImageUrl="~/Images/TreeView/dir.gif" VerticalPadding="0px"></RootNodeStyle>
                        <SelectedNodeStyle Font-Bold="True" />
                    </asp:TreeView>
                </asp:Panel>
            </div>
            <div id="divMainTopPanel">
                <asp:Label ID="lblCurrent" runat="server" CssClass="spanLanbel h2">Current Selection: </asp:Label>
                <asp:HyperLink ID="hlPage" runat="server" CssClass="h2"></asp:HyperLink>
                <asp:Image ID="imLocked" runat="server" Width="26px" Height="28px" Visible="False" Style="vertical-align: middle; margin-left: 6px;"></asp:Image>
            </div>
            <div id="divMainCenterPanel">
                <table>
                    <tr>
                        <td style="height:20px; vertical-align:top;">
                            <asp:Label ID="Label3" runat="server" CssClass="spanLanbel h2">Allow Menu/Page Access:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:10px;">
                            <asp:CheckBoxList ID="cblistSiteAccess" runat="server" RepeatLayout="Flow" CssClass="selectCheckBox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="cmdSecure" runat="server" CssClass="inputButton" Text="Save"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMainRightPanel">
                <table >
                    <tr>
                        <td colspan="2"  style="height:20px; vertical-align:top;">
                            <asp:Label ID="LABEL2" runat="server" CssClass="spanLanbel h2"> Button / Flag Access:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="lbButtons" runat="server" AutoPostBack="True" CssClass="selectListBox" />
                        </td>
                        <td style="vertical-align:top;">
                            <asp:CheckBoxList ID="cblistButtonAccess" runat="server" RepeatLayout="Flow" CssClass="selectCheckBox" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" Text="New"></asp:Button>
                            <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" Text="Save"></asp:Button>
                            <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" Text="Delete"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDlgNewControl" title="New Security Access">
        <p class="validationHints">
            All form fields are required.</p>
        <table id="Table1" class="tableCenter">
            <tr>
                <td style="text-align: left">
                    <label class="spanLabel h3">
                        Control Name:</label>
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtCtrlName" runat="server" Width="166px" CssClass="textEntry"></asp:TextBox>
                </td>
                <td>
                    <div id="helpCtrlName" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left;">
                    <label class="spanLabel h3">
                        Allow Access:</label>
                </td>
                <td style="vertical-align: top; text-align: left;">
                    <div id="testChecked">
                        <asp:CheckBoxList ID="cblistUserTypes" runat="server" CssClass="selectCheckBox" RepeatLayout="Flow" />
                    </div>
                </td>
                <td style="vertical-align: top">
                    <div id="helpUserType" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Selected Item">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            <span class="spanLabel">You are about to Delete the selected item.<br />
                Do you wish to continue?</span>
        </p>
    </div>
</asp:Content>
