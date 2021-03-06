/*#########################################################################
These functions are designed to be used with the dialog popups
to validate dataentry on the client side
#########################################################################*/

/* #########################################################################
						Plugins
#########################################################################*/


//---------------------------------------------------------------------------
//  plugin to toggle the disable attribute
//  Usage: $('input:text').toggleDisabled();
//---------------------------------------------------------------------------
(function ($) {
	$.fn.toggleDisabled = function () {
		return this.each(function () {
			this.disabled = !this.disabled;
		});
	};
})(jQuery);

//---------------------------------------------------------------------------
//  plugin to make textBox enabled based on a checkbox
//  Usage: 
//      var $chkBox = $('#myCheckBox');
//      $('#selector').enabledBasedOnCheckbox($chkBox);
//---------------------------------------------------------------------------
(function ($) {
	$.fn.enabledBasedOnCheckbox = function (checkbox) {
		return this.each(function () {
			var $txtBox = this;
			//enable or disable now
			this.disabled = !checkbox.is(":checked");

			//and whenever the chekcbox changes
			checkbox.click(function () {
				$txtBox.disabled = !checkbox.is(":checked");
			});
		});
	};
})(jQuery);

//---------------------------------------------------------------------------
//  plugin to add a DatePicker to textBox
//  Usage: $('#selector').addDatePicker();
//---------------------------------------------------------------------------
(function ($) {
	$.fn.addDatePicker = function () {
		return this.each(function() {
			$this = $(this);
			$this.datepicker({
						showOn: "button",
						buttonImage: baseUrl + "/images/misc/calendar.gif",
						buttonImageOnly: true,
						buttonText: '',
						changeMonth: true,
						changeYear: true,
						showButtonPanel: true
					});
			});
	};
})(jQuery);

//---------------------------------------------------------------------------
// Extend dialog to have the first button as the default when enter key is pressed
//---------------------------------------------------------------------------
// jqueryui defaults
$.extend($.ui.dialog.prototype.options, {
    create: function () {
        var $this = $(this);
        var $okBtn = $this.parent().find('.ui-dialog-buttonpane button:first');

        // focus first button and bind enter to it
        $okBtn.focus();
        $this.keypress(function (e) {
            if (e.keyCode == $.ui.keyCode.ENTER) {
                $okBtn.click();
                return false;
            }
        });
    }
});


//---------------------------------------------------------------------------
// Extend dialog to be able to remove a button
//    $('#MyDialog').dialog('addbutton', 'New Button', newButtonClickFunction);
//    function newButtonClickFunction() { alert('You clicked the new button!'); }
//---------------------------------------------------------------------------
$.extend($.ui.dialog.prototype, {
    'addbutton': function (buttonName, func) {
        var buttons = this.element.dialog('option', 'buttons');
        buttons[buttonName] = func;
        this.element.dialog('option', 'buttons', buttons);
    }
});
////---------------------------------------------------------------------------
// Extend dialog to be able to remove a button
//    $('#MyDialog').dialog('removebutton', 'Old Button');
//---------------------------------------------------------------------------
$.extend($.ui.dialog.prototype, {
    'removebutton': function (buttonName) {
        var buttons = this.element.dialog('option', 'buttons');
        delete buttons[buttonName];
        this.element.dialog('option', 'buttons', buttons);
    }
}); 
//---------------------------------------------------------------------------
//  plugin to add a Delete Dialog to a div
//      options:  you can pass a control into the options for the postback, the options are alos sent to the dialog
//  Usage: 
//      If you wanted to create the dialog with a width of 450 and on click of Delete button do postback to the cmdDelete button, use:
//      var $cmdDelete = $('#cmdDelete');
//      var $dlgDiv =    $('#divDlgDelete');
//
//      $dlgDiv.deleteDialog({
//                control: $cmdDelete,
//                width: 450,);
//
//---------------------------------------------------------------------------
(function ($) {
	$.fn.deleteDialog = function (options) {
		// build main options before element iteration
		var opts = $.extend({}, $.fn.deleteDialog.defaults, options);

		return this.each(function() {
			$this = $(this);
			// build element specific options
			var o = $.meta ? $.extend({}, opts, $this.data()) : opts;

			$this.dialog({
                appendTo: "#MasterForm",    // add dialog to the MasterPage	Form
				autoOpen: false,
				modal: true,
				resizable: false,
				show: { effect: 'fade', duration: 250 },
				hide: { effect: 'fade', duration: 250 },

				buttons: [
                    {
                        id: "btnDialogDelete",
                        text: "Delete",
                        click: function () {
						        $(this).dialog("close");
						        //if the control was sent, then do postback
						        if (!(o.control === null)) {
							        ShowWaitOverlay();
							        __doPostBack(o.control.prop('name'), '');
						        }
					    }
                    }, 
                    {
                        id: "btnDialogCancel",
                        text: "Cancel",
                        click: function () {
						    $(this).dialog("close");
						    showMessage('');
    					}
                    }
                ],

				open: function () {
					//	set	focus to the Cancel	button
					//$(this).closest('.ui-dialog').find('.ui-dialog-buttonpane button:eq(1)').focus();
                    //try blur(remove focus from	Delete button.	IE9	doesn't	look right with	this)
					//$(this).closest('.ui-dialog').find('.ui-dialog-buttonpane button:eq(0)').blur();
				}
			}).dialog( "option" , options );
		});
	};
	//
	// plugin defaults
	//
	$.fn.deleteDialog.defaults = {
		control: null
	};

})(jQuery);

