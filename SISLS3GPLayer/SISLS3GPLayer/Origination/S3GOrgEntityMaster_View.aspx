<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="S3GOrgEntityMaster_View.aspx.cs" Inherits="Origination_S3GOrgEntityMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row m-0">
                <div class="col">
                    <h6 class="title_name">
                        <asp:Label runat="server" Text="" ID="lblHeading" class="styleInfoLabel"> </asp:Label>
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
            <div class="scrollable-content">
                <div class="section-box py-4 mx-2">
                    <div class="row mx-3 section-box m-0 p-0">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird">
                    <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="False" Width="100%" class="gird_details"
                        OnRowCommand="grvEntityMaster_RowCommand" OnRowDataBound="grvPaging_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle CssClass="styleGridHeader" Width="25px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                        CommandArgument='<%# Bind("Entity_ID") %>' CommandName="Query" runat="server">
                                     <i class="fa fa-search" ></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle CssClass="styleGridHeader" Width="25px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                        CommandArgument='<%# Bind("Entity_ID") %>' CommandName="Modify" runat="server">
                                    <i class="fa fa-pencil-square-o" ></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Entity_Type_Name">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <thead>
                                            <tr align="center">
                                                <th style="padding: 0px !important; background: none !important; border: none">
                                                    <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Entity Type" ToolTip="Sort By Entity type"
                                                        OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"> </asp:LinkButton>
                                                    <asp:Label ID="imgbtnSort1" runat="server" CssClass="styleImageSortingAsc" OnClientClick="return false;" />
                                                </th>
                                            </tr>
                                            <tr align="left">
                                                <th style="padding: 0px !important; background: none !important; border: none">
                                                    <div class="md-input" style="margin: 0px;">
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true" class="form-control form-control-sm mt-1"
                                                            OnTextChanged="FunProHeaderSearch" MaxLength="50"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch1" ValidChars=" "
                                                            FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" Enabled="True">
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
                                    <asp:Label ID="lblEntityType" runat="server" Text='<%# Bind("Entity_Type_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <thead>
                                            <tr align="center">
                                                <th style="padding: 0px !important; background: none !important; border: none">
                                                    <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Entity Code" ToolTip="Sort By Entity code"
                                                        OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"> </asp:LinkButton>
                                                    <asp:Label ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" OnClientClick="return false;" />
                                                </th>
                                            </tr>
                                            <tr align="left">
                                                <th style="padding: 0px !important; background: none !important; border: none">
                                                    <div class="md-input" style="margin: 0px;">
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true" class="form-control form-control-sm mt-1"
                                                            OnTextChanged="FunProHeaderSearch" MaxLength="12"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <span class="bar"></span>
                                                    </div>
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEntityCode" runat="server" Text='<%# Bind("Entity_Code") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <thead>
                                            <tr align="center">
                                                <th style="padding: 0px !important; background: none !important; border: none">
                                                    <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Entity Name" ToolTip="Sort By Entity name"
                                                        OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"> </asp:LinkButton>
                                                    <asp:Label ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" OnClientClick="return false;" />
                                                </th>
                                            </tr>
                                            <tr align="left">
                                                <th style="padding: 0px !important; background: none !important; border: none">
                                                    <div class="md-input" style="margin: 0px;">
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true" class="form-control form-control-sm mt-1"
                                                            OnTextChanged="FunProHeaderSearch" MaxLength="90"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <span class="bar"></span>
                                                    </div>
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEntityName" runat="server" Text='<%# Bind("Entity_Name") %>'></asp:Label>
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
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <thead>
                                            <tr align="center">
                                                <th style="padding: 0px !important; background: none !important; border: none">
                                                    <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="NID/RID/NRID/CR Number" ToolTip="Sort By CR Number"
                                                        OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"> </asp:LinkButton>
                                                    <asp:Label ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" OnClientClick="return false;" />
                                                </th>
                                            </tr>
                                            <tr align="left">
                                                <th style="padding: 0px !important; background: none !important; border: none">
                                                    <div class="md-input" style="margin: 0px;">
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" runat="server" AutoPostBack="true" class="form-control form-control-sm mt-1"
                                                            OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                        <span class="highlight"></span>
                                                        <span class="bar"></span>
                                                    </div>
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCR_Number" runat="server" Text='<%# Bind("CR_NUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                        </div>
                    </div>
                    <div class="row m-0 mx-4">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
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
