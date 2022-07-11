/********************************************************************************   
*   JavaScript functions for the look and feel of the UI
*********************************************************************************/

function AddUIClasses() {
	//Add the jQuery UI Button skin to submit and input buttons 
	$("input:submit, input:button, .sortButton").button();
}


function AddWaitCursurOnWebMenuClick() {
	$('li.dynamic', '#mnMainMenu').on('click', function () { ShowWaitOverlay(); });
}

function AddDirtyClassOnChange() {
    //set textboxes and selects to change color on the "onChange" event
    //20130625 $(":text, select").change(function () { $(this).addClass('dirty'); });
    //changed to not include controls that have the "NoColorOnChange" class
    $(":text, select").not(".NoColorOnChange").change(function () { $(this).addClass('dirty'); });
}

// first timeout option
function AddInactivityTimeoutToPage(timeout) {

	//script is from Phillip Palmieri 	-  idleTimeout
	//https://github.com/philpalmieri/jquery-idleTimeout

	// timeout is being set in master page.
	//  masterpage reads it from the applicationParameterValue '0105'

	var sPath = window.location.pathname;
	var sSearch = window.location.search;
	var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);

	//	var redirectTo = baseUrl + '/Login.aspx?ReturnURL=' + sPath + sSearch;
	//	var redirectTo = encodeURI(sPath + sSearch);
	var redirectTo = sPath + sSearch;

	if ((sPage.length > 0) && (sPage.notEqualTo('Login.aspx'))) {
		$(document).idleTimeout({
			inactivity: timeout,
			alive_url: baseUrl + '/Home.aspx',
			sessionAlive: false,
			redirect_url: redirectTo,
			logout_url: baseUrl + '/Logout.aspx'
		});
	};

}

//Second Timeout option
function AddInactivityDialog() {
	// eric hynds / jquery-idle-timeout
	//https: //github.com/ehynds/jquery-idle-timeout/tree/master/src
	$("#divDlgInactivityWarning").dialog({
		autoOpen: false,
		modal: true,
		width: 400,
		height: 200,
		closeOnEscape: false,
		draggable: false,
		resizable: false,
		buttons: {
			'Yes, Keep Working': function () {
				// Just close the dialog. We pass a reference to this
				// button during the init of the script, so it'll automatically
				// resume once clicked
				$(this).dialog('close');
			},
			'No, Logoff': function () {
				// fire whatever the configured onTimeout callback is.
				$.idleTimeout.options.onTimeout.call(this);
			}
		}
	});

	// start the plugin
	$.idleTimeout('#divDlgInactivityWarning', 'div.ui-dialog-buttonpane button:first', {
		idleAfter: 30, // user is considered idle after 5 minutes of no movement
		pollingInterval: 60, // a request to keepalive.aspx (below) will be sent to the server every minute
		keepAliveURL: 'keepalive.aspx',
		serverResponseEquals: 'OK', // the response from keepalive.php must equal the text "OK"
		onTimeout: function () {

			// redirect the user when they timeout.
			window.location = "logout.aspx";

		},
		onIdle: function () {

			// show the dialog when the user idles
			$(this).dialog("open");

		},
		onCountdown: function (counter) {

			// update the counter span inside the dialog during each second of the countdown
			$("#dialog-countdown").html(counter);

		},
		onResume: function () {

			// the dialog is closed by a button in the dialog
			// no need to do anything else

		}
	});

// use this div on masterpage
//    <div id="divDlgInactivityWarning" title="Your session is about to expire!">
//	    <p>You will be logged off in <span id="dialog-countdown"></span> seconds.</p>
//	    <p>Do you want to continue your session?</p>
//    </div>
}


function AddAJAXSettings() {
	// Tell the browser not to cache the page;
	$.ajaxSetup({ cache: false });
}

function RemoveTreeViewSkipLinks() {
	// remove any _SkipLink references  (added by TreeView)
	$('a[href$=_SkipLink]').each(function () { $(this).remove(); });
}


//==============================================================================
// this will swallow the enter key unless the enter was pressed inside a textarea
function DisableEnterKeyOnPage() {
	$(document).keypress(function (e) {
		if (e.which == 13 && e.target.nodeName != "TEXTAREA") return false;
	});
}


//==============================================================================
// Functions to control the overlays
function ShowCursor_Wait() {  $('body').css('cursor', 'wait');  }
function ShowCursor_Default() {	$('body').css('cursor', 'default'); }

function OverlayHide()    {  $("#divOverlay").hide();     }
function OverlayFadeIn()  {	$("#divOverlay").fadeIn(100); }
function OverlayFadeOut() {	$("#divOverlay").fadeOut();   }

function ClearWaitOverlay() {
	ShowCursor_Default();
	OverlayFadeOut();
}

function ShowWaitOverlay() {
    ShowCursor_Wait();
	OverlayFadeIn();
}

//==============================================================================
// Function to make the Message Label in the Header Pulsate
function BlinkMessage() {  $('#lblMessage').effect('pulsate',{times: 3});  }

//==============================================================================
// moves the icon down and to the right to line up with the textbox
function PositionCalendarPickerIcon() {	$("img[class='ui-datepicker-trigger']").each(function () { $(this).attr('style', 'position:relative; top:5px; left:3px;').css('cursor', 'pointer'); });  }


//==============================================================================
//  applies addiional classes to the asp:calendar control after the page, and javascript files are loaded.
function AddjQueryUIFormatToCalendar() {

	//title header
	$('.calendarTitleStyle').addClass('ui-state-default calendarHeaderRadius');

	//day header
	$('.calendarDayHeaderStyle').addClass('ui-widget-header calendarHeaderRadius');

	//days
	$('.calendarDayStyle').addClass('ui-widget-content calendarRadius');
	$('a', '.calendarDayStyle').addClass('fontDecorationNone');

	//Today
	$('.calendarTodayDayStyle').addClass('ui-widget-content calendarRadius');
	$('.calendarTodayDayStyle').addClass('calendarBorder');
	$('a', '.calendarTodayDayStyle').addClass('fontDecorationNone');

	//selected day
	//	$('.calendarSelectedDayStyle').addClass('ui-widget-content calendarRadius');
	$('.calendarSelectedDayStyle').addClass('ui-widget-header calendarRadius');
	$('a', '.calendarSelectedDayStyle').addClass('fontDecorationNone');

	// weekend 
	$('.calendarWeekendDayStyle').addClass('ui-state-hover calendarRadius');
	$('a', '.calendarWeekendDayStyle').addClass('fontDecorationNone ').addClass('fontWeightNormal');     // add fontWeightNormal? 


	//other month days
	$('.calendarOtherMonthDayStyle').addClass('ui-state-default calendarRadius');
	$('a','.calendarOtherMonthDayStyle').addClass('fontWeightNormal');

}

