<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StationConfiguration.aspx.vb" Inherits="LTMS_Master.StationConfiguration" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemConfiguration.js") %>"></script>
    <script type="text/javascript">
        var $cmdCopy;
        var $dlgCopy;
        var $ddlLineNumber_Copy;
        var $ddlStations_Copy;

        var $cmdButton_New;
        var $dlgDiv_New;
        var $txtNewStationName;
        var $ddlLineNumber_New;


        var $hidDDLStations;
        var $hidDDLStations_Name;

        function contentPageLoad(sender, args) {
            AddAJAXSettings();
            AddDirtyClassOnChange();
            CacheControls();

            $('#divMainLeftPanel select').unbind('change');

            if (!args.get_isPartialLoad()) {               //Specific code that is only needed on first page load, and not for each postback.

                $ddlLineNumber_Copy.change(function () {
                    // This function is found in 'SystemConfiguration.js'
                    LoadStations($ddlStations_Copy, $(this).val());
                });

                //add change events to poppulate hidden fields for postback
                $ddlStations_Copy.change(function () {
                    $hidDDLStations.val($(this).val());
                    $hidDDLStations_Name.val($(this).children("option:selected").text());
                });
            }

            AddDialog_Copy();
            CreateDialog_NewStation();
            DisableEnterKeyOnPage();
        }

        function CacheControls() {
            $cmdCopy = $('#MainContent_cmdCopy');
            $dlgCopy = $('#divDialogCopy');

            $ddlLineNumber_Copy = $('#MainContent_ddlLineNumber_Copy');
            $ddlStations_Copy = $('#MainContent_ddlStations_Copy');

            $cmdButton_New = $('#MainContent_cmdNew');
            $dlgDiv_New = $('#divDialogNew');
            $txtNewStationName = $('#MainContent_txtNewStationName');
            $ddlLineNumber_New = $("#MainContent_ddlLineNumber_New");

            $hidDDLStations = $('#MainContent_hidDDLStation');
            $hidDDLStations_Name = $('#MainContent_hidDDLStation_Name');

            allCopyDialogFields = $([]).add($ddlLineNumber_Copy).add($ddlStations_Copy);
        }

        function AddDialog_Copy() {

            $cmdCopy.click(OpenDialog_Copy);

            $dlgCopy.modalDialog({
                control: $cmdCopy,
                width: 400,
                validationFunction: function () { return ValidateDialog_Copy(); }
            });
        }

        function OpenDialog_Copy() {

            $ddlStations_Copy.empty();

            $dlgCopy.dialog('open');

            return false;
        }

        function ValidateDialog_Copy() {
            var bValid = true;
            try {
                allCopyDialogFields.removeClass("ui-state-error");
                //  compare in reverse order, then the error messages will display in standard order

                bValid = checkDropDownListByIndex($ddlStations_Copy, 1, "Please select a Station.") && bValid;
                bValid = checkDropDownListByIndex($ddlLineNumber_Copy, 1, "Please select a Line Number.") && bValid;

            } catch (err) {
                alert(err);
            }
            return bValid;

        }


        function CreateDialog_NewStation() {

            $cmdButton_New.click(function () {
                $dlgDiv_New.dialog('open');
                $ddlLineNumber_New.val($('#MainContent_ddlLineNumber').val());
                return false; 
            });

            $dlgDiv_New.modalDialog({
                control: $cmdButton_New,
                width: 380,
                validationFunction: function () { return ValidateDialog_NewStation(); }
            });
        }
        function ValidateDialog_NewStation() {
            var bValid = true;
            bValid = checkDropDownListByIndex($ddlLineNumber_New, 1, "Please Select a Line", $("#helpLineNumber")) && bValid;
            bValid = checkText($txtNewStationName, "Please Enter a Station Name. This is a required field.", $("#helpStationName")) && bValid;
            return bValid;
        }
    </script>
    <style type="text/css">
        #divMainContent span.ui-icon-triangle-1-e, #divDialogCopy span.ui-icon-triangle-1-e, #divDialogNew span.ui-icon-triangle-1-e
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 276px;
        }
        #divMainLeftPanel select
        {
            width: 220px;
        }
        .selectListBox
        {
            height: 300px;
            width: 220px;
            float: left;
        }
        #cmdPanel
        {
            width: 220px;
        }
        #divVertButton
        {
            width: 46px;
            margin: 87px 0px 0px 4px;
            float: left;
        }
        #divVertButton span
        {
            display: block;
            padding: 8px 0px;
            text-align: center;
        }
        #divVertButton .sortButton
        {
            width: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <input id="hidDDLStation" type="hidden" runat="server" />
        <input id="hidDDLStation_Name" type="hidden" runat="server" />
    </div>
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label9" runat="server" CssClass="h2 spanLabel">Line Number:</asp:Label>
                <asp:DropDownList ID="ddlLineNumber" runat="server" AutoPostBack="True" CssClass="selectDropDown">
                </asp:DropDownList>
                <asp:Label ID="Label3" runat="server" CssClass="h2 spanLabel">Station:</asp:Label>
                <asp:ListBox ID="lbStations" runat="server" AutoPostBack="true" CssClass="selectListBox"></asp:ListBox>
                <div id="divVertButton">
                    <asp:ImageButton ID="cmdMoveUp" runat="server" ImageUrl="~/Images/Arrows/ArrowUp.png" CssClass="sortButton" />
                    <asp:Label ID="Label1" runat="server" CssClass="spanLabel">Move</asp:Label>
                    <asp:ImageButton ID="cmdMoveDown" runat="server" ImageUrl="~/Images/Arrows/ArrowDown.png" CssClass="sortButton" />
                </div>
                <div id="cmdPanel">
                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="New Station" Text="New"></asp:Button>
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="Save Station changes" Text="Save"></asp:Button>
                    <asp:Button ID="cmdCopy" runat="server" CssClass="inputButton" Height="28px" Width="75px" ToolTip="Copy Station" Text="Copy"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:DataList ID="dlStationsConfig" runat="server">
                    <HeaderTemplate>
                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="lblCaption" runat="server" Width="180px" CssClass="spanLabel">Station Description:</asp:Label>
                        <GGS:WebInputBox ID="wibDesc" runat="server" Width="330px" Text="<%#StationDesc(Container)%>" OnTextChanged="wibDesc_TextChanged" CssClass="textEntry"></GGS:WebInputBox>
                        <asp:Image ID="imgCaption" runat="server" EnableViewState="true" Visible="False" ImageUrl="~/Images/check.gif" Width="20px" Height="18px"></asp:Image>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="dlLblDesc" runat="server" Width="180px" CssClass="spanLabel" Text='<%# DataBinder.Eval(Container.DataItem,"Description")%>'></asp:Label>
                        <GGS:WebDropDownList ID="wddlStationIndex" runat="server" CssClass="selectDropDown" Width="337px" OnSelectedIndexChanged="dbStations_IndexChanged">
                        </GGS:WebDropDownList>
                        <GGS:WebInputBox ID="wibStations" runat="server" Width="330px" Text='<%# Container.DataItem("StationParameterValue")%>' OnTextChanged="wibStations_TextChanged" CssClass="textEntry"></GGS:WebInputBox>
                        <asp:Image ID="dlImgCheck" runat="server" EnableViewState="true" Visible="False" ImageUrl="~/Images/check.gif" Width="20px" Height="18px"></asp:Image>
                        <%# ObjectVisibilityInTemplate(Container)%>
                    </ItemTemplate>
                </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogCopy" title="Copy Station">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table2">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" CssClass="spanLabel">Line to copy to:</asp:Label>
                </td>
                <td>
                    <GGS:WebDropDownList ID="ddlLineNumber_Copy" runat="server" Width="220px" CssClass="selectDropDown" ToolTip="Please select the line where the Station Parameters will be copied to.">
                    </GGS:WebDropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" CssClass="spanLabel">Station to copy to:</asp:Label>
                </td>
                <td>
                    <GGS:WebDropDownList ID="ddlStations_Copy" runat="server" Width="220px" CssClass="selectDropDown" ToolTip="Please select the Station where the Station Parameters will be copied to.">
                    </GGS:WebDropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogNew" title="New Station">
        <p class="validationHints h4">All form fields are required.</p>
        <table class="tableCenter" >
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" CssClass="spanLabel" Width="90">Station Name:</asp:Label>
                    <asp:TextBox ID="txtNewStationName" runat="server" Width="154px" CssClass="textEntry NoColorOnChange" MaxLength="50" ></asp:TextBox>
                </td>
                <td>
                    <div id="helpStationName" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="spanLabel" Width="90">Add to Line:</asp:Label>
                    <asp:DropDownList ID="ddlLineNumber_New" runat="server" Width="160px" CssClass="selectDropDown NoColorOnChange" >
                    </asp:DropDownList>
                </td>
                <td>
                    <div id="helpLineNumber" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
