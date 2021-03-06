<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LotTraceData.aspx.vb" Inherits="LTMS_Master.LotTraceData" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemOperations.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.alphanum.js") %>"></script>
    <script type="text/javascript">

        var allFields;
        var $cmdButton;
        var $cmdPrint;
        var $cmdPrintOK;
        var $txtComponentScan;
        var clipdata;

        var $hidProductID;
        var $hidStyleGroupID;
        var $hidOperationType;
        var $hidOperationName;
        var $hidRequirementData;
        var $hidOperationResult;
        var $hidStationID;
        var $hidPrintString;
        var $hidSelectedPrinterPrelim;
        var $txtTorque01;
        var $txtTorque02;
        var $txtTorque03;
        var $txtTorque04;
        var $txtTorque05;
        var $txtTorque06;
        var $txtTorque07;
        var $txtTorque08;
        var $txtTorque09;
        var $txtTorque10;

        var $txtAngle01;
        var $txtAngle02;
        var $txtAngle03;
        var $txtAngle04;
        var $txtAngle05;
        var $txtAngle06;
        var $txtAngle07;
        var $txtAngle08;
        var $txtAngle09;
        var $txtAngle10;

        var $dlgModifyAuxTaskHistory;
        var $dlgModifyComponentHistory;
        var $dlgModifyTorqueHistory;

        var $rblTaskValuesAuxT;
        var $rblTaskValuesComp;
        var $rblTaskValuesTorq;

        var $dlgValues;

        var $hidResultValid;
        var $ddlLabelTypes;
        var $ddlLabelTypesVal;
        var $ddlPrinterDriver;
        var $lblSSN;

        function contentPageLoad(sender, args) {                   

            AddAJAXSettings();

            AddDirtyClassOnChange();

            CacheControls();

            AddNumericOnlyToControls();

            $("#MainContent_cmdView").on('click', function () { ShowWaitOverlay(); });

            $("#MainContent_cmdPrint").on('click', function () {
                //if (CanTheLabelsBePrinted()) {          
                $('#divDialogPrint').dialog('open');
                $('#MainContent_ddlPrinterDriver').prop('selectedIndex', hidSelectedPrinterPrelimIndex);
                $('#MainContent_ddlLabelTypes').prop('selectedIndex', 0);
                    return false;
            });
                        
            CreateDialog_ModifyAuxTaskHistory();
            CreateDialog_ModifyComponentHistory();
            CreateDialog_ModifyTorqueHistory();

            CreateDialog_Print();

            clipdata = $("#MainContent_hidPrintString").val();

            if ((clipdata != null) && (clipdata.length > 0)) {
                printLabel();
                $("#MainContent_hidPrintString").val('');
            }
                       
            createAnchorTagClickEvent();
                        
        }

  
        function CacheControls() {
            $cmdButton = $('#MainContent_cmdModifyHistory');
            $cmdPrint = $('#MainContent_cmdPrint');
            $ddlLabelTypes = $('#MainContent_ddlLabelTypes');            
            $ddlPrinterDriver = $('#MainContent_ddlPrinterDriver');

            $hidProductID = $('#MainContent_hidProductID'); 
            $dlgModifyAuxTaskHistory = $('#divDlgModifyAuxTaskHistory');
            $dlgModifyComponentHistory = $('#divDlgModifyComponentHistory');
            $dlgModifyTorqueHistory = $('#divDlgModifyTorqueHistory');
            
            $rblTaskValuesAuxT = $('input[name*="rblTaskValues"]:radio', $dlgModifyAuxTaskHistory);
            $rblTaskValuesComp = $('input[name*="rblTaskValues"]:radio', $dlgModifyComponentHistory);
            $rblTaskValuesTorq = $('input[name*="rblTaskValues"]:radio', $dlgModifyTorqueHistory);

            $hidResultValid = $('#MainContent_hidResultValid');

            $txtComponentScan = $('#MainContent_txtComponentScan');

            $hidProductID = $('#MainContent_hidProductID');
            $hidStyleGroupID = $('#MainContent_hidStyleGroupID');
            $hidOperationType = $('#MainContent_hidOperationType');
            $hidOperationName = $('#MainContent_hidOperationName');
            $hidRequirementData = $('#MainContent_hidRequirementData');
            $hidOperationResult = $('#MainContent_hidOperationResult');
            $hidStationID = $('#MainContent_hidStationID');

            $txtTorque01 = $('#MainContent_txtTorque1');
            $txtTorque02 = $('#MainContent_txtTorque2');
            $txtTorque03 = $('#MainContent_txtTorque3');
            $txtTorque04 = $('#MainContent_txtTorque4');
            $txtTorque05 = $('#MainContent_txtTorque5');
            $txtTorque06 = $('#MainContent_txtTorque6');
            $txtTorque07 = $('#MainContent_txtTorque7');
            $txtTorque08 = $('#MainContent_txtTorque8');
            $txtTorque09 = $('#MainContent_txtTorque9');
            $txtTorque10 = $('#MainContent_txtTorque10');

            $txtAngle01 = $('#MainContent_txtAngle1');
            $txtAngle02 = $('#MainContent_txtAngle2');
            $txtAngle03 = $('#MainContent_txtAngle3');
            $txtAngle04 = $('#MainContent_txtAngle4');
            $txtAngle05 = $('#MainContent_txtAngle5');
            $txtAngle06 = $('#MainContent_txtAngle6');
            $txtAngle07 = $('#MainContent_txtAngle7');
            $txtAngle08 = $('#MainContent_txtAngle8');
            $txtAngle09 = $('#MainContent_txtAngle9');
            $txtAngle10 = $('#MainContent_txtAngle10');

            allFields = $([])
                .add($txtTorque01)
                .add($txtTorque02)
                .add($txtTorque03)
                .add($txtTorque04)
                .add($txtTorque05)
                .add($txtTorque06)
                .add($txtTorque07)
                .add($txtTorque08)
                .add($txtTorque09)
                .add($txtTorque10)

                .add($txtAngle01)
                .add($txtAngle02)
                .add($txtAngle03)
                .add($txtAngle04)
                .add($txtAngle05)
                .add($txtAngle06)
                .add($txtAngle07)
                .add($txtAngle08)
                .add($txtAngle09)
                .add($txtAngle10)

                .add($txtComponentScan);
        }

        function AddNumericOnlyToControls() {
            // make controls with class 'numericOnly' into numeric only
            $('.numericOnly').numeric({ allowMinus: false,
                allowThouSep: false,
                allowDecSep: true
            });

        }

        function printLabel() {
            try {
                //DEBUG   alert(clipdata);

                var dir = "C:\\Program Files\\Map_Batchprint";
                var IPLExe = "PrintExec_IPL.exe";

                var oshell = new ActiveXObject("Wscript.Shell");
                var prog = IPLExe + " \"" + clipdata + "\"";

                oshell.CurrentDirectory = dir;
                oshell.run(prog);
            } catch (e) {
                alert('Could not open the Batch print executable.\n\nPlease check that the program exists at: ' + dir + '\\' + IPLExe);
            }
        }

        function createAnchorTagClickEvent() {
            // for each in the lot trace datagrid item add an event to the anchor tag for the popup
            $('a[id^="lotAnchor_"]').click(function () {
                var thisAnchor = $(this);
                var operationType = thisAnchor.attr('data-OperationType');

                // Pull the data for the line item from the attributes and populate the fields in the popup dialog.                                
                $('#MainContent_lblSerialNum, #MainContent_lblCompSerialNum,#MainContent_lblTorqueSerialNum').text(thisAnchor.attr('data-SSN'));
                $('#MainContent_lblStationID, #MainContent_lblCompStationID,#MainContent_lblTorqueStationID').text(thisAnchor.attr('data-StationID'));

                $hidProductID.val(thisAnchor.attr('data-Product'));
                $hidStyleGroupID.val(thisAnchor.attr('data-StyleGroupID'));
                $hidOperationType.val(thisAnchor.attr('data-OperationType'));
                $hidOperationName.val(thisAnchor.attr('data-OperationName'));
                $hidRequirementData.val(thisAnchor.attr('data-RequirementData'));
                $hidOperationResult.val(thisAnchor.attr('data-OperationResult'));
                $hidStationID.val(thisAnchor.attr('data-StationID'));

                $('#MainContent_lblDescription').text(thisAnchor.text() + ":");

                if (operationType.equalTo("Aux Task")) {
                    $dlgModifyAuxTaskHistory.dialog('open').dialogButtons('Ok', 'disabled');
                } 
                else if (operationType.equalTo("Component Scan")) {
                    $dlgModifyComponentHistory.dialog('open').dialogButtons('Ok', 'disabled');
                } 
                else if (operationType.equalTo("Torque")) {

                    $txtTorque01.val(thisAnchor.attr('data-Torque1'));
                    $txtTorque02.val(thisAnchor.attr('data-Torque2'));
                    $txtTorque03.val(thisAnchor.attr('data-Torque3'));
                    $txtTorque04.val(thisAnchor.attr('data-Torque4'));
                    $txtTorque05.val(thisAnchor.attr('data-Torque5'));
                    $txtTorque06.val(thisAnchor.attr('data-Torque6'));
                    $txtTorque07.val(thisAnchor.attr('data-Torque7'));
                    $txtTorque08.val(thisAnchor.attr('data-Torque8'));
                    $txtTorque09.val(thisAnchor.attr('data-Torque9'));
                    $txtTorque10.val(thisAnchor.attr('data-Torque10'));

                    $txtAngle01.val(thisAnchor.attr('data-Angle1'));
                    $txtAngle02.val(thisAnchor.attr('data-Angle2'));
                    $txtAngle03.val(thisAnchor.attr('data-Angle3'));
                    $txtAngle04.val(thisAnchor.attr('data-Angle4'));
                    $txtAngle05.val(thisAnchor.attr('data-Angle5'));
                    $txtAngle06.val(thisAnchor.attr('data-Angle6'));
                    $txtAngle07.val(thisAnchor.attr('data-Angle7'));
                    $txtAngle08.val(thisAnchor.attr('data-Angle8'));
                    $txtAngle09.val(thisAnchor.attr('data-Angle9'));
                    $txtAngle10.val(thisAnchor.attr('data-Angle10'));

                    $dlgModifyTorqueHistory.dialog('open').dialogButtons('Ok', 'disabled');
                }

                return false;
            })
        }

        function CreateDialog_ModifyAuxTaskHistory() {

            $dlgModifyAuxTaskHistory.modalDialog({
                control: $cmdButton,
                width: 400,
                openFunction: function () { 
                    $rblTaskValuesAuxT.each(function (index, elem){
                        $(elem).prop('checked',false);
                })}
            });


            //add event when rblTaskValues are selected to enable the ok button
            $rblTaskValuesAuxT.unbind().click(function () {
                $hidResultValid.val($(this).val());
                $dlgModifyAuxTaskHistory.dialogButtons('Ok', 'enabled');
            });          
        }


       function CreateDialog_ModifyComponentHistory() {
           $dlgModifyComponentHistory.modalDialog({
               control: $cmdButton,
               width: 400,
               validationFunction: function () { return ValidateComponentScan(); },
               openFunction: function () {
                   $rblTaskValuesComp.each(function (index, elem) {
                       $(elem).prop('checked', false);
                   })
               }
           });
            //add event when rblTaskValues are selected to enable the ok button
            $('input[name*="rblTaskValues"]:radio', $dlgModifyComponentHistory).unbind().click(function () {
                $hidResultValid.val($(this).val());
                $dlgModifyComponentHistory.dialogButtons('Ok', 'enabled');
            });
        }


        function CreateDialog_ModifyTorqueHistory() {
            $dlgModifyTorqueHistory.modalDialog({
                control: $cmdButton,
                clearInputOnOpen: false,
                width: 400,
                validationFunction: function () { return ValidateTorque(); },
                openFunction: function () {
                    $rblTaskValuesTorq.each(function (index, elem) {
                        $(elem).prop('checked', false);
                    })
                }

            });
            //add event when rblTaskValues are selected to enable the ok button
            $('input[name*="rblTaskValues"]:radio', $dlgModifyTorqueHistory).unbind().click(function () {
                $hidResultValid.val($(this).val());
                $dlgModifyTorqueHistory.dialogButtons('Ok', 'enabled');
            });
        }

        function ValidateComponentScan() {
            var bValid = true;
            allFields.removeClass("ui-state-error");

            bValid = checkText($txtComponentScan, "A Valid Barcode is Required", $("#helpBarcode"));

            if (bValid) {
                bValid = false;
                LotTraceData_DialogModifyComponentHistory_VerifyComponentScan($hidStationID.val()
                                                                                , $txtComponentScan.val()
                                                                                , $hidOperationName.val()
                                                                                , $hidStyleGroupID.val()
                                                                                , $hidProductID.val()
                                                                                , $hidRequirementData.val()
                        ).success(function (data) {
                            if (data.d.Status.toUpperCase() == "FALSE") {
                                alert(data.d.ErrorMsg);
                                $dlgModifyComponentHistory.dialogButtons('Ok', 'disabled');
                                bValid = false;
                            } else {  // no data errors, now check if passed validation
                                if (data.d.IsScanValid.toUpperCase() == "TRUE") {
                                    bValid = true;
                                } else {
                                    setValidationHint("Invalid Barcode");
                                    HelpIconHideorShow(false, $("#helpBarcode"));
                                    bValid = false;
                                };
                            };
                        });
            }

            return bValid;
        }

        function ValidateTorque() {
            var bValid = true;
            var msg = "A Numeric Value is Required in All Fields";

            allFields.removeClass("ui-state-error");

            bValid = checkText($txtTorque01, msg, $("#helpTorque1"));
            bValid = checkText($txtTorque02, msg, $("#helpTorque2")) && bValid;
            bValid = checkText($txtTorque03, msg, $("#helpTorque3")) && bValid;
            bValid = checkText($txtTorque04, msg, $("#helpTorque4")) && bValid;
            bValid = checkText($txtTorque05, msg, $("#helpTorque5")) && bValid;
            bValid = checkText($txtTorque06, msg, $("#helpTorque6")) && bValid;
            bValid = checkText($txtTorque07, msg, $("#helpTorque7")) && bValid;
            bValid = checkText($txtTorque08, msg, $("#helpTorque8")) && bValid;
            bValid = checkText($txtTorque09, msg, $("#helpTorque9")) && bValid;
            bValid = checkText($txtTorque10, msg, $("#helpTorque10")) && bValid;

            bValid = checkText($txtAngle01, msg, $("#helpAngle1")) && bValid;
            bValid = checkText($txtAngle02, msg, $("#helpAngle2")) && bValid;
            bValid = checkText($txtAngle03, msg, $("#helpAngle3")) && bValid;
            bValid = checkText($txtAngle04, msg, $("#helpAngle4")) && bValid;
            bValid = checkText($txtAngle05, msg, $("#helpAngle5")) && bValid;
            bValid = checkText($txtAngle06, msg, $("#helpAngle6")) && bValid;
            bValid = checkText($txtAngle07, msg, $("#helpAngle7")) && bValid;
            bValid = checkText($txtAngle08, msg, $("#helpAngle8")) && bValid;
            bValid = checkText($txtAngle09, msg, $("#helpAngle9")) && bValid;
            bValid = checkText($txtAngle10, msg, $("#helpAngle10")) && bValid;

            if ($txtTorque01.val() == 0) {
                bValid = false;
                setValidationHint(" At Least 1 Torque Must Be Entered.");
                HelpIconHideorShow(false, $("#helpTorque1"));
            }

            return bValid;
        }

        function ValidateTorque1Value() {
            var bValid = true;
            var msg = "A Non-Zero Numeric Value is Required in the Torque1 Field";

            allFields.removeClass("ui-state-error");

            if ($txtTorque01.val() <= 0) {
                bValid = false;
                setValidationHint(msg);
                }
            else {
                bValid = true;
                }

            return bValid;
        }
       
        function AlertOnActiveXError() {
            var msg = "You must enable ActiveX:\n";
            msg += "   * Click on Tools\n";
            msg += "   * Select Internet Options...\n";
            msg += "   * Select Security tab.\n";
            msg += "   * Select Local Intranet zone.\n";
            msg += "   * Then, click on Custom Level...\n";
            msg += "   * Find 'Initialize and script ActiveX Controls not marked as safe for scripting' and set to enable";

            alert(msg);
        }


        function AddOption(Text, Value, Selected) {
            if (Selected) {
                $("#MainContent_ddlPrinterDriver").append('<option value="' + Value + '" selected="selected">' + Text + '</option>');
            } else {
                $("#MainContent_ddlPrinterDriver").append('<option value="' + Value + '" >' + Text + '</option>');
            }
        }

        function savePrinterSelection(myDropDown) {
            /********************************************************************************
            Name:  John Rose
            Date:  10/10/2007
            Description:  This runs when the user selects an option from the dropdown list of printers
            This function saves what the user selected to a hidden text field.  I do this to save
            the state of the selected option in the printer drop down list between postbacks.
            *********************************************************************************
            */
            var printerDropDownValue = myDropDown.options[myDropDown.selectedIndex];
            document.getElementById("MainContent_hidSelectedPrinterPrelim").value = printerDropDownValue.value;
        }

        function selectOption(myIndex, printerName) {
            /********************************************************************************
            Name:  John Rose
            Date:  10/10/2007
            Description:  Selects a list option in the drop down list ddlPrinterDriver
            *********************************************************************************/
            //get a reference to the drop down list
            var printerDropDown = document.getElementById("MainContent_ddlPrinterDriver");

            if (printerDropDown != null) {
                //select the option from the drop down list indicated by the parameter
                printerDropDown.selectedIndex = parseInt(myIndex);
                document.getElementById("MainContent_hidSelectedPrinterPrelim").value = printerName;
                hidSelectedPrinterPrelimIndex = printerDropDown.selectedIndex
            }
        }

        function setSelectedPrinter(myPrinter) {
            document.getElementById("MainContent_hidSelectedPrinterPrelim").value = myPrinter;
        }

        function printLabel() {
            try {
                //DEBUG   alert(clipdata);

                var dir = "C:\\Program Files\\Map_Batchprint";
                var IPLExe = "PrintExec_IPL.exe";

                var oshell = new ActiveXObject("Wscript.Shell");
                var prog = IPLExe + " \"" + clipdata + "\"";

                oshell.CurrentDirectory = dir;
                oshell.run(prog);
            } catch (e) {
                alert('Could not open the Batch print executable.\n\nPlease check that the program exists at: ' + dir + '\\' + IPLExe);
            }
        }

        function CreateDialog_Print() {            
            $('#divDialogPrint').modalDialog({
                control: $('#MainContent_cmdPrint'),
                width: 400,
                validationFunction: function () { return ValidateDialog_PrintOK(); }
            });
        }
        
        function ValidateDialog_PrintOK() {
            var bValid = true;            
            bValid = checkDropDownListByIndex($("#MainContent_ddlLabelTypes"), 1, "Please Select a Label Type") && bValid;
            return bValid;
        }
      
    </script>
    
    <script type="text/vbscript">
				Sub PopulatePrinters(selectedPrinter)
                    On Error Resume Next

					'*********************************************************************************************
					'Name:  John Rose
					'Date:  10/9/2007
					'Description:  Populates a drop down list with printers set up on the client's machine
					'*********************************************************************************************
					Set WshNetwork = CreateObject("Wscript.Network")
	
                    If Err.Number <> 0 Then
                        Call AlertOnActiveXError()
                        Err.Clear
                    End If

    				Set Printers = WshNetwork.EnumPrinterConnections
					dim i
					dim selectedIndexVal
					dim count
					dim defaultIndexVal
					dim defaultPrinter
					count = 0
					
					'add default option to dropdown list
					Call AddOption("Select a Printer", "0", false)

					'get the default printer name
					defaultPrinter = GetDefaultPrinter
						
					'loop through user's printers and add a list option for each printer to the drop down list                
					For i = 0 To Printers.Count - 1 Step 2
						count = count + 1
					 
						if selectedPrinter = Printers.Item(i+1) then
						   'gets the index value of selected drop down list
							selectedIndexVal = count
						end if
						
						if defaultPrinter = Printers.Item(i + 1) then
							defaultIndexVal = count
							'Msgbox("defaultIndexVal: " & defaultIndexVal)
						end if
						
						'calls js function that creates the list items
						Call AddOption(Printers.Item(i+1), Printers.Item(i+1), false)
					Next
					
					'call js function to select a drop down item
					if selectedIndexVal = "" then
						'if there is no selected item, let's select the default printer
						Call selectOption(defaultIndexVal, defaultPrinter)
					else
						'if there is a selected item, this script is running due to a postback,
						'so let's re-select what the user had selected before the postback
						Call selectOption(selectedIndexVal, selectedPrinter)
					end if
																
				End Sub
				
				'*********************************************************************************************
				'Name:  John Rose
				'Date:  10/16/2007
				'Description:  Selects the user's default printer.
				'*********************************************************************************************				
				Function GetDefaultPrinter
					Set oShell = CreateObject("Wscript.Shell")
					sRegVal = "HKCU\Software\Microsoft\Windows NT\CurrentVersion\Windows\Device"
					sDefault = ""
					On Error Resume Next
						sDefault = oShell.RegRead(sRegVal)
						sDefault = Left(sDefault ,InStr(sDefault, ",") - 1)
						On Error Goto 0
						GetDefaultPrinter = sDefault
				end Function

    </script>

    <style type="text/css">
        .updatePanelDiv
        {
            margin-left: 12px;
            margin-top: 12px;
        }
            
        span.fontRed
        {
            color: #FF0000 !important;
        }
    
        span.fontGreen
        {
            color: #00CA00 !important;
        }
        .rblTaskValues
        {
            margin-left: auto;
            margin-right: auto;
            margin-top:10px;
            margin-bottom:10px;
        } 

        .rblTaskValues td
        {
            text-align: center;
            font-weight:bold;
            width: 60px;
        } 
        .DropDownList
        {
            height: 300px;
            width: 220px;
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelSSN" runat="server" >
        <ContentTemplate>
        <div>
            <asp:Button ID="cmdModifyHistory" runat="server" Text="Modify History" Style="visibility: hidden; display: none;"></asp:Button>
            <asp:HiddenField id="hidStyleGroupID" runat="server" Value="0" />
            <asp:HiddenField id="hidOperationType" runat="server" Value="" />
            <asp:HiddenField id="hidProductID" runat="server" Value="" />      
            <asp:HiddenField id="hidOperationName" runat="server" Value="" />
            <asp:HiddenField id="hidRequirementData" runat="server" Value="" />
            <asp:HiddenField id="hidOperationResult" runat="server" Value="" />
            <asp:HiddenField id="hidStationID" runat="server" Value="" />
            <asp:HiddenField id="hidResultValid" runat="server" Value="" /> 
            <asp:HiddenField id="hidSelectedPrinterPrelim" runat="server" Value="" />    
            <asp:HiddenField id="hidlblSSN" runat="server" Value="" />   
            <asp:HiddenField id="hidlblProductID" runat="server" Value="" />
            <asp:HiddenField id="hidlblSeatType" runat="server" Value="" />
            <asp:HiddenField id="hidlblSeatDesc" runat="server" Value="" />
            <asp:HiddenField id="hidlblSeatStyle" runat="server" Value="" />
            <asp:HiddenField id="hidlblColorDesc" runat="server" Value="" />
            <asp:HiddenField id="hidPrintString" runat="server" Value="" />
            <asp:HiddenField id="hidSelectedPrinterPrelimIndex" runat="server" Value="" />
        </div>

        <asp:Label ID="Label19" CssClass="spanLabel h3" runat="server" style="margin-top:8px;">Scan or Enter Part:</asp:Label>   
        <asp:TextBox ID="txtSSN" runat="server" TabIndex="0" CssClass="textEntry NoColorOnChange" style="margin-top:8px;" ></asp:TextBox>     
        <asp:Button ID="cmdView" runat="server" Text="View" CssClass="inputButton" style="margin-top:-2px; margin-right:8px;"></asp:Button>
        <asp:Label ID="lblddlSSN" CssClass="spanLabel h3" runat="server" Visible="False" style="margin-top:8px;">Select SSN:</asp:Label>
        <asp:DropDownList ID="ddlSSN" runat="server" Visible="False" AutoPostBack="True" CssClass="selectDropDown"></asp:DropDownList>
        <asp:Button ID="cmdPrint" runat="server" AutoPostBack="False" Text="Print Label" CssClass="inputButton" style="margin-top:-2px; margin-right:8px;" Width="100"></asp:Button>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>      
        </ContentTemplate>
    </asp:UpdatePanel>      
    <asp:UpdatePanel ID="UpdatePanel_details" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlShipStatus" runat="server" Visible="false" CssClass="updatePanelDiv">
                <asp:Label ID="Label10" runat="server" CssClass="textEntry h2" style="margin-right:10px;">Ship Status:</asp:Label>&nbsp;
                <asp:Label ID="lblShipStatus" runat="server" CssClass="textEntry h2"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="pnlSSNDetails" runat="server" Visible="False" CssClass="updatePanelDiv">
                <table id="Table1">
                    <tr>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 90px">
                            <asp:Label ID="Label2" runat="server" BorderStyle="None" CssClass="spanLabel h4">SSN:</asp:Label>
                        </td>
                        <td style="width: 260px">
                            <asp:Label ID="lblSSN" runat="server" CssClass="spanLabel h4" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 90px">
                            <asp:Label ID="Label5" runat="server" CssClass="spanLabel h4">Seat Style:</asp:Label>
                        </td>
                        <td style="width: 260px">
                            <asp:Label ID="lblSeatStyle" runat="server" CssClass="spanLabel h4" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" CssClass="spanLabel h4">Product ID:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblProductID" runat="server" CssClass="spanLabel h4" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" CssClass="spanLabel h4">Color Desc:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblColorDesc" runat="server" CssClass="spanLabel h4" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 14px; text-align: left;">
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <td>
                            <asp:Label ID="Label39" runat="server" CssClass="spanLabel h4">Seat Desc:</asp:Label>
                        <td>
                            <asp:Label ID="lblSeatDesc" runat="server" CssClass="spanLabel h4" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" CssClass="spanLabel h4">Seat Type:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSeatType" runat="server" CssClass="spanLabel h4" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 14px; text-align: left;">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="4">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlSSNDataGrid" runat="server" Visible="False">
                <div style="margin-top: 12px; margin-left: 12px; margin-bottom: 10px;" class="datagrid">
                    <asp:DataGrid ID="dgHistory" Width="100%" AutoGenerateColumns="False" runat="server" HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle" AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle" EditItemStyle-CssClass="DataGrid_EditItemStyle" PagerStyle-CssClass="DataGrid_PagerStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="SSN" HeaderText="SSN">
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="StyleGroupID" HeaderText="Style Group ID">
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Product" HeaderText="Product ID">
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:BoundColumn>    
                            <asp:BoundColumn DataField="OperationType" HeaderText="Operation Type">
                                <ItemStyle Wrap="False" Width="150px"></ItemStyle>
                            </asp:BoundColumn>                       
                            <asp:BoundColumn DataField="OperationName" HeaderText="Operation">
                                <ItemStyle Wrap="False" Width="150px"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="RequirementData" HeaderText="Requirement Data">
                                <ItemStyle></ItemStyle>
                            </asp:BoundColumn>
                             <asp:BoundColumn DataField="OperationStatus" HeaderText="Status">
                                <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="OperationResult" HeaderText="Operation Results">
                                <ItemStyle Wrap="False" Width="150px"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="StationID" HeaderText="Station ID">
                                <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                            </asp:BoundColumn>     
                            <asp:BoundColumn DataField="EndDT" HeaderText="Performed DT">
                                <ItemStyle Wrap="False" Width="150px"></ItemStyle>
                            </asp:BoundColumn>                               
                        </Columns>
                    </asp:DataGrid>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDlgModifyAuxTaskHistory" title="Modify Lot Trace Aux Task Data" class="displayNone">
        <table id="Table2" class="tableCenter">
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label15" runat="server" CssClass="spanLabel h3">Serial Number:</asp:Label>
                </td>
                <td style="vertical-align: baseline; padding-left: 10px">
                    <asp:Label ID="lblSerialNum" runat="server" CssClass="spanLabel h3 bold">?????</asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label16" runat="server" CssClass="spanLabel h3">Station ID:</asp:Label>
                </td>
                <td style="vertical-align: baseline; padding-left: 10px">
                    <asp:Label ID="lblStationID" runat="server" CssClass="spanLabel h3 bold">?????</asp:Label>
                </td>
            </tr>
            <tr>
            <td colspan="2" style="height: 20px">
            </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; text-align: center;" colspan="2">
                    <asp:Label ID="Label7" runat="server" CssClass="spanLabel h3">Are you sure you want to create a new record and set the status to</asp:Label>
                    <asp:RadioButtonList ID="rblTaskValuesAuxT" runat="server" RepeatDirection=Horizontal CssClass="rblTaskValues" ></asp:RadioButtonList>
                    <asp:Label ID="Label13" runat="server" CssClass="spanLabel h3">for this task?</asp:Label>
                </td>                
            </tr>           
        </table>               
    </div>
    <div id="divDlgModifyComponentHistory" title="Modify Lot Trace Component Data" class="displayNone">
        <p class="validationHints h4">All form fields are required.</p>
        <table id="Table4" class="tableCenter" style="width: 375px">
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label4" runat="server" CssClass="spanLabel h3">Serial Number:</asp:Label>
                </td>
                <td style="vertical-align: baseline; padding-left: 10px">
                    <asp:Label ID="lblCompSerialNum" runat="server" CssClass="spanLabel h3">?????</asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label18" runat="server" CssClass="spanLabel h3">Station ID:</asp:Label>
                </td>
                <td style="vertical-align: baseline; padding-left: 10px">
                    <asp:Label ID="lblCompStationID" runat="server" CssClass="spanLabel h3">?????</asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label14" runat="server" CssClass="spanLabel h3">Bar Code:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px; width: 130px;">                    
                    <asp:textbox ID="txtComponentScan" runat="server" Width="130px" CssClass="textEntry" MaxLength="82"></asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpBarcode" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>          
            <tr>
                <td colspan="2" style="height: 20px">
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; text-align: center;" colspan="2">
                    <asp:Label ID="Label20" runat="server" CssClass="spanLabel h3">Are you sure you want to create a new record and set the status to</asp:Label>
                    <asp:RadioButtonList ID="rblTaskValuesComp" runat="server" RepeatDirection=Horizontal CssClass="rblTaskValues" ></asp:RadioButtonList>
                    <asp:Label ID="Label40" runat="server" CssClass="spanLabel h3">for this Component?</asp:Label>
                </td>                
            </tr> 
            <tr>
            <td colspan="2" style="height: 20px">
            </td>
            </tr>
        </table>               
    </div>
    <div id="divDlgModifyTorqueHistory" title="Modify Lot Trace Torque Data" class="displayNone">
        <p class="validationHints h4">
            All form fields are required.</p>       
        <table id="Table3" class="tableCenter" style="width: 375">
            <tr>                
                <td style="text-align: right;" colspan="3">
                    <asp:Label ID="Label1" runat="server" CssClass="spanLabel h3">Serial Number:</asp:Label>
                </td>
                <td style="vertical-align: baseline; padding-left: 10px" colspan="4">
                    <asp:Label ID="lblTorqueSerialNum" runat="server" CssClass="spanLabel h3">?????</asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;" colspan="3">
                    <asp:Label ID="Label12" runat="server" CssClass="spanLabel h3">Station ID:</asp:Label>
                </td>
                <td style="vertical-align: baseline; padding-left: 10px" colspan="4">
                    <asp:Label ID="lblTorqueStationID" runat="server" CssClass="spanLabel h3">?????</asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="height: 20px">
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 80px;">
                    <asp:Label ID="Label21" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;1:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px; width: 60px;">                    
                    <asp:textbox ID="txtTorque1" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpTorque1" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right; width: 80px;">
                    <asp:Label ID="Label17" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;1:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px; width: 60px;">                    
                    <asp:textbox ID="txtAngle1" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpAngle1" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>  
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label11" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;2:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque2" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>
                </td>
                <td style="width:10px">
                    <div id="helpTorque2" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label22" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;2:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle2" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpAngle2" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr> 
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label23" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;3:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque3" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpTorque3" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label24" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;3:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle3" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpAngle3" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label25" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;4:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque4" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpTorque4" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label26" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;4:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle4" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpAngle4" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label27" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;5:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque5" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpTorque5" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label28" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;5:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle5" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpAngle5" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr> 
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label29" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;6:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque6" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpTorque6" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label30" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;6:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle6" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpAngle6" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>  
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label31" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;7:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque7" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpTorque7" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label32" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;7:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle7" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpAngle7" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr> 
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label33" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;8:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque8" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpTorque8" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label34" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;8:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle8" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpAngle8" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label35" runat="server" CssClass="spanLabel h3">Torque&nbsp;&nbsp;&nbsp;9:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque9" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpTorque9" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label36" runat="server" CssClass="spanLabel h3">Angle&nbsp;&nbsp;&nbsp;9:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle9" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpAngle9" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label37" runat="server" CssClass="spanLabel h3">Torque 10:</asp:Label>
                </td>                
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtTorque10" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                    
                </td>
                <td style="width:10px">
                    <div id="helpTorque10" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label38" runat="server" CssClass="spanLabel h3">Angle 10:</asp:Label>
                </td>
                <td style="vertical-align: middle; padding-left: 10px">                    
                    <asp:textbox ID="txtAngle10" runat="server" Width="50px" CssClass="textEntry numericOnly">0</asp:textbox>                   
                </td>
                <td style="width:10px">
                    <div id="helpAngle10" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>                               
            <tr>
                <td colspan="6" style="height: 20px">
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; text-align: center;" colspan="6">
                    <asp:Label ID="Label41" runat="server" CssClass="spanLabel h3">Are you sure you want to create a new record and set the status to</asp:Label>
                    <asp:RadioButtonList ID="rblTaskValuesTorq" runat="server" RepeatDirection=Horizontal CssClass="rblTaskValues" ></asp:RadioButtonList>
                    <asp:Label ID="Label42" runat="server" CssClass="spanLabel h3">for this Torque?</asp:Label>
                </td>                
            </tr>           
        </table>               
    </div>    
   
    <div id="divDialogPrint" title="Print Confirmation"> 
        <table id="Table28"> 
            <tr>                                     
                <td>
                    <br>
                <span class="spanLabel h3"> Select a Printer: </span>
                <select id="ddlPrinterDriver" onchange="savePrinterSelection(this)" name="ddlPrinterDriver" runat="server" class="selectDropDown h3 ddlTopSetup" ></select>
                </td>                                        
            </tr>
        </table>
        <table id="Table18"> 
            <tr>             
                <td>                                        
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>                            
                            <span class="spanLabel h3">Select a Label Type: </span>
                            <asp:DropDownList ID="ddlLabelTypes" runat="server" AutoPostBack="True" CssClass="selectDropDown h3 ddlTopSetup" >
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel_Printing" runat="server">
            <ContentTemplate>   
                <table id="Table11">           
                    <tr>
                        <br>
                        <td style="width: 14px; text-align: left;">
                               <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td style="width: 90px">
                               <asp:Label ID="Label8" runat="server" BorderStyle="None" CssClass="spanLabel h3">Print SSN:</asp:Label>
                        </td>
                        <td style="width: 260px">
                               <asp:Label ID="lblPrintSSN" runat="server" CssClass="spanLabel h3" EnableViewState="False"></asp:Label>
                        </td>                       
                    </tr>
                    <p class="validationHints" style="color:#FF0000; margin-left:12px;">
                    <%--<asp:Label ID="lblMessage" runat="server" CssClass="spanLabel h3">No message</asp:Label>--%>
                    </p>    
                </table>       
                
            </ContentTemplate>
        </asp:UpdatePanel>    
    </div>    
</asp:Content>
