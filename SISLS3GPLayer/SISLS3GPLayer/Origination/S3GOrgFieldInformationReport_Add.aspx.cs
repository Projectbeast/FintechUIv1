﻿#region namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
#endregion

public partial class Origination_S3GOrgFieldInformationReport : ApplyThemeForProject
{
    #region InitialValues

    // ddl Static data source.
    string[] _StatusSource = new string[2] { "Floated", "Responded" };  // default value should be in 0'th index
    string[] _ClientCreditibility = new string[3] { "High", "Medium", "Low" };
    string[] _ClientNetWorth = new string[3] { "High", "Medium", "Low" };
    string[] _DocumentValue = new string[7] { "--Select--", "None", "Very Poor", "Poor", "Moderate", "Good", "Excellent" };
    static string _AgencyCode = "";
    static string _FIRNo;
    Dictionary<string, string> Procparam = null;
    int intUserID = 0;
    int maxversion = 0;
    bool chkbox = false;
    int intCompanyID = 0;
    string intCustomerId = "";
    public string strDateFormat = string.Empty;
    bool bCreate = false;
    bool bMod = false;

    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    string strMode = string.Empty;
    DataTable dtCustInfo = new DataTable();
    bool bClearList = false;
    int FIRTRANS_ID = 0;
    static string strPageName = "Field Information Report";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=FEIR';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGFieldInformationReport_Add.aspx';";
    string strAlert = "alert('__ALERT__');";
    string strKey = "Insert";

    public static Origination_S3GOrgFieldInformationReport obj_Page;

    CompanyMgtServicesReference.CompanyMgtServicesClient ObjLOBMasterClient;
    CreditMgtServicesReference.CreditMgtServicesClient ObjMgtCreditMgtClient;
    CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable ObjS3G_BranchList = new CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable();

    CreditMgtServices.S3G_ORG_CustomerMasterDataTable ObjS3G_CustomerMasterByCode = new CreditMgtServices.S3G_ORG_CustomerMasterDataTable();
    CreditMgtServices.S3G_ORG_EnquiryResponseDataTable ObjS3G_EnquiryResponse = new CreditMgtServices.S3G_ORG_EnquiryResponseDataTable();
    CreditMgtServices.S3G_ORG_EntityMasterDataTable ObjS3G_EntityMaster = new CreditMgtServices.S3G_ORG_EntityMasterDataTable();
    CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable ObjS3G_FIRTransactionDocDetails = new CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable();
    UserInfo ObjUserInfo;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // WF Implementation
            ProgramCode = "033";

            obj_Page = this;

            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            //cexDate1.Format = strDateFormat;                       // assigning the first textbox with the End date
            //txtResspondedDate_CalendarExtender.Format = strDateFormat;

            ObjUserInfo = new UserInfo();

            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;


            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;

            ddlBranch.Visible = ddlLOB.Visible = false;

