<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StationToolConfiguration.aspx.vb" Inherits="LTMS_Master.StationToolConfiguration" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemConfiguration.js") %>"></script>
    <script type="text/javascript">
        var $cmdCopy;
        var $dlgCopy;
        var $ddlLineNumber_Copy;
        var $ddlStyles_Copy;
        var $ddlStations_Copy;

        var $hidDDLStyles;
        var $hidDDLStations;
        var $hidDDLStyles_Name;
        var $hidDDLStations_Name;

        function contentPageLoad(sender, args) {
            AddAJAXSettings();
            AddDirtyClassOnChange();
            CacheControls();

            if (!args.get_isPartialLoad()) {
                //Specific code that is only needed on first page load, and not for each postback.

                $ddlLineNumber_Copy.change(function () {
                    // These two functions are found in 'SystemConfiguration.js'
                    LoadStyleGroups($ddlStyles_Copy, $(this).val());
                    LoadStations($ddlStations_Copy, $(this).val());
                });

                //add change events to poppulate hidden fields for postback
                $ddlStyles_Copy.change(function () {
                    $hidDDLStyles.val($(this).val());
                    $hidDDLStyles_Name.val($(this).children("option:selected").text());
                });

                $ddlStations_Copy.change(function () {
                    $hidDDLStations.val($(this).val());
                    $hidDDLStations_Name.val($(this).children("option:selected").text());
                });
            }
            AddDialog_Copy();
        }

        function CacheControls() {
            $cmdCopy = $('#MainContent_cmdCopy');
            $dlgCopy = $('#divDialogCopy');

            $ddlLineNumber_Copy = $('#MainContent_ddlLineNumber_Copy');
            $ddlStyles_Copy = $('#MainContent_ddlStyleGroups_Copy');
            $ddlStations_Copy = $('#MainContent_ddlStations_Copy');

            $hidDDLStyles = $('#MainContent_hidDDLStyleGroup');
            $hidDDLStations = $('#MainContent_hidDDLStation');
            $hidDDLStyles_Name = $('#MainContent_hidDDLStyleGroup_Name');
            $hidDDLStations_Name = $('#MainContent_hidDDLStation_Name');

            allCopyDialogFields = $([]).add($ddlLineNumber_Copy).add($ddlStyles_Copy).add($ddlStations_Copy);
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

            $ddlStyles_Copy.empty();
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
                bValid = checkDropDownListByIndex($ddlStyles_Copy, 1, "Please select a Style Group.") && bValid;
                bValid = checkDropDownListByIndex($ddlLineNumber_Copy, 1, "Please select a Line Number.") && bValid;

            } catch (err) {
                alert(err);
            }
            return bValid;

        }	
    </script>
    <style type="text/css">
        #divMainCenterPanel span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 270px;
            padding-right: 5px;
        }
        #divMainLeftPanel select
        {
            width: 260px;
        }
        #cmdPanel
        {
            width: 260px;
            margin-bottom: 20px;
        }
        
        #cmdPanel2
        {
            width: 260px;
            text-align: center;
            padding-top: 5px;
        }
        #cmdPanel2 input
        {
            width: 150px;
            display: block;
            white-space: normal;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 10px;
        }
        #MainContent_Panel2
        {
            padding-top: 5px;
            width: 260px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <input id="hidDDLStyleGroup" type="hidden" runat="server" />
        <input id="hidDDLStation" type="hidden" runat="server" />
        <input id="hidDDLStyleGroup_Name" type="hidden" runat="server" />
        <input id="hidDDLStation_Name" type="hidden" runat="server" />
    </div>
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label4" runat="server" CssClass="spanLabel h2">Line Number:</asp:Label>
                <asp:DropDownList ID="ddlLineNumber" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                </asp:DropDownList>
                <asp:Label ID="Label2" runat="server" CssClass="spanLabel h2">Style Group:</asp:Label>
                <asp:DropDownList ID="ddlStyleGroups" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server" CssClass="spanLabel h2">Station:</asp:Label>
                <asp:DropDownList ID="ddlStations" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                </asp:DropDownList>
                <div id="cmdPanel">
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save Station Tool"
                        Text="Save"></asp:Button>
                    <asp:Button ID="cmdCopy" runat="server" CssClass="inputButton" ToolTip="Copy Station Tool"
                        Text="Copy"></asp:Button>
                </div>
                <asp:Panel ID="Panel2" runat="server" CssClass="divBorder">
                    <asp:Label ID="Label3" runat="server" CssClass="spanLabel h2">Copy Names to all Style Groups and Stations for Selected Line:</asp:Label>
                    <div id="cmdPanel2">
                        <asp:Button ID="cmdCopyTool1Names" runat="server" CssClass="inputButton" Text="Copy Tool 1 Names"
                            ToolTip="Copy Tool Names from Tool 1 to all Style Groups and Stations for the selected line." />
                        <asp:Button ID="cmdCopyTool2Names" runat="server" CssClass="inputButton" Text="Copy Tool 2 Names"
                            ToolTip="Copy Tool Names from Tool 2 to all Style Groups and Stations for the selected line." />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel" style="width: 800px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:DataList ID="dlStationToolConfig" runat="server" RepeatColumns="2">
                    <ItemTemplate>
                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="dllblDesc" runat="server" CssClass="spanLabel" Width="160px" Text='<%# DataBinder.Eval(Container.DataItem,"Description")%>'></asp:Label>
                        <GGS:WebDropDownList ID="dlwddlToolParam" runat="server" CssClass="selectDropDown"
                            Width="167px" OnSelectedIndexChanged="dlwddlToolParam_IndexChanged">
                        </GGS:WebDropDownList>
                        <GGS:WebInputBox ID="dlwibToolParam" Text='<%# Container.DataItem("ToolParamValue")%>'
                            OnTextChanged="dlwibToolParam_TextChanged" runat="server" Width="160px" CssClass="textEntry"></GGS:WebInputBox>
                        <asp:Image ID="dlImgCheck" runat="server" EnableViewState="true" Visible="False"
                            Width="20px" Height="18px" ImageUrl="~/Images/check.gif"></asp:Image>
                        <%#ObjectVisibilityInTemplate(Container)%>
                    </ItemTemplate>
                </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogCopy" title="Copy Station Tool Configuration">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table2" class="tableCenter">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" CssClass="spanLabel">Line:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlLineNumber_Copy" runat="server" Width="220px" CssClass="selectDropDown"
                        ToolTip="Please select the line where the Tools will be copied to.">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" CssClass="spanLabel">Style Group:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStyleGroups_Copy" runat="server" Width="220px" CssClass="selectDropDown"
                        ToolTip="Please select the Style Group where the Tools will be copied to.">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" CssClass="spanLabel">Station:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStations_Copy" runat="server" Width="220px" CssClass="selectDropDown"
                        ToolTip="Please select the Station where the Tools will be copied to.">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
