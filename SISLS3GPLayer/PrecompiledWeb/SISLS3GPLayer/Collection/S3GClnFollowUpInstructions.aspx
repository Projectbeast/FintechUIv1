﻿<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" enableeventvalidation="false" inherits="Collection_S3GClnFollowUpInstructions, App_Web_4kkykzxm" title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="CustomerDetails"
    TagPrefix="CD" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/S3GCommonLOV.ascx" TagName="FLOV" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
    

  function fnLoadCustomer() {  
        if(document.getElementById('<%= ddlType.ClientID %>').value=="0")
        {
            alert("Select at least one query type");
            document.getElementById('<%= ddlType.ClientID %>').focus();
            return false;
        }        
        document.getElementById('<%= btnLoadCustomer.ClientID %>').click();

        }        
        
        function fnSearchFocus()
        {
          if(document.getElementById('<%= ddlType.ClientID %>').value=="0")
        {
            alert("Select at least one query type");
            document.getElementById('<%= ddlType.ClientID %>').focus();
            return false;
        }
        return true;
        
        }
        
    function funUpValidate()
    {        
        if(document.getElementById('<%= ddlType.ClientID %>')!=null && document.getElementById('<%= ddlType.ClientID %>').value == "0")
        {
            alert("Select at least one query type");
            document.getElementById('<%= ddlType.ClientID %>').focus();
            return false;
        }
        if(document.getElementById('<%= hidCustomerId.ClientID %>').innerText == "")
        {       
            alert("Select the search description");
            return false;
        }
        return true;
    }

    </script>

    <style type="text/css">
        .ajax__calendar
        {
            z-index: 10002 !important;
         
        }
        
    </style>
    <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="stylePageHeading" colspan="4">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Follow Up Instructions" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                          
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="styleFieldLabel" width="25%">
                <asp:Label runat="server" Text="FollowUp No" ID="lblFollowUpNo" CssClass="styleDisplayLabel"></asp:Label>
            </td>
            <td class="styleFieldAlign" width="25%">
                <asp:TextBox ID="txtFollowUpNo" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
            </td>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr id="trHead" runat="server" style="height: 20px">
            <td class="styleFieldLabel" width="25%">
                <asp:Label runat="server" Text="Type" ID="lblType" CssClass="styleDisplayLabel"></asp:Label>
                <span class="styleMandatory">*</span>
            </td>
            <td class="styleFieldLabel" width="25%" valign="middle">
                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                    Width="200px">
                </asp:DropDownList>
            </td>
            <td class="styleFieldLabel" width="25%">
                <asp:Label runat="server" Text="Search Desc" ID="lblSearchDesc" CssClass="styleDisplayLabel"></asp:Label>
                <span class="styleMandatory">*</span>
            </td>
            <td width="25%" class="styleFieldAlign" valign="middle">
                <uc:FLOV ID="ucPopUp" runat="server" />
                <%-- <asp:HiddenField ID="hidUc" runat="server" Value="-1" />--%>
            </td>
        </tr>
        <tr style="display: none">
            <td>
                <cc1:ComboBox Visible="false" ID="ddlSearch" Width="180px" AutoPostBack="true" runat="server"
                    AutoCompleteMode="Suggest" DropDownStyle="DropDownList" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                </cc1:ComboBox>
                <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer"
                    OnClick="btnLoadCustomer_OnClick" CausesValidation="false" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top" style="width: 45%;">
                        <asp:Panel ID="pnlCustomer" runat="server" GroupingText="Customer Details" CssClass="stylePanel">
                            <CD:CustomerDetails ID="ucdCustomer" runat="server" ShowCustomerCode="true" FirstColumnStyle="styleFieldLabel"
                                SecondColumnStyle="styleFieldAlign" />
                            <asp:Label ID="hidCustomerId" runat="server" Style="display: none" />
                        </asp:Panel>
                    </td>
                    <td valign="middle" align="center" width="5%">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnUp" runat="server" CssClass="btnFollowUp_Up" ToolTip="Up" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDown" CssClass="btnFollowUp_Down" runat="server" ToolTip="Down"
                                        OnClick="btnDown_Click" CausesValidation="true" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';return fnCheckPageValidators('vgUp',false);"
                                        ValidationGroup="vgUp" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="center" style="width: 45%;">
                        <asp:Panel ID="pnlAccount" Visible="false" Width="100%" runat="server" GroupingText="Account Details"
                            CssClass="stylePanel">
                            <table style="width: 100%">
                                                             
                                <tr>
                                    <td align="center">
                                        <div class="container" style="height: 101px; width: 95%; overflow-y: auto; overflow-x: hidden;">
                                            <asp:GridView ID="grvMain" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvMain_RowDataBound">
                                                <Columns>
                                                    <%-- LOB  --%>
                                                    <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtLob" runat="server" Text='<%# Bind("LOB")%>'></asp:Label>
                                                            <asp:Label ID="txtLobId" runat="server" Text='<%# Bind("LOB_ID")%>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:TemplateField>
                                                    <%-- Branch  --%>
                                                    <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtBranch" runat="server" Text='<%# Bind("Location")%>'></asp:Label>
                                                            <asp:HiddenField ID="hidIsColor" runat="server" Value='<%# Bind("IsColor")%>' />
                                                            <asp:Label ID="txtBranchId" runat="server" Text='<%# Bind("Location_Id")%>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>
                                                    <%-- PrimeAccountNo  --%>
                                                    <asp:TemplateField HeaderText="Prime Account No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtPrimeAccountNo" runat="server" Text='<%# Bind("PANum")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>
                                                    <%-- SubAccountNo  --%>
                                                    <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtSubAccountNo" runat="server" Text='<%# Bind("SANum")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>
                                                    <%-- Select  --%>
                                                    <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="7%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                <RowStyle HorizontalAlign="Center" />
                                                <EmptyDataTemplate>
                                                    <span>No Records Found...</span>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                  <%--<td height="1px">&nbsp;</td>--%>
                                   <td></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div class="container" style="height: 100px; width: 95%; overflow-y: auto; overflow-x: hidden;">
                                            <asp:GridView runat="server" ID="grvAssetDetails" AutoGenerateColumns="False">
                                                <Columns>
                                                    <%-- PrimeAccountNo  --%>
                                                    <asp:TemplateField HeaderText="Prime Account No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtPrimeAccountNo" runat="server" Text='<%# Bind("PANum")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>
                                                    <%-- SubAccountNo  --%>
                                                    <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtSubAccountNo" runat="server" Text='<%# Bind("SANum")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>
                                                    <%-- Asset Sno/ Vehicle Regn No  --%>
                                                    <asp:TemplateField HeaderText="Asset Sno/ Vehicle Regn No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtAssetSnoReg" runat="server" Text='<%# Bind("AssetSnoReg")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="43%" />
                                                    </asp:TemplateField>
                                                    <%-- Status  --%>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtStatus" runat="server" Text='<%# Bind("Status")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Panel ID="pnlFollowUp" runat="server" Height="280px" Width="100%" Visible="false"
                            GroupingText="Follow Up Details" CssClass="stylePanel" Style="overflow: hidden">
                            <table width="98%">
                                <tr>
                                    <td width="100%" align="left">
                                      <asp:Label ID="lblSTicketNo" runat="server" Text="Ticket Number" CssClass="styleDisplayLabel" ></asp:Label>
                                      &nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:TextBox ID="txtSTicketNo" runat="server" Width="30px" ToolTip="Ticket NUmber" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                Style="text-align: right" MaxLength="3"></asp:TextBox>
                                      <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txtSTicketNo"
                                        FilterType="Numbers" Enabled="True">
                                      </cc1:FilteredTextBoxExtender>
                                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:Label ID="lblSDate" runat="server" Text="Date" CssClass="styleDisplayLabel" ></asp:Label>
                                      &nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:TextBox ID="txtSDate" runat="server" Width="90px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calStartDate" runat="server" Enabled="True" TargetControlID="txtSDate"
                                            PopupButtonID="imgStartDate">
                                        </cc1:CalendarExtender>
                                      <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:Button ID="btnGo" runat="server" 
                                            Text="Go" CssClass="styleGridShortButton" CausesValidation="false" OnClick="btnGo_Click">
                                      </asp:Button>
                                      <asp:Button ID="btnSClear" runat="server" OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_txtSTicketNo').value='';document.getElementById('ctl00_ContentPlaceHolder1_txtSDate').value='';"
                                            Text="Clear" CssClass="styleGridShortButton" CausesValidation="false" OnClick="btnSClear_Click">
                                      </asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%">
                                        <div class="container" style="height: 220px; Width="100%"; overflow-y: auto; overflow-x: hidden;">
                                            <asp:GridView  Width="98%" runat="server" ID="grvFollowUp" AutoGenerateColumns="False" OnRowDataBound="grvFollowUp_RowDataBound">
                                                <Columns>
                                                    <%-- Ticket no  --%>
                                                    <asp:TemplateField HeaderText="Tkt No" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtTicketNo" runat="server" Text='<%# Bind("TicketNo")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                    <%-- Date  --%>
                                                    <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtDate" runat="server" Text='<%#  FormatDate(Eval("Date").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="8%" />
                                                    </asp:TemplateField>
                                                    <%-- From  --%>
                                                    <asp:TemplateField HeaderText="From" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hidFromType" runat="server" Value='<%# Bind("From_Type")%>' />
                                                            <asp:Label ID="hidFromUserName" runat="server" Text='<%# Bind("From_UserName")%>' />
                                                            <asp:HiddenField ID="hidFromID" runat="server" Value='<%# Bind("From")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="13%" />
                                                    </asp:TemplateField>
                                                    <%-- To  --%>
                                                    <asp:TemplateField HeaderText="To" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hidToType" runat="server" Value='<%# Bind("To_Type")%>' />
                                                            <asp:Label ID="hidToUserName" runat="server" Text='<%# Bind("To_UserName")%>'>
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hidToID" runat="server" Value='<%# Bind("To")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="13%" />
                                                    </asp:TemplateField>
                                                    <%-- Query Type  --%>
                                                    <asp:TemplateField HeaderText="Query Type" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hidQueryType" runat="server" Value='<%# Bind("QueryType")%>' />
                                                            <asp:LinkButton ID="lnkQuery" runat="server" Text='<%# Bind("QueryTxt")%>' OnClick="btnView_Click"
                                                                OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';">
                                                            </asp:LinkButton>
                                                            <asp:Label ID="txtQuery" runat="server" Text='<%# Bind("QueryTxt")%>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <%-- Description  --%>
                                                    <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <%--<div style="width:160px;height:50px; overflow-x: auto;">--%>
                                                            <asp:Label ID="txtDescription" Width="150px" runat="server" Text='<%# Bind("Description")%>'></asp:Label>
                                                            <%--</div>--%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:TemplateField>
                                                    <%-- Notify Date  --%>
                                                    <asp:TemplateField HeaderText="Notify Date" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtNotifyDate" runat="server" Text='<%# FormatDate(Eval("NotifyDate").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- Mode  --%>
                                                    <asp:TemplateField HeaderText="Mode" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hidMode" runat="server" Value='<%# Bind("Mode")%>' />
                                                            <asp:Label ID="txtMode" runat="server" Text='<%# Bind("ModeTxt")%>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- Status  --%>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtStatus" runat="server" Text='<%# Bind("Status")%>'></asp:Label>
                                                            <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("Status_Code")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- Add  --%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3px">
                                                        <HeaderTemplate>
                                                            <asp:Button ID="btnAdd" runat="server" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                                                Text="Add" CssClass="styleGridShortButton" CausesValidation="false" OnClick="btnAdd_Click">
                                                            </asp:Button>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnRemove" runat="server" Visible="false" Text="Remove" OnClick="btnRemove_Click"
                                                                CausesValidation="false"></asp:LinkButton>
                                                            <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%# Bind("Version_No")%>' />
                                                            <asp:LinkButton ID="btnView" runat="server" Text="Edit" Visible="false" OnClick="btnView_Click"
                                                                CausesValidation="false"></asp:LinkButton>
                                                            <asp:HiddenField ID="IsMax" runat="server" Value='<%# Bind("IsMax")%>' />
                                                            <asp:HiddenField ID="hidFollowup_Detail_ID" runat="server" Value='<%# Bind("Followup_Detail_ID")%>' />
                                                            <asp:TextBox ID="hidToMailId" runat="server" Visible="false" Text='<%# Bind("ToMailId")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Panel ID="pnlAccountInformation" Visible="false" Width="100%" Height="200px"
                            runat="server" GroupingText="Account Information" CssClass="stylePanel" Style="overflow: hidden">
                            <table width="98%">
                                <tr>
                                    <td width="100%">
                                        <div class="container" style="height: 180px; overflow-y: auto; overflow-x: hidden;">
                                            <asp:GridView runat="server" ID="grvAccountDetails" AutoGenerateColumns="False" OnRowDataBound="grvAccountDetails_RowDataBound">
                                                <Columns>
                                                    <%-- Radion Button   --%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkJE" Text="Ledger" runat="server" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                                                OnClick="lnkJE_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="4%" />
                                                    </asp:TemplateField>
                                                    <%-- LOB --%>
                                                    <%-- <asp:CommandField ShowSelectButton="True" Visible="false" />--%>
                                                    <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtLOBName" runat="server" Text='<%# Bind("LOB")%>'></asp:Label>
                                                            <asp:HiddenField ID="hidLOB" runat="server" Value='<%# Bind("LOB_ID")%>'></asp:HiddenField>
                                                            <asp:HiddenField ID="hidBranch" runat="server" Value='<%# Bind("Location_ID")%>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- PANum  --%>
                                                    <asp:TemplateField HeaderText="Prime Account No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtPANum" runat="server" Text='<%# Bind("PANum")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- SANum  --%>
                                                    <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtSANum" runat="server" Text='<%# Bind("SANum")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- Begin Date  --%>
                                                    <asp:TemplateField HeaderText="Begin Date" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtBeginDate" runat="server" Text='<%# Bind("Creation_Date")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                    </asp:TemplateField>
                                                    <%-- MatureDate   --%>
                                                    <asp:TemplateField HeaderText="Maturity Date" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtMatureDate" runat="server" Text='<%# Bind("MatureDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                    </asp:TemplateField>
                                                    <%-- Finance Amount   --%>
                                                    <asp:TemplateField HeaderText="Finance Amount" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtFinanceAmount" runat="server" Text='<%# Bind("Finance_Amount")%>'></asp:Label>
                                                        </ItemTemplate>                                                        
                                                        <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- UMFC   --%>
                                                    <asp:TemplateField HeaderText="UMFC" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtUMFC" runat="server" Text='<%# Bind("UMFC")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- Billed Amount   --%>
                                                    <asp:TemplateField HeaderText="Billed Amount" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="txtBilledAmount" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                                                runat="server" Text='<%# Bind("BilledAmount")%>' OnClick="txtBilledAmount_Click"></asp:LinkButton>
                                                            <%--<cc1:PopupControlExtender ID="PopExBillAmt" runat="server" TargetControlID="txtBilledAmount"
                                                    PopupControlID="pnlNOD" Position="Bottom" />--%>
                                                        </ItemTemplate>
                                                       <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- Collected Amount   --%>
                                                    <asp:TemplateField HeaderText="Collected Amount" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="txtCollectedAmount" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                                                runat="server" Text='<%# Bind("CollectedAmount")%>' OnClick="txtCollectedAmount_Click"></asp:LinkButton>
                                                            <%--<cc1:PopupControlExtender ID="PopExColAmt" runat="server" TargetControlID="txtCollectedAmount"
                                                    PopupControlID="pnlNOD" Position="Bottom" />--%>
                                                        </ItemTemplate>
                                                      <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- NOD   --%>
                                                    <asp:TemplateField HeaderText="NOD" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtNOD" runat="server" Text='<%# Bind("NOD")%>'></asp:Label>
                                                            <asp:LinkButton ID="lnkNOD" runat="server" Text='<%# Bind("NOD")%>' OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                                                OnClick="lnkNOD_Click"></asp:LinkButton>
                                                            <%-- <cc1:PopupControlExtender ID="PopEx" runat="server" TargetControlID="lnkNOD" PopupControlID="pnlNOD"
                                                    Position="Bottom" />--%>
                                                        </ItemTemplate>
                                                      <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                    </asp:TemplateField>
                                                    <%-- O/S Amount   --%>
                                                    <asp:TemplateField HeaderText="O/S Amount" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtOSAmount" runat="server" Text='<%# Bind("OSAmount")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- Future Principal   --%>
                                                    <asp:TemplateField HeaderText="Future Principal" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtFuturePrincipal" runat="server" Text='<%# Bind("FuturePrincipal")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- LTV Amount   --%>
                                                    <asp:TemplateField HeaderText="LTV Amount" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtLTVAmount" runat="server" Text='<%# Bind("LTVAmount")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- Category Status   --%>
                                                    <asp:TemplateField HeaderText=" Category Status" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtCategoryStatus" runat="server" Text='<%# Bind("CategoryStatus")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                    </asp:TemplateField>
                                                    <%-- DC   --%>
                                                    <asp:TemplateField HeaderText="DC" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtDC" runat="server" Text='<%# Bind("DC")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <%--     <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" ToolTip="Save"
                            Text="Save" OnClick="btnSave_Click" />--%>
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';return fnCheckPageValidators('vgSave');"
                            ValidationGroup="vgSave" OnClick="btnSave_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';return fnConfirmClear();"
                            OnClick="btnClear_Click" />
                        &nbsp;<asp:Button OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                            runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="VGODI" CausesValidation="false"
                            CssClass="styleSubmitButton" Height="26px" OnClick="btnCancel_Click" />
                        <asp:RequiredFieldValidator ID="rfvddlType" runat="server" ControlToValidate="ddlType"
                            ErrorMessage="Select at least one query type" CssClass="styleMandatoryLabel"
                            Display="None" InitialValue="0" SetFocusOnError="True" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlType"
                            ErrorMessage="Select at least one query type" CssClass="styleMandatoryLabel"
                            Display="None" InitialValue="0" SetFocusOnError="True" ValidationGroup="vgUp"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vgSave" runat="server" ValidationGroup="vgSave" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgUp"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                        <asp:CustomValidator ID="cvFollowUp" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnClear" />
        </Triggers>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td>
                <asp:Button OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                    runat="server" ID="btnNOD" CssClass="styleSubmitButton" Text="Ok" Style="display: none" />
                <cc1:ModalPopupExtender ID="moeNOD" runat="server" TargetControlID="btnNOD" PopupControlID="pnlNOD"
                    BackgroundCssClass="styleModalBackground" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Button OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                    runat="server" ID="Button1" CssClass="styleSubmitButton" Text="Ok" Style="display: none" />
                <cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="Button1" PopupControlID="pnlAddFollow"
                    BackgroundCssClass="styleModalBackground" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td width="100%">
                <asp:Panel ID="pnlNOD" GroupingText="" Width="90%" Height="370px" runat="server"
                    Style="display: none" BackColor="White" BorderStyle="Solid">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td class="stylePageHeading" width="100%">
                                        <asp:Label runat="server" ID="lblNodHead" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div class="container" style="height: 300px; width: 100%; overflow: auto;">
                                            <asp:GridView runat="server" ID="grvPopUp" AutoGenerateColumns="true" Width="97%"
                                                OnRowDataBound="grvPopUp_RowDataBound">
                                                <Columns>
                                                </Columns>
                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        &nbsp;<asp:Button runat="server" ID="Button3" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                            CssClass="styleSubmitButton" Text="Close" OnClick="Button3_OnClick" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnlAddFollow" GroupingText="" Width="75%" runat="server" Style="display: none"
                    BackColor="White" BorderStyle="Solid">
                    <asp:UpdatePanel ID="UpAddFollow" runat="server">
                        <ContentTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td colspan="4" class="stylePageHeading">
                                        &nbsp;&nbsp;<asp:Label runat="server" Text="Follow Up" ID="lblFollowUp" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="5px" colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="20%">
                                        &nbsp;&nbsp;<asp:Label ID="lblTicketNo" runat="server" Text="Ticket No"></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnView" />
                                        <asp:HiddenField runat="server" ID="hdnTicketNo" />
                                    </td>
                                    <td width="30%">
                                        <asp:TextBox ID="txtTicketNo" Style="width: 75%" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td width="20%">
                                        &nbsp;&nbsp;<asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                                    </td>
                                    <td width="30%">
                                        <asp:TextBox ID="txtDate" Style="width: 75%" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;<asp:Label ID="lblFromType" runat="server" Text="From Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFrom" Style="width: 75%" AutoPostBack="true" runat="server"
                                            OnSelectedIndexChanged="ddlFrom_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator Display="None" ID="rfvddlFrom" CssClass="styleMandatoryLabel"
                                            InitialValue="0" ErrorMessage="Select the From Type" runat="server" ControlToValidate="ddlFrom"
                                            ValidationGroup="vgOK" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;<asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
                                    </td>
                                    <td>
                                        <uc2:LOV ID="ucFrom" runat="server" style="width: 75%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;<asp:Label ID="lblToType" runat="server" Text="To Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTo" Style="width: 75%" AutoPostBack="true" runat="server"
                                            OnSelectedIndexChanged="ddlTo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator Display="None" ID="rfvddlTo" CssClass="styleMandatoryLabel"
                                            InitialValue="0" ErrorMessage="Select the To Type" runat="server" ControlToValidate="ddlTo"
                                            ValidationGroup="vgOK" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;<asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
                                    </td>
                                    <td>
                                        <uc2:LOV ID="ucTo" style="width: 75%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;<asp:Label ID="lblQuery" runat="server" Text="Query Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlQuery" Style="width: 75%" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlQuery_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator Display="None" ID="rfvddlQuery" CssClass="styleMandatoryLabel" 
                                            InitialValue="0" ErrorMessage="Select the Query Type" runat="server" ControlToValidate="ddlQuery"
                                            ValidationGroup="vgOK" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;<asp:Label ID="lblNotifyDate" runat="server" Text="Notify Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNotifyDt" Width="65%" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgNoDt" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtNotifyDt"
                                            OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="imgNoDt"
                                            ID="ceNotifyDt" Enabled="true">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;
                                        <asp:Label ID="lblMode" runat="server" Text="Mode"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMode" runat="server" Style="width: 75%">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator Display="None" ID="rfvMode" CssClass="styleMandatoryLabel" Enabled="false"
                                            InitialValue="0" ErrorMessage="Select the Mode" runat="server" ControlToValidate="ddlMode"
                                            ValidationGroup="vgOK" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;<asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" Style="width: 75%" Enabled="false" runat="server">
                                        </asp:DropDownList>
                                        <%--  <asp:RequiredFieldValidator Display="None" ID="rfvStatus" CssClass="styleMandatoryLabel"
                                            InitialValue="0" ErrorMessage="Select the Status" runat="server" ControlToValidate="ddlStatus"
                                            ValidationGroup="vgOK" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;<asp:Label ID="lblDesc" runat="server" Text="Description"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescription" Height="100px" Width="92%" MaxLength="300" onkeyup="maxlengthfortxt(300);"
                                            TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="5px" colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Button runat="server" ID="btnOK" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                            ValidationGroup="vgOK" CssClass="styleSubmitButton" Text="Ok" OnClick="btnOK_Click" />
                                        &nbsp;<asp:Button OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                            runat="server" ID="btnCan" CssClass="styleSubmitButton" Text="Cancel" OnClick="btnCan_Click" />
                                    </td>
                                </tr>
                                <tr class="styleButtonArea">
                                    <td colspan="4">
                                        <asp:ValidationSummary runat="server" ID="vsOK" HeaderText="Correct the following validation(s):"
                                            CssClass="styleMandatoryLabel" Width="400px" ShowMessageBox="false" ShowSummary="true"
                                            ValidationGroup="vgOK" />
                                        <asp:CustomValidator ID="cvOK" runat="server" CssClass="styleMandatoryLabel" Enabled="true"
                                            Width="98%"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnOK" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
