﻿<%@ page language="C#" autoeventwireup="true" inherits="Collection_S3gClnLoanToValueSpooling, App_Web_qmqntgub" masterpagefile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td colspan="4" class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" EnableViewState="false" Text="Loan To Value Spooling"
                                        CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr width="100%">
                    <td width="15%">
                        <asp:Label runat="server" Text="Line of Business" ToolTip="Line of Business" ID="lblLOBSearch" CssClass="styleReqFieldLabel" />
                    </td>
                    <td width="25%" align="left">
                        <asp:DropDownList ID="ddlLOBSearch" ToolTip="Line of Business" AutoPostBack="true" runat="server" MaxLength="0"
                            OnSelectedIndexChanged="ddlLOBSearch_SelectedIndexChanged" Width="175px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RFVddlLOB" InitialValue="0" CssClass="styleMandatoryLabel"
                            runat="server" ControlToValidate="ddlLOBSearch" SetFocusOnError="True" ErrorMessage="Select Line of Business"
                            Display="None"></asp:RequiredFieldValidator>
                    </td>
                    <td width="15%">
                        <asp:Label runat="server" Text="Report Date" ToolTip="Report Date" ID="lblReportDate" CssClass="styleDisplayLabel" />
                    </td>
                    <td width="35%" align="left">
                        <asp:TextBox ID="txtReportDate" ToolTip="Report Date" Enabled="false" runat="server" Width="170px"></asp:TextBox>
                    </td>
                </tr>
                <tr width="100%">
                    <td width="15%">
                        <asp:Label runat="server" Text="Location" ToolTip="Location" ID="lblGrvBranch" CssClass="styleDisplayLabel" />
                    </td>
                    <td width="35%" align="left">
                        <uc:Suggest ID="ddlBranch" ToolTip="Location" runat="server" AutoPostBack="True"
                            OnItem_Selected="ddlBranch_SelectedIndexChanged" ServiceMethod="GetBranchList"
                            WatermarkText="--Select--" />
                    </td>
                    <td width="15%">
                        <asp:Label runat="server" ToolTip="Asset Class" Text="Asset Class" ID="Label5" CssClass="styleDisplayLabel" />
                    </td>
                    <td width="25%" align="left">
                        <asp:DropDownList ID="ddlAssetClass" ToolTip="Asset Class" runat="server" MaxLength="0" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlAssetClass_SelectedIndexChanged" Width="175px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr width="100%">
                    <td width="15%">
                        <asp:Label runat="server" Text="Asset Make" ToolTip="Asset Make" ID="Label2" CssClass="styleDisplayLabel" />
                    </td>
                    <td width="25%" align="left">
                        <asp:DropDownList ID="ddlAssetMake" ToolTip="Asset Make" runat="server" MaxLength="0" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged" Width="175px">
                        </asp:DropDownList>
                    </td>
                    <td width="15%">
                        <asp:Label runat="server" Text="Asset Type" ToolTip="Asset Type" ID="Label3" CssClass="styleDisplayLabel" />
                    </td>
                    <td width="35%" align="left">
                        <asp:DropDownList ID="ddlAssetType" ToolTip="Asset Type" runat="server" MaxLength="0" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged" Width="175px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr width="100%">
                    <td width="15%">
                        <asp:Label runat="server" Text="Asset Model" ToolTip="Asset Model" ID="Label4" CssClass="styleDisplayLabel" />
                    </td>
                    <td width="25%" align="left">
                        <asp:DropDownList ID="ddlAssetModel" ToolTip="Asset Model" AutoPostBack="True" runat="server" MaxLength="0"
                            OnSelectedIndexChanged="ddlAssetModel_SelectedIndexChanged" Width="175px">
                        </asp:DropDownList>
                    </td>
                    <td width="15%">
                        &nbsp;
                    </td>
                    <td width="35%" align="left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:LinkButton CausesValidation="false" Visible="false" runat="server" ToolTip="Expand Grid to view better"
                            Text="<--->" ID="lnkGridSize"></asp:LinkButton>
                    </td>
                </tr>
                <tr width="100%">
                    <td colspan="4" width="100%">
                        <table width="100%">
                            <tr width="100%">
                                <td width="100%">
                                    <asp:Panel ID="pnlLoanToValue" Width="100%" Visible="false" GroupingText="Loan To Value Spooling"
                                        CssClass="stylePanel" runat="server">
                                        <div class="container" style="height: 235px; width: 100%; overflow-x: auto; overflow-y: auto;">
                                            <asp:GridView runat="server" ID="grvValueSpooling" ShowFooter="true" FooterStyle-HorizontalAlign="right"
                                                Width="98%" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader"
                                                RowStyle-HorizontalAlign="Left" OnRowDataBound="grvValueSpooling_RowDataBound"
                                                FooterStyle-Font-Bold="true">
                                                <Columns>
                                                    <%--Branch Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Location">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblBranchI" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>--%>
                                                            <asp:Label ID="lblBranchI" ToolTip="Location" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Customer Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Customer">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomerI" ToolTip="Customer" runat="server" Text='<%# Bind("Customer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Prime Account Number Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Prime Account Number">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <%--<HeaderTemplate>
                                                            <asp:Label ID="lblPrimeAccountNumber" runat="server" Text="Prime Account Number"></asp:Label>
                                                        </HeaderTemplate>--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrimeAccountNumberI" runat="server" ToolTip="Prime Account Number" Text='<%# Bind("PrimeAccountNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Sub Account Number Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Sub Account Number">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubAccountNumberI" runat="server" ToolTip="Sub Account Number" Text='<%# Bind("SubAccountNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Asset Description Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Asset Description">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetDescriptionI" ToolTip="Asset Description" runat="server" Text='<%# Bind("AssetDescription") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFooterAssetDescription" runat="server" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--Asset Value Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Asset Value">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetValueI" ToolTip="Asset Value" runat="server" Text='<%# Bind("AssetValue") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Orgin Date Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%" HeaderText="Origin Date">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrginDateI" runat="server" ToolTip="Origin Date" Text='<%# Bind("OriginDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Orgin Value Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Origin Value">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrginValueI" ToolTip="Origin Value" runat="server" Text='<%# Bind("OriginValue") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Principal Outstanding Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Principal Outstanding">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrincipalOutstandingI" ToolTip="Principal Outstanding" runat="server" Text='<%# Bind("PrincipalOutstanding") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFooterPrincipalOutstanding" runat="server" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--Interest Outstanding Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Interest Outstanding">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInterestOutstandingI" ToolTip="Interest Outstanding" runat="server" Text='<%# Bind("InterestOutstanding") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFooterInterestOutstanding" runat="server" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--Total Outstanding Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Total Outstanding">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalOutstandingI" ToolTip="Total Outstanding" runat="server" Text='<%# Bind("TotalOutstanding") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFooterTotalOutstandingF" runat="server" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--Market Value Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Market Value">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMarketValueI" ToolTip="Market Value" runat="server" Text='<%# Bind("MarketValue") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFooterMarketValue" runat="server" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--SLM Depreciation Value Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="SLM Depreciation Value">
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSLMDepreciationValueI" ToolTip="SLM Depreciation Value" runat="server" Text='<%# Bind("SLMDepreciationValue") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Account Closing Date Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Account Closing Date">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAccountClosingDateI" ToolTip="Account Closing Date" runat="server" Text='<%# Bind("AccountClosingDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--Risk Percentage Column--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Risk Percentage">
                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRiskPercentageI" ToolTip="Risk Percentage" runat="server" Text='<%# Bind("RiskPercentage") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr width="100%">
                                <td colspan="4" width="100%" align="center">
                                    <asp:Button Text="Go" ID="btnGo" ToolTip="Go" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                                        OnClick="btnGo_Click" />
                                    &nbsp;
                                    <asp:Button Text="Clear" ID="btnClear" ToolTip="Clear" OnClientClick="return fnConfirmClear();" runat="server"
                                        CauseValidation="False" CssClass="styleSubmitButton" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" width="100%">
                                    <asp:Button Text="Save" ToolTip="Save" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                        CausesValidation="true" runat="server" Enabled="false" CssClass="styleSubmitButton"
                                        OnClick="btnSave_Click" />
                                    &nbsp;
                                    <asp:Button Text="Excel" ToolTip="Excel" ID="btnExcel" CausesValidation="true" runat="server" CssClass="styleSubmitButton"
                                        OnClick="btnExcel_Click" Enabled="false" />
                                    &nbsp;
                                    <asp:Button Text="Flat File" ToolTip="Flat File" ID="btnFlatFile" CausesValidation="true" runat="server"
                                        CssClass="styleSubmitButton" OnClick="btnFlatFile_Click" Enabled="false" />
                                    &nbsp;
                                    <%-- <asp:Button Text="Screen View" ID="btnScreenView" CausesValidation="true" runat="server"
                                        CssClass="styleSubmitButton" OnClick="btnScreenView_Click" />--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnFlatFile" />
        </Triggers>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td colspan="4">
                <asp:CustomValidator ID="CVLoanTovalue" runat="server" Display="None" ValidationGroup="btnSave"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4">
                <asp:ValidationSummary ID="vsSpool" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ShowSummary="true" />
            </td>
        </tr>
    </table>
</asp:Content>
