<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GOrgCashflowMaster_Add.aspx.cs" Inherits="Origination_S3GOrgCashflowMaster_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <div>
                <div class="row m-0">
                    <div class="col">
                        <h6 class="title_name px-3 p-2">
                            <asp:Label runat="server" Text="Asset Master" ID="lblHeading" CssClass="styleInfoLabel" ToolTip="Asset Master"> </asp:Label>
                        </h6>
                    </div>
                    <div class="col mr-3">
                        <div class="float-right">
                            <button class="btn btn-outline-success btn-create" id="btnCancel" title="Exit[Alt+X]" onclick="if(fnConfirmExit())" causesvalidation="false" onserverclick="btnCancel_Click" runat="server"
                                type="button" accesskey="X">
                                <i class="fa fa-reply"></i>&emsp;E<u>x</u>it
                            </button>
                        </div>
                    </div>
                </div>
                <div id="tab-content" class="tab-content scrollable-content-details">
                    <div class="tab-pane fade in active show" id="tab1">
                        <div class="row m-0">
                            <div class="col">
                                <div id="tbCFM" runat="server">
                            <div style="margin-top: 10px;" class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="row m-0" style="padding: 5px !important;">
                                    <div class="row m-0">
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Cash Flow Serial No." ID="lblCashflowNo" CssClass="styleDisplayLabel"></asp:Label>
                                                </label>
                                                <asp:TextBox ID="txtCashflowNo" runat="server" ReadOnly="true" ToolTip="Cash Flow Serial Number" class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Line of Business" CssClass="styleReqFieldLabel" ID="lblLOB"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" ToolTip="Line of Business" CssClass="md-form-control form-control"
                                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ErrorMessage="Select the Line of Business" SetFocusOnError="true" ValidationGroup="Save"
                                                        ControlToValidate="ddlLOB" InitialValue="0" Display="Dynamic" CssClass="validation_msg_box_sapn"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Inflow/Outflow" ID="lblCashflow" CssClass="styleReqFieldLabel"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlCashflow" runat="server" ToolTip="Inflow/Outflow" AutoPostBack="True" CssClass="md-form-control form-control"
                                                    OnSelectedIndexChanged="ddlCashflow_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvCashflow" runat="server" ErrorMessage="Select the Inflow/Outflow" ValidationGroup="Save"
                                                        ControlToValidate="ddlCashflow" InitialValue="0" Display="Dynamic"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Cash Flow Description" ID="lblCashflowDesc" CssClass="styleReqFieldLabel"></asp:Label>
                                                </label>
                                                <asp:TextBox ID="txtCashflowDesc" runat="server" MaxLength="50" ToolTip="CashFlow Description" class="md-form-control form-control login_form_content_input requires_true"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtCashflowDesc"
                                                    FilterType="Custom , UppercaseLetters, LowercaseLetters, Numbers" ValidChars=" "
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvCashflowDesc" runat="server" ValidationGroup="Save" ErrorMessage="Enter the Cash Flow Description"
                                                        ControlToValidate="txtCashflowDesc" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Cash Flow Flag" ID="lblCashflowFlag" CssClass="styleReqFieldLabel"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlCashflowFlag" runat="server" ToolTip="Cash Flow Flag" AutoPostBack="True" CssClass="md-form-control form-control"
                                                    OnSelectedIndexChanged="ddlCashflowFlag_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvCashflowflag" runat="server" ErrorMessage="Select the Cash Flow Flag" ValidationGroup="Save"
                                                        ControlToValidate="ddlCashflowFlag" InitialValue="0" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Scheme" ID="lblProduct" CssClass="styleDisplayLabel"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlProduct" runat="server" ToolTip="Scheme" AutoPostBack="True" CssClass="md-form-control form-control" OnSelectedIndexChanged="ddlProductSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Recovery Type" ID="lblRecoveryType" CssClass="styleReqFieldLabel"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlRecoveryType" runat="server" ToolTip="Recovery Type" CssClass="md-form-control form-control">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Active" ID="Label7"></asp:Label>
                                                </label>
                                                <asp:CheckBox ID="CbxActive" runat="server" ToolTip="Is Active" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%-- </table>
                    </td>--%>
                </div>

                <div class="row m-0" id="trDebit" runat="server">
                    <asp:Label runat="server" Text="Debit" ID="lblDebit" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                    <asp:Panel runat="server" ID="pnlDebit" Width="100%" GroupingText="Debit" CssClass="stylePanel">
                        <div style="margin-top: 10px;" class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="col">
                                <div id="tblDebit" runat="server">
                                    <div class="row mt-3 m-0">
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Type" ID="Label2"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlDType" runat="server" AutoPostBack="True" CssClass="md-form-control form-control"
                                                    OnSelectedIndexChanged="ddlDType_SelectedIndexChanged" ToolTip="Debit Type">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvDType" runat="server" ErrorMessage="Select the Type(Debit)"
                                                        ControlToValidate="ddlDType" InitialValue="0" Display="Dynamic" ValidationGroup="Save"
                                                        Enabled="false"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="GL Account Code" ID="Label8"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlDGLCode" runat="server" ToolTip="Debit GL Code" CssClass="md-form-control form-control"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlDGLCode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvDGlcode" runat="server" ErrorMessage="Select the GL Account Code(Debit)"
                                                        ControlToValidate="ddlDGLCode" InitialValue="0" Display="Dynamic" Enabled="false" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="SL Account Code" ID="Label3"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlDSLCode" runat="server" ToolTip="Debit SL Code" CssClass="md-form-control form-control"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlDSLCode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvDSlcode" runat="server" ErrorMessage="Select the SL Account Code(Debit)" ValidationGroup="Save"
                                                        ControlToValidate="ddlDSLCode" InitialValue="0" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div style="display: none;">
                                                <asp:Label runat="server" Text="GL Account Code" ID="Label1"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="row m-0" id="trCredit" runat="server">
                    <asp:Label runat="server" Text="Credit" ID="lblCredit" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                    <asp:Panel runat="server" ID="pnlCredit" Width="100%" GroupingText="Credit" CssClass="stylePanel">
                        <div style="margin-top: 10px;" class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="col">
                                <div id="tblCredit" runat="server">
                                    <div class="row mt-3 m-0">
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="Type" ID="Label6"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlCType" runat="server" AutoPostBack="True" CssClass="md-form-control form-control"
                                                    OnSelectedIndexChanged="ddlCType_SelectedIndexChanged" ToolTip="Credit Type">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvCType" runat="server" ErrorMessage="Select the Type(Credit)"
                                                        ControlToValidate="ddlCType" InitialValue="0" Display="Dynamic" Enabled="false" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="GL Account Code" ID="Label4"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlCGLCode" runat="server" ToolTip="Credit GL Code" CssClass="md-form-control form-control"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCGLCode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvCGlcode" runat="server" ErrorMessage="Select the GL Account Code(Credit)" ValidationGroup="Save"
                                                        ControlToValidate="ddlCGLCode" InitialValue="0" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                            <div class="md-input">
                                                <label class="tess">
                                                    <asp:Label runat="server" Text="SL Account Code" ID="Label9"></asp:Label>
                                                </label>
                                                <asp:DropDownList ID="ddlCSLCode" runat="server" ToolTip="Credit SL Code" CssClass="md-form-control form-control"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCSLCode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="highlight"></span>
                                                <span class="bar"></span>
                                                <div class="validation_msg_box">
                                                    <asp:RequiredFieldValidator ID="rfvCSlcode" runat="server" ErrorMessage="Select the SL Account Code(Credit)"
                                                        ControlToValidate="ddlCSLCode" InitialValue="0" Display="Dynamic" ValidationGroup="Save"
                                                        Enabled="false"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div style="display: none;">
                                                <asp:Label runat="server" Text="GL Account Code" ID="Label5"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="row mt-2 m-0 text-center">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="row m-0 justify-content-center">
                            <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12 text-center">
                                <div class="checkbox-inline">
                                    <asp:CheckBox ID="CbxBusIRR" runat="server" AutoPostBack="True" ToolTip="Business IRR"
                                        OnCheckedChanged="CbxBusIRR_CheckedChanged" />
                                    &nbsp;<asp:Label runat="server" Text="Business IRR" ID="lblBusIRR" CssClass="control-label"></asp:Label>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12 text-center">
                                <div class="checkbox-inline">
                                    <asp:CheckBox ID="CbxAccIRR" runat="server" AutoPostBack="True" ToolTip="Accounting IRR"
                                        OnCheckedChanged="CbxAccIRR_CheckedChanged" />
                                    &nbsp;<asp:Label runat="server" Text="Accounting IRR" ID="lblAccIRR" CssClass="control-label"></asp:Label>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12 text-center">
                                <div class="checkbox-inline">
                                    <asp:CheckBox ID="CbxBoth" runat="server" AutoPostBack="True" ToolTip="Company IRR"
                                        OnCheckedChanged="CbxBoth_CheckedChanged" />
                                    &nbsp;<asp:Label runat="server" Text="Company IRR" ID="lblBoth" CssClass="control-label"></asp:Label>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12 text-center">
                                <div class="checkbox-inline">
                                    <asp:CheckBox ID="CbxAll" runat="server" AutoPostBack="True" ToolTip="All" OnCheckedChanged="CbxAll_CheckedChanged" />
                                    &nbsp;<asp:Label runat="server" Text="All" ID="lblAll" CssClass="control-label"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row m-0">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Panel runat="server" ID="panGeneralParameters" GroupingText="Program Reference Grid" Width="100%" ScrollBars="Auto"
                            CssClass="stylePanel" Style="overflow: hidden">
                            <div style="height: 235px; overflow-x: auto; overflow-y: auto;">
                                <asp:GridView ID="gvCashflowProgram" runat="server" Width="100%" AutoGenerateColumns="False" ScrollBars="Auto"
                                    DataKeyNames="Program_ID" OnRowDataBound="gvCashflowProgram_RowDataBound" class="gird_details">
                                    <Columns>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProgramid" runat="server" Text='<%# Eval("Program_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Program Ref. No." HeaderStyle-CssClass="styleGridHeader">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProgramRefNo" runat="server" Text='<%# Eval("Program_Ref_No") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Program Description" HeaderStyle-CssClass="styleGridHeader">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProgramName" runat="server" Text='<%# Eval("Program_Name") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle Width="50%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Capture" HeaderStyle-CssClass="styleGridHeader" Visible="false">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="CbxCapture" Enabled="false" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Display" HeaderStyle-CssClass="styleGridHeader" Visible="false">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="CbxDisplay" Enabled="false" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Post" HeaderStyle-CssClass="styleGridHeader" >
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="CbxPost" OnCheckedChanged="CbxPost_CheckedChanged" AutoPostBack="true" ToolTip="Post" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </div>
                    </div>
                </div>
                <%-- </table>
            </td>
                </tr>--%>
                        </div>
                    </div>
                </div>
                <div class="p-2 pb-5 mb-4">
                    <asp:ValidationSummary ID="vsCashfolw" runat="server" ShowSummary="true"
                        CssClass="styleMandatoryLabel" ValidationGroup="Save" ShowMessageBox="false"
                        HeaderText="Correct the following validation(s):" />
                    <asp:CustomValidator ID="cvCashflow" runat="server" CssClass="styleMandatoryLabel"
                        Enabled="true" />
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" />
                </div>
                <div class="fixed_btn" style="bottom: 9px;">
                    <div class="col p-0">
                        <button class="btn btn-success mr-2" id="btnSave" title="Save[Alt+S]" onclick="if(fnCheckPageValidators('Save'))" causesvalidation="false" onserverclick="btnSave_Click" runat="server"
                            type="button" accesskey="S" validationgroup="Save">
                            <i class="fa fa-floppy-o"></i>&emsp;<u>S</u>ave
                        </button>
                        <button class="btn btn-outline-success" id="btnClear" title="Clear[Alt+L]" onclick="if(fnConfirmClear())" causesvalidation="false" onserverclick="btnClear_Click" runat="server"
                            type="button" accesskey="L">
                            <i class="fa fa-refresh"></i>&emsp;C<u>l</u>ear
                        </button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">

        function verifyString(e) {
            document.getElementById(e).value = document.getElementById(e).value.trim();
        }

        function fnUnselectAllExpectSelected(TargetControl, gRow) {
            var TargetBaseControl = document.getElementById('<%=gvCashflowProgram.ClientID %>');
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                Inputs[n].parentElement.parentElement.parentElement.style.backgroundColor = "white";
                if (Inputs[n].type == 'checkbox' && Inputs[n].uniqueID != TargetControl.uniqueID) {
                    Inputs[n].checked = false;
                }
            }
            if (TargetControl.checked == true) {
                document.getElementById(gRow).style.backgroundColor = "#f5f8ff";
            }
            else {
                document.getElementById(gRow).style.backgroundColor = "white";
            }
        }

    </script>

</asp:Content>
