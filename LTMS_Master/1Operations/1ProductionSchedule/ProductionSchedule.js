function AddEventsToControls() {

    $("#MainContent_rbOrders").buttonset(); // Make the Radio button a UI radio button
    $("#MainContent_rbOrders_0 + label").mouseover(function () { if ($orderType == "1") { $(this).css('cursor', 'text'); } else { $(this).css('cursor', 'pointer').click(function () { ShowWaitOverlay(); }); } });
    $("#MainContent_rbOrders_1 + label").mouseover(function () { if ($orderType == "0") { $(this).css('cursor', 'text'); } else { $(this).css('cursor', 'pointer').click(function () { ShowWaitOverlay(); }); } });
    $("#divFilters input:image").mouseover(function () { $(this).css('cursor', 'pointer'); });

    $(".cmdShowWait, #MainContent_cbHold").on('click', function () { ShowWaitOverlay(); });

    $("#MainContent_cmdPrint").on('click', function () { return OpenProductionScheduleReport(); });

    // Add dialogs to button
    $cmdResequence.on('click', function () { $('#divDlgResequence').dialog('open'); ClearWaitOverlay(); return false; });

    $("#MainContent_cmdExport").on('click', function () { ExportSchedule(); return false; });

    // make controls with class 'numericOnly' into numeric only
    $('.numericOnly').numeric({ allowMinus: false, allowThouSep: false, allowDecSep: false });
}

 function MakeDivIntoTabs() {

    $("#tabs").tabs({

        active: $hidLastTab.val(),    //set to previously Selected Tab (for postback)

        beforeActivate: function (event, ui) {
            $hidLastTab.val(ui.newTab.index()); // when the tabs are selected set the hidden field

            $("#divToggleButtons").toggle();       //remove these buttons on single day view
            $("#divComments").toggle();            //remove this div on single day view

            ShowWaitOverlay();
            __doPostBack($cmdRefresh.prop('name'), '');
        }
    });
}

function MakeDateTimePicker() {
    $tbDay.mask("99/99/9999").addDatePicker()
	    .datepicker("option", "firstDay", 1)
	    .datepicker({ onSelect: function (dateText, inst) { DoRefreshPostBack(dateText); }}).change(function () { DoRefreshPostBack(this.value); });

    // position the calendar icon
    PositionCalendarPickerIcon();
}

function DoRefreshPostBack(dateText) {
    d1 = Date.parse(dateText);
    if (d1 != null) {ShowWaitOverlay(); __doPostBack($cmdRefresh.prop('name'), '');  }
}

function RefreshAfterMove() { __doPostBack($cmdRefresh.prop('name'), ''); }

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

function CreateDialog_Delete() {
    var $dlgDelete = $('#divDlgDelete');

    $cmdDeleteDlg.click(function () {
        if ($hidNodeLot.val() != null) {
            var delLot = $hidNodeLot.val().substring(0, 10);
            $("#MainContent_lblLotToDelete").text(delLot);
            $('#divDlgDelete').dialog('open');
        } else {
            alert("Please Select a Lot or Sublot");
        }
        ClearWaitOverlay();
        return false; 
    });

    $dlgDelete.deleteDialog({ control: $cmdDelete, width: 360 });
}

function CreateDialog_Resequence() {
    var $dlgResequence = $('#divDlgResequence');
    $dlgResequence.modalDialog({
        control: $cmdResequence,
        width: 320
    });
}

function OpenProductionScheduleReport() {
    var url = baseUrl + '/2Reporting/Reports.aspx?report=';

    if ($orderType == "1") {
        url += "ProductionSchedule&webpageTitle=Production Schedule Report (Setex)";
    } else {
        url += "ProductionScheduleHonda&webpageTitle=Production Schedule Report (Honda)";
    }
     
    d1 = Date.parse($tbDay.val());
    if (d1 == null) {
        alert('Error:  Invalid Date.');
    } else {
        if (!d1.is().monday()) {
            d1.last().monday();
        }
        url += '&BegDT=' + d1.toString("MM/dd/yyyy");
        url += '&BroadcastPointID=NULL';
    }
    window.open(url);
    return false;
}