//---------------------------------------------------------------------------
//  plugin to add a simple Modal Dialog to a div with Ok and Cancel buttons
//      ctrl:  this is the Control that will postback will be performed on when Ok is clicked.
//  Usage: 
//      var $cmdNew = $('#cmdNew');
//      $('#selector').modalDialog($cmdNew);
//  Assumes any help icons that their name is prefaces with the word help
//  
//  Notes: you can add additonal options from your code by chaining to the ModalDialog
//  For example to change the width would be 
//      $('#selector').modalDialog($cmdNew).dialog({ width: 860 });
//---------------------------------------------------------------------------
(function ($) {
	$.fn.modalDialog = function (options) {
		// build main options before element iteration
		var opts = $.extend({}, $.fn.modalDialog.defaults, options);

		return this.each(function() {
			var $this = $(this);
			// build element specific options
			var o = $.meta ? $.extend({}, opts, $this.data()) : opts;

			$this.dialog({
                appendTo: "#MasterForm",    // add dialog to the MasterPage Form
				autoOpen: false,
				modal: true,
				resizable: false,
				show: { effect: 'fade', duration: 250 },
				hide: { effect: 'fade', duration: 250 },

				buttons: [
                    {
                        id: $this.id + "_btnDialogOK",
                        text: o.btnOk_text,
                        click: function ()  {
						        var bContinue = false;
						
						        //remove the ui-state-error class from the inputs                    
						        $('input[type="text"]', $this).removeClass("ui-state-error");
						        $('select', $this).removeClass("ui-state-error");

                                // clear validation hint, and clear highlight
					            $( ".validationHints:first", $this ).text(o.validationHint).removeClass("ui-state-highlight");

						        if (!(o.validationFunction === null)) {         // check if a validation function was supplied
							        bContinue = o.validationFunction.call();    // use call() to execute the function
						        }  else  {                                      // since there was no function, allow continue.
							        bContinue = true;
						        }

						        if (bContinue) {                        //continue if validation passed validation
							        $this.dialog("close");
							
							        if (!(o.acceptFunction === null)) {         // check if an Accept function was supplied 
								        bContinue = o.acceptFunction.call();    // use call() to execute the function
							        }  else  {                                  // since there was no function, allow continue.
								        bContinue = true;
							        }

							        if ((bContinue) && (!(o.control === null))) {     //if the control was sent, and validation and accept functions passed then do postback
								        ShowWaitOverlay();
								        __doPostBack(o.control.prop('name'), '');
							        }
						        }
                        }
                    }, 
                    {
                        id: $this.id + "_btnDialogCancel",
                        text: o.btnCancel_text,
                        click: function ()  {
                        		$this.dialog("close");            // close dialog

						        if (!(o.cancelFunction === null)) {         // check if a Cancel function was supplied 
							        o.cancelFunction.call();    // use call() to execute the function
						        }  
					       }
                    }
                ],

				open: function (event, ui) {
					// clear the lblMessage label in header
					showMessage('');

					//remove error and dirty classes
					$('input[type="text"], textarea', $this).removeClass('dirty').removeClass("ui-state-error");
					$('select', $this).removeClass('dirty').removeClass("ui-state-error");

                    //clear textboxes, select
					if (o.clearInputOnOpen) {
						$('input[type="text"], textarea', $this).val('');
						$('select', $this).val('-1');
					}

					// clear the all the 'help' icons
					$("div[id^='help']", $this).addClass("ui-helper-hidden");

					// set the validation hint, and clear highlight
					$( ".validationHints:first", $this ).text(o.validationHint).removeClass("ui-state-highlight");

					if (!(o.openFunction === null)) {         // check if an Open function was supplied 
						o.openFunction.call();    // use call() to execute the function
					}  
				}

			}).dialog( "option" , options );


            if ((o.addButton_Ok === false) || o.addButton_Cancel === false){
                var buttons = $this.dialog('option', 'buttons');
                if (o.addButton_Cancel === false) 
                    buttons.splice(1,1);
                if (o.addButton_Ok === false) 
                    buttons.splice(0,1);

                $this.dialog('option', 'buttons', buttons);
            }
		});
	};
			
	//
	// plugin defaults
	//
	$.fn.modalDialog.defaults = {
        validationHint: '',   // Initial validation hint on open.
		control: null,
		clearInputOnOpen: true,
        addButton_Ok: true,
        addButton_Cancel: true,
        btnOk_text: 'Ok',
        btnCancel_text: 'Cancel',
		validationFunction: null,
		acceptFunction: null,
		cancelFunction: null,
		openFunction: null
	};

})(jQuery);


