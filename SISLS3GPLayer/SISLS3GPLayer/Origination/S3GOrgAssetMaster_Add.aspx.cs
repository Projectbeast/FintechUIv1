#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Asset Master
/// Created By			: Nataraj Y
/// Created Date		: 29-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 31-May-2010
/// Reason              : Asset Master
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 01-June-2010
/// Reason              : Asset Master
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 21-June-2010
/// Reason              : Asset Master 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 02-Jul-2010
/// Reason              : Table change and grid implementation
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 23-Jul-2010
/// Reason              : Review Comments implementation

/// Last Updated By	    : Thangam M, Manikandan R, Saran M
/// Last Updated Date   : 10-09-2010
/// Reason              : Bug fixing for the follwing Test cases: 
///                         AH-010,AH-005,TC-140,TC-064,TC-075
///                         
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 11-Nov-2010
/// Reason              : Bug Fixing
/// 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 01-Dec-2010
/// Reason              : Bug Fixing Round 3
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using S3GBusEntity;
using System.Globalization;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.Text;

#endregion

public partial class Origination_S3G_OrgAssetMaster_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyId = 0;
    int intUserId = 0;
    int intAssetId = 0;
    int intAsset_CatId = 0;
    string strCategoryType = string.Empty;
    int intErrorCode = 0;
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryDataTable ObjS3G_ORG_AssetCategoryDataTable;
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable ObjS3G_ORG_AssetMasterDataTable;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgAssetMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgAssetMaster_Add.aspx';";
    string strRedirectPage = "~/Origination/S3GOrgAssetMaster_View.aspx";
    UserInfo ObjUserInfo;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    bool IsEnableLOB = true;
    bool IsLOBCalled = false;
    string strDateFormat = string.Empty;
    S3GSession ObjS3GSession = new S3GSession();
    StringBuilder strbLOBDet = new StringBuilder();
    //Code end


    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            //Code end

            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString["qsAssetId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsAssetId"));
                string[] arrayIds = fromTicket.Name.Split(',');
                intAssetId = Convert.ToInt32(arrayIds[0].ToString());
                /******Commeted by Nataraj Y on 17 -05-2011 for new Asset code****/
                //ddlAssetType.SelectedValue = arrayIds[1].ToString();
            }
            else
            {
                if (Request.QueryString["qsAssetCatId"] != null)
                {
                    FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsAssetCatId"));
                    /******Commeted by Nataraj Y on 17 -05-2011 for new Asset code****/
                    //string[] arrayIds = fromTicket.Name.Split(',');
                    intAsset_CatId = Convert.ToInt32(fromTicket.Name);
                    //ddlAssetType.SelectedValue = arrayIds[1].ToString();
                    strCategoryType = Request.QueryString["qsCatType"];

                }
            }


            if (!IsPostBack)
            {
                FunProLoadCombos();
                //rfvBookDep.Enabled = false;
                rfvBlockDep.Enabled = false;
                //ddlAssetType.Focus();
                chkCategoryType.SelectedIndex = 0;
                FunProIntializeData();
                txtBlockDepreciationRate.ReadOnly = true;
                txtDepreciationRate.ReadOnly = true;
                txtGuideLineValue.SuffixInner(12, 3, true, false, "Guideline Limit");

                CalendarValidFrom.Format = ObjS3GSession.ProDateFormatRW;
                CalendarValidTo.Format = ObjS3GSession.ProDateFormatRW;
                txtValidFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtValidFrom.ClientID + "','" + ObjS3GSession.ProDateFormatRW + "',false,  true);");
                txtValidTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtValidTo.ClientID + "','" + ObjS3GSession.ProDateFormatRW + "',false,  true);");
                ddlAssetType.Focus();
                funPriLoadCommonLookup();

                txtCode.Attributes.Add("autocomplete", "off");
                txtCodeDescription.Attributes.Add("autocomplete", "off");

                if (intAssetId > 0)
                {
                    FunPubProGetAssetDetails(intCompanyId, intAssetId, null);
                    if (strMode == "M")
                    {
                        FunPriAssetControlStatus(1);
                        btnAssetGo.CssClass = "cancel_btn fa fa-times";
                        btnAssetCategoryClear.Enabled_True();
                    }
                    if (strMode == "Q")
                    {
                        FunPriAssetControlStatus(-1);
                        btnAssetGo.CssClass = "cancel_btn fa fa-times";
                        //btnAssetSubmit.CssClass = "cancel_btn fa fa-times";
                        btnAssetCategoryClear.Enabled_True();

                    }
                }

                else if (intAsset_CatId > 0)
                {

                    FunPubLoadAssetCategories(intCompanyId, intAsset_CatId, Convert.ToInt32(ddlAssetType.SelectedValue), strCategoryType);
                    if (strMode == "M")
                    {

                        btnCategoryGo.Attributes.Add("disabled", "disabled");
                        //btnCategoryGo.Attributes.Add("class", "css_btn_disabled");  // enab
                        FunPriAssetControlStatus(1);
                        //btnCategoryGo.CssClass = "cancel_btn fa fa-times";
                        btnAssetCategoryClear.Enabled_False();
                    }
                    if (strMode == "Q")
                    {
                        //btnCategoryGo.Enabled = false;
                        btnCategoryGo.Attributes.Add("disabled", "disabled");
                        //btnCategoryGo.Attributes.Add("class", "css_btn_disabled");  // enab

                        FunPriAssetControlStatus(-1);
                        //btnCategoryGo.CssClass = "cancel_btn fa fa-times";
                        btnAssetCategoryClear.Enabled_False();
                        //btnCategorySubmit.CssClass = "cancel_btn fa fa-times";
                    }

                }
                else
                {
                    FunPriAssetControlStatus(0);
                }


            }
            FunPriSetMaxLength();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region Page Events


    private void funPriLoadLineofBusiness()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            //if (strMode == "C")
            //{
            Procparam.Add("@User_Id", intUserId.ToString());
            //}
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@PROGRAM_ID", "22");
            Procparam.Add("@Asset_Cat_Id", ddlAssetType.SelectedValue);
            DataTable dt = Utility.GetDefaultData(SPNames.LOBMaster, Procparam);
            grvLOB.DataSource = dt;
            grvLOB.DataBind();
            if (dt.Rows.Count == 0)
                lblLineofBusiness.Visible = false;
            else
                lblLineofBusiness.Visible = true;


            //ddlLob.BindDataTable(SPNames.LOBMaster, Procparam,true,"--Select--", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //funPriLoadCommonLookup();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    private void funPriLoadCommonLookup()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Lookup_Type", "ASSET_PURPOSE");
            Procparam.Add("@Table_Name", "S3G_ORG_LOOKUP");

            ddlPurpose.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, true, "--Select--", new string[] { "LOOKUP_CODE", "DESCRIPTION" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    protected void chkCategoryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssetType.SelectedIndex > 0)
            {
                if (txtCode.Text.Trim().Length > 0 || txtCodeDescription.Text.Trim().Length > 0)
                {
                    // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alert", "if(confirm('Are you sure,you want to proceed?')){return true;}else{return false;}",true);
                    // ScriptManager.RegisterOnSubmitStatement(this,this.GetType(), "Alert", "<script>if(confirm('Are you sure,you want to proceed?')){return true;}else{return false;}</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Category code and description are not empty')", true);
                    tcCode.ActiveTab = tcCode.Tabs[tcCode.ActiveTabIndex];
                    return;

                }
                txtCode.Text = "";
                txtCodeDescription.Text = "";

            }
            else if (ddlAssetType.SelectedIndex == 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Asset Type");
                lblCode.Text = "Asset Class Code";
                lblCodeDesc.Text = "Asset Class Code Description";
                lblLastGenCode.Text = "";
                //FunProDisableTabs(0);                                
                tcCode.ActiveTab = tpClassCode;
            }
        }
        catch (Exception)
        {
        }
        //finally
        //{
        //    if (ObjOrgMasterMgtServicesClient != null)
        //        ObjOrgMasterMgtServicesClient.Close();
        //}
    }

    protected void grvAsset_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtDelete;
        switch (tcCode.ActiveTab.HeaderText)
        {
            case "Class Code":
                dtDelete = (DataTable)ViewState["ClassTable"];
                dtDelete.Rows.RemoveAt(e.RowIndex);
                if (dtDelete.Rows.Count <= 0)
                {
                    grvAssetClass.DataSource = null;
                }
                else
                {
                    grvAssetClass.DataSource = dtDelete;
                }
                grvAssetClass.DataBind();
                ViewState["ClassTable"] = dtDelete;
                break;
            case "Make Code":

                dtDelete = (DataTable)ViewState["MakeTable"];
                dtDelete.Rows.RemoveAt(e.RowIndex);
                if (dtDelete.Rows.Count <= 0)
                {
                    grvAssetMake.DataSource = null;
                }
                else
                {
                    grvAssetMake.DataSource = dtDelete;
                }
                grvAssetMake.DataBind();
                ViewState["MakeTable"] = dtDelete;
                break;
            case "Type Code":

                dtDelete = (DataTable)ViewState["TypeTable"];
                dtDelete.Rows.RemoveAt(e.RowIndex);
                if (dtDelete.Rows.Count <= 0)
                {
                    grvAssetType.DataSource = null;
                }
                else
                {
                    grvAssetType.DataSource = dtDelete;
                }
                grvAssetType.DataBind();
                ViewState["TypeTable"] = dtDelete;
                break;
            case "Model Code":

                dtDelete = (DataTable)ViewState["ModelTable"];
                dtDelete.Rows.RemoveAt(e.RowIndex);
                if (dtDelete.Rows.Count <= 0)
                {
                    grvAssetModel.DataSource = null;
                }
                else
                {
                    grvAssetModel.DataSource = dtDelete;
                }
                grvAssetModel.DataBind();
                ViewState["ModelTable"] = dtDelete;
                break;
        }
    }
    private bool funPriCheckLob()
    {
        bool isAtleastOneLOBSelected = false;
        strbLOBDet.Append("<Root>");
        string strLOBID = string.Empty;
        foreach (GridViewRow grvData in grvLOB.Rows)
        {

            CheckBox chkLOB = ((CheckBox)grvData.FindControl("chkSelectLOB"));
            if (chkLOB.Checked)
            {
                isAtleastOneLOBSelected = true;
                strLOBID = ((Label)grvData.FindControl("lblLOBID")).Text;
                strbLOBDet.Append(" <Details LOB_ID='" + strLOBID + "' /> ");
            }
        }
        strbLOBDet.Append("</Root>");

        return isAtleastOneLOBSelected;

    }
    protected void btnAssetSubmit_Click(object sender, EventArgs e)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            txtValidFrom_TextChanged(null, null);
            txtValidTo_TextChanged(null, null);

            //DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
            //dtformat.ShortDatePattern = "MM/dd/yy";

            //if ((!(string.IsNullOrEmpty(txtValidFrom.Text))) &&
            //   (!(string.IsNullOrEmpty(txtValidTo.Text))))                                   // If start and end date is not empty
            //{
            //    if (Utility.StringToDate(txtValidFrom.Text) > Utility.StringToDate(txtValidTo.Text)) // start date should be less than or equal to the enddate
            //    {
            //        if (hidDate.Value.ToUpper() == "START")
            //            Utility.FunShowAlertMsg(this, "Valid From Date should be lesser than the Valid To Date");
            //        else
            //            Utility.FunShowAlertMsg(this, "Valid To date should be greater than the Valid From Date");
            //        return;
            //    }
            //}




            if (!funPriCheckLob())
            {
                Utility.FunShowAlertMsg(this.Page, "Atleast one Line of Business should be selected");
                return;
            }



            ObjS3G_ORG_AssetMasterDataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable();
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterRow ObjAssetMasterRow;
            ObjAssetMasterRow = ObjS3G_ORG_AssetMasterDataTable.NewS3G_ORG_AssetMasterRow();
            int intClass_Id = 0;
            int intMake_Id = 0;
            int intType_Id = 0;
            int intModel_Id = 0;
            if (intAssetId == 0)
            {


                if (ViewState["Classid"] != null)
                {
                    intClass_Id = Convert.ToInt32(ViewState["Classid"].ToString());
                }
                else
                    intClass_Id = 0;

                if (ViewState["Makeid"] != null)
                {
                    intMake_Id = Convert.ToInt32(ViewState["Makeid"].ToString());
                }
                else
                    intMake_Id = 0;

                if (ViewState["Typeid"] != null)
                {
                    intType_Id = Convert.ToInt32(ViewState["Typeid"].ToString());
                }
                else
                    intType_Id = 0;

                if (ViewState["Modelid"] != null)
                {
                    intModel_Id = Convert.ToInt32(ViewState["Modelid"].ToString());
                }
                else
                    intModel_Id = 0;

                ObjAssetMasterRow.Class_ID = intClass_Id;
                ObjAssetMasterRow.Make_ID = intMake_Id;
                ObjAssetMasterRow.Type_ID = intType_Id;
                ObjAssetMasterRow.Model_ID = intModel_Id;
            }


            ///Validation Start
            ///
            if (intAssetId == 0)
            {
                DataTable dt = (DataTable)ViewState["ASSETLOBCATMAP"];
                DataRow[] dr = dt.Select("ASSET_CATEGORY_ID = " + ddlAssetType.SelectedValue);

                if (dr.Length > 0)
                {
                    if (dr[0].ItemArray[0].ToString() == "0")
                    {
                        //tcAssetcode.Tabs[0].Enabled = false;
                    }
                    else
                    {
                        if (intClass_Id == 0)
                        {
                            Utility.FunShowAlertMsg(this, "Select At least one Class");
                            return;
                        }
                        //tcAssetcode.Tabs[0].Enabled = true;
                    }
                    if (dr[0].ItemArray[1].ToString() == "0")
                    {

                        //tcAssetcode.Tabs[1].Enabled = false;
                    }
                    else
                    {
                        if (intMake_Id == 0)
                        {
                            Utility.FunShowAlertMsg(this, "Select At least one Make");
                            return;
                        }
                        //tcAssetcode.Tabs[1].Enabled = true;
                    }
                    if (dr[0].ItemArray[2].ToString() == "0")
                    {

                        //tcAssetcode.Tabs[2].Enabled = false;
                    }
                    else
                    {
                        if (intType_Id == 0)
                        {
                            Utility.FunShowAlertMsg(this, "Select At least one Type");
                            return;
                        }
                        //tcAssetcode.Tabs[2].Enabled = true;
                    }
                    if (dr[0].ItemArray[3].ToString() == "0")
                    {

                        //tcAssetcode.Tabs[3].Enabled = false;
                    }
                    else
                    {
                        if (intModel_Id == 0)
                        {
                            Utility.FunShowAlertMsg(this, "Select At least one Model");
                            return;
                        }
                        //tcAssetcode.Tabs[3].Enabled = true;
                    }

                    if (dr[0].ItemArray[5].ToString() == "0")//Purpose
                    {
                        //ddlPurpose.Enabled = false;
                        //rfvddlPurpose.Enabled = false;
                        //rfvddlPurpose.CssClass = "styleDisplayLabel";
                    }
                    else
                    {
                        //ddlPurpose.Enabled = true;
                        //rfvddlPurpose.Enabled = true;
                        //rfvddlPurpose.CssClass = "styleReqFieldLabel";
                    }
                }

            }





            //Validation End



            ObjAssetMasterRow.Company_ID = intCompanyId;
            ObjAssetMasterRow.Created_By = intUserId;
            ObjAssetMasterRow.Modified_By = intUserId;
            ObjAssetMasterRow.Asset_Code = txtAssetCode.Text.Trim();
            ObjAssetMasterRow.Asset_Description = txtAssetCodeDesc.Text.Trim();
            ObjAssetMasterRow.Asset_Master_ID = intAssetId;


            //New Fields added for MFC Customization Strat
            ObjAssetMasterRow.Purpose_Id = Convert.ToInt32(ddlPurpose.SelectedValue);
            ObjAssetMasterRow.Lob_Id = strbLOBDet.ToString();
            ObjAssetMasterRow.Valid_From = Utility.StringToDate(txtValidFrom.Text).ToString();
            ObjAssetMasterRow.Valid_To = Utility.StringToDate(txtValidTo.Text).ToString();

            /******Code here added by Nataraj Y on 17 -05-2011 for new Asset code****/
            ObjAssetMasterRow.AssetType_ID = Convert.ToInt32(ddlAssetType.SelectedValue);

            if (txtBlockDepreciationRate.Text != String.Empty)
            {
                ObjAssetMasterRow.Block_Depreciation_Category = cmbBlockDepreciationCategory.Text;
            }
            if (txtBlockDepreciationRate.Text != String.Empty)
            {
                ObjAssetMasterRow.Block_Depreciation_Rate = Convert.ToDecimal(txtBlockDepreciationRate.Text);
            }
            if (txtDepreciationRate.Text != String.Empty)
            {
                ObjAssetMasterRow.Book_Depreciation_Category = cmbDepreciationCategory.Text;
            }
            if (txtDepreciationRate.Text != String.Empty)
            {
                ObjAssetMasterRow.Book_Depreciation_Rate = Convert.ToDecimal(txtDepreciationRate.Text);
            }
            if (txtGuideLineValue.Text != String.Empty)
            {
                ObjAssetMasterRow.GuideLine_Value = Convert.ToDecimal(txtGuideLineValue.Text);
            }

            ObjAssetMasterRow.IS_Active = chkActive.Checked;
            ObjS3G_ORG_AssetMasterDataTable.AddS3G_ORG_AssetMasterRow(ObjAssetMasterRow);

            if (ObjS3G_ORG_AssetMasterDataTable.Rows.Count > 0)
            {
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_ORG_AssetMasterDataTable = ClsPubSerialize.Serialize(ObjS3G_ORG_AssetMasterDataTable, SerMode);
                //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                if (intAssetId == 0)
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateAssetCodeInt(SerMode, byteobjS3G_ORG_AssetMasterDataTable);
                }
                if (intAssetId > 0)
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubModifyAssetCodeInt(SerMode, byteobjS3G_ORG_AssetMasterDataTable);
                }
                if (intAssetId > 0)
                {
                    switch (intErrorCode)
                    {
                        case 0:
                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnAssetSubmit.Enabled_False();
                            //End here

                            strAlert = strAlert.Replace("__ALERT__", "Asset code updated successfully");
                            break;
                        case 1:
                            strAlert = strAlert.Replace("__ALERT__", "Asset code doesnot exists");
                            strRedirectPageView = "";
                            break;
                        case 2:
                            strAlert = strAlert.Replace("__ALERT__", "Asset Description already exists");
                            strRedirectPageView = "";
                            break;
                        default:
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating asset code");
                            strRedirectPageView = "";
                            break;
                    }
                }
                else
                {
                    switch (intErrorCode)
                    {
                        case 0:

                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnAssetSubmit.Enabled_False();
                            //End here

                            string strType = Request.QueryString.Get("Type").ToString();
                            strRedirectPageAdd = "window.location.href='../Origination/S3GOrgAssetMaster_Add.aspx?qsMode=C&Type=" + strType + "'";
                            strAlert = "Asset code added successfully";
                            strAlert += @"\n\nWould you like to add one more Asset?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            break;
                        case 1:
                            strAlert = strAlert.Replace("__ALERT__", "Asset code already exists");
                            strRedirectPageView = "";
                            break;
                        case 2:
                            strAlert = strAlert.Replace("__ALERT__", "Asset Description already exists");
                            strRedirectPageView = "";
                            break;
                        default:
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding asset code");
                            strRedirectPageView = "";
                            break;
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            // if (ObjOrgMasterMgtServicesClient != null)
            ObjOrgMasterMgtServicesClient.Close();

        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3gOrgAssetMaster_View.aspx");
    }

    private void FunPriLoadDepricationRate()
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        DataSet dsDepricationRate = new DataSet();
        Procparam.Add("@ASSET_CATEGORY_ID", ddlAssetType.SelectedValue);
        //ddlAssetType.Items.Clear();
        dsDepricationRate = Utility.GetDataset("S3G_ORG_DEPRICIATIONRATE", Procparam);
        if (dsDepricationRate.Tables[0] != null && dsDepricationRate.Tables[0].Rows.Count > 0)
        {
            txtDepreciationRate.Text = dsDepricationRate.Tables[0].Rows[0]["BOOK_DEPRECIATION_RATE"].ToString();
        }
        else
        {
            txtDepreciationRate.Text = "";
        }
    }

    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {

        //btnAssetClear_ServerClick(null, null);
        /******added by Nataraj Y on 17 -05-2011 for new Asset code****/
        funPriRestAssetCategory();
        string[] strArray = ddlAssetType.SelectedItem.Text.Split('-');
        ViewState["AssetType"] = strArray[0].ToString().Trim();
        //FunPriLoadDepricationRate();
        //FunPriGetAssetCode();
        ddlAssetType.Focus();
        if (ViewState["ASSETLOBCATMAP"] != null)
        {
            DataTable dt = (DataTable)ViewState["ASSETLOBCATMAP"];
            DataRow[] dr = dt.Select("ASSET_CATEGORY_ID = " + ddlAssetType.SelectedValue);

            if (dr.Length > 0)
            {
                if (dr[0].ItemArray[0].ToString() == "0")
                {
                    tcAssetcode.Tabs[0].Enabled = false;
                }
                else
                {
                    tcAssetcode.Tabs[0].Enabled = true;
                }
                if (dr[0].ItemArray[1].ToString() == "0")
                {
                    //tcAssetcode.Tabs.Remove(tpAssetclass);
                    tcAssetcode.Tabs[1].Enabled = false;
                }
                else
                {
                    tcAssetcode.Tabs[1].Enabled = true;
                }
                if (dr[0].ItemArray[2].ToString() == "0")
                {
                    //tcAssetcode.Tabs.Remove(tpAssetMake);
                    tcAssetcode.Tabs[2].Enabled = false;
                }
                else
                {
                    tcAssetcode.Tabs[2].Enabled = true;
                }
                if (dr[0].ItemArray[3].ToString() == "0")
                {
                    //tcAssetcode.Tabs.Remove(tpAssetType);
                    tcAssetcode.Tabs[3].Enabled = false;
                }
                else
                {
                    tcAssetcode.Tabs[3].Enabled = true;
                }
                //if (dr[0].ItemArray[4].ToString() == "0")
                //{
                //    //tcAssetcode.Tabs.Remove(tpAssetModel);
                //    tcAssetcode.Tabs[4].Enabled = false;
                //}
                //else
                //{
                //    tcAssetcode.Tabs[4].Enabled = true;
                //}
                if (dr[0].ItemArray[5].ToString() == "0")//Purpose
                {
                    ddlPurpose.Enabled = false;
                    rfvddlPurpose.Enabled = false;
                    rfvddlPurpose.CssClass = "styleDisplayLabel";
                }
                else
                {
                    ddlPurpose.Enabled = true;
                    rfvddlPurpose.Enabled = true;
                    rfvddlPurpose.CssClass = "styleReqFieldLabel";
                }
            }
        }
        funPriLoadLineofBusiness();
        FunPriLoadDepricationRate();

    }

    protected void btnCategoryGo_Click(object sender, EventArgs e)
    {

        try
        {
            DataRow drClass;
            DataRow drMake;
            DataRow drType;
            DataRow drModel;

            DataTable dtCommon;

            try
            {
                if (!txtCode.Text.CheckZeroValidate())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Class", "alert('Enter a valid Code');", true);
                    txtCode.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {

            }
            // switch (chkCategoryType.SelectedItem.Text)
            switch (tcCode.ActiveTab.HeaderText)
            {

                //case "Class":
                case "Class Code":
                    dtCommon = (DataTable)ViewState["ClassTable"];
                    if (!FunPriValidCategoryDuplicate(txtCode.Text, txtCodeDescription.Text, "Class"))
                    {
                        return;
                    }
                    if (dtCommon.Rows.Count < 10)
                    {
                        drClass = dtCommon.NewRow();
                        drClass["Code"] = txtCode.Text.Trim();
                        drClass["Description"] = txtCodeDescription.Text.Trim();
                        drClass["CategoryType"] = "CLASS";
                        dtCommon.Rows.Add(drClass);
                        grvAssetClass.DataSource = dtCommon;
                        grvAssetClass.DataBind();
                        ViewState["ClassTable"] = dtCommon;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Class", "alert('Cannot add more that 10 Rows');", true);
                    }
                    break;
                //case "Make":
                case "Make Code":
                    dtCommon = (DataTable)ViewState["MakeTable"];
                    if (!FunPriValidCategoryDuplicate(txtCode.Text, txtCodeDescription.Text, "Make"))
                    {
                        return;
                    }
                    if (dtCommon.Rows.Count < 10)
                    {
                        drMake = dtCommon.NewRow();
                        drMake["Code"] = txtCode.Text.Trim();
                        drMake["Description"] = txtCodeDescription.Text.Trim();
                        drMake["CategoryType"] = "MAKE";
                        dtCommon.Rows.Add(drMake);
                        grvAssetMake.DataSource = dtCommon;
                        grvAssetMake.DataBind();
                        ViewState["MakeTable"] = dtCommon;

                    }

                    break;
                //case "Type":
                case "Type Code":
                    dtCommon = (DataTable)ViewState["TypeTable"];
                    if (!FunPriValidCategoryDuplicate(txtCode.Text, txtCodeDescription.Text, "Type"))
                    {
                        return;
                    }
                    if (dtCommon.Rows.Count < 10)
                    {
                        drType = dtCommon.NewRow();
                        drType["Code"] = txtCode.Text.Trim();
                        drType["Description"] = txtCodeDescription.Text.Trim();
                        drType["CategoryType"] = "TYPE";
                        dtCommon.Rows.Add(drType);
                        grvAssetType.DataSource = dtCommon;
                        grvAssetType.DataBind();
                        ViewState["TypeTable"] = dtCommon;

                    }

                    break;
                //case "Model":
                case "Model Code":
                    dtCommon = (DataTable)ViewState["ModelTable"];
                    if (!FunPriValidCategoryDuplicate(txtCode.Text, txtCodeDescription.Text, "Model"))
                    {
                        return;
                    }
                    if (dtCommon.Rows.Count < 10)
                    {
                        drModel = dtCommon.NewRow();
                        drModel["Code"] = txtCode.Text.Trim();
                        drModel["Description"] = txtCodeDescription.Text.Trim();
                        drModel["CategoryType"] = "MODEL";
                        dtCommon.Rows.Add(drModel);
                        grvAssetModel.DataSource = dtCommon;
                        grvAssetModel.DataBind();
                        ViewState["ModelTable"] = dtCommon;

                    }
                    break;

            }
            txtCode.Text = "";
            txtCodeDescription.Text = "";
            btnCategorySubmit.Enabled_True();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Code"))
            {
                //Utility.FunShowAlertMsg(this, "Asset Category Code already exists in the grid");
                Utility.FunShowAlertMsg(this, "Asset " + tcCode.ActiveTab.HeaderText + " already exists in the grid");
            }
            if (ex.Message.Contains("Description"))
            {
                Utility.FunShowAlertMsg(this, "Asset " + tcCode.ActiveTab.HeaderText.Replace("Code", "") + " Description already exists in the grid");
            }
        }
    }

    private bool FunPriValidCategoryDuplicate(string strCategoryCode, string strCategoryDesc, string strCategoryType)
    {
        Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
        dictProcParam.Add("@Company_ID", intCompanyId.ToString());
        dictProcParam.Add("@Code", strCategoryCode);
        dictProcParam.Add("@Description", strCategoryDesc);
        dictProcParam.Add("@CategoryType", strCategoryType);
        DataTable dtDupliacteTable = Utility.GetDefaultData("S3G_ORG_CheckDuplicateAssetCategory", dictProcParam);
        if (dtDupliacteTable.Rows[0].ItemArray[0].ToString() == "1")
        {
            Utility.FunShowAlertMsg(this, strCategoryType + " Code already exists.Enter a new " + strCategoryType + " Code");
            return false;
        }
        if (dtDupliacteTable.Rows[0].ItemArray[0].ToString() == "2")
        {
            Utility.FunShowAlertMsg(this, strCategoryType + " description already exists.Enter a new " + strCategoryType + " description");
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void btnCategorySubmit_Click(object sender, EventArgs e)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_ORG_AssetCategoryDataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryDataTable();
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryRow ObjAssetCategoryrRow;


            DataTable dtSubmit = new DataTable();
            dtSubmit.Merge((DataTable)ViewState["ClassTable"]);
            dtSubmit.Merge((DataTable)ViewState["MakeTable"]);
            dtSubmit.Merge((DataTable)ViewState["TypeTable"]);
            dtSubmit.Merge((DataTable)ViewState["ModelTable"]);
            //CHANGED BY BHUVAN FOR SAKTHI FIN

            //if (txtCode.Text == string.Empty)
            //{
            //    Utility.FunShowAlertMsg(this, rfvCode.ErrorMessage);
            //    return;
            //}
            //if (txtCodeDescription.Text == string.Empty)
            //{
            //    Utility.FunShowAlertMsg(this, rfvCodeDescription.ErrorMessage);
            //    return;
            //}

            if (PageMode == PageModes.Create)
            {
                //end here
                if (dtSubmit.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSubmit.Rows)
                    {
                        ObjAssetCategoryrRow = ObjS3G_ORG_AssetCategoryDataTable.NewS3G_ORG_AssetCategoryRow();
                        ObjAssetCategoryrRow.Company_ID = intCompanyId;
                        ObjAssetCategoryrRow.Created_By = intUserId;
                        ObjAssetCategoryrRow.Category_Type = dr["CategoryType"].ToString();
                        ObjAssetCategoryrRow.Category_Code = dr["Code"].ToString();
                        ObjAssetCategoryrRow.Category_Description = dr["Description"].ToString();
                        ObjAssetCategoryrRow.Category_ID = intAsset_CatId.ToString();
                        /******Code here removed by Nataraj Y on 17 -05-2011 for new Asset code****/
                        ObjS3G_ORG_AssetCategoryDataTable.AddS3G_ORG_AssetCategoryRow(ObjAssetCategoryrRow);
                    }
                    if (ObjS3G_ORG_AssetCategoryDataTable.Rows.Count > 0)
                    {
                        SerializationMode SerMode = SerializationMode.Binary;
                        byte[] byteobjS3G_ORG_AssetCategoryDataTable = ClsPubSerialize.Serialize(ObjS3G_ORG_AssetCategoryDataTable, SerMode);
                        //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                        int intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateAssetCategoryInt(SerMode, byteobjS3G_ORG_AssetCategoryDataTable);
                        if (intErrorCode == 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Asset category code added successfully");
                            FunProIntializeData();

                            //Added By Thangam M on 15/Feb/2012 to refresh the code after save
                            string strCategory = tcCode.ActiveTab.HeaderText;
                            FunPriGetLastGenCode(strCategory);

                        }
                        if (intErrorCode == 1)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Asset category code already exists");
                            strRedirectPageAdd = "";
                            FunProIntializeData();

                        }
                        if (intErrorCode == 2)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Asset category description already exists");
                            FunProIntializeData();
                            strRedirectPageAdd = "";

                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error adding asset category code");
                            strRedirectPageAdd = "";
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Atleast one category code should be given');", true);
                }
            }
            else
            {
                ObjAssetCategoryrRow = ObjS3G_ORG_AssetCategoryDataTable.NewS3G_ORG_AssetCategoryRow();
                ObjAssetCategoryrRow.Company_ID = intCompanyId;
                ObjAssetCategoryrRow.Created_By = intUserId;
                ObjAssetCategoryrRow.Category_Type = "";
                ObjAssetCategoryrRow.Category_Code = txtCode.Text;
                ObjAssetCategoryrRow.Category_Description = txtCodeDescription.Text;
                //chnaged by bhuvan
                ObjAssetCategoryrRow.Category_ID = intAsset_CatId.ToString();
                // end here
                /******Code here removed by Nataraj Y on 17 -05-2011 for new Asset code****/
                ObjS3G_ORG_AssetCategoryDataTable.AddS3G_ORG_AssetCategoryRow(ObjAssetCategoryrRow);

                if (ObjS3G_ORG_AssetCategoryDataTable.Rows.Count > 0)
                {
                    SerializationMode SerMode = SerializationMode.Binary;
                    byte[] byteobjS3G_ORG_AssetCategoryDataTable = ClsPubSerialize.Serialize(ObjS3G_ORG_AssetCategoryDataTable, SerMode);
                    //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                    int intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateAssetCategoryInt(SerMode, byteobjS3G_ORG_AssetCategoryDataTable);
                    if (intErrorCode == 0)
                    {

                        strAlert = strAlert.Replace("__ALERT__", "Asset description updated successfully");


                    }
                    if (intErrorCode == 10)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Asset class code description already exists ");
                        strRedirectPageView = "";
                        //FunProIntializeData();

                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            //if (ObjOrgMasterMgtServicesClient != null)
            ObjOrgMasterMgtServicesClient.Close();
        }
    }

    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void cmbDepreciationCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((cmbDepreciationCategory.Text != "" || cmbDepreciationCategory.Text != string.Empty) && (cmbDepreciationCategory.Text != "0"))
        {
            txtDepreciationRate.ReadOnly = false;
            txtDepreciationRate.Focus();
            //rfvBookDep.Enabled = true;
        }
        else
        {
            txtDepreciationRate.Text = "";
            txtDepreciationRate.ReadOnly = true;
            //rfvBookDep.Enabled = false;
        }

    }

    protected void cmbBlockDepreciationCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((cmbBlockDepreciationCategory.Text != "" || cmbBlockDepreciationCategory.Text != string.Empty) && (cmbBlockDepreciationCategory.Text != "0"))
        {
            txtBlockDepreciationRate.ReadOnly = false;
            txtBlockDepreciationRate.Focus();
            rfvBlockDep.Enabled = true;
        }
        else
        {
            txtBlockDepreciationRate.Text = "";
            txtBlockDepreciationRate.ReadOnly = true;
            rfvBlockDep.Enabled = false;
        }
    }

    protected void cmbBlockDepreciationCategory_ItemInserted(object sender, AjaxControlToolkit.ComboBoxItemInsertEventArgs e)
    {
        if ((cmbBlockDepreciationCategory.Text != "" || cmbBlockDepreciationCategory.Text != string.Empty) && (cmbBlockDepreciationCategory.Text != "0"))
        {
            rfvBlockDep.Enabled = true;
            txtBlockDepreciationRate.ReadOnly = false;
        }
        else
        {
            rfvBlockDep.Enabled = false;
            txtBlockDepreciationRate.ReadOnly = true;
        }
    }

    protected void cmbDepreciationCategory_ItemInserted(object sender, AjaxControlToolkit.ComboBoxItemInsertEventArgs e)
    {
        if ((cmbDepreciationCategory.Text != "" || cmbDepreciationCategory.Text != string.Empty) && (cmbDepreciationCategory.Text != "0"))
        {
            //rfvBookDep.Enabled = true;
            txtDepreciationRate.ReadOnly = false;
            txtDepreciationRate.Focus();
        }
        else
        {
            //rfvBookDep.Enabled = false;
            txtDepreciationRate.ReadOnly = true;
        }
    }

    //protected void txtSearchValue_TextChanged(object sender, EventArgs e)
    //{
    //    FunPriResetValues("1", string.Empty);
    //    FunPriSearch(tcAssetcode.ActiveTabIndex);
    //}

    protected void chkCategoryCode_CheckedChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((CheckBox)sender).ClientID;

        if (((CheckBox)sender).Checked)
        {
            if (strFieldAtt.Contains("grvAssetClassCodes"))
            {
                FunGridUncheckAll(grvAssetClassCodes);
            }
            if (strFieldAtt.Contains("grvAssetMakeCodes"))
            {
                FunGridUncheckAll(grvAssetMakeCodes);
            }
            if (strFieldAtt.Contains("grvAssetTypeCodes"))
            {
                FunGridUncheckAll(grvAssetTypeCodes);
            }
            if (strFieldAtt.Contains("grvAssetModelCodes"))
            {
                FunGridUncheckAll(grvAssetModelCodes);
            }
            ((CheckBox)sender).Checked = true;
            FunPriFormAssetCode(strFieldAtt);

        }
        else
        {
            FunPriResetValues("0", strFieldAtt);
        }
    }

    protected void btnGetCatdegoryDetails_Click(object sender, EventArgs e)
    {

        string strCategory = tcCode.ActiveTab.HeaderText;
        FunPriGetLastGenCode(strCategory);
    }

    protected void imgbtnClassSearch_Click(object sender, ImageClickEventArgs e)
    {
        FunPriResetValues("0", "grvAssetClassCodes");
        FunPriSearch("Class");
        txtClassSearchValue.Focus();

    }

    protected void imgbtnMakeSearch_Click(object sender, ImageClickEventArgs e)
    {
        FunPriResetValues("0", "grvAssetMakeCodes");
        FunPriSearch("Make");
        txtMakeSearchValue.Focus();
        // ((TextBox)sender).Focus();
    }

    protected void imgbtnTypeSearch_Click(object sender, ImageClickEventArgs e)
    {
        FunPriResetValues("0", "grvAssetTypeCodes");
        FunPriSearch("Type");
        txtTypeSearchValue.Focus();
        //((TextBox)sender).Focus();
    }

    protected void imgbtnModelSearch_Click(object sender, ImageClickEventArgs e)
    {
        FunPriResetValues("0", "grvAssetModelCodes");
        FunPriSearch("Model");
        txtModelSearchValue.Focus();
        // ((TextBox)sender).Focus();
    }
    #endregion

    #region Page Methods


    /// <summary>
    /// 
    /// </summary>
    /// added by S.Kannan
    //    private void FunPriAssetTypeChanged()
    //    {
    ////        FunPubLoadAssetCategories(intCompanyId, null, Convert.ToInt32(ddlAssetType.SelectedItem.Value), tcCode.ActiveTab.HeaderText);
    //        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
    //        lblLastGenCode.Text = ObjOrgMasterMgtServicesClient.FunPubGetLastAssetCategoryCode(intCompanyId, chkCategoryType.SelectedItem.Text, Convert.ToInt32(ddlAssetType.SelectedValue));
    //        ObjOrgMasterMgtServicesClient.Close();

    //    }

    /// <summary>
    /// Method for searching
    /// </summary>
    private void FunPriSearch(string strCategory)
    {

        DataSet dsSearch = (DataSet)ViewState["AssetCategories"];
        DataView dtSearchView = new DataView();

        switch (strCategory)
        {
            case "Class":
                dtSearchView = new DataView(dsSearch.Tables[0]);
                if (ddlClassSearchby.SelectedIndex == 0)
                    dtSearchView.RowFilter = "Category_Code like '" + txtClassSearchValue.Text + "%'";
                else
                    dtSearchView.RowFilter = "Category_Description like '" + txtClassSearchValue.Text + "%'";
                grvAssetClassCodes.DataSource = dtSearchView;
                grvAssetClassCodes.DataBind();
                break;
            case "Make":
                dtSearchView = new DataView(dsSearch.Tables[1]);
                if (ddlMakeSearchby.SelectedIndex == 0)
                    dtSearchView.RowFilter = "Category_Code like '" + txtMakeSearchValue.Text + "%'";
                else
                    dtSearchView.RowFilter = "Category_Description like '" + txtMakeSearchValue.Text + "%'";
                grvAssetMakeCodes.DataSource = dtSearchView;
                grvAssetMakeCodes.DataBind();
                break;
            case "Type":
                dtSearchView = new DataView(dsSearch.Tables[2]);
                if (ddlTypeSearchby.SelectedIndex == 0)
                    dtSearchView.RowFilter = "Category_Code like '" + txtTypeSearchValue.Text + "%'";
                else
                    dtSearchView.RowFilter = "Category_Description like '" + txtTypeSearchValue.Text + "%'";
                grvAssetTypeCodes.DataSource = dtSearchView;
                grvAssetTypeCodes.DataBind();
                break;
            case "Model":
                dtSearchView = new DataView(dsSearch.Tables[3]);
                if (ddlModelSearchby.SelectedIndex == 0)
                    dtSearchView.RowFilter = "Category_Code like '" + txtModelSearchValue.Text + "%'";
                else
                    dtSearchView.RowFilter = "Category_Description like '" + txtModelSearchValue.Text + "%'";
                grvAssetModelCodes.DataSource = dtSearchView;
                grvAssetModelCodes.DataBind();
                break;
        }
        if (dtSearchView == null || dtSearchView.ToTable().Rows.Count == 0)
        {
            switch (strCategory)
            {
                case "Class":

                    grvAssetClassCodes.DataSource = null;
                    grvAssetClassCodes.DataBind();
                    break;
                case "Make":

                    grvAssetMakeCodes.DataSource = null;
                    grvAssetMakeCodes.DataBind();
                    break;
                case "Type":

                    grvAssetTypeCodes.DataSource = null;
                    grvAssetTypeCodes.DataBind();
                    break;
                case "Model":

                    grvAssetModelCodes.DataSource = null;
                    grvAssetModelCodes.DataBind();
                    break;
            }
            // lblSearchMessage.Text = "No Records Found...";
        }
        else
        {
            //lblSearchMessage.Text = "";
        }
    }

    /// <summary>
    /// To Initialize Data table for temp adding of data
    /// </summary>
    protected void FunProIntializeData()
    {
        DataTable dtAssetClass;
        DataTable dtAssetMake;
        DataTable dtAssetType;
        DataTable dtAssetModel;

        dtAssetClass = new DataTable("Asset Class");
        dtAssetClass.Columns.Add("Code");
        dtAssetClass.Columns.Add("Description");
        dtAssetClass.Columns.Add("CategoryType");
        dtAssetClass.Columns[0].Unique = true;
        dtAssetClass.Columns[1].Unique = true;

        dtAssetMake = new DataTable("Asset Make");
        dtAssetMake.Columns.Add("Code");
        dtAssetMake.Columns.Add("Description");
        dtAssetMake.Columns.Add("CategoryType");
        dtAssetMake.Columns[0].Unique = true;
        dtAssetMake.Columns[1].Unique = true;

        dtAssetType = new DataTable("Asset Type");
        dtAssetType.Columns.Add("Code");
        dtAssetType.Columns.Add("Description");
        dtAssetType.Columns.Add("CategoryType");
        dtAssetType.Columns[0].Unique = true;
        dtAssetType.Columns[1].Unique = true;

        dtAssetModel = new DataTable("Asset Model");
        dtAssetModel.Columns.Add("Code");
        dtAssetModel.Columns.Add("Description");
        dtAssetModel.Columns.Add("CategoryType");
        dtAssetModel.Columns[0].Unique = true;
        dtAssetModel.Columns[1].Unique = true;

        ViewState["ClassTable"] = dtAssetClass;
        ViewState["MakeTable"] = dtAssetMake;
        ViewState["TypeTable"] = dtAssetType;
        ViewState["ModelTable"] = dtAssetModel;

        grvAssetClass.DataSource = null;
        grvAssetClass.DataBind();

        grvAssetMake.DataSource = null;
        grvAssetMake.DataBind();

        grvAssetType.DataSource = null;
        grvAssetType.DataBind();

        grvAssetModel.DataSource = null;
        grvAssetModel.DataBind();
        txtCode.Text = "";
        txtCodeDescription.Text = "";

    }

    /// <summary>
    /// To load asset category codes based on company asset category and type
    /// </summary>
    /// <param name="intCompanyId"></param>
    /// <param name="intAsset_CatId"></param>
    /// <param name="intAssetType_Id"></param>
    protected void FunPubLoadAssetCategories(int intCompanyId, int? intAsset_CatId, int? intAssetType_Id, string strCategoryType)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            DataSet dsAssetCategory = new DataSet();

            //byte[] byte_AssetCategory = ObjOrgMasterMgtServicesClient.FunPubQueryAssetCategoryDetails(intCompanyId, intAsset_CatId, intAssetType_Id);
            /********Code changed by Nataraj Y on 17/05/2010 for new asset code*/
            byte[] byte_AssetCategory = ObjOrgMasterMgtServicesClient.FunPubQueryAssetCategoryDetails(intCompanyId, intAsset_CatId, null);
            dsAssetCategory = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_AssetCategory, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            ViewState["AssetCategories"] = dsAssetCategory;
            if (dsAssetCategory.Tables.Count > 1)
            {
                grvAssetClassCodes.DataSource = dsAssetCategory.Tables[0].DefaultView;
                grvAssetClassCodes.DataBind();

                grvAssetMakeCodes.DataSource = dsAssetCategory.Tables[1].DefaultView;
                grvAssetMakeCodes.DataBind();

                grvAssetTypeCodes.DataSource = dsAssetCategory.Tables[2].DefaultView;
                grvAssetTypeCodes.DataBind();

                grvAssetModelCodes.DataSource = dsAssetCategory.Tables[3].DefaultView;
                grvAssetModelCodes.DataBind();
                ViewState["ASSETLOBCATMAP"] = dsAssetCategory.Tables[4];


            }
            else
            {
                txtCode.Text = dsAssetCategory.Tables[0].Rows[0]["Category_Code"].ToString();
                txtCodeDescription.Text = dsAssetCategory.Tables[0].Rows[0]["Category_Description"].ToString();
                hdnID.Value = dsAssetCategory.Tables[0].Rows[0]["Created_By"].ToString();
                FunPriGetLastGenCode(strCategoryType);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Asset category details");
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();
        }
    }

    /// <summary>
    /// Get Asset Code details
    /// </summary>
    /// <param name="intCompanyId"></param>
    /// <param name="intAssetId"></param>
    /// <param name="intAssetType_Id"></param>
    protected void FunPubProGetAssetDetails(int intCompanyId, int intAssetId, int? intAssetType_Id)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            DataTable dtAssetDetails;
            // ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            byte[] byteAssetDetails = ObjOrgMasterMgtServicesClient.FunPubQueryAssetDetails(intCompanyId, intAssetId, intAssetType_Id);
            dtAssetDetails = (DataTable)ClsPubSerialize.DeSerialize(byteAssetDetails, SerializationMode.Binary, typeof(DataTable));
            txtAssetCode.Text = dtAssetDetails.Rows[0]["Asset_Code"].ToString();
            txtAssetCodeDesc.Text = dtAssetDetails.Rows[0]["Asset_Description"].ToString();
            cmbBlockDepreciationCategory.Text = dtAssetDetails.Rows[0]["Block_Depreciation_Category"].ToString();
            if (dtAssetDetails.Rows[0]["Block_Depreciation_Rate"].ToString() != string.Empty)
            {
                txtBlockDepreciationRate.Text = dtAssetDetails.Rows[0]["Block_Depreciation_Rate"].ToString();
            }
            cmbDepreciationCategory.Text = dtAssetDetails.Rows[0]["Book_Depreciation_Category"].ToString();
            if (dtAssetDetails.Rows[0]["Book_Depreciation_Rate"].ToString() != string.Empty)
            {
                txtDepreciationRate.Text = dtAssetDetails.Rows[0]["Book_Depreciation_Rate"].ToString();
            }
            if (dtAssetDetails.Rows[0]["GuideLine_Value"].ToString() != string.Empty)
            {
                txtGuideLineValue.Text = dtAssetDetails.Rows[0]["GuideLine_Value"].ToString();
            }
            chkActive.Checked = Convert.ToBoolean(dtAssetDetails.Rows[0]["IS_Active"].ToString());
            if (dtAssetDetails.Rows[0]["TranExists"].ToString() == "1")
            {
                chkActive.Enabled = false;
            }
            ddlAssetType.SelectedValue = dtAssetDetails.Rows[0]["AssetType_ID"].ToString();
            lblClassDesc.Text = dtAssetDetails.Rows[0]["Class_Desc"].ToString();
            lblAssetMakeDesc.Text = dtAssetDetails.Rows[0]["Make_Desc"].ToString();
            lblAssetTypDesc.Text = dtAssetDetails.Rows[0]["Type_Desc"].ToString();
            lblAssetModelDesc.Text = dtAssetDetails.Rows[0]["Model_Desc"].ToString();
            hdnID.Value = dtAssetDetails.Rows[0]["Created_By"].ToString();
            //ddlLob.SelectedValue = dtAssetDetails.Rows[0]["LOB_ID"].ToString();
            ddlPurpose.SelectedValue = dtAssetDetails.Rows[0]["PURPOSE_ID"].ToString();
            txtValidFrom.Text = dtAssetDetails.Rows[0]["VALID_FROM"].ToString();
            txtValidTo.Text = dtAssetDetails.Rows[0]["VALID_TO"].ToString();
            if (strMode != "C")
            {
                funPriLoadLineofBusiness();

            }

            if (intAssetId > 0)
            {
                Dictionary<string, string> dictstrPropParm = new Dictionary<string, string>();
                dictstrPropParm.Add("@COMPANY_ID", intCompanyId.ToString());
                dictstrPropParm.Add("@ASSET_ID", intAssetId.ToString());
                dictstrPropParm.Add("@ASSETTYPE_ID", intAssetType_Id.ToString());
                DataTable dt = Utility.GetDefaultData("S3G_OR_GET_AST_LOBMPDT", dictstrPropParm);
                if (dt.Rows.Count > 0)
                {
                    grvLOB.DataSource = dt;
                    grvLOB.DataBind();

                }
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Asset details");
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();
        }
    }
    protected void grvLOB_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lblAssigned = (Label)e.Row.FindControl("lblAssigned");
            CheckBox chkSelectLOB = (CheckBox)e.Row.FindControl("chkSelectLOB");
            Label lblMapLob = (Label)e.Row.FindControl("lblMapLob");


            //chkSelectLOB.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvLOB.ClientID + "','chkSelectAllLOB','chkSelectLOB');");
            //if (lblAssigned.Text == "1")
            //{
            //if (IsLOBCalled == false)
            //{
            //if ((intConstitutionID > 0) && (strMode == "M"))
            //{
            //    IsLOBCalled = true;
            //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //    Procparam.Add("@Constitution_ID", intConstitutionID.ToString());
            //    DataTable dt_IsExists = Utility.GetDefaultData("S3G_ORG_InsertConstitutionTransactionIsExits", Procparam);
            //    if (dt_IsExists != null && dt_IsExists.Rows.Count > 0)
            //        IsEnableLOB = false;
            //}
            //}

            //if (!(IsEnableLOB))
            //    chkSelectLOB.Enabled = false;
            //chkSelectLOB.Checked = true;
            //}

            if (lblMapLob.Text != string.Empty)
            {
                chkSelectLOB.Checked = true;
                chkSelectLOB.Enabled = false;
            }
            else
            {
                chkSelectLOB.Checked = false;
                chkSelectLOB.Enabled = true;
            }

            if (strMode == "Q")
            {
                chkSelectLOB.Enabled = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAllRole");

            CheckBox chkSelectAllLOB = (CheckBox)e.Row.FindControl("chkSelectAllLob");
            chkSelectAllLOB.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvLOB.ClientID + "',this,'chkSelectLOB');");
            //chkAll.Checked = true;
            if (ViewState["SelectAll"] != null)
            {
                bool SelectAll = bool.Parse(ViewState["SelectAll"].ToString());
                chkSelectAllLOB.Checked = SelectAll;
            }
            if (strMode == "Q")
            {
                chkSelectAllLOB.Enabled = false;
            }
        }

    }
    void GetLOBGridSelectedValues()
    {
        // clear the Selected LOB's
        CheckBox chkLOB;
        Label LblLOB;
        ArrayList SelectedLOBs = new ArrayList();
        foreach (GridViewRow grvData in grvLOB.Rows)
        {
            chkLOB = ((CheckBox)grvData.FindControl("chkSelectLOB"));
            LblLOB = ((Label)grvData.FindControl("lblLOBID"));
            if (chkLOB.Checked)
                SelectedLOBs.Add(int.Parse(LblLOB.Text.Trim()));
            chkLOB.Checked = false;
        }
        chkLOB = (CheckBox)grvLOB.HeaderRow.FindControl("chkSelectAllLOB");
        chkLOB.Checked = false;
        ViewState["SelectedLOBs"] = SelectedLOBs;
    }

    void ClearLOBGrid()
    {
        // clear the Selected LOB's
        CheckBox chkLOB;
        foreach (GridViewRow grvData in grvLOB.Rows)
        {
            chkLOB = ((CheckBox)grvData.FindControl("chkSelectLOB"));
            chkLOB.Checked = false;
        }
        chkLOB = (CheckBox)grvLOB.HeaderRow.FindControl("chkSelectAllLOB");
        chkLOB.Checked = false;
    }
    void LoadSelectedLOBsAgain()
    {
        ArrayList SelectedLOB;
        DataTable AllLOB = new DataTable();
        if (ViewState["SelectedLOBs"] != null)
        {
            SelectedLOB = (ArrayList)ViewState["SelectedLOBs"];
            AllLOB = (DataTable)grvLOB.DataSource;

            foreach (DataRow LOB in AllLOB.Rows)
            {
                LOB["Assigned"] = false;
            }
            foreach (DataRow LOB in AllLOB.Rows)
            {
                foreach (int sLOB in SelectedLOB)
                {
                    if (int.Parse(LOB["LOB_ID"].ToString()) == sLOB)
                    {
                        LOB["Assigned"] = true;
                    }
                }

            }
        }
        grvLOB.DataSource = AllLOB;
        grvLOB.DataBind();
    }

    /// <summary>
    /// Method to load DropDownList & Ajax combos
    /// </summary>
    protected void FunProLoadCombos()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Type", "Block");
            cmbBlockDepreciationCategory.BindDataTable(SPNames.S3G_ORG_GetAssetCategory_List, Procparam, new string[] { "Block_Depreciation_Category", "Block_Depreciation_Category" });
            Procparam.Clear();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Type", "Book");
            cmbDepreciationCategory.BindDataTable(SPNames.S3G_ORG_GetAssetCategory_List, Procparam, new string[] { "Book_Depreciation_Category", "Book_Depreciation_Category" });
            Procparam.Clear();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Type", "Asset_Category");
            ddlAssetType.BindDataTable(SPNames.S3G_ORG_GetAssetCategory_List, Procparam, new string[] { "ID", "NAME" });
            funPriLoadLineofBusiness();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// Check valid rate and description
    /// </summary>
    /// <returns></returns>
    protected bool FunProValidatePagebln()
    {
        bool blnIsPageValid = false;
        switch (txtBlockDepreciationRate.Text.Length)
        {
            case 0:
                blnIsPageValid = true;
                break;

            default:
                if (cmbBlockDepreciationCategory.Text.Length <= 0)
                {
                    blnIsPageValid = false;
                }
                else
                {
                    blnIsPageValid = true;
                }
                break;
        }

        switch (txtDepreciationRate.Text.Length)
        {
            case 0:
                blnIsPageValid = true;
                break;

            default:
                if (cmbDepreciationCategory.Text.Length <= 0)
                {
                    blnIsPageValid = false;
                }
                else
                {
                    blnIsPageValid = true;
                }
                break;
        }
        return blnIsPageValid;
    }

    /// <summary>
    /// Asset Controls  
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriAssetControlStatus(int intModeID)
    {
        if (Request.QueryString.Get("Type") == "Code")
        {
            panAssetCode.Visible = true;
            panCategoryCode.Visible = false;
            /******Added by Nataraj Y on 17 -05-2011 for new Asset code****/
            panAssetType.Visible = true;
            FunPubLoadAssetCategories(intCompanyId, null, null, tcCode.ActiveTab.HeaderText);
        }
        else
        {
            panAssetCode.Visible = false;
            panCategoryCode.Visible = true;
            /******Added by Nataraj Y on 17 -05-2011 for new Asset code****/
            panAssetType.Visible = false;
            if (intModeID == 0)
                FunPriGetLastGenCode("Class");

        }

        switch (intModeID)
        {

            case 0://Create

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                grvAssetClassCodes.Enabled = true;
                grvAssetMakeCodes.Enabled = true;
                grvAssetTypeCodes.Enabled = true;
                grvAssetModelCodes.Enabled = true;
                tcAssetcode.Visible = true; divAssetcode.Visible = true;
                cmbDepreciationCategory.Enabled = false;
                //txtDepreciationRate.Enabled = false;
                //tr_Search.Visible = true;
                ddlAssetType.Enabled = true;
                panHierarchy.Visible = true;
                chkActive.Enabled = false;
                txtGuideLineValue.ReadOnly = false;
                if (!bCreate)
                {
                    btnAssetSubmit.Enabled_True();
                    //btnCategoryGo.Enabled = false;
                    btnCategoryGo.Enabled_False();
                    btnCategorySubmit.Enabled_False();
                }
                break;

            case 1://Modify
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                //chkActive.Enabled = false;
                lblLastCode.Visible = false;
                lblLastGenCode.Visible = false;
                txtAssetCode.ReadOnly = true;

                //Changed By Thangam M on 27/Feb/2012
                txtAssetCodeDesc.ReadOnly = false;
                //End here

                grvAssetClassCodes.Enabled = false;
                grvAssetMakeCodes.Enabled = false;
                grvAssetTypeCodes.Enabled = false;
                grvAssetModelCodes.Enabled = false;
                btnAssetSubmit.Enabled_True();
                btnAssetClear.Enabled_False();

                txtGuideLineValue.ReadOnly = false;
                tcAssetcode.Visible = false; divAssetcode.Visible = false;
                divAssetDetails.Attributes.Add("class", "col-lg-12 col-md-12 col-sm-12 col-xs-12");
                //tr_Search.Visible = false;
                panHierarchy.Visible = true;
                cmbBlockDepreciationCategory.Enabled = true;
                cmbDepreciationCategory.Enabled = true;
                //btnCategoryGo.Enabled = false;
                btnCategorySubmit.Enabled_False();
                //ddlLob.Enabled = false;
                ddlPurpose.Enabled = false;
                rfvddlPurpose.Enabled = false;
                rfvddlPurpose.CssClass = "styleDisplayLabel";
                lblPurpose.CssClass = "styleDisplayLabel";
                //Changed by bhuvana 
                btnCategorySubmit.Enabled_True();
                //End here

                tcCode.Visible = false;
                txtCode.ReadOnly = true;

                //changed by bhuvana 
                //txtCodeDescription.ReadOnly = true;
                txtCodeDescription.Enabled = true;
                //end here
                txtValidFrom.Enabled = false;
               

                chkCategoryType.SelectedValue = strCategoryType;
                chkCategoryType.Enabled = false;
                ddlAssetType.Enabled = false;
                if (!bModify)
                {
                    btnAssetSubmit.Enabled_False();
                    //btnCategoryGo.Enabled = false;
                    btnCategoryGo.Enabled_False();
                    btnCategorySubmit.Enabled_False();
                }

                cmbBlockDepreciationCategory_SelectedIndexChanged(null, null);
                //cmbDepreciationCategory_SelectedIndexChanged(null, null);
                btnCategorySubmit.ValidationGroup = "CategoryGroup";
                break;

            case -1://Query

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }
                ddlAssetType.Enabled = false;
                //ddlLob.Enabled = false;
                ddlPurpose.Enabled = false;
                rfvddlPurpose.Enabled = false;
                rfvddlPurpose.CssClass = "styleDisplayLabel";
                CalendarValidFrom.Enabled = false;
                CalendarValidTo.Enabled = false;
                txtValidFrom.Enabled = false;
                txtValidTo.Enabled = false;
                //txtValidFrom.ReadOnly = true;

                txtAssetCode.ReadOnly = true;
                txtAssetCodeDesc.ReadOnly = true;
                lblLastCode.Visible = false;
                lblLastGenCode.Visible = false;
                chkActive.Enabled = false;
                panHierarchy.Visible = true;
                txtBlockDepreciationRate.ReadOnly = true;
                txtDepreciationRate.ReadOnly = true;
                txtGuideLineValue.ReadOnly = true;
                tcAssetcode.Visible = false; divAssetcode.Visible = false;
                //tr_Search.Visible = false;
                btnAssetSubmit.Enabled_False();
                btnAssetClear.Enabled_False();
                //btnCategoryGo.Enabled = false;
                btnCategoryGo.Attributes.Add("disabled", "disabled");
                btnCategoryGo.Attributes.Add("class", "css_btn_disabled");  // enab
                btnCategorySubmit.Enabled_False();
                tcCode.Visible = false;
                txtCode.ReadOnly = true;
                txtCodeDescription.ReadOnly = true;
                chkCategoryType.SelectedValue = strCategoryType;
                chkCategoryType.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (bClearList)
                {
                    ddlAssetType.ClearDropDownList();
                    cmbDepreciationCategory.ClearDropDownList();
                    cmbBlockDepreciationCategory.ClearDropDownList();
                }

                txtBlockDepreciationRate.Enabled =
                cmbBlockDepreciationCategory.Enabled =
                    //txtDepreciationRate.Enabled =
                cmbDepreciationCategory.Enabled = false;

                break;
        }
    }

    /// <summary>
    /// Method to set maxlength
    /// </summary>
    private void FunPriSetMaxLength()
    {
        txtBlockDepreciationRate.SetPercentagePrefixSuffix(3, 2, false, false, "Block Depreciation Rate %");
        txtDepreciationRate.SetPercentagePrefixSuffix(3, 2, false, false, "Book Depreciation Rate %");

    }

    /// <summary>
    /// Method to get Last generated code
    /// </summary>
    /// <param name="strCategory"></param>
    private void FunPriGetLastGenCode(string strCategory)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            //switch (chkCategoryType.SelectedItem.Text)
            switch (strCategory)
            {
                case "Class":
                case "Class Code":
                    lblCode.Text = "Asset Class Code";
                    lblCodeDesc.Text = "Asset Class Code Description";
                    lblLastGenCode.Text = ObjOrgMasterMgtServicesClient.FunPubGetLastAssetCategoryCode(intCompanyId, "CLASS", Convert.ToInt32(ddlAssetType.SelectedValue));
                    txtCode.MaxLength = 3;
                    txtCode.Focus();
                    //FunProDisableTabs(0);
                    break;
                case "Make":
                case "Make Code":
                    lblCode.Text = "Asset Make Code";
                    lblCodeDesc.Text = "Asset Make Code Description";
                    lblLastGenCode.Text = ObjOrgMasterMgtServicesClient.FunPubGetLastAssetCategoryCode(intCompanyId, "MAKE", Convert.ToInt32(ddlAssetType.SelectedValue));
                    txtCode.MaxLength = 3;
                    txtCode.Focus();
                    //FunProDisableTabs(1);
                    break;
                case "Type":
                case "Type Code":
                    lblCode.Text = "Asset Type Code";
                    lblCodeDesc.Text = "Asset Type Code Description";
                    lblLastGenCode.Text = ObjOrgMasterMgtServicesClient.FunPubGetLastAssetCategoryCode(intCompanyId, "TYPE", Convert.ToInt32(ddlAssetType.SelectedValue));
                    txtCode.MaxLength = 3;
                    txtCode.Focus();
                    //FunProDisableTabs(2);
                    break;
                case "Model":
                case "Model Code":
                    lblCode.Text = "Asset Model Code";
                    lblCodeDesc.Text = "Asset Model Code Description";
                    lblLastGenCode.Text = ObjOrgMasterMgtServicesClient.FunPubGetLastAssetCategoryCode(intCompanyId, "MODEL", Convert.ToInt32(ddlAssetType.SelectedValue));
                    txtCode.MaxLength = 2;
                    txtCode.Focus();
                    // FunProDisableTabs(3);
                    break;
            }

            //Added by Thangam M on 15/Feb/2012 to change the validation messages based on the tab
            FunSetCodeValidations();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();
        }

    }

    //Added by Thangam M on 15/Feb/2012 to change the validation messages based on the tab
    private void FunSetCodeValidations()
    {
        rfvCode.ErrorMessage = "Enter the " + lblCode.Text;
        rfvCodeDescription.ErrorMessage = "Enter the " + lblCodeDesc.Text;
    }

    /// <summary>
    /// Method to reset values
    /// </summary>
    /// <param name="strOption"></param>
    /// <param name="strFieldAtt"></param>
    private void FunPriResetValues(string strOption, string strFieldAtt)
    {
        if (strOption == "0")
        {





            if (strFieldAtt.Contains("grvAssetClassCodes"))
            {
                ViewState["Class"] = null;
                ViewState["Classid"] = null;
                lblClassDesc.Text = "";


            }
            if (strFieldAtt.Contains("grvAssetMakeCodes"))
            {
                ViewState["Make"] = null;
                ViewState["Makeid"] = null;
                lblAssetMakeDesc.Text = "";
            }
            if (strFieldAtt.Contains("grvAssetTypeCodes"))
            {
                ViewState["Type"] = null;
                ViewState["Typeid"] = null;
                lblAssetTypDesc.Text = "";
            }
            if (strFieldAtt.Contains("grvAssetModelCodes"))
            {
                ViewState["Model"] = null;
                ViewState["Modelid"] = null;
                lblAssetModelDesc.Text = "";
            }
        }
        FunPriGetAssetCode();
    }

    /// <summary>
    /// Method to form asset code
    /// </summary>
    /// <param name="strFieldAtt"></param>
    private void FunPriFormAssetCode(string strFieldAtt)
    {
        string strVal = string.Empty;
        int gRowIndex = 0;
        if (strFieldAtt.Contains("grvAssetClassCodes"))
        {
            strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvAssetClassCodes_")).Replace("grvAssetClassCodes_ctl", "");
            gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            Label lblCode = (Label)grvAssetClassCodes.Rows[gRowIndex].FindControl("lblCategoryCode");
            Label lblId = (Label)grvAssetClassCodes.Rows[gRowIndex].FindControl("lblClassId");
            Label lblDesc = (Label)grvAssetClassCodes.Rows[gRowIndex].FindControl("lblClassDesc");
            //txtAssetCode.Text += lblCode.Text;
            ViewState["Class"] = lblCode.Text;
            ViewState["Classid"] = lblId.Text;
            lblClassDesc.Text = lblDesc.Text;
        }
        if (strFieldAtt.Contains("grvAssetMakeCodes"))
        {
            strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvAssetMakeCodes_")).Replace("grvAssetMakeCodes_ctl", "");
            gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            Label lblCode = (Label)grvAssetMakeCodes.Rows[gRowIndex].FindControl("lblCategoryCode");
            Label lblId = (Label)grvAssetMakeCodes.Rows[gRowIndex].FindControl("lblMakeId");
            Label lblDesc = (Label)grvAssetMakeCodes.Rows[gRowIndex].FindControl("lblMakeDesc");
            //txtAssetCode.Text += lblCode.Text;
            ViewState["Make"] = lblCode.Text;
            ViewState["Makeid"] = lblId.Text;
            lblAssetMakeDesc.Text = lblDesc.Text;
        }
        if (strFieldAtt.Contains("grvAssetTypeCodes"))
        {
            strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvAssetTypeCodes_")).Replace("grvAssetTypeCodes_ctl", "");
            gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            Label lblCode = (Label)grvAssetTypeCodes.Rows[gRowIndex].FindControl("lblCategoryCode");
            Label lblId = (Label)grvAssetTypeCodes.Rows[gRowIndex].FindControl("lblTypeId");
            Label lblDesc = (Label)grvAssetTypeCodes.Rows[gRowIndex].FindControl("lblTypeDesc");
            //txtAssetCode.Text += lblCode.Text;
            ViewState["Type"] = lblCode.Text;
            ViewState["Typeid"] = lblId.Text;
            lblAssetTypDesc.Text = lblDesc.Text;
        }
        if (strFieldAtt.Contains("grvAssetModelCodes"))
        {
            strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvAssetModelCodes_")).Replace("grvAssetModelCodes_ctl", "");
            gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            Label lblCode = (Label)grvAssetModelCodes.Rows[gRowIndex].FindControl("lblCategoryCode");
            Label lblId = (Label)grvAssetModelCodes.Rows[gRowIndex].FindControl("lblModelId");
            Label lblDesc = (Label)grvAssetModelCodes.Rows[gRowIndex].FindControl("lblModelDesc");
            //txtAssetCode.Text += lblCode.Text;
            ViewState["Model"] = lblCode.Text;
            ViewState["Modelid"] = lblId.Text;
            lblAssetModelDesc.Text = lblDesc.Text;

        }
        FunPriGetAssetCode();
        panHierarchy.Visible = true;
    }

    /// <summary>
    /// Method to get asset code from viewstate 
    /// </summary>
    private void FunPriGetAssetCode()
    {

        string[] strPurpose = ddlPurpose.SelectedItem.Text.Split('-');
        if (strPurpose.Length > 0)
            txtAssetCode.Text = ((ViewState["AssetType"] != null) ? ViewState["AssetType"].ToString() : "NA") + ((ViewState["Class"] != null) ? ViewState["Class"].ToString() : "NA") + ((ViewState["Make"] != null) ? ViewState["Make"].ToString() : "NA") + ((ViewState["Type"] != null) ? ViewState["Type"].ToString() : "NA") + ((ViewState["Model"] != null) ? ViewState["Model"].ToString() : "NA") + strPurpose[0].Trim();
        else
            txtAssetCode.Text = ((ViewState["AssetType"] != null) ? ViewState["AssetType"].ToString() : "") + ((ViewState["Class"] != null) ? ViewState["Class"].ToString() : "") + ((ViewState["Make"] != null) ? ViewState["Make"].ToString() : "") + ((ViewState["Type"] != null) ? ViewState["Type"].ToString() : "") + ((ViewState["Model"] != null) ? ViewState["Model"].ToString() : "");
        //hdnID.Value = ((ViewState["Classid"] != null) ? ViewState["Classid"].ToString() : "") + "," + ((ViewState["Makeid"] != null) ? ViewState["Makeid"].ToString() : "") + "," + ((ViewState["Typeid"] != null) ? ViewState["Typeid"].ToString() : "") + "," + ((ViewState["Modelid"] != null) ? ViewState["Modelid"].ToString() : "");
        //if ((ViewState["AssetType"] != null) && (ViewState["Class"] != null) && (ViewState["Make"] != null) && (ViewState["Type"] != null) && (ViewState["Model"] != null))
        //{
        //    txtAssetCodeDesc.ReadOnly = false;
        //}
        //else
        //{
        //    txtAssetCodeDesc.Text = string.Empty;
        //    txtAssetCodeDesc.ReadOnly = true;
        //}
    }

    /// <summary>
    /// Method to uncheck all checkbox
    /// </summary>
    /// <param name="grv"></param>
    private void FunGridUncheckAll(GridView grv)
    {
        foreach (GridViewRow grvRow in grv.Rows)
        {
            CheckBox chkBox = (CheckBox)grvRow.FindControl("chkCategoryCode");
            chkBox.Checked = false;
        }
    }

    #endregion




    protected void ddlPurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGetAssetCode();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void txtValidFrom_TextChanged(object sender, EventArgs e)
    {
        hidDate.Value = "End";
        CalendarValidFrom.Focus();

        if (!string.IsNullOrEmpty(txtValidFrom.Text))
        {
            if (Utility.StringToDate(txtValidFrom.Text) < Utility.StringToDate(DateTime.Now.ToString(strDateFormat))) //(Convert.ToInt32(Utility.StringToDate(txtFFromDate.Text)) < Convert.ToInt32(Utility.StringToDate(strDate)))
            {
                Utility.FunShowAlertMsg(this, "Valid From cannot be lesser than System Date.");
                txtValidFrom.Text = string.Empty;
                txtValidTo.Focus();
                return;
            }
        }
    }
    protected void txtValidTo_TextChanged(object sender, EventArgs e)
    {
        hidDate.Value = "Start";
        CalendarValidTo.Focus();
        if (!string.IsNullOrEmpty(txtValidTo.Text))
        {
            if (Utility.StringToDate(txtValidTo.Text) < Utility.StringToDate(DateTime.Now.ToString(strDateFormat)))
            {
                Utility.FunShowAlertMsg(this, "Valid To Date cannot be lesser than System Date.");
                txtValidTo.Text = string.Empty;
                //txtValidTo.Focus();
                return;
            }
            if (txtValidTo.Text != string.Empty && txtValidFrom.Text != string.Empty)
            {
                if (Utility.StringToDate(txtValidTo.Text) < Utility.StringToDate(txtValidFrom.Text))
                {
                    Utility.FunShowAlertMsg(this, "Valid To Date should be greater than or equal to Valid From Date");
                    txtValidTo.Text = string.Empty;
                    //txtValidTo.Focus();
                    return;
                }
            }
        }
    }
    protected void ddlLob_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //ddlLob.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {

        }

    }

    protected void btnAssetCategoryClear_ServerClick(object sender, EventArgs e)
    {
        txtCode.Text = string.Empty;
        txtCodeDescription.Text = string.Empty;
        txtDepreciationRate.Text = string.Empty;
        FunPriLoadDepricationRate();
        FunProIntializeData();
    }
    private void funPriRestAssetCategory()
    {
        txtAssetCode.Text = "";

        lblLineofBusiness.Visible = false;
        txtAssetCodeDesc.Text = "";
        ddlClassSearchby.SelectedIndex = 0;
        txtClassSearchValue.Text = "";
        ddlMakeSearchby.SelectedIndex = 0;
        txtMakeSearchValue.Text = "";
        ddlTypeSearchby.SelectedIndex = 0;
        txtTypeSearchValue.Text = "";
        ddlModelSearchby.SelectedIndex = 0;
        txtModelSearchValue.Text = "";
        ddlPurpose.SelectedIndex = 0;
        txtGuideLineValue.Text = "";
        txtValidFrom.Text = "";
        txtValidTo.Text = "";
        grvLOB.DataSource = null;
        grvLOB.DataBind();
        FunPriLoadDepricationRate();
        FunGridUncheckAll(grvAssetClassCodes);
        FunGridUncheckAll(grvAssetMakeCodes);
        FunGridUncheckAll(grvAssetTypeCodes);
        FunGridUncheckAll(grvAssetModelCodes);
        lblClassDesc.Text = string.Empty;
        lblAssetMakeDesc.Text = string.Empty;
        lblAssetTypDesc.Text = string.Empty;
        lblAssetModelDesc.Text = string.Empty;
        tcAssetcode.ActiveTabIndex = 0;
        ViewState["Classid"] = null;
        ViewState["Makeid"] = null;
        ViewState["Typeid"] = null;
        ViewState["Modelid"] = null;
    }

    protected void btnAssetClear_ServerClick(object sender, EventArgs e)
    {
        try
        {

            ddlAssetType.SelectedIndex = 0;
            txtAssetCode.Text = "";
            lblLineofBusiness.Visible = false;
            txtAssetCodeDesc.Text = "";
            ddlClassSearchby.SelectedIndex = 0;
            txtClassSearchValue.Text = "";
            ddlMakeSearchby.SelectedIndex = 0;
            txtMakeSearchValue.Text = "";
            ddlTypeSearchby.SelectedIndex = 0;
            txtTypeSearchValue.Text = "";
            ddlModelSearchby.SelectedIndex = 0;
            txtModelSearchValue.Text = "";
            ddlPurpose.SelectedIndex = 0;
            txtGuideLineValue.Text = "";
            txtValidFrom.Text = "";
            txtValidTo.Text = "";
            grvLOB.DataSource = null;
            grvLOB.DataBind();
            FunPriLoadDepricationRate();
            FunGridUncheckAll(grvAssetClassCodes);
            FunGridUncheckAll(grvAssetMakeCodes);
            FunGridUncheckAll(grvAssetTypeCodes);
            FunGridUncheckAll(grvAssetModelCodes);
            lblClassDesc.Text = string.Empty;
            lblAssetMakeDesc.Text = string.Empty;
            lblAssetTypDesc.Text = string.Empty;
            lblAssetModelDesc.Text = string.Empty;
            tcAssetcode.ActiveTabIndex = 0;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnAssetCancel_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3gOrgAssetMaster_View.aspx");
    }
}
public static class ExtendPropertyAssetMaster
{
    public static void SuffixInner(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq, bool IsNegativeRequired, string strFieldName)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {

            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            S3GSession S3GSession = new S3GSession();
            intGPSPrefix = S3GSession.ProGpsPrefixRW;
            intGPSSuffix = S3GSession.ProGpsSuffixRW;


            txtBox.MaxLength = intPrefix + intSuffix + 1;
            if (IsNegativeRequired)
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,true,this);");
            }
            else
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this);");
            }
            txtBox.Attributes.Add("onkeyup", "funCutDecimal(this,'" + intSuffix + "')");
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "funChkDecimialChromeFix(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", "funChkDecimialChromeFix(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',false);");
            }
            txtBox.Style.Add("text-align", "right");
            //txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }
}