//function ExportSchedule() {
//    var params;
//    var flag;
//    if ($orderType == "1") { //setex
//        params = $("#MainContent_lblDateMon").text();
//        flag=1;
//    } else {
//        params = $("#MainContent_lblDateMon").text();
//        flag=0;
//    }
//    window.open('ProductionScheduleExport.aspx?exportDate=' + params + '&SetexFlag=' + flag + '&BroadcastPointID=1');
//}

function getMonday(d) {
    d = new Date(d);
    var day = d.getDay(),
        diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
    return new Date(d.setDate(diff));
}

function ExportSchedule() {
    var params;
    var flag;
    d = new Date($("#MainContent_tbDay").val());

    d = getMonday(d);

    params = d.getMonth() + 1 + '/' + d.getDate() + '/' + d.getFullYear();
    if ($orderType == "1") { //setex
        //params = $("#MainContent_lblDateMon").text();
        flag = 1;
    } else {
        //params = $("#MainContent_lblDateMon").text();
        flag = 0;
    }
    window.open('ProductionScheduleExport.aspx?exportDate=' + params + '&SetexFlag=' + flag + '&BroadcastPointID=1');
}

function RadTreeView_OnClientNodeClicked(sender, eventArgs) {
    $hidLotNum8.val("");
    $hidNodeLot.val("");
    $hidNodeSeqDT.val("");
    $hidBroadcastPointID.val("");
    $hidNodeLevel.val("");
    

    var treeNode = eventArgs.get_node();


    /* pah 20220221 */
    var snodes = sender.get_selectedNodes();
    var nodeLevel = treeNode.get_level();

    for (var i = 0; i < snodes.length; i++) {
        if (snodes.length == 1) {
            $hidNodeLot.val("");
            $hidNodeLot.val(snodes[i].get_text());
        }
        else if (snodes.length == 2) {
            $hidNodeLot.val("");
            for (var i = 0; i < snodes.length; i++) {
                if (nodeLevel == 1) { $hidNodeLot.val(snodes[i].get_text().substring(0, 10) + " " + $hidNodeLot.val()); }
                if (nodeLevel == 2) { $hidNodeLot.val(snodes[i].get_text().substring(0, 13) + " " + $hidNodeLot.val()); }
            }
        }
        else {
            alert("A Maximum of 2 May Be Selected");
        }
    }
    /*     */

    //var nodeLevel = treeNode.get_level();
    if (nodeLevel == 0) {
        treeNode.set_selected(false);
    } else {
        var lot = treeNode.get_text();
        var SeqDT = treeNode.get_value();
        var lot8 = replaceAll(lot, '-', '').substring(0, 8);
        $hidLotNum8.val(lot8)
        //$hidNodeLot.val(lot);
        $hidNodeSeqDT.val(SeqDT);
        $hidNodeLevel.val(nodeLevel);
        $hidBroadcastPointID.val(lot8.substring(0, 1));

        if ($treeMon !== sender) { $treeMon.unselectAllNodes(); } else { $hidTreeID.val(1); }
        if ($treeTue !== sender) { $treeTue.unselectAllNodes(); } else { $hidTreeID.val(2); }
        if ($treeWed !== sender) { $treeWed.unselectAllNodes(); } else { $hidTreeID.val(3); }
        if ($treeThu !== sender) { $treeThu.unselectAllNodes(); } else { $hidTreeID.val(4); }
        if ($treeFri !== sender) { $treeFri.unselectAllNodes(); } else { $hidTreeID.val(5); }
        if ($treeSat !== sender) { $treeSat.unselectAllNodes(); } else { $hidTreeID.val(6); }
        if ($treeSun !== sender) { $treeSun.unselectAllNodes(); } else { $hidTreeID.val(7); }
    }
    EnableCommandButtons(nodeLevel);
}

function SetButtonsEnabledState() {
    // Set the Action buttons to disabled intially until a tree is selected.
    if ($hidNodeLot.val() == "") {
        $cmdNew.button("disable");
        $cmdMove.button("disable");
        $cmdEdit.button("disable");
        $cmdDelete.button("disable");
        $cmdMoveDlg.button("disable");
        $cmdEditDlg.button("disable");
        $cmdDeleteDlg.button("disable");

        $cmdOnHold.button("disable");
        $cmdOffHold.button("disable");
    } else {
        EnableCommandButtons(1*$hidNodeLevel.val());
    }
}

