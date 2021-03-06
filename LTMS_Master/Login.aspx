<%@ Page Title="Home Page" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
	CodeBehind="Login.aspx.vb" Inherits="LTMS_Master.Login" %>
<%@ MasterType VirtualPath="~/Site.master" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		function contentPageLoad(sender, args) {
		    $("#MainContent_txtUserName").focus().select();
		    $("#MainContent_btnLogin").click(function () { ShowCursor_Wait(); });
		}
	</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
	<div>
		<table id="tblDefault" style="border-collapse: collapse; border-spacing: 0px; text-align: center; width: 100%; height: 100%;">
			<tr>
				<td style="height: 35px;">
				</td>
			</tr>
			<tr>
				<td style="text-align: center; vertical-align: top; height:250px;">
					<img src="Images/home.gif" alt="" style="width: 350px; height: 218px">
				</td>
			</tr>
			<tr>
				<td style="vertical-align: top;">    
					<table id="Table1" class="tableCenter">
						<tr>
							<td>
								<span class="ui-icon ui-icon-triangle-1-e"></span>
							</td>
							<td style="width: 70px; text-align: left;">
								<span class="spanLabel">Username:</span>
							</td>
							<td style="text-align:left;">
								<asp:TextBox ID="txtUserName" runat="server" Width="100" MaxLength="50" CssClass="textEntry" TabIndex="1"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								<span class="ui-icon ui-icon-triangle-1-e"></span>
							</td>
							<td style="width: 70px; text-align: left;">
								<span class="spanLabel">Password:</span>
							</td>
							<td style="text-align:left;">
								<asp:TextBox ID="txtPassword" TabIndex="2" runat="server" Width="100" MaxLength="50" TextMode="Password"  CssClass="passwordEntry"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td colspan="3" style="text-align: center; height: 40px;">
								<asp:Button ID="btnLogin" TabIndex="3" runat="server" Height="25" Width="120" CssClass="inputButton" Text="Login" ></asp:Button>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td colspan="3" style="height:48px;"> </td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="lblLoginInfo" Width="100%" CssClass="spanLabel " runat="server"></asp:Label>
				</td>
			</tr>
		</table>
	</div>

</asp:Content>
