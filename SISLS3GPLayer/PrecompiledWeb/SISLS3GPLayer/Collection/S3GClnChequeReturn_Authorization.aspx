﻿<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="Collection_S3GClnChequeReturn_Authorization, App_Web_la20gqab" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function FunResetGrid() {
            if (document.getElementById('<%=grvCheque.ClientID %>') != null) {
                document.getElementById('<%=grvCheque.ClientID %>').innerText = null;
            }
            document.getElementById('<%=pnlCheque.ClientID %>').style.display = 'none';
            document.getElementById('<%=btnAuthorize.ClientID %>').style.display = 'none';
            document.getElementById('<%=btnUnAuthorize.ClientID %>').style.display = 'none';
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="tab-content" class="tab-content">
                <div class="tab-pane fade in active show" id="tab1">
                    <div class="row">
                        <div class="col">
                            <h6 class="title_name">
                                <asp:Label runat="server" Text="Cheque Return Authorization" ID="lblHeading" CssClass="styleDisplayLabel" BackColor=""> 
                                </asp:Label>
                            </h6>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">


                            <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select the Line of Business"
                                Display="None" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <%--<asp:RequiredFieldValidator ID="rfvBranch" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlBranch" InitialValue="0" ErrorMessage="Select the Location"
                                        Display="None" ValidationGroup="btnSave"></asp:RequiredFieldValidator>--%>
                            <asp:RequiredFieldValidator ID="rfvChequeReturnStatus" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="ddlChequeReturnStatus" InitialValue="0" ErrorMessage="Select the Cheque Return Status"
                                Display="None" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                            <div class="md-input">
                                <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" ToolTip="Line of Business"
                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" class="md-form-control form-control">
                                </asp:DropDownList>
                                <span class="highlight"></span>
                                <span class="bar"></span>
                                <label class="tess">
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                            <div class="md-input">
                                <uc:Suggest ID="ddlBranch" ToolTip="Location" runat="server" AutoPostBack="True"
                                    OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ErrorMessage="Select a Location"
                                    ValidationGroup="btnSave" ServiceMethod="GetBranchList" WatermarkText="--Select--" />
                                <span class="highlight"></span>
                                <span class="bar"></span>
                                <label class="tess">
                                    <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                            <div class="md-input">
                                <asp:TextBox ID="txtFromDate" runat="server" ToolTip="From Date"
                                    class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                <asp:Image ID="imgFrom" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtFromDate"
                                    OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgFrom"
                                    ID="CalendarExtender1" Enabled="True">
                                </cc1:CalendarExtender>
                                <span class="highlight"></span>
                                <span class="bar"></span>
                                <label>
                                    <asp:Label runat="server" Text="From Date" ID="lblFrom">
                                    </asp:Label>
                                </label>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                            <div class="md-input">
                                <asp:TextBox ID="txtToDate" runat="server" ToolTip="To Date"
                                    class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                <asp:Image ID="imgTo" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDate"
                                    OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgTo"
                                    ID="CalendarExtender2" Enabled="True">
                                </cc1:CalendarExtender>
                                <span class="highlight"></span>
                                <span class="bar"></span>
                                <label>
                                    <asp:Label ID="lblTo" runat="server" Text="To Date"></asp:Label>

                                </label>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                            <div class="md-input">
                                <asp:DropDownList ID="ddlChequeReturnStatus" runat="server" AutoPostBack="false"
                                    onchange="return FunResetGrid()" ToolTip="Cheque Return Status" class="md-form-control form-control">
                                    <asp:ListItem Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Authorized" Value="Authorized"></asp:ListItem>
                                    <asp:ListItem Text="Unauthorized " Value="Unauthorized"></asp:ListItem>
                                </asp:DropDownList>

                                <span class="highlight"></span>
                                <span class="bar"></span>
                                <label class="tess">
                                    <asp:Label runat="server" Text="Cheque Return Status" ID="lblChequeReturnStatus" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </label>
                            </div>
                        </div>
                        </div>
                     
                           <div align="right">
                               <button class="css_btn_enabled" id="btnOk" onserverclick="btnOk_Click" runat="server" 
                    type="button" accesskey="G" causesvalidation="false" title="Go,Alt+G" validationgroup="btnSave">
                    
                    <i class="fa fa-floppy-o" aria-hidden="true"></i>&emsp;<u>Go</u>
                                   </button>
                               </div>









                      
                        <tr>
                            <td colspan="4" style="width: 100%;">
                                <asp:Panel ID="pnlCheque" runat="server" Style="display: none" GroupingText="Cheque Return Advice Details"
                                    CssClass="stylePanel">
                                    <asp:HiddenField ID="hdncount" runat="server" />
                                    <asp:GridView ID="grvCheque" runat="server" Width="100%" AutoGenerateColumns="false"
                                        HeaderStyle-CssClass="styleGridHeader" OnRowDataBound="grvCheque_RowDataBound"
                                        RowStyle-HorizontalAlign="Center" OnRowCommand="grvCheque_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cheque Return No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChequeNo" runat="server" Text='<%#Eval("Cheque_Return_Advice_No")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cheque Return Date" Visible="false">
                                                <ItemTemplate>
                                                    <%# FormatDate(Eval("Cheque_Return_Advice_Date").ToString())%>
                                                    <%--<asp:Label ID="lblChequeDate" runat="server" Text='<%#Eval("Cheque_Return_Advice_Date","{0:MM/dd/yyyy}")%>'></asp:Label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Instrument_No" HeaderText="Instrument No" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Customer Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCust" runat="server" Text='<%#Eval("Customer_Code")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Prime Account No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMLA" runat="server" Text='<%#Eval("PANum")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Account No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSLA" runat="server" Text='<%#Eval("SANum")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Transaction_Amount")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Charges">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCharges" runat="server" Text='<%#Eval("Bank_Charges")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="center">Select All
                                                            </td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td>
                                                                <asp:CheckBox ID="chkAllCheques" AutoPostBack="true" OnCheckedChanged="chkAllCheques_OnCheckedChanged"
                                                                    runat="server"></asp:CheckBox>
                                                                <%--onclick="retrun fnDGSelectAll11(this,'chkCheques')"--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td></td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td>
                                                                <%--<asp:CheckBox ID="chkCheques" AutoPostBack="true" Checked='<%# Bind("BolStatus") %>'
                                                            runat="server" OnCheckedChanged="chkCheques_OnCheckedChanged"></asp:CheckBox>--%>
                                                                <asp:CheckBox ID="chkCheques" AutoPostBack="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "BolStatus")))%>'
                                                                    runat="server" OnCheckedChanged="chkCheques_OnCheckedChanged"></asp:CheckBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MonthLock" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonthLock" runat="server" Text='<%#Eval("Month_Option")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cheque Return">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkShow" runat="server" Text="View" OnClick="lnkView_Click" CommandName="query"
                                                        CommandArgument='<%#Eval("Cheque_Return_Advice_No")%>'></asp:LinkButton>
                                                    <asp:Button ID="btnShow" runat="server" Text="View" CommandName="query" CssClass="styleSubmitShortButton" AccessKey="H" ToolTip="Show,Alt+H"
                                                        CommandArgument='<%#Eval("Cheque_Return_Advice_No")%>' Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr class="styleButtonArea" style="padding-left: 4px">
                            <td colspan="4" align="center">
                                <asp:Button runat="server" ID="btnAuthorize" CssClass="styleSubmitButton" Text="Authorize" AccessKey="A" ToolTip="Authorize,Alt+A"
                                    OnClick="btnAuthorize_Click" ValidationGroup="btnSave" CausesValidation="true"
                                    OnClientClick="return fnCheckPageValidators('btnSave');" />
                                &nbsp;<asp:Button runat="server" ID="btnUnAuthorize" CssClass="styleSubmitButton" AccessKey="U" ToolTip="UnAuthorize,Alt+U"
                                    CausesValidation="true" Text="UnAuthorize" OnClick="btnUnAuthorize_Click" ValidationGroup="btnSave"
                                    OnClientClick="return fnCheckPageValidators('btnSave');" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <div style="margin-left: 7px;">
                                    <asp:CustomValidator ID="cvChequeReturnAuthorization" runat="server" CssClass="styleMandatoryLabel"
                                        Enabled="true" Width="98%" />
                                    <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                                </div>
                            </td>
                        </tr>
                        </table>
            <asp:Button ID="BtnHide" runat="server" Style="display: none;" />
                        <cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="BtnHide" PopupControlID="pnlPopupChequeReturn"
                            BackgroundCssClass="styleModalBackground" DropShadow="false" CancelControlID="btnClose" />
                        <asp:Panel ID="pnlPopupChequeReturn" GroupingText="Cheque Return Details" Width="80%"
                            CssClass="stylePanel" Height="550px" runat="server" Style="display: none;" BackColor="White"
                            BorderColor="WhiteSmoke">
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%;" align="left" valign="top">
                                        <asp:Panel ID="pnlChequeRetInfo" GroupingText="Cheque Return Information" runat="server"
                                            CssClass="stylePanel">
                                            <table cellspacing="6px" cellpadding="0">
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <span>Line of Business</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpLob" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label ID="lblpLob" runat="server"></asp:Label>--%>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <span>Cheque Return Number</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpChequeReturnNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label ID="lblpChequeReturnNumber" runat="server"></asp:Label>--%>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <span>Document Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpDocumentDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label ID="lblpDocumentDate" runat="server" Style="text-align: right;"></asp:Label>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="styleFieldLabel">Location</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpBranch" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%--   <asp:Label ID="lblpBranch" runat="server"></asp:Label>--%>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <span>Cheque Number</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpChequeNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label ID="lblpChequeNumber" runat="server"></asp:Label>--%>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <span>Cheque Return Value Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpChequeReturnValueDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%--  <asp:Label ID="lblpChequeReturnValueDate" runat="server"></asp:Label>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <span>Receipt Number</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpReceiptNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label ID="lblpReceiptNumber" runat="server"></asp:Label>--%>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <span class="">Authorized By</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpAuthorizedBy" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%--  <asp:Label ID="lblpAuthorizedBy" runat="server"></asp:Label>--%>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <span class="">Authorized Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="lblpAuthorizedDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label ID="lblpAuthorizedDate" runat="server"></asp:Label>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel GroupingText="Customer Information" Visible="true" ID="pnlCustomerinformation"
                                            runat="server" CssClass="stylePanel">
                                            <uc1:S3GCustomerAddress ID="caCustomerInfo" runat="server" FirstColumnStyle="styleFieldLabel"
                                                SecondColumnStyle="styleFieldAlign" ActiveViewIndex="1" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlBankInfo" runat="server" GroupingText="Bank Information" CssClass="stylePanel">
                                            <table width="100%">
                                                <tr>
                                                    <td class="styleFieldLabel" width="23%">
                                                        <span>Deposit Bank</span>
                                                    </td>
                                                    <td class="styleFieldAlign" align="left" width="23%">
                                                        <asp:TextBox ID="lblDepositBank" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label runat="server" ID="lblDepositBank">
                                            </asp:Label>--%>
                                                    </td>
                                                    <td width="23%" class="styleFieldLabel">
                                                        <span>Bank Account Number</span>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="lblBankAccNo" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label ID="lblBankAccNo" runat="server"></asp:Label>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" width="23%">
                                                        <span>Cheque Amount</span>
                                                    </td>
                                                    <td class="styleFieldAlign" align="left" width="23%">
                                                        <asp:TextBox ID="lblChequeAmount" runat="server" ReadOnly="true" Style="text-align: right;"></asp:TextBox>
                                                        <%--<asp:Label runat="server" ID="lblChequeAmount">
                                            </asp:Label>--%>
                                                    </td>
                                                    <td width="23%" class="styleFieldLabel">
                                                        <span>Bank Charges</span>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="lblBankCharges" runat="server" ReadOnly="true" Style="text-align: right;"></asp:TextBox>
                                                        <%-- <asp:Label ID="lblBankCharges" runat="server"></asp:Label>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel" width="23%">
                                                        <span>Bank Advice Number</span>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="lblBankAdviceNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label runat="server" ID="lblBankAdviceNumber">
                                            </asp:Label>--%>
                                                    </td>
                                                    <td class="styleFieldLabel" width="23%">
                                                        <span>Reason for Return</span>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="lblReasonforReturn" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <%-- <asp:Label runat="server" ID="lblReasonforReturn">
                                            </asp:Label>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" colspan="4">
                                                        <div style="width: 100%; margin-left: 2px; margin-right: 2px;">
                                                            <asp:Panel Width="100%" ID="pnlAcclvlChequeAmount" runat="server" GroupingText="Account Level Cheque Amount"
                                                                CssClass="stylePanel">
                                                                <asp:GridView ID="gvAcountDetails" runat="server" Width="100%" AutoGenerateColumns="false"
                                                                    HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" OnRowDataBound="gvAcountDetails_RowDataBound">
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="Prime Account No" DataField="PANum" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField HeaderText="Sub Account No" DataField="SANum" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField HeaderText="Account Description" DataField="Account Description" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:TemplateField HeaderText="Cheque Amount" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblChequeAmounts" runat="server" Text='<%#Bind("Transaction_Amount") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Cheque Amount" DataField="Transaction_Amount" ItemStyle-HorizontalAlign="Right"
                                                                            Visible="false" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td colspan="2">
                                        <%--<asp:Button ID="Button1" OnClick="btnAuthorize_Click" runat="server" Text="Authorize"
                                CssClass="styleSubmitButton" />--%>
                                        <asp:Button ID="btnClose" runat="server" Text="Exit" CssClass="styleSubmitShortButton" AccessKey="X" ToolTip="Exit,Alt+X" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
