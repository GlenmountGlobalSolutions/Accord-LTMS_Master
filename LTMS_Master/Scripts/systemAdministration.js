// For pages that make AJAx calls include a a call to   AddAJAXSettings();

//==============================================================================================
// Used on page \4SystemAdministration\2SecurityManagement\1UserAccounts\UserAccounts.aspx
//==============================================================================================
function DoesLogInNameExist(loginName) {
    var returnResult = false;

    $.ajax({
        type: "POST",
        async: false,
        url: baseUrl + "/WCF/AdministrationServices.svc/DoesLogInNameExist",
        data: "{\"loginName\": \"" + loginName + "\"}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) { returnResult = data.d; },
        error: ServiceFailed
    });
    return returnResult;
};
//==============================================================================================

