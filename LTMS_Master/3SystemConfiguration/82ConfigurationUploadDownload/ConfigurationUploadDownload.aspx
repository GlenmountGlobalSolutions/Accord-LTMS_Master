<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ConfigurationUploadDownload.aspx.vb" Inherits="LTMS_Master.ConfigurationUploadDownload" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var varTimer;

        var $cmdRefresh;
        var $divDialogConfirm;
        var $dlgDownloadWarning;
        var $dlgUploadWarning;
        var $ddlLineNumber;

        //Build
        var $pbDnldAux;
        var $pbUpldAux;
        var $pbDnldCompScan;
        var $pbUpldCompScan;
        var $pbDnldCompName;
        var $pbUpldCompName;
        var $pbDnldTool;
        var $pbUpldTool;
        var $pbDnldStation;
        var $pbUpldStation;
        var $pbDnldProdCode;
        var $pbUpldProdCode;
        var $pbDnldUser;
        var $pbUpldUser;

        var $hidpbDnldAux;
        var $hidpbUpldAux;
        var $hidpbDnldCompScan;
        var $hidpbUpldCompScan;
        var $hidpbDnldCompName;
        var $hidpbUpldCompName;
        var $hidpbDnldTool;
        var $hidpbUpldTool;
        var $hidpbDnldStation;
        var $hidpbUpldStation;
        var $hidpbDnldProdCode;
        var $hidpbUpldProdCode;
        var $hidpbDnldUser;
        var $hidpbUpldUser;
                
        var $hidInitpbDnldAux;
        var $hidInitpbUpldAux;
        var $hidInitpbDnldCompScan;
        var $hidInitpbUpldCompScan;
        var $hidInitpbDnldCompName;
        var $hidInitpbUpldCompName;
        var $hidInitpbDnldTool;
        var $hidInitpbUpldTool;
        var $hidInitpbDnldStation;
        var $hidInitpbUpldStation;
        var $hidInitpbDnldProdCode;
        var $hidInitpbUpldProdCode;
        var $hidInitpbDnldUser;
        var $hidInitpbUpldUser;

        //Quality
        var $pbUpldQualAux;
        var $pbDnldQualAux;
        var $pbDnldQualCompScan;
        var $pbUpldQualCompScan;
        var $pbDnldQualCompName;
        var $pbUpldQualCompName;
        var $pbUpldQualTool;
        var $pbDnldQualTool;
        var $pbDnldQualStation;
        var $pbUpldQualStation;
        var $pbDnldQualProdCode;
        var $pbUpldQualProdCode;
        var $pbDnldQualUser;
        var $pbUpldQualUser;

        var $hidpbDnldQualAux;
        var $hidpbUpldQualAux;
        var $hidpbDnldQualCompScan;
        var $hidpbUpldQualCompScan;

        var $hidpbDnldQualCompName;
        var $hidpbUpldQualCompName;
        var $hidpbDnldQualTool;
        var $hidpbUpldQualTool;
        var $hidpbDnldQualStation;
        var $hidpbUpldQualStation;
        var $hidpbDnldQualProdCode;
        var $hidpbUpldQualProdCode;
        var $hidpbDnldQualUser;
        var $hidpbUpldQualUser;

        var $hidInitpbDnldQualAux;
        var $hidInitpbUpldQualAux;
        var $hidInitpbDnldQualCompScan;
        var $hidInitpbUpldQualCompScan;
        var $hidInitpbDnldQualCompName;
        var $hidInitpbUpldQualCompName;
        var $hidInitpbDnldQualTool;
        var $hidInitpbUpldQualTool;
        var $hidInitpbDnldQualStation;
        var $hidInitpbUpldQualStation;
        var $hidInitpbDnldQualProdCode;
        var $hidInitpbUpldQualProdCode;
        var $hidInitpbDnldQualUser;
        var $hidInitpbUpldQualUser;

        var $cmdDnldQualAux;

        function contentPageLoad(sender, args) {
            CacheGenControls();
            CachePBControls();
           
            if (args.get_isPartialLoad()) {

                CreateProgressBars();
            }
            else {
                //
            }

            if ($cmdDnldQualAux.is(":visible")) { $(".qualityColumn").removeClass("qualityColumn"); }

            CreateDialog_ConfirmDownload();
            CreateDialog_ConfirmUpload();
            CreateDialog_Confirm();
            AddDropDownAndButtonEvents();
            AddEventToControlsWithClass_downloadButton();
            AddEventToControlsWithClass_uploadButton();
        }

        function CacheGenControls() {
            $cmdRefresh = $('#MainContent_cmdRefresh');
            $ddlLineNumber = $('#MainContent_ddLineNumber');
        }

        function CachePBControls() {

            //Build
            $pbUpldAux = $('#pbUpldAux');
            $pbDnldAux = $('#pbDnldAux');
            $pbDnldCompScan = $('#pbDnldCompScan');
            $pbUpldCompScan = $('#pbUpldCompScan');
            $pbDnldCompName = $('#pbDnldCompName');
            $pbUpldCompName = $('#pbUpldCompName');
            $pbUpldTool = $('#pbUpldTool');
            $pbDnldTool = $('#pbDnldTool');
            $pbDnldStation = $('#pbDnldStation');
            $pbUpldStation = $('#pbUpldStation');
            $pbDnldProdCode = $('#pbDnldProdCode');
            $pbUpldProdCode = $('#pbUpldProdCode');
            $pbDnldUser = $('#pbDnldUser');
            $pbUpldUser = $('#pbUpldUser');

            $hidpbDnldAux = $('#MainContent_hidpbDnldAux');
            $hidpbUpldAux = $('#MainContent_hidpbUpldAux');
            $hidpbDnldCompScan = $('#MainContent_hidpbDnldCompScan');
            $hidpbUpldCompScan = $('#MainContent_hidpbUpldCompScan');
            $hidpbDnldCompName = $('#MainContent_hidpbDnldCompName');
            $hidpbUpldCompName = $('#MainContent_hidpbUpldCompName');
            $hidpbDnldTool = $('#MainContent_hidpbDnldTool');
            $hidpbUpldTool = $('#MainContent_hidpbUpldTool');
            $hidpbDnldStation = $('#MainContent_hidpbDnldStation');
            $hidpbUpldStation = $('#MainContent_hidpbUpldStation');
            $hidpbDnldProdCode = $('#MainContent_hidpbDnldProdCode');
            $hidpbUpldProdCode = $('#MainContent_hidpbUpldProdCode');
            $hidpbDnldUser = $('#MainContent_hidpbDnldUser');
            $hidpbUpldUser = $('#MainContent_hidpbUpldUser');

            $hidInitpbDnldAux = $('#MainContent_hidInitpbDnldAux');
            $hidInitpbUpldAux = $('#MainContent_hidInitpbUpldAux');
            $hidInitpbDnldCompScan = $('#MainContent_hidInitpbDnldCompScan');
            $hidInitpbUpldCompScan = $('#MainContent_hidInitpbUpldCompScan');
            $hidInitpbDnldCompName = $('#MainContent_hidInitpbDnldCompName');
            $hidInitpbUpldCompName = $('#MainContent_hidInitpbUpldCompName');
            $hidInitpbDnldTool = $('#MainContent_hidInitpbDnldTool');
            $hidInitpbUpldTool = $('#MainContent_hidInitpbUpldTool');
            $hidInitpbDnldStation = $('#MainContent_hidInitpbDnldStation');
            $hidInitpbUpldStation = $('#MainContent_hidInitpbUpldStation');
            $hidInitpbDnldProdCode = $('#MainContent_hidInitpbDnldProdCode');
            $hidInitpbUpldProdCode = $('#MainContent_hidInitpbUpldProdCode');
            $hidInitpbDnldUser = $('#MainContent_hidInitpbDnldUser');
            $hidInitpbUpldUser = $('#MainContent_hidInitpbUpldUser');

            //Quality
            $pbUpldQualAux = $('#pbUpldQualAux');
            $pbDnldQualAux = $('#pbDnldQualAux');
            $pbDnldQualCompScan = $('#pbDnldQualCompScan');
            $pbUpldQualCompScan = $('#pbUpldQualCompScan');
            $pbDnldQualCompName = $('#pbDnldQualCompName');
            $pbUpldQualCompName = $('#pbUpldQualCompName');
            $pbUpldQualTool = $('#pbUpldQualTool');
            $pbDnldQualTool = $('#pbDnldQualTool');
            $pbDnldQualStation = $('#pbDnldQualStation');
            $pbUpldQualStation = $('#pbUpldQualStation');
            $pbDnldQualProdCode = $('#pbDnldQualProdCode');
            $pbUpldQualProdCode = $('#pbUpldQualProdCode');
            $pbDnldQualUser = $('#pbDnldQualUser');
            $pbUpldQualUser = $('#pbUpldQualUser');

            $hidpbDnldQualAux = $('#MainContent_hidpbDnldQualAux');
            $hidpbUpldQualAux = $('#MainContent_hidpbUpldQualAux');
            $hidpbDnldQualCompScan = $('#MainContent_hidpbDnldQualCompScan');
            $hidpbUpldQualCompScan = $('#MainContent_hidpbUpldQualCompScan');
            $hidpbDnldQualCompName = $('#MainContent_hidpbDnldQualCompName');
            $hidpbUpldQualCompName = $('#MainContent_hidpbUpldQualCompName');
            $hidpbDnldQualTool = $('#MainContent_hidpbDnldQualTool');
            $hidpbUpldQualTool = $('#MainContent_hidpbUpldQualTool');
            $hidpbDnldQualStation = $('#MainContent_hidpbDnldQualStation');
            $hidpbUpldQualStation = $('#MainContent_hidpbUpldQualStation');
            $hidpbDnldQualProdCode = $('#MainContent_hidpbDnldQualProdCode');
            $hidpbUpldQualProdCode = $('#MainContent_hidpbUpldQualProdCode');
            $hidpbDnldQualUser = $('#MainContent_hidpbDnldQualUser');
            $hidpbUpldQualUser = $('#MainContent_hidpbUpldQualUser');

            $hidInitpbDnldQualAux = $('#MainContent_hidInitpbDnldQualAux');
            $hidInitpbUpldQualAux = $('#MainContent_hidInitpbUpldQualAux');
            $hidInitpbDnldQualCompScan = $('#MainContent_hidInitpbDnldQualCompScan');
            $hidInitpbUpldQualCompScan = $('#MainContent_hidInitpbUpldQualCompScan');
            $hidInitpbDnldQualCompName = $('#MainContent_hidInitpbDnldQualCompName');
            $hidInitpbUpldQualCompName = $('#MainContent_hidInitpbUpldQualCompName');
            $hidInitpbDnldQualTool = $('#MainContent_hidInitpbDnldQualTool');
            $hidInitpbUpldQualTool = $('#MainContent_hidInitpbUpldQualTool');
            $hidInitpbDnldQualStation = $('#MainContent_hidInitpbDnldQualStation');
            $hidInitpbUpldQualStation = $('#MainContent_hidInitpbUpldQualStation');
            $hidInitpbDnldQualProdCode = $('#MainContent_hidInitpbDnldQualProdCode');
            $hidInitpbUpldQualProdCode = $('#MainContent_hidInitpbUpldQualProdCode');
            $hidInitpbDnldQualUser = $('#MainContent_hidInitpbDnldQualUser');
            $hidInitpbUpldQualUser = $('#MainContent_hidInitpbUpldQualUser');

            $cmdDnldQualAux = $('#MainContent_cmdDnldQualAux'); 
        }

        function DoRefreshPostBack() {
            ShowCursor_Wait();
            __doPostBack($cmdRefresh.attr("name"), '');
        }

        function AddDropDownAndButtonEvents() {
            $cmdRefresh.click(function () {
                ShowWaitOverlay();
            });
        }

        function updatePageData() {
            varTimer = setInterval(function () { DoRefreshPostBack(); }, 5000);
        }

        function CreateProgressBarIfNeeeded(testValue, inProgress, progressBar) {
            testValue = parseFloat(testValue);
            if ((testValue > 0 && testValue <= 99) || (inProgress == 1)) {
                progressBar.progressbar({ value: testValue });
            }
        }

        function CreateProgressBars() {
            //Build
            CreateProgressBarIfNeeeded($hidpbDnldAux.val(), $hidInitpbDnldAux.val(), $pbDnldAux);
            CreateProgressBarIfNeeeded($hidpbUpldAux.val(), $hidInitpbUpldAux.val(), $pbUpldAux);

            CreateProgressBarIfNeeeded($hidpbDnldCompScan.val(), $hidInitpbDnldCompScan.val(), $pbDnldCompScan);
            CreateProgressBarIfNeeeded($hidpbUpldCompScan.val(), $hidInitpbUpldCompScan.val(), $pbUpldCompScan);

            CreateProgressBarIfNeeeded($hidpbDnldCompName.val(), $hidInitpbDnldCompName.val(), $pbDnldCompName);
            CreateProgressBarIfNeeeded($hidpbUpldCompName.val(), $hidInitpbUpldCompName.val(), $pbUpldCompName);

            CreateProgressBarIfNeeeded($hidpbDnldTool.val(), $hidInitpbDnldTool.val(), $pbDnldTool);
            CreateProgressBarIfNeeeded($hidpbUpldTool.val(), $hidInitpbUpldTool.val(), $pbUpldTool);

            CreateProgressBarIfNeeeded($hidpbDnldStation.val(), $hidInitpbDnldStation.val(), $pbDnldStation);
            CreateProgressBarIfNeeeded($hidpbUpldStation.val(), $hidInitpbUpldStation.val(), $pbUpldStation);

            CreateProgressBarIfNeeeded($hidpbDnldProdCode.val(), $hidInitpbDnldProdCode.val(), $pbDnldProdCode);
            CreateProgressBarIfNeeeded($hidpbUpldProdCode.val(), $hidInitpbUpldProdCode.val(), $pbUpldProdCode);

            CreateProgressBarIfNeeeded($hidpbDnldUser.val(), $hidInitpbDnldUser.val(), $pbDnldUser);
            CreateProgressBarIfNeeeded($hidpbUpldUser.val(), $hidInitpbUpldUser.val(), $pbUpldUser);


            // Quality
            CreateProgressBarIfNeeeded($hidpbDnldQualAux.val(), $hidInitpbDnldQualAux.val(), $pbDnldQualAux);
            CreateProgressBarIfNeeeded($hidpbUpldQualAux.val(), $hidInitpbUpldQualAux.val(), $pbUpldQualAux);

            CreateProgressBarIfNeeeded($hidpbDnldQualCompScan.val(), $hidInitpbDnldQualCompScan.val(), $pbDnldQualCompScan);
            CreateProgressBarIfNeeeded($hidpbUpldQualCompScan.val(), $hidInitpbUpldQualCompScan.val(), $pbUpldQualCompScan);

            CreateProgressBarIfNeeeded($hidpbDnldQualCompName.val(), $hidInitpbDnldQualCompName.val(), $pbDnldQualCompName);
            CreateProgressBarIfNeeeded($hidpbUpldQualCompName.val(), $hidInitpbUpldQualCompName.val(), $pbUpldQualCompName);

            CreateProgressBarIfNeeeded($hidpbDnldQualTool.val(), $hidInitpbDnldQualTool.val(), $pbDnldQualTool);
            CreateProgressBarIfNeeeded($hidpbUpldQualTool.val(), $hidInitpbUpldQualTool.val(), $pbUpldQualTool);

            CreateProgressBarIfNeeeded($hidpbDnldQualStation.val(), $hidInitpbDnldQualStation.val(), $pbDnldQualStation);
            CreateProgressBarIfNeeeded($hidpbUpldQualStation.val(), $hidInitpbUpldQualStation.val(), $pbUpldQualStation);

            CreateProgressBarIfNeeeded($hidpbDnldQualProdCode.val(), $hidInitpbDnldQualProdCode.val(), $pbDnldQualProdCode);
            CreateProgressBarIfNeeeded($hidpbUpldQualProdCode.val(), $hidInitpbUpldQualProdCode.val(), $pbUpldQualProdCode);

            CreateProgressBarIfNeeeded($hidpbDnldQualUser.val(), $hidInitpbDnldQualUser.val(), $pbDnldQualUser);
            CreateProgressBarIfNeeeded($hidpbUpldQualUser.val(), $hidInitpbUpldQualUser.val(), $pbUpldQualUser);

            CreateProgressBarLabels();          
            clearInterval(varTimer);
            updatePageData();
        }

        function CreateProgressBarLabels() {

            // for all progress bars,  Add it's percent value to it's span
            $("div[id^='pb']").each(function () {
                var currentProgessBar = $(this);
                if (currentProgessBar.hasClass('ui-progressbar')) {
                    var btn = $(this).closest('td').find('.inputButton');
                    var val = currentProgessBar.progressbar("value");
                    currentProgessBar.children(".spanProgressbarLabel").each(function () {
                        var msg;
                        if (btn.val() == "Reset") {
                            msg = "Failed!";
                            currentProgessBar.addClass("failed").children("div").addClass("failedProgressBar");
                            $(this).addClass("failedSpanLabel");
                        } else {
                            currentProgessBar.removeClass("failed").children("div").removeClass("failedProgressBar");
                            if (val > 99) {
                                msg = "Complete!";
                            } else { // if (val > 0) {
                                msg = val.toFixed(2) + " %";
                            }
                        }
                        $(this).html(msg); //spanProgressbarLabel
                    });
                }
            });
        }

        function CreateDialog_ConfirmDownload() {
            $dlgDownloadWarning = $('#dlgDownloadWarning');
            $dlgDownloadWarning.deleteDialog({ width: 400 });
        }

        function CreateDialog_ConfirmUpload() {
            $dlgUploadWarning = $('#dlgUploadWarning');
            $dlgUploadWarning.deleteDialog({ width: 400 });
        }


        function CreateDialog_Confirm() {
            var $button = $(this);
            $divDialogConfirm = $('#divDlgConfirmation');
            $divDialogConfirm.deleteDialog({ width: 400, "buttons": [
                              { text: "Yes", click: function () { __doPostBack($button.prop('name'), ''); $(this).dialog("close"); } }
                            , { text: "Cancel", click: function () { showMessage('Action Cancelled'); $(this).dialog("close"); } }
                    ]
            });
        }


        function AddEventToControlsWithClass_downloadButton() {
            //the selector $('.downloadButton') adds the function to click event of all controls with the class 'downloadButton'
            $('.downloadButton').click(function () {
                var $button = $(this);

                if ($button.val() == "Reset") {
                    $button.removeClass("failed");
                } else {
                    var $buttonIndex = $button.parent().index();
                    var boolShowWarning = false;
                    showMessage('');
                    clearInterval(varTimer);

                    var taskPLCTime = $button.closest('tr').find('td:eq(1)').text();
                    var taskPLCQualTime = $button.closest('tr').find('td:eq(2)').text();
                    var taskJITTime = $button.closest('tr').find('td:eq(3)').text();

                    if (((taskPLCTime > taskJITTime) && $buttonIndex == 4) || ((taskPLCQualTime > taskJITTime) && $buttonIndex == 5)) {
                        boolShowWarning = true;
                    }

                    if (boolShowWarning) {
                        $dlgDownloadWarning.dialog("option", "title", "Download Warning").dialog("option", "buttons", [
                             { text: "Yes", click: function () { __doPostBack($button.prop('name'), ''); $(this).dialog("close"); } }
                            , { text: "Cancel", click: function () { showMessage('Action Cancelled'); $(this).dialog("close"); } }
                            ]).dialog('open');
                        return false;
                    } else {
                        var task = $button.closest('tr').find('.task').text();
                        var line = $ddlLineNumber.children(":selected").text();
                        var msg = 'Are you sure you want to Download ' + task + ' for ' + line + "?";
                        $('#message').text(msg);
                        $divDialogConfirm.dialog("option", "title", "Download Warning").dialog("option", "buttons", [
                                     { text: "Yes", click: function () { __doPostBack($button.prop('name'), ''); $(this).dialog("close"); } }
                                    , { text: "Cancel", click: function () { showMessage('Action Cancelled'); $(this).dialog("close"); } }
                                    ]).dialog('open');
                        return false;
                    }
                }
            });
        }

        function AddEventToControlsWithClass_uploadButton() {
            //the selector $('.uploadButton') adds the function to click event of all controls with the class 'uploadButton'
            $('.uploadButton').click(function () {
                var $button = $(this);

                if ($button.val() == "Reset") {
                    $button.removeClass("failed");
                } else {
                    var $buttonIndex = $button.parent().index();
                    var boolShowWarning = false;
                    showMessage('');
                    clearInterval(varTimer);

                    var taskPLCTime = $button.closest('tr').find('td:eq(1)').text();
                    var taskPLCQualTime = $button.closest('tr').find('td:eq(2)').text();
                    var taskJITTime = $button.closest('tr').find('td:eq(3)').text();

                    if (((taskPLCTime < taskJITTime) && $buttonIndex == 6) || ((taskPLCQualTime < taskJITTime) && $buttonIndex == 7)) {
                        boolShowWarning = true;
                    }

                    if (boolShowWarning) {
                        $dlgUploadWarning.dialog("option", "title", "Upload Warning").dialog("option", "buttons", [
                             { text: "Yes", click: function () { __doPostBack($button.prop('name'), ''); $(this).dialog("close"); } }
                            , { text: "Cancel", click: function () { showMessage('Action Cancelled'); $(this).dialog("close"); } }
                    ]).dialog('open');
                        return false;
                    } else {
                        var task = $button.parent().parent().find('.task').text();
                        var line = $ddlLineNumber.children(":selected").text();
                        var msg = 'Are you sure you want to Upload ' + task + ' for ' + line + "?";
                        $('#message').text(msg);
                        $divDialogConfirm.dialog("option", "title", "Upload Warning").dialog("option", "buttons", [
                             { text: "Yes", click: function () { __doPostBack($button.prop('name'), ''); $(this).dialog("close"); } }
                            , { text: "Cancel", click: function () { showMessage('Action Cancelled'); $(this).dialog("close"); } }
                    ]).dialog('open');
                        return false;
                    }
                }
            });
        }

       

    </script>
    <style type="text/css">
        #Table1
        {
            padding-bottom: 8px;
        }
        .datagrid td
        {
            vertical-align: middle;
            text-align: center;
        }
        .datagrid span
        {
            white-space: nowrap;
        }
        
        #divMainCenterPanel
        {
            width: 1000px;
        }
        .inputButton
        {
            margin: 4px;
        }
        .dateTimeColumn
        {
            min-width: 124px;
        }
        .buttonColumn
        {
            min-width: 120px;            
        }
        .selectDropDown
        {
            height: 24px;
            width: 240px;
        }
        .task
        {
            margin: 0px 6px 0px 6px;
        }
                
        .ui-progressbar 
        { 
            height:1em ; 
            text-align: center; 
            width: 110px;
            margin-left: auto;
            margin-right: auto;
        }         
        .ui-progressbar .ui-progressbar-value 
        {                                             
            height:100%; 
        } 
        .hidden
        {
            display: none;
        }
        .qualityColumn
        {
            color: #8FA4CB;
        }
        .failed
        {
            color: #FF1D3A;
            background-color: #FFC7CE;
            background-image: none;
            border: 1px solid #FF7284;
        }
        .failedProgressBar
        {
            border-color: #FF9DA9;
            background-color: #FF9DA9;
            background-image: none;
        }        
        .failedSpanLabel
        {
            color: #FF1D3A !important;
        }        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainCenterPanel">        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>    
                <div>             
                    <asp:HiddenField id="hidFailAfterSeconds" runat="server" Value="30" />
                    <asp:HiddenField id="hidMultiPLC" runat="server" Value="0" />
                    <asp:HiddenField id="hidLineTypeID" runat="server" Value="0" />       
                    
                    <asp:HiddenField id="hidpbDnldAux" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldAux" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldCompScan" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldCompScan" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldCompName" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldCompName" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldTool" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldTool" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldStation" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldStation" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldProdCode" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldProdCode" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldUser" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldUser" runat="server" Value="0" /> 

                    <asp:HiddenField id="hidInitpbDnldAux" runat="server" Value="0" /> 
                    <asp:HiddenField id="hidInitpbUpldAux" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldCompScan" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldCompScan" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldCompName" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldCompName" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldTool" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldTool" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldStation" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldStation" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldProdCode" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldProdCode" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldUser" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldUser" runat="server" Value="0" />
                    
                    <asp:HiddenField id="hidpbDnldQualAux" runat="server" Value="0" /> 
                    <asp:HiddenField id="hidpbUpldQualAux" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldQualCompScan" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldQualCompScan" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldQualCompName" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldQualCompName" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldQualTool" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldQualTool" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldQualStation" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldQualStation" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldQualProdCode" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldQualProdCode" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbDnldQualUser" runat="server" Value="0" />
                    <asp:HiddenField id="hidpbUpldQualUser" runat="server" Value="0" /> 

                    <asp:HiddenField id="hidInitpbDnldQualAux" runat="server" Value="0" /> 
                    <asp:HiddenField id="hidInitpbUpldQualAux" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldQualCompScan" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldQualCompScan" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldQualCompName" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldQualCompName" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldQualTool" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldQualTool" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldQualStation" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldQualStation" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldQualProdCode" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldQualProdCode" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbDnldQualUser" runat="server" Value="0" />
                    <asp:HiddenField id="hidInitpbUpldQualUser" runat="server" Value="0" />                    
                </div>          
                <table id="Table1">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="spanLabel h2" Width="246px" Height="20px" Text="Line Number:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddLineNumber" runat="server" CssClass="selectDropDown h3" AutoPostBack="True"></asp:DropDownList>                                
                         </td>                           
                        <td>
                            <asp:Button ID="cmdRefreshAndClearComplete" runat="server"  Text="Refresh" CssClass="inputButton"></asp:Button>                                                      
                            <asp:Button ID="cmdRefresh" runat="server"  Text="hidden"  CssClass="inputButton hidden"  ></asp:Button>
                        </td>
                    </tr>
                </table>
                <div class="datagrid">
                    <table>
                        <tr class="DataGrid_HeaderStyle">
                            <td>
                            </td>
                            <td class="dateTimeColumn">
                                Last Modified in Build PLC
                            </td>
                            <td class="dateTimeColumn qualityColumn">
                                Last Modified in Quality PLC
                            </td>
                            <td class="dateTimeColumn">
                                Last Modified in JIT Manager
                            </td>
                            <td class="buttonColumn">
                                Download to Build PLC
                            </td>
                            <td class="buttonColumn qualityColumn">
                                Download to Quality PLC
                            </td>
                            <td class="buttonColumn">
                                Upload Build to DB
                            </td>
                            <td class="buttonColumn qualityColumn">
                                Upload Quality to DB
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:HyperLink ID="hlAuxTask" runat="server" NavigateUrl="~/3SystemConfiguration/1StationConfiguration/5StationTaskConfiguration/StationTaskConfiguration.aspx" CssClass="fontDecorationNone"><asp:Label ID="lblAuxCfg" runat="server" CssClass="spanLabel h2 task" Text="Aux Task Configuration"></asp:Label></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCAux" runat="server" CssClass="spanLabel dateTimeColumn" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCQualAux" runat="server" CssClass="spanLabel dateTimeColumn" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblJITAux" runat="server" CssClass="spanLabel downloadDateTime" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldAux" runat="server" Text="Download" CssClass="inputButton downloadButton" ></asp:Button>
                                <div id="pbDnldAux"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldQualAux" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldQualAux"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdUpldAux" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldAux"><span class="spanProgressbarLabel"></span></div>
                            </td>                           
                            <td>
                                <asp:Button ID="cmdUpldQualAux" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldQualAux"><span class="spanProgressbarLabel"></span></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:HyperLink ID="hlCompScan" runat="server" NavigateUrl="~/3SystemConfiguration/1StationConfiguration/4StationComponentScanAssignment/StationComponentScanAssignment.aspx" CssClass="fontDecorationNone"><asp:Label ID="Label8" runat="server" CssClass="spanLabel h2 task" Text="Component Scan Enable"></asp:Label></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCCompScan" runat="server" CssClass="spanLabel dateTimeColumn" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCQualCompScan" runat="server" CssClass="spanLabel dateTimeColumn" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblJITCompScan" runat="server" CssClass="spanLabel downloadDateTime" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldCompScan" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldCompScan"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldQualCompScan" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldQualCompScan"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdUpldCompScan" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldCompScan"><span class="spanProgressbarLabel"></span></div>
                            </td>                            
                            <td>
                                <asp:Button ID="cmdUpldQualCompScan" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldQualCompScan"><span class="spanProgressbarLabel"></span></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:HyperLink ID="hlCompName" runat="server" NavigateUrl="~/3SystemConfiguration/81ComponentNameConfiguration/ComponentNameConfiguration.aspx" CssClass="fontDecorationNone"><asp:Label ID="Label9" runat="server" CssClass="spanLabel h2 task" Text="Component Name Configuration"></asp:Label></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCCompName" runat="server" CssClass="spanLabel dateTimeColumn" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCQualCompName" runat="server" CssClass="spanLabel dateTimeColumn" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblJITCompName" runat="server" CssClass="spanLabel downloadDateTime" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldCompName" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldCompName" class="downloadProgressBar"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldQualCompName" runat="server" Text="Download" Visible="false" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldQualCompName"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdUpldCompName" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldCompName"><span class="spanProgressbarLabel"></span></div>
                            </td>                            
                            <td>
                                <asp:Button ID="cmdUpldQualCompName" runat="server" Text="Upload"  Visible="false" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldQualCompName"><span class="spanProgressbarLabel"></span></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:HyperLink ID="hlTool" runat="server" NavigateUrl="~/3SystemConfiguration/1StationConfiguration/6StationToolConfiguration/StationToolConfiguration.aspx" CssClass="fontDecorationNone"><asp:Label ID="lblToolCfg" runat="server" CssClass="spanLabel h2 task" Text="Tool Configuration"></asp:Label></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCTool" runat="server" CssClass="spanLabel dateTimeColumn" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCQualTool" runat="server" CssClass="spanLabel dateTimeColumn" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblJITTool" runat="server" CssClass="spanLabel downloadDateTime" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldTool" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldTool"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldQualTool" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldQualTool"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdUpldTool" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>                                
                                <div id="pbUpldTool"><span class="spanProgressbarLabel"></span></div>
                            </td>                            
                            <td>
                                <asp:Button ID="cmdUpldQualTool" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>                                
                                <div id="pbUpldQualTool"><span class="spanProgressbarLabel"></span></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:HyperLink ID="hlStation" runat="server" NavigateUrl="~/3SystemConfiguration/1StationConfiguration/1StationConfiguration/StationConfiguration.aspx" CssClass="fontDecorationNone"><asp:Label ID="Label11" runat="server" CssClass="spanLabel h2 task" Text="Station Configuration"></asp:Label></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCStation" runat="server" CssClass="spanLabel dateTimeColumn" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCQualStation" runat="server" CssClass="spanLabel dateTimeColumn" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblJITStation" runat="server" CssClass="spanLabel downloadDateTime" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldStation" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldStation"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldQualStation" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldQualStation"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdUpldStation" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldStation"><span class="spanProgressbarLabel"></span></div>
                            </td>                             
                            <td>
                                <asp:Button ID="cmdUpldQualStation" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldQualStation"><span class="spanProgressbarLabel"></span></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:HyperLink ID="hlProdCode" runat="server" NavigateUrl="~/3SystemConfiguration/89ProductCodeConfiguration/ProductCodeConfiguration.aspx" CssClass="fontDecorationNone"><asp:Label ID="Label12" runat="server" CssClass="spanLabel h2 task" Text="Product Code Configuration"></asp:Label></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCProdCode" runat="server" CssClass="spanLabel dateTimeColumn" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCQualProdCode" runat="server" CssClass="spanLabel dateTimeColumn" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblJITProdCode" runat="server" CssClass="spanLabel downloadDateTime" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldProdCode" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldProdCode"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldQualProdCode" runat="server" Text="Download"  Visible="false" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldQualProdCode"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdUpldProdCode" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldProdCode"><span class="spanProgressbarLabel"></span></div>
                            </td>                            
                            <td>
                                <asp:Button ID="cmdUpldQualProdCode" runat="server" Text="Upload"  Visible="false" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldQualProdCode"><span class="spanProgressbarLabel"></span></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:HyperLink ID="hlUser" runat="server" NavigateUrl="~/4SystemAdministration/2SecurityManagement/1UserAccounts/UserAccounts.aspx" CssClass="fontDecorationNone"><asp:Label ID="Label2" runat="server" CssClass="spanLabel h2 task" Text="User Configuration"></asp:Label></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCUser" runat="server" CssClass="spanLabel dateTimeColumn" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPLCQualUser" runat="server" CssClass="spanLabel dateTimeColumn" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblJITUser" runat="server" CssClass="spanLabel downloadDateTime" Text="--"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldUser" runat="server" Text="Download" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldUser"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdDnldQualUser" runat="server" Text="Download"  Visible="false" CssClass="inputButton downloadButton"></asp:Button>
                                <div id="pbDnldQualUser"><span class="spanProgressbarLabel"></span></div>
                            </td>
                            <td>
                                <asp:Button ID="cmdUpldUser" runat="server" Text="Upload" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldUser"><span class="spanProgressbarLabel"></span></div>
                            </td>                            
                            <td>
                                <asp:Button ID="cmdUpldQualUser" runat="server" Text="Upload"  Visible="false" CssClass="inputButton uploadButton"></asp:Button>
                                <div id="pbUpldQualUser"><span class="spanProgressbarLabel"></span></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDlgConfirmation" title="">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            <span id="message" class="spanLabel h3"></span>
        </p>
    </div>
    <div id="dlgDownloadWarning" title="Download Warning">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter spanLabel  h3" style="margin-left: 30px;">
            The timestamp of the PLC data is newer than the JIT database data.
        </p>
        <p class="pCenter spanLabel  h3" style="margin-left: 30px;">
            Are you sure you want to overwrite the PLC data?
        </p>
    </div>
    <div id="dlgUploadWarning" title="Upload Warning">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter spanLabel  h3" style="margin-left: 30px;">
            The timestamp of the JIT database data is newer than the PLC data.
        </p>
        <p class="pCenter spanLabel  h3" style="margin-left: 30px;">
            Are you sure you want to overwrite the JIT database data? 
        </p>
    </div>
</asp:Content>
