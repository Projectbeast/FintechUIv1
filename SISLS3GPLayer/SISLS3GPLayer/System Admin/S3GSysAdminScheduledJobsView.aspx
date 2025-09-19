<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3GSysAdminScheduledJobsView.aspx.cs" Inherits="S3GSysAdminScheduledJobsView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
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
                    <button class="btn btn-outline-success float-right mr-4 btn-create" id="btnCreate" title="Schedule[Alt+S]" causesvalidation="false" onserverclick="btnCreate_Click" runat="server"
                        type="button" accesskey="S">
                        <i class="fa fa-clock-o"></i>&emsp;<u>S</u>chedule
                    </button>
                </div>
            </div>

            <div class="scrollable-content ">
                <div class="section-box py-4 mx-2">
                    <div class="row mx-3">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird p-0">
                <asp:GridView ID="grvScheduledJobs" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="gird_details"
                    OnRowCommand="grvScheduledJobs_RowCommand" OnRowDataBound="grvScheduledJobs_RowDataBound"
                    RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="imgbtnQuery" CssClass="styleGridQuery"
                                    CommandArgument='<%# Bind("ID") %>' CommandName="Query" runat="server">
                            <i class="fa fa-search" ></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="imgbtnEdit" CssClass="styleGridEdit"
                                    CommandArgument='<%# Bind("ID") %>' CommandName="Modify" runat="server">
                                 <i class="fa fa-pencil-square-o" ></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job Description" HeaderStyle-CssClass="styleGridHeader">
                            <ItemTemplate>
                                <asp:Label ID="lblJOB" runat="server" Text='<%# Bind("JobDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <thead>
                                        <tr align="center">
                                            <th style="padding: 0px !important; background: none !important; border: none">
                                                <asp:LinkButton ID="lnkbtnSort1" CssClass="styleGridHeaderLinkBtn" runat="server" Text="Job Description" ToolTip="Sort By Job Description"
                                                    OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                <asp:Label ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr align="left">
                                            <th style="padding: 0px !important; background: none !important; border: none">
                                                <div class="md-input" style="margin: 0px;">
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" class="form-control form-control-sm mt-1" onpaste="return false;"
                                                        runat="server" AutoPostBack="true"
                                                        OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="filterLOB" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="- " />
                                                </div>
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job Nature" HeaderStyle-CssClass="styleGridHeader">
                            <HeaderTemplate>
                                <asp:Label ID="lblHJobNature" runat="server" Text="Job Nature"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblJobNature" runat="server" Text='<%# Bind("JobNature") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Frequency" HeaderStyle-CssClass="styleGridHeader">
                            <HeaderTemplate>
                                <asp:Label ID="lblHFrequency" runat="server" Text="Frequency"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFrequency" runat="server" Text='<%# Bind("Frequency") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Date" HeaderStyle-CssClass="styleGridHeader">
                            <HeaderTemplate>
                                <asp:Label ID="lblHStartDate" runat="server" Text="Start Date"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Time" HeaderStyle-CssClass="styleGridHeader">
                            <HeaderTemplate>
                                <asp:Label ID="lblHStartTime" runat="server" Text="Start Time"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStartTime" runat="server" Text='<%# Bind("StartTime") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:Label ID="lblHActive" runat="server" Text="Active"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsActive" Enabled="false" runat="server" />
                                <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                            <ItemTemplate>
                                <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%#Eval("Created_By")%>'></asp:Label>
                                <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%#Eval("User_Level_ID")%>'></asp:Label>
                            </ItemTemplate>
                            <%--<HeaderStyle CssClass="styleGridHeader"></HeaderStyle>--%>
                            <%--<ItemStyle HorizontalAlign="Center"></ItemStyle>--%>
                        </asp:TemplateField>
                    </Columns>
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
                            <asp:CustomValidator ID="cvErrormsg" runat="server">
                            </asp:CustomValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="fixed_btn">
                            <button class="btn btn-success" id="btnShowAll" title="Show All[Alt+H]" causesvalidation="false" onserverclick="btnShowAll_Click" runat="server"
                                type="button" accesskey="H">
                                <i class="fa fa-list"></i>&emsp;S<u>h</u>ow All
                            </button>
                            <button class="btn btn-success" id="btnMonitor" visible="false" title="Schedule Monitor[Alt+M]" causesvalidation="false" onserverclick="btnMonitor_Click" runat="server"
                                type="button" accesskey="M">
                                <i class="fa fa-calendar"></i>&emsp;Schedule <u>M</u>onitor
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
