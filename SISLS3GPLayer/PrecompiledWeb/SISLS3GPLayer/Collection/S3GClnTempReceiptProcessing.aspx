﻿<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="Collection_S3GClnTempReceiptProcessing, App_Web_4kkykzxm" title="Untitled Page" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Receipt Processing" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcReceipt" runat="server" CssClass="styleTabPanel" ScrollBars="None"
                            Width="98%" ActiveTabIndex="0" AutoPostBack="True">
                            <cc1:TabPanel ID="TabMainPage" runat="server" BackColor="Red" CssClass="tabpan" HeaderText="General"
                                Width="98%">
                                <HeaderTemplate>
                                    General</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td width="50%" valign="top">
                                                        <asp:Panel ID="PnlReceiptInfo" runat="server" GroupingText="Receipt Information"
                                                            CssClass="stylePanel" Width="99%">
                                                            <table width="100%" border="0">
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                            AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                            Display="None" InitialValue="0" ErrorMessage="Select a Line of Business" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%--<asp:DropDownList ID="ddlBranch" runat="server" Width="165px" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                            AutoPostBack="True" Style="height: 22px">
                                                                        </asp:DropDownList>--%>
                                                                         <uc:Suggest ID="ddlBranch" ToolTip="Location" runat="server" AutoPostBack="True" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                        ServiceMethod="GetBranchList" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Select a Location"  WatermarkText="--Select--" />
                                                                       <%-- <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                                            Display="None" ErrorMessage="Select a Location" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvShowBranch" runat="server" ControlToValidate="ddlBranch"
                                                                            Display="None" ErrorMessage="Select a Location" InitialValue="0" ValidationGroup="Show"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblPaymentMode" runat="server" CssClass="styleReqFieldLabel" Text="Mode"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlPaymentMode" AutoPostBack="True" runat="server" Width="165px"
                                                                            OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvMode" runat="server" ControlToValidate="ddlPaymentMode"
                                                                            Display="None" ErrorMessage="Select a Mode" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblDocNo" runat="server" CssClass="styleReqFieldLabel" Text="Doc No"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtDocNo" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblDocDate" runat="server" CssClass="styleReqFieldLabel" Text="Doc Date"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtDocDate" runat="server"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                            ID="ftxtDocDate" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtDocDate" ValidChars="/-">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <cc1:CalendarExtender ID="calDocDate" runat="server" Enabled="True" Format="DD/MM/YYYY"
                                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDocdate"
                                                                            TargetControlID="txtDocDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:Image ID="imgDocdate" runat="server" ImageUrl="~/Images/calendaer.gif" /><asp:RequiredFieldValidator
                                                                            ID="rfvDocDate" runat="server" ControlToValidate="txtDocDate" CssClass="styleMandatoryLabel"
                                                                            Display="None" ErrorMessage="Enter Doc Date" SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblDocAmount" runat="server" CssClass="styleReqFieldLabel" Text="Doc Amount"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtDocAmount" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvDocAmount" runat="server" ControlToValidate="txtDocAmount"
                                                                            Display="None" ErrorMessage="Enter a Doc Amount" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblValueDate" runat="server" CssClass="styleReqFieldLabel" Text="Value Date"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtValueDate" runat="server" AutoPostBack="true" OnTextChanged="txtValueDate_TextChanged"></asp:TextBox><cc1:CalendarExtender
                                                                            ID="calValueDate" runat="server" Enabled="True" Format="DD/MM/YYYY" PopupButtonID="imgValueDate"
                                                                            TargetControlID="txtValueDate">
                                                                        </cc1:CalendarExtender>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtValueDate" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtValueDate" ValidChars="/-">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:Image ID="imgValueDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                        <asp:RequiredFieldValidator ID="rfvValueDate" runat="server" ControlToValidate="txtValueDate"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Value Date"
                                                                            SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblBookSINo" runat="server" CssClass="styleReqFieldLabel" Text="Book No"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlBookNo" runat="server" Width="165px" OnSelectedIndexChanged="ddlBookNo_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvBookNo" runat="server" ControlToValidate="ddlBookNo"
                                                                            Display="None" InitialValue="0" ErrorMessage="Select a Book No" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblBookPageNo" runat="server" CssClass="styleReqFieldLabel" Text="Page No"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlBookPageNo" runat="server" Width="165px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvPageNo" runat="server" ControlToValidate="ddlBookPageNo"
                                                                            Display="None" InitialValue="0" ErrorMessage="Select a Page No" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblReason" runat="server" CssClass="styleReqFieldLabel" Text="Reason"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlReason" runat="server" Width="165px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="ddlReason"
                                                                            Display="None" ErrorMessage="Select a Reason" InitialValue="0" ValidationGroup="Cancel"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                    <td width="50%" valign="top">
                                                        <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Informations"
                                                            CssClass="stylePanel" Width="99%">
                                                            <table width="100%" border="0" cellspacing="0">
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="35%">
                                                                        <asp:Label runat="server" ID="lblCustomerName" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <uc2:LOV ID="ucCustomerCodeLov" runat="server" strLOV_Code="CMD" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" width="100%">
                                                                        <asp:Button ID="btnLoadCustomer" runat="server" OnClick="btnLoadCustomer_OnClick"
                                                                            Style="display: none;" Text="Load Customer" CausesValidation="false" />
                                                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                            SecondColumnStyle="styleFieldAlign" ShowCustomerName="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" width="100%">
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabControlDataSheet" runat="server" BackColor="Red" CssClass="tabpan"
                                HeaderText="Drawee Bank" Width="98%">
                                <HeaderTemplate>
                                    Drawee Bank</HeaderTemplate>
                                <ContentTemplate>
                                    <table align="center" width="98%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblInstrumentNo" runat="server" CssClass="styleDisplayLabel" Text="Instrument No"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtInstrumentNo" runat="server" onkeypress="return NumericCheck(event)"
                                                    MaxLength="6"></asp:TextBox>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblpaymentGateway" runat="server" CssClass="styleReqdFieldLabel" Text="Payment Gateway Ref No"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtPaymentGateway" runat="server" MaxLength="20"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftxtPayment" runat="server" Enabled="True" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtPaymentGateway" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblInstrumentDate" runat="server" CssClass="styleDisplayLabel" Text="Instrument Date"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtInstrumentDate" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                    ID="ftxtInstrumentDate" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtInstrumentDate" ValidChars="/-">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:CalendarExtender ID="calInstrumentDate" runat="server" Enabled="True" Format="DD/MM/YYYY"
                                                    PopupButtonID="imgInstrumentDate" TargetControlID="txtInstrumentDate">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgInstrumentDate" runat="server" ImageUrl="~/Images/calendaer.gif" /><asp:RequiredFieldValidator
                                                    ID="rfvInstrumentDate" runat="server" ControlToValidate="txtInstrumentDate" CssClass="styleMandatoryLabel"
                                                    Display="None" Enabled="False" ErrorMessage="Enter Instrument Date" SetFocusOnError="True"
                                                    ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblACKNo" runat="server" CssClass="styleDisplayLabel" Text="ACK No"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtACKNo" runat="server" MaxLength="12"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftxtACKNo" runat="server" Enabled="True" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtACKNo" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblLocation" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtLocation" runat="server" MaxLength="40"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftxtLocation" runat="server" Enabled="True" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtLocation" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblDraweeBank" runat="server" CssClass="styleDisplayLabel" Text="Drawee Bank"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlDraweeBank" runat="server" Width="165px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlDraweeBank_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvDraweeBank" runat="server" ControlToValidate="ddlDraweeBank"
                                                    Display="None" InitialValue="0" Enabled="False" ErrorMessage="Select a Drawee Bank"
                                                    ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                &nbsp;
                                            </td>
                                            <td class="styleFieldAlign">
                                                &nbsp;
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:Label ID="lblBankName" runat="server" Text="BankName" Visible="False"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="TxtBank" runat="server" Visible="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
            </table>
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                            Text="Save" ValidationGroup="Submit" OnClientClick="return fnCheckPageValidators('Submit');"
                            OnClick="btnSave_OnClick" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Clear" OnClick="btnClear_OnClick" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Cancel" OnClick="btnCancel_OnClick" />
                        &nbsp;<asp:Button ID="btnTempReceiptCancel" runat="server" CausesValidation="true"
                            ValidationGroup="Cancel" CssClass="styleSubmitButton" Text="Receipt Cancel" OnClick="btnTempReceiptCancel_OnClick" />
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="cvReceiptProcessing" runat="server" CssClass="styleMandatoryLabel"
                Enabled="true"></asp:CustomValidator>
            <asp:ValidationSummary ID="vsBilling" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="Submit" HeaderText="Correct the following validation(s):" />
            <asp:ValidationSummary ID="vsAddless" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="AddLess" HeaderText="Correct the following validation(s):" />
            <asp:ValidationSummary ID="vsAccountDetails" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="AccountDetails" HeaderText="Correct the following validation(s):" />
            <asp:ValidationSummary ID="vsShowMethod" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="Show" HeaderText="Correct the following validation(s):" />
            <asp:ValidationSummary ID="vsCancel" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="Cancel" HeaderText="Correct the following validation(s):" />
            <asp:HiddenField ID="hdvAmount" runat="server" />
            <asp:HiddenField ID="hvfGLPostingCode" runat="server" />
            <asp:HiddenField ID="hvfCashFlowID" runat="server" />
            <asp:HiddenField ID="hvfCustomerID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">



        function NumericCheck(evt) {

            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);

            var regex = new RegExp("[0-9\r]", "g");


            if (key.match(regex) == null) {


                window.event.keyCode = 0
                return false;
            }

        }


        function AlphaNumericCheck(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);

            var regex = new RegExp("[0-9a-zA-Z\r]", "g");


            if (key.match(regex) == null) {


                window.event.keyCode = 0
                return false;
            }
        }

        function MaskMoney(evt) {
            alert(evt);
            alert(evt.keyCode);
            if (!(evt.keyCode == 46) || (evt.keyCode >= 48 && evt.keyCode <= 57)) return false;
            alert(evt.keyCode);
            var parts = evt.srcElement.value.split('.');
            if (parts.length > 2) return false;
            if (evt.keyCode == 46) return (parts.length == 1);
            if (parts[0].length >= 14) return false;
            if (parts.length == 2 && parts[1].length >= 2) return false;
        }




        function validate(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]|\.{0,3}/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
            }
            //    alert(theEvent.srcElement.value.indexOf('.'));
            //     var parts =theEvent.srcElement.value.indexOf('.')
            //     alert(theEvent.srcElement.value.length)
            //   return false;
            //     

            //    var parts=theEvent.srcElement.value.split('.');
            //    alert(parts)
            //    alert(parts[1].length)
            //     if(parts[1].length>=2) return false;

            //  theEvent.preventDefault();

        }





        function DisableControls() {
            var ddlPaymentMode = document.getElementById('<%=ddlPaymentMode.ClientID%>');
            var ddlDraweeBank = document.getElementById('<%=ddlDraweeBank.ClientID%>');
            var txtInstrumentDate = document.getElementById('<%=txtInstrumentDate.ClientID%>');
            var txtInstrumentNo = document.getElementById('<%=txtInstrumentNo.ClientID%>');
            var txtLocation = document.getElementById('<%=txtLocation.ClientID%>');
            var imgInstrumentDate = document.getElementById('<%=imgInstrumentDate.ClientID%>');

            //alert(ddlPaymentMode);



            if (ddlPaymentMode.options[ddlPaymentMode.selectedIndex].text == "Cash") {
                ddlDraweeBank.disabled = true;
                txtInstrumentDate.disabled = true;
                txtInstrumentNo.disabled = true;
                txtLocation.disabled = true;
                imgInstrumentDate.disable = true;
                document.getElementById('<%=ddlDraweeBank.ClientID%>').options.selectedIndex = 0;
            }
            else {
                ddlDraweeBank.disabled = false;
                txtInstrumentDate.disabled = false;
                txtInstrumentNo.disabled = false;
                imgInstrumentDate.disable = false;
                txtLocation.disabled = false;
                //document.getElementById('<%=ddlDraweeBank.ClientID%>').options.selectedIndex=0;
            }

        }

        function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_tcReceipt_TabMainPage_btnLoadCustomer').click();
        }
    </script>

</asp:Content>
