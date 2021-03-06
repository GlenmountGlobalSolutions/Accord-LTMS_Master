<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ParameterListConfiguration.aspx.vb" Inherits="LTMS_Master.ParameterListConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            try {
                AddDirtyClassOnChange();
                AddDialog_NewParameterList();
                AddDialog_NewListValue();

                AddDialog_CopyList();

                AddDialog_DeleteList();
                AddDialog_DeleteValue();

                // add the wait cursor to the up / down button clicks
                $('.sortButton').click(function () { ShowCursor_Wait(); });

                DisableEnterKeyOnPage();
            } catch (err) {
                alert(err);
            }
        }

        function AddDialog_NewParameterList() {
            var $cmdNew = $('#MainContent_cmdNew');
            var $dlgNew = $('#divDialogNewParameterList');

            // add event to button
            $cmdNew.click(function () { $dlgNew.dialog('open'); return false; });

            // Add New Dialog
            $dlgNew.modalDialog({
                control: $cmdNew,
                width: 410,
                validationFunction: function () { return ValidateDialog_NewParameterList(); }
            });
        }

        function AddDialog_NewListValue() {
            var $cmdNew = $('#MainContent_cmdNewProdParamListValue');
            var $dlgNew = $('#divDialogNewListValue');

            // add event to button
            $cmdNew.click(function () { $dlgNew.dialog('open'); return false; });

            // Add New Dialog
            $dlgNew.modalDialog({
                control: $cmdNew,
                width: 360,
                okFunction: function () { return ValidateDialog_NewListValue(); }
            });
        }

        function AddDialog_CopyList() {
            var $cmdCopy = $('#MainContent_cmdCopy');
            var $dlgCopy = $('#divDialogCopy');

            // add event to button
            $cmdCopy.click(function () { $dlgCopy.dialog('open'); return false; });

            // Add New Dialog
            $dlgCopy.modalDialog({
                control: $cmdCopy,
                width: 410,
                okFunction: function () { return ValidateDialog_CopyList(); }
            });
        }

        function AddDialog_DeleteList() {
            var $cmdDelete = $('#MainContent_cmdDelete');
            var $divDelete = $('#divDialogDeleteList');

            // add event to button
            $cmdDelete.click(function () { $divDelete.dialog('open'); return false; });

            // Add Delete Dialog
            $divDelete.deleteDialog({ control: $cmdDelete, width: 400 });
        }

        function AddDialog_DeleteValue() {
            var $cmdDelete = $('#MainContent_cmdDeleteProdParamListValue');
            var $divDelete = $('#divDialogDeleteValue');

            // add event to button
            $cmdDelete.click(function () { $divDelete.dialog('open'); return false; });

            // Add Delete Dialog
            $divDelete.deleteDialog({ control: $cmdDelete, width: 400 });
        }


        function ValidateDialog_NewParameterList() {
            var bValid = false;
            try {
                bValid = checkText($('#MainContent_txtDesc'), 'Enter a value for the new Parameter List.', $('#helpParameterList'));
            } catch (err) {
                alert(err);
            }
            return bValid;
        }

        function ValidateDialog_NewListValue() {
            var bValid = false;
            try {
                bValid = checkText($('#MainContent_txtVal'), 'Enter a value for the new Parameter List.', $('#helpParameterValue'));
            } catch (err) {
                alert(err);
            }
            return bValid;
        }

        function ValidateDialog_CopyList() {
            var bValid = false;
            try {
                bValid = checkText($('#MainContent_txtList'), 'Enter a value for the new Parameter List.', $('#helpList'));
            } catch (err) {
                alert(err);
            }
            return bValid;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="hidParamListScrollTop" runat="server" value="0" />
    <input type="hidden" id="hidListValuesScrollTop" runat="server" value="0" />
    <asp:Panel ID="mainContentPanel" runat="server" DefaultButton="cmdSave">
        <asp:UpdatePanel ID="upParameterList" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="text-align: center;">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="spanLabel h2">Select List To Modify</asp:Label>
                        </td>
                        <td style="width: 100px;">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" CssClass="spanLabel h2">List Items</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>                           
                           <asp:ListBox ID="lbProdParamList" runat="server" AutoPostBack="True" height="300px" Width="200px" CssClass="selectListBox0"></asp:ListBox>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibUPProdParamList" runat="server" ImageUrl="~/Images/Arrows/ArrowUp.png" CssClass="sortButton" Enabled="False"></asp:ImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 10px; padding-bottom: 14px;">
                                        <asp:Label ID="Label2" runat="server" CssClass="spanLabel">Move</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibDNProdParamList" runat="server" ImageUrl="~/Images/Arrows/ArrowDown.png" CssClass="sortButton" Enabled="False"></asp:ImageButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:ListBox ID="lbParamListValues" runat="server" AutoPostBack="True" Height="300px" Width="200px" CssClass="selectListBox0"></asp:ListBox>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibUpProdParamListValues" runat="server" ImageUrl="~/Images/Arrows/ArrowUp.png" CssClass="sortButton" Enabled="False"></asp:ImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 10px; padding-bottom: 14px;">
                                        <asp:Label ID="Label33" runat="server" CssClass="spanLabel">Move</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibDNProdParamListValues" runat="server" ImageUrl="~/Images/Arrows/ArrowDown.png" CssClass="sortButton" Enabled="False"></asp:ImageButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 6px;">
                            <asp:Label ID="Label3" runat="server" CssClass="spanLabel h2">List Description</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td style="padding-top: 6px;">
                            <asp:Label ID="Label4" runat="server" CssClass="spanLabel h2">List Item Description</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtProdParamListDesc" runat="server" CssClass="textEntry" Width="196px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtProdParamListVal" runat="server" CssClass="textEntry" Width="196px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="tableCenter" style="padding-top: 6px;">
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="cmdSave" runat="server" Width="80px" Text="Save" CssClass="inputButton"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="cmdCopy" runat="server" Width="80px" Text="Copy" CssClass="inputButton"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Button ID="cmdNew" runat="server" Width="80px" Text="New" CssClass="inputButton"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="cmdDelete" runat="server" Width="80px" Text="Delete" CssClass="inputButton"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <table class="tableCenter" style="padding-top: 6px;">
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="cmdSaveProdParamListValue" runat="server" Width="80px" Text="Save" CssClass="inputButton"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Button ID="cmdNewProdParamListValue" runat="server" Width="80px" Text="New" CssClass="inputButton"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="cmdDeleteProdParamListValue" runat="server" Width="80px" Text="Delete" CssClass="inputButton"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div id="divDialogNewParameterList" title="Add New Prod Parameter List">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table1" class="tableCenter">
            <tr>
                <td style="width: 160px;">
                    <asp:Label ID="lblProdParamListID" runat="server" CssClass="spanLabel">New Parameter List:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server" Width="180px" CssClass="textEntry"></asp:TextBox>
                </td>
                <td>
                    <div id="helpParameterList" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogNewListValue" title="Add New Prod Parameter List Item">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table2" class="tableCenter">
            <tr>
                <td style="width: 120px;">
                    <asp:Label ID="Label5" runat="server" CssClass="spanLabel">New List Item:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVal" runat="server" Width="180px" CssClass="textEntry"></asp:TextBox>
                </td>
                <td>
                    <div id="helpParameterValue" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogCopy" title="Copy with New Prod Parameter List Item">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table3" class="tableCenter">
            <tr>
                <td style="width: 160px;">
                    <asp:Label ID="Label7" runat="server" CssClass="spanLabel">Copy Parameter List:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtList" runat="server" Width="180px" CssClass="textEntry"></asp:TextBox>
                </td>
                <td>
                    <div id="helpList" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDeleteList" title="Confirm Deletion">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            <span class="spanLabel">You are about to Delete the selected Parameter List.</span>
        </p>
        <p class="pCenter" style="margin-left: 30px;">
            <span class="spanLabel">Do you wish to continue?</span>
        </p>
    </div>
    <div id="divDialogDeleteValue" title="Confirm Deletion">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            <span class="spanLabel">You are about to Delete the selected Parameter Value.</span>
        </p>
        <p class="pCenter" style="margin-left: 30px;">
            <span class="spanLabel">Do you wish to continue?</span>
        </p>
    </div>
</asp:Content>
