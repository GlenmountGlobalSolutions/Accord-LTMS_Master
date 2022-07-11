<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="DebugHistory.aspx.vb" Inherits="LTMS_Master.DebugHistory" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.maskedinput-1.3.1.min.js") %>"></script>
    <script type="text/javascript">
        var $cmdRefresh;
        var $cmdExport;
        var $txtFromDate;
        var $txtToDate;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            CacheControls();

            CreateDatePickers();

            AddDropDownAndButtonEvents();

            AddExportEvent();
        }

        function CacheControls() {
            $cmdRefresh = $('#MainContent_cmdRefresh');
            $cmdExport = $('#MainContent_cmdExport');
            $txtFromDate = $('#MainContent_txtFromDate');
            $txtToDate = $('#MainContent_txtToDate');
        }

        function CreateDatePickers() {
            var testDate;
            var d1;

            $txtFromDate.mask('99/99/9999').addDatePicker()
                        .datepicker("option", "firstDay", 1)
                        .datepicker({
                            onSelect: function (dateText, inst) {
                                DoRefreshPostBack(dateText);
                            }
                        })
			            .change(function () {           // if the edit box is changed
			                DoRefreshPostBack(this.value);
			            });

            $txtToDate.mask('99/99/9999').addDatePicker()
                        .datepicker("option", "firstDay", 1)
                        .datepicker({
                            onSelect: function (dateText, inst) {
                                DoRefreshPostBack(dateText);
                            }
                        })
			            .change(function () {           // if the edit box is changed
			                DoRefreshPostBack(this.value);
			            });

            PositionCalendarPickerIcon();   //function found in '\Scripts\aesthetics.js'
        }

        function DoRefreshPostBack(dateText) {
            d1 = Date.parse(dateText);
            if (d1 != null) {
                ShowWaitOverlay();
                __doPostBack($cmdRefresh.attr("id"), '');
            }
        }

        function AddDropDownAndButtonEvents() {
            $("select").change(function () { ShowWaitOverlay(); });
            $cmdRefresh.click(function () { ShowWaitOverlay(); });
            $("#MainContent_cmdTogglePaging").click(function () { ShowWaitOverlay(); });
        }

        function AddExportEvent() {
            $cmdExport.on('click', function () {
                var args = "strCount=" + encodeURIComponent($("#MainContent_ddlDisplayCount").val());
                args += "&strFromDate=" + encodeURIComponent($txtFromDate.val());
                args += "&strToDate=" + encodeURIComponent($txtToDate.val());
                args += "&strApplicationID=" + encodeURIComponent($("#MainContent_ddlApplications").val());
                args += "&strSortField=" + encodeURIComponent($("#MainContent_hidSortField").val());

                window.open('DebugHistoryExport.aspx?' + args);
                return false;
            });

        }
    </script>
    <style type="text/css">
        #divMainContent span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        
        .datagrid
        {
            margin: 20px 8px 0px 8px;
        }
        
        input.inputButton
        {
            margin-top: 0px;
        }
        .selectDropDown
        {
            width: 180px;
        }
        
        [type=text]
        {
            width: 90px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divCenterPanel" class="divCenter">
                <asp:HiddenField ID="hidSortField" runat="server" Value="" />
                <table id="Table1" class="tableCenter">
                    <tr>
                        <td style="width: 130px">
                            <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel h3">Date Range:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="h3"></asp:TextBox>
                        </td>
                        <td>
                            <span class="spanLabel h3">To:</span>
                        </td>
                        <td style="width: 126px">
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="h3"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="inputButton">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel h3">Display
                                Count:</span>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlDisplayCount" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                                <asp:ListItem Value="1000" Selected="True">TOP 1000</asp:ListItem>
                                <asp:ListItem Value="5000">TOP 5000</asp:ListItem>
                                <asp:ListItem Value="10000">TOP 10000</asp:ListItem>
                                <asp:ListItem Value="100 PERCENT">ALL</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel h3">Application(s):</span>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlApplications" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="cmdTogglePaging" runat="server" Text="Show All" CssClass="inputButton">
                            </asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="cmdExport" runat="server" Text="Export" CssClass="inputButton"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="datagrid">
                <asp:DataGrid ID="dgDetailGrid" runat="server" AutoGenerateColumns="False" CssClass="borderRadius"
                    HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle"
                    AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle" EditItemStyle-CssClass="DataGrid_EditItemStyle"
                    PagerStyle-CssClass="DataGrid_PagerStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle"
                    ItemStyle-CssClass="DataGrid_ItemStyle" AllowSorting="True" AllowPaging="True"
                    PagerStyle-Mode="NumericPages" PageSize="15">
                    <Columns>
                        <asp:BoundColumn DataField="DateTime" SortExpression="DateTime" HeaderText="Date Time">
                            <ItemStyle Wrap="false" Width="130px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="UserName" SortExpression="UserName" HeaderText="User">
                            <ItemStyle Wrap="false" Width="100px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ScreenName" SortExpression="ScreenName" HeaderText="Screen Name">
                            <ItemStyle Wrap="false" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ActionPerformed" SortExpression="ActionPerformed" HeaderText="Action Performed">
                            <ItemStyle Wrap="true" Width="130px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="UserHostAddress" SortExpression="UserHostAddress" HeaderText="Client IP">
                            <ItemStyle Wrap="false" Width="90px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
