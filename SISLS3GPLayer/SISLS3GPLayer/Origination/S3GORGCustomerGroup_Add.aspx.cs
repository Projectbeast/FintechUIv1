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
using System.Text;

public partial class Origination_S3GOrgComplainceMaster_Add : ApplyThemeForProject
{
    //Created by :Sathish R
    //Created on:24-Jan-2020

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
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGCustomerGroup_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=CUTGRP&Create=1&Modify=1';";
    string strRedirectPagecancel = "../Origination/S3GORGTransLander.aspx?Code=CUTGRP&Create=1&Modify=1";

    ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable ObjMasterDataTable;
    ORGSERVICE.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    public string strDateFormat;
    S3GSession objSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    DataTable dtCustGroup = null;
    DataSet dsCollectionDtls = new DataSet();
    string strCustomerGroupId = string.Empty;
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
            //CalendarExtenderEffectiveFromDate.Format = strDateFormat;                       // assigning the first textbox with the End date

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            intUserLevelID = ObjUserInfo.ProUserLevelIdRW;
            Session["AutoSuggestCompanyID"] = intCompanyId;
            Session["User_Id"] = intUserId;
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strMode = Request.QueryString.Get("qsMode");
                strCustomerGroupId = fromTicket.Name;
            }
            else
            {
                strCustomerGroupId = string.Empty;
            }


            if (!IsPostBack)
            {
                //txtGroupLimit.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, lblGroupLimit.Text);
                txtGroupLimiExpDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtGroupLimiExpDate.ClientID + "','" + strDateFormat + "',false,  false);");
                calGroupLimiExpDate.Format = strDateFormat;

                FunBind_Effective_To();
                FunPriLoadCommonData();
                //btnSave.Visible = false;
                //btnCancel.Visible = false;
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
            }

            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txtCustItemNumber = ((TextBox)ucCustomerCodeLov.FindControl("txtItemName"));
            txtCustItemNumber.Style["display"] = "block";
            txtCustItemNumber.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtCustItemNumber.Attributes.Add("readonly", "false");
            txtCustItemNumber.Width = 0;
            txtCustItemNumber.TabIndex = -1;
            txtCustItemNumber.BorderStyle = BorderStyle.None;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriGetCustomerGroupDetails(int intCustGroupId)
    {
        try
        {
            DataSet ds = new DataSet();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CompanyID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@CustGroupID", strCustomerGroupId);
            Procparam.Add("@Option", "1");
            ds = Utility.GetDataset("S3G_OR_GET_CustGroupDetails", Procparam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtGroupCode.Text = ds.Tables[0].Rows[0]["CUST_GROUP_CODE"].ToString();
                    txtGroupName.Text = ds.Tables[0].Rows[0]["CUST_GROUP_NAME"].ToString();
                    txtGroupLimit.Text = ds.Tables[0].Rows[0]["CUST_GROUP_LIMIT"].ToString();
                    txtGroupLimiExpDate.Text = ds.Tables[0].Rows[0]["CUST_GROUP_LIMIT_EXP"].ToString();
                    if (ds.Tables[0].Rows[0]["Status"].ToString() == "1")
                    {
                        chkHActive.Checked = true;
                    }
                    else
                        chkHActive.Checked = false;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewState["GROUP_CUST"] = ds.Tables[1];
                    grvCustSubLimit.DataSource = (DataTable)ViewState["GROUP_CUST"];
                    grvCustSubLimit.DataBind();
                }

                //Below if added by praba on 29-12-2020
                if (ds.Tables[2].Rows.Count > 0)
                {
                    gvLimit_transfer_history.DataSource = ds.Tables[2];
                    gvLimit_transfer_history.DataBind();
                }
                else
                {
                    gvLimit_transfer_history.EmptyDataText = "No Records Found";
                    gvLimit_transfer_history.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunBindFrom_To_Cusotmer_Details(int intCustGroupId)
    {
        try
        {
            DataSet dsCustDet = new DataSet();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CompanyID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@CustGroupID", strCustomerGroupId);
            Procparam.Add("@Option", "1");
            dsCustDet = Utility.GetDataset("S3G_OR_GET_CUS_GRP_LIMIT_TRANS", Procparam);
            if (dsCustDet.Tables[0].Rows.Count > 0)
            {
                ddlFromCustomer.FillDataTable(dsCustDet.Tables[0], "CUSTOMER_ID", "CUSTOMER_NAME");
                ddlToCustomer.FillDataTable(dsCustDet.Tables[0], "CUSTOMER_ID", "CUSTOMER_NAME");
                ViewState["Customer_Det"] = dsCustDet.Tables[0];
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
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    btnCancel.Enabled_True();
                    btnSave.Enabled_True();
                    btnClear.Enabled_False();
                    txtGroupCode.Enabled = false;
                    txtGroupName.Enabled = false;
                    if (strCustomerGroupId != "0" && strCustomerGroupId != "")
                    {
                        int intCustGroupId = Convert.ToInt32(strCustomerGroupId);
                        FunPriGetCustomerGroupDetails(intCustGroupId);
                        FunBindFrom_To_Cusotmer_Details(intCustGroupId); //Added By Praba on 29-12-2020
                    }
                    break;

                case -1:// Query Mode
                    chkHActive.Enabled = false;
                    if (strCustomerGroupId != "0" && strCustomerGroupId != "")
                    {
                        int intCustGroupId = Convert.ToInt32(strCustomerGroupId);
                        FunPriGetCustomerGroupDetails(intCustGroupId);
                    }
                    btnCancel.Enabled_True();
                    btnClear.Enabled_False();
                    btnSave.Enabled_False();
                    txtGroupCode.Enabled = false;
                    txtGroupName.Enabled = false;
                    txtGroupLimit.Enabled = false;
                    txtGroupLimiExpDate.Enabled = false;
                    lnkAdd.Enabled_False();
                    ucCustomerCodeLov.Enabled = false;
                    Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
                    btnGetLOV.Enabled = false;
                    foreach (GridViewRow grv in grvCustSubLimit.Rows)
                    {
                        LinkButton lnkRemove = grv.FindControl("lnkRemove") as LinkButton;
                        lnkRemove.Enabled = false;
                        lnkRemove.OnClientClick = "";
                    }
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPagecancel);
            btnCancel.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GROUP_CUST"] == null)
            {
                Utility.FunShowAlertMsg(this, "Add customers for the group");
                return;
            }

            if (ViewState["GROUP_CUST"] != null)
            {
                DataTable dtCust = new DataTable();
                dtCust = (DataTable)ViewState["GROUP_CUST"];
                if (dtCust.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Add customers for the group");
                    return;
                }
            }
            DateTime datCurrDate = Utility.StringToDate(DateTime.Today.ToString(strDateFormat));
            DateTime datExpDate = Utility.StringToDate(txtGroupLimiExpDate.Text);
            if (datExpDate <= datCurrDate)
            {
                Utility.FunShowAlertMsg(this, "Group limit expiry Date should be greater than Current Date");
                return;
            }

            //Limit Transfer Validation start by Prabba on 01-01-2021
            if (!string.IsNullOrEmpty(ddlFromCustomer.SelectedValue))
            {
                if (Convert.ToInt32(ddlFromCustomer.SelectedValue) > 0)
                {
                    if (ddlToCustomer.SelectedValue == "" || ddlToCustomer.SelectedValue == "0")
                    {
                        Utility.FunShowAlertMsg(this, "Select to customer");
                        return;
                    }
                    if (txtTransferAmount.Text == "")
                    {
                        Utility.FunShowAlertMsg(this, "Transfer amount should not be empty");
                        return;
                    }
                }
            }
            //Limit Transfer Validation end by Prabba on 01-01-2021

            //Transfer Amount Validation Start by Praba on 29-12-2020
            if (!string.IsNullOrEmpty(txtTransferAmount.Text) && !string.IsNullOrEmpty(txtAvailableLimit.Text))
            {
                if (Convert.ToInt64(txtTransferAmount.Text) > 0 && Convert.ToInt64(txtAvailableLimit.Text) > 0)
                {
                    if (Convert.ToInt64(txtTransferAmount.Text) > Convert.ToInt64(txtAvailableLimit.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Transfer amount should be less than or equal to available limit");
                        return;
                    }
                }

                if (Convert.ToInt64(txtTransferAmount.Text) == 0)
                {
                    Utility.FunShowAlertMsg(this, "Transfer amount should be greater than zero");
                    return;
                }

            }

            //Transfer Amount Validation End by Praba on 29-12-2020



            ObjMasterDataTable = new ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable();

            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Entity_MasterRow objMasterrow;
            objMasterrow = ObjMasterDataTable.NewS3G_ORG_Entity_MasterRow();
            objMasterrow.Company_Id = intCompanyId;
            objMasterrow.Created_By = intUserId;
            objMasterrow.Entity_Code = txtGroupCode.Text;
            objMasterrow.Entity_Name = txtGroupName.Text;
            if (strCustomerGroupId != "" && strCustomerGroupId != "0")
            {
                objMasterrow.Entity_Master_ID = Convert.ToInt32(strCustomerGroupId);
            }
            else
            {
                objMasterrow.Entity_Master_ID = 0;
            }
            if (chkHActive.Checked)
            {
                objMasterrow.Is_Active = 1;
            }
            else
                objMasterrow.Is_Active = 0;

            objMasterrow.Group_Limit = Convert.ToInt64(txtGroupLimit.Text);
            objMasterrow.Effective_To = Utility.StringToDate(txtGroupLimiExpDate.Text);
            objMasterrow.XMLAdditionalParamDet = Utility.FunPubFormXml(((DataTable)ViewState["GROUP_CUST"]), true);

            //Start By Praba on 29-12-2020
            if (!string.IsNullOrEmpty(ddlFromCustomer.SelectedValue))
                objMasterrow.From_Customer_ID = Convert.ToInt32(ddlFromCustomer.SelectedValue);
            else
                objMasterrow.From_Customer_ID = 0;

            if (!string.IsNullOrEmpty(ddlToCustomer.SelectedValue))
                objMasterrow.To_Customer_ID = Convert.ToInt32(ddlToCustomer.SelectedValue);
            else
                objMasterrow.To_Customer_ID = 0;

            if (!string.IsNullOrEmpty(txtTransferAmount.Text))
                objMasterrow.Transfer_Amount = Convert.ToDecimal(txtTransferAmount.Text);
            else
                objMasterrow.Transfer_Amount = 0;
            //End By Praba on 29-12-2020

            ObjMasterDataTable.AddS3G_ORG_Entity_MasterRow(objMasterrow);

            S3GBusEntity.SerializationMode SerMode = S3GBusEntity.SerializationMode.Binary;
            byte[] ObjMasterDT = ClsPubSerialize.Serialize(ObjMasterDataTable, SerMode);
            ObjOrgMasterMgtServicesClient = new ORGSERVICE.OrgMasterMgtServicesClient();

            intErrCode = ObjOrgMasterMgtServicesClient.FunPubCreateCustomerGroupInt(SerMode, ObjMasterDT);
            if (intErrCode == 0)
            {
                if (strCustomerGroupId == "" || strCustomerGroupId == "0")
                {
                    strAlert = Convert.ToString("Customer group created successfully");
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    strRedirectPageView = "";
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Customer group updated successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Customer group code already exists");
                return;
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "Customer group name already exists");
                return;
            }
            else if (intErrCode == 5)
            {
                Utility.FunShowAlertMsg(this, "Customer group limit exceeds");
                return;
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.Save_Error);
                return;
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
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvCollectionDtls_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grvClosureDtls_RowCommand(object sender, GridViewCommandEventArgs e)
    {

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
            txtGroupCode.Clear();
            txtGroupLimiExpDate.Clear();
            txtGroupLimit.Clear();
            txtGroupName.Clear();
            chkHActive.Checked = true;
            ViewState["GROUP_CUST"] = null;
            grvCustSubLimit.ClearGrid();
            ucCustomerCodeLov.Clear();
            ViewState["Customer_Det"] = null;

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

            }

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

    protected void txtEffectiveToDate_TextChanged(object sender, EventArgs e)
    {


    }
    protected void btnLoadCustomerMapFWC_Click(object sender, EventArgs e)
    {
        try
        {
            string strCustomerAddress = string.Empty;
            StringBuilder strFormAddress = new StringBuilder();
            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            //TextBox txtCusomerCodeMapFWChidden = (TextBox)ucCustomerLovCustomerMapFWC.FindControl("txtCusomerCodeMapFWChidden");
            if (hdnCID != null && hdnCID.Value != "")
            {
                Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
                //btnGetLOV.Focus();
                TextBox txtCustomerName = (TextBox)ucCustomerCodeLov.FindControl("txtItemName");
                TextBox TxtName = (TextBox)ucCustomerCodeLov.FindControl("TxtName");
                TxtName.ToolTip = txtCusomerCodeMapFWChidden.Text = TxtName.Text = txtCustomerName.Text;
                DataSet ds = new DataSet();
                Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
                objProcedureParameters.Add("@Option", "1");
                objProcedureParameters.Add("@COMPANY_ID", intCompanyId.ToString());
                objProcedureParameters.Add("@CustomerId", hdnCID.Value);
                ds = Utility.GetDataset("S3G_OR_GET_CUSADDRESS", objProcedureParameters);

                if (ds.Tables[6].Rows.Count > 0)
                {
                    if (ds.Tables[6].Rows[0]["NEGATIVELIST_CUSTOMER"].ToString() == "1")
                    {
                        ViewState["NEGATIVELIST_CUSTOMER"] = 1;
                        //TextBox txtName2 = (TextBox)ucCustomerLovCustomerMapFWC.FindControl("txtName");
                        //HiddenField hdnCID2 = (HiddenField)ucCustomerLovCustomerMapFWC.FindControl("hdnID");
                        //txtName2.Text = string.Empty;
                        //hdnCID2.Value = "0";
                        //ucCustomerCodeLov.Clear();
                        Utility.FunShowAlertMsg(this, "Customer Black Listed not allowed to Create the Application");
                        //return;

                    }
                }

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i <= ds.Tables[0].Columns.Count - 1; i++)
                //    {
                //        strFormAddress.Append(Environment.NewLine);
                //        strFormAddress.Append(ds.Tables[0].Columns[i].ColumnName + " : " + ds.Tables[0].Rows[0][i].ToString());

                //    }
                //    if (ds.Tables[1].Rows.Count > 0)
                //    {
                //        for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                //        {
                //            strFormAddress.Replace(ds.Tables[1].Rows[i]["COLUMN_NAME"].ToString().ToUpper(), ds.Tables[1].Rows[i]["DISPLAY_TEXT"].ToString());
                //        }
                //    }
                //    funPriSetCustomerAddress(ds.Tables[0], strFormAddress, ucCustomerLovFWC);
                //}
                //if (ds.Tables[2].Rows.Count > 0)
                //{
                //    txtCreditLimit.Text = ds.Tables[2].Rows[0]["MAX_LEND_AMOUNT"].ToString();
                //    txtConstitution.Text = ds.Tables[2].Rows[0]["Constitution"].ToString();
                //    //FunPriLoadConsitutionBasedCustomer(Convert.ToInt32(hdnCID.Value));
                //}
                //txtCustomerName.Focus();
            }
            //FunPriAssignAssetLink();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ucCustomerLovCustomerMapFWC_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            //UserControls_CommonSearch ucCustomerLovCustomerMapFWC = grvCustSubLimit.FooterRow.FindControl("ucCustomerLovCustomerMapFWC") as UserControls_CommonSearch;
            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            TextBox txtCustomerName = (TextBox)ucCustomerCodeLov.FindControl("txtItemName");
            hdnCID.Value = ucCustomerCodeLov.SelectedValue;
            txtCusomerCodeMapFWChidden.Text = ucCustomerCodeLov.SelectedValue;
            txtCustomerName.Text = ucCustomerCodeLov.SelectedText;
            btnLoadCustomerMapFWC_Click(null, null);
            if (hdnCID.Value != "0" && hdnCID.Value != "")
            {
                Dictionary<string, string> Procparam;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@CustId", hdnCID.Value);
                Procparam.Add("@Companyid", intCompanyId.ToString());
                Procparam.Add("@Option", "3");
                DataSet ds = new DataSet();
                ds = Utility.GetDataset("S3G_OR_GET_CustGroupDetails", Procparam);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        hdnMaxlimit.Value = ds.Tables[0].Rows[0][0].ToString();
                        hdnLimitExpDate.Value = ds.Tables[0].Rows[0][1].ToString();
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@COMPANY_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@LOV", "CUS_ACCA");
        if (System.Web.HttpContext.Current.Session["User_Id"] != null && Convert.ToString(System.Web.HttpContext.Current.Session["User_Id"]) != string.Empty)
        {
            Procparam.Add("@User_Id", Convert.ToString(System.Web.HttpContext.Current.Session["User_Id"]));
        }
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("SA_GET_CUSTOMER_AGT", Procparam));
        return suggetions.ToArray();
    }

    protected void lnkAddCustomerMapFWC_Click(object sender, EventArgs e)
    {
        try
        {
            //TextBox txtCustomerName = (TextBox)ucCustomerLovCustomerMapFWC.FindControl("txtName");
            //HiddenField hdnCustomerId = (HiddenField)ucCustomerLovCustomerMapFWC.FindControl("hdnID");

            HiddenField hdnCIDClient = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCIDClient.Value == "0")
            {
                Utility.FunShowAlertMsg(this, "Select the Customer");
                return;
            }
            if (ucCustomerCodeLov.SelectedText.Split('-').Length == 1)
            {
                Utility.FunShowAlertMsg(this, "InValid Customer");
                return;
            }

            //if (txtGroupLimit.Text.Trim() != "" && ViewState["GROUP_CUST"]!=null)
            //{
            //    if (Convert.ToInt32(txtGroupLimit.Text) <= grvCustSubLimit.Rows.Count)
            //    {
            //        Utility.FunShowAlertMsg(this, "Group limit exceeds");
            //        return;
            //    }
            //}

            TextBox TxtName = (TextBox)ucCustomerCodeLov.FindControl("TxtName");
            int iSno = 0;
            DataTable dt = (DataTable)ViewState["GROUP_CUST"];
            if (dt == null)
            {
                FunPriGetSubLimitDataTable();
                dt = (DataTable)ViewState["GROUP_CUST"];
            }
            double sum = 0;

            if (dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("ENTITY_ID ='" + hdnCIDClient.Value + "'");
                if (dr.Length > 0)
                {
                    Utility.FunShowAlertMsg(this, "Selected customer already exists");
                    return;
                }

                foreach (DataRow drow in dt.Rows)
                {
                    sum += Convert.ToDouble(drow["MAX_LIMIT"]);
                }
                sum = sum + Convert.ToDouble(hdnMaxlimit.Value);


                decimal decTotalSubLimit = 0;
                decimal decCurrentSubLimit = 0;

                //if (dt.Compute("sum(Limit)", "Limit>0").ToString() != string.Empty)
                //    decTotalSubLimit = Convert.ToDecimal(dt.Compute("sum(Limit)", "Limit>0").ToString());
                //foreach (DataRow dr5 in dt.Rows)
                //{
                //    if (dr5["Limit"].ToString() != string.Empty)
                //    {
                //        decCurrentSubLimit = decCurrentSubLimit + Convert.ToDecimal(dr5["Limit"]);
                //    }
                //}


                //decCurrentSubLimit = Convert.ToDecimal(txtLimitF.Text);
                //if ((decTotalSubLimit + decCurrentSubLimit) > Convert.ToDecimal(txtCreditLimitFWC.Text))
                //{
                //    //Utility.FunShowAlertMsg(this, "Customers sublimit should not exceed the Client Credit Limit ");
                //    Utility.FunShowAlertMsg(this, Convert.ToString(Resources.LocalizationResources.Application_Process_Error_Messege11));
                //    return;
                //}


                //DataRow[] dr = dt.Select("Entity_Id='" + ucCustomerLovFWC.SelectedValue + "'");
                //if (dr.Length > 0)
                //{
                //    //Utility.FunShowAlertMsg(this, "Client not Allowed here");
                //    Utility.FunShowAlertMsg(this, "Client not allowed here");
                //    return;
                //}

                //if (hdnCIDClient.Value == hdnCID2ClientCustomer.Value)
                //{
                //    Utility.FunShowAlertMsg(this, "Client not allowed here");
                //    return;
                //}
            }

            //if (hdnCIDClient.Value == hdnCID2ClientCustomer.Value)
            //{
            //    Utility.FunShowAlertMsg(this, "Client not allowed here");
            //    return;
            //}
            else
            {
                if (string.IsNullOrEmpty(hdnMaxlimit.Value))
                {
                    //Get Customer Max Limt, Start by Praba On 02-01-2021
                    if (hdnCIDClient.Value != "0" && hdnCIDClient.Value != "")
                    {

                        Procparam = new Dictionary<string, string>();
                        Procparam.Add("@CustId", hdnCIDClient.Value);
                        Procparam.Add("@Companyid", Convert.ToString(intCompanyId));
                        Procparam.Add("@Option", "3");
                        DataSet dsResult = new DataSet();
                        dsResult = Utility.GetDataset("S3G_OR_GET_CustGroupDetails", Procparam);
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                hdnMaxlimit.Value = dsResult.Tables[0].Rows[0][0].ToString();
                                //hdnLimitExpDate.Value = ds.Tables[0].Rows[0][1].ToString();
                            }
                        }

                    }
                    //Get Customer Max Limt End by Praba On 02-01-2021
                }
                else
                    sum = Convert.ToDouble(hdnMaxlimit.Value);
            }

            if (txtGroupLimit.Text.Trim() != "")
            {
                if (sum > Convert.ToDouble(txtGroupLimit.Text))
                {
                    Utility.FunShowAlertMsg(this, "Group limit exceeded");
                    return;
                }
            }

            if (txtGroupLimiExpDate.Text.Trim() != "" && hdnLimitExpDate.Value != "")
            {
                if (Utility.StringToDate(hdnLimitExpDate.Value) > Utility.StringToDate(txtGroupLimiExpDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "Group Limit Expiry date exceeded");
                    return;
                }
            }


            string strNo = dt.Compute("max(SERIAL_NUMBER)", "1=1").ToString();
            if (strNo == null)
            {
                iSno = 1;
            }
            else
            {
                iSno = iSno + 1;
            }
            DataSet ds = new DataSet();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CompanyID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@CustId", hdnCIDClient.Value);
            Procparam.Add("@Option", "2");
            if (strCustomerGroupId != "" && strCustomerGroupId != "0")
            {
                Procparam.Add("@Custgroupid", strCustomerGroupId);
            }

            ds = Utility.GetDataset("S3G_OR_GET_CustGroupDetails", Procparam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        Utility.FunShowAlertMsg(this, "Group already assigned for the selected customer.");
                        return;
                    }
                }
            }

            FunPriInsertCustSubLimit(iSno.ToString(), hdnCIDClient.Value, TxtName.Text, hdnMaxlimit.Value, hdnLimitExpDate.Value);
            hdnMaxlimit.Value = null;
            hdnLimitExpDate.Value = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void FunPriInsertCustSubLimit(string strSno, string strEntityId, string strEntityName, string strEntityLimit, string strEntityExpDate)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["GROUP_CUST"];

            if (dt != null)
            {
                DataRow[] dr = dt.Select("SERIAL_NUMBER=0 and SERIAL_NUMBER is not null");
                foreach (DataRow dr2 in dr)
                {
                    dr2.Delete();
                }

                if (ViewState["CUST_SUBLIMIT"] != null)
                {
                    DataRow[] drDupCheck = dt.Select("ENTITY_ID='" + strEntityId + "'");
                    if (drDupCheck.Count() > 0)
                    {
                        Utility.FunShowAlertMsg(this, Convert.ToString(Resources.LocalizationResources.Application_Process_Error_Messege14));
                        return;
                    }
                }
                DataRow drEmptyRow;
                dtCustGroup = FunPriGetSubLimitDataTable();
                drEmptyRow = dtCustGroup.NewRow();
                drEmptyRow["SERIAL_NUMBER"] = strSno;
                drEmptyRow["ENTITY_ID"] = strEntityId;
                drEmptyRow["ENTITY_NAME"] = strEntityName;
                drEmptyRow["MAX_LIMIT"] = strEntityLimit;
                if (strEntityExpDate != "")
                {
                    drEmptyRow["LIMIT_EXP_DATE"] = Convert.ToDateTime(strEntityExpDate).ToString(strDateFormat);
                }
                else
                    drEmptyRow["LIMIT_EXP_DATE"] = "";

                dtCustGroup.Rows.Add(drEmptyRow);
                ViewState["GROUP_CUST"] = dtCustGroup;
                FunPriFillGrid();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    private void FunPriFillGrid()
    {
        try
        {
            DataTable dtCustSubLimit = (DataTable)ViewState["GROUP_CUST"];
            grvCustSubLimit.DataSource = dtCustSubLimit;
            grvCustSubLimit.EmptyDataText = "No Records Found..";
            grvCustSubLimit.DataBind();
            ucCustomerCodeLov.Clear();
            txtCusomerCodeMapFWChidden.Text = string.Empty;
            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            hdnCID.Value = "0";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private DataTable FunPriGetSubLimitDataTable()
    {
        try
        {
            if (ViewState["GROUP_CUST"] == null)
            {
                dtCustGroup = new DataTable();
                dtCustGroup.Columns.Add("SERIAL_NUMBER");
                dtCustGroup.Columns.Add("ENTITY_ID");
                dtCustGroup.Columns.Add("ENTITY_NAME");
                dtCustGroup.Columns.Add("MAX_LIMIT");
                dtCustGroup.Columns.Add("LIMIT_EXP_DATE");
                ViewState["GROUP_CUST"] = dtCustGroup;
            }
            dtCustGroup = (DataTable)ViewState["GROUP_CUST"];

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        return dtCustGroup;

    }
    protected void grvCustSubLimit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                //UserControls_CommonSearch ucCustomerLovCustomerMapFWC = grvCustSubLimit.FooterRow.FindControl("ucCustomerLovCustomerMapFWC") as UserControls_CommonSearch;
                //DropDownList ddlGuarantortype_GuarantorTab1 = grvCustSubLimit.FooterRow.FindControl("ddlGuarantortype_GuarantorTab") as DropDownList;
                //ucCustomerLovCustomerMapFWC.strControlID = ucCustomerLovCustomerMapFWC.ClientID;
                //TextBox txt1 = (TextBox)ucCustomerLovCustomerMapFWC.FindControl("txtName");
                //txt1.Attributes.Add("onfocus", "fnLoadCustomerSubLimit()");

                //UserControls_CommonSearch ucCustomerLovCustomerMapFWC =e.Row.FindControl("ucCustomerLovCustomerMapFWC") as UserControls_CommonSearch;
                //ucCustomerLovCustomerMapFWC.strControlID = ucCustomerLovCustomerMapFWC.ClientID.ToString();
                //TextBox txtItemName = ((TextBox)ucCustomerLovCustomerMapFWC.FindControl("txtItemName"));
                //txtItemName.Attributes.Add("onfocus", "fnLoadCustomerSubLimit()");
                //txtItemName.Width = 0;
                //txtItemName.TabIndex = -1;
                //txtItemName.BorderStyle = BorderStyle.None;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton lnkEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                //LinkButton lnkRemove = e.Row.FindControl("lnkRemove") as LinkButton;
                //LinkButton lnkAdd = e.Row.FindControl("lnkAdd") as LinkButton;
                //TextBox txtCutOffDate = e.Row.FindControl("txtCutOffDate") as TextBox;
                //Label lbldelst = e.Row.FindControl("lbldelst") as Label;
                //Label lblEntityId = e.Row.FindControl("lblEntityId") as Label;



                //if (strMode == "Q")
                //{
                //    lnkEdit.Enabled = false;
                //    lnkRemove.Enabled = false;
                //    lnkAdd.Enabled = false;
                //}
                //else
                //{
                //    Dictionary<string, string> ProParm = new Dictionary<string, string>();
                //    ProParm.Add("@OPTION", "2001");
                //    //ProParm.Add("@Param1", intCustomerId.ToString());
                //    ProParm.Add("@Param2", lblEntityId.Text);
                //    DataTable dt = Utility.GetDefaultData("S3G_OR_GET_CUSTLOOKUP", ProParm);
                //    if (dt.Rows.Count > 0)
                //    {
                //        lnkRemove.Enabled = false;
                //        lnkRemove.OnClientClick = null;
                //    }
                //}
                //if (txtCutOffDate.Text != string.Empty)
                //{
                //    txtCutOffDate.Text = DateTime.Parse(txtCutOffDate.Text.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                //}

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void grvCustSubLimit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {

                //UserControls_CommonSearch ucCustomerLovCustomerMapFWC = grvCustSubLimit.FooterRow.FindControl("ucCustomerLovCustomerMapFWC") as UserControls_CommonSearch;
                //TextBox txtCustomerName = (TextBox)ucCustomerLovCustomerMapFWC.FindControl("txtName");
                //HiddenField hdnCustomerId = (HiddenField)ucCustomerLovCustomerMapFWC.FindControl("hdnID");



                //Label lblSerialNo = grvCustSubLimit.FooterRow.FindControl("lblSerialNo") as Label;
                //TextBox txtLimitF = grvCustSubLimit.FooterRow.FindControl("txtLimitF") as TextBox;
                //TextBox txtCutOffDateF = grvCustSubLimit.FooterRow.FindControl("txtCutOffDateF") as TextBox;
                //FunPriInsertCustSubLimit(lblSerialNo.Text, hdnCustomerId.Value, txtCustomerName.Text, txtLimitF.Text.Trim(), txtCutOffDateF.Text.Trim());
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void grvCustSubLimit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtCustGroup = FunPriGetSubLimitDataTable();
            dtCustGroup.Rows.RemoveAt(e.RowIndex);
            ViewState["GROUP_CUST"] = dtCustGroup;
            if (dtCustGroup.Rows.Count == 0)
            {
                ViewState["GROUP_CUST"] = null;
                FunPriGetSubLimitDataTable();
            }
            FunPriFillGrid();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }



    protected void ddlFromCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlToCustomer.Items.Clear();
            ddlToCustomer.FillDataTable((DataTable)ViewState["Customer_Det"], "CUSTOMER_ID", "CUSTOMER_NAME");

            if (Convert.ToInt32(ddlFromCustomer.SelectedValue) > 0)
                ddlToCustomer.Items.Remove(ddlToCustomer.Items.FindByValue(ddlFromCustomer.SelectedValue));

            //BIND LIMIT VALUES
            if (Convert.ToInt32(ddlFromCustomer.SelectedValue) > 0)
            {
                DataSet dsLimit = new DataSet();
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@CompanyID", ObjUserInfo.ProCompanyIdRW.ToString());
                Procparam.Add("@CustGroupID", strCustomerGroupId);
                Procparam.Add("@Customer_ID", ddlFromCustomer.SelectedValue);
                Procparam.Add("@Option", "2");
                dsLimit = Utility.GetDataset("S3G_OR_GET_CUS_GRP_LIMIT_TRANS", Procparam);
                if (dsLimit.Tables[0].Rows.Count > 0)
                {
                    txtUtilizedLimit.Text = dsLimit.Tables[0].Rows[0]["UTILIZED_AMOUNT"].ToString();
                    txtAvailableLimit.Text = dsLimit.Tables[0].Rows[0]["AVAILABLE_LIMIT"].ToString();
                }
            }
            else
            {
                txtUtilizedLimit.Text = txtAvailableLimit.Text = txtTransferAmount.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlToCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}