<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ShippingLabel.aspx.vb" Inherits="LTMS_Master.ShippingLabel" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField id="hidPrintString" runat="server" Value="" />
            <asp:HiddenField id="hidDupSSN" runat="server" Value="" />
            <asp:HiddenField id="hidDupDT" runat="server" Value="" />
            <div style="text-align:left">
                <table id="Table3">
                    <tr>
                        <td>
                            <table id="tblTop" style="width: 100%;">
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="spanLabel h3">Driver SSN:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDriverSeatSerialNum" runat="server" AutoPostBack="True" TabIndex="1" CssClass="textEntry h3"></asp:TextBox>
                                        <asp:Label ID="lblDriverSeatSerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td style="width: 30px;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="spanLabel h3">Label Seat Code:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tLabelSeatCode" runat="server" Enabled="False" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="spanLabel h3">Passenger SSN:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPassSeatSerialNum" runat="server" AutoPostBack="True"  TabIndex="2" CssClass="textEntry h3"></asp:TextBox>
                                        <asp:Label ID="lblPassSeatSerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" CssClass="spanLabel h3">Shipping Code:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tShippingCode4" runat="server" Enabled="False" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="spanLabel h3">Comp3 SSN:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRearBackSerialNum"  runat="server" AutoPostBack="True"  Enabled="false" TabIndex="3" CssClass="textEntry h3"></asp:TextBox>
                                        <asp:Label ID="lblRearBackSerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="spanLabel h3">Model Code:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tModelCode" runat="server" Enabled="False" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                         <span class="ui-icon ui-icon-triangle-1-e"></span> 
                                    </td>
                                    <td>
                                         <asp:Label ID="Label10" runat="server" CssClass="spanLabel h3">Rear Cushion SSN:</asp:Label> 
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtComp4SerialNum" runat="server" AutoPostBack="True" TabIndex="4" CssClass="textEntry h3"></asp:TextBox> 
                                         <asp:Label ID="lblComp4SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" CssClass="spanLabel h3">Color Description:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tColorDesc" runat="server" Enabled="False" CssClass="textEntry h3"></asp:TextBox>
                                    </td>
                                </tr> 
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                         <span class="ui-icon ui-icon-triangle-1-e"></span> 
                                    </td>
                                    <td>
                                         <asp:Label ID="Label11" runat="server" CssClass="spanLabel h3">Rear Back 40% SSN:</asp:Label> 
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtComp5SerialNum" runat="server" AutoPostBack="True"  TabIndex="4" CssClass="textEntry h3"></asp:TextBox> 
                                         <asp:Label ID="lblComp5SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td>
                                        &nbsp;
                                    </td>--%>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" CssClass="spanLabel h3">Rear Back 60% SSN:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComp6SerialNum" runat="server" AutoPostBack="True"  TabIndex="4" CssClass="textEntry h3"></asp:TextBox>
                                        <asp:Label ID="lblComp6SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                </tr>                               
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                         <span class="ui-icon ui-icon-triangle-1-e"></span> 
                                    </td>
                                    <td>
                                         <asp:Label ID="Label12" runat="server" CssClass="spanLabel h3">Rear Back 100% SSN:</asp:Label> 
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtComp7SerialNum" runat="server" AutoPostBack="True"  TabIndex="4" CssClass="textEntry h3"></asp:TextBox> 
                                         <asp:Label ID="lblComp7SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                    </tr>
                                <tr>
                                    <%--<td>
                                        &nbsp;
                                    </td>--%>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label15" runat="server" CssClass="spanLabel h3">Armrest RFID SSN:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComp8SerialNum" runat="server" AutoPostBack="True"  TabIndex="4" CssClass="textEntry h3"></asp:TextBox>
                                        <asp:Label ID="lblComp8SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                </tr>                               
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                         <span class="ui-icon ui-icon-triangle-1-e"></span> 
                                    </td>
                                    <td>
                                         <asp:Label ID="Label14" runat="server" CssClass="spanLabel h3">Comp9 Serial Number:</asp:Label> 
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtComp9SerialNum" runat="server" AutoPostBack="True" Enabled="false" TabIndex="4" CssClass="textEntry h3"></asp:TextBox> 
                                         <asp:Label ID="lblComp9SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                    </tr>
                                <tr>
                                   <%-- <td>
                                        &nbsp;
                                    </td>--%>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" CssClass="spanLabel h3">Comp10 Serial Number:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComp10SerialNum" runat="server" AutoPostBack="True" Enabled="false" TabIndex="4" CssClass="textEntry h3"></asp:TextBox>
                                        <asp:Label ID="lblComp10SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                </tr>                               
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                         <span class="ui-icon ui-icon-triangle-1-e"></span> 
                                    </td>
                                    <td>
                                         <asp:Label ID="Label19" runat="server" CssClass="spanLabel h3">Comp11 Serial Number:</asp:Label> 
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtComp11SerialNum" runat="server" AutoPostBack="True" Enabled="false" TabIndex="4" CssClass="textEntry h3"></asp:TextBox> 
                                         <asp:Label ID="lblComp11SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                    </tr>
                                <tr>
                                    <%--<td>
                                        &nbsp;
                                    </td>--%>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label21" runat="server" CssClass="spanLabel h3">Comp12 Serial Number:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComp12SerialNum" runat="server" AutoPostBack="True" Enabled="false" TabIndex="4" CssClass="textEntry h3"></asp:TextBox>
                                        <asp:Label ID="lblComp12SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                </tr>                               
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                         <span class="ui-icon ui-icon-triangle-1-e"></span> 
                                    </td>
                                    <td>
                                         <asp:Label ID="Label16" runat="server" CssClass="spanLabel h3">Comp13 Serial Number:</asp:Label> 
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtComp13SerialNum" runat="server" AutoPostBack="True" Enabled="false" TabIndex="4" CssClass="textEntry h3"></asp:TextBox> 
                                         <asp:Label ID="lblComp13SerialNumStatus" runat="server" Width="10px" ForeColor="Red"></asp:Label> 
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>                                                                    
                                </tr>                              
                                <tr>
                                    <td colspan="7" style="height: 8px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 14px; text-align: left;">
                                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" CssClass="spanLabel h3">Printer:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddPrinters" runat="server" AutoPostBack="True"  TabIndex="4" CssClass="selectDropDown h3" Width="180px">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="4" style="">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                <td colspan="2">&nbsp;</td>
                                    <td colspan="1" style="height: 14px">
                                        <asp:Button ID="cmdPrint" runat="server" CssClass="inputButton" Enabled="False" Text="Print" TabIndex="5"  Style="width: 80px;"></asp:Button>
                                    </td>
                                <td colspan="4">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="tMessage" runat="server" TextMode="MultiLine" Width="99%" Height="120px" TabIndex="6"  CssClass="textEntry h3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label8" runat="server" CssClass="spanLabel h3" >Shipping Status:</asp:Label>
                            <span>&nbsp;</span>
                            <asp:Label ID="lblShipStatus" runat="server" ForeColor="Red" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="Table4">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPrintDupSSN" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="cmdPrintYes" runat="server" Width="75px" Height="24px" CssClass="inputButton" Text="Yes" Visible="False"></asp:Button>
                                        <asp:Button ID="cmdPrintNo"  runat="server" Width="75px" Height="24px" CssClass="inputButton" Text="No" Visible="False"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
