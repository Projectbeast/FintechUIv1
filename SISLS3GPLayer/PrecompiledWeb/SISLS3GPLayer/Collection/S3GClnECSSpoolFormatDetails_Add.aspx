<%@ page language="C#" masterpagefile="~/Common/S3GMasterPageCollapse.master" autoeventwireup="true" inherits="Collection_S3GClnECSSpoolFormatDetails_Add, App_Web_la20gqab" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
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
                <tr>
                    <td style="padding-top: 10px">
                        <table width="100%">
                            <tr>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="35%">
                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="205px" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                        ToolTip="Line of Business">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" Display="None" InitialValue="0"
                                        ValidationGroup="submit" ErrorMessage="Select the Line of Business" ControlToValidate="ddlLOB"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel" width="15%">
                                    <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="35%">
                                    <uc:Suggest ID="ddlBranch" ToolTip="Location" runat="server" AutoPostBack="True"
                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" ServiceMethod="GetBranchList"
                                         ValidationGroup="submit" ErrorMessage="Select a Location"
                                        WatermarkText="--Select--" />
                                    <%--<asp:RequiredFieldValidator ID="rfvBranch" runat="server" Display="None" InitialValue="0"
                                        ValidationGroup="submit" ErrorMessage="Select the Location" ControlToValidate="ddlBranch"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblBank" runat="server" Text="Bank" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlBankCode" runat="server" Width="205px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlBankCode_SelectedIndexChanged" ToolTip="Bank">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvBank" runat="server" Display="None" InitialValue="0"
                                        ValidationGroup="submit" ErrorMessage="Select the Bank" ControlToValidate="ddlBankCode"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:Button ID="btnGo" runat="server" Text="Go" ToolTip="Go" ValidationGroup="submit"
                                        CssClass="styleGridShortButton" OnClick="btnGo_Click" UseSubmitBehavior="false" />
                                    <cc1:ConfirmButtonExtender ID="CBEbtnGo" runat="server" ConfirmText="Are you sure want to clear the details?"
                                        Enabled="True" TargetControlID="btnGo">
                                    </cc1:ConfirmButtonExtender>
                                </td>
                            </tr>
                        </table>
                </tr>
                <tr style="height: 10px">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Panel runat="server" ID="panECSHdr" GroupingText="ECS File Format Header" CssClass="stylePanel"
                            Width="65%">
                            <asp:GridView ID="gvECSHdr" runat="server" CssClass="styleInfoLabel" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
                                OnRowCommand="gvECSHdr_RowCommand" OnRowDataBound="gvECSHdr_RowDataBound" 
                                OnRowDeleting="gvECSHdr_RowDeleting">
                                <%--OnRowCancelingEdit="gvECS_RowCancelingEdit"
                                OnRowUpdating="gvECS_RowUpdating" OnRowEditing="gvECS_RowEditing">--%>
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblECSSpoolDetID1" runat="server" Text='<%# Eval("ECS_Spool_Detail_ID") %>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode1" runat="server" Text='<%# Eval("Lookup_Code") %>' /></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblFCode1" runat="server" Text='<%# Eval("Lookup_Code") %>' /></ItemTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header Name" HeaderStyle-CssClass="styleGridHeader"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHeader1" runat="server" Text='<%# Eval("ECS_Line_Type")%>' Style="padding-left: 10px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-left: 10px">
                                                        <asp:TextBox ID="txtFHeader1" runat="server" MaxLength="10" ToolTip="Header Name"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvHeader1" runat="server" InitialValue="0" ControlToValidate="txtFHeader1"
                                                            Display="None" ErrorMessage="Header Name can not be zero" ValidationGroup="Add"
                                                            Enabled="false">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvHeader2" runat="server" ControlToValidate="txtFHeader1"
                                                            Display="None" ErrorMessage="Enter the Header Name" ValidationGroup="Add" Enabled="false">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtHeader1" runat="server" MaxLength="10" Text='<%# Eval("ECS_Line_Type")%>'
                                                Style="padding-left: 10px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvHeader1" runat="server" InitialValue="0" ControlToValidate="txtHeader1"
                                                Display="None" ErrorMessage="Header Name can not be zero" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvHeader2" runat="server" ControlToValidate="txtHeader1"
                                                Display="None" ErrorMessage="Enter the Header Name" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Position" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFrom1" runat="server" Text='<%# Eval("ECS_From_Position")%>' Style="padding-right: 20px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFrom1" runat="server" MaxLength="3" Text='<%# Eval("ECS_From_Position")%>'
                                                Width="30px" Style="text-align: right"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFrom1" runat="server" InitialValue="0" ControlToValidate="txtFrom1"
                                                Display="None" ErrorMessage="From position can not be zero" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFrom2" runat="server" ControlToValidate="txtFrom1"
                                                Display="None" ErrorMessage="Enter the From position" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-right: 20px">
                                                        <asp:TextBox ID="txtFFrom1" runat="server" MaxLength="3" Width="30px" Style="text-align: right"
                                                            ToolTip="From Position"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="rfvFrom1" runat="server" InitialValue="0" ControlToValidate="txtFFrom"
                                                            Display="None" ErrorMessage="From position can not be zero" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvFrom2" runat="server" ControlToValidate="txtFFrom"
                                                            Display="None" ErrorMessage="Enter the From position" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>--%>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender142" runat="server" TargetControlID="txtFFrom1"
                                                            FilterType="Numbers" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Position" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTo1" runat="server" Text='<%# Eval("ECS_To_position")%>' Style="padding-right: 20px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTo1" runat="server" MaxLength="3" Text='<%# Eval("ECS_To_position")%>'
                                                Width="30px" Style="text-align: right"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTo1" runat="server" InitialValue="0" ControlToValidate="txtTo1"
                                                Display="None" ErrorMessage="To position can not be zero" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvTo2" runat="server" ControlToValidate="txtTo1"
                                                Display="None" ErrorMessage="Enter the To position" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-right: 20px">
                                                        <asp:TextBox ID="txtFTo1" runat="server" MaxLength="3" ToolTip="To Position" Width="30px"
                                                            Style="text-align: right"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="rfvTo1" runat="server" InitialValue="0" ControlToValidate="txtFTo"
                                                            Display="None" ErrorMessage="To position can not be zero" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvTo2" runat="server" ControlToValidate="txtFTo"
                                                            Display="None" ErrorMessage="Enter the To position" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>--%>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txtFTo1"
                                                            FilterType="Numbers" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Length" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLength1" runat="server" MaxLength="3" Text='<%# Eval("ECS_Length")%>'
                                                Style="padding-right: 10px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtLength1" runat="server" MaxLength="3" Text='<%# Eval("ECS_Length")%>'
                                                Width="30px" Style="text-align: right"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-right: 10px">
                                                        <asp:TextBox ID="txtFLength1" runat="server" MaxLength="3" TabIndex="-1" ToolTip="Length"
                                                            Width="30px" Style="text-align: right">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Field Description" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc1" runat="server" Text='<%# Eval("Lookup_Description") %>' Style="padding-left: 15px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlType1" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvddlType1" runat="server" InitialValue="0" ControlToValidate="ddlType1"
                                                Display="None" ErrorMessage="Select a Field Description" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvddlType2" runat="server" ControlToValidate="ddlType1"
                                                Display="None" ErrorMessage="Select a Field Description" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-left: 15px">
                                                        <asp:DropDownList ID="ddlFType1" runat="server" ToolTip="Field Type">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlType1" runat="server" InitialValue="0" ControlToValidate="ddlFType1"
                                                            Display="None" ErrorMessage="Select a Field Description" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvddlType2" runat="server" ControlToValidate="ddlFType1"
                                                            Display="None" ErrorMessage="Select a Field Description" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                ToolTip="Edit">
                                            </asp:LinkButton>
                                            &nbsp;|--%>
                                            <%--<asp:LinkButton ID="btnRemove" Text="Delete" CommandName="Delete" runat="server"
                                                CausesValidation="false" />--%>
                                            <asp:Button ID="btnRemove1" runat="server" CausesValidation="false" CommandName="Delete"
                                                CssClass="styleGridShortButton" Text="Delete" ToolTip="Delete"></asp:Button>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate1" runat="server" Text="Update" CommandName="Update"
                                                ValidationGroup="Add" ToolTip="Update" OnClientClick="return fnCheckPageValidators('Add','false');"></asp:LinkButton>
                                            &nbsp;|
                                            <asp:LinkButton ID="lnkCancel1" runat="server" Text="Cancel" CommandName="Cancel"
                                                CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnAddrow" CausesValidation="false" runat="server" CommandName="AddNew"
                                                ToolTip="Add" CssClass="styleGridShortButton" Text="Add"></asp:Button>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td>
                    </td>
                </tr>
                 <tr style="height: 10px">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Panel runat="server" ID="panECS" GroupingText="ECS File Format Details" CssClass="stylePanel"
                            Width="65%">
                            <asp:GridView ID="gvECS" runat="server" CssClass="styleInfoLabel" Width="100%" AutoGenerateColumns="False"
                                OnRowCommand="gvECS_RowCommand" OnRowDataBound="gvECS_RowDataBound" ShowFooter="true"
                                OnRowDeleting="gvECS_RowDeleting">
                                <%--OnRowCancelingEdit="gvECS_RowCancelingEdit"
                                OnRowUpdating="gvECS_RowUpdating" OnRowEditing="gvECS_RowEditing">--%>
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblECSSpoolDetID" runat="server" Text='<%# Eval("ECS_Spool_Detail_ID") %>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Lookup_Code") %>' /></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblFCode" runat="server" Text='<%# Eval("Lookup_Code") %>' /></ItemTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header Name" HeaderStyle-CssClass="styleGridHeader"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHeader" runat="server" Text='<%# Eval("ECS_Line_Type")%>' Style="padding-left: 10px"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-left: 10px">
                                                        <asp:TextBox ID="txtFHeader" runat="server" MaxLength="10" ToolTip="Header Name"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvHeader1" runat="server" InitialValue="0" ControlToValidate="txtFHeader"
                                                            Display="None" ErrorMessage="Header Name can not be zero" ValidationGroup="Add"
                                                            Enabled="false">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvHeader2" runat="server" ControlToValidate="txtFHeader"
                                                            Display="None" ErrorMessage="Enter the Header Name" ValidationGroup="Add" Enabled="false">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtHeader" runat="server" MaxLength="10" Text='<%# Eval("ECS_Line_Type")%>'
                                                Style="padding-left: 10px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvHeader1" runat="server" InitialValue="0" ControlToValidate="txtHeader"
                                                Display="None" ErrorMessage="Header Name can not be zero" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvHeader2" runat="server" ControlToValidate="txtHeader"
                                                Display="None" ErrorMessage="Enter the Header Name" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Position" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFrom" runat="server" Text='<%# Eval("ECS_From_Position")%>' Style="padding-right: 20px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFrom" runat="server" MaxLength="3" Text='<%# Eval("ECS_From_Position")%>'
                                                Width="30px" Style="text-align: right"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFrom1" runat="server" InitialValue="0" ControlToValidate="txtFrom"
                                                Display="None" ErrorMessage="From position can not be zero" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFrom2" runat="server" ControlToValidate="txtFrom"
                                                Display="None" ErrorMessage="Enter the From position" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-right: 20px">
                                                        <asp:TextBox ID="txtFFrom" runat="server" MaxLength="3" Width="30px" Style="text-align: right"
                                                            ToolTip="From Position"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="rfvFrom1" runat="server" InitialValue="0" ControlToValidate="txtFFrom"
                                                            Display="None" ErrorMessage="From position can not be zero" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvFrom2" runat="server" ControlToValidate="txtFFrom"
                                                            Display="None" ErrorMessage="Enter the From position" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>--%>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender142" runat="server" TargetControlID="txtFFrom"
                                                            FilterType="Numbers" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Position" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTo" runat="server" Text='<%# Eval("ECS_To_position")%>' Style="padding-right: 20px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTo" runat="server" MaxLength="3" Text='<%# Eval("ECS_To_position")%>'
                                                Width="30px" Style="text-align: right"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTo1" runat="server" InitialValue="0" ControlToValidate="txtTo"
                                                Display="None" ErrorMessage="To position can not be zero" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvTo2" runat="server" ControlToValidate="txtTo"
                                                Display="None" ErrorMessage="Enter the To position" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-right: 20px">
                                                        <asp:TextBox ID="txtFTo" runat="server" MaxLength="3" ToolTip="To Position" Width="30px"
                                                            Style="text-align: right"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="rfvTo1" runat="server" InitialValue="0" ControlToValidate="txtFTo"
                                                            Display="None" ErrorMessage="To position can not be zero" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvTo2" runat="server" ControlToValidate="txtFTo"
                                                            Display="None" ErrorMessage="Enter the To position" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>--%>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txtFTo"
                                                            FilterType="Numbers" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Length" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLength" runat="server" MaxLength="3" Text='<%# Eval("ECS_Length")%>'
                                                Style="padding-right: 10px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtLength" runat="server" MaxLength="3" Text='<%# Eval("ECS_Length")%>'
                                                Width="30px" Style="text-align: right"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-right: 10px">
                                                        <asp:TextBox ID="txtFLength" runat="server" MaxLength="3" TabIndex="-1" ToolTip="Length"
                                                            Width="30px" Style="text-align: right">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Field Description" HeaderStyle-CssClass="styleGridHeader">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Lookup_Description") %>' Style="padding-left: 15px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlType" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvddlType1" runat="server" InitialValue="0" ControlToValidate="ddlType"
                                                Display="None" ErrorMessage="Select a Field Description" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvddlType2" runat="server" ControlToValidate="ddlType"
                                                Display="None" ErrorMessage="Select a Field Description" ValidationGroup="Add">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <FooterTemplate>
                                            <table>
                                                <tr>
                                                    <td style="padding-left: 15px">
                                                        <asp:DropDownList ID="ddlFType" runat="server" ToolTip="Field Type">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlType1" runat="server" InitialValue="0" ControlToValidate="ddlFType"
                                                            Display="None" ErrorMessage="Select a Field Description" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="rfvddlType2" runat="server" ControlToValidate="ddlFType"
                                                            Display="None" ErrorMessage="Select a Field Description" ValidationGroup="Add">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                ToolTip="Edit">
                                            </asp:LinkButton>
                                            &nbsp;|--%>
                                            <%--<asp:LinkButton ID="btnRemove" Text="Delete" CommandName="Delete" runat="server"
                                                CausesValidation="false" />--%>
                                            <asp:Button ID="btnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                CssClass="styleGridShortButton" Text="Delete" ToolTip="Delete"></asp:Button>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                ValidationGroup="Add" ToolTip="Update" OnClientClick="return fnCheckPageValidators('Add','false');"></asp:LinkButton>
                                            &nbsp;|
                                            <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnAddrow" CausesValidation="false" runat="server" CommandName="AddNew"
                                                ToolTip="Add" CssClass="styleGridShortButton" Text="Add"></asp:Button>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            ValidationGroup="submit" OnClientClick="return fnCheckPageValidators('submit');"
                            OnClick="btnSave_Click" ToolTip="Save" />
                        <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                            CausesValidation="False" OnClick="btnClear_Click" ToolTip="Clear" />
                        <cc1:ConfirmButtonExtender ID="btnAssetClear_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure want to clear?" Enabled="True" TargetControlID="btnClear">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Cancel" OnClick="btnCancel_Click" ToolTip="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 20px">
                        <br />
                        <asp:ValidationSummary ID="vsECS" runat="server" ValidationGroup="submit" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Add"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):  " />
                        <asp:CustomValidator ID="cvECS" runat="server" Display="None" CssClass="styleMandatoryLabel"
                            ValidationGroup="submit">
                        </asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: medium"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        function FunCheckForOverlap(type, txtFrm, txtTo, txtLn, PrevTo, ddlType) {

            var From = document.getElementById(txtFrm).value;
            var To = document.getElementById(txtTo).value;
            var Length = document.getElementById(txtLn).value;
            var ddlCtrl = document.getElementById(ddlType);
            var ddlVal = ddlCtrl.options[ddlCtrl.selectedIndex].value

            if (type == 1) {
                if (From == '' || From == 0) {
                    document.getElementById(txtLn).value = '';
                    alert('Enter From Position.');
                    document.getElementById(txtFrm).focus();
                    return false;
                }
                else if (To == '' || To == 0) {
                    document.getElementById(txtLn).value = '';
                    alert('Enter To Position.');
                    document.getElementById(txtTo).focus();
                    return false;
                }
                else if (parseFloat(ddlVal) == 0) {
                    alert('Select Field Description.');
                    ddlCtrl.focus();
                    return false;
                }
                else if (parseFloat(From) > parseFloat(To)) {
                    alert('To postion getting overlap');
                    document.getElementById(txtTo).focus();
                    return false;
                }
                else {
                    document.getElementById(txtTo).value = parseFloat(To);
                    document.getElementById(txtLn).value = To - From + 1;
                }
            }
            else if (type == 2) {
                if (From == '' || From == 0) {
                    document.getElementById(txtLn).value = '';
                    alert('Enter From Position.');
                    document.getElementById(txtFrm).focus();
                    return false;
                }
                else if (To == '' || To == 0) {
                    document.getElementById(txtLn).value = '';
                    alert('Enter To Position.');
                    document.getElementById(txtTo).focus();
                    return false;
                }
                else if (parseFloat(ddlVal) == 0) {
                    alert('Select Field Type & Description.');
                    ddlCtrl.focus();
                    return false;
                }
                else if (parseInt(PrevTo) >= parseInt(From)) {
                    alert('From postion getting overlap');
                    document.getElementById(txtFrm).focus();
                    return false;
                }
                else if (parseFloat(From) > parseFloat(To)) {
                    alert('To postion getting overlap');
                    document.getElementById(txtTo).focus();
                    return false;
                }
                else {
                    document.getElementById(txtTo).value = parseFloat(To);
                    document.getElementById(txtLn).value = To - From + 1;
                }
            }
        }

    </script>
    
</asp:Content>