function EnableCommandButtons(nodeLevel) {
    var nodeType = ""
    var $selectedNodeOnHold = false;

    if ($orderType == "0" && nodeLevel > 0) {
        $cmdNew.button("enable");
    } else {
        $cmdNew.button("enable");
    }
 
    switch (nodeLevel) {
        case 0:
            //    case 1:   
            nodeType = "SHIFT";
            $cmdEdit.button("disable");
            $cmdEditDlg.button("disable");
            $cmdMove.button("disable");
            $cmdMoveDlg.button("disable");
            $cmdDelete.button("disable");
            $cmdDeleteDlg.button("disable");
            $cmdOnHold.button("disable");
            $cmdOffHold.button("disable");
            break;
        case 1:
            nodeType = "LOT";
            //enable and disable the following buttons
            $cmdEdit.button("disable");
            $cmdEditDlg.button("disable");
            $cmdMove.button("enable");
            $cmdMoveDlg.button("enable");
            $cmdDelete.button("enable");
            $cmdDeleteDlg.button("enable");
            $cmdOnHold.button("enable");
            $cmdOffHold.button("enable");
            break;
        case 2:
            nodeType = "SUBLOT";
            //enable and disable the following buttons
            $cmdEdit.button("enable");
            $cmdEditDlg.button("enable");
            $cmdMove.button("enable");
            $cmdMoveDlg.button("enable");
            $cmdDelete.button("enable");
            $cmdDeleteDlg.button("enable");
            $cmdOnHold.button("disable");
            $cmdOffHold.button("disable");
            break;
        default:
            nodeType = "";
            $cmdEdit.button("disable");
            $cmdEditDlg.button("disable");
            $cmdMove.button("disable");
            $cmdMoveDlg.button("disable");
            $cmdDelete.button("disable");
            $cmdDeleteDlg.button("disable");
            $cmdOnHold.button("disable");
            $cmdOffHold.button("disable");
            break;
    }
    return nodeType;
}

function treeMove_OnClientNodeDragging(sender, eventArgs) {
    //var treeNode = eventArgs.get_node();
    //var nodeLevel = treeNode.get_level();
    //if (nodeLevel != 0)
        //eventArgs.set_cancel(true);
}

function treeMove_OnClientNodeDropping(sender, eventArgs) {
    //var sourceNodes = eventArgs.get_sourceNodes();
    //var destNode = eventArgs.get_destNode();
    //var plurality = (sourceNodes.length > 1) ? "s" : "";

    //if (sourceNodes[0].get_level() == destNode.get_level()) {
    //    var message = "Are you sure you want to move the Lot" + plurality + "\n";

    //    for (var i = 0; i < sourceNodes.length; i++) {
    //        message += "\t" + sourceNodes[i].get_text() + "\n";
    //    }

    //    message += ((eventArgs.get_dropPosition() == "above") ? "before" : "after");
    //    message += " Lot\n\t" + destNode.get_text() + "?";

    //    //PAH 20191002
    //    if (sourceNodes.length > 2) {
    //        alert("No more than 2 Lots may be moved at one time.");
    //        eventArgs.set_cancel(true);
    //    }

    //    else {
    //        if (!confirm(message))
    //            eventArgs.set_cancel(true);
    //    }
    //}

    //else {
    //    eventArgs.set_cancel(true);
    //}
}

function RadAjaxPanel_OnResponseEnd(sender, eventArgs) {
//    alert('Response end initiated by: ' + eventArgs.get_eventTarget());
    //if (eventArgs.get_eventTarget() === $ddlMoveBP._uniqueId) {
    //    MoveLot_ScrollSelectedNodeIntoView();
    //}
    //else if (eventArgs.get_eventTarget() === $treeMove._uniqueId) {
    //    MoveLot_ScrollSelectedNodeIntoView();
    //}
}

function RadTreeView_onClientContextMenuShowing(sender, eventArgs) {
    var treeNode = eventArgs.get_node();
    setMenuItemsState(eventArgs.get_menu().get_items(), treeNode);   //enable/disable menu items
        
    //    var menuItem = eventArgs.get_menuItem();
    //    switch (menuItem._text) {
    //        case "Expand Allxxx":
    //            RadTreeView_ExpandAll(sender);
    //            break;
    //        case "Collapse Allxxx":
    //            RadTreeView_CollapseAll(sender);
    //            break;
    //    }
}

