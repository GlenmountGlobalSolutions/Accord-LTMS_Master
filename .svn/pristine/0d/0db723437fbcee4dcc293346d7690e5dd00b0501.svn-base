<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="UserAccounts.aspx.vb" Inherits="LTMS_Master.UserAccounts" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/systemAdministration.js") %>"></script>
    <script type="text/javascript">
        var $firstName;
        var $lastName;
        var $userType;
        var $loginName;
        var $password;
        var $loginLevel;
        var $badgeID;
        var $PLCUser;
        var $helpLogin;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            AddAJAXSettings();

            CacheControls();

            AddDialog_New();
            AddDialog_Delete();
        }

        function CacheControls() {
            $firstName = $("#MainContent_txtNewFirstName");
            $lastName = $("#MainContent_txtNewLastName");
            $userType = $("#MainContent_ddlNewUserType");
            $loginName = $("#MainContent_txtNewLogInName");
            $password = $("#MainContent_txtNewPassword");
            $loginLevel = $("#MainContent_txtNewLoginLevel");
            $badgeID = $("#MainContent_txtNewBadgeID");
            $PLCUser = $("#MainContent_ddlNewPLCUser");
            $helpLogin = $("#helpLogin");
        }

        function AddDialog_New() {
            var $cmdNew = $('#MainContent_cmdNew');
            var $dlgNew = $('#divDialogNew');

            $cmdNew.click(function () { $('#divDialogNew').dialog('open'); return false; });

            $dlgNew.modalDialog({
                control: $cmdNew,
                width: 400,
                validationFunction: function () { return ValidateDialog_New(); }
            });
        }

        function ValidateDialog_New() {

            var bValid = true;

            bValid = checkDropDownListByIndex($PLCUser, 1, "Please select a value for PLC User.", $("#helpPLCUser"));
            bValid = checkLength($badgeID, "Badge ID", 3, 50, $("#helpBadgeID")) && bValid;
            bValid = checkLength($loginLevel, "Login Level", 1, 2, $("#helpLoginLevel")) && bValid;
            bValid = checkLength($password, "Password", 3, 50, $("#helpPassword")) && bValid;
            bValid = checkLength($loginName, "Login Name", 3, 50, $helpLogin) && IsLoginAvailable() && bValid;
            bValid = checkDropDownListByIndex($userType, 1, "Please select a User Type.", $("#helpUserType")) && bValid;
            bValid = checkLength($lastName, "Last Name", 3, 50, $("#helpLastName")) && bValid;
            bValid = checkLength($firstName, "First Name", 3, 50, $("#helpFirstName")) && bValid;

            return bValid;
        }

        //  Uses an AJAX call to check database  -- DoesLogInNameExist found in the ContextMenu.js (loaded by Site.Master)
        function IsLoginAvailable() {
            var returnResult = DoesLogInNameExist($loginName.val());


            if (returnResult) {
                setValidationHint('Login Name already in use.');
                $helpLogin.removeClass("ui-helper-hidden");
                $loginName.addClass("ui-state-error");

                returnResult = false;
            }
            else {
                $helpLogin.addClass("ui-helper-hidden");
                returnResult = true;
            }
            return returnResult;
        };

        function AddDialog_Delete() {
            var $cmdDelete = $('#MainContent_cmdDelete');
            var $dlgDelete = $('#divDialogDelete');

            // Add Delete User Dialog Click Action
            $cmdDelete.click(function () { $dlgDelete.dialog('open'); return false; });

            $dlgDelete.deleteDialog({
                control: $cmdDelete,
                width: 360
            });
        }

    </script>
    <style type="text/css">
        #divMainLeftPanel
        {
            text-align: left;
            width: 160px;
        }
        #divMainLeftPanel ul
        {
            list-style: none;
            margin: 0px;
            padding: 0px;
        }
        #divMainLeftPanel li
        {
            margin-bottom: 6px;
        }
        
        #cmdPanel
        {
            text-align: center;
            width: 150px;
        }
        
        #cmdPanel ul
        {
            list-style: none;
            margin: 0px;
            padding: 0px;
        }
        
        #cmdPanel li
        {
            margin: 0px;
        }
        
        #divMainCenterPanel td
        {
            vertical-align: middle;
        }
        
        #divMainCenterPanel span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <ul>
                    <li>
                        <asp:Label ID="Label11" runat="server" CssClass="spanLabel h2">Users:</asp:Label>
                    </li>
                    <li>
                        <asp:ListBox ID="lbUsers" runat="server" AutoPostBack="True" Height="305px" Width="150px"
                            CssClass="selectListBox h3"></asp:ListBox>
                    </li>
                    <li>
                        <div id="cmdPanel">
                            <ul>
                                <li>
                                    <asp:Button ID="cmdNew" runat="server" CssClass="inputButton" ToolTip="New User"
                                        Text="New"></asp:Button>
                                </li>
                                <li>
                                    <asp:Button ID="cmdSave" runat="server" CssClass="inputButton" ToolTip="Save User"
                                        Text="Save"></asp:Button>
                                </li>
                                <li>
                                    <asp:Button ID="cmdDelete" runat="server" CssClass="inputButton" ToolTip="Delete User"
                                        Text="Delete"></asp:Button>
                                </li>
                            </ul>
                        </div>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table id="tblUserAccounts">
                    <tr>
                        <td style="width: 200px;">
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label1" runat="server" CssClass="spanLabel h3">First Name:</asp:Label>
                        </td>
                        <td style="width: 150px; text-align: left;">
                            <asp:TextBox ID="txtFName" runat="server" CssClass='textEntry'></asp:TextBox>
                        </td>
                        <td style="width: 20px">
                            <asp:Image ID="imgFirstNameCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label2" runat="server" CssClass="spanLabel h3">Last Name</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLName" runat="server" CssClass='textEntry'></asp:TextBox>
                        </td>
                        <td>
                            <asp:Image ID="imgLastNameCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label6" runat="server" CssClass="spanLabel h3">Login Name:</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="txtLogInName" runat="server" CssClass='textEntry'></GGS:WebInputBox>
                        </td>
                        <td>
                            <asp:Image ID="imgLoginNameQuestion" runat="server" Height="18px" Width="16px" ImageUrl="~/Images/question_mark.gif"
                                Visible="False"></asp:Image>
                            <asp:Image ID="imgLoginNameCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label7" runat="server" CssClass="spanLabel h3">Login Level:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLoginLevel" runat="server" CssClass='textEntry'></asp:TextBox>
                        </td>
                        <td>
                            <asp:Image ID="imgLoginLevelQuestion" runat="server" Height="18px" Width="16px" ImageUrl="~/Images/question_mark.gif"
                                Visible="False"></asp:Image>
                            <asp:Image ID="imgLoginLevelCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label8" runat="server" CssClass="spanLabel h3">Badge ID:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBadgeID" runat="server" CssClass='textEntry'></asp:TextBox>
                        </td>
                        <td>
                            <asp:Image ID="imgBadgeIDQuestion" runat="server" Height="18px" Width="16px" ImageUrl="~/Images/question_mark.gif"
                                Visible="False"></asp:Image>
                            <asp:Image ID="imgBadgeIDCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label3" runat="server" CssClass="spanLabel h3">Password (hidden):</asp:Label>
                        </td>
                        <td>
                            <GGS:WebInputBox ID="txtPassword" runat="server" Style="width: 150px;
                                text-align: left;" TextMode="Password" CssClass='textEntry'></GGS:WebInputBox>
                        </td>
                        <td>
                            <asp:Image ID="imgPasswordQuestion" runat="server" Height="18px" Width="16px" ImageUrl="~/Images/question_mark.gif"
                                Visible="False"></asp:Image>
                            <asp:Image ID="imgPasswordCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label4" runat="server" CssClass="spanLabel h3">User Type:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUserType" runat="server" Width="100%" CssClass="selectDropDown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Image ID="imgUserTypeCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="LABEL12" runat="server" CssClass="spanLabel h3">Station Bypass Enabled?</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStationByPassEnabled" runat="server" Width="156px" CssClass="selectDropDown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Image ID="imgStationBypassCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="LABEL13" runat="server" CssClass="spanLabel h3">Barcode ID:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBarCodeID" runat="server" CssClass='textEntry'></asp:TextBox>
                        </td>
                        <td>
                            <asp:Image ID="imgBarcodeIDCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="LABEL19" runat="server" CssClass="spanLabel h3">PLC User?</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPLCUser" runat="server" Width="156px" CssClass="selectDropDown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Image ID="imgPLCUserCheck" runat="server" Height="17px" Width="19px" ImageUrl="~/Images/check.gif"
                                EnableViewState="False" Visible="False"></asp:Image>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label5" runat="server" CssClass="spanLabel h3">Modified Date:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ModifiedDT" runat="server" CssClass='textEntry' ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="ui-icon ui-icon-triangle-1-e"></span>
                            <asp:Label ID="Label9" runat="server" CssClass="spanLabel h3">Modified By:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ChagedByUser" runat="server" CssClass='textEntry' ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogNew" title="Add New User" style="text-align: center;">
        <p class="validationHints">
            All form fields are required.</p>
        <table id="Table1" style="text-align: left;">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td style="width: 120px;">
                    <asp:Label ID="Label14" runat="server" CssClass="spanLabel h3">First Name:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewFirstName" runat="server" Width="156px" CssClass='textEntry'></asp:TextBox>
                </td>
                <td>
                    <div id="helpFirstName" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label15" runat="server" CssClass="spanLabel h3">Last Name:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewLastName" runat="server" Width="156px" CssClass='textEntry'></asp:TextBox>
                </td>
                <td>
                    <div id="helpLastName" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label16" runat="server" CssClass="spanLabel h3">User Type:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNewUserType" runat="server" Width="161px" CssClass='selectDropDown'>
                    </asp:DropDownList>
                </td>
                <td>
                    <div id="helpUserType" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label17" runat="server" CssClass="spanLabel h3">Login Name:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewLogInName" runat="server" Width="156px" CssClass='textEntry'></asp:TextBox>
                </td>
                <td>
                    <div id="helpLogin" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label18" runat="server" CssClass="spanLabel h3">Password:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" Width="156px" TextMode="Password" CssClass='textEntry'></asp:TextBox>
                </td>
                <td>
                    <div id="helpPassword" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label10" runat="server" CssClass="spanLabel h3">Login Level:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewLoginLevel" runat="server" Width="156px" CssClass='textEntry'></asp:TextBox>
                </td>
                <td>
                    <div id="helpLoginLevel" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label20" runat="server" CssClass="spanLabel h3">Badge ID:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewBadgeID" runat="server" Width="156px" CssClass='textEntry'></asp:TextBox>
                </td>
                <td>
                    <div id="helpBadgeID" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="Label21" runat="server" CssClass="spanLabel h3">PLC User?:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNewPLCUser" runat="server" Width="161px" CssClass='selectDropDown'>
                    </asp:DropDownList>
                </td>
                <td>
                    <div id="helpPLCUser" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete User">
        <p class="pCenter h3">
            <span class="ui-icon ui-icon-alert" style="float: left;"></span><span class="spanLabel">
                You are about to Delete the selected User.<br />
                <br />
                Do you wish to continue? </span>
        </p>
    </div>
</asp:Content>
