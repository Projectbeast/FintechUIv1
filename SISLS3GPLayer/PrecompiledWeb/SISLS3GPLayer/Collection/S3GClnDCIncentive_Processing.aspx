﻿<%@ page language="C#" autoeventwireup="true" masterpagefile="~/Common/S3GMasterPageCollapse.master" inherits="Collection_S3GClnDCIncentive_Processing, App_Web_f2u5fcxj" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <table width="99%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top" align="left">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td colspan="5" valign="top">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <div style="height: 1px;">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:Panel ID="pnlCommissionDetails" runat="server" CssClass="stylePanel" GroupingText="Debt Collector Details"
                                                                Width="99%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line Of Business"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlLineofBusiness" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLineofBusiness_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <%--<asp:RequiredFieldValidator ID="ReqDemandMonth0" runat="server" 
                                                        ControlToValidate="ddlLineofBusiness" ErrorMessage="LOB Required" 
                                                        InitialValue="0" SetFocusOnError="True" ValidationGroup="btnSave"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblFinancialYear" runat="server" Text="Processing Period" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlFinacialYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFinacialYear_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDemandMonth"
                                                                                CssClass="styleMandatoryLabel" ErrorMessage="Only Numbers Allowed" SetFocusOnError="True"
                                                                                ValidationExpression="\d{6}" ValidationGroup="btnSave">yyyymm/yyyy</asp:RegularExpressionValidator>--%>
                                                                            <asp:RequiredFieldValidator Display="None" ID="RFVFinancialyear" runat="server" ErrorMessage="Select Processing Period"
                                                                                SetFocusOnError="True" ControlToValidate="ddlFinacialYear" InitialValue="0" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblBranch" runat="server" Text="Location"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" TabIndex="2"
                                                                                OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <%-- <asp:RequiredFieldValidator ID="ReqDemandMonth1" runat="server" 
                                                        ControlToValidate="ddlBranch" ErrorMessage="Branch Required" InitialValue="0" 
                                                        SetFocusOnError="True" ValidationGroup="btnSave">*</asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblFrequency" runat="server" Text="Frequency" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlFrequency" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator Display="None" ID="RFVFrequency" runat="server" ErrorMessage="Select Frequency"
                                                                                SetFocusOnError="True" ControlToValidate="ddlFrequency" InitialValue="0" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblDebtCollectorCode" runat="server" Text="Debt Collector Code"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlDCCollectorCode" runat="server" AutoPostBack="true" TabIndex="4"
                                                                                OnSelectedIndexChanged="ddlDCCollectorCode_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblDemandMonth" runat="server" Text="Demand Month" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlDemandMonth" runat="server" TabIndex="3" MaxLength="6">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator Display="None" ID="RFVDemandMonth" runat="server" ErrorMessage="Select Demand Month"
                                                                                SetFocusOnError="True" ControlToValidate="ddlDemandMonth" InitialValue="0" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="center">
                                                                        <td colspan="4" align="center">
                                                                            <asp:Button ID="btnProcess" runat="server" CssClass="styleSubmitButton" Text="Process"
                                                                                ValidationGroup="btnSave" OnClick="btnProcess_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <asp:UpdatePanel ID="UpdatePanel_AssetAcquisition" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlCommissions" runat="server" CssClass="stylePanel" GroupingText="DC Commission Details"
                                                                        Width="99%" Visible="false">
                                                                        <asp:Label ID="lblgridErrorMessage" runat="server" />
                                                                        <div style="overflow-x: hidden; overflow-y: auto;" runat="server">
                                                                            <asp:GridView ID="gvDCIncentiveProcessing" runat="server" AutoGenerateColumns="false"
                                                                                Width="100%" ShowFooter="true">
                                                                                <Columns>
                                                                                    <%--<asp:BoundField HeaderText="Line of Business" DataField="LOB" Visible="true" ItemStyle-HorizontalAlign="Center" />--%>
                                                                                    <asp:BoundField HeaderText="LOBID" DataField="LOB_ID" Visible="false" ItemStyle-HorizontalAlign="Center" />
                                                                                    <%--<asp:BoundField HeaderText="Branch" DataField="Branch" Visible="true" ItemStyle-HorizontalAlign="Center" />--%>
                                                                                    <asp:BoundField HeaderText="BranchID" DataField="Branch_ID" Visible="false" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField HeaderText="Debt Collector Code" DataField="debtCollector_code" Visible="true"
                                                                                        ItemStyle-HorizontalAlign="Center" />
                                                                                    <%--<asp:BoundField HeaderText="Demand Month" DataField="Demand_Month" Visible="true"
                                                                                        ItemStyle-HorizontalAlign="Center" />--%>
                                                                                   <%-- <asp:BoundField HeaderText="Demand Month" DataField="Period" Visible="true"
                                                                                        ItemStyle-HorizontalAlign="Center" />--%>
                                                                                    <asp:BoundField HeaderText="Target Amount" DataField="TargetAmount" Visible="true"
                                                                                        ItemStyle-HorizontalAlign="right" />
                                                                                    <asp:BoundField HeaderText="Collection Amount" DataField="CollectionAmount" Visible="true"
                                                                                        ItemStyle-HorizontalAlign="right" />
                                                                                    <asp:BoundField HeaderText="Commission" DataField="Commission" Visible="true" ItemStyle-HorizontalAlign="right" />
                                                                                    <asp:BoundField HeaderText="Special Commission" DataField="SpecialCommission" Visible="true"
                                                                                        ItemStyle-HorizontalAlign="right" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <table width="99%">
        <tr align="center">
            <td colspan="5" align="center">
                <asp:Button runat="server" ID="btnSave" ValidationGroup="btnSave" CssClass="styleSubmitButton"
                    Text="Save" OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" />
                &nbsp;
                <asp:Button Text="Excel" ID="btnExcel" CausesValidation="true" runat="server" CssClass="styleSubmitButton"
                    OnClick="btnExcel_Click" ValidationGroup="btnSave" />
                &nbsp;
                <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                    CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                &nbsp;
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                    CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
            </td>
        </tr>
        <tr>
            <td colspan="5" align="center">
                <asp:CustomValidator ID="CVDCIncentiveProcessing" runat="server" Display="None" ValidationGroup="btnSave"></asp:CustomValidator>
            </td>
        </tr>
    </table>
</asp:Content>
