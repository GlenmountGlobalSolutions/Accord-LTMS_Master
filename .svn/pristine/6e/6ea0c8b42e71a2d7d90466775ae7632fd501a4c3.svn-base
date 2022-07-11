<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ReprintSSNLabel.aspx.vb" Inherits="LTMS_Master.ReprintSSNLabel" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" >

        var $hidPrintString;


        function contentPageLoad(sender, args) {
            $("input:submit").on('click', function () { ShowWaitOverlay(); });

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

    </script>
        <style type="text/css" >
		.selectDropDown, .spanReadOnly
		{
			width: 200px;
		}
        .textEntry
        {
            text-align:left;
            width:192px;
        }
        .copies
        {
            width:72px;
        }
        .inputButton
        {
            width:80px;
        }
	</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate >
<div style="text-align:left">
    <asp:HiddenField id="hidPrintString" runat="server" Value="" />
    <table id="Table1">
        <tr>
            <td style="width: 14px; text-align:left;">
                <span class="ui-icon ui-icon-triangle-1-e"></span>
            </td>
            <td>
                <asp:Label ID="Label8" runat="server" CssClass="spanLabel h3">Seat Serial Number:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tSeatSerialNum" TabIndex="1" runat="server" AutoPostBack="True" CssClass="textEntry h3"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblInvalisSSN" runat="server" ForeColor="Red" Visible="False">Error: Invalid Seat Serial Number</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="ui-icon ui-icon-triangle-1-e"></span>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" CssClass="spanLabel h3">Seat Code:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tSeatCode" TabIndex="2" runat="server" CssClass="textEntry h3"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <span class="ui-icon ui-icon-triangle-1-e"></span>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" CssClass="spanLabel h3">Seat Part Number:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tSeatPartNum" TabIndex="3" runat="server" CssClass="textEntry h3"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <span class="ui-icon ui-icon-triangle-1-e"></span>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" CssClass="spanLabel h3">Comments:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tComments" TabIndex="4" runat="server" CssClass="textEntry h3"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <span class="ui-icon ui-icon-triangle-1-e"></span>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="spanLabel h3"> Printer:</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddPrinters" TabIndex="5" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                </asp:DropDownList>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <span class="ui-icon ui-icon-triangle-1-e"></span>
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" CssClass="spanLabel h3">No. Copies:</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tNCopies" TabIndex="6" runat="server" CssClass="textEntry h3 copies"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
            <td>
                <asp:Button ID="cmdPrint" runat="server" Width="80px" Text="Print" Height="24px" CssClass="inputButton"></asp:Button>
            </td>
            <td>
            </td>
        </tr>
    </table>
</div>
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
