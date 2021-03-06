<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BatchPrint.aspx.vb" Inherits="LTMS_Master.BatchPrint" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.maskedinput-1.3.1.min.js") %>"></script>
    <script type="text/javascript">

        var CONST_SPAN_START = "<span class=\\\'allOrders\\\'>"
        var CONST_SPAN_END = '</span>'
        var previouslySelectedTab = 0;
        var clipdata;

        function contentPageLoad(sender, args) {

            RemoveTreeViewSkipLinks();

            // after the document is loaded and before the Tabs are created get the value of the previously Selected tab
            previouslySelectedTab = $("#MainContent_hidLastTab").val();
            clipdata = $("#MainContent_hidPrintString").val();

            CreateTabControl();

            AddEventsToControls();

            CreateDatePicker();

            CreateDialog_Print();

            if ((clipdata != null) && (clipdata.length > 0)) {
                    printLabel();
                    //$("#MainContent_hidPrintString").val('');
            }
        }

        function CreateTabControl() {
            // Make the div "tabs" into a jQuery UI tab
            $("#tabs").tabs({

                active: previouslySelectedTab,    //call previouslySelectedTab to Select the previous tab. (needed for postback)

                beforeActivate: function (event, ui) {
                    $("#MainContent_hidLastTab").val(ui.newTab.index()); // when the tabs are selected set the hidden field
                    $(".validationHints").hide();
                }
            });        
        }

        function CreateDatePicker() {
            // add datepicker to the textbox
            $("#MainContent_tbDay").mask('99/99/9999').addDatePicker().datepicker("option", "firstDay", 1);
            // add the event functions
            $("#MainContent_tbDay").datepicker({
                onSelect: function (dateText, inst) {
                    SetDateLabels(dateText, true);
                }
            }).change(function () {           // if the edit box is changed, 
                SetDateLabels(this.value, true);
            });

            PositionCalendarPickerIcon();
        }

        function CreateDialog_Print(){
            //printLabel();
            $('#divDlgPrintConfirm').modalDialog({ control: $('#MainContent_btnPrintJob') });
        }
        
        function AddEventsToControls() {
            // add the events for the previous next buttons
            $("#MainContent_ibPrev").on('click', function () { AddWeek(-1); return false; });
            $("#MainContent_ibNext").on('click', function () { AddWeek(1); return false; });


            // when a postback button is clicked change cursor to wait cursor
            $("#MainContent_cmdRefresh, input:checkbox").click(function () { ShowWaitOverlay(); });
            $("#MainContent_ddlLabelTypes, #MainContent_ddlBroadcastPointID").change(function () { ShowWaitOverlay(); });


            //Add event to previous and next week icons to change cursor over calendar button
            $("input:image").mouseover(function () { $(this).css('cursor', 'pointer'); });

            $("#MainContent_btnPrintJob").on('click', function () {
                if (CanTheLabelsBePrinted()) {
                    $('#divDlgPrintConfirm').dialog('open');
                }
                return false;
            });

            $(".validationHints").hide();
        }

        function CanTheLabelsBePrinted() {
            var bResult = false;
            var confirmMessage = "";
            var message = "Please complete the following to print your labels:  "
            var msgLen = message.length;

            var ddlLabelTypeIndex = document.getElementById("MainContent_ddlLabelTypes").selectedIndex;
            
            var ddlPrintIndex = document.getElementById("MainContent_ddlPrinterDriver").selectedIndex;
            var selectedTab = $("#tabs").tabs('option', 'active');

            var ddlSeatStyleIndex = document.getElementById("MainContent_ddlSeatStyle").selectedIndex;
            var seatStyle = document.getElementById("MainContent_ddlSeatStyle").value;
            var nodeLevel = $("#MainContent_hidNodeLevel").val();
            var nodeLot = $("#MainContent_hidNodeLot").val();
            var qty = "0";

            $(".validationHints").hide();


            if (ddlLabelTypeIndex == 0) {
                message += " Select a Label Type";
            }

            if (ddlPrintIndex == 0) {
                message += (message.length > msgLen) ? "," : "";
                message += " Select a Printer";
            }

            if (selectedTab == 0) {

                if ((typeof nodeLevel == 'undefined') || (nodeLevel < 1)) {

                    message += (message.length > msgLen) ? "," : "";
                    message += " Select a Lot";
                }

                qty = $("#MainContent_hidNodeQty").val();
                confirmMessage = "Are you sure you want to print " + qty + " labels for lot " + nodeLot + "?";

            } else {

                if (ddlSeatStyleIndex == 0) {
                    message += (message.length > msgLen) ? "," : "";
                    message += " Select a Seat Style";
                }

                qty = $("#MainContent_txtSeatStyleQty").val();

                if ($.isNumeric(qty) && (qty <= 0)) {
                    message += (message.length > msgLen) ? "," : "";
                    message += " Enter a valid number value in the Qty text field";
                }

                confirmMessage = "Are you sure you want to print " + qty + " labels for style " + seatStyle + "?";
            }

            bResult = (message.length == msgLen);

            if (bResult) {
                //------ Set the Dialog Message  ----
                $("#MainContent_lblPrintConfirm").text(confirmMessage);

            } else {
                $(".validationHints").show();
                setValidationHint(message);
            }

            return bResult;
        }

        function SetDateLabels(dateText, doPostBack) {
            var d1 = Date.parse(dateText);
            if (d1 != null) {
                ShowWaitOverlay();
                __doPostBack($('#MainContent_cmdRefresh').prop('name'), '');
            }
        }

        function AddWeek(weeks) {
            var dpDate = $("#MainContent_tbDay").datepicker('getDate');

            if (dpDate != null) {
                dpDate.addWeeks(weeks);

                $("#MainContent_tbDay").datepicker('setDate', dpDate);
                SetDateLabels(dpDate.toString('MM/dd/yyyy'), true);
            };
        }

        function GetSelectedNode(evt, treeData) {
            try {

                var src = window.event != window.undefined ? window.event.srcElement : evt.target;
                var nodeClick = src.tagName.toLowerCase() == "a" || src.tagName.toLowerCase() == "span";
                if (nodeClick) {

                    var treeViewData = window[treeData];

                    if (treeViewData.selectedNodeID.value != "") {

                        var selectedNode = document.getElementById(treeViewData.selectedNodeID.value);
                        var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                        var text = selectedNode.innerText;
                        var levelSplit = value.split("\\\\");
                        var nodeLevel = levelSplit.length;


                        $("#MainContent_hidTreeName").val(treeData.substring(12, 19));
                        $("#MainContent_hidNodeID").val(selectedNode.id);
                        $("#MainContent_hidNodeSeqDT").val(levelSplit[0].toString());

                        $("#MainContent_hidNodeLevel").val(nodeLevel);
                        $("#MainContent_hidNodeValue").val(text);

                        $("#MainContent_hidNodeQty").val(text.substring(text.indexOf(":") + 1));


                        $("#MainContent_hidNodeLot").val("");
                        $("#MainContent_hidNodeSubLot").val("");

                        if (nodeLevel >= 2) {
                            $("#MainContent_hidNodeLot").val(levelSplit[1].replace(CONST_SPAN_START, '').replace(CONST_SPAN_END, '').substring(0, text.indexOf(":") - 1));
                        }
                        if (nodeLevel >= 3) {
                            $("#MainContent_hidNodeSubLot").val(levelSplit[2].replace(CONST_SPAN_START, '').replace(CONST_SPAN_END, '').substring(0, text.indexOf(":") - 1));
                        }

                        //DEBUG::   alert("Text: " + text + "\r\n" + "Value: " + value + "\r\n" + "Level: " + level.length );
                        return false;   //comment this if you want postback on node click
                    }
                }

            }

            catch (Error) {
                alert(Error);
            }
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
            }
        }

        function setSelectedPrinter(myPrinter) {
            document.getElementById("MainContent_hidSelectedPrinterPrelim").value = myPrinter;
        }

        function printLabel() {
            try {
                //DEBUG   alert(clipdata);

                var dir = "C:\\Program Files\\Map_Batchprint";
                var IPLExe = "BatchPrint_IPL.exe";

                var oshell = new ActiveXObject("Wscript.Shell");
                var prog = IPLExe + " \"" + clipdata + "\"";

                oshell.CurrentDirectory = dir;
                oshell.run(prog);
            } catch (e) {
                alert('Could not open the Batch print executable.\n\nPlease check that the program exists at: ' + dir + '\\' +IPLExe);
            }
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
		.ddlTopSetup
		{
			width: 350px;
		}
        .cbHold
        {
            margin-left:16px;
        }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align:left">
    <div id="divHidden">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <input id="hidPrintString" type="hidden" runat="server" >
                <input id="hidSelectedPrinterPrelim" type="hidden" runat="server" >
                <input id="hidLastTab" type="hidden" runat="server" >
                <input id="hidTreeName" type="hidden" runat="server">
                <input id="hidNodeID" type="hidden" runat="server">
                <input id="hidNodeSeqDT" type="hidden" runat="server">
                <input id="hidNodeLevel" type="hidden" runat="server">
                <input id="hidNodeValue" type="hidden" runat="server">
                <input id="hidNodeLot" type="hidden" runat="server">
                <input id="hidNodeSubLot" type="hidden" runat="server">
                <input id="hidNodeQty" type="hidden" runat="server">
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divDropDowns">
        <table id="Table1">
            <tr>
                <td>
                    <span class="spanLabel h3">Select a Label Type: </span>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlLabelTypes" runat="server" AutoPostBack="True" CssClass="selectDropDown h3 ddlTopSetup" >
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="padding-left:12px">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnPrintJob" runat="server" Text="Print labels" CssClass="inputButton" Width="100"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="spanLabel h3">Select a Printer: </span>
                </td>
                <td>
                    <select id="ddlPrinterDriver" onchange="savePrinterSelection(this)" name="ddlPrinterDriver" runat="server" class="selectDropDown h3 ddlTopSetup" >
                    </select>
                </td>
                <td>
                    <p class="validationHints" style="color:#FF0000; margin-left:12px">
                        <asp:Label ID="lblMessage" runat="server" CssClass="spanLabel h3">No message</asp:Label>
                    </p>                
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id="tabs" style="width: 600px" class="tabs">
                <ul>
                    <li><a href="#tabs-1">Print from Lot</a></li>
                    <li><a href="#tabs-2">Print from Style</a></li>
                </ul>
                <div id="tabs-1">
                    <table style="border-collapse: collapse; padding: 0; margin-bottom: 10px;">
                        <tr style="vertical-align: bottom;">
                            <td style="width: 178px">
                                <asp:Label ID="LABEL1" runat="server" CssClass="spanLabel h3">Enter Date:</asp:Label>
                            </td>
                            <td style="width: 16px; text-align: right;">
                                <asp:ImageButton ID="ibPrev" runat="server" ImageUrl="~/images/Arrows/bbPrevpage.gif"></asp:ImageButton>
                            </td>
                            <td style="width: 140px; text-align: left; padding-left: 10px;">
                                <asp:TextBox ID="tbDay" runat="server" Width="96px" CssClass="textEntry h3"></asp:TextBox>
                            </td>
                            <td style="width: 16px; text-align: left;">
                                <asp:ImageButton ID="ibNext" runat="server" ImageUrl="~/images/Arrows/bbNextpage.gif"></asp:ImageButton>
                            </td>
                            <td style="text-align: left; padding: 0px 0px 0px 28px; width: 100px;">
                                <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass='inputButton'></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <div id="divSelectBroadcastPointID">
				        <asp:Label ID="lblSelectBroadcastPointID" runat="server" CssClass="spanLabel h3"></asp:Label>
                        <asp:DropDownList ID="ddlBroadcastPointID" runat="server" CssClass="selectDropDown NoColorOnChange" AutoPostBack="true" >
                        </asp:DropDownList>
                        <asp:CheckBox ID="cbHold" runat="server" AutoPostBack="True" Text="Show ON HOLD Lots" Checked="True" CssClass="selectCheckBox h3 cbHold"  />
                    </div>
                    <table id="Table2" style="margin-left:165px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblSingleDayOfWeek" runat="server" CssClass="spanLabel h3"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle">
                                <asp:Panel ID="PanelTreeSingleDay" runat="server" ScrollBars="Auto" Width="200" Height="280" CssClass="divBorder">
                                    <asp:TreeView ID="treeSingleDay" runat="server" BorderWidth="0" NodeIndent="10" ExpandDepth="2">
                                        <NodeStyle CssClass="treeViewNode" />
                                        <SelectedNodeStyle CssClass="treeViewNodeSelected ui-state-default" />
                                    </asp:TreeView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <div id="divButtonsSingleDayOfWeek" class="divExpandCollapse" style="padding-bottom: 10px;">
                                    <input id="cmdExpSingleDay" type="button" value="Expand" class='inputButton' onclick="javascript:TreeviewExpandCollapseAll('MainContent_treeSingleDay', true);" />
                                    <input id="cmdColSingleDay" type="button" value="Collapse" class='inputButton' onclick="javascript:TreeviewExpandCollapseAll('MainContent_treeSingleDay', false);" />
                                </div>
                                <asp:Label ID="LABEL5" runat="server" CssClass="spanLabel h4">Total: </asp:Label>
                                <asp:Label ID="lblTotalSingleDay" runat="server" CssClass="spanLabel h4">0</asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tabs-2">
                    <table id="Table4">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlSeatStyle" runat="server" CssClass="selectDropDown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtSeatStyleQty" runat="server" Width="40px" CssClass="textEntry">30</asp:TextBox>
                                <asp:Label ID="lblQty" runat="server" CssClass="spanLabel h3">Qty</asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDlgPrintConfirm" title="Print Confirmation">
        <p style="text-align: center; vertical-align: middle">
            <asp:Label ID="lblPrintConfirm" runat="server" CssClass="spanLabel h3">No message</asp:Label>
        </p>
    </div>
    </div>
</asp:Content>
