﻿/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// /// <Program Summary>
/// Last Updated By		      :   Thalaiselvam N
/// Last Updated Date         :   03-Sep-2011
/// Reason                    :   Encrypted Password Validation
/// <Program Summary>
/// 
using System;
using System.Web.Security;
using System.Globalization;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using S3GBusEntity;
using System.Configuration;

public partial class Origination_S3GORGPricingApproval : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    int intPricing_ID = 0;
    int intUserID = 0;
    int intRow = 0;
    string strCreate = string.Empty;
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    SerializationMode SerMode = SerializationMode.Binary;
    DataTable dtAction = new DataTable();
    DataSet dsApplicationNumberDetails = new DataSet();
    DataSet Ds = new DataSet();
    string strDateFormat;
    string strPassword;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    PricingMgtServicesReference.PricingMgtServicesClient ObjPricingApproval;
    S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalDataTable ObjPricingApproval_DataTable = new S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalDataTable();
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    string strPageName = "Pricing Approval";
    public static Origination_S3GORGPricingApproval obj_Page;
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        ProgramCode = "031";
        obj_Page = this;
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            intPricing_ID = Convert.ToInt32(fromTicket.Name);
        }
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (!IsPostBack)
        {
            
            btnRevoke.Enabled = false;
            PaymentReqID.Visible = false;
            ViewState["strCreate"] ="0";
            //FunPubloadCombo();
            if (Request.QueryString["qsViewId"] == null)
            {
                chkUnapproval.Checked = true;
                //chkUnapproval_CheckedChanged(null, null);
                FunProLoadLOB();
            }
            //if (blnIsWorkflowApplicable && Session["DocumentNo"] != null)
            //{
                
            //}
            //else
            //{
                if (intPricing_ID > 0)
                {
                    FunPubViewApprovedDetails();
                }
                else
                {
                    btnSave.Enabled = false;
                    btnClear.Enabled = false; 
                    btnRevoke.Enabled = false;
                    btnGo.Enabled = false;
                }

                // WORK FLOW Implementation
                if (PageMode == PageModes.WorkFlow)
                {
                    PreparePageForWFLoad();
                }
            //}
                chkUnapproval.Focus();//Added by Suseela
        }
    }

    private void PreparePageForWFLoad()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

        try
        {

            WorkFlowSession WFSessionValue = new WorkFlowSession();
            //WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();
            byte[] byte_ToDoList = objWorkflowToDoClient.FunPubLoadPricingApproval(WFSessionValue.WorkFlowDocumentNo, int.Parse(CompanyId),WFSessionValue.Document_Type);
            DataSet dsEnquiryAppraisal = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_ToDoList, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsEnquiryAppraisal.Tables.Count > 0)
            {
                if (dsEnquiryAppraisal.Tables[0].Rows.Count > 0)
                {
                    intPricing_ID = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Pricing_Id"]);
                    chkUnapproval.Checked = true;
                    FunPriShowUnApprovedDetails();
                    ddllLineOfBusiness.SelectedValue = Convert.ToString(dsEnquiryAppraisal.Tables[0].Rows[0]["LOB_Id"]);
                    ddlBranch.SelectedValue = Convert.ToString(dsEnquiryAppraisal.Tables[0].Rows[0]["Location_Id"]);
                    FunPriLoadBusinessNo();
                    ddlBusinessOfferNumber.SelectedValue = Convert.ToString(dsEnquiryAppraisal.Tables[0].Rows[0]["Pricing_ID"]);
                    FunPriShowCustomerDetails();
                }
            }
            GoClick();
            if (ddllLineOfBusiness.SelectedIndex > 0) ddllLineOfBusiness.ClearDropDownList();
            //if (ddlBranch.SelectedIndex > 0) ddlBranch.ClearDropDownList();
            if (ddlBranch.SelectedValue != "0") ddlBranch.ReadOnly = true;
            //if (ddlBusinessOfferNumber.SelectedIndex > 0) ddlBusinessOfferNumber.ClearDropDownList();
            if (ddlBusinessOfferNumber.SelectedValue != "0") ddlBusinessOfferNumber.ReadOnly = true;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable Load Pricing page");
        }
        finally
        {
            objWorkflowToDoClient.Close();
        }

    }

    public void FunPubloadCombo()
    {
        ddllLineOfBusiness.Items.Insert(0,"--Select--");
        //ddlBranch.Items.Insert(0,"--Select--");
        //ddlBusinessOfferNumber.Items.Insert(0,"--Select--");
    }
    public void FunPubViewApprovedDetails()
    {
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Pricing_ID", intPricing_ID.ToString());
        dsApplicationNumberDetails = Utility.GetDataset("S3G_ORG_GetPricingOfferNumber", dictDropdownListParam);
        if (dsApplicationNumberDetails.Tables[5].Rows.Count > 0)
        {
            txtStatus.Text = dsApplicationNumberDetails.Tables[5].Rows[0]["ISstatus"].ToString();
            if (!string.IsNullOrEmpty(dsApplicationNumberDetails.Tables[5].Rows[0]["Offer_Date"].ToString())) 
            txtOfferDate.Text = DateTime.Parse(dsApplicationNumberDetails.Tables[5].Rows[0]["Offer_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //txtCustomerName.Text = dsApplicationNumberDetails.Tables[5].Rows[0]["Customer_Name"].ToString();
            //txtCustomerCode.Text = dsApplicationNumberDetails.Tables[5].Rows[0]["Customer_Code"].ToString();
            //txtMobileNo.Text = dsApplicationNumberDetails.Tables[5].Rows[0]["Comm_Mobile"].ToString();
            //txtEmailID.Text = dsApplicationNumberDetails.Tables[5].Rows[0]["Comm_EMail"].ToString();
            //txtWebSite.Text = dsApplicationNumberDetails.Tables[5].Rows[0]["Comm_Website"].ToString();
            //txtCustomerAddress1.Text = Utility.SetCustomerAddress(dsApplicationNumberDetails.Tables[5].Rows[0]);
            S3GCustomerPermAddress.SetCustomerDetails(dsApplicationNumberDetails.Tables[5].Rows[0], true);


            //ddlBusinessOfferNumber.FillDataTable(dsApplicationNumberDetails.Tables[5], "Pricing_ID", "Business_Offer_Number");
            ddlBusinessOfferNumber.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[0]["Pricing_ID"].ToString();
            ddlBusinessOfferNumber.SelectedText = dsApplicationNumberDetails.Tables[5].Rows[0]["Business_Offer_Number"].ToString();
            ddlBusinessOfferNumber.Enabled = false;

            ddllLineOfBusiness.FillDataTable(dsApplicationNumberDetails.Tables[5], "LOB_ID", "LOB_Name");
            ddllLineOfBusiness.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[0]["LOB_ID"].ToString();
            ddllLineOfBusiness.Enabled = false;

            //ddlBranch.FillDataTable(dsApplicationNumberDetails.Tables[5], "Location_ID", "Location");
            ddlBranch.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[0]["Location_ID"].ToString();
            ddlBranch.SelectedText = dsApplicationNumberDetails.Tables[5].Rows[0]["Location"].ToString();
            ddlBranch.Enabled = false;

            txtStatus.ReadOnly = true;
            txtOfferDate.ReadOnly = true;
            txtOfferDate.ReadOnly = true;
            //txtCustomerName.ReadOnly = true;
            //txtCustomerCode.ReadOnly = true;
            //txtCustomerAddress1.ReadOnly = true;

            chkApproved.Enabled = false;
            chkUnapproval.Enabled = false;


            PaymentReqID.Visible = true;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlBusinessOfferNumber.SelectedValue.ToString(), false, 0);
            hdnID.Value = "../Origination/S3gOrgPricing_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

            grvApprovalDetails.DataSource = dsApplicationNumberDetails.Tables[5];
            grvApprovalDetails.DataBind();
        }
    }
    #endregion

    #region Button (Save / Clear / Cancel)

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (chkUnapproval.Checked == true || chkApproved.Checked == true)
        {
            Label lblApprovalSNO = new Label();
            Label lblPricingApproval_ID = new Label();
            TextBox txtRemarks = new TextBox();
            TextBox txtPassword = new TextBox();
            DropDownList ddlAction = new DropDownList();
            foreach (GridViewRow grv in grvApprovalDetails.Rows)
            {
                lblApprovalSNO = (Label)grv.FindControl("lblApprovalSNO");
                lblPricingApproval_ID = (Label)grv.FindControl("lblPricingApproval_ID");
                ddlAction = (DropDownList)grv.FindControl("ddlAction");
                txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                txtPassword = (TextBox)grv.FindControl("txtPassword");
            }
            S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalRow ObjRow;

            ObjRow = ObjPricingApproval_DataTable.NewS3G_ORG_PricingApprovalRow();
            ObjRow.Pricing_ID = Convert.ToInt32(ddlBusinessOfferNumber.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(lblPricingApproval_ID.Text))
                ObjRow.PricingApproval_ID = Convert.ToInt32(lblPricingApproval_ID.Text);
            else
                ObjRow.PricingApproval_ID = 0;
            if (!string.IsNullOrEmpty(lblApprovalSNO.Text.Trim()))
                ObjRow.Approval_Serial_Number = Convert.ToInt32(lblApprovalSNO.Text.Trim());
            if (ddlAction.SelectedIndex > 0)
                ObjRow.Action_ID = Convert.ToInt32(ddlAction.SelectedValue.ToString());
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Status');", true);
                return;
            }
            ObjRow.Approver_ID = intUserID;
            if (txtPassword.Text.Trim() != "")
                ObjRow.Password = txtPassword.Text.Trim();
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Password.');", true);
                return;
            }

            S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
            if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                return;
            }

            if (txtRemarks.Text.Trim() != "")
                ObjRow.Remarks = txtRemarks.Text.Trim();
            ObjRow.Company_ID = intCompanyID;
            ObjRow.Created_By = intUserID;
            ObjPricingApproval_DataTable.AddS3G_ORG_PricingApprovalRow(ObjRow);
            ObjPricingApproval = new PricingMgtServicesReference.PricingMgtServicesClient();
            int intNoOfApproval = 0;
            int intApprovedCount = 0;
            int intErrorCode = ObjPricingApproval.FunPubCreatePricingApproval(out intNoOfApproval, out intApprovedCount, SerMode, ClsPubSerialize.Serialize(ObjPricingApproval_DataTable, SerMode));
            if (intErrorCode == 0)
            {
                string strAlert = "";
                if (isWorkFlowTraveler)
                {
                    if (intNoOfApproval > 0 && intApprovedCount > 0 && intNoOfApproval == intApprovedCount)
                    {
                        
                        WorkFlowSession WFValues = new WorkFlowSession();
                        if (ddlAction.SelectedItem.Text == "Approved")
                        {
                            try
                            {
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId,2);

                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here

                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is Not Assigned"; // Closing the status...
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                            }
                        }
                        else if (ddlAction.SelectedItem.Text == "Rejected" || ddlAction.SelectedItem.Text == "Cancelled")       // In case of Approval Rejected or cancelled - By Rao 27 Jan 2012.
                        {
                            try
                            {
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());

                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here

                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is not assigned";
                            }
                        }
                        ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId.ToString(), strAlert);
                        return;

                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing Approval done successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0';", true);
                }
                else
                {
                    if (intNoOfApproval > 0 && intApprovedCount > 0 && intNoOfApproval == intApprovedCount)
                    {
                        DataTable WFFP = new DataTable();
                        // By Palani Kumar.A for Customer Name remove after Concatenation
                        string strBusinessNo = string.Empty;
                        if (!string.IsNullOrEmpty(ddlBusinessOfferNumber.SelectedText.ToString()))
                        {
                            strBusinessNo = ddlBusinessOfferNumber.SelectedText.Substring(0, ddlBusinessOfferNumber.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();                          
                        }
                        else
                        {
                            strBusinessNo = ddlBusinessOfferNumber.SelectedText.ToString();
                        }
                        
                        if (CheckForForcePullOperation(null, strBusinessNo, ProgramCode, null, null, "O", CompanyId, null, int.Parse(hProductID.Value), ddllLineOfBusiness.SelectedItem.Text, null, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            if (ddlAction.SelectedItem.Text == "Approved")
                            {
                                try
                                {
                                    int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), strBusinessNo, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), 2);

                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here
                                    strAlert = "";
                                }
                                catch (Exception Ex)
                                {
                                    strAlert = "Work Flow is Not Assigned"; // Closing the Status.
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());

                                }
                            }
                            else if (ddlAction.SelectedItem.Text == "Rejected" || ddlAction.SelectedItem.Text == "Cancelled")      // In case of Approval Rejected or cancelled - By Rao 27 Jan 2012.
                            {
                                try
                                {
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());

                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here

                                    strAlert = "";
                                }
                                catch (Exception ex)
                                {
                                    strAlert = "Work Flow is not assigned";
                                }
                            }
                        }
                    }

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing Approval done successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0';", true);
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    if (ddlAction.SelectedValue == "47")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing Approved successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0';", true);
                    }
                    else if (ddlAction.SelectedValue == "120")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing Cancelled successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0';", true);
                    }
                    else if (ddlAction.SelectedValue == "51")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing Rejected successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing held successfully.');window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0';", true);
                    }
                }

                return;
            }
            else if (intErrorCode == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                return;
            }
            else if (intErrorCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Authorization Rule Card not defined for this combination');", true);
                return;
            }
            else if (intErrorCode == 3)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Approval should be in sequence as defined in Rule Card');", true);
                return;
            }
            else if (intErrorCode == 4)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Approval Already done for this business offer number');", true);
                return;
            }
            else if (intErrorCode == 5)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Approver not having access to approve the Pricing.');", true);
                return;
            }
            else if (intErrorCode == 15)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert12", "alert('" + Resources.LocalizationResources.Level4andAboveApproval + "');", true);
                return;
            }
            else if (intErrorCode == 16)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert13", "alert('3 and 5 level users can only approve.');", true);
                return;
            }
            else if (intErrorCode == 20)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Pricing Approval Details Approved Failed.');", true);
                return;
            }
        }
        btnSave.Focus();//Added by Suseela
    }
    protected void PaymentReqID_serverclick(object sender, EventArgs e)
    {
        if (hdnID.Value.Length > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
            return;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select business offer number.');", true);
            return;
        }
        //Response.Write(hdnID.Value.ToString());
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        FunPubClear();
        //ddllLineOfBusiness.Items.Clear();
        ddllLineOfBusiness.SelectedValue = "0";
        //ddlBranch.Items.Clear();
        ddlBranch.Clear();
        //ddlBusinessOfferNumber.Items.Clear();
        ddlBusinessOfferNumber.Clear();
        chkApproved.Checked = false;
        chkUnapproval.Checked = false;
        btnGo.Enabled = false;
        FunPubloadCombo();
        btnClear.Focus();//Added by Suseela
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
          //wf Cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();            
        else
        Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0");
        btnCancel.Focus();//Added by Suseela
    }
    #endregion

    #region Control Chaanged Events

    protected void FunProLoadLOB()
    {
        Dictionary<string, string> dictDropdownListParam;
        dictDropdownListParam = new Dictionary<string, string>();
        uinfo = new UserInfo();
        dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        dictDropdownListParam.Add("@Is_Active", "1");
        dictDropdownListParam.Add("@Program_ID", "31");
        ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
    }

    protected void chkUnapproval_CheckedChanged(object sender, EventArgs e)
    {
        if (chkUnapproval.Checked)
        {

            FunPubClear();
            //ddllLineOfBusiness.Items.Clear();
            //ddllLineOfBusiness.SelectedValue = "0";
            //ddlBusinessOfferNumber.Items.Clear();
            //ddlBranch.Items.Clear();
            ddlBranch.Clear();
            //UserInfo uinfo = null;
            //Dictionary<string, string> dictDropdownListParam;
            //dictDropdownListParam = new Dictionary<string, string>();
            //uinfo = new UserInfo();
            //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@Is_Active", "1");
            //dictDropdownListParam.Add("@Program_ID", "31");
            //ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
            //ddlBusinessOfferNumber.Items.Clear();
            //ddlBusinessOfferNumber.Items.Insert(0, "--Select--");
            ddlBusinessOfferNumber.Clear();
            uinfo = null;
            dictDropdownListParam = null;
            
        }
        if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
        {
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            btnClear.Enabled = false;
            PaymentReqID.Visible = false;
            btnGo.Enabled = false;
            FunPubClear();


            //ddllLineOfBusiness.Items.Clear();
            //ddllLineOfBusiness.SelectedValue = "0";
            //ddlBranch.Items.Clear();
            ddlBranch.Clear();
            //ddlBusinessOfferNumber.Items.Clear();
            ddlBusinessOfferNumber.Clear();
            FunPubloadCombo();
 
        }
        chkUnapproval.Focus();//Added by Suseela
    }
    protected void chkApproved_CheckedChanged(object sender, EventArgs e)
    {
        if (chkApproved.Checked)
        {
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            btnClear.Enabled = false;


            FunPubClear();
            //ddllLineOfBusiness.Items.Clear();
            //ddllLineOfBusiness.SelectedValue = "0";
            //ddlBusinessOfferNumber.Items.Clear();
            //ddlBranch.Items.Clear();
            ddlBranch.Clear();
           
            //UserInfo uinfo = null;
            //Dictionary<string, string> dictDropdownListParam;
            //dictDropdownListParam = new Dictionary<string, string>();
            //uinfo = new UserInfo();
            //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
            //dictDropdownListParam.Add("@Is_Active", "1");
            //dictDropdownListParam.Add("@Program_ID", "31");
            //ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //ddlBusinessOfferNumber.Items.Clear();
            //ddlBusinessOfferNumber.Items.Insert(0, "--Select--");
            ddlBusinessOfferNumber.Clear();
            uinfo = null;
            dictDropdownListParam = null;
        }
        else
        {
            if ((!chkApproved.Checked) && (!chkUnapproval.Checked))
            {
                //ddllLineOfBusiness.Items.Clear();
                //ddlBranch.Items.Clear();
                ddlBranch.Clear();
                //ddlBusinessOfferNumber.Items.Clear();
                ddlBusinessOfferNumber.Clear();
                FunPubClear();
                FunPubloadCombo();
            }
        }
        chkApproved.Focus();//Added by Suseela
    }
    private void FunPriShowUnApprovedDetails()
    {
        FunPubClear();
        //ddllLineOfBusiness.Items.Clear();
        ddllLineOfBusiness.SelectedValue = "0";
        //ddlBusinessOfferNumber.Items.Clear();
        ddlBusinessOfferNumber.Clear();
        //ddlBranch.Items.Clear();
        ddlBranch.Clear();

        //UserInfo uinfo = null;
        //Dictionary<string, string> dictDropdownListParam;
        //dictDropdownListParam = new Dictionary<string, string>();
        //uinfo = new UserInfo();
        //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //dictDropdownListParam.Add("@Is_Active", "1");
        //dictDropdownListParam.Add("@Program_ID", "31");
        //ddllLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictDropdownListParam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_CODE", "Location_Name" });
        //uinfo = null;
        //dictDropdownListParam = null;
    }

    private void FunPriLoadBusinessNo()
    {
        FunPubClear();
        //ddlBusinessOfferNumber.Items.Clear();
        ddlBusinessOfferNumber.Clear();
        //uinfo = new UserInfo();
        //dictDropdownListParam = new Dictionary<string, string>();
        //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
        //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
        //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
        //ddlBusinessOfferNumber.BindDataTable("S3G_ORG_GetPricingOfferNumber ", dictDropdownListParam, new string[] { "Pricing_ID", "Business_Offer_Number" });
        //dictDropdownListParam = null;

    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "31");
        Procparam.Add("@Lob_Id", obj_Page.ddllLineOfBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    protected void ddllLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlBranch.Items.Clear();
        //ddlBranch.Items.Insert(0, "--Select--");
        ddlBranch.Clear();
        //ddlBusinessOfferNumber.Items.Clear();
        //ddlBusinessOfferNumber.Items.Insert(0, "--Select--");
        ddlBusinessOfferNumber.Clear();
        FunPubClear();

        //Dictionary<string, string> dictDropdownListParam;
        //dictDropdownListParam = new Dictionary<string, string>();
        //uinfo = new UserInfo();
        //dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //dictDropdownListParam.Add("@User_Id", Convert.ToString(intUserID));
        //dictDropdownListParam.Add("@Is_Active", "1");
        //dictDropdownListParam.Add("@Program_ID", "31");
        //if (ddllLineOfBusiness.SelectedValue != "0")
        //{
        //    dictDropdownListParam.Add("@Lob_Id", ddllLineOfBusiness.SelectedValue.ToString());
        //}
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictDropdownListParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });


        //if (ddlBranch.SelectedIndex > 0)
        if (ddlBranch.SelectedValue != "0")
        {
            if (chkUnapproval.Checked)
            {
                FunPubClear();
                //ddlBusinessOfferNumber.Items.Clear();
                ddlBusinessOfferNumber.Clear();
                //uinfo = new UserInfo();
                //dictDropdownListParam = new Dictionary<string, string>();
                //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                //dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                //ddlBusinessOfferNumber.BindDataTable("S3G_ORG_GetPricingOfferNumber ", dictDropdownListParam, new string[] { "Pricing_ID", "Business_Offer_Number" });
                //dictDropdownListParam = null;
                //if (ddlBusinessOfferNumber.Items.Count == 0)
                //    ddlBusinessOfferNumber.Items.Insert(0, "--Select--");
            }
            else
            {
                FunPubClear();
                //ddlBusinessOfferNumber.Items.Clear();
                ddlBusinessOfferNumber.Clear();
                //uinfo = new UserInfo();
                //dictDropdownListParam = new Dictionary<string, string>();
                //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                //dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                //Ds = Utility.GetDataset("S3G_ORG_GetPricingOfferNumber ", dictDropdownListParam);
                //if (Ds.Tables[7].Rows.Count > 0)
                //{
                //    ddlBusinessOfferNumber.FillDataTable(Ds.Tables[7], "Pricing_ID", "Business_Offer_Number");
                   
                //}
                //if (ddlBusinessOfferNumber.Items.Count == 0)
                //{
                //    ddlBusinessOfferNumber.Items.Insert(0, "--Select--");
                //}
              
            }
        }
        if (ddllLineOfBusiness.SelectedIndex == 0)
        {
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnRevoke.Enabled = false;
        }
        ddllLineOfBusiness.Focus();//Added by Suseela
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetPricingNumbers(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@LOB_ID", obj_Page.ddllLineOfBusiness.SelectedValue.ToString());
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        if (obj_Page.chkUnapproval.Checked)
        {
            Procparam.Add("@Approved", "1");
        }
        else
        {
            Procparam.Add("@Approved", "2");
        }
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetPricingOfferNumber_AGT", Procparam));

        return suggetions.ToArray();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddllLineOfBusiness.SelectedIndex > 0)
        {
            if (chkUnapproval.Checked)
            {
                FunPubClear();
                //ddlBusinessOfferNumber.Items.Clear();
                ddlBusinessOfferNumber.Clear();
                //uinfo = new UserInfo();
                //dictDropdownListParam = new Dictionary<string, string>();
                //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                //dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                //ddlBusinessOfferNumber.BindDataTable("S3G_ORG_GetPricingOfferNumber ", dictDropdownListParam, new string[] { "Pricing_ID", "Business_Offer_Number" });
                //dictDropdownListParam = null;
                //if (ddlBusinessOfferNumber.Items.Count == 0)
                //{
                //    ddlBusinessOfferNumber.Items.Insert(0, "--Select--");
                //}
            }
            else
            {
                FunPubClear();
                //ddlBusinessOfferNumber.Items.Clear();
                ddlBusinessOfferNumber.Clear();
                //uinfo = new UserInfo();
                //dictDropdownListParam = new Dictionary<string, string>();
                //dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                //dictDropdownListParam.Add("@LOB_ID", ddllLineOfBusiness.SelectedValue.ToString());
                //dictDropdownListParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
                //dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
                //Ds = Utility.GetDataset("S3G_ORG_GetPricingOfferNumber ", dictDropdownListParam);
                //if (Ds.Tables[7].Rows.Count > 0)
                //{
                //    ddlBusinessOfferNumber.FillDataTable(Ds.Tables[7], "Pricing_ID", "Business_Offer_Number");
                   
                //}
                //if (ddlBusinessOfferNumber.Items.Count == 0)
                //{
                //    ddlBusinessOfferNumber.Items.Insert(0, "--Select--");
                //}
               
            }
        }
        ddlBranch.Focus();//Added by Suseela
        if (ddlBranch.SelectedValue == "0")
        {
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            btnClear.Enabled = false;
        }
        ddlBranch.Focus();//Added by Suseela
    }
    protected void ddlBusinessOfferNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        //if (ddlBusinessOfferNumber.SelectedIndex > 0)
        if (ddlBusinessOfferNumber.SelectedValue != "0")
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            btnClear.Enabled = false;
            FunPriShowCustomerDetails();
            ddlBusinessOfferNumber.ToolTip = ddlBusinessOfferNumber.SelectedText.ToString();
        }
        else
        {
            FunPubClear();
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;
            btnClear.Enabled = false;

        }
        ddlBusinessOfferNumber.Focus();//Added by Suseela
    }
    private void FunPriShowCustomerDetails()
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@User_ID", intUserID.ToString());
            dictDropdownListParam.Add("@Pricing_ID", ddlBusinessOfferNumber.SelectedValue.ToString());
            dsApplicationNumberDetails = Utility.GetDataset("S3G_ORG_GetPricingOfferNumber ", dictDropdownListParam);
            if (dsApplicationNumberDetails.Tables[1].Rows.Count > 0)
            {
                txtStatus.Text = dsApplicationNumberDetails.Tables[1].Rows[0]["name"].ToString();
                if(!string.IsNullOrEmpty(dsApplicationNumberDetails.Tables[1].Rows[0]["Offer_Date"].ToString()))
                txtOfferDate.Text = DateTime.Parse(dsApplicationNumberDetails.Tables[1].Rows[0]["Offer_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                hProductID.Value = dsApplicationNumberDetails.Tables[1].Rows[0]["Product_Id"].ToString();
                if (dsApplicationNumberDetails.Tables[1].Rows[0]["IsCreate"].ToString() == "1")
                {
                    btnGo.Enabled = false;
                    ViewState["strCreate"]="1";
                }
                else
                {
                    btnGo.Enabled = true;
                }

               
                S3GCustomerPermAddress.SetCustomerDetails(dsApplicationNumberDetails.Tables[1].Rows[0], true);

                txtStatus.ReadOnly = true;
                txtOfferDate.ReadOnly = true;
                txtOfferDate.ReadOnly = true;
           
                PaymentReqID.Visible = true;
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlBusinessOfferNumber.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../Origination/S3gOrgPricing_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";
                btnGo.Enabled = true;

            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        GoClick();
        btnGo.Focus();//Added by Suseela
    }
    void GoClick()
    {
        //if (ddlBusinessOfferNumber.SelectedIndex > 0)
        if (ddlBusinessOfferNumber.SelectedValue != "0")
        {
            uinfo = new UserInfo();
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@User_ID", uinfo.ProUserIdRW.ToString());
            dictDropdownListParam.Add("@Pricing_ID", ddlBusinessOfferNumber.SelectedValue.ToString());
            dsApplicationNumberDetails = Utility.GetDataset("S3G_ORG_GetPricingOfferNumber ", dictDropdownListParam);


            btnSave.Enabled = true;
            //btnRevoke.Enabled = true;
            btnClear.Enabled = true;
            btnClear.Enabled = true;

            if (dsApplicationNumberDetails.Tables[3].Rows.Count > 0)
            {
                grvApprovalDetails.DataSource = dsApplicationNumberDetails.Tables[3];
                grvApprovalDetails.DataBind();
                btnGo.Enabled = false;
            }

        }
    }
    protected void grvApprovalDetails_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            grvApprovalDetails.Columns[3].Visible = true;
            TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
            DropDownList ddlAction = (DropDownList)e.Row.FindControl("ddlAction");
            ddlAction.FillDataTable(dsApplicationNumberDetails.Tables[2], "LookUp_ID", "Name");
            if (intPricing_ID > 0)
            {
                if (dsApplicationNumberDetails.Tables[5].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtStatus.Text))
                    {
                        if (txtStatus.Text.ToUpper().Trim() == "APPROVED" || txtStatus.Text.ToUpper().Trim() == "PENDING")
                        {
                            grvApprovalDetails.Columns[3].Visible = false;
                            ddlAction.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[intRow]["Action_ID"].ToString();
                            txtRemarks.Enabled = false;
                            ddlAction.Enabled = false;
                            btnGo.Enabled = false;
                            btnSave.Enabled = false;
                            btnClear.Enabled = false;
                           
                        }

                    }
                }
            }
            if (intPricing_ID == 0 || txtStatus.Text.ToUpper().Trim() == "APPROVED" || txtStatus.Text.ToUpper().Trim() == "REJECTED")
            {
                if (dsApplicationNumberDetails.Tables[5].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtStatus.Text))
                    {
                        if (txtStatus.Text.ToUpper().Trim() == "APPROVED" || txtStatus.Text.ToUpper().Trim() == "REJECTED")
                        {
                            grvApprovalDetails.Columns[3].Visible = false;
                            ddlAction.SelectedValue = dsApplicationNumberDetails.Tables[5].Rows[intRow]["Action_ID"].ToString();
                            txtRemarks.Enabled = false;
                            ddlAction.Enabled = false;
                            btnSave.Enabled = false;
                            btnClear.Enabled = false;
                            btnGo.Enabled = false;
                        }

                    }
                }
            }
            if (dsApplicationNumberDetails.Tables[3].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtStatus.Text))
                {
                    
                    if (dsApplicationNumberDetails.Tables[3].Rows[intRow]["Action_ID"].ToString().Trim() !="")
                    {
                        ddlAction.SelectedValue = dsApplicationNumberDetails.Tables[3].Rows[intRow]["Action_ID"].ToString();
                        if (dsApplicationNumberDetails.Tables[3].Rows[intRow]["Action_ID"].ToString().Trim() != "49" && Request.QueryString["qsViewId"] == null)
                        {
                            ddlAction.Enabled = false;
                            btnSave.Enabled = false;
                            btnRevoke.Enabled = true;
                            txtRemarks.Enabled = false;
                        }
                        else
                        {
                            if (Request.QueryString["qsMode"].Trim() =="C")
                            {
                                ddlAction.Enabled = true;
                                btnSave.Enabled = true;
                                btnRevoke.Enabled = false;
                            }
                        }


                    }
                    else
                    {
                        if(intPricing_ID==0)
                        ddlAction.SelectedValue = "0";
                    }
                    if (txtStatus.Text.ToUpper().Trim() == "APPROVED" && ddlAction.SelectedValue.Trim() == "0")
                    {
                        DataTable dttemp = new DataTable();
                        dttemp = null;
                        grvApprovalDetails.DataSource = dttemp;
                        grvApprovalDetails.DataBind();
                        btnSave.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alerts", "alert('This Pricing Offer Number already approved by another user');", true);
                        return;
                    }
                    if (txtStatus.Text.ToUpper().Trim() == "REJECTED" && ViewState["strCreate"].ToString()=="1")
                    {
                        if (txtStatus.Text.ToUpper().Trim() == "REJECTED" && ddlAction.SelectedValue.Trim() == "0")
                        {
                            Label lbldate2 = (Label)e.Row.FindControl("lblApprovalDate");
                            if (lbldate2.Text.Trim() != string.Empty)
                            {
                                DateTime Date = Convert.ToDateTime(lbldate2.Text);
                                lbldate2.Text = Date.ToString(strDateFormat);
                            }
                            btnSave.Enabled = true;
                            return;
                        }
                    }
                    if (txtStatus.Text.ToUpper().Trim() == "REJECTED" && ddlAction.SelectedValue.Trim() == "0")
                    {
                        DataTable dttemp = new DataTable();
                        dttemp = null;
                        grvApprovalDetails.DataSource = dttemp;
                        grvApprovalDetails.DataBind();
                        btnSave.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alerts", "alert('This Pricing Offer Number already rejected by another user');", true);
                        return;
                    }

                }
            }
            if (Request.QueryString["qsMode"].Trim() == "Q")
            {
                txtRemarks.Enabled = true;
                txtRemarks.ReadOnly = true;
            }
            intRow++;
            Label lbldate = (Label)e.Row.FindControl("lblApprovalDate");
            if (lbldate.Text.Trim() != string.Empty)
            {
                DateTime Date = Convert.ToDateTime(lbldate.Text);
                lbldate.Text = Date.ToString(strDateFormat);
            }
        }
    }
    public void FunPubClear()
    {
        grvApprovalDetails.DataSource = dsApplicationNumberDetails.Tables.Add();  //make empty grid
        grvApprovalDetails.DataBind();

        PaymentReqID.Visible = false;
        btnSave.Enabled = false;
        btnRevoke.Enabled = false;
        btnClear.Enabled = false;
        txtStatus.Text = string.Empty;
        txtOfferDate.Text = string.Empty;
        S3GCustomerPermAddress.ClearCustomerDetails();
    }
    #endregion
    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjApplicationApproval = null;
        ObjApplicationApproval = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();

        try
        {

            //if (ddlBusinessOfferNumber.SelectedIndex > 0)
            if (ddlBusinessOfferNumber.SelectedValue != "0")
            {
                TextBox txtPassword = new TextBox();
                foreach (GridViewRow grv in grvApprovalDetails.Rows)
                {
                    txtPassword = (TextBox)grv.FindControl("txtPassword");
                }
                if (txtPassword.Text.Trim() != "")
                {
                    strPassword = txtPassword.Text.Trim();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter Password.');", true);
                    return;
                }

                S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
                if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                    return;
                }

                //ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjApplicationApproval = null;
                ObjApplicationApproval = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
                int intResult = ObjApplicationApproval.FunPubRevokeOrUpdateApprovedDetails(1, Convert.ToInt32(ddlBusinessOfferNumber.SelectedValue), intUserID, strPassword.Trim());
                if (intResult == 0)
                {
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnRevoke.Enabled = false;
                    //End here

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Business offer number revoked successfully');window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRAP&Modify=0';", true);
                    return;
                }
                else if (intResult == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Business offer number mapped to the transaction. can not revoke this Business offer number');", true);
                    return;
                }
                else if (intResult == 2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Password.');", true);
                    return;
                }
                else if (intResult == 7)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('No access rights for this User.');", true);
                    return;
                }
                else if (intResult == 8)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Next level approval has been completed, cannot revoke');", true);
                    return;
                }

                else if (intResult == 13)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Credit Guide Transaction has been made using this Pricing Number, cannot revoke');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Please select Business offer number');", true);
                return;
            }
            btnRevoke.Focus();//Added by Suseela
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjApplicationApproval.Close();
        }

    }
}

