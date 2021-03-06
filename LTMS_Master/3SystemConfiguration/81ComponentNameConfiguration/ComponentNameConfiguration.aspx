<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ComponentNameConfiguration.aspx.vb" Inherits="LTMS_Master.ComponentNameConfiguration" %>

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
                control: $cmdButton
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
    <style type="text/css">
        #divMainContent span.ui-icon, #divDialogNew span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        .padTop
        {
            margin-top: 10px;
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
                <asp:Label ID="Label9" runat="server" CssClass="spanLabel h2">Line Number:</asp:Label>
                <GGS:WebDropDownList ID="ddLineNum" runat="server" AutoPostBack="True" CssClass="selectDropDown">
                </GGS:WebDropDownList>
                <asp:Label ID="Label10" runat="server" CssClass="spanLabel h2">Component Names:</asp:Label>
                <asp:ListBox ID="lbComponentNames" runat="server" AutoPostBack="True" CssClass="selectListBox"></asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" ToolTip="Add New Component Name" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save Component Changes" Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" ToolTip="Delete Component Name" Text="Delete"></asp:Button>
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
                            <asp:Label ID="Label5" runat="server" Width="120px" CssClass="spanLabel">Component Index:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tIndex" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgIndex" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label8" runat="server" Width="120px" CssClass="spanLabel">Component Name:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tName" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgName" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label2" runat="server" Width="120px" CssClass="spanLabel">Mask 1 Start:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tMask1Start" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgMask1Start" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label3" runat="server" Width="120px" CssClass="spanLabel">Mask 1 Length:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tMask1Length" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgMask1Length" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label6" runat="server" Width="120px" CssClass="spanLabel">Mask 2 Start:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tMask2Start" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgMask2Start" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label7" runat="server" Width="120px" CssClass="spanLabel">Mask 2 Length:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tMask2Length" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgMask2Length" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label1" runat="server" Width="120px" CssClass="spanLabel">Fails:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="txtFails" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgFails" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="Add New Component">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table class="tableCenter">
            <tr>
                <td style="width: 200px;">
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label14" runat="server" Width="150px" CssClass="spanLabel">Component Index:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewIndex" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label15" runat="server" Width="150px" CssClass="spanLabel">Component Name:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewName" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label16" runat="server" Width="150px" CssClass="spanLabel">Mask 1 Start:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewMask1Start" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label17" runat="server" Width="150px" CssClass="spanLabel">Mask 1 Length:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewMask1Length" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label18" runat="server" Width="150px" CssClass="spanLabel">Mask 2 Start:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewMask2Start" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label19" runat="server" Width="150px" CssClass="spanLabel">Mask 2 Length:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="wibNewMask2Length" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Component">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to Delete the selected component. Do you wish to continue?
        </p>
    </div>
</asp:Content>
