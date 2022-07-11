<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="KDPlanProcessing.aspx.vb" Inherits="LTMS_Master.KDPlanProcessing" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var $treeView;
        var $cmdDelete;
        var $cmdMove;
        var $cmdProcess;
        var $cmdButton1;
        var $lbNfiles;
        var $lbFileContents;

        var $divDialogConfirmValidDisabled;
        var $divDialogProcess;
        var $divDialogConfirmValid;        
        var $dialogProcessAction;

        //Hidden controls for processing Plans
        var $dialogControl;
        var $captureConfirmFlag;
        var $captureValidFlag;
        var $captureProcessFlag;
        var $captureDeleteFlag;
        var $hidSelectedLot;

        function contentPageLoad(sender, args) {
           // ScrollSelectedLBItemIntoView();

            AddDirtyClassOnChange();
            CacheControls();
            AddUIClasses();

            RemoveTreeViewSkipLinks();
            AddEventToTreeView();

            CreateDialog_Delete();
            CreateDialog_Process();
            CreateDialog_ConfirmValid();
            CreateDialog_ConfirmValidDisabled();
            
            AddOverLay();

            if (args.get_isPartialLoad()) {
                //Specific code for partial postbacks can go in here.   code in here will only execute after a post back

                ScrollSelectedTreeviewItemIntoView();
                CheckForAlerts();
            }
        }

         function setFocus1(index)
         {
            var listbox = document.getElementById('<%= lbFileContent.ClientID %>');
            listbox.options[index].selected = true;
         }

        function CacheControls() {
            
            $treeView = $('#MainContent_TreeView');

            $cmdMove = $('#MainContent_cmdMove');
            $cmdDelete = $('#MainContent_cmdDelete');
            $cmdProcess = $('#MainContent_cmdProcess');
            $lbNfiles = $('#MainContent_lbNfiles');
           

            $lbFileContents = $('#MainContent_lbFileContent');

            $dialogControl = $('#MainContent_dialogControl');

            $captureConfirmFlag = $('#MainContent_captureConfirmFlag');
            $captureValidFlag = $('#MainContent_captureValidFlag');
            $captureProcessFlag = $('#MainContent_captureProcessFlag');
            $captureDeleteFlag = $('#MainContent_captureDeleteFlag');
            
            //Dialogs
            $divDialogProcess = $('#divDialogProcess');
            $divDialogConfirmValid = $('#divDialogConfirmValid');
            $divDialogConfirmValidDisabled = $('#divDialogConfirmValidDisabled');
            
            $dialogProcessAction = $('#MainContent_dialogProcessAction');
        }
        
        
       
        function CreateDialog_Delete() {
            var $dlgDiv = $('#divDialogDelete');

            // add event to button
            $cmdDelete.click(function () { $dlgDiv.dialog('open'); return false; });

            // Add Delete Dialog
            $dlgDiv.deleteDialog({ control: $cmdDelete });
        }

        function CreateDialog_Process() {
            // Add Dialog
            $divDialogProcess.modalDialog({ control: $cmdProcess, width: 600 });

            $divDialogProcess.dialog("option", "buttons", [
                            {
                                text: "Process",
                                click: function () {
                                    $dialogProcessAction.val("Process");
                                    //set value for cmdProcess postback to access.
                                    ShowWaitOverlay();

                                    $captureProcessFlag.val("True");
                                    __doPostBack($cmdProcess.prop('name'), '');
                                    $(this).dialog("close");
                                }
                            },
                            {
                                text: "Cancel",
                                click: function () {
                                    $dialogProcessAction.val("Cancel");
                                    //set value for cmdProcess postback to access.
                                    $captureProcessFlag.val("FALSE");
                                    __doPostBack($cmdProcess.prop('name'), '');
                                    $(this).dialog("close");
                                }
                            }
                    ]);
        }


        function CreateDialog_ConfirmValid() {
            // Add Dialog
            $divDialogConfirmValid.modalDialog({ control: $cmdProcess, width: 500, clearInputOnOpen: false });

            $divDialogConfirmValid.dialog("option", "buttons", [
                            {
                                text: "Process File",
                                click: function () {
                                    //set value for cmdProcess postback to access.
                                    ShowWaitOverlay();
                                    $captureValidFlag.val("True");
                                    __doPostBack($cmdProcess.prop('name'), '');
                                    $(this).dialog("close");
                                }
                            },
                            {
                                text: "Cancel",
                                click: function () {
                                    //set value for cmdProcess postback to access.
                                    $captureValidFlag.val("FALSE");
                                    __doPostBack($cmdProcess.prop('name'), '');
                                    $(this).dialog("close");
                                }
                            }
                    ]);
        }

        function CreateDialog_ConfirmValidDisabled() {
            // Add Dialog
            $divDialogConfirmValidDisabled.modalDialog({ control: $cmdProcess, width: 500, clearInputOnOpen: false });

            $divDialogConfirmValidDisabled.dialog("option", "buttons", [
                            {
                                text: "Cancel",
                                click: function () {
                                    //set value for cmdProcess postback to access.
                                    $captureValidFlag.val("FALSE");
                                    __doPostBack($cmdProcess.prop('name'), '');
                                    $(this).dialog("close");
                                }
                            }
                    ]);
        }

        function CheckForAlerts() {
            //  codebehind from cmdProcess.click will set the dialogControl.val to indicate
            //  which popup dialog needs to be processed.
            switch ($dialogControl.val()) {
                case "captureValidDisabled":
                    $divDialogConfirmValidDisabled.dialog('open');
                    break;

                case "captureProcess":
                    $divDialogProcess.dialog('open');
                    break;

                case "captureConfirmValid":
                    $divDialogConfirmValid.dialog('open');
                    break;

                case "captureValid":
                    $divDialogConfirmValid.dialog('open');
                    break;            
                default:
                    break;
            }

            $dialogControl.val("");
        }

        function AddEventToTreeView() {
            $treeView.click(function () { return GetSelectedNode(event, 'MainContent_treeView_Data'); });
        }

        function GetSelectedNode(evt, treeData) {
            try {
                var src = window.event != window.undefined ? window.event.srcElement : evt.target;
                var nodeClick = src.tagName.toLowerCase() == "a" || src.tagName.toLowerCase() == "span";
                if (nodeClick) {
                    ShowWaitOverlay(); 
                    return true;   //change to true if you want postback on node click
                }
            }
            catch (Error) {
                alert(Error);
            }
        }

        function AddOverLay() {
            //$lbNfiles.change(function () { ShowWaitOverlay(); });
            $(".cmdShowWait").on('click', function () { ShowWaitOverlay(); });
        }

        function ScrollSelectedTreeviewItemIntoView() {
            var name = MainContent_TreeView_Data.selectedNodeID.value;
            var selectedNode = $get(name);

            if (selectedNode) {
                selectedNode.scrollIntoView(true);
            }
        }
              
    </script>
    <style type="text/css">
        #PlanProperties
        {
            margin: 0px 0px 12px 0px;
            width: 700px;
        }
        .spanValue
        {
            font-weight: bold;
            width: 500px !important;
        }
        #divMainCenterPanel .spanLabel
        {
            /*Line up the spans to similate table columns.*/
            width: 150px;
            display: inline-block;
            margin-bottom: 4px;
        }
        #PlanProperties .selectListBox
        {
            vertical-align: text-top;
        }
        #PlanProperties .h2
        {
            /*push other labels to the next line.*/
            display: block;
            padding-bottom: 5px;
        }        
        #divMainLeftPanel
        {
            width: 340px;
        }
        #divMainLeftPanel select, #cmdPanel
        {
            width: 300px;
        }
        #cmdPanel input
        {
            width: 200px;
        }
        #MainContent_cmdProcess
        {
            white-space: normal;
        }
        .divBorder
        {
            overflow: auto;
        }
        #MainContent_TreeView
        {
            margin-top: 4px;
            padding-top: 4px;
        }
        #MainContent_TreeView a
        {
            padding-right: 4px;
        }
        #MonthSelection .selectListBox
        {
            vertical-align: text-top;
        }        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>                              
                <asp:Label ID="Label4" runat="server" CssClass="spanLabel h2">Files:</asp:Label>
                <asp:TreeView ID="TreeView" runat="server" ShowLines="False" ShowExpandCollapse="True" 
                    RootNodeStyle-HorizontalPadding="0" RootNodeStyle-VerticalPadding="0" ParentNodeStyle-HorizontalPadding="0"
                    ParentNodeStyle-VerticalPadding="0" LeafNodeStyle-HorizontalPadding="0" LeafNodeStyle-VerticalPadding="0" ExpandDepth="1" Height="400px" Width="310px"
                    BackColor="White" CssClass="divBorder">
                    <SelectedNodeStyle Font-Bold="True" />
                </asp:TreeView>
                <div id="cmdPanel">
                    <asp:Button ID="cmdProcess" runat="server" ToolTip="Process selected file." Text="Process Selected Plan" CssClass="cmdShowWait"></asp:Button>
                    <asp:Button ID="cmdMove" runat="server" ToolTip="Move selected file." Text="Move File" CssClass="cmdShowWait"></asp:Button>
                    <asp:Button ID="cmdDelete" runat="server" ToolTip="Delete selected file." Text="Delete File" CssClass="cmdShowWait"></asp:Button>    
                </div>
                <input id="captureConfirmFlag" type="hidden" name="captureConfirmFlag" runat="server" />
                <input id="captureValidFlag" type="hidden" name="captureValidFlag" runat="server" />
                <input id="captureProcessFlag" type="hidden" name="captureProcessFlag" runat="server" />
                <input id="captureDeleteFlag" type="hidden" name="captureDeleteFlag" runat="server" />
                <input id="dialogControl" type="hidden" name="dialogControl" runat="server" />                
                <!--    Store which action button was pressed.  -->
                <input id="dialogProcessAction" type="hidden" name="dialogProcessAction" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>                
                <div id="PlanProperties">
                    <asp:Label ID="Label3" runat="server" CssClass="h2">File Properties:</asp:Label>
                    <asp:Label ID="Label2" runat="server" CssClass="spanLabel">File Name:</asp:Label>
                    <asp:Label ID="lblFileName" runat="server" CssClass="spanLabel spanValue">asdf</asp:Label>
                    <asp:Label ID="Label20" runat="server" CssClass="spanLabel">Status:</asp:Label>
                    <asp:Label ID="lblStatus" runat="server" CssClass="spanLabel spanValue">asdf</asp:Label>
                    <asp:Label ID="lblSendDateTitle" runat="server" CssClass="spanLabel">Plan Send Date:</asp:Label>
                    <asp:Label ID="lblSendDate" runat="server" CssClass="spanLabel spanValue">asdf</asp:Label>
                    <asp:Label ID="lblNFiles" runat="server" CssClass="spanLabel">Plans Found in File: </asp:Label>
                    <asp:ListBox ID="lbNfiles" runat="server" Width="224px" AutoPostBack="True" CssClass="selectListBox"></asp:ListBox>                                                                            
                </div>                     
                <div id="FileContent">
                    <asp:Label ID="StartLot" runat="server" CssClass="spanLabel h2">Start Lot:</asp:Label>
                    <asp:Label ID="StartLotValue" runat="server" CssClass="spanLabel h2"></asp:Label>
                    <asp:Label ID="EndLot" runat="server" CssClass="spanLabel h2">End Lot:</asp:Label>  
                    <asp:Label ID="EndLotValue" runat="server" CssClass="spanLabel h2"></asp:Label>                            
                    <asp:Label ID="lblAsterick" runat="server" CssClass="spanLabel ui-state-highlight" Width="300px">test</asp:Label>
                    
                   <%-- <asp:ListBox ID="lbFileContent" runat="server" AutoPostBack="True" Width="664px" Height="300px" EnableViewState="true" OnSelectedIndexChanged="setFocus(this.value);" ReadOnly="False" SelectionMode="Multiple" Font-Names="Courier New" BackColor="White"></asp:ListBox> 
                   --%>     
                    <%--<asp:ListBox ID="lbFileContent" runat="server" AutoPostBack="True" Width="664px" Height="300px" OnSelectedIndexChanged="lbFileContent_SelectedIndexChanged1" CssClass="selectListBox" SelectionMode="Multiple"></asp:ListBox>--%>
                    <asp:ListBox ID="lbFileContent" runat="server" AutoPostBack="True" Width="664px" Height="300px" CssClass="selectListBox"  SelectionMode="Multiple" Font-Names="Courier New"></asp:ListBox>
                </div>
                <p></p> 
                <p></p><p></p>
                <div id="BatchIssues"> 
                    <asp:Label ID="lblIssues" runat="server" Width="660px" CssClass="spanLabel h2">Batch Issues:</asp:Label>                  
                    <asp:TextBox ID="tbIssues" runat="server" Width="664px" Height="66px" TextMode="MultiLine" ReadOnly="false" CssClass="textEntry"></asp:TextBox>
                </div>
                <asp:Panel ID="pnlProcessTime" runat="server" Visible="False">
                    <asp:Label ID="Label7" runat="server" CssClass="spanLabel">Process Duration (seconds):</asp:Label>
                    <asp:Label ID="lblProcTime" runat="server" CssClass="spanLabel spanValue">0:00</asp:Label>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>       
    </div>
    <div id="divDialogDelete" title="Delete Label Configuration">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            You are about to delete the selected file. Do you wish to continue?
        </p>
    </div>
    <div id="divDialogConfirmValidDisabled" title="Critical Validation Error">
        <!--    modalValidDisabled_.aspx    -->
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <p class="pCenter">
                    <span class="spanLabel h3">The following validation errors exist in the selected file.</span>
                </p>
                <p class="pCenter">
                    <asp:TextBox ID="tbConfirmValidDisabledMessage" runat="server" Width="420px" Height="100px" TextMode="MultiLine" ReadOnly="True" CssClass="textEntry"></asp:TextBox>
                </p>
                <div style="position: relative;">
                    <div class="ui-corner-all ui-state-default" style="top: 50%; position: absolute; margin-top: -9px; margin-left: 10px;">
                        <span class="ui-icon ui-icon-alert"></span>
                    </div>
                    <p class="pCenter h4" style="margin-left: 30px;">A critical validation error exists, this file cannot be processed.</p>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogProcess" title="Confirm Process File">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <p id="divDialogProcessMessage" class="pCenter h3" runat="server"></p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogConfirmValid" title="Non-critical Validation Errors">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <p class="pCenter">
                    <span class="spanLabel h3">The following validation errors exist in the selected file.</span>
                </p>
                <p class="pCenter">
                    <asp:TextBox ID="tbConfirmValidMessage" runat="server" Width="420px" Height="100px" TextMode="MultiLine" ReadOnly="True" CssClass="textEntry"></asp:TextBox>
                </p>
                <div style="position: relative;">
                    <div class="ui-corner-all ui-state-default" style="top: 50%; position: absolute; margin-top: -9px; margin-left: 100px;">
                        <span class="ui-icon ui-icon-alert"></span>
                    </div>
                    <p class="pCenter h4" style="margin-left: 30px;">
                        Do you wish to process the file?
                    </p>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
