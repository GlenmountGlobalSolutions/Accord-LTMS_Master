<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ApplicationConfiguration.aspx.vb" Inherits="LTMS_Master.ApplicationConfiguration"
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemConfiguration.js") %>"></script>
    <script type="text/javascript">
        function contentPageLoad(sender, args) {

            AddDirtyClassOnChange();
            ClearWaitOverlay();
        }
    </script>
    <style type="text/css">
        #divMainContent span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        #divMainLeftPanel
        {
            width: 250px;
        }
        #divMainLeftPanel select
        {
            width: 240px;
        }
        #cmdPanel
        {
            width: 240px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label4" runat="server" CssClass="spanLabel h2">Applications:</asp:Label>
                <asp:ListBox ID="lbApps" runat="server" Width="240px" Height="350px" AutoPostBack="True" CssClass="selectListBox">
                </asp:ListBox>
                <div id="cmdPanel">
                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save Application Parameters"
                        Text="Save"></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:DataList ID="dlApps" runat="server">
                    <HeaderTemplate>
                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="dllblDesc" runat="server" Width="300px" CssClass="spanLabel">Application Name</asp:Label>
                        <GGS:WebInputBox ID="txtDescription" Text='<%# Description(Container) %>' OnTextChanged="Description_OnTextChaged"
                            runat="server" Width="300px" CssClass="textEntry"></GGS:WebInputBox>
                        <asp:Image ID="Image2" runat="server" Visible="False" ImageUrl="~/Images/check.gif"
                            Width="20px" Height="18px"></asp:Image>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span class="ui-icon ui-icon-triangle-1-e"></span>
                        <asp:Label ID="Label1" runat="server" CssClass="spanLabel" Width="300px" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                        <GGS:WebDropDownList ID="WebDropDownList" runat="server" CssClass="selectDropDown"
                            Width="307px" OnSelectedIndexChanged="dbAppParam_IndexChanged">
                        </GGS:WebDropDownList>
                        <GGS:WebInputBox ID="Webinputbox2" Text='<%# Container.DataItem("ApplicationParameterValue")%>'
                            OnTextChanged="ParameterValueTextChanged" runat="server" Width="300px" CssClass="textEntry"></GGS:WebInputBox>
                        <asp:Image ID="Image1" runat="server" Visible="False" ImageUrl="~/Images/check.gif"
                            Width="20px" Height="18px"></asp:Image>
                        <%#ObjectVisibilityInTemplate(Container)%>
                    </ItemTemplate>
                </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
