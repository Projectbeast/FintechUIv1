using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ORG = S3GBusEntity.Origination;
using ORGSERVICE = OrgMasterMgtServicesReference;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using S3GBusEntity;
using System.Web.Security;

public partial class Origination_S3GOrgComplainceMaster_Add : ApplyThemeForProject
{
    #region [Intialization]

    Dictionary<string, string> dictParam = null;
    Dictionary<string, string> Procparam = null;

    int intComplainceID = 0;
    int intComplainceTypeID = 0;
    int intCollectionID;
    int intClosureID;
    int intDedupeID;
    int intOtherComplainceID;
    int intSMEID;
    int intUserId;
    int intUserLevelID;
    int intCompanyId;
    int intprogramId = 259;
    int intErrCode;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    string strErrorMsg;
    string strMode;
    string strKey = "Insert";
    string strPageName = "Origination - Compliance Master";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgComplainceMaster_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgComplainceMaster_View.aspx';";


    ORG.OrgMasterMgtServices.S3GOrgComplainceMaster_AddDataTable ObjComplainceMasterDataTable;
    ORGSERVICE.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    public string strDateFormat;
    S3GSession objSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();

    DataSet dsCollectionDtls = new DataSet();

    #endregion [Intialization]

    protected void Page_Load(object sender, EventArgs e)
    {
        FunProPageLoad();
    }
    protected void FunProPageLoad()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                              // to get the standard date format of the Application
            CalendarExtenderEffectiveFromDate.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtenderEffectiveToDate.Format = strDateFormat;

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            intUserLevelID = ObjUserInfo.ProUserLevelIdRW;
            intCollectionID = Convert.ToInt32(hdnCollectionApp.Value);
            intClosureID = Convert.ToInt32(hdnClosureApp.Value);

            if (Request.QueryString["qsComplainceId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsComplainceId"));

