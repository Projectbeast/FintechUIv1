<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="SMLMaster.aspx.cs"
    Inherits="Common_S3GMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GAutoSuggest.ascx" TagName="Suggest" TagPrefix="UC2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <title>Smart Fintech Solutions</title>
    <link href="../Content/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../Content/css/style_sheet.css" rel="stylesheet" type="text/css" />
    <link href="../Content/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="../Content/css/custom.css" rel="stylesheet" type="text/css" />
    <link href="../Content/css/tabcontol/demo.css" rel="stylesheet" type="text/css" />
    <link href="../Content/css/tabcontol/reset.css" rel="stylesheet" type="text/css" />
    <link href="../Content/css/tabcontol/style.css" rel="stylesheet" type="text/css" />
    <link href="../Content/css/font.css" rel="stylesheet" type="text/css" />
    <link href="../Content/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body id="bodyMaster" runat="server" onbeforeunload="ConfirmCloseClick(event)" style="margin-right: 0px; margin-left: 1px; margin-bottom: 0px; margin-top: 2px" oncontextmenu="return false;" onkeydown="ConfirmCloseF4(event),ConfirmCloseAltR(event)">
    <form id="form1" method="post" enctype="multipart/form-data" style="margin: 0px;"
        runat="server">
        <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
        </cc1:ToolkitScriptManager>
        <input id="hdnMenuLoad" type="hidden" runat="server" />
        <input id="hdnMenuState" value="0" type="hidden" runat="server" />
        <div class="base_bg">
            <div class="header_bar" id="headermenu">
                <div class="row">

                    <div class="col-3 col-md-4 d-flex">
                        <div class="col" id="SideMenuOpen">
                            <div class="side_menu_icon mt-3" onclick="openNav()">
                                <span style="font-size: 20px !important;"><i class="fa fa-th-list"></i></span>
                            </div>
                        </div>
                        <div class="col-10 col-md-11 p-0">
                            <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Images/smartlend-logo_gray.jpg" />
                            <%--      <img id ID="imgLogo" src="../Content/Images/s3g_logo1.png" style="width: 120px; margin-left: 15px;">
                            --%>
                        </div>
                    </div>
                    <!--<div class="col header_bar_content1 hidden-sm-down">
                        <marquee id="marquee1" scrollamount="2" scrolldelay="15" behavior="scroll" direction="left">                    
                      <span runat="server" id="lblMarquee" onmouseover="document.getElementById('marquee1').stop();" onmouseout="document.getElementById('marquee1').start();" class="styleHeaderInfo">  Sundaram Finance offers you easy interest rate loans...</span>                      
                        </marquee>

                    </div>-->
                    <div class="col col-md-4 p-3 d-none d-sm-block">
                        <span class="welcome-login">Welcome to Smart Fintech Solutions</span>
                    </div>

                    <div class="col-9 col-md-4 header_bar_content2" align="right">
                        <div class="d-flex float-right"  style="height: 0px;">
                            <div>
                                <i class="btn btn-sm fa fa-sliders  user-profile-pic mr-3" aria-hidden="true"></i>
                                <i class="btn btn-sm fa fa-bell  user-profile-pic mr-3" aria-hidden="true"></i>
                                <i class="btn btn-sm fa fa-user-circle-o  user-profile-pic" aria-hidden="true"></i>
                            </div>
                            <div class="dropdown d-flex flex-column">
                                <button class="btn dropdown-toggle px-2 p-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" style="padding: 0px 16px; margin: 0px;" onclick="dropmenuhide()">
                                    
                                    <span class="user-profile">
                                        <span runat="server" id="lblWelcome" class="user-profile">Welcome&nbsp; </span>
                                        <asp:Label ID="lblUser" runat="server" class="user-profile"></asp:Label>
                                    </span>
                                </button>
                                <span class="last-login mt-1">
                                    <span runat="server" id="lblLastLogin" class="last-login">Last Login : </span>
                                    <asp:Label ID="lblLastLoginDate" runat="server" class="last-login"></asp:Label>
                                    &emsp;&emsp;</span>
                                <ul class="dropdown-menu multi-level" aria-labelledby="dropdownMenuButton" x-placement="bottom-start" id="dropmenu">
                                    <li class="dropdown-item">
                                        <a href="" runat="server" style="cursor: pointer"
                                            id="Accpane100_content_lbtn0" onclick="fnLoadIframe(100, 0)" viewpage="HomePage.aspx" detailpage="HomePage.aspx">Home</a>

                                    </li>
                                    <li class="dropdown-item">
                                        <asp:LinkButton CausesValidation="false" runat="server"
                                            ID="lnkAbout" Text="About"></asp:LinkButton>
                                    </li>
                                    <li class="dropdown-item">
                                        <asp:LinkButton CausesValidation="false" runat="server"
                                            ID="lnkMore" Text="More.."></asp:LinkButton>
                                    </li>

                                    <li class="dropdown-submenu">
                                        <span class="dropdown-item" runat="server" id="lblApplyTheme"></span>
                                        <asp:DropDownList ID="ddlTheme" class="dropdown-menu" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTheme_SelectedIndexChanged">
                                            <asp:ListItem class="dropdown-item" Value="S3GTHEME_BLUE" Enabled="false">Blistering Blue</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTHEME_SILVER" Enabled="false">Sizzling Silver</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTHEME_BLUE2" Enabled="false">Blue</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTHEME_ORANGE" Enabled="false">Orange</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTHEME_NIGHT" Enabled="false">Glow</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTHEME_LIGHT" Enabled="false">Light</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTheme_LIGHTBLUE" Enabled="false">Light Blue</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTheme_GREEN_LIGHT">Light Green</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTheme_GREEN_DARK">Dark Green</asp:ListItem>
                                            <asp:ListItem class="dropdown-item" Value="S3GTheme_BLUE_LIGHT" Selected="True">Light Blue</asp:ListItem>                                            
                                            <asp:ListItem class="dropdown-item" Value="S3GTheme_BLUE_DARK" >Dark Blue</asp:ListItem>
                                           
                                        </asp:DropDownList>

                                        <%--                                 <ul  ID="ddlTheme" class="dropdown-menu">
                                        <li class="dropdown-item"><a tabindex="-1" href="#">Blue</a></li>
                                        <li class="dropdown-item"><a tabindex="-1" href="#">Glow</a></li>
                                        <li class="dropdown-item"><a tabindex="-1" href="#">Light</a></li>
                                        <li class="dropdown-item"><a tabindex="-1" href="#">dark Theme</a></li>
                                        <li class="dropdown-item"><a tabindex="-1" href="#">dark Theme</a></li>
                                        <li class="dropdown-item"><a tabindex="-1" href="#">dark Theme</a></li>
                                    </ul>--%>
                                    </li>
                                    <li class="dropdown-item">
                                        <%-- <a href="#">Change Password</a>--%>
                                        <a href="" runat="server" style="cursor: pointer"
                                            id="Accpane100_content_lbtn1" onclick="fnLoadIframe(100, 1)" viewpage="Changepassword.aspx"
                                            detailpage="../Common/Changepassword.aspx" pageurl="Changepassword.aspx">Change Password</a>
                                    </li>
                                    <li class="dropdown-item">
                                        <%--<a href="#">Help</a>--%>
                                        <asp:LinkButton CausesValidation="false" runat="server"
                                            ID="lnkHelp" Text="Help" OnClick="lnkHelp_Click"></asp:LinkButton>
                                    </li>
                                    <li class="dropdown-item">
                                        <%--<a href="#">Log Out</a>--%>
                                        <asp:LinkButton CausesValidation="false" runat="server"
                                            ID="lnkSignOut" Text="Log Out" OnClick="lnkSignOut_Click"></asp:LinkButton>
                                    </li>
                                </ul>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div id="mySidenav" class="sidenav">

                <div class="col d-flex mb-3 m-0 p-0 sidenav-header">
                    <div class="col-2 mt-1" id="Div1">
                        <div class="side_menu_icon mt-3" onclick="openNav()">
                            <span style="font-size: 20px !important;"><i class="fa fa-th-list"></i></span>
                        </div>
                    </div>
                    <div class="col-8 mt-1">
                        <asp:Image ID="sidenavimgLogo" runat="server" ImageUrl="../Images/Light/s3g_logo.png" />
                        <%--      <img id ID="imgLogo" src="../Content/Images/s3g_logo1.png" style="width: 120px; margin-left: 15px;">
                        --%>
                    </div>
                    <div class="col mt-3">
                        <a href="javascript:void(0)" class="closebtn"><i class="fa fa-times" onclick="closeNav()"></i></a>
                    </div>
                </div>



                <asp:Panel ID="pnlMenu" runat="server">
                    <div id="divMenu" title="Menu" class="sidenav-content">
                        <asp:UpdatePanel ID="Uppanel1" runat="server">
                            <ContentTemplate>
                                <cc1:Accordion ID="Accordion1" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                    FadeTransitions="false" FramesPerSecond="40" TransitionDuration="300" AutoSize="None"
                                    RequireOpenedPane="false" SuppressHeaderPostbacks="true" EnableViewState="true"
                                    Width="100%">
                                </cc1:Accordion>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="False" />
            </div>
            <div class="content_base main-content">
                <div class="row mt-2">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server" PathSeparator=" / " EnableTheming="True" Visible="false" CssClass="styleSitepath">
                        <PathSeparatorStyle CssClass="stylePathsep" />
                        <CurrentNodeStyle CssClass="styleCurrentNode" />
                        <NodeStyle CssClass="styleNode" />
                        <RootNodeStyle CssClass="styleRootNode" />
                    </asp:SiteMapPath>
                </div>
                <div class="row px-2 mt-3" id="Container">

                    <div class="col-md-8 col-sm-7 col-xs-12">

                        <!-- Nav tabs -->
                        <nav style="overflow-x: auto; overflow-y: hidden;">
                            <ul id="ulContainer" class="nav nav-tabs dynamic_tab" role="tablist">
                             <!-- <li id="SideMenuOpen">
                                  <div class="side_menu_icon" onclick="openNav()">
                                <span style="font-size:15px !important;"><i class="fa fa-th-list"></i></span>
                              </div>
                              </li>-->
                              <li class="active" id="lihome">
                                  <a href="#tab1" role="tab" data-toggle="tab"  onclick="LoadbasedonTab(this)">
                                    <asp:Label ID="Label1"  runat="server" Text="Home" ToolTip="Home"></asp:Label>
                                  </a>
                                  </li>
                            </ul>
                          </nav>
                    </div>

                    <div class="col-md-4 col-sm-5 col-xs-12">
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="padding: 0px !important;" class="quick_search_input">
                                    <UC2:Suggest ID="ddlProgram" class="styleHeaderInfo quick_search" IsWatermark="true" runat="server" ServiceMethod="GetProgramList" AutoPostBack="true" ToolTip="Program Name"
                                        OnItem_Selected="ddlProgram_Item_Selected" Visible="true" />
                                    <%--<input class="form-control quick_search" placeholder="search" />--%>
                                </td>
                                <!--  <td style="padding: 0px !important; width: 35px">
                                    <div align="right" style="text-align: right;">
                                        <a href="#" onclick="menuhide()" style="background-color: #ada696; padding: 5px 10px; line-height: 25px;"><i class="fa fa-angle-double-up" aria-hidden="true" id="icon_fa"></i></a>
                                    </div>
                                </td>-->
                            </tr>
                        </table>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col" style="border: thick;">
                            <div class="col-md-12">
                                <%--<asp:Label ID="lblProgram" runat="server" Text="Program Name" Width="200px" CssClass="md-form-control form-control login_form_content_input"></asp:Label>--%>
                                <%--<UC2:Suggest ID="ddlProgram" class="styleHeaderInfo" IsWatermark="true" runat="server" ServiceMethod="GetProgramList" AutoPostBack="true" ToolTip="Program Name"
                                    OnItem_Selected="ddlProgram_Item_Selected" Visible="true" />--%>
                            </div>
                            <%-- <span class="highlight"></span>
                            <span class="bar"></span>
                            <label>
                                <asp:Label ID="lblProgram" runat="server" Text="Program"></asp:Label>
                            </label>--%>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <!-- Content page -->
                        <div id="Default" style="overflow: hidden;">
                            <div class="content_base_details" id="content_base">
                                <div id="tab-content" class="tab-content">
                                    <div class="tab-pane fade in active show" id="tab1">
                                        <iframe runat="server" id="ifrmMain" width="100%" frameborder="0" marginheight="0" class="iframe_id frm_height"
                                            marginwidth="0" scrolling="yes" visible="true" src="../Common/HomePage.aspx"></iframe>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Content page -->

                    </div>

                    <div class="col-md-12">
                        <asp:PlaceHolder runat="server" ID="plcHolder"></asp:PlaceHolder>
                    </div>
                </div>
            </div>
            <div class="footer_bar">
                <div class="row">
                    <div class="col-4 mt-1 px-4" align="left">
                        <asp:Image ID="FooterImgLogo" runat="server"  style="width: 70px;"/>
                    </div>
                    <div class="col-8 p-3 pr-4">
                        � Smart Fintech Solution - Copyright 2025
                    </div>
                </div>
            </div>

        </div>


        <asp:HiddenField ID="hdnImgBtn" runat="server" />
        <asp:HiddenField ID="hdnTabCount" runat="server" />
        <%--<div id="divLoading" style="display: none;">
            <input type="button" value="Cancel" onclick="abort()"; />
    </div>--%>
    </form>
    <script type="text/javascript" src="../Content/Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Content/Scripts/tabcontrol/main.js"></script>
    <script type="text/javascript" src="../Content/Scripts/bootstrap.js"></script>
    <script type="text/javascript" src="../Content/Scripts/UserScript.js"></script>
    <%--
    <script src="../JQueryTabs/JQuery/CreateTabs.js" type="text/javascript"></script>--%>

    <script type="text/javascript" language="javascript">
        function triggerMasterButtonClick() {
            __doPostBack('ddlProgram$btnSelected', '');
        }
        //function triggerMasterButtonClick() {
        //    __doPostBack('ddlProgram$btnOpenProgram', '');
        //}

        window.onerror = function () { return true; }
        function autoResize(id) {
            var newheight;
            var newwidth;
            try {
                if (document.getElementById) {
                    newheight = document.getElementById(id).contentWindow.document.body.scrollHeight;
                    newwidth = document.getElementById(id).contentWindow.document.body.scrollWidth;
                }

                if (newheight < 420) {
                    document.getElementById(id).height = 420;
                }
                else {
                    document.getElementById(id).height = (newheight + 5) + "px";
                }
                //document.getElementById(id).width = (screen.width - 50) + "px";

                for (var i = 0; i <= window.frames.length - 1; i++) {
                    // window.frames[i].document.body.clientWidth = (screen.width - 275);
                    //window.frames[i].document.body.style.width = (screen.width - 275) + "px";
                }

            }
            catch (e) {
            }

        }
        function GetResize(img) {

            try {
                //img = document.getElementById('imgDetails');
                //
                if (img.title != null) {

                    if (img.title == "Hide Menu") {
                        for (var i = 0; i <= window.frames.length - 1; i++) {

                            window.frames[i].window.frameElement.width = (screen.width - 53);
                            window.frames[i].window.document.body.style.width = (screen.width - 53);

                            try {
                                if (window.frames[i].GetChildGridResize(img.title) != null)
                                    window.frames[i].GetChildGridResize(img.title);
                            }
                            catch (e) {
                            }
                        }
                    }
                    else {
                        for (var i = 0; i <= window.frames.length - 1; i++) {

                            window.frames[i].window.frameElement.width = (screen.width - 257);
                            window.frames[i].window.document.body.style.width = (screen.width - 257);

                            try {
                                if (window.frames[i].GetChildGridResize(img.title) != null)
                                    window.frames[i].GetChildGridResize(img.title);
                            }
                            catch (e) {
                            }
                        }
                    }
                }
                //
                for (var i = 0; i <= window.frames.length - 1; i++) {
                    var newheight = window.frames[i].frameElement.clientHeight + 20;
                    window.frames[i].window.document.body.style.height = newheight;
                }
            }
            catch (e) {
            }

        }

        function widowminimize() {

            window.blur();
        }

        function funUnload() {
            if (window.opener != null) {
                window.opener.reload(true);
            }
        }

        function CancelF12() {

            var KeyAscii = window.event.keyCode;

            if (KeyAscii == 123) {

                window.event.returnValue = false;

                window.event.keyCode = 0;

            }

        }


        function GetMenuVisibility() {

            var all_Cookies = document.cookie.split(';');

            var tmp_Cookie;
            var Cookie_Value = 1;
            var Cookie_Name;
            //
            for (var i = 0; i < all_Cookies.length; i++) {
                tmp_Cookie = all_Cookies[i].split('=');
                Cookie_Name = tmp_Cookie[0];
                if (Cookie_Name == 'Menu') {
                    Cookie_Value = tmp_Cookie[1];
                    break;
                }
            }

            if (Cookie_Value == 0) {
                showMenu('F');
            }
            else {
                showMenu('T');
            }
        }

        function SetMenuVisibility(CanShow) {
            document.cookie = "Menu=" + CanShow + ";";
        }

        function FloatMenu() {
            $get('divMenu').style.position = 'Absolute';
            $get('divMenu').style.background = 'White';
            $get('divMenu').style.Height = '450px';
            $get('divMenu').style.border = 1;
        }

        //window.onload = window_onload;
        var prm;
        function window_onload() {
            prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            prm.add_pageLoaded(PageLoaded);
            //document.getElementById('divLoading').style.display='none';
            //GetMenuVisibility();
        }


        function InitializeRequest(sender, args) {
            $get('Container').style.cursor = 'wait';
            //$get('Container').style.cursor='wait';
            $get('Container').disabled = true;

            //  document.getElementById('divLoading').style.display='Block';

            //$get('Container').className='Progress';
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                $get('Container').disabled = false;
                //  $get('Container').className='';
                $get('Container').style.cursor = 'default';
                // document.getElementById('divLoading').style.display='none';
            }

        }

        function PageLoaded(sender, args) {
            // alert(sender);
            //



            $get('Container').style.cursor = 'auto';
            //$get('Container').style.cursor='auto';
            $get('Container').disabled = false;
            //  document.getElementById('divLoading').style.display='none';
            //  $get('Container').className='';
            //autoResize('ifrmMain');



        }
        function EndRequest(sender, args) {
            if (args.get_error() != undefined) {
                $get('Container').style.cursor = 'auto';
                $get('Container').disabled = false;
                // document.getElementById('divLoading').style.display='none';
                args.set_errorHandled(true);
            }


            //This lines are used to create new tab after Master page get postback
            if (sender._postBackSettings != null && document.getElementById(sender._postBackSettings.sourceElement.id).parentNode.parentNode.parentNode.id == 'Accordion1') {
                parent.CreateTabForChild(document.getElementById(sender._postBackSettings.sourceElement.id).lastChild.nodeValue, document.getElementById(sender._postBackSettings.sourceElement.id).attributes["PageUrl"].nodeValue);
            }

        }
        function abort() {
            alert(prm.get_isInAsyncPostBack());
            if (prm.get_isInAsyncPostBack()) {
                //  abort the postback
                prm.abortPostBack();
                //  get the reference to the animation for the gridview
                $get('Container').style.cursor = 'auto';
                $get('Container').disabled = false;
                //  document.getElementById('divLoading').style.visible='none';

            }
        }

        function fnLoadIframe(intModuleCount, intProgCount) {
            var curProgram = document.getElementById('Accpane' + intModuleCount + '_content_lbtn' + intProgCount);
            var hdnTabCount = document.getElementById('hdnTabCount');
            var tabcount = document.getElementById('<%= hdnTabCount.ClientID%>').value;

            var frmTabs = $('#Container ul');
            var iFrames = document.getElementsByTagName('iframe');
            var existTab = -1;

            for (var i = 0; i <= iFrames.length - 1; i++) {
                if (iFrames[i].src.indexOf(curProgram.attributes["ViewPage"].nodeValue) != -1
                    || iFrames[i].src.indexOf(curProgram.attributes["DetailPage"].nodeValue) != -1) {
                    existTab = i;
                }
            }
            if (existTab == -1) {
                closeNav();
                LoadDynamictTabIframe(curProgram.innerText, curProgram.attributes["PageUrl"].nodeValue, tabcount);
            }
            else {
                var cntril = $(parent.document).find('#tab-content div');
                if (cntril.hasClass('active show')) {
                    cntril.removeClass('active show');
                }
                if (cntril.hasClass('active')) {
                    cntril.removeClass('active');
                }
                if (cntril.hasClass('show')) {
                    cntril.removeClass('show');
                }
                if (cntril.hasClass('in')) {
                    cntril.removeClass('in');
                }

                var _num = Number(existTab + 1);
                $(parent.document).find('#tab' + _num).addClass('in active show');
                if ($('li').hasClass('active')) {
                    $('li').removeClass('active');
                }
                $(parent.document).find('#li' + _num).addClass('active');
                //alert('kkk'); alert(existTab);
                //frmTabs.tabs('select', existTab);
                closeNav();
            }

            // document.getElementById('divTabClose').style.display = 'none';

        }


        function RemoveTabBasedOnCount(sTCount) {
            //$('#tab-list').append($('<li><a href="#tab' + tabID + '" role="tab" data-toggle="tab">Tab ' + tabID + '<button class="close" type="button" title="Remove this page">�</button></a></li>'));
            //$('#tab-content').append($('<div class="tab-pane fade" id="tab' + tabID + '">Tab ' + tabID + ' content</div>'));
        }

        function CreateTabForChild(name, value) {
            //   $('#Default').append($('<iframe runat="server" id="ifrmMain1" width="100%" frameborder="0" marginheight="0" marginwidth="0" scrolling="no" style="width: 100%; border: 0; min-height: 420px" visible="true" src="'+value+'"></iframe>"'));


            //    <li><a href="#tab' + tabID + '" role="tab" data-toggle="tab">Tab ' + tabID + '<button class="close" type="button" title="Remove this page">�</button></a></li>'));
            //$('#tab-content').append($('<div class="tab-pane fade" id="tab' + tabID + '">Tab ' + tabID + ' content</div>'));



            //var $tabs2 = $('#Container ul').tabs({
            //    add: function (e, ui) {
            //        // append close thingy
            //        $(ui.tab).parents('li:first')
            //                        .append('<em class="ui-tabs-close" title="Close Tab">x</em>')
            //                         .find('em')
            //                        .click(function () {
            //                            $tabs2.tabs('remove', $('li', $tabs2).index($(this).parents('li:first')[0]));
            //                        });
            //        // select just added tab
            //        $tabs2.tabs('select', '#' + ui.panel.id);
            //    }
            //});
            //$tabs2.tabs('add', '#' + "IFrame" + tab_counter, name, value);
            //tab_counter += 1;
        }

        function fnCloseCurrent(ctrl) {

            if (ctrl == '1')
                fnShowTabCloseMenu();
            else
                document.getElementById('divTabClose').style.display = 'none';

            var frmTabs = $('#Container ul');
            var iFrames = document.getElementsByTagName('iframe');
            var selectedIndex = frmTabs.tabs("option", "selected");
            if (selectedIndex != 0) {
                frmTabs.tabs('remove', selectedIndex);
            }
        }

        function fnShowTabCloseMenu() {
            var disply = document.getElementById('divTabClose').style.display;
            if (disply == 'none') {
                document.getElementById('divTabClose').style.display = 'block';
                document.getElementById('divTabClose').focus();
            }
            else
                document.getElementById('divTabClose').style.display = 'none';
        }

        function escapeMenu(e) {
            if (e.keyCode == 27) {
                fnShowTabCloseMenu();
            }
        }

        function fnCloseAll(ExceptTab) {

            fnShowTabCloseMenu();

            var frmTabs = $('#Container ul');
            var iFrames = document.getElementsByTagName('iframe');
            var selectedIndex = 0;
            var frameID = '-1';
            if (ExceptTab != null) {
                selectedIndex = frmTabs.tabs("option", "selected");
                frameID = iFrames[selectedIndex].id;
            }
            //
            for (var i = 1; i <= iFrames.length - 1; i++) {
                if (iFrames[i].id != frameID) {
                    frmTabs.tabs('select', i);
                    frmTabs.tabs('remove', i);
                    i = i - 1;
                }
            }

        }

        function fnSetCurMenuProgram() {
            //
            var frmTabs = $('#Container ul');
            var selectedIndex = frmTabs.tabs("option", "selected");
            var imgBtns = (document.getElementById('divMenu')).getElementsByTagName('img');
            var lbtns = (document.getElementById('divMenu')).getElementsByTagName('a');
            var existTab = -1;
            var iFrames = document.getElementsByTagName('iframe');
            var iFrame = iFrames[selectedIndex];
            var ddlTheme = document.getElementById('ddlTheme');
            var varTheme = ddlTheme.options(ddlTheme.selectedIndex).value;
            var accIndex = 0;

            document.getElementById('divTabClose').style.display = 'none';

            try {
                if (iFrame != undefined && iFrame.src != null) {
                    for (var i = 0; i <= imgBtns.length - 1; i++) {
                        if (varTheme == 'S3GTHEME_BLUE2' || varTheme == "S3GTHEME_NIGHT" || varTheme == "S3GTHEME_LIGHT" || varTheme == "S3GTHEME_GREEN_LIGHT" || varTheme == "S3GTHEME_BLUE_LIGHT" || varTheme == "S3GTHEME_BLUE_DARK" || varTheme == "S3GTHEME_GREEN_DARK") {

                            if (iFrame.src.indexOf(lbtns[i].attributes["ViewPage"].nodeValue) != -1 ||
                                iFrame.src.indexOf(lbtns[i].attributes["DetailPage"].nodeValue) != -1) {
                                imgBtns[i].src = '../images/Blue_2/menu_arrow_blue.gif';
                                lbtns[i].style.fontWeight = 'bold';
                                accIndex = lbtns[i].id;
                            }
                            else {
                                imgBtns[i].src = '../images/spacer.gif'
                                lbtns[i].style.fontWeight = 'normal';
                            }
                        }
                        else {
                            if (iFrame.src.indexOf(lbtns[i].attributes["ViewPage"].nodeValue) != -1 ||
                                iFrame.src.indexOf(lbtns[i].attributes["DetailPage"].nodeValue) != -1) {
                                lbtns[i].style.fontWeight = 'bold';
                                accIndex = lbtns[i].id.replace('Accpane', '').substring(0, 1)
                            }
                            else {
                                imgBtns[i].src = '../images/bullet_button.jpg'
                                lbtns[i].style.fontWeight = 'normal';
                            }
                        }
                    }

                    //
                    accIndex = accIndex.replace('Accpane', '');
                    accIndex = accIndex.replace('Accpane', '').substring(0, accIndex.indexOf('_'));
                    //This line will be used to focus the selected Programs's menu panel
                    //$get('Accordion1').AccordionBehavior.set_SelectedIndex(accIndex);
                }
            } catch (e) {

            }
        }

        //Code added for handling proper logout - Mouse Click Close
        function ConfirmCloseClick(event) {
            if (ToClose == true || event.clientY < 0) {
                event.returnValue = 'Do you want to logout from application?';
            }
            ToClose = false;
        }


        //Code added for handling proper logout - Alter + F4
        var ToClose;
        function ConfirmCloseF4(event) {
            if (event.altKey && event.keyCode == 115) {
                ToClose = true;
            }
        }

        function ConfirmCloseAltR(event) {
            if (event.altKey && event.keyCode == 82) {
                //alert(document.getElementById('ddlProgram_txtItemName').value);
                document.getElementById('ddlProgram_txtItemName').focus();
                return false;
            }
        }
    </script>

    <script type="text/jscript" language="javascript">

        function shouldCancelbackspace(e) {
            var key;
            if (e) {
                key = e.which ? e.which : e.keyCode;
                if (key == null || (key != 8 && key != 13)) { // return when the key is not backspace key. 
                    return false;
                }
            } else {
                return false;
            }

            if (e.srcElement) { // in IE 
                tag = e.srcElement.tagName.toUpperCase();
                type = e.srcElement.type;
                readOnly = e.srcElement.readOnly;
                if (type == null) { // Type is null means the mouse focus on a non-form field. disable backspace button 
                    return true;
                } else {
                    type = e.srcElement.type.toUpperCase();
                }

            } else { // in FF 
                tag = e.target.nodeName.toUpperCase();
                type = (e.target.type) ? e.target.type.toUpperCase() : "";
            }

            // we don't want to cancel the keypress (ever) if we are in an input/text area 
            if (tag == 'INPUT' || type == 'TEXT' || type == 'TEXTAREA') {
                if (readOnly == true) // if the field has been dsabled, disbale the back space button 
                    return true;
                if (((tag == 'INPUT' && type == 'RADIO') || (tag == 'INPUT' && type == 'CHECKBOX'))
    && (key == 8 || key == 13)) {
                    return true; // the mouse is on the radio button/checkbox, disbale the backspace button 
                }
                return false;
            }

            // if we are not in one of the above things, then we want to cancel (true) if backspace 
            return (key == 8 || key == 13);
        }

        // check the browser type 
        function whichBrs() {
            var agt = navigator.userAgent.toLowerCase();
            if (agt.indexOf("opera") != -1) return 'Opera';
            if (agt.indexOf("staroffice") != -1) return 'Star Office';
            if (agt.indexOf("webtv") != -1) return 'WebTV';
            if (agt.indexOf("beonex") != -1) return 'Beonex';
            if (agt.indexOf("chimera") != -1) return 'Chimera';
            if (agt.indexOf("netpositive") != -1) return 'NetPositive';
            if (agt.indexOf("phoenix") != -1) return 'Phoenix';
            if (agt.indexOf("firefox") != -1) return 'Firefox';
            if (agt.indexOf("safari") != -1) return 'Safari';
            if (agt.indexOf("skipstone") != -1) return 'SkipStone';
            if (agt.indexOf("msie") != -1) return 'Internet Explorer';
            if (agt.indexOf("netscape") != -1) return 'Netscape';
            if (agt.indexOf("mozilla/5.0") != -1) return 'Mozilla';

            if (agt.indexOf('\/') != -1) {
                if (agt.substr(0, agt.indexOf('\/')) != 'mozilla') {
                    return navigator.userAgent.substr(0, agt.indexOf('\/'));
                } else
                    return 'Netscape';
            } else if (agt.indexOf(' ') != -1)
                return navigator.userAgent.substr(0, agt.indexOf(' '));
            else
                return navigator.userAgent;
        }

        // Global events (every key press) 

        var browser = whichBrs();
        if (browser == 'Internet Explorer') {
            document.onkeydown = function () { return !shouldCancelbackspace(event); }
        } else if (browser == 'Firefox') {
            document.onkeypress = function (e) { return !shouldCancelbackspace(e); }
        }
        window.history.forward(-1);
        //window.history.forward();

        function fnSetMenuState() {
            var hdnMenuState = document.getElementById('hdnMenuState');
            if (hdnMenuState.value == '0') {
                hdnMenuState.value = '1';
            }
            else {
                hdnMenuState.value = '0';
            }
        }
    </script>

</body>
</html>
