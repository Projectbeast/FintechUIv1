<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_SysAdminAddressNameSetup.aspx.cs"
    Inherits="System_Admin_S3G_SysAdminAddressNameSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row m-0">
        <div class="col">
            <h6 class="title_name">
                <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleInfoLabel">
                </asp:Label>
            </h6>
        </div>

        <div class="col mr-3">
            <div class="float-right">
                <button class="btn btn-outline-success btn-create" id="btnExit" title="Exit[Alt+X]" onclick="if(fnConfirmExit())" causesvalidation="false" onserverclick="btnExit_Click" runat="server"
                    type="button" accesskey="X">
                    <i class="fa fa-reply" aria-hidden="true"></i>&emsp;E<u>x</u>it
                </button>
            </div>
        </div>
    </div>

    <div id="tab-content" class="tab-content scrollable-content">
        <div class="row m-0">
            <div class="col">
                <div class="row">
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                        <div class="md-input">
                            <label class="tess">
                                <asp:Label ID="lblObjectType" CssClass="styleReqFieldLabel" runat="server" Text="Object Type"></asp:Label>
                            </label>
                            <asp:DropDownList ID="ddlObjectType" runat="server" AutoPostBack="true" ToolTip="Object Type" OnSelectedIndexChanged="ddlObjectType_SelectedIndexChanged" class="form-control form-control-sm"></asp:DropDownList>
                            <div class="validation_msg_box">
                                <asp:RequiredFieldValidator ID="rfvddlObjectType" runat="server" ControlToValidate="ddlObjectType" ErrorMessage="Select Object Type"
                                    Display="Dynamic" CssClass="validation_msg_box_sapn" SetFocusOnError="True" ValidationGroup="Save" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                            <span class="highlight"></span>
                            <span class="bar"></span>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                        <div class="md-input">
                            <label class="tess">
                                <asp:Label ID="lblEffectiveFrom" CssClass="styleReqFieldLabel" runat="server" Text="Effective From"></asp:Label>
                            </label>
                            <asp:TextBox ID="txtEffectiveFrom" runat="server" ToolTip="Effective From" AutoPostBack="true" OnTextChanged="txtEffectiveFrom_TextChanged" class="form-control form-control-sm requires_true"></asp:TextBox>
                            <cc1:CalendarExtender ID="ceFromDate" runat="server" Enabled="True"
                                TargetControlID="txtEffectiveFrom">
                            </cc1:CalendarExtender>
                            <div class="validation_msg_box">
                                <asp:RequiredFieldValidator ID="rfvtxtEffectiveFrom" runat="server" ControlToValidate="txtEffectiveFrom" ErrorMessage="Select Effective From"
                                    Display="Dynamic" CssClass="validation_msg_box_sapn" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </div>
                            <span class="highlight"></span>
                            <span class="bar"></span>
                        </div>
                    </div>

                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                        <div class="md-input">
                            <label class="tess">
                                <asp:Label ID="lblEffectiveTo" CssClass="styleReqFieldLabel" runat="server" Text="Effective To"></asp:Label>
                            </label>
                            <asp:TextBox ID="txtEffectiveTo" runat="server" ToolTip="Effective To" AutoPostBack="true" OnTextChanged="txtEffectiveTo_TextChanged"
                                class="form-control form-control-sm requires_true"></asp:TextBox>
                            <cc1:CalendarExtender ID="ceToDate" runat="server" 
                                TargetControlID="txtEffectiveTo" >
                            </cc1:CalendarExtender>
                            <div class="validation_msg_box" id="divreq" runat="server" visible="false">
                                <asp:RequiredFieldValidator ID="rfvtxtEffectiveTo" runat="server" ControlToValidate="txtEffectiveTo" ErrorMessage="Select Effective To"
                                    Display="Dynamic" CssClass="validation_msg_box_sapn" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </div>
                            <span class="highlight"></span>
                            <span class="bar"></span>
                        </div>
                    </div>
        </div>
        
        <div class="row">
            <div class="gird col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Panel runat="server" GroupingText="Address / Name Setup" ID="pnlGrid" Visible="false"
                    CssClass="stylePanel">
                    <asp:GridView ID="gvAddressNameSetup" runat="server" AutoGenerateColumns="False" class="gird_details">
                        <Columns>
                            <asp:TemplateField HeaderText="Address Setup ID" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblAddressSetupID" ToolTip='<%# Bind("ADDSETUP_ID") %>' runat="server" Text='<%# Bind("ADDSETUP_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Column Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblColumnName" ToolTip='<%# Bind("Column_Name") %>' runat="server" Text='<%# Bind("Column_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <ItemStyle Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Display Text">
                                <ItemTemplate>
                                    <div class="md-input">
                                        <asp:TextBox ID="txtDisplayText" ToolTip='<%# Bind("Display_Text") %>' runat="server"
                                            Text='<%# Bind("Display_Text") %>' MaxLength="100" CssClass="form-control form-control-sm"></asp:TextBox>
                                        <span class="highlight"></span>
                                        <span class="bar"></span>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <ItemStyle Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ToolTip">
                                <ItemTemplate>
                                    <div class="md-input">
                                        <asp:TextBox ID="txtTooltip" ToolTip='<%# Bind("Tool_tip") %>' runat="server" Text='<%# Bind("Tool_tip") %>' MaxLength="100" CssClass="form-control form-control-sm"></asp:TextBox>
                                        <span class="highlight"></span>
                                        <span class="bar"></span>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <ItemStyle Width="20%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Display">
                                <ItemTemplate>
                                    <div class="md-input">
                                        <asp:CheckBox ID="chk_IsDisplay" runat="server" ToolTip="Display"
                                            Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Display")))%>' />
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mandatory">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_IsMandatory" runat="server" ToolTip="Mandatory"
                                        Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Mandatory")))%>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Print">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_IsPrint" runat="server" ToolTip="Print"
                                        Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Print")))%>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Label Print">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_IsLablePrint" runat="server" ToolTip="Label Print"
                                        Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "IS_Label_Print")))%>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
        
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 gird">
                <asp:Panel runat="server" GroupingText="Revision History" ID="pnlRevisionHistory" Visible="false"
                    CssClass="stylePanel">
                    <asp:GridView ID="gvRevisionHistory" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="styleGridHeader" class="gird_details">
                        <Columns>
                            <asp:TemplateField HeaderText="Version ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblVersion_ID" runat="server" Text='<%# Bind("VERSION_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective From">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffective_From" runat="server" Text='<%# Bind("EFFECTIVE_FROM") %>' ToolTip="Effective From"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective To">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffective_To" runat="server" Text='<%# Bind("EFFECTIVE_TO") %>' ToolTip="Effective To"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created By">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreated_By" runat="server" Text='<%# Bind("USER_NAME") %>' ToolTip="Created By"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created On">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreated_On" runat="server" Text='<%# Bind("CREATED_ON") %>' ToolTip="Created On"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>

        <div class="p-2 fixed_btn" style="bottom: 10px;">
            <div class="col">
                <button class="btn btn-success mr-2" id="btnSave" title="Save[Alt+S]" onclick="if(fnCheckPageValidators())" causesvalidation="false" onserverclick="btnSave_Click" runat="server"
                    type="button" accesskey="S">
                    <i class="fa fa-floppy-o" aria-hidden="true"></i>&emsp;<u>S</u>ave
                </button>
                <button class="btn btn-outline-success mr-2" id="btnClear" title="Clear[Alt+L]" onclick="if(fnConfirmClear())" causesvalidation="false" onserverclick="btnClear_Click" runat="server"
                    type="button" accesskey="L">
                    <i class="fa fa-refresh" aria-hidden="true"></i>&emsp;C<u>l</u>ear
                </button>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

