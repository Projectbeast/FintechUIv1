<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminDocPath_View.aspx.cs" Inherits="System_Admin_S3GSysAdminDocPath_View" %>

<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="row m-0">
                <div class="col">
                    <h6 class="title_name">
                        <asp:Label runat="server" ID="lblHeading" ToolTip="Document Path Setup" Text="Document Path Setup" CssClass="styleInfoLabel">
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

            <div class="scrollable-content">
                <div class="section-box py-4 mx-2">

                    <div class="row mx-3">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird p-0">
                        <asp:GridView runat="server" ID="grvDocPathMaster" AutoGenerateColumns="False" Width="100%"
                            OnRowCommand="grvDocPathMaster_RowCommand" OnRowDataBound="grvDocPathMaster_RowDataBound" 
                            RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader" CssClass="gird_details">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Document_Path_ID") %>' CommandName="Query" runat="server">
                                    <i class="fa fa-search"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                            CommandArgument='<%# Bind("Document_Path_ID") %>' CommandName="Modify" runat="server">
                                         <i class="fa fa-pencil-square-o"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line of Business" SortExpression="LOB_Name" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLob" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSortLOB" runat="server" ToolTip="Sort By Line of Business"
                                                            Text="Line of Business" CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn">
                                                        </asp:LinkButton>
                                                        <asp:Label ID="imgbtnSortLOB" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                                class="form-control form-control-sm mt-1" onpaste="return false;" OnTextChanged="FunProHeaderSearch" MaxLength="40"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                FilterType="Custom,UppercaseLetters, LowercaseLetters" Enabled="True" ValidChars=" ">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Code" SortExpression="Role_Code" HeaderStyle-Width="15%"><%--SortExpression="Role_Code + ' - ' + Program_Name"--%>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleCode" runat="server" Text='<%# Bind("Role_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <thead>
                                                <tr align="center">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:LinkButton ID="lnkbtnSortRole" runat="server" Text="Role Description" ToolTip="Sort By Role code"
                                                            CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn">
                                                        </asp:LinkButton>
                                                        <asp:Label ID="imgbtnSortRole" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr align="left">
                                                    <th style="padding: 0px !important; background: none !important; border: none">
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                            class="form-control form-control-sm mt-1" onpaste="return false;" OnTextChanged="FunProHeaderSearch" MaxLength="50"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                            FilterType="UppercaseLetters, LowercaseLetters,custom" ValidChars=" " Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Document Flag" SortExpression="PM.Document_Flag"
                                    HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlagDesc" runat="server" Text='<%# Bind("Document_Flag_Desc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortFlagDesc" runat="server" ToolTip="Sort By Document flag"
                                                        Text="Document Flag Desc" CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortFlagDesc" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="Custom,UppercaseLetters, LowercaseLetters" Enabled="True" ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Document Path" SortExpression="Document_Path" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocPath" runat="server" Text='<%# Bind("Document_Path") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <%-- <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnDocPath" runat="server" Text="Document Path" CssClass="styleGridHeader"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSortPath" runat="server" AlternateText="Sort By Path"
                                                OnClick="FunProSortingColumn" ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderDocpath" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" Checked='<%# DataBinder.Eval(Container.DataItem,"Is_Active").ToString().ToUpper() == "TRUE"  %>' />
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
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                        </asp:GridView>
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
            <input type="hidden" id="hdnSortDirection" runat="server" />
            <input type="hidden" id="hdnSortExpression" runat="server" />
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
