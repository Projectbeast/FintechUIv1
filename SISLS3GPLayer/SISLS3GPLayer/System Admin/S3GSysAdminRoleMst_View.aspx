<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GSysAdminRoleMst_View.aspx.cs" Inherits="System_Admin_S3GSysAdminRoleMst_View" %>

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
                            <asp:GridView runat="server" ID="grvUserManagement" OnRowCommand="grvUserManagement_RowCommand"
                                OnRowDataBound="grvUserManagement_RowDataBound" Width="100%" AutoGenerateColumns="false"
                                RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader" class="gird_details">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                                CommandArgument='<%# Bind("User_Group_ID") %>' CommandName="Query" runat="server">
                                        <i class="fa fa-search" ></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                                CommandArgument='<%# Bind("User_Group_ID") %>' CommandName="Modify" runat="server">
                                             <i class="fa fa-pencil-square-o" ></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Group Name" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserGroupName" runat="server" Text='<%# Bind("User_Group_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <thead>
                                                    <tr align="center">
                                                        <th style="padding: 0px !important; background: none !important; border: none">
                                                            <asp:LinkButton ID="lnkbtnUserGroupName" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                                runat="server" ToolTip="Sort By User Group Name" Text="User Group Name"></asp:LinkButton>
                                                            <asp:Label ID="imgbtnUserGroupName" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr align="left">
                                                        <th style="padding: 0px !important; background: none !important; border: none">
                                                            <div class="md-input" style="margin: 0px;">
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" class=" form-control form-control-sm mt-1" onpaste="return false;"
                                                                    runat="server" MaxLength="10" AutoPostBack="true"
                                                                    OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FtxtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                    FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters" FilterMode="ValidChars" ValidChars="@. " Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                            <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By")%>'></asp:Label>
                                            <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>
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
                            <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
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

