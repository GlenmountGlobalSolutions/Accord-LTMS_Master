<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StationSequenceConfiguration.aspx.vb" Inherits="LTMS_Master.StationSequenceConfiguration" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/jquery.alphanum.js") %>"></script>
    <script type="text/javascript">
        // partial load controls
        var $hidUpdateType;
        var $hidOriginalSequence;
        var $hidOriginalJobsCompleted;

        var $cmdPrev;
        var $cmdNext;
        var $cmdUpdate;

        var $ddlSeq;
        var $txtCurrJobsCompleted;
        var $txtUpdateJobsCompleted;
        var $txtUpdateTotalJobs;

        //first Load controls
        var $dlgUpdate;
        var $dlgLabelOldSeq;
        var $dlgLabelOldJobQ;
        var $dlgLabelewSeq;
        var $dlgLabelNewJobQ;

        function contentPageLoad(sender, args) {
            AddDirtyClassOnChange();
            CachePartialLoadControls();

            if (!args.get_isPartialLoad()) {
                //Specific code that is loaded once and not during postbacks can go in here.
                CacheFirstLoadControls();
            }

            AddWaitOnListBoxChange();

            ConvertTextboxToNumericOnly();

            AddDialog_Update();

            AddUpdateEventToButtons();
        }

        function CacheFirstLoadControls() {
            $dlgUpdate = $('#divDialogUpdate');
            $dlgLabelOldSeq = $('#MainContent_lblOldSeq');
            $dlgLabelOldJobQ = $('#MainContent_lblOldJobQ');
            $dlgLabelNewSeq = $('#MainContent_lblNewSeq');
            $dlgLabelNewJobQ = $('#MainContent_lblNewJobQ');
        }

        function CachePartialLoadControls() {
            $hidUpdateType = $('#MainContent_hidUpdateType');
            $hidOriginalSequence = $('#MainContent_hidOriginalSequence');
            $hidOriginalJobsCompleted = $('#MainContent_hidOriginalJobsCompleted');

            $cmdPrev = $('#MainContent_cmdPrev');
            $cmdNext = $('#MainContent_cmdNext');
            $cmdUpdate = $('#MainContent_cmdUpdate');

            $txtCurrJobsCompleted = $('#MainContent_txtCurrJobsCompleted');
            $txtUpdateJobsCompleted = $('#MainContent_txtUpdateJobsCompleted');
            $txtUpdateTotalJobs = $('#MainContent_txtUpdateTotalJobs');

            $ddlSeq = $find("<%=ddlSeq.ClientID%>"); // $('#MainContent_ddlSeq');
        }

        function AddWaitOnListBoxChange() {
            //$('#MainContent_ddlTop').change(function () { ShowWaitOverlay(); });
            $('#MainContent_ddlSeq').change(function () { ShowWaitOverlay(); });
            $('#MainContent_lbStations').change(function () { ShowWaitOverlay(); });
            $('#MainContent_cmdRefresh').click(function () { ShowWaitOverlay(); });
        } 

        function ConvertTextboxToNumericOnly() {
            $txtUpdateJobsCompleted.numeric({
                allowMinus: false,
                allowThouSep: false,
                allowDecSep: false
            });
        }

        function AddDialog_Update() {
            $dlgUpdate.modalDialog({
                control: $cmdUpdate,
                width: 620
            });
        }

        function AddUpdateEventToButtons() {

            $cmdUpdate.click(function () {
                var jobsCompleted = parseInt($txtUpdateJobsCompleted.val());
                var jobsTotal = parseInt($txtUpdateTotalJobs.val());

                if (jobsCompleted > jobsTotal) {
                    showMessage('Error: Jobs Completed must be less than or equal to Total Jobs');
                } else {
                    $hidUpdateType.val("update");
                    SetDialogLabels('');
                    $dlgUpdate.dialog('open');
                }
                return false;
            });

            $cmdPrev.click(function () {
                $hidUpdateType.val("prev");
                SetDialogLabels('Previous Job');
                $dlgUpdate.dialog('open');
                return false;
            });

            $cmdNext.click(function () {
                $hidUpdateType.val("next");
                SetDialogLabels('Next Job');
                $dlgUpdate.dialog('open');
                return false;
            });
        }
        function SetDialogLabels(txtUpdateTo) {
            showMessage('');

            $dlgLabelOldSeq.text($hidOriginalSequence.val());
            $dlgLabelOldJobQ.text($hidOriginalJobsCompleted.val());

            if (txtUpdateTo.length > 0) {
                $dlgLabelNewSeq.text(txtUpdateTo);
                $dlgLabelNewJobQ.text(txtUpdateTo);
            } else {
                $dlgLabelNewSeq.text($ddlSeq.get_selectedItem().get_text());
                $dlgLabelNewJobQ.text($txtUpdateJobsCompleted.val());
            }
        }


    </script>
    <style type="text/css" title="PageContent">
        #divMainLeftPanel
        {
            width: 270px;
            text-align: left;
        }
        
        #divMainCenterPanel
        {
            margin-left: 12px;
        }
        
        #divMainCenterPanel ul
        {
            list-style: none;
            margin-bottom: 0px;
            padding: 0px;
        }
        #divMainCenterPanel li
        {
            margin-bottom: 26px;
        }
        
        
        #divMainCenterPanel span.ui-icon
        {
            float: left;
            padding-right: 5px;
        }
        
        #divMainCenterPanel .buttonRow
        {
            text-align: center;
        }
        
        #divMainCenterPanel .textEntry
        {
            width: 200px;
        }
        
        #divMainCenterPanel table
        {
            padding: 0px;
            width: 500px;
        }
                
        div.ddlSeq .rcbReadOnly
        {
            background-color: white;
        	border: 1px solid #6495ED;
	        border-radius: 5px;
        }
        div.ddlSeq .rcbActionButton
        {
            padding-top: 0px;
        }
        #ctl00_MainContent_ddlSeq_Arrow
        {
            background-image:url("../../../Images/Arrows/ddl.png");
            background-size: 20px 20px;
            background-repeat: no-repeat;
            color: Transparent;
        }
        .rcbActionButton
        {
            margin: 0px -4px 0px 0px;
        }
        
    </style>
    <style type="text/css" title="UpdateDialog">
        TR.toRow span
        {
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divHiddenFields">
		<input id="hidBroadcastPointID" type="hidden" runat="server">   
	</div>
    <div id="divMainLeftPanel">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:ListBox ID="lbStations" runat="server" Height="300px" Width="270px" AutoPostBack="True" CssClass="selectListBox h3"></asp:ListBox>
                <div style="text-align: center;"><asp:Button ID="cmdRefresh" runat="server" CssClass="inputButton" Text="Refresh" Enabled="False"></asp:Button></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divMainCenterPanel">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hidUpdateType" runat="server" Value="0" />
                <asp:HiddenField ID="hidOriginalSequence" runat="server" Value="" />
                <asp:HiddenField ID="hidOriginalTotalJobs" runat="server" Value="" />
                <asp:HiddenField ID="hidOriginalJobsCompleted" runat="server" Value="" />
                <ul>
                    <li>
                        <table>
                            <tr>
                                <td style="width: 100px;">
                                    <span class="spanLabel h2">Station:</span>
                                </td>
                                <td style="width: 200px;">
                                    <asp:Label ID="lblStation" runat="server" CssClass="spanLabel h2"></asp:Label>
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
                            <tr>
                                <td>
                                    <span class="spanLabel h3">Current Sequence:</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurrSeq" runat="server" ReadOnly="True" CssClass="textEntry h3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="spanLabel h3">Total Jobs:</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurrTotalJobs" runat="server" ReadOnly="True" CssClass="textEntry h3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="spanLabel h3">Jobs Completed:</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurrJobsCompleted" runat="server" ReadOnly="True" CssClass="textEntry h3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="buttonRow">
                                <td colspan="2">
                                    <asp:Button ID="cmdPrev" runat="server" CssClass="inputButton" Text="Previous"></asp:Button>
                                    <asp:Button ID="cmdNext" runat="server" CssClass="inputButton" Text="Next"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </li>
                    <li>
                        <table>
                            <tr>
                                <td colspan="3">
                                    <span class="spanLabel h2">Update Sequence:</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <telerik:RadComboBox RenderMode="Lightweight" ID="ddlSeq" runat="server" 
                                            CssClass="selectDropDown ddlSeq"
                                            OnClientLoad="showSelectedItemImage" 
                                            OnClientSelectedIndexChanging="showImageOnSelectedItemChanging" 
                                            OnItemDataBound="ddlSeq_ItemDataBound"
                                            width="300px"
                                            AutoPostBack="true">
                                        <ExpandAnimation Type="OutBack" />
                                        <CollapseAnimation Type="InBack" />
                                    </telerik:RadComboBox>
                                    <span class="spanLabel h3" style="margin-left: 10px"> Show Top </span>
                                    <asp:DropDownList ID="ddlTop" runat="server" Width="90px" Height="24px" CssClass="selectDropDown h5">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td> 
                                <td style="text-align: center; padding-top: 20px;"><span class="spanLabel h2">Pallets</span></td> 
                            </tr>
                            <tr>
                                <td colspan="2" />
                                <td>
                                    <table style="width: 150px; margin-left:30px;">
                                    <tr>
                                        <td style="width: 50px; text-align: center;"><asp:Label ID="lblRow1" runat="server" CssClass="spanLabel h3" Text="Front (A1)"></asp:Label></td>
                                        <td style="width: 50px; text-align: center;"><asp:Label ID="lblRow2" runat="server" CssClass="spanLabel h3" Text="Mid (A2)" Enabled="False"></asp:Label></td>
                                        <td style="width: 50px; text-align: center;"><asp:Label ID="lblRow3" runat="server" CssClass="spanLabel h3" Text="Rear (B1)" Enabled="False"></asp:Label></td>
                                    </tr>
                                    </table>                                    
                                </td> 
                            </tr>
                            <tr>
                                <td >
                                    <span class="spanLabel h3">Total Jobs:</span>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtUpdateTotalJobs" runat="server" Width="108px" ReadOnly="True" CssClass="textEntry h3"></asp:TextBox>
                                </td>
                                <td>
                                    <table style="width: 150px; margin-left:30px;">
                                    <tr>
                                        <td style="width: 50px;"><asp:TextBox ID="txtPalletRow1Total" runat="server" Width="40px" ReadOnly="True" CssClass="textEntry h3 textCenter"></asp:TextBox></td>
                                        <td style="width: 50px;"><asp:TextBox ID="txtPalletRow2Total" runat="server" Width="40px" ReadOnly="True" CssClass="textEntry h3 textCenter" Enabled="False"></asp:TextBox></td>
                                        <td style="width: 50px;"><asp:TextBox ID="txtPalletRow3Total" runat="server" Width="40px" ReadOnly="True" CssClass="textEntry h3 textCenter" Enabled="False"></asp:TextBox></td>
                                    </tr>
                                    </table>
                                </td> 
                            </tr>
                            <tr>
                                <td>
                                    <span class="spanLabel h3">Jobs Completed:</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUpdateJobsCompleted" runat="server" Width="108px" CssClass="textEntry h3"></asp:TextBox>
                                </td>
                                <td>
                                    <table style="width: 150px; margin-left:30px;">
                                    <tr>
                                        <td style="width: 50px;"><asp:TextBox ID="txtPalletRow1Complete" runat="server" Width="40px" CssClass="textEntry h3 textCenter"></asp:TextBox></td>
                                        <td style="width: 50px;"><asp:TextBox ID="txtPalletRow2Complete" runat="server" Width="40px" CssClass="textEntry h3 textCenter" Enabled="False"></asp:TextBox></td>
                                        <td style="width: 50px;"><asp:TextBox ID="txtPalletRow3Complete" runat="server" Width="40px" CssClass="textEntry h3 textCenter" Enabled="False"></asp:TextBox></td>
                                    </tr>
                                    </table>
                                </td> 
                            </tr>
                            <tr class="buttonRow">
                                <td colspan="2">
                                    <asp:Button ID="cmdUpdate" runat="server" CssClass="inputButton" Text="Update"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDialogUpdate" title="Station Sequence Update">
        <%--        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
        --%>
        <table id="Table1" style="padding: 0px;" class="tableCenter">
            <tr class="toRow">
                <td colspan="2" style="text-align: center;">
                    <span class="h2">ARE YOU SURE YOU WANT TO CONTINUE?</span>
                </td>
            </tr>
            <tr class="toRow">
                <td colspan="2" style="text-align: center;">
                    <span>You are about to update the:</span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 250px;">
                    <span class="spanLabel h3">Station Sequence Number From:</span>
                </td>
                <td style="width: 320px;">
                    <asp:Label ID="lblOldSeq" runat="server" CssClass="spanLabel h3"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="spanLabel h3">Jobs Completed From:</span>
                </td>
                <td>
                    <asp:Label ID="lblOldJobQ" runat="server" CssClass="spanLabel h3"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr class="toRow">
                <td>
                    <span class="spanLabel h3">To Station Sequence Number:</span>
                </td>
                <td>
                    <asp:Label ID="lblNewSeq" runat="server" CssClass="spanLabel h3"></asp:Label>
                </td>
            </tr>
            <tr class="toRow">
                <td>
                    <span class="spanLabel h3">To Jobs Completed:</span>
                </td>
                <td>
                    <asp:Label ID="lblNewJobQ" runat="server" CssClass="spanLabel h3"></asp:Label>
                </td>
            </tr>
        </table>
        <%--            </ContentTemplate>
        </asp:UpdatePanel>
        --%>
    </div>
</asp:Content>