            if (!IsPostBack)
            {
                txtResspondedDate.Attributes.Add("readonly", "readonly");
                txtRequestDate.Attributes.Add("readonly", "readonly");
                txtValue.SetDecimalPrefixSuffix(10, 4, true, "Value");
                tcRegBranch.ActiveTabIndex = 0;

                if (PageMode != PageModes.Query)
                {
                    FunPriLoadCombo_CurrencyMaster();
                    ddlRound.Items.Clear();
                    PopulateLookupDescriptions();
                }
                if (PageMode == PageModes.Create)
                {
                    rfvCashflowDesc.Enabled =
                    RequiredFieldValidator4.Enabled =
                    RequiredFieldValidator5.Enabled =
                    RequiredFieldValidator8.Enabled =
                    RequiredFieldValidator7.Enabled =
                    RequiredFieldValidator6.Enabled =
                    RequiredFieldValidator9.Enabled = false;
                }
                FunPriLoadCombos();

                if (Request.QueryString.Get("qsViewId") != null)
                {
                    FormsAuthenticationTicket TicketViewID = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                    if (!(string.IsNullOrEmpty(TicketViewID.Name)))
                    {
                        FIRTRANS_ID = Convert.ToInt32(TicketViewID.Name);
                        ViewState["FIRTRANS_ID"] = FIRTRANS_ID;
                        //ddlEnquiryNumber.SelectedValue = TicketViewID.Name;
                        FunPriInitControls(TicketViewID.Name);

                        strMode = Request.QueryString["qsMode"];
                        if (Request.QueryString.Get("qsMode") != null)
                        {
                            if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
                            {
                                FunPriControlStatus(-1);
                                gvPRDDT.Visible = true;
                                gvPRDDT.Columns[7].Visible = false;
                                foreach (GridViewRow grv in gvPRDDT.Rows)
                                {
                                    Label TxtCollectedBy = (Label)grv.FindControl("lblCollectedBy");
                                    TextBox TxtCollectedDt = (TextBox)grv.FindControl("txtCollectedDate");
                                    //Label TxtScannedBy = (Label)grv.FindControl("lblScannedBy");
                                    TextBox TxtScannedDt = (TextBox)grv.FindControl("txtScannedDate");
                                    TextBox TxtScanRef = (TextBox)grv.FindControl("txtScan");
                                    TextBox TxtRemarks = (TextBox)grv.FindControl("txtRemarks");
                                    CheckBox CkBox = (CheckBox)grv.FindControl("CbxCheck");
                                    ImageButton hyplnkView = (ImageButton)grv.FindControl("hyplnkView");
                                    Label myThrobber = (Label)grv.FindControl("myThrobber");
                                    AjaxControlToolkit.CalendarExtender DtCollect = (AjaxControlToolkit.CalendarExtender)grv.FindControl("cexDate1");
                                    AjaxControlToolkit.CalendarExtender DtScan = (AjaxControlToolkit.CalendarExtender)grv.FindControl("cexDate2");
                                    UserControls_S3GAutoSuggest ddlCollectedby = (UserControls_S3GAutoSuggest)grv.FindControl("ddlCollectedby");
                                    UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)grv.FindControl("ddlScannedBy");
                                    AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");
                                    TxtScanRef.ReadOnly = true;
                                    TxtRemarks.ReadOnly = true;
                                    CkBox.Enabled = false;   //myThrobber.Visible                                    

                                    if (strMode.ToString() == "C")
                                    {
                                        if (myThrobber.Text.Trim() == "")
                                        {
                                            TxtScannedDt.Text = "";
                                        }
                                        if (!CkBox.Checked)
                                        {
                                            TxtCollectedDt.Text = "";
                                        }
                                    }
                                    if (strMode.ToString() == "Q")
                                    {
                                        TxtScannedDt.ReadOnly = TxtCollectedDt.ReadOnly =
                                        ddlCollectedby.ReadOnly = ddlScannedBy.ReadOnly = true;
                                        asyFileUpload.Enabled = false;
                                    }

                                }
                                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                            }
                            else if (string.Compare("M", Request.QueryString.Get("qsMode")) == 0)
                            {
                                FunPriControlStatus(1);
                                tpPDDT.Enabled = true;
                                gvPRDDT.Visible = true;
                                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                            }
                            if (Convert.ToInt32(ddlAgencyCode.SelectedValue) == ObjUserInfo.ProUserIdRW || ObjUserInfo.ProUserTypeRW.ToUpper().Contains("UTPA"))
                            {
                                if (PageMode == PageModes.Modify)
                                {
                                    rfvCashflowDesc.Enabled =
                                    RequiredFieldValidator4.Enabled =
                                    RequiredFieldValidator5.Enabled =
                                    RequiredFieldValidator8.Enabled =
                                    RequiredFieldValidator7.Enabled =
                                    RequiredFieldValidator6.Enabled =
                                    RequiredFieldValidator9.Enabled = true;
                                }
                            }
                            else
                            {
                                rfvCashflowDesc.Enabled =
                                     RequiredFieldValidator4.Enabled =
                                     RequiredFieldValidator5.Enabled =
                                     RequiredFieldValidator8.Enabled =
                                     RequiredFieldValidator7.Enabled =
                                     RequiredFieldValidator6.Enabled =
                                     RequiredFieldValidator9.Enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    FunPriControlStatus(0);
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                }


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

            if (PageMode == PageModes.WorkFlow)
            {
                if (ViewState["FIRTRANS_ID"] != null)
                    FIRTRANS_ID = Convert.ToInt32(ViewState["FIRTRANS_ID"]);
            }

            //FIR TRANS TAB
            foreach (GridViewRow grvData in gvPRDDT.Rows)
            {
                Label myThrobber = (Label)grvData.FindControl("myThrobber");
                HiddenField hidThrobber = (HiddenField)grvData.FindControl("hidThrobber");

                if (hidThrobber.Value != "")
                {
                    myThrobber.Text = hidThrobber.Value;
                }
            }


            lblErrorMessage.Text = string.Empty;

        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }

    }



    #region "WF Code"
    void PreparePageForWorkFlowLoad()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowServiceClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

        try
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            byte[] byte_PreDisDoc = objWorkflowServiceClient.FunPubLoadFIRTransaction(WFSessionValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValues.Document_Type, WFSessionValues.PANUM, WFSessionValues.SANUM);
            DataSet dsWorkflow = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_PreDisDoc, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));


            if (dsWorkflow.Tables.Count > 1)
            {
                if (dsWorkflow.Tables[1].Rows.Count > 0)
                {
                    FIRTRANS_ID = Convert.ToInt32(dsWorkflow.Tables[1].Rows[0]["Doc_Id"].ToString());
                    ViewState["FIRTRANS_ID"] = FIRTRANS_ID;
                    // FunPriDisableControls(1);
                }
            }
            else
            {
                if (dsWorkflow.Tables[0].Rows.Count > 0)
                {
                    ddlDocType.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Doc_Type"].ToString();
                    FunPriSetDoctypeChange();
                    ddlEnquiryNumber.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Doc_ID"].ToString();
                    FunPriLoadCustomerInfo();
                    ddlDocType.ClearDropDownList();
                    //ddlEnquiryNumber.ClearDropDownList();
                    ddlEnquiryNumber.Clear();
                    tpPDDT.Visible = true;
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


    /// <summary>
    /// To populate asset status and inspected by values
    /// </summary>
    private void PopulateLookupDescriptions()
    {
        //throw new NotImplementedException();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();//Inspected By
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //ddlInspBy.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            ddlAgencyType.BindDataTable("S3G_ORG_GetInspectorsType", Procparam, new string[] { "Entity_Type_ID", "Entity_Type_Name" });

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunGetFIRTransDocDetatils()
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsRow ObjFIRTransDocMasterRow;
            ObjS3G_FIRTransactionDocDetails = new CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable();
            ObjFIRTransDocMasterRow = ObjS3G_FIRTransactionDocDetails.NewS3G_ORG_FIRTransactionDocDetailsRow();
            ObjFIRTransDocMasterRow.Field_Information_Report_ID = Convert.ToInt32(ViewState["FIRTRANS_ID"]);
            ObjFIRTransDocMasterRow.Company_ID = intCompanyID;
            ObjS3G_FIRTransactionDocDetails.AddS3G_ORG_FIRTransactionDocDetailsRow(ObjFIRTransDocMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;

            byte[] byteFIRTransDocDetails = ObjMgtCreditMgtClient.FunPubGetFIRTransDoc(SerMode, ClsPubSerialize.Serialize(ObjS3G_FIRTransactionDocDetails, SerMode));
            ObjS3G_FIRTransactionDocDetails = (CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable)ClsPubSerialize.DeSerialize(byteFIRTransDocDetails, SerMode, typeof(CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable));

            Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            dictParam1.Add("@Field_Information_Report_ID", Convert.ToString(ViewState["FIRTRANS_ID"]));
            dictParam1.Add("@Company_ID", intCompanyID.ToString());
            dictParam1.Add("@Doc_Type", Convert.ToString(ViewState["DocType"]));
            dictParam1.Add("@Doc_Ref_ID", Convert.ToString(ViewState["DocRef"]));
            DataTable dtIsDetails = Utility.GetDefaultData("S3G_ORG_GetFIRTransDocIsDetails", dictParam1);
            DataRow dtRow1 = dtIsDetails.Rows[0];
            ViewState["dtIsDetails"] = dtIsDetails;

            if (ObjS3G_FIRTransactionDocDetails.Rows.Count > 0)
            {
                ViewState["Docpath"] = ObjS3G_FIRTransactionDocDetails.Rows[0]["ViewDoc"].ToString();
            }
            gvPRDDT.DataSource = ObjS3G_FIRTransactionDocDetails;// Modify 
            gvPRDDT.DataBind();

            for (int i = 0; i < gvPRDDT.Rows.Count; i++)
            {
                if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    int FIR_Doc_Cat_ID = Convert.ToInt32(gvPRDDT.DataKeys[i]["FIR_Doc_Cat_ID"].ToString());
                    CheckBox Cbx1 = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");
                    Label txtScanBy = (Label)gvPRDDT.Rows[i].FindControl("lblScannedBy");
                    TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                    Label txtcollBy = (Label)gvPRDDT.Rows[i].FindControl("lblCollectedBy");
                    TextBox txtcolldate = (TextBox)gvPRDDT.Rows[i].FindControl("txtCollectedDate");
                    TextBox txtScanRef = (TextBox)gvPRDDT.Rows[i].FindControl("txtScan");
                    ImageButton Viewdoct = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                    TextBox txtRemarks = (TextBox)gvPRDDT.Rows[i].FindControl("txtRemarks");
                    Label lblDesc = (Label)gvPRDDT.Rows[i].FindControl("lblDesc");
                    TextBox txOD = (TextBox)gvPRDDT.Rows[i].FindControl("txOD");
                    TextBox txtScan = (TextBox)gvPRDDT.Rows[i].FindControl("txtScan");
                    Label lblColUser = (Label)gvPRDDT.Rows[i].FindControl("lblColUser");
                    Label lblPath = (Label)gvPRDDT.Rows[i].FindControl("lblPath");
                    Label myThrobber = (Label)gvPRDDT.Rows[i].FindControl("myThrobber");
                    HiddenField hidThrobber = (HiddenField)gvPRDDT.Rows[i].FindControl("hidThrobber");


                    if (lblPath.Text.Trim() == ViewState["Docpath"].ToString().Trim())
                        Viewdoct.Enabled = false;

                    AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");

                    Label lblType = (Label)gvPRDDT.Rows[i].FindControl("lblType");

                    Cbx1.Checked = false;
                    if (FIR_Doc_Cat_ID == Convert.ToInt32(ObjS3G_FIRTransactionDocDetails.Rows[i]["FIR_Doc_Cat_ID"].ToString()))
                    {
                        txtScanBy.Text = Convert.ToString(ObjS3G_FIRTransactionDocDetails.Rows[i]["scandedby"]);
                        txtScanDate.Text = Convert.ToDateTime(ObjS3G_FIRTransactionDocDetails.Rows[i]["Scanned_Date"].ToString()).ToString(strDateFormat);
                        txtcolldate.Text = Convert.ToDateTime(ObjS3G_FIRTransactionDocDetails.Rows[i]["Collected_Date"].ToString()).ToString(strDateFormat);
                        //txtcollBy.Text = Convert.ToString(ObjS3G_ORG_PRDDCTransDocMasterDataTable.Rows[i]["Collectedby"]);
                        //if (txtScanDate.Text == "")
                        //{
                        //    Cbx1.Checked = false;
                        //}
                        maxversion = Convert.ToInt16(ObjS3G_FIRTransactionDocDetails.Rows[i]["Version_No"]);

                        //chkbox = Convert.ToBoolean(ObjS3G_ORG_PRDDCTransDocMasterDataTable.Rows[i]["PRDDTrans"]);
                        if (Convert.ToBoolean(ObjS3G_FIRTransactionDocDetails.Rows[i]["FIRTrans"]) == true)
                        {
                            Cbx1.Checked = true;
                            Cbx1.Enabled = false;

                            txtcollBy.Text = Convert.ToString(ObjS3G_FIRTransactionDocDetails.Rows[i]["CollectedBy"]);
                        }
                        else
                        {
                            txtcollBy.Text = ObjUserInfo.ProUserNameRW;
                            lblColUser.Text = "";
                        }

                        MaxVerChk.Value += maxversion + "@@" + chkbox;

                        if (txtcolldate.Text == "")
                        {
                            txtcolldate.Text = "";
                            Cbx1.Checked = false;
                        }
                        MaxVerChk.Value += "@@" + txtScanDate.Text;
                        MaxVerChk.Value += "@@" + txtcolldate.Text;
                        txtScanRef.Text = Convert.ToString(ObjS3G_FIRTransactionDocDetails.Rows[i]["Scanned_Ref_No"]);
                        txtRemarks.Text = Convert.ToString(ObjS3G_FIRTransactionDocDetails.Rows[i]["Remarks"]);
                        MaxVerChk.Value += "@@" + txtRemarks.Text;

                        if (lblPath.Text.Trim() != ViewState["Docpath"].ToString().Trim())
                        {
                            string[] Path = Convert.ToString(ObjS3G_FIRTransactionDocDetails.Rows[i]["Scanned_Ref_No"]).Split('/');
                            hidThrobber.Value = Path[Path.Length - 1].ToString();
                            myThrobber.Text = Path[Path.Length - 1].ToString();
                            txOD.Text = Convert.ToString(ObjS3G_FIRTransactionDocDetails.Rows[i]["Scanned_Ref_No"]);
                        }
                        else
                        {
                            txtScanBy.Text = ObjUserInfo.ProUserNameRW;
                        }
                    }

                    if (ObjS3G_FIRTransactionDocDetails.Rows.Count == dtIsDetails.Rows.Count)
                    {
                        if (dtIsDetails.Rows[i]["Is_NeedScanCopy"].ToString() == "False")// && dtIsDetails.Rows[i]["Is_Mandatory"].ToString() == "False")
                        {
                            txtScanDate.Enabled = txtScanBy.Enabled = false;
                            txtScanDate.Text = "";
                            txtScanBy.Text = "";
                            Viewdoct.Enabled = false;
                            asyFileUpload.Enabled = false;
                            //myThrobber.Text = "";
                            //Cbx1.Enabled = false;
                        }
                    }
                    MaxVerChk.Value += "~~~";
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
            //if (objPRDDT_MasterClient != null)
            ObjMgtCreditMgtClient.Close();
        }
    }

    private void FunPriLoadCombos()
    {
        try
        {
            if (PageMode == PageModes.Create)
            {

                FunGetDocType();
                FunGetLOB();
                FunPriGetBranchList();
                //FunPriGetEnquiryResponse(); 
                FunPriLoadCombo_CurrencyMaster();
            }

            BindDDL(_StatusSource, ddlStatus);
            BindDDL(_ClientNetWorth, ddlClientNetWorth);
            BindDDL(_ClientCreditibility, ddlClientCreditability);

            txtResspondedDate.Text = txtResspondedDate.Text = txtRequestDate.Text = txtFIRDate.Text = System.DateTime.Now.ToString(strDateFormat);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public bool CheckForUTPA()
    {
        try
        {
            if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public void FunToggleRoundField(bool CanShowDDL)
    {
        try
        {
            ddlRound.Visible = CanShowDDL;
            txtRound.Visible = !CanShowDDL;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void FunPriControlStatus(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    TPRIR.Enabled = false;
                    FunToggleRoundField(false);
                    btnCancel.Enabled = false;

                    if (!bCreate)
                    {
                        ddlEnquiryNumber.Clear();
                        FunToggleButtons(false);
                    }
                    tpPDDT.Enabled = true;
                    gvPRDDT.Visible = true;
                    break;

                case 1: //Modify

                    FunToggleRoundField(false);
                    //ddlEnquiryNumber.Enabled = false;
                    ddlEnquiryNumber.ReadOnly = true;
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }

                    if (!CheckForUTPA())
                    {
                        TPRIR.Enabled = false;
                    }
                    else
                    {
                        FunToggleButtons(false);
                    }

                    if (ddlStatus.SelectedValue == "1")
                    {
                        FunToggleButtons(false);
                        if (CheckForUTPA())
                        {
                            Utility.FunShowAlertMsg(this, "No new request is available to Respond");
                            FunDesableRespondControls(false);
                            btnRespSave.Enabled = false;
                            btnRespClear.Enabled = false;
                            btnClear.Enabled = false;

                        }
                        else
                        {
                            if (Convert.ToInt32(ddlAgencyCode.SelectedValue) == ObjUserInfo.ProUserIdRW)
                            {
                                //btnClear.Enabled = true;
                            }
                            else
                            {
                                Utility.FunShowAlertMsg(this, "Cannot modify responded Field Information Report");
                            }
                        }
                    }
                    //condition to verify that logged_in user and the assigned_to user are same - enable respond tab
                    if (Convert.ToInt32(ddlAgencyCode.SelectedValue) == ObjUserInfo.ProUserIdRW)
                    {
                        TPRIR.Enabled = true;
                        FunToggleButtons(false);
                    }
                    else
                    {
                        if (!CheckForUTPA())
                        {
                            btnCancel.Enabled = true;
                        }
                        else
                        {
                            txtResspondedDate.Text = DateTime.Now.ToString(strDateFormat);
                        }
                    }

                    if (chkCancelled.Checked)
                    {
                        Utility.FunShowAlertMsg(this, "Cannot Modify/Update cancelled Field Information Report");
                        FunToggleButtons(false);
                        FunDesableRespondControls(false);
                        btnRespSave.Enabled = false;
                        btnRespClear.Enabled = false;
                        btnCancel.Enabled = false;
                    }

                    break;
                case -1://Query
                    {
                        if (!bQuery)
                        {
                            Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=FEIR");
                        }

                        FunToggleRoundField(true);
                        ddlEnquiryNumber.ReadOnly = true;
                        //ddlEnquiryNumber.Enabled =
                        ddlAgencyCode.Enabled = false;
                        //imgRequestDate.Visible =
                        //cexDate1.Enabled = false;
                        ddlAgencyType.Enabled = false;
                        FunDesableRespondControls(false);

                        txtTerms.ReadOnly = txtFieldRequest.ReadOnly = true;
                        txtSendEmail.ReadOnly = true;

                        btnClear.Enabled = btnSave.Enabled =
                            btnRespClear.Enabled =
                            btnRespSave.Enabled = false;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    public void FunDisableFIRControlsForSameUser(bool CanEnable)
    {

    }
    //protected void ddlDocValue_SelectedIndexChanged(object sender, EventArgs e)
    //{


    //}
    public void FunDesableRespondControls(bool CanEnable)
    {
        try
        {
            txtRespondedBy.ReadOnly = txtValue.ReadOnly =
                       txtResponseDesgn.ReadOnly = txtEmailRes.ReadOnly = txtMobile.ReadOnly =
                       txtFieldRespond.ReadOnly = !CanEnable; //imgRespondedDate.Visible = !CanEnable;
            //txtResspondedDate_CalendarExtender.Enabled = CanEnable;

            ddlClientCreditability.Enabled =
                                 ddlClientNetWorth.Enabled =
                                 chkCancelled.Enabled = CanEnable;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// Will Load the Document Number Combo Box with FIR Number
    /// </summary>
    private void FunPriLoadCombo_CurrencyMaster()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Currency_ID", "0");
            Procparam.Add("@IS_Active", "1");
            ddlCurrency.BindDataTable("S3G_Get_Currency_Details", Procparam, new string[] { "Currency_ID", "Currency_Code", "Currency_Name" });

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            DataTable CurDT = new DataTable();
            CurDT = Utility.GetDefaultData("S3G_Get_Currency_Details_ByCompanyID", Procparam);

            if (CurDT.Rows.Count > 0)
            {
                ddlCurrency.SelectedValue = CurDT.Rows[0]["Currency_ID"].ToString();
                txtCurrency.Text = CurDT.Rows[0]["Currency"].ToString();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Set the Currency in Global Parameter setup");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "window.location.href='../Origination/S3GORGTransLander.aspx?Code=FEIR';", true);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void SetDefaultScreen(bool IsClear)
    {
        try
        {

            if (IsClear == false)
            {

                FunGetLOB();
                FunPriGetBranchList();
                FunGetDocType();
                //FunPriGetEnquiryResponse();

                BindDDL(_StatusSource, ddlStatus);
                BindDDL(_ClientNetWorth, ddlClientNetWorth);
                BindDDL(_ClientCreditibility, ddlClientCreditability);
                txtResspondedDate.Text = txtResspondedDate.Text = txtRequestDate.Text = txtFIRDate.Text = System.DateTime.Now.ToString(strDateFormat);
            }

            txtRequestBy.Text = ObjUserInfo.ProUserNameRW;//(Session["userid"].ToString());
            //imgRequestDate.Visible = false;

            btnGeneratePdf.Enabled = false;
            btnClear.Enabled = false;
            chkCancelled.Checked = false;
            ddlLOB.SelectedIndex = 0;
            txtLOB.Text = "";
            ddlStatus.SelectedIndex = 0;
            ddlBranch.SelectedIndex = 0;
            txtBranch.Text = "";
            ddlAgencyCode.SelectedIndex = 0;
            txtRequestBy.Text = "";
            txtWebsite.Text = "";
            txtAgencyNAmeAddress.Text = "";
            txtAgencyName.Text = "";
            txtRequestBy.Enabled =
             true;

            txtRequestDate.Text = System.DateTime.Now.ToString(strDateFormat);
            txtCustomerCode.Text = "";
            txtCustNameAddress.Text = "";
            txtTerms.Text = "";
            txtFIR.Text = "";
            txtAgencyNAmeAddress.Text = "";
            txtAgencyName.Text = "";
            txtFieldRequest.Text = "";
            txtSendEmail.Text = "";
            txtCustomerName.Text =
            txtCustomerMobile.Text =
            txtEmailCust.Text = "";
            txtAgencyNAmeAddress.Text = "";
            txtAgencyName.Text = "";
            S3GCustomerAddress1.ClearCustomerDetails();
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriGetEntityMaster()
    {
        try
        {
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();

            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue.ToString());

            if (ddlAgencyCode != null)
                ddlAgencyCode.Items.Clear();
            //Changed based on UAT
            //ddlAgencyCode.BindDataTable("S3G_ORG_GetUTPAField_Surveyor", Procparam, new string[] { "Entity_Master_ID", "Entity_Code" });

            ddlAgencyCode.BindDataTable("S3G_ORG_GetEntityField_Surveyor", Procparam, new string[] { "Entity_Master_ID", "Entity_Code" });
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private DataTable FunPriFilterResponseByLOBandBranch(DataTable ObjER)
    {
        try
        {
            DataTable dtLOB = new DataTable();
            DataTable dtBranch = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@UTPA_ID", intUserID.ToString());
                Procparam.Add("@Is_Active", "1");

                dtLOB = Utility.GetDefaultData("S3G_Get_UTPA_LOB_LIST", Procparam);
                dtBranch = Utility.GetDefaultData("S3G_Get_UTPA_Branch_List", Procparam);
            }
            else
            {
                Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@User_ID", Convert.ToString(intUserID));
                Procparam.Add("@Is_Active", "1");
                Procparam.Add("@Program_ID", "33");
                dtBranch = Utility.GetDefaultData(SPNames.BranchMaster_LIST, Procparam);
                // LOB ComboBoxLOBSearch
                dtLOB = Utility.GetDefaultData(SPNames.LOBMaster, Procparam);
            }
            DataColumn dc = new DataColumn("Check");
            ObjER.Columns.Add(dc);
            for (int i_lob = 0; i_lob < dtLOB.Rows.Count; i_lob++)
            {
                for (int i_ObjER = 0; i_ObjER < ObjER.Rows.Count; i_ObjER++)
                {
                    if (string.Compare(dtLOB.Rows[i_lob]["LOB_ID"].ToString(), ObjER.Rows[i_ObjER]["LOB_ID"].ToString()) == 0)
                    {

                        ObjER.Rows[i_ObjER]["Check"] = "1";
                    }
                }
            }

            ObjER.DefaultView.RowFilter = "Check = 1";
            ObjER = ObjER.DefaultView.ToTable();

            // branch
            for (int i_lob = 0; i_lob < dtBranch.Rows.Count; i_lob++)
            {
                for (int i_ObjER = 0; i_ObjER < ObjER.Rows.Count; i_ObjER++)
                {
                    if (string.Compare(dtBranch.Rows[i_lob]["Location_Id"].ToString(), ObjER.Rows[i_ObjER]["Location_Id"].ToString()) == 0)
                    {

                        ObjER.Rows[i_ObjER]["Check"] = "2";
                    }
                }
            }

            ObjER.DefaultView.RowFilter = "Check = 2";
            ObjER = ObjER.DefaultView.ToTable();


        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        return ObjER;

    }

    private void FunPriGetEnquiryResponse()
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            //if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0 || string.Compare("M", Request.QueryString.Get("qsMode")) == 0)
            //{
            CreditMgtServices.S3G_ORG_EnquiryResponseRow ObjEnquiryResponseRow;

            ObjEnquiryResponseRow = ObjS3G_EnquiryResponse.NewS3G_ORG_EnquiryResponseRow();
            ObjS3G_EnquiryResponse.AddS3G_ORG_EnquiryResponseRow(ObjEnquiryResponseRow);

            //ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;

            byte[] bytesEnquiryResponseDetails = ObjMgtCreditMgtClient.FunPubQueryEnquiryResponseAllRows(SerMode, ClsPubSerialize.Serialize(ObjS3G_EnquiryResponse, SerMode));

            ObjS3G_EnquiryResponse = (CreditMgtServices.S3G_ORG_EnquiryResponseDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryResponseDetails, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_EnquiryResponseDataTable));

            DataTable ObjS3G_EnquiryResponse1 = FunPriFilterResponseByLOBandBranch(ObjS3G_EnquiryResponse);

            //ObjS3G_EnquiryResponse1 = FunPriFilterForUser(ObjS3G_EnquiryResponse1);
            //ddlEnquiryNumber.DataSource = ObjS3G_EnquiryResponse1;
            //ddlEnquiryNumber.DataValueField = "Enquiry_Response_ID";
            //ddlEnquiryNumber.DataTextField = "Enquiry_No";
            // ddlEnquiryNumber.DataBind();

            //System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            // ddlEnquiryNumber.Items.Insert(0, liSelect);


            //}
            //else
            //{
            //    Procparam = new Dictionary<string, string>(); 
            //    Procparam.Add("@Company_ID", intCompanyID.ToString());
            //    DataTable DtResponse = Utility.GetDefaultData("S3G_ORG_GetFIR_Enquiry_Response", Procparam);
            //    DtResponse = FunPriFilterResponseByLOBandBranch(DtResponse);

            //    ddlEnquiryNumber.DataSource = DtResponse;
            //    ddlEnquiryNumber.DataValueField = "Enquiry_Response_ID";
            //    ddlEnquiryNumber.DataTextField = "Enquiry_No";
            //    ddlEnquiryNumber.DataBind();

            //    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            //    ddlEnquiryNumber.Items.Insert(0, liSelect);
            //}

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();
        }
    }

    private DataTable FunPriFilterForUser(DataTable dtEr)
    {
        try
        {
            DataTable dtLOB = (DataTable)ddlLOB.DataSource;
            DataTable dtBranch = (DataTable)ddlBranch.DataSource;

            DataColumn dctemp = new DataColumn("LevelCheck");
            dtEr.Columns.Add(dctemp);

            // lob
            if (dtLOB != null)
            {
                for (int i_lob = 0; i_lob < dtLOB.Rows.Count; i_lob++)
                {
                    for (int i_er = 0; i_er < dtEr.Rows.Count; i_er++)
                    {
                        if ((string.Compare(dtEr.Rows[i_er]["LOB_ID"].ToString(), dtLOB.Rows[i_lob]["LOB_ID"].ToString())) == 0)
                        {
                            dtEr.Rows[i_er]["LevelCheck"] = "1";
                        }
                    }
                }
            }

            // branch
            if (dtBranch != null)
            {
                for (int i_Branch = 0; i_Branch < dtBranch.Rows.Count; i_Branch++)
                {
                    for (int i_er = 0; i_er < dtEr.Rows.Count; i_er++)
                    {
                        if ((string.Compare(dtEr.Rows[i_er]["Location_ID"].ToString(), dtBranch.Rows[i_Branch]["Location_ID"].ToString())) == 0)
                        {
                            dtEr.Rows[i_er]["LevelCheck"] = "1";
                        }
                    }
                }
            }

            dtEr.DefaultView.RowFilter = "LevelCheck = 1";
            dtEr = dtEr.DefaultView.ToTable();
            dtEr.Columns.Remove("LevelCheck");


        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        return dtEr;
    }

    private void FunPriGetBranchList()
    {
        try
        {
            // branch
            if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@UTPA_ID", intUserID.ToString());

                // Commanded the IS_Active , because of avoid the active / inactive bug 
                //Procparam.Add("@Is_Active", "1");

                ddlBranch.BindDataTable("S3G_Get_UTPA_Branch_List", Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            }
            else
            {
                Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@User_ID", Convert.ToString(intUserID));

                // Commanded the IS_Active , because of avoid the active / inactive bug 
                //Procparam.Add("@Is_Active", "1");


                Procparam.Add("@Program_Id", "33");
                ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    private void FunGetLOB()
    {
        try
        {
            // LOB ComboBoxLOBSearch

            if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@UTPA_ID", intUserID.ToString());
                //Procparam.Add("@Is_Active", "1");
                Procparam.Add("@Program_Id", "33");
                ddlLOB.BindDataTable("S3G_Get_UTPA_LOB_LIST", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            }
            else
            {
                Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@User_ID", Convert.ToString(intUserID));
                //  Procparam.Add("@Is_Active", "1");
                Procparam.Add("@Program_Id", "33");
                ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void FunGetDocType()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            ddlDocType.BindDataTable("S3G_ORG_GetRefDocType", Procparam, new string[] { "LookUp_ID", "Name", });


        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void FunPriGetApplicationNo()
    {
        try
        {
            //Procparam.Clear();
            //Procparam.Add("@Company_id", Convert.ToString(intCompanyID));
            //ddlEnquiryNumber.BindDataTable("S3G_ORG_GetApplicationNumberForFIR", Procparam, new string[] { "ID", "AppNumber", });

        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    /// <summary>
    /// Will bind the string[] datasource to the dropdown list, with dropdown value 0,1,2,...,(string[].length-1)
    /// </summary>
    /// <param name="Coll">string[] collection</param>
    /// <param name="ddl">DropDownList</param>
    private void BindDDL(string[] Coll, DropDownList ddl)
    {
        try
        {

            if ((ddl != null) && (Coll != null) && (Coll.Length > 0))
            {
                ddl.Items.Clear();
                // if record exists in the source

                System.Web.UI.WebControls.ListItem statusSourceItem;
                for (int i = 0; i < Coll.Length; i++)
                {
                    statusSourceItem = new System.Web.UI.WebControls.ListItem();
                    statusSourceItem.Text = Convert.ToString(Coll[i]);    // text 
                    statusSourceItem.Value = Convert.ToString(i);     // value
                    ddl.Items.Add(statusSourceItem); // adding the item to the list
                    statusSourceItem = null;
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlAgencyCode.SelectedValue) == ObjUserInfo.ProUserIdRW || ObjUserInfo.ProUserTypeRW.ToUpper().Contains("UTPA"))
        {
            if (PageMode == PageModes.Modify)
            {
                rfvCashflowDesc.Enabled =
                RequiredFieldValidator4.Enabled =
                RequiredFieldValidator5.Enabled =
                RequiredFieldValidator8.Enabled =
                RequiredFieldValidator7.Enabled =
                RequiredFieldValidator6.Enabled =
                RequiredFieldValidator9.Enabled = true;
            }
        }
        else
        {
            rfvCashflowDesc.Enabled =
                 RequiredFieldValidator4.Enabled =
                 RequiredFieldValidator5.Enabled =
                 RequiredFieldValidator8.Enabled =
                 RequiredFieldValidator7.Enabled =
                 RequiredFieldValidator6.Enabled =
                 RequiredFieldValidator9.Enabled = false;
        }
        string strPRDDT_No = string.Empty;
        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=FEIR';";
        string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgFieldInformationReport_Add.aspx';";
        CreditMgtServicesReference.CreditMgtServicesClient objFIR = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            //FIR TRANS
            DataTable dt1 = new DataTable();

            FIRTRANS_ID = Convert.ToInt32(ViewState["FIRTRANS_ID"]);
            if (FIRTRANS_ID > 0)
            {
                dt1 = (DataTable)ViewState["dtIsDetails"];
            }
            else
            {
                dt1 = (DataTable)ViewState["dtPRDDTypeTrans"];
                if ((DataTable)ViewState["dtPRDDTypeTrans"] == null)
                {
                    lblErrorMessage.Text = "";
                    Utility.FunShowAlertMsg(this, "Document details not defined in FIR Master");
                    strRedirectPageView = strRedirectPageAdd;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                    return;
                }
            }
            //Changed By Palani Kumar.A on 21/01/2014 for Product Features
            string strddlEnquiryNumber = string.Empty;
            if (PageMode == PageModes.Create)
            {                
                if (Convert.ToInt32(ddlDocType.SelectedValue) != 79 && ddlEnquiryNumber.SelectedValue.ToString() != "0")
                    strddlEnquiryNumber = ddlEnquiryNumber.SelectedText.Substring(0, ddlEnquiryNumber.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();
                else
                    strddlEnquiryNumber = ddlEnquiryNumber.SelectedText.ToString(); 
            }
            else
            {
                strddlEnquiryNumber = ddlEnquiryNumber.SelectedText.ToString(); 
            }
            
            int counts = 0;
            int Length = gvPRDDT.Rows.Count;

            for (int i = 0; i < gvPRDDT.Rows.Count; i++)
            {
                if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");

                    //Label txtScanDate = (Label)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                    ImageButton hyplnkView = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                    AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");
                    CheckBox CbxCheck = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");
                    Label lblType = (Label)gvPRDDT.Rows[i].FindControl("lblType");
                    Label lblProgramName = (Label)gvPRDDT.Rows[i].FindControl("lblProgramName");
                    HiddenField hidThrobber = (HiddenField)gvPRDDT.Rows[i].FindControl("hidThrobber");
                    Label myThrobber = (Label)gvPRDDT.Rows[i].FindControl("myThrobber");
                    //asyFilepath.Text = asyFileUpload.FileName;

                    UserControls_S3GAutoSuggest ddlCollectedBy = gvPRDDT.Rows[i].FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;

                    // DropDownList ddlScannedBy = gvPDDT.Rows[i].FindControl("ddlScannedBy") as DropDownList;
                    UserControls_S3GAutoSuggest ddlScannedBy = gvPRDDT.Rows[i].FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;


                    if (lblProgramName.Text.Trim() != "")
                    {
                        if (dt1.Rows[i]["Is_Mandatory"].ToString() == "True" && CbxCheck.Checked == false)
                        {
                            Utility.FunShowAlertMsg(this, "FIR Document has to be collected for document type - " + lblType.Text.Trim().ToUpper());
                            return;
                        }
                        if (dt1.Rows[i]["Is_Mandatory"].ToString() == "True" && dt1.Rows[i]["Is_NeedScanCopy"].ToString() == "True")
                        {
                            if (hidThrobber.Value.Trim() == "")
                            {
                                Utility.FunShowAlertMsg(this, "FIR Document has to be scanned for document type - " + lblType.Text.Trim().ToUpper());
                                return;
                            }
                        }
                    }

                    if (hidThrobber.Value.Trim() != "")
                    {
                        string fileExtension = asyFileUpload.FileName.Substring(asyFileUpload.FileName.LastIndexOf('.') + 1);
                        if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                        {
                            lblErrorMessage.Text = "File extension not supported, only image & pdf files should be uploaded.";

                            return;
                        }
                    }

                    if (CbxCheck.Checked) counts++;
                }
            }

            int intRowindex = 0;
            foreach (GridViewRow grv in gvPRDDT.Rows)
            {
                TextBox txOD = grv.FindControl("txOD") as TextBox;

                TextBox txtCollectedDate = grv.FindControl("txtCollectedDate") as TextBox;
                TextBox txtScannedDate = grv.FindControl("txtScannedDate") as TextBox;
                TextBox txtScan = grv.FindControl("txtScan") as TextBox;
                TextBox txtRemark = grv.FindControl("txtRemarks") as TextBox;
                ImageButton HypLnk = (ImageButton)grv.FindControl("hyplnkView");
                HiddenField hidThrobber = (HiddenField)grv.FindControl("hidThrobber");

                UserControls_S3GAutoSuggest ddlCollectedBy = grv.FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;

                UserControls_S3GAutoSuggest ddlScannedBy = grv.FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;

                AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");

                string strPath = "";
                string strNewFileName = AsyncFileUpload1.FileName;                

                //string strEnqNumber = ddlEnquiryNumber.SelectedText.Replace("/", "-");
                string strEnqNumber = strddlEnquiryNumber.Replace("/", "-");

                if (AsyncFileUpload1.FileName != "" && hidThrobber.Value.Trim() != "")
                {
                    if (AsyncFileUpload1.HasFile)
                    {
                        if (ViewState["Docpath"].ToString() != "")
                        {
                            strPath = Path.Combine(ViewState["Docpath"].ToString(), "COMPANY" + intCompanyID.ToString() + "/" + strEnqNumber + "/" + "FIRT-" + intRowindex.ToString());
                            if (Directory.Exists(strPath))
                            {
                                Directory.Delete(strPath, true);
                            }
                            Directory.CreateDirectory(strPath);
                            strPath = strPath + "/" + strNewFileName;
                        }
                        txOD.Text = strPath;// txOD.Text + strEnqNumber + "\\" + "PDDTC-" + intRowindex.ToString() + "\\" + strNewFileName;

                        FileInfo f1 = new FileInfo(strPath);

                        if (f1.Exists == true)
                            f1.Delete();

                        AsyncFileUpload1.SaveAs(strPath);
                    }
                }

                intRowindex++;
            }
            //END
            CreditMgtServices.S3G_ORG_FieldInformationReportDataTable ObjS3G_ORG_FIRTable = new CreditMgtServices.S3G_ORG_FieldInformationReportDataTable();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportRow ObjS3G_ORG_FIRRow = ObjS3G_ORG_FIRTable.NewS3G_ORG_FieldInformationReportRow();

            // foreign key references.

            ObjS3G_ORG_FIRRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjS3G_ORG_FIRRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjS3G_ORG_FIRRow.Doc_Type = Convert.ToInt32(ddlDocType.SelectedValue);

            if (Convert.ToInt32(ddlAgencyCode.SelectedValue) == ObjUserInfo.ProUserIdRW || ObjUserInfo.ProUserTypeRW.ToUpper().Contains("UTPA"))
            {
                if (PageMode == PageModes.Modify)
                {
                    if (_FIRNo == "" || ViewState["ReqFlag"].ToString() == "0")
                    {
                        Utility.FunShowAlertMsg(this, "Field Information Details not assigned.");
                        return;
                    }
                    // this is to notify the end user that the request is responded by the third party
                    ddlStatus.SelectedValue = "1";
                    if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
                    {
                        if (ddlSanum.Visible == true)
                        {
                            if (ddlSanum.Items.Count == 1)
                            {
                                ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                            }
                            else
                            {
                                ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlSanum.SelectedValue);

                            }
                        }
                        else
                        {
                            ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                        }
                    }
                    else
                    {
                        ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                    }
                    if (ViewState["CustomerId"] != null)
                    {
                        ObjS3G_ORG_FIRRow.Customer_ID = Convert.ToInt32(ViewState["CustomerId"].ToString());
                    }
                    else
                    {
                        ObjS3G_ORG_FIRRow.Customer_ID = Convert.ToInt32(ViewState["Customer"].ToString());
                    }
                    ObjS3G_ORG_FIRRow.FIR_ID = Convert.ToInt32(ViewState["FIRID"]);
                    ObjS3G_ORG_FIRRow.Terms_Conditions = txtTerms.Text.Trim();
                    ObjS3G_ORG_FIRRow.FIR_No = _FIRNo;
                    ObjS3G_ORG_FIRRow.FIR_Date = System.DateTime.Now;//Convert.ToDateTime(txtFIRDate.Text);
                    if (ddlRound == null || ddlRound.SelectedIndex < 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select the Enquiry number - to which the FIR to be generated");
                        return;
                    }
                    ObjS3G_ORG_FIRRow.Round = ddlRound.SelectedItem.ToString();// txtRound.Text;
                    ObjS3G_ORG_FIRRow.Requested_By = txtRequestBy.Text.Trim();
                    ObjS3G_ORG_FIRRow.Requested_Date = Utility.StringToDate(txtRequestDate.Text);// Convert.ToDateTime(txtRequestDate.Text);
                    if ((ddlAgencyCode == null || ddlAgencyCode.SelectedValue == "0")
                        ||
                        (string.IsNullOrEmpty(txtTerms.Text)) || (string.IsNullOrEmpty(txtFieldRequest.Text)))
                    {
                        Utility.FunShowAlertMsg(this, "Enter Request Information Details.");
                        return;
                    }
                    ObjS3G_ORG_FIRRow.Company_ID = intCompanyID;
                    ObjS3G_ORG_FIRRow.Entity_ID = Convert.ToInt32(ddlAgencyCode.SelectedValue);
                    ObjS3G_ORG_FIRRow.Field_Request = txtFieldRequest.Text.Trim();
                    ObjS3G_ORG_FIRRow.Notification_Sent_by = txtSendEmail.Text.Trim();
                    ObjS3G_ORG_FIRRow.Cancelled = false;
                    ObjS3G_ORG_FIRRow.Created_By = ObjUserInfo.ProUserIdRW;
                    ObjS3G_ORG_FIRRow.Created_On = DateTime.Now;
                    ObjS3G_ORG_FIRRow.Modified_By = ObjUserInfo.ProUserIdRW;
                    ObjS3G_ORG_FIRRow.Modified_On = System.DateTime.Now;

                    //response
                    ObjS3G_ORG_FIRRow.FIR_No = _FIRNo;
                    ObjS3G_ORG_FIRRow.Responded_By = txtRespondedBy.Text.Trim();
                    if (Utility.StringToDate(txtResspondedDate.Text) < Utility.StringToDate(txtRequestDate.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Respond date should be greater than or equal to Requested Date");
                        return;
                    }
                    ObjS3G_ORG_FIRRow.Responded_Date = Utility.StringToDate(txtResspondedDate.Text);
                    ObjS3G_ORG_FIRRow.Response_Designation = txtResponseDesgn.Text.Trim();
                    ObjS3G_ORG_FIRRow.Field_Officer_EMailID = txtEmailRes.Text.Trim();
                    ObjS3G_ORG_FIRRow.Field_Officer_MobileNo = txtMobile.Text.Trim();
                    ObjS3G_ORG_FIRRow.Client_Credibility = ddlClientCreditability.SelectedItem.ToString();
                    ObjS3G_ORG_FIRRow.Value = txtValue.Text.Trim();
                    ObjS3G_ORG_FIRRow.Currency = ddlCurrency.SelectedValue.ToString();//
                    ObjS3G_ORG_FIRRow.Client_Net_Worth = ddlClientNetWorth.SelectedItem.ToString();
                    ObjS3G_ORG_FIRRow.Response = txtFieldRespond.Text.Trim();
                    ObjS3G_ORG_FIRRow.Status = _StatusSource[Convert.ToInt32(ddlStatus.SelectedValue.ToString())];
                    ObjS3G_ORG_FIRRow.XML_FIRDeltails = FunProFormMLAXML();
                    ObjS3G_ORG_FIRRow.AgencyType = Convert.ToInt32(ddlAgencyType.SelectedValue);
                }
                else
                {
                    if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
                    {
                        if (ddlSanum.Visible == true)
                        {
                            if (ddlSanum.Items.Count == 1)
                            {
                                ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                            }
                            else
                            {
                                ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlSanum.SelectedValue);

                            }
                        }
                        else
                        {
                            ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                        }
                    }
                    else
                    {
                        ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                    }
                    if (ViewState["CustomerId"] != null)
                    {
                        ObjS3G_ORG_FIRRow.Customer_ID = Convert.ToInt32(ViewState["CustomerId"].ToString());
                    }
                    else
                    {
                        ObjS3G_ORG_FIRRow.Customer_ID = Convert.ToInt32(ViewState["Customer"].ToString());
                    }
                    ObjS3G_ORG_FIRRow.Terms_Conditions = txtTerms.Text.Trim();

                    ObjS3G_ORG_FIRRow.FIR_ID = Convert.ToInt32(ViewState["FIRID"]);
                    if (ViewState["FIRNum"] != null)
                    {
                        ObjS3G_ORG_FIRRow.FIR_No = ViewState["FIRNum"].ToString();
                    }
                    else
                    {
                        ObjS3G_ORG_FIRRow.FIR_No = "";
                    }
                    ObjS3G_ORG_FIRRow.FIR_Date = DateTime.Now;// Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat));

                    //System.DateTime.Now;//Convert.ToDateTime(txtFIRDate.Text);
                    ObjS3G_ORG_FIRRow.Round = txtRound.Text;//ddlRound.SelectedItem.ToString();
                    ObjS3G_ORG_FIRRow.Requested_By = txtRequestBy.Text.Trim();
                    ObjS3G_ORG_FIRRow.Responded_Date = ObjS3G_ORG_FIRRow.Requested_Date = Utility.StringToDate(txtRequestDate.Text); //Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat));//System.DateTime.Now;// Convert.ToDateTime(txtRequestDate.Text);
                    ObjS3G_ORG_FIRRow.Entity_ID = Convert.ToInt32(ddlAgencyCode.SelectedValue);
                    ObjS3G_ORG_FIRRow.Field_Request = txtFieldRequest.Text.Trim();
                    ObjS3G_ORG_FIRRow.Notification_Sent_by = txtSendEmail.Text.Trim();
                    ObjS3G_ORG_FIRRow.Status = _StatusSource[Convert.ToInt32(ddlStatus.SelectedValue.ToString())];
                    ObjS3G_ORG_FIRRow.Cancelled = false;
                    ObjS3G_ORG_FIRRow.Company_ID = intCompanyID;
                    ObjS3G_ORG_FIRRow.Created_By = ObjUserInfo.ProUserIdRW;
                    ObjS3G_ORG_FIRRow.Created_On = DateTime.Now;//Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat));//System.DateTime.Now;
                    ObjS3G_ORG_FIRRow.Modified_By = ObjUserInfo.ProUserIdRW;
                    ObjS3G_ORG_FIRRow.Modified_On = DateTime.Now;
                    ObjS3G_ORG_FIRRow.XML_FIRDeltails = FunProFormMLAXML();
                    ObjS3G_ORG_FIRRow.AgencyType = Convert.ToInt32(ddlAgencyType.SelectedValue);
                }
            }
            else
            {
                if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
                {
                    if (ddlSanum.Visible == true)
                    {
                        if (ddlSanum.Items.Count == 1)
                        {
                            ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                        }
                        else
                        {
                            ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlSanum.SelectedValue);

                        }
                    }
                    else
                    {
                        ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                    }
                }
                else
                {
                    ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                }
                if (ViewState["CustomerId"] != null)
                {
                    ObjS3G_ORG_FIRRow.Customer_ID = Convert.ToInt32(ViewState["CustomerId"].ToString());
                }
                else
                {
                    ObjS3G_ORG_FIRRow.Customer_ID = Convert.ToInt32(ViewState["Customer"].ToString());
                }
                ObjS3G_ORG_FIRRow.Terms_Conditions = txtTerms.Text.Trim();

                ObjS3G_ORG_FIRRow.FIR_ID = Convert.ToInt32(ViewState["FIRID"]);
                if (ViewState["FIRNum"] != null)
                {
                    ObjS3G_ORG_FIRRow.FIR_No = ViewState["FIRNum"].ToString();
                }
                else
                {
                    ObjS3G_ORG_FIRRow.FIR_No = "";
                }
                ObjS3G_ORG_FIRRow.FIR_Date = DateTime.Now;// Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat));

                //System.DateTime.Now;//Convert.ToDateTime(txtFIRDate.Text);
                ObjS3G_ORG_FIRRow.Round = txtRound.Text;//ddlRound.SelectedItem.ToString();
                ObjS3G_ORG_FIRRow.Requested_By = txtRequestBy.Text.Trim();
                ObjS3G_ORG_FIRRow.Responded_Date = ObjS3G_ORG_FIRRow.Requested_Date = Utility.StringToDate(txtRequestDate.Text); //Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat));//System.DateTime.Now;// Convert.ToDateTime(txtRequestDate.Text);
                ObjS3G_ORG_FIRRow.Entity_ID = Convert.ToInt32(ddlAgencyCode.SelectedValue);
                ObjS3G_ORG_FIRRow.Field_Request = txtFieldRequest.Text.Trim();
                ObjS3G_ORG_FIRRow.Notification_Sent_by = txtSendEmail.Text.Trim();
                ObjS3G_ORG_FIRRow.Status = _StatusSource[Convert.ToInt32(ddlStatus.SelectedValue.ToString())];
                ObjS3G_ORG_FIRRow.Cancelled = false;
                ObjS3G_ORG_FIRRow.Company_ID = intCompanyID;
                ObjS3G_ORG_FIRRow.Created_By = ObjUserInfo.ProUserIdRW;
                ObjS3G_ORG_FIRRow.Created_On = DateTime.Now;//Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat));//System.DateTime.Now;
                ObjS3G_ORG_FIRRow.Modified_By = ObjUserInfo.ProUserIdRW;
                ObjS3G_ORG_FIRRow.Modified_On = DateTime.Now;
                ObjS3G_ORG_FIRRow.XML_FIRDeltails = FunProFormMLAXML();
                ObjS3G_ORG_FIRRow.AgencyType = Convert.ToInt32(ddlAgencyType.SelectedValue);
            }

            ObjS3G_ORG_FIRTable.AddS3G_ORG_FieldInformationReportRow(ObjS3G_ORG_FIRRow);

            // CreditMgtServicesReference.CreditMgtServicesClient objFIR = new CreditMgtServicesReference.CreditMgtServicesClient();
            SerializationMode SMode = SerializationMode.Binary;

            string FIRNumber_Out = string.Empty;
            int errcode = objFIR.FunPubCreateFIR(out FIRNumber_Out, SMode, ClsPubSerialize.Serialize(ObjS3G_ORG_FIRTable, SMode));

            if (errcode == 0)
            {

                if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler)                       
                {
                    try
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, WFValues.Document_Type);
                            strAlert = "";

                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow not Assigned";
                        }
                        ShowWFAlertMessage(FIRNumber_Out, ProgramCode, strAlert);
                        return;
                    }
                    catch
                    {
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strAlert = "Field Information Report " + FIRNumber_Out + " added successfully";
                        strAlert += @"\n\n And Job not assigned to the next user.";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";

                    }
                }
                else
                {
                    int intDoctype = 0;
                    switch (ddlDocType.SelectedValue)
                    {
                        case "79":
                            intDoctype = 1;
                            break;
                        case "80":
                            intDoctype = 2;
                            break;
                        case "81":
                            intDoctype = 3;
                            break;
                        case "82":
                            intDoctype = 4;
                            break;
                    }
                    DataTable WFFP = new DataTable();
                    if (CheckForForcePullOperation(null, strddlEnquiryNumber.Trim(), ProgramCode, null, null, "O", CompanyId, int.Parse(ddlLOB.SelectedValue), null, null, null, out WFFP))
                    {
                        DataRow dtrForce = WFFP.Rows[0];
                        int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), strddlEnquiryNumber.Trim(), int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), intDoctype);

                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here
                    }

                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    strAlert = "Field Information Report " + FIRNumber_Out + " added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";

                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else if (errcode == 1)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                Utility.FunShowAlertMsg(this, "Updated successfully");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "window.location.href='../Origination/S3GORGTransLander.aspx?Code=FEIR';", true);
            }
            else if (errcode == 12)
            {
                Utility.FunShowAlertMsg(this, "Document Number Control not specified");
                return;
            }
            else if (errcode == 13)
            {
                Utility.FunShowAlertMsg(this, "Document Number Control exceeded");
                return;
            }
            //FunPriInitControls();
        }
        catch (FaultException<CreditMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            Utility.FunShowAlertMsg(this, objFaultExp.Message);
            throw objFaultExp;
        }
        //catch (Exception ex)
        //{
        //    Utility.FunShowAlertMsg(this, ex.Message);
        //    throw ex;
        //}
        catch (Exception ex)
        {
            if (ex.Message.Contains("Access to the path") && ex.Message.Contains("denied"))
            {
                lblErrorMessage.Text = "File Path not formed well or Access to the path is denied";

                return;
            }
            else if (ex.Message.Contains("Could not find a part of the path"))
            {
                lblErrorMessage.Text = "File Path defined in FIR Document Master is not avilable";

                return;
            }
            else
            {
                lblErrorMessage.Text = "File Path defined in FIR Document Master is not avilable ";//ex.Message;

                return;
            }
        }
        finally
        {
            objFIR.Close();
        }
    }
    protected void txtFIR_TextChanged(object sender, EventArgs e)
    {

    }

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
            //string strDocValue = "";
            //if (((DropDownList)grvData.FindControl("ddlDocValue")).SelectedValue != "--Select--")
            //{
            //    strDocValue = ((DropDownList)grvData.FindControl("ddlDocValue")).SelectedValue;
            //}
            //else
            //{
            //    strDocValue = null;
            //}

            if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue != "0")
            {
                if (((Label)grvData.FindControl("lblCollectedBy")).Text != "")
                {
                    strCollectdBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
                    //strCollectdBy = ((Label)grvData.FindControl("lblCollectedBy")).Text;
                }
                else
                {
                    strCollectdBy = "";
                }
            }
            else
            {
                strCollectdBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
            }

            if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).Visible)
            {
                if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).SelectedValue != "0")
                {
                    if (((Label)grvData.FindControl("lblScannedBy")).Text != "")
                    {
                        strScanBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
                        //strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
                    }
                    else
                    {
                        strScanBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
                    }
                }
                else
                {
                    strScanBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).SelectedValue;
                }
            }
            else
            {
                strScanBy = "-1";
            }
            /*
            if (((Label)grvData.FindControl("lblColUser")).Text != "")
                strCollectdBy = ((Label)grvData.FindControl("lblColUser")).Text;
            else
                strCollectdBy = Convert.ToString(intUserID);

            if (((Label)grvData.FindControl("lblScanUser")).Text != "")
                strScanBy = ((Label)grvData.FindControl("lblScanUser")).Text;
            else
                strScanBy = Convert.ToString(intUserID);
            */

            string strCollectdDate = ((TextBox)grvData.FindControl("txtCollectedDate")).Text;
            string strScanDate = "";
            if (((TextBox)grvData.FindControl("txtScannedDate")).Visible)
            {
                strScanDate = ((TextBox)grvData.FindControl("txtScannedDate")).Text;
            }


            //string strCollectdDate = ((Label)grvData.FindControl("txtColletedDate")).Text;
            //string strScanDate = ((Label)grvData.FindControl("txtScannedDate")).Text;

            CheckBox CbxCheck = (CheckBox)grvData.FindControl("CbxCheck");
            AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)grvData.FindControl("asyFileUpload");

            if (FIRTRANS_ID != 0)
            {
                if (asyFileUpload.FileName.ToString() != "")
                {
                    strScanBy = Convert.ToString(intUserID);//((Label)grvData.FindControl("lblScanUser")).Text;// Convert.ToString(intUserId);
                    strScanDate = DateTime.Now.ToString(strDateFormat).ToString();
                }
            }

            string strScanRefNo = ((TextBox)grvData.FindControl("txOD")).Text;
            string strRemarks = ((TextBox)grvData.FindControl("txtRemarks")).Text.Replace("'", "\"").Replace(">", "").Replace("<", "").Replace("&", "");
            string strPRDDTrans = Convert.ToString(((CheckBox)grvData.FindControl("CbxCheck")).Checked);
            string[] temp;

            if (!string.IsNullOrEmpty(MaxVerChk.Value))
            {
                temp = Convert.ToString(temprow[rowcount]).Split('@', '@');
                maxversion = Convert.ToInt16(temp[0]);
                chkbox = Convert.ToBoolean(temp[2]);
                if (strPRDDTrans.ToLower() == Convert.ToString(chkbox).ToLower())
                {
                    //{ strVersionNo = Convert.ToString(maxversion);
                    // Counts = 0;
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
            strScanDate = strScanDate == string.Empty ? strScanDate : Utility.StringToDate(strScanDate).ToString();
            strCollectdDate = strCollectdDate == string.Empty ? strCollectdDate : Utility.StringToDate(strCollectdDate).ToString();//Utility.StringToDate(DateTime.Now.ToString(strDateFormat)).ToString()

            strbuXML.Append(" <Details  FIR_Doc_Cat_ID='" + strlblPRTID + "' Collected_By='" + strCollectdBy.ToString() + "' Collected_Date='" + strCollectdDate +
                "' Scanned_By='" + strScanBy.ToString() + "' Scanned_Date='" + strScanDate + "' Scanned_Ref_No='" + strScanRefNo.ToString() + "' Remarks='" + strRemarks.ToString() + "' FIRTrans='" + strPRDDTrans.ToString() + "' Version_No='" + "' Counts='" + Counts.ToString() + "'/>");
            rowcount = rowcount + 3;

        }
        string tem = "Version_No='" + strVersionNo + "'";
        strbuXML.Replace("Version_No=''", tem);
        strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }

    protected void TxtCustomerCode_TextChanged(object sender, EventArgs e)
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            txtCustNameAddress.Text = "";
            CreditMgtServices.S3G_ORG_CustomerMasterRow objCustomerRow;
            objCustomerRow = ObjS3G_CustomerMasterByCode.NewS3G_ORG_CustomerMasterRow();
            objCustomerRow.Customer_Code = txtCustomerCode.Text;
            ObjS3G_CustomerMasterByCode.AddS3G_ORG_CustomerMasterRow(objCustomerRow);
            //ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
            SerializationMode serMode = SerializationMode.Binary;
            byte[] bytesCustomerDetails = ObjMgtCreditMgtClient.FunPubQueryCustomerMasterByCode(serMode, ClsPubSerialize.Serialize(ObjS3G_CustomerMasterByCode, serMode));
            ObjS3G_CustomerMasterByCode = (CreditMgtServices.S3G_ORG_CustomerMasterDataTable)ClsPubSerialize.DeSerialize(bytesCustomerDetails, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_CustomerMasterDataTable));

            if (ObjS3G_CustomerMasterByCode != null)
            {
                if (ObjS3G_CustomerMasterByCode.Rows.Count > 0)
                {
                    // customer personal details.

                    txtCustNameAddress.Text =
                        ObjS3G_CustomerMasterByCode.Rows[0]["CustomerAddress"].ToString();

                    // customer FIR Details.
                    if (!(string.IsNullOrEmpty(ObjS3G_CustomerMasterByCode.Rows[0]["EnquiryResponse_Number"].ToString())))
                    {
                        _FIRNo = ObjS3G_CustomerMasterByCode.Rows[0]["FIR_No"].ToString();
                        txtFIR.Text = ObjS3G_CustomerMasterByCode.Rows[0]["FIR_No"].ToString() + "/" +
                            ObjS3G_CustomerMasterByCode.Rows[0]["Round"].ToString();
                        txtRequestBy.Text = ObjS3G_EnquiryResponse.Rows[0]["RequestedBy"].ToString();
                        txtTerms.Text = ObjS3G_CustomerMasterByCode.Rows[0]["Terms_Conditions"].ToString();
                        txtAgencyName.Text = ObjS3G_CustomerMasterByCode.Rows[0]["AgencyName"].ToString();
                        txtAgencyNAmeAddress.Text = ObjS3G_CustomerMasterByCode.Rows[0]["AgencyAddress"].ToString();

                        txtSendEmail.Text = ObjS3G_CustomerMasterByCode.Rows[0]["AgencyEmail"].ToString();
                        txtFieldRequest.Text = ObjS3G_CustomerMasterByCode.Rows[0]["FieldRequest"].ToString();
                        if (ObjS3G_EnquiryResponse != null && ObjS3G_EnquiryResponse.Rows.Count > 0)
                        {
                            ddlLOB.SelectedValue = ObjS3G_EnquiryResponse.Rows[0]["LOB_ID"].ToString();
                            txtLOB.Text = ddlLOB.SelectedItem.ToString();
                            ddlBranch.SelectedValue = ObjS3G_EnquiryResponse.Rows[0]["Location_ID"].ToString();
                            txtBranch.Text = ddlBranch.SelectedItem.ToString();
                        }
                        if (Convert.ToInt32(ObjS3G_CustomerMasterByCode.Rows[0]["Round"].ToString()) == 9)
                        {
                            Utility.FunShowAlertMsg(this, "Already the customer covers 9 rounds");
                            return;
                        }
                        else
                        {
                            int totalRound = ((Convert.ToInt32(ObjS3G_CustomerMasterByCode.Rows[0]["Round"].ToString())) + 1);
                            ddlRound.Items.Clear();
                            for (int i = 1; i <= totalRound; i++)
                            {
                                if (i > 9)
                                    break;
                                ddlRound.Items.Add(i.ToString());
                            }
                            ddlRound.SelectedIndex = (totalRound - 2);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();
        }

    }

    private string GetHTMLText()
    {
        string strhtmltext = "";
        try
        {

            string strbranch = "";
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0) strbranch = ddlBranch.SelectedItem.ToString();
            string strLOB = "";
            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0) strLOB = ddlLOB.SelectedItem.ToString();
            string strStatus = ddlStatus.SelectedItem.ToString();
            string strEnquiryNumber = "";

            if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
            {
                if (ddlSanum.Items.Count == 1)
                {
                    strEnquiryNumber = ddlEnquiryNumber.SelectedText.ToString();

                }
                else
                {
                    strEnquiryNumber = ddlSanum.SelectedItem.ToString();

                }
            }
            else
            {
                strEnquiryNumber = ddlEnquiryNumber.SelectedText;
            }


            if (9 > 5)



                strhtmltext = "<font size=\"2\"  color=\"black\" face=\"verdana\">" +
                   " <table align=\"center\" width=\"80%\">" +
                "<tr >" +
                    "<td colspan = \"2\" align=\"center\" >" +
                        "<font size=\"4\"  color=\"Black\" face=\"verdana\">" +
                       "<u> <b>Field Information Report</b> </u>" +
                          "</font> " +
                    "</td>" +
               " </tr>" +
               "<tr>" +
                      "<td colspan=\"2\" height=\"15px\">" +
                            "</td>" +
                        "</tr>" +
               " <tr>" +
                  "  <td  align=\"left\"  valign=\"top\"> " +
                      "  Line of Business:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        strLOB +
                   " </td>" +
                "</tr>" +
                "<tr>" +
                   " <td align=\"left\"  valign=\"top\">" +
                      " Location:" +
                    "</td>" +
                    "<td align=\"left\">" +

                     strbranch +
                   " </td>" +
               " </tr>" +

                " <tr>" +
                    "<td align=\"left\"  valign=\"top\"  valign=\"top\">" +
                      " Reference Number:" +
                    "</td>" +
                    "<td align=\"left\">" +
                       strEnquiryNumber +
                   " </td>" +
               " </tr>" +

                " <tr>" +
                    "<td align=\"left\"  valign=\"top\">" +
                      " Customer Code:" +
                    "</td>" +
                   " <td align=\"left\">" +
                       txtCustomerCode.Text +
                   " </td>" +
               " </tr>" +

                " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Customer Name & Address:" +
                   " </td>" +
                   " <td align=\"left\">" +
                       txtCustomerName.Text + ", " + txtCustNameAddress.Text +
                   " </td>" +
                "</tr>" +

            " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Field Request:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        txtFieldRequest.Text +
                   " </td>" +
              "</tr>" +
                " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Terms & Conditions:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        txtTerms.Text +
                   " </td>" +
              "</tr>" +
                  " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " FIR Number:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        ViewState["FIRNum"].ToString() +
                   " </td>" +
                "</tr>" +

                  " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " FIR Date:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        txtFIRDate.Text +
                   " </td>" +
              "</tr>" +


                  " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Round:" +
                   " </td>" +
                   " <td align=\"left\">" +

                        txtRound.Text +
                   " </td>" +
              "</tr>" +

                  " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Status:" +
                   " </td>" +
                   " <td align=\"left\">" +
                       strStatus +
                   " </td>" +
              "</tr>" +
                  " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Requested By:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        txtRequestBy.Text +
                   " </td>" +
              "</tr>" +

                  " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Requested Date:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        txtRequestDate.Text +
                   " </td>" +
              "</tr>" +

                  " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Agency Code:" +
                   " </td>" +
                   " <td align=\"left\">" +

                   ddlAgencyCode.SelectedItem.ToString() + " - " + txtAgencyName.Text +
                   " </td>" +
              "</tr>" +
                    " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Agency Name & Address:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        txtAgencyNAmeAddress.Text +
                   " </td>" +
              "</tr>" +
                     " <tr>" +
                   " <td align=\"left\" valign=\"top\">" +
                      " Send EMail:" +
                   " </td>" +
                   " <td align=\"left\">" +
                        txtSendEmail.Text +
                   " </td>" +
              "</tr>" +


            "</table>" + "</font>";
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        return strhtmltext;
    }

    // to redirect to the translander
    protected void btnCancelToTransLander_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=FEIR");
    }

    //want to flush the folder orgination/pdf files in Session_end
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopupRemarks.Show();
            txtRemarks.Focus();
        }
        catch (FaultException<CreditMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            Utility.FunShowAlertMsg(this, objFaultExp.Message);
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
            throw ex;
        }
    }

    protected void BtnMPCancel_Click(object sender, EventArgs e)
    {
        CreditMgtServicesReference.CreditMgtServicesClient objFIR = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            //if (txtRemarks.Text == string.Empty)
            //{
            //    Utility.FunShowAlertMsg(this,"Enter remarks");
            //    ModalPopupRemarks.Show();
            //    txtRemarks.Focus();
            //    return;
            //}

            CreditMgtServices.S3G_ORG_FieldInformationReportDataTable ObjS3G_ORG_FIRTable = new CreditMgtServices.S3G_ORG_FieldInformationReportDataTable();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportRow ObjS3G_ORG_FIRRow = ObjS3G_ORG_FIRTable.NewS3G_ORG_FieldInformationReportRow();
            ObjS3G_ORG_FIRRow.FIR_No = ViewState["FIRNum"].ToString();
            ObjS3G_ORG_FIRRow.Field_Request = txtRemarks.Text;
            ObjS3G_ORG_FIRRow.Modified_By = intUserID;

            ObjS3G_ORG_FIRTable.AddS3G_ORG_FieldInformationReportRow(ObjS3G_ORG_FIRRow);

            //CreditMgtServicesReference.CreditMgtServicesClient objFIR = new CreditMgtServicesReference.CreditMgtServicesClient();
            SerializationMode SMode = SerializationMode.Binary;
            objFIR.FunPubUpdateFIRCancel(SMode, ClsPubSerialize.Serialize(ObjS3G_ORG_FIRTable, SMode));

            // Modified On 13/Mar/2012 by Senthilkumar P for Bug Id -- 6046 in Oracle
            Utility.FunShowAlertMsg(this, "Cancelled Successfully");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "window.location.href='../Origination/S3GORGTransLander.aspx?Code=FEIR';", true);
            chkCancelled.Checked = true;
        }
        catch (FaultException<CreditMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            Utility.FunShowAlertMsg(this, objFaultExp.Message);
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
            throw ex;
        }
        finally
        {
            objFIR.Close();
        }
    }

    // letter preview - generate pdf.
    protected void btnGenereatePDFpreview_Click(object sender, EventArgs e)
    {



    }

    protected void PreviewPDF_Click(object sender, EventArgs e)
    {

        try
        {
            String htmlText = GetHTMLText();
            string FIRNum = "";
            FIRNum = ViewState["FIRNum"].ToString();
            if ((string.IsNullOrEmpty(FIRNum)) || FIRNum == "")
            {
                FIRNum = DateTime.Now.ToString() + "/";
            }
            string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + FIRNum.Replace("/", "").Replace(" ", "").Replace(":", "") + ".pdf");
            string strFileName = "/Origination/PDF Files/" + FIRNum.Replace("/", "").Replace(" ", "").Replace(":", "") + ".pdf";
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));
            doc.AddCreator("Sundaram Infotech Solutions");
            doc.AddTitle("New PDF Document");
            doc.Open();
            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
            for (int k = 0; k < htmlarraylist.Count; k++)
            {
                doc.Add((IElement)htmlarraylist[k]);
            }
            doc.AddAuthor("S3G Team");
            doc.Close();

            Session["strPath"] = strnewFile;
            string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

            //System.Diagnostics.Process.Start(strnewFile);
            ModalPopupExtenderMailPreview.Hide();
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
        }
    }

    //private void FunPriInitControls()
    //{
    //    try
    //    {
    //        bool is9RoundsCoverede = false;
    //        ddlRound.Items.Clear();
    //        SetDefaultScreen(true);
    //        if (ddlEnquiryNumber != null && ddlEnquiryNumber.SelectedIndex > 0)
    //        {
    //            //imgRequestDate.Visible =
    //            ddlAgencyCode.Enabled = true;
    //            txtCustNameAddress.Text = "";

    //            Procparam = new Dictionary<string, string>();
    //            Procparam.Add("@Enquiry_ID", ddlEnquiryNumber.SelectedValue);
    //            Procparam.Add("@Company_ID", intCompanyID.ToString());
    //            if (ObjUserInfo.ProUserTypeRW.ToUpper().Contains("UTPA"))
    //            {
    //                Procparam.Add("@UserType", "Y");
    //                Procparam.Add("@UTPA_ID", intUserID.ToString());
    //            }
    //            else
    //            {
    //                Procparam.Add("@UserType", "N");
    //            }

    //            DataTable DtObjS3G_EnquiryResponse = new DataTable();
    //            DtObjS3G_EnquiryResponse = Utility.GetDefaultData("S3G_ORG_GetFIRByEnquiryID", Procparam);

    //            //            CreditMgtServices.S3G_ORG_EnquiryResponseRow objEnquiryResponseRow;
    //            //            ObjS3G_EnquiryResponse.Rows.Clear();
    //            //            objEnquiryResponseRow = ObjS3G_EnquiryResponse.NewS3G_ORG_EnquiryResponseRow();
    //            //            objEnquiryResponseRow.Enquiry_No = ddlEnquiryNumber.SelectedValue;//txtFIR.Text;
    //            //            objEnquiryResponseRow.Company_ID = intCompanyID;
    //            //objEnquiryResponseRow.
    //            //            ObjS3G_EnquiryResponse.AddS3G_ORG_EnquiryResponseRow(objEnquiryResponseRow);
    //            //            ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
    //            //            SerializationMode serMode = SerializationMode.Binary;
    //            //            byte[] bytesEnquiryResonse = ObjMgtCreditMgtClient.FunPubQueryEnquiryResponse(serMode, ClsPubSerialize.Serialize(ObjS3G_EnquiryResponse, serMode));
    //            //            ObjS3G_EnquiryResponse = (CreditMgtServices.S3G_ORG_EnquiryResponseDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryResonse, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_EnquiryResponseDataTable));

    //            if ((DtObjS3G_EnquiryResponse != null) && (DtObjS3G_EnquiryResponse.Rows.Count > 0))
    //            {
    //                ViewState["ReqFlag"] = "1";
    //                txtRequestDate.Enabled =
    //                txtSendEmail.Enabled = true;
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["FIRDate"].ToString())))
    //                    txtFIRDate.Text = Convert.ToDateTime(DtObjS3G_EnquiryResponse.Rows[0]["FIRDate"].ToString()).ToString(strDateFormat);
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["RequestedDate"].ToString())))
    //                    txtRequestDate.Text = Convert.ToDateTime(DtObjS3G_EnquiryResponse.Rows[0]["RequestedDate"].ToString()).ToString(strDateFormat);

    //                btnGeneratePdf.Enabled = true;

    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["RespondedBy"].ToString())))
    //                    txtRespondedBy.Text = DtObjS3G_EnquiryResponse.Rows[0]["RespondedBy"].ToString();
    //                else
    //                    txtRespondedBy.Text = "";
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Responded_Date"].ToString())))
    //                    txtResspondedDate.Text = Convert.ToDateTime(DtObjS3G_EnquiryResponse.Rows[0]["Responded_Date"].ToString()).ToString(strDateFormat);

    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Response_Designation"].ToString())))
    //                    txtResponseDesgn.Text = DtObjS3G_EnquiryResponse.Rows[0]["Response_Designation"].ToString();
    //                else
    //                    txtResponseDesgn.Text = "";
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Field_Officer_EMailID"].ToString())))
    //                    txtEmailRes.Text = DtObjS3G_EnquiryResponse.Rows[0]["Field_Officer_EMailID"].ToString();
    //                else
    //                    txtEmailRes.Text = "";
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Field_Officer_MobileNo"].ToString())))
    //                    txtMobile.Text = DtObjS3G_EnquiryResponse.Rows[0]["Field_Officer_MobileNo"].ToString();
    //                else
    //                    txtMobile.Text = "";
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Client_Credibility"].ToString())))
    //                {
    //                    ddlClientCreditability.SelectedIndex = Array.IndexOf(_ClientCreditibility, DtObjS3G_EnquiryResponse.Rows[0]["Client_Credibility"].ToString());
    //                }
    //                else
    //                    ddlClientCreditability.SelectedIndex = 0;
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Value"].ToString())))
    //                    txtValue.Text = DtObjS3G_EnquiryResponse.Rows[0]["Value"].ToString();
    //                else
    //                    txtValue.Text = "";
    //                if (string.Compare("C", Request.QueryString.Get("qsMode")) != 0)
    //                {
    //                    if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Currency"].ToString())))
    //                    {
    //                        if (ddlCurrency != null && ddlCurrency.Items.Count > 0)
    //                            ddlCurrency.SelectedValue = DtObjS3G_EnquiryResponse.Rows[0]["Currency"].ToString();
    //                        txtCurrency.Text = ddlCurrency.SelectedItem.ToString();
    //                    }
    //                    else
    //                    {
    //                        FunPriLoadCombo_CurrencyMaster();
    //                    }
    //                }
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Client_Net_Worth"].ToString())))
    //                    ddlClientNetWorth.SelectedIndex = Array.IndexOf(_ClientNetWorth, DtObjS3G_EnquiryResponse.Rows[0]["Client_Net_Worth"].ToString());
    //                else
    //                    ddlClientNetWorth.SelectedIndex = 0;
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Response"].ToString())))
    //                    txtFieldRespond.Text = DtObjS3G_EnquiryResponse.Rows[0]["Response"].ToString();
    //                else
    //                    txtFieldRespond.Text = "";

    //                string round = DtObjS3G_EnquiryResponse.Rows[0]["Round"].ToString();
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Cancelled"].ToString())))
    //                    chkCancelled.Checked = Convert.ToBoolean(DtObjS3G_EnquiryResponse.Rows[0]["Cancelled"].ToString());

    //                if ((!(string.IsNullOrEmpty(round))) &&
    //                    (Convert.ToInt32(DtObjS3G_EnquiryResponse.Rows[0]["Round"].ToString()) == 9))
    //                {

    //                    //btnGeneratePdf.Enabled = false;

    //                    //Utility.FunShowAlertMsg(this, "Already the customer covered 9 rounds");
    //                    //ddlRound.Items.Clear();
    //                    //SetDefaultScreen(true);
    //                    //is9RoundsCoverede = true;

    //                }

    //                ddlLOB.SelectedValue = DtObjS3G_EnquiryResponse.Rows[0]["LOB_ID"].ToString();
    //                txtLOB.Text = ddlLOB.SelectedItem.ToString();
    //                ddlBranch.SelectedValue = DtObjS3G_EnquiryResponse.Rows[0]["Location_ID"].ToString();
    //                txtBranch.Text = ddlBranch.SelectedItem.ToString();

    //                FunPriGetEntityMaster();
    //                // customer personal details.
    //                string strCustomerAddress = "";
    //                txtCustomerCode.Text = DtObjS3G_EnquiryResponse.Rows[0]["CustomerCode"].ToString();
    //                txtCustNameAddress.Text = strCustomerAddress =
    //                    DtObjS3G_EnquiryResponse.Rows[0]["CustomerAddress"].ToString() + '\n' +
    //                    DtObjS3G_EnquiryResponse.Rows[0]["CustomerAddress2"].ToString() + '\n' +
    //                    DtObjS3G_EnquiryResponse.Rows[0]["CustomerCity"].ToString() + '\n' +
    //                    DtObjS3G_EnquiryResponse.Rows[0]["CustomerState"].ToString() + '\n' +
    //                    DtObjS3G_EnquiryResponse.Rows[0]["CustomerCountry"].ToString() + '\n' +
    //                    DtObjS3G_EnquiryResponse.Rows[0]["CustomerPincode"].ToString();

    //                S3GCustomerAddress1.SetCustomerDetails(DtObjS3G_EnquiryResponse.Rows[0]["CustomerCode"].ToString(),
    //                   strCustomerAddress, DtObjS3G_EnquiryResponse.Rows[0]["CUstomerName"].ToString(),
    //                   DtObjS3G_EnquiryResponse.Rows[0]["Phone"].ToString(),
    //                   DtObjS3G_EnquiryResponse.Rows[0]["CustomerMobile"].ToString(),
    //                   DtObjS3G_EnquiryResponse.Rows[0]["CustomerEmail"].ToString(),
    //                   DtObjS3G_EnquiryResponse.Rows[0]["CustomerWebsite"].ToString());

    //                txtCustomerName.Text = DtObjS3G_EnquiryResponse.Rows[0]["CUstomerName"].ToString();
    //                txtEmailCust.Text = DtObjS3G_EnquiryResponse.Rows[0]["CustomerEmail"].ToString();
    //                txtCustomerMobile.Text = DtObjS3G_EnquiryResponse.Rows[0]["CustomerMobile"].ToString();
    //                txtWebsite.Text = DtObjS3G_EnquiryResponse.Rows[0]["CustomerWebsite"].ToString();
    //                ViewState["FIRNum"] = _FIRNo = DtObjS3G_EnquiryResponse.Rows[0]["FIR_No"].ToString();
    //                // customer FIR Details.
    //                if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["EnquiryResponse_Number"].ToString())))
    //                {

    //                    txtFIR.Text = DtObjS3G_EnquiryResponse.Rows[0]["FIR_No"].ToString();
    //                    txtFIR.Text += "/" + DtObjS3G_EnquiryResponse.Rows[0]["Round"].ToString();

    //                    if (!(string.IsNullOrEmpty(round)))
    //                    {

    //                        int totalRound = (Convert.ToInt32(DtObjS3G_EnquiryResponse.Rows[0]["Round"].ToString()));

    //                        ddlRound.Items.Clear();
    //                        for (int i = 1; i <= totalRound; i++)
    //                        {
    //                            if (i > 9)
    //                                break;
    //                            ddlRound.Items.Insert(i - 1, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
    //                        }

    //                        ddlRound.SelectedValue = totalRound.ToString();
    //                    }
    //                    else
    //                    {
    //                        ddlRound.Items.Clear();
    //                        ddlRound.Items.Insert(0, new System.Web.UI.WebControls.ListItem("1", "1"));
    //                    }
    //                    if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["FIRStatus"].ToString())))
    //                    {
    //                        ddlStatus.SelectedValue = GetStatusValue(DtObjS3G_EnquiryResponse.Rows[0]["FIRStatus"].ToString()).ToString();

    //                    }

    //                    txtTerms.Text = DtObjS3G_EnquiryResponse.Rows[0]["Terms_Conditions"].ToString();
    //                    txtFieldRequest.Text = DtObjS3G_EnquiryResponse.Rows[0]["FieldRequest"].ToString();
    //                    txtRequestBy.Text = DtObjS3G_EnquiryResponse.Rows[0]["RequestedBy"].ToString();
    //                    if (string.IsNullOrEmpty(txtRequestBy.Text))
    //                    {
    //                        txtRequestBy.Text = ObjUserInfo.ProUserNameRW;
    //                    }

    //                    if (!(string.IsNullOrEmpty(DtObjS3G_EnquiryResponse.Rows[0]["Agency_Id"].ToString())))
    //                    {
    //                        ddlAgencyCode.SelectedValue = DtObjS3G_EnquiryResponse.Rows[0]["Agency_Id"].ToString();
    //                        txtAgencyName.Text = DtObjS3G_EnquiryResponse.Rows[0]["AgencyName"].ToString();
    //                        txtAgencyNAmeAddress.Text = DtObjS3G_EnquiryResponse.Rows[0]["AgencyAddress"].ToString();
    //                        _AgencyCode = DtObjS3G_EnquiryResponse.Rows[0]["Agency_Code"].ToString();
    //                        txtSendEmail.Text = DtObjS3G_EnquiryResponse.Rows[0]["AgencyEmail"].ToString();

    //                    }
    //                    else
    //                    {
    //                        ddlAgencyCode.SelectedIndex = 0;
    //                        txtAgencyNAmeAddress.Text = "";
    //                        txtAgencyName.Text = "";
    //                        _AgencyCode = "";
    //                        txtSendEmail.Text = "";

    //                        if (string.Compare(txtFIR.Text, "/") == 0)
    //                            txtFIR.Text = "";
    //                        txtFIR.Text += "/" + ddlRound.SelectedItem.ToString();
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                SetDefaultScreen(true);
    //                ddlRound.Items.Clear();
    //                ddlRound.Items.Insert(0, new System.Web.UI.WebControls.ListItem("1", "1"));
    //                ddlStatus.SelectedIndex = 0;
    //                txtFIR.Text += "/" + ddlRound.SelectedItem.ToString();
    //            }

    //            txtRound.Text = ddlRound.SelectedItem.ToString();

    //            if (txtRound.Text != "1")
    //            {
    //                ddlAgencyCode.ClearDropDownList();
    //            }
    //        }
    //        else
    //        {
    //            if (ddlEnquiryNumber.SelectedIndex > 0)
    //                Utility.FunShowAlertMsg(this, "There is no releated records in Enquiry Response page");
    //            ddlAgencyCode.Enabled = false;
    //            ddlRound.Items.Clear();
    //            SetDefaultScreen(true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    private void FunPriInitControls(string strFIR_ID)
    {
        try
        {
            int intType = 0;
            int intRefId = 0;
            bool is9RoundsCoverede = false;
            ddlRound.Items.Clear();
            SetDefaultScreen(true);

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@FIR_ID", strFIR_ID);
            DataTable dt = new DataTable();
            dt = Utility.GetDefaultData("S3G_ORG_GetFIRRefDocDtls", Procparam);

            if ((dt != null) && (dt.Rows.Count > 0))
            {
                intType = Convert.ToInt16(dt.Rows[0]["Doc_Type"].ToString());
                ViewState["DocType"] = intType;
                intRefId = Convert.ToInt16(dt.Rows[0]["Doc_Ref_ID"].ToString());
                ViewState["DocRef"] = intRefId;

                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem(dt.Rows[0]["Doc_Type_Name"].ToString(), dt.Rows[0]["Doc_Type"].ToString());
                ddlDocType.Items.Add(lst);

                ddlDocType.SelectedValue = dt.Rows[0]["Doc_Type"].ToString();
                ddlDocType.Enabled = false;
                //funPriLoadRefDocNo();
                if (intType == 82)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["PANum"].ToString()))
                    {
                        ddlEnquiryNumber.SelectedValue = dt.Rows[0]["PANum"].ToString();
                        ddlEnquiryNumber.SelectedText = dt.Rows[0]["PANum_Text"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["SANum"].ToString()))
                    {
                        lblSanum.Visible =
                        ddlSanum.Visible = true;
                        FunPriLoadSanum();
                        //ddlSanum.SelectedItem.Text = dt.Rows[0]["SANum"].ToString();
                        ddlSanum.SelectedValue = intRefId.ToString();
                        ddlSanum.Enabled = false;
                        //ddlSanum.ClearDropDownList();
                    }
                }
                else
                {
                    ddlEnquiryNumber.SelectedValue = dt.Rows[0]["Doc_Ref_ID"].ToString();
                    ddlEnquiryNumber.SelectedText = dt.Rows[0]["Ref_Text"].ToString();
                }


                if (ddlEnquiryNumber != null && Convert.ToInt16(ddlEnquiryNumber.SelectedValue) > 0)
                {
                    //imgRequestDate.Visible =
                    ddlAgencyType.Enabled = true;
                    ddlAgencyCode.Enabled = true;
                    txtCustNameAddress.Text = "";


                    Procparam.Clear();
                    Procparam = new Dictionary<string, string>();

                    Procparam.Add("@Type", intType.ToString());
                    Procparam.Add("@DocRef_ID", intRefId.ToString());
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    if (ObjUserInfo.ProUserTypeRW.ToUpper().Contains("UTPA"))
                    {
                        Procparam.Add("@UserType", "Y");
                        Procparam.Add("@UTPA_ID", intUserID.ToString());
                    }
                    else
                    {
                        Procparam.Add("@UserType", "N");
                    }

                    DataTable dtFIR = new DataTable();
                    dtFIR = Utility.GetDefaultData("S3G_ORG_GetFIRByDocRefID", Procparam);

                    //            CreditMgtServices.S3G_ORG_EnquiryResponseRow objEnquiryResponseRow;
                    //            ObjS3G_EnquiryResponse.Rows.Clear();
                    //            objEnquiryResponseRow = ObjS3G_EnquiryResponse.NewS3G_ORG_EnquiryResponseRow();
                    //            objEnquiryResponseRow.Enquiry_No = ddlEnquiryNumber.SelectedValue;//txtFIR.Text;
                    //            objEnquiryResponseRow.Company_ID = intCompanyID;
                    //objEnquiryResponseRow.
                    //            ObjS3G_EnquiryResponse.AddS3G_ORG_EnquiryResponseRow(objEnquiryResponseRow);
                    //            ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
                    //            SerializationMode serMode = SerializationMode.Binary;
                    //            byte[] bytesEnquiryResonse = ObjMgtCreditMgtClient.FunPubQueryEnquiryResponse(serMode, ClsPubSerialize.Serialize(ObjS3G_EnquiryResponse, serMode));
                    //            ObjS3G_EnquiryResponse = (CreditMgtServices.S3G_ORG_EnquiryResponseDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryResonse, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_EnquiryResponseDataTable));

                    if ((dtFIR != null) && (dtFIR.Rows.Count > 0))
                    {
                        ViewState["ReqFlag"] = "1";
                        txtRequestDate.Enabled =
                        txtSendEmail.Enabled = true;
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["FIRDate"].ToString())))
                            txtFIRDate.Text = Convert.ToDateTime(dtFIR.Rows[0]["FIRDate"].ToString()).ToString(strDateFormat);
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["RequestedDate"].ToString())))
                            txtRequestDate.Text = Convert.ToDateTime(dtFIR.Rows[0]["RequestedDate"].ToString()).ToString(strDateFormat);
                        ViewState["Customer"] = dtFIR.Rows[0]["CustomerID"].ToString();
                        btnGeneratePdf.Enabled = true;

                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["RespondedBy"].ToString())))
                            txtRespondedBy.Text = dtFIR.Rows[0]["RespondedBy"].ToString();
                        else
                            txtRespondedBy.Text = "";
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Responded_Date"].ToString())))
                            txtResspondedDate.Text = Convert.ToDateTime(dtFIR.Rows[0]["Responded_Date"].ToString()).ToString(strDateFormat);

                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Response_Designation"].ToString())))
                            txtResponseDesgn.Text = dtFIR.Rows[0]["Response_Designation"].ToString();
                        else
                            txtResponseDesgn.Text = "";
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Field_Officer_EMailID"].ToString())))
                            txtEmailRes.Text = dtFIR.Rows[0]["Field_Officer_EMailID"].ToString();
                        else
                            txtEmailRes.Text = "";
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Field_Officer_MobileNo"].ToString())))
                            txtMobile.Text = dtFIR.Rows[0]["Field_Officer_MobileNo"].ToString();
                        else
                            txtMobile.Text = "";



                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Client_Credibility"].ToString())))
                        {
                            ddlClientCreditability.SelectedIndex = Array.IndexOf(_ClientCreditibility, dtFIR.Rows[0]["Client_Credibility"].ToString());
                        }
                        else
                            ddlClientCreditability.SelectedIndex = 0;
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Value"].ToString())))
                            txtValue.Text = dtFIR.Rows[0]["Value"].ToString();
                        else
                            txtValue.Text = "";

                        if (PageMode == PageModes.Query)
                        {
                            lst = new System.Web.UI.WebControls.ListItem(dtFIR.Rows[0]["Currency_Text"].ToString(), dtFIR.Rows[0]["Currency"].ToString());
                            ddlCurrency.Items.Add(lst);
                        }

                        if (string.Compare("C", Request.QueryString.Get("qsMode")) != 0)
                        {
                            if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Currency"].ToString())))
                            {
                                if (ddlCurrency != null && ddlCurrency.Items.Count > 0)
                                    ddlCurrency.SelectedValue = dtFIR.Rows[0]["Currency"].ToString();
                                txtCurrency.Text = ddlCurrency.SelectedItem.ToString();
                            }
                            else
                            {
                                FunPriLoadCombo_CurrencyMaster();
                            }
                        }
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Client_Net_Worth"].ToString())))
                            ddlClientNetWorth.SelectedIndex = Array.IndexOf(_ClientNetWorth, dtFIR.Rows[0]["Client_Net_Worth"].ToString());
                        else
                            ddlClientNetWorth.SelectedIndex = 0;
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Response"].ToString())))
                            txtFieldRespond.Text = dtFIR.Rows[0]["Response"].ToString();
                        else
                            txtFieldRespond.Text = "";

                        string round = dtFIR.Rows[0]["Round"].ToString();
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Cancelled"].ToString())))
                            chkCancelled.Checked = Convert.ToBoolean(dtFIR.Rows[0]["Cancelled"].ToString());

                        if ((!(string.IsNullOrEmpty(round))) &&
                            (Convert.ToInt32(dtFIR.Rows[0]["Round"].ToString()) == 9))
                        {

                            //btnGeneratePdf.Enabled = false;

                            //Utility.FunShowAlertMsg(this, "Already the customer covered 9 rounds");
                            //ddlRound.Items.Clear();
                            //SetDefaultScreen(true);
                            //is9RoundsCoverede = true;

                        }


                        lst = new System.Web.UI.WebControls.ListItem(dtFIR.Rows[0]["LOB_Name"].ToString(), dtFIR.Rows[0]["LOB_ID"].ToString());
                        ddlLOB.Items.Add(lst);

                        lst = new System.Web.UI.WebControls.ListItem(dtFIR.Rows[0]["Location_Name"].ToString(), dtFIR.Rows[0]["Location_ID"].ToString());
                        ddlBranch.Items.Add(lst);

                        ddlLOB.SelectedValue = dtFIR.Rows[0]["LOB_ID"].ToString();
                        txtLOB.Text = ddlLOB.SelectedItem.ToString();

                        try
                        {
                            ddlBranch.SelectedValue = dtFIR.Rows[0]["Location_ID"].ToString();
                        }
                        catch (Exception ex)
                        {
                        }

                        txtBranch.Text = ddlBranch.SelectedItem.ToString();

                        if (PageMode != PageModes.Query)
                        {
                            FunPriGetEntityMaster();
                        }
                        // customer personal details.
                        string strCustomerAddress = "";
                        txtCustomerCode.Text = dtFIR.Rows[0]["CustomerCode"].ToString();
                        txtCustNameAddress.Text = strCustomerAddress =
                            dtFIR.Rows[0]["CustomerAddress"].ToString() + '\n' +
                            dtFIR.Rows[0]["CustomerAddress2"].ToString() + '\n' +
                            dtFIR.Rows[0]["CustomerCity"].ToString() + '\n' +
                            dtFIR.Rows[0]["CustomerState"].ToString() + '\n' +
                            dtFIR.Rows[0]["CustomerCountry"].ToString() + '\n' +
                            dtFIR.Rows[0]["CustomerPincode"].ToString();

                        S3GCustomerAddress1.SetCustomerDetails(dtFIR.Rows[0]["CustomerCode"].ToString(),
                           strCustomerAddress, dtFIR.Rows[0]["CUstomerName"].ToString(),
                           dtFIR.Rows[0]["Phone"].ToString(),
                           dtFIR.Rows[0]["CustomerMobile"].ToString(),
                           dtFIR.Rows[0]["CustomerEmail"].ToString(),
                           dtFIR.Rows[0]["CustomerWebsite"].ToString());

                        txtCustomerName.Text = dtFIR.Rows[0]["CUstomerName"].ToString();
                        txtEmailCust.Text = dtFIR.Rows[0]["CustomerEmail"].ToString();
                        txtCustomerMobile.Text = dtFIR.Rows[0]["CustomerMobile"].ToString();
                        txtWebsite.Text = dtFIR.Rows[0]["CustomerWebsite"].ToString();
                        ViewState["FIRNum"] = _FIRNo = dtFIR.Rows[0]["FIR_No"].ToString();
                        // customer FIR Details.
                        if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Number"].ToString())))
                        {

                            txtFIR.Text = dtFIR.Rows[0]["FIR_No"].ToString();
                            txtFIR.Text += "/" + dtFIR.Rows[0]["Round"].ToString();

                            if (!(string.IsNullOrEmpty(round)))
                            {

                                int totalRound = (Convert.ToInt32(dtFIR.Rows[0]["Round"].ToString()));

                                ddlRound.Items.Clear();
                                for (int i = 1; i <= totalRound; i++)
                                {
                                    if (i > 9)
                                        break;
                                    ddlRound.Items.Insert(i - 1, new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                                }

                                ddlRound.SelectedValue = totalRound.ToString();
                            }
                            else
                            {
                                ddlRound.Items.Clear();
                                ddlRound.Items.Insert(0, new System.Web.UI.WebControls.ListItem("1", "1"));
                            }
                            if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["FIRStatus"].ToString())))
                            {
                                ddlStatus.SelectedValue = GetStatusValue(dtFIR.Rows[0]["FIRStatus"].ToString()).ToString();

                            }

                            txtTerms.Text = dtFIR.Rows[0]["Terms_Conditions"].ToString();
                            txtFieldRequest.Text = dtFIR.Rows[0]["FieldRequest"].ToString();
                            txtRequestBy.Text = dtFIR.Rows[0]["RequestedBy"].ToString();
                            if (string.IsNullOrEmpty(txtRequestBy.Text))
                            {
                                txtRequestBy.Text = ObjUserInfo.ProUserNameRW;
                            }

                            if (PageMode == PageModes.Query)
                            {
                                lst = new System.Web.UI.WebControls.ListItem(dtFIR.Rows[0]["Entity_Type_Name"].ToString(), dtFIR.Rows[0]["Entity_Type_ID"].ToString());
                                ddlAgencyType.Items.Add(lst);
                            }

                            if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Entity_Type_ID"].ToString())))
                            {
                                ddlAgencyType.SelectedValue = dtFIR.Rows[0]["Entity_Type_ID"].ToString();
                            }
                            else
                            {
                                //ddlAgencyType.SelectedIndex = 0;
                            }

                            if (PageMode != PageModes.Query)
                            {
                                PopulateInspCode(ddlAgencyType.SelectedValue);
                            }
                            else
                            {
                                lst = new System.Web.UI.WebControls.ListItem(dtFIR.Rows[0]["AgencyName"].ToString(), dtFIR.Rows[0]["Agency_Id"].ToString());
                                ddlAgencyCode.Items.Add(lst);
                            }

                            if (!(string.IsNullOrEmpty(dtFIR.Rows[0]["Agency_Id"].ToString())))
                            {
                                ddlAgencyCode.SelectedValue = dtFIR.Rows[0]["Agency_Id"].ToString();
                                txtAgencyName.Text = dtFIR.Rows[0]["AgencyName"].ToString();
                                txtAgencyNAmeAddress.Text = dtFIR.Rows[0]["AgencyAddress"].ToString();
                                _AgencyCode = dtFIR.Rows[0]["Agency_Code"].ToString();
                                txtSendEmail.Text = dtFIR.Rows[0]["AgencyEmail"].ToString();

                            }
                            else
                            {
                                ddlAgencyCode.SelectedIndex = 0;
                                txtAgencyNAmeAddress.Text = "";
                                txtAgencyName.Text = "";
                                _AgencyCode = "";
                                txtSendEmail.Text = "";

                                if (string.Compare(txtFIR.Text, "/") == 0)
                                    txtFIR.Text = "";
                                txtFIR.Text += "/" + ddlRound.SelectedItem.ToString();
                            }
                        }
                    }
                    else
                    {
                        SetDefaultScreen(true);
                        ddlRound.Items.Clear();
                        ddlRound.Items.Insert(0, new System.Web.UI.WebControls.ListItem("1", "1"));
                        ddlStatus.SelectedIndex = 0;
                        txtFIR.Text += "/" + ddlRound.SelectedItem.ToString();
                    }

                    txtRound.Text = ddlRound.SelectedItem.ToString();

                    if (txtRound.Text != "1")
                    {
                        ddlAgencyCode.ClearDropDownList();
                    }
                }

                else
                {
                    if (ddlEnquiryNumber.SelectedValue != "0")
                        Utility.FunShowAlertMsg(this, "There is no releated records in Enquiry Response page");
                    ddlAgencyCode.Enabled = false;
                    ddlRound.Items.Clear();
                    SetDefaultScreen(true);
                }
                FunGetFIRTransDocDetatils();   //MODIFY
                //FunPriLoadFIRID();
                FunPriAssignValues();
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void FunClearReqAgencyControls(bool IsNew)
    {
        try
        {
            txtAgencyName.Text =
                txtAgencyNAmeAddress.Text =
                txtTerms.Text =
                txtFieldRequest.Text =
                txtSendEmail.Text = "";
            txtRequestDate.Text = DateTime.Now.ToString(strDateFormat);
            chkCancelled.Checked = false;

            ddlAgencyType.SelectedIndex = 0;

            if (ddlAgencyCode.SelectedIndex != -1)
            {
                ddlAgencyCode.SelectedIndex = 0;
            }
            ddlAgencyCode.Items.Clear();

            if (IsNew)
            {
                //txtFIR.Text = @"\1";
                txtFIR.Text = "";
                txtRound.Text = "1";
                txtRequestBy.Text = ObjUserInfo.ProUserNameRW.ToString();
            }
            else
            {
                txtFIR.Text = "";
                txtRound.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unable to Clear Agency Details");
        }
    }

    protected void ddlEnquiryNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            string strCustomerAddress = "";
            if (ddlEnquiryNumber != null && ddlEnquiryNumber.SelectedValue != "0")
            {


                #region To Load Sub Acc
                //added by saranya
                if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82") && ddlEnquiryNumber.SelectedValue != "0")
                {
                    lblSanum.Visible = true;
                    ddlSanum.Visible = true;
                    ddlSanum.Items.Clear();
                    lblSanum.CssClass = "styleDisplayLabel";
                    FunPriLoadSanum();
                    if (ddlSanum.Items.Count > 1)
                    {
                        lblSanum.CssClass = "styleReqFieldLabel";
                        RfvSanum.Enabled = true;
                    }
                    else
                    {
                        RfvSanum.Enabled = false;
                    }
                    if (ddlSanum.SelectedIndex == 0 || ddlSanum.SelectedIndex > 0)
                    {
                        FunPriLoadCustomerInfo();
                        //FunPriLoadFIRID();
                        //FunPriGetPRDDTransType();
                        tpPDDT.Visible = true;
                        gvPRDDT.Visible = true;
                    }
                }
                else
                {
                    lblSanum.Visible = false;
                    ddlSanum.Visible = false;
                    ddlSanum.Items.Clear();
                }

                //end

                # endregion
                //imgRequestDate.Visible = true;
                //Changed based on UAT
                //ddlAgencyCode.Enabled = true;
                txtCustNameAddress.Text = "";
                if (ddlDocType.SelectedIndex > 0 && Convert.ToInt32(ddlDocType.SelectedValue) != Convert.ToInt32("82"))
                {
                    FunPriLoadCustomerInfo();
                    //FunPriLoadFIRID();
                    //FunPriGetPRDDTransType();
                    tpPDDT.Visible = true;
                    gvPRDDT.Visible = true;
                }
                //End Here




            }
            else
            {
                FunPriClearControls();
                if (ddlSanum.Visible == true)
                {
                    lblSanum.Visible = false;
                    ddlSanum.Visible = false;
                }
                ddlAgencyCode.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();
        }
    }

    protected void ddlSanum_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSanum.SelectedIndex > 0)
        {
            FunPriLoadCustomerInfo();
            //FunPriLoadFIRID();
            //FunPriGetPRDDTransType();
        }
        else
        {
            FunPriClearControls();
            ddlAgencyCode.SelectedIndex = -1;
        }
    }

    private void FunPriLoadCustomerInfo()
    {
        try
        {
            string strCustomerAddress = "";
            txtCustNameAddress.Text = "";
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Type", ddlDocType.SelectedValue);
            if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
            {
                if (ddlSanum.Items.Count == 1)
                {
                    Procparam.Add("@DocRef_ID", ddlEnquiryNumber.SelectedValue);
                }
                else
                {
                    Procparam.Add("@DocRef_ID", ddlSanum.SelectedValue);
                }
            }
            else
            {
                Procparam.Add("@DocRef_ID", ddlEnquiryNumber.SelectedValue);
            }
            Procparam.Add("@Company_ID", intCompanyID.ToString());

            dtCustInfo = Utility.GetDefaultData("S3G_ORG_GetFIRByDocRefID", Procparam);
            if ((dtCustInfo != null) && (dtCustInfo.Rows.Count > 0))
            {
                ddlLOB.SelectedValue = dtCustInfo.Rows[0]["LOB_ID"].ToString();
                txtLOB.Text = ddlLOB.SelectedItem.ToString();


                try
                {
                    ddlBranch.SelectedValue = dtCustInfo.Rows[0]["Location_ID"].ToString();
                }
                catch (Exception ex)
                {
                    ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem(dtCustInfo.Rows[0]["Location_Name"].ToString(), dtCustInfo.Rows[0]["Location_ID"].ToString()));
                }



                txtBranch.Text = ddlBranch.SelectedItem.ToString();

                txtRequestBy.Text = dtCustInfo.Rows[0]["RequestedBy"].ToString();

                //FunPriGetEntityMaster();

                //Customer
                ViewState["CustomerId"] = dtCustInfo.Rows[0]["CustomerID"].ToString();

                //Product 
                if (!string.IsNullOrEmpty(dtCustInfo.Rows[0]["Product_ID"].ToString()))
                    ViewState["Product_Id"] = dtCustInfo.Rows[0]["Product_ID"].ToString();

                txtCustomerCode.Text = dtCustInfo.Rows[0]["CustomerCode"].ToString();
                txtCustNameAddress.Text = strCustomerAddress =
                   dtCustInfo.Rows[0]["CustomerAddress"].ToString() + '\n' +
                   dtCustInfo.Rows[0]["CustomerAddress2"].ToString() + '\n' +
                   dtCustInfo.Rows[0]["CustomerCity"].ToString() + '\n' +
                   dtCustInfo.Rows[0]["CustomerState"].ToString() + '\n' +
                   dtCustInfo.Rows[0]["CustomerCountry"].ToString() + '\n' +
                   dtCustInfo.Rows[0]["CustomerPincode"].ToString();

                S3GCustomerAddress1.SetCustomerDetails(dtCustInfo.Rows[0]["CustomerCode"].ToString(),
                    strCustomerAddress, dtCustInfo.Rows[0]["CUstomerName"].ToString(),
                    dtCustInfo.Rows[0]["Phone"].ToString(),
                    dtCustInfo.Rows[0]["CustomerMobile"].ToString(),
                    dtCustInfo.Rows[0]["CustomerEmail"].ToString(),
                    dtCustInfo.Rows[0]["CustomerWebsite"].ToString());


                txtCustomerName.Text = dtCustInfo.Rows[0]["CUstomerName"].ToString();

                txtEmailCust.Text = dtCustInfo.Rows[0]["CustomerEmail"].ToString();
                txtWebsite.Text = dtCustInfo.Rows[0]["CustomerWebsite"].ToString();
                txtCustomerMobile.Text = dtCustInfo.Rows[0]["CustomerMobile"].ToString();

                ViewState["Constitution_ID"] = dtCustInfo.Rows[0]["Constitution_ID"].ToString();

                ViewState["FIRNum"] = _FIRNo = dtCustInfo.Rows[0]["FIR_No"].ToString();
                if (!(string.IsNullOrEmpty(dtCustInfo.Rows[0]["FIR_No"].ToString())))
                {
                    if (!(string.IsNullOrEmpty(dtCustInfo.Rows[0]["Cancelled"].ToString())))
                    {
                        if (Convert.ToBoolean(dtCustInfo.Rows[0]["Cancelled"].ToString()))
                        {
                            FunClearReqAgencyControls(true);
                            FunToggleButtons(true);
                            return;
                        }
                    }
                    if (string.Compare(dtCustInfo.Rows[0]["FIRStatus"].ToString().ToUpper(), "RESPONDED") == 0)
                    {
                        if (dtCustInfo.Rows[0]["Round"].ToString() == "9")
                        {
                            FunClearReqAgencyControls(false);
                            txtFIR.Text = dtCustInfo.Rows[0]["FIR_No"].ToString() + "/" + dtCustInfo.Rows[0]["Round"].ToString();
                            Utility.FunShowAlertMsg(this, "Already this Enquiry covered 9 rounds");
                            FunToggleButtons(false);
                            ddlAgencyCode.SelectedIndex = -1;
                            return;
                        }
                        else
                        {
                            FunClearReqAgencyControls(false);
                            txtRound.Text = (Convert.ToInt32(dtCustInfo.Rows[0]["Round"].ToString()) + 1).ToString();
                            ViewState["FIRNum"] = dtCustInfo.Rows[0]["FIR_No"].ToString();
                            txtFIR.Text = dtCustInfo.Rows[0]["FIR_No"].ToString() + "/" + txtRound.Text;
                            txtRequestBy.Text = ObjUserInfo.ProUserNameRW.ToString();

                            //Change based on UAT
                            //ddlAgencyCode.SelectedIndex = -1;
                            ddlAgencyCode.SelectedValue = dtCustInfo.Rows[0]["Agency_Id"].ToString();
                            //ddlAgencyCode.ClearDropDownList();
                            ProFillAgencyDetails();
                            //

                            FunToggleButtons(true);
                        }
                    }
                    else
                    {
                        FunClearReqAgencyControls(false);
                        txtFIR.Text = dtCustInfo.Rows[0]["FIR_No"].ToString() + "/" + dtCustInfo.Rows[0]["Round"].ToString();
                        Utility.FunShowAlertMsg(this, "Round " + dtCustInfo.Rows[0]["Round"].ToString() + " is not yet responded. Cannot create new round.");
                        FunToggleButtons(false);
                        FunPriClearControls();
                        gvPRDDT.Visible = false;
                        gvPRDDT.DataSource = null;
                        gvPRDDT.DataBind();
                        ddlAgencyCode.SelectedIndex = -1;
                        return;
                    }
                }
                else
                {
                    FunClearReqAgencyControls(true);
                    FunToggleButtons(true);
                }
            }
            FunPriLoadFIRID();
            FunPriGetPRDDTransType();
        }

        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriLoadFIRID()
    {
        Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
        dictParam1.Add("@LOB_ID", ddlLOB.SelectedValue);
        dictParam1.Add("@Constitution_ID", Convert.ToString(ViewState["Constitution_ID"]));
        dictParam1.Add("@Company_ID", intCompanyID.ToString());
        dictParam1.Add("@Doc_Type", ddlDocType.SelectedValue);
        if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
        {
            if (ddlSanum.Items.Count == 1)
            {
                dictParam1.Add("@Doc_Ref_ID", ddlEnquiryNumber.SelectedValue);
            }
            else
            {
                dictParam1.Add("@Doc_Ref_ID", ddlSanum.SelectedValue);
            }
        }
        else
        {
            dictParam1.Add("@Doc_Ref_ID", ddlEnquiryNumber.SelectedValue);
        }


        DataTable dtDetails = Utility.GetDefaultData("S3G_ORG_GetFIRTID", dictParam1);
        if (dtDetails.Rows.Count > 0)
        {
            DataRow dtRow = dtDetails.Rows[0];
            ViewState["FIRID"] = dtRow["FIR_ID"].ToString();
        }
    }

    private void FunPriGetPRDDTransType()
    {

        try
        {
            Dictionary<string, string> dictParam2 = new Dictionary<string, string>();
            dictParam2.Add("@LOB_ID", ddlLOB.SelectedValue);
            dictParam2.Add("@Constitution_ID", Convert.ToString(ViewState["Constitution_ID"]));
            dictParam2.Add("@Company_ID", intCompanyID.ToString());
            dictParam2.Add("@Doc_Type", ddlDocType.SelectedValue);
            if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
            {
                if (ddlSanum.Items.Count == 1)
                {
                    dictParam2.Add("@Doc_Ref_ID", ddlEnquiryNumber.SelectedValue);
                }
                else
                {
                    dictParam2.Add("@Doc_Ref_ID", ddlSanum.SelectedValue);
                }
            }
            else
            {
                dictParam2.Add("@Doc_Ref_ID", ddlEnquiryNumber.SelectedValue);
            }
            dictParam2.Add("@FIR_ID", Convert.ToString(ViewState["FIRID"]));
            DataTable dtPRDDTypeTrans = Utility.GetDefaultData("S3G_ORG_GetFIRTLookup", dictParam2);
            if (dtPRDDTypeTrans.Rows.Count > 0)
            {
                //Test
                gvPRDDT.DataSource = dtPRDDTypeTrans;
                gvPRDDT.DataBind();
                ViewState["dtPRDDTypeTrans"] = dtPRDDTypeTrans;
                gvPRDDT.Visible = true;
                tpPDDT.Visible = true;
                ViewState["Docpath"] = dtPRDDTypeTrans.Rows[0]["Document_Path"].ToString();

                for (int i = 0; i < gvPRDDT.Rows.Count; i++)
                {
                    if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                        Label txtScanBy = (Label)gvPRDDT.Rows[i].FindControl("lblScannedBy");
                        ImageButton hyplnkView = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                        AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");
                        CheckBox CbxCheck = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");
                        //DropDownList ddlDocValue = (DropDownList)gvPRDDT.Rows[i].FindControl("ddlDocValue");

                        if (dtPRDDTypeTrans.Rows[i]["Is_NeedScanCopy"].ToString() == "False")// && dtPRDDTypeTrans.Rows[i]["Is_Mandatory"].ToString() == "False")
                        {
                            txtScanDate.Enabled = txtScanBy.Enabled = false;
                            txtScanDate.Text = "";
                            txtScanBy.Text = "";
                            asyFileUpload.Enabled = false;
                            hyplnkView.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                gvPRDDT.DataSource = null;
                gvPRDDT.DataBind();
                Utility.FunShowAlertMsg(this, "Document details not defined in FIR Master");
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

    }

    private void FunPriLoadSanum()
    {
        try
        {
            string strPanum = string.Empty;
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            if (Convert.ToInt32(ddlDocType.SelectedValue) != 79 && ddlEnquiryNumber.SelectedValue.ToString() != "0")
                strPanum = ddlEnquiryNumber.SelectedText.Substring(0, ddlEnquiryNumber.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();
            else
                strPanum = ddlEnquiryNumber.SelectedText.ToString();
            
            Procparam.Add("@PANUM", strPanum.ToString());
            ddlSanum.BindDataTable("S3G_RPT_GetSubAccountForFIR", Procparam, new string[] { "PA_SA_REF_ID", "SANUM" });

            if (ddlSanum.Items.Count == 2)
                ddlSanum.SelectedIndex = 1;
            else
                ddlSanum.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }

    }

    protected void ddlDocType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            /* if (ddlDocType.SelectedItem.Text == "Enquiry")
             {
                 ddlEnquiryNumber.Enabled = true;
                 FunPriGetEnquiryResponse();
             }
             else if (ddlDocType.SelectedItem.Text == "Application")
             {
                 ddlEnquiryNumber.Enabled = true;
                 FunPriGetApplicationNo();
             }
             else
             {
                 ddlEnquiryNumber.ClearDropDownList();
                 ddlEnquiryNumber.Enabled = false;
             }
             */
            FunPriSetDoctypeChange();
        }
        catch (Exception ex)
        {
            //ObjCustomerService.Close();
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }
    // changed by bhuvana for performance on September 13th 2013//
    [System.Web.Services.WebMethod]
    public static string[] GetDocumentNumber(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", obj_Page.ddlDocType.SelectedValue);
        if (obj_Page.CheckForUTPA())
        {
            Procparam.Add("@USERTYPE", "Y");
        }
        Procparam.Add("@Ref_Id", obj_Page.FIRTRANS_ID.ToString());
        Procparam.Add("@UserId", obj_Page.intUserID.ToString());
        Procparam.Add("@Prefix", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetRefDocNoForFIR_AGT", Procparam));

        return suggetions.ToArray();
    }
    //end here//
    private void FunPriSetDoctypeChange()
    {
        if (ddlDocType.SelectedIndex > 0)
        {

            funPriLoadRefDocNo();
        }
        else
        {
            FunPriClearControls();
            if (ddlSanum.Visible == true)
            {
                lblSanum.Visible = false;
                ddlSanum.Visible = false;
            }
            ddlAgencyCode.SelectedIndex = -1;
        }
    }

    private void funPriLoadRefDocNo()
    {

        ddlEnquiryNumber.Enabled = true;
        if (ddlDocType.SelectedIndex > 0)
        {
            lblSanum.Visible = false;
            ddlSanum.Visible = false;
            ddlSanum.Items.Clear();
            RfvSanum.Enabled = false;
            FunPriClearTextControls();
            if (ddlAgencyCode.SelectedIndex > 0)
            {
                //ddlAgencyCode.SelectedIndex = 0;
                ddlAgencyCode.Items.Clear();
            }
            if (ddlEnquiryNumber.SelectedValue != "0")
            {
                ddlEnquiryNumber.Clear();
            }

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Type", ddlDocType.SelectedValue);
            Procparam.Add("@Company_id", intCompanyID.ToString());

            Procparam.Add("@UserId", intUserID.ToString());

            if (CheckForUTPA())
            {
                Procparam.Add("@USERTYPE", "Y");
            }

            Procparam.Add("@Ref_Id", FIRTRANS_ID.ToString());

            // ddlEnquiryNumber.BindDataTable("S3G_ORG_GetRefDocNoForFIR", Procparam, new string[] { "ID", "REFDOCNO" });
            //ddlEnquiryNumber.Items[0].Text = "--Select--";

        }
        else
        {
            ddlEnquiryNumber.Enabled = false;
            tpPDDT.Enabled = false;
            gvPRDDT.Visible = false;
        }
    }

    public void FunToggleButtons(bool CanEnable)
    {
        try
        {
            if (CanEnable)
            {
                if (bCreate)
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }
            }
            else
            {
                btnSave.Enabled = true;
                
            }

            btnClear.Enabled = 
            btnGeneratePdf.Enabled = ddlAgencyType.Enabled = ddlAgencyCode.Enabled =
            btnGenereatePreviewPDF.Enabled = btnModal.Enabled = //cexDate1.Enabled =
        btnSendMail.Enabled = CanEnable;
            txtTerms.ReadOnly = txtFieldRequest.ReadOnly = txtSendEmail.ReadOnly = !CanEnable;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Cannot toggle Buttons state.");
        }
    }

    private string FunGetFIRNo()
    {
        string strFIRNo = "";
        try
        {
            string fir;
            string guid = System.Guid.NewGuid().ToString();
            fir = "FIR" + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond;
            guid = guid.Replace("-", "");
            int len = (12 - fir.Length);
            guid = guid.Substring(0, len);
            strFIRNo = (fir += guid);
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        return strFIRNo;
    }

    private void FunEnableControls(bool Enable)
    {


    }

    private int GetStatusValue(string strStatus)
    {
        int intvalue = -1;
        try
        {
            for (int i = 0; i < _StatusSource.Length; i++)
            {
                if ((string.Compare(strStatus, _StatusSource[i])) == 0)
                    intvalue = i;
            }

            // Begin Modified By Senthilkumar P [PSK] on 29/Feb/2012
            //intvalue = -1;
            //  End Modified By Senthilkumar P [PSK] on 29/Feb/2012

        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        return intvalue;
    }

    protected void ddlAgencyCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ProFillAgencyDetails();
        }

        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
        }
    }

    protected void ProFillAgencyDetails()
    {
        try
        {
            if (Convert.ToInt32(ddlAgencyCode.SelectedValue.ToString()) > 0)
            {
                if (Procparam == null)
                    Procparam = new Dictionary<string, string>();
                else
                    Procparam.Clear();

                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
                Procparam.Add("@Entity_ID", ddlAgencyCode.SelectedValue.ToString());
                Procparam.Add("@Entity_Type", ddlAgencyType.SelectedValue.ToString());

                DataTable Obj_EntityMaster_DataTable = new DataTable();

                //Obj_EntityMaster_DataTable = Utility.GetDataset("S3G_ORG_GetUTPAField_Surveyor", Procparam).Tables[0];
                Obj_EntityMaster_DataTable = Utility.GetDataset("S3G_ORG_GetEntityField_Surveyor", Procparam).Tables[0];

                txtAgencyName.Text = Obj_EntityMaster_DataTable.Rows[0]["Entity_Name"].ToString();

                if (!string.IsNullOrEmpty(Obj_EntityMaster_DataTable.Rows[0]["Address"].ToString()))
                {
                    txtAgencyNAmeAddress.Text = Obj_EntityMaster_DataTable.Rows[0]["Address"].ToString() + '\n' +
                            Obj_EntityMaster_DataTable.Rows[0]["Address2"].ToString() + '\n' +
                                    Obj_EntityMaster_DataTable.Rows[0]["City"].ToString() + '\n' +
                                        Obj_EntityMaster_DataTable.Rows[0]["State"].ToString() + '\n' +
                                        Obj_EntityMaster_DataTable.Rows[0]["Country"].ToString() + '\n' +
                                            Obj_EntityMaster_DataTable.Rows[0]["PinCode"].ToString() + '\n';
                }
                else
                {
                    txtAgencyNAmeAddress.Text = string.Empty;
                }
                txtSendEmail.Text = Obj_EntityMaster_DataTable.Rows[0]["Email"].ToString();
            }
            else
            {
                txtAgencyNAmeAddress.Text = "";
                txtAgencyName.Text = "";
                txtSendEmail.Text = "";
            }
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnGeneratePdf_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriAssignValues();

            if (ModalPopupExtenderMailPreview.Enabled == false)
            {
                ModalPopupExtenderMailPreview.Enabled = true;

            }

            ModalPopupExtenderMailPreview.Show();

            if (ViewState["FIRNum"] == null)
            {
                ViewState["FIRNum"] = "";
            }
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }

        //txtTo.Text = txtSendEmail.Text;
        //txtBody.Text = "Respected Sir/Madam, \n\n\t" +
        //                txtFieldRequest.Text;
        //txtBody.Text += "\n\nWith thanks and regards,\n";
        //txtBody.Text += txtRequestBy.Text;
    }

    private void FunPriAssignValues()
    {
        ////Changed By Palani Kumar.A on 21/01/2014 for Product Features
        //string strddlEnquiryNumber = string.Empty;
        //if (Convert.ToInt32(ddlDocType.SelectedValue) != 79 && ddlEnquiryNumber.SelectedValue.ToString() != "0")
        //    strddlEnquiryNumber = ddlEnquiryNumber.SelectedText.Substring(0, ddlEnquiryNumber.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();
        //else
        //    strddlEnquiryNumber = ddlEnquiryNumber.SelectedText.ToString();
              
        if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
        {
            if ((ddlSanum.Items.Count == 1) || (ddlSanum.Items.Count == 0))
            {
                txtSubject.Text = "Field Information Report [" + ddlEnquiryNumber.SelectedText.ToString() + "]";
            }
            else
            {
                txtSubject.Text = "Field Information Report [" + ddlSanum.SelectedItem.Text + "]";
            }
        }
        else
        {
            txtSubject.Text = "Field Information Report [" + ddlEnquiryNumber.SelectedText + "]";
        }
        string combo = ddlEnquiryNumber.SelectedText;
        string Body = "Respected Sir/Madam, \n\n\t";
        Body += txtFieldRequest.Text;
        Body += "\n\n With thanks and regards,\n";
        Body += txtRequestBy.Text;
        txtTo.Text = txtSendEmail.Text;
        txtBody.Text = Body;

    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {

        ModalPopupExtenderMailPreview.Hide();
        CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();

        try
        {
            string body;
            body = "Respected Sir Madam, <br/> <br/> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp " +
                        txtFieldRequest.Text;
            body += "<br/><br/>With thanks and regards,<br/>";
            body += txtRequestBy.Text;


            //CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
            ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
            ObjCom_Mail.ProFromRW = "s3g@sundaraminfotech.in";
            ObjCom_Mail.ProTORW = txtSendEmail.Text;

            ObjCom_Mail.ProSubjectRW = "Field Information Report";
            ObjCom_Mail.ProMessageRW = body;
            ObjCommonMail.FunSendMail(ObjCom_Mail);
            Utility.FunShowAlertMsg(this, "Mail sent successfully");
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
            Utility.FunShowAlertMsg(this, "Invalid EMail ID. Mail not sent.");
        }
        finally
        {
            ObjCommonMail.Close();
        }
    }

    protected void btnClosePreview_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopupExtenderMailPreview.Hide();
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunPubRespondSave()
    {
        CreditMgtServicesReference.CreditMgtServicesClient objFIR = new CreditMgtServicesReference.CreditMgtServicesClient();

        try
        {
            if (_FIRNo == "" || ViewState["ReqFlag"].ToString() == "0")
            {
                Utility.FunShowAlertMsg(this, "Field Information Details not assigned.");
                return;
            }
            // this is to notify the end user that the request is responded by the third party
            ddlStatus.SelectedValue = "1";

            CreditMgtServices.S3G_ORG_FieldInformationReportDataTable ObjS3G_ORG_FIRTable = new CreditMgtServices.S3G_ORG_FieldInformationReportDataTable();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportRow ObjS3G_ORG_FIRRow = ObjS3G_ORG_FIRTable.NewS3G_ORG_FieldInformationReportRow();


            ObjS3G_ORG_FIRRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjS3G_ORG_FIRRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjS3G_ORG_FIRRow.Doc_Type = Convert.ToInt32(ddlDocType.SelectedValue);
            if (Convert.ToInt32(ddlDocType.SelectedValue) == Convert.ToInt32("82"))
            {
                if (ddlSanum.Visible == true)
                {
                    if (ddlSanum.Items.Count == 1)
                    {
                        ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                    }
                    else
                    {
                        ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlSanum.SelectedValue);

                    }
                }
                else
                {
                    ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
                }
            }
            else
            {
                ObjS3G_ORG_FIRRow.Doc_Ref_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
            }
            if (ViewState["CustomerId"] != null)
            {
                ObjS3G_ORG_FIRRow.Customer_ID = Convert.ToInt32(ViewState["CustomerId"].ToString());
            }
            else
            {
                ObjS3G_ORG_FIRRow.Customer_ID = Convert.ToInt32(ViewState["Customer"].ToString());
            }//dtCustInfo.Rows[0]["CustomerID"].ToString();
            //ObjS3G_ORG_FIRRow.Enquiry_Response_ID = Convert.ToInt32(ddlEnquiryNumber.SelectedValue);
            ObjS3G_ORG_FIRRow.Terms_Conditions = txtTerms.Text.Trim();
            ObjS3G_ORG_FIRRow.FIR_No = _FIRNo;
            ObjS3G_ORG_FIRRow.FIR_Date = System.DateTime.Now;//Convert.ToDateTime(txtFIRDate.Text);
            if (ddlRound == null || ddlRound.SelectedIndex < 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Enquiry number - to which the FIR to be generated");
                return;
            }
            ObjS3G_ORG_FIRRow.Round = ddlRound.SelectedItem.ToString();// txtRound.Text;
            ObjS3G_ORG_FIRRow.Requested_By = txtRequestBy.Text.Trim();
            ObjS3G_ORG_FIRRow.Requested_Date = Utility.StringToDate(txtRequestDate.Text);// Convert.ToDateTime(txtRequestDate.Text);
            if ((ddlAgencyCode == null || ddlAgencyCode.SelectedValue == "0")
                ||
                (string.IsNullOrEmpty(txtTerms.Text)) || (string.IsNullOrEmpty(txtFieldRequest.Text)))
            {
                Utility.FunShowAlertMsg(this, "Enter Request Information Details.");
                return;
            }

            ObjS3G_ORG_FIRRow.Entity_ID = Convert.ToInt32(ddlAgencyCode.SelectedValue);
            ObjS3G_ORG_FIRRow.Field_Request = txtFieldRequest.Text.Trim();
            ObjS3G_ORG_FIRRow.Notification_Sent_by = txtSendEmail.Text.Trim();
            ObjS3G_ORG_FIRRow.Cancelled = false;

            ObjS3G_ORG_FIRRow.Modified_By = ObjUserInfo.ProUserIdRW;
            ObjS3G_ORG_FIRRow.Modified_On = System.DateTime.Now;

            //response
            ObjS3G_ORG_FIRRow.FIR_No = _FIRNo;
            ObjS3G_ORG_FIRRow.Responded_By = txtRespondedBy.Text.Trim();
            if (Utility.StringToDate(txtResspondedDate.Text) < Utility.StringToDate(txtRequestDate.Text))
            {
                Utility.FunShowAlertMsg(this, "Respond date should be greater than or equal to Requested Date");
                return;
            }
            ObjS3G_ORG_FIRRow.Responded_Date = Utility.StringToDate(txtResspondedDate.Text);
            ObjS3G_ORG_FIRRow.Response_Designation = txtResponseDesgn.Text.Trim();
            ObjS3G_ORG_FIRRow.Field_Officer_EMailID = txtEmailRes.Text.Trim();
            ObjS3G_ORG_FIRRow.Field_Officer_MobileNo = txtMobile.Text.Trim();
            ObjS3G_ORG_FIRRow.Client_Credibility = ddlClientCreditability.SelectedItem.ToString();
            ObjS3G_ORG_FIRRow.Value = txtValue.Text.Trim();
            ObjS3G_ORG_FIRRow.Currency = ddlCurrency.SelectedValue.ToString();//
            ObjS3G_ORG_FIRRow.Client_Net_Worth = ddlClientNetWorth.SelectedItem.ToString();
            ObjS3G_ORG_FIRRow.Response = txtFieldRespond.Text.Trim();
            ObjS3G_ORG_FIRRow.Status = _StatusSource[Convert.ToInt32(ddlStatus.SelectedValue.ToString())];

            ObjS3G_ORG_FIRTable.AddS3G_ORG_FieldInformationReportRow(ObjS3G_ORG_FIRRow);

            //CreditMgtServicesReference.CreditMgtServicesClient objFIR = new CreditMgtServicesReference.CreditMgtServicesClient();
            SerializationMode SMode = SerializationMode.Binary;

            objFIR.FunPubUpdateFIRResponse(SMode, ClsPubSerialize.Serialize(ObjS3G_ORG_FIRTable, SMode));
            Utility.FunShowAlertMsg(this, "Updated Successfully");

            //Added by Thangam M on 18/Oct/2012 to avoid double save click
            btnRespSave.Enabled = false;
            //End here

            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "window.location.href='../Origination/S3GORGTransLander.aspx?Code=FEIR';", true);
        }
        catch (FaultException<CreditMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            // if the field was not saved then revert the responeded status
            ddlStatus.SelectedValue = "0";
            Utility.FunShowAlertMsg(this, objFaultExp.Message);
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
            throw ex;
        }
        finally
        {
            objFIR.Close();
        }

    }



    public void FunClearRespondControls()
    {
        try
        {
            txtRespondedBy.Text =
                txtResponseDesgn.Text =
                txtEmailRes.Text =
                txtMobile.Text =
                txtValue.Text =
                txtFieldRespond.Text = "";
            txtResspondedDate.Text = DateTime.Now.ToString(strDateFormat);
            ddlClientCreditability.SelectedIndex = 0;
            ddlClientNetWorth.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public void FunFillRoundDetails(DataTable DtDetails)
    {
        try
        {
            if (DtDetails.Rows.Count > 0)
            {
                /* --Request Information-- */

                txtFIR.Text = ViewState["FIRNum"].ToString() + "/" + ddlRound.SelectedItem.ToString();
                ddlAgencyCode.SelectedValue = DtDetails.Rows[0]["Agency_Id"].ToString();
                txtAgencyName.Text = DtDetails.Rows[0]["AgencyName"].ToString();
                txtAgencyNAmeAddress.Text = DtDetails.Rows[0]["AgencyAddress"].ToString();
                txtTerms.Text = DtDetails.Rows[0]["Terms_Conditions"].ToString();
                txtFieldRequest.Text = DtDetails.Rows[0]["FieldRequest"].ToString();
                txtRequestBy.Text = DtDetails.Rows[0]["RequestedBy"].ToString();
                txtRequestDate.Text = Convert.ToDateTime(DtDetails.Rows[0]["RequestedDate"].ToString()).ToString(strDateFormat);
                ddlStatus.SelectedValue = GetStatusValue(DtDetails.Rows[0]["FIRStatus"].ToString()).ToString();
                txtSendEmail.Text = DtDetails.Rows[0]["AgencyEmail"].ToString();
                chkCancelled.Checked = Convert.ToBoolean(DtDetails.Rows[0]["Cancelled"].ToString());

                /* --Responded Information-- */

                if (ddlStatus.SelectedItem.ToString().ToUpper() == "FLOATED")  //Floated
                {
                    FunClearRespondControls();
                }
                else //Responded
                {
                    txtRespondedBy.Text = DtDetails.Rows[0]["RespondedBy"].ToString();
                    txtResspondedDate.Text = Convert.ToDateTime(DtDetails.Rows[0]["Responded_Date"].ToString()).ToString(strDateFormat);
                    txtResponseDesgn.Text = DtDetails.Rows[0]["Response_Designation"].ToString();
                    txtEmailRes.Text = DtDetails.Rows[0]["Field_Officer_EMailID"].ToString();
                    txtMobile.Text = DtDetails.Rows[0]["Field_Officer_MobileNo"].ToString();
                    ddlClientCreditability.SelectedIndex = Array.IndexOf(_ClientCreditibility, DtDetails.Rows[0]["Client_Credibility"].ToString());
                    txtValue.Text = DtDetails.Rows[0]["Value"].ToString();
                    if (ddlCurrency != null && ddlCurrency.Items.Count > 0)
                    {
                        ddlCurrency.SelectedValue = DtDetails.Rows[0]["Currency"].ToString();
                        txtCurrency.Text = ddlCurrency.SelectedItem.ToString();
                    }
                    else
                    {
                        FunPriLoadCombo_CurrencyMaster();
                    }
                    ddlClientNetWorth.SelectedIndex = Array.IndexOf(_ClientNetWorth, DtDetails.Rows[0]["Client_Net_Worth"].ToString());
                    txtFieldRespond.Text = DtDetails.Rows[0]["Response"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Cannot fill Round Details");
        }
    }

    protected void ddlRound_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlEnquiryNumber != null && ddlEnquiryNumber.SelectedValue != "0")
            {
                Procparam = new Dictionary<string, string>();

                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@FIR_No", ViewState["FIRNum"].ToString());
                Procparam.Add("@Round", ddlRound.SelectedItem.ToString());

                DataTable DtDetails = new DataTable();
                DtDetails = Utility.GetDefaultData("S3G_ORG_GetFIR_RoundDetails", Procparam);
                if (DtDetails != null)
                {
                    FunFillRoundDetails(DtDetails);
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void DisableAgencyCode()
    {
        try
        {
            if (ddlStatus.SelectedItem.ToString().ToLower() == "responded")
            {
                ddlAgencyCode.Enabled = false;
            }
            else if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                ddlAgencyCode.Enabled = false;
            }
            else
            {
                if (Request.QueryString.Get("qsMode") != null)
                {
                    if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
                    {
                        ddlAgencyCode.Enabled = false;
                    }
                    else
                    {
                        if (string.Compare("M", Request.QueryString.Get("qsMode")) != 0)
                        {
                            if (ddlStatus.SelectedIndex == 0)  // floated
                            {
                                ddlAgencyCode.Enabled = true;
                            }
                            else    // responded.
                            {
                                ddlAgencyCode.Enabled = false;

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnclear_onclick(object sender, EventArgs e)
    {
        try
        {
            if (PageMode == PageModes.Modify)
            {
                if (Convert.ToInt32(ddlAgencyCode.SelectedValue) == ObjUserInfo.ProUserIdRW || ObjUserInfo.ProUserTypeRW.ToUpper().Contains("UTPA"))
                {
                    btnClear.Attributes.Add("OnClick", "javascript:ClearResponse()");
                    txtResspondedDate.Text = DateTime.Now.ToString(strDateFormat);
                }
            }
            if (PageMode != PageModes.Query)
            {               
                    FunPriClearControls();               
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void FunPriClearControls()
    {
        //FunGetDocType();
        FunPriClearTextControls();
        ddlDocType.SelectedIndex = 0;
        if (ddlAgencyType.SelectedIndex > 0)
        {
            ddlAgencyType.ClearSelection();
            ddlAgencyType.Enabled = false;
        }
        if (ddlAgencyCode.SelectedIndex > 0)
        {
            ddlAgencyCode.ClearSelection();
            ddlAgencyCode.Enabled = false;
        }
        //if (ddlEnquiryNumber.Items.Count > 0)
        //{
        // ddlEnquiryNumber.SelectedIndex = 0;
        //ddlEnquiryNumber.ClearDropDownList();
        ddlEnquiryNumber.Clear();

        if (ddlSanum.Visible == true)
        {
            lblSanum.Visible = false;
            ddlSanum.Visible = false;
            ddlSanum.ClearSelection();
        }

        ////}

    }

    private void FunPriClearTextControls()
    {


        //ddlEnquiryNumber.Enabled = false;
        txtRequestDate.Text = System.DateTime.Now.ToString(strDateFormat);
        txtCustomerCode.Text = "";
        txtWebsite.Text = "";
        txtCustNameAddress.Text = "";
        txtTerms.Text = "";
        txtFIR.Text = "";
        txtAgencyNAmeAddress.Text = "";
        txtAgencyName.Text = "";
        txtFieldRequest.Text = "";
        txtSendEmail.Text = "";
        txtCustomerName.Text =
        txtCustomerMobile.Text =
        txtEmailCust.Text = "";
        //ddlAgencyCode.SelectedIndex = 0;
        txtAgencyNAmeAddress.Text = "";
        txtAgencyName.Text = "";
        txtRequestBy.Text = "";
        txtRound.Text = "";
        txtLOB.Text = "";
        txtBranch.Text = "";
        txtRequestDate.Text = DateTime.Now.ToString(strDateFormat);
        S3GCustomerAddress1.ClearCustomerDetails();
        gvPRDDT.Visible = false;
    }

    protected void btnRespClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtResspondedDate.Text = DateTime.Now.ToString(strDateFormat);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }

    protected void hyplnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvPRDDT", strFieldAtt);
            Label lblPath = (Label)gvPRDDT.Rows[gRowIndex].FindControl("lblPath");
            if (lblPath.Text.Trim() != ViewState["Docpath"].ToString().Trim())
            {
                string strFileName = lblPath.Text.Replace("\\", "/").Trim();
                string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "File not to be scanned yet");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvPRDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblColUser = (Label)e.Row.FindControl("lblColUser");
                Label lblScanUser = (Label)e.Row.FindControl("lblScanUser");
                // Label lblCollectedDate = (Label)e.Row.FindControl("lblCollectedDate");
                Label lblPath = (Label)e.Row.FindControl("lblPath");
                //Label lblCollectedby = (Label)e.Row.FindControl("txtColletedBy");
                ImageButton Viewdoct = (ImageButton)e.Row.FindControl("hyplnkView");
                CheckBox Cbx1 = (CheckBox)e.Row.FindControl("CbxCheck");
                TextBox txtUpload = (TextBox)e.Row.FindControl("txOD");
                //Label lblScanneddby = (Label)e.Row.FindControl("txtScannedBy");  
                Label lblCollectedBy = (Label)e.Row.FindControl("lblCollectedBy");
                Label lblType = (Label)e.Row.FindControl("lblType");


                //Added By Palani Kumar.A 21-Sep-2013 to Add Auto Suggestion    
                UserControls_S3GAutoSuggest ddlCollectedby = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlCollectedby");
                AjaxControlToolkit.CalendarExtender calCollectedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calCollectedDate");
                calCollectedDate.Format = calCollectedDate.Format = strDateFormat;
                TextBox txtColletedDate = (TextBox)e.Row.FindControl("txtCollectedDate");

                UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlScannedBy");
                AjaxControlToolkit.CalendarExtender calScannedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calScannedDate");
                calScannedDate.Format = calCollectedDate.Format = strDateFormat;
                TextBox txtScannedDate = (TextBox)e.Row.FindControl("txtScannedDate");

                //DropDownList ddlDocValue = (DropDownList)e.Row.FindControl("ddlDocValue");
                //BindDDL(_DocumentValue, ddlDocValue);

                //Label lblPRTID = (Label)e.Row.FindControl("lblPRTID");
                //int PRTID = Convert.ToInt32(lblPRTID.Text.ToString());
                //if (PRTID >= 54 && PRTID <= 61)
                //{
                //    ddlDocValue.Enabled = false;
                //}
                //else
                //{
                //    ddlDocValue.SelectedItem.Text = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["FIR_Doc_Cat_Type_Desc"].ToString());
                //}
                //if (lblType.Text!="")
                //{
                //   ddlDocValue.SelectedValue = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["FIR_Doc_Cat_ID"].ToString());
                //   ddlDocValue.SelectedItem.Text = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["FIR_Doc_Cat_Type_Desc"].ToString());

                //}

                if (lblColUser.Text != "")
                {
                    ddlCollectedby.SelectedValue = lblColUser.Text;
                    ddlCollectedby.SelectedText = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["CollectedBy"].ToString());
                }
                if (lblScanUser.Text != "")
                {
                    ddlScannedBy.SelectedValue = lblScanUser.Text;
                    ddlScannedBy.SelectedText = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["Scandedby"].ToString());
                }
                txtColletedDate.Attributes.Add("readonly", "readonly");
                txtScannedDate.Attributes.Add("readonly", "readonly");
                //if (lblCollectedDate.Text != "")
                //{
                //    txtColletedDate.Text = lblCollectedDate.Text;
                //}
                if (txtColletedDate.Text.Contains("1900"))
                {
                    txtColletedDate.Text = "";
                }
                if (txtColletedDate.Text != "")
                {
                    txtColletedDate.Text = Convert.ToDateTime(txtColletedDate.Text).ToString(strDateFormat);
                }
                if (txtScannedDate.Text != "")
                {
                    txtScannedDate.Text = Convert.ToDateTime(txtScannedDate.Text).ToString(strDateFormat);
                }

                //if (txtColletedDate.Text.Contains("1900"))
                //{
                //    txtColletedDate.Text = "";
                //    //Viewdoct.Enabled = false;
                //}
                if (FIRTRANS_ID > 0)
                {
                    Viewdoct.Enabled = true;
                }
                else
                {
                    Viewdoct.Enabled = false;
                }

                if (txtScannedDate.Text.Contains("1900"))
                {
                    Cbx1.Checked = false;
                    txtScannedDate.Text = "";
                }
                if (ViewState["Docpath"] != null)
                    txtUpload.Text = ViewState["Docpath"].ToString();

                if (strMode == "M")
                {
                    txtUpload.Text = lblPath.Text;
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

    protected void ddlAgencyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PopulateInspCode(ddlAgencyType.SelectedValue);
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void PopulateInspCode(string strEntityType)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else

                if (ddlAgencyCode.SelectedIndex > 0)
                {
                    txtAgencyName.Text = string.Empty;
                    txtAgencyNAmeAddress.Text = string.Empty;
                    txtSendEmail.Text = string.Empty;
                }

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));

            if (strEntityType.Equals("3")) //Field Investigation Agency
            {
                Procparam.Add("@TypeDesc", "3");
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                Procparam.Add("@Program_ID", "33");
                ddlAgencyCode.BindDataTable("S3g_Org_Get_Entity_ProgramAccess", Procparam, new string[] { "Entity_ID", "Entity_Code" });
            }
            else if (strEntityType.Equals("10"))//Employee
            {
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                Procparam.Add("@Program_ID", "33");
                //ddlAgencyCode.BindDataTable(SPNames.S3G_LOANAD_GetEmployeeInspectorCode, Procparam, new string[] { "User_ID", "User_Code" });
                ddlAgencyCode.BindDataTable("S3G_LOANAD_GetEmpInspecCodeForProgram", Procparam, new string[] { "User_ID", "User_Code" });
            }
            else if (strEntityType.Equals("12"))//Legal consultants
            {
                Procparam.Add("@TypeDesc", "12");
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                Procparam.Add("@Program_ID", "33");
                //ddlAgencyCode.BindDataTable(SPNames.S3G_LOANAD_GetEntity, Procparam, new string[] { "Entity_ID", "Entity_Code" });
                ddlAgencyCode.BindDataTable("S3g_Org_Get_UTPAUsers_ProgramAccess", Procparam, new string[] { "Entity_ID", "Entity_Code" });

            }
            else
            {
                ddlAgencyCode.SelectedIndex = 0;
                if (ddlAgencyCode.Items.Count > 0)
                    ddlAgencyCode.ClearDropDownList();
            }

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void ddlCollectedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Button btn = (Button)sender;
        //int RowIndex = ((GridViewRow)btn.NamingContainer.NamingContainer).RowIndex;


        int RowIndex = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex;
        //((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex
        UserControls_S3GAutoSuggest ddlCollectedBy = gvPRDDT.Rows[RowIndex].FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;
        if (ddlCollectedBy.SelectedValue != "0")
        {

            Label lblCollectedBy = (Label)gvPRDDT.Rows[RowIndex].FindControl("lblCollectedBy");
            lblCollectedBy.Text = ddlCollectedBy.SelectedValue;
        }

    }
    protected void ddlScannedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Button btn = (Button)sender;
        //int RowIndex = ((GridViewRow)btn.NamingContainer.NamingContainer).RowIndex;

        // DropDownList ddlScannedBy = sender as DropDownList;
        int RowIndex = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex;
        //((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex
        UserControls_S3GAutoSuggest ddlScannedBy = gvPRDDT.Rows[RowIndex].FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;

        if (ddlScannedBy.SelectedValue != "0")
        {
            Label lblScannedBy = (Label)gvPRDDT.Rows[RowIndex].FindControl("lblScannedBy");
            lblScannedBy.Text = ddlScannedBy.SelectedValue;
        }
    }
    [System.Web.Services.WebMethod]
    public static string[] GetCollectedby(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_Get_User_List, Procparam));
        return suggestions.ToArray();
    }
    protected void tcRegBranch_ActiveTabChanged(object sender, EventArgs e)
    {
        if (PageMode == PageModes.Create)
        {
            btnClear.Enabled = true;
        }
        if (PageMode == PageModes.Modify)
        {
            if (Convert.ToInt32(ddlAgencyCode.SelectedValue) == ObjUserInfo.ProUserIdRW || ObjUserInfo.ProUserTypeRW.ToUpper().Contains("UTPA"))
            {
                if (tcRegBranch.ActiveTabIndex == 1)
                {
                    btnClear.Enabled = true;
                }
                else
                {
                    btnClear.Enabled = false;
                }
            }
        }
    }


}
