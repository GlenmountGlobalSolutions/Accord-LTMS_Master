<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TruckMapReport.aspx.vb" Inherits="LTMS_Master.TruckMapReport" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.maskedinput-1.3.1.min.js") %>"></script>
    <script type="text/javascript">
        var $hidLastTab;
        var $cmdRefresh;
        var $cmdRefreshHistoryList;
        var $cmdReprint;
        var $txtDay;
        var $txtShipDate_Begin;
        var $txtShipDate_End;
        var $ddlBroadcastPointID;
        var $hidShowNewReport;
        var $BLNumberToOpen;

        function contentPageLoad(sender, args) {

            CacheControls();

            if (args.get_isPartialLoad()) {
                // partial load, check if the hidShowNewReport is populated, if so open report.
                var newReport = $hidShowNewReport.val();
                if (newReport.length > 0) {
                    OpenTruckMapReport(false); 
                }
                $BLNumberToOpen = $("#MainContent_hidBLNumber1");
               
            }
            MakeDateTimePicker();
            MakeDivIntoTabs();

            AddEventsToControls();

            CreateDialog_Revision();
        }

        function CacheControls() {
            $hidLastTab = $("#MainContent_hidLastTab");
            $cmdRefresh = $("#MainContent_cmdRefresh");
            $cmdRefreshHistoryList = $("#MainContent_cmdRefreshHistoryList");
            $cmdReprint = $("#MainContent_cmdReprint");
            $txtDay = $("#MainContent_txtDay");
            $txtShipDate_Begin = $("#MainContent_txtShipDate_Begin");
            $txtShipDate_End = $("#MainContent_txtShipDate_End");
            $ddlBroadcastPointID = $("#MainContent_ddlBroadcastPointID");
            $hidShowNewReport = $("#MainContent_hidShowNewReport"); 
            
        }

        function AddEventsToControls() {
            $(".cmdShowWait").on('click', function () { ShowWaitOverlay(); });
            $cmdReprint.on('click', function () { OpenTruckMapReport(true); });

            //Add Wait cursor to the Previous and Next Image buttons
            $('[id^="MainContent_ib"]').on('click', function () { ShowWaitOverlay(); }).mouseover(function () { $(this).css('cursor', 'pointer'); });

        }

        function MakeDateTimePicker() {
            $txtDay.mask("99/99/9999").addDatePicker()
			.datepicker("option", "firstDay", 1)
			.datepicker({
			    onSelect: function (dateText, inst) {
			        DoRefreshPostBack(dateText, $cmdRefresh.prop('name'));
			    }
			})
			.change(function () {
			    DoRefreshPostBack(this.value, $cmdRefresh.prop('name'));
			});

            $txtShipDate_Begin.mask("99/99/9999").addDatePicker()
			.datepicker("option", "firstDay", 1)
			.datepicker({
			    onSelect: function (dateText, inst) {
			        DoRefreshPostBack(dateText, $cmdRefreshHistoryList.prop('name'));
			    }
			})
			.change(function () {
			    DoRefreshPostBack(this.value, $cmdRefreshHistoryList.prop('name'));
			});

            $txtShipDate_End.mask("99/99/9999").addDatePicker()
			.datepicker("option", "firstDay", 1)
			.datepicker({
			    onSelect: function (dateText, inst) {
			        DoRefreshPostBack(dateText, $cmdRefreshHistoryList.prop('name'));
			    }
			})
			.change(function () {
			    DoRefreshPostBack(this.value, $cmdRefreshHistoryList.prop('name'));
			});

            // position the calendar icon
            PositionCalendarPickerIcon();
        }

        function DoRefreshPostBack(dateText, cmd) {
            d1 = Date.parse(dateText);
            if (d1 != null) {
                ShowWaitOverlay();
                __doPostBack(cmd, '');
            }
        }

        function MakeDivIntoTabs() {

            $("#tabs").tabs({

                active: $hidLastTab.val(),    //set to previously Selected Tab (for postback)

                beforeActivate: function (event, ui) {
                    $hidLastTab.val(ui.newTab.index()); // when the tabs are selected set the hidden field
                    if (ui.newTab.index() == 1) {
                        __doPostBack($cmdRefreshHistoryList.prop('name'), '');
                    } else {
                        __doPostBack($cmdRefresh.prop('name'), '');
                    }
                    showMessage("");
                }
            });
        }

        function CreateDialog_Revision() {
            var $cmdRevision = $('#MainContent_cmdRevInc');
            var $dlgRevision = $('#divDlgAddRevision');

            $cmdRevision.click(function () { $dlgRevision.dialog('open'); return false; });

            $dlgRevision.modalDialog({
                control: $cmdRevision,
                width: 420,
                validationFunction: function () { return checkLength($("#MainContent_txtComment"), "Comment", 1, 500, $("#helpRevisionComment")); }
            });
        }

        function OpenTruckMapReport(reprintReport) {
            var url = baseUrl + '/2Reporting/Reports.aspx?report=TruckMap';
            var basepath = url;
            var blNumber;
            var blnumbers = new Array();

            if (reprintReport) {
                //url += 'Reprint Truck Map Report';
                blNumber = $('#MainContent_ddlOldRepFile').val();
                blNumber = blNumber.substring(0, 11); 
                blnumbers = blNumber.split(",");
            } else {
                //url += 'Truck Map Report';
                blNumber = $('#MainContent_lblBLNumberCombined11').val();
                blnumbers = blNumber.split(",");

            }

            if (blNumber == null || blNumber == "") {
                showMessage("No BLNumber Selected to Print.");
            } else {

                for (var a in blnumbers) {
                    url = basepath + '&BLNumber=' + blnumbers[a];
                    window.open(url);
                    showMessage("");
                   
                }
                
            }
            return false;
        }


    </script>
    <style type="text/css">
        #divDetails .textEntry
        {
            width: 160px;
        }
        
        .selectDropDown
        {
            width: 166px;
        }
        #divComments
        {
            vertical-align: bottom;
            text-align: left;
            margin: 4px;
            max-width: 665px;
        }
        .spanEndDate
        {
            margin-left: 20px;
        }
        .cmdRefreshHistoryList
        {
            margin-left: 20px;
            margin-bottom: 8px;
        }
        tr.paddingUnder > td
        {
            padding-bottom: 1em;
        }
        .reprintList
        {
            margin-left: 20px;
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
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divHiddenFields">
                <input id="hidShowNewReport" type="hidden" runat="server" />
                <input id="hidBLNumber" type="hidden" runat="server" />
                
                <input id="hidBLNumber1" type="hidden" runat="server" />
                <input id="hidLastTab" type="hidden" runat="server" />
            </div>
            <div id="divSelectBroadcastPointID">
                <table>
                    <tr>
                        <td>

                <asp:Label ID="lblSelectBroadcastPointID" runat="server" CssClass="spanLabel"></asp:Label>
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
               
              
            <div id="tabs" style="width: 620px">
                <ul>
                    <li><a href="#tabs-1">Create a New Report</a></li>
                    <li><a href="#tabs-2">Reprint Old Report</a></li>
                </ul>
                <div id="tabs-1" class="displayNone">
                    <div id="divFilters">
                        <table style="border-collapse: collapse; padding: 0; margin-bottom: 10px;">
                            <tr style="vertical-align: bottom;">
                                <td style="width: 170px">
                                    <asp:Label ID="Label1" runat="server" CssClass="spanLabel h3">Shipping Date:</asp:Label>
                                </td>
                                <td style="width: 16px; text-align: right;">
                                    <asp:ImageButton ID="ibPrev" runat="server" ImageUrl="~/images/Arrows/bbPrevpage.gif"></asp:ImageButton>
                                </td>
                                <td style="width: 140px; text-align: left; padding-left: 10px;">
                                    <asp:TextBox ID="txtDay" runat="server" Width="96px" CssClass="textEntry NoColorOnChange"></asp:TextBox>
                                </td>
                                <td style="width: 16px; text-align: left;">
                                    <asp:ImageButton ID="ibNext" runat="server" ImageUrl="~/images/Arrows/bbNextpage.gif"></asp:ImageButton>
                                </td>
                                <td style="text-align: left; padding: 0px 0px 0px 28px; width: 100px;">
                                    <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="inputButton cmdShowWait"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divDetails">
                        <table style="border-collapse: collapse; padding: 0; margin-bottom: 10px;" class="tblDetails">
                            <tr class="paddingUnder">
                                <td style="width: 178px">
                                    <asp:Label ID="Label3" runat="server" CssClass="spanLabel h3">KD Lot Number(s):</asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="cblLotNum" runat="server" AutoPostBack="True" RepeatColumns="3">
                                    </asp:CheckBoxList>
                                </td>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="spanLabel h3">Trailer ID:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTrailer" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </td>
                                </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" CssClass="spanLabel h3">Driver:</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDriver" runat="server" CssClass="selectDropDown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" CssClass="spanLabel h3">B/L Number:</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblBLNumberCombined" runat="server" CssClass="spanReadOnly" Width="160px"></asp:Label>
                                    <input ID="lblBLNumberCombined11" type="hidden" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="cmdCreateReport" runat="server" CssClass="inputButton cmdShowWait" Text="Create Report" />
                                    <asp:Button ID="cmdRevInc" runat="server" CssClass="inputButton" Text="Revisions" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divComments">
                        <asp:Panel ID="pnlRev" runat="server" CssClass="panelComments">
                            <asp:Label ID="Label2" runat="server" ForeColor="Black">Revision No:</asp:Label>
                            <asp:Label ID="lblRevNo" runat="server" CssClass="h3"></asp:Label>
                            <asp:Label ID="Label8" runat="server" ForeColor="Black" Style="margin-left: 8px;">Comments:</asp:Label>
                            <asp:Label ID="lblRevComments" runat="server" CssClass="h3"></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
                <div id="tabs-2" class="ui-widget">
                    <p>
                        <asp:Label ID="Label7" runat="server" CssClass="spanLabel h3">Beginning Date:</asp:Label>
                        <asp:TextBox ID="txtShipDate_Begin" runat="server" Width="96px" CssClass="textEntry NoColorOnChange"></asp:TextBox>
                        <asp:Label ID="Label9" runat="server" CssClass="spanLabel h3 spanEndDate">Ending Date:</asp:Label>
                        <asp:TextBox ID="txtShipDate_End" runat="server" Width="96px" CssClass="textEntry NoColorOnChange"></asp:TextBox>
                        <asp:Button ID="cmdRefreshHistoryList" runat="server" Text="Refresh" CssClass="inputButton cmdShowWait cmdRefreshHistoryList"></asp:Button>
                    </p>
                    <p>
                        <asp:Label ID="Label11" runat="server" CssClass="spanLabel h3">B/L Number:</asp:Label>
                        <asp:DropDownList ID="ddlOldRepFile" runat="server" Width="250px" CssClass="selectDropDown reprintList">
                        </asp:DropDownList>
                    </p>
                    <p>
                        <asp:Button ID="cmdReprint" runat="server" Width="100px" Height="27px" Text="Print"></asp:Button>
                    </p>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDlgAddRevision" title="Revision" class="displayNone">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <p class="validationHints h4">
                    All fields must be completed.</p>
                <table id="tblRevision" class="tableCenter">
                    <tr>
                        <td style="width: 92px">
                            <asp:Label ID="Label10" runat="server" CssClass="spanLabel h4">Date:</asp:Label>
                        </td>
                        <td style="width: 100%">
                            <asp:Label ID="lblDate" runat="server" CssClass="spanLabel h4"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label14" runat="server" CssClass="spanLabel h4">Revision: </asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRev" runat="server" CssClass="spanLabel h4"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="height: 100%">
                        <td style="vertical-align: top">
                            <asp:Label ID="Label16" runat="server" CssClass="spanLabel h4">Comments: </asp:Label>
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
</asp:Content>