//this method disables the appropriate context menu items
function setMenuItemsState(menuItems, treeNode) {
    var onhold = (treeNode.get_text().indexOf("ON HOLD") !== -1);
    /*var onhold = (treeNode.selectedNode.);*/
    var isSelected = treeNode.get_selected();

    for (var i = 0; i < menuItems.get_count(); i++) {
        var menuItem = menuItems.getItem(i);
        switch (menuItem.get_value()) {
            case "OnHold":
                menuItem.set_enabled(treeNode.get_level() === 1 && !onhold && isSelected);
                break;
            case "OffHold":
                menuItem.set_enabled(treeNode.get_level() === 1 && onhold && isSelected);
                break;
        }
    }
}

function CreateDialog_MoveLot() {
    var $dlgMove = $('#divDlgMoveLot');
    $dlgMove.modalDialog({
        addButton_Cancel: false,
        btnOk_text: 'Close',
        width: 300,
        openFunction: function () { MoveLot_onOpen(); }
    });
    $cmdMoveDlg.click(function () {
        $dlgMove.dialog('open');
        return false;
    });
}

function MoveLot_onOpen() {
    ddlMoveBP_SetSelected();
}

function ddlMoveBP_SetSelected(){
//    $ddlMoveBP.trackChanges();
    var item;
    item = $ddlMoveBP.findItemByValue($hidNodeLot.val().substring(0, 1));
    item.select();
//    $ddlMoveBP.commitChanges();
}

function MoveLot_ScrollSelectedNodeIntoView() {
    var selectedNode = $treeMove.get_selectedNode();
    if (selectedNode != null) {
        window.setTimeout(function () { selectedNode.scrollIntoView(); }, 200);
    }
}


function CreateDialog_New() {
    var $dlgNew = $('#divDlgNewLot');
    var $Temp;
    $cmdNew.click(function () {
        var bpid = $hidNodeLot.val().substring(0, 1);
        $dlgNew.dialog('open');
        ShowCursor_Wait();
        $("#divDlgNewOverlay").fadeIn();
        $("#MainContent_lblNewSelectedBroadcastPointID").text(bpid);

        $Temp = replaceAll($hidNodeLot.val(), '-', '').substring(0, 10);
        $hidNodeSeq = $hidNodeSeqDT.val().replace(/-/g, "");

        /*GetProductionSchedule_DialogNew_PopulateDetails($hidNodeSeq, bpid);*/
        GetProductionSchedule_DialogNew_PopulateDetails($Temp, bpid);
        GetProductionSchedule_DialogNew_GetNValueList($ddlNewNValues, bpid);
        GetProductionSchedule_DialogNew_LotList($ddlNewLotNew, $hidNodeLot.val(), $orderType, bpid);
        return false;
    });

    $dlgNew.modalDialog({
        control: $cmdNew,
        width: 400,
        validationFunction: function () { return ValidateDialog_New(); }
    });
}

function CreateDialog_Edit() {
    try {
        var $dlgEdit = $('#divDlgEditLot');
        var $Temp;
        $cmdEditDlg.click(function () {
            $dlgEdit.dialog('open');
            ShowCursor_Wait();
            $("#divDlgEditOverlay").fadeIn();
            $Temp = replaceAll($hidNodeLot.val(), '-', '').substring(0, 10);
            GetProductionSchedule_DialogEdit_PopulateDetails($Temp, $hidNodeLot.val().substring(0, 1));
            $hidEditProdID.val($ddlEditProdID.val());
            return false;
        });

        $dlgEdit.modalDialog({
            control: $cmdEdit,
            width: 410,
            validationFunction: function () { return ValidateDialog_Edit(); }
        });
    } catch (err) {
        alert(err);
    }
}

function OpenMoveDialog() {
    //get all selected nodes
    $cmdMoveDlg.click();
}

