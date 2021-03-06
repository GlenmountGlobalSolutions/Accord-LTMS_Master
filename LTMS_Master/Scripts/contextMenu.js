var $currentPage = GetCurrentPageName();

// ===============================================================
// This is a plugin to create a right click event
// Name:    rightClick
//
// method is the function that is passed in. the function should accept an event and string as parameters 
// ===============================================================
(function ($) {
    $.fn.rightClick = function (method) {
        $(this).on('contextmenu rightclick', function (event) {

            event.preventDefault();                         // stops the browser default context-menu from opening.

            var controlValue = $(this).attr('value');       // get the attibute ('value') of the control that was assinged this plugin.
                                                            // for 'input[type=submit]' this is the buttons value 
            if (typeof controlValue === "undefined") {           // if the value was undefined use the text
                controlValue = $(this).text();
            }

            method(event, controlValue);                   // pass the event and the control value to the calling method
            return false;
        })
    };
})(jQuery);

// ===============================================================
//  After the page loads, add this Javascript
// ===============================================================
function AddRightClickContextMenu() {

    if (($currentPage.length > 0) && ($currentPage.notEqualTo('Login.aspx'))) {

        AddAJAXSettings();

        // ===============================================================
        // "Click" event to the 'body' if they click outside of the dialog box.
        // ===============================================================
        $('body').unbind().on('click', function (event) {
            if ($('#divSecurityContextMenu').dialog('isOpen')
                        && !$(event.target).is('.ui-dialog, a')
                        && !$(event.target).closest('.ui-dialog').length) {
                $('#divSecurityContextMenu').dialog('close');
            }
        });

        AddRightClickToControls();
    }
}

function AddRightClickToControls() {
    // =================================================================================================
    //  Add a right click function to all submit buttons to have popup with the button text as the title    
    // ==================================================================================================
    $('input[type=submit], .pageRefresh, .secureCheckBox').rightClick(function (event, cmdName) {

        if ($currentPage.equalTo('Login.aspx')) {
            alert('Login is not subject to security.');
        } else {
            var title = 'N/A';
            var menu = $('#divSecurityContextMenu');

            if (!(typeof cmdName === "undefined")) {
                // this a control level security
                $('#hidClickedControlName').val(cmdName);
                title = cmdName;
            } else {
                // this is page level security
                title = $currentPage;
            };

            // close if already open
            menu.dialog('close')

            // set location of dialog to be next to mouse click
            menu.dialog({
                title: title,
                position: {
                    my: "left top",
                    at: "right bottom",
                    of: event,
                    offset: "30 0"
                }
            }).dialog('open');

            ShowCursor_Wait();
            $("#divDlgSecurityOverlay").fadeIn('100');
        }
    });
}

function AddDialog_ContextMenu() {
    // ===============================================================
    // Add the Context menu security Dialog to the div tag
    // ===============================================================
    $('#divSecurityContextMenu').dialog({
        autoOpen: false,
        modal: false,
        width: 60,
        resizable: false,

        // define what to do on Opening
        open: function (event, ui) {
            // append to the Form of Master page for postback of 'Secure' buttton to work
            $(this).parent().appendTo($("form:first"));

            //            $('input:checkbox', this).eq(i).attr('checked', false);
            $('#cblistButtonAccess [type=checkbox]').val([]);

            // ===============================================================
            // make an AJAX call to the WCF service to get checkbox
            // ===============================================================
            $.ajax({
                type: "POST",
                url: baseUrl + "/WCF/SecurityService.svc/GetControlSecurityOptions",
                data: BuildDataParameters($(this).dialog('option', 'title')),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: CheckOffControlSecurityOptions,
                error: ServiceFailed
            });
        }
    });
}



//=====================================================
//  Create the Parameter List to be sent to 
//=====================================================
function BuildDataParameters(cmdName) {
    var param = "{\"pageName\": \"" + $currentPage + "\", \"cmdName\": \"" + cmdName + "\"}";
    return param;
}

//=====================================================
//  Sets the "Checked" state of the Security Groups for 
//      selected control
//=====================================================
function CheckOffControlSecurityOptions(strResult) {
    try {
        var temp = new Array();
        temp = strResult.d.split(",");

        // make the substring integers
        for (a in temp) {
            temp[a] = parseInt(temp[a]);
        }

        // check the boxes 
        $('#cblistButtonAccess [type=checkbox]').val(temp);
    }
    catch (err) {
        alert(err);
    }
    finally {
        ShowCursor_Default();
        $("#divDlgSecurityOverlay").fadeOut('100');
    }
}

//=====================================================
//  Retrieve the name of the Current page name from url.
//=====================================================
function GetCurrentPageName() {
    var sPath = window.location.pathname;
    var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
    return sPage;
}

//=====================================================
//  add click event the Secure button on the context security menu
//=====================================================
function AddcmdSecureClickEvent() {
    $('#cmdSecure').click(function () {
        var names = [];
        $('#cblistButtonAccess input:checked').each(function () {
            names.push($(this).val());
        });

        $("#hidCheckedUserTypes").val(names);
    });
}