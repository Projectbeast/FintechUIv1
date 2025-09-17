﻿ <%@ control language="C#" autoeventwireup="true" inherits="UserControls_S3GCustomerAddress, App_Web_yuh0ce1b" %>
<asp:MultiView ID="MCustomerView" runat="server" ActiveViewIndex="0">
    <asp:View ID="CustomerDetailV1" runat="server">
        <table cellspacing="0" width="100%" runat="server" id="tbleV1">
            <tr>
                <td id="V1FirstColumn1">
                    <div class="md-input">
                        <asp:TextBox ID="txtCustomerCode" runat="server" ReadOnly="True" TabIndex="-1"
                            ToolTip="Customer Code" class="md-form-control form-control login_form_content_input"></asp:TextBox>
                        <span class="highlight"></span>
                        <span class="bar"></span>
                        <label>
                            <asp:Label ID="lblCustomerCode" runat="server" Text="Customer Code"></asp:Label>
                        </label>
                    </div>
                </td>
            </tr>
            <tr>
                <td id="V1FirstColumn2">
                    <div class="md-input">
                        <asp:TextBox ID="txtCustomerName" runat="server" TabIndex="-1" ReadOnly="True
                        "
                            ToolTip="Customer Name" class="md-form-control form-control login_form_content_input"></asp:TextBox>
                        <span class="highlight"></span>
                        <span class="bar"></span>
                        <label>
                            <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name">
                            </asp:Label>
                        </label>
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top" id="V1FirstColumn3">
                    <div class="md-input">
                        <asp:TextBox ID="txtCusAddress" runat="server" ReadOnly="true" Rows="4"
                            class="md-form-control form-control login_form_content_input"
                            TextMode="MultiLine" TabIndex="-1" ToolTip="Address"></asp:TextBox>
                        <span class="highlight"></span>
                        <span class="bar"></span>
                        <label>

                            <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                        </label>
                    </div>

                </td>
            </tr>
            <tr>
                <td id="V1FirstColumn4">
                    <div class="md-input">
                        <div>
                        <asp:TextBox ID="txtPhone" runat="server" ReadOnly="True" TabIndex="-1"
                            class="md-form-control form-control login_form_content_input float_left" Width="40%" ToolTip="Phone"></asp:TextBox>
                        <asp:Label ID="lblMobile" runat="server" CssClass="styleDisplayLabel float_left label_padding" Width="20%" Text="[M]"></asp:Label>
                        <asp:TextBox ID="txtMobile" runat="server" TabIndex="-1" ReadOnly="true" Width="40%"
                            class="md-form-control form-control login_form_content_input float_left" ToolTip="Mobile"></asp:TextBox>
                        </div>
                        <span class="highlight"></span>
                        <span class="bar"></span>
                        <label>
                            <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label>
                        </label>
                    </div>
                </td>
            </tr>
            <tr >
                <td id="V1FirstColumn5">
                    <div class="md-input">
                        <asp:TextBox ID="txtEmail" runat="server" ReadOnly="True" TabIndex="-1"
                            class="md-form-control form-control login_form_content_input" ToolTip="EMail ID"></asp:TextBox>

                        <span class="highlight"></span>
                        <span class="bar"></span>
                        <label>
                            <asp:Label ID="lblEmail" runat="server" Text="Email Id"></asp:Label>
                        </label>
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td  id="V1FirstColumn6">
                    <div class="md-input">
                        <asp:TextBox ID="txtWebSite" runat="server" ReadOnly="True" TabIndex="-1"
                            class="md-form-control form-control login_form_content_input" ToolTip="Website"></asp:TextBox>

                        <span class="highlight"></span>
                        <span class="bar"></span>
                        <label>
                            <asp:Label ID="lblWebsite" runat="server" Text="Website"></asp:Label>
                        </label>
                    </div>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="CustomerDetailV2" runat="server">
        <table cellspacing="0" width="100%" runat="server" id="tblView2">
            <tr>
                <td class="styleFieldLabel" id="FirstColumn1">
                    <asp:Label ID="lblCustomerCode1" runat="server" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                </td>
                <td class="styleFieldAlign" id="SecondColumn1" width="30%">
                    <asp:TextBox ID="txtCustomerCode1" runat="server" ReadOnly="True" TabIndex="-1" Width="65%" ToolTip="Customer Code"></asp:TextBox>
                </td>
                <td class="styleFieldLabel" id="ThirdColumn1">
                    <asp:Label ID="lblCustomerName1" runat="server" CssClass="styleDisplayLabel" Text="Customer Name"> </asp:Label>
                </td>
                <td class="styleFieldAlign" id="FourthColumn1" width="30%">
                    <asp:TextBox ID="txtCustomerName1" runat="server" ReadOnly="True" TabIndex="-1" ToolTip="Customer Name"
                        Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="styleFieldLabel" id="FirstColumn2">
                    <asp:Label ID="lblCusAddress1" runat="server" CssClass="styleDisplayLabel" Text="Address"></asp:Label>
                </td>
                <td rowspan="3" class="styleFieldAlign" id="SecondColumn2">
                    <asp:TextBox ID="txtCusAddress1" runat="server" ReadOnly="true" TabIndex="-1" Width="95%" Rows="4"
                        TextMode="MultiLine" ToolTip="Address"></asp:TextBox>
                </td>
                <td class="styleFieldLabel" id="ThirdColumn2">
                    <asp:Label ID="lblPhone1" runat="server" CssClass="styleDisplayLabel" Text="Phone"></asp:Label>
                </td>
                <td class="styleFieldAlign" id="FourthColumn2">
                    <asp:TextBox ID="txtPhone1" runat="server" ReadOnly="True" Width="44%" TabIndex="-1" ToolTip="Phone"></asp:TextBox>
                    <asp:Label ID="lblMobile1" runat="server" CssClass="styleDisplayLabel" Text="[M]"></asp:Label>
                    <asp:TextBox ID="txtMobile1" runat="server" ReadOnly="True" Width="37%" TabIndex="-1" ToolTip="Mobile"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="styleFieldLabel" id="FirstColumn3"></td>
                <td class="styleFieldLabel" id="ThirdColumn3">
                    <asp:Label ID="lblEmaail1" runat="server" CssClass="styleDisplayLabel" Text="Email Id"></asp:Label>
                </td>
                <td class="styleFieldAlign" id="FourthColumn3">
                    <asp:TextBox ID="txtEmail1" runat="server" ReadOnly="True" TabIndex="-1" Width="90%" ToolTip="EMail ID"></asp:TextBox>
                </td>
            </tr>
            <tr style="display:none">
                <td class="styleFieldLabel" id="FirstColumn4"></td>
                <td class="styleFieldLabel" id="ThirdColumn4">
                    <asp:Label ID="lblWebSite1" runat="server" CssClass="styleDisplayLabel" Text="Website"></asp:Label>
                </td>
                <td class="styleFieldAlign" id="FourthColumn4">
                    <asp:TextBox ID="txtWebSite1" runat="server" ReadOnly="True" TabIndex="-1" Width="90%" ToolTip="Website"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
