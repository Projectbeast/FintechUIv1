<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" 
    CodeFile="S3GOrg_RiskLimitGuide_View.aspx.cs" Inherits="S3GOrg_RiskLimitGuide_View" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
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
                        <button class="btn btn-outline-success mr-2 btn-create" id="btnCreate" title="Create,Alt+C" onserverclick="btnCreate_Click" runat="server"
                            type="button" accesskey="C" causesvalidation="False">
                            <i class="fa fa-plus" aria-hidden="true"></i>&emsp;<u>C</u>reate
                        </button>
                    </div>
                </div>
            </div>
            <div class="scrollable-content ">
                <div class="section-box py-4 mx-2">

                    <div class="row mx-3 section-box m-0 p-0">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird">
                            <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="False" Width="100%"
                                OnRowDataBound="grvPaging_RowDataBound" OnRowCommand="grvPaging_RowCommand"
                                CssClass="gird_details">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle CssClass="styleGridHeader" Width="25px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                                CommandArgument='<%# Bind("RISK_LIMIT_HDR_ID") %>' CommandName="Query" runat="server">
                                             <i class="fa fa-search" ></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Modify">
                                        <HeaderStyle CssClass="styleGridHeader" Width="25px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                                AlternateText="Modify" CommandArgument='<%# Bind("RISK_LIMIT_HDR_ID") %>' runat="server"
                                                CommandName="Modify">
                                        <i class="fa fa-pencil-square-o" ></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Risk Limit Doc No. ">
                                        <HeaderTemplate>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <thead>
                                                    <tr align="center">
                                                        <th style="padding: 0px !important; background: none !important; border: none">
                                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Risk Limit Doc No." ToolTip="Sort Risk Limit Doc No."
                                                                OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"> </asp:LinkButton>
                                                            <asp:Label ID="imgbtnSort1" runat="server" CssClass="styleImageSortingAsc" OnClientClick="return false;" />
                                                        </th>
                                                    </tr>
                                                    <tr align="left">
                                                        <th style="padding: 0px !important; background: none !important; border: none">
                                                            <div class="md-input" style="margin: 0px;">
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                                    onmouseover="txt_MouseoverTooltip(this)" MaxLength="70" OnTextChanged="FunProHeaderSearch"
                                                                    class="form-control form-control-sm mt-1"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftxtCollectionType" ValidChars="/-() ." TargetControlID="txtHeaderSearch1"
                                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" runat="server"
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
                                            <asp:Label ID="lblCollectionType" runat="server" Text='<%# Bind("RISK_LIMIT_DOC_NUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="From Month">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("From_Month") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Month">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("To_Month") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" />
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
                        <button class="btn btn-success" id="btnShowAll" onserverclick="btnShowAll_Click" title="Show All, Alt+H" runat="server"
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShowAll" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

