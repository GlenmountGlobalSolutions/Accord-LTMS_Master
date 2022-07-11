<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ParameterConfiguration.aspx.vb" Inherits="LTMS_Master.ParameterConfiguration"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemConfiguration.js") %>"></script>
    <script type="text/javascript">
        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();

            CreateDialog_New();
            CreateDialog_Delete();
        }

        function CreateDialog_Delete() {
            var $cmdButton = $('#MainContent_cmdDelete');
            var $dlgDiv = $('#divDialogDeleteValue');

            // add dialog open event
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Copy Dialog
            $dlgDiv.modalDialog({
                control: $cmdButton,
                width: 320
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
                width: 350
            });
        }

    </script>
    <style type="text/css">
        #divMainContent span.ui-icon-triangle-1-e, #divDialogCopy span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 380px;
        }
        #divMainLeftPanel select
        {
            width: 300px;
        }
        #cmdPanel
        {
            width: 300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label9" runat="server" CssClass="h2 spanLabel">Product Type:</asp:Label>
                <GGS:WebDropDownList ID="ddlProductType" runat="server" AutoPostBack="True" CssClass="selectDropDown">
                </GGS:WebDropDownList>
                <asp:Label ID="Label3" runat="server" CssClass="h2 spanLabel">Product Parameters:</asp:Label>
                <table style="text-align: center; margin-left: -3px;">
                    <tr>
                        <td>
                            <asp:ListBox ID="lbProdParams" runat="server" AutoPostBack="True" CssClass="selectListBox">
                            </asp:ListBox>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibUP" runat="server" Height="30px" ImageUrl="~/Images/Arrows/ArrowUp.png"
                                            CssClass="sortButton" Enabled="False" Width="34px"></asp:ImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 10px; padding-bottom: 14px;">
                                        <asp:Label ID="Label1" runat="server" CssClass="spanLabel">Move</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibDown" runat="server" Height="30px" ImageUrl="~/Images/Arrows/ArrowDown.png"
                                            CssClass="sortButton" Enabled="False" EnableViewState="True" Width="34px"></asp:ImageButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" Height="28px" Width="75px"
                        ToolTip="Add New Product" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" Height="28px" Width="75px"
                        ToolTip="Save Product changes" Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" Height="28px" Width="75px"
                        ToolTip="Delete Product" Text="Delete"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel" style="width: 550px;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table id="Table2" border="0">
                    <tr>
                        <td style="width: 10px">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 129px">
                            <asp:Label ID="lblDescription" runat="server" CssClass="spanLabel" Width="80px">Description:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbParamDesc" runat="server" CssClass="textEntry" Text="<%# ParameterDescription(tbParamDesc) %>"
                                Width="240">
                            </GGS:WebInputBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 129px">
                            <asp:Label ID="lblDefaultValue" runat="server" CssClass="spanLabel">Default Value:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbDefValue" runat="server" CssClass="textEntry" Text="<%# DefaultValue(tbDefValue) %>"
                                Width="240">
                            </GGS:WebInputBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px; height: 23px">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 129px; height: 23px">
                            <asp:Label ID="lblSelectionList" runat="server" CssClass="spanLabel">Selection List:</asp:Label>
                        </td>
                        <td style="height: 23px">
                            <GGS:WebDropDownList ID="dbSelectionList" runat="server" CssClass="selectDropDown"
                                Width="247px">
                            </GGS:WebDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px; height: 30px">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 129px; height: 30px">
                            <asp:Label ID="Label2" runat="server" CssClass="spanLabel">Product Type:</asp:Label>
                        </td>
                        <td style="height: 30px">
                            <GGS:WebDropDownList ID="ddProductTYpe" runat="server" CssClass="selectDropDown"
                                Width="247px" Enabled="False">
                            </GGS:WebDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10px">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 129px">
                            <asp:Label ID="Label4" runat="server" CssClass="spanLabel">Character Swap?</asp:Label>
                        </td>
                        <td>
                            <GGS:WebDropDownList ID="ddCharSwap" runat="server" CssClass="selectDropDown" Width="247px">
                            </GGS:WebDropDownList>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="New Product Parameter">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table1">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="lblNewProdParameterID" runat="server" CssClass="spanLabel">New Parameter:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="txtNewParamDesc" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDeleteValue" title="Confirm Deletion">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            <span class="spanLabel">You are about to Delete the selected item.</span>
        </p>
        <p class="pCenter" style="margin-left: 30px;">
            <span class="spanLabel">Do you wish to continue?</span>
        </p>
    </div>
</asp:Content>
