<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SessionExpired.aspx.cs" Inherits="SessionExpired" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html lang="en">
<head runat="server">
    <title>Session Expired - SmartLend</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">

    <!-- Local CSS Files Only -->
    <link href="Content/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Content/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="Content/css/style.css" rel="stylesheet" type="text/css" />
    <link href="Content/css/custom.css" rel="stylesheet" type="text/css" />
    <link href="Content/css/font.css" rel="stylesheet" type="text/css" />

    <!-- Local Font Face Declarations -->
    <style>
        @font-face {
            font-family: 'FunnelSans';
            src: url('Content/fonts/funnel_sans/FunnelSans-Regular.ttf') format('truetype');
            font-weight: 400;
            font-style: normal;
        }

        @font-face {
            font-family: 'FunnelSans';
            src: url('Content/fonts/funnel_sans/FunnelSans-Medium.ttf') format('truetype');
            font-weight: 500;
            font-style: normal;
        }

        @font-face {
            font-family: 'FunnelSans';
            src: url('Content/fonts/funnel_sans/FunnelSans-SemiBold.ttf') format('truetype');
            font-weight: 600;
            font-style: normal;
        }

        @font-face {
            font-family: 'FunnelSans';
            src: url('Content/fonts/funnel_sans/FunnelSans-Bold.ttf') format('truetype');
            font-weight: 700;
            font-style: normal;
        }
    </style>

    <!-- Session Expired Custom Styles -->
    <style>
        :root {
            --s3g-primary: #004695;
            --s3g-secondary: #0066cc;
            --s3g-accent: #2196f3;
            --s3g-light: #e3f2fd;
            --s3g-dark: #1a237e;
            --gradient-start: #667eea;
            --gradient-middle: #764ba2;
            --gradient-end: #4facfe;
            --modern-purple: #6366f1;
            --modern-blue: #3b82f6;
            --text-primary: #1e293b;
            --text-secondary: #64748b;
            --success: #10b981;
            --warning: #f59e0b;
            --shadow-sm: 0 1px 3px 0 rgba(102, 126, 234, 0.1);
            --shadow-md: 0 4px 6px -1px rgba(102, 126, 234, 0.15);
            --shadow-lg: 0 10px 15px -3px rgba(102, 126, 234, 0.2);
            --shadow-xl: 0 20px 25px -5px rgba(102, 126, 234, 0.25);
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'FunnelSans', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, var(--gradient-start) 0%, var(--gradient-middle) 50%, var(--gradient-end) 100%);
            min-height: 100vh;
            margin: 0;
            padding: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            overflow: hidden;
        }

            body::before {
                content: '';
                position: fixed;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: url('data:image/svg+xml,<svg width="80" height="80" viewBox="0 0 80 80" xmlns="http://www.w3.org/2000/svg"><g fill="none" fill-rule="evenodd"><g fill="%23ffffff" fill-opacity="0.04"><circle cx="40" cy="40" r="2"/></g></g></svg>') repeat;
                z-index: -1;
            }

        .session-expired-container {
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(20px);
            border-radius: 24px;
            padding: 2.5rem;
            box-shadow: var(--shadow-xl);
            border: 1px solid rgba(255, 255, 255, 0.3);
            text-align: center;
            max-width: 480px;
            width: 90%;
            position: relative;
            overflow: visible;
        }

        .logo-container {
            margin-bottom: 1.5rem;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .company-logo {
            max-width: 180px;
            height: auto;
            filter: drop-shadow(0 4px 8px rgba(0, 0, 0, 0.1));
        }

        .icon-container {
            width: 80px;
            height: 80px;
            background: linear-gradient(135deg, var(--s3g-light), var(--s3g-accent), var(--modern-blue));
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 1.8rem auto;
            box-shadow: var(--shadow-lg);
            position: relative;
            animation: pulse 2s infinite;
        }

            .icon-container::after {
                content: '';
                position: absolute;
                width: 100px;
                height: 100px;
                border: 2px solid rgba(102, 126, 234, 0.3);
                border-radius: 50%;
                animation: ripple 2s infinite;
            }

        @keyframes pulse {
            0%, 100% {
                transform: scale(1);
            }

            50% {
                transform: scale(1.05);
            }
        }

        @keyframes ripple {
            0% {
                transform: scale(1);
                opacity: 1;
            }

            100% {
                transform: scale(1.3);
                opacity: 0;
            }
        }

        .session-icon {
            font-size: 2.2rem;
            color: var(--s3g-primary);
            filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.15));
        }

        .session-title {
            font-size: 1.8rem;
            font-weight: 700;
            color: var(--text-primary);
            margin-bottom: 1rem;
            line-height: 1.2;
            background: linear-gradient(135deg, var(--s3g-primary), var(--modern-purple));
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
        }

        .session-subtitle {
            font-size: 1.1rem;
            color: var(--text-secondary);
            margin-bottom: 2rem;
            line-height: 1.5;
            font-weight: 400;
        }

        .icon-container::after {
            content: '';
            position: absolute;
            width: 120px;
            height: 120px;
            border: 2px solid rgba(251, 191, 36, 0.3);
            border-radius: 50%;
            animation: ripple 2s infinite;
        }

        @keyframes pulse {
            0%, 100% {
                transform: scale(1);
            }

            50% {
                transform: scale(1.05);
            }
        }

        @keyframes ripple {
            0% {
                transform: scale(1);
                opacity: 1;
            }

            100% {
                transform: scale(1.4);
                opacity: 0;
            }
        }

        .session-icon {
            font-size: 3rem;
            color: #f59e0b;
            filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.1));
        }

        .session-title {
            font-size: 2rem;
            font-weight: 700;
            color: var(--dark-color);
            margin-bottom: 1rem;
            line-height: 1.2;
        }

        .session-subtitle {
            font-size: 1.1rem;
            color: #64748b;
            margin-bottom: 2rem;
            line-height: 1.5;
            font-weight: 400;
        }

        .login-button {
            background: linear-gradient(135deg, var(--s3g-primary), var(--modern-purple), var(--modern-blue));
            color: white;
            padding: 1rem 2.5rem;
            border-radius: 50px;
            text-decoration: none;
            font-weight: 600;
            font-size: 1.1rem;
            display: inline-flex;
            align-items: center;
            gap: 0.75rem;
            transition: all 0.3s ease;
            box-shadow: var(--shadow-md);
            border: none;
            cursor: pointer;
            position: relative;
            overflow: hidden;
        }

            .login-button::before {
                content: '';
                position: absolute;
                top: 0;
                left: -100%;
                width: 100%;
                height: 100%;
                background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
                transition: left 0.5s;
            }

            .login-button:hover::before {
                left: 100%;
            }

            .login-button:hover {
                transform: translateY(-3px);
                box-shadow: var(--shadow-xl);
                text-decoration: none;
                color: white;
            }

            .login-button:active {
                transform: translateY(0);
            }

        .security-info {
            margin-top: 2rem;
            padding: 1.5rem;
            background: linear-gradient(135deg, rgba(227, 242, 253, 0.8), rgba(219, 234, 254, 0.6));
            border-radius: 16px;
            border: 1px solid rgba(102, 126, 234, 0.2);
        }

        .security-info-title {
            font-size: 1rem;
            font-weight: 600;
            color: var(--s3g-primary);
            margin-bottom: 0.75rem;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.5rem;
        }

        .security-info-text {
            font-size: 0.9rem;
            color: var(--text-secondary);
            line-height: 1.5;
        }

        .floating-shapes {
            position: absolute;
            width: 100%;
            height: 100%;
            overflow: hidden;
            pointer-events: none;
        }

        .shape {
            position: absolute;
            opacity: 0.06;
            animation: float 10s ease-in-out infinite;
        }

        .shape-1 {
            top: 10%;
            left: 5%;
            animation-delay: 0s;
        }

        .shape-2 {
            top: 70%;
            right: 8%;
            animation-delay: 4s;
        }

        .shape-3 {
            bottom: 10%;
            left: 10%;
            animation-delay: 8s;
        }

        @keyframes float {
            0%, 100% {
                transform: translateY(0px) rotate(0deg);
            }

            50% {
                transform: translateY(-25px) rotate(180deg);
            }
        }

        /* Desktop and Laptop - No scrollbars */
        @media (min-width: 1025px) {
            body {
                overflow: hidden;
                padding: 0;
            }

            .session-expired-container {
                max-width: 500px;
                padding: 3rem;
            }

            .company-logo {
                max-width: 200px;
            }

            .session-title {
                font-size: 2rem;
            }

            .session-subtitle {
                font-size: 1.2rem;
            }

            .icon-container {
                width: 90px;
                height: 90px;
            }

            .session-icon {
                font-size: 2.5rem;
            }

            .login-button {
                padding: 1.2rem 3rem;
                font-size: 1.2rem;
            }

            .security-info {
                padding: 2rem;
            }

            .security-info-title {
                font-size: 1.1rem;
            }

            .security-info-text {
                font-size: 1rem;
            }
        }

        /* Tablet - Allow scrolling when needed */
        @media (max-width: 1024px) and (min-width: 769px) {
            body {
                overflow-x: hidden;
                overflow-y: auto;
                padding: 20px 10px;
            }

            .session-expired-container {
                max-width: 420px;
                padding: 2rem;
            }

            .company-logo {
                max-width: 160px;
            }

            .session-title {
                font-size: 1.6rem;
            }

            .session-subtitle {
                font-size: 1rem;
            }

            .icon-container {
                width: 70px;
                height: 70px;
            }

            .session-icon {
                font-size: 2rem;
            }

            .login-button {
                padding: 0.9rem 2rem;
                font-size: 1rem;
            }
        }

        /* Mobile - Allow scrolling when needed */
        @media (max-width: 768px) {
            body {
                padding: 15px 5px;
                overflow-x: hidden;
                overflow-y: auto;
            }

            .session-expired-container {
                padding: 1.5rem;
                max-width: 360px;
                border-radius: 20px;
            }

            .logo-container {
                margin-bottom: 1.2rem;
            }

            .company-logo {
                max-width: 140px;
            }

            .session-title {
                font-size: 1.4rem;
                margin-bottom: 0.8rem;
            }

            .session-subtitle {
                font-size: 0.9rem;
                margin-bottom: 1.5rem;
            }

            .login-button {
                padding: 0.8rem 1.8rem;
                font-size: 0.95rem;
            }

            .icon-container {
                width: 60px;
                height: 60px;
                margin-bottom: 1.2rem;
            }

            .session-icon {
                font-size: 1.8rem;
            }

            .security-info {
                margin-top: 1.5rem;
                padding: 1.2rem;
            }

            .security-info-title {
                font-size: 0.9rem;
                margin-bottom: 0.6rem;
            }

            .security-info-text {
                font-size: 0.8rem;
            }

            .floating-shapes .shape {
                opacity: 0.04;
            }
        }

        @media (max-width: 480px) {
            body {
                padding: 10px 3px;
            }

            .session-expired-container {
                padding: 1.2rem;
                max-width: 300px;
            }

            .session-title {
                font-size: 1.2rem;
            }

            .session-subtitle {
                font-size: 0.85rem;
            }

            .login-button {
                padding: 0.7rem 1.5rem;
                font-size: 0.9rem;
            }

            .company-logo {
                max-width: 120px;
            }

            .icon-container {
                width: 50px;
                height: 50px;
            }

            .session-icon {
                font-size: 1.6rem;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>

        <!-- Modern Session Expired Design -->
        <div class="floating-shapes">
            <div class="shape shape-1">
                <i class="fa fa-shield" style="font-size: 70px; color: var(--s3g-primary);"></i>
            </div>
            <div class="shape shape-2">
                <i class="fa fa-clock-o" style="font-size: 50px; color: var(--modern-purple);"></i>
            </div>
            <div class="shape shape-3">
                <i class="fa fa-lock" style="font-size: 60px; color: var(--modern-blue);"></i>
            </div>
        </div>

        <div class="session-expired-container">
            <div class="icon-container">
                <i class="fa fa-hourglass-end session-icon"></i>
            </div>

            <h1 class="session-title">Session Expired</h1>
            <p class="session-subtitle">
                Your session has timed out for security reasons. Please log in again to continue using the application.
            </p>

            <a href="LoginPage.aspx" target="_parent" class="login-button">
                <i class="fa fa-sign-in"></i>
                <span>Login Again</span>
            </a>

            <div class="security-info">
                <div class="security-info-title">
                    <i class="fa fa-shield"></i>
                    <span>Security Information</span>
                </div>
                <p class="security-info-text">
                    Sessions expire automatically to protect your account and ensure your financial data remains secure even if you forget to log out.
                </p>
            </div>
        </div>

        <!-- Hidden legacy panel for backward compatibility -->
        <div style="display: none;">
            <cc1:DropShadowExtender ID="dseProspect" runat="server" TargetControlID="pnlProspectView"
                Opacity=".3" Rounded="false" TrackPosition="true" />
            <asp:Panel ID="pnlProspectView" runat="server" Style="display: none;"
                Width="350px" BackColor="White" BorderWidth="1px" BorderColor="#e1e6e9">
            </asp:Panel>
        </div>
    </form>

    <!-- Local JavaScript functionality -->
    <script type="text/javascript">
        // Optional: Auto-redirect to login after a certain time
        // setTimeout(function() {
        //     window.location.href = 'LoginPage.aspx';
        // }, 10000); // 10 seconds
        
        // Add smooth animations
        document.addEventListener('DOMContentLoaded', function() {
            const container = document.querySelector('.session-expired-container');
            if (container) {
                container.style.opacity = '0';
                container.style.transform = 'translateY(30px)';
                
                setTimeout(function() {
                    container.style.transition = 'all 0.6s ease-out';
                    container.style.opacity = '1';
                    container.style.transform = 'translateY(0)';
                }, 100);
            }
        });
    </script>
</body>
</html>
