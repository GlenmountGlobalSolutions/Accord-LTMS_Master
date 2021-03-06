<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PrintNewSSNLabel.aspx.vb" Inherits="LTMS_Master.PrintNewSSNLabel" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var $hidPrintString;
        var $hidSelectedPrinterPrelim;
        var hidSelectedPrinterPrelimIndex;
        var $ddlLabelTypes;
        var $ddlLabelTypesVal;
        var $ddlPrinterDriver;
        var $lblSSN;
        var $cmdPrintOK;

        function contentPageLoad(sender, args) {
           
            $("#MainContent_cmdPrint").on('click', function () {                        
                $('#divDialogPrint').dialog('open');
                $('#MainContent_ddlPrinterDriver').prop('selectedIndex', hidSelectedPrinterPrelimIndex);
                $('#MainContent_ddlLabelTypes').prop('selectedIndex', 0);
                return false;
            });

            $ddlLabelTypes = $('#MainContent_ddlLabelTypes');
            $ddlPrinterDriver = $('#MainContent_ddlPrinterDriver');

            CreateDialog_Print();

            clipdata = $("#MainContent_hidPrintString").val();

            if ((clipdata != null) && (clipdata.length > 0)) {
                printLabel();
                $("#MainContent_hidPrintString").val('');
            }           

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
    <style type="text/css" >
		.selectDropDown, .spanReadOnly
		{
			width: 200px;
		}
		.spanReadOnly
		{
			width: 192px;		
			display:inline-block;
		}
        .textEntry
        {
            text-align:left;
            width:70px;
        }
        .inputButton
        {
            width:80px;
        }
	</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:HiddenField id="hidPrintString" runat="server" Value="" />
                <asp:HiddenField id="hidSelectedPrinterPrelimIndex" runat="server" Value="" />
                <asp:HiddenField id="hidSelectedPrinterPrelim" runat="server" Value="" />    
                <asp:HiddenField id="hidlblSSN" runat="server" Value="" /> 
                <asp:HiddenField id="hidlblNextSSN" runat="server" Value="" />   
                <asp:HiddenField id="hidlblProductID" runat="server" Value="" />
            </div>


            <div style="text-align:left">
                <table id="Table1" style="padding: 1px; margin: 1px;">
                    <tr>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" CssClass="spanLabel h3">Product Type:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddSeatType" TabIndex="1" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td>
                            <asp:Label ID="lblDescNextSSN" runat="server" CssClass="spanLabel h3">Next Serial Number:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblNextSSN" runat="server" CssClass="spanLabel h3 spanReadOnly" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" CssClass="spanLabel h3">Printer:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPrinters" TabIndex="2" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 14px; text-align: left;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" CssClass="spanLabel h3">No. Copies:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tNCopies" TabIndex="3" runat="server" CssClass="textEntry h3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td>
                            <asp:Button ID="cmdPrint" runat="server" Text="Print" Enabled="False" CssClass="inputButton"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

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
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                    </p>    
                </table>       
                
            </ContentTemplate>
        </asp:UpdatePanel>    
    </div>    
</asp:Content>
