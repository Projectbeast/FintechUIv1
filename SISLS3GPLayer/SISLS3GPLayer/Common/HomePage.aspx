<%@ Page Title="Home" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="HomePage" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <script type="text/javascript" language="javascript">
        function triggerParent() {
            if (window.parent && typeof window.parent.triggerMasterButtonClick === 'function') {
                window.parent.triggerMasterButtonClick();
            }
        }
    </script>


    <div class="container-fluid home-page-scroll pb-5">
        <div class="row mb-5">
            <div class="col-12">
                <div class="home-bg-image"></div>
            </div>
            <div class="col-12 p-2 mt-2">
                <div class="text-center">
                    <span class="quick-access">Quick access</span>
                </div>
            </div>
            <div class="col-12 mt-1">
                <div class="text-center">
                    <span class="home-info-text">Select and add tabs that are relavent to your workflow today</span>
                </div>
            </div>

        </div>
        <div class="col d-flex flex-row p-0  home-container pb-3 px-5" id="cardRow" runat="server">

            <asp:Repeater ID="rptModules" runat="server">
                <ItemTemplate>
                    <div class="col col-md-3 mr-3">
                        <div class="card shadow-sm" style="height: 250px; min-width: 300px">
                            <!-- Fixed height for the card -->
                            <!-- Card Header -->
                            <div class="card-header mx-3 card-home-header d-flex">
                                <div class="header-icon-bg">
                                    <i class="fa fa-university" aria-hidden="true"></i>
                                </div>
                                <span class="card-title mt-2"><%# Eval("ModuleName") %></span>
                            </div>

                            <!-- Card Body with Scroll -->
                            <div class="card-body" style="height: 100%; overflow-y: auto;">
                                <asp:Repeater ID="rptPrograms" runat="server" DataSource='<%# Eval("Programs") %>'>
                                    <ItemTemplate>
                                        <div class="home-menu-items">
                                            <!-- Checkbox with larger size -->
                                            <%--<asp:CheckBox ID="chkProgram" runat="server" CssClass="mr-2 large-checkbox" />--%>
                                            <label class="custom-checkbox">
                                                <asp:CheckBox ID="chkProgram" runat="server" CssClass="mr-2 "
                                                    Checked='<%# Convert.ToBoolean(Eval("programischecked")) %>' />
                                                <span class="checkmark"></span>
                                            </label>
                                            <asp:HiddenField ID="hdnProgramId" runat="server" Value='<%# Eval("ProgramId") %>' />
                                            <asp:HiddenField ID="hdnProgramCount" runat="server" Value='<%# Eval("ProgramCount") %>' />
                                            <asp:HiddenField ID="hdnModuleCount" runat="server" Value='<%# Eval("ModuleCount") %>' />

                                            <!-- Program Name as Link -->
                                            <a href='javascript:LoadDynamictTabIframe("<%# Eval("ProgramName") %>", "<%# Eval("PageUrl") %>", "1");RemoveTabBasedOnCount("6");'
                                                id='A1'
                                                class='d-block text-decoration-none w-100'
                                                title='<%# Eval("ProgramName") %>'>
                                                <%# Eval("ProgramName") %>
                                            </a>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>




        </div>
        <!-- <div class="row">
            <div class="col-6">
                <div class="text-left">
                    <img alt="home" src="../Content/Images/home-bg-left.png" width="170px"/>
                </div>
                
            </div>
            <div class="col-6">
                
                 <div class="text-right">
                     <img alt="home" src="../Content/Images/home-bg-right.png" width="170px"/>
                </div>
            </div>
        </div>-->
    </div>
    <div class="text-center fixed_btn p-3">
        <%--<asp:Button ID="btnTrigger" runat="server" Text="Trigger Master Event" OnClientClick="triggerParent();" />--%>
        <button id="btnopen" runat="server" onserverclick="btnopen_ServerClick" class="btn btn-success mr-2"><i class="fa fa-check mr-1" aria-hidden="true"></i>Open</button>
        <button id="btnsaveconfiguration" onserverclick="btnsaveconfiguration_ServerClick" runat="server" class="btn btn-outline-success btn-config-save"><i class="fa fa-bookmark-o mr-2" aria-hidden="true"></i>Save Configuration</button>


    </div>




</asp:Content>
