<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MaterialDispositionHistory.aspx.vb" Inherits="LTMS_Master.MaterialDispositionHistory" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            CreateDialog_Delete();
            $("#divPager input:image").mouseover(function () { $(this).css('cursor', 'pointer'); });

        }

        function CreateDialog_Delete() {
            var $cmdButton = $('#MainContent_cmdDelete');
            var $dlgDiv = $('#divDialogDelete');

            // Add Delete User Dialog Click Action
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            $dlgDiv.deleteDialog({
                control: $cmdButton
            });
        }
    </script>
    <style>
        #divPager input
        {
            vertical-align: middle;
        }
        #divPager span
        {
            vertical-align: middle;
            margin-left:4px;
        }
        .datagrid
        {
            margin-top: 6px;
        }
        .datagrid input.textEntry
        {
            /*reset text field so that datagrid looks like a datagrid.*/
            border-radius: 0px;
            padding: 1px 0 1px 0;
            border: 2px inset ThreeDFace;
        }
        
        .divWithScroll
        {
            overflow: auto;
            padding-bottom: 12px;
            width:100%;
        }
        #divMainCenterPanel
        {
            width: 83%;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="cmdPanel">
                    <asp:Button ID="cmdSave" runat="server" Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" Text="Delete"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="divPager">
                    <asp:ImageButton ID="imbFirstPage" runat="server" ToolTip="First Page" ImageUrl="~/Images/Arrows/bbFirst.gif"></asp:ImageButton>
                    <asp:ImageButton ID="imbPrevPage" runat="server" ToolTip="Previous Page" ImageUrl="~/Images/Arrows/bbPrevpage.gif"></asp:ImageButton>
                    <asp:Label ID="Label4" runat="server" CssClass="spanLabel">Go to Page:</asp:Label>
                    <asp:TextBox ID="tbPageNum" runat="server" Width="48px" CssClass="textEntry"></asp:TextBox>
                    <asp:ImageButton ID="imgGoToPage" TabIndex="1" runat="server" ToolTip="Go to Page" ImageUrl="~/Images/Arrows/mgotopage.gif"></asp:ImageButton>
                    <asp:ImageButton ID="imbNextPage" runat="server" ToolTip="Next Page" ImageUrl="~/Images/Arrows/bbNextpage.gif"></asp:ImageButton>
                    <asp:ImageButton ID="imbLastPage" runat="server" ToolTip="Last Page" ImageUrl="~/Images/Arrows/bbLast.gif"></asp:ImageButton>
                </div>
                <div class="divWithScroll">
                <div class="datagrid">
                    <asp:DataGrid ID="dgMain" runat="server" AutoGenerateColumns="False" AllowPaging="True" PagerStyle-Position="Bottom" HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle" AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle" EditItemStyle-CssClass="DataGrid_EditItemStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle" PageSize="15">
                        <PagerStyle PageButtonCount="40" Mode="NumericPages" CssClass="DataGrid_PagerStyle"></PagerStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Recorded Date Time">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DispositionDate", "{0:G}") %>' ForeColor="Black" OnCheckedChanged="CB_CheckedChanged" Width="200" CssClass="selectCheckBox"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Date">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox19" runat="server" OnTextChanged="Textbox_TextChanged" Width="100px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "ShiftStartDate", 1, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Shift">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="WebInputBox1" runat="server" OnTextChanged="Textbox_TextChanged" Width="50px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "Shift", 2, "DispositionDate", "Int") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Rework Area">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox2" runat="server" OnTextChanged="Textbox_TextChanged" Width="120px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "ReworkArea", 3, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Team Member">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox3" runat="server" OnTextChanged="Textbox_TextChanged" Width="80px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "TeamMember", 4, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Lot Number">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox4" runat="server" OnTextChanged="Textbox_TextChanged" Width="120px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "LotNumber", 5, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Vehicle Line">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox5" runat="server" OnTextChanged="Textbox_TextChanged" Width="80px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "VehicleLine", 6, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Seat Style">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox6" runat="server" OnTextChanged="Textbox_TextChanged" Width="80px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "SeatStyle", 7, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Color">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox7" runat="server" OnTextChanged="Textbox_TextChanged" Width="60px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "Color", 8, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Component">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox8" runat="server" OnTextChanged="Textbox_TextChanged" Width="80px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "Component", 9, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Seat Serial Number">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox9" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "SeatSerialNumber", 10, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Category">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox10" runat="server" OnTextChanged="Textbox_TextChanged" Width="150px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "Category", 11, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Part Number">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox11" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "PartNumber", 12, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Disposition Reason">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox12" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "DispositionReason", 13, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Charge To">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox13" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "ChargeTo", 14, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Return Quantity">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox14" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "ReturnQuantity", 15, "DispositionDate", "Int") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Scrap Quantity">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox15" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "ScrapQuantity", 16, "DispositionDate", "Int") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Total Quantity">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox16" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "TotalQuantity", 17, "DispositionDate", "Int") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Location">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox17" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "Location", 18, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Part Description">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox18" runat="server" OnTextChanged="Textbox_TextChanged" Width="140px" CssClass="textEntry"></GGS:WebInputBox><%# BindTB(Container, "PartDescription", 19, "DispositionDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False" HeaderText="ID">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DispositionDate", "{0:G}") %>' ForeColor="Black">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogDelete" title="Delete Rework History">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to delete the selected items. Do you wish to continue?
        </p>
    </div>
</asp:Content>
