<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DailyBuildQuantities.aspx.vb" Inherits="LTMS_Master.DailyBuildQuantities" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.alphanum.js") %>"></script>
    <script type="text/javascript">
        var $hidRad1LastTab;
        var $hidRad2LastTab;
        var $hidDailyBuildDate;
        var $orderType;
        var $ddlMonth;
        var $txtBeginDate;
        var $txtEndDate;
        var $cmdPrePopulate;
        var $cmdEdit;
        var $dlgSecurity;
        var $dlgPrePopulate;
        var $dlgEdit;

        function contentPageLoad(sender, args) {
            try {
                AddAJAXSettings();
                CacheControls();

                MakeDivIntoTabs();
                CreateButtonset();
                
                AddCalendarFormatting();
                AddjQueryUIFormatToCalendar();

                AddDialog_Security();
                AddDialog_Edit();
                AddDialog_PrePopulate();

                AddEventToOpenEditDialog();
                AddEventToOpenPrePopulateDialog();

            } catch (err) {
                alert(err);
            }
        }

        function CacheControls() {
            try {
                $dlgSecurity = $("#divDlg_Security");
                $dlgEdit = $("#divDlg_Edit");
                $dlgPrePopulate = $('#divDlg_PrePopulate');
                $hidRad1LastTab = $("#MainContent_hidRad1LastTab");
                $hidRad2LastTab = $("#MainContent_hidRad2LastTab");
                $hidDailyBuildDate = $("#MainContent_hidDailyBuildDate");
                $orderType = $('#MainContent_rbOrders input:checked').val();
                $ddlMonth = $('#MainContent_ddlMonth');
                $cmdPrePopulate = $('#MainContent_cmdPrePop');
                $cmdEdit = $('#MainContent_cmdEdit');
                $txtBeginDate = $('#MainContent_txtBegin');
                $txtEndDate = $('#MainContent_txtEnd');
            } catch (err) {
                alert(err);
            }
        }

        function CreateButtonset() {
            // Make the Radio button a UI radio button
            $("#MainContent_rbOrders").buttonset();
            $("#MainContent_rbOrders_0 + label").mouseover(function () { if ($orderType == "1") { $(this).css('cursor', 'text'); } else { $(this).css('cursor', 'pointer').click(function () { $(this).css('cursor', 'progress'); }); } });
            $("#MainContent_rbOrders_1 + label").mouseover(function () { if ($orderType == "0") { $(this).css('cursor', 'text'); } else { $(this).css('cursor', 'pointer').click(function () { $(this).css('cursor', 'progress'); }); } });
        }

        function MakeDivIntoTabs() {

            $("#tabPrePop").tabs({
                active: $hidRad1LastTab.val(),    //set to previously Selected Tab (for postback)

                beforeActivate: function (event, ui) {
                    $hidRad1LastTab.val(ui.newTab.index()); // when the tabs are selected set the hidden field
                    RefreshPrePop();
                }
            });

            $("#tabEdit").tabs({
                active: $hidRad2LastTab.val(),    //set to previously Selected Tab (for postback)

                beforeActivate: function (event, ui) {
                    $hidRad2LastTab.val(ui.newTab.index()); // when the tabs are selected set the hidden field
                    RefreshEditBreakdown();
                    //  RefreshEditTotals();
                }
            });
        }

        function AddCalendarFormatting() {
            //Add event to previous and next week icons to change cursor over calendar button
            $("input:image, .calendarCellDiv").mouseover(function () { $(this).css('cursor', 'pointer'); });

            $txtBeginDate.addDatePicker().datepicker("option", "firstDay", 1);  //Set the first day of the week: Sunday is 0, Monday is 1, ... 
            $txtEndDate.addDatePicker().datepicker("option", "firstDay", 1);
            PositionCalendarPickerIcon();

            // add wait cursor when month navigation buttons are pressed.
            $('[id^="MainContent_ib"]').on('click', function () { ShowCursor_Wait(); }).mouseover(function () { $(this).css('cursor', 'pointer'); });
            $('.cmdShowWait').on('click', function () { ShowWaitOverlay(); });

            // make controls with class 'numericOnly' into numeric only
            $('.numericOnly').numeric({ allowMinus: false, allowThouSep: false, allowDecSep: false });
        }

        function AddDialog_Edit() {
            $dlgEdit.modalDialog({
                control: $cmdEdit,
                addButton_Cancel: false,
                width: 920,
                resizable: true,
                clearInputOnOpen: false,
                create: function () { $(this).keypress(function (e) { if (e.keyCode == $.ui.keyCode.ENTER) { return false; } }) },
                open: function () { $("#tabEdit").tabs("option", "active", 0); }
            });
        }

        function AddEventToOpenEditDialog() {
            // add to all DIV's with class 'calendarCellDiv'
            $('.calendarCellDiv').click(function () {
                var $this = $(this);
                if ($cmdEdit.is(":disabled")) {
                    $divDlgSecurity.dialog('open');
                } else {
                    $dlgEdit.dialog('open');
                }
                var prodDate = $('#lblDate', $this).text();
                $('#MainContent_lblDlgDate', $dlgEdit).text(prodDate);
                $hidDailyBuildDate.val(prodDate);
                RefreshEditBreakdown();
                //  RefreshEditTotals();
                return false;
            });

        }

        function AddDialog_PrePopulate() {
            try {
                $dlgPrePopulate.modalDialog({
                    control: $cmdPrePopulate,
                    width: 600,
                    height: 740,
                    resizable: true,
                    clearInputOnOpen: false,
                    create: function () { $(this).keypress(function (e) { if (e.keyCode == $.ui.keyCode.ENTER) { return false; } }) },
                    open: function () { $("#tabPrePop").tabs("option", "active", 0); RefreshPrePop();  }
                });
                $('.dialog').dialogButtons('Cancel', 'enabled');
            }
            catch (err) {
                alert(err);
            } 
        }

        function AddEventToOpenPrePopulateDialog() {
            try {

                $('#MainContent_cmdPrePop').click(function () {
                    $dlgPrePopulate.dialog('open');

                    d1 = Date.parse($ddlMonth.val());
                    if (d1 != null) {
                        $txtBeginDate.val(d1.toString("MM/dd/yyyy"));
                        $txtEndDate.val(d1.toString("MM/dd/yyyy"));
                    }

                    $("#MainContent_cblDays_0").attr('checked', true)
                    $("#MainContent_cblDays_1").attr('checked', true)
                    $("#MainContent_cblDays_2").attr('checked', true)
                    $("#MainContent_cblDays_3").attr('checked', true)
                    $("#MainContent_cblDays_4").attr('checked', true)
                    $("#MainContent_cblDays_5").attr('checked', true)
                    $("#MainContent_cblDays_6").attr('checked', true)

                    return false;
                });
            } catch (err) {
                alert(err);
            }
        }

        function RefreshEditTotals() {
            var masterTableView = $find("<%=rgEditTotals.ClientID%>").get_masterTableView();
            if (masterTableView !== null) { masterTableView.rebind(); }
        }

        function RefreshEditBreakdown() {
            var masterTableView = $find("<%=rgEditBreakdown.ClientID%>").get_masterTableView();
            if (masterTableView !== null) { masterTableView.rebind(); }
        }

        function RefreshPrePop() {
            var masterTableView = $find("<%=rgPrePop.ClientID%>").get_masterTableView();
            if (masterTableView !== null) { masterTableView.rebind(); }
        }

        function AddDialog_Security() {
            $dlgSecurity.modalDialog({ buttons: [{ text: "Ok", click: function () { $(this).dialog("close"); } }] });
        }

        function rowDblClick(sender, eventArgs) {
            sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
        }
        function rowDblClickEdit(sender, eventArgs) {
            var grid = sender;
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
            var cell = MasterTable.getCellByColumnUniqueName(row, "EditCommandColumn");
            var value = cell.innerHTML;

            if (value.indexOf("Disabled") !== -1) {
                eventArgs._domEvent.preventDefault();
            } else {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }
        }

        function OnClientSelectedIndexChanged_EndLot(sender, eventArgs) {
            //            var txt = sender.get_text();
            //            var val = txt.substring(txt.lastIndexOf(" : ")+3);
            var row = Telerik.Web.UI.Grid.GetFirstParentByTagName(sender.get_element(), "tr");
            $telerik.findElement(row, "TB_EndLotProducedJobQuantity").value = '30';
            //            $telerik.findElement(row, "TB_EndLotProducedJobQuantity").value = val;
        }
        function OnClientSelectedIndexChanged_LotShipped(sender, eventArgs) {
            //            var txt = sender.get_text();
            //            var val = txt.substring(txt.lastIndexOf(" : ") + 3);
            var row = Telerik.Web.UI.Grid.GetFirstParentByTagName(sender.get_element(), "tr");
            $telerik.findElement(row, "TB_PlannedShipQuantity").value = '30';
            //            $telerik.findElement(row, "TB_PlannedShipQuantity").value = val;
        }

    </script>
    <style type="text/css">
		#divDlg_Edit .selectDropDown
		#divDlg_PrePopulate .selectDropDown
		{
			width: 126px;
		}
        div.rcbNoPad .rcbInput
        {
            padding-left: 0px !important;
        }
          div.NoPadding .rgRow td.fillerCol
        , div.NoPadding .rgAltRow td.fillerCol
        , div.NoPadding .rgEditRow td.fillerCol
        , div.NoPadding .rgHeader .fillerCol
        , div.NoPadding .rgFooter .fillerCol
        {
            padding: 0px;
        }
        
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<telerik:RadAjaxManager ID="RadAjaxManager" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rgEditBreakdown">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rgEditTotals" LoadingPanelID="RadAjaxLoadingPanel1" />
                <telerik:AjaxUpdatedControl ControlID="rgEditBreakdown" LoadingPanelID="RadAjaxLoadingPanel2" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="rgPrePop">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rgPrePop" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7"></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Windows7"></telerik:RadAjaxLoadingPanel>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<div>
				<input id="hidDailyBuildDate" type="hidden" runat="server" />
    			<input id="hidRad1LastTab" type="hidden" runat="server" />
    			<input id="hidRad1Shift" type="hidden" runat="server" />
    			<input id="hidRad2LastTab" type="hidden" runat="server" />
    			<input id="hidRad2Shift" type="hidden" runat="server" />
			</div>

			<table id="Table1" style="width: 1024px;" >  
				<tr>
					<td style="width: 240px;">
						<asp:RadioButtonList ID="rbOrders" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" CssClass="inputRadioButton">
							<asp:ListItem Value="1" Selected="True">Setex Orders</asp:ListItem>
							<asp:ListItem Value="0">All Orders</asp:ListItem>
						</asp:RadioButtonList>
					</td>
					<td style="width: 60px;">
						<asp:Label ID="Label1" runat="server" CssClass="spanLabel h3">Month</asp:Label>
					</td>
					<td style="width: 14px;">
						<asp:ImageButton ID="ibPrevMo" runat="server" ImageUrl="~/images/Arrows/bbPrevpage.gif"></asp:ImageButton>
					</td>
					<td style="width: 160px; text-align: center;">
						<asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="True" CssClass="selectDropDown" Width="140px">
						</asp:DropDownList>
					</td>
					<td style="width: 14px;">
						<asp:ImageButton ID="ibNextMo" runat="server" ImageUrl="~/images/Arrows/bbNextpage.gif"></asp:ImageButton>
					</td>
					<td style="width: 100px; text-align: center;">
						<asp:Button ID="cmdRefresh" runat="server" CssClass="inputButton cmdShowWait" Text="Refresh"></asp:Button>
					</td>
					<td style="width: 120px; text-align: center;">
						<asp:Button ID="cmdPrePop" runat="server" Text="Pre-Populate" Width="100px" CssClass="inputButton"></asp:Button>
						<asp:Button ID="cmdEdit" runat="server" Text="Edit Day" CssClass="inputButton" Style="display: none"></asp:Button>
					</td>
					<td>
						<asp:Label ID="Label2" runat="server" Visible="False" CssClass="spanLabel">Last Recorded "End Lot"</asp:Label>
						<asp:Label ID="lblEndLot" runat="server" Visible="False" CssClass="spanLabel">?</asp:Label>
						<table class="tblBP">
							<tr>
								<td><asp:Panel ID="Panel1" runat="server" CssClass="divBP" Visible="True" ><asp:Image ID="image1" runat="server" CssClass='calendarFilterImgBP1' AlternateText="1"></asp:Image><asp:Label ID="lblBP1" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
								<td><asp:Panel ID="Panel3" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image3" runat="server" CssClass='calendarFilterImgBP2' AlternateText="2" ></asp:Image><asp:Label ID="lblBP3" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
							</tr>
							<tr>
								<td><asp:Panel ID="Panel2" runat="server" CssClass="divBP" Visible="True"><asp:Image ID="image2" runat="server" CssClass='calendarFilterImgBP3' AlternateText="3" ></asp:Image><asp:Label ID="lblBP2" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
								<td><asp:Panel ID="Panel4" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image4" runat="server" CssClass='calendarFilterImgBP4' AlternateText="4" ></asp:Image><asp:Label ID="lblBP4" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="8">
						<div id="divCalendar" class="calendarBorderAndRadius">
							<asp:Calendar ID="calDailyBuild" runat="server" CssClass="calendarStyle" EnableViewState="true" UseAccessibleHeader="False" 
                            DayNameFormat="Full" FirstDayOfWeek="Monday" ShowNextPrevMonth="False" ShowTitle="True" 
                            DayStyle-CssClass="calendarDayStyle" DayHeaderStyle-CssClass="calendarDayHeaderStyle" 
                            OtherMonthDayStyle-CssClass="calendarOtherMonthDayStyle" SelectedDayStyle-CssClass="calendarSelectedDayStyle" 
                            SelectorStyle-CssClass="calendarSelectorStyle" TitleStyle-CssClass="calendarTitleStyle" TodayDayStyle-CssClass="calendarTodayDayStyle" 
                            WeekendDayStyle-CssClass="calendarWeekendDayStyle" BorderStyle="None" TitleStyle-BackColor="#EEEEEE" CellPadding="0"></asp:Calendar>
						</div>
					</td>
				</tr>
			</table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="divDlg_Edit" title="Edit Build Qauntity">
        <div>
            <table style="text-align: left;" class="tableCenter">
			    <tr>
				    <td>
					    <asp:Label ID="Label11" runat="server" CssClass="spanLabel h2" >Date: </asp:Label>
				    </td>
				    <td>
					    <asp:Label ID="lblDlgDate" runat="server" CssClass="spanLabel h2" text=""></asp:Label>
				    </td>
			    </tr>
            </table>
        </div>
		<table class="tableBorderCollapse" style="margin-left:10px; margin-top: 10px; " >
			<tr>						
				<td>
                    <span class="spanLabel h3">Summarized Build Orders Total For The Day</span>
                </td>
            </tr>
			<tr>						
				<td>
                    <telerik:RadGrid ID="rgEditTotals" runat="server" RenderMode="Lightweight" AutoGenerateColumns="False"
                        CssClass="NoPadding " DataSourceID="SqlDataSourceEditDay" 
                        AllowAutomaticUpdates="True"
                        ShowFooter="True"
                        OnItemDataBound="rgEditTotals_OnItemDataBound"
                        OnUpdateCommand="rgEditTotals_OnUpdateCommand" 
                        Skin="Windows7" >
                        <ClientSettings >
                            <ClientEvents OnRowDblClick="rowDblClickEdit" />
                        </ClientSettings>
                        <MasterTableView DataKeyNames="ProductionDate, SetexSchedule, BroadcastPointID" DataSourceID="SqlDataSourceEditDay" EditMode="InPlace" runat="server">
                            <Columns>

                                <telerik:GridTemplateColumn UniqueName="BroadcastPointDescription" HeaderText="Honda Line Number" ReadOnly="true" >
                                    <ItemTemplate>
                                        <img src="../../Images/Misc/<%#DataBinder.Eval(Container.DataItem, "ImageName")%>"
                                            class="prePopImgBP1" alt='*' />
                                        <%#DataBinder.Eval(Container.DataItem, "Description")%>
                                    </ItemTemplate>
                                    <FooterTemplate>Shift Totals</FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="220px" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="220px" />
                                    <FooterStyle HorizontalAlign="Right" VerticalAlign="Bottom" />
                                </telerik:GridTemplateColumn>
                
                                <telerik:GridBoundColumn DataField="QuantityShift1" DataType="System.Int16" 
                                    HeaderText="1st Shift" SortExpression="QuantityShift1" UniqueName="QuantityShift1" ReadOnly="True" 
                                    Aggregate="Sum" FooterAggregateFormatString="{0}">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Bottom"></FooterStyle>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="QuantityShift2" DataType="System.Int16" 
                                    HeaderText="2nd Shift" SortExpression="QuantityShift2" UniqueName="QuantityShift2" ReadOnly="True" 
                                    Aggregate="Sum" FooterAggregateFormatString="{0}">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Bottom"></FooterStyle>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="QuantityShift3" DataType="System.Int16" 
                                    HeaderText="3rd Shift" SortExpression="QuantityShift3" UniqueName="QuantityShift3" ReadOnly="True" 
                                    Aggregate="Sum" FooterAggregateFormatString="{0}">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Bottom"></FooterStyle>
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridTemplateColumn UniqueName="Filler" HeaderText="" ReadOnly="true" >
                                    <ItemTemplate></ItemTemplate>
                                    <HeaderStyle CssClass="fillerCol" />
                                    <ItemStyle CssClass="fillerCol" />
                                    <FooterStyle CssClass="fillerCol" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="QuantityLineTotal" DataType="System.Int16" 
                                    HeaderText="Line</br>Totals" SortExpression="QuantityLineTotal" UniqueName="QuantityLineTotal" ReadOnly="True" 
                                    Aggregate="Sum" FooterAggregateFormatString="{0}">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Bottom"></FooterStyle>
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn UniqueName="GridTemplateColumnEndLotProduced" HeaderText="End Lot </br> Produced" DataField="EndLotProduced">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "EndLotProducedText")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight"  ID="RadComboBoxEndLotProduced" runat="server"
                                            EnableLoadOnDemand="true" AutoPostBack="true"
                                            DataSourceID="SqlDataSourceEndLotProduced" 
                                            DataTextField="TEXT" 
                                            DataValueField="VALUE"
                                            CssClass="selectDropDown rcbNoPad" 
                                            OnClientSelectedIndexChanged="OnClientSelectedIndexChanged_EndLot"
                                            OnItemsRequested="RadComboBoxEndLotProduced_ItemsRequested"
                                            MarkFirstMatch="False" 
                                            Width="150px" >
                                            <ExpandAnimation Type="OutBack" />
                                            <CollapseAnimation Type="InBack" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="170px" />
					                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"   />
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="EndLotProducedJobQuantity" DataType="System.Int16"
                                    HeaderText="End Lot Produced Job Qty" SortExpression="EndLotProducedJobQuantity"
                                    UniqueName="EndLotProducedJobQuantity" ColumnEditorID="QtyEditor_EditTotal"
                                    Aggregate="Sum" FooterAggregateFormatString="{0}">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Bottom"></FooterStyle>
                                </telerik:GridBoundColumn>
                
                                <telerik:GridTemplateColumn UniqueName="GridTemplateColumnEndLotShipped" HeaderText="End Lot </br> Shipped">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "EndLotShippedText")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight"  ID="RadComboBoxEndLotShipped" runat="server"
                                            EnableLoadOnDemand="true" AutoPostBack="true"
                                            DataSourceID="SqlDataSourceEndLotShipped" 
                                            DataTextField="TEXT" 
                                            DataValueField="VALUE"
                                            CssClass="selectDropDown rcbNoPad" 
                                            OnClientSelectedIndexChanged="OnClientSelectedIndexChanged_LotShipped"
                                            OnItemsRequested="RadComboBoxEndLotShipped_ItemsRequested"
                                            Width="150px">
                                            <ExpandAnimation Type="OutBack" />
                                            <CollapseAnimation Type="InBack" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="170px" />
					                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"    />
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="PlannedShipQuantity" DataType="System.Int16"
                                    HeaderText="Planned Ship Quantity" SortExpression="PlannedShipQuantity"
                                    UniqueName="PlannedShipQuantity" ColumnEditorID="QtyEditor_EditTotal"
                                    Aggregate="Sum" FooterAggregateFormatString="{0}">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                    <FooterStyle HorizontalAlign="Center" VerticalAlign="Bottom" Height="20px" ></FooterStyle>
                                </telerik:GridBoundColumn>
                
						    <telerik:GridEditCommandColumn>
							    <HeaderStyle />
							    <ItemStyle />
						    </telerik:GridEditCommandColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <telerik:GridTextBoxColumnEditor runat="server" ID="QtyEditor_EditTotal">
                        <TextBoxStyle Width="45px" />
                    </telerik:GridTextBoxColumnEditor>
			</td>
            </tr>
        </table>
		<table class="tableBorderCollapse" style="margin-left:10px; margin-top: 10px; width: 784px;" >
            <tr>						
				<td style="padding-top: 20px">
                    <span style="margin-left: 200px" class="spanLabel h3">Build Orders per Shift</span>
                </td>
            </tr>
			<tr>						
				<td>
                    <div style="padding-left:200px;" >
                        <div id="tabEdit" style="width:300px; padding-top:5px;" class="tabs">
				            <ul class="dbqTab">
					            <li><a href="#tabEdit-1">1st Shift</a></li>
					            <li><a href="#tabEdit-2">2nd Shift</a></li>
					            <li><a href="#tabEdit-3">3rd Shift</a></li>
				            </ul>
			                <div id="tabEdit-1" class="tabsDisplayNone"></div>
			                <div id="tabEdit-2" class="tabsDisplayNone"></div>
			                <div id="tabEdit-3" class="tabsDisplayNone"></div>
			            </div>
                        <telerik:RadGrid ID="rgEditBreakdown" runat="server" RenderMode="Lightweight" 
                            AutoGenerateColumns="False"
				            DataSourceID="SqlDataSourceDailyBuildQuantityEditBreakdown" 
                            AllowAutomaticDeletes="True" 
                            AllowAutomaticInserts="True"
				            AllowAutomaticUpdates="True" 
                            CssClass="NoPadding "
                            Skin="Windows7" 
                            ShowFooter="True"
                            OnRowDrop="rgEditBreakdown_RowDrop" 
                            OnItemDeleted="rgEditBreakdown_ItemDeleted"
                            OnItemUpdated="rgEditBreakdown_ItemUpdated"
                            OnItemInserted="rgEditBreakdown_ItemInserted"
                            >
				            <ClientSettings AllowRowsDragDrop="true" Selecting-AllowRowSelect="True" >
					            <ClientEvents OnRowDblClick="rowDblClick" />
					            <Selecting AllowRowSelect="True" EnableDragToSelectRows="false" />
				            </ClientSettings>
				            <MasterTableView DataKeyNames="BreakdownID, BroadcastPointID" 
                                DataSourceID="SqlDataSourceDailyBuildQuantityEditBreakdown" EditMode="InPlace"
					            CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemDisplay="Bottom">
                                <Columns>
						            <telerik:GridBoundColumn DataField="BreakdownID" FilterControlAltText="BreakdownID"
							            ReadOnly="True" UniqueName="BreakdownID" Visible="false">
						            </telerik:GridBoundColumn>

						            <telerik:GridBoundColumn DataField="DisplayID" FilterControlAltText="DisplayID"
							            ReadOnly="True" UniqueName="DisplayID" Visible="false">
							            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"  Width="40px"/>
						            </telerik:GridBoundColumn>
						
                                    <telerik:GridDragDropColumn >
                                        <HeaderStyle Width="40px" />
                                    </telerik:GridDragDropColumn>

						            <telerik:GridBoundColumn DataField="ShiftNumber" FilterControlAltText="ShiftNumber"
							            ReadOnly="True" UniqueName="ShiftNumber" HeaderText="Shift" Visible="True" >
							            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="40px"/>
							            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"  />
						            </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn UniqueName="BroadcastPointDescription" HeaderText="Honda Line Number">
                                        <ItemTemplate>
                                            <img src="../../Images/Misc/<%#DataBinder.Eval(Container.DataItem, "ImageName")%>" class="prePopImgBP1"  alt='*' />
                                            <%#DataBinder.Eval(Container.DataItem, "Description")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox RenderMode="Lightweight"  ID="RadComboBox2" runat="server"
                                                DataSourceID="SqlDataSourceBroadcastPoints" 
                                                DataTextField="Description" 
                                                DataValueField="BroadcastPointID"
                                                SelectedValue='<%#Bind("BroadcastPointID") %>'
                                                CssClass="selectDropDown"
                                                Width="220px"
                                                OnClientLoad="showSelectedItemImage"
                                                OnClientSelectedIndexChanging="showImageOnSelectedItemChanging"
                                                OnItemDataBound="RadComboBox1_ItemDataBound" >
                                                <ExpandAnimation Type="OutBack" />
                                                <CollapseAnimation Type="InBack" />
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>Shift Total</FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="220px" />
							            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"   Width="220px" />
                                        <FooterStyle HorizontalAlign="Right" VerticalAlign="Bottom"></FooterStyle>
                                    </telerik:GridTemplateColumn>

						            <telerik:GridBoundColumn DataField="Quantity" DataType="System.Int16" FilterControlAltText="Quantity"
							            HeaderText="Quantity" SortExpression="Quantity" UniqueName="Quantity" ColumnEditorID="QtyEditor_Breakdown" 
                                        Aggregate="Sum" FooterAggregateFormatString="{0}">
							            <HeaderStyle HorizontalAlign="Center"  Width="80px" />
							            <ItemStyle HorizontalAlign="Center"    />
							            <FooterStyle HorizontalAlign="Center" />
							            <ColumnValidationSettings EnableRequiredFieldValidation="True"></ColumnValidationSettings>
						            </telerik:GridBoundColumn>

						            <telerik:GridEditCommandColumn>
							            <HeaderStyle Width="70px" />
							            <ItemStyle HorizontalAlign="Right" />
						            </telerik:GridEditCommandColumn>
						            <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
							            <HeaderStyle Width="30px" />
						            </telerik:GridButtonColumn>
					            </Columns>
				            </MasterTableView>
			            </telerik:RadGrid>
                        <telerik:GridTextBoxColumnEditor runat="server" ID="QtyEditor_Breakdown">  
				            <TextBoxStyle Width="60px" /> 
			            </telerik:GridTextBoxColumnEditor>
					</div>
                </td>
			</tr>
		</table>
	</div>


    <div id="divDlg_Security" title="Insufficient Security Permissions">
		<p class="pCenter" >
			You do not have permission to make edits.
		</p>
	</div>

        <div id="divDlg_PrePopulate" title="Pre-Populate Daily Build Quantity" style="text-align: center;">
        <div>
		    <table id="Table2" style="text-align: left;" class="tableCenter">
			    <tr>
				    <td style="padding-left: 46px; ">
					    <asp:Label ID="Label3" runat="server" CssClass="spanLabel">Beginning Date</asp:Label>
				    </td>
				    <td>
					    <asp:TextBox ID="txtBegin" runat="server" CssClass="textEntry"></asp:TextBox>
				    </td>
			    </tr>
			    <tr>
				    <td style="padding-left: 66px;" >
					    <asp:Label ID="Label4" runat="server" CssClass="spanLabel">Ending Date</asp:Label>
				    </td>
				    <td>
					    <asp:TextBox ID="txtEnd" runat="server" CssClass="textEntry"></asp:TextBox>
				    </td>
			    </tr>
			    <tr>
				    <td colspan="2" style="padding-left: 40px; height: 60px;" class="h3">
					    <asp:CheckBoxList ID="cblDays" runat="server" RepeatDirection="Horizontal">
						    <asp:ListItem Value="M" Selected="True" Text="M"></asp:ListItem>
						    <asp:ListItem Value="Tu" Selected="True" Text="Tu"></asp:ListItem>
						    <asp:ListItem Value="We" Selected="True" Text="We"></asp:ListItem>
						    <asp:ListItem Value="Th" Selected="True" Text="Th"></asp:ListItem>
						    <asp:ListItem Value="Fr" Selected="True" Text="Fr"></asp:ListItem>
						    <asp:ListItem Value="Sa" Text="Sa"></asp:ListItem>
						    <asp:ListItem Value="Su" Text="Su"></asp:ListItem>
					    </asp:CheckBoxList>
				    </td>
			    </tr>
            </table>
        </div>
        <table id="Table3" style="text-align: left;" class="tableCenter" >
			<tr>
				<td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
		                <ContentTemplate>
					        <table class="tableBorderCollapse">
						        <tr>
							        <td style="width: 200px">&nbsp;</td>
							        <td style="width: 40px"><span class="spanLabel spanCenterBlock">1st</span><span class="spanLabel spanCenterBlock">Shift</span></td>
							        <td style="width: 40px"><span class="spanLabel spanCenterBlock">2nd</span><span class="spanLabel spanCenterBlock">Shift</span></td>
							        <td style="width: 40px"><span class="spanLabel spanCenterBlock">3rd</span><span class="spanLabel spanCenterBlock">Shift</span></td>
							        <td style="width: 40px"><span class="spanLabel spanCenterBlock">Day</span><span class="spanLabel spanCenterBlock">Total</span></td>
							        <td style="width: 40px"><span class="spanLabel spanCenterBlock">Shipping</span><span class="spanLabel spanCenterBlock">Qty</span></td>
							        <td style="width: 40px"><span class="spanLabel spanCenterBlock">Job</span><span class="spanLabel spanCenterBlock">Qty</span></td>
						        </tr>
						        <asp:Panel ID="prepopTopPanel1" runat="server" CssClass="divBP" Visible="True" Height="0px">
						        <tr>
							        <td><asp:Image ID="prePopImage1" runat="server" CssClass='prePopImgBP1' AlternateText="1" ></asp:Image><asp:Label ID="lblPrePopTop1" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label><asp:Label ID="lblBPID1" runat="server" Text="" CssClass="displayNone"></asp:Label></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShift1BP1" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShift2BP1" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center; padding-right:10px;"><asp:TextBox ID="txtPrePopShift3BP1" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopBuildQty1" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShipQty1" runat="server" CssClass="textEntry textCenter numericOnly prepopShipQty" Text="0" Width="40"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopProdQty1" runat="server" CssClass="textEntry textCenter numericOnl prepopProdQty" Text="0" Width="40"></asp:TextBox></td>
						        </tr>
						        </asp:Panel>
						        <asp:Panel ID="prepopTopPanel2" runat="server" CssClass="divBP" Visible="False" Height="0px">
						        <tr>
							        <td><asp:Image ID="prePopImage2" runat="server" CssClass='prePopImgBP1' AlternateText="2" ></asp:Image><asp:Label ID="lblPrePopTop2" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label><asp:Label ID="lblBPID2" runat="server" Text="" CssClass="displayNone"></asp:Label></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShift1BP2" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShift2BP2" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center; padding-right:10px;"><asp:TextBox ID="txtPrePopShift3BP2" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopBuildQty2" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShipQty2" runat="server" CssClass="textEntry textCenter numericOnly prepopShipQty" Text="0" Width="40"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopProdQty2" runat="server" CssClass="textEntry textCenter numericOnly prepopProdQty" Text="0" Width="40"></asp:TextBox></td>
						        </tr>
						        </asp:Panel>
						        <asp:Panel ID="prepopTopPanel3" runat="server" CssClass="divBP" Visible="False" Height="0px">
						        <tr>
							        <td><asp:Image ID="prePopImage3" runat="server" CssClass='prePopImgBP1' AlternateText="3" ></asp:Image><asp:Label ID="lblPrePopTop3" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label><asp:Label ID="lblBPID3" runat="server" Text="" CssClass="displayNone"></asp:Label></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShift1BP3" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShift2BP3" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center; padding-right:10px;"><asp:TextBox ID="txtPrePopShift3BP3" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopBuildQty3" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShipQty3" runat="server" CssClass="textEntry textCenter numericOnly prepopShipQty" Text="0" Width="40"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopProdQty3" runat="server" CssClass="textEntry textCenter numericOnly prepopProdQty" Text="0" Width="40"></asp:TextBox></td>
						        </tr>
						        </asp:Panel>
						        <asp:Panel ID="prepopTopPanel4" runat="server" CssClass="divBP" Visible="False" Height="0px">
						        <tr>
							        <td><asp:Image ID="prePopImage4" runat="server" CssClass='prePopImgBP1' AlternateText="4" ></asp:Image><asp:Label ID="lblPrePopTop4" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label><asp:Label ID="lblBPID4" runat="server" Text="" CssClass="displayNone"></asp:Label></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShift1BP4" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShift2BP4" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center; padding-right:10px;"><asp:TextBox ID="txtPrePopShift3BP4" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopBuildQty4" runat="server" CssClass="textEntry textCenter numericOnly prepopBuildQty" Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopShipQty4" runat="server" CssClass="textEntry textCenter numericOnly prepopShipQty" Text="0" Width="40"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopProdQty4" runat="server" CssClass="textEntry textCenter numericOnly prepopProdQty" Text="0" Width="40"></asp:TextBox></td>
						        </tr>
						        </asp:Panel>
						        <tr>
							        <td style="padding-left: 25px"><asp:Label ID="Label5" runat="server" Text="Daily Total" CssClass="spanLabel h4 divBPSpan "></asp:Label></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopDailyShift1" runat="server" CssClass="textEntry textCenter numericOnly margin-top-10 " Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopDailyShift2" runat="server" CssClass="textEntry textCenter numericOnly margin-top-10 " Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center; padding-right:10px;"><asp:TextBox ID="txtPrePopDailyShift3" runat="server" CssClass="textEntry textCenter numericOnly margin-top-10 " Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"><asp:TextBox ID="txtPrePopDailyTotal" runat="server" CssClass="textEntry textCenter numericOnly margin-top-10 " Text="0" Width="40" ReadOnly="True"></asp:TextBox></td>
							        <td style="text-align: center;"></td>
							        <td style="text-align: center;"></td>
						        </tr>
                            </table>
                        </ContentTemplate>
	                </asp:UpdatePanel>

					<table class="tableBorderCollapse" style="margin-left:20px;">
						<tr>						
							<td>
                            	<div id="tabPrePop" style="width:256px; padding-top:20px;" class="tabs">
				                    <ul class="dbqTab">
					                    <li><a href="#tabPrePop-1">1st Shift</a></li>
					                    <li><a href="#tabPrePop-2">2nd Shift</a></li>
					                    <li><a href="#tabPrePop-3">3rd Shift</a></li>
				                    </ul>
			                        <div id="tabPrePop-1" class="tabsDisplayNone"></div>
			                        <div id="tabPrePop-2" class="tabsDisplayNone"></div>
			                        <div id="tabPrePop-3" class="tabsDisplayNone"></div>
			                    </div>

                                <telerik:RadGrid ID="rgPrePop" runat="server" RenderMode="Lightweight" 
                                    AutoGenerateColumns="False"
				                    DataSourceID="SqlDataSourceDailyBuildQuantityPrePop" 
                                    AllowAutomaticDeletes="True" 
                                    AllowAutomaticInserts="True"
				                    AllowAutomaticUpdates="True" 
                                    CssClass="NoPadding " 
                                    Skin="Windows7" 
                                    ShowFooter="True"
                                    OnRowDrop="rgPrePop_RowDrop" 
                                    OnItemUpdated="rgPrePop_ItemUpdated"
                                    OnItemDeleted="rgPrePop_ItemDeleted"
                                    >
				                    <ClientSettings AllowRowsDragDrop="true" Selecting-AllowRowSelect="True">
					                    <ClientEvents OnRowDblClick="rowDblClick" />
					                    <Selecting AllowRowSelect="True" EnableDragToSelectRows="false" />
				                    </ClientSettings>
				                    <MasterTableView DataKeyNames="PrePopulateID" 
                                        DataSourceID="SqlDataSourceDailyBuildQuantityPrePop" EditMode="InPlace"
					                    CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemDisplay="Bottom">
                                        <Columns>
						                    <telerik:GridBoundColumn DataField="PrePopulateID" FilterControlAltText="PrePopulateID"
							                    ReadOnly="True" UniqueName="PrePopulateID" Visible="false">
						                    </telerik:GridBoundColumn>

						                    <telerik:GridBoundColumn DataField="DisplayID" FilterControlAltText="DisplayID"
							                    ReadOnly="True" UniqueName="DisplayID" Visible="false">
							                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"  Width="40px"/>
						                    </telerik:GridBoundColumn>
						
                                            <telerik:GridDragDropColumn >
                                                <HeaderStyle Width="40px" />
                                            </telerik:GridDragDropColumn>

						                    <telerik:GridBoundColumn DataField="ShiftNumber" FilterControlAltText="ShiftNumber"
							                    ReadOnly="True" UniqueName="ShiftNumber" HeaderText="Shift" Visible="True" >
							                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"  Width="40px"/>
							                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"  />
						                    </telerik:GridBoundColumn>

                                            <telerik:GridTemplateColumn UniqueName="BroadcastPointDescription" HeaderText="Honda Line Number">
                                                <ItemTemplate>
                                                    <img src="../../Images/Misc/<%#DataBinder.Eval(Container.DataItem, "ImageName")%>" class="prePopImgBP1"  alt='*' />
                                                    <%# DataBinder.Eval(Container.DataItem, "Description") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadComboBox RenderMode="Lightweight"  ID="RadComboBox1" runat="server"
                                                        DataSourceID="SqlDataSourceBroadcastPoints" 
                                                        DataTextField="Description" 
                                                        DataValueField="BroadcastPointID"
                                                        SelectedValue='<%# Bind("BroadcastPointID") %>'
                                                        CssClass="selectDropDown"
                                                        Width="220px"
                                                        OnClientLoad="showSelectedItemImage"
                                                        OnClientSelectedIndexChanging="showImageOnSelectedItemChanging"
                                                        OnItemDataBound="RadComboBox1_ItemDataBound" >
                                                        <ExpandAnimation Type="OutBack" />
                                                        <CollapseAnimation Type="InBack" />

                                                    </telerik:RadComboBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="220px" />
							                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"   Width="220px" />
                                            </telerik:GridTemplateColumn>

						                    <telerik:GridBoundColumn DataField="Quantity" DataType="System.Int16" FilterControlAltText="Quantity"
							                    HeaderText="Quantity" SortExpression="Quantity" UniqueName="Quantity" ColumnEditorID="QtyEditor_PrePop" 
                                                Aggregate="Sum" FooterAggregateFormatString="{0}">
							                    <HeaderStyle HorizontalAlign="Center"  Width="60px" />
							                    <ItemStyle HorizontalAlign="Center"    />
							                    <FooterStyle HorizontalAlign="Center" />
							                    <ColumnValidationSettings EnableRequiredFieldValidation="True"></ColumnValidationSettings>
						                    </telerik:GridBoundColumn>

						                    <telerik:GridEditCommandColumn>
							                    <HeaderStyle Width="70px" />
							                    <ItemStyle HorizontalAlign="Right"    />
						                    </telerik:GridEditCommandColumn>
						                    <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
							                    <HeaderStyle Width="30px" />
						                    </telerik:GridButtonColumn>
					                    </Columns>
                                        <EditFormSettings>
						                    <EditColumn UniqueName="EditCommandColumn1" />
						                    <PopUpSettings ScrollBars="None" />
					                    </EditFormSettings>
				                    </MasterTableView>
			                    </telerik:RadGrid>
                                <telerik:GridTextBoxColumnEditor runat="server" ID="QtyEditor_PrePop">  
				                    <TextBoxStyle Width="60px" /> 
			                    </telerik:GridTextBoxColumnEditor>

							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>

    <asp:SqlDataSource ID="SqlDataSourcePrePopBPSum" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" ProviderName="System.Data.SqlClient" 
        CancelSelectOnNullParameter="False"
        SelectCommand="procPSDailyBuildQuantityPrePopSelectSums" SelectCommandType="StoredProcedure" >
        <SelectParameters>
		    <asp:Parameter Name="BroadcastPointID" />
		    <asp:ControlParameter ControlID="rbOrders" Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" />
	    </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceDailyBuildQuantityPrePop" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>"
		SelectCommand="procPSDailyBuildQuantityPrePopSelect" 
        SelectCommandType="StoredProcedure"
        DeleteCommand="procPSDailyBuildQuantityPrePopDelete "
        DeleteCommandType="StoredProcedure"
		InsertCommand="procPSDailyBuildQuantityPrePopInsert"
        InsertCommandType="StoredProcedure"
        UpdateCommand="procPSDailyBuildQuantityPrePopUpdate"
        UpdateCommandType="StoredProcedure">
		<SelectParameters>
			<asp:ControlParameter ControlID="hidRad1Shift" Name="ShiftNumber" PropertyName="Value" Type="Int32" />
			<asp:ControlParameter ControlID="rbOrders"  Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" ConvertEmptyStringToNull="True" />
		</SelectParameters>
		<DeleteParameters>
			<asp:Parameter Name="PrePopulateID" Type="Int32" />
		</DeleteParameters>
		<InsertParameters>
			<asp:ControlParameter ControlID="hidRad1Shift" Name="ShiftNumber" PropertyName="Value" Type="Int32" />
			<asp:ControlParameter ControlID="rbOrders" Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" />
            <asp:Parameter Name="BroadcastPointID" Type="String" />
            <asp:Parameter Name="Quantity" Type="Int32" />
		</InsertParameters>
		<UpdateParameters>
			<asp:Parameter Name="PrePopulateID" />
            <asp:Parameter Name="BroadcastPointID" Type="String" />
			<asp:Parameter Name="Quantity" Type="Int32" />
		</UpdateParameters>
	</asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSourceDailyBuildQuantityEditBreakdown" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>"
        CancelSelectOnNullParameter="False"
		SelectCommand="procPSDailyBuildQuantityBreakdownSelect" 
        SelectCommandType="StoredProcedure"
        DeleteCommand="procPSDailyBuildQuantityBreakdownDelete "
        DeleteCommandType="StoredProcedure"
		InsertCommand="procPSDailyBuildQuantityBreakdownInsert"
        InsertCommandType="StoredProcedure"
        UpdateCommand="procPSDailyBuildQuantityBreakdownUpdate"
        UpdateCommandType="StoredProcedure">
		<SelectParameters>
			<asp:ControlParameter ControlID="hidDailyBuildDate" Name="ProductionDate" PropertyName="Value" Type="DateTime"  />
			<asp:ControlParameter ControlID="hidRad2Shift" Name="ShiftNumber" PropertyName="Value" Type="Int32" />
			<asp:ControlParameter ControlID="rbOrders"  Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" ConvertEmptyStringToNull="True" />
		</SelectParameters>
		<DeleteParameters>
			<asp:Parameter Name="BreakdownID" Type="Int32" />
		</DeleteParameters>
		<InsertParameters>
			<asp:ControlParameter ControlID="hidDailyBuildDate" Name="ProductionDate" PropertyName="Value" Type="DateTime"  />
			<asp:ControlParameter ControlID="hidRad2Shift" Name="ShiftNumber" PropertyName="Value" Type="Int32" />
			<asp:ControlParameter ControlID="rbOrders" Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" />
			<asp:Parameter Name="BroadcastPointID" Type="String" />
            <asp:Parameter Name="Quantity" Type="Int32" />
		</InsertParameters>
		<UpdateParameters>
			<asp:Parameter Name="BreakdownID" />
			<asp:Parameter Name="BroadcastPointID" Type="String" />
            <asp:Parameter Name="Quantity" Type="Int32" />
		</UpdateParameters>
	</asp:SqlDataSource>


    <asp:SqlDataSource ID="SqlDataSourceBroadcastPoints" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>"
		ProviderName="System.Data.SqlClient" SelectCommand="procGetBroadcastPoints" SelectCommandType="StoredProcedure" >
	</asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceEditDay" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>"
        CancelSelectOnNullParameter="False"
        SelectCommand="procPSDailyBuildQuantityEditDaySelect" 
        SelectCommandType="StoredProcedure"
        UpdateCommand="procPSDailyBuildQuantityEditDayUpdate"
        UpdateCommandType="StoredProcedure">
        <SelectParameters>
			<asp:ControlParameter ControlID="hidDailyBuildDate" Name="ProductionDate" PropertyName="Value" Type="DateTime"  />
			<asp:ControlParameter ControlID="rbOrders"  Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" ConvertEmptyStringToNull="True" />
		</SelectParameters>
        <UpdateParameters>
			<asp:ControlParameter ControlID="hidDailyBuildDate" Name="ProductionDate" PropertyName="Value" Type="DateTime"  />
			<asp:ControlParameter ControlID="rbOrders"  Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" ConvertEmptyStringToNull="True" />
		    <asp:Parameter Name="BroadcastPointID" />
			<asp:Parameter Name="EndLotProduced" Type="String" />
			<asp:Parameter Name="EndLotProducedJobQuantity" Type="Int16"  />
			<asp:Parameter Name="EndLotShipped" Type="String" />
			<asp:Parameter Name="PlannedShipQuantity" Type="Int16" />
        </UpdateParameters>
    </asp:SqlDataSource>
            
    <asp:SqlDataSource ID="SqlDataSourceEndLotProduced" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>"
		ProviderName="System.Data.SqlClient" 
        SelectCommand="procPSGetEndLotsWrapper" 
        SelectCommandType="StoredProcedure">
        <SelectParameters>
			<asp:ControlParameter ControlID="hidDailyBuildDate" Name="ProductionDate" PropertyName="Value" Type="DateTime"  />
			<asp:ControlParameter ControlID="rbOrders"  Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" ConvertEmptyStringToNull="True" />
			<asp:Parameter Name="ProdOrShipType" DefaultValue="PROD" Type="String"   />
		    <asp:Parameter Name="BroadcastPointID" />
		</SelectParameters>
	</asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceEndLotShipped" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>"
		ProviderName="System.Data.SqlClient" 
        SelectCommand="procPSGetEndLotsWrapper" 
        SelectCommandType="StoredProcedure">
        <SelectParameters>
			<asp:ControlParameter ControlID="hidDailyBuildDate" Name="ProductionDate" PropertyName="Value" Type="DateTime"  />
			<asp:ControlParameter ControlID="rbOrders"  Name="SetexSchedule" PropertyName="SelectedValue" Type="Int32" ConvertEmptyStringToNull="True" />
			<asp:Parameter Name="ProdOrShipType" DefaultValue="SHIP" Type="String"   />
		    <asp:Parameter Name="BroadcastPointID" />
		</SelectParameters>
	</asp:SqlDataSource>


</asp:Content>
