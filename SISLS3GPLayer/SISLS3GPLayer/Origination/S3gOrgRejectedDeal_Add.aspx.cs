#region Page Header
/// © 2018 SUNDARAM INFOTECH . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Rejected Deal 
/// Created By			: Anbuvel T
/// Created Date		: 04-Jul-2018
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Globalization;
using System.Data;
using S3GBusEntity.Origination;
using System.Web.Security;
using System.Configuration;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using ReportAccountsMgtServicesReference;
using S3GBusEntity;
using S3GBusEntity.Reports;

#endregion

public partial class Origination_S3gOrgRejectedDeal_Add : ApplyThemeForProject
{
    #region Variable declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    Dictionary<string, string> Procparam;
    string _Add = "1", _Edit = "2", _Query = "3";
    int _SlNo = 0;
    // bool PaintBG = false;
    int intCompany_Id, intEnqNewCustomerId;
    int intUserId;
    int intResult;
    int intRejectedId;
    static string strMode;
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    DataTable dtAstChk = new DataTable();
    DataTable DtAlertDetails = new DataTable();
    DataTable DtFollowUp = new DataTable();
    DataTable DtCashFlow = new DataTable();
    DataTable DtCashFlowOut = new DataTable();
    DataTable DtRepayGrid = new DataTable();
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bClearList = false;
    double intAssetamount = 0;
    public string strDateFormat;
    public string strCustomer_Id = string.Empty;
    public string strCustomer_Value = string.Empty;
    public string strCustomer_Name = string.Empty;
    public int intProgramId = 555;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=REDEL';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3gOrgRejectedDeal_Add.aspx?qsMode=C';";
    string strRedirectPage = "~/Origination/S3GORGTransLander.aspx?Code=REDEL";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    PricingMgtServicesReference.PricingMgtServicesClient ObjPricingMgtServices;
    PricingMgtServices.S3G_ORG_PricingDataTable ObjS3G_ORG_Pricing;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;
    string strPageName = "Rejected Deal";
    ReportDocument rpd = new ReportDocument();
    public static Origination_S3gOrgRejectedDeal_Add obj_PageValue;
    ReportAccountsMgtServicesClient objSerClient;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_PageValue = this;
            ObjUserInfo = new UserInfo();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString.Get("qsViewId") != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                intRejectedId = Convert.ToInt32(fromTicket.Name);
            }
            obj_PageValue.intCompany_Id = ObjUserInfo.ProCompanyIdRW;
            intCompany_Id = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
           
            if (!IsPostBack)
            {
                txtNewFollowup.Attributes.Add("onblur", "fnDoDate(this,'" + txtNewFollowup.ClientID + "','" + strDateFormat + "',true,  false);");
                calNewFollowup.Format = strDateFormat;
                BindLOB();
                PopulateBranchList();
                FunPriLoadCredit();
            }
        }        
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);            
        }
    }
    protected void ddlLob_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //FunPriLoadLocation(intCompany_Id, intUserId, intProgramId, Convert.ToInt32(ddlLob.SelectedValue));
            ShowHideColumnName();
            funPriLoadProduct();
            ddlLob.Focus();
            PopulateBranchList();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }

    }

    private void ShowHideColumnName()
    {
        if (ddlLob.SelectedItem.Text.Trim().ToUpper().Contains("NON"))
        {
            lblCreditPurpose.Text = lblCreditPurpose.ToolTip = "Sector";
            lblLeadSourceType.Text = lblCreditPurpose.ToolTip = "Facility Type";
            lblLeadSourceName.Text = lblCreditPurpose.ToolTip = "Risk Clasification";
            lblBussinessSource.Text = lblCreditPurpose.ToolTip = "Bussiness Source";
            lblRCustomerNID.Text = lblRCustomerNID.ToolTip = "CR / ID No.";
            lblRDealerName.Text = lblRDealerName.ToolTip = "Dealer Name";
            lblAssetDesc.Text = lblAssetDesc.ToolTip = "Asset Desc";
            EnableDisableControls(true);
            pnlRetailDetail.GroupingText = pnlRetailDetail.ToolTip = "Corporate Details";
        }
        else
        {
            pnlRetailDetail.GroupingText = pnlRetailDetail.ToolTip = "Retail Details";
            lblCreditPurpose.Text = lblCreditPurpose.ToolTip = "Credit Purpose";           
            lblLeadSourceType.Text = lblCreditPurpose.ToolTip = "Lead Source Type";            
            lblLeadSourceName.Text = lblCreditPurpose.ToolTip = "Lead Source Name";          
            lblBussinessSource.Text = lblCreditPurpose.ToolTip = "Bussiness Source";   
            lblRCustomerNID.Text = lblRCustomerNID.ToolTip = "Customer N.ID.";            
            lblRDealerName.Text = lblRDealerName.ToolTip = "Dealer Name";
            lblAssetDesc.Text = lblAssetDesc.ToolTip = "Asset Desc";
            EnableDisableControls(false);
        }
    }

    private void EnableDisableControls(Boolean isVAlid)
    {
        lblYield.Visible = txtYield.Visible = lblTransactionValue.Visible = txtTransactionValue.Visible = lblRejectedBy.Visible = txtRejectedBy.Visible =
        lblNewFollowup.Visible = txtNewFollowup.Visible = lblMarketingOfficer.Visible=txtMarketingOfficer.Visible=lblAssetType.Visible=aceAssetType.Visible=imgInstrumentDate.Visible= isVAlid;
    }

    private void BindLOB()
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        Procparam.Add("@OPTION", "3");
        Procparam.Add("@COMPANYID", Convert.ToString(intCompany_Id));
        Procparam.Add("@USERID", Convert.ToString(intUserId));
        Procparam.Add("@PROGRAMID", "110");
        ddlLob.BindDataTable("S3G_CLN_LOADLOV", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
    }

    private void funPriLoadProduct()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Company_ID", intCompany_Id.ToString());
            Procparam.Add("@LOB_ID", ddlLob.SelectedValue);
            DataTable dt = Utility.GetDefaultData(SPNames.SYS_ProductMaster, Procparam);
            Utility.FillDataTable(ddlProduct, dt, "Product_ID", "Product_Name");
            if (dt.Rows.Count > 0)
            {

                DataRow[] dr = dt.Select("Product_Code in('HPN','HPV','HPH')");
                if (dr.Length > 0)
                {
                    ddlProduct.SelectedValue = dr.CopyToDataTable().Rows[0]["Product_ID"].ToString();
                    //ddlProduct_SelectedIndexChanged(null, null);
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void btnExit_OnClick(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect(strRedirectPage);
            
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }

    }

    #region TO Get Branch List
    private void PopulateBranchList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompany_Id));
            Procparam.Add("@User_ID", Convert.ToString(UserId));
            Procparam.Add("@Program_ID", "555");
            Procparam.Add("@OPTION", "1");            
            if (PageMode == PageModes.Create)
                Procparam.Add("@Is_Active", "1");
            ddlBranch.ClearSelection();
            ddlBranch.BindDataTable("S3G_GET_LOCATION", Procparam, new string[] { "BRANCH_ID", "BRANCH" });
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    private void FunPriLoadCredit() // To do: here I want to use the SQL Lookup table once the sql  table design get over.
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
                       
            Procparam.Add("@Company_ID", Convert.ToString(intCompany_Id));
            Procparam.Add("@Lookup_Type", "CREDIT_PURPOSE");
            Procparam.Add("@Table_Name", "S3G_ORG_LOOKUP");
            ddlCreditPurpose.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, true, "--Select--", new string[] { "ID", "DESCRIPTION" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCreditPurposeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@COMPANY_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("SA_GET_CUSTOMER_AGT", Procparam));

        return suggetions.ToArray();

    }
}