function CreateDialog_MoveLots() {
    var $cmdMove = $('#MainContent_cmdMoveDlg');
    var $dlgMove = $('#divDlgMoveLots');

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
            /*$("#MainContent_lblLotNum").text($hidLTP.val());*/
            /* $("#MainContent_lblLotNum").text($hidNodeSDT.val());*/
            $("#MainContent_lblLotNum").text($hidNodeLot.val());
        }
        else if (nodeLevel == 2) {
            $("#MainContent_lblType").text("SUBLOT");
            /*$("#MainContent_lblLotNum").text($hidNodeSubLot.val()); */
            $("#MainContent_lblLotNum").text($hidNodeLot.val()); /*PAH*/
        }

        $ddlLotNum.empty(); 
       
        LoadDDLMoveLotNum($hidNodeLot.val().substring(0, 1));
       /* LoadDDLMoveLotNum($hidNodeSDT.val().substring(0, 1));*/
        DoRefreshPostBack();
        ClearWaitOverlay();
        return false;
    });
}

function LoadDDLMoveLotNum(broadcastPointID) {
    var lot = $hidNodeLot.val();

    $hidBroadcastPointID.val(broadcastPointID);

    var param = "{\"nodeLevel\": \"" + $hidNodeLevel.val() +
        "\", \"seqDT\": \"" + $hidNodeSeqDT.val() +
        "\", \"lot\": \"" + lot +
        "\", \"boolSetexOrder\": \"" + $orderType +    // don't know why, but this was hardcoded as a 1 in the old site. I think becuase there is no option to between all orders and Setex ones
        "\", \"broadcastPointID\": \"" + broadcastPointID +
        "\"}";

    GetProductionScheduleGetMoveList(param).success(function (data) {
        PopulateNameValueDDL(data, $ddlLotNum);       
        $hidLotNum.val($ddlLotNum.val());
        ShowCursor_Default();
        $("#divDlgMoveOverlay").fadeOut();        
    });
}

function ValidateDialog_New() {
    var bValid = false;
    var $txtNewQuantity = $("#MainContent_txtNewQuantity");

    try {
        bValid = checkDropDownList($ddlNewProdID, "Enter a value for Frame Code.");
        bValid = checkText($txtNewQuantity, "Enter a value for Job Quantity.", $('#helptxtNewQuantity')) && bValid;

        bValid = checkLength($txtNewSLIndex, "Sub Lot Number", 0, 2, $('#helptxtNewSLIndex')) && bValid;
        bValid = checkText($txtNewSLIndex, "Enter a value for Sub Lot Index.", $('#helptxtNewSLIndex')) && bValid;

        if ($txtNewLotNum.val().replace(/-/g, '').length != 8) {
            bValid = false;

            setValidationHint('Invalid Lot Number use format: 0-1234-567');
            $txtNewLotNum.addClass("ui-state-error");
            HelpIconHideorShow(true, $("#helptxtNewLotNum"));
        }

        bValid = checkText($txtNewLotNum, "Enter a value for Lot Number.", $("#helptxtNewLotNum")) && bValid;

    } catch (err) {
        alert(err);
    }
    return bValid;
}

function ValidateDialog_Edit() {
    var bValid = false;
    var $txtEditQuantity = $("#MainContent_txtEditQuantity");

    try {
        //  compare in reverse order, then the error messages will display in order
        bValid = checkText($txtEditQuantity, "Enter a value for Job Quantity.", $('#helptxtEditQuantity'));

        bValid = checkLength($txtEditSLIndex, "Sub Lot Number", 0, 2, $('#helptxtEditSLIndex')) && bValid;
        bValid = checkText($txtEditSLIndex, "Enter a value for Sub Lot Index.", $('#helptxtEditSLIndex')) && bValid;

    } catch (err) {
        alert(err);
    }
    return bValid;
}

function DialogNewPopulateDetail(strResult) {
    try {
        $("#MainContent_txtNewLotNum").val(strResult.d.LotNumber);
        $("#MainContent_txtNewSLIndex").val(strResult.d.SubLotIndex);
        $("#MainContent_txtNewQuantity").val(strResult.d.Quantity);
        $("#MainContent_lblNewShipCode4").text(strResult.d.ShipCode4);
        $("#MainContent_lblNewDriverSS").text(strResult.d.DriverSeatStyle);
        $("#MainContent_lblNewPassSS").text(strResult.d.PassengerSeatStyle);
        $("#MainContent_lblNewModelDesc").text(strResult.d.ModelDescription);
        GetProductionSchedule_GetProductIDList($ddlNewProdID, strResult.d.ProductID);
    }
    catch (Error) {
        alert(Error);
    }
}

