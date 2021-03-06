<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StationODSRevisionLog.aspx.vb" Inherits="LTMS_Master.ODSRevisionLog" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" href="odsRevisionLog.css?ver=1.2" rel="stylesheet" />
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemConfiguration.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/rgbcolor.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.inputlimiter.1.3.1.min.js") %>"></script>
    <script type="text/javascript">
        var Collection = function () {
            this.count = 0;
            this.collection = {};

            this.add = function (key) {
                if (this.collection[key] != undefined)
                    return undefined;
                this.collection[key] = key;
                return ++this.count
            }

            this.remove = function (key) {
                if (this.collection[key] == undefined)
                    return undefined;
                delete this.collection[key]
                return --this.count
            }

            this.item = function (key) {
                return this.collection[key];
            }

            this.forEach = function (block) {
                for (key in this.collection) {
                    if (this.collection.hasOwnProperty(key)) {
                        block(this.collection[key]);
                    }
                }
            }

            this.toString = function () {
                var localString = "";
                for (key in this.collection) {
                    localString += "," + key;
                }
                return localString.substring(1);
            }

            this.fromString = function (items) {
                this.count = 0;
                this.collection = {};

                if (items.length > 0) {
                    this.collection = items.split(",");
                }
            }
        }

        var listOfPreviousExpanded = new Collection();

        var $txtMCRDateFrom;
        var $txtMCRDateTo;

        var $dlgDivConfirmActive;
        var $dlgDivPrint;
        var $dlgDivDialogEdit;
        var $dlgDivDialogEditStationMCR;
        var $dlgDivDialogActiveStatus;
        var $divDialogSecurity;

        var $cmdPrint;
        var $cmdRefresh;
        var $cmdSetActiveMCR;
        var $cmdEditMCR;
        var $cmdEditStationMCR;

        var $selectedMCRID;
        var $selectedMCRNumber;
        var $selectedRadioButton;
        var $lastSelectedRadioButton;

        var $hidMcrID;
        var $hidStationID;
        var $hidExpandedMCRs;
        var $headerExpandCollapse;

        var $dlgEdit_txtNewMCRNumber;
        var $dlgEdit_txtNewMCRDate;
        var $dlgEdit_ddlNewMCRStatus;
        var $dlgEdit_txtNewMCRDescription;

        var $dlgEditMCRStatusID;
        var $dlgEdit_ddlNewActiveMCRStatus;

        var $dlgEdit_txtEditDescriptionOfChange;
        var $dlgEdit_txtEditStationMCRNumber;


        function contentPageLoad(sender, args) {
            try {
                CacheControls();
                
                if (args.get_isPartialLoad()) {
                    //Specific code for partial postbacks can go in here.   code in here will only execute after a post back
                } else {
                    $dlgEdit_txtNewMCRDescription.inputlimiter({ limit: 50 });  // add an input limit on text field
                    $dlgEdit_txtEditDescriptionOfChange.inputlimiter({ limit: 400 });  // add an input limit on text field
                    AddEventToEditStatusDDL();
                }
                AddControlEvents();
                ConfigureCalenderPickers();
                CreatePrintRevisionLogEvent();
                CreatePrintDialog_MCRInstructionSet();
                CreateDialog_MCREdit();
                CreateDialog_StationMCREdit();
                CreateGridExpandCollapse_Header();
                CreateGridExpandCollapse_Details();
                CreateConfirmationDialog();
                CreateInsufficientSecurityDialog();
                CreateRadioButtonListEvent();

                AutoExpandPreviousMCRs();

            } catch (err) {
                alert(err);
            }
        }

        // cache the controls to $name variable for use later in the script; jQuery will not need to search the DOM for them again.
        function CacheControls() {
            try {
                $txtMCRDateFrom = $('#MainContent_txtMCRDateFrom');
                $txtMCRDateTo = $('#MainContent_txtMCRDateTo');

                $dlgDivPrint = $('#divDialogPrint');
                $dlgDivConfirmActive = $('#divConfirmActive');
                $dlgDivDialogEdit = $('#divDialogEdit');
                $dlgDivDialogEditStationMCR = $('#divDialogEditStationMCR');
                $dlgDivDialogActiveStatus = $('#divDialogChangeActiveStatus');
                $divDialogSecurity = $("#divDialogSecurity");

                $hidMcrID = $('#MainContent_hidMcrID');
                $hidStationID = $('#MainContent_hidStationID');
                $hidExpandedMCRs = $('#MainContent_hidExpandedMCRs');

                $cmdPrint = $('#MainContent_cmdPrint');
                $cmdRefresh = $('#MainContent_cmdRefresh');
                $cmdSetActiveMCR = $('#MainContent_cmdSetActiveMCR');
                $cmdEditMCR = $('#MainContent_cmdEditMCR');
                $cmdEditStationMCR = $('#MainContent_cmdEditStationMCR');
                
                $lastSelectedRadioButton = $('input[name=rbgSelectorGroup]:checked');

                $dlgEdit_txtNewMCRNumber = $('#MainContent_txtNewMCRNumber');
                $dlgEdit_txtNewMCRDate = $('#MainContent_txtNewMCRDate');
                $dlgEdit_ddlNewMCRStatus = $('#MainContent_ddlNewMCRStatus');
                $dlgEdit_txtNewMCRDescription = $('#MainContent_txtNewMCRDescription');

                $dlgEdit_ddlNewActiveMCRStatus = $('#MainContent_ddlNewActiveMCRStatus');

                $dlgEdit_txtEditStationMCRNumber = $('#MainContent_txtEditStationMCRNumber');
                $dlgEdit_txtEditDescriptionOfChange = $('#MainContent_txtEditDescriptionOfChange');

            } catch (err) {
                alert(err);
            }
        }

        function AddControlEvents() {
            // add wait cursor to drop downs
            $('.selectDropDown').not(".NoWaitOnChange").change(function () { ShowCursor_Wait(); $(this).addClass('waitCursor'); }).removeClass('waitCursor');

            // disable radio button based on security (tied to the SetActiveMCR button)
            $('input[name=rbgSelectorGroup]:radio').prop('disabled', $cmdSetActiveMCR.prop('disabled'));
        }

        function AddEventToEditStatusDDL() {
            //create the popup dialog
            $dlgDivDialogActiveStatus.dialog({
                  autoOpen: false
                , appendTo: "#MasterForm"    // add dialog to the MasterPage Form
                //                , show: { effect: 'fade', duration: 200 }
				, hide: { effect: 'fade', duration: 200 }
                , buttons: [{ text: "Ok", click: function () { $(this).dialog("close"); } }
                            , { text: "Cancel", click: function () { $(this).dialog("close"); $dlgEdit_ddlNewMCRStatus.val($dlgEditMCRStatusID); } }]
            });

            $dlgEdit_ddlNewMCRStatus.change(function () {
                if ($dlgEdit_ddlNewMCRStatus.children("option:selected").text() == "Active") {
                    $dlgDivDialogActiveStatus.dialog("open");
                }
            });        
        }

        function ConfigureCalenderPickers() {
            try {
                //Set the first day of the week: Sunday is 0, Monday is 1, ... 
                $txtMCRDateFrom.addDatePicker().datepicker("option", "firstDay", 1)
                    .datepicker({ onSelect: function (dateText) { DoRefreshPostBack(dateText); } })
                    .change(function () { DoRefreshPostBack(this.value); });

                $txtMCRDateTo.addDatePicker().datepicker("option", "firstDay", 1)
                    .datepicker({ onSelect: function (dateText) { DoRefreshPostBack(dateText); } })
                    .change(function () { DoRefreshPostBack(this.value); });
                
                $dlgEdit_txtNewMCRDate.addDatePicker().datepicker("option", "firstDay", 1);

                PositionCalendarPickerIcon();
                AddjQueryUIFormatToCalendar();                
            } catch (err) {
                alert(err);
            }
        }

        function CreatePrintRevisionLogEvent() {
            $cmdPrint.click(function () { return OpenReport_MCRLog(); return false; });
        }

        function CreatePrintDialog_MCRInstructionSet() {
            $dlgDivPrint.modalDialog({
                control: $cmdPrint
                , width: 300
                , acceptFunction: function () { return OpenReport_MCRInstructionSet(); }
            });
        }

        //NOTE: this function is added as an OnClientClick event in the gvMCRGrid_RowDataBound event in StationODSRevisionLog.aspx.vb
        function SetMCRIDandOpenPrintDialog(mcrID, mcrNumber) {
            $selectedMCRID = mcrID;
            $selectedMCRNumber = mcrNumber;
            $dlgDivPrint.dialog('open');
            $('#MainContent_chkIncludeWatermark').prop('checked', true);

        }

        function OpenReport_MCRLog() {
            var url = baseUrl + '/2Reporting/Reports.aspx?report=ODSRevisionLog&amp;webpageTitle=ODS Revision Log Report'
            url += "&LineID=" + $('#MainContent_ddlLineNumbers').val();
            url += "&VehicleModelID=" + $('#MainContent_ddlVehicleModel').val();
            url += "&DateFrom=" + $('#MainContent_txtMCRDateFrom').val();
            url += "&DateTo=" + $('#MainContent_txtMCRDateTo').val();
            url += "&MCRStatusID=" + $('#MainContent_ddlMCRStatus').val();
            window.open(url);
            return false;

        }

        function OpenReport_MCRInstructionSet() {
            var url = baseUrl + '/3SystemConfiguration/1StationConfiguration/7StationODSConfiguration/WorkInstructionExport.aspx?';
            var stationType = 0;
//            if (isNaN($tabIndex) == false) { stationType = $tabIndex; }
            url += "ReportType=0";
            url += "&StationType=" + stationType;
            url += "&MasterChangeRequestID=" + $selectedMCRID;
            url += "&IncludeWatermark=" + $('#MainContent_chkIncludeWatermark').is(':checked');
            url += "&Line=" + $("#MainContent_ddlLineNumbers option:selected").text();
            url += "&Model=" + $("#MainContent_ddlVehicleModel option:selected").text();
            url += "&MCRNumber=" + $selectedMCRNumber;
            window.open(url);   //alert(url);
            // now open another url for the Wire Harness report
            window.open(url.replace("StationType=0", "StationType=1"));   //alert(url);

            return false;
        }

        function CreateDialog_MCREdit() {
            $dlgDivDialogEdit.modalDialog({
                control: $cmdEditMCR
                , height: 240
                , width: 420
                , validationFunction: function () { return ValidateDialog_MCREdit(); }
            });

            $('.mcrDescriptionMaster, .mcrEditable').on('click', function () {
                try {
                    if ($cmdEditMCR.is(":disabled")) {
                        $divDialogSecurity.dialog('open');
                    } else {
                        var thisRow = $(this).closest("tr");
                        var rbActive = thisRow.find('input[name="rbgSelectorGroup"]');
                        $selectedMCRID = rbActive.val();
                        $hidMcrID.val($selectedMCRID);
                        $dlgEditMCRStatusID = thisRow.find('.mcrStatusID').text();

                        $dlgDivDialogEdit.dialog('open');

                        $dlgEdit_txtNewMCRNumber.val(thisRow.find('.mcrNumber').text());
                        $dlgEdit_txtNewMCRDate.val(thisRow.find('.mcrDate').text());
                        $dlgEdit_ddlNewMCRStatus.val($dlgEditMCRStatusID);
                        $dlgEdit_txtNewMCRDescription.val(thisRow.find('.mcrDescriptionMaster').text());
                        $('#MainContent_hidActiveOnClient').val(rbActive.is(':checked'));
                    }
                } catch (err) {
                    alert(err);
                }
            });
        }

        function ValidateDialog_MCREdit() {
            var bValid = true;
            bValid = checkText($dlgEdit_txtNewMCRDescription, "Please Enter an MCR Description. This is a required field.") && bValid;
            bValid = checkDate($dlgEdit_txtNewMCRDate, "Invalid MCR Date. Valid format is MM/DD/YYYY", $("#helpMCRDate")) && bValid;
            bValid = checkText($dlgEdit_txtNewMCRNumber, "Invalid MCR Number. This is a required field.") && bValid;
             return bValid;
        }

        function CreateDialog_StationMCREdit() {
            $dlgDivDialogEditStationMCR.modalDialog({
                control: $cmdEditStationMCR
                , height: 260
                , width: 520
                , validationFunction: function () { return ValidateDialog_StationMCREdit(); }
            });
        }

        function AddEvent_StationMCREdit() {
            $('.editStationMCR').on('click', function () {
                try {
                    if ($cmdEditMCR.is(":disabled")) {
                        $divDialogSecurity.dialog('open');
                    } else {
                        var thisRow = $(this).closest("tr");
                        $hidMcrID.val(thisRow.find('.editStationMCR_MCRID').text());
                        $hidStationID.val(thisRow.find('.editStationMCR_StationID').text());

                        $dlgDivDialogEditStationMCR.dialog('open');

                        $dlgEdit_txtEditStationMCRNumber.val(thisRow.find('.smcrNumber').text().trim());
                        $dlgEdit_txtEditDescriptionOfChange.val(thisRow.find('.smcrDescChange').text().trim());

                    }
                } catch (err) {
                    alert(err);
                }
            });
        }

        function ValidateDialog_StationMCREdit() {
            var bValid = true;
            bValid = checkText($dlgEdit_txtEditDescriptionOfChange, "Please Enter a Description of Change. This is a required field.") && bValid;
            bValid = checkText($dlgEdit_txtEditStationMCRNumber, "Please Enter a Station MCR #. This is a required field.") && bValid;
            return bValid;
        }

        function CreateGridExpandCollapse_Header() {
            try {
                $(".imgHeaderExpandCollapse").click(function () {
                    var imgSrc;
                    var imgTitle;
                    var thisImg = $(this)

                    if (thisImg.attr('src') === "../../../Images/plus.png") {
                        imgSrc = "../../../Images/minus.png";  //swap image
                        imgTitle = "Collapse All";
                        $headerExpandCollapse = "expandAll";
                    } else {
                        imgSrc = '../../../Images/plus.png';
                        imgTitle = "Expand All";
                        $headerExpandCollapse = "collapseAll";
                    }
                    // set the src and title attributes
                    thisImg.attr('src', imgSrc).attr("title", imgTitle);

                    $(".imgExpandCollapse").trigger("click");   //trigger the click event on all expand collapse buttons in grid
                    $headerExpandCollapse = "none";
                });
            } catch (err) {
                alert(err);
            }
        }

        function CreateGridExpandCollapse_Details() {
            try {
                $(".imgExpandCollapse").click(function () {
                    var imgSrc;
                    var thisRow;
                    var imgTitle;
                    var lastColumn;
                    var insideHeader;
                    var thisImgSrc = $(this).attr('src');

                    if ((thisImgSrc === '../../../Images/plus.png') && ($headerExpandCollapse != "collapseAll")) {
                        imgSrc = '../../../Images/minus.png';  //swap image
                        imgTitle = "Collapse";

                        // insert last column TD content in as new next tr;  change background to of new tr to match
                        thisRow = $(this).closest("tr");
                        lastColumn = $(this).closest("td").siblings(":last");
                        thisRow.after("<tr><td colspan='2'></td><td colspan = '999'>" + lastColumn.html() + "</td></tr>")
                                .next().children('td').css('background-color', thisRow.css('background-color'));

                        // make the inside (details) grid header lighter and change the font
                        insideHeader = thisRow.next().find(".subDataGrid_HeaderStyle");
                        insideHeader.css('background-color', lighterColor(thisRow.css('background-color'), .035));

                        AddEvent_StationMCREdit();

                        // Add to list Of Previous Expanded MCRS, so that after postpack they can be expanded.
                        AddToArrayOfExpanded($(this).attr('id'));

                    } else if ((thisImgSrc === '../../../Images/minus.png') && ($headerExpandCollapse != "expandAll")) {
                        imgSrc = '../../../Images/plus.png';
                        imgTitle = "Expand";
                        $(this).closest("tr").next().remove();

                        // remove item from list Of Previous Expanded MCRS, so that after postpack they are not expanded.
                        RemoveFromArrayOfExpanded($(this).attr('id'));
                    };
                    // set the src and title attributes
                    $(this).attr('src', imgSrc).attr("title", imgTitle);
                });
            } catch (err) {
                alert(err);
            }
        }

        function AutoExpandPreviousMCRs() {
            listOfPreviousExpanded.fromString($hidExpandedMCRs.val());

            listOfPreviousExpanded.forEach(function (item) {   
                var id = "#" + item;
                $(id).trigger("click");
            });
        }

        function AddToArrayOfExpanded(item) {
            listOfPreviousExpanded.add(item);
            $hidExpandedMCRs.val(listOfPreviousExpanded.toString());
        }
        function RemoveFromArrayOfExpanded(item) {
            listOfPreviousExpanded.remove(item);
            $hidExpandedMCRs.val(listOfPreviousExpanded.toString());
        }


        function CreateConfirmationDialog() {
            $dlgDivConfirmActive.modalDialog({
                control: $cmdSetActiveMCR
                , width: 400
                , cancelFunction: function () {
                    $selectedRadioButton.prop("checked", false);
                    $lastSelectedRadioButton.prop("checked", true);
                    return false; 
                }
            });
        }

        function CreateInsufficientSecurityDialog() {
            $divDialogSecurity.dialog({
                modal: true
                , autoOpen: false
                , buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function CreateRadioButtonListEvent() {

            $('input[name=rbgSelectorGroup]:radio').change(function () {
                // The one that fires the event is always the
                // checked one; you don't need to test for this
                $selectedRadioButton = $(this);
                $selectedMCRID = $(this).val();
                var mcrNumber = $(this).closest("td").next().find('span:first').html();
                var thisRow = $(this).closest("tr");
                var mcrStatus = thisRow.find('.mcrStatus').text();
                            
                $hidMcrID.val($selectedMCRID);
                $("#confirmMCRID").text(mcrNumber);
                $("#confirmMCRStatus").text(mcrStatus);
                $dlgDivConfirmActive.dialog('open');
            });
        }

        function DoRefreshPostBack(dateText) {
            d1 = Date.parse(dateText);
            if (d1 != null) {
                ShowWaitOverlay();
                __doPostBack($cmdRefresh.prop('name'), '');
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divODSContent">
                <asp:Button ID="cmdEditStationMCR" runat="server" Text="Edit Station MCR" Style="visibility: hidden; display: none;"/>
                <asp:Button ID="cmdEditMCR" runat="server" Text="Edit MCR" Style="visibility: hidden; display: none;"/>
                <asp:Button ID="cmdSetActiveMCR" runat="server" Text="Show on Monitor" Style="visibility: hidden; display: none;"/>
                <input id="hidMcrID" type="hidden" name="MCRID" runat="server" value="-1" />
                <input id="hidStationID" type="hidden" name="StationID" runat="server" value="-1" />
                <input id="hidExpandedMCRs" type="hidden" name="hidExpandedMCRs" runat="server" value="" />
                <div id="divFilterPanel">
                    <p>
                        <asp:Label ID="Label1" runat="server" CssClass="h2 spanLabel">Line:</asp:Label>
                        <asp:DropDownList ID="ddlLineNumbers" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="lblInstructionSet" runat="server" CssClass="h2 spanLabel">Vehicle Model:</asp:Label>
                        <asp:DropDownList ID="ddlVehicleModel" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <span class="h2 spanLabel">From:</span>
    					<asp:TextBox ID="txtMCRDateFrom" runat="server" CssClass="textEntry"></asp:TextBox>
                    </p>            
                    <p>
                        <span class="h2 spanLabel">To:</span>
    					<asp:TextBox ID="txtMCRDateTo" runat="server" CssClass="textEntry"></asp:TextBox>
                    </p>            
                    <p>
                        <span class="h2 spanLabel">MCR Status:</span>
                        <asp:DropDownList ID="ddlMCRStatus" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="inputButton filterButtons" />
                    </p>            
                    <p>
                        <asp:Button ID="cmdPrint" runat="server" Text="Print" CssClass="inputButton filterButtons" />
                    </p>            
                </div>
            </div>
            <div id="divMCRGrid" class="datagrid">
                <asp:GridView ID="gvMCRGrid" runat="server" AutoGenerateColumns="False" DataKeyNames="MasterChangeRequestID" ShowHeaderWhenEmpty="True"
                    DataSourceID="sqlDataSource_RevisionLog"  GridLines="None"  CssClass="" Width="1110px" 
                    AlternatingRowStyle-CssClass="DataGrid_AlternatingItemStyle" EditRowStyle-CssClass="DataGrid_EditItemStyle" 
                    FooterStyle-CssClass="DataGrid_FooterStyle" HeaderStyle-CssClass="DataGrid_HeaderStyle" 
                    RowStyle-CssClass="DataGrid_ItemStyle" PagerStyle-CssClass="DataGrid_PagerStyle" SelectedRowStyle-CssClass="DataGrid_SelectedItemStyle">
                    <Columns>
                        <asp:TemplateField HeaderText="Print All Stations" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgPrint" runat="server" AlternateText="Print" CssClass="imgPrint" ImageUrl="~/Images/ODS/print.png" Height="24" Width="24" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Show on Monitor"  HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:Literal ID="rbSelector" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="130px" ItemStyle-CssClass="MCRNumberColumn" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MCRNumberColumn">
                            <HeaderTemplate>
                                <img id="imgHeaderExpandCollapseID" alt = "" title="Expand All" class="imgHeaderExpandCollapse" src="../../../Images/plus.png" />
                                <asp:Label ID="lblMCRNumberHeader" runat="server" text="Master MCR #" CssClass="mcrNumber"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <img id="imgExpandCollapseID<%# Container.DisplayIndex %>" alt = "" title="Expand" class="imgExpandCollapse" src="../../../Images/plus.png" />
                                <asp:Label ID="lblMCRNumber" runat="server" CssClass="mcrNumber mcrEditable"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MCRDate" HeaderText="MCR Date" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="mcrDate mcrEditable" />
                        <asp:BoundField DataField="MCRStatus" HeaderText="MCR Status" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="mcrStatus mcrEditable"/>
                        <asp:BoundField DataField="MCRStatus_ConfigurationID" HeaderText="MCR Status ID" ItemStyle-CssClass="hiddenClass mcrStatusID" HeaderStyle-CssClass="hiddenClass"/>
                        <asp:BoundField DataField="MCRDescription" HeaderText="MCR Description" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="mcrDescriptionMaster" />
                        <asp:BoundField DataField="MCRModifiedDT" HeaderText="Last Edited Date" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MCRModifiedBy" HeaderText="Last Edited By" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField ItemStyle-CssClass="hiddenClass" HeaderStyle-CssClass="hiddenClass">
                            <ItemTemplate>
                                <asp:Panel ID="pnlDetails" runat="server" CssClass="datagrid gvDetails">
                                    <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeaderWhenEmpty="True" Width="950px" 
                                    CssClass = "" AlternatingRowStyle-CssClass="DataGrid_AlternatingItemStyle" EditRowStyle-CssClass="DataGrid_EditItemStyle" 
                                    FooterStyle-CssClass="DataGrid_FooterStyle" HeaderStyle-CssClass="subDataGrid_HeaderStyle" RowStyle-CssClass="DataGrid_ItemStyle" 
                                    PagerStyle-CssClass="DataGrid_PagerStyle" SelectedRowStyle-CssClass="DataGrid_SelectedItemStyle">
                                        <Columns>
                                            <asp:BoundField DataField="MasterChangeRequestID" HeaderText="MasterChangeRequestID" ItemStyle-CssClass="hiddenClass editStationMCR_MCRID" HeaderStyle-CssClass="hiddenClass"/>
                                            <asp:BoundField DataField="StationID" HeaderText="MasterChangeRequestID" ItemStyle-CssClass="hiddenClass editStationMCR_StationID" HeaderStyle-CssClass="hiddenClass"/>
                                            <asp:BoundField ItemStyle-Width="230px" DataField="StationDescription" HeaderText="Station" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="mcrDescriptionDetail"  />
                                            <asp:BoundField ItemStyle-Width="450px" DataField="ChangeDescription" HeaderText="Description of Change" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="mcrDescriptionDetail editStationMCR smcrDescChange" />
                                            <asp:BoundField ItemStyle-Width="100px" DataField="StationMCRNumber" HeaderText="Station MCR #" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="editStationMCR smcrNumber"/>
                                            <asp:BoundField ItemStyle-Width="170px" DataField="ModifiedDT" HeaderText="Edited Date" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField ItemStyle-Width="150px" DataField="ModifiedBy" HeaderText="Edited By" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDialogPrint" title="Print Watermark Confirmation">
        <p class="pCenter" >
            Printing Instructions for ALL Stations.
        </p>
        <p class="pCenter" >
            <asp:CheckBox ID="chkIncludeWatermark" runat="server" Text="Include Watermark" Checked="True" CssClass="selectCheckBox secureCheckBox" />
        </p>
    </div>
    <div id="divDialogSecurity" title="Insufficient Security Permissions">
        <p class="pCenter" >
            You do not have permission to make edits.
        </p>
    </div>
    <div id="divConfirmActive" title="Change Show on Monitor Confirmation">
        <div class="ui-corner-all ui-state-default" style="top: 30%; position: absolute;">
            <span class="ui-icon ui-icon-help"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;" id="confirmChangeActive" >
            This will show MCR Number <span id="confirmMCRID" class="confirmMCRNumber"></span> (<span id="confirmMCRStatus" class="confirmMCRNumber"></span>) on the Operator Display Stations, Continue?
        </p>
    </div>
    <div id="divDialogEdit" title="Edit MCR Revision">
        <p class="validationHints">
        </p>
        <div id="divEditArea">
            <p>
                <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">MCR Number:</span>
                <asp:TextBox ID="txtNewMCRNumber" CssClass="textEntry" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
            </p>
            <p>
                <span class="ui-icon ui-icon-triangle-1-e mcrDate"></span><span class="spanLabel mcrDate">MCR Date:</span>
                <asp:TextBox ID="txtNewMCRDate" CssClass="textEntry" runat="server" Width="80px" ></asp:TextBox>
            </p>
            <p>
                <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">MCR Status:</span>
                <asp:DropDownList ID="ddlNewMCRStatus" runat="server" CssClass="selectDropDown NoWaitOnChange" Width="116px" />
            </p>
            <p>
                <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">MCR Description:</span>
                <asp:TextBox ID="txtNewMCRDescription" CssClass="textEntry" runat="server" Width="240px" Height="30px" TextMode="MultiLine" ></asp:TextBox>
            </p>
            <input id="hidActiveOnClient" type="hidden" name="MCRID" runat="server" value="-1" />
        </div>
    </div>
    <div id="divDialogEditStationMCR" title="Edit Station MCR Number">
        <p class="validationHints">
        </p>
        <div id="div3">
            <p>
                <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">Station MCR #:</span>
                <asp:TextBox ID="txtEditStationMCRNumber" CssClass="textEntry" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
            </p>
            <p>
                <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">Description of Change:</span>
                <asp:TextBox ID="txtEditDescriptionOfChange" CssClass="textEntry" runat="server" Width="360px" Height="100px" TextMode="MultiLine" ></asp:TextBox>
            </p>
        </div>
    </div>
    <div id="divDialogChangeActiveStatus" title="Change Status">
        <p class="validationHints">
        </p>
        <div id="div2">
            <p style="text-align:center;">
                <span class="spanLabel">Change Status of Current Active to:</span>
                <asp:DropDownList ID="ddlNewActiveMCRStatus" runat="server" CssClass="selectDropDown NoWaitOnChange" Width="116px" />
            </p>
        </div>
    </div>

    <asp:SqlDataSource ID="sqlDataSource_RevisionLog" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" 
    SelectCommandType="StoredProcedure" SelectCommand="[ods].[procGetInstructionRevisionLog_Master]"
    CancelSelectOnNullParameter="False">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlLineNumbers" PropertyName="SelectedValue" Name="LineID" DefaultValue="" ConvertEmptyStringToNull="True" />
        <asp:ControlParameter ControlID="ddlVehicleModel" PropertyName="SelectedValue" Name="ConfigurationID" DefaultValue="" ConvertEmptyStringToNull="True" />
        <asp:ControlParameter ControlID="ddlMCRStatus" PropertyName="SelectedValue" Name="MCRStatusID" DefaultValue="" ConvertEmptyStringToNull="True" />
        <asp:ControlParameter ControlID="txtMCRDateFrom" PropertyName="Text" Name="DateFrom" DefaultValue="" ConvertEmptyStringToNull="True" />
        <asp:ControlParameter ControlID="txtMCRDateTo" PropertyName="Text" Name="DateTo" DefaultValue="" ConvertEmptyStringToNull="True" />
    </SelectParameters>
</asp:SqlDataSource>

</asp:Content>
