﻿<%@ page title="PDC Bulk Replacement" language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" enableeventvalidation="false" autoeventwireup="true" inherits="Collection_S3GClnPDCBulkReplacement, App_Web_wom33hv0" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function Calculate(amt, other, total, suffixlen) {

            if (other.value.length != 0) {
                total.value = parseFloat(parseFloat(amt.value) + parseFloat(other.value)).toFixed(suffixlen);
                other.value = parseFloat(other.value).toFixed(suffixlen);
                total.setAttribute('title', total.value);
                other.setAttribute('title', other.value);

            }
            else if (other.value.length == 0) {
                other.value = parseFloat("0").toFixed(suffixlen);
                other.setAttribute('title', other.value);
            }
        }

        function fnLoadCustomer() {
            document.getElementById('<%=btnLoadCustomer.ClientID%>').click();
        }        
    </script>

    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table width="100%" align="center" cellpadding="0" cellspacing="0" bordre="1">
                    <tr>
                        <td class="stylePageHeading">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">  </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" colspan="4">
                            <%-- <asp:UpdatePanel ID="upanelPDCEntry111" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <cc1:TabContainer ID="tcPDCEntry" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                        Width="99%" ScrollBars="None">
                                        <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                                            BackColor="Red" ToolTip="General" Width="99%">
                                            <HeaderTemplate>
                                                General</HeaderTemplate>
                                            <ContentTemplate>--%>
                            <asp:UpdatePanel ID="UpGeneral" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlGeneral" runat="server" CssClass="stylePanel" GroupingText="General"
                                                    Width="99%">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="styleFieldLabel" width="21%">
                                                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                    ToolTip="Line of Business">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RFVLOB" CssClass="styleMandatoryLabel" runat="server"
                                                                    ControlToValidate="ddlLOB" InitialValue="0" ValidationGroup="VGPDC" Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="PDC" Visible="false" ID="lblPDCEntryNo" CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox runat="server" ID="txtPDCEntryNo" ReadOnly="True" Visible="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                    ToolTip="Branch">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RFVBranch" CssClass="styleMandatoryLabel" runat="server"
                                                                    ControlToValidate="ddlBranch" InitialValue="0" ValidationGroup="VGPDC" Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <%--Changed By Thangam on 14/May/2012 for UAT bug - PDCE_005--%>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <%-- END Here--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCustomerCode" runat="server" Text="Customer Code" CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc2:LOV ID="ucCustomerCodeLov" onblur="fnLoadCustomer()" runat="server" DispalyContent="Code"
                                                                    strLOV_Code="CMB" />
                                                                <asp:RequiredFieldValidator ID="rfvCustomer" CssClass="styleMandatoryLabel" runat="server"
                                                                    ValidationGroup="VGPDC" ControlToValidate="txtCustomerName" Display="None" ErrorMessage="Select the Customer">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:HiddenField ID="hidcuscode" runat="server" />
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Prime Account Number" ID="lblprimeaccno" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlPAN" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPAN_SelectedIndexChanged"
                                                                    ToolTip="Prime A/c Number">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RFVMLA" CssClass="styleMandatoryLabel" runat="server"
                                                                    ControlToValidate="ddlPAN" InitialValue="0" ValidationGroup="VGPDC" Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                &nbsp;
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Sub Account Number" ID="lblsubAccno" CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlSAN" runat="server" AutoPostBack="true" ToolTip="Sub A/c Number"
                                                                    OnSelectedIndexChanged="ddlSAN_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RFVSLA" CssClass="styleMandatoryLabel" runat="server"
                                                                    ControlToValidate="ddlSAN" InitialValue="0" ValidationGroup="VGPDC" Display="None"
                                                                    Visible="False"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitShortButton" OnClick="btnGo_Click"
                                                                    Text="Go" ToolTip="Go" ValidationGroup="VGPDC" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlCommAddress" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                                                    Width="99%" Height="100%">
                                                    <table width="100%" cellspacing="0">
                                                        <tr>
                                                            <td class="styleFieldLabel" width="21%">
                                                                <asp:Label runat="server" Text="Customer Name" ID="Label1" CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox runat="server" ID="txtCustomerName" ToolTip="Customer Name"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <uc1:S3GCustomerAddress ID="CustomerDetails1" runat="server" ActiveViewIndex="1"
                                                                    ShowCustomerCode="false" ShowCustomerName="false" FirstColumnWidth="21%" ThirdColumnWidth="21%"
                                                                    FourthColumnWidth="30%" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <asp:UpdatePanel ID="UpPDCDetails" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlGRDSeqYes" runat="server" CssClass="stylePanel" GroupingText="PDC Bulk Replace Information"
                                                            Visible="false" Width="100%">
                                                            <%-- <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>--%>
                                                            <%--<div id="autoScroll" style="height: 280px; overflow: scroll; direction: inherit;"
                                                                            runat="server">--%>
                                                            <asp:GridView runat="server" ID="GRVPDCDetails" AutoGenerateColumns="False" ShowFooter="true"
                                                                Width="100%" OnRowDataBound="GRVPDCDetails_RowDataBound" OnRowCreated="GRVPDCDetails_RowCreated">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select All" HeaderStyle-Width="7%" HeaderStyle-Wrap="false">
                                                                       <HeaderTemplate>
                                                                            <table>
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        Select All
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged"
                                                                                            ToolTip="Include" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkExclude" runat="server" AutoPostBack="true" CausesValidation="false"
                                                                                CssClass="styleGridHeader" OnCheckedChanged="chkExclude_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PDC_Details_ID" HeaderStyle-Width="5%" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPDC_Details_ID" runat="server" Text='<%#Eval("PDC_Details_ID")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="" Visible="false" />
                                                                    <asp:TemplateField HeaderText="Prime Account Number" HeaderStyle-Width="8%" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPANUM" runat="server" Text='<%#Eval("PANUM")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Sub Account Number" HeaderStyle-Width="8%" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSANUM" runat="server" Text='<%#Eval("SANUM")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Installment Number" HeaderStyle-Width="6%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInstallmentNo" runat="server" Text='<%#Eval("InstallmentNo")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Installment Date" HeaderStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInstallmentDate" runat="server" Text='<%#Eval("InstallmentDate")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Drawee Bank" HeaderStyle-Width="8%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDraweeBankG" runat="server" Text='<%#Eval("DraweeBank")%>'></asp:Label>
                                                                            <%-- <asp:DropDownList ID="ddlDraweeBankG" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlDraweeBankG_SelectedIndexChanged"
                                                                                                Style="border-color: White;" Visible="false">
                                                                                            </asp:DropDownList>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Instrument Number" HeaderStyle-Width="7%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInstrumentno" runat="server" Visible="false" Text='<%#Eval("InstrumentNo")%>'></asp:Label>
                                                                            <asp:TextBox ID="txtInstrumentNo" runat="server" MaxLength="6" Text='<%# Bind("InstrumentNo")%>'
                                                                                Width="96%" ReadOnly="true" Style="border-color: White;"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FTEInstrumentNo" runat="server" TargetControlID="txtInstrumentNo"
                                                                                FilterType="Numbers" Enabled="true">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="New Instrument Number" HeaderStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtInstrumentNoNew" runat="server" MaxLength="6" Text='<%# Bind("NewInstrumentNo")%>'
                                                                                Width="96%" Style="border-color: White;"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FTEInstrumentNoNew" runat="server" TargetControlID="txtInstrumentNoNew"
                                                                                FilterType="Numbers" Enabled="true">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Instrument Date" HeaderStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInstrumentDate" Visible="false" runat="server" Text='<%#Eval("InstrumentDate")%>'></asp:Label>
                                                                            <asp:TextBox ID="txtInstrumentDate" runat="server" Text='<%# Bind("InstrumentDate")%>'
                                                                                Width="96%" ContentEditable="False" Style="border-color: White;"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="New Instrument Date" HeaderStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtInstrumentDateNew" runat="server" Text='<%# Bind("NewInstrumentDate")%>'
                                                                                Width="96%" ContentEditable="True"></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CECPreInstrumentDate1" runat="server" Format="dd/MM/yyyy"
                                                                                Enabled="True" TargetControlID="txtInstrumentDateNew" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                            </cc1:CalendarExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>

                                                                       <asp:TemplateField HeaderText="MICR" HeaderStyle-Width="7%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMICR" runat="server" Visible="false" Text='<%#Eval("MICR")%>'></asp:Label>
                                                                            <asp:TextBox ID="txtMICR" runat="server" MaxLength="11" Text='<%# Bind("MICR")%>'
                                                                                Width="96%" ReadOnly="true" Style="border-color: White;"></asp:TextBox>
                                                                         
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="New MICR" HeaderStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtMICRNew" runat="server" MaxLength="11" Text='<%# Bind("NEWMICR")%>'
                                                                                Width="96%" Style="border-color: White;"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="10%" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtAmount" runat="server" MaxLength="20" Width="96%" Text='<%# Bind("Amount")%>'
                                                                                ReadOnly="true" Style="border-color: White;"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FTEAmount" runat="server" TargetControlID="txtAmount"
                                                                                FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="6%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblStatus1" runat="server" Text='<%#Eval("Status1")%>' Visible="true"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Insurance" HeaderStyle-Width="7%" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtInsurance" runat="server" MaxLength="14" Text='<%# Eval("Insurance")%>'
                                                                                Width="94%" ReadOnly="true" Style="border-color: White;"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FTEInsurance" runat="server" TargetControlID="txtInsurance"
                                                                                FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tax" HeaderStyle-Width="7%" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtTax" runat="server" MaxLength="14" Text='<%# Eval("Tax")%>' Width="94%"
                                                                                ReadOnly="true" Style="border-color: White;"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FTETax" runat="server" TargetControlID="txtTax"
                                                                                FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Other Charges" HeaderStyle-Width="10%" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtothercharges" runat="server" MaxLength="14" Text='<%# Eval("othercharges")%>'
                                                                                Width="96%" ReadOnly="true" Style="border-color: White;"></asp:TextBox>
                                                                            <%--<asp:Label ID="lblothercharges" runat="server" Text='<%#Eval("othercharges")%>'></asp:Label>--%>
                                                                            <cc1:FilteredTextBoxExtender ID="FTEOtherchanrges" runat="server" TargetControlID="txtothercharges"
                                                                                FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%-- Total--%>
                                                                    <asp:TemplateField HeaderText="Instrument Amount" HeaderStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txttotal" runat="server" MaxLength="14" ContentEditable="false"
                                                                                Width="96%" ReadOnly="true" Style="border-color: White;"></asp:TextBox>
                                                                            <%-- <asp:Label ID="lblTotal" runat="server" ContentEditable="true" ></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                            </asp:GridView>
                                                            <%-- <uc1:PageNavigator ID="ucCustomPaging" runat="server" />--%>
                                                            <%--</div>--%>
                                                            <%-- </td>
                                                                </tr>
                                                            </table>--%></asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%-- </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" align="center" cellpadding="0" cellspacing="0">
                    <tr class="styleButtonArea" align="center" style="padding-left: 4px">
                        <td>
                            <asp:UpdatePanel ID="UpButtons" runat="server">
                                <ContentTemplate>
                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators('VGPDC');"
                                        CssClass="styleSubmitButton" Text="Save" ValidationGroup="VGPDC" OnClick="btnSave_Click"
                                        ToolTip="Save" />
                                    <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                                        ToolTip="Clear" />
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="VGPDC" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                                    <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer"
                                        OnClick="btnLoadCustomer_OnClick" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr class="styleButtonArea">
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:CustomValidator ID="CVPDCEntry" runat="server" CssClass="styleMandatoryLabel"
                                        Enabled="true" Width="98%" />
                                    <asp:ValidationSummary runat="server" ID="vsPDCEntry" HeaderText="Correct the following validation(s):"
                                        CssClass="styleMandatoryLabel" ShowMessageBox="false" ShowSummary="true" ValidationGroup="VGPDC" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr class="styleButtonArea">
                        <td>
                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
