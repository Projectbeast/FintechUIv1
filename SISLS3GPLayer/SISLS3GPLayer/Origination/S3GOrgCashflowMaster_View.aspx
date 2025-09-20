<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GOrgCashflowMaster_View.aspx.cs" Inherits="Origination_S3GOrgCashflowMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row m-0">
                <div class="col">
                    <h6 class="title_name">
                        <asp:Label runat="server" Text="Cash Flow Master" ID="lblHeading" class="styleInfoLabel">
                        </asp:Label>
                    </h6>
                </div>
                <div class="col mr-3">
                    <div class="float-right">
                        <button class="btn btn-outline-success mr-2 btn-create" id="btnCreate" onserverclick="btnCreate_Click" runat="server" title="Create,Alt+C"
                            type="button" accesskey="C">
                            <i class="fa fa-plus" aria-hidden="true"></i>&emsp;<u>C</u>reate
                        </button>
                    </div>
                </div>
            </div>
            <div class="scrollable-content ">
                <div class="section-box py-4 mx-2">
                    <div class="row mx-3 section-box m-0 p-0">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird">
                            <asp:GridView ID="grvPaging" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                AutoGenerateColumns="False" RowStyle-HorizontalAlign="Left" DataKeyNames="CashFlow_ID"
                                OnRowDataBound="grvPaging_RowDataBound" Width="100%" OnRowCommand="grvPaging_RowCommand" CssClass="gird_details">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader" Width="25px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("CashFlow_ID") %>' CommandName="Query" runat="server">
                                         <i class="fa fa-search" ></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader" Width="25px" />
                                    <ItemStyle HorizontalAlign="Center" />

                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                            CommandArgument='<%# Bind("CashFlow_ID") %>'
                                            CommandName="Modify" runat="server">
                                    <i class="fa fa-pencil-square-o" ></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LOB" SortExpression="LOB" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblALOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="border: none;">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <asp:LinkButton ID="lnkbtnSort1" runat="server"
                                                            OnClick="FunProSortingColumn" Style="text-decoration: none;" ToolTip="Sort By LOB" Text="Line of Business" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort1" CssClass="styleImageSortingAsc" OnClientClick="return false;"
                                                            runat="server" ImageAlign="Middle" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" MaxLength="45" runat="server"
                                                                class="form-control form-control-sm mt-1" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <span class="highlight"></span>
                                                            <span class="bar"></span>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <ItemStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flow Type" SortExpression="Flow_Type" HeaderStyle-CssClass="styleGridHeader"
                                    ControlStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlowType" runat="server" Text='<%# Bind("Flow_Type") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="border: none;">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <asp:LinkButton ID="lnkbtnSort2" Style="text-decoration: none;" runat="server"
                                                            OnClick="FunProSortingColumn" ToolTip="Sort By Flow Type" Text="Flow Type" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort2" CssClass="styleImageSortingAsc" runat="server"
                                                            ImageAlign="Middle" OnClientClick="return false;" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" MaxLength="46" runat="server"
                                                                Width="80px" class="form-control form-control-sm mt-1" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <span class="highlight"></span>
                                                            <span class="bar"></span>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CFSl_No" HeaderText="CF.Sl.No." HeaderStyle-CssClass="styleGridHeader"></asp:BoundField>
                                <%-- <asp:BoundField DataField="CashFlowFlag_Desc" HeaderText="Cash Flow Flag" HeaderStyle-CssClass="styleGridHeader">
                                </asp:BoundField>--%>
                                <asp:TemplateField HeaderText="Cash Flow Flag" SortExpression="Cash_Flow_Flag" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCashFlowFlag" runat="server" Text='<%# Bind("CashFlowFlag_Desc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="border: none;">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:LinkButton ID="lnkbtnSort3" Style="text-decoration: none;" runat="server"
                                                                OnClick="FunProSortingColumn" ToolTip="Sort By Cash Flow Flag" Text="Cash Flow Flag" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                            <asp:Label ID="imgbtnSort3" CssClass="styleImageSortingAsc" runat="server"
                                                                ImageAlign="Middle" OnClientClick="return false;" />
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" MaxLength="45" runat="server"
                                                                Width="150px" class="form-control form-control-sm mt-1" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch3"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <span class="highlight"></span>
                                                            <span class="bar"></span>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="CashFlow_Description" HeaderText="Cash Flow Desc." HeaderStyle-CssClass="styleGridHeader">
                                </asp:BoundField>--%>
                                <asp:TemplateField HeaderText="Cash Flow Desc." SortExpression="Cash Flow Desc." HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCashFlow_Description" runat="server" Text='<%# Bind("CashFlow_Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="border: none;">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:LinkButton ID="lnkbtnSort4" Style="text-decoration: none;" runat="server"
                                                                OnClick="FunProSortingColumn" ToolTip="Sort By Cash Flow Desc." Text="Cash Flow Desc." CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                            <asp:Label ID="imgbtnSort4" CssClass="styleImageSortingAsc" runat="server"
                                                                ImageAlign="Middle" OnClientClick="return false;" />
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="45" runat="server"
                                                                Width="150px" class="form-control form-control-sm mt-1" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtHeaderSearch4"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <span class="highlight"></span>
                                                            <span class="bar"></span>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Program" SortExpression="Program" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProgram" runat="server" Text='<%# Bind("Program_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="border: none;">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:LinkButton ID="lnkbtnSort5" Style="text-decoration: none;" runat="server"
                                                                OnClick="FunProSortingColumn" ToolTip="Sort By Program" Text="Program" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                            <asp:Label ID="imgbtnSort5" CssClass="styleImageSortingAsc" runat="server"
                                                                ImageAlign="Middle" OnClientClick="return false;" />
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" MaxLength="45" runat="server"
                                                                Width="150px" class="form-control form-control-sm mt-1" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtHeaderSearch5"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <span class="highlight"></span>
                                                            <span class="bar"></span>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="Program_Name" HeaderText="Program" HeaderStyle-CssClass="styleGridHeader">
                                </asp:BoundField>--%>
                                <%--<asp:BoundField DataField="DGL_Account_Code" HeaderText="Debit GL CODE" HeaderStyle-CssClass="styleGridHeader"></asp:BoundField>
                                    <asp:BoundField DataField="DSL_Account_Code" HeaderText="Debit SL CODE" HeaderStyle-CssClass="styleGridHeader"></asp:BoundField>
                                    <asp:BoundField DataField="Debit_Type_ID" HeaderText="Debit Type" HeaderStyle-CssClass="styleGridHeader"></asp:BoundField>
                                    <asp:BoundField DataField="CGL_Account_Code" HeaderText="Credit GL CODE" HeaderStyle-CssClass="styleGridHeader"></asp:BoundField>
                                    <asp:BoundField DataField="CSL_Account_Code" HeaderText="Credit SL CODE" HeaderStyle-CssClass="styleGridHeader"></asp:BoundField>
                                    <asp:BoundField DataField="Credit_Type_ID" HeaderText="Credit Type" HeaderStyle-CssClass="styleGridHeader"></asp:BoundField>
                                --%><asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CbxActive" Enabled="false" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem, "Is_Active").ToString().ToUpper() == "TRUE"%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%#Eval("Created_By")%>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%#Eval("User_Level_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="row m-0 mx-4">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </div>
                </div>
                <div class="row" style="margin-top: 10px; align-content: center">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
                    </div>
                </div>
                <div class="row">
                    <div class="fixed_btn">
                        <button class="btn btn-success" id="btnBranchShowAll" onserverclick="btnBranchShowAll_Click" runat="server" title="Show All,Alt+H"
                            type="button" accesskey="H">
                            <i class="fa fa-list" aria-hidden="true"></i>&emsp;S<u>h</u>ow All
                        </button>
                    </div>
                </div>
                
            </div>
        </div>
            <input type="hidden" id="hdnSortDirection" runat="server" />
            <input type="hidden" id="hdnSortExpression" runat="server" />
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
