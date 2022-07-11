<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ShippingSchedule.aspx.vb" Inherits="LTMS_Master.ShippingSchedule" EnableEventValidation="false" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemOperations.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.maskedinput-1.3.1.min.js") %>"></script>
	<script type="text/javascript">
		var $tbDay;

		var $hidNodeLevel;
		var $hidNodeSeqDT;
		var $hidNodeLot;
		var $hidLotNum;
		var $hidMoveBroadcastPointID;
		var hidRevNumber;
        var hidRevNextNumber;
		var hidComments;

		var $ddlLotNum;
		var $ddlBroadcastPointID;

		var $cmdRefresh;

		function contentPageLoad(sender, args) {
			AddDirtyClassOnChange();
			RemoveTreeViewSkipLinks();

			CacheControls();

			if (!args.get_isPartialLoad()) {
			    $ddlLotNum.change(function () { $hidLotNum.val($(this).val()); });
			}
			CreateDatePicker();
			AddEventsToControls();
			CreateDialog_MoveLot();
			CreateDialog_Revision();
			CreateDialog_AssignTemplate();
		}

		function CacheControls() {
			$tbDay = $("#MainContent_tbDay");

			$hidNodeLevel = $("#MainContent_hidNodeLevel");
			$hidNodeSeqDT = $("#MainContent_hidNodeSeqDT");
			$hidNodeLot = $("#MainContent_hidNodeLot");
			$hidLotNum = $("#MainContent_hidLotNum");
			$hidMoveBroadcastPointID = $("#MainContent_hidMoveBroadcastPointID");
			$hidRevNumber = $("#MainContent_hidRevNumber");
            $hidNextRevNumber = $("#MainContent_hidNextRevNumber");
            $hidComments = $("#MainContent_hidComments");

			$ddlLotNum = $("#MainContent_ddlLot");
			$ddlBroadcastPointID = $find("<%=ddlBroadcastPointID.ClientID%>");
            $cmdRefresh = $("#MainContent_cmdRefresh");

        }

        function CreateDatePicker() {
            $tbDay.mask("99/99/9999").addDatePicker().datepicker("option", "firstDay", 1).datepicker({
                onSelect: function (dateText, inst) {
                    DoRefreshPostBack(dateText);
                }
            })
                .change(function () {// if the edit box is changed
                    DoRefreshPostBack(this.value);
                });

            PositionCalendarPickerIcon();
        }

        function AddEventsToControls() {
            $('#MainContent_cmdPrint').click(function () { OpenScheduleReport(); });

            $(".cmdShowWait").on('click', function () { ShowWaitOverlay(); });
            $("#MainContent_ddlBroadcastPointID").change(function () { ShowWaitOverlay(); });

            //Add Wait cursor to the Previous and Next Image buttons
            $('[id^="MainContent_ib"]').on('click', function () { ShowWaitOverlay(); }).mouseover(function () { $(this).css('cursor', 'pointer'); });
        }


        function DoRefreshPostBack(dateText) {
            d1 = Date.parse(dateText);
            if (d1 != null) {
                ShowWaitOverlay();
                __doPostBack($cmdRefresh.prop('name'), '');
            }
        }

        function OpenScheduleReport() {
            var url = baseUrl + '/2Reporting/Reports.aspx?report=ShippingSchedule&amp;webpageTitle=Shipping Schedule Report'
            url += '&BegDT=' + $tbDay.val();
            url += "&BroadcastPointID=" + $ddlBroadcastPointID.get_value();
            window.open(url);
            return false;
        }

        function CreateDialog_MoveLot() {
            var $cmdMove = $('#MainContent_cmdMove');
            var $dlgMove = $('#divDlgMoveLot');

            $dlgMove.modalDialog({
                control: $cmdMove,
                width: 410
            });

            $cmdMove.click(function () {
                $dlgMove.dialog('open');
                ShowCursor_Wait();
                $("#divDlgMoveOverlay").fadeIn();

                var nodeLevel = $hidNodeLevel.val();
                if (nodeLevel == 1) {
                    $("#MainContent_lblType").text("LOT");
                    $("#MainContent_lblLotNum").text($hidNodeLot.val());
                }
                else if (nodeLevel == 2) {
                    $("#MainContent_lblType").text("SUBLOT");
                    $("#MainContent_lblLotNum").text($hidNodeSubLot.val());
                }

                $ddlLotNum.empty();
                LoadDDLMoveLotNum($ddlBroadcastPointID.get_value());
                DoRefreshPostBack();
                return false;
            });
        }

        function LoadDDLMoveLotNum(broadcastPointID) {
            var lot = $hidNodeLot.val();

            var param = "{\"nodeLevel\": \"" + $hidNodeLevel.val() +
                "\", \"seqDT\": \"" + $hidNodeSeqDT.val() +
                "\", \"lot\": \"" + lot +
                "\", \"boolSetexOrder\": \"1" +    // don't know why, but this was hardcoded as a 1 in the old site. I think becuase there is no option to between all orders and Setex ones
                "\", \"broadcastPointID\": \"" + broadcastPointID +
                "\"}";

            GetShippingScheduleGetMoveList(param).success(function (data) {
                PopulateNameValueDDL(data, $ddlLotNum);
                $hidLotNum.val($ddlLotNum.val());
                ShowCursor_Default();
                $("#divDlgMoveOverlay").fadeOut();
            });
        }

        function CreateDialog_Revision() {
            var $cmdRevision = $("#MainContent_cmdRevInc");
            var $dlgRevision = $('#divDlgAddRevision');

            $cmdRevision.on('click', function () { $dlgRevision.dialog('open'); return false; });

            // Revision Dialog			
            $dlgRevision.modalDialog({
                control: $cmdRevision,
                width: 360,
                width: 420,
                validationFunction: function () { return checkLength($("#MainContent_txtComment"), "Comment", 1, 500, $("#helpRevisionComment")); }
            });
        }

        function CreateDialog_AssignTemplate() {
            var $cmdTemplate = $("#MainContent_cmdTemplate");
            var $dlgTemplate = $('#divDlgAssignTemplate');

            $cmdTemplate.on('click', function () { $dlgTemplate.dialog('open'); return false; });

            // Revision Dialog			
            $dlgTemplate.modalDialog({
                control: $cmdTemplate,
                width: 360,
                validationFunction: function () { return checkDropDownList($("#MainContent_ddlTemplates"), "Please select a Delivery Template.", $('#helpTemplate')); }
            });
        }

        function RadTreeView_OnClientNodeClicked(sender, eventArgs) {
            $hidNodeLot.val("");
            $hidNodeSeqDT.val("");
            $hidNodeLevel.val("");

            var treeNode = eventArgs.get_node();
            var nodeLevel = treeNode.get_level();
            if (nodeLevel == 0) {
                treeNode.set_selected(false);
            } else {
                //		        var lot = treeNode.get_value();
                //		        var SeqDT = treeNode.get_value();
                //		        $hidNodeLot.val(lot);
                //		        $hidNodeSeqDT.val(SeqDT);
                $hidNodeLevel.val(nodeLevel);
            }
        }

        function RefreshAfterTemplate() { __doPostBack($cmdRefresh.prop('name'), ''); }

    </script>
	<style type="text/css">
		#divshippingSchedule .inputButton
		{
			width: 140px;
		}
		#divshippingSchedule .textEntry
		{
			width: 160px;
		}
		#divshippingSchedule .selectDropDown
		{
			width: 165px;
		}
		#divButtonsShip .inputButton
		{
			width: 80px;
		}
		#divComments
		{
		    vertical-align: bottom; 
		    text-align: left; 
		    margin: 4px; 
		    max-width:665px;
		}
        .cbHold
        {
            margin-left:16px;
        }
        
        #divSelectBroadcastPointID
        {
            margin-top: 10px;
        }
        .selectDropDownBP
        {
            margin-left: 14px;
        }

        div.selectDropDownBP .rcbReadOnly
        {
            background-color: white;
        	border: 1px solid #6495ED;
	        border-radius: 5px;
        }
        div.selectDropDownBP .rcbActionButton
        {
            padding-top: 0px;
        }
        #ctl00_MainContent_ddlBroadcastPointID_Arrow
        {
            background-image:url("../../Images/Arrows/ddl.png");
            background-size: 20px 20px;
            background-repeat: no-repeat;
            color: Transparent;
        }
        .rcbActionButton
        {
            margin: 2px -2px 0px 0px;
        }

        .RadTreeView Img.rtImg 
        {
	        height: 12px;
	        width: 12px;
        /*    margin: 0px 6px 0px 2px;*/
	        display: inline;
	        vertical-align: middle;
        }

		        
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<telerik:RadAjaxManager runat="server" ID="RadAjaxManager" DefaultLoadingPanelID="RadAjaxLoadingPanel1" >
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="treeShip">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeShip"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ddlBroadcastPointID">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeShip"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ibPrev">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeShip"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ibNext">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeShip"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmdRefresh">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeShip"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cbHold">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeShip"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
         <telerik:AjaxSetting AjaxControlID="cmdMove">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeShip"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7" BackgroundPosition="Top"></telerik:RadAjaxLoadingPanel>

	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<div id="divHiddenFields">
				<input id="hidLotNum" type="hidden" runat="server">
				<input id="hidNodeLevel" type="hidden" runat="server">
				<input id="hidNodeSeqDT" type="hidden" runat="server">
				<input id="hidNodeLot" type="hidden" runat="server">
				<input id="hidRevNumber" type="hidden" runat="server">
				<input id="hidNextRevNumber" type="hidden" runat="server">
				<input id="hidComments" type="hidden" runat="server">
				<input id="hidMoveBroadcastPointID" type="hidden" runat="server">   
			</div>
            <div id="divSelectBroadcastPointID">
				<asp:Label ID="lblSelectBroadcastPointID" runat="server" CssClass="spanLabel h3"></asp:Label>
                <telerik:RadComboBox RenderMode="Lightweight"  ID="ddlBroadcastPointID" runat="server"
                        AutoPostBack="True"
                        DataTextField="Description" 
                        DataValueField="BroadcastPointID"
                        CssClass="selectDropDown selectDropDownBP NoColorOnChange"
                        Width="220px"
                        OnClientLoad="showSelectedItemImage"
                        OnClientSelectedIndexChanging="showImageOnSelectedItemChanging"
                        OnItemDataBound="RadComboBox1_ItemDataBound" 
                        >
                        <ExpandAnimation Type="OutBack" />
                        <CollapseAnimation Type="InBack" />
                </telerik:RadComboBox>
				<asp:CheckBox ID="cbHold" runat="server" AutoPostBack="True" Text="Show ON HOLD Lots" Checked="True" CssClass="selectCheckBox h4 cmdShowWait cbHold" />
            </div>
			<div id="divFilters" >
				<table style="border-collapse: collapse; padding: 0; margin-bottom: 10px;" >
					<tr style="vertical-align: bottom;">
						<td style="width: 170px">
							<asp:Label ID="LABEL13" runat="server" CssClass="spanLabel h3">Enter Date:</asp:Label>
						</td>
						<td style="width: 16px; text-align: right;">
							<asp:ImageButton ID="ibPrev" runat="server" ImageUrl="~/images/Arrows/bbPrevpage.gif"></asp:ImageButton>
						</td>
						<td style="width: 140px; text-align: left; padding-left: 10px;">
							<asp:TextBox ID="tbDay" runat="server" Width="96px" CssClass="textEntry NoColorOnChange"></asp:TextBox>
						</td>
						<td style="width: 16px; text-align: left;">
							<asp:ImageButton ID="ibNext" runat="server" ImageUrl="~/images/Arrows/bbNextpage.gif"></asp:ImageButton>
						</td>
						<td style="text-align: left; padding: 0px 0px 0px 80px; width: 100px;">
							<asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="inputButton cmdShowWait"></asp:Button>
						</td>
                          <td>
                            <table class="tblBP" style="margin-left: 40px">
							    <tr>
								    <td><asp:Panel ID="Panel1" runat="server" CssClass="divBP" Visible="True" ><asp:Image ID="image1" runat="server" CssClass='calendarFilterImgBP1' AlternateText="1"></asp:Image><asp:Label ID="lblBP1" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
								    <td><asp:Panel ID="Panel3" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image3" runat="server" CssClass='calendarFilterImgBP2' AlternateText="2" ></asp:Image><asp:Label ID="lblBP3" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
							    </tr>
							    <tr>
								    <td><asp:Panel ID="Panel2" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image2" runat="server" CssClass='calendarFilterImgBP3' AlternateText="3" ></asp:Image><asp:Label ID="lblBP2" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
								    <td><asp:Panel ID="Panel4" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image4" runat="server" CssClass='calendarFilterImgBP4' AlternateText="4" ></asp:Image><asp:Label ID="lblBP4" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
							    </tr>
						    </table>
                        </td>
					</tr>
				</table>
			</div>
        </ContentTemplate>
	</asp:UpdatePanel>	

