<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ShippingTemplateConfiguration.aspx.vb" Inherits="LTMS_Master.ShippingTemplateConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.alphanum.js") %>"></script>
    <script type="text/javascript">
        // New dialog validation
        var $rblTemplateNew;
        var $tbTemplateNew;
        var $ddlTemplatesNew;
        var $tbIndexNew;
        var $tbShipNew;
        var $tbArrivalNew;
        var $tbSeqNew;
        var $ddlDriverNew;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();

            CacheControls();
            CreateDialog_New();
            CreateDialog_Delete();

            // Make the Radio button a UI radio button, 
            $rblTemplateNew.buttonset();

            //make textbox numeric only
            $('[id*="tbIndex"]').numeric({
                allowMinus: false,
                allowThouSep: false 
            });
        }

        function CacheControls() {
            // New dialog validation
            $rblTemplateNew = $("#MainContent_rblTemplateNew");
            $tbTemplateNew = $("#MainContent_tbTemplateNew");
            $ddlTemplatesNew = $("#MainContent_ddlTemplatesNew");
            $tbIndexNew = $("#MainContent_tbIndexNew");
            $tbShipNew = $("#MainContent_tbShipNew");
            $tbArrivalNew = $("#MainContent_tbArrivalNew");
            $tbSeqNew = $("#MainContent_tbSeqNew");
            $ddlDriverNew = $("#MainContent_ddlDriverNew");

        }

        function CreateDialog_Delete() {
            var $cmdButton = $('#MainContent_cmdDelete');
            var $dlgDiv = $('#divDialogDelete');

            // Add Delete User Dialog Click Action
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            $dlgDiv.deleteDialog({
                control: $cmdButton
            });
        }

        function CreateDialog_New() {
            var $cmdButton = $('#MainContent_cmdNew');
            var $dlgDiv = $('#divDialogNew');

            // add event to button
            $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add New Dialog
            $dlgDiv.modalDialog({
                control: $cmdButton,
                width: 500,
                clearInputOnOpen: false,
                validationFunction: function () { return ValidateDialog_New(); }
            });
        }

        function ValidateDialog_New() {

            var bValid = true;

            /*
            *   Regex:              ^((([0]?[1-9]|1[0-2])(:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))$
            *   Description:        Enforces HH:MM ?M format
            *   Derived from:       http://regexlib.com/REDetails.aspx?regexp_id=144
            *
            *   HH validation:      ([0]?[1-9]|1[0-2]) = allows 1-9 or 10, 11, 12.
            *   : validation:       (:)
            *   MM validation:      [0-5][0-9] = allows 00-59.
            *   " ?M" validation:   ( )?(AM|am|aM|Am|PM|pm|pM|Pm) = enforces one space and any possible way to write AM/PM.
            */
            bValid = checkRegexp($tbSeqNew, "^((([0]?[1-9]|1[0-2])(:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))$", "Invalid sequence time. Valid format is HH:MM xM", $("#helpSeqNew")) && bValid;
            bValid = checkRegexp($tbArrivalNew, "^((([0]?[1-9]|1[0-2])(:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))$", "Invalid arrival time. Valid format is HH:MM xM", $("#helpArrivalNew")) && bValid;
            bValid = checkRegexp($tbShipNew, "^((([0]?[1-9]|1[0-2])(:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))$", "Invalid ship time. Valid format is HH:MM xM", $("#helpShipNew")) && bValid;
            bValid = checkNumericRange($tbIndexNew, 1, 99, "Invalid Index. Index must be a number between 0 and 100", $("#helpIndexNew")) && bValid;

            //         Value="0" Existing
            //         Value="1" New
            if ($('input:checked', $rblTemplateNew).val() == 1) {
                bValid = checkText($tbTemplateNew, "Invalid template name. This is a required field.", $("#helpTemplateNew")) && bValid;
            } else {
                bValid = checkDropDownList($ddlTemplatesNew, "Invalid template set. Please select a template.", $("#helpTemplateNew")) && bValid;
            }

            return bValid;
        }

    </script>
    <style type="text/css">
        #divMainContent span.ui-icon, #divDialogNew span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 270px;
        }
        #divMainLeftPanel select
        {
            width: 260px;
        }
        #cmdPanel
        {
            width: 260px;
        }
        #newDialogTable tr
        {
            height:24px;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                

		        <asp:Label ID="lblSelectBroadcastPointID" runat="server" CssClass="spanLabel h2"></asp:Label>
                <asp:DropDownList ID="ddlBroadcastPointID" runat="server" CssClass="selectDropDown NoColorOnChange" AutoPostBack="true" >
                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server" CssClass="h2 spanLabel">Delivery Templates:</asp:Label>
                <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="selectDropDown NoColorOnChange" AutoPostBack="True">
                </asp:DropDownList>
                <asp:Label ID="Label4" runat="server" CssClass="h2 spanLabel">Delivery Times:</asp:Label>
                <asp:ListBox ID="lbTTimes" runat="server" AutoPostBack="True" CssClass="selectListBox"></asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" Text="Save"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" Text="Delete"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td style="width: 200px;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="lbl1" runat="server" CssClass="spanLabel">Delivery Time Index:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbIndex" runat="server" Width="200px" CssClass="textEntry">
                            </GGS:WebInputBox>
                        </td>
                        <td>
                            <asp:Image ID="imgIndexCk" runat="server" Width="14px" Height="16px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image><asp:Image ID="imgIndex" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/question_mark.gif" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="lbl2" runat="server" CssClass="spanLabel">Ship Time:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbShip" runat="server" Width="200px" CssClass="textEntry">
                            </GGS:WebInputBox>
                        </td>
                        <td>
                            <asp:Image ID="imgShipCk" runat="server" Width="14px" Height="16px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image><asp:Image ID="imgShip" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/question_mark.gif" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="lbl3" runat="server" CssClass="spanLabel">Arrival Time:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbArrival" runat="server" Width="200px" CssClass="textEntry">
                            </GGS:WebInputBox>
                        </td>
                        <td>
                            <asp:Image ID="imgArrivalCk" runat="server" Width="14px" Height="16px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image><asp:Image ID="imgArrival" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/question_mark.gif" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="lbl4" runat="server" CssClass="spanLabel">Approximate Sequence Time:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbSeq" runat="server" Width="200px" CssClass="textEntry">
                            </GGS:WebInputBox>
                        </td>
                        <td>
                            <asp:Image ID="imgSeqCk" runat="server" Width="14px" Height="16px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image><asp:Image ID="imgSeq" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/question_mark.gif" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="lbl5" runat="server" CssClass="spanLabel">Driver Name:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebDropDownList ID="ddlDriver" runat="server" Width="207px" CssClass="selectDropDown">
                            </GGS:WebDropDownList>
                        </td>
                        <td>
                            <asp:Image ID="imgDriverCk" runat="server" Width="14px" Height="16px" ImageUrl="~/Images/check.gif" EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divRightPanel">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">

            <ContentTemplate>
                <table>

               
     <table class="tblBP" style="margin-left: 40px">
							    <tr>
								    <td><asp:Panel ID="Panel1" runat="server" CssClass="divBP" Visible="True" ><asp:Image ID="image1" runat="server" CssClass='calendarFilterImgBP1' AlternateText="1"></asp:Image><asp:Label ID="lblBP1" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
								    <td><asp:Panel ID="Panel3" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image3" runat="server" CssClass='calendarFilterImgBP2' AlternateText="2" ></asp:Image><asp:Label ID="lblBP3" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
							    </tr>
							    <tr>
								    <td><asp:Panel ID="Panel2" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image2" runat="server" CssClass='calendarFilterImgBP3' AlternateText="3" ></asp:Image><asp:Label ID="lblBP2" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
								    <td><asp:Panel ID="Panel4" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image4" runat="server" CssClass='calendarFilterImgBP4' AlternateText="4" ></asp:Image><asp:Label ID="lblBP4" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
							    </tr>
						    </table>
                     </table>
                 </ContentTemplate>
            </asp:UpdatePanel>
               
            </div> 

    <div id="divDialogNew" title="New Shipping Entry">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <p class="validationHints">
                </p>
                <table id="newDialogTable">
                    <tr style="height:30px;">
                        <td style="width: 200px;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label2" runat="server" CssClass="spanLabel">Select Template:</asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTemplateNew" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True">
                                <asp:ListItem Value="0" Selected="True">Existing</asp:ListItem>
                                <asp:ListItem Value="1">New</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label3" runat="server" CssClass="spanLabel">Delivery Template:</asp:Label>
                        </td>
                        <td >
                            <GGS:WebDropDownList ID="ddlTemplatesNew" runat="server" Width="207px" CssClass="selectDropDown">
                            </GGS:WebDropDownList>
                            <asp:TextBox ID="tbTemplateNew" runat="server" Width="200px" Visible="False" CssClass="textEntry"></asp:TextBox>
                        </td>
                        <td>
                            <div id="helpTemplateNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label5" runat="server" CssClass="spanLabel">Delivery Time Index:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbIndexNew" runat="server" Width="200px" CssClass="textEntry">
                            </GGS:WebInputBox>
                        </td>
                        <td>
                            <div id="helpIndexNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label6" runat="server" CssClass="spanLabel">Ship Time:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbShipNew" runat="server" Width="200px" CssClass="textEntry">
                            </GGS:WebInputBox>
                        </td>
                        <td>
                            <div id="helpShipNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label7" runat="server" CssClass="spanLabel">Arrival Time:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbArrivalNew" runat="server" Width="200px" CssClass="textEntry">
                            </GGS:WebInputBox>
                        </td>
                        <td>
                            <div id="helpArrivalNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label8" runat="server" CssClass="spanLabel">Approximate Sequence Time:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="tbSeqNew" runat="server" Width="200px" CssClass="textEntry">
                            </GGS:WebInputBox>
                        </td>
                        <td>
                            <div id="helpSeqNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label9" runat="server" CssClass="spanLabel">Driver Name:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebDropDownList ID="ddlDriverNew" runat="server" Width="207px" CssClass="selectDropDown">
                            </GGS:WebDropDownList>
                        </td>
                        <td>
                            <div id="helpDriverNew" class="ui-state-default ui-corner-all ui-helper-hidden">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogDelete" title="Delete Shipping Entry">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to delete the selected entry. Do you wish to continue?
        </p>
    </div>
</asp:Content>
