﻿<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="Collection_S3GClnECSFlatFileGeneration, App_Web_4kkykzxm" title="Untitled Page" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function fnReGenerateClick() {
            if (confirm('Are you sure you want to Re-Generate the ECS Flat file?')) {
                return true;
            }
            else {
                return false;
            }
        }
        </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="4">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="ECS Flat File Generation" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="100%">
                        <asp:Panel ID="pnlGeneral" runat="server" CssClass="stylePanel" GroupingText="Input Creteria">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">

                                <tr>

                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleDisplayLabel"></asp:Label>
                                        <asp:HiddenField ID="hdnLocationID" runat="server" Visible="false" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtBranch" runat="server" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="ECS No" ID="lblECSNo" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlECSNo" runat="server" Width="165px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlECSNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Bank Name" ID="lblBankName" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                                        <asp:Label runat="server" Text="Fixed Billing Date" ID="lblFixedBillingDate" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtBankName" runat="server" Visible="false" ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="txtFBDate" runat="server" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>

                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Bank Name" ID="lblBankCode" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlBankCode" runat="server" Width="165px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlBankCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="File Path" ID="lblFilePath" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtFilePath" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel"></td>
                                    <td class="styleFieldAlign"></td>
                                    <td class="styleFieldAlign" colspan="2">&nbsp;
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="4" width="100%">
                                        <asp:Panel ID="pnlGrid" runat="server" CssClass="stylePanel">
                                            <asp:GridView ID="grvECS" runat="server" HeaderStyle-CssClass="styleGridHeader" Width="100%"
                                                RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false">
                                                <RowStyle HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                            <asp:Label ID="lblPANum" Visible="false" runat="server" Text='<%#Eval("PANum")%>'></asp:Label>
                                                            <asp:Label ID="lblSANum" Visible="false" runat="server" Text='<%#Eval("SANum")%>'></asp:Label>                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Customer Name" DataField="Customer_Name" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField HeaderText="Receipt No" DataField="Receipt_No" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField HeaderText="Installment Amount" DataField="Installment_Amount" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField HeaderText="Bank MICR No" DataField="MICR_No" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField HeaderText="Bank Account No" DataField="Bank_Account_No" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField HeaderText="Bank Account Type" DataField="Name" ItemStyle-HorizontalAlign="Left" />
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">&nbsp;&nbsp;&nbsp;<asp:Button ID="btnGeneration" runat="server" CausesValidation="true"
                        OnClientClick="return fnCheckPageValidators();" CssClass="styleSubmitLongButton"
                        Text="Flat File Generation" ValidationGroup="btnGeneration" OnClick="btnGeneration_Click" Enabled="false" />
                        &nbsp;<asp:Button ID="btnReGenerate" runat="server" CausesValidation="true"
                        OnClientClick="return fnReGenerateClick();" CssClass="styleSubmitButton"
                        Text="Re-Generation" ValidationGroup="btnGeneration" OnClick="btnReGenerate_Click" Enabled="false" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="false" OnClientClick="return fnConfirmClear();"
                            CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                        <input type="hidden" runat="server" id="hidBrachCode" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                        OnClick="btnCancel_Click" Text="Cancel" />
                        &nbsp;&nbsp;&nbsp;

                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;
                        <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ControlToValidate="ddlLOB"
                            ErrorMessage="Select the Line of Business" CssClass="styleMandatoryLabel"
                            Display="None" InitialValue="0" SetFocusOnError="True" ValidationGroup="btnGeneration"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvddlBankCode" runat="server" ControlToValidate="ddlBankCode"
                            ErrorMessage="Select the Bank Name" CssClass="styleMandatoryLabel" Display="None"
                            InitialValue="0" SetFocusOnError="True" ValidationGroup="btnGeneration"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvddlECSNo" runat="server" ControlToValidate="ddlECSNo"
                            ErrorMessage="Select the ECS No" CssClass="styleMandatoryLabel" Display="None"
                            InitialValue="0" SetFocusOnError="True" ValidationGroup="btnGeneration"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvddlFilePath" runat="server" ControlToValidate="txtFilePath"
                            ErrorMessage="Enter the File Path" CssClass="styleMandatoryLabel" Display="None" Enabled="false"
                            SetFocusOnError="True" ValidationGroup="btnGeneration"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnGeneration" />
                        <asp:CustomValidator ID="cvECSFFG" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
