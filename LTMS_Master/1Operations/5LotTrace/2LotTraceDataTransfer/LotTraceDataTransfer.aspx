<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="LotTraceDataTransfer.aspx.vb" Inherits="LTMS_Master.LotTraceDataTransfer" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var $ssnFrom;
        var $ssnTo;
        var $cmdTransfer;

        function contentPageLoad(sender, args) {
            CacheControls();

            AddKeyDownEvent();

            AddChangeEvents();

            $(".cmdShowWait, #MainContent_cmdTransfer").on('click', function () { ShowWaitOverlay(); });
        }

        function CacheControls() {
            $ssnFrom = $("#MainContent_txtSSNFrom")
            $ssnTo = $("#MainContent_txtSSNTo");
            $cmdTransfer = $("#MainContent_cmdTransfer");
            
        }

        function AddKeyDownEvent() {
            $ssnFrom.keydown(function (e) {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    //13 = keycode for enter
                    e.preventDefault();
                    $cmdTransfer.click();
                }
            });

            $ssnTo.keydown(function (e) {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    //13 = keycode for enter
                    e.preventDefault();
                    $cmdTransfer.click();
                }
            });
        }

        function AddChangeEvents() {
            $ssnFrom.change(function () { ShowWaitOverlay(); });
            $ssnTo.change(SSNToChanged());
        }
        function SSNToChanged() {
            if ($ssnFrom.val().length == 0) {
                showMessage();
                $ssnFrom.focus();
            } else {
                ShowWaitOverlay();
            }
        }

    
    </script>
    <style type="text/css">
        div.datagrid
        {
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:HiddenField id="hidStyleGroupIDFrom" runat="server" Value="0" />
                <asp:HiddenField id="hidStyleGroupIDTo" runat="server" Value="0" />
                <table id="Table1" style="width: 758px; padding: 0; border-collapse: collapse;">
                    <tr>
                        <td style="width: 338px; text-align: right; vertical-align: middle; padding-top: 12px; padding-right: 4px;">
                            <asp:Label ID="lblPeaseEnterSSN" runat="server" CssClass="spanLabel h3">Enter SSN:</asp:Label>
                        </td>
                        <td style="width: 210px; vertical-align: top; text-align: center; padding-bottom: 4px;" colspan="1" rowspan="1">
                            <asp:Label ID="Label1" runat="server" CssClass="spanLabel h3"> From:</asp:Label>
                            <br />
                            <asp:TextBox ID="txtSSNFrom" runat="server" AutoPostBack="True" TabIndex="1" CssClass="textEntry h3"></asp:TextBox>
                        </td>
                        <td style="vertical-align: top; text-align: center;">
                            <asp:Label ID="Label2" runat="server" CssClass="h3"> To:</asp:Label>
                            <br />
                            <asp:TextBox ID="txtSSNTo" runat="server" AutoPostBack="True" TabIndex="1" CssClass="textEntry h3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 338px; vertical-align: top;">                            
                        </td>
                        <td style="width: 600px; vertical-align: top; text-align: center;">
                            <asp:Panel ID="pnlProdOperHistFrom" CssClass="datagrid" runat="server" 
                                Visible="False">
                                <asp:DataGrid ID="dgProdOperHistFrom" runat="server" AutoGenerateColumns="False" ShowHeader="True" GridLines="Horizontal" Width="100%" HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle" AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle h3" EditItemStyle-CssClass="DataGrid_EditItemStyle" PagerStyle-CssClass="DataGrid_PagerStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle h3">
                                     <Columns>
                                        <asp:BoundColumn DataField="StyleGroupID" HeaderText="StyleGroup">
                                            <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                        </asp:BoundColumn>
                                         <asp:BoundColumn DataField="OperationType" HeaderText="Operation Type">
                                            <ItemStyle Wrap="False" Width="200px"></ItemStyle>
                                        </asp:BoundColumn>                       
                                        <asp:BoundColumn DataField="OperationName" HeaderText="Operation">
                                            <ItemStyle Wrap="False" Width="150px"></ItemStyle>
                                        </asp:BoundColumn>                                       
                                         <asp:BoundColumn DataField="OperationStatus" HeaderText="Status">
                                            <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                        </asp:BoundColumn>                                                                     
                                    </Columns>
                                </asp:DataGrid>
                            </asp:Panel>
                            <asp:Panel ID="pErrFrom" runat="server" Height="46px" Width="164px" Visible="false">
                                <div style="width: 174px; height: 51px">
                                    <asp:Label ID="lblMsgFrom" runat="server" Width="128px" ForeColor="Red">Error: Invalid SSN.</asp:Label>
                                </div>
                            </asp:Panel>
                        </td>
                        <td style="width: 250px; vertical-align: top; text-align: center;">
                            <asp:Panel ID="pnlProdOperHistTo" CssClass="datagrid" runat="server" 
                                Visible="False">
                                <asp:DataGrid ID="dgProdOperHistTo" runat="server" AutoGenerateColumns="False" ShowHeader="True" GridLines="Horizontal" Width="100%" HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle" AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle h3" EditItemStyle-CssClass="DataGrid_EditItemStyle" PagerStyle-CssClass="DataGrid_PagerStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle h3">
                                    <Columns>  
                                        <asp:BoundColumn DataField="StyleGroupID" HeaderText="StyleGroup">
                                            <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                        </asp:BoundColumn>                                                                
                                        <asp:BoundColumn DataField="OperationStatus" HeaderText="Status">
                                            <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                        </asp:BoundColumn>                                                                     
                                    </Columns>
                                </asp:DataGrid>
                            </asp:Panel>
                            <asp:Panel ID="pErrTo" runat="server" Width="164px" Height="40px" Visible="false">
                                <div>
                                    <asp:Label ID="lblMsgTo" Style="left: 15px; top: 12px" runat="server" Width="128px" ForeColor="Red">Error: Invalid SSN.</asp:Label>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="left: 13px; width: 758px; text-align: center; margin-top: 20px;">
                <asp:Button ID="cmdTransfer" runat="server" Height="29px" Enabled="False" Text="Transfer Data" Width="128px"></asp:Button>
            </div>
            <asp:HiddenField ID="hidFrom" runat="server"	Value="0" />
            <asp:HiddenField ID="hidTo" runat="server"	Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