<%--	<asp:UpdatePanel ID="UpdatePanel888" runat="server">	
		<ContentTemplate>--%>
            <div id="divshippingSchedule">
				<table id="tblContent" style="border-collapse: collapse; padding: 4px;" >
					<tr>
						<td style="width: 160px;">
						</td>
						<td style="width: 200px;text-align:center;" >
                        	<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        		<ContentTemplate>
                                    <asp:Label ID="lblDayOfWeek" runat="server" CssClass="h3"></asp:Label>
                                </ContentTemplate>
	                        </asp:UpdatePanel>
						</td>
						<td>
						</td>
					</tr>
					<tr>
						<td style="text-align: center;">
                        	<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        		<ContentTemplate>
							        <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="inputButton cmdShowWait"></asp:Button><br>
							        <asp:Button ID="cmdMove" runat="server" Text="Move" CssClass="inputButton"></asp:Button><br>
							        <asp:Button ID="cmdPrint" runat="server" Text="Print" CssClass="inputButton"></asp:Button><br>
							        <asp:Button ID="cmdTemplate" runat="server" Text="Assign Template" CssClass="inputButton"></asp:Button><br>
							        <asp:Button ID="cmdRevInc" runat="server" Text="Revisions" CssClass="inputButton"></asp:Button>
                                </ContentTemplate>
	                        </asp:UpdatePanel>
						</td>
						<td>
                            <asp:Panel ID="pnlTreeviewShip" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel">
                                <telerik:RadTreeView ID="treeShip" runat="server" RenderMode="Lightweight" BorderWidth="0" 
                                    Skin="Windows7" CssClass="treeViewNode" MultipleSelect="False"
                                    ShowLineImages="False"
                                    Width="200px"
                                    Height="280px"
                                    OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                    OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                    OnNodeClick="RadTreeView_NodeClick">
                                    <ContextMenus>
                                        <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu1" runat="server">
                                            <Items>
                                                <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All">
                                                </telerik:RadMenuItem>
                                                <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All">
                                                </telerik:RadMenuItem>
                                            </Items>
                                            <CollapseAnimation Type="none"></CollapseAnimation>
                                        </telerik:RadTreeViewContextMenu>
                                    </ContextMenus>
                                </telerik:RadTreeView>
                            </asp:Panel>
                        </td>
						<td style="padding-left: 20px;">
                        	<asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        		<ContentTemplate>
							    <asp:Panel ID="pnlLotData" runat="server">
								    <table id="tblDetails" style="border-collapse: separate;">
									    <tr>
										    <td>
											    <asp:Label ID="Label1" runat="server" CssClass="spanLabel">Lot</asp:Label>
										    </td>
										    <td>
											    <asp:Label ID="lblLdLot" runat="server" CssClass="spanLabel h3"></asp:Label>
										    </td>
									    </tr>
									    <tr>
										    <td>
											    <asp:Label ID="Label3" runat="server" CssClass="spanLabel">Load</asp:Label>
										    </td>
										    <td>
											    <asp:Label ID="lblLdLoad" runat="server" CssClass="spanLabel h3"></asp:Label>
										    </td>
									    </tr>
									    <tr>
										    <td>
											    <asp:Label ID="Label5" runat="server" CssClass="spanLabel">Ship Time</asp:Label>
										    </td>
										    <td>
											    <asp:TextBox ID="tbLdShipTime" runat="server" CssClass="textEntry"></asp:TextBox>
										    </td>
									    </tr>
									    <tr>
										    <td>
											    <asp:Label ID="LABEL2" runat="server" CssClass="spanLabel">Arrival Time</asp:Label>
										    </td>
										    <td>
											    <asp:TextBox ID="tbLdArrivalTime" runat="server" CssClass="textEntry"></asp:TextBox>
										    </td>
									    </tr>
									    <tr>
										    <td>
											    <asp:Label ID="LABEL4" runat="server" CssClass="spanLabel">Approx. Seq. Time</asp:Label>
										    </td>
										    <td>
											    <asp:TextBox ID="tbLdSeqTime" runat="server" CssClass="textEntry"></asp:TextBox>
										    </td>
									    </tr>
									    <tr>
										    <td>
											    <asp:Label ID="LABEL6" runat="server" CssClass="spanLabel">Trailer Number</asp:Label>
										    </td>
										    <td>
											    <asp:TextBox ID="txtTrailer" runat="server" CssClass="textEntry"></asp:TextBox>
										    </td>
									    </tr>
									    <tr>
										    <td>
											    <asp:Label ID="LABEL8" runat="server" CssClass="spanLabel">Driver Name</asp:Label>
										    </td>
										    <td>
											    <asp:DropDownList ID="ddlDriver" runat="server" CssClass="selectDropDown">
											    </asp:DropDownList>
										    </td>
									    </tr>
									    <tr>
										    <td>
											    <asp:Label ID="LABEL9" runat="server" CssClass="spanLabel">Actual Departure Time</asp:Label>
										    </td>
										    <td>
											    <asp:TextBox ID="tbLdDepartureTime" runat="server" CssClass="textEntry"></asp:TextBox>
										    </td>
									    </tr>
									    <tr>
										    <td>
											    <asp:Label ID="LABEL10" runat="server" Visible="False" CssClass="spanLabel">Shipping Notes</asp:Label>
										    </td>
										    <td>
											    <asp:TextBox ID="tbLdNotes" runat="server" Visible="False" CssClass="textEntry"></asp:TextBox>
										    </td>
									    </tr>
								    </table>
							    </asp:Panel>
                                </ContentTemplate>
	                        </asp:UpdatePanel>
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td style="text-align: center; height: 30px;">
                        	<asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        		<ContentTemplate>
							        <asp:Label ID="Label7" runat="server" CssClass="spanLabel">Total: </asp:Label>
							        <asp:Label ID="lblTotalShip" runat="server" CssClass="spanLabel">0</asp:Label>
                                </ContentTemplate>
	                        </asp:UpdatePanel>
						</td>
						<td>
						</td>
					</tr>
				</table>
			</div>
			<br />
	<asp:UpdatePanel ID="UpdatePanelComments" runat="server">	
	<ContentTemplate>
			<div id="divComments">
				<asp:Panel ID="pnlRev" runat="server" CssClass="panelComments">
					<asp:Label ID="Label11" runat="server" ForeColor="Black">Revision No:</asp:Label>
					<asp:Label ID="lblRevNo" runat="server" CssClass="h3"></asp:Label>
					<asp:Label ID="Label12" runat="server" ForeColor="Black" Style="margin-left: 8px;">Comments:</asp:Label>
					<asp:Label ID="lblRevComments" runat="server" CssClass="h3"></asp:Label>
				</asp:Panel>
			</div>
	</ContentTemplate>
	</asp:UpdatePanel>

	<div id="divDlgAddRevision" title="Revision">
		<asp:UpdatePanel ID="UpdatePanel2" runat="server">
			<ContentTemplate>
				<p class="validationHints h4">
					All fields must be completed.</p>
				<table id="tblRevision" class="tableCenter">
					<tr>
						<td  style="width: 92px">
							<asp:Label ID="Label14" runat="server" CssClass="spanLabel">Date:</asp:Label>
						</td>
						<td style="width: 100%">
							<asp:Label ID="lblDate" runat="server" CssClass="spanLabel"></asp:Label>
						</td>
						<td>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Label ID="LABEL15" runat="server" CssClass="spanLabel">Revision: </asp:Label>
						</td>
						<td>
							<asp:Label ID="lblRev" runat="server" CssClass="spanLabel"></asp:Label>
						</td>
						<td>
						</td>
					</tr>
					<tr  style="height: 100%">
						<td style="vertical-align: top" >
							<asp:Label ID="LABEL16" runat="server" CssClass="spanLabel">Comments: </asp:Label>
						</td>
						<td>
							<asp:TextBox ID="txtComment" runat="server" CssClass="textEntry h4" MaxLength="500" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
						</td>
						<td>
							<div id="helpRevisionComment" class="ui-state-default ui-corner-all">
								<span class="ui-icon ui-icon-help"></span>
							</div>
						</td>
					</tr>
				</table>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>

	<div id="divDlgAssignTemplate" title="Assign Template">
		<asp:UpdatePanel ID="UpdatePanel3" runat="server">
			<ContentTemplate>
				<p class="validationHints h4">
					All fields must be completed.</p>
				<table id="tblAssign" class="tableCenter">
					<tr>
						<td style="width: 120px;">
							<asp:Label ID="Label17" runat="server" CssClass="spanLabel">Delivery Date:</asp:Label>
						</td>
						<td>
							<asp:Label ID="lblDeliveryDate" runat="server" CssClass="spanLabel"></asp:Label>
						</td>
						<td>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Label ID="label21" runat="server" CssClass="spanLabel">Template:</asp:Label>
						</td>
						<td>
							<asp:DropDownList ID="ddlTemplates" runat="server" CssClass="selectDropDown NoColorOnChange">
							</asp:DropDownList>
						</td>
						<td>
							<div id="helpTemplate" class="ui-state-default ui-corner-all">
								<span class="ui-icon ui-icon-help"></span>
							</div>
						</td>
					</tr>
				</table>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>

	<div id="divDlgMoveLot" title="Move Lot" class="displayNone">
		<div id="divDlgMoveOverlay" class="internalOverlay"></div>
		<table id="tblMoveLot" class="tableCenter">
			<tr>
				<td>
					<asp:Label ID="Label20" runat="server" CssClass="spanLabel h3">Move</asp:Label>
				</td>
				<td>
					<asp:Label ID="lblType" runat="server" CssClass="spanLabel h4"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:Label ID="lblLotNum" runat="server" CssClass="spanLabel h4"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="LABEL22" runat="server" CssClass="spanLabel h3">Location</asp:Label>
				</td>
				<td>
					<asp:RadioButtonList ID="rblBeforeAfter" runat="server" RepeatDirection="Horizontal" CssClass="inputRadioButton h4">
						<asp:ListItem Value="BEFORE" Selected="True">Before</asp:ListItem>
						<asp:ListItem Value="AFTER">After</asp:ListItem>
					</asp:RadioButtonList>
				</td>
			</tr>
			<tr>
				<td>
				</td>
				<td>
					<asp:DropDownList ID="ddlLot" runat="server" Width="160px" CssClass="selectDropDown h4">
					</asp:DropDownList>
				</td>
			</tr>
		</table>
	</div>

  
</asp:Content>
