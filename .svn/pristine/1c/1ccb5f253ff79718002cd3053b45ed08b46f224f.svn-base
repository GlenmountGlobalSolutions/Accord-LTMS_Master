<%@ Page Title="" Language="vb" AutoEventWireup="false"  MasterPageFile="~/Site.Master"  CodeBehind="ReworkMaterialDispositionReports.aspx.vb" Inherits="LTMS_Master.ReworkMaterialDispositionReports"  %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemOperations.js") %>"></script>
<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/date.js") %>"></script>
<script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.maskedinput-1.3.1.min.js") %>"></script>
<script type="text/javascript">
    

    function contentPageLoad(sender, args) {

        if (!args.get_isPartialLoad()) {
                  MakeDateTimePicker();
        }

    }

    function MakeDateTimePicker() {
            $("input[id*='MainContent_tbDay']").mask("99/99/9999").addDatePicker()
			.datepicker("option", "firstDay", 1);
             
        	// position the calendar icon
			PositionCalendarPickerIcon();
		}
</script>

 <style type="text/css">
        #div1_Left ul, #div2_LeftCenter ul, #div3_RightCenter ul, #div4_Right ul
        {
            list-style: none;
            margin: 0px;
            padding: 0px;
        }
        #div1_Left li, #div2_LeftCenter li, #div3_RightCenter li, #div4_Right li
        {
            margin-bottom: 4px;
        }
        
        #div4_Right li
        {
            float: left;
            text-align: left;
            margin-left: 6px;
        }
        
        #div1_Left, #div2_LeftCenter, #div3_RightCenter, #div4_Right
        {
            float: left;
            height: 460px;
            text-align: left;
            width: 850px;
        }
        
        #div4_Right span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        #centerListBox
        {
            width: 246px;
            margin: 0 auto;
            text-align: left;
        }
        .selectCheckBox
        {
            text-align: left;
        }
    </style>			
             
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id='div1_Left'>
			<table id="table1"  cellspacing="3" cellpadding="7" border="0">
				<tr>
					<td><asp:label id="Label1" runat="server" CssClass="spanLabel">Report Type</asp:label></td>
					<td><asp:radiobuttonlist id="rblType" runat="server" repeatdirection="Horizontal" repeatlayout="Flow" autopostback="True" CssClass="selectCheckBox">
							<asp:listitem value="tblRMDReworkHistory" selected="True">Rework</asp:listitem>
							<asp:listitem value="tblRMDMaterialDispositionHistory">Material Disposition</asp:listitem>
						</asp:radiobuttonlist></td>
				</tr>
				<tr>
					<td><asp:label id="label7" runat="server" CssClass="spanLabel">Beginning Date:&nbsp;</asp:label></td>
					<td>
						<asp:imagebutton id="ibPrevBeg" tabindex="1" runat="server" imageurl="~/images/Arrows/bbPrevpage.gif"></asp:imagebutton>&nbsp;
						<asp:textbox id="tbDayBeg" tabindex="2" runat="server" width="96px" enabled="True" CssClass="textEntry h4"></asp:textbox>&nbsp;
						<asp:imagebutton id="ibNextBeg" tabindex="3" runat="server" imageurl="~/images/Arrows/bbNextpage.gif"></asp:imagebutton>&nbsp;
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:label id="label2" runat="server" CssClass="spanLabel">Ending Date:&nbsp;</asp:label>
						<asp:imagebutton id="ibPrevEnd" tabindex="1" runat="server" imageurl="~/images/Arrows/bbPrevpage.gif"></asp:imagebutton>&nbsp;
						<asp:textbox id="tbDayEnd" tabindex="2" runat="server" width="96px" enabled="true" CssClass="textEntry h4"></asp:textbox>&nbsp;
						<asp:imagebutton id="ibNextEnd" tabindex="3" runat="server" imageurl="~/images/Arrows/bbNextpage.gif"></asp:imagebutton>&nbsp;
						&nbsp;&nbsp;&nbsp;
					</td>
				</tr>
				<tr>
					<td><asp:label id="label3" runat="server" CssClass="spanLabel">Shift(s) to Include:&nbsp;</asp:label></td>
					<td><asp:radiobuttonlist id="rblShift" tabindex="5" runat="server" repeatdirection="Horizontal" repeatlayout="Flow" CssClass="selectCheckBox">
							<asp:listitem value="1">1st</asp:listitem>
							<asp:listitem value="2">2nd</asp:listitem>
							<asp:listitem value="3">3rd</asp:listitem>
							<asp:listitem value="ALL" selected="True">All</asp:listitem>
						</asp:radiobuttonlist></td>
				</tr>
				<tr>
					<td><asp:label id="label4" runat="server" CssClass="spanLabel">Areas to Include:&nbsp;</asp:label></td>
					<td><asp:checkboxlist id="cblAreas" runat="server" repeatcolumns="3"  CssClass="selectCheckBox h4"></asp:checkboxlist></td>
				</tr>
				<tr>
					<td><asp:label id="label5" runat="server" CssClass="spanLabel">Sort Order:&nbsp;</asp:label></td>
					<td>
						<table id="table2" cellspacing="0" cellpadding="3" border="1">
							<tr>
								<td><asp:Label id="label6" runat="server" CssClass="spanLabel">Sort Order:&nbsp;</asp:Label></td>
								<td><asp:dropdownlist id="ddlSortParam1" runat="server" CssClass="selectDropDown h3"></asp:dropdownlist></td>
								<td><asp:radiobuttonlist id="rblSort1" runat="server" repeatdirection="Horizontal" repeatlayout="Flow"  CssClass="selectCheckBox">
										<asp:listitem value="ASC" selected="True">Ascending</asp:listitem>
										<asp:listitem value="DESC">Descending</asp:listitem>
									</asp:radiobuttonlist></td>
							</tr>
							<tr>
								<td><asp:label id="label8" runat="server" CssClass="spanLabel">Sort Order:&nbsp;</asp:label></td>
								<td><asp:dropdownlist id="ddlSortParam2" runat="server" CssClass="selectDropDown h3"></asp:dropdownlist></td>
								<td><asp:radiobuttonlist id="rblSort2" runat="server" repeatdirection="Horizontal" repeatlayout="Flow"  CssClass="selectCheckBox">
										<asp:listitem value="ASC" selected="True">Ascending</asp:listitem>
										<asp:listitem value="DESC">Descending</asp:listitem>
									</asp:radiobuttonlist></td>
							</tr>
							<tr>
								<td><asp:label id="label9" runat="server" CssClass="spanLabel">Sort Order:&nbsp;</asp:label></td>
								<td><asp:dropdownlist id="ddlSortParam3" runat="server" CssClass="selectDropDown h3"></asp:dropdownlist></td>
								<td><asp:radiobuttonlist id="rblSort3" runat="server" repeatdirection="Horizontal" repeatlayout="Flow"  CssClass="selectCheckBox">
										<asp:listitem value="ASC" selected="True">Ascending</asp:listitem>
										<asp:listitem value="DESC">Descending</asp:listitem>
									</asp:radiobuttonlist></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td></td>
					<td></td>
				</tr>
				<tr>
					<td> <asp:Button ID="cmdCreateFile" runat="server" Text="Create File" Width="106px" CssClass="inputButton" /></td>
					<td></td>
				</tr>
			</table>
      </div>
</asp:Content>


