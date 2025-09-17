<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GSysAdminRoleMst_Add.aspx.cs" 
    Inherits="System_Admin_S3GSysAdminRoleMst_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row m-0">
                <div class="col">
                    <h6 class="title_name">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                        </asp:Label>
                    </h6>
                </div>
                <div class="col mr-3">
                    <div class="float-right">
                        <button class="btn btn-outline-success btn-create" id="btnCancel" title="Exit[Alt+X]" onclick="if(fnConfirmExit())" causesvalidation="false" onserverclick="btnCancel_Click" runat="server"
                            type="button" accesskey="X">
                            <i class="fa fa-reply" aria-hidden="true"></i>&emsp;E<u>x</u>it
                        </button>
                    </div>
                </div>
            </div>
            
            <div id="tab-content" class="tab-content scrollable-content">
                <div class="row m-0">
                    <div class="col">
                        <!-- User Group Details Section -->
                        <div class="card mb-3 p-3 shadow-sm" style="border-radius: 10px; background: #f8f9fa;">
                            <h5 class="mb-3" style="font-weight: 600; color: #2c3e50;">User Group Details</h5>
                            <div class="row">
                                <div id="divGrpCode" runat="server" class="col-lg-3 col-md-4 col-sm-6 col-xs-12" visible="false">
                                    <div class="md-input">
                                        <label>
                                            <asp:Label runat="server" Visible="false" Text="User Group Code" ID="lblLOBCode" CssClass="styleReqFieldLabel"></asp:Label>
                                        </label>
                                        <asp:TextBox ID="txtUserGroupCode" Visible="false" runat="server" MaxLength="2" onKeyPress="fnChangeLower_Upper(this);" class="md-form-control form-control login_form_content_input requires_true" placeholder="Enter code" ToolTip="Enter User Group Code (2 chars)"></asp:TextBox>
                                        <span class="highlight"></span>
                                        <span class="bar"></span>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                                    <div class="md-input">
                                        <label class="tess">
                                            <asp:Label runat="server" Text="User Group Name" ID="lblUserGroupName" CssClass="styleReqFieldLabel"></asp:Label>
                                        </label>
                                        <asp:TextBox ID="txtUserGroupName" runat="server" MaxLength="30" onKeyPress="fnChangeLower_Upper(this);" class="md-form-control form-control login_form_content_input requires_true" placeholder="Enter group name" ToolTip="Enter User Group Name"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="fteUserGroupName" runat="server" TargetControlID="txtUserGroupName" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" " Enabled="True"></cc1:FilteredTextBoxExtender>
                                        <span class="highlight"></span>
                                        <span class="bar"></span>
                                        <div class="validation_msg_box">
                                            <asp:RequiredFieldValidator ID="rfvUserGroupName" ValidationGroup="btnSave" runat="server" ErrorMessage="Enter the User Group Name" ControlToValidate="txtUserGroupName" SetFocusOnError="true" CssClass="validation_msg_box_sapn" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12">
                                    <div class="md-input">
                                        <label>
                                            <asp:Label runat="server" Text="Group Email" ID="lblGroupEmail" CssClass="styleFieldLabel"></asp:Label>
                                        </label>
                                        <asp:TextBox ID="txtGroupEmail" runat="server" MaxLength="45" class="md-form-control form-control login_form_content_input requires_true" placeholder="Enter group email" ToolTip="Enter Group Email"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="fteGroupEmail" runat="server" TargetControlID="txtGroupEmail" FilterType="Numbers, UppercaseLetters, LowercaseLetters,Custom" ValidChars="._-@" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        <span class="highlight"></span>
                                        <span class="bar"></span>
                                        <div class="validation_msg_box">
                                            <asp:RegularExpressionValidator ID="revGroupEmail" runat="server" ValidationGroup="btnSave" ControlToValidate="txtGroupEmail" CssClass="validation_msg_box_sapn" Display="Dynamic" SetFocusOnError="True" ErrorMessage="Enter a valid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-4 col-sm-6 col-xs-12 d-flex align-items-center justify-content-start mt-4">
                                    <asp:CheckBox ID="chkActive" runat="server" CssClass="user-management-check mr-2" ToolTip="Is Active?" />
                                    <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleDisplayLabel"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <!-- LOB List Section -->
                        <div class="card mb-3 p-3 shadow-sm" style="border-radius: 10px; background: #f8f9fa;">
                            <h5 class="mb-3" style="font-weight: 600; color: #2c3e50;">Line Of Business</h5>
                            <div class="row">
                                <div class="col-12">
                                    <asp:Panel ID="pnlLobList" runat="server" CssClass="stylePanel" GroupingText="Line Of Business" Width="100%">
                                        <div style="overflow: auto; height: 120px; background: #fff; border-radius: 6px; border: 1px solid #e0e0e0;">
                                            <asp:GridView ID="GrvLOBList" runat="server" OnRowDataBound="GrvLOBList_RowDataBound" AutoGenerateColumns="False" Width="100%" class="gird_details">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Line Of Business" HeaderStyle-CssClass="col-10">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLOB_Name" runat="server" Text='<%#Eval("LOB_NAME")%>'></asp:Label>
                                                            <asp:Label ID="lblLOB_ID" runat="server" Visible="false" Text='<%#Eval("LOB_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="col text-center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkLOB" runat="server" CssClass="checkbox-align" />
                                                            <asp:Label ID="lblAssigned" runat="server" Visible="false" Text='<%#Eval("Assigned")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <asp:Panel ID="PnlRoleCode" runat="server" CssClass="stylePanel" GroupingText="Role code" Width="100%">
                                        <div style="overflow: auto; height: 300px">
                                            <asp:GridView ID="grvRoleCode" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvRoleCode_RowDataBound" class="gird_details">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Right" DataField="SINO" HeaderText="S.No" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Role Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRoleCode" runat="server" Text='<%#Eval("Role_Code")%>'></asp:Label>
                                                            <asp:Label
                                                                ID="lblRoleCenterID" runat="server" Visible="false" Text='<%#Eval("Role_Code_ID")%>'></asp:Label>
                                                            <asp:Label ID="lblAssigned" runat="server" Visible="false" Text='<%#Eval("Assigned")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="ProgramDesc" HeaderText="Program Description" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Access">
                                                        <HeaderTemplate>
                                                            <table>
                                                                <thead>
                                                                    <tr>
                                                                        <td colspan="4" align="center">
                                                                            Access
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">Add</td>
                                                                        <td align="center">Modify</td>
                                                                        <td align="center">Delete</td>
                                                                        <td align="center">View</td>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table>
                                                                <thead>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:CheckBox ID="chkAdd" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAdd")) %>' ToolTip="Add" /></td>
                                                                        <td align="center">
                                                                            <asp:CheckBox ID="chkModify" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsModify")) %>' ToolTip="Modify" /></td>
                                                                        <td align="center">
                                                                            <asp:CheckBox ID="chkDelete" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsDelete")) %>' ToolTip="Delete" /></td>
                                                                        <td align="center">
                                                                            <asp:CheckBox ID="chkView" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsView")) %>' ToolTip="View" /></td>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <HeaderTemplate>
                                                            <table>
                                                                <thead>
                                                                    <tr>
                                                                        <td>Select All
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="center">
                                                                        <td colspan="4">
                                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelRoleCode" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelRoleCode_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel" style="display: none;">
                                <div class="md-input">
                                    <asp:ValidationSummary CssClass="styleSummaryStyle" ID="vsUserGroup" runat="server" ShowMessageBox="false"
                                        ShowSummary="true" HeaderText="Please correct the following validation(s):  " />
                                </div>
                            </div>
                        </div>
                        <div class="row" style="display: none;">
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <asp:Label ID="Label1" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                    <asp:CustomValidator ID="CVUsermanagement" runat="server" Display="None" ValidationGroup="save"></asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                        
                        <div class="p-2 fixed_btn" style="bottom: 10px;">
                            <div class="col">
                                <button class="btn btn-success mr-2" id="btnSave" title="Save[Alt+S]" onclick="if(fnCheckPageValidators())" validationgroup="btnSave" causesvalidation="false" onserverclick="btnSave_Click" runat="server"
                                    type="button" accesskey="S">
                                    <i class="fa fa-floppy-o" aria-hidden="true"></i>&emsp;<u>S</u>ave
                                </button>
                                <button class="btn btn-outline-success mr-2" id="btnClear" title="Clear[Alt+L]" onclick="if(fnConfirmClear())" causesvalidation="false" onserverclick="btnClear_Click" runat="server"
                                    type="button" accesskey="L">
                                    <i class="fa fa-refresh" aria-hidden="true"></i>&emsp;C<u>l</u>ear
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

