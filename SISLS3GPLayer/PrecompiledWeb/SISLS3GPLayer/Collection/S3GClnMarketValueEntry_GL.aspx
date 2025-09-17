﻿<%@ page language="C#" autoeventwireup="true" masterpagefile="~/Common/S3GMasterPageCollapse.master" inherits="CLN_S3GCLNMarketValueEntry_GL, App_Web_la20gqab" %>

<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="4">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Market Value Entry" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                        <%--  <span class="styleMandatory">*</span>--%>
                    </td>
                    <td class="styleFieldAlign" width="27%">
                        <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <%--   Commented By Manikandan. R for the Change Req from Bashyam Sir(30/12/2011)--%>
                        <%-- <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ControlToValidate="ddlLOB"
                            ErrorMessage="Select a Line of Business" CssClass="styleMandatoryLabel" Display="None"
                            InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" Text="Market Value No" ID="lblMVNumber" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign" width="27%">
                        <asp:TextBox ID="txtMVNo" runat="server" ReadOnly="True" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" ID="lblBranch" Text="Location" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign" width="27%">
                        <asp:DropDownList ID="ddlBranch" AutoPostBack="true" runat="server" Width="165px"
                            OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" Text="Market Value Entry Date" ID="lblMVDate" CssClass="styleDisplayLabel"></asp:Label>
                        <span class="styleMandatory">*</span>
                    </td>
                    <td class="styleFieldAlign" width="27%">
                        <asp:TextBox ID="txtMVDate" runat="server" ContentEditable="False" OnTextChanged="txtMVDate_TextChangeEvent"
                            AutoPostBack="true" Enabled="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvtxtMvDate" runat="server" CssClass="styleMandatoryLabel"
                            Display="None" ErrorMessage="Select Market Value Entry date" SetFocusOnError="true"
                            ControlToValidate="txtMvDate"></asp:RequiredFieldValidator>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtMVDate"
                            PopupButtonID="Image1" ID="CalendarExtender2" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" colspan="2">
                        &nbsp;
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" Text="Status" ID="lblStatus" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:CheckBox ID="chkStatus" runat="server" Checked Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" width="100%">
                        <asp:Panel Width="100%" ID="pnlOption1" runat="server" GroupingText="Option1" CssClass="stylePanel">
                            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>--%>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 10%">
                                        <asp:RadioButton ID="rdoOption1" runat="server" Enabled="false" Text="Option1" OnCheckedChanged="rdoOption1_CheckedChanged"
                                            AutoPostBack="True" />
                                    </td>
                                    <td class="styleFieldLabel" width="13%">
                                        <asp:Label ID="lblCustomer" runat="server" Text="Customer Code" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" width="17%">
                                        <asp:Label ID="lblMLA" runat="server" Text="Prime Account No" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" width="17%">
                                        <asp:Label ID="lblSLA" runat="server" Text="Sub Account No" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" width="12%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 10%">
                                        <%--&nbsp;--%>
                                    </td>
                                    <td class="styleFieldAlign" width="17%">
                                        <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        &nbsp;--%>
                                        <uc2:LOV ID="ucCustomerCode" onblur="fnLoadCustomer()" DispalyContent="Both" runat="server"
                                            strLOV_Code="CMB" />
                                        <asp:Button ID="btnLoadCustomer" CausesValidation="false" runat="server" Style="display: none"
                                            Text="Load Customer" OnClick="btnLoadCustomer_OnClick" />
                                        <%-- <asp:TextBox ID="cmbCustomerCode" runat="server" MaxLength="50" Width="5%" AutoPostBack="true"
                                            OnTextChanged="cmbCustomerCode_TextChanged" Style="display: none"></asp:TextBox>--%>
                                        <asp:TextBox ID="cmbCustomerCode" runat="server" MaxLength="50" Style="display: none"
                                            ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="cmbCustomerCode"
                                            InitialValue="0" Display="None" ErrorMessage="Select the Customer" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <%--<cc1:AutoCompleteExtender ID="AutoCompleteExtenderDemo" runat="server" TargetControlID="cmbCustomerCode"
                                            ServiceMethod="GetCustomerList" MinimumPrefixLength="1" CompletionSetCount="20"
                                            DelimiterCharacters="" Enabled="True" ServicePath="">
                                        </cc1:AutoCompleteExtender>--%>
                                        <%--</ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlMLA" AutoPostBack="true" runat="server" Enabled="False"
                                            OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvMLA" runat="server" ControlToValidate="ddlMLA"
                                            InitialValue="0" Display="None" ErrorMessage="Select the Prime Account No" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlSLA" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged"
                                            AutoPostBack="true" runat="server" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                    <asp:RequiredFieldValidator ID="rfvSLA" ControlToValidate="ddlSLA" runat="server"
                                        Enabled="false" InitialValue="0" Display="None" ErrorMessage="Select the Sub Account No"
                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <td valign="middle" align="center">
                                        <asp:Button ID="btnOkOption1" Enabled="false" runat="server" Text="Ok" CausesValidation="true"
                                            CssClass="styleSubmitShortButton" OnClick="btnOkOption1_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%></asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel Width="100%" ID="pnlOption2" runat="server" GroupingText="Option2" CssClass="stylePanel">
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td class="styleFieldLabel" width="13%">
                                        <asp:RadioButton ID="rdoOption2" runat="server" Text="Option2" OnCheckedChanged="rdoOption2_CheckedChanged"
                                            AutoPostBack="True" />
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblAssetClass" runat="server" Text="Asset Class" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblAssetMake" runat="server" Text="Asset Make" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblAssetType" runat="server" Text="Asset Type" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblAssetModel" runat="server" Text="Asset Model" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblForAsset" runat="server" Text="For Asset" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        &nbsp;
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlAssetClass" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAssetClass_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlAssetClass" ControlToValidate="ddlAssetClass"
                                            runat="server" InitialValue="0" SetFocusOnError="true" ErrorMessage="Select Asset Class"
                                            Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlAssetMake" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAssetMake_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlAssetMake" ControlToValidate="ddlAssetMake"
                                            runat="server" InitialValue="0" SetFocusOnError="true" ErrorMessage="Select Asset Make"
                                            Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlAssetType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlAssetType" runat="server" ControlToValidate="ddlAssetType"
                                            InitialValue="0" SetFocusOnError="true" ErrorMessage="Select Asset Type" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:DropDownList ID="ddlAssetModel" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAssetModel_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlAssetModel" runat="server" InitialValue="0"
                                            ControlToValidate="ddlAssetModel" SetFocusOnError="true" ErrorMessage="Select Asset Model"
                                            Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="center" class="styleFieldAlign">
                                        <asp:CheckBox ID="chkForAsset" runat="server" Enabled="false" AutoPostBack="true"
                                            OnCheckedChanged="chkForAsset_CheckedChanged" />
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnOption2" runat="server" Text="Ok" CausesValidation="true" OnClick="btnOption2_Click"
                                            CssClass="styleSubmitShortButton" />
                                    </td>
                                </tr>
                                <tr>
                                    <br />
                                    <td colspan="2">
                                    </td>
                                    <td colspan="2" align="right">
                                        <asp:Label runat="server" Text="Current Market Value in " ID="lblMarketValue" Visible="false"
                                            CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtMarketValue" runat="server" Visible="false" MaxLength="10" style="text-align:right"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvMarketVal" runat="server" Enabled="false" ControlToValidate="txtMarketValue"
                                            SetFocusOnError="true" ErrorMessage="Enter the Current Market Value" Display="None"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="fetxtMarketValue" runat="server"
                                            TargetControlID="txtMarketValue" FilterType="Numbers" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <tr id="pnlGrid" runat="server">
                        <td colspan="4">
                            <%-- <asp:Panel runat="server" Height="120px" CssClass="stylePanel">--%>
                            <table width="100%" runat="server">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grvMarket" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader"
                                            Width="100%" OnRowDataBound="grvMarket_RowDataBound">
                                            <RowStyle HorizontalAlign="Left" Width="100%" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="SI.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSINO" runat="server" MaxLength="20" Text="<%#Container.DataItemIndex+1 %> "></asp:Label>
                                                        <asp:Label ID="lblAssetID" runat="server" Text='<%#Eval("Asset_ID")%>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="LOB" HeaderText="Line of Business" />
                                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                                <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" />
                                                <asp:TemplateField HeaderText="Prime Account No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPANum" runat="server" Text='<%#Eval("PANum")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Account No ">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSANum" runat="server" Text='<%#Eval("SANum")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="Asset_Code" HeaderText="Asset Code" />--%>
                                                <asp:BoundField DataField="Asset_Description" HeaderText="Asset Description" />
                                                <%-- <asp:BoundField DataField="Book_Value" HeaderText="Book Value" ItemStyle-HorizontalAlign="Right" />--%>
                                                <asp:TemplateField HeaderText="Book Value" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBookValue" runat="server" Text='<%#Eval("Book_Value")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Entered Value" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblCurValueH" runat="server" Text="Last Entered Value"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurValue" runat="server" Text='<%#Eval("LastEnteredValue")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="LastEnteredValue" HeaderText="Last Entered Value" ItemStyle-HorizontalAlign="Right"
                                                    Visible="false" />
                                                <asp:TemplateField HeaderText="Last Entered Date">
                                                    <ItemTemplate>
                                                        <%# FormatDate(Eval("LastEnteredDate").ToString())%>
                                                        <asp:Label ID="lblLastDate" runat="server" Text='<%#Eval("LastEnteredDate")%>' Visible="false"
                                                            Style="text-align: right"> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  <asp:BoundField Dat
                                                <%--  <asp:BoundField DataField='<%#Eval("DateTime.Parse(LastEnteredDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat)")%>' HeaderText="Last Entered Date" />--%>
                                                <asp:TemplateField HeaderText="Current Value" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblCurval" runat="server" Text="Current Value"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox ID="chkCurvalue" runat="server" AutoPostBack="true" OnCheckedChanged="chkCurvalue_CheckedChanged1" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCurValue" runat="server" MaxLength="10" Text='<%#Eval("CurValue") %>'
                                                            Width="80px" Style="text-align: right"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderCurValue" runat="server"
                                                            TargetControlID="txtCurValue" FilterType="Numbers" Enabled="true">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:TextBox ID="txtCurValue" runat="server" MaxLength="10" 
                                     Text='<%#Eval("CurValue")%>' Width="80px"></asp:TextBox>--%>
                                                        <%--          <asp:RequiredFieldValidator ID="rfvtxtCurValue" runat="server" 
                                         ControlToValidate="txtCurValue" CssClass="styleMandatoryLabel" Display="None" 
                                         ErrorMessage="Please enter a market value" SetFocusOnError="True" 
                                         ValidationGroup="btnSave"></asp:RequiredFieldValidator>--%>
                                                        <%--<cc1:MaskedEditExtender ID="MEEtxtCurValue" runat="server" 
                                     InputDirection="RightToLeft" Mask="9999999.99" MaskType="Number" 
                                     TargetControlID="txtCurValue">
                                 </cc1:MaskedEditExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <%--<tr align="center">
                                    <td>
                                        <uc3:PageNavigator ID="ucCustomPaging" runat="server" />
                                    </td>
                                </tr>--%>
                                <tr visible="false" class="styleRecordCount" runat="server" id="trNoRecord">
                                    <td style="text-align: center">
                                        <asp:Label Visible="false" ID="lblNoRecord" runat="server" Text="No Record found"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <%-- </asp:Panel>--%>
                        </td>
                    </tr>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4" align="center">
                        &nbsp;&nbsp;<asp:Button ID="btnSave" Text="Save" runat="server" CssClass="styleSubmitButton"
                            CausesValidation="true" OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CausesValidation="False"
                            OnClientClick="return fnConfirmClear();" CssClass="styleSubmitButton" OnClick="btnClear_Click" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        <input type="hidden" id="hidCustId" runat="server" />
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ShowMessageBox="false" />
                        <asp:CustomValidator ID="cvMarketValueEntry" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                        <asp:Label ID="lblCustID" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                        <%-- <asp:ValidationSummary ID="vsoption1" runat ="server" CssClass="styleMandatoryLabel" HeaderText ="Correct the following validation(s):" ShowSummary ="true" 
                             ValidationGroup ="Option1" ShowMessageBox ="false"  />--%>
                        <%-- <asp:ValidationSummary ID="vsoption2" runat="server" CssClass ="styleMandatoryLabel" HeaderText ="Correct the following validation(s):" ShowSummary ="true" 
                              ValidationGroup ="option2" ShowMessageBox ="false" />--%>
                        <tr>
                            <td>
                                <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        function fnLoadCustomer() {
            document.getElementById('<%=btnLoadCustomer.ClientID%>').click();
        }
        function checkDate_ApplicationDate(sender, args) {
            document.getElementById('ctl00_ContentPlaceHolder1_txtMVDate').value = vartodayformat;

        }




        function fnChkValidZero() {
            //get reference of GridView control
            var grid = document.getElementById("<%= grvMarket.ClientID %>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    // cell = grid.rows[i].cells[0];
                    cell = document.getElementById(txtCurValue);
                    if ((cell.value != " ") && !(isNaN(cell.value))) {
                        var CurVal
                        CurVal = parseInt(cell.value, 10);
                        if (CurVal == "0") {
                            alert("Market Value cannot be Zero");
                            cell.focus();
                        }
                        else {
                            return;
                        }
                    }

                }
            }
        }       
              

    </script>

</asp:Content>
