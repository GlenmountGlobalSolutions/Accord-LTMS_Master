<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ProductConfiguration.aspx.vb" Inherits="LTMS_Master.ProductConfiguration"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemConfiguration.js") %>"></script>
    <script type="text/javascript">
        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();

            CreateDialog_New();
            CreateDialog_Delete();
            CreateDialog_Rename();
            CreateDialog_Copy();
        }

        function CreateDialog_Copy() {
            var $cmdButton = $('#MainContent_cmdCopy');
            var $dlgDiv = $('#divDialogCopy');

            // add dialog open event
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Copy Dialog
            $dlgDiv.modalDialog({
                control: $cmdButton,
                width: 400
            });
        }

        function CreateDialog_Rename() {
            var $cmdButton = $('#MainContent_cmdRename');
            var $dlgDiv = $('#divDialogRename');

            // add dialog open event
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Copy Dialog
            $dlgDiv.modalDialog({
                control: $cmdButton,
                width: 320
            });
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
                width: 330
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
            width: 230px;
        }
        #divMainLeftPanel select
        {
            width: 220px;
        }
        #cmdPanel
        {
            width: 220px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label9" runat="server" CssClass="h2 spanLabel">Product Type:</asp:Label>
                <GGS:WebDropDownList ID="ddProductType" runat="server" AutoPostBack="True" CssClass="selectDropDown">
                </GGS:WebDropDownList>
                <asp:Label ID="Label3" runat="server" CssClass="h2 spanLabel">Products:</asp:Label>
                <asp:ListBox ID="lbProductList" runat="server" AutoPostBack="True" CssClass="selectListBox">
                </asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" Height="28px" Width="75px"
                        ToolTip="Add New Product" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" Height="28px" Width="75px"
                        ToolTip="Save Product changes" Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" Height="28px" Width="75px"
                        ToolTip="Delete Product" Text="Delete"></asp:Button>
                    <asp:Button ID="cmdCopy" runat="server" CssClass="inputButton" Height="28px" Width="75px"
                        ToolTip="Copy Product" Text="Copy"></asp:Button>
                    <asp:Button ID="cmdRename" runat="server" CssClass="inputButton" Height="28px" Width="75px"
                        ToolTip="Rename Product" Text="Rename"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel" style="width: 550px;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:DataList ID="dlProductConfig" runat="server">
                    <ItemTemplate>
                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="dlLblDesc" runat="server" Width="280px" CssClass="spanLabel" Text='<%# DataBinder.Eval(Container.DataItem,"Description")%>'></asp:Label>
                        <GGS:WebDropDownList ID="WebDropDownList" runat="server" CssClass="selectDropDown"
                            Width="197px" OnSelectedIndexChanged="dbProducts_IndexChanged">
                        </GGS:WebDropDownList>
                        <GGS:WebInputBox ID="ProductParamWebInputBox" runat="server" Width="190px" Text='<%# Container.DataItem("ProductParameterValue")%>'
                            OnTextChanged="dbProducts_TextChanged" CssClass="textEntry"></GGS:WebInputBox>
                        <asp:Image ID="imgSaved" runat="server" EnableViewState="true" Visible="False" ImageUrl="~/Images/check.gif"
                            Width="20px" Height="18px"></asp:Image>
                        <%# ObjectVisibilityInTemplate(Container)%>
                    </ItemTemplate>
                </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="New Product">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table1">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="label5" runat="server" CssClass="spanLabel">Enter New ID:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="txtNewProduct" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogCopy" title="Copy Product">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table2">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" CssClass="spanLabel">New ID for Copied Product:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="txtCopyProduct" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogRename" title="Rename Product">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table3">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" CssClass="spanLabel">New Name:</asp:Label>
                </td>
                <td>
                    <GGS:WebInputBox ID="txtRenameProduct" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
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
