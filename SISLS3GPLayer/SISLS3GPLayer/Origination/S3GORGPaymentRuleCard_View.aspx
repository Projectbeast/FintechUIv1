<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GORGPaymentRuleCard_View.aspx.cs" Inherits="S3GORGPaymentRuleCard_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <div class="row m-0">
                <div class="col">
                    <h6 class="title_name">
                        <asp:Label runat="server" ID="lblHeading" ToolTip="Organization Master" class="styleInfoLabel">
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
                        <asp:GridView ID="grvPaging" runat="server" Width="100%" AutoGenerateColumns="false"
                            OnRowCommand="grvPaging_RowCommand" OnRowDataBound="grvPaging_RowDataBound" CssClass="gird_details">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader" Width="25px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Payment_RuleCard_ID") %>' CommandName="Query" runat="server">
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
                                            CommandArgument='<%# Bind("Payment_RuleCard_ID") %>' CommandName="Modify" runat="server">
                                        <i class="fa fa-pencil-square-o" ></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LOB" SortExpression="LOB">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSort1" runat="server"
                                                            OnClick="FunProSortingColumn" ToolTip="Sort By Line of Business" Text="Line of Business" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort1" runat="server" CssClass="styleImageSortingAsc"
                                                            OnClientClick="return false;" ImageAlign="Middle" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                                class="form-control form-control-sm mt-1" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
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
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBCode" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="AccountType">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSort2" runat="server"
                                                            OnClick="FunProSortingColumn" ToolTip="Sort By AccountType" Text="Account Type" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort2" runat="server" CssClass="styleImageSortingAsc"
                                                            OnClientClick="return false;" ImageAlign="Middle" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                                class="form-control form-control-sm mt-1" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" " Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <span class="highlight"></span>
                                                            <span class="bar"></span>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBName" runat="server" Text='<%# Bind("AccountType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Payment_Rule_Number">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSort3" runat="server"
                                                            OnClick="FunProSortingColumn" ToolTip="Sort By Payment Rule Number" Text="Payment Rule Number" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort3" runat="server" CssClass="styleImageSortingAsc"
                                                            OnClientClick="return false;" ImageAlign="Middle" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                                class="form-control form-control-sm mt-1" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="-/"
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
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentRuleNumber" runat="server" Text='<%# Bind("Payment_Rule_Number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" Checked='<%# FunPriCheckBool(Eval("Is_Active").ToString()) %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
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
                            <button class="btn btn-success" id="btnShowAll" onserverclick="btnShowAll_Click" runat="server" title="Show All,Alt+H"
                                type="button" accesskey="H">
                                <i class="fa fa-list" aria-hidden="true"></i>&emsp;S<u>h</u>ow All
                            </button>
                        </div>
                    </div>
                </div>
            </div>
                <%-- <div class="row" style="float: right; margin-top: 5px;">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <i class="fa fa-floppy-o btn_i" aria-hidden="true"></i>
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CssClass="save_btn fa fa-floppy-o" AccessKey="C" ToolTip="Create,Alt+C"
                            CausesValidation="false" Text="Create" runat="server"></asp:Button>
                        <i class="fa fa-floppy-o btn_i" aria-hidden="true"></i>
                        <asp:Button ID="btnShowAll" OnClick="btnShowAll_Click" runat="server" Text="Show All" AccessKey="H" ToolTip="Show All,Alt+H"
                            CssClass="save_btn fa fa-floppy-o" />
                    </div>
                </div>--%>
                <input type="hidden" id="hdnSortDirection" runat="server" />
                <input type="hidden" id="hdnSortExpression" runat="server" />
                <input type="hidden" id="hdnSearch" runat="server" />
                <input type="hidden" id="hdnOrderBy" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
