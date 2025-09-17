﻿<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="Collection_S3GClnECSAuthorization, App_Web_f2u5fcxj" title="Untitled Page" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="4">
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="ECS Authorization" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                    <span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Fixed Billing Date" ID="lblFixedBillingDate" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtFBDate" ReadOnly="true" Width="160px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtBranch" ReadOnly="true" Width="160px" runat="server" />
                                    <asp:HiddenField ID="hidBranch" runat="server" Value="0" />
                                    <%--<asp:DropDownList ID="ddlBranch" runat="server" Width="165px" 
                            onselectedindexchanged="ddlBranch_SelectedIndexChanged" AutoPostBack ="true" >
                        </asp:DropDownList>--%>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Authorized By" ID="lblAuthorizedBy" CssClass="styleDisplayLabel"></asp:Label>
                                    <span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtAuthorizedBy" runat="server" ContentEditable="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddlAuthUser" runat="server" Width="165px" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="ECS Number" ID="lblECSNo" CssClass="styleDisplayLabel"></asp:Label>
                                    <span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlECSNo" runat="server" Width="165px" OnSelectedIndexChanged="ddlECSNo_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Authorized Date" ID="lblAuthorizeDate" CssClass="styleDisplayLabel"></asp:Label>
                                    <span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtAuthorizeDate" runat="server" Width="160px" ContentEditable="false"></asp:TextBox>
                                    <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtAuthorizeDate"
                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image1"
                            ID="CalendarExtender2" Enabled="True">
                        </cc1:CalendarExtender>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Password" ID="lblPassword" CssClass="styleDisplayLabel"></asp:Label>
                                    <span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtPassword" MaxLength="15" TextMode="Password" Width="160px" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlGrid" runat="server" CssClass="stylePanel" Width="60%" GroupingText="ECS Process Details"
                                        Visible="false">
                                        <asp:GridView Width="100%" ID="grvEcsProcess" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                            RowStyle-HorizontalAlign="Left" AutoGenerateColumns="false">
                                            <RowStyle HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %> '></asp:Label>
                                                        <asp:Label ID="lblCustomerId" Visible="false" runat="server" Text='<%#Eval("Customer_Id")%>'></asp:Label>
                                                        <%-- <asp:Label ID="lblInstallmentNo" Visible="false" runat="server" Text='<%#Eval("Installment_Number")%>'></asp:Label>
                                            <asp:Label ID="lblRepayId" Visible="false" runat="server" Text='<%#Eval("RepayId")%>'></asp:Label>
                                            <asp:Label ID="lblBranchId" Visible="false" runat="server" Text='<%#Eval("Branch_ID")%>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Center" DataField="Customer_Name"
                                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%" />
                                                <asp:BoundField HeaderText="Installment Amount" HeaderStyle-HorizontalAlign="Center"
                                                    DataField="Installment_Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%" />
                                                <asp:BoundField HeaderText="Bank MICR No" HeaderStyle-HorizontalAlign="Center" DataField="MICR_Code"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                                <asp:BoundField HeaderText="Bank Account Type" HeaderStyle-HorizontalAlign="Center"
                                                    DataField="Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" />
                                                <asp:BoundField HeaderText="Receipt Number" HeaderStyle-HorizontalAlign="Center"
                                                    DataField="Receipt_No" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%" />
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    &nbsp;<asp:Button ID="btnAuthorize" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                                        Text="Authorize" ValidationGroup="btnGetLines" OnClick="btnAuthorize_Click" OnClientClick="return fnCheckPageValidators('btnGetLines');" />
                                    &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                        OnClientClick="return fnConfirmClear();" Text="Clear" OnClick="btnClear_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                        OnClick="btnCancel_Click" Text="Cancel" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ControlToValidate="ddlLOB"
                ErrorMessage="Select the Line of Business" CssClass="styleMandatoryLabel" Display="None"
                InitialValue="0" SetFocusOnError="True" ValidationGroup="btnGetLines"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="rfvddlECSNo" runat="server" ControlToValidate="ddlECSNo"
                ErrorMessage="Select the ECS Number" CssClass="styleMandatoryLabel" Display="None"
                InitialValue="0" SetFocusOnError="True" ValidationGroup="btnGetLines"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="rfvddlAuthUser" runat="server" ControlToValidate="txtAuthorizedBy"
                ErrorMessage="Select Authorized By" CssClass="styleMandatoryLabel" Display="None"
                SetFocusOnError="True" ValidationGroup="btnGetLines"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="rfvtxtAuthorizeDate" runat="server" CssClass="styleMandatoryLabel"
                Display="None" ErrorMessage="Enter Authorized Date" SetFocusOnError="True" ControlToValidate="txtAuthorizeDate"
                ValidationGroup="btnGetLines"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" CssClass="styleMandatoryLabel"
                runat="server" ControlToValidate="txtPassword" ErrorMessage="Enter the Password"
                Display="None"></asp:RequiredFieldValidator>
            <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnGetLines" />
            <asp:CustomValidator ID="cvECSAuthorization" runat="server" CssClass="styleMandatoryLabel"
                Enabled="true" Width="98%" />
            <input type="hidden" id="hidDelete" runat="server" value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
