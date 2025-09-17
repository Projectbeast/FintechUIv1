﻿<%@ page title="" language="C#" masterpagefile="~/Common/MasterPage.master" autoeventwireup="true" inherits="Common_GetInwardCustomer, App_Web_3y5trygh" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register Src="~/UserControls/LOVPageNavigator.ascx" TagName="LOVPageNavigator"
    TagPrefix="LOVPN" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/LOVPageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
    function popup(strVal,strVal2,strControl,strControl3)
    {
        opener.window.document.getElementById(strControl).value=strVal;
        opener.window.document.getElementById(strControl3).value=strVal2;
        opener.window.document.getElementById(strControl).focus();
        window.close();
    }
    function ScreenClose()
    {        
        window.close();
    }
    </script>

<%--    <asp:UpdatePanel ID="upLOV" runat="server">
        <ContentTemplate>--%>
            <div>
                <table align="center">
                    <tr>
                        <td align="center">                            
                            <asp:Label ID="lblHeaderText" runat="server"></asp:Label>
                           <%-- <asp:TextBox ID="FocusCtrl" Width="0px" runat="server" ></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdnQuery" runat="server" />
                            <input type="hidden" id="hdnSearch" runat="server" />
                            <input type="hidden" id="hdnOrderBy" runat="server" />
                            <input type="hidden" id="hdnSortDirection" runat="server" />
                            <input type="hidden" id="hdnSortExpression" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false" Width="350px"
                                HeaderStyle-CssClass="styleGridHeader" OnRowDataBound="gvList_RowDataBound" ShowHeader="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Code">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Customer Code" ToolTip="Sort By Customer Code"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                            <asp:TextBox ID="txtHeaderSearch1" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                Width="200px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <ACT:FilteredTextBoxExtender ID="txtExtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                ValidChars="- /" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                Enabled="True">
                                            </ACT:FilteredTextBoxExtender>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<% # Bind("ID") %>' Visible="false"></asp:Label>
                                            <asp:LinkButton ID="lbId" runat="server" Text='<% # Bind("Code") %>' CommandName="Select"
                                                CommandArgument='<% # Bind("ID") %>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Customer Name" ToolTip="Sort By Customer Name"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                            <asp:TextBox ID="txtHeaderSearch2" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                Width="200px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <ACT:FilteredTextBoxExtender ID="txtExtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                ValidChars="- /" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                Enabled="True">
                                            </ACT:FilteredTextBoxExtender>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbName" runat="server" Text='<% # Bind("Name") %>' CommandName="Select"
                                                CommandArgument='<% # Bind("ID") %>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Number">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Account Number" ToolTip="Sort By Account Number"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                            <asp:TextBox ID="txtHeaderSearch3" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                Width="200px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <ACT:FilteredTextBoxExtender ID="txtExtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3"
                                                ValidChars="- /" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                Enabled="True">
                                            </ACT:FilteredTextBoxExtender>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbAccName" runat="server" Text='<% # Bind("panum") %>' CommandName="Select"
                                                CommandArgument='<% # Bind("ID") %>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Registration Number">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Registration Number" ToolTip="Sort By Registration Number"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                           
                                            <asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                            <asp:TextBox ID="txtHeaderSearch4" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                Width="200px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <ACT:FilteredTextBoxExtender ID="txtExtHeaderSearch4" runat="server" TargetControlID="txtHeaderSearch4"
                                                ValidChars="- /" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                Enabled="True">
                                            </ACT:FilteredTextBoxExtender>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbRegisNo" runat="server" Text='<% # Bind("Registrationno") %>' CommandName="Select"
                                                CommandArgument='<% # Bind("ID") %>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                              
                                        </ItemTemplate>
                                    </asp:TemplateField>   

                                     <asp:TemplateField HeaderText="CR Number">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkbtnSort5" runat="server" Text="CR Number" ToolTip="Sort By CR Number"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                           
                                            <asp:ImageButton ID="imgbtnSort5" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                            <asp:TextBox ID="txtHeaderSearch5" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                Width="200px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <ACT:FilteredTextBoxExtender ID="txtExtHeaderSearch5" runat="server" TargetControlID="txtHeaderSearch5"
                                                ValidChars="- /" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                Enabled="True">
                                            </ACT:FilteredTextBoxExtender>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbCRNumber" runat="server" Text='<% # Bind("CRNUMBER") %>' CommandName="Select"
                                                CommandArgument='<% # Bind("ID") %>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                              
                                        </ItemTemplate>
                                    </asp:TemplateField>    
                                                                                                      
                                </Columns>
                                <SelectedRowStyle BackColor="AliceBlue" />
                                <PagerStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:GridView>
                            <uc1:PageNavigator ID="ucLOVPageNavigater" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <table align="center">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnClose" runat="server" Text="Exit" AccessKey="E" ToolTip="Exit,Alt+E"  OnClientClick="ScreenClose();"
                            CssClass="styleSubmitShortButton" />
                    </td>
                </tr>
            </table>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
