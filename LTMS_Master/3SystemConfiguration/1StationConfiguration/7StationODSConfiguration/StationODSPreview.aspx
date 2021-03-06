<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StationODSPreview.aspx.vb" Inherits="LTMS_Master.StationODSPreview" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" href="odsPreview.css?ver=1.2" rel="stylesheet" />
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/rgbcolor.js") %>"></script>
    <script type="text/javascript">
        function contentPageLoad(sender, args) {

            if (args.get_isPartialLoad()) {
            }
            SetMaxSizeForToolsGridView();
            UpdateGridHeaderColor();
            UpdateForColorOfInstructionBasedOnBackColor();
        }

        function UpdateGridHeaderColor() {
            var color = new RGBColor($('#MainContent_hidHeaderColor').val());

            if (color.ok) {
                $('tr.odsGrid_HeaderStyle, .divProductHeader').css("background-color", color.toRGB())
                                                              .css("color", "black");
            }
        }

        function SetMaxSizeForToolsGridView() {
            var divPPEHeight = parseInt($('#divPPE').css("height"), 10);
            var maxToolsHeight = 470 - (divPPEHeight - 60);
            $('.toolsTableWrapper').css("max-height", maxToolsHeight);
        }

        function UpdateForColorOfInstructionBasedOnBackColor() {
            $('tr.odsGrid_HeaderStyle, .divProductHeader').each(function () {
                var opt = $(this);
                var backColor = opt.css("background-color");
                var foreColor = ConvertForeColor(backColor);
                opt.css("color", foreColor);
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input id="hidHeaderColor" type="hidden" name="hidHeaderColor" runat="server" value="" />
            <div id="divMainContent">
                <input id="copyStepCount" type="hidden" name="copyStepCount" runat="server" value="0" />
                <input id="rowSelectorDelta" type="hidden" name="rowSelectorDelta" runat="server" value="0" />
                <div id="divFilterPanel">
                    <p>
                        <asp:Label ID="Label1" runat="server" CssClass="h2 spanLabel">Line:</asp:Label>
                        <asp:DropDownList ID="ddlLineNumbers" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="Label2" runat="server" CssClass="h2 spanLabel">Station:</asp:Label>
                        <asp:DropDownList ID="ddlStations" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="lblInstructionSet" runat="server" CssClass="h2 spanLabel">Instruction Set:</asp:Label>
                        <asp:DropDownList ID="ddlVehicleModels" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="Label4" runat="server" CssClass="h2 spanLabel">MCR Number:</asp:Label>
                        <GGS:WebDropDownList ID="ddlMCRNumber" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="Label3" runat="server" CssClass="h2 spanLabel">Model:</asp:Label>
                        <asp:DropDownList ID="ddlModels" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="Label5" runat="server" CssClass="h2 spanLabel">Product Code:</asp:Label>
                        <asp:DropDownList ID="ddlProductCode" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange" />
                    </p>
                    <p>
                        <asp:Label ID="Label6" runat="server" CssClass="h2 spanLabel">Display:</asp:Label>
                        <asp:DropDownList ID="ddlDisplay" runat="server" AutoPostBack="True" CssClass="selectDropDown waitCursorOnChange selectDropDown2" >
                            <asp:ListItem Text="ODS" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Wire Harness" Value="1" ></asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="inputButton" />
                    </p>
                </div>
                <asp:Panel ID="divSimulateForm" runat="server" CssClass="divSimulateForm">
                    <div id="ODSPreview">
                        <div id="logoHeader">
                            <asp:Image ID="imgSetexLogo" runat="server" CssClass="imgSetexLogo" ImageUrl="~/Images/ODS/setex.png" />
                            <asp:Panel ID="pnlStationDescription" runat="server" CssClass="divProductHeader">
                                <asp:Label ID="lblStationName" runat="server" Text="" CssClass="stationNameLabel"></asp:Label>                        
                            </asp:Panel>
                            <asp:Image ID="imgGGSLogo" runat="server" CssClass="imgGGSLogo" ImageUrl="~/Images/ODS/GGSLargeNoShadow.png" />
                        </div>
                        <div id="previewProductInfo">
                            <span class="productInfoLabel" >Product Information</span>
                            <span class="productInfoLabel" >Honda Code:</span>
                            <asp:Label ID="lblHondaCode" runat="server" Text="" CssClass="boldLabel"></asp:Label>                        
                            <span class="verticleBar"></span>
                            <span class="productInfoLabel">Product Code:</span>
                            <asp:Label ID="lblProductCode" runat="server" Text="" CssClass="boldLabel"></asp:Label>                        
                            <span class="verticleBar"></span>
                            <span class="productInfoLabel">Model:</span>
                            <asp:Label ID="lblModel" runat="server" Text="" CssClass="boldLabel"></asp:Label>                        
                        </div>
                        <asp:Panel ID="pnlODS" runat="server">
                        <div id="divOdsSteps" class="odsGrid">
                            <asp:GridView ID="gvInstructions" runat="server" AutoGenerateColumns="False" GridLines="None" CssClass="odsGrid" HeaderStyle-CssClass="odsGrid_HeaderStyle" 
                            DataSourceID="sqlDataSource_InstructionSteps" DataKeyNames="DisplayID" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:BoundField DataField="DisplayID" HeaderText="" HeaderStyle-CssClass="odsColumnStepID" ItemStyle-CssClass="odsColumnStepID" />
                                    <asp:TemplateField HeaderText="Symbol" HeaderStyle-CssClass="odsColumnSymbol"><ItemTemplate><asp:Image ID="imgSeeEDSFlag" runat="server" AlternateText="See EDS" CssClass="symbolKey" /></ItemTemplate></asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="odsColumnSymbol"><ItemTemplate><asp:Image ID="imgSpecificQualityFlag" runat="server"  AlternateText="Critical Feature" CssClass="symbolKey" /></ItemTemplate></asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="odsColumnSymbol"><ItemTemplate><asp:Image ID="imgSeqControlFlag" runat="server"  AlternateText="Sequence Control" CssClass="symbolKey" /></ItemTemplate></asp:TemplateField>
                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-CssClass="odsColumnDescription" ItemStyle-CssClass="odsColumnDescription"/>
                                </Columns>
                            </asp:GridView>
                        </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlEDS" runat="server" Visible="False">
                        <div id="divEdsSteps" class="divEDSImage">
                            <asp:Image ID="imgWireHarness" class="imgEDSImage imageCenter" runat="server" />
                        </div>
                        </asp:Panel>
                        <div id="MCRColumn">
                            <div class="mcrColumnWrapper"> 
                                <div id="divMCR" class="auxGridDiv">
                                    <asp:GridView ID="gvMCR" runat="server" AutoGenerateColumns="False" GridLines="None" HeaderStyle-CssClass="odsGrid_HeaderStyle" 
                                        DataSourceID="sqlDataSource_MCR" ShowHeaderWhenEmpty="true">
                                        <Columns>
                                            <asp:BoundField DataField="MCRName" HeaderText="Change Point Control (4.2)" ItemStyle-CssClass="mcrName_ItemStyle" />
                                            <asp:BoundField DataField="MCRValue" HeaderText="Date" ItemStyle-CssClass="mcrValue_ItemStyle" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <asp:Panel ID="pnlPPETools" runat="server" >
                                    <div id="divPPE" class="auxGridDiv">
                                        <asp:GridView ID="gvPPE" runat="server" AutoGenerateColumns="False" GridLines="None" HeaderStyle-CssClass="odsGrid_HeaderStyle" 
                                            DataSourceID="sqlDataSource_AuxListPPE" ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:BoundField DataField="Description" HeaderText="PPE" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="toolsBorderWrapper">
                                        <div class="toolsBorder">
                                            <div class="toolsTableWrapper">
                                                <asp:GridView ID="gvTools" runat="server" AutoGenerateColumns="False" GridLines="None" HeaderStyle-CssClass="odsGrid_HeaderStyle" 
                                                    DataSourceID="sqlDataSource_AuxListTools" ShowHeaderWhenEmpty="true">
                                                    <Columns>
                                                        <asp:BoundField DataField="Description" HeaderText="Tools" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divSymbolKey" class="symbolKeyDiv" >
                                        <table>
                                            <tr class="odsGrid_HeaderStyle"><th colspan="2">Symbol Key (7.1)</th></tr>
                                            <tr><td class="symbolKey"><img src="../../../Images/ODS/SeeEDS.png" alt="See EDS" /></td><td>See EDS</td></tr>
                                            <tr><td class="symbolKey"><img src="../../../Images/ODS/SpecificQuality.png" alt="Critical Features" /></td><td>Critical Features</td></tr>
                                            <tr><td class="symbolKey"><img src="../../../Images/ODS/SeqControl.png" alt="Sequence Control" /></td><td>Sequence Control</td></tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div id="divBelowSteps">
                            <asp:Panel ID="pnlAbCntrl" runat="server" >
                                <div class="abnormalControl">
                                    <table>
                                        <tr class="odsGrid_HeaderStyle"><th colspan="2">Abnormal Control (8.3)</th></tr>
                                        <tr><td colspan="2" class="textCenter acFont1">All Team Members have the Responsibility and Authority to:</td></tr>
                                        <tr><td class="textCenter acFont2">STOP -</td><td class="textLeft acFont2">For Safety, Quality Issues</td></tr>
                                        <tr><td class="textCenter acFont2">CALL -</td><td class="textLeft acFont2">Team Leader, Quality, or Supervisor</td></tr>
                                        <tr><td class="textCenter acFont2">WAIT -</td><td class="textLeft acFont2">For Further Instructions</td></tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:SqlDataSource ID="sqlDataSource_InstructionSteps" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" 
        SelectCommandType="StoredProcedure" SelectCommand="[ods].[procGetInstructionSteps]"  >
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlLineNumbers" PropertyName="SelectedValue" Name="LineID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlStations" PropertyName="SelectedValue" Name="StationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlVehicleModels" PropertyName="SelectedValue" Name="ConfigurationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlModels" PropertyName="SelectedValue" Name="ModelCode" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlMCRNumber" PropertyName="SelectedValue" Name="MasterChangeRequestID" DefaultValue="" ConvertEmptyStringToNull="True" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sqlDataSource_MCR" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" 
        SelectCommandType="StoredProcedure" SelectCommand="[ods].[procSelectMasterChangeRequestForPreview]"  >
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlMCRNumber" PropertyName="SelectedValue" Name="MasterChangeRequestID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlStations" PropertyName="SelectedValue" Name="StationID" DefaultValue="" ConvertEmptyStringToNull="True" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sqlDataSource_AuxListPPE" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" 
        SelectCommandType="StoredProcedure" SelectCommand="[ods].[procSelectInstructionAuxiliaryLists]"  >
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlLineNumbers" PropertyName="SelectedValue" Name="LineID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlStations" PropertyName="SelectedValue" Name="StationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlVehicleModels" PropertyName="SelectedValue" Name="ConfigurationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlMCRNumber" PropertyName="SelectedValue" Name="MasterChangeRequestID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:Parameter Name="ConfigurationTypeID" DefaultValue="4" ConvertEmptyStringToNull="True" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="sqlDataSource_AuxListTools" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>" 
        SelectCommandType="StoredProcedure" SelectCommand="[ods].[procSelectInstructionAuxiliaryLists]"  >
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlLineNumbers" PropertyName="SelectedValue" Name="LineID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlStations" PropertyName="SelectedValue" Name="StationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlVehicleModels" PropertyName="SelectedValue" Name="ConfigurationID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:ControlParameter ControlID="ddlMCRNumber" PropertyName="SelectedValue" Name="MasterChangeRequestID" DefaultValue="" ConvertEmptyStringToNull="True" />
            <asp:Parameter Name="ConfigurationTypeID" DefaultValue="3" ConvertEmptyStringToNull="True" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
