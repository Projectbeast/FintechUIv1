﻿<%@ page title="" language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="UnderWriting_S3GUWTransLander, App_Web_1grzfuxm" %>

<%--Ajax Control Toolkit--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Grid User Control--%>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%--Content--%>
<%@ Register TagPrefix="uc" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function Branch_ItemSelected(sender, e) {
            var hdnBranchID = $get('<%= hdnBranchID.ClientID %>');
            hdnBranchID.value = e.get_value();
        }
        function Branch_ItemPopulated(sender, e) {
            var hdnBranchID = $get('<%= hdnBranchID.ClientID %>');
        hdnBranchID.value = '';
    }
    function fnValidateEmpty() { }
    function fnValidateEmptyGotoPage() { }
    </script>
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td colspan="4" class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" EnableViewState="false" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                            <%--Row -1 with 1 columns--%>
                            <tr width="100%">
                                <%--Spacer--%>
                                <td width="100%" colspan="4" align="center">&nbsp;
                                </td>
                            </tr>
                            <%--Row 1 with 4 columns--%>
                            <tr width="100%">
                                <%--Line of Business--%>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOBSearch" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <cc1:ComboBox ID="ComboBoxLOBSearch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                        runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                        MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxLOBSearch_SelectedIndexChanged">
                                    </cc1:ComboBox>
                                    <asp:RequiredFieldValidator ID="RFVComboLOB" ValidationGroup="RFVDTransLander" InitialValue="-- Select --"
                                        CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxLOBSearch"
                                        SetFocusOnError="True" Display="None" ErrorMessage="Select the Line of Business"></asp:RequiredFieldValidator>
                                </td>
                                <%--Start Date--%>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Start Date" ID="lblStartDateSearch" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <input id="hidDate" type="hidden" runat="server" />
                                    <asp:TextBox ID="txtStartDateSearch" runat="server" Width="150px" AutoPostBack="True"
                                        OnTextChanged="txtStartDateSearch_TextChanged"></asp:TextBox>
                                    <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <cc1:CalendarExtender ID="CalendarExtenderStartDateSearch" runat="server" Enabled="True"
                                        PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RFVStartDate" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="txtStartDateSearch" SetFocusOnError="True"
                                        ErrorMessage="Enter a Start Date" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--Row 2 with 4 columns--%>
                            <tr width="100%">
                                <%--Branch--%>
                                <td class="styleFieldLabel">
                                    <asp:Label Text="Branch" runat="server" ID="lblBranchSearch" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <%-- <cc1:ComboBox ID="ComboBoxBranchSearch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                        runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                        MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxBranchSearch_SelectedIndexChanged">
                                    </cc1:ComboBox>
                                    <asp:RequiredFieldValidator ID="RFVComboBranch" ValidationGroup="RFVDTransLander"
                                        InitialValue="-- Select --" CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxBranchSearch"
                                        SetFocusOnError="True" Display="None" ErrorMessage="Select the Location"></asp:RequiredFieldValidator>--%>
                                    <asp:TextBox ID="txtBranchSearch" runat="server" MaxLength="50" OnTextChanged="txtBranchSearch_OnTextChanged"
                                        AutoPostBack="true" Width="182px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="autoBranchSearch" MinimumPrefixLength="3" OnClientPopulated="Branch_ItemPopulated"
                                        OnClientItemSelected="Branch_ItemSelected" runat="server" TargetControlID="txtBranchSearch"
                                        ServiceMethod="GetBranchList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                        CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                        CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                        ShowOnlyCurrentWordInCompletionListItem="true">
                                    </cc1:AutoCompleteExtender>
                                    <cc1:TextBoxWatermarkExtender ID="txtBranchSearchExtender" runat="server" TargetControlID="txtBranchSearch"
                                        WatermarkText="--Select--">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:HiddenField ID="hdnBranchID" runat="server" />
                                    <asp:RequiredFieldValidator ID="RFVComboBranch" ValidationGroup="RFVDTransLander"
                                        InitialValue="" CssClass="styleMandatoryLabel" runat="server" ControlToValidate="txtBranchSearch"
                                        SetFocusOnError="True" Display="None" ErrorMessage="Select the Branch"></asp:RequiredFieldValidator>
                                </td>
                                <%--End Date--%>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" ID="lblEndDateSearch" Text="End Date" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtEndDateSearch" runat="server" Width="150px" AutoPostBack="True"
                                        OnTextChanged="txtEndDateSearch_TextChanged"></asp:TextBox>
                                    <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <cc1:CalendarExtender ID="CalendarExtenderEndDateSearch" runat="server" Enabled="True"
                                        PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RFVEndDate" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="txtEndDateSearch" SetFocusOnError="True" ErrorMessage="Enter a End Date"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--Row 3 with 4 columns--%>
                            <tr width="100%">
                                <%--Multiple Document Number --%>
                                <td class="styleFieldLabel">
                                    <asp:Label Visible="false" Text="Transaction" runat="server" ID="lblMultipleDNC"
                                        CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList Visible="false" AutoPostBack="true" ID="ddlMultipleDNC" runat="server"
                                        Width="185px" OnSelectedIndexChanged="ddlMultipleDNC_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label Visible="false" Text="Select Status" runat="server" ID="lblDNCOption"
                                        CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList Visible="false" ID="ddlDNCOption" runat="server" Width="175px"
                                        OnSelectedIndexChanged="ddlDNCOption_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <%--Row 3 with 4 columns--%>
                            <tr width="100%">
                                <%--Document Number--%>
                                <td class="styleFieldLabel">
                                    <asp:Label Text="" runat="server" ID="lblProgramIDSearch" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <%-- <cc1:ComboBox ID="cmbDocumentNumberSearch" runat="server" AutoCompleteMode="SuggestAppend"
                                        onkeyup="maxlengthfortxt(113)" DropDownStyle="DropDownList" MaxLength="0" CssClass="WindowsStyle"
                                        AutoPostBack="True" OnSelectedIndexChanged="cmbDocumentNumberSearch_SelectedIndexChanged">
                                    </cc1:ComboBox>--%>
                                    <uc:Suggest ID="ddlDocumentNumb" Width="182px" runat="server" AutoPostBack="false"
                                        ServiceMethod="GetDocumentNumberList" />
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label Text="" runat="server" ID="lblStatusSearch" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <%-- <cc1:ComboBox ID="cmbDocumentNumberSearch" runat="server" AutoCompleteMode="SuggestAppend"
                                        onkeyup="maxlengthfortxt(113)" DropDownStyle="DropDownList" MaxLength="0" CssClass="WindowsStyle"
                                        AutoPostBack="True" OnSelectedIndexChanged="cmbDocumentNumberSearch_SelectedIndexChanged">
                                    </cc1:ComboBox>--%>
                                    <uc:Suggest ID="ddlStatus" Width="182px" runat="server" Visible="false" AutoPostBack="false"
                                        ServiceMethod="GetSearchOptionList" />
                                </td>
                            </tr>
                            <%--Row 4 with 1 columns--%>
                            <tr width="100%">
                                <%--Spacer--%>
                                <td width="100%" colspan="4" align="center">&nbsp;
                                </td>
                            </tr>
                            <%--Row 5 with 1 columns--%>
                            <tr width="100%">
                                <%--Search Records--%>
                                <td width="100%" colspan="4" align="center">
                                    <asp:Button Text="Search" ValidationGroup="RFVDTransLander" ID="btnSearch" runat="server"
                                        CssClass="styleSubmitButton" UseSubmitBehavior="true" OnClick="btnSearch_Click" />
                                    <asp:Button Text="Create" ID="btnCreate" runat="server" CssClass="styleSubmitButton"
                                        UseSubmitBehavior="true" OnClick="btnCreate_Click" />
                                    <asp:Button Text="Clear" ID="btnClear" runat="server" CssClass="styleSubmitButton"
                                        UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                            <%--Row 6 with 1 columns--%>
                            <tr width="100%">
                                <%--Spacer--%>
                                <td width="100%" colspan="4" align="center">&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--Row 7 with 1 columns--%>
                <tr width="100%">
                    <%--Grid--%>
                    <td width="100%" colspan="4" align="center">
                        <asp:GridView runat="server" ID="grvTransLander" Width="100%" AutoGenerateColumns="true"
                            OnRowCommand="grvTransLander_RowCommand" HeaderStyle-CssClass="styleGridHeader"
                            RowStyle-HorizontalAlign="Left" OnRowDataBound="grvTransLander_RowDataBound">
                            <Columns>
                                <%--Query Column--%>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--Edit Column--%>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--Created by - User ID Column--%>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By") %>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <%--Row 8 with 1 columns--%>
                <tr width="100%">
                    <%--Grid--%>
                    <td width="100%" colspan="4" align="center">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <%--Row 9 with 1 columns--%>
                <tr width="100%">
                    <%--Spacer--%>
                    <td width="100%" colspan="4" align="center">&nbsp;
                    </td>
                </tr>
                <%--Row 10 with 1 columns--%>
                <%-- <tr width="100%">--%>
                <%--Search Records--%>
                <%-- <td width="100%" colspan="4" align="center">
                        <asp:Button Text="Show All" ID="btnShowAll" runat="server" CssClass="styleSubmitButton"
                            UseSubmitBehavior="true" OnClick="btnShowAll_Click" />
                    </td>
                </tr>--%>
                <%--Row 11 with 1 columns--%>
                <tr width="100%">
                    <%--Error Message--%>
                    <td width="100%" colspan="4" align="center">
                        <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
                    </td>
                </tr>
                <%--Row 12 with 1 columns--%>
                <tr>
                    <td colspan="4">
                        <%--Hidden fields for grid usage--%>
                        <input type="hidden" id="hdnSortDirection" runat="server" />
                        <input type="hidden" id="hdnSortExpression" runat="server" />
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:ValidationSummary ValidationGroup="RFVDTransLander" ID="vsTransLander" runat="server"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

