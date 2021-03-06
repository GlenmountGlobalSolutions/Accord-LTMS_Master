<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ProductCodeConfiguration.aspx.vb" Inherits="LTMS_Master.ProductCodeConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var $hidDirtyBit;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();

            CachePartialLoadControls();

            CreateDialog_AddProductCode();

            CreateDialog_SaveWarning();
        }

        function CachePartialLoadControls() {
            $hidDirtyBit = $('#MainContent_hidDirtyBit');
        }

        function CreateDialog_AddProductCode() {
            var $cmdAddProduct = $('#MainContent_cmdAddProduct');
            var $dlgAddProduct = $('#divDialogAddProduct');

            $cmdAddProduct.click(function () { $('#MainContent_lbAddProduct').find("option").attr("selected", false); $dlgAddProduct.dialog('open'); return false; });

            $dlgAddProduct.modalDialog({
                control: $cmdAddProduct
            });
        }

        function CreateDialog_SaveWarning() {
            var $cmd1_SaveWarning = $('#MainContent_ddlStyleGroups');
            var $cmd2_SaveWarning = $('#MainContent_ddlLineNum');
            var $dlgSaveWarning = $('#divDlgSaveWarning');

            $cmd1_SaveWarning.click(function () { if ($hidDirtyBit.val() == "true") { $dlgSaveWarning.dialog('open'); } return false; });
            $cmd2_SaveWarning.click(function () { if ($hidDirtyBit.val() == "true") { $dlgSaveWarning.dialog('open'); } return false; });

            $dlgSaveWarning.dialog({
                modal: true,
                width: 550,
                autoOpen: false,
                buttons: { Ok: function () { $(this).dialog("close"); } }
            });
        }
    </script>
    <style type="text/css">
        #div1_Left ul, #div2_LeftCenter ul, #div3_RightCenter ul, #div4_Right ul
        {
            list-style: none;
            margin: 0px;
            padding: 0px;
        }
        #div1_Left li, #div2_LeftCenter li, #div3_RightCenter li, #div4_Right li
        {
            margin-bottom: 4px;
        }
        
        #div4_Right li
        {
            float: left;
            text-align: left;
            margin-left: 6px;
        }
        
        #div1_Left, #div2_LeftCenter, #div3_RightCenter, #div4_Right
        {
            float: left;
            height: 460px;
            text-align: left;
            width: 250px;
        }
        
        #div4_Right span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        #centerListBox
        {
            width: 246px;
            margin: 0 auto;
            text-align: left;
        }
        .selectCheckBox
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id='div1_Left'>
        <asp:UpdatePanel ID="upLeft" runat="server">
            <ContentTemplate>
                <ul>
                    <li>
                        <asp:Label ID="Label5" runat="server" CssClass="spanLabel h2" Width="240px">Line Number:</asp:Label></li>
                    <li>
                        <asp:DropDownList ID="ddlLineNum" runat="server" AutoPostBack="True" CssClass="selectDropDown h3" Width="240px">
                        </asp:DropDownList>
                    </li>
                    <li style="padding-top: 18px;">
                        <asp:Label ID="Label8" runat="server" Width="240px" CssClass="spanLabel h2">Style Group:</asp:Label></li>
                    <li>
                        <asp:DropDownList ID="ddlStyleGroups" runat="server" AutoPostBack="True" CssClass="selectDropDown h3" Width="240px">
                        </asp:DropDownList>
                    </li>
                    <li>
                        <asp:CheckBox ID="cbAllProducts" runat="server" Text="Apply Setting to all products in the selected Style Group" AutoPostBack="True" Width="240px" CssClass="selectCheckBox h3" /></li>
                </ul>
                <asp:HiddenField ID="hidDirtyBit" runat="server" Value="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id='div2_LeftCenter'>
        <asp:UpdatePanel ID="upLeftCenter" runat="server">
            <ContentTemplate>
                <ul>
                    <li>
                        <asp:Label ID="Label1" runat="server" Width="100px" CssClass="spanLabel h2">Product:</asp:Label>
                    </li>
                    <li>
                        <asp:ListBox ID="lbProducts" runat="server" AutoPostBack="True" Width="246px" Height="300px" CssClass="selectListBox"></asp:ListBox>
                    </li>
                    <li>
                        <asp:Button ID="cmdAddProduct" runat="server" Text="Add Product" Width="106px" CssClass="inputButton" />
                        <asp:Button ID="cmdRemoveProduct" runat="server" Text="Remove Product" Width="128px" CssClass="inputButton" />
                    </li>
                    <li>
                        <p style="float: right; padding-top: 40px; padding-right: 4px;">
                            <asp:Button ID="cmdSave" runat="server" Text="Save Configuration" Width="146px" CssClass="inputButton" />
                        </p>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id='div3_RightCenter'>
        <asp:UpdatePanel ID="upRightCenter" runat="server">
            <ContentTemplate>
                <ul>
                    <li>
                        <asp:Label ID="Label2" runat="server" CssClass="spanLabel h2">Component Name:</asp:Label>
                    </li>
                    <li>
                        <asp:ListBox ID="lbComponentNames" runat="server" AutoPostBack="True" Width="246px" Height="300px" CssClass="selectListBox"></asp:ListBox>
                    </li>
                    <li>
                        <p style="float: Left; padding-top: 74px; padding-left: 4px;">
                            <asp:Button ID="cmdCancel" runat="server" Text="Cancel Changes" Width="132px" CssClass="inputButton" />
                        </p>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id='div4_Right'>
        <asp:UpdatePanel ID="upRight" runat="server">
            <ContentTemplate>
                <ul>
                    <li>
                        <asp:Label ID="Label3" runat="server" CssClass="spanLabel h2">Component Code:</asp:Label>
                    </li>
                    <li>
                        <asp:DropDownList ID="ddlComponentCodes" runat="server" AutoPostBack="True" Width="246px" Style="margin-bottom: 10px" CssClass="selectDropDown h3">
                        </asp:DropDownList>
                    </li>
                    <li><span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="Label4" runat="server" CssClass="spanLabel h3" Width="108px">Mask 1 Start:</asp:Label>
                        <asp:TextBox ID="txtMask1Start" runat="server" Width="50px" CssClass="textEntry h3" ReadOnly="True"></asp:TextBox>
                    </li>
                    <li><span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="Label6" runat="server" CssClass="spanLabel h3" Width="108px">Mask 1 Length: </asp:Label>
                        <asp:TextBox ID="txtMask1Length" runat="server" Width="50px" CssClass="textEntry h3" ReadOnly="True"></asp:TextBox></li>
                    <li><span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="Label7" runat="server" CssClass="spanLabel h3" Width="108px">Mask 2 Start:</asp:Label>
                        <asp:TextBox ID="txtMask2Start" runat="server" Width="50px" CssClass="textEntry h3" ReadOnly="True"></asp:TextBox></li>
                    <li><span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="Label9" runat="server" CssClass="spanLabel h3" Width="108px">Mask 2 Length: </asp:Label>
                        <asp:TextBox ID="txtMask2Length" runat="server" Width="50px" CssClass="textEntry h3" ReadOnly="True"></asp:TextBox></li>
                    <li><span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="Label10" runat="server" CssClass="spanLabel h3" Width="108px">Fails: </asp:Label>
                        <asp:TextBox ID="txtFails" runat="server" Width="50px" CssClass="textEntry h3" ReadOnly="True"></asp:TextBox></li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogAddProduct" title="Add Product Code">
        <div id="centerListBox">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:ListBox ID="lbAddProduct" runat="server" Width="246px" Height="300px" CssClass="selectListBox" SelectionMode="Multiple"></asp:ListBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="divDlgSaveWarning" title="Warning">
        <p>
            <span class="ui-icon ui-icon-alert" style="float: left;"></span><span class="spanLabel">This Product Configuration has changed.<br />
                Please Save or Cancel your changes before selecting a new Line Number or Style Group. </span>
        </p>
    </div>
</asp:Content>
