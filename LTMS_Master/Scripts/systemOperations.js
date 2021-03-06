// For pages that make AJAx calls include a a call to   AddAJAXSettings();

// Move the ajax calls from ProductionSchedule.aspx into this file
//                          DailyBuildQuantities.aspx into this file
//
//==================================

//==============================================================================================
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function GetProductionScheduleGetMoveList(params) {
	try {
			return $.ajax({
				type: "POST",
				url: baseUrl + "/WCF/OperationServices.svc/ProductionScheduleGetMoveList",
				data: params,
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				error: ServiceFailed
			});
	}
	catch (err) {
		alert(err);
	}
}
function GetShippingScheduleGetMoveList(params) {
    try {
        return $.ajax({
            type: "POST",
            url: baseUrl + "/WCF/OperationServices.svc/ShippingScheduleGetMoveList",
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: ServiceFailed
        });
    }
    catch (err) {
        alert(err);
    }
}
//==============================================================================================
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function GetProductionSchedule_GetProductIDList(ddl, productID) {
	try {
				$.ajax({
				type: "POST",
				url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_GetProductIDList",
				data: '{"productID": "' + productID + '"}',
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (data) { PopulateNameValueDDL(data, ddl) },
				error: ServiceFailed
			});
	}
	catch (err) {
		alert(err);
	}

}

//==============================================================================================
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function GetProductionSchedule_DialogNew_GetNValueList(ddl, broadcastPointID) {
	try {
		$.ajax({
			type: "POST",
			url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_DialogNew_GetNValueList",
			data: '{"broadcastPointID": "' + broadcastPointID + '"}',
            contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) { PopulateNameValueDDL(data, ddl) },
			error: ServiceFailed
		});
	}
	catch (err) {
		alert(err);
	}
}

////==============================================================================================
//// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
////==============================================================================================
////function GetProductionSchedule_DialogNew_LotList(ddl, lotNumber, orderType, broadcastPointID) {
////	try {
		
////		$.ajax({
////			type: "POST",
////			url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_DialogNew_LotList",
////			data: "{\"lotNumber\": \"" + lotNumber + "\", \"boolSetexOrder\": \"" + orderType + "\", \"broadcastPointID\": \"" + broadcastPointID + "\"}",
////			contentType: "application/json; charset=utf-8",
////			dataType: "json",
////			success: function (data) { PopulateNameValueDDL(data, ddl); setControls(); },
////			error: ServiceFailed
////		});
////	}
////	catch (err) {
////		alert(err);
////	}
////}


//==============================================================================================
// Used on page:  1Operations/0ProductionSchedule/ProductionSchedule30.aspx
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function GetProductionSchedule_DialogNew_LotList(ddl, lotNumber, orderType, broadcastPointID) {
	try {

		$.ajax({
			type: "POST",
			url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_DialogNew_LotList",
			data: "{\"lotNumber\": \"" + lotNumber + "\", \"boolSetexOrder\": \"" + orderType + "\", \"broadcastPointID\": \"" + broadcastPointID + "\"}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) { PopulateNameValueDDL(data, ddl); setControls(); },
			error: ServiceFailed
		});
	}
	catch (err) {
		alert(err);
	}
}



//////==============================================================================================
////// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//////==============================================================================================
////function GetProductionSchedule_DialogNew_PopulateDetails(hidNodeSeqDT, broadcastPointID) {
////	try {
////		$.ajax({
////			type: "POST",
////			url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_Dialog_PopulateDetails",
////			data: "{\"seqNum\": \"" + hidNodeSeqDT + "\", \"broadcastPointID\": \"" + broadcastPointID + "\"}",
////			contentType: "application/json; charset=utf-8",
////			dataType: "json",
////			success: function (data) { DialogNewPopulateDetail(data) },
////			error: ServiceFailed
////		});
////	}
////	catch (err) {
////		alert(err);
////	}
////}

//==============================================================================================
// Used on page:  1Operations/0ProductionSchedule/ProductionSchedule30.aspx
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function GetProductionSchedule_DialogNew_PopulateDetails(hidNodeSeq, broadcastPointID) {
	try {
		$.ajax({
			type: "POST",
			url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_Dialog_PopulateDetails",
			data: "{\"seqNum\": \"" + hidNodeSeq + "\", \"broadcastPointID\": \"" + broadcastPointID + "\"}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) { DialogNewPopulateDetail(data) },
			error: ServiceFailed
		});
	}
	catch (err) {
		alert(err);
	}
}

