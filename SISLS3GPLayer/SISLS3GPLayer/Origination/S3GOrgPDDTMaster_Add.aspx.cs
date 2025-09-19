#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: PRDDT Creation
/// Created By			: Kannan RC
/// Created Date		: 15-July-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 10-Aug-2010   
/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Collections;
using System.Web.UI.HtmlControls;
using S3GBusEntity.Origination;
using PRDDCMgtServicesReference;
using System.IO;


public partial class Origination_S3GOrgPDDTMaster_Add : ApplyThemeForProject
{
    #region Intialization
    public string Program_Id = "41";
    int intCompanyId;
    public int intUserId;
    string strUserLogin = string.Empty;
    int PRDDTransID = 0;
    int intErrCode = 0;
    public string strDateFormat = string.Empty;
    string strMode = string.Empty;
    int maxversion = 0;
    bool chkbox = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    bool bMod = false;
    DataTable dt = new DataTable();
    Dictionary<string, string> Procparam;
    string strRedirectPage = "../Origination/S3GOrgPDDTMaster_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgPDDTMaster_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgPDDTMaster_View.aspx';";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    Dictionary<string, string> dictParam = null;
    SerializationMode SerMode = SerializationMode.Binary;
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    PRDDCMgtServicesClient objPRDDT_MasterClient;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();

    //PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
    //PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable ObjS3G_PRDDTLookUpDataTable = new PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable();

    PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable ObjS3G_PRDDDocCategoryDataTable = null;
    PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable ObjS3G_PRDDTransMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable ObjS3G_PRDDTransDocMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable ObjS3G_ORG_PRDDCTransMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable ObjS3G_ORG_PRDDCTransDocMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable ObjS3G_ORG_ExitsPRDDCTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable();
    PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterRow ObjS3G_ORG_ExitsPRDDCTransRows;
    //added by  saranya 
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;
    public static Origination_S3GOrgPDDTMaster_Add obj_PageValue;
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {

        obj_PageValue = this;
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        strUserLogin = ObjUserInfo.ProUserLoginRW;
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end        
        txtDate.Attributes.Add("readonly", "readonly");
        //S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        txtDate.Text = DateTime.Today.ToString(strDateFormat);
        ProgramCode = "041";
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            PRDDTransID = Convert.ToInt32(fromTicket.Name);
            ViewState["PRDDTransID"] = PRDDTransID;
            strMode = Request.QueryString["qsMode"];
        }
        Button btnCustomer = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
        btnCustomer.Enabled = false;
        if (!IsPostBack)
        {

            if (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow)
            {
                FunPriLoadRefPoint();
                FunPriGetLookUpList();

            }

            if ((PRDDTransID > 0) && (strMode == "M"))
            {
                FunPriDisableControls(1);
            }
            else if ((PRDDTransID > 0) && (strMode == "Q"))
            {
                FunPriDisableControls(-1);
            }
            else
            {
                //  FunPriLoadRefPoint();
                FunPriGetLookUpList();
                FunPriDisableControls(0);
            }
            // ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;

            // WF 
            if (PageMode == PageModes.WorkFlow)
            {
                ViewState["PageMode"] = PageModes.WorkFlow;
            }
            if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())
            {
                try
                {
                    PreparePageForWorkFlowLoad();
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                    Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
                }
            }
        }
  
        ddlRefPoint.Focus();
        if (PageMode == PageModes.WorkFlow)
        {
            if (ViewState["PRDDTransID"] != null)
                PRDDTransID = Convert.ToInt32(ViewState["PRDDTransID"]);
        }

        //if (!IsPostBack)
        //{
        //foreach (GridViewRow grvData in gvPRDDT.Rows)
        //{
        //    Label myThrobber = (Label)grvData.FindControl("myThrobber");
        //    HiddenField hidThrobber = (HiddenField)grvData.FindControl("hidThrobber");

        //    if (hidThrobber.Value != "")
        //    {
        //        myThrobber.Text = hidThrobber.Value;
        //    }
        //}
        //}

        //Values assgnment for Csutomer selection control

        //if (ddlBranch.SelectedValue.ToString() != "0")
        //{
        //    ucCustomerCodeLov.strBranchID = ddlBranch.SelectedValue.ToString();
        //}
        //else
        //{
        //    ucCustomerCodeLov.strBranchID = "-1";
        //}
        //if (ddlLOB.SelectedValue.ToString() != "0")
        //{
        //    ucCustomerCodeLov.strLOBID = ddlLOB.SelectedValue.ToString();
        //}
        //else
        //{
        //    ucCustomerCodeLov.strLOBID = "-1";
        //}
        //if (ddlConstitition.Items.Count > 0 && ddlConstitition.SelectedValue.ToString() != "0")
        //{
        //    ucCustomerCodeLov.strRegionID = ddlConstitition.SelectedValue.ToString();
        //}
        //else
        //{
        //    ucCustomerCodeLov.strRegionID = "-1";
        //}
        //ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;

        ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
        TextBox txtCustItemNumber = ((TextBox)ucCustomerCodeLov.FindControl("txtItemName"));
        txtCustItemNumber.Attributes.Add("onfocus", "fnLoadCustomer()");
        txtCustItemNumber.Attributes.Add("readonly", "false");
        txtCustItemNumber.Width = 0;
        txtCustItemNumber.TabIndex = -1;
        txtCustItemNumber.BorderStyle = BorderStyle.None;


        //TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        //if (PageMode != PageModes.WorkFlow)
        //{
        //    txt.Attributes.Add("onfocus", "fnLoadCustomer()");
        //}
        //txt.ToolTip = "Customer Code";
    }
    #endregion
    #region "WF Code"
    void PreparePageForWorkFlowLoad()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowServiceClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

        try
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            byte[] byte_PreDisDoc = objWorkflowServiceClient.FunPubLoadPreDisDocTransaction(WFSessionValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValues.Document_Type);
            DataSet dsWorkflow = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_PreDisDoc, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));

            if (dsWorkflow.Tables.Count > 1)
            {
                if (dsWorkflow.Tables[1].Rows.Count > 0)
                {
                    PRDDTransID = Convert.ToInt32(dsWorkflow.Tables[1].Rows[0]["Doc_Id"].ToString());
                    ViewState["PRDDTransID"] = PRDDTransID;
                    FunPriDisableControls(1);
                }
            }
            else
            {
                if (dsWorkflow.Tables[0].Rows.Count > 0)
                {
                    ddlLOB.Items.Add(new ListItem(dsWorkflow.Tables[0].Rows[0]["LOB"].ToString(), dsWorkflow.Tables[0].Rows[0]["LOB_ID"].ToString()));
                    ddlLOB.ToolTip = dsWorkflow.Tables[0].Rows[0]["LOB"].ToString();
                    //ddlLOB.ClearDropDownList();

                    ddlBranch.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Location_ID"].ToString();
                    //ddlBranch.SelectedText = dsWorkflow.Tables[0].Rows[0]["Location_Name"].ToString();
                    // ddlBranch.Clear();
                    //FunPriLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
                    ddlConstitition.Items.Add(new ListItem(dsWorkflow.Tables[0].Rows[0]["Constitution_Name"].ToString(), dsWorkflow.Tables[0].Rows[0]["Constitution_ID"].ToString()));
                    ddlConstitition.ToolTip = dsWorkflow.Tables[0].Rows[0]["Constitution_Name"].ToString();
                    // ddlConstitition.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Constitution_ID"].ToString();
                    //  ddlConstitition.ClearDropDownList();

                    //ddlRefPoint.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString();
                    //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), PRDDTransID);
                    ddlRefPoint.Items.Add(new ListItem(dsWorkflow.Tables[0].Rows[0]["Ref_Name"].ToString(), dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString()));
                    ddlRefPoint.ToolTip = dsWorkflow.Tables[0].Rows[0]["Ref_Name"].ToString();

                    lblEnquiry.CssClass = "styleReqFieldLabel";
                    if (dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString() == "1")
                    {
                        lblEnquiry.Text = "Enquiry No.";
                    }
                    else if (dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString() == "2")
                    {
                        lblEnquiry.Text = "Checklist Number.";
                    }
                    else if (dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString() == "3")
                    {
                        lblEnquiry.Text = "Application No.";
                    }
                    else
                    {
                        lblEnquiry.Text = "Ref. Code";
                    }
                    // FunPriBindRefNo();
                    ddlEnquiry.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Document_Type_ID"].ToString();
                    //ddlEnquiry.SelectedText = dsWorkflow.Tables[0].Rows[0]["Document_No"].ToString();
                    ddlEnquiry.ToolTip = dsWorkflow.Tables[0].Rows[0]["Document_No"].ToString();



                    //PopulateCustomerCode();
                    FunPriGetCustomerDetails();



                    // ddlRefPoint.ClearDropDownList();
                    //ddlEnquiry.Clear();
                    FunPriGetPRDDTransType();
                    tpPDDT.Enabled = true;
                    gvPRDDT.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objWorkflowServiceClient.Close();
        }
    }
    #endregion
    public void read()
    {
        txtCustomerID.ReadOnly = true;
        txtCustomerCode.ReadOnly = true;
        txtCustomerName.ReadOnly = true;
        txtStatus.ReadOnly = true;
        ddlLOB.Focus();
    }

    #region DDL
    private void FunPriGetLookUpList()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (PRDDTransID == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_Id", intUserId.ToString());
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Program_Id", Program_Id);

            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Name" });

            FunpriloadBranch();

            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
            //if (strMode != "C")
            //{
            //    ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code ", "Location_Name" });
            //}
            //ddlEnquiry.BindDataTable("S3G_ORG_GetPDDTEnquiryNumber", Procparam, new string[] { "Enquiry_Response_ID", "Enquiry_No" });


            //FunPriLoadCustomer();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunpriloadBranch()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@User_Id", intUserId.ToString());
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        if (ddlLOB.SelectedIndex > 0)
            Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
        Procparam.Add("@Program_Id", Program_Id);
        ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
    }

    private void FunPriLoadCustomer()
    {
        Procparam = new Dictionary<string, string>();
        if (PRDDTransID == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }
        Procparam.Add("@User_Id", intUserId.ToString());
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Program_Id", Program_Id);
        Procparam.Add("@Type", "CUSTOMER");
        if (ddlRefPoint.SelectedIndex > 0)
            Procparam.Add("@Option", ddlRefPoint.SelectedValue);
        if (ddlEnquiry.SelectedValue != "0")
            Procparam.Add("@Ref_ID", ddlEnquiry.SelectedValue);

        ddlCustomerCode.BindDataTable("S3G_ORG_GetPDDTCustomer", Procparam, true, "-- Select --", new string[] { "Customer_ID", "Customer_Code" });


    }

    private void FunPriLoadRefPoint()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OptionValue", "1");
            DataTable dt = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", Procparam);
            DataRow[] dr = dt.Select("Value IN(2,3)", "Name", DataViewRowState.CurrentRows);
            //DataRow[] dr = dt.Select("Value IN(1,2,3)", "Name", DataViewRowState.CurrentRows);
            dt = dr.CopyToDataTable();
            ddlRefPoint.BindDataTable(dt, new string[] { "Value", "Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlRefPoint_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlEnquiry.SelectedText = string.Empty;
            ddlEnquiry.SelectedValue = string.Empty;
            FunPriGetLookUpList();
            //tpPDDT.Enabled = false;
            gvPRDDT.Visible = false;
            //if (ddlEnquiry.Items.Count > 0)
            //    ddlEnquiry.Items.Clear();
            //ddlEnquiry.Clear();
            ddlConstitition.Items.Clear();
            Clear();
            ddlBranch.ClearSelection();
            FunProClearCustomerDetails(false);
            ucCustomerCodeLov.SelectedText = string.Empty;
            ucCustomerCodeLov.SelectedValue = string.Empty;
            ucCustomerCodeLov.ClearAddressControls();
            ucCustomerCodeLov.FunPubClearControlValue();
            ddlEnquiry.SelectedValue = string.Empty;
            ddlEnquiry.SelectedText = string.Empty;
            txtProduct.Text = string.Empty;
            txtProductName.Text = string.Empty;
            // FunPriLoadCustomer();
            //if (ddlConstitition.Items.Count > 0)
            //    ddlConstitition.Items.Clear();

            if (ddlRefPoint.SelectedIndex > 0)
            {
                ddlLOB.SelectedIndex = 0;
                lblEnquiry.CssClass = "styleReqFieldLabel";
                //rfvEnqNo.Enabled = true;
                if (ddlRefPoint.SelectedValue == "1")
                {
                    lblEnquiry.Text = "Enquiry No.";
                    //ddlEnquiry.ErrorMessage = "Select Enquiry Number";
                }
                else if (ddlRefPoint.SelectedValue == "2")
                {
                    lblEnquiry.Text = "Checklist Number";
                    //ddlEnquiry.ErrorMessage = "Select Pricing Number";
                }
                else if (ddlRefPoint.SelectedValue == "3")
                {
                    lblEnquiry.Text = "Proposal No";
                    //ddlEnquiry.ErrorMessage = "Select Application Number";
                }
                else
                {
                    lblEnquiry.Text = "Ref. Code";
                    // rfvEnqNo.Enabled = false;
                }

                //Shibu
                //if (ddlCustomerCode.SelectedIndex > 0)
                //{
                //    PopulateEnquiry();
                //}
                //else
                //{
                //    FunPriBindRefNo();
                //}
                //if (ddlEnquiry.Items.Count == 2)
                //{
                //    ddlEnquiry.SelectedValue = ddlEnquiry.Items[1].Value;
                //    FunPriGetCustomerDetails();
                //    FunPriGetPRDDTransType();
                //    tpPDDT.Enabled = true;
                //    gvPRDDT.Visible = true;
                //}
                //End



                //if (ddlCustomerCode.SelectedIndex != 0 && ddlEnquiry.Items.Count > 2)
                //{
                //    Dictionary<string, string> dictParam = new Dictionary<string, string>();
                //    dictParam.Add("@Customer_Id", ddlCustomerCode.SelectedValue.ToString());
                //    dictParam.Add("@Company_ID", intCompanyId.ToString());
                //    DataTable dtCustDetails = Utility.GetDefaultData("S3G_GetCustomerDetails", dictParam);

                //    if (dtCustDetails.Rows.Count > 0)
                //        fnBindCustomerDetails(dtCustDetails.Rows[0], "Customer");
                //}
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }
    //No use Ref No DDL this Control changed to Auto Suggestion By Shibu 
    //private void FunPriBindRefNo()
    //{
    //    // ddlEnquiry.Items.Clear();
    //    if (Procparam != null)
    //        Procparam.Clear();
    //    else
    //        Procparam = new Dictionary<string, string>();



    //    Procparam.Add("@Company_ID", intCompanyId.ToString());
    //    Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
    //    Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
    //    Procparam.Add("@Constitution_ID", ddlConstitition.SelectedValue);
    //    Procparam.Add("@OptionValue", ddlRefPoint.SelectedValue);
    //    ddlEnquiry.BindDataTable("S3G_ORG_GetPDDTRefDocNo", Procparam, new string[] { "RefDoc_ID", "RefDoc_No" });
    //}

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
            //if (ddlConstitition.SelectedValue != "")
            //    ddlConstitition.ClearDropDownList();

            ucCustomerCodeLov.SelectedText = string.Empty;
            ucCustomerCodeLov.SelectedValue = string.Empty;
            ucCustomerCodeLov.ClearAddressControls();
            ucCustomerCodeLov.FunPubClearControlValue();
            ddlEnquiry.SelectedValue = string.Empty;
            ddlEnquiry.SelectedText = string.Empty;
            txtProduct.Text = string.Empty;
            txtProductName.Text = string.Empty;

            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            {
                ddlBranch.ClearSelection();
            }
            else
            {
                if (ddlBranch.SelectedValue != "0") ddlBranch.SelectedValue = "0";
            }

            //if (ddlEnquiry.SelectedValue != null)
            //    ddlEnquiry.Items.Clear();


            //Clear();
            // FunPriLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
            if (Convert.ToInt32(ddlLOB.SelectedValue) < 0)
            {
                //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
                //Dictionary<string, string> Procparam = new Dictionary<string, string>();
                //Procparam.Add("@Is_Active", "1");
                //Procparam.Add("@User_Id", intUserId.ToString());
                //Procparam.Add("@LOB_Id", ddlLOB.SelectedValue.ToString());
                //Procparam.Add("@Company_ID", intCompanyId.ToString());
                //Procparam.Add("@Program_Id", Program_Id);
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code ", "Location_Name" });
            }
            else
            {
                if (ddlBranch.SelectedValue != "0") ddlBranch.SelectedValue = "0";
                if (ddlConstitition.Items.Count > 0) ddlConstitition.SelectedIndex = 0;
                ddlBranch.ClearDropDownList();
                //ddlEnquiry.Clear();
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
                FunProClearCustomerDetails(false);
                ddlConstitition.Items.Clear();
                ucCustomerCodeLov.ButtonEnabled = false;
                //ddlEnquiry.SelectedIndex = 0;
            }
            ddlLOB.Focus();
            //if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            //{
            //    FunPriBindRefNo();
            //}
            //FunPriLoadLocation(intCompanyId, intUserId, intCompanyId, Convert.ToInt32(ddlLOB.SelectedValue));
            FunpriloadBranch();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }
    // No Need Constitution it's build Based On Customer( Each Customer have Only One Constitution)  By Shibu 
    private void FunPriLoadConstitution(int intLineofBusinessID)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            if (PageMode == PageModes.Create)
            {
                dictParam.Add("@Is_Active", "1");
            }
            dictParam.Add("@LOB_ID", intLineofBusinessID.ToString());
            ddlConstitition.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, dictParam, new string[] { "Constitution_ID", "ConstitutionName" });
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp, lblHeading.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }
    //Not Use
    protected void ddlConstitition_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlConstitition.SelectedValue) > 0)
            {
                //saranya
                //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), PRDDTransID);
                //FunPriBindRefNo();
            }
            else
            {
                //if (ddlEnquiry.Items.Count > 0) ddlEnquiry.SelectedIndex = 0;
                //Clear();
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {



            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {

                ddlConstitition.Items.Clear();
                FunProClearCustomerDetails(true);
                //ddlEnquiry.Clear();
                FunProGetCustomerDetails(hdnCustomerId.Value.ToString());
                // FunProLoadPANum(Convert.ToInt32(hdnCustomerId.Value.ToString()), Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), Convert.ToInt32(ViewState["intPDDTransID"]));

                ViewState["CustomerID"] = hdnCustomerId.Value;


            }

            string strCustomerAddress = string.Empty;
            StringBuilder strFormAddress = new StringBuilder();

            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {
                DataSet ds = new DataSet();
                Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
                objProcedureParameters.Add("@Option", "1");
                objProcedureParameters.Add("@COMPANY_ID", intCompanyId.ToString());
                objProcedureParameters.Add("@CustomerId", hdnCustomerId.Value);
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
                    FunPriLoadAddressDetails(Convert.ToInt32(hdnCustomerId.Value));
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ViewState["ConsitutionId"] = ds.Tables[2].Rows[0]["CONSTITUTION_CODE"].ToString();

                }
            }
            Button btnLoadAccount = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
            btnLoadAccount.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to display Customer Details");
        }
    }
    private void FunPriLoadAddressDets(int intCustomerId)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            hdnCustomerId.Value = intCustomerId.ToString();
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {

                ddlConstitition.Items.Clear();
                FunProClearCustomerDetails(true);
                //ddlEnquiry.Clear();
                FunProGetCustomerDetails(hdnCustomerId.Value.ToString());
                // FunProLoadPANum(Convert.ToInt32(hdnCustomerId.Value.ToString()), Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), Convert.ToInt32(ViewState["intPDDTransID"]));

                ViewState["CustomerID"] = hdnCustomerId.Value;


            }

            string strCustomerAddress = string.Empty;
            StringBuilder strFormAddress = new StringBuilder();

            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {
                DataSet ds = new DataSet();
                Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
                objProcedureParameters.Add("@Option", "1");
                objProcedureParameters.Add("@COMPANY_ID", intCompanyId.ToString());
                objProcedureParameters.Add("@CustomerId", hdnCustomerId.Value);
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
                    //FunPriLoadAddressDetails(Convert.ToInt32(hdnCustomerId.Value));
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ViewState["ConsitutionId"] = ds.Tables[2].Rows[0]["CONSTITUTION_CODE"].ToString();

                }
            }
            Button btnLoadAccount = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
            btnLoadAccount.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadAddressDetails(int intCustomerId)
    {
        try
        {
            //S3GCustomerCommAddress.BindAddressSetupDetails(1);
            DataSet ds = new DataSet();
            Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
            objProcedureParameters.Add("@Option", "1");
            objProcedureParameters.Add("@COMPANY_ID", intCompanyId.ToString());
            objProcedureParameters.Add("@CustomerId", intCustomerId.ToString());
            ds = Utility.GetDataset("S3G_OR_GET_CUSTOTHERS_DTLS", objProcedureParameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //S3GCustomerCommAddress.SetAddressDetails(ds.Tables[0].Rows[0]);
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                //txtDateofBirth.Text = ds.Tables[1].Rows[0]["DATEOFBIRTH"].ToString();
                //lblDateofBirth.Text = ds.Tables[1].Rows[0]["NAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
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

            TextBox TxtAccNumber = (TextBox)ucCustomerCodeLov.FindControl("TxtName");
            TextBox txtAccItemNumber = (TextBox)ucCustomerCodeLov.FindControl("txtItemName");

            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

            txtAccItemNumber.Text = hdnCID.Value;
            TxtAccNumber.Text = hdnCID.Value;


            if (dtrCustomer != null)
            {
                if (dtrCustomer.Table.Columns["Customer_Code"] != null)
                    txtCustomerName.Text = txtCustomerCode1.Text = txtCustomerCode.Text = dtrCustomer["Customer_Code"].ToString();
                if (dtrCustomer.Table.Columns["Title"] != null)
                    TxtAccNumber.Text = txtAccItemNumber.Text = txtCustomerName.Text = txtCustomerName1.Text = dtrCustomer["Customer_Name"].ToString();
                else
                    txtCustomerName.Text = TxtAccNumber.Text = txtAccItemNumber.Text = txtCustomerName.Text = txtCustomerName1.Text = dtrCustomer["Customer_Name"].ToString();
                if (dtrCustomer.Table.Columns["MOB_PHONE_NO"] != null) txtMobile.Text = txtMobile1.Text = dtrCustomer["MOB_PHONE_NO"].ToString();
                if (dtrCustomer.Table.Columns.Contains("Comm_Telephone"))
                {
                    txtPhone.Text = txtPhone1.Text = dtrCustomer["Comm_Telephone"].ToString();
                }
                if (dtrCustomer.Table.Columns["Cust_Email"] != null) txtEmail.Text = txtEmail1.Text = dtrCustomer["Cust_Email"].ToString();
                if (dtrCustomer.Table.Columns["Comm_WebSite"] != null) txtWebSite.Text = txtWebSite1.Text = dtrCustomer["Comm_WebSite"].ToString();
                txtCusAddress.Text = txtCusAddress1.Text = strAddress.ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadEnquiryNumber(int intLineofBusinessID, int intBranchID, int intConstititionID, int PRDDTransID)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@LOB_ID", intLineofBusinessID.ToString());
            dictParam.Add("@Location_Id", intBranchID.ToString());
            dictParam.Add("@Constitution_ID", intConstititionID.ToString());
            dictParam.Add("@Document_Type", ddlRefPoint.SelectedValue);
            dictParam.Add("@PreDisbursement_Doc_Tran_ID", PRDDTransID.ToString());
            dictParam.Add("@Program_Id", Program_Id);
            //Shibu
            // ddlEnquiry.BindDataTable("S3G_ORG_GetEnquiryNumber", dictParam, new string[] { "ID", "Number" });
            ddlCustomerCode.BindDataTable("S3G_ORG_GetPRDDCustomerDropdown", dictParam, true, "-- Select --", new string[] { "Customer_ID", "Customer_Code" });

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
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
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("SA_GET_CUSTOMER_AGT", Procparam));

        return suggetions.ToArray();

    }

    private void FunPriGetPRDDTransID()
    {
        try
        {
            Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            DataTable dtConstitution = new DataTable();
            string strCustomerAddress = string.Empty;
            StringBuilder strFormAddress = new StringBuilder();
            if (ddlRefPoint.SelectedValue == "1")
            {

                Dictionary<string, string> dictparam2 = new Dictionary<string, string>();
                dictparam2.Add("@Enquiry_ID", ddlEnquiry.SelectedValue);
                dictparam2.Add("@Company_ID", intCompanyId.ToString());
                dtConstitution = Utility.GetDefaultData("S3G_ORG_GET_CONSTITUTION", dictparam2);

                ListItem lst = new ListItem(dtConstitution.Rows[0]["Constitution_Name"].ToString(), dtConstitution.Rows[0]["Constitution_ID"].ToString());
                ddlConstitition.Items.Add(lst);
            }

            dictParam1.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (ddlConstitition.Items.Count > 0 && ddlConstitition.SelectedValue != "0")
            {
                dictParam1.Add("@Constitution_ID", ddlConstitition.SelectedValue);
            }
            dictParam1.Add("@Company_ID", intCompanyId.ToString());
            dictParam1.Add("@Option", ddlRefPoint.SelectedValue);
            dictParam1.Add("@Ref_ID", ddlEnquiry.SelectedValue);

            DataSet dsDetails = Utility.GetDataset("S3G_ORG_GetPRDDTID", dictParam1);
            if (dsDetails.Tables[0].Rows.Count > 0)
            {
                DataRow dtRow = dsDetails.Tables[0].Rows[0];
                hidPRTID.Value = dtRow["PRDDC_ID"].ToString();
            }
            //FunPriLoadAddressDets(Convert.ToInt32(dtConstitution.Rows[0]["Customer_ID"].ToString()));
            hdnCustomerId.Value = dsDetails.Tables[1].Rows[0]["Customer_ID"].ToString();
            DataRow dtRow1 = dsDetails.Tables[1].Rows[0];
            ddlConstitition.Items.Clear();
            ddlConstitition.Items.Add(new ListItem(dtRow1["Constitution_Name"].ToString(), dtRow1["Constitution_ID"].ToString()));
            ddlConstitition.SelectedValue = dtRow1["Constitution_ID"].ToString();
            txtProductName.Text = dsDetails.Tables[1].Rows[0]["PRODUCT_NAME"].ToString();
            txtProduct.Text = dsDetails.Tables[1].Rows[0]["PRODUCT_ID"].ToString();
            hdnofferdate.Value = dsDetails.Tables[1].Rows[0]["OFFER_DATE"].ToString();
            if (ddlRefPoint.SelectedValue == "1")
            {
                if (dtConstitution.Rows[0]["Customer_ID"] != null || dtConstitution.Rows[0]["Customer_ID"].ToString() != "")
                {
                    FunPriGetCustomerDetails();
                }
            }

            if (dsDetails.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i <= dsDetails.Tables[1].Columns.Count - 1; i++)
                {
                    strFormAddress.Append(Environment.NewLine);
                    strFormAddress.Append(dsDetails.Tables[1].Columns[i].ColumnName + " : " + dsDetails.Tables[1].Rows[0][i].ToString());

                }


                if (dsDetails.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i <= dsDetails.Tables[2].Rows.Count - 1; i++)
                    {
                        strFormAddress.Replace(dsDetails.Tables[2].Rows[i]["COLUMN_NAME"].ToString().ToUpper(), dsDetails.Tables[2].Rows[i]["DISPLAY_TEXT"].ToString());
                    }
                }
                funPriSetCustomerAddress(dsDetails.Tables[1], strFormAddress, ucCustomerCodeLov);
                FunPriLoadAddressDetails(Convert.ToInt32(hdnCustomerId.Value));
            }
            //if (ds.Tables[2].Rows.Count > 0)
            //{
            //    ViewState["ConsitutionId"] = ds.Tables[2].Rows[0]["CONSTITUTION_CODE"].ToString();

            //}
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriGetPRDDTransType()
    {
        if (hidPRTID.Value != "")
        {
            objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
            try
            {
                PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryRow ObjPRDDTypeTransRow;
                ObjS3G_PRDDDocCategoryDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable();
                ObjPRDDTypeTransRow = ObjS3G_PRDDDocCategoryDataTable.NewS3G_ORG_PRDDCDocumentCategoryRow();
                ObjPRDDTypeTransRow.Company_ID = intCompanyId;
                ObjPRDDTypeTransRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                if (ddlConstitition.SelectedValue != "" && ddlConstitition.SelectedValue != "0")
                {
                    ObjPRDDTypeTransRow.Constitution_ID = Convert.ToInt32(ddlConstitition.SelectedValue);
                }
                ObjPRDDTypeTransRow.Option = Convert.ToInt32(ddlRefPoint.SelectedValue);
                ObjPRDDTypeTransRow.Ref_ID = Convert.ToInt32(ddlEnquiry.SelectedValue);
                if (hidPRTID.Value != "") ObjPRDDTypeTransRow.PRDDC_ID = Convert.ToInt32(hidPRTID.Value);
                ObjS3G_PRDDDocCategoryDataTable.AddS3G_ORG_PRDDCDocumentCategoryRow(ObjPRDDTypeTransRow);





                //byte[] bytePRDDTypeTrans = objPRDDT_MasterClient.FunPubGetTypeTrans(SerMode, ClsPubSerialize.Serialize(ObjS3G_PRDDDocCategoryDataTable, SerMode));
                //PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable dtPRDDTypeTrans = (PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable)ClsPubSerialize.DeSerialize(bytePRDDTypeTrans, SerMode, typeof(PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable));

                DataTable dt;
                Dictionary<string, string> strProparm = new Dictionary<string, string>();
                strProparm.Add("@LOB_ID", ddlLOB.SelectedValue);
                strProparm.Add("@Constitution_ID", ddlConstitition.SelectedValue);
                strProparm.Add("@Company_ID", intCompanyId.ToString());
                strProparm.Add("@Option", ddlRefPoint.SelectedValue);
                strProparm.Add("@Ref_ID", ddlEnquiry.SelectedValue);
                strProparm.Add("@PRDDC_ID", hidPRTID.Value);
                dt = Utility.GetDefaultData("S3G_OR_GET_PRDDTLOOKUP", strProparm);



                if (dt.Rows.Count > 0)
                {
                    gvPRDDT.DataSource = dt;
                    gvPRDDT.DataBind();
                    ViewState["dtPRDDTypeTrans"] = dt;
                    gvPRDDT.Visible = true;
                    tpPDDT.Visible = true;
                    ViewState["Docpath"] = dt.Rows[0]["Document_Path"].ToString();

                    for (int i = 0; i < gvPRDDT.Rows.Count; i++)
                    {
                        if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            //Commented and added by saranya for fixing observation on 01-Mar-2012
                            TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                            //Label txtScanBy = (Label)gvPRDDT.Rows[i].FindControl("txtScannedBy");
                            UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[i].FindControl("ddlScannedBy");
                            Label lblScannedBy = (Label)gvPRDDT.Rows[i].FindControl("lblScannedBy");
                            //End Here
                            ImageButton hyplnkView = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                            FileUpload asyFileUpload = (FileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");
                            //AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");
                            CheckBox CbxCheck = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");

                            Label lblReceivedStatus = (Label)gvPRDDT.Rows[i].FindControl("lblReceivedStatus");
                            Label lblChecklistFlag = (Label)gvPRDDT.Rows[i].FindControl("lblChecklistFlag");
                            DropDownList ddlCollAndWaiver = (DropDownList)gvPRDDT.Rows[i].FindControl("ddlCollAndWaiver");
                            UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[i].FindControl("ddlCollectedBy");
                            TextBox txtCollectedDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtCollectedDate");
                            TextBox txtRemarks = (TextBox)gvPRDDT.Rows[i].FindControl("txtRemarks");
                            ddlCollectedBy.Enabled = false;

                            if (lblChecklistFlag.Text == "1" && lblReceivedStatus.Text != "0")
                            {
                                //ddlCollAndWaiver.Enabled = false;
                                //ddlCollectedBy.Enabled = false;
                                //txtCollectedDate.Enabled = false;
                                //ddlScannedBy.Enabled = false;
                                //txtScanDate.Enabled = false;

                                //if (strMode != "C")
                                //{
                                //   asyFileUpload.Enabled = false;
                                //}

                                //if (strMode != "C")
                                //{
                                //    asyFileUpload.Enabled = false;
                                //}

                                //txtRemarks.Enabled = false;

                                CbxCheck.Enabled = false;
                                CbxCheck.Checked = true;
                                if (lblReceivedStatus.Text == "2" || lblReceivedStatus.Text == "NC")
                                {
                                    CbxCheck.Enabled = true;
                                    CbxCheck.Checked = false;
                                }
                                //ddlCollAndWaiver.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));

                                if (lblReceivedStatus.Text == "1" || lblReceivedStatus.Text == "C")
                                {
                                    ddlCollAndWaiver.Enabled = false;
                                    ddlCollectedBy.Enabled = false;
                                    txtCollectedDate.Enabled = false;
                                    lblReceivedStatus.Text = "C";
                                    ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                                    ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                                }
                                else if (lblReceivedStatus.Text == "3" || lblReceivedStatus.Text == "W")
                                {
                                    lblReceivedStatus.Text = "W";
                                    ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                                    ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                                }
                                else if (lblReceivedStatus.Text == "2" || lblReceivedStatus.Text == "NC")
                                {
                                    lblReceivedStatus.Text = "NC";
                                    ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                                    ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                                }
                                else if (lblReceivedStatus.Text == "4" || lblReceivedStatus.Text == "NA")
                                {
                                    lblReceivedStatus.Text = "NA";
                                    ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                                    ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                                }
                                //ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                                //if (lblReceivedStatus.Text == "1")
                                //{
                                //    ddlCollAndWaiver.SelectedItem.Text = "Collected";
                                //}
                            }

                            if (lblReceivedStatus.Text == "0")
                            {
                                ddlCollectedBy.SelectedText = ObjUserInfo.ProUserNameRW;

                                ddlCollectedBy.SelectedValue = ObjUserInfo.ProUserIdRW.ToString();
                                hyplnkView.Enabled = false;
                                hyplnkView.CssClass = "styleGridEditDisabled";
                            }

                            if ((dt.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "false")
                                 || (dt.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "0"))
                            // && dtPRDDTypeTrans.Rows[i]["Is_Mandatory"].ToString() == "False")
                            {
                                //Commented and added by saranya for fixing observation on 01-Mar-2012
                                //txtScanDate.Enabled = ddlScannedBy.Enabled = false;
                                //txtScanDate.Text = "";
                                //ddlScannedBy.ClearDropDownList();
                                //txtScanDate.Visible = ddlScannedBy.Visible = false;
                                //End Here
                                //if (strMode != "C")
                                //{
                                //    asyFileUpload.Enabled = false;
                                //    hyplnkView.Enabled = false;
                                //    hyplnkView.CssClass = "styleGridEditDisabled";
                                //}
                            }
                        }
                    }
                }
                else
                {
                    gvPRDDT.DataSource = null;
                    gvPRDDT.DataBind();
                    Utility.FunShowAlertMsg(this, "Document details not defined in Pre Disbursements Master");
                    strRedirectPageView = strRedirectPageAdd;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                    return;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Document details not defined in predisbursment master')", true);
                }
            }
            catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
            {
                lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
                lblErrorMessage.Text = ex.Message;
            }
            finally
            {
                objPRDDT_MasterClient.Close();
            }
        }
    }

    protected void ddlEnquiry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlEnquiry.SelectedValue != "0")
            {
                // PopulateCustomerCode();
                //FunPriGetCustomerDetails();
                FunPriGetPRDDTransID();
                FunPriGetPRDDTransType();
                //FunPriGetCustomerDetails();
                tpPDDT.Visible = true;
                gvPRDDT.Visible = true;
                tpPDDT.Enabled = true;
            }
            else
            {
                //PopulateCustomerCode();
                // if (ddlConstitition.Items.Count > 0) ddlConstitition.SelectedIndex = 0;
                if (ddlLOB.Items.Count > 0)
                {
                    ddlLOB.SelectedIndex = 0;
                    ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
                }
                // if (ddlCustomerCode.Items.Count > 0) ddlCustomerCode.SelectedIndex = 0;
                if (ddlBranch.SelectedValue != "0") ddlBranch.SelectedValue = "0";
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
                ddlBranch.ClearSelection();
                //ddlEnquiry.Clear();
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
                FunProClearCustomerDetails(false);
                ddlConstitition.Items.Clear();
                Clear();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriGetCustomerDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Option", ddlRefPoint.SelectedValue);
            dictParam.Add("@Ref_ID", ddlEnquiry.SelectedValue);
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtCustDetails = Utility.GetDefaultData("S3G_ORG_GetCustomerDetails", dictParam);

            DataRow dtRow = dtCustDetails.Rows[0];
            //If Customer Dropdown is Empty to Load Customer values Added By Shibu
            if (ddlCustomerCode.Items.Count <= 1)
            {
                ddlCustomerCode.Items.Clear();
                ddlCustomerCode.Items.Add(new ListItem("--Select--", "0"));
                ddlCustomerCode.Items.Add(new ListItem(dtRow["Customer_Code"].ToString(), dtRow["Customer_ID"].ToString()));
            }
            // End
            Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            dictParam1.Add("@Option", ddlRefPoint.SelectedValue);
            dictParam1.Add("@Ref_ID", ddlEnquiry.SelectedValue);
            dictParam1.Add("@Company_ID", intCompanyId.ToString());
            if (PRDDTransID == 0)
            {
                dictParam1.Add("@Is_Active", "1");
            }
            dictParam1.Add("@Program_Id", Program_Id);
            dictParam1.Add("@User_ID", intUserId.ToString());

            DataTable dtLBPDetails = Utility.GetDefaultData("S3G_ORG_GetLOBEnquiryNumber", dictParam1);
            DataRow dtRow1 = dtLBPDetails.Rows[0];

            ddlLOB.Items.Clear();
            ddlLOB.Items.Add(new ListItem(dtRow1["LOB_Name"].ToString(), dtRow1["LOB_ID"].ToString()));
            ddlLOB.SelectedValue = dtRow1["LOB_ID"].ToString();
            ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;

            FunPriLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
            ddlBranch.ClearSelection();
            //ddlBranch.Items.Add(new ListItem(dtRow1["Location"].ToString(), dtRow1["Location_ID"].ToString()));
            ddlBranch.SelectedValue = dtRow1["Location_ID"].ToString();
            ddlBranch.SelectedItem.Text = dtRow1["Location"].ToString();





            ddlConstitition.Items.Clear();
            ddlConstitition.Items.Add(new ListItem(dtRow1["Constitution_Name"].ToString(), dtRow1["Constitution_ID"].ToString()));
            ddlConstitition.SelectedValue = dtRow1["Constitution_ID"].ToString();
            if (ddlRefPoint.SelectedValue != "1")
            {
                if (Convert.ToUInt16(ddlConstitition.SelectedValue) > 0)
                    FunPriGetPRDDTransID();
            }
            fnBindCustomerDetails(dtRow, "Enquiry");



            if ((blnIsWorkflowApplicable && Session["DocumentNo"] != null) || ViewState["Mode"] == null)
            {
                txtStatus.Text = "Null";
            }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    /// <summary>
    /// This methode used in bind the customer details
    /// </summary>
    #region Bind CustomerDetails
    private void fnBindCustomerDetails(DataRow dtRow, string Type)
    {
        try
        {
            txtCustomerName.Text = dtRow["Title"].ToString() + " " + dtRow["Customer_Name"].ToString();
            txtCustomerID.Text = dtRow["Customer_ID"].ToString();
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtCustomerName.Text = txt.Text;
            txt.Text = dtRow["Customer_Code"].ToString();
            S3GCustomerCommAddress.SetCustomerDetails(dtRow, true);
            S3GCustomerPermAddress.SetCustomerDetails(dtRow, false);

            if (Type != "Customer")
            {
                txtProduct.Text = dtRow["Product_ID"].ToString();
                txtProductName.Text = dtRow["Product"].ToString();
                ddlCustomerCode.SelectedValue = dtRow["Customer_ID"].ToString();
                ((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value = dtRow["Customer_ID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }
    #endregion

    /// <summary>
    /// This methode used in SetCustomer Permanent Address in Textarea 
    /// </summary>
    #region SetCustomerPerAddress
    public string SetCustomerPerAddress(DataRow drCust)
    {
        string strAddress = "";
        try
        {
            if (drCust["Perm_Address1"].ToString() != "") strAddress += drCust["Perm_Address1"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_Address2"].ToString() != "") strAddress += drCust["Perm_Address2"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_City"].ToString() != "") strAddress += drCust["Perm_City"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_State"].ToString() != "") strAddress += drCust["Perm_State"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_Country"].ToString() != "") strAddress += drCust["Perm_Country"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_PINCode"].ToString() != "") strAddress += drCust["Perm_PINCode"].ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        return strAddress;
    }
    #endregion

    /// <summary>
    /// This methode used in Get PRDDTransaction details
    /// </summary>
    #region Get PRDDTrans
    private void FunGetPRDDTrans()
    {
        objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

        try
        {
            PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsRow ObjS3G_PRDDTransRowDetails;
            ObjS3G_ORG_PRDDCTransMasterDataTable = new PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable();
            ObjS3G_PRDDTransRowDetails = ObjS3G_ORG_PRDDCTransMasterDataTable.NewS3G_ORG_GetPRDDCTransDetailsRow();
            ObjS3G_PRDDTransRowDetails.PreDisbursement_Doc_Tran_ID = PRDDTransID;
            ObjS3G_PRDDTransRowDetails.Company_ID = intCompanyId;
            ObjS3G_ORG_PRDDCTransMasterDataTable.AddS3G_ORG_GetPRDDCTransDetailsRow(ObjS3G_PRDDTransRowDetails);

            byte[] bytePRDDTransDetails = objPRDDT_MasterClient.FunPubGetPRDDTrans(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCTransMasterDataTable, SerMode));
            ObjS3G_ORG_PRDDCTransMasterDataTable = (PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable)ClsPubSerialize.DeSerialize(bytePRDDTransDetails, SerMode, typeof(PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable));
            if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows.Count > 0)
            {
                txtPRDDC.Text = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["PRDDC_Number"].ToString();
                ddlRefPoint.Items.Add(new ListItem(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Ref_Name"].ToString(), ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString()));
                ddlRefPoint.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Ref_Name"].ToString();
                lblEnquiry.CssClass = "styleReqFieldLabel";
                ucCustomerCodeLov.SelectedText = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Customer_Name"].ToString();
                ucCustomerCodeLov.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Customer_ID"].ToString();
                hdnCustomerId.Value = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Customer_ID"].ToString();

                if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString() == "1")
                {
                    lblEnquiry.Text = "Enquiry No.";
                }
                else if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString() == "2")
                {
                    lblEnquiry.Text = "Checklist Number.";
                }
                else if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString() == "3")
                {
                    lblEnquiry.Text = "Application No.";
                }
                else
                {
                    lblEnquiry.Text = "Ref. Code";
                }
                ddlLOB.Items.Add(new ListItem(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["LOB"].ToString(), ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["LOB"].ToString();

                hidPRTID.Value = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["PRDDC_ID"].ToString();


                ddlBranch.Items.Add(new ListItem(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Location_Name"].ToString(), ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Location_ID"].ToString()));

                //ddlBranch.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Location_ID"].ToString();
                //ddlBranch.SelectedText = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Location_Name"].ToString();
                //ddlBranch.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Location_Name"].ToString();
                //ddlConstitition.Items.Clear();
                //FunPriLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
                ddlConstitition.Items.Add(new ListItem(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Constitution_Name"].ToString(), ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Constitution_ID"].ToString()));
                ddlConstitition.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Constitution_ID"].ToString();
                ddlConstitition.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Constitution_Name"].ToString();

                hdnofferdate.Value = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["OFFER_DATE"].ToString();

                //saranya
                //ddlRefPoint.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString();
                //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), PRDDTransID);
                //saranya

                //FunPriBindRefNo();
                //FunPriLoadRefPoint();

                txtProduct.Text = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Product_ID"].ToString();
                txtProductName.Text = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Product_Name"].ToString();

                ddlEnquiry.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type_ID"].ToString();
                ddlEnquiry.SelectedText = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Doc_Number"].ToString();
                //ddlEnquiry.Items.Add(new ListItem(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Ref_Name"].ToString(), ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString()));

                //ddlEnquiry.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type_ID"].ToString();
                //ddlEnquiry.SelectedText = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Doc_Number"].ToString();
                //ddlEnquiry.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Doc_Number"].ToString();
                //fnBindCustomerDetails(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0], "Customer");
                FunPriLoadAddressDets(Convert.ToInt32(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Customer_ID"].ToString()));
                //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), PRDDTransID);
                //ddlEnquiry.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type_ID"].ToString();

                //FunPriGetCustomerDetails(Convert.ToInt32(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Enquiry_Response_ID"].ToString()));
                // FunPriGetCustomerDetails();

                if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Status"].ToString() == "1")
                {
                    txtStatus.Text = "Null";
                }
                else if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Status"].ToString() == "2")
                {
                    txtStatus.Text = "Hold";
                }
                else
                {
                    txtStatus.Text = "Process";
                    btnSave.Enabled = false;
                    btnSave.CssClass = "css_btn_disabled";
                }
            }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objPRDDT_MasterClient.Close();
        }
    }

    /// <summary>
    /// This methode used in Get PRDDocument Transaction  details
    /// </summary>
    private void FunGetPRDDTransDocDetatils()
    {
        objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            //<<Performance>>
            //PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsRow ObjPRDDTransDocMasterRow;
            //ObjS3G_ORG_PRDDCTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable();
            //ObjPRDDTransDocMasterRow = ObjS3G_ORG_PRDDCTransDocMasterDataTable.NewS3G_ORG_GetPRDDCTransDocDetailsRow();
            //ObjPRDDTransDocMasterRow.PreDisbursement_Doc_Tran_ID = PRDDTransID;
            //ObjPRDDTransDocMasterRow.Company_ID = intCompanyId;
            //ObjPRDDTransDocMasterRow.Enquiry_Response_ID = Convert.ToInt32(ddlEnquiry.SelectedValue);
            //ObjPRDDTransDocMasterRow.Company_ID = intCompanyId;
            //ObjS3G_ORG_PRDDCTransDocMasterDataTable.AddS3G_ORG_GetPRDDCTransDocDetailsRow(ObjPRDDTransDocMasterRow);
            //byte[] bytePRDDTransDocDetails = objPRDDT_MasterClient.FunPubGetPRDDTransDoc(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCTransDocMasterDataTable, SerMode));
            //ObjS3G_ORG_PRDDCTransDocMasterDataTable = (PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable)ClsPubSerialize.DeSerialize(bytePRDDTransDocDetails, SerMode, typeof(PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable));

            //Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            ////dictParam1.Add("@Enquiry_Response_ID", Convert.ToString(ddlEnquiry.SelectedValue));
            //dictParam1.Add("@Company_ID", intCompanyId.ToString());
            //dictParam1.Add("@PreDisbursement_Doc_Tran_ID", PRDDTransID.ToString());
            //DataTable dtIsDetails = Utility.GetDefaultData("S3G_ORG_GetPRDDCTransDocIsDetails", dictParam1);
            //DataRow dtRow1 = dtIsDetails.Rows[0];

            //<<Performance>>
            Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            dictParam1.Add("@Company_ID", intCompanyId.ToString());
            dictParam1.Add("@PreDisbursement_Doc_Tran_ID", PRDDTransID.ToString());
            dictParam1.Add("@Document_Type_ID", ddlEnquiry.SelectedValue);
            DataSet Dset = Utility.GetDataset("S3G_ORG_GetPRDDCTransDocDetails", dictParam1);

            DataTable dtGrid = Dset.Tables[0];
            DataTable dtIsDetails = Dset.Tables[1];

            ViewState["dtIsDetails"] = dtIsDetails;
            string strPreDisDocId = string.Empty;
            if (dtGrid.Rows.Count > 0)
            {
                ViewState["Docpath"] = dtGrid.Rows[0]["ViewDoc"].ToString();
                //strPreDisDocId = Convert.ToString(dtGrid.Rows[0]["PREDISBURSE_DOC_TRAN_DET_ID"]);
            }
            gvPRDDT.DataSource = dtGrid;// Modify 
            gvPRDDT.DataBind();

            for (int i = 0; i < gvPRDDT.Rows.Count; i++)
            {
                if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                {


                    int PRDDC_Doc_Cat_ID = Convert.ToInt32(gvPRDDT.DataKeys[i]["PRDDC_Doc_Cat_ID"].ToString());
                    CheckBox Cbx1 = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");
                    //modified  and added by saranya for fixing observation on 01-Mar-2012
                    // DropDownList ddlScannedBy = (DropDownList)gvPRDDT.Rows[i].FindControl("ddlScannedBy");
                    DropDownList ddlCollAndWaiver = (DropDownList)gvPRDDT.Rows[i].FindControl("ddlCollAndWaiver");
                    UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[i].FindControl("ddlScannedBy");
                    TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                    Label lblScannedBy = (Label)gvPRDDT.Rows[i].FindControl("lblScannedBy");
                    TextBox txtCollectedDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtCollectedDate");

                    UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[i].FindControl("ddlCollectedBy");
                    TextBox txtcolldate = (TextBox)gvPRDDT.Rows[i].FindControl("txtCollectedDate");
                    Label lblCollectedBy = (Label)gvPRDDT.Rows[i].FindControl("lblCollectedBy");
                    //end 

                    TextBox txtScanRef = (TextBox)gvPRDDT.Rows[i].FindControl("txtScan");
                    ImageButton Viewdoct = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                    TextBox txtRemarks = (TextBox)gvPRDDT.Rows[i].FindControl("txtRemarks");
                    Label lblDesc = (Label)gvPRDDT.Rows[i].FindControl("lblDesc");
                    TextBox txOD = (TextBox)gvPRDDT.Rows[i].FindControl("txOD");
                    TextBox txtScan = (TextBox)gvPRDDT.Rows[i].FindControl("txtScan");
                    Label lblColUser = (Label)gvPRDDT.Rows[i].FindControl("lblColUser");
                    Label lblPath = (Label)gvPRDDT.Rows[i].FindControl("lblPath");
                    Label lblPRDDTrans = (Label)gvPRDDT.Rows[i].FindControl("lblPRDDTrans");
                    Label myThrobber = (Label)gvPRDDT.Rows[i].FindControl("myThrobber");
                    Label lblReceivedStatus = (Label)gvPRDDT.Rows[i].FindControl("lblReceivedStatus");
                    Label lblChecklistFlag = (Label)gvPRDDT.Rows[i].FindControl("lblChecklistFlag");
                    CheckBox CbxCheck = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");
                    HiddenField hidThrobber = (HiddenField)gvPRDDT.Rows[i].FindControl("hidThrobber");

                    ddlCollectedBy.Enabled = false;

                    if (lblPath.Text != " ")
                    {
                        Viewdoct.CssClass = "styleGridEdit";
                    }

                    //DataView dv = new DataView(Dset.Tables[2], "PREDISBURSE_DOC_TRAN_DET_ID=" + strPreDisDocId, "", DataViewRowState.CurrentRows);

                    //if (dv.Count == 0)
                    //{
                    //    Viewdoct.Enabled = false;
                    //}
                    //if (!string.IsNullOrEmpty(lblPath.Text.Trim()))
                    //{
                    //    if (lblPath.Text.Trim() == ViewState["Docpath"].ToString().Trim())
                    //    {
                    //        Viewdoct.Enabled = false;
                    //        Viewdoct.CssClass = "styleGridEditDisabled";
                    //    }
                    //    else
                    //    {
                    //        Viewdoct.CssClass = "styleGridEdit";
                    //    }
                    //}
                    //else
                    //{
                    //    Viewdoct.Enabled = false;
                    //    Viewdoct.CssClass = "styleGridEditDisabled";
                    //}

                    //AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");

                    FileUpload asyFileUpload = (FileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");

                    Label lblType = (Label)gvPRDDT.Rows[i].FindControl("lblType");
                    //asyFileUpload.Enabled = false;
                    Cbx1.Checked = false;
                    //if (Convert.ToInt32(dtGrid.Rows[i]["PRDDTrans"].ToString()) == 1)
                    //{

                    //}
                    if (PRDDC_Doc_Cat_ID == Convert.ToInt32(dtGrid.Rows[i]["PRDDC_Doc_Cat_ID"].ToString()))
                    {
                        //Commented and added by saranya
                        //txtScanBy.Text = Convert.ToString(ObjS3G_ORG_PRDDCTransDocMasterDataTable.Rows[i]["Scandedby"]);
                        if ((dtGrid.Rows[i]["Scanned_Date"].ToString()) != string.Empty)
                        {
                            txtScanDate.Text = Utility.StringToDate(dtGrid.Rows[i]["Scanned_Date"].ToString()).ToString(strDateFormat);
                            //Convert.ToDateTime(dtGrid.Rows[i]["Scanned_Date"].ToString()).ToString(strDateFormat);
                        }
                        else
                        {
                            txtScanDate.Text = "";
                        }

                        if ((Convert.ToString(dtGrid.Rows[i]["Scandedby"])) != string.Empty)
                        {
                            ddlScannedBy.SelectedValue = Convert.ToString(dtGrid.Rows[i]["Scanned_By"]);
                            //ddlScannedBy.ClearDropDownList();
                            lblScannedBy.Text = Convert.ToString(dtGrid.Rows[i]["Scanned_By"]);
                        }
                        else
                        {
                            ddlScannedBy.SelectedValue = "0";
                            lblScannedBy.Text = "";
                        }


                        if ((dtGrid.Rows[i]["Collected_Date"].ToString()) != string.Empty)
                        {
                            txtcolldate.Text = Utility.StringToDate(dtGrid.Rows[i]["Collected_Date"].ToString()).ToString(strDateFormat);
                            //txtcolldate.Text = Convert.ToDateTime(dtGrid.Rows[i]["Collected_Date"].ToString()).ToString(strDateFormat);
                        }
                        else
                        {
                            txtcolldate.Text = "";
                        }
                        if ((Convert.ToString(dtGrid.Rows[i]["CollectedBy"])) != string.Empty)
                        {
                            ddlCollectedBy.SelectedValue = Convert.ToString(dtGrid.Rows[i]["Collected_By"]);
                            //ddlCollectedBy.ClearDropDownList();
                            lblCollectedBy.Text = Convert.ToString(dtGrid.Rows[i]["Collected_By"]);
                        }
                        else
                        {
                            ddlCollectedBy.SelectedValue = "0";
                            lblCollectedBy.Text = "";
                            ddlCollectedBy.SelectedText = ObjUserInfo.ProUserNameRW;

                            ddlCollectedBy.SelectedValue = ObjUserInfo.ProUserIdRW.ToString();
                        }
                        //end
                        maxversion = Convert.ToInt32(dtGrid.Rows[i]["Version_No"]);


                        if (Convert.ToBoolean(dtGrid.Rows[i]["PRDDTrans"]) == true)
                        {
                            Cbx1.Checked = true;
                            Cbx1.Enabled = false;
                            //txtcollBy.Text = Convert.ToString(ObjS3G_ORG_PRDDCTransDocMasterDataTable.Rows[i]["CollectedBy"]);

                        }

                        //else
                        //{
                        //    txtcollBy.Text = ObjUserInfo.ProUserNameRW;
                        //    lblColUser.Text = "";
                        //}

                        MaxVerChk.Value += maxversion + "@@" + chkbox;

                        if (txtcolldate.Text == "")
                        {
                            txtcolldate.Text = "";
                            Cbx1.Checked = false;
                        }
                        MaxVerChk.Value += "@@" + txtScanDate.Text;
                        MaxVerChk.Value += "@@" + txtcolldate.Text;
                        txtScanRef.Text = Convert.ToString(dtGrid.Rows[i]["Scanned_Ref_No"]);
                        txtRemarks.Text = Convert.ToString(dtGrid.Rows[i]["Remarks"]);
                        MaxVerChk.Value += "@@" + txtRemarks.Text;

                        if (lblPath.Text.Trim() != ViewState["Docpath"].ToString().Trim())
                        {
                            string[] Path = Convert.ToString(dtGrid.Rows[i]["Scanned_Ref_No"]).Split('/');
                            //hidThrobber.Value = Path[Path.Length - 1].ToString();
                            //myThrobber.Text = Path[Path.Length - 1].ToString();
                            hdnFilePath.Value = Path[Path.Length - 1].ToString();
                            txOD.Text = Convert.ToString(dtGrid.Rows[i]["Scanned_Ref_No"]);
                        }
                        //else
                        //{
                        //    txtScanBy.Text = ObjUserInfo.ProUserNameRW;                            
                        //}
                    }
                    //if (lblChecklistFlag.Text == "1" && lblReceivedStatus.Text != "0")
                    if (lblChecklistFlag.Text == "1" && (lblReceivedStatus.Text != "0" && lblReceivedStatus.Text != "2" && lblReceivedStatus.Text != "NC"))
                    {
                        ddlCollAndWaiver.Enabled = false;
                        ddlCollectedBy.Enabled = false;
                        txtCollectedDate.Enabled = false;
                        //ddlScannedBy.Enabled = false;
                        //txtScanDate.Enabled = false;
                        //if (strMode != "C")
                        //{
                        //    asyFileUpload.Enabled = false;
                        //}
                        //txtRemarks.Enabled = false;
                        //CbxCheck.Enabled = false;

                        //ddlCollAndWaiver.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));

                        if (lblReceivedStatus.Text == "1")
                        {
                            lblReceivedStatus.Text = "C";
                            ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                            ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                        }
                        else if (lblReceivedStatus.Text == "3")
                        {
                            lblReceivedStatus.Text = "W";
                            ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                            ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                        }
                        else if (lblReceivedStatus.Text == "2")
                        {
                            lblReceivedStatus.Text = "NC";
                            ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                            ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                        }
                        else if (lblReceivedStatus.Text == "4")
                        {
                            lblReceivedStatus.Text = "NA";
                            ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                            ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                        }
                    }
                    // New
                    if (lblChecklistFlag.Text == "1" && (lblReceivedStatus.Text != "0" && lblReceivedStatus.Text != "2" && lblReceivedStatus.Text != "NC"))
                    {
                        if (lblPRDDTrans.Text == "1")
                        {
                            CbxCheck.Enabled = false;
                            ddlCollAndWaiver.Enabled = false;
                        }
                    }
                    else
                    {
                        ddlCollAndWaiver.Enabled = true;
                        CbxCheck.Enabled = true;
                    }
                    if (Convert.ToBoolean(dtGrid.Rows[i]["PRDDTrans"]) == true)
                    {
                        Cbx1.Checked = true;
                        Cbx1.Enabled = false;
                        //txtcollBy.Text = Convert.ToString(ObjS3G_ORG_PRDDCTransDocMasterDataTable.Rows[i]["CollectedBy"]);

                    }
                    // New
                    if ((dtIsDetails.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "false")
                        || (dtIsDetails.Rows[i]["Is_NeedScanCopy"].ToString() == "0") && (lblPath.Text != string.Empty))
                    // && dtIsDetails.Rows[i]["Is_Mandatory"].ToString() == "False")
                    {
                        //txtScanDate.Enabled = txtScanBy.Enabled = false;
                        //txtScanDate.Text = "";
                        //txtScanBy.Text = "";
                        //ddlScannedBy.Visible = txtScanDate.Visible = false;
                        //Viewdoct.Enabled = false;
                        //Viewdoct.CssClass = "styleGridEditDisabled";
                        //if (strMode != "C")
                        //{
                        //    asyFileUpload.Enabled = false;
                        //}
                        myThrobber.Text = "";
                        //Cbx1.Enabled = false;
                    }
                    //if (strMode == "M")
                    //{
                    //    if (lblReceivedStatus.Text == "1" || lblReceivedStatus.Text == "C")
                    //    {
                    //        asyFileUpload.Visible = false;
                    //    }
                    //}

                    MaxVerChk.Value += "~~~";
                }
            }
        }
        //catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{
        //    lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        //}
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //if (objPRDDT_MasterClient != null)
            //objPRDDT_MasterClient.Close();
        }
    }
    #endregion

    protected void hyplnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvPRDDT", strFieldAtt);
            Label lblPath = (Label)gvPRDDT.Rows[gRowIndex].FindControl("lblPath");
            HiddenField hdnFilePath = (HiddenField)gvPRDDT.Rows[gRowIndex].FindControl("hdnFilePath");
            if (lblPath.Text.Trim() != ViewState["Docpath"].ToString().Trim())
            {
                string strFileName = hdnFilePath.Value.Replace("\\", "/").Trim();
                string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "File not found");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }


    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }

    /// <summary>
    /// This methode used in Get XML form for PRDDT
    /// </summary>
    #region Save/ Clear/ Cancel
    protected string FunProFormMLAXML()
    {
        int enq = 1;

        string[] temprow = null;
        int Counts = 0;
        int rowcount = 0;
        int versionchk = 0;
        string strVersionNo = "1";
        StringBuilder strbuXML = new StringBuilder();
        if (!string.IsNullOrEmpty(MaxVerChk.Value))
        {
            temprow = (MaxVerChk.Value).Split('~', '~', '~');
            strVersionNo = Convert.ToString(temprow[0]).Substring(0, 1);
        }
        strbuXML.Append("<Root>");
        foreach (GridViewRow grvData in gvPRDDT.Rows)
        {
            int fileIndex = 1;

            string strlblPRTID = ((Label)grvData.FindControl("lblPRTID")).Text;
            string strScanBy = "";//Convert.ToString(intUserId);
            string strCollectdBy = "";
            //Modified by saranya
            //if(((Label)grvData.FindControl("lblColUser")).Text!="")
            //    strCollectdBy = ((Label)grvData.FindControl("lblColUser")).Text;
            //else
            //    strCollectdBy = Convert.ToString(intUserId);


            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion
            //if (((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedIndex > 0)

            //    strCollectdBy = ((Label)grvData.FindControl("lblCollectedBy")).Text;

            //else
            //    strCollectdBy = ((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedValue;

            //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
            if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue != "0")
            {
                // if (((Label)grvData.FindControl("lblCollectedBy")).Text != "")
                // {
                // Modified By Mani
                // strCollectdBy = ((Label)grvData.FindControl("lblCollectedBy")).Text;
                strCollectdBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
                // }
                // else
                // {
                //  strCollectdBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
                //}
            }

            //else
            //{
            //    strCollectdBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
            //}

            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion
            //if (((Label)grvData.FindControl("lblScanUser")).Text != "")
            //    strScanBy = ((Label)grvData.FindControl("lblScanUser")).Text;
            //else
            //    strScanBy = Convert.ToString(intUserId);    
            //if (((DropDownList)grvData.FindControl("ddlScannedBy")).Visible)
            //{
            //    if (((DropDownList)grvData.FindControl("ddlScannedBy")).SelectedIndex > 0)
            //        strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
            //    else
            //        strScanBy = ((DropDownList)grvData.FindControl("ddlScannedBy")).SelectedValue;
            //}
            //else
            //{
            //    strScanBy = "-1";
            //}
            //Changed By Shibu 19-Sep-2013 to Add Auto Suggestion
            if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).Visible)
            {
                //  if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).SelectedValue != "0")
                //   strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
                //else
                strScanBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).SelectedValue;
            }
            // else
            //{
            //    strScanBy = "0";
            //}


            string strCollectdDate = ((TextBox)grvData.FindControl("txtCollectedDate")).Text;
            string strScanDate = "";
            if (((TextBox)grvData.FindControl("txtScannedDate")).Visible)
            {
                strScanDate = ((TextBox)grvData.FindControl("txtScannedDate")).Text;
            }
            //End here

            CheckBox CbxCheck = (CheckBox)grvData.FindControl("CbxCheck");
            //AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)grvData.FindControl("asyFileUpload");
            FileUpload asyFileUpload = (FileUpload)grvData.FindControl("asyFileUpload");
            //if (PRDDTransID != 0)
            //{
            //    if (asyFileUpload.FileName.ToString() != "")
            //    {
            //        //strScanBy = Convert.ToString(intUserId);
            //        strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
            //        //strScanDate = DateTime.Now.ToString(strDateFormat).ToString();
            //        strScanDate = ((TextBox)grvData.FindControl("txtScannedDate")).Text;
            //    }
            //}
            string sCollectedOrWaived = ((DropDownList)grvData.FindControl("ddlCollAndWaiver")).SelectedValue;


            string strScanRefNo = ((HiddenField)grvData.FindControl("hdnFilePath")).Value; //((TextBox)grvData.FindControl("txOD")).Text;
            string strRemarks = ((TextBox)grvData.FindControl("txtRemarks")).Text.Replace("'", "\"").Replace(">", "").Replace("<", "").Replace("&", "");
            string strPRDDTrans = Convert.ToString(((CheckBox)grvData.FindControl("CbxCheck")).Checked);
            string[] temp;
            if (!string.IsNullOrEmpty(MaxVerChk.Value))
            {
                temp = Convert.ToString(temprow[rowcount]).Split('@', '@');
                if (temp.Length > 1)
                {
                    maxversion = Convert.ToInt32(temp[0]);
                    chkbox = Convert.ToBoolean(temp[2]);

                    if (strPRDDTrans.ToLower() == Convert.ToString(chkbox).ToLower())
                    {
                        //{ strVersionNo = Convert.ToString(maxversion);
                        Counts = 0;
                    }
                    else
                    {

                        versionchk = versionchk + 1;
                        Counts = 1;
                        bMod = true;
                    }
                    if (temp[4].ToString() == strScanDate.ToString() && temp[6].ToString() == strCollectdDate.ToString() && temp[8].ToString() == strRemarks.ToString())
                    {
                        //Counts = 0;
                    }
                    else
                    {
                        //Counts = 1;
                        bMod = true;
                    }
                    if (versionchk > 0)
                    {
                        maxversion = maxversion + 1;
                        strVersionNo = Convert.ToString(maxversion);
                    }
                }
            }
            if (strCollectdBy.Trim() == "")
            {
                strCollectdBy = "0";
            }
            else
            {
                strCollectdBy = strCollectdBy.Trim();
            }
            if (strScanBy.Trim() == "")
            {
                strScanBy = "0";
            }
            else
            {
                strScanBy = strScanBy.Trim();
            }
            strScanDate = strScanDate == string.Empty ? strScanDate : Utility.StringToDate(strScanDate).ToString();
            strCollectdDate = strCollectdDate == string.Empty ? strCollectdDate : Utility.StringToDate(strCollectdDate).ToString();//Utility.StringToDate(DateTime.Now.ToString(strDateFormat)).ToString()
            strbuXML.Append(" <Details  PRDDC_Doc_Cat_ID='" + strlblPRTID + "' Collected_By='" + strCollectdBy.ToString() + "' Collected_Date='" + strCollectdDate +
                "' Scanned_By='" + strScanBy.ToString() + "' Scanned_Date='" + strScanDate + "' Scanned_Ref_No='" + strScanRefNo.ToString() + "' Remarks='" + strRemarks.ToString() + "' PRDDTrans='" + strPRDDTrans.ToString() + "' Version_No='" + "' Counts='" + Counts.ToString() + "'  Doc_CollectOrWaived='" + sCollectedOrWaived + "'/>"); // Added By Shibu to Update Collected / Waived Values 6-Jun-2013
            rowcount = rowcount + 3;

        }
        string tem = "Version_No='" + strVersionNo + "'";
        strbuXML.Replace("Version_No=''", tem);
        strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }
    #endregion

    /// <summary>
    /// This methode used in Insert and Update for PRDDT details
    /// </summary>
    #region Save and Update PRDDT
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string strPRDDT_No = string.Empty;
        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRT';";
        string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgPDDTMaster_Add.aspx';";

        objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

        try
        {
            if (ddlEnquiry.SelectedValue == string.Empty || ddlEnquiry.SelectedValue == "0" || ddlEnquiry.SelectedText == string.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, "Select Ref. Code");
                return;
            }
            DataTable dt1 = new DataTable();
            PRDDTransID = Convert.ToInt32(ViewState["PRDDTransID"]);
            if (PRDDTransID > 0)
            {
                dt1 = (DataTable)ViewState["dtIsDetails"];
            }
            else
            {
                dt1 = (DataTable)ViewState["dtPRDDTypeTrans"];
                if ((DataTable)ViewState["dtPRDDTypeTrans"] == null)
                {
                    lblErrorMessage.Text = "";
                    Utility.FunShowAlertMsg(this, "Document details not defined in Pre Disbursements Master");
                    strRedirectPageView = strRedirectPageAdd;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                    return;
                }
            }

            int counts = 0;
            int Length = gvPRDDT.Rows.Count;

            for (int i = 0; i < gvPRDDT.Rows.Count; i++)
            {
                if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                {

                    TextBox txtCollectedDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtCollectedDate");
                    TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                    ImageButton hyplnkView = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                    FileUpload asyFileUpload = (FileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");
                    //AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");
                    CheckBox CbxCheck = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");
                    Label lblType = (Label)gvPRDDT.Rows[i].FindControl("lblType");
                    Label lblProgramName = (Label)gvPRDDT.Rows[i].FindControl("lblProgramName");
                    HiddenField hidThrobber = (HiddenField)gvPRDDT.Rows[i].FindControl("hidThrobber");
                    Label myThrobber = (Label)gvPRDDT.Rows[i].FindControl("myThrobber");
                    TextBox txtRemarks = (TextBox)gvPRDDT.Rows[i].FindControl("txtRemarks");
                    Label lblNeedScanCopy = (Label)gvPRDDT.Rows[i].FindControl("lblNeedScanCopy");
                    TextBox txtScan = (TextBox)gvPRDDT.Rows[i].FindControl("txtScan");
                    CheckBox chkOptMan = (CheckBox)gvPRDDT.Rows[i].FindControl("chkOptMan");
                    Label lblPathUploadlabel = (Label)gvPRDDT.Rows[i].FindControl("lblPathUploadlabel");
                    Label lblActualPathI = (Label)gvPRDDT.Rows[i].FindControl("lblActualPathI");
                    HiddenField hdnFilePath = (HiddenField)gvPRDDT.Rows[i].FindControl("hdnFilePath");

                    //asyFilepath.Text = asyFileUpload.FileName;
                    //Added by Senthil on Apr 16th 2012 Begin

                    DropDownList ddlCollAndWaiver = (DropDownList)gvPRDDT.Rows[i].FindControl("ddlCollAndWaiver");

                    if (CbxCheck.Checked)
                    {
                        //if (lblProgramName.Text.Trim() == "PRDDT")
                        //{
                        //DropDownList ddlCollectedBy = gvPRDDT.Rows[i].FindControl("ddlCollectedBy") as DropDownList;
                        //DropDownList ddlScannedBy = gvPRDDT.Rows[i].FindControl("ddlScannedBy") as DropDownList;
                        UserControls_S3GAutoSuggest ddlCollectedBy = gvPRDDT.Rows[i].FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;
                        UserControls_S3GAutoSuggest ddlScannedBy = gvPRDDT.Rows[i].FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;

                        if (chkOptMan.Checked == true)
                        {
                            if (ddlCollAndWaiver.SelectedValue == "0" || ddlCollAndWaiver.SelectedValue == " " || ddlCollAndWaiver.SelectedValue == "")
                            {
                                //Utility.FunShowAlertMsg(this, " Select the Document Waived/Received for " + lblProgramName.Text + "  ");
                                Utility.FunShowAlertMsg(this, " Select the Document Waived / Collected for Doc. Position");
                                return;
                            }
                            if (!(ddlCollAndWaiver.SelectedValue == "NC" || ddlCollAndWaiver.SelectedValue == "NA"))
                            {
                                if (ddlCollectedBy.SelectedValue == "0" || ddlCollectedBy.SelectedValue == " ")
                                {
                                    //ddlCollAndWaiver.Focus();
                                    //Utility.FunShowAlertMsg(this, "Select Collected / Waived By");
                                    //return;
                                }
                            }
                            if (!(ddlCollAndWaiver.SelectedValue == "NC" || ddlCollAndWaiver.SelectedValue == "NA"))
                            {
                                if (ddlCollectedBy.SelectedValue == "0" || ddlCollectedBy.SelectedText == string.Empty)
                                {
                                    //ddlCollAndWaiver.Focus();
                                    //Utility.FunShowAlertMsg(this, " Select Collected / Waived By");
                                    //return;
                                }
                            }
                        }
                        if (hdnofferdate.Value != string.Empty)
                        {
                            if (txtCollectedDate.Text != string.Empty)
                            {
                                if (Utility.StringToDate(hdnofferdate.Value) > Utility.StringToDate(txtCollectedDate.Text))
                                {
                                    Utility.FunShowAlertMsg(this, "Collected / Waived Date should not be less than the Check List Offer Date");
                                    return;
                                }
                            }
                            if (txtScanDate.Text != string.Empty)
                            {
                                if (Utility.StringToDate(hdnofferdate.Value) > Utility.StringToDate(txtScanDate.Text))
                                {
                                    Utility.FunShowAlertMsg(this, "Scanned Date should not be less than the Check List Offer Date");
                                    return;
                                }
                            }
                        }
                        if (chkOptMan.Checked == true)
                        {
                            //if (asyFileUpload.Enabled && (ddlScannedBy.SelectedValue == "0" || ddlScannedBy.SelectedValue == ""))
                            if ((ddlScannedBy.SelectedValue == "0" || ddlScannedBy.SelectedValue == " "))
                            {
                                if ((ddlCollAndWaiver.SelectedValue == "C" || ddlCollAndWaiver.SelectedValue == "1") && lblNeedScanCopy.Text == "1") // Added By Shibu 6-Jun-2013 Condition only for Collected
                                {
                                    ddlScannedBy.Focus();
                                    Utility.FunShowAlertMsg(this, " Select Scanned By");
                                    return;
                                }
                            }
                            if (lblNeedScanCopy.Text == "1" && asyFileUpload.FileName == string.Empty)
                            {
                                if (hdnFilePath.Value == string.Empty || hdnFilePath.Value == " ")
                                {
                                    if (ddlCollAndWaiver.SelectedValue == "C")
                                    {
                                        asyFileUpload.Focus();
                                        Utility.FunShowAlertMsg(this, "Scanned has to be collected");
                                        return;
                                    }
                                }
                            }

                            if (!(ddlCollAndWaiver.SelectedValue == "NC" || ddlCollAndWaiver.SelectedValue == "NA") && ((ddlCollAndWaiver.SelectedValue == "C" || ddlCollAndWaiver.SelectedValue == "1") || ddlCollAndWaiver.SelectedValue == "W" || ddlCollAndWaiver.SelectedValue == "3"))
                            {
                                if (txtRemarks.Text == string.Empty || txtRemarks.Text == "" || txtRemarks.Text == " ")
                                {
                                    txtRemarks.Focus();
                                    Utility.FunShowAlertMsg(this, "Enter Remarks for all Collected / Waived");
                                    return;
                                }
                            }
                        }
                        //}


                    }

                    //if (lblProgramName.Text.Trim() != "")
                    //{
                    if ((dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "true" ||
                            dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "1")
                        && CbxCheck.Checked == false)
                    {
                        if (ddlCollAndWaiver.SelectedValue == "C" && asyFileUpload.FileName == string.Empty && CbxCheck.Checked == true)
                        {
                            Utility.FunShowAlertMsg(this, "Pre Disbursements Document has to be collected for document type - " + lblType.Text.Trim().ToUpper());
                            return;
                        }
                    }
                    if ((dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "true"
                        && dt1.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "true") ||

                        (dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "1"
                        && dt1.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "1"))
                    {
                        //if (lblPathUploadlabel.Text.Trim() == "")
                        //{
                        //    if (ddlCollAndWaiver.SelectedValue == "C" && lblPathUploadlabel.Text == string.Empty && CbxCheck.Checked == true)
                        //    {
                        //        Utility.FunShowAlertMsg(this, "Pre Disbursements Document has to be scanned for document type - " + lblType.Text.Trim().ToUpper());
                        //        return;
                        //    }
                        //}
                    }
                    //}

                    if (hidThrobber.Value.Trim() != "")
                    {
                        string fileExtension = asyFileUpload.FileName.Substring(asyFileUpload.FileName.LastIndexOf('.') + 1);
                        if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                        {
                            cvPRDTT.ErrorMessage = "File extension not supported, only image & pdf files should be uploaded.";
                            cvPRDTT.IsValid = false;
                            return;
                        }
                    }

                    // Modified By Senthilkumar P , 24/Mar/2012
                    // Reg. Document status not selected in Pre. Dis. Documents.

                    // Validation Begin

                    if (((dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "false"
                            && dt1.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "false") ||

                            (dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "0"
                            && dt1.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "0"))
                        && (!(CbxCheck.Checked)))
                        counts++;

                    // Validation End...

                    if (CbxCheck.Checked) counts++;
                }
            }

            if (Length == counts)
                txtStatus.Text = "Process";
            else if (counts < Length)
                txtStatus.Text = "Hold";
            else
                txtStatus.Text = "Null";

            //int intRowindex = 0;
            //foreach (GridViewRow grv in gvPRDDT.Rows)
            //{
            //    TextBox txOD = grv.FindControl("txOD") as TextBox;
            //    //txOD.Text = ViewState["Docpath"].ToString().Trim();
            //    //Commented and added by saranya
            //    //Label txtCollected = grv.FindControl("txtColletedDate") as Label;
            //    //Label txtCollectedBy = grv.FindControl("txtColletedBy") as Label;
            //    //Label txtScaned = grv.FindControl("txtScannedDate") as Label;
            //    //Label txtScanedBy = grv.FindControl("txtScannedBy") as Label;
            //    //TextBox txtScan = grv.FindControl("txtScan") as TextBox;
            //    TextBox txtCollected = grv.FindControl("txtColletedDate") as TextBox;
            //    // DropDownList ddlCollectedBy = grv.FindControl("ddlCollectedBy") as DropDownList;
            //    UserControls_S3GAutoSuggest ddlCollectedBy = grv.FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;

            //    Label lblCollectedBy = grv.FindControl("lblCollectedBy") as Label;
            //    TextBox txtScaned = grv.FindControl("txtScannedDate") as TextBox;

            //    // DropDownList ddlScannedBy = grv.FindControl("ddlScannedBy") as DropDownList;
            //    UserControls_S3GAutoSuggest ddlScannedBy = grv.FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;

            //    Label lblScannedBy = grv.FindControl("lblScannedBy") as Label;
            //    TextBox txtScan = grv.FindControl("txtScan") as TextBox;
            //    //End Here
            //    TextBox txtRemark = grv.FindControl("txtRemarks") as TextBox;
            //    ImageButton HypLnk = (ImageButton)grv.FindControl("hyplnkView");
            //    HiddenField hidThrobber = (HiddenField)grv.FindControl("hidThrobber");
            //    DropDownList ddlCollAndWaiver = (DropDownList)grv.FindControl("ddlCollAndWaiver"); // Added by Shibu 4-Jun-2013
            //    //AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");

            //    FileUpload AsyncFileUpload1 = (FileUpload)grv.FindControl("asyFileUpload");

            //    string strPath = "";
            //    string strNewFileName = AsyncFileUpload1.FileName;
            //    string strEnqNumber = ddlEnquiry.SelectedText.Replace("/", "-");

            //    //if (AsyncFileUpload1.FileName != "" && hidThrobber.Value.Trim() != "")
            //    //{
            //    //if (AsyncFileUpload1.HasFile)
            //    //{
            //    if (ViewState["Docpath"].ToString() != "")
            //    {
            //        if (ViewState["Docpath"].ToString() != "")
            //        {
            //            strPath = Path.Combine(ViewState["Docpath"].ToString(), "COMPANY" + intCompanyId.ToString() + "/" + strEnqNumber + "/" + "PDDTC-" + intRowindex.ToString());
            //            if (Directory.Exists(strPath))
            //            {
            //                Directory.Delete(strPath, true);
            //            }
            //            Directory.CreateDirectory(strPath);
            //            strPath = strPath + "/" + strNewFileName;
            //        }
            //        txOD.Text = strPath;// txOD.Text + strEnqNumber + "\\" + "PDDTC-" + intRowindex.ToString() + "\\" + strNewFileName;

            //        FileInfo f1 = new FileInfo(strPath);

            //        if (f1.Exists == true)
            //            f1.Delete();

            //        AsyncFileUpload1.SaveAs(strPath);
            //    }
            //    //}
            //    //}

            //    //if (((CheckBox)grv.FindControl("CbxCheck")).Checked)
            //    //{                    
            //    //    if (txtRemark.Text.Length == 0)
            //    //    {
            //    //        cvPRDTT.ErrorMessage = "Enter the Remarks";                        
            //    //        cvPRDTT.IsValid = false;
            //    //        return;
            //    //    }
            //    //}
            //    intRowindex++;
            //}

            ObjS3G_PRDDTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterRow objPRDDTransDocRow;
            ObjS3G_PRDDTransMasterDataTable = new PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PPDDTransMasterRow objPRDDTransRow;
            objPRDDTransRow = ObjS3G_PRDDTransMasterDataTable.NewS3G_ORG_PPDDTransMasterRow();
            objPRDDTransRow.Company_ID = intCompanyId;
            PRDDTransID = Convert.ToInt32(ViewState["PRDDTransID"]);
            objPRDDTransRow.PreDisbursement_Doc_Tran_ID = PRDDTransID;
            objPRDDTransRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objPRDDTransRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objPRDDTransRow.Constitution_ID = Convert.ToInt32(ddlConstitition.SelectedValue);
            objPRDDTransRow.Document_Type = Convert.ToInt32(ddlRefPoint.SelectedValue);
            objPRDDTransRow.Enquiry_Response_ID = Convert.ToInt32(ddlEnquiry.SelectedValue);
            objPRDDTransRow.PRDDC_ID = Convert.ToInt32(hidPRTID.Value);
            //objPRDDTransRow.Customer_ID = Convert.ToInt64(txtCustomerID.Text);
            //if (((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value != null && ((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value != "" && ((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value != "0")
            //{
            //    objPRDDTransRow.Customer_ID =  Convert.ToInt32(hdnCustomerId.Value);
            //}
            objPRDDTransRow.Customer_ID = Convert.ToInt32(hdnCustomerId.Value);
            if (PRDDTransID == 0)
            {
                objPRDDTransRow.PRDDC_Date = Utility.StringToDate(txtDate.Text);
            }
            else
            {
                objPRDDTransRow.PRDDC_Date = DateTime.Now;
            }

            if (txtStatus.Text == "Null")
            {
                objPRDDTransRow.Status = "1";
            }
            else if (txtStatus.Text == "Hold")
            {
                objPRDDTransRow.Status = "2";
            }
            else
            {
                objPRDDTransRow.Status = "3";
            }

            objPRDDTransRow.Created_By = intUserId;
            objPRDDTransRow.Created_On = DateTime.Now;
            objPRDDTransRow.Modified_By = intUserId;
            objPRDDTransRow.Modified_On = DateTime.Now;
            if (PRDDTransID == 0)
            {
                objPRDDTransRow.PRDDC_Number = "0";
            }
            else
            {
                objPRDDTransRow.PRDDC_Number = txtPRDDC.Text;
            }
            objPRDDTransRow.XML_PRDDTDeltails = FunProFormMLAXML();
            ObjS3G_PRDDTransMasterDataTable.AddS3G_ORG_PPDDTransMasterRow(objPRDDTransRow);
            byte[] ObjPRDDTDataTable = ClsPubSerialize.Serialize(ObjS3G_PRDDTransMasterDataTable, SerMode);

            if (PRDDTransID > 0)
            {
                intErrCode = objPRDDT_MasterClient.FunPubModifyPRDDTransMaster(SerMode, ObjPRDDTDataTable);
            }
            else
            {
                intErrCode = objPRDDT_MasterClient.FunPubCreatePRDDTransMaster(out strPRDDT_No, SerMode, ObjPRDDTDataTable);

            }

            if (intErrCode == 0)
            {
                btnSave.Enabled = false;
                btnSave.CssClass = "css_btn_disabled";
                if (PRDDTransID > 0)
                {
                    strPRDDT_No = txtPRDDC.Text;
                    //strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Pre Disbursements Document details updated successfully");
                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler)                       
                    {
                        try
                        {
                            WorkFlowSession WFValues = new WorkFlowSession();
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, int.Parse(ddlRefPoint.SelectedValue));
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow not Assigned";
                            }
                            ShowWFAlertMessage(strPRDDT_No, ProgramCode, strAlert);
                            return;
                        }
                        catch
                        {
                            strAlert = "Pre Disbursements Document Transaction details updated successfull";

                        }
                    }
                }
                else
                {
                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler)                       
                    {
                        try
                        {
                            WorkFlowSession WFValues = new WorkFlowSession();
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, int.Parse(ddlRefPoint.SelectedValue));

                                strAlert = "";

                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow not Assigned";
                            }
                            ShowWFAlertMessage(strPRDDT_No, ProgramCode, strAlert);
                            return;
                        }
                        catch
                        {
                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here

                            strAlert = "Pre Disbursements Document Transaction " + strPRDDT_No + " details added successfully";
                            strAlert += @"\n\n And Job not assigned to the next user.";
                            strAlert += @"\n\nWould you like to add one more record?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            ddlLOB.Focus();
                        }
                    }
                    else
                    {
                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(null, ddlEnquiry.SelectedText.Trim(), ProgramCode, null, null, "O", CompanyId, int.Parse(ddlLOB.SelectedValue), null, null, txtProductName.Text.Trim(), out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), ddlEnquiry.SelectedText.Trim(), int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), int.Parse(ddlRefPoint.SelectedValue));
                        }

                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strAlert = "Pre Disbursements Document Transaction " + strPRDDT_No + " details added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        ddlLOB.Focus();
                    }
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Pre Disbursements Document Transaction details already exists");
            }
            else if (intErrCode == -1)
            {
                if (PRDDTransID == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    strRedirectPageView = "";
                    ddlLOB.Focus();
                }
            }
            else if (intErrCode == -2)
            {
                if (PRDDTransID == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    strRedirectPageView = "";
                    ddlLOB.Focus();
                }
            }
            ddlLOB.Focus();

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Access to the path") && ex.Message.Contains("denied"))
            {
                cvPRDTT.ErrorMessage = "File Path not formed well or Access to the path is denied";
                cvPRDTT.IsValid = false;
                return;
            }
            else if (ex.Message.Contains("Could not find a part of the path"))
            {
                cvPRDTT.ErrorMessage = "File Path defined in Pre Disbursements Document Master is not avilable";
                cvPRDTT.IsValid = false;
                return;
            }
            else
            {
                cvPRDTT.ErrorMessage = "File Path defined in Pre Disbursements Document Master is not avilable ";//ex.Message;
                cvPRDTT.IsValid = false;
                return;
            }
        }
        finally
        {
            //if (objPRDDT_MasterClient != null)
            objPRDDT_MasterClient.Close();
        }
    }
    #endregion

    /* WorkFlow Properties */
    private int WFLOBId { get { return int.Parse(ddlLOB.SelectedValue); } }
    private int WFProduct { get { return 3; } }    /// <summary>
    /// This methode used in Clear the all enterable data
    /// </summary>
    #region Clear
    public void Clear()
    {
        try
        {
            txtCustomerID.Text = "";
            txtCustomerCode.Text = "";
            txtCustomerName.Text = "";
            txtProduct.Text = "";
            txtProductName.Text = "";
            txtStatus.Text = "";
            S3GCustomerCommAddress.ClearCustomerDetails();
            S3GCustomerPermAddress.ClearCustomerDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }
    #endregion

    /// <summary>
    /// This methode used in Cancel button event
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //wf Cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect("../Origination/S3GORGTransLander.aspx?Code=PRT");
    }

    /// <summary>
    /// This methode used in Tab Index Change event
    /// </summary>
    protected void tcPDDT_ActiveTabChanged(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
    }

    /// <summary>
    /// This methode used in Clear the value
    /// </summary>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlConstitition.Items.Clear();// SelectedIndex = -1;
            //ddlConstitition.ClearDropDownList();
            Clear();
            gvPRDDT.Visible = false;
            ddlEnquiry.SelectedText = string.Empty;
            ddlEnquiry.SelectedValue = string.Empty;
            FunPriGetLookUpList();
            ddlBranch.ClearSelection();
            ddlRefPoint.ClearSelection();
            txtProduct.Text = string.Empty;
            txtProductName.Text = string.Empty;
            //if (ddlEnquiry.I 0) ddlEnquiry.Items.Clear();
            //ddlEnquiry.Clear();
            //if (ddlCustomerCode.SelectedIndex > 0)
            //ddlCustomerCode.SelectedIndex = -1;
            //ddlCustomerCode.ClearDropDownList();
            //txtCustomerCode.Text = string.Empty;
            //txtCustomerID.Text = string.Empty;
            FunProClearCustomerDetails(false);
            lblErrorMessage.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// This methode used  Create ,Modify and Query [User Role Access]
    /// </summary>
    #region Role Access Setup
    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                read();
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                txtPRDDC.Enabled = false;
                ddlLOB.Focus();
                tpPDDT.Enabled = true;
                gvPRDDT.Visible = true;
                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                read();
                //txtPRDDC.Enabled = false;
                //ddlLOB.Enabled = ddlBranch.Enabled = ddlRefPoint.Enabled = false;
                //ddlConstitition.Enabled = ddlCustomerCode.Enabled = ddlEnquiry.Enabled = false;
                //txtDate.ReadOnly = true;
                //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                //btnClear.Enabled = false;
                //ucCustomerCodeLov.ButtonEnabled = false;
                //TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                //txt.Enabled = false;
                //txtCustomerName.Enabled = false;
                //tpPDDT.Enabled = true;

                // ddlRefPoint.ClearDropDownList();
                ucCustomerCodeLov.ButtonEnabled = false;
                // TextBox txt1 = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                // txt1.Enabled = false;
                txtPRDDC.ReadOnly = true;
                ddlEnquiry.Enabled = false;
                ddlBranch.Enabled = false;
                txtPRDDC.Enabled = true;
                btnClear.Enabled_False();
                //btnClear.CssClass = "css_btn_disabled";
                //   btnSave.Enabled = false;
                FunGetPRDDTrans();
                //FunPriGetPRDDTransType();
                FunGetPRDDTransDocDetatils();

                if (ViewState["PageMode"] == null)
                {
                    FunPriDeletePRDDT();
                }

                break;

            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                // ddlRefPoint.ClearDropDownList();
                ucCustomerCodeLov.ButtonEnabled = false;
                //  TextBox txt1 = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                // txt1.Enabled = false;
                txtPRDDC.ReadOnly = true;
                ddlEnquiry.Enabled = false;
                ddlBranch.Enabled = false;
                txtPRDDC.Enabled = true;
                //btnClear.Enabled = false;
                //btnClear.CssClass = "css_btn_disabled";
                btnClear.Enabled_False();
                btnSave.Enabled = false;
                btnSave.CssClass = "css_btn_disabled";
                //btnClear.CssClass = "css_btn_disabled";
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                read();
                FunGetPRDDTrans();
                //FunPriGetPRDDTransType();
                FunGetPRDDTransDocDetatils();

                gvPRDDT.Columns[11].Visible = false;
                gvPRDDT.FooterRow.Visible = false;
                gvPRDDT.Columns[gvPRDDT.Columns.Count - 1].Visible = false;
                foreach (GridViewRow grv in gvPRDDT.Rows)
                {
                    //  DropDownList ddlCollectedBy = (DropDownList)grv.FindControl("ddlCollectedBy");
                    UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)grv.FindControl("ddlCollectedBy");
                    DropDownList ddlCollAndWaiver = (DropDownList)grv.FindControl("ddlCollAndWaiver");
                    TextBox TxtCollectedDt = (TextBox)grv.FindControl("txtCollectedDate");

                    // DropDownList ddlScannedBy = (DropDownList)grv.FindControl("ddlScannedBy");
                    UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)grv.FindControl("ddlScannedBy");

                    TextBox TxtScannedDt = (TextBox)grv.FindControl("txtScannedDate");
                    TextBox TxtScanRef = (TextBox)grv.FindControl("txtScan");
                    TextBox TxtRemarks = (TextBox)grv.FindControl("txtRemarks");
                    CheckBox CkBox = (CheckBox)grv.FindControl("CbxCheck");
                    ImageButton hyplnkView = (ImageButton)grv.FindControl("hyplnkView");
                    Label myThrobber = (Label)grv.FindControl("myThrobber");
                    FileUpload asyFileUpload = (FileUpload)grv.FindControl("asyFileUpload");
                    //AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");
                    AjaxControlToolkit.CalendarExtender DtCollect = (AjaxControlToolkit.CalendarExtender)grv.FindControl("calCollectedDate");
                    AjaxControlToolkit.CalendarExtender DtScan = (AjaxControlToolkit.CalendarExtender)grv.FindControl("calScannedDate");
                    TxtScanRef.ReadOnly = true;
                    TxtRemarks.ReadOnly = true;
                    CkBox.Enabled = false;   //myThrobber.Visible 
                    TxtCollectedDt.ReadOnly = true;
                    asyFileUpload.Enabled = false;
                    TxtScannedDt.ReadOnly = true;
                    DtCollect.Enabled = false;
                    DtScan.Enabled = false;
                    ddlCollectedBy.ReadOnly = true;
                    ddlScannedBy.ReadOnly = true;
                    ddlCollAndWaiver.Enabled = false;
                    //if (myThrobber.Text.Trim() == "")
                    //{
                    ////    TxtScannedDt.Text  = TxtScannedBy.Text = "";    
                    //    TxtScannedDt.Visible = ddlScannedBy.Visible = false;
                    //}
                    //if (!CkBox.Checked)
                    //{
                    //    //TxtCollectedBy.Text = TxtCollectedDt.Text = "";
                    //    ddlCollectedBy.Visible = TxtCollectedDt.Visible = false;
                    //}
                }

                if (bClearList)
                {
                    //  ddlLOB.ClearDropDownList();
                    // ddlBranch.Clear();
                    // ddlConstitition.ClearDropDownList();
                    //ddlEnquiry.ClearDropDownList();
                    //ddlCustomerCode.ClearDropDownList();
                }
                break;
        }

    }
    #endregion

    /// <summary>
    /// This methode used  Covert date format
    /// </summary>
    #region Date Format
    private string ConvertToCurrentFormat(string strDate)
    {
        //  string dT = strDate;

        try
        {
            if (strDate.Contains("1900"))
                strDate = string.Empty;


            strDate = strDate.Replace("12:00:00 AM", "");

            CultureInfo myDTFI = new CultureInfo("en-GB", true);
            DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
            DTF.ShortDatePattern = "dd/MM/yyyy";
            DateTime _Date = new DateTime();
            if (strDate != "")
            {
                _Date = System.Convert.ToDateTime(strDate, DTF);
                return _Date.ToString("dd/MM/yyyy");
            }
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            return strDate;
            // throw ex;
        }
    }

    private void FunPriLoadEnquiryDetailsFromWorkflow()
    {
        try
        {

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ucCustomerCodeLov.SelectedText = string.Empty;
            ucCustomerCodeLov.SelectedValue = string.Empty;
            ucCustomerCodeLov.ClearAddressControls();
            ucCustomerCodeLov.FunPubClearControlValue();
            ddlEnquiry.SelectedValue = string.Empty;
            ddlEnquiry.SelectedText = string.Empty;
            txtProduct.Text = string.Empty;
            txtProductName.Text = string.Empty;

            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                //int strConstitution = ddlConstitition.SelectedValue == "" ? 0 : Convert.ToInt32(ddlConstitition.SelectedValue);
                //saranya
                //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), strConstitution , PRDDTransID);

                //if (ddlConstitition.Items.Count > 0) ddlConstitition.SelectedIndex = 0;
                //FunPriBindRefNo();

                //if (ddlEnquiry.Items.Count > 0) ddlEnquiry.SelectedIndex = 0;
                ucCustomerCodeLov.ButtonEnabled = true;
            }
            else
            {
                ddlConstitition.Items.Clear();
                if (ddlEnquiry.SelectedValue != "0") ddlEnquiry.SelectedValue = "0";
                Clear();
                FunProClearCustomerDetails(false);
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
            }
            ddlBranch.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }


    #region Exits PRDDTransaction
    private void FunPriDeletePRDDT()
    {
        objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

        try
        {
            ObjS3G_ORG_ExitsPRDDCTransRows = ObjS3G_ORG_ExitsPRDDCTransDocMasterDataTable.NewS3G_ORG_ExitsPRDDTMasterRow();
            ObjS3G_ORG_ExitsPRDDCTransRows.Company_ID = intCompanyId;
            ObjS3G_ORG_ExitsPRDDCTransRows.Document_Type = Convert.ToInt32(ddlRefPoint.SelectedValue);
            ObjS3G_ORG_ExitsPRDDCTransRows.PreDisbursement_Doc_Tran_ID = PRDDTransID;
            ObjS3G_ORG_ExitsPRDDCTransDocMasterDataTable.AddS3G_ORG_ExitsPRDDTMasterRow(ObjS3G_ORG_ExitsPRDDCTransRows);
            SerializationMode SerMode = SerializationMode.Binary;

            intErrCode = objPRDDT_MasterClient.FunPubExitsPRDDTrans(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_ExitsPRDDCTransDocMasterDataTable, SerMode));
            if (intErrCode > 0)
            {
                btnSave.Enabled = false;
                btnClear.Enabled_False();
                Utility.FunShowAlertMsg(this, "Pre Disbursements Document Transaction details used in transaction");
                return;
            }

            //objPRDDT_MasterClient.Close();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objPRDDT_MasterClient.Close();
        }
    }
    #endregion

    private void FunPriMoveFile(string strFilePath, string strNewPath)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(strFilePath);
            FileInfo fileNewInfo = new FileInfo(strNewPath);
            if (!Directory.Exists(fileNewInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileNewInfo.DirectoryName);
            }
            if (File.Exists(strNewPath))
            {
                File.Delete(strNewPath);
            }
            fileInfo.MoveTo(strNewPath);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvPRDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        try
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    ObjStatus.Option = 35;
            //    ObjStatus.Param1 = intCompanyId.ToString();
            //    ViewState["UserDetails"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblPath = (Label)e.Row.FindControl("lblPath");
                Label lblmyThrobber = (Label)e.Row.FindControl("myThrobber");

                //Added by saranya
                //DropDownList ddlCollectedby = (DropDownList)e.Row.FindControl("ddlCollectedby");

                //Changed By Shibu 19-Sep-2013 to Add Auto Suggestion
                UserControls_S3GAutoSuggest ddlCollectedby = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlCollectedby");

                AjaxControlToolkit.CalendarExtender calCollectedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calCollectedDate");
                AjaxControlToolkit.CalendarExtender calScannedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calScannedDate");
                calScannedDate.Format = calCollectedDate.Format = strDateFormat;
                TextBox txtColletedDate = (TextBox)e.Row.FindControl("txtCollectedDate");
                //end
                ImageButton Viewdoct = (ImageButton)e.Row.FindControl("hyplnkView");
                CheckBox Cbx1 = (CheckBox)e.Row.FindControl("CbxCheck");
                //DropDownList ddlScannedby = (DropDownList)e.Row.FindControl("ddlScannedby");
                TextBox txtScannedDate = (TextBox)e.Row.FindControl("txtScannedDate");
                TextBox txtUpload = (TextBox)e.Row.FindControl("txOD");
                //commented by saranya
                //Label lblCollectedby = (Label)e.Row.FindControl("txtColletedBy");
                //Label txtColletedDate = (Label)e.Row.FindControl("txtColletedDate");
                //Label txtScannedDate = (Label)e.Row.FindControl("txtScannedDate");
                //Label lblScanneddby = (Label)e.Row.FindControl("txtScannedBy");
                //txtColletedDate.Attributes.Add("readonly", "readonly");
                //txtScannedDate.Attributes.Add("readonly", "readonly");
                //end
                //Added by Shibu 6-Jun-2013
                CheckBox chkOptMan = (CheckBox)e.Row.FindControl("chkOptMan");
                CheckBox CbxCheck = (CheckBox)e.Row.FindControl("CbxCheck");
                DropDownList ddlCollAndWaiver = (DropDownList)e.Row.FindControl("ddlCollAndWaiver");
                //DropDownList ddlScannedBy = (DropDownList)e.Row.FindControl("ddlScannedBy");
                UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlScannedBy");
                Label lblOptMan = (Label)e.Row.FindControl("lblOptMan");
                Label lblReceivedStatus = (Label)e.Row.FindControl("lblReceivedStatus");
                Label lblChecklistFlag = (Label)e.Row.FindControl("lblChecklistFlag");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                Label lblPRDDTrans = (Label)e.Row.FindControl("lblPRDDTrans");
                FileUpload asyFileUpload = (FileUpload)e.Row.FindControl("asyFileUpload");
                //AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)e.Row.FindControl("asyFileUpload");
                string Doc_CollectOrWaived = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["Doc_CollectOrWaived"].ToString());
                Button btnBrowseI = (Button)e.Row.FindControl("btnBrowseI");
                Label lblActualPath = (Label)e.Row.FindControl("lblActualPathI");
                asyFileUpload.Attributes.Add("onchange", "fnLoadPath('" + btnBrowseI.ClientID + "'); ");


                string path = Path.GetFileName(lblActualPath.Text);
                lblActualPath.Text = path;
                lblActualPath.Visible = true;

                if (Doc_CollectOrWaived != "")
                {
                    ddlCollAndWaiver.SelectedValue = Doc_CollectOrWaived;

                }
                if (ddlCollAndWaiver.SelectedValue == "W")
                {
                    ddlScannedBy.Enabled = false;
                    txtScannedDate.Enabled = false;
                }
                else
                {
                    ddlScannedBy.Enabled = true;
                    txtScannedDate.Enabled = true;
                }
                if (lblOptMan.Text.Trim() == "True" || lblOptMan.Text.Trim() == "1")
                {
                    chkOptMan.Checked = true;
                }
                else
                {
                    chkOptMan.Checked = false;
                }

                //End 

                //Utility.FillDLL(ddlCollectedby, (DataTable)ViewState["UserDetails"]);
                //Utility.FillDLL(ddlScannedby, (DataTable)ViewState["UserDetails"]);

                //Added by saranya
                /*
                ObjStatus.Option = 35;
                ObjStatus.Param1 = intCompanyId.ToString();
                Utility.FillDLL(ddlCollectedby, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

                ObjStatus.Option = 35;
                ObjStatus.Param1 = intCompanyId.ToString();
                Utility.FillDLL(ddlScannedby, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                */
                Label lblCollectedBy = e.Row.FindControl("lblCollectedBy") as Label;
                Label lblScannedBy = e.Row.FindControl("lblScannedBy") as Label;

                if (lblCollectedBy.Text != "")
                {
                    ddlCollectedby.SelectedValue = lblCollectedBy.Text;
                    ddlCollectedby.SelectedText = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["CollectedBy"].ToString());
                }
                if (lblScannedBy.Text != "")
                {
                    ddlScannedBy.SelectedValue = lblScannedBy.Text;
                    ddlScannedBy.SelectedText = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["ScandedBy"].ToString());
                }


                if (txtColletedDate.Text.Contains("1900"))
                {
                    txtColletedDate.Text = "";

                    //Viewdoct.Enabled = false;
                }
                //if (PRDDTransID > 0)
                //{
                //    Viewdoct.Enabled = true;
                //    Viewdoct.CssClass = "styleGridEdit";
                //}
                //else
                //{
                //    Viewdoct.Enabled = false;
                //    Viewdoct.CssClass = "styleGridEditDisabled";
                //}

                if (lblPath.Text != string.Empty)
                {
                    Viewdoct.Enabled = true;
                    Viewdoct.CssClass = "styleGridEdit";
                }
                else
                {
                    Viewdoct.Enabled = false;
                    Viewdoct.CssClass = "styleGridEditDisabled";
                }
                if (txtScannedDate.Text.Contains("1900"))
                {
                    Cbx1.Checked = false;
                    txtScannedDate.Text = "";
                    //txtScannedDate.Visible = false; //added
                    ddlScannedBy.Clear();
                    //ddlScannedBy.Visible = false; //added   
                }
                if (ViewState["Docpath"] != null)
                    txtUpload.Text = ViewState["Docpath"].ToString();

                if (strMode == "M")
                {
                    txtUpload.Text = lblPath.Text;
                    if (string.IsNullOrEmpty(txtUpload.Text))
                    {
                        Viewdoct.Enabled = false;
                        Viewdoct.CssClass = "styleGridEditDisabled";
                    }
                }

                if (lblChecklistFlag.Text == "1" && lblReceivedStatus.Text != "0" && lblReceivedStatus.Text != "2" && lblReceivedStatus.Text != "NC")
                {
                    ddlCollAndWaiver.Enabled = false;
                    ddlCollectedby.Enabled = false;
                    txtColletedDate.Enabled = false;
                    //ddlScannedBy.Enabled = false;
                    //txtScanDate.Enabled = false;
                    //if (strMode != "C")
                    //{
                    //    asyFileUpload.Enabled = false;
                    //}
                    //txtRemarks.Enabled = false;
                    if (lblChecklistFlag.Text == "1" && lblPRDDTrans.Text != "1")
                    {
                        CbxCheck.Enabled = true;
                    }
                    else
                    {
                        CbxCheck.Enabled = false;
                    }

                    //ddlCollAndWaiver.Items.Add(new System.Web.UI.WebControls.ListItem("", ""));

                    if (lblReceivedStatus.Text == "1")
                    {
                        lblReceivedStatus.Text = "C";
                        ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                        ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                    }
                    else if (lblReceivedStatus.Text == "3")
                    {
                        lblReceivedStatus.Text = "W";
                        ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                        ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                    }
                    else if (lblReceivedStatus.Text == "2")
                    {
                        lblReceivedStatus.Text = "NC";
                        ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                        ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                    }
                    else
                    {
                        lblReceivedStatus.Text = "NA";
                        ddlCollAndWaiver.SelectedItem.Text = ddlCollAndWaiver.Items.FindByValue(lblReceivedStatus.Text).ToString();
                        ddlCollAndWaiver.SelectedValue = lblReceivedStatus.Text;
                    }
                }
                //if (lblCollectedby.Text == "")
                //    lblCollectedby.Text = ObjUserInfo.ProUserNameRW;

                //if (lblScanneddby.Text == "")
                //    lblScanneddby.Text = ObjUserInfo.ProUserNameRW;



            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    //protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
    //        if (hdnCustomerId != null && hdnCustomerId.Value != "")
    //        {

    //            ddlConstitition.Items.Clear();
    //            FunProClearCustomerDetails(true);
    //            ddlEnquiry.Clear();
    //            FunProGetCustomerDetails(hdnCustomerId.Value.ToString());
    //            // FunProLoadPANum(Convert.ToInt32(hdnCustomerId.Value.ToString()), Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), Convert.ToInt32(ViewState["intPDDTransID"]));

    //            ViewState["CustomerID"] = hdnCustomerId.Value;


    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        throw new ApplicationException("Unable to display Customer Details");
    //    }
    //}
    protected void FunProGetCustomerDetails(string intCustomerID)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Customer_Id", intCustomerID.ToString());
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtCustDetails = Utility.GetDefaultData("S3G_GetCustomerDetails", dictParam);

            if (dtCustDetails.Rows.Count > 0)
            {
                fnBindCustomerDetails(dtCustDetails.Rows[0], "Customer");
                ddlConstitition.Items.Add(new ListItem(dtCustDetails.Rows[0]["Constitution_Name"].ToString(), dtCustDetails.Rows[0]["Constitution_ID"].ToString()));
            }
            else
            {
                throw new Exception("Unable to load Customer information");
            }

        }
        catch (Exception ex)
        {
            cvPRDTT.ErrorMessage = ex.ToString();
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }
    protected void FunProClearCustomerDetails(bool FromCustomer)
    {
        if (!FromCustomer)
        {
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Text = "";
            txtCustomerName.Text = "";
            txtCustomerID.Text = "";

            S3GCustomerCommAddress.ClearCustomerDetails();
            S3GCustomerPermAddress.ClearCustomerDetails();
            //ucCustomerCodeLov.strBranchID = "-1";
            //ucCustomerCodeLov.strLOBID = "-1";
            //ucCustomerCodeLov.strRegionID = "-1";
        }
        //txtProduct.Text = "";
        //txtProductName.Text = "";
        //txtStatus.Text = "";
        ViewState["DocDetails"] = gvPRDDT.DataSource = null;
        gvPRDDT.DataBind();
        ViewState["DocPath"] = "";
    }

    //No Need Customer DropdownControl it's Changed to Popup Control By Shibu
    //protected void ddlCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        txtCustomerID.Text = ddlCustomerCode.SelectedValue;
    //        if (ddlConstitition.Items.Count > 0)
    //        {
    //            //ddlConstitition.SelectedIndex = -1;
    //            ddlConstitition.Items.Clear();
    //        }
    //        //if (ddlLOB.Items.Count > 0)
    //        //{
    //        //    ddlLOB.SelectedIndex = 0;
    //        //    ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
    //        //}
    //        //if (ddlBranch.Items.Count > 0) ddlBranch.SelectedIndex = 0;

    //        gvPRDDT.Visible = false;
    //        if (ddlCustomerCode.SelectedIndex == 0) Clear();
    //        //if (ddlRefPoint.SelectedIndex > 0) ddlRefPoint.ClearSelection();
    //        //if (ddlEnquiry.Items.Count > 0)
    //        //{
    //        //    ddlEnquiry.SelectedIndex = 0;
    //        //    ddlEnquiry.Items.Clear();
    //        //}
    //        FunPriGetLookUpList();
    //        Clear();


    //        PopulateEnquiry();
    //        if (ddlEnquiry.Items.Count == 2)
    //        {
    //            ddlEnquiry.SelectedValue = ddlEnquiry.Items[1].Value;
    //            FunPriGetCustomerDetails();
    //            FunPriGetPRDDTransType();
    //            tpPDDT.Enabled = true;
    //            gvPRDDT.Visible = true;
    //        }

    //        if (ddlCustomerCode.SelectedIndex != 0 && ddlEnquiry.Items.Count > 2)
    //        {
    //            Dictionary<string, string> dictParam = new Dictionary<string, string>();
    //            dictParam.Add("@Customer_Id", ddlCustomerCode.SelectedValue.ToString());
    //            dictParam.Add("@Company_ID", intCompanyId.ToString());
    //            DataTable dtCustDetails = Utility.GetDefaultData("S3G_GetCustomerDetails", dictParam);

    //            if (dtCustDetails.Rows.Count > 0)
    //                fnBindCustomerDetails(dtCustDetails.Rows[0], "Customer");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}
    //Add by Shibu 6-Jun-2013 
    protected void ddlCollAndWaiver_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlCollAndWaiver = sender as DropDownList;
        int gvRowIndex = ((GridViewRow)ddlCollAndWaiver.Parent.Parent).RowIndex;
        UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[gvRowIndex].FindControl("ddlScannedBy");
        UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[gvRowIndex].FindControl("ddlCollectedBy");
        TextBox txtScannedDate = (TextBox)gvPRDDT.Rows[gvRowIndex].FindControl("txtScannedDate");
        TextBox txtCollectedDate = (TextBox)gvPRDDT.Rows[gvRowIndex].FindControl("txtCollectedDate");
        if (ddlCollAndWaiver.SelectedValue == "W")
        {
            try
            {
                //txtScannedDate.Text = "";
                if (ddlScannedBy.Visible == true)
                {
                    ddlScannedBy.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            }
            ddlScannedBy.Enabled = false;
            txtScannedDate.Enabled = false;
        }
        else
        {
            ddlScannedBy.Visible = true;
            txtScannedDate.Visible = true;
            ddlScannedBy.Enabled = true;
            txtScannedDate.Enabled = true;
        }
        //ddlCollectedBy.Clear();
        //txtCollectedDate.Text = string.Empty;
        if (ddlCollAndWaiver.SelectedValue == "NC" || ddlCollAndWaiver.SelectedValue == "NA")//Not Collected or Not Applicable
        {
            ddlCollectedBy.Enabled = false;
            txtCollectedDate.Enabled = false;
        }
        else
        {
            //ddlCollectedBy.Enabled = true;
            //txtCollectedDate.Enabled = true;
        }
    }
    // Modified By R. Manikandan

    //protected void ddlCollectedBy_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int intCurrentRow = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex;
    //    //((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex
    //    UserControls_S3GAutoSuggest ddlCollectedBy = gvPRDDT.Rows[intCurrentRow].FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;
    //    if (ddlCollectedBy.SelectedValue != "0")
    //    {

    //        Label lblCollectedBy = (Label)gvPRDDT.Rows[intCurrentRow].FindControl("lblCollectedBy");
    //        lblCollectedBy.Text = ddlCollectedBy.SelectedValue;
    //    }

    //}
    // Modified By R. Manikandan
    //protected void ddlScannedBy_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int intCurrentRow = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex;
    //    UserControls_S3GAutoSuggest ddlScannedBy = gvPRDDT.Rows[intCurrentRow].FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;


    //    if (ddlScannedBy.SelectedValue != "0")
    //    {

    //        Label lblScannedBy = (Label)gvPRDDT.Rows[intCurrentRow].FindControl("lblScannedBy");
    //        lblScannedBy.Text = ddlScannedBy.SelectedValue;
    //    }
    //}

    //private void PopulateEnquiry()
    //{
    //    try
    //    {
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", intCompanyId.ToString());
    //        Procparam.Add("@User_Id", intUserId.ToString());
    //        Procparam.Add("@Program_Id", Program_Id.ToString());
    //        if (ddlRefPoint.SelectedIndex > 0)
    //            Procparam.Add("@Option", ddlRefPoint.SelectedValue);
    //        if (ddlCustomerCode.SelectedIndex != 0)
    //            Procparam.Add("@Customer_ID", ddlCustomerCode.SelectedValue);
    //        Procparam.Add("@Type", "Enquiry");
    //        ddlEnquiry.BindDataTable("S3G_ORG_GetPDDTCustomer", Procparam, new string[] { "ID", "Number" });
    //    }
    //    catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.Text = objFaultExp.Message;
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    //private void PopulateCustomerCode()
    //{
    //    try
    //    {
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", intCompanyId.ToString());
    //        Procparam.Add("@User_Id", intUserId.ToString());
    //        Procparam.Add("@Program_Id", Program_Id.ToString());
    //        if (ddlRefPoint.SelectedIndex > 0)
    //            Procparam.Add("@Option", ddlRefPoint.SelectedValue);
    //        if (ddlEnquiry.SelectedIndex > 0)
    //            Procparam.Add("@Ref_ID", ddlEnquiry.SelectedValue);
    //        Procparam.Add("@Type", "CUSTOMER");
    //        ddlCustomerCode.BindDataTable("S3G_ORG_GetPDDTCustomer", Procparam, true, "-- Select --", new string[] { "Customer_ID", "Customer_Code" });


    //    }
    //    catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.Text = objFaultExp.Message;
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}


    // Added By Shibu 17-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_PageValue.intUserId.ToString());
        Procparam.Add("@Program_Id", obj_PageValue.Program_Id.ToString());
        Procparam.Add("@Lob_Id", obj_PageValue.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    // Added By Shibu 17-Sep-2013 User List (Auto Suggestion)
    [System.Web.Services.WebMethod]
    public static string[] GetUserList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_Get_User_List, Procparam));

        return suggestions.ToArray();
    }

    // Added By Shibu 17-Sep-2013 Reference Number (Auto Suggestion)
    [System.Web.Services.WebMethod]
    public static string[] GetRefDocList(String prefixText, int count)
    {
        //Procparam.Add("@Company_ID", intCompanyId.ToString());
        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
        //Procparam.Add("@Constitution_ID", ddlConstitition.SelectedValue);
        //Procparam.Add("@OptionValue", ddlRefPoint.SelectedValue);
        //ddlEnquiry.BindDataTable("S3G_ORG_GetPDDTRefDocNo", Procparam, new string[] { "RefDoc_ID", "RefDoc_No" });
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@LOB_ID", obj_PageValue.ddlLOB.SelectedValue);
        Procparam.Add("@Location_ID", obj_PageValue.ddlBranch.SelectedValue);
        Procparam.Add("@Constitution_ID", obj_PageValue.ddlConstitition.SelectedValue);
        Procparam.Add("@OptionValue", obj_PageValue.ddlRefPoint.SelectedValue);
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Customer_ID", obj_PageValue.txtCustomerID.Text);
        Procparam.Add("@SearchText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetPDDTRefDocNo_AGT", Procparam));

        return suggestions.ToArray();
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }
    private void FunPriLoadLocation(int CompanyId, int UserId, int ProgramId, int LobId)
    {
        try
        {

            DataTable dt = new DataTable();
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@COMPANY_ID", intCompanyId.ToString());
            strProParm.Add("@USER_ID", intUserId.ToString());
            strProParm.Add("@PROGRAM_ID", Program_Id);
            strProParm.Add("@LOB_ID", ddlLOB.SelectedValue);
            strProParm.Add("@OPTION", "1");
            dt = Utility.GetDefaultData("S3G_GET_LOCATION ", strProParm);
            ddlBranch.FillDataTable(dt, "BRANCH_ID", "BRANCH");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {

        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetRefPoint(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_Id", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@User_Id", obj_PageValue.intUserId.ToString());
        Procparam.Add("@LOB_ID", obj_PageValue.ddlLOB.SelectedValue);
        Procparam.Add("@Location_ID", obj_PageValue.ddlBranch.SelectedValue);
        Procparam.Add("@Constitution_ID", obj_PageValue.ddlConstitition.SelectedValue);
        Procparam.Add("@OptionValue", obj_PageValue.ddlRefPoint.SelectedValue);
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GET_PDD_DTL", Procparam));

        return suggetions.ToArray();
    }

    protected void ddlEnquiry_Item_Selected(object Sender, EventArgs e)
    {
        if (ddlEnquiry.SelectedValue != "0")
        {
            // PopulateCustomerCode();
            //FunPriGetCustomerDetails();
            FunPriGetPRDDTransID();
            FunPriGetPRDDTransType();
            //FunPriGetCustomerDetails();
            tpPDDT.Visible = true;
            gvPRDDT.Visible = true;
            tpPDDT.Enabled = true;
        }
        else
        {
            //PopulateCustomerCode();
            // if (ddlConstitition.Items.Count > 0) ddlConstitition.SelectedIndex = 0;
            if (ddlLOB.Items.Count > 0)
            {
                ddlLOB.SelectedIndex = 0;
                ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
            }
            // if (ddlCustomerCode.Items.Count > 0) ddlCustomerCode.SelectedIndex = 0;
            if (ddlBranch.SelectedValue != "0") ddlBranch.SelectedValue = "0";
            tpPDDT.Enabled = false;
            gvPRDDT.Visible = false;
            ddlBranch.ClearSelection();
            //ddlEnquiry.Clear();
            tpPDDT.Enabled = false;
            gvPRDDT.Visible = false;
            FunProClearCustomerDetails(false);
            ddlConstitition.Items.Clear();
            Clear();
        }
    }
    protected void btnBrowseI_Click(object sender, EventArgs e)
    {
        try
        {


            string strPath = string.Empty;
            string strNewFileName = string.Empty;
            int intRowIndex = Utility.FunPubGetGridRowID("gvPRDDT", ((Button)sender).ClientID);
            //TextBox txtScanDate = (TextBox)gvPRDDT.Rows[intRowIndex].FindControl("txtScannedDate");
            ////Label txtScanBy = (Label)gvPRDDT.Rows[i].FindControl("txtScannedBy");
            //UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[intRowIndex].FindControl("ddlScannedBy");
            //Label lblScannedBy = (Label)gvPRDDT.Rows[intRowIndex].FindControl("lblScannedBy");
            ////End Here
            ImageButton hyplnkView = (ImageButton)gvPRDDT.Rows[intRowIndex].FindControl("hyplnkView");
            FileUpload asyFileUpload = (FileUpload)gvPRDDT.Rows[intRowIndex].FindControl("asyFileUpload");
            ////AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[intRowIndex].FindControl("asyFileUpload");
            //CheckBox CbxCheck = (CheckBox)gvPRDDT.Rows[intRowIndex].FindControl("CbxCheck");

            //Label lblReceivedStatus = (Label)gvPRDDT.Rows[intRowIndex].FindControl("lblReceivedStatus");
            //Label lblChecklistFlag = (Label)gvPRDDT.Rows[intRowIndex].FindControl("lblChecklistFlag");
            //DropDownList ddlCollAndWaiver = (DropDownList)gvPRDDT.Rows[intRowIndex].FindControl("ddlCollAndWaiver");
            //UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[intRowIndex].FindControl("ddlCollectedBy");
            //TextBox txtCollectedDate = (TextBox)gvPRDDT.Rows[intRowIndex].FindControl("txtCollectedDate");
            //TextBox txtRemarks = (TextBox)gvPRDDT.Rows[intRowIndex].FindControl("txtRemarks");
            //TextBox txOD = (TextBox)gvPRDDT.Rows[intRowIndex].FindControl("txOD");
            Label lblPathUploadlabel = (Label)gvPRDDT.Rows[intRowIndex].FindControl("lblPathUploadlabel");
            Label lblPathF = (Label)gvPRDDT.Rows[intRowIndex].FindControl("lblPath");
            lblPathUploadlabel.Text = asyFileUpload.FileName;
            Label lblSno = (Label)gvPRDDT.Rows[intRowIndex].FindControl("lblSno");


            string strEnqNumber = ddlEnquiry.SelectedText.Replace("/", "-");

            //if (AsyncFileUpload1.FileName != "" && hidThrobber.Value.Trim() != "")
            //{
            if (asyFileUpload.HasFile)
            {
                strNewFileName = asyFileUpload.FileName;
                if (!ValidateFileExtn(strNewFileName))
                {
                    return;
                }
                if (ViewState["Docpath"].ToString() != "")
                {
                    HiddenField hdnFilePath = (HiddenField)gvPRDDT.Rows[intRowIndex].FindControl("hdnFilePath") as HiddenField;
                    if (ViewState["Docpath"].ToString() != "")
                    {
                        strPath = Path.Combine(ViewState["Docpath"].ToString(), "COMPANY" + intCompanyId.ToString() + "/" + strEnqNumber + "/" + "PDDTC-" + lblSno.Text);
                        if (Directory.Exists(strPath))
                        {
                            Directory.Delete(strPath, true);
                        }
                        Directory.CreateDirectory(strPath);
                        strPath = strPath + "/" + strNewFileName;
                    }
                    txOD.Text = strPath;// txOD.Text + strEnqNumber + "\\" + "PDDTC-" + intRowindex.ToString() + "\\" + strNewFileName;

                    FileInfo f1 = new FileInfo(strPath);
                    asyFileUpload.Focus();
                    if (f1.Exists == true)
                        f1.Delete();

                    asyFileUpload.SaveAs(strPath);

                    hdnFilePath.Value = Convert.ToString(strPath);
                    if (hdnFilePath.Value == string.Empty)
                    {
                        hyplnkView.Enabled = false;
                        hyplnkView.CssClass = "styleGridEditDisabled";
                    }
                    else
                    {
                        hyplnkView.Enabled = true;
                        hyplnkView.CssClass = "styleGridEdit";
                    }
                }
            }
            //else  
            //{
            //    Utility.FunShowAlertMsg(this, "No File Found");
            //}
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, "");
        }

        //if (asyFileUpload.HasFile)
        //{
        //    strNewFileName = asyFileUpload.FileName;

        //    if (!ValidateFileExtn(strNewFileName))
        //    {
        //        return;
        //    }

        //    lblPathF.Text = lblPathUploadlabel.Text = asyFileUpload.FileName;
        //    asyFileUpload.ToolTip = lblPathF.Text;


        //    string strEnqNumber = ddlEnquiry.SelectedText.Replace("/", "-");
        //    if (txOD.Text != "")
        //    {
        //        strPath = Path.Combine(ViewState["Docpath"].ToString(), "COMPANY" + intCompanyId.ToString() + "/" + strEnqNumber + "/" + "PDDTC-" + intRowindex.ToString());
        //        if (Directory.Exists(strPath))
        //        {
        //            Directory.Delete(strPath, true);
        //        }
        //        Directory.CreateDirectory(strPath);
        //        strPath = strPath + "/" + strNewFileName;
        //    }
        //    strCurrentFilePath = lblActualPath.Text = strPath;


        //    FileInfo f1 = new FileInfo(strPath);

        //    if (f1.Exists == true)
        //        f1.Delete();
        //    asyFileUpload.SaveAs(strPath);
        //    FileUpload t = (FileUpload)(gvPRDDT.FooterRow.Cells[1].FindControl("asyFileUpload"));
        //    //t.Focus();
        //    //flUpload2.Focus();

        //}
        //else
        //{
        //    Utility.FunShowAlertMsg(this, "No HashFile");
        //}

        //if (lblPathF.Text == string.Empty)
        //{
        //    hyplnkViewF.Enabled_False_Link_Asp();
        //}
        //else
        //{
        //    hyplnkViewF.Enabled_True_Link_Asp();
        //}

    }
    protected bool ValidateFileExtn(string strFileName)
    {

        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg" };
        string[] validFileTypes2 = { "bmp", "gif", "png", "jpg", "jpeg" };

        string ext = System.IO.Path.GetExtension(strFileName);

        bool isValidFile = false;

        for (int i = 0; i < validFileTypes.Length; i++)
        {

            if (ext == "." + validFileTypes[i])
            {

                isValidFile = true;

                break;

            }

        }

        if (!isValidFile)
        {
            Utility.FunShowAlertMsg(this, "Invalid File. Please upload a File with extension " + string.Join(",", validFileTypes2));
        }
        else
        {
        }

        return isValidFile;

    }
}


