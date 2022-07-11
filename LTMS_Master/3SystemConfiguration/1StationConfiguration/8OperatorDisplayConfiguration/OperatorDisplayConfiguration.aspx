<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OperatorDisplayConfiguration.aspx.vb" Inherits="LTMS_Master.OperatorDisplayConfiguration" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/rgbcolor.js") %>"></script>

    <script type="text/javascript">
        var $cmdNew;
        var $cmdDelete;
        var $cmdCopy;
        var $cmdRename;

        var $tbDescriptionNew;
        var $tbModelCodeNew;
        var $tbModelTypeNew;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            $('#MainContent_ddConfigurationTypes').unbind('change');

            DisableEnterKeyOnPage();

            CacheControls();
            CreateDialog_New();
            CreateDialog_Delete();
            CreateDialog_Copy();
            CreateDialog_Rename();

            AddColorToColorDropDownOptions();

            $('#MainContent_cmdSave').click(function () { ShowWaitOverlay(); });
        }
        function CacheControls() {

            $cmdNew = $('#MainContent_cmdNew');
            $cmdDelete = $('#MainContent_cmdDelete');
            $cmdCopy = $('#MainContent_cmdCopy');
            $cmdRename = $('#MainContent_cmdRename');


            $tbDescriptionNew = $('#MainContent_tbDescriptionNew');
            $tbModelCodeNew = $('#MainContent_tbModelCodeNew');
            $tbModelTypeNew = $('#MainContent_tbModelTypeNew');

        }

        function ValidateDialog_New() {

            var bValid = true;

            bValid = checkLength($tbModelTypeNew, "Model Type", 2, 2, $("#helptbModelTypeNew")) && bValid;
            bValid = checkLength($tbModelCodeNew, "Model Code", 2, 2, $("#helptbModelCodeNew")) && bValid;
            bValid = checkLength($tbDescriptionNew, "Description", 2, 2, $("#helptbDescriptionNew")) && bValid;


            return bValid;
        }
        function CreateDialog_New() {
            var $dlgDiv = $('#divDialogNew');

            // add event to button
            $cmdNew.click(function () {
                $dlgDiv.dialog('open');
                $("#MainContent_ddNewType").val($('#MainContent_ddConfigurationTypes').val());
                return false;
            });

            // Add New Dialog
            $dlgDiv.modalDialog({
                control: $cmdNew
                , width: 370
            });
        }
        function CreateDialog_Delete() {
            var $dlgDiv = $('#divDialogDelete');

            $cmdDelete.click(function () { $dlgDiv.dialog('open'); return false; });

            $dlgDiv.deleteDialog({ width: 340, control: $cmdDelete });
        }
        function CreateDialog_Copy() {
            var $dlgDiv = $('#divDialogCopy');

            $cmdCopy.click(function () { $dlgDiv.dialog('open'); return false; });

            $dlgDiv.modalDialog({ control: $cmdCopy });
        }
        function CreateDialog_Rename() {
            var $dlgDiv = $('#divDialogRename');

            // add event to button
            $cmdRename.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add New Dialog
            $dlgDiv.modalDialog({
                control: $cmdRename
            });
        }

        function AddColorToColorDropDownOptions() {
            $('.selectDropDown[title="Configuration Parameter Type ID = 2"]').each(function () {
                var opt = $(this);
                var backColor = opt.val();
                var foreColor = ConvertForeColor(backColor);

                var color = new RGBColor(backColor);
                if (color.ok) {
                    opt.css("background-color", color.toRGB())
                   .css("color", foreColor);
                }
            });
        }

    </script>
    <style type="text/css">
        #divMainCenterPanel span.spanLabel
        {
            width: 130px;
            display: inline-block;
        }
        #divDialogNew span.spanLabel, #divDialogCopy span.spanLabel, #divDialogRename span.spanLabel
        {
            width: 100px;
            display: inline-block;
        }
        #divMainContent span.ui-icon-triangle-1-e, #divDialogNew span.ui-icon-triangle-1-e, #divDialogCopy span.ui-icon-triangle-1-e, #divDialogRename span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
            margin-top: 4px;
        }
        #divMainCenterPanel
        {
            width: 500px;
        }
        #divMainLeftPanel
        {
            width: 360px;
        }
        #divMainLeftPanel select
        {
            width: 280px;
        }
        #cmdPanel
        {
            width: 280px;
            }
        #vertButton span
        {
            display: block;
            padding: 8px 0px;
            text-align: center;
            width: 50px;
        }
        #vertButton
        {
            width: 50px;
            float: right;
            margin-top: 80px;
            margin-right: 20px;
        }
        #divDialogNew p
        {
            margin-bottom: 4px;
            margin-top: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label1" runat="server" CssClass="h2 spanLabel">Types:</asp:Label>
                <asp:DropDownList ID="ddConfigurationTypes" runat="server" AutoPostBack="True" CssClass="selectDropDown" />
                <asp:Label ID="Label2" runat="server" CssClass="h2 spanLabel">Configurations:</asp:Label>
                <asp:ListBox ID="lbConfigurations" runat="server" AutoPostBack="True" CssClass="selectListBox" />
                <div id="vertButton">
                    <asp:ImageButton ID="ibUp" runat="server" ImageUrl="~/Images/Arrows/ArrowUp.png" CssClass="sortButton" Enabled="False" />
                    <asp:Label ID="lblMove" runat="server" CssClass="spanLabel">Move</asp:Label>
                    <asp:ImageButton ID="ibDown" runat="server" ImageUrl="~/Images/Arrows/ArrowDown.png" CssClass="sortButton" Enabled="False" />
                </div>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" Text="New" />
                    <asp:Button ID="cmdSave" runat="server" Text="Save" />
                    <asp:Button ID="cmdDelete" runat="server" Text="Delete" />
                </div>
                <asp:HiddenField ID="hidXmlParameters" runat="server" />
                <asp:HiddenField ID="hidXmlParametersSaved" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:DataList ID="dlParameters" runat="server">
                    <ItemTemplate>
                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="Label4" runat="server" CssClass="spanLabel" Text='<%# DataBinder.Eval(Container.DataItem,"Description")%>' />
                        <GGS:WebDropDownList ID="WebDropDownList" runat="server" CssClass="selectDropDown" OnSelectedIndexChanged="Parameter_IndexChanged" Width="197px" />
                        <GGS:WebInputBox ID="WebInputBox" runat="server" CssClass="textEntry" Text='<%# Container.DataItem("Value")%>' Width="190px" OnTextChanged="Parameter_TextChanged" />
                        <asp:Image ID="imgCheck" runat="server" Visible="False" ImageUrl="~/Images/check.gif" Width="20px" Height="18px" CssClass="checkImage" />
                        <%# DLControlBind(Container)%>
                    </ItemTemplate>
                </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="New Configuration Entry">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <p class="validationHints">
                </p>
                <div>
                    <p>
                        <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">Type:</span>
                        <asp:DropDownList ID="ddNewType" runat="server" CssClass="selectDropDown" Width="197px" />
                    </p>
                    <p>
                        <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">New Name:</span>
                        <asp:TextBox ID="tbNewName" CssClass="textEntry" runat="server" Width="190px" autofocus></asp:TextBox>
                    </p>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogDelete" title="Delete Configuration Entry">
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
    <div id="divDialogCopy" title="Copy Configuration Entry">
        <p class="validationHints">
        </p>
        <p>
            <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">New Name:</span>
            <asp:TextBox ID="tbCopyName" CssClass="textEntry" runat="server"></asp:TextBox>
        </p>
    </div>
    <div id="divDialogRename" title="Rename Configuration Entry">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <p class="validationHints">
                </p>
                <p>
                    <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">New Name:</span>
                    <asp:TextBox ID="tbRenameName" CssClass="textEntry" runat="server"></asp:TextBox>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
