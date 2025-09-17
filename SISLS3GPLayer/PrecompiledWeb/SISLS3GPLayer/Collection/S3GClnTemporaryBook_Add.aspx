﻿<%@ page title="" language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="Collection_S3GClnTemporaryBook_Add, App_Web_la20gqab" enableeventvalidation="false" enableviewstate="true" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="uc" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">

    function fnValidateEmpty() { }
    function fnValidateEmptyGotoPage() { }

</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="4">
                        <asp:Label runat="server" Text="Temporary Book Maintenance" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="18%">
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" Text="Line of Business" ID="lblLOB"
                            CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td width="18%">
                        <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                        </asp:DropDownList>
                        <%--<asp:RequiredFieldValidator ID="rfvLOB" runat="server" ErrorMessage="Select a Line of Business"
                            ValidationGroup="submit" Display="None" SetFocusOnError="True" InitialValue="0"
                            ControlToValidate="ddlLOB"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td width="20%" align="center">
                        <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleDisplayLabel"></asp:Label>
                        <span class="styleMandatory">*</span>
                    </td>
                    <td width="18%">
                       <uc:Suggest ID="ddlBranch" ToolTip="Location" runat="server" AutoPostBack="True" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                        ServiceMethod="GetBranchList" IsMandatory="true" ValidationGroup="submit" ErrorMessage="Select a Location"  WatermarkText="--Select--" />
                        <%--<asp:RequiredFieldValidator ID="rfvBranch" runat="server" ErrorMessage="Select a Location"
                            ValidationGroup="submit" Display="None" SetFocusOnError="True" InitialValue="0"
                            ControlToValidate="ddlBranch"></asp:RequiredFieldValidator>--%>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select a Branch"
                            ValidationGroup="Add" Display="None" SetFocusOnError="True" InitialValue="0"
                            ControlToValidate="ddlBranch"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr runat="server" id="pnlGrid">
                    <td colspan="4">
                        <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel" GroupingText="Temporary Book Details">
                            <asp:Panel ID="pnlInner" runat="server" CssClass="stylePanel" Visible="True" ScrollBars="Horizontal">
                                <asp:GridView ID="grvTemp" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader"
                                    OnRowDataBound="grvTemp_RowDataBound" ShowFooter="true" OnRowDeleting="grvTemp_RowDeleting"
                                    OnRowEditing="grvTemp_RowEditing" OnRowCancelingEdit="grvTemp_RowCancelingEdit"
                                    Width="100%" OnRowUpdating="grvTemp_RowUpdating">
                                    <%--< OnRowUpdating="grvTemp_RowUpdating"OnRowEditing="grvTemp_RowEditing" OnRowCancelingEdit="grvTemp_RowCancelingEdit"RowStyle HorizontalAlign="Center" />--%>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle CssClass="styleGridHeader" Width="10%" />
                                            <HeaderTemplate>
                                                <asp:Label ID="lblView" runat="server" Text="View"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                                    runat="server" ToolTip="View" OnClick="imgView_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Book No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookNo" ToolTip="Book No." runat="server" MaxLength="15" Style="min-width: 100px"
                                                    Text='<%# Bind("Temp_ReceiptBook_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtBookNo" runat="server" MaxLength="15" Style="text-align: right"
                                                    ToolTip="Book No." Width="110px"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender428" runat="server" Enabled="True"
                                                    FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" TargetControlID="txtBookNo"
                                                    ValidChars="/ - _ \ ( )">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvBookNo" runat="server" ControlToValidate="txtBookNo"
                                                    Display="None" ErrorMessage="Enter the Book No." ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtBookNo" runat="server" MaxLength="15" Style="text-align: right"
                                                    ToolTip="Book No." Width="110px" Text='<%# Bind("Temp_ReceiptBook_No") %>'></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender428" runat="server" Enabled="True"
                                                    FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" TargetControlID="txtBookNo"
                                                    ValidChars="/ - _ \ ( )">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvBookNo" runat="server" ControlToValidate="txtBookNo"
                                                    Display="None" ErrorMessage="Enter the Book No." ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start No." Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartNo" runat="server" MaxLength="8" Style="text-align: right"
                                                    Width="60px" Text='<%# Bind("Temp_Receipt_Start_No") %>' ToolTip="Start No."></asp:Label>
                                                <%--<cc1:FilteredTextBoxExtender ID="ftxtStartNo" runat="server" Enabled="True" FilterType="Numbers,Custom"
                                                TargetControlID="lblStartNo" ValidChars="">
                                            </cc1:FilteredTextBoxExtender>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtStartNo" runat="server" MaxLength="8" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                    Style="text-align: right" ToolTip="Start No." Width="60px"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender421" runat="server" Enabled="True"
                                                    FilterType="Numbers,Custom" TargetControlID="txtStartNo" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvStartNo" runat="server" ControlToValidate="txtStartNo"
                                                    Display="None" ErrorMessage="Enter the Start No." ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtStartNo" runat="server" MaxLength="8" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                    Style="text-align: right" ToolTip="Start No." Width="60px" Text='<%# Bind("Temp_Receipt_Start_No") %>'></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender421" runat="server" Enabled="True"
                                                    FilterType="Numbers,Custom" TargetControlID="txtStartNo" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvStartNo" runat="server" ControlToValidate="txtStartNo"
                                                    Display="None" ErrorMessage="Enter the Start No." ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End No." Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndNo" runat="server" MaxLength="8" Style="text-align: right" Width="60px"
                                                    Text='<%# Bind("Temp_Receipt_End_No") %>' ToolTip="End No."></asp:Label>
                                                <%--<cc1:FilteredTextBoxExtender ID="ftxtEndNo" runat="server" Enabled="True" FilterType="Numbers,Custom"
                                                TargetControlID="lblEndNo" ValidChars="">
                                            </cc1:FilteredTextBoxExtender>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtEndNo" runat="server" MaxLength="8" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                    Style="text-align: right" ToolTip="End No." Width="60px"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender425" runat="server" Enabled="True"
                                                    FilterType="Numbers,Custom" TargetControlID="txtEndNo" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvEndNo" runat="server" ControlToValidate="txtEndNo"
                                                    Display="None" ErrorMessage="Enter the End No." ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="cmpStartEnd" Display="None" Enabled="false" runat="server" ControlToValidate="txtEndNo" ErrorMessage="End No. Value should be greater than Start No. Value"
                                             ControlToCompare="txtStartNo" ValidationGroup="Add"></asp:CompareValidator>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEndNo" runat="server" MaxLength="8" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                    Style="text-align: right" ToolTip="End No." Width="60px" Text='<%# Bind("Temp_Receipt_End_No") %>'></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender425" runat="server" Enabled="True"
                                                    FilterType="Numbers,Custom" TargetControlID="txtEndNo" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvEndNo" runat="server" ControlToValidate="txtEndNo"
                                                    Display="None" ErrorMessage="Enter the End No." ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Used No." Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastNumber" runat="server" MaxLength="8" Style="text-align: right"
                                                    Width="60px" Text='<%# Bind("Temp_Receipt_Last_Used_No") %>' ToolTip="Last Used No."></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtLastNumber" ReadOnly="true" runat="server" MaxLength="8" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                    Style="text-align: right" ToolTip="Last Used No." Width="60px" Text="0"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender427" runat="server" Enabled="True"
                                                    FilterType="Numbers,Custom" TargetControlID="txtLastNumber" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvLastNumber" runat="server" ControlToValidate="txtLastNumber"
                                                    Display="None" ErrorMessage="Enter the Last Used No." ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date of Issue">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateofIssue" runat="server" MaxLength="8" Width="80px" Text='<%# Bind("Issued_Date") %>'
                                                    ToolTip="Last Number"></asp:Label>
                                                <%-- <asp:Image ID="imgDateofIssueEdit" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                ToolTip="Select Date of Issue" Visible="false" />
                                            <cc1:CalendarExtender ID="calDateofIssueEdit" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDateofIssueEdit"
                                                TargetControlID="lblDateofIssue">
                                            </cc1:CalendarExtender>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtDate" runat="server" MaxLength="8" ToolTip="Date" Width="80px"></asp:TextBox>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Date of Issue"
                                                    Visible="false" />
                                                <cc1:CalendarExtender ID="CEDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                    PopupButtonID="Image1" TargetControlID="txtDate">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvtxtDate" runat="server" ControlToValidate="txtDate"
                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="False"
                                                    ErrorMessage="Enter the Date of Issue"></asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDate" runat="server" MaxLength="8" ToolTip="Date" Width="80px"
                                                    Text='<%# Bind("Issued_Date") %>'></asp:TextBox>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Date of Issue"
                                                    Visible="false" />
                                                <cc1:CalendarExtender ID="CEDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                    OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image1"
                                                    TargetControlID="txtDate">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvtxtDate" runat="server" ControlToValidate="txtDate"
                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="False"
                                                    ErrorMessage="Enter the Date of Issue"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debt Collector Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblddlDebtcollectorId" Width="135px" runat="server" ToolTip="Debtcollector"
                                                    Text='<%# Bind("DebtCollector_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlDebtcollectorItem_SelectedIndexChanged">--%>
                                            <FooterTemplate>
                                                <asp:Label ID="lblFddlDebtcollectorId" runat="server" Text='<%# Bind("DebtCollector_ID") %>'></asp:Label>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblFddlDebtcollectorId" runat="server" Text='<%# Bind("DebtCollector_ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debt Collector Code - Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblddlDebtcollector" runat="server" ToolTip="Debtcollector"
                                                    Text='<%# Bind("DebtCollector_Code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlDebtcollectorItem_SelectedIndexChanged">--%>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlDebtcollector" runat="server" AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ddlDebtcollector_SelectedIndexChanged" ToolTip="Debtcollector">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblFddlDebtcollector" runat="server" Text='<%# Bind("DebtCollector_Code") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvIssuedTo" runat="server" ControlToValidate="ddlDebtcollector"
                                                    Display="None" ErrorMessage="Select a Debt Collector Code" InitialValue="0" ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="ddlDebtcollector"
                                                    Display="None" ErrorMessage="Select a Debtcollector code" ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlDebtcollector" runat="server" AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ddlDebtcollectorEdit_SelectedIndexChanged" ToolTip="Debtcollector">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblFddlDebtcollector" runat="server" Text='<%# Bind("DebtCollector_Code") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvIssuedTo" runat="server" ControlToValidate="ddlDebtcollector"
                                                    Display="None" ErrorMessage="Select a Debtcollector Code" InitialValue="0" ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="ddlDebtcollector"
                                                    Display="None" ErrorMessage="Select a Debtcollector code" ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debt Collector Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDCName" ContentEditable="False" runat="server" ToolTip="Debt Collector Name"
                                                    Width="120px" Text='<%# Bind("DebtCollector_Name") %>'></asp:Label>
                                                <%--<asp:DropDownList ID="lblDCName" runat="server">
                                            </asp:DropDownList>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtDCName" runat="server" ToolTip="Debt Collector Name" ContentEditable="False"
                                                    Width="120px"></asp:TextBox>
                                                <%-- <asp:DropDownList ID="ddlDCName" runat="server" ToolTip="DCName">
                                            </asp:DropDownList>--%>
                                                <asp:Label ID="lblFDCName" runat="server" Text='<%# Bind("DebtCollector_Name") %>'
                                                    Visible="false"></asp:Label>
                                                <%--<asp:RequiredFieldValidator ID="rfvDCName" runat="server" ControlToValidate="txtdebtcollectorname"
                                                Display="None" ErrorMessage="Select a " InitialValue="0" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="ddlDCName"
                                                Display="None" ErrorMessage="Select a DCName&amp;Code" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>--%>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDCName" runat="server" ToolTip="Debt Collector Name" ContentEditable="False"
                                                    Width="120px" Text='<%# Bind("DebtCollector_Name") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" TextMode="MultiLine" Style="padding-left: 5px"
                                                    Text='<%# Bind("Remarks") %>' ToolTip="Remarks" Width="120px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" onkeyup="maxlengthfortxt(200)" TextMode="MultiLine"
                                                    ToolTip="Remarks" Width="120px"></asp:TextBox>
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" onkeyup="maxlengthfortxt(200)" TextMode="MultiLine"
                                                    ToolTip="Remarks" Width="120px" Text='<%# Bind("Remarks") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By") %>'></asp:Label>
                                                <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID") %>'></asp:Label>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                    ToolTip="Edit">
                                                </asp:LinkButton>&nbsp;|
                                                <asp:LinkButton ID="btnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                    OnClientClick="return confirm('Are you sure want to delete?');" Text="Remove" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="btnAdd" runat="server" CausesValidation="true" CommandName="AddNew"
                                                    CssClass="styleGridShortButton" OnClick="btnAdd_Click" Text="Add" ValidationGroup="Add" />
                                            </FooterTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                    ValidationGroup="Add" ToolTip="Update"></asp:LinkButton>&nbsp;|
                                                <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                    CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                            </EditItemTemplate>
                                            <HeaderStyle Width="10%" />
                                            <FooterStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View" Visible="false">
                                            <ItemTemplate>
                                                <asp:Button ID="btnView" runat="server" CausesValidation="true" CssClass="styleGridShortButton"
                                                    Text="Details" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Book Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookID" runat="server" Text='<%# Bind("Temp_Receipt_Book_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr runat="server" id="Tr1">
                    <td colspan="4" width="100%" align="center">
                        <asp:Panel ID="pnlLeaf" runat="server" CssClass="stylePanel" GroupingText="Leaf Details"
                            Width="99%">
                            <table cellpadding="5">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Book No" Visible="false" ID="lblBookNo1" CssClass="styleDisplayLabel"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtBookNo" runat="server" ReadOnly="true" Visible="false" MaxLength="15"
                                            Width="80px" ToolTip="Book No"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grvLeaf" runat="server" AllowPaging="false" HeaderStyle-CssClass="styleGridHeader"
                                            AutoGenerateColumns="false" Width="100%" OnRowDataBound="grvLeaf_RowDataBound">
                                            <RowStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Leaf Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLeafID" runat="server" Text='<%# Bind("Temp_ReceiptBook_Details_Id") %>'
                                                            ToolTip="Leaf Number" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblLeafNumber" runat="server" Text='<%# Bind("Temp_ReceiptBook_Page_No") %>'
                                                            ToolTip="Leaf Number"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Receipt Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceiptNo" runat="server" Text='<%# Bind("Temp_Receipt_No") %>'
                                                            ToolTip="Receipt Number"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' ToolTip="Status"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cancelled Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCancelDate" runat="server" Text='<%# Bind("Cancel_Date") %>' ToolTip="Canceled Date"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    Unused
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td>
                                                                    <asp:CheckBox ID="chkAllUnused" runat="server" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td>
                                                                    <asp:CheckBox ID="chkUnused" AutoPostBack="true" runat="server" OnCheckedChanged="Unused_OnCheckedChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    Cancel
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td>
                                                                    <asp:CheckBox ID="chkAllCancel" runat="server" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td>
                                                                    <asp:CheckBox ID="chkCancel" runat="server" AutoPostBack="true" OnCheckedChanged="Cancel_OnCheckedChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cancelled Reason" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReason" runat="server" Text='<%# Bind("Reason") %>' Visible="false"></asp:Label>
                                                        <asp:DropDownList ID="ddlReason" AutoPostBack="true" OnSelectedIndexChanged="ddlReason_OnSelectedIndexChanged"
                                                            runat="server" ToolTip="Cancelled Reason">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea" style="padding-left: 4px">
                    <td colspan="4" align="center">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            ValidationGroup="submit" OnClientClick="return fnCheckPageValidators('submit');"
                            OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClick="btnClear_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td colspan="4">
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <input type="hidden" id="hdnCompanyCode" runat="server" />
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Correct the following validation(s):"
                            ValidationGroup="submit" Height="250px" CssClass="styleSummaryStyle" Width="500px"
                            ShowMessageBox="false" ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                            Height="250px" CssClass="styleSummaryStyle" Width="500px" ShowMessageBox="false"
                            ValidationGroup="Add" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="cvTempBook" runat="server" CssClass="styleMandatoryLabel"
                            ValidationGroup="submit" Enabled="true" Width="98%" Display="None" />
                        <span id="Span1" runat="server" style="font-size: medium"></span>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <%-- <Triggers>
            <asp:PostBackTrigger ControlID="ucCustomPaging" />           
        </Triggers>--%>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
    
    function fnGridToggleReason(chkbox,ddl,lblDt)
    {
      var checkbox = document.getElementById(chkbox);
      var dropdown = document.getElementById(ddl);
      var lbldate = document.getElementById(lblDt);
      
      if(!checkbox.checked)
      {
         //dropdown.style.visibility = false;
        dropdown.style.display = 'none';
        lbldate.style.display = 'none';
      }
      else
      {
        //dropdown.style.visibility = true;
       dropdown.style.display = 'block';
       lbldate.style.display = 'block';
      }
    }
   
      function showMenu(show) 
       {
            if (show == 'T') {
                
                //Added by Kali K to solve error ( when menu is show scroll should appear in grid )
                //This is used to show scroll bar when div is used in GridView
                if(document.getElementById('divGrid1')!=null)
                    {
                        document.getElementById('divGrid1').style.width="800px";
                        document.getElementById('divGrid1').style.overflow="scroll";
                    }
                
                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                (document.getElementById('ctl00_ContentPlaceHolder1_Panel1')).style.width = screen.width - 270;
            }
            if (show == 'F') {
                
                //Added by Kali K to solve error ( when menu is hide scroll for is not hiding from grid view )
                //This is used to hide scroll bar when div is used in GridView
                if(document.getElementById('divGrid1')!=null)
                    {
                        document.getElementById('divGrid1').style.width="960px";
                        document.getElementById('divGrid1').style.overflow="auto";
                    }
                    
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';

                //document.getElementById('divMenu').setAttribute('width', 0);
                
                (document.getElementById('ctl00_ContentPlaceHolder1_Panel1')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }

      function Resize()
        {
         if(document.getElementById('divMenu').style.display=='none')
          {
           (document.getElementById('ctl00_ContentPlaceHolder1_Panel1')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
          }
         else
         {
           (document.getElementById('ctl00_ContentPlaceHolder1_Panel1')).style.width = screen.width - 270;
         }  
        }
   
    </script>

</asp:Content>
