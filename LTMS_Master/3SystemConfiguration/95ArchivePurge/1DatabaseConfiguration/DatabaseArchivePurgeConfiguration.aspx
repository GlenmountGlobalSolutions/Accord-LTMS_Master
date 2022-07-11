<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DatabaseArchivePurgeConfiguration.aspx.vb" Inherits="LTMS_Master.DatabaseArchivePurgeConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.alphanum.js") %>"></script>
    <script type="text/javascript">
        var $tbDescriptionNew;
        var $cbAutoArchiveNew;
        var $cbAutoArchiveDBNew;
        var $cbAutoPurgeNew;
        var $tbArchiveFolderNew;
        var $cbRequiredForPurgeNew;
        var $tbArchiveAgeNew;
        var $tbPurgeAgeNew;
        var $ddlTableNameNew;
        var $ddlDTFieldNameNew;


        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            CacheControls();

            // make controls with class 'numericOnly' into numeric only
            $('.numericOnly').numeric({ allowMinus: false,
                allowThouSep: false,
                allowDecSep: false,
                maxDigits: '4'
            });

            //add dialogs
            CreateDialog_New()
            CreateDialog_Delete();
        }

        function CacheControls() {
            // New dialog validation
            $tbDescriptionNew = $('#MainContent_tbDescriptionNew');
            $cbAutoArchiveNew = $('#MainContent_cbAutoArchiveNew');
            $cbAutoArchiveDBNew = $('#MainContent_cbAutoArchiveDBNew');
            $cbAutoPurgeNew = $('#MainContent_cbAutoPurgeNew');
            $tbArchiveFolderNew = $('#MainContent_tbArchiveFolderNew');
            $cbRequiredForPurgeNew = $('#MainContent_cbRequiredForPurgeNew');
            $tbArchiveAgeNew = $('#MainContent_tbArchiveAgeNew');
            $tbPurgeAgeNew = $('#MainContent_tbPurgeAgeNew');
            $ddlTableNameNew = $('#MainContent_ddlTableNameNew');
            $ddlDTFieldNameNew = $('#MainContent_ddlDTFieldNameNew');
        }

        function CreateDialog_New() {
            var $cmdBtn = $('#MainContent_cmdNew');
            var $dlgDiv = $('#divDialogNew');

            // add event to button
            $cmdBtn.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add New Dialog
            $dlgDiv.modalDialog({
                control: $cmdBtn,
                width: 600,
                validationFunction: function () { return ValidateDialog_New(); }
            });
        }

        function ValidateDialog_New() {

            var bValid = true;

            bValid = checkDropDownListByIndex($ddlDTFieldNameNew, 1, "Please select a table.", $('#helpddlDTFieldNameNew')) && bValid;
            bValid = checkDropDownListByIndex($ddlTableNameNew, 1, "Please select a table field.", $('#helpddlTableNameNew')) && bValid;
            bValid = checkNumericRange($tbPurgeAgeNew, 1, 9999, "Please enter a Purge Age greater than 0.", $('#helptbPurgeAgeNew')) && bValid;
            bValid = checkText($tbArchiveFolderNew, "Please enter an archive folder path.", $('#helptbArchiveFolderNew')) && bValid;
            bValid = checkNumericRange($tbArchiveAgeNew, 1, 9999, "Please enter an Archive Age greater than 0.", $('#helptbArchiveAgeNew')) && bValid;
            bValid = checkText($tbDescriptionNew, "Please enter a description.", $('#helptbDescriptionNew')) && bValid;

            return bValid;
        }

        function CreateDialog_Delete() {
            var $cmdButton = $('#MainContent_cmdDelete');
            var $dlgDiv = $('#divDialogDelete');

            // add event to button
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Delete Dialog
            $dlgDiv.deleteDialog({ control: $cmdButton });
        }
    </script>
    <style type="text/css">
        #divMainContent span.ui-icon-triangle-1-e, #divDialogNew span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label4" runat="server" CssClass="h2 spanLabel">Purge Entries:</asp:Label>
                <asp:ListBox ID="lbList" runat="server" AutoPostBack="True" CssClass="selectListBox"></asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" Text="Delete"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td style="width: 200px;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label1" runat="server" CssClass="spanLabel">Description</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbDescription" runat="server" Width="288px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgDesc" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestDesc" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label2" runat="server" CssClass="spanLabel">Auto Archive Enabled</asp:Label>
                        </td>
                        <td>
                            <GGS:WebCheckBox ID="cbAutoArchive" runat="server" CssClass="selectCheckBox"></GGS:WebCheckBox>
                            <asp:Image ID="imgAutoArch" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestAutoArch" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label3" runat="server" CssClass="spanLabel">Auto Purge Enabled</asp:Label>
                        </td>
                        <td>
                            <GGS:WebCheckBox ID="cbAutoPurge" runat="server" CssClass="selectCheckBox"></GGS:WebCheckBox>
                            <asp:Image ID="imgAutoPurge" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestAutoPurge" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label21" runat="server" CssClass="spanLabel">Auto Archive to DB Enabled</asp:Label>
                        </td>
                        <td>
                            <GGS:WebCheckBox ID="cbAutoArchiveToDB" runat="server" CssClass="selectCheckBox"></GGS:WebCheckBox>
                            <asp:Image ID="imgAutoArchiveToDB" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestAutoArchiveToDB" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label10" runat="server" CssClass="spanLabel">Archive Folder Path</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbArchiveFolder" runat="server" Width="288px" CssClass="textEntry"></GGS:WebInputBox>
                            <asp:Image ID="imgPath" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestPath" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label5" runat="server" CssClass="spanLabel">Archive Required For Purge</asp:Label>
                        </td>
                        <td>
                            <GGS:WebCheckBox ID="cbRequiredForPurge" runat="server" CssClass="selectCheckBox"></GGS:WebCheckBox>
                            <asp:Image ID="imgArchReq" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestArchReq" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label6" runat="server" CssClass="spanLabel">Age In Days For Archive</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbArchiveAge" runat="server" Width="288px" CssClass="textEntry numericOnly"></GGS:WebInputBox>
                            <asp:Image ID="imgArchAge" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestArchAge" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label7" runat="server" CssClass="spanLabel">Age In Days For Purge</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbPurgeAge" runat="server" Width="288px" CssClass="textEntry numericOnly"></GGS:WebInputBox>
                            <asp:Image ID="imgPurgeAge" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestPurgeAge" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label8" runat="server" CssClass="spanLabel">Table Name</asp:Label>
                        </td>
                        <td>
                            <GGS:WebDropDownList ID="ddlTableName" runat="server" Width="295px" AutoPostBack="True" CssClass="selectDropDown">
                            </GGS:WebDropDownList>
                            <asp:Image ID="imgTable" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestTable" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label9" runat="server" CssClass="spanLabel">D/T Field Name</asp:Label>
                        </td>
                        <td>
                            <GGS:WebDropDownList ID="ddlDTFieldName" runat="server" Width="295px" CssClass="selectDropDown">
                            </GGS:WebDropDownList>
                            <asp:Image ID="imgDT" runat="server" Width="18px" Height="18px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                            <asp:Image ID="imgQuestDT" runat="server" ImageUrl="~/Images/question_mark.gif" Height="18px" Width="18px" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="New Purge Configuration">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <p class="validationHints">
                </p>
                <table class="tableCenter">
                    <tr>
                        <td style="width: 200px;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label11" runat="server" CssClass="spanLabel">Description</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbDescriptionNew" runat="server" Width="288px" CssClass="textEntry"></asp:TextBox>
                        </td>
                        <td>
                            <div id="helptbDescriptionNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label12" runat="server" CssClass="spanLabel">Auto Archive Enabled</asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbAutoArchiveNew" runat="server" CssClass="selectCheckBox"></asp:CheckBox>
                        </td>
                        <td>
                            <div id="helpcbAutoArchiveNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label20" runat="server" CssClass="spanLabel">Auto Archive to DB Enabled</asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbAutoArchiveDBNew" runat="server" CssClass="selectCheckBox"></asp:CheckBox>
                        </td>
                        <td>
                            <div id="helpcbAutoArchiveDBNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label13" runat="server" CssClass="spanLabel">Auto Purge Enabled</asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbAutoPurgeNew" runat="server" CssClass="selectCheckBox"></asp:CheckBox>
                        </td>
                        <td>
                            <div id="helpcbAutoPurgeNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label14" runat="server" CssClass="spanLabel">Archive Folder Path</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbArchiveFolderNew" runat="server" Width="288px" CssClass="textEntry"></asp:TextBox>
                        </td>
                        <td>
                            <div id="helptbArchiveFolderNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label15" runat="server" CssClass="spanLabel">Archive Required For Purge</asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbRequiredForPurgeNew" runat="server" CssClass="selectCheckBox"></asp:CheckBox>
                        </td>
                        <td>
                            <div id="helpcbRequiredForPurgeNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label16" runat="server" CssClass="spanLabel">Age In Days For Archive</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbArchiveAgeNew" runat="server" Width="288px" CssClass="textEntry numericOnly"></asp:TextBox>
                        </td>
                        <td>
                            <div id="helptbArchiveAgeNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label17" runat="server" CssClass="spanLabel">Age In Days For Purge</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbPurgeAgeNew" runat="server" Width="288px" CssClass="textEntry numericOnly"></asp:TextBox>
                        </td>
                        <td>
                            <div id="helptbPurgeAgeNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label18" runat="server" CssClass="spanLabel">Table Name</asp:Label>
                        </td>
                        <td style="height: 16px">
                            <asp:DropDownList ID="ddlTableNameNew" runat="server" Width="295px" AutoPostBack="True" CssClass="selectDropDown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <div id="helpddlTableNameNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label19" runat="server" CssClass="spanLabel">D/T Field Name</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDTFieldNameNew" runat="server" Width="295px" CssClass="selectDropDown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <div id="helpddlDTFieldNameNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogDelete" title="Delete Purge Configuration">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to delete the selected Purge Entry. Do you wish to continue?
        </p>
    </div>
</asp:Content>
