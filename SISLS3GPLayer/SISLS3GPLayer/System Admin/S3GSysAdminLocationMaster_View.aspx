<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminLocationMaster_View.aspx.cs" Inherits="System_Admin_S3GSysAdminLocationMaster_View"
    Title="S3G - Location Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upLocationMaster" runat="server">
        <ContentTemplate>

            <div class="row m-0">
                <div class="col">
                    <h6 class="title_name">
                        <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleInfoLabel"> </asp:Label>
                    </h6>
                </div>
                <div class="col">
                    <button class="btn btn-outline-success float-right mr-4 btn-create" id="btnCreate" title="Create[Alt+C]" causesvalidation="false" onserverclick="btnCreate_Click" runat="server"
                        type="button" accesskey="C">
                        <i class="fa fa-plus"></i>&emsp;<u>C</u>reate
                    </button>
                </div>
            </div>

            <div class="scrollable-content">
                <div class="section-box py-4 mx-2">
                    <div id="tab-content" class="tab-content">
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <cc1:TabContainer ID="tcLocation" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                            Width="100%" ScrollBars="None" TabStripPlacement="top" AutoPostBack="True" OnActiveTabChanged="tcLocation_ActiveTabChanged">
                            <%-- --%>
                            <cc1:TabPanel runat="server" ID="tpLocationDef" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Branch Definition &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <%--<asp:UpdatePanel ID="upLocationDef" runat="server">
                                            <ContentTemplate>--%>
                                    <cc1:TabContainer ID="tcLocationDef" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                        ScrollBars="Auto" AutoPostBack="True" OnActiveTabChanged="tcLocationDef_ActiveTabChanged">
                                    </cc1:TabContainer>
                                    <div class="row mx-3">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird p-0">
                                            <asp:GridView ID="gvLocationDef" runat="server" AutoGenerateColumns="False" ToolTip="Branch Definition"
                                                Width="100%" OnRowCommand="gvLocationDef_RowCommand" OnRowDataBound="gvLocationDef_RowDataBound" 
                                                RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader" class="gird_details">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                                                AlternateText="Query" runat="server" CommandName="Query" CommandArgument='<%# Bind("Location_Category_ID") %>'>
                                                                <i class="fa fa-search"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Modify" SortExpression="Location_Category_ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                                                AlternateText="Modify" runat="server" CommandName="Modify" CommandArgument='<%# Bind("Location_Category_ID") %>'>
                                                                <i class="fa fa-pencil-square-o"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Branch Code" SortExpression="Location_Code" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocationCode" runat="server" Text='<%# Bind("Location_Code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <thead>
                                                                    <tr align="center">
                                                                        <th style="padding: 0px !important; background: none !important; border: none">
                                                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" CssClass="styleGridHeaderLinkBtn" ToolTip="Sort By Branch Code" Text="Branch Code"
                                                                                OnClick="FunProSortingColumn">
                                                                            </asp:LinkButton>
                                                                            <asp:Label ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                                        </th>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <th style="padding: 0px !important; background: none !important; border: none">
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                                                class="form-control form-control-sm mt-1" MaxLength="50" OnTextChanged="FunProHeaderSearch" onpaste="return false;"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" Enabled="True"
                                                                                ValidChars=" ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </HeaderTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Branch Description" SortExpression="LocationCat_Description" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocationDesc" runat="server" Text='<%# Bind("LocationCat_Description") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <thead>
                                                                    <tr align="center">
                                                                        <th style="padding: 0px !important; background: none !important; border: none">
                                                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" CssClass="styleGridHeaderLinkBtn" Text="Branch Description" ToolTip="Sort By Branch Description"
                                                                                OnClick="FunProSortingColumn">
                                                                            </asp:LinkButton>
                                                                            <asp:Label ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                                        </th>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <th style="padding: 0px !important; background: none !important; border: none">
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                                                class="form-control form-control-sm mt-1" MaxLength="50" OnTextChanged="FunProHeaderSearch" onpaste="return false;"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars=" "
                                                                                Enabled="True">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </HeaderTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hierarchy Description" SortExpression="Location_Description" HeaderStyle-CssClass="styleGridHeader">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHierarchyDesc" runat="server" Text="Hierarchy Description"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHierarchyDescription" runat="server" Text='<%# Bind("Location_Description") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkActive" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>' runat="server" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By")%>'></asp:Label>
                                                            <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <%-- </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tpLocationMap" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Branch Mapping &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upLocationMap" runat="server">
                                        <ContentTemplate>
                                        <div class="row mx-3">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird p-0">
                                                <asp:GridView ID="gvLocationMapping" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    OnRowCommand="gvLocationMapping_RowCommand" OnRowDataBound="gvLocationDef_RowDataBound" 
                                                    RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader" class="gird_details">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                                                    AlternateText="Query" runat="server" CommandName="Query" CommandArgument='<%# Bind("Location_ID") %>'>
                                                                    <i class="fa fa-search"></i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Modify" SortExpression="Location_ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                                                    AlternateText="Modify" runat="server" CommandName="Modify" CommandArgument='<%# Bind("Location_ID") %>'>
                                                                    <i class="fa fa-pencil-square-o"></i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Branch Code" SortExpression="Location_Code" HeaderStyle-CssClass="styleGridHeader">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <thead>
                                                                        <tr align="center">
                                                                            <th style="padding: 0px !important; background: none !important; border: none">
                                                                                <asp:LinkButton ID="lnkbtnSort1" runat="server" CssClass="styleGridHeaderLinkBtn" Text="Branch Code"
                                                                                    ToolTip="Sort By Branch Code" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                <asp:Label ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                                            </th>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <th style="padding: 0px !important; background: none !important; border: none">
                                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" OnTextChanged="FunProHeaderSearch"
                                                                                    runat="server" AutoPostBack="true" class="form-control form-control-sm mt-1" onpaste="return false;"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                                    ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                                                    Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </th>
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLocationCode" runat="server" Text='<%# Bind("Location_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Branch Description" SortExpression="Location" HeaderStyle-CssClass="styleGridHeader">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <thead>
                                                                        <tr align="center">
                                                                            <th style="padding: 0px !important; background: none !important; border: none">
                                                                                <asp:LinkButton ID="lnkbtnSort2" runat="server" CssClass="styleGridHeaderLinkBtn" Text="Branch Description"
                                                                                    ToolTip="Sort By Branch Description" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                <asp:Label ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                                            </th>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <th style="padding: 0px !important; background: none !important; border: none">
                                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" OnTextChanged="FunProHeaderSearch"
                                                                                    runat="server" AutoPostBack="true" class="form-control form-control-sm mt-1" onpaste="return false;"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                                    ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                                                    Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </th>
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblActve" runat="server" Text="Active"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkActive" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>' runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operational" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblIs_Operational" runat="server" Text="Operational"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkIs_Operational" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Operational")))%>' runat="server"
                                                                    Enabled="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By")%>'></asp:Label>
                                                                <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </div>
                </div>
                <div class="row m-0">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
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

                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:CustomValidator ID="cvLocation" runat="server"></asp:CustomValidator>
                        <input type="hidden" id="hdnSortDirection" runat="server" />
                        <input type="hidden" id="hdnSortExpression" runat="server" />
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                        <input type="hidden" id="hdnHierarc" runat="server" />
                    </div>
                </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
