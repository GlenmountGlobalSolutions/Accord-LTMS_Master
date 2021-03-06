<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StationTaskConfiguration.aspx.vb" Inherits="LTMS_Master.StationTaskConfiguration" EnableEventValidation="false" %>

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

        var allCopyDialogFields;

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

                //add change events to poppulate hidden fields for postback.
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

            ClearWaitOverlay();
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
        #divMainContent span.ui-icon, #divDialogCopy span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 230px;
        }
        #divMainLeftPanel select
        {
            width: 220px;
        }
        #cmdPanel
        {
            width: 220px;
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
                <asp:Label ID="Label2" runat="server" CssClass="spanLabel h2" Enabled="false">Style Group:</asp:Label>
                <asp:DropDownList ID="ddlStyleGroups" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server" CssClass="spanLabel h2" Enabled="false">Station:</asp:Label>
                <asp:DropDownList ID="ddlStations" runat="server" AutoPostBack="True" CssClass="selectDropDown h3">
                </asp:DropDownList>
                <div id="cmdPanel">
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save Station Task" Text="Save"></asp:Button>
                    <asp:Button ID="cmdCopy" runat="server" CssClass="inputButton" ToolTip="Copy Station Task" Text="Copy"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:DataList ID="dataListTaskConfig" runat="server">
                    <ItemTemplate>
                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="dllblDesc" runat="server" CssClass="spanLabel h3" Width="150px" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                        <GGS:WebDropDownList ID="dlwddlTaskParam" runat="server" CssClass="selectDropDown h3" Width="107px" OnSelectedIndexChanged="dlwddlTaskParam_IndexChanged"></GGS:WebDropDownList>
                        <GGS:WebInputBox ID="dlwibTaskParam" Text='<%# Container.DataItem("TaskParamValue")%>' OnTextChanged="dlwibTaskParam_TextChanged" runat="server" Width="100px" CssClass="textEntry h3"></GGS:WebInputBox>
                        <GGS:WebCheckBox ID="cbLTReq" Text="LT Req." runat="server"  OnCheckedChanged="cbLTReq_CheckedChanged" Checked='<%# Container.DataItem("LT_Req")%>' CssClass="selectCheckBox h3"></GGS:WebCheckBox>
                        <asp:Image ID="dlImgCheck" runat="server" Visible="False" ImageUrl="~/Images/check.gif" Width="20px" Height="18px"></asp:Image>
                        <%#ObjectVisibilityInTemplate(Container)%>
                        <%#ObjectVisibilityInTemplate2(Container)%>
                    </ItemTemplate>
                </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogCopy" title="Copy Station Task Configuration">
        <p class="validationHints h4">
            All form fields are required.</p>
        <table id="Table1" class="tableCenter">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" CssClass="spanLabel">Line:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlLineNumber_Copy" runat="server" Width="220px" CssClass="selectDropDown" ToolTip="Please select the line where the Tasks will be copied to.">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" CssClass="spanLabel">Style Group:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStyleGroups_Copy" runat="server" Width="220px" CssClass="selectDropDown" ToolTip="Please select the Style Group where the Tasks will be copied to.">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" CssClass="spanLabel">Station:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStations_Copy" runat="server" Width="220px" CssClass="selectDropDown" ToolTip="Please select the Station where the Tasks will be copied to.">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