//==============================================================================================
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function ProductionSchedule_DialogNew_PopulateProductDetail(ProductID) {
    try {
        $.ajax({
            type: "POST",
            url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_Dialog_PopulateProductDetail",
            data: "{\"ProductID\": \"" + ProductID + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) { DialogNewPopulateProductDetail(data) },
            error: ServiceFailed
        });
    }
    catch (err) {
        alert(err);
    }
}
//==============================================================================================
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function ProductionSchedule_DialogEdit_PopulateProductDetail(ProductID) {
    try {
        $.ajax({
            type: "POST",
            url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_Dialog_PopulateProductDetail",
            data: "{\"ProductID\": \"" + ProductID + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) { DialogEditPopulateProductDetail(data) },
            error: ServiceFailed
        });
    }
    catch (err) {
        alert(err);
    }
}

//==============================================================================================
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function GetProductionSchedule_DialogEdit_PopulateDetails(hidNodeSeqDT, broadcastPointID) {
	try {
		$.ajax({
			type: "POST",
			url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_Dialog_PopulateDetails",
			data: "{\"seqNum\": \"" + hidNodeSeqDT + "\", \"broadcastPointID\": \"" + broadcastPointID + "\"}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (data) { DialogEditPopulateDetail(data) },
			error: ServiceFailed
		});
	}
	catch (err) {
		alert(err);
	}
}

//==============================================================================================
// Used on page:  1Operations\3DailyBuildQuantities\DailyBuildQuantities.aspx
//==============================================================================================
function GetDailyBuildQuantities_DialogEdit_DailyBuildGetEndLots(ddl, hid, params, successFunction1, successFunction2) {
	try {
	    $.ajax({
	        type: "POST",
	        url: baseUrl + "/WCF/OperationServices.svc/DailyBuildGetEndLots",
	        data: params,
	        contentType: "application/json; charset=utf-8",
	        dataType: "json",
	        success: function (data) {
	            PopulateNameValueDDL(data, ddl);

	            $(hid).val($(ddl).val());

	            if (!(successFunction1 === null)) { successFunction1.call(); }
	            if (!(successFunction2 === null)) { successFunction2.call(); }
	        },
	        error: ServiceFailed
	    });
	}
	catch (err) {
		alert(err);
	}
}

//==============================================================================================
// Used on page:  1Operations\3DailyBuildQuantities\DailyBuildQuantities.aspx
//==============================================================================================
function GetDailyBuildQuantities_DialogEdit_DailyBuildGetLotSize(param) {
	try {
		return $.ajax({
			type: "POST",
			url: baseUrl + "/WCF/OperationServices.svc/DailyBuildGetLotSize",
			data: param,
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			error: ServiceFailed
		});
	}
	catch (err) {
		alert(err);
	}
}

//==============================================================================================
// Used on page:  1Operations/1ProductionSchedule/ProductionSchedule.aspx
//==============================================================================================
function GetProductionSchedule_DialogMove_BroadcastPointIDs(ddl, selectedBroadcastPointID) {
    try {

        $.ajax({
            type: "POST",
            url: baseUrl + "/WCF/OperationServices.svc/ProductionSchedule_DialogMove_BroadcastPointIDs",
            data: "{\"selectedBroadcastPointID\": \"" + selectedBroadcastPointID + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) { PopulateNameValueDDL(data, ddl); },
            error: ServiceFailed
        });
    }
    catch (err) {
        alert(err);
    }
}

//==============================================================================================
// Used on page:  1Operations/5LotTrace/1LotTraceData/LotTraceData.aspx
//==============================================================================================
function LotTraceData_DialogModifyComponentHistory_VerifyComponentScan(StationID, ComponentScan, ComponentName, StyleGroupID , ProductID , ComponentNameIDX ) {
    try {
        return $.ajax({
            type: "POST",
            async: false,
            url: baseUrl + "/WCF/OperationServices.svc/LotTraceData_DialogModifyComponentHistory_VerifyComponentScan",
            data: "{\"StationID\": \"" + StationID +
                  "\", \"ComponentScan\": \"" + ComponentScan +
                  "\", \"ComponentName\": \"" + ComponentName +
                  "\", \"StyleGroupID\": \"" + StyleGroupID +
                  "\", \"ProductID\": \"" + ProductID + 
                  "\", \"ComponentNameIDX\": \"" + ComponentNameIDX + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: ServiceFailed
        });
    }
    catch (err) {
        alert(err);
    }
}
