﻿<%@ page language="C#" masterpagefile="~/Common/FAMasterPageCollapse.master" autoeventwireup="true" inherits="Sysadm_FASysHierarchyMaster_Add, App_Web_tfexpijv" title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Panel ID="PNLHierachyMaster" runat="server" CssClass="stylePanel" GroupingText="Hierarchy Details"
                            Width="60%">
                           <%-- <table cellpadding="0" cellspacing="0" width="98%">
                                <tr>
                                    <td colspan="4 " width="80%">--%>
                                        <asp:GridView runat="server" ID="GRVHeirachy" AutoGenerateColumns="False" Width="100%"
                                            ToolTip="Hierarchy Master" OnRowDataBound="GRVHeirachy_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Hierarchy" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHierachy" runat="server" Text='<%# Bind("Hierachy")%>' ToolTip="Hierarchy"
                                                            Width="100%"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Width" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWidth" runat="server" Text='<%# Bind("Width")%>' MaxLength="1"
                                                           onmouseover="txt_MouseoverTooltip(this)" ToolTip="Width" Width="97%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTEWidth" runat="server" TargetControlID="txtWidth"
                                                            FilterType="Custom" Enabled="true" ValidChars="123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location Description" HeaderStyle-Width="50%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLocationDesc" runat="server" Text='<%# Bind("Location_Description")%>'
                                                         onmouseover="txt_MouseoverTooltip(this)"   ToolTip="Location Description" onblur="FunTrimwhitespace(this,'Location Description');"
                                                            MaxLength="50" Width="97%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ExttxtLocationDesc" runat="server" TargetControlID="txtLocationDesc"
                                                            ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                            Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Active" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkActive" runat="server" ToolTip="Active Status" AutoPostBack="true"
                                                            OnCheckedChanged="chkActive_CheckedChanged" />
                                                        <asp:Label ID="lblActive" runat="server" Text='<%# Bind("Active")%>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Operational Location" HeaderStyle-Width="5%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkOperational" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem, "Operational").ToString().ToUpper() == "TRUE"%>'
                                                            AutoPostBack="true" OnCheckedChanged="chkOperational_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <%--</div>--%>
                                    <%--</td>
                                </tr>
                            </table>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <%--Total--%>
                <tr class="styleButtonArea" align="center" style="padding-left: 4px">
                    <td>
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                            CssClass="styleSubmitButton" Text="Save" ValidationGroup="VGHierachy" ToolTip="Save,Alt+S"
                             OnClick="btnSave_Click" AccessKey="S"  />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear_FA" OnClientClick="return fnConfirmClear();" ToolTip="Clear_FA,Alt+L" AccessKey="L" 
                            Enabled="false" />
                        &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Exit" ValidationGroup="VGNOC"
                            CausesValidation="false" CssClass="styleSubmitButton" ToolTip="Exit,Alt+X" AccessKey="X" 
                            OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsHierachy" HeaderText="Correct the following validation(s):"
                            Height="177px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="VGHierachy" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:CustomValidator ID="cvHierarchyMaster" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
                <%--<tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>--%>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
