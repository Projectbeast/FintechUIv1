<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="S3GClnECSSpoolFormatDetails_View, App_Web_3jwyxbhk" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="stylePageHeading">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvPaging_RowDataBound"
                    HeaderStyle-CssClass="styleGridHeader" Width="100%" OnRowCommand="grvPaging_RowCommand">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    CommandArgument='<%# Bind("ECS_Spool_ID") %>' CommandName="Query" runat="server" ToolTip="Query" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                    CommandArgument='<%# Bind("ECS_Spool_ID") %>' CommandName="Modify" runat="server" ToolTip="Modify" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Line of Business" SortExpression="LOB">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                ToolTip="Sort By Line of Business" OnClick="FunProSortingColumn" Text="Line of Business"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort1" runat="server" CssClass="styleImageSortingAsc"
                                                ImageAlign="Middle" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" MaxLength="40"
                                                AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" Enabled="True" ValidChars="-| ">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLOBCode" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch" SortExpression="Location">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                ToolTip="Sort By Location" OnClick="FunProSortingColumn" Text="Location"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort2" runat="server" CssClass="styleImageSortingAsc"
                                                ImageAlign="Middle" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" CssClass="styleSearchBox"
                                                AutoPostBack="true" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="-| "
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bank" SortExpression="Bank">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort3" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                ToolTip="Sort By Bank Name" OnClick="FunProSortingColumn" Text="Bank"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort3" runat="server" CssClass="styleImageSortingAsc"
                                                ImageAlign="Middle" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" CssClass="styleSearchBox"
                                                AutoPostBack="true" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch3"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="- "
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBank" runat="server" Text='<%# Bind("Bank") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
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
            </td>
        </tr>
        <tr align="center">
            <td align="center" valign="top">
                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
            </td>
        </tr>
        <tr class="styleButtonArea">
            <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                <span runat="server" id="lblErrorMessage" cssclass="styleMandatory"></span>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" CssClass="styleSubmitButton"
                    Text="Create" ToolTip="Create"></asp:Button>
                <asp:Button ID="btnShowAll" runat="server" OnClick="btnShowAll_Click" CssClass="styleSubmitButton"
                    Text="Show All" ToolTip="Show All" />
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
</asp:Content>
