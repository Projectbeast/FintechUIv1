<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" title="FIR Transaction" autoeventwireup="true" inherits="Origination_S3GOrgFieldInformationReport, App_Web_w304vrwx" %>

<%@ MasterType VirtualPath="~/Common/S3GMasterPageCollapse.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="ContentFIR" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    

    <script language="javascript" type="text/javascript">

        ////////////////////

        ////////////////////////

        //var tab; //,tab1,tab2,tab3 ;

        //function pageLoad() {
        //    tab = $find('ctl00_ContentPlaceHolder1_tcRegBranch');
        //    tab.add_activeTabChanged(on_Change);
        //}

        //var index = 0;
        //function on_Change(sender, e) {
        //    var newindex = tab.get_activeTabIndex();

        //    if (newindex > index) {
        //        switch (newindex) {
        //            case 1:
        //                if (!Page_ClientValidate('VGRequest')) {
        //                    tab.set_activeTabIndex(0);
        //                    index = 0;
        //                    break;
        //                }
        //                break;

        //        }
        //    }
        //}      
        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=gvGrigPDDT.ClientID %>').style.width = parseInt(screen.width) - 20;
                document.getElementById('<%=gvGrigPDDT.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=gvGrigPDDT.ClientID %>').style.width = parseInt(screen.width) - 260;
                document.getElementById('<%=gvGrigPDDT.ClientID %>').style.overflow = "scroll";
            }
        }
        function pageLoad(s, a) {
            document.getElementById('<%=gvGrigPDDT.ClientID %>').style.width = parseInt(screen.width) - 260;
            document.getElementById('<%=gvGrigPDDT.ClientID %>').style.overflow = "scroll";
        }
        function showMenu(show) {
            if (show == 'T') {
                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                (document.getElementById('<%=gvGrigPDDT.ClientID %>')).style.width = screen.width - 260;
            }
            if (show == 'F') {
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
                (document.getElementById('<%=gvGrigPDDT.ClientID %>')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }
        // Resize  Grid End

        function uploadComplete(sender, args) {
            var objID = sender._inputFile.id.split("_");
            objID = "<%= gvPRDDT.ClientID %>" + "_" + objID[5];
            if (document.getElementById(objID + "_hidThrobber") != null) {                
                document.getElementById(objID + "_hidThrobber").value = args._fileName;
            }
        }

        function ConformCancellation() {

        }

        function ClearResponse() {
            if (fnConfirmClear())       // confirmation to clear
            {
                document.getElementById('<%=ddlClientCreditability.ClientID%>').selectedIndex = 0; // setting the selected index  = 0
                document.getElementById('<%=ddlClientNetWorth.ClientID%>').selectedIndex = 0; // setting the selected index  = 0

                document.getElementById('<%=txtRespondedBy.ClientID%>').value = '';
                document.getElementById('<%=txtFieldRespond.ClientID%>').value = '';               
                document.getElementById('<%=txtValue.ClientID%>').value = '';
                document.getElementById('<%=txtMobile.ClientID%>').value = '';
                document.getElementById('<%=txtEmailRes.ClientID%>').value = '';
                document.getElementById('<%=txtResponseDesgn.ClientID%>').value = '';



            }
        }
        function SetDefaultForm() {

            if (fnConfirmClear())       // confirmation to clear
            {

                document.getElementById('<%=txtLOB.ClientID%>').value = '';
                document.getElementById('<%=ddlStatus.ClientID%>').selectedIndex = 0; // setting the selected index  = 0
                document.getElementById('<%=txtBranch.ClientID%>').value = '';
                document.getElementById('<%=ddlAgencyCode.ClientID%>').selectedIndex = 0; // setting the selected index  = 0
                document.getElementById('<%=ddlEnquiryNumber.ClientID%>').selectedIndex = 0; // setting the selected index  = 0
                document.getElementById('<%=ddlEnquiryNumber.ClientID%>').items = "";
                document.getElementById('<%=txtTerms.ClientID%>').value = '';
                document.getElementById('<%=txtFIR.ClientID%>').value = '';              
                document.getElementById('<%=txtAgencyNAmeAddress.ClientID%>').value = '';
                document.getElementById('<%=txtFieldRequest.ClientID%>').value = '';
                document.getElementById('<%=txtSendEmail.ClientID%>').value = '';
              
                document.getElementById('<%=txtRequestBy.ClientID%>').value = '';
                document.getElementById('<%=txtAgencyNAmeAddress.ClientID%>').value = '';

                var myval = document.getElementById('<%=vsFIR.ClientID%>');
                ValidatorEnable(myval, false);
                var myval1 = document.getElementById('<%=ValidationSummary1.ClientID%>');
                ValidatorEnable(myval1, false);
                return true;
            }
            else {
                return false;
            }
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="FIR Transaction" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
            </table>
            <cc1:TabContainer ID="tcRegBranch" runat="server" Width="100%" CssClass="styleTabPanel" OnActiveTabChanged="tcRegBranch_ActiveTabChanged"
                ActiveTabIndex="1" AutoPostBack="true">
                <cc1:TabPanel runat="server" ID="TPFIR" CssClass="tabpan" TabIndex="0" BackColor="Red"
                    Width="100%" HeaderText="FIR Transaction">
                    <HeaderTemplate>
                        Field Information Report</HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                            <ContentTemplate>
                                <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel">
                                                <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="50%">
                                                            <table align="center" cellspacing="0" width="100%">
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" width="40%">
                                                                        <asp:Label ID="lblDocType" runat="server" CssClass="styleReqFieldLabel" Text="Reference Type"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:DropDownList ID="ddlDocType" runat="server" AutoPostBack="true" Width="205px"
                                                                            OnSelectedIndexChanged="ddlDocType_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlDocType"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Reference Type"
                                                                            InitialValue="0" ValidationGroup="VGRequest"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="ddlDocType"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Reference Type"
                                                                            InitialValue="0" ValidationGroup="VGPreview"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" width="40%">
                                                                        <asp:Label ID="lblEnquiryNumber" runat="server" CssClass="styleReqFieldLabel" Text="Reference No."></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <%--<cc1:ComboBox ID="ddlEnquiryNumber" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                                                            runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                                                            MaxLength="0" Width="170px" Enabled="false" CssClass="WindowsStyle" OnSelectedIndexChanged="ddlEnquiryNumber_SelectedIndexChanged">
                                                                        </cc1:ComboBox>--%>
                                                                        <uc2:Suggest ID="ddlEnquiryNumber" runat="server" ServiceMethod="GetDocumentNumber"
                                                                            AutoPostBack="true" OnItem_Selected="ddlEnquiryNumber_SelectedIndexChanged" IsMandatory="true"
                                                                            ValidationGroup="VGRequest" ErrorMessage="Select Reference Number" />
                                                                        <%-- <asp:DropDownList ID="ddlEnquiryNumber" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEnquiryNumber_SelectedIndexChanged"
                                                                            Width="205px" ToolTip="Enquiry Numer">
                                                                        </asp:DropDownList>--%>
                                                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="ddlEnquiryNumber"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Reference Number"
                                                                            InitialValue="0" ValidationGroup="VGRequest"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlEnquiryNumber"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Reference Number"
                                                                            InitialValue="0" ValidationGroup="VGPreview"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td height="8px">
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" width="40%">
                                                                        <asp:Label ID="lblSanum" runat="server" CssClass="styleReqFieldLabel" Text="Sub Acc. No."
                                                                            Visible="false"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:DropDownList ID="ddlSanum" runat="server" ValidationGroup="VGRequest" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlSanum_SelectedIndexChanged" Visible="False" Width="205px"
                                                                            ToolTip="Sub Account Number">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RfvSanum" runat="server" ControlToValidate="ddlSanum"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Sub Account Number"
                                                                            InitialValue="0" ValidationGroup="VGRequest" Enabled="false"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="50%" valign="top">
                                                            <table align="center" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" width="40%">
                                                                        <asp:Label ID="lblLOB" runat="server" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:DropDownList ID="ddlLOB" runat="server" Enabled="False" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                            ValidationGroup="VGRequest" Visible="False" Width="205px" ToolTip="Line Of Business">
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtLOB" runat="server" ReadOnly="true" Width="200px" ToolTip="Line Of Business"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" width="40%">
                                                                        <asp:Label ID="lblBranch" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:DropDownList ID="ddlBranch" runat="server" Enabled="False" Visible="False" Width="205px">
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtBranch" runat="server" ReadOnly="true" Width="160px" ToolTip="Location"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table align="center" cellpadding="0" cellspacing="0" width="100%">
                                                <tr width="100%">
                                                    <td width="50%">
                                                        <asp:Panel ID="pnlFIRInfo" runat="server" CssClass="stylePanel" GroupingText="Field Information">
                                                            <table align="center" cellpadding="0" cellspacing="1" width="100%">
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblFIRDate" runat="server" CssClass="styleDisplayLabel" Text="FIR Date"
                                                                            ReadOnly="true"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="50%">
                                                                        <asp:TextBox ID="txtFIRDate" runat="server" MaxLength="12" ReadOnly="true" Width="50%"
                                                                            ToolTip="FIR Date"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblCashflowDesc" runat="server" CssClass="styleDisplayLabel" Text="FIR Number"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtFIR" runat="server" AutoPostBack="True" MaxLength="11" ReadOnly="true"
                                                                            Width="200px" ToolTip="FIR Number"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblRequestBy" runat="server" CssClass="styleDisplayLabel" Text="Requested By"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtRequestBy" runat="server" MaxLength="40" ReadOnly="true" ValidationGroup="VGRequest"
                                                                            Width="200px" ToolTip="Requested By"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                                                runat="server" ControlToValidate="txtRequestBy" Display="None" ErrorMessage="Enter Requested By"
                                                                                ValidationGroup="VGRequest"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblRequestDate" runat="server" CssClass="styleReqFieldLabel" Text="Requested Date"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="60%">
                                                                        <asp:TextBox ID="txtRequestDate" runat="server" MaxLength="200" Width="45%" ToolTip="Requested Date"></asp:TextBox>
                                                                        <%-- &nbsp;&nbsp;
                                                                        <asp:Image ID="imgRequestDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                                            Visible="false" />--%>
                                                                        <%-- <cc1:CalendarExtender ID="cexDate1" runat="server" Enabled="True" 
                                                                            PopupButtonID="imgRequestDate" TargetControlID="txtRequestDate">
                                                                        </cc1:CalendarExtender>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblRound" runat="server" CssClass="styleDisplayLabel" Text="Round"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:DropDownList ID="ddlRound" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRound_SelectedIndexChanged"
                                                                            Width="20%" ToolTip="Round">
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtRound" runat="server" ReadOnly="true" Width="10%" ToolTip="Round"
                                                                            Style="text-align: right"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblStatus" runat="server" CssClass="styleDisplayLabel" Text="Status"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:DropDownList ID="ddlStatus" runat="server" Enabled="False" Width="48%" AutoPostBack="True"
                                                                            ToolTip="Status">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="23%">
                                                                        <asp:Label ID="lblAgencyType" runat="server" Text="Employee/Agency Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" align="left" width="27%">
                                                                        <asp:DropDownList ID="ddlAgencyType" runat="server" Enabled="False" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlAgencyType_SelectedIndexChanged" Width="165px" ToolTip="Agency Type">
                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="ddlAgencyType"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Agency Type"
                                                                            InitialValue="0" ValidationGroup="VGRequest"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="ddlAgencyType"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Agency Type"
                                                                            InitialValue="0" ValidationGroup="VGPreview"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblAgencyCode" runat="server" CssClass="styleReqFieldLabel" Text="Employee/Agency Code"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:DropDownList ID="ddlAgencyCode" runat="server" AutoPostBack="True" Enabled="False"
                                                                            OnSelectedIndexChanged="ddlAgencyCode_SelectedIndexChanged" Width="205px" ToolTip="Agency Code">
                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlAgencyCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Agency Code"
                                                                            InitialValue="0" ValidationGroup="VGRequest"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlAgencyCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Agency Code"
                                                                            InitialValue="0" ValidationGroup="VGPreview"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="Label2" runat="server" CssClass="styleDisplayLabel" Text="Agency Name"
                                                                            Visible="false"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtAgencyName" runat="server" ReadOnly="true" ValidationGroup="VGRequest"
                                                                            Width="200px" ToolTip="Agency Name" Visible="false"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblAgencyNameAddress" runat="server" CssClass="styleDisplayLabel"
                                                                            Text="Agency Address"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtAgencyNAmeAddress" runat="server" Height="50px" ReadOnly="True"
                                                                            TextMode="MultiLine" Width="200px" ToolTip="Agency Address"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                    <td valign="top" width="50%">
                                                        <asp:Panel ID="Panel3" runat="server" CssClass="stylePanel" GroupingText="Float Details">
                                                            <table align="center" cellpadding="1" cellspacing="0" width="100%">
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblFieldRequest" runat="server" CssClass="styleReqFieldLabel" Text="Field Request"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtFieldRequest" runat="server" Height="102px" MaxLength="200" onkeyup="maxlengthfortxt(200)"
                                                                            TextMode="MultiLine" Width="200px" ToolTip="Field Request"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFieldRequest"
                                                                            Display="None" ErrorMessage="Enter Field Request" ValidationGroup="VGRequest">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtFieldRequest"
                                                                            Display="None" ErrorMessage="Enter Field Request" ValidationGroup="VGPreview">
                                                                        </asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblTerms" runat="server" CssClass="styleReqFieldLabel" Text="Terms &amp; Conditions"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtTerms" runat="server" Height="100px" MaxLength="100" onkeyup="maxlengthfortxt(100)"
                                                                            TextMode="MultiLine" ValidationGroup="VGRequest" Width="200px" ToolTip="Terms and Conditions"></asp:TextBox><asp:RequiredFieldValidator
                                                                                ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTerms" Display="None"
                                                                                ErrorMessage="Enter Terms &amp; Conditions" ValidationGroup="VGRequest"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblSendEmail" runat="server" CssClass="styleReqFieldLabel" Text="Send EMail Id"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="60%">
                                                                        <asp:TextBox ID="txtSendEmail" runat="server" MaxLength="60" Width="200px" ToolTip="Send EMail ID"></asp:TextBox><asp:RegularExpressionValidator
                                                                            ID="revEmailId" runat="server" ControlToValidate="txtSendEmail" Display="None"
                                                                            ErrorMessage="Enter a valid Agency EMailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                            ValidationGroup="VGRequest"></asp:RegularExpressionValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtSendEmail"
                                                                            Display="None" ErrorMessage="Enter Send EMail ID" ValidationGroup="VGRequest"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtSendEmail"
                                                                            Display="None" ErrorMessage="Enter Send EMail ID" ValidationGroup="VGPreview"></asp:RequiredFieldValidator>
                                                                        <asp:RegularExpressionValidator ValidationGroup="VGPreview" ID="RegularExpressionValidator3"
                                                                            runat="server" ControlToValidate="txtSendEmail" Display="None" ErrorMessage="Enter a valid EMailID"
                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                        </asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="Label1" runat="server" CssClass="styleDisplayLabel" Text="Cancelled"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="60%">
                                                                        <asp:CheckBox ID="chkCancelled" runat="server" Enabled="False" ToolTip="Cancelled Status" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" align="center">
                                            <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" GroupingText="Customer Information">
                                                <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" ActiveViewIndex="1"
                                                    FirstColumnWidth="20%" SecondColumnWidth="30%" ThirdColumnWidth="20%" FourthColumnWidth="30%" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlCustomerInfo" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                                                Visible="false">
                                                <table align="center" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td colspan="2" height="15px">
                                                        </td>
                                                    </tr>
                                                    <tr width="100%">
                                                        <td valign="top" width="50%">
                                                            <table align="center" cellpadding="1" cellspacing="1" width="100%">
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblCustomerCode" runat="server" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtCustomerCode" runat="server" AutoPostBack="True" MaxLength="50"
                                                                            ReadOnly="true" Width="200px" ToolTip="Customer Code"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top">
                                                                        <asp:Label ID="lblCustNameAddress" runat="server" CssClass="styleDisplayLabel" Text="Address"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtCustNameAddress" runat="server" Height="70px" MaxLength="100"
                                                                            ReadOnly="True" TextMode="MultiLine" Width="200px" ToolTip="Address"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" width="50%">
                                                            <table align="center" cellpadding="1" cellspacing="1" width="100%">
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblCustomerName" runat="server" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtCustomerName" runat="server" MaxLength="50" ReadOnly="true" Width="200px"
                                                                            ToolTip="Customer Name"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblCustomerMobile" runat="server" CssClass="styleDisplayLabel" Text="Mobile"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtCustomerMobile" runat="server" MaxLength="60" ReadOnly="true"
                                                                            Width="200px" ToolTip="Mobile"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                                        <asp:Label ID="lblEmailCust" runat="server" CssClass="styleDisplayLabel" Text="EMail Id"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                                        <asp:TextBox ID="txtEmailCust" runat="server" MaxLength="60" ReadOnly="true" Width="200px"
                                                                            ToolTip="EMail ID"></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                                                runat="server" ControlToValidate="txtEmailCust" Display="None" ErrorMessage="Enter a valid Customer EMailID"
                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="VGRequest"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                            <asp:Label ID="Label3" runat="server" CssClass="styleDisplayLabel" Text="Website"></asp:Label>
                                                        </td>
                                                        <td align="left" class="styleFieldAlign" style="width: 60%">
                                                            <asp:TextBox ID="txtWebsite" runat="server" MaxLength="60" ReadOnly="true" Width="200px"
                                                                ToolTip="Website"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <%--</td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="2" height="8px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RequiredFieldValidator ID="rfvRequestDate" runat="server" ControlToValidate="txtRequestDate"
                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Requested Date"
                                                ValidationGroup="VGRequest"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" ID="TPRIR" CssClass="tabpan" BackColor="Red" Width="100%"
                    HeaderText="Respond Information Report">
                    <HeaderTemplate>
                        Respond Information Report
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table width="100%" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td colspan="2">
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtRespondedBy"
                                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="  .~`!@#$%^&*()_-+=[]{};':<>,?/"
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtMobile"
                                                FilterType="Numbers" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTEDesignation" runat="server" TargetControlID="txtResponseDesgn"
                                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="  .~`!@#$%^&*()_-+=[]{};':<>,?/"
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="15px" style="width: 8%">
                                        </td>
                                    </tr>
                                    <tr width="100%">
                                        <td valign="top" style="width: 50%">
                                            <table align="center" cellpadding="1" cellspacing="1" width="100%">
                                                <tr width="100%">
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblRespondedBy" runat="server" CssClass="styleReqFieldLabel" Text="Responded By"></asp:Label>
                                                    </td>
                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                        <asp:TextBox ValidationGroup="VGRequest" ID="txtRespondedBy" runat="server" Width="200px"
                                                            MaxLength="40" ToolTip="Responded By"></asp:TextBox><asp:RequiredFieldValidator ValidationGroup="VGRequest"
                                                                ID="rfvCashflowDesc" runat="server" ErrorMessage="Enter Responded By" ControlToValidate="txtRespondedBy"
                                                                Display="None"> </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblRespondedDate" runat="server" CssClass="styleDisplayLabel" Text="Responded Date"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="60%">
                                                        <asp:TextBox ID="txtResspondedDate" runat="server" Enabled="true" MaxLength="200"
                                                            Width="45%" ToolTip="Responded Date"></asp:TextBox>
                                                        <%-- <cc1:CalendarExtender ID="txtResspondedDate_CalendarExtender" runat="server" Enabled="True"
                                                             PopupButtonID="imgRespondedDate"
                                                            TargetControlID="txtResspondedDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgRespondedDate" runat="server" ImageUrl="~/Images/calendaer.gif" />--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="styleFieldLabel" valign="top" width="50%">
                                                        <asp:Label ID="lblResponseDesgn" runat="server" CssClass="styleReqFieldLabel" Text="Responder Designation"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="60%">
                                                        <asp:TextBox ValidationGroup="VGRequest" ID="txtResponseDesgn" runat="server" MaxLength="40"
                                                            Width="200px" ToolTip="Responder Designation"></asp:TextBox><asp:RequiredFieldValidator
                                                                ValidationGroup="VGRequest" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Enter Responder Designation"
                                                                ControlToValidate="txtResponseDesgn" Display="None"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblEmail" runat="server" CssClass="styleReqFieldLabel" Text="Responder EMail Id"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="60%">
                                                        <asp:TextBox EnableTheming="True" ValidationGroup="VGRequest" ID="txtEmailRes" runat="server"
                                                            MaxLength="60" Width="200px" ToolTip="Responder EMail ID"></asp:TextBox><asp:RequiredFieldValidator
                                                                ValidationGroup="VGRequest" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter Responder EMailID"
                                                                ControlToValidate="txtEmailRes" Display="None"> </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ValidationGroup="VGRequest" ID="RegularExpressionValidator2"
                                                            runat="server" ControlToValidate="txtEmailRes" Display="None" ErrorMessage="Enter a valid EMailID"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                        </asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblMobile" runat="server" CssClass="styleReqFieldLabel" Text="Responder Mobile"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="60%">
                                                        <asp:TextBox ID="txtMobile" ValidationGroup="VGRequest" runat="server" MaxLength="12"
                                                            Width="45%" ToolTip="Responder Mobile"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ValidationGroup="VGRequest" ID="RequiredFieldValidator8"
                                                            runat="server" ErrorMessage="Enter Responder Mobile Number" ControlToValidate="txtMobile"
                                                            Display="None"> </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr width="100%">
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblClientCreditability" runat="server" CssClass="styleReqFieldLabel"
                                                            Text="Client Creditability"></asp:Label>
                                                    </td>
                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                        <asp:DropDownList ID="ddlClientCreditability" runat="server" Width="45%" ToolTip="Client Credibility">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                               
                                            </table>
                                        </td>
                                        <td width="50%" valign="top">
                                            <table align="center" cellpadding="1" cellspacing="1" width="100%">
                                                <tr>
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblValue" runat="server" CssClass="styleReqFieldLabel" Text="Asset Value"></asp:Label>
                                                    </td>
                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                        <asp:TextBox ValidationGroup="VGRequest" ID="txtValue" runat="server" MaxLength="15"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Width="55%" Style="text-align: right"
                                                            ToolTip="Asset Value"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txtValue"
                                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ValidationGroup="VGRequest" ID="RequiredFieldValidator7"
                                                            runat="server" ErrorMessage="Enter Asset Value" ControlToValidate="txtValue"
                                                            Display="None"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblCurrency" runat="server" CssClass="styleDisplayLabel" Text="Currency"></asp:Label>
                                                    </td>
                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                        <asp:TextBox ValidationGroup="VGRequest" ID="txtCurrency" ReadOnly="true" runat="server"
                                                            Width="200px" ToolTip="Currency"></asp:TextBox><asp:DropDownList ID="ddlCurrency"
                                                                runat="server" Width="205px" ValidationGroup="VGRequest" Visible="false">
                                                            </asp:DropDownList>
                                                        <asp:RequiredFieldValidator Display="None" ValidationGroup="VGRequest" ID="RequiredFieldValidator6"
                                                            CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ddlCurrency"
                                                            InitialValue="0" ErrorMessage="Select Currency"></asp:RequiredFieldValidator><%--   <asp:RequiredFieldValidator ValidationGroup="VGRequest" ID="RequiredFieldValidator8"
                                                    runat="server" ErrorMessage="Select Currency" ControlToValidate="txtCurrency" Display="None"></asp:RequiredFieldValidator>
                                            --%>
                                                    </td>
                                                </tr>
                                                <tr width="100%">
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblClientNetWorth" runat="server" CssClass="styleReqFieldLabel" Text="Client Net Worth"></asp:Label>
                                                    </td>
                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                        <asp:DropDownList ID="ddlClientNetWorth" runat="server" Width="45%" ToolTip="Client Net Worth">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="styleFieldLabel" valign="top" width="40%">
                                                        <asp:Label ID="lblFieldRespond" runat="server" CssClass="styleReqFieldLabel" Text="Field Respond"></asp:Label>
                                                    </td>
                                                    <td align="left" class="styleFieldAlign" style="width: 60%">
                                                        <asp:TextBox ValidationGroup="VGRequest" ID="txtFieldRespond" runat="server" Height="70px"
                                                            MaxLength="200" TextMode="MultiLine" Width="200px" onkeyup="maxlengthfortxt(200)"
                                                            ToolTip="Field Respond"></asp:TextBox><asp:RequiredFieldValidator ValidationGroup="VGRequest"
                                                                ID="RequiredFieldValidator9" runat="server" ErrorMessage="Enter Field Respond"
                                                                ControlToValidate="txtFieldRespond" Display="None"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td height="20px">
                                        </td>
                                    </tr>
                                    <tr width="100%">
                                        <td width="40%">
                                        </td>
                                        <td width="60%" class="styleFieldAlign">
                                            <asp:Button ID="btnRespSave" runat="server" CssClass="styleSubmitButton"
                                                ValidationGroup="VGRequest" OnClientClick="return fnCheckPageValidators('VGRequest');"
                                                Text="Save" ToolTip="Save" Visible="false" />
                                            <asp:Button ID="btnRespClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                OnClientClick="return ClearResponse();" Visible="false" Text="Clear" OnClick="btnRespClear_Click"
                                                ToolTip="Clear" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="Left">
                                            <asp:ValidationSummary HeaderText="Please correct the following validation(s):" ID="ValidationSummary1"
                                                runat="server" ValidationGroup="VGRespond" CssClass="styleMandatoryLabel" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10px" colspan="2">
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tpPDDT" runat="server" HeaderText="FIR Transaction Details" CssClass="tabpan"
                    BackColor="Red" Width="99%">
                    <HeaderTemplate>
                        FIR Transaction Details
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                            <%-- <asp:Panel id="pnlFIR" runat="server" ScrollBars="Auto" CssClass="stylePanel" Width="790px">                           
                                <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>--%>
                                            <div runat="server" id="gvGrigPDDT">
                                                <asp:GridView ID="gvPRDDT" runat="server" AutoGenerateColumns="False" Width="1024px"
                                                    BorderColor="Gray" DataKeyNames="FIR_Doc_Cat_ID,CollectedBy,Scandedby" CssClass="styleInfoLabel" OnRowDataBound="gvPRDDT_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="FIR TypeId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPRTID" runat="server" Text='<%# Bind("FIR_Doc_Cat_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FIR Type" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("FIR_Doc_Type") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="left" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FIR Description" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("FIR_Doc_Description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Collected By" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                             <asp:Label ID="lblCollectedBy" runat="server"  Visible="false"></asp:Label>
                                                        <asp:Label ID="lblColUser" Visible="false" runat="server" Text='<%# Bind("Collected_By") %>'></asp:Label>
                                                        
                                                        <uc2:Suggest ID="ddlCollectedBy" runat="server" ToolTip='<%# Bind("CollectedBy") %>'  ServiceMethod="GetCollectedby" AutoPostBack="true"
                                                            IsMandatory="false" OnItem_Selected="ddlCollectedBy_SelectedIndexChanged" />
                                                                <%--<asp:Label ID="txtColletedBy" runat="server" Text='<%# Bind("CollectedBy") %>'></asp:Label>
                                                                <asp:Label ID="lblColUser" Visible="false" runat="server" Text='<%# Bind("Collected_By") %>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Collected Date" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                            <asp:TextBox ID="txtCollectedDate" runat="server" Width="80px" Text='<%#Bind("GetDates") %>'>
                                                        </asp:TextBox>
                                                        <cc1:CalendarExtender ID="calCollectedDate" runat="server" Enabled="True" TargetControlID="txtCollectedDate"
                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Label ID="lblCollectedDate" Visible="false" runat="server" Text='<%#Bind("Collected_Date") %>'></asp:Label>
                                                                <%--<asp:Label ID="txtColletedDate" runat="server" Width="70px" Text='<%#Bind("GetDates")%>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scanned By" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                             <asp:Label ID="lblScannedBy" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblScanUser" runat="server" Visible="false" Text='<%# Bind("Scanned_By") %>'></asp:Label>
                                                        
                                                         <uc2:Suggest ID="ddlScannedBy" runat="server" ToolTip='<%# Bind("Scandedby") %>'  ServiceMethod="GetCollectedby" AutoPostBack="true"
                                                            IsMandatory="false" OnItem_Selected="ddlScannedBy_SelectedIndexChanged" />
                                                               <%-- <asp:Label ID="txtScannedBy" runat="server" Text='<%# Bind("Scandedby") %>'></asp:Label>
                                                                <asp:Label ID="lblScanUser" runat="server" Visible="false" Text='<%# Bind("Scanned_By") %>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scanned Date" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                             <asp:TextBox ID="txtScannedDate" runat="server" Width="80px" Text='<%# Bind("GetDates") %>'>
                                                        </asp:TextBox>
                                                        <cc1:CalendarExtender ID="calScannedDate" runat="server" Enabled="True" TargetControlID="txtScannedDate"
                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                        </cc1:CalendarExtender>
                                                         <%-- <asp:Label ID="lblScannedDate" Visible="false" runat="server" Text='<%#Bind("Scanned_Date") %>'></asp:Label>--%>
                                                                <%--<asp:Label ID="txtScannedDate" runat="server" Width="70px" Text='<%#Bind("GetDates")%>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                       <%-- <asp:TemplateField HeaderText="Document Value" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                  
                                                        <asp:DropDownList ID="ddlDocValue" runat="server" OnSelectedIndexChanged="ddlDocValue_SelectedIndexChanged"
                                                            AutoPostBack="true">                                                          
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center"  Width="10%" CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:TemplateField>--%>
                                                        
                                                        
                                                        <asp:TemplateField HeaderText="File Upload">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txOD" runat="server" Width="100px" MaxLength="500" Text='<%# Bind("Document_Path") %>'
                                                                    Visible="false"></asp:TextBox>
                                                                <cc1:AsyncFileUpload ID="asyFileUpload" OnClientUploadComplete="uploadComplete" runat="server"
                                                                    Width="175px" OnUploadedComplete="asyncFileUpload_UploadedComplete" />
                                                                <asp:Label runat="server" ID="myThrobber" Visible="false"></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hidThrobber" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scan Ref. No." Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtScan" runat="server" Width="95%" MaxLength="12" Enabled="false"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtScan"
                                                                    FilterType="Numbers, Custom , UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                    Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="View Document">
                                                            <ItemTemplate>
                                                                <%--<asp:LinkButton runat="server" ID="hyplnkView" OnClick="hyplnkView_Click" Text="View Document"></asp:LinkButton>--%>
                                                                <asp:ImageButton ID="hyplnkView" CommandArgument='<%# Bind("Scanned_Ref_No") %>'
                                                                    OnClick="hyplnkView_Click" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                    runat="server" />
                                                                <asp:Label runat="server" ID="lblPath" Text='<%# Eval("Scanned_Ref_No")%>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemarks" runat="server" Width="250px" TextMode="MultiLine" onkeyup="maxlengthfortxt(150)"
                                                                    MaxLength="150"></asp:TextBox>                                                               
                                                                <asp:Label ID="lblProgramName" runat="server" Visible="false" Text='<%# Eval("ProgramName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Collect">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CbxCheck" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <%--   <table style="border-collapse: collapse; border-left: solid 1px #aaaaff; border-top: solid 1px #aaaaff;"
                                        runat="server" cellpadding="3" id="clientSide" />
                                    <asp:Label runat="server" Text="&nbsp;" ID="uploadResult" />--%>
                                       <%-- </td>
                                    </tr>
                                </table>
                                </asp:Panel>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td height="10px">
                    </td>
                </tr>
                <tr width="100%">
                    <td align="center" width="10%">
                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            OnClientClick="return fnCheckPageValidators('VGRequest');" Text="Save" ToolTip="Save" />
                        <asp:Button ID="btnGeneratePdf" runat="server" CssClass="styleSubmitButton" OnClick="btnGeneratePdf_Click"
                            ValidationGroup="VGPreview" Text="Mail Preview" ToolTip="Preview" /><%--OnClientClick="return FillMailPreview();"--%>
                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            OnClick="btnclear_onclick" Text="Clear" ToolTip="Clear" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" OnClientClick="return confirm('Are you sure want to cancel this record? Once you cancel you cannot revert it back.');"
                            Text="Cancel FIR" Enabled="false" ToolTip="Cancel FIR" />
                        <asp:Button ID="btnCancelToTransLander" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            OnClick="btnCancelToTransLander_Click" Text="Cancel" ToolTip="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td height="10px">
                    </td>
                </tr>
                <tr>
                    <td align="Left">
                        <asp:ValidationSummary HeaderText="Please correct the following validation(s):" ID="vsFIR"
                            runat="server" ValidationGroup="VGRequest" CssClass="styleMandatoryLabel" />
                        <asp:ValidationSummary HeaderText="Please correct the following validation(s):" ID="ValidationSummary2"
                            runat="server" ValidationGroup="VGPreview" CssClass="styleMandatoryLabel" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" />
                    </td>
                </tr>
                <tr>
                    <td height="10px">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Panel ID="PnlLetterPreview" Style="display: none" runat="server" Height="80%"
                    BackColor="White" BorderStyle="Solid" BorderColor="Black" Width="50%">
                    <table width="100%">
                        <tr>
                            <td colspan="2" class="stylePageHeading">
                                <asp:Label runat="server" Text="Mail Preview" ID="lblMailPreview" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="5px" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="styleFieldLabel" width="10%" valign="top">
                                <asp:Label runat="server" Text="To" ID="lblTo" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtTo" ReadOnly="true" EnableTheming="true" runat="server" Width="99%"
                                    MaxLength="12"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="styleFieldLabel" width="10%" valign="top">
                                <asp:Label runat="server" Text="Subject" ID="lblSubject" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtSubject" runat="server" Width="99%" Text="" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="2px" colspan="2">
                            </td>
                        </tr>
                        <tr height="50%" valign="top">
                            <td colspan="2" height="50%" valign="top">
                                <asp:TextBox ID="txtBody" TextMode="MultiLine" runat="server" Width="99%" Height="300px"
                                    ReadOnly="true" Style="text-align: justify;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px" colspan="2">
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center" colspan="2">
                                <asp:Button runat="server" ID="btnGenereatePreviewPDF" Text="Create Pdf" OnClick="PreviewPDF_Click"
                                    CausesValidation="False" CssClass="styleSubmitButton" />
                                <asp:Button runat="server" ID="btnSendMail" Text="Send Mail" OnClick="btnSendMail_Click"
                                    CausesValidation="False" CssClass="styleSubmitButton" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="styleSubmitButton"
                                    OnClick="btnClosePreview_Click" />
                                <asp:Button ID="btnModal" runat="server" Width="0px" Height="0px" />
                                <cc1:ModalPopupExtender ID="ModalPopupExtenderMailPreview" runat="server" TargetControlID="btnModal"
                                    PopupControlID="PnlLetterPreview" BackgroundCssClass="styleModalBackground" DynamicServicePath=""
                                    Enabled="True">
                                </cc1:ModalPopupExtender>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PnlRemarks" Style="display: none" runat="server" BackColor="White"
                    BorderStyle="Solid" BorderColor="Black">
                    <table width="45%">
                        <tr>
                            <td colspan="2" class="stylePageHeading">
                                <asp:Label runat="server" Text="Remarks" ID="Label4" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="5px" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="styleFieldLabel" width="10%" valign="top">
                                <asp:Label runat="server" Text="Remarks" ID="Label5" CssClass="styleReqFieldLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtRemarks" EnableTheming="true" runat="server" Width="95%" ToolTip="Remarks"
                                    TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtRemarks"
                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Remarks" ValidationGroup="VGRemarks"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="5px" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="VGRemarks"
                                    CssClass="styleMandatoryLabel" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center" colspan="2">
                                <asp:Button ID="BtnMPCancel" runat="server" Text="Cancel FIR" OnClick="BtnMPCancel_Click"
                                    CssClass="styleSubmitButton" ToolTip="Cancel FIR" OnClientClick="return fnCheckPageValidators('VGRemarks','false');" />
                                <asp:Button ID="BtnMPRemarks" runat="server" Width="0px" Height="0px" />
                                <cc1:ModalPopupExtender ID="ModalPopupRemarks" runat="server" TargetControlID="BtnMPRemarks"
                                    PopupControlID="PnlRemarks" BackgroundCssClass="styleModalBackground" DynamicServicePath=""
                                    Enabled="True">
                                </cc1:ModalPopupExtender>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="MaxVerChk" runat="server" />
    <input type="hidden" id="hdntxtSubject" runat="server" />
</asp:Content>
