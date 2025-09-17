<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true" CodeFile="GetCustomerSearch.aspx.cs" Inherits="Common_GetCustomerSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/LOVPageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function popup(strVal, strVal2, strControl, strControl3) {
            //window.opener.document.body.disabled = false;

            //var coll = window.opener.document.forms[0].elements; for (var i = 0; i < coll.length; i++) {

            //    coll[i].disabled = false;
            //}

            opener.window.document.getElementById(strControl).value = strVal;
            opener.window.document.getElementById(strControl3).value = strVal2;
            opener.window.document.getElementById(strControl).focus();

            window.close();

        }
        function ScreenClose() {
            window.close();
        }

        window.onload = function () {
            //window.opener.document.body.disabled = true;

            //var coll = window.opener.document.forms[0].elements;
            //for (var i = 0; i < coll.length; i++) {

            //    coll[i].disabled = true;
            //}

        }


        window.onunload = function () {
            //window.opener.document.body.disabled = false;

            //var coll = window.opener.document.forms[0].elements; for (var i = 0; i < coll.length; i++) {

            //    coll[i].disabled = false;
            //}
        }


    </script>
    <div class="section-box mx-3 p-3 mt-3 scrollable-content">

        <div class="row">
            <asp:Label ID="lblHeaderText" CssClass="styleDisplayLabel" runat="server"></asp:Label>
            <%-- <asp:TextBox ID="FocusCtrl" Width="0px" runat="server" ></asp:TextBox>--%>
        </div>
        <div class="row">
            <asp:HiddenField ID="hdnQuery" runat="server" />
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
            <input type="hidden" id="hdnSortDirection" runat="server" />
            <input type="hidden" id="hdnSortExpression" runat="server" />
        </div>
        <div class="row">
            <div class="col">
                <div class="float-right">
                    <button class="btn btn-outline-success btn-create" id="btnExit" title="Exit[Alt+T]" onserverclick="btnExit_ServerClick" causesvalidation="false"
                        onclick="if(fnConfirmExit())" runat="server" type="button" accesskey="T">
                        <i class="fa fa-reply" aria-hidden="true"></i>&emsp;Re<u>t</u>urn
                    </button>

                </div>
            </div>
        </div>
        <div class="row m-0">
            <asp:Panel ID="pnlcustomerhdr" runat="server" CssClass="col p-0">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 p-0">
                    <div class="col p-0">
                        <div class="row m-0">
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <label class="tess">
                                        <asp:Label ID="lblCustomerCode" runat="server" ToolTip="Customer Code" Text="Customer Code"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtCustomerCode" MaxLength="20" CssClass="md-form-control form-control login_form_content_input requires_false" runat="server"></asp:TextBox>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>

                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <label>
                                        <asp:Label ID="lblCustomerName" runat="server" ToolTip="Customer Name" Text="Customer Name"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtcustomerName" MaxLength="20" CssClass="md-form-control form-control login_form_content_input requires_false" runat="server"></asp:TextBox>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>

                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <label>
                                        <asp:Label ID="lblEmployeerCode" runat="server" ToolTip="Employer Code" Text="Employer Code"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtEmployeerCode" MaxLength="20" CssClass="md-form-control form-control login_form_content_input requires_false" runat="server"></asp:TextBox>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>

                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <label>
                                        <asp:Label ID="lblAccountNumber" runat="server" ToolTip="Account No." Text="Account No."></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtAccountNumber" MaxLength="20" CssClass="md-form-control form-control login_form_content_input requires_false" runat="server"></asp:TextBox>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>

                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <label>
                                        <asp:Label ID="lblnidcrno" runat="server" ToolTip="NID/CR/RID No." Text="NID/CR/RID No."></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtNIDCRNo" MaxLength="20" CssClass="md-form-control form-control login_form_content_input requires_false" runat="server"></asp:TextBox>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>

                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12 styleFieldLabel">
                                <div class="md-input">
                                    <label>
                                        <asp:Label ID="lblRegNumber" runat="server" ToolTip="Reg. Number" Text="Reg. Number"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtRegNumber" MaxLength="20" CssClass="md-form-control form-control login_form_content_input requires_false" runat="server"></asp:TextBox>
                                    <span class="highlight"></span>
                                    <span class="bar"></span>

                                </div>
                            </div>
                            <div class="row">
                                <div class="styleRecordCount">
                                    <div>
                                        <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div>

                        <button class="btn btn-success mr-2" id="btnGo" onserverclick="btnGo_Click" runat="server"
                            type="button" causesvalidation="false" accesskey="G" title="Go,Alt+G">
                            <i class="fa fa-arrow-circle-right" aria-hidden="true"></i>&emsp;<u>G</u>o
                        </button>
                        <button class="btn btn-outline-success mr-2" id="btnClear" title="Clear[Alt+R]" onclick="if(fnConfirmClear())" causesvalidation="false" runat="server"
                            onserverclick="btnClear_Click"
                            type="button" accesskey="R">
                            <i class="fa fa-refresh" aria-hidden="true"></i>&emsp;<u>R</u>eset
                        </button>

                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="gird">
                    <!--class="gird_details"-->
                    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false"
                        HeaderStyle-CssClass="styleGridHeader" OnRowDataBound="gvList_RowDataBound" ShowHeader="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Code" ItemStyle-Width="20%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnSort1" Enabled="false" runat="server" Text="Customer Code" ToolTip="Sort By Customer Code"
                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                    <%--  <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<% # Bind("ID")%>' Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lbId" runat="server" Text='<% # Bind("Code")%>' CommandName="Select"
                                        CommandArgument='<% # Bind("ID")%>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" ItemStyle-Width="40%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnSort2" Enabled="false" runat="server" Text="Customer Name" ToolTip="Sort By Customer Name"
                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                    <%-- <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbName" runat="server" Text='<% # Bind("Name")%>' CommandName="Select"
                                        CommandArgument='<% # Bind("ID")%>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Number" ItemStyle-Width="20%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnSort3" Enabled="false" runat="server" Text="Account No." ToolTip="Sort By Account Number"
                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                    <%-- <asp:ImageButton ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbAccName" runat="server" Text='<% # Bind("panum")%>' CommandName="Select"
                                        CommandArgument='<% # Bind("ID")%>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reg. Number" ItemStyle-Width="20%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnSort4" Enabled="false" runat="server" Text="Reg. Number" ToolTip="Sort By Registration Number"
                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                    <%--<asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbRegisNo" runat="server" Text='<% # Bind("Registrationno")%>' CommandName="Select"
                                        CommandArgument='<% # Bind("ID")%>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NID/CR/RID Number" ItemStyle-Width="20%">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkbtnSort5" Enabled="false" runat="server" Text="NID/CR/RID Number" ToolTip="NID/CR/RID Number"
                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                    <%--<asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbCRNNo" runat="server" Text='<% # Bind("CRNUMBER")%>' CommandName="Select"
                                        CommandArgument='<% # Bind("ID")%>' Font-Underline="false" ForeColor="Black"> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle BackColor="AliceBlue" />
                        <PagerStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                        <HeaderStyle CssClass="styleGridHeader" />
                    </asp:GridView>
                </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <uc1:PageNavigator ID="ucLOVPageNavigater" runat="server" Visible="false" />
            </div>
        </div>
    </div>

    <%--    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                
            </div>

        </div>
    </div>--%>
</asp:Content>
