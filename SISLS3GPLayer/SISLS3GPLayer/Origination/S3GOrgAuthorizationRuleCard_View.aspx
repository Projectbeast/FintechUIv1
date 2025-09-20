<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GOrgAuthorizationRuleCard_View.aspx.cs"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="Origination_S3GOrgAuthorizationRuleCard_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row m-0">
                <div class="col">
                    <h6 class="title_name">
                        <asp:Label runat="server" ID="lblHeading" class="styleInfoLabel">
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
                        <asp:GridView ID="grvPaging" runat="server" OnRowDataBound="grvPaging_RowDataBound"
                            OnRowCommand="grvPaging_RowCommand" AutoGenerateColumns="false" CssClass="gird_details">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader" Width="25px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Authorization_Rule_Card_ID") %>' CommandName="Query" runat="server">
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
                                            CommandArgument='<%# Bind("Authorization_Rule_Card_ID") %>' CommandName="Modify"
                                            runat="server">
                                    <i class="fa fa-pencil-square-o" ></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="LOB">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSort1" runat="server"
                                                            OnClick="FunProSortingColumn" ToolTip="Sort By Line of Business" Text="Line of Business" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort1" CssClass="styleImageSortingAsc" OnClientClick="return false;"
                                                            runat="server" ImageAlign="Middle" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" MaxLength="45" runat="server"
                                                                class="form-control form-control-sm mt-1" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBName" runat="server" Text='<%# Bind("LOB_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <table>
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSort2" runat="server"
                                                            OnClick="FunProSortingColumn" ToolTip="Sort By Scheme"
                                                            Text="Scheme" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort2" CssClass="styleImageSortingAsc" runat="server"
                                                            ImageAlign="Middle" OnClientClick="return false;" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2"
                                                                MaxLength="46" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"
                                                                class="form-control form-control-sm mt-1"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Product_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <table>
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSort3" runat="server"
                                                            OnClick="FunProSortingColumn" ToolTip="Sort By Constitution"
                                                            Text="Constitution" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort3" CssClass="styleImageSortingAsc" runat="server"
                                                            ImageAlign="Middle" OnClientClick="return false;" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3"
                                                                MaxLength="45" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"
                                                                class="form-control form-control-sm mt-1"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblconstitution" runat="server" Text='<%# Bind("Constitution_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort4" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Transaction Type" Text="Transaction Type"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort4" CssClass="styleImageSortingAsc" runat="server"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="40" runat="server" Width="120px"
                                                        AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <table>
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSort4" runat="server"
                                                            OnClick="FunProSortingColumn" ToolTip="Sort By Program" Text="Program" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:Label ID="imgbtnSort4" CssClass="styleImageSortingAsc" runat="server"
                                                            ImageAlign="Middle" OnClientClick="return false;" />
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="40" runat="server"
                                                                AutoPostBack="true" OnTextChanged="FunProHeaderSearch" class="form-control form-control-sm mt-1"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch4" runat="server" TargetControlID="txtHeaderSearch4"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblprogram" runat="server" Text='<%# Bind("Program_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                        <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
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
                    <button class="btn btn-success" id="btnShowAll" onserverclick="btnShowAll_Click" runat="server" title="Show All,Alt+H"
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
