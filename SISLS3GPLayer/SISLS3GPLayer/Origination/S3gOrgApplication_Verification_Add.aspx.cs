#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Application Verification-MFC Change
/// Created By			: Sathish R--008181
/// Created Date		: 01-Nov-2019
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

public partial class Origination_S3gOrgPricing_Add : ApplyThemeForProject
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
    int intPricingId;
    string strProgramName = string.Empty;
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

    int intApplicationProcessId = 0;


    double intAssetamount = 0;
    public string strDateFormat;
    public string strCustomer_Id = string.Empty;
    public string strCustomer_Value = string.Empty;
    public string strCustomer_Name = string.Empty;
    public int intProgramId = 42;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgPricing_Add.aspx?qsMode=C';";

    //string strRedirectPage = "~/Origination/S3GORGTransLander.aspx?Code=APPVER&Create=1&Modify=1";
    string strRedirectPage = "window.location.href='../Common/HomePage.aspx';";

    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    PricingMgtServicesReference.PricingMgtServicesClient ObjPricingMgtServices;
    PricingMgtServices.S3G_ORG_PricingDataTable ObjS3G_ORG_Pricing;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;
    //string strNewWin = "window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&NewCustomerID=0', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
    //string strNewWin_Repay = "window.showModalDialog('../Origination/S3GORGRepaymentDetails_Add.aspx?IsFrom=Pricing', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;";
    string strPageName = "Pricing";
    ReportDocument rpd = new ReportDocument();
    public static Origination_S3gOrgPricing_Add obj_PageValue;
    ReportAccountsMgtServicesClient objSerClient;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;

    //string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=APPP&Create=1&Modify=1';";

    string strRedirectPageView = "window.location.href='../Common/HomePage.aspx';";
    ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjAProcessSave;
    int intResult;
    #endregion

    #region Page Load
    private void funPriSetProgramName()
    {
        try
        {
            Dictionary<string, string> strProparm = new Dictionary<string, string>();
            strProparm.Add("@Program_ID", "585");
            DataTable dtProgram = Utility.GetDefaultData("S3G_GET_PROGRAM_NAME", strProparm);
            if (dtProgram.Rows.Count > 0)
            {
                strProgramName = dtProgram.Rows[0]["NAME"].ToString();

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // WF Implementation
            ProgramCode = "042";
            obj_PageValue = this;
            ObjUserInfo = new UserInfo();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];

            if (Request.QueryString.Get("qsViewId") != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                intPricingId = Convert.ToInt32(fromTicket.Name);
            }
            obj_PageValue.intCompany_Id = ObjUserInfo.ProCompanyIdRW;
            intCompany_Id = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            HttpContext.Current.Session["Company_Id"] = intCompany_Id;

            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            strDateFormat = ObjS3GSession.ProDateFormatRW;



            if (!IsPostBack)
            {
                if (!chkVerificationComplted.Checked)
                {
                    chkVerificationComplted.Checked = true;
                }
                funPriSetProgramName();
                FormsAuthenticationTicket fromTicket;
                if (Request.QueryString["qsViewId"] != null)
                {
                    fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                    if (fromTicket != null)
                    {
                        intApplicationProcessId = Convert.ToInt32(fromTicket.Name);
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Invalid Application Details");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }

                    if (intApplicationProcessId == 0)
                    {

                        FunPriDisableControls(0);
                    }
                    else if (intApplicationProcessId > 0)
                    {
                        if (strMode == "M")
                        {
                            FunPriDisableControls(1);
                        }
                        else if (strMode == "Q")
                        {
                            FunPriDisableControls(-1);
                        }
                    }
                }

            }

    #endregion
        }
        catch (Exception ex)
        {

        }
    }

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode
                    lblHeading.Text = strProgramName + " - Create"; //FunPubGetPageTitles(enumPageTitle.Create);
                    Label lblUserVal = (Label)Page.Master.FindControl("lblPageName");

                    if (!bCreate)
                    {
                        //btnSave.Enabled = false;
                        btnSave.Enabled_False();

                    }

                    break;

                case 1: // Modify Mode
                    lblHeading.Text = strProgramName + " - Modify"; //FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bModify)
                    {
                        //btnSave.Enabled = false;
                        btnSave.Enabled_False();

                    }
                    ddlApplicationNo.SelectedValue = intApplicationProcessId.ToString();
                    btnGo_Click(null, null);


                    break;


                case -1:// Query Mode
                    //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    lblHeading.Text = strProgramName + " - View";
                    //btnSave.Enabled = false;
                    btnSave.Enabled_False();
                    ddlApplicationNo.SelectedValue = intApplicationProcessId.ToString();
                    btnGo_Click(null, null);
                    ddlApplicationNo.Enabled = false;
                    btnGo.Enabled_False();



                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetProposalFromCheckList(String prefixText, int count)
    {

        List<String> suggetions = new List<String>();
        Dictionary<string, string> strProParm = new Dictionary<string, string>();
        strProParm.Add("@OPTION", "18");
        if (HttpContext.Current.Session["Company_Id"] != null)
            strProParm.Add("@COMPANYID", HttpContext.Current.Session["Company_Id"].ToString());
        if (HttpContext.Current.Session["User_Id"] != null)
            strProParm.Add("@USERID", HttpContext.Current.Session["User_Id"].ToString());
        strProParm.Add("@PROGRAMID", "38");
        strProParm.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_OR_LOAD_LOV_APP", strProParm));
        return suggetions.ToArray();



        return suggetions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetAccuntNoList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@COMPANY_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("SA_GET_PROPOSAL_AGT", Procparam));

        return suggetions.ToArray();
    }
    protected void grvAssetDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblChkVerified = e.Row.FindControl("lblChkVerified") as Label;
            Label lblChkVerificationResultsNotinOrder = e.Row.FindControl("lblChkVerificationResultsNotinOrder") as Label;

            CheckBox ChkVerified = e.Row.FindControl("ChkVerified") as CheckBox;
            CheckBox ChkVerificationResultsNotinOrder = e.Row.FindControl("ChkVerificationResultsNotinOrder") as CheckBox;

            TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;

            if (lblChkVerified.Text == "1")
            {
                ChkVerified.Checked = true;
            }
            else
            {
                ChkVerified.Checked = false;
            }

            if (lblChkVerificationResultsNotinOrder.Text == "1")
            {
                ChkVerificationResultsNotinOrder.Checked = true;
            }
            else
            {
                ChkVerificationResultsNotinOrder.Checked = false;
            }

            if (strMode == "Q")
            {
                ChkVerified.Enabled = false;
                ChkVerificationResultsNotinOrder.Enabled = false;
                txtRemarks.Enabled = false;
            }



        }
    }
    private Int32 FunPriTypeCast(string val)
    {
        try
        {
            // Modified : Rajendran 
            int returnValue = 0;
            decimal decX = 0;
            //Added by Kali K on 15-DEC-2010 for date time display based on GPS
            DateTime tempdatetime;
            if (DateTime.TryParse(val, out tempdatetime))
                return 2;
            if (val is int)
                returnValue = 1;
            if (Decimal.TryParse(val, out decX))
                returnValue = 1;
            ////else if (val is DateTime)
            ////    returnValue = 2;
            else if (val is string)
                returnValue = 3;
            return returnValue;
        }
        catch (Exception)
        {
            return 0;
        }
    }
    protected void grvApplicantDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblChkVerified = e.Row.FindControl("lblChkVerified") as Label;
            Label lblChkVerificationResultsNotinOrder = e.Row.FindControl("lblChkVerificationResultsNotinOrder") as Label;

            CheckBox ChkVerified = e.Row.FindControl("ChkVerified") as CheckBox;
            CheckBox ChkVerificationResultsNotinOrder = e.Row.FindControl("ChkVerificationResultsNotinOrder") as CheckBox;
            TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
            TextBox lblVaue = e.Row.FindControl("lblVaue") as TextBox;

            Label lblDataType = e.Row.FindControl("lblDataType") as Label;

            if (lblChkVerified.Text == "1")
            {
                ChkVerified.Checked = true;
            }
            else
            {
                ChkVerified.Checked = false;
            }

            if (lblChkVerificationResultsNotinOrder.Text == "1")
            {
                ChkVerificationResultsNotinOrder.Checked = true;
            }
            else
            {
                ChkVerificationResultsNotinOrder.Checked = false;
            }

            if (strMode == "Q")
            {
                ChkVerified.Enabled = false;
                ChkVerificationResultsNotinOrder.Enabled = false;
                txtRemarks.Enabled = false;
            }

            if (lblVaue.Text == string.Empty)
            {
                ChkVerified.Enabled = false;
                ChkVerificationResultsNotinOrder.Enabled = false;
                txtRemarks.Enabled = false;
            }
            if (lblDataType.Text == "2")
            {
                lblVaue.Style["text-align"] = "right";
            }
            else
            {
                lblVaue.Style["text-align"] = "left";
            }

        }


    }



    protected void grvApplicantDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void grvApplicantDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void grvApplicantDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void grvApplicantDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

    }
    protected void grvApplicantDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    private void funPriLoadApplicationVerificationdtls()
    {
        try
        {

            if (ddlApplicationNo.SelectedValue == "" || ddlApplicationNo.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Select the Proposal Number");
                ddlApplicationNo.Focus();
                return;
            }

            DataSet dtVerDetail = new DataSet();
            Dictionary<string, string> strProparm = new Dictionary<string, string>();
            strProparm.Add("@Company_Id", intCompany_Id.ToString());
            strProparm.Add("@USER_ID", intUserId.ToString());
            strProparm.Add("@Program_Id", "585");
            strProparm.Add("@OPTION", "1");
            strProparm.Add("@Application_No", ddlApplicationNo.SelectedValue);
            strProparm.Add("@Page_Mode", strMode);
            dtVerDetail = Utility.GetDataset("S3G_ORG_GET_APPLNVERDTL", strProparm);


            ViewState["ASSETDETAILSPARM"] = dtVerDetail.Tables[2]; ;
            ViewState["ASSETDETAILS"] = dtVerDetail.Tables[3];

            funPriUpdateApplicationDetils(dtVerDetail.Tables[0]);
            funPriUpdateAssetDetails();

            if (dtVerDetail.Tables[0].Rows.Count > 0)
            {
                grvApplicantDetails.DataSource = dtVerDetail.Tables[0];
                grvApplicantDetails.DataBind();
                cpeDemo.AutoExpand = true;
                Utility.FunShowAlertMsg(this, "Verification Details Loaded successfully Expand and Update the Details ");

            }
            else
            {
                Utility.FunShowAlertMsg(this, "Data Combination(Product or Line of Business or Constitution) not avilable for the selected Proposal ");
                cpeDemo.AutoExpand = false;
                return;
            }
            ddlApplicationNo.SelectedText = dtVerDetail.Tables[1].Rows[0]["APPLICATION_NO"].ToString();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void funPriUpdateAssetDetails()
    {
        DataTable ASSETDETAILS = (DataTable)ViewState["ASSETDETAILS"];
        DataTable ASSETDETAILSPARM = (DataTable)ViewState["ASSETDETAILSPARM"];
        if (ASSETDETAILS.Rows.Count > 0)
        {
            foreach (DataRow drASSETDETAILS in ASSETDETAILS.Rows)
            {

                DataRow[] dr2 = ASSETDETAILSPARM.Select("ASSET_ID='" + drASSETDETAILS["ASSET_ID"] + "'");

                foreach (DataRow dr in dr2)
                {
                    dr["Value"] = drASSETDETAILS[dr["VARIFICATION_FIELD_REF"].ToString()].ToString();
                }
            }
        }
        ASSETDETAILSPARM.AcceptChanges();
        //grvAssetDetails.DataSource = ASSETDETAILSPARM;
        //grvAssetDetails.DataBind();
        ViewState["ASSETPARM"] = ASSETDETAILSPARM;
    }


    private void funPriUpdateApplicationDetils(DataTable dtVerDetail)
    {

        try
        {
            string strSPName = "S3G_OR_Get_AppProcDet_Ver";
            Dictionary<string, string> procparam = new Dictionary<string, string>();
            procparam.Add("@ApplicationProcessId", ddlApplicationNo.SelectedValue);
            procparam.Add("@COMPANYID", intCompany_Id.ToString());
            DataSet dsApplicationProcessDetails = Utility.GetDataset(strSPName, procparam);

            DataTable dtAppHeader = dsApplicationProcessDetails.Tables[0];

            DataTable dtAssetDetails = dsApplicationProcessDetails.Tables[12];
            ViewState["dtAssetDetails"] = dtAssetDetails;

            if (dsApplicationProcessDetails.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["LOBID"] = dsApplicationProcessDetails.Tables[0].Rows[0]["LOB_ID"].ToString();
            }

            foreach (DataRow dr in dtVerDetail.Rows)
            {
                if (dtAppHeader.Columns.Contains(dr["VARIFICATION_FIELD_REF"].ToString()))
                {
                    dr["Value"] = dtAppHeader.Rows[0][dr["VARIFICATION_FIELD_REF"].ToString()].ToString();
                }
            }

            dtVerDetail.AcceptChanges();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void ddlApplicationNo_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            string strURL;

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlApplicationNo.SelectedValue, false, 0);
            strURL = "../Origination/S3G_ORG_ApplicationProcessing.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";
            lnkViewAccountDetails.Attributes.Add("onclick", "window.open('" + strURL + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')");

            if (ddlApplicationNo.SelectedValue != "0")
            {
                ddlApplicationNo.Enabled = false;
            }

            grvAssetDetails.DataSource = null;
            grvAssetDetails.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void funPriLoadApplicationVerificationdtlsAsset()
    {

        if (ddlAssetCode.SelectedValue == "0" || ddlAssetCode.SelectedValue == string.Empty)
        {
            Utility.FunShowAlertMsg(this, "Select the Asset Code");
            return;

        }
        DataTable dtAssetDetailsParmDetails = (DataTable)ViewState["ASSETPARM"];
        if (dtAssetDetailsParmDetails.Rows.Count > 0)
        {
            DataRow[] drAssetVer = dtAssetDetailsParmDetails.Select("ASSET_ID='" + ddlAssetCode.SelectedValue + "'");
            if (drAssetVer.Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Asset Details not available");
                return;
            }
            grvAssetDetails.DataSource = drAssetVer.CopyToDataTable();
            grvAssetDetails.DataBind();

        }

    }

    private void funPriModifyAssetDetails(DataTable dtAssetModify)
    {
        DataTable dtAssetUpdate = (DataTable)ViewState["dtAssetDetails"];

        foreach (DataRow dr in dtAssetModify.Rows)
        {
            if (dtAssetUpdate.Columns.Contains(dr["VARIFICATION_FIELD_REF"].ToString()))
            {
                dr["Value"] = dtAssetUpdate.Rows[0][dr["VARIFICATION_FIELD_REF"].ToString()].ToString();
            }
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAsset(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@COMPANYID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Contract_Type", "3");
        Procparam.Add("@Poposal_No_Id", obj_PageValue.ddlApplicationNo.SelectedValue);
        Procparam.Add("@LOB_ID", HttpContext.Current.Session["LOBID"].ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_OR_AST_VER_LOV", Procparam), true);
        return suggetions.ToArray();

    }
    protected void rbtnApplicantName_CheckedChanged(object sender, EventArgs e)
    {
        funPriLoadApplicationVerificationdtls();
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            funPriLoadApplicationVerificationdtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    protected void btnGoAsset_Click(object sender, EventArgs e)
    {
        funPriLoadApplicationVerificationdtlsAsset();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool bVerCheck = true;

        foreach (GridViewRow grv in grvApplicantDetails.Rows)
        {
            CheckBox ChkVerified = grv.FindControl("ChkVerified") as CheckBox;
            if (ChkVerified.Checked == false)
            {
                bVerCheck = false;
            }
        }
        if (ddlApplicationNo.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this, "Select the Proposal Number");
            return;
        }
        //if (bVerCheck == false)
        //{
        //    Utility.FunShowAlertMsg(this,"Verify the Items");
        //    return;
        //}
        bool bVerCheckRInorder = true;

        foreach (GridViewRow grv in grvApplicantDetails.Rows)
        {
            CheckBox ChkVerificationResultsNotinOrder = grv.FindControl("ChkVerificationResultsNotinOrder") as CheckBox;
            if (ChkVerificationResultsNotinOrder.Checked == false)
            {
                bVerCheckRInorder = false;
            }
        }

        //if (bVerCheckRInorder == false)
        //{
        //    Utility.FunShowAlertMsg(this, "Verify the Result in Order");
        //    return;
        //}

        string strApp_Number = string.Empty;
        string strWFMsg = "";
        ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjAProcessSave = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
        S3GBusEntity.ApplicationProcess.ApplicationProcess ObjBusApplicationProcess = new S3GBusEntity.ApplicationProcess.ApplicationProcess();
        ObjBusApplicationProcess.Application_Process_ID = Convert.ToInt32(ddlApplicationNo.SelectedValue);
        ObjBusApplicationProcess.Created_By = intUserId;
        ObjBusApplicationProcess.XMLRepaymentStructure = grvApplicantDetails.FunPubFormXml();
        ObjBusApplicationProcess.XMLRepayDetailsOthers = ((DataTable)ViewState["ASSETPARM"]).FunPubFormXml();


        if (chkVerificationComplted.Checked)
        {
            ObjBusApplicationProcess.Stage_Status = 3;//Verification Completed
            ObjBusApplicationProcess.Rootback = 1;//
        }
        else
        {
            ObjBusApplicationProcess.Stage_Status = 2;//Verification Completed
        }
        if (ChkDataEntryRouteback.Checked)
        {
            ObjBusApplicationProcess.Rootback = 2;//
            ObjBusApplicationProcess.Stage_Status = 2;//Data Entry Pending
        }

        intResult = ObjAProcessSave.FunPubCreateApplicationProcessIntVer(out strApp_Number,ObjBusApplicationProcess);
        if (intResult == 0)
        {
            btnSave.Enabled_False();


            if (Request.QueryString["Popup"] != null)
            {
                Utility.FunShowAlertMsg(this, "Application Verification Details Saved successfully");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
            }
            else
            {
                strKey = "Modify";
                strAlert = strAlert.Replace("__ALERT__", "Application Verification Details Saved successfully");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);

            }
        }
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlApplicationNo.Clear();
            ddlApplicationNo.Enabled = true;
            ddlAssetCode.Clear();
            ddlAssetCode.Enabled = true;
            grvApplicantDetails.DataSource = null;
            grvApplicantDetails.DataBind();
            grvAssetDetails.DataSource = null;
            grvAssetDetails.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnCalcel_Click(object sender, EventArgs e)
    {

    }
    protected void btnApplicationCancel_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClosePage();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void FunPriClosePage()
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
            }
            else
            {
                Response.Redirect("~/Common/HomePage.aspx");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnUpdateAsset_Click(object sender, EventArgs e)
    {
        DataTable dtASSETPARM = (DataTable)ViewState["ASSETPARM"];
        DataRow[] dr = dtASSETPARM.Select("ASSET_ID='" + ddlAssetCode.SelectedValue + "'");
        if (dr.Length > 0)
        {

            foreach (GridViewRow gr in grvAssetDetails.Rows)
            {
                CheckBox ChkVerified = gr.FindControl("ChkVerified") as CheckBox;
                TextBox lblVaue = gr.FindControl("lblVaue") as TextBox;

                if (lblVaue.Text != string.Empty)
                {
                    if (!ChkVerified.Checked)
                    {

                        Utility.FunShowAlertMsg(this, "Data not Verified for available Fields");
                        return;
                    }
                }

            }

            foreach (GridViewRow gr in grvAssetDetails.Rows)
            {
                Label lbllblVerDetId = gr.FindControl("lblVerDetId") as Label;
                CheckBox ChkVerified = gr.FindControl("ChkVerified") as CheckBox;
                CheckBox ChkVerificationResultsNotinOrder = gr.FindControl("ChkVerificationResultsNotinOrder") as CheckBox;

                TextBox txtRemarks = gr.FindControl("txtRemarks") as TextBox;
                DataRow[] dr3 = dtASSETPARM.Select("ASSET_ID='" + ddlAssetCode.SelectedValue + "' and app_verify_extn_det_id='" + lbllblVerDetId.Text + "'");
                if (ChkVerified.Checked)
                    dr3[0]["VIO"] = "1";
                else
                {
                    dr3[0]["VIO"] = "0";
                }

                if (ChkVerificationResultsNotinOrder.Checked)
                    dr3[0]["VNIO"] = "1";
                else
                {
                    dr3[0]["VNIO"] = "0";
                }

                if (txtRemarks.Text != string.Empty)
                {
                    dr3[0]["RM"] = txtRemarks.Text;
                }

            }
        }
        dtASSETPARM.AcceptChanges();
        grvAssetDetails.DataSource = dr.CopyToDataTable();
        grvAssetDetails.DataBind();
        Utility.FunShowAlertMsg(this, "Asset Information Updated Successfully");
        ViewState["ASSETPARM"] = dtASSETPARM;

    }
    private void funPriLoadAssetTable()
    {
        DataTable dtAssetUpdate = new DataTable();
        dtAssetUpdate.Columns.Add("APPLICATION_PROCESS_ID", typeof(int));
        dtAssetUpdate.Columns.Add("APPLICATION_PROCESS_ASSET_ID", typeof(int));
        dtAssetUpdate.Columns.Add("APP_Verify_Extn_Det_ID", typeof(int));
        dtAssetUpdate.Columns.Add("Is_Varified", typeof(int));
        dtAssetUpdate.Columns.Add("Is_In_Order", typeof(int));
        dtAssetUpdate.Columns.Add("Remarks", typeof(string));
        ViewState["AssetTable"] = dtAssetUpdate;

    }
    protected void chkRiskRootback_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void ChkDataEntryRouteback_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkDataEntryRouteback.Checked)
        {
            chkVerificationComplted.Checked = false;
        }
    }
    protected void chkVerificationComplted_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVerificationComplted.Checked)
        {
            ChkDataEntryRouteback.Checked = false;
        }

    }
    protected void lnkViewAccountDetails_Click(object sender, EventArgs e)
    {
        string strURL = string.Empty;
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlApplicationNo.SelectedValue, false, 0);
        strURL = "../Origination/S3G_ORG_ApplicationProcessing.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + strURL + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
        return;
    }

    protected void chkAll_CheckedChangedVerBase(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)grvApplicantDetails.HeaderRow.FindControl("chkAll");
        

        for (int i = 0; i < grvApplicantDetails.Rows.Count; i++)
        {
            TextBox lblVaue = (TextBox)grvApplicantDetails.Rows[i].FindControl("lblVaue");
            CheckBox ChkVerified = (CheckBox)grvApplicantDetails.Rows[i].FindControl("ChkVerified");
            if (lblVaue.Text != string.Empty)
            {
               
                ChkVerified.Checked = chk.Checked;
            }

          
        }

    }
    protected void chkAllInOrder_CheckedChangedVerBase(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)grvApplicantDetails.HeaderRow.FindControl("chkAllInOrder");


        for (int i = 0; i < grvApplicantDetails.Rows.Count; i++)
        {
            TextBox lblVaue = (TextBox)grvApplicantDetails.Rows[i].FindControl("lblVaue");
            CheckBox ChkVerified = (CheckBox)grvApplicantDetails.Rows[i].FindControl("ChkVerificationResultsNotinOrder");
            if (lblVaue.Text != string.Empty)
            {

                ChkVerified.Checked = chk.Checked;
            }


        }

    }


    //Asset 
   
    protected void chkAllAsset_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)grvAssetDetails.HeaderRow.FindControl("chkAllAsset");


        for (int i = 0; i < grvAssetDetails.Rows.Count; i++)
        {
            TextBox lblVaue = (TextBox)grvAssetDetails.Rows[i].FindControl("lblVaue");
            CheckBox ChkVerified = (CheckBox)grvAssetDetails.Rows[i].FindControl("ChkVerified");
            if (lblVaue.Text != string.Empty)
            {

                ChkVerified.Checked = chk.Checked;
            }


        }

    }
    protected void chkAllAssetInOrder_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)grvAssetDetails.HeaderRow.FindControl("chkAllAssetInOrder");


        for (int i = 0; i < grvAssetDetails.Rows.Count; i++)
        {
            TextBox lblVaue = (TextBox)grvAssetDetails.Rows[i].FindControl("lblVaue");
            CheckBox ChkVerified = (CheckBox)grvAssetDetails.Rows[i].FindControl("ChkVerificationResultsNotinOrder");
            if (lblVaue.Text != string.Empty)
            {

                ChkVerified.Checked = chk.Checked;
            }


        }
    }
    protected void ChkVerificationResultsNotinOrder_CheckedChanged(object sender, EventArgs e)
    {
        //CheckBox ChkVerificationResultsNotinOrder = (CheckBox)sender;
        //CheckBox chk = (CheckBox)grvAssetDetails.HeaderRow.FindControl("chkAllAssetInOrder");
        //chk.Checked = ChkVerificationResultsNotinOrder.Checked;

    }
}