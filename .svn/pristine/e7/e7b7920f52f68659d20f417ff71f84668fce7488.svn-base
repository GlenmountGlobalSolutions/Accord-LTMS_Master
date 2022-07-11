<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ReworkMaterialDispositionListConfiguration.aspx.vb" Inherits="LTMS_Master.ReworkMaterialDispositionListConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var $treeComp;
        var $btnExp;
        var $btnCol;

        var $cmdAction;
        var $cmdCopy;
        var $cmdDelete;
        var $cmdRename;

        var $helpDesc;
        var $txtDlgDescription;

        var $hidAction;
        var nodeType;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            RemoveTreeViewSkipLinks();

            if (!args.get_isPartialLoad()) {
                //Specific code that is only needed on first page load, and not for each postback.
                CacheLoadOnceControls();
                AddEventsToExpandAndCollapseButtons();
            }

            CachePartialLoadControls();

            EnableDefaultCommandButtons();

            AddEventToTreeView();

            AddModalDialog();

            AddDialog_Delete();

            AddStyleToStaticNodes();
        }

        function CachePartialLoadControls() {
            $treeComp = $('#MainContent_treeComp');

            $hidAction = $("#MainContent_hidAction");

            $cmdAction = $('#MainContent_cmdAction');
            $cmdCopy = $('#MainContent_cmdCopy');
            $cmdDelete = $('#MainContent_cmdDelete');
            $cmdRename = $('#MainContent_cmdRename');
        }

        function CacheLoadOnceControls() {
            $btnExp = $('#btnExp');
            $btnCol = $('#btnCol');

            $helpDesc = $("#MainContent_helpDesc");
            $txtDlgDescription = $("#MainContent_txtDlgDescription");
        }

        function AddEventsToExpandAndCollapseButtons() {
            $btnExp.click(function () { TreeviewExpandCollapseAll('MainContent_treeComp', true); return false; });
            $btnCol.click(function () { TreeviewExpandCollapseAll('MainContent_treeComp', false); return false; });
        }

        function EnableDefaultCommandButtons() {
            var treeViewData = window['MainContent_treeComp_Data'];
            if (treeViewData.selectedNodeID.value == "") {
                EnableCommandButtons(0);
                nodeType = "Component";
            } else {
                EnableCommandButtons(TreeViewGetSelectedNodeDepth(treeViewData.selectedNodeID));
            }
        }

        function AddModalDialog() {
            var $dlg = $('#divModalDialog');

            $cmdAction.click(function () {
                $hidAction.val('New');
                $dlg.dialog("option", "title", "Add New " + nodeType).dialog('open');
                return false;
            });

            $cmdCopy.click(function () {
                $hidAction.val('Copy');
                $dlg.dialog("option", "title", "Copy " + nodeType).dialog('open');
                return false;
            });

            $cmdRename.click(function () {
                $hidAction.val('Rename');
                $dlg.dialog("option", "title", "Rename " + nodeType).dialog('open');
                return false;
            });

            $dlg.modalDialog({
                control: $cmdAction,
                width: 360,
                validationFunction: function () { return ValidateDialog(); }
            });
        }

        function AddDialog_Delete() {
            var $dlgDelete = $('#divDialogDelete');

            $cmdDelete.click(function () { $dlgDelete.dialog('open'); return false; });

            $dlgDelete.deleteDialog({ control: $cmdDelete, width: 360 });
        }

        function AddStyleToStaticNodes() {
            $("a:contains('Material Category')", '#MainContent_treeComp').addClass('treeViewNodeCategory');
            $("a:contains('Rework Category')", '#MainContent_treeComp').addClass('treeViewNodeCategory');
            $("a:contains('Disposition Reasons')", '#MainContent_treeComp').addClass('treeViewNodeCategory');
            $("a:contains('Rework Reasons')", '#MainContent_treeComp').addClass('treeViewNodeCategory');
            $("a:contains('Parts List')", '#MainContent_treeComp').addClass('treeViewNodeCategory');
        }

        function AddEventToTreeView() {
            $treeComp.click(function () { return GetSelectedNode(event, 'MainContent_treeComp_Data'); });
        }

        function ValidateDialog() {
            var bValid = true;
            bValid = checkLength($txtDlgDescription, "Description", 1, 50, $helpDesc);
            return bValid;
        }

        function GetSelectedNode(evt, treeData) {
            try {
                var nodeLevel;
                var src = window.event != window.undefined ? window.event.srcElement : evt.target;
                var nodeClick = src.tagName.toLowerCase() == "a" || src.tagName.toLowerCase() == "span";
                if (nodeClick) {

                    var treeViewData = window[treeData];

                    nodeLevel = TreeViewGetSelectedNodeDepth(treeViewData.selectedNodeID);

                    if (nodeLevel >= 0) {
                        EnableCommandButtons(nodeLevel);

                        return false;   //change to true if you want postback on node click
                    }
                }
            }
            catch (Error) {
                alert(Error);
            }
        }

        function EnableCommandButtons(nodeLevel) {

            switch (nodeLevel) {
                case 1:
                    nodeType = "Component";
                    $cmdAction.removeAttr("disabled");
                    $cmdCopy.removeAttr("disabled");
                    $cmdDelete.removeAttr("disabled");
                    $cmdRename.removeAttr("disabled");

                    break;
                case 2:
                    nodeType = "Category";
                    $cmdAction.removeAttr("disabled");
                    $cmdCopy.attr("disabled", "disabled")
                    $cmdDelete.attr("disabled", "disabled")
                    $cmdRename.attr("disabled", "disabled")

                    break;
                case 3:
                    nodeType = "Material Type";
                    $cmdAction.attr("disabled", "disabled")
                    $cmdCopy.removeAttr("disabled");
                    $cmdDelete.removeAttr("disabled");
                    $cmdRename.removeAttr("disabled");

                    break;
                case 4:
                    nodeType = "Material List";
                    $cmdAction.removeAttr("disabled");
                    $cmdCopy.attr("disabled", "disabled")
                    $cmdDelete.attr("disabled", "disabled")
                    $cmdRename.attr("disabled", "disabled")

                    break;
                case 5:
                    nodeType = "List Value";
                    $cmdAction.attr("disabled", "disabled")
                    $cmdCopy.attr("disabled", "disabled")
                    $cmdDelete.removeAttr("disabled");
                    $cmdRename.removeAttr("disabled");

                    break;

                default:
                    nodeType = "";
                    $cmdAction.removeAttr("disabled");
                    $cmdCopy.attr("disabled", "disabled")
                    $cmdDelete.attr("disabled", "disabled")
                    $cmdRename.attr("disabled", "disabled")

                    break;
            }
        }

    </script>
    <style type="text/css">
        #divMainLeftPanel
        {
            text-align: center;
            width: 360px;
        }
        #cmdPanel
        {
            float: left;
        }
        
        .treePanel
        {
            background-color: #fff;
        }
        
        #divTree
        {
            background-color: #6495ed;
            border-radius: 5px;
            border: 0px none;
            padding: 1px 1px 0px 1px;
            margin-bottom: 12px;
        }
        
        .treeViewNode
        {
            font-size: 1.1em;
        }
        
        .treeViewNodeCategory
        {
            color: #828282;
            font-style: italic;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="divTree">
                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="350" Height="360" CssClass="divBorder treePanel">
                        <asp:TreeView ID="treeComp" runat="server" BorderWidth="0" ExpandDepth="0" EnableClientScript="true">
                            <NodeStyle CssClass="treeViewNode" />
                            <SelectedNodeStyle CssClass="treeViewNodeSelected ui-state-default" />
                        </asp:TreeView>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <input id="btnExp" type="button" value="Expand" />
        <input id="btnCol" type="button" value="Collapse" />
    </div>
    <div id="cmdPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hidAction" runat="server" Value="" />
                <asp:Button ID="cmdAction" runat="server" Text="New"></asp:Button>
                <asp:Button ID="cmdCopy" runat="server" Text="Copy"></asp:Button>
                <asp:Button ID="cmdDelete" runat="server" Text="Delete"></asp:Button>
                <asp:Button ID="cmdRename" runat="server" Text="Rename"></asp:Button>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divModalDialog" title="">
        <p class="validationHints">
            All form fields are required.</p>
        <table id="Table1" style="text-align: left;" class="tableCenter">
            <tr>
                <td>
                    <span class="ui-icon ui-icon-triangle-1-e"></span>
                </td>
                <td>
                    <asp:Label ID="lblDlgLabel" runat="server" CssClass="spanLabel h3">New Description:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDlgDescription" runat="server" CssClass='textEntry'></asp:TextBox>
                </td>
                <td>
                    <div id="helpDesc" class="ui-state-default ui-corner-all">
                        <span class="ui-icon ui-icon-help"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogDelete" title="Delete Selected Item">
        <div class="ui-corner-all ui-state-default" style="top: 40%; position: absolute;">
            <span class="ui-icon ui-icon-alert"></span>
        </div>
        <p class="pCenter" style="margin-left: 30px;">
            <span class="spanLabel">You are about to Delete the selected Style Group.<br />
                Do you wish to continue?</span>
        </p>
    </div>
</asp:Content>
