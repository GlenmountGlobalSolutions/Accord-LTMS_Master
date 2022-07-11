<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ArchiveLotTraceDataTransfer.aspx.vb" Inherits="LTMS_Master.ArchiveLotTraceDataTransfer" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            $("input:submit").on('click', function () { ShowWaitOverlay(); });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 12px; margin-left: 12px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="spanLabel h3">Enter SSN:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSSN" Style="width: 140px;" runat="server" CssClass="textEntry h3" Text=""></asp:TextBox>
                        </td>
                        <td >
                            <asp:Button ID="cmdViewData" runat="server" Width="90px" CssClass="inputButton" Text="View Data"></asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="cmdTransfer" runat="server" Visible="False" Width="120px" CssClass="inputButton" Text="Transfer Data"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:DataGrid ID="dgProductOperationsRequired" runat="server" Visible="false"  HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle" AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle" EditItemStyle-CssClass="DataGrid_EditItemStyle" PagerStyle-CssClass="DataGrid_PagerStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle" CssClass="borderRadius">
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlOpResults" runat="server">
                    <asp:DataGrid ID="dgProductOperationResults" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="DataGrid_HeaderStyle" FooterStyle-CssClass="DataGrid_FooterStyle" AlternatingItemStyle-CssClass="DataGrid_AlternatingItemStyle" EditItemStyle-CssClass="DataGrid_EditItemStyle" PagerStyle-CssClass="DataGrid_PagerStyle" SelectedItemStyle-CssClass="DataGrid_SelectedItemStyle" ItemStyle-CssClass="DataGrid_ItemStyle" CssClass="borderRadius">
                        <Columns>
                            <asp:BoundColumn DataField="OperationId" HeaderText="Operation"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Results" HeaderText="Results"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