//=====================================================
//  Call to WCF Service Failed - show failure message
//=====================================================
//Disable button 'Ok' on dialog with class 'dialog':
//$('.dialog').dialogButtons('Ok', 'disabled');
//
//Enable all buttons:
//$('.dialog').dialogButtons('enabled');
//
//Enable 'Close' button and change color:
//$('.dialog').dialogButtons('Close', 'enabled').css('color','red');
//
//Text on all buttons red:
//$('.dialog').dialogButtons().css('color','red');
//
(function ($) {
    $.fn.dialogButtons = function(name, state){
        var buttons = $(this).next('div').find('button');
        if(!name)return buttons;
        return buttons.each(function(){
            var text = $(this).text();
            if(text==name && state=='disabled') {$(this).attr('disabled',true).addClass('ui-state-disabled');return this;}
            if(text==name && state=='enabled') {$(this).attr('disabled',false).removeClass('ui-state-disabled');return this;}
            if(text==name){return this;}
            if(name=='disabled'){$(this).attr('disabled',true).addClass('ui-state-disabled');return buttons;}
            if(name=='enabled'){$(this).attr('disabled',false).removeClass('ui-state-disabled');return buttons;}
        });
    };
})(jQuery);

//=====================================================

/* #########################################################################
						Functions
#########################################################################*/

//=====================================================
//  Call to WCF Service Failed - show failure message
//=====================================================
//function ServiceFailed(result) {
//    ShowCursor_Default();
//    alert('WCF Service Failed:                        \n\n' + result.status + ' ' + result.statusText);
//}
function ServiceFailed( jqXHR, textStatus, errorThrown )
{
    ShowCursor_Default();
    alert('WCF Service Failed:                        \n\n' + jqXHR.status + ' :: ' + jqXHR.statusText  + '\n' + jqXHR.responseText);
}

//=====================================================
//  HTML Encode and decode
//=====================================================
function htmlEncode(value) {
	return $('<div/>').text(value).html();
}

function htmlDecode(value) {
	return $('<div/>').html(value).text();
}


//---------------------------------------------------------------------------
//  Set the Validation Message
//---------------------------------------------------------------------------
function setValidationHint(t) {
	var tips = $( ".validationHints" );
	
	tips.text(t).addClass("ui-state-highlight");

	setTimeout(function () {
		tips.removeClass("ui-state-highlight", 1500);
	}, 500);
}

