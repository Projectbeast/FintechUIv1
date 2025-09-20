<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GORGPaymentRuleCard_Add.aspx.cs"
    Inherits="Origination_S3GORGPaymentRuleCard" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput) {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
        function fnGetCompensationValue(CompensationValue) {
            var exp = /_/gi;
            return parseFloat(CompensationValue.value.replace(exp, '0'));
        }
        function CompensationPercentage_OnBlur(values) {
            var qMode = ('<%= Request.QueryString["qsMode"]%>');
            if (qMode.toLowerCase() != 'q') {
                document.getElementById('<%=rfvCompensationLevyPattern.ClientID%>').enabled = false;
                document.getElementById('<%=rfvFrequency.ClientID%>').enabled = false;

                document.getElementById('<%=lblCompensationLevyPattern.ClientID%>').className = "";
                document.getElementById('<%=lblFrequency.ClientID%>').className = "";
                var varCompensationValue = fnGetCompensationValue(values);
                if (varCompensationValue > 0) {
                    document.getElementById('<%=rfvCompensationLevyPattern.ClientID%>').enabled = true;
                    document.getElementById('<%=rfvFrequency.ClientID%>').enabled = true;

                    document.getElementById('<%=lblCompensationLevyPattern.ClientID%>').className = "styleReqFieldLabel";
                    document.getElementById('<%=lblFrequency.ClientID%>').className = "styleReqFieldLabel";
                }
            }
            //            alert(values.value);
            var exp = /_/gi;
            values.value = parseFloat(values.value.replace(exp, '0'));

            if (values.value == '0') {
                alert('Compensation % should not be Zero');
                values.value = "";
                values.focus();
                return false;
            }
        }

        function fncheckvalidpercentage(ObjCtrl) {
            if (ObjCtrl.value != '') {
                if (parseFloat(ObjCtrl.value) > 100) {
                    alert('Value should not be greater than 100%');
                    ObjCtrl.value = '';
                    ObjCtrl.focus();
                }
            }

        }

        function chkemptycompensation(input) {
            if (input.value == '') {
                alert('Compensation% cannot be zero or empty');
                //input.focus();
                return false;
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col">
                    <h6 class="title_name px-3 p-2">
                        <asp:Label runat="server" ID="lblHeading" ToolTip="Payment Rule Card" Text="Payment Rule Card" CssClass="styleInfoLabel">
                        </asp:Label>
                    </h6>
                </div>
                <div class="col mr-3">
                    <div class="float-right">
                        <button class="btn btn-outline-success btn-create" id="btnCancel" onserverclick="btnCancel_Click" causesvalidation="false" runat="server" onclick="if(fnConfirmExit())"
                            type="button" accesskey="X" title="Exit,Alt+X">
                            <i class="fa fa-reply"></i>&emsp;E<u>x</u>it
                        </button>
                    </div>
                </div>
            </div>
            <div id="tab-content" class="tab-content scrollable-content">
                <div class="row m-0">
                    <div class="col">
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblPaymentRuleNumber" runat="server" Text="Payment Rule Number" ToolTip="Payment Rule Number"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtPaymentRuleNumber" runat="server" Style="margin-left: 0px" ToolTip="Payment Rule Number"
                                        class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line Of Business" ToolTip="Line Of Business"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="ddlLOB1" runat="server" OnSelectedIndexChanged="ddlLOB1_SelectedIndexChanged"
                                        AutoPostBack="true" ToolTip="Line Of Business" class="md-form-control form-control">
                                    </asp:DropDownList>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                    <div class="validation_msg_box">
                                        <asp:RequiredFieldValidator ID="rfvLineOfBusiness" runat="server" Display="Dynamic"
                                            ControlToValidate="ddlLOB1" ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select Line of Business"
                                            CssClass="validation_msg_box_sapn" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblAccountType" runat="server" Text="Account Type" ToolTip="Account Type"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="ddlAccountType" runat="server" OnSelectedIndexChanged="AccountType_SelectedIndexChanged"
                                        AutoPostBack="true" ToolTip="Account Type" class="md-form-control form-control">
                                    </asp:DropDownList>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                    <div class="validation_msg_box">
                                        <asp:RequiredFieldValidator ID="rfvAccountType" runat="server" Display="Dynamic" ControlToValidate="ddlAccountType"
                                            ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select Account type"
                                            CssClass="validation_msg_box_sapn" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblProductName" runat="server" Text="Scheme Name" ToolTip="Scheme Name"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="ddlProductName" runat="server" ToolTip="Scheme Name" class="md-form-control form-control">
                                    </asp:DropDownList>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblEntityType" runat="server" Text="Entity Type" ToolTip="Entity Type"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="ddlEntityType" runat="server" ToolTip="Entity Type" class="md-form-control form-control">
                                    </asp:DropDownList>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                    <div class="validation_msg_box">
                                        <asp:RequiredFieldValidator ID="rfvEntityType" runat="server" Display="Dynamic" ControlToValidate="ddlEntityType"
                                            ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select Entity type"
                                            CssClass="validation_msg_box_sapn" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblCompensationPercentage" runat="server" Text="Compensation %" ToolTip="Compensation %"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtCompensationPercentage" Style="text-align: right" MaxLength="7"
                                        runat="server" ToolTip="Compensation %" class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                    <cc1:FilteredTextBoxExtender ID="ftxtCompensationPercentage" runat="server" TargetControlID="txtCompensationPercentage"
                                        FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                    </cc1:FilteredTextBoxExtender>
                                    <div class="validation_msg_box">
                                        <asp:RequiredFieldValidator ID="rfvCompensationPercentage" runat="server" CssClass="validation_msg_box_sapn"
                                            ControlToValidate="txtCompensationPercentage" ErrorMessage="Enter Compensation %" Enabled="false" ValidationGroup="btnSave" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblCompensationLevyPattern" runat="server" Text="Compensation Levy Pattern"
                                            ToolTip="Compensation Levy Pattern"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="ddlCompensationLevyPattern" runat="server" ToolTip="Compensation Levy Pattern" class="md-form-control form-control">
                                    </asp:DropDownList>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                    <div class="validation_msg_box">
                                        <asp:RequiredFieldValidator ID="rfvCompensationLevyPattern" runat="server" Display="Dynamic" CssClass="validation_msg_box_sapn"
                                            Enabled="false" ControlToValidate="ddlCompensationLevyPattern" ValidationGroup="btnSave"
                                            InitialValue="0" ErrorMessage="Select Compensation levy pattern"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblFrequency" runat="server" Text="Levy Frequency" ToolTip="Levy Frequency"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="ddlFrequency" runat="server" ToolTip="Levy Frequency" class="md-form-control form-control">
                                    </asp:DropDownList>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                    <div class="validation_msg_box">
                                        <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" Display="Dynamic" Enabled="false"
                                            ControlToValidate="ddlFrequency" ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select levy frequency"
                                            CssClass="validation_msg_box_sapn" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblOnTapRefinance" runat="server" Text="On Tap Refinance" ToolTip="On Tap Refinance"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="ddlOnTapRefinance" runat="server" ToolTip="On Tap Refinance" class="md-form-control form-control">
                                    </asp:DropDownList>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>
                                    <div class="validation_msg_box">
                                        <asp:RequiredFieldValidator ID="rfvOnTapRefinance" runat="server" Display="Dynamic" CssClass="validation_msg_box_sapn"
                                            ControlToValidate="ddlOnTapRefinance" ValidationGroup="btnSave" InitialValue="-1"
                                            ErrorMessage="Select On tap refinance" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                <div class="md-input md-check-list">
                                    <asp:CheckBox ID="chkActive" runat="server" ToolTip="Active Indicator" />
                                    <asp:Label ID="lblRate" runat="server" Text="Active" ToolTip="Active Indicator"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="p-2 pb-5 mb-4">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
            <%--<asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />--%>
        </div>
        <div class="fixed_btn" style="bottom: 9px;">
            <div class="col p-0">
                <button class="btn btn-success mr-2" id="btnSave" onserverclick="btnSave_Click" runat="server" onclick="if(fnCheckPageValidators('Save'))"
                    type="button" accesskey="S" causesvalidation="false" title="Save,Alt+S" validationgroup="btnSave">
                    <i class="fa fa-floppy-o"></i>&emsp;<u>S</u>ave
                </button>
                <button class="btn btn-outline-success" id="btnClear" onserverclick="btnClear_Click" causesvalidation="false" runat="server" onclick="if(fnConfirmClear())"
                    type="button" accesskey="L" title="Clear,Alt+L">
                    <i class="fa fa-refresh"></i>&emsp;C<u>l</u>ear
                </button>
            </div>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
