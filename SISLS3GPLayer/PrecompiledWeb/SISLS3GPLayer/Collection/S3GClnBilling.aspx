﻿<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="Collection_S3GClnBilling, App_Web_f2u5fcxj" title="Bill Generation" %>

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
                                    <asp:Label runat="server" Text="Bill Generation" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcBilling" runat="server" CssClass="styleTabPanel" ScrollBars="None"
                            Width="98%">
                            <cc1:TabPanel ID="TabMainPage" runat="server" BackColor="Red" CssClass="tabpan" HeaderText="Process"
                                Width="98%">
                                <HeaderTemplate>
                                    Process
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td width="60%" valign="top">
                                                <asp:Panel ID="Panel1" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                                                    Width="99%">
                                                    <table width="100%" border="0">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlLOB" runat="server" onchange="fnAssignLOB(this)">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                    Display="None" InitialValue="0" Enabled="true" ErrorMessage="Select a Line of Business"
                                                                    ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblFrequency" runat="server" CssClass="styleReqFieldLabel" Text="Frequency"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlFrequency" runat="server" onchange="fnAssignFrequency(this)">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ControlToValidate="ddlFrequency"
                                                                    Display="None" InitialValue="0" Enabled="true" ErrorMessage="Select a Frequency"
                                                                    ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblMonthYear" runat="server" CssClass="styleReqFieldLabel" Text="Month/Year"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtMonthYear" runat="server" ReadOnly="True"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calMonthYear" Format="MMM-yyyy" TodaysDateFormat="MMM-yyyy"
                                                                    OnClientShown="onShown" OnClientHidden="onHidden" runat="server" DefaultView="Months"
                                                                    Enabled="True" TargetControlID="txtMonthYear" PopupButtonID="imgMonthYear">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgMonthYear" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblStartDate" runat="server" CssClass="styleReqFieldLabel" Text="Start Date"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calStartDate" runat="server" Enabled="True" TargetControlID="txtStartDate"
                                                                    PopupButtonID="imgStartDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                                                                    Display="None"  Enabled="true" ErrorMessage="Select a Start Date"
                                                                    ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblEndDate" runat="server" CssClass="styleReqFieldLabel" Text="End Date"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" colspan="2">
                                                                <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calEndDate" runat="server" Enabled="True" TargetControlID="txtEndDate"
                                                                    PopupButtonID="imgEndDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                                                                    Display="None"  Enabled="true" ErrorMessage="Select a End Date"
                                                                    ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td width="40%" valign="top">
                                                <asp:Panel ID="panSchedule" runat="server" GroupingText="Schedule Details" CssClass="stylePanel"
                                                    Width="99%">
                                                    <table cellpadding="1" cellspacing="1" border="0" width="100%">
                                                        <tr>
                                                            <td width="45%" valign="top">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel" colspan="2">
                                                                            <asp:RadioButtonList ID="rbtnSchedule" runat="server" AutoPostBack="true" AppendDataBoundItems="true" RepeatDirection="Horizontal"
                                                                                OnSelectedIndexChanged="rbtnSchedule_SelectedIndexChanged">
                                                                                <asp:ListItem Text="Schedule Now" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Selected="True" Text="Schedule At :" Value="0"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblScheduleDate" runat="server" CssClass="styleReqFieldLabel" Text="Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtScheduleDate" runat="server" ReadOnly="True" Width="50%"></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="calScheduleDate" runat="server" Enabled="True" TargetControlID="txtScheduleDate"
                                                                                PopupButtonID="imgScheduleDate">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:Image ID="imgScheduleDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblScheduleTime" runat="server" CssClass="styleReqFieldLabel" Text="Time"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtScheduleTime" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldAlign" colspan="1">
                                                                            <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitShortButton" OnClick="btnGo_Click"
                                                                                Text="Go" CausesValidation="true" ValidationGroup="Go" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel runat="server" ID="pnlBranch" CssClass="stylePanel" GroupingText="Branch Details"
                                        Width="99%" Visible="false">
                                        <br />
                                        <div id="div1" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server"
                                            border="1">
                                            <asp:GridView ID="gvBranchWise" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                                EmptyDataText="No Branch Found for Billing!...." Width="100%" OnRowDataBound="gvBranchWise_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAllBranch" runat="server" onclick="javascript:fnSelectAll(this,'chkSelectBranch');" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectBranch" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Branch Id" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBranchId" runat="server" Text='<%#Bind("Branch_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Branch_Name" HeaderText="Branch" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="AccountCount" HeaderText="Number of Accounts" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="DebitAmount" HeaderText="Debit Amount" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="JVReference" HeaderText="System JV Reference" />
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Bind("Remarks") %>' MaxLength="100"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                        <br />
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabControlDataSheet" runat="server" BackColor="Red" CssClass="tabpan"
                                HeaderText="Process" Width="98%">
                                <HeaderTemplate>
                                    Control Data Sheet
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table align="center" width="85%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblDataMonthYear" runat="server" CssClass="styleReqdFieldLabel" Text="Month/Year"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtDataMonthYear" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="Label3" runat="server" CssClass="styleReqdFieldLabel" Text="Frequency"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="1">
                                                <asp:TextBox ID="txtDataFrequency" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabBillOutput" runat="server" BackColor="Red" CssClass="tabpan"
                                HeaderText="Process" Width="98%">
                                <HeaderTemplate>
                                    Bill Generation Output
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="85%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblBillLOB" runat="server" CssClass="styleReqdFieldLabel" Text="Line of Business"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtBillLOB" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblBillMonthYear" runat="server" CssClass="styleReqdFieldLabel" Text="Month/Year"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtBillMonthYear" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblBillFrequency" runat="server" CssClass="styleReqdFieldLabel" Text="Frequency"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="3">
                                                <asp:TextBox ID="txtBillFrequency" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel Width="98%" HorizontalAlign="Center" GroupingText="Branch" runat="server"
                                        ID="pnlBranches" CssClass="stylePanel">
                                        <table width="100%" align="center" border="0">
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList CssClass="styleDisplayLabel" ID="chklstBranches" runat="server"
                                                        TextAlign="Right" RepeatDirection="Horizontal" Width="100%">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <table width="65%" align="center">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Button ID="btnBillPDF" runat="server" CssClass="styleSubmitButton" Text="Billing By PDF"
                                                    Enabled="false" />&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnBillStatement" runat="server" CssClass="styleSubmitButton" Text="Billling By Statement"
                                                    Width="175px" Enabled="false" />&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnBillEMail" runat="server" CssClass="styleSubmitButton" Text="Billing By EMail"
                                                    Enabled="false" />&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" Enabled="false" CausesValidation="true" CssClass="styleSubmitButton"
                            Text="Save" ValidationGroup="btnSave" OnClick="btnSave_OnClick" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Clear" OnClick="btnClear_OnClick" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Cancel" OnClick="btnCancel_Click" />
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                        <asp:CustomValidator ID="cvBilling" runat="server" CssClass="styleMandatoryLabel"
                            Display="None" ValidationGroup="Submit"></asp:CustomValidator>
                        <asp:ValidationSummary ID="vsBilling" runat="server" CssClass="styleMandatoryLabel"
                            ValidationGroup="Go" HeaderText="Please correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" src="../Scripts/jsBilling.js" type="text/javascript">
    </script>

</asp:Content>
