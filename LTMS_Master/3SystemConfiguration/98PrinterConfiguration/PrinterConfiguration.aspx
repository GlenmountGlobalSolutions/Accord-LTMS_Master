<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PrinterConfiguration.aspx.vb" Inherits="LTMS_Master.PrinterConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();

            CreateDialog_New();
            CreateDialog_Delete();
        }

        function CreateDialog_New() {
            var $cmdButton = $('#MainContent_cmdNew');
            var $dlgDiv = $('#divDialogNew');

            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            $dlgDiv.modalDialog({
                control: $cmdButton,
                width:360,
                validationFunction: function () { return ValidateDialog_New(); }
            });
        }

        function ValidateDialog_New() {
            var bValid = true;
            bValid = checkLength($("#MainContent_txtPrinterName"), "Printer Name", 1, 50, $("#helpPrinterName"));
            bValid = checkLength($("#MainContent_txtPrinterPath"), "Printer Path", 1, 50, $("#helpPrinterPath")) && bValid;
            return bValid; 
        }

        function CreateDialog_Delete() {
            var $cmdButton = $('#MainContent_cmdDelete');
            var $dlgDiv = $('#divDialogDelete');

            // add event to button
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Delete Dialog
            $dlgDiv.deleteDialog({ control: $cmdButton, width:360 });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" DefaultButton="cmdSave" runat="server">
                <table id="tblPrinterConfiguration">
                    <tr>
                        <td style="vertical-align: top;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" CssClass="spanLabel h2">Printers:</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <asp:ListBox ID="lbPrinters" runat="server" Height="300px" AutoPostBack="True" Width="144px" CssClass="selectListBox" DataTextField="Name" DataValueField="tblLabelPrintersID"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; padding-top: 8px;">
                                        <asp:Button ID="cmdNew" runat="server" Width="80px" Text="New"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="cmdSave" runat="server" Width="80px" Text="Save"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="cmdDelete" runat="server" Width="80px" Text="Delete"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table id="Table1" style="width: 391px; height: 41px">
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="spanLabel">Printer Name:</asp:Label>
                                    </td>
                                    <td>
                                        <GGS:WebInputBox ID="tPrinterName" runat="server" Width="176px" CssClass="textEntry"></GGS:WebInputBox>
                                        <asp:Image ID="Image1" runat="server" Width="15px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="spanLabel">Network Path:</asp:Label>
                                    </td>
                                    <td>
                                        <GGS:WebInputBox ID="tPrinterPath" runat="server" Width="176px" CssClass="textEntry"></GGS:WebInputBox>
                                        <asp:Image ID="Image2" runat="server" Height="14px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="spanLabel">Label Printer:</asp:Label>
                                    </td>
                                    <td>
                                        <GGS:WebCheckBox ID="xLabelPrinter" runat="server"></GGS:WebCheckBox>
                                        <asp:Image ID="Image3" runat="server" Height="14px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table2" style="margin-top: 14px;">
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="spanLabel h2">Printers:</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 12px;">
                                    </td>
                                    <td>
                                        <div class="datagrid">
                                            <asp:DataGrid ID="dgLabelPrinters" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle" AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle" EditItemStyle-CssClass="DataGrid_EditItemStyle" PagerStyle-CssClass="DataGrid_PagerStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle" CssClass="borderRadius">
                                                <Columns>
                                                    <asp:BoundColumn DataField="Name" HeaderText="Printer Name"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="NetworkPath" HeaderText="Network Path"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDialogNew" title="New Printer">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table3">
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Height="1px" Width="117px" CssClass="spanLabel"> Printer Name:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPrinterName" runat="server" Width="180px" CssClass="textEntry"></asp:TextBox>
                </td>
                <td>
                    <div id="helpPrinterName" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Width="99px" CssClass="spanLabel"> Network Path:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPrinterPath" runat="server" Width="180px" CssClass="textEntry"></asp:TextBox>
                </td>
                <td>
                    <div id="helpPrinterPath" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Printer">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to Delete the selected Printer. <br />Do you wish to continue?
        </p>
    </div>
</asp:Content>
