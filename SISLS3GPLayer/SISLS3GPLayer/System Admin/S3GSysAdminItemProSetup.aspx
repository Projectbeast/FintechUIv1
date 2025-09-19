<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminItemProSetup.aspx.cs" Inherits="System_Admin_S3GSysAdminLookupMaster"
    Title="S3G - Lookup Master" %>

<%@ Register Src="~/UserControls/S3GAutoSuggest.ascx" TagName="AutoSugg" TagPrefix="UC5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function Program_ItemSelected(sender, e) {
            var hdnLocationID = $get('<%= hdnProgram.ClientID %>');
            hdnLocationID.value = e.get_value();
        }
        function Program_ItemPopulated(sender, e) {
            var hdnLocationID = $get('<%= hdnProgram.ClientID %>');
            hdnLocationID.value = '';
        }


    </script>


    <div class="row">
        <div class="col">
            <h6 class="title_name px-3 p-2">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                </asp:Label>
            </h6>
        </div>
        <div class="col mr-3">
            <div class="float-right">
                <button class="btn btn-outline-success btn-create" id="btnCancel" onserverclick="btnClear_Click" causesvalidation="false" runat="server" onclick="if(fnConfirmExit())"
                    type="button" accesskey="T" title="Return,Alt+T">
                    <i class="fa fa-reply"></i>&emsp;Re<u>t</u>urn
                </button>
            </div>
        </div>
    </div>
    <div id="tab-content" class="tab-content scrollable-content-details">
        <div class="tab-pane fade in active show" id="tab1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row m-0 p-0">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Panel GroupingText="Item Profile Setup" Width="100%" ID="pnlLookInfo" runat="server"
                                CssClass="stylePanel">
                                <div class="row mt-3">
                                    <div class="col-lg-4 col-md-6 col-sm-8 col-xs-12">
                                        <div class="md-input">
                                            <label class="tess">
                                                <asp:Label ID="lblProgramName" Text="Module Description" runat="server" class="styleReqFieldLabel"></asp:Label>
                                            </label>
                                            <UC5:AutoSugg ID="txtProgramName" runat="server" ToolTip="Module Description" AutoPostBack="true" ErrorMessage="Select the Module Description"
                                                class="md-form-control form-control" ServiceMethod="GetProgramName"
                                                OnItem_Selected="txtProgramName_Item_Selected" IsMandatory="false" ValidationGroup="Go" />
                                            <asp:HiddenField ID="hdnProgram" runat="server" />
                                            <div class="validation_msg_box">
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-4 col-xs-12">
                                        <div class="mt-4">
                                            <button class="btn btn-success btn-sm" id="btnGo" onserverclick="btnGoProgramName_TextChanged" validationgroup="Go" runat="server"
                                                type="button" causesvalidation="true" accesskey="G" title="Go,Alt+G">
                                                <i class="fa fa-arrow-circle-right"></i>&emsp;<u>G</u>o
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col">
                                        <asp:ValidationSummary ID="vsbtnGo" runat="server" ShowSummary="true"
                                            CssClass="styleMandatoryLabel" ValidationGroup="Go" ShowMessageBox="false"
                                            HeaderText="Correct the following validation(s):" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    
                    <div class="row m-0 p-0 mt-0">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Panel GroupingText="Item Profile Details" ID="pnlDetail" runat="server" ToolTip="Item Profile Details"
                                CssClass="stylePanel">
                                                    <div class="grid">
                                                        <asp:GridView ID="grvItmProfSetUp" runat="server" AutoGenerateColumns="false" Width="100%" ToolTip="Item Profile Details"
                                                            OnRowDataBound="grvItmProfSetUp_RowDataBound" OnRowUpdating="grvItmProfSetUp_RowUpdating" class="gird_details">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNO" runat="server" Text='<%#Bind("S_NO") %>' ToolTip='<%#Bind("S_NO") %>'></asp:Label>
                                                                        <asp:Label ID="lblPAGESETUPID" Visible="false" runat="server" Text='<%#Bind("PAGESETUP_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Display Name">
                                                                    <ItemTemplate>
                                                                        <div class="md-input" style="margin: 0px;">
                                                                            <asp:TextBox ID="txtDISPLAYTEXT" ToolTip='<%#Bind("Display_Name") %>' class="md-form-control form-control login_form_content_input requires_true" runat="server" Text='<%#Bind("Display_Name") %>'></asp:TextBox>
                                                                            <span class="highlight"></span>
                                                                            <span class="bar"></span>
                                                                            <div class="validation_msg_box">
                                                                                <asp:RequiredFieldValidator ID="rfvtxtDISPLAYTEXT" runat="server" ControlToValidate="txtDISPLAYTEXT"
                                                                                    CssClass="validation_msg_box_sapn" Display="Dynamic" ValidationGroup="btnUpdate" SetFocusOnError="True"
                                                                                    ErrorMessage="Enter the Display Name"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tool Tip">
                                                                    <ItemTemplate>
                                                                        <div class="md-input" style="margin: 0px;">
                                                                            <asp:TextBox ID="txtTOOLTIP" runat="server" ToolTip='<%#Bind("TOOL_TIP") %>' Text='<%#Bind("TOOL_TIP") %>' class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                                                            <span class="highlight"></span>
                                                                            <span class="bar"></span>
                                                                            <div class="validation_msg_box">
                                                                                <asp:RequiredFieldValidator ID="rfvtxtTOOLTIP" runat="server" ControlToValidate="txtTOOLTIP"
                                                                                    CssClass="validation_msg_box_sapn" Display="Dynamic" ValidationGroup="btnUpdate" SetFocusOnError="True"
                                                                                    ErrorMessage="Enter the Tool Tip"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Type" Visible="false">
                                                                    <ItemTemplate>
                                                                        <div class="md-input" style="margin: 0px; display: none;">
                                                                            <asp:DropDownList ID="ddlDATATYPE" runat="server" ToolTip="Data Type" CssClass="md-form-control form-control" Width="100px"></asp:DropDownList>
                                                                            <asp:Label ID="lblDATATYPE" Text='<%#Bind("Data_Type") %>' Visible="false" runat="server"></asp:Label>
                                                                            <span class="highlight"></span>
                                                                            <span class="bar"></span>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Column Width" Visible="false">
                                                                    <ItemTemplate>
                                                                        <div class="md-input" style="margin: 0px; display: none;">
                                                                            <asp:TextBox ID="txtCOLUMNWIDTH" runat="server" ToolTip="Column Width" Style="text-align: right" Text='<%#Bind("COLUMN_WIDTH") %>' Width="100px" class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                                                            <span class="highlight"></span>
                                                                            <span class="bar"></span>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Column Decimal" Visible="false">
                                                                    <ItemTemplate>
                                                                        <div class="md-input" style="margin: 0px; display: none;">
                                                                            <asp:TextBox ID="txtCOLUMNDECIMAL" runat="server" ToolTip="Column Decimal" Style="text-align: right" Text='<%#Bind("COLUMN_DECIMAL") %>' Width="100px" class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                                                            <span class="highlight"></span>
                                                                            <span class="bar"></span>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Effective From">
                                                                    <ItemTemplate>
                                                                        <div class="md-input" style="margin: 0px;">
                                                                            <asp:TextBox ID="txtEFFECTIVEFROM" Width="100px" runat="server" Text='<%#Bind("EFFECTIVE_FROM") %>' ToolTip="Effective From" class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="calEFFECTIVEFROM" runat="server" Enabled="True" TargetControlID="txtEFFECTIVEFROM">
                                                                            </cc1:CalendarExtender>
                                                                            <span class="highlight"></span>
                                                                            <span class="bar"></span>
                                                                            <div class="validation_msg_box">
                                                                                <asp:RequiredFieldValidator ID="rfvtxtEFFECTIVEFROM" runat="server" ControlToValidate="txtEFFECTIVEFROM"
                                                                                    CssClass="validation_msg_box_sapn" Display="Dynamic" ValidationGroup="btnUpdate" SetFocusOnError="True"
                                                                                    ErrorMessage="Enter the Effective From"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Effective To">
                                                                    <ItemTemplate>
                                                                        <div class="md-input" style="margin: 0px;">
                                                                            <asp:TextBox ID="txtEFFECTIVETO" Width="100px" runat="server" Text='<%#Bind("EFFECTIVE_TO") %>' OnTextChanged="txtEFFECTIVETO_TextChanged" AutoPostBack="true" ToolTip="Effective To" class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="calEFFECTIVETo" runat="server" Enabled="True" TargetControlID="txtEFFECTIVETO"
                                                                                OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                            </cc1:CalendarExtender>
                                                                            <span class="highlight"></span>
                                                                            <span class="bar"></span>
                                                                            <div class="validation_msg_box">
                                                                                <asp:RequiredFieldValidator ID="rfvtxtEFFECTIVETO" runat="server" ControlToValidate="txtEFFECTIVETO"
                                                                                    CssClass="validation_msg_box_sapn" Display="Dynamic" ValidationGroup="btnUpdate" SetFocusOnError="True"
                                                                                    ErrorMessage="Enter the Effective To"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Update">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" ValidationGroup="btnUpdate" OnClientClick="return confirm('Do you want to Update?');" CommandName="Update" ToolTip="Update,Alt+U" AccessKey="U" OnClick="btnUpdate_Click"
                                                                            CssClass="grid_btn" CausesValidation="true"></asp:Button>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
    
    <div class="p-2 pb-5 mb-4">
        <asp:ValidationSummary ID="vsFunderTransaction" runat="server" ShowSummary="true"
            CssClass="styleMandatoryLabel" ValidationGroup="Main" ShowMessageBox="false"
            HeaderText="Correct the following validation(s):" />
        <asp:ValidationSummary runat="server" ID="vsCurrency" ValidationGroup="Add"
            HeaderText="Please correct the following validation(s):" ShowSummary="true"
            CssClass="styleMandatoryLabel" ShowMessageBox="false" />
        <asp:ValidationSummary runat="server" ID="vsUpdate" ValidationGroup="Update"
            HeaderText="Please correct the following validation(s):" ShowSummary="true"
            CssClass="styleMandatoryLabel" ShowMessageBox="false" />
        <asp:ValidationSummary runat="server" ID="ValidationSummary1" ValidationGroup="btnUpdate"
            HeaderText="Please correct the following validation(s):" ShowSummary="true"
            CssClass="styleMandatoryLabel" ShowMessageBox="false" />
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
    </div>
    
    <%--<div class="fixed_btn" style="bottom: 9px;">
        <div class="col p-0">
            <button class="btn btn-success mr-2" id="btnSave" onserverclick="btnSave_Click" runat="server" onclick="if(fnCheckPageValidators('Save'))"
                type="button" accesskey="S" causesvalidation="false" title="Save,Alt+S" validationgroup="Main">
                <i class="fa fa-floppy-o"></i>&emsp;<u>S</u>ave
            </button>
            <button class="btn btn-outline-success" id="btnClear" onserverclick="btnClear_Click" causesvalidation="false" runat="server" onclick="if(fnConfirmClear())"
                type="button" accesskey="R" title="Clear,Alt+R">
                <i class="fa fa-refresh"></i>&emsp;<u>R</u>eset
            </button>
        </div>
    </div>--%>
</asp:Content>
