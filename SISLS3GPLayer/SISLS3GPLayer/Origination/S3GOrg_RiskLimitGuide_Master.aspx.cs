
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
//using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.IO;

public partial class S3GOrg_RiskLimitGuide_Master : ApplyThemeForProject
{
    public static S3GOrg_RiskLimitGuide_Master Obj_Page;
    int intCompanyID, intUserID = 0;
    int intErrorCode;
    int intservice_id = 0;
    Dictionary<string, string> Procparam = null;
    UserInfo ObjUserInfo = new UserInfo();
    string strMode = string.Empty;
    DataTable dtDet = new DataTable();
    DataTable dtFAMap = new DataTable();
    
    String StrConnectionName;
    string strKey = "Insert";
    static string strPageName = "Risk Limit Guide Line Master Add";
    int intProgramId = 674;
    int int_Mst_id = 0;
    string strDateFormat = string.Empty;
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrg_RiskLimitGuide_Master.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrg_RiskLimitGuide_View.aspx'";
    string strRedirectPagecancel = "../Origination/S3GOrg_RiskLimitGuide_View.aspx";

    S3GSession objSession = new S3GSession();
    int intErrCode = 0;
    string strRiskLimitMasterID = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }

    }

    #region "EVENTS"

    #region"BUTTON EVENTS"

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
           

            dtFAMap = (DataTable)ViewState["dtExecutiveDetails"];

            if (dtFAMap.Rows.Count > 0)
            {
                if (Convert.ToString(dtFAMap.Rows[0]["RiskLimitDtlID"]) == "")
                {
                    dtFAMap.Rows[0].Delete();
                    dtFAMap.AcceptChanges();
                }
            }

            DataRow[] drduplicate = dtFAMap.Select("LOB_ID = '" + Convert.ToString(ddlLOB.SelectedValue) + "'");

            if (drduplicate != null && drduplicate.Length == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Risk Limit Details already exists");
                return;
            }

            DataRow drEmptyRow = dtFAMap.NewRow();
            drEmptyRow["SlNo"] = dtFAMap.Rows.Count + 1;
            drEmptyRow["RiskLimitDtlID"] = "0";
            drEmptyRow["LOB_ID"] = Convert.ToString(ddlLOB.SelectedValue);
            drEmptyRow["LOB"] = Convert.ToString(ddlLOB.SelectedItem.Text);
            drEmptyRow["Overdue_ID"] = Convert.ToString(txtOverDue.Text);
            drEmptyRow["Overdue"] = Convert.ToString(txtOverDue.Text);
            drEmptyRow["NPA_ID"] = Convert.ToString(txtNPA.Text);
            drEmptyRow["NPA"] = Convert.ToString(txtNPA.Text);
            drEmptyRow["Provision_ID"] = Convert.ToString(txtProvision.Text);
            drEmptyRow["Provision"] = Convert.ToString(txtProvision.Text);
            
            drEmptyRow["IS_MODIFY"] = "0";

            dtFAMap.Rows.Add(drEmptyRow);
            ViewState["dtExecutiveDetails"] = dtFAMap;

            FunFillgrid(grvDetails, dtFAMap);
            FunPriClearExecutiveDetails();
            ddlLOB.Focus();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        try
        {
           

            dtFAMap = (DataTable)ViewState["dtExecutiveDetails"];

            DataRow[] drduplicate = dtFAMap.Select("LOB_ID = '" + Convert.ToString(ddlLOB.SelectedValue) + "'");

            //if (drduplicate != null && drduplicate.Length == 1)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Risk Limit Details already exists");
            //    return;
            //}

            DataRow[] drExecutive = dtFAMap.Select("LOB_ID = '" + Convert.ToString(ddlLOB.SelectedValue) + "'");
            if (drExecutive != null && drExecutive.Length == 1)
            {
                drExecutive[0]["LOB_ID"] = Convert.ToString(ddlLOB.SelectedValue);
                drExecutive[0]["LOB"] = Convert.ToString(ddlLOB.SelectedItem.Text);
                drExecutive[0]["Overdue_ID"] = Convert.ToString(txtOverDue.Text);
                drExecutive[0]["Overdue"] = Convert.ToString(txtOverDue.Text);
                drExecutive[0]["NPA_ID"] = Convert.ToString(txtNPA.Text);
                drExecutive[0]["NPA"] = Convert.ToString(txtNPA.Text);
                drExecutive[0]["Provision_ID"] = Convert.ToString(txtProvision.Text);
                drExecutive[0]["Provision"] = Convert.ToString(txtProvision.Text);


                drExecutive[0]["IS_MODIFY"] = (strRiskLimitMasterID == "" || strRiskLimitMasterID == "0") ? "0" : "1";

                
                dtFAMap.AcceptChanges();
            }

            ViewState["dtExecutiveDetails"] = dtFAMap;
            btnModify.Enabled_False();
            btnAdd.Enabled_True();

            FunFillgrid(grvDetails, dtFAMap);
            FunPriClearExecutiveDetails();
            ddlLOB.Focus();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }
    protected void ddlToYearMonthBase_SelectedIndexChanged(object sender, EventArgs e)
    {
       
            if (ddlFromYearMonthBase.SelectedIndex > ddlToYearMonthBase.SelectedIndex)
            {
                Utility.FunShowAlertMsg(this, "To Year Month should be greater than or Equal to From Year and month ");
                ddlToYearMonthBase.ClearSelection();
                return;
            }
       
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            if ((Convert.ToDecimal(txtLongTermMin.Text) + Convert.ToDecimal(txtShortTermMax.Text)) > 100)
            {
                Utility.FunShowAlertMsg(this.Page, "Sum of Long Term Min and Short Term Max should not be greater than 100%.");
                return;
            }

            //if (Convert.ToDecimal(txtLongTermMax.Text) < Convert.ToDecimal(txtLongTermMin.Text))
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Long Term Min should not be greater than Long Term Max");
            //    return;
            //}

            dtDet = (DataTable)ViewState["dtExecutiveDetails"];

            if (dtDet.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Enter atleast one Risk Limit Guideline Details");
                return;
            }
            else if (Convert.ToString(dtDet.Rows[0]["RiskLimitDtlID"]) == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter atleast one Risk Limit Guideline Details");
                return;
            }
            
            dtFAMap = (DataTable)ViewState["FAMappingDetails"];

            if (dtFAMap.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Enter atleast one FA Mapping Details");
                return;
            }
            else if (Convert.ToString(dtFAMap.Rows[0]["FAMap_Detail_ID"]) == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter atleast one FA Mapping Details");
                return;
            }


            FunPriSaveDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnclear_Click(object sender, EventArgs e)
    {
        try
        {
            //ddloption.SelectedValue = "0";
            //txtAddStartDate.Text = System.DateTime.Now.ToString(strDateFormat);
            //txtAddEndDate.Text = "";
            txtCreditRiskH.Text = "";
            txtCreditRiskM.Text = "";
            txtCreditRiskML.Text = "";
            txtCreditRiskL.Text = "";

            
            ddlLOB.SelectedValue = "0";
            //FunBind_Effective_To();
            FunPriSetEmptyDtl();
            FunPriClearDetails();
            FunPriClearFAMapViewState();
            FunPriClearFAMapDetails();

          }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }


    private void FunPriSetEmptyDtl()
    {
        try
        {
            DataRow drEmptyRow;
            dtFAMap = new DataTable();
            dtFAMap.Columns.Add("SlNo");
            dtFAMap.Columns.Add("RiskLimitDtlID");
            dtFAMap.Columns.Add("LOB_ID");
            dtFAMap.Columns.Add("LOB");
            dtFAMap.Columns.Add("OverDue_ID");
            dtFAMap.Columns.Add("OverDue");
            dtFAMap.Columns.Add("NPA_ID");
            dtFAMap.Columns.Add("NPA");
            dtFAMap.Columns.Add("Provision_ID");
            dtFAMap.Columns.Add("Provision");
            
            drEmptyRow = dtFAMap.NewRow();
            dtFAMap.Rows.Add(drEmptyRow);
            ViewState["dtExecutiveDetails"] = dtFAMap;
            FunFillgrid(grvDetails, dtFAMap);
            grvDetails.Rows[0].Visible = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }


    protected void btnCleargrid_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }


    private void FunPriClearDetails()
    {
        try
        {
            ddlLOB.Enabled = true;
            lblExecutiveSlNo.Text = string.Empty;
            txtOverDue.Text="";
            txtNPA.Text="";
            txtProvision.Text = "";
            btnAdd.Enabled_True();
            btnModify.Enabled_False();
            FunPriResetRdButton(grvDetails, -1);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }


    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPagecancel);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnFAMapAdd_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            if (!FunPriCheckFAMapDetails())
                return;

            dtFAMap = (DataTable)ViewState["FAMappingDetails"];

            if (dtFAMap.Rows.Count > 0)
            {
                if (Convert.ToString(dtFAMap.Rows[0]["FAMap_Detail_ID"]) == "")
                {
                    dtFAMap.Rows[0].Delete();
                    dtFAMap.AcceptChanges();
                }
            }

            DataRow[] drduplicate = dtFAMap.Select("GL_Code = '" + Convert.ToString(ddlGLCode.SelectedText) + "'");

            if (drduplicate != null && drduplicate.Length == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "GL Code already exists");
                return;
            }


            DataRow drEmptyRow = dtFAMap.NewRow();
            drEmptyRow["SlNo"] = dtFAMap.Rows.Count + 1;
            //drEmptyRow["Incentive_FAMap_ID"] = "0";
            drEmptyRow["FAMap_Detail_ID"] = Convert.ToString(ddlGLCode.SelectedValue);
            drEmptyRow["GL_Code"] = Convert.ToString(ddlGLCode.SelectedText);
            drEmptyRow["GL_Desc"] = Convert.ToString(txtGLDesc.Text);
            drEmptyRow["Provision_Coverage"] = Convert.ToString(txtProvCovrg.Text);
            drEmptyRow["IS_MODIFY"] = "0";
            
            dtFAMap.Rows.Add(drEmptyRow);
            ViewState["FAMappingDetails"] = dtFAMap;

            FunPriBindFAMappingGrid();
            FunPriClearFAMapDetails();
            ddlGLCode.Focus();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnFAMapModify_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            if (!FunPriCheckFAMapDetails())
                return;

            dtFAMap = (DataTable)ViewState["FAMappingDetails"];

            if (dtFAMap.Rows.Count > 0)
            {
                if (Convert.ToString(dtFAMap.Rows[0]["FAMap_Detail_ID"]) == "")
                {
                    dtFAMap.Rows[0].Delete();
                    dtFAMap.AcceptChanges();
                }
            }

            DataRow[] drduplicate = dtFAMap.Select("GL_Code = '" + Convert.ToString(ddlGLCode.SelectedValue) + "'");

            if (drduplicate != null && drduplicate.Length == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "GL Code already exists");
                return;
            }


            DataRow[] drFAMap = dtFAMap.Select("SlNo = '" + Convert.ToString(lblFAMapSlNo.Text) + "'");
            if (drFAMap != null && drFAMap.Length == 1)
            {
                drFAMap[0]["FAMap_Detail_ID"] = Convert.ToString(ddlGLCode.SelectedValue);
                drFAMap[0]["GL_Code"] = Convert.ToString(ddlGLCode.SelectedText);
                drFAMap[0]["GL_Desc"] = Convert.ToString(txtGLDesc.Text);
                drFAMap[0]["Provision_Coverage"] = Convert.ToString(txtProvCovrg.Text);
                drFAMap[0]["IS_MODIFY"] = (strRiskLimitMasterID == "" || strRiskLimitMasterID == "0") ? "0" : "1";
                
                
                dtFAMap.AcceptChanges();
            }

            ViewState["FAMappingDetails"] = dtFAMap;

            FunPriBindFAMappingGrid();
            FunPriClearFAMapDetails();
            ddlGLCode.Focus();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnFAMapClear_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            FunPriClearFAMapDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "TEXTBOX EVENTS"



    #endregion

    #region "RADIO BUTTON EVENTS"

    protected void RBDetailsSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int intRowIndex = Utility.FunPubGetGridRowID("grvDetails", ((RadioButton)sender).ClientID);

            Label lblgvexeExecutiveid = (Label)grvDetails.Rows[intRowIndex].FindControl("lblLOBid");
            Label lblgvexeSerialNo = (Label)grvDetails.Rows[intRowIndex].FindControl("lblgvSerialNo");
            dtFAMap = (DataTable)ViewState["dtExecutiveDetails"];

            DataRow[] drExecutive = dtFAMap.Select("LOB_ID = '" + Convert.ToString(lblgvexeExecutiveid.Text) + "' and slno = '" + Convert.ToString(lblgvexeSerialNo.Text) + "'");
            if (drExecutive != null && drExecutive.Length == 1)
            {
                ddlLOB.SelectedValue = Convert.ToString(drExecutive[0]["LOB_ID"]);
                txtOverDue.Text = Convert.ToString(drExecutive[0]["OverDue"]);
                txtNPA.Text = Convert.ToString(drExecutive[0]["NPA"]);
                txtProvision.Text = Convert.ToString(drExecutive[0]["Provision"]);

                if (Convert.ToString(drExecutive[0]["RiskLimitDtlID"]) != "" && Convert.ToString(drExecutive[0]["RiskLimitDtlID"]) != "0")
                {
                    ddlLOB.Enabled = false;
                 }
            }

            FunPriResetRdButton(grvDetails, intRowIndex);

            btnAdd.Enabled_False();
            btnModify.Enabled_True();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void rdbtnFAMapSelect_CheckedChanged(object sender, System.EventArgs e)
    {
        try
        {
            int intRowIndex = Utility.FunPubGetGridRowID("grvFAMapping", ((RadioButton)sender).ClientID);

            Label lblgvFAMapDetailID = (Label)grvFAMapping.Rows[intRowIndex].FindControl("lblgvFAMap_Detail_ID");
            Label lblgvFAMapSlNo = (Label)grvFAMapping.Rows[intRowIndex].FindControl("lblgvFAMapSlNo");
            dtFAMap = (DataTable)ViewState["FAMappingDetails"];

            DataRow[] drFAMap = dtFAMap.Select("FAMap_Detail_ID = '" + Convert.ToString(lblgvFAMapDetailID.Text) + "' and slno = '" + Convert.ToString(lblgvFAMapSlNo.Text) + "'");
            if (drFAMap != null && drFAMap.Length == 1)
            {
                ddlGLCode.SelectedValue = Convert.ToString(drFAMap[0]["FAMap_Detail_ID"]);
                ddlGLCode.SelectedText = Convert.ToString(drFAMap[0]["GL_Code"]);
                txtGLDesc.Text = Convert.ToString(drFAMap[0]["GL_Desc"]);
                txtProvCovrg.Text = Convert.ToString(drFAMap[0]["Provision_Coverage"]);
                lblFAMapSlNo.Text = Convert.ToString(drFAMap[0]["SlNo"]);
                

                ddlGLCode.ReadOnly = true;
            }

            FunPriResetFAMapRdButton(grvFAMapping, intRowIndex);

            btnFAMapAdd.Enabled_False();
            btnFAMapModify.Enabled_True();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "GRID VIEW EVENTS"

    protected void grvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int i = e.RowIndex;
            Label lblSerialNo = grvDetails.Rows[i].FindControl("lblgvSerialNo") as Label;
            string serialno = lblSerialNo.Text.ToString();

            dtFAMap = (DataTable)ViewState["dtExecutiveDetails"];

            DataRow drow = dtFAMap.Rows[Convert.ToInt32(lblSerialNo.Text) - 1];
            drow.Delete();
            dtFAMap.AcceptChanges();
            if (dtFAMap != null)
            {
                if (grvDetails.Rows.Count > 0)
                {
                    grvDetails.DataSource = dtFAMap;
                    grvDetails.DataBind();
                }
                else
                {
                    grvDetails.DataSource = null;
                    grvDetails.DataBind();
                    FunPriSetEmptyExecutiveDtl();
                }
            }

            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();

            //Procparam.Add("@service_id", service_id);

            //DataTable dtdata = Utility.GetDefaultData("FA_Tax_Delete_NOSST", Procparam);
            //if (dtdata.Rows[0]["errorcode"].ToString() == "0")
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Service code Deleted successfully");
            //    PopulateGrid();

            //}
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }


    protected void grvFAMapping_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        try
        {
            int i = e.RowIndex;
            Label lblgvFAMapSlNo = grvFAMapping.Rows[i].FindControl("lblgvFAMapSlNo") as Label;

            dtFAMap = (DataTable)ViewState["FAMappingDetails"];

            DataRow[] drduplicate = dtFAMap.Select("SlNo = '" + Convert.ToString(lblgvFAMapSlNo.Text) + "'");
            if (drduplicate != null && drduplicate.Length == 1)
            {
                drduplicate[0].Delete();
            }

            dtFAMap.AcceptChanges();

            if (dtFAMap != null)
            {
                if (grvFAMapping.Rows.Count > 0)
                {
                    for (int _i = 0; _i < dtFAMap.Rows.Count; _i++)
                    {
                        dtFAMap.Rows[_i]["SlNo"] = Convert.ToString(_i + 1);
                    }
                    dtFAMap.AcceptChanges();
                    grvFAMapping.DataSource = dtFAMap;
                    grvFAMapping.DataBind();
                }
                else
                {
                    grvFAMapping.DataSource = null;
                    grvFAMapping.DataBind();
                    FunPriSetEmptyFAMapDetails();
                }
            }

            ViewState["FAMappingDetails"] = dtFAMap;

            dtFAMap = (DataTable)ViewState["FAMapHoldMarginDetails"];
            drduplicate = dtFAMap.Select("FAMap_Sl_No = '" + Convert.ToString(lblgvFAMapSlNo.Text) + "'");

            if (drduplicate != null && drduplicate.Length > 0)
            {
                foreach (DataRow dr in drduplicate)
                {
                    dr.Delete();
                }
            }

            dtFAMap.AcceptChanges();

            drduplicate = dtFAMap.Select("FAMap_Sl_No > '" + Convert.ToString(lblgvFAMapSlNo.Text) + "'");

            if (drduplicate != null && drduplicate.Length > 0)
            {
                foreach (DataRow dr in drduplicate)
                {
                    dr["FAMap_Sl_No"] = Convert.ToString(Convert.ToInt32(dr["FAMap_Sl_No"]) - 1);
                }
            }

            ViewState["FAMapHoldMarginDetails"] = dtFAMap;
            FunPriClearFAMapDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void grvFAMapping_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkbtngvFAMapHoldPErcentage = (LinkButton)e.Row.FindControl("lnkbtngvFAMapHoldPErcentage");
                lnkbtngvFAMapHoldPErcentage.Text = Convert.ToDecimal(lnkbtngvFAMapHoldPErcentage.Text).ToString(FunsetPercentagesuffix());
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "DROPDOWN EVENTS"

   

    //protected void ddlLOB_SelectedIndexChanged(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        FunPriSetEmptyExecutiveDtl();
    //        FunPriClearExecutiveDetails();
    //        FunPriClearFAMapViewState();
    //        FunPriClearFAMapDetails();
    //        ddlGLCode.ReadOnly = (Convert.ToInt32(ddlLOB.SelectedValue) > 0 && Convert.ToInt32(ddlLOB.SelectedValue) > 0) ? false : true;
    //        //ddlEmployeeName.ReadOnly = (Convert.ToInt32(ddlLOB.SelectedValue) > 0 && Convert.ToInt32(ddlLOB.SelectedValue) > 0) ? false : true;
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
    //    }
    //}

    #endregion

    #region "USER CONTROL EVENTS"

    protected void ddlGLCode_Item_Selected(object Sender, System.EventArgs e)
    {
        try
        {
            if (Convert.ToString(ddlGLCode.SelectedText) != "" && Convert.ToString(ddlGLCode.SelectedValue) != "0")
            {
                string[] strFAMapCode = Convert.ToString(ddlGLCode.SelectedText).Split('-');
                txtGLDesc.Text = strFAMapCode[1];
                ddlGLCode.SelectedText = strFAMapCode[0];
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "LINK BUTTON EVENTS"


    #endregion

    #endregion

    #region "FUNCTIONS"

    private void FunPriLoadPage()
    {
        try
        {
            Obj_Page = this;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            //User Authorization
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            //StrConnectionName = objSession.ProConnectionName;
            strDateFormat = objSession.ProDateFormatRW;

            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyID.ToString();

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strMode = Request.QueryString.Get("qsMode");
                strRiskLimitMasterID = fromTicket.Name;
            }
            else
            {
                strRiskLimitMasterID = string.Empty;
            }

            
            if (!IsPostBack)
            {
                FunPriGetIncentiveMstLOV();
                //txtAddStartDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtAddStartDate.ClientID + "','" + strDateFormat + "',false,  true);");
                //txtAddEndDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtAddEndDate.ClientID + "','" + strDateFormat + "',false,  true);");

                txtTrading.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, 2, true, "txtTrading");
                txtConstruc.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, 2, true, "txtConstruc");
                txtServices.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, 2, true, "txtServices");
                txtManufacture.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, 2, true, "txtManufacture");
                txtPersonal.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, 2, true, "txtPersonal");

                
                txtGLDesc.ReadOnly = true;
                

                if (Request.QueryString["qsMode"] == "M")
                {
                    FunPriGetDetails(strRiskLimitMasterID);
                    FunPriDisableControls(1);
                }
                else if (Request.QueryString["qsMode"] == "Q")
                {
                    FunPriGetDetails(strRiskLimitMasterID);
                    FunPriDisableControls(-1);
                }
                else
                {
                    // FunPriLoadGroup();
                    FunPriSetEmptyExecutiveDtl();
                    FunPriDisableControls(0);
                }

                //txtTargetgrid.SetDecimalPrefixSuffix(objSession.ProGpsPrefixRW, objSession.ProGpsSuffixRW, true, "Target");
                ddlLOB.Focus();
            }

        }
        catch (Exception objexception)
        {
            throw objexception;
        }
    }

    private void FunPriSetEmptyExecutiveDtl()
    {
        try
        {
            DataRow drEmptyRow;
            dtFAMap = new DataTable();
            dtFAMap.Columns.Add("SlNo");
            dtFAMap.Columns.Add("RiskLimitDtlID");
            dtFAMap.Columns.Add("LOB_ID");
            dtFAMap.Columns.Add("LOB");
            dtFAMap.Columns.Add("OverDue_ID");
            dtFAMap.Columns.Add("OverDue");
            dtFAMap.Columns.Add("NPA_ID");
            dtFAMap.Columns.Add("NPA");
            dtFAMap.Columns.Add("Provision_ID");
            dtFAMap.Columns.Add("Provision");
            dtFAMap.Columns.Add("IS_MODIFY");
            
            drEmptyRow = dtFAMap.NewRow();
            dtFAMap.Rows.Add(drEmptyRow);
            ViewState["dtExecutiveDetails"] = dtFAMap;
            FunFillgrid(grvDetails, dtFAMap);
            grvDetails.Rows[0].Visible = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunFillgrid(GridView grv, DataTable dtbl)
    {
        try
        {
            grv.DataSource = dtbl;
            grv.DataBind();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void FunPriClearExecutiveDetails()
    {
        try
        {
            ddlLOB.Enabled = true;

            lblExecutiveSlNo.Text = string.Empty;

            txtOverDue.Text = "";
            txtNPA.Text = "";
            txtProvision.Text = "";


            btnAdd.Enabled_True();
            btnModify.Enabled_False();
            FunPriResetRdButton(grvDetails, -1);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriResetRdButton(GridView grv, int intRowIndex)
    {
        try
        {
            for (int i = 0; i <= grv.Rows.Count - 1; i++)
            {
                if (i != intRowIndex)
                {
                    RadioButton rdSelect = grv.Rows[i].FindControl("RBSelect") as RadioButton;
                    rdSelect.Checked = false;
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriResetFAMapRdButton(GridView grv, int intRowIndex)
    {
        try
        {
            for (int i = 0; i <= grv.Rows.Count - 1; i++)
            {
                if (i != intRowIndex)
                {
                    RadioButton rdSelect = grv.Rows[i].FindControl("rdbtnFAMapSelect") as RadioButton;
                    rdSelect.Checked = false;
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private int FunPriSaveDetails()
    {
        S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_RiskGuideLineLimitDataTable objMaster_DTB=new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_RiskGuideLineLimitDataTable();
        S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_RiskGuideLineLimitRow objMasterrow;
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {
            string strGroupDocNumber = string.Empty;
            string strAlert = "alert('__ALERT__');";

            objMasterrow = objMaster_DTB.NewS3G_ORG_RiskGuideLineLimitRow();
            objMasterrow.Risk_Limit_Master_ID = (strRiskLimitMasterID == "" || strRiskLimitMasterID == "0") ? 0 : Convert.ToInt32(strRiskLimitMasterID);

            objMasterrow.From_Month =ddlFromYearMonthBase.SelectedValue;
            objMasterrow.To_Month = ddlToYearMonthBase.SelectedValue;
            
            objMasterrow.CreditRiskH = txtCreditRiskH.Text;
            objMasterrow.CreditRiskM = txtCreditRiskM.Text;
            objMasterrow.CreditRiskML = txtCreditRiskML.Text;
            objMasterrow.CreditRiskL = txtCreditRiskL.Text;

            objMasterrow.LongTermMax = txtLongTermMax.Text;
            objMasterrow.LongTermMin = txtLongTermMin.Text;
            objMasterrow.ShortTermMax = txtShortTermMax.Text;
            objMasterrow.ShortTermMin = txtShortTermMin.Text;

            objMasterrow.Trading = txtTrading.Text;
            objMasterrow.Construction = txtConstruc.Text;
            objMasterrow.Services = txtServices.Text;
            objMasterrow.Manufacturing = txtManufacture.Text;
            objMasterrow.Personal = txtPersonal.Text;
            objMasterrow.Quick_Mortality_Months = txtQuickMor.Text;
            objMasterrow.Created_By = intUserID;

            objMasterrow.XML_Dtl = Utility.FunPubFormXml(((DataTable)ViewState["dtExecutiveDetails"]), true);
            objMasterrow.XML_FAMap_Details = Utility.FunPubFormXml(((DataTable)ViewState["FAMappingDetails"]), true);
            
            objMaster_DTB.AddS3G_ORG_RiskGuideLineLimitRow(objMasterrow);

            S3GBusEntity.SerializationMode SerMode = S3GBusEntity.SerializationMode.Binary;

            byte[] ObjMasterDataTable = ClsPubSerialize.Serialize(objMaster_DTB, SerMode);
            intErrCode = objMasterClient.FunPubCreateRiskLimitGuidelineMaster(out strGroupDocNumber, SerMode, ObjMasterDataTable);
            if (intErrCode == 0)
            {
                if (strRiskLimitMasterID == "" || strRiskLimitMasterID == "0")
                {
                    strAlert = Convert.ToString("Risk Limit Guideline Master added successfully");
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    strRedirectPageView = "";
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Risk Limit Guideline Master updated successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            else if (intErrCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
            }
            else if (intErrCode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Risk Limit Guideline master details already exists");
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.Save_Error);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);

        }
        finally
        {
            //if (objFAMap_MasterClient != null)
            //    objFAMap_MasterClient.Close();

        }
        return intErrCode;
    }

    private void FunPriGetDetails(string strRiskLimitMasterID)
    {
        try
        {
            pnlTolerance.Visible = true;
            //DataTable ds.Tables[0] = new DataTable();
            DataSet ds = new DataSet();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            Procparam.Add("@Risk_Limit_Master_ID", strRiskLimitMasterID);
            ds = Utility.GetDataset("S3G_CLN_GET_RISK_LIMIT_GL", Procparam);
            if (ds.Tables[0].Rows.Count > 0)
            {
                System.Web.UI.WebControls.ListItem objList;
                objList = new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[0]["FROM_MONTH"].ToString(), ds.Tables[0].Rows[0]["FROM_MONTH"].ToString());
                ddlFromYearMonthBase.Items.Insert(0, objList);

                System.Web.UI.WebControls.ListItem objList1;
                objList1 = new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[0]["TO_MONTH"].ToString(), ds.Tables[0].Rows[0]["TO_MONTH"].ToString());
                ddlToYearMonthBase.Items.Insert(0, objList1);
                
                txtCreditRiskH.Text = Convert.ToString(ds.Tables[0].Rows[0]["CreditRiskH"]);
                txtCreditRiskM.Text =Convert.ToString(ds.Tables[0].Rows[0]["CreditRiskM"]);
                txtCreditRiskML.Text =Convert.ToString(ds.Tables[0].Rows[0]["CreditRiskML"]);
                txtCreditRiskL.Text = Convert.ToString(ds.Tables[0].Rows[0]["CreditRiskL"]);
                txtTrading.Text = Convert.ToString(ds.Tables[0].Rows[0]["Trading"]);
                txtConstruc.Text = Convert.ToString(ds.Tables[0].Rows[0]["Construction"]);
                txtServices.Text = Convert.ToString(ds.Tables[0].Rows[0]["Services"]);
                txtManufacture.Text = Convert.ToString(ds.Tables[0].Rows[0]["Manufacturing"]);
                txtPersonal.Text = Convert.ToString(ds.Tables[0].Rows[0]["Personal"]);
                txtQuickMor.Text = Convert.ToString(ds.Tables[0].Rows[0]["Quick_Mortality"]);
                txtLongTermMax.Text = Convert.ToString(ds.Tables[0].Rows[0]["LongTerm_Max"]);
                txtLongTermMin.Text = Convert.ToString(ds.Tables[0].Rows[0]["LongTerm_Min"]);
                txtShortTermMax.Text = Convert.ToString(ds.Tables[0].Rows[0]["ShortTerm_Max"]);
                txtShortTermMin.Text = Convert.ToString(ds.Tables[0].Rows[0]["ShortTerm_Min"]);

            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                ViewState["dtExecutiveDetails"] = ds.Tables[1];
                grvDetails.DataSource = ds.Tables[1];
                grvDetails.DataBind();
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                ViewState["FAMappingDetails"] = ds.Tables[2];
                grvFAMapping.DataSource = ds.Tables[2];
                grvFAMapping.DataBind();
            }

           
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode                    
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    //FunBind_Effective_To();
                    FunPriBindFAMappingGrid();
                    //txtAddStartDate.Text = System.DateTime.Now.ToString(strDateFormat);
                    btnModify.Enabled_False();
                    btnFAMapModify.Enabled_False();
                    //ddlGLCode.ReadOnly = true;
                    ddlFromYearMonthBase.FillFinancialMonth(FunPriLoadCurrentFinancialYear());
                    ddlToYearMonthBase.FillFinancialMonth(FunPriLoadCurrentFinancialYear());
                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    //txtGroup.ReadOnly = true;
                    btnclear.Enabled_False();
                    //ddlLOB.ClearDropDownList();
                    btnModify.Enabled_False();
                    btnFAMapModify.Enabled_False();

                    ddlFromYearMonthBase.Enabled = ddlToYearMonthBase.Enabled = false;

                    break;

                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled_False();
                    btnclear.Enabled_False();
                    btnAdd.Enabled_False();
                    btnCleargrid.Enabled_False();
                    btnFAMapAdd.Enabled_False();
                    btnFAMapClear.Enabled_False();
                    btnModify.Enabled_False();
                    btnFAMapModify.Enabled_False();
                    ddlLOB.ClearDropDownList();
                    txtCreditRiskH.ReadOnly =
                    txtCreditRiskM.ReadOnly=txtCreditRiskML.ReadOnly=txtCreditRiskL.ReadOnly=true;
                    
                                       
                    ddlGLCode.ReadOnly = true;
                    ddlFromYearMonthBase.Enabled = ddlToYearMonthBase.Enabled = false;
                    
                    grvDetails.Columns[grvDetails.Columns.Count - 1].Visible = false;
                    grvFAMapping.Columns[grvFAMapping.Columns.Count - 1].Visible = false;
                    grvDetails.Columns[0].Visible = false;
                    grvFAMapping.Columns[0].Visible = false;
                    break;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }

    }

    protected void ddlFromYearMonthBase_SelectedIndexChanged(object sender, EventArgs e)
    {
        
            if (ddlToYearMonthBase.SelectedIndex > 0)
            {
                if (ddlFromYearMonthBase.SelectedIndex > ddlToYearMonthBase.SelectedIndex)
                {
                    Utility.FunShowAlertMsg(this, "From Year Month should not be greater than the To Year and month ");
                    ddlFromYearMonthBase.ClearSelection();


                    return;
                }
            }
    
    }

    private void FunPriLoadGroup()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            // Procparam.Add("@Location_code", ddloption.SelectedValue);
            // ddlGroup.BindDataTable("FA_GET_FAMap_Group", Procparam, new string[] { "GROUP_NAME", "GROUP_NAME" });
        }
        catch (Exception ex)
        {
            //    ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    private string FunPriLoadCurrentFinancialYear()
    {
        int intCurrentYear = DateTime.Now.Year;
        int intCurrentMonth = DateTime.Now.Month;
        int intFinancialYearStartMonth = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StartMonth"]);
        if (intCurrentMonth >= intFinancialYearStartMonth)
        {
            return Convert.ToString(intCurrentYear) + "-" + Convert.ToString(intCurrentYear + 1);
        }
        else
        {
            return Convert.ToString(intCurrentYear - 1) + "-" + Convert.ToString(intCurrentYear);
        }
    }


    private void FunPriGetIncentiveMstLOV()
    {
        try
        {
            FunPriGetLOBList();
            
            //Procparam = Utility.FunPubClearDictParam(Procparam);
            //Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            //Procparam.Add("@USER_ID", Convert.ToString(intUserID));
            //Procparam.Add("@OPTION", "1");
            //DataSet dsLkup = Utility.GetDataset("S3G_CLN_GET_MRKTINCLKPUP", Procparam);

            
            }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable To Lookup");
        }
    }


    private void FunPriGetLOBList()
    {
        try
        {
            Procparam = Utility.FunPubClearDictParam(Procparam);
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (Convert.ToString(strRiskLimitMasterID) == "" || Convert.ToInt32(strRiskLimitMasterID) == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@Program_ID", Convert.ToString(intProgramId));
            ddlLOB.BindDataTable("S3G_Get_LOB_LIST", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.AddItemToolTip();
        }
        catch (Exception objException)
        {
            throw objException;
            throw new ApplicationException("Unable To Load Line of Business");
        }
    }

    private void FunPriSetEmptyFAMapDetails()
    {
        try
        {
            DataTable dtFAMapDetails = new System.Data.DataTable();

            dtFAMapDetails.Columns.Add("SlNo");
            dtFAMapDetails.Columns.Add("FAMap_Detail_ID");
            dtFAMapDetails.Columns.Add("GL_Code");
            dtFAMapDetails.Columns.Add("GL_Desc");
            dtFAMapDetails.Columns.Add("Provision_Coverage");
            dtFAMapDetails.Columns.Add("Is_Modify");
            
            DataRow drFAMap = dtFAMapDetails.NewRow();
            dtFAMapDetails.Rows.Add(drFAMap);

            ViewState["FAMappingDetails"] = dtFAMapDetails;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriBindFAMappingGrid()
    {
        try
        {
            if (ViewState["FAMappingDetails"] == null)
                FunPriSetEmptyFAMapDetails();

            grvFAMapping.DataSource = (DataTable)ViewState["FAMappingDetails"];
            grvFAMapping.DataBind();

            if (Convert.ToString(((DataTable)ViewState["FAMappingDetails"]).Rows[0]["FAMap_Detail_ID"]) == "")
                grvFAMapping.Rows[0].Visible = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearFAMapDetails()
    {
        try
        {
            ddlGLCode.ReadOnly = false;
            ddlGLCode.Clear();
            txtGLDesc.Text = lblFAMapSlNo.Text = txtProvCovrg.Text = string.Empty;
            FunPriResetFAMapRdButton(grvFAMapping, -1);
            btnFAMapAdd.Enabled_True();
            btnFAMapModify.Enabled_False();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private bool FunPriCheckFAMapDetails()
    {
        bool dlnRslt = true;
        try
        {
            if (Convert.ToString(ddlGLCode.SelectedText) == "" || Convert.ToInt32(ddlGLCode.SelectedValue) == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Enter GL Code");
                return false;
            }

          
        }
        catch (Exception objException)
        {
            dlnRslt = false;
        }
        return dlnRslt;
    }


    private void FunPriClearFAMapViewState()
    {
        try
        {
            ViewState["FAMappingDetails"] = null;
            FunPriBindFAMappingGrid();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }


    private string Funsetsuffix()
    {
        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    private string FunsetPercentagesuffix()
    {
        int suffix = 2;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    protected void FunBind_Effective_To()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "793");
            Procparam.Add("@Param1", Convert.ToString(intCompanyID));
            DataTable dtdata = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);
            if (dtdata.Rows.Count > 0)
            {
                ddlToYearMonthBase.Text = dtdata.Rows[0][0].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }


    #endregion

    //protected void ddloption_SelectedIndexChanged(object sender, System.EventArgs e)
    //{
    //    FunPriLoadGroup();
    //    //ddloption.Focus();
    //}

    #region "WEB METHODS"

   

    [System.Web.Services.WebMethod]
    public static string[] GetGLCodeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Add("@Company_ID", "1");
        //Procparam.Add("@PARAM1", Convert.ToString(Obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@GL_Code", prefixText);
        //Procparam.Add("@Option", "2");
        //Procparam.Add("@PARAM1", Convert.ToString(Obj_Page.ddlLOB.SelectedValue));

        DataSet dsFAMapList = Utility.GetDataset("S3G_ORG_GET_RISK_LIMIT_GL_CODE", Procparam);

        suggestions = Utility.GetSuggestions(dsFAMapList.Tables[0]);
        return suggestions.ToArray();
    }

    #endregion


}