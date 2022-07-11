<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LabelConfiguration.aspx.vb" Inherits="LTMS_Master.LabelConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.alphanum.js") %>"></script>
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();

            //Make Next Prod Number Textbox Numeric Only (alphanum() plugin found in jquery.alphanum.js)
            $('#MainContent_tNextProdNumber').alphanum({maxLength:7});

            CreateDialog_New();
            CreateDialog_Delete();
        }

        function CreateDialog_New() {
            var $cmdButton = $('#MainContent_cmdNew');
            var $dlgDiv = $('#divDialogNew');

            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            $dlgDiv.modalDialog({
                control: $cmdButton,
                validationFunction: function () { return checkDropDownList($("#MainContent_ddProductStyle"), "Please select a Style Code.", $('#helpSeatTyle')); }
            });
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
        #divMainContent span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        #divDialogNew select
        {
            width: 100px;
            margin-left: 10px;
        }
        #labelData
        {
            margin-top: 6px;
            padding: 12px 6px;
        }
        div.datagrid
        {
            margin-top: 6px;
            overflow: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label4" runat="server" CssClass="h2 spanLabel">Style Code:</asp:Label>
                <asp:ListBox ID="lbSeatStyle" runat="server" AutoPostBack="True" CssClass="selectListBox"></asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" ToolTip="Add new seat style." Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" ToolTip="Save changes to selected seat style." Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" ToolTip="Delete selected seat style." Text="Delete"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="labelProperties">
                    <table>
                        <tr>
                            <td style="width: 200px;">
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label1" runat="server" Width="150px" CssClass="spanLabel">Next Prod. Number:</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="tNextProdNumber" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                                <asp:Image ID="Image2" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                                <asp:Label ID="lblNextProdNumber" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label3" runat="server" Width="150px" CssClass="spanLabel">Comment:</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="tComment" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                                <asp:Image ID="Image4" runat="server" Height="14px" Visible="False" ImageUrl="~/Images/check.gif" EnableViewState="False"></asp:Image>
                                <asp:Label ID="lblComment" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label6" runat="server" Width="150px" CssClass="spanLabel">Seat Part Number:</asp:Label>
                            </td>
                            <td>
                                <GGS:WebInputBox ID="tSeatPartNumber" runat="server" Width="184px" CssClass="textEntry"></GGS:WebInputBox>
                                <asp:Image ID="Image5" runat="server" Height="14px" EnableViewState="False" ImageUrl="~/Images/check.gif" Visible="False"></asp:Image>
                                <asp:Label ID="lblSeatPartNumber" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label18" runat="server" Width="150px" CssClass="spanLabel">Modified Date Time:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblModifyDT" runat="server" Width="184px" CssClass="spanReadOnly"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label20" runat="server" Width="150px" CssClass="spanLabel">Recorded By:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblRecordedBy" runat="server" Width="184px" CssClass="spanReadOnly"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="labelData">
                    <asp:Label ID="Label2" runat="server" CssClass="h2 spanLabel">Seat Styles:</asp:Label>
                    <div class="datagrid">
                        <asp:DataGrid ID="dgSeatStyle" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle"
                            AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle" EditItemStyle-CssClass="DataGrid_EditItemStyle" PagerStyle-CssClass="DataGrid_PagerStyle"
                            SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle" CssClass="">
                            <ItemStyle Wrap="False"></ItemStyle>
                            <Columns>
                                <asp:BoundColumn DataField="STYLE_CODE" HeaderText="Seat Style"></asp:BoundColumn>
                                <asp:BoundColumn DataField="NEXT_PRODUCTION_NUMBER" HeaderText="Next Prod. Number"></asp:BoundColumn>
                                <asp:BoundColumn DataField="LABEL_SEAT_PART_NUMBER" HeaderText="Seat Part Number"></asp:BoundColumn>
                                <asp:BoundColumn DataField="LABEL_HUMAN_READABLE" HeaderText="Comment"></asp:BoundColumn>
                                <asp:BoundColumn DataField="MODIFIED_DT" HeaderText="Mod. Date Time"></asp:BoundColumn>
                                <asp:BoundColumn DataField="RECORDED_BY" HeaderText="Recorded By"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="New Label Configuration">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table class="tableCenter">
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" CssClass="spanLabel">Style Code:</asp:Label>
                    <asp:DropDownList ID="ddProductStyle" runat="server" CssClass="selectDropDown">
                    </asp:DropDownList>
                </td>
                <td>
                    <div id="helpSeatTyle" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Label Configuration">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to delete the selected Style. Do you wish to continue?.
        </p>
    </div>
</asp:Content>