function sendValidationHintToControl(message, ctrl) {
	if (!(typeof optionalSendHintToCtrl === "undefined")) {
		ctrl.text(message);
	}
}

function clearMessage() {
    $('#lblMessage:first').text('');
}

function showMessage(msg) {
	$('#lblMessage:first').text(msg);
}

//---------------------------------------------------------------------------
//Helper for other check fucntions
//---------------------------------------------------------------------------
function HelpIconHideorShow(bResult, optionalIcon) {

	if (!(typeof optionalIcon === "undefined")) {
		if (bResult)
			optionalIcon.addClass("ui-helper-hidden")
		else
			optionalIcon.removeClass("ui-helper-hidden");
	}
}


//---------------------------------------------------------------------------
// Check the length of Text
//      ctrl:           the control to validate
//      label:          the Text to display for Label for the Validation Hint
//      min:            the shortest the text length can be
//      max:            the longest the text length can be
//      optionalIcon:   an optional argument, this is an icon that will appear if fails validation
//      optionalSendHintToCtrl: label control to send the hint to.
//---------------------------------------------------------------------------
function checkLength(ctrl, label, min, max, optionalIcon, optionalSendHintToCtrl) {
	var bResult = false;

	var txt = ctrl.val() || ctrl.text();
	txt = txt.replace(/\s/g, '');    //remove newlines, spaces, tabs
		
	if (txt.length > max || txt.length < min) {
		ctrl.addClass("ui-state-error");

		if (!(typeof optionalSendHintToCtrl === "undefined")) {
			sendValidationHintToControl("Length of " + label + " must be between " + min + " and " + max + ".", optionalSendHintToCtrl);
		} else {
			setValidationHint("Length of " + label + " must be between " + min + " and " + max + ".");
		}
		bResult = false;
	} else {
		bResult = true;
	}

	HelpIconHideorShow(bResult, optionalIcon);
	
	return bResult;

}

//---------------------------------------------------------------------------
// Check the length of Text; return true if Not blank
//      ctrl:               the control to validate
//      message:            the Text to display for Label for the Validation Hint
//      optionalIcon:       an optional argument, this is an icon that will appear if fails validation
//      optionalSendHintToCtrl: label control to send the hint to.
//---------------------------------------------------------------------------
function checkText(ctrl, message, optionalIcon, optionalSendHintToCtrl) {
    var bResult = false;
    var txt = ctrl.val() || ctrl.text();
    txt = txt.replace(/\s/g,'');    //remove newlines, spaces, tabs
	try{
		if (txt.length == 0 ) {
			ctrl.addClass("ui-state-error");

			if (!(typeof optionalSendHintToCtrl === "undefined")) {
				sendValidationHintToControl(message, optionalSendHintToCtrl);
			} else {
				setValidationHint(message);
			}
			bResult = false;
		} else {
			bResult = true;
		}

		HelpIconHideorShow(bResult, optionalIcon);

	} catch (err) {
		alert(err);
	}

	return bResult;

}

//---------------------------------------------------------------------------
//  Check the Control Value against a Regular Expression
//      ctrl:           the control to validate
//      regexp:         the Regular Expression
//      message:        the message to display as the Validation Hint
//      optionalIcon:   an optional argument, this is an icon that will appear if fails validation
//---------------------------------------------------------------------------
function checkRegexp(ctrl, regexp, message, optionalIcon) {
	var bResult = false;
	var re = new RegExp(regexp);

	if (!re.test(ctrl.val())) {
		ctrl.addClass("ui-state-error");
		setValidationHint(message);
		bResult = false;
	} else {
		bResult = true;
	};

	HelpIconHideorShow(bResult, optionalIcon);

	return bResult;
}

//---------------------------------------------------------------------------
//  Check the Control Value against a Regular Expression For a valid date (mm/dd/yyyy)
//      ctrl:           the control to validate
//      message:        the message to display as the Validation Hint
//      optionalIcon:   an optional argument, this is an icon that will appear if fails validation
//---------------------------------------------------------------------------
function checkDate(ctrl, message, optionalIcon) {
	var bResult = false;
//    var re = new RegExp('^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$');

	var m = ctrl.val().match(/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/);
	if (((m) ? new Date(m[3], m[1] - 1, m[2]) : null) != null) {
		bResult = true;
	}


	if (!bResult) {
		ctrl.addClass("ui-state-error");
		setValidationHint(message);
		bResult = false;
	} else {
		bResult = true;
	};

	HelpIconHideorShow(bResult, optionalIcon);

	return bResult;
}

