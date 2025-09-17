<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="Collection_S3GClnEcsProcess, App_Web_x5tdsxrh" title="Untitled Page" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

function fnGetLines()
{
//    if(document.getElementById("<%= ddlLOB.ClientID %>").value=="0")
//    {
//        alert("Select the Line of Business.");
//        document.getElementById("<%= ddlLOB.ClientID %>").focus();
//        return false;
//    }  
//    if(document.getElementById("<%= ddlBranch.ClientID %>").value=="0")
//    {
//        alert("Select the Branch.");
//        document.getElementById("<%= ddlBranch.ClientID %>").focus();
//        return false;
//    }
    if(document.getElementById("<%= ddlFixedBillDate.ClientID %>").value=="0")
    {
        alert("Select the Fixed Billing Date.");
        document.getElementById("<%= ddlFixedBillDate.ClientID %>").focus();
        return false;
    }
    return true;
   
}

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="4">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="ECS Process" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                       <%--  <span class="styleMandatory">*</span> --%>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" Text="ECS No" ID="lblECSNo" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtECSNo" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleDisplayLabel"></asp:Label>
                        <%-- <span class="styleMandatory">*</span>--%>
                    </td>
                    <td class="styleFieldAlign">
                       <uc:Suggest ID="ddlBranch" ToolTip="Location" runat="server" AutoPostBack="True" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                          ServiceMethod="GetBranchList"  WatermarkText="--All--" />
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" Text="ECS Document Date" ID="lblECSDate" CssClass="styleDisplayLabel"></asp:Label>
                        <span class="styleMandatory">*</span>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtECSDate" runat="server"></asp:TextBox>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtECSDate"
                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="Image1"
                            ID="CalendarExtender2" Enabled="false">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" Text="Fixed Billing Date" ID="lblFixedBillingDate" CssClass="styleDisplayLabel"></asp:Label>
                        <span class="styleMandatory">*</span>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlFixedBillDate" runat="server" Width="165px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlFixedBillDate_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnGetLines" runat="server" OnClientClick="return fnGetLines();"
                            CssClass="styleSubmitButton" Text="Get Lines" OnClick="btnGetLines_Click" />
                        <asp:Button ID="btnDeleteECS" runat="server" CssClass="styleSubmitButton" Visible="false"
                            Text="Delete ECS" OnClick="btnDeleteECS_Click" />
                    </td>
                    <%--   <td class="styleFieldLabel">
                        <asp:Label runat="server" Text="Select All" ID="lblSelectAll" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:CheckBox ID="chkSelectAll" runat="server" Width="165px" AutoPostBack="true"></asp:CheckBox>
                    </td>--%>
                </tr>
                <tr runat="server" id="trStatus" visible="false">
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" Text="Status" ID="Label1" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlStaus" runat="server" Width="165px">
                            <asp:ListItem Text="Authorize" Value="A"></asp:ListItem>
                            <asp:ListItem Text="Process" Value="P"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" width="100%">
                        <asp:Panel ID="pnlGrid" runat="server" CssClass="stylePanel" Style="display: none;">
                            <asp:GridView Width="100%" ID="grvEcsProcess" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false" OnRowCreated="grvEcsProcess_RowCreated"
                                OnRowDataBound="grvEcsProcess_RowDataBound">
                                <RowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %> '></asp:Label>
                                            <asp:Label ID="lblCustomerId" Visible="false" runat="server" Text='<%#Eval("Customer_Id")%>'></asp:Label>
                                            <asp:Label ID="lblInstallmentNo" Visible="false" runat="server" Text='<%#Eval("Installment_Number")%>'></asp:Label>
                                            <asp:Label ID="lblRepayId" Visible="false" runat="server" Text='<%#Eval("RepayId")%>'></asp:Label>
                                            <asp:Label ID="lblBranchId" Visible="false" runat="server" Text='<%#Eval("Location_ID")%>'></asp:Label>
                                            <asp:Label ID="lblEcsStatus" Visible="false" runat="server" Text='<%#Eval("ECS_STATUS")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Customer Name" DataField="Customer_Name" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Prime Account Number" DataField="PANum" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Sub Account Number" DataField="SANum" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText="Installment Date" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# FormatDate(Eval("Installment_Date").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Installment Amount" DataField="Installment_Amount" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="Bank MICR No" DataField="MICR_Code" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Bank Name" DataField="Bank_Name" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Bank Account No" DataField="Account_Number" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Bank Account Type" DataField="Name" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Receipt No" DataField="Receipt_No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Receipt Date" DataField="Receipt_Date" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Value Date" DataField="Value_Date" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblGenStatus" runat="server" Text="Generate Status" />
                                            <br />
                                            <asp:CheckBox ID="chkHrdGenStatus" runat="server" CssClass="styleGridHeader" CausesValidation="false"
                                                OnCheckedChanged="chkHrdGenStatus_CheckedChanged" AutoPostBack="True" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkGenStatus" runat="server" OnCheckedChanged="chkGenStatus_CheckedChanged"
                                                AutoPostBack="True"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDelete" runat="server" Text="Delete" />
                                            <br />
                                            <asp:CheckBox ID="chkHrdDelete" runat="server" CssClass="styleGridHeader" CausesValidation="false"
                                                OnCheckedChanged="chkHrdDelete_CheckedChanged" AutoPostBack="True" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDelete" runat="server" OnCheckedChanged="chkDelete_CheckedChanged"
                                                AutoPostBack="True"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                </Columns>
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:GridView>
                            <uc1:PageNavigator ID="ucCustomPaging" runat="server" Visible="false" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        &nbsp;<asp:Button ID="btnReceiptGen" Enabled="false" runat="server" CausesValidation="true"
                            CssClass="styleSubmitLongButton" Text="Receipt Generation" OnClientClick="return fnCheckPageValidators('btnGetLines');"
                            ValidationGroup="btnGetLines" OnClick="btnReceiptGen_Click" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            OnClientClick="return fnConfirmClear();" Text="Clear" OnClick="btnClear_Click" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" Text="Cancel" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                        <%--<asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ControlToValidate="ddlLOB"
                            ErrorMessage="Select the Line of Business" CssClass="styleMandatoryLabel" Display="None"
                            InitialValue="0" SetFocusOnError="True" ValidationGroup="btnGetLines"></asp:RequiredFieldValidator>--%>
                        <%--<asp:RequiredFieldValidator ID="rfvddlBranch" runat="server" ControlToValidate="ddlBranch"
                            ErrorMessage="Select the Branch" CssClass="styleMandatoryLabel" Display="None"
                            InitialValue="0" SetFocusOnError="True" ValidationGroup="btnGetLines"></asp:RequiredFieldValidator>--%>
                        <asp:RequiredFieldValidator ID="rfvtxtECSDate" runat="server" CssClass="styleMandatoryLabel"
                            Display="None" ErrorMessage="Enter the ECS Document Date" SetFocusOnError="True"
                            ControlToValidate="txtECSDate" ValidationGroup="btnGetLines"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlFixedBillDate"
                            ErrorMessage="Select the Fixed Billing Date" CssClass="styleMandatoryLabel" Display="None"
                            InitialValue="0" SetFocusOnError="True" ValidationGroup="btnGetLines"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnGetLines" />
                        <asp:CustomValidator ID="cvECSProcess" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                        <input type="hidden" id="hidDelete" runat="server" value="0" />
                        <input type="hidden" id="hdnSortDirection" runat="server" />
                        <input type="hidden" id="hdnSortExpression" runat="server" />
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
