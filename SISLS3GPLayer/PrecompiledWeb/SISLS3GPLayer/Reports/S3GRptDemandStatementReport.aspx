﻿<%@ page language="C#" autoeventwireup="true" inherits="Reports_S3GRptDemandStatementReport, App_Web_nmps0mjf" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CRVDemandStatement" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="false" EnableParameterPrompt="false"  EnableDrillDown="false" />
     </div>
    </form>
</body>
</html>
