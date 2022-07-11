<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ProductionSchedule.aspx.vb" Inherits="LTMS_Master.ProductionSchedule"  EnableEventValidation="false"%>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemOperations.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.maskedinput-1.3.1.min.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.alphanum.js") %>"></script>
	<script type="text/javascript" src="<%# ResolveUrl("ProductionSchedule.js") %>"></script>
	<script type="text/javascript">
	    var $orderType;
	    var $hidLastTab;
	    var $hidNewProdID;
	    var $hidNewLotNew;
	    var $hidNewNValues;
	    var $hidLotNum8;
	    var $hidNodeLot;
	    var $hidNodeLevel;
	    var $hidNodeSeqDT;
	    var $hidTreeID;
	    var $hidEditProdID;
	    var $hidBroadcastPointID;
	    var $tbDay;

	    var $cmdRefresh;
	    var $cmdNew;
	    var $cmdMove;
	    var $cmdEdit;
	    var $cmdDelete;
	    var $cmdMoveDlg;
	    var $cmdEditDlg;
	    var $cmdDeleteDlg;
	    var $cmdResequence;
	    var $cmdOnHold
	    var $cmdOffHold

	    var $treeMon;
	    var $treeTue;
	    var $treeWed;
	    var $treeThu;
	    var $treeFri;
	    var $treeSat;
	    var $treeSun;
	    var $treeMove;

	    var $ddlNewLotNew;
	    var $ddlNewNValues;
	    var $ddlNewProdID;
	    var $ddlEditProdID;
	    var $ddlMoveBP;

	    var $txtNewLotNum;
	    var $txtNewSLIndex;
	    var $txtEditSLIndex;


	    function contentPageLoad(sender, args) {
	        CacheControls();

	        if (!args.get_isPartialLoad()) {
	            $ddlNewProdID.change(function () {
	                $hidNewProdID.val($(this).val());
	                ProductionSchedule_DialogNew_PopulateProductDetail($(this).val());
	            });

	            $ddlEditProdID.change(function () {
	                $hidEditProdID.val($(this).val());
	                ProductionSchedule_DialogEdit_PopulateProductDetail($(this).val());
	            });

	            $ddlNewLotNew.change(function () { $hidNewLotNew.val($(this).val()); });
	            $ddlNewNValues.change(function () { $hidNewNValues.val($(this).val()); });

	            $txtNewLotNum.change(function () { setControls(); });
	            $('#MainContent_cbNewNextPos').click(function () { setControls(); });
            }

	        MakeDivIntoTabs();
	        MakeDateTimePicker();
	        AddEventsToControls();
	        CreateDialog_Revision();
	        CreateDialog_Delete();
	        CreateDialog_Resequence();
	        CreateDialog_New();
	        CreateDialog_Edit();
	        CreateDialog_MoveLot();

	        SetButtonsEnabledState();
        }

        function CacheControls() {
            $hidLastTab = $("#MainContent_hidLastTab");
            $hidLotNum8 = $("#MainContent_hidLotNum8");
            $hidNodeLot = $("#MainContent_hidNodeLot");
            $hidNodeLevel = $("#MainContent_hidNodeLevel");
            $hidNodeSeqDT = $("#MainContent_hidNodeSeqDT");

            $hidNewProdID = $("#MainContent_hidNewProdID");
            $hidNewLotNew = $("#MainContent_hidNewLotNew");
            $hidNewNValues = $("#MainContent_hidNewNValues");
            $hidEditProdID = $("#MainContent_hidEditProdID");
            $hidBroadcastPointID = $("#MainContent_hidBroadcastPointID");

            $hidTreeID = $("#MainContent_hidTreeID");
            $orderType = $('#MainContent_rbOrders input:checked').val();
            $tbDay = $("#MainContent_tbDay");

            $cmdRefresh = $('#MainContent_cmdRefresh');
            $cmdNew = $('#MainContent_cmdNew');
            $cmdMove = $('#MainContent_cmdMove');
            $cmdEdit = $('#MainContent_cmdEdit');
            $cmdDelete = $('#MainContent_cmdDelete');
            $cmdMoveDlg = $('#MainContent_cmdMoveDlg');
            $cmdEditDlg = $('#MainContent_cmdEditDlg');
            $cmdDeleteDlg = $('#MainContent_cmdDeleteDlg');
            $cmdResequence = $('#MainContent_cmdResequence');
            $cmdOnHold = $('#MainContent_cmdOnHold');
            $cmdOffHold = $('#MainContent_cmdOffHold');

            $treeMon = $find("<%=treeMon.ClientID%>");
            $treeTue = $find("<%=treeTue.ClientID%>");
            $treeWed = $find("<%=treeWed.ClientID%>");
            $treeThu = $find("<%=treeThu.ClientID%>");
            $treeFri = $find("<%=treeFri.ClientID%>");
            $treeSat = $find("<%=treeSat.ClientID%>");
            $treeSun = $find("<%=treeSun.ClientID%>");
            $treeMove = $find("<%=treeMove.ClientID%>");
            $ddlMoveBP = $find("<%=ddlMoveBP.ClientID%>");


            $ddlNewLotNew = $("#MainContent_ddlNewLotNew");
            $ddlNewNValues = $("#MainContent_ddlNewNValues");
            $ddlNewProdID = $("#MainContent_ddlNewProdID");
            $ddlEditProdID = $('#MainContent_ddlEditProdID');

            $txtNewLotNum = $("#MainContent_txtNewLotNum").mask("9-9999-999");
            $txtNewSLIndex = $("#MainContent_txtNewSLIndex").mask("99");
            $txtEditSLIndex = $("#MainContent_txtEditSLIndex").mask("99");
            //            $(".resizable").resizable();

        }

    </script>


	<!-- This added for this page only we want the scroll for the tabs -->
	<style type="text/css" >
		#divButtonsTop .inputButton, #divButtonsBottom .inputButton
		{
			width: 105px;
		}
		.ui-tab-content, .ui-tabs-panel
		{
			overflow-x:scroll;
			overflow-y:hidden;
		}
		#tabs
		{
			max-width:1333px;
		}
		#divDlgNewLot .selectDropDown, #divDlgEditLot .selectDropDown
		{
			width: 200px;
		}
		#divDlgNewLot .textEntry, #divDlgEditLot .textEntry
		{
			width: 190px;
		}
		#divComments
		{
			vertical-align: bottom; 
			text-align: left; 
			margin: 4px; 
			max-width:1330px;
			padding-top:20px;
		}
		#divSelectBroadcastPointID
		{
			margin-left:170px;
		}
		td.treeArea
		{
		    width:186px;
		    min-width:186px;
		    text-align: center;
		}
		div.treePanel
		{
		    min-height:380px;
		    height:380px;
		    width:180px;
		}

		div.treePanelMove
		{
		    min-height:380px;
		    height:380px;
		    width:212px;
		}
        #ctl00_MainContent_treeMove
        {
            padding-left:8px;
        }
        .RadTreeView Img.rtImg 
        {
	        height: 12px;
	        width: 12px;
        /*    margin: 0px 6px 0px 2px;*/
	        display: inline;
	        vertical-align: middle;
        }
		
		.cmdBtn
		{
		    margin-left: 20px;
		}
		
		.cmdExport
		{
		    margin-right:20px;
		}		        
		.cmdResequence
		{
		    margin-left: 40px;
		}
		
        div.selectDropDownBP .rcbReadOnly
        {
            background-color: white;
        	border: 1px solid #6495ED;
	        border-radius: 5px;
        }
        div.selectDropDownBP .rcbActionButton
        {
            padding-top: 0px;
        }
        #ctl00_MainContent_ddlMoveBP_Arrow
        {
            background-image:url("../../Images/Arrows/ddl.png");
            background-size: 20px 20px;
            background-repeat: no-repeat;
            color: Transparent;
        }
        .rcbActionButton
        {
            margin: 0px -2px 0px 0px;
        }

		
	</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" DefaultLoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnResponseEnd="RadAjaxPanel_OnResponseEnd">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="treeMon">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="treeTue">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="treeWed">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="treeThu">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="treeFri">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="treeSat">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="treeSun">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="treeDay">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="rbOrders">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ibPrev">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ibNext">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmdRefresh">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cbHold">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmdExpandAll">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmdCollapseAll">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmdEdit">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
         <telerik:AjaxSetting AjaxControlID="cmdOnHold">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
         <telerik:AjaxSetting AjaxControlID="cmdOffHold">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="cmdDelete">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMon"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ddlMoveBP">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMove"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="treeMove">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="treeMove" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeMon" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeTue" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeWed" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeThu" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeFri" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSat" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeSun" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="treeDay" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
    </telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7" BackgroundPosition="Top"></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" BackgroundPosition="None"></telerik:RadAjaxLoadingPanel>

	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
        	<div id="divHiddenFields">
				<input id="hidLastTab" type="hidden" runat="server" />
				<input id="hidLotNum8" type="hidden" runat="server" />
				<input id="hidNodeLot" type="hidden" runat="server" />
				<input id="hidNodeLevel" type="hidden" runat="server" />
				<input id="hidNodeSeqDT" type="hidden" runat="server" />
				<input id="hidTreeID" type="hidden" runat="server" />

                <input id="hidNewProdID" type="hidden" runat="server" />
                <input id="hidNewLotNew" type="hidden" runat="server" />
				<input id="hidNewNValues" type="hidden" runat="server" />
				<input id="hidEditProdID" type="hidden" runat="server" />
				<input id="hidBroadcastPointID" type="hidden" runat="server" />
			</div>
            <div id="divFilters" style="margin: 4px; text-align: left;">
                <table style="border-collapse: collapse; padding: 0; margin-bottom: 10px; width: 1400px">
                    <tr style="vertical-align: middle;">
                        <td style="width: 220px">
                            <asp:RadioButtonList ID="rbOrders" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                CssClass="inputRadioButton">
                                <asp:ListItem Value="1" Selected="True">Setex Orders</asp:ListItem>
                                <asp:ListItem Value="0">All Orders</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 80px">
                            <asp:Label ID="LABEL18" runat="server" CssClass="spanLabel">Enter Date:</asp:Label>
                        </td>
                        <td style="padding-left: 20px; width: 16px; text-align: right; padding-top: 8px;">
                            <asp:ImageButton ID="ibPrev" runat="server" CssClass="cmdShowWait" ImageUrl="~/images/Arrows/bbPrevpage.gif">
                            </asp:ImageButton>
                        </td>
                        <td style="width: 140px; text-align: left; padding-left: 10px;">
                            <asp:TextBox ID="tbDay" runat="server" Width="96px" CssClass="textEntry"></asp:TextBox>
                        </td>
                        <td style="width: 16px; text-align: left; padding-top: 8px;">
                            <asp:ImageButton ID="ibNext" runat="server" CssClass="cmdShowWait" ImageUrl="~/images/Arrows/bbNextpage.gif">
                            </asp:ImageButton>
                        </td>
                        <td style="text-align: left; padding: 0px 0px 0px 12px; width: 90px;">
                            <asp:Button ID="cmdRefresh" runat="server" CssClass="inputButton cmdShowWait" Text="Refresh">
                            </asp:Button>
                        </td>
                        <td style="width: 180px;">
                            <asp:CheckBox ID="cbHold" runat="server" AutoPostBack="True" Text="Show ON HOLD Lots"
                                Checked="True" CssClass="selectCheckBox h3" />
                        </td>
                        <td style="width: 120px;">
                            <asp:Label ID="lblWeekTotalCaption" runat="server" CssClass="spanLabel">Total for Week:</asp:Label>
                            <asp:Label ID="lblTotal" runat="server" CssClass="spanLabel">?</asp:Label>
                        </td>
                        <td>
                            <table class="tblBP" style="margin-left: 40px">
							    <tr>
								    <td><asp:Panel ID="Panel1" runat="server" CssClass="divBP" Visible="True" ><asp:Image ID="image1" runat="server" CssClass='calendarFilterImgBP1' AlternateText="1"></asp:Image><asp:Label ID="lblBP1" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
								    <td><asp:Panel ID="Panel3" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image3" runat="server" CssClass='calendarFilterImgBP2' AlternateText="2" ></asp:Image><asp:Label ID="lblBP3" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
							    </tr>
							    <tr>
								    <td><asp:Panel ID="Panel2" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image2" runat="server" CssClass='calendarFilterImgBP3' AlternateText="3" ></asp:Image><asp:Label ID="lblBP2" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
								    <td><asp:Panel ID="Panel4" runat="server" CssClass="divBP" Visible="False"><asp:Image ID="image4" runat="server" CssClass='calendarFilterImgBP4' AlternateText="4" ></asp:Image><asp:Label ID="lblBP4" runat="server" Text="" CssClass="spanLabel h4 divBPSpan"></asp:Label></asp:Panel></td>
							    </tr>
						    </table>

                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel>
        
            <div id="tabs" class="">
                <ul>
                    <li><a href="#tabs-1">Week</a></li>
                    <li><a href="#tabs-2">Day</a></li>
                </ul>
                <div id="tabs-1" class="displayNone resizable" style="">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <table id="Table2" style="border-collapse: collapse; padding: 0; margin-left: -14px;">
                                <tr>
                                    <td class="treeArea">
                                        <asp:Label ID="lblDateMon" runat="server" CssClass="spanLabel h4">Monday</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="lblDateTue" runat="server" CssClass="spanLabel h4">Tuesday</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="lblDateWed" runat="server" CssClass="spanLabel h4">Wednesday</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="lblDateThu" runat="server" CssClass="spanLabel h4">Thursday</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="lblDateFri" runat="server" CssClass="spanLabel h4">Fri</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="lblDateSat" runat="server" CssClass="spanLabel h4">Saturday</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="lblDateSun" runat="server" CssClass="spanLabel h4">Sunday</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
	                </asp:UpdatePanel>

                    <table id="Table1" style="border-collapse: collapse; padding: 0; margin-left: -14px;">
                        <tr>
                            <td style="vertical-align: middle">
                                <asp:Panel ID="pnlTreeviewMon" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel ">
                                    <telerik:RadTreeView ID="treeMon" runat="server" RenderMode="Lightweight" 
                                        BorderWidth="0" 
                                        OnClientContextMenuShowing="RadTreeView_onClientContextMenuShowing"
                                        OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                        OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                        skin="Windows7"  CssClass="treeViewNode"
                                        MultipleSelect="False" ShowLineImages="False" 
                                        >
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="rtvMonContextMenu1" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Value="OnHold" Text="On Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="OffHold" Text="Off Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="none" Text="None" IsSeparator="True"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All"></telerik:RadMenuItem>
                                                </Items>
                                                <CollapseAnimation Type="none"></CollapseAnimation>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlTreeviewTue" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel">
                                    <telerik:RadTreeView ID="treeTue" runat="server" RenderMode="Lightweight" 
                                        BorderWidth="0" 
                                        OnClientContextMenuShowing="RadTreeView_onClientContextMenuShowing"
                                        OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                        OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                        skin="Windows7"  CssClass="treeViewNode"
                                        MultipleSelect="False" ShowLineImages="False">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu1" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Value="OnHold" Text="On Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="OffHold" Text="Off Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="none" Text="None" IsSeparator="True"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All"></telerik:RadMenuItem>
                                                </Items>
                                                <CollapseAnimation Type="none"></CollapseAnimation>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlTreeviewWed" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel">
                                    <telerik:RadTreeView ID="treeWed" runat="server" RenderMode="Lightweight" 
                                        BorderWidth="0" 
                                        OnClientContextMenuShowing="RadTreeView_onClientContextMenuShowing"
                                        OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                        OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                        skin="Windows7"  CssClass="treeViewNode"
                                        MultipleSelect="False" ShowLineImages="False">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu2" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Value="OnHold" Text="On Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="OffHold" Text="Off Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="none" Text="None" IsSeparator="True"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All"></telerik:RadMenuItem>
                                                </Items>
                                                <CollapseAnimation Type="none"></CollapseAnimation>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlTreeviewThu" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel">
                                    <telerik:RadTreeView ID="treeThu" runat="server" RenderMode="Lightweight" 
                                        BorderWidth="0" 
                                        OnClientContextMenuShowing="RadTreeView_onClientContextMenuShowing"
                                        OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                        OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                        skin="Windows7"  CssClass="treeViewNode"
                                        MultipleSelect="False" ShowLineImages="False">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu6" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Value="OnHold" Text="On Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="OffHold" Text="Off Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="none" Text="None" IsSeparator="True"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All"></telerik:RadMenuItem>
                                                </Items>
                                                <CollapseAnimation Type="none"></CollapseAnimation>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlTreeviewFri" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel">
                                    <telerik:RadTreeView ID="treeFri" runat="server" RenderMode="Lightweight" 
                                        BorderWidth="0" 
                                        OnClientContextMenuShowing="RadTreeView_onClientContextMenuShowing"
                                        OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                        OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                        skin="Windows7"  CssClass="treeViewNode"
                                        MultipleSelect="False" ShowLineImages="False">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu5" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Value="OnHold" Text="On Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="OffHold" Text="Off Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="none" Text="None" IsSeparator="True"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All"></telerik:RadMenuItem>
                                                </Items>
                                                <CollapseAnimation Type="none"></CollapseAnimation>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlTreeviewSat" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel">
                                    <telerik:RadTreeView ID="treeSat" runat="server" RenderMode="Lightweight" 
                                        BorderWidth="0" 
                                        OnClientContextMenuShowing="RadTreeView_onClientContextMenuShowing"
                                        OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                        OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                        skin="Windows7"  CssClass="treeViewNode"
                                        MultipleSelect="False" ShowLineImages="False">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu4" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Value="OnHold" Text="On Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="OffHold" Text="Off Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="none" Text="None" IsSeparator="True"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All"></telerik:RadMenuItem>
                                                </Items>
                                                <CollapseAnimation Type="none"></CollapseAnimation>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlTreeviewSun" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel">
                                    <telerik:RadTreeView ID="treeSun" runat="server" RenderMode="Lightweight" 
                                        BorderWidth="0" 
                                        OnClientContextMenuShowing="RadTreeView_onClientContextMenuShowing"
                                        OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                        OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                        skin="Windows7"  CssClass="treeViewNode"
                                        MultipleSelect="False" ShowLineImages="False">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu3" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Value="OnHold" Text="On Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="OffHold" Text="Off Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="none" Text="None" IsSeparator="True"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All"></telerik:RadMenuItem>
                                                </Items>
                                                <CollapseAnimation Type="none"></CollapseAnimation>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <table id="Table4" style="border-collapse: collapse; padding: 0; margin-left: -14px;">
                                <tr>
                                    <td class="treeArea">
                                        <asp:Label ID="Label7" runat="server" CssClass="spanLabel h4">Total: </asp:Label>
                                        <asp:Label ID="lblTotalMon" runat="server" CssClass="spanLabel h4">0</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="Label9" runat="server" CssClass="spanLabel h4">Total: </asp:Label>
                                        <asp:Label ID="lblTotalTue" runat="server" CssClass="spanLabel h4">0</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="Label11" runat="server" CssClass="spanLabel h4">Total: </asp:Label>
                                        <asp:Label ID="lblTotalWed" runat="server" CssClass="spanLabel h4">0</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="Label13" runat="server" CssClass="spanLabel h4">Total: </asp:Label>
                                        <asp:Label ID="lblTotalThu" runat="server" CssClass="spanLabel h4">0</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="Label15" runat="server" CssClass="spanLabel h4">Total: </asp:Label>
                                        <asp:Label ID="lblTotalFri" runat="server" CssClass="spanLabel h4">0</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="Label17" runat="server" CssClass="spanLabel h4">Total: </asp:Label>
                                        <asp:Label ID="lblTotalSat" runat="server" CssClass="spanLabel h4">0</asp:Label>
                                    </td>
                                    <td class="treeArea">
                                        <asp:Label ID="Label19" runat="server" CssClass="spanLabel h4">Total: </asp:Label>
                                        <asp:Label ID="lblTotalSun" runat="server" CssClass="spanLabel h4">0</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
	                </asp:UpdatePanel>
                </div>
                <div id="tabs-2" class="displayNone">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <table id="Table3" style="border-collapse: collapse; padding: 0;">
                                <tr>
                                    <td class="treeArea">
                                        <asp:Label ID="lblSingleDayOfWeek" runat="server" CssClass="spanLabel h4"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
	                </asp:UpdatePanel>
                    <table id="Table5" style="border-collapse: collapse; padding: 0;">
                        <tr>
                            <td style="vertical-align: middle">
                                <asp:Panel ID="PaneltreeDay" runat="server" ScrollBars="Auto" CssClass="divBorder treePanel">
                                    <telerik:RadTreeView ID="treeDay" runat="server" RenderMode="Lightweight" 
                                        BorderWidth="0" 
                                        OnClientContextMenuShowing="RadTreeView_onClientContextMenuShowing"
                                        OnClientNodeClicked="RadTreeView_OnClientNodeClicked"
                                        OnContextMenuItemClick="RadTreeView_ContextMenuItemClick"
                                        skin="Windows7"  CssClass="treeViewNode"
                                        MultipleSelect="False" ShowLineImages="False">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu7" runat="server">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Value="OnHold" Text="On Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="OffHold" Text="Off Hold"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="none" Text="None" IsSeparator="True"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="ExpandAll" Text="Expand All"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" Value="CollapseAll" Text="Collapse All"></telerik:RadMenuItem>
                                                </Items>
                                                <CollapseAnimation Type="none"></CollapseAnimation>
                                            </telerik:RadTreeViewContextMenu>
                                        </ContextMenus>
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <table id="Table6" style="border-collapse: collapse; padding: 0;">
                                <tr>
                                    <td class="treeArea">
                                        <asp:Label ID="LABEL21" runat="server" CssClass="spanLabelh4">Total: </asp:Label>
                                        <asp:Label ID="lblTotalSingleDay" runat="server" CssClass="spanLabel h4">0</asp:Label>
                                    </td>
                                </tr>
                            </table>
                    </ContentTemplate>
	            </asp:UpdatePanel>
            </div>
        </div>
	<div id="divButtonsTop" style="margin: 4px; text-align: left;">
		<asp:UpdatePanel ID="UpdatePanel6" runat="server">
			<ContentTemplate>
		        <asp:Button ID="cmdNew" runat="server" Text="New" CssClass="inputButton"></asp:Button>
		        <asp:Button ID="cmdMove" runat="server" Text="Move" CssClass="displayNone"></asp:Button>
		        <asp:Button ID="cmdEdit" runat="server" Text="Edit" CssClass="displayNone"></asp:Button>
		        <asp:Button ID="cmdDelete" runat="server" Text="Delete" CssClass="displayNone"></asp:Button>
		        <asp:Button ID="cmdMoveDlg" runat="server" Text="Move" CssClass="inputButton"></asp:Button>
		        <asp:Button ID="cmdEditDlg" runat="server" Text="Edit" CssClass="inputButton"></asp:Button>
		        <asp:Button ID="cmdOnHold" runat="server" Text="On Hold" CssClass="inputButton"></asp:Button>
		        <asp:Button ID="cmdOffHold" runat="server" Text="Off Hold" CssClass="inputButton"></asp:Button>
		        <asp:Button ID="cmdDeleteDlg" runat="server" Text="Delete" CssClass="inputButton"></asp:Button>
		        <asp:Button ID="cmdRevInc" runat="server" Text="Revisions" CssClass="inputButton cmdBtn"></asp:Button>
		        <asp:Button ID="cmdPrint" runat="server" Text="Print" CssClass="inputButton cmdBtn"></asp:Button>
		        <asp:Button ID="cmdExport" runat="server" Text="Export" CssClass="inputButton cmdExport"></asp:Button>
		        <asp:Button ID="cmdExpandAll" runat="server" Text="Expand All" CssClass="inputButton cmdExpandAll"></asp:Button>
		        <asp:Button ID="cmdCollapseAll" runat="server" Text="Collapse All" CssClass="inputButton cmdCollapseAll"></asp:Button>
		        <asp:Button ID="cmdResequence" runat="server" Text="Resequence" CssClass="inputButton cmdResequence cmdShowWait"></asp:Button>
            </ContentTemplate>
        </asp:UpdatePanel>
	</div>
	<div id="divComments" >
		<asp:UpdatePanel ID="UpdatePanel2" runat="server">
			<ContentTemplate>
				<asp:Panel ID="pnlRev" runat="server">
					<asp:Label ID="Label8" runat="server" ForeColor="Black" CssClass="spanLabel">Revision No:</asp:Label>
					<asp:Label ID="lblRevNo" runat="server" CssClass="spanLabel h3"></asp:Label>
					<asp:Label ID="Label12" runat="server" ForeColor="Black" CssClass="spanLabel">Comments:</asp:Label>
					<asp:Label ID="lblRevComments" runat="server" CssClass="spanLabel h3"></asp:Label>
				</asp:Panel>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
    <div id="divDlgAddRevision" title="Revision" class="displayNone">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <p class="validationHints h4">
                    All fields must be completed.</p>
                <table id="tblRevision" class="tableCenter">
                    <tr>
                        <td style="width: 92px">
                            <asp:Label ID="Label10" runat="server" CssClass="spanLabel h4">Date:</asp:Label>
                        </td>
                        <td style="width: 100%">
                            <asp:Label ID="lblDate" runat="server" CssClass="spanLabel h4"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LABEL14" runat="server" CssClass="spanLabel h4">Revision: </asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRev" runat="server" CssClass="spanLabel h4"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="height: 100%">
                        <td style="vertical-align: top">
                            <asp:Label ID="LABEL16" runat="server" CssClass="spanLabel h4">Comments: </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtComment" runat="server" CssClass="textEntry h4" MaxLength="500" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
                        </td>
                        <td>
                            <div id="helpRevisionComment" class="ui-state-default ui-corner-all">
                                <span class="ui-icon ui-icon-help"></span>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
	<div id="divDlgDelete" title="Delete" class="displayNone">
		<p class="pCenter">
			<span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>You are about to Delete. Do you wish to continue?
		</p>
	</div>
	<div id="divDlgResequence" title="Resequence" class="displayNone">
		<p class="pCenter">
			<span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>You are about to Resequence the Lots.<br />Do you wish to continue?
		</p>
	</div>
	<div id="divDlgNewLot" title="New" class="displayNone">
		<div id="divDlgNewOverlay" class="internalOverlay" ></div>
			<p class="validationHints h5">
				All fields must be completed.</p>
			<table id="tblNewLot" class="tableCenter">
			<tr>
				<td>
					<asp:Label ID="lblNewDescriptionBroadcastPointID" runat="server" CssClass="spanLabel h3"></asp:Label>
				</td>
				<td>
					<asp:Label ID="lblNewSelectedBroadcastPointID" runat="server" CssClass="spanLabel"></asp:Label>
				</td>
			</tr>
				<tr>
					<td>
						<asp:Label ID="Label23" runat="server" CssClass="spanLabel h3">Lot Number</asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtNewLotNum" runat="server" CssClass="textEntry h4"></asp:TextBox>
					</td>
					<td>
						<div id="helptxtNewLotNum" class="ui-state-default ui-corner-all">
							<span class="ui-icon ui-icon-help"></span>
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="Label24" runat="server" CssClass="spanLabel h3">Sub-Lot Index</asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtNewSLIndex" runat="server" CssClass="textEntry h4"></asp:TextBox>
					</td>
					<td>
						<div id="helptxtNewSLIndex" class="ui-state-default ui-corner-all">
							<span class="ui-icon ui-icon-help"></span>
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="Label25" runat="server" CssClass="spanLabel h3">Job Quantity</asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtNewQuantity" runat="server" CssClass="textEntry h4 numericOnly"></asp:TextBox>
					</td>
					<td>
						<div id="helptxtNewQuantity" class="ui-state-default ui-corner-all">
							<span class="ui-icon ui-icon-help"></span>
						</div>
					</td>
				</tr>
				<tr style="vertical-align: top">
					<td>
						<asp:Label ID="Label26" runat="server" CssClass="spanLabel h3" Width="120px">Frame Code & Interior Color</asp:Label>
					</td>
					<td>
						<asp:DropDownList ID="ddlNewProdID" runat="server" CssClass="selectDropDown h3">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="LABEL28" runat="server" CssClass="spanLabel h3">N-Value</asp:Label>
					</td>
					<td>
						<asp:DropDownList ID="ddlNewNValues" runat="server" CssClass="selectDropDown h3">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="LABEL29" runat="server" Width="140px" CssClass="spanLabel h3">Shipping Code (4)</asp:Label>
					</td>
					<td>
						<asp:Label ID="lblNewShipCode4" runat="server" CssClass="spanLabel h4"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="Label30" runat="server" CssClass="spanLabel h3">Model Desc.</asp:Label>
					</td>
					<td>
						<asp:Label ID="lblNewModelDesc" runat="server" CssClass="spanLabel h4"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="LABEL31" runat="server" CssClass="spanLabel h3">Driver Seat Style</asp:Label>
					</td>
					<td>
						<asp:Label ID="lblNewDriverSS" runat="server" CssClass="spanLabel h4"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="LABEL32" runat="server" CssClass="spanLabel h3">Pass. Seat Style</asp:Label>
					</td>
					<td>
						<asp:Label ID="lblNewPassSS" runat="server" CssClass="spanLabel h4"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="LABEL33" runat="server" CssClass="spanLabel h3">Location</asp:Label>
					</td>
					<td>
						<asp:CheckBox ID="cbNewNextPos" runat="server" Text="Next Position" CssClass="inputRadioButton h4"></asp:CheckBox><br>
						<asp:RadioButtonList ID="rblNewLocation" runat="server" RepeatDirection="Horizontal" CssClass="h4">
							<asp:ListItem Value="BEFORE" Selected="True">Before</asp:ListItem>
							<asp:ListItem Value="AFTER">After</asp:ListItem>
						</asp:RadioButtonList>
					</td>
				</tr>
				<tr>
					<td style="height: 13px">
					</td>
					<td style="height: 13px">
						<asp:DropDownList ID="ddlNewLotNew" runat="server" CssClass="selectDropDown h4">
						</asp:DropDownList>
					</td>
				</tr>
			</table>
	</div>
	<div id="divDlgEditLot" title="Edit" class="displayNone">
		<div id="divDlgEditOverlay" class="internalOverlay"></div>
		<p class="validationHints h5">All fields must be completed.</p>
		<table id="tblEditLot" class="tableCenter">
			<tr>
				<td>
					<asp:Label ID="Label_23" runat="server" CssClass="spanLabel h3">Lot Number</asp:Label>
				</td>
				<td>
					<asp:TextBox ID="txtEditLotNum" runat="server" CssClass="textEntry h4" ReadOnly="true"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="Label_24" runat="server" CssClass="spanLabel h3">Sub-Lot Index</asp:Label>
				</td>
				<td>
					<asp:TextBox ID="txtEditSLIndex" runat="server" CssClass="textEntry h4"></asp:TextBox>
				</td>
				<td>
					<div id="helptxtEditSLIndex" class="ui-state-default ui-corner-all">
						<span class="ui-icon ui-icon-help"></span>
					</div>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="Label_25" runat="server" CssClass="spanLabel h3">Job Quantity</asp:Label>
				</td>
				<td>
					<asp:TextBox ID="txtEditQuantity" runat="server" CssClass="textEntry h4 numericOnly"></asp:TextBox>
				</td>
				<td>
					<div id="helptxtEditQuantity" class="ui-state-default ui-corner-all">
						<span class="ui-icon ui-icon-help"></span>
					</div>
				</td>
			</tr>
			<tr style="vertical-align: top">
				<td>
					<asp:Label ID="Label_26" runat="server" CssClass="spanLabel h3" Width="120px">Frame Code & Interior Color</asp:Label>
				</td>
				<td>
					<asp:DropDownList ID="ddlEditProdID" runat="server" CssClass="selectDropDown h3" >
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="LABEL_29" runat="server" Width="140px" CssClass="spanLabel h3">Shipping Code (4)</asp:Label>
				</td>
				<td>
					<asp:Label ID="lblEditShipCode4" runat="server" CssClass="spanLabel h4"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="Label_30" runat="server" CssClass="spanLabel h3">Model Desc.</asp:Label>
				</td>
				<td>
					<asp:Label ID="lblEditModelDesc" runat="server" CssClass="spanLabel h4"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="LABEL_31" runat="server" CssClass="spanLabel h3">Driver Seat Style</asp:Label>
				</td>
				<td>
					<asp:Label ID="lblEditDriverSS" runat="server" CssClass="spanLabel h4"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="LABEL_32" runat="server" CssClass="spanLabel h3">Pass. Seat Style</asp:Label>
				</td>
				<td>
					<asp:Label ID="lblEditPassSS" runat="server" CssClass="spanLabel h4"></asp:Label>
				</td>
			</tr>
			<tr>
				<td style="height: 13px">
					<asp:Label ID="LABEL_47" runat="server" CssClass="spanLabel h3">Production Notes</asp:Label>
				</td>
				<td style="height: 13px">
					<asp:TextBox ID="txtEditNotes" runat="server" CssClass="textEntry h3"></asp:TextBox>
				</td>
			</tr>
		</table>
	</div>
    <div id="divDlgMoveLot" title="Move" class="displayNones">
        <table class="tableCenter">
            <tr>
                <td>
                    <telerik:RadComboBox ID="ddlMoveBP" RenderMode="Lightweight" runat="server"
                        AutoPostBack="True"
                        DataSourceID="SqlDataSourceBroadcastPoints" 
                        DataTextField="Description" 
                        DataValueField="BroadcastPointID"
                        CssClass="selectDropDown selectDropDownBP"
                        Width="220px"
                        OnClientLoad="showSelectedItemImage"
                        OnClientSelectedIndexChanging="showImageOnSelectedItemChanging"
                        OnItemDataBound="RadComboBox1_ItemDataBound" 
                        >
                        <ExpandAnimation Type="OutBack" />
                        <CollapseAnimation Type="InBack" />
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle">
                    <asp:Panel ID="Panel5" runat="server" ScrollBars="Auto" CssClass="divBorder treePanelMove ">
                        <telerik:RadTreeView ID="treeMove" runat="server" RenderMode="Lightweight" 
                            BorderWidth="0px" 
                            EnableDragAndDrop="True"
                            EnableDragAndDropBetweenNodes="True"
                            Skin="Windows7"
                            CssClass="treeViewNode"
                            MultipleSelect="True" 
                            ShowLineImages="False" 
                            DataTextField = "TextField"
                            DataValueField = "ValueField"
                            DataFieldID = "ID"
                            DataFieldParentID = "ParentID"
                            DataSourceID="SqlDataSourceGetClosestLots" 
                            OnClientNodeDragging="treeMove_OnClientNodeDragging"
                            OnClientNodeDropping="treeMove_OnClientNodeDropping"
                            OnNodeDrop="treeMove_NodeDrop"
                            Height="380"
                            >
                        </telerik:RadTreeView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>


    <asp:SqlDataSource ID="SqlDataSourceBroadcastPoints" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>"
		ProviderName="System.Data.SqlClient" SelectCommand="procGetBroadcastPoints" SelectCommandType="StoredProcedure" >
	</asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceGetClosestLots" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringSQL %>"
		ProviderName="System.Data.SqlClient" SelectCommand="procPSGetClosestLots" SelectCommandType="StoredProcedure" 
        CancelSelectOnNullParameter="False">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidLotNum8" PropertyName="value" Name="LotNumber8" />
            <asp:ControlParameter ControlID="hidBroadcastPointID" PropertyName="value" Name="BroadcastPointID" />
			<asp:ControlParameter ControlID="rbOrders"  Name="SetexOrdersOnly" PropertyName="SelectedValue" Type="Int32" ConvertEmptyStringToNull="True" />
        </SelectParameters>
	</asp:SqlDataSource>

</asp:Content>
