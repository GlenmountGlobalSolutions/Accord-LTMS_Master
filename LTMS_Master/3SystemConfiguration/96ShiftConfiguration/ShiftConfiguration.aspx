<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ShiftConfiguration.aspx.vb" Inherits="LTMS_Master.ShiftConfiguration" %>

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
                control: $cmdButton,
                width: 500
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
                width: 500,
                validationFunction: function () { return ValidateDialog_New(); }
            });
        }

        function ValidateDialog_New() {
            var bValid = false;
            try {
                bValid = checkText($('#MainContent_tbShiftNumber'), 'Enter a value for the new Shift Number.', $('#helpShiftNumber'));
                bValid = checkText($('#MainContent_tbDesc'), 'Enter a description for the new Shift.', $('#helpDescription')) && bValid;
                bValid = checkText($('#MainContent_tbStart'), 'Enter a Start Time for the new Shift.', $('#helpStartTime')) && bValid;
                bValid = checkText($('#MainContent_tbEnd'), 'Enter a End Time for the new Shift.', $('#helpEndTime')) && bValid;
            }
            catch (err) {
                alert(Err);
            }
            return bValid;
        }

    </script>
    <style type="text/css">
        #divMainContent span.ui-icon, #divDialogNew span.ui-icon
        {
            float: left;
            padding-right: 5px;
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
            padding-top: 12px;
            padding-bottom: 12px;
            width:100%;
        }
        #divMainCenterPanel
        {
            width: 85%;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="Add new shift" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="Save changed data" Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="Delete selected row" Text="Delete"></asp:Button>
                    <asp:Button ID="cmdCopy" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="Copy selected row" Text="Copy"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="divWithScroll">
                <div class="datagrid">
                    <asp:DataGrid ID="dgMain" runat="server" AutoGenerateColumns="False" AllowPaging="False" PagerStyle-Position="Bottom" PagerStyle-PageButtonCount="40" PagerStyle-Mode="NumericPages" PagerStyle-CssClass="DataGrid_PagerStyle" HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle" AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle" EditItemStyle-CssClass="DataGrid_EditItemStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Select Shift">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox6" runat="server" CssClass="selectCheckBox" Text='<%# DataBinder.Eval(Container, "DataItem.ShiftID") %>' OnCheckedChanged="CB_CheckedChanged"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Shift Number">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="WebInputBox1" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "ShiftNumber", 1, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Description">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox13" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Description", 2, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Shift Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox2" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "ShiftStartTime", 3, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Shift End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox3" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "ShiftEndTime", 4, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 1 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox4" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour1StartTime", 5, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 1 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox5" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour1EndTime", 6, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 1 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox8" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour1PlannedProduction", 7, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 2 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox6" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour2StartTime", 8, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 2 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox7" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour2EndTime", 9, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 2 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox9" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour2PlannedProduction", 10, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 3 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox10" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour3StartTime", 11, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 3 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox11" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour3EndTime", 12, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 3 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox12" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour3PlannedProduction", 13, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 4 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox14" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour4StartTime", 14, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 4 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox15" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour4EndTime", 15, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 4 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox16" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour4PlannedProduction", 16, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 5 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox17" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour5StartTime", 17, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 5 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox18" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour5EndTime", 18, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 5 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox19" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour5PlannedProduction", 19, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 6 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox20" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour6StartTime", 20, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 6 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox21" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour6EndTime", 21, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 6 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox22" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour6PlannedProduction", 22, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 7 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox23" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour7StartTime", 23, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 7 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox24" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour7EndTime", 24, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 7 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox25" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour7PlannedProduction", 25, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 8 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox26" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour8StartTime", 26, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 8 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox27" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour8EndTime", 27, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 8 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox28" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour8PlannedProduction", 28, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 9 Start Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox29" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour9StartTime", 29, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 9 End Time">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox30" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour9EndTime", 30, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Hour 9 Planned Production">
                                <ItemTemplate>
                                    <GGS:WebInputBox ID="Webinputbox31" runat="server" CssClass="textEntry" OnTextChanged="Textbox_TextChanged" Width="100px"></GGS:WebInputBox>
                                    <%# BindTB(Container, "Hour9PlannedProduction", 31, "ShiftID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False" HeaderText="ID">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" CssClass="spanLabel" Text='<%# DataBinder.Eval(Container, "DataItem.ShiftID") %>'>
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
    <div id="divDialogNew" title="Add New Production Shift">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table1">
            <tr>
                <td style="width: 200px">
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label1" runat="server" Width="100px" CssClass="spanLabel">Shift Number:</asp:Label>
                </td>
                <td style="height: 15px">
                    <asp:TextBox ID="tbShiftNumber" runat="server" CssClass="textEntry" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <div id="helpShiftNumber" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label3" runat="server" Width="100px" CssClass="spanLabel">Description:</asp:Label>
                </td>
                <td style="height: 15px">
                    <asp:TextBox ID="tbDesc" runat="server" CssClass="textEntry" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <div id="helpDescription" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label4" runat="server" Width="100px" CssClass="spanLabel">Start Time:</asp:Label>
                </td>
                <td style="height: 15px">
                    <asp:TextBox ID="tbStart" runat="server" CssClass="textEntry" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <div id="helpStartTime" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                    <asp:Label ID="Label5" runat="server" Width="100px" CssClass="spanLabel">End Time:</asp:Label>
                </td>
                <td style="height: 15px">
                    <asp:TextBox ID="tbEnd" runat="server" CssClass="textEntry" Width="200px"></asp:TextBox>
                </td>
                <td>
                    <div id="helpEndTime" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Production Shift">
        <p class="pCenter">
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span><span class="spanLabel">You are about to Delete the selected Production Shift. Do you wish to continue?</span>
        </p>
    </div>
</asp:Content>
