﻿<%@ page language="C#" autoeventwireup="true" inherits="Reports_CollectionPerformance, App_Web_qvacefhr" title="Collection Amount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CRVCollectionPerformanceReport" runat="server" AutoDataBind="true"
            Height="50px" Width="350px" EnableDatabaseLogonPrompt="false" EnableParameterPrompt="false"
            EnableDrillDown="false"  HasToggleGroupTreeButton ="false"    ToolPanelView="None" />
    </div>
    </form>
</body>
</html>
