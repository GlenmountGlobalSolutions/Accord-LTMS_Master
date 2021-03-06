<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StationODSConfiguration.aspx.vb" Inherits="LTMS_Master.ODSStationConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" href="ods.css?ver=1.4" rel="stylesheet" />
    <link type="text/css" href="../../../Styles/checkboxStyle.css?ver=1.3" rel="stylesheet" />
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.maskedinput-1.3.1.min.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.inputlimiter.1.3.1.min.js") %>"></script>
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/rgbcolor.js") %>"></script>
    <script type="text/javascript">
        var gridsDisabled = false;

        //main controls
        var $cmdSave;
        var $cmdCopy;
        var $cmdPrint;
        var $cmdNewMCR;
        var $cmdRefreshImages;

        var $ddlMCRNumber;
        var $treeView;
        var $treeViewPanel;
        var $browsedTxtBox;

        var $tabIndex=0;
        var $hidLastTab;
        var $hidBrowsedImagePathID;
        var $rowSelectorDelta_steps;
        var $rowSelectorDelta_images;


        var $selectedIdx_Steps;
        var $selectedIdx_Images;

        //copy dialog
        var $ddlLineNumbersCopySrc;
        var $ddlStationsCopySrc;
        var $ddlInstructionSetsCopySrc;
        var $ddlMCRNumberCopySrc;

        var $ddlLineNumbersCopyDst;
        var $ddlStationsCopyDst;
        var $ddlInstructionSetsCopyDst;
        var $ddlMCRNumberCopyDst;

        var $copyStepCount;
        var $lbxInstructionsCopySrc;
        var $lbxInstructionsCopyDst;

        //MCR Edit Dialog
        var $txtMCRNumberEdit;
        var $txtMCRDateEdit;
        var $txtMCRStatusEditValue;
        var $txtMCRDescriptionEditValue;
        var $txtStationMCR;
        var $txtStationChangeDescriptionEdit;

        var $txtMCRNumberEditValue;
        var $txtMCRDateEditValue;

        //MCR New Dialog
        var $txtMCRNumberNew;
        var $txtMCRDateNew;
        var $txtMCRDescriptionNew;

        //Select Image Dialog
        var $dlgSelectImage;
        var $browsedBtnClicked;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChangeLimited();
            
            CacheControls();

            CreateDialog_MCREdit();
            CreateDialog_MCRNew();
            CreateDialog_MCRCopy();
            CreateDialog_Print();
            CreateDialog_SelectImage();

            MakeDivIntoTabs();
            MakeMCRDialogDateTimePickers();
            AddStylesAndEvents();
            AddSortButtonEvent();
            SetupGridEvents();
            AddColorToColorDropDownOptions();
            DisableEnterKeyOnPage();
            SetMaxLengthForInputboxes();
            RemoveTreeViewSkipLinks();
            AddSelectOnlyOneToCheckboxes();

            if (args.get_isPartialLoad()) {
                //Specific code for partial postbacks can go in here.   code in here will only execute after a post back
                var t = $('#MainContent_copyMsg').text();

                if (t.length > 0 && $("#divDialogCopy").dialog( "isOpen" )) {
                    setValidationHint(t);
                }

                if ($rowSelectorDelta_steps.val() != "0") {
                    MoveRow($rowSelectorDelta_steps.val());
                    $rowSelectorDelta_steps.val("0");
                }
                if ($rowSelectorDelta_images.val() != "0") {
                    MoveRow($rowSelectorDelta_images.val());
                    $rowSelectorDelta_images.val("0");
                }

                HighlightSelectedRow();

                //reset the variable holding the imagepath textbox
                var id = '#' + $hidBrowsedImagePathID.val();
                $browsedTxtBox = $(id)

                $treeViewPanel.removeClass('treeViewOverlay');
            }
        }

        function CacheControls() {
            gridsDisabled = $("#MainContent_gvSteps").hasClass('aspNetDisabled');

            // hidden tags
            $hidBrowsedImagePathID = $('#MainContent_hidBrowsedImagePathID');
            $hidLastTab = $("#MainContent_hidLastTab");

            //main controls
            $cmdSave = $('#MainContent_cmdSave');
            $cmdCopy = $('#MainContent_cmdCopy');
            $cmdPrint = $('#MainContent_cmdPrint');
            $cmdNewMCR = $('#MainContent_cmdCreateNew');
            $cmdRefreshImages = $('#MainContent_cmdRefreshImages');

            $rowSelectorDelta_steps = $('#MainContent_rowSelectorDelta_steps');
            $rowSelectorDelta_images = $('#MainContent_rowSelectorDelta_images');
            $ddlMCRNumber = $('#MainContent_ddlMCRNumber');

            //copy dialog
            $ddlLineNumbersCopySrc = $('#MainContent_ddlLineNumbersCopySrc');
            $ddlStationsCopySrc = $('#MainContent_ddlStationsCopySrc');
            $ddlInstructionSetsCopySrc = $('#MainContent_ddlInstructionSetsCopySrc');
            $ddlMCRNumberCopySrc = $('#MainContent_ddlMCRNumberCopySrc');

            $ddlLineNumbersCopyDst = $('#MainContent_ddlLineNumbersCopyDst');
            $ddlStationsCopyDst = $('#MainContent_ddlStationsCopyDst');
            $ddlInstructionSetsCopyDst = $('#MainContent_ddlInstructionSetsCopyDst');
            $ddlMCRNumberCopyDst = $('#MainContent_ddlMCRNumberCopyDst');

            $copyStepCount = $("#MainContent_copyStepCount");
            
            $lbxInstructionsCopySrc = $("#MainContent_lbxInstructionsCopySrc");
            $lbxInstructionsCopyDst = $("#MainContent_lbxInstructionsCopyDst");

            //MCR Edit Dialog
            $txtMCRNumberEdit = $('#MainContent_txtMCRNumberEdit');
            $txtMCRDateEdit = $('#MainContent_txtMCRDateEdit');
            $ddlMCRStatusEdit = $('#MainContent_ddlMCRStatusEdit');
            $txtMCRDescriptionEdit = $('#MainContent_txtMCRDescriptionEdit');
            $txtStationMCR = $('#MainContent_txtStationMCR');
            $txtStationChangeDescriptionEdit = $('#MainContent_txtStationChangeDescriptionEdit');

            $txtMCRNumberEditValue = $txtMCRNumberEdit.val();
            $txtMCRDateEditValue = $txtMCRDateEdit.val();
            $txtMCRStatusEditValue = $ddlMCRStatusEdit.val();
            $txtMCRDescriptionEditValue = $txtMCRDescriptionEdit.val();

            //MCR New Dialog
            $txtMCRNumberNew = $('#MainContent_txtMCRNumberNew');
            $txtMCRDateNew = $('#MainContent_txtMCRDateNew');
            $txtMCRDescriptionNew = $('#MainContent_txtMCRDescriptionNew');

            $dlgSelectImage = $('#divDialogSelectImage');
            $treeView = $('#MainContent_treeViewFolder');
            $treeViewPanel = $('#MainContent_pnlTreeview');
        }

        function AddStylesAndEvents() {
            $(".css-checkbox-td input").addClass("css-checkbox med");
            $(".css-checkbox-td label").addClass("css-label med elegant");

            $('input[name="rbgSelectorGroup"]').change(function () { HighlightSelectedRow(); });
            $('.waitCursorOnChange').change(function () { ShowCursor_Wait(); });
            $('#MainContent_cmdClearRow, #MainContent_cmdRefresh').one('click', function () { ShowWaitOverlay(); });

            $('.copyButton').click(function () { ShowCursor_Wait(); $(this).addClass('waitCursor'); }).button();
            $('.selectListBox').change(function () { ShowCursor_Wait(); $(this).addClass('waitCursor'); }).removeClass('waitCursor');
            $('.cmdRefreshImages').click(function () { ShowCursor_Wait(); ClearTreeView(); }).button();
            $treeView.click(function () { return SelectNode(); });
        }

        function AddDirtyClassOnChangeLimited() {
            $(":text, select[id*='ddlColor']").not(".NoColorOnChange").change(function () { $(this).addClass('dirty'); });
            $(".odsGrid :checkbox").not(".NoColorOnChange").change(function () { $(this).parent().parent().addClass('dirty'); });
            $(".divAuxList :checkbox").not(".NoColorOnChange").change(function () { $("label[for='"+$(this).attr("id")+"']").addClass('dirty'); });
            $('#divDialogMCREdit :text').unbind('change');
        }

        function MakeDivIntoTabs() {
            $("#odsTabs").tabs({
                beforeActivate: function (event, ui) {
                    $tabIndex = ui.newTab.index();
                    $hidLastTab.val($tabIndex); // when the tabs are selected set the hidden field
                    clearMessage();
                    $(".stepsOnly").toggleClass('disabledControl').prop('disabled', function (i, v) { return !v; }); //toggle the move/sort buttons
                }
            }).tabs("option", "active", $hidLastTab.val());  //set to previously Selected Tab (for postback)
        }

        function MakeMCRDialogDateTimePickers() {
            $txtMCRDateNew.mask("99/99/9999").addDatePicker().datepicker("option", "firstDay", 1);
            PositionCalendarPickerIcon();
        }

        function CreateDialog_MCREdit() {
            var $dlgDiv = $('#divDialogMCREdit');

            $cmdSave.click(function () {
                $dlgDiv.dialog('open');
                $txtMCRNumberEdit.val($txtMCRNumberEditValue);
                $txtMCRDateEdit.val($txtMCRDateEditValue);
                $ddlMCRStatusEdit.val($txtMCRStatusEditValue);
                $txtMCRDescriptionEdit.val($txtMCRDescriptionEditValue);
                $txtStationChangeDescriptionEdit.val('');
                return false;
            });

            $dlgDiv.modalDialog({
                control: $cmdSave,
                width: 440,
                clearInputOnOpen: false,
                validationFunction: function () { return ValidateDialog_MCREdit(); }
            });
        }

        function ValidateDialog_MCREdit() {
            var bValid = true;
            bValid = checkText($txtStationChangeDescriptionEdit, "Please Enter a Station Change Description. This is a required field.") && bValid;
            bValid = checkText($txtStationMCR, "Please Enter a Station MCR Number. This is a required field.") && bValid;
            bValid = checkDate($txtMCRDateEdit, "Invalid MCR Date. Valid format is MM/DD/YYYY", $("#helpMCRDate")) && bValid;
            bValid = checkText($txtMCRNumberEdit, "Invalid MCR Number. This is a required field.", $("#helpMCRNumber")) && bValid;
            return bValid;
        }

        function CreateDialog_MCRNew() {
            var $dlgDiv = $('#divDialogMCRNew');

            $cmdNewMCR.click(function () {
                $dlgDiv.dialog('open');
                $('#MainContent_ddlLineNumbersNew').val($('#MainContent_ddlLineNumbers').val());
                $('#MainContent_ddlInstructionSetsNew').val($('#MainContent_ddlInstructionSets').val());
                $txtMCRNumberNew.val('');
                $txtMCRDescriptionNew.text('');
                if ($txtMCRDateNew.val().length == 0) {
                    $txtMCRDateNew.datepicker('setDate', new Date())
                }
                return false;
            });

            $dlgDiv.modalDialog({
                control: $cmdNewMCR,
                width: 440,
                clearInputOnOpen: false,
                validationFunction: function () { return ValidateDialog_MCRNew(); }
            });
        }

        function ValidateDialog_MCRNew() {
            var bValid = true;
            bValid = checkText($txtMCRDescriptionNew, "Invalid MCR Description. This is a required field.") && bValid;
            bValid = checkDropDownList($("#MainContent_ddlMCRStatusNew"), "Please Select a Status") && bValid;
            bValid = checkDate($txtMCRDateNew, "Invalid MCR Date. Valid format is MM/DD/YYYY") && bValid;
            bValid = checkText($txtMCRNumberNew, "Invalid MCR Number. This is a required field.") && bValid;
            bValid = checkDropDownListByIndex($("#MainContent_ddlInstructionSetsNew"), 1, "Please Select a Model Number") && bValid;
            bValid = checkDropDownListByIndex($("#MainContent_ddlLineNumbersNew"), 1, "Please Select a Line Number") && bValid;
            return bValid;
        }

        function CreateDialog_MCRCopy() {
            var $dlgDiv = $('#divDialogCopy');

            $cmdCopy.click(function () {
                $('input[name$="rblCopyMode"], #MainContent_chkCopyChecks').prop('checked', false);

                // check if there was a validation hint, if so redisplay after opening dialog.  (modalDialog clears the message on open event)
                var t = $('#MainContent_copyMsg').text();

                $dlgDiv.dialog('open');

                if (t.length > 0) {
                    $('#MainContent_copyMsg').text(t);
                    setValidationHint(t);
                }

                return false;
            });

            $dlgDiv.modalDialog({
                control: $cmdCopy,
                width: 780,
                height: 735,
                clearInputOnOpen: false,
                validationFunction: function () { return ValidateDialog_MCRCopy(); }
            });
        }

        function ValidateDialog_MCRCopy() {
            var bValid = true;

            if ($("#MainContent_rblCopyMode :checked").val() === undefined) {
                bValid = false;
                setValidationHint('Select a Copy Method');
            } else if (($ddlLineNumbersCopySrc.val() == $ddlLineNumbersCopyDst.val())
                        && ($ddlStationsCopySrc.val() == $ddlStationsCopyDst.val())
                        && ($ddlInstructionSetsCopySrc.val() == $ddlInstructionSetsCopyDst.val())
                        && ($ddlMCRNumberCopySrc.val() == $ddlMCRNumberCopyDst.val())) {
                bValid = false;
                setValidationHint('Stations, Vehicle Models and MCR Numbers are the same, please change the selections.');
            } else if ($copyStepCount.val() == "0") {
                bValid = false;
                setValidationHint('No Steps have been added to the "Copy To".');
            }

            bValid = checkDropDownListByIndex($ddlMCRNumberCopyDst, 1, "Please Select an MCR Number to Copy To") && bValid;

            bValid = checkText($lbxInstructionsCopyDst, 'No Steps have been added to the "Copy To".') && bValid;
            bValid = checkText($lbxInstructionsCopySrc, 'The "Copy From" does not have any Steps to Copy.') && bValid;

            return bValid;
        }

        function CreateDialog_Print() {
            var $dlgDiv = $('#divDialogPrint');
            // add dialog open event
            $cmdPrint.click(function () {
                $dlgDiv.dialog('open');
                $('#MainContent_chkIncludeWatermark').prop('checked', true).removeClass('dirty');
                return false; 
            });

            // Add Print Dialog
            $dlgDiv.modalDialog({
                control: $cmdPrint
                , width: 300
                , acceptFunction: function () { return OpenPDFReport(); }
            });
        }

        function CreateDialog_SelectImage() {

            $dlgSelectImage.modalDialog({
                width: 500
                , acceptFunction: function () { return SetNewImage(); }
            });

            $('.browseButton').click(function () {
                $browsedBtnClicked = $(this);

                $browsedTxtBox = $browsedBtnClicked.prev();
                $hidBrowsedImagePathID.val($browsedTxtBox.attr("id"));

                if ($treeView.children().length == 0) {
                    $treeViewPanel.addClass('treeViewOverlay');
                    $cmdRefreshImages.click();
                }
                
                $dlgSelectImage.dialog('open');
                return false;
            });
        }

        function AddSortButtonEvent() {
            $('.sortButton').click(function () {
                var result = false;
                clearMessage();

                if ((isNaN($selectedIdx_Steps) == false && $tabIndex == 0) || (isNaN($selectedIdx_Images) == false && $tabIndex == 1)) {
                    ShowWaitOverlay();
                    result = true;
                }
                else {
                    showMessage('Please Select a Row.');
                }
                return result;
            });
        }

        function SetupGridEvents() {
            if (gridsDisabled == false) {
                $('.stepID').removeClass('stepID'); // this class makes the stepID appear greyed out.
                AddChangeSelectedRowOnClick();
                AddChangeRowColorOnHover();
                // 2012.09.19 removed - AddChangeColumnColorOnHover();
            } else {
                if (isNaN($selectedIdx_Steps) == false) { $('input[name=rbgSelectorGroupSteps]')[$selectedIdx_Steps].checked = false; }
                if (isNaN($selectedIdx_Images) == false) { $('input[name=rbgSelectorGroupImages]')[$selectedIdx_Images].checked = false; }
            }
        }

        function MoveRow(delta) {
            if ($tabIndex == 0) {
                $('input[name=rbgSelectorGroupSteps]')[$selectedIdx_Steps + parseInt(delta)].checked = true;
                $selectedIdx_Steps = $selectedIdx_Steps + parseInt(delta);
            } else {
                $('input[name=rbgSelectorGroupImages]')[$selectedIdx_Images + parseInt(delta)].checked = true;
                $selectedIdx_Images = $selectedIdx_Images + parseInt(delta);
            }
        }

        function AddChangeSelectedRowOnClick() {
            $('#divOdsTabSteps table[id^="MainContent_gv"] tr:has(td)').click(function () {
                $selectedIdx_Steps = parseInt($(this).index()) - 1;
                $('input[name=rbgSelectorGroupSteps]')[$selectedIdx_Steps].checked = true;
                HighlightSelectedRow();
            });
            $('#divOdsTabImages table[id^="MainContent_gv"] tr:has(td)').click(function () {
                $selectedIdx_Images = parseInt($(this).index()) - 1;
                $('input[name=rbgSelectorGroupImages]')[$selectedIdx_Images].checked = true;
                HighlightSelectedRow();
            });
        }

        function HighlightSelectedRow() {
            $('#MainContent_gvSteps tr:has(td)').removeClass("odsSelected").eq($selectedIdx_Steps).addClass("odsSelected");
            $('#MainContent_gvChecks tr:has(td)').find("td").removeClass("odsSelected");
            $('#MainContent_gvChecks tr:has(td)').eq($selectedIdx_Steps).find("td").addClass("odsSelected");

            $('#MainContent_gvImages tr:has(td)').removeClass("odsSelected").eq($selectedIdx_Images).addClass("odsSelected");
            $('#MainContent_gvImageChecks tr:has(td)').find("td").removeClass("odsSelected");
            $('#MainContent_gvImageChecks tr:has(td)').eq($selectedIdx_Images).find("td").addClass("odsSelected");
        }

        function AddChangeRowColorOnHover() {
            // add hover to change the background of the row
            $('#MainContent_gvSteps tr, #MainContent_gvChecks tr').hover(
                function () {   // handlerIn
                    var idx = parseInt($(this).index()) - 1;
                    $('#MainContent_gvSteps tr:has(td)').eq(idx).addClass("odsHover");
                    $('#MainContent_gvChecks tr:has(td)').eq(idx).find("td").addClass("odsHover");
                },
                function () {   // handlerOut
                    var idx = parseInt($(this).index()) - 1;
                    $('#MainContent_gvSteps tr:has(td)').eq(idx).removeClass("odsHover");
                    $('#MainContent_gvChecks tr:has(td)').eq(idx).find("td").removeClass("odsHover");
                }
            );

            $('#MainContent_gvImages tr:has(td), #MainContent_gvImageChecks tr:has(td)').hover(
                function () {   // handlerIn
                    var idx = parseInt($(this).index()) - 1;
                    $('#MainContent_gvImages tr:has(td)').eq(idx).addClass("odsHover");
                    $('#MainContent_gvImageChecks tr:has(td)').eq(idx).find("td").addClass("odsHover");
                },
                function () {   // handlerOut
                    var idx = parseInt($(this).index()) - 1;
                    $('#MainContent_gvImages tr:has(td)').eq(idx).removeClass("odsHover");
                    $('#MainContent_gvImageChecks tr:has(td)').eq(idx).find("td").removeClass("odsHover");
                }
            );
        }
        
        function AddChangeColumnColorOnHover() {
            // adds or removes a css style to all cells within hovering column.
            $("#MainContent_gvChecks td").hover(
                function () {
                    var count = $(this).closest("td").prevAll("td").length;
                    $(this).parents("#MainContent_gvChecks").find("tr:has(td)").each(function () {
                        $(this).find("td:eq(" + count + ")").addClass("odsHoverColumn");
                    });
                },
                function () {
                    var count = $(this).closest("td").prevAll("td").length;
                    $(this).parents("#MainContent_gvChecks").find("tr:has(td)").each(function () {
                        $(this).find("td:eq(" + count + ")").removeClass("odsHoverColumn");
                    });
                }
            );
        }

        function AddColorToColorDropDownOptions() {
            $('#divInstructionPanel option').each(function () {
                var opt = $(this);
                var backColor = opt.val();
                var foreColor = ConvertForeColor(backColor);

                var color = new RGBColor(backColor);
                if (color.ok) {
                    opt.css("background-color", color.toRGB())
                   .css("color", foreColor);
                }
            });
        }

        function SetMaxLengthForInputboxes() {
            $(".odsGrid_ItemStyle input").inputlimiter({ limit: 100 });
            $txtMCRDescriptionNew.inputlimiter({ limit: 50 });
            $txtStationChangeDescriptionEdit.inputlimiter({ limit: 400 });
        }

        function OpenPDFReport() {
            var url = baseUrl + '/3SystemConfiguration/1StationConfiguration/7StationODSConfiguration/WorkInstructionExport.aspx?';
            var stationType = 0;
            if (isNaN($tabIndex) == false) { stationType = $tabIndex; }
            url += "ReportType=1";
            url += "&StationType=" + stationType;
            url += "&StationID=" + $('#MainContent_ddlStations').val();
            url += "&MasterChangeRequestID=" + $('#MainContent_ddlMCRNumber').val();
            url += "&MCRNumber=" + $('option:selected', $('#MainContent_ddlMCRNumber')).attr('MCRNumber');
            url += "&IncludeWatermark=" + $('#MainContent_chkIncludeWatermark').is(':checked');
            window.open(url);
            return false;
        }

        function SelectNode() {
            try {
                var src = window.event != window.undefined ? window.event.srcElement : evt.target;
                var nodeClick = src.tagName.toLowerCase() == "a" || src.tagName.toLowerCase() == "span";
                if (nodeClick) {
                    return false;
                }
            }
            catch (Error) {
                alert(Error);
            }
        }
        
        function SetNewImage() {
            try {
                var treeViewData = window['MainContent_treeViewFolder_Data'];
                if (treeViewData.selectedNodeID.value != "") {

                    var selectedNode = document.getElementById(treeViewData.selectedNodeID.value);
                    var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                    value = replaceAll(value, "\\\\", "\\").substring(value.indexOf("\\") + 1);
                    $browsedTxtBox.val(value).change();
                    //DEBUG:: alert("Text: " + text + "\r\n" + "Value: " + value + "\r\n" + "Level: " + nodeLevel);
                    return true;
                }
            }
            catch (Error) {
                alert(Error);
            }
        } 

        function ClearTreeView(){
            try {
                $treeView.empty();
            }
            catch (Error) {
                alert(Error);
            }
        }

        function pad(n) { return (n < 10) ? ("0" + n) : n; }

        function AddSelectOnlyOneToCheckboxes() {
            var colIDX = $('#MainContent_hidNumberOfModelCols').val();
            if (colIDX <= 3) {
                $('#coverClipForImages').toggle();
            } else {
                for (i = 2; i <= colIDX - 2; i++) {
                    $(function () {
                        var j = "MainContent_gvImageChecks_ctl" + pad(i);
                        $("input:checkbox[id^='" + j + "']").click(function () {
                            $("input:checkbox[id^='" + j + "']").not($(this)).removeAttr("checked");
                            $(this).attr("checked", $(this).attr("checked"));
                        });
                    })
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
			<input id="hidBrowsedImagePathID" type="hidden" runat="server">
			<input id="hidLastTab" type="hidden" runat="server">

            <div id="divODSContent">
                <input id="copyStepCount" type="hidden" name="copyStepCount" runat="server" value="0" />
                <input id="rowSelectorDelta_steps" type="hidden" name="rowSelectorDelta" runat="server" value="0" />
                <input id="rowSelectorDelta_images" type="hidden" name="rowSelectorDelta" runat="server" value="0" />
                <input id="hidNumberOfModelCols" type="hidden" runat="server" value="0" />
                <input id="hidModfiedBy" type="hidden" runat="server" value="" />
                <div id="divFilterPanel">
                    <p>
                        <asp:Label ID="Label1" runat="server" CssClass="h2 spanLabel">Line:</asp:Label>
                        <GGS:WebDropDownList ID="ddlLineNumbers" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="Label2" runat="server" CssClass="h2 spanLabel">Station:</asp:Label>
                        <asp:DropDownList ID="ddlStations" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="lblInstructionSet" runat="server" CssClass="h2 spanLabel">Instruction Set:</asp:Label>
                        <asp:DropDownList ID="ddlInstructionSets" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <span class="h2 spanLabel">MCR #:</span>
                        <GGS:WebDropDownList ID="ddlMCRNumber" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange">
                        </GGS:WebDropDownList>
                    </p>
                    <p>
                        <span class="h2 spanLabel">Station MCR #:</span>
                        <asp:Label ID="lblStationMCRNumber" runat="server" CssClass="spanReadOnly StationMCRNumber"></asp:Label>
                    </p>            
                    <p>
                        <span class="h2 spanLabel spanMCRDate">MCR Date:</span>
                        <asp:Label ID="lblMCRDate" runat="server" CssClass="spanReadOnly MCRDate"></asp:Label>
                    </p>            
                    <p>
                        <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="inputButton filterButtons" />
                    </p>            
                    <p>
                        <asp:Button ID="cmdCreateNew" runat="server" Text="Create New MCR" CssClass="inputButton filterButtons" />
                    </p>            
                            
                </div>
                <div id="divInstructionPanel">
                    <div id="divVertButton">
                        <asp:ImageButton ID="cmdMoveUp" runat="server" ImageUrl="~/Images/Arrows/ArrowUpLeft.png" CssClass="sortButton" />
                        <asp:Label ID="Label5" runat="server" CssClass="spanLabel">Move</asp:Label>
                        <asp:ImageButton ID="cmdMoveDown" runat="server" ImageUrl="~/Images/Arrows/ArrowDownLeft.png" CssClass="sortButton" />
                    </div>
                    <div id="odsTabs">
                        <ul>
                            <li><a href="#divOdsTabSteps">Steps</a></li>
                            <li><a href="#divOdsTabImages">Images</a></li>
                        </ul>
                        <div id="divOdsTabSteps" >
                            <div id="divOdsSteps" class="odsGrid">
                                <GGS:BulkEditGridView ID="gvSteps" runat="server" AutoGenerateColumns="False" DataSourceID="sqlDataSource_InstructionSteps" DataKeyNames="DisplayID" GridLines="None" CssClass="odsGrid" HeaderStyle-CssClass="odsGrid_HeaderStyle">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderStyle Width="30px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Literal ID="rbSelectorSteps" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <GGS:BulkEditBoundField DataField="InstructionID" AllowedToClear="false" HeaderText="InstructionID"/>
                                    <GGS:BulkEditBoundField DataField="StepID" AllowedToClear="false" HeaderText="StepID"  />
                                    <GGS:BulkEditBoundField DataField="DisplayID" HeaderText="Step" Frozen="true" ReadOnly="true" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="stepID"/>
                                    <asp:TemplateField HeaderText="Color" HeaderStyle-Width="80px">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlColor" runat="server" DataSourceID="sqlDataSource_Colors" DataTextField="ColorName" DataValueField="ColorName" Width="100px" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <ControlStyle Width="540" />
                                        <ItemStyle CssClass="odsGrid_ItemStyle" Width="540px" />
                                    </asp:BoundField>
                                </Columns>
                                </GGS:BulkEditGridView>
                            </div>
                            <div id="divOdsChecks" class="odsGrid">
                                <GGS:BulkEditGridView ID="gvChecks" runat="server" AutoGenerateColumns="False" DataKeyNames="DisplayID" 
                                    GridLines="None" CssClass="odsGrid" HeaderStyle-CssClass="odsGrid_HeaderStyle">
                                    <Columns>
                                        <GGS:BulkEditBoundField DataField="InstructionID" AllowedToClear="false" HeaderText="InstructionID" />
                                        <GGS:BulkEditBoundField DataField="StepID" AllowedToClear="false" HeaderText="StepID" />
                                        <GGS:BulkEditBoundField DataField="DisplayID" HeaderText="DisplayID" Frozen="true" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                                        <asp:CheckBoxField DataField="See EDS" HeaderText="See EDS" HeaderStyle-CssClass="odsColumnSpecial"  Text=" " />
                                        <asp:CheckBoxField DataField="Specific Quality" HeaderText="Critical Feature" HeaderStyle-CssClass="odsColumnSpecial" Text=" " />
                                        <asp:CheckBoxField DataField="Seq Control" HeaderText="Sequence Control" HeaderStyle-CssClass="odsColumnSpecial" Text=" " />
                                    </Columns>
                                </GGS:BulkEditGridView>
                            </div>
                            <div id="coverClip" class="coverClipBorder"></div>
                        </div>
                        <div id="divOdsTabImages" >
                            <div id="divOdsImages" class="odsGrid">
                                <GGS:BulkEditGridView ID="gvImages" runat="server" AutoGenerateColumns="False" DataSourceID="sqlDataSource_InstructionImages" DataKeyNames="DisplayID" 
                                GridLines="None" CssClass="odsGrid" HeaderStyle-CssClass="odsGrid_HeaderStyle">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderStyle Width="30px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Literal ID="rbSelectorImages" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <GGS:BulkEditBoundField DataField="InstructionID" AllowedToClear="false" HeaderText="InstructionID"/>
                                    <GGS:BulkEditBoundField DataField="ImageID" AllowedToClear="false" HeaderText="ImageID"  />
                                    <GGS:BulkEditBoundField DataField="DisplayID" HeaderText="Image" Frozen="true" ReadOnly="true" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="imageID"/>
                                    <asp:TemplateField HeaderText="Image Location">
                                        <HeaderStyle Width="415px" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtImagePath" runat="server" Text='<%# Bind("ImagePath") %>' Width="380"></asp:TextBox>
                                            <asp:Button ID="btnBrowse" runat="server" Text="..." CssClass="browseButton" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <ControlStyle Width="252" />
                                        <ItemStyle CssClass="odsGrid_ItemStyle" Width="252px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ImageMCR" HeaderText="MCR">
                                        <ControlStyle Width="60" />
                                        <ItemStyle CssClass="odsGrid_ItemStyle" Width="60px" />
                                    </asp:BoundField>
                                </Columns>
                                </GGS:BulkEditGridView>
                            </div>
                            <div id="divOdsImageChecks" class="odsGrid">
                                <GGS:BulkEditGridView ID="gvImageChecks" runat="server" AutoGenerateColumns="False" DataKeyNames="DisplayID" 
                                    GridLines="None" CssClass="odsGrid" HeaderStyle-CssClass="odsGrid_HeaderStyle">
                                    <Columns>
                                        <GGS:BulkEditBoundField DataField="InstructionID" AllowedToClear="false" HeaderText="InstructionID" />
                                        <GGS:BulkEditBoundField DataField="ImageID" AllowedToClear="false" HeaderText="ImageID" />
                                        <GGS:BulkEditBoundField DataField="DisplayID" HeaderText="DisplayID" Frozen="true" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                </GGS:BulkEditGridView>
                            </div>
                            <div id="coverClipForImages" class="coverClipBorderForImages"></div>
                        </div>
                    </div>
                </div>
                <div id="bottomPanel">
                    <div id="cmdPanel">
                        <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="inputButton" />
                        <asp:Button ID="cmdCopy" runat="server" Text="Copy" CssClass="inputButton stepsOnly" />
                        <asp:Button ID="cmdPrint" runat="server" Text="Print" CssClass="inputButton" />
                        <asp:Button ID="cmdClearRow" runat="server" Text="Clear Row" CssClass="inputButton" />
                    </div>
                    <div class="divAuxList datagrid">
                        <asp:Label ID="lblAuxListB" runat="server" CssClass="spanLabel">Auxiliary List B</asp:Label>
                        <asp:CheckBoxList ID="cblAuxListB" runat="server" RepeatColumns="1" RepeatLayout="Flow" CssClass="checkBoxList css-checkbox-td">
                        </asp:CheckBoxList>
                    </div>
                    <div class="divAuxList datagrid">
                        <asp:Label ID="lblAuxListA" runat="server" CssClass="spanLabel">Auxiliary List A</asp:Label>
                        <asp:CheckBoxList ID="cblAuxListA" runat="server" RepeatColumns="3" RepeatLayout="Flow" CssClass="checkBoxList css-checkbox-td">
                        </asp:CheckBoxList>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDialogMCREdit" title="Enter MCR Number">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
            <p class="validationHints">
            </p>
            <fieldset class="groupBox">
            <legend class="groupBoxLegend">MCR</legend>
                    <p>
                        <span class="spanLabel icon-empty">MCR Number:</span>
                        <asp:TextBox ID="txtMCRNumberEdit" runat="server" Width="90px" CssClass="textEntry" MaxLength="10" ReadOnly="True" Enabled="False" ></asp:TextBox>
                    </p>
                    <p>
                        <span class="spanLabel icon-empty">MCR Date:</span>
                        <asp:TextBox ID="txtMCRDateEdit" runat="server" Width="90px" CssClass="textEntry" ReadOnly="True" Enabled="False" />
                    </p>
                    <p>
                        <span class="spanLabel icon-empty">MCR Status:</span>
                        <asp:DropDownList ID="ddlMCRStatusEdit" runat="server" Width="120px" CssClass="selectDropDown" Enabled="False" />
                    </p>
                    <p>
                        <span class="spanLabel icon-empty">MCR Description:</span>
                        <asp:TextBox ID="txtMCRDescriptionEdit" CssClass="textEntry" runat="server" Width="240px" Height="60px" TextMode="MultiLine" Enabled="False" ReadOnly="True" ></asp:TextBox>
                    </p>
            </fieldset>
            <fieldset class="groupBox" style="margin-top: 8px">
            <legend class="groupBoxLegend">Station Instructions</legend>
                    <p>
                        <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">Station MCR Number:</span>
                        <asp:TextBox ID="txtStationMCR" runat="server" Width="90px" CssClass="textEntry" MaxLength="10"></asp:TextBox>
                    </p>
                    <p>
                        <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">Description of Change:</span>
                        <asp:TextBox ID="txtStationChangeDescriptionEdit" CssClass="textEntry" runat="server" Width="240px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                    </p>
            </fieldset>
<%--            <fieldset class="groupBox" style="margin-top: 8px">
            <legend class="groupBoxLegend">Images MCR</legend>
                    <p>
                        <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">Images MCR Number:</span>
                        <asp:TextBox ID="TextBox2" runat="server" Width="90px" CssClass="textEntry" MaxLength="10"></asp:TextBox>
                    </p>
                    <p>
                        <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">Description of Change:</span>
                        <asp:TextBox ID="TextBox3" CssClass="textEntry" runat="server" Width="240px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                    </p>
            </fieldset>
--%>            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogMCRNew" title="Create New MCR">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            <p class="validationHints">
            </p>
            <p class="pFilterHeader">
                <span class="spanLabel spanFilterHeader filterLeft">Line:</span>
                <span class="spanLabel spanFilterHeader">Vehicle Model:</span>
            </p>
            <p class="pFilterDDL">
                <asp:DropDownList ID="ddlLineNumbersNew" runat="server" Width="180px" CssClass="selectDropDown waitCursorOnChange filterLeft" AutoPostBack="True" />
                <asp:DropDownList ID="ddlInstructionSetsNew" runat="server" Width="180px" CssClass="selectDropDown waitCursorOnChange" AutoPostBack="True" />
            </p>
            <fieldset class="groupBox">
            <legend class="groupBoxLegend">New MCR</legend>
                <p>
                    <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">MCR Number:</span>
                    <asp:TextBox ID="txtMCRNumberNew" runat="server" Width="90px" CssClass="textEntry" MaxLength="10" />
                </p>
                <p>
                    <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">MCR Date:</span>
                    <asp:TextBox ID="txtMCRDateNew" runat="server" Width="90px" CssClass="textEntry" MaxLength="10" />
                </p>
                <p>
                    <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">MCR Status:</span>
                    <asp:DropDownList ID="ddlMCRStatusNew" runat="server" Width="122px" CssClass="selectDropDown" />
                </p>
                <p>
                    <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">MCR Description:</span>
                    <asp:TextBox ID="txtMCRDescriptionNew" CssClass="textEntry" runat="server" Width="240px" Height="30px" TextMode="MultiLine"></asp:TextBox>
                </p>
            </fieldset>
            <fieldset class="groupBox" style="margin-top: 8px">
            <legend class="groupBoxLegend">Copy From</legend>
                <p>
                    <span class="ui-icon ui-icon-triangle-1-e"></span><span class="spanLabel">MCR Number:</span>
                    <GGS:WebDropDownList ID="ddlMCRNumberNewCopyFrom" runat="server" Width="246px" CssClass="selectDropDown">
                        <asp:ListItem Text="- Do Not Copy -" Value="-1" Selected="True" />
                    </GGS:WebDropDownList>
                </p>
            </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogCopy" title="Copy Station Configuration">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <p id="copyMsg" class="validationHints" runat="server"></p>
                <div id="copyBody">
                    <div id="copyTop" class="divBorder">
                        <p class="leftPanel">
                            <span class="spanLabel h1 block">Copy From:</span> <span class="spanLabel h2 block">Line:</span>
                            <asp:DropDownList ID="ddlLineNumbersCopySrc" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                            <span class="spanLabel h2 block">Station:</span>
                            <asp:DropDownList ID="ddlStationsCopySrc" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                            <asp:Label ID="lblInstructionSetCopySrc" CssClass="spanLabel h2 block" runat="server">Instruction Set:</asp:Label>
                            <asp:DropDownList ID="ddlInstructionSetsCopySrc" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                            <asp:Label ID="lblMCRNumberCopySrc" CssClass="spanLabel h2 block" runat="server">MCR Number:</asp:Label>
                            <GGS:WebDropDownList ID="ddlMCRNumberCopySrc" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                        </p>
                        <p class="rightPanel">
                            <asp:ListBox ID="lbxInstructionsCopySrc" runat="server" AutoPostBack="True" CssClass="selectListBox" Width="520" Height="240"/>
                        </p>
                    </div>
                    <p class="middleLeftPanel">
                        <span class="spanLabel h2 block copyMode">Copy Method:</span> 
                        <span style="margin-top: 2px; display: inline-block;">
                            <asp:RadioButtonList ID="rblCopyMode" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True">
                                <asp:ListItem Value="0">Copy All</asp:ListItem>
                                <asp:ListItem Value="1" Selected="False">Copy Selected</asp:ListItem>
                            </asp:RadioButtonList>
                        </span>
                        <span style="margin-top: 2px; display: inline-block;">
                            <asp:CheckBox ID="chkCopyChecks" runat="server" Text="Copy Checkmarks" CssClass="css-checkbox-td" />
                        </span>
                    </p>
                    <p class="middleRightPanel">
                        <span class="copyBtnSpan">
                            <asp:ImageButton ID="cmdMoveDownCopy" runat="server" ImageUrl="~/Images/Arrows/ArrowDownFlat.png" CssClass="copyButton" />
                            <asp:Label ID="Label3" CssClass="spanLabel block" runat="server">Add to "Copy To" List</asp:Label>
                        </span>
                        <span class="copyBtnSpan">
                            <asp:ImageButton ID="cmdMoveUpCopy" runat="server" ImageUrl="~/Images/Arrows/ArrowUpFlat.png" CssClass="copyButton" />
                            <asp:Label ID="Label4" CssClass="spanLabel block" runat="server">Remove from "Copy To" List</asp:Label>
                        </span>
                    </p>
                    <div id="copyBottom" class="divBorder">
                        <p class="leftPanel">
                            <span class="spanLabel h1 block">Copy To:</span> <span class="spanLabel h2 block">Line:</span>
                            <asp:DropDownList ID="ddlLineNumbersCopyDst" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                            <span class="spanLabel h2 block">Station:</span>
                            <asp:DropDownList ID="ddlStationsCopyDst" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                            <asp:Label ID="lblInstructionSetCopyDst" CssClass="spanLabel h2 block" runat="server">Vehicle Model:</asp:Label>
                            <asp:DropDownList ID="ddlInstructionSetsCopyDst" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                            <asp:Label ID="lblMCRNumberCopyDst" CssClass="spanLabel h2 block" runat="server">MCR Number:</asp:Label>
                            <GGS:WebDropDownList ID="ddlMCRNumberCopyDst" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                        </p>
                        <p class="rightPanel">
                            <asp:ListBox ID="lbxInstructionsCopyDst" runat="server" AutoPostBack="True" CssClass="selectListBox" Width="520" Height="240" />
                        </p>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogPrint" title="Print Watermark Confirmation">
        <p class="pCenter" >
            Printing Instructions For Current Station.
        </p>
        <p class="pCenter" >
            <asp:CheckBox ID="chkIncludeWatermark" runat="server" Text="Include Watermark" Checked="True" CssClass="selectCheckBox secureCheckBox NoColorOnChange css-checkbox-td" />
        </p>
    </div>
    <div id="divDialogSelectImage" title="Image Selection">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <p>
                    <asp:Label ID="Label6" runat="server" Text="Select an Image." CssClass="h3"></asp:Label>
                    <asp:Button ID="cmdRefreshImages" runat="server" Text="Refresh Image List" CssClass="cmdRefreshImages" />
                </p>
				<asp:Panel ID="pnlTreeview" runat="server" ScrollBars="Auto" Width="460" Height="280" CssClass="divBorder">
					<asp:TreeView ID="treeViewFolder" runat="server" BorderWidth="0" NodeIndent="10" ImageSet="XPFileExplorer" ShowLines="True" >
						<NodeStyle CssClass="treeViewNode" />
						<SelectedNodeStyle CssClass="treeViewNodeSelected ui-state-default" />
					</asp:TreeView>
				</asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <asp:SqlDataSource ID="sqlDataSource_InstructionSteps" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" 
        SelectCommandType="StoredProcedure" SelectCommand="[ods].[procSelectInstructionSteps]"
        UpdateCommandType="StoredProcedure" UpdateCommand="[ods].[procUpdateInstructionSteps]" CancelSelectOnNullParameter="False">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlLineNumbers" PropertyName="SelectedValue" Name="LineID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlStations" PropertyName="SelectedValue" Name="StationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlInstructionSets" PropertyName="SelectedValue" Name="ConfigurationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlMCRNumber" PropertyName="SelectedValue" Name="MasterChangeRequestID" DefaultValue="" ConvertEmptyStringToNull="True" />
        </SelectParameters>
        <UpdateParameters>
            <asp:ControlParameter ControlID="ddlLineNumbers" Name="LineID" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="ddlStations" Name="StationID" PropertyName="SelectedValue" Type="String" />
            <asp:ControlParameter ControlID="ddlInstructionSets" Name="instruction_ConfigurationID" PropertyName="SelectedValue" Type="Int32" />
            <asp:Parameter Name="InstructionID" Type="Int32" DefaultValue="" />
            <asp:ControlParameter ControlID="ddlMCRNumber" Name="MasterChangeRequestID" PropertyName="SelectedValue" Type="Int32" />
            <asp:Parameter Name="StepID" Type="Int32" DefaultValue="" />
            <asp:Parameter Name="DisplayID" Type="Int32" DefaultValue="" />
            <asp:Parameter Name="Color" Type="String" DefaultValue="" />
            <asp:Parameter Name="Description" Type="String" DefaultValue="" />
            <asp:ControlParameter ControlID="hidModfiedBy" Name="ModifiedBy" PropertyName="Value" Type="String" />
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDataSource_Colors" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" SelectCommandType="StoredProcedure" SelectCommand="[ods].[procSelectColors]"
        EnableCaching="True"  CacheKeyDependency="CacheDefault"/>

    <asp:SqlDataSource ID="sqlDataSource_InstructionImages" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" 
        SelectCommandType="StoredProcedure" SelectCommand="[ods].[procSelectInstructionImages]"
        UpdateCommandType="StoredProcedure" UpdateCommand="[ods].[procUpdateInstructionImages]" CancelSelectOnNullParameter="False">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlLineNumbers" PropertyName="SelectedValue" Name="LineID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlStations" PropertyName="SelectedValue" Name="StationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlInstructionSets" PropertyName="SelectedValue" Name="ConfigurationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlMCRNumber" PropertyName="SelectedValue" Name="MasterChangeRequestID" DefaultValue="" ConvertEmptyStringToNull="True" />
        </SelectParameters>
        <UpdateParameters>
            <asp:ControlParameter ControlID="ddlLineNumbers" Name="LineID" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="ddlStations" Name="StationID" PropertyName="SelectedValue" Type="String" />
            <asp:ControlParameter ControlID="ddlInstructionSets" Name="instruction_ConfigurationID" PropertyName="SelectedValue" Type="Int32" />
            <asp:Parameter Name="InstructionID" Type="Int32" DefaultValue="" />
            <asp:ControlParameter ControlID="ddlMCRNumber" Name="MasterChangeRequestID" PropertyName="SelectedValue" Type="Int32" />
            <asp:Parameter Name="ImageID" Type="Int32" DefaultValue="" />
            <asp:Parameter Name="DisplayID" Type="Int32" DefaultValue="" />
            <asp:Parameter Name="Description" Type="String" DefaultValue="" />
            <asp:Parameter Name="ImagePath" Type="String" DefaultValue="" />
            <asp:Parameter Name="ImageMCR" Type="String" DefaultValue="" />
            <asp:ControlParameter ControlID="hidModfiedBy" Name="ModifiedBy" PropertyName="Value" Type="String" />
            <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
