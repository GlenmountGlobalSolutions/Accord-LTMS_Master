<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TeardownAndRelease.aspx.vb" Inherits="LTMS_Master.TeardownAndRelease" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var $tbSSN;
        var $tbComponentSN;

        function contentPageLoad(sender, args) {

            CacheControls();

            CreateDialog_TearDown();
            CreateDialog_ConfirmRelease();

            AddKeyUpEvent_tbSSN();
            AddKeyUpEvent_tbComponentSN();

            document.getElementById('MainContent_cmdTeardown').disabled = true;
            document.getElementById('MainContent_cmdRelease').disabled = true;
        }

        function CacheControls() {
            $tbSSN = $("#MainContent_tbSSN");
            $tbComponentSN = $("#MainContent_tbComponentSN");
        }


        function CreateDialog_TearDown() {
            $('#MainContent_cmdTeardown').click(function () { $('#divConfirmationTeardown').dialog('open'); return false; });

            //------------------------------------------------------------
            // Teardown Dialog			
            //------------------------------------------------------------
            $('#divConfirmationTeardown').dialog({
                appendTo: "#MasterForm",
                autoOpen: false,
                modal: true,
                resizable: false,

                buttons: {
                    "Yes": function () {
                        $(this).dialog("close");
                        ShowWaitOverlay();

                        $('#MainContent_HiddenFlag').val('TRUE');
                        __doPostBack($('#MainContent_cmdTeardown').prop('name'), '');
                    },

                    "Cancel": function () {
                        $(this).dialog("close");
                    }
                },

                open: function () {

                    $("#MainContent_lblTeardown", this).text("Are you sure you want to teardown SSN: " + $tbSSN.val());

                }

            });
        }

        function CreateDialog_ConfirmRelease() {
            $('#MainContent_cmdRelease').click(function () { $('#divConfirmationRelease').dialog('open'); return false; });

            //------------------------------------------------------------
            // Release Dialog			
            //------------------------------------------------------------
            $('#divConfirmationRelease').dialog({
                appendTo: "#MasterForm",
                autoOpen: false,
                modal: true,
                resizable: false,

                buttons: {
                    "Yes": function () {
                        $('#MainContent_HiddenFlag').val('TRUE');
                        ShowWaitOverlay();
                        $(this).dialog("close");
                        __doPostBack($('#MainContent_cmdRelease').prop('name'), '');
                    },

                    "Cancel": function () {
                        $(this).dialog("close");
                    }
                },

                open: function () {

                    $("#MainContent_lblRelease", this).text("Are you sure you want to release component: " + $tbComponentSN.val());

                }

            });
        }

        function AddKeyUpEvent_tbSSN() {
            $tbSSN.keyup(function () {
                if ($tbSSN.val().trim().length > 0) {
                    document.getElementById('MainContent_cmdTeardown').disabled = false;
                } else {
                    document.getElementById('MainContent_cmdTeardown').disabled = true;
                }
            });

            $tbSSN.val('');
        }

        function AddKeyUpEvent_tbComponentSN() {
            $tbComponentSN.keyup(function () {
                if ($tbComponentSN.val().trim().length > 0) {
                    document.getElementById('MainContent_cmdRelease').disabled = false;
                } else {
                    document.getElementById('MainContent_cmdRelease').disabled = true;
                }
            });

            $tbComponentSN.val('');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 1024px;" class="tableCenter">
                <tr>
                    <td>
                        <hr style="height: 1px; width: 500px; text-align: right; margin-right: 0">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="spanLabel h2">Teardown Seat</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table1" style="left: 24px; width: 416px; height: 48px; width: 416px">
                            <tr>
                                <td style="width: 127px; height: 40px; text-align: right;" colspan="1">
                                    <asp:Label ID="Label2" runat="server" CssClass="spanLabel h4">Enter SSN:</asp:Label>
                                </td>
                                <td style="height: 40px; text-align: center;">
                                    <asp:TextBox ID="tbSSN" runat="server" Width="259px" CssClass="textEntry"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 127px">
                                    &nbsp;
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="cmdTeardown" runat="server" Text="Teardown Seat" CssClass="inputButton" Width="120px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr style="height: 1px; width: 500px; text-align: right; margin-right: 0">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" CssClass="spanLabel h2">Component Release</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table2" style="left: 24px; height: 86px; width: 416px">
                            <tr>
                                <td style="width: 133px; text-align: right;">
                                    <p>
                                        <asp:Label ID="Label3" runat="server" CssClass="spanLabel h4">Component SN:</asp:Label></p>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="tbComponentSN" runat="server" Width="259" CssClass="textEntry"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 133px" colspan="1">
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="cmdRelease" runat="server" Text="Release Component" CssClass="inputButton" Width="150px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr style="height: 1px; width: 500px; text-align: right; margin-right: 0">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input id="HiddenFlag" type="hidden" runat="server">
    <div id="divConfirmationTeardown" title="Confirmation">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <p class="pCenter">
                    <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
                    <asp:Label ID="lblTeardown" runat="server" Text="Label" CssClass="spanLabel"></asp:Label>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divConfirmationRelease" title="Confirmation">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <p class="pCenter">
                    <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
                    <asp:Label ID="lblRelease" runat="server" Text="Label" CssClass="spanLabel"></asp:Label>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
