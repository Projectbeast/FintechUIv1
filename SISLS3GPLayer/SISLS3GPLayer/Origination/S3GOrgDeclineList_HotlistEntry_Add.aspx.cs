using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI;
using ORG = S3GBusEntity.Origination;
using ORGSERVICE = OrgMasterMgtServicesReference;
using System.Data;
using S3GBusEntity;
using System.Web.Security;
using System.Text;
using Resources;

public partial class Origination_S3GOrgDeclineList_HotlistEntry_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyId = 0;
    int intUserId = 0;
    int intProgram_Id = 537;
    int intNegativeListID = 0;
    int intErrorCode = 0;
    string strEntityCode;
    ORG.OrgMasterMgtServices.S3G_ORG_NEGATIVELISTDataTable ObjNegativeListDataTable;
    ORGSERVICE.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;

    public string strCustomerID = string.Empty;
    static string strPageName = "Decline List Hotlist Entry";
    public string strDateFormat;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgDeclineList_HotlistEntry_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgDeclineList_HotlistEntry_Add.aspx';";
    string strRedirectPage = "~/Origination/S3GOrgDeclineList_HotlistEntry_View.aspx";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    public static Origination_S3GOrgDeclineList_HotlistEntry_Add obj_Page;
    Dictionary<string, string> Procparam = null;

    //Code end
    bool IsLOBCalled = false;
    DataTable dtDealerMatDet = new DataTable();
    string strProgramName = string.Empty;

    #endregion

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

            obj_Page = this;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;//Convert.ToInt32(Request.QueryString["qsCmpnyId"]);
            intUserId = ObjUserInfo.ProUserIdRW;

            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            ceDeclineDate.Format = strDateFormat;

            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyId.ToString();

            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];

            if (Request.QueryString["qsNegativeListId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsNegativeListId"));
                if (fromTicket != null)
                {
                    intNegativeListID = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }

            if (ViewState["PageMode"] == null)
            {
                ViewState["PageMode"] = PageMode.ToString();
            }

            //Customer  Code Popup Start
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txtCustItemNumber = ((TextBox)ucCustomerCodeLov.FindControl("txtItemName"));
            txtCustItemNumber.Style["display"] = "block";
            txtCustItemNumber.Attributes.Add("onfocus", "fnLoadCust()");
            txtCustItemNumber.Attributes.Add("readonly", "false");
            txtCustItemNumber.Width = 0;
            txtCustItemNumber.TabIndex = -1;
            txtCustItemNumber.BorderStyle = BorderStyle.None;
            //Customer Code Popup End

            //TextBox txtNationality1 = ((TextBox)ddlNationality.FindControl("txtItemName"));
            //txtNationality1.Attributes.Add("onchange", "fnTrashSuggest('" + ddlNationality.ClientID + "');");


            //TextBox ucCustomerCodeLov2 = ((TextBox)ucCustomerCodeLov.FindControl("TxtName"));
            //ucCustomerCodeLov2.Attributes.Add("onchange", "fnTrashCustomerSuggest('" + ucCustomerCodeLov.ClientID + "');");


            if (!IsPostBack)
            {
                FunPriLoadPage();
                funPriSetProgramName();

                txtDeclineDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);

                if (intNegativeListID > 0)
                {
                    FunPubGetNegativeListDetails();

                    if (strMode == "M")
                    {

                        FunPriDeclineListControlStatus(1);
                        //chkIs_Active.Enabled = true;
                    }
                    if (strMode == "Q")
                    {
                        FunPriDeclineListControlStatus(-1);
                        chkIs_Active.Enabled = false;
                    }
                }
                else
                {
                    FunPriDeclineListControlStatus(0);
                    chkIs_Active.Enabled = false;
                }

                TextBox txtname = ((TextBox)ddlNationality.FindControl("txtItemName"));
                txtname.Focus();
            }



            txtDeclineDate.Attributes.Add("onChange", "fnDoDate(this,'" + txtDeclineDate.ClientID + "','" + strDateFormat + "',false,  true);");

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName); ;
        }
    }

    #region Button Events

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            strCustomerID = string.Empty;

            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

            txtCustomerNameDummy.Text = ucCustomerCodeLov.SelectedValue;

            if (hdnCID != null && hdnCID.Value != "")
            {
                System.Web.HttpContext.Current.Session.Remove("Cust_id");
                strCustomerID = hdnCID.Value;
                System.Web.HttpContext.Current.Session["Cust_id"] = hdnCID.Value;

            }

            FunBindProposalNumber();
            txtCustomerCode.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {

            if (ddlNationality.SelectedValue == "0" || ddlNationality.SelectedValue == "" || ddlNationality.SelectedText == "")
            {
                Utility.FunShowAlertMsg(this, "Select the Nationality");
                //ddlNationality.Focus();
                TextBox txtname = ((TextBox)ddlNationality.FindControl("txtItemName"));
                txtname.Focus();
                return;
            }

            if (rblProspectCust.SelectedValue == "2")
            {
                if (ucCustomerCodeLov.SelectedValue == "0" || ucCustomerCodeLov.SelectedValue == "" || ucCustomerCodeLov.SelectedText == "")
                {
                    Utility.FunShowAlertMsg(this, "Select the Customer Code");
                    //ddlNationality.Focus();
                    TextBox txtname = ((TextBox)ucCustomerCodeLov.FindControl("txtItemName"));
                    txtname.Focus();
                    return;
                }
            }


            ObjNegativeListDataTable = new ORG.OrgMasterMgtServices.S3G_ORG_NEGATIVELISTDataTable();
            ORG.OrgMasterMgtServices.S3G_ORG_NEGATIVELISTRow ObjNegativelistRow;
            ObjNegativelistRow = ObjNegativeListDataTable.NewS3G_ORG_NEGATIVELISTRow();
            ObjNegativelistRow.Company_ID = intCompanyId;
            ObjNegativelistRow.NegativeList_ID = intNegativeListID;
            ObjNegativelistRow.Nationality = Convert.ToInt32(ddlNationality.SelectedValue);
            ObjNegativelistRow.Constitution_ID = Convert.ToInt32(ddlConstitutionName.SelectedValue);
            ObjNegativelistRow.CR_Number = txtIdentityValue.Text;
            ObjNegativelistRow.NegativeList_Type = Convert.ToInt32(ddlNegativeList_Type.SelectedValue);
            ObjNegativelistRow.Customer_ID = ucCustomerCodeLov.SelectedValue == "" ? 0 : Convert.ToInt32(ucCustomerCodeLov.SelectedValue);
            ObjNegativelistRow.Prospect_Name = txtProspectName.Text;
            ObjNegativelistRow.Prospect_Address = txtProspectAddress.Text;
            ObjNegativelistRow.Prospect_Mobile = txtProspectMobile.Text;
            ObjNegativelistRow.NegativeList_Reason = Convert.ToInt32(ddlNegativeList_Reason.SelectedValue);
            ObjNegativelistRow.NegativeList_Remarks = txtNegativelistRemarks.Text;
            ObjNegativelistRow.Codified_Remarks = txtCodRemarks.Text;

            if (ddlProposalNo.SelectedValue == "--Select--" || ddlProposalNo.SelectedValue == "")
                ObjNegativelistRow.Application_ID = 0;
            else
                ObjNegativelistRow.Application_ID = Convert.ToInt32(ddlProposalNo.SelectedValue); ;

            ObjNegativelistRow.Proposal_No = "";
            if (rblProspectCust.SelectedValue == "2") //for customer
            {
                if (!string.IsNullOrEmpty(ddlProposalNo.SelectedValue) && ddlProposalNo.SelectedValue != "0")
                    ObjNegativelistRow.Proposal_No = ddlProposalNo.SelectedItem.Text;
                else
                    ObjNegativelistRow.Proposal_No = "";
            }

            ObjNegativelistRow.Decline_Date = Utility.StringToDate(txtDeclineDate.Text);
            ObjNegativelistRow.Is_Active = Convert.ToInt32(chkIs_Active.Checked);
            ObjNegativelistRow.Created_By = intUserId;

            ObjNegativeListDataTable.AddS3G_ORG_NEGATIVELISTRow(ObjNegativelistRow);

            if (ObjNegativeListDataTable.Rows.Count > 0)
            {
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_ORG_NegativeList_DataTable = ClsPubSerialize.Serialize(ObjNegativeListDataTable, SerMode);
                intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateModifyNegativeList(SerMode, byteobjS3G_ORG_NegativeList_DataTable);

                switch (intErrorCode)
                {
                    case 0:

                        btnSave.Enabled_False();

                        if (intNegativeListID > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Decline list entry details are updated successfully");
                            //Utility.FunShowAlertMsg(this.Page, "Decline list entry " + ValidationMsgs.S3G_ValMsg_Update, strRedirectPage);

                        }
                        else
                        {
                            strAlert = "Decline list entry " + Resources.ValidationMsgs.S3G_ValMsg_Save;
                            strAlert += @"\n\nWould you like to add one more Entry?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                        }
                        break;

                    case -4://Added by Tamilselvan.S on 23/11/2011 for Adding validation for Duplication check
                        strAlert = strAlert.Replace("__ALERT__", " Decline list Already Exists");
                        strRedirectPageView = "";
                        break;
                    default:
                        if (intNegativeListID > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Decline list entry details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Decline list entry details");
                        }
                        strRedirectPageView = "";
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {
            // if (ObjOrgMasterMgtServicesClient != null)
            ObjOrgMasterMgtServicesClient.Close();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearControls();
            txtDeclineDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
            TextBox txtname = ((TextBox)ddlNationality.FindControl("txtItemName"));
            txtname.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3GOrgDeclineList_HotlistEntry_View.aspx");
    }
    #endregion

    private void FunPriLoadPage()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Lookup_Type", "NEGATIVELIST_TYPE");
            Procparam.Add("@Table_Name", "S3G_ORG_Lookup");
            ddlNegativeList_Type.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, new string[] { "LOOKUP_CODE", "DESCRIPTION" });

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Lookup_Type", "NEGATIVELIST_REASON");
            Procparam.Add("@Table_Name", "S3G_ORG_Lookup");
            ddlNegativeList_Reason.BindDataTable("S3G_GET_COMMON_LOOKUP_VAL", Procparam, new string[] { "LOOKUP_CODE", "DESCRIPTION" });

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", Convert.ToString(intCompanyId));
            ddlConstitutionName.BindDataTable("S3G_OR_GET_CONSTITUTION_NAME", Procparam, true, "--Select--", new string[] { "Constitution_ID", "Constitution" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void FunBindProposalNumber()
    {
        try
        {
            //Added By Praba Start
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "790");
            Procparam.Add("@Param1", Convert.ToString(intCompanyId));
            Procparam.Add("@Param2", strCustomerID);

            ddlProposalNo.BindDataTable("S3G_ORG_GetCustomerLookUp", Procparam, true, "--Select--", new string[] { "Application_Process_Id", "Application_Number" });
            //Added By Praba End

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    private void FunPriDeclineListControlStatus(int intModeID)
    {
        try
        {

            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = strProgramName + " - Create"; //FunPubGetPageTitles(enumPageTitle.Create);

                    btnClear.Enabled_True();
                    chkIs_Active.Checked = true;
                    chkIs_Active.Enabled = false;
                    divIsActive.Visible = false;

                    if (!bCreate)
                    {
                        btnSave.Enabled_False();
                    }
                    break;

                case 1: //Modify


                    lblHeading.Text = strProgramName + " - Modify"; //FunPubGetPageTitles(enumPageTitle.Modify);
                    btnClear.Enabled_False();

                    if (!bModify)
                    {
                        btnSave.Enabled_False();
                    }

                    ddlNationality.Enabled = false;
                    ddlConstitutionName.Enabled = false;
                    ucCustomerCodeLov.Enabled = false;
                    rblProspectCust.Enabled = false;
                    ddlProposalNo.Enabled = false;
                    chkIs_Active.Enabled = true;
                    divIsActive.Visible = true;
                    if (rblProspectCust.SelectedValue == "1")
                        txtIdentityValue.ReadOnly = false;
                    else
                        txtIdentityValue.ReadOnly = true;

                    break;
                case -1://Query

                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }
                    lblHeading.Text = strProgramName + " - View";  //FunPubGetPageTitles(enumPageTitle.View);

                    btnClear.Enabled_False();
                    btnSave.Enabled_False();

                    if (bClearList)
                    {
                    }

                    ddlNationality.Enabled = false;
                    ddlConstitutionName.Enabled = false;
                    ddlNegativeList_Type.Enabled = false;
                    ddlNegativeList_Reason.Enabled = false;
                    ucCustomerCodeLov.Enabled = false;
                    rblProspectCust.Enabled = false;
                    ddlProposalNo.Enabled = false;
                    txtProspectName.Enabled = false;
                    txtProspectMobile.Enabled = false;
                    txtProspectAddress.Enabled = false;
                    //txtIdentityValue.Enabled = false;
                    txtIdentityValue.ReadOnly = true;
                    txtNegativelistRemarks.Enabled = false;
                    txtCodRemarks.Enabled = false;
                    txtDeclineDate.Enabled = false;
                    chkIs_Active.Enabled = false;
                    divIsActive.Visible = true;
                    break;

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    protected void rblProspectCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (rblProspectCust.SelectedValue == "1")  //Prospect
            {
                divProspect.Visible = true;
                divProspectMob.Visible = true;
                divProspecAddress.Visible = true;
                divCustomerName.Visible = false;
                ddlProposalNo.Items.Clear();

                ddlProposalNo.Items.Insert(0, "--Select--");
                rfvtxtProspectName.Enabled = true;
                ucCustomerCodeLov.IsMandatory = false;
                ucCustomerCodeLov.ErrorMessage = "";
                rfvtxtCustomerNameDummy.Enabled = false;


                //txtIdentityValue.Enabled = true;
                txtIdentityValue.ReadOnly = false;
            }
            if (rblProspectCust.SelectedValue == "2") //Customer
            {
                divCustomerName.Visible = true;
                divProspect.Visible = false;
                divProspectMob.Visible = false;
                divProspecAddress.Visible = false;

                ddlProposalNo.Items.Clear();
                ddlProposalNo.Items.Insert(0, "--Select--");

                TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
                HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                txtUserName.Text = string.Empty;
                txtCustomerNameDummy.Clear();
                hdnCID.Value = string.Empty;

                rfvtxtProspectName.Enabled = false;
                ucCustomerCodeLov.IsMandatory = true;
                ucCustomerCodeLov.ErrorMessage = "Select Customer Name";
                rfvtxtCustomerNameDummy.Enabled = true;
                //txtIdentityValue.Enabled = false;
                txtIdentityValue.ReadOnly = true;
            }
            rblProspectCust.Focus();
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetNationalityList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_NATIONALITY_LIST", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        string strNationality = "";
        string strConstitutionID = "";

        if (obj_Page.ddlNationality.SelectedValue != "0" && obj_Page.ddlNationality.SelectedValue != "")
            strNationality = obj_Page.ddlNationality.SelectedValue;

        if (obj_Page.ddlConstitutionName.SelectedIndex > 0)
            strConstitutionID = obj_Page.ddlConstitutionName.SelectedValue;

        Procparam.Clear();
        Procparam.Add("@COMPANY_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@Nationality", strNationality);
        Procparam.Add("@Constitution_ID", strConstitutionID);
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("SA_GET_CUSTOMER_AGT", Procparam));

        return suggetions.ToArray();

    }


    protected void ucCustomerCodeLov_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            Button btnLoadCust = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");

            //Load Address Start
            string strCustomerAddress = string.Empty;
            StringBuilder strFormAddress = new StringBuilder();

            txtCustomerNameDummy.Text = ucCustomerCodeLov.SelectedValue;

            if (ucCustomerCodeLov.SelectedValue != "")
            {
                //CustomerDetails1.ClearCustomerDetails();
                Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
                btnGetLOV.Focus();
                UserControl CustomerDetails1 = (UserControl)ucCustomerCodeLov.FindControl("S3GCustomerAddress1");
                TextBox txtCustomerName = (TextBox)CustomerDetails1.FindControl("txtCustomerName");
                txtCustomerCode.Text = txtCustomerName.Text;
                DataSet ds = new DataSet();
                Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
                objProcedureParameters.Add("@Option", "1");
                objProcedureParameters.Add("@COMPANY_ID", intCompanyId.ToString());
                // objProcedureParameters.Add("@CustomerId", hdnCID.Value);
                objProcedureParameters.Add("@CustomerId", ucCustomerCodeLov.SelectedValue);
                ds = Utility.GetDataset("S3G_OR_GET_CUSADDRESS", objProcedureParameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds.Tables[0].Columns.Count - 1; i++)
                    {
                        strFormAddress.Append(Environment.NewLine);
                        strFormAddress.Append(ds.Tables[0].Columns[i].ColumnName + " : " + ds.Tables[0].Rows[0][i].ToString());

                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                        {
                            strFormAddress.Replace(ds.Tables[1].Rows[i]["COLUMN_NAME"].ToString().ToUpper(), ds.Tables[1].Rows[i]["DISPLAY_TEXT"].ToString());
                        }
                    }
                    funPriSetCustomerAddress(ds.Tables[0], strFormAddress, ucCustomerCodeLov);
                }

                FunPriLoadCustomerBasicDetails();
            }
            //Load Address End
            btnLoadCust.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnLoadCust_Click(object sender, EventArgs e)
    {
        try
        {
            TextBox txtCustNumber = (TextBox)ucCustomerCodeLov.FindControl("TxtName");
            TextBox txtAccItemNumber = (TextBox)ucCustomerCodeLov.FindControl("txtItemName");
            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            Button btnLoadCust = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
            //txtAccItemNumber.Text = hdnCID.Value;
            txtCustNumber.Text = txtCustomerNameDummy.Text = txtAccItemNumber.Text;

            //Load Address Start
            string strCustomerAddress = string.Empty;
            StringBuilder strFormAddress = new StringBuilder();
            // HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCID != null && hdnCID.Value != "")
            {
                //CustomerDetails1.ClearCustomerDetails();
                Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
                btnGetLOV.Focus();
                UserControl CustomerDetails1 = (UserControl)ucCustomerCodeLov.FindControl("S3GCustomerAddress1");
                TextBox txtCustomerName = (TextBox)CustomerDetails1.FindControl("txtCustomerName");
                txtCustomerCode.Text = txtCustomerNameDummy.Text = txtCustomerName.Text;
                DataSet ds = new DataSet();
                Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
                objProcedureParameters.Add("@Option", "1");
                objProcedureParameters.Add("@COMPANY_ID", intCompanyId.ToString());
                objProcedureParameters.Add("@CustomerId", hdnCID.Value);
                ds = Utility.GetDataset("S3G_OR_GET_CUSADDRESS", objProcedureParameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds.Tables[0].Columns.Count - 1; i++)
                    {
                        strFormAddress.Append(Environment.NewLine);
                        strFormAddress.Append(ds.Tables[0].Columns[i].ColumnName + " : " + ds.Tables[0].Rows[0][i].ToString());

                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                        {
                            strFormAddress.Replace(ds.Tables[1].Rows[i]["COLUMN_NAME"].ToString().ToUpper(), ds.Tables[1].Rows[i]["DISPLAY_TEXT"].ToString());
                        }
                    }
                    funPriSetCustomerAddress(ds.Tables[0], strFormAddress, ucCustomerCodeLov);
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    txtCustNumber.Text = ds.Tables[2].Rows[0]["Customer_Name"].ToString();
                }
            }
            //Load Address End

            ucCustomerCodeLov.SelectedValue = txtCustomerNameDummy.Text = hdnCID.Value;

            FunPriLoadCustomerBasicDetails();



            btnLoadCust.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    private void funPriSetCustomerAddress(DataTable dtCustomer, StringBuilder strAddress, UserControl ucCustomerCodeLovDyn)
    {
        try
        {

            DataRow dtrCustomer;
            dtrCustomer = dtCustomer.Rows[0];
            UserControl CustomerDetails1 = (UserControl)ucCustomerCodeLovDyn.FindControl("S3GCustomerAddress1");
            TextBox txtCustomerCode = (TextBox)CustomerDetails1.FindControl("txtCustomerCode");
            TextBox txtCustomerCode1 = (TextBox)CustomerDetails1.FindControl("txtCustomerCode1");
            TextBox txtCustomerName = (TextBox)CustomerDetails1.FindControl("txtCustomerName");
            TextBox txtCustomerName1 = (TextBox)CustomerDetails1.FindControl("txtCustomerName1");
            TextBox txtMobile = (TextBox)CustomerDetails1.FindControl("txtMobile");
            TextBox txtMobile1 = (TextBox)CustomerDetails1.FindControl("txtMobile1");
            TextBox txtPhone = (TextBox)CustomerDetails1.FindControl("txtPhone");
            TextBox txtPhone1 = (TextBox)CustomerDetails1.FindControl("txtPhone1");
            TextBox txtEmail = (TextBox)CustomerDetails1.FindControl("txtEmail");
            TextBox txtEmail1 = (TextBox)CustomerDetails1.FindControl("txtEmail1");
            TextBox txtWebSite = (TextBox)CustomerDetails1.FindControl("txtWebSite");
            TextBox txtWebSite1 = (TextBox)CustomerDetails1.FindControl("txtWebSite1");
            TextBox txtCusAddress = (TextBox)CustomerDetails1.FindControl("txtCusAddress");
            TextBox txtCusAddress1 = (TextBox)CustomerDetails1.FindControl("txtCusAddress1");
            if (dtrCustomer != null)
            {
                if (dtrCustomer.Table.Columns["Customer_Code"] != null)
                    txtCustomerCode1.Text = txtCustomerCode.Text = dtrCustomer["Customer_Code"].ToString();
                if (dtrCustomer.Table.Columns["Title"] != null)
                    txtCustomerName.Text = txtCustomerName1.Text = txtCustomerNameDummy.Text = dtrCustomer["Title"].ToString() + " " + dtrCustomer["Customer_Name"].ToString();
                else
                    txtCustomerName.Text = txtCustomerName1.Text = dtrCustomer["Customer_Name"].ToString();
                if (dtrCustomer.Table.Columns["MOB_PHONE_NO"] != null) txtMobile.Text = txtMobile1.Text = dtrCustomer["MOB_PHONE_NO"].ToString();
                if (dtrCustomer.Table.Columns.Contains("OFF_PHONE_NO"))
                {
                    txtPhone.Text = txtPhone1.Text = dtrCustomer["OFF_PHONE_NO"].ToString();
                }
                if (dtrCustomer.Table.Columns["CUST_EMAIL"] != null) txtEmail.Text = txtEmail1.Text = dtrCustomer["CUST_EMAIL"].ToString();
                if (dtrCustomer.Table.Columns["Comm_WebSite"] != null) txtWebSite.Text = txtWebSite1.Text = dtrCustomer["Comm_WebSite"].ToString();
                txtCusAddress.Text = txtCusAddress1.Text = strAddress.ToString();


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPubGetNegativeListDetails()
    {
        try
        {
            DataSet dsNegativeListDet = new DataSet();
            Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
            objProcedureParameters.Add("@COMPANY_ID", intCompanyId.ToString());
            objProcedureParameters.Add("@NegativeList_Id", Convert.ToString(intNegativeListID));
            dsNegativeListDet = Utility.GetDataset("S3G_ORG_GET_HOTLIST_DET", objProcedureParameters);

            if (dsNegativeListDet.Tables[0].Rows.Count > 0)
            {
                ddlNationality.SelectedValue = dsNegativeListDet.Tables[0].Rows[0]["ID"].ToString();
                ddlNationality.SelectedText = dsNegativeListDet.Tables[0].Rows[0]["Nationality"].ToString();
                ddlConstitutionName.SelectedValue = dsNegativeListDet.Tables[0].Rows[0]["Constitution_ID"].ToString();
                ddlNegativeList_Type.SelectedValue = dsNegativeListDet.Tables[0].Rows[0]["Negativelist_Type"].ToString();
                ddlNegativeList_Reason.SelectedValue = dsNegativeListDet.Tables[0].Rows[0]["NegativeList_Reason"].ToString();
                txtIdentityValue.Text = dsNegativeListDet.Tables[0].Rows[0]["CR_Number"].ToString();
                txtProspectMobile.Text = dsNegativeListDet.Tables[0].Rows[0]["PROSPECT_MOBILE"].ToString();
                txtProspectName.Text = dsNegativeListDet.Tables[0].Rows[0]["Prospect_Name"].ToString();
                txtProspectAddress.Text = dsNegativeListDet.Tables[0].Rows[0]["Prospect_Address"].ToString();
                txtDeclineDate.Text = dsNegativeListDet.Tables[0].Rows[0]["Decline_Date"].ToString();
                txtNegativelistRemarks.Text = dsNegativeListDet.Tables[0].Rows[0]["Negativelist_Remarks"].ToString();
                txtCodRemarks.Text = dsNegativeListDet.Tables[0].Rows[0]["Codified_Remarks"].ToString();
                chkIs_Active.Checked = Convert.ToBoolean(dsNegativeListDet.Tables[0].Rows[0]["Is_Active"]);

                if (dsNegativeListDet.Tables[0].Rows[0]["PROPOSAL_ID"].ToString() != "0")
                {
                    ListItem lst = new ListItem(dsNegativeListDet.Tables[0].Rows[0]["PROPOSAL_NO"].ToString(), dsNegativeListDet.Tables[0].Rows[0]["PROPOSAL_ID"].ToString());
                    ddlProposalNo.Items.Add(lst);
                }

                if (dsNegativeListDet.Tables[0].Rows[0]["Customer_ID"].ToString() == "0" || string.IsNullOrEmpty(dsNegativeListDet.Tables[0].Rows[0]["Customer_ID"].ToString()))
                {
                    divProspect.Visible = true;
                    divProspectMob.Visible = true;
                    divProspecAddress.Visible = true;
                    rblProspectCust.SelectedValue = "1";
                    rblProspectCust.Enabled = false;
                    divCustomerName.Visible = false;
                }
                else
                {
                    divProspect.Visible = false;
                    divProspectMob.Visible = false;
                    divProspecAddress.Visible = false;
                    rblProspectCust.SelectedValue = "2";
                    rblProspectCust.Enabled = false;
                    divCustomerName.Visible = true;

                    //bind branch address start

                    TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    txtName.Text = dsNegativeListDet.Tables[0].Rows[0]["CUSTOMER_DESC"].ToString();
                    txtName.Enabled = false;
                    txtName.ToolTip = txtName.Text;
                    Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
                    btnGetLOV.Visible = false;
                    hdnCustomerID.Value = dsNegativeListDet.Tables[0].Rows[0]["Customer_ID"].ToString();
                    ucCustomerCodeLov.SelectedValue = txtCustomerNameDummy.Text = hdnCustomerID.Value;
                    FunPriBindAddress(Convert.ToInt32(hdnCustomerID.Value));
                    //bind branch address end
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriBindAddress(int NegativeListID)
    {
        try
        {
            StringBuilder strFormAddress = new StringBuilder();
            DataSet ds = new DataSet();
            Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();


            objProcedureParameters.Add("@Option", "1");
            objProcedureParameters.Add("@COMPANY_ID", intCompanyId.ToString());
            objProcedureParameters.Add("@CustomerId", Convert.ToString(NegativeListID));
            ds = Utility.GetDataset("S3G_OR_GET_CUSADDRESS", objProcedureParameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= ds.Tables[0].Columns.Count - 1; i++)
                {
                    strFormAddress.Append(Environment.NewLine);
                    strFormAddress.Append(ds.Tables[0].Columns[i].ColumnName + " : " + ds.Tables[0].Rows[0][i].ToString());

                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                    {
                        strFormAddress.Replace(ds.Tables[1].Rows[i]["COLUMN_NAME"].ToString().ToUpper(), ds.Tables[1].Rows[i]["DISPLAY_TEXT"].ToString());
                    }
                }

                funPriSetCustomerAddress(ds.Tables[0], strFormAddress, ucCustomerCodeLov);


            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void ClearControls()
    {
        try
        {
            ddlNationality.SelectedText = "";
            ddlNationality.SelectedValue = "0";
            ddlConstitutionName.ClearSelection();
            ddlNegativeList_Type.ClearSelection();
            rblProspectCust.ClearSelection();

            txtProspectName.Clear();
            txtProspectMobile.Clear();
            txtProspectAddress.Clear();
            ddlNegativeList_Reason.ClearSelection();
            ddlProposalNo.Items.Clear();
            txtIdentityValue.Clear();
            txtDeclineDate.Clear();
            txtNegativelistRemarks.Clear();
            txtCodRemarks.Clear();
            chkIs_Active.Checked = true;
            chkIs_Active.Enabled = false;
            ucCustomerCodeLov.FunPubClearControlValue();
            ucCustomerCodeLov.ClearAddressControls();
            txtCustomerNameDummy.Clear();
            Session["Cust_Search_ConstitutionID"] = null;
            Session["Cust_Search_Nationality"] = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void funPriSetProgramName()
    {
        try
        {
            Dictionary<string, string> strProparm = new Dictionary<string, string>();
            strProparm.Add("@Program_ID", Convert.ToString(intProgram_Id));
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


    protected void ddlNationality_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            if (ddlNationality.SelectedValue == "" || ddlNationality.SelectedValue == "0")
            {
                Session["Cust_Search_Nationality"] = null;
            }
            else
            {
                Session["Cust_Search_Nationality"] = ddlNationality.SelectedValue;
            }

            ucCustomerCodeLov.Clear();
            ucCustomerCodeLov.ClearAddressControls();
            txtCustomerNameDummy.Clear();
            ddlProposalNo.Items.Clear();
            ddlProposalNo.Items.Insert(0, "--Select--");
            txtIdentityValue.Clear();
            ddlNationality.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ddlConstitutionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlConstitutionName.SelectedIndex > 1)
            {
                Session["Cust_Search_ConstitutionID"] = ddlConstitutionName.SelectedValue;
            }
            else
            {
                Session["Cust_Search_ConstitutionID"] = null;
            }



            ucCustomerCodeLov.Clear();
            ucCustomerCodeLov.ClearAddressControls();
            txtCustomerNameDummy.Clear();
            ddlProposalNo.Items.Clear();
            ddlProposalNo.Items.Insert(0, "--Select--");
            txtIdentityValue.Clear();
            ddlConstitutionName.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadCustomerBasicDetails()
    {
        try
        {
            if (ucCustomerCodeLov.SelectedValue != "0")
            {
                txtCustomerNameDummy.Text = ucCustomerCodeLov.SelectedValue;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Option", "794");
                Procparam.Add("@Param1", ucCustomerCodeLov.SelectedValue);
            }

            DataSet dsCustDetails = Utility.GetDataset("S3G_OR_GET_CUSTLOOKUP", Procparam);
            if (dsCustDetails.Tables.Count > 0)
            {
                if (dsCustDetails.Tables[0].Rows.Count > 0)
                {
                    ddlNationality.SelectedValue = dsCustDetails.Tables[0].Rows[0]["ID"].ToString();
                    ddlNationality.SelectedText = dsCustDetails.Tables[0].Rows[0]["Nationality"].ToString();
                    ddlConstitutionName.SelectedValue = dsCustDetails.Tables[0].Rows[0]["Constitution_Id"].ToString();
                    txtIdentityValue.Text = dsCustDetails.Tables[0].Rows[0]["NID_CR_RID_NUMBER"].ToString();
                }
                if (dsCustDetails.Tables[1].Rows.Count > 0)
                {
                    ddlProposalNo.BindDataTable(dsCustDetails.Tables[1], new string[] { "PROPOSAL_ID", "PROPOSAL_NO" });
                }
                else
                {
                    ddlProposalNo.Items.Clear();
                    ddlProposalNo.Items.Insert(0, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

}