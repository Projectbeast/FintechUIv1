﻿<%@ page language="C#" autoeventwireup="true" inherits="Reports_S3GPerformanceofAssetReport, App_Web_qvacefhr" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Performance of Asset Report</title>
</head>
<body >
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CRVPOA" runat="server" AutoDataBind="True" Height="50px" Width="350px"
         EnableDatabaseLogonPrompt="false" EnableParameterPrompt="false" EnableDrillDown="false"  />
        
    </div>
    </form>
</body>
</html>