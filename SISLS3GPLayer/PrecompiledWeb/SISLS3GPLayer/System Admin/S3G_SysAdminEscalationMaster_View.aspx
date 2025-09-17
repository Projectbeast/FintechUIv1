﻿<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="System_Admin_S3G_SysAdminEscalationMaster_View, App_Web_xht0hlsp" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div>
               
                     <div id="tab-content" class="tab-content">
                <div class="tab-pane fade in active show" id="tab1">
                    <div class="row">
                        <div class="col">
                            <h6 class="title_name">
                                <asp:Label runat="server" Text="Escalation Master" ID="lblHeading" CssClass="styleDisplayLabel">
                                </asp:Label>
                        </div>
                    </div>

                     <div class="row">
                          <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                              <div class="gird">
                            <asp:GridView ID="gvEscalation" runat="server" Width="100%" CssClass="gird_details"
                                AutoGenerateColumns="False" DataKeyNames="Escalation_ID" OnDataBound="gvEscalation_DataBound"
                                OnRowDataBound="gvEscalation_RowDataBound"  OnRowCommand="gvEscalation_RowCommand">
                                <Columns>
                                    <asp:TemplateField Visible="False" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEscalationID" runat="server" Text='<%# Eval("Escalation_ID") %>' /></ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                CommandArgument='<%# Bind("Escalation_ID") %>' CommandName="Query" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Modify">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnModify" runat="server" CommandArgument='<%# Bind("Escalation_ID") %>'
                                                ImageUrl="~/Images/spacer.gif" CommandName="Modify" CssClass="styleGridEdit" /></ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Font-Underline="false" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="4%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Line of Business" SortExpression="LOB">
                                        <HeaderTemplate>
                                            <table>
                                            <thead>
                                            <tr align="center">
                                                 <th style="padding: 0px !important; background: none !important;">
                                                        <asp:LinkButton ID="lnkbtnSortLOB" runat="server" ToolTip="Sort By Line of Business"
                                                            OnClick="FunProSortingColumn" Text="Line of Business" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSortLOB" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                    </th>
                                                </tr>
                                                
                                                <tr align="left">
                                                   <th style="padding: 0px !important; background: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                           class="md-form-control form-control login_form_content_input" Width="95px" MaxLength="50" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars=" ,-" Enabled="True">
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
                                            <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <ItemStyle Width="13%" />
                                        <HeaderStyle CssClass="styleGridHeaderLinkBtn" Font-Underline="false" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role Code" SortExpression="Role_Code">
                                        <HeaderTemplate>
                                             <table>
                                            <thead>
                                            <tr align="center">
                                                 <th style="padding: 0px !important; background: none !important;">
                                                        <asp:LinkButton ID="lnkbtnSortRolecode" OnClick="FunProSortingColumn" ToolTip="Sort By Role Description"
                                                            runat="server" Text="Role Description" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSortRolecode" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                     </th>
                                                </tr>
                                                
                                                   <tr align="left">
                                                   <th style="padding: 0px !important; background: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox ID="txtHeaderSearch2" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                                class="md-form-control form-control login_form_content_input" Width="85px" OnTextChanged="FunProHeaderSearch" MaxLength="50"></asp:TextBox>
                                                           <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                        FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars=" ,-" Enabled="True">
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
                                            <asp:Label ID="lblRoleCode" runat="server" Text='<%# Bind("Role_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <ItemStyle Width="15%" />
                                        <HeaderStyle CssClass="styleGridHeaderLinkBtn" Font-Underline="false" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User" SortExpression="LOB" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUser" runat="server" Text='<%# Bind("Users") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                        <HeaderTemplate>
                                            <table>
                                            <thead>
                                            <tr align="center">
                                                 <th style="padding: 0px !important; background: none !important;">
                                                        <asp:LinkButton ID="lnkbtnUser" runat="server" Text="User" ToolTip="Sort By User"
                                                            OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnUser" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                     </th>
                                                </tr>
                                                
                                                   <tr align="left">
                                                   <th style="padding: 0px !important; background: none !important;">
                                                        <div class="md-input" style="margin: 0px;">
                                                            <asp:TextBox ID="txtHeaderSearch3" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                                CssClass="styleSearchBox" Width="85px" OnTextChanged="FunProHeaderSearch" MaxLength="25"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars=" ,-" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                          <span class="highlight"></span>
                                                            <span class="bar"></span>
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                        </HeaderTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <ItemStyle Width="10%" />
                                        <HeaderStyle CssClass="styleGridHeaderLinkBtn" Font-Underline="false" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NL_Days" HeaderText="NL Hours" HeaderStyle-CssClass="styleGridHeader"
                                        HeaderStyle-HorizontalAlign="right">
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle Width="6%" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="NL SMS" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                        HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkNLSMS" runat="server" Enabled="false" Checked='<%# CheckBool(Eval("NL_SMS").ToString()) %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NL Email" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                        HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkNLEmail" runat="server" Enabled="false" Checked='<%# CheckBool(Eval("NL_Email").ToString()) %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NL CC1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                        HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CbxNLCC1" runat="server" Enabled="false" Checked='<%# CheckBool(Eval("NL_CC").ToString()) %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="HL Hours" DataField="HL_Days" HeaderStyle-CssClass="styleGridHeader">
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="6%" HorizontalAlign="Right"/>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="HL SMS" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                        HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CbxHLSMS" runat="server" Enabled="false" Checked='<%# CheckBool(Eval("HL_SMS").ToString()) %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HL Email" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                        HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CbxHLEmail" runat="server" Enabled="false" Checked='<%# CheckBool(Eval("HL_Email").ToString())%>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HL CC1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                        HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CbxHLCC1" runat="server" Enabled="false" Checked='<%#CheckBool(Eval("HL_CC1").ToString())%>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HL CC2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                        HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CbxHLCC2" runat="server" Enabled="false" Checked='<%# CheckBool(Eval("HL_CC2").ToString())%>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox Enabled="false" runat="server" ID="CbxEscalationActive" />
                                            <asp:Label ID="lblEscalationActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <ItemStyle Width="5%" />
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
                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                            </asp:GridView>
                                  </div>
                              </div>
                         </div>
                     <div class="row">
                             <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                        </div>
                         </div>
                   
                       <div class="row">
                             <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" Style="color: Red;
                                font-size: medium"></asp:Label>
                        </div>
                         </div>
                      <div class="btn_height"></div>
                   <div align="right" class="fixed_btn">
                         <button class="css_btn_enabled" id="btnCreate" onserverclick="btnCreate_Click" runat="server"
                type="button" accesskey="C" title="Create,Alt+C">
                        <i class="fa fa-plus-square" aria-hidden="true"></i>&emsp;<u>C</u>reate
                       </button>
                       <button class="css_btn_enabled" id="btnShowAll" onserverclick="btnShowAll_Click" runat="server"
                         type="button" accesskey="H" title="Show All,Alt+H">
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
