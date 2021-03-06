<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BillOfMaterialMaintenance.aspx.vb" Inherits="LTMS_Master.BillOfMaterialMaintenance" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();

            AddDialogsForCopy();
        }


        function AddDialogsForCopy() {
            var $cmdCopy = $('#MainContent_cmdCopy');
            var $dlgCopy = $('#divDialogCopy');

            // add dialog open event
            $cmdCopy.click(function () {
                $dlgCopy.dialog('open');
                return false;
            });

            // Add Copy Dialog
            $dlgCopy.modalDialog({
                width: 410,
                validationFunction: function () { return ValidateDialog_Copy(); },
                acceptFunction: function () { return AcceptDialog_Copy(); },
                cancelFunction: function () { return CancelCopy(); }
            });

            // Add Confirm Dialog
            $('#divDialogConfirm').modalDialog({
                control: $cmdCopy,
                width: 410,
                acceptFunction: function () { ShowWaitOverlay(); return true; },
                cancelFunction: function () { return false; }
            });
        }

        function ValidateDialog_Copy() {
            var bResult = false;
            var $ddlProdID = $('#MainContent_ddlProd option:selected').val();

            if (!(typeof $ddlProdID === "undefined")) {
                if ($ddlProdID.length > 0) {
                    $('#MainContent_lblWarning').text("WARNING: PREVIOUSLY SAVED DATA FOR PRODUCT " + $ddlProdID + " WILL BE OVERWRITTEN!");
                    bResult = true;
                }
            }
            return bResult;
        }

        function AcceptDialog_Copy() {

            return $('#divDialogConfirm').dialog("open");  // return value will be supplied by the function that is called by which ever button was pressed.
        }

        function CancelCopy() {
            showMessage('Copy Cancelled');
            return false;
        }


    </script>
    <style type="text/css">
        #divMainContent span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label6" runat="server" CssClass="h2 spanLabel">Products:</asp:Label>
                <asp:ListBox ID="lbProducts" runat="server" AutoPostBack="True" CssClass="selectListBox"></asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdSave" runat="server" ToolTip="Save changes" Text="Save"></asp:Button>
                    <asp:Button ID="cmdCancel" runat="server" ToolTip="Cancel changes" Text="Cancel"></asp:Button>
                    <asp:Button ID="cmdCopy" runat="server" ToolTip="Copy item" Text="Copy"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel" style="width: 400px;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="divBorder">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblPartInfo" runat="server" Width="370px" CssClass="h2 spanLabel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label3" runat="server" Width="150px" CssClass="spanLabel">Seat Component</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlComp" runat="server" AutoPostBack="True" Width="185px" CssClass="selectDropDown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="Label4" runat="server" Width="150px" CssClass="spanLabel">Material Category</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMatType" runat="server" AutoPostBack="True" Width="185px" CssClass="selectDropDown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainRightPanel">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Panel ID="hideMe" runat="server">
                    <div class="divBorder">
                        <asp:DataList ID="dlValues" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="Label2" runat="server" Width="180px" CssClass="h2 spanLabel">Part Description:</asp:Label>
                                <asp:Label ID="Label5" runat="server" Width="180px" CssClass="h2 spanLabel">Part Number:</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="ui-icon ui-icon-triangle-1-e"></span>
                                <asp:Label ID="dlLblDesc" runat="server" Width="160px" CssClass="spanLabel" Text='<%# Container.DataItem("ListValue")%>'></asp:Label>
                                <GGS:WebInputBox ID="dlWibPN" runat="server" Width="170px" OnTextChanged="TextBox_TextChanged" CssClass=" textEntry"></GGS:WebInputBox>
                                <asp:Image ID="dlImgCheck" runat="server" EnableViewState="true" Visible="False" ImageUrl="~/Images/check.gif" Width="20px" Height="18px"></asp:Image>
                                <%# DLControlBind(Container)%>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogCopy" title="Copy Product">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <p class="pCenter">
                    <asp:Label ID="lblPrompt" runat="server" CssClass="spanLabel"> </asp:Label>
                    <asp:DropDownList ID="ddlProd" runat="server" CssClass="selectDropDown">
                    </asp:DropDownList>
                    <span class="spanLabel">?</span>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogConfirm" title="Copy Product">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            <asp:Label ID="lblWarning" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
            <br />
            <asp:Label ID="lblWarning2" runat="server" ForeColor="Red" Font-Bold="True" Text="DO YOU WANT TO CONTINUE?"></asp:Label>
        </p>
    </div>
</asp:Content>
