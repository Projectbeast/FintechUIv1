﻿<%@ page language="C#" autoeventwireup="true" inherits="Reports_S3GRptDemandCollectionChart, App_Web_zznul5le" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Demand Collection Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
           <CR:CrystalReportViewer ID="CRVDemandCollectionChart" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="false" EnableDrillDown="false" Height="50px" Width="350px"/>
 
    </div>
    </form>
</body>
</html>
