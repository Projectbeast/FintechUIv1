﻿<%@ page language="C#" masterpagefile="~/Common/FAMasterPageCollapse.master" autoeventwireup="true" inherits="FinancialAccounting_FAAuthorisationRuleCard, App_Web_4hds5vgj" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function ChkToamountValidate(txtFrm, txtTo) {
            var frmamt = 'ctl00_ContentPlaceHolder1_' + txtFrm;
            var toamt = 'ctl00_ContentPlaceHolder1_' + txtTo;
            var txtfrmamt = document.getElementById(frmamt).value;
            var txttoamt = document.getElementById(toamt).value;
            if (txtfrmamt != "") {
                if (parseFloat(txttoamt) < parseFloat(txtfrmamt)) {
                    document.getElementById(toamt).value = "";
                    alert("ToAmount Should Be Greater Than FromAmount");
                    document.getElementById(toamt).focus();
                }
                else
                    return true;
            }
            else {
                document.getElementById(toamt).value = "";
                alert("Shoud Be Enter FromAmount Before Enter ToAmount");
                document.getElementById(frmamt).focus();
            }
        }
        function ValidateMandatory1() {
            TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_grvAuthorizationrulecardDetail');
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var TotalScore = 0;

            var ddlProgram;
            var ddlApproverType;
            ddlProgram = document.getElementById('<%=ddlprogram.ClientID %>');
       ddlApproverType = document.getElementById('<%=ddlEntityType.ClientID %>');

    if (ddlProgram.value == '0') {
        alert('Select Program');
        return false;
    }
    if (ddlApproverType.value == '-1') {
        alert('Select Approver Type');
        return false;
    }


    for (var n = 0; n < Inputs.length; ++n) {
        if (Inputs[n].type == 'text') {
            if (Inputs[n].value != '') {
                if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 13) == 'AddFromAmount') {
                    var fromamount = parseFloat(Inputs[n].value)
                }
                if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 11) == 'AddToAmount') {
                    var Toamount = parseFloat(Inputs[n].value)
                    if (fromamount >= Toamount) {
                        document.getElementById(Inputs[n].id).focus();
                        alert(' To Amount should be greater than From Amount ');
                        return false;
                    }
                }

            }
            else {
                if (n == 0) {
                    document.getElementById(Inputs[n].id).focus();
                    alert('Enter From Amount');
                    return false;
                }
                if (n == 1) {
                    document.getElementById(Inputs[n].id).focus();
                    alert('Enter To Amount');
                    return false;
                }
                if (n == 2) {
                    document.getElementById(Inputs[n].id).focus();
                    alert('Enter Total Approval(s)');
                    return false;
                }
                if (n == 3) {
                    document.getElementById(Inputs[n].id).focus();
                    alert('Enter Level 4 Approval(s)');
                    return false;
                }
            }

        }

    }
}

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table class="stylePageHeading">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label runat="server" Text="Activity" ID="lblActivity" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlActivity" runat="server" Width="196px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:RequiredFieldValidator ID="rfvActivity" ValidationGroup="main" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlActivity" InitialValue="0" ErrorMessage="Select Activity"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--<tr>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label runat="server" Text="Constitution" ID="lblConstitution" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlConstitution" runat="server" Width="196px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label runat="server" Text="Product" ID="lblproduct" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlproduct" runat="server" Width="196px">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label runat="server" Text="Transaction Type" ID="lbltrasactionType" Visible="false" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlTransacType" Visible="false" runat="server" Width="196px">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="ddlTransacType" InitialValue="0" ErrorMessage="Select Transaction Type"
                                        Display="None">
                                    </asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label runat="server" Text="Program" ID="lblprogram" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlprogram" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="main" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="ddlprogram" InitialValue="0" ErrorMessage="Select Program"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label runat="server" Text="Approver Type" ID="lblEntityType" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlEntityType" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Designation" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="User Level" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="User Name" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:RequiredFieldValidator ID="rfvEntityType" CssClass="styleMandatoryLabel" runat="server"
                                        InitialValue="-1" ControlToValidate="ddlEntityType" ValidationGroup="main" ErrorMessage="Select Approver Type"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="Label1" runat="server" Text="Active"></asp:Label>&nbsp;
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:CheckBox ID="ChkActive" runat="server" Checked="True" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" width="100%">
                                    <asp:Panel ID="panDateFormat" GroupingText="Approval Rule Card Details" runat="server"
                                        Width="100%" CssClass="stylePanel">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <br />
                                                    <asp:GridView ID="grvAuthorizationrulecardDetail" ShowFooter="true" AutoGenerateColumns="false"
                                                        runat="server" OnRowCommand="grvAuthorizationrulecardDetail_RowCommand" OnRowDataBound="grvAuthorizationrulecardDetail_RowDataBound"
                                                        OnRowDeleting="grvAuthorizationrulecardDetail_RowDeleting" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No." ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%# Bind("SNo") %>'></asp:Label>
                                                                    <asp:Label runat="server" ID="Label1" ToolTip="S.No." Width="70%" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Amount">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtFromAmount" MaxLength="12" EnableViewState="true" ToolTip="From Amount" Text='<%# Bind("From_Amount")%>'
                                                                        Width="60%" Style="text-align: right" runat="server"></asp:TextBox>
                                                                    <asp:Label ID="lblRulecardDetailID" runat="server" Text='<%# Bind("AUTHORIZATION_RULE_CARD_DTL_ID")%>'
                                                                        Visible="false"></asp:Label>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtFromAmount"
                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30%" HorizontalAlign="Center" />
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAddFromAmount" ToolTip="From Amount" MaxLength="12" runat="server" Style="text-align: right"
                                                                        Width="60%"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtAddFromAmount"
                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <FooterStyle Width="30%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Amount">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtToAmount" MaxLength="12" ToolTip="To Amount" Width="60%" Style="text-align: right"
                                                                        Text='<%# Bind("To_Amount")%>' runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtToAmount"
                                                                        FilterType="Numbers" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30%" HorizontalAlign="Center" />
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAddToAmount" MaxLength="12" ToolTip="To Amount" runat="server" Width="60%" Style="text-align: right">
                                                                    </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtAddToAmount"
                                                                        FilterType="Numbers" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <FooterStyle Width="30%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--      <asp:TemplateField HeaderText="Total Approvals">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txttotalapproval" ToolTip="Total Approval" MaxLength="1" Text='<%# Bind("Total_Approvals")%>'
                                                                        Width="20%" Style="text-align: right" runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txttotalapproval"
                                                                        FilterType="Numbers" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAddtotalapprovals" ToolTip="Total Approval" MaxLength="1" runat="server" Width="20%" Style="text-align: right">
                                                                    </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtAddtotalapprovals"
                                                                        FilterType="Numbers" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Level 4 Approvals">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtlevel4approvals" ToolTip="Level 4 Approval" MaxLength="1" Text='<%# Bind("Level4_Approvals")%>' Width="20%"
                                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtlevel4approvals"
                                                                        FilterType="Numbers" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAddlevel4approvals" ToolTip="Level 4 Approval" MaxLength="1" runat="server" Width="20%" Style="text-align: right">
                                                                    </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtAddlevel4approvals"
                                                                        FilterType="Numbers" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Approver">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnIApprover" CssClass="styleSubmitShortButton" runat="server" Text="Approver"
                                                                        OnClick="btnIApprover_Click" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnFApprover" CssClass="styleSubmitShortButton" runat="server" Text="Approver"
                                                                        OnClick="btnFApprover_Click" OnClientClick="return ValidateMandatory1();" />
                                                                </FooterTemplate>
                                                                <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnRemove" Text="Remove" ToolTip="Remove" CommandName="Delete" runat="server"
                                                                        CausesValidation="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnDetails" Text="Add" ToolTip="Add"
                                                                        CommandName="AddNew" runat="server" CssClass="styleGridShortButton" CausesValidation="false" />
                                                                </FooterTemplate>
                                                                <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transaction" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txttrans" ToolTip="Level 4 Approval" Text='<%# Bind("trans")%>' Width="20%"
                                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Center" />

                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Button ID="btnReset" Visible="false" CssClass="styleSubmitButton" Text="Reset"
                                                        runat="server" CausesValidation="false" OnClick="btnReset_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <br />
                                    <asp:Button runat="server" ID="btnSave" ValidationGroup="main" ToolTip="Save,Alt+S" OnClientClick="return fnCheckPageValidators('main');"
                                        CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" AccessKey="S"  />
                                    <asp:Button ID="btnClear" class="styleSubmitButton" runat="server" ToolTip="Clear_FA,Alt+L" Text="Clear_FA"
                                        OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" CausesValidation="false" AccessKey="L" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Exit" ToolTip="cancel,Alt+x" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" AccessKey="X" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td colspan="4">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="AuthorizationRulecardAdd" runat="server" ValidationGroup="main" ShowSummary="true"
                                    HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                                <input id="hdnGrvCnt" runat="server" value="0" type="hidden" />
                            </td>
                        </tr>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="ModalPopupExtenderApprover" runat="server" TargetControlID="btnModal"
        PopupControlID="PnlApprover" BackgroundCssClass="styleModalBackground" Enabled="true">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="PnlApprover" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="850px">
        <asp:UpdatePanel ID="upPass" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td width="100%">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <div id="divTransaction" class="container" runat="server" style="height: 200px; overflow: scroll">
                                            <asp:GridView ID="grvApprover" runat="server" AutoGenerateColumns="false" Height="50px"
                                                OnRowDataBound="grvApprover_DataBound" OnRowDeleting="grvApprover_RowDeleting"
                                                ShowFooter="true">
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSNo" Text='<% #Bind("SNo")%>' Visible="false" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sequence Number" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSequenceNumber" ToolTip="Sequence Number" Text='<% #Bind("SequenceNumber")%>'
                                                                runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Approval Entity" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalEntity" Visible="false" runat="server" Text='<% #Bind("ApprovEntity") %>'>
                                                            </asp:Label>
                                                            <asp:DropDownList ID="ddlApprovalEntity" ToolTip="Approval Entity" runat="server"
                                                                Visible="false">
                                                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="Designation" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="User Level" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="User Name" Value="3"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Branch">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocation" runat="server" Text='<% #Bind("Location")%>'>
                                                            </asp:Label>
                                                            <asp:Label ID="lblLocationID" runat="server" Visible="false" Text='<% #Bind("LocationID")%>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlLocation" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                                ToolTip="Branch" runat="server" Style="width: 340px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ValidationGroup="PopUpAdd"
                                                                InitialValue="0" ControlToValidate="ddlLocation" Display="None" ErrorMessage="Select Branch"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Authority">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalPerson" runat="server" Text='<% #Bind("ApprovPerson")%>'>
                                                            </asp:Label>
                                                            <asp:Label ID="ApprovPersonID" runat="server" Visible="false" Text='<% #Bind("ApprovPersonID")%>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlApprovPerson" ToolTip="Approval Authority" runat="server"
                                                                Style="width: 340px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvApprovalPerson" runat="server" ValidationGroup="PopUpAdd"
                                                                ControlToValidate="ddlApprovPerson" Display="None" InitialValue="0" ErrorMessage="Select Approval Authority"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnDelete" Text="Delete" CommandName="Delete" runat="server"
                                                                CausesValidation="false" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnDetails" Text="Add" ValidationGroup="PopUpAdd" CommandName="Add"
                                                                runat="server" CssClass="styleGridShortButton" CausesValidation="false" OnClick="btnDetails_Click"
                                                                OnClientClick="return fnCheckPageValidators('PopUpAdd',false);" />
                                                        </FooterTemplate>
                                                        <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnDEVModalAdd" runat="server" Text="Save" ToolTip="Save" class="styleSubmitButton"
                                            OnClick="btnDEVModalAdd_Click" />
                                        <asp:Button ID="btnDEVModalCancel" runat="server" Text="Close" OnClick="btnDEVModalCancel_Click"
                                            ToolTip="Close" class="styleSubmitButton" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="vsPopUp" runat="server" ShowSummary="true" ValidationGroup="PopUpAdd"
                                            HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Button ID="btnModal" Style="display: none" runat="server" />

</asp:Content>