//---------------------------------------------------------------------------
//  Check the CheckBoxList for at least x checked values
//      ctrl:           the control to validate
//      min:            the least number of checked values
//      message:        the message to display as the Validation Hint
//      optionalIcon:   an optional argument, this is an icon that will appear if fails validation
//---------------------------------------------------------------------------
function checkCheckBoxListCount(ctrl, min, message, optionalIcon) {
	var bResult = false;

	if (ctrl.length < min) {
		ctrl.addClass("ui-state-error");
		setValidationHint(message);
		bResult = false;
	} else {
		bResult = true;
	};

	HelpIconHideorShow(bResult, optionalIcon);

	return bResult;
}

//---------------------------------------------------------------------------
//  Check the DropdownList
//      ctrl:           the control to validate
//      message:        the message to display as the Validation Hint
//      optionalIcon:   an optional argument, this is an icon that will appear if fails validation
//---------------------------------------------------------------------------
function checkDropDownList(ctrl, message, optionalIcon) {
	var bResult = checkDropDownListByIndex(ctrl, 0, message, optionalIcon);
	return bResult;
}

//---------------------------------------------------------------------------
//  Check the DropdownList
//      ctrl:           the control to validate
//      idx:            the selectedIndex must be greater than or equal to this number
//      message:        the message to display as the Validation Hint
//      optionalIcon:   an optional argument, this is an icon that will appear if fails validation
//---------------------------------------------------------------------------
function checkDropDownListByIndex(ctrl, idx, message, optionalIcon) {
	var bResult = false;
	var index = getSelectedIndex(ctrl);

	if (index >= idx) { bResult = true; }

	if (bResult == false) {
		ctrl.addClass("ui-state-error");
		setValidationHint(message);
	}

	HelpIconHideorShow(bResult, optionalIcon);

	return bResult;
}

//---------------------------------------------------------------------------
//  Check the Numeric Range of input
//      ctrl:           the control to validate
//      min:            the number must be must be greater than or equal to this number
//      max:            the number must be must be less than or equal to this number
//      message:        the message to display as the Validation Hint
//      optionalIcon:   an optional argument, this is an icon that will appear if fails validation
//---------------------------------------------------------------------------
function checkNumericRange(ctrl, min, max, message, optionalIcon) {
	var bResult = false;
	var num = ctrl.val();

	if (isNaN(num) || (num < min) || (num > max) || (num == "")) {
		bResult = false;
	} else {
		bResult = true;
	}

	if (bResult == false) {
		ctrl.addClass("ui-state-error");
		setValidationHint(message);
	}

	HelpIconHideorShow(bResult, optionalIcon);

	return bResult;
}


//---------------------------------------------------------------------------
//  Return the value of selectedIndex property
//      ctrl:    the control to get the selected index property
//---------------------------------------------------------------------------
function getSelectedIndex(ctrl) {
	return ctrl.prop("selectedIndex");
}

//---------------------------------------------------------------------------
//  String prototype to compare text without regard to case
//      usage:  if (stringVar.equalTo('comparisonString')) 
//                  ...
//---------------------------------------------------------------------------
String.prototype.equalTo = function (str) {
	return this.toLowerCase() === str.toLowerCase();
}

//---------------------------------------------------------------------------
//  String prototype to compare text without regard to case
//      usage:  if (stringVar.notEqualTo('comparisonString')) 
//                  ...
//---------------------------------------------------------------------------
String.prototype.notEqualTo = function (str) {
	return (!(this.toLowerCase() === str.toLowerCase()));
}

function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}
function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}
// ===============================================================

// ===============================================================
// ASP.Net TreeView Util functions ExpandAll CollapseAll
// ===============================================================
// From http://pushpontech.blogspot.com/2007/06/client-side-expandcollapse-all-nodes.html

