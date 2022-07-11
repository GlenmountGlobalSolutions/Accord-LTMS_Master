<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ReworkMaterialDisposition.aspx.vb" Inherits="LTMS_Master.ReworkMaterialDisposition" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var confirmationPostBack = ""

        function contentPageLoad(sender, args) {
            //------------------------------------------------------------
            // Teardown Dialog			
            //------------------------------------------------------------
            $('#divConfirmation').dialog({
                autoOpen: false,
                modal: true,
                resizable: false,

                buttons: {
                    "Yes": function () {
                        $(this).dialog("close");
                        __doPostBack($(confirmationPostBack).prop('name'), '');
                    },

                    "Cancel": function () {
                        $(this).dialog("close");
                    }
                },

                open: function () {
                    // add dialog to the MasterPage Form
                    $(this).parent().appendTo($("form:first"));

                    //  set focus to the Cancel button,  try blur(remove focus from Delete button.  IE9 doesn't look right with this)
                    $(this).closest('.ui-dialog').find('.ui-dialog-buttonpane button:eq(1)').focus();
                    $(this).closest('.ui-dialog').find('.ui-dialog-buttonpane button:eq(0)').blur();
                }

            });

            // ========== add events ====================================
            $('#MainContent_cmdRewCancel').click(function () {
                confirmationPostBack = '#MainContent_cmdRewCancel';
                $("#MainContent_lblMessage").text("Are you sure you want to Cancel?");
                $('#divConfirmation').dialog('open');
                return false;
            });

            $('#MainContent_cmdRewSave').click(function () {
                confirmationPostBack = '#MainContent_cmdRewSave';
                $("#MainContent_lblMessage").text("Are you sure you want to Save?");
                $('#divConfirmation').dialog('open');
                return false;
            });

            $('#MainContent_cmdMatCancel').click(function () {
                confirmationPostBack = '#MainContent_cmdMatCancel';
                $("#MainContent_lblMessage").text("Are you sure you want to Cancel?");
                $('#divConfirmation').dialog('open');
                return false;
            });

            $('#MainContent_cmdMatSave').click(function () {
                confirmationPostBack = '#MainContent_cmdMatSave';
                $("#MainContent_lblMessage").text("Are you sure you want to Save?");
                $('#divConfirmation').dialog('open');
                return false;
            });

            $("#MainContent_tbRewQuantity").blur(rewValidate);
            $("#MainContent_tbMatQtyReturned").blur(matTotal);
            $("#MainContent_tbMatQtyScrapped").blur(matTotal);
        }

        function matTotal() {
            try {
                var total = 0;

                var tb1 = document.getElementById('MainContent_tbMatQtyReturned');
                if (!tb1) {
                    return false;
                }

                var tb2 = document.getElementById('MainContent_tbMatQtyScrapped');
                if (!tb2) {
                    return false;
                }

                var bl1 = ValidNumber(tb1.value);
                var bl2 = ValidNumber(tb2.value);

                if (bl1 == false) {
                    tb1.value = 0;
                }
                if (bl2 == false) {
                    tb2.value = 0;
                }

                total = parseInt(tb1.value) + parseInt(tb2.value);

                var tb3 = document.getElementById('MainContent_tbMatQtyTotal');
                if (!tb3) {
                    return false;
                }
                tb1.value = parseInt(tb1.value);
                tb2.value = parseInt(tb2.value);
                tb3.value = total;

                return true;
            }
            catch (error) {
                //alert(error.message);
                return false;
            }

        }

        function rewValidate() {
            try {
                var total = 1;

                var tb1 = document.getElementById('MainContent_tbRewQuantity');
                if (!tb1) {
                    return false;
                }

                var bl1 = ValidNumber(tb1.value);

                if (bl1 == false) {
                    tb1.value = 0;
                    return false;
                }

                tb1.value = parseInt(tb1.value);

                return true;
            }
            catch (error) {
                //alert(error.message);
                return false;
            }
        }

        function ValidNumber(thestring) {
            if (thestring.length <= 0) {
                return false;
            }

            for (i = 0; i < thestring.length; i++) {
                ch = thestring.substring(i, i + 1);
                if (ch < "0" || ch > "9") {
                    //alert("The numbers may contain digits 0 thru 9 only!");
                    return false;
                }
            }
            return true;
        }
    </script>
    <style type="text/css">
        span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        tr
        {
            height: 24px;
        }
        div.divBorder
        {
        	background-color: #EEEEEE;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="Table7" style="left: 16px;" >
                <tr>
                    <td colspan="3">
                        <table id="Table1" class="tableCenter">
                            <tr>
                                <td>
                                    <asp:Label ID="LABEL7" runat="server" CssClass="spanLabel">Date:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbDay" runat="server" ReadOnly="true" Width="80px" CssClass="textEntry"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="LABEL12" runat="server" CssClass="spanLabel">Shift:</asp:Label>
                                    <asp:RadioButtonList ID="rblShift" TabIndex="5" runat="server" Enabled="False" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="selectCheckBox">
                                        <asp:ListItem Value="1" Selected="True">1st</asp:ListItem>
                                        <asp:ListItem Value="2">2nd</asp:ListItem>
                                        <asp:ListItem Value="3">3rd</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <table id="Table2" style="border-collapse: collapse; width: 360px;" class="tableCenter">
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Width="140px" CssClass="spanLabel h3">Rework Area</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlInitRework" TabIndex="6" runat="server" Width="320px" CssClass="selectDropDown h3">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" CssClass="spanLabel h3">Lot Number</asp:Label>
                                </td>
                                <td>
                                    <GGS:WebDropDownList ID="ddlInitLot" runat="server" Width="100%" AutoPostBack="True" CssClass="selectDropDown h3">
                                    </GGS:WebDropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" CssClass="spanLabel h3">Serial Number</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbInitSerial" TabIndex="8" runat="server" Width="310px" CssClass="textEntry h3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" CssClass="spanLabel h3">Seat Component</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlInitComp" TabIndex="9" runat="server" Width="100%" AutoPostBack="True" CssClass="selectDropDown h3">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <table id="Table3" style="width: 260px; border-collapse: collapse; margin-top: 16px;">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label1" runat="server" CssClass="spanLabel h2">Rework:</asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div class="divBorder" style="height: 208px; padding-right: 8px;">
                            <table id="Table9" style="border-collapse: collapse; margin-top: 8px;">
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label2" runat="server" Width="140px" CssClass="spanLabel h3">Rework Category</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRewCategory" TabIndex="10" runat="server" Width="100%" AutoPostBack="True" CssClass="selectDropDown h3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label3" runat="server" CssClass="spanLabel h3">Rework Reason</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRewReason" TabIndex="11" runat="server" Width="100%" CssClass="selectDropDown h3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label4" runat="server" CssClass="spanLabel h3">Charge To</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRewResponsibility" TabIndex="12" runat="server" Width="100%" CssClass="selectDropDown h3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label5" runat="server" CssClass="spanLabel h3">Rework Action</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRewAction" TabIndex="13" runat="server" Width="100%" AutoPostBack="True" CssClass="selectDropDown h3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label6" runat="server" CssClass="spanLabel h3">Quantity Reworked</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbRewQuantity" TabIndex="14" runat="server" Width="240px" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="vertical-align: top;">
                        <table id="Table4" style="width: 300px; border-collapse: collapse; margin-top: 16px;">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label18" runat="server" CssClass="spanLabel h2">Material Disposition:</asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div class="divBorder" style="height: 208px; padding-right: 8px;">
                            <table id="Table8" style="border-collapse: collapse; margin-top: 8px;">
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label17" runat="server" CssClass="spanLabel h3">Material Category</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMatCategory" TabIndex="17" runat="server" Width="100%" AutoPostBack="True" CssClass="selectDropDown h3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label16" runat="server" CssClass="spanLabel h3">Part</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMatPart" TabIndex="18" runat="server" Width="100%" AutoPostBack="True" CssClass="selectDropDown h3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label15" runat="server" CssClass="spanLabel h3">Part No.</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbMatPartNo" TabIndex="19" runat="server" Enabled="False" Width="244px" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label14" runat="server" CssClass="spanLabel h3">Disposition Reason</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMatReason" TabIndex="20" runat="server" Width="100%" CssClass="selectDropDown h3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="LABEL19" runat="server" CssClass="spanLabel h3">Charge To</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMatResponsibility" TabIndex="21" runat="server" Width="100%" AutoPostBack="True" CssClass="selectDropDown h3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="Label13" runat="server" Width="170px" CssClass="spanLabel h3">Qty. Returned to Vendor</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbMatQtyReturned" TabIndex="22" runat="server" Width="244px" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="LABEL20" runat="server" CssClass="spanLabel h3">Qty. Scrapped</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbMatQtyScrapped" TabIndex="23" runat="server" Width="244px" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                        <asp:Label ID="LABEL21" runat="server" CssClass="spanLabel h3">Qty. of Parts Total</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbMatQtyTotal" TabIndex="24" runat="server" Width="244px" ReadOnly="True" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table5" class="tableCenter">
                            <tr>
                                <td>
                                    <asp:Button ID="cmdRewSave" TabIndex="16" runat="server" Width="84px" Text="Save" Height="26px" CssClass="inputButton"></asp:Button>
                                    <asp:Button ID="cmdRewCancel" TabIndex="15" runat="server" Width="84px" Text="Cancel" Height="26px" CssClass="inputButton"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: center;">
                        <table id="Table6" class="tableCenter">
                            <tr>
                                <td>
                                    <asp:Button ID="cmdMatSave" TabIndex="26" runat="server" Width="84px" Text="Save" Height="26px" CssClass="inputButton"></asp:Button>
                                    <asp:Button ID="cmdMatCancel" TabIndex="25" runat="server" Width="84px" Text="Cancel" Height="26px" CssClass="inputButton"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divConfirmation" title="R M D Confirmation">
        <p class="pCenter">
            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
            <asp:Label ID="lblMessage" runat="server" Text="N/A" CssClass="spanLabel"></asp:Label>
        </p>
    </div>
</asp:Content>
