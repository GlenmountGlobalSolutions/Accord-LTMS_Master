<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UserTypes.aspx.vb" Inherits="LTMS_Master.UserTypes" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">

	    function contentPageLoad(sender, args) {
	        AddDirtyClassOnChange();

	        CreateDialog_New();
	        CreateDialog_Delete();
	        CreateDialog_Rename();
	    }

	    function CreateDialog_New() {
	        var $cmdButton = $('#MainContent_cmdNew');
	        var $dlgDiv = $('#divDialogNew');

	        $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

	        $dlgDiv.modalDialog({
	            control: $cmdButton,
	            width: 360,
                validationFunction: function () { return checkLength($("#MainContent_txtUserType"), "User Type Name", 1, 50, $("#helpUserType")); }
	        });
	    }

	    function CreateDialog_Delete() {
	        var $cmdButton = $('#MainContent_cmdDelete');
	        var $dlgDiv = $('#divDialogDelete');

	        $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });
	        $dlgDiv.deleteDialog({ control: $cmdButton, width: 360 });
	    }

	    function CreateDialog_Rename() {
	        var $cmdButton = $('#MainContent_cmdRename');
	        var $dlgDiv = $('#divDialogRename');

	        $cmdButton.click(function () { $dlgDiv.dialog('open'); return false; });

	        $dlgDiv.modalDialog({
	            control: $cmdButton, 
                width: 360,
	            validationFunction: function () { return checkLength($("#MainContent_txtRenameTo"), "User Type Name", 1, 50, $("#helpRenameUserType")); }
	        });
	    }

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate >
    <div id="divMainLeftPanel">
        <asp:Label ID="Label4" runat="server" CssClass="h2 spanLabel">User Types:</asp:Label>
        <asp:ListBox ID="lbUserTypes" runat="server" AutoPostBack="True" CssClass="selectListBox"></asp:ListBox>
        <div id="cmdPanel">
            <asp:Button ID="cmdNew" runat="server" Text="New" ToolTip="Add New User Type"></asp:Button>
            <asp:Button ID="cmdDelete" runat="server" Text="Delete" ToolTip="Delete Current User Type"></asp:Button>
            <asp:Button ID="cmdRename" runat="server" Text="Rename" ToolTip="Rename Current User Type"></asp:Button>
        </div>
    </div>
</ContentTemplate>
</asp:UpdatePanel>
<div id="divDialogNew" title="New User Type">
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
<ContentTemplate >
	<p class="validationHints">	All form fields are required.</p>
	<table id="tblNewUserType" style="text-align: center; border-collapse: collapse; padding: 0;" >
		<tr>
			<td style="text-align: left">
				<asp:Label ID="Label1" runat="server" CssClass="spanLabel"> New User Type:</asp:Label>
			</td>
			<td style="text-align: left">
				<asp:TextBox ID="txtUserType" TabIndex="4" runat="server" CssClass="textEntry" Width="164px"></asp:TextBox>
			</td>
			<td>
				<div id="helpUserType" class="ui-state-default ui-corner-all">
					<span class="ui-icon ui-icon-help"></span>
				</div>
			</td>
		</tr>
		<tr>
			<td style="text-align: right;">
				<asp:Label ID="Label2" runat="server" CssClass="spanLabel">Copy Permissions From:</asp:Label>
			</td>
			<td style="text-align: left">
				<asp:DropDownList ID="ddlUserTypes" runat="server" CssClass="selectDropDown" Width="170px" />
			</td>
			<td>
			</td>
		</tr>
	</table>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div id="divDialogDelete" title="Delete User Type">
	<p>
		<span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 50px 0;"></span>
                <p class="pCenter" style="margin-left: 30px;">You are about to Delete the selected User Type.</br>Do you wish to continue?</span>
	</p>
</div>
<div id="divDialogRename" title="Rename User Type">
	<table id="tblRename">
		<tr>
			<td>
				<asp:Label ID="lblRename" runat="server" CssClass="spanLabel">New Name:</asp:Label>
			</td>
			<td>
				<asp:TextBox ID="txtRenameTo" runat="server" Width="180px" CssClass="textEntry"></asp:TextBox>
			</td>
			<td>
				<div id="helpRenameUserType" class="ui-state-default ui-corner-all">
					<span class="ui-icon ui-icon-help"></span>
				</div>
			</td>
		</tr>
	</table>
</div>
</asp:Content>