function TreeviewExpandCollapseAll(treeViewId, expandAll) //pass true/false for expand/collapse all
{
	var displayState = (expandAll == true ? "none" : "block");
	var treeView = document.getElementById(treeViewId);
	if (treeView) {
		var treeLinks = treeView.getElementsByTagName("a");
		var nodeCount = treeLinks.length;
		var flag = true;

		for (i = 0; i < nodeCount; i++) {
			if (treeLinks[i].firstChild.tagName) {
				if (treeLinks[i].firstChild.tagName.toLowerCase() == "img") {
					var currentToggleLink = treeLinks[i];
					var childContainer = GetParentByTagName("table", currentToggleLink).nextSibling;
					if (childContainer.style.display == displayState) {
						eval(currentToggleLink.href);
					}
				}
			}
		} //for loop ends
	}
}

function TreeViewGetSelectedNodeDepth(selectedNodeID) {
	var selectedNode;
	var value;
	var levelSplit;
	var nodeLevel = -1;

	if (selectedNodeID.value != "") {
		selectedNode = document.getElementById(selectedNodeID.value);
		value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
		levelSplit = value.split("\\\\");
		nodeLevel = levelSplit.length;
	}
	return nodeLevel;
}

function TreeViewClearAllExcept(exceptTree) {
    $('a[href*="tree"]').not('a[href*="' + exceptTree + '"]').removeClass("ui-state-default").parent().removeClass("ui-state-default");

//    $('a[href*="tree"]').removeClass("ui-state-default");

//    var allAnc = document.getElementsByTagName("a");
//    var ancHref = "javascript:__doPostBack('ctl00$MainContent$tree";
//    for (var i = 0; i < allAnc.length; i++) {
//        if (allAnc[i].href.indexOf(ancHref) == 0) {
//            allAnc[i].parentElement.removeAttribute("class");
//        }
//    }
}

//utility function to get the container of an element by tagname
function GetParentByTagName(parentTagName, childElementObj) {
	var parent = childElementObj.parentNode;
	while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
		parent = parent.parentNode;
	}
	return parent;
} 

// ===============================================================
// These functions are other utility functions
// ===============================================================

function PopulateNameValueDDL(strResult, ddl) {
	try {
		var options = [];
		var strSelected = ' selected="selected" ';
		var strEndOption = '">';

		$(ddl).empty();  //TODO:  Is this call needed?

		for (var i = 0; i < strResult.d.length; i++) {

			strEndOption = '" ';
			if (strResult.d[i].Selected == "1") {
				strEndOption += strSelected;
			}
			strEndOption += '>';

			options.push('<option value="',
						strResult.d[i].Value, 
						strEndOption,
						strResult.d[i].Name, '</option>');
		}
		$(ddl).html(options.join(''));
	}
	catch (Error) {
		alert(Error);
	}
}
function padLeft(nr, n, str) {
    return Array(n - String(nr).length + 1).join(str || '0') + nr;
}
function GetCurrentDateTime() {
    var myTime 
    try {
        var currentTime = new Date();
        var hours = currentTime.getHours();
        var minutes = padLeft(currentTime.getMinutes(), 2);
        var seconds = padLeft(currentTime.getSeconds(), 2);
        var milliseconds = padLeft(currentTime.getMilliseconds(), 3);
        myTime = hours + ":" + minutes + ":" + seconds + ":" + milliseconds;
	}
	catch (Error) {
		alert(Error);
	}
    return myTime;
}
function ConsoleWrite(msg) {
    console.log(GetCurrentDateTime() + ' :: ' + msg);
}

function showImageOnSelectedItemChanging(sender, eventArgs) {
    var input = sender.get_inputDomElement();
    input.style.background = "url(" + eventArgs.get_item().get_imageUrl() + ") no-repeat";
    input.style.backgroundSize = "16px 16px";
}

function showSelectedItemImage(sender) {
    var input = sender.get_inputDomElement();
    var idx = sender.get_selectedItem().get_index();
    input.style.background = "url(" + sender.get_items().getItem(idx).get_imageUrl() + ") no-repeat";
    input.style.backgroundSize = "16px 16px";
}
