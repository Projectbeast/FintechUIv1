
using S3GBusEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ORG = S3GBusEntity.Origination;
using ORGSERVICE = OrgMasterMgtServicesReference;

public partial class Origination_S3GOrgDealerMatrix_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyId = 0;
    int intUserId = 0;
    int intDealerMatrixID = 0;
    int intErrorCode = 0;
    string strEntityCode;
    ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable ObjEntityMasterDataTable;
    //ORG.OrgMasterMgtServices.S3G_ORG_EntityBankMappingDataTable ObjEntityBankMappingDataTable;
    ORGSERVICE.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;

    public string strCustomerID = string.Empty;
    static string strPageName = "Dealer Matrix";
    public string strDateFormat;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgDealerMatrix_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgDealerMatrix_Add.aspx';";
    string strRedirectPage = "~/Origination/S3GOrgDealerMatrix_Add.aspx";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    public static Origination_S3GOrgDealerMatrix_Add obj_Page;
    Dictionary<string, string> Procparam = null;
    //Code end
    bool IsLOBCalled = false;
    DataTable dtDealerMatDet = new DataTable();

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            ObjUserInfo = new UserInfo();
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end


            intCompanyId = ObjUserInfo.ProCompanyIdRW;//Convert.ToInt32(Request.QueryString["qsCmpnyId"]);
            intUserId = ObjUserInfo.ProUserIdRW;

            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            //ceMatrixDate.Format = strDateFormat;
            ceFromDate.Format = strDateFormat;                       // assigning the first textbox with the End date
            ceToDate.Format = strDateFormat;

            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyId.ToString();

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intDealerMatrixID = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }



            if (ViewState["PageMode"] == null)
            {
                ViewState["PageMode"] = PageMode.ToString();
            }

            //Entity Code Popup Start
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
            txtUserName.Attributes.Add("onfocus", "fnLoadCustomer()");
            if (txtUserName.Text == "")
            {
                System.Web.HttpContext.Current.Session.Remove("Cust_id");
                HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                hdnCID.Value = string.Empty;
            }
            //Entity Code Popup End

            if (!IsPostBack)
            {
                //  FunGetEntityTypeDetails();
                FunPriLoadLOB();
                FunPriLoadBranch();
                FunPriLoadPage();
                if ((intDealerMatrixID > 0) && (strMode == "M"))
                {
                    hdnMode.Value = "M";
                    FunPriDisableControls(1);
                }
                else if ((intDealerMatrixID > 0) && (strMode == "Q")) // Query // Modify
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    strMode = "C"; //Create Mode
                    FunPriDisableControls(0);

                }
               // txtMatrixNumber.Focus();//Added by Suseela
                ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
                Button btnGetLov = ((Button)ucCustomerCodeLov.FindControl("btnGetLOV"));
                btnGetLov.Focus();
            }

            txtEffectiveFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtEffectiveFrom.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEffectiveTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtEffectiveTo.ClientID + "','" + strDateFormat + "',true,  false);");
           // txtMatrixDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtMatrixDate.ClientID + "','" + strDateFormat + "',true,  false);");//commented for design
            //pnlLOBDetails.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName); ;
        }
    }

    #endregion

    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                if (!bCreate)
                {
                    btnSave.Enabled = true;
                }

                chkIs_Active.Checked = true;
                chkIs_Active.Enabled = false;

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                if (!bModify)
                {
                    btnSave.Enabled = false;
                }

                btnClear.Enabled = false;
                break;

            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }


                btnClear.Enabled = false;
                btnSave.Enabled = false;
                chkIs_Active.Enabled = false;

                break;
        }

    }

    #region Button Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Focus();//Added by Suseela
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ucCustomerCodeLov.Controls.Clear();
        txtMatrixNumber.Clear();
        //txtMatrixDate.Clear();//commented for design
        ddlSchemeType.Clear();
        txtSchemeName.Clear();
        txtEffectiveFrom.Clear();
        txtEffectiveTo.Clear();
        ViewState["DealerMatrixDetails"] = null;
        ClearDealerDetailsControls();
        gvDealerMatrixDet.Visible = false;
        //ddlLocation.Clear();
        btnClear.Focus();//Added by Suseela

    }

    protected void btnExit_Click(object sender, EventArgs e)
    {

        Response.Redirect("~/Origination/S3GOrgDealerMatrix_View.aspx");
        btnExit.Focus();//Added by Suseela

    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            strCustomerID = string.Empty;

            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCID != null && hdnCID.Value != "")
            {
                System.Web.HttpContext.Current.Session.Remove("Cust_id");
                strCustomerID = hdnCID.Value;
                System.Web.HttpContext.Current.Session["Cust_id"] = hdnCID.Value;

            }

          //  txtMatrixDate.Focus();//commented for design

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region Grid Events

    protected void grvLOB_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAssigned = (Label)e.Row.FindControl("lblAssigned");
            CheckBox chkSelectLOB = (CheckBox)e.Row.FindControl("chkSelectLOB");
            chkSelectLOB.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvLOB.ClientID + "','chkSelectAllLOB','chkSelectLOB');");
            if (lblAssigned.Text == "1")
            {
                if (IsLOBCalled == false)
                {
                    //if ((intDealerMatrixID > 0) && (strMode == "M"))
                    //{
                    //    IsLOBCalled = true;
                    //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
                    //    Procparam.Add("@Constitution_ID", intConstitutionID.ToString());
                    //    DataTable dt_IsExists = Utility.GetDefaultData("S3G_ORG_InsertConstitutionTransactionIsExits", Procparam);
                    //    if (dt_IsExists != null && dt_IsExists.Rows.Count > 0)
                    //        IsEnableLOB = false;
                    //}
                }

                //if (!(IsEnableLOB))
                //    chkSelectLOB.Enabled = false;
                chkSelectLOB.Checked = true;
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

    #endregion

    #region WebMethod


    [System.Web.Services.WebMethod]
    //public static string[] GetLocationList(String prefixText, int count)
    //{
    //    Dictionary<string, string> Procparam;
    //    Procparam = new Dictionary<string, string>();
    //    List<String> suggestions = new List<String>();
    //    DataTable dtCommon = new DataTable();
    //    DataSet Ds = new DataSet();
    //    Procparam.Clear();
    //    Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
    //    Procparam.Add("@PrefixText", prefixText);
    //    suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3g_Get_Location_List", Procparam));
    //    return suggestions.ToArray();
    //}

    #endregion

    #region Text Changed Events`

    protected void txtEffectiveFrom_TextChanged(object sender, EventArgs e)
    {
        FunDateValidation(1);
        txtEffectiveFrom.Focus();//Added by Suseela
    }

    protected void txtEffectiveTo_TextChanged(object sender, EventArgs e)
    {
        FunDateValidation(2);
        if (txtEffectiveTo.Text != string.Empty)
            txtStartTenure.Focus();
        //txtEffectiveTo.Focus();//Added by Suseela
    }

    private void FunDateValidation(int No)
    {
        try
        {
            if ((!string.IsNullOrEmpty(txtEffectiveFrom.Text)) && (!string.IsNullOrEmpty(txtEffectiveTo.Text)))
            {
                if ((Convert.ToInt32(Utility.StringToDate(txtEffectiveFrom.Text).ToString("yyyyMMdd"))) > (Convert.ToInt32(Utility.StringToDate(txtEffectiveTo.Text).ToString("yyyyMMdd"))))
                {
                    Utility.FunShowAlertMsg(this, "Effective To should be greater than or equal to Effective From");
                    if (No == 1)
                    {
                        txtEffectiveFrom.Text = string.Empty;
                        txtEffectiveFrom.Focus();
                    }
                    else
                    {
                        txtEffectiveTo.Text = string.Empty;
                        txtEffectiveTo.Focus();
                    }
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriLoadLOB()
    {
        // LOB ComboBoxLOBSearch
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Dealermatrix_Id", Convert.ToString(intDealerMatrixID));
            Procparam.Add("@User_ID", Convert.ToString(intUserId));

            if (PageMode == PageModes.Query)
                Procparam.Add("@IsQueryMode", "1");
            else
                Procparam.Add("@IsQueryMode", "0");

            //ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            DataTable dtGrid = Utility.GetDefaultData("S3G_OR_GET_DEALER_MAT_LOB", Procparam);
            if (dtGrid.Rows.Count > 0)
            {
                grvLOB.DataSource = dtGrid;
                grvLOB.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadBranch()
    {

        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Dealermatrix_Id", Convert.ToString(intDealerMatrixID));
            Procparam.Add("@User_ID", Convert.ToString(intUserId));

            if (PageMode == PageModes.Query)
                Procparam.Add("@IsQueryMode", "1");
            else
                Procparam.Add("@IsQueryMode", "0");

            DataTable dtGrid = Utility.GetDefaultData("S3G_OR_GET_DEALR_MAT_BRNCH_LST", Procparam);
            if (dtGrid.Rows.Count > 0)
            {
                grvBranchDetails.DataSource = dtGrid;
                grvBranchDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadPage()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Lookup_Type", "DEAL_TYPE");
            Procparam.Add("@Table_Name", "S3G_ORG_Lookup");
            ddlDealType.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, new string[] { "LOOKUP_CODE", "DESCRIPTION" });

            //Bind Grid Details Start

            DataRow drDealerMatDet;
            dtDealerMatDet.Columns.Add("SNo");
            dtDealerMatDet.Columns.Add("Dealer_Matrix_Det_ID");
            dtDealerMatDet.Columns.Add("StartTenure");
            dtDealerMatDet.Columns.Add("EndTenure");
            dtDealerMatDet.Columns.Add("Monthly_ICC");
            dtDealerMatDet.Columns.Add("HighRate");
            dtDealerMatDet.Columns.Add("LowRate");
            dtDealerMatDet.Columns.Add("MinDPRequired");
            dtDealerMatDet.Columns.Add("HighDP");
            dtDealerMatDet.Columns.Add("LowDP");
            dtDealerMatDet.Columns.Add("DealType");
            dtDealerMatDet.Columns.Add("DealTypeID");
            dtDealerMatDet.Columns.Add("RowStatus");

            drDealerMatDet = dtDealerMatDet.NewRow();
            drDealerMatDet["SNo"] = 0;
            dtDealerMatDet.Rows.Add(drDealerMatDet);

            ViewState["DealerMatrixDetails"] = dtDealerMatDet;
            gvDealerMatrixDet.DataSource = dtDealerMatDet;
            gvDealerMatrixDet.DataBind();
            gvDealerMatrixDet.Rows[0].Visible = false;
            ViewState["DealerMatrixDetails"] = dtDealerMatDet;

            //Bind Grid Details End
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
    #endregion




    protected void gvDealerMatrixDet_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gvDealerMatrixDet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkbtnRemove = (LinkButton)e.Row.FindControl("btnRemove");

            if (strMode == "M" || strMode == "Q")
                lnkbtnRemove.Visible = false;


            if (dtDealerMatDet.Rows.Count != 0)
            {
                if (dtDealerMatDet.Rows[0]["StartTenure"].ToString() == "")
                {
                    dtDealerMatDet = (DataTable)ViewState["DealerMatrixDetails"];
                    dtDealerMatDet.Rows[0].Delete();
                    ViewState["DealerMatrixDetails"] = dtDealerMatDet;
                }
            }
        }
    }

    protected void gvDealerMatrixDet_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete = (DataTable)ViewState["DealerMatrixDetails"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            gvDealerMatrixDet.DataSource = dtDelete;
            gvDealerMatrixDet.DataBind();
            //FunPriHideBankColumns();
            ViewState["DealerMatrixDetails"] = dtDelete;
            ClearDealerDetailsControls();
            if (gvDealerMatrixDet.Rows.Count == 0)
            {
                btnAdd.Enabled = true;
                //btnModify.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            // cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Remove Bank Details";
            //cvCustomerMaster.IsValid = false;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataRow dr;
            dtDealerMatDet = (DataTable)ViewState["DealerMatrixDetails"];
            dr = dtDealerMatDet.NewRow();
            string strDealerMatrixId = Convert.ToString(dtDealerMatDet.Rows.Count + 1);

            dr["SNo"] = strDealerMatrixId;
            dr["StartTenure"] = txtStartTenure.Text.Trim();
            dr["EndTenure"] = txtEndTenure.Text.Trim();
            dr["Monthly_ICC"] = txtMonthly_ICC.Text.Trim();
            dr["HighRate"] = txtHighRate.Text.Trim();
            dr["LowRate"] = txtLowRate.Text.Trim();
            dr["MinDPRequired"] = txtMinDPRequired.Text.Trim();
            dr["HighDP"] = txtHighDP.Text.Trim();
            dr["LowDP"] = txtLowDP.Text.Trim();
            dr["DealType"] = ddlDealType.SelectedItem.Text;
            dr["DealTypeID"] = ddlDealType.SelectedItem.Value;
            dtDealerMatDet.Rows.Add(dr);
            gvDealerMatrixDet.DataSource = dtDealerMatDet;
            gvDealerMatrixDet.DataBind();
            ViewState["DealerMatrixDetails"] = dtDealerMatDet;
            btnAdd.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }
    protected void ClearDealerDetailsControls()
    {
        try
        {

            txtStartTenure.Clear();
            txtEndTenure.Clear();
            txtMonthly_ICC.Clear();
            txtHighRate.Clear();
            txtLowRate.Clear();
            txtMinDPRequired.Clear();
            txtHighDP.Clear();
            txtLowDP.Clear();
            ddlDealType.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void grvBranchDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          //  Label lblAssigned = (Label)e.Row.FindControl("lblAssigned");
            CheckBox chkSelectBranch = (CheckBox)e.Row.FindControl("chkSelectBranch");
            chkSelectBranch.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvLOB.ClientID + "','chkSelectAllBranch','chkSelectBranch');");
            //if (lblAssigned.Text == "1")
            //{
                if (IsLOBCalled == false)
                {
                    //if ((intDealerMatrixID > 0) && (strMode == "M"))
                    //{
                    //    IsLOBCalled = true;
                    //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
                    //    Procparam.Add("@Constitution_ID", intConstitutionID.ToString());
                    //    DataTable dt_IsExists = Utility.GetDefaultData("S3G_ORG_InsertConstitutionTransactionIsExits", Procparam);
                    //    if (dt_IsExists != null && dt_IsExists.Rows.Count > 0)
                    //        IsEnableLOB = false;
                    //}
              //  }

                //if (!(IsEnableLOB))
                //    chkSelectLOB.Enabled = false;
               // chkSelectBranch.Checked = true;
            }
            if (strMode == "Q")
            {
                chkSelectBranch.Enabled = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAllRole");

            CheckBox chkSelectAllBranch = (CheckBox)e.Row.FindControl("chkSelectAllBranch");
            chkSelectAllBranch.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvBranchDetails.ClientID + "',this,'chkSelectBranch');");
            //chkAll.Checked = true;
            if (ViewState["SelectAll"] != null)
            {
                bool SelectAll = bool.Parse(ViewState["SelectAll"].ToString());
                chkSelectAllBranch.Checked = SelectAll;
            }
            if (strMode == "Q")
            {
                chkSelectAllBranch.Enabled = false;
            }
        }
    }
}