                if (fromTicket != null)
                {
                    intComplainceID = Convert.ToInt32(fromTicket.Name.Split('-')[0]);
                    intComplainceTypeID = Convert.ToInt32(fromTicket.Name.Split('-')[1]);
                    ViewState["ComplainceId"] = intComplainceID;
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Compliance Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }
            if (!IsPostBack)
            {
                FunBind_Effective_To();
                txtSmallMinTurnOver.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Small SME Min Turn Over");
                txtSmallMaxTurnOver.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Small SME Max Turn Over");
                txtMicroMinTurnOver.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Micro SME  Min Turn Over");
                txtMicroMaxTurnOver.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Micro SME Max Turn Over");
                txtMediumMinTurnOver.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Medium SME Min Turn Over");
                txtMediumMaxTurnOver.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Medium SME Max Turn Over");


                txtMinSmallAssetValue.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Small SME Min Total Asset Value");
                txtMaxSmallAssetValue.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Small SME Max Total Asset Value");
                txtMinMediumAssetValue.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Medium SME Min Total Asset Value");
                txtMaxMediumAssetValue.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Medium SME Max Total Asset Value");
                txtMinMicroAssetValue.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Micro SME Min Total Asset Value");
                txtMaxMicroAssetValue.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Micro SME Max Total Asset Value");

                //txtSmallMinTurnOver.SetDecimalPrefixSuffix(9, 3, true, "Min Turn Over");
                //txtSmallMaxTurnOver.SetDecimalPrefixSuffix(9, 3, true, "Max Turn Over");
                //txtMicroMinTurnOver.SetDecimalPrefixSuffix(9, 3, true, "Min Turn Over");
                //txtMicroMaxTurnOver.SetDecimalPrefixSuffix(9, 3, true, "Max Turn Over");
                //txtMediumMinTurnOver.SetDecimalPrefixSuffix(9, 3, true, "Min Turn Over");
                //txtMediumMaxTurnOver.SetDecimalPrefixSuffix(9, 3, true, "Max Turn Over");

                //txtMinSmallAssetValue.SetDecimalPrefixSuffix(9, 3, true, "Min Total Asset Value");
                //txtMaxSmallAssetValue.SetDecimalPrefixSuffix(9, 3, true, "Max Total Asset Value");
                //txtMinMediumAssetValue.SetDecimalPrefixSuffix(9, 3, true, "Min Total Asset Value");
                //txtMaxMediumAssetValue.SetDecimalPrefixSuffix(9, 3, true, "Max Total Asset Value");
                //txtMinMicroAssetValue.SetDecimalPrefixSuffix(9, 3, true, "Min Total Asset Value");
                //txtMaxMicroAssetValue.SetDecimalPrefixSuffix(9, 3, true, "Max Total Asset Value");

                FunPriLoadCommonData();
                Panel1.Visible = false;
                PnlOtherComplaince.Visible = false;
                pnlDedupe.Visible = false;
                PnlClosure.Visible = false;
                pnlCollectionDtls.Visible = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                FunPubGetDesignationDetails();
                FunPubGetLocation();
                FunPubClosureDesignationDetails();
                ddlComplainceType.SelectedIndex = 1;
                ddlComplainceType.SelectedValue = "1";
                ddlComplainceType_SelectedIndexChanged(null, null);
                if (strMode == "M")
                {
                    FunPriDisableControls(0);
                }
                else if (strMode == "Q")
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(1);
                }
                ddlComplainceType.Focus();
                txtEffectiveFromDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtEffectiveFromDate.ClientID + "','" + strDateFormat + "',false,  true);");
                txtEffectiveToDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtEffectiveToDate.ClientID + "','" + strDateFormat + "',false,  true);");
                //TextBox txtFTAmount = (TextBox)grvCollectionDtls.FooterRow.FindControl("txtFTAmount");
                //txtFTAmount.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Amount");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    public void FunPubGetComplainceDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@COMPANY_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@COMPMAST_ID", intComplainceID.ToString());
            dictParam.Add("@Complaince_Type_ID", intComplainceTypeID.ToString());
            DataSet dsComplaince = Utility.GetDataset("S3G_ORG_GET_COMPLAINCEDTLS", dictParam);

            if (intComplainceTypeID == 1)
            {
                Panel1.Visible = false;
                PnlOtherComplaince.Visible = false;
                pnlDedupe.Visible = false;
                PnlClosure.Visible = false;
                pnlCollectionDtls.Visible = true;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                ddlComplainceType.SelectedValue = intComplainceTypeID.ToString();
                ddlComplainceType.ClearDropDownList();
                txtEffectiveFromDate.Text = dsComplaince.Tables[0].Rows[0]["Effective_From"].ToString();
                txtEffectiveToDate.Text = dsComplaince.Tables[0].Rows[0]["Effective_To"].ToString();
                if (strMode == "M" || strMode == "Q")
                {
                    chkHActive.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Is_Active"]));
                }
                if (dsComplaince.Tables[0].Rows.Count > 0)
                {
                    ViewState["CollectionDetails"] = dsComplaince.Tables[0];
                    ViewState["dtTempCollectionApprover"] = dsComplaince.Tables[1];

                    hdnCollectionApp.Value = Convert.ToString(dsComplaince.Tables[0].Rows[0]["Coll_Compliance_ID"]);
                    grvCollectionDtls.DataSource = dsComplaince.Tables[0];
                    grvCollectionDtls.DataBind();
                    grvCollectionDtls.FooterRow.Visible = false;
                    grvApprover.DataSource = dsComplaince.Tables[1];
                    grvApprover.DataBind();
                }
                else
                {
                    FunPriInitializeCollectionApproval();
                }

            }
            else if (intComplainceTypeID == 2)
            {
                Panel1.Visible = false;
                PnlOtherComplaince.Visible = false;
                pnlDedupe.Visible = false;
                PnlClosure.Visible = true;
                pnlCollectionDtls.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                txtEffectiveFromDate.ReadOnly = true;
                txtEffectiveToDate.ReadOnly = true;
                grvClosureDtls.Visible = true;
                ddlComplainceType.SelectedValue = intComplainceTypeID.ToString();
                ddlComplainceType.ClearDropDownList();
                //  tcComplainceMaster.ActiveTabIndex = 1;
                txtEffectiveFromDate.Text = dsComplaince.Tables[0].Rows[0]["Effective_From"].ToString();
                txtEffectiveToDate.Text = dsComplaince.Tables[0].Rows[0]["Effective_To"].ToString();
                if (strMode == "M" || strMode == "Q")
                {

                    chkHActive.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Is_Active"]));

                }
                if (dsComplaince.Tables[0].Rows.Count > 0)
                {
                    ViewState["ClosureDetails"] = dsComplaince.Tables[0];
                    ViewState["dtTempClosureApprover"] = dsComplaince.Tables[2];
                    grvClosureDtls.DataSource = dsComplaince.Tables[0];
                    grvClosureDtls.DataBind();
                    grvClosureDtls.FooterRow.Visible = false;
                    grvDEVApprover.DataSource = dsComplaince.Tables[2];
                    grvDEVApprover.DataBind();
                }
                else
                {
                    FunPriInitializeClosureApproval();
                }


            }
            else if (intComplainceTypeID == 3)
            {
                Panel1.Visible = false;
                PnlOtherComplaince.Visible = false;
                pnlDedupe.Visible = true;
                PnlClosure.Visible = false;
                pnlCollectionDtls.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                txtEffectiveFromDate.Text = dsComplaince.Tables[0].Rows[0]["Effective_From"].ToString();
                txtEffectiveToDate.Text = dsComplaince.Tables[0].Rows[0]["Effective_To"].ToString();

                chkName.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Dedupe_Name"]));
                chkDOB.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Dedupe_DOB"]));
                chkCRNo.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Dedupe_CRNO"]));
                chkPassport.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Dedupe_Passport"]));
                chkNID.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Dedupe_NID"]));
                chkNRID.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Dedupe_NRID"]));
                chkLabourCard.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Dedupe_Labourcard"]));
                chkThirdPartyCheck.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Third_Party_Check"]));
                ddlComplainceType.SelectedValue = intComplainceTypeID.ToString();
                ddlComplainceType.ClearDropDownList();

                if (strMode == "M" || strMode == "Q")
                {
                    chkHActive.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Is_Active"]));
                }
            }
            else if (intComplainceTypeID == 4)
            {
                //TabOtherComplaince.Visible = true;
                //TabCollection.Visible = false;
                //TabClosure.Visible = false;
                //TabDedupe.Visible = false;
                //TabSME.Visible = false;
                Panel1.Visible = false;
                PnlOtherComplaince.Visible = true;
                pnlDedupe.Visible = false;
                PnlClosure.Visible = false;
                pnlCollectionDtls.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                txtMaxAgeBorrower.Text = dsComplaince.Tables[0].Rows[0]["Age_Limit"].ToString();
                txtEffectiveFromDate.Text = dsComplaince.Tables[0].Rows[0]["Effective_From"].ToString();
                txtEffectiveToDate.Text = dsComplaince.Tables[0].Rows[0]["Effective_To"].ToString();
                txtEffectiveFromDate.ReadOnly = true;
                txtEffectiveToDate.ReadOnly = true;
                txtMaxAgeBorrower.ReadOnly = true;
                ddlComplainceType.SelectedValue = intComplainceTypeID.ToString();
                ddlComplainceType.ClearDropDownList();
                //tcComplainceMaster.ActiveTabIndex = 3;
                if (strMode == "M" || strMode == "Q")
                {
                    chkHActive.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[0].Rows[0]["Is_Active"]));
                }
            }
            else if (intComplainceTypeID == 5)
            {
                //TabSME.Visible = true;
                //TabCollection.Visible = false;
                //TabClosure.Visible = false;
                //TabDedupe.Visible = false;
                //TabOtherComplaince.Visible = false;
                Panel1.Visible = true;
                PnlOtherComplaince.Visible = false;
                pnlDedupe.Visible = false;
                PnlClosure.Visible = false;
                pnlCollectionDtls.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                //   tcComplainceMaster.ActiveTabIndex = 4;
                ddlComplainceType.SelectedValue = intComplainceTypeID.ToString();
                ddlComplainceType.ClearDropDownList();
                txtEffectiveFromDate.Text = dsComplaince.Tables[3].Rows[0]["Effective_From"].ToString();
                txtEffectiveToDate.Text = dsComplaince.Tables[3].Rows[0]["Effective_To"].ToString();

                //FOR SMALL SME SIZE
                txtSmallMinEmpSize.Text = dsComplaince.Tables[3].Rows[0]["Min_Employee_Size"].ToString();
                txtSmallMinTurnOver.Text = dsComplaince.Tables[3].Rows[0]["Min_Turnover"].ToString();
                txtMinSmallAssetValue.Text = dsComplaince.Tables[3].Rows[0]["Min_Asset_value"].ToString();
                txtSmallMaxEmpSize.Text = dsComplaince.Tables[3].Rows[0]["Max_Employee_Size"].ToString();
                txtSmallMaxTurnOver.Text = dsComplaince.Tables[3].Rows[0]["Max_Turnover"].ToString();
                txtMaxSmallAssetValue.Text = dsComplaince.Tables[3].Rows[0]["Max_Asset_value"].ToString();
                //FOR MEDIUM SME SIZE
                txtMediumMinEmpSize.Text = dsComplaince.Tables[4].Rows[0]["Min_Employee_Size"].ToString();
                txtMediumMinTurnOver.Text = dsComplaince.Tables[4].Rows[0]["Min_Turnover"].ToString(); ;
                txtMinMediumAssetValue.Text = dsComplaince.Tables[4].Rows[0]["Min_Asset_value"].ToString();
                txtMediumMaxEmpSize.Text = dsComplaince.Tables[4].Rows[0]["Max_Employee_Size"].ToString();
                txtMediumMaxTurnOver.Text = dsComplaince.Tables[4].Rows[0]["Max_Turnover"].ToString();
                txtMaxMediumAssetValue.Text = dsComplaince.Tables[4].Rows[0]["Max_Asset_value"].ToString();
                //FOR MICRO SME SIZE
                txtMicroMinEmpSize.Text = dsComplaince.Tables[5].Rows[0]["Min_Employee_Size"].ToString();
                txtMicroMinTurnOver.Text = dsComplaince.Tables[5].Rows[0]["Min_Turnover"].ToString();
                txtMinMicroAssetValue.Text = dsComplaince.Tables[5].Rows[0]["Min_Asset_value"].ToString();
                txtMicroMaxEmpSize.Text = dsComplaince.Tables[5].Rows[0]["Max_Employee_Size"].ToString();
                txtMicroMaxTurnOver.Text = dsComplaince.Tables[5].Rows[0]["Max_Turnover"].ToString();
                txtMaxMicroAssetValue.Text = dsComplaince.Tables[5].Rows[0]["Max_Asset_value"].ToString();
                if (strMode == "M" || strMode == "Q")
                {
                    chkHActive.Checked = Convert.ToBoolean(Convert.ToInt32(dsComplaince.Tables[5].Rows[0]["Is_Active"]));
                }
            }
            else
            {
                grvCollectionDtls.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriDisableControls(int intModeID)
    {
        try
        {

            switch (intModeID)
            {
                case 0: // Modify Mode   
                    chkHActive.Enabled = true;
                    FunPubGetComplainceDetails();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    //btnCancel.Enabled = true;
                    btnCancel.Enabled_True();
                    btnSave.Enabled_True();
                    //btnSave.Enabled = true;
                    txtEffectiveFromDate.ReadOnly = true;
                    txtEffectiveToDate.ReadOnly = true;
                    txtSmallMinEmpSize.ReadOnly = true;
                    txtSmallMinTurnOver.ReadOnly = true;
                    txtMinSmallAssetValue.ReadOnly = true;
                    txtSmallMaxEmpSize.ReadOnly = true;
                    txtSmallMaxTurnOver.ReadOnly = true;
                    txtMaxSmallAssetValue.ReadOnly = true;
                    txtMediumMinEmpSize.ReadOnly = true;
                    txtMediumMinTurnOver.ReadOnly = true;
                    txtMinMediumAssetValue.ReadOnly = true;
                    txtMediumMaxEmpSize.ReadOnly = true;
                    txtMediumMaxTurnOver.ReadOnly = true;
                    txtMaxMediumAssetValue.ReadOnly = true;
                    txtMicroMinEmpSize.ReadOnly = true;
                    txtMicroMinTurnOver.ReadOnly = true;
                    txtMinMicroAssetValue.ReadOnly = true;
                    txtMicroMaxEmpSize.ReadOnly = true;
                    txtMicroMaxTurnOver.ReadOnly = true;
                    txtMaxMicroAssetValue.ReadOnly = true;
                    chkName.Enabled = false;
                    chkDOB.Enabled = false;
                    chkCRNo.Enabled = false;
                    chkLabourCard.Enabled = false;
                    chkNID.Enabled = false;
                    chkNRID.Enabled = false;
                    chkPassport.Enabled = false;
                    chkThirdPartyCheck.Enabled = false;
                    grvClosureDtls.Columns[grvClosureDtls.Columns.Count - 1].Visible = false;
                    grvCollectionDtls.Columns[grvCollectionDtls.Columns.Count - 1].Visible = false;
                    //   btnModalAdd.Enabled = false;
                    btnModalAdd.Enabled_False();
                    btnClear.Enabled_False();
                    //  btnDEVModalAdd.Enabled = false;
                    btnDEVModalAdd.Enabled_False();
                    CalendarExtenderEffectiveFromDate.Enabled = false;
                    CalendarExtenderEffectiveToDate.Enabled = false;
                    break;

                case -1:// Query Mode
                    chkHActive.Enabled = false;
                    FunPubGetComplainceDetails();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    //btnCancel.Enabled = true;
                    btnCancel.Enabled_True();
                    //btnSave.Enabled = false;
                    btnSave.Enabled_False();
                    btnClear.Enabled_False();
                    //   btnModalAdd.Enabled = false;
                    btnModalAdd.Enabled_False();
                    //  btnDEVModalAdd.Enabled = false;
                    btnDEVModalAdd.Enabled_False();
                    txtEffectiveFromDate.ReadOnly = true;
                    txtEffectiveToDate.ReadOnly = true;
                    txtSmallMinEmpSize.ReadOnly = true;
                    txtSmallMinTurnOver.ReadOnly = true;
                    txtMinSmallAssetValue.ReadOnly = true;
                    txtSmallMaxEmpSize.ReadOnly = true;
                    txtSmallMaxTurnOver.ReadOnly = true;
                    txtMaxSmallAssetValue.ReadOnly = true;
                    txtMediumMinEmpSize.ReadOnly = true;
                    txtMediumMinTurnOver.ReadOnly = true;
                    txtMinMediumAssetValue.ReadOnly = true;
                    txtMediumMaxEmpSize.ReadOnly = true;
                    txtMediumMaxTurnOver.ReadOnly = true;
                    txtMaxMediumAssetValue.ReadOnly = true;
                    txtMicroMinEmpSize.ReadOnly = true;
                    txtMicroMinTurnOver.ReadOnly = true;
                    txtMinMicroAssetValue.ReadOnly = true;
                    txtMicroMaxEmpSize.ReadOnly = true;
                    txtMicroMaxTurnOver.ReadOnly = true;
                    txtMaxMicroAssetValue.ReadOnly = true;
                    chkName.Enabled = false;
                    chkDOB.Enabled = false;
                    chkCRNo.Enabled = false;
                    chkLabourCard.Enabled = false;
                    chkNID.Enabled = false;
                    chkNRID.Enabled = false;
                    chkPassport.Enabled = false;
                    chkThirdPartyCheck.Enabled = false;
                    grvClosureDtls.Columns[grvClosureDtls.Columns.Count - 1].Visible = false;
                    grvCollectionDtls.Columns[grvCollectionDtls.Columns.Count - 1].Visible = false;
                    //   btnModalAdd.Enabled = false;
                    btnModalAdd.Enabled_False();
                    CalendarExtenderEffectiveFromDate.Enabled = false;
                    CalendarExtenderEffectiveToDate.Enabled = false;
                    break;
                case 1:
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriInitializeClosureApproval()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SNo", typeof(string));
            dt.Columns.Add("ClosureID", typeof(string));
            dt.Columns.Add("Down_Payment", typeof(string));
            dt.Columns.Add("Month_Of_Closure", typeof(decimal));
            dt.Columns.Add("No_of_Approval", typeof(int));
            dt.Columns.Add("Active", typeof(bool));
            dt.Columns.Add("Is_Delete", typeof(int));
            dr = dt.NewRow();
            dr["SNo"] = 0;
            dr["ClosureID"] = 0;
            dr["Down_Payment"] = 0;
            dr["Month_Of_Closure"] = 0;
            dr["No_of_Approval"] = 0;
            dr["Active"] = true;
            dr["Is_Delete"] = 0;
            dt.Rows.Add(dr);
            ViewState["ClosureDetails"] = dt;
            grvClosureDtls.DataSource = dt;
            grvClosureDtls.DataBind();
            dt.Rows[0].Delete();
            grvClosureDtls.Rows[0].Visible = false;
        }
        catch (Exception objException)
        {
            throw new ApplicationException("Unable to initiate the row");
        }
    }

    private void FunPriInitializeCollectionApproval()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SNo", typeof(string));
            dt.Columns.Add("CollectionID", typeof(string));
            dt.Columns.Add("COLLTYPE", typeof(string));
            dt.Columns.Add("Type_ID", typeof(string));
            dt.Columns.Add("NO_OF_DAYS", typeof(decimal));
            dt.Columns.Add("RECEIVED_AMT", typeof(decimal));
            dt.Columns.Add("NO_OF_APPROVAL", typeof(int));
            dt.Columns.Add("Active", typeof(bool));
            dt.Columns.Add("Is_Delete", typeof(int));
            dr = dt.NewRow();

            dr["SNo"] = 0;
            dr["CollectionID"] = string.Empty;
            dr["COLLTYPE"] = string.Empty;
            dr["Type_ID"] = string.Empty;
            dr["NO_OF_DAYS"] = 0;
            dr["RECEIVED_AMT"] = 0;
            dr["NO_OF_APPROVAL"] = 0;
            dr["Active"] = true;
            dr["Is_Delete"] = 0;
            dt.Rows.Add(dr);
            ViewState["CollectionDetails"] = dt;
            grvCollectionDtls.DataSource = dt;
            grvCollectionDtls.DataBind();
            dt.Rows[0].Delete();

            grvCollectionDtls.Rows[0].Visible = false;
            FunPriLoadCollection();
        }
        catch (Exception objException)
        {
            throw new ApplicationException("Unable to initiate the row");
        }
    }
    protected void btnFApproverDesig_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            Button btn = (Button)sender;
            GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
            TextBox txtFApprovalOrder = (TextBox)gvRow.FindControl("txtFApprovalOrder");
            string strSLNo = "";
            Label lblSNo = (Label)grvCollectionDtls.Rows[grvCollectionDtls.Rows.Count - 1].FindControl("lblSNo");
            strSLNo = (Convert.ToInt32(lblSNo.Text) + 1).ToString();

            if (ViewState["dtTempCollectionApprover"] == null)
            {
                ViewState["dtTempCollectionApprover"] = FunProInitializePopup();
            }
            DataTable dt1 = (DataTable)ViewState["dtTempCollectionApprover"];
            DataTable dtApprove = new DataTable();
            DataRow[] dRow = dt1.Select("SNo ='" + strSLNo + "'");
            if (dRow.Length > 0)
            {
                dtApprove = dt1.Clone();
                dRow.CopyToDataTable(dtApprove, LoadOption.OverwriteChanges);
            }

            if (dtApprove != null && dtApprove.Rows.Count > 0)
            {
                int intNoofApproval = Convert.ToInt32(txtFApprovalOrder.Text);
                int intRowCount = dtApprove.Rows.Count;

                if (intNoofApproval > intRowCount)
                {
                    for (int inti = intRowCount + 1; inti <= (intRowCount + (intNoofApproval - intRowCount)); inti++)
                    {
                        DataRow dr;
                        dr = dtApprove.NewRow();
                        dr["SNo"] = strSLNo;
                        dr["SequenceNumber"] = inti;
                        dr["ApprovEntity"] = string.Empty;
                        dr["Location"] = string.Empty;
                        dr["ApprovPerson"] = string.Empty;
                        dtApprove.Rows.Add(dr);
                    }
                }

                if (intNoofApproval < intRowCount)
                {
                    for (int inti = 1; inti <= intRowCount - intNoofApproval; inti++)
                    {
                        dtApprove.Rows[dtApprove.Rows.Count - 1].Delete();
                        dtApprove.AcceptChanges();
                    }
                }

                grvApprover.DataSource = dtApprove;
                grvApprover.DataBind();
            }
            else
            {
                dtApprove = FunProInitializePopup();
                int intNoofApproval = Convert.ToInt32(txtFApprovalOrder.Text);
                for (int inti = 1; inti <= intNoofApproval; inti++)
                {
                    DataRow dr;
                    dr = dtApprove.NewRow();
                    dr["SNo"] = strSLNo;
                    dr["SequenceNumber"] = inti;
                    dr["ApprovEntity"] = string.Empty;
                    dr["Location"] = string.Empty;
                    dr["ApprovPerson"] = string.Empty;
                    dtApprove.Rows.Add(dr);
                }

                grvApprover.DataSource = dtApprove;
                grvApprover.DataBind();

                dt1.Merge(dtApprove);

                ViewState["dtTempCollectionApprover"] = dt1;
            }
            ModalPopupExtenderApprover.Show();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected DataTable FunProInitializePopup()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns.Add("SequenceNumber");
        dt.Columns.Add("ApprovEntity");
        dt.Columns.Add("Location");
        dt.Columns.Add("ApprovPerson");

        return dt;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        FunPubSaveCAPMaster();
        // btnSave.Focus();
    }
    private void FunPubSaveCAPMaster()
    {
        try
        {
            if (strMode == "C" || strMode == null)
            {
                if (txtEffectiveFromDate.Text != string.Empty && txtEffectiveToDate.Text != string.Empty)
                {
                    if (Utility.StringToDate(txtEffectiveFromDate.Text) > Utility.StringToDate(txtEffectiveToDate.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Effective From date should be less than or equal to  Effective To date");
                        txtEffectiveToDate.Focus();
                        txtEffectiveToDate.Text = string.Empty;
                        //txtEffectiveToDate.Focus();
                        return;
                    }
                }
            }

            if (ddlComplainceType.SelectedValue == "3")
            {
                if (chkName.Checked == false && chkNID.Checked == false && chkNRID.Checked == false && chkPassport.Checked == false &&
                   chkThirdPartyCheck.Checked == false && chkCRNo.Checked == false && chkDOB.Checked == false && chkLabourCard.Checked == false)
                {
                    Utility.FunShowAlertMsg(this, "Select atleast one De Dupe details");
                    return;
                }
            }
            if (ddlComplainceType.SelectedValue == "1")
            {
                if (((DataTable)ViewState["CollectionDetails"]).Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one Collection Approval details");
                    //  tcComplainceMaster.ActiveTabIndex = 0;
                    return;
                }

            }
            if (ddlComplainceType.SelectedValue == "2")
            {
                if (((DataTable)ViewState["ClosureDetails"]).Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one Closure Approval details");
                    // tcComplainceMaster.ActiveTabIndex = 1;
                    return;
                }
            }
            if (ddlComplainceType.SelectedValue == "5")
            {
                if ((Convert.ToDecimal(txtSmallMinEmpSize.Text)) > (Convert.ToDecimal(txtSmallMaxEmpSize.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Small SME Min Employee Size should not be greater than Small SME Max Employee Size");
                    txtSmallMinEmpSize.Focus();
                    txtSmallMinEmpSize.Text = "";
                    return;
                }
                if ((Convert.ToDecimal(txtSmallMinTurnOver.Text)) > (Convert.ToDecimal(txtSmallMaxTurnOver.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Small SME Min Turn Over should not be greater than Small SME Max Turn Over ");
                    txtSmallMinTurnOver.Focus();
                    txtSmallMinTurnOver.Text = "";
                    return;
                }
                if ((Convert.ToDecimal(txtMinSmallAssetValue.Text)) > (Convert.ToDecimal(txtMaxSmallAssetValue.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Small SME  Min Total Asset Value should not be greater than Small SME Max Total Asset Value ");
                    txtMinSmallAssetValue.Focus();
                    txtMinSmallAssetValue.Text = "";
                    return;
                }
                if ((Convert.ToDecimal(txtMicroMinEmpSize.Text)) > (Convert.ToDecimal(txtMicroMaxEmpSize.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Micro SME Min Employee size should not be greater than Micro SME Max Employee size");
                    txtMicroMinEmpSize.Focus();
                    txtMicroMinEmpSize.Text = "";
                    return;
                }
                if ((Convert.ToDecimal(txtMicroMinTurnOver.Text)) > (Convert.ToDecimal(txtMicroMaxTurnOver.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Micro SME Min Turn Over should not be greater than Micro SME Max Turn Over");
                    txtMicroMinTurnOver.Focus();
                    txtMicroMinTurnOver.Text = "";
                    return;
                }
                if ((Convert.ToDecimal(txtMinMicroAssetValue.Text)) > (Convert.ToDecimal(txtMaxMicroAssetValue.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Micro SME Min Total Asset Value should not be greater than Micro SME Max Total Asset Value");
                    txtMinMicroAssetValue.Focus();
                    txtMinMicroAssetValue.Text = "";
                    return;
                }
                if ((Convert.ToDecimal(txtMediumMinEmpSize.Text)) > (Convert.ToDecimal(txtMediumMaxEmpSize.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Medium SME Min Employee Size should not be greater than Medium SME Max Employee Size");
                    txtMediumMinEmpSize.Focus();
                    txtMediumMinEmpSize.Text = "";
                    return;
                }
                if ((Convert.ToDecimal(txtMediumMinTurnOver.Text)) > (Convert.ToDecimal(txtMediumMaxTurnOver.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Medium SME Min Turn Over should not be greater than Medium SME Max Turn Over");
                    txtMediumMinTurnOver.Focus();
                    txtMediumMinTurnOver.Text = "";
                    return;
                }
                if ((Convert.ToDecimal(txtMinMediumAssetValue.Text)) > (Convert.ToDecimal(txtMaxMediumAssetValue.Text)))
                {
                    Utility.FunShowAlertMsg(this, "Medium SME Min Total Asset Value should not be greater than Medium SME Max Total Asset Value");
                    txtMinMediumAssetValue.Focus();
                    txtMinMediumAssetValue.Text = "";
                    return;
                }
            }
            ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;
            ObjComplainceMasterDataTable = new ORG.OrgMasterMgtServices.S3GOrgComplainceMaster_AddDataTable();
            ORG.OrgMasterMgtServices.S3GOrgComplainceMaster_AddRow ObjComplainceMasterRow;
            ObjComplainceMasterRow = ObjComplainceMasterDataTable.NewS3GOrgComplainceMaster_AddRow();
            ObjComplainceMasterRow.COMPMAST_ID = intComplainceID.ToString();
            ObjComplainceMasterRow.Company_ID = intCompanyId.ToString();
            ObjComplainceMasterRow.User_ID = intUserId.ToString();
            if (!(string.IsNullOrEmpty(txtEffectiveFromDate.Text)))
            {
                ObjComplainceMasterRow.EFFECTIVEFROM_DATE = Utility.StringToDate(txtEffectiveFromDate.Text.ToString().ToString());
            }
            if (!(string.IsNullOrEmpty(txtEffectiveToDate.Text)))
            {
                ObjComplainceMasterRow.EFFECTIVETO_DATE = Utility.StringToDate(txtEffectiveToDate.Text.ToString().ToString());
            }
            ObjComplainceMasterRow.NID = Convert.ToBoolean(chkNID.Checked);
            ObjComplainceMasterRow.Labourcard = Convert.ToBoolean(chkLabourCard.Checked);
            ObjComplainceMasterRow.Name = Convert.ToBoolean(chkName.Checked);
            ObjComplainceMasterRow.DOB = Convert.ToBoolean(chkDOB.Checked);
            ObjComplainceMasterRow.CRNO = Convert.ToBoolean(chkCRNo.Checked);
            ObjComplainceMasterRow.Passport = Convert.ToBoolean(chkPassport.Checked);
            ObjComplainceMasterRow.ThirdParty_Check = Convert.ToBoolean(chkThirdPartyCheck.Checked);
            ObjComplainceMasterRow.NRID = Convert.ToBoolean(chkNRID.Checked);
            if (txtMaxAgeBorrower.Text != "")
            {
                ObjComplainceMasterRow.COMP_Max_Age_Borrower = Convert.ToInt32(txtMaxAgeBorrower.Text);
            }
            else
                ObjComplainceMasterRow.COMP_Max_Age_Borrower = 0;


            if (txtSmallMinEmpSize.Text != "")
            {
                ObjComplainceMasterRow.SMESmall_Min_Employee_Size = Convert.ToInt32(txtSmallMinEmpSize.Text);
            }
            else
                ObjComplainceMasterRow.SMESmall_Min_Employee_Size = 0;

            if (txtMediumMinEmpSize.Text != "")
            {
                ObjComplainceMasterRow.SMEMedium_Min_Employee_Size = Convert.ToInt32(txtMediumMinEmpSize.Text);
            }
            else
                ObjComplainceMasterRow.SMEMedium_Min_Employee_Size = 0;

            if (txtMicroMinEmpSize.Text != "")
            {
                ObjComplainceMasterRow.SMEMicro_Min_Employee_Size = Convert.ToInt32(txtMicroMinEmpSize.Text);
            }
            else
                ObjComplainceMasterRow.SMEMicro_Min_Employee_Size = 0;


            if (txtSmallMaxEmpSize.Text != "")
            {
                ObjComplainceMasterRow.SME_SmallMax_Employee_Size = Convert.ToInt32(txtSmallMaxEmpSize.Text);
            }
            else
                ObjComplainceMasterRow.SME_SmallMax_Employee_Size = 0;

            if (txtMicroMaxEmpSize.Text != "")
            {
                ObjComplainceMasterRow.SMEMicro_Max_Employee_Size = Convert.ToInt32(txtMicroMaxEmpSize.Text);
            }
            else
                ObjComplainceMasterRow.SMEMicro_Max_Employee_Size = 0;


            if (txtMediumMaxEmpSize.Text != "")
            {
                ObjComplainceMasterRow.SMEMedium_Max_Employee_Size = Convert.ToInt32(txtMediumMaxEmpSize.Text);
            }
            else
                ObjComplainceMasterRow.SMEMedium_Max_Employee_Size = 0;


            if (txtSmallMinTurnOver.Text != "")
            {
                ObjComplainceMasterRow.SMESmall_Min_Turnover = Convert.ToDecimal(txtSmallMinTurnOver.Text);
            }
            else
                ObjComplainceMasterRow.SMESmall_Min_Turnover = 0;

            if (txtMediumMinTurnOver.Text != "")
            {
                ObjComplainceMasterRow.SMEMedium_Min_Turnover = Convert.ToDecimal(txtMediumMinTurnOver.Text);
            }
            else
                ObjComplainceMasterRow.SMEMedium_Min_Turnover = 0;

            if (txtMicroMinTurnOver.Text != "")
            {
                ObjComplainceMasterRow.SMEMicro_Min_Turnover = Convert.ToDecimal(txtMicroMinTurnOver.Text);
            }
            else
                ObjComplainceMasterRow.SMEMicro_Min_Turnover = 0;

            if (txtSmallMaxTurnOver.Text != "")
            {
                ObjComplainceMasterRow.SMESmall_Max_Turnover = Convert.ToDecimal(txtSmallMaxTurnOver.Text);
            }
            else
                ObjComplainceMasterRow.SMESmall_Max_Turnover = 0;

            if (txtMicroMaxTurnOver.Text != "")
            {
                ObjComplainceMasterRow.SMEMicro_Max_Turnover = Convert.ToDecimal(txtMicroMaxTurnOver.Text);
            }
            else
                ObjComplainceMasterRow.SMEMicro_Max_Turnover = 0;

            if (txtMediumMaxTurnOver.Text != "")
            {
                ObjComplainceMasterRow.SMEMedium_Max_Turnover = Convert.ToDecimal(txtMediumMaxTurnOver.Text);
            }
            else
                ObjComplainceMasterRow.SMEMedium_Max_Turnover = 0;

            if (txtMinSmallAssetValue.Text != "")
            {
                ObjComplainceMasterRow.SMESmall_Min_Asset_value = Convert.ToDecimal(txtMinSmallAssetValue.Text);
            }
            else
                ObjComplainceMasterRow.SMESmall_Min_Asset_value = 0;

            if (txtMinMicroAssetValue.Text != "")
            {
                ObjComplainceMasterRow.SMEMicro_Min_Asset_value = Convert.ToDecimal(txtMinMicroAssetValue.Text);
            }
            else
                ObjComplainceMasterRow.SMEMicro_Min_Asset_value = 0;


            if (txtMinMediumAssetValue.Text != "")
            {
                ObjComplainceMasterRow.SMEMedium_Min_Asset_value = Convert.ToDecimal(txtMinMediumAssetValue.Text);
            }
            else
                ObjComplainceMasterRow.SMEMedium_Min_Asset_value = 0;


            if (txtMaxSmallAssetValue.Text != "")
            {
                ObjComplainceMasterRow.SMESmall_Max_Asset_value = Convert.ToDecimal(txtMaxSmallAssetValue.Text);
            }
            else
                ObjComplainceMasterRow.SMESmall_Max_Asset_value = 0;


            if (txtMaxMediumAssetValue.Text != "")
            {
                ObjComplainceMasterRow.SMEMedium_Max_Asset_value = Convert.ToDecimal(txtMaxMediumAssetValue.Text);
            }
            else
                ObjComplainceMasterRow.SMEMedium_Max_Asset_value = 0;


            if (txtMaxMicroAssetValue.Text != "")
            {
                ObjComplainceMasterRow.SMEMicro_Max_Asset_value = Convert.ToDecimal(txtMaxMicroAssetValue.Text);
            }
            else
                ObjComplainceMasterRow.SMEMicro_Max_Asset_value = 0;

            ObjComplainceMasterRow.CREATED_BY = intUserId;
            ObjComplainceMasterRow.Modified_By = intUserId;
            if (ViewState["CollectionDetails"] != null)
            {
                ObjComplainceMasterRow.XMLCOLLECTION = ((DataTable)ViewState["CollectionDetails"]).FunPubFormXml();
            }
            if (ViewState["dtTempCollectionApprover"] != null)
            {
                ObjComplainceMasterRow.XMLAPPROVERCOLLECTION = Convert.ToString(((DataTable)ViewState["dtTempCollectionApprover"]).FunPubFormXml());
            }
            if (ViewState["ClosureDetails"] != null)
            {
                ObjComplainceMasterRow.XMLCLOSURE = Convert.ToString(((DataTable)ViewState["ClosureDetails"]).FunPubFormXml());
            }
            if (ViewState["dtTempClosureApprover"] != null)
            {
                ObjComplainceMasterRow.XMLAPPROVERCLOSURE = Convert.ToString(((DataTable)ViewState["dtTempClosureApprover"]).FunPubFormXml());
            }
            ObjComplainceMasterRow.Complaince_Type_ID = Convert.ToInt32(ddlComplainceType.SelectedValue);
            ObjComplainceMasterRow.Is_Active = Convert.ToBoolean(chkHActive.Checked);
            ObjComplainceMasterDataTable.AddS3GOrgComplainceMaster_AddRow(ObjComplainceMasterRow);

            intErrCode = ObjOrgMasterMgtServicesClient.FunPubCreateComplainceMaster(SerMode, ClsPubSerialize.Serialize(ObjComplainceMasterDataTable, SerMode));

            switch (intErrCode)
            {
                case 0:
                    {
                        if (strMode == "M")
                        {
                            if (ddlComplainceType.SelectedValue == "1")
                            {
                                strKey = "Modify";
                                strAlert = strAlert.Replace("__ALERT__", "Collection details updated successfully");
                                btnSave.Enabled_False();
                            }
                            else if (ddlComplainceType.SelectedValue == "2")
                            {
                                strKey = "Modify";
                                strAlert = strAlert.Replace("__ALERT__", "Closure details updated successfully");
                                btnSave.Enabled_False();
                            }
                            else if (ddlComplainceType.SelectedValue == "3")
                            {
                                strKey = "Modify";
                                strAlert = strAlert.Replace("__ALERT__", "Dedupe details updated successfully");
                                btnSave.Enabled_False();
                            }
                            else if (ddlComplainceType.SelectedValue == "4")
                            {
                                strKey = "Modify";
                                strAlert = strAlert.Replace("__ALERT__", "Other Compliance details updated successfully");
                                btnSave.Enabled_False();
                            }
                            else if (ddlComplainceType.SelectedValue == "5")
                            {
                                strKey = "Modify";
                                strAlert = strAlert.Replace("__ALERT__", "SME details updated successfully");
                                btnSave.Enabled_False();
                            }
                        }

                        else
                        {
                            if (ddlComplainceType.SelectedValue == "1")
                            {
                                strAlert = "Collection details added successfully";
                                strAlert += @"\n\nWould you like to add one more record?";
                                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                                strRedirectPageView = "";

                                //strAlert = "Collection details added successfully";
                                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageAdd + "}";
                                //strRedirectPageView = "";
                                //btnSave.Enabled = false;
                                btnSave.Enabled_False();
                            }
                            else if (ddlComplainceType.SelectedValue == "2")
                            {
                                strAlert = "Closure details added successfully";
                                strAlert += @"\n\nWould you like to add one more record?";
                                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                                strRedirectPageView = "";

                                //strAlert = "Closure details added successfully";
                                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageAdd + "}";
                                //strRedirectPageView = "";
                                //  btnSave.Enabled = false;
                                btnSave.Enabled_False();
                            }
                            else if (ddlComplainceType.SelectedValue == "3")
                            {
                                strAlert = "Dedupe details added successfully";
                                strAlert += @"\n\nWould you like to add one more record?";
                                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                                strRedirectPageView = "";

                                //strAlert = "Dedupe details added successfully";
                                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageAdd + "}";
                                //strRedirectPageView = "";
                                //btnSave.Enabled = false;
                                btnSave.Enabled_False();
                            }
                            else if (ddlComplainceType.SelectedValue == "4")
                            {
                                strAlert = "Other Compliance details added successfully";
                                strAlert += @"\n\nWould you like to add one more record?";
                                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                                strRedirectPageView = "";

                                //strAlert = "Other Compliance details added successfully";
                                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageAdd + "}";
                                //strRedirectPageView = "";
                                //btnSave.Enabled = false;
                                btnSave.Enabled_False();
                            }
                            else if (ddlComplainceType.SelectedValue == "5")
                            {
                                strAlert = "SME details added successfully";
                                strAlert += @"\n\nWould you like to add one more record?";
                                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                                strRedirectPageView = "";

                                //strAlert = "SME details added successfully";
                                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageAdd + "}";
                                //strRedirectPageView = "";
                                //btnSave.Enabled = false;
                                btnSave.Enabled_False();
                            }
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }
                    break;
                case 1:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Selected Effective From Date Already Exists");
                        break;
                    }
                case 2:
                    {
                        // Utility.FunShowAlertMsg(this.Page, "Effective From Date should not be less than the Previous Effective From date range");
                        Utility.FunShowAlertMsg(this.Page, "Data Already Exists In Given Range");
                        break;
                    }
                case 3:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Data Already Exists In Given Range,Cannot active a record");
                        break;
                    }
                case 4:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Same Active Record Already Exists");
                        break;
                    }
                case 20: Utility.FunShowAlertMsg(this.Page, "Error in Adding Compliance Master Details");
                    break;
            }

        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Origination/S3GOrgComplainceMaster_View.aspx");
            btnCancel.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void FunProAssignApprover(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataTable dtApprove = new DataTable();
            Button btn = (Button)sender;
            GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
            GridView grv = (GridView)gvRow.Parent.Parent;

            dt = (DataTable)ViewState["dtTempCollectionApprover"];
            Label lblINoofApprovals = (Label)gvRow.FindControl("lblINoofApprovals");
            Label lblCollectionSNo = (Label)gvRow.FindControl("lblCollectionSNo");
            Label lblCollectionID = (Label)gvRow.FindControl("lblCollectionID");
            Button btnFCollectionApprover = (Button)gvRow.FindControl("btnFCollectionApprover");
            Button btnICollectionApprover = (Button)gvRow.FindControl("btnICollectionApprover");

            if (strMode == "M" || strMode == "Q")
            {
                int intSNo = Convert.ToInt32(lblCollectionID.Text);
                DataRow[] drCollApprover = dt.Select("CollectionID='" + intSNo + "'");
                dtApprove = drCollApprover.CopyToDataTable();
            }

            else
            {
                int intSNo = Convert.ToInt32(lblCollectionSNo.Text);
                DataRow[] drCollApprover = dt.Select("SNo='" + intSNo + "'");
                dtApprove = drCollApprover.CopyToDataTable();
            }
            grvApprover.DataSource = dtApprove;
            grvApprover.DataBind();
            ModalPopupExtenderApprover.Show();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        //  btnFCollectionApprover.Focus(); 
    }

    protected void FunProClosureApprover(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataTable dtCloApprove = new DataTable();
            Button btn = (Button)sender;
            GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
            GridView grv = (GridView)gvRow.Parent.Parent;

            dt = (DataTable)ViewState["dtTempClosureApprover"];
            Label lblIClosureApprovals = (Label)gvRow.FindControl("lblIClosureApprovals");
            Label lblClosureID = (Label)gvRow.FindControl("lblClosureID");
            Label lblClosureSNo = (Label)gvRow.FindControl("lblClosureSNo");
            Button btnFClosureApprover = (Button)gvRow.FindControl("btnFClosureApprover");
            if (strMode == "M" || strMode == "Q")
            {
                int intSNo = Convert.ToInt32(lblClosureID.Text);
                DataRow[] drCloApprover = dt.Select("ClosureID='" + intSNo + "'");
                dtCloApprove = drCloApprover.CopyToDataTable();
            }

            else
            {
                int intSNo = Convert.ToInt32(lblClosureSNo.Text);
                DataRow[] drCloApprover = dt.Select("SNo='" + intSNo + "'");
                dtCloApprove = drCloApprover.CopyToDataTable();
            }
            grvDEVApprover.DataSource = dtCloApprove;
            grvDEVApprover.DataBind();
            ModalPopupExtenderDEVApprover.Show();
            // btnFClosureApprover.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public void FunPubGetDesignationDetails()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            ViewState["ApprovalEntity"] = Utility.GetDefaultData("S3G_ORG_GETAPPROVALENTITY", dictParam);
            ViewState["Designation"] = Utility.GetDefaultData("S3G_ORG_GETDESIGNATION", dictParam);
            //ViewState["Location"] = Utility.GetDefaultData("S3G_ORG_GETLOCATION", dictParam);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public void FunPubGetLocation()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@Mode", strMode);
            dictParam.Add("@PROGRAM_ID", Convert.ToString(intprogramId));
            ViewState["Location"] = Utility.GetDefaultData("S3G_ORG_GETLOCATION", dictParam);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    public void FunPubClosureDesignationDetails()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            ViewState["ApprovalClosureEntity"] = Utility.GetDefaultData("S3G_ORG_GETAPPROVALENTITY", dictParam);
            ViewState["DesignationClosure"] = Utility.GetDefaultData("S3G_ORG_GETDESIGNATION", dictParam);
            ViewState["LocationClosure"] = Utility.GetDefaultData("S3G_ORG_GETLOCATION", dictParam);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProDeleteRowsFromDataTable(ref DataTable dtApproval, ref DataTable dtApprover, ref int intDeletedRow, int intRowIndex)
    {
        try
        {
            DataRow[] dRow = dtApprover.Select("SNo='" + intDeletedRow + "'");
            if (dRow.Length > 0)
            {
                foreach (DataRow dr in dRow)
                {
                    dtApprover.Rows.Remove(dr);
                }
            }
            dtApprover.AcceptChanges();
            dtApproval.Rows[intRowIndex].Delete();
            dtApproval.AcceptChanges();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnModalAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtModal = new DataTable();
            DataRow[] drApprover;
            string strApprovalEntity = "";
            foreach (GridViewRow GvRow in grvApprover.Rows)
            {
                DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
                DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
                DropDownList ddlLocation = (DropDownList)GvRow.FindControl("ddlLocation");
                Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");

                if (ddlApprovalEntity.Items.Count > 1 && ddlApprovalEntity.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this, "Select Approver");
                    ddlApprovalEntity.Focus();
                    return;
                }
                if (ddlApprovalEntity.SelectedValue == "2")
                {
                    if (ddlLocation.Items.Count > 1 && ddlLocation.SelectedValue == "0")
                    {
                        Utility.FunShowAlertMsg(this, "Select Branch");
                        ddlApprovPerson.Focus();
                        return;
                    }
                }

                if (ddlApprovPerson.Items.Count > 1 && ddlApprovPerson.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this, "Select Approval Authority");
                    ddlApprovPerson.Focus();
                    return;
                }


                if (grvApprover.Rows.Count > 0)
                {
                    DropDownList ddlEntity = (DropDownList)grvApprover.Rows[0].FindControl("ddlApprovalEntity");
                    strApprovalEntity = ddlEntity.SelectedValue;
                }
                if (ddlApprovalEntity.SelectedValue != strApprovalEntity)
                {
                    Utility.FunShowAlertMsg(this, "Select Same Approver");
                    ddlApprovalEntity.Focus();
                    return;
                }

            }
            dtModal = (DataTable)ViewState["dtTempCollectionApprover"];


            int intMasterSeq = Convert.ToInt32(((Label)grvApprover.Rows[0].FindControl("LblSNo")).Text);
            DataRow[] Drow = dtModal.Select("SNo='" + intMasterSeq + "'");
            foreach (DataRow dr in Drow)
            {
                dtModal.Rows.Remove(dr);
            }

            foreach (GridViewRow GvRow in grvApprover.Rows)
            {
                DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
                DropDownList ddlLocation = (DropDownList)GvRow.FindControl("ddlLocation");
                DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
                Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");

                // int intSeq = Convert.ToInt32(((Label)grvCollectionDtls.Rows[0].FindControl("lblCollectionSNo")).Text);
                //Label lbl = (Label)grvCollectionDtls.Rows[grvCollectionDtls.Rows.Count - 1].FindControl("lblCollectionSNo");
                // int intSNo = (Convert.ToInt32(lbl.Text)) + 1;

                DataRow dRow = dtModal.NewRow();

                drApprover = dtModal.Select("SNo='" + intMasterSeq +
                       "' and ApprovEntity='" + ddlApprovalEntity.SelectedValue
                       + "' and Location='" + ddlLocation.SelectedValue
                       + "' and ApprovPerson='" + ddlApprovPerson.SelectedValue
                       + "'");
                if (ddlApprovalEntity.SelectedValue == "2")
                {
                    if (drApprover.Length > 0)
                    {
                        Utility.FunShowAlertMsg(this, "Approval Authority have to be different");
                        return;
                    }
                }
                // dRow["No"] = intSeq;
                dRow["SNo"] = intMasterSeq;
                dRow["SequenceNumber"] = lblSequenceNumber.Text;
                dRow["ApprovEntity"] = ddlApprovalEntity.SelectedValue;
                dRow["Location"] = ddlLocation.SelectedValue;
                dRow["ApprovPerson"] = ddlApprovPerson.SelectedValue;
                dtModal.Rows.Add(dRow);

            }
            ViewState["dtTempCollectionApprover"] = dtModal;
            ModalPopupExtenderApprover.Hide();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnModalCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderApprover.Hide();
    }
    protected void btnDEVModalAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //Button btn = (Button)sender;
            DataTable dtCloModal = new DataTable();
            string strApprovalEntity = "";
            foreach (GridViewRow GvRow in grvDEVApprover.Rows)
            {
                DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
                DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
                DropDownList ddlLocation = (DropDownList)GvRow.FindControl("ddlLocation");
                Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");
                if (ddlApprovalEntity.Items.Count > 1 && ddlApprovalEntity.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this, "Select Approver");
                    ddlApprovalEntity.Focus();
                    return;
                } if (ddlApprovalEntity.SelectedValue == "2")
                {
                    if (ddlLocation.Items.Count > 1 && ddlLocation.SelectedValue == "0")
                    {
                        Utility.FunShowAlertMsg(this, "Select Branch");
                        ddlApprovPerson.Focus();
                        return;
                    }
                }
                if (ddlApprovPerson.Items.Count > 1 && ddlApprovPerson.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this, "Select Approval Authority");
                    ddlApprovPerson.Focus();
                    return;
                }

                if (grvDEVApprover.Rows.Count > 0)
                {
                    DropDownList ddlEntity = (DropDownList)grvDEVApprover.Rows[0].FindControl("ddlApprovalEntity");
                    strApprovalEntity = ddlEntity.SelectedValue;
                }
                if (ddlApprovalEntity.SelectedValue != strApprovalEntity)
                {
                    Utility.FunShowAlertMsg(this, "Select Same Approver");
                    ddlApprovalEntity.Focus();
                    ddlLocation.ClearDropDownList();
                    return;
                }

            }
            dtCloModal = (DataTable)ViewState["dtTempClosureApprover"];

            int intMasterSeq = Convert.ToInt32(((Label)grvDEVApprover.Rows[0].FindControl("LblSNo")).Text);

            DataRow[] Drow = dtCloModal.Select("SNo='" + intMasterSeq + "'");
            foreach (DataRow dr in Drow)
            {
                dtCloModal.Rows.Remove(dr);
            }

            foreach (GridViewRow GvRow in grvDEVApprover.Rows)
            {

                DropDownList ddlApprovalEntity = (DropDownList)GvRow.FindControl("ddlApprovalEntity");
                DropDownList ddlApprovPerson = (DropDownList)GvRow.FindControl("ddlApprovPerson");
                DropDownList ddlLocation = (DropDownList)GvRow.FindControl("ddlLocation");
                Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");
                DataRow dRow = dtCloModal.NewRow();
                DataRow[] drColapprover;

                drColapprover = dtCloModal.Select("SNo='" + intMasterSeq +
                       "' and ApprovEntity='" + ddlApprovalEntity.SelectedValue
                                + "' and Location='" + ddlLocation.SelectedValue
                                + "' and ApprovPerson='" + ddlApprovPerson.SelectedValue
                                + "'");
                if (ddlApprovalEntity.SelectedValue == "2")
                {
                    if (drColapprover.Length > 0)
                    {
                        Utility.FunShowAlertMsg(this, "Approval Authority have to be different");
                        return;
                    }
                }
                dRow["SNo"] = intMasterSeq;
                dRow["SequenceNumber"] = lblSequenceNumber.Text;
                dRow["ApprovEntity"] = ddlApprovalEntity.SelectedValue;
                dRow["Location"] = ddlLocation.SelectedValue;
                dRow["ApprovPerson"] = ddlApprovPerson.SelectedValue;
                dtCloModal.Rows.Add(dRow);

            }
            ViewState["dtTempClosureApprover"] = dtCloModal;

            ModalPopupExtenderDEVApprover.Hide();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnDEVModalCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderDEVApprover.Hide();
    }

    protected void grvDEVApprover_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblApprovalEntity = (Label)e.Row.FindControl("lblApprovalEntity");
                Label lblApprovalPerson = (Label)e.Row.FindControl("lblApprovalPerson");
                DropDownList ddlApprovalEntity = (DropDownList)e.Row.FindControl("ddlApprovalEntity");
                DropDownList ddlApprovPerson = (DropDownList)e.Row.FindControl("ddlApprovPerson");
                Label lblLocation = (Label)e.Row.FindControl("lblLocation");
                DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");

                ddlApprovalEntity.BindDataTable((DataTable)ViewState["ApprovalClosureEntity"]);
                ddlApprovalEntity.SelectedValue = lblApprovalEntity.Text;
                ddlLocation.BindDataTable((DataTable)ViewState["LocationClosure"]);
                ddlLocation.SelectedValue = lblLocation.Text;
                ddlLocation_SelectedIndexChanged1(ddlLocation, null);
                if (ddlApprovalEntity.SelectedValue == "1")
                {
                    ddlApprovPerson.BindDataTable((DataTable)ViewState["DesignationClosure"]);
                }
                else
                {
                    ddlApprovPerson.BindDataTable((DataTable)ViewState["UserNameClosure"]);
                }
                ddlApprovPerson.SelectedValue = lblApprovalPerson.Text;

                //ddlApprovalEntity.BindDataTable((DataTable)ViewState["ApprovalEntity"]);
                //ddlApprovalEntity.SelectedValue = lblApprovalEntity.Text;
                //ddlApprovPerson.BindDataTable((DataTable)ViewState["Designation"]);
                //ddlLocation.BindDataTable((DataTable)ViewState["Location"]);

                //if (ddlApprovalEntity.Items.Count == 2)
                //{
                //    ddlApprovalEntity.Items.RemoveAt(0);
                //}
                //if (ddlApprovPerson.Items.Count == 2)
                //{
                //    ddlApprovPerson.Items.RemoveAt(0);
                //}
                //if (ddlLocation.Items.Count == 2)
                //{
                //    ddlLocation.Items.RemoveAt(0);
                //}

                //ddlApprovPerson.SelectedValue = lblApprovalPerson.Text;
                //ddlLocation.SelectedValue = lblLocation.Text;

                // if (btnDEVModalAdd.Enabled == false)
                if (btnDEVModalAdd.Disabled == true)
                {
                    ddlApprovalEntity.ClearDropDownList();
                    ddlApprovPerson.ClearDropDownList();
                    ddlLocation.ClearDropDownList();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvApprover_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataTable dtModal = new DataTable();
            DataRow[] drApprover;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblApprovalEntity = (Label)e.Row.FindControl("lblApprovalEntity");
                Label lblApprovalPerson = (Label)e.Row.FindControl("lblApprovalPerson");
                Label lblLocation = (Label)e.Row.FindControl("lblLocation");

                DropDownList ddlApprovalEntity = (DropDownList)e.Row.FindControl("ddlApprovalEntity");
                DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");
                DropDownList ddlApprovPerson = (DropDownList)e.Row.FindControl("ddlApprovPerson");


                ddlApprovalEntity.BindDataTable((DataTable)ViewState["ApprovalEntity"]);
                ddlApprovalEntity.SelectedValue = lblApprovalEntity.Text;
                ddlLocation.BindDataTable((DataTable)ViewState["Location"]);
                ddlLocation.SelectedValue = lblLocation.Text;
                ddlLocation_SelectedIndexChanged(ddlLocation, null);
                if (ddlApprovalEntity.SelectedValue == "1")
                {
                    ddlApprovPerson.BindDataTable((DataTable)ViewState["Designation"]);
                }
                else
                {
                    ddlApprovPerson.BindDataTable((DataTable)ViewState["UserName"]);
                }
                ddlApprovPerson.SelectedValue = lblApprovalPerson.Text;

                //  if (btnModalAdd.Enabled == false)
                if (btnModalAdd.Disabled == true)
                {
                    ddlApprovalEntity.ClearDropDownList();
                    ddlApprovPerson.ClearDropDownList();
                    ddlLocation.ClearDropDownList();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvCollectionDtls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataTable dtCollectionDtls = (DataTable)ViewState["CollectionDetails"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDays = (Label)e.Row.FindControl("lblDays");
                Label lblIsDelete = (Label)e.Row.FindControl("lblIsDelete");
                Button lnkDelete = (Button)e.Row.FindControl("lnkDelete");
                Button btnIApproverDesig = (Button)e.Row.FindControl("btnICollectionApprover");
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtFTAmount = (TextBox)e.Row.FindControl("txtFTAmount");
                DropDownList ddlFType = (DropDownList)e.Row.FindControl("ddlFType");
                //  txtFTAmount.SetPercentagePrefixSuffix(9, 3, true, "Received Amount");
                txtFTAmount.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Amount");
                if (dtCollectionDtls != null && dtCollectionDtls.Rows.Count == 0)
                {
                    txtFTAmount.Text = "0.01";
                    txtFTAmount.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvClosureDtls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtFDownPayment = (TextBox)e.Row.FindControl("txtFDownPayment");
                txtFDownPayment.SetDecimalPrefixSuffix(3, 2, true, "Down Payment %");
                Label lblMonthClosure = (Label)e.Row.FindControl("lblMonthClosure");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblClosureIsDelete = (Label)e.Row.FindControl("lblClosureIsDelete");
                Button lnkClosureDelete = (Button)e.Row.FindControl("lnkClosureDelete");
                Button btnIClosureApprover = (Button)e.Row.FindControl("btnIClosureApprover");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadCommonData()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            DataTable dtComplaince = new DataTable();
            if (dtComplaince != null)
            {
                Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
                dtComplaince = Utility.GetDefaultData("S3G_ORG_LOADCOMPLAINCETYPE", Procparam);
                ddlComplainceType.FillDataTable(dtComplaince, "Value", "Name");
                ddlComplainceType.Items.RemoveAt(0);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadCollection()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            DataTable dtCollection = new DataTable();
            Procparam.Add("@COMPANY_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            DropDownList ddlFType = (DropDownList)grvCollectionDtls.FooterRow.FindControl("ddlFType");
            ddlFType.Items.Clear();
            dtCollection = Utility.GetDefaultData("S3G_ORG_GETCOLLECTIONTYPE", Procparam);
            if (dtCollection != null && dtCollection.Rows.Count > 0)
            {
                ddlFType.FillDataTable(dtCollection, "Value", "Name");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnFCollectionApprover_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            Button btn = (Button)sender;
            GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
            TextBox txtFNoofApproval = (TextBox)gvRow.FindControl("txtFNoofApproval");
            Label lblCollectionID = (Label)gvRow.FindControl("lblCollectionID");
            Label lblClosureID = (Label)gvRow.FindControl("lblClosureID");
            Button btnFCollectionApprover = (Button)gvRow.FindControl("btnFCollectionApprover");

            string strSLNo = "";
            Label lblSNo = (Label)grvCollectionDtls.Rows[grvCollectionDtls.Rows.Count - 1].FindControl("lblCollectionSNo");
            strSLNo = (Convert.ToInt32(lblSNo.Text) + 1).ToString();

            //grvApprover.DataSource = ViewState["dtTempCollectionApprover"];
            //grvApprover.DataBind();

            if (ViewState["dtTempCollectionApprover"] == null)
            {
                ViewState["dtTempCollectionApprover"] = FunProInitializePopup();
            }
            DataTable dt1 = (DataTable)ViewState["dtTempCollectionApprover"];
            DataTable dtApprove = new DataTable();
            DataRow[] dRow = dt1.Select("SNo ='" + strSLNo + "'");
            //DataRow[] dRow = dt1.Select("SNo='" + strSLNo + "' AND CollID='" + lblCollectionID.Text.ToString() + "'");
            if (dRow.Length > 0)
            {
                dtApprove = dt1.Clone();
                dRow.CopyToDataTable(dtApprove, LoadOption.OverwriteChanges);
            }

            if (dtApprove != null && dtApprove.Rows.Count > 0)
            {
                int intNoofApproval = Convert.ToInt32(txtFNoofApproval.Text);
                int intRowCount = dtApprove.Rows.Count;

                if (intNoofApproval > intRowCount)
                {
                    for (int inti = intRowCount + 1; inti <= (intRowCount + (intNoofApproval - intRowCount)); inti++)
                    {
                        DataRow dr;
                        dr = dtApprove.NewRow();
                        dr["SNo"] = strSLNo;
                        dr["SequenceNumber"] = inti;
                        dr["ApprovEntity"] = string.Empty;
                        dr["Location"] = string.Empty;
                        dr["ApprovPerson"] = string.Empty;
                        dtApprove.Rows.Add(dr);
                    }
                }

                if (intNoofApproval < intRowCount)
                {
                    for (int inti = 1; inti <= intRowCount - intNoofApproval; inti++)
                    {
                        dtApprove.Rows[dtApprove.Rows.Count - 1].Delete();
                        dtApprove.AcceptChanges();
                    }
                }

                grvApprover.DataSource = dtApprove;
                grvApprover.DataBind();
                btnFCollectionApprover.Focus();
            }
            else
            {
                dtApprove = FunProInitializePopup();
                int intNoofApproval = Convert.ToInt32(txtFNoofApproval.Text);
                for (int inti = 1; inti <= intNoofApproval; inti++)
                {
                    DataRow dr;
                    dr = dtApprove.NewRow();
                    dr["SNo"] = strSLNo;
                    dr["SequenceNumber"] = inti;
                    dr["ApprovEntity"] = string.Empty;
                    dr["Location"] = string.Empty;
                    dr["ApprovPerson"] = string.Empty;
                    dtApprove.Rows.Add(dr);
                }

                grvApprover.DataSource = dtApprove;
                grvApprover.DataBind();

                dt1.Merge(dtApprove);

                ViewState["dtTempCollectionApprover"] = dt1;
            }
            ModalPopupExtenderApprover.Show();
            btnFCollectionApprover.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnICollectionApprover_Click(object sender, EventArgs e)
    {
        FunProAssignApprover(sender, e);
    }
    protected void btnIClosureApprover_Click(object sender, EventArgs e)
    {
        FunProClosureApprover(sender, e);
    }
    protected void btnFClosureApprover_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            Button btn = (Button)sender;
            GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
            TextBox txtFClosureApproval = (TextBox)gvRow.FindControl("txtFClosureApproval");
            string strSLNo = "";
            Label lblSNo = (Label)grvClosureDtls.Rows[grvClosureDtls.Rows.Count - 1].FindControl("lblClosureSNo");
            strSLNo = (Convert.ToInt32(lblSNo.Text) + 1).ToString();
            Button btnFClosureApprover = (Button)gvRow.FindControl("btnFClosureApprover");

            grvDEVApprover.DataSource = ViewState["dtTempClosureApprover"];
            grvDEVApprover.DataBind();
            if (ViewState["dtTempClosureApprover"] == null)
            {
                ViewState["dtTempClosureApprover"] = FunProInitializePopup();
            }
            DataTable dt1 = (DataTable)ViewState["dtTempClosureApprover"];
            DataTable dtCloApprove = new DataTable();
            DataRow[] dRow = dt1.Select("SNo ='" + strSLNo + "'");
            if (dRow.Length > 0)
            {
                dtCloApprove = dt1.Clone();
                dRow.CopyToDataTable(dtCloApprove, LoadOption.OverwriteChanges);
            }

            if (dtCloApprove != null && dtCloApprove.Rows.Count > 0)
            {
                int intNoofApproval = Convert.ToInt32(txtFClosureApproval.Text);
                int intRowCount = dtCloApprove.Rows.Count;

                if (intNoofApproval > intRowCount)
                {
                    for (int inti = intRowCount + 1; inti <= (intRowCount + (intNoofApproval - intRowCount)); inti++)
                    {
                        DataRow dr;
                        dr = dtCloApprove.NewRow();
                        dr["SNo"] = strSLNo;
                        dr["SequenceNumber"] = inti;
                        dr["ApprovEntity"] = string.Empty;
                        dr["Location"] = string.Empty;
                        dr["ApprovPerson"] = string.Empty;
                        dtCloApprove.Rows.Add(dr);
                    }
                }

                if (intNoofApproval < intRowCount)
                {
                    for (int inti = 1; inti <= intRowCount - intNoofApproval; inti++)
                    {
                        dtCloApprove.Rows[dtCloApprove.Rows.Count - 1].Delete();
                        dtCloApprove.AcceptChanges();
                    }
                }
                grvDEVApprover.DataSource = dtCloApprove;
                grvDEVApprover.DataBind();
            }
            else
            {
                dtCloApprove = FunProInitializePopup();
                int intNoofApproval = Convert.ToInt32(txtFClosureApproval.Text);
                for (int inti = 1; inti <= intNoofApproval; inti++)
                {
                    DataRow dr;
                    dr = dtCloApprove.NewRow();
                    dr["SNo"] = strSLNo;
                    dr["SequenceNumber"] = inti;
                    dr["ApprovEntity"] = string.Empty;
                    dr["Location"] = string.Empty;
                    dr["ApprovPerson"] = string.Empty;
                    dtCloApprove.Rows.Add(dr);
                }

                grvDEVApprover.DataSource = dtCloApprove;
                grvDEVApprover.DataBind();
                dt1.Merge(dtCloApprove);
                ViewState["dtTempClosureApprover"] = dt1;
            }
            ModalPopupExtenderDEVApprover.Show();
            PnlApprover1.Visible = true;
            btnFClosureApprover.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvCollectionDtls_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void grvCollectionDtls_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtNew = new DataTable();
            Label lblSNo = (Label)grvCollectionDtls.Rows[e.RowIndex].FindControl("lblCollectionSNo");
            int intDeletedRow = Convert.ToInt32(lblSNo.Text);
            DataTable dt = (DataTable)ViewState["CollectionDetails"];
            DataTable dt1 = (DataTable)ViewState["dtTempCollectionApprover"];
            var c = from d1 in dt1.AsEnumerable()
                    where !dt.AsEnumerable().Any(r2 => r2.Field<string>("SNo").ToString() == d1.Field<string>("SNo").ToString())
                    select d1;

            DataRow[] dArray = c.ToArray();
            if (dArray.Length > 0)
            {
                foreach (DataRow drow in dArray)
                {
                    dt1.Rows.Remove(drow);
                    dt1.AcceptChanges();
                }
            }

            FunProDeleteRowsFromDataTable(ref dt, ref dt1, ref intDeletedRow, e.RowIndex);
            ViewState["dtTempCollectionApprover"] = dt1;
            if (dt.Rows.Count == 0)
            {
                FunPriInitializeCollectionApproval();
            }
            else
            {
                ViewState["CollectionDetails"] = dt;
                grvCollectionDtls.DataSource = dt;
                grvCollectionDtls.DataBind();
            }
            FunPriLoadCollection();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvClosureDtls_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Label lblSNo = (Label)grvClosureDtls.Rows[e.RowIndex].FindControl("lblClosureSNo");
            int intDeletedRow = Convert.ToInt32(lblSNo.Text);
            DataTable dt = (DataTable)ViewState["ClosureDetails"];
            DataTable dt1 = (DataTable)ViewState["dtTempClosureApprover"];
            var c = from d1 in dt1.AsEnumerable()
                    where !dt.AsEnumerable().Any(r2 => r2.Field<string>("SNo").ToString() == d1.Field<string>("SNo").ToString())
                    select d1;

            DataRow[] dArray = c.ToArray();
            if (dArray.Length > 0)
            {
                foreach (DataRow drow in dArray)
                {
                    dt1.Rows.Remove(drow);
                    dt1.AcceptChanges();
                }
            }
            FunProDeleteRowsFromDataTable(ref dt, ref dt1, ref intDeletedRow, e.RowIndex);
            ViewState["dtTempClosurepprover"] = dt1;
            if (dt.Rows.Count == 0)
            {
                FunPriInitializeClosureApproval();
            }
            else
            {
                ViewState["ClosureDetails"] = dt;
                grvClosureDtls.DataSource = dt;
                grvClosureDtls.DataBind();
            }
            FunPriLoadCollection();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvClosureDtls_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void ddlComplainceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearViewState();
            if (ddlComplainceType.SelectedValue == "1")
            {
                FunPriInitializeCollectionApproval();
                FunProInitializePopup();
                btnSave.Visible = true;
                btnCancel.Visible = true;
                pnlCollectionDtls.Visible = true;
                PnlClosure.Visible = false;
                pnlDedupe.Visible = false;
                PnlOtherComplaince.Visible = false;
                Panel1.Visible = false;
                ddlComplainceType.Focus();
            }
            if (ddlComplainceType.SelectedValue == "2")
            {
                FunPriInitializeClosureApproval();
                FunProInitializePopup();
                btnSave.Visible = true;
                btnCancel.Visible = true;
                PnlClosure.Visible = true;
                pnlCollectionDtls.Visible = false;
                pnlDedupe.Visible = false;
                PnlOtherComplaince.Visible = false;
                Panel1.Visible = false;
                ddlComplainceType.Focus();
            }
            if (ddlComplainceType.SelectedValue == "3")
            {
                //TabDedupe.Visible = true;
                //TabCollection.Visible = false;
                //TabClosure.Visible = false;
                //TabOtherComplaince.Visible = false;
                //TabSME.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                //   tcComplainceMaster.ActiveTabIndex = 2;
                pnlDedupe.Visible = true;
                PnlClosure.Visible = false;
                pnlCollectionDtls.Visible = false;
                PnlOtherComplaince.Visible = false;
                Panel1.Visible = false;
                Button btnFCollectionApprover = (Button)grvCollectionDtls.FooterRow.FindControl("btnFCollectionApprover");
                Button btnICollectionApprover = (Button)grvCollectionDtls.FooterRow.FindControl("btnICollectionApprover");
                btnFCollectionApprover.Visible = false;
                // btnICollectionApprover.Visible = false;
                // Button btnFClosureApprover = (Button)grvClosureDtls.FooterRow.FindControl("btnFClosureApprover");
                // Button btnIClosureApprover = (Button)grvClosureDtls.FooterRow.FindControl("btnIClosureApprover");
                //btnIClosureApprover.Visible = false;
                //btnFClosureApprover.Visible = false;
                ddlComplainceType.Focus();
            }
            if (ddlComplainceType.SelectedValue == "4")
            {
                //TabOtherComplaince.Visible = true;
                //TabCollection.Visible = false;
                //TabClosure.Visible = false;
                //TabDedupe.Visible = false;
                //TabSME.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                // tcComplainceMaster.ActiveTabIndex = 3;
                PnlOtherComplaince.Visible = true;
                pnlDedupe.Visible = false;
                PnlClosure.Visible = false;
                pnlCollectionDtls.Visible = false;
                Panel1.Visible = false;
                ddlComplainceType.Focus();
            }
            if (ddlComplainceType.SelectedValue == "5")
            {
                //TabSME.Visible = true;
                //TabCollection.Visible = false;
                // TabClosure.Visible = false;
                // TabDedupe.Visible = false;
                // TabOtherComplaince.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                // tcComplainceMaster.ActiveTabIndex = 4;
                Panel1.Visible = true;
                PnlOtherComplaince.Visible = false;
                pnlDedupe.Visible = false;
                PnlClosure.Visible = false;
                pnlCollectionDtls.Visible = false;
                ddlComplainceType.Focus();
            }
            ddlComplainceType.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void ddlApprovalEntity_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddl.Parent.Parent;

            Label lblApprovalEntity = gvRow.FindControl("lblApprovalEntity") as Label;
            DropDownList ddlApprovalEntity = gvRow.FindControl("ddlApprovalEntity") as DropDownList;
            DropDownList ddlApprovPerson = gvRow.FindControl("ddlApprovPerson") as DropDownList;
            DropDownList ddlLocation = gvRow.FindControl("ddlLocation") as DropDownList;
            if (ddlApprovalEntity.SelectedValue == "1")
            {
                ddlApprovPerson.BindDataTable((DataTable)ViewState["Designation"]);
                ddlLocation.Enabled = false;
                ddlLocation.SelectedValue = "0";
                ddlApprovPerson.Enabled = true;
            }
            else if (ddlApprovalEntity.SelectedValue == "2")
            {
                // FunPubGetUserDetails();
                ddlApprovPerson.BindDataTable((DataTable)ViewState["UserName"]);
                ddlLocation.Enabled = true;
                ddlApprovPerson.Enabled = true;
            }
            else if (ddlApprovalEntity.SelectedValue == "0")
            {
                ddlLocation.Enabled = false;
                ddlLocation.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void ddlApprovalEntity_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddl.Parent.Parent;

            Label lblApprovalEntity = gvRow.FindControl("lblApprovalEntity") as Label;
            DropDownList ddlApprovalEntity = gvRow.FindControl("ddlApprovalEntity") as DropDownList;
            DropDownList ddlApprovPerson = gvRow.FindControl("ddlApprovPerson") as DropDownList;
            DropDownList ddlLocation = gvRow.FindControl("ddlLocation") as DropDownList;

            if (ddlApprovalEntity.SelectedValue == "1")
            {
                ddlApprovPerson.BindDataTable((DataTable)ViewState["Designation"]);
                ddlLocation.Enabled = false;
                ddlApprovPerson.Enabled = true;
            }
            else if (ddlApprovalEntity.SelectedValue == "2")
            {
                ddlApprovPerson.BindDataTable((DataTable)ViewState["UserName"]);
                ddlLocation.Enabled = true;
                ddlApprovPerson.Enabled = true;
            }
            else if (ddlApprovalEntity.SelectedValue == "0")
            {
                ddlLocation.Enabled = false;
                ddlLocation.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunBind_Effective_To();
            txtEffectiveFromDate.Text = "";
            //  txtEffectiveToDate.Text = "";
            //ddlComplainceType.ClearDropDownList();
            txtMaxAgeBorrower.Text = "";
            txtMaxMediumAssetValue.Text = "";
            txtMaxMicroAssetValue.Text = "";
            txtMaxSmallAssetValue.Text = "";
            txtMediumMaxEmpSize.Text = "";
            txtMediumMaxTurnOver.Text = "";
            txtMediumMinEmpSize.Text = "";
            txtMediumMinTurnOver.Text = "";
            txtMicroMaxEmpSize.Text = "";
            txtMicroMaxTurnOver.Text = "";
            txtMicroMinEmpSize.Text = "";
            txtMicroMinTurnOver.Text = "";
            txtMinMediumAssetValue.Text = "";
            txtMinMicroAssetValue.Text = "";
            txtMinSmallAssetValue.Text = "";
            txtSmallMaxEmpSize.Text = "";
            txtSmallMaxTurnOver.Text = "";
            txtSmallMinEmpSize.Text = "";
            txtSmallMinTurnOver.Text = "";
            grvClosureDtls.DataSource = null;
            grvClosureDtls.DataBind();
            FunPriInitializeClosureApproval();
            grvCollectionDtls.DataSource = null;
            grvCollectionDtls.DataBind();
            FunPriInitializeCollectionApproval();
            grvDEVApprover.DataSource = null;
            grvDEVApprover.DataBind();
            FunProInitializePopup();
            grvApprover.DataSource = null;
            grvApprover.DataBind();
            FunProInitializePopup();
            chkDOB.Checked = false;
            chkLabourCard.Checked = false;
            chkName.Checked = false;
            chkNID.Checked = false;
            chkNRID.Checked = false;
            chkPassport.Checked = false;
            chkThirdPartyCheck.Checked = false;
            chkCRNo.Checked = false;
            ddlComplainceType_SelectedIndexChanged(1, null);
            //  ddlComplainceType.SelectedValue = "1";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void FunBind_Effective_To()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "793");
            Procparam.Add("@Param1", Convert.ToString(intCompanyId));
            DataTable dtdata = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);
            if (dtdata.Rows.Count > 0)
            {
                txtEffectiveToDate.Text = dtdata.Rows[0][0].ToString();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCollectionApproval = (DataTable)ViewState["CollectionDetails"];
            DataRow dRow;
            DataRow[] dRow1;
            //if (e.CommandName == "Add")
            //{
            Label lbl = (Label)grvCollectionDtls.Rows[grvCollectionDtls.Rows.Count - 1].FindControl("lblCollectionSNo");
            int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
            DropDownList ddlFType = (DropDownList)grvCollectionDtls.FooterRow.FindControl("ddlFType");
            TextBox txtFDays = (TextBox)grvCollectionDtls.FooterRow.FindControl("txtFDays");
            TextBox txtFTAmount = (TextBox)grvCollectionDtls.FooterRow.FindControl("txtFTAmount");
            TextBox txtFNoofApproval = (TextBox)grvCollectionDtls.FooterRow.FindControl("txtFNoofApproval");

            dRow1 = dtCollectionApproval.Select("COLLTYPE='" + ddlFType.SelectedItem.Text
                   + "' and NO_OF_DAYS='" + txtFDays.Text
                   + "' and RECEIVED_AMT='" + txtFTAmount.Text
                   + "'");

            if (dRow1.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Collection Details have to be different");
                return;
            }
            //if (ViewState["dtTempCollectionApprover"] != null)
            //{
            //    Utility.FunShowAlertMsg(this, "Select Approvers in Approver grid");
            //    return;
            //}
            if (dtCollectionApproval != null)
            {
                if (((DataTable)(ViewState["dtTempCollectionApprover"])) != null)
                {
                    DataRow[] dArray = ((DataTable)(ViewState["dtTempCollectionApprover"])).Select("SNo='" + intSNo + "' and ApprovEntity<>'' and Location<>'' and ApprovPerson<>''");
                    if (dArray.Length != (Convert.ToInt32(txtFNoofApproval.Text)))
                    {
                        Utility.FunShowAlertMsg(this, "Select Approvers in Approver grid");
                        //txtFNoofApproval.Text = "";
                        return;
                    }
                }
                //else
                //{
                //    Utility.FunShowAlertMsg(this, "Assign the Approvers");
                //    return;
                //}
            }


            if (txtFTAmount.Text == "0.00")
            {
                Utility.FunShowAlertMsg(this, "Amount cannot be 0");
                return;
                txtFTAmount.Focus();
            }
            if (txtFDays.Text == "0")
            {
                Utility.FunShowAlertMsg(this, "Within No. Of Days Should be greater than 0");
                return;
                txtFDays.Focus();
            }
            dRow = dtCollectionApproval.NewRow();
            dRow["SNo"] = (Convert.ToInt32(lbl.Text) + 1).ToString();
            dRow["CollectionID"] = 0;
            dRow["Type_ID"] = ddlFType.SelectedValue;
            dRow["COLLTYPE"] = ddlFType.SelectedItem.Text;
            dRow["NO_OF_DAYS"] = txtFDays.Text;
            dRow["RECEIVED_AMT"] = txtFTAmount.Text;
            dRow["NO_OF_APPROVAL"] = txtFNoofApproval.Text;
            //  dRow["Active"] = chkFCollectionFActive.Checked = true;
            dtCollectionApproval.Rows.Add(dRow);
            grvCollectionDtls.DataSource = dtCollectionApproval;
            grvCollectionDtls.DataBind();
            ViewState["CollectionDetails"] = dtCollectionApproval;
            FunPriLoadCollection();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnAdd_ServerClick1(object sender, EventArgs e)
    {
        try
        {
            DataTable dtClosureApproval = (DataTable)ViewState["ClosureDetails"];
            DataRow dRow;
            DataRow[] dRClosure;
            Label lbl = (Label)grvClosureDtls.Rows[grvClosureDtls.Rows.Count - 1].FindControl("lblClosureSNo");
            int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
            TextBox txtFDownPayment = (TextBox)grvClosureDtls.FooterRow.FindControl("txtFDownPayment");
            TextBox txtFMonthClosure = (TextBox)grvClosureDtls.FooterRow.FindControl("txtFMonthClosure");
            TextBox txtFClosureApproval = (TextBox)grvClosureDtls.FooterRow.FindControl("txtFClosureApproval");
            // CheckBox chkFClosureActive = (CheckBox)grvClosureDtls.FooterRow.FindControl("chkFClosureActive");

            dRClosure = dtClosureApproval.Select("Down_Payment='" + txtFDownPayment.Text
               + "' and Month_Of_Closure='" + txtFMonthClosure.Text
               + "' and No_of_Approval='" + txtFClosureApproval.Text
               + "'");

            if (dRClosure.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Closure Details have to be different");
                return;
            }

            if (txtFMonthClosure.Text == "0")
            {
                Utility.FunShowAlertMsg(this, "Month Closure Value Should be greater than 0");
                return;
                txtFMonthClosure.Focus();
            }
            //if (ViewState["dtTempClosureApprover"] != null)
            //{
            //    Utility.FunShowAlertMsg(this, "Select Approvers in Approver grid");
            //    return;
            //}
            if (dtClosureApproval != null)
            {
                if (((DataTable)(ViewState["dtTempClosureApprover"])) != null)
                {
                    DataRow[] dArray = ((DataTable)(ViewState["dtTempClosureApprover"])).Select("SNo='" + intSNo + "' and ApprovEntity<>'' and Location<>'' and ApprovPerson<>''");
                    if (dArray.Length != (Convert.ToInt32(txtFClosureApproval.Text)))
                    {
                        //Utility.FunShowAlertMsg(this, "No. of Approvers selected does not match with No.of Approvals");
                        Utility.FunShowAlertMsg(this, "Select Approvers in Approver grid");
                        //txtFClosureApproval.Text = "";
                        return;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Assign the Approvers");
                    return;
                }
            }

            dRow = dtClosureApproval.NewRow();
            dRow["SNo"] = (Convert.ToInt32(lbl.Text) + 1).ToString();
            dRow["ClosureID"] = 0;
            dRow["Down_Payment"] = txtFDownPayment.Text;
            dRow["Month_Of_Closure"] = txtFMonthClosure.Text;
            dRow["No_of_Approval"] = txtFClosureApproval.Text;
            //  dRow["Active"] = chkFClosureActive.Checked = true;
            dtClosureApproval.Rows.Add(dRow);
            grvClosureDtls.DataSource = dtClosureApproval;
            grvClosureDtls.DataBind();
            ViewState["ClosureDetails"] = dtClosureApproval;
            // FunProClosureApprover();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddl.Parent.Parent;

            Label lblApprovalEntity = gvRow.FindControl("lblApprovalEntity") as Label;
            DropDownList ddlApprovalEntity = gvRow.FindControl("ddlApprovalEntity") as DropDownList;
            DropDownList ddlApprovPerson = gvRow.FindControl("ddlApprovPerson") as DropDownList;
            DropDownList ddlLocation = gvRow.FindControl("ddlLocation") as DropDownList;
            if (ddlApprovalEntity.SelectedValue == "1")
            {
                ddlApprovPerson.BindDataTable((DataTable)ViewState["Designation"]);
                ddlLocation.Enabled = false;
                ddlLocation.SelectedValue = "0";
                ddlApprovPerson.Enabled = true;
            }
            else if (ddlApprovalEntity.SelectedValue == "2")
            {
                // FunPubGetUserDetails();
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("@COMPANY_ID", intCompanyId.ToString());
                dictParam.Add("@Location_ID", ddlLocation.SelectedValue);
                dictParam.Add("@Mode", strMode);
                dictParam.Add("@PROGRAM_ID", Convert.ToString(intprogramId));
                DataTable dt = new DataTable();
                dt = Utility.GetDefaultData("S3G_ORG_GETUSERNAME", dictParam);
                ddlApprovPerson.FillDataTable(dt, "Value", "Name");
                ViewState["UserName"] = dt;
                ddlLocation.Enabled = true;
                ddlApprovPerson.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void ddlLocation_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow gvRow = (GridViewRow)ddl.Parent.Parent;

        Label lblApprovalEntity = gvRow.FindControl("lblApprovalEntity") as Label;
        DropDownList ddlApprovalEntity = gvRow.FindControl("ddlApprovalEntity") as DropDownList;
        DropDownList ddlApprovPerson = gvRow.FindControl("ddlApprovPerson") as DropDownList;
        DropDownList ddlLocation = gvRow.FindControl("ddlLocation") as DropDownList;
        if (ddlApprovalEntity.SelectedValue == "1")
        {
            ddlApprovPerson.BindDataTable((DataTable)ViewState["Designation"]);
            ddlLocation.Enabled = false;
            ddlLocation.SelectedValue = "0";
            ddlApprovPerson.Enabled = true;
        }
        else if (ddlApprovalEntity.SelectedValue == "2")
        {
            // FunPubGetUserDetails();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@COMPANY_ID", intCompanyId.ToString());
            dictParam.Add("@Location_ID", ddlLocation.SelectedValue);
            dictParam.Add("@Mode", strMode);
            dictParam.Add("@PROGRAM_ID", Convert.ToString(intprogramId));
            DataTable dt = new DataTable();
            dt = Utility.GetDefaultData("S3G_ORG_GETUSERNAME", dictParam);
            ddlApprovPerson.FillDataTable(dt, "Value", "Name");
            ViewState["UserNameClosure"] = dt;
            ddlLocation.Enabled = true;
            ddlApprovPerson.Enabled = true;
        }
        else
        {
            ddlLocation.SelectedValue = "0";
        }
    }
    private void FunPriClearViewState()
    {
        try
        {
            //ViewState["ApprovalEntity"] = null;
            //ViewState["Designation"] = null;
            //ViewState["UserName"] = null;
            //ViewState["Location"] = null;
            ViewState["dtTempCollectionApprover"] = null;
            ViewState["dtTempClosureApprover"] = null;
            //ViewState["ApprovalClosureEntity"] = null;
            //ViewState["DesignationClosure"] = null;
            //ViewState["UserName"] = null;
            //ViewState["LocationClosure"] = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void txtEffectiveFromDate_TextChanged(object sender, EventArgs e)
    {
        //if (ddlComplainceType.SelectedValue == "3" || ddlComplainceType.SelectedValue == "4" || ddlComplainceType.SelectedValue == "5")
        //{
        //    PnlClosure.Visible = false;
        //    pnlCollectionDtls.Visible = false;
        //    PnlApprover.Visible = false;
        //    PnlApprover1.Visible = false;
        //}
        //else if (ddlComplainceType.SelectedValue == "1")
        //{
        //    PnlClosure.Visible = false;
        //    pnlCollectionDtls.Visible = true;
        //    PnlApprover.Visible = true;
        //    PnlApprover1.Visible = false;
        //}
        //else if (ddlComplainceType.SelectedValue == "2")
        //{
        //    PnlClosure.Visible = true;
        //    pnlCollectionDtls.Visible = false;
        //    PnlApprover.Visible = false;
        //    PnlApprover1.Visible = true;
        //}
        //else
        //{
        //    PnlClosure.Visible = false;
        //    pnlCollectionDtls.Visible = false;
        //    PnlApprover.Visible = false;
        //    PnlApprover1.Visible = false;

        //}
        txtEffectiveFromDate.Focus();
    }
    protected void txtEffectiveToDate_TextChanged(object sender, EventArgs e)
    {
        //if (ddlComplainceType.SelectedValue == "3" || ddlComplainceType.SelectedValue == "4" || ddlComplainceType.SelectedValue == "5")
        //{
        //    PnlClosure.Visible = false;
        //    pnlCollectionDtls.Visible = false;
        //    PnlApprover.Visible = false;
        //    PnlApprover1.Visible = false;
        //}
        //else if (ddlComplainceType.SelectedValue == "1")
        //{
        //    PnlClosure.Visible = false;
        //    pnlCollectionDtls.Visible = true;
        //    PnlApprover.Visible = true;
        //    PnlApprover1.Visible = false;
        //}
        //else if (ddlComplainceType.SelectedValue == "2")
        //{
        //    PnlClosure.Visible = true;
        //    pnlCollectionDtls.Visible = false;
        //    PnlApprover.Visible = false;
        //    PnlApprover1.Visible = true;
        //}
        //else
        //{
        //    PnlClosure.Visible = false;
        //    pnlCollectionDtls.Visible = false;
        //    PnlApprover.Visible = false;
        //    PnlApprover1.Visible = false;

        //}
        txtEffectiveToDate.Focus();
    }
    protected void txtFNoofApproval_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Label lbl = (Label)grvCollectionDtls.Rows[grvCollectionDtls.Rows.Count - 1].FindControl("lblCollectionSNo");
            int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
            if (ViewState["dtTempCollectionApprover"] == null)
            {
                ViewState["dtTempCollectionApprover"] = FunProInitializePopup();
            }
            DataTable dt = (DataTable)ViewState["dtTempCollectionApprover"];
            DataRow[] dr = dt.Select("SNo='" + intSNo + "'");
            if (dr.Length > 0)
            {
                foreach (DataRow drrow in dr)
                {
                    drrow.Delete();
                    dt.AcceptChanges();
                }
                ViewState["dtTempCollectionApprover"] = dt;
            }

        }
        catch (Exception EX)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(EX);
        }
    }
    protected void txtFClosureApproval_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Label lbl = (Label)grvClosureDtls.Rows[grvClosureDtls.Rows.Count - 1].FindControl("lblClosureSNo");
            int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
            if (ViewState["dtTempClosureApprover"] == null)
            {
                ViewState["dtTempClosureApprover"] = FunProInitializePopup();
            }
            DataTable dt = (DataTable)ViewState["dtTempClosureApprover"];
            DataRow[] dr = dt.Select("SNo='" + intSNo + "'");
            if (dr.Length > 0)
            {
                foreach (DataRow drrow in dr)
                {
                    drrow.Delete();
                    dt.AcceptChanges();
                }
                ViewState["dtTempClosureApprover"] = dt;
            }

        }
        catch (Exception EX)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(EX);
        }
    }
}