<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3GTemplate_View.aspx.cs" Inherits="System_Admin_S3GTemplate_View" ValidateRequest="false" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="row m-0">
                <div class="col">
                    <h6 class="title_name">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                        </asp:Label>
                    </h6>
                </div>
                <div class="col">
                    <button class="btn btn-outline-success float-right mr-4 btn-create" id="btnCreate" title="Create[Alt+C]" causesvalidation="false" onserverclick="btnCreate_Click" runat="server"
                        type="button" accesskey="C">
                        <i class="fa fa-plus"></i>&emsp;<u>C</u>reate
                    </button>

                </div>
            </div>

            <div class="scrollable-content ">
                <div class="section-box py-4 mx-2">

                    <div class="row mx-3">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird p-0">
                            <asp:GridView ID="grvTemplatePaging" runat="server" AutoGenerateColumns="False" Width="100%"
                                OnRowCommand="grvTemplatePaging_RowCommand" OnRowDataBound="grvTemplatePaging_RowDataBound"
                                RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader" class="gird_details">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                                CommandArgument='<%# Bind("ID") %>' CommandName="Query" runat="server">
                                    <i class="fa fa-search" ></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                                CommandArgument='<%# Bind("ID") %>' CommandName="Modify" runat="server">
                                         <i class="fa fa-pencil-square-o" ></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="LOB">
                                        <HeaderTemplate>
                                            <table>
                                                <thead>
                                                    <tr align="center">
                                                        <th style="padding: 0px !important; background: none !important;">
                                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Line of Business" ToolTip="Sort By Line of Business"
                                                                OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                            <asp:Label ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr align="left">
                                                        <th style="padding: 0px !important; background: none !important;">
                                                            <div class="md-input" style="margin: 0px;">
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" class=" form-control form-control-sm mt-1" onpaste="return false;"
                                                                    runat="server" MaxLength="40" AutoPostBack="true"
                                                                    OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="filtertxtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                    FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters" FilterMode="ValidChars" ValidChars="@.- " Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLineofBusiness" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <table>
                                                <thead>
                                                    <tr align="center">
                                                        <th style="padding: 0px !important; background: none !important;">
                                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Branch" ToolTip="Sort By Branch"
                                                                OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                            <asp:Label ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr align="left">
                                                        <th style="padding: 0px !important; background: none !important;">
                                                            <div class="md-input" style="margin: 0px;">
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" class="form-control form-control-sm mt-1" onpaste="return false;"
                                                                    MaxLength="40" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="filtertxtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                    FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters" FilterMode="ValidChars" ValidChars="@.- " Enabled="True" />
                                                            </div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Template Ref No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTemplateRefNo" runat="server" Text='<%# Bind("TemplateRefNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <table>
                                                <thead>
                                                    <tr align="center">
                                                        <th style="padding: 0px !important; background: none !important;">
                                                            <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Template Ref No" ToolTip="Sort By Template Ref No"
                                                                OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                            <asp:Label ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr align="left">
                                                        <th style="padding: 0px !important; background: none !important;">
                                                            <div class="md-input" style="margin: 0px;">
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" class=" form-control form-control-sm mt-1" onpaste="return false;"
                                                                    runat="server" MaxLength="40" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="filtertxtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3"
                                                                    FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters" FilterMode="ValidChars" ValidChars="@.-/ " Enabled="True" />
                                                            </div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Template Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTemplateType" runat="server" Text='<%# Bind("TemplateType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <table>
                                                <thead>
                                                    <tr align="center">
                                                        <th style="padding: 0px !important; background: none !important;">
                                                            <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Template Type" ToolTip="Sort By Template Type"
                                                                OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                            <asp:Label ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr align="left">
                                                        <th style="padding: 0px !important; background: none !important;">
                                                            <div class="md-input" style="margin: 0px;">
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" class=" form-control form-control-sm mt-1" onpaste="return false;"
                                                                    MaxLength="40" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="filtertxtHeaderSearch4" runat="server" TargetControlID="txtHeaderSearch4"
                                                                    FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters" FilterMode="ValidChars" ValidChars="@.- " Enabled="True" />
                                                            </div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsActive" Enabled="false" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>' />
                                            <%--<asp:Label ID="lblActive" runat="server" Visible="false" Text='<%# Eval("Is_Active") %>'></asp:Label>--%>
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
                    <div class="row m-0">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:CustomValidator ID="cvErrormsg" runat="server"></asp:CustomValidator>
                        </div>
                    </div>
                    <div class="row">

                        <div class="fixed_btn">
                            <button class="btn btn-success" id="btnShowAll" title="Show All[Alt+H]" causesvalidation="false" onserverclick="btnShowAll_Click" runat="server"
                                type="button" accesskey="H">
                                <i class="fa fa-list"></i>&emsp;S<u>h</u>ow All
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
