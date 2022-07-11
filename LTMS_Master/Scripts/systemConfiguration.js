// For pages that make AJAx calls include a a call to   AddAJAXSettings();

//==============================================================================================
// Used on pages:
//  3SystemConfiguration\1StationConfiguration\4StationComponentScanAssignment\StationComponentScanAssignment.aspx
//  3SystemConfiguration\1StationConfiguration\5StationTaskConfiguration\StationTaskConfiguration.aspx
//  3SystemConfiguration\1StationConfiguration\6StationToolConfiguration\StationToolConfiguration.aspx
//  3SystemConfiguration\80StyleGroupConfiguration\StyleGroupConfiguration.aspx.vb
//
//==============================================================================================
function LoadStyleGroups(ddl, lineNumber) {
    try {
        var methodParameters = "";

        if (lineNumber > 0) {

            methodParameters = '{"lineNumber": "' + lineNumber + '"}';

            $.ajax({
                type: "POST",
                url: baseUrl + "/WCF/ConfigurationServices.svc/GetStyleGroupsByLineNumber",
                data: methodParameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    PopulateNameValueDDL(data, ddl);
                },
                error: ServiceFailed
            });
        }
    }
    catch (err) {
        alert(err);
    }
}

//==============================================================================================
// Used on pages:
//  3SystemConfiguration\1StationConfiguration\4StationComponentScanAssignment\StationComponentScanAssignment.aspx
//  3SystemConfiguration\1StationConfiguration\5StationTaskConfiguration\StationTaskConfiguration.aspx
//  3SystemConfiguration\1StationConfiguration\6StationToolConfiguration\StationToolConfiguration.aspx
//  3SystemConfiguration\80StyleGroupConfiguration\StyleGroupConfiguration.aspx.vb
//
//==============================================================================================
function LoadStations(ddl, lineNumber) {
    try {
        var methodParameters = "";

        if (lineNumber > 0) {

            methodParameters = '{"lineNumber": "' + lineNumber + '"}';

            $.ajax({
                type: "POST",
                url: baseUrl + "/WCF/ConfigurationServices.svc/GetStationsByLineNumber",
                data: methodParameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    PopulateNameValueDDL(data, ddl);
                },
                error: ServiceFailed
            });
        }
    }
    catch (err) {
        alert(err);
    }
}
