<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ErrorHistory.aspx.vb" Inherits="LTMS_Master.ErrorHistory" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.maskedinput-1.3.1.min.js") %>"></script>
    <script type="text/javascript">
        var $cmdRefresh;
        var $txtFromDate;
        var $txtToDate;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            CacheControls();

            CreateDatePickers();

            AddDropDownAndButtonEvents();

            CreateDialog_AdditionalInfo();

            createAnchorTagClickEvent();
        }


        function CacheControls() {
            $cmdRefresh = $('#MainContent_cmdRefresh');
            $txtFromDate = $('#MainContent_txtFromDate');
            $txtToDate = $('#MainContent_txtToDate');
            
        }

        function createAnchorTagClickEvent() {
            // for each in the lot trace datagrid item add an event to the anchor tag for the popup
            $('a[id^="lotAnchor_"]').click(function () {
                var thisAnchor = $(this);                
                // Pull the data for the line item from the attributes and populate the fields in the popup dialog.                                
                $('#MainContent_txtMSG').text(thisAnchor.attr('data-eMSG'));
                $('#MainContent_txtInfo').text(thisAnchor.attr('data-aInfo'));
                $('#divDlgViewErrorHistory').dialog('open');

                return false;
            })
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



        //------------------------------------------------------------
        // Error Dialog			
        //------------------------------------------------------------
        function CreateDialog_AdditionalInfo() {
            var $dlgDiv = $('#divDlgViewErrorHistory');

            $dlgDiv.modalDialog({ width: 410, 
                                  height: 500,
                                  clearInputOnOpen: false,
                                  buttons: { "Ok": function () { $(this).dialog("close"); } }
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
                <input id="txtSortField" type="hidden" value="DT1 DESC" name="txtSortField" runat="server" />
                <table id="Table1" class="tableCenter">
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 143px">
                            <asp:Label ID="Label4" runat="server" CssClass="h3 spanLabel">Date Range:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" Width="100" CssClass="textEntry" AutoPostBack="True"></asp:TextBox>
                            <asp:Label ID="Label5" runat="server" style="padding-left: 20px" CssClass="h3 spanLabel">To:</asp:Label>
                            <asp:TextBox ID="txtToDate" runat="server" Width="100" CssClass="textEntry" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="inputButton" style="margin-left: 10px">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 143px">
                            <asp:Label ID="Label3" runat="server" CssClass="h3 spanLabel">Display Count:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDisplayCount" runat="server" CssClass="selectDropDown" AutoPostBack="True">
                                <asp:ListItem Value="1000" Selected="True">TOP 1000</asp:ListItem>
                                <asp:ListItem Value="5000">TOP 5000</asp:ListItem>
                                <asp:ListItem Value="10000">TOP 10000</asp:ListItem>
                                <asp:ListItem Value="100 PERCENT">ALL</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 143px">
                            <asp:Label ID="Label2" runat="server" CssClass="h3 spanLabel">Application(s):</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlApplications" runat="server" CssClass="selectDropDown" AutoPostBack="True">
                            </asp:DropDownList>
                             <asp:Button ID="cmdTogglePaging" runat="server" Text="Show All" CssClass="inputButton">
                            </asp:Button>
                        </td>
                        <%--<td>
                            <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="inputButton">
                            </asp:Button>
                        </td>--%>
                        <td style="text-align: right;">
                           <%-- <asp:Button ID="cmdTogglePaging" runat="server" Text="Show All" CssClass="inputButton">
                            </asp:Button>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="datagrid">
                <asp:DataGrid ID="dgDetailGrid" runat="server" CellPadding="4" AllowSorting="True"
                    AutoGenerateColumns="False" AllowPaging="True" PageSize="15">
                    <SelectedItemStyle CssClass="DataGrid_SelectedItemStyle"></SelectedItemStyle>
                    <AlternatingItemStyle CssClass="DataGrid_AlternatingItemStyle"></AlternatingItemStyle>
                    <ItemStyle CssClass="DataGrid_ItemStyle"></ItemStyle>
                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                    <FooterStyle CssClass="DataGrid_FooterStyle"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn DataField="RecordedDT" SortExpression="RecordedDT" HeaderText="Recorded DT">
                            <ItemStyle Wrap="false"/>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="AppTitle" SortExpression="AppTitle" HeaderText="Module Name">
                            <ItemStyle Wrap="false"/>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ErrDescription" SortExpression="ErrorDescription" HeaderText="Error Description">
                            <ItemStyle Wrap="True" Width="600px"/>
                        </asp:BoundColumn>                      
                       <asp:BoundColumn DataField="More Info" SortExpression="More Info" HeaderText="More Info">
                            <ItemStyle Wrap="True" Width="75px"/>
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle CssClass="DataGrid_PagerStyle" Mode="NumericPages">
                    </PagerStyle>
                </asp:DataGrid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDlgViewErrorHistory" title="More Info" class="displayNone">
        <table id="Table4" class="tableCenter" style="width: 380px">
            <tr>
                <td style="text-align: center;">
                    <asp:Label ID="Label1" runat="server" CssClass="spanLabel h3">Error Msg</asp:Label>
                </td>                
            </tr>
            <tr>
            <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtMSG" runat="server" Width="370px" CssClass="text" 
                        TextMode="MultiLine" Height="80px"></asp:textbox>
            </td>  
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Label ID="Label18" runat="server" CssClass="spanLabel h3">Error Info</asp:Label>
                </td>                
            </tr>
            <tr>
             <td style="vertical-align: middle; padding-left: 10px; position: relative;">                    
                    <asp:textbox ID="txtInfo" runat="server" Width="370px" CssClass="text" 
                        TextMode="MultiLine" Height="225px"></asp:textbox>
                </td>
            </tr>           
                
        </table>               
    </div>
</asp:Content>