function DialogNewPopulateProductDetail(strResult) {
    try {
        $("#MainContent_lblNewShipCode4").text(strResult.d.ShipCode4);
        $("#MainContent_lblNewDriverSS").text(strResult.d.DriverSeatStyle);
        $("#MainContent_lblNewPassSS").text(strResult.d.PassengerSeatStyle);
        $("#MainContent_lblNewModelDesc").text(strResult.d.ModelDescription);
    }
    catch (Error) {
        alert(Error);
    }
}

function DialogEditPopulateDetail(strResult) {
    try {
        $("#MainContent_txtEditLotNum").val(strResult.d.LotNumber);
        $txtEditSLIndex.val(strResult.d.SubLotIndex);
        $("#MainContent_txtEditQuantity").val(strResult.d.Quantity);
        $("#MainContent_lblEditShipCode4").text(strResult.d.ShipCode4);
        $("#MainContent_lblEditDriverSS").text(strResult.d.DriverSeatStyle);
        $("#MainContent_lblEditPassSS").text(strResult.d.PassengerSeatStyle);
        $("#MainContent_lblEditModelDesc").text(strResult.d.ModelDescription);
        $("#MainContent_txtEditNotes").val(strResult.d.mProductionNotes);
        GetProductionSchedule_GetProductIDList($ddlEditProdID, strResult.d.ProductID);
        ShowCursor_Default();
        $("#divDlgEditOverlay").fadeOut();
    }
    catch (Error) {
        alert(Error);
    }
}

function DialogEditPopulateProductDetail(strResult) {
    try {
        $("#MainContent_lblEditShipCode4").text(strResult.d.ShipCode4);
        $("#MainContent_lblEditDriverSS").text(strResult.d.DriverSeatStyle);
        $("#MainContent_lblEditPassSS").text(strResult.d.PassengerSeatStyle);
        $("#MainContent_lblEditModelDesc").text(strResult.d.ModelDescription);
    }
    catch (Error) {
        alert(Error);
    }
}


function setControls() {
    try {
        $hidNewProdID.val($ddlNewProdID.val());
        $hidNewLotNew.val($ddlNewLotNew.val());
        $hidNewNValues.val($ddlNewNValues.val());

        var cb = document.getElementById('MainContent_cbNewNextPos');
        cb.disabled = false;

        if (searchLots() == true) {
            disableDlgLocationControls();
        }
        else if (cb.checked == true) {
            disableSomeControls();
        }
        else {
            enableAllControls();
        }

        ShowCursor_Default();
        $("#divDlgNewOverlay").fadeOut();

    } catch (error) {
        alert('Error: ' + error)
    }
}

function searchLots() {
    try {
        var input = document.getElementById('MainContent_txtNewLotNum').value.toLowerCase();
        var ddl = document.getElementById('MainContent_ddlNewLotNew');
        var ddlOptions = document.getElementById('MainContent_ddlNewLotNew').options;

        input = input.replace(/-/g, '');

        for (var i = 0; i < ddlOptions.length; i++) {
            if (ddlOptions[i].value.toLowerCase() == input) {
                return true;
            }
        }
        return false;
    } catch (error) {
        alert('Error: ' + error)
    }
}

function disableDlgLocationControls() {
    try {
        document.getElementById('MainContent_ddlNewLotNew').disabled = true;
        document.getElementById('MainContent_rblNewLocation').disabled = true;
        document.getElementById('MainContent_cbNewNextPos').disabled = true;
        document.getElementById('MainContent_cbNewNextPos').parentNode.disabled = true;
    }
    catch (error) {
        alert('Error: ' + error)
    }
}

function disableSomeControls() {
    try {
        document.getElementById('MainContent_ddlNewLotNew').disabled = true;
        document.getElementById('MainContent_rblNewLocation').disabled = true;
    }
    catch (error) {
        alert('Error: ' + error)
    }
}

function enableAllControls() {
    try {
        document.getElementById('MainContent_ddlNewLotNew').disabled = false;
        document.getElementById('MainContent_rblNewLocation').disabled = false;
        document.getElementById('MainContent_cbNewNextPos').disabled = false;
        document.getElementById('MainContent_cbNewNextPos').parentNode.disabled = false;
    }
    catch (error) {
        alert('Error: ' + error)
    }
}

